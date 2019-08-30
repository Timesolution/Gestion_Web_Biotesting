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

        Configuracion config = new Configuracion();
        
        Mensajes m = new Mensajes();
        List<Gestion_Api.Entitys.Promocione> listPromociones;
        int soloProveedorPredeterminado;
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
            catch(Exception ex)
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
                DataTable dt = contCliente.obtenerClientesDT();

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

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

        }
        protected void btnSeteaPrecioventa_Click(object sender, EventArgs e)
        {

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

        }
        protected void btnInformeStock2_Click(object sender, EventArgs e)
        {

        }
        protected void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {

        }
        protected void btnImprimirListaPrecios_Click(object sender, EventArgs e)
        {

        }
        protected void btnImprimirListaPrecios2_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockAFecha_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockAFechaXLS_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockValorizado_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockValorizadoXLS_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnProvRefBuscar_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockDiasPDF_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnStockDiasXLS_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnProvNoVendido_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnNoVendidaPDF_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnNoVendidaXLS_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnImprimirUnicoCentral_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnExportarUnicoCentral_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnNominaArticulosImprimir_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnNominaArticulosExportar_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnImprimirMovStock_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnExportarMovStock_Click(object sender, EventArgs e)
        {

        }
        protected void lbtnDesactualizados_Click(object sender, EventArgs e)
        {

        }
        protected void btnIEArticulosPdf_Click(object sender, EventArgs e)
        {

        }
        protected void btnIEArticulosExcel_Click(object sender, EventArgs e)
        {

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