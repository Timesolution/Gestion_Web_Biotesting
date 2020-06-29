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

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ABMEstadosCRM : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorEstadoCliente controlador = new controladorEstadoCliente();
        controladorUsuario contUser = new controladorUsuario();

        ControladorClienteEntity ControladorClienteEntity = new ControladorClienteEntity();
        //valores
        private int valor;
        private int idEstado;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idEstado = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarEstados();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        //EstadoCliente sg = this.controlador.obtenerEstadoID(this.idEstado);
                        txtEstado.Text = ControladorClienteEntity.OtenerDescripcionEventoClienteById(this.idEstado);
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Clientes.Estados") != 1)
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
                        if (s == "24")
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


        private void cargarEstados()
        {
            try
            {
                phEstados.Controls.Clear();

                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                List<Estados_Clientes_Eventos> Estados = controladorClienteEntity.ObtenerEstadosEventoCliente();

                foreach (Estados_Clientes_Eventos sg in Estados)
                {
                    this.cargarEstadosPH(sg);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Estados. " + ex.Message));

            }
        }


        private void cargarEstadosPH(Estados_Clientes_Eventos sg)
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
                btnEditar.ID = sg.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarEstados);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + sg.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + sg.Id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phEstados.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Estado en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                if (valor == 2)
                {
                    Estados_Clientes_Eventos est = new Estados_Clientes_Eventos();
                    est.Id = this.idEstado;
                    est.descripcion = this.txtEstado.Text;
                    est.Estado = 1;
                    var i = controladorClienteEntity.EditarEstadoEventoCliente(est);
                    if (i != null)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Estado de CRM: " + est.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Estado modificado con exito", "ABMEstadosCRM.aspx"));
                        this.cargarEstados();
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Estado"));

                    }
                }
                else
                {
                    Estados_Clientes_Eventos est = new Estados_Clientes_Eventos();
                    est.descripcion = this.txtEstado.Text;
                    est.Estado = 1;

                    var i = controladorClienteEntity.AgregarEstadoEventoCliente(est);
                    if (i != null)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Estado de CRM: " + this.txtEstado.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Estado cargado con exito", null));
                        this.borrarCampos();
                        this.cargarEstados();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Estado"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Estado. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtEstado.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarEstados(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMEstadosCRM.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Estado de Articulo. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                int idEstado = Convert.ToInt32(this.txtMovimiento.Text);
                Estados_Clientes_Eventos est = controladorClienteEntity.ObtenerEstadosEventoClienteById(idEstado);
                est.Estado = 0;
                var i = controladorClienteEntity.EditarEstadoEventoCliente(est);
                if (i != null)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Articulo: " + est.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Estado eliminada con exito", null));
                    this.cargarEstados();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Estado"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Estado. " + ex.Message));
            }
        }
    }
}