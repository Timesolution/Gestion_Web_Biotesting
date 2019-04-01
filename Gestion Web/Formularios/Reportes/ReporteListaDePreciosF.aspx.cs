using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReporteListasDePreciosF : System.Web.UI.Page
    {
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
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

                    cargarDLLListaDePrecio();
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
                if (this.ddlListaDePrecios.SelectedValue != "-1")
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
                        if (s == "201")
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
                ip.Informe = 5;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;
                ip.NombreInforme = "ARTICULOS-LISTA-DE-PRECIOS" + "_" + DateTime.Now.ToString("ddMMyyyy");

                if (chkUbicacion.Checked)//si es agrupado por ubicacion
                {
                    ip.Informe = 6;
                    ip.NombreInforme = "ARTICULOS-LISTA-DE-PRECIOS-AGRUPADO-POR-UBICACION" + "_" + DateTime.Now.ToString("ddMMyyyy");
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: cargarDatosInformeListaDePrecios. Ex: " + Ex.Message));
            }
        }
        public void cargarDatosInformeXML(InformeXML infXML)
        {
            try
            {
                infXML.ListaPrecio = Convert.ToInt32(ddlListaDePrecios.SelectedValue);

                //if (this.chkDescuentoCantidad.Checked == true)
                //    infXMLartInactivos = 1;

                infXML.ArticulosPrecioConIva = 1; //1 es sin iva
                if (RadioConIva.Checked) infXML.ArticulosPrecioConIva = 2;

                if (chkUbicacion.Checked) infXML.ArticulosAguparPorUbicacion = 1;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de InformeXML lista de precios. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Carga Inicial
        public void cargarDLLListaDePrecio()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                DataTable dt = contCliente.obtenerListaPrecios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                //controles modalEtiquetas
                this.ddlListaDePrecios.DataSource = dt;
                this.ddlListaDePrecios.DataValueField = "id";
                this.ddlListaDePrecios.DataTextField = "nombre";
                this.ddlListaDePrecios.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        #endregion


    }
}