using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ReporteAF : System.Web.UI.Page
    {
        controladorArticulo contArticulos = new controladorArticulo();
        controladorSucursal contSucursal = new controladorSucursal();

        int accion;
        int grupo;
        int subgrupo;
        int dias;
        int sucursal;
        int idVendedor;
        int proveedor;
        int ceros;
        int lista;
        int tipoEtiqueta;
        int excel;
        int sinStock;
        int marca;
        string textoBuscar;
        int inactivos;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.grupo = Convert.ToInt32(Request.QueryString["g"]);
                this.subgrupo = Convert.ToInt32(Request.QueryString["sg"]);
                this.textoBuscar = Request.QueryString["txt"];
                this.dias = Convert.ToInt32(Request.QueryString["d"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.proveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.ceros = Convert.ToInt32(Request.QueryString["c"]);
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.lista = Convert.ToInt32(Request.QueryString["l"]);
                this.tipoEtiqueta = Convert.ToInt32(Request.QueryString["t"]);
                this.excel = Convert.ToInt32(Request.QueryString["e"]);
                this.marca = Convert.ToInt32(Request.QueryString["m"]);
                this.sinStock = Convert.ToInt32(Request.QueryString["cero"]);
                this.inactivos = Convert.ToInt32(Request.QueryString["i"]);

                if (!IsPostBack)
                {
                    if (accion == 1)
                    {
                        this.cargarInforme();
                    }
                    if (accion == 2)
                    {
                        this.cargarInforme2();
                    }
                    if (accion == 3 || accion == 4 || accion == 5)
                    {
                        this.cargarInformeEtiquetas(lista);
                    }
                    if (accion == 6)
                    {
                        this.cargarInforme3();
                    }
                    if (accion == 7)
                    {
                        this.cargarInforme4();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarInforme()
        {
            try
            {
                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                var stock = contArticulos.obtenerStocksArticulosBySuc(sucursal, grupo, subgrupo, proveedor, sdias, ceros, marca, this.inactivos);
             
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteStockR.rdlc");
                ReportDataSource rds = new ReportDataSource("DSStocks", stock);

                string fecha = DateTime.Today.ToString("dd/MM/yyyy");
                ReportParameter rp = new ReportParameter("ParamFecha", fecha);
                
                controladorSucursal cont = new controladorSucursal();
                var suc = cont.obtenerSucursalID(this.sucursal);
                ReportParameter rp2 = new ReportParameter("ParamSucursal", suc.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(rp);
                this.ReportViewer1.LocalReport.SetParameters(rp2);

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Stocks", "xls");

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

            }
        }

        private void cargarInforme2()
        {
            try
            {
                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                var stock = contArticulos.obtenerStocksArticulosTodasSucursales(sucursal, grupo, subgrupo, proveedor, sdias, ceros, marca, this.inactivos);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteStock2.rdlc");
                ReportDataSource rds = new ReportDataSource("DsStock", stock);

                string fecha = DateTime.Today.ToString("dd/MM/yyyy");
                ReportParameter rp = new ReportParameter("ParamFecha", fecha);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(rp);
                //this.ReportViewer1.LocalReport.SetParameters(rp2);

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get pdf content
                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Stock-Sucursal", "xls");

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

        private void cargarInforme3()
        {
            try
            {
                controladorSucursal cont = new controladorSucursal();

                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                var stock = contArticulos.obtenerStocksUnicoArticulosBySuc(sucursal, grupo, subgrupo, proveedor, sdias, marca,this.inactivos);
                string fecha = DateTime.Today.ToString("dd/MM/yyyy");
                var suc = cont.obtenerSucursalID(this.sucursal);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteStockR.rdlc");

                ReportDataSource rds = new ReportDataSource("DSStocks", stock);
                ReportParameter rp = new ReportParameter("ParamFecha", fecha);
                ReportParameter rp2 = new ReportParameter("ParamSucursal", suc.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(rp);
                this.ReportViewer1.LocalReport.SetParameters(rp2);

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Stocks", "xls");

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
            catch
            {

            }
        }

        private void cargarInforme4()
        {
            try
            {
                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                var stock = contArticulos.obtenerStocksArticulosBySucConArticulosPendientes(sucursal, grupo, subgrupo, proveedor, sdias, ceros, marca, this.inactivos);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteStockR.rdlc");
                ReportDataSource rds = new ReportDataSource("DSStocks", stock);

                string fecha = DateTime.Today.ToString("dd/MM/yyyy");
                ReportParameter rp = new ReportParameter("ParamFecha", fecha);

                controladorSucursal cont = new controladorSucursal();
                var suc = cont.obtenerSucursalID(this.sucursal);
                ReportParameter rp2 = new ReportParameter("ParamSucursal", suc.nombre);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(rp);
                this.ReportViewer1.LocalReport.SetParameters(rp2);

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Stocks", "xls");

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

        private void cargarInformeEtiquetas(int lista)
        {
            try
            {
                //proveedor
                string sdias = null;

                List<Articulo> articulos = null;

                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }

                Log.EscribirSQL(1, "Info", "Voy a obtener los articulos para hacer las etiquetas");

                if (this.accion == 3)
                {
                    articulos = this.contArticulos.filtrarArticulosEtiquetas(grupo, subgrupo, proveedor, sdias, this.sucursal, marca);
                }
                if (this.accion == 4)
                {
                    articulos = this.contArticulos.buscarArticuloList(this.textoBuscar);
                }
                if (this.accion == 5)
                {
                    DateTime fecha = DateTime.Today.AddDays(this.dias * -1);
                    articulos = this.contArticulos.obtenerArticuloByFechaActualizacion(fecha);
                }

                Log.EscribirSQL(1,"Info","Obtuve la lista de articulos para hacer las etiquetas, voy a agregar las columnas al datatable");

                DataTable dt = new DataTable();
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Margen");
                dt.Columns.Add("Codigo");
                
                dt.Columns.Add("CodigoBarra");
                dt.Columns.Add("Imagen");

                Log.EscribirSQL(1, "Info", "voy a obtener la sucursal");
                Sucursal sucu = this.contSucursal.obtenerSucursalID(this.sucursal);

                foreach (var item in articulos)//Si el stock es 0, no lo agrego.
                {
                    Log.EscribirSQL(1, "Info", "voy a obtener los stocks de los articulos");
                    List<Stock> sList = this.contArticulos.obtenerStockArticulo(item.id);//busco los stock del articulo

                    Log.EscribirSQL(1, "Info", "voy a obtener el stock del articulo de la sucursal seleccionada");
                    Stock s = sList.Where(x => x.articulo.id == item.id && x.sucursal.nombre == sucu.nombre).FirstOrDefault();// Tomo de la list el stock de la sucursal seleccionada.

                    if (s != null)
                    {
                        Log.EscribirSQL(1, "Info", "voy a obtener el precio de la lista de precios");
                        item.precioVenta = contArticulos.obtenerPrecioLista(item, lista);

                        DataRow dr = dt.NewRow();
                        dr["Descripcion"] = item.descripcion;
                        dr["Precio"] = item.precioVenta.ToString("N");
                        dr["Codigo"] = item.codigo;
                        dr["CodigoBarra"] = item.codigoBarra ;
                        //
                        Log.EscribirSQL(1, "Info", "voy a obtener a generar el codigo");
                        string imagen = this.generarCodigo(dr["CodigoBarra"].ToString(), item.id);
                        dr["Imagen"] = @"file:///" + imagen;

                        if (item.margen <= 20)
                        {
                            dr["Margen"] = "★";
                        }
                        if (item.margen > 20 && item.margen <= 40)
                        {
                            dr["Margen"] = "★★";
                        }
                        if (item.margen > 40 && item.margen <= 60)
                        {
                            dr["Margen"] = "★★★";
                        }
                        if (item.margen > 60 && item.margen <= 80)
                        {
                            dr["Margen"] = "★★★★";
                        }
                        if (item.margen > 80)
                        {
                            dr["Margen"] = "★★★★★";
                        }

                        if (this.sinStock == 0)//Si el filtro no incluye articulos con stock 0. Agrego al DT.
                        {
                            if (s.cantidad > 0)
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                        else //Si el filtro incluye articulos con stock 0. Agrego al DT.
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }

                //foreach (var item in articulos)
                //{
                //    item.precioVenta = contArticulos.obtenerPrecioLista(item, lista);

                //    DataRow dr = dt.NewRow();
                //    dr["Descripcion"] = item.descripcion;
                //    dr["Precio"] = item.precioVenta.ToString("N");
                //    dt.Rows.Add(dr);
                //}


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                if (this.tipoEtiqueta == 1)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("REtiquetasChicas.rdlc");
                }
                if (this.tipoEtiqueta == 2)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("REtiquetasMedianas.rdlc");
                }
                if (this.tipoEtiqueta == 3)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("REtiquetasGrandes.rdlc");
                }
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DataSetArticulos", dt);

                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                
                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get pdf content

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/pdf";
                this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                this.Response.BinaryWrite(pdfContent);

                this.Response.End();

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al imprimir etiquetas " + ex.Message);
            }
        }

        public string generarCodigo(string codigo, long Traza)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = codigo;

                //Barcode39 code = new Barcode39();
                //code.CodeType = Barcode.CODE128;
                //code.ChecksumText = true;
                //code.GenerateChecksum = true;
                //code.StartStopText = false;
                //code.Code = codigo;


                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                //System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/CodigosEtiqueta/" + Traza + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + Traza + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para etiqueta. " + ex.Message);
                return null;
            }

        }
    }
}