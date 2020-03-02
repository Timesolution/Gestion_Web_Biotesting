using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class CCCompraF : System.Web.UI.Page
    {
        //mensajes popUp
        Mensajes m = new Mensajes();

        ControladorCCProveedor controlador = new ControladorCCProveedor();
        controladorCliente contCliente = new controladorCliente();
        controladorCompraEntity contCompra = new controladorCompraEntity();
        Gestion_Api.Modelo.Configuracion configuracion = new Gestion_Api.Modelo.Configuracion();

        int idProveedor;
        int sucursal;
        int tipo;
        int tipoDocumento;
        string fechaDesde;
        string fechaHasta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idProveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.tipo = Convert.ToInt32(Request.QueryString["t"]);
                this.fechaDesde = Request.QueryString["fd"];
                this.fechaHasta = Request.QueryString["fh"];
                this.tipoDocumento = Convert.ToInt32(Request.QueryString["td"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaDesde) && String.IsNullOrEmpty(this.fechaHasta))
                    {
                        this.fechaDesde = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaHasta = DateTime.Today.ToString("dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(configuracion.FechaFiltrosCuentaCorriente))
                            this.fechaDesde = configuracion.FechaFiltrosCuentaCorriente.Substring(0, 10).Replace(";", "/");
                    }

                    this.txtFechaDesde.Text = this.fechaDesde;
                    this.txtFechaHasta.Text = this.fechaHasta;

                    this.cargarProveedores();
                    this.cargarSucursal();
                }
                if (idProveedor > 0)
                {
                    lblParametros.Text = "Proveedor: " + this.contCliente.obtenerProveedorID(idProveedor).alias;
                    this.cargarMovimientos(idProveedor);
                }
                else
                {
                    lblParametros.Text = "Seleccione Proveedor.";
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina. " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Articulos.Articulos") != 1)
                    if (this.verificarAcceso() != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                    else
                    {
                       
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
                        if (s == "32")
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        private void cargarMovimientos(int proveedor)
        {
            try
            {
                DateTime fdesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                var mov = this.controlador.obtenerMovimientosProveedorByBN(proveedor, this.sucursal, this.tipo, fdesde, fhasta, this.tipoDocumento);
                mov = mov.OrderBy(x => x.Fecha).ToList();

                this.phCuentaCorriente.Controls.Clear();
                decimal saldoAcumulado = 0;
                decimal saldoVencimiento = 0;
                foreach (var m in mov)
                {
                    if (m.Debe > 0)
                    {
                        saldoAcumulado += (decimal)m.Debe;
                    }
                    if (m.Haber > 0)
                    {
                        saldoAcumulado -= (decimal)m.Haber;                        
                    }
                    if (m.TipoDocumento.Value == 19)
                    {
                        if (m.Compra.Vencimiento == (DateTime.Today.AddDays(1)) || m.Compra.Vencimiento == DateTime.Today)
                        {
                            saldoVencimiento += (decimal)m.Saldo;
                        }
                    }
                    this.cargarMovimientoPH(m, saldoAcumulado);                    
                }
                this.lblSaldo.Text = "$ " + saldoAcumulado.ToString("N");
                this.lblVencimiento.Text = "$ " + saldoVencimiento.ToString("N");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }
        private void cargarMovimientoPH(MovimientosCCP m, decimal saldoAcumulado)
        {
            try
            {
                TableRow tr = new TableRow();
                string tipoDocumento = " FC ";
                if (m.Ftp == 1)
                    tipoDocumento = " PRP ";
                if (m.Ftp == 2)
                    tipoDocumento = " ";
                

                tr.ID = m.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(m.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();

                if (m.TipoDocumento == 19)
                {
                    celNumero.Text = "<a href=\"ComprasABM.aspx?a=1&i=" + m.Documento + "\" target=\"_blank\">" + m.Compra.TipoDocumento + tipoDocumento + m.Numero + "</a>";
                }
                else if (m.TipoDocumento == 21)
                {
                    celNumero.Text = "<a href=\"../Pagos/ReportesR.Aspx?a=0&id=" + m.Documento + "\" target=\"_blank\">" + m.tipoDocumento1.tipo + tipoDocumento + m.Numero + "</a>";
                }
                else
                {
                    celNumero.Text = "<a href=\"ComprasABM.aspx?a=1&i=" + m.Documento + "\" target=\"_blank\">" + m.tipoDocumento1.tipo + tipoDocumento + m.Numero + "</a>";
                }


                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + m.Debe;
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + m.Haber;
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + m.Saldo;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldo);

                TableCell celSaldoAcumulado = new TableCell();
                celSaldoAcumulado.Text = "$" + saldoAcumulado;
                celSaldoAcumulado.VerticalAlign = VerticalAlign.Middle;
                celSaldoAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldoAcumulado);


                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                if (m.TipoDocumento == 19)
                {
                    LinkButton btnDetalles = new LinkButton();
                    btnDetalles.CssClass = "btn btn-info ui-tooltip";
                    btnDetalles.Attributes.Add("data-toggle", "tooltip");
                    btnDetalles.Attributes.Add("title data-original-title", "Comentarios");
                    btnDetalles.ID = "btnComent_" + m.Compra.Id;
                    btnDetalles.Text = "<span class='shortcut-icon icon-comment'></span>";
                    btnDetalles.Font.Size = 12;
                    //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                    btnDetalles.Click += new EventHandler(this.ComentariosCaja);
                    celAccion.Controls.Add(btnDetalles);
                }
                tr.Cells.Add(celAccion);

                this.phCuentaCorriente.Controls.Add(tr);

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error cargando movimeinto en PH. " + ex.Message));
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int proveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                Response.Redirect("CCCompraF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&p=" + proveedor + "&s=" + this.DropListSucursal.SelectedValue + "&t=" + this.DropListTipo.SelectedValue + "&td=" + this.DropListTipoDocumento.SelectedValue);
            }
            catch (Exception ex)
            {
 
            }
        }
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);

                //cargo la lista
                this.ListProveedor.DataSource = dtClientes;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";
                this.ListProveedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionCompras.aspx?a=6&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&ex=1&prov=" + this.idProveedor + "&s=" + this.sucursal + "&t=" + this.tipo + "&td=" + this.tipoDocumento);
            }
            catch
            {

            }
        }
        private void ComentariosCaja(object sender, EventArgs e)
        {
            try
            {                
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCompra = atributos[1];                
                string comentario = "-";

                Compra c = this.contCompra.obtenerCompraId(Convert.ToInt32(idCompra));
                if (c != null && c.Compras_Observaciones.Count > 0)
                {
                    comentario = c.Compras_Observaciones.FirstOrDefault().Observacion;
                }
                if (String.IsNullOrEmpty(comentario))
                {
                    comentario = "";
                }
                else
                {
                    comentario = Regex.Replace(comentario, @"\t|\n|\r", "");
                }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('" + comentario + "')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar comentario. " + ex.Message));                
            }
        }
        protected void lbtnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=6&fd="+this.txtFechaDesde.Text+"&fh="+this.txtFechaHasta.Text+"&prov=" + this.idProveedor + "&s=" + this.sucursal + "&t=" + this.tipo + "&td=" + this.tipoDocumento + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

    }
}