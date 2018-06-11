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

namespace Gestion_Web.Formularios.Pagos
{
    public partial class ReportesR : System.Web.UI.Page
    {
        controladorPagos contPagos = new controladorPagos();
        long idPago;
        int accion;
        int excel;
        string listaPagos;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idPago = Convert.ToInt64(Request.QueryString["id"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.listaPagos = Request.QueryString["lp"];

                    if (!string.IsNullOrEmpty(this.listaPagos))
                        this.listaPagos = this.listaPagos.Remove(this.listaPagos.Length - 1);

                    if (accion == 0)
                    {
                        this.cargarInforme();
                    }
                    if (accion == 1) //Informe Detalle de Pagos
                    {
                        this.generarReporte1(listaPagos);
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
                var p = contPagos.obtenerPagoById(this.idPago);
                //var d =  DSPagos("Encabezado");
                
                //obtengo datos
                var dt = this.obtenerDatosCompra(p);

                //obtengo compras
                var dtDocumentos = this.obtenerDocCancelados(p.Id);
                //Cheques propio
                var dtCheques = this.obtenerChequesPropios(p.Id);
                //var cheques terceros
                var dtChequesTer = this.obtenerChequesTerceros(p.Id);
                //Transferencias
                var dtTrans = this.obtenerTransferencias(p.Id);
                //detalle
                var dtDetalle = this.obtenerDetalle();

                string observacion = "";
                if (p.PagosCompras_Observaciones != null)
                    observacion = p.PagosCompras_Observaciones.Observaciones;

                //if (dtDocumentos != null && dtDocumentos.Rows.Count > 0)
                //{
                //    foreach (DataRow drDoc in dtDocumentos.Rows)
                //    {
                //        if (drDoc["Tipo"].ToString().ToLower().Contains("pago a cta"))
                //        {
                //            DataRow drDetalle = dtDetalle.NewRow();
                //            drDetalle["Tipo"] = drDoc["Tipo"];
                //            decimal montoPagoCta = Convert.ToDecimal(drDoc["Importe"]) * -1;
                //            drDetalle["Importe"] = montoPagoCta.ToString();
                //            dtDetalle.Rows.Add(drDetalle);
                //        }
                //    }
                //}


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReciboPagos.rdlc");
                ReportDataSource rds = new ReportDataSource("DSEncabezado", dt);
                ReportDataSource rds2 = new ReportDataSource("DSDocumentos", dtDocumentos);
                ReportDataSource rds3 = new ReportDataSource("DSChequesPropios", dtCheques);
                ReportDataSource rds4 = new ReportDataSource("DSChequesTerceros", dtChequesTer);
                ReportDataSource rds5 = new ReportDataSource("DSTransferencias", dtTrans);
                ReportDataSource rds6 = new ReportDataSource("DSDetalle", dtDetalle);

                ReportParameter param = new ReportParameter("ParamObservacion", observacion);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.DataSources.Add(rds6);

                this.ReportViewer1.LocalReport.SetParameters(param);

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
        private void generarReporte1(string listaPagos)
        {
            try
            {
                DataTable dt = this.generarDetallePagosDT(listaPagos);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetallePagos.rdlc");

                ReportDataSource rds = new ReportDataSource("DetallePagos", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get pdf content

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "DetallePagos", "xls");

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
        private DataTable obtenerDatosCompra(PagosCompra p)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Recibo");
                dt.Columns.Add("Empresa");
                dt.Columns.Add("Proveedor");
                dt.Columns.Add("Total");

                //obtengo proveedor
                controladorCliente cont = new controladorCliente();
                var prov = cont.obtenerProveedorID((int)p.Proveedor);

                ControladorEmpresa contEmpr = new ControladorEmpresa();
                var emp = contEmpr.obtenerEmpresa((int)p.Empresa);

                dt.Rows.Add(Convert.ToDateTime(p.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy"), 
                    p.Numero , emp.RazonSocial, prov.razonSocial, 
                    Convert.ToDecimal(p.Total).ToString("C") );

                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        private DataTable obtenerDocCancelados(long idPago)
        {
            try
            {
                ControladorCCProveedor contCCProveedor = new ControladorCCProveedor();
                var dt = new DataTable();
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Importe",typeof(decimal));                              
                dt.Columns.Add("Comentario");
                dt.Columns.Add("Saldo", typeof(decimal));  
                dt.Columns.Add("Imputacion", typeof(decimal));

                List<Gestion_Api.Entitys.Compra> compras = this.contPagos.obtenerComprasDePago(idPago);
                var pagosCta = this.contPagos.obtenerPagoCuentasImputados(idPago);
                var pg = this.contPagos.obtenerPagoById(idPago);

                foreach (var c in compras)
                {
                    string comentario = "";
                    decimal saldoRestante = 0;
                    decimal valorImputacion = 0;
                    if (c.Compras_Observaciones.Count > 0)
                    {
                        comentario = c.Compras_Observaciones.FirstOrDefault().Observacion;
                    }
                    if (c.MovimientosCCPs != null)
                    {
                        if (c.MovimientosCCPs.Count > 0)
                        {
                            saldoRestante = c.MovimientosCCPs.FirstOrDefault().Saldo.Value;
                            
                            var imp = pg.ImputacionesCompras.Where(x => x.Movimiento == c.MovimientosCCPs.FirstOrDefault().Id).FirstOrDefault();
                            if (imp != null)
                                valorImputacion = imp.Imputar.Value;
                        }
                    }

                    dt.Rows.Add(c.TipoDocumento + " Nro " + c.Numero, Convert.ToDecimal(c.Total), comentario,saldoRestante, valorImputacion);
                }
                foreach (var p in pagosCta)
                {
                    
                    decimal saldoImputar = 0;
                    if (pg != null)
                    {
                        var imputacion = pg.ImputacionesCompras.Where(x => x.Movimiento == p.Id).FirstOrDefault();
                        if (imputacion != null)
                        {
                            saldoImputar = (decimal)imputacion.Imputar;
                        }
                            
                    }

                    dt.Rows.Add("Pago a Cta Nro " + p.Numero, Convert.ToDecimal(-p.Haber),"", 0,saldoImputar);
                }

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
                var p = contPagos.obtenerPagoById(this.idPago);
               
                var formas = p.Pagos_Compras.Where(x => x.TipoPago == 2).ToList();

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
                var p = contPagos.obtenerPagoById(this.idPago);
                
                
                var formas = p.Pagos_Compras.Where(x => x.TipoPago == 7).ToList();

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
                var p = contPagos.obtenerPagoById(this.idPago);
               
                
                var formas = p.Pagos_Compras.Where(x => x.TipoPago == 3).ToList();

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
                var dt = contPagos.obtenerDetallePago(this.idPago);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private DataTable generarDetallePagosDT(string listaPagos)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdPago");
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Numero");
                dt.Columns.Add("Proveedor");
                dt.Columns.Add("Efectivo" ,typeof(Decimal));
                dt.Columns.Add("Cheques", typeof(Decimal));
                dt.Columns.Add("Transferencias", typeof(Decimal));
                dt.Columns.Add("Tarjetas", typeof(Decimal));
                dt.Columns.Add("Retenciones", typeof(Decimal));
                dt.Columns.Add("Total", typeof(Decimal));

               

                string[] pagos = listaPagos.Split(';');
                foreach (string pag in pagos)
                {
                    DataRow row = dt.NewRow();
                    Decimal efectivo = 0.00m;
                    Decimal cheques = 0.00m;
                    Decimal transferencias = 0.00m;
                    Decimal retenciones = 0.00m;
                    Decimal tarjetas = 0.00m;

                    var p = contPagos.obtenerPagoById(Convert.ToInt64(pag));
                    var dtDatosCompra = this.obtenerDatosCompra(p);
                    var dtDetalleCompra = this.contPagos.obtenerDetallePago(p.Id);
                    DateTime fecha = Convert.ToDateTime(dtDatosCompra.Rows[0][0], new CultureInfo("es-AR"));

                    row["IdPago"] = pag;
                    row["Fecha"] = fecha.ToString("dd/MM/yyyy");
                    row["Numero"] = dtDatosCompra.Rows[0][1].ToString();
                    row["Proveedor"] = dtDatosCompra.Rows[0][3].ToString();
                    row["Total"] = Convert.ToDecimal(dtDatosCompra.Rows[0][4].ToString().Replace("$",""));

                    foreach (DataRow dr in dtDetalleCompra.Rows)
                    {
                        if (dr["Tipo"].ToString() == "Pesos" || dr["Tipo"].ToString() == "Dolar" || dr["Tipo"].ToString() == "Euro")
                        {
                            efectivo += Convert.ToDecimal(dr["Importe"]);
                            row["Efectivo"] = efectivo;
                        }
                        if (dr["Tipo"].ToString().ToLower().Contains("tarjeta"))
                        {
                            tarjetas += Convert.ToDecimal(dr["Importe"]);
                            row["Tarjetas"] = tarjetas;
                        }
                        if (dr["Tipo"].ToString().ToLower().Contains("cheque"))
                        {
                            cheques += Convert.ToDecimal(dr["Importe"]);
                            row["Cheques"] = cheques;
                        }
                        if (dr["Tipo"].ToString().ToLower().Contains("retencion"))
                        {
                            retenciones += Convert.ToDecimal(dr["Importe"]);
                            row["Retenciones"] = retenciones;
                        }
                        if (dr["Tipo"].ToString().ToLower().Contains("transferencia"))
                        {
                            transferencias += Convert.ToDecimal(dr["Importe"]);
                            row["Transferencias"] = transferencias;
                        }
                    }

                    dt.Rows.Add(row);
                }

                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "Fecha desc";
                    dt = dv.ToTable();
                }

                return dt;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
    }
}