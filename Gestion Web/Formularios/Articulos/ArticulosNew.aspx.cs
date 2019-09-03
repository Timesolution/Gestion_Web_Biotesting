using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ArticulosNew : System.Web.UI.Page
    {

        private controladorListaPrecio contPrecio = new controladorListaPrecio();
        private controladorArticulosNew contArticulo = new controladorArticulosNew();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
        private ControladorCobranzaEntity contCobranzaEntity = new ControladorCobranzaEntity();
        private controladorPais contPais = new controladorPais();
        private controladorListaPrecio contListaPrecio = new controladorListaPrecio();
        private controladorCliente contCliente = new controladorCliente();


        int idSucursal;
        //int grupo;
        //int subgrupo;
        //int marca;
        //int dias;
        //int proveedor;
        //int soloProveedorPredeterminado;
        //string descSubGrupo;

        Configuracion config = new Configuracion();

        Mensajes m = new Mensajes();
        List<Gestion_Api.Entitys.Promocione> listPromociones;
        int permisoEliminar = 0;
        int permisoStockValorizado = 0;//1 muestra costo, 0 muestra costo imponible
        int permisoMostrarBotonAgregarMateriasPrimas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    idSucursal = (int)Session["Login_SucUser"];
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    //cargo combos
                    this.cargarGruposArticulos();
                    this.cargarSubGruposArticulos(Convert.ToInt32(ListGrupo.SelectedValue));
                    this.cargarMarcasArticulos();
                    this.cargarClientes();
                    this.cargarSucursal();
                    this.cargarListaPrecio();
                    this.cargarListaCategoria();
                    this.txtFechaHasta_St.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaRefDesde.Text = DateTime.Now.AddMonths(-6).ToString("dd/MM/yyyy");
                    this.txtFechaRefHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaDesdeMovStock.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaHastaMovStock.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaDesdeNoVendido.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaHastaNoVendido.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtDesdeIEArticulos.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtHastaIEArticulos.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    CargarProveedor();
                    //Obtengo todas las promociones
                    this.listPromociones = this.contArticulosEntity.obtenerPromociones();
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

                string permiso = listPermisos.Where(x => x == "14").FirstOrDefault();

                if (permiso == null)
                {
                    return 0;
                }
                else
                {
                    //verifico si puede cambiar preci0s
                    string perfil = Session["Login_NombrePerfil"] as string;
                    string permiso2 = listPermisos.Where(x => x == "62").FirstOrDefault();
                    string permisoCambioSuc = listPermisos.Where(x => x == "207").FirstOrDefault();

                    if (permiso2 != null)
                    {
                        this.phActualizacionPrecios.Visible = true;
                        this.permisoEliminar = 1;
                    }
                    //verifico si puede cambiar sucursal
                    if (perfil == "SuperAdministrador" || perfil == "Stock" || permiso2 != null)
                    {
                        this.DropListSucursal_St2.Attributes.Remove("disabled");
                        this.DropListSucursalRef.Attributes.Remove("disabled");
                        this.DropListSucNoVendido.Attributes.Remove("disabled");
                    }
                    else
                    {
                        this.DropListSucursal_St2.SelectedValue = Session["Login_SucUser"].ToString();
                        this.DropListSucursalRef.SelectedValue = Session["Login_SucUser"].ToString();
                        this.DropListSucNoVendido.SelectedValue = Session["Login_SucUser"].ToString();
                    }

                    if (string.IsNullOrEmpty(permisoCambioSuc))
                    {
                        listSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                        listSucursal.Enabled = false;
                        listSucursal.CssClass = "form-control";
                    }

                    if (listPermisos.Contains("179"))
                        this.permisoStockValorizado = 1;

                    //verifico si muestro el boton de agregar materias primas de los articulos
                    string permiso195 = listPermisos.Where(x => x == "195").FirstOrDefault();
                    if (permiso195 != null)
                    {
                        this.permisoMostrarBotonAgregarMateriasPrimas = 1;
                    }

                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        private void cargarGruposArticulos()
        {
            try
            {
                DataTable dt = contArticulo.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos SubGrupos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.ListGrupo.DataSource = dt;
                this.ListGrupo.DataValueField = "id";
                this.ListGrupo.DataTextField = "descripcion";

                this.ListGrupo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarSubGruposArticulos(int selectedGrupo)
        {
            try
            {
                if (selectedGrupo == 0)
                {
                    DataTable dt = contArticulo.obtenerSubGruposDistinctDT();


                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["descripcion"] = "Todos";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListSubGrupo.DataSource = dt;
                    this.ListSubGrupo.DataValueField = "id";
                    this.ListSubGrupo.DataTextField = "descripcion";

                    this.ListSubGrupo.DataBind();
                }
                else
                {
                    DataTable dt = contArticulo.obtenerSubGruposArticulos(selectedGrupo);

                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["descripcion"] = "Todos";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListSubGrupo.DataSource = dt;
                    this.ListSubGrupo.DataValueField = "id";
                    this.ListSubGrupo.DataTextField = "descripcion";

                    this.ListSubGrupo.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarMarcasArticulos()
        {
            try
            {
                DataTable dt = contArticulo.obtenerMarcasDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["marca"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListMarca.DataSource = dt;
                this.ListMarca.DataValueField = "id";
                this.ListMarca.DataTextField = "marca";

                this.ListMarca.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando marcas de articulos a la lista. " + ex.Message));
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

                this.listSucursal.DataSource = dt;
                this.listSucursal.DataValueField = "Id";
                this.listSucursal.DataTextField = "nombre";
                this.listSucursal.DataBind();

                this.ListSucursalEtiquetas.DataSource = dt;
                this.ListSucursalEtiquetas.DataValueField = "Id";
                this.ListSucursalEtiquetas.DataTextField = "nombre";
                this.ListSucursalEtiquetas.DataBind();

                this.DropListSucursalRef.DataSource = dt;
                this.DropListSucursalRef.DataValueField = "Id";
                this.DropListSucursalRef.DataTextField = "nombre";
                this.DropListSucursalRef.DataBind();
                this.DropListSucursalRef.SelectedValue = Session["Login_SucUser"].ToString();

                this.ListSucursalIEArticulos.DataSource = dt;
                this.ListSucursalIEArticulos.DataValueField = "Id";
                this.ListSucursalIEArticulos.DataTextField = "nombre";
                this.ListSucursalIEArticulos.DataBind();
                this.ListSucursalIEArticulos.SelectedValue = Session["Login_SucUser"].ToString();

                //stock no vendido
                this.DropListSucNoVendido.DataSource = dt;
                this.DropListSucNoVendido.DataValueField = "Id";
                this.DropListSucNoVendido.DataTextField = "nombre";
                this.DropListSucNoVendido.DataBind();
                this.DropListSucNoVendido.SelectedValue = Session["Login_SucUser"].ToString();

                dt.Rows[0].ItemArray[2] = "Seleccione...";
                dr["nombre"] = "Seleccione...";

                this.DropListSucursal_St.DataSource = dt;
                this.DropListSucursal_St.DataValueField = "Id";
                this.DropListSucursal_St.DataTextField = "nombre";

                this.DropListSucursal_St.DataBind();

                this.DropListSucursal_St2.DataSource = dt;
                this.DropListSucursal_St2.DataValueField = "Id";
                this.DropListSucursal_St2.DataTextField = "nombre";

                this.DropListSucursal_St2.DataBind();
                this.DropListSucursal_St2.SelectedValue = Session["Login_SucUser"].ToString();

                //informe stock unico en sucursal y en la otra no
                this.ListSucursalCentral.DataSource = dt;
                this.ListSucursalCentral.DataValueField = "Id";
                this.ListSucursalCentral.DataTextField = "nombre";
                this.ListSucursalCentral.DataBind();
                this.ListSucursalCentral.SelectedValue = Session["Login_SucUser"].ToString();

                this.ListSucursalComparar.DataSource = dt;
                this.ListSucursalComparar.DataValueField = "Id";
                this.ListSucursalComparar.DataTextField = "nombre";
                this.ListSucursalComparar.DataBind();
                this.ListSucursalComparar.SelectedValue = Session["Login_SucUser"].ToString();
                //list informe mov stock sucursal
                this.ListSucursalMovStock.DataSource = dt;
                this.ListSucursalMovStock.DataValueField = "Id";
                this.ListSucursalMovStock.DataTextField = "nombre";
                this.ListSucursalMovStock.DataBind();
                this.ListSucursalMovStock.SelectedValue = Session["Login_SucUser"].ToString();


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

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();


                dt = contCliente.obtenerProveedoresDT();
                dt2 = contCliente.obtenerProveedoresDT();


                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();

                this.DropListProvRef.DataSource = dt;
                this.DropListProvRef.DataValueField = "id";
                this.DropListProvRef.DataTextField = "alias";

                this.DropListProvRef.DataBind();

                DataRow dr2 = dt.NewRow();
                dr2["razonSocial"] = "Todos";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                this.ListRazonSocial.DataSource = dt;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";
                this.ListRazonSocial.DataBind();

                //stock no vendido
                this.DropListProvNoVendido.DataSource = dt;
                this.DropListProvNoVendido.DataValueField = "id";
                this.DropListProvNoVendido.DataTextField = "razonSocial";
                this.DropListProvNoVendido.DataBind();

                //list modal actualizacion precios
                DataRow dr3 = dt2.NewRow();
                dr3["razonSocial"] = "Seleccione...";
                dr3["id"] = -1;
                dt2.Rows.InsertAt(dr3, 0);
                this.DropListOtroProveedor.DataSource = dt2;
                this.DropListOtroProveedor.DataValueField = "id";
                this.DropListOtroProveedor.DataTextField = "razonSocial";
                this.DropListOtroProveedor.DataBind();



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        public void cargarListaPrecio()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                DataTable dt = contCliente.obtenerListaPrecios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                //controles modalEtiquetas
                this.ListListaPrecio.DataSource = dt;
                this.ListListaPrecio.DataValueField = "id";
                this.ListListaPrecio.DataTextField = "nombre";
                this.ListListaPrecio.DataBind();

                //controles modalListaPrecios
                this.DropListListaPrecios.DataSource = dt;
                this.DropListListaPrecios.DataValueField = "id";
                this.DropListListaPrecios.DataTextField = "nombre";
                this.DropListListaPrecios.DataBind();

                foreach (DataRow lista in dt.Rows)
                {
                    if (lista["nombre"].ToString() != "Seleccione...")
                    {
                        ListItem item = new ListItem(lista["nombre"].ToString(), lista["id"].ToString());

                        this.chkListListas.Items.Add(item);
                        int i = this.chkListListas.Items.IndexOf(item);
                        this.chkListListas.Items[i].Selected = true;

                        //reporte stock no vendido
                        this.chkListListasNoVendido.Items.Add(item);
                        int j = this.chkListListasNoVendido.Items.IndexOf(item);
                        this.chkListListasNoVendido.Items[j].Selected = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }

        public void cargarListaCategoria()
        {
            try
            {
                DataTable dt = this.contListaPrecio.obtenerCategoriasSubListasPreciosDT();

                //agrego todos
                DataRow dr = dt.NewRow();

                if (dt.Rows.Count > 1)
                {
                    dr["categoria"] = "Todos";
                    dr["id"] = 0;
                    dt.Rows.InsertAt(dr, 0);
                }
                this.DropListCategoria.DataSource = dt;
                this.DropListCategoria.DataValueField = "id";
                this.DropListCategoria.DataTextField = "categoria";

                this.DropListCategoria.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando categoria. " + ex.Message));
            }
        }

        private void CargarProveedor()
        {
            try
            {
                DataTable dt = contCliente.obtenerProveedoresDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropListProveedor.DataSource = dt;
                DropListProveedor.DataValueField = "id";
                DropListProveedor.DataTextField = "alias";

                DropListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes. " + ex.Message));
            }
        }

        [WebMethod]
        public static string ObtenerProveedor(string proveedor)
        {
            controladorCliente controladorCliente = new controladorCliente();
            string buscar = proveedor.Replace(' ', '%');
            var dtProveedores = controladorCliente.obtenerProveedoresAliasDT(buscar);

            if (string.IsNullOrEmpty(buscar))
            {
                DataRow dr = dtProveedores.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dtProveedores.Rows.InsertAt(dr, 0);
            }

            List<ProveedorTemporal> proveedoresTemp = new List<ProveedorTemporal>();

            foreach (DataRow row in dtProveedores.Rows)
            {
                ProveedorTemporal proveedorTemporal = new ProveedorTemporal();
                proveedorTemporal.id = row["id"].ToString();
                proveedorTemporal.alias = row["alias"].ToString();
                proveedoresTemp.Add(proveedorTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(proveedoresTemp);
            return resultadoJSON;
        }

        class ProveedorTemporal
        {
            public string id;
            public string alias;
        }

        [WebMethod]
        public static string getArticulosFiltrados(int grupo, int subgrupo, int proveedor, int dias, int marca, string descSubGrupo, int soloProveedorPredeterminado, int lastPageId)
        {
            try
            {
                controladorArticulosNew contArticulo = new controladorArticulosNew();
                List<ArticulosClase> ArticulosFiltrados = new List<ArticulosClase>();
                string selectedDias = null;
                if (dias > 0)
                {
                    selectedDias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                DataTable dt = contArticulo.obtenerArticulosFiltrados(grupo, subgrupo, proveedor, selectedDias, marca, descSubGrupo, soloProveedorPredeterminado, lastPageId);
                foreach (DataRow Row in dt.Rows)
                {
                    ArticulosClase articulo = new ArticulosClase();
                    articulo.Id = Convert.ToInt32(Row["Id"]).ToString();
                    articulo.Codigo = Row["Codigo"].ToString();
                    articulo.Descripcion = Row["Descripcion"].ToString();
                    articulo.Grupo = Row["Grupo"].ToString();
                    articulo.SubGrupo = Row["SubGrupo"].ToString();
                    articulo.Marca = Row["Marca"].ToString();
                    articulo.UltimaActualizacion = Row["UltimaActualizacion"].ToString();
                    articulo.Proveedor = Row["Proveedor"].ToString();
                    articulo.PVenta = Convert.ToInt32(Row["PVenta"]).ToString();
                    articulo.ApareceLista = Convert.ToInt32(Row["ApareceLista"]).ToString();
                    ArticulosFiltrados.Add(articulo);
                }
                JavaScriptSerializer javaScript = new JavaScriptSerializer();
                javaScript.MaxJsonLength = 5000000;
                string resultadoJSON = javaScript.Serialize(ArticulosFiltrados);
                return resultadoJSON;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        [WebMethod]
        public static string buscarArticulo(string busqueda)
        {
            try
            {
                List<ArticulosClase> ArticulosBuscados = new List<ArticulosClase>();
                controladorArticulosNew contArticulo = new controladorArticulosNew();

                DataTable dt = contArticulo.buscarArticulos(busqueda.Replace(' ', '%'));

                foreach (DataRow Row in dt.Rows)
                {
                    ArticulosClase articulo = new ArticulosClase();
                    articulo.Id = Convert.ToInt32(Row["Id"]).ToString();
                    articulo.Codigo = Row["Codigo"].ToString();
                    articulo.Descripcion = Row["Descripcion"].ToString();
                    articulo.Grupo = Row["Grupo"].ToString();
                    articulo.SubGrupo = Row["SubGrupo"].ToString();
                    articulo.Marca = Row["Marca"].ToString();
                    articulo.UltimaActualizacion = Row["UltimaActualizacion"].ToString();
                    articulo.Proveedor = Row["Proveedor"].ToString();
                    articulo.PVenta = Convert.ToInt32(Row["PVenta"]).ToString();
                    articulo.ApareceLista = Convert.ToInt32(Row["ApareceLista"]).ToString();
                    ArticulosBuscados.Add(articulo);
                }

                JavaScriptSerializer javaScript = new JavaScriptSerializer();
                javaScript.MaxJsonLength = 5000000;
                string resultadoJSON = javaScript.Serialize(ArticulosBuscados);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [WebMethod]
        public static string getArticulosFiltradosPrevious(int grupo, int subgrupo, int proveedor, int dias, int marca, string descSubGrupo, int soloProveedorPredeterminado, int lastPageId)
        {
            try
            {
                controladorArticulosNew contArticulo = new controladorArticulosNew();
                List<ArticulosClase> ArticulosFiltrados = new List<ArticulosClase>();
                string selectedDias = null;
                if (dias > 0)
                {
                    selectedDias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                DataTable dt = contArticulo.obtenerArticulosFiltradosPrevious(grupo, subgrupo, proveedor, selectedDias, marca, descSubGrupo, soloProveedorPredeterminado, lastPageId);
                foreach (DataRow Row in dt.Rows)
                {
                    ArticulosClase articulo = new ArticulosClase();
                    articulo.Id = Convert.ToInt32(Row["Id"]).ToString();
                    articulo.Codigo = Row["Codigo"].ToString();
                    articulo.Descripcion = Row["Descripcion"].ToString();
                    articulo.Grupo = Row["Grupo"].ToString();
                    articulo.SubGrupo = Row["SubGrupo"].ToString();
                    articulo.Marca = Row["Marca"].ToString();
                    articulo.UltimaActualizacion = Row["UltimaActualizacion"].ToString();
                    articulo.Proveedor = Row["Proveedor"].ToString();
                    articulo.PVenta = Convert.ToInt32(Row["PVenta"]).ToString();
                    articulo.ApareceLista = Convert.ToInt32(Row["ApareceLista"]).ToString();
                    ArticulosFiltrados.Add(articulo);
                }
                JavaScriptSerializer javaScript = new JavaScriptSerializer();
                javaScript.MaxJsonLength = 5000000;
                string resultadoJSON = javaScript.Serialize(ArticulosFiltrados);
                return resultadoJSON;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        [WebMethod]
        public static string getVistaTablaArticulos()
        {
            VisualizacionArticulos vista = new VisualizacionArticulos();
            JavaScriptSerializer javaScript = new JavaScriptSerializer();
            javaScript.MaxJsonLength = 5000000;
            string resultadoJSON = javaScript.Serialize(vista);
            return resultadoJSON;
        }

        protected void btnUltimoDia_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos2_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos3_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos4_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos5_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos6_Click(object sender, EventArgs e)
        {

        }
        protected void btnUltimos7_Click(object sender, EventArgs e)
        {

        }
        protected void btnActualizarTodo_Click(object sender, EventArgs e)
        {

        }
        protected void btnSi_Click(object sender, EventArgs e)
        {

        }
        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {

        }

        protected void btnModificarPrecio_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    decimal porcentaje = Convert.ToDecimal(this.txtPorcentajeAumento.Text, CultureInfo.InvariantCulture);
            //    string noActu = "";
            //    foreach (var c in this.phArticulos.Controls)
            //    {
            //        TableRow tr = c as TableRow;
            //        string id = tr.ID.Split('_')[1];
            //        int i = this.contArticulo.aumentarPrecioPorcentaje(Convert.ToInt32(id), porcentaje);
            //        if (i <= 0)
            //        {
            //            //no se atualizo
            //            if (!String.IsNullOrEmpty(id))
            //            {
            //                Articulo art = this.contArticulo.obtenerArticuloByID(Convert.ToInt32(id));
            //                noActu += art.codigo + "; ";
            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(noActu))
            //    {
            //        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel11, UpdatePanel11.GetType(), "alert", "$.msgbox(\"Precios modificados con exito!\", {type: \"info\"});", true);
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Precios modificados con exito", null));
            //    }
            //    else
            //    {
            //        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel11, UpdatePanel11.GetType(), "alert", "$.msgbox(\"Los siguientes articulos no se actualizaron: " + noActu + "\" , {type: \"alert\"});", true);
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los siguientes articulos no se actualizaron. " + noActu));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando precios. " + ex.Message));
            //}
        }
        protected void btnSeteaPrecioventa_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    decimal precio = Convert.ToDecimal(this.txtPrecioVenta.Text, CultureInfo.InvariantCulture);
            //    string noActu = "";
            //    foreach (var c in this.phArticulos.Controls)
            //    {
            //        TableRow tr = c as TableRow;
            //        string id = tr.ID.Split('_')[1];
            //        int i = this.controlador.setearPrecioVenta(Convert.ToInt32(id), precio);
            //        if (i <= 0)
            //        {
            //            //no se atualizo
            //            if (!String.IsNullOrEmpty(id))
            //            {
            //                Articulo art = this.controlador.obtenerArticuloByID(Convert.ToInt32(id));
            //                noActu += art.codigo + "; ";
            //                //noActu += id + "; ";
            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(noActu))
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Precios modificados con exito", null));
            //    }
            //    else
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los siguientes articulos no se actualizaron. " + noActu));
            //    }
            //    //filtro
            //    if (Convert.ToInt32(this.hiddenAccion.Value) == 2)
            //    {
            //        this.filtrar(grupo, subgrupo, proveedor, dias, marca, descSubGrupo);
            //    }
            //    //busco
            //    if (Convert.ToInt32(this.hiddenAccion.Value) == 1)
            //    {
            //        this.buscar(this.textoBuscar);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando precios. " + ex.Message));
            //}
        }
        protected void lbtnBuscarProveedorDesdeActualizarProveedor_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnActualizarOtrosProveedores_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnActualizarArticulosDespacho_Click(object sender, EventArgs e)
        {

        }
        protected void btnInformeStock_Click(object sender, EventArgs e)
        {
            try
            {
                string inactivos = "0";
                if (this.CheckIncluirInactivos.Checked)
                    inactivos = "1";

                if (this.listSucursal.SelectedValue != "-1")
                {
                    string ceros = "1";
                    if (this.CheckIncluirCeros.Checked)
                    {
                        ceros = "0";
                    }
                    if (this.CheckBoxStockFaltante.Checked)
                    {
                        ceros = "-1";
                    }
                    if (this.CheckBoxUnicoSucursal.Checked)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=6&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&m=" + this.hiddenMarca.Value + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=1&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&m=" + this.hiddenMarca.Value + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }

                }
                else
                {
                    string ceros = "1";
                    if (this.CheckIncluirCeros.Checked)
                    {
                        ceros = "0";
                    }
                    if (this.CheckBoxStockFaltante.Checked)
                    {
                        ceros = "-1";
                    }
                    if (this.CheckBoxUnicoSucursal.Checked)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una sucursal para el informe de stock unico. "));
                    }
                    else
                    {
                        //Response.Redirect("ReporteAF.aspx?accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.ListGrupo.SelectedValue + "&sg=" + this.ListSubGrupo.SelectedValue + "&p=" + this.ListProveedor.SelectedValue + "&d=" + this.txtDiasActualizacion.Text + "&c=" + ceros);
                        //Response.Redirect("ReporteAF.aspx?accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&m=" + this.hiddenMarca.Value + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando informe de stock. " + ex.Message));
            }
        }
        protected void btnInformeStock2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listSucursal.SelectedValue != "-1")
                {
                    string ceros = "1";
                    if (this.CheckIncluirCeros.Checked)
                    {
                        ceros = "0";
                    }
                    if (this.CheckBoxStockFaltante.Checked)
                    {
                        ceros = "-1";
                    }
                    if (this.CheckBoxUnicoSucursal.Checked)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=6&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        Response.Redirect("ReporteAF.aspx?e=1&accion=6&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&c=" + ceros + "&m=" + this.hiddenMarca.Value);
                    }
                    else
                    {
                        Response.Redirect("ReporteAF.aspx?e=1&accion=1&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&c=" + ceros + "&m=" + this.hiddenMarca.Value);
                    }
                    //Response.Redirect("ReporteAF.aspx?e=1&accion=1&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros);
                }
                else
                {
                    string ceros = "1";
                    if (this.CheckIncluirCeros.Checked)
                    {
                        ceros = "0";
                    }
                    if (this.CheckBoxStockFaltante.Checked)
                    {
                        ceros = "-1";
                    }
                    if (this.CheckBoxUnicoSucursal.Checked)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una sucursal para el informe de stock unico. "));
                    }
                    else
                    {
                        Response.Redirect("ReporteAF.aspx?e=1&accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&c=" + ceros + "&m=" + this.hiddenMarca.Value);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    //Response.Redirect("ReporteAF.aspx?e=1&accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando informe de stock. " + ex.Message));
            }
        }
        protected void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            try
            {

                //filtro
                if (Convert.ToInt32(this.hiddenAccion.Value) == 2)
                {
                    Response.Redirect("ReporteAF.aspx?accion=3&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.hiddenMarca.Value);
                }
                //busco
                if (Convert.ToInt32(this.hiddenAccion.Value) == 1)
                {
                    Response.Redirect("ReporteAF.aspx?accion=4&txt=" + this.hiddenBuscar.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.hiddenMarca.Value);

                }
                //    //actualizaciones de precios
                //    if (this.accion == 3)
                //    {
                //        Response.Redirect("ReporteAF.aspx?accion=5&d=" + this.dias + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.marca);
                //    }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando informe de etiquetas. " + ex.Message));
            }
        }
        protected void btnImprimirListaPrecios_Click(object sender, EventArgs e)
        {
            try
            {
                int idListap = Convert.ToInt32(this.DropListListaPrecios.SelectedValue);

                int descuentoPorCantidad = 0;

                if (DescuentoPorCantidad.Checked == true)
                    descuentoPorCantidad = 1;

                if (this.chkUbicacion.Checked == true)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=1&l=" + DropListListaPrecios.SelectedValue + "&psi=" + Convert.ToInt32(PrecioSinIva.Checked) + "&dpc=" + descuentoPorCantidad + "&p=" + idProveedorHF.Value + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?l=" + DropListListaPrecios.SelectedValue + "&psi=" + Convert.ToInt32(PrecioSinIva.Checked) + "&dpc=" + Convert.ToInt32(DescuentoPorCantidad.Checked) + "&p=" + idProveedorHF.Value + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de precios. "));
            }
        }
        protected void btnImprimirListaPrecios2_Click(object sender, EventArgs e)
        {
            try
            {
                int idListap = Convert.ToInt32(this.DropListListaPrecios.SelectedValue);

                int descuentoPorCantidad = 0;

                if (DescuentoPorCantidad.Checked == true)
                    descuentoPorCantidad = 1;

                if (this.chkUbicacion.Checked == true)
                {
                    Response.Redirect("/Formularios/Articulos/ImpresionListaPrecios.aspx?ex=1&v=1&l=" + this.DropListListaPrecios.SelectedValue + "&psi=" + Convert.ToInt32(PrecioSinIva.Checked) + "&dpc=" + descuentoPorCantidad + "&p=" + idProveedorHF.Value);
                }
                else
                    Response.Redirect("/Formularios/Articulos/ImpresionListaPrecios.aspx?ex=1&l=" + this.DropListListaPrecios.SelectedValue + "&psi=" + Convert.ToInt32(PrecioSinIva.Checked) + "&dpc=" + descuentoPorCantidad + "&p=" + idProveedorHF.Value);
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de articulos. "));
            }
        }
        protected void lbtnStockAFecha_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.hiddenAccion.Value) == 2)//filtro
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=2&ex=0&f=1&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&m=" + this.hiddenMarca.Value + "&dsg=" + this.hiddenDescSubGrupo.Value + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }

                else//todos
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=2&ex=0&f=0&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnStockAFechaXLS_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.hiddenAccion.Value) == 2)//filtro
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=2&ex=1&f=1&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "&m=" + this.hiddenMarca.Value + "&dsg=" + this.hiddenDescSubGrupo.Value);
                }
                else
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=2&ex=1&f=0&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue);
                }
            }
            catch
            {

            }

        }
        protected void lbtnStockValorizado_Click(object sender, EventArgs e)
        {
            if (!cbStockDetallado.Checked)
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=3&ex=0&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=10&ex=0&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }
        protected void lbtnStockValorizadoXLS_Click(object sender, EventArgs e)
        {
            if (!cbStockDetallado.Checked)
                Response.Redirect("ImpresionMovStock.aspx?a=3&ex=1&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue);
            else
                Response.Redirect("ImpresionMovStock.aspx?a=10&ex=1&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue);
        }
        protected void lbtnProvRefBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtCodProveedor.Text);

                //cargo la lista                
                DataRow dr2 = dtProveedores.NewRow();
                dr2["razonSocial"] = "Todos";
                dr2["id"] = -1;
                dtProveedores.Rows.InsertAt(dr2, 0);

                this.DropListProvRef.DataSource = dtProveedores;
                this.DropListProvRef.DataValueField = "id";
                this.DropListProvRef.DataTextField = "razonSocial";

                this.DropListProvRef.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnStockDiasPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string listas = "";
                foreach (ListItem lista in chkListListas.Items)
                {
                    if (lista.Selected == true)
                    {
                        listas += lista.Value + ",";
                    }
                }
                int cero = Convert.ToInt32(this.chkDiasCero.Checked);

                if (listas != "")
                {
                    if (this.chkNoVendida.Checked)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=5&ex=0&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&c=" + this.DropListCategoria.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=4&ex=0&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&cero=" + cero + "&c=" + this.DropListCategoria.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"), true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnStockDiasXLS_Click(object sender, EventArgs e)
        {
            try
            {
                string listas = "";
                foreach (ListItem lista in chkListListas.Items)
                {
                    if (lista.Selected == true)
                    {
                        listas += lista.Value + ",";
                    }
                }

                int cero = Convert.ToInt32(this.chkDiasCero.Checked);

                if (listas != "")
                {
                    if (this.chkNoVendida.Checked)
                    {
                        Response.Redirect("ImpresionMovStock.aspx?a=5&ex=1&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&c=" + this.DropListCategoria.SelectedValue);
                    }
                    else
                    {
                        Response.Redirect("ImpresionMovStock.aspx?a=4&ex=1&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&cero=" + cero + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&c=" + this.DropListCategoria.SelectedValue);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"), true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnProvNoVendido_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtProvNoVendido.Text);

                //cargo la lista                
                DataRow dr2 = dtProveedores.NewRow();
                dr2["razonSocial"] = "Todos";
                dr2["id"] = -1;
                dtProveedores.Rows.InsertAt(dr2, 0);

                this.DropListProvNoVendido.DataSource = dtProveedores;
                this.DropListProvNoVendido.DataValueField = "id";
                this.DropListProvNoVendido.DataTextField = "razonSocial";

                this.DropListProvNoVendido.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnNoVendidaPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string listas = "";
                foreach (ListItem lista in chkListListasNoVendido.Items)
                {
                    if (lista.Selected == true)
                    {
                        listas += lista.Value + ",";
                    }
                }
                if (listas != "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=5&ex=0&listas=" + listas + "&fd=" + this.txtFechaDesdeNoVendido.Text + "&fh=" + this.txtFechaHastaNoVendido.Text + "&s=" + this.DropListSucNoVendido.SelectedValue + "&p=" + this.DropListProvNoVendido.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"), true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnNoVendidaXLS_Click(object sender, EventArgs e)
        {
            try
            {
                string listas = "";
                foreach (ListItem lista in chkListListasNoVendido.Items)
                {
                    if (lista.Selected == true)
                    {
                        listas += lista.Value + ",";
                    }
                }
                if (listas != "")
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=5&ex=1&listas=" + listas + "&s=" + this.DropListSucNoVendido.SelectedValue + "&fd=" + this.txtFechaDesdeNoVendido.Text + "&fh=" + this.txtFechaHastaNoVendido.Text + "&p=" + this.DropListProvNoVendido.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"), true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnImprimirUnicoCentral_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.hiddenAccion.Value) == 2)//filtro
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=6&ex=0&f=1&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }

                else//todos
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=6&ex=0&f=0&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnExportarUnicoCentral_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.hiddenAccion.Value) == 2)//filtro
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=6&ex=1&f=1&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue + "&g=" + this.hiddenGrupoValue.Value + "&sg=" + this.hiddenSubGrupoValue.Value + "&p=" + this.hiddenProveedor.Value + "&d=" + this.hiddenDiasUltimaActualizacion.Value);
                }
                else
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=6&ex=1&f=0&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue);
                }
            }
            catch
            {

            }
        }
        protected void lbtnNominaArticulosImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=9&ex=0&ai=" + Convert.ToInt32(this.chArtInactivos.Checked) + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
        protected void lbtnNominaArticulosExportar_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=9&ex=1&ai=" + Convert.ToInt32(this.chArtInactivos.Checked) + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
        protected void lbtnImprimirMovStock_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=7&ex=0&fd=" + this.txtFechaDesdeMovStock.Text + "&fh=" + this.txtFechaHastaMovStock.Text + "&s=" + this.ListSucursalMovStock.SelectedValue + "&movStk=" + this.ListTipoMovStock.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
        protected void lbtnExportarMovStock_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionMovStock.aspx?a=7&ex=1&fd=" + this.txtFechaDesdeMovStock.Text + "&fh=" + this.txtFechaHastaMovStock.Text + "&s=" + this.ListSucursalMovStock.SelectedValue + "&movStk=" + this.ListTipoMovStock.SelectedValue);
            }
            catch
            {
            }
        }
        protected void lbtnDesactualizados_Click(object sender, EventArgs e)
        {

            //// HAY QUE VER


            //try
            //{
            //    DateTime fecha = DateTime.Today.AddDays(Convert.ToInt32(this.txtDiasDesactualizado.Text) * -1);
            //    //List<Articulo> articulos = this.controlador.obtenerArticuloDesactualizadosByFecha(fecha);
            //    //this.cargarArticulosTabla(articulos);
            //    DataTable dt = this.contArticulo.obtenerArticuloDesactualizadosByFechaDT(fecha);
            //    this.cargarArticulosTablaDT(dt);
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos precios desactualizados. "));
            //}
        }
        protected void btnIEArticulosPdf_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=8&ex=0&fd=" + this.txtDesdeIEArticulos.Text + "&fh=" + this.txtHastaIEArticulos.Text + "&s=" + this.ListSucursalIEArticulos.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
        protected void btnIEArticulosExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionMovStock.aspx?a=8&ex=1&fd=" + this.txtDesdeIEArticulos.Text + "&fh=" + this.txtHastaIEArticulos.Text + "&s=" + this.ListSucursalIEArticulos.SelectedValue);
            }
            catch
            {
            }
        }
        protected void ListGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSubGruposArticulos(Convert.ToInt32(ListGrupo.SelectedValue));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando subgrupo" + ex.Message));
            }
        }
        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ListRazonSocial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class ArticulosClase
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        //public float cambioMoneda { get; set; }//--
        //public string monedaVenta { get; set; }--
        //public float precioVenta { get; set; }//--
        ////public float precioSinIva { get; set; }--
        ////public float porcentajeIva { get; set; }--
        public string Grupo { get; set; }
        public string SubGrupo { get; set; }
        public string Marca { get; set; }
        //public int grupo { get; set; }--
        //public int subGrupo { get; set; }--
        public string UltimaActualizacion { get; set; }
        public string Proveedor { get; set; }
        //public float costo { get; set; }--
        //public float costoReal { get; set; }--
        //public int SubLista { get; set; }--
        //public int proveedor { get; set; }--
        public string PVenta { get; set; }
        public string ApareceLista { get; set; }
    }

}