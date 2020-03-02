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

namespace Gestion_Web.Formularios.Articulos
{
    public partial class StockPopUp : System.Web.UI.Page
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
            catch(Exception ex)
            {

            }
        }

        private void cargarStock()
        {
            try
            {
                Stock st = contArt.obtenerStockID(idStock);
                txtCodigo.Text = st.id.ToString();
                txtSucursal.Text = st.sucursal.nombre;
                txtArticulo.Text = st.articulo.descripcion;
                txtStockActual.Text = st.cantidad.ToString();
            }
            catch(Exception ex)
            {

            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtAgregarStock.Text))
                {
                    Stock st = contArt.obtenerStockID(idStock);
                    stockMovimiento s = new stockMovimiento();                    
                    txtAgregarStock.Text = txtAgregarStock.Text.Replace(',', '.');

                    //Agrego el movimiento de stock                  
                    s.IdUsuario = (int)Session["Login_IdUser"];
                    s.Cantidad = Convert.ToDecimal(this.txtAgregarStock.Text);
                    s.Articulo = st.articulo.id;
                    s.IdSucursal = st.sucursal.id;
                    s.Fecha = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
                    s.TipoMovimiento = "Inventario";
                    s.Comentarios = this.txtComentarios.Text;

                    int j = contArt.AgregarMovimientoStock(s);
                    if (j > 0)
                    {
                        int i = contArt.ActualizarStock(this.idStock, Convert.ToDecimal(txtAgregarStock.Text));
                        if (i > 0)
                        {
                            Modal.Close(this, "OK");
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al actualizar el stock del producto. "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al agregar movimiento de stock. "));
                    }
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error: " + ex.Message));
            }
        }

    }
}