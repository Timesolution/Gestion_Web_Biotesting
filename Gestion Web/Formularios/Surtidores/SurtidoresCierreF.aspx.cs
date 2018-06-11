using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Surtidores
{
    public partial class SurtidoresCierreF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador        
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();

        int surtidor;
        string fDesde;
        string fHasta;
        int permisoAnular;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.fDesde = Request.QueryString["fd"];
                this.fHasta = Request.QueryString["fh"];
                this.surtidor = Convert.ToInt32(Request.QueryString["s"]);

                this.VerificarLogin();                
                if (!IsPostBack)
                {
                    if (this.fDesde == null && this.fHasta == null)
                    {
                        this.fDesde = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fHasta = DateTime.Today.ToString("dd/MM/yyyy");
                    }
                    this.cargarSurtidores();
                    this.ListSurtidores.SelectedValue = this.surtidor.ToString();
                    this.txtFechaDesde.Text = this.fDesde;
                    this.txtFechaHasta.Text = this.fHasta;
                }
                this.cargarCierresSurtidores();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "100")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil != "SuperAdministrador")
                            {
                                this.verificarPermisoAnular();
                            }
                            else
                            {
                                this.permisoAnular = 1;
                            }
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
        private void verificarPermisoAnular()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "101")
                        {
                            this.permisoAnular = 1;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void cargarSurtidores()
        {
            try
            {
                List<Surtidore> list = this.controlador.obtenerSurtidores();
                this.ListSurtidores.DataSource = list;
                this.ListSurtidores.DataTextField = "Descripcion";
                this.ListSurtidores.DataValueField = "Id";
                this.ListSurtidores.DataBind();

                this.ListSurtidores.Items.Insert(0, new ListItem("Todos", "0"));
            }
            catch
            {

            }
        }
        private void cargarCierresSurtidores()
        {
            try
            {
                phCierres.Controls.Clear();
                DateTime desde = Convert.ToDateTime(this.fDesde, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fHasta, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<Surtidores_Cierre> cierres = this.controlador.obtenerCierresSurtidor(desde, hasta, this.surtidor);
                foreach (Surtidores_Cierre s in cierres)
                {
                    this.cargarCierreSurtidorPH(s);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Surtidores. " + ex.Message));

            }
        }
        private void cargarCierreSurtidorPH(Surtidores_Cierre s)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celSurtidor = new TableCell();
                celSurtidor.Text = s.Surtidore.Descripcion;
                celSurtidor.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSurtidor);

                TableCell celFecha = new TableCell();
                celFecha.Text = s.Fecha.Value.ToString("dd/MM/yyyy HH:mm:ss tt");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celCantInicial = new TableCell();
                celCantInicial.Text = s.CantidadInicial.Value.ToString();
                celCantInicial.VerticalAlign = VerticalAlign.Middle;
                celCantInicial.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantInicial);

                TableCell celCantCierre = new TableCell();
                celCantCierre.Text = s.CantidadCierre.Value.ToString();
                celCantCierre.VerticalAlign = VerticalAlign.Middle;
                celCantCierre.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantCierre);               

                TableCell celTotalLitros = new TableCell();
                celTotalLitros.Text = (s.CantidadCierre.Value - s.CantidadInicial.Value).ToString();
                celTotalLitros.VerticalAlign = VerticalAlign.Middle;
                celTotalLitros.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotalLitros);

                TableCell celTotalPesos = new TableCell();
                decimal total = 0;
                if (s.PrecioVenta != null)
                {
                    total = ((s.CantidadCierre.Value - s.CantidadInicial.Value) * s.PrecioVenta.Value);
                    total = decimal.Round(total, 2);
                }
                celTotalPesos.Text = total.ToString("C");
                celTotalPesos.VerticalAlign = VerticalAlign.Middle;
                celTotalPesos.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotalPesos);

                TableCell celUsuario = new TableCell();
                Usuario user = this.contUser.obtenerUsuariosID(s.IdUsuario.Value);
                celUsuario.Text = user.usuario;
                celUsuario.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celUsuario);

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(10);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + s.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + s.Id + ");";
                if (this.permisoAnular == 0)
                {
                    btnEliminar.Attributes.Add("disabled", "disabled");
                }
                celAction.Controls.Add(btnEliminar);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phCierres.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando posnet en la lista. " + ex.Message));
            }
        }    
        protected void btnQuitarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                int idCierre = Convert.ToInt32(this.txtMovimiento.Text);
                Surtidores_Cierre cierre = this.controlador.obtenerCierreSurtidorByID(idCierre);
                int i = this.controlador.eliminarCierreSurtidor(cierre);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Cierre Surtidor : " + idCierre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Eliminado con exito", Request.Url.ToString()));                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando."));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar. " + ex.Message));
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SurtidoresCierreF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&s=" + this.ListSurtidores.SelectedValue);
            }
            catch
            {

            }
        }
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionSurtidores.aspx?a=1&ex=1&FD=" + this.txtFechaDesde.Text + "&FH=" + this.txtFechaHasta.Text + "&s=" + this.ListSurtidores.SelectedValue);
            }
            catch
            {

            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Surtidores/ImpresionSurtidores.aspx?a=1&FD=" + this.txtFechaDesde.Text + "&FH=" + this.txtFechaHasta.Text + "&s=" + this.ListSurtidores.SelectedValue + "', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

    }
}