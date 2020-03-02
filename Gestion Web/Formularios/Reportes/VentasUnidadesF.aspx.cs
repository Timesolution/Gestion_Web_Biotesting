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
    public partial class VentasUnidadesF : System.Web.UI.Page
    {
        controladorSucursal contSucursal = new controladorSucursal();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();
        ControladorInformesEntity contInformesEnt = new ControladorInformesEntity();
        controladorReportes contReportes = new controladorReportes();
        controladorCliente contCliente = new controladorCliente();
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
                    this.txtFechaDesde.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1).ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, today.AddDays(-1).Day).ToString("dd/MM/yyyy");

                    this.cargarEmpresas();
                    this.cargarListaPrecios();
                }
            }
            catch (Exception Ex)
            {

            }
        }

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
                        if (s == "121")
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
                ip.Informe = 4;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;
                ip.NombreInforme = "VENTAS-UNIDADES" + "_" + DateTime.Now.ToString("ddMMyyyy");
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

                //Primero convierto en DateTime las fechas agregadas, por si escribió cualquier cosa
                DateTime fechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR"));

                infXML.Empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                infXML.FechaDesde = fechaDesde.ToString("dd/MM/yyyy");
                infXML.FechaHasta = fechaHasta.ToString("dd/MM/yyyy");
                infXML.ListasDePrecios = obtenerListasPreciosTildadas();
                infXML.ArticulosInactivos = artInactivos;

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de InformeXML. Excepción: " + Ex.Message));
            }

        }
        #endregion

        #region Eventos Controles
        protected void lbtnSolicitarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifico que haya seleccionado alguna lista de precios. Genero una variable bool para realizar la verificacion
                bool listPreciosChecked = false;

                foreach (ListItem item in chkListListaPrecios.Items)
                {
                    if (item.Selected == true)
                        listPreciosChecked = true;
                }

                if (this.ListEmpresa.SelectedValue != "-1" && !string.IsNullOrEmpty(this.txtFechaDesde.Text) && !string.IsNullOrEmpty(this.txtFechaHasta.Text) && listPreciosChecked)
                {
                    Informes_Pedidos ip = new Informes_Pedidos();
                    InformeXML infXML = new InformeXML();

                    //Cargo el objeto Informes_Pedidos
                    this.cargarDatosInformePedido(ip);

                    //Cargo el objeto InformeXML
                    this.cargarDatosInformeXML(infXML);

                    if (ip == null || infXML == null)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando datos de informe. "));
                        return;
                    }

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

                if (dt.Rows.Count > 1)
                {
                    //DataRow dr2 = dt.NewRow();
                    //dr2["Razon Social"] = "Todas";
                    //dr2["Id"] = "0";
                    //dt.Rows.InsertAt(dr2, 1);
                }

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
        public void cargarListaPrecios()
        {
            try
            {
                this.chkListListaPrecios.Items.Clear();

                var dt = this.contCliente.obtenerListaPrecios();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem item = new ListItem(dr["nombre"].ToString(), dr["id"].ToString());

                        this.chkListListaPrecios.Items.Add(item);
                        int i = this.chkListListaPrecios.Items.IndexOf(item);
                        this.chkListListaPrecios.Items[i].Selected = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Listas de Precios a la lista. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
        public string obtenerListasPreciosTildadas()
        {
            try
            {
                //Recorro las listas de precios seleccionadas, si tildó todas, ingreso como valor un 0, que significa que filtró por todas.
                string listas = string.Empty;
                int flag = 0;

                foreach (ListItem item in chkListListaPrecios.Items)
                {
                    if (item.Selected == true)
                    {
                        listas += item.Value.ToString() + ",";
                        flag++;
                    }
                }

                //Si la variable flag coincide con la cantidad de items de la lista de checkbox de listas de precios, retorno un 0. Sino, retorno el string que generé.
                if (flag == chkListListaPrecios.Items.Count)
                    return "0";

                listas = listas.Substring(0, listas.Length - 1);

                return listas;

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo las Listas de Precios que fueron tildadas. Excepción: " + Ex.Message));
                return "0";
            }
        }
        #endregion
    }
}