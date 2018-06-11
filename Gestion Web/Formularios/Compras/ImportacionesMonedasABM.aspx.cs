using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImportacionesMonedasABM : System.Web.UI.Page
    {        
        Mensajes mje = new Mensajes();
        //controlador
        ControladorImportaciones contImportaciones = new ControladorImportaciones();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idMoneda;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idMoneda = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (valor == 2)
                    {
                        this.cargarMoneda();
                    }
                }
                this.cargarMonedasImportacion();
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
                    //if(this.contUser.validarAcceso(this.idUsuario,"Maestro.Articulos.Grupos") != 1)
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "15")
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
        private void cargarMoneda()
        {
            try
            {
                Monedas_Importacion m = this.contImportaciones.obtenerMonedaImportacion(this.idMoneda);
                this.txtMoneda.Text = m.Moneda;
                this.txtCotizacion.Text = m.Cotizacion.Value.ToString();
            }
            catch
            {

            }
        }
        private void cargarMonedasImportacion()
        {
            try
            {
                this.phMonedas.Controls.Clear();
                List<Monedas_Importacion> monedas = this.contImportaciones.obtenerMonedasImportacion();

                foreach (var m in monedas)
                {
                    this.cargarMonedasPH(m);
                }
            }
            catch
            {

            }
        }
        private void cargarMonedasPH(Monedas_Importacion m)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celMoneda = new TableCell();
                celMoneda.Text = m.Moneda;
                celMoneda.HorizontalAlign = HorizontalAlign.Left;
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celMoneda);

                TableCell celCotizacion = new TableCell();
                celCotizacion.Text = m.Cotizacion.Value.ToString("C", new CultureInfo("es-AR"));
                celCotizacion.HorizontalAlign = HorizontalAlign.Right;
                celCotizacion.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCotizacion);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + m.Id;
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.editarMoneda);
                celAccion.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + m.Id;
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + m.Id + ");";
                celAccion.Controls.Add(btnEliminar);

                tr.Controls.Add(celAccion);

                this.phMonedas.Controls.Add(tr);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.valor == 2)
                {
                    this.modificarMoneda();
                }
                else
                {
                    this.agregarMoneda();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error guardando. " + ex.Message));
            }

        }
        private void agregarMoneda()
        {
            try
            {
                Monedas_Importacion m = new Monedas_Importacion();
                m.Moneda = this.txtMoneda.Text;
                m.Cotizacion = Convert.ToDecimal(this.txtCotizacion.Text);
                m.Estado = 1;
                int ok = this.contImportaciones.agregarMonedaImportacion(m);
                if(ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Guardado con exito!", "ImportacionesMonedasABM.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo guardar"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error guardando moneda . " + ex.Message));
            }
        }
        private void modificarMoneda()
        {
            try
            {
                Monedas_Importacion m = this.contImportaciones.obtenerMonedaImportacion(this.idMoneda);
                m.Moneda = this.txtMoneda.Text;
                m.Cotizacion = Convert.ToDecimal(this.txtCotizacion.Text);                
                int ok = this.contImportaciones.modificarMonedaImportacion(m);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Modificado con exito!", "ImportacionesMonedasABM.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo guardar"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error guardando moneda . " + ex.Message));
            }
        }
        private void editarMoneda(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImportacionesMonedasABM.aspx?valor=2&id=" + (sender as LinkButton).ID.Split('_')[1]);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.txtMovimiento.Text);
                Monedas_Importacion m = this.contImportaciones.obtenerMonedaImportacion(id);
                m.Estado = 0;

                int i = this.contImportaciones.modificarMonedaImportacion(m);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Moneda Importacion: " + m.Moneda );
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Eliminado con exito", "ImportacionesMonedasABM.aspx"));                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando."));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar.. " + ex.Message));
            }
        }
    }
}