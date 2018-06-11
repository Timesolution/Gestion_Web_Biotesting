using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMSubGruposArticulos : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorArticulo controlador = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idSubGrupo;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idSubGrupo = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarSubGrupos();
                if (!IsPostBack)
                {
                    
                    this.cargarGruposArticulos();
                    if (valor == 2)
                    {
                        SubGrupo sg = this.controlador.obtenerSubGrupoID(this.idSubGrupo);
                        ListGruposArticulos.SelectedValue = sg.grupo.id.ToString();
                        txtSubGrupo.Text = sg.descripcion;
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Articulos.Sub-Grupos") != 1)
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
                        if (s == "16")
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

        private void cargarSubGrupos()
        {
            try
            {
                phSubGrupos.Controls.Clear();
                List<SubGrupo> grupos = this.controlador.obtenerSubGrupos();
                foreach (SubGrupo sg in grupos)
                {
                    this.cargarSubGruposPH(sg);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando SubGrupos. " + ex.Message));

            }
        }

        private void cargarGruposArticulos()
        {
            try
            {

                DataTable dt = controlador.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListGruposArticulos.DataSource = dt;
                this.ListGruposArticulos.DataValueField = "id";
                this.ListGruposArticulos.DataTextField = "descripcion";

                this.ListGruposArticulos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarSubGruposPH(SubGrupo sg)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celGrupo = new TableCell();
                celGrupo.Text = sg.grupo.descripcion;
                celGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celGrupo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = sg.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = sg.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarSubGrupos);
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

                phSubGrupos.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando SubGrupo en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    SubGrupo sg = new SubGrupo();
                    sg.id = this.idSubGrupo;
                    sg.grupo.id = Convert.ToInt32(ListGruposArticulos.SelectedValue);
                    sg.descripcion = txtSubGrupo.Text;
                    sg.estado = 1;
                    int i = this.controlador.modificarSubGrupo(sg);
                    this.cargarSubGrupos();
                        if (i > 0)
                        {
                            //agrego bien
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modificacion SubGrupo: " + sg.descripcion);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("SubGrupo modificado con exito", "ABMSubGruposArticulos.aspx"));
                            this.borrarCampos();

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando SubGrupo"));

                        }
                }
                else
                {
                    int grupo = Convert.ToInt32(ListGruposArticulos.SelectedValue);
                    int i = this.controlador.agregarSubGrupo(grupo,this.txtSubGrupo.Text);
                        if (i > 0)
                        {
                            //agrego bien
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta SubGrupo: " + this.txtSubGrupo.Text);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("SubGrupo cargado con exito", null));
                            this.borrarCampos();
                            this.cargarSubGrupos();

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando SubGrupo"));

                        }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando SubGrupo. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.ListGruposArticulos.SelectedValue = "-1";
                this.txtSubGrupo.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarSubGrupos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMSubGruposArticulos.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar SubGrupo de Articulo. " + ex.Message));
            }
        }

        private void eliminarSubGrupos(object sender, EventArgs e)
        {
            try
            {
                string[] t = (sender as LinkButton).ID.Split(new Char[] { '_' });
                SubGrupo sg = this.controlador.obtenerSubGrupoID(Convert.ToInt32(t[1]));
                sg.estado = 0;
                int i = this.controlador.modificarSubGrupo(sg);
                if (i > 0)
                {
                    //agrego bien
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("SubGrupo eliminada con exito", null));
                    this.cargarSubGrupos();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando SubGrupo"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar SubGrupo. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idSubGrupo = Convert.ToInt32(this.txtMovimiento.Text);
                SubGrupo sg = this.controlador.obtenerSubGrupoID(idSubGrupo);
                sg.estado = 0;
                int i = this.controlador.modificarSubGrupo(sg);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja SubGrupo: " + sg.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("SubGrupo eliminada con exito", null));
                    this.cargarSubGrupos();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando SubGrupo"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar SubGrupo. " + ex.Message));
            }
        }
    }
}