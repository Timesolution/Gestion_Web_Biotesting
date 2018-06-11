using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class BalanceF : System.Web.UI.Page
    {

        controladorInformes cont = new controladorInformes();
        controladorFunciones f = new controladorFunciones();
        int accion;
        int sucursal;
        string desde;
        string hasta;        
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.desde = Request.QueryString["d"];
                this.hasta = Request.QueryString["h"];
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);

                this.VerificarLogin();                

                if (!IsPostBack)
                {

                    //sucursal = (int)Session["Login_SucUser"];
                    this.cargarSucursal();
                    //this.cargarTiposPago();
                    this.txtFechaDesde.Text = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.DropListSucursal.SelectedValue = sucursal.ToString();
                }
                //cargo el informe
                if (this.accion == 2)
                {
                    this.generarInforme();
                }
            }
            catch (Exception ex)
            {
 
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
                        if (s == "67")
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

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        private void generarInforme()
        {
            try
            {
                int sucursal = Convert.ToInt32(this.sucursal);
                this.cargarBalance(this.desde, this.hasta, sucursal);
                this.cargarCompraVenta(this.desde, this.hasta, sucursal);
                this.DropListSucursal.SelectedValue = sucursal.ToString();
                this.lblParametros.Text = this.DropListSucursal.SelectedItem.Text + ", " + this.desde + ", " + this.hasta; 
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarBalance(string Desde, string Hasta, int sucursal)
        {
            try
            {
                //formateo fechas
                Desde = f.formatearFechaSQL(Desde);
                Hasta = f.formatearFechaSQL(Hasta);

               Activo act = this.cont.obtenerActivo(sucursal, Desde, Hasta);
               //this.LabelActivo.Text = act.Total().ToString("C");
               this.LabelActivo.Text = (act.Efectivo + act.ChequesCartera + act.SaldoCC + act.StockValorizado).ToString("C");

               this.cargarActivoTabla(act);

               Pasivo p = this.cont.obtenerPasivo(sucursal, Desde, Hasta);
               this.LabelPasivo.Text = p.Total().ToString("C");
               this.cargarPasivoTabla(p);

               //this.cargarInfoCirque(act.Efectivo.ToString("N"), act.ChequesCartera.ToString("N"), act.TotalCaja.ToString("N"),
               //    act.SaldoCC.ToString("N"), act.StockValorizado.ToString("N"),
               //    act.Total().ToString("N"));
               //this.cargarInfoCirque("50", "50", "100");
            }
            catch (Exception ex)
            {
                
            }
        }

        private void cargarActivoTabla(Activo act)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = "EFECTIVO";
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(65);
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = "$ " + act.Efectivo.ToString("N");
                celCantidad.Width = Unit.Percentage(30);
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celPuntoVta = new TableCell();

                LinkButton btnEditar = new LinkButton();
                
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar.OnClientClick = "window.open('../../Formularios/Valores/CajaF.aspx?FD=" + DateTime.Today.AddYears(-2).ToString("dd/MM/yyyy") + "&FH=" + DateTime.Today.AddYears(2).ToString("dd/MM/yyyy") + "&S=" + this.sucursal + "&TP=1&TM=0', '_blank')";                
                //btnEditar.PostBackUrl = "../../Formularios/Valores/CajaF.aspx?FD=" + this.desde + "&FH=" + this.hasta + "&S=" + this.sucursal + "&TP=1&TM=0";
                celPuntoVta.Controls.Add(btnEditar);
                celPuntoVta.Width = Unit.Percentage(5);
                tr.Cells.Add(celPuntoVta);

                phTopClientes.Controls.Add(tr);


                //cheques en cartera
                TableRow tr2 = new TableRow();

                TableCell celCodigo2 = new TableCell();
                celCodigo2.Text = "Cheques En Cartera";
                celCodigo2.VerticalAlign = VerticalAlign.Bottom;
                celCodigo2.HorizontalAlign = HorizontalAlign.Left;
                celCodigo2.Width = Unit.Percentage(65);
                tr2.Cells.Add(celCodigo2);

                TableCell celCantidad2 = new TableCell();
                celCantidad2.Text = "$ " + act.ChequesCartera.ToString("N");
                celCantidad2.Width = Unit.Percentage(30);
                celCantidad2.VerticalAlign = VerticalAlign.Bottom;
                celCantidad2.HorizontalAlign = HorizontalAlign.Right;
                tr2.Cells.Add(celCantidad2);

                TableCell celPuntoVta2 = new TableCell();

                LinkButton btnEditar2 = new LinkButton();

                btnEditar2.CssClass = "btn btn-info ui-tooltip";
                btnEditar2.Attributes.Add("data-toggle", "tooltip");
                btnEditar2.Attributes.Add("title data-original-title", "Editar");
                btnEditar2.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar2.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar2.OnClientClick = "window.open('../../Formularios/Valores/ChequesF.aspx?fechadesde=" + DateTime.Today.AddYears(-2).ToString("dd/MM/yyyy") + "&fechaHasta=" + DateTime.Today.AddYears(2).ToString("dd/MM/yyyy") + "&Sucursal=" + this.sucursal + "&Cliente=0&fdC=" + this.desde + "&fhC=" + DateTime.Today.AddYears(1).ToString("dd/MM/yyyy") + "&tf=1&o=2&e=1', '_blank')";
                //btnEditar2.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx?fechadesde="+this.desde+"&fechaHasta="+this.hasta+"&Sucursal="+this.sucursal+"&Cliente=0&fdC="+this.desde+"&fhC="+DateTime.Today.AddYears(1).ToString("dd/MM/yyyy")+"&tf=1&o=2&e=1";
                celPuntoVta2.Controls.Add(btnEditar2);
                celPuntoVta2.Width = Unit.Percentage(5);
                tr2.Cells.Add(celPuntoVta2);

                phTopClientes.Controls.Add(tr2);

                //Total Caja
                TableRow tr3 = new TableRow();

                TableCell celCodigo3 = new TableCell();
                celCodigo3.Text = "Total Caja";
                celCodigo3.VerticalAlign = VerticalAlign.Bottom;
                celCodigo3.HorizontalAlign = HorizontalAlign.Left;
                celCodigo3.Width = Unit.Percentage(65);
                //tr3.Cells.Add(celCodigo3);

                TableCell celCantidad3 = new TableCell();
                celCantidad3.Text = "$ " + act.TotalCaja.ToString("N");
                celCantidad3.Width = Unit.Percentage(30);
                celCantidad3.VerticalAlign = VerticalAlign.Bottom;
                celCantidad3.HorizontalAlign = HorizontalAlign.Right;
                //tr3.Cells.Add(celCantidad3);

                TableCell celPuntoVta3 = new TableCell();

                LinkButton btnEditar3 = new LinkButton();

                btnEditar3.CssClass = "btn btn-info ui-tooltip";
                btnEditar3.Attributes.Add("data-toggle", "tooltip");
                btnEditar3.Attributes.Add("title data-original-title", "Editar");
                btnEditar3.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar3.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar3.OnClientClick = "window.open('../../Formularios/Valores/CajaF.aspx?FD=" + DateTime.Today.AddYears(-2).ToString("dd/MM/yyyy") + "&FH=" + DateTime.Today.AddYears(2).ToString("dd/MM/yyyy") + "&S=" + this.sucursal + "&TP=0&TM=0', '_blank')";                
                //btnEditar3.PostBackUrl = "../../Formularios/Valores/CajaF.aspx?FD=" + this.desde + "&FH=" + this.hasta + "&S=" + this.sucursal + "&TP=0&TM=0";
                celPuntoVta3.Controls.Add(btnEditar3);
                celPuntoVta3.Width = Unit.Percentage(5);
                //tr3.Cells.Add(celPuntoVta3);

                //phTopClientes.Controls.Add(tr3);

                //Total CC 
                TableRow tr4 = new TableRow();

                TableCell celCodigo4= new TableCell();
                celCodigo4.Text = "Saldo CTA CTE";
                celCodigo4.VerticalAlign = VerticalAlign.Bottom;
                celCodigo4.HorizontalAlign = HorizontalAlign.Left;
                celCodigo4.Width = Unit.Percentage(65);
                tr4.Cells.Add(celCodigo4);

                TableCell celCantidad4 = new TableCell();
                celCantidad4.Text = "$ " + act.SaldoCC.ToString("N");
                celCantidad4.Width = Unit.Percentage(30);
                celCantidad4.VerticalAlign = VerticalAlign.Bottom;
                celCantidad4.HorizontalAlign = HorizontalAlign.Right;
                tr4.Cells.Add(celCantidad4);

                TableCell celPuntoVta4 = new TableCell();

                LinkButton btnEditar4 = new LinkButton();

                btnEditar4.CssClass = "btn btn-info ui-tooltip";
                btnEditar4.Attributes.Add("data-toggle", "tooltip");
                btnEditar4.Attributes.Add("title data-original-title", "Editar");
                btnEditar4.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar4.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar4.OnClientClick = "window.open('../../Formularios/Facturas/CuentaCorrienteF.aspx', '_blank')";
                //btnEditar4.PostBackUrl = "../../Formularios/Facturas/CuentaCorrienteF.aspx";
                celPuntoVta4.Controls.Add(btnEditar4);
                celPuntoVta4.Width = Unit.Percentage(5);
                tr4.Cells.Add(celPuntoVta4);

                phTopClientes.Controls.Add(tr4);

                //Stock Valorizado
                TableRow tr5 = new TableRow();

                TableCell celCodigo5 = new TableCell();
                celCodigo5.Text = "Stock Valorizado";
                celCodigo5.VerticalAlign = VerticalAlign.Bottom;
                celCodigo5.HorizontalAlign = HorizontalAlign.Left;
                celCodigo5.Width = Unit.Percentage(65);
                tr5.Cells.Add(celCodigo5);

                TableCell celCantidad5 = new TableCell();
                celCantidad5.Text = "$ " + act.StockValorizado.ToString("N");
                celCantidad5.Width = Unit.Percentage(30);
                celCantidad5.VerticalAlign = VerticalAlign.Bottom;
                celCantidad5.HorizontalAlign = HorizontalAlign.Right;
                tr5.Cells.Add(celCantidad5);

                TableCell celPuntoVta5 = new TableCell();

                LinkButton btnEditar5 = new LinkButton();

                btnEditar5.CssClass = "btn btn-info ui-tooltip";
                btnEditar5.Attributes.Add("data-toggle", "tooltip");
                btnEditar5.Attributes.Add("title data-original-title", "Editar");
                btnEditar5.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar5.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar5.OnClientClick = "window.open('../../Formularios/Articulos/Articulos.aspx', '_blank')";
                //btnEditar5.PostBackUrl = "../../Formularios/Articulos/Articulos.aspx";
                celPuntoVta5.Controls.Add(btnEditar5);
                celPuntoVta5.Width = Unit.Percentage(5);
                tr5.Cells.Add(celPuntoVta5);

                phTopClientes.Controls.Add(tr5);

                //TOTAL
                TableRow tr6 = new TableRow();

                TableCell celCodigo6 = new TableCell();
                celCodigo6.Text = "Total";
                celCodigo6.VerticalAlign = VerticalAlign.Bottom;
                celCodigo6.HorizontalAlign = HorizontalAlign.Left;
                celCodigo6.Width = Unit.Percentage(65);
                tr6.Cells.Add(celCodigo6);

                TableCell celCantidad6 = new TableCell();
                celCantidad6.Text = "$ " + (act.Efectivo + act.ChequesCartera + act.SaldoCC + act.StockValorizado).ToString("N");
                celCantidad6.Width = Unit.Percentage(30);
                celCantidad6.VerticalAlign = VerticalAlign.Bottom;
                celCantidad6.HorizontalAlign = HorizontalAlign.Right;
                tr6.Cells.Add(celCantidad6);

                TableCell celPuntoVta6 = new TableCell();

                LinkButton btnEditar6 = new LinkButton();

                //btnEditar6.CssClass = "btn btn-info ui-tooltip";
                //btnEditar6.Attributes.Add("data-toggle", "tooltip");
                //btnEditar6.Attributes.Add("title data-original-title", "Editar");
                //btnEditar6.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEditar6.Font.Size = 9;
                ////btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                //btnEditar6.OnClientClick = "window.open('../../Formularios/Facturas/Articulos.aspx', '_blank')";
                ////btnEditar6.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                //celPuntoVta6.Controls.Add(btnEditar6);
                celPuntoVta6.Width = Unit.Percentage(5);
                tr6.Cells.Add(celPuntoVta6);

                phTopClientes.Controls.Add(tr6);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Activo. " + ex.Message));

            }
        }

        private void cargarPasivoTabla(Pasivo p)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = "Cheques Dados";
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(65);
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = "$ " + p.ChequesDados.ToString("N");
                celCantidad.Width = Unit.Percentage(30);
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celPuntoVta = new TableCell();

                LinkButton btnEditar = new LinkButton();

                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar.OnClientClick = "window.open('../../Formularios/Valores/ChequesF.aspx?fechadesde=" + DateTime.Today.AddYears(-2).ToString("dd/MM/yyyy") + "&fechaHasta=" + DateTime.Today.AddYears(2).ToString("dd/MM/yyyy") + "&Sucursal=" + this.sucursal + "&Cliente=0&fdC=" + DateTime.Today.AddYears(-1).ToString("dd/MM/yyyy") + "&fhC=" + DateTime.Today.AddYears(1).ToString("dd/MM/yyyy") + "&tf=1&o=1&e=1', '_blank')";
                //btnEditar.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx?fechadesde=" + this.desde + "&fechaHasta=" + this.hasta + "&Sucursal=" + this.sucursal + "&Cliente=0&fdC=" + DateTime.Today.AddYears(-1).ToString("dd/MM/yyyy") + "&fhC=" + DateTime.Today.AddYears(1).ToString("dd/MM/yyyy") + "&tf=1&o=1&e=1";
                celPuntoVta.Controls.Add(btnEditar);
                celPuntoVta.Width = Unit.Percentage(5);
                tr.Cells.Add(celPuntoVta);

                phPasivo.Controls.Add(tr);


                //cheques en cartera
                TableRow tr2 = new TableRow();

                TableCell celCodigo2 = new TableCell();
                celCodigo2.Text = "Saldos CC Proveedores";
                celCodigo2.VerticalAlign = VerticalAlign.Bottom;
                celCodigo2.HorizontalAlign = HorizontalAlign.Left;
                celCodigo2.Width = Unit.Percentage(65);
                tr2.Cells.Add(celCodigo2);

                TableCell celCantidad2 = new TableCell();
                celCantidad2.Text = "$ " + p.SaldoCCProveedores.ToString("N");
                celCantidad2.Width = Unit.Percentage(30);
                celCantidad2.VerticalAlign = VerticalAlign.Bottom;
                celCantidad2.HorizontalAlign = HorizontalAlign.Right;
                tr2.Cells.Add(celCantidad2);

                TableCell celPuntoVta2 = new TableCell();

                LinkButton btnEditar2 = new LinkButton();

                btnEditar2.CssClass = "btn btn-info ui-tooltip";
                btnEditar2.Attributes.Add("data-toggle", "tooltip");
                btnEditar2.Attributes.Add("title data-original-title", "Editar");
                btnEditar2.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar2.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar2.OnClientClick = "window.open('../../Formularios/Compras/CCCompraF.aspx', '_blank')";
                //btnEditar2.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                celPuntoVta2.Controls.Add(btnEditar2);
                celPuntoVta2.Width = Unit.Percentage(5);
                tr2.Cells.Add(celPuntoVta2);

                phPasivo.Controls.Add(tr2);

                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Pasivo. " + ex.Message));

            }
        }
        private void cargarInfoCirque(string value1, string value2, string value3, string value4, string value5, string valueTotal)
        {
            //string html = "CONSUMADO<div class=\"ui-cirque\" data-value=\""+ value1 +"\" data-total=\""+ valueTotal +"\" data-arc-color=\"#FF9900\"   data-label=\"ratio\"></div>";
            string html = "Efectivo<div class=\"ui-cirque\" data-value=\"" + value1 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#FF9900\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Cheques Cartera<div class=\"ui-cirque\" data-value=\"" + value2 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Total Caja<div class=\"ui-cirque\" data-value=\"" + value3 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Saldo Cta Cte<div class=\"ui-cirque\" data-value=\"" + value4 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Stock Valorizado<div class=\"ui-cirque\" data-value=\"" + value5 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";

            //html += "TENTATIVA<div class=\"ui-cirque\" data-value=\"" + value2 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#0101DF\" data-radius=\"90\"  data-label=\"ratio\"></div>";



            //this.LitChar1.Text = html;

        }

        private void cargarCompraVenta(string Desde, string Hasta, int sucursal)
        {
            try
            {
                ////formateo fechas
                Desde =  Convert.ToDateTime(Desde, new CultureInfo("es-AR")).ToString();
                Hasta = Convert.ToDateTime(Hasta, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59).ToString();

                var cv = this.cont.obtenerVentasCompras(sucursal, Desde, Hasta);

                this.cargarVentasTabla(cv);
                this.cargarComprasTabla(cv);

                this.LitIvaVentas.Text = "$ " + cv.IvaVentas.ToString("N");
                this.LitIvaCompras.Text = "$ " + cv.IvaCompras.ToString("N");
                this.LitIvaPagar.Text = "$ " + (cv.IvaVentas - cv.IvaCompras).ToString("N");
                this.lblIvaVentas.Text = this.LitIvaVentas.Text;
                this.lblIvaCompras.Text = this.LitIvaCompras.Text;
                this.lblIvaTotalPagar.Text = this.LitIvaPagar.Text;
                //this.LitTotalIva.Text = (cv.IvaVentas + cv.IvaCompras).ToString("C");
                //this.LitIvaCompra.Text = cv.IvaCompras.ToString("C");
                //this.LitIvaVenta.Text = cv.IvaVentas.ToString("C");
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarVentasTabla(VentasCompras vc)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = "Ventas";
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(65);
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = "$ " + vc.Ventas.ToString("N") ;
                celCantidad.Width = Unit.Percentage(30);
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celPuntoVta = new TableCell();

                LinkButton btnEditar = new LinkButton();

                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);                
                //btnEditar.PostBackUrl = "../../Formularios/Facturas/FacturasF.aspx";
                btnEditar.OnClientClick = "window.open('../../Formularios/Facturas/FacturasF.aspx?fechadesde=" + this.desde + "&fechaHasta=" + this.hasta + "&Sucursal=" + this.sucursal + "', '_blank')";
                celPuntoVta.Controls.Add(btnEditar);
                celPuntoVta.Width = Unit.Percentage(5);
                tr.Cells.Add(celPuntoVta);

                phVentas.Controls.Add(tr);


                //cheques en cartera
                TableRow tr2 = new TableRow();

                TableCell celCodigo2 = new TableCell();
                celCodigo2.Text = "Iva Ventas";
                celCodigo2.VerticalAlign = VerticalAlign.Bottom;
                celCodigo2.HorizontalAlign = HorizontalAlign.Left;
                celCodigo2.Width = Unit.Percentage(65);
                tr2.Cells.Add(celCodigo2);

                TableCell celCantidad2 = new TableCell();
                celCantidad2.Text = "$ " + vc.IvaVentas.ToString("N");
                celCantidad2.Width = Unit.Percentage(30);
                celCantidad2.VerticalAlign = VerticalAlign.Bottom;
                celCantidad2.HorizontalAlign = HorizontalAlign.Right;
                tr2.Cells.Add(celCantidad2);

                TableCell celPuntoVta2 = new TableCell();

                //LinkButton btnEditar2 = new LinkButton();

                //btnEditar2.CssClass = "btn btn-info ui-tooltip";
                //btnEditar2.Attributes.Add("data-toggle", "tooltip");
                //btnEditar2.Attributes.Add("title data-original-title", "Editar");
                //btnEditar2.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEditar2.Font.Size = 9;
                ////btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                //btnEditar2.OnClientClick = "window.open('../../Formularios/Facturas/FacturasF.aspx?fechadesde=" + this.desde + "&fechaHasta=" + this.hasta + "&Sucursal=" + this.sucursal + "', '_blank')";
                ////btnEditar2.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                //celPuntoVta2.Controls.Add(btnEditar2);
                celPuntoVta2.Width = Unit.Percentage(5);
                tr2.Cells.Add(celPuntoVta2);

                phVentas.Controls.Add(tr2);

                //Total Caja
                TableRow tr3 = new TableRow();

                TableCell celCodigo3 = new TableCell();
                celCodigo3.Text = "Percepcion II. BB.";
                celCodigo3.VerticalAlign = VerticalAlign.Bottom;
                celCodigo3.HorizontalAlign = HorizontalAlign.Left;
                celCodigo3.Width = Unit.Percentage(65);
                tr3.Cells.Add(celCodigo3);

                TableCell celCantidad3 = new TableCell();
                celCantidad3.Text = "$ " + vc.PIngresosBrutos.ToString("N");
                celCantidad3.Width = Unit.Percentage(30);
                celCantidad3.VerticalAlign = VerticalAlign.Bottom;
                celCantidad3.HorizontalAlign = HorizontalAlign.Right;
                tr3.Cells.Add(celCantidad3);

                TableCell celPuntoVta3 = new TableCell();

                //LinkButton btnEditar3 = new LinkButton();

                //btnEditar3.CssClass = "btn btn-info ui-tooltip";
                //btnEditar3.Attributes.Add("data-toggle", "tooltip");
                //btnEditar3.Attributes.Add("title data-original-title", "Editar");
                //btnEditar3.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEditar3.Font.Size = 9;
                ////btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                //btnEditar3.OnClientClick = "window.open('../../Formularios/Facturas/FacturasF.aspx?fechadesde=" + this.desde + "&fechaHasta=" + this.hasta + "&Sucursal=" + this.sucursal + "', '_blank')";
                ////btnEditar3.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                //celPuntoVta3.Controls.Add(btnEditar3);
                celPuntoVta3.Width = Unit.Percentage(5);
                tr3.Cells.Add(celPuntoVta3);

                phVentas.Controls.Add(tr3);

                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de ventas. " + ex.Message));

            }
        }

        private void cargarComprasTabla(VentasCompras vc)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = "Compras";
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(65);
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = "$ " + vc.Compras.ToString("N");
                celCantidad.Width = Unit.Percentage(30);
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celPuntoVta = new TableCell();

                LinkButton btnEditar = new LinkButton();

                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar.OnClientClick = "window.open('../../Formularios/Compras/ComprasF.aspx', '_blank')";
                //btnEditar.PostBackUrl = "../../Formularios/Valores/CajaF.aspx";
                celPuntoVta.Controls.Add(btnEditar);
                celPuntoVta.Width = Unit.Percentage(5);
                tr.Cells.Add(celPuntoVta);

                phCompras.Controls.Add(tr);


                //cheques en cartera
                TableRow tr2 = new TableRow();

                TableCell celCodigo2 = new TableCell();
                celCodigo2.Text = "Iva Compras";
                celCodigo2.VerticalAlign = VerticalAlign.Bottom;
                celCodigo2.HorizontalAlign = HorizontalAlign.Left;
                celCodigo2.Width = Unit.Percentage(65);
                tr2.Cells.Add(celCodigo2);

                TableCell celCantidad2 = new TableCell();
                celCantidad2.Text = "$ " + vc.IvaCompras.ToString("N");
                celCantidad2.Width = Unit.Percentage(30);
                celCantidad2.VerticalAlign = VerticalAlign.Bottom;
                celCantidad2.HorizontalAlign = HorizontalAlign.Right;
                tr2.Cells.Add(celCantidad2);

                TableCell celPuntoVta2 = new TableCell();

                //LinkButton btnEditar2 = new LinkButton();

                //btnEditar2.CssClass = "btn btn-info ui-tooltip";
                //btnEditar2.Attributes.Add("data-toggle", "tooltip");
                //btnEditar2.Attributes.Add("title data-original-title", "Editar");
                //btnEditar2.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEditar2.Font.Size = 9;
                ////btnEditar.Click += new EventHandler(this.editarPuntoVenta);

                ////btnEditar2.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                //celPuntoVta2.Controls.Add(btnEditar2);
                celPuntoVta2.Width = Unit.Percentage(5);
                tr2.Cells.Add(celPuntoVta2);

                phCompras.Controls.Add(tr2);

                //Total Caja
                TableRow tr3 = new TableRow();

                TableCell celCodigo3 = new TableCell();
                celCodigo3.Text = "Retenciones";
                celCodigo3.VerticalAlign = VerticalAlign.Bottom;
                celCodigo3.HorizontalAlign = HorizontalAlign.Left;
                celCodigo3.Width = Unit.Percentage(65);
                tr3.Cells.Add(celCodigo3);

                TableCell celCantidad3 = new TableCell();
                celCantidad3.Text = "$ " + vc.Retenciones.ToString("N");
                celCantidad3.Width = Unit.Percentage(30);
                celCantidad3.VerticalAlign = VerticalAlign.Bottom;
                celCantidad3.HorizontalAlign = HorizontalAlign.Right;
                tr3.Cells.Add(celCantidad3);

                TableCell celPuntoVta3 = new TableCell();

                //LinkButton btnEditar3 = new LinkButton();

                //btnEditar3.CssClass = "btn btn-info ui-tooltip";
                //btnEditar3.Attributes.Add("data-toggle", "tooltip");
                //btnEditar3.Attributes.Add("title data-original-title", "Editar");
                //btnEditar3.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEditar3.Font.Size = 9;
                ////btnEditar.Click += new EventHandler(this.editarPuntoVenta);

                //btnEditar3.PostBackUrl = "../../Formularios/Valores/ChequesF.aspx";
                //celPuntoVta3.Controls.Add(btnEditar3);
                celPuntoVta3.Width = Unit.Percentage(5);
                tr3.Cells.Add(celPuntoVta3);

                phCompras.Controls.Add(tr3);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Compras. " + ex.Message));

            }
        }

        private void cargarInfoCirqueVenta(string value1, string value2, string value3, string valueTotal)
        {
            //string html = "CONSUMADO<div class=\"ui-cirque\" data-value=\""+ value1 +"\" data-total=\""+ valueTotal +"\" data-arc-color=\"#FF9900\"   data-label=\"ratio\"></div>";
            string html = "Ventas<div class=\"ui-cirque\" data-value=\"" + value1 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#FF9900\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Iva Ventas<div class=\"ui-cirque\" data-value=\"" + value2 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";
            html += "Precepciones II. BB.<div class=\"ui-cirque\" data-value=\"" + value3 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#222222\" data-radius=\"90\" data-label=\"ratio\"></div>";
            
            //html += "TENTATIVA<div class=\"ui-cirque\" data-value=\"" + value2 + "\" data-total=\"" + valueTotal + "\" data-arc-color=\"#0101DF\" data-radius=\"90\"  data-label=\"ratio\"></div>";

            //this.LitCharVentas.Text = html;

        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("BalanceF.aspx?a=2&d=" + this.txtFechaDesde.Text + "&h=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue);
            }
            catch
            {

            }
        }

        

    
    }
}