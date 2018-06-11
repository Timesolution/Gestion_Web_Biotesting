using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMMarcas : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorArticulo controlador = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idMarca;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idMarca = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarMarcas();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        Marca m = this.controlador.obtenerMarcaID(this.idMarca);
                        txtMarca.Text = m.descripcion;
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Procedencias") != 1)
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
                        if (s == "8")
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


        private void cargarMarcas()
        {
            try
            {
                phMarcas.Controls.Clear();
                List<Marca> marcas = this.controlador.obtenerMarcas();
                foreach (Marca m in marcas)
                {
                    this.cargarMarcaPH(m);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Marcas. " + ex.Message));

            }
        }


        private void cargarMarcaPH(Marca m)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = m.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = m.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarMarca);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + m.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + m.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phMarcas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Marca en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Marca m = new Marca();
                m.id = this.idMarca;
                m.descripcion = txtMarca.Text;
                m.estado = 1;

                if (valor == 2)
                {
                    
                    
                    int i = this.controlador.modificarMarca(m);
                    this.cargarMarcas();
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico la Marca: " + m.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Marca modificada con exito", "ABMMarcas.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Marca"));

                    }
                }
                else
                {
                    int i = this.controlador.agregarMarca(m);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego la Marca: " + this.txtMarca.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Marca cargada con exito", "ABMMarcas.aspx"));
                        

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Marca"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Marca. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtMarca.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando Marca. " + ex.Message));
            }
        }

        private void editarMarca(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMMarcas.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Marca. " + ex.Message));
            }
        }

        private void eliminarMarca(object sender, EventArgs e)
        {
            try
            {
                string[] t = (sender as LinkButton).ID.Split(new Char[] { '_' });
                Marca m = this.controlador.obtenerMarcaID(Convert.ToInt32(t[1]));
                m.estado = 0;
                int i = this.controlador.modificarMarca(m);
                if (i > 0)
                {
                    //agrego bien
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Marca eliminada con exito", null));
                    this.cargarMarcas();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Marca"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Marca. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idMarca = Convert.ToInt32(this.txtMovimiento.Text);
                Marca m = this.controlador.obtenerMarcaID(idMarca);
                m.estado = 0;
                int i = this.controlador.modificarMarca(m);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Elimino la Marca: " + m.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Marca eliminada con exito", null));
                    this.cargarMarcas();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Marca"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Marca. " + ex.Message));
            }
        }
    }
}