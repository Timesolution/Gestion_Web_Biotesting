using Disipar.Models;
using ExportToExcel;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ImpresionReportes : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorPedido contPedido = new ControladorPedido();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();

        private int valor;
        private int excel;
        private string fechaD;
        private string fechaH;
        private int idSuc;
        private int idGrupo;
        private int idSubGrupo;
        private int idArticulo;
        private int idCliente;
        private int idVendedor;
        private int idProveedor;
        private int idTraza;
        private string listas;
        private int estado;
        private int tipo;
        private int categoria;
        private int auxiliar;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.fechaD = Request.QueryString["fd"] as string;
                    this.fechaH = Request.QueryString["fh"] + " 23:59:59.000";
                    this.idSuc = Convert.ToInt32(Request.QueryString["s"]);
                    this.idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                    this.idSubGrupo = Convert.ToInt32(Request.QueryString["sg"]);
                    this.idArticulo = Convert.ToInt32(Request.QueryString["a"]);
                    this.idCliente = Convert.ToInt32(Request.QueryString["c"]);
                    this.idVendedor = Convert.ToInt32(Request.QueryString["v"]);
                    this.idProveedor = Convert.ToInt32(Request.QueryString["prov"]);
                    this.estado = Convert.ToInt32(Request.QueryString["es"]);
                    this.listas = Request.QueryString["l"] as string;
                    this.tipo = Convert.ToInt32(Request.QueryString["t"]);
                    this.categoria = Convert.ToInt32(Request.QueryString["cat"]);
                    this.auxiliar = Convert.ToInt32(Request.QueryString["aux"]);

                    if (valor == 1)// reporte Articulos cantidad = 1 
                    {
                        this.generarReporte();
                    }
                    if (valor == 2)//reporte Articulos importes = 2
                    {
                        this.generarReporte2();
                    }
                    if (valor == 3)//reporte Cliente importes = 2
                    {
                        this.generarReporte3();
                    }
                    if (valor == 4)
                    {
                        this.generarReporte4();//reporte Sucursales importe
                    }
                    if (valor == 5)
                    {
                        this.generarReporte5();//Articulos x Cliente
                    }
                    if (valor == 6)
                    {
                        this.generarReporte6();//Importe x Vendedor Comision
                    }
                    if (valor == 7)
                    {
                        this.generarReporte7();// Reportes Articulos x Lista Precio Sucursal
                    }
                    if (valor == 8)
                    {
                        this.generarReporte8();//Reporte trazabilidad
                    }
                    if (valor == 9)
                    {
                        this.generarReporte9();//Reporte detalle trazabilidad
                    }
                    if (valor == 10)
                    {
                        this.generarReporte10(); //Reporte Articulos por Grupo
                    }
                    if (valor == 11)
                    {
                        this.generarReporte11(); // Reporte Compras.Articulos agrupado
                    }
                    if (valor == 12)
                    {
                        this.generarReporte12(); // Reporte Ventas.Articulos agrupado fecha
                    }
                    if (valor == 13)
                    {
                        this.generarReporte13(); // Reporte compras ventas.Articulos agrupado por proveedor
                    }
                    if (valor == 14)
                    {
                        this.generarReporte14(); // Reporte compras ventas.Articulos agrupado por categoria y proveedor
                    }
                    if (valor == 15)
                    {
                        this.generarReporte15(); // Reporte ventas. Por sucursales y puntos de venta
                    }
                    if (valor == 16)
                    {
                        this.generarReporte16(); // Reporte ventas. Por sucursales grupo subgrupo marca
                    }
                    if (valor == 17)
                    {
                        this.generarReporte17(); // Reporte compras y ventas de articulos. Por grupo marca cantidad
                    }
                    if (valor == 18)
                    {
                        this.generarReporte18(); // Reporte ventas por rango horario
                    }
                    if (valor == 19)
                    {
                        this.generarReporte19(); // Reporte ventas articulos con importe
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de ventas. " + ex.Message));
            }
        }

        private void generarReporte()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);

                //Tablas TOP Cantidades
                //fechaH += " 23:59:59.000";
                DataTable dtArticulosCant = contFacturacion.obtenerTopArticulosCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo,this.auxiliar);
                DataTable dtClientesCant = contFacturacion.obtenerTopClientesCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);
                DataTable dtVendedoresCant = contFacturacion.obtenerTopVendedoresCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasR.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("ArticulosCant", dtArticulosCant);
                ReportDataSource rds2 = new ReportDataSource("ClientesCant", dtClientesCant);
                ReportDataSource rds3 = new ReportDataSource("VendedoresCant", dtVendedoresCant);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                //this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Ranking_Ventas", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }
        }

        private void generarReporte2()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);
                //Tablas TOP Importes
                DataTable dtArticulosImporte = contFacturacion.obtenerTopArticulosImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo,this.auxiliar);
                //DataTable dtClientesImporte = contFacturacion.obtenerTopClientesImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);
                //DataTable dtVendedoresImporte = contFacturacion.obtenerTopVendedoresImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Ventas2R.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("ArticulosImporte", dtArticulosImporte);
                //ReportDataSource rds2 = new ReportDataSource("ClientesCant", dtClientesCant);
                //ReportDataSource rds3 = new ReportDataSource("VendedoresCant", dtVendedoresCant);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                //this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Ranking_Ventas", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }
        }

        private void generarReporte3()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);
                //Tablas TOP Importes
                //DataTable dtArticulosImporte = contFacturacion.obtenerTopArticulosImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);
                DataTable dtClientesImporte = contFacturacion.obtenerTopClientesImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);
                //DataTable dtVendedoresImporte = contFacturacion.obtenerTopVendedoresImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasClientesR.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("ClientesImporte", dtClientesImporte);
                //ReportDataSource rds2 = new ReportDataSource("ClientesCant", dtClientesCant);
                //ReportDataSource rds3 = new ReportDataSource("VendedoresCant", dtVendedoresCant);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                //this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Ranking_Ventas", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }

        }

        private void generarReporte4()
        {
            try
            {

                DataTable dtSucursalesImporte = contFacturacion.obtenerTopSucursalesImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasSucursalesR.rdlc");

                ReportDataSource rds = new ReportDataSource("SucursalesImporte", dtSucursalesImporte);

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

                    String filename = string.Format("{0}.{1}", "Ranking_Ventas_Sucursales", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }

        }

        private void generarReporte5()
        {
            try
            {

                DataTable dtArticulosClientes = contFacturacion.obtenerCantidadArticulosCliente(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosClientes.rdlc");

                ReportDataSource rds = new ReportDataSource("ArticulosClientes", dtArticulosClientes);

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

                    String filename = string.Format("{0}.{1}", "Cantidad_Articulos_Clientes", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas articulos x cliente. " + ex.Message));
            }

        }

        private void generarReporte6()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);
                DataTable dt = contFacturacion.obtenerTopVendedoresImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasVendedores.rdlc");

                ReportDataSource rds = new ReportDataSource("ImporteVendedor", dt);

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

                    String filename = string.Format("{0}.{1}", "Importe_Vendedores", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas articulos x cliente. " + ex.Message));
            }

        }

        private void generarReporte7()
        {
            try
            {
                controladorListaPrecio contListas = new controladorListaPrecio();

                string nombreLista = "";
                string listasElegidas = "";
                if (listas != "")
                {
                    listasElegidas = this.listas.Remove(listas.Length - 1);

                    foreach (string nombre in listasElegidas.Split(','))
                    {
                        listaPrecio ls = contListas.obtenerlistaPrecioID(Convert.ToInt32(nombre));

                        if (ls != null)
                        {
                            nombreLista += ls.nombre + ",";
                        }

                    }
                }

                DataTable dtImporte = contFacturacion.obtenerTopImporteArticulosListaSucursal(fechaD, fechaH, idSuc, idArticulo, listasElegidas, this.idProveedor);
                DataTable dtCantidad = contFacturacion.obtenerTopCantidadArticulosListaSucursal(fechaD, fechaH, idSuc, idArticulo, listasElegidas, this.idProveedor);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosLista.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosImportes", dtImporte);
                ReportDataSource rds2 = new ReportDataSource("DatosCantidad", dtCantidad);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

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

                    String filename = string.Format("{0}.{1}", "Reporte_Articulos_Lista", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas articulos x cliente. " + ex.Message));
            }

        }

        private void generarReporte8()
        {
            try
            {
                controladorCompraEntity contCompra = new controladorCompraEntity();
                controladorReportes contReport = new controladorReportes();

                DataTable dtTrazas = contCompra.obtenerTrazabilidadItemByArticuloGrupo(this.idGrupo, this.idArticulo, this.idSuc, this.estado);
                DataTable dtDatos = new DataTable();

                dtDatos.Columns.Add("Codigo");
                dtDatos.Columns.Add("Articulo");

                List<Trazabilidad_Campos> lstCampos = this.contArticulo.obtenerCamposTrazabilidadByGrupo(this.idGrupo);
                foreach (Trazabilidad_Campos campo in lstCampos)
                {
                    dtDatos.Columns.Add(campo.nombre);
                }
                dtDatos.Columns.Add("Estado");
                dtDatos.Columns.Add("Sucursal");
                dtDatos.Columns.Add("Numero");

                int pos = 0;
                int columnas = 0;

                DataRow dr = dtDatos.NewRow();

                foreach (DataRow row in dtTrazas.Rows)
                {
                    Log.EscribirSQL(1, "INFO", "Voy a pasar la traza con id " + row["Id"].ToString());
                    //this.cargarEnPH(row, pos);                    
                    if (columnas == 0)
                    {
                        dr = dtDatos.NewRow();
                        Articulo arti = this.contArticulo.obtenerArticuloByID(Convert.ToInt32(row["idArticulo"]));
                        dr["Codigo"] = arti.codigo;
                        dr["Articulo"] = arti.descripcion;
                        dr["Numero"] = row["Traza"].ToString();
                    }

                    if (columnas < dtDatos.Columns.Count)
                    {

                        dr[columnas + 2] = row["valor"];
                        columnas++;

                    }
                    if (columnas == (dtDatos.Columns.Count - 5))
                    {

                        if (row["estado"].ToString() == "1")
                        {
                            dr["Estado"] = "EN STOCK";
                        }
                        if (row["estado"].ToString() == "2")
                        {
                            dr["Estado"] = "VENDIDO";
                        }

                        dr["sucursal"] = row["sucursal"].ToString();

                        dtDatos.Rows.Add(dr);
                        columnas = 0;
                        pos++;
                    }
                }
                String path = HttpContext.Current.Server.MapPath("/Formularios/Reportes/Trazabildad/Excel/");

                string archivo = contReport.exportarTrazabilidad(dtDatos, path);

                Response.Redirect("/Formularios/Reportes/Trazabildad/Excel/" + archivo);

            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte9()
        {
            try
            {
                controladorReportes contReport = new controladorReportes();
                Articulo art = this.contArticulo.obtenerArticuloByID(this.idArticulo);

                DataTable dtDetalle = contReport.generarDetalleTrazabilidad(this.idArticulo, this.idTraza, this.idGrupo, this.idSuc);

                String path = HttpContext.Current.Server.MapPath("/Formularios/Reportes/Trazabildad/Excel/");

                string archivo = contReport.exportarTrazabilidad(dtDetalle, path, "DetalleTrazabilidad_" + art.codigo);

                Response.Redirect("/Formularios/Reportes/Trazabildad/Excel/" + archivo);

            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte10()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);

                //Tablas TOP Cantidades
                //fechaH += " 23:59:59.000";
                DataTable dtArticulosCant = contFacturacion.obtenerTopGruposArticulosCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);
                DataTable dtClientesCant = contFacturacion.obtenerTopClientesCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);
                DataTable dtVendedoresCant = contFacturacion.obtenerTopVendedoresCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosGrupoR.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("ArticulosCant", dtArticulosCant);
                ReportDataSource rds2 = new ReportDataSource("ClientesCant", dtClientesCant);
                ReportDataSource rds3 = new ReportDataSource("VendedoresCant", dtVendedoresCant);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Ranking_Ventas_Grupos", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }
        }

        private void generarReporte11()
        {
            try
            {
                DateTime fechaDesde = Convert.ToDateTime(fechaD, new CultureInfo("es-AR"));
                DateTime fechaHasta = Convert.ToDateTime(fechaH, new CultureInfo("es-AR"));

                var dtRemitosComprasItems = controladorCompraEntity.obtenerRemitosCompra_Items(fechaDesde, fechaHasta, idProveedor);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComprasArticulosR.rdlc");

                ReportDataSource rds = new ReportDataSource("ReportesCompras", dtRemitosComprasItems);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Reporte_CompraArticulos", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
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

        private void generarReporte12()
        {
            try
            {
                controladorListaPrecio contListas = new controladorListaPrecio();

                string nombreLista = "";
                string listasElegidas = "";
                if (listas != "")
                {
                    listasElegidas = this.listas.Remove(listas.Length - 1);

                    foreach (string nombre in listasElegidas.Split(','))
                    {
                        listaPrecio ls = contListas.obtenerlistaPrecioID(Convert.ToInt32(nombre));

                        if (ls != null)
                        {
                            nombreLista += ls.nombre + ",";
                        }

                    }
                }

                DataTable dtArticulosFecha = contFacturacion.obtenerTopArticulosListaByFecha(fechaD, fechaH, idSuc, idArticulo, listasElegidas, this.idProveedor);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosListaXFecha.rdlc");

                ReportDataSource rds = new ReportDataSource("CantArtGroupByFecha", dtArticulosFecha);

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

                    String filename = string.Format("{0}.{1}", "Reporte_Articulos_Cantidad_Ventas_Por_Fecha", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas articulos x cliente. " + ex.Message));
            }

        }

        private void generarReporte13()
        {
            try
            {
                //ventas
                DataTable dt = this.contCliente.obtenerListaPrecios();

                string listas = "";
                foreach (DataRow lista in dt.Rows)
                {
                    if (lista[2].ToString().Contains("1"))
                    {
                        listas += lista[0] + ",";
                    }
                }
                listas = listas.Remove(listas.Length - 1, 1);
                DataTable dtVentas = contFacturacion.obtenerTopArticulosCantidadByProveedor(fechaD, fechaH, idSuc, idProveedor, listas, 0);
                //compras
                DateTime fechaDesde = Convert.ToDateTime(fechaD, new CultureInfo("es-AR"));
                DateTime fechaHasta = Convert.ToDateTime(fechaH, new CultureInfo("es-AR"));
                DataTable dtCompras = controladorCompraEntity.obtenerTopRemitosProveedores_ItemsFiltro(fechaDesde, fechaHasta, idProveedor);

                dtVentas.Merge(dtCompras, true);

                DataTable dtVentasArtEnOferta = contFacturacion.obtenerTopArticulosCantidadByProveedorArticulosEnOferta(fechaD, fechaH, idSuc, idProveedor, listas, 0);
                dtVentas.Merge(dtVentasArtEnOferta, true);

                var result = from row in dtVentas.AsEnumerable()
                             where row.Field<decimal?>("cantidadComprada") > 0 ||
                                   row.Field<decimal?>("cantidadVendida") > 0 ||
                                   row.Field<decimal?>("cantidadVendidaOferta") > 0
                             group row by new { razonSocial = row.Field<string>("razonSocial") } into grp
                             select new
                             {
                                 razonSocial = grp.Key.razonSocial,
                                 cantidadVendida = grp.Sum(x => x.Field<decimal?>("cantidadVendida")),
                                 cantidadComprada = grp.Sum(x => x.Field<decimal?>("cantidadComprada")),
                                 cantidadVendidaOferta = grp.Sum(x => x.Field<decimal?>("cantidadVendidaOferta")),
                             };
                var listAgrupado = result.ToList();

                DataTable dtFinal = new DataTable();
                dtFinal = ListToDataTable(listAgrupado);

                //Lo ordeno por cantidad
                DataView dv = dtFinal.DefaultView;

                DataTable sortedDT = dv.ToTable();

                var dtComprasVentasProveedores = dtVentas;

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComprasVentasArticulosProv.rdlc");

                ReportDataSource rds = new ReportDataSource("dsComprasVentasProveedores", dtFinal);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "Reporte_CompraVentaArticulosProveedores", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
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

        private void generarReporte14()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                    this.listas = this.listas.Remove(listas.Length - 1);

                DataTable dtArticulosCant = contFacturacion.obtenerArticulosVendidosAgrupadoPorCategoriaAndProveedor(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, this.listas, this.tipo);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosByCategoriaAndProveedor.rdlc");

                ReportDataSource rds = new ReportDataSource("ArticulosCategoria", dtArticulosCant);
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

                    String filename = string.Format("{0}.{1}", "Ventas_Articulos_Categoria_proveedores", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas. " + ex.Message));
            }
        }

        private void generarReporte15()
        {
            try
            {
                DataTable dtReporteVentasBySucursalesAndPuntosDeVenta = contFacturacion.obtenerTotalVentasRealizadasBySucursalesAndPuntosDeVenta(fechaD, fechaH);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasBySucursalesAndPuntoDeVenta.rdlc");

                ReportDataSource rds = new ReportDataSource("VentasBySucursalesAndPuntoDeVenta", dtReporteVentasBySucursalesAndPuntosDeVenta);
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

                    String filename = string.Format("{0}.{1}", "Ventas_Por_Sucursales_y_puntos_de_venta", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte15. " + ex.Message));
            }
        }

        private void generarReporte16()
        {
            try
            {
                DataTable dtReporteSucursalesGrupoSubGrupoMarca = contFacturacion.obtenerVentasRealizadasAgrupadoPor_Sucursal_Grupo_SubGrupo_Marca(fechaD, fechaH);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasBySucursalesAgrupadoGrupoSubGrupoMarca.rdlc");

                ReportDataSource rds = new ReportDataSource("VentasAgrupadoSucursalGrupoSubGrupoMarca", dtReporteSucursalesGrupoSubGrupoMarca);
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

                    String filename = string.Format("{0}.{1}", "Ventas_Por_Sucursales_Grupo_SubGrupo_Marca", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte15. " + ex.Message));
            }
        }

        private void generarReporte17()
        {
            try
            {
                DataTable dtReporteComprasAndVentasByMarcaGrupo = contFacturacion.obtenerCantidadArticulosCompradosAndVendidosAgrupado_Marca_Grupo_Cantidad(fechaD, fechaH);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComprasYVentasByGrupoMarcaCantidad.rdlc");

                ReportDataSource rds = new ReportDataSource("DataSetComprasAndVentas", dtReporteComprasAndVentasByMarcaGrupo);
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

                    String filename = string.Format("{0}.{1}", "Compras_Y_Ventas_Por_Marca_Grupo", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte17. " + ex.Message));
            }
        }

        private DataTable procesarDataTableYObtenerloParaReporteVentasAgrupadoPorRangoHorario(DataTable tablaConRegistrosDeRangoHorario)
        {
            try
            {
                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");
                bool restoHoras = false;

                if (Convert.ToInt32(modificoHora) == 1)
                {
                    restoHoras = true;
                }

                string fd = this.fechaD.ToString();

                DateTime fechaAFiltrar = Convert.ToDateTime(fd, new CultureInfo("es-AR"));
                string fechaAFiltrarString = fechaAFiltrar.ToString("dd/MM/yyyy") + " 06:00 AM";
                DateTime horaPorVenta = Convert.ToDateTime(fechaAFiltrarString, new CultureInfo("es-AR"));

                DataTable tablaConHorasSinCantidad = new DataTable();
                tablaConHorasSinCantidad.Columns.Add("fechaDateTime");
                tablaConHorasSinCantidad.Columns.Add("rangoHorario");
                tablaConHorasSinCantidad.Columns.Add("facturasRealizadas", typeof(decimal));

                for (int i = 0; i < 17; i++)
                {
                    tablaConHorasSinCantidad.Rows.Add();

                    tablaConHorasSinCantidad.Rows[i]["fechaDateTime"] = horaPorVenta;
                    tablaConHorasSinCantidad.Rows[i]["rangoHorario"] = horaPorVenta.ToString("dd/MM/yyyy") + " " + horaPorVenta.ToShortTimeString() + " A " + horaPorVenta.AddHours(1).ToShortTimeString();
                    tablaConHorasSinCantidad.Rows[i]["facturasRealizadas"] = 0;

                    horaPorVenta = horaPorVenta.AddHours(1);
                }

                foreach (DataRow dr in tablaConRegistrosDeRangoHorario.Rows)
                {
                    DateTime fechaDelRegistroDeLaBase = Convert.ToDateTime(dr["fecha"]);

                    if (restoHoras)//si es azure resto 3 horas
                    {
                        fechaDelRegistroDeLaBase = fechaDelRegistroDeLaBase.AddHours(-3);
                    }

                    for (int i = 0; i < 17; i++)
                    {
                        DateTime fechaDeLaTablaSinCantidad = Convert.ToDateTime(tablaConHorasSinCantidad.Rows[i]["fechaDateTime"]);

                        if (fechaDelRegistroDeLaBase == fechaDeLaTablaSinCantidad)
                        {
                            tablaConHorasSinCantidad.Rows[i]["facturasRealizadas"] = dr["facturasRealizadas"];
                        }
                    }
                }
                return tablaConHorasSinCantidad;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        private void generarReporte18()
        {
            try
            {
                DataTable dtReporteVentasByRangoHorario = contFacturacion.obtenerFacturasVendidasPorSucursalesEnRangosDeHora(fechaD, idSuc);

                DataTable dtFinal = procesarDataTableYObtenerloParaReporteVentasAgrupadoPorRangoHorario(dtReporteVentasByRangoHorario);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasDeFacturasBySucursalesByRangoHorario.rdlc");

                ReportDataSource rds = new ReportDataSource("VentasDeFacturasAgrupadoPorRangoHorario", dtFinal);
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

                    String filename = string.Format("{0}.{1}", "VentasPorFacturasRangoHorario", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte17. " + ex.Message));
            }
        }

        private void generarReporte19()
        {
            try
            {
                DataTable dtReporte_Sucursal_Grupo_SubGrupo_Marca_Codigo_Descripcion_Cantidad_ImporteTotal = contFacturacion.obtenerVentasRealizadasAgrupadoPor_Sucursal_Grupo_SubGrupo_Marca_Codigo_Descripcion_Cantidad_ImporteTotal(fechaD, fechaH, listas);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasArticulosSucursalesConImporte.rdlc");

                ReportDataSource rds = new ReportDataSource("VentasArticulosSucursalesConImporte", dtReporte_Sucursal_Grupo_SubGrupo_Marca_Codigo_Descripcion_Cantidad_ImporteTotal);
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

                    String filename = string.Format("{0}.{1}", "Ventas_Por_Sucursales_Articulos_Cantidad_Importe", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte15. " + ex.Message));
            }
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
    }
}