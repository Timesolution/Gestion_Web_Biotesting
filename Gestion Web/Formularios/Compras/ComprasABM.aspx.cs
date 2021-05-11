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
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ComprasABM : System.Web.UI.Page
    {
        //mensajes popUp
        Mensajes m = new Mensajes();
        int esUruguay = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsUruguay"));
        //controladores
        private controladorCompras controlador = new controladorCompras();
        private controladorCompraEntity contEntity = new controladorCompraEntity();
        ControladorPlanCuentas contPlanCta = new ControladorPlanCuentas();
        ControladorCCProveedor contCCProveedor = new ControladorCCProveedor();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        //private string cuit;
        private long idCompra;
        private long idRemito;
        private long idOrdenCompra;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();
                accion = Convert.ToInt32(Request.QueryString["a"]);
                idCompra = Convert.ToInt32(Request.QueryString["c"]);
                idRemito = Convert.ToInt64(Request.QueryString["r"]);
                idOrdenCompra = Convert.ToInt64(Request.QueryString["oc"]);

                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                btnAgregarPagar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarPagar, null) + ";");

                if (esUruguay == 1)
                {
                    divNeto2.Visible = false;
                    divNeto5.Visible = false;
                    divNeto10.Visible = false;
                    divNeto21.Visible = false;
                    divNeto27.Visible = false;
                    divIngBru.Visible = false;
                    divPercepcionIVA.Visible = false;
                    divImpInt.Visible = false;
                    divRetIIBB.Visible = false;
                    divRetIVA.Visible = false;
                    divRetGan.Visible = false;
                    divRetSUSS.Visible = false;
                    divITC.Visible = false;
                    divTasaCO2.Visible = false;
                }

                if (!IsPostBack)
                {
                    cargarEmpresas();
                    ListEmpresa.SelectedValue = Session["Login_EmpUser"].ToString();
                    cargarSucursal(Convert.ToInt32(ListEmpresa.SelectedValue));
                    cargarProveedores();
                    //pongo fecha de hoy
                    txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtVencimiento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtImputacionCont.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    if (accion == 2)
                    {
                        cargarCompra();
                    }
                    if (accion == 3)
                    {
                        CargarCompraDesdeRemito();
                    }
                    if (accion == 4)
                    {
                        CargarCompraDesdeOrdenCompra();
                    }
                    if (accion == 5)
                    {
                        EditarCompra();
                    }
                    //cargo sucursal
                    ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                    if (ListSucursal.SelectedValue != "")
                    {
                        ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                        cargarPuntoVta(Convert.ToInt32(ListSucursal.SelectedValue));
                    }
                    cargarCuentas();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error inicializando formulario. " + ex.Message));
            }
        }

        private void CargarCompraDesdeOrdenCompra()
        {
            try
            {
                var remitosCompra = contEntity.ObtenerRemitoComprasDiferenciasPorOrdenCompra(idOrdenCompra);
                string numerosRemitos = string.Empty;

                txtObservaciones.Text = "Remitos relacionados con orden de compra: ";

                foreach (var remitoCompra in remitosCompra)
                {
                    var puntoVenta = remitoCompra.Numero.Substring(0, 4);
                    var numero = remitoCompra.Numero.Substring(4, 8);

                    numerosRemitos += puntoVenta + "-" + numero + " ";
                }

                txtObservaciones.Text += numerosRemitos;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar compra desde orden de compra " + ex.Message);
            }
        }

        #region Carga Inicial
        private void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                if (dt.Rows.Count > 1)
                {
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["Razon Social"] = "Seleccione...";
                    dr["Id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                }

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";

                this.ListEmpresa.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Empresas. " + ex.Message));
            }
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                if (dt.Rows.Count > 1)
                {
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["nombre"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                }

                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";

                this.ListSucursal.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu, int idPuntoVenta = 0)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";

                this.ListPuntoVenta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta.SelectedIndex = 1;
                }
                else
                {
                    if (idPuntoVenta > 0)
                    {
                        ListPuntoVenta.SelectedValue = idPuntoVenta.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
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

        private void EditarCompra()
        {
            try
            {
                Gestion_Api.Entitys.Compra c = this.contEntity.obtenerCompraId(this.idCompra);
                this.ListEmpresa.SelectedValue = c.IdEmpresa.ToString();
                this.cargarSucursal(c.IdEmpresa.Value);
                this.ListSucursal.SelectedValue = c.IdSucursal.ToString();
                this.cargarPuntoVta(c.IdSucursal.Value, (int)c.IdPuntoVenta);
                this.txtFecha.Text = Convert.ToDateTime(c.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                this.ListTipoDocumento.SelectedValue = c.TipoDocumento;
                this.txtPVenta.Text = c.PuntoVenta;
                this.txtNumero.Text = c.Numero;
                this.ListProveedor.SelectedValue = c.Proveedor.ToString();
                this.txtCuit.Text = c.Cuit;
                this.txtIva.Text = c.Iva;
                this.txtNetoNoGrabado.Text = c.NetoNoGrabado.ToString();
                this.txtNeto2.Text = c.Neto2.ToString();
                this.txtNeto5.Text = c.Neto5.ToString();
                this.txtNeto105.Text = c.Neto105.ToString();
                this.txtNeto21.Text = c.Neto21.ToString();
                this.txtNeto27.Text = c.Neto27.ToString();
                this.txtIvaNeto2.Text = c.Iva2.ToString();
                this.txtIvaNeto5.Text = c.Iva5.ToString();
                this.txtIvaNeto105.Text = c.Iva105.ToString();
                this.txtIvaNeto21.Text = c.Iva21.ToString();
                this.txtIvaNeto27.Text = c.Iva27.ToString();
                this.txtPIB.Text = c.PIB.ToString();
                this.txtPIva.Text = c.PIva.ToString();
                this.txtImpuestosInternos.Text = c.ImpuestosInternos.ToString();
                this.txtOtros.Text = c.Otros.ToString();

                this.txtRetencionIIBB.Text = c.RetencionIIBB.ToString();
                this.txtRetencionIVA.Text = c.RetencionIVA.ToString();
                this.txtRetencionGanancias.Text = c.RetencionGanancia.ToString();
                this.txtRetencionSuss.Text = c.RetencionSuss.ToString();

                this.txtITC.Text = c.ITC.ToString();
                this.txtTasaCo2.Text = c.TasaCo2.ToString();

                this.txtVencimiento.Text = Convert.ToDateTime(c.Vencimiento, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                this.txtTotal.Text = c.Total.ToString();
                this.ListTipoCompra.SelectedValue = c.Tipo.ToString();
                this.txtImputacionCont.Text = Convert.ToDateTime(c.FechaImputacion, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");

                if (c.Compras_Observaciones.Count > 0)
                    this.txtObservaciones.Text = c.Compras_Observaciones.FirstOrDefault().Observacion;

                //this.btnAgregar.Visible = false;
                this.btnCancelar.Text = "Volver";
                this.BloquearControlesAlEditar();

                var cta = this.contPlanCta.obtenerCuentaContableCompra(c.Id);
                if (cta != null)
                {
                    this.txtPlanCtaProv.Text = cta.Cuentas_Contables.Codigo + " - " + cta.Cuentas_Contables.Descripcion;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error inicializando formulario. " + ex.Message));
            }
        }

        private void BloquearControlesAlEditar()
        {
            try
            {
                this.ListEmpresa.Attributes.Add("disabled", "true");
                this.ListSucursal.Attributes.Add("disabled", "true");
                this.ListPuntoVenta.Attributes.Add("disabled", "true");
                this.txtFecha.Attributes.Add("disabled", "true");
                this.ListTipoDocumento.Attributes.Add("disabled", "true");
                this.ListProveedor.Attributes.Add("disabled", "true");
                this.txtCuit.Attributes.Add("disabled", "true");
                this.txtObservaciones.Attributes.Add("disabled", "true");
                this.txtVencimiento.Attributes.Add("disabled", "true");
                this.txtTotal.Attributes.Add("disabled", "true");
                this.ListTipoCompra.Attributes.Add("disabled", "true");
                //this.txtImputacionCont.Attributes.Add("disabled", "true");
                this.btnAgregarPagar.Visible = false;
            }
            catch
            {

            }
        }

        private void cargarCompra()
        {
            try
            {
                Gestion_Api.Entitys.Compra c = this.contEntity.obtenerCompraId(this.idCompra);
                this.ListEmpresa.SelectedValue = c.IdEmpresa.ToString();
                this.cargarSucursal(c.IdEmpresa.Value);
                this.ListSucursal.SelectedValue = c.IdSucursal.ToString();
                this.cargarPuntoVta(c.IdSucursal.Value, (int)c.IdPuntoVenta);
                this.txtFecha.Text = Convert.ToDateTime(c.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                this.ListTipoDocumento.SelectedValue = c.TipoDocumento;
                this.txtPVenta.Text = c.PuntoVenta;
                this.txtNumero.Text = c.Numero;
                this.ListProveedor.SelectedValue = c.Proveedor.ToString();
                this.txtCuit.Text = c.Cuit;
                this.txtIva.Text = c.Iva;
                this.txtNetoNoGrabado.Text = c.NetoNoGrabado.ToString();
                this.txtNeto2.Text = c.Neto2.ToString();
                this.txtNeto5.Text = c.Neto5.ToString();
                this.txtNeto105.Text = c.Neto105.ToString();
                this.txtNeto21.Text = c.Neto21.ToString();
                this.txtNeto27.Text = c.Neto27.ToString();
                this.txtIvaNeto2.Text = c.Iva2.ToString();
                this.txtIvaNeto5.Text = c.Iva5.ToString();
                this.txtIvaNeto105.Text = c.Iva105.ToString();
                this.txtIvaNeto21.Text = c.Iva21.ToString();
                this.txtIvaNeto27.Text = c.Iva27.ToString();
                this.txtPIB.Text = c.PIB.ToString();
                this.txtPIva.Text = c.PIva.ToString();
                this.txtImpuestosInternos.Text = c.ImpuestosInternos.ToString();
                this.txtOtros.Text = c.Otros.ToString();

                this.txtRetencionIIBB.Text = c.RetencionIIBB.ToString();
                this.txtRetencionIVA.Text = c.RetencionIVA.ToString();
                this.txtRetencionGanancias.Text = c.RetencionGanancia.ToString();
                this.txtRetencionSuss.Text = c.RetencionSuss.ToString();

                this.txtITC.Text = c.ITC.ToString();
                this.txtTasaCo2.Text = c.TasaCo2.ToString();

                this.txtVencimiento.Text = Convert.ToDateTime(c.Vencimiento, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                this.txtTotal.Text = c.Total.ToString();
                this.ListTipoCompra.SelectedValue = c.Tipo.ToString();
                this.txtImputacionCont.Text = Convert.ToDateTime(c.FechaImputacion, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");

                if (c.Compras_Observaciones.Count > 0)
                    this.txtObservaciones.Text = c.Compras_Observaciones.FirstOrDefault().Observacion;

                //this.btnAgregar.Visible = false;
                this.btnCancelar.Text = "Volver";
                this.bloquearControles();

                var cta = this.contPlanCta.obtenerCuentaContableCompra(c.Id);
                if (cta != null)
                {
                    this.txtPlanCtaProv.Text = cta.Cuentas_Contables.Codigo + " - " + cta.Cuentas_Contables.Descripcion;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error inicializando formulario. " + ex.Message));
            }
        }

        private void cargarCuit()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                }
                this.txtCuit.Text = c.cuit;
                this.txtIva.Text = c.iva;

                this.verificarNroCompra();
                this.cargarDatosCuentaProveedor();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de proveedor. " + ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
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

                if (!listPermisos.Contains("181"))
                {
                    ListSucursal.Enabled = false;
                    ListSucursal.CssClass = "form-control";
                }

                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "31")
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
        private void bloquearControles()
        {
            try
            {
                this.ListEmpresa.Attributes.Add("disabled", "true");
                this.ListSucursal.Attributes.Add("disabled", "true");
                this.ListPuntoVenta.Attributes.Add("disabled", "true");
                this.txtFecha.Attributes.Add("disabled", "true");
                this.ListTipoDocumento.Attributes.Add("disabled", "true");
                this.txtPVenta.Attributes.Add("disabled", "true");
                this.txtNumero.Attributes.Add("disabled", "true");
                this.ListProveedor.Attributes.Add("disabled", "true");
                this.txtCuit.Attributes.Add("disabled", "true");
                this.txtIva.Attributes.Add("disabled", "true");
                this.txtNetoNoGrabado.Attributes.Add("disabled", "true");
                this.txtNeto2.Attributes.Add("disabled", "true");
                this.txtNeto5.Attributes.Add("disabled", "true");
                this.txtITC.Attributes.Add("disabled", "true");
                this.txtTasaCo2.Attributes.Add("disabled", "true");
                this.txtImputacionCont.Attributes.Add("disabled", "true");
                this.txtObservaciones.Attributes.Add("disabled", "true");
                this.txtNeto105.Attributes.Add("disabled", "true");
                this.txtNeto21.Attributes.Add("disabled", "true");
                this.txtNeto27.Attributes.Add("disabled", "true");
                this.txtIvaNeto105.Attributes.Add("disabled", "true");
                this.txtIvaNeto21.Attributes.Add("disabled", "true");
                this.txtIvaNeto27.Attributes.Add("disabled", "true");
                this.txtPIB.Attributes.Add("disabled", "true");
                this.txtPIva.Attributes.Add("disabled", "true");
                this.txtImpuestosInternos.Attributes.Add("disabled", "true");
                this.txtOtros.Attributes.Add("disabled", "true");
                this.txtVencimiento.Attributes.Add("disabled", "true");
                this.txtTotal.Attributes.Add("disabled", "true");
                this.ListTipoCompra.Attributes.Add("disabled", "true");
                this.txtRetencionIIBB.Attributes.Add("disabled", "true");
                this.txtRetencionIVA.Attributes.Add("disabled", "true");
                this.txtRetencionGanancias.Attributes.Add("disabled", "true");
                this.txtRetencionSuss.Attributes.Add("disabled", "true");

            }
            catch
            {

            }
        }
        private int verificarNroCompra()
        {
            try
            {
                List<Gestion_Api.Entitys.Compra> Compras = this.contEntity.buscarComprasProveedorDoc(Convert.ToInt32(this.ListProveedor.SelectedValue));
                Compras = Compras.Where(x => x.TipoDocumento == this.ListTipoDocumento.SelectedItem.Text && x.PuntoVenta == this.txtPVenta.Text && x.Numero == this.txtNumero.Text).ToList();

                if (Compras.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"El proveedor seleccionado ya contiene una compra con la misma numeracion y tipo de documento!.\");", true);
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error verificando nro Compra. " + ex.Message + ".\");", true);
                return -1;
            }
        }
        private void limpiarCampos()
        {
            try
            {
                this.ListEmpresa.SelectedIndex = 0;
                this.ListSucursal.SelectedIndex = 0;
                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.ListTipoDocumento.SelectedIndex = 0;
                this.txtNumero.Text = "";
                this.txtPVenta.Text = "";
                this.ListProveedor.SelectedIndex = 0;
                this.txtCuit.Text = "";
                this.txtNetoNoGrabado.Text = "0";
                this.txtNeto2.Text = "0";
                this.txtIvaNeto2.Text = "0";
                this.txtNeto5.Text = "0";
                this.txtIvaNeto5.Text = "0";
                this.txtNeto105.Text = "0";
                this.txtIvaNeto105.Text = "0";
                this.txtNeto21.Text = "0";
                this.txtIvaNeto21.Text = "0";
                this.txtNeto27.Text = "0";
                this.txtIvaNeto27.Text = "0";
                this.txtPIB.Text = "0";
                this.txtPIva.Text = "0";
                this.txtImpuestosInternos.Text = "0";
                this.txtOtros.Text = "0";
                this.txtVencimiento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.txtImputacionCont.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.txtTotal.Text = "0";
                this.txtITC.Text = "0";
                this.txtTasaCo2.Text = "0";
                this.txtRetencionSuss.Text = "0";

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Eventos Controles
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (this.accion == 1 || this.accion == 3 || this.accion == 4)
                this.agregarCompra(0);

            if (this.accion == 2)
                this.modificarCompra(0);

            if (this.accion == 5)
                this.GuardarCompraEditada();

        }
        #endregion

        #region ABM
        private void agregarCompra(int pagar)
        {
            try
            {
                //int pagar: 1 te tiene que redirigir a la pantalla de pagos, 0 no te tiene que redirigir


                //recalculo total
                this.calcularTotal();
                //verifico el numero del documento
                int j = this.verificarNroCompra();

                if (j > 0)
                {
                    Gestion_Api.Entitys.Compra c = new Gestion_Api.Entitys.Compra();
                    c = this.obtenerDatosCompra(c);

                    if (c != null)
                    {
                        int i = this.contEntity.agregarCompra(c);

                        if (i > 0)
                        {
                            if (accion == 3)
                            {
                                contEntity.AgregarCompra_Remito(c.Id, idRemito);

                                i = contEntity.GuardarCompra_Remito();

                                if (i < 0)
                                    Log.EscribirSQL(1, "Error", "Error al guardar compra_remito");

                            }

                            this.agregarPlanCtaCompra(c);

                            if (pagar == 1)
                            {
                                //Variable que me indica si la compra es en negro o en blanco
                                int tipoDocumento = 0;

                                //Verifico si el tipo de documento es presupuesto, si es asi le cambio el valor a la variable creada 
                                if (this.ListTipoDocumento.SelectedItem.Text.ToLower().Contains("prp") || this.ListTipoDocumento.SelectedItem.Text.ToLower().Contains("presupuesto"))
                                    tipoDocumento = 1;

                                Response.Redirect("../Pagos/PagosABM.aspx?bn=" + tipoDocumento + "&d=" + c.MovimientosCCPs.FirstOrDefault().Id.ToString() + "&p=" + this.ListProveedor.SelectedValue + "&e=" + this.ListEmpresa.SelectedValue + "&s=" + this.ListSucursal.SelectedValue + "&pv=" + this.ListPuntoVenta.SelectedValue + "&m=0&a=1");
                            }


                            this.limpiarCampos();

                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Compra Agregada con Exito\", {type: \"info\"});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se Pudo agregar compra\";", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error agregando compra. " + ex.Message + "\", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando compra. " + ex.Message));
            }
        }

        private void GuardarCompraEditada()
        {
            try
            {
                Gestion_Api.Entitys.Compra c = this.contEntity.obtenerCompraId(this.idCompra);

                if (c != null)
                {
                    calcularTotal();

                    c = this.obtenerDatosCompra(c);
                    int i = this.contEntity.EditarCompra(c);
                    if (i >= 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Compra modificada con exito\", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo modificar compra\";", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error agregando compra. " + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        private void AgregarObservacionACompra(Gestion_Api.Entitys.Compra c)
        {
            if (c.Compras_Observaciones.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtObservaciones.Text))
                    c.Compras_Observaciones.FirstOrDefault().Observacion = this.txtObservaciones.Text;
            }
            else
            {
                Gestion_Api.Entitys.Compras_Observaciones obs = new Gestion_Api.Entitys.Compras_Observaciones();
                obs.Observacion = this.txtObservaciones.Text;
                c.Compras_Observaciones.Add(obs);
            }
        }

        private void modificarCompra(int pagar)
        {
            try
            {
                //int pagar: 1 te tiene que redirigir a la pantalla de pagos, 0 no te tiene que redirigir

                Gestion_Api.Entitys.Compra c = this.contEntity.obtenerCompraId(this.idCompra);
                c.FechaImputacion = Convert.ToDateTime(this.txtImputacionCont.Text, new CultureInfo("es-AR"));

                AgregarObservacionACompra(c);

                if (c != null)
                {
                    int i = this.contEntity.modificarCompra(c);
                    if (i >= 0)
                    {
                        if (pagar == 1)
                        {
                            //Variable que me indica si la compra es en negro o en blanco
                            int tipoDocumento = 0;

                            //Verifico si el tipo de documento es presupuesto, si es asi le cambio el valor a la variable creada 
                            if (this.ListTipoDocumento.SelectedItem.Text.ToLower().Contains("prp") || this.ListTipoDocumento.SelectedItem.Text.ToLower().Contains("presupuesto"))
                                tipoDocumento = 1;

                            Response.Redirect("../Pagos/PagosABM.aspx?bn=" + tipoDocumento + "&d=" + c.MovimientosCCPs.FirstOrDefault().Id.ToString() + "&p=" + this.ListProveedor.SelectedValue + "&e=" + this.ListEmpresa.SelectedValue + "&s=" + this.ListSucursal.SelectedValue + "&pv=" + c.IdPuntoVenta + "&m=0&a=1");
                        }

                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Compra modificada con exito\", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo modificar compra\";", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error agregando compra. " + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        private Gestion_Api.Entitys.Compra obtenerDatosCompra(Gestion_Api.Entitys.Compra c)
        {
            try
            {
                c.IdEmpresa = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                c.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                c.IdPuntoVenta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);
                c.Fecha = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                c.TipoDocumento = this.ListTipoDocumento.SelectedValue;
                c.PuntoVenta = this.txtPVenta.Text.Trim();
                c.Numero = this.txtNumero.Text.Trim();
                c.Proveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                c.Cuit = this.txtCuit.Text;
                c.Iva = this.txtIva.Text;
                c.NetoNoGrabado = Convert.ToDecimal(this.txtNetoNoGrabado.Text);
                c.Neto2 = Convert.ToDecimal(this.txtNeto2.Text);
                c.Iva2 = Convert.ToDecimal(this.txtIvaNeto2.Text);
                c.Neto5 = Convert.ToDecimal(this.txtNeto5.Text);
                c.Iva5 = Convert.ToDecimal(this.txtIvaNeto5.Text);
                c.Neto105 = Convert.ToDecimal(this.txtNeto105.Text);
                c.Iva105 = Convert.ToDecimal(this.txtIvaNeto105.Text);
                c.Neto21 = Convert.ToDecimal(this.txtNeto21.Text);
                c.Iva21 = Convert.ToDecimal(this.txtIvaNeto21.Text);
                c.Neto27 = Convert.ToDecimal(this.txtNeto27.Text);
                c.Iva27 = Convert.ToDecimal(this.txtIvaNeto27.Text);
                c.PIB = Convert.ToDecimal(this.txtPIB.Text);
                c.PIva = Convert.ToDecimal(this.txtPIva.Text);
                c.ImpuestosInternos = Convert.ToDecimal(this.txtImpuestosInternos.Text);
                c.Otros = Convert.ToDecimal(this.txtOtros.Text);

                c.RetencionIIBB = Decimal.Round(Convert.ToDecimal(this.txtRetencionIIBB.Text), 2);
                c.RetencionIVA = Decimal.Round(Convert.ToDecimal(this.txtRetencionIVA.Text), 2);
                c.RetencionGanancia = Decimal.Round(Convert.ToDecimal(this.txtRetencionGanancias.Text), 2);
                c.RetencionSuss = Decimal.Round(Convert.ToDecimal(this.txtRetencionSuss.Text), 2);

                c.ITC = Decimal.Round(Convert.ToDecimal(this.txtITC.Text), 2);
                c.TasaCo2 = Decimal.Round(Convert.ToDecimal(this.txtTasaCo2.Text), 2);

                c.Total = Convert.ToDecimal(this.txtTotal.Text);
                c.Vencimiento = Convert.ToDateTime(this.txtVencimiento.Text, new CultureInfo("es-AR"));
                c.FechaImputacion = Convert.ToDateTime(this.txtImputacionCont.Text, new CultureInfo("es-AR"));
                c.Tipo = Convert.ToInt32(this.ListTipoCompra.SelectedValue);

                AgregarObservacionACompra(c);

                //check de imputa
                c.ImputaCC = 1;
                if (this.checkImputaCC.Checked)
                {
                    c.ImputaCC = 0;
                }

                //Verifico si es blanco o negro
                c.Ftp = 0;
                if (this.ListTipoDocumento.SelectedValue.ToLower().Contains("prp") || this.ListTipoDocumento.SelectedValue.ToLower().Contains("presupuesto"))
                    c.Ftp = 1;

                return c;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error obteniendo datos dela compra. " + ex.Message + "\", {type: \"error\"});", true);

                return null;
            }
        }
        #endregion

        #region calculos
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSucursal(Convert.ToInt32(this.ListEmpresa.SelectedValue));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en select index changes en list Empresa. " + ex.Message));
            }
        }

        protected void ListProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCuit();
        }

        protected void txtNeto2_TextChanged(object sender, EventArgs e)
        {
            //calculo el neto del iva en 2
            try
            {
                decimal iva = Decimal.Round(Convert.ToDecimal(this.txtNeto2.Text), 2) * (decimal)0.025;
                this.txtIvaNeto2.Text = Decimal.Round(iva, 2).ToString();

                //recalculo el total
                calcularTotal();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Iva 2.5. " + ex.Message));
            }
        }

        protected void txtNeto5_TextChanged(object sender, EventArgs e)
        {
            //calculo el neto del iva en 5
            try
            {
                decimal iva = Decimal.Round(Convert.ToDecimal(this.txtNeto5.Text), 2) * (decimal)0.05;
                this.txtIvaNeto5.Text = Decimal.Round(iva, 2).ToString();

                //recalculo el total
                calcularTotal();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Iva 5. " + ex.Message));
            }
        }

        protected void txtNeto105_TextChanged(object sender, EventArgs e)
        {
            //calculo el neto del iva en 10.5
            try
            {
                decimal iva = Decimal.Round(Convert.ToDecimal(this.txtNeto105.Text), 2) * (decimal)0.105;
                this.txtIvaNeto105.Text = Decimal.Round(iva, 2).ToString();

                //recalculo el total
                calcularTotal();
                this.txtNeto21.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Iva 10.5. " + ex.Message));
            }
        }

        protected void txtNeto21_TextChanged(object sender, EventArgs e)
        {
            //calculo el neto del iva en 21
            try
            {
                decimal iva = Decimal.Round(Convert.ToDecimal(this.txtNeto21.Text), 2) * (decimal)0.21;
                this.txtIvaNeto21.Text = Decimal.Round(iva, 2).ToString();

                //recalculo el total
                calcularTotal();
                this.txtNeto27.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Iva 21. " + ex.Message));
            }
        }

        protected void txtIvaNeto27_TextChanged(object sender, EventArgs e)
        {
            //calculo el neto del iva en 10.5
            try
            {
                decimal iva = Decimal.Round(Convert.ToDecimal(this.txtNeto27.Text), 2) * (decimal)0.27;
                this.txtIvaNeto27.Text = Decimal.Round(iva, 2).ToString();

                //recalculo el total
                calcularTotal();

                this.txtPIB.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Iva 22. " + ex.Message));
            }
        }

        private void calcularTotal()
        {
            try
            {
                decimal neto = Decimal.Round(Convert.ToDecimal(this.txtNetoNoGrabado.Text), 2);
                decimal neto2 = Decimal.Round(Convert.ToDecimal(this.txtNeto2.Text), 2);
                decimal neto5 = Decimal.Round(Convert.ToDecimal(this.txtNeto5.Text), 2);
                decimal neto105 = Decimal.Round(Convert.ToDecimal(this.txtNeto105.Text), 2);
                decimal neto21 = Decimal.Round(Convert.ToDecimal(this.txtNeto21.Text), 2);
                decimal neto27 = Decimal.Round(Convert.ToDecimal(this.txtNeto27.Text), 2);
                decimal Iva2 = Decimal.Round(Convert.ToDecimal(this.txtIvaNeto2.Text), 2);
                decimal Iva5 = Decimal.Round(Convert.ToDecimal(this.txtIvaNeto5.Text), 2);
                decimal Iva105 = Decimal.Round(Convert.ToDecimal(this.txtIvaNeto105.Text), 2);
                decimal Iva21 = Decimal.Round(Convert.ToDecimal(this.txtIvaNeto21.Text), 2);
                decimal Iva27 = Decimal.Round(Convert.ToDecimal(this.txtIvaNeto27.Text), 2);
                decimal percepcionIIBB = Decimal.Round(Convert.ToDecimal(this.txtPIB.Text), 2);
                decimal percepcionIva = Decimal.Round(Convert.ToDecimal(this.txtPIva.Text), 2);
                decimal impInternos = Decimal.Round(Convert.ToDecimal(this.txtImpuestosInternos.Text), 2);
                decimal otros = Decimal.Round(Convert.ToDecimal(this.txtOtros.Text), 2);
                decimal RetencionIIBB = Decimal.Round(Convert.ToDecimal(this.txtRetencionIIBB.Text), 2);
                decimal RetencionIva = Decimal.Round(Convert.ToDecimal(this.txtRetencionIVA.Text), 2);
                decimal RetencionGanancia = Decimal.Round(Convert.ToDecimal(this.txtRetencionGanancias.Text), 2);
                decimal ITC = Decimal.Round(Convert.ToDecimal(this.txtITC.Text), 2);
                decimal tasaCo2 = Decimal.Round(Convert.ToDecimal(this.txtTasaCo2.Text), 2);
                decimal RetencionSuss = Decimal.Round(Convert.ToDecimal(this.txtRetencionSuss.Text), 2);

                decimal total = neto + neto2 + neto5 + neto105 + neto21 + neto27 + Iva2 + Iva5 + Iva105 + Iva21 + Iva27 + percepcionIva + impInternos + otros + percepcionIIBB + RetencionIIBB + RetencionIva + RetencionGanancia + ITC + tasaCo2 + RetencionSuss;
                this.txtTotal.Text = Decimal.Round(total, 2).ToString();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error calculando Total. " + ex.Message + "\", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando Total. " + ex.Message));
            }
        }

        protected void txtNetoNoGrabado_TextChanged(object sender, EventArgs e)
        {
            //recalculo el total
            calcularTotal();
            this.txtNeto105.Focus();
        }

        protected void txtOtros_TextChanged(object sender, EventArgs e)
        {
            //recalculo el total
            calcularTotal();
            this.txtVencimiento.Focus();
        }

        protected void txtPIva_TextChanged(object sender, EventArgs e)
        {
            //recalculo el total
            calcularTotal();
            this.txtImpuestosInternos.Focus();
        }

        protected void txtPIB_TextChanged(object sender, EventArgs e)
        {
            //recalculo el total
            calcularTotal();
            this.txtPIva.Focus();
        }

        protected void txtImpuestosInternos_TextChanged(object sender, EventArgs e)
        {
            //recalculo el total
            calcularTotal();
            this.txtOtros.Focus();
        }

        protected void txtRetencionIIBB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtRetencionIIBB.Focus();
            }
            catch
            {

            }
        }

        protected void txtRetencionIVA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtRetencionIVA.Focus();
            }
            catch
            {

            }
        }

        protected void txtRetencionGanancias_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtRetencionGanancias.Focus();
            }
            catch
            {

            }
        }

        protected void txtRetencionSuss_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtRetencionSuss.Focus();
            }
            catch
            {

            }
        }

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtImputacionCont.Text = this.txtFecha.Text;
            }
            catch { }
        }

        protected void txtTasaCo2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtTasaCo2.Focus();
            }
            catch
            {

            }
        }

        protected void txtITC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //recalculo el total
                calcularTotal();
                this.txtITC.Focus();
            }
            catch
            {

            }
        }
        #endregion

        #region plan cuentas

        private void agregarPlanCtaCompra(Gestion_Api.Entitys.Compra c)
        {
            try
            {
                int idCta = Convert.ToInt32(this.ListCtaContables.SelectedValue);
                if (idCta > 0)
                {
                    int ok = 0;
                    var cta = this.contPlanCta.obtenerCuentaContableCompra(c.Id);
                    if (cta != null)
                    {
                        cta.IdCuentaContable = idCta;
                        ok = this.contPlanCta.modificarCuentaContableCompra(cta);
                    }
                    else
                    {
                        ok = this.contPlanCta.agregarCuentaContableCompra(c.Id, idCta);
                    }

                }
            }
            catch
            {

            }
        }
        private void cargarDatosCuentaProveedor()
        {
            try
            {
                int idProv = Convert.ToInt32(this.ListProveedor.SelectedValue);
                var cta = this.contPlanCta.obtenerCuentaContableProveedor(idProv);
                if (cta != null)
                {
                    this.cargarCuentasNivel1();
                    this.ListCtaContables1.SelectedValue = cta.Cuentas_Contables.Nivel1.ToString();
                    this.cargarCuentasNivel2();
                    this.ListCtaContables2.SelectedValue = cta.Cuentas_Contables.Nivel2.ToString();
                    this.cargarCuentasNivel3();
                    this.ListCtaContables3.SelectedValue = cta.Cuentas_Contables.Nivel3.ToString();
                    this.cargarCuentasNivel4();
                    this.ListCtaContables.SelectedValue = cta.IdCuentaContable.ToString();
                    this.txtPlanCtaProv.Text = cta.Cuentas_Contables.Codigo + " - " + cta.Cuentas_Contables.Descripcion;
                }
            }
            catch
            {

            }
        }
        private void cargarCuentas()
        {
            try
            {
                List<Cuentas_Contables> cuentas = this.contPlanCta.obtenerCuentasContables();
                if (cuentas.Count > 0)
                {
                    this.cargarCuentasNivel1();
                    this.PanelCtaCtble.Visible = true;
                }
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel1()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(1, 0);

                this.ListCtaContables1.DataSource = ctas.ToList();
                this.ListCtaContables1.DataValueField = "Id";
                this.ListCtaContables1.DataTextField = "Descripcion";
                this.ListCtaContables1.DataBind();

                this.ListCtaContables1.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel2()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(2, Convert.ToInt32(this.ListCtaContables1.SelectedValue));

                this.ListCtaContables2.DataSource = ctas.ToList();
                this.ListCtaContables2.DataValueField = "Id";
                this.ListCtaContables2.DataTextField = "Descripcion";
                this.ListCtaContables2.DataBind();

                this.ListCtaContables2.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel3()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(3, Convert.ToInt32(this.ListCtaContables2.SelectedValue));

                this.ListCtaContables3.DataSource = ctas.ToList();
                this.ListCtaContables3.DataValueField = "Id";
                this.ListCtaContables3.DataTextField = "Descripcion";
                this.ListCtaContables3.DataBind();
                this.ListCtaContables3.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel4()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(4, Convert.ToInt32(this.ListCtaContables3.SelectedValue));

                this.ListCtaContables.DataSource = ctas.ToList();
                this.ListCtaContables.DataValueField = "Id";
                this.ListCtaContables.DataTextField = "Descripcion";
                this.ListCtaContables.DataBind();
                this.ListCtaContables.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        protected void lbtnAgregarMovCtaCbe_Click(object sender, EventArgs e)
        {
            try
            {
                int idCta = Convert.ToInt32(this.ListCtaContables.SelectedValue);
                int idProv = Convert.ToInt32(this.ListProveedor.SelectedValue);
                if (idCta > 0)
                {
                    var cta = this.contPlanCta.obtenerCuentaById(idCta);
                    if (cta != null)
                    {
                        this.txtPlanCtaProv.Text = cta.Codigo + " - " + cta.Descripcion;
                        this.lblIdCtaContable.Text = cta.Id.ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", " cerrarModal2(); ", true);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void ListCtaContables1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel2();
            }
            catch
            {

            }
        }
        protected void ListCtaContables2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel3();
            }
            catch
            {

            }
        }
        protected void ListCtaContables3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel4();
            }
            catch
            {

            }
        }

        #endregion

        protected void btnAgregarPagar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 1 || this.accion == 3)
                    this.agregarCompra(1);

                if (this.accion == 2)
                    this.modificarCompra(1);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error enviando a procesar compra. Excepción: " + Ex.Message + "\", {type: \"error\"});", true);
            }
        }

        public void CargarCompraDesdeRemito()
        {
            try
            {
                var remito = contEntity.obtenerRemito(idRemito);

                this.ListEmpresa.SelectedValue = Session["Login_EmpUser"].ToString();
                this.cargarSucursal(Convert.ToInt32(Session["Login_EmpUser"]));
                this.ListSucursal.SelectedValue = remito.IdSucursal.ToString();
                this.cargarPuntoVta((int)remito.IdSucursal);
                this.txtFecha.Text = Convert.ToDateTime(remito.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                this.ListProveedor.SelectedValue = remito.IdProveedor.ToString();
                this.cargarCuit();
                this.ListTipoCompra.SelectedValue = 2.ToString(); //compra mercaderia
                this.txtObservaciones.Text = "Compra generada desde el remito numero " + remito.Numero;
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error al cargar compra desde remito. Excepción: " + Ex.Message + "\", {type: \"error\"});", true);
            }
        }
    }
}