using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMSubListas : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorListaPrecio controlador = new controladorListaPrecio();
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
                        ListaCategoria c = this.controlador.obtenerCategoriaID(this.idCategoria);
                        txtCategoria.Text = c.categoria;
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
                        if (s == "27")
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
                phSubListas.Controls.Clear();
                List<ListaCategoria> categorias = this.controlador.obtenerCategoriasList();
                foreach (ListaCategoria c in categorias)
                {
                    this.cargarCategoriasPH(c);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando categorias. " + ex.Message));

            }
        }


        private void cargarCategoriasPH(ListaCategoria c)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = c.categoria;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = c.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarSubLista);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + c.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + c.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phSubListas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando categoria en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    this.modificarCategoria();
                }
                else
                {
                    this.agregarCategoria();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando SubLista. " + ex.Message));
            }
        }

        public void agregarCategoria()
        {
            try
            {
                ListaCategoria c = new ListaCategoria();
                c.categoria = this.txtCategoria.Text;
                int i = this.controlador.agregarCategoria(c);
                
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta SubLista de Precio: " + c.categoria);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Categoria Agregada con exito", "ABMSubListas.aspx"));
                    this.borrarCampos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando categoria"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando categoria. " + ex.Message));
            }
        }

        public void modificarCategoria()
        {
            try
            {
                ListaCategoria c = new ListaCategoria();
                c.id = this.idCategoria;
                c.categoria = this.txtCategoria.Text;
                c.estado = 1;
                int i = this.controlador.modificarCategoria(c);

                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico SubLista de Precio: " + c.categoria);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("categoria modificada con exito", "ABMSubListas.aspx"));
                    this.borrarCampos();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando categoria"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando categoria. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando categoria. " + ex.Message));
            }
        }

        private void editarSubLista(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMSubListas.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar SubLista. " + ex.Message));
            }
        }

        private int verificarArticulosLista(int idSublista)
        {
            try
            {
                controladorArticulo contArticulo = new controladorArticulo();
                List<Articulo> lst = contArticulo.obtenerArticulosBySubLista(idSublista);
                if (lst.Count > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("La Sublista contiene articulos, no se puede eliminar. "));
                    return -1;
                }
                else
                {
                    return 1;
                }
                
            }
            catch (Exception ex)
            {
                return -1;
            }
        }



        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(this.txtMovimiento.Text);
                ListaCategoria cat = this.controlador.obtenerCategoriaID(idPerfil);
                cat.estado = 0;

                int verificaBorrar = this.verificarArticulosLista(idPerfil);

                if (verificaBorrar == 1)
                {
                    int i = this.controlador.modificarCategoria(cat);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja SubLista de Precio: " + cat.categoria);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Categoria eliminada con exito", "ABMSubListas.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Categoria"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Categoria. " + ex.Message));
            }
        }
    }
}