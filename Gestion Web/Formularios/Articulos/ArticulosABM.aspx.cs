using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ArticulosABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        //controlador
        controladorArticulo controlador = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

        controladorListaPrecio contLista = new controladorListaPrecio();
        controladorUsuario contUser = new controladorUsuario();

        controladorCobranza contCobranza = new controladorCobranza();

        //articulo
        Articulo articulo = new Articulo();
        
        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private int id;
        private int EditarAc;
        private int idCompuesto;
        private int EditarPa;
        private int idProvArt;
        //idtemporario
        private int idTemp;
        //url filtro anterior
        string urlFiltro = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                  

                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.id = Convert.ToInt32(Request.QueryString["id"]);
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    //cargo combos
                    this.EditarAc = 0;
                    this.idCompuesto = 0;
                    Session.Add("ArticulosABM_EditarAc", EditarAc);
                    Session.Add("ArticulosABM_IdCompuesto", idCompuesto);

                    this.EditarPa = 0;
                    this.idProvArt = 0;
                    Session.Add("ArticulosABM_EditarPa", EditarPa);
                    Session.Add("ArticulosABM_IdProvArt", idProvArt);

                    this.cargarProveedores();
                    this.cargarProveedores2();
                    this.cargarGruposArticulos();
                    this.cargarSubGruposArticulos(Convert.ToInt32(DropListGrupo.SelectedValue));
                    this.cargarMonedasVenta();
                    this.cargarMonedasVenta2();
                    this.cargarPaises();
                    this.cargarSubListas();
                    this.cargarMarcas();
                    this.cargarSucursal();
                    this.cargarStores();

                    //cargo fecha de carga
                    //this.txtFechaAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //this.txtModificado.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //this.txtUltModificacion.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    //modifico
                    if (this.accion == 2 || this.accion == 3)
                    {
                        try
                        {
                            //url anterior
                            this.urlFiltro = Request.UrlReferrer.ToString();
                            this.lblFiltroAnterior.Text = this.urlFiltro;
                        }
                        catch
                        {
                            this.lblFiltroAnterior.Text = "";
                        }
                        //cargo fechas en la solapa store
                        this.txtDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        this.txtHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        //cargo fechas en la solapa datos extra
                        this.txtFechaDesdeExtra.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        this.txtFechaHastaExtra.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        this.txtBeneficiosDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        this.txtBeneficiosHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        cargarArticulo(this.id);
                        //habilita panel carga imagenes
                        this.linkImg.Visible = true;
                        //habilita panel otros pro
                        this.linkProv.Visible = true;
                        //habilita panel compo
                        this.linkCompo.Visible = true;
                        //habilito el panle del store
                        this.linkStore.Visible = true;
                        //habilito el panel de hsitorico costos
                        //this.linkCostos.Visible = true;
                        //habilito el panel de desc x cant
                        //this.linkDesc.Visible = true;
                        //habilito el panel de datos extras
                        this.linkExtras.Visible = true;
                        this.linkMedidas.Visible = true;
                        this.linkBeneficios.Visible = true;
                        this.linkCatalogo.Visible = true;

                        this.txtPrecio.Text = "0";
                        this.txtDescuento.Text = "0";
                        this.txtDescuento2.Text = "0";
                        this.txtDescuento3.Text = "0";
                        this.txtCostoTotalComposicion.Text = "0";
                        this.txtFechaAct.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        //Pongo el botón de código de articulo en disabled, salvo que sea administrador
                        string perfil = Session["Login_NombrePerfil"] as string;
                        if (perfil != "SuperAdministrador")
                        {
                            this.txtCodArticulo.Enabled = false;
                            this.txtCodArticulo.CssClass = "form-control";
                        }
                            

                    }

                    
                }
                //if (!String.IsNullOrEmpty(this.txtCodArticulo.Text))
                if (this.id > 0)
                {
                    this.cargarImagenes(this.id.ToString());
                }

                if (accion == 1)
                {
                    //cargo fecha de carga
                    this.txtFechaAlta.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    this.txtModificado.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    this.txtUltModificacion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    this.btnAgregarProvArt.Enabled = false;
                    this.lbtnAgregarImagen.Enabled = false;
                    this.lbBuscar.Enabled = false;
                    this.lbAgregarArticuloComp.Enabled = false;

                    Configuracion c = new Configuracion();
                    if (c.numeracionArticulos == "1")
                    {
                        this.generarCodigoNuevo();                        
                        //this.txtCodArticulo.Attributes.Add("disabled", "disabled");
                    }
                }
                if (accion == 2 || this.accion == 3)
                {
                    DateTime fechaA = (DateTime)Session["Articulos_FechaAlta"];
                    DateTime UltAct = (DateTime)Session["Articulos_FechaUltActualizacion"];
                    this.txtFechaAlta.Text = fechaA.ToString("dd/MM/yyyy HH:mm");
                    this.txtUltModificacion.Text = UltAct.ToString("dd/MM/yyyy HH:mm");
                    //this.txtModificado.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    this.cargarProveedorArticulos(this.id);
                    this.cargarArticulosCompuestos(this.id);
                    this.cargarDescuentos();
                    this.cargarMedidasVentaArticulo(this.id);
                    this.cargarDatosCombustibles();
                    this.cargarHistorialCostos();
                }


                this.Form.DefaultButton = lbBuscar.UniqueID;
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));

            }
        }

        #region verificaciones load
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
                    if(this.verificarAcceso() != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                    else
                    {
                        this.ocultarPaneles();
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
                string permiso = listPermisos.Where(x => x == "108").FirstOrDefault();

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
                        this.btnAgregar.Visible = true;
                        if (accion == 2)
                        {
                            this.linkDesc.Visible = true;
                            this.btnAgregarSig.Visible = true;
                            this.btnDuplicar.Visible = true;
                            this.linkCostos.Visible = true;
                        }
                    }
                    else
                    {
                        this.panelCosto.Visible = false;
                        this.panelCosto2.Visible = false;
                        this.panelFecha.Visible = false;
                    }
                    return 1;
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void ocultarPaneles()
        {
            try
            {
                int valor = this.contUser.verificarPerfilUsuario((int)Session["Login_IdUser"]);
                //if (valor == 1)
                //{
                //    this.panelCosto.Visible = false;
                //}
                if (valor == 2)
                {
                    this.panelCosto.Visible = false;
                    this.panelCosto2.Visible = false;
                }
            }
            catch
            {

            }
        }

        private void generarCodigoNuevo()
        {
            try
            {
                int codigoNuevo = this.controlador.obtenerUltimoCodigoArticuloNumerico();


                //this.txtCodArticulo.Text = codigoNuevo.ToString().PadLeft(9,'0');

                this.txtCodArticulo.Text = codigoNuevo.ToString();
                this.txtCodArticulo.MaxLength = 8;
            }
            catch
            {

            }
        }
        #endregion

        #region carga combos

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

                this.DropListProveedor.DataSource = dt;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";
                this.DropListProveedor.DataBind();

                this.ListProveedorCombustible.DataSource = dt;
                this.ListProveedorCombustible.DataValueField = "id";
                this.ListProveedorCombustible.DataTextField = "alias";
                this.ListProveedorCombustible.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        public void cargarProveedores2()
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

                this.DropListProveedores2.DataSource = dt;
                this.DropListProveedores2.DataValueField = "id";
                this.DropListProveedores2.DataTextField = "alias";

                this.DropListProveedores2.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        private void cargarGruposArticulos()
        {
            try
            {

                DataTable dt = controlador.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListGrupo.DataSource = dt;
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();

                this.DropListGrupo2.DataSource = dt;
                this.DropListGrupo2.DataValueField = "id";
                this.DropListGrupo2.DataTextField = "descripcion";

                this.DropListGrupo2.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarSubGruposArticulos(int grupo)
        {
            try
            {
                DataTable dt = controlador.obtenerSubGruposArticulos(grupo);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropDownSubGrupo.DataSource = dt;
                this.DropDownSubGrupo.DataValueField = "id";
                this.DropDownSubGrupo.DataTextField = "descripcion";

                this.DropDownSubGrupo.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarMonedasVenta()
        {
            try
            {
                DataTable dt = controlador.obtenerMonedas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["moneda"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropDownMonedaVent.DataSource = dt;
                this.DropDownMonedaVent.DataValueField = "id";
                this.DropDownMonedaVent.DataTextField = "moneda";

                this.DropDownMonedaVent.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando monedas de venta a la lista. " + ex.Message));
            }
        }

        private void cargarMonedasVenta2()
        {
            try
            {
                DataTable dt = controlador.obtenerMonedas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["moneda"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListMoneda2.DataSource = dt;
                this.DropListMoneda2.DataValueField = "id";
                this.DropListMoneda2.DataTextField = "moneda";

                this.DropListMoneda2.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando monedas de venta a la lista. " + ex.Message));
            }
        }

        private void cargarPaises()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerPaisesClientes();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["pais"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListPais.DataSource = dt;
                this.DropListPais.DataValueField = "id";
                this.DropListPais.DataTextField = "pais";

                this.DropListPais.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando paises a la lista. " + ex.Message));
            }
        }

        private void cargarMarcas()
        {
            try
            {
                DataTable dt = controlador.obtenerMarcasDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["marca"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListMarca.DataSource = dt;
                this.DropListMarca.DataValueField = "id";
                this.DropListMarca.DataTextField = "marca";

                this.DropListMarca.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando marcas a la lista. " + ex.Message));
            }
        }

        public void cargarSubListas()
        {
            try
            {
                DataTable dt = this.contLista.obtenerCategoriasSubListasPreciosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["categoria"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSubLista.DataSource = dt;
                this.DropListSubLista.DataValueField = "id";
                this.DropListSubLista.DataTextField = "categoria";

                this.DropListSubLista.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando categoria de sub lista. " + ex.Message));
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.ListSucursalesExtra.DataSource = dt;
                this.ListSucursalesExtra.DataValueField = "Id";
                this.ListSucursalesExtra.DataTextField = "nombre";
                this.ListSucursalesExtra.DataBind();
                this.ListSucursalesExtra.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListSucursalesExtra.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarStores()
        {
            try
            {
                List<Store> storesAux = new List<Store>();
                storesAux = this.contArtEnt.obtenerStores();
                if(storesAux != null)
                {
                    this.ListStores.DataSource = storesAux;
                    this.ListStores.DataValueField = "Id";
                    this.ListStores.DataTextField = "Descripcion";
                    this.ListStores.DataBind();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando la lista de stores. "));
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando stores. " + Ex.Message));
            }
        }

        private void cargarStore_ArticuloAListbox()
        {
            try
            {
                var storesArticulos = this.contArtEnt.obtenerStores_ArticulosPorIdArticulo(this.id);
                if(storesArticulos != null)
                {
                    ListBoxStores.Items.Clear();
                    foreach (Stores_Articulos sa in storesArticulos)
                    {
                        var item = new ListItem();
                        item.Value = sa.Id.ToString();
                        //item.Text = sa.Store1.Descripcion.ToString();
                        if (sa.Store1 == null)
                        {
                            sa.Store1 = this.contArtEnt.obtenerStorePorId((long)sa.Store);
                            
                        }
                        item.Text = sa.Store1.Descripcion;
                        this.ListBoxStores.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando los stores de los articulos. " + ex.Message));
            }
        }



        /// <summary>
        /// cambia la seleccion del combo grupo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropListGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSubGruposArticulos(Convert.ToInt32(DropListGrupo.SelectedValue));
        }
        #endregion

        #region cargar datos articulo
        /// <summary>
        /// Consulta los datos del articulo en la DB y lo devuelve
        /// </summary>
        /// <param name="id">codigo del articulo</param>
        private void cargarArticulo(int idArticulo)
        {
            try
            {
                //reinicio
                this.articulo = new Articulo();

                this.articulo = this.controlador.obtenerArticuloByID(idArticulo);
                this.articulo.alerta = this.controlador.obtenerAlertaArticuloByID(idArticulo);
                if (this.articulo != null)
                {
                    this.idTemp = this.articulo.id;
                    this.txtCodArticulo.Text = this.articulo.codigo;
                    this.txtDescripcion.Text = this.articulo.descripcion;
                    this.DropListProveedor.SelectedValue = this.articulo.proveedor.id.ToString();
                    this.DropListGrupo.SelectedValue = this.articulo.grupo.id.ToString();
                    this.cargarSubGruposArticulos(this.articulo.grupo.id);
                    this.DropDownSubGrupo.SelectedValue = this.articulo.subGrupo.id.ToString();
                    this.txtCosto.Text = this.articulo.costo.ToString(CultureInfo.InvariantCulture);
                    this.txtMargen.Text = this.articulo.margen.ToString(CultureInfo.InvariantCulture);
                    this.txtImpInternos.Text = this.articulo.impInternos.ToString(CultureInfo.InvariantCulture);
                    this.txtIngBrutos.Text = this.articulo.ingBrutos.ToString(CultureInfo.InvariantCulture);
                    this.tPrecioVenta.Value = this.articulo.precioVenta.ToString(CultureInfo.InvariantCulture);
                    this.DropDownMonedaVent.SelectedValue = this.articulo.monedaVenta.id.ToString();
                    //cargo cotizacion
                    this.cargarCotizacion(this.DropDownMonedaVent);

                    this.cargarPrecioVentaMonedaOriginal(this.articulo, ltCotizacion.Text);
                    this.cargarCostoRealMonedaOriginal(this.articulo, ltCotizacion.Text);

                    this.txtStock.Text = this.articulo.stockMinimo.ToString(CultureInfo.InvariantCulture);

                    //if (this.articulo.apareceLista == 1)
                    //    this.DropListAparece.SelectedValue = "SI";
                    //else
                    //    this.DropListAparece.SelectedValue = "NO";
                    this.DropListAparece.SelectedValue = this.articulo.apareceLista.ToString();

                    this.txtUbicacion.Text = this.articulo.ubicacion;
                    this.txtFechaAlta.Text = this.articulo.fechaAlta.ToString("dd/MM/yyyy HH:mm");
                    this.txtUltModificacion.Text = this.articulo.ultActualizacion.ToString("dd/MM/yyyy HH:mm");
                    //this.txtModificado.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    this.txtModificado.Text = this.articulo.modificado.ToString("dd/MM/yyyy HH:mm");
                    this.DropListPais.SelectedValue = this.articulo.procedencia.id.ToString();
                    this.completarDropIva(this.articulo.porcentajeIva);
                    this.txtCodigoBarra.Text = this.articulo.codigoBarra;
                    this.txtIncidencia.Text = this.articulo.incidencia.ToString(CultureInfo.InvariantCulture);
                    this.tCostoImponible.Value = this.articulo.costoImponible.ToString(CultureInfo.InvariantCulture);
                    this.txtCostoIva.Value = (this.articulo.costoImponible * (1 + (this.articulo.porcentajeIva / 100))).ToString("N");

                    this.tCostoReal.Value = this.articulo.costoReal.ToString(CultureInfo.InvariantCulture);
                    this.tPrecioSinIva.Value = this.articulo.precioSinIva.ToString(CultureInfo.InvariantCulture);
                    this.DropListSubLista.SelectedValue = this.articulo.listaCategoria.id.ToString();
                    this.txtObservacion.Text = this.articulo.observacion;
                    this.txtAlerta.Text = this.articulo.alerta.descripcion;
                    //guardo temporales
                    Session.Add("Articulos_Codigo", articulo.codigo);
                    Session.Add("Articulos_FechaAlta", articulo.fechaAlta);
                    Session.Add("Articulos_FechaUltActualizacion", articulo.ultActualizacion);
                    ViewState.Add("PrecioVenta", articulo.precioVenta);

                    Session.Add("Articulos_PrecioVenta", articulo.precioVenta);

                    //cargar datos marcas, tipo distribucion
                    //this.DropListMarca.SelectedValue = this.controlador.obtenerArticuloMarca(this.articulo.id).ToString();
                    this.cargarDatosMarca(this.articulo.id);

                    //cargo articulo store
                    this.cargarDatosStore(this.articulo.id);
                    this.cargarStore_ArticuloAListbox();

                    if (!this.DropListPais.SelectedItem.Text.Contains("Argentina") && !this.DropListPais.SelectedItem.Text.Contains("NACIONAL"))
                    {
                        this.panelDespacho.Visible = true;
                        //cargo datos despacho
                        this.cargarDatosDespacho(this.articulo.id);
                    }
                    //cargo datos presentaciones
                    this.cargarDatosPresentaciones(this.articulo.id);

                    //cargo datos extras
                    this.cargarDatosExtras();

                    this.cargarMedidasVentaArticulo(this.id);

                    this.cargarDatosBeneficios(this.id);

                    //Cargo datos del Catalogo
                    this.cargarCatalogoArticulo();

                    //Cargo datos sobre el articulo si aparece en lista de precios
                    this.cargarApareceListaArticulo();

                    grupo g = this.controlador.obtenerGrupoID(this.articulo.grupo.id);
                    if (g.descripcion.Contains("COMBUSTIBLES"))
                    {
                        //habilito el panel combustible
                        this.linkCombustible.Visible = true;
                    }
                }
                else
                {
                    //Pop up 
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo cargar el articulo"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando campos del articulo. " + ex.Message));
            }
        }

        private void cargarDatosStore(int idArticulo)
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio a cargar datos del articulo store");
                var store = this.controlador.obtenerArticuloStoreByArticulo(idArticulo);
                if (store != null)
                {
                    //si aparece es porque esta tildado
                    if (this.controlador.apareceEnStore(idArticulo))
                    {
                        this.ListApareceStore.SelectedValue = "SI";
                        this.UpdatePanelStoresArticulos.Visible = true;
                    }
                        

                    if (store.Oferta != null)
                        this.ListOferta.SelectedValue = store.Oferta.ToString();

                    if (store.Destacado == 1)
                        this.ListDestacado.SelectedValue = "SI";

                    if (store.Novedad == 1)
                        this.ListNovedades.SelectedValue = "SI";

                    this.txtDesde.Text = Convert.ToDateTime(store.Desde, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                    this.txtDesdeHora.Text = Convert.ToDateTime(store.Desde, new CultureInfo("es-AR")).ToString("HH:mm");
                    this.txtHasta.Text = Convert.ToDateTime(store.Hasta, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                    this.txtHastaHora.Text = Convert.ToDateTime(store.Hasta, new CultureInfo("es-AR")).ToString("HH:mm");

                    this.txtPrecioOferta.Text = store.Precio.ToString();
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Articulo store cargado");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando campos del store . " + ex.Message));
            }
        }

        private void cargarDatosMarca(int idArticulo)
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio a cargar datos marca del articulo " + idArticulo);
                var marca = this.contArtEnt.obtenerMarcaByArticulo(idArticulo);

                this.DropListMarca.SelectedValue = marca.idMarca.ToString();
                this.ListTipoDistribucion.SelectedValue = marca.TipoDistribucion.ToString();
                //cargo cot si tiene
                this.txtCodCot.Text = marca.CodigoCot;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando campos de la marca . " + ex.Message));
            }
        }

        #endregion

        #region funciones abm articulos
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
                this.agregarArticulo(0);

            if (this.accion == 2)
                this.modificarArticulo(0);
            
           
        }

        protected void btnAgregarSig_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
                this.agregarArticulo(1);

            if (this.accion == 2)
                this.modificarArticulo(1);
        }

        protected void btnDuplicar_Click(object sender, EventArgs e)
        {
            try
            {
                //redirijo
                this.DuplicarArticulo();
            }
            catch(Exception ex)
            {
 
            }

        }

        private int agregarArticulo(int a)
        {
            try
            {
                Configuracion c = new Configuracion();
                if (c.numeracionArticulos == "1")
                {
                    this.generarCodigoNuevo();
                }

                Articulo art = new Articulo();                
                art.codigo = this.txtCodArticulo.Text;
                art.descripcion = this.txtDescripcion.Text;
                art.proveedor.id = Convert.ToInt32(this.DropListProveedor.SelectedValue,CultureInfo.InvariantCulture);
                art.grupo.id = Convert.ToInt32(this.DropListGrupo.SelectedValue, CultureInfo.InvariantCulture);
                art.subGrupo.id = Convert.ToInt32(this.DropDownSubGrupo.SelectedValue, CultureInfo.InvariantCulture);
                art.costo = Convert.ToDecimal(this.txtCosto.Text, CultureInfo.InvariantCulture);
                art.margen = Convert.ToDecimal(this.txtMargen.Text, CultureInfo.InvariantCulture);
                art.impInternos = Convert.ToDecimal(this.txtImpInternos.Text, CultureInfo.InvariantCulture);
                art.ingBrutos = Convert.ToDecimal(this.txtIngBrutos.Text, CultureInfo.InvariantCulture);
                art.precioVenta = Convert.ToDecimal(this.tPrecioVenta.Value, CultureInfo.InvariantCulture);
                art.monedaVenta.id = Convert.ToInt32(this.DropDownMonedaVent.SelectedValue, CultureInfo.InvariantCulture);
                art.stockMinimo = Convert.ToDecimal(this.txtStock.Text, CultureInfo.InvariantCulture);
                art.apareceLista = Convert.ToInt32(this.DropListAparece.SelectedValue, CultureInfo.InvariantCulture);
                art.ubicacion = this.txtUbicacion.Text;
                art.fechaAlta = DateTime.Now;
                art.ultActualizacion = DateTime.Now;
                art.modificado = DateTime.Now;
                art.procedencia.id = Convert.ToInt32(this.DropListPais.SelectedValue, CultureInfo.InvariantCulture);
                art.porcentajeIva = Convert.ToDecimal(this.DropListPorcentajeIVA.SelectedValue, CultureInfo.InvariantCulture);
                art.codigoBarra = this.txtCodigoBarra.Text;
                art.incidencia = Convert.ToDecimal(this.txtIncidencia.Text, CultureInfo.InvariantCulture);
                art.costoImponible = Convert.ToDecimal(this.tCostoImponible.Value, CultureInfo.InvariantCulture);
                art.costoReal = Convert.ToDecimal(this.tCostoReal.Value, CultureInfo.InvariantCulture);
                art.precioSinIva = Convert.ToDecimal(this.tPrecioSinIva.Value, CultureInfo.InvariantCulture);
                art.listaCategoria.id = Convert.ToInt32(this.DropListSubLista.SelectedValue);
                art.observacion = this.txtObservacion.Text;

                art.alerta = new AlertaArticulo();
                art.alerta.descripcion = this.txtAlerta.Text;

                int i = this.controlador.agregarArticulo(art);

                if (i > 0)
                {
                    //si logre dar de alta el articulo intento guardar los datos de despacho y los datos de presentaciones 
                    // i = idArticulo nuevo
                    this.guardarDatosDespacho(i);
                    this.guardarDatosPresentaciones(i);                    
                }
                //traspaso temporal para el siguiente
                //int idart=0;
                if (a == 0)
                {
                    if (i > 0)
                    {
                        //agrego la marca
                        Articulos_Marca am = new Articulos_Marca();
                        am.idArticulo = i;
                        am.idMarca = Convert.ToInt64(this.DropListMarca.SelectedValue);
                        am.TipoDistribucion = Convert.ToInt32(this.ListTipoDistribucion.SelectedValue);
                        //this.controlador.agregarArticuloMarca(i, Convert.ToInt32(this.DropListMarca.SelectedValue));
                        i = this.contArtEnt.agregarMarca(am);
                        i = 1;
                    }
                    this.RespuestaAgregarArticulo(i);
                }
                else
                {
                   //estoy duplicando
                    return i;
                }
                return 0;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulo" + ex.Message));
                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
                return -1;
            }
        }

        private void modificarArticulo(int a)
        {
            try
            {
                Articulo art = new Articulo();

                //Asigno el codigo
                art.id = this.id;
                DateTime fechaA = (DateTime)Session["Articulos_FechaAlta"];
                DateTime UltAct = (DateTime)Session["Articulos_FechaUltActualizacion"];
                string cod = Session["Articulos_Codigo"] as string;
                art.codigo = this.txtCodArticulo.Text;
                art.descripcion = this.txtDescripcion.Text;
                art.proveedor.id = Convert.ToInt32(this.DropListProveedor.SelectedValue, CultureInfo.InvariantCulture);
                art.grupo.id = Convert.ToInt32(this.DropListGrupo.SelectedValue, CultureInfo.InvariantCulture);
                art.subGrupo.id = Convert.ToInt32(this.DropDownSubGrupo.SelectedValue, CultureInfo.InvariantCulture);
                art.costo = Convert.ToDecimal(this.txtCosto.Text, CultureInfo.InvariantCulture);
                art.margen = Convert.ToDecimal(this.txtMargen.Text, CultureInfo.InvariantCulture);
                art.impInternos = Convert.ToDecimal(this.txtImpInternos.Text, CultureInfo.InvariantCulture);
                art.ingBrutos = Convert.ToDecimal(this.txtIngBrutos.Text, CultureInfo.InvariantCulture);
                //art.precioVenta = Convert.ToDecimal(this.txtPrecioVenta.Text.Replace('.', ','));
                art.precioVenta = Convert.ToDecimal(this.tPrecioVenta.Value, CultureInfo.InvariantCulture);
                art.monedaVenta.id = Convert.ToInt32(this.DropDownMonedaVent.SelectedValue.Replace(',', '.'), CultureInfo.InvariantCulture);
                art.stockMinimo = Convert.ToDecimal(this.txtStock.Text, CultureInfo.InvariantCulture);
                art.apareceLista = Convert.ToInt32(this.DropListAparece.SelectedValue, CultureInfo.InvariantCulture);
                art.ubicacion = this.txtUbicacion.Text;
                //art.fechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
                //art.ultActualizacion = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                art.fechaAlta = fechaA;
                art.observacion = this.txtObservacion.Text;
                
                decimal precioTemp = (decimal)ViewState["PrecioVenta"] ;
                if (precioTemp != art.precioVenta)
                {
                    art.ultActualizacion = DateTime.Now;
                }
                else
                {
                    art.ultActualizacion = UltAct;
                }

                art.modificado = DateTime.Now;
                art.procedencia.id = Convert.ToInt32(this.DropListPais.SelectedValue, CultureInfo.InvariantCulture);
                art.porcentajeIva = Convert.ToDecimal(this.DropListPorcentajeIVA.SelectedValue, CultureInfo.InvariantCulture);
                art.codigoBarra = this.txtCodigoBarra.Text;
                art.estado = 1;
                art.incidencia = Convert.ToDecimal(this.txtIncidencia.Text, CultureInfo.InvariantCulture);
                art.costoImponible = Convert.ToDecimal(this.tCostoImponible.Value, CultureInfo.InvariantCulture);
                art.costoReal = Convert.ToDecimal(this.tCostoReal.Value, CultureInfo.InvariantCulture);
                art.precioSinIva = Convert.ToDecimal(this.tPrecioSinIva.Value, CultureInfo.InvariantCulture);
                art.listaCategoria.id = Convert.ToInt32(this.DropListSubLista.SelectedValue);

                art.alerta = this.controlador.obtenerAlertaArticuloByID(art.id);
                art.alerta.descripcion = this.txtAlerta.Text;

                this.guardarDatosDespacho(art.id);
                this.guardarDatosPresentaciones(art.id);
                this.guardarHistorialCosto(art);

                int i = this.controlador.modificarArticulo(art,cod);
                if (i == 1)
                {
                    //cargo bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Articulo: " + this.txtDescripcion.Text);
                    //this.controlador.modificarArticuloMarca(art.id, Convert.ToInt32(this.DropListMarca.SelectedValue));
                    //modifico marca
                    Articulos_Marca am = this.contArtEnt.obtenerMarcaByArticulo(art.id);
                    am.idMarca = Convert.ToInt32(this.DropListMarca.SelectedValue);
                    am.TipoDistribucion = Convert.ToInt32(this.ListTipoDistribucion.SelectedValue);
                    i = this.contArtEnt.modificarMarca(am);
                    if (a == 1)
                    {
                        this.irProximoArticulo(art.id);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo modificado con exito", this.lblFiltroAnterior.Text));
                        //Response.Write("<html><body><script>history.go(-2);</script></body></html>");          
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.volverAtras());
                    }
                }
                if (i == -2)
                {

                    //Agrego mal
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El Codigo de Articulo ya fue ingresado", null));
                }
                if (i == -7)
                {

                    //Agrego mal
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Codigo de Barras del Articulo ya fue ingresado"));
                }
                else
                {
                    //Agrego mal
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar articulo. Verifique los datos ingresados", null));
                }
                        
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando articulo" + ex.Message));
            }
 
        }

       
        
        private void DuplicarArticulo()
        {
            try
            {
                int i = this.controlador.duplicarArticulo(this.id, this.txtCodArticulo.Text, this.txtCodigoBarra.Text,
                    Convert.ToInt64(this.DropListMarca.SelectedValue), 
                    Convert.ToInt32(this.ListTipoDistribucion.SelectedValue));

                if (i > 0)
                {

                    Response.Redirect("ArticulosABM.aspx?accion=2&id=" + i);

                }

                else
                {

                    if (i == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Para Duplicar el Articulo, debe ingresar otro código en el campo Codigo Articulo. "));
                    }
                    if (i == -7)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Para Duplicar el Articulo, debe ingresar otro código en el campo Codigo Barra. "));
                    }

                    else
                    {

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo duplicar articulo "));

                    }

                }

            }

            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error duplicando articulos " + ex.Message));

            }

        }

        private int agregarArticuloStore()
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio agregar articulo store");
                ArticulosStore artStore = new ArticulosStore();
                artStore.IdArticulo = this.id;

                artStore.Oferta = Convert.ToInt32(this.ListOferta.SelectedValue);
                //artStore.Oferta = 0;
                //if(this.ListOferta.SelectedValue == "SI")
                //    artStore.Oferta = 1;
                
                artStore.Destacado = 0;
                if (this.ListDestacado.SelectedValue == "SI")
                    artStore.Destacado = 1;

                artStore.Novedad = 0;
                if (this.ListNovedades.SelectedValue == "SI")
                    artStore.Novedad = 1;

                var horaInicio = this.txtDesdeHora.Text.Split(':');
                var horafin = this.txtHastaHora.Text.Split(':');

                artStore.Desde = Convert.ToDateTime(this.txtDesde.Text, new CultureInfo("es-AR")).AddHours(Convert.ToInt32(horaInicio[0])).AddMinutes(Convert.ToInt32(horaInicio[1]));

                artStore.Hasta = Convert.ToDateTime(this.txtHasta.Text, new CultureInfo("es-AR")).AddHours(Convert.ToInt32(horafin[0])).AddMinutes(Convert.ToInt32(horafin[1]));

                artStore.Precio = Convert.ToDecimal(this.txtPrecioOferta.Text, CultureInfo.InvariantCulture);
                artStore.Especificaciones = this.txtEspecificacionStore.Text;

                int aparece = 0;
                if (this.ListApareceStore.SelectedValue == "SI") 
                    aparece = 1;
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Voy a agregar el articulo");
                int i = this.controlador.agregarStore(artStore, aparece);

                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Articulo agregado a store ");
                    return i;
                }
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se pudo agregar Articulo a store ");
                return 0;

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error agegarndo/modificando store articulo" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agegarndo/modificando store articulo" + ex.Message));
                return -1;
            }
        }

        private void RespuestaAgregarArticulo(int i)
        {
            try
            {
                switch(i)
                {
                    case 1:
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Articulo: " + this.txtDescripcion.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo agregado con exito", "ArticulosABM.aspx?accion=1"));                                      

                        //if (a == 1)
                        //{
                        //    //articulo siguiente
                        //    this.irProximoArticulo(idarticulo);
                        //}
                        //else
                        //{
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo agregado con exito", "ArticulosABM.aspx?accion=1"));
                        //}
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar articulo. Verifique los datos ingresados"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Codigo de Articulo ya fue ingresado"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error agregando articulo"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error asignando stock de articulo"));
                        break;
                    case -7:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Código de Barras del Articulo ya fue ingresado"));
                        break;
                }

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "+ex.Message));
            }
        }

        #endregion
        
        #region funciones auxiliares, alta grupo, calculos precios, imagenes
        private void cargarProducto(string busqueda)
        {
            try
            {
                //Articulo art = this.controlador.obtenerArticuloCodigo(busqueda);
                DataTable dtArticulos = this.controlador.buscarArticulosDT(busqueda);

                if (dtArticulos != null)
                {
                    //agrego todos
                    DataRow dr = dtArticulos.NewRow();
                    dr["Descripcion"] = "Seleccione...";
                    dr["id"] = -1;
                    dtArticulos.Rows.InsertAt(dr, 0);

                    this.DropListArticulosComp.DataSource = dtArticulos;
                    this.DropListArticulosComp.DataValueField = "id";
                    this.DropListArticulosComp.DataTextField = "Descripcion";

                    this.DropListArticulosComp.DataBind();
                    //this.txtDescripcionArticulo.Text = art.descripcion;
                    //this.txtArticuloCompuesto.Text = art.id.ToString();
                    //this.txtCantidadArticulo.Focus();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra Articulo " + busqueda));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Articulo. " + ex.Message));
            }
        }
        private void irProximoArticulo(int idArticulo)
        {
            try
            {
                idArticulo = idArticulo + 1;
                var art = this.controlador.obtenerArticuloId(idArticulo);
                if (art != null)
                {
                    Response.Redirect("ArticulosABM.aspx?accion=2&id=" + idArticulo);
                }
            }
            catch
            {

            }
        }
        protected void btnAgregarGrupo_Click(object sender, EventArgs e)
        {
            try 
            {
                
                int i = this.controlador.agregarGrupo(this.txtGrupo.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo grupos
                    this.cargarGruposArticulos();
                    this.txtGrupo.Text = "";
                }
                else
                {
                    if (i == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya existe un grupo con el nombre " + this.txtGrupo.Text));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Grupo"));
                    }
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando grupo. " + ex.Message));
            }
        }
        protected void btnAgregarSubgrupo_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.controlador.agregarSubGrupo(Convert.ToInt32(this.DropListGrupo2.SelectedValue), this.TxtSubGrupo.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo grupos
                    this.cargarGruposArticulos();
                    this.TxtSubGrupo.Text = "";
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando subGrupo. " + ex.Message));
            }
        }
        protected void lbtnAgregarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                Marca marca = new Marca();
                marca.descripcion = this.txtMarcaNueva.Text;
                marca.estado = 1;
                int i = this.controlador.agregarMarca(marca);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego la Marca: " + this.txtMarcaNueva.Text);
                    this.cargarMarcas();
                    this.txtMarcaNueva.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Marca"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando marca. " + ex.Message));
            }
        }
        private void completarDropIva(decimal porcentaje)
        {
            string p = porcentaje.ToString();
            try
            {
                if (p == "21.00" || p == "21,00")
                {
                    this.DropListPorcentajeIVA.SelectedValue = "21";
                }
                if (p == "10.50" || p == "10,50")
                {
                    this.DropListPorcentajeIVA.SelectedValue = "10.5";
                }
                if (p == "0.00" || p == "0,00")
                {
                    this.DropListPorcentajeIVA.SelectedValue = "0";
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando porcentaje de IVA " + ex.Message));

            }
        }
        protected void lbGenerarPrecioVenta_Click(object sender, EventArgs e)
        {
            this.calcularPrecioDesdeVenta();
        }
        private void calcularPrecioDesdeCosto()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtCosto.Text))
                {
                    if (!String.IsNullOrEmpty(this.txtIncidencia.Text))
                    {
                        if (!String.IsNullOrEmpty(this.txtImpInternos.Text))
                        {
                            if (!String.IsNullOrEmpty(this.txtIngBrutos.Text))
                            {

                                if (!String.IsNullOrEmpty(this.txtMargen.Text))
                                {
                                    if (DropListPorcentajeIVA.SelectedValue != "-1")
                                    {

                                        Articulo art = new Articulo();
                                        art.costo = Convert.ToDecimal(txtCosto.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.incidencia = Convert.ToDecimal(txtIncidencia.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.ingBrutos = Convert.ToDecimal(txtIngBrutos.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.impInternos = Convert.ToDecimal(txtImpInternos.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.margen = Convert.ToDecimal(txtMargen.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.porcentajeIva = Convert.ToDecimal(DropListPorcentajeIVA.SelectedValue, CultureInfo.InvariantCulture);
                                        art.monedaVenta.id = Convert.ToInt32(this.DropDownMonedaVent.SelectedValue);
                                        art.monedaVenta.cambio = this.contCobranza.obtenerCotizacion(art.monedaVenta.id);

                                        Articulo a = this.controlador.obtenerPrecioVentaDesdeCosto(art);

                                        this.tCostoImponible.Value = a.costoImponible.ToString(CultureInfo.InvariantCulture);
                                        this.txtCostoIva.Value = (a.costoImponible * (1 + (a.porcentajeIva / 100))).ToString("N");
                                        this.tCostoReal.Value = a.costoReal.ToString(CultureInfo.InvariantCulture);
                                        this.tPrecioVenta.Value = a.precioVenta.ToString(CultureInfo.InvariantCulture);
                                        this.tPrecioSinIva.Value = a.precioSinIva.ToString(CultureInfo.InvariantCulture);
                                        //decimal precioVentaMonedaOriginal = Math.Round(a.precioVenta / art.monedaVenta.cambio, 2);
                                        //this.tPrecioVentaMonedaOriginal.Value = precioVentaMonedaOriginal.ToString("0.00");
                                        this.cargarPrecioVentaMonedaOriginal(a, art.monedaVenta.cambio.ToString());
                                        this.cargarCostoRealMonedaOriginal(a, art.monedaVenta.cambio.ToString());
                                    }
                                    else
                                    {
                                        //this.DropListPorcentajeIVA.Focus();
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Seleccione un porcentaje de IVA", null));
                                        this.DropListPorcentajeIVA.Focus();
                                    }

                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso porcentaje de margen", null));
                                    this.txtMargen.Focus();
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Ingresos Brutos", null));
                                this.txtIngBrutos.Focus();
                            }

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Impuestos Internos", null));
                            this.txtImpInternos.Focus();
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Incidencia", null));
                        this.txtIncidencia.Focus();
                    }
                }
                else
                {

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso costo del articulo", null));
                    this.txtCosto.Focus();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Precio de Venta" + ex.Message));

            }
        }
        private void calcularPrecioDesdeVenta()
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtCosto.Text))
                {
                    if (!String.IsNullOrEmpty(this.txtIncidencia.Text))
                    {
                        if (!String.IsNullOrEmpty(this.txtImpInternos.Text))
                        {
                            if (!String.IsNullOrEmpty(this.txtIngBrutos.Text))
                            {

                                if (!String.IsNullOrEmpty(this.txtMargen.Text))
                                {
                                    if (DropListPorcentajeIVA.SelectedValue != "-1")
                                    {
                                        Articulo art = new Articulo();
                                        art.costo = Convert.ToDecimal(txtCosto.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.costoReal = Convert.ToDecimal(tCostoReal.Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.incidencia = Convert.ToDecimal(txtIncidencia.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.ingBrutos = Convert.ToDecimal(txtIngBrutos.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.impInternos = Convert.ToDecimal(txtImpInternos.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.margen = Convert.ToDecimal(txtMargen.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                                        art.porcentajeIva = Convert.ToDecimal(DropListPorcentajeIVA.SelectedValue, CultureInfo.InvariantCulture);
                                        art.monedaVenta.id = Convert.ToInt32(this.DropDownMonedaVent.SelectedValue);
                                        art.monedaVenta.cambio = this.contCobranza.obtenerCotizacion(art.monedaVenta.id);

                                        Articulo a = this.controlador.obtenerPrecioVentaDesdeVenta(art, Convert.ToDecimal(this.tPrecioVenta.Value));

                                        this.txtMargen.Text = a.margen.ToString(CultureInfo.InvariantCulture);

                                        //this.tCostoImponible.Value = a.costoImponible.ToString(CultureInfo.InvariantCulture);
                                        this.tCostoReal.Value = a.costoReal.ToString(CultureInfo.InvariantCulture);
                                        //this.tPrecioVenta.Value = a.precioVenta.ToString(CultureInfo.InvariantCulture);
                                        this.tPrecioSinIva.Value = a.precioSinIva.ToString(CultureInfo.InvariantCulture);
                                        //decimal precioVentaMonedaOriginal = Math.Round(a.precioVenta / art.monedaVenta.cambio, 2);
                                        //this.tPrecioVentaMonedaOriginal.Value = precioVentaMonedaOriginal.ToString("0.00");
                                        this.cargarPrecioVentaMonedaOriginal(a, art.monedaVenta.cambio.ToString());
                                        this.cargarCostoRealMonedaOriginal(a, art.monedaVenta.cambio.ToString());
                                    }
                                    else
                                    {
                                        //this.DropListPorcentajeIVA.Focus();
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Seleccione un porcentaje de IVA", null));
                                        this.DropListPorcentajeIVA.Focus();
                                    }

                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso porcentaje de margen", null));
                                    this.txtMargen.Focus();
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Ingresos Brutos", null));
                                this.txtIngBrutos.Focus();
                            }

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Impuestos Internos", null));
                            this.txtImpInternos.Focus();
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso un porcentaje de Incidencia", null));
                        this.txtIncidencia.Focus();
                    }
                }
                else
                {

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se ingreso costo del articulo", null));
                    this.txtCosto.Focus();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Precio de Venta" + ex.Message));

            }
        }
        protected void lbtnAgregarPais_Click(object sender, EventArgs e)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                int i = controladorPais.agregarPais(this.txtPais.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo paises
                    this.cargarPaises();
                    this.txtPais.Text = "";
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Pais. " + ex.Message));
            }
        }
        protected void lbtnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                this.subirImagen1();
            }
            catch
            {

            }
        }
        protected void cargarPrecioVentaMonedaOriginal(Articulo a, string valorMoneda)
        {
            try
            {
                this.panelMonedaOriginal.Visible = false;

                if (this.DropDownMonedaVent.SelectedItem.Text != "Pesos")
                {
                    this.panelMonedaOriginal.Visible = true;
                    if (a != null && !string.IsNullOrEmpty(valorMoneda))
                    {
                        decimal precioVentaMonedaOriginal = Math.Round(a.precioVenta / Convert.ToDecimal(valorMoneda), 2);
                        this.tPrecioVentaMonedaOriginal.Value = precioVentaMonedaOriginal.ToString("0.00");
                    }
                }
                
            }
            catch (Exception Ex)
            {

            }
        }
        protected void cargarCostoRealMonedaOriginal(Articulo a, string valorMoneda)
        {
            try
            {
                this.panelCostoRealMonedaOriginal.Visible = false;

                if (this.DropDownMonedaVent.SelectedItem.Text != "Pesos")
                {
                    this.panelCostoRealMonedaOriginal.Visible = true;
                    if (a != null && !string.IsNullOrEmpty(valorMoneda))
                    {
                        decimal precioVentaMonedaOriginal = Math.Round(a.costoReal / Convert.ToDecimal(valorMoneda), 2);
                        this.tCostoRealMonedaOriginal.Value = precioVentaMonedaOriginal.ToString("0.00");
                    }
                }

            }
            catch (Exception Ex)
            {

            }
        }
        public void subirImagen1()
        {
            //if(!String.IsNullOrEmpty(this.txtCodArticulo.Text))
            if(this.id > 0)
            {
                if (IsPostBack)
                {
                    Boolean fileOK = false;

                    String path = Server.MapPath("../../images/Productos/" + this.id + "/");

                    if (FileUpload1.HasFile)
                    {
                        String fileExtension =
                            System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                        String[] allowedExtensions = { ".jpg" };

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
                        try
                        {
                            //creo el directorio si no exites y subo la foto
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Voy a subir imagen");

                            if (!Directory.Exists(path))
                            {
                                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "No existe directorio. " + path + ". lo creo");

                                Directory.CreateDirectory(path);
                                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "directorio creado");
                            }

                            int cant = this.verificarCantidad(path);
                            if(cant < 3 )
                            {
                                //guardo nombre archivo
                                string imagen = FileUpload1.FileName;
                                
                                //lo subo
                                FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);

                                

                                //cambio imagen y le asigno el nuevo nombre
                                imagen = this.modificarNombre(path + imagen, this.id.ToString(), cant);
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Subio Imagen a Articulo: " + this.id);
                                ////////////////////STORE\\\\\\\\\\\\\\\\\\\\\\\\\
                                //subo la imagen al store
                                //datos del FTP
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio subir Imagen a store: " + this.id);
                                string carpeta = imagen.Split('\\')[imagen.Split('\\').Length - 2];
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Carpeta:" + carpeta);
                                string nombreImagen = imagen.Split('\\')[imagen.Split('\\').Length - 1];
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Imagen:" + nombreImagen);
                                FTPManager theFTP = new FTPManager();

                                theFTP.ftpServer = WebConfigurationManager.AppSettings.Get("ImagenesStore");
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "URL FTP:" + theFTP.ftpServer);
                                
                                theFTP.ftpUser = WebConfigurationManager.AppSettings.Get("Usuario");
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "usuario FTP:" + theFTP.ftpUser);
                                
                                theFTP.ftpPass = WebConfigurationManager.AppSettings.Get("pass");
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Contraseña FTP:" + theFTP.ftpPass);
                                controladorFunciones fun = new controladorFunciones();
                                

                                //verifico si existe el directorio en destino
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "verifico si Existe directorio");
                                if (!fun.ExisteDirectorio(theFTP, carpeta + "//"))
                                {
                                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No existe directorio voy a crearlo");
                                    //si no existe lo creo
                                    fun.CreateFolder(theFTP, carpeta);
                                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Directotio creado");
                                }
                                //subo el archivo al ftp del store
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Voy a subir imagen a FTP");
                                fun.subirArchivoFTP(path, nombreImagen, theFTP, carpeta);
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Imagen Subida");
                                //actualizo el panel de imagenes
                                this.cargarImagenes(this.id.ToString());
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya alcanzo la cantidad permitida de Imagenes por Articulo"));

                            }


                            
                            //Label1.Text = "File uploaded!";

                            //verifico si borra

                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Imagen Actualizada con exito');", true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Imagen agregada con exito ", null));

                        }

                        catch (Exception ex)
                        {
                            //Label1.Text = "File could not be uploaded.";
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error actualizando imagen " + ex.Message + " ');", true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando imagen " + ex.Message));
                        }
                    }
                    else
                    {
                        //Label1.Text = "Cannot accept files of this type.";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El archivo debe ser JPG o PNG "));
                    }
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe ingresar el Codigo de Articulo para poder Subir Imagenes"));

            }
        }
        private string modificarNombre(string pathFile, string id, int cant)
        {
            try
            {
                string imagenCodigo = Server.MapPath("../../images/Productos/" + id + "//") + id + "_0.jpg";
                //si existe la imagen cero la agrego una nueva, sino la cero
                if (File.Exists(imagenCodigo))
                {
                    imagenCodigo = Server.MapPath("../../images/Productos/" + id + "//") + id + "_" + cant.ToString() + ".jpg";
                }
                File.Copy(pathFile, imagenCodigo, true);
                File.Delete(pathFile);
                return imagenCodigo;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Error cambiando nombre de Imagen. " + ex.Message + " ');", true);
                return String.Empty;
            }
        }
        private int verificarCantidad(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                int numFiles = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains(".jpg") || files[i].Contains(".png"))
                    {
                        numFiles++;
                    }
                }

                return numFiles;
            }
            catch
            {
                return -1;
            }
        }
        private void cargarImagenes(string idArticulo)
        {
            try
            {
                
                string[] imagenes =  Directory.GetFiles(Server.MapPath("../../images/Productos/" + idArticulo + "/"));
                TableRow tr = new TableRow();
                //limpio el placeholder
                this.phImagenesArticulos.Controls.Clear();
                for (int i = 0; i < imagenes.Length; i++)
                {
                    FileInfo fi = new FileInfo(imagenes[i]);

                    TableCell celImagen = new TableCell();
                    Label gallery = new Label();
                    gallery.Text += @"<li>";
                    gallery.Text += @"<a href=../../images/Productos/" + idArticulo + "/" + fi.Name + " class=\"ui-lightbox\" >";
                    gallery.Text += "<img height=\"100\" width = \"100\" src=\"/images/Productos/" + idArticulo + "/" + fi.Name + "\" alt=\"\" />";
                    gallery.Text += @"</a>";
                    gallery.Text += @"<a href=../../images/Productos/" + idArticulo + "/" + fi.Name + " class=\"preview\"></a>";
                    gallery.Text += @" </li>";
                    gallery.Text += "<br/>";

                    celImagen.Controls.Add(gallery);

                    CheckBox ck = new CheckBox();
                    ck.ID = fi.Name.Substring(0, fi.Name.Length - 4);
                    celImagen.Controls.Add(ck);

                    tr.Cells.Add(celImagen);
                }

                phImagenesArticulos.Controls.Add(tr);
            }
            catch
            {

            }
        }
        protected bool existeArticuloStores_Articulos(Stores_Articulos sa)
        {
            try
            {
                Stores_Articulos saAux = this.contArtEnt.obtenerStores_ArticulosPorArticuloyStore(sa);
                if (saAux != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return true;
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    this.cargarProducto(this.txtBusqueda.Text);
                }
                else
                {
                    this.txtDescripcionArticulo.Text = "";
                }
            }
            catch
            {

            }
        }
        protected void btnCalcCostos_Click(object sender, EventArgs e)
        {
            this.calcularPrecioDesdeCosto();
        }
        protected void DropDownMonedaVent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCotizacion(DropDownMonedaVent);
                this.cargarPrecioVentaMonedaOriginal(null, null);
                this.cargarCostoRealMonedaOriginal(null, null);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxError("Error obteniendo cotizacion moneda. " + ex.Message), true);
            }
        }
        protected void lbtnCostocompuesto_Click(object sender, EventArgs e)
        {
            try
            {
                decimal costoCompuesto = Convert.ToDecimal(this.txtCostoTotalComposicion.Text);
                this.txtCosto.Text = costoCompuesto.ToString();
                this.calcularPrecioDesdeCosto();

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Costo recalculado con exito\", {type: \"info\"});", true);

            }
            catch
            {

            }
        }
        protected void btnAgregarStores_Click(object sender, EventArgs e)
        {
            try
            {
                Stores_Articulos sa = new Stores_Articulos();
                sa.Articulo = this.id;
                sa.Store = Convert.ToInt64(this.ListStores.SelectedValue);
                if (existeArticuloStores_Articulos(sa) == false)
                {
                    int i = this.contArtEnt.agregarStores_Articulos(sa);
                    if (i > 0)
                    {

                        //this.ListBoxStores.Items.Add(this.ListStores.SelectedItem.Text);
                        this.ListStores.SelectedIndex = 0;
                        //recargo items a listbox
                        this.cargarStore_ArticuloAListbox();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error agregando el Articulo en el Store seleccionado. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ya existe el Articulo en el Store seleccionado. "));
                }


            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Articulo al Store seleccionado." + Ex.Message));
            }
        }
        protected void btnQuitarStore_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.contArtEnt.eliminarStores_Articulos(Convert.ToInt64(ListBoxStores.SelectedValue));
                if (i > 0)
                {
                    //this.ListBoxStores.Items.Remove(this.ListBoxStores.SelectedItem);
                    this.ListStores.SelectedIndex = 0;
                    //recargo el listox item
                    this.cargarStore_ArticuloAListbox();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo sacar el Articulo del Store seleccionado."));
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Articulo del Store seleccionado." + Ex.Message));
            }

        }

        protected void ListApareceStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListApareceStore.SelectedValue == "SI")
            {
                this.UpdatePanelStoresArticulos.Visible = true;
            }
            else
            {
                this.UpdatePanelStoresArticulos.Visible = false;
            }

        }
        #endregion
        
        //cotizacion del dolar en detalle

        #region store
        protected void btnAgregarStore_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                {
                    int i = this.agregarArticuloStore();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta/ modificacion Articulo en store: " + this.txtDescripcion.Text + "en store");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito\", {type: \"info\"});", true);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxInfo("Articulo guardado con exito en store", null), true);
                    }
                    else
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se pudo realizar Alta/ modificacion Articulo en store: " + this.txtDescripcion.Text + "en store");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito\", {type: \"error\"});", true);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxInfo("No se pudo guardar Articulo en store", null), true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error agregando articulo a store " + ex.Message), true);
            }
        }
        protected void lbtnAgregar_Click1(object sender, EventArgs e)
        {
            try
            {
                foreach (var c in this.phImagenesArticulos.Controls)
                {
                    var tr = c as TableRow;
                    foreach (TableCell cell in tr.Cells)
                    {
                        var check = cell.Controls[1] as CheckBox;
                        if (check.Checked)
                        {
                            this.borrarImagen(check.ID);
                            this.borrarStore(check.ID);
                        }
                    }

                }
                //vuelvo a cargar imagenes
                this.cargarImagenes(this.id.ToString());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error eliminando imagen " + ex.Message), true);
            }
        }
        private void borrarImagen(string imagen)
        {
            try
            {
                string codigo = imagen.Split('_')[0];
                imagen = imagen + ".jpg";
                //string path = Server.MapPath("../../images/Productos/" + codigo + "/");
                string path = Server.MapPath("../../images/Productos/" + this.id + "/");
                File.Delete(path + imagen);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error borran imagen " + imagen + ". " + ex.Message), true);
            }
        }
        private void borrarStore(string imagen)
        {
            try
            {
                //datos del FTP
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Inicio borrar Imagen a store: " + this.txtCodArticulo.Text);

                string codigo = imagen.Split('_')[0];
                imagen = imagen + ".jpg";


                FTPManager theFTP = new FTPManager();
                theFTP.ftpServer = WebConfigurationManager.AppSettings.Get("ImagenesStore") + codigo + "//";
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "URL FTP:" + theFTP.ftpServer);
                theFTP.ftpUser = WebConfigurationManager.AppSettings.Get("Usuario");
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "usuario FTP:" + theFTP.ftpUser);
                theFTP.ftpPass = WebConfigurationManager.AppSettings.Get("pass");
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Contraseña FTP:" + theFTP.ftpPass);
                controladorFunciones fun = new controladorFunciones();

                //borro el archivo de ftp del store
                fun.DeleteFile(theFTP, imagen);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error borran imagen en store " + imagen + ". " + ex.Message), true);
            }
        }

        #endregion

        #region art compuestos
        private void cargarArticulosCompuestos(int idArticulo)
        {
            try
            {
                phArticulosCompuestos.Controls.Clear();
                List<ArticulosCompuestos> articulosCompuestos = this.controlador.obtenerArticulosByArticuloCompuesto(idArticulo);
                decimal costoTotal = 0;
                foreach (ArticulosCompuestos ac in articulosCompuestos)
                {
                    this.cargarArticulosCompuestosPH(ac);
                    costoTotal += Decimal.Round((ac.articulo.costo * ac.cantidad), 2);
                }

                this.txtCostoTotalComposicion.Text = costoTotal.ToString("0.00");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando Grupos. " + ex.Message));
            }
        }
        private void cargarArticulosCompuestosPH(ArticulosCompuestos ac)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celCodigo = new TableCell();
                celCodigo.Text = ac.articulo.codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = ac.articulo.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = ac.cantidad.ToString("0.00");
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celCosto = new TableCell();
                celCosto.Text = ac.articulo.costo.ToString("0.00");
                celCosto.VerticalAlign = VerticalAlign.Middle;
                celCosto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCosto);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditarAC_" + ac.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.EditarArticuloCompuesto);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminarAC_" + ac.id;
                btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.Attributes.Add("data-toggle", "modal");
                //btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                btnEliminar.Click += new EventHandler(this.QuitarArticuloCompuesto);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                //btnEliminar.OnClientClick = "abrirdialog(" + sg.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phArticulosCompuestos.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Grupo en la lista. " + ex.Message));
            }
        }
        protected void DropListArticulosComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtDescripcionArticulo.Text = DropListArticulosComp.SelectedItem.Text;
                this.txtArticuloCompuesto.Text = DropListArticulosComp.SelectedValue;
                this.txtCantidadArticulo.Focus();
            }
            catch
            {

            }
        }
        protected void lbAgregarArticuloComp_Click(object sender, EventArgs e)
        {
            try
            {
                int editar = (int)Session["ArticulosABM_EditarAc"];
                if (editar == 0)
                {
                    if (this.id != Convert.ToInt32(this.txtArticuloCompuesto.Text))
                    {
                        ArticulosCompuestos art = new ArticulosCompuestos();
                        art.articuloCompuesto.id = this.id;
                        art.articulo.id = Convert.ToInt32(this.txtArticuloCompuesto.Text);
                        art.estado = 1;
                        art.cantidad = Convert.ToDecimal(this.txtCantidadArticulo.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                        int i = this.controlador.agregarArticulosCompuestos(art);
                        if (i > 0)
                        {
                            //agrego bien
                            //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo Compuesto agregado con exito", null));
                            this.txtBusqueda.Text = "";
                            this.txtDescripcionArticulo.Text = "";
                            this.txtCantidadArticulo.Text = "";
                            this.lbAvisoCompuesto.Visible = false;
                            this.cargarArticulosCompuestos(this.id);

                        }
                        else
                        {
                            if (i == -2)
                            {
                                this.lbAvisoCompuesto.Text = "El Articulo ya figura en la lista de Articulos Compuestos";
                                this.lbAvisoCompuesto.Visible = true;
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Datos de Proveedor"));

                            }
                        }
                    }
                    else
                    {
                        this.lbAvisoCompuesto.Text = "No se puede ingresar el mismo Articulo como Compuesto";
                        this.lbAvisoCompuesto.Visible = true;
                        this.txtBusqueda.Focus();
                    }
                }
                else
                {
                    ArticulosCompuestos art = new ArticulosCompuestos();
                    art.id = (int)Session["ArticulosABM_idCompuesto"];
                    art.articuloCompuesto.id = this.id;
                    art.articulo.id = Convert.ToInt32(this.txtArticuloCompuesto.Text);
                    art.estado = 1;
                    art.cantidad = Convert.ToDecimal(this.txtCantidadArticulo.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                    int i = this.controlador.modificarArticulosCompuestos(art);
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo Compuesto agregado con exito", null));
                        this.txtBusqueda.Text = "";
                        this.txtDescripcionArticulo.Text = "";
                        this.txtCantidadArticulo.Text = "";
                        this.txtBusqueda.Enabled = true;
                        this.cargarArticulosCompuestos(this.id);

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Articulo Compuesto"));

                    }
                }
            }
            catch
            {

            }
        }
        private void QuitarArticuloCompuesto(object sender, EventArgs e)
        {
            try
            {

                string[] datos = (sender as LinkButton).ID.Split('_');
                string idArticuloCompuesto = datos[1];
                ArticulosCompuestos ac = this.controlador.obtenerArticulosCompuestosID(Convert.ToInt32(idArticuloCompuesto));
                ac.estado = 0;
                int i = this.controlador.modificarArticulosCompuestos(ac);
                if (i > 0)
                {
                    //agrego bien
                    //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo Compuesto eliminado con exito", null));
                    this.cargarArticulosCompuestos(this.id);

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminado Articulo Compuesto"));

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando Articulo Compuesto. " + ex.Message));
            }
        }
        private void EditarArticuloCompuesto(object sender, EventArgs e)
        {
            try
            {
                string[] datos = (sender as LinkButton).ID.Split('_');
                string idArticuloCompuesto = datos[1];
                //obtengo el cliente del session
                ArticulosCompuestos ac = this.controlador.obtenerArticulosCompuestosID(Convert.ToInt32(idArticuloCompuesto));
                this.txtArticuloCompuesto.Text = ac.articulo.id.ToString();
                this.txtBusqueda.Text = ac.articulo.codigo;
                this.txtDescripcionArticulo.Text = ac.articulo.descripcion;
                this.txtCantidadArticulo.Text = ac.cantidad.ToString();
                this.EditarAc = 1;
                this.txtBusqueda.Enabled = false;
                this.idCompuesto = Convert.ToInt32(idArticuloCompuesto);
                Session.Add("ArticulosABM_EditarAc", EditarAc);
                Session.Add("ArticulosABM_IdCompuesto", idCompuesto);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando direccion " + ex.Message));
            }
        }
        #endregion

        #region proveedores art
        private void cargarProveedorArticulos(int idArticulo)
        {
            try
            {
                phProveedorArticulo.Controls.Clear();
                List<ProveedorArticulo> grupos = this.controlador.obtenerProveedorArticulosByArticulo(idArticulo);
                foreach (ProveedorArticulo pa in grupos)
                {
                    this.cargarPHProveedores(pa);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando Otros Prov. " + ex.Message));

            }
        }
        private void cargarPHProveedores(ProveedorArticulo pa)
        {
            try
            {
                TableRow tr = new TableRow();
                                
                tr.ID = "tr_" + pa.id.ToString();


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = pa.proveedor.alias;
                celDescripcion.Width = Unit.Percentage(15);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celNombre = new TableCell();
                celNombre.Text = pa.codigoProveedor;
                celNombre.Width = Unit.Percentage(5);
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + pa.precio.ToString();
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                celPrecio.Width = Unit.Percentage(10);
                tr.Cells.Add(celPrecio);

                TableCell celAlicuota = new TableCell();
                celAlicuota.Text = pa.alicuotaIVA.ToString() + "%";
                celAlicuota.VerticalAlign = VerticalAlign.Middle;
                celAlicuota.Width = Unit.Percentage(5);
                tr.Cells.Add(celAlicuota);

                TableCell celMoneda = new TableCell();
                celMoneda.Text = pa.moneda.moneda;
                //celMoneda.ID = pa.moneda.id.ToString();
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                celMoneda.Width = Unit.Percentage(5);
                tr.Cells.Add(celMoneda);

                TableCell celDtoFinal = new TableCell();
                celDtoFinal.Text = pa.descuentoFinal.ToString() + "%";
                celDtoFinal.VerticalAlign = VerticalAlign.Middle;
                celDtoFinal.HorizontalAlign = HorizontalAlign.Right;
                celDtoFinal.Width = Unit.Percentage(5);
                tr.Cells.Add(celDtoFinal);

                TableCell celPrecioFinal = new TableCell();
                celPrecioFinal.Text = "$ " + pa.precioFinal.ToString();
                celPrecioFinal.VerticalAlign = VerticalAlign.Middle;
                celPrecioFinal.HorizontalAlign = HorizontalAlign.Right;
                celPrecioFinal.Width = Unit.Percentage(10);
                tr.Cells.Add(celPrecioFinal);

                TableCell celPrecioFinalIVA = new TableCell();
                celPrecioFinalIVA.Text = "$ " + Decimal.Round((pa.precioFinal * (1 + (pa.alicuotaIVA / 100))), 2).ToString();
                celPrecioFinalIVA.VerticalAlign = VerticalAlign.Middle;
                celPrecioFinalIVA.HorizontalAlign = HorizontalAlign.Right;
                celPrecioFinalIVA.Width = Unit.Percentage(10);
                tr.Cells.Add(celPrecioFinalIVA);

                TableCell celPrecioPesos = new TableCell();
                celPrecioPesos.Text = "$ " + Decimal.Round((pa.precioPesos * (1 + (pa.alicuotaIVA / 100))), 2).ToString();// pa.precioPesos.ToString();
                celPrecioPesos.VerticalAlign = VerticalAlign.Middle;
                celPrecioPesos.HorizontalAlign = HorizontalAlign.Right;
                celPrecioPesos.Width = Unit.Percentage(10);
                tr.Cells.Add(celPrecioPesos);

                TableCell celCuotas = new TableCell();
                celCuotas.Text = pa.fechaActualizacion.ToString("dd/MM/yyyy");
                celCuotas.VerticalAlign = VerticalAlign.Middle;
                celCuotas.Width = Unit.Percentage(10);
                tr.Cells.Add(celCuotas);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditarPA_" + pa.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.EditarProveedorArticulo);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminarPA_" + pa.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarProveedorArticulo);
                celAction.Controls.Add(btnEliminar);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);


                RadioButton chkProveedor = new RadioButton();
                chkProveedor.ID = "chkProveedor_" + pa.id;
                chkProveedor.CssClass = "btn btn-info";
                chkProveedor.Font.Size = 12;
                chkProveedor.GroupName = "ProvArt";


                celAction.Controls.Add(chkProveedor);

                celAction.Width = Unit.Percentage(15);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                if (DropListProveedor.SelectedValue == pa.proveedor.id.ToString()) //id proveeedor cargado en el ddl == id proveedor en lectura por el ph
                {
                    chkProveedor.Checked = true;
                    tr.ForeColor = Color.Green;
                    tr.Font.Bold = true;

                }

                phProveedorArticulo.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando otros prov en ph. " + ex.Message));
            }
        }
        protected void btnAgregarProvArt_Click(object sender, EventArgs e)
        {
            //agrego o modifico proveedor de articulo
            this.agregarModificarProveedor();
        }
        private ProveedorArticulo cargarProvModificado()
        {
            try
            {
                ProveedorArticulo prov = new ProveedorArticulo();
                prov.proveedor.id = Convert.ToInt32(this.DropListProveedores2.SelectedValue);
                prov.articulo.id = this.id;
                prov.fechaActualizacion = DateTime.Now;
                prov.estado = 1;
                prov.precio = Convert.ToDecimal(this.txtPrecio.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.codigoProveedor = this.txtCodigoProveedor.Text;
                prov.moneda.id = Convert.ToInt32(this.DropListMoneda2.SelectedValue);
                prov.descuento = Convert.ToDecimal(this.txtDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.descuento2 = Convert.ToDecimal(this.txtDescuento2.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.descuento3 = Convert.ToDecimal(this.txtDescuento3.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.descuentoFinal = Convert.ToDecimal(this.txtDescuentoFinal.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.precioFinal = Convert.ToDecimal(this.txtPrecioFinal.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.precioPesos = Convert.ToDecimal(this.txtPrecioPesos.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                prov.alicuotaIVA = Convert.ToDecimal(this.DropListIva.SelectedValue);

                if (this.txtDescripcionDesc.Text == "")
                {
                    prov.descripcionDto = "-";
                }
                else
                {
                    prov.descripcionDto = "-";
                }
                if (this.txtDescripcionDesc2.Text == "")
                {
                    prov.descripcionDto2 = "-";
                }
                else
                {
                    prov.descripcionDto2 = "-";
                }
                if (this.txtDescripcionDesc3.Text == "")
                {
                    prov.descripcionDto3 = "-";
                }
                else
                {
                    prov.descripcionDto3 = "-";
                }

                return prov;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clase ProveedorArticulo. " + ex.Message));
                return null;
            }

        }
        private void limpiarCampos()
        {
            this.DropListProveedores2.SelectedValue = "-1";
            this.txtPrecioFinal.Text = "";
            this.txtCodigoProveedor.Text = "";
            this.DropListMoneda2.SelectedValue = "-1";
            this.cargarProveedorArticulos(this.id);
            this.txtPrecio.Text = "0";
            this.txtPrecioPesos.Text = "0";
            this.txtDescuento.Text = "0";
            this.txtDescuento2.Text = "0";
            this.txtDescuento3.Text = "0";
            this.txtDescuentoFinal.Text = "0";
            this.DropListIva.SelectedValue = "-1";
        }
        private void agregarModificarProveedor()
        {
            try
            {
                int editar = (int)Session["ArticulosABM_EditarPa"];
                //si edita== 0 esta agregando un nuevo proveedor, sino modificando
                if (editar == 0)
                {
                    ProveedorArticulo prov = cargarProvModificado();
                    int i = this.controlador.agregarProveedorArticulo(prov);
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Tarjeta : " + t.nombre);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Datos de Proveedor cargado con exito", null));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Datos de Proveedor cargado con exito\", {type: \"info\"});", true);
                        limpiarCampos();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Error cargando Datos de Proveedor.\", {type: \"error\"});", true);
                    }
                }
                else
                {
                    //edita
                    ProveedorArticulo prov = cargarProvModificado();
                    prov.id = (int)Session["ArticulosABM_idProvArt"];
                    int i = this.controlador.modificarProveedorArticulo(prov);
                    if (i > 0)
                    {
                        //agrego el editado bien
                        //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Tarjeta : " + t.nombre);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Datos de Proveedor cargado con exito\", {type: \"info\"});", true);
                        limpiarCampos();
                        int vaciarEditar = 0;
                        Session.Add("ArticulosABM_EditarPa", vaciarEditar);
                    }
                    else
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Datos de Proveedor"));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Error cargando Datos de Proveedor.\", {type: \"error\"});", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Error guardando datos de proveedor." + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        private void RecalcularCostoArticulo(int idProv)
        {
            try
            {
                ProveedorArticulo pa = this.controlador.obtenerProveedorArticuloID(idProv);
                if (pa.articulo.id == this.id) //articulo del proveedor buscado con el id del articulo en edicion.
                {
                    DropListProveedor.SelectedValue = pa.proveedor.id.ToString();
                    txtCosto.Text = pa.precioFinal.ToString();
                    DropDownMonedaVent.SelectedValue = pa.moneda.id.ToString();
                    this.txtCosto.Focus();
                    this.calcularPrecioDesdeCosto();
                    this.cargarProveedorArticulos(this.id);
                    UpdatePanel2.Update();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxError("Error calculando costos proveedor predeterminado. " + ex.Message), true);
            }
        }
        private void QuitarProveedorArticulo(object sender, EventArgs e)
        {
            try
            {
                string idArticuloCompuesto = (sender as LinkButton).ID.Split('_')[1]; //.Substring(14, 3);
                ProveedorArticulo pa = this.controlador.obtenerProveedorArticuloID(Convert.ToInt32(idArticuloCompuesto));
                pa.estado = 0;
                int i = this.controlador.modificarProveedorArticulo(pa);
                if (i > 0)
                {
                    //agrego bien
                    //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Prov articulo eliminado con exito", null));
                    this.cargarArticulosCompuestos(this.id);
                    UpdatePanel3.Update();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminado otro Prov articulo"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando otro Prov articulo. " + ex.Message));
            }
        }
        private void EditarProveedorArticulo(object sender, EventArgs e)
        {
            try
            {
                string idArticuloCompuesto = (sender as LinkButton).ID.Split('_')[1];
                //obtengo el cliente del session
                ProveedorArticulo pa = this.controlador.obtenerProveedorArticuloID(Convert.ToInt32(idArticuloCompuesto));
                this.DropListProveedores2.SelectedValue = pa.proveedor.id.ToString();
                this.txtCodigoProveedor.Text = pa.codigoProveedor;
                this.txtPrecioFinal.Text = pa.precioFinal.ToString();
                this.DropListMoneda2.SelectedValue = pa.moneda.id.ToString();
                cargarCotizacion(this.DropListMoneda2);
                this.txtPrecio.Text = pa.precio.ToString();
                this.txtPrecioPesos.Text = pa.precioPesos.ToString();
                this.txtDescuento.Text = pa.descuento.ToString();
                this.txtDescuento2.Text = pa.descuento2.ToString();
                this.txtDescuento3.Text = pa.descuento3.ToString();
                this.txtDescuentoFinal.Text = pa.descuentoFinal.ToString();
                this.DropListIva.SelectedValue = pa.alicuotaIVA.ToString();
                this.EditarPa = 1;
                this.idProvArt = Convert.ToInt32(idArticuloCompuesto);
                Session.Add("ArticulosABM_EditarPa", EditarPa);
                Session.Add("ArticulosABM_IdProvArt", idProvArt);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando direccion " + ex.Message));
            }
        }
        private void calcularDescuento(List<decimal> descuentos)
        {
            try
            {
                //decimal total = 0;
                decimal auxDesc = 100;
                decimal desc2 = 0;
                decimal descTotal = 0;
                //recorro los descuentos y sumos
                foreach (var item in descuentos)
                {
                    decimal desc = item;
                    desc = desc / 100;
                    //cargo el valor 
                    desc2 = auxDesc - (auxDesc * desc);

                    auxDesc = desc2;
                }
                descTotal = 100 - auxDesc;
                this.txtDescuentoFinal.Text = descTotal.ToString("N");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error calculando descuento. " + ex.Message), true);
            }
        }
        private void calcularCotizacion()
        {
            try
            {
                decimal cz = Convert.ToDecimal(this.ltCotizacion.Text);
                decimal precio = Convert.ToDecimal(this.txtPrecio.Text);
                decimal desc = Convert.ToDecimal(this.txtDescuentoFinal.Text);
                decimal total, precioFinal;

                if (desc > 0) //Calcula el precio final con descuento final incluido.
                {
                    total = precio * (desc / 100);
                    total = decimal.Round(total, 2);
                    precioFinal = precio - total;
                    this.txtPrecioFinal.Text = Convert.ToString(precioFinal);
                }
                else
                {
                    precioFinal = precio;
                    this.txtPrecioFinal.Text = this.txtPrecio.Text;
                }

                this.txtPrecioPesos.Text = Convert.ToString(decimal.Round((precioFinal * cz), 2));


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error calculando precio final. " + ex.Message), true);
            }
        }
        protected void btnCalcularDesc_Click(object sender, EventArgs e)
        {
            try
            {
                List<decimal> descuentos = new List<decimal>();
                descuentos.Add(Convert.ToDecimal(txtDescuento.Text));
                descuentos.Add(Convert.ToDecimal(txtDescuento2.Text));
                descuentos.Add(Convert.ToDecimal(txtDescuento3.Text));

                calcularDescuento(descuentos);
                calcularCotizacion();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelStore, UpdatePanelStore.GetType(), "alert", m.mensajeBoxError("Error calculando descuento final. " + ex.Message), true);
            }


        }
        private void cargarCotizacion(DropDownList list)
        {

            string cotizacion = contCobranza.obtenerCotizacion(Convert.ToInt32(list.SelectedValue)).ToString().Replace(',', '.');
            this.ltCotizacion.Text = cotizacion;
            this.LitCotizacionCosto.Text = cotizacion;
        }

        protected void DropListMoneda2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //txtCotizacion.Text = contCobranza.obtenerCotizacion(Convert.ToInt32(this.DropListMoneda2.SelectedValue)).ToString().Replace(',', '.');
                cargarCotizacion(DropListMoneda2);
                txtPrecio.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo cotización. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error obteniendo cotizacion al realizar cobro. " + ex.Message);
            }
        }
        protected void btnSetearProv_Click(object sender, EventArgs e)
        {
            try //Recorro las filas que tiene el PH y busco la que tiene el radiobutton checked. tr.ID == id de Proveedorarticulo.
            {
                foreach (var item in phProveedorArticulo.Controls)
                {
                    TableRow tr = (TableRow)item;
                    RadioButton ck = (RadioButton)tr.Cells[10].Controls[4];

                    if (ck.Checked == true)
                    {
                        var idTr = tr.ID.Split('_');
                        RecalcularCostoArticulo(Convert.ToInt32(idTr[1]));
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxInfo("Proveedor asignado!. ", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxError("Error asignando proveedor de articulo. " + ex.Message), true);
            }
        }

        #endregion

        #region dto articulos
        protected void lbtnAgregarDto_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                Gestion_Api.Entitys.articulo artDto = contArtEntity.obtenerArticuloEntity(this.id);
                Gestion_Api.Entitys.Articulos_Descuentos dto = new Articulos_Descuentos();

                dto.Desde = Convert.ToDecimal(this.txtCantDesde.Text);
                dto.Hasta = Convert.ToDecimal(this.txtCantHasta.Text);
                dto.Descuento = Convert.ToDecimal(this.txtDescuentoCantidad.Text);

                artDto.Articulos_Descuentos.Add(dto);

                int i = contArtEntity.agregarDescuentoCantidad(artDto);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Descuento agregado con exito\", {type: \"info\"});", true);
                    this.cargarDescuentos();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Ocurrio un error agregando dto.\");", true);
                }                
                
            }
            catch (Exception ex)
            {

            }
        }

        public void cargarDescuentos()
        {
            try
            {
                this.phDescuentos.Controls.Clear();

                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Gestion_Api.Entitys.articulo artDto = contArtEntity.obtenerArticuloEntity(this.id);

                foreach (Gestion_Api.Entitys.Articulos_Descuentos dto in artDto.Articulos_Descuentos)
                {
                    this.cargarDescuentosPH(dto,artDto.codigo);    
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void cargarDescuentosPH(Gestion_Api.Entitys.Articulos_Descuentos dto,string codigo)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celCodigo = new TableCell();
                celCodigo.Text = codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDesde = new TableCell();
                celDesde.Text = dto.Desde.ToString();
                celDesde.VerticalAlign = VerticalAlign.Middle;
                celDesde.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDesde);

                TableCell celHasta = new TableCell();
                celHasta.Text = dto.Hasta.ToString();
                celHasta.VerticalAlign = VerticalAlign.Middle;
                celHasta.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHasta);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = dto.Descuento + "%";
                celDescuento.VerticalAlign = VerticalAlign.Middle;
                celDescuento.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDescuento);

                TableCell celAction = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminarDTO_" + dto.IdArticulo + "_" + dto.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarDescuentoArticulo);
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                this.phDescuentos.Controls.Add(tr);
            }
            catch
            {

            }
        }
        private void QuitarDescuentoArticulo(object sender, EventArgs e)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                string[] datos = (sender as LinkButton).ID.Split('_');
                string idArticulo = datos[1];
                string idDtoArticulo = datos[2];


                int i = contArtEntity.eliminarDescuentoCantidad(Convert.ToInt32(idArticulo), Convert.ToInt32(idDtoArticulo));
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja de descuento por cantidad Articulo: " +this.id);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Descuento de articulo eliminado con exito\", {type: \"info\"});", true);                    
                    this.cargarDescuentos();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Ocurrio un error eliminando desc.\");", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Ocurrio un error eliminando dto.\");", true);
            }
        }
        
        #endregion

        #region datos despachos

        private void guardarDatosDespacho(int idArticulo)
        {
            try
            {
                if (this.accion == 1)//alta articulo
                {
                    if (!this.DropListPais.SelectedItem.Text.Contains("Argentina") && !this.DropListPais.SelectedItem.Text.Contains("NACIONAL"))
                    {
                        //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                        Gestion_Api.Entitys.articulo artEntity = contArtEnt.obtenerArticuloEntity(idArticulo);
                        Articulos_Despachos datosDespacho = new Articulos_Despachos();
                        datosDespacho.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                        datosDespacho.NumeroDespacho = this.txtNumeroDespacho.Text;
                        datosDespacho.Lote = this.txtLote.Text;
                        datosDespacho.Vencimiento = this.txtVencimiento.Text;

                        artEntity.Articulos_Despachos.Add(datosDespacho);
                        contArtEnt.guardarDatosDespacho(artEntity);
                    }
                }
                else//modificar art
                {
                    if (!this.DropListPais.SelectedItem.Text.Contains("Argentina") && !this.DropListPais.SelectedItem.Text.Contains("NACIONAL"))
                    {
                        //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                        Gestion_Api.Entitys.articulo artEntity = contArtEnt.obtenerArticuloEntity(idArticulo);
                        if (artEntity.Articulos_Despachos.Count > 0)
                        {
                            artEntity.Articulos_Despachos.FirstOrDefault().FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                            artEntity.Articulos_Despachos.FirstOrDefault().NumeroDespacho = this.txtNumeroDespacho.Text;
                            artEntity.Articulos_Despachos.FirstOrDefault().Lote = this.txtLote.Text;
                            artEntity.Articulos_Despachos.FirstOrDefault().Vencimiento = this.txtVencimiento.Text;
                        }
                        else
                        {
                            Articulos_Despachos datosDespacho = new Articulos_Despachos();
                            datosDespacho.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                            datosDespacho.NumeroDespacho = this.txtNumeroDespacho.Text;
                            datosDespacho.Lote = this.txtLote.Text;
                            datosDespacho.Vencimiento = this.txtVencimiento.Text;
                            artEntity.Articulos_Despachos.Add(datosDespacho);                            
                        }
                        contArtEnt.guardarDatosDespacho(artEntity);
                    }
                }
            }
            catch
            {

            }
        }
        private void cargarDatosDespacho(int idArticulo)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(idArticulo);

                if (art.Articulos_Despachos.Count > 0)
                {
                    var datos = art.Articulos_Despachos.FirstOrDefault();
                    this.txtFechaDespacho.Text = datos.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    this.txtNumeroDespacho.Text = datos.NumeroDespacho.ToString();
                    this.txtVencimiento.Text = datos.Lote.ToString();
                    this.txtLote.Text = datos.Vencimiento.ToString();
                }

            }
            catch
            {

            }
        }
        protected void DropListPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListPais.SelectedItem.Text != "Argentina" && this.DropListPais.SelectedItem.Text != "NACIONAL")
                {
                    this.panelDespacho.Visible = true;
                }
                else
                {
                    this.panelDespacho.Visible = false;
                }
            }
            catch
            {

            }
        }

        #endregion

        #region presentaciones
        protected void ListPresentaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ListPresentaciones.SelectedValue == "SI")
                {
                    this.panelPresentaciones.Visible = true;
                }
                else
                {
                    this.panelPresentaciones.Visible = false;
                }
            }
            catch
            {

            }
        }

        private void guardarDatosPresentaciones(int idArticulo)
        {
            try
            {
                if (this.accion == 1)//alta articulo
                {
                    if (this.ListPresentaciones.SelectedValue == "SI")
                    {
                        //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                        Gestion_Api.Entitys.articulo artEntity = contArtEnt.obtenerArticuloEntity(idArticulo);

                        Articulos_Presentaciones datosPresentacion = new Articulos_Presentaciones();
                        datosPresentacion.Minima = this.txtPresentacionMin.Text;
                        datosPresentacion.Media = this.txtPresentacionMed.Text;
                        datosPresentacion.Maxima = this.txtPresentacionMax.Text;

                        artEntity.Articulos_Presentaciones.Add(datosPresentacion);
                        contArtEnt.agregarPresentacionArticulo(artEntity);
                    }
                }
                else//modificacion art
                {
                    if (this.ListPresentaciones.SelectedValue == "SI")
                    {
                        //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                        Gestion_Api.Entitys.articulo artEntity = contArtEnt.obtenerArticuloEntity(idArticulo);
                        if (artEntity.Articulos_Presentaciones.Count > 0)
                        {
                            artEntity.Articulos_Presentaciones.FirstOrDefault().Minima = this.txtPresentacionMin.Text;
                            artEntity.Articulos_Presentaciones.FirstOrDefault().Media = this.txtPresentacionMed.Text;
                            artEntity.Articulos_Presentaciones.FirstOrDefault().Maxima = this.txtPresentacionMax.Text;                            
                        }
                        else
                        {
                            Articulos_Presentaciones datosPresentacion = new Articulos_Presentaciones();
                            datosPresentacion.Minima = this.txtPresentacionMin.Text;
                            datosPresentacion.Media = this.txtPresentacionMed.Text;
                            datosPresentacion.Maxima = this.txtPresentacionMax.Text;
                            artEntity.Articulos_Presentaciones.Add(datosPresentacion);                            
                        }
                        contArtEnt.agregarPresentacionArticulo(artEntity);
                    }
                }
            }
            catch
            {

            }
        }
        private void cargarDatosPresentaciones(int idArticulo)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Gestion_Api.Entitys.articulo art = contArtEntity.obtenerArticuloEntity(idArticulo);

                if (art.Articulos_Presentaciones.Count > 0)
                {
                    this.panelPresentaciones.Visible = true;
                    this.ListPresentaciones.SelectedValue = "SI";
                    var datos = art.Articulos_Presentaciones.FirstOrDefault();
                    this.txtPresentacionMin.Text = datos.Minima;
                    this.txtPresentacionMed.Text = datos.Media;
                    this.txtPresentacionMax.Text = datos.Maxima;
                }
            }
            catch
            {

            }
        }

        #endregion

        #region combustibles
        public void cargarDatosCombustibles()
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                this.phDatosCombustible.Controls.Clear();
                List<Articulos_Combustible> datos = contArtEntity.obtenerDatosCombustibleByArticulo(this.id);

                foreach (var d in datos)
                {
                    this.cargarCombustiblesPH(d);
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void cargarCombustiblesPH(Articulos_Combustible datos)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                TableRow tr = new TableRow();
                tr.ID = datos.Id.ToString();

                TableCell celProveedor = new TableCell();
                celProveedor.Text = contCliente.obtenerProveedorID(datos.Proveedor.Value).razonSocial;
                celProveedor.HorizontalAlign = HorizontalAlign.Left;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celProveedor);

                TableCell celITC = new TableCell();
                celITC.Text = "$" + datos.ITC.Value.ToString("");
                celITC.HorizontalAlign = HorizontalAlign.Right;
                celITC.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celITC);

                TableCell celTasaHidrica = new TableCell();
                celTasaHidrica.Text = "$" + datos.TasaHidrica.Value.ToString("");
                celTasaHidrica.HorizontalAlign = HorizontalAlign.Right;
                celTasaHidrica.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTasaHidrica);

                TableCell celTasaVial = new TableCell();
                celTasaVial.Text = "$" + datos.TasaVial.Value.ToString("");
                celTasaVial.HorizontalAlign = HorizontalAlign.Right;
                celTasaVial.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTasaVial);

                TableCell celTasaMunicipal = new TableCell();
                celTasaMunicipal.Text = "$" + datos.TasaMunicipal.Value.ToString("");
                celTasaMunicipal.HorizontalAlign = HorizontalAlign.Right;
                celTasaMunicipal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTasaMunicipal);

                TableCell celFechaMod = new TableCell();
                celFechaMod.Text = datos.FechaModificacion.Value.ToString("dd/MM/yyyy hh:mm:ss");
                celFechaMod.HorizontalAlign = HorizontalAlign.Left;
                celFechaMod.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFechaMod);

                TableCell celAccion = new TableCell();
                LinkButton btnBorrar = new LinkButton();
                btnBorrar.ID = "btnBorrar_" + datos.Id.ToString();
                btnBorrar.CssClass = "btn btn-info";
                btnBorrar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnBorrar.Click += new EventHandler(this.QuitarDatoCombustible);
                celAccion.Controls.Add(btnBorrar);

                tr.Cells.Add(celAccion);

                this.phDatosCombustible.Controls.Add(tr);

            }
            catch
            {

            }
        }
        public void agregarDatosCombustible()
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                Articulos_Combustible datos = new Articulos_Combustible();

                datos.IdArticulo = this.id;
                datos.ITC = decimal.Round(Convert.ToDecimal(this.txtCombustibleITC.Text), 8);
                datos.TasaHidrica = decimal.Round(Convert.ToDecimal(this.txtCombustibleTasaHidrica.Text), 8);
                datos.TasaMunicipal = decimal.Round(Convert.ToDecimal(this.txtCombustibleTasaMunicipal.Text), 8);
                datos.TasaVial = decimal.Round(Convert.ToDecimal(this.txtCombustibleTasaVial.Text), 8);
                datos.Proveedor = Convert.ToInt32(this.ListProveedorCombustible.SelectedValue);
                datos.FechaModificacion = DateTime.Now;

                int i = contArtEntity.agregarDatosCombustible(datos);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito!. \", {type: \"info\"});", true);
                    this.cargarDatosCombustibles();
                    this.limpiarCamposCombustible();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"No se pudieron guardar los datos.\");", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"Ocurrio un error intentando guardar datos. " + ex.Message + " .\", {type: \"error\"});", true);
            }
        }
        public void limpiarCamposCombustible()
        {
            try
            {
                this.txtCombustibleITC.Text = "";
                this.txtCombustibleTasaHidrica.Text = "";
                this.txtCombustibleTasaMunicipal.Text = "";
                this.txtCombustibleTasaVial.Text = "";
                this.ListProveedorCombustible.SelectedValue = "-1";
            }
            catch
            {

            }
        }
        private void QuitarDatoCombustible(object sender, EventArgs e)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                string[] datos = (sender as LinkButton).ID.Split('_');                
                string idDatos = datos[1];

                int i = contArtEntity.quitarDatosCombustible(Convert.ToInt32(idDatos));
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja de datos combustible de Articulo: " + this.id);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"Datos eliminados con exito\", {type: \"info\"});", true);
                    this.cargarDatosCombustibles();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"No se pudo eliminar datos combustible.\");", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel6, UpdatePanel6.GetType(), "alert", "$.msgbox(\"Ocurrio un error intentando eliminar datos. " + ex.Message + " .\", {type: \"error\"});", true);
            }
        }
        protected void btnAgregarDatosCombustible_Click(object sender, EventArgs e)
        {
            try
            {
                this.agregarDatosCombustible();
            }
            catch
            {

            }
        }
        #endregion

        #region costos
        public void cargarHistorialCostos()
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                this.phCostos.Controls.Clear();

                decimal anterior = 0;

                List<Articulos_Costos> costos = contArtEntity.obtenerCostosArticulo(this.id);
                foreach (var c in costos)
                {
                    this.cargarHistorialCostosPH(c, anterior,0);
                    anterior = c.Costo.Value;
                }

                //cargo el costo actual para tener referencia
                Articulos_Costos ac = new Articulos_Costos();
                ac.articulo = contArtEntity.obtenerArticuloEntity(this.id);
                ac.Costo = Convert.ToDecimal(this.txtCosto.Text);                
                ac.Proveedor = ac.articulo.proveedor;
                ac.Fecha = DateTime.Now;
                this.cargarHistorialCostosPH(ac, anterior,1);

            }
            catch
            {

            }
        }
        public void cargarHistorialCostosPH(Articulos_Costos c,decimal anterior,int costoActual)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                TableRow tr = new TableRow();
                tr.ID = c.Id.ToString();

                TableCell celProveedor = new TableCell();
                celProveedor.Text = contCliente.obtenerProveedorID(c.Proveedor.Value).razonSocial;
                celProveedor.HorizontalAlign = HorizontalAlign.Left;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celProveedor);

                TableCell celFecha = new TableCell();
                celFecha.Text = c.Fecha.Value.ToString("dd/MM/yyyy hh:mm:ss");
                if (costoActual == 1)
                    celFecha.Text = "ACTUAL";
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celCosto = new TableCell();
                celCosto.Text = "$" + c.Costo.Value.ToString();
                celCosto.HorizontalAlign = HorizontalAlign.Right;
                celCosto.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCosto);

                TableCell celMoneda = new TableCell();
                if (costoActual == 1)
                    celMoneda.Text = this.DropDownMonedaVent.SelectedItem.Text;
                else
                    celMoneda.Text = c.moneda1.moneda1;
                celMoneda.HorizontalAlign = HorizontalAlign.Left;
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMoneda);

                TableCell celPorcentaje = new TableCell();
                if (anterior > 0)
                    celPorcentaje.Text = decimal.Round((((c.Costo.Value * 100 / anterior) / 100) - 1) * 100, 2).ToString() + " %";
                else
                    celPorcentaje.Text = "0 %";
                celPorcentaje.HorizontalAlign = HorizontalAlign.Right;
                celPorcentaje.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPorcentaje);

                //TableCell celAccion = new TableCell();
                //LinkButton btnBorrar = new LinkButton();
                //btnBorrar.ID = "btnBorrar_" + c.Id.ToString();
                //btnBorrar.CssClass = "btn btn-info";
                //btnBorrar.Text = "<span class='shortcut-icon icon-trash'></span>";
                ////btnBorrar.Click += new EventHandler(this.QuitarDatoCombustible);
                //celAccion.Controls.Add(btnBorrar);
                //tr.Cells.Add(celAccion);

                if (costoActual == 1)
                {
                    tr.ForeColor = System.Drawing.Color.ForestGreen;
                    tr.Font.Bold = true;
                }
                this.phCostos.Controls.Add(tr);

            }
            catch
            {

            }
        }
        
        public void guardarHistorialCosto(Articulo art)
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

                var a = contArtEntity.obtenerArticuloEntity(art.id);
                
                if(a.costo != art.costo)
                {
                    Articulos_Costos reg = new Articulos_Costos();
                    reg.IdArticulo = art.id;
                    reg.Proveedor = art.proveedor.id;
                    reg.Costo = a.costo;//guardo el costo viejo
                    reg.Fecha = DateTime.Now;
                    reg.Moneda = art.monedaVenta.id;
                    contArtEntity.agregarArticuloCosto(reg);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region datos extras
        private void cargarDatosExtras()
        {
            try
            {
                //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                int i = contArtEnt.obtenerSiDatosExtra(this.id);
                if (i > 0)
                    this.PanelDatosExtra.Visible = true;
                else
                    this.PanelDatosExtra.Visible = false;

                this.ListSiDatosExtra.SelectedValue = i.ToString();
            }
            catch
            {

            }
        }
        protected void btnAgregarSiDatosExtra_Click(object sender, EventArgs e)
        {
            try
            {
                //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                //si ya esta creado el si/no en la base modifico o agrego
                if (contArtEnt.obtenerDatosExtra(this.id) != null)
                {
                    int i = contArtEnt.modificarSiDatosExtra(this.id, Convert.ToInt32(this.ListSiDatosExtra.SelectedValue));
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelExtras, UpdatePanelExtras.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito!. \", {type: \"info\"});", true);                        
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelExtras, UpdatePanelExtras.GetType(), "alert", "$.msgbox(\"No se pudieron guardar los datos.\");", true);
                    }
                }
                else
                {
                    int i = contArtEnt.agregarSiDatosExtra(this.id, Convert.ToInt32(this.ListSiDatosExtra.SelectedValue));
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelExtras, UpdatePanelExtras.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito!. \", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelExtras, UpdatePanelExtras.GetType(), "alert", "$.msgbox(\"No se pudieron guardar los datos.\");", true);
                    }
                }

                if (this.ListSiDatosExtra.SelectedValue == "1")
                    this.PanelDatosExtra.Visible = true;
                else
                    this.PanelDatosExtra.Visible = false;
            }
            catch
            {

            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                this.buscarDatosExtras();
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelBusqueda, UpdatePanelBusqueda.GetType(), "alert", "clickTab();cerrarModalBusqueda();", true);                
            }
            catch
            {

            }
        }
        private void buscarDatosExtras()
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                //ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();

                DateTime desde = Convert.ToDateTime(this.txtFechaDesdeExtra.Text,new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.txtFechaHastaExtra.Text,new CultureInfo("es-AR"));
                int sucursal = Convert.ToInt32(this.ListSucursalesExtra.SelectedValue);

                this.phDatosExtras.Controls.Clear();

                List<Articulos_DatosExtra> datos = contArtEnt.buscarDatosExtraArticulo(this.id);
                foreach (var d in datos)
                {
                    Factura f = contFact.obtenerFacturaId(d.Documento.Value);
                    if (f != null)
                    {
                        if (f.fecha > desde && f.fecha < hasta && (f.sucursal.id == sucursal || sucursal == 0))
                        {
                            TableRow tr = new TableRow();
                            tr.ID = d.Id.ToString();

                            TableCell celFecha = new TableCell();
                            celFecha.Text = f.fecha.ToString("dd/MM/yyyy");
                            celFecha.HorizontalAlign = HorizontalAlign.Center;
                            celFecha.VerticalAlign = VerticalAlign.Middle;
                            celFecha.Width = Unit.Percentage(25);
                            tr.Cells.Add(celFecha);

                            TableCell celDocumento = new TableCell();
                            celDocumento.Text = f.tipo.tipo + " Nº " + f.numero;
                            celDocumento.HorizontalAlign = HorizontalAlign.Center;
                            celDocumento.VerticalAlign = VerticalAlign.Middle;
                            celDocumento.Width = Unit.Percentage(25);
                            tr.Cells.Add(celDocumento);

                            TableCell celDatos = new TableCell();
                            celDatos.Text = d.Datos;
                            celDatos.HorizontalAlign = HorizontalAlign.Center;
                            celDatos.VerticalAlign = VerticalAlign.Middle;
                            celDatos.Width = Unit.Percentage(50);
                            tr.Cells.Add(celDatos);


                            this.phDatosExtras.Controls.Add(tr);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        #endregion

        #region unidades venta
        private void cargarMedidasVentaArticulo(int idArt)
        {
            try
            {
                
                List<Articulos_MedidasVenta> lstMedidas = contArtEnt.obtenerMedidasVentaArticulo(idArt);
                if (lstMedidas != null)
                {
                    this.phMedidas.Controls.Clear();
                    foreach (var m in lstMedidas)
                    {
                        TableRow tr = new TableRow();

                        TableCell celCodigo = new TableCell();
                        celCodigo.Text = m.articulo.codigo;
                        tr.Cells.Add(celCodigo);

                        TableCell celMedida = new TableCell();
                        celMedida.Text = m.Medida;
                        tr.Cells.Add(celMedida);

                        TableCell celCodBarra = new TableCell();
                        celCodBarra.Text = m.CodigoBarra;
                        tr.Cells.Add(celCodBarra);

                        TableCell celCantidad = new TableCell();
                        celCantidad.Text = m.Cantidad.Value.ToString();
                        tr.Cells.Add(celCantidad);

                        TableCell celAccion = new TableCell();
                        LinkButton btnEliminarMedida = new LinkButton();
                        btnEliminarMedida.ID = "btnEliminarMedida_" + m.Id;
                        btnEliminarMedida.CssClass = "btn btn-info ui-tooltip";
                        btnEliminarMedida.Text = "<i class='shortcut-icon icon-trash'></i>";
                        btnEliminarMedida.Click += new EventHandler(this.QuitarUnidadMedida);
                        celAccion.Controls.Add(btnEliminarMedida);
                        tr.Cells.Add(celAccion);

                        this.phMedidas.Controls.Add(tr);
                    }
                }
            }
            catch
            {

            }
        }
        private void QuitarUnidadMedida(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID;
                int idMedidad = Convert.ToInt32(id.Split('_')[1]);

                var m = this.contArtEnt.obtenerMedidasVentaID(idMedidad);
                int ok = this.contArtEnt.eliminaMedidaVenta(m);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMedidas, UpdatePanelMedidas.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito\", {type: \"info\"});", true);
                    this.cargarMedidasVentaArticulo(this.id);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMedidas, UpdatePanelMedidas.GetType(), "alert", "$.msgbox(\"No se pudo eliminar.\");", true);
                }  
            }
            catch(Exception ex)
            {

            }
        }
        protected void lbtnAgregarMedida_Click(object sender, EventArgs e)
        {
            try
            {
                Articulos_MedidasVenta medida = new Articulos_MedidasVenta();
                medida.IdArticulo = this.id;
                medida.Medida = this.txtMedidaNombre.Text;
                medida.Cantidad = Convert.ToDecimal(this.txtMedidaCantidad.Text);
                medida.CodigoBarra = this.txtCodigoBarraMedida.Text;

                int ok = this.contArtEnt.agregarMedidaVenta(medida);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMedidas, UpdatePanelMedidas.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito\", {type: \"info\"});", true);
                    this.txtMedidaNombre.Text = "";
                    this.txtMedidaCantidad.Text = "";
                    this.txtCodigoBarraMedida.Text = "";
                    this.cargarMedidasVentaArticulo(this.id);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMedidas, UpdatePanelMedidas.GetType(), "alert", "$.msgbox(\"No se pudo agregar.\");", true);
                }           

            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMedidas, UpdatePanelMedidas.GetType(), "alert", "$.msgbox(\"Ocurrio un error\", {type: \"error\"});", true);
            }
        }
        #endregion

        #region beneficios
        protected void lbtnAgregarBeneficios_Click(object sender, EventArgs e)
        {
            try
            {
                var existe = this.contArtEnt.obtenerArticuloBeneficioByArticulo(this.id);
                int ok = 0;
                if (existe != null)//modifico
                {                    
                    existe.Beneficios = Convert.ToInt32(this.ListSiBeneficios.SelectedValue);
                    existe.Estado = Convert.ToInt32(this.ListSiBeneficios.SelectedValue);
                    existe.EsDestacado = Convert.ToInt32(this.ListSiDestacadoBeneficios.SelectedValue);
                    existe.EsOferta = Convert.ToInt32(this.ListSiOfertaBeneficios.SelectedValue);
                    existe.DesdeOferta = Convert.ToDateTime(this.txtBeneficiosDesde.Text, new CultureInfo("es-AR"));
                    existe.HastaOferta = Convert.ToDateTime(this.txtBeneficiosHasta.Text, new CultureInfo("es-AR"));                    
                    existe.PuntoOferta = Convert.ToDecimal(this.txtBeneficiosPuntos.Text);
                    existe.Especificaciones = this.txtBeneficiosEspecificaciones.Text;
                    ok = this.contArtEnt.modificaArticuloBeneficios(existe);
                }
                else//agrego
                {
                    Articulos_Beneficios nuevo = new Articulos_Beneficios();
                    nuevo.IdArticulo = this.id;
                    nuevo.Beneficios = Convert.ToInt32(this.ListSiBeneficios.SelectedValue);
                    nuevo.Estado = Convert.ToInt32(this.ListSiBeneficios.SelectedValue);
                    nuevo.EsOferta = Convert.ToInt32(this.ListSiOfertaBeneficios.SelectedValue);
                    nuevo.EsDestacado = Convert.ToInt32(this.ListSiDestacadoBeneficios.SelectedValue);
                    nuevo.DesdeOferta = Convert.ToDateTime(this.txtBeneficiosDesde.Text, new CultureInfo("es-AR"));
                    nuevo.HastaOferta = Convert.ToDateTime(this.txtBeneficiosHasta.Text, new CultureInfo("es-AR"));
                    nuevo.PuntoOferta = Convert.ToDecimal(this.txtBeneficiosPuntos.Text);
                    nuevo.Especificaciones = this.txtBeneficiosEspecificaciones.Text;
                    ok = this.contArtEnt.agregarArticuloBeneficios(nuevo);
                }

                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelBeneficios, UpdatePanelBeneficios.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito\", {type: \"info\"});", true);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelBeneficios, UpdatePanelBeneficios.GetType(), "alert", "$.msgbox(\"No se pudo agregar.\");", true);
                } 
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelBeneficios, UpdatePanelBeneficios.GetType(), "alert", "$.msgbox(\"Ocurrio un error." + ex.Message + "\");", true);
            }
        }
        private void cargarDatosBeneficios(int idArt)
        {
            try
            {
                var beneficio = this.contArtEnt.obtenerArticuloBeneficioByArticulo(idArt);
                if (beneficio != null)
                {
                    this.ListSiBeneficios.SelectedValue = beneficio.Beneficios.Value.ToString();
                    this.ListSiOfertaBeneficios.SelectedValue = beneficio.EsOferta.ToString();
                    this.txtBeneficiosDesde.Text = beneficio.DesdeOferta.Value.ToString("dd/MM/yyyy");
                    this.txtBeneficiosHasta.Text = beneficio.HastaOferta.Value.ToString("dd/MM/yyyy");
                    this.txtBeneficiosPuntos.Text = beneficio.PuntoOferta.Value.ToString();
                    this.txtBeneficiosEspecificaciones.Text = beneficio.Especificaciones;
                }
            }
            catch(Exception ex)
            {

            }
        }


        #endregion

        #region Otros

        #region Catalogo
        private void cargarCatalogoArticulo()
        {
            try
            {
                string catalogo = this.contArtEnt.obtenerCatalogoByArticulo(this.id);
                if (!string.IsNullOrEmpty(catalogo))
                    this.txtCatalogo.Text = catalogo;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error en cargarCatalogoArticulo. Excepción: " + Ex.Message);
            }
        }
        protected void lbtnAgregarCatalogo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCatalogo.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxError("Debe ingresar algún valor para el campo catálogo."), true);
                    return;
                }

                Articulos_Catalogo artCat = new Articulos_Catalogo();
                artCat.Articulo = this.id;
                artCat.Catalogo = this.txtCatalogo.Text;

                int i = this.contArtEnt.agregarArticulos_Catalogo(artCat);
                
                if (i >= 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", "$.msgbox(\"Los datos del Catálogo del Artículo se cargaron con éxito.\", {type: \"info\"});", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando los datos del Catálogo del Artículo."), true);
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error agregando Catalogo al Articulo. Excepción: " + Ex.Message);
            }
        }
        protected void lbtnEliminarCatalogo_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.contArtEnt.eliminarArticulos_Catalogo(this.id);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", "$.msgbox(\"Los datos del Catálogo del Artículo se eliminaron con éxito.\", {type: \"info\"});", true);
                    this.txtCatalogo.Text = string.Empty;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", "$.msgbox(\"Ocurrió un error eliminando los datos del Catálogo del Artículo.\");", true);
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error eliminando Catalogo del Articulo. Excepción: " + Ex.Message);
            }
        }
        #endregion

        #region COT
        protected void btnAgregarCodCot_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(this.txtCodCot.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", m.mensajeBoxError("Debe ingresar algún valor para el campo Codigo COT."), true);
                    return;
                }

                var marca = this.contArtEnt.obtenerMarcaByArticulo(this.id);
                marca.CodigoCot = this.txtCodCot.Text;

                int i = this.contArtEnt.modificarMarca(marca);

                if (i >= 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", "$.msgbox(\"Los datos del codigo COT se cargaron con éxito.\", {type: \"info\"});", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando los datos del Codigo COT del Artículo."), true);
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error agregando Catalogo al Articulo. Excepción: " + Ex.Message);
            }

        }
        #endregion

        #region Aparece en Lista
        private void cargarApareceListaArticulo()
        {
            try
            {
                short apareceLista = this.contArtEnt.obtenerApareceListaByArticulo(this.id);

                if (apareceLista == 0)
                    this.chkApareceLista.Checked = false;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error en cargarApareceListaArticulo. Excepción: " + Ex.Message);
            }
        }
        protected void lbtnApareceLista_Click(object sender, EventArgs e)
        {
            try
            {
                short apareceLista = 1;

                if (!this.chkApareceLista.Checked)
                    apareceLista = 0;

                Articulos_Catalogo artCat = new Articulos_Catalogo();
                artCat.Articulo = this.id;
                artCat.ApareceLista = apareceLista;

                int i = this.contArtEnt.agregarApareceLista(artCat);

                if (i >= 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", "$.msgbox(\"La información del Articulo fué modificada con éxito.\", {type: \"info\"});", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelCatalogo, UpdatePanelCatalogo.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando información del Artículo."), true);

            }
            catch (Exception Ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Ocurrió un error modificando campo ApareceEnLista de Articulos_Catalogo . Excepción: " + Ex.Message);
            }
        }
        #endregion

        #endregion

    }
}