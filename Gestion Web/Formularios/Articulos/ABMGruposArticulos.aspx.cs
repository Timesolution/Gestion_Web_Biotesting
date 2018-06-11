using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMGruposArticulos : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorArticulo controlador = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idGrupo;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idGrupo = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarGrupos();
                if (!IsPostBack)
                {
                    
                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (valor == 2)
                    {
                        grupo sg = this.controlador.obtenerGrupoID(this.idGrupo);
                        txtGrupo.Text = sg.descripcion;
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
                    //if(this.contUser.validarAcceso(this.idUsuario,"Maestro.Articulos.Grupos") != 1)
                    if(this.verificarAcceso() != 1)
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

        private void cargarGrupos()
        {
            try
            {
                phGruposArticulos.Controls.Clear();
                List<grupo> grupos = this.controlador.obtenerGruposArticulosList();
                foreach (grupo sg in grupos)
                {
                    this.cargarGruposPH(sg);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Grupos. " + ex.Message));

            }
        }

        private void cargarGruposPH(grupo sg)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = sg.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = sg.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarGrupos);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + sg.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + sg.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phGruposArticulos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    grupo sg = new grupo();
                    sg.id = this.idGrupo;
                    sg.descripcion = txtGrupo.Text;
                    sg.estado = 1;
                    int i = this.controlador.modificarGrupo(sg);
                    this.cargarGrupos();
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL(idUsuario, "INFO", "Modifico el Grupo de Articulo: " + this.idGrupo);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  Grupo de Articulo: " + sg.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo modificada con exito", "ABMGruposArticulos.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Grupo"));
                    }
                }
                else
                {
                    int i = this.controlador.agregarGrupo(this.txtGrupo.Text);
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL(idUsuario, "INFO", "Agrego el Grupo de Articulo: " + i);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Grupo de Articulo: " + this.txtGrupo.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo cargada con exito", null));
                        this.borrarCampos();
                        this.cargarGrupos();
                        Response.Redirect("ABMGruposArticulos.aspx");

                    }
                    else
                    {
                        if (i == -2)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya existe un grupo con el nombre " + this.txtGrupo.Text));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo"));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtGrupo.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarGrupos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMGruposArticulos.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Grupo de Articulo. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idGrupo = Convert.ToInt32(this.txtMovimiento.Text);
                grupo g = this.controlador.obtenerGrupoID(idGrupo);
                g.estado = 0;
                int i = this.controlador.eliminarGrupo(g);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo eliminado con exito", null));
                    this.cargarGrupos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Grupo"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Grupo. " + ex.Message));
            }
        }
    }
}