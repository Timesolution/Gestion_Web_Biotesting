using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Vendedores.Comisiones
{
    public partial class ImpresionComisiones : System.Web.UI.Page
    {
        private int _accion;
        private int _excel;
        private int _idEmpresa;
        private int _idSucursal;
        private int _idPuntoVenta;
        private int _vendedor;
        private string _totalNeto;
        private string _totalComision;
        private string _fechaDesde;
        private string _fechaHasta;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _accion = Convert.ToInt32(Request.QueryString["a"]);
                _excel = Convert.ToInt32(Request.QueryString["ex"]);
                _fechaDesde = Request.QueryString["fd"];
                _fechaHasta = Request.QueryString["fh"];
                _idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                _idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                _idPuntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                _vendedor = Convert.ToInt32(Request.QueryString["v"]);
                _totalNeto = Request.QueryString["tn"];
                _totalComision = Request.QueryString["tc"];

                if (!IsPostBack)
                {
                    if (_accion == 1)
                    {
                        GenerarReporteComisionesPorVendedor();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }            
        }

        void GenerarReporteComisionesPorVendedor()
        {
            try
            {
                controladorVendedor controladorVendedor = new controladorVendedor();

                DateTime fechaDesde = Convert.ToDateTime(_fechaDesde, CultureInfo.InvariantCulture);
                DateTime fechaHasta = Convert.ToDateTime(_fechaHasta, CultureInfo.InvariantCulture);

                DataTable dtComisiones = controladorVendedor.ObtenerVentasPorComisionByGrupo(fechaDesde, fechaHasta, _idEmpresa, _idSucursal, _idPuntoVenta, _vendedor);

                controladorCliente cont = new controladorCliente();

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComisionesPorGrupo.rdlc");

                ReportDataSource rds = new ReportDataSource("ComisionesVendedores", dtComisiones);
                ReportParameter param1 = new ReportParameter("TotalNeto", _totalNeto);
                ReportParameter param2 = new ReportParameter("TotalComision", _totalComision);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this._excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Compras", "xls");

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
                Log.EscribirSQL(1,"Error","Error al generar reporte de comisiones por vendedor " + ex.Message);
            }
        }
    }
}