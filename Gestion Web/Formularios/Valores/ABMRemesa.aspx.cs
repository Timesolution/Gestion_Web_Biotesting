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
        controladorSucursal contSucursal = new controladorSucursal();

        string sucursalDestino;
        string totalLetras;

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            sucursalDestino = Request.QueryString["sd"];

            btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

            if (!IsPostBack)
            {
                CargarDatosEnRemesa();
                calcularTotal();
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
                        //if (s == "162")
                        //{
                        //    return 1;
                        //}
                    }
                }

                return 1;
            }
            catch
            {
                return -1;
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
                txtNumeroRemesa.Text = (contRemesa.ObtenerNumeracionUltimaRemesa()+1).ToString("D8");
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
                this.txt1000Total.Text = tMil.ToString();
                //billetes 500
                int tQuin = Convert.ToInt32(this.txt500Cant.Text) * 500;
                this.txt500Total.Text = tQuin.ToString();
                //billetes 200
                int tDoscien = Convert.ToInt32(this.txt200Cant.Text) * 200;
                this.txt200Total.Text = tDoscien.ToString();
                //billetes 100
                int tCien = Convert.ToInt32(this.txt100Cant.Text) * 100;
                this.txt100Total.Text = tCien.ToString();
                //billetes 50
                int tCincuenta = Convert.ToInt32(this.txt50Cant.Text) * 50;
                this.txt50Total.Text = tCincuenta.ToString();
                //billetes 20
                int tVeinte = Convert.ToInt32(this.txt20Cant.Text) * 20;
                this.txt20Total.Text = tVeinte.ToString();
                //billetes 10
                int tDiez = Convert.ToInt32(this.txt10Cant.Text) * 10;
                this.txt10Total.Text = tDiez.ToString();
                //billetes 5
                int tCinco = Convert.ToInt32(this.txt5Cant.Text) * 5;
                this.txt5Total.Text = tCinco.ToString();
                //Cambio
                int tCincoM = Convert.ToInt32(this.txt1Cant.Text);
                this.txt1Total.Text = tCincoM.ToString();

                //total cargado
                int totalCargado = tMil + tQuin + tDoscien + tCien + tCincuenta + tVeinte + tDiez + tCinco + tCincoM;
                this.txtTotal.Text = totalCargado.ToString();
                string totalTemp = totalCargado.ToString();

                totalLetras = Numalet.ToCardinal(totalTemp.ToString());

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
                if (!Page.IsValid)
                    return;

                calcularTotal();
                int idRemesa = AgregarRemesaYDetalle();
                string script;

                if (idRemesa >= 0)
                {
                    script = "window.open('ImpresionValores.aspx?a=11&remesa=" + idRemesa + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');";
                    script += " $.msgbox(\"Remesa generada con exito! \", {type: \"info\"}); location.href = 'RemesasF.aspx'";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                }
                else
                {
                    script = " $.msgbox(\"Error generando remesa! \", {type: \"info\"});";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                }
                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al agregar remesa a la base de datos " + ex.Message);
            }
        }

        public int AgregarRemesaYDetalle()
        {
            try
            {
                var remesa = ConfigurarRemesa();

                List<Remesa_Moneda_Detalle> rmd = new List<Remesa_Moneda_Detalle>();

                ConfigurarRemesaDetalle(remesa, rmd);

                return contRemesa.AgregarYGuardarRemesa(remesa, rmd);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al agregar remesa y su detalle " + ex.Message);
                return -1;
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
                remesa.SucursalDestino = contSucursal.obtenerSucursalNombre(sucursalDestino).id;
                remesa.Observaciones = txtObservacion.Text;
                remesa.SonPesos = totalLetras;
                remesa.Estado = 1;

                return remesa;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al configurar remesa " + ex.Message);
                return new Remesa();
            }            
        }

        public void ConfigurarRemesaDetalle(Remesa remesa, List<Remesa_Moneda_Detalle> rmd)
        {
            try
            {
                Remesa_Moneda_Detalle rmd1000 = new Remesa_Moneda_Detalle();
                rmd1000.Denominacion = "$1000.00 Pesos";
                rmd1000.Cantidad = Convert.ToInt32(this.txt1000Cant.Text);
                rmd1000.Total = (int)Convert.ToInt32(this.txt1000Total.Text);
                rmd1000.Remesa1 = remesa;
                rmd1000.Valor = 1000;
                rmd.Add(rmd1000);

                Remesa_Moneda_Detalle rmd500 = new Remesa_Moneda_Detalle();
                rmd500.Denominacion = "$500.00 Pesos";
                rmd500.Cantidad = Convert.ToInt32(this.txt500Cant.Text);
                rmd500.Total = (int)Convert.ToInt32(this.txt500Total.Text);
                rmd500.Remesa1 = remesa;
                rmd500.Valor = 500;
                rmd.Add(rmd500);

                Remesa_Moneda_Detalle rmd200 = new Remesa_Moneda_Detalle();
                rmd200.Denominacion = "$200.00 Pesos";
                rmd200.Cantidad = Convert.ToInt32(this.txt200Cant.Text);
                rmd200.Total = (int)Convert.ToInt32(this.txt200Total.Text);
                rmd200.Remesa1 = remesa;
                rmd200.Valor = 200;
                rmd.Add(rmd200);

                Remesa_Moneda_Detalle rmd100 = new Remesa_Moneda_Detalle();
                rmd100.Denominacion = "$100.00 Pesos";
                rmd100.Cantidad = Convert.ToInt32(this.txt100Cant.Text);
                rmd100.Total = (int)Convert.ToInt32(this.txt100Total.Text);
                rmd100.Remesa1 = remesa;
                rmd100.Valor = 100;
                rmd.Add(rmd100);

                Remesa_Moneda_Detalle rmd50 = new Remesa_Moneda_Detalle();
                rmd50.Denominacion = "$50.00 Pesos";
                rmd50.Cantidad = Convert.ToInt32(this.txt50Cant.Text);
                rmd50.Total = (int)Convert.ToInt32(this.txt50Total.Text);
                rmd50.Remesa1 = remesa;
                rmd50.Valor = 50;
                rmd.Add(rmd50);

                Remesa_Moneda_Detalle rmd20 = new Remesa_Moneda_Detalle();
                rmd20.Denominacion = "$20.00 Pesos";
                rmd20.Cantidad = Convert.ToInt32(this.txt20Cant.Text);
                rmd20.Total = (int)Convert.ToInt32(this.txt20Total.Text);
                rmd20.Remesa1 = remesa;
                rmd20.Valor = 20;
                rmd.Add(rmd20);

                Remesa_Moneda_Detalle rmd10 = new Remesa_Moneda_Detalle();
                rmd10.Denominacion = "$10.00 Pesos";
                rmd10.Cantidad = Convert.ToInt32(this.txt10Cant.Text);
                rmd10.Total = (int)Convert.ToInt32(this.txt10Total.Text);
                rmd10.Remesa1 = remesa;
                rmd10.Valor = 10;
                rmd.Add(rmd10);

                Remesa_Moneda_Detalle rmd5 = new Remesa_Moneda_Detalle();
                rmd5.Denominacion = "$5.00 Pesos";
                rmd5.Cantidad = Convert.ToInt32(this.txt5Cant.Text);
                rmd5.Total = (int)Convert.ToInt32(this.txt5Total.Text);
                rmd5.Remesa1 = remesa;
                rmd5.Valor = 5;
                rmd.Add(rmd5);

                Remesa_Moneda_Detalle rmdCambio = new Remesa_Moneda_Detalle();
                rmdCambio.Denominacion = "$1.00 Pesos";
                rmdCambio.Cantidad = Convert.ToInt32(this.txt1Cant.Text);
                rmdCambio.Total = (int)Convert.ToInt32(this.txt1Total.Text);
                rmdCambio.Remesa1 = remesa;
                rmdCambio.Valor = 1;
                rmd.Add(rmdCambio);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al configurar remesa detalle" + ex.Message);
            }
        }
    }
}