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

namespace Gestion_Web.Formularios.Surtidores
{
    public partial class SurtidoresF : System.Web.UI.Page
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

                this.cargarSurtidores();
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
                        if (s == "99")
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
        private void cargarSurtidores()
        {
            try
            {
                phSurtidores.Controls.Clear();
                List<Surtidore> surtidores = this.controlador.obtenerSurtidores();
                foreach (Surtidore s in surtidores)
                {
                    this.cargarSurtidoresPH(s);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Surtidores. " + ex.Message));

            }
        }
        private void cargarSurtidoresPH(Surtidore s)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celNombre = new TableCell();
                celNombre.Text = s.Codigo;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = s.Descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celNumero = new TableCell();
                celNumero.Text = s.Numero.ToString();
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNumero);

                TableCell celContador = new TableCell();
                celContador.Text = s.Contador.ToString();
                celContador.VerticalAlign = VerticalAlign.Middle;
                celContador.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celContador);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = s.Precio.Value.ToString("C");
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(15);                

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + s.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";                
                btnEditar.PostBackUrl = "../../Formularios/Surtidores/ABMSurtidores.aspx?a=2&id=" + s.Id;
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + s.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + s.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phSurtidores.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando posnet en la lista. " + ex.Message));
            }
        }        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idSurt = Convert.ToInt32(this.txtMovimiento.Text);
                Surtidore s = this.controlador.obtenerSurtidorByID(idSurt);
                int i = this.controlador.eliminarSurtidor(s);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Surtidor : " + idSurt);
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

    }
}