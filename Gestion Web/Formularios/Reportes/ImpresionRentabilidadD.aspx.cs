using Disipar.Models;
using Gestion_Api.Controladores;
using Microsoft.Reporting.WebForms;
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
    public partial class ImpresionRentabilidadD : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorInformes contInformes = new controladorInformes();
        
        private int accion;
        private int excel;
        private string fechaD;
        private string fechaH;
        private string articulo;
        private string numFact;
        private int tipoBusqueda;
        private int idSuc;        
        private string nombreArticulo;
        private int conIva;
        private int idCliente;
        private string paramRentabilidadCosto;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.fechaD = Request.QueryString["fd"] as string;
                    this.fechaH = Request.QueryString["fh"] as string;
                    this.articulo = Request.QueryString["ar"] as string;
                    this.numFact = Request.QueryString["num"] as string;
                    this.tipoBusqueda = Convert.ToInt32(Request.QueryString["t"]);
                    this.idSuc = Convert.ToInt32(Request.QueryString["suc"]);
                    this.conIva = Convert.ToInt32(Request.QueryString["iva"]);
                    this.nombreArticulo = Request.QueryString["art"] as string;
                    this.idCliente = Convert.ToInt32(Request.QueryString["c"]);
                    this.paramRentabilidadCosto = Request.QueryString["p"] as string;
                    //this.idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                    //this.idSubGrupo = Convert.ToInt32(Request.QueryString["sg"]);
                    //this.idVendedor = Convert.ToInt32(Request.QueryString["v"]);

                    if (accion == 1)// reporte rentabilidad por sucursal
                    {
                        this.generarReporte();
                    }
                    if (accion == 2)//reporte rentabilidadF.aspx
                    {
                        this.generarReporte2();
                    }
                    if (accion == 3) //reporte RentabilidadFCosto
                    {
                        this.generarReporte3();
                    }
                    if (accion == 4) //reporte RentabilidadFCosto filtrado por Articulo
                    {
                        this.generarReporte4();
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
                //Tablas TOP Cantidades
                DataTable dtRentabilidad = contInformes.obtenerRentabilidadCostoPorSucursal(Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23.9), this.articulo);
                
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RentabilidadSucursal.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("dtRentabilidad", dtRentabilidad);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                
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
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));
                hasta = hasta.AddHours(23).AddMinutes(59).AddSeconds(59);//23:59:59 hs

                DataTable datos = this.contInformes.obtenerRentabilidadCosto(desde, hasta, this.idSuc, this.articulo, this.numFact, this.tipoBusqueda,this.conIva);
                datos.Columns[4].ColumnName = "Costo_Real";
                datos.Columns[5].ColumnName = "Costo_Imponible";
                datos.Columns[7].ColumnName = "Precio_Unitario";
                datos.Columns[8].ColumnName = "Rentabilidad_Costo";
                datos.Columns[9].ColumnName = "Porcentaje_Rentabilidad";

                decimal costoT = 0;
                decimal precioT = 0;

                foreach (DataRow dr in datos.Rows)
                {
                    if (Convert.ToDecimal(dr["Costo"]) <= 0)
                    {
                        costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(0.01);
                    }
                    else
                    {
                        costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo"]);
                    }
                    //costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo"]);
                    precioT += Convert.ToDecimal(dr["Cantidad"]) * Decimal.Round(Convert.ToDecimal(dr["Precio_Unitario"]), 4);
                }

                //ganancia
                decimal ganancia = decimal.Round(precioT - costoT, 4);
                //porcentaje
                decimal porGanancia = decimal.Round((((precioT / costoT) - 1) * 100), 4);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RentabilidadR.rdlc");
                
                ReportDataSource rds = new ReportDataSource("DatosRentabilidad", datos);
                ReportParameter param = new ReportParameter("ParamPrecio", decimal.Round(precioT, 4).ToString("N"));
                ReportParameter param2 = new ReportParameter("ParamCosto", decimal.Round(costoT, 4).ToString("N"));
                ReportParameter param3 = new ReportParameter("ParamGanancia", decimal.Round(ganancia, 4).ToString("N"));
                ReportParameter param4 = new ReportParameter("ParamPorcentaje", decimal.Round(porGanancia, 4).ToString("N") + "%");

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

                    String filename = string.Format("{0}.{1}", "Reporte_Rentabilidad", "xls");

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

        private void generarReporte3()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));

                var dt = this.contInformes.Reportes_Rentabilidad_CostosImponible(desde, hasta, this.idSuc, this.idCliente);
                dt.Columns[4].ColumnName = "Costo_Real";
                dt.Columns[5].ColumnName = "Costo_Imponible";
                dt.Columns[7].ColumnName = "Precio_Unitario";
                dt.Columns[9].ColumnName = "Porcentaje";
                dt.Columns[8].ColumnName = "Rentabilidad";

                string totalVendido = "0";
                string totalCosto = "0";
                string porcentajeRentabilidad = "0";
                string rentabilidad = "0";

                if (!string.IsNullOrEmpty(this.paramRentabilidadCosto))
                {
                    string[] listParametros = this.paramRentabilidadCosto.Split(';');
                    porcentajeRentabilidad = listParametros[0];
                    rentabilidad = listParametros[1];
                    totalCosto = listParametros[2];
                    totalVendido = listParametros[3];
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RentabilidadRCosto.rdlc");

                ReportParameter param1 = new ReportParameter("ParamPorRentabilidad", porcentajeRentabilidad.ToString());
                ReportParameter param2 = new ReportParameter("ParamRentabilidad", rentabilidad.ToString());
                ReportParameter param3 = new ReportParameter("ParamTotalCosto", totalCosto.ToString());
                ReportParameter param4 = new ReportParameter("ParamTotalVendido", totalVendido.ToString());
                ReportDataSource rds = new ReportDataSource("RentabilidadCosto", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
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

                    String filename = string.Format("{0}.{1}", "Rentabilidad_Costo", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir reporte de ventas. " + ex.Message));
            }
        }

        private void generarReporte4()
        {
            try
            {
                var dt = this.contInformes.Reportes_Rentabilidad_CostosImponibleByDesc(this.idSuc, this.nombreArticulo.ToString(), this.idCliente);
                dt.Columns[4].ColumnName = "Costo_Real";
                dt.Columns[5].ColumnName = "Costo_Imponible";
                dt.Columns[7].ColumnName = "Precio_Unitario";
                dt.Columns[9].ColumnName = "Porcentaje";
                dt.Columns[8].ColumnName = "Rentabilidad";

                string totalVendido = "0";
                string totalCosto = "0";
                string porcentajeRentabilidad = "0";
                string rentabilidad = "0";

                if (!string.IsNullOrEmpty(this.paramRentabilidadCosto))
                {
                    string[] listParametros = this.paramRentabilidadCosto.Split(';');
                    porcentajeRentabilidad = listParametros[0];
                    rentabilidad = listParametros[1];
                    totalCosto = listParametros[2];
                    totalVendido = listParametros[3];
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RentabilidadRCosto.rdlc");

                ReportParameter param1 = new ReportParameter("ParamPorRentabilidad", porcentajeRentabilidad.ToString());
                ReportParameter param2 = new ReportParameter("ParamRentabilidad", rentabilidad.ToString());
                ReportParameter param3 = new ReportParameter("ParamTotalCosto", totalCosto.ToString());
                ReportParameter param4 = new ReportParameter("ParamTotalVendido", totalVendido.ToString());
                ReportDataSource rds = new ReportDataSource("RentabilidadCosto", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
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

                    String filename = string.Format("{0}.{1}", "Rentabilidad_Costo", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir reporte de ventas. " + ex.Message));
            }
        }
    }
}