using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Gestion_Api.Entitys;
using System.Globalization;
using System.Web.Configuration;
using Gestion_Api.Modelo.Enums;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CRM : System.Web.UI.Page
    {
        controladorFacturacion controlador = new controladorFacturacion();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorRemitos contRemito = new controladorRemitos();
        controladorCliente contCliente = new controladorCliente();
        controladorFactEntity contFactEntity = new controladorFactEntity();
        controladorSucursal cs = new controladorSucursal();
        controladorFunciones contFunciones = new controladorFunciones();
        ControladorEmpresa controlEmpresa = new ControladorEmpresa();
        ControladorFormasPago contFormPago = new ControladorFormasPago();
        controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();

        Mensajes m = new Mensajes();
        private int idUsuario;
        private string fechaD;
        private string fechaH;
        private string fechaVencimientoD;
        private string fechaVencimientoH;
        private string estado;
        private string descripcion;

        private int cliente;
        private int filtroPorFecha;
        private int filtroPorFechaVencimiento;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                //datos de filtro
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                fechaVencimientoD = Request.QueryString["fechaVencimientoDesde"];
                fechaVencimientoH = Request.QueryString["fechaVencimientoHasta"];

                descripcion = Request.QueryString["des"];

                estado = Request.QueryString["estado"];
                cliente = Convert.ToInt32(Request.QueryString["cl"]);
                filtroPorFecha = Convert.ToInt32(Request.QueryString["fpf"]);
                filtroPorFechaVencimiento = Convert.ToInt32(Request.QueryString["fpfv"]);
                idUsuario = Convert.ToInt32(Request.QueryString["us"]);

                if (!IsPostBack)
                {

                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    if (fechaD == null && fechaH == null && cliente == 0 && fechaVencimientoD == null && fechaVencimientoH == null)
                    {
                        //idUsuario = (int)Session["Login_SucUser"];
                        idUsuario = 0;
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        filtroPorFecha = 1;
                        filtroPorFechaVencimiento = 0;
                        DropListEstado.SelectedIndex = 0;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtVencimientoDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtVencimientoHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        descripcion = "";


                    }

                    this.cargarEstadosEventos();
                    this.cargarClientes();
                    this.cargarUsuario();


                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtVencimientoDesde.Text = fechaD;
                    txtVencimientoHasta.Text = fechaH;
                    DropListClientes.SelectedValue = cliente.ToString();
                    DropListEstado.SelectedIndex = Convert.ToInt32(estado);
                    txtDescripcion.Text = descripcion;

                }

                cargarEventoRango(fechaD, fechaH, fechaVencimientoD, fechaVencimientoH, cliente, Convert.ToInt32(estado), filtroPorFecha, filtroPorFechaVencimiento, idUsuario, descripcion);

                this.Form.DefaultButton = this.btnBuscarCod.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        public void cargarEstadosEventos()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                var estados = controladorClienteEntity.ObtenerEstadosEventoCliente();

                estados.Insert(0, new Gestion_Api.Entitys.Estados_Clientes_Eventos
                {
                    Id = 0,
                    descripcion = "Todos"
                });
                //agrego todos
                this.DropListEstado.DataSource = estados;
                this.DropListEstado.DataValueField = "Id";
                this.DropListEstado.DataTextField = "descripcion";
                this.DropListEstado.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados filtros. " + ex.Message));
            }
        }

        public void cargarUsuario()
        {
            try
            {
                controladorUsuario controladorUsuario = new controladorUsuario();

                DataTable usuarios = controladorUsuario.obtenerUsuarios();


                DataRow dr = usuarios.NewRow();
                dr["id"] = -1;
                dr["usuario"] = "Todos";
                usuarios.Rows.InsertAt(dr, 0);

                this.DropListUsuarios.DataSource = usuarios;
                this.DropListUsuarios.DataValueField = "id";
                this.DropListUsuarios.DataTextField = "usuario";
                this.DropListUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados filtros. " + ex.Message));
            }
        }

        // seguir con esto

        private void cargarEventoRango(string fechaD, string fechaH, string fechaVencimientoD, string fechaVencimientoH, int idCliente, int estado, int filtroPorFecha, int filtroPorFechaVencimiento, int idUsuario, string descripcion)

        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                decimal saldo = 0;
                string estados;


                var clientesEventos = controladorClienteEntity.GetEventosClientesFilter(fechaD, fechaH, fechaVencimientoD, fechaVencimientoH, idCliente, estado, filtroPorFecha, filtroPorFechaVencimiento, idUsuario, descripcion);


                foreach (var row in clientesEventos)
                {
                    estados = controladorClienteEntity.OtenerDescripcionEventoClienteById(Convert.ToInt32(row.Estado));
                    this.cargarEnPh(row, estados);
                    saldo += 1;
                }
                labelSaldo.Text = saldo.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando eventos clientes en CRM.  " + ex.Message));
            }
        }

        #region carga inicial
        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();

                dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }


        private void cargarEventosClientes()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                List<Clientes_Eventos> clientes_Eventos = controladorClienteEntity.GetAllEventosCliente();
                foreach (Clientes_Eventos e in clientes_Eventos)
                {
                    string estado = controladorClienteEntity.OtenerDescripcionEventoClienteById(Convert.ToInt32(e.Estado));
                    this.cargarEnPh(e, estado);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }
        private void cargarEnPh(Clientes_Eventos clientes_Eventos, string estado)
        {
            try
            {

                controladorContacto controladorContacto = new controladorContacto();
                controladorUsuario controladorUsuario = new controladorUsuario();
                controladorCliente controladorCliente = new controladorCliente();
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");
                string restaHoras;

                if (Convert.ToInt32(modificoHora) == 1)
                    restaHoras = WebConfigurationManager.AppSettings.Get("HorasDiferencia");


                string mail = string.Empty;
                List<contacto> contactos = this.contCliente.obtenerContactos((int)clientes_Eventos.Cliente);
                var primerMailContacto = contactos.FirstOrDefault();
                Cliente_Datos clienteDatos = controladorClienteEntity.obtenerClienteDatosByIdCliente((int)clientes_Eventos.Cliente);

                if(contactos.Count != 0)
                {
                    if (clienteDatos != null)
                        mail = primerMailContacto.mail + "; " + clienteDatos.Mail;
                    else
                        mail = primerMailContacto.mail;
                }
                else if (clienteDatos != null)
                    mail = clienteDatos.Mail;

                var cliente = controladorCliente.obtenerClienteID((int)clientes_Eventos.Cliente);


                //fila
                TableRow tr = new TableRow();
                tr.ID = clientes_Eventos.Id.ToString();


                //var email = controladorCliente.obtenerContactoCliente()

                //int estaRefact = this.contFactEntity.verificarRefacturado(f.id);
                //if (estaRefact > 0)
                //{
                //    tr.ForeColor = System.Drawing.Color.DarkGreen;
                //    tr.Font.Bold = true;
                //}

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = cliente.codigo;
                celCodigo.Width = Unit.Percentage(5);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCodigo);

                TableCell celCliente = new TableCell();
                celCliente.Text = cliente.razonSocial;
                celCliente.HorizontalAlign = HorizontalAlign.Center;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);


                TableCell celEmail = new TableCell();
                celEmail.Text = mail;
                celEmail.HorizontalAlign = HorizontalAlign.Center;
                celEmail.VerticalAlign = VerticalAlign.Middle;
                celEmail.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEmail);

                TableCell celFecha = new TableCell();
                celFecha.Text = clientes_Eventos.Fecha?.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celDetalle = new TableCell();
                celDetalle.Text = clientes_Eventos.Descripcion;
                celDetalle.VerticalAlign = VerticalAlign.Middle;
                celDetalle.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDetalle);

                TableCell celTarea = new TableCell();
                celTarea.Text = clientes_Eventos.Tarea;
                celTarea.VerticalAlign = VerticalAlign.Middle;
                celTarea.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTarea);

                TableCell celVencimiento = new TableCell();
                celVencimiento.Text = clientes_Eventos.Vencimiento?.ToString("dd/MM/yyyy");
                celVencimiento.VerticalAlign = VerticalAlign.Middle;
                celVencimiento.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celVencimiento);

                TableCell celEstado = new TableCell();
                celEstado.Text = estado;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celEstado);

                TableCell celUsuario = new TableCell();
                celUsuario.Text = controladorUsuario.obtenerUsuariosID((int)clientes_Eventos.Usuario).usuario;
                celUsuario.VerticalAlign = VerticalAlign.Middle;
                celUsuario.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celUsuario);

                TableCell celAccion = new TableCell();
                LinkButton btnConfirmacion = new LinkButton();
                btnConfirmacion.CssClass = "btn btn-info";
                btnConfirmacion.Attributes.Add("data-toggle", "modal");
                btnConfirmacion.Attributes.Add("href", "#modalConfirmarFinalizado");
                btnConfirmacion.ID = "btnSelec_" + clientes_Eventos.Id.ToString();
                btnConfirmacion.Text = "<span class='shortcut-icon icon-ok'></span>";
                btnConfirmacion.ToolTip = "Marcar como finalizado";
                btnConfirmacion.OnClientClick = "abrirdialog(" + clientes_Eventos.Id + ");";
                btnConfirmacion.Font.Size = 12;
                celAccion.Controls.Add(btnConfirmacion);
                tr.Cells.Add(celAccion);

                if (estado == "Atrasado")
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnRedireccionarCRM = new LinkButton();
                btnRedireccionarCRM.ID = "btnRedireccionar_" + clientes_Eventos.Id;
                btnRedireccionarCRM.CssClass = "btn btn-info ui-tooltip";
                btnRedireccionarCRM.Attributes.Add("data-toggle", "tooltip");
                btnRedireccionarCRM.PostBackUrl = "../Clientes/ClientesABM.aspx?accion=2&id=" + clientes_Eventos.Cliente.ToString();
                btnRedireccionarCRM.Text = "<span class='shortcut-icon icon-user'></span>";
                btnRedireccionarCRM.Attributes.Add("title data-original-title", "Ir a CRM");
                celAccion.Controls.Add(btnRedireccionarCRM);
                tr.Cells.Add(celAccion);


                phFacturas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando CRM. " + ex.Message));
            }

        }

        #endregion

        #region Eventos Controles
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    Response.Redirect("CRM.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&fechaVencimientoDesde=" + txtVencimientoDesde.Text + "&fechaVencimientoHasta=" + txtVencimientoHasta.Text + "&cl=" + DropListClientes.SelectedValue + "&estado=" + DropListEstado.SelectedValue + "&fpf=" + Convert.ToInt32(RdFecha.Checked) + "&fpfv=" + Convert.ToInt32(RdFechaVencimiento.Checked) + "&us=" + DropListUsuarios.SelectedValue + "&des=" + txtDescripcion.Text);

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));

            }
        }

        #endregion

        protected void lbtnConfirmarEventoFinalizado_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                int idEvento = Convert.ToInt32(this.txtMovimiento.Text);

                controladorClienteEntity.FinalizarEvento(idEvento);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Tarea finalizada", null));

                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            catch (Exception ex)
            {

            }
        }
    }
}