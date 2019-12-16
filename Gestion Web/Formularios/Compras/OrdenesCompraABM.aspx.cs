using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Gestion_Web.Formularios.Compras
{
    public partial class OrdenesCompraABM : System.Web.UI.Page
    {
        controladorCompraEntity controlador = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorSucursal contSuc = new controladorSucursal();
        controladorCliente contCliente = new controladorCliente();

        DataTable dtItemsTemp;
        Mensajes m = new Mensajes();
        int accion;
        long orden;

        static List<ArticulosProveedorTemp> _articulosOrdenCompra = new List<ArticulosProveedorTemp>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                accion = Convert.ToInt32(Request.QueryString["a"]);
                orden = Convert.ToInt32(Request.QueryString["oc"]);

                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

                this.VerificarLogin();

                //this.CargarItems();

                if (!IsPostBack)
                {
                    _articulosOrdenCompra = new List<ArticulosProveedorTemp>();
                    //cargo fecha de hoy
                    txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaEntrega.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    CargarProveedores();
                    CargarSucursal();
                    AsignarSucursalPorDefault();

                    if (this.accion == 2)
                    {
                        dtItemsTemp = new DataTable();
                        CrearTablaItems();
                        cargarOrdenCompra();
                    }

                    //lbtnBuscarArticulo.Visible = false;
                }

                //RecorrerArticulosBuscados();
                //this.actualizarTotales();
            }
            catch (Exception ex)
            {

            }

        }

        public void AsignarSucursalPorDefault()
        {
            this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
            if (this.ListSucursal.SelectedValue != "")
            {
                this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                this.CargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
            }
        }

        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.guardarOrden();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remito. " + ex.Message));
            }

        }

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

                if (!listPermisos.Contains("181"))
                {
                    ListSucursal.Enabled = false;
                    ListSucursal.CssClass = "form-control";
                }

                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "176")
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
        public void CargarProveedores()
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
        public void CargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
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
        public void CargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                this.ListPtoVenta.DataSource = dt;
                this.ListPtoVenta.DataValueField = "Id";
                this.ListPtoVenta.DataTextField = "NombreFantasia";

                this.ListPtoVenta.DataBind();

                if (Convert.ToInt32(ListPtoVenta.SelectedValue) > 0)
                    this.obtenerNroOrden(Convert.ToInt32(ListPtoVenta.SelectedValue), "Orden de Compra");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Pto Venta. " + ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
        private void CrearTablaItems()
        {
            try
            {
                dtItemsTemp.Columns.Add("Codigo");
                dtItemsTemp.Columns.Add("Descripcion");
                dtItemsTemp.Columns.Add("Costo");
                dtItemsTemp.Columns.Add("CostoMasIva");
                dtItemsTemp.Columns.Add("Cant");

                dtItems = dtItemsTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando tabla de item. " + ex.Message));
            }

        }
        protected DataTable dtItems
        {
            get
            {
                if (ViewState["dtItems"] != null)
                {
                    return (DataTable)ViewState["dtItems"];
                }
                else
                {
                    return dtItemsTemp;
                }
            }
            set
            {
                ViewState["dtItems"] = value;
            }
        }
        private void limpiarCamposProveedor_OC()
        {
            try
            {
                this.lblMailOC.Text = string.Empty;
                this.lblRequiereAnticipoOC.Text = string.Empty;
                this.lblMontoAutorizacionOC.Text = string.Empty;
                this.lblRequiereAutorizacionOC.Text = string.Empty;
                this.lblObservacion.Text = string.Empty;
            }
            catch (Exception Ex)
            {

            }
        }

        private void obtenerNroOrden(int idPtoVta, string tipoDoc)
        {
            try
            {
                //int ptoVenta = Convert.ToInt32(this.ListPtoVenta.SelectedValue);
                PuntoVenta pv = this.contSuc.obtenerPtoVentaId(idPtoVta);
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.contFact.obtenerFacturaNumero(idPtoVta, "Orden de Compra");
                this.txtPVenta.Text = pv.puntoVenta;
                if (accion != 2)
                {
                    this.txtNumero.Text = nro.ToString().PadLeft(8, '0');
                }
                //this.labelNroRemito.Text = "Remito N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Orden de compra. " + ex.Message));
            }
        }
        #endregion

        #region ABM
        private void guardarOrden()
        {
            try
            {
                ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();

                var puntoVenta = Convert.ToInt32(Request.Form[ListPtoVenta.UniqueID]);
                var proveedor = Convert.ToInt32(Request.Form[ListProveedor.UniqueID]);
                var sucursal = Convert.ToInt32(Request.Form[ListSucursal.UniqueID]);

                if (sucursal <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se encuentra una sucursal seleccionada"));
                    return;
                }

                OrdenesCompra oc = null;
                if (this.accion == 2)
                {
                    oc = this.controlador.obtenerOrden(this.orden);
                }
                else
                {
                    oc = new OrdenesCompra();
                }

                oc.IdProveedor = Convert.ToInt32(proveedor);

                var prov = contClienteEntity.obtenerProveedor_OC_PorProveedor((int)oc.IdProveedor);

                if (prov == null)
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", " $.msgbox(\"Debe completar los datos de Orden de Compra correspondiente al Proveedor desde la pantalla de edicion. \");", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe completar los datos de Orden de Compra correspondiente al Proveedor desde la pantalla de edicion"));
                    return;
                }

                oc.Fecha = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                oc.FechaEntrega = Convert.ToDateTime(this.txtFechaEntrega.Text, new CultureInfo("es-AR"));
                oc.IdSucursal = Convert.ToInt32(sucursal);
                oc.Observaciones = this.txtObservaciones.Text;
                oc.IdPtoVenta = Convert.ToInt32(puntoVenta);
                oc.TipoDocumento = 27;
                this.obtenerNroOrden(Convert.ToInt32(puntoVenta), "Orden de Compra");
                oc.Numero = this.txtPVenta.Text + "-" + this.txtNumero.Text;
                oc.FormaDePago = this.txtFormaDePago.Text;
                //obtengo items los borro y los leo de la pagina
                oc.OrdenesCompra_Items.Clear();
                oc.MailProveedor = hfLabelMailOC.Value;
                oc.OrdenesCompra_Items = this.ObtenerItems();
                decimal tempTotal = 0;
                foreach (var item in oc.OrdenesCompra_Items)
                {
                    tempTotal += (decimal)item.Precio * (decimal)item.Cantidad;
                    item.CantidadYaRecibida = 0;
                }

                oc.Total = tempTotal;

                oc.Estado = 1;
                oc.EstadoGeneral = 11;

                if (oc.OrdenesCompra_Items.Count > 0)
                {
                    int i = 0;
                    if (this.accion == 2)
                    {
                        i = this.controlador.modificarOrden(oc);
                    }
                    else
                    {
                        i = this.controlador.agregarOrdenCompra(oc, (int)oc.IdSucursal);
                    }
                    if (i > 0)
                    {
                        if (this.accion == 2)
                            i = Convert.ToInt32(oc.Id);

                        if (prov.RequiereAutorizacion < 1)
                        {
                            this.enviarMail(oc);
                        }
                        else
                        {
                            if (prov.MontoAutorizacion > 0 && prov.MontoAutorizacion > oc.Total)
                                this.enviarMail(oc);
                        }

                        string script = string.Empty;
                        //script = "window.open('ImpresionCompras.aspx?a=3&oc=" + i + "', '_blank');";
                        script = "window.open('ImpresionCompras.aspx?a=3&oc=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');";
                        //script += "$.msgbox(\"Orden de Compra agregada. \", {type: \"info\"}); location.href = 'OrdenesCompraABM.aspx?accion=3';";
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script,true);
                        //Response.Redirect("OrdenesCompraF.aspx?accion=1");
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                        ResetearCampos();
                        CargarProveedores();
                        CargarSucursal();
                        AsignarSucursalPorDefault();
                    }
                    else
                    {
                        if (i == -1)
                        {
                            //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar Orden de compra. Reintente\", {type: \"warning\"});", true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo guardar Orden de compra. Reintente"));
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar Orden de compra. Reintente\", {type: \"warning\"});", true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo guardar Orden de compra. Reintente"));
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"La cantidad de Items de la Orden de compra debe ser mayor a 0.\", {type: \"warning\"}); ", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La cantidad de Items de la Orden de compra debe ser mayor a 0"));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando  Orden de compra. " + ex.Message));
            }
        }
        public void ResetearCampos()
        {
            ListProveedor.SelectedValue = "-1";
            txtObservaciones.Text = "";
            txtFormaDePago.Text = "";
            _articulosOrdenCompra.Clear();
            txtCodProveedor.Text = "";
        }
        public void cargarOrdenCompra()
        {
            try
            {
                var oc = this.controlador.obtenerOrden(this.orden);

                this.txtFecha.Text = Convert.ToDateTime(oc.Fecha).ToString("dd/MM/yyyy");
                this.txtFechaEntrega.Text = Convert.ToDateTime(oc.FechaEntrega).ToString("dd/MM/yyyy");
                this.ListSucursal.SelectedValue = oc.IdSucursal.ToString();
                CargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));

                this.ListPtoVenta.SelectedValue = oc.IdPtoVenta.ToString();

                this.txtObservaciones.Text = oc.Observaciones;
                this.txtPVenta.Text = oc.Numero.Substring(0, 4);
                this.txtNumero.Text = oc.Numero.Substring(5, 8);

                //proveedor y sus items
                this.ListProveedor.SelectedValue = oc.IdProveedor.ToString();
                //cargo productos
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Alerta Proveedor: " + c.alerta.descripcion));
                }
                this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));

                //cargar items
                this.cargarItemsCompra(oc.OrdenesCompra_Items.ToList());

                this.cargarProveedor_OC();
            }
            catch
            {

            }
        }
        #endregion

        #region Funciones Proveedor
        private void cargarProveedor_OC()
        {
            try
            {
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

                this.limpiarCamposProveedor_OC();

                var poc = contClienteEnt.obtenerProveedor_OC_PorProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));

                if (poc != null)
                {
                    this.lblMailOC.Text = poc.Mail;
                    this.lblRequiereAnticipoOC.Text = "Si";
                    this.lblRequiereAutorizacionOC.Text = "Si";
                    this.lblMontoAutorizacionOC.Text = "$" + poc.MontoAutorizacion.ToString();
                    this.lblObservacion.Text = poc.cliente.observaciones;
                    this.txtFormaDePago.Text = poc.FormaDePago;
                    if (poc.RequiereAnticipo == 0)
                        this.lblRequiereAnticipoOC.Text = "No";
                    if (poc.RequiereAutorizacion == 0)
                        this.lblRequiereAutorizacionOC.Text = "No";
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de las Ordenes de Compra del proveedor. Excepción: " + Ex.Message));
            }
        }
        private void cargarArticulosProveedor(int idPRoveedor)
        {
            try
            {
                DataTable dtArticulos = this.contArticulos.obtenerArticulosByProveedorDT(idPRoveedor);
                this.dtItems.Rows.Clear();
                foreach (DataRow a in dtArticulos.Rows)
                {
                    DataTable dt = this.dtItems;

                    DataRow drFila = dt.NewRow();

                    //cargo otros proveedores, si lo tiene configuraco
                    string codProveedor = WebConfigurationManager.AppSettings.Get("CodProveedorCompras");
                    Log.EscribirSQL(1, "INFO", "obtuve el codigo de provedor webConfig = " + codProveedor);
                    if (codProveedor == "1" && !String.IsNullOrEmpty(codProveedor))
                    {
                        List<ProveedorArticulo> ProvArticulo = this.contArticulos.obtenerProveedorArticulosByArticulo(Convert.ToInt32(a["id"]));
                        string codArtProveedor = "";
                        Log.EscribirSQL(1, "INFO", "Obtuve articulos de ese proveedor");
                        foreach (var p in ProvArticulo)
                        {
                            codArtProveedor += p.codigoProveedor + " - ";
                        }

                        if (codArtProveedor.Length > 0)//saco el ultimo guion
                        {
                            codArtProveedor = codArtProveedor.Substring(0, codArtProveedor.Length - 3);
                        }

                        drFila["Codigo"] = a["codigo"].ToString() + " (" + codArtProveedor + ") ";
                    }
                    else
                    {
                        drFila["Codigo"] = a["codigo"].ToString();
                        Log.EscribirSQL(1, "INFO", "Obtuve un codigo= " + a["codigo"].ToString());
                    }

                    drFila["Descripcion"] = a["descripcion"];
                    drFila["Cant"] = 0;
                    drFila["Costo"] = Convert.ToDecimal(a["costo"].ToString());
                    drFila["CostoMasIva"] = Decimal.Round(Convert.ToDecimal(a["costoImponible"]) * ObtenerIvaArticulo(a), 2);

                    dt.Rows.Add(drFila);

                    this.dtItems = dt;
                }

                this.CargarItems();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos (cargarArticulosProveedor)" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos del proveedor. " + ex.Message));
            }
        }

        private void CargarItems()
        {
            //try
            //{
            //    Log.EscribirSQL(1, "INFO", "Inicio cargar items en pantalla");
            //    int verCargados = Convert.ToInt32(this.lblVerCargados.Text);
            //    this.phProductos.Controls.Clear();
            //    if (this.dtItems != null)
            //    {
            //        foreach (DataRow item in this.dtItems.Rows)
            //        {
            //            if (verCargados > 0)
            //            {
            //                if (item["Cant"].ToString() != "0" && !String.IsNullOrEmpty(item["Cant"].ToString()))
            //                {
            //                    this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]), Convert.ToDecimal(item["Costo"]), Convert.ToDecimal(item["CostoMasIva"]));
            //                }
            //            }
            //            else
            //            {
            //                this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]), Convert.ToDecimal(item["Costo"]), Convert.ToDecimal(item["CostoMasIva"]));
            //            }
            //        }
            //    }
            //    Log.EscribirSQL(1, "INFO", "Finalizo cargar items en pantalla");
            //    //this.UpdatePanel1.Update();
            //}
            //catch (Exception ex)
            //{
            //    Log.EscribirSQL(1, "ERROR", "Error cargando items en pantalla " + ex.Message);
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            //}
        }

        private decimal ObtenerIvaArticulo(DataRow articulo)
        {
            try
            {
                decimal ivaArticulo = Convert.ToDecimal(articulo["porcentajeIva"]);

                if (ivaArticulo > 0)
                {
                    return (ivaArticulo / 100) + 1;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private string obtenerCodigo(string codigo)
        {
            try
            {
                var paren = codigo.IndexOf('(');
                if (paren > 0)
                {
                    codigo = codigo.Substring(0, paren);
                }
                return codigo.Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Funciones Item
        private void cargarItemsCompra(List<OrdenesCompra_Items> items)
        {
            try
            {
                foreach (var item in items)
                {
                    foreach (var c in this.phProductos.Controls)
                    {
                        TableRow tr = c as TableRow;
                        string codigo = this.obtenerCodigo((tr.Cells[0]).Text);
                        var art = contArticulos.obtenerArticuloCodigoAparece(codigo);
                        OrdenesCompra_Items itemOrdenCompra = controlador.OrdenCompra_ItemGetOne((long)item.IdOrden, art.id.ToString());

                        TextBox txtCantidad = tr.Cells[4].Controls[0] as TextBox;
                        TextBox txtPrecio = tr.Cells[2].Controls[0] as TextBox;

                        if (itemOrdenCompra != null)
                        {
                            txtCantidad.Text = itemOrdenCompra.Cantidad.ToString();
                            txtPrecio.Text = itemOrdenCompra.Precio.ToString();
                        }
                        else
                        {
                            if (art.id == Convert.ToInt32(item.Codigo))
                            {
                                txtCantidad.Text = item.Cantidad.ToString();
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private List<OrdenesCompra_Items> ObtenerItems()
        {
            try
            {
                List<OrdenesCompra_Items> items = new List<OrdenesCompra_Items>();

                foreach (var articulo in _articulosOrdenCompra)
                {
                    string codigo = obtenerCodigo(articulo.codigo);
                    Articulo a = contArticulos.obtenerArticuloCodigoAparece(codigo);
                    //OrdenesCompra_Items item = controlador.OrdenCompra_ItemGetOne(orden, a.id.ToString());

                    OrdenesCompra_Items item = new OrdenesCompra_Items();

                    item.Codigo = codigo;
                    item.PrecioConIVA = 0.00m;
                    item.PrecioConIVA = decimal.Round(Convert.ToDecimal(articulo.precio) * (1 + (a.porcentajeIva / 100)), 2);
                    item.Codigo = a.id.ToString();

                    item.Descripcion = articulo.descripcion;

                    item.Precio = Convert.ToDecimal(articulo.precio);
                    item.Cantidad = Convert.ToDecimal(articulo.cantidad);
                    item.Estado = 2;

                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error obteniendo items " + ex.Message + "\", {type: \"warning\"}); ", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo items. " + ex.Message));
                return null;
            }
        }
        #endregion

        #region Envio Mail
        private void enviarMail(OrdenesCompra oc)
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();
                string destinatarios = this.lblMailOC.Text;
                if (destinatarios.Length > 0)
                {
                    String pathArchivoGenerar = Server.MapPath("../../OrdenesCompra/" + oc.Id + "/" + "/oc-" + oc.Numero + "_" + oc.Id + ".pdf");
                    string pathDirectorio = Server.MapPath("../../OrdenesCompra/" + oc.Id + "/");

                    //Si el directorio no existe, lo creo
                    if (!Directory.Exists(pathDirectorio))
                    {
                        Directory.CreateDirectory(pathDirectorio);
                    }

                    int i = this.generarOrdenCompraPDF(oc, pathArchivoGenerar);
                    if (i > 0)
                    {
                        Attachment adjunto = new Attachment(pathArchivoGenerar);

                        int ok = contFunciones.enviarMailOrdenesCompra(adjunto, oc, destinatarios);
                        if (ok > 0)
                        {
                            adjunto.Dispose();
                            File.Delete(pathArchivoGenerar);
                            Directory.Delete(pathDirectorio);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de Compra enviada correctamente!", ""));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar la Orden de Compra por mail. "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar impresion Orden de Compra a enviar. "));
                    }
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando mail. Excepción: " + Ex.Message));
            }
        }
        private int generarOrdenCompraPDF(OrdenesCompra ordenCompra, string pathGenerar)
        {
            try
            {
                controladorCliente cont = new controladorCliente();
                controladorSucursal contSuc = new controladorSucursal();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();


                //Gestion_Api.Entitys.OrdenesCompra ordenCompra = this.contCompraEntity.obtenerOrden(this.ordenCompra);
                Gestor_Solution.Modelo.Cliente p = cont.obtenerProveedorID(ordenCompra.IdProveedor.Value);

                Sucursal s = contSuc.obtenerSucursalID(ordenCompra.IdSucursal.Value);

                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById(s.empresa.id);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;

                String Fecha = " ";
                String FechaEntrega = " ";
                String Numero = " ";
                String Proveedor = " ";
                String Observacion = "-";

                foreach (DataRow row in dtEmpresa.Rows)//Datos empresa 
                {
                    razonSoc = row["Razon Social"].ToString();
                    condIVA = row["Condicion IVA"].ToString();
                    direComer = row["Direccion"].ToString();
                }

                if (ordenCompra != null && p != null)
                {
                    Fecha = ordenCompra.Fecha.Value.ToString("dd/MM/yyyy");
                    FechaEntrega = ordenCompra.FechaEntrega.Value.ToString("dd/MM/yyyy");
                    Numero = "Nº " + ordenCompra.Numero;
                    Proveedor = p.razonSocial;
                    Observacion = ordenCompra.Observaciones;
                }

                string logo = Server.MapPath("../../Facturas/" + s.empresa.id + "/Logo.jpg");

                List<Gestion_Api.Entitys.OrdenesCompra_Items> itemsOrdenes = ordenCompra.OrdenesCompra_Items.ToList();//obtengo los items de la OC
                DataTable dtItems = ListToDataTable(itemsOrdenes);//Paso la list a datatable para pasarlo al report.
                dtItems.Columns.Add("CodProv");

                foreach (DataRow row in dtItems.Rows)
                {
                    ProveedorArticulo codProv = this.contArticulos.obtenerProveedorArticuloByArticulo(Convert.ToInt32(row["Codigo"]));

                    Articulo art = this.contArticulos.obtenerArticuloByID(Convert.ToInt32(Convert.ToInt32(row["Codigo"])));

                    if (art != null)
                    {
                        row["Codigo"] = art.codigo;
                    }
                    if (codProv != null)
                    {
                        row["CodProv"] = codProv.codigoProveedor;
                    }
                }


                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("OrdenesCompraR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("ItemsOrden", dtItems);
                ReportParameter param1 = new ReportParameter("ParamFecha", Fecha);
                ReportParameter param2 = new ReportParameter("ParamFechaEntrega", FechaEntrega);
                ReportParameter param3 = new ReportParameter("ParamNumero", Numero);
                ReportParameter param4 = new ReportParameter("ParamProveedor", Proveedor);
                ReportParameter param5 = new ReportParameter("ParamObservacion", Observacion);

                ReportParameter param12 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);

                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);

                this.ReportViewer1.LocalReport.SetParameters(param12);//datos empresa
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);

                this.ReportViewer1.LocalReport.SetParameters(param32);//logo

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                return 1;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al intentar guardar la Orden de Compra. Excepción: " + Ex.Message));
                return -1;
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
        #endregion

        [WebMethod]
        public static string CargarAlertaProveedor(int idProveedor)
        {
            if (idProveedor <= 0)
                return "";

            controladorCliente contCliente = new controladorCliente();

            Cliente c = contCliente.obtenerProveedorID(idProveedor);
            c.alerta = contCliente.obtenerAlertaClienteByID(c.id);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = "";

            if (!String.IsNullOrEmpty(c.alerta.descripcion))
                resultadoJSON = serializer.Serialize(c.alerta.descripcion);

            return resultadoJSON;
        }

        [WebMethod]
        public static string CargarProveedor_OC(int idProveedor)
        {
            if (idProveedor <= 0)
                return "";

            ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

            var poc = contClienteEnt.obtenerProveedor_OC_PorProveedor(idProveedor);

            DatosProveedorTemp datosProveedorTemp = new DatosProveedorTemp();

            if (poc != null)
            {
                datosProveedorTemp.mail = poc.Mail;
                datosProveedorTemp.montoAutorizacion = poc.MontoAutorizacion.ToString();
                datosProveedorTemp.observaciones = poc.cliente.observaciones;
                datosProveedorTemp.formaDePago = poc.FormaDePago;
                datosProveedorTemp.requiereAnticipo = poc.RequiereAnticipo.ToString();
                datosProveedorTemp.requiereAutorizacion = poc.RequiereAutorizacion.ToString();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(datosProveedorTemp);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerArticulosProveedor(int idProveedor, int idSucursal)
        {
            if (idProveedor <= 0 || idSucursal <= 0)
                return "";

            controladorArticulo controladorArticulo = new controladorArticulo();
            ControladorArticulosEntity controladorArticulosEntity = new ControladorArticulosEntity();

            var articulos = controladorArticulo.ObtenerArticulosProveedorOrdenCompra(idProveedor, idSucursal);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = JsonConvert.SerializeObject(articulos);
            return resultadoJSON;
        }

        [WebMethod]
        public static string CargarPuntoVenta(int sucursal)
        {
            if (sucursal <= 0)
                return "";

            controladorSucursal controladorSucursal = new controladorSucursal();

            DataTable dt = null;

            if (sucursal > 0)
                dt = controladorSucursal.obtenerPuntoVentaDT(sucursal);
            else
                dt = controladorSucursal.obtenerPuntoVenta();

            List<PuntoVentaTemporal> puntosVenta = new List<PuntoVentaTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                PuntoVentaTemporal puntoVentaTemporal = new PuntoVentaTemporal();
                puntoVentaTemporal.id = row["Id"].ToString();
                puntoVentaTemporal.nombreFantasia = row["NombreFantasia"].ToString();
                puntosVenta.Add(puntoVentaTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(puntosVenta);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerSucursales()
        {
            controladorSucursal contSucu = new controladorSucursal();
            DataTable sucursales = contSucu.obtenerSucursales();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = JsonConvert.SerializeObject(sucursales);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerProveedores()
        {
            controladorCliente contCliente = new controladorCliente();
            DataTable proveedores = contCliente.obtenerProveedoresReducDT();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = JsonConvert.SerializeObject(proveedores);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerNumeroOrden(int puntoVenta)
        {
            if (puntoVenta <= 0)
                return "";

            controladorSucursal controladorSucursal = new controladorSucursal();
            controladorFacturacion controladorFacturacion = new controladorFacturacion();
            PuntoVenta puntoDeVenta = controladorSucursal.obtenerPtoVentaId(puntoVenta);
            int numero = controladorFacturacion.obtenerFacturaNumero(puntoVenta, "Orden de Compra");

            NumeroOrdenTemporal numeroOrdenTemporal = new NumeroOrdenTemporal();
            numeroOrdenTemporal.puntoVenta = puntoDeVenta.puntoVenta;
            numeroOrdenTemporal.numero = numero.ToString("D8");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(numeroOrdenTemporal);
            return resultadoJSON;
        }

        [WebMethod]
        public static string BuscarProveedor(string codigoProveedor)
        {
            if (string.IsNullOrEmpty(codigoProveedor))
                return "";

            controladorCliente controladorCliente = new controladorCliente();
            String buscar = codigoProveedor.Replace(' ', '%');
            DataTable dtClientes = controladorCliente.obtenerProveedorNombreDT(buscar);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = JsonConvert.SerializeObject(dtClientes);
            return resultadoJSON;
        }

        [WebMethod]
        public static void ObtenerArticulosParaGenerarOrdenCompra(string[] articulos)
        {
            foreach (var articulo in articulos)
            {
                string[] articuloTemp= { };
                if (articulo.Contains("lt"))
                {
                    articuloTemp = articulo.Replace("&lt;", "<").Split(';');
                }
                if (articulo.Contains("gt"))
                {
                    articuloTemp = articulo.Replace("&gt;", ">").Split(';');
                }
                if (articulo.Contains("amp"))
                {
                    articuloTemp = articulo.Replace("&amp;", "&").Split(';');
                }

                ArticulosProveedorTemp articuloProveedorTemp = new ArticulosProveedorTemp();
                articuloProveedorTemp.codigo = articuloTemp[0];
                articuloProveedorTemp.descripcion = articuloTemp[1];
                articuloProveedorTemp.precio = articuloTemp[2];
                articuloProveedorTemp.cantidad = articuloTemp[3];

                _articulosOrdenCompra.Add(articuloProveedorTemp);
            }
        }
    }

    public class DatosProveedorTemp
    {
        public string mail;
        public string montoAutorizacion;
        public string observaciones;
        public string formaDePago;
        public string requiereAnticipo;
        public string requiereAutorizacion;
    }

    public class ArticulosProveedorTemp
    {
        public string codigo;
        public string descripcion;
        public string precio;
        public string stockSucursal;
        public string stockTotal;
        public string stockMinimo;
        public string alerta;
        public string cantidad;
    }
    class PuntoVentaTemporal
    {
        public string id;
        public string nombreFantasia;
    }
    class NumeroOrdenTemporal
    {
        public string puntoVenta;
        public string numero;
    }
}