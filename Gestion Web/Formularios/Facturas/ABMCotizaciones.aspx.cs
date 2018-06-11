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
    public partial class ABMCotizaciones : System.Web.UI.Page
    {
        
        Mensajes m = new Mensajes();
        controladorCotizaciones controlador = new controladorCotizaciones();
        controladorUsuario contUser = new controladorUsuario();
        //
        
        controladorArticulo contArticulo = new controladorArticulo();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        public PlaceHolder phArticulos = new PlaceHolder();
        controladorSucursal cs = new controladorSucursal();
        //Cotizacion
        Cotizaciones Cotizacion = new Cotizaciones();

        Cliente cliente = new Cliente();
        TipoDocumento tp = new TipoDocumento();

        int accion;
        int idEmpresa;
        int idSucursal;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);

                //dibujo los items en la tabla
                if (Session["Cotizacion"] != null)
                {
                    this.cargarItems();
                }

                if (!IsPostBack)
                {

                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cotizacion agregada", null));
                    //genero la Cotizacion de la session

                    idEmpresa = (int)Session["Login_EmpUser"];
                    idSucursal = (int)Session["Login_SucUser"];

                    Cotizaciones cotizacion = new Cotizaciones();
                    Session.Add("Cotizacion", cotizacion);

                    //this.cargarIva();
                    this.cargarEmpleados();
                    this.cargarTipoCotizacion();
                    this.cargarVendedor();
                    this.cargarFormaPAgo();
                    this.cargarListaPrecio();
                    this.cargarClientes();
                    this.cargarEmpresas();
                    this.ListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
                    this.ListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
                    this.ListPuntoVenta.SelectedIndex = 1;
                    this.obtenerNroCotizacion();

                    //me pasaron el articulo
                    if (this.accion == 2)
                    {
                        //obtengo codigo
                        this.txtCodigo.Text = Request.QueryString["articulo"];
                        this.cargarProducto(this.txtCodigo.Text);

                    }
                    //me pasaron el cuit del cliente
                    if (this.accion == 3)
                    {
                        //obtengo codigo
                        //this.txtBusquedaCliente.Text = Request.QueryString["cliente"];
                        //this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));

                    }
                    //Me fijo si hay que cargar un cliente por defecto
                    this.verificarClienteDefecto();
                }
                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //vengo desde cliente modal
                if (Session["CotizacionesABM_ClienteModal"] != null)
                {
                    //obtengo codigo
                    this.cargarClienteDesdeModal();
                    //int idCliente = (int)Session["CotizacionesABM_ClienteModal"];
                    //DropListClientes.SelectedValue = idCliente.ToString();
                    //this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                }


                if (Session["CotizacionesABM_ArticuloModal"] != null)
                {
                    //obtengo codigo
                    string CodArt = Session["CotizacionesABM_ArticuloModal"] as string;
                    this.txtCodigo.Text = CodArt;
                    this.cargarProducto(this.txtCodigo.Text);

                }
                //cargo el numero de cotizacion
                //this.obtenerNroCotizacion();  
            }
            catch
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
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "36")
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

        #region
        //public void cargarIva()
        //{
        //    try
        //    {
        //        DataTable dt = controlador.obtenerIva();

        //        //agrego todos
        //        DataRow dr = dt.NewRow();
        //        dr["descripcion"] = "Seleccione...";
        //        dr["id"] = -1;
        //        dt.Rows.InsertAt(dr, 0);

        //        this.DropListIva.DataSource = dt;
        //        this.DropListIva.DataValueField = "id";
        //        this.DropListIva.DataTextField = "descripcion";

        //        this.DropListIva.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando iva a la lista. " + ex.Message));
        //    }
        //}

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
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        //private void cargarClienteDesdeModal()
        //{
        //    try
        //    {
        //        //obtengo codigo
        //        int idCliente = (int)Session["CotizacionesABM_ClienteModal"];
        //        try
        //        {
        //            this.DropListClientes.SelectedValue = idCliente.ToString();
        //        }
        //        catch
        //        {
        //            //el cliente no estaba en el drop list
        //            //lo agrego y selecciono
        //            //lo busco y agrego
        //            var c = contCliente.obtenerClienteID(idCliente);

        //            this.DropListClientes.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
        //            this.DropListClientes.SelectedValue = idCliente.ToString();
        //        }
        //        this.cargarCliente(idCliente);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cliente desde modal. " + ex.Message));
        //    }
        //}

        private void cargarClienteDesdeModal()
        {
            try
            {
                //obtengo codigo
                int idCliente = (int)Session["CotizacionesABM_ClienteModal"];
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



        public void cargarTipoCotizacion()
        {
            try
            {
                //DataTable dt = controlador.obtenerTipoCotizacion();

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos Cotizacion. " + ex.Message));
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
        
        #endregion
        /// <summary>
        /// Busca el cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //this.cargarCliente(this.txtBusquedaCliente.Text);
        }

        private void cargarCliente(int idCliente)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                //this.cliente = contCliente.obtenerClienteCuit(cuit);
                this.cliente = contCliente.obtenerClienteID(idCliente);

                if (this.cliente != null)
                {
                    this.labelCliente.Text = this.cliente.razonSocial + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                    this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();
                    this.DropListVendedor.SelectedValue = this.cliente.vendedor.id.ToString();
                    this.DropListFormaPago.SelectedValue = this.cliente.formaPago.id.ToString();
                    //this.obtenerCotizacion(this.cliente.iva);
                    //this.DropListIva.SelectedIndex = 1;

                    //pongo visible el panel para Cotizacionr
                    //this.UpdatePanel1.Visible = true;
                    this.txtCantidad.Text = "1";
                    this.txtCantidad.Text = "0";
                    
                    //cargo el cliente en la Cotizacion session
                    Cotizaciones c = Session["Cotizacion"] as Cotizaciones;
                    c.cliente.id = this.cliente.id;
                    Session.Add("Cotizacion", c);
                    Session["CotizacionesABM_ClienteModal"] = null;

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
                    this.obtenerNroCotizacion();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando cliente por defecto. " + ex.Message));                
            }
        }

        private void verificarAlerta()
        {
            try
            {
                Cliente c = contCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Alerta Cliente: " + c.alerta.descripcion + ". \");", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Alerta Cliente: " + c.alerta.descripcion + "."));
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Obtiene el ultimo numero de Cotizacion
        /// </summary>
        
        private void obtenerNroCotizacion()
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerCotizacionNumero(ptoVenta, "Cotizacion");
                this.labelNroCotizacion.Text = "Cotizacion N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                
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
                Articulo art = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));

                if (art != null)
                {
                    //agrego los txt
                    this.txtDescripcion.Text = art.descripcion;
                    this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                    //decimal PrecioSinIva = decimal.Round(art.precioVenta - (art.precioVenta * (art.porcentajeIva / 100)), 2);
                    this.txtPUnitario.Text = art.precioVenta.ToString();
                    Session["CotizacionesABM_ArticuloModal"] = null;
                    this.txtCantidad.Focus();

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
            this.cargarProductoACotizacion();
        }

        private void cargarProductoACotizacion()
        {
            try
            {
                //item
                ItemCotizacion item = new ItemCotizacion();
                //item.articulo = contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text.Replace(",", ".")); a chequear
                //item.cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", "."));
                //item.total = Convert.ToDecimal(this.txtTotalArri.Text.Replace(",", "."));
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text.Replace(",", "."));
                //item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                item.articulo = contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text);
                item.cantidad = Convert.ToDecimal(this.txtCantidad.Text); // armar funcion para actualizar cantidad
                item.total = Convert.ToDecimal(this.txtTotalArri.Text);
                decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text);
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
                this.Cotizacion.items.Add(item);
                //lo agrego al session
                if (Session["Cotizacion"] == null)
                {
                    Cotizaciones cot = new Cotizaciones();
                    Session.Add("Cotizacion", cot);

                }
                Cotizaciones c = Session["Cotizacion"] as Cotizaciones;
                c.items.Add(item);
                Session.Add("Cotizacion", c);

                //lo dibujo en pantalla
                this.cargarItems();

                //agrego abajo
                //this.Cotizacion.items.Add(item);
                //actualizo totales
                this.actualizarTotales();

                //borro los campos
                this.borrarCamposagregarItem();
                this.UpdatePanel1.Update();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }

        private void borrarCamposagregarItem()
        {
            try
            {
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "0";
                this.txtDescripcion.Text = "";
                this.txtIva.Text = "";
                this.TxtDescuentoArri.Text = "0";
                this.txtPUnitario.Text = "";
                this.txtTotalArri.Text = "0";
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
                Cotizaciones c = Session["Cotizacion"] as Cotizaciones;
                //limpio el place holder y lo vuelvo a cargar
                this.phArticulos.Controls.Clear();
                int pos = 0;
                foreach(ItemCotizacion item in c.items)
                {
                    pos = c.items.IndexOf(item);
                    this.agregarItemCotizacion(item,pos);
                   
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando items en cotizacion. " + ex.Message));
            }
        }
        
        /// <summary>
        /// Carga el item en la tabla items
        /// </summary>
        private void agregarItemCotizacion(ItemCotizacion item,int pos)
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
                celCantidad.Text = item.cantidad.ToString();
                celCantidad.Width = Unit.Percentage(10);
                celCantidad.HorizontalAlign = HorizontalAlign.Center;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.articulo.descripcion;
                celDescripcion.Width = Unit.Percentage(40);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + item.articulo.precioVenta.ToString();
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
                btnEliminar.Click += new EventHandler(this.QuitarItem);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phArticulos.Controls.Add(tr);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
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
                this.Cotizacion = Session["Cotizacion"] as Cotizaciones;

                //obtengo total de suma de item
                decimal totalC = this.Cotizacion.obtenerTotalNeto();
                decimal total = decimal.Round(totalC, 2);
                this.Cotizacion.neto = total;

                //Subtotal = neto menos el descuento
                this.Cotizacion.descuento = this.Cotizacion.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100);
                this.Cotizacion.subTotal = this.Cotizacion.neto - this.Cotizacion.descuento;

                //del subtotal obtengo iva
                //this.factura.neto21 = (this.factura.subTotal * Convert.ToDecimal(0.21));
                //decimal totalIva = this.factura.obtenerTotalIva();
                string[] lbl = this.labelNroCotizacion.Text.Split('°');
                if (lbl[0] == "Presupuesto N")
                {
                    Configuracion c = new Configuracion();
                    this.Cotizacion.neto21 = (this.Cotizacion.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100);
                }
                else
                {
                    decimal iva = decimal.Round(this.Cotizacion.obtenerTotalIva(), 2);
                    decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
                    this.Cotizacion.neto21 = iva - decimal.Round(descuento, 2);
                }

                this.Cotizacion.totalSinDescuento = this.Cotizacion.neto + this.Cotizacion.obtenerTotalIva();

                //retencion sobre el sub total
                this.Cotizacion.retencion = this.Cotizacion.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100);

                //total: subtotal + iva + retencion 
                this.Cotizacion.total = this.Cotizacion.subTotal + this.Cotizacion.neto21 + this.Cotizacion.retencion;


                //cargo en pantalla
                string neto = decimal.Round(this.Cotizacion.neto, 2).ToString();
                //this.txtNeto.Text = decimal.Round(this.factura.neto, 2).ToString();
                this.txtNeto.Text = neto;

                this.txtDescuento.Text = decimal.Round(this.Cotizacion.descuento, 2).ToString();

                this.txtsubTotal.Text = decimal.Round(this.Cotizacion.subTotal, 2).ToString();

                this.txtIvaTotal.Text = decimal.Round(this.Cotizacion.neto21, 2).ToString();

                this.txtRetencion.Text = decimal.Round(this.Cotizacion.retencion, 2).ToString();

                //string Stotal = .ToString();
                this.txtTotal.Text = decimal.Round(this.Cotizacion.total, 2).ToString();
                //this.txtImporteEfectivo.Text = decimal.Round(this.remito.total, 2).ToString();

                Cotizaciones cot = this.Cotizacion;

                Session.Add("Cotizacion", cot);
            }

            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));
            }
            //try
            //{
            //    this.Cotizacion = Session["Cotizacion"] as Cotizaciones;
                
            //    obtengo total de suma de item
            //    decimal total = this.Cotizacion.obtenerTotalNeto();
            //    this.Cotizacion.neto = total;

            //    Subtotal = neto menos el descuento
            //    this.Cotizacion.descuento = this.Cotizacion.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100);
            //    this.Cotizacion.subTotal = this.Cotizacion.neto - this.Cotizacion.descuento;

            //    del subtotal obtengo iva
            //    this.Cotizacion.neto21 = (this.Cotizacion.subTotal * Convert.ToDecimal(0.21));
            //    this.Cotizacion.neto21 = this.Cotizacion.obtenerTotalIva();
                
            //    retencion sobre el sub total
            //    this.Cotizacion.retencion = this.Cotizacion.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100);

            //    total: subtotal + iva + retencion 
            //    this.Cotizacion.total = this.Cotizacion.subTotal + this.Cotizacion.neto21 + this.Cotizacion.retencion;


            //    cargo en pantalla
            //    this.txtNeto.Text = decimal.Round(this.Cotizacion.neto, 2).ToString();
                
            //    this.txtDescuento.Text = decimal.Round(this.Cotizacion.descuento, 2).ToString();
               
            //    this.txtsubTotal.Text = decimal.Round(this.Cotizacion.subTotal, 2).ToString();
                
            //    this.txtIvaTotal.Text = decimal.Round(this.Cotizacion.neto21, 2).ToString();
                
            //    this.txtRetencion.Text = decimal.Round(this.Cotizacion.retencion, 2).ToString();
                
            //    this.txtTotal.Text = decimal.Round(this.Cotizacion.total,2).ToString();

            //    Cotizaciones f = this.Cotizacion;
                
            //    Session.Add("Cotizacion", f);
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));
            //}
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtengo items
                //List<ItemCotizacion> items = this.obtenerItems();
                Cotizaciones cotizacion = Session["Cotizacion"] as Cotizaciones;
                //List<ItemCotizacion> items = fact.items;

                if (cotizacion.items.Count > 0)
                {
                    
                    // cambiar 

                    cotizacion.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);

                    cotizacion.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);

                    PuntoVenta pv = new PuntoVenta();

                    cotizacion.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));


                    cotizacion.fecha = DateTime.Now;

                    cotizacion.vendedor.id = Convert.ToInt32(this.DropListVendedor.SelectedValue);

                    cotizacion.formaPAgo.id = Convert.ToInt32(this.DropListFormaPago.SelectedValue);

                    cotizacion.listaP.id = Convert.ToInt32(this.DropListLista.SelectedValue);

                    tp = controlador.obtenerTipoDoc("Cotizacion");
                    cotizacion.tipo.id = tp.id;
                   

                    int i = this.controlador.Cotizar(cotizacion);
                    if (i > 0)
                    {
                        Session.Remove("Cotizacion");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta " + labelNroCotizacion.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cotizacion agregada", "ABMCotizaciones.aspx"));
                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cotizacion "));
                    }


                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la Cotizacion " + this.txtCodigo.Text));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando Cotizaciones. " + ex.Message));
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
                Cotizaciones ct = Session["Cotizacion"] as Cotizaciones;
                foreach (ItemCotizacion item in ct.items)
                {
                    if ((item.articulo.codigo == idCodigo) && Convert.ToInt32(pos) == ct.items.IndexOf(item))
                    {
                        //lo quito
                        ct.items.Remove(item);
                        break;
                    }
                }

                //cargo el nuevo pedido a la sesion
                Session["Cotizacion"] = ct;

                //vuelvo a cargar los items
                this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item a la cotizacion. " + ex.Message));
            }
        }

        private List<ItemCotizacion> obtenerItems()
        {
            List<ItemCotizacion> items = new List<ItemCotizacion>();

            foreach(Control cr in this.phArticulos.Controls)
            {
                //item
                ItemCotizacion item = new ItemCotizacion();
                TableRow tr = cr as TableRow;
                item.articulo = this.contArticulo.obtenerArticuloCodigo(tr.Cells[0].ToString());
                item.cantidad = Convert.ToDecimal(tr.Cells[1]);
                item.descuento =0;
                item.precioUnitario = Convert.ToDecimal(tr.Cells[3]);
                item.total = Convert.ToDecimal(tr.Cells[4]);

                items.Add(item);
            }

            return items;


        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", ".")); a chequear
                //decimal precio = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                decimal precio = Convert.ToDecimal(this.txtPUnitario.Text);
                decimal total = cantidad * precio;
                this.txtTotalArri.Text = decimal.Round(total,2).ToString();
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
                decimal total = Convert.ToDecimal(this.txtPUnitario.Text);
                decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                decimal totalDesc = total * (1 - (desc/100));
                this.txtTotalArri.Text = decimal.Round(totalDesc,2).ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total con descuento. Verifique que ingreso numeros en Descuento" + ex.Message));
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
            //Me fijo si hay que cargar un cliente por defecto
            this.verificarClienteDefecto();
        }

        protected void ListPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.obtenerNroCotizacion();
        }

        protected void txtPorcRetencion_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
            
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

        protected void txtPorcRetencion_TextChanged1(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }

        //protected void txtPorcRetencion_TextChanged(object sender, EventArgs e)
        //{
        //    this.actualizarTotales();
        //}

   
    }
}

