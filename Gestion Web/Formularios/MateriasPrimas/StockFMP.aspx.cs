using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
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

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class StockFMP : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        controladorSucursal contSucu = new controladorSucursal();
        Mensajes m = new Mensajes();

        private int accion;
        private int suc;
        private int idMP;
        private string fechaD;
        private string fechaH;

        private int permisoEditar = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idMP = Convert.ToInt32(Request.QueryString["mp"]);
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
                        //this.btnAccion.Visible = true;
                    }
                    if (accion == 0)
                    {
                        this.txtFechaDesdeMov.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.txtFechaHastaMov.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        this.txtDesdePF.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.txtHastaPF.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        this.filtrar();
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
                    //string permiso2 = listPermisos.Where(x => x == "62").FirstOrDefault();
                    //if (permiso2 != null)
                    //{                        
                    //    if (accion == 2)
                    //    {
                    //        this.permisoEditar = 1;
                                                    
                    //    }
                    //}
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

                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                //List<Stock> stocks = this.contArticulo.obtenerStockArticulo(this.idArticulo);

                var materiaPrimaSuc = controladorMateriaPrima.ObtenerStockMP(idMP);

                //stocks = stocks.OrderBy(x => x.sucursal.nombre).ToList();

                var materiaPrima = materiaPrimaSuc.OrderBy(x => x.NombreSucursal).ToList();

                foreach (var mat in materiaPrima)
                {
                    if (suc == mat.IdSuc)
                    {
                        this.lblStockSuc.Text = mat.Cant.ToString();
                        this.lblSucursal.Text = mat.NombreSucursal.ToString();
                    }
                    
                    this.cargarStockTable1(mat.Id, mat.Cant, mat.NombreSucursal);
                    labelNombre.Text = mat.Codigo + "- " + mat.Descripcion;
                    
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
                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                if (idMP > 0)
                {
                    var list = controladorMateriaPrima.ObtenerStockMP(idMP);
                    decimal total = 0;

                    if (list != null)
                    {
                        foreach (var mat in list)
                        {
                            total += mat.Cant;
                        }

                        lblStockTotalSucursales.Text = total.ToString();
                    }
                }
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stock total de la materia prima. Excepción: " + Ex.Message));
            }
        }

        private void cargarStockTable1(int idMP, decimal cantidad, string nombreSucursal)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celSucursal = new TableCell();
                celSucursal.Text = nombreSucursal;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celStock = new TableCell();
                celStock.Text = cantidad.ToString("N");
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);

                //cargarVisualizacionTablaStock(tr, s);
                TableCell celAccion = new TableCell();

                //LinkButton btnHistorico = new LinkButton();
                //btnHistorico.CssClass = "btn btn-info ui-tooltip";
                //btnHistorico.Attributes.Add("data-toggle", "tooltip");
                //btnHistorico.Attributes.Add("title data-original-title", "Historico");
                //btnHistorico.ID = "btnSelec_" + idMP;
                //btnHistorico.Text = "<span class='shortcut-icon icon-list'></span>";
                ////btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                ////btnHistorico.PostBackUrl = "StockHistoricoF.aspx?producto=" + idMP;
                //celAccion.Controls.Add(btnHistorico);
                //celAccion.VerticalAlign = VerticalAlign.Middle;

                //Literal l = new Literal();
                //l.Text = "&nbsp";
                //celAccion.Controls.Add(l);

                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.ID = "btnEditar_" + idMP;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.OnClientClick = "create(" + idMP + ");";
                //permiso editar
                if (this.permisoEditar == 0)
                {
                    btnEditar.Visible = false;
                }

                celAccion.Controls.Add(btnEditar);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                celAccion.Width = Unit.Percentage(8);
                tr.Cells.Add(celAccion);

                phStock.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando materia prima en la lista. " + ex.Message));
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego Seleccione...
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

                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                //DataTable dt = this.contArticulo.obtenerStockSucursalesDT(this.idArticulo);

                var totalStockSuc = controladorMateriaPrima.obtenerStockSucursales(idMP);

                foreach (var dr in totalStockSuc)
                {
                    this.cargarStockSucursal(dr.NombreSucursal, dr.StockTotal);
                    //labelNombre.Text = s.articulo.codigo + "- " + s.articulo.descripcion;
                }
            }
            catch
            {

            }
        }

        private void cargarStockSucursal(string nombreSucursal, decimal stockTotal)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celSucursal = new TableCell();
                celSucursal.Text = nombreSucursal;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celStock = new TableCell();
                celStock.Text = stockTotal.ToString();
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
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                //DateTime desde = DateTime.ParseExact(fechaD, "MM/dd/yyyy", new CultureInfo("es-AR"));
                //DateTime hasta = DateTime.ParseExact(fechaH, "MM/dd/yyyy", new CultureInfo("es-AR"));

                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                var movimientosMP = controladorMateriaPrima.ObtenerMovimientoStockMateriaPrima(idMP, desde, hasta, suc);

                decimal total = 0;

                //DataTable dt = this.contArticulo.obtenerMovimientoStockArticuloCompra(this.idArticulo.ToString(), desde, hasta, this.suc);
                //DataTable dt2 = this.contArticulo.obtenerMovimientoStockArticuloVenta(this.idArticulo.ToString(), desde, hasta, this.suc);
                //DataTable dt3 = this.contArticulo.obtenerMovimientoStockArticulo(this.idArticulo.ToString(), desde, hasta, this.suc);
                //dt.Merge(dt2);
                //dt.Merge(dt3);

                //dt.DefaultView.Sort = "Fecha";
                //dt = dt.DefaultView.ToTable();

                //decimal saldo = 0;

                //foreach (DataRow dr in dt.Rows)
                //{
                //    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                //    if (dr["Numero"].ToString().Contains("Credito"))
                //    {
                //        cantidad = cantidad * -1;
                //    }
                //    this.cargarMovimientoStock(dr, cantidad);
                //    saldo += cantidad;
                //}
                //this.labelSaldo.Text = saldo.ToString();


                if (movimientosMP != null)
                {
                    foreach (var movimiento in movimientosMP)
                    {
                        cargarMovientoStock1(movimiento.Fecha, movimiento.Cantidad, movimiento.NombreCliente, movimiento.DescripcionTratamiento);

                        total += movimiento.Cantidad;

                    }

                    this.labelSaldo.Text = total.ToString();

                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stocks en la lista. " + ex.Message));
            }
        }

        private void cargarMovientoStock1(DateTime fecha, decimal cantidad, string nombreCliente, string descripcionTratamiento)
        {
            TableRow tr = new TableRow();

            TableCell celFecha = new TableCell();
            celFecha.Text = fecha.ToString("dd/MM/yyyy");
            celFecha.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(celFecha);

            TableCell celTratamiento = new TableCell();
            celTratamiento.Text = descripcionTratamiento;
            celTratamiento.VerticalAlign = VerticalAlign.Middle;
            celTratamiento.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(celTratamiento);

            TableCell celCantidad = new TableCell();
            celCantidad.Text = cantidad.ToString("N");
            celCantidad.VerticalAlign = VerticalAlign.Middle;
            celCantidad.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(celCantidad);

            TableCell celCliente = new TableCell();
            celCliente.Text = nombreCliente;
            celCliente.VerticalAlign = VerticalAlign.Middle;
            celCliente.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(celCliente);

            

            phMovimientoStock.Controls.Add(tr);
        }

        private void filtrar()
        {
            Response.Redirect("StockFMP.aspx?a=2&mp=" + this.idMP + "&fd=" + this.txtFechaDesdeMov.Text + "&fh=" + this.txtFechaHastaMov.Text + "&s=" + this.lstSucursal.SelectedValue);
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
            //Response.Redirect("ImpresionMovStock.aspx?a=1&articulo=" + this.idArticulo + "&fd=" + this.txtFechaDesdeMov.Text + "&fh=" + this.txtFechaHastaMov.Text + "&s=" + this.lstSucursal.SelectedValue);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=1&articulo=" + this.idArticulo + "&s=" + this.lstSucursal.SelectedValue + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

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
            Response.Redirect("StockFMP.aspx?a=3&mp=" + this.idMP + "&fd=" + this.txtDesdePF.Text + "&fh=" + this.txtHastaPF.Text + "&s=" + this.ListSucursalPF.SelectedValue);
        }


        #endregion
    }
}