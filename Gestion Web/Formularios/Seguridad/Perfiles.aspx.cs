using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Seguridad
{
    public partial class Perfiles : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario controlador = new controladorUsuario();
        //valores
        private int valor;
        private int idPerfil;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idPerfil = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarPerfiles();
                if (!IsPostBack)
                {

                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    if (valor == 2)
                    {
                        Perfil p = this.controlador.obtenerPerfilID(this.idPerfil);
                        txtPerfil.Text = p.nombre;
                    }

                }

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
                        if (s == "58")
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

        private void cargarPerfiles()
        {
            try
            {
                phPerfiles.Controls.Clear();
                List<Perfil> perfiles = this.controlador.obtenerPerfilesList();
                foreach (Perfil p in perfiles)
                {
                    this.cargarPerfilesPH(p);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando lista de Permisos. " + ex.Message));

            }
        }

        private void cargarPerfilesPH(Perfil p)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = p.nombre;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = p.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarPerfil);
                if (p.nombre == "Administrador" || p.nombre == "SuperAdministrador" || p.nombre == "Vendedor" || p.nombre == "Cliente")
                {
                    btnEditar.Attributes.Add("Disabled", "true");
                }
                celAction.Controls.Add(btnEditar);

                    Literal l = new Literal();
                    l.Text = "&nbsp";
                    celAction.Controls.Add(l);


                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + p.id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    //btnEliminar.Font.Size = 9;
                    //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                    //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                    btnEliminar.OnClientClick = "abrirdialog(" + p.id + ");";
                    //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                    if (p.nombre == "Administrador" || p.nombre == "SuperAdministrador" || p.nombre == "Vendedor" || p.nombre == "Cliente")
                    {
                        btnEliminar.Attributes.Add("Disabled", "true");
                    }
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phPerfiles.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Permiso en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    Perfil p = new Perfil();
                    p.id = this.idPerfil;
                    p.nombre = txtPerfil.Text;
                    p.estado = 1;
                    int i = this.controlador.modificarPerfil(p);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Modifico Perfil: " + p.nombre);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Perfil modificado con exito", "Perfiles.aspx"));
                        this.borrarCampos();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Perfil"));

                    }
                }
                else
                {
                    int i = this.controlador.agregarPerfil(this.txtPerfil.Text);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Alta Perfil: " + this.txtPerfil.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Perfil cargado con exito", "Perfiles.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Perfil"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Perfil. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtPerfil.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarPerfil(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Perfiles.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Perfil. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(this.txtMovimiento.Text);
                Perfil p = this.controlador.obtenerPerfilID(idPerfil);
                p.estado = 0;
                int i = this.controlador.eliminarPerfil(p);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Baja Perfil: " + p.nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Perfil eliminado con exito", "Perfiles.aspx"));

                }
                else
                {
                    if(i == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se puede eliminar el Perfil ya que posee Usuarios asignados"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Permiso"));
                    }

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Permiso. " + ex.Message));
            }
        }

    }
}