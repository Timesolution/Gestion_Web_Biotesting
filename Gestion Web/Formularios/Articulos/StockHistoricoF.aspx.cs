using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Disipar.Models;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class StockHistoricoF : System.Web.UI.Page
    {
        private int idStock;
        private controladorArticulo controlador = new controladorArticulo();
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                idStock = Convert.ToInt32(Request.QueryString["producto"]);
                this.cargarStockHistorico();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error: " + ex.Message));
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

        private void cargarStockHistorico()
        {
            try
            {
                List<StockHistorico> stocks = controlador.obtenerStockHistoricoByStock(idStock);
                foreach (StockHistorico s in stocks)
                {
                    this.cargarTable(s);
                    labelNombre.Text = " " + s.stock.articulo.codigo + "- " + s.stock.articulo.descripcion + " ";
                    labelSucursal.Text = "- Sucursal: " + s.stock.sucursal.nombre;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void cargarTable(StockHistorico s)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celSucursal = new TableCell();
                celSucursal.Text = s.stock.sucursal.nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursal);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = s.stock.articulo.descripcion;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell celFecha = new TableCell();
                celFecha.Text = s.fecha.ToString();
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celStock = new TableCell();
                celStock.Text = s.cantidad.ToString();
                celStock.VerticalAlign = VerticalAlign.Middle;
                celStock.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celStock);

                phStockHistorico.Controls.Add(tr);
            }
            catch(Exception ex)
            {

            }
        }
    }
}