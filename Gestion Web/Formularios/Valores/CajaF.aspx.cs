using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CajaF : System.Web.UI.Page
    {
        ControladorCaja controlador = new ControladorCaja();
        controladorUsuario contUser = new controladorUsuario();
        controladorCobranza contCobranza = new controladorCobranza();
        controladorRemuneraciones contRemuneracion = new controladorRemuneraciones();
        controladorCajaEntity contCajaCierre = new controladorCajaEntity();
        controladorSucursal contSucursal = new controladorSucursal();
        ControladorBanco contBanco = new ControladorBanco();

        Mensajes m = new Mensajes();
        private int suc;
        private int ptoVenta;
        private string fechaD;
        private string fechaH;
        private int tipoPago;
        private int tipoMovimiento;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["FD"];
                this.fechaH = Request.QueryString["FH"];
                this.suc = Convert.ToInt32(Request.QueryString["S"]);
                this.ptoVenta = Convert.ToInt32(Request.QueryString["PV"]);
                this.tipoPago = Convert.ToInt32(Request.QueryString["TP"]);
                this.tipoMovimiento = Convert.ToInt32(Request.QueryString["TM"]);

                btnAgregarTraspaso.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarTraspaso, null) + ";");
                btnAgregarMovimientoBanco.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarMovimientoBanco, null) + ";");

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    if (fechaD == null && fechaH == null && suc == 0 && ptoVenta == 0 && tipoPago == 0 && tipoMovimiento == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        ptoVenta = (int)Session["Login_PtoUser"];
                        this.cargarSucursal();
                        this.cargarPuntoVta(suc);
                        //this.cargarTiposPago();
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        this.tipoPago = 1;
                        this.tipoMovimiento = 0;
                        this.txtFechaDesde.Text = this.fechaD;
                        this.txtFechaHasta.Text = this.fechaH;
                        this.DropListSucursal.SelectedValue = suc.ToString();
                        this.ListPuntoVenta.SelectedValue = ptoVenta.ToString();
                        this.ListTipos.SelectedValue = this.tipoPago.ToString();
                        this.ListMovimiento.SelectedValue = this.tipoMovimiento.ToString();

                        this.obtenerFechaUltimoCierre();


                        Response.Redirect("CajaF.aspx?FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&PV=" + ListPuntoVenta.SelectedValue + "&TP=" + ListTipos.SelectedValue + "&TM=" + ListMovimiento.SelectedValue);

                    }
                    this.cargarSucursal();
                    this.cargarSucursalOrigen();
                    this.cargarSucursalDestino();
                    this.cargarEmpresas();
                    //this.cargarTiposPago();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    ListTipos.SelectedValue = this.tipoPago.ToString();
                    this.ListMovimiento.SelectedValue = this.tipoMovimiento.ToString();
                    
                    DropListSucursal.SelectedValue = suc.ToString();
                    ListSucursal2.SelectedValue = suc.ToString();
                    ListSucursalOrigen.SelectedValue = suc.ToString();
                    ListSucursalOrigenMovimientoBanco.SelectedValue = suc.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.cargarPuntoVta2(Convert.ToInt32(this.ListSucursal2.SelectedValue));
                    this.cargarPuntoVentaOrigen(Convert.ToInt32(this.ListSucursalOrigen.SelectedValue));
                    this.cargarPuntoVentaOrigenMovimientoBanco(Convert.ToInt32(this.ListSucursalOrigenMovimientoBanco.SelectedValue));
                    this.cargarTiposMovimientos();

                    this.verificarAccesoCajaEspecial();
                }               

                if (fechaD != null && fechaH != null && suc != 0)
                {
                    this.cargarCajasRango(fechaD, fechaH, suc, tipoPago, tipoMovimiento, ptoVenta);
                    if (tipoPago == 1)
                    {
                        this.btnaEfectivo.Visible = true;
                    }
                    if (tipoPago == 5)
                    {
                        this.btnaTarjeta.Visible = true;
                    }
                }
                if (fechaD != null && fechaH != null && suc == 0)//todas
                {
                    this.cargarCajasRango(fechaD, fechaH, suc, tipoPago, tipoMovimiento, ptoVenta);
                    if (tipoPago == 1)
                    {
                        this.btnaEfectivo.Visible = true;
                    }
                    if (tipoPago == 5)
                    {
                        this.btnaTarjeta.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                    if(this.verificarAcceso() != 1)
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
                int valor = 0;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach(string s in listPermisos)
                {
                    if(!String.IsNullOrEmpty(s))
                    {
                        if (s == "45")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                                this.ListSucursal2.Attributes.Remove("disabled");
                                this.lbtnaEfectivo.Visible = true;
                                this.lbtnaTarjeta.Visible = true;
                            }
                            else
                            {
                                this.verficarPermisoCambiarSucursal();
                                int i = this.verficarPermisoTraspaso();
                                if (i > 0)
                                {
                                    this.lbtnaEfectivo.Visible = true;
                                    this.lbtnaTarjeta.Visible = true;
                                }
                            }
                            valor = 1;
                        }
                    }
                }

                //Verifico permisos de Movimiento Banco
                string permisoAgregarMovimientoBanco = listPermisos.Where(x => x == "146").FirstOrDefault();
                string permisoCambiarSucursalAgregarMovimientoBanco = listPermisos.Where(x => x == "147").FirstOrDefault();

                if (string.IsNullOrEmpty(permisoAgregarMovimientoBanco))
                    this.phPasarBanco.Visible = false;
                if (!string.IsNullOrEmpty(permisoCambiarSucursalAgregarMovimientoBanco))
                {
                    this.ListSucursalOrigenMovimientoBanco.Attributes.Remove("disabled");
                    this.ListSucursalOrigenMovimientoBanco.CssClass = "form-control";
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }
        public int verficarPermisoTraspaso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "77")
                        {                            
                            return 1;
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public void verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                string permiso = listPermisos.Where(x => x == "82").FirstOrDefault();

                if (permiso != null)
                {
                    this.DropListSucursal.Attributes.Remove("disabled");
                    this.ListSucursal2.Attributes.Remove("disabled");
                }
                
            }
            catch
            {

            }
        }
        public void verificarAccesoCajaEspecial()
        {
            try
            {
                int idUser = (int)Session["Login_IdUser"];
                int i = this.contCajaCierre.verificarAccesoCajaEspecial(this.ptoVenta, idUser);
                if(i < 0)
                {
                    Response.Redirect("/Default.aspx?m=1", false);
                }
            }
            catch
            {

            }
        }

        #region cargas iniciales
        public void cargarSucursal()
        {
            try
            {
                //saco las privadas 
                controladorSucursal contSucu = new controladorSucursal();
                string perfil = Session["Login_NombrePerfil"] as string;

                DataTable dt = new DataTable();

                if (perfil == "SuperAdministrador")
                {
                    dt = contSucu.obtenerSucursales();
                }
                else
                {
                    dt = contSucu.obtenerSucursalesSinPrivadas();
                }

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                this.ListSucursal2.DataSource = dt;
                this.ListSucursal2.DataValueField = "Id";
                this.ListSucursal2.DataTextField = "nombre";

                this.ListSucursal2.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarEmpresas()
        {
            try
            {
                //Obtengo las empresas
                var dt = this.contSucursal.obtenerEmpresas();

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Razon Social"] = "Seleccione...";
                        dr["Cuit"] = "-1";
                        dt.Rows.InsertAt(dr, 0);
                    }

                    this.ListEmpresaDestinoMovimientoBanco.DataSource = dt;
                    this.ListEmpresaDestinoMovimientoBanco.DataValueField = "Cuit";
                    this.ListEmpresaDestinoMovimientoBanco.DataTextField = "Razon Social";
                    this.ListEmpresaDestinoMovimientoBanco.DataBind();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Empresas"));
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMovimientoBanco, UpdatePanelMovimientoBanco.GetType(), "alert", "$.msgbox(\"Ocurrió un error cargando Empresas a la lista. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        public void cargarSucursalOrigen()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListSucursalOrigen.DataSource = dt;
                this.ListSucursalOrigen.DataValueField = "Id";
                this.ListSucursalOrigen.DataTextField = "nombre";

                this.ListSucursalOrigen.DataBind();

                this.ListSucursalOrigenMovimientoBanco.DataSource = dt;
                this.ListSucursalOrigenMovimientoBanco.DataValueField = "Id";
                this.ListSucursalOrigenMovimientoBanco.DataTextField = "nombre";
                this.ListSucursalOrigenMovimientoBanco.DataBind(); 
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales de origen. " + ex.Message));
            }
        }
        public void cargarSucursalDestino()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListSucursalDestino.DataSource = dt;
                this.ListSucursalDestino.DataValueField = "Id";
                this.ListSucursalDestino.DataTextField = "nombre";

                this.ListSucursalDestino.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }
        protected void ListSucursal2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarPuntoVta2(Convert.ToInt32(this.ListSucursal2.SelectedValue));
        }

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                if (sucu != 0)
                {
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["NombreFantasia"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListPuntoVenta.DataSource = dt;
                    this.ListPuntoVenta.DataValueField = "Id";
                    this.ListPuntoVenta.DataTextField = "NombreFantasia";

                    this.ListPuntoVenta.DataBind();

                    if (dt.Rows.Count == 2)
                    {
                        this.ListPuntoVenta.SelectedIndex = 1;
                    }
                }
                else
                {
                    dt.Clear();
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["NombreFantasia"] = "Todas";
                    dr["id"] = 0;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListPuntoVenta.DataSource = dt;
                    this.ListPuntoVenta.DataValueField = "Id";
                    this.ListPuntoVenta.DataTextField = "NombreFantasia";

                    this.ListPuntoVenta.DataBind();
                }

                //this.ListPuntoVenta2.DataSource = dt;
                //this.ListPuntoVenta2.DataValueField = "Id";
                //this.ListPuntoVenta2.DataTextField = "NombreFantasia";

                //this.ListPuntoVenta2.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta2(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);               

                this.ListPuntoVenta2.DataSource = dt;
                this.ListPuntoVenta2.DataValueField = "Id";
                this.ListPuntoVenta2.DataTextField = "NombreFantasia";

                this.ListPuntoVenta2.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta2.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVentaOrigen(int sucu)
        {
            controladorSucursal contSucu = new controladorSucursal();
            DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

            //agrego todos
            DataRow dr = dt.NewRow();
            dr["NombreFantasia"] = "Seleccione...";
            dr["id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            this.ListPuntoVentaOrigen.DataSource = dt;
            this.ListPuntoVentaOrigen.DataValueField = "Id";
            this.ListPuntoVentaOrigen.DataTextField = "NombreFantasia";

            this.ListPuntoVentaOrigen.DataBind();

            
        }
        public void cargarPuntoVentaOrigenMovimientoBanco(int idSucursal)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(idSucursal);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["NombreFantasia"] = "Seleccione...";
                        dr["id"] = -1;
                        dt.Rows.InsertAt(dr, 0);
                    }

                    this.ListPuntoVentaOrigenMovimientoBanco.DataSource = dt;
                    this.ListPuntoVentaOrigenMovimientoBanco.DataValueField = "Id";
                    this.ListPuntoVentaOrigenMovimientoBanco.DataTextField = "NombreFantasia";

                    this.ListPuntoVentaOrigenMovimientoBanco.DataBind();
                }
            }
            catch (Exception Ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMovimientoBanco, UpdatePanelMovimientoBanco.GetType(), "alert", "$.msgbox(\"Ocurrió un error cargando puntos de venta a la lista. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + Ex.Message));
            }
        }

        public void cargarPuntoVentaDestino(int sucu)
        {
            controladorSucursal contSucu = new controladorSucursal();
            DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

            //agrego todos
            DataRow dr = dt.NewRow();
            dr["NombreFantasia"] = "Seleccione...";
            dr["id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            this.ListPuntoVentaDestino.DataSource = dt;
            this.ListPuntoVentaDestino.DataValueField = "Id";
            this.ListPuntoVentaDestino.DataTextField = "NombreFantasia";

            this.ListPuntoVentaDestino.DataBind();
        }

        public void cargarTiposPago()
        {
            try
            {
                DataTable dt = controlador.obtenerTiposPagos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["tipoPago"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["tipoPago"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.ListTipos.DataSource = dt;
                this.ListTipos.DataValueField = "id";
                this.ListTipos.DataTextField = "tipoPago";

                this.ListTipos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos de pago. " + ex.Message));
            }
        }

        public void cargarTiposMovimientos()
        {
            try
            {
                DataTable dt = controlador.obtenerMovimientosCaja();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["tipo"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListTipos.DataSource = dt;
                this.DropListTipos.DataValueField = "id";
                this.DropListTipos.DataTextField = "tipo";

                this.DropListTipos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos de movimientos de Caja. " + ex.Message));
            }
        }
        #endregion
        
        private void obtenerFechaUltimoCierre()
        {
            try
            {
                Gestion_Api.Entitys.Caja_Cierre ultimo = this.contCajaCierre.obtenerUltimoCierre(this.suc);

                if (ultimo != null)
                {
                    this.txtFechaDesde.Text = ultimo.FechaApertura.Value.ToString("dd/MM/yyyy");
                }

            }
            catch
            {

            }
        }
        private void cargarCajasRango(string fechaD, string fechaH, int idSuc, int tipoPago, int tipoMovimiento, int idPtoVta)
        {
            try
            {

                if (fechaD != null && fechaH != null && suc != 0 && tipoPago == 0)
                {
                    if (this.ListTipos.SelectedValue == "0")
                    {
                        List<Caja> cajas = controlador.obtenerCajasRango(fechaD, fechaH, idSuc, tipoPago, tipoMovimiento, idPtoVta);
                        decimal saldo = 0;
                        foreach (Caja c in cajas)
                        {
                            if (this.cargarCaja(c))
                            {
                                this.cargarEnPh(c);
                                saldo += c.importe;
                            }
                        }
                        lblSaldo.Text = "$ " + saldo.ToString("N");
                        this.cargarLabel(fechaD, fechaH, idSuc, 0);

                    }
                    else
                    {
                        List<Caja> cajas = controlador.obtenerCajasRango(fechaD, fechaH, idSuc, tipoPago, tipoMovimiento, idPtoVta);

                        decimal saldo = 0;
                        foreach (Caja c in cajas)
                        {
                            if (this.cargarCaja(c))
                            {
                                this.cargarEnPh(c);
                                saldo += c.importe;
                            }
                        }
                        lblSaldo.Text = "$ " + saldo.ToString("N");
                        this.cargarLabel(fechaD, fechaH, idSuc, tipoPago);
                    }
                }
                else
                {
                    if (this.ListTipos.SelectedValue == "0" && suc != 0)//
                    {
                        List<Caja> cajas = controlador.obtenerCajasRangoReduc(fechaD, fechaH, idSuc);

                        decimal saldo = 0;
                        foreach (Caja c in cajas)
                        {
                            if (this.cargarCaja(c))
                            {
                                this.cargarEnPh(c);
                                saldo += c.importe;
                            }
                        }
                        lblSaldo.Text = "$ " + saldo.ToString("N");
                        this.cargarLabel(fechaD, fechaH, idSuc, 0);

                    }
                    else
                    {
                        List<Caja> cajas = controlador.obtenerCajasRango(fechaD, fechaH, idSuc, tipoPago, tipoMovimiento, idPtoVta);

                        decimal saldo = 0;
                        foreach (Caja c in cajas)
                        {
                            if (cargarCaja(c))
                            {
                                this.cargarEnPh(c);
                                saldo += c.importe;
                            }
                        }

                        lblSaldo.Text = "$ " + saldo.ToString("N");


                        this.cargarLabel(fechaD, fechaH, idSuc, tipoPago);

                    }
                }
                
                //Verificar si muetro saldo
                if (!mostrarSaldo())
                {
                    lblSaldo.Text = "$ " + "****.**";
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de movimientos de caja. " + ex.Message));
            }
        }

        private bool mostrarSaldo()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                string permiso = listPermisos.Where(x => x == "111").FirstOrDefault();

                if (permiso == null)
                {
                    DateTime fechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                    DateTime fechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                    var cierre = this.contCajaCierre.obtenerCierres(this.suc, this.ptoVenta, fechaDesde, fechaHasta);
                    
                    if(cierre.Count > 0)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando el permiso para mostrar saldo. Excepción: " + Ex.Message));
                return false;
            }
        }

        private bool cargarCaja(Caja c)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                
                var suc = contSucu.obtenerSucursalID(c.suc.id);
                if (suc.clienteDefecto == -2)
                {
                    string perfil = Session["Login_NombrePerfil"] as string;
                    
                    if (perfil == "SuperAdministrador")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        
        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int tipoPago)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (tipoPago > -1)
                {
                    label += ListTipos.Items.FindByValue(tipoPago.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

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
                    Gestion_Api.Entitys.Caja_Cierre cierre = this.contCajaCierre.obtenerCierreByID(c.cobro.id);
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
                if (c.tipoMovimiento == 11)
                {
                    TableCell celRecibo = new TableCell();
                    celRecibo.Text = "Traspaso a Banco";
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
                    TableCell celRecibo = new TableCell();
                    Gestion_Api.Entitys.PagoRemuneracione pago = this.contRemuneracion.obtenerPagoRemuneracionByID(c.cobro.id);
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

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Comentarios");
                btnDetalles.ID = "btnComent_" + c.id;
                btnDetalles.Text = "<span class='shortcut-icon icon-comment'></span>";
                btnDetalles.Font.Size = 12;
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.ComentariosCaja);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnRecibo = new LinkButton();
                btnRecibo.CssClass = "btn btn-info ui-tooltip";
                btnRecibo.Attributes.Add("data-toggle", "tooltip");
                btnRecibo.Attributes.Add("title data-original-title", "Comentarios");
                btnRecibo.ID = "btnRecibo_" + c.id;
                btnRecibo.Text = "<span class='shortcut-icon icon-search'></span>";
                btnRecibo.Font.Size = 12;
                btnRecibo.PostBackUrl = "";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //Movimiento de cobro cobro
                if (c.tipoMovimiento == 1)
                {
                    if (c.cobro.cliente != null)
                        btnRecibo.Click += new EventHandler(this.VerCobro);
                }
                if (c.tipoMovimiento == 2 || c.tipoMovimiento == 5 || c.tipoMovimiento == 6 || c.tipoMovimiento == 11)
                {
                    btnRecibo.Click += new EventHandler(this.ImpresionComprobante);
                }
                celAccion.Controls.Add(btnRecibo);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + c.id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);                

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                //celda comentarios para exportar
                TableCell celComentario = new TableCell();
                celComentario.Visible = false;
                celComentario.Text = c.comentario;
                celComentario.VerticalAlign = VerticalAlign.Middle;
                celComentario.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celComentario);

                this.phCaja.Controls.Add(tr);

                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando movimiento de caja en PH. " + ex.Message));
            }

        }

        #region acciones botones y ctrles

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1" && ListPuntoVenta.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("CajaF.aspx?FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&PV=" + ListPuntoVenta.SelectedValue + "&TP=" + ListTipos.SelectedValue + "&TM=" + ListMovimiento.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal y punto de venta."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos de caja. " + ex.Message));

            }
        }

        protected void lbtnAgregarCaja_Click(object sender, EventArgs e)
        {
            try
            {
                Caja caja = new Caja();
                caja.mov = this.controlador.obtenerMovimientoCajaID(Convert.ToInt32(this.DropListTipos.SelectedValue));
                caja.tipo = this.contCobranza.obtenerTipoPagoDesc("Efectivo");
                caja.importe = Convert.ToDecimal(this.txtImporte.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                if (caja.mov.tipo == 2 || caja.mov.tipo == 5)
                {
                    caja.importe = caja.importe * -1;
                }

                caja.tipoMovimiento = 2;
                //if (caja.mov.id == 5)
                //{
                //    caja.tipoMovimiento = 5;
                //}
                //else
                //{
                //    caja.tipoMovimiento = 2;
                //}
                caja.fecha = DateTime.Now;
                caja.suc.id = Convert.ToInt32(this.ListSucursal2.SelectedValue);
                caja.pv.id = Convert.ToInt32(this.ListPuntoVenta2.SelectedValue);
                //cargo comentarios
                caja.comentario = this.txtComentarios.Text;


                int i = this.controlador.agregar(caja, 0);
                if (i > 0)
                {
                    //agrego bien
                    //Log.EscribirSQL(idUsuario, "INFO", "Agrego el Grupo de Articulo: " + i);
                    //Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Caja: " + this.txtMovimiento.Text);
                    
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Caja cargado con exito", null));
                    this.fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    this.fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    this.tipoPago = 0;
                    this.tipoMovimiento = 0;
                    this.suc = (int)Session["Login_SucUser"];
                    Response.Redirect("CajaF.aspx?FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento);
                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Caja"));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando nuevo movimiento de Caja" + ex.Message));
            }
        }
        //pasaje de caja
        protected void btnAgregarTraspaso_Click(object sender, EventArgs e)
        {
            try
            {
                int sucOrigen = Convert.ToInt32(this.ListSucursalOrigen.SelectedValue);
                int ptoVentaOrigen = Convert.ToInt32(this.ListPuntoVentaOrigen.SelectedValue);

                int sucDestino = Convert.ToInt32(this.ListSucursalDestino.SelectedValue);
                int ptoVentaDestino = Convert.ToInt32(this.ListPuntoVentaDestino.SelectedValue);

                decimal importe = Convert.ToDecimal(this.txtImporteTraspaso.Text);
                string comentario = this.txtObservacionTraspaso.Text;

                if (ptoVentaDestino > 0 && ptoVentaOrigen > 0)
                {
                    int cierreOK = this.verificarCierreCaja();
                    if (cierreOK < 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se realizo un cierre de caja en el dia de hoy para el punto de venta destino."));
                        return;
                    }

                    int i = this.controlador.pasarCaja(sucOrigen, ptoVentaOrigen, sucDestino, ptoVentaDestino, importe, comentario);
                    if (i > 0)
                    {
                        //envio un mail avisando que se hizo la transferencia
                        this.enviarMailTraspasoCaja(sucOrigen, ptoVentaOrigen, sucDestino, ptoVentaDestino);

                        this.fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        this.tipoPago = 0;
                        this.tipoMovimiento = 0;
                        this.suc = (int)Session["Login_SucUser"];
                        Response.Redirect("CajaF.aspx?FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando pasaje de caja"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar ambos puntos de venta!."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando traspaso de caja " + ex.Message));
            }
        }
        
        protected void ListSucursalOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVentaOrigen(Convert.ToInt32(this.ListSucursalOrigen.SelectedValue));
        }

        protected void ListSucursalDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVentaDestino(Convert.ToInt32(this.ListSucursalDestino.SelectedValue));
        }       

        private void ComentariosCaja(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCaja = atributos[1];

                var c = this.controlador.obtenerCajaID(Convert.ToInt32(idCaja));
                string comentario;
                if (String.IsNullOrEmpty(c.comentario))
                {
                    comentario = "";
                }
                else
                {
                    comentario = Regex.Replace(c.comentario, @"\t|\n|\r", "");
                }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('" + comentario + "', '" + c.id + "')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        private void ImpresionComprobante(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCaja = atributos[1];

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "window.open('../Valores/ImpresionValores.aspx?Caja=" + idCaja + "&a=4', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        protected void VerCobro(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCobro = atributos[1];

                var caja = this.controlador.obtenerCajaID(Convert.ToInt32(idCobro));

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "window.open('../Cobros/ImpresionCobro.aspx?Cobro=" + caja.cobro.id + "&valor=2', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

               // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCobro.aspx?Cobro=" + caja.cobro.id + "&valor=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                

                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        protected void btnActualizarComentario_Click(object sender, EventArgs e)
        {
            try
            {

                //obtengo numero factura
                int idBoton = Convert.ToInt32(this.txtNumero.Text);

                int i = this.controlador.modificarCOmentario(idBoton, this.txtComentario2.Text);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        
        protected void btnaEfectivo_Click(object sender, EventArgs e)
        {
            try
            {
                //de Tarjeta (5) a Efectivo (1)
                modificarTipoPago(5, 1);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando movimiento de caja . " + ex.Message));
            }
        }

        protected void btnaTarjeta_Click(object sender, EventArgs e)
        {
            try
            {                
                //de Efectivo (1) a Tarjeta (5) 
                modificarTipoPago(1, 5);                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando movimiento de caja . " + ex.Message));
            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {

                //DataTable dtDatos = new DataTable();                
                //dtDatos.Columns.Add("fecha");
                //dtDatos.Columns.Add("descripcion");
                //dtDatos.Columns.Add("importe");
                //dtDatos.Columns.Add("tipo");
                //dtDatos.Columns.Add("comentario");

                //foreach (var control in this.phCaja.Controls)
                //{
                //    DataRow drDatos = dtDatos.NewRow();
                //    TableRow tr = control as TableRow;
                                        
                //    drDatos[0] = tr.Cells[0].Text;
                //    drDatos[1] = tr.Cells[1].Text;
                //    drDatos[2] = tr.Cells[2].Text;
                //    drDatos[3] = tr.Cells[3].Text;
                //    drDatos[4] = tr.Cells[5].Text;

                //    dtDatos.Rows.Add(drDatos);
                //}
                //Session.Add("datosCaja", dtDatos);
                //Session.Add("saldoCaja", this.lblSaldo.Text);

                //Response.Redirect("ImpresionValores.aspx?valor=1&a=2&FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&PV=" + ListPuntoVenta.SelectedValue + "&TP=" + ListTipos.SelectedValue + "&TM=" + ListMovimiento.SelectedValue);
                Response.Redirect("ImpresionValores.aspx?valor=1&a=2&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&PV=" + this.ptoVenta + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cta cte desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error exportando detalles de cta cte a excel. " + ex.Message);
            }
            
        }

        protected void lbtnGastos_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionValores.aspx?a=3&FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&PV=" + ListPuntoVenta.SelectedValue + "&TP=" + ListTipos.SelectedValue + "&TM=" + ListMovimiento.SelectedValue);
            }
            catch
            {

            }
        }
        
        protected void lbtnResumen_Click(object sender, EventArgs e)
        {
            try
            {                

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionValores.aspx?a=5&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&PV=" + this.ptoVenta + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);                
            }
            catch
            {

            }
        }

        protected void lbtnImprimirCaja_Click(object sender, EventArgs e)
        {
            try
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionValores.aspx?a=2&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&PV=" + this.ptoVenta + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void lbtnInformeCierres_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionValores.aspx?a=6&FD=" + this.fechaD + "&FH=" + this.fechaH + "&S=" + this.suc + "&PV=" + this.ptoVenta + "&TP=" + this.tipoPago + "&TM=" + this.tipoMovimiento + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        #endregion
        
        private int enviarMailTraspasoCaja(int sucOrigen, int ptoVtaOrigen, int sucDestino, int ptoVtaDestino)
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();
                Boolean fileOK = false;
                String path = Server.MapPath("../../images/Comprobante/" + this.ListPuntoVentaOrigen.SelectedValue + "/");

                decimal importe = Convert.ToDecimal(this.txtImporteTraspaso.Text);
                string comentario = this.txtObservacionTraspaso.Text;

                if (FileUpload1.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".jpg", ".png", ".gif" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                else
                {
                    int i = contFunciones.enviarMailTraspasoCaja(sucOrigen, ptoVtaOrigen, sucDestino, ptoVtaDestino, importe, comentario, null);
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (fileOK)
                {
                    string[] filePaths = Directory.GetFiles(path);
                    foreach (string filePath in filePaths)
                    {
                        File.Delete(filePath);
                    }

                    //guardo nombre archivo
                    string imagen = FileUpload1.FileName;
                    //lo subo
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);
                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    Attachment myAttachment = new Attachment(FileUpload1.FileContent, fileName);

                    int i = contFunciones.enviarMailTraspasoCaja(sucOrigen, ptoVtaOrigen, sucDestino, ptoVtaDestino, importe, comentario, myAttachment);
                }
                else
                {
                    return -2;
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }
        private int verificarCierreCaja()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                int sucursal = Convert.ToInt32(this.ListSucursalDestino.SelectedValue);
                int ptoVenta = Convert.ToInt32(this.ListPuntoVentaDestino.SelectedValue);

                var fecha = contCaja.obtenerUltimaApertura(sucursal, ptoVenta);
                //si la fecha de apertura es mas gande q hoy no lo dejo
                if (DateTime.Now < fecha)
                {
                    //ya existe una un cierre para el dia de hoy                    
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private void modificarTipoPago(int tipoPagoOrigen, int tipoPagoDestino)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phCaja.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[4] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = 0;
                    foreach (string id in idtildado.Split(';'))
                    {
                        if (id != "" && id != null)
                        {
                            //obtengo caja
                            Caja c = this.controlador.obtenerCajaID(Convert.ToInt32(id));
                            if (c != null && c.tipo.id == tipoPagoOrigen)
                            {
                                int j = this.controlador.moverCaja(c, tipoPagoDestino);
                                if (j < 0)
                                {
                                    i = -1;
                                }
                            }
                            else
                            {
                                if (c.tipo.id != tipoPagoOrigen)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Verifique el tipo de pago del movimiento seleccionado. "));
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando movimiento caja. "));
                                }

                            }
                        }

                    }
                    if (i > -1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Tipo de pago Caja modificado con exito. ", "CajaF.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando movimiento caja. "));
                    }

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar al menos un movimiento. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando movimiento de caja . " + ex.Message));
            }
        }

        protected void lbtnBuscarTipoMovNombre_Click(object sender, EventArgs e)
        {
            try
            {
                string busqueda = this.txtBusquedaTipoMov.Text;
                DataTable dt = controlador.obtenerMovimientosCajaByNombre(busqueda);

                ////agrego todos
                //DataRow dr = dt.NewRow();
                //dr["tipo"] = "Seleccione...";
                //dr["id"] = -1;
                //dt.Rows.InsertAt(dr, 0);

                this.DropListTipos.DataSource = dt;
                this.DropListTipos.DataValueField = "id";
                this.DropListTipos.DataTextField = "tipo";

                this.DropListTipos.DataBind();
            }
            catch
            {

            }
        }

        #region Movimiento a Banco
        protected void ListSucursalOrigenMovimientoBanco_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ListEmpresaDestinoMovimientoBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ListEmpresaDestinoMovimientoBanco.SelectedValue != "-1")
                    this.cargarCuentas(this.ListEmpresaDestinoMovimientoBanco.SelectedValue);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMovimientoBanco, UpdatePanelMovimientoBanco.GetType(), "alert", "$.msgbox(\"Ocurrió un error enviando a cargar Cuentas Bancarias a la lista. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }

        protected void btnAgregarMovimientoBanco_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifico si ingresó todos los datos

                //Valido antes de realizar el proceso
                int validar = this.validarMovimientoBanco();
                if (validar < 0)
                {
                    if (validar == -1)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe ingresar todos los datos. "));

                    if (validar == -2)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El importe de la transferencia debe ser mayor a 0. "));

                    if (validar == -3)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se realizó un cierre de caja en el dia de hoy en el Punto de Venta de origen. "));

                    return;
                }
                
                //Seteo valores para realizar el traspaso
                int idSucursalOrigen = Convert.ToInt32(this.ListSucursalOrigenMovimientoBanco.SelectedValue);
                int idPuntoVentaOrigen = Convert.ToInt32(this.ListPuntoVentaOrigenMovimientoBanco.SelectedValue);
                int idCuentaDestino = Convert.ToInt32(this.ListCuentaDestinoMovimientoBanco.SelectedValue);
                string cuenta = this.ListCuentaDestinoMovimientoBanco.SelectedItem.Text;
                string empresa = this.ListEmpresaDestinoMovimientoBanco.SelectedItem.Text;

                Gestion_Api.Entitys.CuentasBancarias_Movimientos mov = new Gestion_Api.Entitys.CuentasBancarias_Movimientos();
                mov.Fecha = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
                mov.Monto = Convert.ToDecimal(this.txtImporteMovimientoBanco.Text) * -1;
                mov.IdCuenta = Convert.ToInt32(this.ListCuentaDestinoMovimientoBanco.SelectedValue);
                //Obtengo el id del concepto del movimiento
                string concepto = WebConfigurationManager.AppSettings.Get("IdConceptoMovimientoCajaBanco");
                mov.IdConcepto = Convert.ToInt32(concepto);
                mov.Observaciones = this.txtObservacionMovimientoBanco.Text;

                int i = this.controlador.pasarCajaBanco(idSucursalOrigen, idPuntoVentaOrigen, cuenta, empresa, mov);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Movimiento agregado con éxito. ","CajaF.aspx"));
                if (i == -1)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error agregando movimiento a la Caja."));
                if (i == -2)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error agregando movimiento al Banco."));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error generando un movimiento de Caja a Banco. Excepción: " + Ex.Message));
            }
        }
        private int verificarCierreCajaMovimientoBanco(int idSucursal, int idPuntoVenta)
        {
            try
            {
                var fecha = this.controlador.obtenerUltimaApertura(idSucursal, idPuntoVenta);
                
                //si la fecha de apertura es mas gande q hoy no lo dejo
                if (DateTime.Now < fecha)
                    return -1;
                else
                    return 1;

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMovimientoBanco, UpdatePanelMovimientoBanco.GetType(), "alert", "$.msgbox(\"Ocurrió un error verificando cierre de caja para movimiento a Banco. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
                return -1;
            }
        }
        private void cargarCuentas(string cuit)
        {
            try
            {
                cuit = cuit.Replace("-", "");
                this.ListCuentaDestinoMovimientoBanco.Items.Clear();
                List<Gestion_Api.Entitys.CuentasBancaria> cuentas = this.contBanco.obtenerCuentasBancariasByCuit(cuit);

                if (cuentas != null)
                {
                    foreach (var cta in cuentas)
                    {
                        string text = cta.Banco1.entidad + " - " + cta.Numero;
                        this.ListCuentaDestinoMovimientoBanco.Items.Add(new ListItem(text, cta.Id.ToString()));
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelMovimientoBanco, UpdatePanelMovimientoBanco.GetType(), "alert", "$.msgbox(\"Ocurrió un error cargando Cuentas Bancarias a la lista. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        private int validarMovimientoBanco()
        {
            try
            {
                //Verifico si ingresó todos los datos
                if (String.IsNullOrEmpty(this.ListSucursalOrigenMovimientoBanco.SelectedValue) || this.ListSucursalOrigenMovimientoBanco.SelectedValue == "-1")
                    return -1;

                if (String.IsNullOrEmpty(this.ListPuntoVentaOrigenMovimientoBanco.SelectedValue) || this.ListPuntoVentaOrigenMovimientoBanco.SelectedValue == "-1")
                    return -1;

                if (String.IsNullOrEmpty(this.ListEmpresaDestinoMovimientoBanco.SelectedValue) || this.ListEmpresaDestinoMovimientoBanco.SelectedValue == "-1")
                    return -1;

                if (String.IsNullOrEmpty(this.ListCuentaDestinoMovimientoBanco.SelectedValue) || this.ListCuentaDestinoMovimientoBanco.SelectedValue == "-1")
                    return -1;

                if (String.IsNullOrEmpty(this.txtImporteMovimientoBanco.Text))
                    return -1;

                if (Convert.ToDecimal(this.txtImporteMovimientoBanco.Text) <= 0)
                    return -2;

                int cierreOk = this.verificarCierreCajaMovimientoBanco(Convert.ToInt32(this.ListSucursalOrigenMovimientoBanco.SelectedValue), Convert.ToInt32(this.ListPuntoVentaOrigenMovimientoBanco.SelectedValue));
                if (cierreOk < 0)
                {
                    return -3;
                }

                return 1;
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error validando datos para realizar Movimiento a Banco. Excepción: " + Ex.Message));
                return -1;
            }
        }
        #endregion

        protected void lbtnRemesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarUnicoTraspasoTildado())
                {
                    string idtildado = "";
                    foreach (Control C in phCaja.Controls)
                    {
                        TableRow tr = C as TableRow;
                        CheckBox ch = tr.Cells[4].Controls[4] as CheckBox;
                        if (ch.Checked == true)
                        {
                            idtildado += ch.ID.Substring(12, ch.ID.Length - 12);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al redireccionar a la pagina de creacion de remesa " + ex.Message);
            }
        }

        public bool ComprobarUnicoTraspasoTildado()
        {
            try
            {
                int tildados = 0;

                foreach (Control C in phCaja.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[4] as CheckBox;

                    string descripcion = tr.Cells[1].Text;

                    if (ch.Checked == true)
                    {
                        if(descripcion.Contains("Traspaso de Caja") || descripcion.Contains("Traspaso Caja"))
                            tildados++;
                    }
                }

                if (tildados <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un traspaso de caja!"));
                    return false;
                }
                if (tildados > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo un traspaso de caja!"));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error comprobando si hay un solo traspaso de caja seleccionado. " + ex.Message);
                return false;
            }
        }
    }    
}