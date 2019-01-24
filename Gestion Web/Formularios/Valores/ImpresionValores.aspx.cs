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

namespace Gestion_Web.Formularios.Valores
{
    public partial class ImpresionValores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        ControladorBanco contBanco = new ControladorBanco();

        private int suc;
        private int ptoVenta;
        private string fechaD;
        private string fechaH;
        private int tipoPago;
        private int tipoMovimiento;   
        //busqueda cheques
        private int idCliente;
        private string fechaDCobro;
        private string fechaHCobro;
        private string fechaDImp;
        private string fechaHImp;
        private int origen;
        private int estadoCh;
        private int idProveedor;
        private int idSucPago;
        //busqueda tarjetas
        private int estado;
        private string nombreTarjeta;
        private int tipoFecha;
        private int operador;
        private int origenPago;
        //Pagares
        private int idMutual;

        private int valor;
        private int group;
        private int accion;
        private int idCaja;
        private int idRemesa;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.fechaD = Request.QueryString["FD"];
                    this.fechaH = Request.QueryString["FH"];
                    this.fechaDCobro = Request.QueryString["fdC"];
                    this.fechaHCobro = Request.QueryString["fhC"];
                    this.fechaDImp = Request.QueryString["fdI"];
                    this.fechaHImp = Request.QueryString["fhI"];

                    this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                    this.idProveedor = Convert.ToInt32(Request.QueryString["Prov"]);
                    this.origen = Convert.ToInt32(Request.QueryString["o"]);
                    this.estadoCh = Convert.ToInt32(Request.QueryString["e"]);
                    this.idSucPago = Convert.ToInt32(Request.QueryString["SP"]);

                    this.suc = Convert.ToInt32(Request.QueryString["S"]);
                    this.ptoVenta = Convert.ToInt32(Request.QueryString["PV"]);
                    this.tipoPago = Convert.ToInt32(Request.QueryString["TP"]);
                    this.tipoMovimiento = Convert.ToInt32(Request.QueryString["TM"]);

                    this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.group = Convert.ToInt32(Request.QueryString["g"]);
                    this.idCaja = Convert.ToInt32(Request.QueryString["Caja"]);

                    this.nombreTarjeta = Request.QueryString["Nombre"];
                    this.estado = Convert.ToInt32(Request.QueryString["estado"]);
                    this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);

                    this.operador = Convert.ToInt32(Request.QueryString["ope"]);
                    this.origenPago = Convert.ToInt32(Request.QueryString["origenP"]);

                    this.idMutual = Convert.ToInt32(Request.QueryString["Mutual"]);
                    this.idRemesa = Convert.ToInt32(Request.QueryString["remesa"]);

                    if (accion == 1)
                    {
                        generarReporte();//cheques
                    }
                    if (accion == 2)
                    {
                        generarReporte2();//Caja
                    }
                    if (accion == 3)
                    {
                        generarReporte3();//Reporte gastos
                    }
                    if (accion == 4)
                    {
                        generarReporte4();//Comprobante caja
                    }
                    if (accion == 5)
                    {
                        generarReporte5();//Resumen Caja agrupado.
                    }
                    if (accion == 6)
                    {
                        generarReporte6();//Informe cierres caja.
                    }
                    if (accion == 7)
                    {
                        generarReporte7();//Reporte tarjetas.
                    }
                    if (accion == 8)
                    {
                        generarReporte8();//Resumen Cierre Turno
                    }
                    if (accion == 9)
                    {
                        generarReporte9();
                    }
                    if (accion == 10)
                    {
                        generarReporte10(); //Pagarés
                    }
                    if (accion == 11)
                    {
                        generarReporte11(); //Remesa
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
                controladorCobranza controlador = new controladorCobranza();
                List<ChequesValores> lstCheques = controlador.obtenerDatosCheque2(this.fechaD, this.fechaH, this.suc, this.idCliente, this.fechaDCobro, this.fechaHCobro, this.tipoFecha, this.origen, this.estadoCh,this.fechaDImp,this.fechaHImp);

                decimal saldo = 0;
                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("id");
                dtDatos.Columns.Add("fechaRE");
                dtDatos.Columns.Add("fechaA");
                dtDatos.Columns.Add("fechaI");
                dtDatos.Columns.Add("reciboC");
                dtDatos.Columns.Add("reciboP");
                dtDatos.Columns.Add("importe", typeof(double));
                dtDatos.Columns.Add("numero");
                dtDatos.Columns.Add("banco");
                dtDatos.Columns.Add("cuenta");
                dtDatos.Columns.Add("cliente");
                dtDatos.Columns.Add("Sucursal");
                dtDatos.Columns.Add("estado");
                dtDatos.Columns.Add("cuit");
                dtDatos.Columns.Add("Observacion");

                foreach (ChequesValores ch in lstCheques)
                {
                    DataRow drDatos = dtDatos.NewRow();
                    drDatos["id"] = ch.Cheque.id;
                    drDatos["fechaRE"] = ch.FechaCobro.ToString("dd/MM/yyyy");
                    if (ch.ReciboPago != "" && ch.ReciboPago != null)
                    {
                        drDatos["fechaRE"] = ch.FechaPago.ToString("dd/MM/yyyy");
                    }
                    Cheques_Cuentas datosImputacion = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                    if (datosImputacion != null)
                    {
                        drDatos["fechaI"] = datosImputacion.fechaImputado.Value.ToString("dd/MM/yyyy");
                    }
                    else
                        drDatos["fechaI"] = "";

                    drDatos["fechaA"] = ch.Cheque.fecha.ToString("dd/MM/yyyy");
                    drDatos["reciboC"] = ch.ReciboCobro;
                    drDatos["reciboP"] = ch.ReciboPago;
                    drDatos["importe"] = ch.Cheque.importe;
                    drDatos["numero"] = ch.Cheque.numero;
                    drDatos["banco"] = ch.Cheque.banco.entidad;
                    drDatos["cuenta"] = ch.Cheque.cuenta;
                    drDatos["cliente"] = ch.Cliente;

                    if (!String.IsNullOrEmpty(ch.sucursalCobro.nombre))
                        drDatos["Sucursal"] = ch.sucursalCobro.nombre;
                    if (!String.IsNullOrEmpty(ch.sucursalPago.nombre))
                        drDatos["Sucursal"] = ch.sucursalPago.nombre;

                    drDatos["estado"] = ch.Cheque.estado;
                    drDatos["cuit"] = ch.Cheque.cuit;

                    if (ch.Cheque.estado == 1)
                    {
                        drDatos["estado"] = "Disponible";
                    }
                    if (ch.Cheque.estado == 2)
                    {
                        drDatos["estado"] = "Depositado";
                    }
                    if (ch.Cheque.estado == 3)
                    {
                        drDatos["estado"] = "Entregado";
                    }
                    if (ch.Cheque.estado == 4)
                    {
                        drDatos["estado"] = "Disponible";
                    }
                    if (ch.Cheque.estado == 5)
                    {
                        drDatos["estado"] = "Imputado a Cta.";
                    }

                    Gestion_Api.Entitys.Cheques_Cuentas datos = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                    if (datos != null)
                        drDatos["fechaI"] = datos.fechaImputado.Value.ToString("dd/MM/yyyy");

                    drDatos["Observacion"] = controlador.obtenerObservacionChequeManual(ch.Cheque.id);
                    saldo += ch.Cheque.importe;

                    dtDatos.Rows.Add(drDatos);
                }
                //DataTable dtCheques = Session["datosCheques"] as DataTable;                
                //Session["datosCheques"] = null;
                ////dtCheques.Columns[]

                //String totalCheques = Session["totalCheques"].ToString();
                //Session["totalCheques"] = null;

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (this.group == 1)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ChequesAgrupadoR.rdlc");
                    this.valor = 1;//.xls
                }
                else
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ChequesR.rdlc");
                }

                ReportParameter param = new ReportParameter("ParamTotal", saldo.ToString());
                ReportDataSource rds = new ReportDataSource("DatosCheques", dtDatos);                
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);                
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
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
        private void generarReporte2()
        {
            try
            {
                ControladorCaja controlador = new ControladorCaja();
                controladorSucursal contSucursal = new controladorSucursal();
                controladorRemuneraciones contRemuneracion = new controladorRemuneraciones();
                controladorCajaEntity contCajaCierre = new controladorCajaEntity();
                List<Caja> cajas = new List<Caja>();                
                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("fecha");
                dtDatos.Columns.Add("descripcion");
                dtDatos.Columns.Add("importe", typeof(decimal));
                dtDatos.Columns.Add("tipo");
                dtDatos.Columns.Add("comentario");

                decimal saldo = 0;

                if (this.fechaD != null && this.fechaH != null && this.suc != 0 && this.tipoPago == 0)
                {
                    if (this.tipoPago == 0)
                    {
                        cajas = controlador.obtenerCajasRango(this.fechaD, this.fechaH, this.suc, this.tipoPago, this.tipoMovimiento, this.ptoVenta);
                    }
                    else
                    {
                        cajas = controlador.obtenerCajasRango(this.fechaD, this.fechaH, this.suc, this.tipoPago, this.tipoMovimiento, this.ptoVenta);
                    }
                }
                else
                {
                    if (this.tipoPago == 0 && this.suc != 0)//
                    {
                        cajas = controlador.obtenerCajasRangoReduc(this.fechaD, this.fechaH, this.suc);
                    }
                    else
                    {
                        cajas = controlador.obtenerCajasRango(this.fechaD, this.fechaH, this.suc, this.tipoPago, this.tipoMovimiento, this.ptoVenta);
                    }
                }

                
                foreach (Caja c in cajas)
                {
                    if (this.cargarCaja(c))
                    {
                        DataRow drDatos = dtDatos.NewRow();

                        drDatos["fecha"] = c.fecha.ToString("dd/MM/yyyy");

                        if (c.tipoMovimiento == 1)
                        {
                            drDatos["descripcion"] = c.cobro.tipoDocumento.tipo + " " + c.cobro.numero;
                            if (c.cobro.cliente != null)
                                drDatos["descripcion"] += " - " + c.cobro.cliente.razonSocial;
                        }

                        if (c.tipoMovimiento == 2 || c.tipoMovimiento == 5)
                        {
                            drDatos["descripcion"] = c.mov.descripcion;
                        }
                        if (c.tipoMovimiento == 3)
                        {
                            drDatos["descripcion"] = "Recibo de Pago - " + c.pCompra.Numero; ;
                        }
                        if (c.tipoMovimiento == 4)
                        {
                            Gestion_Api.Entitys.Caja_Cierre cierre = contCajaCierre.obtenerCierreByID(c.cobro.id);
                            drDatos["descripcion"] = "Cierre de Caja Nro " + cierre.Numero;
                        }
                        if (c.tipoMovimiento == 6)
                        {
                            drDatos["descripcion"] = "Traspaso de Caja";
                        }
                        if (c.tipoMovimiento == 7)
                        {
                            drDatos["descripcion"] = "Apertura Caja";
                        }
                        if (c.tipoMovimiento == 8)
                        {
                            drDatos["descripcion"] = "Traspaso Efectivo Tarjeta";
                        }
                        if (c.tipoMovimiento == 9)
                        {
                            Gestion_Api.Entitys.PagoRemuneracione pago = contRemuneracion.obtenerPagoRemuneracionByID(c.cobro.id);
                            drDatos["descripcion"] = "Pago Remuneracion Nº " + pago.Numero;
                        }
                        if (c.tipoMovimiento == 11)
                        {
                            drDatos["descripcion"] = "Traspaso a Banco";
                        }

                        drDatos["importe"] = c.importe;
                        drDatos["tipo"] = c.tipo.descripcion;
                        drDatos["comentario"] = c.comentario;

                        dtDatos.Rows.Add(drDatos);
                        saldo += c.importe;
                    }
                }

                string sucursal = "TODAS";
                string ptoVenta = "TODOS";

                if (this.suc > 0)
                {
                    Sucursal sucursalReporte = contSucursal.obtenerSucursalID(this.suc);
                    if (sucursalReporte != null)
                    {
                        sucursal = sucursalReporte.nombre;                        
                    }                    
                }                
                if (this.ptoVenta > 0)
                {
                    PuntoVenta pv = contSucursal.obtenerPtoVentaId(this.ptoVenta);
                    if (pv != null)
                    {
                        ptoVenta = pv.nombre_fantasia;
                    }
                }
                
                

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CajaR.rdlc");

                ReportParameter param = new ReportParameter("ParamSaldo", saldo.ToString());
                ReportParameter param2 = new ReportParameter("ParamSucursal", sucursal);
                ReportParameter param3 = new ReportParameter("ParamPtoVenta", ptoVenta);

                ReportDataSource rds = new ReportDataSource("DatosCaja", dtDatos);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Caja", "xls");

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

                //get xls content
                //Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //String filename = string.Format("{0}.{1}", "Caja", "xls");

                //this.Response.Clear();
                //this.Response.Buffer = true;
                //this.Response.ContentType = "application/ms-excel";
                //this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                //this.Response.BinaryWrite(xlsContent);

                //this.Response.End();
            }
            catch (Exception ex)
            {

            }
        }
        private void generarReporte3()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));
                hasta = hasta.AddHours(23).AddMinutes(59).AddSeconds(59);//23:59:00hs
                
                ControladorCaja contCaja = new ControladorCaja();
                DataTable dtGastos = contCaja.obtenerGastosCajaBySuc(desde.ToString(), hasta.ToString(), this.suc);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("GastosR.rdlc");
                                
                ReportDataSource rds = new ReportDataSource("DatosGastos", dtGastos);

                ReportParameter param1 = new ReportParameter("ParamDesde", this.fechaD);
                ReportParameter param2 = new ReportParameter("ParamHasta", this.fechaH);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Gastos", "xls");

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
        private void generarReporte4()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                controladorSucursal contSuc= new controladorSucursal();
                controladorRemuneraciones contRemuneracion = new controladorRemuneraciones();

                Caja c = contCaja.obtenerCajaID(Convert.ToInt32(idCaja));
                Sucursal suc = contSuc.obtenerSucursalID(c.suc.id);
                PuntoVenta pv = contSuc.obtenerPtoVentaId(c.pv.id);
                string descripcion = "-";

                if(c.tipoMovimiento == 1)
                {                   
                    descripcion= c.cobro.tipoDocumento.tipo + " " + c.cobro.numero;                    
                }
                if (c.tipoMovimiento == 2 || c.tipoMovimiento == 5 )
                {
                    descripcion = c.mov.descripcion;
                }
                if (c.tipoMovimiento == 3)
                {                       
                    descripcion = "Recibo de Pago - " + c.pCompra.Numero;                    
                }
                if (c.tipoMovimiento == 4)
                {                    
                    descripcion = "Cierre de Caja";                    
                }
                if (c.tipoMovimiento == 6)
                {                    
                    descripcion = "Traspaso de Caja";
                }
                if (c.tipoMovimiento == 7)
                {                    
                    descripcion = "Apertura Caja";
                }
                if (c.tipoMovimiento == 8)
                {                    
                   descripcion = "Traspaso Efectivo Tarjeta";                    
                }
                if (c.tipoMovimiento == 9)
                {                    
                    Gestion_Api.Entitys.PagoRemuneracione pago = contRemuneracion.obtenerPagoRemuneracionByID(c.cobro.id);
                    descripcion = "Pago Remuneracion Nº " + pago.Numero;                    
                }
                if (c.tipoMovimiento == 11)
                {
                    descripcion = "Traspaso a Banco";
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComprobanteCajaR.rdlc");                

                ReportParameter param = new ReportParameter("ParamFecha", c.fecha.ToString("dd/MM/yyyy"));
                ReportParameter param2 = new ReportParameter("ParamDesc",descripcion);
                ReportParameter param3 = new ReportParameter("ParamImporte", c.importe.ToString());
                ReportParameter param4 = new ReportParameter("ParamObservacion", c.comentario);
                ReportParameter param5 = new ReportParameter("ParamSucursal", suc.nombre);
                ReportParameter param6 = new ReportParameter("ParamPuntoVta", pv.nombre_fantasia);
                
                this.ReportViewer1.LocalReport.DataSources.Clear();                
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 2)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Comprobante_Caja", "xls");

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
                ControladorCaja contCaja = new ControladorCaja();
                controladorSucursal contSuc = new controladorSucursal();
                Sucursal sucursal = new Sucursal();
                sucursal.nombre = "Todas";

                if (this.suc > 0)
                {
                    sucursal = contSuc.obtenerSucursalID(this.suc);
                }

                List<Caja> cajas = contCaja.obtenerCajasRango(fechaD, fechaH, suc, tipoPago, tipoMovimiento, ptoVenta);
                DataTable dtCaja = new DataTable();
                dtCaja.Columns.Add("TipoMov");
                dtCaja.Columns.Add("TipoPago");
                dtCaja.Columns.Add("Sucursal");
                dtCaja.Columns.Add("Monto",typeof(decimal));
                dtCaja.Columns.Add("TipoDoc");

                foreach (Caja caja in cajas)
                {
                    if (this.cargarCaja(caja))
                    {
                        DataRow row = dtCaja.NewRow();
                        row["TipoDoc"] = "PRP";

                        switch (caja.tipoMovimiento)
                        {
                            case 1:
                                //row["TipoMov"] = "Cobro";
                                row["TipoMov"] = caja.cobro.tipoDocumento.tipo;
                                if (caja.cobro.tipoDocumento.tipo.Contains("FC"))
                                {
                                    row["TipoDoc"] = "FC";
                                }
                                break;
                            case 2:
                                row["TipoMov"] = "Gasto";
                                break;
                            case 3:
                                row["TipoMov"] = "Pago";
                                row["TipoDoc"] = "FC";
                                break;
                            case 4:
                                row["TipoMov"] = "Cierre Caja";
                                row["TipoDoc"] = "FC";
                                break;
                            case 5:
                                row["TipoMov"] = "Diferencia Caja";
                                row["TipoDoc"] = "OTROS";
                                break;
                            case 6:
                                row["TipoMov"] = "Traspaso Caja";
                                row["TipoDoc"] = "OTROS";
                                break;
                            case 7:
                                row["TipoMov"] = "Apertura Caja";
                                row["TipoDoc"] = "FC";
                                break;
                            case 8:
                                row["TipoMov"] = "Traspaso efectivo tarjeta";
                                row["TipoDoc"] = "OTROS";
                                break;
                            case 9:
                                row["TipoMov"] = "Pago Remuneracion";
                                row["TipoDoc"] = "FC";
                                break;
                            case 11:
                                row["TipoMov"] = "Traspaso a Banco";
                                row["TipoDoc"] = "OTROS";
                                break;
                            default:
                                break;

                        }
                        row["TipoPago"] = caja.tipo.descripcion;
                        row["Monto"] = caja.importe;
                        Sucursal sCaja = contSuc.obtenerSucursalID(caja.suc.id);
                        if (sCaja != null)
                        {
                            row["Sucursal"] = sCaja.nombre;
                        }
                        else
                        {
                            row["Sucursal"] = "";
                        }

                        dtCaja.Rows.Add(row);
                    }
                }
                


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CajaResumenR.rdlc");

                ReportParameter param = new ReportParameter("ParamSucursal", sucursal.nombre);
                ReportDataSource rds = new ReportDataSource("dsDatosResumen", dtCaja);

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

                ////get xls content
                //Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //String filename = string.Format("{0}.{1}", "Caja", "xls");

                //this.Response.Clear();
                //this.Response.Buffer = true;
                //this.Response.ContentType = "application/ms-excel";
                //this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                ////this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                //this.Response.BinaryWrite(xlsContent);

                //this.Response.End();
            }
            catch
            {

            }
        }
        private void generarReporte6()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                controladorSucursal contSuc = new controladorSucursal();                

                List<Caja> cajas = contCaja.obtenerCajasRango(fechaD, fechaH, suc, tipoPago, 4, ptoVenta);//cierres
                cajas.AddRange(contCaja.obtenerCajasRango(fechaD, fechaH, suc, tipoPago, 5, ptoVenta));//diferencias

                DataTable dtCaja = new DataTable();
                dtCaja.Columns.Add("TipoMov");
                dtCaja.Columns.Add("TipoPago");
                dtCaja.Columns.Add("Sucursal");
                dtCaja.Columns.Add("Monto", typeof(decimal));

                foreach (Caja caja in cajas)
                {
                    if (this.cargarCaja(caja))
                    {
                        DataRow row = dtCaja.NewRow();

                        switch (caja.tipoMovimiento)
                        {
                            case 4:
                                row["TipoMov"] = "Cierre Caja";
                                break;
                            case 5:
                                row["TipoMov"] = "Diferencia Caja";
                                break;
                            default:
                                break;
                        }

                        row["TipoPago"] = caja.tipo.descripcion;
                        row["Monto"] = caja.importe;
                        Sucursal sCaja = contSuc.obtenerSucursalID(caja.suc.id);
                        if (sCaja != null)
                        {
                            row["Sucursal"] = sCaja.nombre;
                        }
                        else
                        {
                            row["Sucursal"] = "";
                        }

                        dtCaja.Rows.Add(row);
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeCierresR.rdlc");

                //ReportParameter param = new ReportParameter("ParamSucursal", sucursal.nombre);
                ReportDataSource rds = new ReportDataSource("DatosInforme", dtCaja);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.SetParameters(param);
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
        private void generarReporte7()
        {
            try
            {
                controladorTarjeta controlador = new controladorTarjeta();
                DataTable dtTarjetas = controlador.obtenerDatosTarjeta2(this.fechaD, this.fechaH, this.suc, this.nombreTarjeta, this.tipoFecha, estado,this.operador,this.origenPago);

                decimal total = 0;

                foreach (DataRow row in dtTarjetas.Rows)
                {
                    total += Convert.ToDecimal(row["monto"]);


                    DateTime fecha = Convert.ToDateTime(row["fecha"], new CultureInfo("es-AR"));
                    int diasAcreditacion = Convert.ToInt32(row["diasAcreditacion"]);
                    int tipoAcreditacion = Convert.ToInt32(row["tipoAcreditacion"]);
                    int diaMesAcreditacion = Convert.ToInt32(row["fechaAcreditacion"]);
                    int diaCierre = Convert.ToInt32(row["diaCierre"]);

                    if (tipoAcreditacion == 0)
                    {
                        DateTime fechaAcreditacion = fecha.AddDays(diasAcreditacion);
                        row["fechaVenta"] = fechaAcreditacion;
                    }
                    else
                    {
                        if (fecha.Day < diaCierre)
                        {
                            DateTime fechaAcreditacion = new DateTime(fecha.Year, fecha.AddMonths(1).Month, diaMesAcreditacion);
                            row["fechaVenta"] = fechaAcreditacion;
                        }
                        else
                        {
                            DateTime fechaAcreditacion = new DateTime(fecha.Year, fecha.AddMonths(2).Month, diaMesAcreditacion);
                            row["fechaVenta"] = fechaAcreditacion;
                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("TarjetasR.rdlc");

                ReportParameter param = new ReportParameter("ParamTotal", total.ToString());
                ReportDataSource rds = new ReportDataSource("DatosTarjetas", dtTarjetas);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Valores_Tarjetas", "xls");

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
                controladorCajaEntity contCajaEnt = new controladorCajaEntity();
                controladorSucursal contSuc = new controladorSucursal();            
                controladorArticulo contArt = new controladorArticulo();
                controladorFactEntity contSurtidores = new controladorFactEntity();
                controladorUsuario contUser = new controladorUsuario();

                DataTable dtStock = contArt.obtenerStocksBySuc(this.suc);
                DataTable dtvendido = contArt.obtenerStockVendidoCierreTurno(this.suc, this.ptoVenta);

                dtStock.Columns.Add("Cantidad", typeof(decimal));
                foreach (DataRow row in dtvendido.Rows)
                {
                    var drow = dtStock.AsEnumerable().Where(x => x["Producto"].ToString() == row["Producto"].ToString()).FirstOrDefault();
                    if (drow != null)
                    {
                        drow["Cantidad"] = Convert.ToDecimal(row["Cantidad"]);
                    }
                }

                List<CajaEnt> list = contCajaEnt.obtenerListadoCajaUltimoTurno(this.suc, this.ptoVenta);
                Sucursal sucursal = contSuc.obtenerSucursalID(this.suc);
                decimal totalEfectivo = list.Where(x => x.tipoPago == 1).Sum(x => x.importe).Value;
                decimal totalTarjeta = list.Where(x => x.tipoPago == 5).Sum(x => x.importe).Value;
                decimal totalTraspasos = list.Where(x => x.tipoMovimiento == 6).Sum(x => x.importe).Value;

                List<Surtidore> surtidores = contSurtidores.obtenerSurtidores();
                DataTable dtSurtidores = new DataTable();
                dtSurtidores.Columns.Add("Id");
                dtSurtidores.Columns.Add("Surtidor");
                dtSurtidores.Columns.Add("Contador", typeof(decimal));
                dtSurtidores.Columns.Add("Precio", typeof(decimal));
                dtSurtidores.Columns.Add("Total", typeof(decimal));

                foreach (var c in surtidores)
                {
                    var cierre = c.Surtidores_Cierre.LastOrDefault();
                    decimal total = cierre.CantidadCierre.Value - cierre.CantidadInicial.Value;
                    
                    DataRow row = dtSurtidores.NewRow();
                    row["Id"] = c.Id.ToString();
                    row["Surtidor"] = c.Descripcion;
                    row["Contador"] = c.Contador.Value;
                    row["Precio"] = c.Precio.Value;
                    row["Total"] = total;
                    dtSurtidores.Rows.Add(row);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ResumenTurnoR.rdlc");

                ReportParameter param = new ReportParameter("ParamSucursal", sucursal.nombre);
                ReportParameter param2 = new ReportParameter("ParamTotalEfectivo", totalEfectivo.ToString());
                ReportParameter param3 = new ReportParameter("ParamTotalTarjeta", totalTarjeta.ToString());
                ReportParameter param4 = new ReportParameter("ParamTotalTraspasos", totalTraspasos.ToString());
                ReportDataSource rds = new ReportDataSource("DatosStock", dtStock);
                ReportDataSource rds2 = new ReportDataSource("DatosSurtidores", dtSurtidores);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Cierre_Turno", "xls");

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
        private void generarReporte9()
        {
            try
            {
                controladorCobranza controlador = new controladorCobranza();
                List<ChequesValores> lstCheques = controlador.obtenerDatosCheque3(this.fechaD, this.fechaH, this.suc,this.idSucPago, this.idCliente, this.idProveedor,this.origen,this.estadoCh, this.tipoFecha);

                decimal saldo = 0;
                DataTable dtDatos = new DataTable();
                dtDatos.Columns.Add("id");
                dtDatos.Columns.Add("fechaRE");
                dtDatos.Columns.Add("FechaEntregado");
                dtDatos.Columns.Add("fechaA");
                dtDatos.Columns.Add("fechaI");
                dtDatos.Columns.Add("reciboC");
                dtDatos.Columns.Add("reciboP");
                dtDatos.Columns.Add("importe", typeof(double));
                dtDatos.Columns.Add("numero");
                dtDatos.Columns.Add("banco");
                dtDatos.Columns.Add("cuenta");
                dtDatos.Columns.Add("cliente");
                dtDatos.Columns.Add("Sucursal");
                dtDatos.Columns.Add("estado");
                dtDatos.Columns.Add("cuit");
                dtDatos.Columns.Add("Observacion");
                dtDatos.Columns.Add("SucursalPago");
                dtDatos.Columns.Add("Proveedor");
                dtDatos.Columns.Add("Tipo");

                foreach (ChequesValores ch in lstCheques)
                {
                    DataRow drDatos = dtDatos.NewRow();

                    drDatos["id"] = ch.Cheque.id;
                    drDatos["fechaRE"] = "";
                    drDatos["FechaEntregado"] = "";

                    if (ch.FechaCobro > new DateTime(0001,1,1))
                        drDatos["fechaRE"] = ch.FechaCobro.ToString("dd/MM/yyyy");
                    if (ch.FechaPago > new DateTime(0001, 1, 1))
                        drDatos["FechaEntregado"] = ch.FechaPago.ToString("dd/MM/yyyy");

                    Cheques_Cuentas datosImputacion = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                    if (datosImputacion != null)
                        drDatos["fechaI"] = datosImputacion.fechaImputado.Value.ToString("dd/MM/yyyy");
                    else
                        drDatos["fechaI"] = "";

                    drDatos["fechaA"] = ch.Cheque.fecha.ToString("dd/MM/yyyy");
                    drDatos["reciboC"] = ch.ReciboCobro;
                    drDatos["reciboP"] = ch.ReciboPago;
                    drDatos["importe"] = ch.Cheque.importe;
                    drDatos["numero"] = ch.Cheque.numero;
                    drDatos["banco"] = ch.Cheque.banco.entidad;
                    drDatos["cuenta"] = ch.Cheque.cuenta;

                    if(!String.IsNullOrEmpty(ch.Cliente))
                        drDatos["cliente"] = ch.Cliente;

                    if (!String.IsNullOrEmpty(ch.Proveedor))
                        drDatos["Proveedor"] = ch.Proveedor;

                    if (!String.IsNullOrEmpty(ch.sucursalCobro.nombre))
                        drDatos["Sucursal"] = ch.sucursalCobro.nombre;

                    if (!String.IsNullOrEmpty(ch.sucursalPago.nombre))
                        drDatos["SucursalPago"] = ch.sucursalPago.nombre;

                    drDatos["estado"] = ch.Cheque.estado;
                    drDatos["cuit"] = ch.Cheque.cuit;

                    if (ch.Cheque.estado == 1)
                        drDatos["estado"] = "Disponible";
                    if (ch.Cheque.estado == 2)
                        drDatos["estado"] = "Depositado";
                    if (ch.Cheque.estado == 3)
                        drDatos["estado"] = "Entregado";
                    if (ch.Cheque.estado == 4)
                        drDatos["estado"] = "Disponible";
                    if (ch.Cheque.estado == 5)
                        drDatos["estado"] = "Imputado a Cta.";

                    Gestion_Api.Entitys.Cheques_Cuentas datos = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                    if (datos != null)
                        drDatos["fechaI"] = datos.fechaImputado.Value.ToString("dd/MM/yyyy");
                    drDatos["Observacion"] = controlador.obtenerObservacionChequeManual(ch.Cheque.id);

                    // Tipo de Cheque - Blanco(0)  / Negro(1)
                    drDatos["Tipo"] = string.Empty;
                    if (ch.tipoCheque == 0)
                        drDatos["Tipo"] = "FC";
                    if (ch.tipoCheque == 1)
                        drDatos["Tipo"] = "PRP";

                    saldo += ch.Cheque.importe;

                    dtDatos.Rows.Add(drDatos);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (this.group == 1)
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ChequesAgrupadoR_2.rdlc");
                    this.valor = 1;//.xls
                }
                else
                {
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ChequesR_2.rdlc");
                }

                ReportParameter param = new ReportParameter("ParamTotal", saldo.ToString());
                ReportDataSource rds = new ReportDataSource("DatosCheques", dtDatos);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Valores_Cheques", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
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
                controladorFactEntity contFactEnt = new controladorFactEntity();

                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                DataTable dt = new DataTable();
                dt.Columns.Add("Fecha", typeof(DateTime));
                dt.Columns.Add("Sucursal");
                dt.Columns.Add("Documento");
                dt.Columns.Add("Mutual");
                dt.Columns.Add("Socio");
                dt.Columns.Add("Autorizacion");
                dt.Columns.Add("Numero");
                dt.Columns.Add("Importe", typeof(decimal));
                dt.Columns.Add("Vencimiento", typeof(DateTime));
                dt.Columns.Add("Cuota");
                dt.Columns.Add("Estado");

                List<Mutuales_Pagares> pagares = contFactEnt.obtenerPagares(desde, hasta, this.suc, this.idMutual, this.tipoFecha, this.estado);

                if (pagares != null)
                {
                    foreach (var p in pagares)
                    {
                        DataRow dr = dt.NewRow();

                        dr["Fecha"] = p.Fecha;
                        dr["Sucursal"] = p.sucursale.nombre;
                        dr["Documento"] = this.obtenerDocumentoPagare(p.Factura.Value);
                        dr["Mutual"] = p.Mutuale.Nombre;
                        dr["Socio"] = p.NroSocio;
                        dr["Autorizacion"] = p.NroAutorizacion;
                        dr["Numero"] = p.Numero;
                        dr["Importe"] = p.Importe;
                        dr["Vencimiento"] = p.Vencimiento;
                        dr["Cuota"] = p.Vencimiento.Value.Month - p.Fecha.Value.Month;
                        dr["Estado"] = this.obtenerEstadoPagare((long)p.Estado);

                        dt.Rows.Add(dr);
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PagaresR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosPagares", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Pagares", "xls");

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
            catch (Exception Ex)
            {

            }
        }
        private void generarReporte11()
        {
            try
            {
                ControladorRemesaEntity contRemesa = new ControladorRemesaEntity();
                controladorSucursal contSuc = new controladorSucursal();

                //DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                //DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                Remesa remesa = contRemesa.ObtenerRemesaPorID(idRemesa);
                List<Remesa_Moneda_Detalle> remesaDetalle = contRemesa.ObtenerDetalleDeRemesaPorIDRemesa(idRemesa);

                DataTable dtRemesa = new DataTable();
                dtRemesa.Columns.Add("NumeroRemesa");
                dtRemesa.Columns.Add("Fecha", typeof(DateTime));
                dtRemesa.Columns.Add("Entrega");
                dtRemesa.Columns.Add("Observaciones");
                dtRemesa.Columns.Add("Recibe");
                dtRemesa.Columns.Add("SonPesos");
                dtRemesa.Columns.Add("SucursalDestino");
                dtRemesa.Columns.Add("SucursalOrigen");
                dtRemesa.Columns.Add("DomicilioDestino");
                dtRemesa.Columns.Add("DomicilioOrigen");
                dtRemesa.Columns.Add("OtrosValores");

                DataTable dtRemesaDetalle = new DataTable();
                dtRemesaDetalle.Columns.Add("Cantidad");
                dtRemesaDetalle.Columns.Add("Denominacion");
                dtRemesaDetalle.Columns.Add("Total");

                DataRow drRemesa = dtRemesa.NewRow();

                drRemesa["NumeroRemesa"] = remesa.NumeroRemesa.Value.ToString("D8");
                drRemesa["Fecha"] = remesa.Fecha;
                drRemesa["Entrega"] = remesa.Entrega;
                drRemesa["Observaciones"] = remesa.Observaciones;
                drRemesa["Recibe"] = remesa.Recibe;
                drRemesa["SonPesos"] = remesa.SonPesos;
                drRemesa["SucursalDestino"] = contSuc.obtenerSucursalID((int)remesa.SucursalDestino).nombre;
                drRemesa["SucursalOrigen"] = contSuc.obtenerSucursalID((int)remesa.SucursalOrigen).nombre;
                drRemesa["DomicilioDestino"] = contSuc.obtenerSucursalID((int)remesa.SucursalDestino).direccion;
                drRemesa["DomicilioOrigen"] = contSuc.obtenerSucursalID((int)remesa.SucursalOrigen).direccion;
                drRemesa["OtrosValores"] = remesa.OtrosValores;

                dtRemesa.Rows.Add(drRemesa);                
                int totalFinal = 0;

                foreach (var item in remesaDetalle)
                {
                    DataRow drRemesaDetalle = dtRemesaDetalle.NewRow();

                    drRemesaDetalle["Cantidad"] = item.Cantidad;
                    drRemesaDetalle["Denominacion"] = item.Denominacion;
                    drRemesaDetalle["total"] = item.Cantidad * item.Valor;
                    totalFinal += (int)item.Cantidad * (int)item.Valor;

                    dtRemesaDetalle.Rows.Add(drRemesaDetalle);
                }                

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RemesaR.rdlc");

                ReportDataSource rds = new ReportDataSource("Remesa", dtRemesa);
                ReportDataSource rds2 = new ReportDataSource("RemesaDetalle", dtRemesaDetalle);
                ReportParameter param = new ReportParameter("ParamTotal", totalFinal.ToString());

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.valor == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Pagares", "xls");

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
            catch (Exception Ex)
            {

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
        private bool cargarCaja(Caja c)
        {
            try
            {
                if (c.suc.clienteDefecto == -2)
                {
                    string perfil = Session["Login_NombrePerfil"] as string;

                    DataTable dt = new DataTable();

                    if (perfil == "SuperAdministrador")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string obtenerDocumentoPagare(int idFactura)
        {
            try
            {
                controladorFacturacion contFacturacion = new controladorFacturacion();
                string documento = string.Empty;
                
                Factura fc = contFacturacion.obtenerFacturaId(idFactura);

                if (fc != null)
                    documento = fc.tipo.tipo + " " + fc.numero;

                return documento;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error obteniendo documento de Pagaré. Excepción: " + Ex.Message);
                return string.Empty;
            }
        }
        private string obtenerEstadoPagare(long idEstado)
        {
            try
            {
                controladorFactEntity contFactEnt = new controladorFactEntity();
                string documento = string.Empty;

                var estado = contFactEnt.obtenerPagares_EstadosById(idEstado);
                if (estado != null)
                    documento = estado.Descripcion;

                return documento;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error obteniendo estado de Pagaré. Excepción: " + Ex.Message);
                return string.Empty;
            }
        }
    }
}