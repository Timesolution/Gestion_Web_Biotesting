using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Surtidores
{
    public partial class ImpresionSurtidores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();

        private int surtidor;
        private string fechaD;
        private string fechaH;
        private int accion;
        private int excel;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.fechaD = Request.QueryString["FD"];
                    this.fechaH = Request.QueryString["FH"];
                    this.surtidor = Convert.ToInt32(Request.QueryString["s"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);

                    if (accion == 1)
                    {
                        this.generarReporte();
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        private void generarReporte()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<Surtidores_Cierre> cierres = this.controlador.obtenerCierresSurtidor(desde, hasta, this.surtidor);

                decimal totalLitro = 0;
                decimal totalPesos = 0;

                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("Id");
                dtDatos.Columns.Add("Fecha", typeof(DateTime));
                dtDatos.Columns.Add("Surtidor");
                dtDatos.Columns.Add("CantidadInicial", typeof(decimal));
                dtDatos.Columns.Add("CantidadCierre", typeof(decimal));
                dtDatos.Columns.Add("Usuarios");
                dtDatos.Columns.Add("Observaciones");
                dtDatos.Columns.Add("PrecioVenta", typeof(decimal));

                foreach (var c in cierres)
                {
                    DataRow row = dtDatos.NewRow();
                    row["Id"] = c.Id.ToString();
                    row["Fecha"] = c.Fecha;
                    row["Surtidor"] = c.Surtidore.Descripcion;
                    row["CantidadInicial"] = c.CantidadInicial.Value;
                    row["CantidadCierre"] = c.CantidadCierre.Value;
                    row["Observaciones"] = c.Observaciones;
                    row["PrecioVenta"] = c.PrecioVenta.Value;
                    Usuario user = this.contUser.obtenerUsuariosID(c.IdUsuario.Value);
                    row["Usuarios"] = user.usuario;

                    totalLitro += (c.CantidadCierre.Value - c.CantidadInicial.Value);
                    totalPesos += (c.CantidadCierre.Value - c.CantidadInicial.Value) * c.PrecioVenta.Value;

                    dtDatos.Rows.Add(row);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("SurtidoresR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCierres", dtDatos);
                ReportParameter param = new ReportParameter("ParamTotalLitros", totalLitro.ToString("C"));
                ReportParameter param2 = new ReportParameter("ParamTotalPesos", totalPesos.ToString("C"));

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

                    String filename = string.Format("{0}.{1}", "Valores_Cheques", "xls");

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

            }
        }
    }
}