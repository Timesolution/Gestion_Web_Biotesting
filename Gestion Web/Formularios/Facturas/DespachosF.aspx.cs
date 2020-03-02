using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class DespachosF : System.Web.UI.Page
    {
        //controladorCotizaciones controlador = new controladorCotizaciones();
        controladorRemitos controlador = new controladorRemitos();
        controladorUsuario contUser = new controladorUsuario();
        controladorDespacho controladorDespachos = new controladorDespacho();

        Mensajes m = new Mensajes();

        private int suc;
        private string fechaD;
        private string fechaH;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc= Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.cargarSucursal();

                if (fechaD == null && fechaH == null && suc == 0)
                {
                    suc = (int)Session["Login_SucUser"];
                    this.cargarSucursal();
                    fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DropListSucursal.SelectedValue = suc.ToString();
                }

                if (!IsPostBack)
                {
                   
                    
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                }

                this.cargarDespachos();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

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
                        if (s == "39")
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarDespachos()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                this.phDespachos.Controls.Clear();
                List<Despacho> guias = this.controladorDespachos.obtenerDespachoByFechaSuc(desde,hasta,this.suc);

                foreach (Despacho d in guias)
                {
                    this.cargarDespachosPH(d);
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void cargarDespachosPH(Despacho d)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = d.ID.ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = d.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = "Guia despacho Nº " + d.numero;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celNumero);

                TableCell celExpreso = new TableCell();
                celExpreso.Text = d.expreso;
                celExpreso.HorizontalAlign = HorizontalAlign.Left;
                celExpreso.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celExpreso);

                TableCell celValor = new TableCell();
                celValor.Text = "$" + d.Valor.Value.ToString();
                celValor.HorizontalAlign = HorizontalAlign.Right;
                celValor.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celValor);

                TableCell celContraReembolso = new TableCell();
                if(d.contrareembolso.Value == 1)
                    celContraReembolso.Text = "SI";
                else
                    celContraReembolso.Text = "NO";
                celContraReembolso.HorizontalAlign = HorizontalAlign.Left;
                celContraReembolso.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celContraReembolso);

                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + d.ID.ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.detalleDespacho);
                celAccion.Controls.Add(btnDetalles);
                tr.Controls.Add(celAccion);

                this.phDespachos.Controls.Add(tr);
            }
            catch(Exception ex)
            {

            }
        }
        private void detalleDespacho(object sender, EventArgs e)
        {            
            try
            {
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idGuia = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionReportes.aspx?a=1&Desp=" + idGuia + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {
                
            }
        }      

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        Response.Redirect("DespachosF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de guias de despachos. " + ex.Message));
            }
        }               
        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando despachos. " + ex.Message));
            }
        }

    }
}