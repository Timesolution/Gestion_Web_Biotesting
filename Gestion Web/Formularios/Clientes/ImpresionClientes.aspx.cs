using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Gestion_Web.Formularios.Clientes
{
    public partial class ImpresionClientes : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private string clientes;
        private int excel;
        private int accion;
        private int vendedor;
        controladorCliente controlador = new controladorCliente();
        ControladorClienteEntity contEntity = new ControladorClienteEntity();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.clientes = Request.QueryString["cli"];
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.vendedor = Convert.ToInt32(Request.QueryString["ven"]);
                    if (accion == 0)
                    {
                        this.generarReporte(clientes);
                    }
                    if (accion == 1)
                    {
                        this.generarReporte2();
                    }
                    if (accion == 2)//reporte por provincia
                    {
                        this.generarReporte3();
                    }
                    if (accion == 3)//reporte por zona cliente
                    {
                        this.generarReporte4();
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pedido. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pedido. " + ex.Message);
            }
        }

        #region Reportes
        private void generarReporte(string clientes)
        {
            try
            {
                DataTable dtClientes = new DataTable();
                if (clientes != null && clientes != "")
                {
                    dtClientes = controlador.obtenerClientesAliasDTReporte(clientes);
                }
                else
                {
                    //dtClientes = controlador.obtenerClientesReducDT();
                    dtClientes = controlador.obtenerClientesAliasDTReporte("%");
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeClientes.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleCliente", dtClientes);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "ListadoClientes", "xls");

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

        private void generarReporte2()
        {
            try
            {
                DataTable dtClientes = new DataTable();
                dtClientes = this.controlador.obtenerClientesByVendedorDT(this.vendedor);

                dtClientes.Columns.Add("Telefono");
                dtClientes.Columns.Add("Mail");
                dtClientes.Columns.Add("Direccion");
                dtClientes.Columns.Add("Localidad");
                dtClientes.Columns.Add("Provincia");

                if (dtClientes.Rows.Count > 0)
                {
                    foreach (DataRow row in dtClientes.Rows)
                    {
                        DataTable dtTelefono = this.controlador.obtenerContactoCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtTelefono.Rows.Count > 0)
                        {
                            row["Telefono"] = dtTelefono.Rows[0].ItemArray[1].ToString();
                            row["Mail"] = dtTelefono.Rows[0].ItemArray[2].ToString();
                        }
                        DataTable dtDireccion = this.controlador.obtenerDireccionCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtDireccion.Rows.Count > 0)
                        {
                            row["Direccion"] = dtDireccion.Rows[0]["direccion"].ToString();
                            row["Localidad"] = dtDireccion.Rows[0]["localidad"].ToString();
                            row["Provincia"] = dtDireccion.Rows[0]["provincia"].ToString();

                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeClientesVendedorR.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleCliente", dtClientes);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();
                //    }
                //}

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "ListadoClientes", "xls");

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

        private void generarReporte3()
        {
            try
            {
                DataTable dtClientes = new DataTable();
                if (clientes != null && clientes != "")
                {
                    dtClientes = controlador.obtenerClientesAliasDT(clientes);
                }
                else
                {
                    dtClientes = controlador.obtenerClientesReducDT();
                }

                dtClientes.Columns.Add("Telefono");
                dtClientes.Columns.Add("Mail");
                dtClientes.Columns.Add("Direccion");
                dtClientes.Columns.Add("Localidad");
                dtClientes.Columns.Add("Provincia");

                if (dtClientes.Rows.Count > 0)
                {
                    foreach (DataRow row in dtClientes.Rows)
                    {
                        DataTable dtTelefono = this.controlador.obtenerContactoCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtTelefono.Rows.Count > 0)
                        {
                            row["Telefono"] = dtTelefono.Rows[0].ItemArray[1].ToString();
                            row["Mail"] = dtTelefono.Rows[0].ItemArray[2].ToString();
                        }
                        DataTable dtDireccion = this.controlador.obtenerDireccionCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtDireccion.Rows.Count > 0)
                        {
                            row["Direccion"] = dtDireccion.Rows[0]["direccion"].ToString();
                            row["Localidad"] = dtDireccion.Rows[0]["localidad"].ToString();
                            row["Provincia"] = dtDireccion.Rows[0]["provincia"].ToString();

                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeClientesZonaR2.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleCliente", dtClientes);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();
                //    }
                //}

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "ListadoClientes", "xls");

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

        private void generarReporte4()
        {
            try
            {
                DataTable dtClientes = new DataTable();
                if (clientes != null && clientes != "")
                {
                    dtClientes = controlador.obtenerClientesAliasDT(clientes);
                }
                else
                {
                    //dtClientes = controlador.obtenerClientesReducDT();
                    //Si el parametro clientes me viene vacío o null, le mando a que me traiga todos los clientes
                    dtClientes = controlador.obtenerClientesAliasDT("%");
                }

                dtClientes.Columns.Add("Telefono");
                dtClientes.Columns.Add("Mail");
                dtClientes.Columns.Add("Direccion");
                dtClientes.Columns.Add("Localidad");
                dtClientes.Columns.Add("Provincia");
                dtClientes.Columns.Add("Zona");
                dtClientes.Columns.Add("Direccion2"); //Se agrega esta columna para no tocar lo que estaba hecho.

                if (dtClientes.Rows.Count > 0)
                {
                    foreach (DataRow row in dtClientes.Rows)
                    {
                        DataTable dtTelefono = this.controlador.obtenerContactoCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtTelefono.Rows.Count > 0)
                        {
                            row["Telefono"] = dtTelefono.Rows[0].ItemArray[1].ToString();
                            row["Mail"] = dtTelefono.Rows[0].ItemArray[2].ToString();
                        }
                        DataTable dtDireccion = this.controlador.obtenerDireccionCliente(Convert.ToInt32(row["id"]), 1);
                        if (dtDireccion.Rows.Count > 0)
                        {
                            row["Direccion"] = dtDireccion.Rows[0]["direccion"].ToString();
                            row["Localidad"] = dtDireccion.Rows[0]["localidad"].ToString();
                            row["Provincia"] = dtDireccion.Rows[0]["provincia"].ToString();
                            row["Direccion2"] = dtDireccion.Rows[0]["direccion"].ToString();
                        }

                        var entrega = this.contEntity.obtenerEntregaCliente(Convert.ToInt32(row["id"]));
                        if (entrega != null)
                        {
                            row["Zona"] = entrega.Zona.nombre;
                        }
                        else
                        {
                            row["Zona"] = "-";
                        }
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeClientesZonaR.rdlc");

                ReportDataSource rds = new ReportDataSource("DetalleCliente", dtClientes);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();
                //    }
                //}

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    String filename = string.Format("{0}.{1}", "ListadoClientes", "xls");

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
        #endregion

    }
}