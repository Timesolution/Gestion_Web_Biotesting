using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Millas_Api.Controladores;
using Neodynamic.WebControls.BarcodeProfessional;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Task_Api;
using Task_Api.Entitys;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMFacturasImagenes : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFacturacion controlador = new controladorFacturacion();
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArticuloEntity = new ControladorArticulosEntity();
        controladorCliente contCliente = new controladorCliente();
        public PlaceHolder phArticulos = new PlaceHolder();
        controladorRemitos cr = new controladorRemitos();
        controladorSucursal cs = new controladorSucursal();
        controladorTarjeta ct = new controladorTarjeta();
        Configuracion configuracion = new Configuracion();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        ControladorVendedorEntity contVendedorEntity = new ControladorVendedorEntity();
        controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();

        //factura
        Factura factura = new Factura();
        Cliente cliente = new Cliente();

        Configuracion c = new Configuracion();

        int accion;
        int idEmpresa;
        int idSucursal;
        int idPtoVentaUser;
        int idClientePadre;

        //flag si cambio la fecha de la factura
        int flag_cambioFecha = 0;

        int flag_clienteModal = 0;

        DataTable lstPagosTemp;
        DataTable dtTrazasTemp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region original
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.idClientePadre = Convert.ToInt32(Request.QueryString["cp"]);

                btnFactImagen.Attributes.Add("onclick", " this.disabled = true;  " + btnFactImagen.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnFactImagen, null) + ";");

                btnAgregar.Attributes.Add("onclick", " this.disabled = true;  " + btnAgregarRemitir.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                btnAgregarRemitir.Attributes.Add("onclick", " this.disabled = true; " + btnAgregar.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarRemitir, null) + ";");
                btnRefacturar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnRefacturar, null) + ";");
                btnCargarTraza.Attributes.Add("onclick", " this.disabled = true;  " + btnCargarTraza.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnCargarTraza, null) + ";");


                //dibujo los items en la tabla
                if (Session["Factura"] != null)
                {
                    this.cargarItems();
                    this.cargarTablaArticulosModoImagenes();
                }
                cargarArticulosFavoritosPh();
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Ingreso a facturacion.");

                    Session["CobroAnticipo"] = null;
                    Session["PagoCuentaAnticipo"] = null;
                    Session["PagoCuentaAnticipoMutual"] = null;
                    Session["idGrupo"] = null;

                    this.verificarModoBlanco();

                    //genero la factura de la session
                    idEmpresa = (int)Session["Login_EmpUser"];
                    idSucursal = (int)Session["Login_SucUser"];
                    idPtoVentaUser = (int)Session["Login_PtoUser"];

                    lstPagosTemp = new DataTable();
                    this.InicializarListaPagos();

                    dtTrazasTemp = new DataTable();
                    //this.InicializarListaTrazas();

                    Factura fac = new Factura();
                    Session.Add("Factura", fac);

                    //cargas iniciales listas
                    this.cargarEmpleados();
                    //this.cargarTipoFactura();
                    this.cargarVendedor();
                    this.cargarFormaPAgo();
                    this.cargarFormasVenta();
                    this.cargarListaPrecio();
                    this.cargarClientes();
                    this.cargarProveedoresCombustible();
                    this.cargarEmpresas();
                    //this.cargarTarjetas();
                    this.cargarOperadores();
                    this.cargarMutuales();
                    this.ListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
                    this.ListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
                    this.cargarIvaClientes();
                    if (accion != 6 && accion != 7 && accion != 9)
                    {
                        //si el usuario tiene pto vta selecciono la del user
                        this.ListPuntoVenta.SelectedValue = this.idPtoVentaUser.ToString();
                    }
                    else
                    {
                        //selecciono punto de venta por defecto
                        this.ListPuntoVenta.SelectedIndex = 1;
                    }

                    //si es punto fical muesto el boton cierre Z
                    PuntoVenta pv = this.cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    if (pv.formaFacturar == "Fiscal")
                    {
                        this.btnCierreZ.Visible = true;
                    }
                    else
                    {
                        this.btnCierreZ.Visible = false;
                    }

                    if (pv.formaFacturar == "Electronica")
                    {
                        //cargo Paises para exportacion
                        this.cargarPaisesExportacion();
                    }

                    //verifico que no este cerrada la caja para el punto de venta
                    this.verificarCierreCaja();

                    if (accion != 6 && accion != 7)
                    {
                        this.obtenerNroFacturaInicio();
                        //Me fijo si hay que cargar un cliente por defecto
                        this.verificarClienteDefecto();
                    }
                    else
                    {
                        this.obtenerNroNotaCreditoInicio();
                    }

                    //vengo desde el remito y voy a facturar
                    if (this.accion == 4)
                    {
                        int idRemito = Convert.ToInt32(Request.QueryString["id_rem"]);
                        GenerarFacturaRemito(idRemito);
                    }
                    //vengo desde pedidos y voy a facturar
                    if (this.accion == 5)
                    {
                        string pedidos = Request.QueryString["pedidos"];
                        GenerarFacturaPedido(pedidos);
                    }
                    //genero nota de creditos desde facturas
                    if (this.accion == 6)
                    {
                        string facturas = Request.QueryString["facturas"];
                        this.GenerarNotaCredito(facturas);
                        //habilito panel comentarios
                        this.phDatosEntrega.Visible = true;
                    }

                    //genero factura desde refacturacion PRP
                    if (this.accion == 9)
                    {
                        string clientePrp = Request.QueryString["prpsc"];
                        string presupuestos = Request.QueryString["prps"];
                        this.GenerarRefacturacion(presupuestos, clientePrp);
                        this.panelBusquedaCliente.Visible = false;
                        //this.lbtnAccion.Visible = false;
                    }

                    //si vengo del modulo de mascotas para facturar agenda
                    //cargo el cliente del propietario
                    if (this.accion == 11)
                    {
                        int idcliente = Convert.ToInt32(Request.QueryString["cliente"]);
                        if (idcliente > 0)
                        {
                            this.cargarClienteEnLista(idcliente);
                            this.cargarCliente(idcliente);
                        }
                    }

                    //Vengo desde pedidos y voy a facturar por Cliente Padre
                    if (this.accion == 12)
                    {
                        string pedidos = Request.QueryString["pedidos"];
                        GenerarFacturaPedidoPorPadre(pedidos);
                    }

                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaPagareMutual.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaVtoCuotaMutual.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                    this.txtCodigo.Focus();
                }

                this.cargarTablaPAgos();

                //si es punto preimpresa dejo modificar fecha                
                PuntoVenta ptovta = this.cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                if (ptovta.formaFacturar == "Preimpresa")
                {
                    //dejo modificar fecha
                    this.txtFecha.Attributes.Remove("Disabled");
                }
                if (this.txtFecha.Text == "")
                {
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                //si viene de la pantalla de clientes, modal
                if (Session["FacturasABM_ClienteModal"] != null)
                {
                    //seleccion cliente desde modal
                    this.flag_clienteModal = 1;
                    this.cargarClienteDesdeModal();

                }
                //si viene de la pantalla de articulos, modal
                if (Session["FacturasABM_ArticuloModal"] != null)
                {
                    //obtengo codigo
                    string CodArt = Session["FacturasABM_ArticuloModal"] as string;
                    this.txtCodigo.Text = CodArt;
                    this.cargarProducto(this.txtCodigo.Text);
                    this.actualizarTotales();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.foco(this.txtCantidad.ClientID));
                }

                //Dejo editable el campo de descripcion del articulo o no
                this.verficarConfiguracionEditar();
                //Si se egresa Stock por Remito, oculto el Boton 'Facturar' para evitar que se olviden de bajar stock.
                this.verficarConfiguracionEgresoStock();
                //Verifico si el PERFIL tiene permitido hacer NC
                this.verificarPermisoNC();
                //verifico si el PERFIL tiene permitido cambiar de sucursal seleccionada
                this.verficarPermisoCambiarSucursal();
                //verifico si el PERFIL tiene permitido vender en Cta Cte
                this.verificarPermisoVentaCtaCte();
                //verifico si hace vta de combustible para mostrar o no el panel(tab)
                this.verificarVtaCombustible();

                //verifico si es postback y tengo que llenar la tabla de las trazas para poder obtener el estado de los chkbox
                if (this.lblMovTraza.Text != "")
                {
                    string idTraza = this.lblMovTraza.Text;
                    int idArticulo = Convert.ToInt32(idTraza.Split('_')[1]);
                    this.CargarTrazasPH(idArticulo);
                }
                if (this.lbMovTrazaNueva.Text != "")
                {
                    Factura f = Session["Factura"] as Factura;
                    string posItem = this.lbMovTrazaNueva.Text.Split('_')[2];
                    ItemFactura item = f.items[Convert.ToInt32(posItem)];

                    this.cargarCamposGrupo(item.articulo);
                    this.cargarTrazasAgregadas();
                }

                //verifico si es postback y tengo que llenar la tabla de las solicitudes para poder obtener el estado de los rbtn
                if (this.lblMovSolicitud.Text == "OK")
                {
                    this.buscarSolicitudes();
                }
                if (this.rbtnPagoCuentaCredito.Checked)
                {
                    this.cargarPagosCuentaCliente();
                }
                if (this.rbtnPagoCuentaMutual.Checked)
                {
                    this.cargarPagosCuentaMutualCliente();
                }
                if (this.DropListFormaPago.SelectedItem.Text == "Credito")
                {
                    this.verificarCobroAnticipo();
                }

                if (this.accion == 9)
                {
                    this.btnAgregar.Visible = false;
                    this.btnAgregarRemitir.Visible = false;
                    this.btnRefacturar.Visible = true;

                    try
                    {
                        this.DropListClientes.Attributes.Add("disabled", "disabled");
                    }
                    catch { }
                }
                #endregion
                //alta rapida cliente
                this.cargarTipoClientes();
                this.cargarGrupoClientes();
                this.generarCodigo();
                //fin alta rapida
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        #region original


        #region Generacion de facuras desde otros documentos
        /// <summary>
        /// Genera el remito con los datos recibidos del pedido
        /// </summary>
        /// 
        public void GenerarFacturaRemito(int id_rem)
        {
            try
            {
                this.factura = Session["Factura"] as Factura;
                Remito r = new Remito();
                r = cr.obtenerRemitoId(id_rem);

                //Obtengo los comentarios ingresados en el remito.
                DataTable dtComentariosRemito = cr.obtenerComentarioRemito(r.id);
                r.comentario = dtComentariosRemito.Rows[0]["observaciones"].ToString();

                Factura f = controlador.AsignarRemito(r);
                Session.Add("Factura", f);
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);
                this.cargarCliente(f.cliente.id);
                //this.DropListClientes.SelectedValue = f.cliente.id.ToString();
                //cargocliente
                Session.Add("FacturasABM_ClienteModal", f.cliente.id);
                this.cargarClienteDesdeModal();

                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();
                this.cargarComentariosDeRemito();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos remito a factura " + ex.Message));
            }
        }
        public void GenerarFacturaPedido(string pedidos)
        {
            try
            {
                this.factura = Session["Factura"] as Factura;
                string recalcularPrecio = WebConfigurationManager.AppSettings.Get("recalcularPrecioPedido");
                Configuracion config = new Configuracion();
                Factura f = controlador.GenerarFacturaDesdePedido(pedidos, Convert.ToInt32(recalcularPrecio));
                Session.Add("Factura", f);
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.ListSucursal.SelectedValue = "-1";
                this.ListPuntoVenta.SelectedValue = "-1";
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);

                //antes de cargar cliente me guardo temporalmente el descuento y subtotal
                decimal subtotal = f.subTotal;
                decimal descuento = f.descuento;

                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                //cargo los datos de entrega del pedido.
                this.txtFechaEntrega.Text = f.pedidos[0].fechaEntrega.ToString("dd/MM/yyyy");
                this.txtHorarioEntrega.Text = f.pedidos[0].horaEntrega;
                this.txtBultosEntrega.Text = f.bultosEntrega;
                //cargocliente
                Session.Add("FacturasABM_ClienteModal", f.cliente.id);
                this.cargarClienteDesdeModal();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();

                if (config.infoImportacionFacturas == "1")
                {
                    foreach (ItemFactura item in f.items)
                    {
                        this.agregarInfoDespachoDesdePedido(item);
                    }
                }

                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();

                var itemCero = f.items.Where(x => x.total == 0).FirstOrDefault();
                if (itemCero != null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Existe/n item/s en la factura con precio final cero."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));
            }
        }
        public void GenerarNotaCredito(string facturas)
        {
            try
            {
                Configuracion config = new Configuracion();
                Factura f = controlador.GenerarNotaCredito(facturas);

                Session.Add("Factura", f);

                //pongo el iva del cliente que tenia al momento en que se le hizo esa fc
                if (config.siemprePRP == "1")
                {
                    int idDoc = Convert.ToInt32(facturas.Split(';')[0]);
                    int idIva = this.obtenerDatosIvasFactura(idDoc);
                    if (idIva > 0)
                        this.establecerIvaCliente(idIva, f.cliente.id);
                }

                this.factura = Session["Factura"] as Factura;
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);
                this.cargarCliente(f.cliente.id);
                this.cargarClienteEnLista(f.cliente.id);
                this.DropListClientes.SelectedValue = f.cliente.id.ToString();
                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                if (!String.IsNullOrEmpty(f.comentario))
                {
                    this.checkDatos.Checked = true;
                    this.txtComentarios.Text = f.comentario;
                }
                if (f.formaPAgo.forma == "Tarjeta")
                {
                    this.cargarPagosTarjetasDesdeFactura(f);
                }

                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();

                this.lblSaldoTarjeta.Text = f.total.ToString();
                this.lblMontoOriginal.Text = f.total.ToString();

                this.DropListClientes.Attributes.Add("disabled", "disabled");

                this.lbtnAccion.Visible = false;
                this.panelBusquedaCliente.Visible = false;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));

            }
        }
        private void cargarPagosTarjetasDesdeFactura(Factura f)
        {
            try
            {
                string facturas = Request.QueryString["facturas"];
                controladorCobranza contCob = new controladorCobranza();
                Cobro c = contCob.obtenerCobroByFactura(Convert.ToInt32(facturas.Split(';')[0]), f.cliente.id);
                Pago_Tarjeta pt = new Pago_Tarjeta();
                DataTable dt = lstPago;

                foreach (var pago in c.pagos)
                {
                    if (pt.sosPagoTarjeta(pago))
                    {
                        //Guardar la info de pago en el DT Temporal de pagos                        
                        DataRow dr = dt.NewRow();
                        dr["Tipo Pago"] = (pago as Pago_Tarjeta).tarjeta.nombre;
                        dr["Importe"] = pago.monto;
                        dr["Neto"] = pago.monto;
                        dr["Recargo"] = Convert.ToDecimal(0.00);

                        dt.Rows.Add(dr);
                        lstPago = dt;
                    }
                    else
                    {

                        DataRow dr = dt.NewRow();
                        dr["Tipo Pago"] = "Efectivo";
                        dr["Importe"] = pago.monto;
                        dr["Neto"] = pago.monto;
                        dr["Recargo"] = Convert.ToDecimal(0.00);

                        dt.Rows.Add(dr);
                        lstPago = dt;
                    }
                }
            }
            catch
            {

            }
        }
        public void GenerarRefacturacion(string facturas, string idCLiente)
        {
            try
            {
                this.factura = Session["Factura"] as Factura;
                Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(facturas));
                var clienteAux = this.contCliente.obtenerClienteID(f.cliente.id);
                if (!string.IsNullOrEmpty(idCLiente))//si cambiaron el cliente al momento de facturar
                {
                    f.cliente.id = Convert.ToInt32(idCLiente);
                }

                this.cargarClienteEnLista(f.cliente.id);

                Session.Add("Factura", f);

                string tipoOriginal = f.tipo.tipo;
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);
                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                this.cargarCliente(f.cliente.id);
                this.DropListClientes.SelectedValue = f.cliente.id.ToString();
                this.cargarItems();
                this.actualizarTotales();

                this.txtComentarios.Text += "\n Correspondiente a PRP Nº " + factura.numero + " Cliente PRP " + clienteAux.razonSocial;

                if (tipoOriginal.Contains("Debito"))
                {
                    this.obtenerNroNotaDebito();
                }
                else
                {
                    if (f.tipo.tipo.Contains("Credito"))
                    {
                        this.obtenerNroNotaDebito();
                    }
                    else
                    {
                        this.obtenerNroFactura();
                    }
                }


                int ok = this.controlador.verificarRefacturarProveedor(f);
                if (ok < 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede refacturar porque uno o más articulos tienen proveedor con condicion IVA NO INFORMA."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));

            }
        }
        #endregion

        #region verificaciones iniciales
        public void verificarModoBlanco()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.modoBlanco == "1")
                {
                    this.lbtnPRP.Visible = false;
                    this.lbNC.Visible = false;
                    this.lbND.Visible = false;
                    this.lbtnPRP.Attributes.Add("style", "display:none");
                    this.lbNC.Attributes.Add("style", "display:none");
                    this.lbND.Attributes.Add("style", "display:none");
                }
            }
            catch
            {

            }
        }
        public void verificarCierreCaja()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                int sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);

                var fecha = contCaja.obtenerUltimaApertura(sucursal, ptoVenta);
                //si la fecha de apertura es mas gande q hoy no lo dejo
                if (DateTime.Now < fecha)
                {
                    //ya existe una un cierre para el dia de hoy
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se realizo un cierre de caja en el dia de hoy para este punto de venta. La proxima fecha de apertura es " + fecha.ToString("dd/MM/yyyy")));
                    this.btnAgregar.Visible = false;
                    this.btnAgregarRemitir.Visible = false;
                }
                else
                {
                    this.btnAgregar.Visible = true;
                    this.btnAgregarRemitir.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando cierre de caja. " + ex.Message));
            }
        }
        public void verificarClienteDefecto()
        {
            try
            {
                //string idCliente = WebConfigurationManager.AppSettings.Get("ClienteDefecto");

                idSucursal = (int)Session["Login_SucUser"];
                if (IsPostBack)//Si cambio la sucursal en el list manualmente uso ese valor en lugar del de usuario.
                {
                    idSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                }
                Sucursal s = this.cs.obtenerSucursalID(idSucursal);
                string idCliente = s.clienteDefecto.ToString();

                if (idCliente != "-1" && idCliente != null)
                {
                    if (this.DropListClientes.Items.FindByValue(idCliente) == null)
                    {
                        this.cargarClienteEnLista(Convert.ToInt32(idCliente));
                    }
                    this.DropListClientes.SelectedValue = idCliente;
                    this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                    this.obtenerNroFactura();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error verificando cliente por defecto " + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        public void verficarConfiguracionEditar()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permisoDesc = listPermisos.Where(x => x == "80").FirstOrDefault();
                string permisoPrecio = listPermisos.Where(x => x == "81").FirstOrDefault();

                Configuracion c = new Configuracion();
                if (c.editarArticulo == "1")
                {
                    if (permisoDesc != null)
                    {
                        this.txtDescripcion.Attributes.Remove("disabled");

                    }
                    if (permisoPrecio != null)
                    {
                        this.txtPUnitario.Attributes.Remove("disabled");
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error verificando configuracion editar descripcion.  " + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        public void verficarConfiguracionEgresoStock()
        {
            try
            {
                Configuracion c = new Configuracion();
                if (c.egresoStock == "1")
                {

                    if (this.labelNroFactura.Text.Contains("Credito") || this.accion == 4)
                    {
                        this.btnAgregarRemitir.Attributes.Add("style", "display:none");
                        this.btnAgregar.Attributes.Remove("style");
                    }
                    else
                    {
                        this.btnAgregar.Attributes.Add("style", "display:none");
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error verificando configuracion editar descripcion.  " + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        public int verficarPermisoFactSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "71")
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
        public void verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "75")
                        {
                            this.ListSucursal.Attributes.Remove("disabled");
                            this.ListEmpresa.Attributes.Remove("disabled");
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void verificarPermisoNC()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "72")
                        {
                            this.lbtnNC.Visible = true;
                            this.lbNC.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void verificarAlerta()
        {
            try
            {
                string script = "";
                string alerta1 = "";
                string alerta2 = "";
                string alerta3 = "";
                string alerta4 = "";

                int okSMS = 0;

                Cliente c = contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    if (!String.IsNullOrEmpty(c.alerta.descripcion))
                    {
                        c.alerta.descripcion = Regex.Replace(c.alerta.descripcion, @"\t|\n|\r", "");
                    }
                    script += "$.msgbox(\"Alerta Cliente: " + c.alerta.descripcion + ". \");";
                    alerta1 += "Alerta Cliente: " + c.alerta.descripcion + "." + "<br>";
                }

                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                decimal saldoMax = contCC.saldoCuentaPorCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));

                if (saldoMax >= this.cliente.saldoMax && this.cliente.saldoMax > 0)
                {
                    okSMS = this.verificarMostrarAccionSMS();
                    if (okSMS <= 0)//para saber si muestro el cartel javascript o no
                    {
                        script += "$.msgbox(\"Alerta: Cliente con saldo max. superado ($" + c.saldoMax + ") \");";
                        alerta2 += "Alerta: Cliente con saldo max. superado ($" + c.saldoMax + ")." + "<br>";
                    }
                }

                if (c.vencFC > 0)
                {
                    decimal saldo = 0;
                    DataTable dtImpagas = contCC.obtenerMovimientosImpagas("01/01/2015", this.txtFecha.Text, this.idSucursal, c.id, 0, -1);

                    if (dtImpagas.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtImpagas.Rows)
                        {
                            DateTime fechaImpaga = Convert.ToDateTime(row["fecha"].ToString());
                            if ((DateTime.Today.DayOfYear - fechaImpaga.DayOfYear) > c.vencFC)
                            {
                                saldo += Convert.ToDecimal(row["saldo"]);
                            }
                        }
                    }

                    if (saldo > 0)
                    {
                        script += "$.msgbox(\"Alerta: Cliente con facturas impagas mayor a " + c.vencFC + " dias. \");";
                        alerta3 += "Alerta: Cliente con facturas impagas mayor a " + c.vencFC + " dias." + "<br>";
                    }
                }

                var sucu = this.contClienteEntity.obtenerSucursalesCliente(c.id);
                if (sucu != null)
                {
                    if (sucu.Count > 0)
                    {
                        int permiso = this.verficarPermisoFactSucursal();
                        if (permiso <= 0)
                        {
                            script += "$.msgbox(\"Alerta: No tiene permiso para facturar entre sucursales. \");";
                            alerta3 += "Alerta: No tiene permiso para facturar entre sucursales." + "<br>";
                        }
                    }
                }


                if (script != "" || okSMS > 0)
                {
                    if (this.flag_clienteModal > 0 || (!IsPostBack && this.flag_clienteModal == 0))//si vienen desde modal uso un script sino uso el otro.
                    {
                        this.abrirModalEnvioSMS(0);
                        if ((alerta1 + alerta2 + alerta3) != "")
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta1 + alerta2 + alerta3));
                        }
                    }
                    else
                    {
                        this.abrirModalEnvioSMS(1);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", script, true);
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        private void verificarAlertaArticulo(Articulo art)
        {
            try
            {
                string script = "";
                string alerta = "";
                AlertaArticulo alert = this.contArticulo.obtenerAlertaArticuloByID(art.id);
                if (alert != null)
                {
                    if (!String.IsNullOrEmpty(alert.descripcion))
                    {
                        //concateno alerta
                        script += "$.msgbox(\"Alerta: " + alert.descripcion + ". \");";
                        alerta += "Alerta: " + alert.descripcion + ".<br>";
                    }
                }

                //busco si el articulo ya esta en la factura mediante el codigo
                Factura f = Session["Factura"] as Factura;
                var a = f.items.Where(x => x.articulo.codigo == art.codigo).FirstOrDefault();
                if (a != null)
                {
                    //si esta concateno la alerta
                    script += "$.msgbox(\"Este articulo ya fue cargado previamente a la factura: Cod.: " + art.codigo + " \");";
                    alerta += "Este articulo fue cargado previamente a la factura: Articulo: " + art.codigo;
                }

                //miro si esta en alguna promocion 
                ControladorArticulosEntity contEnt = new ControladorArticulosEntity();
                decimal cant = 0;
                if (this.txtCantidad.Text != "")
                    cant = Convert.ToDecimal(this.txtCantidad.Text);
                Gestion_Api.Entitys.Promocione p = contEnt.obtenerPromocionValidaArticulo(art.id, Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.DropListFormaPago.SelectedValue), Convert.ToInt32(this.DropListLista.SelectedValue), Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")), cant);
                if (p != null)
                {
                    script += "$.msgbox(\"Articulo en promocion: " + p.Promocion + ". Todos los Artículos a facturar deben tener Promoción." + " \");";
                    alerta += "Articulo en promocion: " + p.Promocion + ". Todos los Artículos a facturar deben tener Promoción.";
                }

                if (script != "")
                {
                    string CodArt = Session["FacturasABM_ArticuloModal"] as string;
                    if (!String.IsNullOrEmpty(CodArt))//si vienen desde modal uso un script sino uso el otro.
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta));
                    }
                    else
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                    }
                }
            }
            catch
            {

            }
        }
        private void verificarPermisoVentaCtaCte()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permisoCtaCte = listPermisos.Where(x => x == "86").FirstOrDefault();

                if (permisoCtaCte == null)
                {
                    try
                    {
                        this.DropListFormaPago.Items.Remove(this.DropListFormaPago.Items.FindByText("Cuenta Corriente"));
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void verificarVtaCombustible()
        {
            try
            {
                string vtaCombustible = WebConfigurationManager.AppSettings.Get("Combustible");
                if (vtaCombustible == "1")
                {
                    this.linkCombustible.Visible = true;
                }
            }
            catch
            {

            }
        }
        #endregion 

        #region cargar Datos iniciales

        public void cargarVendedor()
        {
            try
            {
                if (DropListVendedor.Items.Count > 0)
                {
                    DropListVendedor.Items.Clear();
                    ListVendedoresAR.Items.Clear();
                }

                controladorVendedor contVendedor = new controladorVendedor();
                //DataTable dt = contVendedor.obtenerVendedores();
                int idSucursal = 0;
                if (ListSucursal.Items.Count > 0)
                {
                    idSucursal = Convert.ToInt32(ListSucursal.SelectedValue);
                }
                else
                {
                    idSucursal = (int)Session["Login_SucUser"];
                }
                DataTable dt = contVendedor.obtenerVendedoresBySuc(idSucursal);

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    DropListVendedor.Items.Add(item);
                    ListVendedoresAR.Items.Add(item);
                }
                ListVendedoresAR.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                // agrego todos
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

                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";

                this.ListPuntoVenta.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pto ventas. " + ex.Message));
            }
        }
        public void cargarFormaPAgo()
        {
            try
            {
                DataTable dt = this.controlador.obtenerFormasPago();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["forma"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListFormaPago.DataSource = dt;
                this.DropListFormaPago.DataValueField = "id";
                this.DropListFormaPago.DataTextField = "forma";
                this.DropListFormaPago.DataBind();

                this.DropListFormaPagoAR.DataSource = dt;
                this.DropListFormaPagoAR.DataValueField = "id";
                this.DropListFormaPagoAR.DataTextField = "forma";
                this.DropListFormaPagoAR.DataBind();
                this.DropListFormaPagoAR.SelectedValue = "1";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }
        public void cargarFormasVenta()
        {
            try
            {
                controladorFactEntity contFcEnt = new controladorFactEntity();
                List<Gestion_Api.Entitys.Formas_Venta> formas = contFcEnt.obtenerFormasVenta();
                formas = formas.OrderBy(x => x.Nombre).ToList();

                this.ListFormaVenta.DataSource = formas;
                this.ListFormaVenta.DataValueField = "Id";
                this.ListFormaVenta.DataTextField = "Nombre";
                this.ListFormaVenta.DataBind();

                this.ListFormaVenta.Items.Insert(0, new ListItem("NO", "-1"));
            }
            catch
            {

            }
        }
        public void cargarFormasVentaByCliente(int idC)
        {
            try
            {
                if (idC > 0)
                {
                    //cargo solo la forma que tiene el cliente
                    this.ListFormaVenta.Items.Clear();
                    var forma = this.contClienteEntity.obtenerFormaVentaCliente(idC);

                    this.ListFormaVenta.Items.Insert(0, new ListItem("NORMAL", "-1"));
                    this.ListFormaVenta.Items.Insert(1, new ListItem(forma.Nombre, forma.Id.ToString()));
                }
                else
                {
                    this.ListFormaVenta.Items.Insert(0, new ListItem("NORMAL", "-1"));
                }
            }
            catch
            {

            }
        }
        public void cargarListaPrecio()
        {
            try
            {
                this.DropListLista.ClearSelection();
                ControladorFormasPago contForma = new ControladorFormasPago();
                List<Gestion_Api.Entitys.listasPrecio> listas = contForma.obtenerListasByFormaPago(Convert.ToInt32(this.DropListFormaPago.SelectedValue));

                if (listas != null && listas.Count > 0)
                {
                    listas = listas.OrderBy(o => o.nombre).ToList();
                    this.DropListLista.DataSource = listas;
                    this.DropListLista.DataValueField = "id";
                    this.DropListLista.DataTextField = "nombre";
                    this.DropListLista.DataBind();

                    this.ListListaPreciosAR.DataSource = listas;
                    this.ListListaPreciosAR.DataValueField = "id";
                    this.ListListaPreciosAR.DataTextField = "nombre";
                    this.ListListaPreciosAR.DataBind();
                }
                else
                {
                    DataTable dt = this.contCliente.obtenerListaPrecios();

                    //agrego seleccione
                    DataRow dr = dt.NewRow();
                    dr["nombre"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                    this.DropListLista.DataSource = dt;
                    this.DropListLista.DataValueField = "id";
                    this.DropListLista.DataTextField = "nombre";
                    this.DropListLista.DataBind();


                    this.ListListaPreciosAR.DataSource = dt;
                    this.ListListaPreciosAR.DataValueField = "id";
                    this.ListListaPreciosAR.DataTextField = "nombre";
                    this.ListListaPreciosAR.DataBind();
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";

                this.ListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarOperadores()
        {
            try
            {
                List<Gestion_Api.Entitys.Operadores_Tarjeta> operadores = this.ct.obtenerOperadores();

                this.ListOperadores.DataSource = operadores;
                this.ListOperadores.DataValueField = "Id";
                this.ListOperadores.DataTextField = "Operador";
                this.ListOperadores.DataBind();

                this.ListOperadores.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando operadores. " + ex.Message));

            }
        }
        public void cargarTarjetasByOperador(int idOperador)
        {
            try
            {
                List<Gestion_Api.Entitys.Tarjeta> tarjetas = this.ct.obtenerTarjetasEntityByOperador(idOperador);
                tarjetas = tarjetas.OrderBy(x => x.nombre).ToList();
                this.ListTarjetas.DataSource = tarjetas;
                this.ListTarjetas.DataValueField = "id";
                this.ListTarjetas.DataTextField = "nombre";
                this.ListTarjetas.DataBind();

                this.ListTarjetas.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        public void cargarTarjetas()
        {
            try
            {
                controladorTarjeta contTarj = new controladorTarjeta();
                DataTable dt = contTarj.obtenerTarjetasDT();

                DataRow dr = dt.NewRow();
                dr["id"] = "-1";
                dr["nombre"] = "Seleccione...";
                dt.Rows.InsertAt(dr, 0);

                this.ListTarjetas.DataSource = dt;
                this.ListTarjetas.DataValueField = "id";
                this.ListTarjetas.DataTextField = "nombre";

                this.ListTarjetas.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tarjetas. " + ex.Message));
            }
        }
        public void cargarMutuales()
        {
            try
            {
                controladorFactEntity controlMutual = new controladorFactEntity();
                List<Gestion_Api.Entitys.Mutuale> mutuales = controlMutual.obtenerMutuales();
                this.ListMutuales.DataSource = mutuales;
                this.ListMutuales.DataValueField = "Id";
                this.ListMutuales.DataTextField = "Nombre";

                this.ListMutuales.DataBind();

                this.ListMutuales.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        public void cargarEmpleados()
        {
            try
            {
                controladorEmpleado contrEmp = new controladorEmpleado();
                DataTable dt = contrEmp.obtenerEmpleadosNoVendedoresDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreC"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListEmpleados.DataSource = dt;
                this.ListEmpleados.DataValueField = "id";
                this.ListEmpleados.DataTextField = "NombreC";

                this.ListEmpleados.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                //this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        public void cargarProveedoresCombustible()
        {
            try
            {
                ControladorArticulosEntity contArticuloEnt = new ControladorArticulosEntity();

                List<Gestion_Api.Entitys.cliente> provedores = contArticuloEnt.obtenerProveedoresCombustibles();

                this.ListProveedorCombustible.DataSource = provedores;
                this.ListProveedorCombustible.DataValueField = "id";
                this.ListProveedorCombustible.DataTextField = "alias";
                this.ListProveedorCombustible.DataBind();

                this.ListProveedorCombustible.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        protected DataTable lstPago
        {

            get
            {
                if (ViewState["ListaPagos"] != null)
                {
                    return (DataTable)ViewState["ListaPagos"];
                }
                else
                {
                    return lstPagosTemp;
                }
            }
            set
            {
                ViewState["ListaPagos"] = value;
            }
        }
        private void cargarCliente(int idCliente)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                this.cliente = contCliente.obtenerClienteID(idCliente);
                Configuracion c = new Configuracion();

                if (this.cliente != null)
                {
                    this.lblMovSolicitud.Text = "";
                    if (this.accion != 9 && this.accion != 6 && c.siemprePRP == "1")//no es refact
                    {
                        this.labelCliente.Text = this.cliente.razonSocial.Replace('-', ' ') + " - " + "No Informa" + " - " + this.cliente.cuit;
                        this.lbNombreClienteImagenes.Text = this.cliente.razonSocial.Replace('-', ' ') + " - " + "No Informa" + " - " + this.cliente.cuit;
                    }
                    else
                    {
                        this.labelCliente.Text = this.cliente.razonSocial.Replace('-', ' ') + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                        this.lbNombreClienteImagenes.Text = this.cliente.razonSocial.Replace('-', ' ') + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                    }
                    if (this.cliente.cuit.Length == 11)
                    {
                        this.txtDniCredito.Text = this.cliente.cuit.Substring(2, this.cliente.cuit.Length - 3);
                    }
                    else
                    {
                        this.txtDniCredito.Text = this.cliente.cuit;
                    }

                    try
                    {
                        this.DropListVendedor.SelectedValue = this.cliente.vendedor.id.ToString();
                    }
                    catch { }
                    //this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();

                    //SI ES CONSUMIDOR FINAL no permito venta en CTA CTE
                    this.cargarFormaPAgo();
                    this.DropListFormaPago.SelectedValue = this.cliente.formaPago.id.ToString();
                    //CARGO LAS LISTAS DE PRECIO QUE TIENE LA FORMA DE PAGO QUE TIENE EL CLIENTE
                    this.cargarListaPrecio();
                    try
                    {
                        this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();
                    }
                    catch { }
                    //verifico si el PERFIL tiene permitido vender en Cta Cte
                    this.verificarPermisoVentaCtaCte();
                    if (this.cliente.iva == "Consumidor Final")
                    {

                        if (c.consumidorFinalCC != "1")
                        {
                            //this.DropListFormaPago.Items.RemoveAt(2);
                            try
                            {
                                this.DropListFormaPago.Items.Remove(this.DropListFormaPago.Items.FindByText("Cuenta Corriente"));
                            }
                            catch { }
                        }
                        this.chkIvaNoInformado.Visible = true;
                        this.lblPercepcionCF.Visible = true;
                        this.txtPercepcionCF.Visible = true;

                    }
                    else
                    {
                        this.chkIvaNoInformado.Visible = false;
                        this.lblPercepcionCF.Visible = false;
                        this.txtPercepcionCF.Visible = false;

                    }
                    this.chkIvaNoInformado.Checked = false;

                    try
                    {
                        this.DropListFormaPago.SelectedValue = this.cliente.formaPago.id.ToString();
                    }
                    catch { }

                    this.cambiarLabelNro();

                    //Si el tipo de pago por defecto es tarjeta
                    if (DropListFormaPago.SelectedItem.Text == "Tarjeta")
                    {
                        this.btnTarjeta.Visible = true;
                        this.btnCredito.Visible = false;
                        this.btnMutual.Visible = false;
                    }
                    else
                    {
                        if (DropListFormaPago.SelectedItem.Text == "Credito")
                        {
                            this.btnCredito.Visible = true;
                            this.btnMutual.Visible = false;
                        }
                        else
                        {
                            this.btnMutual.Visible = true;
                            this.btnCredito.Visible = false;
                        }
                        this.btnTarjeta.Visible = false;
                    }

                    if (DropListFormaPago.SelectedItem.Text == "Cuenta Corriente")
                    {
                        this.lbFormaDePago.Text = "Forma de pago: Cuenta Corriente";
                    }
                    //pongo en cero por si eligieron un cliente con desc o percepciones y dps lo cambiaron
                    this.txtPorcDescuento.Text = "0";
                    this.txtPorcRetencion.Text = "0";

                    //cargo el descuento
                    if (this.cliente.descFC > 0 && this.accion != 9)
                    {
                        this.txtPorcDescuento.Text = Decimal.Round(this.cliente.descFC, 2).ToString();
                    }
                    //cargo Ingresos brutos
                    var IIBB = this.contClienteEntity.obtenerIngresosBrutoCliente(idCliente);
                    if (IIBB != null)
                    {
                        this.txtPorcRetencion.Text = IIBB.Percepcion.ToString();
                    }
                    //cargar forma venta si es porcentual
                    try
                    {
                        this.cargarFormasVentaByCliente(idCliente);
                        var fv = this.contClienteEntity.obtenerFormaVentaCliente(idCliente);
                        if (fv != null)
                        {
                            this.ListFormaVenta.SelectedValue = fv.Id.ToString();
                        }
                    }
                    catch { }

                    //datos entrega
                    try
                    {
                        var entrega = this.contClienteEntity.obtenerEntregaCliente(idCliente);
                        if (entrega != null)
                        {
                            this.phDatosEntrega.Visible = true;
                            this.txtHorarioEntrega.Text = entrega.HorarioEntrega;
                            this.txtFechaEntrega.Text = DateTime.Today.ToString("dd/MM/yyyy");
                            this.checkDatos.Checked = true;

                        }
                        this.cargarDatosEnvioCliente(idCliente);
                    }
                    catch { }

                    //cargo el cliente en la factura session
                    Factura f = Session["Factura"] as Factura;
                    //f.cliente.id = this.cliente.id;
                    f.cliente = contCliente.obtenerClienteID(this.cliente.id);
                    Session.Add("Factura", f);
                    this.verificarAlerta();
                    Session["FacturasABM_ClienteModal"] = null;
                    Session["CobroAnticipo"] = null;
                    //verifico si tiene permitido facturar entre sucursales
                    if (this.verficarPermisoFactSucursal() == 1)
                    {
                        //verifico si tiene sucursales 
                        var sucu = this.contClienteEntity.obtenerSucursalesCliente(idCliente);
                        if (sucu != null)
                        {
                            if (sucu.Count > 0)
                            {
                                //cargo
                                this.ListSucursalCliente.Visible = true;
                                this.ListSucursalCliente.DataSource = sucu;
                                this.ListSucursalCliente.DataTextField = "nombre";
                                this.ListSucursalCliente.DataValueField = "id";
                                this.ListSucursalCliente.DataBind();

                                this.ListSucursalCliente.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                                if (this.ListSucursalCliente.Items.Contains(this.ListSucursal.SelectedItem) == true)
                                {
                                    ListItem item = this.ListSucursalCliente.Items.FindByValue(this.ListSucursal.SelectedValue);
                                    this.ListSucursalCliente.Items.Remove(item);
                                }
                                this.lbtnStockDestinoProd.Visible = true;
                            }
                            else
                            {
                                this.lbtnStockDestinoProd.Visible = false;
                                this.ListSucursalCliente.Visible = false;
                            }
                        }
                        else
                        {
                            this.lbtnStockDestinoProd.Visible = false;
                            this.ListSucursalCliente.Visible = false;
                            this.btnAgregar.Visible = false;
                            this.btnAgregarRemitir.Visible = false;
                        }
                    }
                    else
                    {
                        this.ListSucursalCliente.Visible = false;
                    }


                }

                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra cliente "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarClienteDesdeModal()
        {
            try
            {
                //obtengo codigo
                int idCliente = (int)Session["FacturasABM_ClienteModal"];
                try
                {
                    this.DropListClientes.SelectedValue = idCliente.ToString();
                    if (this.DropListClientes.SelectedValue == "-1")
                    {
                        this.cargarClienteEnLista(idCliente);
                    }
                }
                catch
                {
                    //el cliente no estaba en el drop list
                    //lo agrego y selecciono
                    //lo busco y agrego
                    this.cargarClienteEnLista(idCliente);
                }

                this.cargarCliente(idCliente);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cliente desde modal. " + ex.Message));
            }
        }

        private void cargarClienteEnLista(int idCliente)
        {
            var c = contCliente.obtenerClienteID(idCliente);
            if (c != null)
            {
                this.DropListClientes.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
                this.DropListClientes.SelectedValue = idCliente.ToString();
            }
        }

        /// <summary>
        /// Obtiene el ultimo numero de factura
        /// </summary>
        /// <param name="iva"></param>
        /// 
        private void cambiarLabelNro()
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente[1].TrimStart().TrimEnd() == "No Informa")
                {
                    this.obtenerNroPresupuesto();
                    this.lbtnAccion.Text = "PRP <span class='caret'></span>";
                    this.lbtnFC.Visible = false;
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroFactura();
                    this.lbtnAccion.Text = "FC <span class='caret'></span>";
                    this.lbtnFC.Visible = true;
                    this.actualizarTotales();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }

        private void obtenerFactura(string iva)
        {
            try
            {
                //int fact;
                TipoDocumento ti = null;
                if (iva == "Responsable Inscripto")
                {
                    //ti = this.controlador.obtenerTipoFact("Factura A");
                    ti = this.controlador.obtenerFacturaNumero(1, 1);

                }
                else
                {
                    //ti = this.controlador.obtenerTipoFact("Factura B");
                    ti = this.controlador.obtenerFacturaNumero(1, 2);

                }
                //this.txtNroFactura.Text = ti.tipo + " " + ti.idNumeracion.ToString().PadLeft(8, '0'); ;

                //cargo el tipo en la factura session
                Factura f = Session["Factura"] as Factura;
                f.tipo.id = ti.id;
                Session.Add("Factura", f);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de factura. " + ex.Message));
            }
        }

        private void obtenerNroFactura()
        {
            try
            {
                this.btnFacturaE.Visible = false;
                this.btnAgregar.Visible = true;
                this.btnAgregarRemitir.Visible = true;

                if (accion != 6 && accion != 7)
                {
                    string[] cliente = this.labelCliente.Text.Split('-');
                    if (cliente[1].Contains("Responsable Inscripto"))
                    {
                        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        //como estoy en cotizacion pido el ultimo numero de este documento
                        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura A");
                        this.labelNroFactura.Text = "Factura A N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    }
                    else
                    {
                        if (cliente[1].Contains("Cliente del Exterior") || cliente[1].Contains("IVA Liberado"))
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //como estoy en cotizacion pido el ultimo numero de este documento
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura E");
                            this.labelNroFactura.Text = "Factura E N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            if (pv.formaFacturar == "Electronica")
                            {
                                this.btnFacturaE.Visible = true;
                                this.btnAgregar.Visible = false;
                                this.btnAgregarRemitir.Visible = false;
                                this.validarIdImpositivoCliente();
                            }
                        }
                        else
                        {
                            //si el iva es "No informa" presupuesto
                            if (cliente[1].Contains("No Informa"))
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Presupuesto");
                                this.labelNroFactura.Text = "Presupuesto N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            }
                            else
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                if (c.monotributo == "1")
                                {
                                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura C");
                                    this.labelNroFactura.Text = "Factura C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                }
                                else
                                {
                                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura B");
                                    this.labelNroFactura.Text = "Factura B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                }

                            }
                        }

                    }
                }
                else
                {
                    //si vengo de Factura-Accion-NotadeCredito me fijo en el tipo de doc del que vengo y no el tipo de IVA del cliente
                    string tipoDoc = this.factura.tipo.tipo;
                    if (tipoDoc.Contains("Presupuesto") || tipoDoc.Contains("PRP"))
                    {
                        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito PRP");
                        this.labelNroFactura.Text = "Nota de Credito PRP N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    }
                    else
                    {
                        if (tipoDoc.Contains("Factura A") || tipoDoc.Contains("Credito A"))
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito A");
                            this.labelNroFactura.Text = "Nota de Credito A N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        }
                        else
                        {
                            if (tipoDoc.Contains("Factura E") || tipoDoc.Contains("Credito E"))
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito E");
                                this.labelNroFactura.Text = "Nota de Credito E N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                if (pv.formaFacturar == "Electronica")
                                {
                                    this.btnFacturaE.Visible = true;
                                    this.btnAgregar.Visible = false;
                                    this.btnAgregarRemitir.Visible = false;
                                    //this.validarIdImpositivoCliente();
                                }
                            }
                            else
                            {
                                if (c.monotributo == "1")
                                {
                                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito C");
                                    this.labelNroFactura.Text = "Nota de Credito C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                }
                                else
                                {
                                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                                    this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }

        private void obtenerNroNotaCredito()
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');

                this.btnFacturaE.Visible = false;
                this.btnAgregar.Visible = true;
                this.btnAgregarRemitir.Visible = true;

                if (cliente[1].Contains("Responsable Inscripto"))
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito A");
                    this.labelNroFactura.Text = "Nota de Credito A N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    if (cliente[1].Contains("Cliente del Exterior") || cliente[1].Contains("IVA Liberado"))
                    {
                        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        //como estoy en cotizacion pido el ultimo numero de este documento
                        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito E");
                        this.labelNroFactura.Text = "Nota de Credito E N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        if (pv.formaFacturar == "Electronica")
                        {
                            this.btnFacturaE.Visible = true;
                            this.btnAgregar.Visible = false;
                            this.btnAgregarRemitir.Visible = false;
                            this.validarIdImpositivoCliente();
                        }
                    }
                    else
                    {
                        if (cliente[1].Contains("_"))
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //como estoy en cotizacion pido el ultimo numero de este documento
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito PRP");
                            this.labelNroFactura.Text = "Nota de Credito PRP N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        }
                        else
                        {
                            if (c.monotributo == "1")
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito C");
                                this.labelNroFactura.Text = "Nota de Credito C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            }
                            else
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                                this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            }
                        }
                    }

                }
            }
            catch
            {

            }
        }

        private void obtenerNroNotaDebito()
        {
            try
            {
                this.btnFacturaE.Visible = false;
                this.btnAgregar.Visible = true;
                this.btnAgregarRemitir.Visible = true;

                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente[1].Contains("Responsable Inscripto"))
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito A");
                    this.labelNroFactura.Text = "Nota de Debito A N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    if (cliente[1].Contains("Cliente del Exterior") || cliente[1].Contains("IVA Liberado"))
                    {
                        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        //como estoy en cotizacion pido el ultimo numero de este documento
                        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito E");
                        this.labelNroFactura.Text = "Nota de Debito E N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        if (pv.formaFacturar == "Electronica")
                        {
                            this.btnFacturaE.Visible = true;
                            this.btnAgregar.Visible = false;
                            this.btnAgregarRemitir.Visible = false;
                            this.validarIdImpositivoCliente();
                        }
                    }
                    else
                    {
                        if (cliente[1].Contains("_"))
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //como estoy en cotizacion pido el ultimo numero de este documento
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota Debito Presupuesto");
                            this.labelNroFactura.Text = "Nota Debito Presupuesto N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        }
                        else
                        {
                            if (c.monotributo == "1")
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito C");
                                this.labelNroFactura.Text = "Nota de Debito C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            }
                            else
                            {
                                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                //como estoy en cotizacion pido el ultimo numero de este documento
                                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito B");
                                this.labelNroFactura.Text = "Nota de Debito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            }
                        }
                    }

                }
            }
            catch
            {

            }
        }
        private void obtenerNroNotaDebitoPresupuesto()
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota Debito Presupuesto");
                this.labelNroFactura.Text = "Nota Debito Presupuesto N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }

        private void obtenerNroFacturaInicio()
        {
            try
            {
                if (c.monotributo == "1")
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura C");
                    this.labelNroFactura.Text = "Factura C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura B");
                    this.labelNroFactura.Text = "Factura B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }

        private void obtenerNroNotaCreditoInicio()
        {
            try
            {
                if (c.monotributo == "1")
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito C");
                    this.labelNroFactura.Text = "Nota de Credito C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                    this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Nota de Credito. " + ex.Message));
            }
        }

        private void obtenerNroNotaDebitoInicio()
        {
            try
            {
                if (c.monotributo == "1")
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito C");
                    this.labelNroFactura.Text = "Nota de Debito C N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito B");
                    this.labelNroFactura.Text = "Nota de Debito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Nota de Debito. " + ex.Message));
            }
        }

        private void obtenerNroPresupuesto()
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Presupuesto");
                this.labelNroFactura.Text = "Presupuesto N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }

        private void obtenerNroNotaCreditoPresupuesto()
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito PRP");
                this.labelNroFactura.Text = "Nota de Credito PRP N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Factura. " + ex.Message));
            }
        }


        private TipoDocumento cargarTiposFactura(string tipo)
        {
            try
            {
                TipoDocumento ti = null;
                string tipos = "";
                switch (tipo)
                {
                    case "Presupuesto N":
                        ti = controlador.obtenerTipoDoc("Presupuesto");
                        //tipos = tp.id.ToString() + ";2";
                        break;
                    case "Factura A N":
                        ti = controlador.obtenerTipoDoc("Factura A");
                        //tipos = tp.id.ToString() + ";1";
                        break;
                    case "Factura B N":
                        ti = controlador.obtenerTipoDoc("Factura B");
                        //tipos = tp.id.ToString() + ";1";
                        break;
                    case "Factura C N":
                        ti = controlador.obtenerTipoDoc("Factura C");
                        //tipos = tp.id.ToString() + ";1";
                        break;
                    case "Factura E N":
                        ti = controlador.obtenerTipoDoc("Factura E");
                        //tipos = tp.id.ToString() + ";1";
                        break;
                    case "Nota de Credito A N":
                        ti = controlador.obtenerTipoDoc("Nota de Credito A");
                        //tipos = tp.id.ToString() + ";3";
                        break;
                    case "Nota de Credito B N":
                        ti = controlador.obtenerTipoDoc("Nota de Credito B");
                        //tipos = tp.id.ToString() + ";3";
                        break;
                    case "Nota de Credito C N":
                        ti = controlador.obtenerTipoDoc("Nota de Credito C");
                        //tipos = tp.id.ToString() + ";3";
                        break;
                    case "Nota de Credito E N":
                        ti = controlador.obtenerTipoDoc("Nota de Credito E");
                        //tipos = tp.id.ToString() + ";3";
                        break;
                    case "Nota de Credito PRP N":
                        ti = controlador.obtenerTipoDoc("Nota de Credito PRP");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                    case "Nota de Debito A N":
                        ti = controlador.obtenerTipoDoc("Nota de Debito A");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                    case "Nota de Debito B N":
                        ti = controlador.obtenerTipoDoc("Nota de Debito B");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                    case "Nota de Debito C N":
                        ti = controlador.obtenerTipoDoc("Nota de Debito C");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                    case "Nota de Debito E N":
                        ti = controlador.obtenerTipoDoc("Nota de Debito E");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                    case "Nota Debito Presupuesto N":
                        ti = controlador.obtenerTipoDoc("Nota Debito Presupuesto");
                        //tipos = tp.id.ToString() + ";4";
                        break;
                }

                return ti;
            }
            catch
            {
                return null;
            }
        }

        private void InicializarListaPagos()
        {
            try
            {
                lstPagosTemp.Columns.Add("Tipo Pago");
                lstPagosTemp.Columns.Add("Importe");
                lstPagosTemp.Columns.Add("Neto");
                lstPagosTemp.Columns.Add("Recargo");
                lstPago = lstPagosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        private void cargarPaisesExportacion()
        {
            try
            {
                DataTable dt = this.controlador.obtenerPaisesExportacion();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["codigo"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListPais.DataSource = dt;
                this.DropListPais.DataValueField = "codigo";
                this.DropListPais.DataTextField = "descripcion";

                this.DropListPais.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando paises exportacion. " + ex.Message));
            }
        }
        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //this.cargarCliente(this.txtBusquedaCliente.Text);
        }

        protected void btnBuscarProducto_Click(object sender, EventArgs e)
        {

            this.cargarProducto(this.txtCodigo.Text);
        }

        private void cargarProducto(string codigo)
        {
            try
            {
                contArticulo = new controladorArticulo();
                ControladorArticulosEntity contEnt = new ControladorArticulosEntity();
                Configuracion config = new Configuracion();
                Articulo art = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));

                //Limpio campo de Descuento Arriba
                this.TxtDescuentoArri.Text = "0";

                if (art != null)
                {
                    Cliente prov = this.contCliente.obtenerProveedoresRazonSocial(art.proveedor.razonSocial);
                    if (prov != null)
                    {
                        if (prov.iva.Contains("No Informa"))
                        {
                            if (!this.labelNroFactura.Text.Contains("Presupuesto") && !this.labelNroFactura.Text.Contains("PRP"))
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No puede hacer FACTURA de este articulo ya que el proveedor no informa iva."));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!this.labelNroFactura.Text.Contains("Presupuesto") && !this.labelNroFactura.Text.Contains("PRP"))
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El articulo no contiene proveedor asignado. Para realizar FACTURA revise el articulo primero."));
                            return;
                        }
                    }

                    decimal cantPromo = 0;
                    if (this.txtCantidad.Text != "")
                        cantPromo = Convert.ToDecimal(this.txtCantidad.Text);
                    Gestion_Api.Entitys.Promocione p = contEnt.obtenerPromocionValidaArticulo(art.id, Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.DropListFormaPago.SelectedValue), Convert.ToInt32(this.DropListLista.SelectedValue), Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")), cantPromo);
                    if (p != null)
                    {
                        if (p.PrecioFijo > 0)
                        {
                            art.precioVenta = p.PrecioFijo.Value;
                            this.txtPUnitario.Attributes.Add("disabled", "disabled");
                        }
                        else
                        {
                            this.TxtDescuentoArri.Text = p.Descuento.ToString();
                            this.TxtDescuentoArri.Attributes.Add("disabled", "disabled");
                        }
                    }
                    else
                    {
                        var artEnt = contEnt.obtenerOfertaArticuloParaFacturar(art.id);
                        if (artEnt != null)
                        {
                            if (artEnt.Oferta > 1 && DateTime.Now >= artEnt.Desde && DateTime.Now <= artEnt.Hasta)
                            {
                                if (Session["FacturasABM_ArticuloModal"] != null)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo en oferta vigente. ", null));
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Articulo en oferta vigente. \", {type: \"info\"});", true);
                                }
                                this.txtPUnitario.Attributes.Add("disabled", "disabled");
                                this.TxtDescuentoArri.Attributes.Add("disabled", "disabled");
                            }
                        }
                    }
                    this.verificarMedidasVenta(art.id);
                    //agrego los txt
                    this.txtDescripcion.Text = art.descripcion;

                    this.verificarStockMinimo();
                    //TODO
                    if (this.labelNroFactura.Text.Contains("Factura E") || this.labelNroFactura.Text.Contains("Nota de Credito E") || this.labelNroFactura.Text.Contains("Nota de Debito E"))
                    {
                        this.txtIva.Text = 0 + "%";
                        art.precioVenta = (art.precioVenta / ((1 + (art.porcentajeIva / 100))));
                        this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();
                    }
                    else
                    {
                        this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                        this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();

                    }

                    if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["PrecioFacturaA"]) && WebConfigurationManager.AppSettings["PrecioFacturaA"] == "1")
                    {
                        if (this.labelNroFactura.Text.Contains("Factura A") || this.labelNroFactura.Text.Contains("Nota de Credito A") || this.labelNroFactura.Text.Contains("Nota de Debito A"))
                        {
                            this.txtPUnitario.Text = decimal.Round((art.precioVenta / ((1 + (art.porcentajeIva / 100)))), 2).ToString();
                        }
                    }

                    this.verificarAlertaArticulo(art);

                    if (config.infoImportacionFacturas == "1")
                    {
                        //si tiene datos de despacho se los cargo
                        this.agregarInfoDespachoItem(art);
                    }

                    //this.txtPUnitario.Text = decimal.Round(art.precioVenta,4).ToString();
                    Session["FacturasABM_ArticuloModal"] = null;
                    this.txtPorcDescuento.Focus();
                    this.txtCantidad.Focus();
                    //recalculo total
                    this.totalItem();

                    Factura f = Session["Factura"] as Factura;
                    this.txtRenglon.Text = (f.items.Count + 1).ToString();

                    //Si tiene configuracion CON COMMITANTE
                    //agrego automaticamente el articulo a la FC con la cant y dto que este escrito

                    if (config.commitante == "1")
                    {
                        if (!String.IsNullOrEmpty(this.txtCodigo.Text))
                        {
                            if (String.IsNullOrEmpty(this.txtCantidad.Text))
                            {
                                this.txtCantidad.Text = "1";
                            }
                            if (String.IsNullOrEmpty(this.TxtDescuentoArri.Text))
                            {
                                this.TxtDescuentoArri.Text = "0";
                            }
                            this.cargarProductoAFactura(0);
                            this.txtCodigo.Text = "";
                        }
                    }

                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra Articulo " + this.txtCodigo.Text));
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encuentra Articulo. \");", true);
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Articulo. " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error buscando articulo." + ex.Message + " \", {type: \"error\"});", true);
            }
        }

        private void borrarCamposagregarItem()
        {
            try
            {
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
                this.txtDescripcion.Text = "";
                this.txtIva.Text = "";
                this.TxtDescuentoArri.Text = "0";
                this.txtPUnitario.Text = "";
                this.txtTotalArri.Text = "0";
                this.lbtnStockProd.Text = "0";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void cargarItems()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                //limpio el place holder y lo vuelvo a cargar
                this.phArticulos.Controls.Clear();
                int pos = 0;

                //Obtengo la Key de Criterio de Ordenamiento de Articulos en Facturacion
                string ordenar = WebConfigurationManager.AppSettings.Get("CriterioOrdenarArticulosFacturacion");

                //Si viene de Pedidos y existe criterio de ordenamiento en el web.config, ordeno los articulos segun ese criterio
                if (this.accion == 5 && !string.IsNullOrEmpty(ordenar) && ordenar == "1")
                    f.items = f.items.OrderBy(x => x.articulo.ubicacion).ToList();

                foreach (ItemFactura item in f.items)
                {
                    pos = f.items.IndexOf(item);
                    this.agregarItemFactura(item, pos);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando items en facrura. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga el item en la tabla items
        /// </summary>
        private void agregarItemFactura(ItemFactura item, int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = item.articulo.codigo.ToString() + pos;

                //Celdas

                TableCell celCodigo = new TableCell();
                //celCodigo.Text = (pos+1) + " - " + item.articulo.codigo;
                if (item.nroRenglon > 0)
                    celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                else
                    celCodigo.Text = (pos + 1) + " - " + item.articulo.codigo;

                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                TextBox txtCant = new TextBox();
                txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtCant.ID = "Text_" + pos.ToString() + "_" + item.cantidad;
                txtCant.CssClass = "form-control";
                txtCant.Style.Add("text-align", "Right");
                //txtCant.TextMode = TextBoxMode.Number;
                txtCant.Text = item.cantidad.ToString();
                txtCant.TextChanged += new EventHandler(ActualizarTotalPH);
                txtCant.AutoPostBack = true;
                celCantidad.Controls.Add(txtCant);
                celCantidad.Width = Unit.Percentage(10);
                tr.Cells.Add(celCantidad);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.articulo.descripcion;
                celDescripcion.Width = Unit.Percentage(35);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + item.precioUnitario.ToString();
                celPrecio.Width = Unit.Percentage(10);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = "$" + item.descuento.ToString();
                celDescuento.Width = Unit.Percentage(10);
                celDescuento.VerticalAlign = VerticalAlign.Middle;
                celDescuento.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDescuento);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + item.total.ToString();
                celTotal.Width = Unit.Percentage(10);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);
                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + item.articulo.codigo + "_" + pos;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnEliminar, null) + ";");
                //btnEliminar.Click += new EventHandler(this.QuitarItem);
                celAccion.Controls.Add(btnEliminar);

                int trazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                if (trazable > 0)
                {
                    int cargada = this.validarTrazaCargadaItemFactura(item);
                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAccion.Controls.Add(l2);

                    LinkButton btnTraza = new LinkButton();
                    if (accion == 5)//viene de un pedido
                    {
                        btnTraza.Enabled = false;
                        btnTraza.CssClass = "btn btn-default";
                    }
                    else
                    {
                        if (cargada > 0)
                        {
                            btnTraza.CssClass = "btn btn-info";
                        }
                        else
                        {
                            btnTraza.CssClass = "btn btn-danger";
                        }
                    }
                    btnTraza.ID = "btnTraza_" + item.articulo.id + "_" + pos;
                    btnTraza.Text = "<span class='shortcut-icon icon-road'></span>";
                    if (item.cantidad < 0)
                        btnTraza.Click += new EventHandler(this.CargarTrazabilidadItem);
                    else
                        btnTraza.Click += new EventHandler(this.TrazabilidadItem);
                    celAccion.Controls.Add(btnTraza);

                }
                else
                {
                    ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                    int datosExtra = contArtEnt.obtenerSiDatosExtra(item.articulo.id);
                    if (datosExtra > 0)
                    {
                        Literal l2 = new Literal();
                        l2.Text = "&nbsp";
                        celAccion.Controls.Add(l2);

                        LinkButton btnDatosExtras = new LinkButton();
                        int cargado = this.verificarDatosExtrasCargados(item);
                        if (cargado > 0)
                        {
                            btnDatosExtras.CssClass = "btn btn-info";
                        }
                        else
                        {
                            btnDatosExtras.CssClass = "btn btn-danger";
                        }
                        btnDatosExtras.ID = "btnDatosExtras_" + item.articulo.id + "_" + pos;
                        btnDatosExtras.Text = "<span class='shortcut-icon fa fa-info-circle'></span>";
                        btnDatosExtras.Click += new EventHandler(this.DatosExtrasItem);
                        celAccion.Controls.Add(btnDatosExtras);
                    }
                }
                if (this.accion == 9)
                {
                    string permisos = Session["Login_Permisos"] as string;
                    string[] listPermisos = permisos.Split(';');
                    string permisoDesc = listPermisos.Where(x => x == "80").FirstOrDefault();

                    if (!String.IsNullOrEmpty(permisoDesc))
                    {
                        Literal l4 = new Literal();
                        l4.Text = "&nbsp";
                        celAccion.Controls.Add(l4);

                        LinkButton btnModificarDesc = new LinkButton();
                        btnModificarDesc.CssClass = "btn btn-info";
                        btnModificarDesc.ID = "btnModificarDesc_" + item.articulo.id + "_" + pos;
                        btnModificarDesc.Text = "<span class='shortcut-icon icon-pencil'></span>";
                        btnModificarDesc.Click += new EventHandler(this.EditarDescItemRefacturar);
                        celAccion.Controls.Add(btnModificarDesc);
                    }
                }

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phArticulos.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }
        private void actualizarTotales()
        {
            try
            {
                this.factura = Session["Factura"] as Factura;

                //documento
                string[] lbl = this.labelNroFactura.Text.Split('°');
                this.factura.tipo = this.cargarTiposFactura(lbl[0]);

                //impuestos vta de combustible
                decimal impuestosCombustible = 0;
                this.factura.provCombustibles = Convert.ToInt32(this.ListProveedorCombustible.SelectedValue);
                if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0)
                {
                    impuestosCombustible = this.obtenerTotalImpuestosCombustibles(this.factura);
                    this.actualizarTotalesVentaCombustible();

                }
                else
                {
                    //obtengo total de suma de item
                    decimal totalC = this.factura.obtenerTotalNeto();
                    decimal total = decimal.Round(totalC, 2, MidpointRounding.AwayFromZero);
                    this.factura.neto = total;

                    if (this.accion == 6 || this.accion == 9)// si viene de generar nota de credito mantengo el descuento que le habia hecho a la factura
                    {
                        this.txtPorcDescuento.Text = decimal.Round(this.factura.neto10, 2).ToString();
                    }

                    //Subtotal = neto menos el descuento
                    this.factura.descuento = decimal.Round((this.factura.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100)), 2, MidpointRounding.AwayFromZero);
                    this.factura.subTotal = decimal.Round((this.factura.neto - this.factura.descuento), 2, MidpointRounding.AwayFromZero);

                    //del subtotal obtengo iva
                    //this.factura.neto21 = (this.factura.subTotal * Convert.ToDecimal(0.21));
                    //decimal totalIva = this.factura.obtenerTotalIva();

                    if (lbl[0].Contains("Presupuesto") || lbl[0].Contains("PRP"))
                    {
                        Configuracion c = new Configuracion();

                        if (c.ivaArticulosPresupuesto == "1")
                        {
                            this.factura.neto21 = decimal.Round((this.factura.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100), 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            decimal iva = decimal.Round(this.factura.obtenerTotalIva(), 2);
                            decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
                            //AL DESCUENTO DE LA FACTURA LE AGREGO EL DESC DEL IVA
                            this.factura.descuento += descuento; decimal.Round(descuento, 2, MidpointRounding.AwayFromZero);
                            //*/
                            this.factura.neto21 = decimal.Round((iva - decimal.Round(descuento, 2)), 2, MidpointRounding.AwayFromZero);
                        }

                    }
                    else
                    {
                        if (lbl[0].Contains("Factura E") || lbl[0].Contains("Credito E") || lbl[0].Contains("Debito E"))
                        {
                            this.factura.neto21 = 0;
                        }
                        else
                        {
                            decimal iva = decimal.Round(this.factura.obtenerTotalIva(), 2);
                            decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);

                            //SI ES 'B' AL DESCUENTO DE LA FACTURA LE AGREGO EL DESC DEL IVA
                            if (lbl[0].Contains("Factura B") || lbl[0].Contains("Credito B") || lbl[0].Contains("Debito B"))
                            {
                                this.factura.descuento += descuento; decimal.Round(descuento, 2, MidpointRounding.AwayFromZero);
                            }

                            this.factura.neto21 = decimal.Round((iva - decimal.Round(descuento, 2)), 2, MidpointRounding.AwayFromZero);
                        }
                    }

                    this.factura.totalSinDescuento = decimal.Round(this.factura.neto + this.factura.obtenerTotalIva(), 2);

                    //retencion sobre el sub total
                    this.factura.retencion = decimal.Round((this.factura.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100)), 2, MidpointRounding.AwayFromZero);

                    //percepcion iva == checked
                    if (this.chkIvaNoInformado.Checked == true)
                    {
                        this.factura.iva10 = decimal.Round(this.factura.neto * Convert.ToDecimal(0.105), 2);//iva 10.5%
                    }
                    else
                    {
                        this.factura.iva10 = 0;
                    }

                    this.factura.total = decimal.Round((this.factura.subTotal + this.factura.neto21 + this.factura.iva10 + this.factura.retencion), 2, MidpointRounding.AwayFromZero);

                }

                this.txtTotalITC.Text = this.factura.totalITC.ToString();
                this.txtTotalTasaHidrica.Text = this.factura.totalHidrica.ToString();
                this.txtTotalTasaVial.Text = this.factura.totalVial.ToString();
                this.txtTotalTasaMunicipal.Text = this.factura.totalMunicipal.ToString();
                this.txtTotalImpuestos.Text = impuestosCombustible.ToString();

                //total: subtotal + iva + retencion + ivaPercepcion 
                //impuestosCombustible
                //if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0)
                //{
                //    this.factura.total = decimal.Round((this.factura.subTotal + this.factura.neto21 + this.factura.iva10 + this.factura.iva21 + this.factura.retencion), 2, MidpointRounding.AwayFromZero);
                //}
                //else
                //{
                //    //this.factura.total = decimal.Round((this.factura.subTotal + this.factura.neto21 + this.factura.iva10 + this.factura.retencion), 2, MidpointRounding.AwayFromZero);
                //}
                //cargo en pantalla

                string neto = decimal.Round(this.factura.neto, 2).ToString();
                //this.txtNeto.Text = decimal.Round(this.factura.neto, 2).ToString();
                this.txtNeto.Text = neto;

                this.txtDescuento.Text = decimal.Round(this.factura.descuento, 2).ToString();

                this.txtsubTotal.Text = decimal.Round(this.factura.subTotal, 2).ToString();

                this.txtIvaTotal.Text = decimal.Round(this.factura.neto21, 2).ToString();

                this.txtPercepcionCF.Text = decimal.Round(this.factura.iva10, 2).ToString();

                this.txtRetencion.Text = decimal.Round(this.factura.retencion, 2).ToString();

                //string Stotal = .ToString();
                this.txtTotal.Text = decimal.Round(this.factura.total, 2).ToString();
                //this.txtImporteEfectivo.Text = decimal.Round(this.factura.total, 2).ToString();
                //this.txtImporteT.Text = decimal.Round(this.factura.total, 2).ToString();

                //this.lblSaldoTarjeta.Text = decimal.Round(this.factura.total, 2).ToString();
                this.txtImporteFinanciar.Text = decimal.Round(this.factura.total, 2).ToString();

                try
                {
                    if (!String.IsNullOrEmpty(this.txtImporteFinanciar.Text) && !String.IsNullOrEmpty(this.txtAnticipo.Text))
                    {
                        decimal totalFC = Convert.ToDecimal(this.txtImporteFinanciar.Text);
                        decimal anticipo = Convert.ToDecimal(this.txtAnticipo.Text);

                        this.txtFinanciado.Text = (totalFC - anticipo).ToString();
                    }
                }
                catch { }
                Factura f = this.factura;
                Session.Add("Factura", f);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));

            }
        }
        /// <summary>
        /// cuando hace clic en guardar y se genera la factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    //Verifico si tiene la alerta de precios de articulos sin actualizar
                    if (!this.verificarArticulosSinActualizar())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Existen artículos cuyos precios no se actualizan hace mas de " + this.configuracion.AlertaArticulosSinActualizar + " dias. \");", true);
                        return;
                    }

                    //Verifico si coinciden los saldos de la factura en caso de que la forma de pago sea mutual
                    if (!this.validarSaldoMutual())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El monto final ingresado en la forma de pago es diferente al total de la factura. \");", true);
                        return;
                    }

                    //Verifico en caso de que sea Nota de Crédito de una Factura con Forma de Pago Mutual, que haya eliminado los cobros asociados
                    if (!this.verificarAnularFcMutual())
                    {
                        //Si existen cobros pendientes, retorno. El mensaje lo muestro en el método.
                        return;
                    }

                    //Verifico en caso de que sea Nota de Crédito que ya no se hayan realizado Notas de Crédito sobre la factura seleccionada
                    if (!validarNotaCreditoFactura())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Ya se han realizado Notas de Crédito sobre las Facturas seleccionadas. \");", true);
                        return;
                    }

                    //Verifico, si tiene la opción de combustibles, valido que si está facturando combustibles, que todos los items de la factura sean combustibles
                    string combustible = WebConfigurationManager.AppSettings.Get("Combustible");
                    if (!string.IsNullOrEmpty(combustible) && combustible == "1")
                    {
                        int itemsCombustible = this.validarItemsCombustible();
                        if (itemsCombustible < 0)
                        {
                            if (itemsCombustible == -1)
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Error validando items de la factura. \");", true);

                            if (itemsCombustible == -2)
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Los items de la factura deben ser todos del grupo de combustibles. \");", true);

                            if (itemsCombustible == -3)
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se calcularon los impuestos de combustibles. \");", true);

                            return;
                        }
                    }

                    if (this.DropListFormaPago.SelectedItem.Text == "Tarjeta")
                    {

                        int i = validarSaldoTarjeta();
                        if (i == 1)
                        {
                            int j = validarUltimaFactura();
                            if (j == 1)
                            {
                                int t = this.validarTrazasCargadas();
                                if (t == 1)
                                {
                                    int iec = this.validarItemsEnCero();
                                    if (iec == 1)
                                    {
                                        this.generarFactura(0);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                    }

                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se seleccionaron las trazabilidades de los articulos a vender. \", {type: \"error\"});", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Verifique la fecha de la factura ingresada \", {type: \"error\"});", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El monto ingresado en la forma de pago no es igual al monto total de la factura. \", {type: \"error\"});", true);
                            //m.mensajeBoxError("El monto ingresado en la forma de pago no es igual al monto total de la factura.");
                        }
                    }
                    else
                    {
                        int j = validarUltimaFactura();//solo para preimpresa
                        if (j == 1)
                        {
                            int t = this.validarTrazasCargadas();
                            if (t == 1)
                            {
                                int iec = this.validarItemsEnCero();
                                if (iec == 1)
                                {
                                    this.generarFactura(0);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se seleccionaron las trazabilidades de los articulos a vender. \", {type: \"error\"});", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Verifique la fecha de la factura ingresada \", {type: \"error\"});", true);
                            this.txtFecha.Attributes.Remove("Disabled");//Si esta mal la fecha que ingreso antes lo vuelvo a habilitar para que corriga
                        }
                    }
                }
                else
                {
                    this.btnAgregar.Attributes.Remove("Disabled");
                    this.btnAgregarRemitir.Attributes.Remove("Disabled");
                }

            }
            catch
            {

            }

        }
        protected void btnAgregarFactE_Click(object sender, EventArgs e)
        {
            try
            {
                Factura factE = Session["Factura"] as Factura;
                PuntoVenta pv = this.cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                if (pv.formaFacturar == "Electronica")
                {
                    if (this.txtPermisoEmbarque.Text.Length >= 16 || String.IsNullOrEmpty(this.txtPermisoEmbarque.Text))
                    {
                        factE.facturaExportacion.paisDestino = Convert.ToInt32(this.DropListPais.SelectedValue);
                        factE.facturaExportacion.incoterms = this.DropListIncoterms.SelectedItem.Text;

                        factE.facturaExportacion.permisoEmbarque = this.txtPermisoEmbarque.Text;
                        Session["Factura"] = factE;//agrego los datos de la exportacion.

                        this.generarFactura(0);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Permiso de embarque tiene que contener 16 o más caracteres o estar vacio. \", {type: \"error\"});", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Permiso de embarque tiene que contener 16 o más caracteres o estar vacio."));
                    }
                }
                else
                {
                    this.generarFactura(0);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void btnRefacturar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    Factura fact = Session["Factura"] as Factura;

                    int ok = this.controlador.verificarRefacturarProveedor(fact);
                    if (ok < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede refacturar porque uno o más articulos tienen proveedor con condicion IVA NO INFORMA. \", {type: \"error\"});", true);
                        return;
                    }
                    int fechaOK = validarUltimaFactura();
                    if (fechaOK <= 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Verifique la fecha de la factura ingresada. \", {type: \"error\"});", true);
                        return;
                    }
                    int verificaTotalCero = this.validarFacturarTotalCero(fact);
                    if (verificaTotalCero < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar en monto cero. \", {type: \"error\"});", true);
                        return;
                    }

                    if (fact.items.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Debe agregar articulos a la factura. \", {type: \"error\"});", true);
                    }

                    fact.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                    fact.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    fact.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));

                    string[] lbl = this.labelNroFactura.Text.Split('°');
                    fact.tipo = this.cargarTiposFactura(lbl[0]);

                    if (fact.tipo.tipo.Contains("Presupuesto"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede refacturar en TIPO Presupuesto. \");", true);
                    }

                    if (this.flag_cambioFecha == 1)
                    {
                        //seteo la fecha de la factura igual a la que eligio y con la hora actual.
                        DateTime fechaModificada = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                        fact.fecha = fechaModificada.AddHours(DateTime.Now.TimeOfDay.Hours);
                    }
                    else
                    {
                        fact.fecha = DateTime.Now;
                    }

                    fact.formaPAgo = controlador.obtenerFormaPagoFP(this.DropListFormaPago.SelectedItem.Text);
                    //agrego el porcentaje de descuento
                    fact.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);
                    //obtengo el Neto no gravado || de los items con alicuota 0%
                    fact.iva21 = fact.obtenerNetoNoGravado();

                    fact.comentario = this.txtComentarios.Text;

                    int user = (int)Session["Login_IdUser"];
                    string presupuestos = Request.QueryString["prps"];

                    int i = this.controlador.ProcesoRefacturarPRPEditado(null,fact, user, presupuestos);
                    if (i > 0)
                    {
                        //despues de refacturar establezco el iva de nuevo en NO informa
                        this.restablecerIvaCliente();
                        //mando imprimir factura
                        string imprimir = WebConfigurationManager.AppSettings.Get("Imprime");
                        if (imprimir == "1")
                        {
                            //imprimo factura
                            this.ImprimirFactura(fact.id, fact.tipo.id, 0);
                        }
                        else
                        {
                            Session["Factura"] = null;
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturasLargo.aspx';", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo generar factura. \", {type: \"error\"});", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnAgregarRemitir_Click(object sender, EventArgs e)
        {

            if (IsValid)
            {
                //Verifico si tiene la alerta de precios de articulos sin actualizar
                if (!this.verificarArticulosSinActualizar())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Existen artículos cuyos precios no se actualizan hace mas de " + this.configuracion.AlertaArticulosSinActualizar + " dias. \");", true);
                    return;
                }

                //Verifico si coinciden los saldos de la factura en caso de que la forma de pago sea mutual
                if (!this.validarSaldoMutual())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El monto final ingresado en la forma de pago es diferente al total de la factura. \");", true);
                    return;
                }

                //Verifico en caso de que sea Nota de Crédito que ya no se hayan realizado Notas de Crédito sobre la factura seleccionada
                if (!validarNotaCreditoFactura())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Ya se han realizado Notas de Crédito sobre las Facturas seleccionadas. \");", true);
                    return;
                }

                //Verifico, si tiene la opción de combustibles, valido que si está facturando combustibles, que todos los items de la factura sean combustibles
                string combustible = WebConfigurationManager.AppSettings.Get("Combustible");
                if (!string.IsNullOrEmpty(combustible) && combustible == "1")
                {
                    int itemsCombustible = this.validarItemsCombustible();
                    if (itemsCombustible < 0)
                    {
                        if (itemsCombustible == -1)
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Error validando items de la factura. \");", true);

                        if (itemsCombustible == -2)
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Los items de la factura deben ser todos del grupo de combustibles. \");", true);

                        if (itemsCombustible == -3)
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se calcularon los impuestos de combustibles. \");", true);

                        return;
                    }
                }

                if (this.DropListFormaPago.SelectedItem.Text == "Tarjeta")
                {

                    int i = validarSaldoTarjeta();
                    if (i == 1)
                    {
                        int j = validarUltimaFactura();
                        if (j == 1)
                        {
                            int t = this.validarTrazasCargadas();
                            if (t == 1)
                            {
                                int iec = this.validarItemsEnCero();
                                if (iec == 1)
                                {
                                    this.generarFactura(1);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                }
                            }
                            else
                            {
                                if (t == -1)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se seleccionaron las trazabilidades de los articulos a vender. \", {type: \"error\"});", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se tiene suficientes trazabilidades para la cantidad a vender. \");", true);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Verifique la fecha de la factura ingresada \", {type: \"error\"});", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El monto ingresado en la forma de pago no es igual al monto total de la factura. \", {type: \"error\"});", true);
                        //m.mensajeBoxError("El monto ingresado en la forma de pago no es igual al monto total de la factura.");
                    }
                }
                else
                {
                    int j = validarUltimaFactura();
                    if (j == 1)
                    {
                        int t = this.validarTrazasCargadas();
                        if (t == 1)
                        {
                            int iec = this.validarItemsEnCero();
                            if (iec == 1)
                            {
                                this.generarFactura(1);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se seleccionaron las trazabilidades de los articulos a vender. \", {type: \"error\"});", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Verifique la fecha de la factura ingresada \", {type: \"error\"});", true);
                        this.txtFecha.Attributes.Remove("Disabled");//Si esta mal la fecha que ingreso antes lo vuelvo a habilitar para que corriga
                    }
                }
            }

        }
        private void generarFactura(int generaRemito)
        {
            try
            {
                //Obtengo items
                Factura fact = Session["Factura"] as Factura;

                //agrego info traza a los items
                fact.items = this.agregarInfoTrazaFactura(fact);

                //valido que si esta facturando con lista de precio al 100% dto

                int verificaTotalCero = this.validarFacturarTotalCero(fact);
                if (verificaTotalCero < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar en monto cero. \", {type: \"error\"});", true);
                    return;
                }

                if (fact.items.Count > 0)
                {

                    int datosExtras = this.validarDatosExtrasCargadosFactura(fact);
                    if (datosExtras < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se cargaron todos los Datos extras de los items. \");", true);
                        return;
                    }

                    fact.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                    fact.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    fact.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));

                    int validarDto = this.validarDescuentoFactura();
                    if (validarDto < 0)
                    {
                        this.txtPorcDescuento.Text = "0";
                        this.lstPago.Clear();
                        this.actualizarTotales();
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al permitido. \", {type: \"error\"});", true);
                        return;
                    }
                    if (this.flag_cambioFecha == 1)
                    {
                        //seteo la fecha de la factura igual a la que eligio y con la hora actual.
                        DateTime fechaModificada = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                        fact.fecha = fechaModificada.AddHours(DateTime.Now.TimeOfDay.Hours);
                    }
                    else
                    {
                        fact.fecha = DateTime.Now;
                        //fact.fecha = new DateTime(2017,11,30);
                    }
                    if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0)
                    {
                        this.agregarDatosCombustibleAComentarios(fact);
                    }
                    fact.vendedor.id = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                    fact.comentario = this.txtComentarios.Text;
                    if (this.chkIvaNoInformado.Checked == true)
                    {
                        fact.comentario += " - Percepcion IVA a Consumidor Final ($" + this.factura.iva10 + ").";
                    }
                    fact.fechaEntrega = this.txtFechaEntrega.Text;
                    fact.horaEntrega = this.txtHorarioEntrega.Text;
                    fact.bultosEntrega = this.txtBultosEntrega.Text;

                    //agrego el porcentaje de descuento
                    fact.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);
                    fact.formaPAgo = controlador.obtenerFormaPagoFP(this.DropListFormaPago.SelectedItem.Text);
                    fact.listaP.id = Convert.ToInt32(this.DropListLista.SelectedValue);
                    string[] lbl = this.labelNroFactura.Text.Split('°');
                    fact.tipo = this.cargarTiposFactura(lbl[0]);

                    #region forma pago
                    DataTable dtPago = lstPago;
                    if (fact.formaPAgo.forma == "Tarjeta")
                    {
                        if (dtPago.Rows.Count <= 0 && this.accion != 6)//si es generacion de nota de cred desde factura anterior
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se agregaron pagos con tarjeta \", {type: \"error\"});", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se agregaron pagos con tarjeta"));
                            return;
                        }
                    }
                    if (fact.formaPAgo.forma == "Credito")
                    {
                        if (String.IsNullOrEmpty(fact.NroSolicitud) && this.accion != 6)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se cargo solicitud del credito!. \");", true);
                            return;
                        }
                        int okAnticipo = this.validarCobroAnticipo();
                        if (okAnticipo <= 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se genero el cobro del anticipo!. \");", true);
                            return;
                        }

                    }
                    if (fact.formaPAgo.forma == "Mutuales")
                    {
                        if (!fact.tipo.tipo.Contains("Credito PRP"))
                        {
                            if (fact.pagare == null)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se cargo pago mutual!. \");", true);
                                return;
                            }

                            int okAnticipoMutual = this.validarAnticipoMutual();
                            if (okAnticipoMutual < 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se genero el cobro del anticipo!. \");", true);
                                return;
                            }

                            decimal importePagare = Convert.ToDecimal(this.lblTotalRecargoMutualFinal.Text);
                            fact.pagare.Importe = decimal.Round(importePagare / fact.cantCuotasPagare, 2);
                            fact.totalAnticipo = Convert.ToDecimal(this.lblTotalAnticipoMutualFinanciado.Text);
                            //fact.pagare.Importe = decimal.Round(fact.total / fact.cantCuotasPagare, 2);
                        }
                    }
                    #endregion

                    Configuracion c = new Configuracion();
                    if (c.consumidorFinalCC != "1")
                    {
                        if (fact.cliente.iva == "Consumidor Final" && fact.formaPAgo.forma.Contains("Cuenta Corriente"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar en cta. cte. a consumidor final. \", {type: \"error\"});", true);
                            return;
                        }
                    }
                    //obtengo si tengo que facturar a otra sucursal
                    //int sucursalFacturar = 0;
                    if (this.ListSucursalCliente.Visible == true)
                    {
                        fact.sucursalFacturada = Convert.ToInt32(this.ListSucursalCliente.SelectedValue);
                    }

                    int fiscalConsumidor = this.validarTotalConsumidorFinal(fact);
                    if (fiscalConsumidor < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar mas de $1000 a Consumidor Final desde Punto de venta tipo Fiscal. \", {type: \"error\"});", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede facturar mas de $1000 a Consumidor Final desde Punto de venta tipo Fiscal."));
                        return;
                    }
                    int fiscalTope = this.validarTotalFiscal(fact);
                    if (fiscalTope < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar un monto de $" + fact.ptoV.tope + " o mayor desde un Punto de venta tipo Fiscal. \", {type: \"error\"});", true);
                        return;
                    }

                    int user = (int)Session["Login_IdUser"];

                    if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0)
                    {
                        //total NetosNoGravados cuando es venta combustible los guardo en .iva21
                        fact.iva21 = this.obtenerTotalImpuestosCombustibles(this.factura);
                    }
                    else
                    {
                        //obtengo el Neto no gravado || de los items con alicuota 0%
                        //fact.iva21 = fact.obtenerNetoNoGravado();
                        //fact.netoNGrabado = fact.obtenerNetoNoGravado();
                    }

                    int idForma = Convert.ToInt32(this.ListFormaVenta.SelectedValue);
                    int porcenOK = this.validarFacturacionPorcentual();

                    //proceso para facturar mitad y mitad (50 y 50)
                    if (porcenOK == 1 && idForma > 0)
                    {
                        this.procesoFacturarPorcentual(fact, dtPago, user, generaRemito);
                        return;
                    }

                    //facturo
                    int i = this.controlador.ProcesarFactura(null,fact, dtPago, user, generaRemito);
                    if (i > 0)
                    {
                        #region func post generar
                        if (this.accion == 4)
                        {
                            controladorFactEntity contFcEnt = new controladorFactEntity();
                            int idRemito = Convert.ToInt32(Request.QueryString["id_rem"]);
                            contFcEnt.agregarFacturaRemito(fact.id, idRemito);
                        }
                        if (this.accion == 6)
                        {
                            string facturas = Request.QueryString["facturas"];
                            Configuracion config = new Configuracion();
                            controladorFactEntity contFcEnt = new controladorFactEntity();

                            if (config.siemprePRP == "1")
                            {
                                //despues de generar NC establezco el iva de nuevo en NO informa
                                this.restablecerIvaCliente();

                                //si NC de prp cuando facturo anulo el remito par devolver las trazas
                                if (fact.tipo.tipo.Contains("Credito PRP") && config.egresoStock == "1")
                                {

                                    Remito r = this.controlador.obtenerRemitoByFactura(Convert.ToInt32(facturas.Split(';')[0]));
                                    this.contArticulo.AnularTrazabilidadArticulosDesdeRemito(r);
                                }

                                if (fact.formaPAgo.forma == "Mutuales")
                                {
                                    //Pongo en estado 0 los pagarés generados
                                    int anularPagares = contFcEnt.quitarPagoMutualByFactura(Convert.ToInt32(facturas.Split(';')[0]));
                                    if (anularPagares >= 0)
                                        Log.EscribirSQL(1, "INFO", "Se anularon los pagarés asociados a la factura con id " + facturas.Split(';')[0]);
                                    else
                                        Log.EscribirSQL(1, "ERROR", "No se pudieron anular los pagarés asociados a la factura con id " + facturas.Split(';')[0]);
                                }
                            }
                            if (fact.tipo.tipo.Contains("Credito PRP"))//elimino el anticipo
                            {
                                controladorCobranza contCobranza = new controladorCobranza();
                                Gestion_Api.Entitys.Facturas_Anticipos datosAnticipo = contFcEnt.obtenerDatosFacturaAnticipo(Convert.ToInt32(facturas.Split(';')[0]));
                                if (datosAnticipo != null)
                                {
                                    Movimiento_Cobro movAnticipo = contCobranza.obtenerMovimientoCobroByIDCobro(datosAnticipo.IdCobroAnticipo);
                                    //contCobranza.ProcesoEliminarCobro(movAnticipo.id);
                                    contCobranza.ProcesoEliminarCobroCompensacion(movAnticipo.id);
                                }
                            }

                            //Guardo la relación de la factura/s con la Nota de Crédito
                            int f_nc = contFcEnt.agregarFacturas_NotasCredito(facturas, fact.id);
                            if (f_nc <= 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Ocurrió un error guardando la relación de la Nota de Crédito con la/s factura/s. \", {type: \"error\"});", true);
                            }

                        }

                        if (this.accion != 6 && fact.formaPAgo.forma.Contains("Credito"))
                        {
                            //una vez que facture le agrgo el id de la factura al registro del cel y dni
                            this.agregarFacturaCodigoTelefono(fact);
                            if (this.accion != 9)
                            {
                                int okAnticipo = this.validarCobroAnticipo();
                                if (okAnticipo > 0)
                                {
                                    controladorCobranza contCobranza = new controladorCobranza();
                                    //genero primero el anticipo
                                    Cobro cobroAnticipo = Session["CobroAnticipo"] as Cobro;
                                    if (cobroAnticipo != null)
                                    {
                                        int anticipo = contCobranza.ProcesarCobroPagoCuenta(cobroAnticipo);
                                        if (anticipo > 0)
                                        {
                                            fact.idAnticipo = anticipo;
                                            //guardo
                                            var contFE = new controladorFactEntity();
                                            contFE.agregarDatosFacturaAnticipo(fact.id, anticipo);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo guardar el cobro del anticipo. \", {type: \"error\"});", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (this.rbtnPagoCuentaCredito.Checked && Convert.ToDecimal(this.txtAnticipo.Text) > 0)
                                        {
                                            string pagos = Session["PagoCuentaAnticipo"].ToString();
                                            if (!String.IsNullOrEmpty(pagos))
                                            {
                                                this.procesarPagosCuentaCredito(pagos);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo guardar el cobro del anticipo. \", {type: \"error\"});", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Guardo datos de anticipo Mutual
                        if (this.accion != 6 && this.accion != 9 && fact.formaPAgo.forma.Contains("Mutual"))
                        {
                            int okAnticipoMutual = this.validarAnticipoMutual();
                            {
                                if (okAnticipoMutual > 0)
                                {
                                    controladorCobranza contCobranza = new controladorCobranza();
                                    controladorFactEntity contFacturaEnt = new controladorFactEntity();
                                    Cobro cobroAnticipoMutual = Session["CobroAnticipo"] as Cobro;

                                    //Si se agregó un anticipo y se generó un cobro Pago a Cuenta
                                    if (cobroAnticipoMutual != null && rbtnAnticipoMutual.Checked)
                                    {
                                        int anticipoMutual = contCobranza.ProcesarCobroPagoCuenta(cobroAnticipoMutual);
                                        if (anticipoMutual > 0)
                                        {
                                            Log.EscribirSQL(1, "INFO", "Voy a almacenar los datos de anticipo de mutual. El id del cobro anticipo es: " + anticipoMutual + ". El id de la fc es: " + fact.id);
                                            fact.idAnticipo = anticipoMutual;
                                            int fcAntMutual = contFacturaEnt.agregarDatosFacturaAnticipo(fact.id, anticipoMutual);
                                            if (fcAntMutual > 0)
                                                Log.EscribirSQL(1, "INFO", "Se grabó correctamente el anticipo de mutual en la fc. El id del cobro anticipo es: " + anticipoMutual + ". El id de la fc es: " + fact.id);
                                            else
                                                Log.EscribirSQL(1, "ERROR", "Ocurrió un error guardando los datos de anticipo de mutual. El id del cobro anticipo es: " + anticipoMutual + ". El id de la fc es: " + fact.id);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo guardar el cobro del anticipo de Mutual. \", {type: \"error\"});", true);
                                            return;
                                        }
                                    }

                                    //Si se utilizo un cobro a cuenta que ya tenia el Cliente
                                    if (this.rbtnPagoCuentaMutual.Checked && Convert.ToDecimal(this.txtAnticipoMutual.Text) > 0)
                                    {
                                        string pagosMutual = Session["PagoCuentaAnticipoMutual"].ToString();

                                        if (!String.IsNullOrEmpty(pagosMutual))
                                            this.procesarPagosCuentaMutual(pagosMutual); //Imputo los pagos a cuenta seleccionados. Logeo en el metodo
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo guardar el cobro del anticipo de Mutual. \", {type: \"error\"});", true);
                                            return;
                                        }
                                    }
                                }

                                if (okAnticipoMutual < 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se genero el cobro del anticipo de Mutual!. \");", true);
                                    return;
                                }
                            }
                        }

                        if (this.chkEnviarMail.Checked == true && !String.IsNullOrEmpty(this.txtMailEntrega.Text))
                        {
                            this.EnviarFacturaMail(fact);
                        }

                        if (this.accion != 9)
                        {
                            this.agregarMovimientoMillas(fact);
                            this.EnviarSMSAviso(fact);
                        }
                        #endregion

                        //factura exitosa
                        Session.Remove("Factura");
                        string imprimir = WebConfigurationManager.AppSettings.Get("Imprime");
                        if (imprimir == "1")
                        {
                            this.ImprimirFactura(fact.id, fact.tipo.id, fact.idRemito);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturasLargo.aspx';", true);
                        }

                        this.btnNueva.Visible = true;
                        this.btnAgregar.Visible = false;
                        this.btnAgregarRemitir.Visible = false;
                    }
                    else
                    {
                        this.actualizarTotales();
                        if (i == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Se debe reprocesar factura electronica. Vuelva a facturar. \", {type: \"error\"});", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Se debe reprocesar factura electronica. Vuelva a facturar"));
                        }
                        string motivo = "";
                        try
                        {
                            motivo = fact.respuestaFE.Split('_')[2];
                        }
                        catch { }
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo generar factura." + motivo + ". \", {type: \"error\"});", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar factura "));
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Debe agregar articulos a la factura. " + this.txtCodigo.Text + " \", {type: \"error\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la factura " + this.txtCodigo.Text));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Error guardando facturas." + ex.Message + " \", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando facturas. " + ex.Message));
            }
        }
        private void procesoFacturarPorcentual(Factura fact, DataTable dtPago, int user, int generaRemito)
        {
            try
            {
                controladorFactEntity contFcEnt = new controladorFactEntity();
                controladorRemitos contR = new controladorRemitos();

                int idForma = Convert.ToInt32(this.ListFormaVenta.SelectedValue);
                //solo hago en cuenta corriente la fact partida
                fact.formaPAgo = this.controlador.obtenerFormaPagoFP("Cuenta Corriente");
                int i = contFcEnt.procesarFacturacionPorcentual(null,fact, dtPago, user, idForma);
                if (i > 0)
                {
                    int idPRP = contFcEnt.obtenerFacturaPorcentualById(i, 0);
                    Factura prp = this.controlador.obtenerFacturaId(idPRP);

                    if (this.chkEnviarMail.Checked == true && this.txtMailEntrega.Text != "")
                    {
                        this.EnviarFacturaMail(fact);
                        this.EnviarFacturaMail(prp);
                    }

                    int idRemito = 0;
                    if (generaRemito > 0)
                    {
                        idRemito = contR.RemitirDesdeFactura(fact);
                    }

                    //factura exitosa
                    Session.Remove("Factura");
                    string imprimir = WebConfigurationManager.AppSettings.Get("Imprime");
                    if (imprimir == "1")
                    {
                        this.ImprimirFacturaPorcentuales(fact, prp, generaRemito, idRemito);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturasLargo.aspx';", true);
                    }

                    this.btnNueva.Visible = true;
                    this.btnAgregar.Visible = false;
                    this.btnAgregarRemitir.Visible = false;
                }
                else
                {
                    this.actualizarTotales();
                    if (i == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Se debe reprocesar factura electronica. Vuelva a facturar. \", {type: \"error\"});", true);
                    }
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo generar factura. \", {type: \"error\"});", true);
                }
                return;
            }
            catch
            {
                return;
            }
        }
        private int restablecerIvaCliente()
        {
            try
            {
                //despues de refacturar dejo el IVA en no informa
                Cliente c = this.contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                int idIva = this.contCliente.obtenerIvaIdClienteByNombre("No Informa");
                c.iva = idIva.ToString();
                int cMod = this.contCliente.modificarCliente(c, c.cuit, c.codigo);
                return cMod;
            }
            catch
            {
                return -1;
            }
        }
        private int obtenerDatosIvasFactura(int idFc)
        {
            try
            {
                controladorFactEntity contFactEntity = new controladorFactEntity();
                var datos = contFactEntity.obtenerDatosIvasFactura(idFc);

                if (datos.CondicionIva != null)
                    return datos.CondicionIva.Value;
                else
                    return 2; // condicion iva = Consumidor final
            }
            catch
            {
                return -1;
            }
        }
        private void establecerIvaCliente(int idIva, int idCliente)
        {
            try
            {
                Cliente c = this.contCliente.obtenerClienteID(idCliente);
                c.iva = idIva.ToString();
                int cMod = this.contCliente.modificarCliente(c, c.cuit, c.codigo);
                return;
            }
            catch
            {

            }
        }
        private bool verificarArticulosSinActualizar()
        {
            try
            {
                if (Convert.ToInt32(this.configuracion.AlertaArticulosSinActualizar) > 0)
                {
                    Factura f = Session["Factura"] as Factura;
                    foreach (ItemFactura item in f.items)
                    {
                        if ((DateTime.Now - item.articulo.ultActualizacion).TotalDays > Convert.ToInt32(this.configuracion.AlertaArticulosSinActualizar))
                            return false;
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error verificando los articulos con precios sin actualizar. Excepción: " + Ex.Message));
                return true;
            }
        }

        #region items factura



        private void restarCantidadArticulo(object sender, EventArgs e)
        {
            try
            {
                var aux = (sender as LinkButton).ID.ToString().Split('_');
                string idArticulo = aux[1];
                Factura f = Session["Factura"] as Factura;
                foreach (var item in f.items)
                {
                    if (item.articulo.codigo == idArticulo)
                        f.items.Remove(item);
                }
                Session["Factura"] = f;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void QuitarItem(ItemFactura itemFactura)
        {
            try
            {
                //obtengo el pedido del session
                Factura ct = Session["Factura"] as Factura;
                foreach (ItemFactura item in ct.items)
                {
                    if (item.articulo.id == itemFactura.articulo.id)
                    {
                        //lo quito
                        ct.items.Remove(item);
                        break;
                    }
                }
                if (ct.items.Count == 0)
                {
                    this.lbMovTrazaNueva.Text = "";
                    this.lblMovTraza.Text = "";
                }
                //cargo el nuevo pedido a la sesion
                Session["Factura"] = ct;

                //vuelvo a cargar los items
                this.cargarItems();
                this.actualizarTotales();

                this.lblTotalMutuales.Text = decimal.Round(this.factura.total, 2).ToString();
                this.lblTotalOriginalMutuales.Text = decimal.Round(this.factura.total, 2).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item a la factura. " + ex.Message));
            }
        }
        private void TrazabilidadItem(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;
                int idArticulo = Convert.ToInt32(idBoton.Split('_')[1]);
                int posItem = Convert.ToInt32(idBoton.Split('_')[2]);

                Factura f = Session["Factura"] as Factura;
                ItemFactura item = f.items[posItem];

                this.lblTrazaTotal.Text = item.cantidad.ToString();

                this.lblMovTraza.Text = idBoton;

                this.CargarTrazasPH(idArticulo);

                //Articulo a = this.contArticulo.obtenerArticuloByID(idArticulo);
                //this.cargarCamposGrupoTrazabilidad(a);
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar trazabilidad item factura. " + ex.Message));
            }
        }
        private void CargarTrazabilidadItem(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;
                int idArticulo = Convert.ToInt32(idBoton.Split('_')[1]);
                int posItem = Convert.ToInt32(idBoton.Split('_')[2]);

                Factura f = Session["Factura"] as Factura;
                ItemFactura item = f.items[posItem];

                this.lbMovTrazaNueva.Text = idBoton;

                this.cargarCamposGrupo(item.articulo);


                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirCargaTraza('')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar trazabilidad item factura. " + ex.Message));
            }
        }
        private void EditarItem(object sender, EventArgs e)
        {
            try
            {
                string idCodigo = (sender as LinkButton).ID.ToString().Substring(10, (sender as LinkButton).ID.Length - 10);
                //obtengo el pedido del session
                Factura ct = Session["Factura"] as Factura;
                foreach (ItemFactura item in ct.items)
                {
                    if (item.articulo.codigo == idCodigo)
                    {
                        //lo quito
                        txtCodigo.Text = item.articulo.codigo;
                        txtCantidad.Text = item.cantidad.ToString();
                        txtDescripcion.Text = item.articulo.descripcion;
                        txtIva.Text = item.articulo.porcentajeIva.ToString();
                        TxtDescuentoArri.Text = item.descuento.ToString();
                        txtPUnitario.Text = item.precioUnitario.ToString();
                        txtTotalArri.Text = item.total.ToString();
                        ct.items.Remove(item);
                        break;

                    }
                }

                //cargo el nuevo pedido a la sesion
                Session["Factura"] = ct;

                //vuelvo a cargar los items
                this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item a la factura. " + ex.Message));
            }
        }
        private void DatosExtrasItem(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;
                //int idArticulo = Convert.ToInt32(idBoton.Split('_')[1]);
                //int posItem = Convert.ToInt32(idBoton.Split('_')[2]);

                this.lblDatosExtraItem.Text = idBoton;

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog2('')", true);

            }
            catch (Exception ex)
            {

            }
        }
        private void EditarDescItemRefacturar(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;
                this.lblEditarDescRefacturar.Text = idBoton.Split('_')[2];

                int pos = Convert.ToInt32(idBoton.Split('_')[2]);

                Factura f = Session["Factura"] as Factura;
                this.txtDescripcionItemRefact.Text = f.items[pos].articulo.descripcion;

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog3('')", true);

            }
            catch (Exception ex)
            {

            }
        }
        private int validarTrazaCargadaItemFactura(ItemFactura item)
        {
            try
            {
                int esTrazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                if (esTrazable == 1 && item.cantidad > 0)
                {
                    int cantTrazas = 0;
                    int trazaActual = -1;

                    //recorro las traza y veo cuantos Nro de traza distintos hay, los sumo y comparo con la cant del item.
                    foreach (Gestion_Api.Entitys.Trazabilidad tr in item.lstTrazas)
                    {
                        if (trazaActual < 0)
                        {
                            trazaActual = tr.Traza.Value;
                            cantTrazas++;
                        }
                        else
                        {
                            if (trazaActual != tr.Traza.Value)
                            {
                                cantTrazas++;
                                trazaActual = tr.Traza.Value;
                            }
                        }
                    }
                    if (cantTrazas == 0)
                    {
                        return -1;
                    }
                    if (Math.Abs(cantTrazas) < Math.Abs(item.cantidad))
                    {
                        return -2;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private void verificarDescuentoCantidad()
        {
            try
            {
                //Verifica en ClienteDatos si al cliente se le aplica descuento por cantidad
                Factura f = Session["Factura"] as Factura;
                var clienteDatos = this.contClienteEntity.obtenerClienteDatosByCliente(f.cliente.id);

                if (clienteDatos.Count > 0)
                {
                    if (clienteDatos[0].AplicaDescuentoCantidad == 1)
                    {
                        ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                        decimal cant = Convert.ToDecimal(this.txtCantidad.Text);

                        Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCod(this.txtCodigo.Text);
                        if (artEnt != null)
                        {
                            if (artEnt.Articulos_Descuentos.Count > 0)
                            {
                                var desc = artEnt.Articulos_Descuentos.Where(x => x.Desde <= cant && cant <= x.Hasta);
                                if (desc != null)
                                {
                                    var porcentaje = desc.Where(x => x.Desde == desc.Max(z => z.Desde)).FirstOrDefault();
                                    if (porcentaje != null)
                                        this.TxtDescuentoArri.Text = porcentaje.Descuento.ToString();
                                    else
                                        this.TxtDescuentoArri.Text = "0";
                                }
                                else
                                {
                                    this.TxtDescuentoArri.Text = "0";
                                }
                            }
                        }
                        else
                        {
                            //this.TxtDescuentoArri.Text = "0";
                        }
                    }
                }

            }
            catch
            {

            }
        }
        private void verificarMedidasVenta(int idArticulo)
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                var medida = contArtEnt.obtenerMedidasVentaArticulo(idArticulo);
                if (medida != null)
                {
                    if (medida.Count > 0)
                    {
                        this.chkVentaMedidaVenta.Visible = true;
                        this.chkVentaMedidaVenta.Checked = true;
                    }
                    else
                    {
                        this.chkVentaMedidaVenta.Checked = false;
                        this.chkVentaMedidaVenta.Visible = false;
                    }
                }
                else
                {
                    this.chkVentaMedidaVenta.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void verificarStockMinimo()
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                decimal cant = 0;

                try
                {
                    cant = Convert.ToDecimal(this.txtCantidad.Text);
                }
                catch { }

                //Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCod(this.txtCodigo.Text);
                Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCodigoYcodigoBarra(this.txtCodigo.Text);
                if (artEnt != null)
                {

                    //List<Stock> stocks = this.contArticulo.obtenerStockArticulo(artEnt.id);
                    var stocks = contArtEntity.obtenerStockArticuloLocal(artEnt.id, Convert.ToInt32(this.ListSucursal.SelectedValue));


                    decimal stock = 0;
                    decimal stockDestino = 0;
                    try
                    {
                        stock = stocks.stock1.Value;

                        //verifico stock, si es cliente interno

                        if (this.ListSucursalCliente.Visible == true)
                        {
                            var StockDestino = contArtEntity.obtenerStockArticuloLocal(artEnt.id, Convert.ToInt32(this.ListSucursalCliente.SelectedValue));
                            if (StockDestino != null)
                            {
                                stockDestino = StockDestino.stock1.Value;
                            }
                        }

                    }
                    catch { }

                    if (artEnt.stockMinimo > 0)
                    {
                        if (stock <= artEnt.stockMinimo)
                        {
                            this.lbtnStockProd.BackColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            if (stock - cant <= artEnt.stockMinimo)
                            {
                                this.lbtnStockProd.BackColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                this.lbtnStockProd.BackColor = System.Drawing.Color.Gray;
                            }
                        }
                    }
                    else
                    {
                        this.lbtnStockProd.BackColor = System.Drawing.Color.Gray;
                    }
                    this.lbtnStockProd.Text = decimal.Round(stock, 0).ToString();
                    this.lbtnStockDestinoProd.Text = decimal.Round(stockDestino, 0).ToString();
                }
            }
            catch
            {

            }
        }
        private void totalItem()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtCantidad.Text))
                {
                    decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                    decimal precio = Convert.ToDecimal(this.txtPUnitario.Text);
                    decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);

                    decimal total = decimal.Round((cantidad * precio), 2);

                    total = total - (total * (desc / 100));

                    total = decimal.Round(total, 2);

                    this.txtTotalArri.Text = decimal.Round(total, 2).ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total " + ex.Message));
            }
        }

        protected void lbtnEditarDescRefacturar_Click(object sender, EventArgs e)
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                string pos = this.lblEditarDescRefacturar.Text;
                int index = Convert.ToInt32(pos);
                f.items[index].articulo.descripcion = this.txtDescripcionItemRefact.Text;

                Session["Factura"] = f;

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel14, UpdatePanel14.GetType(), "alert", "$.msgbox(\"Modificado con exito!. \", {type: \"info\"}); cerrarModalEditarDesc(); ", true);

                this.cargarItems();
            }
            catch
            {

            }
        }
        #endregion

        #region controles
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.totalItem();
                //this.txtDescuento.Focus();
                this.verificarDescuentoCantidad();
                this.verificarStockMinimo();
                //decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", "."));
                //decimal precio = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                //decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                //decimal precio = Convert.ToDecimal(this.txtPUnitario.Text);
                //decimal total = cantidad * precio;
                //this.txtTotalArri.Text = decimal.Round(total,2).ToString();               

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "focoDesc();", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }
        protected void ActualizarTotalPH(object sender, EventArgs e)
        {
            try
            {
                string posicion = (sender as TextBox).ID.ToString().Split('_')[1];
                Factura ct = Session["Factura"] as Factura;
                ItemFactura item = ct.items[Convert.ToInt32(posicion)];
                string cantNueva = (sender as TextBox).Text.Replace(',', '.');
                item.cantidad = Convert.ToDecimal(cantNueva, CultureInfo.InvariantCulture);

                //item.porcentajeDescuento = 0;

                //me puede pasar que el item tenga un descuento que puso el usuario, pero si el art tiene desc por cantidad
                //Prima el descuento por cantidad

                //verifico si tengo que hacer un descuento por cantidad
                decimal descCantidad = this.obtenerNuevoDescuentoCantidad(item.articulo.codigo, item.cantidad);
                if (descCantidad > 0)//si es descuento por cantidad, piso el del item, sino lo dejo
                {
                    item.porcentajeDescuento = descCantidad;
                }
                else
                {
                    //el articulo tiene desc por cantidad y el cliente aplica descuento por cantidad, mantengo descuento por cantidad
                    //verifico si cliente aplica descuento por cantidad y el arti tambien aplica pero la cant. seleccionada no tiene un descuento lo pongo en cero
                    if (descCantidad == -1)
                    {
                        item.porcentajeDescuento = 0;//pongo descuento en cero
                    }
                    else //mantengo el descuento 
                    {
                        item.porcentajeDescuento = item.porcentajeDescuento;
                    }
                }


                item.descuento = (item.precioUnitario * (item.porcentajeDescuento / 100)) * item.cantidad;
                item.total = ((item.precioUnitario * (1 - (item.porcentajeDescuento / 100))) * item.cantidad);
                ct.items.Remove(item);
                ct.items.Insert(Convert.ToInt32(posicion), item);
                TableRow tr = this.phArticulos.Controls[Convert.ToInt32(posicion)] as TableRow;
                //actualizo descuento
                TableCell c2 = tr.Cells[4] as TableCell;
                c2.Text = "$" + Decimal.Round(item.descuento, 2).ToString();
                //actualizo total
                TableCell c = tr.Cells[5] as TableCell;
                c.Text = "$" + Decimal.Round(item.total, 2).ToString();
                //cargo el nuevo pedido a la sesion
                Session["Factura"] = ct;

                //vuelvo a cargar los items
                //this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }
        private decimal obtenerNuevoDescuentoCantidad(string codigo, decimal cantNueva)
        {
            try
            {
                //Verifica en ClienteDatos si al cliente se le aplica descuento por cantidad
                Factura f = Session["Factura"] as Factura;
                var clienteDatos = this.contClienteEntity.obtenerClienteDatosByCliente(f.cliente.id);
                if (clienteDatos.Count > 0)
                {
                    if (clienteDatos[0].AplicaDescuentoCantidad == 1)
                    {
                        ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                        Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCod(codigo);
                        if (artEnt != null)
                        {
                            if (artEnt.Articulos_Descuentos.Count > 0)
                            {
                                var desc = artEnt.Articulos_Descuentos.Where(x => x.Desde <= Convert.ToDecimal(cantNueva) && Convert.ToDecimal(cantNueva) <= x.Hasta);
                                if (desc != null)
                                {
                                    var porcentaje = desc.Where(x => x.Desde == desc.Max(z => z.Desde)).FirstOrDefault();
                                    if (porcentaje != null)
                                    {
                                        return porcentaje.Descuento.Value;
                                    }
                                    else
                                    {
                                        return -1; //cliente tiene desc por cantidad pero la cantidad elegida no tiene descuento
                                    }
                                }
                                else
                                {
                                    return -1; //cliente tiene desc por cantidad pero la cantidad elegida no tiene descuento
                                }
                            }
                        }
                        return 0;
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }
        protected void TxtDescuentoArri_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Configuracion c = new Configuracion();
                decimal dto = Convert.ToDecimal(TxtDescuentoArri.Text);
                decimal dtoMax = Convert.ToDecimal(c.maxDtoUnitario);

                if (dtoMax > 0)
                {
                    if (dto <= dtoMax)
                    {
                        this.totalItem();
                        this.lbtnAgregarArticuloASP.Focus();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al " + dtoMax + "%. \"); ", true);
                        TxtDescuentoArri.Text = "0";
                        this.totalItem();
                    }
                }
                else
                {
                    this.totalItem();
                    this.lbtnAgregarArticuloASP.Focus();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total con descuento. Verifique que ingreso numeros en Descuento" + ex.Message));
            }
        }
        protected void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.validarDescuentoFactura();
                if (i < 0)
                {
                    Configuracion c = new Configuracion();
                    decimal dtoMax = Convert.ToDecimal(c.maxDtoFactura);

                    this.txtPorcDescuento.Text = "0";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al " + dtoMax + "%. \"); ", true);
                    this.actualizarTotales();
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No puede ingresar un descuento mayor al " + dtoMax + "%."));
                }
            }
            catch
            {

            }

        }
        protected void txtRetencion_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }
        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtPtoVenta.Text = this.ListSucursal.SelectedValue;
            cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));

            cargarVendedor();
            //Me fijo si hay que cargar un cliente por defecto
            this.verificarClienteDefecto();
        }
        protected void ListPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //obtengo el numero de factura
                this.obtenerNroFactura();
                //si es punto fical muesto el boton cierre Z
                PuntoVenta pv = this.cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                if (pv.formaFacturar == "Fiscal")
                {
                    this.btnCierreZ.Visible = true;
                }
                else
                {
                    this.btnCierreZ.Visible = false;
                }
                if (pv.formaFacturar == "Preimpresa")
                {
                    //dejo modificar fecha
                    this.txtFecha.Attributes.Remove("Disabled");
                }
                else
                {
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                //verifico el cierre de caja del punto de venta
                this.verificarCierreCaja();
            }
            catch (Exception ex)
            {

            }
        }
        protected void txtPorcRetencion_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }
        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //vacio la factura actual

                if (Session["Factura"] != null)
                {
                    Factura f = Session["Factura"] as Factura;
                    f.items.Clear();
                    this.borrarCamposagregarItem();
                    Session.Add("Factura", f);
                    //lo dibujo en pantalla
                    this.cargarItems();
                }

                this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                this.obtenerNroFactura();

            }
            catch
            {

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
        }
        protected void btnAgregarFP_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.controlador.agregarFormaPAgo(this.txtFormaPago.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarFormaPAgo();
                    this.txtFormaPago.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar forma de pago "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando forma de pago. " + ex.Message));
            }
        }
        protected void btnAgregarLista_Click(object sender, EventArgs e)
        {
            try
            {
                listaPrecio lst = new listaPrecio();
                lst.nombre = txtNombreLista.Text;
                int i = this.controlador.agregarlistaPrecio(lst);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarListaPrecio();
                    this.txtNombreLista.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar lista de precio "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista de precio " + ex.Message));
            }
        }
        protected void btnAgregarVendedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorVendedor contVen = new controladorVendedor();
                Vendedor ven = new Vendedor();
                ven.emp.id = Convert.ToInt32(ListEmpleados.SelectedValue);
                if (!String.IsNullOrEmpty(txtComision.Text))
                {
                    ven.comision = Convert.ToDecimal(txtComision.Text);
                }
                else
                {
                    ven.comision = 0;
                }
                int i = contVen.agregarVendedor(ven);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarVendedor();
                    this.ListEmpleados.SelectedValue = "-1";
                    this.txtComision.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar vendedor "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando vendedor " + ex.Message));
            }
        }
        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBuscarCliente.Text))
            {

            }
            else
            {
            }
        }
        protected void DropListFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DropListFormaPago.SelectedItem.Text == "Tarjeta")
                {
                    this.btnTarjeta.Visible = true;
                    this.btnCredito.Visible = false;
                    this.btnMutual.Visible = false;
                }
                else
                {
                    if (DropListFormaPago.SelectedItem.Text == "Credito")
                    {
                        this.btnCredito.Visible = true;
                    }
                    else
                    {
                        this.btnCredito.Visible = false;
                    }

                    if (DropListFormaPago.SelectedItem.Text == "Mutuales")
                    {
                        this.btnMutual.Visible = true;
                    }
                    else
                    {
                        this.btnMutual.Visible = false;
                    }

                    this.btnTarjeta.Visible = false;
                }

                //me guardo el id de la lista seleccionada para mantenerlo al recargar la lista
                int listaAnt = Convert.ToInt32(this.DropListLista.SelectedValue);
                this.cargarListaPrecio();
                this.DropListLista.SelectedValue = listaAnt.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error seleccionando tipo de Pago. " + ex.Message));
            }
        }
        protected void checkDatos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkDatos.Checked)
                {
                    this.phDatosEntrega.Visible = true;
                }
                else
                {
                    this.phDatosEntrega.Visible = false;
                }
            }
            catch
            { }
        }
        protected void chkIvaNoInformado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.actualizarTotales();
                //if (this.chkIvaNoInformado.Checked == true)
                //{
                //    this.txtComentarios.Text += "Percepcion IVA a Consumidor Final ($" + this.factura.iva10 + ")";
                //}
                //else
                //{
                //    this.txtComentarios.Text = "";
                //}
            }
            catch
            {

            }
        }
        protected void ListOperadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.ListOperadores.SelectedValue);
                this.cargarTarjetasByOperador(id);
            }
            catch
            {

            }
        }
        protected void ListTarjetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //this.txtImporteT.Text = totalRecargo.ToString();
                //this.txtImporteEfectivo.Text = totalRecargo.ToString();
                DataTable dt = lstPago;
                decimal resta = 0;
                foreach (DataRow row in dt.Rows)
                {
                    resta += Convert.ToDecimal(row["Neto"]);
                }

                this.txtImporteT.Text = (Convert.ToDecimal(this.txtTotal.Text) - resta).ToString();
                this.txtImporteEfectivo.Text = (Convert.ToDecimal(this.txtTotal.Text) - resta).ToString();

                //solo cuando es pago total con una sola tarjeta dejo la promocion
                if (resta == 0)
                    this.verificarPromocionTarjeta();

                //actualizo lbl de monto cuotas
                this.calcularPagosCuotas();
            }
            catch (Exception ex)
            {

            }
        }
        protected void txtPUnitario_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Configuracion c = new Configuracion();
                if (c.edicionPrecioUnitario == "1" && c.editarArticulo == "1")
                {
                    Articulo art = contArticulo.obtenerArticuloFacturar(this.txtCodigo.Text, Convert.ToInt32(this.DropListLista.SelectedValue));
                    decimal precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text);

                    if (precioUnitario < art.precioVenta)
                    {
                        this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede modificar el precio del art a un valor menor del precio original!. \");", true);
                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
        }
        protected void lbtnStockProd_Click(object sender, EventArgs e)
        {
            try
            {
                //abre en pestaña nueva pantalla de stock del articulo
                Articulo art = this.contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('../Articulos/StockF.aspx?a=2&fd=" + DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy") + "&fh=" + DateTime.Today.AddDays(1).ToString("dd/MM/yyyy") + "&articulo=" + art.id + "&s=" + this.ListSucursal.SelectedValue + "');", true);
            }
            catch
            {

            }
        }
        protected void chkEnviarMail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (chkEnviarMail.Checked)
                //{
                //    this.txtMailEntrega.Attributes.Remove("disabled");
                //}
                //else
                //{
                //    this.txtMailEntrega.Attributes.Add("style", "disabled");
                //}
            }
            catch
            {

            }
        }
        protected void lbtnVerCtaCte_Click(object sender, EventArgs e)
        {
            try
            {
                int idCliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                int idSuc = Convert.ToInt32(this.ListSucursal.SelectedValue);

                if (idCliente > 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "window.open('CuentaCorrienteF.aspx?a=1&tipo=-1&Cliente=" + idCliente + "&Sucursal=" + idSuc + "','_blank');", true);
            }
            catch
            {

            }
        }
        protected void lbtnAnticipo_Click(object sender, EventArgs e)
        {
            try
            {
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;
                if (anticipoCargado != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos1, UpdatePanelCreditos1.GetType(), "alert", " $.msgbox(\"El cobro del anticipo ya fue cargado anteriormente. \", {type: \"info\"});", true);
                }

                int idEmp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int idSuc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int idPv = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                int idCli = Convert.ToInt32(this.DropListClientes.SelectedValue);
                if (idEmp > 0 && idSuc > 0 & idPv > 0 && idCli > 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos1, UpdatePanelCreditos1.GetType(), "alert", "window.open('../Cobros/ABMCobros?documentos=0;&cliente=" + idCli + "&empresa=" + idEmp + "&sucursal=" + idSuc + "&puntoVenta=" + idPv + "&monto=" + this.txtAnticipo.Text + "&valor=2&tipo=2&anticipo=1');", true);
            }
            catch
            {

            }
        }
        #endregion

        #region validaciones fin
        private int validarSaldoTarjeta()
        {
            try
            {
                if (this.accion == 6)//genera nc desde fc
                {
                    return 1;
                }

                decimal total = 0;
                decimal totalFC = Convert.ToDecimal(this.lblMontoOriginal.Text.ToString());
                DataTable dt = this.lstPago;
                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["Neto"].ToString());
                }

                if (total == totalFC)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando monto igresado con tarjeta." + ex.Message));
                return -1;
            }
        }
        private bool validarSaldoMutual()
        {
            try
            {
                if (this.DropListFormaPago.SelectedItem.Text != "Mutuales")
                    return true;

                if (this.accion == 6)
                    return true;

                decimal totalFcConRecargo = Convert.ToDecimal(this.lblTotalRecargoFinalOriginalMutuales.Text);
                Factura fact = Session["Factura"] as Factura;

                if (fact != null && (Math.Truncate(fact.total) == Math.Truncate(totalFcConRecargo)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando saldo ingresado en mutual. Excepción:" + Ex.Message));
                return false;
            }
        }
        private int validarUltimaFactura()
        {
            try
            {
                Factura fact = Session["Factura"] as Factura;

                if (this.txtFecha.Text != DateTime.Now.ToString("dd/MM/yyyy"))
                {
                    PuntoVenta pv = this.cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    Factura ultimaFact = this.controlador.obtenerUltimaFacturaPtoVta(Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(pv.puntoVenta), fact.tipo.id);
                    if (pv.formaFacturar == "Preimpresa" && ultimaFact != null)
                    {
                        //if (Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")).DayOfYear >= ultimaFact.fecha.DayOfYear)
                        DateTime fechaFactActual = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")).AddHours(DateTime.Now.Hour);
                        if (fechaFactActual >= ultimaFact.fecha)
                        {
                            this.flag_cambioFecha = 1;
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando fecha de la factura." + ex.Message));
                return -2;
            }
        }
        private int validarTrazasCargadas()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                Configuracion config = new Configuracion();
                if (config.siemprePRP == "1" && this.accion == 6 && !f.tipo.tipo.Contains("PRP"))
                {
                    return 1;
                }

                foreach (ItemFactura item in f.items)
                {
                    int esTrazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                    if (esTrazable == 1 && item.cantidad > 0)
                    {
                        int cantTrazas = 0;
                        int trazaActual = -1;

                        //recorro las traza y veo cuantos Nro de traza distintos hay, los sumo y comparo con la cant del item.
                        foreach (Gestion_Api.Entitys.Trazabilidad tr in item.lstTrazas)
                        {
                            if (trazaActual < 0)
                            {
                                trazaActual = tr.Traza.Value;
                                cantTrazas++;
                            }
                            else
                            {
                                if (trazaActual != tr.Traza.Value)
                                {
                                    cantTrazas++;
                                    trazaActual = tr.Traza.Value;
                                }
                            }
                        }
                        if (cantTrazas == 0)
                        {
                            return -1;
                        }
                        if (cantTrazas < item.cantidad)
                        {
                            return -2;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private int validarItemsEnCero()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                Configuracion config = new Configuracion();
                if (config.ItemsEnCero == "0")
                {
                    foreach (ItemFactura item in f.items)
                    {
                        if (item.precioUnitario == 0)
                        {
                            return -1;
                        }
                    }
                }

                return 1;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error en validar items en cero. Excepción: " + Ex.Message);
                return 1;
            }
        }

        private int validarFacturacionPorcentual()
        {
            try
            {
                controladorFactEntity contFcEnt = new controladorFactEntity();
                //Si tiene configurado para que facutre 
                string formaFacturarPorcentual = WebConfigurationManager.AppSettings.Get("FormaFacturar");
                //obtengo formas de vta porcentuales
                List<Gestion_Api.Entitys.Formas_Venta> formas = contFcEnt.obtenerFormasVenta();
                Factura fact = Session["Factura"] as Factura;

                //si el sistema tiene cargadas formas de venta porcentuales y si no es PRP
                if ((formaFacturarPorcentual == "1" && formas.Count > 0) && (!this.labelNroFactura.Text.Contains("PRP") && !this.labelNroFactura.Text.Contains("Presupuesto")))
                {
                    //si el cliente tiene cargado una forma de venta porcentual
                    var fv = this.contClienteEntity.obtenerFormaVentaCliente(fact.cliente.id);
                    if (fv != null)
                    {
                        //OK puede facturar porcentual
                        return 1;
                    }
                }

                //no cumple, entonces factura normal
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        private int validarTotalConsumidorFinal(Factura f)
        {
            try
            {
                int i = 0;
                if (f.ptoV.formaFacturar == "Fiscal" && (f.tipo.id == 1 || f.tipo.id == 2))
                {
                    decimal total = f.total;
                    String cuit = f.cliente.cuit;

                    if ((cuit == "00000000000" || cuit == "00-00000000-0") && (total > 1000) && (f.formaPAgo.id == 1))
                    {
                        i = -1;
                    }
                    else
                    {
                        i = 1;
                    }

                }
                else
                {
                    i = 1;
                }

                return i;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private int validarTotalFiscal(Factura f)
        {
            try
            {
                int i = 0;
                if (f.ptoV.formaFacturar == "Fiscal" && (f.tipo.id == 1 || f.tipo.id == 2 || f.tipo.id == 3 || f.tipo.id == 24)) // Factura A,B,C,E
                {
                    decimal total = f.total;

                    //if (total >= 10000)
                    if (total >= f.ptoV.tope)
                    {
                        i = -1;
                    }
                    else
                    {
                        i = 1;
                    }

                }
                else
                {
                    i = 1;
                }

                return i;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private int validarIdImpositivoCliente()
        {
            try
            {
                int cliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                DataTable dt = this.contCliente.obtenerIdImpositivoCliente(cliente);

                if (dt.Rows.Count > 0)
                {
                    return 1;
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El cliente no tiene ID Impositivo para exportacion!."));
                    this.btnFacturaE.Visible = false;
                    return -1;
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo datos de cliente para exportacion." + ex.Message));
                return -1;
            }
        }
        private int validarDescuentoFactura()
        {
            try
            {
                Configuracion c = new Configuracion();
                decimal dto = Convert.ToDecimal(txtPorcDescuento.Text);
                decimal dtoMax = Convert.ToDecimal(c.maxDtoFactura);
                this.factura = Session["Factura"] as Factura;

                //si tiene seteado limite
                if (dtoMax > 0)
                {
                    //verifico que el dto que trato de agregar no sea mayor al limite
                    if (dto > dtoMax)
                    {
                        return -1;
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al " + dtoMax + "%. \"); ", true);
                        //this.actualizarTotales();
                    }
                    //else
                    //{
                    //    return -1;
                    //    //this.txtPorcDescuento.Text = "0";
                    //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No puede ingresar un descuento mayor al " + dtoMax + "%."));
                    //    ////ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al " + dtoMax + "%. \"); ", true);
                    //    //this.actualizarTotales();
                    //}
                }
                //else
                //{
                //    return 1;
                //    //this.actualizarTotales();
                //}
                this.actualizarTotales();
                this.lblMontoOriginal.Text = this.factura.total.ToString();
                Factura f = this.factura;
                Session.Add("Factura", f);

                return 1; //Puede poner ese dto en la fc,recalculo y sigo
            }
            catch
            {
                return -1;
            }
        }
        private int validarFacturarTotalCero(Factura f)
        {
            try
            {
                //controladorListaPrecio controlLista = new controladorListaPrecio();
                //listaPrecio lista = controlLista.obtenerlistaPrecioID(Convert.ToInt32(this.DropListLista.SelectedValue));
                //bool flag = false;
                ////si esta facturando con listas de precio con dto al 100% lo dejo seguir
                //foreach (ItemFactura item in f.items)
                //{
                //    if (item.total == 0)
                //    {
                //        SubListaPrecio sl = controlLista.obtenerSubListaProducto(lista.id, item.articulo.listaCategoria.id);
                //        //if (sl.porcentaje < 100 && sl.AumentoDescuento != 2)
                //        if (sl.porcentaje >= 100 && sl.AumentoDescuento == 2)
                //        {
                //            return 1;
                //        }
                //        flag = true;
                //    }
                //}
                //if (flag == true)
                //{
                //    return -1;
                //}
                //else
                //{
                //    return 1;
                //}
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private int validarCobroAnticipo()
        {
            try
            {
                if (this.rbtnPagoCuentaCredito.Checked)
                {
                    string pagos = Session["PagoCuentaAnticipo"].ToString();
                    if (!String.IsNullOrEmpty(pagos))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                if (!String.IsNullOrEmpty(this.txtAnticipo.Text))
                {
                    if (Convert.ToDecimal(this.txtAnticipo.Text) > 0)
                    {
                        if (Session["CobroAnticipo"] != null)
                        {
                            Cobro anticipo = Session["CobroAnticipo"] as Cobro;
                            if (anticipo != null)
                                return 1;
                            else
                                return -1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
        private int validarDatosExtrasCargadosFactura(Factura f)
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.siemprePRP == "1" && this.accion == 6 && !f.tipo.tipo.Contains("PRP"))
                {
                    return 1;
                }
                //verifico que estan cargados los datos extras
                foreach (ItemFactura item in f.items)
                {
                    int i = this.verificarDatosExtrasCargados(item);
                    if (i < 1)
                        return -1;
                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        private bool validarNotaCreditoFactura()
        {
            try
            {
                controladorFactEntity contFactEnt = new controladorFactEntity();
                Factura factura = Session["Factura"] as Factura;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                //Verifico si es Nota de Credito. Si lo es, realizo las validaciones
                if (accion == 6)
                {
                    //Verifico si tiene el permiso para refacturar nota de crédito. Si lo tiene devuelvo true.
                    string permisoReFacturarNotaCredito = listPermisos.Where(x => x == "149").FirstOrDefault();
                    if (!string.IsNullOrEmpty(permisoReFacturarNotaCredito))
                    {
                        return true;
                    }

                    //Valido que no se hayan realizado Notas de Crédito para las facturas seleccionadas
                    bool response = contFactEnt.verificarFacturas_NotasCreditoByFacturas(Request.QueryString["facturas"]);
                    return response;
                }

                return true;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Ocurrió un error validando facturas para realizar Nota de Crédito. Excepción:" + Ex.Message + " \", {type: \"error\"});", true);
                return false;
            }
        }
        #endregion

        #region pagos tarjeta
        public void calcularPagosCuotas()
        {
            try
            {
                //genero la clase
                Tarjeta t = ct.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas.SelectedValue));
                //obtengo parametro si usa recargos o no
                string recargo = WebConfigurationManager.AppSettings.Get("Recargo");
                if (recargo == "1")
                {
                    //decimal totalRecargo = Convert.ToDecimal(this.txtTotal.Text);
                    decimal totalRecargo = Convert.ToDecimal(this.txtImporteT.Text);
                    totalRecargo = Decimal.Round(totalRecargo * (1 + (t.recargo / 100)), 2);
                    decimal cuotasFinal = Decimal.Round((totalRecargo / t.cuotas), 2);
                    this.lblMontoCuotas.Text = "Pago en " + t.cuotas + " cuotas de $" + cuotasFinal + " final. ";
                    if (t.recargo > 0)
                    {
                        this.lblMontoCuotas.Text += "Con recargo del " + t.recargo + "%";
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(this.txtTotal.Text);
                    decimal cuotasFinal = Decimal.Round((total / t.cuotas), 2);
                    this.lblMontoCuotas.Text = "Pago en " + t.cuotas + " cuotas de $" + cuotasFinal + " final. ";
                    if (t.recargo > 0)
                    {
                        this.lblMontoCuotas.Text += "Con recargo del " + t.recargo + "%";
                    }
                }
            }
            catch
            {

            }
        }
        protected void txtImporteT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.txtImporteT.Text != "")
                    this.calcularPagosCuotas();
            }
            catch
            {

            }
        }
        protected void lbtnAgregarEfectivo_Click(object sender, EventArgs e)
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                if (f.items.Count > 0)
                {
                    //Guardar la info de pago en el DT Temporal de pagos
                    DataTable dt = lstPago;
                    DataRow dr = dt.NewRow();
                    dr["Tipo Pago"] = "Efectivo";
                    dr["Importe"] = Convert.ToDecimal(this.txtImporteEfectivo.Text);
                    dr["Neto"] = Convert.ToDecimal(this.txtImporteEfectivo.Text);
                    dr["Recargo"] = Convert.ToDecimal(0.00);

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    //llamo al metodo que muestra los pagos en la tabla
                    this.cargarTablaPAgos();
                    this.txtImporteEfectivo.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la factura "));
                }

            }
            catch
            {

            }
        }
        protected void lbtnAgregarPago_Click(object sender, EventArgs e)
        {
            try
            {
                //genero la clase
                Tarjeta t = ct.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas.SelectedValue));
                Pago_Tarjeta ptarjeta = new Pago_Tarjeta();

                Factura f = Session["Factura"] as Factura;
                if (f.items.Count > 0)
                {
                    //obtengo parametro si usa recargos o no
                    string recargo = WebConfigurationManager.AppSettings.Get("Recargo");
                    if (recargo == "1")
                    {
                        decimal totalActual = Convert.ToDecimal(this.txtImporteEfectivo.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                        ptarjeta.tarjeta.id = Convert.ToInt32(this.ListTarjetas.SelectedValue);
                        ptarjeta.tarjeta.nombre = this.ListTarjetas.SelectedItem.Text;
                        decimal monto = Convert.ToDecimal(txtImporteT.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        ptarjeta.monto = monto;
                        ptarjeta.tarjeta.recargo = t.recargo;
                        //decimal TotalIngresado = decimal.Round(ptarjeta.monto + ptarjeta.monto * (ptarjeta.tarjeta.recargo / 100), 2);
                        decimal TotalIngresado = decimal.Round(ptarjeta.monto, 2);


                        if (totalActual >= TotalIngresado)
                        {
                            decimal montoRecargado = TotalIngresado;
                            if (t.recargo > 0)
                            {
                                montoRecargado = Convert.ToDecimal(txtImporteT.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture) * (1 + (t.recargo / 100));
                                //this.SumarRecargoTarjeta(t);
                                montoRecargado = decimal.Round(montoRecargado, 2);
                            }

                            //Guardar la info de pago en el DT Temporal de pagos
                            DataTable dt = lstPago;
                            DataRow dr = dt.NewRow();
                            dr["Tipo Pago"] = ptarjeta.tarjeta.nombre;
                            dr["Importe"] = montoRecargado;//TotalIngresado;
                            dr["Neto"] = ptarjeta.monto;
                            dr["Recargo"] = ptarjeta.tarjeta.recargo;

                            dt.Rows.Add(dr);
                            lstPago = dt;

                            this.lblAvisoTarjeta.Visible = false;

                            this.lbFormaDePago.Text = "Forma de pago: Tarjeta";
                        }
                        else
                        {
                            this.lblAvisoTarjeta.Text = "El Monto Ingresado supera al total de la factura. ";
                            this.lblAvisoTarjeta.Visible = true;
                            this.txtImporteT.Focus();
                        }

                        //llamo al metodo que muestra los pagos en la tabla
                        this.cargarTablaPAgos();
                        this.txtImporteT.Text = "";
                    }
                    else
                    {
                        decimal totalActual = Convert.ToDecimal(this.txtImporteEfectivo.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                        ptarjeta.tarjeta.id = Convert.ToInt32(this.ListTarjetas.SelectedValue);
                        ptarjeta.tarjeta.nombre = this.ListTarjetas.SelectedItem.Text;
                        decimal monto = Convert.ToDecimal(txtImporteT.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        ptarjeta.monto = monto;
                        ptarjeta.tarjeta.recargo = t.recargo;
                        //decimal TotalIngresado = decimal.Round(ptarjeta.monto + ptarjeta.monto * (ptarjeta.tarjeta.recargo / 100), 2);
                        decimal TotalIngresado = decimal.Round(ptarjeta.monto, 2);

                        if (totalActual >= TotalIngresado)
                        {

                            //Guardar la info de pago en el DT Temporal de pagos
                            DataTable dt = lstPago;
                            DataRow dr = dt.NewRow();
                            dr["Tipo Pago"] = ptarjeta.tarjeta.nombre;
                            dr["Importe"] = TotalIngresado;//TotalIngresado;
                            dr["Neto"] = ptarjeta.monto;
                            dr["Recargo"] = ptarjeta.tarjeta.recargo;

                            dt.Rows.Add(dr);
                            lstPago = dt;

                            this.lblAvisoTarjeta.Visible = false;
                        }
                        else
                        {
                            this.lblAvisoTarjeta.Text = "El Monto Ingresado supera al total de la factura. ";
                            this.lblAvisoTarjeta.Visible = true;
                            this.txtImporteT.Focus();
                        }

                        //llamo al metodo que muestra los pagos en la tabla
                        this.cargarTablaPAgos();
                        this.txtImporteT.Text = "";
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.updatePanelModoImagen, updatePanelModoImagen.GetType(), "alert", "$.msgbox(\"Debe agregar articulos a la factura. \", {type: \"info\"});cerrarModalTarjeta();", true);
                }
            }
            catch
            {

            }
        }
        private void cargarTablaPAgos()
        {
            try
            {
                decimal totalFactura = Convert.ToDecimal(this.txtTotal.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                decimal totalRecargos = 0;
                decimal totTarjeta = 0;
                DataTable dt = this.lstPago;

                //limpio el Place holder
                this.phPagosTarjeta.Controls.Clear();
                //decimal saldo = 0;

                //obtengo parametro si usa recargos o no
                string recargo = WebConfigurationManager.AppSettings.Get("Recargo");
                if (recargo == "1")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int pos = dt.Rows.IndexOf(dr);
                        this.cargarPHPagos(dr, pos);
                        //totTarjeta += Convert.ToDecimal(dr["Importe"]) / (1 + (Convert.ToDecimal(dr["Recargo"]) / 100));
                        totTarjeta += Convert.ToDecimal(dr["Neto"]);
                        totalRecargos += Convert.ToDecimal(dr["Importe"]);
                    }
                    decimal totalI = Decimal.Round(totalFactura - totTarjeta, 2);
                    this.txtImporteEfectivo.Text = totalI.ToString();
                    //this.lblSaldoTarjeta.Text = totalFactura.ToString();
                    this.lblSaldoTarjeta.Text = totalRecargos.ToString();
                    if (totalI == 0 && dt.Rows.Count > 0)//si ya complete el monto final sin recargos de la factura , le aplico el recargo a los items
                    {
                        int i = this.SumarRecargoTarjeta(dt);
                        this.txtImporteEfectivo.Text = "0.00";
                        if (this.accion != 6)
                        {
                            if (i > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int pos = dt.Rows.IndexOf(dr);
                        this.cargarPHPagos(dr, pos);
                        totTarjeta += Convert.ToDecimal(dr["Importe"]);
                    }
                    decimal totalI = Decimal.Round(totalFactura - totTarjeta, 2);
                    this.txtImporteEfectivo.Text = totalI.ToString();
                    this.lblSaldoTarjeta.Text = totalFactura.ToString();
                    if (totalI == 0 && dt.Rows.Count > 0)//si ya complete el monto final sin recargos de la factura , le aplico el recargo a los items
                    {
                        this.txtImporteEfectivo.Text = "0.00";
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista Pagos " + ex.Message));
            }

        }
        protected void cargarPHPagos(DataRow dr, int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["Tipo Pago"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Importe"].ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Center;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celNeto = new TableCell();
                celNeto.Text = dr["Neto"].ToString();
                celNeto.HorizontalAlign = HorizontalAlign.Center;
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                celNeto.Visible = false;
                tr.Cells.Add(celNeto);

                TableCell celAccion = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + pos.ToString();
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarPago);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phPagosTarjeta.Controls.Add(tr);

            }
            catch
            {

            }
        }
        private void QuitarPago(object sender, EventArgs e)
        {
            try
            {
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                //obtengo el pedido del session
                DataTable dt = lstPago;
                decimal montoOriginal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dt.Rows.IndexOf(dr).ToString() == codigo[1])
                    {
                        string recargo = WebConfigurationManager.AppSettings.Get("Recargo");
                        if (recargo == "1")
                        {
                            int i = this.QuitarRecargoTarjeta(dt);
                            if (i > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                            }
                            //if (dr["Tipo Pago"].ToString() != "Efectivo")
                            //{
                            //    int idTarjeta = Convert.ToInt32(this.ListTarjetas.Items.FindByText(dr["Tipo Pago"].ToString()).Value);
                            //    Tarjeta tarjeta = this.ct.obtenerTarjetaID(idTarjeta);
                            //    if (tarjeta.recargo > 0)
                            //    {
                            //        if ((this.lblMontoOriginal.Text != this.lblSaldoTarjeta.Text) && dt.Rows.Count > 0)//si es el primero que borro y ya aplique el recargo a la factura.
                            //        {
                            //            int i = this.QuitarRecargoTarjeta(dt);
                            //            if (i > 0)
                            //            {
                            //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                            //            }
                            //            else
                            //            {
                            //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                            //            }
                            //        }

                            //    }
                            //}
                            //else
                            //{
                            //    int i = this.QuitarRecargoTarjeta(dt);
                            //    if (i > 0)
                            //    {
                            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                            //    }
                            //    else
                            //    {
                            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                            //    }
                            //}
                        }

                        //lo quito
                        dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));
                        this.lblAvisoTarjeta.Visible = false;
                        break;

                    }
                }

                //cargo el nuevo pedido a la sesion
                lstPago = dt;

                //vuelvo a cargar los items
                this.cargarTablaPAgos();

                this.lblMontoCuotas.Text = "";
                //this.lblSaldoTarjeta.Text = this.lblMontoOriginal.Text;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando pago. " + ex.Message));
            }
        }
        private int SumarRecargoTarjeta(DataTable dt)
        {
            try
            {
                //decimal montoFinalRecargo = 0;
                //decimal montoOriginal = 0;
                decimal montoFinalRecargo = Convert.ToDecimal(this.lblSaldoTarjeta.Text);
                decimal montoOriginal = Convert.ToDecimal(this.lblMontoOriginal.Text);
                //decimal recargo = decimal.Round(montoFinalRecargo / montoOriginal, 3, MidpointRounding.AwayFromZero);
                decimal recargo = (montoFinalRecargo / montoOriginal);

                //foreach (DataRow row in dt.Rows)
                //{
                //    if (row["Tipo Pago"].ToString() != "Efectivo")
                //    {
                //        montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //        montoOriginal += Convert.ToDecimal(row["Neto"]);
                //    }
                //    //montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //}
                //calculo cuanto del total de la factura pago con tarjeta, en caso de que sea parte en tarjeta y otra en efectivo
                //decimal porcentajeOriginal = montoOriginal / Convert.ToDecimal(this.lblMontoOriginal.Text);
                //decimal recargo = Decimal.Round((( (montoFinalRecargo * 100) / montoOriginal ) / 100),2,MidpointRounding.AwayFromZero);

                Factura f = Session["Factura"] as Factura;

                foreach (ItemFactura item in f.items)
                {
                    item.precioSinIva = Decimal.Round(item.precioSinIva * recargo, 2, MidpointRounding.AwayFromZero);//(1 + (t.recargo / 100));                    
                    item.precioUnitario = Decimal.Round(item.precioUnitario * recargo, 2, MidpointRounding.AwayFromZero);// (1 + (t.recargo / 100));
                    //item.precioSinIva = Decimal.Round(item.precioSinIva * porcentajeOriginal * recargo, 2, MidpointRounding.AwayFromZero) + Decimal.Round(item.precioSinIva * (1 - porcentajeOriginal), 2, MidpointRounding.AwayFromZero);                    
                    //item.precioUnitario = Decimal.Round(item.precioSinIva * (1 + (item.articulo.porcentajeIva / 100)), 2, MidpointRounding.AwayFromZero);
                    item.total = Decimal.Round((item.precioUnitario * item.cantidad) * (1 - (item.porcentajeDescuento / 100)), 2, MidpointRounding.AwayFromZero);
                    item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2, MidpointRounding.AwayFromZero);
                }

                Session.Add("Factura", f);

                //lo dibujo en pantalla
                this.cargarItems();
                //actualizo totales
                this.actualizarTotales();
                //this.txtImporteEfectivo.Text = decimal.Round(f.total, 2).ToString();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private int QuitarRecargoTarjeta(DataTable dt)
        {
            try
            {
                //decimal montoFinalRecargo = 0;
                //decimal montoOriginal = 0;

                //foreach (DataRow row in dt.Rows)
                //{
                //    if (row["Tipo Pago"].ToString() != "Efectivo")
                //    {
                //        montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //        montoOriginal += Convert.ToDecimal(row["Neto"]);
                //    }
                //    //montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //    //montoOriginal += Convert.ToDecimal(row["neto"]);
                //}
                //calculo cuanto del total de la factura pago con tarjeta, en caso de que sea parte en tarjeta y otra en efectivo
                //decimal porcentajeOriginal = montoOriginal / Convert.ToDecimal(this.lblMontoOriginal.Text);
                //decimal recargo = Decimal.Round((((montoFinalRecargo * 100) / montoOriginal) / 100),2,MidpointRounding.AwayFromZero);

                Factura f = Session["Factura"] as Factura;

                foreach (ItemFactura item in f.items)
                {
                    //item.precioSinIva = Decimal.Round((item.precioSinIva / recargo),2);
                    //item.precioUnitario = Decimal.Round(( item.precioUnitario / recargo),2);
                    item.precioSinIva = item.precioSinRecargo;
                    item.precioUnitario = item.precioVentaSinRecargo;
                    item.total = Decimal.Round(((item.precioUnitario * item.cantidad) / (1 - (item.porcentajeDescuento / 100))), 2);
                    item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2);
                }

                Session.Add("Factura", f);

                //lo dibujo en pantalla
                this.cargarItems();
                //actualizo totales
                this.actualizarTotales();
                //this.txtImporteEfectivo.Text = decimal.Round(f.total, 2).ToString();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        protected void lbtnConfirmarPago_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = this.lstPago;
                Tarjeta t = ct.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas.SelectedValue));
                this.txtImporteEfectivo.Text = "0.00";

                if (t.recargo > 0)
                {
                    int i = this.SumarRecargoTarjeta(dt);
                    if (i > 0)
                    {
                        this.lbtnCancelarPago.Visible = true;

                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Recargos aplicados a factura.. \", {type: \"info\"});cerrarModalTarjeta();", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"No se pudo aplicar recargo/s a factura!. \");", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Pagos cargados con exito. \", {type: \"info\"});cerrarModalTarjeta();", true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnCancelarPago_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = lstPago;
                Tarjeta t = ct.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    if (t.recargo > 0)
                    {
                        int i = this.QuitarRecargoTarjeta(dt);
                        if (i > 0)
                        {
                            dt.Clear();
                            this.cargarTablaPAgos();
                            lstPago = dt;
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Proceso concluido con exito. \", {type: \"info\"});", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"No se pudo quitar recargo/s a factura!. \");", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Pagos removidos con exito. \", {type: \"info\"});", true);
                    }
                }
            }
            catch
            {

            }
        }
        private void verificarPromocionTarjeta()
        {
            try
            {

                //miro si esta en alguna promocion 
                ControladorArticulosEntity contEnt = new ControladorArticulosEntity();
                Gestion_Api.Entitys.Promocione p = contEnt.obtenerPromocionValidaTarjeta(Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.DropListFormaPago.SelectedValue), Convert.ToInt32(this.DropListLista.SelectedValue), Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")), Convert.ToInt32(ListTarjetas.SelectedValue));
                if (p != null)
                {
                    this.txtImporteEfectivo.Attributes.Add("disabled", "disabled");
                    this.txtPorcDescuento.Text = p.Descuento.Value.ToString();
                    this.actualizarTotales();
                    this.txtImporteT.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                    this.lblMontoOriginal.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                    this.txtImporteEfectivo.Text = "0";
                    this.lbtnAgregarEfectivo.Visible = false;

                    this.lblAvisoPromocion.Text = "Tarjeta en Promocion " + p.Descuento.Value.ToString() + "% de dto.";
                    this.lblAvisoPromocion.Visible = true;
                }
                else
                {
                    this.txtImporteEfectivo.Attributes.Remove("disabled");
                    this.txtPorcDescuento.Text = "0";
                    this.actualizarTotales();
                    this.txtImporteT.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                    this.lblMontoOriginal.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                    this.txtImporteEfectivo.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                    this.lblAvisoPromocion.Text = "";
                    this.lbtnAgregarEfectivo.Visible = true;
                    this.lblAvisoPromocion.Visible = false;
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Funciones cambio documento
        protected void lbtnFC_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroFactura();
                    this.lbtnAccion.Text = "FC <span class='caret'></span>";
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroFacturaInicio();
                }
            }
            catch
            {

            }
        }

        protected void lbtnPRP_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroPresupuesto();
                    this.lbtnAccion.Text = "PRP <span class='caret'></span>";
                    this.actualizarTotales();
                }
            }
            catch
            {

            }
        }

        protected void lbtnNC_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroNotaCredito();
                    this.lbtnAccion.Text = "NC <span class='caret'></span>";
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroNotaCreditoInicio();
                }
            }
            catch
            {

            }
        }

        protected void lbNC_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroNotaCreditoPresupuesto();
                    this.lbtnAccion.Text = "NC PRP <span class='caret'></span>";
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroNotaCreditoInicio();
                }
            }
            catch
            {

            }
        }

        protected void lbND_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroNotaDebitoPresupuesto();
                    this.lbtnAccion.Text = "ND PRP <span class='caret'></span>";
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroNotaDebitoInicio();
                }
            }
            catch
            {

            }
        }

        protected void lbtnND_Click(object sender, EventArgs e)
        {
            try
            {
                string[] cliente = this.labelCliente.Text.Split('-');
                if (cliente != null)
                {
                    this.obtenerNroNotaDebito();
                    this.lbtnAccion.Text = "ND <span class='caret'></span>";
                    this.actualizarTotales();
                }
                else
                {
                    this.obtenerNroNotaDebitoInicio();
                }
            }
            catch
            {

            }
        }

        protected void btnCierreZ_Click(object sender, EventArgs e)
        {
            try
            {
                int empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int puntoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                int i = this.controlador.generarCierreZ(empresa, sucursal, puntoVenta);
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre Realizado. ", "ABMFacturasLargo.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo realizar cierre. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error HAciendo Cierre Z. " + ex.Message));
            }
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("ABMFacturasLargo.aspx");
        }

        #endregion

        #region impresion , mail y sms

        private void ImprimirFactura(int idFactura, int tipo, int remito)
        {
            try
            {
                string script;

                //obtengo numero factura
                controladorDocumentos contDocumentos = new controladorDocumentos();

                TipoDocumento tp = contDocumentos.obtenerTipoDoc("Presupuesto");

                if (tipo == tp.id || tipo == 11 || tipo == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    script = "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "','_blank');";
                }
                else
                {
                    if (tipo == 1 || tipo == 9 || tipo == 4 || tipo == 24 || tipo == 25 || tipo == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        script = "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', '_blank');";
                    }
                    else//Factura B o cualquier otro.
                    {
                        script = "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "','_blank');";
                    }
                }

                if (remito > 0)
                {
                    script += " window.open('ImpresionPresupuesto.aspx?a=3&Presupuesto=" + remito + "&o=1','_blank');";
                }

                script += " $.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturasImagenes.aspx';";

                ScriptManager.RegisterClientScriptBlock(this.updatePanelModoImagen, updatePanelModoImagen.GetType(), "alert", script, true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir factura. " + ex.Message));
                //Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        private void EnviarFacturaMail(Factura f)
        {
            try
            {
                this.GenerarImpresionPDF(f);
            }
            catch
            {

            }
        }
        private void GenerarImpresionPDF(Factura f)
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();
                controladorCobranza contCobranza = new controladorCobranza();

                string destinatarios = this.txtMailEntrega.Text;

                if (f.tipo.tipo.Contains("Factura A") || f.tipo.tipo.Contains("Debito A") || f.tipo.tipo.Contains("Credito A")
                    || f.tipo.tipo.Contains("Factura E") || f.tipo.tipo.Contains("Debito E") || f.tipo.tipo.Contains("Credito E"))
                {
                    #region Fact A/E
                    //obtengo detalle de items
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);

                    //datos de encabezado y pie
                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    //nro remito factura
                    DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(f.id);

                    //Factura fact = controlador.obtenerFacturaId(idPresupuesto);

                    //datos del emisor
                    String razonSoc = String.Empty;
                    String direComer = String.Empty;
                    String condIVA = String.Empty;
                    String ingBrutos = String.Empty;
                    String fechaInicio = String.Empty;
                    String cuitEmpresa = String.Empty;
                    String nroFactura = String.Empty;
                    String tipoDoc = String.Empty;
                    String letraDoc = String.Empty;
                    String CodigoDoc = String.Empty;
                    String CAE = String.Empty;
                    String ptoVta = String.Empty;
                    String codBarra = String.Empty;
                    String fechaVto = string.Empty;

                    //levanto los datos de la factura
                    var drDatosFactura = dtDetalle.Rows[0];
                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }
                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        controladorSucursal contSuc = new controladorSucursal();
                        Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }


                    //datos empresa emisora
                    DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                    foreach (DataRow row in dtEmpresa.Rows)
                    {
                        //verifico cual es la empresa de la factura
                        //if ((int)row[0] == )
                        //{
                        cuitEmpresa = row.ItemArray[1].ToString();
                        razonSoc = row.ItemArray[2].ToString();
                        ingBrutos = row.ItemArray[3].ToString();
                        fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();
                        //fechaInicio = Convert.ToDateTime(fechaInicio).ToShortDateString();
                        condIVA = row.ItemArray[5].ToString();
                        direComer = row.ItemArray[6].ToString();
                        //}
                    }

                    //datos factura
                    string auxNro = drDatosFactura["Numero"].ToString();
                    nroFactura = auxNro.Substring(auxNro.Length - 13, 13);
                    //nombre tipo documento para el parametro
                    tipoDoc = auxNro.Substring(0, auxNro.Length - 16);
                    //letra y cod. factura                
                    if (tipoDoc == "Factura E ")
                    {
                        letraDoc = "E";
                        CodigoDoc = "Cod. 19";
                    }
                    else
                    {
                        letraDoc = "A";
                        CodigoDoc = "Cod. 01";
                    }

                    if (drDatosFactura["Cae"] != null)
                    {
                        CAE = drDatosFactura["Cae"].ToString();
                        //CAE = "-";
                    }
                    else
                    {
                        CAE = "-";
                    }
                    ptoVta = drDatosFactura["ptoVenta"].ToString();
                    fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                    codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                    if (string.IsNullOrEmpty(codBarra))
                    {
                        codBarra = "0";

                    }

                    //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                    PuntoVenta pv = this.cs.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                    int esPreimpresa = 0;
                    if (pv != null)
                    {
                        if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                        {
                            esPreimpresa = 1;
                        }
                    }

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];

                    //neto no grabado
                    decimal subtotal = Convert.ToDecimal(dr[4]);

                    decimal descuento = Convert.ToDecimal(dr[1]);

                    //subtotal menos el descuento
                    decimal subtotal2 = Convert.ToDecimal(dr[5]);

                    //iva discriminado de la fact
                    decimal iva = Convert.ToDecimal(dr[3]);

                    //IIBB (retenciones)
                    decimal retencion = Convert.ToDecimal(dr["retenciones"]);

                    //total de la factura
                    decimal total = Convert.ToDecimal(dr[2]);

                    //letras
                    string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                    //string totalS = Numalet.ToCardinal("18.25");

                    //cant unidades
                    decimal cant = 2;

                    //Total equivalente en dolares
                    controladorMoneda contMoneda = new controladorMoneda();
                    Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                    decimal TotalDolares = 0;
                    String textoDolares = String.Empty;
                    if (dolar != null)
                    {
                        TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                    }
                    if (tipoDoc.Contains("Nota de"))
                    {
                        textoDolares = " ";
                    }
                    else
                    {
                        textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                    }

                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    if (dtDireccion != null)
                    {
                        foreach (DataRow drl in dtDireccion.Rows)
                        {
                            if (drl[0].ToString() == "Legal")
                            {
                                direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                    drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                            if (drl[0].ToString() == "Entrega")
                            {
                                direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                    drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                        }
                    }

                    decimal saldoCtaCte = 0;
                    try
                    {
                        //obtengo el saldo de la cuenta corriente del cliente                
                        DataTable dt = contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), f.fecha.AddHours(23).ToString("dd/MM/yyyy"), f.cliente.id, 0, f.sucursal.id, 1, 0);
                        saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                    }
                    catch { }

                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //obtengo id empresa para buscar el logo correspondiente
                    int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                    string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/Logo.jpg");
                    Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);
                    BarcodeProfessional bcp = new BarcodeProfessional();

                    //Barcode settings
                    bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;
                    bcp.BarHeight = 0.25f;
                    bcp.Code = codBarra;

                    byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                    DataTable dtImagen = new DataTable();

                    DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));

                    dtImagen.Columns.Add(ColumnImagen);
                    dtImagen.Rows.Add(prodBarcode);
                    //Generate the barcode image and store it into the Barcode Column

                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    if (letraDoc == "A")
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                    }
                    if (letraDoc == "E")
                    {
                        //letras
                        totalS = Numalet.ToCardinal(subtotal2.ToString().Replace(',', '.'));
                        if (CAE == "-")
                        {
                            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE_2.rdlc");
                        }
                        else
                        {
                            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE.rdlc");
                        }

                    }
                    //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                    this.ReportViewer1.LocalReport.EnableExternalImages = true;

                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                    ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                    ReportParameter param31 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                    //logo
                    //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///C:\Imagen\Logo.jpg");
                    ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                    Log.EscribirSQL(1, "INFO", @"Asigno Ruta file:///" + logo);

                    //string imagePath = Server.MapPath("~/images/Facturas/GS_LOGO.png");
                    //ReportParameter paramImg = new ReportParameter("ParamImagen", imagePath);

                    ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                    ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                    ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                    ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                    ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                    ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                    ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                    ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                    ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                    ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                    ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                    ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                    ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                    ReportParameter param19 = new ReportParameter("ParamPtoVta", ptoVta);
                    ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);
                    ReportParameter param21 = new ReportParameter("ParamLetra", letraDoc);
                    ReportParameter param22 = new ReportParameter("ParamCodDoc", CodigoDoc);
                    ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);
                    ReportParameter param24 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());

                    ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());
                    this.ReportViewer1.LocalReport.SetParameters(param33);
                    //ReportParameter param16 = new ReportParameter("ParamRazonSoc", nroFactura);


                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param03);//sucfacturada
                    this.ReportViewer1.LocalReport.SetParameters(param31);
                    this.ReportViewer1.LocalReport.SetParameters(param4);

                    this.ReportViewer1.LocalReport.SetParameters(param3b);
                    this.ReportViewer1.LocalReport.SetParameters(param4b);

                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);

                    this.ReportViewer1.LocalReport.SetParameters(param7);
                    this.ReportViewer1.LocalReport.SetParameters(param8);

                    //parametros datos empresa
                    this.ReportViewer1.LocalReport.SetParameters(param10);
                    this.ReportViewer1.LocalReport.SetParameters(param11);
                    this.ReportViewer1.LocalReport.SetParameters(param12);
                    this.ReportViewer1.LocalReport.SetParameters(param13);
                    this.ReportViewer1.LocalReport.SetParameters(param14);
                    this.ReportViewer1.LocalReport.SetParameters(param15);
                    this.ReportViewer1.LocalReport.SetParameters(param16);
                    this.ReportViewer1.LocalReport.SetParameters(param17);
                    this.ReportViewer1.LocalReport.SetParameters(param18);
                    this.ReportViewer1.LocalReport.SetParameters(param19);
                    this.ReportViewer1.LocalReport.SetParameters(param20);
                    this.ReportViewer1.LocalReport.SetParameters(param21);
                    this.ReportViewer1.LocalReport.SetParameters(param22);
                    //equivalente total dolares
                    this.ReportViewer1.LocalReport.SetParameters(param23);
                    //param esPreimpresa o no
                    this.ReportViewer1.LocalReport.SetParameters(param24);
                    //imagen
                    this.ReportViewer1.LocalReport.SetParameters(param32);

                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                    FileStream stream = File.Create(path, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    Attachment adjunto = new Attachment(path);

                    int i = contFunciones.enviarMailFactura(adjunto, f, destinatarios);
                    if (i > 0)
                    {
                        adjunto.Dispose();
                        File.Delete(path);
                    }
                    #endregion
                }
                if (f.tipo.tipo.Contains("Factura B") || f.tipo.tipo.Contains("Debito B") || f.tipo.tipo.Contains("Credito B"))
                {
                    #region Fact B
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);
                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    //nro remito factura
                    DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(f.id);

                    //levanto los datos de la factura
                    var drDatosFactura = dtDetalle.Rows[0];
                    //datos empresa emisora
                    DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                    String razonSoc = String.Empty;
                    String direComer = String.Empty;
                    String condIVA = String.Empty;
                    String ingBrutos = String.Empty;
                    String fechaInicio = String.Empty;
                    String cuitEmpresa = String.Empty;
                    String nroFactura = String.Empty;
                    String tipoDoc = String.Empty;
                    String CAE = String.Empty;
                    String ptoVta = String.Empty;
                    String codBarra = String.Empty;
                    String fechaVto = String.Empty;

                    foreach (DataRow row in dtEmpresa.Rows)
                    {
                        //verifico cual es la empresa de la factura
                        //if ((int)row[0] == )
                        //{
                        cuitEmpresa = row.ItemArray[1].ToString();
                        razonSoc = row.ItemArray[2].ToString();
                        ingBrutos = row.ItemArray[3].ToString();
                        fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();
                        //fechaInicio = Convert.ToDateTime(fechaInicio).ToShortDateString();
                        condIVA = row.ItemArray[5].ToString();
                        direComer = row.ItemArray[6].ToString();
                        //}
                    }

                    //datos factura
                    string auxNro = drDatosFactura["Numero"].ToString();
                    nroFactura = auxNro.Substring(auxNro.Length - 13, 13);

                    tipoDoc = auxNro.Substring(0, auxNro.Length - 16);

                    if (drDatosFactura["Cae"].ToString() != "")
                    {
                        CAE = drDatosFactura["Cae"].ToString();
                        //CAE = "-";
                    }
                    else
                    {
                        CAE = "-";
                    }
                    ptoVta = drDatosFactura["ptoVenta"].ToString();
                    fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                    codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                    if (string.IsNullOrEmpty(codBarra))
                    {
                        codBarra = "0";

                    }

                    //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                    PuntoVenta pv = this.cs.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                    int esPreimpresa = 0;
                    if (pv != null)
                    {
                        if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                        {
                            esPreimpresa = 1;
                        }
                    }

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }

                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        controladorSucursal contSuc = new controladorSucursal();
                        Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];

                    //neto no grabado
                    decimal subtotal = Convert.ToDecimal(dr[4]);

                    //subtotal menos el descuento
                    decimal subtotal2 = Convert.ToDecimal(dr[5]);

                    //iva discriminado de la fact
                    decimal iva = Convert.ToDecimal(dr[3]);

                    subtotal = subtotal + iva;

                    //total de la factura
                    decimal total = Convert.ToDecimal(dr[2]);

                    //retenciones
                    decimal retencion = Convert.ToDecimal(dr[6]);

                    //percepcion IVA Cons. Final
                    decimal percepIVA = Convert.ToDecimal(dr[8]);

                    //decimal descuento = Convert.ToDecimal(dr[1]);
                    //sumo el total de items - total de factura y saco el descuento
                    DataTable dtDescuento = controlador.obtenerTotalItem2(f.id);
                    decimal descuento = 0;
                    foreach (DataRow drr in dtDescuento.Rows)
                    {
                        descuento = Convert.ToDecimal(drr[0]);
                    }

                    descuento = decimal.Round(((descuento + retencion) - total), 2);
                    if (Math.Abs(descuento) == Convert.ToDecimal(0.01))
                    {
                        descuento = 0;
                    }

                    //letras
                    string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                    //string totalS = Numalet.ToCardinal("18.25");

                    //cant unidades
                    decimal cant = 2;

                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    if (dtDireccion != null)
                    {
                        foreach (DataRow drl in dtDireccion.Rows)
                        {
                            if (drl[0].ToString() == "Legal")
                            {
                                //direLegal = "-";
                                direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                            if (drl[0].ToString() == "Entrega")
                            {
                                //direEntrega = "";
                                direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                        }
                    }
                    if (direLegal != "-" && direEntrega == "-")
                    {
                        direEntrega = direLegal;
                    }

                    //Total equivalente en dolares
                    controladorMoneda contMoneda = new controladorMoneda();
                    Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                    decimal TotalDolares = 0;
                    String textoDolares = String.Empty;
                    if (dolar != null)
                    {
                        TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                    }
                    if (tipoDoc.Contains("Nota de"))
                    {
                        textoDolares = " ";
                    }
                    else
                    {
                        textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                    }


                    decimal saldoCtaCte = 0;
                    try
                    {
                        //obtengo el saldo de la cuenta corriente del cliente                
                        DataTable dt = contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), f.fecha.AddHours(23).ToString("dd/MM/yyyy"), f.cliente.id, 0, f.sucursal.id, 1, 0);
                        saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                    }
                    catch { }

                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //obtengo id empresa para buscar el logo correspondiente
                    int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                    //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/Logo.jpg");
                    string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/Logo.jpg");
                    //codigo barra codBarra
                    //Create an instance of Barcode Professional
                    BarcodeProfessional bcp = new BarcodeProfessional();

                    //Barcode settings

                    bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;

                    bcp.BarHeight = 0.25f;
                    bcp.Code = codBarra;

                    byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                    DataTable dtImagen = new DataTable();

                    DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));

                    dtImagen.Columns.Add(ColumnImagen);

                    //DataRow drImagen = dtImagen.NewRow();
                    // object [] rowArray = new object[1];
                    // rowArray.SetValue(prodBarcode, 0);

                    //drImagen.ItemArray = rowArray;
                    //drImagen. = prodBarcode;

                    dtImagen.Rows.Add(prodBarcode);
                    //Generate the barcode image and store it into the Barcode Column


                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRB.rdlc");
                    this.ReportViewer1.LocalReport.EnableExternalImages = true;
                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("DatosPresupuesto", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                    ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param3a = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                    ReportParameter param3a2 = new ReportParameter("ParamPercepIVA", percepIVA.ToString("C"));//percepIVA
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                    ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                    param3b.Visible = false;
                    ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));
                    param4b.Visible = false;

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                    ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                    ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                    //parametros Datos empresa,cae y doc
                    ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                    ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                    ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                    ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                    ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                    ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                    ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                    ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                    ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                    ReportParameter param19 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());
                    ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);

                    ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);

                    ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                    this.ReportViewer1.LocalReport.SetParameters(param32);

                    ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());
                    this.ReportViewer1.LocalReport.SetParameters(param33);

                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param3a);//retencion
                    this.ReportViewer1.LocalReport.SetParameters(param3a2);//percepcion iva cf
                    this.ReportViewer1.LocalReport.SetParameters(param03);
                    this.ReportViewer1.LocalReport.SetParameters(param4);

                    this.ReportViewer1.LocalReport.SetParameters(param3b);
                    this.ReportViewer1.LocalReport.SetParameters(param4b);

                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);

                    this.ReportViewer1.LocalReport.SetParameters(param7);
                    this.ReportViewer1.LocalReport.SetParameters(param8);

                    //parametros datos empresa
                    this.ReportViewer1.LocalReport.SetParameters(param10);
                    this.ReportViewer1.LocalReport.SetParameters(param11);
                    this.ReportViewer1.LocalReport.SetParameters(param12);
                    this.ReportViewer1.LocalReport.SetParameters(param13);
                    this.ReportViewer1.LocalReport.SetParameters(param14);
                    this.ReportViewer1.LocalReport.SetParameters(param15);
                    this.ReportViewer1.LocalReport.SetParameters(param16);
                    this.ReportViewer1.LocalReport.SetParameters(param17);
                    this.ReportViewer1.LocalReport.SetParameters(param18);
                    this.ReportViewer1.LocalReport.SetParameters(param19);
                    this.ReportViewer1.LocalReport.SetParameters(param20);

                    this.ReportViewer1.LocalReport.SetParameters(param23);

                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                    FileStream stream = File.Create(path, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    Attachment adjunto = new Attachment(path);

                    int i = contFunciones.enviarMailFactura(adjunto, f, destinatarios);
                    if (i > 0)
                    {
                        adjunto.Dispose();
                        File.Delete(path);
                    }

                    #endregion
                }
                if (f.tipo.tipo.Contains("Presupuesto") || f.tipo.tipo.Contains("PRP"))
                {
                    #region presupuesto
                    //obtengo detalle de items
                    //DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);

                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];
                    decimal subtotal = Convert.ToDecimal(dr[0]);

                    decimal descuento = Convert.ToDecimal(dr[1]);
                    decimal total = Convert.ToDecimal(dr[2]);


                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl[0].ToString() == "Legal")
                        {
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString();
                        }
                        if (drl[0].ToString() == "Entrega")
                        {
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString();
                        }
                    }

                    //sucursal venta
                    string sucursalOrigen = dtDetalle.Rows[0]["Sucursal"].ToString();
                    Sucursal sucvta = this.cs.obtenerSucursalID(Convert.ToInt32(sucursalOrigen));
                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        Sucursal s = this.cs.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }

                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }

                    //string logo = Server.MapPath("../../Images/Logo.jpg");

                    //Cargo configuracion para mostrar Precio de venta con o sin IVA.
                    Configuracion c = new Configuracion();

                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    if (c.porcentajeIva != "0")
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                    }
                    else
                    {
                        //subtotal sin iva
                        subtotal = Convert.ToDecimal(dr[4]);
                        total = subtotal - descuento;
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2SinIva.rdlc");
                    }

                    decimal saldoCtaCte = 0;
                    try
                    {
                        //obtengo el saldo de la cuenta corriente del cliente                
                        DataTable dt = contCobranza.obtenerTablaTopClientes(DateTime.Today.ToString("dd/MM/yyyy"), f.fecha.AddHours(23).ToString("dd/MM/yyyy"), f.cliente.id, 0, f.sucursal.id, 1, 0);
                        saldoCtaCte = Convert.ToDecimal(dt.Rows[0]["importe"].ToString());
                    }
                    catch { }

                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("ItemsPresupuesto", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("DatosPresupuesto", dtDatos);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);


                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact
                    ReportParameter param04 = new ReportParameter("ParamSucursal", sucvta.nombre);//sucursalVta

                    //ReportParameter param32 = new ReportParameter("Porcentaje", porcentaje.ToString("N"));

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                    //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                    //this.ReportViewer1.LocalReport.SetParameters(param32);
                    ReportParameter param33 = new ReportParameter("ParamSaldoCtaCte", saldoCtaCte.ToString());
                    this.ReportViewer1.LocalReport.SetParameters(param33);

                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);

                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param03);
                    this.ReportViewer1.LocalReport.SetParameters(param04);

                    this.ReportViewer1.LocalReport.SetParameters(param4);
                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);
                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                    FileStream stream = File.Create(path, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    Attachment adjunto = new Attachment(path);

                    int i = contFunciones.enviarMailFactura(adjunto, f, destinatarios);
                    if (i > 0)
                    {
                        adjunto.Dispose();
                        File.Delete(path);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "No se pudo generar pdf para enviar factura por correo." + ex.Message);
            }
        }
        private void ImprimirFacturaPorcentuales(Factura fc, Factura prp, int generaRemito, int idRemito)
        {
            try
            {
                string script;
                string script2;
                string script3;
                string scriptRemito = "";

                if (fc.tipo.tipo.Contains("Factura A") || fc.tipo.tipo.Contains("Debito A") || fc.tipo.tipo.Contains("Credito A")
                    || fc.tipo.tipo.Contains("Factura E") || fc.tipo.tipo.Contains("Debito E") || fc.tipo.tipo.Contains("Credito E"))
                {
                    //factura
                    script = "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + fc.id + "', '_blank');";
                }
                else//Factura B o cualquier otro.
                {
                    script = "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + fc.id + "','_blank');";
                }
                if (idRemito > 0)
                {
                    scriptRemito = " window.open('ImpresionPresupuesto.aspx?a=3&Presupuesto=" + idRemito + "&o=1','_blank');";
                }

                script2 = "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + prp.id + "','_blank');";
                script3 = " $.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturasLargo.aspx';";

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script + script2 + script3 + scriptRemito, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir factura. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        private void EnviarSMSAviso(Factura fc)
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();
                ControladorConfiguracion contConfig = new ControladorConfiguracion();
                Gestion_Api.Entitys.Configuraciones_SMS c = contConfig.ObtenerConfiguracionesAlertasSMS();

                if (c != null)
                {
                    if (c.Estado == 1)
                    {
                        contSMS.enviarAlertaFc(fc, (int)Session["Login_IdUser"]);
                    }
                }
                return;
            }
            catch
            {

            }
        }
        private void EnviarSMSAvisoSaldoMax()
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();
                ControladorConfiguracion contConfig = new ControladorConfiguracion();
                Gestion_Api.Entitys.Configuraciones_SMS c = contConfig.ObtenerConfiguracionesAlertasSMS();

                if (c != null)
                {
                    if (c.Estado == 1 && c.AlertaSaldoMax == 1 && !String.IsNullOrEmpty(c.MensajeSaldoMax))
                    {
                        if (this.txtCodArea.Text.Length + this.txtTelefono.Text.Length != 10)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel11, UpdatePanel11.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");", true);
                            return;
                        }
                        string telefono = "+549" + this.txtCodArea.Text + this.txtTelefono.Text;
                        string textoSMS = Regex.Replace(this.txtMensajeSMS.Text, @"\t|\n|\r", " ");
                        this.guardarNumeroTelefonoCliente(this.txtCodArea.Text + "-" + this.txtTelefono.Text, Convert.ToInt32(this.DropListClientes.SelectedValue));
                        int i = contSMS.enviarAlertaSaldoMax(telefono, textoSMS, (int)Session["Login_IdUser"], Convert.ToInt32(this.DropListClientes.SelectedValue));
                        if (i > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Aviso enviado con exito!. ", ""));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar aviso. "));
                        }
                    }
                }
                return;
            }
            catch
            {

            }
        }
        protected void lbtnEnviarSMSAviso_Click(object sender, EventArgs e)
        {
            try
            {
                this.EnviarSMSAvisoSaldoMax();
            }
            catch
            {

            }
        }
        private int verificarMostrarAccionSMS()
        {
            try
            {
                ControladorConfiguracion controlConfig = new ControladorConfiguracion();
                Gestion_Api.Entitys.Configuraciones_SMS c = controlConfig.ObtenerConfiguracionesAlertasSMS();
                if (c != null)
                {
                    if (c.Estado == 1 && c.AlertaSaldoMax == 1 && !String.IsNullOrEmpty(c.MensajeSaldoMax))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        private void guardarNumeroTelefonoCliente(string telefono, int idCliente)
        {
            try
            {
                var datosMail = this.contClienteEntity.obtenerClienteDatosByCliente(idCliente);
                if (datosMail != null)
                {
                    if (datosMail.Count > 0)
                    {
                        datosMail.FirstOrDefault().Celular = telefono;
                        this.contClienteEntity.modificarClienteDatos(datosMail.FirstOrDefault());
                    }
                    else
                    {
                        Gestion_Api.Entitys.Cliente_Datos datos = new Gestion_Api.Entitys.Cliente_Datos();
                        datos.Mail = "";
                        datos.IdCliente = idCliente;
                        datos.Celular = telefono;
                        this.contClienteEntity.agregarClienteDatos(datos);
                    }
                }
            }
            catch
            {

            }
        }
        private int abrirModalEnvioSMS(int script)
        {
            try
            {
                int idC = Convert.ToInt32(this.DropListClientes.SelectedValue);
                int ok = this.verificarMostrarAccionSMS();
                if (ok < 1)
                {
                    return -1;
                }
                else
                {
                    ControladorConfiguracion contConfigSMS = new ControladorConfiguracion();
                    Gestion_Api.Entitys.Configuraciones_SMS config = contConfigSMS.ObtenerConfiguracionesAlertasSMS();
                    string texto = config.MensajeSaldoMax;
                    string cod = "";
                    string nro = "";

                    this.txtMensajeSMS.Text = texto;
                    this.txtIdEnvioSMS.Text = idC.ToString();

                    var mail = this.contClienteEntity.obtenerClienteDatosByCliente(idC);
                    if (mail != null && mail.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(mail.FirstOrDefault().Celular))
                        {
                            cod = mail.FirstOrDefault().Celular.Split('-')[0];
                            nro = mail.FirstOrDefault().Celular.Split('-')[1];
                        }
                    }

                    this.txtCodArea.Text = cod;
                    this.txtTelefono.Text = nro;

                    if (script == 0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "openModal", "openModal('" + cod + "','" + nro + "','" + texto + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "openModal", "openModal('" + cod + "','" + nro + "','" + texto + "');", true);

                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region trazabilidad
        private void CargarTrazasPH(int idArt)
        {
            try
            {
                Articulo a = this.contArticulo.obtenerArticuloByID(idArt);
                this.cargarCamposGrupoTrazabilidad(a);
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarCamposGrupoTrazabilidad(Articulo art)
        {
            try
            {
                phCamposTrazabilidad.Controls.Clear();

                List<Gestion_Api.Entitys.Trazabilidad_Campos> lstCampos = this.contArticulo.obtenerCamposTrazabilidadByGrupo(art.grupo.id);

                foreach (Gestion_Api.Entitys.Trazabilidad_Campos campos in lstCampos)
                {

                    TableHeaderCell th = new TableHeaderCell();
                    th.Text = campos.nombre;
                    phCamposTrazabilidad.Controls.Add(th);

                }
                this.CargarTrazasArticulo(art.id, lstCampos.Count);
            }
            catch (Exception ex)
            {

            }
        }
        private void CargarTrazasArticulo(int idArticulo, int cantCampos)
        {
            try
            {
                controladorCompraEntity contCompra = new controladorCompraEntity();
                this.phStockTrazabilidad.Controls.Clear();

                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                DataTable dt = new DataTable();

                //Si estoy generando una NC busco las trazabilidades que salieron con la factura o remito original
                if (this.accion == 6)
                {
                    string prp = Request.QueryString["facturas"];
                    Remito r = this.controlador.obtenerRemitoByFactura(Convert.ToInt32(prp.Split(';')[0]));
                    if (r.id > 0)
                    {
                        //si es una factura que tuvo remito
                        dt = contCompra.obtenerTrazabilidadVendidasItemByArticuloDoc(idArticulo, suc, r.id, 14);

                        //Verifico si mueve stock por factura y si el DataTable que me devuelve es vacio, si es asi, le digo que me traiga las trazabilidades asociadas a la factura
                        if (c.egresoStock == "0" && dt.Rows != null && dt.Rows.Count == 0)
                        {
                            dt = contCompra.obtenerTrazabilidadVendidasItemByArticuloDoc(idArticulo, suc, Convert.ToInt32(prp.Split(';')[0]), 0);
                        }
                    }
                    else
                    {
                        //sino la busco por id de factura
                        dt = contCompra.obtenerTrazabilidadVendidasItemByArticuloDoc(idArticulo, suc, Convert.ToInt32(prp.Split(';')[0]), 0);
                    }
                }
                else
                {
                    //si es facturacion normal busco las trazas disponibles para vender
                    if (!this.labelNroFactura.Text.Contains("Credito"))
                    {
                        dt = contCompra.obtenerTrazabilidadItemByArticulo(idArticulo, suc);
                    }
                    else
                    {
                        //busco todas las vendidas de esta sucursal de ese articulo para devolver
                        dt = contCompra.obtenerTrazabilidadVendidasItemByArticuloDoc(idArticulo, suc, 0, 0);
                    }
                }

                int pos = 0;
                int columnas = 0;
                TableRow tr = new TableRow();
                string idTrazas = "";

                foreach (DataRow row in dt.Rows)
                {
                    //this.cargarEnPH(row, pos);                    
                    if (columnas == 0)
                    {
                        tr = new TableRow();

                        TableCell celIndice = new TableCell();
                        celIndice.Text = (pos + 1).ToString();
                        celIndice.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celIndice);
                    }

                    if (columnas < cantCampos)
                    {
                        TableCell celCampo1 = new TableCell();
                        celCampo1.Text = row["valor"].ToString();
                        celCampo1.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celCampo1);
                        columnas++;
                        idTrazas += row["Id"].ToString() + "-";
                    }
                    if (columnas == (cantCampos))
                    {
                        TableCell celAccion = new TableCell();
                        CheckBox chkSeleccionT = new CheckBox();
                        chkSeleccionT.ID = "chkSeleccionT_" + idTrazas;
                        chkSeleccionT.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        chkSeleccionT.Attributes.Add("onchange", "javascript:return updatebox(" + 1 + ",'" + chkSeleccionT.ID + "');");
                        celAccion.Controls.Add(chkSeleccionT);

                        celAccion.Width = Unit.Percentage(5);
                        celAccion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celAccion);

                        columnas = 0;
                        pos++;
                        idTrazas = "";
                        phStockTrazabilidad.Controls.Add(tr);

                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }
        protected void AgregarTraza_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCompraEntity contCompra = new controladorCompraEntity();

                string idTraza = this.lblMovTraza.Text;
                int idArticulo = Convert.ToInt32(idTraza.Split('_')[1]);
                int posItem = Convert.ToInt32(idTraza.Split('_')[2]);

                Factura f = Session["Factura"] as Factura;
                this.lblTrazaTotal.Text = f.items[posItem].cantidad.ToString();

                string idtildado = "";
                foreach (Control control in phStockTrazabilidad.Controls)
                {
                    TableRow tr = control as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[0] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    List<Gestion_Api.Entitys.Trazabilidad> listaTrazas = new List<Gestion_Api.Entitys.Trazabilidad>();

                    foreach (string traza in idtildado.Split(';'))//Por cada traza seleccionada
                    {
                        if (!String.IsNullOrEmpty(traza))
                        {
                            foreach (string campo in traza.Split('-'))//Por cada valor campo de la traza
                            {
                                if (!String.IsNullOrEmpty(campo))
                                {
                                    int id = Convert.ToInt32(campo);
                                    Gestion_Api.Entitys.Trazabilidad t = this.contArticulo.ObtenerTrazabilidadByID(id);
                                    if (t != null)
                                    {
                                        listaTrazas.Add(t);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error cargando traza de articulo. \", {type: \"error\"});", true);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    //cargo de nuevo la factura con las trazas en los items
                    f.items[posItem].lstTrazas = listaTrazas;

                    Session["Factura"] = f;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Debe seleccionar al menos una. \");", true);
                    return;
                }

                this.cargarItems();
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel8, UpdatePanel8.GetType(), "alert", "$.msgbox(\"Traza seleccionada con exito!. \", {type: \"info\"}); cerrarModal(); ", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error cargando trazas. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private List<ItemFactura> agregarInfoTrazaFactura(Factura f)
        {
            try
            {
                int NroTraza = -1;
                foreach (ItemFactura item in f.items)
                {
                    if (item.lstTrazas.Count > 0 && item.lstTrazas != null)
                    {
                        foreach (Gestion_Api.Entitys.Trazabilidad traza in item.lstTrazas)
                        {
                            if (NroTraza != -1 && NroTraza != traza.Traza.Value)
                            {
                                item.articulo.descripcion += '\n' + "---";
                            }
                            if (!String.IsNullOrEmpty(traza.valor))
                            {
                                Gestion_Api.Entitys.Trazabilidad_Campos campo = this.contArticulo.obtenerCamposTrazabilidadById(Convert.ToInt32(traza.idCampo));
                                item.articulo.descripcion += '\n' + campo.nombre + ": " + traza.valor;
                            }
                            NroTraza = traza.Traza.Value;//NroTraza
                        }
                    }
                }
                return f.items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //carga de trazas nuevas desde item cantidad -1        
        private void cargarCamposGrupo(Articulo art)
        {
            try
            {
                this.phCampos.Controls.Clear();
                this.phCamposTrazaNueva.Controls.Clear();

                List<Gestion_Api.Entitys.Trazabilidad_Campos> lstCampos = this.contArticulo.obtenerCamposTrazabilidadByGrupo(art.grupo.id);

                foreach (Gestion_Api.Entitys.Trazabilidad_Campos campos in lstCampos)
                {
                    CampoDinamico campo = (CampoDinamico)Page.LoadControl("../../Controles/CampoDinamico.ascx");
                    campo.lblCampo.InnerText = campos.nombre;
                    phCampos.Controls.Add(campo);

                    TableHeaderCell th = new TableHeaderCell();
                    th.Text = campos.nombre;
                    phCamposTrazaNueva.Controls.Add(th);
                }

                //this.CrearTablaItems(lstCampos);
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarTrazasAgregadas()
        {
            try
            {
                this.phTrazabilidadNueva.Controls.Clear();
                Factura f = Session["Factura"] as Factura;
                string posItem = this.lbMovTrazaNueva.Text.Split('_')[2];

                ItemFactura item = f.items[Convert.ToInt32(posItem)];


                int cantCampos = phCamposTrazaNueva.Controls.Count;
                TableRow tr = new TableRow();
                int pos = 0;

                foreach (var traza in item.lstTrazas)
                {
                    if (pos == 0)
                    {
                        tr = new TableRow();
                        tr.ID = "tr_" + this.phTrazabilidadNueva.Controls.Count;
                    }

                    TableCell celCampo = new TableCell();
                    celCampo.Text = traza.valor;
                    tr.Controls.Add(celCampo);
                    pos++;

                    if (pos == cantCampos)
                    {
                        this.phTrazabilidadNueva.Controls.Add(tr);
                        pos = 0;
                    }
                }
                //this.phTrazabilidadNueva.Controls.Add(tr);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCargarTraza_Click(object sender, EventArgs e)
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                string posItem = this.lbMovTrazaNueva.Text.Split('_')[2];
                string idArti = this.lbMovTrazaNueva.Text.Split('_')[1];

                int NroTraza = 0;
                NroTraza = this.contArticulo.obtenerUltimoNumeroTrazaArticulo(f.items[Convert.ToInt32(posItem)].articulo.id);

                //Verifico que se haya completado el campo
                if (!this.validarCamposDinamicosVaciosTraza())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCargaTraza, UpdatePanelCargaTraza.GetType(), "alert", "$.msgbox(\"Debe completar todos los campos con al menos ún carácter. \");", true);
                    return;
                }

                foreach (CampoDinamico txt in phCampos.Controls)
                {

                    Gestion_Api.Entitys.Trazabilidad traza = new Gestion_Api.Entitys.Trazabilidad();
                    Gestion_Api.Entitys.Trazabilidad_Campos campo = this.contArticulo.obtenerCamposTrazabilidadByNombre(txt.lblCampo.InnerText, f.items[Convert.ToInt32(posItem)].articulo.grupo.id);

                    traza.idArticulo = Convert.ToInt32(idArti);
                    traza.idCampo = campo.id;
                    traza.valor = txt.txtCampo.Text;
                    traza.estado = 1;
                    traza.Sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    traza.Traza = NroTraza + 1;

                    f.items[Convert.ToInt32(posItem)].lstTrazas.Add(traza);
                    txt.txtCampo.Text = "";
                }

                Session["Factura"] = f;
                this.cargarTrazasAgregadas();
            }
            catch (Exception ex)
            {

            }
        }
        private bool validarCamposDinamicosVaciosTraza()
        {
            try
            {
                foreach (CampoDinamico txt in phCampos.Controls)
                {
                    if (string.IsNullOrEmpty(txt.txtCampo.Text))
                        return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCargaTraza, UpdatePanelCargaTraza.GetType(), "error", "$.msgbox(\"Ocurrió un error verificando campos dinamicos de traza. Excepción: " + Ex.Message + " \");", true);
                return false;
            }
        }
        protected void btnLimpiarTrazaNueva_Click(object sender, EventArgs e)
        {
            try
            {
                string posItem = this.lbMovTrazaNueva.Text.Split('_')[2];
                Factura f = Session["Factura"] as Factura;
                f.items[Convert.ToInt32(posItem)].lstTrazas.Clear();

                Session["Factura"] = f;
                this.cargarTrazasAgregadas();
            }
            catch
            {

            }
        }
        protected void lbtnConfirmarTrazas_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargarItems();
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCargaTraza, UpdatePanelCargaTraza.GetType(), "alert", "$.msgbox(\"Traza cargadas con exito!. \", {type: \"info\"}); cerrarModal2(); ", true);
            }
            catch
            {

            }
        }

        #endregion

        #region datos despacho

        private void agregarInfoDespachoItem(Articulo articulo)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                controladorPais contPais = new controladorPais();

                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(articulo.id);

                Factura fact = Session["Factura"] as Factura;
                //si es PRP omito la info de despachos
                if (fact.tipo.tipo.Contains("Presupuesto") || fact.tipo.tipo.Contains("PRP"))
                    return;

                if (art.Articulos_Despachos.Count > 0)
                {
                    Pais pais = contPais.obtenerPaisID(art.procedencia.Value);
                    var datos = art.Articulos_Despachos.FirstOrDefault();

                    if (datos.FechaDespacho != null)
                        this.txtDescripcion.Text += " |" + "Fecha despacho: " + datos.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    if (!String.IsNullOrEmpty(datos.NumeroDespacho))
                        this.txtDescripcion.Text += " |" + "D.I.: " + datos.NumeroDespacho;
                    if (!String.IsNullOrEmpty(datos.Lote))
                        this.txtDescripcion.Text += " |" + "Lote: " + datos.Lote;
                    if (!String.IsNullOrEmpty(datos.Vencimiento))
                        this.txtDescripcion.Text += " |" + "Vencimiento: " + datos.Vencimiento;
                    if (pais != null)
                        this.txtDescripcion.Text += " |" + "Procedencia: " + pais.descripcion;
                }

                return;
            }
            catch
            {
                return;
            }
        }
        private void agregarInfoDespachoDesdePedido(ItemFactura item)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                controladorPais contPais = new controladorPais();

                Factura fact = Session["Factura"] as Factura;
                //si es PRP omito la info de despachos
                if (fact.tipo.tipo.Contains("Presupuesto") || fact.tipo.tipo.Contains("PRP"))
                    return;

                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(item.articulo.id);
                if (art.Articulos_Despachos.Count > 0)
                {
                    Pais pais = contPais.obtenerPaisID(art.procedencia.Value);
                    var datos = art.Articulos_Despachos.FirstOrDefault();

                    if (datos.FechaDespacho != null && !item.articulo.descripcion.Contains("Fecha despacho:"))
                        item.articulo.descripcion += " |" + "Fecha despacho: " + datos.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    if (!String.IsNullOrEmpty(datos.NumeroDespacho) && !item.articulo.descripcion.Contains("D.I.:"))
                        item.articulo.descripcion += " |" + "D.I.: " + datos.NumeroDespacho;
                    if (!String.IsNullOrEmpty(datos.Lote) && !item.articulo.descripcion.Contains("Lote:"))
                        item.articulo.descripcion += " |" + "Lote: " + datos.Lote;
                    if (!String.IsNullOrEmpty(datos.Vencimiento) && !item.articulo.descripcion.Contains("Vencimiento:"))
                        item.articulo.descripcion += " |" + "Vencimiento: " + datos.Vencimiento;
                    if (pais != null && !item.articulo.descripcion.Contains("Procedencia:"))
                        item.articulo.descripcion += " |" + "Procedencia: " + pais.descripcion;
                }

                return;
            }
            catch
            {
                return;
            }
        }
        #endregion        

        #region CREDITOS
        private void buscarSolicitudes()
        {
            try
            {
                ControladorPlenario contPlena = new ControladorPlenario();
                List<SolicitudPlenario> solicitudes = contPlena.obtenerSolicitudesPlenarioFiltradas(this.txtDniCredito.Text);

                int pos = 0;
                this.phSolicitud.Controls.Clear();
                foreach (SolicitudPlenario s in solicitudes)
                {
                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = s.NroSolicitud.ToString();

                    TableCell celPos = new TableCell();
                    celPos.Text = (pos + 1).ToString();
                    celPos.Width = Unit.Percentage(10);
                    celPos.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celPos);

                    TableCell celFecha = new TableCell();
                    celFecha.Text = s.FechaOperacion.ToString("dd/MM/yyyy");
                    celFecha.Width = Unit.Percentage(20);
                    celFecha.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celFecha);

                    TableCell celNumero = new TableCell();
                    celNumero.Text = s.NroSolicitud.ToString();
                    celNumero.Width = Unit.Percentage(20);
                    celNumero.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNumero);

                    TableCell celCapital = new TableCell();
                    celCapital.Text = "$" + s.Capital;
                    celCapital.Width = Unit.Percentage(20);
                    celCapital.VerticalAlign = VerticalAlign.Middle;
                    celCapital.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celCapital);

                    TableCell celAnticipo = new TableCell();
                    celAnticipo.Text = "$" + s.Anticipo;
                    celAnticipo.Width = Unit.Percentage(20);
                    celAnticipo.VerticalAlign = VerticalAlign.Middle;
                    celAnticipo.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celAnticipo);

                    TableCell celAccion = new TableCell();
                    RadioButton rbtnSeleccion = new RadioButton();
                    rbtnSeleccion.ID = "rbtnSeleccion_" + s.NroSolicitud.ToString();
                    rbtnSeleccion.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    rbtnSeleccion.CssClass = "btn btn-info";
                    rbtnSeleccion.Font.Size = 12;
                    rbtnSeleccion.Attributes.Add("onchange", "javascript:return updateNroSolicitud(" + s.NroSolicitud.ToString() + ",'" + rbtnSeleccion.ID + "','" + s.Anticipo + "');");
                    rbtnSeleccion.GroupName = "GroupSolicitudes";
                    celAccion.Controls.Add(rbtnSeleccion);

                    //CheckBox chkSeleccion = new CheckBox();
                    //chkSeleccion.ID = "cbSeleccion_" + s.NroSolicitud.ToString();
                    //chkSeleccion.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    //chkSeleccion.CssClass = "btn btn-info";
                    //chkSeleccion.Font.Size = 12;
                    //chkSeleccion.Attributes.Add("onchange", "javascript:return updateNroSolicitud(" + s.NroSolicitud.ToString() + ",'" + chkSeleccion.ID + "');");
                    //celAccion.Controls.Add(chkSeleccion);

                    celAccion.Width = Unit.Percentage(10);
                    celAccion.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celAccion);

                    this.phSolicitud.Controls.Add(tr);
                    pos += 1;
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void guardarSolicitud()
        {
            try
            {
                string nroSolicitud = "";
                string datos = "";
                SolicitudPlenario solicitud = new SolicitudPlenario();
                ControladorPlenario contPlenario = new ControladorPlenario();

                string omitioValidacion = this.lblOmitioCodigoCredito.Text;
                string motivo = this.txtMotivoCredito.Text;
                if (omitioValidacion == "1" && (String.IsNullOrEmpty(motivo) || motivo.Length < 3))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Debe cargar un motivo de porque omitio validacion. \");", true);
                    return;
                }

                foreach (Control c in this.phSolicitud.Controls)
                {
                    TableRow tr = c as TableRow;
                    RadioButton rd = tr.Cells[5].Controls[0] as RadioButton;
                    if (rd.Checked == true)
                    {
                        nroSolicitud += rd.ID.Split('_')[1];
                        datos = "Solicitud nº " + nroSolicitud + ", DNI: " + this.txtDniCredito.Text + ", Fecha: " + tr.Cells[1].Text + ", Capital:" + tr.Cells[3].Text + ", Anticipo:" + tr.Cells[4].Text;
                        solicitud.Dni = this.txtDniCredito.Text;
                        solicitud.FechaOperacion = Convert.ToDateTime(tr.Cells[1].Text, new CultureInfo("es-AR"));
                        solicitud.NroSolicitud = Convert.ToInt32(tr.Cells[2].Text);
                        solicitud.Capital = Convert.ToDecimal(tr.Cells[3].Text.Replace("$", ""));
                        solicitud.Anticipo = Convert.ToDecimal(tr.Cells[4].Text.Replace("$", ""));
                        solicitud.Validada = 1;
                        solicitud.FechaValidacion = DateTime.Now;
                    }
                }
                if (!String.IsNullOrEmpty(nroSolicitud))
                {
                    int okSucursal = contPlenario.validarSucursalSolicitudPlenario(solicitud.Dni, solicitud.NroSolicitud, Convert.ToInt32(this.ListSucursal.SelectedValue));
                    if (okSucursal < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"La solicitud seleccionada no corresponde a esta sucursal. \");", true);
                        return;
                    }

                    Factura f = Session["Factura"] as Factura;

                    decimal diferencia = f.total - (solicitud.Capital + solicitud.Anticipo.Value);
                    if (Math.Abs(diferencia) > Convert.ToDecimal(10))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"El capital de la solicitud debe ser igual al monto de la factura. \");", true);
                        return;
                    }

                    if (solicitud.Anticipo.ToString() != this.txtAnticipo.Text)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Verifique el monto del anticipo cargado. \");", true);
                        return;
                    }
                    this.txtComentarios.Text += "\n " + datos;
                    f.NroSolicitud = nroSolicitud;
                    f.Solicitud = solicitud;
                    Session["Factura"] = f;
                    //lo marco como que se uso
                    contPlenario.editarEstadoRegistroTelefonoDniByID(Convert.ToInt32(this.lblIdRegistro.Text), 2);

                    if (omitioValidacion == "0")
                    {
                        //lo marco como que se uso
                        contPlenario.editarEstadoRegistroTelefonoDniByID(Convert.ToInt32(this.lblIdRegistro.Text), 2);
                        this.txtComentarios.Text += " - VALIDADO POR SMS";
                    }
                    else
                    {
                        if (this.lblIdRegistro.Text == "0")
                        {
                            CodigosTelefono registro = new CodigosTelefono();
                            registro.DNI = this.txtDniCredito.Text;
                            string telefono = "+549" + this.txtCodAreaCredito.Text + this.txtNroCelularCredito.Text;//+54 9 cod + tel
                            if (!String.IsNullOrEmpty(this.txtCodAreaCredito.Text) && !String.IsNullOrEmpty(this.txtNroCelularCredito.Text) && (this.txtCodAreaCredito.Text.Length + this.txtNroCelularCredito.Text.Length == 10))
                            {
                                registro.Telefono = telefono;
                            }
                            registro.FechaHora = DateTime.Now;
                            registro.Estado = 0;
                            registro.IdEmpresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                            registro.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                            registro.IdVendedor = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                            registro.Motivo = this.txtMotivoCredito.Text;
                            contPlenario.agregarCodigoTelefono(registro);
                            this.lblIdRegistro.Text = registro.Id.ToString();
                        }
                        else
                        {
                            Planario_Api.Plenario p = new Planario_Api.Plenario();
                            CodigosTelefono registro = p.obtenerRegistroTelefonoDNI(Convert.ToInt32(this.lblIdRegistro.Text));
                            registro.Motivo = this.txtMotivoCredito.Text;
                            p.modificarTelefonoDNI(registro);
                        }
                    }
                    this.guardarDatosFechaNacimiento();
                    this.verificarCobroAnticipo();
                    this.obtenerPagosCuentaAnticipo();

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Solicitud seleccionada con exito!. \", {type: \"info\"}); cerrarModalCredito(); ", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Debe seleccionar al menos una solicitud. \");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando solicitud. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void guardarSolicitudManual()
        {
            try
            {
                ControladorPlenario contPlenario = new ControladorPlenario();

                string omitioValidacion = this.lblOmitioCodigoCredito.Text;
                string motivo = this.txtMotivoCredito.Text;
                if (omitioValidacion == "1" && (String.IsNullOrEmpty(motivo) || motivo.Length < 3))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Debe cargar un motivo de porque omitio validacion. \");", true);
                    return;
                }

                if (this.txtNroSolicitudManual.Text.Length != 6)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Nro de solicitud debe ser de 6 digitos. \");", true);
                    return;
                }

                if (!String.IsNullOrEmpty(this.txtNroSolicitudManual.Text) && !String.IsNullOrEmpty(this.txtFechaSolicitudManual.Text) && !String.IsNullOrEmpty(this.txtCapitalSolicitudManual.Text))
                {
                    SolicitudPlenario solicitud = new SolicitudPlenario();
                    solicitud.Dni = this.txtDniCredito.Text;
                    solicitud.FechaOperacion = Convert.ToDateTime(this.txtFechaSolicitudManual.Text, new CultureInfo("es-AR"));
                    solicitud.NroSolicitud = Convert.ToInt32(this.txtNroSolicitudManual.Text);
                    solicitud.Capital = Convert.ToDecimal(this.txtCapitalSolicitudManual.Text);
                    solicitud.Anticipo = Convert.ToDecimal(this.txtAnticipoSolicitudManual.Text);
                    solicitud.Validada = 0;

                    string datos = "Solicitud nº " + this.txtNroSolicitudManual.Text + ", DNI: " + this.txtDniCredito.Text + ", Fecha: " + this.txtFechaSolicitudManual.Text + ", Capital:" + this.txtCapitalSolicitudManual.Text + ", Anticipo:" + this.txtAnticipoSolicitudManual.Text;

                    Factura f = Session["Factura"] as Factura;

                    decimal diferencia = f.total - (solicitud.Capital + solicitud.Anticipo.Value);
                    if (Math.Abs(diferencia) > Convert.ToDecimal(10))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"El capital de la solicitud debe ser igual al monto de la factura. \");", true);
                        return;
                    }

                    if (solicitud.Anticipo.ToString() != this.txtAnticipo.Text)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Verifique el monto del anticipo cargado. \");", true);
                        return;
                    }

                    f.NroSolicitud = this.txtNroSolicitudManual.Text;
                    f.Solicitud = solicitud;
                    Session["Factura"] = f;
                    this.txtComentarios.Text += "\n " + datos;

                    if (omitioValidacion == "0")
                    {
                        //lo marco como que se uso
                        contPlenario.editarEstadoRegistroTelefonoDniByID(Convert.ToInt32(this.lblIdRegistro.Text), 2);
                        this.txtComentarios.Text += " - VALIDADO POR SMS";
                    }
                    else
                    {
                        if (this.lblIdRegistro.Text == "0")
                        {
                            CodigosTelefono registro = new CodigosTelefono();
                            registro.DNI = this.txtDniCredito.Text;
                            string telefono = "+549" + this.txtCodAreaCredito.Text + this.txtNroCelularCredito.Text;//+54 9 cod + tel
                            if (!String.IsNullOrEmpty(this.txtCodAreaCredito.Text) && !String.IsNullOrEmpty(this.txtNroCelularCredito.Text) && (this.txtCodAreaCredito.Text.Length + this.txtNroCelularCredito.Text.Length == 10))
                            {
                                registro.Telefono = telefono;
                            }
                            registro.FechaHora = DateTime.Now;
                            registro.Estado = 0;
                            registro.IdEmpresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                            registro.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                            registro.IdVendedor = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                            registro.Motivo = this.txtMotivoCredito.Text;
                            contPlenario.agregarCodigoTelefono(registro);
                            this.lblIdRegistro.Text = registro.Id.ToString();
                        }
                        else
                        {
                            Planario_Api.Plenario p = new Planario_Api.Plenario();
                            CodigosTelefono registro = p.obtenerRegistroTelefonoDNI(Convert.ToInt32(this.lblIdRegistro.Text));
                            registro.Motivo = this.txtMotivoCredito.Text;
                            p.modificarTelefonoDNI(registro);
                        }
                    }
                    this.guardarDatosFechaNacimiento();
                    this.verificarCobroAnticipo();
                    this.obtenerPagosCuentaAnticipo();

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Solicitud cargada con exito!. \", {type: \"info\"}); cerrarModalCredito(); ", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Debe cargar todos los datos!. \");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos2, UpdatePanelCreditos2.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando solicitud. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void guardarDatosFechaNacimiento()
        {
            try
            {

                var datos = this.contClienteEntity.obtenerClienteDatosByCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                if (datos != null)
                {
                    int ok = 0;
                    if (datos.Count > 0)
                    {
                        datos.FirstOrDefault().FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoCredito.Text, new CultureInfo("es-AR"));
                        ok = this.contClienteEntity.modificarClienteDatos(datos.FirstOrDefault());
                    }
                    else
                    {
                        Gestion_Api.Entitys.Cliente_Datos nuevo = new Gestion_Api.Entitys.Cliente_Datos();
                        nuevo.IdCliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                        nuevo.Celular = this.txtCodAreaCredito.Text + "-" + this.txtNroCelularCredito.Text;
                        nuevo.FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoCredito.Text, new CultureInfo("es-AR"));
                        datos.Add(nuevo);
                        ok = this.contClienteEntity.agregarClienteDatos(nuevo);

                    }
                    if (ok >= 0)
                    {
                        this.verificarAgregarTareaAvisoCumpleanios(datos.FirstOrDefault());
                    }
                }
            }
            catch
            {

            }
        }
        protected void lbtnBuscarSolicitudes_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblMovSolicitud.Text = "OK";
                this.buscarSolicitudes();
            }
            catch
            {

            }
        }
        protected void txtAnticipo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtImporteFinanciar.Text) && !String.IsNullOrEmpty(this.txtAnticipo.Text))
                {
                    decimal totalFC = Convert.ToDecimal(this.txtImporteFinanciar.Text);
                    decimal anticipo = Convert.ToDecimal(this.txtAnticipo.Text);
                    this.txtFinanciado.Text = (totalFC - anticipo).ToString();
                    this.txtAnticipoSolicitudManual.Text = this.txtAnticipo.Text;
                }

            }
            catch
            {

            }
        }
        protected void lbtnAgregarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.chkCreditoManual.Checked == true)
                {
                    this.guardarSolicitudManual();
                }
                else
                {
                    this.guardarSolicitud();
                }
            }
            catch
            {

            }
        }
        protected void chkCreditoManual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkCreditoManual.Checked == true)
                {
                    this.panelSolicitudes.Visible = false;
                    this.panelCreditoManual.Visible = true;
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCreditos, UpdatePanelCreditos.GetType(), "alert", "tooltipCredito();", true);
                }
                else
                {
                    this.panelSolicitudes.Visible = true;
                    this.panelCreditoManual.Visible = false;
                }
            }
            catch
            {

            }
        }
        private void verificarCobroAnticipo()
        {
            try
            {
                if (this.txtAnticipo.Text != "0" && this.txtAnticipo.Text != "" && this.rbtnAnticipoCredito.Checked)
                {
                    if (Session["CobroAnticipo"] != null)
                    {
                        Cobro cobroAnticipo = Session["CobroAnticipo"] as Cobro;
                        if (cobroAnticipo != null)
                        {
                            this.btnCredito.Attributes["class"] = "btn btn-success";
                        }
                        else
                        {
                            this.btnCredito.Attributes["class"] = "btn btn-danger";
                        }
                    }
                    else
                    {
                        this.btnCredito.Attributes["class"] = "btn btn-danger";
                    }
                }
                else
                {
                    this.btnCredito.Attributes["class"] = "btn btn-success";
                }
            }
            catch
            {

            }
        }
        protected void btnCerrarCreditos_Click(object sender, EventArgs e)
        {
            try
            {
                this.verificarCobroAnticipo();
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCerrarCred, UpdatePanelCerrarCred.GetType(), "alert", " cerrarModalCredito(); ", true);
            }
            catch
            {

            }
        }
        protected void btnEnviarCodigoCredito_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "bloquear();", true);                   
                //this.btnEnviarCodigoCredito.Enabled = false;
                ControladorPlenario contPlena = new ControladorPlenario();

                if (this.txtCodAreaCredito.Text.Length + this.txtNroCelularCredito.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");desbloquearEnvioCod();", true);
                    return;
                }
                if (String.IsNullOrEmpty(this.txtDniCredito.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Debe completar el campo DNI !. \");desbloquearEnvioCod();", true);
                    return;
                }

                string dni = this.txtDniCredito.Text;
                string telefono = "+549" + this.txtCodAreaCredito.Text + this.txtNroCelularCredito.Text;//+54 9 cod + tel

                int ok = contPlena.validarTelefonoDNI(dni, telefono);
                if (ok > 0)
                {
                    CodigosTelefono registro = new CodigosTelefono();
                    registro.DNI = dni;
                    registro.Telefono = telefono;
                    registro.IdEmpresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                    registro.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    registro.IdVendedor = Convert.ToInt32(this.DropListVendedor.SelectedValue);

                    //int envioCodigo = contPlena.enviarCodigoTelefono(dni, telefono);
                    int envioCodigo = contPlena.enviarCodigoTelefono(registro);
                    this.lblIdRegistro.Text = registro.Id.ToString();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo enviado. \", {type: \"info\"});desbloquearEnvioCod();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Este telefono ya fue utilizado con otro DNI!. \");desbloquearEnvioCod();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error. " + ex.Message + " \", {type: \"error\"});desbloquearEnvioCod();", true);
            }
        }
        protected void btnVerificarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorPlenario contPlena = new ControladorPlenario();
                string codigo = this.txtCodigoVerif.Text;
                string dni = this.txtDniCredito.Text;
                string telefono = "+549" + this.txtCodAreaCredito.Text + this.txtNroCelularCredito.Text;//+54 9 cod + tel

                if (this.txtCodAreaCredito.Text.Length + this.txtNroCelularCredito.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");desbloquearEnvioCod();", true);
                    return;
                }
                if (String.IsNullOrEmpty(this.txtDniCredito.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Debe completar el campo DNI !. \");desbloquearEnvioCod();", true);
                    return;
                }

                int ok = contPlena.validarCodigoVerificacion(dni, telefono, codigo);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo validado. \", {type: \"info\"});", true);
                    this.lblIdRegistro.Text = ok.ToString();
                    this.mostarPanelSolicitud();
                }
                else
                {
                    if (ok == -2)
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"Codigo incorrecto. \");", true);
                    else
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelSms, UpdatePanelSms.GetType(), "alert", "$.msgbox(\"No se pudo validar. \", {type: \"error\"});", true);

                    this.ocultarPanelSolicitud();
                }
            }
            catch
            {

            }
        }
        private void ocultarPanelSolicitud()
        {
            try
            {
                this.panelSolicitudes.Visible = false;
                this.panelCreditoManual.Visible = false;
                this.lbtnBuscarSolicitudes.Visible = false;
                this.divCargaManual.Visible = false;
            }
            catch
            {

            }
        }
        private void mostarPanelSolicitud()
        {
            try
            {
                this.panelSolicitudes.Visible = true;
                this.panelCreditoManual.Visible = false;
                this.lbtnBuscarSolicitudes.Visible = true;
                this.divCargaManual.Visible = true;
                this.txtDniCredito.Attributes.Add("disabled", "disabled");
            }
            catch
            {

            }
        }
        protected void btnLimpiarProcesoCredito_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtDniCredito.Attributes.Remove("disabled");
                this.txtCodAreaCredito.Text = "";
                this.txtNroCelularCredito.Text = "";
                this.txtCodigoVerif.Text = "";
                this.lblOmitioCodigoCredito.Text = "0";
                this.ocultarPanelSolicitud();
            }
            catch
            {

            }
        }
        protected void lbtnNoValidar_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostarPanelSolicitud();
                this.panelCodigoSMS.Visible = false;
                this.panelMotivoCodigoSMS.Visible = true;
                this.lblOmitioCodigoCredito.Text = "1";
                this.lbtnVolverValidar.Visible = true;
                this.lbtnNoValidar.Visible = false;
            }
            catch
            {

            }
        }
        protected void lbtnVolverValidar_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbtnVolverValidar.Visible = false;
                this.lbtnNoValidar.Visible = true;
                this.ocultarPanelSolicitud();
                this.panelCodigoSMS.Visible = true;
                this.panelMotivoCodigoSMS.Visible = false;
                this.lblOmitioCodigoCredito.Text = "0";

            }
            catch
            {

            }
        }
        private void agregarFacturaCodigoTelefono(Factura f)
        {
            try
            {
                ControladorPlenario contPlena = new ControladorPlenario();
                int idRegistro = Convert.ToInt32(this.lblIdRegistro.Text);
                contPlena.agregarFacturaACodigo(f.id, idRegistro);
            }
            catch
            {

            }
        }
        private void verificarAgregarTareaAvisoCumpleanios(Gestion_Api.Entitys.Cliente_Datos datos)
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();
                ControladorTareas contTareas = new ControladorTareas();
                ControladorConfiguracion contConfig = new ControladorConfiguracion();
                string origen = WebConfigurationManager.AppSettings.Get("OrigenSMS");
                string telefono = "+549" + this.txtCodAreaCredito.Text + this.txtNroCelularCredito.Text;//+54 9 cod + tel

                if (this.txtCodAreaCredito.Text.Length + this.txtNroCelularCredito.Text.Length != 10)
                {
                    return;
                }

                Gestion_Api.Entitys.Configuraciones_SMS configAvisos = contConfig.ObtenerConfiguracionesAlertasSMS();
                //var asunto = contSMS.obtenerAsuntoSMSNombre("CUMPLEAÑOS");
                if (configAvisos.AlertaCumpleanios.Value == 1)
                {
                    var tarea = contTareas.obtenerTareaByClienteAsunto(datos.IdCliente.Value.ToString(), "CUMPLEAÑOS");
                    if (tarea == null)//si no hay ninguna agregada aun
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego Tarea SMS por aviso cumpleaños.");

                        string fc = datos.FechaNacimiento.Value.ToString("dd/MM/");
                        fc += DateTime.Today.Year.ToString();

                        contTareas.agregarTareaClienteNuevo(datos.IdCliente.Value, telefono, fc, configAvisos.MensajeCumpleanios, origen);
                    }
                    else
                    {
                        var listTareas = contTareas.obtenerTareasByClienteAsunto(datos.IdCliente.Value.ToString(), "CUMPLEAÑOS");
                        foreach (var t in listTareas)
                        {
                            string fc = datos.FechaNacimiento.Value.ToString("dd/MM/") + t.Fecha.Value.Year;
                            t.Fecha = Convert.ToDateTime(fc, new CultureInfo("es-AR")).AddHours(10);
                        }
                        contTareas.modificarTareas(listTareas);
                    }
                }
            }
            catch
            {

            }
        }
        protected void rbtnPagoCuentaCredito_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rbtnPagoCuentaCredito.Checked == true)
                {
                    this.panelPagosCtaCredito.Visible = true;
                    this.cargarPagosCuentaCliente();
                    this.lbtnAnticipo.Visible = false;
                }
                else
                {
                    this.lbtnAnticipo.Visible = true;
                    this.panelPagosCtaCredito.Visible = false;
                }
            }
            catch
            {

            }
        }
        protected void rbtnAnticipoCredito_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rbtnPagoCuentaCredito.Checked == true)
                {
                    this.panelPagosCtaCredito.Visible = true;
                    this.cargarPagosCuentaCliente();
                    this.lbtnAnticipo.Visible = false;
                }
                else
                {
                    this.lbtnAnticipo.Visible = true;
                    this.panelPagosCtaCredito.Visible = false;
                }
            }
            catch
            {

            }
        }
        private void cargarPagosCuentaCliente()
        {
            try
            {
                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                int cliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                int emp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int pv = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);

                List<Movimiento_Cobro> pagos = contCC.obtenerMovimientosCobroByTipo(cliente, pv, emp, suc, 2);// 2 = prp;
                if (pagos != null)
                {
                    this.phPagosCuentaCredito.Controls.Clear();
                    foreach (var p in pagos)
                    {
                        var movV = p.ListarMovimiento();
                        //fila
                        TableRow tr = new TableRow();
                        tr.ID = movV.id.ToString();

                        //Celdas

                        TableCell celFecha = new TableCell();
                        celFecha.Text = movV.fecha.ToString("dd/MM/yyyy");
                        celFecha.VerticalAlign = VerticalAlign.Middle;
                        celFecha.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(celFecha);

                        TableCell celNumero = new TableCell();
                        celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                        celNumero.VerticalAlign = VerticalAlign.Middle;
                        celNumero.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(celNumero);

                        TableCell celSaldo = new TableCell();
                        celSaldo.Text = "$" + movV.saldo.ToString().Replace(',', '.');
                        celSaldo.VerticalAlign = VerticalAlign.Middle;
                        celSaldo.HorizontalAlign = HorizontalAlign.Right;
                        tr.Cells.Add(celSaldo);

                        TableCell celSeleccion = new TableCell();
                        CheckBox cbSeleccion = new CheckBox();
                        cbSeleccion.ID = "cbSeleccion_" + movV.id;
                        cbSeleccion.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        cbSeleccion.CssClass = "btn btn-info";
                        celSeleccion.Controls.Add(cbSeleccion);
                        celSeleccion.HorizontalAlign = HorizontalAlign.Center;
                        celSeleccion.Controls.Add(cbSeleccion);
                        cbSeleccion.Attributes.Add("onchange", "javascript:return updateboxCredito(" + movV.saldo + "," + movV.id.ToString() + ");");
                        tr.Cells.Add(celSeleccion);

                        this.phPagosCuentaCredito.Controls.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void obtenerPagosCuentaAnticipo()
        {
            try
            {
                string numeroDoc = "";
                if (this.rbtnPagoCuentaCredito.Checked)
                {
                    string idPagos = "";
                    foreach (Control control in phPagosCuentaCredito.Controls)
                    {
                        TableRow tr = control as TableRow;
                        CheckBox ch = tr.Cells[3].Controls[0] as CheckBox;
                        if (ch.Checked)
                        {
                            idPagos += ch.ID.Split('_')[1] + ";";
                            numeroDoc += tr.Cells[1].Text + "; ";
                        }
                    }
                    if (!String.IsNullOrEmpty(idPagos))
                    {
                        Session["PagoCuentaAnticipo"] = idPagos;
                        this.txtComentarios.Text += " - " + numeroDoc;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void procesarPagosCuentaCredito(string pagos)
        {
            try
            {
                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                Factura fc = Session["factura"] as Factura;
                int ok = contCC.imputarMovimientosAnticiposCreditos(pagos, fc);

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region combustibles
        private int validarItemsCombustible()
        {
            try
            {
                //Recorro los items de la factura y verifico si son todos del grupo de combustibles, o si no lo son

                Factura factura = Session["Factura"] as Factura;
                bool flagItemCombustible = false;
                bool flagItemNoCombustible = false;

                foreach (var item in factura.items)
                {
                    if (item.articulo.grupo.descripcion.ToLower().Contains("combustible"))
                    {
                        if (flagItemNoCombustible)
                            return -2;

                        flagItemCombustible = true;
                    }
                    else
                    {
                        if (flagItemCombustible)
                            return -2;

                        flagItemNoCombustible = true;
                    }
                }

                //Luego de recorrer los items, verifico en caso de que sean items del tipo combustible, que se haya seleccionado el proveedor de combustible
                if (flagItemCombustible)
                {
                    if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) <= 0)
                        return -3;
                }

                return 1;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error validando items de factura de combustibles. Excepción:" + Ex.Message));
                return -1;
            }
        }
        protected void ListProveedorCombustible_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.actualizarTotales();

                if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0)
                    this.actualizarPrecioItemsCombustible();

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "clickTab(); ", true);
            }
            catch
            {

            }
        }
        private decimal obtenerTotalImpuestosCombustibles(Factura f)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                decimal total = 0;

                decimal totalItc = 0;
                decimal totalHidrica = 0;
                decimal totalVial = 0;
                decimal totalMunicipal = 0;

                foreach (ItemFactura item in f.items)
                {
                    var datos = contArtEntity.obtenerDatosCombustibleByArticuloProveedor(item.articulo.id, Convert.ToInt32(ListProveedorCombustible.SelectedValue));
                    if (datos != null)
                    {
                        totalItc += decimal.Round((item.cantidad * datos.ITC.Value), 2);
                        totalHidrica += decimal.Round((item.cantidad * datos.TasaHidrica.Value), 2);
                        totalVial += decimal.Round((item.cantidad * datos.TasaVial.Value), 2);
                        totalMunicipal += decimal.Round((item.cantidad * datos.TasaMunicipal.Value), 2);
                    }
                }

                total = totalItc + totalHidrica + totalVial + totalMunicipal;

                this.factura.totalITC = totalItc;
                this.factura.totalHidrica = totalHidrica;
                this.factura.totalMunicipal = totalMunicipal;
                this.factura.totalVial = totalVial;

                return total;
            }
            catch
            {
                return 0;
            }
        }
        private void agregarDatosCombustibleAComentarios(Factura f)
        {
            try
            {
                string info = "";

                if (f.totalITC > 0)
                    info += " ITC: $" + f.totalITC.ToString();
                if (f.totalHidrica > 0)
                    info += "\nTasa Hidrica: $" + f.totalHidrica.ToString();
                if (f.totalVial > 0)
                    info += "\nTasa Vial: $" + f.totalVial.ToString();
                if (f.totalMunicipal > 0)
                    info += "\nTasa Municipal: $" + f.totalMunicipal.ToString();

                this.txtComentarios.Text += "\n" + info;
            }
            catch (Exception ex)
            {

            }
        }
        private void actualizarTotalesVentaCombustible()
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

                //documento
                string[] lbl = this.labelNroFactura.Text.Split('°');
                this.factura.tipo = this.cargarTiposFactura(lbl[0]);
                decimal iva = 0;

                //Calculo los impuestos correspondiente a la venta de combustibles (ITC,CO2)
                this.factura.totalImpuestosCombustible = this.obtenerTotalImpuestosCombustibles(this.factura);

                //obtengo total de suma de item
                //decimal totalC = contArtEnt.obtenerTotalNetoCombustible(this.factura, Convert.ToInt32(this.ListProveedorCombustible.SelectedValue));
                decimal totalC = 0;
                foreach (var item in factura.items)
                {
                    totalC += item.total;
                }
                decimal total = decimal.Round(totalC, 2, MidpointRounding.AwayFromZero);
                this.factura.neto = total;

                if (this.accion == 6 || this.accion == 9)// si viene de generar nota de credito mantengo el descuento que le habia hecho a la factura
                {
                    this.txtPorcDescuento.Text = decimal.Round(this.factura.neto10, 2).ToString();
                }

                //Subtotal = neto menos el descuento
                this.factura.descuento = decimal.Round((this.factura.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100)), 2, MidpointRounding.AwayFromZero);
                this.factura.subTotal = decimal.Round((this.factura.neto - this.factura.descuento), 2, MidpointRounding.AwayFromZero);

                if (lbl[0].Contains("Presupuesto") || lbl[0].Contains("PRP"))
                {
                    Configuracion c = new Configuracion();

                    if (c.ivaArticulosPresupuesto == "1")
                    {
                        this.factura.neto21 = decimal.Round((this.factura.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        iva = contArtEnt.obtenerTotalIvaCombustible(this.factura, Convert.ToInt32(this.ListProveedorCombustible.SelectedValue));
                        decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
                        //AL DESCUENTO DE LA FACTURA LE AGREGO EL DESC DEL IVA
                        this.factura.descuento += descuento; decimal.Round(descuento, 2, MidpointRounding.AwayFromZero);
                        this.factura.neto21 = decimal.Round((iva - decimal.Round(descuento, 2)), 2, MidpointRounding.AwayFromZero);
                    }

                }
                else
                {
                    if (lbl[0].Contains("Factura E") || lbl[0].Contains("Credito E") || lbl[0].Contains("Debito E"))
                    {
                        this.factura.neto21 = 0;
                    }
                    else
                    {
                        iva = contArtEnt.obtenerTotalIvaCombustible(this.factura, Convert.ToInt32(this.ListProveedorCombustible.SelectedValue));
                        decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);

                        //SI ES 'B' AL DESCUENTO DE LA FACTURA LE AGREGO EL DESC DEL IVA
                        if (lbl[0].Contains("Factura B") || lbl[0].Contains("Credito B") || lbl[0].Contains("Debito B"))
                        {
                            this.factura.descuento += decimal.Round(descuento, 2, MidpointRounding.AwayFromZero);
                        }

                        this.factura.neto21 = decimal.Round((iva - decimal.Round(descuento, 2)), 2, MidpointRounding.AwayFromZero);
                    }
                }

                //this.factura.totalSinDescuento = decimal.Round(this.factura.neto + contArtEnt.obtenerTotalIvaCombustible(this.factura, Convert.ToInt32(this.ListProveedorCombustible.SelectedValue)), 2);
                this.factura.totalSinDescuento = decimal.Round(this.factura.neto + iva);

                //Las percepciones se hacen sobre el neto de la factura + impuestos de combustible
                this.factura.retencion = decimal.Round(((this.factura.subTotal + this.factura.totalImpuestosCombustible) * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100)), 2, MidpointRounding.AwayFromZero);
                //total NetosNoGravados cuando es venta combustible los guardo en .iva21

                //El total de la FC sería: Neto + Impuestos + IVA + Percepciones
                this.factura.total = decimal.Round((this.factura.subTotal + this.factura.totalImpuestosCombustible + this.factura.neto21 + this.factura.retencion), 2, MidpointRounding.AwayFromZero);

            }
            catch
            {

            }
        }
        private void actualizarPrecioItemsCombustible()
        {
            try
            {
                Factura factura = Session["Factura"] as Factura;
                ControladorArticulosEntity contArticulosEnt = new ControladorArticulosEntity();

                //Recorro items de la factura y actualizo el precio sin iva y el precio unitario
                foreach (var item in factura.items)
                {
                    //Verifico si el item pertenece al grupo de combustibles
                    if (item.articulo.grupo.descripcion.ToLower().Contains("combustible"))
                    {
                        decimal totalItc = 0;
                        decimal totalHidrica = 0;
                        decimal totalVial = 0;
                        decimal totalMunicipal = 0;

                        //Obtengo datos del proveedor de combustible
                        var datos = contArticulosEnt.obtenerDatosCombustibleByArticuloProveedor(item.articulo.id, Convert.ToInt32(ListProveedorCombustible.SelectedValue));
                        if (datos != null)
                        {
                            totalItc += decimal.Round((datos.ITC.Value), 2);
                            totalHidrica += decimal.Round((datos.TasaHidrica.Value), 2);
                            totalVial += decimal.Round((datos.TasaVial.Value), 2);
                            totalMunicipal += decimal.Round((datos.TasaMunicipal.Value), 2);
                        }

                        //Le agrego el iva al precio unitario del item
                        decimal precioConIva = decimal.Round(item.precioUnitario * (1 + (item.articulo.porcentajeIva / 100)), 2);

                        item.precioSinIva = item.precioUnitario; //Como es factura de combustible, hago esta asignacion, ya que en este momento el precio unitario del item es el precio del item sin iva
                        item.precioUnitario = precioConIva + totalItc + totalHidrica + totalVial + totalMunicipal; //Sumo al precio del item el importe de impuestos

                        //Genero un nuevo item, le seteo los valores del item que estoy modificando, remuevo el item y agrego este nuevo item
                        ItemFactura nuevoItem = item;
                        factura.items.Remove(item);
                        factura.items.Add(nuevoItem);
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error actualizando precio de items de factura de combustible. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region datos Cliente
        public void cargarDatosEnvioCliente(int idCliente)
        {
            try
            {
                List<Gestion_Api.Entitys.Cliente_Datos> datos = this.contClienteEntity.obtenerClienteDatosByCliente(idCliente);
                this.txtMailEntrega.Text = "";
                this.txtCodAreaCredito.Text = "";
                this.txtNroCelularCredito.Text = "";
                this.txtFechaNacimientoCredito.Text = "";

                if (datos != null)
                {
                    foreach (var d in datos)
                    {
                        if (!String.IsNullOrEmpty(d.Mail))
                        {
                            this.txtMailEntrega.Text += d.Mail + ";";
                        }
                        if (!String.IsNullOrEmpty(d.Celular))
                        {
                            try
                            {
                                this.txtCodAreaCredito.Text = d.Celular.Split('-')[0];
                                this.txtNroCelularCredito.Text = d.Celular.Split('-')[1];
                            }
                            catch { }
                        }
                        if (d.FechaNacimiento != null)
                        {
                            this.txtFechaNacimientoCredito.Text = d.FechaNacimiento.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (this.txtMailEntrega.Text != "")
                {
                    this.checkDatos.Checked = true;
                    this.phDatosEntrega.Visible = true;
                    this.txtMailEntrega.Attributes.Remove("disabled");
                    this.chkEnviarMail.Checked = true;
                }
            }
            catch
            {

            }
        }
        #endregion

        #region MUTUALES
        protected void lbtnAgregarPagoMutual_Click(object sender, EventArgs e)
        {
            try
            {
                Factura fact = Session["Factura"] as Factura;

                //Verifico si ya existe hay un pago mutual cargado
                if (fact.pagare != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ya existe un pago Mutual cargado. \");", true);
                    return;
                }

                //Verifico si presionó el boton de calcular montos
                if (this.lblFlagCalcularMontosMutual.Text == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Debe calcular montos para poder continuar. \");", true);
                    return;
                }

                if (this.txtCantCuotasMutual.Text != "0")
                {
                    this.calcularRecargoMutual();

                    Gestion_Api.Entitys.Mutuales_Pagares pago = new Gestion_Api.Entitys.Mutuales_Pagares();
                    pago.Mutual = Convert.ToInt32(this.ListMutuales.SelectedValue);
                    pago.Numero = this.txtNroPagareMutual.Text;
                    pago.Sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    pago.PuntoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);

                    pago.Fecha = Convert.ToDateTime(this.txtFechaPagareMutual.Text, new CultureInfo("es-AR"));
                    pago.Vencimiento = Convert.ToDateTime(this.txtFechaVtoCuotaMutual.Text, new CultureInfo("es-AR"));
                    pago.NroSocio = this.txtNroSocioMutual.Text;
                    pago.NroAutorizacion = this.txtNroAutorizacionMutual.Text;

                    //Verifico primero si el campo es diferente de nulo o vacio, si no lo es verifico que el anticipo cargado no sea mayor al total de la factura.
                    if (!string.IsNullOrEmpty(this.txtAnticipoMutual.Text) && Convert.ToDecimal(this.txtAnticipoMutual.Text) > fact.total)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"El monto del anticipo no puede ser mayor al de la factura. \");", true);
                        return;
                    }

                    //Verifico si se agregaron pagos a cuenta para anticipo o se generó un cobro por anticipo
                    bool pagosCuentaMutual = this.verificarPagosCuentaAnticipoMutual();
                    bool anticipoMutual = this.verificarAnticipoMutual();

                    if (!pagosCuentaMutual || !anticipoMutual)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Verifique el monto del anticipo cargado. \");", true);
                        return;
                    }

                    int ok = this.AplicarRecargoMutuales();
                    if (ok > 0)
                    {
                        this.lblTotalAnticipoMutualFinal.Text = txtAnticipoMutual.Text;
                        this.lblTotalRecargoMutualFinal.Text = this.lblTotalRecargoMutuales.Text;
                        this.lblTotalRecargoFinalOriginalMutuales.Text = this.lblTotalFacturaFinal.Text;
                        fact.pagare = pago;
                        fact.cantCuotasPagare = Convert.ToInt32(this.txtCantCuotasMutual.Text);
                        Session["Factura"] = fact;

                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Pago mutual guardado con exito. \", {type: \"info\"}); cerrarModalMutuales();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"No se pudo guardar pago. \");", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Cantidad de cuotas debe ser mayor a cero!. \");", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando pago mutual. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        protected void ListMutuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                controladorFactEntity contMutual = new controladorFactEntity();

                //Seteo la flag de mutuales en 0
                this.lblFlagCalcularMontosMutual.Text = "0";

                int id = Convert.ToInt32(this.ListMutuales.SelectedValue);
                if (id > 0)
                {
                    Gestion_Api.Entitys.Mutuale m = contMutual.obtenerMutualByID(id);
                    string nroSocio = contMutual.obtenerNroSocioCliente(Convert.ToInt32(this.DropListClientes.SelectedValue), id);
                    this.txtNroPagareMutual.Text = m.NroPagare.ToString().PadLeft(8, '0');
                    this.txtNroSocioMutual.Text = nroSocio;

                    this.ListMutualesPagos.DataSource = m.Mutuales_Pagos.ToList();
                    this.ListMutualesPagos.DataTextField = "Nombre";
                    this.ListMutualesPagos.DataValueField = "Id";
                    this.ListMutualesPagos.DataBind();

                    this.ListMutualesPagos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                }
            }
            catch
            {

            }
        }
        protected void ListMutualesPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Seteo la flag de mutuales en 0
                this.lblFlagCalcularMontosMutual.Text = "0";

                //Cuando cambia la mutual, calculo el recargo, y seteo ese recargo al label de recargo original, para que en caso de que haya un anticipo, no se pierda.
                this.calcularRecargoMutual();
            }
            catch
            {

            }
        }
        private void calcularRecargoMutual()
        {
            try
            {
                controladorFactEntity contMutual = new controladorFactEntity();
                int id = Convert.ToInt32(this.ListMutualesPagos.SelectedValue);
                if (id > 0)
                {
                    Gestion_Api.Entitys.Mutuales_Pagos m = contMutual.obtenerMutualPagosByID(id);
                    this.txtCantCuotasMutual.Text = m.Cuotas.ToString();
                    if (m.Coeficiente.Value > 0)
                    {
                        decimal total = Convert.ToDecimal(this.lblTotalMutuales.Text);
                        decimal totalRecago = total * (1 + (m.Coeficiente.Value / 100));
                        this.lblTotalRecargoMutuales.Text = decimal.Round(totalRecago, 2).ToString();

                        //Calculo el valor de recargo sobre el valor original de la fc, para guardarlo en una variable auxiliar que servirá para realizar la verificación mas adelante.
                        decimal total2 = Convert.ToDecimal(this.lblTotalOriginalMutuales.Text);
                        decimal totalRecargo2 = total2 * (1 + (m.Coeficiente.Value / 100));
                        this.lblTotalRecargoOriginalMutuales.Text = decimal.Round(totalRecargo2, 2).ToString();
                    }
                    else
                    {
                        //si es cero lo dejo igual

                        this.lblTotalRecargoMutuales.Text = this.lblTotalMutuales.Text;
                        this.lblTotalRecargoOriginalMutuales.Text = this.lblTotalOriginalMutuales.Text;
                    }
                }
            }
            catch
            {

            }
        }
        protected void lbtnQuitarPagoMutual_Click(object sender, EventArgs e)
        {
            try
            {
                int ok = this.QuitarRecargoMutuales();
                if (ok > 0)
                {
                    Factura fact = Session["Factura"] as Factura;
                    fact.pagare = null;
                    fact.cantCuotasPagare = 0;
                    this.lblTotalMutuales.Text = fact.total.ToString();
                    this.lblTotalOriginalMutuales.Text = fact.total.ToString();
                    this.lblTotalFacturaFinal.Text = "0.00";
                    this.lblTotalAnticipoMutualFinal.Text = "0.00";
                    this.lblTotalAnticipoMutualFinanciado.Text = "0.00";
                    Session["Factura"] = fact;
                    Session["PagoCuentaAnticipoMutual"] = null;
                    Session["CobroAnticipo"] = null;

                    //Seteo la flag de mutuales en 0
                    this.lblFlagCalcularMontosMutual.Text = "0";

                    //Formateo los campos del popup para que pueda modificar
                    this.FormatearCamposMutual(2);

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Pago mutual quitado con exito. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"No se pudo quitar pago!. \");", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrio un error quitando pago mutual. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private int AplicarRecargoMutuales()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                //decimal montoFinalRecargo = Convert.ToDecimal(this.lblTotalRecargoMutuales.Text);
                //decimal montoOriginal = Convert.ToDecimal(this.lblTotalMutuales.Text);                
                //decimal recargo = (montoFinalRecargo / montoOriginal);

                decimal montoFinalRecargo = Convert.ToDecimal(this.lblTotalFacturaFinal.Text);
                decimal montoOriginal = Convert.ToDecimal(this.lblTotalOriginalMutuales.Text);
                decimal recargo = (montoFinalRecargo / montoOriginal);


                if (recargo > 0)//si la mutual tiene recargo recalculo, sino dejo como esta
                {
                    foreach (ItemFactura item in f.items)
                    {
                        item.precioSinIva = Decimal.Round(item.precioSinIva * recargo, 2, MidpointRounding.AwayFromZero);//(1 + (t.recargo / 100));                    
                        item.precioUnitario = Decimal.Round(item.precioUnitario * recargo, 2, MidpointRounding.AwayFromZero);// (1 + (t.recargo / 100));                    
                        item.total = Decimal.Round((item.precioUnitario * item.cantidad) * (1 - (item.porcentajeDescuento / 100)), 2, MidpointRounding.AwayFromZero);
                        item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2, MidpointRounding.AwayFromZero);
                    }
                }
                Session.Add("Factura", f);

                //lo dibujo en pantalla
                this.cargarItems();
                //actualizo totales
                this.actualizarTotales();

                return 1;
            }
            catch
            {
                return -1;
            }
        }
        private int QuitarRecargoMutuales()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;

                foreach (ItemFactura item in f.items)
                {
                    item.precioSinIva = item.precioSinRecargo;
                    item.precioUnitario = item.precioVentaSinRecargo;
                    item.total = Decimal.Round(((item.precioUnitario * item.cantidad) / (1 - (item.porcentajeDescuento / 100))), 2);
                    item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2);
                }

                Session.Add("Factura", f);

                //lo dibujo en pantalla
                this.cargarItems();
                //actualizo totales
                this.actualizarTotales();

                this.ListMutuales.SelectedValue = "-1";
                this.ListMutualesPagos.SelectedValue = "-1";
                this.lblTotalRecargoMutuales.Text = this.lblTotalMutuales.Text;
                this.txtNroPagareMutual.Text = "";
                this.txtFechaVtoCuotaMutual.Text = "";
                this.txtCantCuotasMutual.Text = "";
                this.lblTotalAnticipoMutual.Text = "0.00";
                this.lblTotalAnticipoPagoCuentaMutual.Text = "0.00";
                this.txtAnticipoMutual.Text = "0";
                this.lblTotalMutuales.Text = this.lblTotalOriginalMutuales.Text;
                this.lblTotalRecargoMutuales.Text = "0.00";

                this.calcularLabelAnticiposMutual();



                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        protected void txtAnticipoMutual_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Obtengo la Factura desde la Session
                Factura f = Session["Factura"] as Factura;

                //Si está checkeado el radio button de anticipo mutual, elimino pongo en null el cobro de la session
                if (rbtnAnticipoMutual.Checked)
                    Session["CobroAnticipo"] = null;

                //Verifico que el campo total tenga algun valor, y que el campo anticipo tenga algun valor
                if (!String.IsNullOrEmpty(this.lblTotalMutuales.Text) && !String.IsNullOrEmpty(this.txtAnticipoMutual.Text))
                {
                    //Calculo el total original restando el anticipo
                    decimal totalFC = decimal.Round(f.total, 2);
                    decimal anticipo = Convert.ToDecimal(this.txtAnticipoMutual.Text);
                    this.lblTotalMutuales.Text = (totalFC - anticipo).ToString();

                    //Calculo el recargo, si lo hay
                    this.calcularRecargoMutual();

                    //Calculo los label de anticipo
                    this.calcularLabelAnticiposMutual();
                }

                ScriptManager.RegisterStartupScript(UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "abrirModalMutuales", "abrirModalMutuales();", true);

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error calculando Total Original y Total Recargo de Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void calcularLabelAnticiposMutual()
        {
            try
            {
                if (rbtnAnticipoMutual.Checked)
                {
                    if (!string.IsNullOrEmpty(this.txtAnticipoMutual.Text))
                        this.lblTotalAnticipoMutual.Text = this.txtAnticipoMutual.Text;
                }
                else
                    this.lblTotalAnticipoMutual.Text = "0.00";


                if (rbtnPagoCuentaMutual.Checked)
                {
                    if (!string.IsNullOrEmpty(this.txtAnticipoMutual.Text))
                        this.lblTotalAnticipoPagoCuentaMutual.Text = this.txtAnticipoMutual.Text;
                }
                else
                    this.lblTotalAnticipoPagoCuentaMutual.Text = "0.00";

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error calculando los anticipos de Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        protected void rbtnAnticipoMutual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rbtnAnticipoMutual.Checked == true)
                {
                    //Limpio campo de anticipo, y muestro boton para generar un cobro pago a cuenta
                    this.txtAnticipoMutual.Text = "0";

                    //Limpio el lbl de total con el original de la factura
                    this.lblTotalMutuales.Text = this.lblTotalOriginalMutuales.Text;

                    //Muestro boton para cargar anticipo
                    this.lbtnAnticipoMutual.Visible = true;

                    //Oculto tabla de pagos a cuenta
                    this.panelPagosCuentaMutual.Visible = false;

                    //Si ya se generó un cobro pago a cuenta, lo muestro en el campo anticipo
                    Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;
                    if (anticipoCargado != null)
                        this.txtAnticipoMutual.Text = anticipoCargado.total.ToString();

                    //Actualizo los labels de anticipos
                    this.calcularLabelAnticiposMutual();

                    //Recalculo recargo
                    this.calcularRecargoMutual();
                }

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error al tildar boton de anticipo de Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        protected void lbtnAnticipoMutual_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifico que ya no haya un anticipo cargado en la Session
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;

                if (anticipoCargado != null)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", " $.msgbox(\"El cobro del anticipo ya fue cargado anteriormente. \", {type: \"info\"});", true);

                int idEmp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int idSuc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int idPv = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                int idCli = Convert.ToInt32(this.DropListClientes.SelectedValue);

                //Verifico que se haya cargado algun monto en el campo anticipo
                if (this.txtAnticipoMutual.Text == "0" || this.txtAnticipoMutual.Text == "")
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", " $.msgbox(\"Debe cargar algún monto en el campo Anticipo. \", {type: \"info\"});", true);

                //Verifico que se hayan filtrado los campos de emrpesa, sucursal, punto de venta y cliente
                if (idEmp > 0 && idSuc > 0 && idPv > 0 && idCli > 0)
                {
                    //Seteo la flag de mutuales en 0
                    this.lblFlagCalcularMontosMutual.Text = "0";

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "window.open('../Cobros/ABMCobros?documentos=0;&cliente=" + idCli + "&empresa=" + idEmp + "&sucursal=" + idSuc + "&puntoVenta=" + idPv + "&monto=" + this.txtAnticipoMutual.Text + "&valor=2&tipo=2&anticipo=1'); ", true);
                }

            }
            catch
            {

            }
        }
        protected void rbtnPagoCuentaMutual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //Verifico que ya no haya un anticipo cargado en la Session. Si existe, no le permito que cargue los pagos a cuenta
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;

                if (anticipoCargado != null)
                {
                    this.rbtnAnticipoMutual.Checked = true;
                    this.rbtnPagoCuentaMutual.Checked = false;
                    return;
                }

                if (this.rbtnPagoCuentaMutual.Checked == true)
                {

                    this.txtAnticipoMutual.Text = "0";

                    //Limpio el lbl de total con el original de la factura
                    this.lblTotalMutuales.Text = this.lblTotalOriginalMutuales.Text;

                    this.lbtnAnticipoMutual.Visible = false;
                    this.panelPagosCuentaMutual.Visible = true;
                    this.cargarPagosCuentaMutualCliente();

                    //Actualizo los labels de anticipos
                    this.calcularLabelAnticiposMutual();

                    //Recalculo recargo
                    this.calcularRecargoMutual();
                }

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error al tildar boton de pago a cuenta de Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void cargarPagosCuentaMutualCliente()
        {
            try
            {
                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                int cliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                int emp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int pv = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);

                List<Movimiento_Cobro> pagos = contCC.obtenerMovimientosCobroByTipo(cliente, pv, emp, suc, 2);// 2 = prp;
                if (pagos != null)
                {
                    this.phPagosCuentaMutual.Controls.Clear();
                    foreach (var p in pagos)
                    {
                        var movV = p.ListarMovimiento();
                        //fila
                        TableRow trMutual = new TableRow();
                        //trMutual.ID = movV.id.ToString();

                        //Celdas

                        TableCell celFechaMutual = new TableCell();
                        celFechaMutual.Text = movV.fecha.ToString("dd/MM/yyyy");
                        celFechaMutual.VerticalAlign = VerticalAlign.Middle;
                        celFechaMutual.HorizontalAlign = HorizontalAlign.Left;
                        trMutual.Cells.Add(celFechaMutual);

                        TableCell celNumeroMutual = new TableCell();
                        celNumeroMutual.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                        celNumeroMutual.VerticalAlign = VerticalAlign.Middle;
                        celNumeroMutual.HorizontalAlign = HorizontalAlign.Left;
                        trMutual.Cells.Add(celNumeroMutual);

                        TableCell celSaldoMutual = new TableCell();
                        celSaldoMutual.Text = "$" + movV.saldo.ToString().Replace(',', '.');
                        celSaldoMutual.VerticalAlign = VerticalAlign.Middle;
                        celSaldoMutual.HorizontalAlign = HorizontalAlign.Right;
                        trMutual.Cells.Add(celSaldoMutual);

                        TableCell celSeleccionMutual = new TableCell();
                        CheckBox cbSeleccionMutual = new CheckBox();
                        cbSeleccionMutual.ID = "cbSeleccionMutual_" + movV.id;
                        cbSeleccionMutual.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        cbSeleccionMutual.CssClass = "btn btn-info";
                        celSeleccionMutual.Controls.Add(cbSeleccionMutual);
                        celSeleccionMutual.HorizontalAlign = HorizontalAlign.Center;
                        celSeleccionMutual.Controls.Add(cbSeleccionMutual);
                        cbSeleccionMutual.Attributes.Add("onchange", "javascript:return updateboxMutual(" + movV.saldo + "," + movV.id.ToString() + ");");
                        trMutual.Cells.Add(celSeleccionMutual);

                        this.phPagosCuentaMutual.Controls.Add(trMutual);
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un cargando pagos a cuenta a la lista en Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void procesarPagosCuentaMutual(string pagos)
        {
            try
            {
                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                Factura fc = Session["factura"] as Factura;

                int i = contCC.imputarMovimientosAnticiposMutuales(pagos, fc);

                if (i > 0)
                {
                    Log.EscribirSQL(1, "INFO", "Se imputaron correctamente los pagos " + pagos + " a la Factura con id " + fc.id);
                }
                else
                {
                    Log.EscribirSQL(1, "INFO", "Ocurrió un error imputando los pagos " + pagos + " a la Factura con id " + fc.id);
                }

            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "INFO", "Ocurrió un error en procesarPagosCuentaMutual. Excepción: " + Ex.Message);
            }
        }
        private bool verificarPagosCuentaAnticipoMutual()
        {
            try
            {
                string numeroDoc = "";
                string importeCobro;
                decimal monto = 0;

                //Verifico si esta tildado el boton de pagos a cuenta
                if (this.rbtnPagoCuentaMutual.Checked)
                {
                    string idPagos = "";

                    //Si está tildado, recorro los pagos a cuenta y voy sumando los montos de los pagos tildados
                    foreach (Control control in phPagosCuentaMutual.Controls)
                    {
                        TableRow tr = control as TableRow;
                        CheckBox ch = tr.Cells[3].Controls[0] as CheckBox;
                        if (ch.Checked)
                        {
                            idPagos += ch.ID.Split('_')[1] + ";";
                            numeroDoc += tr.Cells[1].Text + "; ";
                            importeCobro = tr.Cells[2].Text;
                            importeCobro = importeCobro.Substring(2, importeCobro.Length - 2);
                            monto += Convert.ToDecimal(importeCobro);
                        }
                    }

                    //Si la suma de los montos es diferente al monto que está cargado en el campo de anticipo, devuelvo falso, ya que se produjo una modificación incorrecta.
                    if (Convert.ToDecimal(this.lblTotalAnticipoMutualFinanciado.Text) != monto)
                        return false;

                    //Si está todo bien, compruebo que se hayan tildado pagos y los cargo en la session
                    if (!String.IsNullOrEmpty(idPagos))
                    {
                        Session["PagoCuentaAnticipoMutual"] = idPagos;
                        this.txtComentarios.Text += " - " + numeroDoc;

                        return true;
                    }
                    else
                    {
                        //Devuelvo falso ya que está tildado el boton de pagos a cuenta pero no se tildó ningun pago
                        return false;
                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error verificando pagos a cuenta para anticipo de mutual. Excepción: " + Ex.Message + " \");", true);
                return false;
            }
        }
        private bool verificarAnticipoMutual()
        {
            try
            {
                //Verifico si hay un anticipo cargado en la session
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;
                if (anticipoCargado != null)
                {
                    //Si existe anticipo en la session, verifico que el monto cargado en el campo anticipo sea el mismo del cobro generado en la session
                    if (Convert.ToDecimal(this.lblTotalAnticipoMutualFinanciado.Text) == anticipoCargado.total)
                        return true;

                    return false;
                }

                //Verifico si el radiobutton de anticipo esta tildado
                if (this.rbtnAnticipoMutual.Checked)
                {
                    //Si el campo anticipo esta vacío y el campo de anticipo está vacío o contiene 0, retorno verdadero, porque por defecto el radiobutton este te viene tildado.
                    if (string.IsNullOrEmpty(this.txtAnticipoMutual.Text) || this.txtAnticipoMutual.Text == "0")
                        return true;

                    //Si el campo anticipo no está vacio, retorno falso, ya que cargó un anticipo pero no generó el cobro
                    if (Convert.ToDecimal(this.txtAnticipoMutual.Text) > 0)
                        return false;
                }

                //Si no hay nada en la session y el radiobutton de anticipo no está tildado, lo dejo seguir
                return true;

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error verificando anticipo manual de mutual. \");", true);
                return false;
            }
        }
        private int validarAnticipoMutual()
        {
            try
            {
                controladorCobranza contCobranza = new controladorCobranza();
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;
                string pagos = Session["PagoCuentaAnticipoMutual"] as string;
                Factura f = Session["Factura"] as Factura;

                decimal montoAnticipo = Convert.ToDecimal(this.lblTotalAnticipoMutualFinal.Text);

                //Si está tildado el boton de Anticipo y el monto de anticipo es > 0, verifico que se haya generado el cobro.
                if (rbtnAnticipoMutual.Checked && montoAnticipo > 0)
                {
                    if (anticipoCargado != null)
                        return 1;

                    return -1;
                }

                //Si está tildado el botón de Pagos a Cuenta y el monto de anticipo es > 0, verifico que se hayan cargado los pagos.
                if (rbtnPagoCuentaMutual.Checked && montoAnticipo > 0)
                {
                    if (!String.IsNullOrEmpty(pagos))
                        return 1;

                    return -1;
                }

                //Si no se cargó ningun anticipo, devuelvo 0.
                return 0;

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un validando anticipos en pago Mutual. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
                return -1;
            }
        }
        private bool verificarAnularFcMutual()
        {
            try
            {
                controladorCobranza contCobranza = new controladorCobranza();
                controladorFactEntity contFactEnt = new controladorFactEntity();
                string facturas = Request.QueryString["facturas"];
                string cobrosAnular = string.Empty;
                string pagaresAnular = string.Empty;

                //Verifico si la Factura tiene forma de pago mutual
                if (this.accion == 6)
                {
                    if (!string.IsNullOrEmpty(facturas))
                    {
                        //Obtengo la Factura
                        var fc = this.controlador.obtenerFacturaId(Convert.ToInt32(facturas.Split(';')[0]));

                        //Si es nulo, devuelvo falso
                        if (fc == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error obteniendo FC para realizar la Nota de Crédito. \");", true);
                            return false;
                        }

                        if (fc.formaPAgo.forma.ToLower() == "mutuales")
                        {
                            //Primero verifico si la factura tiene pagarés en estado liquidado

                            var pagares = contFactEnt.obtenerPagaresByFacturaEstado(fc.id, 2);
                            if (pagares != null && pagares.Count > 0)
                            {
                                //Si tiene pagarés liquidados, concateno los numeros en un string, y lo muestro
                                foreach (var pag in pagares)
                                {
                                    pagaresAnular += pag.Numero + " ; ";
                                }

                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Para realizar la NC de esta FC debe anular el/los siguiente/s Pagaré/s que se encuentran liquidados: " + pagaresAnular.Substring(0, pagaresAnular.Length - 2) + " \");", true);
                                return false;
                            }

                            //Luego verifico si la factura tiene recibos de cobro generados en consecuencia de la factura con forma de pago mutual

                            //Obtengo el cobro generado cuando facturé con forma de pago mutual
                            List<Gestion_Api.Entitys.Facturas_Anticipos> datosFcMutual = contFactEnt.obtenerDatosFacturaAnticipoByFactura(fc.id);

                            if (datosFcMutual != null && datosFcMutual.Count > 0)
                            {
                                //Recorro la lista y obtengo la información de los cobros asociados a esa factura. Guardo los numeros en un string
                                foreach (var item in datosFcMutual)
                                {
                                    var cobro = contCobranza.obtenerCobroID(item.IdCobroAnticipo);

                                    if (cobro != null)
                                        cobrosAnular += cobro.numero + " ; ";
                                }

                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Para realizar la NC de esta FC debe anular el/los siguiente/s Cobro/s: " + cobrosAnular.Substring(0, cobrosAnular.Length - 2) + " \");", true);
                                return false;
                            }
                            else
                            {
                                //Si el o los cobros que estaban asociados a la factura ya se encuentran eliminados, retorno true
                                return true;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un verificando verificando pagos de Mutual para la Factura. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
                return false;
            }
        }
        protected void lbtnCalcularMontosMutual_Click(object sender, EventArgs e)
        {
            try
            {
                Factura fc = Session["Factura"] as Factura;
                Cobro anticipoCargado = Session["CobroAnticipo"] as Cobro;

                //Verifico que el total de la Factura sea mayor a 0
                if (fc.total <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"El total de la factura no puede ser 0. \");", true);
                    return;
                }

                //Verifico que se haya seleccionado la mutual
                if (ListMutuales.SelectedValue == "-1" || ListMutualesPagos.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Debe seleccionar los filtros de mutual. \");", true);
                    return;
                }

                //Calculo parte de Anticipos
                if (!string.IsNullOrEmpty(this.txtAnticipoMutual.Text) && Convert.ToDecimal(this.txtAnticipoMutual.Text) > 0)
                {
                    //Cobro Generado
                    if (this.rbtnAnticipoMutual.Checked)
                    {
                        if (anticipoCargado == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"No se generó el cobro por el anticipo cargado. \");", true);
                            return;
                        }

                        this.lblTotalAnticipoMutualFinanciado.Text = anticipoCargado.total.ToString();
                    }

                    //Pagos a Cuenta Anteriores
                    if (this.rbtnPagoCuentaMutual.Checked)
                    {
                        string importeCobro;
                        decimal monto = 0;

                        foreach (Control control in phPagosCuentaMutual.Controls)
                        {
                            TableRow tr = control as TableRow;
                            CheckBox ch = tr.Cells[3].Controls[0] as CheckBox;
                            if (ch.Checked)
                            {
                                importeCobro = tr.Cells[2].Text;
                                importeCobro = importeCobro.Substring(2, importeCobro.Length - 2);
                                monto += Convert.ToDecimal(importeCobro);
                            }
                        }

                        //Si esta tildado el radiobutton de pagos a cuenta anteriores y el monto es menor o igual a 0, no lo dejo seguir
                        if (monto <= 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Debe seleccionar algun pago a cuenta. \");", true);
                            return;
                        }

                        this.lblTotalAnticipoMutualFinanciado.Text = monto.ToString();

                    }
                }

                //Calculo el total final de la factura
                decimal totalAnticipo = Convert.ToDecimal(this.lblTotalAnticipoMutualFinanciado.Text);
                decimal totalRecargo = Convert.ToDecimal(this.lblTotalRecargoMutuales.Text);
                decimal totalFacturaFinal = totalAnticipo + totalRecargo;

                //Modifico flag de calcular montos
                this.lblFlagCalcularMontosMutual.Text = "1";

                //Formateo los campos del popup para que no pueda modificar
                this.FormatearCamposMutual(1);

                this.lblTotalFacturaFinal.Text = totalFacturaFinal.ToString();

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error calculando montos de mutual. Excepción:" + Ex.Message + " \");", true);
            }
        }
        private void FormatearCamposMutual(int flag)
        {
            try
            {
                //Calculó montos, por lo tanto deshabilito los controles
                if (flag == 1)
                {
                    this.ListMutuales.Attributes.Add("disabled", "disabled");
                    this.ListMutualesPagos.Attributes.Add("disabled", "disabled");
                    this.txtAnticipoMutual.Attributes.Add("disabled", "disabled");
                    this.txtAnticipoMutual.CssClass = "form-control";
                    this.lbtnAnticipoMutual.Visible = false;
                    this.rbtnAnticipoMutual.Visible = false;
                    this.rbtnPagoCuentaMutual.Visible = false;
                }

                //Quitó el pago mutual, por lo tanto habilito los controles
                if (flag == 2)
                {
                    this.ListMutuales.Attributes.Remove("disabled");
                    this.ListMutualesPagos.Attributes.Remove("disabled");
                    this.txtAnticipoMutual.Attributes.Remove("disabled");
                    this.lbtnAnticipoMutual.Visible = true;
                    this.rbtnAnticipoMutual.Visible = true;
                    this.rbtnPagoCuentaMutual.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMutuales, UpdatePanelMutuales.GetType(), "alert", "$.msgbox(\"Ocurrió un error formateando campos de mutual. Excepción:" + Ex.Message + " \");", true);
            }
        }
        #endregion

        #region DATOS EXTRAS
        public int verificarDatosExtrasCargados(ItemFactura item)
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

                int esDatosExtra = contArtEnt.obtenerSiDatosExtra(item.articulo.id);
                int esTrazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                //si es traza, ignoro los datos extras
                if (esDatosExtra == 1 && esTrazable < 1)
                {
                    if (item.datosExtras == null)
                        return -1;
                    else
                    {
                        if (String.IsNullOrEmpty(item.datosExtras.Datos))
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        protected void btnDatosExtra_Click(object sender, EventArgs e)
        {
            try
            {
                int idArt = Convert.ToInt32(this.lblDatosExtraItem.Text.Split('_')[1]);
                int posItem = Convert.ToInt32(this.lblDatosExtraItem.Text.Split('_')[2]);

                if (!String.IsNullOrEmpty(this.txtDatoExtra.Text))
                {
                    Gestion_Api.Entitys.Articulos_DatosExtra datos = new Gestion_Api.Entitys.Articulos_DatosExtra();
                    datos.IdArticulo = idArt;
                    datos.Datos = this.txtDatoExtra.Text;

                    Factura fc = Session["Factura"] as Factura;
                    fc.items[posItem].datosExtras = datos;
                    fc.items[posItem].articulo.descripcion += "\n" + datos.Datos;
                    Session["Factura"] = fc;
                    this.txtDatoExtra.Text = "";

                    this.cargarItems();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Datos cargados con exito!. \", {type: \"info\"}); cerrarModalDatosExtra(); ", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Debe cargar los datos!. \");", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando datos extras. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }

        #endregion                

        #region MILLAS
        private void agregarMovimientoMillas(Factura f)
        {
            try
            {
                string sistema = WebConfigurationManager.AppSettings.Get("Millas");
                if (!String.IsNullOrEmpty(sistema))
                {
                    ControladorMillas contMilla = new ControladorMillas();
                    Configuracion config = new Configuracion();
                    controladorFactEntity contFcEnt = new controladorFactEntity();
                    int flag_tipo = 0; //para saber si suma o resta
                    string cuitDoc = f.cliente.cuit.Replace("-", "");
                    string dniReal = "";
                    if (cuitDoc.Length >= 11)//cuit con guiones
                    {
                        dniReal = cuitDoc.Substring(2, 8);
                    }
                    else//dni solo
                    {
                        dniReal = cuitDoc.PadLeft(8, '0');
                    }

                    int ok = 0;
                    string datosEmpresa = this.ListEmpresa.SelectedItem.Text + " - " + this.ListSucursal.SelectedItem.Text;
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio agregar Movimiento millas al DNI:" + dniReal);
                    if (config.siemprePRP == "1")
                    {
                        if (f.tipo.tipo.Contains("Presupuesto"))
                        {
                            ok = contMilla.agregarMovimiento(dniReal, f.tipo.tipo + " " + f.numero + " - " + datosEmpresa, f.fecha, f.total, 1);
                            flag_tipo = 0;//suma
                        }
                        if (f.tipo.tipo.Contains("Credito PRP"))
                        {
                            ok = contMilla.agregarMovimiento(dniReal, f.tipo.tipo + " " + f.numero + " - " + datosEmpresa, f.fecha, f.total, 2);
                            flag_tipo = 1;//resta
                        }
                    }
                    else
                    {
                        if (f.tipo.tipo.Contains("Factura") || f.tipo.tipo.Contains("Presupuesto"))
                        {
                            ok = contMilla.agregarMovimiento(dniReal, f.tipo.tipo + " " + f.numero + " - " + datosEmpresa, f.fecha, f.total, 1);
                            flag_tipo = 0;//suma
                        }
                        if (f.tipo.tipo.Contains("Credito"))
                        {
                            ok = contMilla.agregarMovimiento(dniReal, f.tipo.tipo + " " + f.numero + " - " + datosEmpresa, f.fecha, f.total, 2);
                            flag_tipo = 1;//resta
                        }
                    }
                    if (ok > 0)
                    {
                        var comentario = contFcEnt.obtenerComentarioFactura(f.id);
                        if (comentario != null)
                        {
                            int millaSumadas = contMilla.obtenerTotalMillas(f.total, flag_tipo);

                            comentario.Observaciones += "\n Sumo " + millaSumadas.ToString() + " puntos por esta compra.";
                            contFcEnt.modificarComentarioFactura(comentario);
                        }

                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Movimiento millas agregado al DNI:" + dniReal);
                    }
                    else
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se pudo agregar movimiento millas al DNI:" + dniReal);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se pudo agregar movimiento millas desde fc: " + f.id + ". " + ex.Message);
            }
        }

        #endregion

        #region Familia
        public void GenerarFacturaPedidoPorPadre(string pedidos)
        {
            try
            {
                this.factura = Session["Factura"] as Factura;
                string recalcularPrecio = WebConfigurationManager.AppSettings.Get("recalcularPrecioPedido");
                Configuracion config = new Configuracion();
                Factura f = controlador.GenerarFacturaDesdePedido(pedidos, Convert.ToInt32(recalcularPrecio));
                Session.Add("Factura", f);
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.ListSucursal.SelectedValue = "-1";
                this.ListPuntoVenta.SelectedValue = "-1";
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);

                //antes de cargar cliente me guardo temporalmente el descuento y subtotal
                decimal subtotal = f.subTotal;
                decimal descuento = f.descuento;

                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                //cargo los datos de entrega del pedido.
                this.txtFechaEntrega.Text = f.pedidos[0].fechaEntrega.ToString("dd/MM/yyyy");
                this.txtHorarioEntrega.Text = f.pedidos[0].horaEntrega;
                this.txtBultosEntrega.Text = f.bultosEntrega;
                //cargocliente
                Session.Add("FacturasABM_ClienteModal", this.idClientePadre);
                this.DropListClientes.Attributes.Add("disabled", "disabled");
                this.cargarClienteDesdeModal();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();

                if (config.infoImportacionFacturas == "1")
                {
                    foreach (ItemFactura item in f.items)
                    {
                        this.agregarInfoDespachoDesdePedido(item);
                    }
                }

                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();

                var itemCero = f.items.Where(x => x.total == 0).FirstOrDefault();
                if (itemCero != null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Existe/n item/s en la factura con precio final cero."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));
            }
        }
        #endregion

        protected void btnCambiarPorcentajeCantidadFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal porcentaje = Convert.ToInt32(this.txtPorcentajeCantFacturar.Text);
                if (porcentaje > 0)
                {
                    Factura fc = Session["Factura"] as Factura;
                    foreach (var item in fc.items)
                    {
                        item.cantidad = item.cantidad * (porcentaje / 100);
                        item.cantidad = Convert.ToInt32(item.cantidad);
                        item.descuento = (item.precioUnitario * (item.porcentajeDescuento / 100)) * item.cantidad;
                        item.total = ((item.precioUnitario * (1 - (item.porcentajeDescuento / 100))) * item.cantidad);
                    }
                    Session["Factura"] = fc;
                    //vuelvo a cargar los items
                    this.cargarItems();
                    this.actualizarTotales();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarComentariosDeRemito()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                this.txtComentarios.Text = f.comentario;
            }
            catch (Exception Ex)
            {

            }
        }

        protected void lbtnAvanzada_Click(object sender, EventArgs e)
        {
            Session.Remove("Factura");
            Response.Redirect("ABMFacturas.aspx");
        }

        #region alta rapida clientes
        protected void btnAltaRapida_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cRapido = new Cliente();
                cRapido.codigo = this.txtCodigoAR.Text;
                cRapido.razonSocial = this.txtRazonAR.Text;
                cRapido.alias = this.txtRazonAR.Text;
                cRapido.tipoCliente.id = Convert.ToInt32(this.DropListTipoAR.SelectedValue);
                cRapido.tipoCliente.descripcion = this.DropListTipoAR.SelectedItem.Text;
                cRapido.grupo.id = Convert.ToInt32(this.DropListGrupoAR.SelectedValue);
                cRapido.categoria.id = 1;
                cRapido.cuit = this.txtCuitAR.Text;
                cRapido.iva = this.DropListIvaAR.SelectedValue.ToString();
                cRapido.formaPago.id = Convert.ToInt32(this.DropListFormaPagoAR.SelectedValue);
                cRapido.vendedor.id = Convert.ToInt32(this.ListVendedoresAR.SelectedValue);
                cRapido.lisPrecio.id = Convert.ToInt32(this.ListListaPreciosAR.SelectedValue);
                cRapido.saldoMax = 0;
                cRapido.vencFC = 0;
                cRapido.descFC = 0;
                cRapido.observaciones = "";
                cRapido.hijoDe = 0;
                cRapido.sucursal.id = (int)Session["Login_SucUser"];
                cRapido.origen = 1;
                cRapido.alerta.descripcion = "";
                cRapido.alerta.idCliente = cRapido.id;
                cRapido.estado.id = 1;
                cRapido.pais.id = 1;

                if (this.contCliente.validateCuit(this.txtCuitAR.Text, this.DropListTipoAR.SelectedItem.Text))
                {
                    int i = this.contCliente.agregarCliente(cRapido);
                    cRapido.id = i;
                    if (i > 0)
                    {
                        this.cargarClienteEnLista(cRapido.id);
                        this.cargarCliente(cRapido.id);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel7, UpdatePanel7.GetType(), "alert", "$.msgbox(\"No se pudo agregar cliente. \");", true);
                    }
                }
            }
            catch
            {

            }
        }
        private void generarCodigo()
        {
            try
            {
                string p = this.contCliente.obtenerLastCodigoCliente();
                int newp = Convert.ToInt32(p);
                this.txtCodigoAR.Text = newp.ToString().PadLeft(6, '0');
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generarCodigo. Ex: " + ex.Message));
            }
        }
        private void cargarTipoClientes()
        {
            try
            {
                controladorTipoCliente contTipoCliente = new controladorTipoCliente();
                this.DropListTipoAR.DataSource = contTipoCliente.obtenerTiposClientes();
                this.DropListTipoAR.DataValueField = "id";
                this.DropListTipoAR.DataTextField = "tipo";

                this.DropListTipoAR.DataBind();
                //this.DropListTipoAR.SelectedValue = this.DropListTipoAR.Items.FindByText("Empresa").Value;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista tipo cliente. " + ex.Message));
            }
        }
        private void cargarIvaClientes()
        {
            try
            {
                this.DropListIvaAR.DataSource = this.contCliente.obtenerIvaClientes();
                this.DropListIvaAR.DataValueField = "id";
                this.DropListIvaAR.DataTextField = "descripcion";

                this.DropListIvaAR.DataBind();
                ListItem ls = new ListItem();
                ls.Text = "Seleccione...";
                ls.Value = "-1";

                this.DropListIvaAR.Items.Insert(0, ls);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
            }
        }
        private void cargarGrupoClientes()
        {
            try
            {
                controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();
                this.DropListGrupoAR.DataSource = contGrupoCliente.obtenerGruposClientes();
                this.DropListGrupoAR.DataValueField = "id";
                this.DropListGrupoAR.DataTextField = "descripcion";

                this.DropListGrupoAR.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando fun: cargarGrupoClientes. Ex: " + ex.Message));
            }
        }
        #endregion

        #endregion
        override protected void OnInit(EventArgs e)
        {
            try
            {
                this.cargasInicialesModoImagen();
            }
            catch (Exception)
            {

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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "39")
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.ListSucursal.Attributes.Remove("disabled");
                                this.ListEmpresa.Attributes.Remove("disabled");
                            }
                            else
                            {

                            }
                            valor = 1;
                        }
                        if (s == "75")
                        {
                            this.ListSucursal.Attributes.Remove("disabled");
                            this.ListEmpresa.Attributes.Remove("disabled");
                        }

                        //Permiso para que pueda modificar forma de pago
                        if (s == "123")
                            this.DropListFormaPago.Attributes.Remove("disabled");

                        //Permiso para bloquear la lista de precios
                        if (s == "150")
                            this.DropListLista.Attributes.Add("disabled", "disabled");
                    }
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        #region FACTURAR IMAGENES PANADERIA
        [WebMethod]
        public static string SetearEnLaSessionIdArticuloYCantidad(string idArticulo, string cantidad)
        {
            try
            {
                Page objp = new Page();
                objp.Session["idArticuloCalculadora"] = idArticulo;
                objp.Session["cantidadArticuloCalculadora"] = cantidad;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string resultadoJSON = serializer.Serialize(idArticulo);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string SetearEnLaSessionIdArticulo(string idArticulo)
        {
            try
            {
                Page objp = new Page();
                objp.Session["idArticuloCalculadora"] = idArticulo;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string resultadoJSON = serializer.Serialize(idArticulo);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void agregarArticuloAventa_Click(object sender, EventArgs e)
        {
            try
            {
                int idArticulo;
                if (!int.TryParse(idArticuloHidden.Value, out idArticulo))
                {
                    return;
                }
                decimal cantidadArticulo;
                if (!decimal.TryParse(cantidadArticuloHidden.Value, out cantidadArticulo))
                {
                    return;
                }
                Articulo articulo = contArticulo.obtenerArticuloByID(idArticulo);
                guardarArticuloEnFactura(articulo, cantidadArticulo);
            }
            catch (Exception ex)
            {

            }
        }

        private void agregarArticuloAventa_TextChanged(object sender, EventArgs e)
        {
            var idLinkButton = (sender as TextBox).ID;
            int idArt = Convert.ToInt32(idLinkButton.Split('_')[1]);
            Articulo articulo = contArticulo.obtenerArticuloByID(Convert.ToInt32(idArt));
            decimal cantidad = 0;

            TableRow tr;
            foreach (Control cr in this.phItemsModoImagenes.Controls)
            {
                tr = cr as TableRow;
                TextBox txtBoxCantidad = tr.Cells[3].Controls[0] as TextBox;
                int idArtPh = Convert.ToInt32(txtBoxCantidad.ID.Split('_')[1]);

                if (idArt == idArtPh)
                {
                    txtBoxCantidad.Text = txtBoxCantidad.Text.Replace(',', '.');
                    cantidad = Convert.ToDecimal(txtBoxCantidad.Text);
                }
            }
            this.guardarArticuloEnFactura(articulo, cantidad);
        }

        private void sumarArticuloImagenes(object sender, EventArgs e)
        {
            var idLinkButton = (sender as LinkButton).ID;
            int idArt = Convert.ToInt32(idLinkButton.Split('_')[1]);
            bool restar = (sender as LinkButton).ID.Contains("Restar");
            decimal cantidad = 0;
            TableRow tr;

            foreach (Control cr in this.phItemsModoImagenes.Controls)
            {
                tr = cr as TableRow;
                TextBox txtBoxCantidad = tr.Cells[3].Controls[0] as TextBox;
                int idArtPh = Convert.ToInt32(txtBoxCantidad.ID.Split('_')[1]);

                if (idArt == idArtPh)
                {
                    cantidad = Convert.ToDecimal(txtBoxCantidad.Text);
                }
            }
            Articulo articulo = contArticulo.obtenerArticuloByID(Convert.ToInt32(idArt));
            if (restar)
            {
                cantidad -= 1;
            }
            else
            {
                cantidad += 1;
            }
            guardarArticuloEnFactura(articulo, cantidad);
        }

        private void guardarArticuloEnFactura(Articulo articulo, decimal cantidad)
        {
            try
            {
                reproducirSonido();

                string cant = cantidad.ToString();

                this.txtCodigo.Text = articulo.codigo;
                this.txtCantidad.Text = cant.ToString();
                this.txtDescripcion.Text = articulo.descripcion;
                this.txtPUnitario.Text = articulo.precioVenta.ToString();

                Factura f = Session["Factura"] as Factura;

                //verifico si el articulo ya existe lo borro le sumo 1 y lo agrega a la session
                var articuloDeFactura = f.items.Where(x => x.articulo.id == Convert.ToInt32(articulo.id)).FirstOrDefault();
                int posArtEnLaSessionFactura = f.items.IndexOf(articuloDeFactura);
                if (articuloDeFactura != null)
                {
                    f.items.Remove(articuloDeFactura);
                    if (cantidad <= 0)
                    {
                        this.QuitarItem(articuloDeFactura);
                    }
                    else
                    {
                        this.cargarProductoAFactura(posArtEnLaSessionFactura);
                    }
                }
                else
                {
                    //if(f.items.Count > 0){
                    //    this.cargarProductoAFactura(f.items.Count - 1);
                    //}
                    //else
                    //{
                    this.cargarProductoAFactura(-1);
                    //}
                }
                this.cargarTablaArticulosModoImagenes();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error en agregarArticuloAventa. Excepcion: " + ex.Message));
            }
        }

        private void cargarTablaArticulosModoImagenes()//agrega el articulo seleccionado a la tabla de articulos a facturar
        {
            Factura f = Session["Factura"] as Factura;

            this.phItemsModoImagenes.Controls.Clear();
            foreach (var item in f.items)
            {
                cargarItemsTablaModoImagenes(item);
            }

            //limpio la tabla de total y la cargo devuelta
            this.phTotalModoImagen.Controls.Clear();
            //fila
            TableRow tr = new TableRow();

            TableCell celTxtTotal = new TableCell();
            celTxtTotal.Text = "<h3><b>Total:</b></h3>";
            celTxtTotal.VerticalAlign = VerticalAlign.Middle;
            celTxtTotal.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(celTxtTotal);

            TableCell celTotal = new TableCell();
            celTotal.Text = "<h3><b>" + f.total.ToString("C") + "</b></h3>";
            celTotal.VerticalAlign = VerticalAlign.Middle;
            celTotal.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(celTotal);

            this.phTotalModoImagen.Controls.Add(tr);
        }

        protected void btnFacturarImagen_Click(object sender, EventArgs e)
        {
            this.generarFactura(0);
        }

        protected void btnAbrirModalTarjeta_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnSetearFormaDePagoPorTarjeta();

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "openModalTarjeta();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.updatePanelModoImagen, updatePanelModoImagen.GetType(), "alert", "$.msgbox(\"No se pudo agregar cliente. Ex: " + ex.Message + " \");", true);
            }
        }

        private void cargasInicialesModoImagen()
        {
            //dibujo los items en la tabla
            this.cargarGruposPh();
        }

        private void cargarGruposPh()
        {
            try
            {
                List<grupo> grupos = contArticulo.obtenerGruposArticulosList().ToList();
                this.phImagenCuadroGrupos.Controls.Clear();
                foreach (var item in grupos)
                {
                    CuadroImagen cuadroImagen = (CuadroImagen)Page.LoadControl("CuadroImagen.ascx");
                    cuadroImagen.Linkbutton1.ID = item.id.ToString();
                    cuadroImagen.Label1.Text = item.descripcion;
                    cuadroImagen.Image1.ImageUrl = "/images/no_picture.jpg";
                    cuadroImagen.Linkbutton1.Click += new EventHandler(MostrarSubGruposArticulos);
                    String path = Server.MapPath("../../images/Grupos/" + item.id + "/");
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        var files = di.GetFiles();
                        foreach (var f in files)
                        {
                            cuadroImagen.Image1.ImageUrl = "../../images/Grupos/" + item.id + "/" + f.Name;
                        }
                    }

                    CargarSubGruposPh(item.id);

                    this.phImagenCuadroGrupos.Controls.Add(cuadroImagen);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarSubGruposPh(int idGrupo)
        {
            try
            {
                PlaceHolder phSubGruposArticulos = new PlaceHolder();
                phSubGruposArticulos.ID = "phSubGrupo_" + idGrupo + "_" + DateTime.Now.ToString("hhmmssfff");
                phSubGruposArticulos.Visible = false;

                //CuadroImagen cuadroImagenVolver = (CuadroImagen)Page.LoadControl("CuadroImagen.ascx");
                //cuadroImagenVolver.Linkbutton1.ID = idGrupo.ToString();
                //cuadroImagenVolver.Label1.Text = "volver";
                //cuadroImagenVolver.Image1.ImageUrl = "/images/flecha_volver.png";
                //cuadroImagenVolver.Linkbutton1.Click += new EventHandler(OcultarSubGrupos);
                //phSubGruposArticulos.Controls.Add(cuadroImagenVolver);

                var subGruposArticulos = contArticulo.obtenerSubGrupoByGrupo(idGrupo);
                foreach (var item in subGruposArticulos)
                {
                    CuadroImagen imagenSubGrupo = (CuadroImagen)Page.LoadControl("CuadroImagen.ascx");
                    imagenSubGrupo.Linkbutton1.ID = "_" + item.id.ToString();
                    imagenSubGrupo.Label1.Text = item.descripcion;
                    imagenSubGrupo.Image1.ImageUrl = "/images/no_picture.jpg";
                    imagenSubGrupo.Linkbutton1.Click += new EventHandler(mostrarArticulosSubGrupo);

                    string path = Server.MapPath("../../images/SubGrupos/" + item.id + "/");
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        var files = di.GetFiles();
                        foreach (var f in files)
                        {
                            imagenSubGrupo.Image1.ImageUrl = "../../images/SubGrupos/" + item.id + "/" + f.Name;
                        }
                    }

                    cargarArticulosPh(item.id);

                    phSubGruposArticulos.Controls.Add(imagenSubGrupo);
                }

                phImagenCuadroSubGruposGrupos.Controls.Add(phSubGruposArticulos);
            }
            catch (Exception)
            {

            }
        }

        private void cargarArticulosPh(int idSubGrupo)
        {
            try
            {
                PlaceHolder placeHolder = new PlaceHolder();
                placeHolder.ID = "phArticulo_" + idSubGrupo + "_" + DateTime.Now.ToString("hhmmssfff");
                placeHolder.Visible = false;

                var articulos = contArticuloEntity.obtenerArticulosEntityByIdSubGrupo(Convert.ToInt32(idSubGrupo)).ToList();
                foreach (var item in articulos)
                {
                    CuadroImagen cuadroImagen = (CuadroImagen)Page.LoadControl("CuadroImagen.ascx");
                    cuadroImagen.Linkbutton1.ID = "_" + item.id.ToString();
                    cuadroImagen.Label1.Text = item.descripcion.ToLower();
                    if (item.descripcion.Length >= 40)
                    {
                        cuadroImagen.Label1.Text = item.descripcion.Substring(0, 40).ToLower();
                    }
                    cuadroImagen.Image1.ImageUrl = "/images/no_picture.jpg";
                    cuadroImagen.Linkbutton1.Click += new EventHandler(this.MostrarPopUpCalculadora);
                    String path = Server.MapPath("../../images/Productos/" + item.id + "/");
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        var files = di.GetFiles();
                        foreach (var f in files)
                        {
                            cuadroImagen.Image1.ImageUrl = "../../images/Productos/" + item.id + "/" + f.Name;
                        }
                    }
                    placeHolder.Controls.Add(cuadroImagen);
                }
                phImagenCuadroArt.Controls.Add(placeHolder);
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarArticulosFavoritosPh()
        {
            try
            {
                var artStore = contArticuloEntity.obtenerArticulosStore();
                this.phImagenCuadroArticulosFavoritos.Controls.Clear();
                foreach (var item in artStore)
                {
                    CuadroImagen cuadroImagen = (CuadroImagen)Page.LoadControl("CuadroImagen.ascx");
                    cuadroImagen.Linkbutton1.ID = "_" + item.id.ToString();
                    cuadroImagen.Label1.Text = item.descripcion.ToLower();
                    if (item.descripcion.Length >= 40)
                    {
                        cuadroImagen.Label1.Text = item.descripcion.Substring(0, 40).ToLower();
                    }
                    cuadroImagen.Image1.ImageUrl = "/images/no_picture.jpg";
                    cuadroImagen.Linkbutton1.Click += new EventHandler(this.MostrarPopUpCalculadora);
                    String path = Server.MapPath("../../images/Productos/" + item.id + "/");
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        var files = di.GetFiles();
                        foreach (var f in files)
                        {
                            cuadroImagen.Image1.ImageUrl = "../../images/Productos/" + item.id + "/" + f.Name;
                        }
                    }
                    this.phImagenCuadroArticulosFavoritos.Controls.Add(cuadroImagen);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MostrarSubGruposArticulos(object sender, EventArgs e)
        {
            var idGrupo = (sender as LinkButton).ID;

            phImagenCuadroSubGruposGrupos.Visible = true;

            foreach (Control item in phImagenCuadroSubGruposGrupos.Controls)
            {
                var idPlaceHolder = item.ID.Split('_')[1];
                if (idGrupo == idPlaceHolder)
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }

            //phImagenCuadroGrupos.Visible = false;
        }

        private void OcultarSubGrupos(object sender, EventArgs e)
        {
            //phImagenCuadroSubGruposGrupos.Visible = false;
            phImagenCuadroArt.Visible = false;
            //phImagenCuadroGrupos.Visible = true;
        }

        private void mostrarArticulosSubGrupo(object sender, EventArgs e)
        {
            try
            {
                var idSubGrupo = (sender as LinkButton).ID.Split('_')[1];

                phImagenCuadroArt.Visible = true;

                foreach (Control item in phImagenCuadroArt.Controls)
                {
                    var idPlaceHolder = item.ID.Split('_')[1];
                    if (idSubGrupo == idPlaceHolder)
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }

                //phImagenCuadroGrupos.Visible = false;
                //phImagenCuadroSubGruposGrupos.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void ocultarArticulosSubGrupo(object sender, EventArgs e)
        {
            try
            {
                var idSubGrupo = (sender as LinkButton).ID;

                var grupo = contArticulo.obtenerSubGrupoID(Convert.ToInt32(idSubGrupo)).grupo.id;

                phImagenCuadroSubGruposGrupos.Visible = true;

                foreach (Control item in phImagenCuadroSubGruposGrupos.Controls)
                {
                    var idPlaceHolder = item.ID.Split('_')[1];
                    if (grupo.ToString() == idPlaceHolder)
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
                phImagenCuadroArt.Visible = false;
                phImagenCuadroGrupos.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void MostrarPopUpCalculadora(object sender, EventArgs e)
        {
            var idLinkButton = (sender as LinkButton).ID;
            int idArt = Convert.ToInt32(idLinkButton.Split('_')[1]);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MostrarCalculadora(" + idArt + ");", true);
            Session.Add("idArticuloCalculadora", idArt.ToString());
        }

        protected void btnIrAHome_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("../../Default.aspx");
            }
            catch (Exception)
            {

            }
        }

        protected void btnCancelarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove("Factura");
                Response.Redirect("ABMFacturasImagenes.aspx");
            }
            catch (Exception)
            {

            }
        }

        protected void btnSetearFormaDePagoPorTarjeta()
        {
            try
            {
                this.lbFormaDePago.Text = "Forma de pago: Tarjeta";
                this.DropListFormaPago.SelectedValue = this.DropListFormaPago.Items.FindByText("Tarjeta").Value;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: btnSetearFormaDePagoPorTarjeta. Ex:" + ex.Message));
            }
        }

        protected void btnSetearFormaDePagoPorContado(object sender, EventArgs e)
        {
            try
            {
                this.lbFormaDePago.Text = "Forma de pago: Contado";
                this.DropListFormaPago.SelectedValue = this.DropListFormaPago.Items.FindByText("Contado").Value;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: btnSetearFormaDePagoPorContado. Ex:" + ex.Message));
            }
        }

        protected void btntest_Click(object sender, EventArgs e)
        {
            string t = "hello world";
        }

        protected void reproducirSonido()
        {
            try
            {
                String path = Server.MapPath("../../content/Sounds/pulsar.wav");
                SoundPlayer soundPlayer = new SoundPlayer(path);
                soundPlayer.Play();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnAgregarArt_Click(object sender, EventArgs e)
        {
            this.cargarProductoAFactura(0);
        }

        private void cargarProductoAFactura(int posArtEnLaSessionFactura)
        {
            try
            {
                ControladorArticulosEntity contEnt = new ControladorArticulosEntity();

                if (this.txtCantidad.Text == "")
                {
                    this.txtCantidad.Text = "0";
                }
                if (this.TxtDescuentoArri.Text == "")
                {
                    this.TxtDescuentoArri.Text = "0";
                }
                if (this.txtTotalArri.Text == "")
                {
                    this.txtTotalArri.Text = "0";
                }


                Articulo artVerPromo = contArticulo.obtenerArticuloFacturar(this.txtCodigo.Text, Convert.ToInt32(this.DropListLista.SelectedValue));

                Gestion_Api.Entitys.Promocione p = contEnt.obtenerPromocionValidaArticulo(artVerPromo.id, Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.DropListFormaPago.SelectedValue), Convert.ToInt32(this.DropListLista.SelectedValue), Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")), Convert.ToDecimal(this.txtCantidad.Text));
                if (p != null)
                {
                    if (p.PrecioFijo > 0)
                        this.txtPUnitario.Text = p.PrecioFijo.Value.ToString();
                    else
                        this.TxtDescuentoArri.Text = p.Descuento.ToString();

                    this.verificarAlertaArticulo(artVerPromo);
                    this.TxtDescuentoArri.Attributes.Remove("disabled");
                    this.txtPUnitario.Attributes.Remove("disabled");
                }

                var medida = contEnt.obtenerMedidasVentaArticulo(artVerPromo.id);
                if (medida != null)
                {
                    if (medida.Count > 0 && this.chkVentaMedidaVenta.Checked == true)
                    {
                        this.txtDescripcion.Text += "(" + this.txtCantidad.Text + " " + medida.FirstOrDefault().Medida + " x " + medida.FirstOrDefault().Cantidad + ")";
                        decimal cantBulto = medida.FirstOrDefault().Cantidad.Value * Convert.ToDecimal(this.txtCantidad.Text);
                        this.txtCantidad.Text = decimal.Round(cantBulto, 2).ToString();
                    }
                }

                //recalculo total
                this.totalItem();

                //item
                ItemFactura item = new ItemFactura();
                item.articulo = contArticulo.obtenerArticuloFacturar(this.txtCodigo.Text, Convert.ToInt32(this.DropListLista.SelectedValue));
                item.cantidad = Convert.ToDecimal(this.txtCantidad.Text, CultureInfo.InvariantCulture);
                decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text, CultureInfo.InvariantCulture);
                item.porcentajeDescuento = Convert.ToDecimal(this.TxtDescuentoArri.Text, CultureInfo.InvariantCulture);
                item.total = Convert.ToDecimal(this.txtTotalArri.Text, CultureInfo.InvariantCulture);

                //cargo la descripcion del articulo que tengo en pantalla
                item.articulo.descripcion = this.txtDescripcion.Text;

                //agrego//costos
                item.Costo = item.articulo.costo;
                item.costoImponible = item.articulo.costoImponible;
                item.CostoReal = item.articulo.costoReal;
                //agrego iva 
                //SI ES FACTURA EXPORTACION LE DEJO 0%
                item.porcentajeIva = item.articulo.porcentajeIva;
                if (this.labelNroFactura.Text.Contains("Factura E") || this.labelNroFactura.Text.Contains("Nota de Credito E") || this.labelNroFactura.Text.Contains("Nota de Debito E"))
                {
                    item.porcentajeIva = 0;
                    item.articulo.porcentajeIva = 0;
                }

                if (this.txtPUnitario.Text.Contains(','))
                {
                    this.txtPUnitario.Text = this.txtPUnitario.Text.Replace(",", "");
                }

                item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text, CultureInfo.InvariantCulture);
                //en base al precio unitario calculo iva del item
                item.precioSinIva = decimal.Round(item.precioUnitario / (1 + (item.articulo.porcentajeIva / 100)), 2);

                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["PrecioFacturaA"]) && WebConfigurationManager.AppSettings["PrecioFacturaA"] == "1")
                {
                    if (this.labelNroFactura.Text.Contains("Factura A") || this.labelNroFactura.Text.Contains("Nota de Credito A") || this.labelNroFactura.Text.Contains("Nota de Debito A"))
                    {
                        item.precioSinIva = item.precioUnitario;
                    }
                }

                //guardo los precios originales por si hago recalculos por recargo con tarjeta de credito
                item.precioSinRecargo = item.precioSinIva;
                item.precioVentaSinRecargo = item.precioUnitario;
                item.porcentajeIIBB = item.articulo.ingBrutos;
                item.porcentajeOtrosImpuestos = item.articulo.impInternos;

                //Si es factura de combustibles, seteo los valores al item
                if (Convert.ToInt32(this.ListProveedorCombustible.SelectedValue) > 0 && item.articulo.grupo.descripcion.ToLower().Contains("combustible"))
                {
                    decimal totalItc = 0;
                    decimal totalHidrica = 0;
                    decimal totalVial = 0;
                    decimal totalMunicipal = 0;

                    var datos = contEnt.obtenerDatosCombustibleByArticuloProveedor(item.articulo.id, Convert.ToInt32(ListProveedorCombustible.SelectedValue));
                    if (datos != null)
                    {
                        totalItc += decimal.Round((datos.ITC.Value), 2);
                        totalHidrica += decimal.Round((datos.TasaHidrica.Value), 2);
                        totalVial += decimal.Round((datos.TasaVial.Value), 2);
                        totalMunicipal += decimal.Round((datos.TasaMunicipal.Value), 2);
                    }

                    decimal precioConIva = decimal.Round(item.precioUnitario * (1 + (item.articulo.porcentajeIva / 100)), 2);

                    item.precioSinIva = item.precioUnitario; //Como es factura de combustible, hago esta asignacion, ya que en este momento el precio unitario del item es el precio del item sin iva
                    item.precioUnitario = precioConIva + totalItc + totalHidrica + totalVial + totalMunicipal;
                }

                if (desc > 0)
                {
                    decimal tot = decimal.Round(item.precioUnitario * item.cantidad, 2);
                    decimal totDesc = decimal.Round(tot * (desc / 100), 2, MidpointRounding.AwayFromZero);
                    //item.descuento = decimal.Round(totDesc, 2);
                    item.descuento = totDesc;
                }
                else
                {
                    item.descuento = 0;
                }

                ////si es importado cargo los datos de despacho si tiene alguno cargado
                //this.agregarInfoDespachoItem(item);
                this.factura.items.Add(item);

                //lo agrego al session
                if (Session["Factura"] == null)
                {
                    Factura fac = new Factura();
                    Session.Add("Factura", fac);
                }
                Factura f = Session["Factura"] as Factura;

                if (!String.IsNullOrEmpty(this.txtRenglon.Text))
                    item.nroRenglon = Convert.ToInt32(this.txtRenglon.Text);
                else
                    item.nroRenglon = f.items.Count() + 1;

                if (posArtEnLaSessionFactura != -1)
                {
                    f.items.Insert(posArtEnLaSessionFactura, item);
                }
                else
                {
                    f.items.Add(item);
                }

                Session.Add("Factura", f);

                //lo dibujo en pantalla
                this.cargarItems();

                //agrego abajo
                //this.factura.items.Add(item);
                //actualizo totales
                this.actualizarTotales();

                //borro los campos
                this.borrarCamposagregarItem();
                //this.UpdatePanel1.Update();
                this.txtCodigo.Focus();

                this.lblMontoOriginal.Text = f.total.ToString();
                this.lblTotalMutuales.Text = decimal.Round(this.factura.total, 2).ToString();
                this.lblTotalOriginalMutuales.Text = decimal.Round(this.factura.total, 2).ToString();

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando articulos." + ex.Message + " \", {type: \"error\"});", true);
            }
        }

        private void cargarItemsTablaModoImagenes(ItemFactura item)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = item.articulo.codigo.ToString();
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.articulo.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = item.precioUnitario.ToString();
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celCantidad = new TableCell();
                TextBox txtCantidadImagenes = new TextBox();
                txtCantidadImagenes.ID = "_" + item.articulo.id;
                txtCantidadImagenes.Text = item.cantidad.ToString();
                txtCantidadImagenes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtCantidadImagenes.CssClass = "form-control disabled";
                txtCantidadImagenes.Width = 55;
                txtCantidadImagenes.Attributes.Add("Disabled", "Disabled");
                txtCantidadImagenes.TextChanged += new EventHandler(this.agregarArticuloAventa_TextChanged);
                txtCantidadImagenes.AutoPostBack = true;
                celCantidad.Controls.Add(txtCantidadImagenes);
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celTotal = new TableCell();
                celTotal.Text = Decimal.Round(item.precioUnitario * item.cantidad, 2).ToString();
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celAccion = new TableCell();//botones sumar restar

                LinkButton btnEditarCantidad = new LinkButton();
                btnEditarCantidad.ID = "btnEditarCantidad" + item.articulo.id;
                btnEditarCantidad.CssClass = "btn btn-info";
                btnEditarCantidad.Text = "<span class='shortcut-icon icon-pencil'></span>";

                btnEditarCantidad.OnClientClick = "return MostrarCalculadoraEditarCantidad(" + item.articulo.id + ");";
                celAccion.Controls.Add(btnEditarCantidad);

                lb_IdArticulo_ModalCalculadora.Text = item.articulo.id.ToString();

                //Literal l = new Literal();
                //l.Text = "&nbsp";
                //celAccion.Controls.Add(l);

                //LinkButton btnRestar = new LinkButton();
                //btnRestar.ID = "btnRestar_" + item.articulo.id;
                //btnRestar.CssClass = "btn btn-info";
                //btnRestar.Text = "<span class='shortcut-icon icon-minus'></span>";
                //btnRestar.Click += new EventHandler(this.sumarArticuloImagenes);
                //celAccion.Controls.Add(btnRestar);

                //Literal l2 = new Literal();
                //l2.Text = "&nbsp";
                //celAccion.Controls.Add(l2);

                //LinkButton btnSumar = new LinkButton();
                //btnSumar.ID = "btnSumar_" + item.articulo.id;
                //btnSumar.CssClass = "btn btn-info";
                //btnSumar.Text = "<span class='shortcut-icon icon-plus'></span>";
                //btnSumar.Click += new EventHandler(this.sumarArticuloImagenes);
                //celAccion.Controls.Add(btnSumar);

                tr.Cells.Add(celAccion);

                this.phItemsModoImagenes.Controls.Add(tr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnSetearClienteScrap_Click(object sender, EventArgs e)
        {
            try
            {
                int idClienteScrap = contClienteEntity.ObtenerIdClienteScrapParaPanaderias();
                if (idClienteScrap == 0)
                {
                    idClienteScrap = CrearElClienteScrapYObtenerElId();
                }
                this.cargarCliente(idClienteScrap);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        public int CrearElClienteScrapYObtenerElId()
        {
            try
            {
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cli = new Cliente();
                    Session.Add("ClientesABM_Cliente", cli);
                }
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                string perfil = Session["Login_NombrePerfil"] as string;

                string p = this.contCliente.obtenerLastCodigoCliente();
                int newp = Convert.ToInt32(p);
                cliente.codigo = newp.ToString().PadLeft(6, '0');

                cliente.tipoCliente.id = 6;

                cliente.razonSocial = "Scrap";

                CrearElGrupoSiNoExiste("Scrap");

                cliente.grupo.id = contGrupoCliente.obtenerGrupoDesc("Scrap").id;
                cliente.categoria.id = 1;
                cliente.estado.id = 1;
                cliente.cuit = "00000000000";
                cliente.iva = "13";
                cliente.pais.id = 1;//ARGENTINA
                cliente.expreso.id = 1;
                string saldMax = "0";
                cliente.saldoMax = Convert.ToDecimal(saldMax);
                cliente.vencFC = 0;
                cliente.descFC = 0;
                cliente.observaciones = "";

                //alerta cliente                
                cliente.alerta.descripcion = "";
                cliente.alerta.idCliente = cliente.id;

                cliente.hijoDe = 0;
                cliente.alias = "Scrap";

                cliente.vencFC = 0;

                cliente.lisPrecio.id = 1;
                cliente.formaPago.id = 7;//cuenta corriente

                cliente.origen = 1;

                var Vendedor = contVendedorEntity.ObtenerPrimerVendedorDisponible();
                cliente.vendedor.id = Vendedor.id;
                cliente.sucursal.id = (int)Vendedor.sucursal;//preguntar
                cliente.estado.id = 1;

                if (contCliente.agregarCliente(cliente) > 0)
                {
                    if (contClienteEntity.agregarClienteDatos(new Gestion_Api.Entitys.Cliente_Datos
                    {
                        IdCliente = cliente.id
                    }) > 0)
                    {}
                }
                return cliente.id;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
                return 0;
            }
        }

        void CrearElGrupoSiNoExiste(string nombreDelGrupo)
        {
            try
            {
                string grupoDB = contGrupoCliente.obtenerGrupoDesc(nombreDelGrupo).descripcion;
                if (grupoDB != nombreDelGrupo)
                {
                    int i = contGrupoCliente.agregarGrupo(nombreDelGrupo);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }
        #endregion
    }
}