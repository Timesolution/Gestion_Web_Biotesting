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
    public partial class UsuariosF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        //controlador
        controladorUsuario controlador = new controladorUsuario();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.cargarUsuarios();
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando Usuarios. " + ex.Message));
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
                        if (s == "59")
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


        private void cargarUsuarios()
        {
            try
            {
                phUsuarios.Controls.Clear();
                List<Usuario> usuarios = this.controlador.obtenerUsuariosByIdUser(Convert.ToInt32(Session["Login_IdUser"]));
                foreach (Usuario user in usuarios)
                {
                    this.cargarUsuarioTabla(user);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarUsuarioTabla(Usuario user)
        {
            try
            {

                TableRow tr = new TableRow();
                TableCell celUsuario = new TableCell();
                celUsuario.Text = user.usuario;
                celUsuario.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celUsuario);

                TableCell CelFormaFacturar = new TableCell();
                CelFormaFacturar.Text = user.contraseña;
                //CelFormaFacturar.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelFormaFacturar);

                TableCell CelNombreFantasia = new TableCell();
                CelNombreFantasia.Text = user.empresa.RazonSocial;
                CelNombreFantasia.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelNombreFantasia);

                TableCell CelDireccion = new TableCell();
                CelDireccion.Text = user.sucursal.nombre;
                CelDireccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelDireccion);

                TableCell celPerfil = new TableCell();
                if(user.perfil.nombre != "Cliente")
                {
                    celPerfil.Text = user.perfil.nombre;
                }
                else
                {
                    celPerfil.Text = "BioExperta/o";
                }
                celPerfil.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPerfil);

                TableCell celPuntoVta = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = user.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar.PostBackUrl = "ABMUsuarios.aspx?valor=2&id=" + user.id;
                celPuntoVta.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celPuntoVta.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + user.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + user.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celPuntoVta.Controls.Add(btnEliminar);
                celPuntoVta.Width = Unit.Percentage(10);
                tr.Cells.Add(celPuntoVta);

                phUsuarios.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando usuario en la lista. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(this.txtMovimiento.Text);
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                user.estado = 0;
                int i = this.controlador.modificarUsuarios(user);
                if (i > 0)
                {
                    //agrego bien
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Usuario eliminado con exito", "UsuariosF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Usuario"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Usuario. " + ex.Message));
            }
        }
    }
}