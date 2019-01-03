using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text.pdf;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImpresionCompras : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private string tipoDoc; 
        private int puntoVenta;
        private int accion;
        private int excel;
        private int ordenCompra;
        private int idRemito;
        private int proveedor;
        private int tipoFecha;
        private int idArticulo;
        private int idGrupo;
        private int tipo;//Remito compra
        private string idsRemitos;
        private int tipoDocumento;

        controladorCompraEntity contCompraEntity = new controladorCompraEntity();
        ControladorEmpresa controlEmpresa = new ControladorEmpresa();
        ControladorCCProveedor controladorCCP = new ControladorCCProveedor();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorPagos controladorPagos = new controladorPagos();
        controladorCliente controladorCliente = new controladorCliente();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {                    
                    this.fechaD = Request.QueryString["fd"];
                    this.fechaH = Request.QueryString["fh"];
                    this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                    this.suc = Convert.ToInt32(Request.QueryString["s"]);
                    this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                    this.tipoDoc = Request.QueryString["t"];
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);  
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.ordenCompra = Convert.ToInt32(Request.QueryString["oc"]);
                    this.idRemito = Convert.ToInt32(Request.QueryString["rc"]);
                    this.proveedor = Convert.ToInt32(Request.QueryString["prov"]);
                    this.tipo = Convert.ToInt32(Request.QueryString["tipo"]);//tipo RC = FC,PRP o ANULADOS
                    this.idsRemitos = Request.QueryString["ids"];
                    this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                    this.idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                    this.tipoDocumento = Convert.ToInt32(Request.QueryString["td"]);

                    if (accion == 1)
                    {
                        this.generarReporte();//comprasR
                    }
                    if (accion == 2)
                    {
                        this.generarReporte2();//citi compras
                    }
                    if(accion == 3)
                    {
                        this.generarReporte3();//ordenes de compra
                    }
                    if (accion == 4)
                    {
                        this.generarReporte4();//impagas proveedores
                    }
                    if (accion == 5)
                    {
                        this.generarReporte5();//Informe Remitos Compras
                    }
                    if (accion == 6)
                    {
                        this.generarReporte6();//impresion cta cte.
                    }
                    if (accion == 7)
                    {
                        this.generarReporte7();//detalle Compras 2do formato
                    }
                    if (accion == 8)
                    {
                        this.generarReporte8();//Remito Compra Devolucion
                    }
                    if (accion == 9)
                    {
                        this.generarReporte9();//Remito Compra Etiquetas
                    }
                    if (accion == 10)
                    {
                        this.generarReporte10();//Remito Compra Etiquetas
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void generarReporte()
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                controladorCliente cont = new controladorCliente();

                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime Hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));               

                if (tipoDoc == "0")
                    tipoDoc = null;

                List<Gestion_Api.Entitys.Compra> compras = this.contCompraEntity.buscarCompras(desde, Hasta, tipoDoc, suc, puntoVenta,proveedor,tipoFecha);
                
                DataTable dtCompras = new DataTable();
                dtCompras = ListToDataTable(compras);
                dtCompras.Columns.Add("razonSocial", typeof(string));
                dtCompras.Columns.Add("PlanDeCuentas", typeof(string));

                decimal saldoTotal = 0;

                foreach (DataRow row in dtCompras.Rows)
                {
                    if (row["tipoDocumento"].ToString().Contains("Crédito"))
                    {
                        row["NetoNoGrabado"] = Convert.ToDecimal(row["NetoNoGrabado"]) * -1;
                        row["Neto2"] = Convert.ToDecimal(row["Neto2"]) * -1;
                        row["Iva2"] = Convert.ToDecimal(row["Iva2"]) * -1;
                        row["Neto5"] = Convert.ToDecimal(row["Neto5"]) * -1;
                        row["Iva5"] = Convert.ToDecimal(row["Iva5"]) * -1;
                        row["Neto105"] = Convert.ToDecimal(row["Neto105"]) * -1;
                        row["Iva105"] = Convert.ToDecimal(row["Iva105"]) * -1;
                        row["Neto21"] = Convert.ToDecimal(row["Neto21"]) * -1;
                        row["Iva21"] = Convert.ToDecimal(row["Iva21"]) * -1;
                        row["Neto27"] = Convert.ToDecimal(row["Neto27"]) * -1;
                        row["Iva27"] = Convert.ToDecimal(row["Iva27"]) * -1;
                        row["PIB"] = Convert.ToDecimal(row["PIB"]) * -1;
                        row["PIva"] = Convert.ToDecimal(row["PIva"]) * -1;
                        row["ImpuestosInternos"] = Convert.ToDecimal(row["ImpuestosInternos"]) * -1;
                        row["Otros"] = Convert.ToDecimal(row["Otros"]) * -1;
                        row["Total"] = Convert.ToDecimal(row["Total"]) * -1;                        
                    }
                    saldoTotal += Convert.ToDecimal(row["Total"]);
                    var p = cont.obtenerProveedorID((int)row["Proveedor"]);
                    row["razonSocial"] = p.razonSocial;
                    long temp = Convert.ToInt64(row["Id"].ToString());

                    try
                    {
                        row["PlanDeCuentas"] = contPlanCuentas.obtenerCuentaContableCompra(temp).Cuentas_Contables.Codigo + " - " + contPlanCuentas.obtenerCuentaContableCompra(temp).Cuentas_Contables.Descripcion;
                    }
                    catch { };
                }   



                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComprasR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCompras", dtCompras);
                ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString("C"));                

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
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

        private void generarReporte2()
        {
            try
            {
                controladorReportes contReport = new controladorReportes();

                String rutaTxt = Server.MapPath("CitiCompras.txt");                
                String comprobante = contReport.generarCitiCompra(fechaD, fechaH, tipoDoc, suc, puntoVenta, rutaTxt,proveedor,tipoFecha);

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
                    this.Response.AddHeader("Content-disposition", "attachment; filename= "+ DateTime.Today.Date.ToShortDateString() +"CitiCompras.txt");
                    this.Response.BinaryWrite(btFile);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte3()
        {
            try
            {
                controladorCliente cont = new controladorCliente();
                controladorSucursal contSuc = new controladorSucursal();

                Gestion_Api.Entitys.OrdenesCompra ordenCompra = this.contCompraEntity.obtenerOrden(this.ordenCompra);
                Gestor_Solution.Modelo.Cliente p = cont.obtenerProveedorID(ordenCompra.IdProveedor.Value);

                Sucursal s = contSuc.obtenerSucursalID(ordenCompra.IdSucursal.Value);

                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById(s.empresa.id);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;

                String Fecha = " ";
                String FechaEntrega = " ";
                String Numero = " ";
                String Proveedor = " ";
                String Observacion = "-";

                foreach (DataRow row in dtEmpresa.Rows)//Datos empresa 
                {                    
                    razonSoc = row["Razon Social"].ToString();
                    condIVA = row["Condicion IVA"].ToString();
                    direComer = row["Direccion"].ToString();
                }

                if (ordenCompra != null && p != null)
                {
                    Fecha = ordenCompra.Fecha.Value.ToString("dd/MM/yyyy");
                    FechaEntrega = ordenCompra.FechaEntrega.Value.ToString("dd/MM/yyyy");
                    Numero = "Nº " + ordenCompra.Numero;
                    Proveedor = p.razonSocial;
                    Observacion = ordenCompra.Observaciones;
                }

                string logo = Server.MapPath("../../Facturas/" + s.empresa.id + "/Logo.jpg");

                List<Gestion_Api.Entitys.OrdenesCompra_Items> itemsOrdenes = ordenCompra.OrdenesCompra_Items.ToList();//obtengo los items de la OC
                DataTable dtItems = ListToDataTable(itemsOrdenes);//Paso la list a datatable para pasarlo al report.
                dtItems.Columns.Add("CodProv");

                foreach (DataRow row in dtItems.Rows)
                {
                    ProveedorArticulo codProv = this.contArticulo.obtenerProveedorArticuloByArticulo(Convert.ToInt32(row["Codigo"]));

                    Articulo art = this.contArticulo.obtenerArticuloByID(Convert.ToInt32(Convert.ToInt32(row["Codigo"])));

                    if (art != null)
                    {
                        row["Codigo"] = art.codigo;
                    }
                    if (codProv != null)
                    {
                        row["CodProv"] = codProv.codigoProveedor;
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("OrdenesCompraR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("ItemsOrden", dtItems);
                ReportParameter param1 = new ReportParameter("ParamFecha", Fecha);
                ReportParameter param2 = new ReportParameter("ParamFechaEntrega", FechaEntrega);
                ReportParameter param3 = new ReportParameter("ParamNumero", Numero);
                ReportParameter param4 = new ReportParameter("ParamProveedor", Proveedor);
                ReportParameter param5 = new ReportParameter("ParamObservacion", Observacion);

                ReportParameter param12 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);

                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);

                this.ReportViewer1.LocalReport.SetParameters(param12);//datos empresa
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);

                this.ReportViewer1.LocalReport.SetParameters(param32);//logo

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

        private void generarReporte4()
        {
            try
            {
                DataTable dtImpagas = this.controladorCCP.obtenerMovimientosProveedorRangoDetallado(this.fechaH, this.proveedor, this.suc, Convert.ToInt32(this.tipoDoc));
                dtImpagas.Columns.Add("Telefono");

                foreach (DataRow documentoImpago in dtImpagas.Rows)
                {
                    if (documentoImpago["documento"].ToString() == "Pago")
                    {
                        var pago = this.controladorPagos.obtenerPagoById(Convert.ToInt64(documentoImpago["DocumentoId"]));
                        if (pago != null)
                        {
                            if (pago.Ftp == 0)
                            {
                                documentoImpago["documento"] = "Pago FC";
                            }
                            if (pago.Ftp == 1)
                            {
                                documentoImpago["documento"] = "Pago PRP";
                            }
                        }
                    }

                    ObtenerContactoProveedorDeDocumentoImpago(documentoImpago);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ImpagasProvR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DatosImpagas", dtImpagas);
                ReportParameter param1 = new ReportParameter("ParamFecha", this.fechaH);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);                

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Impagas_Proveedores", "xls");

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

        private void generarReporte5()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime Hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));

                DataTable dtDetalleRemitos = this.contCompraEntity.obtenerDetallesRemitosDT(desde, Hasta, this.proveedor, this.suc,this.tipo);               
                

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RemitosCompraR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DetalleRemito", dtDetalleRemitos);
                ReportParameter param1 = new ReportParameter("ParamDesde", this.fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", this.fechaH);
                ReportParameter param3 = new ReportParameter("ParamTipo", this.tipo.ToString());

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Remitos_Compras", "xls");

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

        private void generarReporte6()
        {
            try
            {
                DateTime fdesde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<MovimientosCCP> listCuentaProv = this.controladorCCP.obtenerMovimientosProveedorByBN(this.proveedor, this.suc, Convert.ToInt32(this.tipoDoc), fdesde, fhasta, Convert.ToInt32(this.tipoDocumento));
                listCuentaProv = listCuentaProv.OrderBy(x => x.Fecha).ToList();
                DataTable dtDetalleCuenta = new DataTable();

                dtDetalleCuenta = ListToDataTable(listCuentaProv);
                
                controladorCliente contCliente = new controladorCliente();
                Cliente proveedor = contCliente.obtenerProveedorID(this.proveedor);

                dtDetalleCuenta.Columns.Add("Tipo");
                dtDetalleCuenta.Columns.Add("SaldoAcumulado", typeof(decimal));
                decimal saldoAcumulado = 0;
                foreach (DataRow row in dtDetalleCuenta.Rows)
                {
                    string tipoDocumento = " FC ";
                    int idFact = Convert.ToInt32(row["Id"]);
                    MovimientosCCP m = listCuentaProv.Where(x => x.Id == idFact).FirstOrDefault();
                    
                    if (m.Ftp == 1)
                        tipoDocumento = " PRP ";
                    if (m.Ftp == 2)
                        tipoDocumento = " ";

                    if (row["TipoDocumento"].ToString() == "19")
                    {
                        row["Tipo"] = tipoDocumento + "  Nº";
                    }
                    if (row["TipoDocumento"].ToString() == "21")
                    {
                        row["Tipo"] = tipoDocumento + "Pago Nº";
                    }
                    if (row["TipoDocumento"].ToString() == "7" || row["TipoDocumento"].ToString() == "8" || row["TipoDocumento"].ToString() == "9" || row["TipoDocumento"].ToString() == "25")
                    {
                        row["Tipo"] = tipoDocumento = " NC ";
                    }
                    if (Convert.ToDecimal(row["Debe"]) > 0)
                    {
                        saldoAcumulado += Convert.ToDecimal(row["Debe"]);                        
                    }
                    if (Convert.ToDecimal(row["Haber"]) > 0)
                    {
                        saldoAcumulado -= Convert.ToDecimal(row["Haber"]);
                    }

                    row["SaldoAcumulado"] = saldoAcumulado;
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CuentaCorrientePR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DetalleCuenta", dtDetalleCuenta);
                ReportParameter param1 = new ReportParameter("ParamProveedor", proveedor.alias);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Cta Cte Proveedor", "xls");

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
        private void generarReporte7()
        {
            try
            {
                controladorCliente cont = new controladorCliente();

                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime Hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));

                if (tipoDoc == "0")
                    tipoDoc = null;

                List<Gestion_Api.Entitys.Compra> compras = this.contCompraEntity.buscarCompras(desde, Hasta, tipoDoc, suc, puntoVenta, proveedor, tipoFecha);

                DataTable dtCompras = new DataTable();
                dtCompras = ListToDataTable(compras);
                dtCompras.Columns.Add("razonSocial", typeof(string));

                decimal saldoTotal = 0;

                foreach (DataRow row in dtCompras.Rows)
                {
                    if (row["tipoDocumento"].ToString().Contains("Crédito"))
                    {
                        row["NetoNoGrabado"] = Convert.ToDecimal(row["NetoNoGrabado"]) * -1;
                        row["Neto105"] = Convert.ToDecimal(row["Neto105"]) * -1;
                        row["Iva105"] = Convert.ToDecimal(row["Iva105"]) * -1;
                        row["Neto21"] = Convert.ToDecimal(row["Neto21"]) * -1;
                        row["Iva21"] = Convert.ToDecimal(row["Iva21"]) * -1;
                        row["Neto27"] = Convert.ToDecimal(row["Neto27"]) * -1;
                        row["Iva27"] = Convert.ToDecimal(row["Iva27"]) * -1;
                        row["PIB"] = Convert.ToDecimal(row["PIB"]) * -1;
                        row["PIva"] = Convert.ToDecimal(row["PIva"]) * -1;
                        row["ImpuestosInternos"] = Convert.ToDecimal(row["ImpuestosInternos"]) * -1;
                        row["Otros"] = Convert.ToDecimal(row["Otros"]) * -1;
                        row["Total"] = Convert.ToDecimal(row["Total"]) * -1;
                    }
                    saldoTotal += Convert.ToDecimal(row["Total"]);
                    var p = cont.obtenerProveedorID((int)row["Proveedor"]);
                    row["razonSocial"] = p.razonSocial;
                }



                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleComprasR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCompras", dtCompras); //guarda el dataset en un datasource para el report viewer
                ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
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
        private void generarReporte8()
        {
            try
            {
                controladorCliente cont = new controladorCliente();
                controladorSucursal contSuc = new controladorSucursal();

                var remito = this.contCompraEntity.obtenerRemito(this.idRemito);
                Cliente p = cont.obtenerProveedorID(remito.IdProveedor.Value);
                Sucursal s = contSuc.obtenerSucursalID(remito.IdSucursal.Value);

                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById(s.empresa.id);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;

                foreach (DataRow row2 in dtEmpresa.Rows)//Datos empresa 
                {
                    razonSoc = row2["Razon Social"].ToString();
                    condIVA = row2["Condicion IVA"].ToString();
                    direComer = row2["Direccion"].ToString();
                }

                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("Fecha");
                dtDatos.Columns.Add("Proveedor");
                dtDatos.Columns.Add("Numero");
                dtDatos.Columns.Add("Tipo");
                dtDatos.Columns.Add("Sucursal");
                dtDatos.Columns.Add("Devolucion");
                dtDatos.Columns.Add("Observaciones");

                DataRow row = dtDatos.NewRow();
                row["Fecha"] = remito.Fecha.Value.ToString("dd/MM/yyyy");
                row["Proveedor"] = p.razonSocial;
                row["Sucursal"] = s.nombre;
                row["Numero"] = remito.Numero;
                row["Tipo"] = remito.Tipo;
                row["Devolucion"] = remito.Devolucion;
                if (remito.RemitosCompras_Comentarios != null)
                {
                    row["Observaciones"] = remito.RemitosCompras_Comentarios.Observacion;
                }
                dtDatos.Rows.Add(row);


                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("Codigo");
                dtItems.Columns.Add("Descripcion");
                dtItems.Columns.Add("Cantidad", typeof(decimal));
                dtItems.Columns.Add("FechaDespacho");
                dtItems.Columns.Add("NumeroDespacho");
                dtItems.Columns.Add("Lote");
                dtItems.Columns.Add("Vencimiento");

                foreach (var item in remito.RemitosCompras_Items)
                {
                    DataRow dr = dtItems.NewRow();
                    var articulo = this.contArticulo.obtenerArticuloByID(item.Codigo.Value);

                    dr["Codigo"] = articulo.codigo;
                    dr["Descripcion"] = articulo.descripcion;
                    dr["Cantidad"] = item.Cantidad;
                    dr["FechaDespacho"] = item.FechaDespacho;
                    dr["NumeroDespacho"] = item.NumeroDespacho;
                    dr["Lote"] = item.Lote;
                    dr["Vencimiento"] = item.Vencimiento;
                    dtItems.Rows.Add(dr);
                }                

                string logo = Server.MapPath("../../Facturas/" + s.empresa.id + "/Logo.jpg");
                Log.EscribirSQL(1, "INFO", "Logo Remito Compra: " + logo);

                //codigo de barra
                string imagen = this.generarCodigo(this.idRemito);
                ReportParameter param10 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);
                ReportParameter param11 = new ReportParameter("ParamIdRemito", idRemito.ToString());

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleRemitoR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;


                ReportDataSource rds = new ReportDataSource("DatosRemito", dtDatos);
                ReportDataSource rds2 = new ReportDataSource("ItemsRemito", dtItems);
                
                ReportParameter param1 = new ReportParameter("ParamImagen", @"file:///" + logo);
                ReportParameter param12 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);

                this.ReportViewer1.LocalReport.SetParameters(param1);//logo
                this.ReportViewer1.LocalReport.SetParameters(param12);
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "RemitoCompra", "xls");

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
        private void generarReporte9()
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("Codigo");
                dtItems.Columns.Add("Descripcion");
                dtItems.Columns.Add("Cantidad",typeof(decimal));
                dtItems.Columns.Add("Imagen");
                dtItems.Columns.Add("CodigoBarras");

                foreach (string id in this.idsRemitos.Split(';'))
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        RemitosCompra rc = this.contCompraEntity.obtenerRemito(Convert.ToInt32(id));
                        foreach (var item in rc.RemitosCompras_Items)
                        {
                            var art = contArtEnt.obtenerArticuloEntity(item.Codigo.Value);

                            for (int i = 0; i < item.Cantidad.Value; i++)
                            {
                                DataRow row = dtItems.NewRow();                                
                                row["Codigo"] = art.codigo;
                                row["Descripcion"] = art.descripcion;
                                row["Cantidad"] = item.Cantidad.Value;

                                if (art != null)
                                {
                                    if (!string.IsNullOrEmpty(art.codigoBarra) && art.codigoBarra != "0")
                                    {
                                        string imagen = this.generarCodigoEtiquetasRemitosCompra(art.codigoBarra, Convert.ToInt32(id), art.id);
                                        row["Imagen"] = @"file:///" + imagen;
                                        row["CodigoBarras"] = art.codigoBarra;
                                    }
                                }

                                dtItems.Rows.Add(row);
                            }   
                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("EtiquetasR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DatosItems", dtItems);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);               

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "RemitoCompra", "xls");

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
        private void generarReporte10()
        {
            try
            {
                var articulo = this.contArticulo.obtenerArticuloByID(this.idArticulo);
                List<Trazabilidad_Campos> lstCampos = this.contArticulo.obtenerCamposTrazabilidadByGrupo(articulo.grupo.id);
                DataTable dtDatos = this.contCompraEntity.obtenerTrazabilidadItemByRemito(this.idRemito, this.idArticulo);

                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("Campo1");
                dtItems.Columns.Add("Campo2");
                dtItems.Columns.Add("Campo3");
                dtItems.Columns.Add("Campo4");
                dtItems.Columns.Add("Campo5");
                dtItems.Columns.Add("Campo6");
                dtItems.Columns.Add("Campo7");
                dtItems.Columns.Add("Campo8");
                dtItems.Columns.Add("Campo9");
                dtItems.Columns.Add("Campo10"); 
                dtItems.Columns.Add("Imagen");
                dtItems.Columns.Add("Codigo");


                int cont = 0;
                while (cont < dtDatos.Rows.Count)
                {
                    DataRow drItem = dtItems.NewRow();//cargo la fila con los datos de la traza
                    int i = 0;
                    foreach (var c in lstCampos)
                    {
                        var drDatos = dtDatos.Rows[cont];
                        if (i == 0)//guardo un id de trazabilidad para dps relacionar, en el codigo de barra
                        {
                            string imagen = this.generarCodigo(drDatos[0].ToString(), Convert.ToInt64(drDatos[0]));
                            drItem["Imagen"] = @"file:///" + imagen;

                            //Agrego codigo
                            try {drItem["Codigo"] = drDatos[0].ToString();}
                            catch { }
                        }
                        
                        drItem[i] = drDatos[2] + ": " + drDatos[3];
                        i++;
                        cont++;
                    }
                    dtItems.Rows.Add(drItem);
                }
                
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("EtiquetasTrazas.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DatosItems", dtItems);

                //ReportParameter param12 = new ReportParameter("CodigoBarra", "9834320");

                

                
                //ReportParameter rp16 = new ReportParameter("rpImagen", @"file:///" + imagen);


                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                //this.ReportViewer1.LocalReport.SetParameters(param12);
                //this.ReportViewer1.LocalReport.SetParameters(rp16);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "EstiquetasTrazas", "xls");

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
        public string generarCodigo(int idRemito)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = idRemito.ToString();

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/RemitosCompras/" + idRemito + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + idRemito + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                //Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para pedido. " + ex.Message);
                return null;
            }
        }
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
        public string generarCodigo(string codigo, long Traza)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = codigo;

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/CodigosTraza/" + Traza + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + Traza + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para etiqueta de trazabilidad. " + ex.Message);
                return null;
            }

        }
        public string generarCodigoEtiquetasRemitosCompra(string codigo, int idRemitoCompra, int idArticulo)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = codigo;

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/RemitosCompras/Etiquetas/" + idRemitoCompra + "/" + idArticulo + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + idArticulo + "_" + idRemitoCompra + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para etiqueta de remito de compra para el articulo con id " + idArticulo + " y Remito de Compra con id " + idRemitoCompra + ". Excepción: " + Ex.Message);
                return null;
            }

        }
        public void ObtenerContactoProveedorDeDocumentoImpago(DataRow filaProveedor)
        {
            try
            {
                string numerosTelefonicos = string.Empty;
                
                List<contacto> contactosCliente = controladorCliente.obtenerContactos(Convert.ToInt32(filaProveedor["id"]));
                foreach (var contacto in contactosCliente)
                {
                    numerosTelefonicos += contacto.numero + " | ";
                }

                filaProveedor["Telefono"] = numerosTelefonicos.Substring(0,numerosTelefonicos.Length - 2);
            }
            catch
            {
            }
        }

    }
}