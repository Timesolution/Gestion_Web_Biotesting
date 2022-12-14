using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class ArticulosC : System.Web.UI.Page
    {
        private controladorListaPrecio controladorPrecio = new controladorListaPrecio();
        private controladorArticulo controlador = new controladorArticulo();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
        private controladorSucursal contSucursal = new controladorSucursal();
        private controladorCliente contCliente = new controladorCliente();
        private ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        private ControladorPedidoEntity contPedidoEntity = new ControladorPedidoEntity();
        private ControladorPedido controladorPedido = new ControladorPedido();

        Mensajes m = new Mensajes();

        int accion;
        int grupo;
        int subgrupo;
        int proveedor;
        string descSubGrupo;
        string textoBuscar;
        public Dictionary<string, string> camposArticulos = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.grupo = Convert.ToInt32(Request.QueryString["g"]);
                this.subgrupo = Convert.ToInt32(Request.QueryString["sg"]);
                this.textoBuscar = Request.QueryString["t"];
                this.proveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.descSubGrupo = Request.QueryString["dsg"];

                this.cargarComentarioDesdeLaSession();
                //if (ConfigurationManager.AppSettings["ArticulosCV2"] == "1")
                //{
                //    divBusqueda.Style.Remove("display");
                //    divBusqueda.Style.Add("display", "none");
                //}
                if (!IsPostBack)
                {
                    if (Session["PedidoCliente"] == null)
                    {
                        Session.Add("PedidoCliente", new Pedido());
                    }
                    //cargo combos
                    this.cargarGruposArticulos();
                    string tipoUsuario = Session["Login_NombrePerfil"].ToString();
                  
                        this.cargarClientes();
                    
                    this.cargarSubGruposArticulos(Convert.ToInt32(ListGrupo.SelectedValue));
                }
                //this.lbtnVerPedido.Visible = true;
                //ver carro
                //if (this.accion == 3)
                //{
                //    this.lbtnVerPedido.Visible = false;
                    this.verCarroPedido();
                //}
                //filtro
                if (this.accion == 2)
                {
                    this.filtrar(grupo, subgrupo, proveedor, 0, 0);
                }
                //busco
                if (this.accion == 1)
                {
                    this.buscar(this.textoBuscar);
                }
                //cargo defecto
                if (this.accion == 0)
                {
                    List<Articulo> articulos = new List<Articulo>();
                    if(ConfigurationManager.AppSettings["ArticulosCLimitado"] == "1")
                    {
                        articulos = this.controlador.obtenerArticulosReducStore();
                    }
                    else
                    {
                        articulos = this.controlador.obtenerArticulosStore();
                    }
                        this.cargarArticulosTabla(articulos);

                }
                // editar
                if (accion == 5)
                {

                    this.verCarroPedidoEditar();

                }

                this.cargarBotonesDeLosGrupos();
                cargarBotonesDeLosSubGrupos(grupo);

                this.txtBusqueda.Focus();
                //Page.Form.DefaultButton = this.lbBuscar.UniqueID;

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
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

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



        private void cargarClientes()
        {
            try
            {

                DataTable dt = contClienteEntity.ObtenerFamiliaDelCliente(Convert.ToInt32((int)Session["Login_Vendedor"]));
                
                Gestion_Api.Entitys.cliente clienteUsuario = contClienteEntity.ObtenerClienteId(Convert.ToInt32((int)Session["Login_Vendedor"]));

                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                if( clienteUsuario != null)
                {
                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = clienteUsuario.alias;
                    dr2["id"] = clienteUsuario.id;
                    dt.Rows.InsertAt(dr2, 1);
                }
                //agrego todos


                this.ListClientes.DataSource = dt;
                this.ListClientes.DataValueField = "id";
                this.ListClientes.DataTextField = "alias";

                this.ListClientes.DataBind();
                Pedido p = Session["PedidoCliente"] as Pedido;
                if (p.cliente.id > 0)
                {
                    ListClientes.SelectedValue = p.cliente.id.ToString();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes. " + ex.Message));
            }

        }

        private void cargarSubGruposArticulos(int sGrupo)
        {
            try
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
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarArticulosTabla(List<Articulo> articulos)
        {
            try
            {
                int idCliente = (int)Session["Login_Vendedor"];
                Cliente cl = this.contCliente.obtenerClienteID(idCliente);
                //vacio place holder
                this.phArticulos.Controls.Clear();
                //List<Articulo> articulos2 = articulos.Take(5).ToList();
                foreach (Articulo art in articulos)
                {
                    if (art.apareceLista == 1)
                    {
                        this.cargarArticuloPH(art, cl);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a tabla. " + ex.Message));
            }
        }

        private void cargarArticuloPH(Articulo art, Cliente cl)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = "tr_" + art.id.ToString();

                //Celdas
                TableCell celImagen = new TableCell();
                try
                {
                    string[] imagenes = Directory.GetFiles(Server.MapPath("../../images/Productos/" + art.id + "/"));
                    if (imagenes.Length > 0)
                    {
                        FileInfo fi = new FileInfo(imagenes[0]);
                        Label gallery = new Label();
                        gallery.Text += @"<li>";
                        gallery.Text += @"<a href=../../images/Productos/" + art.id + "/" + fi.Name + " class=\"ui-lightbox\" >";
                        gallery.Text += "<img height=\"100\" width = \"100\" src=\"/images/Productos/" + art.id + "/" + fi.Name + "\" alt=\"\" />";
                        gallery.Text += @"</a>";
                        gallery.Text += @"<a href=../../images/Productos/" + art.id + "/" + fi.Name + " class=\"preview\"></a>";
                        gallery.Text += @" </li>";
                        gallery.Text += "<br/>";

                        celImagen.Controls.Add(gallery);
                    }
                }
                catch (Exception ex) { }

                celImagen.Width = Unit.Percentage(10);
                tr.Cells.Add(celImagen);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo;
                celCodigo.Width = Unit.Percentage(10);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = art.descripcion;
                celDescripcion.Width = Unit.Percentage(25);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                //TableCell celMarca = new TableCell();
                //Gestion_Api.Entitys.articulo a = this.contArtEntity.obtenerArticuloEntity(art.id);
                //if (a != null)
                //{
                //    if (a.Articulos_Marca.Count > 0)
                //    {
                //        celMarca.Text = a.Articulos_Marca.FirstOrDefault().Marca.marca1;
                //    }
                //    else
                //    {
                //        celMarca.Text = "SIN MARCA";
                //    }
                //}
                //else
                //{
                //    celMarca.Text = "SIN MARCA";
                //}
                ////celMarca.Text = art.grupo.descripcion;
                //celMarca.Width = Unit.Percentage(10);
                //celMarca.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celMarca);

                //TableCell celGrupo = new TableCell();
                //celGrupo.Text = art.grupo.descripcion;
                //celGrupo.Width = Unit.Percentage(10);
                //celGrupo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celGrupo);

                //TableCell celSubGrupo = new TableCell();
                //celSubGrupo.Text = art.subGrupo.descripcion;
                //celSubGrupo.Width = Unit.Percentage(10);
                //celSubGrupo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celSubGrupo);

                TableCell celPrecio = new TableCell();
                //celPrecio.Text = art.precioVenta.ToString("C");
                //celPrecio.Text = this.controlador.obtenerArticuloFacturar(art.codigo, cl.lisPrecio.id).precioSinIva.ToString("C");
                Articulo articulo = this.controlador.obtenerArticuloFacturar(art.codigo, cl.lisPrecio.id);
                //decimal totalSIva = (articulo.precioVenta / (1 + (articulo.porcentajeIva / 100)));
                decimal totalSIva = articulo.precioVenta;

                celPrecio.Text = totalSIva.ToString("C");
                celPrecio.Width = Unit.Percentage(5);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                //TableCell celIva = new TableCell();
                //celIva.Text = (articulo.precioVenta- totalSIva).ToString("C");
                //celPrecio.Width = Unit.Percentage(5);
                //celPrecio.VerticalAlign = VerticalAlign.Middle;
                //celPrecio.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celIva);

                TableCell celAction = new TableCell();
                TextBox txtCantidad = new TextBox();
                txtCantidad.TextMode = TextBoxMode.Number;
                txtCantidad.Text = "";
                txtCantidad.Width = Unit.Percentage(50);
                txtCantidad.ID = "txtCantidad_" + art.id;
                txtCantidad.Attributes.Add("Style", "text-align: right;");
                txtCantidad.Attributes.Add("MinValue", "0");
                txtCantidad.CssClass = "form-control col-xs-1";
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtCantidad.Attributes.Add("min", "0");
                celAction.Controls.Add(txtCantidad);

                Literal lit = new Literal();
                lit.Text = "&nbsp";
                celAction.Controls.Add(lit);

                LinkButton btnAddCarro = new LinkButton();
                btnAddCarro.ID = "btnAddCarro_" + art.id;
                btnAddCarro.CssClass = "btn btn-info";
                btnAddCarro.Text = "<span class='shortcut-icon icon-shopping-cart'></span>";
                btnAddCarro.Click += new EventHandler(this.AgregarItem);
                celAction.Controls.Add(btnAddCarro);
                celAction.Width = Unit.Percentage(15);

                tr.Cells.Add(celAction);

                this.phArticulos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articuloa a tabla. " + ex.Message));
            }
        }

        private void cargarCarroPh(Articulo art, int pos, decimal cantidad, Cliente cliente)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = "tr_" + art.id.ToString() + "_" + pos;

                art.grupo = this.controlador.obtenerGrupoID(art.grupo.id);
                art.subGrupo = this.controlador.obtenerSubGrupoID(art.subGrupo.id);

                //Celdas
                TableCell celImagen = new TableCell();
                try
                {
                    string[] imagenes = Directory.GetFiles(Server.MapPath("../../images/Productos/" + art.id + "/"));
                    if (imagenes.Length > 0)
                    {
                        FileInfo fi = new FileInfo(imagenes[0]);
                        Label gallery = new Label();
                        gallery.Text += @"<li>";
                        gallery.Text += @"<a href=../../images/Productos/" + art.id + "/" + fi.Name + " class=\"ui-lightbox\" >";
                        gallery.Text += "<img height=\"50\" width = \"50\" src=\"/images/Productos/" + art.id + "/" + fi.Name + "\" alt=\"\" />";
                        gallery.Text += @"</a>";
                        gallery.Text += @"<a href=../../images/Productos/" + art.id + "/" + fi.Name + " class=\"preview\"></a>";
                        gallery.Text += @" </li>";
                        gallery.Text += "<br/>";

                        celImagen.Controls.Add(gallery);
                    }
                }
                catch { }

                celImagen.Width = Unit.Percentage(3);
                tr.Cells.Add(celImagen);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo;
                celCodigo.Width = Unit.Percentage(10);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                //TableCell celDescripcion = new TableCell();
                //celDescripcion.Text = art.descripcion;
                //celDescripcion.Width = Unit.Percentage(30);
                //celDescripcion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celDescripcion);

                //TableCell celMarca = new TableCell();
                //Gestion_Api.Entitys.articulo a = this.contArtEntity.obtenerArticuloEntity(art.id);
                //if (a != null)
                //{
                //    if (a.Articulos_Marca.Count > 0)
                //    {
                //        celMarca.Text = a.Articulos_Marca.FirstOrDefault().Marca.marca1;
                //    }
                //    else
                //    {
                //        celMarca.Text = "SIN MARCA";
                //    }
                //}
                //else
                //{
                //    celMarca.Text = "SIN MARCA";
                //}
                //celMarca.Width = Unit.Percentage(10);
                //celMarca.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celMarca);

                //TableCell celGrupo = new TableCell();
                //celGrupo.Text = art.grupo.descripcion;
                //celGrupo.Width = Unit.Percentage(10);
                //celGrupo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celGrupo);

                //TableCell celSubGrupo = new TableCell();
                //celSubGrupo.Text = art.subGrupo.descripcion;
                //celSubGrupo.Width = Unit.Percentage(10);
                //celSubGrupo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celSubGrupo);

                Articulo articulo = this.controlador.obtenerArticuloFacturar(art.codigo, cliente.lisPrecio.id);
                decimal totalSIva = (articulo.precioVenta / (1 + (articulo.porcentajeIva / 100)));

                TableCell celPrecio = new TableCell();
                //celPrecio.Text = art.precioVenta.ToString("C");
                celPrecio.Text = articulo.precioVenta.ToString("C");
                celPrecio.Width = Unit.Percentage(5);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

                TableCell celAction = new TableCell();
                TextBox txtCantidad = new TextBox();
                txtCantidad.TextMode = TextBoxMode.Number;
                txtCantidad.Text = cantidad.ToString();
                txtCantidad.Width = Unit.Percentage(50);
                txtCantidad.ID = "txtCantidad_" + art.id + "_" + pos;
                txtCantidad.Attributes.Add("Style", "text-align: right;");
                if (accion != 3)
                    txtCantidad.Attributes.Add("disabled", "disabled");
                //txtCantidad.Attributes.Add("disabled", "disabled");
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtCantidad.Attributes.Add("min", "1");
                txtCantidad.CssClass = "form-control col-xs-1";
                txtCantidad.TextChanged += new EventHandler(this.CambiarCantidadItem);
                txtCantidad.AutoPostBack = true;

                celAction.Controls.Add(txtCantidad);

                Literal lit = new Literal();
                lit.Text = "&nbsp";
                celAction.Controls.Add(lit);

                LinkButton btnQuitarCarro = new LinkButton();
                btnQuitarCarro.ID = "btnQuitarCarro_" + art.id + "_" + pos;
                btnQuitarCarro.CssClass = "btn btn-info";
                btnQuitarCarro.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnQuitarCarro.Click += new EventHandler(this.QuitarItem);
                celAction.Controls.Add(btnQuitarCarro);

                celAction.Width = Unit.Percentage(30);
                tr.Cells.Add(celAction);

                this.phCarroCargado.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a tabla. " + ex.Message));
            }
        }

        private void verCarroPedido()
        {
            try
            {
                //vacio place holder
                this.phCarroCargado.Controls.Clear();
                this.phArticulosCarrito.Controls.Clear();
                Pedido p = Session["PedidoCliente"] as Pedido;
                int idCliente = (int)Session["Login_Vendedor"];
                Cliente cliente = contCliente.obtenerClienteID(idCliente);
                decimal total = 0;
                decimal cantidadTotal = 0;

                foreach (ItemPedido item in p.items)
                {
                    this.cargarCarroPh(item.articulo, p.items.IndexOf(item), item.cantidad, cliente);
                    total += item.precioUnitario * item.cantidad;
                    cantidadTotal += item.cantidad;
                }
                txtTotalPedido.Text = total.ToString("C");
                txtArtTotalPedido.Text = cantidadTotal.ToString();

                //this.lbtnVerPedido.Visible = false;
                this.lbtnGenerarPedido.Visible = true;
                //this.lbtnContinuarPedido.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void verCarroPedidoEditar()
        {
            try
            {
                this.phCarro.Controls.Clear();
                //vacio place holder
                Pedido p = Session["PedidoCliente"] as Pedido;

                foreach (ItemPedido item in p.items)
                {
                    this.cargarCarroPh(item.articulo, p.items.IndexOf(item), item.cantidad, p.cliente);
                }

                //this.lbtnVerPedido.Visible = false;
                this.lbtnGenerarPedido.Visible = true;
                //this.lbtnContinuarPedido.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void buscar(string art)
        {
            try
            {
                List<Articulo> articulos = new List<Articulo>();
                //this.LitFiltro.Text = "Articulo " + art;

                articulos = this.controlador.buscarArticuloListStore(art);
                this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void filtrar(int grupo, int subgrupo, int proveedor, int dias, int marca)
        {
            try
            {
                controladorCliente contCli = new controladorCliente();
                if (grupo > 0)
                {
                    string Sgrupo = this.ListGrupo.Items.FindByValue(grupo.ToString()).Text;
                    string SSubgrupo = "";
                    try
                    {
                        SSubgrupo = this.controlador.obtenerSubGrupoID(subgrupo).descripcion;
                    }
                    catch { }
                    //this.LitFiltro.Text = "Filtros: " + Sgrupo + ", " + SSubgrupo;

                }


                string sdias = null;
                    if (dias > 0)
                    {
                        sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                    }


                List<Articulo> articulos = this.controlador.filtrarArticulosGrupoSubGrupoStore(grupo, subgrupo, proveedor, sdias, marca, this.descSubGrupo);
                    this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void generarPedido(int cliente = 0, int borrador = 0)
        {
            try
            {
                ControladorPedido controladorPedido = new ControladorPedido();

                Pedido p = Session["PedidoCliente"] as Pedido;

                if (p.items.Count > 0)
                {
                    int idEmpresa = (int)Session["Login_EmpUser"];
                    int idSucursal = (int)Session["Login_SucUser"];
                    int idPtoVentaUser = (int)Session["Login_PtoUser"];

                    p.empresa = this.contSucursal.obtenerEmpresaID(idEmpresa);
                    p.sucursal = this.contSucursal.obtenerSucursalID(idSucursal);
                    p.ptoV = this.contSucursal.obtenerPtoVentaId(idPtoVentaUser);

                    if (borrador == 1)
                    {
                        p.estado = controladorPedido.obtenerEstadoDesc("Borrador");
                        p.cliente.id = cliente;

                    }
                    else
                    {
                        p.cliente.id = (int)Session["Login_Vendedor"];

                    }

                    Cliente c = this.contCliente.obtenerClienteID(p.cliente.id);
                    p.listaP.id = c.lisPrecio.id;
                    p.formaPAgo.id = c.formaPago.id;
                    p.vendedor.id = c.vendedor.id;

                    //obtengo total de suma de item
                    decimal totalC = p.obtenerTotalNeto();
                    decimal total = decimal.Round(totalC, 2);
                    p.neto = total;

                    //Subtotal = neto menos el descuento
                    p.descuento = 0;
                    p.subTotal = p.neto - p.descuento;

                    decimal iva = decimal.Round(p.obtenerTotalIva(), 2);
                    p.neto21 = iva;
                    p.totalSinDescuento = p.neto + p.obtenerTotalIva();
                    //retencion sobre el sub total
                    p.retencion = 0;

                    //total: subtotal + iva + retencion 
                    p.total = p.subTotal + p.neto21 + p.retencion;

                    p.fecha = DateTime.Now;
                    p.fechaEntrega = DateTime.Now;
                    p.horaEntrega = "";
                    p.domicilioEntrega = "";
                    p.zonaEntrega = "-1";
                    p.entrega.Id = -1;
                    p.senia = "0";
                    p.comentario = "-";
                    p.tipo = controladorPedido.obtenerTipoDoc("Pedido");
                    if(borrador ==0)
                    p.estado = controladorPedido.obtenerEstadoDesc("A Autorizar");


                    foreach (var item in p.items)
                    {
                        item.nroRenglon = p.items.IndexOf(item) + 1;
                    }

                    int i = controladorPedido.ProcesarPedido(p);
                    if (i > 0)
                    {
                        if (contPedidoEntity.verificarPadre(p.cliente.id) > 0)
                        {
                            int j = contPedidoEntity.agregarPedidoReferido(i, p.cliente.id);
                          
                        }
                                Session.Remove("PedidoCliente");

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedido generado con exito!. ", "../../Default.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar pedido. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar productos al pedido!. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando pedido. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    Response.Redirect("ArticulosC.aspx?accion=1&t=" + this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ArticulosC.aspx?accion=2&g=" + this.ListGrupo.SelectedValue + "&sg=" + this.ListSubGrupo.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error filtrando articulos. " + ex.Message));
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

        protected void lbtnGenerarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                Pedido p = Session["PedidoCliente"] as Pedido;
                if (p.id <= 0)
                {
                    this.generarPedido();
                }
                else
                {
                    this.modificarPedido();
                }
            }
            catch
            {

            }
        }

        private void modificarPedido(int cliente = 0, int borrador = 0)
        {
            try
            {
                ControladorPedido controladorPedido = new ControladorPedido();

                Pedido p = Session["PedidoCliente"] as Pedido;
                int idPedidoOriginal = p.id;

                if (p.items.Count > 0)
                {
                    int idEmpresa = (int)Session["Login_EmpUser"];
                    int idSucursal = (int)Session["Login_SucUser"];
                    int idPtoVentaUser = (int)Session["Login_PtoUser"];

                    p.empresa = this.contSucursal.obtenerEmpresaID(idEmpresa);
                    p.sucursal = this.contSucursal.obtenerSucursalID(idSucursal);
                    p.ptoV = this.contSucursal.obtenerPtoVentaId(idPtoVentaUser);

                    if (borrador == 1)
                    {
                        p.estado = controladorPedido.obtenerEstadoDesc("Borrador");
                        p.cliente.id = cliente;

                    }
                    else
                    {
                        p.cliente.id = (int)Session["Login_Vendedor"];
                        p.estado = controladorPedido.obtenerEstadoDesc("A Autorizar");

                    }

                    Cliente c = this.contCliente.obtenerClienteID(p.cliente.id);
                    p.listaP.id = c.lisPrecio.id;
                    p.formaPAgo.id = c.formaPago.id;
                    p.vendedor.id = c.vendedor.id;

                    //obtengo total de suma de item
                    decimal totalC = p.obtenerTotalNeto();
                    decimal total = decimal.Round(totalC, 2);
                    p.neto = total;

                    //Subtotal = neto menos el descuento
                    p.descuento = 0;
                    p.subTotal = p.neto - p.descuento;

                    decimal iva = decimal.Round(p.obtenerTotalIva(), 2);
                    p.neto21 = iva;
                    p.totalSinDescuento = p.neto + p.obtenerTotalIva();
                    //retencion sobre el sub total
                    p.retencion = 0;

                    //total: subtotal + iva + retencion 
                    p.total = p.subTotal + p.neto21 + p.retencion;

                    p.fecha = DateTime.Now;
                    p.fechaEntrega = DateTime.Now;
                    p.horaEntrega = "";
                    p.domicilioEntrega = "";
                    p.zonaEntrega = "-1";
                    p.entrega.Id = -1;
                    p.senia = "0";
                    p.comentario = "-";
                    p.tipo = controladorPedido.obtenerTipoDoc("Pedido");


                    foreach (var item in p.items)
                    {
                        item.nroRenglon = p.items.IndexOf(item) + 1;
                    }

                    int i = controladorPedido.ProcesarPedido(p);
                    if (i > 0)
                    {
                        ControladorPedidoEntity contPedEnt = new ControladorPedidoEntity();
                        if (contPedidoEntity.verificarPadre(p.cliente.id) > 0)
                        {
                            int j = contPedEnt.agregarPedidoReferido(i, p.cliente.id);
                            int k = contPedEnt.eliminarPedidoReferidoPorPedido(idPedidoOriginal);
                        }

                        contPedEnt.modificarNumeroPedidoEnt(p.id, p.numero);
                        string original = idPedidoOriginal + ";";
                        controladorPedido.anularPedidosModificados(original);
                        Session.Remove("PedidoCliente");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedido generado con exito!. ", "../../Default.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar pedido. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe cargar productos al pedido!. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando pedido. " + ex.Message));
            }
        }

        protected void lbtnVerPedido_Click(object sender, EventArgs e)
        {
            try
            {
                //this.verCarroPedido();
                Response.Redirect("ArticulosC.aspx?accion=3");
            }
            catch
            {

            }
        }

        protected void lbtnContinuarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ArticulosC");
                //this.lbtnContinuarPedido.Visible = false;
                //this.lbtnGenerarPedido.Visible = true;
                //this.lbtnVerPedido.Visible = true;
            }
            catch
            {

            }
        }

        private void CambiarCantidadItem(object sender, EventArgs e)
        {
            try
            {
                string boton = (sender as TextBox).ID.ToString();
                int idArticulo = Convert.ToInt32(boton.Split('_')[1]);
                int pos = Convert.ToInt32(boton.Split('_')[2]);
                string cantidad = (sender as TextBox).Text;

                if (!String.IsNullOrEmpty(cantidad) && Convert.ToDecimal(cantidad) > 0)
                {
                    //obtengo el pedido del session
                    Pedido p = Session["PedidoCliente"] as Pedido;

                    p.items[pos].cantidad = Convert.ToDecimal(cantidad);
                    p.items[pos].total = decimal.Round(p.items[pos].precioUnitario * p.items[pos].cantidad, 2, MidpointRounding.AwayFromZero);

                    Session.Add("PedidoCliente", p);
                    Response.Redirect("ArticulosC.aspx");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La cantidad tiene que ser mayor a 0."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: CambiarCantidadItem. " + ex.Message));
            }
        }

        private void QuitarItem(object sender, EventArgs e)
        {
            try
            {
                string boton = (sender as LinkButton).ID.ToString();
                int idArticulo = Convert.ToInt32(boton.Split('_')[1]);
                int pos = Convert.ToInt32(boton.Split('_')[2]);

                //obtengo el pedido del session
                Pedido p = Session["PedidoCliente"] as Pedido;
                p.items.Remove(p.items[pos]);
                Session.Add("PedidoCliente", p);

                //this.verCarroPedido();                
                Response.Redirect("ArticulosC.aspx");
            }
            catch (Exception ex)
            {
            }
        }
        //private void CargarSubGruposArticulos(object sender, EventArgs e)
        //{
        //    int idBoton = Convert.ToInt32((sender as LinkButton).ID);
        //    cargarBotonesDeLosSubGrupos(idBoton);

        //}

        private void AgregarItem(object sender, EventArgs e)
        {
            try
            {
                string idArticulo = (sender as LinkButton).ID.ToString();
                int idCliente = (int)Session["Login_Vendedor"];
                Cliente cl = this.contCliente.obtenerClienteID(idCliente);
                ItemPedido itemAagregar = new ItemPedido();
                int id = 0;

                foreach (Control c in this.phArticulos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txtCantidad = tr.Cells[4].Controls[0] as TextBox;
                    LinkButton btnArticulo = tr.Cells[4].Controls[2] as LinkButton;
                    id = Convert.ToInt32(btnArticulo.ID.Split('_')[1]);

                    //creo el item pedido
                    if (Convert.ToInt32(idArticulo.Split('_')[1]) == id)
                    {
                        string codigo = tr.Cells[1].Text;
                        Articulo a = this.controlador.obtenerArticuloFacturar(codigo, cl.lisPrecio.id);
                        itemAagregar.articulo = a;
                        itemAagregar.precioUnitario = a.precioVenta;
                        itemAagregar.descripcion = a.descripcion;
                        itemAagregar.cantidad = Convert.ToInt32(txtCantidad.Text);
                        itemAagregar.total = decimal.Round(itemAagregar.precioUnitario * itemAagregar.cantidad, 2, MidpointRounding.AwayFromZero);

                        agregarArticuloAlaSessionOmodificarSuCantidad(id, itemAagregar);
                         Response.Redirect(Request.RawUrl);  
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando item al carro. " + ex.Message));
            }
        }

        private void agregarArticuloAlaSessionOmodificarSuCantidad(int id, ItemPedido itemAagregar)
        {
            try
            {
                //obtengo el pedido del session
                Pedido p = Session["PedidoCliente"] as Pedido;

                //si el articulo ya esta en la session modifico la cantidad
                if (p.items.Where(x => x.articulo.id == id).FirstOrDefault() != null)
                {
                    p.items.Where(x => x.articulo.id == id).FirstOrDefault().cantidad += itemAagregar.cantidad;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cantidad actualizada con exito.", ""));
                }
                else
                {
                    p.items.Add(itemAagregar);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo agregado al carro.", ""));
                }
                Session.Remove("PedidoCliente");
                Session.Add("PedidoCliente", p);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: agregarArticuloAlaSessionOmodificarSuCantidad. " + ex.Message));
            }
        }

        private void cargarBotonesDeLosGrupos()
        {
            try
            {
                this.phBotonesGrupos.Controls.Clear();

                DataTable dtGrupos = controlador.obtenerGruposStore();

                char letra = 'A';
                string colorBoton = "btn btn-primary";
                string colorNaranja = "btn btn-primary";
                string colorAzul = "btn btn-info";

                //boton de todos los grupos
                LinkButton btnTodosLosGrupos = new LinkButton();
                btnTodosLosGrupos.ID = "btnTodosLosGrupos";
                btnTodosLosGrupos.Text = "TODOS LOS GRUPOS";
                btnTodosLosGrupos.Attributes.Add("href", "ArticulosC.aspx?accion=0");
                btnTodosLosGrupos.Attributes.Add("Style", "margin:2px");
                btnTodosLosGrupos.CssClass = colorBoton;

                HtmlGenericControl icono = new HtmlGenericControl();
                icono.InnerHtml = "<i class='icon-refresh'></i>";

                btnTodosLosGrupos.Controls.Add(icono);
                phBotonesGrupos.Controls.Add(btnTodosLosGrupos);

                foreach (DataRow fila in dtGrupos.Rows)
                {
                    int idGrupo = Convert.ToInt32(fila["id"]);
                    string descripcion = fila["descripcion"].ToString().Trim().ToUpper();

                    LinkButton btnGrupo = new LinkButton();
                    btnGrupo.ID = "btnGrupo_" + idGrupo;
                    btnGrupo.Text = descripcion;
                    btnGrupo.Attributes.Add("href", "ArticulosC.aspx?accion=2&g=" + idGrupo + "&sg=-1");
                    btnGrupo.Attributes.Add("Style", "margin:2px");
                    //btnGrupo.Click += new EventHandler(this.CargarSubGruposArticulos);
                    if (!String.IsNullOrWhiteSpace(descripcion))
                    {
                        //char auxLetraBtn = descripcion[0];
                        //if (!auxLetraBtn.Equals(letra))
                        //{
                        //    letra = auxLetraBtn;
                        //    if (colorBoton.Contains("primary"))
                        //    {
                        //        colorBoton = colorAzul;
                        //    }
                        //    else
                        //    {
                        //        colorBoton = colorNaranja;
                        //    }
                        //}
                        btnGrupo.CssClass = colorBoton;
                        phBotonesGrupos.Controls.Add(btnGrupo);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarBotonesDeLosSubGrupos(int grupo)
        {
            try
            {
                this.phBotonesSubGrupos.Controls.Clear();

                DataTable dtSubGrupos = controlador.obtenerSubGruposArticulos(grupo);

                char letra = 'A';
                string colorBoton = "btn btn-primary";
                string colorNaranja = "btn btn-primary";
                string colorAzul = "btn btn-info";

                //boton de todos los grupos
                LinkButton btnTodosLosSubGrupos = new LinkButton();
                btnTodosLosSubGrupos.ID = "btnTodosLosSubGrupos";
                btnTodosLosSubGrupos.Attributes.Add("href", "ArticulosC.aspx?g=" + grupo + "&sg=-1");
                btnTodosLosSubGrupos.Attributes.Add("Style", "margin:2px");
                btnTodosLosSubGrupos.CssClass = colorBoton;

                HtmlGenericControl icono = new HtmlGenericControl();
                icono.InnerHtml = "<i class='icon-refresh'></i>";

                btnTodosLosSubGrupos.Controls.Add(icono);

                phBotonesSubGrupos.Controls.Add(btnTodosLosSubGrupos);

                if (grupo > 0)
                {
                    foreach (DataRow fila in dtSubGrupos.Rows)
                    {
                        int idSubGrupo = Convert.ToInt32(fila["id"]);
                        string descripcion = fila["descripcion"].ToString().Trim().ToUpper();

                        LinkButton btnSubGrupo = new LinkButton();
                        btnSubGrupo.ID = "btnSubGrupo_" + idSubGrupo;
                        btnSubGrupo.Text = descripcion;
                        btnSubGrupo.Attributes.Add("href", "ArticulosC.aspx?accion=2&g=" + grupo + "&sg=" + idSubGrupo);
                        btnSubGrupo.Attributes.Add("Style", "margin:2px");
                        if (!String.IsNullOrWhiteSpace(descripcion))
                        {
                            //char auxLetraBtn = descripcion[0];
                            //if (!auxLetraBtn.Equals(letra))
                            //{
                            //    letra = auxLetraBtn;
                            //    if (colorBoton.Contains("primary"))
                            //    {
                            //        colorBoton = colorAzul;
                            //    }
                            //    else
                            //    {
                            //        colorBoton = colorNaranja;
                            //    }
                            //}
                            btnSubGrupo.CssClass = colorBoton;
                            phBotonesSubGrupos.Controls.Add(btnSubGrupo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void lbtnGuardarComentarios_Click(object sender, EventArgs e)
        {
            try
            {
                //obtengo el pedido del session
                Pedido p = Session["PedidoCliente"] as Pedido;

                //actualizo el comentario en la session
                p.comentario = txtComentarios.Text;

                Session.Remove("PedidoCliente");
                Session.Add("PedidoCliente", p);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Comentarios guardados con exito.", ""));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: lbtnGuardarComentarios_Click. " + ex.Message));
            }
        }
    
    private void cargarComentarioDesdeLaSession()
    {
        try
        {
            //obtengo el pedido del session
            Pedido p = Session["PedidoCliente"] as Pedido;

            if (p != null)
            {
                txtComentarios.Text = p.comentario;
            }
            Session.Remove("PedidoCliente");
            Session.Add("PedidoCliente", p);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: cargarComentarioDesdeLaSession. " + ex.Message));
        }
    }

        protected void lbtnGenerarPedidoBorrador_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalPedidoBorrador();", true);

            }
            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando pedido. " + ex.Message));
            }

        }

        protected void lbtnGenerarPedidoModalBorrador_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["PedidoCliente"] != null)
            {
                Pedido p = Session["PedidoCliente"] as Pedido;
                if (p.id <= 0)
                {
                    generarPedido(Convert.ToInt32(ListClientes.SelectedValue), 1);

                }
                else
                {
                    this.modificarPedido(Convert.ToInt32(ListClientes.SelectedValue), 1);
                }
            }
        }

        catch (Exception ex)
        {

        }
    }
}
}
