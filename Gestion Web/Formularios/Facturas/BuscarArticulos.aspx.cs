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
        private ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
        private Mensajes m = new Mensajes();
        private int accion;
        private int idSucursal;
        private string buscarText;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.buscarText = Request.QueryString["b"];
                this.idSucursal = Convert.ToInt32(Request.QueryString["suc"]);

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
                //cargarHeaderTablaArticulos();
                //vacio place holder
                this.phArticulos.Controls.Clear();

                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";

                ////Celdas
                //TableCell celCodigoH = new TableCell();
                //celCodigoH.Text = "Codigo";
                //celCodigoH.VerticalAlign = VerticalAlign.Middle;
                //celCodigoH.HorizontalAlign = HorizontalAlign.Left;

                //TableCell celDescripcionH = new TableCell();
                //celDescripcionH.Text = "Descripcion";
                //celDescripcionH.VerticalAlign = VerticalAlign.Middle;
                //celDescripcionH.HorizontalAlign = HorizontalAlign.Left;

                //TableCell celStockH = new TableCell();
                //celStockH.Text = "Stock";
                //celStockH.VerticalAlign = VerticalAlign.Middle;
                //celStockH.HorizontalAlign = HorizontalAlign.Left;

                //TableCell celMonedaH = new TableCell();
                //celMonedaH.Text = "Moneda";
                //celMonedaH.VerticalAlign = VerticalAlign.Middle;
                //celMonedaH.HorizontalAlign = HorizontalAlign.Left;

                //TableCell celPrecioH = new TableCell();
                //celPrecioH.Text = "P.Venta";
                //celPrecioH.VerticalAlign = VerticalAlign.Middle;
                //celPrecioH.HorizontalAlign = HorizontalAlign.Left;

                //TableCell celAccionH = new TableCell();
                //celAccionH.Text = "";
                //celAccionH.VerticalAlign = VerticalAlign.Middle;
                //celAccionH.HorizontalAlign = HorizontalAlign.Left;

                //TableRow trH = new TableRow();

                ////arego fila a tabla
                //trH.Cells.Add(celCodigoH);
                //trH.Cells.Add(celDescripcionH);
                //trH.Cells.Add(celStockH);
                //trH.Cells.Add(celMonedaH);
                //trH.Cells.Add(celPrecioH);
                //trH.Cells.Add(celAccionH);

                ////arego fila a tabla
                //table.Controls.Add(trH);

                foreach (Articulo art in articulos)
                {
                    if (art.apareceLista == 0)
                    {
                        continue;
                    }

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

                    TableCell celStock = new TableCell();
                    celStock.Text = this.obtenerStockArticuloBySucursal(art.codigo).ToString();//traer el stock de ese articulo en esa sucursal
                    celStock.VerticalAlign = VerticalAlign.Middle;
                    celStock.HorizontalAlign = HorizontalAlign.Right;

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

                    if(accion == 1)
                    {
                        Literal l1 = new Literal();
                        l1.Text = "&nbsp";
                        celAction.Controls.Add(l1);

                        CheckBox cbSeleccion = new CheckBox();
                        cbSeleccion.ID = "cbSeleccion_" + art.codigo.ToString();
                        cbSeleccion.CssClass = "btn btn-info";
                        cbSeleccion.Font.Size = 3;
                        celAction.Controls.Add(cbSeleccion);
                    }                    

                    TableRow tr = new TableRow();
                    //tr.ID = "TR_" + art.id + "1";

                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celDescripcion);
                    tr.Cells.Add(celStock);
                    tr.Cells.Add(celMoneda);
                    tr.Cells.Add(celPrecio);
                    tr.Cells.Add(celAction);

                    this.phArticulos.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Articulo en la Lista. " + ex.Message));
            }
        }

        private decimal obtenerStockArticuloBySucursal(string codigoArticulo)//obtenerStockArticuloBySucursal
        {
            try
            {
                ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
                decimal stock = 0;
                Gestion_Api.Entitys.articulo artEnt = contArtEntity.obtenerArticuloEntityByCodigoYcodigoBarra(codigoArticulo);
                if (artEnt != null)
                {
                    int idSuc = this.idSucursal;
                    var stocks = contArtEntity.obtenerStockArticuloLocal(artEnt.id, idSuc);
                    stock = 0;

                    stock = stocks.stock1.Value;
                    return stock;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo stock de articulo por sucursal. " + ex.Message));
            }
            return 0;
        }

        private void buscar()
        {
            try
            {
                Configuracion configuracion = new Configuracion();
                List<Articulo> articulos = new List<Articulo>();
                if (String.IsNullOrEmpty(this.buscarText))
                {
                    articulos = this.controlador.obtenerArticulosReduc();
                    //pregunta por la configuracion de si solo se quiere mostrar articulos en esa sucursal
                    if (configuracion.FiltroArticulosSucursal == "1")
                    {
                        articulos = this.controlador.obtenerArticulosReduc_Sucursales(this.idSucursal);
                    }
                }
                else
                {
                    //pregunta por la configuracion de si solo se quiere mostrar articulos en esa sucursal
                    if (configuracion.FiltroArticulosSucursal == "1")
                    {
                        articulos = this.controlador.buscarArticuloListReduc_Sucursales(this.buscarText, this.idSucursal);
                    }
                    else
                    {
                        articulos = this.controlador.buscarArticuloList(this.buscarText);
                    }
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
                if (!String.IsNullOrEmpty(this.txtBuscarArticulos.Text))
                {
                    Response.Redirect("BuscarArticulos.aspx?accion=" + this.accion + "&b=" + this.txtBuscarArticulos.Text + "&suc=" + this.idSucursal);
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

        protected void lbtnAgregarArticulosMultiples_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> idMultiple = new List<string>();

                foreach (Control control in phArticulos.Controls)
                {
                    TableRow tr = control as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;

                    if (ch.Checked == true)
                        idMultiple.Add(ch.ID.Split('_')[1]);
                }

                if (idMultiple.Count <= 0)
                    return;

                if (accion == 1)
                {
                    Session.Add("FacturasABM_ArticuloModalMultiple", idMultiple);
                    Modal.Close(this, "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}