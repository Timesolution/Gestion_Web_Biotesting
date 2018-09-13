

namespace Gestion_Web.Formularios.OrdenReparacion
{
    using Gestion_Api.Entitys;
    using Gestion_Api.Modelo;
    using iTextSharp.text.pdf;
    using System.IO;
    using System.Net.Mail;
    using Gestion_Api.Controladores;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class ImpresionOrdenReparacion : System.Web.UI.Page
    {
        private int accion;
        private int excel;
        public int ordenReparacion;
        public int idPresupuesto;
        public int enviarPorMail = 0;
        ReportViewer rv = new ReportViewer();

        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();
        controladorArticulo contArt = new controladorArticulo();
        controladorSucursal contSuc = new controladorSucursal();
        ControladorClienteEntity contCliente = new ControladorClienteEntity();

        protected void Page_Load(object sender, EventArgs e)
        {

            this.VerificarLogin();

            if (!IsPostBack)
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                this.ordenReparacion = Convert.ToInt32(Request.QueryString["or"]);
                this.idPresupuesto = Convert.ToInt32(Request.QueryString["prp"]);

                if (accion == 1)
                {
                    this.GenerarImpresion();
                }
                else if(accion == 2)
                {
                    this.GenerarEtiqueta();
                }
            }
        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
                else
                {
                    if (this.verificarAcceso() != 1)
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Herramientas.Presupuesto") != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        return 1;
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void GenerarEtiqueta()
        {
            try
            {
                var or = this.contOrdenReparacion.ObtenerOrdenReparacionPorID(ordenReparacion);

                DataTable dtOR = new DataTable();
                dtOR.Columns.Add("NumeroSerie");
                dtOR.Columns.Add("Fecha");
                dtOR.Columns.Add("Producto");
                dtOR.Columns.Add("NumeroOrdenReparacion");
                dtOR.Columns.Add("SucursalOrigen");

                DataRow drItem = dtOR.NewRow();

                drItem[0] = or.NumeroSerie;
                drItem[1] = or.Fecha.Value.ToString("dd/MM/yyyy");
                drItem[2] = contArt.obtenerArticuloByID((int)or.Producto);
                drItem[3] = or.NumeroOrdenReparacion.Value.ToString("D8");
                drItem[4] = contSuc.obtenerSucursalID((int)or.SucursalOrigen).nombre;

                dtOR.Rows.Add(drItem);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("EtiquetasOR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                

                ReportDataSource rds = new ReportDataSource("OrdenReparacion", dtOR);
                
                string imagen = generarCodigo((int)or.NumeroOrdenReparacion); 
                ReportParameter param = new ReportParameter("ParamCodBarra", @"file:///" + imagen);

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

                    String filename = string.Format("{0}.{1}", "EtiquetasOrdenReparacion", "xls");

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
                Log.EscribirSQL(1, "ERROR", "Error al generar etiqueta. " + ex.Message);
            }
        }
        public string generarCodigo(int idOR)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = idOR.ToString();

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/OrdenesReparacion/" + idOR + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + idOR + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                //Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para pedido. " + ex.Message);
                return null;
            }
        }
        public int GenerarImpresion()
        {
            try
            {
                DataTable dtDetalle = contFacturacion.obtenerDetallePresupuesto(idPresupuesto);

                var drDatosFactura = dtDetalle.Rows[0];

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;
                //String ingBrutos = String.Empty;
                //String fechaInicio = String.Empty;
                String cuitEmpresa = String.Empty;
                int idEmpresa = 0;

                DataTable dtEmpresa = contEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                foreach (DataRow row in dtEmpresa.Rows)
                {
                    cuitEmpresa = row.ItemArray[1].ToString();
                    razonSoc = row.ItemArray[2].ToString();
                    condIVA = row.ItemArray[5].ToString();
                    direComer = row.ItemArray[6].ToString();
                    idEmpresa = Convert.ToInt32(row.ItemArray[0]);
                }

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(ordenReparacion);
                var or_st = contOrdenReparacion.ObtenerOrdenReparacion_ServicioTecnicoPorORdenReparacionID(ordenReparacion);

                DataTable dtOrdenReparacion = new DataTable();

                dtOrdenReparacion = contOrdenReparacion.ObtenerOrdenesReparacionDT(or.Id);

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = Server.MapPath("OrdenReparacionR.rdlc");
                rv.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("DatosOrdenReparacion", dtOrdenReparacion);                               

                ReportParameter param = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param2 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param3 = new ReportParameter("ParamCondIva", condIVA);
                //ReportParameter param4 = new ReportParameter("ParamNroOR", or.NumeroOrdenReparacion.Value.ToString("D8"));
                string imagen = generarCodigo((int)or.NumeroOrdenReparacion);
                ReportParameter param4 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);
                ReportParameter param5 = new ReportParameter("ParamEstadoOR", contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID((int)or.Estado).Descripcion);                                    

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.SetParameters(param);
                rv.LocalReport.SetParameters(param2);
                rv.LocalReport.SetParameters(param3);
                rv.LocalReport.SetParameters(param4);
                rv.LocalReport.SetParameters(param5);

                //si se le asigno un servicio tecnico, entonces muestro el numero de orden de reparacion que nos dio el servicio tecnico                
                if (or_st != null)
                {
                    ReportParameter param6 = new ReportParameter("ParamOrdenReparacionST", or_st.NumeroOrden);
                    ReportParameter param7 = new ReportParameter("ParamTextOrdenReparacionST", "Numero de Orden de Reparacion de Servicio Tecnico: ");
                    rv.LocalReport.SetParameters(param6);
                    rv.LocalReport.SetParameters(param7);
                }

                rv.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = rv.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

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
                    Byte[] pdfContent = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    if (enviarPorMail > 0)
                    {
                        EnviarORMail(pdfContent, or, idEmpresa);
                        return 1;
                    }
                    else
                    {
                        this.Response.Clear();
                        this.Response.Buffer = true;
                        this.Response.ContentType = "application/pdf";
                        this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                        this.Response.BinaryWrite(pdfContent);
                        this.Response.End();
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al generar impresion. " + ex.Message);
                return -1;
            }
        }

        public void EnviarORMail(Byte[] pdfContent, OrdenReparacion or, int idEmpresa)
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();

                var datosCliente = contCliente.obtenerClienteDatosByCliente((int)or.Cliente);

                //save the generated report in the server
                String path = Server.MapPath("../OrdenReparacion/" + idEmpresa + "/or-" + or.NumeroOrdenReparacion + "_" + or.Id + ".pdf");
                String folder = Server.MapPath("../OrdenReparacion/" + idEmpresa);

                if (!Directory.Exists(folder))
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "No existe directorio. " + folder + ". lo creo");              
                    Directory.CreateDirectory(folder);
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "directorio creado");
                }

                FileStream stream = File.Create(path, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                Attachment adjunto = new Attachment(path);

                var mail = datosCliente.Select(x => x.Mail).FirstOrDefault();

                int i = contFunciones.enviarMailOrdenReparacion(adjunto, or, mail);

                if (i > 0)
                {
                    adjunto.Dispose();
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al enviar la impresion por mail. " + ex.Message);
            }
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
    }
}