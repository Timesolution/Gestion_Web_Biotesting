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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class PedidosP : System.Web.UI.Page
    {
        ControladorPedido controlador = new ControladorPedido();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPedidoEntity contPedEntity = new ControladorPedidoEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorRemitos contRemito = new controladorRemitos();
        Configuracion configuracion = new Configuracion();

        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private string fechaEntregaD;
        private string fechaEntregaH;
        private int idCliente;
        private int idEstado;
        int idVendedor;
        int tipoEntrega;
        int tipoFecha;
        int tipoFecha2;
        //int idArticulo;

        int accion;
        string numero = string.Empty;
        string cliente = string.Empty;
        string observacion = string.Empty;
        //para el de la lista
        int Vendedor;

        DataTable dtPedidosTemp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                estadoPedido est = controlador.obtenerEstadoDesc("Pendiente");
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idEstado = Convert.ToInt32(Request.QueryString["estado"]);
                Vendedor = Convert.ToInt32(Request.QueryString["V"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.tipoEntrega = Convert.ToInt32(Request.QueryString["te"]);
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                this.tipoFecha2 = Convert.ToInt32(Request.QueryString["tf2"]);
                //this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);

                this.numero = Request.QueryString["n"];
                this.cliente = Request.QueryString["c"];
                this.observacion = Request.QueryString["o"];

                if (!IsPostBack)
                {
                    dtPedidosTemp = new DataTable();
                    this.InicializarDtPedidos();
                    this.cargarSucursal();
                    this.cargarClientes();
                    this.cargarProveedores();
                    this.cargarVendedor();
                    this.cargarEstados();
                    this.cargarEntregas();
                    this.cargarGruposArticulos();
                    this.cargarArticulos();
                    this.cargarZonaEntrega();


                    if (fechaD == null && fechaH == null && suc == 0)
                    {
                        idEstado = est.id;
                        string perfil2 = Session["Login_NombrePerfil"] as string;
                        if (perfil2 == "Cliente")
                        {
                            idEstado = 0;
                        }
                        suc = (int)Session["Login_SucUser"];
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaEntregaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaEntregaH = DateTime.Now.ToString("dd/MM/yyyy");
                        this.tipoFecha = 1;
                    }

                    this.txtFechaDesde.Text = fechaD;
                    this.txtFechaHasta.Text = fechaH;
                    this.txtFechaDesdeCantidad.Text = fechaD;
                    this.txtFechaHastaCantidad.Text = fechaH;
                    this.txtFechaEntregaDesde.Text = fechaD;
                    this.txtFechaEntregaHasta.Text = fechaH;
                    this.txtFechaDesdeFamilia.Text = fechaD;
                    this.txtFechaHastaFamilia.Text = fechaH;
                    this.DropListSucursal.SelectedValue = suc.ToString();
                    this.DropListClientes.SelectedValue = idCliente.ToString();
                    this.DropListClientesFamilia.SelectedValue = idCliente.ToString();
                    this.ListVendedor.SelectedValue = Vendedor.ToString();
                    this.DropListEstado.SelectedValue = idEstado.ToString();
                    this.DropListEstadoFamilia.SelectedValue = idEstado.ToString();
                    this.DropListEstadoPendientes.SelectedValue = idEstado.ToString();
                    this.ListTipoEntrega.SelectedValue = this.tipoEntrega.ToString();
                    this.RadioFechaPedido.Checked = Convert.ToBoolean(this.tipoFecha);
                    this.RadioFechaEntrega.Checked = Convert.ToBoolean(this.tipoFecha2);
                }
                string perfil = Session["Login_NombrePerfil"] as string;
                //por defecto
                if (accion == 0)
                {
                    bultosTabla.Visible = false;
                    lineasTabla.Visible = false;
                    tiempoTabla.Visible = false;
                    if (perfil == "Vendedor")
                    {
                        //deshabilito el list de vendedor
                        this.ListVendedor.Attributes.Add("disabled", "true");
                        this.ListVendedor.SelectedValue = this.idVendedor.ToString();
                        this.Vendedor = idVendedor;
                        this.cargarPedidosRango(fechaD, fechaH, suc, idCliente, idEstado, this.Vendedor);
                    }
                    else if (perfil == "Cliente")
                    {
                        //deshabilito el list de vendedor
                        this.ListVendedor.Attributes.Add("disabled", "true");
                        //para asegurarme de cargar por defecto los pedidos del cliente
                        this.cargarPedidosRango(fechaD, fechaH, suc, this.idVendedor, idEstado, this.Vendedor);
                    }
                    else if (perfil == "Distribuidor" || perfil == "Lider" || perfil == "Experta")
                    {
                        this.cargarPedidosPorPadre(fechaD, fechaH, idVendedor, idEstado);
                        this.iconoBusqueda.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        this.cargarPedidosRango(fechaD, fechaH, suc, idCliente, idEstado, this.Vendedor);
                    }                    

                    if (idCliente > 0)
                    {
                        this.lbtnFacturar.Visible = true;
                    }
                }
                //busca por pedido
                if (accion == 1)
                {
                    this.buscarPorNumero();
                    this.lbtnFacturar.Visible = true;
                }
                //busca por numero
                if (accion == 2)
                {
                    this.buscarPorCliente();
                    this.lbtnFacturar.Visible = true;
                }
                //busco por cliente padre
                if (accion == 3)
                {
                    this.cargarPedidosPorPadre(fechaD, fechaH, idCliente, idEstado);
                    if (idCliente > 0)
                    {
                        lbtnFacturarFamilia.Visible = true;
                        this.iconoBusqueda.Attributes.Add("disabled", "disabled");
                    }
                }
                //busco por observacion
                if (accion == 4)
                {
                    this.buscarPorObservacion();
                    this.lbtnFacturar.Visible = true;
                }

                if(accion == 5)
                {
                    
                    cargarPedidosRango(fechaD, fechaH, suc, idCliente, idEstado, this.Vendedor);
                }

                if (idCliente > 0)
                    lbtnRemitir.Visible = true;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                        if (s == "37")
                        {

                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "Cliente")
                            {
                                //desactivo acciones
                                this.phAcciones.Visible = false;
                            }
                            valor = 1;
                        }
                        //Permiso ver saldo
                        if (s == "120")
                            this.lblSaldo.Visible = true;
                    }
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        #region cargasIniciales
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.ListSucursalCantidad.DataSource = dt;
                this.ListSucursalCantidad.DataValueField = "Id";
                this.ListSucursalCantidad.DataTextField = "nombre";
                this.ListSucursalCantidad.DataBind();

                this.DropListSucursal.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.DropListSucursal.Items.Insert(1, new ListItem("Todas", "0"));
                this.ListSucursalCantidad.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarClientes()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;

                controladorCliente contCliente = new controladorCliente();
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

                DataTable dt = new DataTable();
                bool auxiliar = false;

                if (perfil == "Vendedor")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }
                if (perfil == "Cliente")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                    this.btnBuscarCod.Visible = false;
                    this.txtCodCliente.Attributes.Add("disabled", "true");
                    //Oculto el Saldo en caso de que el logueado sea un Cliente
                    this.phSaldo.Visible = false;
                }
                if (perfil == "Distribuidor" || perfil == "Lider" || perfil == "Experta")
                {
                    auxiliar = true;
                    dt = contClienteEnt.obtenerReferidosDeCliente(this.idVendedor);
                    DataRow dr = dt.NewRow();
                    Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                    if (c != null)
                    {
                        dr["alias"] = c.razonSocial;
                    }

                    dr["id"] = this.idVendedor;
                    dt.Rows.InsertAt(dr, 0);
                }
                if (auxiliar == false)
                {
                    dt = contCliente.obtenerClientesDT();

                    //Si filtró, busco por el cliente que seleccionó
                    if (this.idCliente > 0)
                    {
                        dt = contCliente.obtenerClientesByClienteDT(this.idCliente);
                    }


                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }

                //DataTable dt = contCliente.obtenerClientesDT();



                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

                //para filtro de pendientes
                this.ListClientesCantidad.DataSource = dt;
                this.ListClientesCantidad.DataValueField = "id";
                this.ListClientesCantidad.DataTextField = "alias";

                this.ListClientesCantidad.DataBind();

                this.DropListClientesFamilia.DataSource = dt;
                this.DropListClientesFamilia.DataValueField = "id";
                this.DropListClientesFamilia.DataTextField = "alias";

                this.DropListClientesFamilia.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();

                dt = contCliente.obtenerProveedoresDT();
                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.ListProveedoresCantidad.DataSource = dt;
                this.ListProveedoresCantidad.DataValueField = "id";
                this.ListProveedoresCantidad.DataTextField = "alias";

                this.ListProveedoresCantidad.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        public void cargarVendedor()
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                DataRow dr3 = dt.NewRow();
                dr3["nombre"] = "Todos";
                dr3["id"] = 0;
                dt.Rows.InsertAt(dr3, 1);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedor.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }

        public void cargarEstados()
        {
            try
            {

                DataTable dt = controlador.obtenerEstadosPedidos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListEstado.DataSource = dt;
                this.DropListEstado.DataValueField = "id";
                this.DropListEstado.DataTextField = "descripcion";

                this.DropListEstado.DataBind();

                this.DropListEstadoFamilia.DataSource = dt;
                this.DropListEstadoFamilia.DataValueField = "id";
                this.DropListEstadoFamilia.DataTextField = "descripcion";

                this.DropListEstadoFamilia.DataBind();

                this.DropListEstadoPendientes.DataSource = dt;
                this.DropListEstadoPendientes.DataValueField = "id";
                this.DropListEstadoPendientes.DataTextField = "descripcion";

                this.DropListEstadoPendientes.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Estados de Pedidos a la lista. " + ex.Message));
            }
        }

        private void cargarEntregas()
        {
            try
            {
                var conExpreso = new ControladorExpreso();
                List<Gestion_Api.Entitys.TiposEntrega> listaEntregas = conExpreso.obtenerTiposEntrega();

                if (listaEntregas != null)
                {
                    listaEntregas.Insert(0, (new Gestion_Api.Entitys.TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));
                    listaEntregas.Insert(1, (new Gestion_Api.Entitys.TiposEntrega { Id = 0, Descripcion = "Todas" }));
                    this.ListTipoEntrega.DataSource = listaEntregas;
                    this.ListTipoEntrega.DataValueField = "Id";
                    this.ListTipoEntrega.DataTextField = "Descripcion";
                    this.ListTipoEntrega.DataBind();
                }
                else
                {
                    listaEntregas = new List<Gestion_Api.Entitys.TiposEntrega>();
                    listaEntregas.Insert(0, (new Gestion_Api.Entitys.TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));
                    listaEntregas.Insert(1, (new Gestion_Api.Entitys.TiposEntrega { Id = 0, Descripcion = "Todas" }));
                    this.ListTipoEntrega.DataSource = listaEntregas;
                    this.ListTipoEntrega.DataValueField = "Id";
                    this.ListTipoEntrega.DataTextField = "Descripcion";

                    this.ListTipoEntrega.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista entregas. " + ex.Message));
            }

            #region new r
            //try
            //{
            //    var contExpreso = new ControladorExpreso();
            //    List<Gestion_Api.Entitys.TiposEntrega> listaEntregas = new List<Gestion_Api.Entitys.TiposEntrega>();
            //    listaEntregas.Insert(0, (new Gestion_Api.Entitys.TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));
            //    listaEntregas.Insert(1, (new Gestion_Api.Entitys.TiposEntrega { Id = 0, Descripcion = "Todas" }));

            //    List<Gestion_Api.Entitys.TiposEntrega> listaAux = contExpreso.obtenerTiposEntrega();
            //    listaAux = contExpreso.obtenerTiposEntrega();
            //    foreach (var item in listaAux)
            //    {
            //        listaEntregas.Add(item);
            //    }

            //    if (listaAux != null)
            //    {//TODO r descomentar esto aca esta el error
            //        //listaEntregas.Insert(0, (new Gestion_Api.Entitys.TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));
            //        //listaEntregas.Insert(1, (new Gestion_Api.Entitys.TiposEntrega { Id = 0, Descripcion = "Todas" }));
            //        this.ListTipoEntrega.DataSource = listaEntregas;
            //        this.ListTipoEntrega.DataValueField = "Id";
            //        this.ListTipoEntrega.DataTextField = "Descripcion";
            //        this.ListTipoEntrega.DataBind();
            //    }
            //    else
            //    {
            //        listaEntregas = new List<Gestion_Api.Entitys.TiposEntrega>();
            //        listaEntregas.Insert(0, (new Gestion_Api.Entitys.TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));
            //        listaEntregas.Insert(1, (new Gestion_Api.Entitys.TiposEntrega { Id = 0, Descripcion = "Todas" }));
            //        this.ListTipoEntrega.DataSource = listaEntregas;
            //        this.ListTipoEntrega.DataValueField = "Id";
            //        this.ListTipoEntrega.DataTextField = "Descripcion";

            //        this.ListTipoEntrega.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista entregas. " + ex.Message));
            //}
            #endregion
        }

        public void cargarZonaEntrega()
        {
            try
            {
                controladorZona contZona = new controladorZona();

                var list = contZona.obtenerZona();
                if (list != null)
                {

                    list.Insert(0, new Gestion_Api.Entitys.Zona
                    {
                        id = 0,
                        nombre = "Seleccione..."
                    });


                    this.DropListZonaEntregaCantidad.DataSource = list;
                    this.DropListZonaEntregaCantidad.DataValueField = "id";
                    this.DropListZonaEntregaCantidad.DataTextField = "nombre";

                    this.DropListZonaEntregaCantidad.DataBind();

                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Zonas de Entrega. Excepción: " + Ex.Message));
            }
        }

        private void InicializarDtPedidos()
        {
            try
            {
                dtPedidosTemp.Columns.Add("fecha");
                dtPedidosTemp.Columns.Add("numero");
                dtPedidosTemp.Columns.Add("razon");
                dtPedidosTemp.Columns.Add("total");
                dtPedidosTemp.Columns.Add("estado");
                dtPedidos = dtPedidosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Se produjo un error generado dtpedidos " + ex.Message));

            }

        }

        public DataTable dtPedidos
        {

            get
            {
                if (ViewState["Pedidos"] != null)
                {
                    return (DataTable)ViewState["Pedidos"];
                }
                else
                {
                    return dtPedidosTemp;
                }
            }
            set
            {
                ViewState["Pedidos"] = value;
            }
        }
        private void cargarGruposArticulos()
        {
            try
            {
                DataTable dt = this.contArticulos.obtenerGruposArticulos();

                this.ListGruposCantidad.DataSource = dt;
                this.ListGruposCantidad.DataValueField = "id";
                this.ListGruposCantidad.DataTextField = "descripcion";
                this.ListGruposCantidad.DataBind();

                this.ListGruposCantidad.Items.Insert(0, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarArticulos()
        {
            try
            {
                DataTable dt = this.contArticulos.obtenerArticulos2();

                ////agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListArticulosCantidad.DataSource = dt;
                this.DropListArticulosCantidad.DataValueField = "id";
                this.DropListArticulosCantidad.DataTextField = "descripcion";

                this.DropListArticulosCantidad.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + Ex.Message));
            }
        }
        private int verificarPermisoEditar()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permiso2 = listPermisos.Where(x => x == "109").FirstOrDefault();
                if (permiso2 == null)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region funciones busquedas
        private void cargarPedidosRango(string fechaD, string fechaH, int idSuc, int idCliente, int idEstado, int vendedor)
        {
            try
            {
                if (fechaD != null && fechaD != null && idCliente != 0 && idSuc != 0 && idEstado != 0 && vendedor != 0)
                {
                    DataTable dt = this.controlador.obtenerPedidosRangoDT(fechaD, fechaH, fechaD, fechaH, idSuc, idCliente, idEstado, vendedor, this.tipoEntrega, this.tipoFecha, this.tipoFecha2);
                    this.cargarPedidos(dt);
                    this.cargarLabel(fechaD, fechaH, idSuc, idCliente, idEstado);
                }
                else
                {
                    DataTable dt = this.controlador.obtenerPedidosRangoDT(this.txtFechaDesde.Text, this.txtFechaHasta.Text, this.txtFechaEntregaDesde.Text, this.txtFechaEntregaHasta.Text,
                    Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue),
                    Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue), Convert.ToInt32(this.ListTipoEntrega.SelectedValue),
                    Convert.ToInt32(this.RadioFechaPedido.Checked), Convert.ToInt32(this.RadioFechaEntrega.Checked));
                    this.cargarPedidos(dt);
                    this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void buscarPorNumero()
        {
            try
            {
                Pedido p = this.controlador.obtenerPedidoPorNumero(this.numero);
                this.phPedidos.Controls.Clear();
                this.cargarEnPh(p);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando pedido por numero. " + ex.Message));
            }
        }

        private void buscarPorCliente()
        {
            try
            {
                //Pedido p = this.controlador.obtenerPedidoPorCliente(this.cliente);
                var pedidos = this.controlador.obtenerListaPedidosPorCliente(this.cliente);
                if (pedidos != null)
                {
                    this.phPedidos.Controls.Clear();
                    foreach (var p in pedidos)
                    {
                        this.cargarEnPh(p);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando pedido por cliente. " + ex.Message));
            }
        }
        private void buscarPorObservacion()
        {
            try
            {
                var pedidos = this.controlador.obtenerListaPedidosPorObservacion(this.observacion, 13); //13 es el tipo de documento pedido
                if (pedidos != null)
                {
                    this.phPedidos.Controls.Clear();
                    foreach (var p in pedidos)
                    {
                        this.cargarEnPh(p);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando pedido por cliente. " + ex.Message));
            }
        }
        #endregion

        #region carga informacion busquedas
        private void cargarLabel(string fechaD, string fechaH, int idSuc, int idCliente, int idEstado)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idCliente > 0)
                {
                    try
                    {
                        label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                    }
                    catch
                    { }
                }
                if (idSuc > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSuc.ToString()).Text + ",";
                }
                if (idEstado > -1)
                {
                    label += DropListEstado.Items.FindByValue(idEstado.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarLista(List<Pedido> pedidos)
        {
            try
            {
                decimal saldo = 0;
                foreach (Pedido p in pedidos)
                {
                    saldo += p.total;
                    this.cargarEnPh(p);
                }
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    lblSaldo.Text = decimal.Round((saldo / (decimal)1.21), 2).ToString("C");
                }
                else
                {
                    lblSaldo.Text = saldo.ToString("C");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Pedidos. " + ex.Message));

            }
        }
        private void cargarPedidos(DataTable dtPedidos)
        {
            try
            {
                decimal saldo = 0;
                foreach (DataRow row in dtPedidos.Rows)
                {
                    saldo += Convert.ToDecimal(row["total"]);
                    this.cargarEnPhDR(row);
                }
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    lblSaldo.Text = decimal.Round((saldo / (decimal)1.21), 2).ToString("C");
                }
                else
                {
                    lblSaldo.Text = saldo.ToString("C");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Pedidos. " + ex.Message));

            }
        }
        private void cargarEnPhDR(DataRow p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p["id"].ToString();             

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p["fecha"]).ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p["numero"].ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p["razonSocial"].ToString();
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);


                TableCell celTotal = new TableCell();
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    decimal total = Convert.ToDecimal(p["total"]);
                    celTotal.Text = "$" + Decimal.Round((total / (decimal)1.21), 2).ToString();
                }
                else
                {
                    celTotal.Text = Convert.ToDecimal(p["total"]).ToString("C");
                }
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                var estado = this.controlador.obtenerEstadoID(Convert.ToInt32(p["estado"]));
                celTipo.Text = estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                if (accion == 5)
                {
                    var pedido = this.controlador.obtenerPedidoId(Convert.ToInt32(p["id"]));

                    TableCell celLineas = new TableCell();
                    celLineas.Text = pedido.items.Count.ToString();
                    celLineas.HorizontalAlign = HorizontalAlign.Left;
                    celLineas.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celLineas);

                    TableCell celBultos = new TableCell();
                    celBultos.Text = pedido.items.Sum(x => x.cantidad).ToString();
                    celBultos.HorizontalAlign = HorizontalAlign.Left;
                    celBultos.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celBultos);

                    TableCell celTiempo = new TableCell();
                    var tiempo = configuracion.TiempoLineasPedido.Split(';');
                    try
                    {
                        TimeSpan tiempoPorLinea = new TimeSpan(0, Convert.ToInt32(tiempo[0]), Convert.ToInt32(tiempo[1]));
                        tiempoPorLinea = TimeSpan.FromTicks(tiempoPorLinea.Ticks * pedido.items.Count);
                        celTiempo.Text = tiempoPorLinea.ToString(@"hh\:mm\:ss");
                    }
                    catch { }
                    celTiempo.HorizontalAlign = HorizontalAlign.Left;
                    celTiempo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celTiempo);
                }

                //arego fila a tabla
                TableCell celAccion = new TableCell();

                LinkButton btnImprimir = new LinkButton();
                btnImprimir.CssClass = "btn btn-info ui-tooltip";
                btnImprimir.Attributes.Add("data-toggle", "tooltip");
                btnImprimir.Attributes.Add("title data-original-title", "Detalles");
                btnImprimir.ID = "btnSelec_" + p["id"].ToString();
                btnImprimir.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnImprimir.Font.Size = 12;
                btnImprimir.Click += new EventHandler(this.detallePedido);
                celAccion.Controls.Add(btnImprimir);
                celAccion.Width = Unit.Percentage(15);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + p["id"].ToString();//_id
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + p["id"].ToString();
                lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" >";
                lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                lDetail.Text += "</a>";
                if (this.verificarPermisoEditar() > 0)
                {
                    celAccion.Controls.Add(lDetail);
                }

                tr.Cells.Add(celAccion);

                phPedidos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando pedido. " + ex.Message));
            }

        }
        private void cargarEnPh(Pedido p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = p.fecha.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p.numero.ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p.cliente.razonSocial;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);


                TableCell celTotal = new TableCell();
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    celTotal.Text = "$" + Decimal.Round((p.total / (decimal)1.21), 2).ToString();
                }
                else
                {
                    celTotal.Text = "$" + p.total;
                }
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                celTipo.Text = p.estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                TableCell celLineas = new TableCell();
                celLineas.Text = p.items.Count.ToString();
                celLineas.HorizontalAlign = HorizontalAlign.Left;
                celLineas.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celLineas);

                TableCell celBultos = new TableCell();
                celBultos.Text = p.items.Sum(x => x.cantidad).ToString();
                celBultos.HorizontalAlign = HorizontalAlign.Left;
                celBultos.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celBultos);

                TableCell celTiempo = new TableCell();
                var tiempo = configuracion.TiempoLineasPedido.Split(';');
                try
                {
                    TimeSpan tiempoPorLinea = new TimeSpan(0, Convert.ToInt32(tiempo[0]), Convert.ToInt32(tiempo[1]));
                    tiempoPorLinea = TimeSpan.FromTicks(tiempoPorLinea.Ticks * p.items.Count);
                    celTiempo.Text = tiempoPorLinea.ToString(@"hh\:mm\:ss");
                }
                catch { }
                celTiempo.HorizontalAlign = HorizontalAlign.Left;
                celTiempo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTiempo);

                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Detalles");
                btnEliminar.ID = "btnSelec_" + p.id;
                btnEliminar.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnEliminar.Font.Size = 12;
                btnEliminar.Click += new EventHandler(this.detallePedido);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + p.id;//_id
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + p.id.ToString();
                lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p.id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" >";
                lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                lDetail.Text += "</a>";
                if (this.verificarPermisoEditar() > 0)
                {
                    celAccion.Controls.Add(lDetail);
                }

                tr.Cells.Add(celAccion);

                phPedidos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando pedido. " + ex.Message));
            }

        }
        #endregion
        private void detallePedido(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idPedido = atributos[1];

                Configuracion config = new Configuracion();

                try
                {//Si es un pedido pendiente de vendedor, lo cambio a pendiente la primera vez que lo imprimo
                    Pedido ped = this.controlador.obtenerPedidoId(Convert.ToInt32(idPedido));
                    if (ped.estado.id == 5)
                    {
                        int i = this.contPedEntity.cambiarEstadoPedido(ped.id, 1);
                        if (i > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Cambio estado pendiente vendedor a pendiente pedido id: " + ped.id);
                        }
                    }
                }
                catch { }

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + idPedido + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog(" + idPedido + ")", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        //obtengo vendedor del cliente
        private void obtenerVendedor(int Cliente)
        {
            try
            {
                controladorCliente contCl = new controladorCliente();
                var cl = contCl.obtenerClienteID(Cliente);
                this.ListVendedor.SelectedValue = cl.vendedor.id.ToString();
            }
            catch
            {

            }
        }
        private void obtenerClientesVendedor(int idvendedor)
        {
            try
            {
                controladorCliente contCl = new controladorCliente();
                DataTable dt = contCl.obtenerClientesByVendedorDT(idvendedor);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        #region funciones botones
        protected void btnBuscarProveedorCantidad_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedoresAliasDT(this.txtProveedorCantidad.Text);

                //cargo la lista
                this.ListProveedoresCantidad.DataSource = dtProveedores;
                this.ListProveedoresCantidad.DataValueField = "id";
                this.ListProveedoresCantidad.DataTextField = "alias";
                this.ListProveedoresCantidad.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Proveedor" + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        if (this.RadioFechaPedido.Checked)
                        {
                            Response.Redirect("PedidosP.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal="
                                + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&estado=" + DropListEstado.SelectedValue
                                + "&v=" + ListVendedor.SelectedValue + "&te=" + ListTipoEntrega.SelectedValue + "&tf=" + Convert.ToInt32(RadioFechaPedido.Checked) + "&tf2=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&art=");
                        }
                        else
                        {
                            Response.Redirect("PedidosP.aspx?fechadesde=" + this.txtFechaEntregaDesde.Text + "&fechaHasta=" + this.txtFechaEntregaHasta.Text + "&Sucursal="
                                + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&estado=" + DropListEstado.SelectedValue
                                + "&v=" + ListVendedor.SelectedValue + "&te=" + ListTipoEntrega.SelectedValue + "&tf=" + Convert.ToInt32(RadioFechaPedido.Checked) + "&tf2=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&art=");
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de pedidos. " + ex.Message));

            }
        }
        protected void btnBuscarNumeros_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtNumeroPedido.Text))
            {
                Response.Redirect("PedidosP.aspx?a=1&n=" + this.txtNumeroPedido.Text);
            }
            if (!string.IsNullOrEmpty(this.txtCodigoCliente.Text))
            {
                Response.Redirect("PedidosP.aspx?a=2&c=" + this.txtCodigoCliente.Text);
            }
            if (!string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                Response.Redirect("PedidosP.aspx?a=4&o=" + this.txtObservacion.Text);
            }
        }
        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox ch = null;
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    TableRow tr = C as TableRow;
                    if(accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;

                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("../../Formularios/Facturas/ABMFacturas.aspx?accion=5&pedidos=" + idtildado);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para facturar. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.controlador.anularPedidos(idtildado);
                    if (i > 0)
                    {
                        Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "ANULACION Pedido id: " + idtildado);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedidos anulados con exito. ", "PedidosP.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Pedidos. "));

                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para facturar. " + ex.Message));
            }
        }
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox ch = null;
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;

                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.controlador.autorizarPedidos(idtildado);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedidos autorizados con exito. ", "PedidosP.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error autorizando Pedidos. "));

                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error autorizando pedidos . " + ex.Message));
            }
        }

        //boton imprimir
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = this.dtPedidos;

                foreach (var control in this.phPedidos.Controls)
                {
                    DataRow drDatos = dtDatos.NewRow();
                    TableRow tr = control as TableRow;

                    //drDatos[0] = tr.ID;
                    drDatos[0] = tr.Cells[0].Text;
                    drDatos[1] = tr.Cells[1].Text;
                    drDatos[2] = tr.Cells[2].Text;
                    drDatos[3] = tr.Cells[3].Text;
                    drDatos[4] = tr.Cells[4].Text;

                    dtDatos.Rows.Add(drDatos);
                }

                Session.Add("dtDatosPedidos", dtDatos);


                Response.Redirect("ImpresionPedido.aspx?a=2");
            }
            catch
            {

            }
        }

        //boton buscar en el cliente
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                if (dtClientes.Rows.Count > 1)
                {
                    //agrego todos
                    DataRow dr = dtClientes.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dtClientes.Rows.InsertAt(dr, 0);
                }

                if (string.IsNullOrEmpty(buscar))
                {
                    DataRow dr2 = dtClientes.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dtClientes.Rows.InsertAt(dr2, 1);
                }

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        //buscar
        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.obtenerVendedor(Convert.ToInt32(this.DropListClientes.SelectedValue));
            }
            catch
            {

            }
        }

        protected void lbtnRemitir_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                int contador = 0;
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        contador++;
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }

                if(contador > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo puede remitir un pedido a la vez!"));
                    return;
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "" && id != null)
                        {
                            Pedido p = new Pedido();
                            p = this.controlador.obtenerPedidoId(Convert.ToInt32(id));
                            if (p != null)
                            {
                                Response.Redirect("ABMRemitos.aspx?accion=4&id_ped=" + p.id);
                                int i = this.contRemito.RemitirDesdePedido(p);

                                if (i < 1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Remito desde pedido. "));
                                    return;
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo pedido a remitir. "));
                                return;
                            }
                        }
                    }

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "PedidosP.aspx"));

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un documento!. "));
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void ListVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 != "Cliente")
                {
                    this.obtenerClientesVendedor(Convert.ToInt32(ListVendedor.SelectedValue));
                }

            }
            catch
            {

            }
        }

        protected void lbtnImprimirCantidadNeto_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=3&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&art=" + this.DropListArticulosCantidad.SelectedValue + "&p=" + this.ListProveedoresCantidad.SelectedValue + "&ze=" + this.DropListZonaEntregaCantidad.SelectedValue + "&ep=" + this.DropListEstadoPendientes.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnExportarCantidadNeto_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionPedido.aspx?a=3&ex=1&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&art=" + this.DropListArticulosCantidad.SelectedValue + "&p=" + this.ListProveedoresCantidad.SelectedValue + "&ze=" + this.DropListZonaEntregaCantidad.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnImprimirPedCliente_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=7&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&art=" + this.DropListArticulosCantidad.SelectedValue + "&p=" + this.ListProveedoresCantidad.SelectedValue + "&ze=" + this.DropListZonaEntregaCantidad.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnCargarBultos_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargaImpresionBultosPedido(0);
            }
            catch
            {

            }
        }
        protected void lbtnCargarImprimirBultos_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargaImpresionBultosPedido(1);
            }
            catch (Exception ex)
            {

            }
        }
        private void cargaImpresionBultosPedido(int imprime)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    var pb = this.contPedEntity.obtenerCantidadBultosPedido(Convert.ToInt32(idtildado));
                    if (pb != null)
                    {
                        pb.CantidadBultos = Convert.ToInt32(this.txtCantidadBultos.Text);
                        int i = this.contPedEntity.modificarBultosPedido(pb);
                        if (i >= 0)
                        {
                            Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Carga de bultos Pedido id: " + idtildado);
                            if (imprime > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=4&Pedido=" + idtildado + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cargado con exito. ", Request.Url.ToString()));
                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
                        }
                    }
                    else
                    {
                        Gestion_Api.Entitys.Pedidos_Bultos p = new Gestion_Api.Entitys.Pedidos_Bultos();
                        p.IdPedido = Convert.ToInt32(idtildado);
                        p.CantidadBultos = Convert.ToInt32(this.txtCantidadBultos.Text);
                        int i = this.contPedEntity.agregarBultosPedido(p);
                        if (i > 0)
                        {
                            Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Carga de bultos Pedido id: " + idtildado);
                            if (imprime > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=4&Pedido=" + idtildado + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cargado con exito. ", Request.Url.ToString()));
                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnModula_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.controlador.generarPedidosModula(idtildado);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedidos enviados a modula con exito. ", "PedidosP.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar pedidos a modula"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para facturar. " + ex.Message));
            }
        }
        protected void btnBuscarClienteCantidad_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(this.txtClienteCantidad.Text);

                //cargo la lista
                this.ListClientesCantidad.DataSource = dtClientes;
                this.ListClientesCantidad.DataValueField = "id";
                this.ListClientesCantidad.DataTextField = "alias";
                this.ListClientesCantidad.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        protected void lbtnBuscarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtArticuloCantidad.Text.Replace(' ', '%');
                DataTable dt = this.contArticulos.obtenerArticulosByDescDT(buscar);

                //cargo la lista
                this.DropListArticulosCantidad.DataSource = dt;
                this.DropListArticulosCantidad.DataValueField = "id";
                this.DropListArticulosCantidad.DataTextField = "descripcion";
                this.DropListArticulosCantidad.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + ex.Message));
            }

        }
        protected void ltbnConsolidados_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    //Response.Redirect("ImpresionPedido.aspx?a=3&ex=1&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&art=" + this.DropListArticulosCantidad.SelectedValue);
                    Response.Redirect("ImpresionPedido.aspx?a=5&ex=1&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&pedidos=" + idtildado);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }
            }
            catch
            {

            }

        }
        protected void lbtnConsolidadosPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=5&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&pedidos=" + idtildado + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }
            }
            catch
            {

            }
        }

        #endregion

        #region Familia
        protected void lbtnBuscarClienteFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();
                DataTable dtClientes = contClienteEnt.obtenerReferidosDeClienteAlias(this.idVendedor, this.txtClienteFamilia.Text);

                //cargo la lista
                this.DropListClientesFamilia.DataSource = dtClientes;
                this.DropListClientesFamilia.DataValueField = "id";
                this.DropListClientesFamilia.DataTextField = "alias";
                this.DropListClientesFamilia.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        protected void lbtnBuscarPedidoFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesdeFamilia.Text) && (!String.IsNullOrEmpty(txtFechaHastaFamilia.Text)))
                {
                    if (DropListClientesFamilia.SelectedValue != "-1" && DropListClientesFamilia.SelectedValue != "0")
                    {
                        Response.Redirect("PedidosP.aspx?a=3&fechadesde=" + txtFechaDesdeFamilia.Text + "&fechaHasta=" + txtFechaHastaFamilia.Text + "&Cliente=" + DropListClientesFamilia.SelectedValue + "&estado=" + DropListEstadoFamilia.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar un cliente"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de pedidos. " + Ex.Message));

            }
        }
        private void cargarPedidosPorPadre(string fechaD, string fechaH, int idCliente, int idEstado)
        {
            try
            {
                if (fechaD != null && fechaD != null && idCliente != 0 && idEstado != 0)
                {
                    DataTable dt = this.controlador.obtenerPedidosPorPadre(fechaD, fechaH, idCliente, idEstado);
                    this.cargarPedidos(dt);
                    this.cargarLabel(fechaD, fechaH, this.suc, idCliente, idEstado);
                }
                else
                {
                    DataTable dt = this.controlador.obtenerPedidosPorPadre(this.txtFechaDesdeFamilia.Text, this.txtFechaHastaFamilia.Text,
                      Convert.ToInt32(this.DropListClientesFamilia.SelectedValue), Convert.ToInt32(this.DropListEstadoFamilia.SelectedValue));
                    this.cargarPedidos(dt);
                    this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + Ex.Message));
            }
        }
        protected void lbtnFacturarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                //Verifico si el cliente padre tiene un padre
                int idPadre = this.contPedEntity.verificarAbuelo(this.idCliente);
                if (idPadre > 0)
                {
                    this.idCliente = idPadre;
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("../../Formularios/Facturas/ABMFacturas.aspx?accion=12&pedidos=" + idtildado + "&cp=" + this.idCliente);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos a facturar. " + Ex.Message));
            }
        }
        protected void lbtnExportarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phPedidos.Controls)
                {
                    CheckBox ch = null;
                    TableRow tr = C as TableRow;
                    if (accion == 5)
                        ch = tr.Cells[8].Controls[2] as CheckBox;
                    else
                        ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("ImpresionPedido.aspx?a=6&ex=1&fd=" + this.txtFechaDesdeCantidad.Text + "&fh=" + this.txtFechaHastaCantidad.Text + "&suc=" + this.ListSucursalCantidad.SelectedValue + "&c=" + this.ListClientesCantidad.SelectedValue + "&g=" + this.ListGruposCantidad.SelectedValue + "&pedidos=" + idtildado);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error enviando a exportar pedido por grupo."));
            }
        }


        #endregion

        protected void lbtnVistaAvanzada_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RadioFechaPedido.Checked)
                {
                    Response.Redirect("PedidosP.aspx?a=5&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal="
                        + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&estado=" + DropListEstado.SelectedValue
                        + "&v=" + ListVendedor.SelectedValue + "&te=" + ListTipoEntrega.SelectedValue + "&tf=" + Convert.ToInt32(RadioFechaPedido.Checked) + "&tf2=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&art=");
                }
                else
                {
                    Response.Redirect("PedidosP.aspx?a=5&fechadesde=" + this.txtFechaEntregaDesde.Text + "&fechaHasta=" + this.txtFechaEntregaHasta.Text + "&Sucursal="
                        + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&estado=" + DropListEstado.SelectedValue
                        + "&v=" + ListVendedor.SelectedValue + "&te=" + ListTipoEntrega.SelectedValue + "&tf=" + Convert.ToInt32(RadioFechaPedido.Checked) + "&tf2=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&art=");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}