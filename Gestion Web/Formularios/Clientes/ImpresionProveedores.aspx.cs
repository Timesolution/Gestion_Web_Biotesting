using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ImpresionProveedores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private string proveedores;
        private int excel;
        controladorCliente controlador = new controladorCliente();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.proveedores = Request.QueryString["proveedores"];
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.generarReporte(proveedores);
                    // this.generarReporte2(proveedores);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pedido. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pedido. " + ex.Message);
            }
        }

        private void generarReporte(string proveedores)
        {
            try
            {
                DataTable dtProveedores = new DataTable();
                if (proveedores != null && proveedores != "")
                {
                    dtProveedores = controlador.obtenerProveedoresAliasDT(proveedores);//.obtenerClientesAliasDT(proveedores);
                }
                else
                {
                    dtProveedores = controlador.obtenerProveedoresReducDT();//.obtenerClientesReducDT();
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeProveedoresR.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleProveedor", dtProveedores);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "ListadoProveedores", "xls");

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
        private void generarReporte2(string proveedores)
        {
            try
            {
                //string[] prov = proveedores.Split(';');
                //foreach(string p in prov)
                //{
                //    if(!String.IsNullOrEmpty(p))
                //    {
                        DataTable dtDetalle = controlador.obtenerDetalleCliente(0);
                        DataTable dtDirecciones = controlador.obtenerDireccionCliente(0,2);
                        DataTable dtContactos = controlador.obtenerContactoCliente(0,2);
                        this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeProveedores.rdlc");
                        ReportDataSource rds = new ReportDataSource("DetalleProveedor", dtDetalle);
                        ReportDataSource rds2 = new ReportDataSource("ContactoProveedor", dtContactos);
                        ReportDataSource rds3 = new ReportDataSource("DireccionProveedor", dtDirecciones);
                        this.ReportViewer1.LocalReport.DataSources.Clear();
                        this.ReportViewer1.LocalReport.DataSources.Add(rds);
                        this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                        this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                        this.ReportViewer1.LocalReport.Refresh();
                //    }
                //}

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
            catch
            {

            }
        }
    }
}