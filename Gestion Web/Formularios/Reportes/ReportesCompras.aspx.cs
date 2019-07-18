using Disipar.Models;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReportesCompras : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorCliente controladorCliente = new controladorCliente();
        controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();

        private string fechaDesde;
        private string fechaHasta;
        private int proveedor;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                fechaDesde = Request.QueryString["fd"];
                fechaHasta = Request.QueryString["fh"];
                proveedor = Convert.ToInt32(Request.QueryString["p"]);

                if (!IsPostBack)
                {
                    cargarProveedores();

                    if (string.IsNullOrEmpty(fechaDesde) && string.IsNullOrEmpty(fechaHasta))
                    {
                        fechaDesde = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaHasta = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");

                        Response.Redirect("ReportesCompras.aspx?fd=" + fechaDesde + "&fh=" + fechaHasta + "&p=" + proveedor);
                    }

                    txtFechaDesde.Text = fechaDesde;
                    txtFechaHasta.Text = fechaHasta;
                    DropListProveedor.SelectedValue = proveedor.ToString();
                }

                CargarDatosRango();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando página Reportes Compras. Excepción: " + ex.Message));
            }
        }

        #region Eventos Controles
        protected void lbtnReporteArticulosPorProveedor_Click(object sender, EventArgs e)
        {
            this.ImprimirReporteComprasArticulos(0);
        }

        protected void lbtnReporteArticulosPorProveedorPDF_Click(object sender, EventArgs e)
        {
            this.ImprimirReporteComprasArticulos(1);
        }

        protected void lbtnReporteArticulosCompradosVendidosPorProveedor_Click(object sender, EventArgs e)
        {
            this.ImprimirReporteComprasVentasArticulos(0);
        }

        protected void lbtnReporteArticulosCompradosVendidosPorProveedorPDF_Click(object sender, EventArgs e)
        {
            this.ImprimirReporteComprasVentasArticulos(1);
        }

        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtProveedor.Text.Replace(' ', '%');
                DataTable dt = controladorCliente.obtenerProveedoresAliasDT(buscar);

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["id"] = -1;
                        dt.Rows.InsertAt(dr, 0);

                        if (string.IsNullOrEmpty(buscar))
                        {
                            DataRow dr2 = dt.NewRow();
                            dr2["alias"] = "Todos";
                            dr2["id"] = 0;
                            dt.Rows.InsertAt(dr2, 1);
                        }
                    }

                    this.DropListProveedor.DataSource = dt;
                    this.DropListProveedor.DataValueField = "id";
                    this.DropListProveedor.DataTextField = "alias";
                    this.DropListProveedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando proveedor a la lista. Excepción: " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ReportesCompras.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Carga Inicial
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
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarProveedores()
        {
            try
            {
                DataTable dt = controladorCliente.obtenerProveedoresReducDT();

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["id"] = -1;
                        dt.Rows.InsertAt(dr, 0);

                        DataRow dr2 = dt.NewRow();
                        dr2["alias"] = "Todos";
                        dr2["id"] = 0;
                        dt.Rows.InsertAt(dr2, 1);
                    }

                    DropListProveedor.DataSource = dt;
                    DropListProveedor.DataValueField = "id";
                    DropListProveedor.DataTextField = "alias";

                    this.DropListProveedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando Proveedores. Excepción: " + ex.Message));
            }
        }
        #endregion

        #region Carga de Tablas
        private void CargarDatosRango()
        {
            try
            {
                CargarTablaTopArticulosCantidad();
                CargarTablaTopProveedoresCantidad();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando datos en pantalla. Excepción: " + ex.Message));
            }
        }

        private void CargarTablaTopArticulosCantidad()
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaH = Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));

                var dt = controladorCompraEntity.obtenerTopRemitosCompras_ItemsFiltro(fechaD, fechaH, proveedor);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CargarTablaTopArticulosCantidadPh(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error obteniendo top de articulos para cargar en la tabla top articulos. Excepción: " + ex.Message));
            }
        }

        private void CargarTablaTopArticulosCantidadPh(DataRow dr)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["CodigoArticulo"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(30);
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["Descripcion"].ToString();
                celDescripcion.VerticalAlign = VerticalAlign.Bottom;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.Width = Unit.Percentage(40);
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = Decimal.Round(Convert.ToDecimal(dr["Cantidad"]), 2).ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopArticulosCantidad.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando articulo en tabla top articulos. Excepción: " + ex.Message));
            }
        }

        private void CargarTablaTopProveedoresCantidad()
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaH = Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));

                var dt = controladorCompraEntity.obtenerTopRemitosProveedores_ItemsFiltro(fechaD, fechaH, proveedor);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CargarTablaTopProveedoresCantidadPh(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error obteniendo top de articulos para cargar en la tabla top articulos. Excepción: " + ex.Message));
            }
        }

        private void CargarTablaTopProveedoresCantidadPh(DataRow dr)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celNombreProveedor = new TableCell();
                celNombreProveedor.Text = dr["razonSocial"].ToString();
                celNombreProveedor.VerticalAlign = VerticalAlign.Bottom;
                celNombreProveedor.HorizontalAlign = HorizontalAlign.Left;
                celNombreProveedor.Width = Unit.Percentage(30);
                tr.Cells.Add(celNombreProveedor);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = Decimal.Round(Convert.ToDecimal(dr["cantidadComprada"]), 2).ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopProveedoresCantidad.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error en fun: CargarTablaTopProveedoresCantidadPh. Excepción: " + ex.Message));
            }
        }
        #endregion

        #region Impresion

        private void ImprimirReporteComprasArticulos(int tipoImpresion)
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaH = Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));

                // Impresión PDF
                if (tipoImpresion == 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=11&fd=" + fechaD.ToString("dd/MM/yyyy") + "&fh=" + fechaH.ToString("dd/MM/yyyy") + " &prov=" + proveedor + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }

                // Impresión Excel
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=11&ex=1&fd=" + fechaD.ToString("dd/MM/yyyy") + "&fh=" + fechaH.ToString("dd/MM/yyyy") + " &prov=" + proveedor + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error enviando a imprimir Reporte de Compras por Articulo. Excepción: " + ex.Message));
            }
        }

        private void ImprimirReporteComprasVentasArticulos(int tipoImpresion)
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaH = Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));

                // Impresión PDF
                if (tipoImpresion == 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=13&fd=" + fechaD.ToString("dd/MM/yyyy") + "&fh=" + fechaH.ToString("dd/MM/yyyy") + " &prov=" + proveedor + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }

                // Impresión Excel
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=13&ex=1&fd=" + fechaD.ToString("dd/MM/yyyy") + "&fh=" + fechaH.ToString("dd/MM/yyyy") + " &prov=" + proveedor + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error en fun: ImprimirReporteComprasVentasArticulos. Excepción: " + ex.Message));
            }
        }
        #endregion

    }
}