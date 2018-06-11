using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
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

namespace Gestion_Web.Formularios.Personal
{
    public partial class ReportesR : System.Web.UI.Page
    {
        controladorPagos contPagos = new controladorPagos();
        controladorRemuneraciones contRem = new controladorRemuneraciones();

        int idPago;
        int idRemuneracion;
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idPago = Convert.ToInt32(Request.QueryString["p"]);
                    this.idRemuneracion = Convert.ToInt32(Request.QueryString["r"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    if (this.accion == 1)
                    {
                        this.cargarInforme();
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
                controladorPagos contPagos = new controladorPagos();
                ControladorEmpresa contEmpresa = new ControladorEmpresa();                
                PagoRemuneracione p = contRem.obtenerPagoRemuneracionByID(this.idPago);


                DataTable dt = this.obtenerDatosPago(p);

                //obtengo remuneraciones
                var dtDocumentos = this.obtenerDocCancelados(p.Id);
                //Cheques propio
                var dtCheques = this.obtenerChequesPropios(p.Id);
                //var cheques terceros
                var dtChequesTer = this.obtenerChequesTerceros(p.Id);
                //Transferencias
                var dtTrans = this.obtenerTransferencias(p.Id);
                //detalle
                var dtDetalle = this.obtenerDetalle();                

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReciboPagoRemuneracionR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosPago", dt);
                ReportDataSource rds3 = new ReportDataSource("DSChequesPropios", dtCheques);
                ReportDataSource rds4 = new ReportDataSource("DSChequesTerceros", dtChequesTer);
                ReportDataSource rds5 = new ReportDataSource("DSTransferencias", dtTrans);
                ReportDataSource rds6 = new ReportDataSource("DSDetalle", dtDetalle); ;
                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);                
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.DataSources.Add(rds6);

                this.ReportViewer1.LocalReport.Refresh();

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
 
            }
        }

        private DataTable obtenerDocCancelados(long idPago)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Importe");

                var remuneracion = this.contPagos.obtenerRemuneracionesPago(idPago);
                var pagosCta = this.contPagos.obtenerPagoCuentasEmpleadosImputados(idPago);

                foreach (var r in remuneracion)
                {
                    dt.Rows.Add("Remuneracion Nro " + r.Numero, Convert.ToDecimal(r.Total).ToString("C"));
                }
                foreach (var p in pagosCta)
                {
                    dt.Rows.Add("Pago a Cta Nro " + p.Numero, Convert.ToDecimal(-p.Haber).ToString("C"));
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable obtenerDatosPago(PagoRemuneracione p)
        {
            try
            {
                controladorEmpleado contEmpleado = new controladorEmpleado();

                var dt = new DataTable();
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Recibo");
                dt.Columns.Add("Empresa");
                dt.Columns.Add("Empleado");
                dt.Columns.Add("Total");

                //obtengo empleado                
                Gestion_Api.Modelo.Empleado empleado = contEmpleado.obtenerEmpleadoID(Convert.ToInt32(p.Empleado.Value));
                string nombre = empleado.apellido + " " + empleado.nombre;

                ControladorEmpresa contEmpr = new ControladorEmpresa();
                var emp = contEmpr.obtenerEmpresa((int)p.Empresa);

                dt.Rows.Add(
                    Convert.ToDateTime(p.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy"),
                    p.Numero, 
                    emp.RazonSocial, 
                    nombre, 
                    Convert.ToDecimal(p.Total).ToString() 
                    );

                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
             
        private DataTable obtenerChequesPropios(long idPago)
        {
            try
            {
                var p = contRem.obtenerPagoRemuneracionByID(this.idPago);
               
                var formas = p.Pago_Remuneraciones.Where(x => x.TipoPago == 2).ToList();
                
                var dt = this.contPagos.obtenerChequesPropios(formas);
                
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable obtenerChequesTerceros(long idPago)
        {
            try
            {
                var p = contRem.obtenerPagoRemuneracionByID(this.idPago);


                var formas = p.Pago_Remuneraciones.Where(x => x.TipoPago == 7).ToList();

                var dt = this.contPagos.obtenerChequesTerceros(formas);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable obtenerTransferencias(long idPago)
        {
            try
            {
                var p = contRem.obtenerPagoRemuneracionByID(this.idPago);


                var formas = p.Pago_Remuneraciones.Where(x => x.TipoPago == 3).ToList();

                var dt = this.contPagos.obtenerTransferencias(formas);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable obtenerDetalle()
        {
            try
            {
                var dt = contPagos.obtenerDetallePagoRemuneracion(this.idPago);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}