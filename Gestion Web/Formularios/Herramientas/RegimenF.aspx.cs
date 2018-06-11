using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class RegimenF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        controladorReportes contReportes = new controladorReportes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                lbtnGenerar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(lbtnGenerar, null) + ";");
                if (!IsPostBack)
                {
                    DateTime today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    this.txtFechaDesde.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1).ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, today.AddDays(-1).Day).ToString("dd/MM/yyyy");

                    this.cargarEmpresas();                    
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina. " + ex.Message));
            }
        }

        #region Cargas Iniciales
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = "-1";
                dt.Rows.InsertAt(dr, 0);

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

                this.ListSucursal.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListSucursal.Items.Insert(1, new ListItem("Todas", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                this.ListPuntoVta.DataSource = dt;
                this.ListPuntoVta.DataValueField = "Id";
                this.ListPuntoVta.DataTextField = "NombreFantasia";
                this.ListPuntoVta.DataBind();

                this.ListPuntoVta.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListPuntoVta.Items.Insert(1, new ListItem("Todas", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pto ventas. " + ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListEmpresa.SelectedValue != "-1" && this.ListSucursal.SelectedValue != "-1" && this.ListPuntoVta.SelectedValue != "-1")
                {
                    this.generarArchivos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar empresa. "));
                }
            }
            catch
            {

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                if (id > 0)
                    this.cargarSucursalByEmpresa(id);
            }
            catch
            {

            }
        }
        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                if (id > 0)
                    this.cargarPuntoVta(id);
            }
            catch
            {

            }
        }
        protected void lbtnSolicitarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorInformesEntity contInfEnt = new ControladorInformesEntity();
                Informes_Pedidos ip = new Informes_Pedidos();
                InformeXML infXML = new InformeXML();

                //Cargo el objeto Informes_Pedidos
                cargarDatosInformePedido(ip);

                //Cargo el objeto InformeXML
                cargarDatosInformeXML(infXML);

                if (ip == null || infXML == null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando datos del Informe "));
                    return;
                }

                //Mando a grabar el pedido de informe, y genero el XML. Si todo es correcto retorna 1. En caso contrario, revisar error segun el entero.
                int i = contInfEnt.generarPedidoDeInforme(infXML,ip);

                if (i > 0)
                   ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se ha generado la solicitud de informe! ", "/Formularios/Reportes/InformesF.aspx?us=" + ip.Usuario.ToString()));
                if(i == -1)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error grabando el pedido de informe! "));
                if(i == -2)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error generando el Informe XML! "));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error generando la solicitud de informe! " + Ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
                else
                {
                    if (this.verificarAcceso() != 1)
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Herramientas.Presupuesto") != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }
        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "68")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        public void cargarDatosInformeXML(InformeXML infXML)
        {
            DateTime fechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
            DateTime fechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR"));

            //Si seleccionó los elementos de los combos, cargo la clase InformeXML
            if (this.ListEmpresa.SelectedValue != "-1" && this.ListSucursal.SelectedValue != "-1" && this.ListPuntoVta.SelectedValue != "-1")
            {
                infXML.Empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                infXML.Sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                infXML.PuntoVenta = Convert.ToInt32(this.ListPuntoVta.SelectedValue);
                infXML.FechaDesde = fechaDesde.ToString("dd/MM/yyyy");
                infXML.FechaHasta = fechaHasta.ToString("dd/MM/yyyy");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar los filtros. "));
            }
        }
        public void cargarDatosInformePedido(Informes_Pedidos ip)
        {
            DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
            ip.Fecha = DateTime.Now;
            ip.Informe = 1;
            ip.Usuario = (int)Session["Login_IdUser"];
            ip.Estado = 0;
            ip.NombreInforme = "REGIMEN-" + fechaD.ToString("MMyyyy");
            
        }
        private void generarArchivos()
        {
            try
            {
                //this.lblProcesando.Visible = true;
                string desde = this.txtFechaDesde.Text;
                string hasta = this.txtFechaHasta.Text;
                int emp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int pv = Convert.ToInt32(this.ListPuntoVta.SelectedValue);

                string rutaTxt = Server.MapPath("../Herramientas/" + DateTime.Today.Month + "/");

                if (!Directory.Exists(rutaTxt))
                {
                    Directory.CreateDirectory(rutaTxt);
                }
                else
                {
                    Directory.Delete(rutaTxt, true);
                    Directory.CreateDirectory(rutaTxt);
                }

                string archivos = this.contReportes.generarRegimenInformativo(desde, hasta, rutaTxt, emp, suc, pv);

                System.IO.FileStream fs = null;
                fs = System.IO.File.Open(rutaTxt + archivos, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs.Length];
                fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/octet-stream";
                //this.Response.AddHeader("content-length", comprobante.Length.ToString());
                this.Response.AddHeader("Content-disposition", "attachment; filename= " + archivos);
                this.Response.BinaryWrite(btFile);
                this.Response.End();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}