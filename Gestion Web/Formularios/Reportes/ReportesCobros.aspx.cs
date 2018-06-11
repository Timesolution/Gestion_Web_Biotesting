using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReportesCobros : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        controladorSucursal contSucu = new controladorSucursal();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        controladorCuentaCorriente contrCC = new controladorCuentaCorriente();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();

        Mensajes m = new Mensajes();
        private string fechaD;
        private string fechaH;
        private int idCliente;
        private int idVendedor;
        private int idEmpresa;
        private int idSucursal;
        private int tipo;
        private int estado;
        private int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idVendedor = Convert.ToInt32(Request.QueryString["Vendedor"]);
                tipo = Convert.ToInt32(Request.QueryString["tipo"]);
                idSucursal = Convert.ToInt32(Request.QueryString["suc"]);
                estado = Convert.ToInt32(Request.QueryString["estado"]);
                accion = Convert.ToInt32(Request.QueryString["accion"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && idVendedor == 0 && idCliente == 0 && idSucursal == 0)
                    {                        
                        fechaD = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        idSucursal = (int)Session["Login_SucUser"];
                    }
                    this.cargarClientes();
                    this.cargarVendedores();
                    this.cargarSucursal();
                    //txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListClientes.SelectedValue = idCliente.ToString();
                    DropListVendedores.SelectedValue = idVendedor.ToString();
                    DropListTipo.SelectedValue = tipo.ToString();
                    DropListSucursal.SelectedValue = idSucursal.ToString();
                    idEmpresa = (int)Session["Login_EmpUser"];
                    this.ListEstado.SelectedValue = this.estado.ToString();
                }
                this.verificarTipoPerfil();
                this.cargarDatosRango(fechaD, fechaH, idCliente, idVendedor, tipo, idSucursal, estado);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));

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
                        if (s == "53")
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
        private void verificarTipoPerfil()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil == "SuperAdministrador")
                {
                    this.DropListSucursal.Attributes.Remove("disabled");
                }
                else
                {
                    if (perfil == "Vendedor")
                    {
                        this.DropListVendedores.Attributes.Add("disabled", "disabled");
                        this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                        int vend = (int)Session["Login_Vendedor"];
                        this.DropListVendedores.SelectedValue = vend.ToString();

                        //Si no filtro, no le muestro la tabla de clientes y el saldo
                        if (this.accion == 0)
                        {
                            this.phTopClientes.Visible = false;
                            this.lblDocumentosImpagosPesos.Visible = false;
                        }
                            
                    }
                    else
                    {
                        //this.DropListVendedores.SelectedValue = this.idVendedor.ToString();
                        this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                    }
                }
            }
            catch
            {

            }
        }
        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                DataTable dt = new DataTable();
                //verifico si es super admin
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil == "Vendedor")
                {
                    int idVendedor = (int)Session["Login_Vendedor"];
                    dt = contCliente.obtenerClientesByVendedorDT(idVendedor);
                }
                else
                {
                    dt = contCliente.obtenerClientesDT();                    
                }

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
                this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.DropListClientes.Items.Insert(1, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        public void cargarVendedores()
        {
            try
            {
                DataTable dt = contVendedor.obtenerVendedores();
                this.DropListVendedores.Items.Clear();
                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                DataRow dr3 = dt.NewRow();
                dr3["nombre"] = "Todos";
                dr3["id"] = 0;
                dt.Rows.InsertAt(dr3, 1);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    DropListVendedores.Items.Add(item);
                }

                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego selecc
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);
                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 1);

                //modalbusqueda
                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.DropListSucursal.SelectedValue = this.idSucursal.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        private void cargarDatosRango(string fechaD, string fechaH, int idCliente, int idVendedor, int tipoFac, int idSuc, int estado)
        {
            try
            {
                if (fechaD != null && fechaH != null)
                {
                    //this.lblDocumentosImpagosPesos.Text = this.controlador.obtenerTotalDocumentosImpagos(fechaD, fechaH, idCliente, idVendedor, this.idSucursal).ToString("0.00");

                    this.cargarTablaTopClientesCantidad(fechaD, fechaH, idCliente, idVendedor, tipoFac, idSuc, estado);
                    this.cargarTablaTopVendedoresCantidad(fechaD, fechaH, idCliente, idVendedor, tipoFac, idSuc);
                    this.cargarLabel(fechaD, fechaH, idCliente, idVendedor, tipoFac);
                }
                else
                {
                    this.cargarTablaTopClientesCantidad(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.ListEstado.SelectedValue));
                    this.cargarTablaTopVendedoresCantidad(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Datos. " + ex.Message));
            }
        }
        private void cargarLabel(string fechaD, string fechaH, int idCliente, int idVendedor, int tipoFac)
        {
            try
            {
                string label = "";
                //label += fechaD + "," + fechaH + ",";
                label += "Impagas al " + fechaH + ",";
                if (idCliente > 0)
                {
                    label += this.contCliente.obtenerClienteID(idCliente).alias + ","; //DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                }
                if (idVendedor > 0)
                {
                    Vendedor vend = this.contVendedor.obtenerVendedorID(idVendedor);
                    label += vend.emp.apellido + " " + vend.emp.nombre + ",";//DropListVendedores.Items.FindByValue(idVendedor.ToString()).Text;
                }

                label += DropListTipo.Items.FindByValue(tipoFac.ToString()).Text + ",";
                label += ListEstado.SelectedItem.Text;

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        // Top Clientes
        public void cargarTablaTopClientesCantidad(string fechaD, string fechaH, int idCliente, int idVendedor, int tipoFac, int idSuc, int estado)
        {
            try
            {
                DataTable dt = controlador.obtenerTablaTopClientes(fechaD, fechaH, idCliente, idVendedor, idSuc, tipoFac, estado);
                Decimal importe = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    importe += Convert.ToDecimal(dr["importe"]);
                    this.cargarTopClientesCantidadTable(dr);
                }
                this.lblDocumentosImpagosPesos.Text = importe.ToString("'$'#,0.00");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes" + ex.Message));
            }
        }
        private void cargarTopClientesCantidadTable(DataRow dr)
        {
            try
            {
                if (Math.Abs(Convert.ToDecimal(dr["Importe"])) > Convert.ToDecimal(0.05))
                {
                    TableRow tr = new TableRow();

                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = dr["razonSocial"].ToString();
                    celCodigo.VerticalAlign = VerticalAlign.Bottom;
                    celCodigo.HorizontalAlign = HorizontalAlign.Left;
                    celCodigo.Width = Unit.Percentage(55);

                    tr.Cells.Add(celCodigo);

                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = "$ " + dr["Importe"].ToString();
                    celCantidad.Width = Unit.Percentage(30);
                    celCantidad.VerticalAlign = VerticalAlign.Bottom;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celCantidad);

                    TableCell celAccion = new TableCell();

                    LinkButton btnEditar = new LinkButton();
                    btnEditar.ID = dr["id"].ToString();
                    btnEditar.CssClass = "btn btn-info ui-tooltip";
                    btnEditar.Attributes.Add("data-toggle", "tooltip");
                    btnEditar.Attributes.Add("title data-original-title", "Editar");
                    btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                    btnEditar.Font.Size = 9;
                    int PuntoVenta = this.contSucu.obtenerPrimerPuntoVenta(Convert.ToInt32(this.DropListSucursal.SelectedValue), idEmpresa);
                    btnEditar.PostBackUrl = "../../Formularios/Facturas/CuentaCorrienteF.aspx?a=2&cliente=" + dr["id"].ToString() + "&sucursal=" + Convert.ToInt32(this.DropListSucursal.SelectedValue) + "&tipo=" + DropListTipo.SelectedValue;
                    celAccion.Controls.Add(btnEditar);

                    Literal l = new Literal();
                    l.Text = "&nbsp";
                    celAccion.Controls.Add(l);


                    LinkButton btnAvisarSMS = new LinkButton();
                    btnAvisarSMS.ID = "btnAvisarSMS_" + dr["id"].ToString() + "_" + dr["Importe"].ToString();
                    btnAvisarSMS.CssClass = "btn btn-info ui-tooltip";
                    btnAvisarSMS.Attributes.Add("data-toggle", "tooltip");
                    btnAvisarSMS.Attributes.Add("title data-original-title", "Enviar SMS");
                    btnAvisarSMS.Text = "<span class='shortcut-icon fa fa-paper-plane'></span>";
                    btnAvisarSMS.Click += new EventHandler(this.abrirModalEnvioSMS);
                    celAccion.Controls.Add(btnAvisarSMS);
                    celAccion.Width = Unit.Percentage(15);

                    tr.Cells.Add(celAccion);
                    phTopClientes.Controls.Add(tr);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes. " + ex.Message));

            }
        }
        private int verificarMostrarAccionSMS()
        {
            try
            {
                ControladorConfiguracion controlConfig = new ControladorConfiguracion();
                Gestion_Api.Entitys.Configuraciones_SMS c = controlConfig.ObtenerConfiguracionesAlertasSMS();
                if (c != null)
                {
                    if (c.Estado == 1 && c.AlertaSaldoCC == 1 && !String.IsNullOrEmpty(c.MensajeSaldoCC))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        private void abrirModalEnvioSMS(object sender, EventArgs e)
        {
            try
            {
                int idC = Convert.ToInt32((sender as LinkButton).ID.ToString().Split('_')[1]);
                int ok = this.verificarMostrarAccionSMS();
                if (ok < 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Para poder enviar avisos por sms debe activar esta opcion en Herramientas -> SMS -> Alertas. "));
                    return;
                }
                else
                {
                    ControladorConfiguracion contConfigSMS = new ControladorConfiguracion();
                    Gestion_Api.Entitys.Configuraciones_SMS config = contConfigSMS.ObtenerConfiguracionesAlertasSMS();
                    if (this.estado == 0)
                    {
                        this.txtMensajeSMS.Text = config.MensajeSaldoCC + "- Saldo " + (sender as LinkButton).ID.ToString().Split('_')[2];
                    }
                    else
                    {
                        this.txtMensajeSMS.Text = config.MensajeFcVencida + "- Saldo " + (sender as LinkButton).ID.ToString().Split('_')[2];
                    }
                    this.txtIdEnvioSMS.Text = idC.ToString();
                    var mail = this.contClienteEntity.obtenerClienteDatosByCliente(idC);
                    if (mail != null && mail.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(mail.FirstOrDefault().Celular))
                        {
                            this.txtCodArea.Text = mail.FirstOrDefault().Celular.Split('-')[0];
                            this.txtTelefono.Text = mail.FirstOrDefault().Celular.Split('-')[1];
                        }
                    }
                    ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "openModal", "openModal();", true);
                }
            }
            catch
            {

            }
        }
        private void enviarAvisoSaldoCtaCte()
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();
                string idCliente = this.txtIdEnvioSMS.Text;

                if (this.txtCodArea.Text.Length + this.txtTelefono.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");", true);
                    return;
                }
                this.guardarNumeroTelefonoCliente(this.txtCodArea.Text + "-" + this.txtTelefono.Text, Convert.ToInt32(idCliente));
                string telefono = "+549" + this.txtCodArea.Text + this.txtTelefono.Text;
                string textoSMS = Regex.Replace(this.txtMensajeSMS.Text, @"\t|\n|\r", " ");
                int i = contSMS.enviarAlertaSaldoCC(telefono, textoSMS, (int)Session["Login_IdUser"], Convert.ToInt32(idCliente));
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Aviso enviado con exito!. ", ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar aviso. "));
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void guardarNumeroTelefonoCliente(string telefono, int idCliente)
        {
            try
            {
                var datosMail = this.contClienteEntity.obtenerClienteDatosByCliente(idCliente);
                if (datosMail != null)
                {
                    if (datosMail.Count > 0)
                    {
                        datosMail.FirstOrDefault().Celular = telefono;
                        this.contClienteEntity.modificarClienteDatos(datosMail.FirstOrDefault());
                    }
                    else
                    {
                        Cliente_Datos datos = new Cliente_Datos();
                        datos.Mail = "";
                        datos.IdCliente = idCliente;
                        datos.Celular = telefono;
                        this.contClienteEntity.agregarClienteDatos(datos);
                    }
                }
            }
            catch
            {

            }
        }
        // Top Vendedores
        public void cargarTablaTopVendedoresCantidad(string fechaD, string fechaH, int idCliente, int idVendedor, int tipoFac, int idSuc)
        {
            try
            {
                DataTable dt = this.controlador.obtenerTablaTopVendedores(fechaD, fechaH, idCliente, idVendedor, idSuc, tipoFac);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopVendedoresCantidadTable(dr);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Vendedores por Cantidad. " + ex.Message));
            }
        }
        private void cargarTopVendedoresCantidadTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["Nombre"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(70);
                tr.Cells.Add(celCodigo);


                TableCell celCantidad = new TableCell();
                celCantidad.Text = "$ " + dr["Importe"].ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopVendedores.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos" + ex.Message));

            }
        }

        #region controles
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    Response.Redirect("ReportesCobros.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&suc=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&Vendedor=" + DropListVendedores.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&estado=" + this.ListEstado.SelectedValue + "&accion=1");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de cotizaciones. " + ex.Message));

            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dtDatos = new DataTable();
                //dtDatos.Columns.Add("id");
                //dtDatos.Columns.Add("nombre");
                //dtDatos.Columns.Add("importeAcumulado");

                //foreach (var control in this.phTopClientes.Controls)
                //{
                //    DataRow drDatos = dtDatos.NewRow();
                //    TableRow tr = control as TableRow;

                //    drDatos[0] = 1;
                //    drDatos[1] = tr.Cells[0].Text;
                //    drDatos[2] = tr.Cells[1].Text;
                //    dtDatos.Rows.Add(drDatos);

                //}
                //Session.Add("datosMov", dtDatos);
                //Session.Add("saldoMov", lblDocumentosImpagos.Text);

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?Cobro=" + 0 + "&valor=3', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?Cobro=" + 0 + "&valor=3&fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&suc=" + DropListSucursal.SelectedValue + "&cli=" + DropListClientes.SelectedValue + "&ven=" + DropListVendedores.SelectedValue + "&t=" + DropListTipo.SelectedValue + "&vencida=" + this.ListEstado.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error imprimiendo reporte" + ex.Message));
            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = txtFechaDesde.Text;
                    fechaH = txtFechaHasta.Text;
                }
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?Cobro=" + 0 + "&valor=5', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&vencida=" + this.ListEstado.SelectedValue + "&t=" + this.tipo + "&Cobro=" + 0 + "&valor=5");
            }
            catch
            {
            }

        }

        protected void lbtnImprimirDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = txtFechaDesde.Text;
                    fechaH = txtFechaHasta.Text;
                }
                //Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&Cobro=" + 0 + "&valor=4");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&t=" + this.tipo + "&Cobro=" + 0 + "&valor=4', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }

        }

        protected void lbtnExportar2_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaD == null && fechaH == null)
                {
                    this.fechaD = this.txtFechaDesde.Text;
                    this.fechaH = this.txtFechaHasta.Text;
                }

                Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&t=" + this.tipo + "&vencida=" + this.ListEstado.SelectedValue + "&Cobro=" + 0 + "&valor=3&ex=1");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error exportando reporte" + ex.Message));
            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClientes = this.contCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
                //this.cargarClientesTable(cliente);

                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "razonSocial";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnEnviarSMS_Click(object sender, EventArgs e)
        {
            try
            {
                this.enviarAvisoSaldoCtaCte();
            }
            catch
            {

            }
        }        

        protected void lbtnImprimirVencimientos_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?Cobro=" + 0 + "&valor=8&fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&suc=" + DropListSucursal.SelectedValue + "&cli=" + DropListClientes.SelectedValue + "&ven=" + DropListVendedores.SelectedValue + "&t=" + DropListTipo.SelectedValue + "&vencida=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void lbtnExportarVencimientos_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&t=" + this.tipo + "&vencida=1&Cobro=" + 0 + "&valor=8&ex=1");
            }
            catch
            {

            }
        }

        protected void btnImpagasVendedor_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = txtFechaDesde.Text;
                    fechaH = txtFechaHasta.Text;
                }
                //Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&Cobro=" + 0 + "&valor=4");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&t=" + this.tipo + "&Cobro=" + 0 + "&valor=9', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }


        #endregion

        protected void btnExpImpagasVendedor_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = txtFechaDesde.Text;
                    fechaH = txtFechaHasta.Text;
                }
                //Response.Redirect("/Formularios/Cobros/ImpresionCobro.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&Cobro=" + 0 + "&valor=4");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Cobros/ImpresionCobro.aspx?ex=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&ven=" + this.idVendedor + "&t=" + this.tipo + "&Cobro=" + 0 + "&valor=9', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
    }
}