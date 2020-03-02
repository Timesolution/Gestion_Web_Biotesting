using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Promociones
{
    public partial class ABMPromociones : System.Web.UI.Page
    {        
        controladorUsuario contUser = new controladorUsuario();
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorCliente contCliente = new controladorCliente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorTarjeta contTarjeta = new controladorTarjeta();

        Mensajes m = new Mensajes();

        int id;
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();                

                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.id = Convert.ToInt32(Request.QueryString["id"]);

                if (!IsPostBack)
                {
                    this.cargarEmpresas();
                    this.cargarGruposArticulos();
                    this.cargarProveedores();
                    this.cargarArticulos();
                    this.cargarFormaPAgo();
                    this.cargarListaPrecio();
                    this.cargarTarjetas();
                    this.cargarPromocion();   
                }
                this.verficarPermisoGuardar();
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
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "89")
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
        public int verficarPermisoGuardar()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "90")
                        {
                            this.btnGuardar.Visible = true;
                            return 1;
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        #region cargas iniciales
        private void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["Razon Social"] = "Todas";
                dr2["Id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

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
        private void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                // agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                //DataRow dr1 = dt.NewRow();
                //dr1["nombre"] = "Todas";
                //dr1["id"] = 0;
                //dt.Rows.InsertAt(dr1, 1);

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
        private void cargarGruposArticulos()
        {
            try
            {

                DataTable dt = contArticulo.obtenerGruposArticulos();

                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                
                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.ListGrupos.DataSource = dt;
                this.ListGrupos.DataValueField = "id";
                this.ListGrupos.DataTextField = "descripcion";
                this.ListGrupos.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        //private void cargarSubGruposArticulosByGrupo(int Grupo)
        //{
        //    try
        //    {
        //        DataTable dt = this.contArticulo.obtenerSubGruposArticulos(Grupo);

        //        DataRow dr = dt.NewRow();
        //        dr["descripcion"] = "Seleccione...";
        //        dr["Id"] = -1;
        //        dt.Rows.InsertAt(dr, 0);

        //        DataRow dr2 = dt.NewRow();
        //        dr2["descripcion"] = "Todos";
        //        dr2["id"] = 0;
        //        dt.Rows.InsertAt(dr2, 1);

        //        this.ListSubgrupos.DataSource = dt;
        //        this.ListSubgrupos.DataValueField = "id";
        //        this.ListSubgrupos.DataTextField = "descripcion";
        //        this.ListSubgrupos.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
        //    }
        //}
        private void cargarProveedores()
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
                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.ListProveedores.DataSource = dt;
                this.ListProveedores.DataValueField = "id";
                this.ListProveedores.DataTextField = "alias";
                this.ListProveedores.DataBind();
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
                
                DataTable dt = this.contCliente.obtenerListaPrecios();

                this.ListListasPrecio.DataSource = dt;
                this.ListListasPrecio.DataValueField = "id";
                this.ListListasPrecio.DataTextField = "nombre";
                this.ListListasPrecio.DataBind();
                this.ListListasPrecio.Items.Insert(0, new ListItem("Seleccione...","-1"));
                this.ListListasPrecio.Items.Insert(1, new ListItem("Todas", "0"));


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        public void cargarFormaPAgo()
        {
            try
            {
                DataTable dt = this.contFact.obtenerFormasPago();

                this.ListFormasPago.DataSource = dt;
                this.ListFormasPago.DataValueField = "id";
                this.ListFormasPago.DataTextField = "forma";
                this.ListFormasPago.DataBind();
                this.ListFormasPago.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListFormasPago.Items.Insert(1, new ListItem("Todas", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }
        private void cargarArticulos()
        {
            try
            {
                                
                DataTable dt = this.contArticulo.obtenerArticulos2();                

                this.ListArticulos.DataSource = dt;
                this.ListArticulos.DataValueField = "id";
                this.ListArticulos.DataTextField = "descripcion";
                this.ListArticulos.DataBind();
                
                this.ListArticulos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListArticulos.Items.Insert(1, new ListItem("Todos", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + ex.Message));
            }
        }
        private void cargarArticulosProveedor(int idProveedor)
        {
            try
            {
                this.ListArticulos.Items.Clear();
                List<Articulo> articulos = this.contArticulo.obtenerArticulosByProveedorReduc(idProveedor);
                articulos = articulos.OrderBy(x => x.descripcion).ToList();

                foreach (Articulo a in articulos)
                {
                    this.ListArticulos.Items.Add(new ListItem(a.descripcion, a.id.ToString()));
                }

                this.ListArticulos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListArticulos.Items.Insert(1, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos del proveedor. " + ex.Message));
            }
        }
        public void cargarTarjetas()
        {
            try
            {                
                DataTable dt = this.contTarjeta.obtenerTarjetasDT();

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
        #endregion
        public void cargarPromocion()
        {
            try
            {
                Promocione p = this.contArtEnt.obtenerPromocionByID(this.id);
                if (p != null)
                {
                    this.txtPromocion.Text = p.Promocion;
                    this.txtDescuento.Text = p.Descuento.ToString();
                    this.txtPrecio.Text = p.PrecioFijo.ToString();
                    this.ListEmpresa.SelectedValue = p.Empresa.ToString();
                    this.ListProveedores.SelectedValue = p.Proveedor.ToString();
                    this.ListFormasPago.SelectedValue = p.FormaPago.ToString();
                    this.ListListasPrecio.SelectedValue = p.ListaPrecio.ToString();
                    this.txtDesde.Text = p.Desde.Value.ToString("dd/MM/yyyy");
                    this.txtHasta.Text = p.Hasta.Value.ToString("dd/MM/yyyy");
                    this.txtCantidad.Text = p.Cantidad.ToString();
                    if (p.Descuento > 0)
                    {
                        this.PanelDto.Visible = true;
                        this.ListTipoPromocion.SelectedValue = "0";
                    }
                    else
                    {
                        this.PanelPrecio.Visible = true;
                        this.ListTipoPromocion.SelectedValue = "1";
                    }
                    if (this.ListFormasPago.SelectedItem.Text == "Tarjeta")
                    {
                        this.panelTarjetas.Visible = true;
                        this.panelArticulos.Visible = true;
                    }
                    else
                    {
                        this.panelTarjetas.Visible = false;
                        this.panelArticulos.Visible = true;
                    }
                    this.cargarArticulosPromocion(p);
                    this.cargarSucursalesPromocion(p);
                    this.cargarGruposPromocion(p);
                    this.cargarTarjetasPromocion(p);
                }
            }
            catch
            {

            }
        }
        public void cargarTarjetasPromocion(Promocione p)
        {
            try
            {
                foreach (Gestion_Api.Entitys.Tarjeta t in p.Tarjetas)
                {
                    this.ListBoxTarjetas.Items.Add(new ListItem(t.nombre, t.id.ToString()));
                }
            }
            catch
            {

            }
        }
        public void cargarArticulosPromocion(Promocione p)
        {
            try
            {
                foreach (articulo a in p.articulos)
                {
                    this.ListBoxArticulos.Items.Add(new ListItem(a.descripcion, a.id.ToString()));
                }
            }
            catch
            {

            }
        }
        public void cargarSucursalesPromocion(Promocione p)
        {
            try
            {
                foreach (sucursale s in p.sucursales)
                {
                    this.ListBoxSucursales.Items.Add(new ListItem(s.nombre, s.id.ToString()));
                }
            }
            catch
            {

            }
        }
        public void cargarGruposPromocion(Promocione p)
        {
            try
            {
                foreach (gruposArticulo g in p.gruposArticulos)
                {
                    this.ListBoxGrupos.Items.Add(new ListItem(g.descripcion, g.id.ToString()));
                }
            }
            catch
            {

            }
        }
        public void agregarPromo()
        {
            try
            {
                Promocione p = new Promocione();
                p.Promocion = this.txtPromocion.Text;
                p.Empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                p.FormaPago = Convert.ToInt32(this.ListFormasPago.SelectedValue);
                p.ListaPrecio = Convert.ToInt32(this.ListListasPrecio.SelectedValue);
                p.Proveedor = Convert.ToInt32(this.ListProveedores.SelectedValue);
                p.Cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                p.Desde = Convert.ToDateTime(this.txtDesde.Text, new CultureInfo("es-AR"));
                p.Hasta = Convert.ToDateTime(this.txtHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                p.Descuento = Convert.ToDecimal(this.txtDescuento.Text);
                p.PrecioFijo = Convert.ToDecimal(this.txtPrecio.Text);               

                if (this.ListTipoPromocion.SelectedValue == "0")
                    p.PrecioFijo = 0;
                if (this.ListTipoPromocion.SelectedValue == "1")
                    p.Descuento = 0;

                p.articulos = this.obtenerArticulosCargados();
                p.sucursales = this.obtenerSucursalesCargardas();
                p.gruposArticulos = this.obtenerGruposCargados();
                p.Tarjetas = this.obtenerTarjetasCargadas();

                p.Estado = 1;

                int i = this.contArtEnt.agregarPromocion(p);
                if (i > 0)
                {
                    Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Promocion " + p.Promocion + " generado con exito.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Promocion guardada con exito\", {type: \"info\"}); location.href='PromocionesF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo generar promocion. Reintente\");", true);                    
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando promocion." + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        public void modificarPromo()
        {
            try
            {
                Promocione p = this.contArtEnt.obtenerPromocionByID(this.id);                
                p.Promocion = this.txtPromocion.Text;
                p.Empresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                p.FormaPago = Convert.ToInt32(this.ListFormasPago.SelectedValue);
                p.ListaPrecio = Convert.ToInt32(this.ListListasPrecio.SelectedValue);
                p.Proveedor = Convert.ToInt32(this.ListProveedores.SelectedValue);
                p.Cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                p.Desde = Convert.ToDateTime(this.txtDesde.Text, new CultureInfo("es-AR"));
                p.Hasta = Convert.ToDateTime(this.txtHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);
                p.Descuento = Convert.ToDecimal(this.txtDescuento.Text);
                p.PrecioFijo = Convert.ToDecimal(this.txtPrecio.Text);

                if (this.ListTipoPromocion.SelectedValue == "0")
                    p.PrecioFijo = 0;
                if (this.ListTipoPromocion.SelectedValue == "1")
                    p.Descuento = 0;

                p.articulos.Clear();
                p.sucursales.Clear();
                p.gruposArticulos.Clear();
                p.Tarjetas.Clear();

                p.articulos = this.obtenerArticulosCargados();
                p.sucursales = this.obtenerSucursalesCargardas();
                p.gruposArticulos = this.obtenerGruposCargados();
                p.Tarjetas = this.obtenerTarjetasCargadas();

                int i = this.contArtEnt.modificarPromocion(p);
                if (i >= 0)
                {
                    Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Promocion " + p.Id + " modificada con exito.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Promocion modificada con exito\", {type: \"info\"}); location.href='PromocionesF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo modificar promocion. Reintente\");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error guardando promocion." + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private void cargarProducto(string busqueda)
        {
            try
            {
                //Articulo art = this.controlador.obtenerArticuloCodigo(busqueda);
                DataTable dtArticulos = this.contArticulo.buscarArticulosDT(busqueda);

                if (dtArticulos != null)
                {
                    //agrego todos
                    DataRow dr = dtArticulos.NewRow();
                    dr["Descripcion"] = "Seleccione...";
                    dr["id"] = -1;
                    dtArticulos.Rows.InsertAt(dr, 0);

                    this.ListArticulos.DataSource = dtArticulos;
                    this.ListArticulos.DataValueField = "id";
                    this.ListArticulos.DataTextField = "Descripcion";
                    this.ListArticulos.DataBind();
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
        private void cargarArticulosByGrupoProv(int grupo, int prov)
        {
            try
            {
                this.ListArticulos.Items.Clear();
                DataTable dt = this.contArticulo.obtenerArticulosByGrupoSubgrupoProvDias(grupo, 0, prov, null,0,null);

                this.ListArticulos.DataSource = dt;
                this.ListArticulos.DataValueField = "id";
                this.ListArticulos.DataTextField = "descripcion";
                this.ListArticulos.DataBind();

                this.ListArticulos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListArticulos.Items.Insert(1, new ListItem("Todos", "0"));
            }
            catch
            {

            }
        }
        public List<sucursale> obtenerSucursalesCargardas()
        {
            try
            {
                List<sucursale> list = new List<sucursale>();
                foreach (ListItem item in ListBoxSucursales.Items)
                {
                    sucursale s = this.contArtEnt.obtenerSucursalEntityByID(Convert.ToInt32(item.Value));
                    //sucursale s = new sucursale();
                    //s.id = Convert.ToInt32(item.Value);
                    list.Add(s);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<gruposArticulo> obtenerGruposCargados()
        {
            try
            {
                List<gruposArticulo> list = new List<gruposArticulo>();
                foreach (ListItem item in this.ListBoxGrupos.Items)
                {
                    gruposArticulo g = this.contArtEnt.obtenerGrupoArticuloEntByID(Convert.ToInt32(item.Value));
                    //gruposArticulo g = new gruposArticulo();
                    //g.id = Convert.ToInt32(item.Value);
                    list.Add(g);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<articulo> obtenerArticulosCargados()
        {
            try
            {
                List<articulo> list = new List<articulo>();
                foreach (ListItem item in this.ListBoxArticulos.Items)
                {
                    articulo a = this.contArtEnt.obtenerArticuloEntity(Convert.ToInt32(item.Value));
                    //articulo a = new articulo();
                    //a.id = Convert.ToInt32(item.Value);
                    list.Add(a);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<Gestion_Api.Entitys.Tarjeta> obtenerTarjetasCargadas()
        {
            try
            {
                List<Gestion_Api.Entitys.Tarjeta> list = new List<Gestion_Api.Entitys.Tarjeta>();
                foreach (ListItem item in this.ListBoxTarjetas.Items)
                {
                    Gestion_Api.Entitys.Tarjeta t = this.contArtEnt.obtenerTarjetaEntityById(Convert.ToInt32(item.Value));
                    list.Add(t);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        protected void ListTipoPromocion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.PanelDto.Visible = false;
                this.PanelPrecio.Visible = false;

                if (ListTipoPromocion.SelectedValue == "0")
                {
                    this.PanelDto.Visible = true;
                    this.PanelPrecio.Visible = false;
                }
                if (ListTipoPromocion.SelectedValue == "1")
                {
                    this.PanelDto.Visible = false;
                    this.PanelPrecio.Visible = true;
                }
            }
            catch
            {

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSucursalByEmpresa(Convert.ToInt32(ListEmpresa.SelectedValue));
            }
            catch
            {

            }
        }
        protected void ListGrupos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int grupo = Convert.ToInt32(this.ListGrupos.SelectedValue);
                int prov = Convert.ToInt32(this.ListProveedores.SelectedValue);

                if (grupo > 0)
                    this.cargarArticulosByGrupoProv(grupo, prov);
            }
            catch
            {

            }
        }
        protected void ListProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int grupo = Convert.ToInt32(this.ListGrupos.SelectedValue);                
                int prov = Convert.ToInt32(this.ListProveedores.SelectedValue);
                this.cargarArticulosByGrupoProv(grupo, prov);
                //if (grupo > 0 && prov > 0)
                //    this.cargarArticulosByGrupoProv(grupo, prov);
                //else
                //    this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedores.SelectedValue));
            }
            catch
            {

            }
        }
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBuscarArticulo.Text))
                    this.cargarProducto(this.txtBuscarArticulo.Text);
            }
            catch
            {

            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                    this.modificarPromo();
                else
                    this.agregarPromo();
            }
            catch
            {

            }
        }        
        protected void btnAgregarSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                if (suc >= 0)
                {
                    ListItem item = new ListItem();
                    item.Value = suc.ToString();
                    item.Text = this.ListSucursal.SelectedItem.Text;

                    //si no esta , lo agrego
                    if (this.ListBoxSucursales.Items.FindByValue(item.Value) == null)
                    {
                        this.ListBoxSucursales.Items.Add(item);
                    }

                }
            }
            catch
            {

            }
        }
        protected void btnEliminarSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxSucursales.Items.Remove(this.ListBoxSucursales.SelectedItem);
            }
            catch
            {

            }
        }
        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                int art = Convert.ToInt32(this.ListArticulos.SelectedValue);
                if (art >= 0)
                {
                    ListItem item = new ListItem();
                    item.Value = this.ListArticulos.SelectedValue;
                    item.Text = this.ListArticulos.SelectedItem.Text;
                    if (this.ListBoxArticulos.Items.FindByValue(item.Value) == null)
                    {
                        this.ListBoxArticulos.Items.Add(item);
                    }
                }
            }
            catch
            {

            }
        }
        protected void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxArticulos.Items.Remove(this.ListBoxArticulos.SelectedItem);
            }
            catch
            {

            }
        }
        protected void btnAgregarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                int gr = Convert.ToInt32(this.ListGrupos.SelectedValue);
                if (gr >= 0)
                {
                    ListItem item = new ListItem();
                    item.Value = this.ListGrupos.SelectedValue;
                    item.Text = this.ListGrupos.SelectedItem.Text;
                    if (this.ListBoxGrupos.Items.FindByValue(item.Value) == null)
                    {
                        this.ListBoxGrupos.Items.Add(item);
                    }
                }
            }
            catch
            {

            }
        }
        protected void btnEliminarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxGrupos.Items.Remove(this.ListBoxGrupos.SelectedItem);
            }
            catch
            {

            }
        }
        protected void btnAgregarTarjetas_Click(object sender, EventArgs e)
        {
            try
            {
                int t = Convert.ToInt32(this.ListTarjetas.SelectedValue);
                if (t >= 0)
                {
                    ListItem item = new ListItem();
                    item.Value = this.ListTarjetas.SelectedValue;
                    item.Text = this.ListTarjetas.SelectedItem.Text;
                    if (this.ListBoxTarjetas.Items.FindByValue(item.Value) == null)
                    {
                        this.ListBoxTarjetas.Items.Add(item);
                    }
                }
            }
            catch
            {

            }
        }
        protected void btnEliminarTarjetas_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxTarjetas.Items.Remove(this.ListBoxTarjetas.SelectedItem);
            }
            catch
            {

            }
        }

        protected void ListFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ListFormasPago.SelectedItem.Text == "Tarjeta"){
                    this.panelTarjetas.Visible = true;
                    this.panelArticulos.Visible = true;
                }
                else
                {
                    this.panelTarjetas.Visible = false;
                    this.panelArticulos.Visible = true;
                }
            }
            catch
            {

            }
        }        
    }
}