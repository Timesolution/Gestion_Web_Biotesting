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

namespace Gestion_Web.Formularios.Valores
{
    public partial class BancosF : System.Web.UI.Page
    {        
        controladorUsuario contUser = new controladorUsuario();
        ControladorBanco contBanco = new ControladorBanco();
        controladorTarjeta contTarj = new controladorTarjeta();
        controladorCobranza contCobranza = new controladorCobranza();
        controladorPagos contPagos = new controladorPagos();
        Mensajes m = new Mensajes();

        private int cuenta;        
        private string fechaD;
        private string fechaH;
        private string cuit;
        private int concepto;
        private int tipoConcepto;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];                
                this.cuit = Request.QueryString["cuit"];
                this.cuenta = Convert.ToInt32(Request.QueryString["cta"]);
                this.concepto = Convert.ToInt32(Request.QueryString["c"]);
                this.tipoConcepto = Convert.ToInt32(Request.QueryString["tc"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null)
                    {
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                    }

                    this.cargarEmpresas();
                    this.cargarConceptos();
                    this.obtenerDatosBancos();
                    
                    this.txtFechaDesde.Text = fechaD;
                    this.txtFechaHasta.Text = fechaH;
                    this.txtFechaAgregar.Text = fechaD;
                    this.cargarCuentas(this.cuit);
                    this.ListEmpresa.SelectedValue = this.cuit;
                    this.ListCuentas.SelectedValue = this.cuenta.ToString();
                    this.ListCtaBancariasAgregar.SelectedValue = this.cuenta.ToString();
                }
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
                        if (s == "44")
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
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Cuit";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();

                this.ListEmpresasAgregar.DataSource = dt;
                this.ListEmpresasAgregar.DataValueField = "Cuit";
                this.ListEmpresasAgregar.DataTextField = "Razon Social";
                this.ListEmpresasAgregar.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        private void cargarConceptos()
        {
            try
            {                
                List<Bancos_Conceptos> conceptos = this.contBanco.obtenerConceptosBancarios();

                this.ListConceptosAgregar.DataSource = conceptos;
                this.ListConceptosAgregar.DataTextField = "Descripcion";
                this.ListConceptosAgregar.DataValueField = "Id";
                this.ListConceptosAgregar.DataBind();

                this.ListConceptosAgregar.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarConceptosBuscar(int tipoConcepto)
        {
            try
            {
                if (tipoConcepto == 1)
                {
                    List<Bancos_Conceptos> conceptos = this.contBanco.obtenerConceptosBancarios();
                    this.ListConcepto.DataSource = conceptos;
                    this.ListConcepto.DataTextField = "Descripcion";
                    this.ListConcepto.DataValueField = "Id";
                    this.ListConcepto.DataBind();

                    if (conceptos != null && conceptos.Count > 1)
                    {
                        this.ListConcepto.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                        this.ListConcepto.Items.Insert(1, new ListItem("Todos", "0"));
                    }
                }

                if (tipoConcepto == 2)
                {
                    this.ListConcepto.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    this.ListConcepto.Items.Insert(1, new ListItem("Todos", "0"));
                    this.ListConcepto.Items.Insert(2, new ListItem("Cheques", "1"));
                    this.ListConcepto.Items.Insert(3, new ListItem("Transferencias", "2"));
                    this.ListConcepto.Items.Insert(4, new ListItem("Liquidaciones", "3"));
                }
               
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando conceptos a la lista. Excepción: " + Ex.Message));
            }
        }
        private void cargarCuentas(string cuit)
        {
            try
            {
                this.ListCuentas.Items.Clear();                
                cuit = cuit.Replace("-","");
                List<CuentasBancaria> cuentas = this.contBanco.obtenerCuentasBancariasByCuit(cuit);

                foreach (var cta in cuentas)
                {
                    string text = cta.Banco1.entidad + " - " + cta.Numero;
                    this.ListCuentas.Items.Add(new ListItem(text, cta.Id.ToString()));                    
                }

                this.ListCuentas.Items.Insert(0, new ListItem("Todas", "0"));                
            }
            catch
            {

            }
        }
        private void obtenerDatosBancos()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));
                hasta = hasta.AddHours(23).AddMinutes(59);
                this.cuit = this.cuit.Replace("-", "");

                List<Gestion_Api.Entitys.Cheque> list = new List<Gestion_Api.Entitys.Cheque>();
                List<Gestion_Api.Entitys.Transferencia> listTransferencias = new List<Gestion_Api.Entitys.Transferencia>();
                List<Gestion_Api.Entitys.Liquidacione> listLiquidaciones = new List<Gestion_Api.Entitys.Liquidacione>();
                List<Gestion_Api.Entitys.CuentasBancarias_Movimientos> listMovCuentas = new List<Gestion_Api.Entitys.CuentasBancarias_Movimientos>();


                //Verifico si filtró por tipo de evento
                if (this.tipoConcepto != 0)
                {
                    //Si filtró por tipo de concepto, verifico cual seleccionó

                    //Conceptos Manuales
                    if (this.tipoConcepto == 1)
                    {
                        //Si seleccionó todos, concepto = 0, de lo contrario filtro por IdConcepto
                        listMovCuentas = this.contBanco.obtenerMovimientosCuentas(desde, hasta, this.cuit, this.cuenta);
                        if (this.concepto != 0)
                            listMovCuentas = listMovCuentas.Where(x => x.Bancos_Conceptos.Id == this.concepto).ToList();
                    }
                    //Conceptos Automaticos
                    if (this.tipoConcepto == 2)
                    {
                        if (this.concepto != 0)
                        {
                            if (this.concepto == 1)
                                list = this.contBanco.obtenerListChequesEnBancos(desde, hasta, this.cuit, this.cuenta);
                            if (this.concepto == 2)
                                listTransferencias = this.contBanco.obtenerListTransferenciasBancos(desde, hasta, this.cuit, this.cuenta);
                            if (this.concepto == 3)
                                listLiquidaciones = this.contTarj.obtenerLiquidacionesByFechaCuenta(desde, hasta, this.cuit, this.cuenta);
                        }
                        else
                        {
                            list = this.contBanco.obtenerListChequesEnBancos(desde, hasta, this.cuit, this.cuenta);
                            listTransferencias = this.contBanco.obtenerListTransferenciasBancos(desde, hasta, this.cuit, this.cuenta);
                            listLiquidaciones = this.contTarj.obtenerLiquidacionesByFechaCuenta(desde, hasta, this.cuit, this.cuenta);
                        }
                    }
                }
                else
                {
                    list = this.contBanco.obtenerListChequesEnBancos(desde, hasta, this.cuit, this.cuenta);
                    listTransferencias = this.contBanco.obtenerListTransferenciasBancos(desde, hasta, this.cuit, this.cuenta);
                    listLiquidaciones = this.contTarj.obtenerLiquidacionesByFechaCuenta(desde, hasta, this.cuit, this.cuenta);
                    listMovCuentas = this.contBanco.obtenerMovimientosCuentas(desde, hasta, this.cuit, this.cuenta);
                }
               
                DataTable dt = this.cargarTable(list, listTransferencias, listLiquidaciones, listMovCuentas);

                if (dt != null)
                {
                    decimal saldo = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        saldo += Convert.ToDecimal(row["Importe"]);
                        this.cargarDatosPH(row, saldo);
                    }
                    this.lblSaldo.Text = saldo.ToString("'$'#,0.00");
                }
                this.cargarDatosLabel();
            }
            catch
            {

            }
        }
        private void cargarDatosLabel()
        {
            try
            {
                if (this.cuenta > 0)
                {
                    var cuenta = this.contBanco.obtenerCuentaBancariaByID(this.cuenta);
                    this.lblDatosCuenta.Text = cuenta.Banco1.entidad + " - " + cuenta.Numero + " - " + cuenta.Descripcion;
                }
            }
            catch
            {

            }
        }
        private void cargarDatosPH(DataRow row,decimal acumulado)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = row["Id"].ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = row["Fecha"].ToString();
                tr.Controls.Add(celFecha);

                TableCell celNumero = new TableCell();
                //if (row["NroCheque"].ToString() != "")                    
                //else
                //    celNumero.Text = row["NroTransferencia"].ToString();
                celNumero.Text = row["Numero"].ToString();
                celNumero.HorizontalAlign = HorizontalAlign.Right;
                tr.Controls.Add(celNumero);

                TableCell celConcepto = new TableCell();
                celConcepto.Text = row["Concepto"].ToString();
                celConcepto.HorizontalAlign = HorizontalAlign.Center;                
                tr.Controls.Add(celConcepto);

                TableCell celObservacion = new TableCell();
                celObservacion.Text = row["Observaciones"].ToString();
                celObservacion.Width = Unit.Percentage(25);
                tr.Controls.Add(celObservacion);

                TableCell celImporte = new TableCell();
                celImporte.Text = Convert.ToDecimal(row["Importe"]).ToString("'$'#,0.00");
                celImporte.HorizontalAlign = HorizontalAlign.Right;                
                tr.Controls.Add(celImporte);

                TableCell celAcumulado = new TableCell();
                celAcumulado.Text = acumulado.ToString("'$'#,0.00");
                celAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Controls.Add(celAcumulado);

                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                //btnDetalles.ID = "btnSelec_" + t.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnDetalles.Click += new EventHandler(this.detallePago);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                //btnEliminar.ID = "btnEliminar_" + p.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.OnClientClick = "abrirdialog(" + p.Id + ");";
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);

                this.phBancos.Controls.Add(tr);
            }
            catch
            {

            }
        }
        private DataTable cargarTable(List<Gestion_Api.Entitys.Cheque> cheques, List<Gestion_Api.Entitys.Transferencia> transferencias, List<Gestion_Api.Entitys.Liquidacione> liquidaciones,List<Gestion_Api.Entitys.CuentasBancarias_Movimientos> movCuentas)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("Id");
                dtDatos.Columns.Add("Fecha",typeof(DateTime));
                dtDatos.Columns.Add("Numero");
                //dtDatos.Columns.Add("NroTransferencia");
                dtDatos.Columns.Add("Concepto");
                dtDatos.Columns.Add("Importe");
                dtDatos.Columns.Add("Observaciones");

                foreach (var c in cheques)
                {
                    DataRow row = dtDatos.NewRow();
                    row["Id"] = "cheque_" + c.id;
                    row["Fecha"] = c.fecha.Value;
                    row["Numero"] = c.numero.Value.ToString();
                    if (c.estado == 5)
                    {
                        row["Concepto"] = "Imputado a cta.";
                        row["Importe"] = c.importe.Value;
                    }
                    else
                    {
                        row["Concepto"] = "Debitado de cta.";
                        row["Importe"] = c.importe.Value * -1;
                    }
                    row["Observaciones"] = this.contCobranza.obtenerObservacionChequeManual(c.id);
                    dtDatos.Rows.Add(row);
                }

                foreach (var t in transferencias)
                {
                    DataRow row = dtDatos.NewRow();
                    row["Id"] = "transferencia_" + t.id;
                    row["Fecha"] = t.fecha.Value;
                    var pago = this.contPagos.obtenerPagoDesdeTransferencia(t);
                    row["Observaciones"] = "";//this.contCobranza.obtenerObservacionChequeManual(c.id);
                    if (pago != null)
                    {
                        row["Importe"] = t.monto.Value * -1;
                        row["Observaciones"] = "Pago Nº: " + pago.Numero;
                    }
                    else
                    {
                        row["Importe"] = t.monto.Value;
                    }
                    //row["NroCheque"] = "-";
                    row["Numero"] = t.nro_cuenta;
                    row["Concepto"] = "Transferencia";
                    
                   
                    dtDatos.Rows.Add(row);
                }

                foreach (var l in liquidaciones)
                {
                    DataRow row = dtDatos.NewRow();
                    row["Id"] = "liquidacion_" + l.Id;
                    row["Fecha"] = l.FechaAcreditacion.Value;
                    row["Importe"] = l.TotalLiquidar.Value;
                    row["Numero"] = l.NroLiquidacion;
                    row["Concepto"] = "Liquidacion tarjeta";
                    row["Observaciones"] = "";
                    dtDatos.Rows.Add(row);
                }

                foreach (var mov in movCuentas)
                {
                    DataRow row = dtDatos.NewRow();
                    row["Id"] = "movCuentas_" + mov.Id;
                    row["Fecha"] = mov.Fecha.Value;
                    row["Importe"] = mov.Monto.Value * -1;
                    row["Numero"] = mov.CuentasBancaria.Numero;
                    row["Concepto"] = mov.Bancos_Conceptos.Descripcion;
                    row["Observaciones"] = mov.Observaciones;
                    dtDatos.Rows.Add(row);
                }

                DataView dv = dtDatos.DefaultView;
                dv.Sort = "Fecha";
                DataTable sortedDT = dv.ToTable();
                return sortedDT;
            }
            catch
            {
                return null;
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("BancosF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&cuit=" + this.ListEmpresa.SelectedValue + "&cta=" + this.ListCuentas.SelectedValue + "&c=" + this.ListConcepto.SelectedValue + "&tc=" + this.ListTipoConceptos.SelectedValue);
            }
            catch
            {

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ListEmpresa.SelectedValue != "-1")
                {
                    this.cargarCuentas(this.ListEmpresa.SelectedValue);
                }
            }
            catch
            {

            }
        }

        protected void ListEmpresasAgregar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListEmpresasAgregar.SelectedValue != "-1")
            {                
                this.ListCtaBancariasAgregar.Items.Clear();
                string cuit = this.ListEmpresasAgregar.SelectedValue.ToString().Replace("-", "");
                List<CuentasBancaria> cuentas = this.contBanco.obtenerCuentasBancariasByCuit(cuit);

                foreach (var cta in cuentas)
                {
                    string text = cta.Banco1.entidad + " - " + cta.Numero;                 
                    this.ListCtaBancariasAgregar.Items.Add(new ListItem(text, cta.Id.ToString()));
                }
                
                this.ListCtaBancariasAgregar.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
        }

        protected void lbtnAgregarConcepto_Click(object sender, EventArgs e)
        {
            try
            {
                CuentasBancarias_Movimientos mov = new CuentasBancarias_Movimientos();
                mov.Fecha = Convert.ToDateTime(this.txtFechaAgregar.Text, new CultureInfo("es-AR"));
                mov.Monto = Convert.ToDecimal(this.txtImporteAgregar.Text);
                mov.IdCuenta = Convert.ToInt32(this.ListCtaBancariasAgregar.SelectedValue);
                mov.IdConcepto = Convert.ToInt32(this.ListConceptosAgregar.SelectedValue);
                mov.Observaciones = this.txtObservacionesAgregar.Text;

                if (this.ListTipoMovimiento.SelectedItem.Text == "INGRESO")
                    mov.Monto = mov.Monto * -1;

                int ok = this.contBanco.agregarMovimientoBancos(mov);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Agregado con exito!.", "BancosF.aspx"));
                    //Response.Redirect("BancosF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&cuit=" + this.ListEmpresa.SelectedValue + "&cta=" + this.ListCuentas.SelectedValue + "&c=" + this.ListConcepto.SelectedValue + "&tc=" + this.ListTipoConceptos.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar movimiento. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        protected void ListTipoConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListConcepto.Items.Clear();

                if (this.ListTipoConceptos.SelectedValue != "-1")
                {
                    //Si selecciona todos, en el DropDownList de Conceptos, dejo todos
                    if (this.ListTipoConceptos.SelectedValue == "0")
                    {
                        this.ListConcepto.Items.Insert(0, new ListItem("Todos", "0"));
                    }

                    //Si selecciona conceptos manuales, obtengo los conceptos desde Bancos_Conceptos
                    if (this.ListTipoConceptos.SelectedValue == "1")
                    {
                        this.cargarConceptosBuscar(1);
                    }

                    //Si selecciona conceptos automaticos, cargo los conceptos de cheques, transferencias y liquidaciones
                    if (this.ListTipoConceptos.SelectedValue == "2")
                    {
                        this.cargarConceptosBuscar(2);
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un cargando conceptos. Excepción: " + Ex.Message));
            }
        }
    }
}