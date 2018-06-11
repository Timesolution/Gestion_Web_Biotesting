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

namespace Gestion_Web.Formularios.Transportes
{
    public partial class EntregasF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        ControladorExpreso controlador = new ControladorExpreso();
        
        //valores
        private int accion;
        private int idTipoEntrega;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.accion = Convert.ToInt32(Request.QueryString["a"]);
            this.idTipoEntrega = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarEntregas();
                if (!IsPostBack)
                {
                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (accion == 2)
                    {
                        var te = this.controlador.obtenerTipoEntregaPorID(this.idTipoEntrega);
                        txtTipo.Text = te.Descripcion;
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
                        if (s == "12")
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

        private void cargarEntregas()
        {
            try
            {
                phEntregas.Controls.Clear();
                var tiposEntregas = this.controlador.obtenerTiposEntrega();
                foreach (var entrega in tiposEntregas)
                {
                    this.cargarEntregaPH(entrega);    
                }         
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Tipos de entregas. " + ex.Message));

            }
        }

        private void cargarEntregaPH(TiposEntrega te)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = te.Descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = te.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarTipos);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + te.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + te.Id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phEntregas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Tipo Entrega en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //estoy modificando
                if (this.accion == 2)
                {
                    this.modificarTipoEntrega();
                }
                else
                {
                    //agrego nueva entrega
                    this.agregarTipoEntrega();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo. " + ex.Message));
            }

        }

        void agregarTipoEntrega()
        {
            try
            {
                var te = new TiposEntrega();
                te.Descripcion = this.txtTipo.Text;
                int i = this.controlador.agregarTipoEntrega(te);
                if (i > 0)
                {
                    //agrego bien
                    //Log.EscribirSQL(idUsuario, "INFO", "Agrego el Grupo de Articulo: " + i);
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta tipo articulo: " + this.txtTipo.Text);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tipo Entrega cargado con exito", null));
                    this.borrarCampos();
                    this.cargarEntregas();
                    
                }
                else
                {
                    if (i == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya existe tipo entrega con el nombre " + this.txtTipo.Text));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando tipoEntrega"));
                    }

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Tipo Entrega. " + ex.Message));
            }

        }

        void modificarTipoEntrega()
        {
            try
            {
                TiposEntrega te = new TiposEntrega();
                te.Id = this.idTipoEntrega;
                te.Descripcion = txtTipo.Text;
                int i = this.controlador.modificarTipoEntrega(te);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  tipo de entrega: " + te.Descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tipo entrega modificado con exito", "EntregasF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Tipo Entrega"));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Tipo Entrega. " + ex.Message));
            }
        }

        public void borrarCampos()
        {
            try
            {
                this.txtTipo.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarTipos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("EntregasF.aspx?a=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar tipo de entrega. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                long idTipoEntrega = Convert.ToInt64(this.txtMovimiento.Text);
                int i = this.controlador.quitarTipoEntrega(idTipoEntrega);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja tipo entrega ID : " + idTipoEntrega);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tipo Entrega eliminado con exito", null));
                    this.cargarEntregas();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Tipo Entrega"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Tipo Entrega. " + ex.Message));
            }
        }
    }
}