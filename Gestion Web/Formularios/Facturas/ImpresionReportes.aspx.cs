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
    public partial class ImpresionReportes : System.Web.UI.Page
    {
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        Mensajes mje = new Mensajes();
        Mensajes m = new Mensajes();
        

        int idCliente;
        int idSucursal;
        int idTipo;
        int accion;
        int excel;

        int idDespacho;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                    this.idSucursal = Convert.ToInt32(Request.QueryString["Sucursal"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.idTipo = Convert.ToInt32(Request.QueryString["Tipo"]);
                    this.excel = Convert.ToInt32(Request.QueryString["e"]);

                    this.idDespacho = Convert.ToInt32(Request.QueryString["Desp"]);

                    if (idCliente > 0)
                    {
                        this.generarReportes(idCliente, idSucursal, idTipo);
                    }
                    if (accion == 1)
                    {
                        this.generarReporteDespacho();

                        
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al generar reporte. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al imprimir reporte. " + ex.Message);
            }
            

        }

        private void generarReportes(int idCliente, int idSucursal, int idTipo)
        {
            try
            {
                
                //DataTable dtDatos = controlador.obtenerMovimientosFacturaByCuentaDT(idCliente, idSucursal, idTipo);
                //DataTable dtDatos2 = controlador.obtenerMovimientosCobroByCuentaDT(idCliente, idSucursal, idTipo);
                //dtDatos.Merge(dtDatos2);
                controladorCliente controlador = new controladorCliente();

                var cliente = controlador.obtenerClienteID(idCliente);

                DataTable dtDatos = Session["datosMov"] as DataTable;
                Session["datosMov"] = null;
                String lblSaldo = Session["saldoMov"] as String;
                Session["saldoMov"] = null;

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Report2.rdlc");

                ReportDataSource rds2 = new ReportDataSource("dsCtaCorriente", dtDatos);
                ReportParameter param = new ReportParameter("ParamCliente", cliente.razonSocial + " - " + cliente.codigo);
                ReportParameter param2 = new ReportParameter("ParamSaldo",lblSaldo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);                
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "DetalleCuentaCorriente", "xls");

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error imprimiendo movimientos. " + ex.Message));
            }
        }

        private void generarReporteDespacho()
        {
            int idDesp = this.idDespacho;
            
            controladorDespacho controlDespacho = new controladorDespacho();
            Despacho d = controlDespacho.obtenerDespachoPorID(idDesp);

            DataTable dtDespacho = new DataTable();
            dtDespacho.Columns.Add("ID");
            dtDespacho.Columns.Add("Fecha");
            dtDespacho.Columns.Add("Valor");
            dtDespacho.Columns.Add("Numero");
            dtDespacho.Columns.Add("Expreso");
            dtDespacho.Columns.Add("contrareembolso");

            DataRow dr = dtDespacho.NewRow();
            dr[0] = d.ID;
            dr[1] = d.Fecha.Value.ToString("dd/MM/yyyy");
            dr[2] = d.Valor;
            dr[3] = d.numero;
            dr[4] = d.expreso;
            dr[5] = d.contrareembolso;

            dtDespacho.Rows.Add(dr);

            DataTable dtDetalles = ListToDataTable(d.detalleDespachoes.ToList());

            controladorFacturacion contFact = new controladorFacturacion();
            controladorCliente contCl = new controladorCliente();
            Factura fact = contFact.obtenerFacturaId(d.Facturas_Despachos.FirstOrDefault().Factura);
            DataTable dtDireccion = contCl.obtenerDireccionClienteNombre(fact.cliente.razonSocial);
            String calle = String.Empty;
            String localidad = String.Empty;

            if (dtDireccion.Rows.Count > 0)
            {
                DataRow row = dtDireccion.Rows[0];
                calle = row["Direccion"].ToString();
                localidad = row["localidad"].ToString();
            }
            else
            {
                calle = "-";
                localidad = "-";
            }

            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("DespachosR.rdlc");

            ReportDataSource rds1 = new ReportDataSource("DatosDespacho", dtDespacho);
            ReportDataSource rds2 = new ReportDataSource("DetalleDespacho", dtDetalles);
            ReportParameter param = new ReportParameter("ParamNumero", d.numero.ToString().PadLeft(8,'0'));
            ReportParameter param1 = new ReportParameter("ParamDestinatario", fact.cliente.razonSocial);//fact.cliente.alias
            ReportParameter param2 = new ReportParameter("ParamCalle", calle);
            ReportParameter param3 = new ReportParameter("ParamLocalidad", localidad);

            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);
            this.ReportViewer1.LocalReport.DataSources.Add(rds2);
            this.ReportViewer1.LocalReport.SetParameters(param);
            this.ReportViewer1.LocalReport.SetParameters(param1);
            this.ReportViewer1.LocalReport.SetParameters(param2);
            this.ReportViewer1.LocalReport.SetParameters(param3);
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