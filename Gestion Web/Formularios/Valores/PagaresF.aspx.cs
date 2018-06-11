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
    public partial class PagaresF : System.Web.UI.Page
    {
        controladorFactEntity controlador = new controladorFactEntity();
        controladorFacturacion contFacturas = new controladorFacturacion();
        controladorCobranza contCobranza = new controladorCobranza();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private int idMutual;
        private string fechaD;
        private string fechaH;        
        private int tipoFecha;
        private long estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.suc = Convert.ToInt32(Request.QueryString["suc"]);
                this.estado = Convert.ToInt64(Request.QueryString["e"]);
                this.idMutual = Convert.ToInt32(Request.QueryString["m"]);
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);

                //Griseo boton cuando confirma liquidación.
                btnSi.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnSi, null) + ";");

                if (!IsPostBack)
                {
                    this.cargarSucursal();
                    this.cargarMutuales();
                    this.cargarEstadosPagare();

                    if (fechaD == null && fechaH == null)
                    {
                        suc = (int)Session["Login_SucUser"];
                        
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        //tipoFecha = 1;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtDesdeVto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaVto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListEstado.SelectedValue = estado.ToString();
                        this.btnLiquidar.Visible = false;
                    }
                    
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtDesdeVto.Text = fechaD;
                    txtHastaVto.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                    DropListEstado.SelectedValue = estado.ToString();
                }
                this.cargarPagares();
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
                        if (s == "91")
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();
                this.DropListSucursal.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.DropListSucursal.Items.Insert(1, new ListItem("Todas", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarMutuales()
        {
            try
            {
                List<Gestion_Api.Entitys.Mutuale> mutuales = controlador.obtenerMutuales();
                this.ListMutuales.DataSource = mutuales;
                this.ListMutuales.DataValueField = "Id";
                this.ListMutuales.DataTextField = "Nombre";

                this.ListMutuales.DataBind();

                //this.ListMutuales.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListMutuales.Items.Insert(0, new ListItem("Todas", "0"));
            }
            catch
            {

            }
        }
        public void cargarEstadosPagare()
        {
            try
            {
                List<Pagares_Estados> list = controlador.obtenerPagares_Estados();
                if (list != null)
                {
                    this.DropListEstado.DataSource = list;
                    this.DropListEstado.DataValueField = "Id";
                    this.DropListEstado.DataTextField = "Descripcion";

                    this.DropListEstado.DataBind();

                    this.DropListEstado.Items.Insert(0, new ListItem("Todos", "0"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error cargando los Estados de Pagarés a la lista."));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error cargando los Estados de Pagarés a la lista. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RadioFechaVto.Checked)
                    Response.Redirect("PagaresF.aspx?fd=" + this.txtDesdeVto.Text + "&fh=" + this.txtHastaVto.Text + "&tf=1" + "&suc=" + this.DropListSucursal.SelectedValue + "&m=" + this.ListMutuales.SelectedValue + "&e=" + this.DropListEstado.SelectedValue);
                else
                    Response.Redirect("PagaresF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&tf=0" + "&suc=" + this.DropListSucursal.SelectedValue + "&m=" + this.ListMutuales.SelectedValue + "&e=" + this.DropListEstado.SelectedValue);
            }
            catch
            {

            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idTildado = string.Empty;
                decimal monto = 0;

                //Verifico que se haya ingresado algo en el campo liquidacion
                if (string.IsNullOrEmpty(this.txtNumeroLiquidacion.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", " $.msgbox(\"Debe ingresar algún valor en el campo Liquidación. \"); ", true);
                    return;
                }

                //Recorro los pagarés, guardo los id y acumulo los montos
                foreach (Control c in phPagares.Controls)
                {
                    TableRow tr = c as TableRow;
                    CheckBox ch = tr.Cells[11].Controls[2] as CheckBox;
                    TableCell tc = tr.Cells[7] as TableCell;

                    if (ch.Checked == true)
                    {
                        idTildado += ch.ID.Split('_')[1] + ";";
                        monto += Convert.ToDecimal(tc.Text.Split('$')[1]);
                    }
                    
                }
                if (!string.IsNullOrEmpty(idTildado))
                {

                    //Verifico que los pagarés seleccionados sólo estén en estado pendiente y que sean de la misma Mutual
                    bool validarEstadoPagares = this.controlador.validarEstadoPagares(idTildado.Substring(0,idTildado.Length - 1));
                    if (!validarEstadoPagares)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", " $.msgbox(\"Se seleccionaron pagarés liquidados o de distintas mutuales. \"); ", true);
                        return;
                    }

                    this.liquidarPagares(idTildado.Substring(0,idTildado.Length - 1),monto, this.txtNumeroLiquidacion.Text);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", " $.msgbox(\"No se seleccionó ningun pagaré. \");", true);
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error enviando a liquidar los pagarés seleccionados. Excepción: " + Ex.Message));
            }
        }
        protected void btnReporteExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionValores.aspx?a=10&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&Mutual=" + this.idMutual + "&tf=" + this.tipoFecha + "&estado=" + this.estado + "&valor=1");
            }
            catch (Exception Ex)
            {

            }
        }
        protected void btnReportePdf_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionValores.aspx?a=10&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&Mutual=" + this.idMutual + "&tf=" + this.tipoFecha + "&estado=" + this.estado + "&valor=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception Ex)
            {

            }
        }
        #endregion

        #region Funciones ABM
        private void cargarPagares()
        {
            try
            {
                this.phPagares.Controls.Clear();

                DateTime desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); 
                DateTime hasta = desde.AddHours(23).AddMinutes(59);

                if (this.fechaD != null && this.fechaH != null)
                {
                    desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                    hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                }
                
                //Mutuale mutual = this.controlador.obtenerMutualByID(this.idMutual);
                List<Mutuales_Pagares> pagares = this.controlador.obtenerPagares(desde, hasta, this.suc, this.idMutual, this.tipoFecha, this.estado);

                if (pagares != null)
                {
                    decimal saldo = 0;
                    decimal comision = 0;
                    foreach (Mutuales_Pagares p in pagares)
                    {
                        //var cuotas = pagares.Where(x => x.Pagare == p.Pagare);
                        //int nro = pagares.IndexOf(p) + 1;

                        //Calculo cuota
                        int cuota = p.Vencimiento.Value.Month - p.Fecha.Value.Month;
                        
                        this.cargarPagaresPH(p, cuota);
                        saldo += p.Importe.Value;
                        comision += decimal.Round(p.Importe.Value * (p.Mutuale.Comision.Value / 100), 2);
                    }
                    this.lblSaldo.Text = saldo.ToString("C");
                    this.lblComision.Text = comision.ToString("C");
                    //if (mutual != null)
                    //{
                    //    if (mutual.Comision != null)
                    //    {
                    //        this.lblComision.Text = decimal.Round(saldo * (mutual.Comision.Value / 100), 2).ToString("C");                            
                    //    }
                    //}
                }
            }
            catch
            {

            }
        }
        private void cargarPagaresPH(Mutuales_Pagares p, int cuota)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = p.Id.ToString();

                string estado = string.Empty;
                var estadoPagare = this.controlador.obtenerPagares_EstadosById((long)p.Estado);
                if (estadoPagare != null)
                    estado = estadoPagare.Descripcion;

                TableCell celFecha = new TableCell();
                celFecha.Text = p.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Center;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.Width = Unit.Percentage(10);
                tr.Controls.Add(celFecha);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = p.sucursale.nombre;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celSucursal);

                TableCell celDoc = new TableCell();
                string documento = "";
                if (p.Factura != null)
                {
                    Factura fc = this.contFacturas.obtenerFacturaId(p.Factura.Value);
                    documento = fc.tipo.tipo + " " + fc.numero;
                }
                celDoc.Text = documento;
                celDoc.HorizontalAlign = HorizontalAlign.Left;
                celDoc.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celDoc);

                TableCell celMutual = new TableCell();
                celMutual.Text = p.Mutuale.Nombre;
                celMutual.HorizontalAlign = HorizontalAlign.Left;
                celMutual.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celMutual);

                TableCell celSocio = new TableCell();
                celSocio.Text = p.NroSocio;
                celSocio.HorizontalAlign = HorizontalAlign.Left;
                celSocio.VerticalAlign = VerticalAlign.Middle;
                celSocio.Width = Unit.Percentage(10);
                tr.Controls.Add(celSocio);

                TableCell celAuth = new TableCell();
                celAuth.Text = p.NroAutorizacion;
                celAuth.HorizontalAlign = HorizontalAlign.Left;
                celAuth.VerticalAlign = VerticalAlign.Middle;
                celAuth.Width = Unit.Percentage(10);
                tr.Controls.Add(celAuth);

                TableCell celNumero = new TableCell();
                celNumero.Text = p.Numero;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.Width = Unit.Percentage(5);
                tr.Controls.Add(celNumero);

                TableCell celImporte = new TableCell();
                celImporte.Text = p.Importe.Value.ToString("C");
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.Width = Unit.Percentage(10);
                tr.Controls.Add(celImporte);

                TableCell celVencimiento = new TableCell();
                celVencimiento.Text = p.Vencimiento.Value.ToString("dd/MM/yyyy");
                celVencimiento.HorizontalAlign = HorizontalAlign.Center;
                celVencimiento.VerticalAlign = VerticalAlign.Middle;
                celVencimiento.Width = Unit.Percentage(10);
                tr.Controls.Add(celVencimiento);

                TableCell celCuota = new TableCell();
                celCuota.Text = cuota.ToString();
                celCuota.HorizontalAlign = HorizontalAlign.Center;
                celCuota.VerticalAlign = VerticalAlign.Middle;
                celCuota.Width = Unit.Percentage(5);
                tr.Controls.Add(celCuota);

                TableCell celEstado = new TableCell();
                celEstado.Text = estado;
                celEstado.HorizontalAlign = HorizontalAlign.Center;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.Width = Unit.Percentage(5);
                tr.Controls.Add(celEstado);

                TableCell celAction = new TableCell();
                LinkButton btnImprimir = new LinkButton();
                btnImprimir.ID = "btnImprimir_" + p.Id.ToString();
                btnImprimir.CssClass = "btn btn-info ui-tooltip";
                btnImprimir.Attributes.Add("data-toggle", "tooltip");
                btnImprimir.Attributes.Add("title data-original-title", "Ver Liquidación");
                if (String.IsNullOrEmpty(estado) || estado == "Pendiente")
                    btnImprimir.Style.Add("visibility","hidden");
                btnImprimir.Text = "<span class='shortcut-icon icon-search'></span>";
                btnImprimir.Click += new EventHandler(this.liquidacionPagare);
                celAction.Controls.Add(btnImprimir);
                celAction.Width = Unit.Percentage(10);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + p.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAction.Controls.Add(cbSeleccion);

                tr.Cells.Add(celAction);

                this.phPagares.Controls.Add(tr);
            }
            catch
            {

            }
        }
        private void liquidarPagares(string listPagares, decimal monto, string numeroLiquidacion)
        {
            try
            {
                //Genero PRP
                int i = this.controlador.generarFacturaLiquidacionPagares(listPagares, monto, numeroLiquidacion);
                if (i > 0)
                {
                    //Limpio campo liquidación
                    this.txtNumeroLiquidacion.Text = string.Empty;

                    //Obtengo datos de la factura
                    var fc = this.contFacturas.obtenerFacturaId(i);

                    //Obtengo el id del Movimiento de Cuenta Corriente para enviar a Cobros
                    var mov = this.contCobranza.obtenerMovimientoIDDoc(i, 17);

                    //Lo envio a la pantalla de cobros para que genere el cobro
                    Response.Redirect("../Cobros/ABMCobros.aspx?valor=1" + "&documentos=" + mov.id.ToString() + "&cliente=" + fc.cliente.id + "&empresa=" + fc.empresa.id + "&sucursal=" + fc.sucursal.id + "&puntoVenta=" + fc.ptoV.id + "&pagares=" + listPagares + "&monto=0&tipo=2");
                }
                
                if (i == -1)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", "$.msgbox(\"Ocurrió un error agregando los datos a la Factura. \", {type: \"error\"});", true);
                if (i == -2)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", "$.msgbox(\"Ocurrió un error procesando la Factura. \", {type: \"error\"});", true);
                if (i == -3)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", " $.msgbox(\"Debe asignar un Cliente a la Mutual. \");", true);
                if (i == -4)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelLiquidar, UpdatePanelLiquidar.GetType(), "alert", "$.msgbox(\"Ocurrió un error en la Facturación de los Pagarés. \", {type: \"error\"});", true);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error liquidando pagarés. Excepción: " + Ex.Message));
            }
        }

        #endregion

        #region Funciones Auxiliares
        private void liquidacionPagare(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idPagare = atributos[1];

                //Verifico el estado del pagaré, si está en estado Pendiente le aviso, si está en Liquidado abro el Cobro y la Factura, y si está con Liquidacion Pendiente le abro sólo la Factura
                var pagare = this.controlador.obtenerPagareById(Convert.ToInt32(idPagare));
                if (pagare != null)
                {
                    //Estado Liquidado / Estado Liquidacion Pendiente
                    if (pagare.Estado == 2 || pagare.Estado == 3)
                    {
                        string script;

                        var pagLiq = this.controlador.obtenerPagares_LiquidacionesByPagare(pagare.Id);
                        if (pagLiq != null)
                        {
                            //Abro la Factura
                            script = "window.open('../Facturas/ImpresionPresupuesto.aspx?Presupuesto=" + pagLiq.Factura + "','_blank');";

                            //Abro el Cobro    
                            if (pagare.Estado == 2)
                            {
                                script += "window.open('../Cobros/ImpresionCobro.aspx?valor=2&Cobro=" + pagLiq.Cobro + "','_blank');";
                            }

                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrió un error obteniendo datos de liquidación del Pagaré. \", {type: \"error\"});", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrió un error obteniendo información del Pagaré. \", {type: \"error\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrió un error al mostrar el detalle de liquidación del pagaré. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        #endregion
    }
}