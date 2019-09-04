using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Sucursales
{
    public partial class PuntoVentaABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        //controlador
        controladorSucursal controlador = new controladorSucursal();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorUsuario contUser = new controladorUsuario();
        //PuntoVenta
        PuntoVenta ptoVenta = new PuntoVenta();
        Empresa emp = new Empresa();
        //para saber si es alta(1) o modificacion(2)
        private int codigo;
        private int empresa;
        private int valor;
        private int idPuntoVenta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.codigo = Convert.ToInt32(Request.QueryString["codigo"]);
                this.empresa = Convert.ToInt32(Request.QueryString["empresa"]);
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idPuntoVenta = Convert.ToInt32(Request.QueryString["id"]);

                if (valor != 2)
                {
                    PuntoVenta pv = new PuntoVenta();

                    pv = controlador.obtenerLastPuntoVta(codigo);
                    string p = pv.puntoVenta;
                    int newp = Convert.ToInt32(p) + 1;
                    this.txtPuntoVta.Text = newp.ToString().PadLeft(4, '0');
                }

                this.txtPuntoVta.Enabled = false;
                emp = contr.obtenerEmpresa(empresa);
                this.txtEmpresa.Text = emp.RazonSocial;
                this.txtEmpresa.Enabled = false;
                txtEmpresa.CssClass = "form-control";
                txtPuntoVta.CssClass = "form-control";                

                if (!IsPostBack)
                {
                    CargarMonedaFacturacion();

                    if (valor == 2)
                    {
                        PuntoVenta pv2 = controlador.obtenerPtoVentaId(idPuntoVenta);
                        Gestion_Api.Entitys.PuntoVta datosMail = this.controlador.obtenerPtoVentaEntityID(idPuntoVenta);

                        if (datosMail.PuntoVta_Datos.Count > 0)
                        {
                            this.txtMailPtoVenta.Text = datosMail.PuntoVta_Datos.FirstOrDefault().MailContacto;
                            this.txtTelPtoVenta.Text = datosMail.PuntoVta_Datos.FirstOrDefault().TelefonoContacto;
                        }

                        this.txtPuntoVta.Text = pv2.puntoVenta;
                        this.ddlFormaFactura.SelectedValue = pv2.formaFacturar;
                        DropDownListMonedaFacturacion.SelectedValue = pv2.monedaFacturacion.ToString();
                        this.txtNombreFantasia.Text = pv2.nombre_fantasia;
                        this.txtDireccion.Text = pv2.direccion;
                        if (pv2.retiene_ib)
                        {
                            this.ddlRetIngresosBrutos.SelectedValue = "Si";
                        }
                        else
                        {
                            this.ddlRetIngresosBrutos.SelectedValue = "No";
                        }

                        if (pv2.retiene_gan)
                        {
                            this.ddlRetieneGanancias.SelectedValue = "Si";
                        }
                        else
                        {
                            this.ddlRetieneGanancias.SelectedValue = "No";
                        }
                        if (this.ddlFormaFactura.SelectedValue == "Fiscal")
                        {
                            this.panelFiscal.Visible = true;
                            this.txtTope.Text = pv2.tope.ToString();
                        }
                        this.panelContacto.Visible = true;

                        try
                        {
                            var pvC = controlador.obtenerPuntoVentaPV(pv2.puntoVenta, pv2.id_suc, pv2.empresa.id);
                            this.txtCAIRemito.Text = pvC.caiRemito;
                            this.txtCAIVencimiento.Text = pvC.caiVencimiento.ToString("dd/MM/yyyy");
                        }
                        catch
                        { }
                    }
                }
            }
            catch
            {

            }
        }

        void CargarMonedaFacturacion()
        {
            try
            {
                controladorMoneda controladorMoneda = new controladorMoneda();

                var monedas = controladorMoneda.obtenerMonedasDT();

                DropDownListMonedaFacturacion.DataSource = monedas;
                DropDownListMonedaFacturacion.DataValueField = "id";
                DropDownListMonedaFacturacion.DataTextField = "moneda";
                DropDownListMonedaFacturacion.DataBind();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "error", "Error cargando moneda facturacion: " + ex.Message);
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
                        if (s == "10")
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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    PuntoVenta ptoVenta = new PuntoVenta();
                    Gestion_Api.Entitys.PuntoVta pvDatosMail = this.controlador.obtenerPtoVentaEntityID(this.idPuntoVenta);

                    ptoVenta.id = idPuntoVenta;
                    ptoVenta.id_suc = codigo;
                    ptoVenta.puntoVenta = this.txtPuntoVta.Text;
                    ptoVenta.formaFacturar = this.ddlFormaFactura.SelectedValue;
                    if (this.ddlRetIngresosBrutos.SelectedValue.ToString() == "Si")
                    {
                        ptoVenta.retiene_ib = true;
                    }
                    else
                    { 
                        ptoVenta.retiene_ib = false;
                    }

                    if (this.ddlRetieneGanancias.SelectedValue.ToString() == "Si")
                    {
                        ptoVenta.retiene_gan = true;
                    }
                    else
                    {
                        ptoVenta.retiene_gan = false;
                    }
                    ptoVenta.nombre_fantasia = this.txtNombreFantasia.Text;
                    ptoVenta.direccion = this.txtDireccion.Text;
                    ptoVenta.empresa.id = empresa;
                    ptoVenta.estado = 1;
                    ptoVenta.tope = Convert.ToDecimal(this.txtTope.Text);

                    string mailsPuntoVenta = this.txtMailPtoVenta.Text.Replace(" ", "");

                    if (pvDatosMail.PuntoVta_Datos.Count > 0)
                    {
                        pvDatosMail.PuntoVta_Datos.FirstOrDefault().MailContacto = mailsPuntoVenta;
                        pvDatosMail.PuntoVta_Datos.FirstOrDefault().TelefonoContacto = this.txtTelPtoVenta.Text;
                    }
                    else
                    {
                        Gestion_Api.Entitys.PuntoVta_Datos datos = new Gestion_Api.Entitys.PuntoVta_Datos();
                        datos.MailContacto = mailsPuntoVenta;
                        datos.TelefonoContacto = this.txtTelPtoVenta.Text;

                        pvDatosMail.PuntoVta_Datos.Add(datos);
                    }
                    try
                    {
                        ptoVenta.caiRemito = this.txtCAIRemito.Text;
                        ptoVenta.caiVencimiento = Convert.ToDateTime(this.txtCAIVencimiento.Text, new CultureInfo("es-AR"));
                        ptoVenta.monedaFacturacion = Convert.ToInt32(DropDownListMonedaFacturacion.SelectedValue);
                    }
                    catch
                    { }

                    int i = this.controlador.editarPtoVenta(ptoVenta);
                    if (i > 0)
                    {
                        this.controlador.modificarPtoVentaEntity(pvDatosMail);
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Punto de Venta: " + ptoVenta.nombre_fantasia);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Punto de venta editado con exito", "ABMPuntoVenta.aspx?codigo=" + codigo + "&empresa=" + empresa));
                        borrarCampos();
                    }
                }
                else
                {
                    PuntoVenta ptoVenta = new PuntoVenta();
                    ptoVenta.id_suc = codigo;
                    ptoVenta.puntoVenta = this.txtPuntoVta.Text;
                    ptoVenta.formaFacturar = this.ddlFormaFactura.SelectedValue;

                    if (this.ddlRetIngresosBrutos.SelectedValue.ToString() == "Si")
                    {
                        ptoVenta.retiene_ib = true;
                    }
                    else
                    {
                        ptoVenta.retiene_ib = false;
                    }

                    if (this.ddlRetieneGanancias.SelectedValue.ToString() == "Si")
                    {
                        ptoVenta.retiene_ib = true;
                    }
                    else
                    {
                        ptoVenta.retiene_ib = false;
                    }

                    ptoVenta.nombre_fantasia = this.txtNombreFantasia.Text;
                    ptoVenta.direccion = this.txtDireccion.Text;
                    ptoVenta.empresa.id = empresa;
                    ptoVenta.tope = Convert.ToDecimal(this.txtTope.Text);
                    ptoVenta.monedaFacturacion = Convert.ToInt32(DropDownListMonedaFacturacion.SelectedValue);
                    int i = this.controlador.agregarPtoVenta(ptoVenta);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Punto de Venta: " + ptoVenta.nombre_fantasia);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Punto de venta cargada con exito", "ABMPuntoVenta.aspx?codigo=" + codigo + "&empresa=" + empresa));
                        //this.cargarSucursal();
                        borrarCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando punto de venta . " + ex.Message));
            }
        }

        public void borrarCampos()
        {
            try
            {
                PuntoVenta pv = new PuntoVenta();
                this.txtNombreFantasia.Text = "";
                this.txtDireccion.Text = "";
                pv = controlador.obtenerLastPuntoVta(codigo);
                string p = pv.puntoVenta;
                int newp = Convert.ToInt32(p) + 1;
                this.txtPuntoVta.Text = newp.ToString().PadLeft(4, '0');
                this.txtTope.Text = "0.00";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void editarPuntoVenta(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMPuntoVenta.aspx?codigo=" + codigo + "&empresa=" + empresa + "&valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar sucursales. " + ex.Message));
            }
        }

        protected void ddlFormaFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string forma = this.ddlFormaFactura.SelectedItem.Text;
                if (forma == "Fiscal")
                {
                    this.panelFiscal.Visible = true;
                    this.txtTope.Text = "0.00";
                }
                else
                {
                    this.panelFiscal.Visible = false;
                    this.txtTope.Text = "0.00";
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}