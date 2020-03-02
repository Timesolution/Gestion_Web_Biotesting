using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras.Entregas
{
    public partial class EntregasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int proveedor;
        private int sucursal;
        private int tipo;
        private string fechaD;
        private string fechaH;

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];
            sucursal = Convert.ToInt32(Request.QueryString["suc"]);
            proveedor = Convert.ToInt32(Request.QueryString["p"]);
            tipo = Convert.ToInt32(Request.QueryString["t"]);

            Page.Form.DefaultButton = btnBuscarCodigoProveedor.UniqueID;

            if (!IsPostBack)
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");

                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                cargarProveedores();
                cargarSucursal();

                txtFechaDesde.Text = fechaD;
                txtFechaHasta.Text = fechaH;
                ListProveedor.SelectedValue = proveedor.ToString();
                ListTipo.SelectedValue = tipo.ToString();
                ListSucursal.SelectedValue = sucursal.ToString();
            }
            cargarEntregas();
        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../../Account/Login.aspx");
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
                Response.Redirect("../../../Account/Login.aspx");
            }
        }

        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                //string[] listPermisos = permisos.Split(';');
                //foreach (string s in listPermisos)
                //{
                //    if (!String.IsNullOrEmpty(s))
                //    {

                //    }
                //}


                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
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

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        protected void btnBuscarCodigoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodProveedor.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);
                this.phEntregas.Controls.Clear();

                this.ListProveedor.DataSource = dtClientes;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";
                this.ListProveedor.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }

        private void cargarEntregas()
        {
            try
            {
                controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();

                phEntregas.Controls.Clear();

                DateTime desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime hasta = desde.AddHours(23).AddMinutes(59);
                proveedor = Convert.ToInt32(ListProveedor.SelectedValue);
                sucursal = Convert.ToInt32(ListSucursal.SelectedValue);
                int tipo = Convert.ToInt32(ListTipo.SelectedValue);

                if (fechaD != null && fechaH != null)
                {
                    desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                    hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                }

                List<RemitosCompra> remitos = controladorCompraEntity.buscarRemito(desde, hasta, proveedor, sucursal, tipo, 0);

                CargarEntregasEnPH(remitos);
            }
            catch
            {

            }
        }

        public void CargarEntregasEnPH(List<RemitosCompra> remitos)
        {
            try
            {
                foreach (var remito in remitos)
                {
                    cargarEnPh(remito);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar los items de la factura en el PH " + ex.Message);
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntregasF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + ListProveedor.SelectedValue + "&suc=" + this.ListSucursal.SelectedValue + "&t=" + this.ListTipo.SelectedValue);
        }

        private void cargarEnPh(RemitosCompra remitoCompra)
        {
            try
            {
                controladorSucursal controladorSucursal = new controladorSucursal();
                controladorCliente controladorCliente = new controladorCliente();

                Sucursal suc = controladorSucursal.obtenerSucursalID(remitoCompra.IdSucursal.Value);

                //fila
                TableRow tr = new TableRow();
                tr.ID = remitoCompra.Id.ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = remitoCompra.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = remitoCompra.Numero;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celProveedor = new TableCell();
                celProveedor.Text = controladorCliente.obtenerProveedorID((int)remitoCompra.IdProveedor).razonSocial;
                celProveedor.HorizontalAlign = HorizontalAlign.Left;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celProveedor);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = suc.nombre;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + remitoCompra.Id + "_";
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.Click += new EventHandler(this.detalleRemito);

                celAccion.Controls.Add(btnDetalles);

                tr.Cells.Add(celAccion);

                phEntregas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun cargarEnPh EntregasMercaderiaF. Ex: " + ex.Message));
            }
        }

        private void detalleRemito(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idRemito = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('../ImpresionCompras.aspx?a=8&rc=" + idRemito + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de remito desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando detalle remito desde la interfaz. " + ex.Message);
            }
        }

    }
}