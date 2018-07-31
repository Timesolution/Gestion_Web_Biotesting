using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
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
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net.Mail;

namespace Gestion_Web.Formularios.Pagos
{
    public partial class PagosRealizadosF : System.Web.UI.Page
    {
        controladorCobranza contCobranza = new controladorCobranza();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorCuentaCorriente contrCC = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorPagos contPagos = new controladorPagos();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int idProveedor;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private int formaPago;
        //private int idTipo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idProveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.formaPago = Convert.ToInt32(Request.QueryString["fp"]);//TODO r new querystring
                //this.idTipo = Convert.ToInt32(Request.QueryString["t"]);
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    if (idEmpresa == 0 && idSucursal == 0)
                    {
                        //this.idCliente = 1;
                        this.idSucursal = (int)Session["Login_SucUser"];
                        this.idEmpresa = (int)Session["Login_EmpUser"];
                        this.fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        this.idProveedor = -1;
                        this.formaPago = 0;//TODO r new
                        //this.puntoVenta = this.contCobranza.obtenerPrimerPuntoVenta(idSucursal, idEmpresa);
                        //this.puntoVenta = 1;
                    }
                    this.cargarProveedores();
                    this.ListProveedor.SelectedValue = this.idProveedor.ToString();
                    this.cargarEmpresas();
                    this.DropListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.DropListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.ListPuntoVenta.SelectedValue = this.puntoVenta.ToString();
                    this.DropListFormaPago.SelectedValue = this.formaPago.ToString(); //TODO r new
                    //this.DropListTipo.SelectedValue = this.idTipo.ToString();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                }
                if (this.idProveedor > -1)
                {
                    this.cargarPagos();
                }
                this.Form.DefaultButton = lbBuscar.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros Realizados") != 1)
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
                        if (s == "34")
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

        #region cargas iniciales
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


                this.DropListEmpresa.DataSource = dt;
                this.DropListEmpresa.DataValueField = "Id";
                this.DropListEmpresa.DataTextField = "Razon Social";

                this.DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";

                this.ListPuntoVenta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
            }
        }

        //public void cargarPuntoVta(int sucu)
        //{
        //    try
        //    {
        //        controladorSucursal contSucu = new controladorSucursal();
        //        DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

        //        //agrego todos
        //        DataRow dr = dt.NewRow();
        //        dr["NombreFantasia"] = "Seleccione...";
        //        dr["Id"] = -1;
        //        dt.Rows.InsertAt(dr, 0);


        //        this.DropListPuntoVta.DataSource = dt;
        //        this.DropListPuntoVta.DataValueField = "Id";
        //        this.DropListPuntoVta.DataTextField = "NombreFantasia";

        //        this.DropListPuntoVta.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
        //    }
        //}

        //protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        //}

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }

        //protected void DropListPuntoVta_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

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

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        #endregion

        #region busquedas

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PagosRealizadosF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + ListProveedor.SelectedValue + "&e=" + DropListEmpresa.SelectedValue + "&s=" + DropListSucursal.SelectedValue + "&pv=" + this.ListPuntoVenta.SelectedValue + "&fp=" + this.DropListFormaPago.SelectedValue);//TODO r new

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }
        }

        private void cargarPagos()
        {
            try
            {
                controladorPagos contPagos = new controladorPagos();
                
                if (idProveedor == 0 && idEmpresa == 0 && idSucursal == 0 )
                {
                    this.idProveedor = Convert.ToInt32(ListProveedor.SelectedValue);
                    this.idSucursal = (int)Session["Login_SucUser"];
                    this.idEmpresa = (int)Session["Login_EmpUser"];
                    this.puntoVenta = (int)Session["Login_PtoUser"];
                    //this.idTipo = Convert.ToInt32(DropListTipo.SelectedValue);
                    this.fechaD = this.txtFechaDesde.Text;
                    this.fechaH = this.txtFechaHasta.Text;

                    var pagos = contPagos.buscarPagos(Convert.ToDateTime(fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(fechaH, new CultureInfo("es-AR")), idProveedor, idEmpresa, idSucursal, puntoVenta);
                    phPagosRealizados.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Gestion_Api.Entitys.PagosCompra p in pagos)
                    {
                        saldo += Convert.ToDecimal(p.Total);
                        if (verificarFormaPago(p))
                        {
                            this.cargarEnPh(p);
                        }
                    }
                    this.labelSaldo.Text = "$ " + saldo.ToString("N", CultureInfo.DefaultThreadCurrentUICulture);
                    //this.lblSaldo.Text = "Saldo $ " + saldo.ToString("C");
                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, idProveedor, idEmpresa, idSucursal);

                }
                else
                {
                    var pagos = contPagos.buscarPagos(Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR")), Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR")), Convert.ToInt32(ListProveedor.SelectedValue), idEmpresa, idSucursal, puntoVenta);
                    phPagosRealizados.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Gestion_Api.Entitys.PagosCompra p in pagos)
                    {
                        saldo += Convert.ToDecimal(p.Total);
                        if (verificarFormaPago(p))
                        {
                            this.cargarEnPh(p);
                        }
                    }
                    this.labelSaldo.Text = "$ " + saldo.ToString("N", CultureInfo.DefaultThreadCurrentUICulture);
                    //this.lblSaldo.Text = "Saldo $ " + saldo.ToString("C");
                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(ListProveedor.SelectedValue),  idEmpresa, idSucursal);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando pagos. " + ex.Message));
            }
        }

        private bool verificarFormaPago(Gestion_Api.Entitys.PagosCompra p)
        {
            try
            {
                if (this.formaPago > 0)
                {
                    var pago = this.contPagos.obtenerPagoById(p.Id);
                    foreach (var item in pago.Pagos_Compras)
                    {
                        if (item.TipoPago == formaPago)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
            }
        }

        private void cargarEnPh(Gestion_Api.Entitys.PagosCompra p)
        {
           
            try
            {
                string tipoPago = string.Empty;
                if (p.Ftp == null)
                    tipoPago = "Pago - ";
                if (p.Ftp == 1)
                    tipoPago = "Pago PRP - ";
                if (p.Ftp == 0)
                    tipoPago = "Pago FC - ";


                //fila
                TableRow tr = new TableRow();
                tr.ID = p.Id.ToString();
                Cliente c = this.contrCliente.obtenerProveedorID((int)p.Proveedor);
                
                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = tipoPago + p.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celProveedor = new TableCell();
                celProveedor.Text = c.razonSocial;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                celProveedor.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celProveedor);


                //TableCell celDebe = new TableCell();
                //celDebe.Text = "$" + movV.debe.ToString().Replace(',', '.');
                //celDebe.VerticalAlign = VerticalAlign.Middle;
                //celDebe.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = Decimal.Round(Convert.ToDecimal(p.Total), 2).ToString("C");
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                //TableCell celSaldo = new TableCell();
                //celSaldo.Text = "$" + movV.saldo.ToString().Replace(',', '.');
                //celSaldo.VerticalAlign = VerticalAlign.Middle;
                //celSaldo.HorizontalAlign = HorizontalAlign.Right;
                //celSaldo.Width = Unit.Percentage(20);
                //tr.Cells.Add(celSaldo);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + p.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnDetalles.PostBackUrl = "ReportesR.aspx?id=" + p.Id;
                btnDetalles.Click += new EventHandler(this.detallePago);
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
               // btnDetalles.Click += new EventHandler(this.detalleCobro);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + p.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + p.Id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAccion.Controls.Add(btnEliminar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + p.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);

                this.phPagosRealizados.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando movimiento a PH. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idProveedor, int idEmpresa, int idSucursal)
        {
            try
            {
                string label = fechaD + "," + fechaH + ",";
                if (idProveedor > 0)
                {
                    label += ListProveedor.Items.FindByValue(idProveedor.ToString()).Text + ",";
                }
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idEmpresa > 0)
                {
                    label += DropListEmpresa.Items.FindByValue(idEmpresa.ToString()).Text + ",";
                }
                //if (idPuntoVenta > 0)
                //{
                //    label += DropListPuntoVta.Items.FindByValue(idPuntoVenta.ToString()).Text + ",";
                //}
                //if (idTipo > -1)
                //{
                //    label += DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                //}

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void detallePago(object sender, EventArgs e)
        {
            try
            {
                ////obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idPago = atributos[1];
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReportesR.aspx?id=" + idPago + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message);
            }
        }

        #endregion

        #region Eventos Controles
        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorPagos contPago = new controladorPagos();

                string id = this.txtMovimiento.Text;

                int i = contPago.quitarPagosCompras(Convert.ToInt64(id));
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Pago eliminado con exito!.", "PagosRealizadosF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo quitar pago. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al intentar eliminar pago. " + ex.Message));
            }
        }

        protected void lbtnReportePagosRealizados_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionPago.aspx?valor=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&prov=" + this.idProveedor + "&suc=" + this.idSucursal + "&ptoVta=" + this.puntoVenta + "&emp=" + this.idEmpresa + "&sd=" + labelSaldo.Text + "&ex=1");
            }
            catch
            {

            }
        }

        protected void lbtnReportePagosRealizadosPDF_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('ImpresionPago.aspx?valor=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&prov=" + this.idProveedor + "&suc=" + this.idSucursal + "&ptoVta=" + this.puntoVenta + "&emp=" + this.idEmpresa + "&sd=" + labelSaldo.Text + "','_blank');", true);
            }
            catch
            {

            }

        }
        protected void lbtnReporteDetallePagos_Click(object sender, EventArgs e)
        {
            try
            {
                string listaPagos = string.Empty;
                foreach (Control C in phPagosRealizados.Controls)
                {
                    TableRow tr = C as TableRow;
                    LinkButton lbtn = tr.Cells[4].Controls[0] as LinkButton;
                    listaPagos += lbtn.ID.Split('_')[1] + ";";
                }
                if (!String.IsNullOrEmpty(listaPagos))
                {
                    Response.Redirect("ReportesR.aspx?a=1&ex=1&lp=" + listaPagos);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
                }
            }
            catch (Exception Ex)
            {

            }
        }

        protected void lbtnReporteDetallePagosPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string listaPagos = string.Empty;
                foreach (Control C in phPagosRealizados.Controls)
                {
                    TableRow tr = C as TableRow;
                    LinkButton lbtn = tr.Cells[4].Controls[0] as LinkButton;
                    listaPagos += lbtn.ID.Split('_')[1] + ";";
                }
                if (!String.IsNullOrEmpty(listaPagos))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('ReportesR.aspx?a=1&lp=" + listaPagos + "','_blank');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
                }
            }
            catch (Exception Ex)
            {

            }
        }

        protected void lbtnEnvioReciboPagoMail_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity contCliEnt = new ControladorClienteEntity();

                string idtildado = "";
                foreach (Control C in phPagosRealizados.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[4] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    this.txtIdEnvioReciboPago.Text = idtildado;

                    var p = this.contPagos.obtenerPagoById(Convert.ToInt64(this.txtIdEnvioReciboPago.Text));
                    var datos = contrCliente.obtenerContactos(p.cliente.id);

                    if (datos.Count > 0)
                    {
                        foreach (var d in datos)
                        {
                            this.txtEnvioMail.Text += d.mail + ";";
                        }
                    }
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalMail", "openModalMail();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe seleccionar algún documento!" + "\", {type: \"error\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error buscando recibo de pago para envio via mail. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnEnviarMail_Click(object sender, EventArgs e)
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();
                var p = this.contPagos.obtenerPagoById(Convert.ToInt64(this.txtIdEnvioReciboPago.Text));
                string destinatarios = this.txtEnvioMail.Text + ";" + this.txtEnvioMail2.Text;
                String pathArchivoGenerar = Server.MapPath("../../ReciboPagos/" + p.Id + "/" + "/p-" + p.Numero + "_" + p.Id + ".pdf");
                string pathDirectorio = Server.MapPath("../../ReciboPagos/" + p.Id + "/");

                //Si el directorio no existe, lo creo
                if (!Directory.Exists(pathDirectorio))
                {
                    Directory.CreateDirectory(pathDirectorio);
                }

                int i = this.generarReciboPagoPDF(pathArchivoGenerar, p);
                if (i > 0)
                {
                    Attachment adjunto = new Attachment(pathArchivoGenerar);

                    int ok = contFunciones.enviarMailReciboPago(adjunto, p, destinatarios);
                    if (ok > 0)
                    {
                        adjunto.Dispose();
                        File.Delete(pathArchivoGenerar);
                        Directory.Delete(pathDirectorio);
                        this.txtEnvioMail.Text = "";
                        this.txtEnvioMail2.Text = "";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Recibo de Pago enviado correctamente!", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo enviar el Recibo de Pago por mail. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo generar impresion Recibo de Pago a enviar. "));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error enviando Recibo de Pago por mail. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
        private int generarReciboPagoPDF(string pathGenerar, PagosCompra p)
        {
            try
            {
                //obtengo pago
                //var p = this.contPagos.obtenerPagoById(Convert.ToInt64(idPago));
                //obtengo datos
                var dt = this.contPagos.obtenerDatosCompra(p);
                //obtengo compras
                var dtDocumentos = this.contPagos.obtenerDocCancelados(p.Id);
                //Cheques propio
                var dtCheques = this.contPagos.obtenerChequesPropios(p.Id);
                //Cheques terceros
                var dtChequesTer = this.contPagos.obtenerChequesTerceros(p.Id);
                //Transferencias
                var dtTrans = this.contPagos.obtenerTransferencias(p.Id);
                //detalle
                var dtDetalle = this.contPagos.obtenerDetalle(p.Id);

                string observacion = "";
                if (p.PagosCompras_Observaciones != null)
                    observacion = p.PagosCompras_Observaciones.Observaciones;

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReciboPagos.rdlc");
                ReportDataSource rds = new ReportDataSource("DSEncabezado", dt);
                ReportDataSource rds2 = new ReportDataSource("DSDocumentos", dtDocumentos);
                ReportDataSource rds3 = new ReportDataSource("DSChequesPropios", dtCheques);
                ReportDataSource rds4 = new ReportDataSource("DSChequesTerceros", dtChequesTer);
                ReportDataSource rds5 = new ReportDataSource("DSTransferencias", dtTrans);
                ReportDataSource rds6 = new ReportDataSource("DSDetalle", dtDetalle);

                ReportParameter param = new ReportParameter("ParamObservacion", observacion);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.DataSources.Add(rds6);

                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get pdf content

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                return 1;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al intentar guardar el recibo de pago. Excepción: " + Ex.Message));
                return -1;
            }
        }
        #endregion

    }
}