using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Informes
{
    public partial class RentabilidadF : System.Web.UI.Page
    {
        controladorInformes cont = new controladorInformes();
        controladorArticulo contArticulo = new controladorArticulo();
        int accion;
        int sucursal;
        string desde;
        string hasta;
        string producto;
        string nroFactura;
        int tipoBusqueda;
        int sinIva;

        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.desde = Request.QueryString["d"];
                this.hasta = Request.QueryString["h"];
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.producto = Request.QueryString["p"];
                this.nroFactura = Request.QueryString["nro"];
                this.tipoBusqueda = Convert.ToInt32(Request.QueryString["b"]);
                this.sinIva = Convert.ToInt32(Request.QueryString["iva"]);

                if (!IsPostBack)
                {
                    if (this.sucursal == 0)
                    {
                        sucursal = (int)Session["Login_SucUser"];
                    }
                    this.cargarSucursal();
                    //this.cargarTiposPago();
                    this.txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    
                    this.DropListSucursal.SelectedValue = sucursal.ToString();
                }
                //cargo el informe
                if (this.accion == 2 || this.accion == 3)
                {
                    this.generarInformeCosto();
                }      
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando formulario de rentabilidad. " + ex.Message);
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
                else
                {
                    if (this.verificarAcceso() != 1)
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Herramientas.Presupuesto") != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "66")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        private void generarInformeCosto()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.desde, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.hasta, new CultureInfo("es-AR"));
                hasta = hasta.AddHours(23).AddMinutes(59).AddSeconds(59);//23:59:59 hs
                int sucursal = Convert.ToInt32(this.sucursal);
                string producto = this.producto;
                string numeroFactura = this.nroFactura;
                if (!String.IsNullOrEmpty(numeroFactura))
                {
                    numeroFactura = this.nroFactura.PadLeft(8, '0');
                }

                this.cargarRentabilidadCosto(desde, hasta, sucursal, producto, numeroFactura);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando obteniendo datos para generar informe de rentabilidad. " + ex.Message);
            }
        }
        private void cargarRentabilidadCosto(DateTime Desde, DateTime Hasta, int sucursal, string producto,string numeroFactura)
        {
            try
            {
                this.GridInforme.AutoGenerateColumns = false;
                DataTable datos = null;
                int tipo = this.tipoBusqueda;

                datos = this.cont.obtenerRentabilidadCosto(Desde, Hasta, sucursal, producto, numeroFactura, tipo, this.sinIva);
                
                this.GridInforme.DataSource = datos;               

                decimal costoT = 0;
                decimal precioT = 0;

                foreach (DataRow dr in datos.Rows)
                {
                    if (Convert.ToDecimal(dr["Costo"]) <= 0)
                    {
                        costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(0.01);
                    }
                    else
                    {
                        costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo"]);
                    }
                    //costoT += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo"]);
                    precioT += Convert.ToDecimal(dr["Cantidad"]) * Decimal.Round(Convert.ToDecimal(dr["Precio Unitario"]),4);
                }

                //ganancia
                decimal ganancia = decimal.Round(precioT - costoT,4);
                //porcentaje
                decimal porGanancia = decimal.Round((((precioT / costoT) - 1) * 100),4);

                this.labelTotalVendido.Text = "$ " + decimal.Round(precioT,4).ToString("N");
                this.labelTotalCosto.Text = "$ " + decimal.Round(costoT,4).ToString("N");

                this.labelRentabilidad.Text = "$ " + decimal.Round(ganancia,4).ToString("N");
                this.labelPorRentabilidad.Text = decimal.Round(porGanancia, 4).ToString("N") + "%";

                
                //GridView2.DataSource = dtVictimario;
                this.GridInforme.DataBind();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando informe de rentabilidad. " + ex.Message);
            }
        }
        
        protected void GridInforme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridInforme.PageIndex = e.NewPageIndex;
            this.GridInforme.DataBind();
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string articulo = this.txtBusqueda.Text.Replace("+","%2B");
                Response.Redirect("RentabilidadF.aspx?a=2&d=" + this.txtFechaDesde.Text + "&h=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&p=" + articulo + "&nro=" + this.txtNroFactura.Text + "&b=" + Convert.ToInt32(this.chkTipoBusqueda.Checked) + "&iva=" + this.ListConIva.SelectedValue);
            }
            catch
            {
 
            }
        }

        protected void lbBuscarProducto_Click(object sender, EventArgs e)
        {
            string idSuc = Session["Login_SucUser"].ToString();
            Response.Redirect("RentabilidadFCosto.aspx?a=3&s=" + idSuc + "&p=" + txtBusqueda.Text);
        }

        //
        protected void btnRentPorSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionRentabilidadD.aspx?a=1&ex=1&fd=" + this.desde + "&fh=" + this.hasta + "&ar=" + this.producto);
            }
            catch
            {

            }
        }

        protected void lbtnRemitir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionRentabilidadD.aspx?a=2&ex=1&fd=" + this.desde + "&suc=" + this.sucursal + "&fh=" + this.hasta + "&num=" + this.nroFactura + "&ar=" + this.producto + "&t=" + this.tipoBusqueda + "&iva=" + this.sinIva);
            }
            catch
            {

            }
        }

    }
}