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

        static List<Articulo> _articulosProveedor = new List<Articulo>();
        static List<Articulo> _articulosProveedorBuscados = new List<Articulo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.orden = Convert.ToInt32(Request.QueryString["oc"]);

                #region btnAguarde
                btnVerStockMinimo.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnVerStockMinimo, null) + ";");
                btnVerStockMinimoSucursal.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnVerStockMinimoSucursal, null) + ";");
                btnVerOC.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnVerOC, null) + ";");
                btnVerTodos.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnVerTodos, null) + ";");
                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                #endregion

                this.VerificarLogin();
                this.CargarItems();

                if (!IsPostBack)
                {
                    //cargo fecha de hoy
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaEntrega.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.cargarProveedores();
                    this.cargarSucursal();
                    _articulosProveedor.Clear();
                    _articulosProveedorBuscados.Clear();
                    ObtenerArticulosProveedor();
                    //cargo sucursal
                    this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                    if (this.ListSucursal.SelectedValue != "")
                    {
                        this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                        this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
                    }

                    this.dtItemsTemp = new DataTable();
                    this.CrearTablaItems();

                    if (this.accion == 2)
                    {
                        //cargar orden
                        this.cargarOrdenCompra();
                    }

                    lbtnBuscarArticulo.Visible = false;
                }

                RecorrerArticulosBuscados();
                this.actualizarTotales();                
            }
            catch (Exception ex)
            {

            }

        }

        #region Eventos Controles
        protected void lbtnAgregarArticuloASP_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = this.dtItems;
                //verifico que no este agregado a la grilla
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Codigo"].ToString() == this.txtCodigo.Text)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"El articulo con codigo  ya se encuentra en la grilla\", {type: \"error\"});", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El articulo con codigo " + this.txtCodigo.Text + " ya se encuentra en la grilla"));
                        return;
                    }
                }

                DataRow drFila = dt.NewRow();

                drFila["Codigo"] = this.txtCodigo.Text;
                drFila["Descripcion"] = this.txtDescripcion.Text;
                drFila["Costo"] = this.txtPrecio.Text;
                drFila["Cant"] = this.txtCantidad.Text;
                drFila["CostoMasIva"] = "0.00";

                dt.Rows.Add(drFila);

                this.dtItems = dt;

                this.agregarItemATabla(drFila["Codigo"].ToString(), drFila["Descripcion"].ToString(), Convert.ToDecimal(drFila["Cant"]), Convert.ToDecimal(drFila["Costo"]), Convert.ToDecimal(drFila["CostoMasIva"]));
                //this.CargarItems();
                //limpio los campos
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
                this.txtDescripcion.Text = "";
                this.txtPrecio.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.accion == 1)
                this.guardarOrden();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remito. " + ex.Message));
            }

        }
        
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            this.filtrarItems();
        }
        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
            }
            catch
            {

            }
        }
        protected void ListPtoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.obtenerNroOrden(Convert.ToInt32(ListPtoVenta.SelectedValue), "Orden de Compra");
            }
            catch
            {

            }

        }
        protected void btnVerOC_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblVerCargados.Text = "1";
                this.CargarItems();
                //this.btnVerOC.Visible = false;
                //this.btnVerTodos.Visible = true;
            }
            catch
            {

            }
        }
        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblVerCargados.Text = "0";
                this.CargarItems();
                //this.btnVerOC.Visible = true;
                //this.btnVerTodos.Visible = false;
            }
            catch
            {

            }

        }
        protected void btnVerStockMinimo_Click(object sender, EventArgs e)
        {
            try
            {
                this.filtrarItemsByStock(1);
            }
            catch (Exception Ex)
            {

            }
        }
        protected void btnVerStockMinimoSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                this.filtrarItemsByStock(2);
            }
            catch (Exception Ex)
            {

            }
        }
        protected void btnBuscarCodigoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodProveedor.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);
                this.phProductos.Controls.Clear();
                this.limpiarCamposProveedor_OC();

                //cargo la lista
                this.ListProveedor.DataSource = dtClientes;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";
                this.ListProveedor.DataBind();

                //Cargo los articulos del primer proveedor que me quede seleccionado en el DropDownList, ya que no se ejecuta el evento SelectedIndexChanged del mismo.
                if (dtClientes.Rows.Count > 0 && buscar.Length > 0)
                {

                    this.cargarAlertaProveedor();
                    //this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));
                    this.cargarProveedor_OC();
                    _articulosProveedorBuscados.Clear();
                    ObtenerArticulosProveedor();
                    lbtnBuscarArticulo.Visible = true;
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
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
        public void cargarSucursal()
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
        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListPtoVenta.DataSource = dt;
                this.ListPtoVenta.DataValueField = "Id";
                this.ListPtoVenta.DataTextField = "NombreFantasia";

                this.ListPtoVenta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPtoVenta.SelectedIndex = 1;
                    this.obtenerNroOrden(Convert.ToInt32(ListPtoVenta.SelectedValue), "Orden de Compra");
                }


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
        private void limpiarCampos()
        {
            try
            {
                this.ListProveedor.SelectedIndex = 0;
                this.txtPVenta.Text = "";
                this.txtNumero.Text = "";
                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error limpiando campos. " + ex.Message));
            }
        }
        private void actualizarTotales()
        {
            try
            {
                List<OrdenesCompra_Items> items = this.obtenerItems();
                decimal total = 0;
                if (items != null)
                {
                    foreach (OrdenesCompra_Items item in items)
                    {
                        total += decimal.Round(item.Precio.Value * item.Cantidad.Value, 2, MidpointRounding.AwayFromZero);
                    }
                }
                //this.lblCartelTotal.Text = total.ToString("C");
            }
            catch
            {

            }
        }
        private void obtenerNroOrden(int idPtoVta, string tipoDoc)
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPtoVenta.SelectedValue);
                PuntoVenta pv = this.contSuc.obtenerPtoVentaId(Convert.ToInt32(ListPtoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.contFact.obtenerFacturaNumero(ptoVenta, "Orden de Compra");
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

                OrdenesCompra oc = null;
                if (this.accion == 2)
                {
                    oc = this.controlador.obtenerOrden(this.orden);
                }
                else
                {
                    oc = new OrdenesCompra();
                }

                oc.IdProveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);

                var prov = contClienteEntity.obtenerProveedor_OC_PorProveedor((int)oc.IdProveedor);

                if (prov == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", " $.msgbox(\"Debe completar los datos de Orden de Compra correspondiente al Proveedor desde la pantalla de edicion. \");", true);
                    return;
                }

                oc.Fecha = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                oc.FechaEntrega = Convert.ToDateTime(this.txtFechaEntrega.Text, new CultureInfo("es-AR"));
                oc.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                oc.Observaciones = this.txtObservaciones.Text;
                oc.IdPtoVenta = Convert.ToInt32(this.ListPtoVenta.SelectedValue);
                oc.TipoDocumento = 27;
                this.obtenerNroOrden(Convert.ToInt32(this.ListPtoVenta.SelectedValue), "Orden de Compra");
                oc.Numero = this.txtPVenta.Text + "-" + this.txtNumero.Text;
                oc.FormaDePago = this.txtFormaDePago.Text;
                //obtengo items los borro y los leo de la pagina
                oc.OrdenesCompra_Items.Clear();
                oc.MailProveedor = this.lblMailOC.Text;
                oc.OrdenesCompra_Items = this.obtenerItems();
                decimal tempTotal = 0;
                foreach (var item in oc.OrdenesCompra_Items)
                {
                    tempTotal += (decimal)item.Precio * (decimal)item.Cantidad;
                    item.CantidadYaRecibida = 0;
                }

                oc.Total = tempTotal;

                //TODO esto queda o vuela?
                //Agrego Estado
                //if (prov.RequiereAutorizacion < 1)
                //    oc.Estado = 1;
                //else
                //    if(prov.MontoAutorizacion > 0 && oc.Total < prov.MontoAutorizacion)
                //    oc.Estado = 1;
                //    else
                //    oc.Estado = 8;

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

                        if(prov.RequiereAutorizacion < 1)
                        {
                            this.enviarMail(oc);
                        }
                        else
                        {
                            if(prov.MontoAutorizacion > 0 && prov.MontoAutorizacion > oc.Total)
                                this.enviarMail(oc);
                        }

                        string script = string.Empty;
                        script = "window.open('ImpresionCompras.aspx?a=3&oc=" + i + "', '_blank');";
                        script += " $.msgbox(\"Orden de Compra agregada. \", {type: \"info\"}); location.href = 'OrdenesCompraABM.aspx?a=1';";
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                    }
                    else
                    {
                        if (i == -1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar Orden de compra. Reintente\", {type: \"warning\"});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar Orden de compra. Reintente\", {type: \"warning\"});", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"La cantidad de Items de la Orden de compra debe ser mayor a 0.\", {type: \"warning\"}); ", true);
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando  Orden de compra. " + ex.Message));
            }
        }
        public void cargarOrdenCompra()
        {
            try
            {
                var oc = this.controlador.obtenerOrden(this.orden);

                this.txtFecha.Text = Convert.ToDateTime(oc.Fecha).ToString("dd/MM/yyyy");
                this.txtFechaEntrega.Text = Convert.ToDateTime(oc.FechaEntrega).ToString("dd/MM/yyyy");
                this.ListSucursal.SelectedValue = oc.IdSucursal.ToString();
                cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));

                this.ListPtoVenta.SelectedValue = oc.IdPtoVenta.ToString();

                this.txtObservaciones.Text = oc.Observaciones;
                this.txtPVenta.Text = oc.Numero.Substring(0, 4);
                this.txtNumero.Text = oc.Numero.Substring(5, 8);

                //proveedor y sus items
                this.ListProveedor.SelectedValue = oc.IdProveedor.ToString();
                //cargo productos
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                //if (!String.IsNullOrEmpty(c.alerta.descripcion))
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                //}
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
                    Log.EscribirSQL(1, "INFO", "obtuve el codigo de provedor webConfig = "+codProveedor);
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
                        Log.EscribirSQL(1, "INFO", "Obtuve un codigo= "+ a["codigo"].ToString());
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
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos (cargarArticulosProveedor)"+ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos del proveedor. " + ex.Message));
            }
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

        private void cargarAlertaProveedor()
        {
            try
            {
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                }
            }
            catch (Exception Ex)
            {

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

                        if(itemOrdenCompra != null)
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
        private void agregarItemATabla(string codigo, string Descripcion, decimal cant, decimal precio, decimal costoMasIva)
        {
            try
            {

                Articulo articulo = new Articulo();
                //articulo = contArticulosEntity.(codigo);


                Log.EscribirSQL(1, "INFO", "TODO agregarItemATabla()");
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCodigo);

                TableCell celCant = new TableCell();
                celCant.Text = Descripcion;
                celCant.VerticalAlign = VerticalAlign.Middle;
                celCant.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCant);

                TableCell celPrecio = new TableCell();
                celPrecio.HorizontalAlign = HorizontalAlign.Right;

                TextBox txtCantidadPrecio = new TextBox();
                txtCantidadPrecio.Text = precio.ToString();
                txtCantidadPrecio.TextMode = TextBoxMode.Number;
                txtCantidadPrecio.Attributes.Add("Style", "text-align: right;");
                celPrecio.Controls.Add(txtCantidadPrecio);
                tr.Cells.Add(celPrecio);

                TableCell celPrecioMasIva = new TableCell();
                celPrecioMasIva.Text = "$ " + costoMasIva;
                celPrecioMasIva.VerticalAlign = VerticalAlign.Middle;
                celPrecioMasIva.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecioMasIva);

                TableCell celCantidad = new TableCell();
                celCantidad.HorizontalAlign = HorizontalAlign.Right;

                TextBox txtCantidad = new TextBox();
                txtCantidad.ID = codigo;
                if (cant > 0)
                {
                    txtCantidad.Text = cant.ToString();
                }
                else
                {
                    txtCantidad.Text = "";
                }
                txtCantidad.TextMode = TextBoxMode.Number;
                txtCantidad.Attributes.Add("Style", "text-align: right;");
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtCantidad.AutoPostBack = true;
                txtCantidad.TextChanged += new EventHandler(this.cargarCantidadItem);
                celCantidad.Controls.Add(txtCantidad);
                tr.Cells.Add(celCantidad);

                TableCell celStockSucursal = new TableCell();
                celStockSucursal.Text = "0.00";
                celStockSucursal.VerticalAlign = VerticalAlign.Middle;
                celStockSucursal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStockSucursal);

                TableCell celStockMinimoSucursal = new TableCell();
                celStockMinimoSucursal.Text = "0.00";
                celStockMinimoSucursal.VerticalAlign = VerticalAlign.Middle;
                celStockMinimoSucursal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStockMinimoSucursal);

                TableCell celStockTotal = new TableCell();
                celStockTotal.Text = "0.00";
                celStockTotal.VerticalAlign = VerticalAlign.Middle;
                celStockTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStockTotal);

                TableCell celStockMinimo = new TableCell();
                celStockMinimo.Text = "0.00";
                celStockMinimo.VerticalAlign = VerticalAlign.Middle;
                celStockMinimo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStockMinimo);

                TableCell celAccion = new TableCell();

                LinkButton btnDetails = new LinkButton();
                //btnDetails.ID = art.id.ToString();
                btnDetails.CssClass = "btn btn-info ui-tooltip";
                btnDetails.Attributes.Add("data-toggle", "tooltip");
                btnDetails.Attributes.Add("title data-original-title", "Ver y/o Editar");
                btnDetails.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnDetails.Attributes.Add("onclick", "window.open('../Articulos/ArticulosABM.aspx?accion=2&id=" + idArticulo+"')");

                //btnDetails.Attributes.Add("target", "_blank");
                //btnDetails.PostBackUrl = "../Articulos/ArticulosABM.aspx?accion=2&id=" + idArticulo;
                //fa-exclamation-triangle

                celAccion.Controls.Add(btnDetails);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                //Stock Articulo: stock minimo, stock total (todas las sucursales), stock por sucursal seleccionada en el DropDownList
                //cortar los parentesis cuando trae el codigo
                int posParentesis = codigo.IndexOf('(');
                string codigoSinParentesis = codigo;
                if (posParentesis > 0)
                {
                    codigoSinParentesis = codigo.Substring(0, posParentesis).Trim();
                }
                
                Articulo A = this.contArticulos.obtenerArticuloCodigo(codigoSinParentesis);                

                if (A != null && A.descripcion == Descripcion)
                {
                    var stockMinimoSucursalByArticulo = contArticulosEntity.getAllStockMinimoSucursalesByArticulo(A.id);
                    var list = this.contArticulos.obtenerStockArticuloReduc(A.id);

                    celStockMinimo.Text = A.stockMinimo.ToString();
                    celStockTotal.Text = list.Sum(x => x.cantidad).ToString();

                    var stockMinimoSucursal = stockMinimoSucursalByArticulo.Where(x => x.sucursal == Convert.ToInt32(ListSucursal.SelectedValue)).Select(x => x.stockMinimo).FirstOrDefault().ToString();

                    if (!String.IsNullOrEmpty(stockMinimoSucursal))
                        celStockMinimoSucursal.Text = stockMinimoSucursal;
                    else
                    {
                        stockMinimoSucursal = "0";
                        celStockMinimoSucursal.Text = "-";
                    }                        

                    celStockSucursal.Text = list.Where(x => x.sucursal.id == Convert.ToInt32(ListSucursal.SelectedValue)).Sum(x => x.cantidad).ToString();
                    //Si el stock total del articulo es menor al stock minimo de ese articulo, muestro un icono                    

                    if (A.stockMinimo > Convert.ToDecimal(celStockTotal.Text) || Convert.ToDecimal(stockMinimoSucursal) > Convert.ToDecimal(celStockTotal.Text))
                    {
                        Literal ltAviso = new Literal();
                        ltAviso.Text = "<span>   <span><i class=\"fa fa-exclamation-triangle text-danger\"></i>";
                        celAccion.Controls.Add(ltAviso);
                    }
                }

                celAccion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celAccion);

                this.phProductos.Controls.Add(tr);

                Log.EscribirSQL(1, "INFO", "cargue el item al ph");
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando al ph " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando item a tabla. " + ex.Message));
            }
        }
        private void CargarItems()
        {
            try
            {
                Log.EscribirSQL(1, "INFO", "Inicio cargar items en pantalla");
                int verCargados = Convert.ToInt32(this.lblVerCargados.Text);
                this.phProductos.Controls.Clear();
                if (this.dtItems != null)
                {
                    foreach (DataRow item in this.dtItems.Rows)
                    {
                        if (verCargados > 0)
                        {
                            if (item["Cant"].ToString() != "0" && !String.IsNullOrEmpty(item["Cant"].ToString()))
                            {
                                this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]), Convert.ToDecimal(item["Costo"]), Convert.ToDecimal(item["CostoMasIva"]));
                            }
                        }
                        else
                        {
                            this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]), Convert.ToDecimal(item["Costo"]), Convert.ToDecimal(item["CostoMasIva"]));
                        }
                    }
                }
                Log.EscribirSQL(1, "INFO", "Finalizo cargar items en pantalla");
                //this.UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando items en pantalla " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }
        private List<OrdenesCompra_Items> obtenerItems()
        {
            try
            {
                List<OrdenesCompra_Items> items = new List<OrdenesCompra_Items>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[4].Controls[0] as TextBox;
                    if (txt.Text != "0" && !String.IsNullOrEmpty(txt.Text))
                    {
                        string codigo = this.obtenerCodigo((tr.Cells[0]).Text);
                        Articulo a = contArticulos.obtenerArticuloCodigoAparece(codigo);
                        OrdenesCompra_Items item = controlador.OrdenCompra_ItemGetOne(orden, a.id.ToString());

                        if (item == null)
                        {
                            item = new OrdenesCompra_Items();

                            if (a == null)
                            {
                                item.Codigo = codigo;
                                item.PrecioConIVA = 0.00m;
                            }
                            else
                            {
                                item.PrecioConIVA = decimal.Round(a.costo * (1 + (a.porcentajeIva / 100)), 2);
                                item.Codigo = a.id.ToString();
                            }

                            item.Descripcion = tr.Cells[1].Text;

                            var nuevoPrecio = tr.Cells[2].Controls[0] as TextBox;

                            item.Precio = Convert.ToDecimal(nuevoPrecio.Text);
                            item.Cantidad = Convert.ToDecimal(txt.Text);
                            item.Estado = 2;
                        }
                        else
                        {
                            var nuevoPrecio = tr.Cells[2].Controls[0] as TextBox;

                            if (item.Cantidad != Convert.ToDecimal(txt.Text))
                                item.Cantidad = Convert.ToDecimal(txt.Text);

                            if(item.Precio != Convert.ToDecimal(nuevoPrecio.Text))
                                item.Precio = Convert.ToDecimal(nuevoPrecio.Text);
                            
                        }
                        items.Add(item);
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error obteniendo items " + ex.Message + "\", {type: \"warning\"}); ", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo items. " + ex.Message));
                return null;
            }
        }
        private List<RemitosCompras_Items> filtrarItems()
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[3].Controls[0] as TextBox;
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        tr.Visible = true;
                    }
                    else
                    {
                        tr.Visible = false;
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo items. " + ex.Message));
                return null;
            }
        }
        private void filtrarItemsByStock(int tipo)
        {
            try
            {
                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TableCell tcStockSucursal = tr.Cells[4] as TableCell;
                    TableCell tcStockTotal = tr.Cells[5] as TableCell;
                    TableCell tcStockMinimo = tr.Cells[6] as TableCell;

                    if (tipo == 1)
                    {
                        if (Convert.ToDecimal(tcStockMinimo.Text) > Convert.ToDecimal(tcStockTotal.Text))
                        {
                            tr.Visible = true;
                        }
                        else
                        {
                            tr.Visible = false;
                        }
                    }
                    if (tipo == 2)
                    {
                        if (Convert.ToDecimal(tcStockMinimo.Text) > Convert.ToDecimal(tcStockSucursal.Text))
                        {
                            tr.Visible = true;
                        }
                        else
                        {
                            tr.Visible = false;
                        }
                    }

                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error filtrando items. Excepción: " + Ex.Message));
            }
        }
        private void cargarCantidadItem(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as TextBox).ID;
                foreach (DataRow row in this.dtItems.Rows)
                {
                    if (row["codigo"] == id)
                    {
                        row["Cant"] = (sender as TextBox).Text;
                    }
                }
            }
            catch
            {

            }
        }
        private void actualizarTotalItem(object sender, EventArgs e)
        {
            try
            {
                this.actualizarTotales();
            }
            catch
            {

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

        protected void lbtnCargarArticulos_Click(object sender, EventArgs e) 
        {
            try
            {
                this.cargarAlertaProveedor();
                this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));
                this.cargarProveedor_OC();
                _articulosProveedorBuscados.Clear();
                phBuscarArticulo.Controls.Clear();
                txtDescripcionArticulo.Text = "";
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando cargando articulos del proveedor " + Ex.Message);
            }
        }

        protected void ListProveedor_SelectedIndexChanged(object sender, EventArgs e) 
        {
            try
            {
                cargarAlertaProveedor();
                phProductos.Controls.Clear();
                cargarProveedor_OC();
                ObtenerArticulosProveedor();
                dtItems.Rows.Clear();
                _articulosProveedorBuscados.Clear();
                if (ListProveedor.SelectedIndex > 0)
                    lbtnBuscarArticulo.Visible = true;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando cambiando de proveedor " + Ex.Message);
            }
        }

        private void ObtenerArticulosProveedor()
        {
            _articulosProveedor = contArticulos.obtenerArticulosByProveedor(Convert.ToInt32(ListProveedor.SelectedValue));
        }

        private void CargarEnPHBusquedaDeArticulos(Articulo articulo)
        {
            TableRow tr = new TableRow();

            TableCell celCodigo = new TableCell();
            celCodigo.Text = articulo.codigo;
            celCodigo.Width = Unit.Percentage(15);
            celCodigo.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(celCodigo);

            TableCell celDescripcion = new TableCell();
            celDescripcion.Text = articulo.descripcion;
            celDescripcion.Width = Unit.Percentage(15);
            celDescripcion.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(celDescripcion);

            TableCell celCosto = new TableCell();
            celCosto.Text = articulo.costo.ToString();
            celCosto.Width = Unit.Percentage(15);
            celCosto.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(celCosto);

            TableCell celPrecioVenta = new TableCell();
            celPrecioVenta.Text = articulo.precioVenta.ToString();
            celPrecioVenta.Width = Unit.Percentage(15);
            celPrecioVenta.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(celPrecioVenta);

            TableCell celAction = new TableCell();
            LinkButton btnEliminar = new LinkButton();
            btnEliminar.ID = "btnEliminar_" + articulo.id;
            btnEliminar.CssClass = "btn btn-info";
            btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
            btnEliminar.Click += new EventHandler(EliminarArticuloBuscado);
            celAction.Controls.Add(btnEliminar);
            tr.Cells.Add(celAction);

            phBuscarArticulo.Controls.Add(tr);
            UpdatePanel7.Update();
        }

        private void EliminarArticuloBuscado(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID;
                int idArticulo = Convert.ToInt32(id.Split('_')[1]);

                var articulo = _articulosProveedorBuscados.Where(x => x.id == idArticulo).FirstOrDefault();
                _articulosProveedorBuscados.Remove(articulo);

                RecorrerArticulosBuscados();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error eliminando articulo de articulos buscados " + ex.Message);
            }

        }

        protected void btnBuscarArticuloDescripcion_Click(object sender, EventArgs e)
        {
            ObtenerDatosArticuloYDibujarlosEnPantalla(txtDescripcionArticulo.Text);
        }

        public void ObtenerDatosArticuloYDibujarlosEnPantalla(string txtDescripcion)
        {
            if (string.IsNullOrEmpty(txtDescripcion))
                return;

            AgregarArticulosBuscados(txtDescripcion);

            RecorrerArticulosBuscados();
        }

        public void RecorrerArticulosBuscados()
        {
            if (_articulosProveedorBuscados.Count > 0)
            {
                phBuscarArticulo.Controls.Clear();

                foreach (var articulo in _articulosProveedorBuscados)
                {
                    CargarEnPHBusquedaDeArticulos(articulo);
                }
            }
        }

        public void AgregarArticulosBuscados(string txtDescripcion)
        {
            if (!_articulosProveedorBuscados.Exists(j => j.codigo.ToLower().Trim() == txtDescripcion.ToLower().Trim() || j.descripcion.ToLower().Trim() == txtDescripcion.ToLower().Trim()))
            {
                _articulosProveedorBuscados.Add(_articulosProveedor.Where
                (
                    x => x.descripcion.ToLower().Trim() == txtDescripcion.ToLower().Trim()
                    ||
                    x.codigo.ToLower().Trim() == txtDescripcion.ToLower().Trim()).FirstOrDefault()
                );
            }
        }

        public void AgregarArticulosATablaDeItems()
        {
            foreach (var articulo in _articulosProveedorBuscados)
            {
                DataTable dt = this.dtItems;

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[1].ToString() == articulo.descripcion)
                        return;
                }

                DataRow drFila = dt.NewRow();

                List<ProveedorArticulo> ProvArticulo = this.contArticulos.obtenerProveedorArticulosByArticulo(Convert.ToInt32(articulo.id));
                string codArtProveedor = "";
                
                foreach (var p in ProvArticulo)
                {
                    codArtProveedor += p.codigoProveedor + " - ";
                }

                if (codArtProveedor.Length > 0)//saco el ultimo guion
                {
                    codArtProveedor = codArtProveedor.Substring(0, codArtProveedor.Length - 3);
                }

                drFila["Codigo"] = articulo.codigo.ToString() + " (" + codArtProveedor + ") ";

                drFila["Descripcion"] = articulo.descripcion;
                drFila["Cant"] = 0;
                drFila["Costo"] = Convert.ToDecimal(articulo.costo);

                decimal porcentajeIvaArticulo = Convert.ToDecimal(articulo.porcentajeIva);
                decimal ivaArticulo = Convert.ToDecimal(articulo.porcentajeIva);

                if (porcentajeIvaArticulo > 0)                
                    ivaArticulo = (porcentajeIvaArticulo / 100) + 1;                
                else
                    ivaArticulo = 0;

                drFila["CostoMasIva"] = Decimal.Round(Convert.ToDecimal(articulo.costoImponible) * ivaArticulo, 2);

                dt.Rows.Add(drFila);

                this.dtItems = dt;
            }

            this.CargarItems();
        }

        protected void lbtnAgregarArticulosBuscadosATablaItems_Click(object sender, EventArgs e)
        {
            try
            {
                phBuscarArticulo.Controls.Clear();
                txtDescripcionArticulo.Text = "";
                AgregarArticulosATablaDeItems();
                _articulosProveedorBuscados.Clear();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error agregando articulos buscados a tabla items " + ex.Message);
            }
        }
        //[WebMethod]
        //public static void AgregarArticulosBuscados(string txtDescripcion)
        //{
        //    if (!articulosProveedorBuscados.Exists(j => j.codigo.ToLower().Trim() == txtDescripcion.ToLower().Trim() || j.descripcion.ToLower().Trim() == txtDescripcion.ToLower().Trim()))
        //    {
        //        articulosProveedorBuscados.Add(articulosProveedor.Where
        //        (
        //            x => x.descripcion.ToLower().Trim() == txtDescripcion.ToLower().Trim()
        //            ||
        //            x.codigo.ToLower().Trim() == txtDescripcion.ToLower().Trim()).FirstOrDefault()
        //        );
        //    }
        //}

        //[WebMethod]
        //public static string ObtenerDatosArticuloYDibujarlosEnPantalla(string txtDescripcion)
        //{
        //    if (string.IsNullOrEmpty(txtDescripcion))
        //        return "";

        //    AgregarArticulosBuscados(txtDescripcion);

        //    JavaScriptSerializer TheSerializer = new JavaScriptSerializer();

        //    if(articulosProveedorBuscados.Count > 0)
        //    {
        //        List<ArticuloBuscado> articulosBuscados = new List<ArticuloBuscado>();

        //        foreach (var articulo in articulosProveedorBuscados)
        //        {
        //            ArticuloBuscado articuloBuscado = new ArticuloBuscado();

        //            articuloBuscado.id = articulo.id;
        //            articuloBuscado.codigo = articulo.codigo;
        //            articuloBuscado.descripcion = articulo.descripcion;
        //            articuloBuscado.costo = articulo.costo;
        //            articuloBuscado.precioVenta = articulo.precioVenta;

        //            articulosBuscados.Add(articuloBuscado);                    
        //        }

        //        var TheJson = TheSerializer.Serialize(articulosBuscados);

        //        return TheJson;
        //    }

        //    return "";
        //}

    }

    //public class ArticuloBuscado
    //{
    //    public int id;
    //    public string codigo;
    //    public string descripcion;
    //    public decimal costo;
    //    public decimal precioVenta;
    //}
}