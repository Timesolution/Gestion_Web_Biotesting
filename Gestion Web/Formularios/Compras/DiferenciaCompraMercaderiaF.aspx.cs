using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class DiferenciaCompraMercaderiaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorCompraEntity contCompraEntity = new controladorCompraEntity();

        int accion = 0;
        int sucursalDestino = 0;
        string fechaD = "";
        string fechaH = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            accion = Convert.ToInt32(Request.QueryString["a"]);
            sucursalDestino = Convert.ToInt32(Request.QueryString["sd"]);
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];

            if (accion == 0)
            {
                PrimeraCarga();
            }

            if (!IsPostBack)
            {
                this.txtFechaDesde.Text = fechaD;
                this.txtFechaHasta.Text = fechaH;

                cargarSucursales();
            }

            if (accion == 1)
                CargarFacturasMercaderiasDiferencias();
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
                int tienePermiso = 0;
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "169")
                            tienePermiso = 1;

                        //if (tienePermiso == 1)
                        //{
                        //    if (s == "166")
                        //    {
                        //        this.DropListSucursalDestino.Enabled = true;
                        //    }
                        //}
                    }
                }

                if (tienePermiso == 1)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursalOrigen.DataSource = dt;
                this.DropListSucursalOrigen.DataValueField = "Id";
                this.DropListSucursalOrigen.DataTextField = "nombre";
                this.DropListSucursalOrigen.DataBind();

                this.DropListSucursalDestino.DataSource = dt;
                this.DropListSucursalDestino.DataValueField = "Id";
                this.DropListSucursalDestino.DataTextField = "nombre";
                this.DropListSucursalDestino.DataBind();

                this.DropListSucursalDestino.SelectedValue = Session["Login_SucUser"].ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        protected void PrimeraCarga()
        {
            try
            {
                fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                sucursalDestino = Convert.ToInt32(Session["Login_SucUser"]);

                FiltrarDiferenciaMercaderia();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al hacer la primera carga de compras diferencias mercaderias. " + ex.Message);
            }
        }

        protected void FiltrarDiferenciaMercaderia()
        {
            try
            {
                Response.Redirect("DiferenciaCompraMercaderiaF.aspx?a=1&sd=" + sucursalDestino + "&fd=" + fechaD + "&fh=" + fechaH);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
            }
        }

        public void CargarFacturasMercaderiasDiferencias()
        {
            try
            {
                phFacturas.Controls.Clear();

                var diferencias = contCompraEntity.ObtenerRemitosOrdenesCompraMercaderiasDiferencias();

                foreach (var diferencia in diferencias)
                {
                    cargarEnPh(diferencia);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar facturas mercaderias diferencias. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al cargar facturas mercaderias diferencias. " + ex.Message);
            }
        }

        private void cargarEnPh(RemitoCompraOrdenCompra_Diferencias f)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorArticulo contArt = new controladorArticulo();
                //controladorFacturacion contFacturacion = new controladorFacturacion();
                //var ordenCompra = f.OrdenesCompra;//contFacturacion.obtenerFacturaId((int)f.FacturasMercaderias_Detalle.Facturas_Mercaderias.Factura);
                //fila
                TableRow tr = new TableRow();
                //tr.ID = f["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                DateTime date = Convert.ToDateTime(f.OrdenesCompra.Fecha);
                celFecha.Text = date.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celSucursalDestino = new TableCell();
                celSucursalDestino.Text = contSucursal.obtenerSucursalID((int)f.OrdenesCompra.IdSucursal).nombre;
                celSucursalDestino.HorizontalAlign = HorizontalAlign.Left;
                celSucursalDestino.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalDestino);

                TableCell celNumeroRemito = new TableCell();
                string ptoVenta = f.RemitosCompra.Numero.Substring(0, 4).ToString();
                string numero = f.RemitosCompra.Numero.Substring(4, 8).ToString();
                celNumeroRemito.Text = ptoVenta + "-" + numero;
                celNumeroRemito.HorizontalAlign = HorizontalAlign.Left;
                celNumeroRemito.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroRemito);

                TableCell celOrdenCompra = new TableCell();
                celOrdenCompra.Text = f.OrdenesCompra.Numero;
                celOrdenCompra.HorizontalAlign = HorizontalAlign.Left;
                celOrdenCompra.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celOrdenCompra);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = contArt.obtenerArticuloByID((int)f.Articulo).descripcion;
                celArticulo.HorizontalAlign = HorizontalAlign.Left;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell celCantidadPedida = new TableCell();
                celCantidadPedida.Text = f.CantidadPedida.ToString();
                celCantidadPedida.HorizontalAlign = HorizontalAlign.Left;
                celCantidadPedida.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidadPedida);

                TableCell celCantidadRecibida = new TableCell();
                celCantidadRecibida.Text = f.CantidadRecibida.ToString();
                celCantidadRecibida.HorizontalAlign = HorizontalAlign.Left;
                celCantidadRecibida.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidadRecibida);

                TableCell celDiferencia = new TableCell();
                decimal diferencia = Convert.ToDecimal(f.Diferencia);

                if (diferencia > 0)
                    celDiferencia.Text = (diferencia * -1).ToString();
                else
                    celDiferencia.Text = (Math.Abs(diferencia)).ToString();

                celDiferencia.HorizontalAlign = HorizontalAlign.Left;
                celDiferencia.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDiferencia);

                //TableCell celEstado = new TableCell();
                //celEstado.Text = contFactEntity.ObtenerFacturasMercaderias_EstadoByID(Convert.ToInt32(f["Estado"].ToString())).Descripcion;
                //celEstado.HorizontalAlign = HorizontalAlign.Left;
                //celEstado.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celEstado);

                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + f.OrdenesCompra.Id + "_" + f.RemitosCompra.Id + "_" + f.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando diferencias de mercaderia en el PH. " + ex.Message));
            }
        }

        private void detalleFactura(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idOrdenCompra = atributos[1];
                string idRemitoCompra = atributos[2];
                //window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', '_blank');
                string script = "window.open('ImpresionCompras.aspx?a=3&oc=" + idOrdenCompra + "', '_blank');";
                script += " window.open('ImpresionCompras.aspx?a=8&rc=" + idRemitoCompra + "', '_blank');";

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
    }
}