using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ArticulosTest : System.Web.UI.Page
    {
        private controladorArticulo controlador = new controladorArticulo();
        Mensajes m = new Mensajes();
        int accion;

        public Dictionary<string, string> camposArticulos = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                //if (!IsPostBack)
                //{
                if (String.IsNullOrEmpty(txtBusqueda.Text))
                {
                    this.cargarArticulos();
                }
                else
                {
                    this.buscar(txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {

            }

            //}
        }

        /// <summary>
        /// Carga los articulos en la tabla de articulos
        /// </summary>
        //private void cargarArticulos()
        //{
        //    try
        //    {
        //        List<Articulo> articulos = new List<Articulo>();
        //        articulos = this.controlador.obtenerArticulosReduc();

        //        this.cargarArticulosTabla(articulos);



        //        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos cargados con exito"));
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos. " + ex.Message));
        //    }
        //}

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

        private void cargarArticulos()
        {
            try
            {

                List<Articulo> articulos = new List<Articulo>();
                articulos = this.controlador.obtenerArticulosReduc();

                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el articulo
                int i = 0;

                foreach (Articulo art in articulos)
                {
                    //Celdas
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = art.codigo;
                    celCodigo.Width = Unit.Percentage(5);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = art.descripcion;
                    celDescripcion.Width = Unit.Percentage(40);
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;

                    TableCell celPorcentaje = new TableCell();
                    celPorcentaje.Text = art.porcentajeIva.ToString() + "%";
                    celPorcentaje.Width = Unit.Percentage(5);
                    celPorcentaje.VerticalAlign = VerticalAlign.Middle;
                    celPorcentaje.HorizontalAlign = HorizontalAlign.Right;

                    TableCell celGrupo = new TableCell();
                    celGrupo.Text = art.grupo.descripcion;
                    celGrupo.Width = Unit.Percentage(15);
                    celGrupo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celSubGrupo = new TableCell();
                    celSubGrupo.Text = art.subGrupo.descripcion;
                    celSubGrupo.Width = Unit.Percentage(15);
                    celSubGrupo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celMoneda = new TableCell();
                    celMoneda.Text = art.monedaVenta.moneda;
                    celMoneda.Width = Unit.Percentage(5);
                    celMoneda.VerticalAlign = VerticalAlign.Middle;

                    TableCell celPrecio = new TableCell();
                    celPrecio.Text = "$" + art.precioVenta.ToString();
                    celPrecio.Width = Unit.Percentage(10);
                    celPrecio.VerticalAlign = VerticalAlign.Middle;
                    celPrecio.HorizontalAlign = HorizontalAlign.Right;

                    LinkButton btnDetails = new LinkButton();
                    TableCell celAction = new TableCell();
                    btnDetails.ID = art.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-search'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    btnDetails.Click += new EventHandler(this.mostrarArticuloDetalles);
                    celAction.Controls.Add(btnDetails);
                    celAction.Width = Unit.Percentage(5);
                    celAction.VerticalAlign = VerticalAlign.Middle;
                    celAction.HorizontalAlign = HorizontalAlign.Center;

                    //cargo el primer cliente en detalle
                    if (i == 0)
                    {
                        this.cargarArticuloDetalle(art.id);
                        //agrego el link al boton editar
                        this.linkEditar.HRef = "ArticulosABM.aspx?accion=2&id=" + art.id;
                        this.linkStock.HRef = "StockF.aspx?articulo=" + art.id;
                    }

                    i++;


                    TableRow tr = new TableRow();
                    tr.ID = art.id + "1";

                    //arego fila a tabla
                    //table.Controls.Add(tr);
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celDescripcion);
                    tr.Cells.Add(celPorcentaje);
                    tr.Cells.Add(celGrupo);
                    tr.Cells.Add(celSubGrupo);
                    tr.Cells.Add(celMoneda);
                    tr.Cells.Add(celPrecio);
                    tr.Cells.Add(celAction);
                    //arego fila a tabla
                    //table.Controls.Add(tr);

                    this.phArticulos.Controls.Add(tr);

                }
                //agrego la tabla al placeholder

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// carga la lista de articulos en la tabla de la pantalla
        /// </summary>
        /// <param name="articulos"></param>
        private void cargarArticulosTabla(List<Articulo> articulos)
        {
            try
            {
                //vacio place holder
                this.phArticulos.Controls.Clear();

                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                foreach (Articulo art in articulos)
                {

                    //Celdas
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = art.codigo;
                    celCodigo.Width = Unit.Percentage(5);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = art.descripcion;
                    celDescripcion.Width = Unit.Percentage(40);
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;

                    TableCell celPorcentaje = new TableCell();
                    celPorcentaje.Text = art.porcentajeIva.ToString() + "%";
                    celPorcentaje.Width = Unit.Percentage(5);
                    celPorcentaje.VerticalAlign = VerticalAlign.Middle;
                    celPorcentaje.HorizontalAlign = HorizontalAlign.Right;

                    TableCell celGrupo = new TableCell();
                    celGrupo.Text = art.grupo.descripcion;
                    celGrupo.Width = Unit.Percentage(15);
                    celGrupo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celSubGrupo = new TableCell();
                    celSubGrupo.Text = art.subGrupo.descripcion;
                    celSubGrupo.Width = Unit.Percentage(15);
                    celSubGrupo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celMoneda = new TableCell();
                    celMoneda.Text = art.monedaVenta.moneda;
                    celMoneda.Width = Unit.Percentage(5);
                    celMoneda.VerticalAlign = VerticalAlign.Middle;

                    TableCell celPrecio = new TableCell();
                    celPrecio.Text = "$" + art.precioVenta.ToString();
                    celPrecio.Width = Unit.Percentage(10);
                    celPrecio.VerticalAlign = VerticalAlign.Middle;
                    celPrecio.HorizontalAlign = HorizontalAlign.Right;

                    LinkButton btnDetails = new LinkButton();
                    TableCell celAction = new TableCell();
                    btnDetails.ID = art.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-search'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    btnDetails.Click += new EventHandler(this.mostrarArticuloDetalles);
                    celAction.Controls.Add(btnDetails);
                    celAction.Width = Unit.Percentage(5);
                    celAction.VerticalAlign = VerticalAlign.Middle;
                    celAction.HorizontalAlign = HorizontalAlign.Center;

                    //cargo el primer cliente en detalle
                    if (i == 0)
                    {
                        this.cargarArticuloDetalle(art.id);
                        //agrego el link al boton editar
                        this.linkEditar.HRef = "ArticulosABM.aspx?accion=2&id=" + art.id;
                        this.linkStock.HRef = "StockF.aspx?articulo=" + art.id;
                    }

                    i++;


                    TableRow tr = new TableRow();
                    tr.ID = art.id + "1";

                    //arego fila a tabla
                    //table.Controls.Add(tr);
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celDescripcion);
                    tr.Cells.Add(celPorcentaje);
                    tr.Cells.Add(celGrupo);
                    tr.Cells.Add(celSubGrupo);
                    tr.Cells.Add(celMoneda);
                    tr.Cells.Add(celPrecio);
                    tr.Cells.Add(celAction);
                    //arego fila a tabla
                    //table.Controls.Add(tr);

                    this.phArticulos.Controls.Add(tr);

                }
                //agrego la tabla al placeholder

            }
            catch (Exception ex)
            {

            }
        }

        private void mostrarArticuloDetalles(object sender, EventArgs e)
        {
            try
            {
                //this.cargarArticuloDetalle(Convert.ToInt32((sender as LinkButton).ID));
                //agrego el link al boton editar
                this.linkEditar.HRef = "ArticulosABM.aspx?accion=2&id=" + (sender as LinkButton).ID;

                this.linkStock.HRef = "StockF.aspx?articulo=" + (sender as LinkButton).ID;

                TableRow tr = this.phArticulos.Controls[0].FindControl((sender as LinkButton).ID + "1") as TableRow;
                string hex = "#cc7a00";

                //tr.CssClass = "active";
                //Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                //tr.BackColor = _color;
                //tr.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de articulos desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        /// <summary>
        /// Carga los datos del cliente en el detalle de la pagina
        /// </summary>
        /// <param name="cuit">cuit del cliente que se quiere mostrar</param>
        private void cargarArticuloDetalle(int id)
        {
            try
            {
                //borro
                //phArtDetalle.Controls.Clear();

                //obtengo cliente con el cuit
                this.camposArticulos = this.controlador.obtenerCamposArticuloID(id);
                this.cargarEncabezadoDetalle(id);
                //cargo los campos
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                //Label1.Text = ""; 
                foreach (KeyValuePair<string, string> kvp in camposArticulos)
                {
                    //lo agrego a la etiqueta en el widget
                    //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                    //fila
                    TableRow tr = new TableRow();

                    //Celdas

                    TableCell celTitulo = new TableCell();
                    celTitulo.Text = kvp.Key;
                    celTitulo.Width = Unit.Percentage(30);
                    celTitulo.ForeColor = Color.Black;
                    //celTitulo.BorderStyle = BorderStyle.Solid;
                    celTitulo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celTitulo);


                    TableCell celValor = new TableCell();
                    celValor.Text = kvp.Value;
                    celValor.Width = Unit.Percentage(70);
                    celValor.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celValor);

                    //arego fila a tabla
                    table.Controls.Add(tr);

                }
                //phArtDetalle.Controls.Add(table);

                return;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos detalle desde la interfaz. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga los datos de codigo y descripcion del articulo como titulos
        /// </summary>
        /// <param name="cuit">codigo del articulo a mostrar</param>
        private void cargarEncabezadoDetalle(int id)
        {
            try
            {
                //this.phArtEncabezado.Controls.Clear();
                Articulo art = this.controlador.obtenerArticuloId(id);
                //cargo los campos
                Table table = new Table();
                table.CssClass = "btn btn-primary";
                table.Width = Unit.Percentage(100);
                //Label1.Text = "";

                //lo agrego a la etiqueta en el widget
                //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo + " - " + art.descripcion;
                celCodigo.Attributes.Add("Style", "font-size:medium");
                //celCodigo.ForeColor = Color.White;
                //string hex = "#cc7a00";
                //Color color = System.Drawing.ColorTranslator.FromHtml(hex);
                //celCodigo.BackColor = color;
                celCodigo.Width = Unit.Percentage(100);

                //celTitulo.BorderStyle = BorderStyle.Solid;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);


                //TableCell celDescrip = new TableCell();
                //celDescrip.Text = art.descripcion;
                //celDescrip.Width = Unit.Percentage(70);
                //celDescrip.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celDescrip);

                //arego fila a tabla
                table.Controls.Add(tr);


                //phArtEncabezado.Controls.Add(table);

                return;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando encabezado desde la interfaz. " + ex.Message));
            }
        }

        private void buscar(string art)
        {
            try
            {
                List<Articulo> articulos = new List<Articulo>();
                string query = "select * from articulos where descripcion like '%" + art + "%'";
                articulos = this.controlador.buscarArticuloList(query);
                this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.buscar(this.txtBusqueda.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }
    }
}