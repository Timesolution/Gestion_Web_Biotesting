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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Cobros
{
    public partial class ImpresionCobro : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int idCobro;
        private int valor;
        private string fechaD;
        private string fechaH;
        private int idCliente;
        private int idVendedor;
        private int idSucursal;
        private int idPuntoVta;
        private int idEmpresa;
        private int idTipo;
        private int excel;
        private int impagasVencidas;
        private string listaCobros;

        controladorCobranza controlador = new controladorCobranza();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idCobro = Convert.ToInt32(Request.QueryString["Cobro"]);
                    this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                    this.fechaD = Request.QueryString["fd"];
                    this.fechaH = Request.QueryString["fh"];
                    this.idCliente = Convert.ToInt32(Request.QueryString["cli"]);
                    this.idVendedor = Convert.ToInt32(Request.QueryString["ven"]);
                    this.idSucursal = Convert.ToInt32(Request.QueryString["suc"]);
                    this.idPuntoVta = Convert.ToInt32(Request.QueryString["pv"]);
                    this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                    this.idTipo = Convert.ToInt32(Request.QueryString["t"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.impagasVencidas = Convert.ToInt32(Request.QueryString["vencida"]);
                    this.listaCobros = Request.QueryString["lc"];

                    if (!string.IsNullOrEmpty(this.listaCobros))
                    {
                        this.listaCobros = this.listaCobros.Remove(this.listaCobros.Length - 1);
                    }

                    if (valor == 1)
                    {
                        this.generarReporte2(idCobro);
                    }
                    if (valor == 2)
                    {
                        this.generarReporte3(idCobro);
                    }
                    if (valor == 3)// reporte impagas
                    {
                        this.generarReporte4();
                    }
                    if (valor == 4)//reporte detallado de impagas
                    {
                        this.generarReporte5();
                    }
                    if (valor == 5)//impagas detallado a excel
                    {
                        this.generarReporte6();
                    }
                    if (valor == 6)// cobros realizados a excel
                    {
                        this.generarReporte7();
                    }
                    if (valor == 7)// reporte cobranza x vendedor
                    {
                        this.generarReporte8();
                    }
                    if (valor == 8)
                    {
                        this.generarReporte9();
                    }
                    if (valor == 9)//impagas x vendedor
                    {
                        this.generarReporte10();
                    }
                    if (valor == 10) //reporte detalle cobros
                    {
                        this.generarReporte11(listaCobros);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pedido. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pedido. " + ex.Message);
            }
        }

        private void generarReporte2(int idCobro)
        {
            try
            {
                DataTable dtDatos = controlador.obtenerDatosCobro(idCobro);
                DataTable dtDetalle = controlador.obtenerDetalleCobro(idCobro);
                DataTable dtTotal = controlador.obtenerTotalCobro(idCobro);
                DataTable dtDocumentos = controlador.obtenerDocumentosCancelados(idCobro);
                DataTable dtCheques = controlador.obtenerDetalleCheques(idCobro);
                DataTable dtTransferencia = controlador.obtenerDetalleTransferencia(idCobro);
                //datos empresa emisora
                DataRow drEmpresa = dtDatos.Rows[0];
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();
                String razon = drEmpresa["Empresa"].ToString();
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaByRazon(razon);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (drEmpresa["tipo_doc"].ToString() == "15")//RC Cobro - FC
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Cobros.rdlc");
                }
                else//RC Cobro - PRP
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CobrosPRP.rdlc");
                }

                ReportDataSource rds = new ReportDataSource("DetalleCobro", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosCobro", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("SumaCobros", dtTotal);
                ReportDataSource rds4 = new ReportDataSource("DocumentosCancelados", dtDocumentos);
                ReportDataSource rds5 = new ReportDataSource("DetalleCheques", dtCheques);
                ReportDataSource rds6 = new ReportDataSource("DetalleTransferencia", dtTransferencia);
                ReportDataSource rds7 = new ReportDataSource("DatosEmpresa", dtEmpresa);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.DataSources.Add(rds6);
                //empresa
                this.ReportViewer1.LocalReport.DataSources.Add(rds7);

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
            catch
            {

            }
        }
        private void generarReporte3(int idCobro)
        {
            try
            {
                Movimiento_Cobro mov = this.controlador.obtenerMovimientoCobroByIDCobro(idCobro);                
                
                DataTable dtDatos = controlador.obtenerDatosCobro(mov.id);
                DataTable dtDetalle = controlador.obtenerDetalleCobro(mov.id);
                DataTable dtTotal = controlador.obtenerTotalCobro(mov.id);
                DataTable dtDocumentos = controlador.obtenerDocumentosCancelados(mov.id);
                DataTable dtCheques = controlador.obtenerDetalleCheques(mov.id);
                DataTable dtTransferencia = controlador.obtenerDetalleTransferencia(mov.id);

                try
                {
                    DataTable dtContado = this.controlador.obtenerPagosEfectivoDetalleDTByCobro(idCobro);
                    if (dtContado != null)
                    {
                        if (dtContado.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtContado.Rows)
                            {
                                var rowEfectivo = dtDetalle.AsEnumerable().Where(x => x["Tipo"].ToString() == "Efectivo").FirstOrDefault();
                                if(rowEfectivo != null)
                                {
                                    rowEfectivo["Tipo"] = row["Tipo"];
                                }
                            }
                        }
                    }
                }
                catch { }  

                //datos empresa emisora
                DataRow drEmpresa = dtDatos.Rows[0];
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();
                String razon = drEmpresa["Empresa"].ToString();
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaByRazon(razon);

                String comentarios = String.Empty;
                comentarios = mov.cob.comentarios;


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Cobros.rdlc");
                if (drEmpresa["tipo_doc"].ToString() == "15")//RC Cobro - FC
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Cobros.rdlc");
                }
                else//RC Cobro - PRP
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CobrosPRP.rdlc");
                }
                ReportDataSource rds = new ReportDataSource("DetalleCobro", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosCobro", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("SumaCobros", dtTotal);
                ReportDataSource rds4 = new ReportDataSource("DocumentosCancelados", dtDocumentos);
                ReportDataSource rds5 = new ReportDataSource("DetalleCheques", dtCheques);
                ReportDataSource rds6 = new ReportDataSource("DetalleTransferencia", dtTransferencia);
                ReportDataSource rds7 = new ReportDataSource("DatosEmpresa", dtEmpresa);
                ReportParameter param = new ReportParameter("ParamComentarios", comentarios);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                this.ReportViewer1.LocalReport.DataSources.Add(rds6);
                this.ReportViewer1.LocalReport.DataSources.Add(rds7);
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
            catch
            {

            }
        }
        private void generarReporte4()
        {
            try
            {
                //DataTable dtDatos2 = Session["datosMov"] as DataTable;
                //Session["datosMov"] = null;

                Decimal saldoTotal = 0;
                DataTable dtDatos2 = controlador.obtenerTablaTopClientes(this.fechaD, this.fechaH, this.idCliente, this.idVendedor, this.idSucursal, this.idTipo, this.impagasVencidas);
                
                foreach (DataRow row in dtDatos2.Rows)
                {
                    //String[] saldo = row.ItemArray[2].ToString().Split('$');
                    //saldoTotal += Convert.ToDecimal(saldo[1]);
                    saldoTotal += Convert.ToDecimal(row["importe"].ToString());
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Impagas.rdlc");

                ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());
                ReportDataSource rds = new ReportDataSource("dsImpaga", dtDatos2);
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

                    String filename = string.Format("{0}.{1}", "Impagas", "xls");

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
                controladorCuentaCorriente controlador = new controladorCuentaCorriente();
                controladorCliente contCliente = new controladorCliente();
                controladorVendedor contVendedor = new controladorVendedor();

                DataTable dtImpagas = controlador.obtenerMovimientosImpagas(this.fechaD, this.fechaH, this.idSucursal, this.idCliente, this.idVendedor, this.idTipo);
                dtImpagas.Columns.Add("codigoCliente");
                dtImpagas.Columns.Add("Telefono");
                decimal saldoAcum = 0;
                if (dtImpagas.Rows.Count > 0)
                {
                    foreach (DataRow row in dtImpagas.Rows)
                    {
                        Gestor_Solution.Modelo.Cliente cAux = contCliente.obtenerClienteID(Convert.ToInt32(row["cliente"]));
                        if(cAux!=null)
                        {
                            row["codigoCliente"] = cAux.codigo;
                        }
                        else
                        {
                            row["codigoCliente"] = "";
                        }
                       
                        DataTable dtTelefono = contCliente.obtenerContactoCliente(Convert.ToInt32(row["cliente"]), 1);
                        if (dtTelefono.Rows.Count > 0)
                        {
                            row["Telefono"] = dtTelefono.Rows[0].ItemArray[1].ToString();
                        }
                        ////saldo acum saldoAcumulado
                        //var saldo = Convert.ToDecimal(row["saldo"]);

                        //saldoAcum = saldoAcum + saldo;
                        //row["saldoAcumulado"] = saldoAcum; 

                    }
                }

                String vendedor = "Todos";

                if (this.idVendedor > 0)
                {
                    Vendedor vend = contVendedor.obtenerVendedorID(this.idVendedor);
                    vendedor = vend.emp.nombre + " " + vend.emp.apellido;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleImpagas.rdlc");

                ReportDataSource rds = new ReportDataSource("dsImpagas", dtImpagas);
                ReportParameter param = new ReportParameter("ParamVendedor", vendedor);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
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
        //Exportar excel
        private void generarReporte6()
        {
            try
            {
                controladorCuentaCorriente controlador = new controladorCuentaCorriente();
                controladorVendedor contVendedor = new controladorVendedor();
                controladorCliente contCliente = new controladorCliente();

                DataTable dtImpagas = controlador.obtenerMovimientosImpagas(this.fechaD, this.fechaH, this.idSucursal, this.idCliente, this.idVendedor, this.idTipo);
                dtImpagas.Columns.Add("codigoCliente");

                if (dtImpagas.Rows.Count > 0)
                {
                    foreach (DataRow row in dtImpagas.Rows)
                    {
                        Gestor_Solution.Modelo.Cliente cAux = contCliente.obtenerClienteID(Convert.ToInt32(row["cliente"]));
                        if (cAux != null)
                        {
                            row["codigoCliente"] = cAux.codigo;
                        }
                        else
                            row["codigoCliente"] = "";
                        
                    }
                }

                String vendedor = "Todos";

                if (this.idVendedor > 0)
                {
                    Vendedor vend = contVendedor.obtenerVendedorID(this.idVendedor);
                    vendedor = vend.emp.nombre + " " + vend.emp.apellido;
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleImpagas.rdlc");

                ReportDataSource rds = new ReportDataSource("dsImpagas", dtImpagas);
                ReportParameter param = new ReportParameter("ParamVendedor", vendedor);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;


                string[] streams;

                //get pdf content

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "DetalleImpagas", "xls");


                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                this.Response.BinaryWrite(pdfContent);

                this.Response.End();

            }
            catch (Exception ex)
            {

            }
        }
        private void generarReporte7()
        {
            try
            {
                //DataTable dtCobrosRealizados2 = Session["datosRc"] as DataTable;
                //Session["datosRc"] = null;
                String lblSaldo = Session["saldoRc"] as String;
                Session["saldoRc"] = null;

                string[] fecha = fechaD.Split('/');
                string nuevaFechaD = fecha[2] + fecha[1] + fecha[0] + " 00:00";

                string[] fecha2 = fechaH.Split('/');
                string nuevaFechaH = fecha2[2] + fecha2[1] + fecha2[0] + " 23:59:59.997";

                //con el dt traido del controlador no tengo el cliente
                controladorCuentaCorriente controlador = new controladorCuentaCorriente();
                DataTable dtCobrosRealizados = controlador.obtenerCobrosRealizadosDT(nuevaFechaD, nuevaFechaH, this.idCliente, this.idPuntoVta, this.idEmpresa, this.idSucursal, this.idTipo, this.idVendedor);
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CobrosRealizadosR.rdlc");

                ReportDataSource rds = new ReportDataSource("dsCobros", dtCobrosRealizados);
                ReportParameter param = new ReportParameter("ParamSaldo", lblSaldo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;


                string[] streams;

                //get pdf content

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "DetalleCobros", "xls");


                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                this.Response.BinaryWrite(pdfContent);

                this.Response.End();

            }
            catch (Exception ex)
            {

            }
        }
        private void generarReporte8()
        {
            try
            {
                controladorFactEntity contFactEnt = new controladorFactEntity();
                controladorFacturacion contFact = new controladorFacturacion();
                controladorVendedor contVendedor = new controladorVendedor();
                controladorCuentaCorriente contCtaCte = new controladorCuentaCorriente();

                DateTime fdesde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23.9);
                int idCta = 0;
                if (this.idCliente > 0)
                {
                    CuentaCorriente cc = contCtaCte.obtenerCuentaCorrienteCliente(this.idCliente);
                    if (cc != null)
                        idCta = cc.id;
                }

                List<Movimiento_CuentaEnt> cobradas = contFactEnt.obtenerFacturasCobradas(fdesde, fhasta, this.idSucursal, idCta);
                List<Factura> fcCobrados = new List<Factura>();

                DataTable dt = new DataTable();
                dt.Columns.Add("FechaFc");
                dt.Columns.Add("Documento");
                dt.Columns.Add("Cliente");
                dt.Columns.Add("FechaCancelacion");
                dt.Columns.Add("Vendedor");
                dt.Columns.Add("TotalFc", typeof(decimal));
                dt.Columns.Add("Importe", typeof(decimal));
                dt.Columns.Add("ImporteComision", typeof(decimal));
                dt.Columns.Add("PorcentajeComision", typeof(decimal));
                dt.Columns.Add("Cobro");

                foreach (Movimiento_CuentaEnt cob in cobradas)
                {
                    Factura f = contFact.obtenerFacturaId(cob.id_doc.Value);
                    if (f != null)
                    {
                        fcCobrados.Add(f);
                    }
                }

                if (this.idPuntoVta > 0)
                    fcCobrados = fcCobrados.Where(x => x.ptoV.id == this.idPuntoVta).ToList();
                if (this.idVendedor > 0)
                    fcCobrados = fcCobrados.Where(x => x.vendedor.id == this.idVendedor).ToList();
                if (this.idTipo == 1)//FC
                    fcCobrados = fcCobrados.Where(x => x.tipo.id != 17 || x.tipo.id != 11 || x.tipo.id != 12).ToList();
                if (this.idTipo == 2)//PRP-NC PRP-ND PRP
                    fcCobrados = fcCobrados.Where(x => x.tipo.id == 17 || x.tipo.id == 11 || x.tipo.id == 12).ToList();

                foreach (Factura f in fcCobrados)
                {
                    DataRow row = dt.NewRow();

                    row["FechaFc"] = f.fecha.ToString("dd/MM/yyyy");
                    row["FechaCancelacion"] = cobradas.Where(x => x.id_doc == f.id).FirstOrDefault().Movimiento_Cuenta_Cancelaciones.FechaCancelacion.Value.ToString("dd/MM/yyyy");
                    row["TotalFc"] = f.total;
                    //row["Importe"] = f.netoNGrabado;
                    row["Importe"] = f.subTotal;
                    if (f.tipo.tipo.Contains("Credito"))
                    {
                        row["TotalFc"] = f.total * -1;
                        //row["Importe"] = f.netoNGrabado * -1;
                        row["Importe"] = f.subTotal * -1;
                    }
                    
                    row["Documento"] = f.tipo.tipo + "" + f.numero;
                    row["Cliente"] = f.cliente.razonSocial;

                    DataTable cobros = this.controlador.obtenerCobrosByIdMovimientoFactura(cobradas.Where(x => x.id_doc == f.id).FirstOrDefault().id);
                    if (cobros != null)
                    {
                        if (cobros.Rows.Count > 0)
                        {
                            row["Cobro"] = "Cobro Nº " + cobros.Rows[0]["numero"].ToString();
                        }
                    }

                    Vendedor v = contVendedor.obtenerVendedorID(f.vendedor.id);
                    if (v != null)
                    {
                        row["Vendedor"] = v.emp.nombre + " " + v.emp.apellido;
                        row["ImporteComision"] = decimal.Round(Convert.ToDecimal(row["Importe"]) * (v.comision / 100), 2);
                        row["PorcentajeComision"] = v.comision;
                    }
                    dt.Rows.Add(row);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CobrosVendedoresR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosVendedores", dt);
                //ReportParameter param = new ReportParameter("ParamSaldo", "saldo");

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                if (this.excel == 1)
                {
                    Warning[] warnings;
                    string mimeType, encoding, fileNameExtension;
                    string[] streams;
                    //get xls content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "DetalleCobros_Vendedores", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }
                else
                {
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
            }
            catch
            {

            }
        }
        private void generarReporte9()
        {
            try
            {
                DateTime quince = DateTime.Today.AddDays(15);
                DateTime treinta = DateTime.Today.AddDays(30);
                DateTime cuarenta = DateTime.Today.AddDays(45);
                DateTime sesenta = DateTime.Today.AddDays(60);
                
                DataTable dt = this.controlador.obtenerImpagasClientesByRango(this.fechaD, this.fechaH, idCliente, idVendedor, this.idSucursal, this.idTipo, this.impagasVencidas);
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ImpagasVencimientos.rdlc");

                //ReportParameter param = new ReportParameter("ParamSaldo", saldoTotal.ToString());

                ReportDataSource rds = new ReportDataSource("dsImpaga15", dt);

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

                    String filename = string.Format("{0}.{1}", "Impagas", "xls");

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
        private void generarReporte10()
        {
            try
            {
                controladorCuentaCorriente controlador = new controladorCuentaCorriente();
                controladorCliente contCliente = new controladorCliente();
                controladorVendedor contVendedor = new controladorVendedor();

                DataTable dtImpagas = controlador.obtenerMovimientosImpagas(this.fechaD, this.fechaH, this.idSucursal, this.idCliente, this.idVendedor, this.idTipo);
                dtImpagas.Columns.Add("Telefono");
                dtImpagas.Columns.Add("Direccion");

                if (dtImpagas.Rows.Count > 0)
                {
                    foreach (DataRow row in dtImpagas.Rows)
                    {
                        DataTable dtTelefono = contCliente.obtenerContactoCliente(Convert.ToInt32(row["cliente"]), 1);
                        var dtDireccion = contCliente.obtenerDireccionCliente(Convert.ToInt32(row["cliente"]), 1);

                        if (dtTelefono.Rows.Count > 0)
                            row["Telefono"] = dtTelefono.Rows[0].ItemArray[1].ToString();

                        //Verifico que el cliente tenga direccion cargadas, y lo recorro
                        if (dtDireccion != null && dtDireccion.Rows.Count > 0)
                        {
                            Log.EscribirSQL(1,"ERROR", "Voy agregar la direccion del cliente con id: " + Convert.ToInt32(row["cliente"]));
                            row["Direccion"] = dtDireccion.Rows[0]["direccion"] + ", " + dtDireccion.Rows[0]["localidad"];

                            //Si el cliente tiene mas de una direccion, lo concateno con el resto
                            if (dtDireccion.Rows.Count > 1)
                            {
                                //Log.EscribirSQL(1,"ERROR", "El cliente con id: " + Convert.ToInt32(row["cliente"].ToString() + " tiene mas direcciones, las voy agregar"));
                                for (int i = 1; i < dtDireccion.Rows.Count; i++)
                                {
                                    row["Direccion"] += " | " + dtDireccion.Rows[i]["direccion"] + ", " + dtDireccion.Rows[i]["localidad"];
                                }
                            }

                        }
                    }
                }

                String vendedor = "Todos";

                if (this.idVendedor > 0)
                {
                    Vendedor vend = contVendedor.obtenerVendedorID(this.idVendedor);
                    vendedor = vend.emp.nombre + " " + vend.emp.apellido;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleImpagasVendedor.rdlc");

                ReportDataSource rds = new ReportDataSource("dsImpagas", dtImpagas);
                ReportParameter param = new ReportParameter("ParamVendedor", vendedor);

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

                    String filename = string.Format("{0}.{1}", "Impagas", "xls");

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
        private void generarReporte11(string listaCobros)
        {
            try
            {
                DataTable dt = this.generarDetalleCobrosDT(listaCobros);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetalleCobros.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleCobros", dt);

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

                    String filename = string.Format("{0}.{1}", "DetalleCobros", "xls");

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
        private DataTable generarDetalleCobrosDT(string listaCobros)
        {
            try
            {
                DataTable dt = this.controlador.obtenerDetalleCobrosDT(listaCobros);

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