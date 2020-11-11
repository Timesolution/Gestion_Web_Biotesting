using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMFacturas : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFacturacion controlador = new controladorFacturacion();
        controladorUsuario contUser = new controladorUsuario();
        //
        controladorArticulo contArticulo = new controladorArticulo();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        public PlaceHolder phArticulos = new PlaceHolder();
        controladorRemitos cr = new controladorRemitos();
        controladorSucursal cs = new controladorSucursal();
        ControladorPedido cp = new ControladorPedido();
        controladorTarjeta ct = new controladorTarjeta();

        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();

        //factura
        Factura factura = new Factura();
        Cliente cliente = new Cliente();
        TipoDocumento tp = new TipoDocumento();

        int accion;
        int idEmpresa;
        int idSucursal;
        int idPtoVentaUser;

        //flag si cambio la fecha de la factura
        int flag_cambioFecha = 0;

        int flag_clienteModal = 0;

        DataTable lstPagosTemp;
        DataTable dtTrazasTemp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                   
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);

                btnAgregar.Attributes.Add("onclick", " this.disabled = true;  " + btnAgregarRemitir.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                btnAgregarRemitir.Attributes.Add("onclick", " this.disabled = true; " + btnAgregar.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarRemitir, null) + ";");
                btnRefacturar.Attributes.Add("onclick", " this.disabled = true; " +  ClientScript.GetPostBackEventReference(btnRefacturar, null) + ";");

                //dibujo los items en la tabla
                if (Session["Factura"] != null)
                {
                    this.cargarItems();
                }
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());

                    Session["CobroAnticipo"] = null;
                    Session["PagoCuentaAnticipo"] = null;
                    Session["PagoCuentaAnticipoMutual"] = null;
                    Session["Factura"] = null;
                    Session["FacturasABM_ArticuloModalMultiple"] = null;
                    Session["FacturasABM_ArticuloModal"] = null;
                    phArticulos.Controls.Clear();
                    
                    //Si tiene configurada la pantalla vieja lo redirecciono
                    string pantalla = WebConfigurationManager.AppSettings.Get("FormularioFC");
                    if (pantalla == "1")
                    {
                        //obtengo la url para salvar los parametros que tenga asi los envio al prox formulario
                        string urlActual = Request.Url.ToString();
                        string urlNueva = urlActual.Replace("ABMFacturas", "ABMFacturasLargo");
                        Response.Redirect(urlNueva);
                    }
                    if (pantalla == "2")
                    {
                        //obtengo la url para salvar los parametros que tenga asi los envio al prox formulario
                        string urlActual = Request.Url.ToString();
                        string urlNueva = urlActual.Replace("ABMFacturas", "ABMFacturasUnidadMedida");
                        Response.Redirect(urlNueva);
                    }
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
                    this.cargarEmpresas();
                    this.cargarOperadores();
                    //this.cargarTarjetas();
                    this.ListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
                    this.ListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));

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

                    this.lblAcordeonSuc.InnerText = this.ListSucursal.SelectedItem.Text + " - " + this.ListPuntoVenta.SelectedItem.Text;
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
                        GenerarNotaCredito(facturas);
                    }

                    //genero factura desde refacturacion PRP
                    if (this.accion == 9)
                    {
                        string presupuestos = Request.QueryString["prps"];
                        this.GenerarRefacturacion(presupuestos);
                    }

                    //si vengo del modulo de mascotas para facturar agenda
                    //cargo el cliente del propietario
                    if (this.accion == 11)
                    {
                        int idcliente = Convert.ToInt32(Request.QueryString["cliente"]);
                        this.cargarClienteEnLista(idcliente);
                        this.cargarCliente(idcliente);
                    }
                    
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

                //verifico si es postback y tengo que llenar la tabla de las trazas para poder obtener el estado de los chkbox
                if (this.lblMovTraza.Text != "")
                {
                    string idTraza = this.lblMovTraza.Text;
                    int idArticulo = Convert.ToInt32(idTraza.Split('_')[1]);
                    this.CargarTrazasPH(idArticulo);
                }

                if (this.accion == 9)
                {
                    this.btnAgregar.Visible = false;
                    this.btnAgregarRemitir.Visible = false;
                    this.btnRefacturar.Visible = true;
                }
                //cargo el numero de remito
                //this.obtenerNroFactura();
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                if (!listPermisos.Contains("173"))
                    return 0;

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
                            return 1;
                        }
                        if (s == "75")
                        {
                            this.ListSucursal.Attributes.Remove("disabled");
                            this.ListEmpresa.Attributes.Remove("disabled");
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
                //Factura f = controlador.AsignarRemito(r);
                Factura f = new Factura();

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
            }
            catch(Exception ex)
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
                Factura f = controlador.GenerarFacturaDesdePedido(pedidos, Convert.ToInt32(recalcularPrecio));
                
                Session.Add("Factura", f);
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);
                //this.cargarCliente(f.cliente.id);

                //this.DropListClientes.SelectedValue = f.cliente.id.ToString();
                //antes de cargar cliente me guardo temporalmente el descuento y subtotal
                decimal subtotal = f.subTotal;
                decimal descuento = f.descuento;
                
                //cargocliente
                Session.Add("FacturasABM_ClienteModal", f.cliente.id);
                this.cargarClienteDesdeModal();

                //pongo valores de descuentos que se cargo en el pedido
                decimal descRedondeado = decimal.Round((descuento * 100),2);
                decimal totalRedondeado = decimal.Round((subtotal + descuento),2);
                this.txtPorcDescuento.Text = decimal.Round((descRedondeado / totalRedondeado),2).ToString();

                //cargo los datos de entrega del pedido.
                this.txtFechaEntrega.Text = f.pedidos[0].fechaEntrega.ToString("dd/MM/yyyy");
                this.txtHorarioEntrega.Text = f.pedidos[0].horaEntrega;

                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                this.txtComentarios.Text = f.comentario;
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();
                
                //this.txtPorcRetencion.Text = f.retencion.ToString();
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
                
                Factura f = controlador.GenerarNotaCredito(facturas);
                //if (idped > 0)
                //{
                //    f.ped.id = idped;
                //}
                //else
                //{
                //    f.ped.id = 0;
                //}
                Session.Add("Factura", f);
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
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();

                this.cargarClienteEnLista(f.cliente.id);
                this.DropListClientes.Attributes.Add("disabled", "disabled");

                this.lbtnAccion.Visible = false;
                this.panelBusquedaCliente.Visible = false;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));

            }
        }
        public void GenerarRefacturacion(string facturas)
        {
            try
            {
                this.factura = Session["Factura"] as Factura;
                Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(facturas));
                this.cargarClienteEnLista(f.cliente.id);
                //if (idped > 0)
                //{
                //    f.ped.id = idped;
                //}
                //else
                //{
                //    f.ped.id = 0;
                //}
                Session.Add("Factura", f);
                this.ListEmpresa.SelectedValue = f.empresa.id.ToString();
                this.cargarSucursal(f.empresa.id);
                this.cargarPuntoVta(f.sucursal.id);
                this.cargarCliente(f.cliente.id);
                this.DropListClientes.SelectedValue = f.cliente.id.ToString();
                this.DropListVendedor.SelectedValue = f.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = f.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = f.listaP.id.ToString();
                this.ListSucursal.SelectedValue = f.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = f.ptoV.id.ToString();
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroFactura();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a factura " + ex.Message));

            }
        }
        #endregion

        #region verificaciones iniciales
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
            catch(Exception ex)
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
            catch(Exception ex)
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
                    
                    if (this.labelNroFactura.Text.Contains("Credito"))
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
                bool flag = false;
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "72")
                        {
                            this.lbtnNC.Visible = true;
                            this.lbNC.Visible = true;
                            flag = true;
                        }
                    }
                }
                if (flag == false)
                {
                    this.ListDocumentos.Items.Remove("NC");
                    this.ListDocumentos.Items.Remove("NC PRP");
                }
            }
            catch(Exception ex)
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

                Cliente c = contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    script += "$.msgbox(\"Alerta Cliente: " + c.alerta.descripcion + ". \");";
                    alerta1 += "Alerta Cliente: " + c.alerta.descripcion + "." + "<br>";
                }

                controladorCuentaCorriente contCC = new controladorCuentaCorriente();
                decimal saldoMax = contCC.saldoCuentaPorCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));

                if (saldoMax >= this.cliente.saldoMax && this.cliente.saldoMax > 0)
                {
                    script += "$.msgbox(\"Alerta: Cliente con saldo max. superado.($" + c.saldoMax + ") \");";
                    alerta2 += "Alerta: Cliente con saldo max. superado.($" + c.saldoMax + ")." + "<br>";
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
                            if ( (DateTime.Today.DayOfYear - fechaImpaga.DayOfYear) > c.vencFC)
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

                if (script != "")
                {                    
                    if (this.flag_clienteModal > 0)//si vienen desde modal uso un script sino uso el otro.
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta1 + alerta2 + alerta3));
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta.Split(';')[1]));
                    }
                    else
                    {
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
                if (alert.descripcion != "" && alert != null)
                {
                    //concateno alerta
                    script += "$.msgbox(\"Alerta: " + alert.descripcion + ". \");";
                    alerta += "Alerta: " + alert.descripcion + ".<br>";
                }

                //busco si el articulo ya esta en la factura mediante el codigo
                Factura f = Session["Factura"] as Factura;
                var a = f.items.Where(x => x.articulo.codigo == art.codigo).FirstOrDefault();
                if (a != null)
                {
                    //si esta concateno la alerta
                    script += "$.msgbox(\"Este articulo ya fue cargado previamente a la factura: Cod.:" + art.codigo + " \");";
                    alerta += "Este articulo fue cargado previamente a la factura: Articulo:" + art.codigo;
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
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
        #endregion 

        #region cargar Datos iniciales

        //public void cargarTipoFactura()
        //{
        //    try
        //    {
        //        //DataTable dt = controlador.obtenerTipoFactura();

        //        ////agrego todos
        //        //DataRow dr = dt.NewRow();
        //        //dr["tipo"] = "Seleccione...";
        //        //dr["id"] = -1;
        //        //dt.Rows.InsertAt(dr, 0);

        //        //this.DropListTipoDoc.DataSource = dt;
        //        //this.DropListTipoDoc.DataValueField = "id";
        //        //this.DropListTipoDoc.DataTextField = "tipo";

        //        //this.DropListTipoDoc.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos Factura. " + ex.Message));
        //    }
        //}
        public void cargarVendedor()
        {
            try
            {
                if (DropListVendedor.Items.Count > 0)
                {
                    DropListVendedor.Items.Clear();
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
                }

                

                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
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

                this.lblAcordeonSuc.InnerText = this.ListSucursal.SelectedItem.Text + " - " + this.ListPuntoVenta.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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

                this.ListFormaVenta.Items.Insert(0, new ListItem("NORMAL", "-1"));
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
                ControladorFormasPago contForma = new ControladorFormasPago();
                List<Gestion_Api.Entitys.listasPrecio> listas = contForma.obtenerListasByFormaPago(Convert.ToInt32(this.DropListFormaPago.SelectedValue));                

                if (listas != null && listas.Count > 0)
                {
                    listas = listas.OrderBy(o => o.nombre).ToList();
                    this.DropListLista.DataSource = listas;
                    this.DropListLista.DataValueField = "id";
                    this.DropListLista.DataTextField = "nombre";
                    this.DropListLista.DataBind();
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
        public void cargarTarjetasByOperador(int idOperador)
        {
            try
            {
                List<Gestion_Api.Entitys.Tarjeta> tarjetas = this.ct.obtenerTarjetasEntityByOperador(idOperador);

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

                if (this.cliente != null)
                {

                    this.labelCliente.Text = this.cliente.razonSocial.Replace('-',' ') + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                    this.lblAcordeon1.InnerText = this.cliente.razonSocial.Replace('-', ' ') + " - " + this.cliente.iva + " - " + this.cliente.cuit;
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
                    this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();
                    //verifico si el PERFIL tiene permitido vender en Cta Cte
                    this.verificarPermisoVentaCtaCte();
                    if (this.cliente.iva == "Consumidor Final")
                    {
                        Configuracion c = new Configuracion();
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
                    }
                    else
                    {
                        this.btnTarjeta.Visible = false;
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

                    //cargo el cliente en la factura session
                    Factura f = Session["Factura"] as Factura;
                    //f.cliente.id = this.cliente.id;
                    f.cliente = contCliente.obtenerClienteID(this.cliente.id);
                    Session.Add("Factura", f);
                    this.verificarAlerta();
                    Session["FacturasABM_ClienteModal"] = null;

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

                                if (this.ListSucursalCliente.Items.Contains(this.ListSucursal.SelectedItem) == true)
                                {
                                    ListItem item = this.ListSucursalCliente.Items.FindByValue(this.ListSucursal.SelectedValue);
                                    this.ListSucursalCliente.Items.Remove(item);
                                }

                            }
                            else
                            {
                                this.ListSucursalCliente.Visible = false;
                            }
                        }
                        else
                        {
                            this.ListSucursalCliente.Visible = false;
                            this.btnAgregar.Visible = false;
                            this.btnAgregarRemitir.Visible = false;
                        }
                    }
                    else
                    {
                        this.ListSucursalCliente.Visible = false;
                    }

                    this.lblAcordeonFPLista.InnerText = this.DropListFormaPago.SelectedItem.Text + " - " + this.DropListLista.SelectedItem.Text;                         
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
                    if(this.DropListClientes.SelectedValue == "-1")
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
            catch(Exception ex)
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
                Configuracion c = new Configuracion();
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
                        this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                        this.ListDocumentos.SelectedValue = "FC";
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
                            this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                            this.ListDocumentos.SelectedValue = "FC";
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
                                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                                this.ListDocumentos.SelectedValue = "PRP";
                            }
                            else
                            {
                                #region Old
                                //int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                                //PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                                ////como estoy en cotizacion pido el ultimo numero de este documento
                                //int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura B");
                                //this.labelNroFactura.Text = "Factura B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                                //this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                                //this.ListDocumentos.SelectedValue = "FC";
                                #endregion

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
                            //int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            //PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                            //this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                        }
                    }

                    #region Old
                    //string[] cliente = this.labelCliente.Text.Split('-');
                    //if (cliente[1].Contains("Responsable Inscripto"))
                    //{
                    //    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    //    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //    //como estoy en cotizacion pido el ultimo numero de este documento
                    //    int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito A");
                    //    this.labelNroFactura.Text = "Nota de Credito A N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    //    this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                    //    this.ListDocumentos.SelectedValue = "NC";
                    //}
                    //else
                    //{
                    //    if (cliente[1].Contains("No Informa"))
                    //    {
                    //        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    //        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //        //como estoy en cotizacion pido el ultimo numero de este documento
                    //        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito PRP");
                    //        this.labelNroFactura.Text = "Nota de Credito PRP N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    //        this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                    //        this.ListDocumentos.SelectedValue = "NC PRP";
                    //    }
                    //    else
                    //    {
                    //        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    //        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //        //como estoy en cotizacion pido el ultimo numero de este documento
                    //        int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                    //        this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    //        this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                    //        this.ListDocumentos.SelectedValue = "NC";
                    //    }
                    //}
                    #endregion

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
                    this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                    this.ListDocumentos.SelectedValue = "NC";
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
                        this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                        this.ListDocumentos.SelectedValue = "NC";
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
                            this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                            this.ListDocumentos.SelectedValue = "NC PRP";
                        }
                        else
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //como estoy en cotizacion pido el ultimo numero de este documento
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                            this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                            this.ListDocumentos.SelectedValue = "NC";
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
                    this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                    this.ListDocumentos.SelectedValue = "ND";
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
                        this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                        this.ListDocumentos.SelectedValue = "ND";
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
                            this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                            this.ListDocumentos.SelectedValue = "ND PRP";
                        }
                        else
                        {
                            int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                            PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                            //como estoy en cotizacion pido el ultimo numero de este documento
                            int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito B");
                            this.labelNroFactura.Text = "Nota de Debito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                            this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                            this.ListDocumentos.SelectedValue = "ND";
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
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "ND PRP";
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
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Factura B");
                this.labelNroFactura.Text = "Factura B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "FC";

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
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Credito B");
                this.labelNroFactura.Text = "Nota de Credito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "NC";

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
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerFacturaNumero(ptoVenta, "Nota de Debito B");
                this.labelNroFactura.Text = "Nota de Debito B N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "ND";

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
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "PRP";
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
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;
                this.ListDocumentos.SelectedValue = "NC PRP";
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
            catch(Exception ex)
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
                Articulo art = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                Cliente prov = this.contCliente.obtenerProveedoresRazonSocial(art.proveedor.razonSocial);
                if (art != null)
                {
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
                    //agrego los txt
                    this.txtDescripcion.Text = art.descripcion;
                    List<Stock> stocks = this.contArticulo.obtenerStockArticulo(art.id);
                    decimal stock = 0;
                    try
                    {
                        stock = stocks.Where(x => x.sucursal.id == Convert.ToInt32(this.ListSucursal.SelectedValue)).FirstOrDefault().cantidad;
                    }
                    catch { }

                    if (stock < 0)
                    {
                        stock = 0;
                    }
                    this.lbtnStockProd.Text = stock.ToString();
                    //string[] lbl = this.labelNroFactura.Text.Split('°');
                    //if (lbl[0].Contains("Presupuesto"))
                    //{
                    //    Configuracion c = new Configuracion();
                    //    this.txtIva.Text = c.porcentajeIva;
                    //}
                    //else
                    //{
                    //    this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                    //}
                    //decimal PrecioSinIva = decimal.Round(art.precioVenta - (art.precioVenta * (art.porcentajeIva / 100)),2);
                    this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                    if(this.txtPUnitario.Text == "")
                        this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();
                    this.verificarAlertaArticulo(art);
                    //this.txtPUnitario.Text = decimal.Round(art.precioVenta,4).ToString();
                    Session["FacturasABM_ArticuloModal"] = null;
                    this.txtCantidad.Focus();
                    //recalculo total
                    this.totalItem();

                    //Si tiene configuracion CON COMMITANTE
                    //agrego automaticamente el articulo a la FC con la cant y dto que este escrito
                    Configuracion config = new Configuracion();
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
                            this.cargarProductoAFactura();
                            this.txtCodigo.Text = "";
                        }
                    }
                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra Articulo " + this.txtCodigo.Text));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encuentra Articulo. \", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Articulo. " + ex.Message));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error buscando articulo." + ex.Message + " \", {type: \"error\"});", true);
            }
        }

        protected void btnAgregarArt_Click(object sender, EventArgs e)
        {
            this.cargarProductoAFactura();
        }

        private void cargarProductoAFactura()
        {
            try
            {
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
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

                Articulo artVerPromo = contArticulo.obtenerArticuloFacturar(txtCodigo.Text, Convert.ToInt32(this.DropListLista.SelectedValue));

                Gestion_Api.Entitys.Promocione p = contArtEnt.obtenerPromocionValidaArticulo(artVerPromo.id, Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.DropListFormaPago.SelectedValue), Convert.ToInt32(this.DropListLista.SelectedValue), Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")), Convert.ToDecimal(this.txtCantidad.Text));
                if (p != null && p.FormaPago != 8)
                {
                    if (p.PrecioFijo > 0)
                        this.txtPUnitario.Text = p.PrecioFijo.Value.ToString();
                    else
                        this.TxtDescuentoArri.Text = p.Descuento.ToString();

                    this.verificarAlertaArticulo(artVerPromo);
                    this.TxtDescuentoArri.Attributes.Remove("disabled");
                    this.txtPUnitario.Attributes.Remove("disabled");
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
                item.porcentajeIva = item.articulo.porcentajeIva;

                if (this.txtPUnitario.Text.Contains(','))
                {
                    this.txtPUnitario.Text = this.txtPUnitario.Text.Replace(",","");
                }

                item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text, CultureInfo.InvariantCulture);
                //en base al precio unitario calculo iva del item
                item.precioSinIva = decimal.Round(item.precioUnitario / (1 + (item.articulo.porcentajeIva / 100)), 2);
                //guardo los precios originales por si hago recalculos por recargo con tarjeta de credito
                item.precioSinRecargo = item.precioSinIva;
                item.precioVentaSinRecargo = item.precioUnitario;

                item.porcentajeIIBB = item.articulo.ingBrutos;
                item.porcentajeOtrosImpuestos = item.articulo.impInternos;

                if (desc > 0)
                {
                    decimal tot = decimal.Round(item.precioUnitario * item.cantidad,2);
                    decimal totDesc =  decimal.Round(tot * (desc/100),2,MidpointRounding.AwayFromZero);
                    //item.descuento = decimal.Round(totDesc, 2);
                    item.descuento = totDesc;
                }
                else
                {
                    item.descuento = 0;
                }            
    
                //si es importado cargo los datos de despacho si tiene alguno cargado
                this.agregarInfoDespachoItem(item);

                this.factura.items.Add(item);
                //lo agrego al session
                if (Session["Factura"] == null)
                {
                    Factura fac = new Factura();
                    Session.Add("Factura", fac);
                }

                Factura f = Session["Factura"] as Factura;

                f.items.Add(item);
                f.items = f.items.Distinct().ToList();
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

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando articulos." + ex.Message + " \", {type: \"error\"});", true);
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
                foreach(ItemFactura item in f.items)
                {
                    pos = f.items.IndexOf(item);
                    this.agregarItemFactura(item,pos);
                   
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
                celCodigo.Text = item.articulo.codigo;
                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                //celCantidad.Text = item.cantidad.ToString();
                //celCantidad.Width = Unit.Percentage(10);
                //celCantidad.HorizontalAlign = HorizontalAlign.Center;
                //celCantidad.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celCantidad);
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
                celDescuento.Text = "$" +item.descuento.ToString();
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
                btnEliminar.Click += new EventHandler(this.QuitarItem);
                celAccion.Controls.Add(btnEliminar);

                int trazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                if (trazable > 0)
                {
                    int cargada = this.validarTrazaCargadaItemFactura(item);

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAccion.Controls.Add(l2);

                    LinkButton btnTraza = new LinkButton();
                    if (cargada > 0)
                    {
                        btnTraza.CssClass = "btn btn-info";
                    }
                    else
                    {
                        btnTraza.CssClass = "btn btn-danger";
                    }
                    btnTraza.ID = "btnTraza_" + item.articulo.id + "_" + pos;
                    btnTraza.Text = "<span class='shortcut-icon icon-road'></span>";
                    btnTraza.Click += new EventHandler(this.TrazabilidadItem);
                    celAccion.Controls.Add(btnTraza);
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

                //obtengo total de suma de item
                decimal totalC = this.factura.obtenerTotalNeto();
                decimal total = decimal.Round(totalC, 2,MidpointRounding.AwayFromZero);
                this.factura.neto = total;

                if (this.accion == 6)// si viene de generar nota de credito mantengo el descuento que le habia hecho a la factura
                {
                    this.txtPorcDescuento.Text = decimal.Round(this.factura.neto10, 2).ToString();
                }

                //Subtotal = neto menos el descuento
                this.factura.descuento = decimal.Round((this.factura.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100)),2,MidpointRounding.AwayFromZero);
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

                        this.factura.neto21 = decimal.Round((iva - decimal.Round(descuento, 2)),2, MidpointRounding.AwayFromZero);
                    }                    
                }

                this.factura.totalSinDescuento = decimal.Round(this.factura.neto + this.factura.obtenerTotalIva(),2);

                //retencion sobre el sub total
                this.factura.retencion = decimal.Round((this.factura.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100)),2, MidpointRounding.AwayFromZero);

                //percepcion iva == checked
                if (this.chkIvaNoInformado.Checked == true)
                {
                    this.factura.iva10 = decimal.Round(this.factura.neto * Convert.ToDecimal(0.105),2);//iva 10.5%
                }
                else
                {
                    this.factura.iva10 = 0;
                }

                //total: subtotal + iva + retencion + ivaPercepcion
                this.factura.total = decimal.Round((this.factura.subTotal + this.factura.neto21 + this.factura.iva10 + this.factura.retencion),2,MidpointRounding.AwayFromZero);                

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
                this.lblMontoOriginal.Text = decimal.Round(this.factura.total, 2).ToString();

                //cargo los label del acordeon
                this.lblAcordeonNeto.InnerText = "Neto: $" + neto;
                this.lblAcordeonIva.InnerText = "Iva: $" + decimal.Round(this.factura.neto21, 2).ToString();
                this.lblAcordeonSub.InnerText = "Subtotal: $" + decimal.Round(this.factura.subTotal, 2).ToString();
                this.lblAcordeonTotal.InnerText = "Total: $" + decimal.Round(this.factura.total, 2).ToString();
                this.lblCartelTotal.Text = "Total: $" + decimal.Round(this.factura.total, 2).ToString();
                this.lblAcordeon2.InnerText = "Total: $" + decimal.Round(this.factura.total, 2).ToString();
                //--*

                Factura f = this.factura;
                Session.Add("Factura", f);
            }

            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));

            }
        }

        private bool VerificarDescripcionDeItems()
        {
            try
            {
                Factura factura = Session["Factura"] as Factura;

                foreach (var item in factura.items)
                {
                    if (string.IsNullOrEmpty(item.articulo.descripcion))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool VerificarFacturaEnCero()
        {
            try
            {
                Factura factura = Session["Factura"] as Factura;

                if (factura.EsFactura() && factura.total <= 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
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
                    if (!VerificarFacturaEnCero())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El total de la Factura debe ser mayor a 0. \");", true);
                        return;
                    }

                    if (!VerificarDescripcionDeItems())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Hay items sin descripción. \");", true);
                        return;
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
                                    if(this.validarItemsEnCero() < 1)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                        return;
                                    }
                                    this.generarFactura(0);
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
                        int j = validarUltimaFactura();
                        if (j == 1)
                        {
                            int t = this.validarTrazasCargadas();
                            if (t == 1)
                            {
                                if (this.validarItemsEnCero() < 1)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                    return;
                                }
                                this.generarFactura(0);
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
                    fact.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                    fact.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    fact.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));

                    string[] lbl = this.labelNroFactura.Text.Split('°');
                    fact.tipo = this.cargarTiposFactura(lbl[0]);
                    fact.formaPAgo = controlador.obtenerFormaPagoFP(this.DropListFormaPago.SelectedItem.Text);
                    //agrego el porcentaje de descuento
                    fact.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);
                    //obtengo el Neto no gravado || de los items con alicuota 0%
                    fact.iva21 = this.obtenerNetoNoGravado(fact);

                    int user = (int)Session["Login_IdUser"];
                    string presupuestos = Request.QueryString["prps"];

                    int i = this.controlador.ProcesoRefacturarPRPEditado(null,fact, user,presupuestos);
                    if (i > 0)
                    {
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
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturas.aspx';", true);
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

        private int validarSaldoTarjeta()
        {
            try
            {
                //decimal total = 0;
                //foreach (var item in phPagosTarjeta.Controls)
                //{
                //    TableRow row = item as TableRow;
                //    total += Convert.ToDecimal(row.Cells[2].Text.ToString());
                //}

                //if (total == Convert.ToDecimal(this.lblSaldoTarjeta.Text.ToString()))
                //{
                //    return 1;
                //}
                //else
                //{
                //    return -1;
                //}
                //return 1;
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
                        if (Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")) >= ultimaFact.fecha)
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando monto igresado con tarjeta." + ex.Message));
                return -2;
            }
        }

        private int validarTrazasCargadas()
        {
            try
            {
                Factura f = Session["Factura"] as Factura;
                if (f != null)
                {
                    foreach (ItemFactura item in f.items)
                    {
                        int esTrazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                        if (esTrazable == 1 && item.cantidad>0)
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

        private int validarFacturarTotalCero(Factura f)
        {
            try
            {
                controladorListaPrecio controlLista = new controladorListaPrecio();
                listaPrecio lista = controlLista.obtenerlistaPrecioID(Convert.ToInt32(this.DropListLista.SelectedValue));

                //si esta facturando con listas de precio con dto al 100% lo dejo seguir
                foreach (ItemFactura item in f.items)
                {
                    SubListaPrecio sl = controlLista.obtenerSubListaProducto(lista.id, item.articulo.listaCategoria.id);
                    //if (sl.porcentaje < 100 && sl.AumentoDescuento != 2)
                    if (sl.porcentaje >= 100 && sl.AumentoDescuento == 2)
                    {
                        return 1;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        protected void btnAgregarRemitir_Click(object sender, EventArgs e)
        {
            //if (this.DropListFormaPago.SelectedItem.Text == "Tarjeta")
            //{
            //    int i = validarSaldoTarjeta();
            //    if (i == 1)
            //    {
            //        this.generarFactura(1);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"El monto ingresado en la forma de pago tarjeta no es igual al monto total de la factura. \", {type: \"error\"});", true);
            //        //m.mensajeBoxError("El monto ingresado en la forma de pago tarjeta no es igual al monto total de la factura.");
            //    }
            //}
            //else
            //{
            //    this.generarFactura(1);
            //}       
            
            if (IsValid)
            {
                if (this.DropListFormaPago.SelectedItem.Text == "Tarjeta")
                {

                    int i = validarSaldoTarjeta();
                    if (i == 1)
                    {
                        int j = validarUltimaFactura();
                        if (j == 1)
                        {
                            int t = this.validarTrazasCargadas();
                            if(t == 1)
                            {
                                if (this.validarItemsEnCero() < 1)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                    return;
                                }
                                this.generarFactura(1);
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
                            if (this.validarItemsEnCero() < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No es posible procesar articulos con importe 0. \", {type: \"error\"});", true);
                                return;
                            }
                            this.generarFactura(1);
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
                //List<ItemFactura> items = this.obtenerItems();
                Factura fact = Session["Factura"] as Factura;
                //List<ItemFactura> items = fact.items;

                //agrego info traza a los items
                fact.items = this.agregarInfoTrazaFactura(fact);                

                if (fact.items.Count > 0)
                {                    
                    
                    //valido que si esta facturando con lista de precio al 100% dto
                    if (fact.total == 0)
                    {
                        int verificaTotalCero = this.validarFacturarTotalCero(fact);
                        if (verificaTotalCero < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar en monto cero. \", {type: \"error\"});", true);
                            return;
                        }
                    }

                    //obtengo datos
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
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No puede ingresar un descuento mayor al " + dtoMax + "%."));
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
                    }
                    
                    fact.vendedor.id = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                    fact.comentario = this.txtComentarios.Text;
                    if (this.chkIvaNoInformado.Checked == true)
                    {
                        fact.comentario += " - Percepcion IVA a Consumidor Final ($" + this.factura.iva10 + ").";
                    }
                    fact.fechaEntrega = this.txtFechaEntrega.Text;
                    fact.horaEntrega = this.txtHorarioEntrega.Text;
                    

                    //agrego el porcentaje de descuento
                    fact.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);

                    fact.formaPAgo = controlador.obtenerFormaPagoFP(this.DropListFormaPago.SelectedItem.Text);

                    fact.listaP.id = Convert.ToInt32(this.DropListLista.SelectedValue);

                    string[] lbl = this.labelNroFactura.Text.Split('°');

                    fact.tipo = this.cargarTiposFactura(lbl[0]);

                    DataTable dtPago = lstPago;
                    if (fact.formaPAgo.forma == "Tarjeta")
                    {
                        if (dtPago.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se agregaron pagos con tarjeta \", {type: \"error\"});", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se agregaron pagos con tarjeta"));
                            return;
                        }
                    }
                    Configuracion c = new Configuracion();
                    if (c.consumidorFinalCC != "1")
                    {
                        if (fact.cliente.iva == "Consumidor Final" && fact.formaPAgo.forma.Contains("Cuenta Corriente"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar en cta. cte. a consumidor final. \", {type: \"error\"});", true);
                            return;
                        }
                    }
                    //s
                    //obtengo si tengo que facturar a otra sucursal
                    //int sucursalFacturar = 0;
                    if (this.ListSucursalCliente.Visible == true)
                    {
                        fact.sucursalFacturada = Convert.ToInt32(this.ListSucursalCliente.SelectedValue);
                    }

                    //int fiscalConsumidor = this.validarTotalConsumidorFinal(fact);
                    //if (fiscalConsumidor < 0)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar mas de $1000 a Consumidor Final desde Punto de venta tipo Fiscal. \", {type: \"error\"});", true);
                    //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede facturar mas de $1000 a Consumidor Final desde Punto de venta tipo Fiscal."));
                    //    return;
                    //}
                    int fiscalTope = this.validarTotalFiscal(fact);
                    if (fiscalTope < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se puede facturar un monto de $" + fact.ptoV.tope + " o mayor desde un Punto de venta tipo Fiscal. \", {type: \"error\"});", true);                        
                        return;
                    }

                    int user = (int)Session["Login_IdUser"];

                    //obtengo el Neto no gravado || de los items con alicuota 0%
                    fact.iva21 = this.obtenerNetoNoGravado(fact);

                    #region facturacion porcentual

                    int porcenOK = this.validarFacturacionPorcentual();
                    if (porcenOK == 1)
                    {
                        this.procesoFacturarPorcentual(fact, dtPago, user);
                    }
                    else
                    {
                        //facturo
                        int i = this.controlador.ProcesarFactura(null,fact, dtPago, user,generaRemito);
                        if (i > 0)
                        {
                            //factura exitosa
                            Session.Remove("Factura");
                            string imprimir = WebConfigurationManager.AppSettings.Get("Imprime");
                            if (imprimir == "1")
                            {
                                this.ImprimirFactura(fact.id, fact.tipo.id, fact.idRemito);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturas.aspx';", true);
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
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo generar factura. \", {type: \"error\"});", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar factura "));
                        }
                    }
                    #endregion

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Debe agregar articulos a la factura. " + this.txtCodigo.Text +" \", {type: \"error\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la factura " + this.txtCodigo.Text));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Error guardando facturas."+ ex.Message +" \", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando facturas. " + ex.Message));
            }
        }

        private void procesoFacturarPorcentual(Factura fact, DataTable dtPago, int user)
        {
            try
            {
                controladorFactEntity contFcEnt = new controladorFactEntity();

                int idForma = Convert.ToInt32(this.ListFormaVenta.SelectedValue);
                //solo hago en cuenta corriente la fact partida
                fact.formaPAgo = this.controlador.obtenerFormaPagoFP("Cuenta Corriente");
                int i = contFcEnt.procesarFacturacionPorcentual(null,fact, dtPago, user, idForma);
                if (i > 0)
                {
                    //factura exitosa
                    Session.Remove("Factura");
                    string imprimir = WebConfigurationManager.AppSettings.Get("Imprime");
                    if (imprimir == "1")
                    {
                        this.ImprimirFactura(fact.id, fact.tipo.id, 0);
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

        private int validarTotalConsumidorFinal(Factura f)
        {
            try
            {
                int i = 0;
                if (f.ptoV.formaFacturar == "Fiscal" && (f.tipo.id == 1 || f.tipo.id == 2) )
                {
                    decimal total = f.total;
                    String cuit = f.cliente.cuit;

                    if( (cuit == "00000000000" || cuit == "00-00000000-0") && (total > 1000) &&(f.formaPAgo.id==1) )
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

        private decimal obtenerNetoNoGravado(Factura fact)
        {
            try
            {
                decimal noGravado = 0;

                foreach (ItemFactura item in fact.items)
                {
                    if (item.porcentajeIva == 0 && (fact.tipo.id != 17 && fact.tipo.id != 11 && fact.tipo.id != 12))// porc 0% y no es PRP
                    {
                        noGravado += (item.precioUnitario * item.cantidad) - (item.cantidad * item.descuento);
                    }
                }

                decimal descNoGravado = Decimal.Round((noGravado * (fact.neto10 / 100)), 2);
                noGravado = noGravado - descNoGravado;

                return noGravado;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Hubo un problema calculando neto no gravado." + ex.Message + " \", {type: \"error\"});", true);
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
            catch(Exception ex)
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

        #region items factura

        private void QuitarItem(object sender, EventArgs e)
        {
            try
            {
                //string idCodigo = (sender as LinkButton).ID.ToString().Substring(11, (sender as LinkButton).ID.Length - 11);
                string idCodigo = (sender as LinkButton).ID.ToString();

                string[] datos = idCodigo.Split('_');
                
                idCodigo = datos[1];

                string pos = datos[2];
                
                //obtengo el pedido del session
                Factura ct = Session["Factura"] as Factura;
                foreach (ItemFactura item in ct.items)
                {
                    if ((item.articulo.codigo == idCodigo) && Convert.ToInt32(pos) == ct.items.IndexOf(item))
                    {
                        //lo quito
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

        private List<ItemFactura> obtenerItems()
        {
            List<ItemFactura> items = new List<ItemFactura>();

            foreach(Control cr in this.phArticulos.Controls)
            {
                //item
                ItemFactura item = new ItemFactura();
                TableRow tr = cr as TableRow;
                item.articulo = this.contArticulo.obtenerArticuloCodigo(tr.Cells[0].ToString());
                item.cantidad = Convert.ToDecimal(tr.Cells[1]);
                item.descuento = 0;
                item.precioUnitario = Convert.ToDecimal(tr.Cells[3]);
                item.total = Convert.ToDecimal(tr.Cells[4]);

                items.Add(item);
               
            }

            return items;


        }
        private int validarTrazaCargadaItemFactura(ItemFactura item)
        {
            try
            {
                int esTrazable = this.contArticulo.verificarGrupoTrazableByID(item.articulo.grupo.id);
                if (esTrazable == 1)
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
        
        #endregion
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.totalItem();
                this.txtDescuento.Focus();
                this.verificarDescuentoCantidad();
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
                string posicion = (sender as TextBox).ID.ToString().Split('_')[1];//.Substring(5, (sender as TextBox).ID.Length - 5);
                Factura ct = Session["Factura"] as Factura;
                ItemFactura item = ct.items[Convert.ToInt32(posicion)];
                item.cantidad = Convert.ToDecimal((sender as TextBox).Text.Replace(',', '.'), CultureInfo.InvariantCulture);

                item.porcentajeDescuento = 0;

                //verifico si tengo que hacer un descuento por cantidad
                decimal descCantidad = this.obtenerNuevoDescuentoCantidad(item.articulo.codigo, item.cantidad);
                if (descCantidad > 0)//si es descuento por cantidad, piso el del item, sino lo dejo
                {
                    item.porcentajeDescuento = descCantidad;
                }

                item.descuento = (item.precioUnitario * (item.porcentajeDescuento / 100)) * item.cantidad;
                item.total = ((item.precioUnitario*(1-(item.porcentajeDescuento/100))) * item.cantidad);
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
                        //this.lbtnAgregarArticuloASP.Focus();
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
                    //this.lbtnAgregarArticuloASP.Focus();
                }
                this.txtPUnitario.Focus();
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total con descuento. Verifique que ingreso numeros en Descuento" + ex.Message));
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

                    decimal total = decimal.Round((cantidad * precio),2);
                    
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

                //Configuracion c = new Configuracion();
                //decimal dto = Convert.ToDecimal(txtPorcDescuento.Text);
                //decimal dtoMax = Convert.ToDecimal(c.maxDtoFactura);
                //this.factura = Session["Factura"] as Factura;
                
                //if (dtoMax > 0)
                //{
                //    if (dto <= dtoMax)
                //    {

                //        this.actualizarTotales();
                //    }
                //    else
                //    {
                //        this.txtPorcDescuento.Text = "0";
                //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No puede ingresar un descuento mayor al " + dtoMax + "%."));
                //        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No puede ingresar un descuento mayor al " + dtoMax + "%. \"); ", true);
                //        this.actualizarTotales();
                //    }
                //}
                //else
                //{
                //    this.actualizarTotales();
                //}

                //this.lblMontoOriginal.Text = this.factura.total.ToString();
                //Factura f = this.factura;
                //Session.Add("Factura", f);
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
                this.lblAcordeonSuc.InnerText = this.ListSucursal.SelectedItem.Text + " - " + this.ListPuntoVenta.SelectedItem.Text;
            }
            catch(Exception ex)
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
            if(!String.IsNullOrEmpty(txtBuscarCliente.Text))
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
                }
                else 
                {
                    this.btnTarjeta.Visible = false;
                }
                //me guardo el id de la lista seleccionada para mantenerlo al recargar la lista
                int listaAnt = Convert.ToInt32(this.DropListLista.SelectedValue);
                this.cargarListaPrecio();                
                this.DropListLista.SelectedValue = listaAnt.ToString();
                this.lblAcordeonFPLista.InnerText = this.DropListFormaPago.SelectedItem.Text + " - " + this.DropListLista.SelectedItem.Text;
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error seleccionando tipo de Pago. " + ex.Message));
            }
        }
        protected void DropListLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblAcordeonFPLista.InnerText = this.DropListFormaPago.SelectedItem.Text + " - " + this.DropListLista.SelectedItem.Text;
            }
            catch
            {

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
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('../Articulos/StockF.aspx?a=2&fd="+DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy")+"&fh="+DateTime.Today.AddDays(1).ToString("dd/MM/yyyy")+"&articulo="+art.id+"&s="+this.ListSucursal.SelectedValue+"');", true);
            }
            catch
            {

            }
        }

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
        protected void ListTarjetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region old
                ////genero la clase
                //Tarjeta t = ct.obtenerTarjetaID(Convert.ToInt32(this.ListTarjetas.SelectedValue));
                ////obtengo parametro si usa recargos o no
                //string recargo = WebConfigurationManager.AppSettings.Get("Recargo");
                //if (recargo == "1")
                //{
                //    decimal totalRecargo = Convert.ToDecimal(this.txtTotal.Text);
                //    totalRecargo = Decimal.Round(totalRecargo * (1 + (t.recargo / 100)), 2);
                //    decimal cuotasFinal = Decimal.Round((totalRecargo / t.cuotas), 2);
                //    this.lblMontoCuotas.Text = "Pago en " + t.cuotas + " cuotas de $" + cuotasFinal + " final. ";
                //}
                //else
                //{
                //    decimal total = Convert.ToDecimal(this.txtTotal.Text);
                //    decimal cuotasFinal = Decimal.Round((total / t.cuotas), 2);
                //    this.lblMontoCuotas.Text = "Pago en " + t.cuotas + " cuotas de $" + cuotasFinal + " final. ";
                //    if (t.recargo > 0)
                //    {
                //        this.lblMontoCuotas.Text += "Con recargo del " + t.recargo + "%";
                //    }
                //}

                ////this.txtImporteT.Text = totalRecargo.ToString();
                ////this.txtImporteEfectivo.Text = totalRecargo.ToString();
                //DataTable dt = lstPago;
                //decimal resta = 0;
                //foreach (DataRow row in dt.Rows)
                //{
                //    resta += Convert.ToDecimal(row["Neto"]);
                //}

                //this.txtImporteT.Text = (Convert.ToDecimal(this.txtTotal.Text) - resta).ToString();
                #endregion
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la factura "));
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
                            #region old
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
                            #endregion
                            int i = this.QuitarRecargoTarjeta(dt);
                            if (i > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                            }
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
                this.lblSaldoTarjeta.Text = this.lblMontoOriginal.Text;

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
                #region old
                //decimal montoFinalRecargo = 0;
                //decimal montoOriginal = Convert.ToDecimal(this.lblMontoOriginal.Text);

                //foreach (DataRow row in dt.Rows)
                //{
                //    montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //}
                //decimal recargo = Decimal.Round((((montoFinalRecargo * 100) / montoOriginal) / 100), 3);

                //Factura f = Session["Factura"] as Factura;

                //foreach (ItemFactura item in f.items)
                //{
                //    item.precioSinIva = Decimal.Round(item.precioSinIva * recargo, 2);//(1 + (t.recargo / 100));
                //    item.precioUnitario = Decimal.Round(item.precioUnitario * recargo, 2);// (1 + (t.recargo / 100));
                //    item.total = Decimal.Round((item.precioUnitario * item.cantidad) * (1 - (item.porcentajeDescuento / 100)), 2);
                //    item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2);
                //}

                //Session.Add("Factura", f);

                ////lo dibujo en pantalla
                //this.cargarItems();
                ////actualizo totales
                //this.actualizarTotales();
                //this.txtImporteEfectivo.Text = decimal.Round(f.total, 2).ToString();
                //return 1;
                #endregion
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
            catch(Exception ex)
            {
                return -1;
            }
        }
        private int QuitarRecargoTarjeta(DataTable dt)
        {
            try
            {
                #region old
                //decimal montoFinalRecargo = 0;
                //decimal montoOriginal = 0;

                //foreach (DataRow row in dt.Rows)
                //{
                //    montoFinalRecargo += Convert.ToDecimal(row["Importe"]);
                //    montoOriginal += Convert.ToDecimal(row["neto"]);
                //}
                //decimal recargo = Decimal.Round((((montoFinalRecargo * 100) / montoOriginal) / 100), 2);

                //Factura f = Session["Factura"] as Factura;

                //foreach (ItemFactura item in f.items)
                //{
                //    item.precioSinIva = Decimal.Round((item.precioSinIva / recargo), 2);
                //    item.precioUnitario = Decimal.Round((item.precioUnitario / recargo), 2);
                //    item.total = Decimal.Round(((item.precioUnitario * item.cantidad) / (1 - (item.porcentajeDescuento / 100))), 2);
                //    item.descuento = Decimal.Round(((item.precioUnitario * item.cantidad) - item.total), 2);
                //}

                //Session.Add("Factura", f);

                ////lo dibujo en pantalla
                //this.cargarItems();
                ////actualizo totales
                //this.actualizarTotales();
                //this.txtImporteEfectivo.Text = decimal.Round(f.total, 2).ToString();

                //return 1;
                #endregion

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
                //int i = this.SumarRecargoTarjeta(dt);
                this.txtImporteEfectivo.Text = "0.00";
                if (t.recargo > 0)
                {
                    int i = this.SumarRecargoTarjeta(dt);
                    if (i > 0)
                    {
                        this.lbtnCancelarPago.Visible = true;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
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
                if (dt.Rows.Count > 0)
                {
                    int i = this.QuitarRecargoTarjeta(dt);
                    if (i > 0)
                    {
                        dt.Clear();
                        lstPago = dt;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Recargos aplicados a factura. ", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo aplicar recargo/s a factura!."));
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
                    this.txtImporteEfectivo.Text = "0";
                    this.lbtnAgregarEfectivo.Visible = false;

                    this.lblAvisoPromocion.Text = "Tarjeta en Promocion " + p.Descuento.Value.ToString() + "% de dto.";
                    this.lblAvisoPromocion.Visible = true;
                }
                else
                {
                    this.txtImporteEfectivo.Attributes.Remove("disabled");
                    //this.txtPorcDescuento.Text = "0";
                    this.actualizarTotales();
                    this.txtImporteT.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
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




        #endregion

        #region Funciones cambio documento
        protected void ListDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string doc = this.ListDocumentos.SelectedValue;
                switch (doc)
                {
                    case "FC":
                        this.obtenerFC();
                        break;
                    case "NC":
                        this.obtenerNC();
                        break;
                    case "ND":
                        this.obtenerND();
                        break;
                    case "PRP":
                        this.obtenerPRP();
                        break;
                    case "NC PRP":
                        this.obtenerNCPRP();
                        break;
                    case "ND PRP":
                        this.obtenerNDPRP();
                        break;
                    default:
                        break;
                }
                this.lblAcordeonNumero.InnerText = this.labelNroFactura.Text;

            }
            catch
            {

            }
        }
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
        private void obtenerFC()
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
        private void obtenerPRP()
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
        private void obtenerNC()
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
        private void obtenerNCPRP()
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

        private void obtenerNDPRP()
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
        private void obtenerND()
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre Realizado. ", "ABMFacturas.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo realizar cierre. "));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error HAciendo Cierre Z. " + ex.Message));
            }
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("ABMFacturas.aspx");
        }

        #endregion

        #region impresion

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

                script += " $.msgbox(\"Factura agregada. \", {type: \"info\"}); location.href = 'ABMFacturas.aspx';";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir factura. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
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
        private void CargarTrazasArticulo(int idArticulo,int cantCampos)
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
                    if(item.lstTrazas.Count > 0 && item.lstTrazas != null)
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
            catch(Exception ex)
            {
                return null;
            }
        }
        
        #endregion

        #region datos despacho

        private ItemFactura agregarInfoDespachoItem(ItemFactura item)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(item.articulo.id);
                if (art.Articulos_Despachos.Count > 0)
                {
                    var datos = art.Articulos_Despachos.FirstOrDefault();
                    item.articulo.descripcion += '\n' + "Fecha despacho: " + datos.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    item.articulo.descripcion += '\n' + "D.I.: " + datos.NumeroDespacho;
                }

                return item;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        

        















    }
}