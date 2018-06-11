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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;


namespace Gestion_Web.Formularios.Valores
{
    public partial class ChequesF_2 : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        ControladorBanco contBanco = new ControladorBanco();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int sucCobro;
        private int sucPago;
        private string fechaD;
        private string fechaH;
        private int idCliente;
        private int idProveedor;
        private int tipoFecha;
        private int origen;
        private int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                #region QueryStrings
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                this.sucCobro = Convert.ToInt32(Request.QueryString["SucCobro"]);
                this.sucPago = Convert.ToInt32(Request.QueryString["SucPago"]);
                this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                this.idProveedor = Convert.ToInt32(Request.QueryString["prov"]);
                this.origen = Convert.ToInt32(Request.QueryString["o"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);
                #endregion

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && sucCobro == 0 && sucPago == 0 && idCliente == 0 && idProveedor == 0)
                    {
                        this.cargarSucursal();
                        this.cargarClientes();
                        this.cargarProveedores();

                        this.inicializarVariables();

                        Response.Redirect("ChequesF_2.aspx?fechaDesde=" + txtDesdeC.Text + "&fechaHasta=" + txtHastaC.Text + "&SucCobro=" + DropListSucursal.SelectedValue + "&SucPago=" + DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&prov=" + this.DropListProveedores.SelectedValue + "&tf=3");
                    }

                    this.cargarSucursal();
                    this.cargarClientes();
                    this.cargarProveedores();
                    this.cargarCuentas();
                    this.cargarBancos();
                    txtFechaDesdeRecibido.Text = fechaD;
                    txtFechaHastaRecibido.Text = fechaH;
                    txtFechaDesdeEntregado.Text = fechaD;
                    txtFechaHastaEntregado.Text = fechaH;
                    txtDesdeC.Text = fechaD;
                    txtHastaC.Text = fechaH;
                    txtDesdeI.Text = fechaD;
                    txtHastaI.Text = fechaH;

                    this.txtFechaAgregar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaCAgregar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaImputar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.DropListClientes.SelectedValue = idCliente.ToString();
                    this.DropListProveedores.SelectedValue = this.idProveedor.ToString();
                    this.DropListSucursal.SelectedValue = this.sucCobro.ToString();
                    this.DropListSucursalPago.SelectedValue = this.sucPago.ToString();
                    this.DropDownListOrigen.SelectedValue = this.origen.ToString();
                    this.DropListEstado.SelectedValue = this.estado.ToString();

                    this.verificarPermisoImputar();
                }

                this.cargarChequesRango(fechaD, fechaH, sucCobro, sucPago, idCliente, idProveedor);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        #region Carga Inicial
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
                        if (s == "47")
                        {    //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                                this.DropListSucursalPago.Attributes.Remove("disabled");
                            }
                            else
                            {
                                this.verficarPermisoCambiarSucursal();
                            }
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
        public void verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                string permiso = listPermisos.Where(x => x == "84").FirstOrDefault();

                if (permiso != null)
                {
                    this.DropListSucursal.Attributes.Remove("disabled");
                    this.DropListSucursalPago.Attributes.Remove("disabled");
                }
                else
                {
                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                    this.DropListSucursalPago.SelectedValue = Session["Login_SucUser"].ToString();
                }

            }
            catch
            {

            }
        }
        private void verificarPermisoImputar()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permisoImputar = listPermisos.Where(x => x == "87").FirstOrDefault();

                if (permisoImputar == null)
                {
                    this.btnImputar.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                //Sucursal Cobro
                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                //Sucursal Pago
                this.DropListSucursalPago.DataSource = dt;
                this.DropListSucursalPago.DataValueField = "Id";
                this.DropListSucursalPago.DataTextField = "nombre";

                this.DropListSucursalPago.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();
                DataTable dt2 = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

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
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListProveedores.DataSource = dt;
                this.DropListProveedores.DataValueField = "id";
                this.DropListProveedores.DataTextField = "alias";

                this.DropListProveedores.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }
        public void cargarBancos()
        {
            try
            {
                DataTable dt = this.controlador.obtenerBancosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListBancoAgregar.DataSource = dt;
                this.ListBancoAgregar.DataValueField = "id";
                this.ListBancoAgregar.DataTextField = "entidad";
                this.ListBancoAgregar.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando bancos a la lista. " + ex.Message));
            }
        }
        public void cargarCuentas()
        {
            try
            {
                ControladorBanco contBanco = new ControladorBanco();
                var cuentas = contBanco.obtenerCuentasBancarias();
                //this.DropListCuentas.Items.Add("Seleccione...");
                foreach (var c in cuentas)
                {
                    this.DropListCuentas.Items.Add(c.Id + " - " + c.Banco1.entidad + " - " + c.Descripcion + " - " + c.Numero + " - " + c.Cuit + " - " + c.Librador);
                    this.DropListCuentas.Items[this.DropListCuentas.Items.Count - 1].Value = c.Id.ToString();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifico que se haya seleccionado alguna sucursal
                if (this.DropListSucursal.SelectedValue == "-1")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    return;
                }

                //Segun el radiobutton tildado, verifico que sus respectivas fechas esten cargadas.

                //Filtro por Fecha Recibido
                if (this.RadioFechaRecibido.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtFechaDesdeRecibido.Text) && string.IsNullOrEmpty(this.txtFechaHastaRecibido.Text))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                        return;
                    }

                    Response.Redirect("ChequesF_2.aspx?fechadesde=" + txtFechaDesdeRecibido.Text + "&fechaHasta=" + txtFechaHastaRecibido.Text + "&SucCobro=" + DropListSucursal.SelectedValue + "&SucPago=" + this.DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&prov=" + this.DropListProveedores.SelectedValue + "&tf=1");
                }

                //Filtro por Fecha Entregado
                if (this.RadioFechaEntregado.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtFechaDesdeEntregado.Text) && string.IsNullOrEmpty(this.txtFechaHastaEntregado.Text))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                        return;
                    }

                    Response.Redirect("ChequesF_2.aspx?fechadesde=" + txtFechaDesdeEntregado.Text + "&fechaHasta=" + txtFechaHastaEntregado.Text + "&SucCobro=" + DropListSucursal.SelectedValue + "&SucPago=" + this.DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&prov=" + this.DropListProveedores.SelectedValue + "&tf=2");
                }

                //Filtro por Fecha Acreditacion
                if (this.RadioFechaCobro.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtDesdeC.Text) && string.IsNullOrEmpty(this.txtHastaC.Text))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                        return;
                    }

                    Response.Redirect("ChequesF_2.aspx?fechadesde=" + txtDesdeC.Text + "&fechaHasta=" + txtHastaC.Text + "&SucCobro=" + DropListSucursal.SelectedValue + "&SucPago=" + DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&prov=" + this.DropListProveedores.SelectedValue + "&tf=3");
                }

                //Filtro por Fecha Imputacion
                if (this.RadioFechaImp.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtDesdeI.Text) && string.IsNullOrEmpty(this.txtHastaI.Text))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                        return;
                    }

                    Response.Redirect("ChequesF_2.aspx?fechadesde=" + txtDesdeI.Text + "&fechaHasta=" + txtHastaI.Text + "&SucCobro=" + DropListSucursal.SelectedValue + "&SucPago=" + DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&prov=" + this.DropListProveedores.SelectedValue + "&tf=4");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos de caja. " + ex.Message));

            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                this.imprimirCheques(0);
            }
            catch
            {

            }
        }
        protected void btnImprimir2_Click(object sender, EventArgs e)
        {
            try
            {
                this.imprimirCheques(1);
            }
            catch
            {

            }
        }
        protected void btnImputar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorBanco contBanco = new ControladorBanco();

                string idtildado = "";
                //recorro los cheques en pantalla
                foreach (Control C in phCheques.Controls)
                {
                    TableRow tr = C as TableRow;
                    String estadoCh = tr.Cells[12].Text;
                    CheckBox ch = tr.Cells[14].Controls[2] as CheckBox;
                    //Si esta seleccionado, tiene estado disponible y es de terceros guardo el id.
                    if (ch.Checked == true && estadoCh == "Disponible")
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (idtildado == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo se pueden imputar cheques en estado Disponible y de Terceros. "));
                }
                else
                {
                    int i = 0;
                    int idUser = (int)Session["Login_IdUser"];
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")//por cada id de cheque que tenga creo un cheque_cuentas, lo imputo a cta y lo pongo en estado (5)-imputado.
                        {
                            Cheques_Cuentas cc = new Cheques_Cuentas();
                            cc.idCheque = Convert.ToInt32(id);
                            cc.idCuenta = Convert.ToInt32(this.DropListCuentas.SelectedValue);
                            cc.fechaImputado = Convert.ToDateTime(txtFechaImputar.Text, new CultureInfo("es-AR"));
                            i = contBanco.imputarChequeCuenta(cc);
                            if (i > 0)
                            {
                                Log.EscribirSQL(idUser, "INFO", "Imputo cheque id: " + id + " a cuenta.");
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error imputando cheques a cta bancaria. " + ex.Message));
            }

        }
        protected void lbtnAgregarCh_Click(object sender, EventArgs e)
        {
            try
            {
                this.agregarCheque();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error enviando agregar cheque. Excepción " + Ex.Message));
            }
        }
        protected void RadioFechaImp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.DropListSucursal.SelectedValue = "0";
                this.DropListSucursalPago.SelectedValue = "0";

                if (this.RadioFechaImp.Checked)
                    this.DropListEstado.SelectedValue = "5";
                else
                    this.DropListEstado.SelectedValue = "0";
                    
            }
            catch
            {

            }
        }
        protected void lbtnSiDebitar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorCobranzaEntity contCobEnt = new ControladorCobranzaEntity();

                string idtildado = "";
                //recorro los cheques en pantalla
                foreach (Control C in phCheques.Controls)
                {
                    TableRow tr = C as TableRow;
                    String estadoCh = tr.Cells[12].Text;
                    CheckBox ch = tr.Cells[14].Controls[2] as CheckBox;
                    var cheque = this.controlador.obtenerChequeId(Convert.ToInt32(ch.ID.Split('_')[1]));
                    //Si esta seleccionado, tiene estado entregado .
                    if (ch.Checked == true && estadoCh == "Entregado" && cheque.origen == "P")
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (idtildado == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo se pueden debitar cheques en estado Entregados y Propios. "));
                }
                else
                {
                    int i = 0;
                    int idUser = (int)Session["Login_IdUser"];
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")//por cada id de cheque que tenga creo un cheques_debitos, y lo pongo en estado (6) debitado.
                        {
                            DateTime fechaDebitado = Convert.ToDateTime(txtFechaDebitar.Text, new CultureInfo("es-AR"));
                            i = contCobEnt.debitarChequeEntregado(Convert.ToInt32(id), fechaDebitado);
                            if (i > 0)
                            {
                                Log.EscribirSQL(idUser, "INFO", "Debito cheque id: " + id);
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error debitando cheques. " + ex.Message));
            }
        }
        protected void btnBuscarCodigoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClienteNombreDT(buscar);

                if (dtClientes.Rows.Count > 1 && buscar == "%")
                {
                    DataRow dr2 = dtClientes.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dtClientes.Rows.InsertAt(dr2, 0);
                }

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }
        protected void btnBuscarCodigoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodProveedor.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);

                if (dtClientes.Rows.Count > 1 && buscar == "%")
                {
                    DataRow dr2 = dtClientes.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dtClientes.Rows.InsertAt(dr2, 0);
                }

                //cargo la lista
                this.DropListProveedores.DataSource = dtClientes;
                this.DropListProveedores.DataValueField = "id";
                this.DropListProveedores.DataTextField = "alias";
                this.DropListProveedores.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }
        protected void RadioFechaCobro_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.DropListSucursal.SelectedValue = "0";
                this.DropListSucursalPago.SelectedValue = "0";
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error al modificar estado de controles . Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region ABM
        private void cargarChequesRango(string fechaD, string fechaH, int idSucCobro, int idSucPago, int idCliente, int idProveedor)
        {
            try
            {
                if (fechaD != null && fechaH != null)
                {

                    List<ChequesValores> lstCheques = controlador.obtenerDatosCheque3(fechaD, fechaH, idSucCobro,idSucPago, idCliente, idProveedor, this.origen, this.estado, this.tipoFecha);

                    decimal saldo = 0;
                    foreach (ChequesValores ch in lstCheques)
                    {
                        this.cargarEnPh(ch);
                        saldo += ch.Cheque.importe;
                    }
                    lblSaldo.Text = saldo.ToString("C");

                    this.cargarLabel(fechaD, fechaH, idSucCobro, idSucPago, idCliente, idProveedor, this.tipoFecha);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Cheques. " + ex.Message));
            }
        }
        private void cargarEnPh(ChequesValores ch)
        {
            try
            {
                VisualizacionCheques vista = new VisualizacionCheques();
                TableRow tr = new TableRow();

                int i = (ch.Cheque.fecha.AddDays(30) - DateTime.Today).Days;
                if (i <= 10 && ch.Cheque.estado == 1)
                {
                    tr.ForeColor = System.Drawing.Color.Orange;
                }
                if (i <= 5 && ch.Cheque.estado == 1)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }

                //Celdas
                TableCell celFechaCobro = new TableCell();
                celFechaCobro.Text = string.Empty;
                if (ch.FechaCobro != null && ch.FechaCobro > new DateTime(0001, 1, 1))
                    celFechaCobro.Text = ch.FechaCobro.ToString("dd/MM/yyyy");
                celFechaCobro.VerticalAlign = VerticalAlign.Middle;
                celFechaCobro.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaCobro);

                TableCell celFechaPago = new TableCell();
                celFechaPago.Text = string.Empty;
                if (ch.FechaPago != null && ch.FechaPago > new DateTime(0001, 1, 1))
                    celFechaPago.Text = ch.FechaPago.ToString("dd/MM/yyyy");
                celFechaPago.VerticalAlign = VerticalAlign.Middle;
                celFechaPago.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaPago);

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(ch.Cheque.fecha);
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celFechaImp = new TableCell();
                Cheques_Cuentas datos = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                if (datos != null)
                {
                    celFechaImp.Text = datos.fechaImputado.Value.ToString("dd/MM/yyyy");
                }
                else
                    celFechaImp.Text = "";
                celFechaImp.VerticalAlign = VerticalAlign.Middle;
                celFechaImp.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaImp);

                TableCell celReciboC = new TableCell();
                celReciboC.Text = ch.ReciboCobro.ToString();
                celReciboC.VerticalAlign = VerticalAlign.Middle;
                celReciboC.HorizontalAlign = HorizontalAlign.Left;
                if (vista.columnaReciboCobro == 1)
                {
                    this.phColumnaReciboCobro.Visible = true;
                    tr.Cells.Add(celReciboC);
                }

                TableCell celReciboP = new TableCell();
                celReciboP.Text = ch.ReciboPago.ToString();
                celReciboP.VerticalAlign = VerticalAlign.Middle;
                celReciboP.HorizontalAlign = HorizontalAlign.Left;
                if (vista.columnaReciboPago == 1)
                {
                    this.phColumnaReciboPago.Visible = true;
                    tr.Cells.Add(celReciboP);
                }

                TableCell celImporte = new TableCell();
                celImporte.Text = ch.Cheque.importe.ToString("C");
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celNumero = new TableCell();
                celNumero.Text = ch.Cheque.numero.ToString();
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celEntidad = new TableCell();
                celEntidad.Text = ch.Cheque.banco.entidad.ToString();
                celEntidad.VerticalAlign = VerticalAlign.Middle;
                celEntidad.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEntidad);

                TableCell celCuenta = new TableCell();
                celCuenta.Text = ch.Cheque.cuenta.ToString();
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCuenta);

                TableCell celCliente = new TableCell();
                if (!String.IsNullOrEmpty(ch.Cliente))
                    celCliente.Text = ch.Cliente;
                celCliente.Text = ch.Cliente;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celProveedor = new TableCell();
                if (!String.IsNullOrEmpty(ch.Proveedor))
                    celProveedor.Text = ch.Proveedor;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                celProveedor.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celProveedor);

                TableCell celSucursal = new TableCell();
                if (!String.IsNullOrEmpty(ch.sucursalCobro.nombre))
                    celSucursal.Text = ch.sucursalCobro.nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                if (vista.columnaSucursalCobro == 1)
                {
                    this.phColumnaSucCobro.Visible = true;
                    tr.Cells.Add(celSucursal);
                }

                TableCell celSucursalPago = new TableCell();
                if (!String.IsNullOrEmpty(ch.sucursalPago.nombre))
                    celSucursalPago.Text = ch.sucursalPago.nombre;
                celSucursalPago.VerticalAlign = VerticalAlign.Middle;
                celSucursalPago.HorizontalAlign = HorizontalAlign.Left;
                if (vista.columnaSucursalPago == 1)
                {
                    this.phColumnaSucPago.Visible = true;
                    tr.Cells.Add(celSucursalPago);
                }

                TableCell celObservacion = new TableCell();
                celObservacion.Text = this.controlador.obtenerObservacionChequeManual(ch.Cheque.id);
                celObservacion.VerticalAlign = VerticalAlign.Middle;
                celObservacion.HorizontalAlign = HorizontalAlign.Left;
                if (vista.columnaObservacion == 1)
                {
                    this.phColumaObservacion.Visible = true;
                    tr.Cells.Add(celObservacion);
                }

                TableCell celEstado = new TableCell();
                if (ch.Cheque.estado == 1)
                    celEstado.Text = "Disponible";
                if (ch.Cheque.estado == 2)
                    celEstado.Text = "Depositado";
                if (ch.Cheque.estado == 3)
                    celEstado.Text = "Entregado";
                if (ch.Cheque.estado == 4)
                    celEstado.Text = "Disponible";
                if (ch.Cheque.estado == 5)
                    celEstado.Text = "Imputado a Cta.";
                if (ch.Cheque.estado == 6)
                    celEstado.Text = "Debitado";

                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEstado);

                TableCell celTipo = new TableCell();
                celTipo.Text = "";
                if (ch.tipoCheque == 0)
                    celTipo.Text = "Blanco";
                if (ch.tipoCheque == 1)
                    celTipo.Text = "Negro";
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celAccion = new TableCell();

                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + ch.Cheque.id + "_" + ch.idCobro.ToString() + "_" + ch.idPago.ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Click += new EventHandler(this.detalleCobro);

                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + ch.Cheque.id.ToString();
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;

                celAccion.Controls.Add(cbSeleccion);
                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);

                phCheques.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Cheques en PH. " + ex.Message));
            }

        }
        private void agregarCheque()
        {
            try
            {
                ControladorCobranzaEntity contCobranzaEnt = new ControladorCobranzaEntity();
                Gestion_Api.Modelo.Cheque ch = new Gestion_Api.Modelo.Cheque();
                ch.fecha = Convert.ToDateTime(this.txtFechaCAgregar.Text, new CultureInfo("es-AR"));
                ch.importe = Convert.ToDecimal(this.txtImporteAgregar.Text);
                ch.numero = Convert.ToDecimal(this.txtNumeroAgregar.Text);
                ch.librador = this.txtLibradorAgregar.Text;
                ch.cuit = this.txtCuitAgregar.Text;
                ch.cuenta = this.txtCuentaAgregar.Text;
                ch.banco.id = Convert.ToInt32(this.ListBancoAgregar.SelectedValue);

                string observacion = this.txtObservacionAgregar.Text;
                string fechaRecibo = this.txtFechaAgregar.Text;

                int suc = this.sucCobro;
                if (suc == 0)
                    suc = (int)Session["Login_SucUser"];

                int idUser = (int)Session["Login_IdUser"];
                int ok = this.controlador.validarChequeManualExiste(ch.numero.ToString(), ch.banco.id, ch.cuenta);

                if (ok < 0)
                {
                    if (ok == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El numero de cheque ya existe!. "));
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error validando numero de cheque a ingresar."));
                        return;
                    }
                }

                Log.EscribirSQL(idUser, "INFO", "Inicio proceso agregar cheque manual.");
                int i = this.controlador.agregarChequeManual(ch, "M", suc, fechaRecibo, observacion);
                if (i > 0)
                {
                    //Busco el id del cobro que se generó para poder generar el registro en la tabla Cheques_Datos
                    int idCobro = contCobranzaEnt.obtenerIdCobroPorCheque(i);
                    if (idCobro > 0)
                    {
                        Cheques_Datos cd = new Cheques_Datos();
                        cd.Cheque = i;
                        cd.Cobro = idCobro;
                        cd.CobroFtp = 1;

                        int k = contCobranzaEnt.agregarCheques_Datos(cd);
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "ChequesF_2.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar cheque. "));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error agregando cheque. Excepción " + Ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
        private void detalleCobro(object sender, EventArgs e)
        {
            try
            {
                ////obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCobro = atributos[2];
                string idPago = atributos[3];
                string script = "";

                if (idCobro != "0")
                {
                    script = "window.open('../Cobros/ImpresionCobro.aspx?Cobro=" + idCobro + "&valor=2','_blank');";
                }
                if (idPago != "0")
                {
                    script += "window.open('../Pagos/ReportesR.aspx?id=" + idPago + "&valor=2','_blank');";
                }
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message);
            }
        }
        private void imprimirCheques(int agrupado)
        {
            try
            {

                if (agrupado == 1)//reporte agrupado por chk
                {
                    Response.Redirect("/Formularios/Valores/ImpresionValores.aspx?a=9&g=1&valor=1&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + DropListSucursal.SelectedValue + "&SP=" + DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&fdI=" + txtDesdeI.Text + "&fhI=" + txtHastaI.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=" + this.tipoFecha + "&prov=" + this.idProveedor);
                }
                else
                {
                    Response.Redirect("/Formularios/Valores/ImpresionValores.aspx?a=9&g=0&valor=1&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + DropListSucursal.SelectedValue + "&SP=" + DropListSucursalPago.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&fdI=" + txtDesdeI.Text + "&fhI=" + txtHastaI.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=" + this.tipoFecha + "&prov=" + this.idProveedor);
                }
            }
            catch
            {

            }
        }
        private void cargarLabel(string fechaD, string fechaH, int idSucCobro, int idSucPago, int idCliente, int idProveedor, int tipoFecha)
        {
            try
            {
                string label = "";

                if (tipoFecha == 1)
                    label += "Recibido ";
                if (tipoFecha == 2)
                    label += "Entregado ";
                if (tipoFecha == 3)
                    label += "Acreditación ";
                if (tipoFecha == 4)
                    label += "Imputación ";

                label += fechaD + "," + fechaH + ",";

                label += DropListSucursal.Items.FindByValue(idSucCobro.ToString()).Text + ",";

                label += DropListSucursalPago.Items.FindByValue(idSucPago.ToString()).Text + ",";

                this.lblParametros.Text = label;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. Excepción: " + Ex.Message));

            }
        }
        private void inicializarVariables()
        {
            try
            {
                fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaDesdeRecibido.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaHastaRecibido.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaDesdeEntregado.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaHastaEntregado.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDesdeC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtHastaC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDesdeI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtHastaI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                estado = Convert.ToInt32(DropListEstado.SelectedValue);
                DropListSucursal.SelectedValue = sucCobro.ToString();
                DropListSucursalPago.SelectedValue = sucPago.ToString();
                DropListClientes.SelectedValue = idCliente.ToString();
                DropListProveedores.SelectedValue = idProveedor.ToString();
                DropDownListOrigen.SelectedValue = this.origen.ToString();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error inicializando variables. Excepción: " + Ex.Message));
            }
        }

        #endregion

    }
}