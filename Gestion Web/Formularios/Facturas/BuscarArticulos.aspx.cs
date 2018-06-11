using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class BuscarArticulos : System.Web.UI.Page
    {
        private controladorArticulo controlador = new controladorArticulo();
        private Mensajes m = new Mensajes();
        private int accion;
        private string buscarText;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.buscarText = Request.QueryString["b"];
                
                this.txtBuscarArticulos.Focus();

                this.buscar();

                this.Form.DefaultButton = lbBuscarArticulos.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));

            }
        }

        private void cargarArticulos()
        {
            try
            {

                List<Articulo> articulos = new List<Articulo>();
                articulos = this.controlador.obtenerArticulosReduc();
                this.cargarArticulosTabla(articulos);
                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                ////para cargar el articulo
                //int i = 0;

                //foreach (Articulo art in articulos)
                //{
                //    //Celdas
                //    TableCell celCodigo = new TableCell();
                //    celCodigo.Text = art.codigo;
                //    celCodigo.Width = Unit.Percentage(15);
                //    celCodigo.VerticalAlign = VerticalAlign.Middle;

                //    TableCell celDescripcion = new TableCell();
                //    celDescripcion.Text = art.descripcion;
                //    celDescripcion.Width = Unit.Percentage(40);
                //    celDescripcion.VerticalAlign = VerticalAlign.Middle;

                //    TableCell celMoneda = new TableCell();
                //    celMoneda.Text = art.monedaVenta.moneda;
                //    celMoneda.Width = Unit.Percentage(15);
                //    celMoneda.VerticalAlign = VerticalAlign.Middle;
                //    celMoneda.HorizontalAlign = HorizontalAlign.Right;

                //    TableCell celPrecio = new TableCell();
                //    celPrecio.Text = "$" + art.precioVenta.ToString();
                //    celPrecio.Width = Unit.Percentage(20);
                //    celPrecio.VerticalAlign = VerticalAlign.Middle;

                //    LinkButton btnDetails = new LinkButton();
                //    TableCell celAction = new TableCell();
                //    btnDetails.ID = art.codigo.ToString();
                //    btnDetails.CssClass = "btn btn-info";
                //    btnDetails.Text = "<span class='shortcut-icon icon-ok'></span>";
                //    //btnDetails.Height = Unit.Pixel(30);
                //    btnDetails.Font.Size = 9;
                //    btnDetails.Click += new EventHandler(this.RedireccionarArticulos);
                //    celAction.Controls.Add(btnDetails);
                //    celAction.Width = Unit.Percentage(10);
                //    celAction.VerticalAlign = VerticalAlign.Middle;


                //    TableRow tr = new TableRow();
                //    tr.ID = art.id + "1";

                //    //arego fila a tabla

                //    tr.Cells.Add(celCodigo);
                //    tr.Cells.Add(celDescripcion);
                //    tr.Cells.Add(celMoneda);
                //    tr.Cells.Add(celPrecio);
                //    tr.Cells.Add(celAction);

                //    table.Controls.Add(tr);

                //}
                ////agrego la tabla al placeholder
                //this.phArticulos.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Articulo en la Lista. " + ex.Message));

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

                Table table = new Table();
                table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                foreach (Articulo art in articulos)
                {

                    //Celdas
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = art.codigo;
                    //celCodigo.Width = Unit.Percentage(25);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;
                    celCodigo.HorizontalAlign = HorizontalAlign.Left;

                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = art.descripcion;
                    //celDescripcion.Width = Unit.Percentage(40);
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    celDescripcion.HorizontalAlign = HorizontalAlign.Left;


                    TableCell celMoneda = new TableCell();
                    celMoneda.Text = art.monedaVenta.moneda;
                    //celMoneda.Width = Unit.Percentage(10);
                    celMoneda.VerticalAlign = VerticalAlign.Middle;
                    celMoneda.HorizontalAlign = HorizontalAlign.Left;

                    TableCell celPrecio = new TableCell();
                    celPrecio.Text = "$" + art.precioVenta.ToString();
                    //celPrecio.Width = Unit.Percentage(20);
                    celPrecio.VerticalAlign = VerticalAlign.Middle;
                    celPrecio.HorizontalAlign = HorizontalAlign.Right;

                    LinkButton btnDetails = new LinkButton();
                    TableCell celAction = new TableCell();
                    btnDetails.ID = "btn_" + art.codigo.ToString();
                   btnDetails.CssClass = "btn btn-info";
                   btnDetails.Text = "<span class='shortcut-icon icon-ok'></span>";
                   //btnDetails.Height = Unit.Pixel(30);
                   btnDetails.Font.Size = 9;
                   btnDetails.Click += new EventHandler(this.RedireccionarArticulos);
                    celAction.Controls.Add(btnDetails);
                    //celAction.Width = Unit.Percentage(10);
                    celAction.VerticalAlign = VerticalAlign.Middle;


                    TableRow tr = new TableRow();
                    tr.ID = "TR_" + art.id + "1";

                    if (art.apareceLista == 1)
                    {
                        //arego fila a tabla
                        table.Controls.Add(tr);
                        tr.Cells.Add(celCodigo);
                        tr.Cells.Add(celDescripcion);
                        tr.Cells.Add(celMoneda);
                        tr.Cells.Add(celPrecio);
                        tr.Cells.Add(celAction);
                        //arego fila a tabla
                        //table.Controls.Add(tr);
                    }

                }
                //agrego la tabla al placeholder
                this.phArticulos.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Articulo en la Lista. " + ex.Message));

            }
        }

        private void buscar()
        {
            try
            {
                List<Articulo> articulos = new List<Articulo>();
                if (String.IsNullOrEmpty(this.buscarText))
                {
                    
                    articulos = this.controlador.obtenerArticulosReduc();
                    
                }
                else
                {
                    articulos = this.controlador.buscarArticuloList(this.buscarText);
                }
                this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void btnBuscarArticulos_Click(object sender, EventArgs e)
        {
            try
            {
                if(!String.IsNullOrEmpty(this.txtBuscarArticulos.Text))
                {
                    Response.Redirect("BuscarArticulos.aspx?accion="+this.accion + "&b=" + this.txtBuscarArticulos.Text );
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }


        protected void RedireccionarArticulos(object sender, EventArgs e)
        {
            try
            {
                String id = (sender as LinkButton).ID.ToString().Split('_')[1];
                if (accion == 1)
                {
                    Session.Add("FacturasABM_ArticuloModal", id);
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMFacturas.aspx?accion=3&cliente=" + (sender as Button).ID);

                }
                if (accion == 2)
                {
                    Session.Add("CotizacionesABM_ArticuloModal", id);
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMCotizaciones.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
                if (accion == 3)
                {
                    Session.Add("RemitosABM_ArticuloModal", id);
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMRemitos.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
                if (accion == 4)
                {
                    Session.Add("PedidosABM_ArticuloModal", id);
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMPedidos.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
                if (accion == 5)
                {
                    Session.Add("ArticuloABM_ArticuloModal", id);
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMPedidos.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
            }
            catch
            {

            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                Modal.Close(this, "OK");
            }
            catch
            {

            }
        }
    }
}