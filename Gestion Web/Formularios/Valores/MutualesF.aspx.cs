using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class MutualesF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                if (!IsPostBack)
                {
                                   
                }

                this.cargarMutuales();
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
                        if (s == "91")
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
        private void cargarMutuales()
        {
            try
            {
                phMutuales.Controls.Clear();
                List<Mutuale> mutuales = this.controlador.obtenerMutuales();
                foreach (Mutuale m in mutuales)
                {
                    this.cargarMutualesPH(m);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando mutuales. " + ex.Message));

            }
        }
        private void cargarMutualesPH(Mutuale m)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celNombre = new TableCell();
                celNombre.Text = m.Nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(15);
                tr.Cells.Add(celNombre);

                TableCell celDireccion = new TableCell();
                celDireccion.Text = m.Direccion;
                celDireccion.VerticalAlign = VerticalAlign.Middle;
                celDireccion.Width = Unit.Percentage(15);
                tr.Cells.Add(celDireccion);

                TableCell celComision = new TableCell();
                celComision.Text = m.Comision.ToString() + "%";
                celComision.VerticalAlign = VerticalAlign.Middle;
                celComision.Width = Unit.Percentage(5);
                tr.Cells.Add(celComision);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = m.Telefono;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                celTelefono.Width = Unit.Percentage(10);
                tr.Cells.Add(celTelefono);

                TableCell celObservacion = new TableCell();
                celObservacion.Text = m.Observaciones;
                celObservacion.VerticalAlign = VerticalAlign.Middle;
                celObservacion.Width = Unit.Percentage(25);
                tr.Cells.Add(celObservacion);

                TableCell celAction = new TableCell();                
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = m.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";                
                btnEditar.Click += new EventHandler(this.editarMutual);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + m.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + m.Id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(15);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phMutuales.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando operadores en la lista. " + ex.Message));
            }
        }
        private void editarMutual(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMMutuales.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Mutual. " + ex.Message));
            }
        }        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                Mutuale m = this.controlador.obtenerMutualByID(Convert.ToInt32(this.txtMovimiento.Text));
                m.Estado = 0;
                int i = this.controlador.modificarMutual(m);                
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Mutual : ");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Mutual eliminada con exito", null));
                    this.cargarMutuales();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Mutual"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Mutual. " + ex.Message));
            }
        }
        

    }
}