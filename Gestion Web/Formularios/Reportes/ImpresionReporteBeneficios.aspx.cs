using Gestion_Api.Modelo;
using Microsoft.Reporting.WebForms;
using Millas_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ImpresionReporteBeneficios : System.Web.UI.Page
    {
        private int accion;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                accion = Convert.ToInt32(Request.QueryString["a"]);

                if (accion == 1)
                {
                    this.generarReporte();
                }
            }
        }

        private void generarReporte()
        {
            try
            {
                ControladorMillas controladorMillas = new ControladorMillas();

                var datosSocios = controladorMillas.ObtenerDatosSociosYMillasAHoy();

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("SocioBeneficiosR.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("SocioBeneficios", datosSocios);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Datos Socios", "xls");

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
                Log.EscribirSQL(1,"Error","Error al generar reporte de datos de socios " + ex.Message);
            }
        }
    }
}