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
    public partial class ABMRemitos : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorRemitos controlador = new controladorRemitos();
        controladorUsuario contUser = new controladorUsuario();
        //
        controladorArticulo contArticulo = new controladorArticulo();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        public PlaceHolder phArticulos = new PlaceHolder();
        controladorSucursal cs = new controladorSucursal();
        ControladorPedido cp = new ControladorPedido();
        Remito remito = new Remito();

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

                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

                //dibujo los items en la tabla
                if (Session["Remito"] != null)
                {
                    this.cargarItems();
                }
                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("factura agregada", null));
                    //genero la factura de la session
                    //Factura fac = new Factura();

                    idEmpresa = (int)Session["Login_EmpUser"];
                    idSucursal = (int)Session["Login_SucUser"];

                    Remito rem = new Remito();
                    Session.Add("Remito", rem);

                    //this.cargarIva();
                    this.cargarEmpleados();
                    this.cargarTipoRemito();
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
                    this.obtenerNroRemito();

                    //me pasaron el articulo
                    if (this.accion == 2)
                    {
                        //obtengo codigo
                        //this.txtCodigo.Text = Request.QueryString["articulo"];
                        //this.cargarProducto(this.txtCodigo.Text);

                    }
                    //me pasaron el cuit del cliente
                    if (this.accion == 3)
                    {
                        //obtengo codigo
                        //this.txtBusquedaCliente.Text = Request.QueryString["cliente"];
                        //this.cargarCliente(this.txtBusquedaCliente.Text);

                    }
                    if (this.accion == 4)
                    {
                        int idPedido = Convert.ToInt32(Request.QueryString["id_ped"]);
                        GenerarRemitoPedido(idPedido);
                    }
                    //Me fijo si hay que cargar un cliente por defecto
                    this.verificarClienteDefecto();
                }

                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                
                if (Session["RemitosABM_ClienteModal"] != null)
                {
                    //obtengo codigo
                    this.cargarClienteDesdeModal();

                    //int idCliente = (int)Session["RemitosABM_ClienteModal"];
                    //DropListClientes.SelectedValue = idCliente.ToString();
                    //this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                }

                if (Session["RemitosABM_ArticuloModal"] != null)
                {
                    //obtengo codigo
                    string CodArt = Session["RemitosABM_ArticuloModal"] as string;
                    this.txtCodigo.Text = CodArt;
                    this.cargarProducto(this.txtCodigo.Text);
                }
                //cargo el numero de remito
                //this.obtenerNroRemito();
                //Dejo editable el campo de descripcion del articulo o no
                this.verficarConfiguracionEditar();
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
                        if (s == "38")
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
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error verificando configuracion editar descripcion.  " + ex.Message + "\", {type: \"error\"});", true);
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
        public void cargarTipoRemito()
        {
            try
            {
                //DataTable dt = controlador.obtenerTipoFactura();

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos Remito. " + ex.Message));
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


        /// <summary>
        /// Genera el remito con los datos recibidos del pedido
        /// </summary>
        public void GenerarRemitoPedido(int id_ped)
        {
            try
            {
                this.remito = Session["Remito"] as Remito;
                Pedido p = new Pedido();
                p = cp.obtenerPedidoId(id_ped);
                Remito r = controlador.AsignarPedido(p);
                if (id_ped > 0)
                {
                    r.pedido.id = id_ped;
                }
                else
                {
                    r.pedido.id = 0;
                }
                Session.Add("Remito", r);
                this.ListEmpresa.SelectedValue = r.empresa.id.ToString();
                this.cargarSucursal(r.empresa.id);
                this.cargarPuntoVta(r.sucursal.id);
                this.cargarCliente(r.cliente.id);
                this.DropListClientes.SelectedValue = r.cliente.id.ToString();
                this.DropListVendedor.SelectedValue = r.vendedor.id.ToString();
                this.DropListFormaPago.SelectedValue = r.formaPAgo.id.ToString();
                this.DropListLista.SelectedValue = r.listaP.id.ToString();
                this.ListSucursal.SelectedValue = r.sucursal.id.ToString();
                this.ListPuntoVenta.SelectedValue = r.ptoV.id.ToString();
                this.txtComentarios.Text = r.comentario;
                if (r.comentario.Length > 0)
                {
                    this.checkDatos.Checked = true;
                    this.phDatosEntrega.Visible = true;
                }
                this.cargarItems();
                this.actualizarTotales();
                this.obtenerNroRemito();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error asignando datos pedido a remito " + ex.Message));

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

        private void cargarCliente(int idCliente)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                this.cliente = contCliente.obtenerClienteID(idCliente);

                if (this.cliente != null)
                {
                    this.labelCliente.Text = this.cliente.razonSocial + " - " + this.cliente.iva + " - " + this.cliente.cuit;
                    this.obtenerRemito(this.cliente.iva);
                    this.DropListVendedor.SelectedValue = this.cliente.vendedor.id.ToString();
                    this.DropListLista.SelectedValue = this.cliente.lisPrecio.id.ToString();
                    this.DropListFormaPago.SelectedValue = this.cliente.formaPago.id.ToString();
                    //this.DropListIva.SelectedIndex = 1;

                    //pongo visible el panel para facturar
                    //this.UpdatePanel1.Visible = true;
                    this.txtCantidad.Text = "1";
                    this.txtCantidad.Text = "0";

                    //cargo el cliente en la factura session
                    Remito r = Session["Remito"] as Remito;
                    r.cliente.id = this.cliente.id;
                    Session.Add("Remito", r);
                    Session["RemitosABM_ClienteModal"] = null;


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

                if (idCliente != "-1" && idCliente != "-2" && idCliente != null)
                {
                    if (this.DropListClientes.Items.FindByValue(idCliente) == null)
                    {
                        this.cargarClienteEnLista(Convert.ToInt32(idCliente));
                    }
                    this.DropListClientes.SelectedValue = idCliente;
                    this.cargarCliente(Convert.ToInt32(this.DropListClientes.SelectedValue));
                    this.obtenerNroRemito();
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

        //private void cargarClienteDesdeModal()
        //{
        //    try
        //    {
        //        //obtengo codigo
        //        int idCliente = (int)Session["RemitosABM_ClienteModal"];
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
                int idCliente = (int)Session["RemitosABM_ClienteModal"];
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
        /// Obtiene el ultimo numero de factura
        /// </summary>
        /// <param name="iva"></param>
        private void obtenerRemito(string iva)
        {
            try
            {
                //cargo el tipo en la factura session
                Remito r = Session["Remito"] as Remito;

                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                TipoDocumento ti = this.controlador.obtenerFacturaNumero(ptoVenta, 14);

                r.tipo.id = ti.id;
                Session.Add("Remito", r);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de remito. " + ex.Message));
            }
        }

        private void obtenerNroRemito()
        {
            try
            {
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                PuntoVenta pv = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));
                //como estoy en cotizacion pido el ultimo numero de este documento
                int nro = this.controlador.obtenerRemitoNumero(ptoVenta, "Remito");
                this.labelNroRemito.Text = "Remito N° " + pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
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
                //Articulo art = contArticulo.obtenerArticuloCodigo(codigo);
                Articulo art = contArticulo.obtenerArticuloFacturar(codigo, Convert.ToInt32(this.DropListLista.SelectedValue));
                if (art != null)
                {
                    //agrego los txt
                    this.txtDescripcion.Text = art.descripcion;
                    this.txtIva.Text = art.porcentajeIva.ToString() + "%";
                    //decimal PrecioSinIva = decimal.Round(art.precioVenta - (art.precioVenta * (art.porcentajeIva / 100)), 2);
                    this.txtPUnitario.Text = decimal.Round(art.precioVenta, 2).ToString();
                    Session["RemitosABM_ArticuloModal"] = null;
                    this.txtCantidad.Focus();
                    this.totalItem();

                    Remito r = Session["Remito"] as Remito;
                    this.txtRenglon.Text = (r.items.Count + 1).ToString();
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
                    //total = total * (desc / 100);
                    total = total - (total * (desc / 100));

                    this.txtTotalArri.Text = decimal.Round(total, 2).ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total " + ex.Message));
            }
        }

        protected void btnAgregarArt_Click(object sender, EventArgs e)
        {
            this.cargarProductoARemito();
        }

        private void cargarProductoARemito()
        {
            try
            {
                //recalculo total
                this.totalItem();

                //item
                ItemRemito item = new ItemRemito();
                //item.articulo = contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text.Replace(",", "."));
                //item.cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", "."));
                //item.total = Convert.ToDecimal(this.txtTotalArri.Text.Replace(",", "."));
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text.Replace(",", "."));
                //item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                item.articulo = contArticulo.obtenerArticuloCodigo(this.txtCodigo.Text);
                item.cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                item.total = Convert.ToDecimal(this.txtTotalArri.Text);
                decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text);
                item.precioUnitario = Convert.ToDecimal(this.txtPUnitario.Text);
                item.descripcion = this.txtDescripcion.Text;
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
                this.remito.items.Add(item);
                //lo agrego al session
                if (Session["Remito"] == null)
                {
                    //Factura fac = new Factura();
                    Remito rem = new Remito();
                    Session.Add("Remito", rem);

                }
                Remito r = Session["Remito"] as Remito;

                if (!String.IsNullOrEmpty(this.txtRenglon.Text))
                    item.nroRenglon = Convert.ToInt32(this.txtRenglon.Text);
                else
                    item.nroRenglon = r.items.Count() + 1;

                r.items.Add(item);
                Session.Add("Remito", r);

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
                Remito r = Session["Remito"] as Remito;
                //limpio el place holder y lo vuelvo a cargar
                this.phArticulos.Controls.Clear();
                int pos = 0;
                foreach (ItemRemito item in r.items)
                {
                    pos = r.items.IndexOf(item);
                    this.agregarItemRemito(item,pos);
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
        private void agregarItemRemito(ItemRemito item, int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = item.articulo.codigo.ToString() + pos;

                //Celdas

                TableCell celCodigo = new TableCell();
                //celCodigo.Text = item.articulo.codigo;
                if (item.nroRenglon > 0)
                    celCodigo.Text = item.nroRenglon + " - " + item.articulo.codigo;
                else
                    celCodigo.Text = (pos + 1) + " - " + item.articulo.codigo;
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
                tr.Cells.Add(celCantidad);
                //celCantidad.Text = item.cantidad.ToString();
                //celCantidad.Width = Unit.Percentage(10);
                //celCantidad.HorizontalAlign = HorizontalAlign.Center;
                //celCantidad.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celCantidad);


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.descripcion;
                //celDescripcion.Text = item.articulo.descripcion;
                celDescripcion.Width = Unit.Percentage(40);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                //celPrecio.Text = "$" + item.articulo.precioVenta.ToString();
                celPrecio.Text = "$" + item.precioUnitario.ToString();
                celPrecio.Width = Unit.Percentage(10);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = "$" + item.descuento.ToString();
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
                this.remito = Session["Remito"] as Remito;

                //obtengo total de suma de item
                decimal totalC = this.remito.obtenerTotalNeto();
                //decimal total = decimal.Round(totalC, 2);
                decimal total = decimal.Round(totalC, 2, MidpointRounding.AwayFromZero);
                this.remito.neto = total;

                //Subtotal = neto menos el descuento
                this.remito.descuento = this.remito.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100);
                this.remito.subTotal = this.remito.neto - this.remito.descuento;

                //del subtotal obtengo iva
                //this.factura.neto21 = (this.factura.subTotal * Convert.ToDecimal(0.21));
                //decimal totalIva = this.factura.obtenerTotalIva();
                string[] lbl = this.labelNroRemito.Text.Split('°');
                if (lbl[0] == "Presupuesto N")
                {
                    Configuracion c = new Configuracion();
                    this.remito.neto21 = (this.remito.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100);
                }
                else
                {
                    decimal iva = decimal.Round(this.remito.obtenerTotalIva(), 2);
                    decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
                    this.remito.neto21 = iva - decimal.Round(descuento, 2);
                }

                this.remito.totalSinDescuento = this.remito.neto + this.remito.obtenerTotalIva();

                //retencion sobre el sub total
                this.remito.retencion = this.remito.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100);

                //total: subtotal + iva + retencion 
                this.remito.total = this.remito.subTotal + this.remito.neto21 + this.remito.retencion;


                //cargo en pantalla
                string neto = decimal.Round(this.remito.neto, 2).ToString();
                //this.txtNeto.Text = decimal.Round(this.factura.neto, 2).ToString();
                this.txtNeto.Text = neto;

                this.txtDescuento.Text = decimal.Round(this.remito.descuento, 2).ToString();

                this.txtsubTotal.Text = decimal.Round(this.remito.subTotal, 2).ToString();

                this.txtIvaTotal.Text = decimal.Round(this.remito.neto21, 2).ToString();

                this.txtRetencion.Text = decimal.Round(this.remito.retencion, 2).ToString();

                //string Stotal = .ToString();
                this.txtTotal.Text = decimal.Round(this.remito.total, 2).ToString();
                //this.txtImporteEfectivo.Text = decimal.Round(this.remito.total, 2).ToString();

                Remito f = this.remito;

                Session.Add("Remito", f);
            }

            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));
            }
            //try
            //{
            //    this.remito = Session["Remito"] as Remito;

            //    //obtengo total de suma de item
            //    decimal totalC = this.remito.obtenerTotalNeto(); 
            //    decimal total = decimal.Round(totalC, 2);
            //    this.remito.neto = total;


            //    //Subtotal = neto menos el descuento
            //    this.remito.descuento = this.remito.neto * (Convert.ToDecimal(this.txtPorcDescuento.Text) / 100);
            //    this.remito.subTotal = this.remito.neto - this.remito.descuento;

            //    ///
            //    string[] lbl = this.labelNroRemito.Text.Split('°');
            //    if (lbl[0] == "Presupuesto N")
            //    {
            //        Configuracion c = new Configuracion();
            //        this.remito.neto21 = (this.remito.subTotal * Convert.ToDecimal(c.porcentajeIva, CultureInfo.InvariantCulture) / 100);
            //    }
            //    else
            //    {
            //        decimal iva = decimal.Round(this.remito.obtenerTotalIva(), 2);//cambiar
            //        decimal descuento = iva * (Convert.ToDecimal(this.txtPorcDescuento.Text.Replace(',', '.'), CultureInfo.InvariantCulture) / 100);
            //        this.remito.neto21 = iva - decimal.Round(descuento, 2);
            //    }
            //    //agregar
            //    this.remito.totalSinDescuento = this.remito.neto + this.remito.obtenerTotalIva();

            //    //retencion sobre el sub total
            //    this.remito.retencion = this.remito.subTotal * (Convert.ToDecimal(this.txtPorcRetencion.Text) / 100);

            //    //total: subtotal + iva + retencion 
            //    this.remito.total = this.remito.subTotal + this.remito.neto21 + this.remito.retencion;


            //    //cargo en pantalla
            //    string neto = decimal.Round(this.remito.neto, 2).ToString();
            //    //this.txtNeto.Text = decimal.Round(this.factura.neto, 2).ToString();
            //    this.txtNeto.Text = neto;

            //    this.txtDescuento.Text = decimal.Round(this.remito.descuento, 2).ToString();

            //    this.txtsubTotal.Text = decimal.Round(this.remito.subTotal, 2).ToString();

            //    this.txtIvaTotal.Text = decimal.Round(this.remito.neto21, 2).ToString();

            //    this.txtRetencion.Text = decimal.Round(this.remito.retencion, 2).ToString();

            //    //string Stotal = .ToString();
            //    //this.txtTotal.Text = decimal.Round(this.factura.total,2).ToString();
            //    this.txtTotal.Text = decimal.Round(this.remito.total, 2).ToString();
            //    //this.txtImporteEfectivo.Text = decimal.Round(this.factura.total, 2).ToString();

            //    Remito r = this.remito;

            //    Session.Add("Remito", r);
            //    //----------------------------------
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando totales. " + ex.Message));
            //}
        }

        protected void ActualizarTotalPH(object sender, EventArgs e)
        {
            try
            {
                string posicion = (sender as TextBox).ID.ToString().Substring(5, (sender as TextBox).ID.Length - 5);
                posicion = posicion.Split('_')[0];

                //Pedido ct = Session["Pedido"] as Pedido;
                Remito ct = Session["Remito"] as Remito;
                ItemRemito item = ct.items[Convert.ToInt32(posicion)];
                item.cantidad = Convert.ToDecimal((sender as TextBox).Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                item.total = item.precioUnitario * item.cantidad;
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
                Session["Remito"] = ct;

                //vuelvo a cargar los items
                //this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtengo items
                //List<ItemFactura> items = this.obtenerItems();
                Remito rem = Session["Remito"] as Remito;
                //List<ItemFactura> items = fact.items;

                if (rem.items.Count > 0)
                {
                    //this.factura.items = items;
                    ////obtengo datos
                    // cambiar 

                    rem.empresa.id = Convert.ToInt32(this.ListEmpresa.SelectedValue);

                    rem.sucursal.id = Convert.ToInt32(this.ListSucursal.SelectedValue);

                    rem.ptoV = cs.obtenerPtoVentaId(Convert.ToInt32(ListPuntoVenta.SelectedValue));

                    rem.fecha = DateTime.Now;

                    rem.vendedor.id = Convert.ToInt32(this.DropListVendedor.SelectedValue);

                    rem.formaPAgo.id = Convert.ToInt32(this.DropListFormaPago.SelectedValue);

                    rem.listaP.id = Convert.ToInt32(this.DropListLista.SelectedValue);

                    tp = controlador.obtenerTipoDoc("Remito");
                    rem.tipo.id = tp.id;

                    rem.comentario = this.txtComentarios.Text;

                    //datos para cot
                    string cuitTransportista = this.txtCuitTransportista.Text;
                    string patente = this.txtPatenteTransportista.Text;

                    int i = this.controlador.GenerarRemito(rem, cuitTransportista, patente);
                    if (i > 0)
                    {
                        Session.Remove("Remito");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta " + labelNroRemito.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Remito agregado", "ABMRemitos.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar remito "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos al remito " + this.txtCodigo.Text));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remitos. " + ex.Message));
            }
        }

        private void QuitarItem(object sender, EventArgs e)
        {
            try
            {
                string idCodigo = (sender as LinkButton).ID.ToString().Split('_')[1];
                string pos = (sender as LinkButton).ID.ToString().Split('_')[2];
                //string idCodigo = (sender as LinkButton).ID.ToString().Substring(11, (sender as LinkButton).ID.Length -11);

                //obtengo el pedido del session
                Remito ct = Session["Remito"] as Remito;
                foreach (ItemRemito item in ct.items)
                {
                    if ((item.articulo.codigo == idCodigo) && Convert.ToInt32(pos) == ct.items.IndexOf(item))
                    {
                        //lo quito
                        ct.items.Remove(item);
                        break;
                    }
                }

                //cargo el nuevo pedido a la sesion
                Session["Remito"] = ct;

                //vuelvo a cargar los items
                this.cargarItems();
                this.actualizarTotales();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item al Remito. " + ex.Message));
            }
        }

        private List<ItemRemito> obtenerItems()
        {
            List<ItemRemito> items = new List<ItemRemito>();

            foreach (Control cr in this.phArticulos.Controls)
            {
                //item
                ItemRemito item = new ItemRemito();
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

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ////decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text.Replace(",", "."));
                ////decimal precio = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                //decimal cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                //decimal precio = Convert.ToDecimal(this.txtPUnitario.Text);
                //decimal total = cantidad * precio;
                //this.txtTotalArri.Text = decimal.Round(total, 2).ToString();
                this.totalItem();

                this.txtDescuento.Focus();

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
                //decimal total = Convert.ToDecimal(this.txtPUnitario.Text.Replace(",", "."));
                //decimal desc = Convert.ToDecimal(this.TxtDescuentoArri.Text.Replace(",", "."));
                this.totalItem();
                this.lbtnAgregarArticuloASP.Focus();
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

        protected void txtRetencion_TextChanged(object sender, EventArgs e)
        {
            this.actualizarTotales();
        }

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtPtoVenta.Text = this.ListSucursal.SelectedValue;
            cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
            //Me fijo si hay que cargar un cliente por defecto
            this.verificarClienteDefecto();
        }

        protected void ListPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.obtenerNroRemito();
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

        protected void checkDatos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkDatos.Checked)
                {
                    this.phDatosEntrega.Visible = true;
                }
                else
                {
                    this.phDatosEntrega.Visible = false;
                }
            }
            catch(Exception ex)
            {
 
            }
        }
    }
}