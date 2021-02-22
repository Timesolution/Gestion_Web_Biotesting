//using CrystalDecisions.CrystalReports.Engine;
using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Configuration;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ImpresionPedido : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int idPedido;
        private int accion;
        private string fdesde;
        private string fhasta;
        private int sucursal;
        private int idGrupo;
        private int excel;
        private int cliente;
        private int articulo;
        private string idPedidos;
        private int cotizacion;
        private int proveedor;
        private int monedaOriginal = 0;
        private long zonaEntrega;
        private int estadoPedido;
        private decimal imprimirOtraDivisa;
        private int idMoneda;
        Moneda monedaElegida = new Moneda();

        DataTable dtPedidosTemp;

        controladorMoneda controladorMoneda = new controladorMoneda();
        ControladorPedido controlador = new ControladorPedido();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorArticulo contArt = new controladorArticulo();
        controladorFunciones contFunciones = new controladorFunciones();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    dtPedidosTemp = new DataTable();
                    this.InicializarDtPedidos();

                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    idPedido = Convert.ToInt32(Request.QueryString["Pedido"]);
                    fdesde = Request.QueryString["fd"];
                    fhasta = Request.QueryString["fh"];
                    sucursal = Convert.ToInt32(Request.QueryString["suc"]);
                    idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                    excel = Convert.ToInt32(Request.QueryString["ex"]);
                    cliente = Convert.ToInt32(Request.QueryString["c"]);
                    proveedor = Convert.ToInt32(Request.QueryString["p"]);
                    articulo = Convert.ToInt32(Request.QueryString["art"]);
                    idPedidos = Request.QueryString["pedidos"];
                    cotizacion = Convert.ToInt32(Request.QueryString["co"]);
                    zonaEntrega = Convert.ToInt64(Request.QueryString["ze"]);
                    estadoPedido = Convert.ToInt32(Request.QueryString["ep"]);

                    ///Verifico si el usuario eligio imprimir el documento en otra divisa
                    if (Request.QueryString["div"] != null)
                    {
                        idMoneda = Convert.ToInt32(Request.QueryString["div"]);
                        monedaElegida = controladorMoneda.obtenerMonedaID(idMoneda);
                        imprimirOtraDivisa = monedaElegida.cambio;
                    }

                    //Obtengo la configuracion para ver los pedidos en moneda original o no.
                    string pmo = WebConfigurationManager.AppSettings.Get("PedidosMonedaOriginal");
                    if (!String.IsNullOrEmpty(pmo))
                        monedaOriginal = Convert.ToInt32(pmo);

                    //Remuevo el último ; de la cadena de idPedidos
                    if (!string.IsNullOrEmpty(idPedidos))
                        idPedidos = idPedidos.Remove(idPedidos.Length - 1);

                    if (accion == 1 && monedaOriginal == 0)
                    {
                        this.generarReporte2(idPedido);
                    }
                    if (accion == 2)
                    {
                        this.generarReporte3();
                    }
                    if (accion == 3)
                    {
                        this.generarReporte4();
                    }
                    if (accion == 4)
                    {
                        this.generarReporte5();
                    }
                    if (accion == 5)
                    {
                        this.generarReporte6(); //Pedidos Consolidado
                    }
                    if (accion == 6)
                    {
                        this.generarReporte7(); //Pedidos por Grupo
                    }
                    if (accion == 7)
                    {
                        this.generarReporte8(); //Pedidos por cliente agrupado
                    }
                    if (accion == 1 && monedaOriginal == 1)
                    {
                        this.generarReporte9(); //Pedidos con moneda original
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pedido. " + ex.Message));

                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pedido. " + ex.Message);
            }
        }

        private void InicializarDtPedidos()
        {
            try
            {
                dtPedidosTemp.Columns.Add("fecha");
                dtPedidosTemp.Columns.Add("numero");
                dtPedidosTemp.Columns.Add("razon");
                dtPedidosTemp.Columns.Add("total");
                dtPedidosTemp.Columns.Add("estado");
                dtPedidos = dtPedidosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Se produjo un error generado dtpedidos " + ex.Message));

            }

        }

        public DataTable dtPedidos
        {

            get
            {
                if (ViewState["Pedidos"] != null)
                {
                    return (DataTable)ViewState["Pedidos"];
                }
                else
                {
                    return dtPedidosTemp;
                }
            }
            set
            {
                ViewState["Pedidos"] = value;
            }
        }


        private void generarReporte(int idPedido)
        {
            try
            {
                //ReportDocument reporte = new ReportDocument();
                //reporte.Load(Server.MapPath("Pedidos.rpt"));
                //DataTable dtDatos = controlador.obtenerDatosPedido(idPedido);
                //DataTable dtDetalle = controlador.obtenerDetallePedido(idPedido);
                //decimal TotalFinal = 0;

                //foreach(DataRow dr in dtDetalle.Rows)
                //{
                //    reporte.SetParameterValue("Numero", dr["Numero"]);
                //    reporte.SetParameterValue("Fecha", dr["Fecha"]);
                //    reporte.SetParameterValue("RazonSocial", dr["RazonSocial"]);
                //    reporte.SetParameterValue("CUIT", dr["CUIT"]);
                //    reporte.SetParameterValue("IVA", dr["IVA"]);
                //}

                //foreach(DataRow dr in dtDatos.Rows)
                //{
                //    reporte.SetParameterValue("Codigo", dr["Codigo"]);
                //    reporte.SetParameterValue("Descripcion", dr["Descripcion"]);
                //    reporte.SetParameterValue("PrecioUnitario", dr["PrecioUnitario"]);
                //    reporte.SetParameterValue("Cantidad", dr["Cantidad"]);
                //    reporte.SetParameterValue("Total", dr["Total"]);
                //    TotalFinal += Convert.ToDecimal(dr["Total"]);
                //}

                //reporte.SetParameterValue("TotalFinal", TotalFinal);

                //CrystalReportViewer1.ReportSource = reporte;
                //reporte.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, "Pedido_" + idPedido.ToString());
            }
            catch
            {

            }
        }

        private void generarReporte2(int idPedido)
        {
            try
            {
                string fecha = string.Empty;
                string hora = string.Empty;
                string domicilio = string.Empty;
                string zona = string.Empty;
                string telefono = string.Empty;
                string formaPago = string.Empty;
                ControladorClienteEntity controlCli = new ControladorClienteEntity();
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                controladorZona controlZona = new controladorZona();
                controladorCliente controlCliente = new controladorCliente();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();
                Configuracion configuracion = new Configuracion();

                DataTable dtDatos = controlador.obtenerDatosPedido(idPedido);
                DataTable dtDetalle = controlador.obtenerDetallePedido(idPedido);
                DataTable dtTotal = controlador.obtenerTotalPedido(idPedido);

                dtDatos.Columns.Add("codigoBarra");
                Articulo a = new Articulo();
                foreach (DataRow rowDatos in dtDatos.Rows)
                {
                    a = this.contArt.obtenerArticuloByID(Convert.ToInt32(rowDatos["Id"]));
                    if (a != null)
                    {
                        rowDatos["codigoBarra"] = a.codigoBarra;
                    }
                    else
                    {
                        rowDatos["codigoBarra"] = "";
                    }
                }

                var tiempo = configuracion.TiempoLineasPedido.Split(';');
                dtDetalle.Columns.Add("TiempoLineasPedido");
                try
                {
                    TimeSpan tiempoPorLinea = new TimeSpan(0, Convert.ToInt32(tiempo[0]), Convert.ToInt32(tiempo[1]));
                    tiempoPorLinea = TimeSpan.FromTicks(tiempoPorLinea.Ticks * dtDatos.Rows.Count);
                    dtDetalle.Rows[0]["TiempoLineasPedido"] = tiempoPorLinea.ToString(@"hh\:mm\:ss");
                }
                catch { }



                int suc = Convert.ToInt32(dtDetalle.Rows[0]["Id_suc"]);

                //levanto los datos de la factura
                var drDatosFactura = dtDetalle.Rows[0];
                //si es cotizacion reemplazo Pedido por cotizacion
                if (this.cotizacion == 1)
                {
                    dtDetalle.Rows[0]["Numero"] = dtDetalle.Rows[0]["Numero"].ToString().Replace("Pedido", "Cotización");
                }


                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;
                String ingBrutos = String.Empty;
                String fechaInicio = String.Empty;
                String cuitEmpresa = String.Empty;

                foreach (DataRow rowEmp in dtEmpresa.Rows)
                {
                    cuitEmpresa = rowEmp.ItemArray[1].ToString();
                    razonSoc = rowEmp.ItemArray[2].ToString();
                    ingBrutos = rowEmp.ItemArray[3].ToString();
                    fechaInicio = Convert.ToDateTime(rowEmp["Fecha Inicio"]).ToString("dd/MM/yyyy");
                    condIVA = rowEmp.ItemArray[5].ToString();
                    direComer = rowEmp.ItemArray[6].ToString();
                }
                //verifico si tiene un contacto
                int idCliente = 0;
                foreach (DataRow dr in dtDetalle.Rows)
                {
                    //obtengo el id del cliente
                    idCliente = Convert.ToInt32(dr["idCliente"]);
                    //obtengo la forma de pago
                    formaPago = dr["formaPago"].ToString();
                }
                //obtengo el telefono del cliente para agregarlo al pedido
                List<contacto> contactosClientes = controlCliente.obtenerContactos(idCliente);
                if (contactosClientes.Count > 0 & contactosClientes != null)
                {
                    telefono = contactosClientes[0].numero;
                }
                if (String.IsNullOrEmpty(telefono))
                {
                    telefono = "-";
                }

                //obtengo los datos de Zona entregaentrega
                Clientes_Entregas cl = controlCli.obtenerEntregaCliente(idCliente);

                if (cl != null)
                {
                    if (!string.IsNullOrEmpty(cl.Zona.nombre))
                    {
                        zona = cl.Zona.nombre;
                    }
                }
                if (string.IsNullOrEmpty(zona))
                {
                    zona = "-";
                }

                dtDatos = contArtEntity.obtenerPresentacionesArtDT(dtDatos);
                dtDatos = contArtEntity.obtenerStockArtDT(dtDatos, suc);

                ///subtotal, retencion, descuento, total
                DataRow row = dtTotal.Rows[0];
                decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                decimal descuento = Convert.ToDecimal(row["descuento"]);
                decimal retencion = Convert.ToDecimal(row["retenciones"]);
                decimal total = Convert.ToDecimal(row["TotalFinal"]);

                ///Chequeo si eleigio imprimir el documento en otra divisa para hacer los calculos correspondientes
                if (imprimirOtraDivisa > 0)
                {
                    foreach (DataRow rowDatos in dtDatos.Rows)
                    {
                        rowDatos["PrecioUnitario"] = Decimal.Round(Convert.ToDecimal(rowDatos["PrecioUnitario"]) / imprimirOtraDivisa, 2);
                        rowDatos["Total"] = Decimal.Round(Convert.ToDecimal(rowDatos["Total"]) / imprimirOtraDivisa, 2);
                    }

                    subtotal = Decimal.Round(subtotal / imprimirOtraDivisa, 2);
                    descuento = Decimal.Round(descuento / imprimirOtraDivisa, 2);
                    retencion = Decimal.Round(retencion / imprimirOtraDivisa, 2);
                    total = Decimal.Round(total / imprimirOtraDivisa, 2);

                    dtTotal.Rows[0][0] = total.ToString();
                    dtTotal.Rows[0][1] = subtotal.ToString();
                    dtTotal.Rows[0][2] = descuento.ToString();
                    dtTotal.Rows[0][3] = retencion.ToString();

                    ///Sumo el comentario al campo de observaciones para informar en base a que divisa se realizaron los calculos de los precios
                    dtDetalle.Rows[0]["Observaciones"] += "\r\n\r\n*Cotización emitida en divisa: (" + monedaElegida.moneda + ")";
                }

                int tieneSistemaEstetica = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TieneSistemaEstetica"));

                if (tieneSistemaEstetica == 1)
                {
                    dtDetalle.Rows[0]["Observaciones"] += "\r\n\r\n*Referido:  " + dtDetalle.Rows[0]["ZonaDescripcion"].ToString() + ".";
                }


                ReportParameter paramZona = new ReportParameter("ParamZona", zona);
                ReportParameter paramTel = new ReportParameter("ParamTel", telefono);
                ReportParameter paramFormaPago = new ReportParameter("ParamFormaPago", formaPago);

                ReportParameter param1 = new ReportParameter("ParamSubtotal", subtotal.ToString("C"));
                ReportParameter param2 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                ReportParameter param3 = new ReportParameter("ParamDescuento", descuento.ToString("C"));
                //parametros Datos empresa
                ReportParameter param4 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param5 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                ReportParameter param6 = new ReportParameter("ParamFechaIni", fechaInicio);
                ReportParameter param7 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param8 = new ReportParameter("ParamCondIva", condIVA);
                ReportParameter param9 = new ReportParameter("ParamCuitEmp", cuitEmpresa);


                string imagen = this.generarCodigo(idPedido);
                ReportParameter param10 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Pedidos.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                //ReportDataSource rds = new ReportDataSource("DetallePedido", dtDetalle);
                ReportDataSource rds = new ReportDataSource("DatosDetalle", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPedido", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("TotalPedido", dtTotal);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.SetParameters(paramZona);
                this.ReportViewer1.LocalReport.SetParameters(paramTel);
                this.ReportViewer1.LocalReport.SetParameters(paramFormaPago);
                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);
                this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);

                //this.ReportViewer1.LocalReport.SetParameters(rpHora);
                //this.ReportViewer1.LocalReport.SetParameters(rpDomicilio);
                this.ReportViewer1.LocalReport.EnableExternalImages = true;


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

        //listado pedidos
        private void generarReporte3()
        {
            try
            {

                this.dtPedidos = Session["dtDatosPedidos"] as DataTable;
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosListado.rdlc");
                ReportDataSource rds = new ReportDataSource("dtPedidos", this.dtPedidos);


                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                //this.ReportViewer1.LocalReport.SetParameters(rpFecha);
                //this.ReportViewer1.LocalReport.SetParameters(rpHora);
                //this.ReportViewer1.LocalReport.SetParameters(rpDomicilio);

                Session["dtDatosPedidos"] = null;
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;
                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Detalle_Facturas", "xls");

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                this.Response.BinaryWrite(xlsContent);

                this.Response.End();

                //Warning[] warnings;

                //string mimeType, encoding, fileNameExtension;

                //string[] streams;

                ////get pdf content

                //Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //this.Response.Clear();
                //this.Response.Buffer = true;
                //this.Response.ContentType = "application/pdf";
                //this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                //this.Response.BinaryWrite(pdfContent);

                //this.Response.End();


            }
            catch
            {

            }
        }
        private void generarReporte4()
        {
            try
            {
                DataTable dt = this.controlador.obtenerCantidadArticulosEnPedidos(this.fdesde, this.fhasta, this.sucursal, this.idGrupo, this.cliente, this.articulo, this.proveedor, this.zonaEntrega, this.estadoPedido);

                //Sucursal s = this.contSucursal.obtenerSucursalID(this.sucursal);
                Sucursal s = new Sucursal();
                if (sucursal > 0)
                {
                    s = this.contSucursal.obtenerSucursalID(this.sucursal);
                }
                else
                {
                    s.nombre = "Todos";
                }

                String grupo = "Todos";
                if (this.idGrupo > 0)
                {
                    grupo g = this.contArt.obtenerGrupoID(this.idGrupo);
                    grupo = g.descripcion;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosNeto.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCantidad", dt);
                ReportParameter param = new ReportParameter("ParamSuc", s.nombre);
                ReportParameter param2 = new ReportParameter("ParamDesde", this.fdesde);
                ReportParameter param3 = new ReportParameter("ParamHasta", this.fhasta);
                ReportParameter param4 = new ReportParameter("ParamGrupo", grupo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;
                string mimeType, encoding, fileNameExtension;
                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Cantidad_Articulos_Pedidos", "xls");

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
            catch
            {

            }
        }
        private void generarReporte5()
        {
            try
            {
                controladorZona contZona = new controladorZona();
                ControladorClienteEntity contCliEnt = new ControladorClienteEntity();
                ControladorPedidoEntity contPedEnt = new ControladorPedidoEntity();

                Pedido p = this.controlador.obtenerPedidoId(this.idPedido);
                var pEnt = contPedEnt.obtenerCantidadBultosPedido(this.idPedido);
                var comentarios = contPedEnt.obtenerComentariosPedido(this.idPedido);
                var zonaCliente = contCliEnt.obtenerEntregaCliente(p.cliente.id);
                var expreso = contCliEnt.obtenerExpresoCliente(p.cliente.id);

                DataTable dtHojas = new DataTable();
                dtHojas.Columns.Add("Cliente");
                dtHojas.Columns.Add("Bultos");
                dtHojas.Columns.Add("NroBulto");
                dtHojas.Columns.Add("Expreso");
                dtHojas.Columns.Add("Direccion");
                dtHojas.Columns.Add("Zona");

                if (pEnt != null)
                {
                    for (int i = 0; i < pEnt.CantidadBultos.Value; i++)
                    {
                        DataRow row = dtHojas.NewRow();
                        row["Cliente"] = p.cliente.razonSocial;
                        row["Bultos"] = pEnt.CantidadBultos.Value;
                        row["NroBulto"] = i + 1;
                        if (expreso != null)
                        {
                            row["Expreso"] = expreso.nombre;
                        }
                        if (comentarios != null)
                        {
                            row["Direccion"] = comentarios.DomicilioEntrega;
                        }
                        if (zonaCliente != null)
                        {
                            row["Zona"] = zonaCliente.Zona.nombre;
                        }

                        dtHojas.Rows.Add(row);
                    }
                }
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosBultos.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCantidad", dtHojas);
                ReportParameter param2 = new ReportParameter("ParamBultos", pEnt.CantidadBultos.Value.ToString());

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param2);

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
        private void generarReporte6()
        {
            try
            {
                DataTable dt = new DataTable();
                //dt.Columns.Add("Pedido");
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Cantidad", typeof(decimal));
                dt.Columns.Add("Ubicacion");
                dt.Columns.Add("Pedido");
                dt.Columns.Add("Stock");

                string[] pedidos = idPedidos.Split(';');
                foreach (string ped in pedidos)
                {
                    if (!String.IsNullOrEmpty(ped))
                    {
                        Pedido p = this.controlador.obtenerPedidoId(Convert.ToInt32(ped));
                        if (p != null)
                        {
                            foreach (var item in p.items)
                            {
                                DataRow row = dt.NewRow();
                                if (!string.IsNullOrEmpty(item.articulo.codigo))
                                {
                                    row["Codigo"] = item.articulo.codigo;
                                }
                                if (!string.IsNullOrEmpty(item.articulo.ubicacion))
                                {
                                    row["Ubicacion"] = item.articulo.ubicacion;
                                }
                                if (!string.IsNullOrEmpty(item.descripcion))
                                {
                                    row["Descripcion"] = item.descripcion;
                                }
                                else
                                {
                                    Articulo a = this.contArt.obtenerArticuloByID(item.articulo.id);
                                    if (a != null)
                                    {
                                        row["Descripcion"] = a.descripcion;
                                    }
                                }
                                if (item.cantidad >= 0)
                                {
                                    row["Cantidad"] = item.cantidad;
                                }

                                row["Pedido"] = p.numero + ", " + p.cliente.razonSocial;
                                row["Stock"] = this.contArt.obtenerStockTotalArticulo(item.articulo.id).ToString("G");

                                dt.Rows.Add(row);
                            }
                        }

                    }
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosConsolidado.rdlc");

                ReportDataSource rds = new ReportDataSource("dsDatosPedidos", dt);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;
                string mimeType, encoding, fileNameExtension;
                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Consolidado Pedidos", "xls");

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
            catch (Exception Ex)
            {

            }
        }
        private void generarReporte7()
        {
            try
            {
                string path = Server.MapPath("pdfsPedidos/");
                int contadorOk = 0;
                int contadorTotal = 0;
                DataTable dt = new DataTable();

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                Directory.CreateDirectory(path);
                string[] pedidos = idPedidos.Split(';');
                foreach (string ped in pedidos)
                {
                    if (!String.IsNullOrEmpty(ped))
                    {
                        Pedido p = this.controlador.obtenerPedidoId(Convert.ToInt32(ped));
                        if (p != null)
                        {
                            string fileName = "p-" + p.numero + "_" + p.id + ".pdf";
                            int i = this.GenerarImpresionPDF(p, path + fileName);
                            if (i > 0)
                            {
                                contadorOk++;
                            }
                        }
                    }

                    contadorTotal++;
                }

                string[] pdfs = Directory.GetFiles(path);
                string nombre = path + "p-" + DateTime.Now.ToString("dd-MM-yyyy_hhmmss") + ".pdf";
                int ok = this.contFunciones.CombineMultiplePDFs(pdfs, nombre);
                if (ok > 0)
                {
                    try
                    {
                        foreach (string filePath in pdfs)
                        {
                            File.Delete(filePath);
                        }
                    }
                    catch { }

                    this.descargar(nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Realizados con exito:" + contadorOk + "de " + contadorTotal, ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo imprimir"));
                }
            }
            catch (Exception Ex)
            {

            }
        }
        private void generarReporte8()
        {
            try
            {
                DataTable dt = this.controlador.obtenerCantidadEnPedidosPorCliente(this.fdesde, this.fhasta, this.sucursal, this.idGrupo, this.cliente, this.articulo, this.proveedor, this.zonaEntrega);
                //Sucursal s = this.contSucursal.obtenerSucursalID(this.sucursal);
                Sucursal s = new Sucursal();
                if (sucursal > 0)
                {
                    s = this.contSucursal.obtenerSucursalID(this.sucursal);
                }
                else
                {
                    s.nombre = "Todos";
                }

                String grupo = "Todos";
                if (this.idGrupo > 0)
                {
                    grupo g = this.contArt.obtenerGrupoID(this.idGrupo);
                    grupo = g.descripcion;
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosPendientesCliente.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosCantidad", dt);
                ReportParameter param = new ReportParameter("ParamSuc", s.nombre);
                ReportParameter param2 = new ReportParameter("ParamDesde", this.fdesde);
                ReportParameter param3 = new ReportParameter("ParamHasta", this.fhasta);
                ReportParameter param4 = new ReportParameter("ParamGrupo", grupo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;
                string mimeType, encoding, fileNameExtension;
                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "Cantidad_Articulos_Pedidos", "xls");

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
            catch
            {

            }
        }
        private void generarReporte9()
        {
            try
            {
                string fecha = string.Empty;
                string hora = string.Empty;
                string domicilio = string.Empty;
                string zona = string.Empty;
                string telefono = string.Empty;
                ControladorClienteEntity controlCli = new ControladorClienteEntity();
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                controladorZona controlZona = new controladorZona();
                controladorCliente controlCliente = new controladorCliente();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();

                DataTable dtDatos = controlador.obtenerDatosPedidoMO(idPedido);
                DataTable dtDetalle = controlador.obtenerDetallePedido(idPedido);
                DataTable dtTotal = controlador.obtenerTotalPedidoMO(idPedido);
                DataTable dtTotalPesos = controlador.obtenerTotalPedido(idPedido);

                dtDatos.Columns.Add("CodigoBarra");
                dtDatos.Columns.Add("PorcentajeIVA");
                dtDatos.Columns.Add("TotalSinIVA", typeof(decimal));
                foreach (DataRow rowDatos in dtDatos.Rows)
                {
                    Articulo a = new Articulo();
                    a = this.contArt.obtenerArticuloByID(Convert.ToInt32(rowDatos["Id"]));
                    rowDatos["CodigoBarra"] = "";
                    rowDatos["PorcentajeIVA"] = "";
                    rowDatos["TotalSinIVA"] = rowDatos["Total"];
                    if (a != null)
                    {
                        rowDatos["CodigoBarra"] = a.codigoBarra;
                        rowDatos["PorcentajeIVA"] = a.porcentajeIva;
                        rowDatos["TotalSinIVA"] = decimal.Round(Convert.ToDecimal(rowDatos["Total"]) / ((a.porcentajeIva / 100) + 1), 2);
                    }
                }

                decimal tipoDeCambio = ObtenerMontoTipoDeCambioDePedido(dtDatos);

                int suc = Convert.ToInt32(dtDetalle.Rows[0]["Id_suc"]);

                //levanto los datos de la factura
                var drDatosFactura = dtDetalle.Rows[0];
                //si es cotizacion reemplazo Pedido por cotizacion
                if (this.cotizacion == 1)
                {
                    dtDetalle.Rows[0]["Numero"] = dtDetalle.Rows[0]["Numero"].ToString().Replace("Pedido", "Cotización");
                }


                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;
                String ingBrutos = String.Empty;
                String fechaInicio = String.Empty;
                String cuitEmpresa = String.Empty;

                foreach (DataRow rowEmp in dtEmpresa.Rows)
                {
                    cuitEmpresa = rowEmp.ItemArray[1].ToString();
                    razonSoc = rowEmp.ItemArray[2].ToString();
                    ingBrutos = rowEmp.ItemArray[3].ToString();
                    fechaInicio = Convert.ToDateTime(rowEmp["Fecha Inicio"]).ToString("dd/MM/yyyy");
                    condIVA = rowEmp.ItemArray[5].ToString();
                    direComer = rowEmp.ItemArray[6].ToString();
                }
                //verifico si tiene un contacto
                int idCliente = 0;
                foreach (DataRow dr in dtDetalle.Rows)
                {
                    //obtengo el id del cliente
                    idCliente = Convert.ToInt32(dr["idCliente"]);
                }
                //obtengo el telefono del cliente para agregarlo al pedido
                List<contacto> contactosClientes = controlCliente.obtenerContactos(idCliente);
                if (contactosClientes.Count > 0 & contactosClientes != null)
                {
                    telefono = contactosClientes[0].numero;
                }
                if (String.IsNullOrEmpty(telefono))
                {
                    telefono = "-";
                }

                //obtengo los datos de Zona entregaentrega
                Clientes_Entregas cl = controlCli.obtenerEntregaCliente(idCliente);

                if (cl != null)
                {
                    if (!string.IsNullOrEmpty(cl.Zona.nombre))
                    {
                        zona = cl.Zona.nombre;
                    }
                }
                if (string.IsNullOrEmpty(zona))
                {
                    zona = "-";
                }



                dtDatos = contArtEntity.obtenerPresentacionesArtDT(dtDatos);
                dtDatos = contArtEntity.obtenerStockArtDT(dtDatos, suc);

                //subtotal, retencion, descuento
                DataRow row = dtTotal.Rows[0];
                decimal subtotal = Convert.ToDecimal(row["Subtotal"]);
                decimal descuento = Convert.ToDecimal(row["Descuento"]);
                decimal retencion = 0;

                ReportParameter paramZona = new ReportParameter("ParamZona", zona);
                ReportParameter paramTel = new ReportParameter("ParamTel", telefono);

                ReportParameter param1 = new ReportParameter("ParamSubtotal", subtotal.ToString("C"));
                ReportParameter param2 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                ReportParameter param3 = new ReportParameter("ParamDescuento", descuento.ToString("C"));
                //parametros Datos empresa
                ReportParameter param4 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param5 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                ReportParameter param6 = new ReportParameter("ParamFechaIni", fechaInicio);
                ReportParameter param7 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param8 = new ReportParameter("ParamCondIva", condIVA);
                ReportParameter param9 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                ReportParameter paramTipoDeCambio = new ReportParameter("ParamTipoDeCambio", tipoDeCambio.ToString());

                string imagen = this.generarCodigo(idPedido);
                ReportParameter param10 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosMO.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                //ReportDataSource rds = new ReportDataSource("DetallePedido", dtDetalle);
                ReportDataSource rds = new ReportDataSource("DatosDetalle", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPedidoMO", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("TotalPedidoMO", dtTotal);
                ReportDataSource rds4 = new ReportDataSource("TotalPedidoPesos", dtTotalPesos);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.SetParameters(paramZona);
                this.ReportViewer1.LocalReport.SetParameters(paramTel);
                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);
                this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(paramTipoDeCambio);

                //this.ReportViewer1.LocalReport.SetParameters(rpHora);
                //this.ReportViewer1.LocalReport.SetParameters(rpDomicilio);
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

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

        private decimal ObtenerMontoTipoDeCambioDePedido(DataTable dtDatos)
        {
            try
            {
                decimal monto = Convert.ToDecimal(dtDatos.Rows[0]["CotizacionMO"]);
                return monto;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error en fun: ObtenerMontoTipoDeCambioDePedido. " + ex.Message);
                return 0;
            }
        }

        public string generarCodigo(int idPedido)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = idPedido.ToString();

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/Pedidos/" + idPedido + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + idPedido + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para pedido. " + ex.Message);
                return null;
            }
        }
        private void descargar(string path)
        {
            try
            {
                System.IO.FileInfo toDownload =
                     new System.IO.FileInfo(path);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition",
                           "attachment; filename=" + toDownload.Name);
                HttpContext.Current.Response.AddHeader("Content-Length",
                           toDownload.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.WriteFile(path);
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", " Error descargando pdf de pedidos. " + ex.Message);
            }
        }
        private int GenerarImpresionPDF(Pedido p, string pathGenerar)
        {
            try
            {

                string fecha = string.Empty;
                string hora = string.Empty;
                string domicilio = string.Empty;
                string zona = string.Empty;
                string telefono = string.Empty;
                ControladorClienteEntity controlCli = new ControladorClienteEntity();
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                controladorZona controlZona = new controladorZona();
                controladorCliente controlCliente = new controladorCliente();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();

                DataTable dtDatos = controlador.obtenerDatosPedido(p.id);
                DataTable dtDetalle = controlador.obtenerDetallePedido(p.id);
                DataTable dtTotal = controlador.obtenerTotalPedido(p.id);

                dtDatos.Columns.Add("codigoBarra");
                Articulo a = new Articulo();
                foreach (DataRow rowDatos in dtDatos.Rows)
                {
                    a = this.contArt.obtenerArticuloByID(Convert.ToInt32(rowDatos["Id"]));
                    if (a != null)
                    {
                        rowDatos["codigoBarra"] = a.codigoBarra;
                    }
                    else
                    {
                        rowDatos["codigoBarra"] = "";
                    }
                }

                int suc = Convert.ToInt32(dtDetalle.Rows[0]["Id_suc"]);

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
                String familia = String.Empty;

                foreach (DataRow rowEmp in dtEmpresa.Rows)
                {
                    cuitEmpresa = rowEmp.ItemArray[1].ToString();
                    razonSoc = rowEmp.ItemArray[2].ToString();
                    ingBrutos = rowEmp.ItemArray[3].ToString();
                    fechaInicio = Convert.ToDateTime(rowEmp["Fecha Inicio"]).ToString("dd/MM/yyyy");
                    condIVA = rowEmp.ItemArray[5].ToString();
                    direComer = rowEmp.ItemArray[6].ToString();
                }
                //verifico si tiene un contacto
                int idCliente = 0;
                foreach (DataRow dr in dtDetalle.Rows)
                {
                    //obtengo el id del cliente
                    idCliente = Convert.ToInt32(dr["idCliente"]);
                }
                //obtengo el telefono del cliente para agregarlo al pedido
                List<contacto> contactosClientes = controlCliente.obtenerContactos(idCliente);
                if (contactosClientes.Count > 0 & contactosClientes != null)
                {
                    telefono = contactosClientes[0].numero;
                }
                if (String.IsNullOrEmpty(telefono))
                {
                    telefono = "-";
                }

                //obtengo los datos de Zona entregaentrega
                Clientes_Entregas cl = controlCli.obtenerEntregaCliente(idCliente);

                if (cl != null)
                {
                    if (!string.IsNullOrEmpty(cl.Zona.nombre))
                    {
                        zona = cl.Zona.nombre;
                    }
                }
                if (string.IsNullOrEmpty(zona))
                {
                    zona = "-";
                }

                familia = controlCli.obtenerAliasFamilia(idCliente);

                dtDatos = contArtEntity.obtenerPresentacionesArtDT(dtDatos);
                dtDatos = contArtEntity.obtenerStockArtDT(dtDatos, suc);

                //subtotal, retencion, descuento
                DataRow row = dtTotal.Rows[0];
                decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                decimal descuento = Convert.ToDecimal(row["descuento"]);
                decimal retencion = Convert.ToDecimal(row["retenciones"]);

                ReportParameter paramZona = new ReportParameter("ParamZona", zona);
                ReportParameter paramFamilia = new ReportParameter("ParamFamilia", familia);
                ReportParameter paramTel = new ReportParameter("ParamTel", telefono);

                ReportParameter param1 = new ReportParameter("ParamSubtotal", subtotal.ToString("C"));
                ReportParameter param2 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                ReportParameter param3 = new ReportParameter("ParamDescuento", descuento.ToString("C"));
                //parametros Datos empresa
                ReportParameter param4 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param5 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                ReportParameter param6 = new ReportParameter("ParamFechaIni", fechaInicio);
                ReportParameter param7 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param8 = new ReportParameter("ParamCondIva", condIVA);
                ReportParameter param9 = new ReportParameter("ParamCuitEmp", cuitEmpresa);


                string imagen = this.generarCodigo(idPedido);
                ReportParameter param10 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("PedidosGrupo.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                //ReportDataSource rds = new ReportDataSource("DetallePedido", dtDetalle);
                ReportDataSource rds = new ReportDataSource("DatosDetalle", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPedido", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("TotalPedido", dtTotal);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.SetParameters(paramZona);
                this.ReportViewer1.LocalReport.SetParameters(paramTel);
                this.ReportViewer1.LocalReport.SetParameters(paramFamilia);
                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);
                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);
                this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);

                //this.ReportViewer1.LocalReport.SetParameters(rpHora);
                //this.ReportViewer1.LocalReport.SetParameters(rpDomicilio);
                this.ReportViewer1.LocalReport.EnableExternalImages = true;


                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get pdf content

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //save the generated report in the server
                FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                return 1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error enviando factura por mail. " + ex.Message));
                return -1;
            }
        }
    }
}