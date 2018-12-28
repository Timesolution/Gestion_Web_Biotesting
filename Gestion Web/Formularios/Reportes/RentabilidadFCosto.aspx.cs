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
using Gestor_Solution.Controladores;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class RentabilidadFCosto : System.Web.UI.Page
    {
        controladorInformes cont = new controladorInformes();
        controladorCliente contCliente = new controladorCliente();
        int accion;
        int sucursal;
        int cliente;
        string desde;
        string hasta;
        string producto;
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.cliente = Convert.ToInt32(Request.QueryString["c"]);
                this.desde = Request.QueryString["d"];
                this.hasta = Request.QueryString["h"];                
                this.producto = Request.QueryString["p"];

                if (!IsPostBack)
                {
                    if (this.sucursal == 0)
                    {
                        sucursal = (int)Session["Login_SucUser"];
                    }
                    this.cargarSucursal();
                    this.cargarClientes();
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
            catch (Exception ex)
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
                        if (s == "65")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                            }
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

        public void cargarClientes()
        {
            try
            {
                var dt = contCliente.obtenerClientesDT();

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["Id"] = "-1";
                        dt.Rows.InsertAt(dr, 0);

                        DataRow dr2 = dt.NewRow();
                        dr2["alias"] = "Todos";
                        dr2["Id"] = "0";
                        dt.Rows.InsertAt(dr2, 1);

                    }

                    this.DropListCliente.DataSource = dt;
                    this.DropListCliente.DataValueField = "id";
                    this.DropListCliente.DataTextField = "alias";

                    this.DropListCliente.DataBind();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cliente. Excepción: " + Ex.Message));
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
                this.cargarRentabilidadCosto(desde, hasta, sucursal);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando obteniendo datos para generar informe de rentabilidad. " + ex.Message);
            }
        }

        private void cargarRentabilidadCosto(DateTime Desde, DateTime Hasta, int sucursal)
        {
            try
            {
                this.GridInforme.AutoGenerateColumns = false;
                DataTable datos = null;
                if (this.accion == 3)
                {

                    datos = this.cont.Reportes_Rentabilidad_CostosImponibleByDesc(sucursal, this.producto, this.cliente);
                }
                else
                {   
                    datos =this.cont.Reportes_Rentabilidad_CostosImponible(Desde, Hasta, sucursal,this.cliente);
                }

                decimal totalVendidoConIva = 0;
                decimal costoTotalConIVA = 0;
                decimal costoTotalSinIVA = 0;
                decimal totalVendidoSinIVA = 0;

                foreach (DataRow dr in datos.Rows)
                {
                    if (Convert.ToDecimal(dr["Costo"]) <= 0)
                    {
                        costoTotalSinIVA += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(0.01);
                        costoTotalConIVA += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(0.01);
                    }
                    else
                    {
                        costoTotalSinIVA += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo Imponible"]);
                        costoTotalConIVA += Convert.ToDecimal(dr["Cantidad"]) * Convert.ToDecimal(dr["Costo Imponible Con Iva"]);
                    }

                    totalVendidoConIva += Convert.ToDecimal(dr["Cantidad"]) * Decimal.Round(Convert.ToDecimal(dr["Precio Unitario"]), 4);//es sin iva
                    totalVendidoSinIVA += Convert.ToDecimal(dr["Cantidad"]) * Decimal.Round(Convert.ToDecimal(dr["Precio Unitario Sin Iva"]), 4);
                }

                decimal markUp = decimal.Round(totalVendidoSinIVA - costoTotalSinIVA, 4);
                decimal porcentajeMarkUp = decimal.Round(((totalVendidoSinIVA / costoTotalSinIVA) - 1) * 100, 4);

                decimal utilidad = decimal.Round(totalVendidoConIva - costoTotalConIVA, 4);
                decimal porcentajeUtilidad = decimal.Round(((costoTotalConIVA / totalVendidoConIva) - 1) * 100, 4);

                this.labelTotalVendidoConIva.Text = "$ " + decimal.Round(totalVendidoConIva, 4).ToString("N");
                this.labelTotalCostoSinIva.Text = "$ " + decimal.Round(costoTotalSinIVA, 4).ToString("N");

                this.labelTotalCostoConIva.Text = "$ " + decimal.Round(costoTotalConIVA, 4).ToString("N");

                this.labelTotalVendidoSinIva.Text = "$ " + decimal.Round(totalVendidoSinIVA, 4).ToString("N");

                this.lblMarkUp.Text = "$ " + decimal.Round(markUp, 4).ToString("N");
                this.lblPorcentajeMarkUp.Text = decimal.Round(porcentajeMarkUp, 4).ToString("N") + "%";

                this.lblUtilidad.Text = "$ " + decimal.Round(utilidad, 4).ToString("N");
                this.lblPorcentajeUtilidad.Text = decimal.Round(porcentajeUtilidad * -1, 4).ToString("N") + "%";

                this.GridInforme.DataSource = datos;
                
                var sum = datos.Compute("Sum([Rentabilidad Costo])", "");
                this.labelSaldo.Text = "$ " + decimal.Round(Convert.ToDecimal(sum),2).ToString("N");
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
                Response.Redirect("RentabilidadFCosto.aspx?a=2&d=" + this.txtFechaDesde.Text + "&h=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&c=" + this.DropListCliente.SelectedValue);
            }
            catch
            {

            }
        }

        protected void lbBuscarProducto_Click(object sender, EventArgs e)
        {
            string idSuc = Session["Login_SucUser"].ToString();
            Response.Redirect("RentabilidadFCosto.aspx?a=3&s=" + idSuc + "&p=" + txtBusqueda.Text + "&c=" + this.DropListCliente.SelectedValue);
        }

        protected void btnBuscarCodigoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contCliente.obtenerClientesAliasDT(buscar);

                if (dtClientes != null)
                {
                    if (dtClientes.Rows.Count > 1)
                    {
                        DataRow dr = dtClientes.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["Id"] = "-1";
                        dtClientes.Rows.InsertAt(dr, 0);
                    }

                    if (string.IsNullOrEmpty(this.txtCodCliente.Text))
                    {
                        DataRow dr2 = dtClientes.NewRow();
                        dr2["alias"] = "Todos";
                        dr2["Id"] = "0";
                        dtClientes.Rows.InsertAt(dr2, 1);
                    }

                    //Cargo la lista
                    this.DropListCliente.DataSource = dtClientes;
                    this.DropListCliente.DataValueField = "id";
                    this.DropListCliente.DataTextField = "alias";
                    this.DropListCliente.DataBind();

                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnReporteRentabilidad_Click(object sender, EventArgs e)
        {
            try
            {
                string parametros = this.lblPorcentajeMarkUp.Text + ";" + this.lblMarkUp.Text + ";" + this.labelTotalCostoConIva.Text + ";" + this.labelTotalVendidoSinIva.Text;
                if (this.accion == 2)
                    Response.Redirect("ImpresionRentabilidadD.aspx?a=3&fd=" + this.desde + "&fh=" + this.hasta + "&suc=" + this.sucursal + "&c=" + this.cliente + "&ex=1" + "&p=" + parametros);

                if (this.accion == 3)
                    Response.Redirect("ImpresionRentabilidadD.aspx?a=4&fd=" + this.desde + "&fh=" + this.hasta + "&suc=" + this.sucursal + "&c=" + this.cliente + "&ex=1" + "&art=" + this.producto + "&p=" + parametros);

                //Si no seleccionó ningún filtro, es decir accion = 0, le informo que tiene que filtrar
                if (this.accion == 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar algún filtro."));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error imprimiendo Reporte de Rentabilidad. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnReporteRentabilidadPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string parametros = this.lblPorcentajeMarkUp.Text + ";" + this.lblMarkUp.Text + ";" + this.labelTotalCostoConIva.Text + ";" + this.labelTotalVendidoSinIva.Text;
                
                if (this.accion == 2)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionRentabilidadD.aspx?a=3&fd=" + this.desde + "&fh=" + this.hasta + "&suc=" + this.sucursal + "&c=" + this.cliente + "&ex=0" + "&p=" + parametros + "','_blank');", true);

                if (this.accion == 3)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionRentabilidadD.aspx?a=4&fd=" + this.desde + "&fh=" + this.hasta + "&suc=" + this.sucursal + "&c=" + this.cliente + "&ex=0" + "&art=" + this.producto + "&p=" + parametros + "','_blank');", true);

                //Si no seleccionó ningún filtro, es decir accion = 0, le informo que tiene que filtrar
                if (this.accion == 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar algún filtro."));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error exportando Reporte de Rentabilidad. Excepción: " + Ex.Message));
            }
        }
    }
}