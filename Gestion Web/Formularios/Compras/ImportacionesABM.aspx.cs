using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImportacionesABM : System.Web.UI.Page
    {
        ControladorImportaciones contImportaciones = new ControladorImportaciones();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();

        Mensajes m = new Mensajes();
        int accion;
        int id;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.id = Convert.ToInt32(Request.QueryString["id"]);
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    //cargo fecha de hoy
                    this.txtFechaFactura.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaDespacho.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.cargarProveedores();
                    this.cargarSucursal();
                    this.cargarMonedasImportacion();

                    if (this.accion == 2) 
                    {
                        this.cargarImportacion();                        
                    }
                }
            }
            catch (Exception ex)
            {
 
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
                        if (s == "29")
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
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
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
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

                this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarMonedasImportacion()
        {
            try
            {
                List<Monedas_Importacion> list = this.contImportaciones.obtenerMonedasImportacion();

                this.ListMonedaImportacion.DataSource = list;
                this.ListMonedaImportacion.DataValueField = "Id";
                this.ListMonedaImportacion.DataTextField = "Moneda";
                this.ListMonedaImportacion.DataBind();

                this.ListMonedaImportacion.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                {
                    this.modificarImportacion();
                }
                else
                {
                    this.agregarImportacion();
                }
            }
            catch
            {

            }
        }
        private void agregarImportacion()
        {
            try
            {
                Importacione importacion = new Importacione();
                importacion.Sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                importacion.Proveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                importacion.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                importacion.NroDespacho = this.txtNumeroDespacho.Text;
                importacion.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text, new CultureInfo("es-AR"));
                importacion.NroFactura = this.txtNumeroFactura.Text;
                importacion.DolarDespacho = Convert.ToDecimal(this.txtCotizacionDespacho.Text);
                importacion.RelacionUsdEuro = Convert.ToDecimal(this.txtRelacionEuro.Text);
                importacion.TotalFactura = Convert.ToDecimal(this.txtTotalFactura.Text);
                importacion.TotalDespacho = Convert.ToDecimal(this.txtTotalDespacho.Text);
                importacion.MonedaFactura = Convert.ToInt32(this.ListMonedaImportacion.SelectedValue);
                importacion.Coeficiente = Convert.ToDecimal(this.txtCoeficienteDF.Text);
                importacion.CodigoAutorizacion = this.txtCodigoAutorizacion.Text;
                importacion.NroReferencia = this.txtNroReferencia.Text;
                importacion.TipoCambioGastos = Convert.ToDecimal(this.txtCambioGastos.Text);
                importacion.Observaciones = this.txtObservaciones.Text;
                importacion.MercaderiaArribo = 0;
                importacion.Estado = 1;
                int ok = this.contImportaciones.agregarImportacion(importacion);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Agregada con Exito\", {type: \"info\"});location.href = 'ImportacionesF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se Pudo agregar.\";", true);                    
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error. " + ex.Message + " .\";", true);
            }
        }
        private void modificarImportacion()
        {
            try
            {
                Importacione importacion = this.contImportaciones.obtenerImportacionByID(this.id);
                importacion.Sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                importacion.Proveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                importacion.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                importacion.NroDespacho = this.txtNumeroDespacho.Text;
                importacion.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text, new CultureInfo("es-AR"));
                importacion.NroFactura = this.txtNumeroFactura.Text;
                importacion.DolarDespacho = Convert.ToDecimal(this.txtCotizacionDespacho.Text);
                importacion.RelacionUsdEuro = Convert.ToDecimal(this.txtRelacionEuro.Text);
                importacion.TotalFactura = Convert.ToDecimal(this.txtTotalFactura.Text);
                importacion.TotalDespacho = Convert.ToDecimal(this.txtTotalDespacho.Text);
                importacion.MonedaFactura = Convert.ToInt32(this.ListMonedaImportacion.SelectedValue);
                importacion.Coeficiente = Convert.ToDecimal(this.txtCoeficienteDF.Text);
                importacion.CodigoAutorizacion = this.txtCodigoAutorizacion.Text;
                importacion.NroReferencia = this.txtNroReferencia.Text;
                importacion.TipoCambioGastos = Convert.ToDecimal(this.txtCambioGastos.Text);
                importacion.Observaciones = this.txtObservaciones.Text;                
                int ok = this.contImportaciones.modificarImportacion(importacion);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Modificado con Exito\", {type: \"info\"});location.href = 'ImportacionesF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se Pudo Modificado.\";", true);
                }

            }
            catch
            {

            }
        }
        private void cargarImportacion()
        {
            try
            {
                Importacione i = this.contImportaciones.obtenerImportacionByID(this.id);
                if (i != null)
                {
                    this.ListSucursal.SelectedValue = i.Sucursal.Value.ToString();
                    this.ListProveedor.SelectedValue = i.Proveedor.Value.ToString();
                    this.txtFechaDespacho.Text = i.FechaDespacho.Value.ToString("dd/MM/yyyy");
                    this.txtNumeroDespacho.Text = i.NroDespacho;
                    this.txtFechaFactura.Text = i.FechaFactura.Value.ToString("dd/MM/yyyy");
                    this.txtNumeroFactura.Text = i.NroFactura;
                    this.txtCotizacionDespacho.Text = i.DolarDespacho.Value.ToString();
                    this.txtRelacionEuro.Text = i.RelacionUsdEuro.Value.ToString();
                    this.txtTotalFactura.Text = i.TotalFactura.Value.ToString();
                    this.txtTotalDespacho.Text = i.TotalDespacho.Value.ToString();
                    this.ListMonedaImportacion.SelectedValue = i.MonedaFactura.Value.ToString();
                    this.txtCoeficienteDF.Text = i.Coeficiente.Value.ToString();
                    this.txtCodigoAutorizacion.Text = i.CodigoAutorizacion;
                    this.txtNroReferencia.Text = i.NroReferencia;
                    this.txtCambioGastos.Text = i.TipoCambioGastos.Value.ToString();
                    this.txtObservaciones.Text = i.Observaciones;
                }
            }
            catch
            {

            }
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                Request.UrlReferrer.ToString();
            }
            catch
            {

            }
        }
    }
}