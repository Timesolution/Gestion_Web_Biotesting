using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CajaCierreABM : System.Web.UI.Page
    {
        controladorCajaEntity contCaja = new controladorCajaEntity();
        Mensajes m = new Mensajes();
        int sucursal;
        int puntoVenta;
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnConfirmarCierre.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde'; " + ClientScript.GetPostBackEventReference(btnConfirmarCierre, null) + ";");
                btnMovDiferencia.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde'; " + ClientScript.GetPostBackEventReference(btnMovDiferencia, null) + ";");
                lblCierre.Attributes.Add("Text", "this.value='<i class=\"fa fa-spinner fa-spin\"></i>'");

                sucursal = Convert.ToInt32(Request.QueryString["s"]);
                puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                accion = Convert.ToInt32(Request.QueryString["a"]);

                this.VerificarLogin();

                if (!IsPostBack)
                {
                    this.cargarDatosIniciales();
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaApertura.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    this.calcularTotal();
                    this.obtenerFechaUltimoCierre();
                    this.verificarAccesoCajaEspecial();

                    this.cargarListTurnos();
                    this.cargarResumenCierreTurno();
                }

                this.cargarFacturasRango();
                //this.cargarCreditosAValidar();
                this.formaCierreCaja();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Valores.Caja") != 1)
                    if (this.verificarAcceso() != 1)
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
                        if (s == "45")
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

        private void formaCierreCaja()
        {
            try
            {
                //tapice
                string version = WebConfigurationManager.AppSettings.Get("VersionCierreCaja");
                if (version == "1")
                {
                    this.phDiferencia.Visible = false;
                    this.phDiferenciaModal.Visible = true;
                }
            }
            catch
            { }
        }
        public void verificarAccesoCajaEspecial()
        {
            try
            {
                int idUser = (int)Session["Login_IdUser"];
                int i = this.contCaja.verificarAccesoCajaEspecial(this.puntoVenta, idUser);
                if (i < 0)
                {
                    Response.Redirect("/Default.aspx?m=1", false);
                }
            }
            catch
            {

            }
        }
        private void cargarDatosIniciales()
        {
            try
            {
                //sucursal y PV
                controladorSucursal contSucu = new controladorSucursal();
                var s = contSucu.obtenerSucursalID(this.sucursal);
                var pv = contSucu.obtenerPtoVentaId(this.puntoVenta);

                this.txtSucursal.Text = s.nombre;
                this.txtPuntoVenta.Text = pv.nombre_fantasia;
                //total en caja
                ControladorCaja controlCaja = new ControladorCaja();
                this.txtTotalCaja.Text = controlCaja.obtenerTotalCaja(this.sucursal, this.puntoVenta).ToString("N");
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                bool mostrar = false;
                foreach (string sl in listPermisos)
                {
                    if (sl == "111")
                    {
                        mostrar = true;
                    }
                }
                if (mostrar)
                {
                    this.lblSaldo.Text = "$" + this.txtTotalCaja.Text;
                }
                else
                {
                    this.lblSaldo.Text = "$" + "****.**";
                }
                string ultimoNro = this.contCaja.obtenerUltimoNumeroCierre(this.sucursal, this.puntoVenta);
                this.txtNumeroCierre.Text = (Convert.ToInt32(ultimoNro) + 1).ToString().PadLeft(8, '0'); 
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en carga inicial de pagina " + ex.Message));
            }
        }
        private void obtenerFechaUltimoCierre()
        {
            try
            {
                Gestion_Api.Entitys.Caja_Cierre ultimo = this.contCaja.obtenerUltimoCierrePV(this.sucursal, this.puntoVenta);

                if (ultimo != null)
                {
                    this.txtFecha.Text = ultimo.FechaApertura.Value.ToString("dd/MM/yyyy");
                }

            }
            catch
            {

            }
        }
        
        #region funciones cierre
        private void hacerCierre()
        {
            try
            {
                Caja_Cierre cc = new Caja_Cierre();
                cc.Sucursal = this.sucursal;
                cc.PuntoVenta = this.puntoVenta;
                cc.FechaApertura = Convert.ToDateTime(this.txtFechaApertura.Text, new CultureInfo("es-AR")).AddHours(7);
                cc.FechaCierre = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                cc.Total = Convert.ToDecimal(this.txtTotal.Text);
                cc.Numero = this.txtNumeroCierre.Text;

                Caja_Cierre_Detalle ccd1000 = new Caja_Cierre_Detalle();
                ccd1000.Denominacion = "Billetes 1000 Pesos";
                ccd1000.Cantidad = Convert.ToInt32(this.txt1000Cant.Text);
                ccd1000.Importe = Convert.ToDecimal(this.txt1000Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd1000);

                Caja_Cierre_Detalle ccd500 = new Caja_Cierre_Detalle();
                ccd500.Denominacion = "Billetes 500 Pesos";
                ccd500.Cantidad = Convert.ToInt32(this.txt500Cant.Text);
                ccd500.Importe = Convert.ToDecimal(this.txt500Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd500);

                Caja_Cierre_Detalle ccd200 = new Caja_Cierre_Detalle();
                ccd200.Denominacion = "Billetes 200 Pesos";
                ccd200.Cantidad = Convert.ToInt32(this.txt200Cant.Text);
                ccd200.Importe = Convert.ToDecimal(this.txt200Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd200);

                Caja_Cierre_Detalle ccd100 = new Caja_Cierre_Detalle();
                ccd100.Denominacion = "Billetes 100 Pesos";
                ccd100.Cantidad = Convert.ToInt32(this.txt100Cant.Text);
                ccd100.Importe = Convert.ToDecimal(this.txt100Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd100);

                Caja_Cierre_Detalle ccd50 = new Caja_Cierre_Detalle();
                ccd50.Denominacion = "Billetes 50 Pesos";
                ccd50.Cantidad = Convert.ToInt32(this.txt50Cant.Text);
                ccd50.Importe = Convert.ToDecimal(this.txt50Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd50);

                Caja_Cierre_Detalle ccd20 = new Caja_Cierre_Detalle();
                ccd20.Denominacion = "Billetes 20 Pesos";
                ccd20.Cantidad = Convert.ToInt32(this.txt20Cant.Text);
                ccd20.Importe = Convert.ToDecimal(this.txt20Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd20);
                
                Caja_Cierre_Detalle ccd10 = new Caja_Cierre_Detalle();
                ccd10.Denominacion = "Billetes 10 Pesos";
                ccd10.Cantidad = Convert.ToInt32(this.txt10Cant.Text);
                ccd10.Importe = Convert.ToDecimal(this.txt10Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd10);

                Caja_Cierre_Detalle ccd5 = new Caja_Cierre_Detalle();
                ccd5.Denominacion = "Billetes 5 Pesos";
                ccd5.Cantidad = Convert.ToInt32(this.txt5Cant.Text);
                ccd5.Importe = Convert.ToDecimal(this.txt5Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd5);

                Caja_Cierre_Detalle ccd2 = new Caja_Cierre_Detalle();
                ccd2.Denominacion = "Billetes 2 Pesos";
                ccd2.Cantidad = Convert.ToInt32(this.txt2Cant.Text);
                ccd2.Importe = Convert.ToDecimal(this.txt2Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd2);

                Caja_Cierre_Detalle ccd2M = new Caja_Cierre_Detalle();
                ccd2M.Denominacion = "Monedas 2 Pesos";
                ccd2M.Cantidad = Convert.ToInt32(this.txt2MCant.Text);
                ccd2M.Importe = Convert.ToDecimal(this.txt2MTotal.Text);
                cc.Caja_Cierre_Detalle.Add(ccd2M);
                
                Caja_Cierre_Detalle ccd1M = new Caja_Cierre_Detalle();
                ccd1M.Denominacion = "Monedas 1 Pesos";
                ccd1M.Cantidad = Convert.ToInt32(this.txt1Cant.Text);
                ccd1M.Importe = Convert.ToDecimal(this.txt1Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd1M);

                Caja_Cierre_Detalle ccd05M = new Caja_Cierre_Detalle();
                ccd05M.Denominacion = "Monedas 50 Centavos";
                ccd05M.Cantidad = Convert.ToInt32(this.txt05Cant.Text);
                ccd05M.Importe = Convert.ToDecimal(this.txt05Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd05M);

                Caja_Cierre_Detalle ccd025M = new Caja_Cierre_Detalle();
                ccd025M.Denominacion = "Monedas 25 Centavos";
                ccd025M.Cantidad = Convert.ToInt32(this.txt025Cant.Text);
                ccd025M.Importe = Convert.ToDecimal(this.txt025Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd025M);

                Caja_Cierre_Detalle ccd010M = new Caja_Cierre_Detalle();
                ccd010M.Denominacion = "Monedas 10 Centavos";
                ccd010M.Cantidad = Convert.ToInt32(this.txt010Cant.Text);
                ccd010M.Importe = Convert.ToDecimal(this.txt010Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd010M);

                Caja_Cierre_Detalle ccd005M = new Caja_Cierre_Detalle();
                ccd005M.Denominacion = "Cambio";
                ccd005M.Cantidad = Convert.ToInt32(this.txt005Cant.Text);
                ccd005M.Importe = Convert.ToDecimal(this.txt005Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd005M);

                Caja_Cierre_Detalle ccd001M = new Caja_Cierre_Detalle();
                ccd001M.Denominacion = "Retiro";
                ccd001M.Cantidad = Convert.ToInt32(this.txt001Cant.Text);
                ccd001M.Importe = Convert.ToDecimal(this.txt001Total.Text);
                cc.Caja_Cierre_Detalle.Add(ccd001M);

                int i = this.contCaja.agregarCierre(cc);

                if (i > 0)
                {
                    Response.Redirect("CajaF.aspx");
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre Realizado con exito", null));
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo realizar cierre"));
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo realizar cierre. \");", true);                                                
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error haciendo cierre " + ex.Message));                
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error haciendo cierre. " + ex.Message + ". \", {type: \"error\"});", true);    
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                string fechaD = this.txtFecha.Text;
                string fechaH = DateTime.Now.ToString("dd/MM/yyyy");                
                List<Factura> Facturas = contFact.obtenerFacturasEntreSucursal(fechaD, fechaH, 0, this.sucursal);

                if (Facturas != null)
                {
                    int ok = this.contCaja.verificarValidarMercaderiaCaja(this.sucursal, Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR")));
                    if (ok > 0 && Facturas.Count > 0 || Facturas.Count == 0)
                    {
                        //agrego diferencia
                        this.agregarDiferencia();
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "alert", "abrirdialog()", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "alert", "$.msgbox(\"Debe aceptar la mercaderia recibida primero!. \");", true);
                    }
                }
                else
                {
                    //agrego diferencia
                    this.agregarDiferencia();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "alert", "abrirdialog()", true);
                }                
                
            }
            catch
            {

            }
            
        }

        private void agregarDiferencia()
        {
            try
            {
                decimal diferencia = Convert.ToDecimal(this.txtDiferenciaModal.Text);
                if (diferencia > 0)
                {
                    Caja_Diferencias cf = new Caja_Diferencias();
                    cf.Sucursal = this.sucursal;
                    cf.PuntoVenta = this.puntoVenta;
                    cf.Fecha = DateTime.Now;
                    cf.Diferencia = diferencia;
                    cf.Total = Convert.ToDecimal(this.txtCajaConfirmacion.Text);
                    this.contCaja.agregarDiferencia(cf);
                }
            }
            catch(Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error agregando diferencia de caja desde web. " + ex.Message);
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Datos: Sucursal: " + this.sucursal + 
                    " Punto venta: " + this.puntoVenta +
                    " Fecha: " +  DateTime.Now.ToString("dd/MM/yyyy") + 
                    " Total: " + this.txtCajaConfirmacion.Text +
                    " Diferencia: " + this.txtDiferenciaModal.Text);
            }
        }

        protected void btnConfirmarCierre_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.accion == 1)
                {
                    decimal diferencia = Convert.ToDecimal(this.txtDiferencia.Text);
                    if (diferencia == 0)
                    {
                        this.hacerCierre();
                    }
                    else
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede hacer cierre con diferencia de caja"));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se puede hacer cierre con diferencia de caja. \");", true);
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando cierre. " + ex.Message));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error guardando cierre. " + ex.Message + ". \", {type: \"error\"});", true);

            }
        }
        private void calcularTotal()
        {
            try
            {
                //billetes 1000
                decimal tMil = Convert.ToInt32(this.txt1000Cant.Text) * 1000;
                this.txt1000Total.Text = tMil.ToString("N");
                //billetes 500
                decimal tQuin = Convert.ToInt32(this.txt500Cant.Text) * 500;
                this.txt500Total.Text = tQuin.ToString("N");
                //billetes 200
                decimal tDoscien = Convert.ToInt32(this.txt200Cant.Text) * 200;
                this.txt200Total.Text = tDoscien.ToString("N");
                //billetes 100
                decimal tCien = Convert.ToInt32(this.txt100Cant.Text) * 100;
                this.txt100Total.Text = tCien.ToString("N");
                //billetes 50
                decimal tCincuenta = Convert.ToInt32(this.txt50Cant.Text) * 50;
                this.txt50Total.Text = tCincuenta.ToString("N");
                //billetes 20
                decimal tVeinte = Convert.ToInt32(this.txt20Cant.Text) * 20;
                this.txt20Total.Text = tVeinte.ToString("N");
                //billetes 10
                decimal tDiez = Convert.ToInt32(this.txt10Cant.Text) * 10;
                this.txt10Total.Text = tDiez.ToString("N");
                //billetes 5
                decimal tCinco = Convert.ToInt32(this.txt5Cant.Text) * 5;
                this.txt5Total.Text = tCinco.ToString("N");
                //billetes 2
                decimal tDos = Convert.ToInt32(this.txt2Cant.Text) * 2;
                this.txt2Total.Text = tDos.ToString("N");
                //Monedas 2
                decimal tDosM = Convert.ToInt32(this.txt2MCant.Text) * 2;
                this.txt2MTotal.Text = tDosM.ToString("N");
                //Monedas 1
                decimal tUnoM = Convert.ToInt32(this.txt1Cant.Text) * 1;
                this.txt1Total.Text = tUnoM.ToString("N");
                //Monedas 50
                decimal tCincuentaM = Convert.ToInt32(this.txt05Cant.Text) * (decimal)0.5;
                this.txt05Total.Text = tCincuentaM.ToString("N");
                //Monedas 25
                decimal tVeinticincoM = Convert.ToInt32(this.txt025Cant.Text) * (decimal)0.25;
                this.txt025Total.Text = tVeinticincoM.ToString("N");
                //Monedas 10
                decimal tDiezM = Convert.ToInt32(this.txt010Cant.Text) * (decimal)0.1;
                this.txt010Total.Text = tDiezM.ToString("N");
                //Cambio
                decimal tCincoM = Convert.ToInt32(this.txt005Cant.Text) * (decimal)1;
                this.txt005Total.Text = tCincoM.ToString("N");
                //Retiro
                decimal tUnM = Convert.ToInt32(this.txt001Cant.Text) * (decimal)1;
                this.txt001Total.Text = tUnM.ToString("N");

                //total cargado
                decimal totalCargado = tMil + tQuin + tDoscien + tCien + tCincuenta + tVeinte + tDiez + tCinco + tDos +
                    tDosM + tUnoM + tCincuentaM + tVeinticincoM + tDiezM + tCincoM + tUnM;
                this.txtTotal.Text = totalCargado.ToString("N");

                //total caja
                decimal totalCaja = Convert.ToDecimal(this.txtTotalCaja.Text );

                //diferencia
                decimal diferencia = totalCaja - totalCargado;
                this.txtDiferencia.Text = diferencia.ToString("N");
                this.txtDiferenciaModal .Text = diferencia.ToString("N");

                //datos modal confirmacion
                this.actualizarDatosConfirmacion();

            }
            catch (Exception ex)
            {
 
            }
        }

        private void actualizarDatosConfirmacion()
        {
            try
            {
                this.txtFechaAperturaConfirmacion.Text = this.txtFechaApertura.Text;
                this.txtFechaCierreConfirmacion.Text = this.txtFecha.Text;
                this.txtCajaConfirmacion.Text = this.txtTotalCaja.Text;
                
                DateTime fecha =  Convert.ToDateTime(this.txtFechaAperturaConfirmacion.Text,new CultureInfo("es-AR"));
                var culture = new System.Globalization.CultureInfo("es-AR");
                string dia = culture.DateTimeFormat.GetDayName(fecha.DayOfWeek);
                string mes = culture.DateTimeFormat.GetMonthName(fecha.Month);
                
                this.lblAlerta.Text = "La caja quedara cerrada para facturar hasta el dia " + dia + " " + fecha.Day + " de " + mes + " de " + fecha.Year;

            }
            catch
            {

            }
        }
        
        private void agregarDifCaja(decimal dif)
        {
            try
            {
                ControladorCaja contrCaja = new ControladorCaja();
                Caja caja = new Caja();
                //agrego el valor de movimiento caja, 1 para dif de caja
                caja.mov.id = 1;
                //forma pago
                caja.tipo.id = 1;
                //le cambio el signo a la diferencia, si me falta va a la caja restando como plata que no tengo
                //Si me sobra va a la caja sumando, plata que tengo de mas
                caja.importe = dif * -1;
                
                //5 y agrega el id del  movimiento en ID cobro de la tabla que refiere a dif caja
                caja.tipoMovimiento = 5;
                caja.fecha = DateTime.Now;
                caja.suc.id = this.sucursal;
                caja.pv.id = this.puntoVenta;
                int i = contrCaja.agregar(caja, 0);
                if (i > 0)
                {
                    //recalculo total en caja
                    this.txtTotalCaja.Text = contrCaja.obtenerTotalCaja(this.sucursal, this.puntoVenta).ToString("N");

                    //recalculo todo de vuelta con el nuevo total
                    this.calcularTotal();

                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeBoxInfo("Diferencia generada", null), true);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Diferencia generada\", {type: \"info\"});", true);
                    this.btnMovDiferencia.Enabled = false;

                }
            }
            catch (Exception ex)
            {
 
            }
        }

        #endregion

        #region Acciones textbox

        

        protected void txt1000Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt500Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }
        protected void txt200Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }
        protected void txt100Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt50Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt20Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt10Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt5Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt2Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt2MCant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt1Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt05Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt025Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt010Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt005Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }

        protected void txt001Cant_TextChanged(object sender, EventArgs e)
        {
            this.calcularTotal();
        }
        #endregion

        protected void btnMovDiferencia_Click(object sender, EventArgs e)
        {
            try
            {
                Configuracion c = new Configuracion();
                decimal dif = Convert.ToDecimal(this.txtDiferencia.Text);
                decimal limiteDif = Convert.ToDecimal(c.limiteDifCaja);

                if (limiteDif > 0)
                {
                    if (dif <= limiteDif)
                    {
                        this.agregarDifCaja(dif);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se puede generar un diferencia mayor a $" + limiteDif + ". \");", true);
                    }
                }
                else
                {
                    this.agregarDifCaja(dif);
                }                
                
            }
            catch
            {
 
            }
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargarDatosIniciales();
                this.calcularTotal();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina " + ex.Message));
            }
            
        }

        protected void txtFechaApertura_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Configuracion c = new Configuracion();

                //DateTime cierre = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                //DateTime abre = Convert.ToDateTime(this.txtFechaApertura.Text, new CultureInfo("es-AR"));
                //int diasMax = Convert.ToInt32(c.diasMaxApertura);
                //TimeSpan dif = abre - cierre;

                //if (dif.Days > diasMax)
                //{
                //    this.txtFechaApertura.Text = cierre.AddDays(1).ToString("dd/MM/yyyy");
                //    this.txtFechaApertura.Focus();
                //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se puede hacer un cierre de caja con apertura a mas de " + diasMax + " dias. \");", true);
                //}


            }
            catch
            {

            }
        }

        #region mercaderia entre sucursales
        protected void lbtnAceptarMercaderia_Click(object sender, EventArgs e)
        {
            try
            {
                Caja_Cierre_Mercaderia cierre = new Caja_Cierre_Mercaderia();
                cierre.Estado = 1;
                cierre.Fecha = DateTime.Now;
                cierre.Sucursal = this.sucursal;
                cierre.PtoVenta = this.puntoVenta;

                int i = this.contCaja.agregarValidacionMercaderiaCaja(cierre);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Aceptado con exito. \", {type: \"info\"});", true);
                }
            }
            catch
            {

            }
        }
        private void cargarFacturasRango()
        {
            try
            {
                string fechaD = this.txtFecha.Text;
                string fechaH = DateTime.Now.ToString("dd/MM/yyyy");

                controladorFacturacion contFact = new controladorFacturacion();
                List<Factura> Facturas = contFact.obtenerFacturasEntreSucursal(fechaD, fechaH, 0, this.sucursal);
                
                foreach (Factura f in Facturas)
                {
                    this.cargarEnPh(f);
                }               

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando facturas.  " + ex.Message));
            }
        }
        private void cargarEnPh(Factura f)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();

                //fila
                TableRow tr = new TableRow();
                tr.ID = f.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = f.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                celTipo.Text = f.tipo.tipo;
                celTipo.HorizontalAlign = HorizontalAlign.Center;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celNumero = new TableCell();
                celNumero.Text = f.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celSucOrigen = new TableCell();
                celSucOrigen.Text = f.sucursal.nombre;
                celSucOrigen.VerticalAlign = VerticalAlign.Middle;
                celSucOrigen.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucOrigen);

                TableCell celSucDestino = new TableCell();
                celSucDestino.Text = contSucu.obtenerSucursalID(f.sucursalFacturada).nombre;
                celSucDestino.VerticalAlign = VerticalAlign.Middle;
                celSucDestino.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucDestino);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + f.total;
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + f.id + "_" + f.tipo.id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }

        }
        private void detalleFactura(object sender, EventArgs e)
        {
            try
            {
                controladorDocumentos contDocumentos = new controladorDocumentos();
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idFactura = atributos[1];
                int tipo = Convert.ToInt32(atributos[2]);
                TipoDocumento tp = contDocumentos.obtenerTipoDoc("Presupuesto");

                if (tipo == tp.id || tipo == 11 || tipo == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('../../Formularios/Facturas/ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    if (tipo == 1 || tipo == 9 || tipo == 4 || tipo == 24 || tipo == 25 || tipo == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('../../Formularios/Facturas/ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        //facturab
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('../../Formularios/Facturas/ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }

                }




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }        

        #endregion

        #region cierre turno resumen

        private void cargarListTurnos()
        {
            try
            {
                List<Turnos_Caja> turnos = this.contCaja.obtenerTurnos();

                this.ListTurnos.DataSource = turnos;
                this.ListTurnos.DataValueField = "Id";
                this.ListTurnos.DataTextField = "Turno";
                this.ListTurnos.DataBind();

                this.ListTurnos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarResumenCierreTurno()
        {
            try
            {
                controladorCobranza contCobranza = new controladorCobranza();
                ControladorCaja controladorCaja = new ControladorCaja();

                List<Turnos_Caja> turnos = this.contCaja.obtenerTurnos();

                if (turnos != null && turnos.Count > 0)
                {

                    List<CajaEnt> list = this.contCaja.obtenerListadoCajaUltimoTurno(this.sucursal, this.puntoVenta);
                    List<Caja> cajas = new List<Caja>();
                    decimal total = 0;
                    decimal totalEfectivo = 0;
                    decimal totalTarjeta = 0;
                    decimal totalTraspasos = 0;
                    if (list != null)
                    {
                        foreach (CajaEnt caja in list)
                        {
                            Caja c = new Caja();
                            c.id = caja.id;
                            c.importe = caja.importe.Value;
                            c.tipo = controladorCaja.obtenerTipoPagoID(caja.tipoPago.Value);
                            c.fecha = caja.fecha.Value;
                            c.suc.id = caja.sucursal.Value;
                            c.tipoMovimiento = caja.tipoMovimiento.Value;
                            try
                            {
                                c.pv.id = caja.IdPuntoVenta.Value;
                            }
                            catch { }
                            //cobro
                            if (c.tipoMovimiento == 1)
                            {
                                c.cobro = contCobranza.obtenerCobroID(caja.idCobro.Value);
                            }
                            //gasto o dif de caja
                            if (c.tipoMovimiento == 2 || c.tipoMovimiento == 5)
                            {
                                c.mov = controladorCaja.obtenerMovimientoCajaID(caja.idCobro.Value);
                            }
                            //pago compra
                            if (c.tipoMovimiento == 3)
                            {
                                controladorPagos cp = new controladorPagos();
                                c.pCompra = cp.obtenerPagoById(Convert.ToInt64(caja.idCobro.Value));
                            }
                            if (c.tipoMovimiento == 9)
                            {
                                c.cobro.id = caja.idCobro.Value;
                                //agregar cierre de caja a la caja y cargarlo aca
                            }
                            //cierre de caja 
                            if (c.tipoMovimiento == 4)
                            {
                                c.cobro.id = Convert.ToInt32(caja.idCobro.Value);//id Cierre
                            }
                            //comentario
                            try
                            {
                                c.comentario = controladorCaja.obtenerCajaComentario(caja.id);
                            }
                            catch { }

                            this.cargarEnPh(c);
                            total += c.importe;
                        }
                        totalEfectivo = list.Where(x => x.tipoPago == 1).Sum(x => x.importe).Value;
                        totalTarjeta = list.Where(x => x.tipoPago == 5).Sum(x => x.importe).Value;
                        totalTraspasos = list.Where(x => x.tipoMovimiento == 6).Sum(x => x.importe).Value;
                    }
                    this.txtTotalCierreTurno.Text = total.ToString();
                    this.txtTotalEfectivo.Text = totalEfectivo.ToString();
                    this.txtTotalTarjeta.Text = totalTarjeta.ToString();
                    this.txtTotalTraspasos.Text = totalTraspasos.ToString();
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void cargarEnPh(Caja c)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = c.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                if (c.tipoMovimiento == 1)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = c.cobro.tipoDocumento.tipo + " " + c.cobro.numero;
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }

                if (c.tipoMovimiento == 2 || c.tipoMovimiento == 5)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = c.mov.descripcion;
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 3)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Recibo de Pago - " + c.pCompra.Numero;
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 4)
                {
                    Gestion_Api.Entitys.Caja_Cierre cierre = this.contCaja.obtenerCierreByID(c.cobro.id);
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Cierre de Caja Nro " + cierre.Numero;
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 6)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Traspaso de Caja";
                    if (c.comentario != "" && c.comentario != null)
                    {
                        celRecibo.Text += " - " + c.comentario.Split('|')[0];
                    }
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 7)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Apertura Caja";
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 8)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Traspaso Efectivo Tarjeta";
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }
                if (c.tipoMovimiento == 9)
                {
                    controladorRemuneraciones contRemuneracion = new controladorRemuneraciones();
                    TableCell celRecibo = new TableCell();
                    Gestion_Api.Entitys.PagoRemuneracione pago = contRemuneracion.obtenerPagoRemuneracionByID(c.cobro.id);
                    celRecibo.Text = "Pago Remuneracion Nº " + pago.Numero;
                    celRecibo.VerticalAlign = VerticalAlign.Middle;
                    celRecibo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celRecibo);
                }

                TableCell celImporte = new TableCell();
                celImporte.Text = c.importe.ToString();
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celTipo = new TableCell();
                celTipo.Text = c.tipo.descripcion;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                //celda comentarios para exportar
                TableCell celComentario = new TableCell();
                celComentario.Visible = false;
                celComentario.Text = c.comentario;
                celComentario.VerticalAlign = VerticalAlign.Middle;
                celComentario.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celComentario);

                this.phCajaTurno.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando movimiento de caja en PH. " + ex.Message));
            }

        }
        protected void btnConfirmarCierreTurno_Click(object sender, EventArgs e)
        {
            try
            {
                Cierre_Turno cierre = new Cierre_Turno();
                cierre.Fecha = DateTime.Now;
                cierre.IdSucursal = this.sucursal;
                cierre.IdPuntoVta = this.puntoVenta;
                cierre.Usuario = (int)Session["Login_IdUser"];
                cierre.IdTurno = Convert.ToInt32(this.ListTurnos.SelectedValue);
                cierre.Importe = Convert.ToDecimal(this.txtTotalCierreTurno.Text);
                cierre.Estado = 1;

                int ok = this.contCaja.agregarCierreTurno(cierre);
                int ok2 = this.contCaja.agregarMovCajaCierreTurno(cierre, this.sucursal, this.puntoVenta);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"Cierre turno generado con exito!. \", {type: \"info\"});cerrarModalCierreTurno();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo realizar cierre. \");", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", "$.msgbox(\"No se pudo realizar cierre. " + ex.Message + " \");", true);
            }
        }
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            this.generarResumen(1);
        }
        protected void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            this.generarResumen(0);
        }
        private void generarResumen(int excel)
        {
            try
            {
                if (excel == 1)
                {
                    Response.Redirect("ImpresionValores.aspx?a=8&valor=1&s=" + this.sucursal + "&pv=" + this.puntoVenta + "&FD=" + this.txtFecha.Text + "&FH=" + this.txtFechaApertura.Text);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "window.open('../Valores/ImpresionValores.aspx?a=8&s=" + this.sucursal + "&pv=" + this.puntoVenta + "&FD=" + this.txtFecha.Text + "&FH=" + this.txtFechaApertura.Text + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
            }
            catch
            {

            }
        }

        #endregion

        

        
    }
}