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

namespace Gestion_Web.Formularios.Personal
{
    public partial class PagoRemuneracionABM : System.Web.UI.Page
    {
        controladorPagos contPagos = new controladorPagos();
        controladorCCEmpleado controlador = new controladorCCEmpleado();
        controladorSucursal contSucu = new controladorSucursal();
        controladorCobranza contCobranza = new controladorCobranza();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        ControladorClienteEntity contCliEnt = new ControladorClienteEntity();
        ControladorEmpresa contEmp = new ControladorEmpresa();
        controladorTarjeta contTarjeta = new controladorTarjeta();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();

        controladorEmpleado contEmpleados = new controladorEmpleado();

        Mensajes mje = new Mensajes();
        string empleados = "";
        string documentos = "";
        private int idProveedor;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private decimal monto;
        private int tipoDoc;
        int posCheques;
        int accion;

        DataTable lstPagoTemp;
        DataTable lstMonedaTemp;
        DataTable lstTransferenciaTemp;
        DataTable lstChequeTemp;
        DataTable lstChequeTercerosTemp;
        DataTable lstChequeTerceros2Temp;
        DataTable lstDocumentosTemp;
        DataTable lstTarjetasTemp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.documentos = Request.QueryString["d"];
                this.empleados = Request.QueryString["emp"];
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.monto = Convert.ToDecimal(Request.QueryString["m"].Replace(',', '.'), CultureInfo.InvariantCulture);

                if (!IsPostBack)
                {

                    this.txtFechaCh.Text = DateTime.Now.ToString("dd/MM/yyyy");                    
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

                    this.InicializarListaPagos();
                    this.InicializarListaMoneda();
                    this.InicializarListaTransferencia();
                    this.InicializarListaCheque();
                    this.InicializarListaChequeTercero();
                    this.InicializarListaChequeTercero2();
                    this.InicializarListaDocumentos();
                    this.InicializarListaTarjeta();

                    //Session["ListaPAgos"] = null;
                    this.cargarTipoMoneda();
                    //this.cargarBancos();
                    this.cargarCuentas();
                    this.cargarBancosTransf();
                    this.cargarTarjetas();

                    //pago normal
                    if (this.accion == 1)
                    {
                        this.CargarMovimientos(documentos);
                    }
                    //pago a cuenta, favor del empleado
                    if (this.accion == 2)
                    {
                        this.CargarPagoCuenta();
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
                        if (s == "7")//personal
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
                lstPagoTemp.Columns.Add("Tipo Pago");
                lstPagoTemp.Columns.Add("Monto");
                lstPagoTemp.Columns.Add("Cotizacion");
                lstPagoTemp.Columns.Add("Total");
                lstPagoTemp.Columns.Add("ID");
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
                //lstDocumentosTemp.Columns.Add("Id");
                //lstDocumentosTemp.Columns.Add("Nombre");
                //lstDocumentosTemp.Columns.Add("Apellido");
                //lstDocumentosTemp.Columns.Add("Remuneracion");        
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
                lstTransferencia = lstTransferenciaTemp;
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

        #region eventos pantalla

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

        #endregion

        #region carga de remuneraciones a pagar
        public void CargarMovimientos(string doc)
        {
            try
            {
                //decimal totalSaldo = 0;                
                //foreach (string idEmpleado in empleados.Split(';'))
                //{
                //    if (!String.IsNullOrEmpty(idEmpleado))
                //    {
                //        Empleado emp = this.contEmpleados.obtenerEmpleadoID(Convert.ToInt32(idEmpleado));
                //        totalSaldo += (decimal)emp.remuneracion;
                //        this.cargarMovimientoDT(emp);
                //    }
                //}

                //txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');
                List<MovimientosCCE> movimientos = this.controlador.obtenerMovimientos(doc);
                decimal totalSaldo = 0;
                foreach (MovimientosCCE m in movimientos)
                {                    
                    totalSaldo += (decimal)m.Saldo;
                    
                    this.cargarMovimientoDT(m);
                }
                txtSaldoDoc.Text = totalSaldo.ToString("0.00").Replace(',', '.');
                
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
                int nro = this.contFact.obtenerFacturaNumero(this.puntoVenta, "Pago Empleado a Cuenta");
                string numero = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                return numero;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
                return null;
            }
        }

        //public void cargarMovimientoDT(Empleado emp)
        //{
        //    try
        //    {
        //        DataTable dt = lstDocumentos;

        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = emp.id;
        //        dr["Nombre"] = emp.nombre;
        //        dr["Apellido"] = emp.apellido;
        //        dr["Remuneracion"] = emp.remuneracion;              


        //        dt.Rows.Add(dr);
        //        int pos = dt.Rows.IndexOf(dr);
        //        this.cargarDocumentoEnPh(dr, pos);
        //        lstDocumentos = dt;               

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos a DT. " + ex.Message));

        //    }
        //}

        public void cargarMovimientoDT(MovimientosCCE m)
        {
            try
            {
                DataTable dt = lstDocumentos;

                DataRow dr = dt.NewRow();
                dr["Id"] = m.Id;
                if(m.TipoDocumento == 28)
                    dr["Tipo"] = "Remuneracion ";
                else
                    dr["Tipo"] = "Pago Empleado a Cuenta ";
                dr["Numero"] = m.Numero;
                dr["Saldo"] = m.Saldo;
                dr["Imputar"] = m.Saldo;


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

                //TableCell celNumero = new TableCell();
                //celNumero.Text = dr["Nombre"].ToString() + " " + dr["Apellido"].ToString();
                //celNumero.VerticalAlign = VerticalAlign.Middle;
                //celNumero.Width = Unit.Percentage(70);
                //tr.Cells.Add(celNumero);
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
                txtImputar.Text = dr["Saldo"].ToString().Replace(',', '.');
                txtImputar.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtImputar.ID = "Text_" + pos.ToString();                
                txtImputar.AutoPostBack = true;
                txtImputar.Style.Add("text-align", "right");
                txtImputar.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtImputar.Attributes.Add("onchange", "javascript:TotalImputado();");
                if (dr["Tipo"].ToString().Contains("Pago Empleado a Cuenta") && this.accion == 1)
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
                    totalSaldo += Convert.ToDecimal(dr["Saldo"]);

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


                //limpio el Place holder
                this.phChequeCartera.Controls.Clear();
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

                //TableCell celAccion2 = new TableCell();
                //LinkButton btnEditar = new LinkButton();
                //btnEditar.CssClass = "btn btn-info";
                //btnEditar.ID = "btnEditarTercero_" + idCheque;
                //btnEditar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEditar.Click += new EventHandler(this.EliminarChequeTercero);
                //celAccion2.Controls.Add(btnEditar);
                //celAccion2.Width = Unit.Percentage(5);
                //celAccion2.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion2);
               

                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + idCheque;
                cbSeleccion.CssClass = "btn btn-info";
                celSeleccion.Controls.Add(cbSeleccion);
                celSeleccion.Width = Unit.Percentage(5);
                //celSeleccion.VerticalAlign = VerticalAlign.Middle;
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
                    CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
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
                    dtTransferencia.Rows.Add(drTransferencia);
                    lstTransferencia = dtTransferencia;

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Transferencia N° " + txtCuentaTransf.Text;
                    dr["Monto"] = decimal.Round(monto, 2);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = decimal.Round(monto, 2);

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
                    dr["Tipo"] = "Pago Empleado a Cuenta N° ";
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
        
        #region generacion PAGO
        protected void lbtnAgregarPago_Click(object sender, EventArgs e)
        {
            this.generarPago();
        }

        public void generarPago()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtDocumentos = this.lstDocumentos;

                foreach (DataRow row in dtDocumentos.Rows)
                {
                    var imputado = this.phDocumentos.Controls[dtDocumentos.Rows.IndexOf(row)] as TableRow;
                    TextBox txtValorImputado = imputado.Cells[2].Controls[0] as TextBox;

                    row["Imputar"] = txtValorImputado.Text;
                }

                //agrego imputaciones
                List<ImputacionesEmpleado> imputaciones = this.obtenerImputaciones();

                List<Pago> listPago = contCobranza.obtenerPagosdesdeDT(lstMoneda, lstCheque, lstTransferencia, lstTarjetas, dt);// paso un dt vacio para poder reutilizar la fc

                PagoRemuneracione pago = new PagoRemuneracione();
                pago.Fecha = Convert.ToDateTime(this.txtFechaPago.Text, new CultureInfo("es-AR"));
                pago.Empleado = Convert.ToInt32(this.empleados.Split(';')[0]);
                pago.Empresa = this.idEmpresa;
                pago.Sucursal = this.idSucursal;
                pago.PuntoVta = this.puntoVenta;
                pago.Numero = this.txtNumeroCobro.Text;

                pago.Ingresado = Convert.ToDecimal(this.txtTotalIngresado.Text, CultureInfo.DefaultThreadCurrentUICulture);
                pago.Imputado = Convert.ToDecimal(this.TotalDoc.Value, CultureInfo.DefaultThreadCurrentUICulture);
                pago.Total = Convert.ToDecimal(this.txtTotalIngresado.Text, CultureInfo.DefaultThreadCurrentUICulture);

                pago.ImputacionesEmpleados = imputaciones;

                int i = 0;
                if (this.accion == 2)
                {
                    i = this.contPagos.agregarPagoCuentaEmpleado(pago, listPago, dtDocumentos, this.lstChequeTercero2);
                }
                else
                {
                    i = this.contPagos.generarPagoRemuneracion(pago, listPago, dtDocumentos, this.lstChequeTercero2);
                }

                if (i > 0)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Remuneracion agregada con exito.","Empleados.aspx"));                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReportesR.aspx?a=1&p=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');;location.href = 'PagosEmpleadosF.aspx';", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo generar pago remuneracion."));
                }
            }
            catch(Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error generando pago. " + ex.Message));
                
            }
        }

        public List<ImputacionesEmpleado> obtenerImputaciones()
        {
            try
            {
                List<ImputacionesEmpleado> imputaciones = new List<ImputacionesEmpleado>();
                foreach (Control c in phDocumentos.Controls)
                {
                    TableRow tr = c as TableRow;
                    ImputacionesEmpleado imp = new ImputacionesEmpleado();
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if (!tr.Cells[0].Text.Contains("Pago Empleado a Cuenta"))
                    {
                        var mov = controlador.obtenerMovimientoByID(Convert.ToInt32(tr.ID));
                        imp.Movimiento = mov.Id;
                        imp.Total = Convert.ToDecimal(tr.Cells[1].Text.Substring(1).Replace(',', '.'), CultureInfo.InvariantCulture);
                        if (!String.IsNullOrEmpty(txt.Text))
                        {
                            if (!tr.Cells[0].Text.Contains("Remuneracion") && mov.Saldo > imp.Total)
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
        

        #endregion
                       
      

        

        

        

        

       
    }
}