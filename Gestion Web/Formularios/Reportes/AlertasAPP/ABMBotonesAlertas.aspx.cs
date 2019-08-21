using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores.APP;
using Disipar.Models;

namespace Gestion_Web.Formularios.Reportes.AlertasAPP
{
    public partial class ABMBotonesAlertas : System.Web.UI.Page
    {
        Mensajes _m = new Mensajes();
        ControladorAlertaAPP _controladorAlertaAPP = new ControladorAlertaAPP();
        int _valor;
        int _idAlertaBotonMensaje;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();

                _valor = Convert.ToInt32(Request.QueryString["valor"]);
                _idAlertaBotonMensaje = Convert.ToInt32(Request.QueryString["idam"]);

                CargarBotonesMensajes();

                if (!IsPostBack)
                {
                    CargarAlertaBotones();

                    if (_valor == 2)
                    {
                        EditarMensaje();
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar alertasAPP. " + ex.Message);
            }
            
        }

        private void EditarMensaje()
        {
            try
            {
                var alertaBotonMensaje = _controladorAlertaAPP.ObtenerAlertaBotonMensajeID(_idAlertaBotonMensaje);
                listBotones.SelectedValue = alertaBotonMensaje.IdBoton.ToString();
                txtMensaje.Text = alertaBotonMensaje.Mensaje;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al editar mensaje. " + ex.Message);
            }
            
        }

        void CargarBotonesMensajes()
        {
            try
            {
                phBotones.Controls.Clear();
                var alertasBotonesMensajes = _controladorAlertaAPP.ObtenerAlertasBotonesMensajes();
                
                foreach (var botonMensaje in alertasBotonesMensajes)
                {
                    CargarBotonMensajePH(botonMensaje);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando alertas botones. " + ex.Message));
            }
        }

        void CargarBotonMensajePH(AlertasAPP_Botones_MensajesTemporal botonMensaje)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celGrupo = new TableCell();
                celGrupo.Text = botonMensaje.grupo;
                celGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celGrupo);

                TableCell celMensaje = new TableCell();
                celMensaje.Text = botonMensaje.mensaje;
                celMensaje.VerticalAlign = VerticalAlign.Middle;
                celMensaje.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celMensaje);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = botonMensaje.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(EditarAlertasBotonesMensajes);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + botonMensaje.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + botonMensaje.id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phBotones.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando botones alertas mensajes en la lista. " + ex.Message));
            }
        }

        private void EditarAlertasBotonesMensajes(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMBotonesAlertas.aspx?valor=2&idam=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error al editar alerta botones mensajes. " + ex.Message));
            }
        }

        void CargarAlertaBotones()
        {
            try
            {                
                var alertasBotones = _controladorAlertaAPP.ObtenerAlertasBotones();

                listBotones.DataSource = alertasBotones;
                listBotones.DataValueField = "Id";
                listBotones.DataTextField = "NombreBoton";
                listBotones.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando alertas botones. " + ex.Message));
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

                    }
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_valor == 2)
                {
                    AlertasAPP_Botones_Mensajes alertaBotonMensaje = new AlertasAPP_Botones_Mensajes();
                    alertaBotonMensaje.Id = _idAlertaBotonMensaje;
                    alertaBotonMensaje.IdBoton = Convert.ToInt32(listBotones.SelectedValue);
                    alertaBotonMensaje.Mensaje = txtMensaje.Text;

                    int i = _controladorAlertaAPP.ModificarAlertaBotonMensaje(alertaBotonMensaje);

                    if (i > 0)
                    {
                        txtMensaje.Text = "";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxInfo("Mensaje modificado con exito!", "ABMBotonesAlertas.aspx"));
                    }
                    else                    
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error modificando mensaje"));
                }
                else
                {
                    AlertasAPP_Botones_Mensajes botonMensaje = new AlertasAPP_Botones_Mensajes();

                    botonMensaje.IdBoton = Convert.ToInt32(listBotones.SelectedValue);
                    botonMensaje.Mensaje = txtMensaje.Text;

                    int i = _controladorAlertaAPP.AgregarMensajeAlBotonAlerta(botonMensaje);

                    if (i > 0)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxInfo("Mensaje agregado con exito!", "ABMBotonesAlertas.aspx"));
                    else
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error agregando mensaje!"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al agregar mensaje al boton alerta. " + ex.Message);
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idAlertaBotonMensaje = Convert.ToInt32(this.txtMovimiento.Text);

                int i = _controladorAlertaAPP.EliminarAlertaBotonMensaje(idAlertaBotonMensaje);
                if (i > 0)                
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxInfo("Mensaje eliminado con exito", null));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error eliminando mensaje"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error al eliminar el mensaje. " + ex.Message));
            }
        }
    }
}