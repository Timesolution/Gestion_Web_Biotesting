using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
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
    public partial class ArticulosC : System.Web.UI.Page
    {
        private controladorListaPrecio controladorPrecio = new controladorListaPrecio();
        private controladorArticulo controlador = new controladorArticulo();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
        private controladorSucursal contSucursal = new controladorSucursal();
        private controladorCliente contCliente = new controladorCliente();

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

                if (!IsPostBack)
                {
                    if (Session["PedidoCliente"] == null)
                    {
                        Session.Add("PedidoCliente", new Pedido());
                    }
                    //cargo combos
                    this.cargarGruposArticulos();
                    this.cargarSubGruposArticulos(Convert.ToInt32(ListGrupo.SelectedValue));
                    this.cargarBotonesDeLosGrupos();
                }
                this.lbtnVerPedido.Visible = true;
                //ver carro
                if (this.accion == 3)
                {
                    this.lbtnVerPedido.Visible = false;
                    this.verCarroPedido();
                }
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
                    articulos = this.controlador.obtenerArticulosReduc();
                    this.cargarArticulosTabla(articulos);
                }
                this.txtBusqueda.Focus();
                Page.Form.DefaultButton = this.lbBuscar.UniqueID;

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

                foreach (Articulo art in articulos)
                {
                    if (art.apareceLista == 1)
                    {
                        //cargo articulo aal ph
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
                catch { }

                celImagen.Width = Unit.Percentage(10);
                tr.Cells.Add(celImagen);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo;
                celCodigo.Width = Unit.Percentage(10);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = art.descripcion;
                celDescripcion.Width = Unit.Percentage(30);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celMarca = new TableCell();
                Gestion_Api.Entitys.articulo a = this.contArtEntity.obtenerArticuloEntity(art.id);
                if (a != null)
                {
                    if (a.Articulos_Marca.Count > 0)
                    {
                        celMarca.Text = a.Articulos_Marca.FirstOrDefault().Marca.marca1;
                    }
                    else
                    {
                        celMarca.Text = "SIN MARCA";
                    }
                }
                else
                {
                    celMarca.Text = "SIN MARCA";
                }
                //celMarca.Text = art.grupo.descripcion;
                celMarca.Width = Unit.Percentage(10);
                celMarca.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMarca);

                TableCell celGrupo = new TableCell();
                celGrupo.Text = art.grupo.descripcion;
                celGrupo.Width = Unit.Percentage(10);
                celGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celGrupo);

                TableCell celSubGrupo = new TableCell();
                celSubGrupo.Text = art.subGrupo.descripcion;
                celSubGrupo.Width = Unit.Percentage(10);
                celSubGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSubGrupo);

                TableCell celPrecio = new TableCell();
                //celPrecio.Text = art.precioVenta.ToString("C");
                //celPrecio.Text = this.controlador.obtenerArticuloFacturar(art.codigo, cl.lisPrecio.id).precioSinIva.ToString("C");
                Articulo articulo = this.controlador.obtenerArticuloFacturar(art.codigo, cl.lisPrecio.id);
                decimal totalSIva = (articulo.precioVenta / (1 + (articulo.porcentajeIva / 100)));
                celPrecio.Text = totalSIva.ToString("C");
                celPrecio.Width = Unit.Percentage(5);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);

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

        private void cargarCarroPh(Articulo art, int pos, decimal cantidad)
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
                        gallery.Text += "<img height=\"100\" width = \"100\" src=\"/images/Productos/" + art.id + "/" + fi.Name + "\" alt=\"\" />";
                        gallery.Text += @"</a>";
                        gallery.Text += @"<a href=../../images/Productos/" + art.id + "/" + fi.Name + " class=\"preview\"></a>";
                        gallery.Text += @" </li>";
                        gallery.Text += "<br/>";

                        celImagen.Controls.Add(gallery);
                    }
                }
                catch { }

                celImagen.Width = Unit.Percentage(10);
                tr.Cells.Add(celImagen);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo;
                celCodigo.Width = Unit.Percentage(10);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = art.descripcion;
                celDescripcion.Width = Unit.Percentage(30);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celMarca = new TableCell();
                Gestion_Api.Entitys.articulo a = this.contArtEntity.obtenerArticuloEntity(art.id);
                if (a != null)
                {
                    if (a.Articulos_Marca.Count > 0)
                    {
                        celMarca.Text = a.Articulos_Marca.FirstOrDefault().Marca.marca1;
                    }
                    else
                    {
                        celMarca.Text = "SIN MARCA";
                    }
                }
                else
                {
                    celMarca.Text = "SIN MARCA";
                }
                celMarca.Width = Unit.Percentage(10);
                celMarca.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMarca);

                TableCell celGrupo = new TableCell();
                celGrupo.Text = art.grupo.descripcion;
                celGrupo.Width = Unit.Percentage(10);
                celGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celGrupo);

                TableCell celSubGrupo = new TableCell();
                celSubGrupo.Text = art.subGrupo.descripcion;
                celSubGrupo.Width = Unit.Percentage(10);
                celSubGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSubGrupo);

                TableCell celPrecio = new TableCell();
                //celPrecio.Text = art.precioVenta.ToString("C");
                celPrecio.Text = art.precioSinIva.ToString("C");
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
                //txtCantidad.Attributes.Add("disabled", "disabled");
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
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

                celAction.Width = Unit.Percentage(15);
                tr.Cells.Add(celAction);

                this.phCarro.Controls.Add(tr);
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
                this.phCarro.Controls.Clear();
                this.phArticulos.Controls.Clear();
                Pedido p = Session["PedidoCliente"] as Pedido;

                foreach (ItemPedido item in p.items)
                {
                    this.cargarCarroPh(item.articulo, p.items.IndexOf(item), item.cantidad);
                }

                this.lbtnVerPedido.Visible = false;
                this.lbtnGenerarPedido.Visible = true;
                this.lbtnContinuarPedido.Visible = true;
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
                this.LitFiltro.Text = "Articulo " + art;

                articulos = this.controlador.buscarArticuloList(art);
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
                string Sgrupo = this.ListGrupo.Items.FindByValue(grupo.ToString()).Text;
                string SSubgrupo = "";
                try
                {
                    SSubgrupo = this.controlador.obtenerSubGrupoID(subgrupo).descripcion;
                }
                catch { }

                string sdias = null;
                if (dias > 0)
                {
                    sdias = DateTime.Today.AddDays(dias * -1).ToString("yyyyMMdd");
                }
                this.LitFiltro.Text = "Filtros: " + Sgrupo + ", " + SSubgrupo;

                List<Articulo> articulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor, sdias, marca, this.descSubGrupo);
                this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void generarPedido()
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

                    p.cliente.id = (int)Session["Login_Vendedor"];

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
                    p.estado = controladorPedido.obtenerEstadoDesc("A Autorizar");

                    foreach (var item in p.items)
                    {
                        item.nroRenglon = p.items.IndexOf(item) + 1;
                    }

                    int i = controladorPedido.ProcesarPedido(p);
                    if (i > 0)
                    {
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
                this.generarPedido();
            }
            catch
            {

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

                if (!String.IsNullOrEmpty(cantidad))
                {
                    //obtengo el pedido del session
                    Pedido p = Session["PedidoCliente"] as Pedido;

                    p.items[pos].cantidad = Convert.ToDecimal(cantidad);
                    p.items[pos].total = decimal.Round(p.items[pos].precioUnitario * p.items[pos].cantidad, 2, MidpointRounding.AwayFromZero);

                    Session.Add("PedidoCliente", p);
                    Response.Redirect("ArticulosC.aspx?accion=3");
                }

            }
            catch (Exception ex)
            {
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
                Response.Redirect("ArticulosC.aspx?accion=3");
            }
            catch (Exception ex)
            {
            }
        }

        private void AgregarItem(object sender, EventArgs e)
        {
            try
            {
                string idArticulo = (sender as LinkButton).ID.ToString();
                int idCliente = (int)Session["Login_Vendedor"];
                Cliente cl = this.contCliente.obtenerClienteID(idCliente);

                //obtengo el pedido del session
                Pedido p = Session["PedidoCliente"] as Pedido;
                ItemPedido i = new ItemPedido();

                foreach (Control c in this.phArticulos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txtCantidad = tr.Cells[7].Controls[0] as TextBox;
                    LinkButton btnArticulo = tr.Cells[7].Controls[2] as LinkButton;
                    int id = Convert.ToInt32(btnArticulo.ID.Split('_')[1]);

                    if (Convert.ToInt32(idArticulo.Split('_')[1]) == id)
                    {
                        string codigo = tr.Cells[1].Text;
                        Articulo a = this.controlador.obtenerArticuloFacturar(codigo, cl.lisPrecio.id);
                        i.articulo = a;
                        i.precioUnitario = a.precioVenta;
                        i.descripcion = a.descripcion;
                        i.cantidad = Convert.ToInt32(txtCantidad.Text);
                        i.total = decimal.Round(i.precioUnitario * i.cantidad, 2, MidpointRounding.AwayFromZero);

                    }
                }

                p.items.Add(i);
                Session.Add("PedidoCliente", p);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Articulo agregado al carro.", ""));

                //this.verCarroPedido();
                Response.Redirect("ArticulosC.aspx?accion=3");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando item al carro. " + ex.Message));
            }
        }

        private void cargarBotonesDeLosGrupos()
        {
            try
            {
                this.phBotonesGrupos.Controls.Clear();

                DataTable dtGrupos = controlador.obtenerGruposArticulos();

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

                    char auxLetraBtn = descripcion[0];
                    if (!auxLetraBtn.Equals(letra))
                    {
                        letra = auxLetraBtn;
                        if (colorBoton.Contains("primary"))
                        {
                            colorBoton = colorAzul;
                        }
                        else
                        {
                            colorBoton = colorNaranja;
                        }
                    }
                    btnGrupo.CssClass = colorBoton;
                    phBotonesGrupos.Controls.Add(btnGrupo);
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
