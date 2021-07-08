using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Controladores.ControladoresEntity;
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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ArticulosMobile : System.Web.UI.Page
    {
        #region Variables y Controladores

        private controladorListaPrecio controladorPrecio = new controladorListaPrecio();
        private controladorArticulo controlador = new controladorArticulo();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        private ControladorCobranzaEntity contCobranzaEntity = new ControladorCobranzaEntity();
        private controladorPais contPais = new controladorPais();
        private controladorListaPrecio contListaPrecio = new controladorListaPrecio();
        controladorCliente _controladorCliente = new controladorCliente();
        ControladorEmpresa controladorEmpresa = new ControladorEmpresa();

        Mensajes m = new Mensajes();
        Configuracion config = new Configuracion();
        private int permisoEliminarArticulo = 0;
        int accion;
        int listas;
        int grupo;
        int subgrupo;
        int marca;
        int dias;
        int idVendedor;
        int proveedor;
        int soloProveedorPredeterminado;
        string textoBuscar;
        string descSubGrupo;
        List<Gestion_Api.Entitys.Promocione> listPromociones;
        int desactualizados;
        int permisoEliminar = 0;
        int permisoStockValorizado = 0;//1 muestra costo, 0 muestra costo imponible
        int permisoMostrarBotonAgregarMateriasPrimas = 0;
        public Dictionary<string, string> camposArticulos = null;

        #endregion

        #region Verificacion de Usuario

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
                    string permisoEliminarArticulo = listPermisos.Where(x => x == "242").FirstOrDefault();

                    if (permisoEliminarArticulo != null)
                    {
                        this.permisoEliminarArticulo = 1;
                    }

                    if (permiso2 != null)
                    {
                        this.permisoEliminar = 1;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                int idEmpresa = (int)Session["Login_SucUser"];
                Empresa empresa = controladorEmpresa.obtenerEmpresaByIdSucursal(idEmpresa);

                ///Chequeo si la empresa es parte de Deport Show, para habilitar la descarga del archivo .txt de los articulos para Magento


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
                this.soloProveedorPredeterminado = Convert.ToInt32(Request.QueryString["spp"]);

                // this.litNumero.Text = "(" + this.dias + ")";

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());

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


            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ocurrio un error en la seccion de articulos. Ubicacion: Articulos.aspx. Metodo: Page_Load. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
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




                TableRow tr = new TableRow();

                tr.ID = "tr_" + row["id"].ToString();
                if (Convert.ToInt32(row["apareceLista"]) == 0)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }

                //arego fila a tabla

                tr.Cells.Add(celCodigo);
                tr.Cells.Add(celDescripcion);

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
                    //tr.Cells.Add(celMoneda);
                    phColumna4.Visible = true;
                }

                //if (vista.columnaPrecioVentaMonedaOriginal == 1)
                //{
                //    tr.Cells.Add(celPrecioVentaMonedaOriginal);
                //    phColumna10.Visible = true;
                //}

                if (config.precioArticulo.Contains("Con") == true)
                {
                    tr.Cells.Add(celPrecio);
                }
                else
                {
                    tr.Cells.Add(celPrecioSIva);
                }

                if (!String.IsNullOrEmpty(oferta))
                {
                    tr.ForeColor = System.Drawing.Color.ForestGreen;
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

                int idSucursal = (int)Session["Login_SucUser"];
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


                Configuracion configuracion = new Configuracion();
                DataTable dt = this.controlador.filtrarArticulosGrupoSubGrupoDT(grupo, subgrupo, proveedor, sdias, marca, descSubGrupo, soloProveedorPredeterminado);
                if (configuracion.FiltroArticulosSucursal == "1" && soloProveedorPredeterminado == 0)
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


       


       
        #region busquedas actualizacion precios
        private void cargarArticulosActualizacionPrecios(int dias)
        {
            try
            {
                DateTime fecha = DateTime.Today.AddDays(dias * -1);
               
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
              
                DataTable dt = this.controlador.obtenerArticuloDesactualizadosByFechaDT(fecha);
                this.cargarArticulosTablaDT(dt);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos precios desactualizados. "));
            }
        }



        #endregion

        #region informes

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

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    Response.Redirect("ArticulosMobile.aspx?accion=1&t=" + this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }




    }
}

#endregion