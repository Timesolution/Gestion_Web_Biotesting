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

namespace Gestion_Web.Formularios.OrdenReparacion
{
    using Gestion_Api.Entitys;

    public partial class ImpresionOrdenReparacion : System.Web.UI.Page
    {
        private int accion;
        private int excel;
        private int ordenReparacion;
        private int idPresupuesto;

        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();

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
                    this.generarImpresion();
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
                        if (s == "57")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void generarImpresion()
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

                DataTable dtEmpresa = contEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                foreach (DataRow row in dtEmpresa.Rows)
                {
                    cuitEmpresa = row.ItemArray[1].ToString();
                    razonSoc = row.ItemArray[2].ToString();
                    condIVA = row.ItemArray[5].ToString();
                    direComer = row.ItemArray[6].ToString();
                }

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(ordenReparacion);

                DataTable dtOrdenReparacion = new DataTable();

                dtOrdenReparacion = contOrdenReparacion.ObtenerOrdenesReparacionDT(or.Id);        

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("OrdenReparacionR.rdlc");

                ReportDataSource rds = new ReportDataSource("DatosOrdenReparacion", dtOrdenReparacion);

                ReportParameter param = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param2 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param3 = new ReportParameter("ParamCondIva", condIVA);
                //ReportParameter param4 = new ReportParameter("ParamNroOR", or.NumeroOrdenReparacion.Value.ToString("D8"));

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                //this.ReportViewer1.LocalReport.SetParameters(param4);
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
    }
}