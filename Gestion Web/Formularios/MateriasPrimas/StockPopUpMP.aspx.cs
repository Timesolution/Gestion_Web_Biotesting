using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Disipar.Models;
using System.Drawing;
using Gestion_Api.Entitys;
using System.Globalization;

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class StockPopUpMP : System.Web.UI.Page
    {
        private int idStock;
        private controladorArticulo contArt = new controladorArticulo();
        private Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idStock = Convert.ToInt32(Request.QueryString["idStock"]);

                btn_Agregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btn_Agregar, null) + ";");

                this.cargarStock();
                this.txtAgregarStock.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarStock()
        {
            try
            {

                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                var stockMP = controladorMateriaPrima.ObtenerStockMPrima(idStock);

                txtCodigo.Text = stockMP.Codigo;
                txtSucursal.Text = stockMP.NombreSucursal;
                txtArticulo.Text = stockMP.Descripcion;
                txtStockActual.Text = stockMP.Cant.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtAgregarStock.Text))
                {
                    controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();
                    Stock_MateriaPrimas stockMP = new Stock_MateriaPrimas();
                    //Stock st = contArt.obtenerStockID(idStock);
                    stockMP = controladorMateriaPrima.ObtenerStockMPrimaById(idStock);
                    //stockMovimiento s = new stockMovimiento();                    
                    //txtAgregarStock.Text = txtAgregarStock.Text.Replace(',', '.');

                    ////Agrego el movimiento de stock                  
                    //s.IdUsuario = (int)Session["Login_IdUser"];
                    //s.Cantidad = Convert.ToDecimal(this.txtAgregarStock.Text);
                    //s.Articulo = st.articulo.id;
                    //s.IdSucursal = st.sucursal.id;
                    //s.Fecha = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
                    //s.TipoMovimiento = "Inventario";
                    //s.Comentarios = this.txtComentarios.Text;

                    //int j = contArt.AgregarMovimientoStock(s);
                    //if (j > 0)
                    //{

                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ",";

                    decimal cantidad = decimal.Parse(txtAgregarStock.Text, nfi);

                    stockMP.Cantidad += cantidad;

                    stockMP = controladorMateriaPrima.ActualizarStock(stockMP);

                    if (stockMP != null)
                    {
                        Modal.Close(this, "OK");
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al actualizar el stock de la materia prima. "));
                    }
                }
                //else
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al agregar movimiento de stock. "));
                //}
                //}
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error: " + ex.Message));
            }
        }

    }
}