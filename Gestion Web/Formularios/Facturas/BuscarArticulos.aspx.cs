using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class BuscarArticulos : System.Web.UI.Page
    {
        private controladorArticulo controlador = new controladorArticulo();
        private ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();
        private Mensajes m = new Mensajes();
        private static int accion = 0;
        private static int idSucursal = 0;
        private string buscarText;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                accion = Convert.ToInt32(Request.QueryString["accion"]);
                buscarText = Request.QueryString["b"];
                idSucursal = Convert.ToInt32(Request.QueryString["suc"]);

                txtBuscarArticulos.Focus();                   
                    
                Buscar();                              

                Form.DefaultButton = lbBuscarArticulos.UniqueID;

                if (accion == 1)
                    lbtnAgregarArticulosMultiples.Visible = true;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));

            }
        }

        private void CargarArticulos()
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
                        cbSeleccion.Font.Size = 1;
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
                    int idSuc = idSucursal;
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

        private void Buscar()
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
                        articulos = this.controlador.obtenerArticulosReduc_Sucursales(idSucursal);
                    }
                }
                else
                {
                    //pregunta por la configuracion de si solo se quiere mostrar articulos en esa sucursal
                    if (configuracion.FiltroArticulosSucursal == "1")
                    {
                        articulos = this.controlador.buscarArticuloListReduc_Sucursales(this.buscarText, idSucursal);
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
                    Response.Redirect("BuscarArticulos.aspx?accion=" + accion + "&b=" + this.txtBuscarArticulos.Text + "&suc=" + idSucursal);
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