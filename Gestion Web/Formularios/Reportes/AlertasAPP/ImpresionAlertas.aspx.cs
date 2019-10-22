using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Controladores.APP;
using Gestor_Solution.Controladores;
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

namespace Gestion_Web.Formularios.Reportes.AlertasAPP
{
    public partial class ImpresionAlertas : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        public string FechaDesde;
        public string FechaHasta;
        public int IdCliente;
        public int IdVendedor;
        public int TipoAlerta;
        public int EstadoAlerta;
        int excel;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FechaDesde = Request.QueryString["fd"];
                    FechaHasta = Request.QueryString["fh"];
                    IdCliente = Convert.ToInt32(Request.QueryString["c"]);
                    IdVendedor = Convert.ToInt32(Request.QueryString["v"]);
                    TipoAlerta = Convert.ToInt32(Request.QueryString["ta"]);
                    EstadoAlerta = Convert.ToInt32(Request.QueryString["ea"]);
                    excel = Convert.ToInt32(Request.QueryString["excel"]);
                    generarReporte();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void generarReporte()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                controladorCliente controladorCliente = new controladorCliente();
                controladorDireccion controladorDireccion = new controladorDireccion();
                ControladorAlertaAPP controladorAlertaAPP = new ControladorAlertaAPP();
                DateTime fechaDesde = Convert.ToDateTime(FechaDesde, new CultureInfo("es-AR"));
                DateTime fechaHasta = Convert.ToDateTime(FechaHasta, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59).AddSeconds(59);

                List<AlertasAPPEstructura> alertas = controladorAlertaAPP.ObtenerAlertas(fechaDesde, fechaHasta, IdCliente, IdVendedor, TipoAlerta, EstadoAlerta);

                DataTable dt = new DataTable();
                dt.Columns.Add("cliente", typeof(string));
                dt.Columns.Add("estado", typeof(string));
                dt.Columns.Add("fecha", typeof(string));
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("mensaje", typeof(string));
                dt.Columns.Add("tipoAlerta", typeof(string));
                dt.Columns.Add("vendedor", typeof(string));
                dt.Columns.Add("direccion", typeof(string));
                dt.Columns.Add("localidad", typeof(string));
                dt.Columns.Add("codigoPostal", typeof(string));

                foreach (AlertasAPPEstructura item in alertas)
                {
                    Gestion_Api.Entitys.AlertasAPP alertasAPP = controladorAlertaAPP.ObtenerAlertaById(item.id);
                    var cliente = controladorCliente.obtenerClienteID((int)alertasAPP.IdCliente);

                    DataRow dr = dt.NewRow();
                    dr["cliente"] = item.cliente;
                    dr["estado"] = item.estado;
                    dr["fecha"] = item.fecha.ToString("dd/MM/yyyy") + " " + item.fecha.AddHours(-3).ToString("HH:mm");
                    dr["id"] = item.id;
                    dr["mensaje"] = item.mensaje;
                    dr["tipoAlerta"] = item.tipoAlerta;
                    dr["vendedor"] = item.vendedor;
                    dr["direccion"] = "";
                    dr["localidad"] = "";
                    dr["codigoPostal"] = "";
                    if (cliente.direcciones.Count > 0)
                    {
                        dr["direccion"] = cliente.direcciones.FirstOrDefault().direc;
                        dr["localidad"] = cliente.direcciones.FirstOrDefault().localidad;
                        dr["codigoPostal"] = cliente.direcciones.FirstOrDefault().codPostal;
                    }
                    dt.Rows.Add(dr);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ImpresionAlertas.rdlc");

                ReportDataSource rds = new ReportDataSource("ImpresionAlertas", dt);

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
                    String filename = string.Format("{0}.{1}", "StockPorTalles" + DateTime.Today.ToString("dd/MM/yyyy"), "xls");

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