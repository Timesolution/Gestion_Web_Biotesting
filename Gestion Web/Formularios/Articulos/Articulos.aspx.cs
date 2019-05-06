using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class Articulos : System.Web.UI.Page
    {
        private controladorListaPrecio controladorPrecio = new controladorListaPrecio();
        private controladorArticulo controlador = new controladorArticulo();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        private ControladorCobranzaEntity contCobranzaEntity = new ControladorCobranzaEntity();
        private controladorPais contPais = new controladorPais();
        private controladorListaPrecio contListaPrecio = new controladorListaPrecio();
        Mensajes m = new Mensajes();
        Configuracion config = new Configuracion();
        int accion;
        int listas;
        int grupo;
        int subgrupo;
        int marca;
        int dias;
        int idVendedor;
        int proveedor;
        string textoBuscar;
        string descSubGrupo;
        List<Gestion_Api.Entitys.Promocione> listPromociones;
        int desactualizados;
        int permisoEliminar = 0;
        int permisoStockValorizado = 0;//1 muestra costo, 0 muestra costo imponible
        int permisoMostrarBotonAgregarMateriasPrimas = 0;
        public Dictionary<string, string> camposArticulos = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnModificarPrecio.Attributes.Add("onclick", " this.disabled = true;  " + btnModificarPrecio.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnModificarPrecio, null) + ";");
                btnSeteaPrecioventa.Attributes.Add("onclick", " this.disabled = true;  " + btnSeteaPrecioventa.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnSeteaPrecioventa, null) + ";");

                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                //listas = 1, cambio precios segun la lista de precios seleccionada
                this.listas = Convert.ToInt32(Request.QueryString["l"]);
                this.grupo = Convert.ToInt32(Request.QueryString["g"]);
                this.subgrupo = Convert.ToInt32(Request.QueryString["sg"]);
                this.marca = Convert.ToInt32(Request.QueryString["m"]);
                this.textoBuscar = Request.QueryString["t"];
                this.dias = Convert.ToInt32(Request.QueryString["d"]);
                this.desactualizados = Convert.ToInt32(Request.QueryString["desact"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.proveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.descSubGrupo = Request.QueryString["dsg"];

                this.litNumero.Text = "(" + this.dias + ")";

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

                    //Obtengo todas las promociones
                    this.listPromociones = this.contArtEnt.obtenerPromociones();
                }
                //filtro
                if (this.accion == 2)
                {
                    this.filtrar(grupo, subgrupo, proveedor, dias, marca, descSubGrupo);
                }
                //busco
                if (this.accion == 1)
                {
                    this.buscar(this.textoBuscar);
                }
                //actualizaciones de precios
                if (this.accion == 3)
                {
                    if (this.desactualizados > 0)
                    {
                        cargarArticulosDesactualizadosPrecios(this.dias);
                    }
                    else
                    {
                        cargarArticulosActualizacionPrecios(this.dias);
                    }
                }
                //cargo defecto
                Configuracion c = new Configuracion();
                if (this.accion == 0)
                {
                    //List<Articulo> articulos = this.controlador.obtenerArticulosReduc();
                    DataTable articulos;
                    if (c.FiltroArticulosSucursal == "1")
                    {
                        int idSucursal = (int)Session["Login_SucUser"];
                        articulos = this.controlador.obtenerArticulosReducDT_Sucursales(idSucursal);
                    }
                    else
                    {
                        articulos = this.controlador.obtenerArticulosReducDT();
                    }
                    this.cargarArticulosTablaDT(articulos);
                }
                this.txtBusqueda.Focus();
                Page.Form.DefaultButton = this.lbBuscar.UniqueID;

                this.lblConfigCSV.Text = "*Archivo .CSV delimitado por ";
                if (c.separadorListas == "0")
                    this.lblConfigCSV.Text += "PuntoComa(;)";
                else
                    this.lblConfigCSV.Text += "Coma(,)";

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
                    string permiso2 = listPermisos.Where(x => x == "62").FirstOrDefault();
                    if (permiso2 != null)
                    {
                        this.phActualizacionPrecios.Visible = true;
                        this.permisoEliminar = 1;
                    }
                    string perfil = Session["Login_NombrePerfil"] as string;
                    //verifico si puede cambiar sucursal
                    string permisoCambioSuc = listPermisos.Where(x => x == "75").FirstOrDefault();
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

                //foreach (string s in listPermisos)
                //{
                //    if (!String.IsNullOrEmpty(s))
                //    {
                //        if (s == "14")
                //        {
                //            return 1;
                //        }
                //    }
                //}

                //return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarArticulosTablaDT(DataTable dt)
        {
            try
            {
                //vacio place holder
                this.phArticulos.Controls.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    this.cargarArticuloPH(row);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a tabla. " + ex.Message));
            }
        }

        private void cargarArticuloPH(DataRow row)
        {
            try
            {
                //Agrego las celdas que seleccione en la configuracion de visualizacion.                    
                VisualizacionArticulos vista = new VisualizacionArticulos();

                string oferta = "";
                var artStore = this.contArtEnt.obtenerOfertaArticuloParaFacturar(Convert.ToInt32(row["id"]));
                if (artStore != null)
                {
                    if (artStore.Oferta > 1 && DateTime.Today >= artStore.Desde && DateTime.Today <= artStore.Hasta)
                    {
                        oferta = "&nbsp <i class='icon-star'></i>";
                    }
                }

                Gestion_Api.Entitys.articulo artEnt = this.contArtEnt.obtenerArticuloEntity(Convert.ToInt32(row["id"]));

                //Verifico si el articulo está o no en promoción. Primero verifico que no esté en oferta, para no agregar dos veces el estilo. Uso el mismo string.
                if (string.IsNullOrEmpty(oferta))
                {
                    int idSuc = (int)Session["Login_SucUser"];
                    int idEmp = (int)Session["Login_EmpUser"];
                    int idArt = Convert.ToInt32(row["id"]);
                    var verificarPromo = this.contArtEnt.verificarPromocionArticulo(idArt, idSuc, idEmp);
                    if (verificarPromo)
                        oferta = "&nbsp <i class='icon-star'></i>";
                }

                string presen = "";
                string st = "";
                string marca = "";

                if (vista.columnaMarca == 1)
                {
                    var ma = artEnt.Articulos_Marca.FirstOrDefault();
                    if (ma != null)
                    {
                        marca = ma.Marca.marca1;
                    }
                }
                if (vista.columnaStock == 1)
                {
                    var stock = this.contArtEnt.obtenerStockArticuloLocal(Convert.ToInt32(row["id"]), (int)Session["Login_SucUser"]);
                    if (stock != null)
                    {
                        st = stock.stock1.Value.ToString();
                    }
                }
                if (vista.columnaPresentacion == 1)
                {
                    if (artEnt != null)
                    {
                        if (artEnt.Articulos_Presentaciones.Count > 0)
                        {
                            var p = artEnt.Articulos_Presentaciones.FirstOrDefault();
                            presen = p.Minima + "|" + p.Media + "|" + p.Maxima;
                        }
                    }
                }

                //Precio venta moneda original
                string precioVentaMonedaOriginal = string.Empty;
                if (vista.columnaPrecioVentaMonedaOriginal == 1)
                {
                    decimal precioVtaMonedaAux = Math.Round(Convert.ToDecimal(row["precioVenta"]) / Convert.ToDecimal(row["cambioMoneda"]), 2);
                    precioVentaMonedaOriginal = precioVtaMonedaAux.ToString("C");
                }


                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = row["codigo"].ToString() + oferta;
                celCodigo.Width = Unit.Percentage(5);
                celCodigo.VerticalAlign = VerticalAlign.Middle;

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = row["descripcion"].ToString();
                celDescripcion.Width = Unit.Percentage(35);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;

                TableCell celProveedor = new TableCell();
                celProveedor.Text = row["razonSocial"].ToString();
                celProveedor.Width = Unit.Percentage(10);
                celProveedor.VerticalAlign = VerticalAlign.Middle;

                TableCell celGrupo = new TableCell();
                celGrupo.Text = row["descGr"].ToString();
                celGrupo.Width = Unit.Percentage(10);
                celGrupo.VerticalAlign = VerticalAlign.Middle;

                TableCell celSubGrupo = new TableCell();
                celSubGrupo.Text = row["descSG"].ToString();
                celSubGrupo.Width = Unit.Percentage(10);
                celSubGrupo.VerticalAlign = VerticalAlign.Middle;

                TableCell celPresentacion = new TableCell();
                celPresentacion.Text = presen;
                celPresentacion.Width = Unit.Percentage(10);
                celPresentacion.VerticalAlign = VerticalAlign.Middle;

                TableCell celMarca = new TableCell();
                celMarca.Text = marca;
                celMarca.Width = Unit.Percentage(10);
                celMarca.VerticalAlign = VerticalAlign.Middle;

                TableCell celStock = new TableCell();
                celStock.Text = st;
                celStock.Width = Unit.Percentage(10);
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;

                TableCell celMoneda = new TableCell();
                celMoneda.Text = row["monedaVenta"].ToString();
                celMoneda.Width = Unit.Percentage(5);
                celMoneda.VerticalAlign = VerticalAlign.Middle;

                TableCell celUltimaActualizacion = new TableCell();
                celUltimaActualizacion.Text = Convert.ToDateTime(row["ultimaActualizacion"]).ToString("dd/MM/yyyy");
                celUltimaActualizacion.Width = Unit.Percentage(5);
                celUltimaActualizacion.VerticalAlign = VerticalAlign.Middle;

                TableCell celPrecioSIva = new TableCell();
                celPrecioSIva.Text = Convert.ToDecimal(row["precioSinIva"]).ToString("C");
                celPrecioSIva.Width = Unit.Percentage(5);
                celPrecioSIva.VerticalAlign = VerticalAlign.Middle;
                celPrecioSIva.HorizontalAlign = HorizontalAlign.Right;

                TableCell celPrecioVentaMonedaOriginal = new TableCell();
                celPrecioVentaMonedaOriginal.Text = precioVentaMonedaOriginal;
                celPrecioVentaMonedaOriginal.Width = Unit.Percentage(5);
                celPrecioVentaMonedaOriginal.VerticalAlign = VerticalAlign.Middle;
                celPrecioVentaMonedaOriginal.HorizontalAlign = HorizontalAlign.Right;

                TableCell celPrecio = new TableCell();
                celPrecio.Text = Convert.ToDecimal(row["precioVenta"]).ToString("C");
                celPrecio.Width = Unit.Percentage(5);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(20);

                Literal lDetail = new Literal();
                lDetail.ID = row["id"].ToString();

                lDetail.Text = "<a href=\"ArticulosABM.aspx?accion=2&id=" + row["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >";
                //lDetail.Text += "style=\"width: 100%\">";
                lDetail.Text += "<i class=\"shortcut-icon icon-search\"></i>";
                lDetail.Text += "</a>";
                celAction.Controls.Add(lDetail);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);

                LinkButton btnStock = new LinkButton();
                btnStock.ID = "btnStock_" + row["id"].ToString();
                btnStock.CssClass = "btn btn-info ui-tooltip";
                btnStock.Attributes.Add("data-toggle", "tooltip");
                btnStock.Attributes.Add("title data-original-title", "Stock");
                btnStock.Text = "<span class='shortcut-icon icon-list-alt'></span>";
                btnStock.PostBackUrl = "StockF.aspx?articulo=" + row["id"].ToString();
                celAction.Controls.Add(btnStock);

                //para que muestre el boton de agregar materia prima
                if (this.permisoMostrarBotonAgregarMateriasPrimas == 1)
                {
                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAction.Controls.Add(l2);

                    LinkButton btnComposicionMateriasPrimas = new LinkButton();
                    btnComposicionMateriasPrimas.ID = "btnComposicionMateriasPrimas_" + row["id"].ToString();
                    btnComposicionMateriasPrimas.CssClass = "btn btn-info ui-tooltip";
                    btnComposicionMateriasPrimas.Attributes.Add("data-toggle", "tooltip");
                    btnComposicionMateriasPrimas.Attributes.Add("title data-original-title", "Composicion");
                    btnComposicionMateriasPrimas.Text = "<span class='shortcut-icon icon-dropbox'></span>";
                    btnComposicionMateriasPrimas.PostBackUrl = "../MateriasPrimas/MateriasPrimas_Composicion.aspx?idArt=" + row["id"].ToString();
                    celAction.Controls.Add(btnComposicionMateriasPrimas);
                }

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAction.Controls.Add(l3);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + row["id"].ToString();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";

                btnEliminar.OnClientClick = "abrirdialog(" + row["id"].ToString() + ");";
                if (this.permisoEliminar == 0)
                {
                    btnEliminar.Visible = false;
                }

                celAction.Controls.Add(btnEliminar);


                TableRow tr = new TableRow();
                //tr.ID = art.id + "1";
                tr.ID = "tr_" + row["id"].ToString();
                if (Convert.ToInt32(row["apareceLista"]) == 0)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }

                //arego fila a tabla
                //table.Controls.Add(tr);
                tr.Cells.Add(celCodigo);
                tr.Cells.Add(celDescripcion);

                if (vista.columnaProveedores == 1)
                {
                    tr.Cells.Add(celProveedor);
                    phColumna1.Visible = true;
                }
                if (vista.columnaGrupo == 1)
                {
                    tr.Cells.Add(celGrupo);
                    phColumna2.Visible = true;
                }
                if (vista.columnaSubGrupo == 1)
                {
                    tr.Cells.Add(celSubGrupo);
                    phColumna3.Visible = true;
                }
                if (vista.columnaPresentacion == 1)
                {
                    tr.Cells.Add(celPresentacion);
                    phColumna7.Visible = true;
                }
                if (vista.columnaStock == 1)
                {
                    tr.Cells.Add(celStock);
                    phColumna8.Visible = true;
                }
                if (vista.columnaMarca == 1)
                {
                    tr.Cells.Add(celMarca);
                    phColumna9.Visible = true;
                }
                if (vista.columnaMoneda == 1)
                {
                    if (config.monedaArticulos == "1")//0=en pesos, 1 = en dolar/euro/lo que sea
                    {
                        if (Convert.ToDecimal(row["cambioMoneda"]) > 0)
                        {
                            celPrecio.Text = decimal.Round(Convert.ToDecimal(row["precioVenta"]) / Convert.ToDecimal(row["cambioMoneda"]), 4).ToString("C");
                            celPrecioSIva.Text = decimal.Round(Convert.ToDecimal(row["precioSinIva"]) / Convert.ToDecimal(row["cambioMoneda"]), 4).ToString("C");
                        }
                    }
                    tr.Cells.Add(celMoneda);
                    phColumna4.Visible = true;
                }
                if (vista.columnaActualizacion == 1)
                {
                    tr.Cells.Add(celUltimaActualizacion);
                    phColumna5.Visible = true;
                }
                if (vista.columnaPrecioVentaMonedaOriginal == 1)
                {
                    tr.Cells.Add(celPrecioVentaMonedaOriginal);
                    phColumna10.Visible = true;
                }

                if (config.precioArticulo.Contains("Con") == true)
                {
                    tr.Cells.Add(celPrecio);
                }
                else
                {
                    tr.Cells.Add(celPrecioSIva);
                }
                tr.Cells.Add(celAction);

                if (!String.IsNullOrEmpty(oferta))
                {
                    this.LitReferencia.Visible = true;
                    tr.ForeColor = System.Drawing.Color.ForestGreen;
                }


                this.phArticulos.Controls.Add(tr);
            }
            catch (Exception ex)
            {

            }
        }

        private void buscar(string busqueda)
        {
            try
            {
                //List<Articulo> articulos = new List<Articulo>();
                //articulos = this.controlador.buscarArticuloList(art);
                int idSucursal = (int)Session["Login_SucUser"];
                this.LitFiltro.Text = "Articulo " + busqueda;
                DataTable articulos;
                Configuracion configuracion = new Configuracion();
                if (configuracion.FiltroArticulosSucursal == "1")
                {
                    articulos = this.controlador.buscarArticulosDT_Sucursales(busqueda.Replace(' ', '%'), idSucursal);
                }
                else
                {
                    articulos = this.controlador.buscarArticulosDT(busqueda.Replace(' ', '%'));
                }
                this.cargarArticulosTablaDT(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void filtrar(int grupo, int subgrupo, int proveedor, int dias, int marca, string descSubGrupo)
        {
            try
            {
                controladorCliente contCli = new controladorCliente();
                string Sgrupo = this.ListGrupo.Items.FindByValue(grupo.ToString()).Text;
                string SSubgrupo = "";
                try
                {
                    SSubgrupo = this.controlador.obtenerSubGrupoID(subgrupo).descripcion;
                }
                catch { }


                string Sproveedor = "";
                try
                {
                    //proveedor                
                    Sproveedor = contCli.obtenerProveedorID(proveedor).alias;//this.ListProveedor.Items.FindByValue(proveedor.ToString()).Text;
                }
                catch { }

                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                this.LitFiltro.Text = "Filtros: " + Sgrupo + ", " + SSubgrupo + ", " + Sproveedor;

                //List<Articulo> articulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor, sdias);
                //this.cargarArticulosTabla(articulos);
                Configuracion configuracion = new Configuracion();
                DataTable dt = this.controlador.filtrarArticulosGrupoSubGrupoDT(grupo, subgrupo, proveedor, sdias, marca, descSubGrupo);
                if (configuracion.FiltroArticulosSucursal == "1")
                {
                    int idSucursal = (int)Session["Login_SucUser"];
                    dt = this.controlador.filtrarArticulosGrupoSubGrupoDT_Sucursales(grupo, subgrupo, proveedor, sdias, marca, descSubGrupo, idSucursal);
                }
                this.cargarArticulosTablaDT(dt);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void ListRazonSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListProveedor.SelectedValue = this.ListRazonSocial.SelectedValue;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error seleccionando valor en cliente. " + ex.Message));
            }
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListRazonSocial.SelectedValue = this.ListProveedor.SelectedValue;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error seleccionando valor en razon social. " + ex.Message));
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

        #region cargas iniciales
        private void cargarGruposArticulos()
        {
            try
            {
                DataTable dt = controlador.obtenerGruposArticulos();

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
        private void cargarSubGruposArticulos(int sGrupo)
        {
            try
            {
                if (sGrupo == 0)
                {
                    DataTable dt = controlador.obtenerSubGruposDistinctDT();


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
                    DataTable dt = controlador.obtenerSubGruposArticulos(sGrupo);

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
                DataTable dt = controlador.obtenerMarcasDT();

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
        #endregion

        #region busquedas actualizacion precios
        private void cargarArticulosActualizacionPrecios(int dias)
        {
            try
            {
                DateTime fecha = DateTime.Today.AddDays(dias * -1);
                //List<Articulo> articulos = this.controlador.obtenerArticuloByFechaActualizacion(fecha);
                //this.cargarArticulosTabla(articulos);
                DataTable dt = this.controlador.obtenerArticuloByFechaActualizacionDT(fecha);
                this.cargarArticulosTablaDT(dt);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos modificacion precios articulos. "));
            }
        }
        private void cargarArticulosDesactualizadosPrecios(int dias)
        {
            try
            {
                DateTime fecha = DateTime.Today.AddDays(dias * -1);
                //List<Articulo> articulos = this.controlador.obtenerArticuloDesactualizadosByFecha(fecha);
                //this.cargarArticulosTabla(articulos);
                DataTable dt = this.controlador.obtenerArticuloDesactualizadosByFechaDT(fecha);
                this.cargarArticulosTablaDT(dt);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos precios desactualizados. "));
            }
        }
        protected void btnUltimoDia_Click(object sender, EventArgs e)
        {
            Response.Redirect("Articulos.aspx?accion=3&d=1");
        }
        protected void btnUltimos2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Articulos.aspx?accion=3&d=2");
        }
        protected void btnUltimos3_Click(object sender, EventArgs e)
        {
            Response.Redirect("Articulos.aspx?accion=3&d=3");
        }
        protected void btnUltimos4_Click(object sender, EventArgs e)
        {
            Response.Redirect("Articulos.aspx?accion=3&d=4");
        }
        protected void btnUltimos5_Click(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID.Split('_')[1];
                Response.Redirect("Articulos.aspx?accion=3&d=" + id);
            }
            catch
            {

            }
        }
        protected void lbtnDesactualizados_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Articulos.aspx?accion=3&desact=1&d=" + this.txtDiasDesactualizado.Text);
            }
            catch
            {

            }
        }


        #endregion 

        #region informes
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=6&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=1&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ReporteAF.aspx?accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&c=" + ceros + "&i=" + inactivos + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
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
                        Response.Redirect("ReporteAF.aspx?e=1&accion=6&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros + "&m=" + this.marca);
                    }
                    else
                    {
                        Response.Redirect("ReporteAF.aspx?e=1&accion=1&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros + "&m=" + this.marca);
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
                        Response.Redirect("ReporteAF.aspx?e=1&accion=2&s=" + this.listSucursal.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&c=" + ceros + "&m=" + this.marca);
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
                if (this.accion == 2)
                {
                    Response.Redirect("ReporteAF.aspx?accion=3&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.marca);
                }
                //busco
                if (this.accion == 1)
                {
                    Response.Redirect("ReporteAF.aspx?accion=4&txt=" + this.textoBuscar + "&d=" + this.dias + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.marca);

                }
                //actualizaciones de precios
                if (this.accion == 3)
                {
                    Response.Redirect("ReporteAF.aspx?accion=5&d=" + this.dias + "&l=" + this.ListListaPrecio.SelectedValue + "&t=" + this.ListEtiqueta.SelectedValue + "&s=" + this.ListSucursalEtiquetas.SelectedValue + "&cero=" + Convert.ToInt32(this.StockCero.Checked) + "&m=" + this.marca);
                }
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
                if (this.chkDescuentoCantidad.Checked == true)
                    descuentoPorCantidad = 1;

                int iva = 0;
                if (this.RadioSinIva.Checked == true)
                    iva = 1;
                else
                    iva = 2;

                if (this.chkUbicacion.Checked == true)
                {
                    if (accion == 2)//si se filtro
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=1&a=1&iva=" + iva + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        if (accion == 1)// por busqueda
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=1&a=3&iva=" + iva + "&dc=" + descuentoPorCantidad + "&t=" + this.textoBuscar + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }

                        else//default
                        {
                            if (accion == 3)// por fecha actualizacion
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=1&a=4&d=" + this.dias + "&desact" + this.desactualizados + "&dc=" + descuentoPorCantidad + "&iva=" + iva + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=1&a=2&iva=" + iva + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                        }
                    }
                }
                else
                {
                    if (accion == 2)//si se filtro
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=0&a=1&iva=" + iva + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        if (accion == 1)// por busqueda
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=0&a=3&iva=" + iva + "&t=" + this.textoBuscar + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        else//default
                        {
                            if (accion == 3)// por fecha actualizacion
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=0&a=4&iva=" + iva + "&d=" + this.dias + "&dc=" + descuentoPorCantidad + "&desact=" + this.desactualizados + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionListaPrecios.aspx?v=0&a=2&iva=" + iva + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                            }
                        }
                    }
                }

            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de articulos. "));
            }

        }

        protected void btnImprimirListaPrecios2_Click(object sender, EventArgs e)
        {
            try
            {
                int idListap = Convert.ToInt32(this.DropListListaPrecios.SelectedValue);

                int descuentoPorCantidad = 0;
                if (this.chkDescuentoCantidad.Checked == true)
                    descuentoPorCantidad = 1;

                int iva = 0;
                if (this.RadioSinIva.Checked == true)
                {
                    iva = 1;
                }
                else
                {
                    iva = 2;
                }

                if (this.chkUbicacion.Checked == true)
                {
                    if (accion == 2)//si se filtro
                    {
                        Response.Redirect("ImpresionListaPrecios.aspx?v=1&ex=1&a=1&iva=" + iva + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&dc=" + descuentoPorCantidad + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&l=" + this.DropListListaPrecios.SelectedValue);
                    }
                    else
                    {
                        if (accion == 1)// por busqueda
                        {
                            Response.Redirect("ImpresionListaPrecios.aspx?v=1&ex=1&a=3&iva=" + iva + "&t=" + this.textoBuscar + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue);
                        }
                        else//default
                        {
                            if (accion == 3)
                            {
                                Response.Redirect("ImpresionListaPrecios.aspx?v=1&ex=1&a=4&iva=" + iva + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&dc=" + descuentoPorCantidad + "&desact=" + this.desactualizados + "&l=" + this.DropListListaPrecios.SelectedValue);
                            }
                            else
                            {
                                Response.Redirect("ImpresionListaPrecios.aspx?v=1&ex=1&a=2&iva=" + iva + "&dc=" + descuentoPorCantidad + "&l=" + this.DropListListaPrecios.SelectedValue);
                            }
                        }
                    }
                }
                else
                {
                    if (accion == 2)//si se filtro
                    {
                        Response.Redirect("ImpresionListaPrecios.aspx?ex=1&a=1&iva=" + iva + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&dc=" + descuentoPorCantidad + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&l=" + this.DropListListaPrecios.SelectedValue);
                    }
                    else
                    {
                        if (accion == 1)// por busqueda
                        {
                            Response.Redirect("ImpresionListaPrecios.aspx?ex=1&a=3&iva=" + iva + "&t=" + this.textoBuscar + "&l=" + this.DropListListaPrecios.SelectedValue + "&dc=" + descuentoPorCantidad);
                        }
                        else//default
                        {
                            if (accion == 3)
                            {
                                Response.Redirect("ImpresionListaPrecios.aspx?ex=1&a=4&iva=" + iva + "&d=" + this.dias + "&desact=" + this.desactualizados + "&l=" + this.DropListListaPrecios.SelectedValue + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "&dc=" + descuentoPorCantidad);
                            }
                            else
                            {
                                Response.Redirect("ImpresionListaPrecios.aspx?ex=1&a=2&iva=" + iva + "&l=" + this.DropListListaPrecios.SelectedValue + "&dc=" + descuentoPorCantidad);
                            }
                        }
                    }
                }
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
                if (this.accion == 2)//filtro
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=2&ex=0&f=1&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
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
            if (this.accion == 2)//filtro
            {
                Response.Redirect("ImpresionMovStock.aspx?a=2&ex=1&f=1&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "&m=" + this.marca + "&dsg=" + this.descSubGrupo);
            }
            else
            {
                Response.Redirect("ImpresionMovStock.aspx?a=2&ex=1&f=0&fh=" + this.txtFechaHasta_St.Text + "&s=" + this.DropListSucursal_St.SelectedValue);
            }
        }

        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtCodProveedor.Text);

                //cargo la lista
                this.ListProveedor.DataSource = dtProveedores;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();

                DataRow dr2 = dtProveedores.NewRow();
                dr2["razonSocial"] = "Todos";
                dr2["id"] = -1;
                dtProveedores.Rows.InsertAt(dr2, 0);

                this.ListRazonSocial.DataSource = dtProveedores;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";

                this.ListRazonSocial.DataBind();

                this.ListRazonSocial.SelectedValue = this.ListProveedor.SelectedValue;

            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnStockValorizado_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=3&ex=0&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnStockValorizadoXLS_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImpresionMovStock.aspx?a=3&ex=1&costo=" + this.permisoStockValorizado + "&s=" + this.DropListSucursal_St2.SelectedValue);
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=5&ex=0&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&c=" + this.DropListCategoria.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=4&ex=0&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&cero=" + cero + "&c=" + this.DropListCategoria.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
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
                        Response.Redirect("ImpresionMovStock.aspx?a=5&ex=1&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&c=" + this.DropListCategoria.SelectedValue);
                    }
                    else
                    {
                        Response.Redirect("ImpresionMovStock.aspx?a=4&ex=1&listas=" + listas + "&fd=" + this.txtFechaRefDesde.Text + "&fh=" + this.txtFechaRefHasta.Text + "&s=" + this.DropListSucursalRef.SelectedValue + "&p=" + this.DropListProvRef.SelectedValue + "&d=" + this.txtDias.Text + "&cero=" + cero + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&c=" + this.DropListCategoria.SelectedValue);
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

        protected void lbtnActualizarOtrosProveedores_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;

                String path = Server.MapPath("../../OtrosProveedores/");//Liquidacion_" + DateTime.Today.Month + "-" + DateTime.Today.Day + "/");

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
                    //guardo nombre archivo
                    string archivo = FileUpload1.FileName;
                    //lo subo
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);

                    DataTable dtProveedoresArticulos = this.controlador.obtenerProveedorArticulosByProveedorDT(Convert.ToInt32(DropListOtroProveedor.SelectedValue));
                    int cantRegistros = 0;

                    if (dtProveedoresArticulos != null)
                    {
                        cantRegistros = dtProveedoresArticulos.Rows.Count;
                    }
                    int i = this.controlador.actualizarPrecioProveedoresXLS(path + archivo, Convert.ToInt32(DropListOtroProveedor.SelectedValue));

                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito. Actualizados: " + i + " de " + cantRegistros, Request.Url.ToString()));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron procesar una o mas articulos."));
                    }

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar un archivo '.csv'!. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando precios Proveedores desde archivo. "));
            }
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

        protected void btnActualizarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                List<Articulo> lstArticulos = this.controlador.obtenerArticulosReduc2();
                foreach (Articulo art in lstArticulos)
                {
                    //recalculo
                    Articulo nuevo = this.controlador.obtenerPrecioVentaDesdeVenta(art, art.precioVenta);
                    //actualizo fecha
                    nuevo.ultActualizacion = DateTime.Now;

                    //guardo
                    int i = this.controlador.modificarArticulo(nuevo, nuevo.codigo, 1);
                }

            }
            catch
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

        protected void lbtnExportarUnicoCentral_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)//filtro
                {
                    Response.Redirect("ImpresionMovStock.aspx?a=6&ex=1&f=1&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias);
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

        protected void lbtnImprimirUnicoCentral_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)//filtro
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Articulos/ImpresionMovStock.aspx?a=6&ex=0&f=1&sCentral=" + this.ListSucursalCentral.SelectedValue + "&sCompara=" + this.ListSucursalComparar.SelectedValue + "&g=" + this.grupo + "&sg=" + this.subgrupo + "&p=" + this.proveedor + "&d=" + this.dias + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
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

        #endregion

        #region despacho
        protected void lbtnActualizarArticulosDespacho_Click(object sender, EventArgs e)
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Inicio procesar actualizacion articulos despacho excel");
                Boolean fileOK = false;

                String path = Server.MapPath("../../content/excelFiles/artDespacho/");
                String fileExtension = "";
                if (FileUploadArticulosDespacho.HasFile)
                {
                    fileExtension = Path.GetExtension(FileUploadArticulosDespacho.FileName).ToLower();

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
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "No existe directorio. " + path + ". lo creo");

                        Directory.CreateDirectory(path);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "directorio creado");
                    }
                    //guardo nombre archivo
                    string nombreArchivoExcel = FileUploadArticulosDespacho.FileName;

                    //lo subo
                    FileUploadArticulosDespacho.PostedFile.SaveAs(path + FileUploadArticulosDespacho.FileName);

                    Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Voy a traer datos desde el excel " + path + FileUploadArticulosDespacho.FileName);
                    var artDespachoExcel = new ArtDespachoExcel();
                    var tablaDatos = artDespachoExcel.traerDatos(path + FileUploadArticulosDespacho.FileName);

                    if (tablaDatos != null)
                    {
                        int contador = 0;
                        //recorro los articulos del excel
                        foreach (var item in tablaDatos)
                        {
                            if (!string.IsNullOrWhiteSpace(item.Codigo))
                            {
                                int i = this.actualizarDatosDelArticuloDespacho(item);
                                if (i >= 1)
                                {
                                    contador++;
                                }
                            }
                        }
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se actualizaron " + contador + " articulos.", null));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Verificar codigos y datos del excel."));
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

        private int actualizarDatosDelArticuloDespacho(ArtDespachoExcel artDespachoExcel)
        {
            try
            {
                Gestion_Api.Entitys.articulo artEntity = this.contArtEnt.obtenerArticuloEntityByCod(artDespachoExcel.Codigo);
                Articulos_Despachos datosDespacho = new Articulos_Despachos();
                //datosDespacho.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                //datosDespacho.Lote = this.txtLote.Text;
                //datosDespacho.Vencimiento = this.txtVencimiento.Text;
                datosDespacho.NumeroDespacho = artDespachoExcel.NumeroDespacho;
                datosDespacho.IdArticulo = artEntity.id;
                datosDespacho.FechaDespacho = DateTime.Now;

                var listaPaises = this.contPais.obtenerPaisList();
                var paisEncontrado = listaPaises.Where(x => x.descripcion.ToLower() == artDespachoExcel.Procedencia.ToLower()).FirstOrDefault();
                if (paisEncontrado != null)
                {
                    artEntity.procedencia = paisEncontrado.id;
                }
                int i = this.contArtEnt.actualizarCostoYPrecioVentaYRecalcularlo(artEntity.id, Convert.ToDecimal(artDespachoExcel.Costo.Replace(",", ".")), Convert.ToDecimal(artDespachoExcel.PrecioVenta.Replace(",", ".")), artDespachoExcel.Moneda);
                i += this.contArtEnt.crearOActualizarDatosDespachoArticulo(datosDespacho);
                if (i == 2)
                {
                    return i;
                }
                return -1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error en fun: actualizarDatosDelArticuloDespacho. " + ex.Message));
                return 0;
            }

            ////ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
            //Gestion_Api.Entitys.articulo artEntity = contArtEnt.obtenerArticuloEntity(idArticulo);
            //if (artEntity.Articulos_Despachos.Count > 0)
            //{
            //    artEntity.Articulos_Despachos.FirstOrDefault().FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
            //    artEntity.Articulos_Despachos.FirstOrDefault().NumeroDespacho = this.txtNumeroDespacho.Text;
            //    artEntity.Articulos_Despachos.FirstOrDefault().Lote = this.txtLote.Text;
            //    artEntity.Articulos_Despachos.FirstOrDefault().Vencimiento = this.txtVencimiento.Text;
            //}
            //else
            //{
            //    Articulos_Despachos datosDespacho = new Articulos_Despachos();
            //    datosDespacho.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
            //    datosDespacho.NumeroDespacho = this.txtNumeroDespacho.Text;
            //    datosDespacho.Lote = this.txtLote.Text;
            //    datosDespacho.Vencimiento = this.txtVencimiento.Text;
            //    artEntity.Articulos_Despachos.Add(datosDespacho);
            //}
            //contArtEnt.guardarDatosDespacho(artEntity);

        }

        //private int actualizarPrecioDelArticuloByMoneda(ArtDespachoExcel artDespachoExcel)
        //{
        //    try
        //    {
        //        decimal nuevoPrecioVenta = this.contCobranzaEntity.obtenerCotizacionPorNombreMoneda(artDespachoExcel.Moneda);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion

        #region eventos botones
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
        protected void lbtnBuscarProveedorDesdeActualizarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtBuscarProveedor.Text);

                //cargo la lista
                this.DropListOtroProveedor.DataSource = dtProveedores;
                this.DropListOtroProveedor.DataValueField = "id";
                this.DropListOtroProveedor.DataTextField = "alias";

                this.DropListOtroProveedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pueden cargar los proveedores. Excepción: " + ex.Message), true);
            }

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    Response.Redirect("Articulos.aspx?accion=1&t=" + this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idArticulo = Convert.ToInt32(this.txtMovimiento.Text);
                Articulo art = this.controlador.obtenerArticuloByID(idArticulo);
                art.estado = 0;
                int i = this.controlador.eliminarArticulo(art);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Articulo: " + art.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo eliminado con exito", null));
                }
                if (i == -2)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede eliminar articulo ya que posee pedidos pendientes"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Articulo"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Articulo. " + ex.Message));
            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Articulos.aspx?accion=2&g=" + this.ListGrupo.SelectedValue
                    + "&sg=" + this.ListSubGrupo.SelectedValue
                    + "&p=" + this.ListProveedor.SelectedValue
                    + "&m=" + this.ListMarca.SelectedValue
                    + "&dsg=" + this.ListSubGrupo.SelectedItem.Text
                    + "&d=" + this.txtDiasActualizacion.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error filtrando articulos. " + ex.Message));
            }
        }
        protected void btnModificarPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                decimal porcentaje = Convert.ToDecimal(this.txtPorcentajeAumento.Text, CultureInfo.InvariantCulture);
                string noActu = "";
                foreach (var c in this.phArticulos.Controls)
                {
                    TableRow tr = c as TableRow;
                    string id = tr.ID.Split('_')[1];
                    int i = this.controlador.aumentarPrecioPorcentaje(Convert.ToInt32(id), porcentaje);
                    if (i <= 0)
                    {
                        //no se atualizo
                        if (!String.IsNullOrEmpty(id))
                        {
                            Articulo art = this.controlador.obtenerArticuloByID(Convert.ToInt32(id));
                            noActu += art.codigo + "; ";
                        }
                    }
                }
                if (string.IsNullOrEmpty(noActu))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel11, UpdatePanel11.GetType(), "alert", "$.msgbox(\"Precios modificados con exito!\", {type: \"info\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Precios modificados con exito", null));
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel11, UpdatePanel11.GetType(), "alert", "$.msgbox(\"Los siguientes articulos no se actualizaron: " + noActu + "\" , {type: \"alert\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los siguientes articulos no se actualizaron. " + noActu));
                }
                //filtro
                if (this.accion == 2)
                {
                    this.filtrar(grupo, subgrupo, proveedor, dias, marca, descSubGrupo);
                }
                //busco
                if (this.accion == 1)
                {
                    this.buscar(this.textoBuscar);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando precios. " + ex.Message));
            }
        }
        protected void btnSeteaPrecioventa_Click(object sender, EventArgs e)
        {
            try
            {
                decimal precio = Convert.ToDecimal(this.txtPrecioVenta.Text, CultureInfo.InvariantCulture);
                string noActu = "";
                foreach (var c in this.phArticulos.Controls)
                {
                    TableRow tr = c as TableRow;
                    string id = tr.ID.Split('_')[1];
                    int i = this.controlador.setearPrecioVenta(Convert.ToInt32(id), precio);
                    if (i <= 0)
                    {
                        //no se atualizo
                        if (!String.IsNullOrEmpty(id))
                        {
                            Articulo art = this.controlador.obtenerArticuloByID(Convert.ToInt32(id));
                            noActu += art.codigo + "; ";
                            //noActu += id + "; ";
                        }
                    }
                }
                if (string.IsNullOrEmpty(noActu))
                {                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Precios modificados con exito", null));
                }
                else
                {                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los siguientes articulos no se actualizaron. " + noActu));
                }
                //filtro
                if (this.accion == 2)
                {
                    this.filtrar(grupo, subgrupo, proveedor, dias, marca, descSubGrupo);
                }
                //busco
                if (this.accion == 1)
                {
                    this.buscar(this.textoBuscar);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando precios. " + ex.Message));
            }
        }

        #endregion

        //protected void btnModificarPrecio2_Click(object sender, EventArgs e)
        //{

        //    decimal porcentaje = Convert.ToDecimal(this.txtPorcentajeAumento.Text, CultureInfo.InvariantCulture);
        //    string noActu = "";
        //    foreach (var c in this.phArticulos.Controls)
        //    {
        //        TableRow tr = c as TableRow;
        //        string id = tr.ID.Split('_')[1];
        //        int i = this.controlador.aumentarPrecioPorcentaje(Convert.ToInt32(id), porcentaje);
        //        if (i <= 0)
        //        {
        //            //no se atualizo
        //            if (!String.IsNullOrEmpty(id))
        //            {
        //                Articulo art = this.controlador.obtenerArticuloByID(Convert.ToInt32(id));
        //                noActu += art.codigo + "; ";
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
        //    if (this.accion == 2)
        //    {
        //        this.filtrar(grupo, subgrupo, proveedor, dias, marca, descSubGrupo);
        //    }
        //    //busco
        //    if (this.accion == 1)
        //    {
        //        this.buscar(this.textoBuscar);
        //    }
        //}

    }
}
