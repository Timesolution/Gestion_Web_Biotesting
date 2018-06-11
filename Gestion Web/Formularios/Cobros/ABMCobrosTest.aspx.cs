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

namespace Gestion_Web.Formularios.Cobros
{
    public partial class ABMCobrosTest : System.Web.UI.Page
    {
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        controladorSucursal contSucu = new controladorSucursal();
        controladorCobranza contCobranza = new controladorCobranza();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        List<Imputacion> imputaciones = new List<Imputacion>();
        List<Pago> listPagoC = new List<Pago>();
        Pago_Contado pagoC = new Pago_Contado();
        Mensajes mje = new Mensajes();
        string documentos = "";
        string posCheques;

        DataTable lstPagoTemp;
        DataTable lstMonedaTemp;
        DataTable lstTransferenciaTemp;
        DataTable lstChequeTemp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                //cargo fechas en campos de fecha
                txtFechaCh.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaTransf.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaCobro.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //obtengo los movimientos
                this.documentos = Request.QueryString["documentos"];
                this.obtenerNroRecibo();

                CargarMovimientos(documentos);
                if (!IsPostBack)
                {
                    lstPagoTemp = new DataTable();
                    lstChequeTemp = new DataTable();
                    lstMonedaTemp = new DataTable();
                    lstTransferenciaTemp = new DataTable();

                    this.InicializarListaPagos();
                    this.InicializarListaMoneda();
                    this.InicializarListaTransferencia();
                    this.InicializarListaCheque();

                    //Session["ListaPAgos"] = null;
                    this.cargarTipoMoneda();
                    this.cargarBancos();
                    this.cargarBancosTransf();

                }
                this.cargarTablaPAgos();
                this.cargarTablaCheques();
                this.obtenerNroRecibo();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error cargando la pagina.  " + ex.Message));
            }
        }

        private void InicializarListaPagos()
        {
            try
            {
                lstPagoTemp.Columns.Add("Tipo Pago");
                lstPagoTemp.Columns.Add("Monto");
                lstPagoTemp.Columns.Add("Cotizacion");
                lstPagoTemp.Columns.Add("Total");
                lstPago = lstPagoTemp;
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

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

        private void InicializarListaTransferencia()
        {
            try
            {
                lstTransferenciaTemp.Columns.Add("Fecha");
                lstTransferenciaTemp.Columns.Add("Importe");
                lstTransferenciaTemp.Columns.Add("Banco");
                lstTransferenciaTemp.Columns.Add("Banco Entidad");
                lstTransferenciaTemp.Columns.Add("Cuenta");
                lstTransferenciaTemp.Columns.Add("CBU");
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

        public void CargarMovimientos(string doc)
        {
            try
            {
                string[] documento = doc.Split(new Char[] { ';' });
                List<Movimiento> movimiento = new List<Movimiento>();
                decimal totalSaldo = 0;
                foreach (string d in documento)
                {
                    if (!String.IsNullOrEmpty(d))
                    {
                        Movimiento m = controlador.obtenerMovimientoID(Convert.ToInt32(d));
                        totalSaldo += m.saldo;
                        this.cargarEnPh(m);

                    }
                }
                labelSaldo.Text = "$ " + totalSaldo.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos " + ex.Message));

            }

        }

        private void cargarEnPh(Movimiento m)
        {
            MovimientoView movV = new MovimientoView();
            movV = m.ListarMovimiento();
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = movV.id.ToString();

                //Celdas

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$ " + movV.saldo;
                celSaldo.Width = Unit.Percentage(15);
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSaldo);

                TableCell celImputar = new TableCell();
                TextBox txtImputar = new TextBox();
                txtImputar.CssClass = "form-control";
                txtImputar.AutoPostBack = true;
                txtImputar.ID = "txtImputar_" + movV.id;
                txtImputar.TextChanged += new EventHandler(this.ActualizarTotales);
                txtImputar.Attributes.Add("onpresskey", "javascript:return validarNro(event)");
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
                            ClientScript.RegisterClientScriptBlock(UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("El monto a imputar es mayor al saldo del documento"));
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("factura agregada", null));
                        }

                    }
                }
                labelTotal.Text = "$ " + suma.ToString();

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
        
        protected DataTable lstPago
        {
            
            get
            {
                if(ViewState["ListaPagos"] != null)
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
                    return lstPagoTemp;
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
                    return lstChequeTemp;
                }
            }
            set
            {
                ViewState["ListaTransferencia"] = value;
            }
        }

        public decimal cargarEnPH(DataRow dr)
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
                celMonto.Text = "$ " + dr["Monto"].ToString();
                celMonto.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMonto);

                TableCell celCotizacion = new TableCell();
                celCotizacion.Text = dr["Cotizacion"].ToString();
                celCotizacion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCotizacion);

                //decimal montoC = Convert.ToDecimal(dr[1]) * Convert.ToDecimal(dr[2]);
                TableCell celTotal = new TableCell();
                celTotal.Text = "$ " + dr["Total"].ToString();
                celTotal.Width = Unit.Percentage(15);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                phEfectivo.Controls.Add(tr);

                return Convert.ToDecimal(dr["Total"]);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando montos " + ex.Message));
                return 0;

            }
        }

        public void cargarEnPHCheque(DataRow dr, int pos)
        {

            try
            {
                TableRow tr = new TableRow();


                TableCell celMoneda = new TableCell();
                celMoneda.Text = "Cheque N° " + dr["Numero"].ToString();
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMoneda);

                TableCell celMonto = new TableCell();
                celMonto.Text = "$ " +  dr["Importe"].ToString();
                celMonto.Width = Unit.Percentage(20);
                celMonto.HorizontalAlign = HorizontalAlign.Right;
                celMonto.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMonto);

                TableCell celAccion2 = new TableCell();
                Button btnEditar = new Button();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.ID = "btnEditar_" + pos;
                btnEditar.Text = "Editar";
                btnEditar.Click += new EventHandler(this.EditarCheque);
                celAccion2.Controls.Add(btnEditar);
                celAccion2.Width = Unit.Percentage(10);
                celAccion2.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion2);

                phCheques.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando cheques a la lista. " + ex.Message));
            }
        }

        private void EditarCheque(object sender, EventArgs e)
        {
            try
            {
                string posCh = Session["ABMCobros_PosCheque"] as string;
                string[] cheques = posCh.Split(new Char[] { ';' });
                DataTable dt = lstCheque;
                string[] Cheque = (sender as Button).ID.Split('_');
                int posCheque = Convert.ToInt32(Cheque[1]);
                DataRow dr = dt.Rows[Convert.ToInt32(posCheque)];
                txtImporteCh.Text = dr["Importe"].ToString();
                txtNumeroCh.Text = dr["Numero"].ToString();
                DropListBancoCh.SelectedValue = dr["Banco"].ToString();
                txtCuentaCh.Text = dr["Cuenta"].ToString();
                txtCuitCh.Text = dr["Cuit"].ToString();
                txtLibradorCh.Text = dr["Librador"].ToString();

                DataTable dtPago = lstPago;
                dtPago.Rows.RemoveAt(Convert.ToInt32(cheques[dt.Rows.IndexOf(dr)]));

                dt.Rows.Remove(dr);
                lstCheque = dt;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error editando direccion" + ex.Message));
            }
        }

        protected void btnAgregarPagoM_Click(object sender, EventArgs e)
        {
            try
            {
                if (labelTotal.Text != "$ 0" && labelTotal.Text != "0")
                {
                    //genero la clase
                    Pago_Contado p = new Pago_Contado();
                    p.moneda.id = Convert.ToInt32(this.DropListTipo.SelectedValue);
                    p.moneda.moneda = this.DropListTipo.SelectedItem.Text;
                    string monto = txtMonto.Text.ToString().Replace('.', ',');
                    p.monto = Convert.ToDecimal(monto);
                    p.moneda.cambio = contCobranza.obtenerCotizacion(Convert.ToInt32(this.DropListTipo.SelectedValue));


                    DataTable dtMoneda = lstMoneda;
                    DataRow drMoneda = dtMoneda.NewRow();
                    drMoneda["Id Pago"] = p.moneda.id;
                    drMoneda["Tipo Pago"] = p.moneda.moneda;
                    drMoneda["Monto"] = p.monto;
                    drMoneda["Cotizacion"] = p.moneda.cambio;
                    drMoneda["Total"] = p.monto * p.moneda.cambio;

                    dtMoneda.Rows.Add(drMoneda);
                    lstMoneda = dtMoneda;

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = p.moneda.moneda;
                    dr["Monto"] = p.monto;
                    dr["Cotizacion"] = p.moneda.cambio;
                    dr["Total"] = p.monto * p.moneda.cambio;

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposM();
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Debe ingresar importes", null));
                    this.limpiarCamposM();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        public void limpiarCamposM()
        {
            DropListTipo.SelectedIndex = -1;
            txtMonto.Text = "";
            txtCotizacion.Text = "";
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
               

            }
            catch
            {

            }
        }

        private void cargarTablaPAgos()
        {
            try
            {
                   DataTable dt = lstPago;

                    //limpio el Place holder
                    this.phEfectivo.Controls.Clear();
                    decimal saldo = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        //que me cargue la tabla, recibiendo una clase PAgo_contado
                        saldo += this.cargarEnPH(dr);

                    }
                    txtTotal.Text = "$ " + saldo.ToString();
                    txtTotalCobro.Text = "$ " + saldo.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista Pagos " + ex.Message));
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
                    this.cargarEnPHCheque(dr,pos);

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando lista cheques " + ex.Message));
            }
        }

        protected void btnAgregarPagoCh_Click(object sender, EventArgs e)
        {
            try
            {
                if (labelTotal.Text != "$ 0" && labelTotal.Text != "0")
                {
                    //genero la clase
                    string monto = txtImporteCh.Text.ToString().Replace('.', ',');
                    DataTable dtCheque = lstCheque;
                    DataRow drCheque = dtCheque.NewRow();
                    drCheque["Fecha"] = Convert.ToDateTime(txtFechaCh.Text, new CultureInfo("es-AR"));
                    drCheque["Importe"] = Convert.ToDecimal(monto);
                    drCheque["Numero"] = Convert.ToDecimal(txtNumeroCh.Text);
                    drCheque["Banco"] = Convert.ToInt32(DropListBancoCh.SelectedValue);
                    drCheque["Banco Entidad"] = DropListBancoCh.SelectedItem.Text;
                    drCheque["Cuenta"] = txtCuentaCh.Text.ToString();
                    drCheque["Cuit"] = txtCuitCh.Text;
                    drCheque["Librador"] = txtLibradorCh.Text;
                    drCheque["Monto"] = Convert.ToDecimal(monto);
                    dtCheque.Rows.Add(drCheque);
                    lstCheque = dtCheque;


                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Cheque N° " + Convert.ToDecimal(txtNumeroCh.Text);
                    dr["Monto"] = Convert.ToDecimal(monto);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = Convert.ToDecimal(monto);

                    dt.Rows.Add(dr);


                    if (Session["ABMCobros_PosCheque"] == null)
                    {
                        Session.Add("ABMCobros_PosCheque", posCheques);

                    }
                    posCheques = Session["ABMCobros_PosCheque"] as string;
                    posCheques += dt.Rows.IndexOf(dr).ToString() + ";";
                    Session.Add("ABMCobros_PosCheque", posCheques);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.cargarTablaCheques();
                    this.limpiarCamposCh();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Debe ingresar importes", null)); 
                    this.limpiarCamposCh();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        protected void btnAgregarPago_Click(object sender, EventArgs e)
        {
            this.hacerCobro();
        }

        public List<Imputacion> obtenerImputaciones()
        {
            try
            {

                foreach (Control c in phDocumentos.Controls)
                {
                    Imputacion imp = new Imputacion();
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    txt.Text = txt.Text.Replace('.', ',');
                    imp.movimiento = controlador.obtenerMovimientoID(Convert.ToInt32(tr.ID));
                    //Movimiento m = controlador.obtenerMovimientoID(Convert.ToInt32(tr.ID));
                    //MovimientoView movV = new MovimientoView();
                    //movV = m.ListarMovimiento();
                    //imp.id_doc = movV.id_doc;
                    //imp.tp = movV.tipo;
                    //imp.saldo = Convert.ToDecimal(movV.saldo); 
                    imp.total = Convert.ToDecimal(tr.Cells[1].Text.Substring(1));
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        imp.imputar = Convert.ToDecimal(txt.Text);
                    }
                    else
                    {
                        imp.imputar = 0;
                    }
                    imputaciones.Add(imp);

                }
                return imputaciones;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ha ocurrido un error obteniendo Lista de Imputaciones. " + ex.Message));
                return null;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAgregarPagoTrans_Click(object sender, EventArgs e)
        {
            try
            {
                if (labelTotal.Text != "$ 0" && labelTotal.Text != "0")
                {
                    //genero la clase
                    string monto = txtImporteTransf.Text.ToString().Replace('.', ',');
                    DataTable dtTransferencia = lstTransferencia;
                    DataRow drTransferencia = dtTransferencia.NewRow();
                    drTransferencia["Fecha"] = Convert.ToDateTime(txtFechaTransf.Text,new CultureInfo("es-AR"));
                    drTransferencia["Importe"] = Convert.ToDecimal(monto);
                    drTransferencia["Banco"] = Convert.ToInt32(DropListBancoTransf.SelectedValue);
                    drTransferencia["Banco Entidad"] = DropListBancoTransf.SelectedItem.Text;
                    drTransferencia["Cuenta"] = txtCuentaTransf.Text;
                    
                    drTransferencia["Monto"] = Convert.ToDecimal(monto);
                    dtTransferencia.Rows.Add(drTransferencia);
                    lstTransferencia = dtTransferencia;

                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Transferencia N° " + txtCuentaTransf.Text;
                    dr["Monto"] = Convert.ToDecimal(monto);
                    dr["Cotizacion"] = 1;
                    dr["Total"] = Convert.ToDecimal(monto);

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.limpiarCamposTransf();
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar importes"));
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Debe ingresar importes", null));
                    this.limpiarCamposCh();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar pago  " + ex.Message));
            }
        }

        private void hacerCobro()
        {
            try
            {
                List<Imputacion> imputaciones = obtenerImputaciones();

                if (imputaciones != null)
                {
                    List<Pago> listPago = null;
                    if (listPago != null)
                    {
                        string idCliente = Session["ClienteCobranza"] as string;

                        Cobro cobro = new Cobro();
                        cobro.fecha = DateTime.Now;
                        cobro.cliente.id = Convert.ToInt32(idCliente);
                        cobro.Doc_imputar = imputaciones;
                        cobro.pagos = listPago;
                        cobro.puntoVenta.puntoVenta = Session["PuntoVentaCobranza"] as string;
                        cobro.empresa.id = Convert.ToInt32(Session["CobroEmpresa"] as string);
                        cobro.sucursal.id = Convert.ToInt32(Session["CobroSucursal"] as string);
                        //cobro.puntoVenta.puntoVenta = "0006";
                        //cobro.empresa.id = 1;
                        //cobro.sucursal.id = 1;
                        //total por el que se realiza el pago
                        cobro.total = Convert.ToDecimal(txtTotal.Text.Substring(1));
                        //total por el que se realizan imputaciones
                        cobro.imputado = Convert.ToDecimal(this.labelTotal.Text.Substring(1));
                        cobro.ingresado = Convert.ToDecimal(this.txtTotal.Text.Substring(1));
                        int i = contCobranza.ProcesarCobro(cobro,-1,1);
                        if (i > 0)
                        {
                            lstPago = null;
                            lstCheque = null;
                            lstMoneda = null;
                            lstTransferencia = null;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Cobro Agregado", "CobranzaF.aspx"));

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo agregar cobro "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargaron pagos "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargaron imputaciones "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo agregar cobro  " + ex.Message));
            }
        }

        protected void DropListTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCotizacion.Text = contCobranza.obtenerCotizacion(Convert.ToInt32(DropListTipo.SelectedValue)).ToString();
            txtMonto.Focus();
        }

        private void obtenerNroRecibo()
        {
            try
            {
                string NroPuntoVenta = Session["PuntoVentaCobranza"] as string;
                int idEmpresa = Convert.ToInt32(Session["CobroEmpresa"] as string);
                int idSucursal = Convert.ToInt32(Session["CobroSucursal"] as string);
                PuntoVenta ptoVenta = contSucu.obtenerPuntoVentaPV(NroPuntoVenta, idSucursal, idEmpresa);
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.contCobranza.obtenerReciboNumero(ptoVenta.id, "Recibo de Cobro");
                this.txtNumeroCobro.Text = NroPuntoVenta + "-" + nro.ToString().PadLeft(8, '0');

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Cobro. " + ex.Message));
            }
        }
    }
}