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

namespace Gestion_Web.Formularios.Pagos
{
    public partial class PagosABM : System.Web.UI.Page
    {
        controladorPagos contPagos = new controladorPagos();
        ControladorCCProveedor controlador = new ControladorCCProveedor();
        controladorSucursal contSucu = new controladorSucursal();
        controladorCobranza contCobranza = new controladorCobranza();
        ControladorCobranzaEntity contCobranzaEnt = new ControladorCobranzaEntity();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        ControladorClienteEntity contCliEnt = new ControladorClienteEntity();
        ControladorEmpresa contEmp = new ControladorEmpresa();
        controladorTarjeta contTarjeta = new controladorTarjeta();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();

        Mensajes mje = new Mensajes();
        string documentos = "";
        private int idProveedor;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private decimal monto;
        private int tipoDoc;
        int posCheques;
        int accion;
        int bn;

        DataTable lstPagoTemp;
        DataTable lstMonedaTemp;
        DataTable lstTransferenciaTemp;
        DataTable lstChequeTemp;
        DataTable lstChequeTercerosTemp;
        DataTable lstChequeTerceros2Temp;
        DataTable lstDocumentosTemp;
        DataTable lstTarjetasTemp;
        DataTable lstRetencionTemp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                lbtnAgregarPago.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(lbtnAgregarPago, null) + ";");

                //cargo fechas en campos de fecha
                //obtengo los movimientos
                this.documentos = Request.QueryString["d"];
                this.idProveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.monto = Convert.ToDecimal(Request.QueryString["m"].Replace(',', '.'), CultureInfo.InvariantCulture);
                this.tipoDoc = Convert.ToInt32(Request.QueryString["t"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.bn = Convert.ToInt32(Request.QueryString["bn"]);
                btnAtras.Attributes.Add("onclick", "history.back(); return false;");
                //this.obtenerNroRecibo();

                

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    //pongo variables de session
                    Session["ABMCobros_PosCheque"] = null;

                    this.txtFechaCh.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaRetencion.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaTransf.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaPago.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.btnNuevo.Visible = false;
                    this.btnFinalizarPago.Visible = true;
                    lstPagoTemp = new DataTable();
                    lstChequeTemp = new DataTable();
                    this.lstChequeTercerosTemp = new DataTable();
                    this.lstChequeTerceros2Temp = new DataTable();
                    lstMonedaTemp = new DataTable();
                    lstTransferenciaTemp = new DataTable();
                    lstDocumentosTemp = new DataTable();
                    lstTarjetasTemp = new DataTable();
                    lstRetencionTemp = new DataTable();

                    this.InicializarListaPagos();
                    this.InicializarListaMoneda();
                    this.InicializarListaTransferencia();
                    this.InicializarListaCheque();
                    this.InicializarListaChequeTercero();
                    this.InicializarListaChequeTercero2();
                    this.InicializarListaDocumentos();
                    this.InicializarListaTarjeta();
                    this.InicializarListaRetencion();

                    //Session["ListaPAgos"] = null;
                    this.cargarTipoMoneda();
                    //this.cargarBancos();
                    this.cargarCuentas();
                    this.cargarBancosTransf();
                    this.cargarTarjetas();
                    this.cargarTiposRetencion();
                    //pago normal
                    if (this.accion == 1)
                    {
                        this.CargarMovimientos(documentos);
                        
                    }
                    //pago a cuenta, favor del proveedor
                    if (this.accion == 2)
                    {
                        this.CargarPagoCuenta();
                    }
                    if (this.tipoDoc == 2)
                    {
                        this.panelRetencion.Visible = false;
                    }

                    //cargo retenciones si es cancelacion total
                    this.checkearRetencion();

                    //Dependiendo la config. el recibo es autonumerico o con nro del prov
                    Configuracion c = new Configuracion();
                    if (c.numeracionPagos == "1")
                    {
                        this.txtNumeroCobro.Text = this.obtenerNroReciboPago();
                        this.txtNumeroCobro.ReadOnly = true;
                    }
                    else
                    {
                        this.txtNumeroCobro.Text = "";
                        this.txtNumeroCobro.ReadOnly = false;
                    }
                    

                }
                else
                {
                    this.cargarTablaCheques();
                    this.cargarTablaChequesTerceros();
                    this.cargarTablaDocumentos();
                    this.cargarTablaPAgos();

                    //this.cargarTablaChequesTerceros();
                    this.actualizarTotales(0);                    
                }
                //cargo los cheques en cartera
                this.cargarTablaChequesCartera();
                
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros") != 1)
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
                        if (s == "41")
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.A1.Attributes.Remove("style");
                                this.A1.Attributes.Remove("display");
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

        #region inicializar Temporales
        private void InicializarListaPagos()
        {
            try
            {
                lstPagoTemp.Columns.Add("Tipo Pago");
                lstPagoTemp.Columns.Add("Monto");
                lstPagoTemp.Columns.Add("Cotizacion");
                lstPagoTemp.Columns.Add("Total");
                lstPagoTemp.Columns.Add("ID");
                lstPagoTemp.Columns.Add("IdTipoPago");
                lstPagoTemp.Columns.Add("FilaTipoPago");
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
                lstChequeTemp.Columns.Add("Origen");
                lstCheque = lstChequeTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));
            }
        }

        private void InicializarListaChequeTercero()
        {
            try
            {
                lstChequeTercerosTemp.Columns.Add("Fecha");
                lstChequeTercerosTemp.Columns.Add("Importe");
                lstChequeTercerosTemp.Columns.Add("Numero");
                lstChequeTercerosTemp.Columns.Add("Banco");
                lstChequeTercerosTemp.Columns.Add("Banco Entidad");
                lstChequeTercerosTemp.Columns.Add("Cuenta");
                lstChequeTercerosTemp.Columns.Add("Cuit");
                lstChequeTercerosTemp.Columns.Add("Librador");
                lstChequeTercerosTemp.Columns.Add("Monto");
                lstChequeTercero = lstChequeTercerosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }
        }

        private void InicializarListaChequeTercero2()
        {
            try
            {
                lstChequeTerceros2Temp.Columns.Add("ID");
                lstChequeTerceros2Temp.Columns.Add("Fecha");
                lstChequeTerceros2Temp.Columns.Add("Importe");
                lstChequeTerceros2Temp.Columns.Add("Numero");
                lstChequeTerceros2Temp.Columns.Add("Banco");
                lstChequeTerceros2Temp.Columns.Add("Banco Entidad");
                lstChequeTerceros2Temp.Columns.Add("Cuenta");
                lstChequeTerceros2Temp.Columns.Add("Cuit");
                lstChequeTerceros2Temp.Columns.Add("Librador");
                lstChequeTerceros2Temp.Columns.Add("Monto");
                lstChequeTercero2 = lstChequeTerceros2Temp;
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

        protected DataTable lstChequeTercero
        {
            get
            {
                if (ViewState["ListaChequesTercero"] != null)
                {
                    return (DataTable)ViewState["ListaChequesTercero"];
                }
                else
                {
                    return lstChequeTercerosTemp;
                }
            }
            set
            {
                ViewState["ListaChequesTercero"] = value;
            }
        }

        protected DataTable lstChequeTercero2
        {
            get
            {
                if (ViewState["ListaChequesTercero2"] != null)
                {
                    return (DataTable)ViewState["ListaChequesTercero2"];
                }
                else
                {
                    return lstChequeTerceros2Temp;
                }
            }
            set
            {
                ViewState["ListaChequesTercero2"] = value;
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

        #region Cargas Iniciales
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

                //this.DropListBancoCh.DataSource = dt;
                //this.DropListBancoCh.DataValueField = "id";
                //this.DropListBancoCh.DataTextField = "entidad";
                //this.DropListBancoCh.DataBind();

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
                //DataTable dt = contCobranza.obtenerBancosDT();
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("entidad");                

                ControladorBanco contBanco = new ControladorBanco();
                var cuentas = contBanco.obtenerCuentasBancarias();
                

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                
                foreach (var c in cuentas)
                {
                    DataRow dr2 = dt.NewRow();
                    dr2["entidad"] = c.Banco1.entidad +" - " + c.Descripcion + " - " + c.Numero + " - " + c.Cuit + " - " + c.Librador + "|" + c.Id;
                    dr2["id"] = c.Id;
                    dt.Rows.Add(dr2);
                }

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
                this.ListCuenta.Items.Add("Seleccione...");
                foreach (var c in cuentas)
                {
                    this.ListCuenta.Items.Add(c.Id + " - " + c.Banco1.entidad + " - " + c.Descripcion + " - " + c.Numero + " - " + c.Cuit + " - " + c.Librador);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cuentas a la lista. " + ex.Message));
            }
        }
       

        #endregion

        #region eventos de la pantalla

        //carga las monedas en el acordeon de contado
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
        protected void btnRecalcularRetencion_Click1(object sender, EventArgs e)
        {
            this.checkearRetencion();
        }       

        #endregion

        #region carga de documentos a imputar
        public void CargarMovimientos(string doc)
        {
            try
            {
                //carga automatica
                if (this.monto == 0)
                {
                    List<MovimientosCCP> movimientos = this.controlador.obtenerMovimientos(doc);
                    decimal totalSaldo = 0;
                    foreach (MovimientosCCP m in movimientos)
                    {
                        //if (!m.tipo.tipo.Contains("Recibo de Cobro"))
                        //{
                            totalSaldo += (decimal)m.Saldo;
                        //}
                        this.cargarMovimientoDT(m, 0);
                    }
                    txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');

                    //if (bn == 1)
                    //{
                    //    //verifico IIBB
                    //    this.cargarRetencionesIIBB(totalSaldo);                        

                    //}

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
                //obtengo los movimientos
                List<MovimientosCCP> movimientos = this.controlador.obtenerMovimientos(doc);

                decimal totalSaldo = 0;
                decimal montoActual = monto;
                //los recorro y voy cargando
                foreach (var m in movimientos)
                {
                    if (m.TipoDocumento != 21)
                    {
                        //verifico cuanto le imputo al movimiento
                        decimal imputacion = controlador.obtenerMontoImputar((decimal)m.Saldo, montoActual);
                        montoActual -= imputacion;
                        totalSaldo += (decimal)m.Saldo;
                        this.cargarMovimientoDT(m, imputacion);
                    }
                    else // si es saldo a favor
                    {
                        this.cargarMovimientoDT(m, 0);
                    }
                }
                //sim me queda plata accion favor genero un Pago a cuenta(favor) al proveedor
                totalSaldo += this.generarPagoCuenta(totalSaldo);
                txtSaldoDoc.Text = totalSaldo.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos " + ex.Message));
            }
        }

        public decimal generarPagoCuenta(decimal saldo)
        {
            try
            {
                if (this.monto > saldo)
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
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));
                return 0;
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

        private string obtenerNroReciboPago()
        {
            try
            {
                controladorSucursal cs = new controladorSucursal();
                PuntoVenta pv = cs.obtenerPtoVentaId(this.puntoVenta);
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.contFact.obtenerFacturaNumero(this.puntoVenta, "Recibo Pago");
                string numero = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                return numero;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
                return null;
            }
        }

        public void cargarMovimientoDT(MovimientosCCP m, decimal imputacion)
        {
            try
            {
                DataTable dt = lstDocumentos;

                DataRow dr = dt.NewRow();
                dr["Id"] = m.Id;
                dr["Tipo"] = m.tipoDocumento1.tipo;
                dr["Numero"] = m.Numero;
                dr["Saldo"] = m.Saldo;
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

        private void cargarDocumentoEnPh(DataRow dr, int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = dr["Id"].ToString();

                //Celdas

                TableCell celNumero = new TableCell();
                celNumero.Text = dr["Tipo"].ToString() + " " + dr["Numero"].ToString().PadLeft(8, '0');
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
                if (Convert.ToDecimal(dr["Imputar"]) > 0)
                {
                    txtImputar.Text = dr["Imputar"].ToString().Replace(',', '.');
                }
                txtImputar.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtImputar.ID = "Text_" + pos.ToString();
                //txtImputar.TextChanged += new EventHandler(VerificarImputacion);
                txtImputar.AutoPostBack = true;
                txtImputar.Style.Add("text-align", "right");
                //txtImputar.TextChanged += new EventHandler(this.ActualizarTotales);
                txtImputar.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtImputar.Attributes.Add("onchange", "javascript:TotalImputado(); return false");
                if (dr["Tipo"].ToString().Contains("Recibo de Cobro") && this.accion == 1)
                {
                    txtImputar.Attributes.Add("Disabled", "true");
                }
                celImputar.Controls.Add(txtImputar);
                celImputar.Width = Unit.Percentage(15);
                celImputar.HorizontalAlign = HorizontalAlign.Right;
                celImputar.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celImputar);

                phDocumentos.Controls.Add(tr);

                //Si es un Pago Total/Cancelacion del documento agrego la Retencion
                //this.checkearRetencion(dr, Convert.ToDecimal(txtImputar.Text));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando documentos. " + ex.Message));
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
                    if (!dr["Tipo"].ToString().Contains("Recibo de Cobro"))
                    {
                        totalSaldo += Convert.ToDecimal(dr["Saldo"]);
                    }
                    pos = dt.Rows.IndexOf(dr);
                    this.cargarDocumentoEnPh(dr, pos);

                   
                }
                txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');
                
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista de Documentos.  " + ex.Message));
            }
        }

        private void cargarRetencionesIIBB(decimal totalImputado)
        {
            try
            {
                //verifico si al proveedor le tengo que retener ingresos brutos
                var IIBB = this.contCliEnt.obtenerIngresosBrutoCliente(this.idProveedor);
                if (IIBB != null)
                {
                    if (IIBB.Retencion > 0)
                    {
                        decimal retencion = (decimal)IIBB.Retencion;
                        //decimal total = Convert.ToDecimal(TotalDoc.Value);

                        //calculo la retencion en base al total
                        decimal totalRetencion = decimal.Round(totalImputado * (retencion / 100), 2);

                        int nro = this.contFact.obtenerFacturaNumero(this.puntoVenta, "Retencion");
                        //cargo retencion
                        this.agregarRetencion(totalRetencion, DateTime.Today, 1, nro.ToString().PadLeft(6, '0'), 1);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo ingresos brutos proveedor.  " + ex.Message));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error actualizando Ingresos Brutos" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        #endregion

        #region carga de PAgos contado
        protected void btnAgregarPagoM_Click(object sender, EventArgs e)
        {
            try
            {
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && Convert.ToDecimal(txtSaldoDoc.Text) >= Convert.ToDecimal("0.00") ))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    Pago_Contado p = new Pago_Contado();
                    p.moneda.id = Convert.ToInt32(this.DropListTipo.SelectedValue);
                    p.moneda.moneda = this.DropListTipo.SelectedItem.Text;
                    decimal monto = Convert.ToDecimal(txtMonto.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
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
                    lstMoneda = dtMoneda;

                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = p.moneda.moneda;
                    dr["Monto"] = p.monto;
                    dr["Cotizacion"] = p.moneda.cambio;
                    dr["Total"] = decimal.Round(p.monto * p.moneda.cambio, 2);
                    dr["IdTipoPago"] = "1";//tipo efectivo
                    dr["FilaTipoPago"] = dtMoneda.Rows.IndexOf(drMoneda).ToString();

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposM();
                    this.actualizarTotales(0);
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
                celTotal.Text = "$ " + dr["Total"].ToString().Replace(',', '.');
                celTotal.Width = Unit.Percentage(20);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celAccion = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + pos.ToString() + "_" + dr["IdTipoPago"].ToString() + "_" + dr["FilaTipoPago"].ToString(); 
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

        /// <summary>
        /// actualiza los totales en la pantalla
        /// </summary>
        /// Si i ==1 viene de retenciones       
        private void actualizarTotales(int i)
        {
            try
            {
                DataTable dt = lstPago;
                decimal saldo = 0;
                foreach (DataRow dr in dt.Rows)
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
                else
                {
                    //viene de retenciones y tomo el saldo
                    if (i == 1)
                    {
                        //decimal total = Convert.ToDecimal(txtSaldoDoc.Text);
                        decimal restan = Convert.ToDecimal(txtSaldoDoc.Text) -saldo;
                        if (restan < 0)
                        {
                            lbRestan.Text = "Pago a Cuenta";
                        }
                        else
                        {
                            lbRestan.Text = "Restan";
                        }
                        txtRestan.Text = decimal.Round(restan, 2).ToString().Replace(',', '.');
                        //UpdatePanel1.UpdateMode = UpdatePanelUpdateMode.Always;
                        //UpdatePanel1.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error actualizando totales. " + ex.Message));

            }
        }

        //Si es un Pago Total/Cancelacion del documento agrego la Retencion        
        private void checkearRetencion()
        {
            try
            {
                Configuracion configuracion = new Configuracion();
                this.lstRetencion.Clear();                
                int cDocumento = 0;
                decimal totalRetencion = 0;
                int filaRetencion = 0;

                if (lstPago.Rows.Count > 0)
                {
                    foreach (DataRow dr in lstPago.Rows)
                    {
                        if (dr[0].ToString().Contains("Retencion"))
                        {
                            filaRetencion = lstPago.Rows.IndexOf(dr);
                        }
                    }
                    lstPago.Rows[filaRetencion].Delete();//elimino la retencion de la tabla
                }

                DataTable dtRetencion = lstRetencion;
                List<MovimientosCCP> m = this.controlador.obtenerMovimientos(this.documentos);

                if (!String.IsNullOrEmpty(txtSaldoDoc.Text))//Si no hay ninguna retencion ya cargada
                {
                    foreach (Control c in phDocumentos.Controls)
                    {

                        //Verifico que el tipo de documento sea diferente a presupuesto y que existan movimientos
                        if (m[cDocumento] != null && m[cDocumento].Ftp == 0)
                        {
                            TableRow tr = c as TableRow;
                            TextBox txtImputar = tr.Cells[2].Controls[0] as TextBox;

                            Decimal saldo = Convert.ToDecimal(tr.Cells[1].Text.Replace("$", ""));//Saldo del documento
                            Decimal imputacion = Convert.ToDecimal(txtImputar.Text);//Imputacion al documento

                            if (tr.Cells[0].Text.Contains("Compra"))//Si es compra sumo los netos y lo voy acumulando. Verifico que el saldo sea mayor al tope minimo de retenciones
                            {
                                if (imputacion == saldo)//Solo si es cancelacion total de la compra.
                                {

                                    decimal totalNetoRetencion = (decimal)(m[cDocumento].Compra.Neto105.Value + m[cDocumento].Compra.Neto21.Value + m[cDocumento].Compra.Neto27.Value + 
                                                                            m[cDocumento].Compra.Neto2.Value + m[cDocumento].Compra.Neto5.Value + m[cDocumento].Compra.NetoNoGrabado.Value);

                                    //Si es nota de crédito, pongo en negativo el monto para luego restarlo 
                                    if (m[cDocumento].Compra.TipoDocumento.ToLower().Contains("nota de crédito"))
                                    {
                                        totalNetoRetencion = totalNetoRetencion * -1;
                                    }
                                    //sumatoria de los ivas de las compras canceladas total
                                    totalRetencion += totalNetoRetencion;
                                }
                            }

                            cDocumento += 1;//fila del documento compra en el list de movimientos
                        }
                        
                    }

                    if (totalRetencion > 0 && totalRetencion > Convert.ToDecimal(configuracion.TopeMinimoRetencion))
                    {
                        //this.checkearRetencion(documentos);
                        this.cargarRetencionesIIBB(totalRetencion);
                    }
                    else
                    {
                        //si no cargue ninguna retencion recargo la pantalla
                        this.cargarTablaPAgos();                        
                        this.actualizarTotales(1);
                    }
                    
                }
                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error Verificando retenciones. " + ex.Message));
            }
        }

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
        #endregion

        #region carga de Cheques

        protected void ListCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] info = this.ListCuenta.SelectedItem.Text.Split('-');
                this.txtBanco.Text = info[1].Trim();
                this.txtTipoCuenta.Text = info[2].Trim();
                this.txtNroCuenta.Text = info[3].Trim();
                this.txtCuitCh.Text = info[4].Trim();
                this.txtLibradorCh.Text = info[5].Trim();

                //actualizo
                //llamo al metodo que muestra los pagos en la tabla
                this.cargarTablaPAgos();
                this.cargarTablaCheques();
                //this.limpiarCamposCh();
                this.actualizarTotales(0);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de cuenta. " + ex.Message));
            }
        }
        protected void btnAgregarPagoCh_Click(object sender, EventArgs e)
        {
            try
            {
                //if (contCobranza.validateCuit(this.txtCuitCh.Text))
                //{
                    if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                    //if (TotalDoc.Value != "0.00")
                    {
                        //genero la clase
                        decimal monto = Convert.ToDecimal(txtImporteCh.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        DataTable dtCheque = lstCheque;
                        DataRow drCheque = dtCheque.NewRow();
                        drCheque["Fecha"] = Convert.ToDateTime(txtFechaCh.Text, new CultureInfo("es-AR"));
                        drCheque["Importe"] = decimal.Round(monto, 2);
                        drCheque["Numero"] = Convert.ToDecimal(txtNumeroCh.Text);
                        var b = this.contPagos.obtenerBancoPorEntidad(this.txtBanco.Text);
                        drCheque["Banco"] = b.id;
                        drCheque["Banco Entidad"] = this.txtBanco.Text;
                        //drCheque["Cuenta"] = txtCuentaCh.Text.ToString();
                        drCheque["Cuenta"] = txtNroCuenta.Text;
                        drCheque["Cuit"] = txtCuitCh.Text;
                        drCheque["Librador"] = txtLibradorCh.Text;
                        drCheque["Monto"] = decimal.Round(monto, 2);
                        drCheque["Origen"] = "P";

                        dtCheque.Rows.Add(drCheque);
                        lstCheque = dtCheque;
                        //pago que voy agregando al pago
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
                            dr["IdTipoPago"] = "2";//tipo cheque Pr
                            dr["FilaTipoPago"] = dtCheque.Rows.IndexOf(drCheque).ToString();
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
                            drNueva["IdTipoPago"] = "2";//tipo cheque Pr
                            dr["FilaTipoPago"] = dtCheque.Rows.IndexOf(drCheque).ToString();
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
                        this.actualizarTotales(0);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes a imputar"));
                        this.limpiarCamposCh();
                    }
                //}
                //else
                //{
                //    this.lblErrorCuit.Visible = true;
                //    this.txtCuitCh.Focus();
                //}
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques " + ex.Message));
            }
        }

        public void cargarEnPHCheque(DataRow dr, int pos)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celMoneda = new TableCell();
                string fecha = Convert.ToDateTime(dr["Fecha"].ToString()).ToString("dd/MM/yyyy");
                celMoneda.Text = fecha;
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                celMoneda.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celMoneda);

                TableCell celMonto = new TableCell();
                celMonto.Text = "$ " + dr["Importe"].ToString().Replace(',', '.');
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando cheques a la lista. " + ex.Message));
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

        public void limpiarCamposCh()
        {
            try
            {
                this.ListCuenta.SelectedIndex = 0;
                txtImporteCh.Text = "";
                txtNumeroCh.Text = "";
                txtBanco.Text = "";
                txtTipoCuenta.Text = "";
                //DropListBancoCh.SelectedValue = "-1";
                //txtCuentaCh.Text = "";
                txtNroCuenta.Text = "";
                txtCuitCh.Text = "";
                txtLibradorCh.Text = "";
                this.lblErrorCuit.Visible = false;
            }
            catch
            {

            }
        }

        #endregion

        #region carga de Cheques terceros
        protected void btnAgregarPagoChTerceros_Click(object sender, EventArgs e)
        {
            try
            {
                if (contCobranza.validateCuit(this.txtCuitCh.Text))
                {
                    if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                    //if (TotalDoc.Value != "0.00")
                    {
                        //genero la clase
                        decimal monto = Convert.ToDecimal(txtImporteCh.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        DataTable dtCheque = lstCheque;
                        DataRow drCheque = dtCheque.NewRow();
                        drCheque["Fecha"] = Convert.ToDateTime(txtFechaCh.Text, new CultureInfo("es-AR"));
                        drCheque["Importe"] = decimal.Round(monto, 2);
                        drCheque["Numero"] = Convert.ToDecimal(txtNumeroCh.Text);
                        //drCheque["Banco"] = Convert.ToInt32(DropListBancoCh.SelectedValue);
                        //drCheque["Banco Entidad"] = DropListBancoCh.SelectedItem.Text;
                        //drCheque["Cuenta"] = txtCuentaCh.Text.ToString();
                        drCheque["Cuenta"] = txtNroCuenta.Text;
                        drCheque["Cuit"] = txtCuitCh.Text;
                        drCheque["Librador"] = txtLibradorCh.Text;
                        drCheque["Monto"] = decimal.Round(monto, 2);
                        
                        dtCheque.Rows.Add(drCheque);
                        lstCheque = dtCheque;
                        //pago que voy agregando al pago
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
                            dr["IdTipoPago"] = "7";//tipo cheque 3ro
                            dr["FilaTipoPago"] = dtCheque.Rows.IndexOf(drCheque).ToString();
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
                            drNueva["IdTipoPago"] = "7";//tipo cheque 3ro
                            dr["FilaTipoPago"] = dtCheque.Rows.IndexOf(drCheque).ToString();

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
                        this.actualizarTotales(0);
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }
        protected void btnLiberarCheques_Click(object sender, EventArgs e)
        {
            try
            {
                //limpio todos los cheques cargados
                this.lstChequeTercero.Clear();
                this.lstChequeTercero2.Clear();
                //this.lstChequeTerceros2Temp.Clear();

                //busco los cheques en la tabla de pagos del final y los saco
                DataTable dtPagos = lstPago;
                foreach (Control C in phChequesTerceros.Controls)
                {
                    TableRow tr = C as TableRow;
                    LinkButton ch = tr.Cells[5].Controls[0] as LinkButton;
                    string idCheque = ch.ID.Split('_')[1];
                    foreach (DataRow dr in dtPagos.Rows)
                    {
                        if (Convert.ToInt32(dr["ID"]) == Convert.ToInt32(idCheque))
                        {
                            dtPagos.Rows.Remove(dr);
                            break;
                        }
                    }
                }

                int i = this.contCobranza.LiberarChequesTomados();
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiberar, this.UpdatePanelLiberar.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito!. \", {type: \"info\"}); cerrarModal(); ", true);
                    //llamo al metodo que muestra los pagos en la tabla
                    //this.cargarTablaPAgos();
                    this.cargarTablaChequesTerceros();
                    this.cargarTablaChequesCartera();
                    this.actualizarTotales(0);
                    this.cargarTablaPAgos();
                    //cargo tabla                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error liberando cheques tomados. " + ex.Message));
            }
        }
        private decimal actualizarTotalChequeTerceros()
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

        private void cargarTablaChequesCartera()
        {
            try
            {
                DataTable dt = lstChequeTercero;
                List<Gestion_Api.Modelo.Cheque> cheques = this.contCobranza.obtenerChequesDisponibles();

                if (this.txtLectorCheque.Text != "")
                {
                    string lectorCheque = this.txtLectorCheque.Text;
                    string nroCheque = lectorCheque.Substring(11, 8);

                    cheques = cheques.Where(x => x.numero == Convert.ToDecimal(nroCheque)).ToList();
                }

                //limpio el Place holder
                this.phChequeCartera.Controls.Clear();

                // Creo columna al DataTable para Tipo de Cheque, Blanco / Negro

                dt.Columns.Add("Tipo");

                foreach (var ch in cheques)
                {
                    DataRow drCheque = dt.NewRow();
                    drCheque["Fecha"] = ch.fecha;
                    drCheque["Importe"] = ch.importe;
                    drCheque["Numero"] = ch.numero;
                    drCheque["Banco"] = ch.banco.id;
                    var b = this.contPagos.obtenerBancoPorId(ch.banco.id);
                    drCheque["Banco Entidad"] =  b.entidad;
                    drCheque["Cuenta"] = ch.cuenta;
                    drCheque["Cuit"] = ch.cuit;
                    drCheque["Librador"] = ch.librador;
                    drCheque["Monto"] = ch.importe;
                    drCheque["Tipo"] = this.contCobranza.obtenerTipoCheque(ch);

                    //que me cargue la tabla, recibiendo una clase PAgo_contado
                    int pos = dt.Rows.IndexOf(drCheque);
                    this.cargarEnPHChequeCartera(drCheque, ch.id);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques terceros " + ex.Message));
            }
        }
        
        public void cargarEnPHChequeCartera(DataRow dr, int idCheque)
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
                celMonto.Text = "$ " + dr["Importe"].ToString().Replace(',', '.');
                celMonto.HorizontalAlign = HorizontalAlign.Right;
                celMonto.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMonto);

                TableCell celNumero = new TableCell();
                celNumero.Text = dr["Numero"].ToString();
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celBanco = new TableCell();
                celBanco.Text = dr["Banco Entidad"].ToString();
                celBanco.HorizontalAlign = HorizontalAlign.Left;
                celBanco.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celBanco);

                TableCell celCuenta = new TableCell();
                celCuenta.Text = dr["Cuenta"].ToString();
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCuenta);

                TableCell celTipo = new TableCell();
                celTipo.Text = dr["Tipo"].ToString();
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + idCheque;
                cbSeleccion.CssClass = "btn btn-info";
                celSeleccion.Controls.Add(cbSeleccion);
                celSeleccion.Width = Unit.Percentage(12);
                celSeleccion.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celSeleccion);
                
                phChequeCartera.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando cheques a la lista. " + ex.Message));
            }
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //recorro y busco los chequeados
            //    foreach (Control C in phChequeCartera.Controls)
            //    {
            //        TableRow tr = C as TableRow;
            //        CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
            //        if (ch.Checked == true)
            //        {
            //            string idtildado = ch.ID.Split('_')[1];
            //            this.agregarChequesAPago(Convert.ToInt32(idtildado));
            //        }
            //    }
            //    //llamo al metodo que muestra los pagos en la tabla
            //    //this.cargarTablaPAgos();
            //    this.cargarTablaChequesTerceros();
            //    this.cargarTablaChequesCartera();
            //    this.actualizarTotales();
            //    this.cargarTablaPAgos();
            //    //cargo tabla
            //    //this.cargarTablaChequesTerceros();
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error editando cheque. " + ex.Message));
            //}
        }

        protected void btnAgregar_Click1(object sender, EventArgs e)
        {
            try
            {
                //recorro y busco los chequeados
                foreach (Control C in phChequeCartera.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[0] as CheckBox;
                    if (ch.Checked == true)
                    {
                        string idtildado = ch.ID.Split('_')[1];
                        this.agregarChequesAPago(Convert.ToInt32(idtildado));
                    }
                }
                //llamo al metodo que muestra los pagos en la tabla
                //this.cargarTablaPAgos();
                this.cargarTablaChequesTerceros();
                this.cargarTablaChequesCartera();
                this.actualizarTotales(0);
                this.cargarTablaPAgos();
                //cargo tabla
                //this.cargarTablaChequesTerceros();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error editando cheque. " + ex.Message));
            }
        }
        private void agregarChequesAPago(int idCheque)
        {
            try
            {
                //lo pongo tomado
                int i = this.contPagos.tomarCheque(idCheque);
                if (i > 0)
                {
                    var ch = this.contCobranza.obtenerChequeId(idCheque);

                    DataTable dtCheque = lstChequeTercero2;
                    
                    DataRow drCheque = dtCheque.NewRow();
                    drCheque["ID"] = ch.id;
                    drCheque["Fecha"] = ch.fecha;
                    drCheque["Importe"] = decimal.Round(ch.importe, 2);
                    drCheque["Numero"] = ch.numero;
                    var b = this.contPagos.obtenerBancoPorId(ch.banco.id);
                    drCheque["Banco"] = b.id;
                    drCheque["Banco Entidad"] = b.entidad;
                    //drCheque["Cuenta"] = txtCuentaCh.Text.ToString();
                    drCheque["Cuenta"] = ch.cuenta;
                    drCheque["Cuit"] = ch.cuit;
                    drCheque["Librador"] = ch.librador;
                    drCheque["Monto"] = decimal.Round(ch.importe, 2);
                    dtCheque.Rows.Add(drCheque);
                    
                    lstChequeTercero2 = dtCheque;

                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["ID"] = ch.id;
                    dr["Tipo Pago"] = "Cheque Tercero ";
                    dr["Monto"] = ch.importe;
                    dr["Cotizacion"] = "1 ";
                    dr["Total"] = decimal.Round(ch.importe, 2);

                    dt.Rows.Add(dr);

                    lstPago = dt;
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo tomar cheque para imputar. Reintente. "));
                }
            }
            catch (Exception ex)
            {
                //libero cheque
                int i = this.contPagos.liberarCheque(idCheque);
                if (i > 0)
                {
                    Log.EscribirSQL(1, "INFO", "Se libero cheque:  " + idCheque + ". ");
                }
                else
                {
                    Log.EscribirSQL(1, "INFO", "No se pudo liberar cheque:  " + idCheque + ". ");
                }

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar cheque a pago. " + ex.Message));
            }
        }

        // Cheques agregados a la tabla
        private void cargarTablaChequesTerceros()
        {
            try
            {
                DataTable dt = lstChequeTercero2;
                //List<Gestion_Api.Modelo.Cheque> cheques = this.contCobranza.obtenerChequesDisponibles();

                //limpio el Place holder
                this.phChequesTerceros.Controls.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    //que me cargue la tabla, recibiendo una clase PAgo_contado
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarEnPHChequeTercero(dr, pos);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques terceros " + ex.Message));
            }
        }

        public void cargarEnPHChequeTercero(DataRow dr, int pos)
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
                celMonto.Text = "$ " + dr["Importe"].ToString().Replace(',', '.');
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
                btnEditar.ID = "btnEditar_" + dr["ID"].ToString();
                btnEditar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEditar.Click += new EventHandler(this.EliminarChequeTercero);
                celAccion2.Controls.Add(btnEditar);
                celAccion2.Width = Unit.Percentage(5);
                celAccion2.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion2);

                
                phChequesTerceros.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando cheques a la lista. " + ex.Message));
            }
        }

        private void EliminarChequeTercero(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = lstChequeTercero2;
                string[] Cheque = (sender as LinkButton).ID.Split('_');
                int idCheque = Convert.ToInt32(Cheque[1]);
               
                int f = this.contPagos.liberarCheque(idCheque);
                if (f > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["ID"]) == idCheque)
                        {
                            dt.Rows.Remove(dr);
                            break;
                        }
                    }

                    lstChequeTercero2 = dt;

                    DataTable dtPagos = lstPago;
                    foreach (DataRow dr in dtPagos.Rows)
                    {
                        if (Convert.ToInt32(dr["ID"]) == idCheque)
                        {
                            dtPagos.Rows.Remove(dr);
                            break;
                        }
                    }
                    
                    //posCheques = (int)Session["ABMCobros_PosCheque"];
                    //DataRow drPago = dtPagos.Rows[posCheques];
                    //drPago["Monto"] = this.actualizarTotalCheque();
                    //drPago["Total"] = this.actualizarTotalCheque();
                    //dt.Rows.RemoveAt(posCheques);
                    //dt.Rows.InsertAt(drPago, posCheques);
                    //posCheques = dt.Rows.IndexOf(drPago);
                    //Session.Add("ABMCobros_PosCheque", posCheques);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo quitar cheque. "));
                }
                this.cargarTablaChequesCartera();
                this.cargarTablaChequesTerceros();
                this.actualizarTotalCheque();
                this.actualizarTotalChequeTerceros();
                this.actualizarTotales(0);
                this.cargarTablaPAgos();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error editando cheque. " + ex.Message));
            }
        }

        #endregion

        #region transferencias
        protected void btn_AgregarPagoTrans_Click(object sender, EventArgs e)
        {
            try
            {
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    decimal monto = Convert.ToDecimal(txtImporteTransf.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    DataTable dtTransferencia = lstTransferencia;
                    DataRow drTransferencia = dtTransferencia.NewRow();
                    drTransferencia["Fecha"] = Convert.ToDateTime(txtFechaTransf.Text, new CultureInfo("es-AR"));
                    drTransferencia["Importe"] = decimal.Round(monto, 2);
                    drTransferencia["Banco"] = Convert.ToInt32(DropListBancoTransf.SelectedValue);
                    drTransferencia["Banco Entidad"] = DropListBancoTransf.SelectedItem.Text;
                    drTransferencia["Cuenta"] = txtCuentaTransf.Text;
                    //drTransferencia["CBU"] = txtCbu.Text;
                    drTransferencia["Monto"] = decimal.Round(monto, 2);
                    drTransferencia["IdCuentaBanc"] = DropListBancoTransf.SelectedItem.Text.Split('|')[1];

                    dtTransferencia.Rows.Add(drTransferencia);
                    lstTransferencia = dtTransferencia;

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Transferencia N° " + txtCuentaTransf.Text;
                    dr["Monto"] = decimal.Round(monto, 2);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(monto, 2);
                    dr["IdTipoPago"] = "3";//tipo transf
                    dr["FilaTipoPago"] = dtTransferencia.Rows.IndexOf(drTransferencia).ToString();
                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposTransf();
                    this.actualizarTotales(0);
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes a imputar"));
                    this.limpiarCamposCh();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        public void limpiarCamposTransf()
        {
            try
            {
                txtImporteTransf.Text = "";
                txtCuentaTransf.Text = "";
                //txtCbu.Text = "";
                DropListBancoTransf.SelectedValue = "-1";
            }
            catch
            {

            }
        }

        #endregion

        #region Tarjetas
        protected void btnAgregarPagoTarjeta_Click(object sender, EventArgs e)
        {
            try
            {
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //genero la clase
                    Gestion_Api.Modelo.Pago_Tarjeta ptarjeta = new Gestion_Api.Modelo.Pago_Tarjeta();
                    ptarjeta.tarjeta.id = Convert.ToInt32(this.ListTarjetas.SelectedValue);
                    ptarjeta.tarjeta.nombre = this.ListTarjetas.SelectedItem.Text;
                    decimal monto = Convert.ToDecimal(txtMontoTarjeta.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
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
                    lstTarjetas = dtTarjeta;

                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = ptarjeta.tarjeta.nombre;
                    dr["Monto"] = ptarjeta.monto;
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(ptarjeta.monto, 2);
                    dr["IdTipoPago"] = "5";//tipo tarjeta
                    dr["FilaTipoPago"] = dtTarjeta.Rows.IndexOf(drTarjeta).ToString();
                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposTarjeta();
                    this.actualizarTotales(0);
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

        public void limpiarCamposTarjeta()
        {
            try
            {
                ListTarjetas.SelectedIndex = -1;
                txtMontoTarjeta.Text = "";
            }
            catch
            {

            }
        }

        #endregion

        #region Retenciones
        protected void btnAgregoPagoRetencion_Click(object sender, EventArgs e)
        {
            try
            {
                if ((TotalDoc.Value != "0.00") || (TotalDoc.Value == "0.00" && txtSaldoDoc.Text == "0.00"))//Si es cancelacion de fact con NC o que la imputacion sea != 0.00
                //if (TotalDoc.Value != "0.00")
                {
                    //obtengo datos
                    decimal monto = Convert.ToDecimal(txtRetencion.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    DateTime fecha = Convert.ToDateTime(txtFechaRetencion.Text, new CultureInfo("es-AR"));
                    int tipo = Convert.ToInt32(DropListTipoRetencion.SelectedValue);
                    string numero = txtNumeroRetencion.Text;
                    this.agregarRetencion(monto, fecha, tipo, numero,0);
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

        public void agregarRetencion(decimal monto, DateTime fecha, int tipo, string numero, int origen)
        {
            try
            {
                //genero la clase
                //decimal monto = Convert.ToDecimal(txtRetencion.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                DataTable dtRetencion = lstRetencion;
                DataRow drRetencion = dtRetencion.NewRow();
                drRetencion["Fecha"] = fecha;
                drRetencion["Importe"] = monto;
                drRetencion["Tipo"] = tipo;
                drRetencion["Numero"] = numero;
                //drTransferencia["CBU"] = txtCbu.Text;
                drRetencion["Monto"] = monto;
                drRetencion["Origen"] = "P";
                dtRetencion.Rows.Add(drRetencion);
                lstRetencion = dtRetencion;

                DataTable dt = lstPago;
                DataRow dr = dt.NewRow();
                dr["Tipo Pago"] = "Retencion N° " + numero;
                dr["Monto"] = decimal.Round(monto, 2);
                dr["Cotizacion"] = 1;
                dr["Total"] = decimal.Round(monto, 2);
                dr["IdTipoPago"] = "6";//tipo reten
                dr["FilaTipoPago"] = dtRetencion.Rows.IndexOf(drRetencion).ToString();

                dt.Rows.Add(dr);

                lstPago = dt;

                //llamo al metodo que muestra los pagos en la tabla
                this.cargarTablaPAgos();
                this.limpiarCamposRetencion();
                this.actualizarTotales(origen);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar retencion  " + ex.Message));
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

        #region Pago a Cuenta
        public void CargarPagoCuenta()
        {
            try
            {
                try
                {
                    DataTable dt = this.lstDocumentos;

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

                    txtSaldoDoc.Text = monto.ToString("0.00").Replace(',', '.');

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
        #endregion

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
                        if (posCheques == Convert.ToInt32(codigo[1]))
                        {
                            //quito el registro cheques y los cheques
                            DataTable dtCheques = lstCheque;
                            dtCheques.Rows.Clear();
                            lstCheque = dtCheques;
                            //this.cargarTablaCheques();
                            Session.Add("ABMCobros_PosCheque", -1);
                            dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));
                            break;
                        }
                        else
                        {
                            //quito del temporal a la forma de pago que corresponde
                            int forma = Convert.ToInt32(dr["IdTipoPago"]);
                            int fila = Convert.ToInt32(dr["FilaTipoPago"]);
                            this.quitarPagodeTemp(fila, forma);

                            //lo quito
                            dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));
                            break;
                        }
                    }
                }
                //cargo el nuevo pedido a la sesion
                lstPago = dt;

                //vuelvo a cargar los items
                this.cargarTablaPAgos();
                this.actualizarTotales(0);

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
                if (forma == 5)
                {
                    DataTable dtTarj = this.lstTarjetas;
                    dtTarj.Rows.RemoveAt(fila);
                    this.lstTarjetas = dtTarj;
                }
                //quito Retencion
                if (forma == 6)
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
        
        #region generacion PAGO
        protected void lbtnAgregarPago_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    this.generarPago();
                }
            }
            catch
            {

            }
            
        }

        public void generarPago()
        {
            try
            {
                //agrego imputaciones
                List<ImputacionesCompra> imputaciones = this.obtenerImputaciones();
                //agrego pagos
                controladorCobranza contCobranza = new controladorCobranza();

                int tieneRetencion = 0;
                if (lstRetencion.Rows.Count > 0)
                {
                    tieneRetencion = 1;
                }

                //si hay imputaciones ingreso los pagos
                List<Pago> listPago = contCobranza.obtenerPagosdesdeDT(lstMoneda, lstCheque, lstTransferencia, lstTarjetas, lstRetencion);

                PagosCompra pc = new PagosCompra();
                pc.Fecha = Convert.ToDateTime(this.txtFechaPago.Text, new CultureInfo("es-AR"));
                pc.Proveedor = this.idProveedor;
                pc.Empresa = this.idEmpresa;
                pc.Sucursal = this.idSucursal;
                pc.PuntVenta = this.puntoVenta;
                
                pc.Imputado = Convert.ToDecimal(this.TotalDoc.Value, CultureInfo.DefaultThreadCurrentUICulture);
                pc.Numero = this.txtNumeroCobro.Text;

                pc.Ingresado = Convert.ToDecimal(this.txtTotalIngresado.Text, CultureInfo.DefaultThreadCurrentUICulture);
                pc.ImputacionesCompras = imputaciones;
                pc.Total = Convert.ToDecimal(this.txtTotalIngresado.Text, CultureInfo.DefaultThreadCurrentUICulture);

                pc.PagosCompras_Observaciones = new PagosCompras_Observaciones();                
                pc.PagosCompras_Observaciones.Observaciones = this.txtObservaciones.Text;

                pc.Ftp = 0;

                if (this.bn == 2)
                    pc.Ftp = 1;

                if (pc.Ingresado >= pc.Imputado)
                {
                    int i = this.contPagos.generarPago(pc, listPago, this.lstChequeTercero2);

                    if (i > 0)
                    {
                        //Verifico si se agregaron cheques para agregarlos a la tabla Cheques_Datos
                        if (lstCheque.Rows.Count > 0 || lstChequeTercero2.Rows.Count > 0 || lstChequeTercero.Rows.Count > 0)
                            this.agregarCheques_Datos(pc.Id);

                        if (tieneRetencion > 0)
                        {
                            int idRet = this.obtenerIdRetenecionGenerada(pc);

                            //Actualizo la numeracion de retenciones
                            this.actualizarNumeracionRetenciones(lstRetencion);

                            string script = "window.open('../Compras/ImpresionRetenciones.aspx?a=1&Retencion=" + idRet + "','_blank');";
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Pago agregado. \", {type: \"info\"}); window.open('ReportesR.aspx?id=" + i + "','_blank');" + script + "location.href = 'PagosF.aspx';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"Pago agregado. \", {type: \"info\"}); window.open('ReportesR.aspx?id=" + i + "');location.href = 'PagosF.aspx';", true);                            
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudo generar pago. \");", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo generar pago. "));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"El pago es mayor a lo imputado. \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El pago es mayor a lo imputado"));
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error generando pago. " + ex.Message), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error generando pago. " + ex.Message));
                
            }
        }

        private void agregarCheques_Datos(long idPagosCompras)
        {
            try
            {
                //Con el id del PagoCompra, obtengo todos los id de los cheques generados y escribo en la tabla Cheques_Datos
                var listCheques = this.contCobranzaEnt.obtenerChequesPorIdPagos_Compras(idPagosCompras);

                foreach (int idCheque in listCheques)
                {
                    //Verifico si existe el cheque en la tabla de Cheques_Datos, si existe modifico el registro. Si no existe significa que es un cheque propio que estoy ingresando al momento de agregar el pago
                    var chd = this.contCobranzaEnt.obtenerCheques_DatosPorCheque(idCheque);
                    if (chd != null)
                    {
                        chd.Pago = idPagosCompras;
                        chd.PagoFtp = 0;
                        if (this.bn == 2)
                            chd.PagoFtp = 1;

                        int k = this.contCobranzaEnt.modificarCheques_Datos(chd);
                        if (k < 0)
                            Log.EscribirSQL(1, "ERROR", "Ocurrió un error modificando objeto Cheques_Datos con id " + chd.Id);
                    }
                    else
                    {
                        Cheques_Datos chdNuevo = new Cheques_Datos();
                        chdNuevo.Cheque = idCheque;
                        chdNuevo.Pago = idPagosCompras;
                        chdNuevo.PagoFtp = 0;
                        if (this.bn == 2)
                            chdNuevo.PagoFtp = 1;

                        int l = this.contCobranzaEnt.agregarCheques_Datos(chdNuevo);
                        if (l < 0)
                            Log.EscribirSQL(1, "ERROR", "Ocurrió un error agregando objeto Cheques_Datos al pago con id " + idPagosCompras);
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error guardando datos de cheques. Excepción: " + Ex.Message), true);
            }
        }
        public List<ImputacionesCompra> obtenerImputaciones()
        {
            try
            {
                List<ImputacionesCompra> imputaciones = new List<ImputacionesCompra>();
                foreach (Control c in phDocumentos.Controls)
                {
                    TableRow tr = c as TableRow;
                    ImputacionesCompra imp = new ImputacionesCompra();
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if (!tr.Cells[0].Text.Contains("Pago a Cuenta"))
                    {
                        var mov = controlador.obtenerMovimientoByID(Convert.ToInt64(tr.ID));
                        imp.Movimiento = mov.Id;
                        imp.Total = Convert.ToDecimal(tr.Cells[1].Text.Substring(1).Replace(',', '.'), CultureInfo.InvariantCulture);
                        if (!String.IsNullOrEmpty(txt.Text))
                        {
                            if (!tr.Cells[0].Text.Contains("Recibo de Cobro") && mov.Saldo > imp.Total)
                            {
                                decimal resto = Convert.ToDecimal(mov.Saldo - imp.Total);
                                imp.Imputar = resto + Convert.ToDecimal(txt.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                imp.Imputar = Convert.ToDecimal(txt.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            imp.Imputar = 0;
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

        public int obtenerIdRetenecionGenerada(PagosCompra pc)
        {
            try
            {
                var pago_ret = pc.Pagos_Compras.Where(x => x.TipoPago == 6).FirstOrDefault();
                return pago_ret.IdFormaPago.Value;
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        protected void txtLectorCheque_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarTablaChequesCartera();
            }
            catch
            {

            }
        }

        public void actualizarNumeracionRetenciones(DataTable lstRetenciones)
        {
            try
            {
                foreach (DataRow dr in lstRetenciones.Rows)
                {
                    int numero = Convert.ToInt32(dr["Numero"]);
                    int j = this.contFact.actualizarNumeroFactura2(numero.ToString(), this.puntoVenta, 23);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error actualizando numeracion de retenciones. Excepción: " + Ex.Message), true);
            }
        }
       
    }
}