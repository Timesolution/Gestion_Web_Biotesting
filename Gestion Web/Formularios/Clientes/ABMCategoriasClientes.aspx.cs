using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ABMCategoriasClientes : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorCategoria controlador = new controladorCategoria();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idCategoria;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idCategoria = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarCategorias();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        categoria sg = this.controlador.obtenerCategoriaID(this.idCategoria);
                        txtCategoria.Text = sg.descripcion;
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Clientes.Categorias") != 1)
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
                        if (s == "22")
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


        private void cargarCategorias()
        {
            try
            {
                phCategorias.Controls.Clear();
                List<categoria> Categorias = this.controlador.obtenerCategoriasClientesList();
                foreach (categoria sg in Categorias)
                {
                    this.cargarCategoriasPH(sg);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Categorias. " + ex.Message));

            }
        }


        private void cargarCategoriasPH(categoria sg)
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
                btnEditar.Click += new EventHandler(this.editarCategorias);
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

                phCategorias.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Categoria en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    categoria est = new categoria();
                    est.id = this.idCategoria;
                    est.descripcion = this.txtCategoria.Text;
                    est.estado = 1;
                    int i = this.controlador.modificarCategoria(est);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Categoria: " + est.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Categoria modificada con exito", "ABMCategoriasClientes.aspx"));
                        this.cargarCategorias();
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Categoria"));

                    }
                }
                else
                {
                    int i = this.controlador.agregarCategoriaCliente(this.txtCategoria.Text);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Categoria: " + this.txtCategoria.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Categoria cargada con exito", null));
                        this.borrarCampos();
                        this.cargarCategorias();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Categoria"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Categoria. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtCategoria.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarCategorias(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMCategoriasClientes.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Categoria." + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idCategoria = Convert.ToInt32(this.txtMovimiento.Text);
                categoria c = this.controlador.obtenerCategoriaID(idCategoria);
                c.estado = 0;
                int i = this.controlador.modificarCategoria(c);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Categoria: " + c.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Categoria eliminada con exito", null));
                    this.cargarCategorias();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Categoria"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Categoria. " + ex.Message));
            }
        }
    }
}