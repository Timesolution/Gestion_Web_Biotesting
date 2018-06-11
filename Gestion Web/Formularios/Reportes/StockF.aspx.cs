using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Gestion_Api.Entitys;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class StockF : System.Web.UI.Page
    {
        controladorSucursal contSucursal = new controladorSucursal();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();
        ControladorInformesEntity contInformesEnt = new ControladorInformesEntity();
        controladorReportes contReportes = new controladorReportes();
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                lbtnSolicitarInforme.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(lbtnSolicitarInforme, null) + ";");

                if (!IsPostBack)
                {
                    DateTime today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    //this.txtFechaDesde.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1).ToString("dd/MM/yyyy");
                    //this.txtFechaHasta.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, today.AddDays(-1).Day).ToString("dd/MM/yyyy");

                    this.cargarEmpresas();
                    this.cargarSucursales();
                }
            }
            catch (Exception Ex)
            {

            }
        }

        #region Eventos Controles
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //this.cargarSucursales();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Sucursales de la Empresa seleccionada. Excepción: " + Ex.Message));
            }
        }
        protected void lbtnSolicitarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListEmpresa.SelectedValue != "-1")
                {
                    Informes_Pedidos ip = new Informes_Pedidos();
                    InformeXML infXML = new InformeXML();

                    //Cargo el objeto Informes_Pedidos
                    this.cargarDatosInformePedido(ip);

                    //Cargo el objeto InformeXML
                    this.cargarDatosInformeXML(infXML);

                    //Mando a grabar el pedido de informe, y genero el XML. Si todo es correcto retorna 1. En caso contrario, revisar error segun el entero.
                    int i = this.contInformesEnt.generarPedidoDeInforme(infXML, ip);

                    if (i > 0)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se ha generado la solicitud de informe! ", "/Formularios/Reportes/InformesF.aspx?us=" + ip.Usuario.ToString()));
                    if (i == -1)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error grabando el pedido de informe! "));
                    if (i == -2)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error generando el Informe XML! "));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar los filtros. "));
                }
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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "122")
                            valor = 1;
                    }
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }
        public void cargarDatosInformePedido(Informes_Pedidos ip)
        {
            try
            {
                ip.Fecha = DateTime.Now;
                ip.Informe = 3;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;
                ip.NombreInforme = "STOCK-UNIDADES" + "_" + DateTime.Now.ToString("ddMMyyyy");
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Informe_Pedido. Excepción: " + Ex.Message));
            }
        }
        public void cargarDatosInformeXML(InformeXML infXML)
        {
            try
            {
                int artInactivos = 0;
                if (this.chkArticulosInactivos.Checked == true)
                    artInactivos = 1;

                infXML.Empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                infXML.ArticulosInactivos = artInactivos;

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de InformeXML. Excepción: " + Ex.Message));
            }
            
        }
        #endregion

        #region Carga Inicial
        public void cargarEmpresas()
        {
            try
            {
                var dt = this.contSucursal.obtenerEmpresas();

                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = "-1";
                dt.Rows.InsertAt(dr, 0);

                //DataRow dr2 = dt.NewRow();
                //dr2["Razon Social"] = "Todas";
                //dr2["Id"] = "0";
                //dt.Rows.InsertAt(dr2, 1);

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Empresas a la lista. Excepción: " + Ex.Message));
            }
        }
        public void cargarSucursales()
        {
            try
            {
                this.chkListSucursales.Items.Clear();

                if (this.ListEmpresa.SelectedItem.Text != "Seleccione...")
                {
                    DataTable dt = new DataTable();

                    if (this.ListEmpresa.SelectedItem.Text == "Todas")
                        dt = this.contSucursal.obtenerSucursales();
                    else
                        dt = this.contSucursal.obtenerSucursalesDT(Convert.ToInt32(this.ListEmpresa.SelectedValue));

                    if (dt != null)
                    {
                        //this.phSucursal.Visible = true;

                        foreach (DataRow dr in dt.Rows)
                        {
                            ListItem item = new ListItem(dr["nombre"].ToString(), dr["id"].ToString());

                            this.chkListSucursales.Items.Add(item);
                            int i = this.chkListSucursales.Items.IndexOf(item);
                            this.chkListSucursales.Items[i].Selected = true;
                        }
                    }
                }
                else
                {
                    this.chkListSucursales.Items.Clear();
                    this.phSucursal.Visible = false;
                }
                    
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Sucursales. Excepción: " + Ex.Message));
            }
        }

        #endregion
    }
}