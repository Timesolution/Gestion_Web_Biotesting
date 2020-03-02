using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ImpresionRetenciones : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();
        controladorCliente contCliente = new controladorCliente();
        ControladorCCProveedor contCCProvedor = new ControladorCCProveedor();

        Mensajes mje = new Mensajes();
        int idRetencion;

        private int suc;
        private string fechaD;
        private string fechaH;
        private string tipoDoc;
        private int puntoVenta;
        private int accion;
        private int excel;
        private int ordenCompra;
        private int proveedor;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idRetencion = Convert.ToInt32(Request.QueryString["Retencion"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.fechaD = Request.QueryString["fd"];
                    this.fechaH = Request.QueryString["fh"];
                    this.suc = Convert.ToInt32(Request.QueryString["s"]);
                    string saldo = Request.QueryString["sd"];

                    if (accion == 1)
                    {
                        this.generarReportes();//Detalle Retencion
                    }
                    if (accion == 2)
                    {
                        this.generarReporte2();//Retenciones Pagos (ARBA)
                    }
                    if (accion == 3)
                    {
                        this.generarReporte3(saldo); //Retenciones Total
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al generar reporte. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al imprimir reporte. " + ex.Message);
            }
        }

        #region Reportes
        private void generarReportes()
        {
            try
            {
                DataTable dtRetencion = this.controlador.obtenerCobrosRetencionesById(this.idRetencion);
                //obtengo el detalle de la retencion agregando el cod de empresa
                DataRow drRetencionEmpresa = dtRetencion.Rows[0];
                DataRow dr = dtRetencion.Rows[0];
                string numero = dr["numero"].ToString();

                int empresa = Convert.ToInt32(dr["Empresa"].ToString());
                int idProveedor = Convert.ToInt32(dr["idProveedor"].ToString());
                int sucursal = Convert.ToInt32(dr["Sucursal"].ToString());
                int ptoVenta = Convert.ToInt32(dr["PuntoVta"].ToString());
                int idPago = Convert.ToInt32(dr["Pago"].ToString());
                string tipoRet = dr["TipoRetencion"].ToString();

                DataTable dtCanceladas = this.controlador.obtenerDocumentosCancelados(Convert.ToInt32(dr["idMov"]));
                //List<Gestion_Api.Entitys.Compra> ComprasCanceladas = this.contCCProvedor.obtenerMovimientosPagosComprasByPago(idPago, idProveedor, empresa, sucursal, ptoVenta);
                //DataTable dtComprasCanceladas = ListToDataTable(ComprasCanceladas);                

                //if (dtComprasCanceladas.Rows.Count > 0 && dtComprasCanceladas != null)
                //{
                //    foreach (DataRow row in dtComprasCanceladas.Rows)
                //    {

                //    }
                //}                


                //Fecha Retencion
                string fechaRet = dr["fecha"].ToString();

                //Datos Cliente                
                string cliente = dr["Cliente"].ToString();
                DataTable dtCliente = this.contCliente.obtenerDireccionClienteNombreOrigen(cliente, 1);//1 = cliente

                //obtengo datos empresa.
                DataTable dtEmpresa = this.contEmpresa.obtenerEmpresaById(Convert.ToInt32(drRetencionEmpresa["Empresa"].ToString()));
                DataRow drEmpresa = dtEmpresa.Rows[0];

                //cargo datos empresa
                string cuit = drEmpresa["Cuit"].ToString();
                string razonSocial = drEmpresa["Razon Social"].ToString();
                string IIBB = drEmpresa["Ingresos Brutos"].ToString();
                string fechaInicio = drEmpresa["Fecha inicio"].ToString();
                string condIva = drEmpresa["Condicion IVA"].ToString();
                string direccion = drEmpresa["Direccion"].ToString();

                string[] lugar = direccion.Split('-');
                string localidad = "-";
                string provincia = "-";

                if (lugar.Count() > 2)
                {
                    localidad = lugar[1].ToString();
                    provincia = lugar[2].ToString();
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RetencionCobros.rdlc");

                ReportDataSource rds = new ReportDataSource("dsRetencion", dtRetencion);
                //ReportDataSource rds2 = new ReportDataSource("dsEmpresa", dtEmpresa);
                ReportDataSource rds3 = new ReportDataSource("dsCliente", dtCliente);
                ReportDataSource rds4 = new ReportDataSource("dsDocumentos", dtCanceladas);

                ReportParameter param = new ReportParameter("ParamCliente", cliente);
                ReportParameter param1 = new ReportParameter("ParamNumero", numero);

                //param con datos empresa
                ReportParameter param2 = new ReportParameter("ParamCuit", cuit);
                ReportParameter param3 = new ReportParameter("ParamRazonSocial", razonSocial);
                ReportParameter param4 = new ReportParameter("ParamIIBB", IIBB);
                ReportParameter param5 = new ReportParameter("ParamFechaInicio", fechaInicio);
                ReportParameter param6 = new ReportParameter("ParamCondIva", condIva);
                ReportParameter param7 = new ReportParameter("ParamDireccion", direccion);
                ReportParameter param8 = new ReportParameter("ParamLocalidad", localidad);
                ReportParameter param9 = new ReportParameter("ParamProvincia", provincia);
                //param fecha retencion
                ReportParameter param10 = new ReportParameter("ParamFecha", Convert.ToDateTime(fechaRet).ToString("dd/MM/yyyy"));
                ReportParameter param11 = new ReportParameter("ParamTipoRetencion", tipoRet);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);

                this.ReportViewer1.LocalReport.SetParameters(param);
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

            }
        }

        private void generarReporte2()
        {
            try
            {
                controladorReportes contReport = new controladorReportes();

                String rutaTxt = Server.MapPath("RetencionesCompras.txt");
                String comprobante = contReport.generarRetencionesPago(fechaD, fechaH, suc, "P", rutaTxt);

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
                    this.Response.AddHeader("Content-disposition", "attachment; filename= " + DateTime.Today.Date.ToShortDateString() + "_Retenciones.txt");
                    this.Response.BinaryWrite(btFile);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void generarReporte3(string saldo)//Reporte listado de retenciones
        {
            try
            {
                DataTable dtRetencionesRealizadas = this.controlador.obtenerCobrosRetenciones(Convert.ToDateTime(fechaD, new System.Globalization.CultureInfo("es-AR")).ToString(), Convert.ToDateTime(fechaH, new System.Globalization.CultureInfo("es-AR")).ToString(), this.suc, "C");
                if (dtRetencionesRealizadas != null)
                {
                    dtRetencionesRealizadas.Columns.Add("Documento");
                    
                    foreach (DataRow dr in dtRetencionesRealizadas.Rows)
                    {
                        DataTable dtCobroRetencion = this.controlador.obtenerCobrosRetencionesById(Convert.ToInt32(dr["id"]));
                        DataRow drCobroRetencion = dtCobroRetencion.Rows[0];
                        DataTable dtCanceladas = this.controlador.obtenerDocumentosCancelados(Convert.ToInt32(drCobroRetencion["idMov"]));
                        string documentos = "";
                        foreach (DataRow dr2 in dtCanceladas.Rows)
                        {
                            documentos += dr2[0].ToString() + " - ";
                        }
                        dr["Documento"] = documentos.Substring(0, documentos.Length - 3);
                    }

                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RetencionesListado.rdlc");
                    ReportDataSource rds = new ReportDataSource("DetalleRetenciones", dtRetencionesRealizadas);
                    ReportParameter param = new ReportParameter("ParamSaldo", saldo);

                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;


                    string[] streams;

                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "DetalleRetenciones", "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
            }
            catch
            {

            }
        } 
        #endregion

        #region Funciones Auxiliares
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
        #endregion
    }
}