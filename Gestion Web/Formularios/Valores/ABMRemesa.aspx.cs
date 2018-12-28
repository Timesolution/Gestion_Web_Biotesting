using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMRemesa : System.Web.UI.Page
    {
        ControladorRemesaEntity contRemesa = new ControladorRemesaEntity();
        ControladorCaja contCaja = new ControladorCaja();
        int idMovCaja;
                
        protected void Page_Load(object sender, EventArgs e)
        {
            idMovCaja = Convert.ToInt32(Request.QueryString["movCaja"]);

            if (!IsPostBack)
            {
                CargarDatosEnRemesa();
                calcularTotal();
                //CargarArticulosDropDownList();
                //if (accion == 1)
                //    
                //else if (accion == 2)
                //    ModificarOrdenReparacion();
            }
        }

        public void CargarDatosEnRemesa()
        {
            try
            {
                if (String.IsNullOrEmpty(txtFecha.Text))
                {
                    txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                txtNumeroRemesa.CssClass = "form-control";
                txtNumeroRemesa.Text = contRemesa.ObtenerNumeracionUltimaRemesa()+1.ToString("D8");
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar datos en remesa " + ex.Message);
            }
        }

        private void calcularTotal()
        {
            try
            {
                //billetes 1000
                int tMil = Convert.ToInt32(this.txt1000Cant.Text) * 1000;
                this.txt1000Total.Text = tMil.ToString("N");
                //billetes 500
                int tQuin = Convert.ToInt32(this.txt500Cant.Text) * 500;
                this.txt500Total.Text = tQuin.ToString("N");
                //billetes 200
                int tDoscien = Convert.ToInt32(this.txt200Cant.Text) * 200;
                this.txt200Total.Text = tDoscien.ToString("N");
                //billetes 100
                int tCien = Convert.ToInt32(this.txt100Cant.Text) * 100;
                this.txt100Total.Text = tCien.ToString("N");
                //billetes 50
                int tCincuenta = Convert.ToInt32(this.txt50Cant.Text) * 50;
                this.txt50Total.Text = tCincuenta.ToString("N");
                //billetes 20
                int tVeinte = Convert.ToInt32(this.txt20Cant.Text) * 20;
                this.txt20Total.Text = tVeinte.ToString("N");
                //billetes 10
                int tDiez = Convert.ToInt32(this.txt10Cant.Text) * 10;
                this.txt10Total.Text = tDiez.ToString("N");
                //billetes 5
                int tCinco = Convert.ToInt32(this.txt5Cant.Text) * 5;
                this.txt5Total.Text = tCinco.ToString("N");
                //Cambio
                int tCincoM = Convert.ToInt32(this.txt1Cant.Text);
                this.txt1Total.Text = tCincoM.ToString("N");

                //total cargado
                int totalCargado = tMil + tQuin + tDoscien + tCien + tCincuenta + tVeinte + tDiez + tCinco + tCincoM;
                this.txtTotal.Text = totalCargado.ToString("N");

                string totalS = Numalet.ToCardinal(totalCargado.ToString().Replace(',', '.'));

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al calcular totales " + ex.Message);
            }
        }

        protected void txt1000Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt500Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt200Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt100Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt50Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt20Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt10Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt5Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void txt1Cant_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarRemesaYDetalle();                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al agregar remesa a la base de datos " + ex.Message);
            }
        }

        public void AgregarRemesaYDetalle()
        {
            try
            {
                var remesa = ConfigurarRemesa();

                ConfigurarRemesaDetalle(remesa);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al agregar remesa y su detalle " + ex.Message);
            }
        }

        public Remesa ConfigurarRemesa()
        {
            try
            {
                Remesa remesa = new Remesa();

                remesa.NumeroRemesa = contRemesa.ObtenerNumeracionUltimaRemesa() + 1;
                remesa.Fecha = Convert.ToDateTime(txtFecha.Text, new CultureInfo("es-AR"));
                remesa.Entrega = txtEntrega.Text;
                remesa.Recibe = txtRecibe.Text;
                remesa.SucursalOrigen = (int)Session["Login_SucUser"];
                remesa.SucursalDestino = contCaja.obtenerCajaID(idMovCaja).suc.id;
                remesa.Observaciones = txtObservacion.Text;
                remesa.SonPesos = Numalet.ToCardinal(txtTotal.Text.Replace(',', '.'));
                remesa.Estado = 1;

                return remesa;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al configurar remesa " + ex.Message);
                return null;
            }            
        }

        public void ConfigurarRemesaDetalle(Remesa remesa)
        {
            try
            {
                Remesa_Moneda_Detalle rmd1000 = new Remesa_Moneda_Detalle();
                rmd1000.Denominacion = "$1000.00 Pesos";
                rmd1000.Cantidad = Convert.ToInt32(this.txt1000Cant.Text);
                rmd1000.Total = (int)Convert.ToInt32(this.txt1000Total.Text);
                rmd1000.Remesa1 = remesa;
                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al configurar remesa detalle" + ex.Message);
            }
        }
    }
}