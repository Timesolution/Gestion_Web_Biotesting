using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ArticulosPedirF : System.Web.UI.Page
    {
        ControladorArticulosEntity controlador = new ControladorArticulosEntity();

        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cargarArticulosPedir();
        }

        private void cargarArticulosPedir()
        {
            try
            {
                var articulos = this.controlador.ObtenerArticulosPedir().OrderBy(x => x.articulo.descripcion).ToList();
                //borro todo
                this.phOrdenes.Controls.Clear();
                foreach (var a in articulos)
                {
                    this.cargarEnPh(a);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarEnPh(Gestion_Api.Entitys.Articulos_PedirOC a)
        {
            try
            {
               

                //fila
                TableRow tr = new TableRow();
                tr.ID = a.Id.ToString();

                //Celdas

                TableCell celNumero2 = new TableCell();
                celNumero2.Text = a.articulo.codigo;
                celNumero2.VerticalAlign = VerticalAlign.Middle;
                celNumero2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero2);

                TableCell celNumero = new TableCell();
                celNumero.Text = a.articulo.descripcion;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(a.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                

               

                //si estoy cargando una nota de credito

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + a.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-delete'></span>";
                btnDetalles.Font.Size = 12;
                //btnDetalles.PostBackUrl = "ImpresionCompras.aspx?a=3&oc=" + oc.Id;
                btnDetalles.Click += new EventHandler(this.eliminar);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                //CheckBox cbSeleccion = new CheckBox();
                ////cbSeleccion.Text = "&nbsp;Imputar";
                //cbSeleccion.ID = "cbSeleccion_" + a.Id;
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;
                //celAccion.Controls.Add(cbSeleccion);
                ////celAccion.Controls.Add(btnEliminar);

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phOrdenes.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando ordenes a la tabla. " + ex.Message));
            }
        }

        private void eliminar(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idArticuloPedir = atributos[1];

                var artP = this.controlador.obtenerArticuloPedirPorId(Convert.ToInt32(idArticuloPedir));
                artP.Estado = 0;

                this.controlador.modificarArticulo(artP);
                Response.Redirect("articulosPedirF.aspx");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando detalle orden desde la interfaz. " + ex.Message);
            }
        }

    }
}