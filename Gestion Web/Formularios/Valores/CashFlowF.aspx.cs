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

namespace Gestion_Web.Formularios.Valores
{
    public partial class CashFlowF : System.Web.UI.Page
    {
        controladorInformes contInformes = new controladorInformes();
        controladorFunciones contFunciones = new controladorFunciones();
        ControladorCashFlow contCashFlow = new ControladorCashFlow();
        Mensajes mje = new Mensajes();

        int accion;
        int sucursal;
        int tipoPeriodo;
        string fechaD;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.fechaD = Request.QueryString["fd"];
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.tipoPeriodo = Convert.ToInt32(Request.QueryString["tp"]);

                this.VerificarLogin();                

                if (!IsPostBack)
                {
                    this.cargarSucursal();       
                }

                if (string.IsNullOrEmpty(this.fechaD))
                {
                    this.sucursal = (int)Session["Login_SucUser"];
                    this.fechaD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString("dd/MM/yyyy");
                    this.txtFechaDesde.Text = this.fechaD;
                }

                if (this.accion == 1)
                {
                    this.obtenerCashFlow();
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando CashFlow. Excepción: " + Ex.Message));
            }
        }

        #region Carga Inicial
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
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. Excepción: " + Ex.Message));
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
                        if (s == "67")
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
        private void obtenerCashFlow()
        {
            try
            {
                var dtIngresos = this.contCashFlow.obtenerIngresos(Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")), 1);

                if (dtIngresos != null)
                {

                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando CashFlow. Excepción: " + Ex.Message));
            }
        }
        private void cargarTablaIngresos(DataTable dt)
        {
            try
            {
                //Limpio controles del PlaceHolder de Ingresos
                this.phIngresos.Controls.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando tabla de ingresos. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CashFlowF.aspx?a=1&fd=" + this.txtFechaDesde.Text + "&tp=" + this.ListTipoPeriodo.SelectedValue);
            }
            catch
            {

            }
        }
        protected void btnCalcularMontos_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcularTotalIngresos();
                this.calcularTotalEgresos();
                this.calcularTotalGeneral();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error enviando a calcular montos. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Cash Flow
        private void generarInforme()
        {
            try
            {
                this.lblParametros.Text = this.DropListSucursal.SelectedItem.Text + ", " + this.fechaD;

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Ingresos
        private void cargarPhIngresos(DataRow dr)
        {
            try
            {
                TableRow tr = new TableRow();

                

                for (int i = 0; i < 12; i++)
                {
                    if (i == 0)
                    {
                        TableCell celConcepto = new TableCell();
                        celConcepto.Text = dr[i].ToString();
                        celConcepto.VerticalAlign = VerticalAlign.Middle;
                        celConcepto.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(celConcepto);
                    }
                    else
                    {
                        //TableCell celMontoPeriodo = new TableCell();
                        //celMontoPeriodo.HorizontalAlign = HorizontalAlign.Right;

                        //TextBox txtCantidad = new TextBox();
                        //txtCantidad.ID = txtIngresos;
                        //if (cant > 0)
                        //{
                        //    txtCantidad.Text = cant.ToString();
                        //}
                        //else
                        //{
                        //    txtCantidad.Text = "";
                        //}
                        //txtCantidad.TextMode = TextBoxMode.Number;
                        //txtCantidad.Attributes.Add("Style", "text-align: right;");
                        //txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                        //txtCantidad.AutoPostBack = true;
                        //txtCantidad.TextChanged += new EventHandler(this.cargarCantidadItem);
                        //celCantidad.Controls.Add(txtCantidad);
                        //tr.Cells.Add(celCantidad);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
        }
        #endregion

        #region Egresos

        #endregion

        #region Totales
        private void calcularTotalIngresos()
        {
            try
            {
                //Periodo 1
                decimal ingresosCaja1 = Convert.ToDecimal(this.txtIngresosCaja_1.Text);
                decimal ingresosCheques1 = Convert.ToDecimal(this.txtIngresosCheques_1.Text);
                decimal ingresosTarjetas1 = Convert.ToDecimal(this.txtIngresosTarjetas_1.Text);
                decimal ingresosCuentasCobrar1 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_1.Text);
                decimal ingresosBancos1 = Convert.ToDecimal(this.txtIngresosBancos_1.Text);
                decimal ingresosOtros1 = Convert.ToDecimal(this.txtIngresosOtros_1.Text);
                decimal ingresosVentas1 = Convert.ToDecimal(this.txtIngresosVentas_1.Text);
                decimal totalIngresos1 = ingresosCaja1 + ingresosCheques1 + ingresosTarjetas1 + ingresosCuentasCobrar1 + ingresosBancos1 + ingresosOtros1 + ingresosVentas1;

                this.lblTotalIngresos_1.Text = totalIngresos1.ToString();

                //Periodo 2
                decimal ingresosCaja2 = Convert.ToDecimal(this.txtIngresosCaja_2.Text);
                decimal ingresosCheques2 = Convert.ToDecimal(this.txtIngresosCheques_2.Text);
                decimal ingresosTarjetas2 = Convert.ToDecimal(this.txtIngresosTarjetas_2.Text);
                decimal ingresosCuentasCobrar2 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_2.Text);
                decimal ingresosBancos2 = Convert.ToDecimal(this.txtIngresosBancos_2.Text);
                decimal ingresosOtros2 = Convert.ToDecimal(this.txtIngresosOtros_2.Text);
                decimal ingresosVentas2 = Convert.ToDecimal(this.txtIngresosVentas_2.Text);
                decimal totalIngresos2 = ingresosCaja2 + ingresosCheques2 + ingresosTarjetas2 + ingresosCuentasCobrar2 + ingresosBancos2 + ingresosOtros2 + ingresosVentas2;

                this.lblTotalIngresos_2.Text = totalIngresos2.ToString();

                //Periodo 3
                decimal ingresosCaja3 = Convert.ToDecimal(this.txtIngresosCaja_3.Text);
                decimal ingresosCheques3 = Convert.ToDecimal(this.txtIngresosCheques_3.Text);
                decimal ingresosTarjetas3 = Convert.ToDecimal(this.txtIngresosTarjetas_3.Text);
                decimal ingresosCuentasCobrar3 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_3.Text);
                decimal ingresosBancos3 = Convert.ToDecimal(this.txtIngresosBancos_3.Text);
                decimal ingresosOtros3 = Convert.ToDecimal(this.txtIngresosOtros_3.Text);
                decimal ingresosVentas3 = Convert.ToDecimal(this.txtIngresosVentas_3.Text);
                decimal totalIngresos3 = ingresosCaja3 + ingresosCheques3 + ingresosTarjetas3 + ingresosCuentasCobrar3 + ingresosBancos3 + ingresosOtros3 + ingresosVentas3;

                this.lblTotalIngresos_3.Text = totalIngresos3.ToString();

                //Periodo 4
                decimal ingresosCaja4 = Convert.ToDecimal(this.txtIngresosCaja_4.Text);
                decimal ingresosCheques4 = Convert.ToDecimal(this.txtIngresosCheques_4.Text);
                decimal ingresosTarjetas4 = Convert.ToDecimal(this.txtIngresosTarjetas_4.Text);
                decimal ingresosCuentasCobrar4 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_4.Text);
                decimal ingresosBancos4 = Convert.ToDecimal(this.txtIngresosBancos_4.Text);
                decimal ingresosOtros4 = Convert.ToDecimal(this.txtIngresosOtros_4.Text);
                decimal ingresosVentas4 = Convert.ToDecimal(this.txtIngresosVentas_4.Text);
                decimal totalIngresos4 = ingresosCaja4 + ingresosCheques4 + ingresosTarjetas4 + ingresosCuentasCobrar4 + ingresosBancos4 + ingresosOtros4 + ingresosVentas4;

                this.lblTotalIngresos_4.Text = totalIngresos4.ToString();

                //Periodo 5
                decimal ingresosCaja5 = Convert.ToDecimal(this.txtIngresosCaja_5.Text);
                decimal ingresosCheques5 = Convert.ToDecimal(this.txtIngresosCheques_5.Text);
                decimal ingresosTarjetas5 = Convert.ToDecimal(this.txtIngresosTarjetas_5.Text);
                decimal ingresosCuentasCobrar5 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_5.Text);
                decimal ingresosBancos5 = Convert.ToDecimal(this.txtIngresosBancos_5.Text);
                decimal ingresosOtros5 = Convert.ToDecimal(this.txtIngresosOtros_5.Text);
                decimal ingresosVentas5 = Convert.ToDecimal(this.txtIngresosVentas_5.Text);
                decimal totalIngresos5 = ingresosCaja5 + ingresosCheques5 + ingresosTarjetas5 + ingresosCuentasCobrar5 + ingresosBancos5 + ingresosOtros5 + ingresosVentas5;

                this.lblTotalIngresos_5.Text = totalIngresos5.ToString();

                //Periodo 6
                decimal ingresosCaja6 = Convert.ToDecimal(this.txtIngresosCaja_6.Text);
                decimal ingresosCheques6 = Convert.ToDecimal(this.txtIngresosCheques_6.Text);
                decimal ingresosTarjetas6 = Convert.ToDecimal(this.txtIngresosTarjetas_6.Text);
                decimal ingresosCuentasCobrar6 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_6.Text);
                decimal ingresosBancos6 = Convert.ToDecimal(this.txtIngresosBancos_6.Text);
                decimal ingresosOtros6 = Convert.ToDecimal(this.txtIngresosOtros_6.Text);
                decimal ingresosVentas6 = Convert.ToDecimal(this.txtIngresosVentas_6.Text);
                decimal totalIngresos6 = ingresosCaja6 + ingresosCheques6 + ingresosTarjetas6 + ingresosCuentasCobrar6 + ingresosBancos6 + ingresosOtros6 + ingresosVentas6;

                this.lblTotalIngresos_6.Text = totalIngresos6.ToString();

                //Periodo 7
                decimal ingresosCaja7 = Convert.ToDecimal(this.txtIngresosCaja_7.Text);
                decimal ingresosCheques7 = Convert.ToDecimal(this.txtIngresosCheques_7.Text);
                decimal ingresosTarjetas7 = Convert.ToDecimal(this.txtIngresosTarjetas_7.Text);
                decimal ingresosCuentasCobrar7 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_7.Text);
                decimal ingresosBancos7 = Convert.ToDecimal(this.txtIngresosBancos_7.Text);
                decimal ingresosOtros7 = Convert.ToDecimal(this.txtIngresosOtros_7.Text);
                decimal ingresosVentas7 = Convert.ToDecimal(this.txtIngresosVentas_7.Text);
                decimal totalIngresos7 = ingresosCaja7 + ingresosCheques7 + ingresosTarjetas7 + ingresosCuentasCobrar7 + ingresosBancos7 + ingresosOtros7 + ingresosVentas7;

                this.lblTotalIngresos_7.Text = totalIngresos7.ToString();

                //Periodo 8
                decimal ingresosCaja8 = Convert.ToDecimal(this.txtIngresosCaja_8.Text);
                decimal ingresosCheques8 = Convert.ToDecimal(this.txtIngresosCheques_8.Text);
                decimal ingresosTarjetas8 = Convert.ToDecimal(this.txtIngresosTarjetas_8.Text);
                decimal ingresosCuentasCobrar8 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_8.Text);
                decimal ingresosBancos8 = Convert.ToDecimal(this.txtIngresosBancos_8.Text);
                decimal ingresosOtros8 = Convert.ToDecimal(this.txtIngresosOtros_8.Text);
                decimal ingresosVentas8 = Convert.ToDecimal(this.txtIngresosVentas_8.Text);
                decimal totalIngresos8 = ingresosCaja8 + ingresosCheques8 + ingresosTarjetas8 + ingresosCuentasCobrar8 + ingresosBancos8 + ingresosOtros8 + ingresosVentas8;

                this.lblTotalIngresos_8.Text = totalIngresos8.ToString();

                //Periodo 9
                decimal ingresosCaja9 = Convert.ToDecimal(this.txtIngresosCaja_9.Text);
                decimal ingresosCheques9 = Convert.ToDecimal(this.txtIngresosCheques_9.Text);
                decimal ingresosTarjetas9 = Convert.ToDecimal(this.txtIngresosTarjetas_9.Text);
                decimal ingresosCuentasCobrar9 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_9.Text);
                decimal ingresosBancos9 = Convert.ToDecimal(this.txtIngresosBancos_9.Text);
                decimal ingresosOtros9 = Convert.ToDecimal(this.txtIngresosOtros_9.Text);
                decimal ingresosVentas9 = Convert.ToDecimal(this.txtIngresosVentas_9.Text);
                decimal totalIngresos9 = ingresosCaja9 + ingresosCheques9 + ingresosTarjetas9 + ingresosCuentasCobrar9 + ingresosBancos9 + ingresosOtros9 + ingresosVentas9;

                this.lblTotalIngresos_9.Text = totalIngresos9.ToString();

                //Periodo 10
                decimal ingresosCaja10 = Convert.ToDecimal(this.txtIngresosCaja_10.Text);
                decimal ingresosCheques10 = Convert.ToDecimal(this.txtIngresosCheques_10.Text);
                decimal ingresosTarjetas10 = Convert.ToDecimal(this.txtIngresosTarjetas_10.Text);
                decimal ingresosCuentasCobrar10 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_10.Text);
                decimal ingresosBancos10 = Convert.ToDecimal(this.txtIngresosBancos_10.Text);
                decimal ingresosOtros10 = Convert.ToDecimal(this.txtIngresosOtros_10.Text);
                decimal ingresosVentas10 = Convert.ToDecimal(this.txtIngresosVentas_10.Text);
                decimal totalIngresos10 = ingresosCaja10 + ingresosCheques10 + ingresosTarjetas10 + ingresosCuentasCobrar10 + ingresosBancos10 + ingresosOtros10 + ingresosVentas10;

                this.lblTotalIngresos_10.Text = totalIngresos10.ToString();

                //Periodo 11
                decimal ingresosCaja11 = Convert.ToDecimal(this.txtIngresosCaja_11.Text);
                decimal ingresosCheques11 = Convert.ToDecimal(this.txtIngresosCheques_11.Text);
                decimal ingresosTarjetas11 = Convert.ToDecimal(this.txtIngresosTarjetas_11.Text);
                decimal ingresosCuentasCobrar11 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_11.Text);
                decimal ingresosBancos11 = Convert.ToDecimal(this.txtIngresosBancos_11.Text);
                decimal ingresosOtros11 = Convert.ToDecimal(this.txtIngresosOtros_11.Text);
                decimal ingresosVentas11 = Convert.ToDecimal(this.txtIngresosVentas_11.Text);
                decimal totalIngresos11 = ingresosCaja11 + ingresosCheques11 + ingresosTarjetas11 + ingresosCuentasCobrar11 + ingresosBancos11 + ingresosOtros11 + ingresosVentas11;

                this.lblTotalIngresos_11.Text = totalIngresos11.ToString();

                //Periodo 12
                decimal ingresosCaja12 = Convert.ToDecimal(this.txtIngresosCaja_12.Text);
                decimal ingresosCheques12 = Convert.ToDecimal(this.txtIngresosCheques_12.Text);
                decimal ingresosTarjetas12 = Convert.ToDecimal(this.txtIngresosTarjetas_12.Text);
                decimal ingresosCuentasCobrar12 = Convert.ToDecimal(this.txtIngresosCuentasCobrar_12.Text);
                decimal ingresosBancos12 = Convert.ToDecimal(this.txtIngresosBancos_12.Text);
                decimal ingresosOtros12 = Convert.ToDecimal(this.txtIngresosOtros_12.Text);
                decimal ingresosVentas12 = Convert.ToDecimal(this.txtIngresosVentas_12.Text);
                decimal totalIngresos12 = ingresosCaja12 + ingresosCheques12 + ingresosTarjetas12 + ingresosCuentasCobrar12 + ingresosBancos12 + ingresosOtros12 + ingresosVentas12;

                this.lblTotalIngresos_12.Text = totalIngresos12.ToString();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error calculando el Total de Ingresos. Excepción: " + Ex.Message));
            }
        }
        private void calcularTotalEgresos()
        {
            try
            {
                //Periodo 1
                decimal egresosGastosEfectivo1 = Convert.ToDecimal(this.txtEgresosEfectivo_1.Text);
                decimal egresosCheques1 = Convert.ToDecimal(this.txtEgresosCheques_1.Text);
                decimal egresosCuentasPagar1 = Convert.ToDecimal(this.txtEgresosCuentasPagar_1.Text);
                decimal egresosSueldos1 = Convert.ToDecimal(this.txtEgresosSueldos_1.Text);
                decimal egresosImpuestos1 = Convert.ToDecimal(this.txtEgresosImpuestos_1.Text);
                decimal egresosBancos1 = Convert.ToDecimal(this.txtEgresosBancos_1.Text);
                decimal totalEgresos1 = egresosGastosEfectivo1 + egresosCheques1 + egresosCuentasPagar1 + egresosSueldos1 + egresosImpuestos1 + egresosBancos1;

                this.lblTotalEgresos_1.Text = totalEgresos1.ToString();

                //Periodo 2
                decimal egresosGastosEfectivo2 = Convert.ToDecimal(this.txtEgresosEfectivo_2.Text);
                decimal egresosCheques2 = Convert.ToDecimal(this.txtEgresosCheques_2.Text);
                decimal egresosCuentasPagar2 = Convert.ToDecimal(this.txtEgresosCuentasPagar_2.Text);
                decimal egresosSueldos2 = Convert.ToDecimal(this.txtEgresosSueldos_2.Text);
                decimal egresosImpuestos2 = Convert.ToDecimal(this.txtEgresosImpuestos_2.Text);
                decimal egresosBancos2 = Convert.ToDecimal(this.txtEgresosBancos_2.Text);
                decimal totalEgresos2 = egresosGastosEfectivo2 + egresosCheques2 + egresosCuentasPagar2 + egresosSueldos2 + egresosImpuestos2 + egresosBancos2;

                this.lblTotalEgresos_2.Text = totalEgresos2.ToString();

                //Periodo 3
                decimal egresosGastosEfectivo3 = Convert.ToDecimal(this.txtEgresosEfectivo_3.Text);
                decimal egresosCheques3 = Convert.ToDecimal(this.txtEgresosCheques_3.Text);
                decimal egresosCuentasPagar3 = Convert.ToDecimal(this.txtEgresosCuentasPagar_3.Text);
                decimal egresosSueldos3 = Convert.ToDecimal(this.txtEgresosSueldos_3.Text);
                decimal egresosImpuestos3 = Convert.ToDecimal(this.txtEgresosImpuestos_3.Text);
                decimal egresosBancos3 = Convert.ToDecimal(this.txtEgresosBancos_3.Text);
                decimal totalEgresos3 = egresosGastosEfectivo3 + egresosCheques3 + egresosCuentasPagar3 + egresosSueldos3 + egresosImpuestos3 + egresosBancos3;

                this.lblTotalEgresos_3.Text = totalEgresos3.ToString();

                //Periodo 4
                decimal egresosGastosEfectivo4 = Convert.ToDecimal(this.txtEgresosEfectivo_4.Text);
                decimal egresosCheques4 = Convert.ToDecimal(this.txtEgresosCheques_4.Text);
                decimal egresosCuentasPagar4 = Convert.ToDecimal(this.txtEgresosCuentasPagar_4.Text);
                decimal egresosSueldos4 = Convert.ToDecimal(this.txtEgresosSueldos_4.Text);
                decimal egresosImpuestos4 = Convert.ToDecimal(this.txtEgresosImpuestos_4.Text);
                decimal egresosBancos4 = Convert.ToDecimal(this.txtEgresosBancos_4.Text);
                decimal totalEgresos4 = egresosGastosEfectivo4 + egresosCheques4 + egresosCuentasPagar4 + egresosSueldos4 + egresosImpuestos4 + egresosBancos4;

                this.lblTotalEgresos_4.Text = totalEgresos4.ToString();

                //Periodo 5
                decimal egresosGastosEfectivo5 = Convert.ToDecimal(this.txtEgresosEfectivo_5.Text);
                decimal egresosCheques5 = Convert.ToDecimal(this.txtEgresosCheques_5.Text);
                decimal egresosCuentasPagar5 = Convert.ToDecimal(this.txtEgresosCuentasPagar_5.Text);
                decimal egresosSueldos5 = Convert.ToDecimal(this.txtEgresosSueldos_5.Text);
                decimal egresosImpuestos5 = Convert.ToDecimal(this.txtEgresosImpuestos_5.Text);
                decimal egresosBancos5 = Convert.ToDecimal(this.txtEgresosBancos_5.Text);
                decimal totalEgresos5 = egresosGastosEfectivo5 + egresosCheques5 + egresosCuentasPagar5 + egresosSueldos5 + egresosImpuestos5 + egresosBancos5;

                this.lblTotalEgresos_5.Text = totalEgresos5.ToString();

                //Periodo 6
                decimal egresosGastosEfectivo6 = Convert.ToDecimal(this.txtEgresosEfectivo_6.Text);
                decimal egresosCheques6 = Convert.ToDecimal(this.txtEgresosCheques_6.Text);
                decimal egresosCuentasPagar6 = Convert.ToDecimal(this.txtEgresosCuentasPagar_6.Text);
                decimal egresosSueldos6 = Convert.ToDecimal(this.txtEgresosSueldos_6.Text);
                decimal egresosImpuestos6 = Convert.ToDecimal(this.txtEgresosImpuestos_6.Text);
                decimal egresosBancos6 = Convert.ToDecimal(this.txtEgresosBancos_6.Text);
                decimal totalEgresos6 = egresosGastosEfectivo6 + egresosCheques6 + egresosCuentasPagar6 + egresosSueldos6 + egresosImpuestos6 + egresosBancos6;

                this.lblTotalEgresos_6.Text = totalEgresos6.ToString();

                //Periodo 7
                decimal egresosGastosEfectivo7 = Convert.ToDecimal(this.txtEgresosEfectivo_7.Text);
                decimal egresosCheques7 = Convert.ToDecimal(this.txtEgresosCheques_7.Text);
                decimal egresosCuentasPagar7 = Convert.ToDecimal(this.txtEgresosCuentasPagar_7.Text);
                decimal egresosSueldos7 = Convert.ToDecimal(this.txtEgresosSueldos_7.Text);
                decimal egresosImpuestos7 = Convert.ToDecimal(this.txtEgresosImpuestos_7.Text);
                decimal egresosBancos7 = Convert.ToDecimal(this.txtEgresosBancos_7.Text);
                decimal totalEgresos7 = egresosGastosEfectivo7 + egresosCheques7 + egresosCuentasPagar7 + egresosSueldos7 + egresosImpuestos7 + egresosBancos7;

                this.lblTotalEgresos_7.Text = totalEgresos7.ToString();

                //Periodo 8
                decimal egresosGastosEfectivo8 = Convert.ToDecimal(this.txtEgresosEfectivo_8.Text);
                decimal egresosCheques8 = Convert.ToDecimal(this.txtEgresosCheques_8.Text);
                decimal egresosCuentasPagar8 = Convert.ToDecimal(this.txtEgresosCuentasPagar_8.Text);
                decimal egresosSueldos8 = Convert.ToDecimal(this.txtEgresosSueldos_8.Text);
                decimal egresosImpuestos8 = Convert.ToDecimal(this.txtEgresosImpuestos_8.Text);
                decimal egresosBancos8 = Convert.ToDecimal(this.txtEgresosBancos_8.Text);
                decimal totalEgresos8 = egresosGastosEfectivo8 + egresosCheques8 + egresosCuentasPagar8 + egresosSueldos8 + egresosImpuestos8 + egresosBancos8;

                this.lblTotalEgresos_8.Text = totalEgresos8.ToString();

                //Periodo 9
                decimal egresosGastosEfectivo9 = Convert.ToDecimal(this.txtEgresosEfectivo_9.Text);
                decimal egresosCheques9 = Convert.ToDecimal(this.txtEgresosCheques_9.Text);
                decimal egresosCuentasPagar9 = Convert.ToDecimal(this.txtEgresosCuentasPagar_9.Text);
                decimal egresosSueldos9 = Convert.ToDecimal(this.txtEgresosSueldos_9.Text);
                decimal egresosImpuestos9 = Convert.ToDecimal(this.txtEgresosImpuestos_9.Text);
                decimal egresosBancos9 = Convert.ToDecimal(this.txtEgresosBancos_9.Text);
                decimal totalEgresos9 = egresosGastosEfectivo9 + egresosCheques9 + egresosCuentasPagar9 + egresosSueldos9 + egresosImpuestos9 + egresosBancos9;

                this.lblTotalEgresos_9.Text = totalEgresos9.ToString();

                //Periodo 10
                decimal egresosGastosEfectivo10 = Convert.ToDecimal(this.txtEgresosEfectivo_10.Text);
                decimal egresosCheques10 = Convert.ToDecimal(this.txtEgresosCheques_10.Text);
                decimal egresosCuentasPagar10 = Convert.ToDecimal(this.txtEgresosCuentasPagar_10.Text);
                decimal egresosSueldos10 = Convert.ToDecimal(this.txtEgresosSueldos_10.Text);
                decimal egresosImpuestos10 = Convert.ToDecimal(this.txtEgresosImpuestos_10.Text);
                decimal egresosBancos10 = Convert.ToDecimal(this.txtEgresosBancos_10.Text);
                decimal totalEgresos10 = egresosGastosEfectivo10 + egresosCheques10 + egresosCuentasPagar10 + egresosSueldos10 + egresosImpuestos10 + egresosBancos10;

                this.lblTotalEgresos_10.Text = totalEgresos10.ToString();

                //Periodo 11
                decimal egresosGastosEfectivo11 = Convert.ToDecimal(this.txtEgresosEfectivo_11.Text);
                decimal egresosCheques11 = Convert.ToDecimal(this.txtEgresosCheques_11.Text);
                decimal egresosCuentasPagar11 = Convert.ToDecimal(this.txtEgresosCuentasPagar_11.Text);
                decimal egresosSueldos11 = Convert.ToDecimal(this.txtEgresosSueldos_11.Text);
                decimal egresosImpuestos11 = Convert.ToDecimal(this.txtEgresosImpuestos_11.Text);
                decimal egresosBancos11 = Convert.ToDecimal(this.txtEgresosBancos_11.Text);
                decimal totalEgresos11 = egresosGastosEfectivo11 + egresosCheques11 + egresosCuentasPagar11 + egresosSueldos11 + egresosImpuestos11 + egresosBancos11;

                this.lblTotalEgresos_11.Text = totalEgresos11.ToString();

                //Periodo 12
                decimal egresosGastosEfectivo12 = Convert.ToDecimal(this.txtEgresosEfectivo_12.Text);
                decimal egresosCheques12 = Convert.ToDecimal(this.txtEgresosCheques_12.Text);
                decimal egresosCuentasPagar12 = Convert.ToDecimal(this.txtEgresosCuentasPagar_12.Text);
                decimal egresosSueldos12 = Convert.ToDecimal(this.txtEgresosSueldos_12.Text);
                decimal egresosImpuestos12 = Convert.ToDecimal(this.txtEgresosImpuestos_12.Text);
                decimal egresosBancos12 = Convert.ToDecimal(this.txtEgresosBancos_12.Text);
                decimal totalEgresos12 = egresosGastosEfectivo12 + egresosCheques12 + egresosCuentasPagar12 + egresosSueldos12 + egresosImpuestos12 + egresosBancos12;

                this.lblTotalEgresos_12.Text = totalEgresos12.ToString();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error calculando el Total de Egresos. Excepción: " + Ex.Message));
            }
        }
        private void calcularTotalGeneral()
        {
            try
            {
                //Total Periodo 1
                decimal totalIngresos_1 = Convert.ToDecimal(this.lblTotalIngresos_1.Text);
                decimal totalEgresos_1 = Convert.ToDecimal(this.lblTotalEgresos_1.Text);
                decimal total_1 = totalIngresos_1 - totalEgresos_1;
                decimal totalPeriodoAnterior_1 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_1.Text);
                decimal totalFinal_1 = total_1 + totalPeriodoAnterior_1;
                this.lblTotal_1.Text = total_1.ToString();
                this.lblTotalFinal_1.Text = totalFinal_1.ToString();

                //Total Periodo 2
                decimal totalIngresos_2 = Convert.ToDecimal(this.lblTotalIngresos_2.Text);
                decimal totalEgresos_2 = Convert.ToDecimal(this.lblTotalEgresos_2.Text);
                decimal total_2 = totalIngresos_2 - totalEgresos_2;

                this.txtTotalPeriodoAnterior_2.Text = totalFinal_1.ToString();
                decimal totalPeriodoAnterior_2 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_2.Text);
                decimal totalFinal_2 = total_2 + totalPeriodoAnterior_2;

                this.lblTotal_2.Text = total_2.ToString();
                this.lblTotalFinal_2.Text = totalFinal_2.ToString();

                //Total Periodo 3
                decimal totalIngresos_3 = Convert.ToDecimal(this.lblTotalIngresos_3.Text);
                decimal totalEgresos_3 = Convert.ToDecimal(this.lblTotalEgresos_3.Text);
                decimal total_3 = totalIngresos_3 - totalEgresos_3;

                this.txtTotalPeriodoAnterior_3.Text = totalFinal_2.ToString();
                decimal totalPeriodoAnterior_3 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_3.Text);
                decimal totalFinal_3 = total_3 + totalPeriodoAnterior_3;

                this.lblTotal_3.Text = total_3.ToString();
                this.lblTotalFinal_3.Text = totalFinal_3.ToString();

                //Total Periodo 4
                decimal totalIngresos_4 = Convert.ToDecimal(this.lblTotalIngresos_4.Text);
                decimal totalEgresos_4 = Convert.ToDecimal(this.lblTotalEgresos_4.Text);
                decimal total_4 = totalIngresos_4 - totalEgresos_4;

                this.txtTotalPeriodoAnterior_4.Text = totalFinal_3.ToString();
                decimal totalPeriodoAnterior_4 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_4.Text);
                decimal totalFinal_4 = total_4 + totalPeriodoAnterior_4;

                this.lblTotal_4.Text = total_4.ToString();
                this.lblTotalFinal_4.Text = totalFinal_4.ToString();

                //Total Periodo 5
                decimal totalIngresos_5 = Convert.ToDecimal(this.lblTotalIngresos_5.Text);
                decimal totalEgresos_5 = Convert.ToDecimal(this.lblTotalEgresos_5.Text);
                decimal total_5 = totalIngresos_5 - totalEgresos_5;
                
                this.txtTotalPeriodoAnterior_5.Text = totalFinal_4.ToString();
                decimal totalPeriodoAnterior_5 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_5.Text);
                decimal totalFinal_5 = total_5 + totalPeriodoAnterior_5;

                this.lblTotal_5.Text = total_5.ToString();
                this.lblTotalFinal_5.Text = totalFinal_5.ToString();

                //Total Periodo 6
                decimal totalIngresos_6 = Convert.ToDecimal(this.lblTotalIngresos_6.Text);
                decimal totalEgresos_6 = Convert.ToDecimal(this.lblTotalEgresos_6.Text);
                decimal total_6 = totalIngresos_6 - totalEgresos_6;

                this.txtTotalPeriodoAnterior_6.Text = totalFinal_5.ToString();
                decimal totalPeriodoAnterior_6 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_6.Text);
                decimal totalFinal_6 = total_6 + totalPeriodoAnterior_6;

                this.lblTotal_6.Text = total_6.ToString();
                this.lblTotalFinal_6.Text = totalFinal_6.ToString();

                //Total Periodo 7
                decimal totalIngresos_7 = Convert.ToDecimal(this.lblTotalIngresos_7.Text);
                decimal totalEgresos_7 = Convert.ToDecimal(this.lblTotalEgresos_7.Text);
                decimal total_7 = totalIngresos_7 - totalEgresos_7;

                this.txtTotalPeriodoAnterior_7.Text = totalFinal_6.ToString();
                decimal totalPeriodoAnterior_7 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_7.Text);
                decimal totalFinal_7 = total_7 + totalPeriodoAnterior_7;

                this.lblTotal_7.Text = total_7.ToString();
                this.lblTotalFinal_7.Text = totalFinal_7.ToString();

                //Total Periodo 8
                decimal totalIngresos_8 = Convert.ToDecimal(this.lblTotalIngresos_8.Text);
                decimal totalEgresos_8 = Convert.ToDecimal(this.lblTotalEgresos_8.Text);
                decimal total_8 = totalIngresos_8 - totalEgresos_8;

                this.txtTotalPeriodoAnterior_8.Text = totalFinal_7.ToString();
                decimal totalPeriodoAnterior_8 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_8.Text);
                decimal totalFinal_8 = total_8 + totalPeriodoAnterior_8;

                this.lblTotal_8.Text = total_8.ToString();
                this.lblTotalFinal_8.Text = totalFinal_8.ToString();

                //Total Periodo 9
                decimal totalIngresos_9 = Convert.ToDecimal(this.lblTotalIngresos_9.Text);
                decimal totalEgresos_9 = Convert.ToDecimal(this.lblTotalEgresos_9.Text);
                decimal total_9 = totalIngresos_9 - totalEgresos_9;

                this.txtTotalPeriodoAnterior_9.Text = totalFinal_8.ToString();
                decimal totalPeriodoAnterior_9 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_9.Text);
                decimal totalFinal_9 = total_9 + totalPeriodoAnterior_9;

                this.lblTotal_9.Text = total_9.ToString();
                this.lblTotalFinal_9.Text = totalFinal_9.ToString();

                //Total Periodo 10
                decimal totalIngresos_10 = Convert.ToDecimal(this.lblTotalIngresos_10.Text);
                decimal totalEgresos_10 = Convert.ToDecimal(this.lblTotalEgresos_10.Text);
                decimal total_10 = totalIngresos_10 - totalEgresos_10;

                this.txtTotalPeriodoAnterior_10.Text = totalFinal_9.ToString();
                decimal totalPeriodoAnterior_10 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_10.Text);
                decimal totalFinal_10 = total_10 + totalPeriodoAnterior_10;

                this.lblTotal_10.Text = total_10.ToString();
                this.lblTotalFinal_10.Text = totalFinal_10.ToString();

                //Total Periodo 11
                decimal totalIngresos_11 = Convert.ToDecimal(this.lblTotalIngresos_11.Text);
                decimal totalEgresos_11 = Convert.ToDecimal(this.lblTotalEgresos_11.Text);
                decimal total_11 = totalIngresos_11 - totalEgresos_11;

                this.txtTotalPeriodoAnterior_11.Text = totalFinal_10.ToString();
                decimal totalPeriodoAnterior_11 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_11.Text);
                decimal totalFinal_11 = total_11 + totalPeriodoAnterior_11;

                this.lblTotal_11.Text = total_11.ToString();
                this.lblTotalFinal_11.Text = totalFinal_11.ToString();

                //Total Periodo 12
                decimal totalIngresos_12 = Convert.ToDecimal(this.lblTotalIngresos_12.Text);
                decimal totalEgresos_12 = Convert.ToDecimal(this.lblTotalEgresos_12.Text);
                decimal total_12 = totalIngresos_12 - totalEgresos_12;

                this.txtTotalPeriodoAnterior_12.Text = totalFinal_11.ToString();
                decimal totalPeriodoAnterior_12 = Convert.ToDecimal(this.txtTotalPeriodoAnterior_12.Text);
                decimal totalFinal_12 = total_12 + totalPeriodoAnterior_12;

                this.lblTotal_12.Text = total_12.ToString();
                this.lblTotalFinal_12.Text = totalFinal_12.ToString();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error calculando el Total General. Excepción: " + Ex.Message));
            }
        }
        #endregion

        
    }
}