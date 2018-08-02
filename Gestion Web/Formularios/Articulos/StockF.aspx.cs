using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class StockF : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        controladorSucursal contSucu = new controladorSucursal();
        Mensajes m = new Mensajes();

        private int accion;
        private int suc;
        private int idArticulo;
        private string fechaD;
        private string fechaH;

        private int permisoEditar = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idArticulo = Convert.ToInt32(Request.QueryString["articulo"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.suc = Convert.ToInt32(Request.QueryString["s"]);

                this.VerificarLogin();                    

                if (!IsPostBack)
                {

                    this.cargarSucursal();
                    if (accion == 2) //Auditoria stock
                    {
                        this.suc = Convert.ToInt32(Request.QueryString["s"]);
                        this.lstSucursal.SelectedValue = this.suc.ToString();
                        this.txtFechaDesdeMov.Text = fechaD;
                        this.txtFechaHastaMov.Text = fechaH;
                        this.txtDesdePF.Text = fechaD;
                        this.txtHastaPF.Text = fechaH;
                        this.lblParametros.Text = fechaD + "," + fechaH + ", " + this.lstSucursal.SelectedItem.Text;
                        this.cargarMovimientoStock();
                        this.btnAccion.Visible = true;
                    }
                    if (accion == 0)
                    {
                        this.txtFechaDesdeMov.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.txtFechaHastaMov.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtDesdePF.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.txtHastaPF.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        this.filtrar();
                    }
                    if (accion == 3) //Cantidad ventas PRP FACT
                    {
                        this.suc = Convert.ToInt32(Request.QueryString["s"]);
                        this.ListSucursalPF.SelectedValue = this.suc.ToString();
                        this.txtFechaDesdeMov.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.txtFechaHastaMov.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtDesdePF.Text = fechaD;
                        this.txtHastaPF.Text = fechaH;
                        this.lblParametros.Text = fechaD + "," + fechaH + ", " + this.lstSucursal.SelectedItem.Text;
                        this.cargarVentasProducto();
                        this.cargarCompraProducto();
                        this.btnAccion.Visible = true;
                    }
                    this.cargarStockPRoducto();
                    this.cargarStockSucursal();
                    this.cargarStockTotalProducto();
                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error: " + ex.Message));
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
                //    if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Articulos.Articulos") != 1)
                //    {
                //        Response.Redirect("/Default.aspx?m=1", false);
                //    }
                    if(this.verificarAcceso() != 1)
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
                string permiso = listPermisos.Where(x => x == "14").FirstOrDefault();

                if (permiso == null)
                {
                    return 0;
                }
                else
                {
                    //verifico si puede cambiar preci0s
                    string permiso2 = listPermisos.Where(x => x == "62").FirstOrDefault();
                    if (permiso2 != null)
                    {                        
                        if (accion == 2)
                        {
                            this.permisoEditar = 1;
                                                    
                        }
                    }
                    //verifico si puede cambiar stock
                    string permiso3 = listPermisos.Where(x => x == "69").FirstOrDefault();
                    if (permiso3 != null)
                    {
                        if (accion == 2)
                        {
                            this.permisoEditar = 1;                            
                        }
                    }

                    //verifico si es super admin
                    string perfil = Session["Login_NombrePerfil"] as string;
                    if (perfil == "SuperAdministrador")
                    {
                        this.permisoEditar = 1;
                        this.lstSucursal.Attributes.Remove("disabled");
                        this.ListSucursalPF.Attributes.Remove("disabled");
                    }
                    else
                    {
                        //o si tiene permiso cambio suc                        
                        string permiso4 = listPermisos.Where(x => x == "153").FirstOrDefault();
                        if (permiso4 != null)
                        {
                            this.lstSucursal.Attributes.Remove("disabled");
                            this.ListSucursalPF.Attributes.Remove("disabled");
                        }
                        else
                        {
                            this.lstSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                        }
                    }

                    return 1;
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarStockPRoducto()
        {
            try
            {
                int suc = (int)Session["Login_SucUser"];
                phStock.Controls.Clear();
                List<Stock> stocks = this.contArticulo.obtenerStockArticulo(this.idArticulo);
                stocks = stocks.OrderBy(x => x.sucursal.nombre).ToList();
                foreach (Stock s in stocks)
                {
                    if (suc == s.sucursal.id)
                    {
                        this.lblStockSuc.Text = s.cantidad.ToString();
                        this.lblSucursal.Text = s.sucursal.nombre;
                    }
                    
                    this.cargarStockTable(s);
                    labelNombre.Text = s.articulo.codigo + "- " + s.articulo.descripcion;
                    
                }
            }

            catch
            {

            }
        }

        private void cargarStockTotalProducto()
        {
            try
            {
                if (this.idArticulo > 0)
                {
                    var list = this.contArticulo.obtenerStockArticulo(this.idArticulo);
                    decimal total = 0;

                    if (list != null)
                    {
                        foreach (Stock s in list)
                        {
                            total += s.cantidad;
                        }

                        lblStockTotalSucursales.Text = total.ToString();
                    }
                }
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stock total del articulo. Excepción: " + Ex.Message));
            }
        }

        private void cargarStockTable(Stock s)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celSucursal = new TableCell();
                celSucursal.Text = s.sucursal.nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celStock = new TableCell();
                celStock.Text = s.cantidad.ToString("N");
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);

                cargarVisualizacionTablaStock(tr, s);//TODO r new fun
                TableCell celAccion = new TableCell();

                LinkButton btnHistorico = new LinkButton();
                btnHistorico.CssClass = "btn btn-info ui-tooltip";
                btnHistorico.Attributes.Add("data-toggle", "tooltip");
                btnHistorico.Attributes.Add("title data-original-title", "Historico");
                btnHistorico.ID = "btnSelec_" + s.id;
                btnHistorico.Text = "<span class='shortcut-icon icon-list'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnHistorico.PostBackUrl = "StockHistoricoF.aspx?producto=" + s.id;
                celAccion.Controls.Add(btnHistorico);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.ID = "btnEditar_" + s.id;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.OnClientClick = "create(" + s.id + ");";
                //permiso editar
                if (this.permisoEditar == 0)
                {
                    btnEditar.Visible = false;
                }
                
                celAccion.Controls.Add(btnEditar);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                celAccion.Width = Unit.Percentage(12);
                tr.Cells.Add(celAccion);                

                phStock.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stock en la lista. " + ex.Message));
            }
        }
        void cargarVisualizacionTablaStock(TableRow tr, Stock s)//TODO r new fun carga las columnas dinamicamente
        {
            VisualizacionStock visualizacionStock = new VisualizacionStock();
            var stockImpPendiente = contArticulo.obtenerStockImportacionesPendientesBySuc(s.articulo.id, s.sucursal.id);//TODO r asignar el dato; 
            var stockRemitoPendiente = contArticulo.obtenerStockRemitosPendientesBySuc(s.articulo.id, s.sucursal.id);//TODO r asignar el dato 
            if (visualizacionStock.columnaImportacionesPendientes == 1)
            {
                TableCell celStock = new TableCell();
                celStock.Text = stockImpPendiente.ToString("N");
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);
                phImportacionesPendientes.Visible = true;
            }
            if (visualizacionStock.columnaRemitosPendientes == 1)
            {
                TableCell celStock = new TableCell();
                celStock.Text = stockRemitoPendiente.ToString("N");
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);
                phRemitosPendientes.Visible = true;
            }
            if (visualizacionStock.columnaStockReal == 1)
            {
                TableCell celStock = new TableCell();
                celStock.Text = (stockImpPendiente + stockRemitoPendiente).ToString("N");
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);
                phStockReal.Visible = true;
            }


        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                ////agrego Seleccione...
                //DataRow dr = dt.NewRow();
                //dr["nombre"] = "Seleccione...";
                //dr["id"] = -1;
                //dt.Rows.InsertAt(dr, 0);


                this.lstSucursal.DataSource = dt;
                this.lstSucursal.DataValueField = "Id";
                this.lstSucursal.DataTextField = "nombre";

                this.lstSucursal.DataBind();
                this.lstSucursal.SelectedValue = Session["Login_SucUser"].ToString();

                this.ListSucursalPF.DataSource = dt;
                this.ListSucursalPF.DataValueField = "Id";
                this.ListSucursalPF.DataTextField = "nombre";

                this.ListSucursalPF.DataBind();
                this.ListSucursalPF.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        private void cargarStockSucursal()
        {
            try
            {
                phStockAgrupado.Controls.Clear();
                DataTable dt = this.contArticulo.obtenerStockSucursalesDT(this.idArticulo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarStockSucursal(dr);
                    //labelNombre.Text = s.articulo.codigo + "- " + s.articulo.descripcion;
                }
            }
            catch
            {

            }
        }

        private void cargarStockSucursal(DataRow dr)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celSucursal = new TableCell();
                celSucursal.Text = dr[0].ToString();
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celStock = new TableCell();
                celStock.Text = dr[1].ToString();
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);

                phStockAgrupado.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stock en la lista. " + ex.Message));
            }
        }

        private void cargarMovimientoStock()
        {
            
            try
            {
                phMovimientoStock.Controls.Clear();
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23);

                DataTable dt = this.contArticulo.obtenerMovimientoStockArticuloCompra(this.idArticulo.ToString(), desde, hasta, this.suc);
                DataTable dt2 = this.contArticulo.obtenerMovimientoStockArticuloVenta(this.idArticulo.ToString(), desde, hasta, this.suc);
                DataTable dt3 = this.contArticulo.obtenerMovimientoStockArticulo(this.idArticulo.ToString(), desde, hasta, this.suc);
                dt.Merge(dt2);
                dt.Merge(dt3);

                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                decimal saldo = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                    if (dr["Numero"].ToString().Contains("Credito"))
                    {
                        cantidad = cantidad * -1;    
                    }
                    

                    this.cargarMovimientoStock(dr, cantidad);
                    saldo += cantidad;
                }
                this.labelSaldo.Text = saldo.ToString();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stocks en la lista. " + ex.Message));
            }
        }
        private void cargarMovimientoStock(DataRow dr, decimal cantidad)
        {
            try
            {
                TableRow tr = new TableRow();
                //Convert.ToDateTime(dr["Fecha"].ToString(), new CultureInfo("es-AR"));
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                //verifico si es una nota de credito y lo pongo como ingreso
                if (!dr["Numero"].ToString().Contains("Credito"))
                {
                    if (dr["Tipo"].ToString().Contains("Compra a"))//si es una compra a otra sucursal
                    {
                        celTipo.Text = "Compra Interna";
                    }
                    else
                    {
                        if (dr["Tipo"].ToString().Contains("RemitoCompra") || dr["Tipo"].ToString().Contains("Baja"))//si es anulacion de remito compra
                        {
                            celTipo.Text = "Egreso";
                        }
                        else
                        {
                            if (dr["Tipo"].ToString().Contains("Compuesto"))
                            {
                                celTipo.Text = "Articulo Compuesto";
                            }
                            else
                            {
                                celTipo.Text = dr["Tipo"].ToString();//si es ingreso o egreso por RemitoCompra o Venta.
                            }
                        }
                    }                    
                }
                else//si es nta de credito lo anoto como un ingreso
                {
                    celTipo.Text = "Ingreso";
                }
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["Tipo"].ToString(); 

                if (dr["Tipo"].ToString() == "Ingreso")
                {
                    celDescripcion.Text = "Remito " + dr["Numero"].ToString();
                }
                if (dr["Tipo"].ToString() == "Inventario")
                {
                    celDescripcion.Text = "Correcion stock 0" + dr["Numero"].ToString();
                }
                if (dr["Tipo"].ToString().Contains("Compra a"))
                {                    
                    celDescripcion.Text = dr["Tipo"].ToString();
                }
                if (dr["Tipo"].ToString() == "Egreso")
                {                     
                    celDescripcion.Text = dr["Numero"].ToString(); 
                }
                if (dr["Tipo"].ToString().Contains("RemitoCompra") || dr["Tipo"].ToString().Contains("Baja"))
                {
                    celDescripcion.Text = dr["Tipo"].ToString(); 
                }
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.Text = cantidad.ToString();
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celCliente = new TableCell();
                if (dr["SucursalFacturada"].ToString() != "0" && !String.IsNullOrEmpty(dr["SucursalFacturada"].ToString()))
                {
                    Sucursal facturada = this.contSucu.obtenerSucursalID(Convert.ToInt32(dr["SucursalFacturada"]));
                    celCliente.Text = facturada.nombre;
                }
                else
                {
                    celCliente.Text = dr["Cliente"].ToString();  
                }
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCliente);

                phMovimientoStock.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos en la lista. " + ex.Message));
            }
        }

        private void filtrar()
        {
            Response.Redirect("StockF.aspx?a=2&articulo=" + this.idArticulo + "&fd=" + this.txtFechaDesdeMov.Text + "&fh=" + this.txtFechaHastaMov.Text + "&s=" + this.lstSucursal.SelectedValue);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                filtrar();
            }
            catch(Exception ex)
            {

            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImpresionMovStock.aspx?a=1&articulo=" + this.idArticulo + "&fd=" + this.txtFechaDesdeMov.Text + "&fh=" + this.txtFechaHastaMov.Text + "&s=" + this.lstSucursal.SelectedValue);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=1&articulo=" + this.idArticulo + "&s=" + this.lstSucursal.SelectedValue + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }


        //private void actualixarStock()
        //{
        //    try
        //    {

        //        StreamWriter sw = new StreamWriter(Server.MapPath("ArchivoStock.txt") );
        //        List<Articulo> articulos = this.controlador.buscarArticuloList("%");
        //        foreach (var a in articulos)
        //        {
        //            decimal stock = this.obtnerStock(a.id.ToString());
        //            //
        //            int suc = Convert.ToInt32(this.lstSucursal.SelectedValue);
        //            string query = a.codigo + " ; " + a.descripcion.Replace('\n',' ') + " ; " + stock;
        //            //string query = "update stock set stock =" + stock + " where producto=" + a.id + " and local=" + suc;
        //            sw.WriteLine(query);
        //        }
        //        sw.Close();
        //    }
        //    catch
        //    {
 
        //    }
        //}

        private decimal obtnerStock(string art)
        {
            try
            {
                int suc = this.suc;//(int)Session["Login_SucUser"];

                phMovimientoStock.Controls.Clear();
                DateTime ddesde = new DateTime(2016, 1, 1);
                //DataTable dt = this.controlador.obtenerMovimientoStockArticuloCompra(art, Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), Convert.ToInt32(this.lstSucursal.SelectedValue));
                //DataTable dt2 = this.controlador.obtenerMovimientoStockArticuloVenta(art, Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), Convert.ToInt32(this.lstSucursal.SelectedValue));
                //DataTable dt3 = this.controlador.obtenerMovimientoStockArticulo(art, Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), Convert.ToInt32(this.lstSucursal.SelectedValue));
                DataTable dt = this.contArticulo.obtenerMovimientoStockArticuloCompra(art,ddesde, Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
                DataTable dt2 = this.contArticulo.obtenerMovimientoStockArticuloVenta(art, Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
                DataTable dt3 = this.contArticulo.obtenerMovimientoStockArticulo(art, Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
                dt.Merge(dt2);
                dt.Merge(dt3);

                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                decimal saldo = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                    
                    if (dr["Numero"].ToString().Contains("Credito"))
                    {
                        cantidad = cantidad * -1;
                    }

                    saldo += cantidad;
                }

                return saldo;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //this.actualixarStock();
            this.obtenerStockHistorico();
        }

        private void obtenerStockHistorico()
        {
            try
            {
                int suc = this.suc;//(int)Session["Login_SucUser"];
                StreamWriter sw = new StreamWriter(Server.MapPath("ArchivoStockHistorico.txt"));
                //List<Articulo> articulos = this.controlador.buscarArticuloList("%");
                List<Articulo> articulos = this.contArticulo.buscarArticuloListReduc("%");
                Sucursal sucursal = this.contSucu.obtenerSucursalID(suc);

                foreach (var a in articulos)                
                {
                    //List<Stock> stocks = this.controlador.obtenerStockArticulo(a.id);// obtengo stocks del art en todas las suc
                    List<Stock> stocks = this.contArticulo.obtenerStockArticuloReduc(a.id);// obtengo stocks del art en todas las suc
                    if (stocks != null && stocks.Count > 0)
                    {
                        Stock s = stocks.Where(x => x.sucursal.id == sucursal.id).FirstOrDefault();//filtro y obtengo id stock en esta suc      
                        if (s != null)
                        {
                            decimal stock = this.obtnerStock(a.id.ToString());// obtengo la cant de stock actual/real con el historico

                            //if (stock > 0)
                            if (stock != s.cantidad)
                            {
                                this.contArticulo.ActualizarStock(s.id, stock);//corrigo el stock segun lo que me dio el historico.
                                //string query = "update stock set stock =" + stock + " where Id=" + s.id + " and local=" + suc;//query que ejecuto
                                //string query = a.codigo + ";" + sucursal.id + ";" + s.cantidad + ";" + stock;
                                //sw.WriteLine(query);
                            }
                            //else
                            //{
                            //    //this.controlador.ActualizarStock(s.id, stock);//corrigo el stock segun lo que me dio el historico.
                            //    string query = "NEGATIVO --update stock set stock =" + stock + " where Id=" + s.id + " and local=" + suc;//query que ejecuto
                            //    sw.WriteLine(query);
                            //}
                        }
                        else
                        {
                            string query = "NO HAY STOCK EN LA SUCURSAL: " + suc + " PARA EL ARTICULO: ID = " + a.id;
                            sw.WriteLine(query);
                        }
                    }
                    else
                    {
                        string query = "NO HAY STOCK EN LA SUCURSAL: " + suc +" PARA EL ARTICULO: ID = " + a.id ;
                        sw.WriteLine(query);
                    }
                    
                }
                sw.Close();
            }
            catch(Exception ex)
            {

            }
        }

        #region filtro de ventas PF
        protected void btnFiltrarPF_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockF.aspx?a=3&articulo=" + this.idArticulo + "&fd=" + this.txtDesdePF.Text + "&fh=" + this.txtHastaPF.Text + "&s=" + this.ListSucursalPF.SelectedValue);
        }

        private void cargarVentasProducto()
        {
            try
            {
                phVentas.Controls.Clear();
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23);

                DataTable dt = this.contArticulo.obtenerArticulosVentasPF(this.idArticulo, desde, hasta, this.suc);
                
                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                decimal CantVentaP = 0;
                decimal CantVentaF = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                    if (dr["Factura"].ToString().Contains("Credito"))
                    {
                        cantidad = cantidad * -1;
                    }

                    string resp = this.cargarMovimientoVenta(dr, cantidad);
                    var val = resp.Split(';');
                    CantVentaP += Convert.ToDecimal(val[0]);
                    CantVentaF += Convert.ToDecimal(val[1]);
                }
                this.LitVentasP.Text = CantVentaP.ToString();
                this.LitVentasF.Text = CantVentaF.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stocks en la lista. " + ex.Message));
            }
        }

        private string cargarMovimientoVenta(DataRow dr, decimal cantidad)
        {
            try
            {
                TableRow tr = new TableRow();
                //Convert.ToDateTime(dr["Fecha"].ToString(), new CultureInfo("es-AR"));
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celCliente = new TableCell();
                celCliente.Text = dr["Factura"].ToString();
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["Cliente"].ToString();
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDescripcion);


                TableCell celCantidadP = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();

                celCantidadP.VerticalAlign = VerticalAlign.Middle;
                celCantidadP.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidadP);

                TableCell celCantidadF = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();

                celCantidadF.VerticalAlign = VerticalAlign.Middle;
                celCantidadF.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidadF);
                decimal CantVentaF = 0;
                decimal CantVentaP = 0;
                controladorFactEntity contFactEntity = new controladorFactEntity();
                int estaRefact = contFactEntity.verificarRefacturado(Convert.ToInt32(dr["id"]));
                if (estaRefact > 0)//venta en blanco
                {
                    celCantidadF.Text = cantidad.ToString();
                    celCantidadP.Text = "";

                    CantVentaF += cantidad;
                }
                else
                {
                    celCantidadP.Text = cantidad.ToString();
                    celCantidadF.Text = "";

                    CantVentaP += cantidad;
                }
                
                phVentas.Controls.Add(tr);

                return CantVentaP.ToString() + ";" + CantVentaF.ToString();

            }
            catch (Exception ex)
            {
                return null;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos en la lista. " + ex.Message));
            }
        }


        private void cargarCompraProducto()
        {
            try
            {
                phCompras.Controls.Clear();
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23);

                DataTable dt = this.contArticulo.obtenerArticulosComprasPF(this.idArticulo, desde, hasta, this.suc);

                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                decimal CantComprasP = 0;
                decimal CantComprasF = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                    if (Convert.ToInt32(dr["Devolucion"]) == 1 )
                    {
                        cantidad = cantidad * -1;
                    }

                    string resp = this.cargarMovimientCompra(dr, cantidad);
                    var val = resp.Split(';');
                    CantComprasP += Convert.ToDecimal(val[0]);
                    CantComprasF += Convert.ToDecimal(val[1]);
                   
                    CantComprasP += cantidad;
                }

                this.LitComprasP.Text = CantComprasP.ToString();
                this.LitComprasF.Text = CantComprasF.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stocks en la lista. " + ex.Message));
            }
        }
       

        private string cargarMovimientCompra(DataRow dr, decimal cantidad)
        {
            try
            {
                TableRow tr = new TableRow();
                //Convert.ToDateTime(dr["Fecha"].ToString(), new CultureInfo("es-AR"));
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celCliente = new TableCell();
                celCliente.Text = dr["Numero"].ToString();
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["Proveedor"].ToString();
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidadP = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();
                //celCantidadP.Text = cantidad.ToString();
                celCantidadP.VerticalAlign = VerticalAlign.Middle;
                celCantidadP.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidadP);

                TableCell celCantidadF = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();
                //celCantidadF.Text = cantidad.ToString();
                celCantidadF.VerticalAlign = VerticalAlign.Middle;
                celCantidadF.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidadF);

                decimal CantCompraF = 0;
                decimal CantCompraP = 0;

                if (Convert.ToInt32(dr["tipo"]) == 1)//venta en blanco
                {
                    celCantidadF.Text = cantidad.ToString();
                    celCantidadP.Text = "";

                    CantCompraF += cantidad;
                }
                else
                {
                    celCantidadP.Text = cantidad.ToString();
                    celCantidadF.Text = "";

                    CantCompraP += cantidad;
                }
                phCompras.Controls.Add(tr);

                return CantCompraP.ToString() + ";" + CantCompraF.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos en la lista. " + ex.Message));
                return null;
            }
        }


        #endregion
    }
}