using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ImpresionPresupuesto : System.Web.UI.Page
    {
        private int idPresupuesto;
        private int accion;
        private int original;
        controladorFacturacion controlador = new controladorFacturacion();
        controladorCliente controlCliente = new controladorCliente();
        ControladorEmpresa controlEmpresa = new ControladorEmpresa();
        controladorRemitos controlRemito = new controladorRemitos();
        controladorSucursal controlSucursal = new controladorSucursal();
        controladorCobranza contCobranza = new controladorCobranza();
        ControladorPedido contPedidos = new ControladorPedido();
        controladorFactEntity controladorFactEntity = new controladorFactEntity();

        Configuracion configuracion = new Configuracion();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    idPresupuesto = Convert.ToInt32(Request.QueryString["Presupuesto"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.original = Convert.ToInt32(Request.QueryString["o"]);
                    //presupuesto
                    if (accion == 0)
                    {
                        this.generarReporte2(idPresupuesto);
                    }
                    //factura a,e
                    if (accion == 1)
                    {
                        this.generarReporte3(idPresupuesto);
                    }
                    //factura b
                    if (accion == 2)
                    {
                        this.generarReporte4(idPresupuesto);
                    }
                    //Remito
                    if (accion == 3)
                    {
                        this.generarReporte5(idPresupuesto);
                    }
                    //Formulario 12 
                    if (accion == 4)
                    {
                        this.generarReporte6(idPresupuesto);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Presupuesto. " + ex.Message);
            }
        }

        private void generarReporte(int idPresupuesto)
        {
            try
            {
                //ReportDocument reporte = new ReportDocument();
                //reporte.Load(Server.MapPath("Presupuesto.rpt"));
                //DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                //DataTable dtDetalle = controlador.obtenerDetallePresupuesto(idPresupuesto);
                //decimal TotalFinal = 0;

                //foreach (DataRow dr in dtDetalle.Rows)
                //{
                //    reporte.SetParameterValue("Numero", dr["Numero"]);
                //    reporte.SetParameterValue("Fecha", dr["Fecha"]);
                //    reporte.SetParameterValue("RazonSocial", dr["RazonSocial"]);
                //    reporte.SetParameterValue("CUIT", dr["CUIT"]);
                //    reporte.SetParameterValue("IVA", dr["IVA"]);
                //}

                //foreach (DataRow dr in dtDatos.Rows)
                //{
                //    reporte.SetParameterValue("Codigo", dr["Codigo"]);
                //    reporte.SetParameterValue("Descripcion", dr["Descripcion"]);
                //    reporte.SetParameterValue("PrecioUnitario", dr["PrecioUnitario"]);
                //    reporte.SetParameterValue("Cantidad", dr["Cantidad"]);
                //    reporte.SetParameterValue("Total", dr["Total"]);
                //    TotalFinal += Convert.ToDecimal(dr["Total"]);
                //}

                //reporte.SetParameterValue("TotalFinal", TotalFinal);

                ////CrystalReportViewer1.ReportSource = reporte;
                //reporte.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, "Pedido_" + idPedido.ToString());
            }
            catch
            {

            }
        }

        //Presupuesto
        private void generarReporte2(int idPresupuesto)
        {
            try
            {
                Factura fact = this.controlador.obtenerFacturaId(idPresupuesto);
                //obtengo detalle de items
                //DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                DataTable dtDetalle = controlador.obtenerDetallePresupuesto(idPresupuesto);

                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();

                //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                DataTable dtTotales = controlador.obtenerTotalPresupuesto2(idPresupuesto);
                DataRow dr = dtTotales.Rows[0];
                decimal subtotal = Convert.ToDecimal(dr[0]);
                decimal descuento = Convert.ToDecimal(dr[1]);
                decimal total = Convert.ToDecimal(dr[2]);
                decimal saldoCtaCte = 0;
                try
                {
                    //obtengo el saldo de la cuenta corriente del cliente                
                    DataTable dt = this.contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), fact.fecha.AddHours(23).ToString("dd/MM/yyyy"), fact.cliente.id, 0, fact.sucursal.id, 1, 0);
                    saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                }
                catch (Exception ex)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error en generarReporte2. " + ex.Message);
                }

                String cotizacionFecha = String.Empty;

                //obtengo el telefono del cliente para agregarlo al pedido
                string telefono = "";
                List<contacto> contactosClientes = controlCliente.obtenerContactos(Convert.ToInt32(srCliente["idCliente"]));
                if (contactosClientes.Count > 0 & contactosClientes != null)
                {
                    telefono = contactosClientes[0].numero;
                }
                if (String.IsNullOrEmpty(telefono))
                {
                    telefono = "-";
                }
                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";
                DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(idPresupuesto);
                foreach (DataRow drl in dtDireccion.Rows)
                {
                    if (drl[0].ToString() == "Legal")
                    {
                        direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                            drl[4].ToString() + " " + drl[5].ToString();
                    }
                    if (drl[0].ToString() == "Entrega")
                    {
                        direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                            drl[4].ToString() + " " + drl[5].ToString();
                    }
                }

                //sucursal venta
                string sucursalOrigen = dtDetalle.Rows[0]["Sucursal"].ToString();
                Sucursal sucvta = this.controlSucursal.obtenerSucursalID(Convert.ToInt32(sucursalOrigen));
                //sucursalfacturada                
                string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                if (sucursalFact != "0")
                {
                    Sucursal s = this.controlSucursal.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                    sucursalFact = "-" + s.nombre;
                }
                else
                {
                    sucursalFact = " ";
                }

                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                {
                    dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                }
                //datos cotizacion al momento de fc
                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                {
                    cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                }

                //string logo = Server.MapPath("../../Images/Logo.jpg");

                //Cargo configuracion para mostrar Precio de venta con o sin IVA.
                Configuracion c = new Configuracion();

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (c.porcentajeIva != "0")
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                }
                else
                {
                    //subtotal sin iva
                    //subtotal = Convert.ToDecimal(dr[4]);
                    total = subtotal - descuento;
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2SinIva.rdlc");
                }

                String nroPedido = String.Empty;
                String nroRemito = String.Empty;
                //Pedido Relacionado
                Pedido pedidoFc = this.contPedidos.obtenerPedidoByFacturaID(idPresupuesto);
                if (pedidoFc != null)
                {
                    nroPedido = pedidoFc.numero;
                }
                //Remito Relacionado
                Remito remitoFc = this.controlador.obtenerRemitoByFactura(idPresupuesto);
                if (remitoFc != null)
                {
                    nroRemito = remitoFc.numero;
                }

                //Comentario factura
                DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(idPresupuesto);

                //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("ItemsPresupuesto", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("DatosPresupuesto", dtDatos);
                ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);


                ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact
                ReportParameter param04 = new ReportParameter("ParamSucursal", sucvta.nombre);//sucursalVta

                //ReportParameter param32 = new ReportParameter("Porcentaje", porcentaje.ToString("N"));

                ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);
                ReportParameter param7 = new ReportParameter("TelefonoEntrega", telefono);
                ReportParameter param8 = new ReportParameter("ParamCambioDolar", cotizacionFecha);

                //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                //this.ReportViewer1.LocalReport.SetParameters(param32);

                ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());

                ReportParameter param40 = new ReportParameter("ParamNroPedido", nroPedido);
                ReportParameter param41 = new ReportParameter("ParamNroRemito", nroRemito);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param03);
                this.ReportViewer1.LocalReport.SetParameters(param04);

                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);

                this.ReportViewer1.LocalReport.SetParameters(param8);

                this.ReportViewer1.LocalReport.SetParameters(param33);

                //nro pedido y nro remito relacionados
                this.ReportViewer1.LocalReport.SetParameters(param40);
                this.ReportViewer1.LocalReport.SetParameters(param41);

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
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }

        //factura a-e
        private void generarReporte3(int idFactura)
        {
            try
            {

                Factura fact = this.controlador.obtenerFacturaId(idFactura);

                DataTable dtDatos = new DataTable();

                dtDatos = this.obtenerDTarticulos(dtDatos);

                //datos de encabezado y pie
                DataTable dtDetalle = controlador.obtenerDetallePresupuesto(idPresupuesto);
                //nro remito factura
                DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(idPresupuesto);

                //datos del emisor
                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;
                String ingBrutos = String.Empty;
                String fechaInicio = String.Empty;
                String cuitEmpresa = String.Empty;
                String nroFactura = String.Empty;
                String tipoDoc = String.Empty;
                String letraDoc = String.Empty;
                String CodigoDoc = String.Empty;
                String CAE = String.Empty;
                String ptoVta = String.Empty;
                String codBarra = String.Empty;
                String fechaVto = string.Empty;
                String cotizacionFecha = String.Empty;

                //levanto los datos de la factura
                var drDatosFactura = dtDetalle.Rows[0];
                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                {
                    dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                }
                //datos cotizacion al momento de fc
                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                {
                    cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                }
                //sucursalfacturada                
                string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                if (sucursalFact != "0")
                {
                    controladorSucursal contSuc = new controladorSucursal();
                    Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                    sucursalFact = "-" + s.nombre;
                }
                else
                {
                    sucursalFact = " ";
                }

                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                foreach (DataRow row in dtEmpresa.Rows)
                {
                    //verifico cual es la empresa de la factura
                    //if ((int)row[0] == )
                    //{
                    cuitEmpresa = row.ItemArray[1].ToString();
                    razonSoc = row.ItemArray[2].ToString();
                    ingBrutos = row.ItemArray[3].ToString();
                    fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();
                                                                                                 //fechaInicio = Convert.ToDateTime(fechaInicio).ToShortDateString();
                    condIVA = row.ItemArray[5].ToString();
                    direComer = row.ItemArray[6].ToString();
                    //}
                }

                //datos factura
                string auxNro = drDatosFactura["Numero"].ToString();
                nroFactura = auxNro.Substring(auxNro.Length - 13, 13);
                //nombre tipo documento para el parametro
                tipoDoc = auxNro.Substring(0, auxNro.Length - 16);
                //letra y cod. factura                

                if (drDatosFactura["Cae"] != null)
                {
                    CAE = drDatosFactura["Cae"].ToString();
                    //CAE = "-";
                }
                else
                {
                    CAE = "-";
                }
                ptoVta = drDatosFactura["ptoVenta"].ToString();
                fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                if (string.IsNullOrEmpty(codBarra))
                {
                    codBarra = "0";

                }

                //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                PuntoVenta pv = this.controlSucursal.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                int esPreimpresa = 0;
                if (pv != null)
                {
                    if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                    {
                        esPreimpresa = 1;
                    }
                }

                if (tipoDoc == "Factura E ")
                {
                    letraDoc = "E";
                    CodigoDoc = "Cod. 19";
                }
                else
                {
                    letraDoc = "A";
                    if (pv.FacturaPyme == 1)
                    {
                        CodigoDoc = "Cod. 201";
                    }
                    else
                    {
                        CodigoDoc = "Cod. 01";
                    }
                }
                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();

                //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                DataTable dtTotales = controlador.obtenerTotalPresupuesto2(idPresupuesto);

                AgregarIvasToTotalesDeFactura(dtTotales, fact);

                DataRow dr = dtTotales.Rows[0];

                //obtengo el saldo de la cuenta corriente del cliente                
                DataTable dt = this.contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), fact.fecha.AddHours(23).ToString("dd/MM/yyyy"), fact.cliente.id, 0, fact.sucursal.id, 0, 0);
                decimal saldoCtaCte = 0;
                try
                {
                    saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                }
                catch (Exception ex)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error buscando saldoCtaCte. " + ex.Message);
                }

                //neto no grabado
                decimal subtotal = Convert.ToDecimal(dr[4]);
                decimal descuento = Convert.ToDecimal(dr[1]);
                //subtotal menos el descuento
                decimal subtotal2 = Convert.ToDecimal(dr[5]);
                //iva discriminado de la fact
                decimal iva = Convert.ToDecimal(dr[3]);
                //IIBB (retenciones)
                decimal retencion = Convert.ToDecimal(dr["retenciones"]);
                //conceptos no gravados(combustible)
                decimal conceptos = Convert.ToDecimal(dr["iva21"]);
                //total de la factura
                decimal total = Convert.ToDecimal(dr[2]);
                //letras
                string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                //string totalS = Numalet.ToCardinal("18.25");
                //cant unidades
                decimal cant = 2;
                //decimal totalIva105 = Convert.ToDecimal(dr["TotalIva105"]);
                //decimal totalIva21 = Convert.ToDecimal(dr["TotalIva21"]);
                //decimal totalIva27 = Convert.ToDecimal(dr["TotalIva27"]);

                //Total equivalente en dolares
                controladorMoneda contMoneda = new controladorMoneda();
                Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                decimal TotalDolares = 0;
                String textoDolares = String.Empty;
                if (dolar != null)
                {
                    TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                }
                if (tipoDoc.Contains("Nota de"))
                {
                    textoDolares = " ";
                }
                else
                {
                    textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                }

                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";
                DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(idPresupuesto);
                if (dtDireccion != null)
                {
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl[0].ToString() == "Legal")
                        {
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                        if (drl[0].ToString() == "Entrega")
                        {
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                    }
                }

                String nroPedido = String.Empty;
                String nroRemito = String.Empty;
                //Pedido Relacionado
                Pedido pedidoFc = this.contPedidos.obtenerPedidoByFacturaID(idPresupuesto);
                if (pedidoFc != null)
                {
                    nroPedido = pedidoFc.numero;
                }
                //Remito Relacionado
                Remito remitoFc = this.controlador.obtenerRemitoByFactura(idPresupuesto);
                if (remitoFc != null && !string.IsNullOrEmpty(remitoFc.numero))
                {
                    nroRemito = remitoFc.numero;
                }

                //Comentario factura
                DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(idPresupuesto);

                //obtengo id empresa para buscar el logo correspondiente
                int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc +"/Logo.jpg");                
                string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/" + pv.id + "/Logo.jpg");
                Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);
                BarcodeProfessional bcp = new BarcodeProfessional();

                //Barcode settings
                bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;
                bcp.BarHeight = 0.25f;
                bcp.Code = codBarra;

                byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                DataTable dtImagen = new DataTable();

                DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));

                dtImagen.Columns.Add(ColumnImagen);
                dtImagen.Rows.Add(prodBarcode);
                //Generate the barcode image and store it into the Barcode Column

                //Condición de Pago
                string condicionPago = String.Empty;
                if (fact.formaPAgo.id == 7)
                {
                    condicionPago = fact.cliente.vencFC.ToString();
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                if (letraDoc == "A")
                {
                    if (pv.monedaFacturacion > 1)
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaREnMonedaOriginal.rdlc");
                    else
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                }
                if (letraDoc == "E")
                {
                    //letras
                    totalS = Numalet.ToCardinal(subtotal2.ToString().Replace(',', '.'));
                    if (CAE == "-")
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE_2.rdlc");
                    }
                    else
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE.rdlc");
                    }

                }
                //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                ReportParameter param31 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                ReportParameter param31a = new ReportParameter("ParamNoGravado", conceptos.ToString("C"));//Conc No Grav
                //logo
                //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///C:\Imagen\Logo.jpg");
                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                Log.EscribirSQL(1, "INFO", @"Asigno Ruta file:///" + logo);

                //string imagePath = Server.MapPath("~/images/Facturas/GS_LOGO.png");
                //ReportParameter paramImg = new ReportParameter("ParamImagen", imagePath);                                    

                ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));

                ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                ReportParameter param19 = new ReportParameter("ParamPtoVta", ptoVta);
                ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);
                ReportParameter param21 = new ReportParameter("ParamLetra", letraDoc);
                ReportParameter param22 = new ReportParameter("ParamCodDoc", CodigoDoc);
                ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);
                ReportParameter param24 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());

                ReportParameter param25 = new ReportParameter("ParamCambioDolar", cotizacionFecha);
                ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());

                ReportParameter param40 = new ReportParameter("ParamNroPedido", nroPedido);
                ReportParameter param41 = new ReportParameter("ParamNroRemito", nroRemito);

                ReportParameter param42 = new ReportParameter("ParamCondicionPago", condicionPago);

                //ReportParameter param43 = new ReportParameter("ParamTotalIva105", totalIva105.ToString("C"));
                //ReportParameter param44 = new ReportParameter("ParamTotalIva21", totalIva21.ToString("C"));
                //ReportParameter param45 = new ReportParameter("ParamTotalIva27", totalIva27.ToString("C"));

                if (pv.monedaFacturacion > 1)
                {
                    string cambioMoneda = controladorFactEntity.obtenerDatosIvasFactura(idFactura).TipoCambio.Value.ToString();
                    ReportParameter param46 = new ReportParameter("MonedaOriginal", cambioMoneda);
                    ReportViewer1.LocalReport.SetParameters(param46);
                }

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param03);//sucfacturada
                this.ReportViewer1.LocalReport.SetParameters(param31);
                this.ReportViewer1.LocalReport.SetParameters(param31a);
                this.ReportViewer1.LocalReport.SetParameters(param4);

                this.ReportViewer1.LocalReport.SetParameters(param3b);
                this.ReportViewer1.LocalReport.SetParameters(param4b);

                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);

                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);

                //parametros datos empresa
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);
                this.ReportViewer1.LocalReport.SetParameters(param12);
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);
                this.ReportViewer1.LocalReport.SetParameters(param15);
                this.ReportViewer1.LocalReport.SetParameters(param16);
                this.ReportViewer1.LocalReport.SetParameters(param17);
                this.ReportViewer1.LocalReport.SetParameters(param18);
                this.ReportViewer1.LocalReport.SetParameters(param19);
                this.ReportViewer1.LocalReport.SetParameters(param20);
                this.ReportViewer1.LocalReport.SetParameters(param21);
                this.ReportViewer1.LocalReport.SetParameters(param22);
                //equivalente total dolares
                this.ReportViewer1.LocalReport.SetParameters(param23);
                this.ReportViewer1.LocalReport.SetParameters(param25);
                //param esPreimpresa o no
                this.ReportViewer1.LocalReport.SetParameters(param24);
                //imagen
                this.ReportViewer1.LocalReport.SetParameters(param32);
                //cta cte
                this.ReportViewer1.LocalReport.SetParameters(param33);
                //nro pedido y nro remito relacionados
                this.ReportViewer1.LocalReport.SetParameters(param40);
                this.ReportViewer1.LocalReport.SetParameters(param41);
                this.ReportViewer1.LocalReport.SetParameters(param42);

                //this.ReportViewer1.LocalReport.SetParameters(param43);
                //this.ReportViewer1.LocalReport.SetParameters(param44);
                //this.ReportViewer1.LocalReport.SetParameters(param45);

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
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Factura A. " + ex.Message);
            }
        }

        private void AgregarIvasToTotalesDeFactura(DataTable totalesFactura, Factura factura)
        {
            try
            {
                totalesFactura.Columns.Add("TotalIva105", typeof(decimal));
                totalesFactura.Columns.Add("TotalIva21", typeof(decimal));
                totalesFactura.Columns.Add("TotalIva27", typeof(decimal));

                var facturaIvas = controladorFactEntity.obtenerDatosIvasFactura(factura.id);

                if (facturaIvas != null && totalesFactura.Rows.Count > 0)
                {
                    var filaTotales = totalesFactura.Rows[0];

                    filaTotales["TotalIva105"] = "0.00";
                    filaTotales["TotalIva21"] = "0.00";
                    filaTotales["TotalIva27"] = "0.00";

                    if (facturaIvas.TotalIva105 != null)
                    {
                        filaTotales["TotalIva105"] = facturaIvas.TotalIva105;
                    }
                    if (facturaIvas.TotalNeto21 != null)
                    {
                        filaTotales["TotalIva21"] = facturaIvas.TotalIva21;
                    }
                    if (facturaIvas.TotalIva27 != null)
                    {
                        filaTotales["TotalIva27"] = facturaIvas.TotalIva27;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Ocurrió un error en AgregarIvasToTotalesDeFactura. Excepción: " + ex.Message);
            }
        }

        //factura b
        private void generarReporte4(int idFactura)
        {
            try
            {
                Factura fact = this.controlador.obtenerFacturaId(idFactura);
                DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);

                dtDatos = agregarAlicuotaIVAEnLaDescripcionDeLosArticulos(dtDatos);

                DataTable dtDetalle = controlador.obtenerDetallePresupuesto(idPresupuesto);

                //nro remito factura
                DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(idPresupuesto);

                //levanto los datos de la factura
                var drDatosFactura = dtDetalle.Rows[0];
                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;
                String ingBrutos = String.Empty;
                String fechaInicio = String.Empty;
                String cuitEmpresa = String.Empty;
                String nroFactura = String.Empty;
                String tipoDoc = String.Empty;
                String CAE = String.Empty;
                String ptoVta = String.Empty;
                String codBarra = String.Empty;
                String fechaVto = String.Empty;
                String cotizacionFecha = String.Empty;

                foreach (DataRow row in dtEmpresa.Rows)
                {
                    cuitEmpresa = row.ItemArray[1].ToString();
                    razonSoc = row.ItemArray[2].ToString();
                    ingBrutos = row.ItemArray[3].ToString();
                    fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();                    
                    condIVA = row.ItemArray[5].ToString();
                    direComer = row.ItemArray[6].ToString();
                }

                //datos factura
                string auxNro = drDatosFactura["Numero"].ToString();
                nroFactura = auxNro.Substring(auxNro.Length - 13, 13);

                tipoDoc = auxNro.Substring(0, auxNro.Length - 16);
                if (configuracion.monotributo == "1")
                {
                    if (tipoDoc.Contains("Debito"))
                    {
                        tipoDoc = "Nota de Debito C";
                    }
                    else
                    {
                        if (tipoDoc.Contains("Credito"))
                        {
                            tipoDoc = "Nota de Credito C";
                        }
                        else
                        {
                            tipoDoc = "Factura C";
                        }
                    }
                }


                if (drDatosFactura["Cae"].ToString() != "")
                {
                    CAE = drDatosFactura["Cae"].ToString();
                    //CAE = "-";
                }
                else
                {
                    CAE = "-";
                }
                ptoVta = drDatosFactura["ptoVenta"].ToString();
                fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                if (string.IsNullOrEmpty(codBarra))
                {
                    codBarra = "0";

                }

                //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                PuntoVenta pv = this.controlSucursal.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                int esPreimpresa = 0;
                if (pv != null)
                {
                    if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                    {
                        esPreimpresa = 1;
                    }
                }

                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();

                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                {
                    dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                }

                //datos cotizacion al momento de fc
                if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                {
                    cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                }

                //sucursalfacturada                
                string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                if (sucursalFact != "0")
                {
                    controladorSucursal contSuc = new controladorSucursal();
                    Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                    sucursalFact = "-" + s.nombre;
                }
                else
                {
                    sucursalFact = " ";
                }

                //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                DataTable dtTotales = controlador.obtenerTotalPresupuesto2(idPresupuesto);
                DataRow dr = dtTotales.Rows[0];
                //neto no grabado
                decimal subtotal = Convert.ToDecimal(dr[4]);
                //subtotal menos el descuento
                decimal subtotal2 = Convert.ToDecimal(dr[5]);
                //iva discriminado de la fact
                decimal iva = Convert.ToDecimal(dr[3]);
                subtotal = subtotal + iva;
                //total de la factura
                decimal total = Convert.ToDecimal(dr[2]);
                //retenciones
                decimal retencion = Convert.ToDecimal(dr[6]);
                //percepcion IVA Cons. Final
                decimal percepIVA = Convert.ToDecimal(dr[8]);
                //conceptos no gravados(combustible)
                decimal conceptos = Convert.ToDecimal(dr["iva21"]);
                //decimal descuento = Convert.ToDecimal(dr[1]);
                //sumo el total de items - total de factura y saco el descuento
                DataTable dtDescuento = controlador.obtenerTotalItem2(idPresupuesto);
                decimal descuento = 0;
                foreach (DataRow drr in dtDescuento.Rows)
                {
                    descuento = Convert.ToDecimal(drr[0]);
                }
                descuento = decimal.Round(((descuento + retencion) - total), 2);

                if (Math.Abs(descuento) == Convert.ToDecimal(0.01))
                {
                    descuento = 0;
                }

                //obtengo el saldo de la cuenta corriente del cliente                
                DataTable dt = this.contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), fact.fecha.AddHours(23).ToString("dd/MM/yyyy"), fact.cliente.id, 0, fact.sucursal.id, 0, 0);
                decimal saldoCtaCte = 0;
                try
                {
                    saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                }
                catch { }

                //letras
                string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                //string totalS = Numalet.ToCardinal("18.25");
                //cant unidades
                decimal cant = 2;
                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";
                DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(idPresupuesto);
                if (dtDireccion != null)
                {
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl[0].ToString() == "Legal")
                        {
                            //direLegal = "-";
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                            drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                        if (drl[0].ToString() == "Entrega")
                        {
                            //direEntrega = "";
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                            drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                    }
                }
                if (direLegal != "-" && direEntrega == "-")
                {
                    direEntrega = direLegal;
                }

                //Total equivalente en dolares
                controladorMoneda contMoneda = new controladorMoneda();
                Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                decimal TotalDolares = 0;
                String textoDolares = String.Empty;
                if (dolar != null)
                {
                    TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                }
                if (tipoDoc.Contains("Nota de"))
                {
                    textoDolares = " ";
                }
                else
                {
                    textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                }

                //Condición de Pago
                string condicionPago = String.Empty;
                if (fact.formaPAgo.id == 7)
                {
                    condicionPago = fact.cliente.vencFC.ToString();
                }

                String nroPedido = String.Empty;
                String nroRemito = String.Empty;
                //Pedido Relacionado
                Pedido pedidoFc = this.contPedidos.obtenerPedidoByFacturaID(idPresupuesto);
                if (pedidoFc != null)
                {
                    nroPedido = pedidoFc.numero;
                }
                //Remito Relacionado
                Remito remitoFc = this.controlador.obtenerRemitoByFactura(idPresupuesto);
                if (remitoFc != null)
                {
                    nroRemito = remitoFc.numero;
                }
                //Comentario factura
                DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(idPresupuesto);

                //obtengo id empresa para buscar el logo correspondiente
                int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/Logo.jpg");
                //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/Logo.jpg");
                string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/" + pv.id + "/Logo.jpg");
                Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);
                //codigo barra codBarra
                //Create an instance of Barcode Professional
                BarcodeProfessional bcp = new BarcodeProfessional();
                //Barcode settings                
                bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;
                bcp.BarHeight = 0.25f;
                bcp.Code = codBarra;
                byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                DataTable dtImagen = new DataTable();
                DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));
                dtImagen.Columns.Add(ColumnImagen);
                dtImagen.Rows.Add(prodBarcode);
                //Generate the barcode image and store it into the Barcode Column

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRB.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPresupuesto", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                ReportParameter param3a = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                ReportParameter param3a2 = new ReportParameter("ParamPercepIVA", percepIVA.ToString("C"));//percepIVA
                ReportParameter param3a3 = new ReportParameter("ParamNoGravado", conceptos.ToString("C"));//Conc No Grav
                ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                param3b.Visible = false;
                ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));
                param4b.Visible = false;
                ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);
                ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);
                ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                //parametros Datos empresa,cae y doc
                ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer+" \n Direccion Sucursal: "+pv.direccion);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                ReportParameter param19 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());
                ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);
                ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);
                ReportParameter param23b = new ReportParameter("ParamCambioDolar", cotizacionFecha);
                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());

                ReportParameter param40 = new ReportParameter("ParamNroPedido", nroPedido);
                ReportParameter param41 = new ReportParameter("ParamNroRemito", nroRemito);
                ReportParameter param42 = new ReportParameter("ParamCondicionPago", condicionPago);


                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param3a);//retencion
                this.ReportViewer1.LocalReport.SetParameters(param3a2);//percepcion iva cf
                this.ReportViewer1.LocalReport.SetParameters(param3a3);//
                this.ReportViewer1.LocalReport.SetParameters(param03);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param3b);
                this.ReportViewer1.LocalReport.SetParameters(param4b);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);

                //parametros datos empresa
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);
                this.ReportViewer1.LocalReport.SetParameters(param12);
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);
                this.ReportViewer1.LocalReport.SetParameters(param15);
                this.ReportViewer1.LocalReport.SetParameters(param16);
                this.ReportViewer1.LocalReport.SetParameters(param17);
                this.ReportViewer1.LocalReport.SetParameters(param18);
                this.ReportViewer1.LocalReport.SetParameters(param19);
                this.ReportViewer1.LocalReport.SetParameters(param20);
                this.ReportViewer1.LocalReport.SetParameters(param23);
                this.ReportViewer1.LocalReport.SetParameters(param23b);
                this.ReportViewer1.LocalReport.SetParameters(param32);
                this.ReportViewer1.LocalReport.SetParameters(param33);
                //nro pedido y nro remito relacionados
                this.ReportViewer1.LocalReport.SetParameters(param40);
                this.ReportViewer1.LocalReport.SetParameters(param41);
                this.ReportViewer1.LocalReport.SetParameters(param42);
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
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de factura B. " + ex.Message);
            }
        }
        //Remito
        private void generarReporte5(int idRemito)
        {
            try
            {
                DataTable dtDatos = controlador.obtenerDetalleRemito(idRemito);

                DataTable dtDetalle = controlador.obtenerDatosRemito(idRemito);

                DataTable dtNroFacturas = controlador.obtenerNroFacturaByRemito(idRemito);

                controladorMoneda contMoneda = new controladorMoneda();

                List<Moneda> dtMonedas = contMoneda.obtenerMonedas();

                decimal euro = 0;
                int sucursalFacturada = 0;
                Sucursal sucursalOrigen = new Sucursal();
                Sucursal sucursalRemitida = new Sucursal();
                //Comentario factura
                DataTable dtComentariosFactura = new DataTable();

                //Si el remito es de una factura.
                if (dtNroFacturas.Rows.Count > 0)
                {
                    //busco comentario con el idfactura que esta en el remito
                    int nroFactura = Convert.ToInt32(dtNroFacturas.Rows[0].ItemArray[1].ToString());
                    dtComentariosFactura = this.controlador.obtenerComentarioPresupuesto(nroFactura);

                    //obtengo el numero de la suc a la que se movio stock
                    string sucFact = dtNroFacturas.Rows[0]["SucursalFacturada"].ToString();

                    if (!String.IsNullOrEmpty(sucFact))
                    {
                        sucursalFacturada = Convert.ToInt32(sucFact);
                    }

                    ////si el comentario de la factura tiene la fecha en null o vacio, elimino la fila para que no se muestre la tabla en el report.
                    //if (dtComentariosFactura.Rows[0].ItemArray[2].ToString() == null || dtComentariosFactura.Rows[0].ItemArray[2].ToString() == "")
                    //{
                    //    dtComentariosFactura.Rows.Remove(dtComentariosFactura.Rows[0]);
                    //}                    
                }

                //suc de donde se hizo
                int idSuc = Convert.ToInt32(dtDetalle.Rows[0]["sucursal"].ToString());
                sucursalOrigen = this.controlSucursal.obtenerSucursalID(idSuc);

                //suc remitida
                if (sucursalFacturada > 0)
                {
                    sucursalRemitida = this.controlSucursal.obtenerSucursalID(sucursalFacturada);
                }

                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();
                int idCliente = Convert.ToInt32(srCliente["idCliente"].ToString());

                //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                DataTable dtTotales = controlador.obtenerTotalRemito(idRemito);
                DataRow dr = dtTotales.Rows[0];

                //Busco senias de los pedidos remitidos
                decimal senia = controlRemito.obtenerSeniaRemito(idRemito);
                string seniaLetras = Numalet.ToCardinal(senia);

                //neto no grabado
                decimal subtotal = Convert.ToDecimal(dr[4]);
                decimal neto = Convert.ToDecimal(dr[4]);

                decimal descuento = Convert.ToDecimal(dr[1]);

                //subtotal menos el descuento
                decimal subtotal2 = Convert.ToDecimal(dr[5]);

                //iva discriminado de la fact
                decimal iva = Convert.ToDecimal(dr[3]);

                subtotal = subtotal + iva;

                //total de la factura
                decimal total = Convert.ToDecimal(dr[2]);

                //letras
                string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                //string totalS = Numalet.ToCardinal("18.25");

                //cant unidades
                decimal cant = 2;

                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";

                DataTable dtDireccion = controlCliente.obtenerDireccionesById(idCliente);
                if (dtDireccion != null)
                {
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl["nombre"].ToString() == "Legal")
                        {
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                        if (drl["nombre"].ToString() == "Entrega")
                        {
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                    }
                }

                controladorRemitos contRemito = new controladorRemitos();
                //obtengo id empresa para buscar el logo correspondiente    
                Remito r = contRemito.obtenerRemitoId(idRemito);
                //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc +"/Logo.jpg");                
                string logo = Server.MapPath("../../Facturas/" + sucursalOrigen.empresa.id + "/" + sucursalOrigen.id + "/" + r.ptoV.id + "/Logo.jpg");
                Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);

                //datos cai
                string cai = "";
                string fechaVencCai = "";
                try
                {
                    var pv = this.controlSucursal.obtenerPuntoVentaPV(r.ptoV.puntoVenta.PadLeft(4, '0'), r.sucursal.id, r.empresa.id);
                    cai = pv.caiRemito;
                    fechaVencCai = pv.caiVencimiento.ToString("dd/MM/yyyy");

                    euro = dtMonedas.Where(x => x.moneda == "Euro").FirstOrDefault().cambio;
                }
                catch
                { }



                DataTable dtComentarios = contRemito.obtenerComentarioRemito(idRemito);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RemitoR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPresupuesto", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("DetalleComentarios", dtComentarios);
                ReportDataSource rds4 = new ReportDataSource("NumerosFacturas", dtNroFacturas);
                ReportDataSource rds5 = new ReportDataSource("DetalleComentariosFactura", dtComentariosFactura);

                //ReportDataSource rds3 = new ReportDataSource("TotalPresupuesto", dtTotal);
                ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));

                ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                param3b.Visible = false;
                ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));
                param4b.Visible = false;

                ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                ReportParameter param9 = new ReportParameter("ParamSenia", "Seña: $" + senia.ToString());
                ReportParameter param10 = new ReportParameter("ParamSeniaLetras", seniaLetras);

                ReportParameter param11 = new ReportParameter("ParamSucursalRemitida", sucursalRemitida.nombre);

                ReportParameter param12 = new ReportParameter("ParamReimprime", this.original.ToString());

                ReportParameter param13 = new ReportParameter("ParamSucursal", sucursalOrigen.nombre);

                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                ReportParameter param33 = new ReportParameter();
                //cot
                if (configuracion.cot == "1")
                {
                    try
                    {
                        ControladorRemitoEntity contRemEntity = new ControladorRemitoEntity();
                        var rd = contRemEntity.obtenerRemitoDatosByRemito(idRemito);
                        param33 = new ReportParameter("COT", rd.COT + " CODIGO UNICO " + rd.CodUnico);
                    }
                    catch
                    {

                    }
                }

                string imagen = this.generarCodigo(idRemito);
                ReportParameter param34 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);

                ReportParameter param35 = new ReportParameter("CAI", cai);

                ReportParameter param36 = new ReportParameter("CAIVencimiento", fechaVencCai);

                ReportParameter param37 = new ReportParameter("Neto", neto.ToString("C"));

                ReportParameter param38 = new ReportParameter("Euro", euro.ToString("C"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);

                this.ReportViewer1.LocalReport.SetParameters(param3b);
                this.ReportViewer1.LocalReport.SetParameters(param4b);

                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);

                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);

                this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);
                this.ReportViewer1.LocalReport.SetParameters(param12);
                this.ReportViewer1.LocalReport.SetParameters(param13);

                this.ReportViewer1.LocalReport.SetParameters(param32);

                this.ReportViewer1.LocalReport.SetParameters(param33);
                this.ReportViewer1.LocalReport.SetParameters(param34);
                this.ReportViewer1.LocalReport.SetParameters(param35);
                this.ReportViewer1.LocalReport.SetParameters(param36);
                this.ReportViewer1.LocalReport.SetParameters(param37);
                this.ReportViewer1.LocalReport.SetParameters(param38);

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
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Remito. " + ex.Message);
            }
        }

        private void generarReporte6(int idPresupuesto)
        {
            try
            {
                controladorCompraEntity contCompra = new controladorCompraEntity();
                ControladorArticulosEntity contArticulosEnt = new ControladorArticulosEntity();
                controladorArticulo contArticulo = new controladorArticulo();

                Factura fact = this.controlador.obtenerFacturaId(idPresupuesto);
                //obtengo detalle de items
                //DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                DataTable dtDetalle = controlador.obtenerDetallePresupuesto(idPresupuesto);


                //datos cliente
                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();
                string cliente = srCliente[0].ToString();
                string dni = srCliente[1].ToString();
                if (dni.Length > 8)
                {
                    dni = dni.Substring(2, 8);
                }


                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";
                string direccion = "";
                DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(idPresupuesto);
                foreach (DataRow drl in dtDireccion.Rows)
                {
                    if (drl[0].ToString() == "Legal")
                    {
                        direLegal = drl[1].ToString() + " " + drl[2].ToString() + " ";
                        //+ drl[3].ToString() + " " +
                        //drl[4].ToString();
                    }
                    //if (drl[0].ToString() == "Entrega")
                    //{
                    //    direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                    //        drl[4].ToString();
                    //}
                }
                //if (direEntrega != "-")
                //    direccion = direEntrega;
                if (direLegal != "-")
                    direccion = direLegal;


                //datos de trazabilidad
                DataTable traza = null;
                string modelo = "";
                string motor = "";
                string chasis = "";

                //datos articulo
                string marca = "";
                string descripcion = "";

                var rem = controlador.obtenerRemitoByFactura(this.idPresupuesto);

                foreach (var item in fact.items)
                {
                    if (item.articulo.grupo.descripcion == "MOTOCICLETAS" || item.articulo.grupo.descripcion == "MOTOS")
                    {
                        traza = contCompra.obtenerTrazabilidadVendidasItemByArticuloDoc(item.articulo.id, fact.sucursal.id, rem.id, rem.tipo.id);

                        var marc = contArticulosEnt.obtenerMarcaByArticulo(item.articulo.id);
                        marca = marc.Marca.marca1;
                        var art = contArticulo.obtenerArticuloId(item.articulo.id);
                        descripcion = art.descripcion;

                        foreach (DataRow drTraza in traza.Rows)
                        {
                            if (drTraza["nombre"].ToString().ToLower() == "motor")
                                motor = drTraza["valor"].ToString();
                            if (drTraza["nombre"].ToString().ToLower() == "chasis")
                                chasis = drTraza["valor"].ToString();
                            if (drTraza["nombre"].ToString().ToLower() == "modelo-año")
                                modelo = drTraza["valor"].ToString();
                        }
                    }
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RFormulario12.rdlc");

                ReportParameter param = new ReportParameter("ParamMarca", marca);
                ReportParameter param2 = new ReportParameter("ParamModelo", modelo);
                ReportParameter param3 = new ReportParameter("ParamMotor", motor);
                ReportParameter param4 = new ReportParameter("ParamChasis", chasis);
                ReportParameter param5 = new ReportParameter("ParamSucursal", ".............");
                ReportParameter param6 = new ReportParameter("ParamFecha", rem.fecha.ToString("dd/MM/yyyy"));
                ReportParameter param7 = new ReportParameter("ParamCliente", cliente);
                ReportParameter param8 = new ReportParameter("ParamDireccion", direccion);
                //ReportParameter param9 = new ReportParameter("ParamLocalidad", "LocalidadPrueba");
                ReportParameter param10 = new ReportParameter("ParamDNI", dni);
                ReportParameter param11 = new ReportParameter("ParamDescripcion", descripcion);


                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);
                //this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);


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
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }

        private int obtenerSeñaRemito(int idRemito)
        {
            try
            {
                controladorRemitos contR = new controladorRemitos();


                return 1;
            }
            catch (Exception ex)
            {
                return -1;
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
                String path = HttpContext.Current.Server.MapPath("/Remitos/" + idRemito + "/");
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
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para pedido. " + ex.Message);
                return null;
            }
        }

        private DataTable obtenerDTarticulos(DataTable dataTable)
        {
            try
            {
                dataTable = this.traerDataTableSiEsModoFacturaUnidadDeMedida(dataTable);
                dataTable = this.agregarAlicuotaIVAEnLaDescripcionDeLosArticulos(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error en fun: obtenerDTarticulos. " + ex.Message);
                return dataTable;
            }
        }

        private DataTable traerDataTableSiEsModoFacturaUnidadDeMedida(DataTable dataTable)
        {
            try
            {
                string facturaPorUnidadDeMedida = WebConfigurationManager.AppSettings.Get("FormularioFC");
                if (!string.IsNullOrEmpty(facturaPorUnidadDeMedida) && facturaPorUnidadDeMedida == "2")
                {
                    dataTable = controlador.obtenerDatosPresupuestoUnidadDeMedida(idPresupuesto);
                    return dataTable;
                }
                else
                {
                    //obtengo detalle de items por defecto
                    dataTable = controlador.obtenerDatosPresupuesto(idPresupuesto);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error en fun: traerDataTableSiEsModoFacturaUnidadDeMedida. " + ex.Message);
                return null;
            }
        }

        private DataTable agregarAlicuotaIVAEnLaDescripcionDeLosArticulos(DataTable tablaArticulos)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.configuracion.MostrarAlicuotaIVAenDescripcionArticulosDeFacturas)
                    && this.configuracion.MostrarAlicuotaIVAenDescripcionArticulosDeFacturas == "1")
                {
                    ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
                    foreach (DataRow row in tablaArticulos.Rows)
                    {
                        Gestion_Api.Entitys.articulo articulo = contArticulosEntity.obtenerArticuloEntityByCod(row["Codigo"].ToString());
                        row["Descripcion"] += " (" + articulo.porcentajeIva.ToString() + ")";
                    }
                }
                return tablaArticulos;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error en fun: agregarAlicuotaIVAEnLaDescripcionDeLosArticulos. " + ex.Message);
                return tablaArticulos;
            }
        }
    }
}