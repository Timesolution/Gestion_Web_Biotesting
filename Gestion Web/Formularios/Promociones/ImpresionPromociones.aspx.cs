using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Promociones
{
    public partial class ImpresionPromociones : System.Web.UI.Page
    {
        controladorArticulo contArt = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorSucursal contSucu = new controladorSucursal();
        ControladorFormasPago contFP = new ControladorFormasPago();

        private int accion;
        private int vigentes;
        private int excel;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.vigentes = Convert.ToInt32(Request.QueryString["v"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                }

                if (accion == 1)
                {
                    this.generarReporte();//PromocionesR
                }

            }
            catch
            {

            }
        }
        private void generarReporte()
        {
            try
            {
                List<Promocione> promo = this.contArtEnt.obtenerPromociones();
                if (this.vigentes > 0)
                    promo = promo.Where(x => DateTime.Today >= x.Desde.Value && DateTime.Today <= x.Hasta).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Desde");
                dt.Columns.Add("Hasta");
                dt.Columns.Add("FormaPago");
                dt.Columns.Add("ListaPrecio");
                dt.Columns.Add("Empresa");
                dt.Columns.Add("Sucursal");
                dt.Columns.Add("Descuento");
                dt.Columns.Add("Precio");

                foreach (Promocione p in promo)
                {
                    DataRow row = dt.NewRow();
                    row["Id"] = p.Id.ToString();
                    row["Nombre"] = p.Promocion;
                    row["Desde"] = p.Desde.Value.ToString("dd/MM/yyyy");
                    row["Hasta"] = p.Hasta.Value.ToString("dd/MM/yyyy");

                    if (p.Empresa.Value > 0)
                        row["Empresa"] = this.contSucu.obtenerEmpresaID(p.Empresa.Value).RazonSocial;
                    else
                        row["Empresa"] = "TODAS";
                    //if (p.Sucursal.Value > 0)
                    //    row["Sucursal"] = this.contSucu.obtenerSucursalID(p.Sucursal.Value).nombre;
                    //else
                    //    row["Sucursal"] = "TODAS";
                    if (p.FormaPago.Value > 0)
                        row["FormaPago"] = this.contFP.obtenerFormaPagoEntByID(p.FormaPago.Value).forma;
                    else
                        row["FormaPago"] = "TODAS";
                    if (p.ListaPrecio.Value > 0)
                        row["ListaPrecio"] = this.contFP.obtenerListaPrecioEntByID(p.ListaPrecio.Value).nombre;
                    else
                        row["ListaPrecio"] = "TODAS";

                    row["Descuento"] = p.Descuento.Value.ToString();
                    row["Precio"] = p.PrecioFijo.Value.ToString();

                    dt.Rows.Add(row);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PromocionesR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosPromociones", dt);
                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
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

            }
        }
    }
}