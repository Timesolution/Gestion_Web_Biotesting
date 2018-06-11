using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
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

namespace Gestion_Web.Formularios.Pagos
{
    public partial class ImpresionPago : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private string fechaD;
        private string fechaH;
        private int valor;
        private int idPago;
        private int idProveedor;
        private int idSucursal;
        private int idEmpresa;
        private int idPuntoVenta;
        private string saldo;
        private int excel;
        controladorPagos contPagos = new controladorPagos();
        controladorCliente contCliente = new controladorCliente();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idPago = Convert.ToInt32(Request.QueryString["Pago"]);
                    this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                    this.fechaD = Request.QueryString["fd"];
                    this.fechaH = Request.QueryString["fh"];
                    this.idProveedor = Convert.ToInt32(Request.QueryString["prov"]);
                    this.idSucursal = Convert.ToInt32(Request.QueryString["suc"]);
                    this.idEmpresa = Convert.ToInt32(Request.QueryString["emp"]);
                    this.idPuntoVenta = Convert.ToInt32(Request.QueryString["ptoVta"]);
                    this.saldo = Request.QueryString["sd"];
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);

                    if (valor == 1)
                    {
                        this.generarReporte1(saldo);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pago. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pago. " + ex.Message);
            }

        }

        private void generarReporte1(string saldo)
        {
            try
            {
                List<PagosCompra> pagos = contPagos.buscarPagos(Convert.ToDateTime(fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(fechaH, new CultureInfo("es-AR")), idProveedor, idEmpresa, idSucursal, idPuntoVenta);
                DataTable dt = new DataTable();
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Numero");
                dt.Columns.Add("Importe", typeof(decimal));
                dt.Columns.Add("Proveedor");

                foreach (var p in pagos)
                {
                    Gestor_Solution.Modelo.Cliente c = new Gestor_Solution.Modelo.Cliente();
                    c = this.contCliente.obtenerProveedorID((int)p.Proveedor);
                    DataRow row = dt.NewRow();
                    row["Fecha"] = p.Fecha.Value.ToString("dd/MM/yyyy");
                    row["Numero"] = p.Numero;
                    row["Importe"] = p.Total;
                    row["Proveedor"] = c.razonSocial;
                    dt.Rows.Add(row);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PagosListado.rdlc");
                ReportDataSource rds = new ReportDataSource("dsDatosPagos", dt);
                ReportParameter param = new ReportParameter("ParamSaldo", saldo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;


                string[] streams;

                if(this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "DetallePagos", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);


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
            catch (Exception Ex)
            {

            }
        }
    }
}