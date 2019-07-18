using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMCobros : System.Web.UI.Page
    {
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        controladorSucursal contSucu = new controladorSucursal();
        controladorCobranza contCobranza = new controladorCobranza();
        ControladorCobranzaEntity contCobranzaEnt = new ControladorCobranzaEntity();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        ControladorEmpresa contEmp = new ControladorEmpresa();
        controladorTarjeta contTarjeta = new controladorTarjeta();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity contFactEnt = new controladorFactEntity();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        List<Imputacion> imputaciones = new List<Imputacion>();
        List<Pago> listPagoC = new List<Pago>();
        Pago_Contado pagoC = new Pago_Contado();
        Mensajes mje = new Mensajes();
        Configuracion config = new Configuracion();
        string documentos = "";
        private int idCliente;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private decimal monto;
        private int tipoDoc;
        int posCheques;
        int valor;
        int anticipo;
        string pagares;

        DataTable lstPagoTemp;
        DataTable lstMonedaTemp;
        DataTable lstTransferenciaTemp;
        DataTable lstChequeTemp;
        DataTable lstDocumentosTemp;
        DataTable lstTarjetasTemp;
        DataTable lstRetencionTemp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.anticipo = Convert.ToInt32(Request.QueryString["anticipo"]);

                this.VerificarLogin();
                lbtnAgregarPago.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(lbtnAgregarPago, null) + ";");
                //cargo fechas en campos de fecha
                //obtengo los movimientos
                this.documentos = Request.QueryString["documentos"];
                this.idCliente = Convert.ToInt32(Request.QueryString["cliente"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["empresa"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["sucursal"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);
                this.monto = Convert.ToDecimal(Request.QueryString["monto"].Replace(',', '.'), CultureInfo.InvariantCulture);
                this.tipoDoc = Convert.ToInt32(Request.QueryString["tipo"]);
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.pagares = Request.QueryString["pagares"];

                btnAtras.Attributes.Add("onclick", "history.back(); return false;");
                this.obtenerNroRecibo();
                
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());

                    Session["ABMCobros_PosCheque"] = null;

                    this.txtFechaCh.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaRetencion.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaTransf.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaCobro.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.btnNuevo.Visible = false;
                    this.btnFinalizarPago.Visible = true;
                    lstPagoTemp = new DataTable();
                    lstChequeTemp = new DataTable();
                    lstMonedaTemp = new DataTable();
                    lstTransferenciaTemp = new DataTable();
                    lstDocumentosTemp = new DataTable();
                    lstTarjetasTemp = new DataTable();
                    lstRetencionTemp = new DataTable();

                    this.InicializarListaPagos();
                    this.InicializarListaMoneda();
                    this.InicializarListaTransferencia();
                    this.InicializarListaCheque();
                    this.InicializarListaDocumentos();
                    this.InicializarListaTarjeta();
                    this.InicializarListaRetencion();

                    //Session["ListaPAgos"] = null;
                    this.cargarTipoMoneda();
                    this.cargarBancos();
                    this.cargarBancosTransf();
                    this.cargarOperadores();
                    this.cargarTarjetas();
                    this.cargarTiposRetencion();
                    this.cargarCuentas();
                    //si es un movimiento de cobro
                    if(this.valor == 1)
                    {
                        this.CargarMovimientos(documentos);
                    }
                    //generar pago a cuenta
                    if (this.valor == 2)
                    {
                        this.CargarPagoCuenta();
                    }
                    if(this.tipoDoc == 2)
                    {
                        this.panelRetencion.Visible = false;
                    }                    
                }
                else
                {
                    this.cargarTablaDocumentos();
                    this.cargarTablaPAgos();
                    this.cargarTablaCheques();
                    this.actualizarTotales();                    
                }                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error cargando la pagina.  " + ex.Message));
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
                        if (this.anticipo <= 0)
                        {
                            //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros") != 1)
                            Response.Redirect("/Default.aspx?m=1", false);
                        }
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
                        if (s == "41")
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

        #region inicializar Temporales
        private void InicializarListaPagos()
        {
            try
            {
                lstPagoTemp.Columns.Add("Fila");
                //valores forma pgoa 
                //1 Efectivo
                //2 Cheques
                //3 transf
                //4 tarhetas
                //5 retenciones
                lstPagoTemp.Columns.Add("FormaPago");
                lstPagoTemp.Columns.Add("Tipo Pago");
                lstPagoTemp.Columns.Add("Monto");
                lstPagoTemp.Columns.Add("Cotizacion");
                lstPagoTemp.Columns.Add("Total");
                lstPago = lstPagoTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void InicializarListaDocumentos()
        {
            try
            {
                lstDocumentosTemp.Columns.Add("Id");
                lstDocumentosTemp.Columns.Add("Tipo");
                lstDocumentosTemp.Columns.Add("Numero");
                lstDocumentosTemp.Columns.Add("Saldo");
                lstDocumentosTemp.Columns.Add("Imputar");
                lstDocumentos = lstDocumentosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas de documentos.  " + ex.Message));

            }

        }

        private void InicializarListaMoneda()
        {
            try
            {
                lstMonedaTemp.Columns.Add("Id Pago");
                lstMonedaTemp.Columns.Add("Tipo Pago");
                lstMonedaTemp.Columns.Add("Monto");
                lstMonedaTemp.Columns.Add("Cotizacion");
                lstMonedaTemp.Columns.Add("Total");
                lstMoneda = lstMonedaTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void InicializarListaTarjeta()
        {
            try
            {
                lstTarjetasTemp.Columns.Add("Id Pago");
                lstTarjetasTemp.Columns.Add("Tipo Pago");
                lstTarjetasTemp.Columns.Add("Monto");
                lstTarjetasTemp.Columns.Add("Total");
                lstTarjetas = lstTarjetasTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void InicializarListaTransferencia()
        {
            try
            {
                lstTransferenciaTemp.Columns.Add("Fecha");
                lstTransferenciaTemp.Columns.Add("Importe");
                lstTransferenciaTemp.Columns.Add("Banco");
                lstTransferenciaTemp.Columns.Add("Banco Entidad");
                lstTransferenciaTemp.Columns.Add("Cuenta");
                lstTransferenciaTemp.Columns.Add("Monto");
                lstTransferenciaTemp.Columns.Add("IdCuentaBanc");
                lstTransferencia = lstTransferenciaTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void InicializarListaRetencion()
        {
            try
            {
                lstRetencionTemp.Columns.Add("Fecha");
                lstRetencionTemp.Columns.Add("Importe");
                lstRetencionTemp.Columns.Add("Numero");
                lstRetencionTemp.Columns.Add("Tipo");
                lstRetencionTemp.Columns.Add("Monto");
                lstRetencionTemp.Columns.Add("Origen");
                lstRetencion = lstRetencionTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void InicializarListaCheque()
        {
            try
            {
                lstChequeTemp.Columns.Add("Fecha");
                lstChequeTemp.Columns.Add("Importe");
                lstChequeTemp.Columns.Add("Numero");
                lstChequeTemp.Columns.Add("Banco");
                lstChequeTemp.Columns.Add("Banco Entidad");
                lstChequeTemp.Columns.Add("Cuenta");
                lstChequeTemp.Columns.Add("Cuit");
                lstChequeTemp.Columns.Add("Librador");
                lstChequeTemp.Columns.Add("Monto");
                lstCheque = lstChequeTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        protected DataTable lstPago
        {

            get
            {
                if (ViewState["ListaPagos"] != null)
                {
                    return (DataTable)ViewState["ListaPagos"];
                }
                else
                {
                    return lstPagoTemp;
                }
            }
            set
            {
                ViewState["ListaPagos"] = value;
            }
        }

        protected DataTable lstCheque
        {
            get
            {
                if (ViewState["ListaCheques"] != null)
                {
                    return (DataTable)ViewState["ListaCheques"];
                }
                else
                {
                    return lstChequeTemp;
                }
            }
            set
            {
                ViewState["ListaCheques"] = value;
            }
        }

        protected DataTable lstMoneda
        {

            get
            {
                if (ViewState["ListaMoneda"] != null)
                {
                    return (DataTable)ViewState["ListaMoneda"];
                }
                else
                {
                    return lstMonedaTemp;
                }
            }
            set
            {
                ViewState["ListaMoneda"] = value;
            }
        }

        protected DataTable lstTransferencia
        {
            get
            {
                if (ViewState["ListaTransferencia"] != null)
                {
                    return (DataTable)ViewState["ListaTransferencia"];
                }
                else
                {
                    return lstTransferenciaTemp;
                }
            }
            set
            {
                ViewState["ListaTransferencia"] = value;
            }
        }

        protected DataTable lstRetencion
        {
            get
            {
                if (ViewState["ListaRetencion"] != null)
                {
                    return (DataTable)ViewState["ListaRetencion"];
                }
                else
                {
                    return lstRetencionTemp;
                }
            }
            set
            {
                ViewState["ListaRetencion"] = value;
            }
        }

        protected DataTable lstDocumentos
        {
            get
            {
                if (ViewState["ListaDocumentos"] != null)
                {
                    return (DataTable)ViewState["ListaDocumentos"];
                }
                else
                {
                    return lstDocumentosTemp;
                }
            }
            set
            {
                ViewState["ListaDocumentos"] = value;
            }
        }

        protected DataTable lstTarjetas
        {
            get
            {
                if (ViewState["ListaTarjetas"] != null)
                {
                    return (DataTable)ViewState["ListaTarjetas"];
                }
                else
                {
                    return lstTarjetasTemp;
                }
            }
            set
            {
                ViewState["ListaTarjetas"] = value;
            }
        }
        
        #endregion

        #region cargas Iniciales
        /// <summary>
        /// carga los id de los movimientos a imputar, en el cobro
        /// </summary>
        /// <param name="doc">Id de los mov en Cuenta corriente</param>
        public void CargarMovimientos(string doc)
        {
            try
            {
                if(this.monto == 0)
                {
                    List<MovimientoView> movimiento = this.controlador.obtenerListaMovimientos(doc);
                    decimal totalSaldo = 0;
                    foreach (MovimientoView m in movimiento)
                    {
                            if(!m.tipo.tipo.Contains("Recibo de Cobro"))
                            {
                                totalSaldo += m.saldo;
                            }
                            this.cargarMovimientoDT(m, 0);  
                    }
                    //txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');
                    txtSaldoDoc.Text = totalSaldo.ToString("N");
                }
                else
                {
                    this.CargarMovimientosMonto(doc);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos " + ex.Message));
            }
        }

        public void CargarMovimientosMonto(string doc)
        {
            try
            {
                List<MovimientoView> movimiento = this.controlador.obtenerListaMovimientos(doc);
                decimal totalSaldo = 0;
                decimal montoActual = monto;
                foreach (MovimientoView m in movimiento)
                {
                    if (!m.tipo.tipo.Contains("Recibo de Cobro"))
                    {
                        decimal imputacion = controlador.obtenerMontoImputar(m.saldo, montoActual);
                        montoActual -= imputacion;
                        totalSaldo += m.saldo;
                        this.cargarMovimientoDT(m, imputacion);
                    }
                    else
                    {
                        //rodrigo verifico
                        montoActual -= m.saldo;
                        totalSaldo += m.saldo;

                        this.cargarMovimientoDT(m, 0);
                    }
                }
                totalSaldo += this.generarPagoCuenta(totalSaldo);
                txtSaldoDoc.Text = totalSaldo.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos " + ex.Message));
            }
        }

        public void CargarPagoCuenta()
        {
            try
            {
                try
                {
                    DataTable dt = lstDocumentos;

                    DataRow dr = dt.NewRow();

                    dr["Id"] = dt.Rows.Count;
                    dr["Tipo"] = "Pago a Cuenta N° ";
                    dr["Numero"] = this.obtenerNroPagoCuenta();
                    dr["Saldo"] = monto;
                    dr["Imputar"] = 0;

                    dt.Rows.Add(dr);
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarDocumentoEnPh(dr, pos);
                    lstDocumentos = dt;

                    txtSaldoDoc.Text = monto.ToString("0.00").Replace(',','.');

                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos a DT. " + ex.Message));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos " + ex.Message));
            }
        }

        private string obtenerNroPagoCuenta()
        {
            try
            {
                controladorSucursal cs = new controladorSucursal();
                PuntoVenta pv = cs.obtenerPtoVentaId(this.puntoVenta);
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.contFact.obtenerFacturaNumero(this.puntoVenta, "Pago a Cuenta");
                string numero = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                return numero;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
                return null;
            }
        }

        public decimal generarPagoCuenta(decimal saldo)
        {
            try
            {
                if(this.monto > saldo)
                {
                    DataTable dt = lstDocumentos;

                    DataRow dr = dt.NewRow();

                    dr["Id"] = dt.Rows.Count;
                    dr["Tipo"] = "Pago a Cuenta N° ";
                    dr["Numero"] = this.obtenerNroPagoCuenta();
                    dr["Saldo"] = this.monto - saldo;
                    dr["Imputar"] = 0;

                    dt.Rows.Add(dr);
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarDocumentoEnPh(dr, pos);
                    lstDocumentos = dt;

                    return this.monto - saldo;
                }
                return 0;

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));
                return 0;
            }
        }

        //carga el movimiento en la tabla de documentos imputados de 
        public void cargarMovimientoDT(MovimientoView movV, decimal imputacion)
        {
            try
            {
                DataTable dt = lstDocumentos;

                DataRow dr = dt.NewRow();
                dr["Id"] = movV.id;
                dr["Tipo"] = movV.tipo.tipo;
                dr["Numero"] = movV.numero;
                dr["Saldo"] = movV.saldo;
                dr["Imputar"] = imputacion;

                dt.Rows.Add(dr);
                int pos = dt.Rows.IndexOf(dr);
                this.cargarDocumentoEnPh(dr, pos);
                lstDocumentos = dt;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos a DT. " + ex.Message));
            }
        }

        private void cargarDocumentoEnPh(DataRow dr,int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = dr["Id"].ToString();

                //Celdas

                TableCell celNumero = new TableCell();
                celNumero.Text = dr["Tipo"].ToString() + " " + dr["Numero"].ToString().PadLeft(8,'0'); 
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.Width = Unit.Percentage(70);
                tr.Cells.Add(celNumero);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$ " + dr["Saldo"].ToString().Replace(',', '.');
                celSaldo.Width = Unit.Percentage(15);
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSaldo);

                TableCell celImputar = new TableCell();
                TextBox txtImputar = new TextBox();
                txtImputar.CssClass = "form-control";
                //txtImputar.AutoPostBack = true;
                txtImputar.ID = "txtImputar_" + dr["Id"].ToString();
                if (Convert.ToDecimal(dr["Imputar"]) == 0)
                {
                    txtImputar.Text = dr["Saldo"].ToString().Replace(',', '.');
                }
                if (Convert.ToDecimal(dr["Imputar"]) != 0)
                {
                    txtImputar.Text = dr["Imputar"].ToString().Replace(',', '.');
                }
                txtImputar.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtImputar.ID = "Text_" + pos.ToString();
                txtImputar.TextChanged += new EventHandler(VerificarImputacion);
                txtImputar.AutoPostBack = true;
                txtImputar.Style.Add("text-align", "right");
                //txtImputar.TextChanged += new EventHandler(this.ActualizarTotales);
                txtImputar.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtImputar.Attributes.Add("onchange", "javascript:TotalImputado()");
                if (dr["Tipo"].ToString().Contains("Recibo de Cobro") && this.valor == 1)
                {
                    txtImputar.Attributes.Add("Disabled", "true");
                }
                celImputar.Controls.Add(txtImputar);
                celImputar.Width = Unit.Percentage(15);
                celImputar.HorizontalAlign = HorizontalAlign.Right;
                celImputar.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celImputar);

                phDocumentos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando documentos. " + ex.Message));
            }

        }

        public void ActualizarTotales(object sender, EventArgs e)
        {

            try
            {
                decimal suma = 0;
                foreach (Control C in phDocumentos.Controls)
                {
                    TableRow tr = C as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    decimal saldo = Convert.ToDecimal(tr.Cells[1].Text.Substring(1));
                    txt.Text = txt.Text.Replace('.', ',');
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        if (saldo >= Convert.ToDecimal(txt.Text))
                        {
                            suma += Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El monto a imputar es mayor al saldo del documento"));
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El monto a imputar es mayor al saldo del documento"));
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("factura agregada", null));
                        }

                    }
                }
                //txtTotalDoc.Text = "$ " + suma.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error actualizando totales. " + ex.Message));
            }
        }

        public void cargarTipoMoneda()
        {
            try
            {
                DataTable dt = contCobranza.obtenerMonedasDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["moneda"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListTipo.DataSource = dt;
                this.DropListTipo.DataValueField = "id";
                this.DropListTipo.DataTextField = "moneda";
                this.DropListTipo.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando monedas a la lista. " + ex.Message));
            }
        }

        public void cargarTiposRetencion()
        {
            try
            {
                DataTable dt = contCobranza.obtenerTiposRetencionDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["tipo"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListTipoRetencion.DataSource = dt;
                this.DropListTipoRetencion.DataValueField = "id";
                this.DropListTipoRetencion.DataTextField = "tipo";
                this.DropListTipoRetencion.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando tipos de Retencion a la lista. " + ex.Message));
            }
        }

        public void cargarOperadores()
        {
            try
            {
                List<Gestion_Api.Entitys.Operadores_Tarjeta> operadores = this.contTarjeta.obtenerOperadores();

                this.ListOperadores.DataSource = operadores;
                this.ListOperadores.DataValueField = "Id";
                this.ListOperadores.DataTextField = "Operador";
                this.ListOperadores.DataBind();

                this.ListOperadores.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando operadores. " + ex.Message));

            }
        }
        public void cargarTarjetasByOperador(int idOperador)
        {
            try
            {
                List<Gestion_Api.Entitys.Tarjeta> tarjetas = this.contTarjeta.obtenerTarjetasEntityByOperador(idOperador);
                tarjetas = tarjetas.OrderBy(x => x.nombre).ToList();
                this.ListTarjetas2.DataSource = tarjetas;
                this.ListTarjetas2.DataValueField = "id";
                this.ListTarjetas2.DataTextField = "nombre";
                this.ListTarjetas2.DataBind();

                this.ListTarjetas2.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        public void cargarTarjetas()
        {
            try
            {
                controladorTarjeta contTarj = new controladorTarjeta();
                DataTable dt = contTarj.obtenerTarjetasDT();


                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListTarjetas.DataSource = dt;
                this.ListTarjetas.DataValueField = "id";
                this.ListTarjetas.DataTextField = "nombre";

                this.ListTarjetas.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando tarjetas. " + ex.Message));
            }
        }

        public void cargarBancos()
        {
            try
            {
                DataTable dt = contCobranza.obtenerBancosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListBancoCh.DataSource = dt;
                this.DropListBancoCh.DataValueField = "id";
                this.DropListBancoCh.DataTextField = "entidad";
                this.DropListBancoCh.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando bancos a la lista. " + ex.Message));
            }
        }

        public void cargarBancosTransf()
        {
            try
            {
                DataTable dt = contCobranza.obtenerBancosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListBancoTransf.DataSource = dt;
                this.DropListBancoTransf.DataValueField = "id";
                this.DropListBancoTransf.DataTextField = "entidad";
                this.DropListBancoTransf.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando bancos a la lista. " + ex.Message));
            }
        }

        private void cargarCuentas()
        {
            try
            {

                ControladorBanco contBanco = new ControladorBanco();
                var cuentas = contBanco.obtenerCuentasBancarias();
                this.DropListCuentas.SelectedIndex = 0;
                this.DropListCuentas.Items.Add("Seleccione...");
                foreach (var c in cuentas)
                {
                    this.DropListCuentas.Items.Add(c.Id + " | " + c.Banco1.entidad + " | " + c.Descripcion + " | " + c.Numero + " | " + c.Cuit + " | " + c.Librador);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cuentas a la lista. " + ex.Message));
            }
        }

        #endregion

        /// <summary>
        /// Carga la info del DatarOW en el place holder de PAgos
        /// </summary>
        /// <param name="dr">Fila con datos a cargar</param>
        public void cargarEnPH(DataRow dr, int pos)
        {
            try
            {
                TableRow tr = new TableRow();
                //tr.ID = pgView.id.ToString();

                TableCell celMoneda = new TableCell();
                celMoneda.Text = dr["Tipo Pago"].ToString();
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMoneda);

                TableCell celMonto = new TableCell();
                celMonto.Text = "$ " + dr["Monto"].ToString().Replace(',', '.');
                celMonto.VerticalAlign = VerticalAlign.Middle;
                celMonto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celMonto);

                TableCell celCotizacion = new TableCell();
                celCotizacion.Text = dr["Cotizacion"].ToString().Replace(',', '.');
                celCotizacion.VerticalAlign = VerticalAlign.Middle;
                celCotizacion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCotizacion);

                //decimal montoC = Convert.ToDecimal(dr[1]) * Convert.ToDecimal(dr[2]);
                TableCell celTotal = new TableCell();
                celTotal.Text = "$ " + dr["Total"].ToString().Replace(',','.');
                celTotal.Width = Unit.Percentage(20);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celAccion = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + pos.ToString();
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarPago);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phEfectivo.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista de Pagos en PH. " + ex.Message));

            }
        }
        //elimina el pago del temporal de pagos y del temporal de la forma de pago
        private void QuitarPago(object sender, EventArgs e)
        {
            try
            {
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                //obtengo el pedido del session
                DataTable dt = lstPago;
                if (Session["ABMCobros_PosCheque"] != null)
                {
                    posCheques = (int)Session["ABMCobros_PosCheque"];
                }
                else
                {
                    posCheques = -1;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    if (dt.Rows.IndexOf(dr).ToString() == codigo[1])
                    {
                        if(posCheques == Convert.ToInt32(codigo[1]))
                        {
                            //quito el registro cheques y los cheques
                            DataTable dtCheques = lstCheque;
                            dtCheques.Rows.Clear();
                            lstCheque = dtCheques;
                            this.cargarTablaCheques();
                            Session.Add("ABMCobros_PosCheque", -1);
                            dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));
                            break;
                        }
                        else
                        {
                            //quito del temporal a la forma de pago que corresponde
                            int forma = Convert.ToInt32(dr["FormaPago"]);
                            int fila = Convert.ToInt32(dr["Fila"]);
                            this.quitarPagodeTemp(fila, forma);

                            //lo quito del temporla de pagos
                            dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));

                            break;
                        }
                    }
                }

                //cargo el nuevo pedido a la sesion
                lstPago = dt;

                //vuelvo a cargar los items
                this.cargarTablaPAgos();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error quitando pago. " + ex.Message));
            }
        }

        public void quitarPagodeTemp(int fila, int forma)
        {
            try
            {
                //quito efectivo
                if (forma == 1)
                {
                    DataTable dtMoneda = this.lstMoneda;
                    dtMoneda.Rows.RemoveAt(fila);
                    this.lstMoneda = dtMoneda;
                }
                //quito Cheques
                if (forma == 2)
                {
                    DataTable dtCheques = this.lstCheque;
                    dtCheques.Rows.RemoveAt(fila);
                    this.lstCheque = dtCheques;
                }
                //quito Transferencia
                if (forma == 3)
                {
                    DataTable dtTransf = this.lstTransferencia;
                    dtTransf.Rows.RemoveAt(fila);
                    this.lstTransferencia = dtTransf;
                }
                //quito Tarjetas
                if (forma == 4)
                {
                    DataTable dtTarj = this.lstTarjetas;
                    dtTarj.Rows.RemoveAt(fila);
                    this.lstTarjetas = dtTarj;
                }
                //quito Retencion
                if (forma == 5)
                {
                    DataTable dtRet = this.lstRetencion;
                    dtRet.Rows.RemoveAt(fila);
                    this.lstRetencion = dtRet;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error quitando pago. " + ex.Message));
            }
        }

        public void cargarEnPHCheque(DataRow dr, int pos)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celMoneda = new TableCell();
                DateTime fecha = Convert.ToDateTime(dr["Fecha"].ToString());
                celMoneda.Text = fecha.ToString("dd/MM/yyyy");
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                celMoneda.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celMoneda);

                TableCell celMonto = new TableCell();
                celMonto.Text = "$ " + dr["Importe"].ToString().Replace(',','.');
                celMonto.HorizontalAlign = HorizontalAlign.Right;
                celMonto.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMonto);

                TableCell celNumero = new TableCell();
                celNumero.Text = dr["Numero"].ToString();
                //celNumero.Width = Unit.Percentage(20);
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celBanco = new TableCell();
                celBanco.Text = dr["Banco Entidad"].ToString();
                //celBanco.Width = Unit.Percentage(20);
                celBanco.HorizontalAlign = HorizontalAlign.Left;
                celBanco.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celBanco);

                TableCell celCuenta = new TableCell();
                celCuenta.Text = dr["Cuenta"].ToString();
                //celCuenta.Width = Unit.Percentage(20);
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCuenta);

                TableCell celAccion2 = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.ID = "btnEditar_" + pos;
                btnEditar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEditar.Click += new EventHandler(this.EliminarCheque);
                celAccion2.Controls.Add(btnEditar);
                celAccion2.Width = Unit.Percentage(5);
                celAccion2.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion2);
                phCheques.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando cheques a la lista. " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error agregando cheques a la lista  " + dr["Fecha"].ToString() + " - " + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        private void EliminarCheque(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = lstCheque;
                string[] Cheque = (sender as LinkButton).ID.Split('_');
                int posCheque = Convert.ToInt32(Cheque[1]);
                DataRow dr = dt.Rows[posCheque];
                dt.Rows.Remove(dr);
                lstCheque = dt;

                DataTable dtPagos = lstPago;
                posCheques = (int)Session["ABMCobros_PosCheque"];
                DataRow drPago = dtPagos.Rows[posCheques];
                drPago["Monto"] = this.actualizarTotalCheque();
                drPago["Total"] = this.actualizarTotalCheque();
                dt.Rows.RemoveAt(posCheques);
                dt.Rows.InsertAt(drPago, posCheques);
                posCheques = dt.Rows.IndexOf(drPago);
                Session.Add("ABMCobros_PosCheque", posCheques);

                this.cargarTablaCheques();
                this.cargarTablaPAgos();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error editando cheque. " + ex.Message));
            }
        }
        //agrega efectivo a los pagos
        protected void btnAgregarPagoM_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && Convert.ToDecimal(txtSaldoDoc.Text) >= Convert.ToDecimal("0.00") ))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    Pago_Contado p = new Pago_Contado();
                    p.moneda.id = Convert.ToInt32(this.DropListTipo.SelectedValue);
                    p.moneda.moneda = this.DropListTipo.SelectedItem.Text;
                    decimal monto = Convert.ToDecimal(txtMonto.Text.ToString().Replace(',', '.'),CultureInfo.InvariantCulture);
                    p.monto = decimal.Round(monto, 2);
                    p.moneda.cambio = Convert.ToDecimal(txtCotizacion.Text.Replace(',', '.'), CultureInfo.InvariantCulture);

                    //Guardo la info de monedas en el DT Temporal
                    DataTable dtMoneda = this.lstMoneda;
                    DataRow drMoneda = dtMoneda.NewRow();
                    drMoneda["Id Pago"] = p.moneda.id;
                    drMoneda["Tipo Pago"] = p.moneda.moneda;
                    drMoneda["Monto"] = p.monto;
                    drMoneda["Cotizacion"] = p.moneda.cambio;
                    drMoneda["Total"] = decimal.Round(p.monto * p.moneda.cambio, 2);

                    dtMoneda.Rows.Add(drMoneda);
                    //guardo la fila y lo agrego al temporal con los pagos
                    fila = dtMoneda.Rows.IndexOf(drMoneda);

                    lstMoneda = dtMoneda;

                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = p.moneda.moneda;
                    dr["Monto"] = p.monto;
                    dr["Cotizacion"] = p.moneda.cambio;
                    dr["Total"] = decimal.Round(p.monto * p.moneda.cambio, 2);

                    dr["FormaPago"] = 1;
                    dr["Fila"] = fila;

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposM();
                    this.actualizarTotales();
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Debe ingresar importes a imputar"));
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe ingresar importes a imputar\", {type: \"warning\"});", true);
                    this.limpiarCamposM();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));

            }
        }

        protected void btnAgregarPagoCh_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                int okNroCheque = contCobranza.validarChequeManualExiste(this.txtNumeroCh.Text, Convert.ToInt32(DropListBancoCh.SelectedValue), this.txtCuentaCh.Text);
                if (okNroCheque <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"El nro de cheque ya fue ingresado previamente! \", {type: \"error\"});", true);
                    return;
                }
                if (contCobranza.validateCuit(this.txtCuitCh.Text))//&& 
                {
                    if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                    //if (TotalDoc.Value != "0.00")
                    {
                        //genero la clase
                        decimal monto = Convert.ToDecimal(txtImporteCh.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        DataTable dtCheque = lstCheque;
                        DataRow drCheque = dtCheque.NewRow();
                        string fecha = Convert.ToDateTime(txtFechaCh.Text, new CultureInfo("es-AR")).ToString();//.ToString("dd/MM/YYYY");
                        drCheque["Fecha"] = fecha;
                        drCheque["Importe"] = decimal.Round(monto, 2);
                        drCheque["Numero"] = Convert.ToDecimal(txtNumeroCh.Text);
                        drCheque["Banco"] = Convert.ToInt32(DropListBancoCh.SelectedValue);
                        drCheque["Banco Entidad"] = DropListBancoCh.SelectedItem.Text;
                        drCheque["Cuenta"] = txtCuentaCh.Text.ToString();
                        drCheque["Cuit"] = txtCuitCh.Text;
                        drCheque["Librador"] = txtLibradorCh.Text;
                        drCheque["Monto"] = decimal.Round(monto, 2);
                        dtCheque.Rows.Add(drCheque);
                        //Guardo la fila
                        fila = dtCheque.Rows.IndexOf(drCheque);

                        lstCheque = dtCheque;

                        DataTable dt = lstPago;

                        if (Session["ABMCobros_PosCheque"] == null)
                        {
                            Session.Add("ABMCobros_PosCheque", -1);
                        }
                        posCheques = (int)Session["ABMCobros_PosCheque"];

                        if (posCheques == -1)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Tipo Pago"] = "Cheques ";
                            dr["Monto"] = decimal.Round(monto, 2);
                            dr["Cotizacion"] = 1;
                            dr["Total"] = decimal.Round(monto, 2);
                            dr["FormaPago"] = 2;
                            dr["Fila"] = fila;

                            dt.Rows.Add(dr);

                            posCheques = dt.Rows.IndexOf(dr);
                            Session.Add("ABMCobros_PosCheque", posCheques);
                        }
                        else
                        {
                            posCheques = (int)Session["ABMCobros_PosCheque"];
                            DataRow dr = dt.Rows[posCheques];
                            DataRow drNueva = dt.NewRow();
                            drNueva["Tipo Pago"] = "Cheques ";
                            drNueva["Monto"] = this.actualizarTotalCheque();
                            drNueva["Cotizacion"] = 1;
                            drNueva["Total"] = this.actualizarTotalCheque();
                            dt.Rows.Remove(dr);
                            dt.Rows.InsertAt(drNueva, posCheques);
                            posCheques = dt.Rows.IndexOf(drNueva);
                            Session.Add("ABMCobros_PosCheque", posCheques);
                        }

                        lstPago = dt;
                        //llamo al metodo que muestra los pagos en la tabla
                        this.cargarTablaPAgos();
                        this.cargarTablaCheques();
                        this.limpiarCamposCh();
                        this.actualizarTotales();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes a imputar"));
                        this.limpiarCamposCh();
                    }
                }
                else
                {
                    this.lblErrorCuit.Visible = true;
                    this.txtCuitCh.Focus();
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"No se pudo agregar pago de cheque  " + ex.Message + "\", {type: \"warning\"});", true);
            }
        }

        /// <summary>
        /// Agrega pago Transferencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarPagoTrans_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    decimal monto = Convert.ToDecimal(txtImporteTransf.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    DataTable dtTransferencia = lstTransferencia;
                    DataRow drTransferencia = dtTransferencia.NewRow();
                    string fecha = Convert.ToDateTime(txtFechaTransf.Text, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                    drTransferencia["Fecha"] = fecha;
                    drTransferencia["Importe"] = decimal.Round(monto, 2);
                    drTransferencia["Banco"] = Convert.ToInt32(DropListBancoTransf.SelectedValue);
                    drTransferencia["Banco Entidad"] = DropListBancoTransf.SelectedItem.Text;
                    drTransferencia["Cuenta"] = txtCuentaTransf.Text;
                    //drTransferencia["CBU"] = txtCbu.Text;
                    drTransferencia["IdCuentaBanc"] = DropListCuentas.SelectedItem.Text.Split('|')[0];
                    drTransferencia["Monto"] = decimal.Round(monto, 2);
                    dtTransferencia.Rows.Add(drTransferencia);

                    //guardo la fila y lo agrego al temporal con los pagos
                    fila = dtTransferencia.Rows.IndexOf(drTransferencia);

                    lstTransferencia = dtTransferencia;

                    

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Transferencia N° " + txtCuentaTransf.Text;
                    dr["Monto"] = decimal.Round(monto, 2);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(monto, 2);
                    //agrego la forma y la fila en pagos
                    dr["FormaPago"] = 3;
                    dr["Fila"] = fila;


                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposTransf();
                    this.actualizarTotales();
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes a imputar"));
                    this.limpiarCamposCh();
                    this.limpiarCamposTransf();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        protected void btnAgregarPagoTarjeta_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    Tarjeta t = this.contTarjeta.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas2.SelectedValue));
                    //genero la clase
                    Pago_Tarjeta ptarjeta = new Pago_Tarjeta();
                    ptarjeta.tarjeta.id = Convert.ToInt32(this.ListTarjetas2.SelectedValue);
                    ptarjeta.tarjeta.nombre = this.ListTarjetas2.SelectedItem.Text;
                    decimal monto = Convert.ToDecimal(txtMontoTarjeta.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    if (t.recargo > 0)
                        monto = decimal.Round(monto * (1 + (t.recargo / 100)), 2);

                    ptarjeta.monto = decimal.Round(monto, 2);
                    //ptarjeta.tarjeta.recargo = Convert.ToDecimal(this.txtRecargo.Text);

                    //Guardo la info de monedas en el DT Temporal
                    DataTable dtTarjeta = this.lstTarjetas;
                    DataRow drTarjeta = dtTarjeta.NewRow();
                    drTarjeta["Id Pago"] = ptarjeta.tarjeta.id;
                    drTarjeta["Tipo Pago"] = ptarjeta.tarjeta.nombre;
                    drTarjeta["Monto"] = ptarjeta.monto;
                    drTarjeta["Total"] = decimal.Round(ptarjeta.monto, 2);

                    dtTarjeta.Rows.Add(drTarjeta);
                    //guardo la fila
                    fila = dtTarjeta.Rows.IndexOf(drTarjeta);

                    lstTarjetas = dtTarjeta;

                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = ptarjeta.tarjeta.nombre;
                    dr["Monto"] = ptarjeta.monto;
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(ptarjeta.monto, 2);

                    //agrego la forma y la fila en pagos
                    dr["FormaPago"] = 4;
                    dr["Fila"] = fila;

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposTarjeta();
                    this.actualizarTotales();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Debe ingresar importes a imputar"));
                    this.limpiarCamposM();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        protected void btnAgregarPago_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    if (this.valor == 1)
                    {
                        this.hacerCobro();
                    }
                    if (this.valor == 2)
                    {
                        this.hacerCobroPagoCuenta();
                    }
                }
                else
                {
                    this.lbtnAgregarPago.Attributes.Remove("Disabled");                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar Cobro.  " + ex.Message));

            }
        }

        #region Pago retencion
        protected void btnAgregoPagoRetencion_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    decimal monto = Convert.ToDecimal(txtRetencion.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    DataTable dtRetencion = lstRetencion;
                    DataRow drRetencion = dtRetencion.NewRow();
                    drRetencion["Fecha"] = Convert.ToDateTime(txtFechaRetencion.Text, new CultureInfo("es-AR"));
                    drRetencion["Importe"] = decimal.Round(monto, 2);
                    drRetencion["Tipo"] = Convert.ToInt32(DropListTipoRetencion.SelectedValue);
                    drRetencion["Numero"] = txtNumeroRetencion.Text;
                    //drTransferencia["CBU"] = txtCbu.Text;
                    drRetencion["Monto"] = decimal.Round(monto, 2);
                    drRetencion["Origen"] = "C";
                    
                    dtRetencion.Rows.Add(drRetencion);

                    //guardo la fila
                    fila = dtRetencion.Rows.IndexOf(drRetencion);

                    lstRetencion = dtRetencion;

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Retencion N° " + txtNumeroRetencion.Text;
                    dr["Monto"] = decimal.Round(monto, 2);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(monto, 2);
                    //agrego la forma y la fila en pagos
                    dr["FormaPago"] = 5;
                    dr["Fila"] = fila;
                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposRetencion();
                    this.actualizarTotales();
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes a imputar"));
                    this.limpiarCamposRetencion();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        #endregion

        #region limpiar campos
        public void limpiarCamposM()
        {
            try
            {
                DropListTipo.SelectedIndex = -1;
                txtMonto.Text = "";
                txtCotizacion.Text = "";
            }
            catch
            {

            }
        }

        public void limpiarCamposTarjeta()
        {
            try
            {
                ListTarjetas.SelectedIndex = -1;
                ListTarjetas2.SelectedIndex = -1;
                txtMontoTarjeta.Text = "";
            }
            catch
            {

            }
        }

        public void limpiarCamposCh()
        {
            try
            {
                txtImporteCh.Text = "";
                txtNumeroCh.Text = "";
                DropListBancoCh.SelectedValue = "-1";
                txtCuentaCh.Text = "";
                txtCuitCh.Text = "";
                txtLibradorCh.Text = "";
                this.lblErrorCuit.Visible = false;
            }
            catch
            {

            }

        }

        public void limpiarCamposTransf()
        {
            try
            {
                txtImporteTransf.Text = "";
                txtCuentaTransf.Text = "";
                //txtCbu.Text = "";
                this.DropListBancoTransf.SelectedValue = "-1";
                this.DropListCuentas.SelectedValue = "-1";

            }
            catch
            {

            }
        }

        public void limpiarCamposRetencion()
        {
            try
            {
                txtRetencion.Text = "";
                txtNumeroRetencion.Text = "";
                //txtCbu.Text = "";
                DropListTipoRetencion.SelectedValue = "-1";

            }
            catch
            {

            }
        }
        #endregion
        public string calcularTotal()
        {
            try
            {
                DataTable dt = lstPago;
                decimal saldo = 0;
                foreach(DataRow dr in dt.Rows)
                {
                    saldo += Convert.ToDecimal(dr["Total"]);
                }
                return "$ " + saldo.ToString(); 
            }
            catch
            {
                return null;
            }
        }

        #region cargar tablas temporales
        private void cargarTablaPAgos()
        {
            try
            {
                DataTable dt = this.lstPago;

                //limpio el Place holder
                this.phEfectivo.Controls.Clear();
                //decimal saldo = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarEnPH(dr, pos);
                } 
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista Pagos " + ex.Message));
            }

        }

        private void cargarTablaDocumentos()
        {
            try
            {
                DataTable dt = this.lstDocumentos;

                //limpio el Place holder
                this.phDocumentos.Controls.Clear();
                decimal totalSaldo = 0;
                int pos = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if(!dr["Tipo"].ToString().Contains("Recibo de Cobro"))
                    {
                        totalSaldo += Convert.ToDecimal(dr["Saldo"]);
                    }
                    pos = dt.Rows.IndexOf(dr);
                    this.cargarDocumentoEnPh(dr,pos);
                }

                txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista de Documentos.  " + ex.Message));
            }

        }

        private void cargarTablaCheques()
        {
            try
            {

                DataTable dt = lstCheque;

                //limpio el Place holder
                this.phCheques.Controls.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    //que me cargue la tabla, recibiendo una clase PAgo_contado
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarEnPHCheque(dr, pos);

                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error cargando lista cheques  " + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        #endregion
       
        private decimal actualizarTotalCheque()
        {
            try
            {
                DataTable dt = lstCheque;

                //limpio el Place holder
                decimal total = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    total += Convert.ToDecimal(dr["Importe"]);
                }

                return decimal.Round(total, 2);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques " + ex.Message));
                return -1;
            }
        }

        public List<Imputacion> obtenerImputaciones()
        {
            try
            {
                foreach (Control c in phDocumentos.Controls)
                {
                    TableRow tr = c as TableRow;
                    Imputacion imp = new Imputacion();
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if (!tr.Cells[0].Text.Contains("Pago a Cuenta"))
                    {
                        imp.movimiento = controlador.obtenerMovimientoID(Convert.ToInt32(tr.ID));
                        imp.total = Convert.ToDecimal(tr.Cells[1].Text.Substring(1).Replace(',', '.'), CultureInfo.InvariantCulture);
                        if (!String.IsNullOrEmpty(txt.Text))
                        {
                            if (!tr.Cells[0].Text.Contains("Recibo de Cobro") && imp.movimiento.saldo > imp.total)
                            {
                                decimal resto = imp.movimiento.saldo - imp.total;
                                imp.imputar = resto + Convert.ToDecimal(txt.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                imp.imputar = Convert.ToDecimal(txt.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            imp.imputar = 0;
                        }

                        imputaciones.Add(imp);
                    }
                    
                }
                return imputaciones;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ha ocurrido un error obteniendo Lista de Imputaciones. " + ex.Message));
                return null;
            }
        }

        protected void DropListTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCotizacion.Text = contCobranza.obtenerCotizacion(Convert.ToInt32(DropListTipo.SelectedValue)).ToString().Replace(',', '.');
                txtMonto.Focus();
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo cotización. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error obteniendo cotizacion al realizar cobro. " + ex.Message);
            }
        }

        private void obtenerNroRecibo()
        {
            try
            {
                //como estoy en cotizacion pido el ultimo numero de este documento
                PuntoVenta pv = contSucu.obtenerPtoVentaId(this.puntoVenta);
                int nro = this.contCobranza.obtenerReciboNumero(this.puntoVenta, "Recibo de Cobro - FC");
                if (this.config.numeracionCobros == "0")
                {
                    this.txtNumeroCobro.Attributes.Remove("disabled");
                }
                else
                {
                    this.txtNumeroCobro.Text = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Cobro. " + ex.Message));
            }
        }

        #region finalizar Cobro
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtFechaCobro.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.obtenerNroRecibo();
                //this.txtTotalImputadoCobro.Text = "$" + txtTotalDoc.Text;
                //this.txtTotalCobro.Text = txtTotal.Text;

                //(sender as Button).PostBackUrl = "#modalFacturaDetalle";

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "function clickButton()  {  document.getElementById('abreDialog').click()  }");
                ////ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "function clickButton()  {  alert('hola');  }", true);

                //modalFacturaDetalle.Visible = true;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }


        private void hacerCobro()
        {
            try
            {
                //obtengo las imputaciones
                string imputado = this.TotalDoc.Value;
                List<Imputacion> imputaciones = obtenerImputaciones();
                //Configuracion config = new Configuracion();
                if (imputaciones != null)
                {
                    //si hay imputaciones ingreso los pagos
                    List<Pago> listPago = contCobranza.obtenerPagosdesdeDT(lstMoneda, lstCheque, lstTransferencia, lstTarjetas, lstRetencion);
                    if (listPago.Count > 0 & listPago != null)
                    {
                        Cobro cobro = new Cobro();
                        //cobro.fecha = DateTime.Now;
                        cobro.fecha = Convert.ToDateTime(this.txtFechaCobro.Text, new CultureInfo("es-AR"));
                        cobro.cliente.id = this.idCliente;
                        cobro.Doc_imputar = imputaciones;
                        cobro.pagos = listPago;
                        //cobro.puntoVenta.puntoVenta = Session["PuntoVentaCobranza"] as string;  
                        cobro.empresa.id = this.idEmpresa;
                        cobro.sucursal.id = this.idSucursal;
                        cobro.puntoVenta = contSucu.obtenerPtoVentaId(this.puntoVenta);
                        cobro.total = Convert.ToDecimal(this.txtTotalIngresado.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                        //string imputado = this.txtTotalDoc.Text.Replace('.', ',');
                        cobro.imputado = Convert.ToDecimal(imputado.Replace(',', '.'), CultureInfo.InvariantCulture);
                        cobro.ingresado = Convert.ToDecimal(this.txtTotalIngresado.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                        cobro.comentarios = this.txtObservaciones.Text;
                        if (this.config.numeracionCobros == "0")
                        {
                            cobro.numero = this.txtNumeroCobro.Text;
                        }

                        //agrego el tipo de documento que se imputa                        
                        if (this.tipoDoc == 1)
                        {
                            cobro.tipoDocumento.tipo = "Factura";
                        }
                        if (this.tipoDoc == 2)
                        {
                            cobro.tipoDocumento.tipo = "Presupuesto";
                        }

                        if (cobro.ingresado >= cobro.imputado)
                        {
                            int i = contCobranza.ProcesarCobro(cobro, -1, tipoDoc);
                            if (i > 0)
                            {
                                //Verifico si se generó el cobro con algún cheque y lo guardo en la tabla Cheques_Datos
                                if (lstCheque.Rows.Count > 0)
                                {
                                    var listCheques = this.contCobranzaEnt.obtenerChequesPorIdCobro(cobro.id);
                                    foreach (int idCheque in listCheques)
                                    {
                                        Gestion_Api.Entitys.Cheques_Datos chd = new Gestion_Api.Entitys.Cheques_Datos();
                                        chd.Cheque = idCheque;
                                        chd.Cobro = cobro.id;
                                        chd.CobroFtp = 0;
                                        if (this.tipoDoc == 2)
                                            chd.CobroFtp = 1;

                                        int k = this.contCobranzaEnt.agregarCheques_Datos(chd);
                                    }
                                }

                                lstPago = null;
                                lstCheque = null;
                                lstMoneda = null;
                                lstTransferencia = null;
                                this.btnNuevo.Visible = true;
                                this.btnFinalizarPago.Visible = false;
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Genero el Recibo de Cobro N°" + this.txtNumeroCobro.Text);

                                //Verifico si hay que liquidar pagarés. Si no hay que liquidar, el entero es = 0
                                int l = this.liquidarPagares(i);
                               
                                int c = contCobranza.enviarSmsCobro(idCliente, (int)Session["Login_IdUser"], cobro);
                                
                                if (l > 0)
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Cobro agregado. Se liquidaron correctamente los pagarés en el cobro \", {type: \"info\"}); location.href = '../Valores/PagaresF.aspx';", true);
                                if (l < 0)
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudieron liquidar los pagarés en el cobro. \", {type: \"alert\"}); window.open('ImpresionCobro.aspx?Cobro=" + i + "&valor=2');location.href = 'CobranzaF.aspx';", true);

                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", "window.open('ImpresionCobro.aspx?Cobro=" + i + "&valor=2', 'fullscreen', 'top=0,left=0,width=' + (screen.availWidth) + ',height =' + (screen.availHeight) + ',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0'); location.href = 'CobranzaF.aspx';", true);
                                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Cobro agregado. \", {type: \"info\"}); window.open('ImpresionCobro.aspx?Cobro=" + i + "&valor=2');location.href = 'CobranzaF.aspx';", true);
                                mostrarMensaje(c, cobro.id,i);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudo generar cobro. \");", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"El cobro es mayor a lo imputado. \");", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El cobro es mayor a lo imputado"));
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se cargaron pagos. \");", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se cargaron pagos. "));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudo obtener imputaciones. \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargaron imputaciones "));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error generando cobro. " + ex.Message), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar cobro  " + ex.Message));
            }
        }

        /*smsCobro devuelve > que 0 si pudo enviar el mensaje y 
         * procesaCobro devuelve > 0 tambien si pudo realizar correctamente el cobro
         * cobro es el numero id del cobro realizado
         * */
        private void mostrarMensaje(int smsCorrecto,int idCobro, int cobroCorrecto)
        {
            try
            {
                if (cobroCorrecto > 0)//si pudo realizarse el pago
                {
                    if (smsCorrecto > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Cobro agregado. Se ha enviado correctamente el sms \", {type: \"info\"}); location.href = 'CobranzaF.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Cobro agregado. Ocurrió un error enviando el sms \", {type: \"info\"}); location.href = 'CobranzaF.aspx';", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Cobro no realizado. \", {type: \"info\"})');location.href = 'CobranzaF.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Ocurrio un error en la funcion: mostrarMensaje. Ex: "+ex.Message+" \");", true);
            }
            
        }

        private void hacerCobroPagoCuenta()
        {
            try
            {
                if (this.anticipo > 0)
                {
                    string anticipoCargado = Session["CobroAnticipo"] as string;
                    if (anticipoCargado == "OK")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"El cobro del anticipo ya fue cargado anteriormente. \", {type: \"info\"});", true);
                    }
                }

                //Configuracion config = new Configuracion();
                string imputado = this.TotalDoc.Value;
                List<Pago> listPago = contCobranza.obtenerPagosdesdeDT(lstMoneda, lstCheque, lstTransferencia, lstTarjetas, lstRetencion);
                if (listPago != null)
                {
                    Cobro cobro = new Cobro();
                    cobro.fecha = DateTime.Now;
                    cobro.cliente.id = this.idCliente;
                    cobro.Doc_imputar = imputaciones;
                    cobro.pagos = listPago;
                    //cobro.puntoVenta.puntoVenta = Session["PuntoVentaCobranza"] as string;  
                    cobro.empresa.id = this.idEmpresa;
                    cobro.sucursal.id = this.idSucursal;
                    cobro.puntoVenta = contSucu.obtenerPtoVentaId(this.puntoVenta);
                    cobro.total = Convert.ToDecimal(this.txtTotalIngresado.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    //string imputado = this.txtTotalDoc.Text.Replace('.', ',');
                    cobro.imputado = Convert.ToDecimal(imputado.Replace(',', '.'), CultureInfo.InvariantCulture);
                    cobro.ingresado = Convert.ToDecimal(this.txtTotalIngresado.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    cobro.comentarios = this.txtObservaciones.Text;
                    if (this.config.numeracionCobros == "0")
                    {
                        cobro.numero = this.txtNumeroCobro.Text;
                    }
                    if (this.anticipo > 0)
                    {
                        cobro.esAnticipo = 1;
                    }
                    if (this.tipoDoc == 1)
                    {
                        cobro.tipoDocumento = this.contDocumentos.obtenerTipoDoc("Recibo de Cobro - FC");
                    }
                    else
                    {
                        cobro.tipoDocumento = this.contDocumentos.obtenerTipoDoc("Recibo de Cobro - Presupuesto");
                    }


                    if ((cobro.ingresado == cobro.imputado) || (this.anticipo > 0 && cobro.ingresado >= cobro.imputado))
                    {
                        //lo que cargo puede llegar ser mayor a la imputacion por los recargos de las tarjetas
                        //solo cuando es anticipo de creditos
                        if (this.anticipo > 0 && cobro.ingresado > cobro.imputado)
                        {
                            cobro.imputado = cobro.ingresado;
                        }
                        if (this.anticipo > 0)
                        {
                            Session["CobroAnticipo"] = cobro;
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Anticipo agregado. \", {type: \"info\"});location.href = 'CobranzaF.aspx';", true);
                        }
                        else
                        {
                            int i = contCobranza.ProcesarCobroPagoCuenta(cobro);
                            if (i > 0)
                            {
                                if (lstCheque.Rows.Count > 0)
                                {
                                    var listCheques = this.contCobranzaEnt.obtenerChequesPorIdCobro(cobro.id);
                                    foreach (int idCheque in listCheques)
                                    {
                                        Gestion_Api.Entitys.Cheques_Datos chd = new Gestion_Api.Entitys.Cheques_Datos();
                                        chd.Cheque = idCheque;
                                        chd.Cobro = cobro.id;
                                        chd.CobroFtp = 0;
                                        if (this.tipoDoc == 2)
                                            chd.CobroFtp = 1;

                                        int k = this.contCobranzaEnt.agregarCheques_Datos(chd);
                                    }
                                }

                                lstPago = null;
                                lstCheque = null;
                                lstMoneda = null;
                                lstTransferencia = null;
                                this.btnNuevo.Visible = true;
                                this.btnFinalizarPago.Visible = false;
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Genero el Recibo de Cobro N°" + this.txtNumeroCobro.Text);

                                int c = contCobranza.enviarSmsCobro(idCliente, (int)Session["Login_IdUser"], cobro);
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", "window.open('ImpresionCobro.aspx?Cobro=" + i + "&valor=2', 'fullscreen', 'top=0,left=0,width=' + (screen.availWidth) + ',height =' + (screen.availHeight) + ',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0'); location.href = 'CobranzaF.aspx';", true);
                                this.mostrarMensaje(c, cobro.id, i);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudo agregar cobro. \");", true);
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo agregar cobro "));
                            }
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"El monto del Cobro debe ser igual al monto del Pago a Cuenta. \");", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El monto del Cobro debe ser igual al monto del Pago a Cuenta "));
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se cargaron pagos. \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargaron pagos "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar cobro  " + ex.Message));
            }
        }


        #endregion

        private void actualizarTotales()
        {
            try
            {
                DataTable dt = lstPago;
                decimal saldo = 0;
                foreach(DataRow dr in dt.Rows)
                {
                    saldo += decimal.Round(Convert.ToDecimal(dr["Total"]), 2);
                }

                txtTotalIngresado.Text = saldo.ToString("0.00").Replace(',', '.');
                if (!String.IsNullOrEmpty(this.TotalDoc.Value))
                {
                    decimal restan = Convert.ToDecimal(this.TotalDoc.Value.Replace(',', '.'), CultureInfo.InvariantCulture) - saldo;
                    if (restan < 0)
                    {
                        lbRestan.Text = "Pago a Cuenta";
                    }
                    else
                    {
                        lbRestan.Text = "Restan";
                    }                      
                    txtRestan.Text = decimal.Round(restan, 2).ToString().Replace(',', '.');
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error actualizando totales. " + ex.Message));
            }
        }

        protected void VerificarImputacion(object sender, EventArgs e)
        {
            try
            {
                if(this.valor == 1)
                {
                    string posicion = (sender as TextBox).ID.ToString().Substring(5, (sender as TextBox).ID.Length - 5);
                    TableRow tr = this.phDocumentos.Controls[Convert.ToInt32(posicion)] as TableRow;
                    TableCell c = tr.Cells[1] as TableCell;
                    string saldoA = c.Text.Substring(1, c.Text.Length - 1);
                    decimal saldo = Convert.ToDecimal(saldoA.Replace(',', '.'), CultureInfo.InvariantCulture);
                    decimal imputado = Convert.ToDecimal((sender as TextBox).Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    if (saldo > 0)
                    {
                        if (imputado > saldo)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El importe a imputar no puede ser mayor al saldo del documento. "));
                        }
                    }
                    if (saldo < 0)
                    {
                        if (saldo > imputado & imputado > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El importe a imputar no puede ser menor al saldo del documento o debe ser menor a cero. "));
                        }
                    }
                    

                }
                if(this.valor == 2)
                {
                    string posicion = (sender as TextBox).ID.ToString().Substring(5, (sender as TextBox).ID.Length - 5);
                    TableRow tr = this.phDocumentos.Controls[Convert.ToInt32(posicion)] as TableRow;
                    decimal saldo = Convert.ToDecimal((sender as TextBox).Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    DataTable dt = lstDocumentos;
                    DataRow dr = lstDocumentos.Rows[Convert.ToInt32(tr.ID)];
                    DataRow dr2 = dt.NewRow();
                    dr2["Id"] = dr["Id"];
                    dr2["Tipo"] = dr["Tipo"];
                    dr2["Numero"] = dr["Numero"];
                    dr2["Imputar"] = 0;
                    dr2["Saldo"] = saldo;
                    dt.Rows.Remove(dr);
                    dt.Rows.Add(dr2);
                    lstDocumentos = dt;
                    this.cargarTablaDocumentos();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error verificando Imputacion. " + ex.Message));
            }
        }

        private void actualizarPagoCuenta(decimal total)
        {
            try
            {
                if(this.monto != total)
                {
                    DataTable dt = lstDocumentos;
                    DataRow dr = lstDocumentos.Rows[Convert.ToInt32(lstDocumentos.Rows.Count -1)];
                    DataRow dr2 = dt.NewRow();
                    dr2["Id"] = dr["Id"];
                    dr2["Tipo"] = dr["Tipo"];
                    dr2["Numero"] = dr["Numero"];
                    dr2["Imputar"] = 0;
                    dr2["Saldo"] = this.monto - total;
                    dt.Rows.Remove(dr);
                    dt.Rows.Add(dr2);
                    lstDocumentos = dt;
                    
                }
            }
            catch
            {

            }
        }

        private string validarPagoCuenta()
        {
            try
            {
                int i = 0;
                decimal suma = 0;
                foreach (Control C in phDocumentos.Controls)
                {
                    TableRow tr = C as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if(tr.Cells[0].Text.Contains("Pago a Cuenta"))
                    {
                        i++;
                    }
                    if (!tr.Cells[0].Text.Contains("Recibo de Cobro") && !tr.Cells[0].Text.Contains("Pago a Cuenta"))
                    {
                        suma += Convert.ToDecimal(txt.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    }    
                }


                return i + "/" + suma;
            }
            catch
            {
                return null;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CobranzaF.aspx");
            }
            catch
            {

            }
        }

        protected void DropListCuentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] info = this.DropListCuentas.SelectedItem.Text.Split('|');
                //this.txtBanco.Text = info[1].Trim();
                this.DropListBancoTransf.SelectedIndex = this.DropListBancoTransf.Items.IndexOf(this.DropListBancoTransf.Items.FindByText(info[1].Trim()));
                //this.DropListBancoTransf.SelectedItem.Text = info[1].Trim();
                this.txtCuentaTransf.Text = info[3].Trim();
               
            }
                catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de cuenta. " + ex.Message));
            }
        }
                
        protected void txtCuentaCh_Disposed(object sender, EventArgs e)
        {
            obtenerCuitYCuenta();
        }

        protected void obtenerCuitYCuenta()
        {
            try
            {
                if (this.DropListBancoCh.SelectedValue != "-1")
                {
                    if (!String.IsNullOrEmpty(this.txtCuentaCh.Text))
                    {
                        List<Cheque> lstCheques = this.contCobranza.obtenerCheques();
                        Cheque chq = lstCheques.Where(x => x.cuenta == this.txtCuentaCh.Text && x.banco.id == Convert.ToInt32(this.DropListBancoCh.SelectedValue)).FirstOrDefault();

                        if (chq != null)
                        {
                            this.txtCuitCh.Text = chq.cuit;
                            this.txtLibradorCh.Text = chq.librador;
                        }
                    }
                }
                //this.txtCuitCh.Focus();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error en obtenerCuitYCuenta. " + ex.Message));
            }
        }

        protected void ListOperadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idOperador = Convert.ToInt32(this.ListOperadores.SelectedValue);
                if (idOperador > 0)
                    this.cargarTarjetasByOperador(idOperador);
            }
            catch
            {

            }
        }

        protected void ListTarjetas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtMontoTarjeta.Text))
                {
                    this.calcularPagosCuotas();
                }
            }
            catch
            {

            }
        }

        public void calcularPagosCuotas()
        {
            try
            {
                //genero la clase
                Tarjeta t = this.contTarjeta.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas2.SelectedValue));

                decimal totalRecargo = Convert.ToDecimal(this.txtMontoTarjeta.Text);
                totalRecargo = Decimal.Round(totalRecargo * (1 + (t.recargo / 100)), 2);
                decimal cuotasFinal = Decimal.Round((totalRecargo / t.cuotas), 2);
                this.lblMontoCuotas.Text = "Pago en " + t.cuotas + " cuotas de $" + cuotasFinal + " final. ";
                if (t.recargo > 0)
                {
                    this.lblMontoCuotas.Text += "Con recargo del " + t.recargo + "%. Total $" + totalRecargo;
                }
            }
            catch
            {

            }
        }

        protected void txtMontoTarjeta_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtMontoTarjeta.Text))
                {
                    this.calcularPagosCuotas();
                }
            }
            catch
            {

            }
        }

        protected void txtCodBarraCh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string lector = this.txtCodBarraCh.Text;
                string codBanco = "";
                if (lector != "" && lector.Length >= 25)
                {
                    this.txtNumeroCh.Text = lector.Substring(11, 8);
                    this.txtCuentaCh.Text = lector.Substring(25, 5) + "-" + lector.Substring(4, 3);
                    codBanco = lector.Substring(1, 3);
                    List<Banco> listBcos = this.contCobranza.obtenerBancosList();
                    var bco = listBcos.Where(x => x.codigo == Convert.ToInt32(codBanco).ToString()).FirstOrDefault();
                    this.DropListBancoCh.SelectedValue = bco.id.ToString();
                    obtenerCuitYCuenta();
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error en txtCodBarraCh_TextChanged. " + ex.Message));
            }
        }

        #region Pagares
        private int liquidarPagares(int idCobro)
        {
            try
            {
                //Si está liquidando pagarés, guardo registro en la tabla Pagares_Liquidaciones y cambio el estado de los pagares
                if (!string.IsNullOrEmpty(this.pagares))
                {
                    int l = this.contFactEnt.liquidarPagares(this.pagares, idCobro);

                    return l;
                }

                return 0;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Ocurrió un error liquidando pagarés. Excepción: " + Ex.Message + " \");", true);
                return -1;
            }
        }
        
        #endregion



    }
}