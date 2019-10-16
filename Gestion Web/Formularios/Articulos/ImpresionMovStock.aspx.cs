using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ImpresionMovStock : System.Web.UI.Page
    {
        private controladorArticulo controlador = new controladorArticulo();
        private controladorSucursal contSucursal = new controladorSucursal();
        Mensajes m = new Mensajes();

        private int accion;
        private int suc;
        private int idArticulo;
        private string fechaD;
        private string fechaH;
        private int excel;

        private int tipofiltro;
        private int grupo;
        private int subgrupo;
        private int dias;
        private int proveedor;
        private int diasCero;

        private int costoValorizado;

        private string listas;

        private int sucCentral;
        private int sucCompara;

        private int movStock;
        int marca;
        string descSubGrupo;
        int categoria;

        private int artInactivos;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idArticulo = Convert.ToInt32(Request.QueryString["articulo"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.fechaD = Request.QueryString["fd"];
                    this.fechaH = Request.QueryString["fh"];
                    this.suc = Convert.ToInt32(Request.QueryString["s"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.diasCero = Convert.ToInt32(Request.QueryString["cero"]);
                    //tipo costo valorizado
                    this.costoValorizado = Convert.ToInt32(Request.QueryString["costo"]);

                    //filtro de articulos
                    this.tipofiltro = Convert.ToInt32(Request.QueryString["f"]);
                    this.grupo = Convert.ToInt32(Request.QueryString["g"]);
                    this.subgrupo = Convert.ToInt32(Request.QueryString["sg"]);
                    this.proveedor = Convert.ToInt32(Request.QueryString["p"]);
                    this.dias = Convert.ToInt32(Request.QueryString["d"]);
                    this.movStock = Convert.ToInt32(Request.QueryString["movStk"]);
                    this.marca = Convert.ToInt32(Request.QueryString["m"]);
                    this.descSubGrupo = Request.QueryString["dsg"];
                    this.categoria = Convert.ToInt32(Request.QueryString["c"]);

                    //listado stock unico en una suc
                    this.sucCentral = Convert.ToInt32(Request.QueryString["sCentral"]);
                    this.sucCompara = Convert.ToInt32(Request.QueryString["sCompara"]);

                    this.listas = Request.QueryString["listas"];

                    this.artInactivos = Convert.ToInt32(Request.QueryString["ai"]);

                    if (accion == 1)
                    {
                        generarReporte();
                    }
                    if (accion == 2)
                    {
                        generarReporte2();
                    }
                    if (accion == 3)
                    {
                        generarReporte3();
                    }
                    if (accion == 4)
                    {
                        //Thread t = new Thread(generarReporte4);
                        //t.Start();
                        generarReporte4();//Stock a dias
                    }
                    if (accion == 5)
                    {
                        generarReporte5();//Mercaderia no rotada (parodi)
                    }
                    if (accion == 6)
                    {
                        generarReporte6();//Stock unico en una sucursal contra otra
                    }
                    if (accion == 7)
                    {
                        generarReporte7();//Movimiento stock de sucursal
                    }
                    if (accion == 8)
                    {
                        generarReporte8();//Ingresos egresos por articulo
                    }
                    if (accion == 9)
                    {
                        generarReporte9();//Nomina de Articulos
                    }
                    if (accion == 10)
                    {
                        generarReporte10();//Stock Valorizado Detallado
                    }
                    if (accion == 11)
                    {
                        generarReporte11(); // Articulos Otros Proveedores
                    }
                    if (accion == 12)
                    {
                        generarReporte12(); // Stock por talles
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar Movimiento de stock. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar Movimiento de stock. " + ex.Message);
            }
        }

        private void generarReporte()
        {
            try
            {
                DateTime fechaHasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59).AddSeconds(59);
                Articulo articulo = this.controlador.obtenerArticuloId(this.idArticulo);
                DataTable dt = this.controlador.obtenerMovimientoStockArticuloCompra(this.idArticulo.ToString(), Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), fechaHasta, Convert.ToInt32(this.suc));
                DataTable dt2 = this.controlador.obtenerMovimientoStockArticuloVenta(this.idArticulo.ToString(), Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), fechaHasta, Convert.ToInt32(this.suc));
                DataTable dt3 = this.controlador.obtenerMovimientoStockArticulo(this.idArticulo.ToString(), Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), Convert.ToInt32(this.suc));

                dt.Merge(dt2);
                dt.Merge(dt3);
                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                decimal Cantidad = 0;
                decimal CantidadAux = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    CantidadAux = Convert.ToDecimal(dr["Cantidad"]);

                    if (dr["Tipo"].ToString() == "Ingreso")
                    {
                        dr["Numero"] = "Remito " + dr["Numero"].ToString();
                    }
                    if (dr["Tipo"].ToString() == "Inventario")
                    {
                        dr["Numero"] = "Correcion stock " + dr["Numero"].ToString().PadLeft(4, '0');
                    }
                    //si es nota de credito cambio cantidad y ingreso
                    if (dr["Numero"].ToString().Contains("Credito"))
                    {
                        CantidadAux = CantidadAux * -1;
                        dr["Cantidad"] = CantidadAux;
                        dr["Tipo"] = "Ingreso";
                    }

                    Cantidad += CantidadAux;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("MovimientoStockR.rdlc");

                ReportDataSource rds = new ReportDataSource("MovStock", dt);

                ReportParameter param = new ReportParameter("ParamCantidad", Cantidad.ToString());
                ReportParameter param2 = new ReportParameter("ParamArticulo", articulo.codigo + ", " + articulo.descripcion);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Movimiento_Stock", "xls");

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                this.Response.BinaryWrite(xlsContent);

                this.Response.End();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Movimiento Stock. " + ex.Message);
            }
        }

        private void generarReporte2()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Stock");

                List<Articulo> lstArticulos = new List<Articulo>();

                if (this.tipofiltro == 0)//todos
                {
                    lstArticulos = this.controlador.buscarArticuloList("%");
                }
                if (this.tipofiltro == 1)//filtrado
                {
                    string sdias = null;
                    if (dias > 0)
                    {
                        sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                    }

                    lstArticulos = this.controlador.filtrarArticulosGrupoSubGrupo(this.grupo, this.subgrupo, this.proveedor, sdias, this.marca, this.descSubGrupo);
                }

                if (lstArticulos != null && lstArticulos.Count > 0)
                {
                    foreach (Articulo art in lstArticulos)
                    {
                        decimal stock = this.obtnerStock(art.id.ToString(), this.fechaH, this.suc);

                        DataRow dr = dt.NewRow();
                        dr["Codigo"] = art.codigo.ToString();
                        dr["Descripcion"] = art.descripcion.ToString();
                        dr["Stock"] = stock;

                        dt.Rows.Add(dr);
                    }
                }

                Sucursal s = this.contSucursal.obtenerSucursalID(this.suc);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockAFechaR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosStock", dt);

                ReportParameter param = new ReportParameter("ParamSucursal", s.nombre);
                ReportParameter param2 = new ReportParameter("ParamFecha", this.fechaH);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Stock_Al_" + this.fechaH, "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Stock a Fecha. " + ex.Message);
            }
        }

        private void generarReporte3()
        {
            try
            {
                DataTable dtStockValorizado = this.controlador.obtenerStockValorizado(this.suc);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (this.costoValorizado == 1)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockValorizadoImponibleR.rdlc");
                }
                else
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockValorizadoR.rdlc");
                }

                ReportDataSource rds = new ReportDataSource("DatosValorizado", dtStockValorizado);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "StockValorizado_" + DateTime.Today.ToString("dd/MM/yyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        }

        private void generarReporte4()
        {
            try
            {
                controladorListaPrecio contListas = new controladorListaPrecio();
                DataTable dt = new DataTable();
                string listasElegidas = this.listas.Remove(listas.Length - 1);
                string[] l = listasElegidas.Split(',');

                string nombresListas = "";
                string nombresucursal = "TODAS";

                foreach (string nombre in l)
                {
                    listaPrecio ls = contListas.obtenerlistaPrecioID(Convert.ToInt32(nombre));

                    if (ls != null)
                    {
                        nombresListas += ls.nombre + ",";
                    }

                }

                if (this.suc > 0)
                {
                    Sucursal s = this.contSucursal.obtenerSucursalID(this.suc);
                    nombresucursal = s.nombre;
                }

                dt = this.controlador.obtenerStockArticulosAPedir(this.fechaD, this.fechaH, listasElegidas, this.proveedor, this.suc, this.dias, this.grupo, this.subgrupo, this.categoria);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockAPedir.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosStockRef", dt);

                ReportParameter param = new ReportParameter("ParamSucursal", nombresucursal);
                ReportParameter param2 = new ReportParameter("ParamDesde", this.fechaD);
                ReportParameter param3 = new ReportParameter("ParamHasta", this.fechaH);
                ReportParameter param4 = new ReportParameter("ParamListas", nombresListas);
                ReportParameter param5 = new ReportParameter("ParamCeros", this.diasCero.ToString());

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Stock_Al_" + this.fechaH, "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Stock a Fecha. " + ex.Message);
            }
        }

        private void generarReporte5()
        {
            try
            {
                controladorListaPrecio contListas = new controladorListaPrecio();
                DataTable dt = new DataTable();
                string listasElegidas = this.listas.Remove(listas.Length - 1);
                string[] l = listasElegidas.Split(',');

                string nombresListas = "";
                string nombresucursal = "TODAS";

                foreach (string nombre in l)
                {
                    listaPrecio ls = contListas.obtenerlistaPrecioID(Convert.ToInt32(nombre));

                    if (ls != null)
                    {
                        nombresListas += ls.nombre + ",";
                    }

                }

                if (this.suc > 0)
                {
                    Sucursal s = this.contSucursal.obtenerSucursalID(this.suc);
                    nombresucursal = s.nombre;
                }

                dt = this.controlador.obtenerStockArticulosNoVendidos(this.fechaD, this.fechaH, listasElegidas, this.proveedor, this.suc);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockNoVentaR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosStockRef", dt);

                ReportParameter param = new ReportParameter("ParamSucursal", nombresucursal);
                ReportParameter param2 = new ReportParameter("ParamDesde", this.fechaD);
                ReportParameter param3 = new ReportParameter("ParamHasta", this.fechaH);
                ReportParameter param4 = new ReportParameter("ParamListas", nombresListas);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Stock_Al_" + this.fechaH, "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        }

        private void generarReporte6()
        {
            try
            {
                DataTable dt = this.controlador.obtenerStockUnicoSucursalCompara(this.sucCentral, this.sucCompara);
                Sucursal central = this.contSucursal.obtenerSucursalID(this.sucCentral);
                Sucursal compara = this.contSucursal.obtenerSucursalID(this.sucCompara);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockUnicoR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosStockUnico", dt);
                ReportParameter param = new ReportParameter("ParamSucursalCentral", central.nombre);
                ReportParameter param2 = new ReportParameter("ParamSucursalCompara", compara.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Stock_Unico", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        }

        private void generarReporte7()
        {
            try
            {
                Sucursal suc = this.contSucursal.obtenerSucursalID(this.suc);
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                DataTable dt = new DataTable();

                if (this.movStock == 1)
                {
                    dt = this.controlador.obtenerMovimientoStockArticulo(this.idArticulo.ToString(), desde, hasta, this.suc);
                }
                else
                {
                    dt = this.controlador.obtenerMovimientoStockArticuloCompra(this.idArticulo.ToString(), desde, hasta, this.suc);
                    DataTable dt2 = this.controlador.obtenerMovimientoStockArticuloVenta(this.idArticulo.ToString(), desde, hasta, this.suc);
                    DataTable dt3 = this.controlador.obtenerMovimientoStockArticulo(this.idArticulo.ToString(), desde, hasta, this.suc);
                    dt.Merge(dt2);
                    dt.Merge(dt3);
                }

                dt.DefaultView.Sort = "Fecha";
                dt = dt.DefaultView.ToTable();

                foreach (DataRow dr in dt.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(dr["Cantidad"]);
                    if (dr["Numero"].ToString().Contains("Credito"))
                    {
                        cantidad = cantidad * -1;
                    }

                    if (dr["Tipo"].ToString().Contains("Inventario"))
                    {
                        dr["Numero"] = dr["Numero"].ToString().PadLeft(6, '0');
                    }
                    if (dr["Tipo"].ToString().Contains("Compra a"))
                    {
                        dr["Numero"] = dr["Tipo"].ToString();
                        dr["Tipo"] = "Compra Interna";
                    }
                    if (dr["Tipo"].ToString().Contains("Ingreso"))
                    {
                        dr["Numero"] = "Remito Compra nº " + dr["Numero"].ToString();
                    }
                }
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("MovStockSucursalR.rdlc");

                ReportDataSource rds = new ReportDataSource("MovStock", dt);
                ReportParameter param = new ReportParameter("ParamSucursal", suc.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Mov_Stock_Sucursal", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        }

        private void generarReporte8()
        {
            try
            {
                Sucursal suc = this.contSucursal.obtenerSucursalID(this.suc);
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                DataTable dtCompras = new DataTable();
                dtCompras = this.controlador.obtenerIngresosEgresosArticulosCompras(desde, hasta, this.suc);

                DataTable dtVentas = new DataTable();
                dtVentas = this.controlador.obtenerIngresosEgresosArticulosVentas(desde, hasta, this.suc);

                //dt.DefaultView.Sort = "Fecha";
                //dt = dt.DefaultView.ToTable();


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RIngEgrArticulos.rdlc");

                ReportDataSource rds = new ReportDataSource("dsMovimientos", dtCompras);
                ReportDataSource rds2 = new ReportDataSource("dsMovimientosVentas", dtVentas);
                ReportParameter param = new ReportParameter("ParamSucursal", suc.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Ingresos_Egresos", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        }

        private void generarReporte9()
        {
            try
            {
                DataTable dt = this.controlador.obtenerNominaDeArticulos(artInactivos);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("NominaArticulosR.rdlc");

                ReportDataSource rds = new ReportDataSource("NominaArticulos", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Nomina_Articulos", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al generar reporte de nomina de articulos. Error: " + ex.Message);
            }
        }

        private void generarReporte10()
        {
            try
            {
                DataTable dtStockValorizado = this.controlador.obtenerStockValorizadoDetallado(this.suc);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockValorizadoDetalladoR.rdlc");

                ReportDataSource rds = new ReportDataSource("ValorizadoDetallado", dtStockValorizado);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("EXCELOPENXML", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "StockValorizadoDetallado_" + DateTime.Today.ToString("dd/MM/yyyy"), "xlsx");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/openxmlformats-officedocument";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte11()
        {
            try
            {
                DataTable dtArticulosOtrosProv = this.controlador.obtenerArticulosOtrosProveedores();

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ArticulosOtrosProveedores.rdlc");

                ReportDataSource rds = new ReportDataSource("ProvArticulos", dtArticulosOtrosProv);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ArticulosOtrosProveedores_" + DateTime.Today.ToString("dd/MM/yyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                //else
                //{
                //    //get pdf content

                //    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //    this.Response.Clear();
                //    this.Response.Buffer = true;
                //    this.Response.ContentType = "application/pdf";
                //    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                //    this.Response.BinaryWrite(pdfContent);

                //    this.Response.End();
                //}
            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte12()
        {
            try
            {
                DataTable dtStockPorTalles = this.controlador.obtenerStockPorTalles(this.suc);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("StockPorTalles.rdlc");

                ReportDataSource rds = new ReportDataSource("StockPorTallesR", dtStockPorTalles);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "StockPorTalles" + DateTime.Today.ToString("dd/MM/yyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private decimal obtnerStock(string art, string fechaH, int suc)
        {
            try
            {
                DataTable dt = this.controlador.obtenerMovimientoStockArticuloCompra(art, Convert.ToDateTime("01/01/2000", new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
                DataTable dt2 = this.controlador.obtenerMovimientoStockArticuloVenta(art, Convert.ToDateTime("01/01/2000", new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
                DataTable dt3 = this.controlador.obtenerMovimientoStockArticulo(art, Convert.ToDateTime("01/01/2000", new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")), suc);
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
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}