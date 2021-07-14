using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ImpresionFacturas : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int idPresupuesto;
        controladorFacturacion controlador = new controladorFacturacion();
        controladorListaPrecio contLista = new controladorListaPrecio();

        private string fechaD;
        private string fechaH;
        private int tipo;
        private int tipofact;
        private int suc;
        private int cliente;
        private int excel;
        private int accion;
        private int lista;
        private int sucOrigen;
        private int sucDestino;
        private int anuladas;
        private int emp;
        private string listasP;
        private int formaPago;
        private int vendedor;
        private int formaIVA;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    fechaD = Request.QueryString["Fechadesde"];
                    fechaH = Request.QueryString["FechaHasta"];
                    suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                    tipo = Convert.ToInt32(Request.QueryString["tipo"]);
                    tipofact = Convert.ToInt32(Request.QueryString["doc"]);
                    cliente = Convert.ToInt32(Request.QueryString["cl"]);
                    excel = Convert.ToInt32(Request.QueryString["e"]);
                    accion = Convert.ToInt32(Request.QueryString["a"]);
                    lista = Convert.ToInt32(Request.QueryString["ls"]);
                    sucOrigen = Convert.ToInt32(Request.QueryString["Origen"]);
                    sucDestino = Convert.ToInt32(Request.QueryString["Destino"]);
                    anuladas = Convert.ToInt32(Request.QueryString["anuladas"]);
                    emp = Convert.ToInt32(Request.QueryString["Emp"]);
                    listasP = Request.QueryString["l"];
                    formaPago = Convert.ToInt32(Request.QueryString["fp"]);
                    vendedor = Convert.ToInt32(Request.QueryString["vend"]);
                    formaIVA = Convert.ToInt32(Request.QueryString["fo"]);

                    if (accion == 1)
                    {
                        this.generarReporte2(fechaD, fechaH, suc, tipo, cliente);//REPORTE IVA VENTAS
                    }
                    if (accion == 2)
                    {
                        this.generarReporte3(fechaD, fechaH, suc, tipo, cliente);//REPORTE IIBB
                    }
                    if (accion == 3)
                    {
                        this.generarReporte4(fechaD, fechaH, suc, tipo, cliente);//REPORTE VENTAS 2
                    }
                    if (accion == 4)
                    {
                        this.generarReporte5(fechaD, fechaH, suc, tipo, cliente);//REPORTE DETALLES VENTAS
                    }
                    if (accion == 5)
                    {
                        this.generarReporte6(fechaD, fechaH);//REPORTE TOTAL VENTAS X SUCURSAL
                    }
                    if (accion == 6)
                    {
                        this.generarReporte7(fechaD, fechaH, suc, tipo, cliente, lista);//REPORTE POR LISTA PRECIO
                    }
                    if (accion == 7)
                    {
                        this.generarReporte8();//CITI VENTAS                        
                    }
                    if (accion == 8)
                    {
                        this.generarReporte9();//Retenciones ARBA
                    }
                    if (accion == 9)
                    {
                        this.generarReporte10(fechaD, fechaH, sucOrigen, sucDestino);//Impresion vtas E/Sucursales
                    }
                    if (accion == 10)
                    {
                        this.generarReporte11(fechaD, fechaH, suc, tipo, cliente);//REPORTE DETALLES VENTAS x Vendedor
                    }
                    if (accion == 11)
                    {
                        this.generarReporte12(fechaD, fechaH, suc, tipo, cliente);//Detalle descuentos x sucursal (parodi)
                    }
                    if (accion == 12)
                    {
                        this.generarReporte13(fechaD, fechaH, suc, tipo, cliente);//Impresion busqueda actual
                    }
                    if (accion == 13)
                    {
                        this.generarReporte14(fechaD, fechaH, suc, tipo, cliente);//Impresion busqueda actual
                    }
                    if (accion == 14)
                    {
                        this.generarReporte15(fechaD, fechaH, suc, tipo, cliente);//Detalle ventas con solicitudes
                    }
                    if (accion == 15)
                    {
                        this.generarReporte16(fechaD, fechaH, suc, tipo, cliente);//Detalle ventas de presupuestos facturados
                    }
                    if (accion == 16)
                    {
                        this.generarReporte17(fechaD, fechaH, suc, tipofact, cliente);//Detalle ventas de presupuestos facturados
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Presupuesto. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Presupuesto. " + ex.Message);
            }
        }
        private void generarReporte2(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                DataTable dtDetalles = this.controlador.obtenerFacturasRangoTipoDTLista(fechaD, fechaH, suc, tipo, cliente, tipofact, this.lista, this.anuladas, this.emp, this.vendedor, this.formaPago);
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);
                DataTable dtFechas = this.controlador.obtenerFechasFactura(fechaD, fechaH);

                Decimal total = 0;

                if (dtDetalles.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDetalles.Rows)
                    {
                        if (formaIVA == 1)
                        {
                            string tipoF = "";
                            string LetraF = "";
                            if (row["tipo"].ToString().Contains("Factura"))
                            {
                                tipoF = "Fc";
                                LetraF = row["tipo"].ToString().Substring(row["tipo"].ToString().Length - 1, 1);
                            }
                            if (row["tipo"].ToString().Contains("Credito"))
                            {
                                tipoF = "Cr";
                                LetraF = row["tipo"].ToString().Substring(row["tipo"].ToString().Length - 1, 1);
                            }
                            if (row["tipo"].ToString().Contains("Debito"))
                            {
                                tipoF = "De";
                                LetraF = row["tipo"].ToString().Substring(row["tipo"].ToString().Length - 1, 1);
                            }

                            string comprobante = tipoF + " " + LetraF + row["numero"].ToString().Replace("-", "");
                            row["numero"] = comprobante;

                            string clienteR = row["razonSocial"].ToString();
                            if (clienteR.Length > 26)
                            {
                                clienteR = clienteR.Substring(0, 26);
                            }
                            row["razonSocial"] = clienteR;
                        }

                        //row["fecha"] = row["fechaFormateada"];
                        if (row["Tipo"].ToString().Contains("Credito"))
                        {
                            row["Total"] = Convert.ToDecimal(row["Total"].ToString()) * -1;
                            row["neto21"] = Convert.ToDecimal(row["neto21"].ToString()) * -1;
                            row["subtotal"] = Convert.ToDecimal(row["subtotal"].ToString()) * -1;
                            row["retenciones"] = Convert.ToDecimal(row["retenciones"].ToString()) * -1;
                            row["netoNoGrabado"] = Convert.ToDecimal(row["netoNoGrabado"].ToString()) * -1;

                            row["TotalIva105"] = Convert.ToDecimal(row["TotalIva105"].ToString()) * -1;
                            row["TotalIva21"] = Convert.ToDecimal(row["TotalIva21"].ToString()) * -1;
                            row["TotalIva27"] = Convert.ToDecimal(row["TotalIva27"].ToString()) * -1;
                            row["TotalNeto0"] = Convert.ToDecimal(row["TotalNeto0"].ToString()) * -1;
                            row["TotalNeto105"] = Convert.ToDecimal(row["TotalNeto105"].ToString()) * -1;
                            row["TotalNeto21"] = Convert.ToDecimal(row["TotalNeto21"].ToString()) * -1;
                            row["TotalNeto27"] = Convert.ToDecimal(row["TotalNeto27"].ToString()) * -1;
                        }
                        //si esta anulada la pongo en cero para que no sume
                        if (row["estado"].ToString() == "0")
                        {
                            row["Total"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["retenciones"] = Convert.ToDecimal(0);
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                        }

                        total += Convert.ToDecimal(row["Total"].ToString());
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Factura.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("FechasFactura", dtFechas);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);

                ReportParameter param = new ReportParameter("ParamTotal", total.ToString("C"));
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Ventas", "xls");

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
                Log.EscribirSQL(1, "Error", "Error al generar informe de iva ventas. " + ex.Message);
            }
        }
        private void generarReporte3(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                DataTable dtDetalles = this.controlador.obtenerIngresosBrutosByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);
                //Agregamos la columna TipoDistribucion al DataTable
                dtDetalles.Columns.Add("TipoDistribucion");
                //Recorremos el DataTable y por cada Articulo, le agregamos el Tipo de Distribucion que tiene la marca del Articulo
                foreach (DataRow dr in dtDetalles.Rows)
                {
                    var a = contArtEnt.obtenerArticuloEntityByCod(dr["codigo"].ToString());
                    if (a != null)
                    {
                        var m = contArtEnt.obtenerMarcaByArticulo(a.id);
                        if (m != null)
                        {
                            if (m.TipoDistribucion == 1)
                                dr["TipoDistribucion"] = "Distribucion";
                            if (m.TipoDistribucion == 2)
                                dr["TipoDistribucion"] = "Fabricacion";
                        }
                    }



                }

                Decimal total = 0;

                if (dtDetalles.Rows.Count > 0)
                {
                    //foreach (DataRow row in dtDetalles.Rows)
                    //{
                    //    if (row["tipo"].ToString().Contains("Credito"))
                    //    {
                    //        row["cantidad"] = Convert.ToDecimal(row["cantidad"].ToString()) * -1;
                    //        row["iva"] = Convert.ToDecimal(row["iva"].ToString()) * -1;
                    //        row["neto"] = Convert.ToDecimal(row["neto"].ToString()) * -1;
                    //        row["pSinIva"] = Convert.ToDecimal(row["pSinIva"].ToString()) * -1;

                    //    }
                    //    //si esta anulada la pongo en cero para que no sume
                    //    if (row["estado"].ToString() == "0")
                    //    {
                    //        row["iva"] = Convert.ToDecimal(0);
                    //        row["neto"] = Convert.ToDecimal(0);
                    //        row["pSinIva"] = Convert.ToDecimal(0);
                    //    }

                    //    //total += Convert.ToDecimal(row["Total"].ToString());
                    //}
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("IngresosBrutosR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFacturas", dtDatos);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_IIBB", "xls");

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
        private void generarReporte4(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                //DataTable dtDetalles = this.controlador.obtenerFacturasRangoTipoDT(fechaD, fechaH, suc, tipo, cliente, tipofact);
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);
                DataTable dtFechas = this.controlador.obtenerFechasFactura(fechaD, fechaH);

                DataTable dtFacturasA = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 1, this.anuladas, this.emp);
                DataTable dtFacturasB = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 2, this.anuladas, this.emp);
                DataTable dtFacturasE = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 24, this.anuladas, this.emp);
                DataTable dtFacturasNC = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 9, this.anuladas, this.emp);
                DataTable dtFacturasND = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 4, this.anuladas, this.emp);
                DataTable dtFacturasPresupuesto = new DataTable();

                dtFacturasNC.Merge(this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 8, this.anuladas, this.emp), true);
                dtFacturasND.Merge(this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, 1, cliente, 5, this.anuladas, this.emp), true);

                //NoGravado
                dtFacturasA.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0                
                dtFacturasB.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0
                dtFacturasE.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0
                dtFacturasNC.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0
                dtFacturasND.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0
                //dtFacturasPresupuesto.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0

                if (dtFacturasA.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasA.Rows)
                    {
                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["iva21"];
                        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }
                //if (dtFacturasPresupuesto.Rows.Count > 0)
                //{
                //    foreach (DataRow row in dtFacturasPresupuesto.Rows)
                //    {
                //        //Al Neto le resto el NoGravado y los muestro por separado.
                //        row["noGravado"] = row["iva21"];
                //        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                //        if (row["estado"].ToString() == "0")
                //        {
                //            row["netoNoGrabado"] = Convert.ToDecimal(0);
                //            row["neto21"] = Convert.ToDecimal(0);
                //            row["subtotal"] = Convert.ToDecimal(0);
                //            row["total"] = Convert.ToDecimal(0);
                //        }
                //    }
                //}

                if (dtFacturasB.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasB.Rows)
                    {
                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["iva21"];
                        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }

                if (dtFacturasE.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasE.Rows)
                    {
                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["subtotal"];
                        row["subtotal"] = 0;

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }

                if (dtFacturasND.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasND.Rows)
                    {
                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["iva21"];
                        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }

                if (dtFacturasNC.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasNC.Rows)
                    {
                        if (row["Tipo"].ToString().Contains("Credito"))
                        {
                            row["Total"] = Convert.ToDecimal(row["Total"].ToString()) * -1;
                            row["neto21"] = Convert.ToDecimal(row["neto21"].ToString()) * -1;
                            row["subtotal"] = Convert.ToDecimal(row["subtotal"].ToString()) * -1;
                            row["retenciones"] = Convert.ToDecimal(row["retenciones"].ToString()) * -1;
                            row["netoNoGrabado"] = Convert.ToDecimal(row["netoNoGrabado"].ToString()) * -1;
                            row["iva21"] = Convert.ToDecimal(row["iva21"].ToString()) * -1;
                        }

                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["iva21"];
                        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasR.rdlc");
                ReportDataSource rdsA = new ReportDataSource("DetalleFacturasA", dtFacturasA);
                ReportDataSource rdsB = new ReportDataSource("DetalleFacturasB", dtFacturasB);
                ReportDataSource rdsE = new ReportDataSource("DetalleFacturasE", dtFacturasE);
                ReportDataSource rdsNC = new ReportDataSource("DetalleFacturasNC", dtFacturasNC);
                ReportDataSource rdsND = new ReportDataSource("DetalleFacturasND", dtFacturasND);
                ReportDataSource rdspPRP = new ReportDataSource("DetallePresupuesto", dtFacturasPresupuesto);
                ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("FechasFactura", dtFechas);
                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rdsA);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsB);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsE);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsNC);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsND);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rdspPRP);

                //ReportParameter param = new ReportParameter("ParamTotal", total.ToString("C"));
                //this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Ventas_2", "xls");

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
        private void generarReporte5(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                DataTable dtDetalles = new DataTable();

                if (tipo > 0)
                {
                    if (tipo == 1)
                    {
                        dtDetalles = this.controlador.obtenerIngresosBrutosByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }
                    else
                    {
                        dtDetalles = this.controlador.obtenerDetalleVentasPresupuestoByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }

                }
                else
                {
                    dtDetalles = this.controlador.obtenerDetalleVentasByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                }

                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);

                Decimal total = 0;

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetallesVentasR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFacturas", dtDatos);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Detalle_Ventas", "xls");

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
        private void generarReporte6(string fechaD, string fechaH)
        {
            try
            {
                DataTable dtDetalles = new DataTable();
                dtDetalles = this.controlador.obtenerTotalVentasSucursalesByFecha(fechaD, fechaH);
                Decimal total = 0;

                if (dtDetalles.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDetalles.Rows)
                    {
                        total += Convert.ToDecimal(row["Total"].ToString());
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("TotalVentasR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleSucursales", dtDetalles);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_TotalSucursales", "xls");

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
        private void generarReporte7(string fechaD, string fechaH, int suc, int tipoF, int cliente, int listaP)
        {
            try
            {
                controladorSucursal contSuc = new controladorSucursal();
                Sucursal sucursal = contSuc.obtenerSucursalID(suc);

                DataTable dtDetalles = controlador.obtenerFacturasRangoTipoDTLista(fechaD, fechaH, suc, tipo, cliente, tipofact, listaP, this.anuladas, this.emp, 0, this.formaPago);
                dtDetalles.Columns.Add("Lista");

                Decimal total = 0;

                if (dtDetalles.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDetalles.Rows)
                    {
                        Gestor_Solution.Modelo.listaPrecio listPrecio = this.contLista.obtenerlistaPrecioID(Convert.ToInt32(row["id_lista"]));
                        row["Lista"] = listPrecio.nombre;
                        total += Convert.ToDecimal(row["Total"].ToString());

                        if (row["Tipo"].ToString().Contains("Credito"))
                        {
                            row["NetoNoGrabado"] = Convert.ToDecimal(row["NetoNoGrabado"].ToString()) * -1;
                            row["iva10"] = Convert.ToDecimal(row["iva10"].ToString()) * -1;
                            row["iva21"] = Convert.ToDecimal(row["iva21"].ToString()) * -1;
                            row["neto21"] = Convert.ToDecimal(row["neto21"].ToString()) * -1;
                            row["descuento"] = Convert.ToDecimal(row["descuento"].ToString()) * -1;
                            row["subtotal"] = Convert.ToDecimal(row["subtotal"].ToString()) * -1;
                            row["Retenciones"] = Convert.ToDecimal(row["Retenciones"].ToString()) * -1;
                            row["Total"] = Convert.ToDecimal(row["Total"].ToString()) * -1;
                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasListasR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Venta_x_ListaPrecio", "xls");

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
        private void generarReporte8()
        {
            try
            {
                controladorReportes contReport = new controladorReportes();

                String rutaTxt = Server.MapPath("CitiVentas.txt");
                int comprobante = contReport.generarCitiVenta(fechaD, fechaH, suc, rutaTxt);

                if (comprobante > 0)
                {

                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(rutaTxt, System.IO.FileMode.Open);

                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/octet-stream";
                    //this.Response.AddHeader("content-length", comprobante.Length.ToString());
                    this.Response.AddHeader("Content-disposition", "attachment; filename= " + "CitiVentas_" + fechaH + ".txt");
                    this.Response.BinaryWrite(btFile);

                    this.Response.End();
                }

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

                String rutaTxt = Server.MapPath("Percepcion_ARBA.txt");
                string comprobante = contReport.generarPercepcionesVentas(fechaD, fechaH, suc, rutaTxt);

                if (comprobante != null)
                {

                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(rutaTxt, System.IO.FileMode.Open);

                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/octet-stream";
                    //this.Response.AddHeader("content-length", comprobante.Length.ToString());
                    this.Response.AddHeader("Content-disposition", "attachment; filename= " + "Percepcion_" + fechaH + ".txt");
                    this.Response.BinaryWrite(btFile);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void generarReporte10(string fechaD, string fechaH, int idSucOrigen, int idSucDestino)
        {
            try
            {
                DataTable dtDetalles = new DataTable();
                dtDetalles = this.controlador.obtenerDetalleVentasEntreSucursalesByFecha(fechaD, fechaH, idSucOrigen, idSucDestino);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasSucursalesR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleVentasSucursales", dtDetalles);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);

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

                    String filename = string.Format("{0}.{1}", "Reporte_Detalle_Ventas", "xls");

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
        private void generarReporte11(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                controladorVendedor contVendedores = new controladorVendedor();
                DataTable dtDetalles = new DataTable();
                if (tipo > 0)
                {
                    if (tipo == 1)
                    {
                        dtDetalles = this.controlador.obtenerIngresosBrutosByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }
                    else
                    {
                        dtDetalles = this.controlador.obtenerDetalleVentasPresupuestoByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }

                }
                else
                {
                    dtDetalles = this.controlador.obtenerDetalleVentasByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                }
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);

                Decimal total = 0;

                //dtDetalles.Columns.Add("Vendedor");

                //if (dtDetalles.Rows.Count > 0)
                //{
                //    foreach (DataRow row in dtDetalles.Rows)
                //    {
                //        //Vendedor v = contVendedores.obtenerVendedorID(Convert.ToInt32(row["IdVendedor"]));
                //        //row["Vendedor"] = v.emp.nombre + " " + v.emp.apellido;

                //        if (row["tipo"].ToString().Contains("Credito"))
                //        {
                //            row["iva"] = Convert.ToDecimal(row["iva"].ToString()) * -1;
                //            row["neto"] = Convert.ToDecimal(row["neto"].ToString()) * -1;
                //            row["pSinIva"] = Convert.ToDecimal(row["pSinIva"].ToString()) * -1;
                //        }
                //        //total += Convert.ToDecimal(row["Total"].ToString());
                //    }
                //}

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleVentasVendedores.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFacturas", dtDatos);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Detalle_Ventas_Vendedor", "xls");

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
        private void generarReporte12(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                DataTable dtDetalles = new DataTable();
                string listas = this.listasP.Remove(this.listasP.Length - 1);
                dtDetalles = this.controlador.obtenerDetalleDescuentosVentas(fechaD, fechaH, suc, this.emp, 0, listas);

                if (dtDetalles.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDetalles.Rows)
                    {
                        if (row["tipo"].ToString().Contains("Credito"))
                        {
                            row["iva"] = Convert.ToDecimal(row["iva"].ToString()) * -1;
                            row["neto"] = Convert.ToDecimal(row["neto"].ToString()) * -1;
                            row["pSinIva"] = Convert.ToDecimal(row["pSinIva"].ToString()) * -1;

                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DescuentosR.rdlc");
                ReportDataSource rds = new ReportDataSource("DatosDescuentos", dtDetalles);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);

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

                    String filename = string.Format("{0}.{1}", "Reporte_Descuentos_Ventas", "xls");

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
        private void generarReporte13(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                DataTable dtDetalles = this.controlador.obtenerFacturasRangoTipoDTLista(fechaD, fechaH, suc, tipo, cliente, tipofact, this.lista, this.anuladas, this.emp, this.vendedor, this.formaPago);
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);
                DataTable dtFechas = this.controlador.obtenerFechasFactura(fechaD, fechaH);

                Decimal total = 0;

                if (dtDetalles.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDetalles.Rows)
                    {
                        if (row["Tipo"].ToString().Contains("Credito"))
                        {
                            row["Total"] = Convert.ToDecimal(row["Total"].ToString()) * -1;
                            row["neto21"] = Convert.ToDecimal(row["neto21"].ToString()) * -1;
                            row["subtotal"] = Convert.ToDecimal(row["subtotal"].ToString()) * -1;
                            row["retenciones"] = Convert.ToDecimal(row["retenciones"].ToString()) * -1;
                            row["netoNoGrabado"] = Convert.ToDecimal(row["netoNoGrabado"].ToString()) * -1;
                        }
                        //si esta anulada la pongo en cero para que no sume
                        if (row["estado"].ToString() == "0")
                        {
                            row["Total"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["retenciones"] = Convert.ToDecimal(0);
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                        }

                        total += Convert.ToDecimal(row["Total"].ToString());
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasFormaPagoR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("FechasFactura", dtFechas);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);

                ReportParameter param = new ReportParameter("ParamTotal", total.ToString("C"));
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Forma_Pago", "xls");

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

        private void generarReporte14(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                controladorVendedor contVendedores = new controladorVendedor();
                DataTable dtDetalles = new DataTable();
                if (tipo > 0)
                {
                    if (tipo == 1)
                    {
                        dtDetalles = this.controlador.obtenerIngresosBrutosByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }
                    else
                    {
                        dtDetalles = this.controlador.obtenerDetalleVentasPresupuestoByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                    }

                }
                else
                {
                    dtDetalles = this.controlador.obtenerDetalleVentasByFecha(fechaD, fechaH, suc, this.emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);
                }
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);

                Decimal total = 0;

                //dtDetalles.Columns.Add("Vendedor");

                //if (dtDetalles.Rows.Count > 0)
                //{
                //    foreach (DataRow row in dtDetalles.Rows)
                //    {
                //        //Vendedor v = contVendedores.obtenerVendedorID(Convert.ToInt32(row["IdVendedor"]));
                //        //row["Vendedor"] = v.emp.nombre + " " + v.emp.apellido;

                //        if (row["tipo"].ToString().Contains("Credito"))
                //        {
                //            row["iva"] = Convert.ToDecimal(row["iva"].ToString()) * -1;
                //            row["neto"] = Convert.ToDecimal(row["neto"].ToString()) * -1;
                //            row["pSinIva"] = Convert.ToDecimal(row["pSinIva"].ToString()) * -1;
                //        }
                //        //total += Convert.ToDecimal(row["Total"].ToString());
                //    }
                //}

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleVentasVendedoresSucursalCategoria.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFacturas", dtDatos);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Reporte_Detalle_Ventas_Vendedor_Sucursal_Categoria", "xls");

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
                throw ex;
            }
        }

        private void generarReporte15(string fechaD, string fechaH, int idSuc, int tipo, int cliente)
        {
            try
            {
                controladorSucursal controladorSucursal = new controladorSucursal();
                ControladorPlenario controladorPlenario = new ControladorPlenario();
                controladorFacturacion controladorFacturacion = new controladorFacturacion();

                DataTable dtDetalles = new DataTable();

                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                dtDetalles = this.controlador.obtenerDetalleVentasByFecha(fechaD, fechaH, suc, emp, tipo, cliente, tipofact, this.lista, this.anuladas, this.vendedor, this.formaPago);

                List<Planario_Api.Entidades.SolicitudPlenario> solicitudes = controladorPlenario.obtenerSolicitudesPlenariosByFecha(desde, hasta, emp);

                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, emp);

                Decimal total = 0;

                dtDetalles.Columns.Add("Solicitud", typeof(System.Int32));

                foreach (DataRow dr in dtDetalles.Rows)
                {
                    long idFactura = Convert.ToInt64(dr["id"]);
                    var solicitud = solicitudes.FirstOrDefault(x => x.Factura == idFactura);

                    if (solicitud != null)
                        dr["Solicitud"] = solicitud.NroSolicitud;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetallesVentasSolicitudesR.rdlc");
                ReportDataSource rds = new ReportDataSource("DetalleFacturas", dtDetalles);
                ReportDataSource rds2 = new ReportDataSource("DatosFacturas", dtDatos);

                ReportParameter param = new ReportParameter("ParamDesde", fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", fechaH);
                ReportParameter param3 = new ReportParameter("ParamTotal", total.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;


                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Reporte_Ventas_2", "xls");

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

            }
        }
        private void generarReporte16(string fechaD, string fechaH, int suc, int tipo, int cliente)
        {
            try
            {
                DataTable dtDatos = this.controlador.obtenerTotalFacturasRango(fechaD, fechaH, suc, tipo, this.emp);
                DataTable dtFechas = this.controlador.obtenerFechasFactura(fechaD, fechaH);
                DataTable dtFacturasA = new DataTable();
                DataTable dtFacturasB = new DataTable();
                DataTable dtFacturasE = new DataTable();
                DataTable dtFacturasNC = new DataTable();
                DataTable dtFacturasND = new DataTable();
                DataTable dtFacturasPresupuesto = this.controlador.obtenerFacturasRangoTipoDireccionDT(fechaD, fechaH, suc, -1, cliente, 17, this.anuladas, this.emp, 1);



                dtFacturasPresupuesto.Columns.Add("noGravado", typeof(decimal));//cuando el neto21 == 0


                if (dtFacturasPresupuesto.Rows.Count > 0)
                {
                    foreach (DataRow row in dtFacturasPresupuesto.Rows)
                    {
                        //Al Neto le resto el NoGravado y los muestro por separado.
                        row["noGravado"] = row["iva21"];
                        row["subtotal"] = Convert.ToDecimal(row["subtotal"]) - Convert.ToDecimal(row["iva21"]);

                        if (row["estado"].ToString() == "0")
                        {
                            row["netoNoGrabado"] = Convert.ToDecimal(0);
                            row["neto21"] = Convert.ToDecimal(0);
                            row["subtotal"] = Convert.ToDecimal(0);
                            row["total"] = Convert.ToDecimal(0);
                        }
                    }
                }



                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VentasR.rdlc");
                ReportDataSource rdsA = new ReportDataSource("DetalleFacturasA", dtFacturasA);
                ReportDataSource rdsB = new ReportDataSource("DetalleFacturasB", dtFacturasB);
                ReportDataSource rdsE = new ReportDataSource("DetalleFacturasE", dtFacturasE);
                ReportDataSource rdsNC = new ReportDataSource("DetalleFacturasNC", dtFacturasNC);
                ReportDataSource rdsND = new ReportDataSource("DetalleFacturasND", dtFacturasND);
                ReportDataSource rdspPRP = new ReportDataSource("DetallePresupuesto", dtFacturasPresupuesto);
                ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("FechasFactura", dtFechas);
                this.ReportViewer1.LocalReport.DataSources.Clear();


                this.ReportViewer1.LocalReport.DataSources.Add(rdsA);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsB);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsE);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsNC);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsND);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rdspPRP);

                //ReportParameter param = new ReportParameter("ParamTotal", total.ToString("C"));
                //this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;


                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Reporte_Ventas_2", "xls");

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

            }
        }

        private void generarReporte17(string fechaD, string fechaH, int suc, int tipo, int cliente)
        {
            try
            {
                DataTable dtFacturas = this.controlador.ReporteVentaConArticulos(fechaD, fechaH, suc, tipo, this.emp);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reporte_VentaConArticulos.rdlc");
                ReportDataSource rds1 = new ReportDataSource("VentasConArticulos", dtFacturas);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds1);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;


                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Reporte_VentasConArticulos", "xls");

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

            }
        }

    }
}