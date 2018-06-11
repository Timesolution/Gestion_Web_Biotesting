using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class BusquedaPopUp : System.Web.UI.Page
    {

        //DBHandler MyDBHandler = new DBHandler();
        controladorArticulo controlador = new controladorArticulo();
        int idArticulo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = ImageButton1.UniqueID;

            if (!IsPostBack)
            {
                List<Articulo> articulos = new List<Articulo>();
                Session.Add("ArticulosBusqueda", articulos);
                //cargo lo que manda por parametro
                this.idArticulo = Convert.ToInt32(Request.QueryString["idArticulo"]);

                string buscar = Request.QueryString["buscar"];
                this.txtDescripcion.Text = buscar;
                this.buscarArticulo(buscar);
            }
            else
            {
                List<Articulo> articulos = Session["ArticulosBusqueda"] as List<Articulo>;
                this.cargarEnTable(articulos);
            }
            this.txtDescripcion.Focus();

        }

        protected void btnAgregarArticuloASP_Click(object sender, EventArgs e)
        {
            string SearchString = txtDescripcion.Text;
            this.buscarArticulo(SearchString);
        }

        private void buscarArticulo(string articulo)
        {
            try
            {
                //List<Articulo> articulos = controlador.
                //this.phArticulos.Controls.Clear();
                //this.cargarEnTable(articulos);
                //Session.Add("ArticulosBusqueda", articulos);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Error buscando articulo. " + ex.Message + "')");
            }
        }

        private void cargarEnTable(List<Articulo> articulos)
        {
            foreach (Articulo a in articulos)
            {
                this.MostrarArticulos(a);
            }
        }

        private void MostrarArticulos(Articulo a)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = a.codigo.ToString();

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = a.codigo.ToString();
                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                //TableCell celCantidad = new TableCell();
                //celCantidad.Text = a.Qty.ToString();
                //celCantidad.Width = Unit.Percentage(10);
                //celCantidad.HorizontalAlign = HorizontalAlign.Center;
                //celCantidad.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celCantidad);


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = a.descripcion;
                celDescripcion.Width = Unit.Percentage(40);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + a.precioVenta.ToString();
                celPrecio.Width = Unit.Percentage(10);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);


                //TableCell celTotal = new TableCell();
                //celTotal.Text = "$" + (a.Qty * a.precioU).ToString();
                //celTotal.Width = Unit.Percentage(10);
                //celTotal.VerticalAlign = VerticalAlign.Middle;
                //celTotal.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celTotal);
                ////arego fila a tabla

                TableCell celAccion = new TableCell();

                Button btnEliminar = new Button();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar" + a.codigo;
                btnEliminar.Text = "Seleccionar";
                btnEliminar.BackColor = Color.Black;
                btnEliminar.ForeColor = Color.White;
                btnEliminar.Click += new EventHandler(QuitarItem);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phArticulos.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Error agregando articulos. " + ex.Message + "')");
            }
        }



        private void QuitarItem(object sender, EventArgs e)
        {
            try
            {
                string nameBtn = (sender as Button).ID.ToString();
                string idProducto = nameBtn.Substring(11, nameBtn.Length - 11);
                //obtengo el pedido del session
                //cargo en el Session
                Session.Add("Codigo", idProducto);
                //Modal.Close(this, "OK");

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Error quitando articulo al pedido. " + ex.Message + "')");

            }
        }
    }
}