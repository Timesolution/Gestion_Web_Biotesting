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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.OleDb;
using Gestion_Api.Auxiliares;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMPedidos : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        ControladorPedido controlador = new ControladorPedido();
        ControladorPedidoEntity ControladorPedidoEntity = new ControladorPedidoEntity();
        controladorUsuario contUser = new controladorUsuario();
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        controladorZona contZona = new controladorZona();
        public PlaceHolder phArticulos = new PlaceHolder();
        controladorSucursal cs = new controladorSucursal();
        controladorCotizaciones ct = new controladorCotizaciones();
        controladorFacturacion cf = new controladorFacturacion();
        Configuracion confEstados = new Configuracion();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        controladorCotizaciones contCot = new controladorCotizaciones();
        //Pedido
        Pedido Pedido = new Pedido();
        Cliente cliente = new Cliente();
        TipoDocumento tp = new TipoDocumento();

        int flag_clienteModal = 0;

        int accion;
        int idEmpresa;
        int idSucursal;
        int idPtoVentaUser;
        int idVendedor;
        int idPedido;
        int cotizacion;
        int idCliente;

        string idCotizacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.idPedido = Convert.ToInt32(Request.QueryString["id"]);
                this.cotizacion = Convert.ToInt32(Request.QueryString["c"]);
                this.idCotizacion = Request.QueryString["cot"];
                this.idCliente = Convert.ToInt32(Request.QueryString["cliente"]);


                if (Session["Pedido"] != null)
                {
                    this.cargarItems();
                }

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.btnAgregar.Visible = true;
                    this.btnNuevo.Visible = false;

                    idEmpresa = (int)Session["Login_EmpUser"];
                    idSucursal = (int)Session["Login_SucUser"];
                    idPtoVentaUser = (int)Session["Login_PtoUser"];
                    Session["PedidosABM_ArticuloModalMultiple"] = null;
                    Session["PedidosABM_ArticuloModal"] = null;
                    //ControladorPedidoEntity.LimpiarUsuarioCliente(Convert.ToInt32(Session["Login_IdUser"]));
                    //_idCliente = 0;
                    phArticulos.Controls.Clear();
                    this.verificarModoBlanco();

                    Pedido Pedido = new Pedido();
                    Session.Add("Pedido", Pedido);
                    List<Articulos_PedirOC> artPedir = new List<Articulos_PedirOC>();
                    Session.Add("ArtPedir", artPedir);

                    this.cargarEmpleados();
                    this.cargarTipoPedido();
                    this.cargarVendedor();
                    this.cargarFormaPAgo();
                    this.cargarListaPrecio();
                    this.cargarClientes();
                    this.cargarFormasVenta();
                    this.cargarEmpresas();
                    this.cargarZonas();
                    this.ListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
                    this.cargarEntregas();
                    this.ListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
                    this.ListPuntoVenta.SelectedIndex = 1;
                    this.obtenerNroPedido();

                    //si el usuario tiene pto vta selecciono la del user
                    this.ListPuntoVenta.SelectedValue = this.idPtoVentaUser.ToString();
                    // edicion pedido
                    if (this.accion == 2)
                    {
                        cargarPedidoEditar(this.idPedido);
                    }
                    else
                    {
                        this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    // se genera pedido desde la cotizacion
                    if (this.accion == 4)
                    {
                        
                        //cargarCliente(Convert.ToInt32(cliente));
                        GenerarPedidoCotizacion();
                    }
                    //Me fijo si hay que cargar un cliente por defecto
                    this.verificarClienteDefecto();

                    if (cliente != null && cotizacion == 1 && accion == 0)
                    {
                        if (idCliente != 0)
                        {
                            this.cargarCliente(idCliente);
                            //GenerarPedidoCotizacion();
                        }
                    }

                    //HABILITO O NO EL BOTON btnImportarXML si es el cliente LEVAL SA

                    cambiarHabilitacionBotonbtnImportarXML();
                }

                

                //si viene de la pantalla de articulos, modal
                if (Session["PedidosABM_ArticuloModal"] != null)
                {
                    string CodArt = Session["PedidosABM_ArticuloModal"] as string;
                    txtCodigo.Text = CodArt;
                    cargarProducto(txtCodigo.Text);
                    actualizarTotales();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.foco(this.txtCantidad.ClientID));
                    Session["PedidosABM_ArticuloModalMultiple"] = null;
                    Session["PedidosABM_ArticuloModal"] = null;
                }

                if (Session["PedidosABM_ArticuloModalMultiple"] != null)
                {
                    List<string> CodigosArticulos = Session["PedidosABM_ArticuloModalMultiple"] as List<string>;
                    Configuracion config = new Configuracion();
                    foreach (var codigoArticulo in CodigosArticulos)
                    {
                        Session["PedidosABM_ArticuloModalMultiple"] = codigoArticulo;
                        txtCodigo.Text = codigoArticulo;
                        txtCantidad.Text = "1";
                        cargarProducto(txtCodigo.Text);
                        if (config.commitante != "1")
                            cargarProductoAPedido();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.foco(this.txtCantidad.ClientID));
                    }
                    txtCodigo.Text = "";
                    actualizarTotales();
                    Session["PedidosABM_ArticuloModalMultiple"] = null;
                    Session["PedidosABM_ArticuloModal"] = null;
                }

                //Si es perfil vendedor bloqueo los droplist, dejo que solo pueda elegir el cliente
                this.verificarVendedor();
                if (txtFecha.Text == "")
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy"); //Juan

                //si viene de la pantalla de busqueda cliente
                if (Session["PedidosABM_ClienteModal"] != null)
                {
                    //seleccion cliente desde modal
                    this.flag_clienteModal = 1;
                    this.cargarClienteDesdeModal();
                }

                //si viene de la pantalla de busqueda de articulos
                if (Session["PedidosABM_ArticuloModal"] != null)
                {
                    //obtengo codigo
                    string CodArt = Session["PedidosABM_ArticuloModal"] as string;
                    this.txtCodigo.Text = CodArt;
                    this.cargarProducto(this.txtCodigo.Text);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.foco(this.txtCantidad.ClientID));
                }

                //Dejo editable el campo de descripcion del articulo o no
                this.verficarConfiguracionEditar();

            }
            catch (Exception ex)
            {

            }
        }


        private void CargarDropList_DireccionesDeEntregaDelCliente(int idCliente)
        {
            try
            {
                var direcciones = contCliente.obtenerDireccionesById(idCliente);
                int contadorTipoEntrega = 0;
                int contadorTipoLegal = 0;
                dropList_DomicilioEntrega.Items.Clear();
                dropList_DomicilioEntrega.Items.Add(new ListItem("Seleccione...", "-1"));
                foreach (DataRow item in direcciones.Rows)
                {
                    if (item.ItemArray[0].ToString() == "Entrega")
                    {
                        contadorTipoEntrega++;
                        dropList_DomicilioEntrega.Items.Add(new ListItem(item.ItemArray[1] + ", " + item.ItemArray[2] + ", " + item.ItemArray[3], item.ItemArray[3].ToString()));
                    }
                    else if (item.ItemArray[0].ToString() == "Legal")
                    { contadorTipoLegal++; }
                }
                if (dropList_DomicilioEntrega.Items.Count == 2 && contadorTipoEntrega > 0)
                {
                    dropList_DomicilioEntrega.SelectedIndex = 1;
                }
                else
                {
                    if (dropList_DomicilioEntrega.Items.Count > 0 && contadorTipoLegal > 0)
                    {
                        foreach (DataRow item in direcciones.Rows)
                        {
                            if (item.ItemArray[0].ToString() == "Legal")
                            {
                                dropList_DomicilioEntrega.Items.Add(new ListItem(item.ItemArray[1] + ", " + item.ItemArray[2] + ", " + item.ItemArray[3], item.ItemArray[3].ToString()));
                            }
                        }
                        if (dropList_DomicilioEntrega.Items.Count == 2)
                        {
                            dropList_DomicilioEntrega.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                var perIngresaPantalla = listPermisos.Where(x => x == "37").FirstOrDefault();

                if (perIngresaPantalla == "37")
                {
                    if (!listPermisos.Contains("151"))
                        this.DropListLista.Attributes.Add("disabled", "disabled");

                    return 1;
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        #region Cargas Iniciales

        public void cargarPedidoEditar(int idpedido)
        {
            try
            {
                this.Pedido = Session["Pedido"] as Pedido;

                Pedido p = controlador.obtenerPedidoId(idpedido);

                Session.Add("Pedido", p);
                this.ListEmpresa.SelectedValue = p.empresa.id.ToString();
                this.cargarSucursal(p.empresa.id);
                this.cargarCliente(p.cliente.id);
                this.DropListClientes.SelectedValue = p.cliente.id.ToString();
                this.DropListVendedor.SelectedValue = p.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = p.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = p.listaP.id.ToString();
                this.ListSucursal.SelectedValue = p.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = p.ptoV.id.ToString();
                this.txtFecha.Text = p.fecha.ToString("dd/MM/yyyy");
                this.ListTipoEntrega.SelectedValue = p.tipoEntrega.ToString();
                this.txtHorarioEntrega.Text = p.horaEntrega;
                this.DropListZonaEntrega.SelectedValue = p.zonaEntrega.ToString();
                this.txtComentarios.Text = p.comentario;
                this.txtDescuento.Text = p.descuento.ToString();
                this.cargarItems();
                this.actualizarTotales();
                if(p.tipo.tipo == "Cotizacion") //cotizacion
                {
                    this.labelNroPedido.Text = "Cotizacion N° " + p.numero;
                }
                else if(p.tipo.tipo == "Pedido")
                {
                    this.labelNroPedido.Text = "Pedido N° " + p.numero;
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pedido. " + ex.Message));
            }
        }
        /// <summary>
        /// Genera Punto de Venta
        /// </summary>
        public void GenerarPuntoVenta()
        {
            PuntoVenta pv = new PuntoVenta();
            pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
            //txtPtoVenta.Text = pv.puntoVenta;
        }

        /// <summary>
        /// Genera el pedido con los datos recibidos de la cotizacion
        /// </summary>
        public void GenerarPedidoCotizacion()
        {
            try
            {
                string numerosCotizaciones = "";
                Pedido p = CargarPedidoDesdeCotizacion(ref numerosCotizaciones);
                Session.Add("Pedido", p);
                this.ListEmpresa.SelectedValue = idEmpresa.ToString();
                this.cargarSucursal(idEmpresa);
                this.cargarPuntoVta(idSucursal);
                this.cargarCliente(p.cliente.id);
                this.DropListClientes.SelectedValue = p.cliente.id.ToString();
                this.DropListFormaPago.SelectedValue = p.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = p.listaP.id.ToString();
                this.ListSucursal.SelectedValue = idSucursal.ToString();
                this.ListPuntoVenta.SelectedValue = idPtoVentaUser.ToString();
                this.CheckBox1.Checked = true;
                this.phDatosEntrega.Visible = true;
                this.txtComentarios.Text = "COTIZACIONES Nº: " + numerosCotizaciones;
                this.txtPorcDescuento.Text = p.neto10.ToString();
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroPedido();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos cotizacion a pedido " + ex.Message));
            }
        }

        public Pedido CargarPedidoDesdeCotizacion(ref string numerosCotizaciones)
        {
            try
            {
                Pedido cotizacion = new Pedido();
                bool descuentosDiferentes = false;
                Cliente cliente = contCliente.obtenerClienteID(idCliente);
                var idsCotizaciones = idCotizacion.Split(';').ToList();
                idsCotizaciones.Remove(idsCotizaciones.Last());
                decimal descuentoTemp = controlador.obtenerPedidoId(Convert.ToInt32(idsCotizaciones[0])).neto10;

                Pedido p = new Pedido();

                p.empresa.id = idEmpresa;
                p.sucursal.id = idSucursal;
                p.listaP = cliente.lisPrecio;
                p.ptoV.id = idPtoVentaUser;
                p.fecha = DateTime.Now;
                p.cliente = cliente;
                p.tipo.id = 13;//id tipo documento pedido
                p.formaPAgo.id = cliente.formaPago.id;

                for (int i = 0; i < idsCotizaciones.Count; i++)
                {
                    if (string.IsNullOrEmpty(idsCotizaciones[i]))
                        continue;

                    cotizacion = controlador.obtenerPedidoId(Convert.ToInt32(idsCotizaciones[i]));

                    if (i != idsCotizaciones.Count - 1)
                        numerosCotizaciones += cotizacion.numero + ", ";
                    else
                        numerosCotizaciones += cotizacion.numero;

                    foreach (ItemPedido item in cotizacion.items)
                    {
                        ItemPedido itemPedido = new ItemPedido();
                        itemPedido.articulo = item.articulo;
                        itemPedido.descripcion = item.descripcion;
                        itemPedido.cantidad = item.cantidad;
                        itemPedido.descuento = item.descuento;
                        itemPedido.precioUnitario = item.precioUnitario;
                        itemPedido.total = item.total;
                        p.items.Add(itemPedido);
                    }

                    if (descuentoTemp != cotizacion.neto10)
                        descuentosDiferentes = true;
                }

                if (!descuentosDiferentes)
                    p.neto10 = descuentoTemp;
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Las cotizaciones tienen descuentos distintos! El descuento para el pedido sera 0"));
                }

                //p.cotizacion.id = cotizacion.id;
                //p.neto = cotizacion.neto;
                //p.netoNGrabado = cotizacion.netoNGrabado;
                //p.neto10 = cotizacion.neto10;
                //p.neto21 = cotizacion.neto21;
                //p.iva10 = cotizacion.iva10;
                //p.iva21 = cotizacion.iva21;
                //p.descuento = cotizacion.descuento;
                //p.subTotal = cotizacion.subTotal;
                //p.retencion = cotizacion.retencion;
                //p.total = cotizacion.total;

                return p;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error asignando datos de cotizacion a pedido " + ex.Message);
                return null;
            }
        }

        public void cargarTipoPedido()
        {
            try
            {
                //DataTable dt = controlador.obtenerTipoPedido();

                ////agrego todos
                //DataRow dr = dt.NewRow();
                //dr["tipo"] = "Seleccione...";
                //dr["id"] = -1;
                //dt.Rows.InsertAt(dr, 0);

                //this.DropListTipoDoc.DataSource = dt;
                //this.DropListTipoDoc.DataValueField = "id";
                //this.DropListTipoDoc.DataTextField = "tipo";

                //this.DropListTipoDoc.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos Pedido. " + ex.Message));
            }
        }
        public void cargarVendedor()
        {
            try
            {
                //controladorVendedor contVendedor = new controladorVendedor();
                //DataTable dt = contVendedor.obtenerVendedores();

                ////agrego todos
                //DataRow dr2 = dt.NewRow();
                //dr2["nombre"] = "Seleccione...";
                //dr2["id"] = -1;
                //dt.Rows.InsertAt(dr2, 0);

                //foreach (DataRow dr in dt.Rows)
                //{
                //    ListItem item = new ListItem();
                //    item.Value = dr["id"].ToString();
                //    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                //    DropListVendedor.Items.Add(item);
                //}
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
                this.ListPuntoVenta.Items.Clear();

                if (ListPuntoVenta.SelectedIndex > -1)
                {
                    this.ListPuntoVenta.SelectedIndex = 0;
                }

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
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

        public void cargarListaPrecio()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerListaPrecios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListLista.DataSource = dt;
                this.DropListLista.DataValueField = "id";
                this.DropListLista.DataTextField = "nombre";

                this.DropListLista.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }

        private void cargarEntregas()
        {
            try
            {
                var conExpreso = new ControladorExpreso();
                List<TiposEntrega> listaEntregas = conExpreso.obtenerTiposEntrega();
                listaEntregas.Insert(0, (new TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));

                this.ListTipoEntrega.DataSource = listaEntregas;
                this.ListTipoEntrega.DataValueField = "Id";
                this.ListTipoEntrega.DataTextField = "Descripcion";

                this.ListTipoEntrega.DataBind();
                //cargo fecha ho
                this.txtFechaEntrega.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista entregas. " + ex.Message));
            }
        }

        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //this.cargarCliente(this.txtBusquedaCliente.Text);
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
                string perfil = Session["Login_NombrePerfil"] as string;
                controladorCliente contCliente = new controladorCliente();
                DataTable dt = new DataTable();

                bool auxiliar = false;

                if (perfil == "Vendedor")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);
                }
                if (perfil == "Cliente")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                }

                if (perfil == "Distribuidor" || perfil == "Experta" || perfil == "Lider")
                {
                    auxiliar = true;
                    dt = this.contClienteEntity.obtenerReferidosDeCliente(this.idVendedor);
                    DataRow dr2 = dt.NewRow();
                    Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                    if (c != null)
                    {
                        dr2["alias"] = c.razonSocial;
                    }

                    dr2["id"] = this.idVendedor;
                    dt.Rows.InsertAt(dr2, 0);
                }
                if (auxiliar == false)
                {
                    dt = contCliente.obtenerClientesDT();
                }

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

                if (idCliente > 0)
                {
                    CargarPacienteEspecifico(Convert.ToInt32(idCliente));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        private void CargarPacienteEspecifico(int idCliente)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                var cliente = controladorClienteEntity.ObtenerClienteId(idCliente);

                if (cliente == null)
                    return;

                if (DropListClientes.SelectedValue == cliente.id.ToString())
                    return;

                ListItem item = new ListItem(cliente.razonSocial, cliente.id.ToString());
                this.DropListClientes.Items.Add(item);
                DropListClientes.SelectedValue = cliente.id.ToString();
            }
            catch
            {

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
        public void verificarClienteDefecto()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;
                idSucursal = (int)Session["Login_SucUser"];

                if (accion == 4)
                    return;

                if (perfil == "Cliente")
                {
                    return;
                }

                if (IsPostBack)//Si cambio la sucursal en el list manualmente uso ese valor en lugar del de usuario.
                {
                    idSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                }
                Sucursal s = this.cs.obtenerSucursalID(idSucursal);
                string idCliente = s.clienteDefecto.ToString();

                if (idCliente != "-1" && idCliente != "-2" && idCliente != null)
                {
                    if (this.DropListClientes.Items.FindByValue(idCliente) == null)
                    {
                        this.cargarClienteEnLista(Convert.ToInt32(idCliente));
                    }
                    this.DropListClientes.SelectedValue = idCliente;
                    this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                    this.obtenerNroPedido();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando cliente por defecto. " + ex.Message));
            }
        }
        public void verficarConfiguracionEditar()
        {
            try
            {
                Configuracion c = new Configuracion();
                if (c.editarArticulo == "1")
                {
                    this.txtDescripcion.Attributes.Remove("disabled");
                    this.txtPUnitario.Attributes.Remove("disabled");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error verificando configuracion editar descripcion.  " + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        public void verificarVendedor()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;
                Cliente cl = this.contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));

                try
                {
                    this.DropListLista.SelectedValue = cl.lisPrecio.id.ToString();
                    this.DropListFormaPago.SelectedValue = cl.formaPago.id.ToString();
                }
                catch
                {

                }
                if (perfil == "Vendedor" || perfil == "Cliente")
                {

                    this.DropListVendedor.Attributes.Add("disabled", "true");
                    this.DropListLista.Attributes.Add("disabled", "true");
                    this.DropListFormaPago.Attributes.Add("disabled", "true");
                    this.ListEmpresa.Attributes.Add("disabled", "true");
                    this.ListSucursal.Attributes.Add("disabled", "true");
                    this.ListPuntoVenta.Attributes.Add("disabled", "true");
                    this.PanelBuscar.Visible = false;

                    this.DropListLista.SelectedValue = cl.lisPrecio.id.ToString();
                    this.DropListFormaPago.SelectedValue = cl.formaPago.id.ToString();

                    this.cargarVendedorPerfilCliente(cl.vendedor.id);
                    this.DropListVendedor.SelectedValue = cl.vendedor.id.ToString();
                }
                if (perfil == "Distribuidor")
                {
                    this.DropListVendedor.Attributes.Add("enabled", "true");
                    this.DropListFormaPago.Attributes.Add("enabled", "true");
                    this.DropListLista.Attributes.Add("enabled", "true");
                    this.PanelBuscar.Visible = false;
                }
            }
            catch
            {

            }
        }
        private void verificarDescuentoCantidad()
        {
            try
            {
                //Verifica en ClienteDatos si al cliente se le aplica descuento por cantidad
                Pedido p = Session["Pedido"] as Pedido;
                var clienteDatos = this.contClienteEntity.obtenerClienteDatosByCliente(p.cliente.id);

                if (clienteDatos.Count > 0)
                {
                    if (clienteDatos[0].AplicaDescuentoCantidad == 1)
                    {
                        ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                        decimal cant = Convert.ToDecimal(this.txtCantidad.Text);

                        Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCod(this.txtCodigo.Text);
                        if (artEnt != null)
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
                        else
                        {
                            this.TxtDescuentoArri.Text = "0";
                        }
                    }
                }

            }
            catch
            {

            }
        }
        private decimal obtenerNuevoDescuentoCantidad(string codigo, decimal cantNueva)
        {
            try
            {
                //Verifica en ClienteDatos si al cliente se le aplica descuento por cantidad
                Pedido p = Session["Pedido"] as Pedido;
                var clienteDatos = this.contClienteEntity.obtenerClienteDatosByCliente(p.cliente.id);
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
        private void verificarAlerta()
        {

            try
            {
                string script = "";
                string alerta1 = "";
                string alerta2 = "";
                string alerta3 = "";

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
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "Error en verificar alerta. Ex: " + ex.Message, true);
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

                //busco si el articulo ya esta en la Pedido mediante el codigo
                Pedido p = Session["Pedido"] as Pedido;
                var a = p.items.Where(x => x.articulo.codigo == art.codigo).FirstOrDefault();
                if (a != null)
                {
                    //si esta concateno la alerta
                    script += "$.msgbox(\"Este articulo ya fue cargado previamente al Pedido: Cod.:" + art.codigo + " \");";
                    alerta += "Este articulo fue cargado previamente al Pedido: Articulo:" + art.codigo;
                }

                if (script != "")
                {
                    string CodArt = Session["PedidosABM_ArticuloModal"] as string;
                    if (!String.IsNullOrEmpty(CodArt))//si vienen desde modal uso un script sino uso el otro.
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(alerta));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                    }
                }
            }
            catch
            {

            }
        }
        private void cargarVendedorPerfilCliente(int idvendedor)
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();

                Vendedor vend = contVendedor.obtenerVendedorID(idvendedor);
                if (vend != null)
                {
                    this.DropListVendedor.Items.Clear();
                    this.DropListVendedor.Items.Add(new ListItem(vend.emp.nombre + " " + vend.emp.apellido, vend.id.ToString()));
                }
            }
            catch
            {

            }
        }
        private void cargarCliente(int idCliente)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();
                this.cliente = contCliente.obtenerClienteID(idCliente);

                if (this.cliente != null)
                {
                    CargarPacienteEspecifico(Convert.ToInt32(idCliente));

                    this.labelCliente.Text = this.cliente.razonSocial + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                    this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();
                    try
                    {
                        this.DropListVendedor.SelectedValue = this.cliente.vendedor.id.ToString();
                    }
                    catch { }
                    this.DropListFormaPago.SelectedValue = this.cliente.formaPago.id.ToString();

                    //pongo en cero por si eligieron un cliente con desc o percepciones y dps lo cambiaron
                    this.txtPorcDescuento.Text = "0";
                    this.txtPorcRetencion.Text = "0";

                    //cargo el descuento
                    if (this.cliente.descFC > 0)
                    {
                        this.txtPorcDescuento.Text = Decimal.Round(this.cliente.descFC, 2).ToString();
                    }
                    //cargo Ingresos brutos
                    var IIBB = this.contClienteEntity.obtenerIngresosBrutoCliente(idCliente);
                    if (IIBB != null)
                    {
                        this.txtPorcRetencion.Text = IIBB.Percepcion.ToString();
                    }

                    //datos entrega
                    var entrega = contClienteEnt.obtenerEntregaCliente(idCliente);
                    if (entrega != null)
                    {
                        this.ListTipoEntrega.SelectedValue = entrega.TipoEntrega.ToString();
                        this.txtHorarioEntrega.Text = entrega.HorarioEntrega;
                        this.DropListZonaEntrega.SelectedValue = entrega.ZonaEntrega.ToString();
                    }

                    var dire = cliente.direcciones.Where(x => x.nombre == "Entrega").FirstOrDefault();
                    if (dire != null)
                    {
                        //this.txtDomicilioEntrega.Text = dire.direc + " " + dire.codPostal + "" + dire.localidad + " " + dire.provincia + " " + dire.pais;
                        CargarDropList_DireccionesDeEntregaDelCliente(idCliente);
                    }
                    else
                    {
                        var legal = cliente.direcciones.Where(x => x.nombre == "Legal").FirstOrDefault();
                        if (legal != null)
                        {

                            //this.txtDomicilioEntrega.Text = legal.direc + " " + legal.codPostal + " " + legal.localidad + " " + legal.provincia + " " + legal.pais;
                            CargarDropList_DireccionesDeEntregaDelCliente(idCliente);
                        }
                        else
                        {
                            //this.txtDomicilioEntrega.Text = "";
                            CargarDropList_DireccionesDeEntregaDelCliente(idCliente);
                            //this.dropList_DomicilioEntrega.Text = "";
                        }
                    }

                    //cargo el cliente en la Pedido session
                    Pedido c = Session["Pedido"] as Pedido;
                    c.cliente.id = this.cliente.id;
                    Session.Add("Pedido", c);
                    Session["PedidosABM_ClienteModal"] = null;

                    this.verificarAlerta();
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
                int idCliente = (int)Session["PedidosABM_ClienteModal"];
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
            this.DropListClientes.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
            this.DropListClientes.SelectedValue = idCliente.ToString();
        }


        /// <summary>
        /// Obtiene el ultimo numero de Pedido
        /// </summary>
        /// <param name="iva"></param>
        private void obtenerPedido(string iva)
        {
            try
            {
                //int fact;
                TipoDocumento ti = null;
                if (iva == "Responsable Inscripto")
                {
                    //ti = this.controlador.obtenerTipoFact("Factura A");
                    ti = this.controlador.obtenerFacturaNumero(1, 13);

                }
                else
                {
                    //ti = this.controlador.obtenerTipoFact("Factura B");
                    ti = this.controlador.obtenerFacturaNumero(1, 13);

                }
                //this.txtNroPedido.Text = ti.tipo + " " + ti.idNumeracion.ToString().PadLeft(8, '0'); ;

                //cargo el tipo en la Pedido session
                Pedido c = Session["Pedido"] as Pedido;
                c.tipo.id = ti.id;
                Session.Add("Pedido", c);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Pedido. " + ex.Message));
            }
        }

        private void obtenerNroPedido()
        {
            try
            {
                if (this.cotizacion == 1)
                {
                    int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                    PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                    //como estoy en cotizacion pido el ultimo numero de este documento
                    int nro = this.contCot.obtenerCotizacionNumero(ptoVenta, "Cotizacion");
                    this.labelNroPedido.Text = "Cotizacion N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
                else
                {
                    if (this.accion != 2)
                    {
                        int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                        PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        int nro = this.controlador.obtenerPedidoNumero(ptoVenta, "Pedido");
                        this.labelNroPedido.Text = "Pedido N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Cotizacion. " + ex.Message));
            }
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
                //Articulo art = contArticulo.obtenerArticuloCodigo(codigo);
                Articulo art = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                if (art != null)
                {

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

                    this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                    //decimal PrecioSinIva = decimal.Round(art.precioVenta - (art.precioVenta * (art.porcentajeIva / 100)), 2);
                    this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();
                    this.verificarAlertaArticulo(art);

                    //si tiene datos de despacho se los cargo
                    Configuracion config = new Configuracion();
                    if (config.infoImportacionPedidos == "1")
                        this.agregarInfoDespachoItem(art);

                    Session["PedidosABM_ArticuloModal"] = null;
                    this.txtCantidad.Focus();
                    this.totalItem();
                    //this.obtenerDatosReferenciaArticulo();
                    //this.obtenerDescuentosCantidadArticulo();


                    Pedido c = Session["Pedido"] as Pedido;
                    this.txtRenglon.Text = (c.items.Count + 1).ToString();

                    //verifico si el articulo esta pendiete en otro articulo pedido
                    //this.obtenerPendientesCliente(art.id, c.cliente.id);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra Articulo " + this.txtCodigo.Text));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Articulo. " + ex.Message));
            }
        }

        protected void btnAgregarArt_Click(object sender, EventArgs e)
        {
            this.cargarProductoAPedido();
        }

        private void cargarProductoAPedido()
        {
            try
            {
                //recalculo total
                this.totalItem();
                //item
                ItemPedido item = new ItemPedido();

                item.articulo = contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text);
                item.cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                item.total = Convert.ToDecimal(this.txtTotalArri.Text);
                decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                item.porcentajeDescuento = desc;
                item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text);

                item.descripcion = this.txtDescripcion.Text;

                //item.articulo = contArticulo.obtenerArticuloFacturar(this.txtCodigo.Text, Convert.ToInt32(this.DropListLista.SelectedValue));
                //item.cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                //item.total = Convert.ToDecimal(this.txtTotalArri.Text);
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                //item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text);

                if (desc > 0)
                {
                    decimal tot = item.precioUnitario * item.cantidad;
                    decimal totDesc = tot * (desc / 100);
                    item.descuento = decimal.Round(totDesc, 2);
                }
                else
                {
                    item.descuento = 0;
                }

                //si es importado cargo los datos de despacho si tiene alguno cargado
                //this.agregarInfoDespachoItem(item);


                this.Pedido.items.Add(item);
                //lo agrego al session
                if (Session["Pedido"] == null)
                {
                    Pedido cot = new Pedido();
                    Session.Add("Pedido", cot);

                }
                Pedido c = Session["Pedido"] as Pedido;

                if (!String.IsNullOrEmpty(this.txtRenglon.Text))
                    item.nroRenglon = Convert.ToInt32(this.txtRenglon.Text);
                else
                    item.nroRenglon = c.items.Count() + 1;

                c.items.Add(item);
                c.items = c.items.Distinct().ToList(); //AGREGA LOS ITEMS AL PEDIDO Y CHEQUEA QUE NO HAYA ID REPTIDOS DE LOS ITEMS 
                Session.Add("Pedido", c);

                //lo dibujo en pantalla
                this.cargarItems();

                //agrego abajo
                //this.Pedido.items.Add(item);
                //actualizo totales
                this.actualizarTotales();

                //verifico si tengo que agregar el pedido a pedir
                if (this.checkMarcar.Checked)
                {
                    //agrego
                    this.agregarArticuloaPedir(item.articulo.id);
                }

                //borro los campos
                this.borrarCamposagregarItem();
                //this.UpdatePanel1.Update();
                this.txtCodigo.Focus();
                //ClientScript.RegisterClientScriptBlock(this.UpdatePanel1.GetType(), "alert", m.foco(this.txtCodigo.ClientID));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", m.foco(this.txtCodigo.ClientID), true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }

        #region datos despacho
        private void agregarInfoDespachoItem(Articulo articulo)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(articulo.id);
                if (art.Articulos_Despachos.Count > 0)
                {
                    var datos = art.Articulos_Despachos.FirstOrDefault();

                    if (datos.FechaDespacho != null)
                        this.txtDescripcion.Text += '\n' + "Fecha despacho: " + datos.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    if (!String.IsNullOrEmpty(datos.NumeroDespacho))
                        this.txtDescripcion.Text += '\n' + "D.I.: " + datos.NumeroDespacho;
                    if (!String.IsNullOrEmpty(datos.Lote))
                        this.txtDescripcion.Text += '\n' + "Lote: " + datos.Lote;
                    if (!String.IsNullOrEmpty(datos.Vencimiento))
                        this.txtDescripcion.Text += '\n' + "Vencimiento: " + datos.Vencimiento;
                }

                return;
            }
            catch
            {
                return;
            }
        }
        #endregion

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
                this.checkMarcar.Checked = false;
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
                Pedido c = Session["Pedido"] as Pedido;
                //limpio el place holder y lo vuelvo a cargar
                this.phArticulos.Controls.Clear();
                int pos = 0;
                int cont = 1;
                foreach (ItemPedido item in c.items)
                {
                    pos = c.items.IndexOf(item);
                    this.agregarItemPedido(item, pos, cont);
                    cont++;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPopover();", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando items en Pedido. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga el item en la tabla items
        /// </summary>
        private void agregarItemPedido(ItemPedido item, int pos, int numero)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = item.articulo.codigo.ToString() + pos;

                //Celdas
                TableCell celCodigo = new TableCell();
                if (item.nroRenglon > 0)
                    celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                else
                    celCodigo.Text = (pos + 1) + " - " + item.articulo.codigo;
                //celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                TextBox txtCant = new TextBox();
                txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtCant.ID = "Text_" + pos.ToString() + "_" + item.cantidad;
                txtCant.CssClass = "form-control";
                txtCant.Style.Add("text-align", "Right");
                txtCant.Text = item.cantidad.ToString();
                txtCant.TextChanged += new EventHandler(ActualizarTotalPH);
                txtCant.AutoPostBack = true;
                celCantidad.Controls.Add(txtCant);
                celCantidad.Width = Unit.Percentage(10);
                //if (ListSucursalCliente.SelectedIndex > 0)
                //    txtCant.Enabled = PuedeModificarCantidadDeItemSegunConfiguracion();
                //else
                //    txtCant.Enabled = true;
                tr.Cells.Add(celCantidad);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.descripcion;
                celDescripcion.Width = Unit.Percentage(35);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + item.precioUnitario.ToString();
                celPrecio.Width = Unit.Percentage(8);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = "$" + item.descuento.ToString();
                celDescuento.Width = Unit.Percentage(8);
                celDescuento.VerticalAlign = VerticalAlign.Middle;
                celDescuento.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDescuento);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + item.total.ToString();
                celTotal.Width = Unit.Percentage(8);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);
                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + item.articulo.codigo + "_" + pos;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarItem);

                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(21);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l1 = new Literal();
                l1.Text = "&nbsp";

                celAccion.Controls.Add(l1);

                if (item.stockCompleto != null)
                {
                    AgregarLabelsDeStockToCelda(celAccion, item);
                }

                tr.Cells.Add(celAccion);

                phArticulos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }
        /// <summary>
        /// Carga el item en la tabla items
        /// </summary>
        private void agregarItemPedidoRenglon(ItemPedido item, int pos, int numero)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = item.articulo.codigo.ToString() + pos;

                //Celdas
                TableCell celCodigo = new TableCell();
                if (item.nroRenglon > 0)
                    celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                else
                    celCodigo.Text = (pos + 1) + " - " + item.articulo.codigo;
                //celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                TextBox txtCant = new TextBox();
                txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtCant.ID = "Text_" + pos.ToString() + "_" + item.cantidad;
                txtCant.CssClass = "form-control";
                txtCant.Style.Add("text-align", "Right");
                txtCant.Text = item.cantidad.ToString();
                txtCant.TextChanged += new EventHandler(ActualizarTotalPH);
                txtCant.AutoPostBack = true;
                celCantidad.Controls.Add(txtCant);
                celCantidad.Width = Unit.Percentage(10);
                //if (ListSucursalCliente.SelectedIndex > 0)
                //    txtCant.Enabled = PuedeModificarCantidadDeItemSegunConfiguracion();
                //else
                //    txtCant.Enabled = true;
                tr.Cells.Add(celCantidad);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.descripcion;
                celDescripcion.Width = Unit.Percentage(35);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + item.precioUnitario.ToString();
                celPrecio.Width = Unit.Percentage(8);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = "$" + item.descuento.ToString();
                celDescuento.Width = Unit.Percentage(8);
                celDescuento.VerticalAlign = VerticalAlign.Middle;
                celDescuento.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDescuento);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + item.total.ToString();
                celTotal.Width = Unit.Percentage(8);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);
                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + item.articulo.codigo + "_" + pos;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarItem);

                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(21);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l1 = new Literal();
                l1.Text = "&nbsp";

                celAccion.Controls.Add(l1);

                if (item.stockCompleto != null)
                {
                    AgregarLabelsDeStockToCelda(celAccion, item);
                }

                tr.Cells.Add(celAccion);

                phArticulos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }

        public void AgregarLabelsDeStockToCelda(TableCell celda, ItemPedido item)
        {
            try
            {
                Literal l1 = new Literal();
                l1.Text = "&nbsp";

                Label lbStockFisico = new Label();
                lbStockFisico.CssClass = "badge ui-tooltip";
                lbStockFisico.Attributes.Add("name", "Stock_Tooltip");
                lbStockFisico.Attributes.Add("data-toggle", "tooltip");
                lbStockFisico.Attributes.Add("data-placement", "top");
                lbStockFisico.Attributes.Add("title", "Stock Fisico");
                lbStockFisico.Attributes.Add("data-original-title", "Stock Fisico");
                lbStockFisico.Text = decimal.Round(item.stockCompleto.StockFisico, 0).ToString();

                Label lbStockComprometido = new Label();
                lbStockComprometido.CssClass = "badge ui-tooltip";
                lbStockFisico.Attributes.Add("name", "Stock_Tooltip");
                lbStockComprometido.Attributes.Add("data-toggle", "tooltip");
                lbStockComprometido.Attributes.Add("data-placement", "top");
                lbStockComprometido.Attributes.Add("title", "Stock Comprometido");
                lbStockComprometido.Attributes.Add("data-original-title", "Stock Comprometido");
                lbStockComprometido.Text = decimal.Round(item.stockCompleto.StockComprometido, 0).ToString();

                Label lbStockReal = new Label();
                lbStockReal.CssClass = "badge ui-tooltip";
                lbStockFisico.Attributes.Add("name", "Stock_Tooltip");
                lbStockReal.Attributes.Add("data-toggle", "tooltip");
                lbStockReal.Attributes.Add("data-placement", "top");
                lbStockReal.Attributes.Add("title", "Stock Real");
                lbStockReal.Attributes.Add("data-original-title", "Stock Real");
                lbStockReal.Text = decimal.Round(item.stockCompleto.StockReal, 0).ToString();

                celda.Controls.Add(l1);
                celda.Controls.Add(lbStockFisico);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";

                celda.Controls.Add(l2);
                celda.Controls.Add(lbStockComprometido);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celda.Controls.Add(l3);

                celda.Controls.Add(lbStockReal);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en AgregarLabelsDeStockToCelda. " + ex.Message));
            }
        }

        private void actualizarTotales()
        {
            try
            {
                this.Pedido = Session["Pedido"] as Pedido;

                //obtengo total de suma de item
                decimal totalC = this.Pedido.obtenerTotalNeto();
                decimal total = decimal.Round(totalC, 2);
                this.Pedido.neto = total;

                //Subtotal = neto menos el descuento
                this.Pedido.descuento = this.Pedido.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100);
                this.Pedido.subTotal = this.Pedido.neto - this.Pedido.descuento;

                string[] lbl = this.labelNroPedido.Text.Split('°');
                string perfil = Session["Login_NombrePerfil"] as string;

                if (lbl[0] == "Presupuesto N" || perfil == "Cliente")
                {
                    Configuracion c = new Configuracion();
                    this.Pedido.neto21 = (this.Pedido.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100);
                }
                else
                {
                    decimal iva = decimal.Round(this.Pedido.obtenerTotalIva(), 2);
                    decimal descuento = 0;
                    if (this.txtDescuento.Text != "")
                        descuento = Convert.ToDecimal(this.txtDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    else
                        descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
                    this.Pedido.descuento += decimal.Round(descuento, 2, MidpointRounding.AwayFromZero);
                    this.Pedido.neto21 = iva - decimal.Round(descuento, 2);
                }

                this.Pedido.totalSinDescuento = this.Pedido.neto + this.Pedido.obtenerTotalIva();

                //retencion sobre el sub total
                this.Pedido.retencion = this.Pedido.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100);

                //total: subtotal + iva + retencion 
                this.Pedido.total = this.Pedido.subTotal + this.Pedido.neto21 + this.Pedido.retencion;

                //cargo en pantalla
                string neto = decimal.Round(this.Pedido.neto, 2).ToString();
                this.txtNeto.Text = neto;

                this.txtDescuento.Text = decimal.Round(this.Pedido.descuento, 2).ToString();

                this.txtsubTotal.Text = decimal.Round(this.Pedido.subTotal, 2).ToString();

                this.txtIvaTotal.Text = decimal.Round(this.Pedido.neto21, 2).ToString();

                this.txtRetencion.Text = decimal.Round(this.Pedido.retencion, 2).ToString();

                this.txtTotal.Text = decimal.Round(this.Pedido.total, 2).ToString();

                if (!string.IsNullOrEmpty(this.txtDescuento.Text))
                    txtPorcDescuento.Text = ((Convert.ToDecimal(txtMontoParaAplicarDescuentoAlTotal.Text) / Convert.ToDecimal(txtTotal.Text)) * 100).ToString();

                Pedido p = this.Pedido;

                Session.Add("Pedido", p);
            }

            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));
            }
        }

        private void agregarArticuloaPedir(int idArticulo)
        {
            try
            {
                if (Session["ArtPedir"] == null)
                {
                    List<Articulos_PedirOC> artPedirN = new List<Articulos_PedirOC>();
                    Session.Add("ArtPedir", artPedirN);
                }
                List<Articulos_PedirOC> artPedir = Session["ArtPedir"] as List<Articulos_PedirOC>;

                Articulos_PedirOC artP = new Articulos_PedirOC();
                artP.IdArticulo = idArticulo;
                artP.Fecha = DateTime.Now;
                artP.Estado = 1;

                artPedir.Add(artP);

                Session.Add("ArtPedir", artPedir);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando item a artiulos a pedir. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                {
                    this.modificarPedido();
                }
                else
                {
                    this.generarPedido();
                }
            }
            catch
            {

            }
        }
        private void generarPedido()
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Cliente cl = this.contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));

                //Verifico que el tipo de moneda de todos los items sea el mismo
                if (!this.verificarMonedaItems())
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El tipo de moneda de los items no puede ser distinto, verifíquelo. "));
                    return;
                }


                if (p.cliente.id > 0)
                {
                    if (p.items.Count > 0)
                    {
                        //this.Pedido.items = items;
                        ////obtengo datos
                        // cambiar
                        p.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                        p.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                        p.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        p.fecha = DateTime.Now;
                        p.cotizacion.id = Convert.ToInt32(this.ListFormaVenta.SelectedValue);
                        p.vendedor.id = cl.vendedor.id;
                        p.formaPAgo.id = cl.formaPago.id;
                        p.listaP.id = cl.lisPrecio.id;
                        p.comentario = this.txtComentarios.Text;
                        p.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);
                        //datos entrega
                        p.entrega.Id = Convert.ToInt64(this.ListTipoEntrega.SelectedValue);
                        p.fechaEntrega = Convert.ToDateTime(this.txtFechaEntrega.Text, new CultureInfo("es-AR"));
                        //p.domicilioEntrega = this.txtDomicilioEntrega.Text;
                        //p.domicilioEntrega = this.dropList_DomicilioEntrega.Text;
                        p.domicilioEntrega = dropList_DomicilioEntrega.Items.Count > 1 ? dropList_DomicilioEntrega.SelectedItem.Text : "-";
                        //dropList_DomicilioEntrega.Items.Add(new ListItem(item.ItemArray[1] + ", " + item.ItemArray[2] + ", " + item.ItemArray[3], item.ItemArray[3].ToString()));
                        p.horaEntrega = this.txtHorarioEntrega.Text;
                        p.zonaEntrega = this.DropListZonaEntrega.SelectedValue;
                        p.senia = this.txtSenia.Text;

                        if (cotizacion == 1)
                        {
                            tp = controlador.obtenerTipoDoc("Cotizacion");
                        }
                        else
                        {
                            tp = controlador.obtenerTipoDoc("Pedido");
                        }
                        p.tipo = tp;

                        string perfil = Session["Login_NombrePerfil"] as string;
                        if (perfil == "Vendedor")
                        {
                            p.estado = this.controlador.obtenerEstadoDesc("Pendiente Vendedor");
                        }
                        else
                        {
                            if (perfil == "Cliente")
                            {
                                p.estado = this.controlador.obtenerEstadoDesc("A Autorizar");
                            }
                            else
                            {
                                p.estado.id = int.Parse(confEstados.EstadoInicialPedidos);
                            }
                        }

                        int i = this.controlador.ProcesarPedido(p);
                        if (i > 0)
                        {
                            ControladorPedidoEntity contPedEnt = new ControladorPedidoEntity();
                            if (accion == 4)//agrego el dato de la cotizacion y el pedido generado
                            {
                                int t = contPedEnt.agregarPedidoCotizacion(i, idCotizacion);
                                //cambio estado a cotizacion
                                t = contPedEnt.CambiarEstadoCotizaciones(idCotizacion, 6);
                            }
                            //Verifico si utiliza modo distribución (Cliente_Referidos, Pedidos_Referidos)
                            if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                            {
                                int j = contPedEnt.agregarPedidoReferido(i, p.cliente.id);
                            }

                            //guardo los articulos a pedir
                            this.guardarArticulosPedir();
                            Session.Remove("Pedido");
                            this.btnAgregar.Visible = false;
                            this.btnNuevo.Visible = true;
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta  " + this.labelNroPedido.Text);
                            if (cotizacion == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&co=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'ABMPedidos.aspx?c=1';", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'ABMPedidos.aspx';", true);
                            }

                            //Response.Redirect("ABMPedidos.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Pedido "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos al Pedido " + this.txtCodigo.Text));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un cliente "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando Pedido. " + ex.Message));
            }
        }
        private void modificarPedido()
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Cliente cl = this.contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                string fechaAux = ControladorPedidoEntity.obtenerFechaPedido(p.id);
                //Verifico que el tipo de moneda de todos los items sea el mismo
                if (!this.verificarMonedaItems())
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El tipo de moneda de los items no puede ser distinto, verifíquelo. "));
                    return;
                }

                if (p.cliente.id > 0)
                {
                    if (p.items.Count > 0)
                    {
                        p.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                        p.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);
                        p.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                        if (!string.IsNullOrEmpty(fechaAux))
                            p.fecha = Convert.ToDateTime(fechaAux, new CultureInfo("es-AR"));
                        else
                            p.fecha = DateTime.Now;
                        p.vendedor.id = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                        p.formaPAgo.id = cl.formaPago.id;
                        p.listaP.id = cl.lisPrecio.id;
                        p.comentario = this.txtComentarios.Text;
                        p.neto10 = Convert.ToDecimal(this.txtPorcDescuento.Text);
                        //datos entrega
                        p.entrega.Id = Convert.ToInt64(this.ListTipoEntrega.SelectedValue);
                        p.fechaEntrega = Convert.ToDateTime(this.txtFechaEntrega.Text, new CultureInfo("es-AR"));
                        //p.domicilioEntrega = this.txtDomicilioEntrega.Text;
                        p.domicilioEntrega = dropList_DomicilioEntrega.Items.Count > 1 ? dropList_DomicilioEntrega.SelectedItem.Text : "-";
                        p.horaEntrega = this.txtHorarioEntrega.Text;
                        p.zonaEntrega = this.DropListZonaEntrega.SelectedValue;
                        p.senia = this.txtSenia.Text;
                        p.estado.id = 1;//por defecto lo pongo en estado pendiente.

                        if(this.labelNroPedido.Text.ToLower().Contains("cotizacion"))
                        {
                            tp = controlador.obtenerTipoDoc("Cotizacion");
                            p.tipo = tp;
                            this.cotizacion = 1;
                        }
                        else
                        {
                            tp = controlador.obtenerTipoDoc("Pedido");
                            p.tipo = tp;
                        }
                        

                        string perfil = Session["Login_NombrePerfil"] as string;
                        if (perfil == "Vendedor")
                        {
                            p.estado = this.controlador.obtenerEstadoDesc("Pendiente Vendedor");
                        }
                        else
                        {
                            if (perfil == "Cliente")
                            {
                                p.estado = this.controlador.obtenerEstadoDesc("A Autorizar");
                            }
                            else
                            {
                                p.estado = this.controlador.obtenerEstadoDesc("Pendiente");
                            }
                        }

                        int i = this.controlador.ProcesarPedido(p);
                        if (i > 0)
                        {

                            //Verifico si utiliza modo distribución (Cliente_Referidos, Pedidos_Referidos)
                            ControladorPedidoEntity contPedEnt = new ControladorPedidoEntity();
                            if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                            {
                                int j = contPedEnt.agregarPedidoReferido(i, p.cliente.id);
                                int k = contPedEnt.eliminarPedidoReferidoPorPedido(this.idPedido);
                            }

                            contPedEnt.modificarNumeroPedidoEnt(p.id, p.numero);
                            string original = this.idPedido + ";";
                            controlador.anularPedidos(original);
                            Session.Remove("Pedido");
                            this.btnAgregar.Visible = false;
                            this.btnNuevo.Visible = true;

                            if(this.cotizacion == 1)
                            {
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico pedido  " + this.labelNroPedido.Text);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&co=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'ABMPedidos.aspx';", true);
                            }
                            else
                            {
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico pedido  " + this.labelNroPedido.Text);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'ABMPedidos.aspx';", true);
                                //Response.Redirect("ABMPedidos.aspx");
                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Pedido "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos al Pedido " + this.txtCodigo.Text));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un cliente "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando Pedido. " + ex.Message));
            }
        }
        private bool verificarMonedaItems()
        {
            try
            {
                int monedaOriginal = 0;

                string pmo = WebConfigurationManager.AppSettings.Get("PedidosMonedaOriginal");
                if (!String.IsNullOrEmpty(pmo))
                    monedaOriginal = Convert.ToInt32(pmo);

                if (monedaOriginal == 1)
                {
                    Pedido p = Session["Pedido"] as Pedido;

                    var firstItem = p.items.First();
                    string moneda = firstItem.articulo.monedaVenta.moneda;

                    var list = p.items.Where(x => x.articulo.monedaVenta.moneda != moneda).ToList();
                    if (list.Count > 0)
                        return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error verificando el tipo de moneda de los items del Pedido. Excepción: " + Ex.Message));
                return false;
            }
        }
        private void guardarArticulosPedir()
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                List<Articulos_PedirOC> artPedir = Session["ArtPedir"] as List<Articulos_PedirOC>;
                if (artPedir.Count > 0 && artPedir != null)
                {
                    contArtEnt.agregarArticulosAPedir(artPedir);
                }
                Session.Remove("ArtPedir");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando articulos a pedir. " + ex.Message));
            }
        }
        private void QuitarItem(object sender, EventArgs e)
        {
            try
            {
                //string idCodigo = (sender as LinkButton).ID.ToString().Substring(11, (sender as LinkButton).ID.Length - 11);
                string idCodigo = (sender as LinkButton).ID.ToString().Split('_')[1];
                string pos = (sender as LinkButton).ID.ToString().Split('_')[2];

                //obtengo el pedido del session
                Pedido ct = Session["Pedido"] as Pedido;
                foreach (ItemPedido item in ct.items)
                {
                    if ((item.articulo.codigo == idCodigo) && Convert.ToInt32(pos) == ct.items.IndexOf(item))
                    {
                        //lo quito
                        ct.items.Remove(item);
                        break;
                    }
                }

                //cargo el nuevo pedido a la sesion
                Session["Pedido"] = ct;

                //vuelvo a cargar los items
                this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item a la Pedido. " + ex.Message));
            }
        }
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", ".")); a chequear
                //decimal precio = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                this.totalItem();
                this.verificarDescuentoCantidad();
                this.txtDescuento.Focus();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "focoDesc();", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }
        protected void TxtDescuentoArri_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //decimal total = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", ".")); a chequear
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text.Replace(",", "."));
                //decimal total = Convert.ToDecimal(this.txtPUnitario.Text);
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                //decimal totalDesc = total * (1 - (desc / 100));
                //this.txtTotalArri.Text = decimal.Round(totalDesc, 2).ToString();
                this.totalItem();
                this.lbtnAgregarArticuloASP.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total con descuento. Verifique que ingreso numeros en Descuento" + ex.Message));
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
        private void totalItem()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtCantidad.Text))
                {
                    decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                    decimal precio = Convert.ToDecimal(this.txtPUnitario.Text);
                    decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);

                    decimal total = (cantidad * precio);
                    total = total - (total * (desc / 100));


                    this.txtTotalArri.Text = decimal.Round(total, 2).ToString();

                    //decimal total = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                    //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text.Replace(",", "."));
                    //decimal total = Convert.ToDecimal(this.txtPUnitario.Text);
                    //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                    //decimal totalDesc = total * (1 - (desc / 100));
                    //this.txtTotalArri.Text = decimal.Round(totalDesc, 2).ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total " + ex.Message));
            }
        }

        protected void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }

        //protected void txtRetencion_TextChanged(object sender, EventArgs e)
        //{
        //    this.actualizarTotales();
        //}

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtPtoVenta.Text = this.ListSucursal.SelectedValue;
            cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));

            //cargo vendedores de la sucursal
            this.cargarVendedor();
            //Me fijo si hay que cargar un cliente por defecto
            this.verificarClienteDefecto();
        }

        protected void ListPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.obtenerNroPedido();
        }

        protected void txtPorcRetencion_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
        }
        protected void cargarZonas()
        {
            try
            {
                List<Zona> listaZonas = contZona.obtenerZona();
                listaZonas.Insert(0, (new Zona { id = -1, nombre = "Seleccione..." }));

                this.DropListZonaEntrega.DataSource = listaZonas;
                this.DropListZonaEntrega.DataValueField = "id";
                this.DropListZonaEntrega.DataTextField = "nombre";

                this.DropListZonaEntrega.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista zonas. " + ex.Message));
            }
        }

        /// <summary>
        /// Metodo para habilitar o no el boton btnImportarXML segun la empresa que sea
        /// </summary>
        public void cambiarHabilitacionBotonbtnImportarXML()
        {
            if (ListEmpresa.SelectedItem.Text.ToLower() != "leval sa")
            {
                //btnImportarXML.Enabled = false;
                this.btnImportarXML.Attributes.Add("disabled", "true");
                // btnImportarXML.CssClass = "form-control";
            }
            else
            {
                this.btnImportarXML.Attributes.Remove("disabled");
                //this.btnImportarXML.Visible = true;
            }
        }

        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
            cambiarHabilitacionBotonbtnImportarXML();
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

        protected void ActualizarTotalPH(object sender, EventArgs e)
        {
            try
            {
                string posicion = (sender as TextBox).ID.ToString().Substring(5, (sender as TextBox).ID.Length - 5);
                posicion = posicion.Split('_')[0];

                Pedido ct = Session["Pedido"] as Pedido;
                ItemPedido item = ct.items[Convert.ToInt32(posicion)];
                item.cantidad = Convert.ToDecimal((sender as TextBox).Text.Replace(',', '.'), CultureInfo.InvariantCulture);

                item.porcentajeDescuento = 0;

                //verifico si tengo que hacer un descuento por cantidad
                decimal descCantidad = this.obtenerNuevoDescuentoCantidad(item.articulo.codigo, item.cantidad);
                if (descCantidad > 0)//si es descuento por cantidad, piso el del item, sino lo dejo
                {
                    item.porcentajeDescuento = descCantidad;
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
                Session["Pedido"] = ct;

                //vuelvo a cargar los items
                //this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMPedidos.aspx");
            }
            catch
            {

            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckBox1.Checked)
                {
                    this.phDatosEntrega.Visible = true;
                }
                else
                {
                    this.phDatosEntrega.Visible = false;
                }
            }
            catch
            {

            }
        }

        protected void btnAgregar_senia_Click(object sender, EventArgs e)
        {
            try
            {
                Pedido c = Session["Pedido"] as Pedido;
                int idCli = c.cliente.id;
                String senia = this.txtSenia.Text;

                string filtro = "a=1&cliente=" + idCli + "&empresa=" + ListEmpresa.SelectedValue + "&sucursal=" + ListSucursal.SelectedValue + "&puntoVenta=" + ListPuntoVenta.SelectedValue + "&tipo=1&s=" + senia;

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('../Cobros/CobranzaF.aspx?" + filtro + "','_blank')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando seña a pedido  - " + ex.Message + "\", {type: \"error\"});", true);
            }


        }

        protected void txtPorcRetencion_TextChanged1(object sender, EventArgs e)
        {
            this.actualizarTotales();
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
        //private void obtenerDatosReferenciaArticulo()
        //{
        //    try
        //    {
        //        ControladorArticulosEntity contArticulosEntitys = new ControladorArticulosEntity();
        //        controladorFactEntity contFactEntity = new controladorFactEntity();
        //        controladorFacturacion contFacturas = new controladorFacturacion();

        //        articulo a = contArticulosEntitys.obtenerArticuloEntityByCod(this.txtCodigo.Text);
        //        string datos = "";

        //        if (a != null)
        //        {
        //            Factura ultima = contFacturas.obtenerUltimaFacturaClienteArticulo(Convert.ToInt32(this.DropListClientes.SelectedValue), a.id);
        //            if (ultima != null)
        //            {
        //                if (ultima.id > 0)
        //                {
        //                    itemsFactura ultimoFacturado = contFactEntity.obtenerDetalleUltimaFacturaByArticulo(ultima.id, a.id);

        //                    datos += "Ultimo precio facturado: " + ultimoFacturado.precioUnitario.Value.ToString("C") + " - " + ultima.fecha.ToString("dd/MM/yyyy") + " / ";
        //                }
        //            }

        //            stock s = contArticulosEntitys.obtenerStockArticuloLocal(a.id, Convert.ToInt32(this.ListSucursal.SelectedValue));
        //            if (s != null)
        //            {
        //                datos += "Stock: " + s.stock1 + " - ";

        //                decimal comprometido = this.controlador.obtenerStockComprometidoPedidos(a.id, Convert.ToInt32(this.ListSucursal.SelectedValue));

        //                datos += "Comprometido suc.: " + comprometido + " - ";

        //                datos += "Disponible: " + (s.stock1 - comprometido) + " / ";
        //            }

        //            if (a.Articulos_Presentaciones.Count() > 0)
        //            {
        //                datos += "Presentacion Min: " + a.Articulos_Presentaciones.FirstOrDefault().Minima + " / ";
        //                datos += "Med: " + a.Articulos_Presentaciones.FirstOrDefault().Media + " / ";
        //                datos += "Max: " + a.Articulos_Presentaciones.FirstOrDefault().Maxima + " / ";
        //            }

        //            this.lblDatosReferenciaArt.Visible = true;
        //            this.lblDatosReferenciaArt.Text = datos;
        //        }
        //        else
        //        {
        //            this.lblDatosReferenciaArt.Text = "";
        //            this.lblDatosReferenciaArt.Visible = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblDatosReferenciaArt.Text = "";
        //        this.lblDatosReferenciaArt.Visible = false;
        //    }
        //}

        //private void obtenerDescuentosCantidadArticulo()
        //{
        //    try
        //    {
        //        ControladorArticulosEntity contArticulosEntitys = new ControladorArticulosEntity();

        //        Pedido p = Session["Pedido"] as Pedido;
        //        articulo a = contArticulosEntitys.obtenerArticuloEntityByCod(this.txtCodigo.Text);
        //        string datos = "";

        //        if (a != null)
        //        {
        //            var clienteDatos = this.contClienteEntity.obtenerClienteDatosByCliente(p.cliente.id);

        //            if (clienteDatos.Count > 0)
        //            {
        //                if (clienteDatos[0].AplicaDescuentoCantidad == 1)
        //                {
        //                    ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
        //                    Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCod(this.txtCodigo.Text);
        //                    if (artEnt != null)
        //                    {
        //                        var desc = artEnt.Articulos_Descuentos;
        //                        int contador = 1;
        //                        foreach (var item in desc)
        //                        {
        //                            datos += "Descuento " + contador + " Desde " + item.Desde + " - " + item.Hasta + " = " + item.Descuento.ToString() + "% / ";
        //                            contador++;
        //                        }

        //                        this.lblDescuentoCantidad.Visible = true;
        //                        this.lblDescuentoCantidad.Text = datos;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            this.lblDescuentoCantidad.Text = "";
        //            this.lblDescuentoCantidad.Visible = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblDescuentoCantidad.Text = "";
        //        this.lblDescuentoCantidad.Visible = false;
        //    }
        //}

        //private void obtenerPendientesCliente(int idArticulo, int idCliente)
        //{
        //    try
        //    {
        //        var resp = this.controlador.obtenerPedidosPendientesClientes(idCliente, idArticulo);
        //        if (resp > 0)
        //        {
        //            //Genero un string con el link para imprimir los pedidos pendientes por cliente
        //            string link = "ImpresionPedido.aspx?a=7&fd=" + DateTime.Now.AddYears(-2).ToString("dd/MM/yyyy") + "&fh=" + DateTime.Now.ToString("dd/MM/yyyy") + "&suc=-1&c=" + idCliente + "&g=0&art=-1";
        //            this.LabelPendientes.Visible = true;
        //            this.LabelPendientes.Text = "El cliente posee el articulo en pedidos pendientes. Cantidad pendiente: " + resp + ".  " + "<a href=\"#\" onclick=\"imprimirCantidadPendientesCliente('" + link + "')\">Ver</a>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        private void cantidadPendienteCliente(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception Ex)
            {

            }
        }

        public void verificarModoBlanco()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.modoBlanco == "1")
                {
                    //this.lbtnPRP.Visible = false;
                    //this.lbNC.Visible = false;
                    //this.lbND.Visible = false;
                    //this.lbtnPRP.Attributes.Add("style", "display:none");
                    //this.lbNC.Attributes.Add("style", "display:none");
                    //this.lbND.Attributes.Add("style", "display:none");
                }
            }
            catch
            {

            }
        }

        #region importacion
        protected void btnImportarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                this.importarPedido();
            }
            catch
            {

            }
        }
        private void importarPedido()
        {
            try
            {
                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;
                    int comienzoArticulos = 0;
                    int contador = 0;

                    while ((linea = sr.ReadLine()) != null)
                    {
                        if (comienzoArticulos > 0)
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            if (config.separadorListas == "0")// punto y coma
                            {
                                datos = linea.Split(';');
                            }

                            if (datos.Count() > 3)
                            {
                                if (!String.IsNullOrEmpty(datos[4]))
                                {
                                    decimal d;//para verificar que sea decimal
                                    if (Decimal.TryParse(datos[4], out d))
                                    {
                                        if (Convert.ToDecimal(datos[4]) > 0)
                                        {
                                            int i = this.AgregarItemImportadoAPedido(datos[0], Convert.ToDecimal(datos[4]), 0);
                                            if (i < 0)
                                            {
                                                contador++;
                                                //Session.Add("Pedido", null);
                                                this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                                this.phDatosEntrega.Visible = true;
                                                this.CheckBox1.Checked = true;
                                                //return;
                                            }
                                        }
                                        else
                                        {
                                            contador++;
                                            //Session.Add("Pedido", null);
                                            this.txtComentarios.Text += "\n Codigo: " + datos[0] + " con cantidad negativa o cero.";
                                            this.phDatosEntrega.Visible = true;
                                            this.CheckBox1.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        contador++;
                                        //Session.Add("Pedido", null);
                                        this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                        this.phDatosEntrega.Visible = true;
                                        this.CheckBox1.Checked = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            foreach (string col in datos)
                            {
                                if (col.Contains("*BOF*"))//si encuentro la marca de inicio
                                {
                                    comienzoArticulos = 1;//en la linea que sigue empiezan los articulos
                                }
                            }
                        }
                    }

                    this.cargarItems();
                    this.actualizarTotales();

                    if (contador > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron importar " + contador + " codigo(s). Revise las observaciones del pedido."));
                        this.txtComentarios.Focus();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito.", ""));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar un archivo .csv"));
                }
            }
            catch
            {

            }
        }
        private void importarPedido2()
        {
            try
            {
                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;
                    int comienzoArticulos = 0;
                    int contador = 0;

                    while ((linea = sr.ReadLine()) != null)
                    {
                        if (comienzoArticulos > 0)
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            if (config.separadorListas == "0")// punto y coma
                            {
                                datos = linea.Split(';');
                            }

                            if (datos.Count() > 3)
                            {
                                if (!String.IsNullOrEmpty(datos[4]))
                                {
                                    decimal d;//para verificar que sea decimal
                                    if (Decimal.TryParse(datos[4], out d))
                                    {
                                        if (Convert.ToDecimal(datos[4]) > 0)
                                        {
                                            int i = this.AgregarItemImportadoAPedido(datos[0], Convert.ToDecimal(datos[4]), 0);
                                            if (i < 0)
                                            {
                                                contador++;
                                                //Session.Add("Pedido", null);
                                                this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                                this.phDatosEntrega.Visible = true;
                                                this.CheckBox1.Checked = true;
                                                //return;
                                            }
                                        }
                                        else
                                        {
                                            contador++;
                                            //Session.Add("Pedido", null);
                                            this.txtComentarios.Text += "\n Codigo: " + datos[0] + " con cantidad negativa o cero.";
                                            this.phDatosEntrega.Visible = true;
                                            this.CheckBox1.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        contador++;
                                        //Session.Add("Pedido", null);
                                        this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                        this.phDatosEntrega.Visible = true;
                                        this.CheckBox1.Checked = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            foreach (string col in datos)
                            {
                                if (col.Contains("*BOF*"))//si encuentro la marca de inicio
                                {
                                    comienzoArticulos = 1;//en la linea que sigue empiezan los articulos
                                }
                            }
                        }
                    }

                    this.cargarItems();
                    this.actualizarTotales();

                    if (contador > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron importar " + contador + " codigo(s). Revise las observaciones del pedido."));
                        this.txtComentarios.Focus();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito.", ""));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar un archivo .csv"));
                }
            }
            catch
            {

            }
        }
        private void importarPedidoExcel()
        {
            try
            {
                Boolean fileOK = false;

                String path = Server.MapPath("../../content/excelFiles/pedidos");
                String fileExtension = "";
                if (FileUpload1.HasFile)
                {
                    fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".xlsx" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //guardo nombre archivo
                    string nombreArchivoExcel = FileUpload1.FileName;

                    //lo subo
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);

                    var pedidoExcel = new PedidoExcel();
                    var pedidos = pedidoExcel.traerDatos(path + FileUpload1.FileName);

                    if (pedidos != null)
                    {
                        List<StockCompleto> stockCompletoByArticulos = new List<StockCompleto>();
                        Session.Add("ListaDeStockCompletoByArticulo", stockCompletoByArticulos);
                        int contador = 0;
                        foreach (var item in pedidos)
                        {
                            if (Convert.ToDecimal(item.Cantidad) > 0)
                            {
                                int i = this.AgregarItemImportadoAPedido(item.Codigo, Convert.ToDecimal(item.Cantidad), item.Precio);
                                if (i < 0)
                                {
                                    contador++;
                                    this.txtComentarios.Text += "\n Codigo: " + item.Codigo + " no encontrado.";
                                    this.phDatosEntrega.Visible = true;
                                    this.CheckBox1.Checked = true;
                                }
                            }
                        }

                        this.cargarItems();
                        this.actualizarTotales();

                        if (contador > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron importar " + contador + " codigo(s). Revise las observaciones del pedido."));
                            this.txtComentarios.Focus();
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito.", ""));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Verificar codigos y cantidades del excel."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar un archivo .xlsx"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Error procesando excel " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error procesando excel " + ex.Message));
            }
        }
        private int AgregarItemImportadoAPedido(string codigo, decimal cantidad, decimal precio)
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Articulo articulo = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                if (articulo != null)
                {
                    if (precio > 0)
                        articulo.precioVenta = precio;

                    ItemPedido item = new ItemPedido();

                    item.articulo = articulo;
                    item.cantidad = cantidad;
                    item.descuento = 0;
                    item.porcentajeDescuento = 0;
                    item.precioUnitario = decimal.Round(articulo.precioVenta, 2);

                    try
                    {
                        item.descripcion = articulo.descripcion;
                        item.nroRenglon = p.items.Count() + 1;
                    }
                    catch { }

                    decimal total = (articulo.precioVenta * cantidad);
                    total = total - (total * (item.porcentajeDescuento / 100));
                    item.total = decimal.Round(total, 2);

                    AsignarStockCompletoAlItem(item);

                    p.items.Add(item);

                    Session.Add("Pedido", p);

                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error importando items pedido."));
                return -1;
            }
        }

        /// <summary>
        /// Este metodo me sirve para importar el archivo .xml para tener en cuenta el campo descricpion del articulo
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="cantidad"></param>
        /// <param name="precio"></param>
        /// <returns></returns>
        private int AgregarItemImportadoAPedidoXML(string codigo, decimal cantidad, decimal precio, string descripcionAgregar)
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Articulo articulo = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                if (articulo != null)
                {
                    if (precio > 0)
                        articulo.precioVenta = precio;
                    if (!string.IsNullOrEmpty(descripcionAgregar))
                        articulo.descripcion += " - COLOR: " + descripcionAgregar + "";

                    ItemPedido item = new ItemPedido();

                    item.articulo = articulo;
                    item.cantidad = cantidad;
                    item.descuento = 0;
                    item.porcentajeDescuento = 0;
                    item.precioUnitario = decimal.Round(articulo.precioVenta, 2);

                    try
                    {
                        item.descripcion = articulo.descripcion;
                        item.nroRenglon = p.items.Count() + 1;
                    }
                    catch { }

                    decimal total = (articulo.precioVenta * cantidad);
                    total = total - (total * (item.porcentajeDescuento / 100));
                    item.total = decimal.Round(total, 2);

                    AsignarStockCompletoAlItem(item);

                    p.items.Add(item);

                    Session.Add("Pedido", p);

                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error importando items pedido."));
                return -1;
            }
        }

        public void AsignarStockCompletoAlItem(ItemPedido item)
        {
            try
            {
                int idSucursal = Convert.ToInt32(ListSucursal.SelectedValue);
                decimal stockFisico = (decimal)contArticulosEntity.obtenerStockArticuloLocal(item.articulo.id, idSucursal).stock1;
                decimal stockComprometido = contArticulo.obtenerStockComprometidoByArticuloAndSucursal(item.articulo.id, idSucursal);
                decimal stockReal = stockFisico - stockComprometido;

                item.stockCompleto = new StockCompleto(stockFisico, stockComprometido, stockReal);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error en AsignarStockCompletoAlItem. Ex: " + ex.Message));
            }
        }

        /// <summary>
        /// Este metodo agregar items al pedido teniendo en cuenta al renglon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private int AgregarItemImportadoAPedidoConRenglon(string codigo, int renglon, decimal cantidad, string descripcion, decimal precio, decimal descuento)
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Articulo articulo = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                if (articulo != null)
                {
                    if (precio > 0)
                        articulo.precioVenta = precio;

                    ItemPedido item = new ItemPedido();
                    item.nroRenglon = renglon;
                    item.articulo = articulo;
                    item.cantidad = cantidad;
                    item.descuento = descuento;
                    item.porcentajeDescuento = decimal.Round((descuento * 100) / precio, 2);
                    item.precioUnitario = decimal.Round(articulo.precioVenta, 2);

                    try
                    {
                        item.descripcion = descripcion;
                        //item.nroRenglon = p.items.Count() + 1;
                    }
                    catch { }

                    decimal total = (articulo.precioVenta * cantidad);
                    total = total - (total * (item.porcentajeDescuento / 100));
                    item.total = decimal.Round(total, 2);

                    //AsignarStockCompletoAlItem(item);

                    p.items.Add(item);

                    Session.Add("Pedido", p);

                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error importando items pedido con renglon."));
                return -1;
            }
        }

        protected void btnImportarPedidoExcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.importarPedidoExcel();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        protected void btnImportarRenglon_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;
                    int comienzoArticulos = 0;
                    int contador = 0;

                    while ((linea = sr.ReadLine()) != null)
                    {
                        if (comienzoArticulos > 0)
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            if (config.separadorListas == "0")// punto y coma
                            {
                                datos = linea.Split(',');
                            }

                            if (datos.Count() > 3)
                            {
                                if (!String.IsNullOrEmpty(datos[4]))
                                {
                                    decimal d;//para verificar que sea decimal
                                    if (Decimal.TryParse(datos[4], out d))
                                    {
                                        if (Convert.ToDecimal(datos[4]) > 0)
                                        {
                                            int i = this.AgregarItemImportadoAPedidoConRenglon(datos[0], Convert.ToInt16(datos[1]), Convert.ToDecimal(datos[2]), datos[3], Convert.ToDecimal(datos[4]), Convert.ToDecimal(datos[5]));
                                            if (i < 0)
                                            {
                                                contador++;
                                                //Session.Add("Pedido", null);
                                                this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                                this.phDatosEntrega.Visible = true;
                                                this.CheckBox1.Checked = true;
                                                //return;
                                            }
                                        }
                                        else
                                        {
                                            contador++;
                                            //Session.Add("Pedido", null);
                                            this.txtComentarios.Text += "\n Codigo: " + datos[0] + " con cantidad negativa o cero.";
                                            this.phDatosEntrega.Visible = true;
                                            this.CheckBox1.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        contador++;
                                        //Session.Add("Pedido", null);
                                        this.txtComentarios.Text += "\n Codigo: " + datos[0] + " no encontrado.";
                                        this.phDatosEntrega.Visible = true;
                                        this.CheckBox1.Checked = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string[] datos = linea.Split(',');//obtengo datos del registro
                            foreach (string col in datos)
                            {
                                if (col.Contains("*BOF*"))//si encuentro la marca de inicio
                                {
                                    comienzoArticulos = 1;//en la linea que sigue empiezan los articulos
                                }
                            }
                        }
                    }

                    this.cargarItems();
                    this.actualizarTotales();

                    if (contador > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron importar " + contador + " codigo(s). Revise las observaciones del pedido."));
                        this.txtComentarios.Focus();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito.", ""));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar un archivo .csv"));
                }
            }
            catch
            {

            }
        }

        protected void lbtnAgregarMontoParaCalcularPorcentajeDescuento_Click(object sender, EventArgs e)
        {
            try
            {
                txtPorcDescuento.Text = "0";
                actualizarTotales();
                if (!String.IsNullOrWhiteSpace(txtMontoParaAplicarDescuentoAlTotal.Text))
                {
                    txtPorcDescuento.Text = ((Convert.ToDecimal(txtMontoParaAplicarDescuentoAlTotal.Text) / Convert.ToDecimal(txtTotal.Text)) * 100).ToString();
                    actualizarTotales();
                }
                //Para recalcular el label del modal de metodo de pago de tarjeta
                //this.lblMontoOriginal.Text = (Convert.ToDecimal(this.txtTotal.Text)).ToString();
                ScriptManager.RegisterClientScriptBlock(this.updatePanelAgregarMontoParaCalcularPorcentajeDescuento, updatePanelAgregarMontoParaCalcularPorcentajeDescuento.GetType(), "alert", "cerrarModalDescuentoMonto();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.updatePanelAgregarMontoParaCalcularPorcentajeDescuento, updatePanelAgregarMontoParaCalcularPorcentajeDescuento.GetType(), "alert", "Error en fun: lbtnAgregarMontoParaCalcularPorcentajeDescuento_Click. Ex: " + ex.Message, true);
            }
        }

        protected void btnImportarXML_Click(object sender, EventArgs e)
        {
            string rutaCompleta = "";

            try
            {
                Boolean fileOK = false;
                String path = Server.MapPath("../../ImportarPedidosXML/");

                if (FileUpload1.HasFile)
                {
                    //LEO Y VERFICO SI SE ELIGIO UN ARCHIVO Y SI ES DEL TIPO REQUERIDO
                    String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".txt" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    //VARIABLES
                    int importoBien = 0;
                    int contador = 0;
                    DateTime fechaEntregaXML;
                    string codigo = null;
                    decimal cantidad = 0;
                    decimal precio = 0; //POR AHORA NO LO USO, HASTA QUE LO PIDAN, SOLO USO EL PRECIO GUARDADO EN EL SISTEMA NUESTRO
                    string numeroPedidoClienteXML;
                    string idEspecificadorXML = "";
                    string nombreProfesionalXML = "";
                    string color = "";

                    //CHEQUEO SI EXISTE EL DIRECTORIO O NO
                    if (!Directory.Exists(path))
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "No existe directorio. " + path + ". Lo creo");

                        Directory.CreateDirectory(path);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Directorio creado");
                    }

                    //GUARDO EL ARCHIVO COMO .xml
                    string rutaTXT = FileUpload1.FileName.Replace(".txt", ".xml");

                    //SUBO EL ARCHIVO
                    rutaCompleta = path + rutaTXT;
                    FileUpload1.PostedFile.SaveAs(rutaCompleta);

                    //SETEO EL encoding a UTF-8 porque sino no puedo cargarlo, ya que lo cargan como UTF-16
                    Encoding utf8 = new UTF8Encoding(false);
                    Encoding ansi = Encoding.GetEncoding(1252);

                    string xml = File.ReadAllText(rutaCompleta, ansi);

                    XDocument xmlGuardar = XDocument.Parse(xml);

                    //SOBREESCRIBO EL ARCHIVO CON UTF-8
                    File.WriteAllText(
                        rutaCompleta,
                        @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlGuardar.ToString(),
                       utf8
                    );

                    //LO BUSCO DE VUELTA PERO AHORA ESTA COMO .xml CON EL ENCODING QUE NECESITO
                    var xmlDoc = XDocument.Load(@rutaCompleta);

                    //COMIENZO A LEER ATRIBUTOS PARA CARGAR LOS ITEMS

                    var fecha = from fec in xmlDoc.Descendants("SALESORDER") select fec;

                    foreach (XElement u in fecha.Elements("SALESTABLE"))
                    {
                        //FECHA DE ENTREGA DEL ARCHIVO
                        if ((fechaEntregaXML = Convert.ToDateTime((u.Element("DELIVERYDATE").Value), new CultureInfo("es-AR"))) == null)
                            txtFechaEntrega.Text = DateTime.Today.ToString("dd/MM/yyyy");

                        //NUMERO DE PEDIDO DEL CLIENTE EXTRAIDO DEL ARCHIVO

                        numeroPedidoClienteXML = u.Element("CUSTPROJREF").Value;
                        idEspecificadorXML = u.Element("IDESPECIFICADOR").Value;       //ID EXTRAIDO DEL ARCHIVO
                        nombreProfesionalXML = u.Element("NOMBREPROFESIONAL").Value;  //NOMBRE EXTRAIDO DEL ARCHIVO

                        //CARGO LOS DETALLES EN EL OBSERVACIONES (TXTCOMENTARIOS)
                        txtComentarios.Text = "PEDIDO HUNTER DOUGLAS:  " + numeroPedidoClienteXML + " \n" +
                            "DNI ARQUITECTO : " + idEspecificadorXML + " \n" +
                            "NOMBRE ARQUITECTO: " + nombreProfesionalXML + " \n";
                    }

                    var numeroPedido = from num in xmlDoc.Descendants("SALESORDER") select num;

                    foreach (XElement u in numeroPedido.Elements("ITEMS"))
                    {
                        foreach (XElement x in u.Elements("ITEM"))
                        {
                            
                            codigo = x.Element("INVENTID").Value.ToString();
                            cantidad = Convert.ToDecimal(x.Element("QTY").Value);
                            color = x.Element("COLOR").Value.ToString();
                            importoBien = AgregarItemImportadoAPedidoXML(codigo, cantidad, 0, color);
                            if (importoBien < 0)
                            {
                                this.txtComentarios.Text += "\n Codigo: " + codigo + " no encontrado.";
                            }
                            else
                            {
                                contador++;
                            }
                        }
                    }

                    //CARGO LOS ITEMS A LA PANTALLA
                    this.cargarItems();
                    this.actualizarTotales();

                    //MANDO MENSAJE DE ESTADO DE LA IMPORTACION
                    if (contador > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito. Se importaron " + contador + " items. Revise la casilla Observaciones.", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado. Se importaron " + contador + " items. Revise la casilla Observaciones.", ""));
                    }

                    //BORRO EL ARCHIVO QUE GUARDE
                    File.Delete(rutaCompleta);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar un archivo de tipo '.txt'. "));
                }
            }
            catch (Exception ex)
            {
                //BORRO EL ARCHIVO QUE GUARDE PORQUE HUBO UN ERROR
                File.Delete(rutaCompleta);
                Log.EscribirSQL(1, "ERROR", "Error importando pedidos desde archivo. Ubicacion: ABMPedidos.aspx / Metodo: btnImportarXML_Click " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("CATH: Error importando pedidos desde archivo."));
            }
        }
    }
}