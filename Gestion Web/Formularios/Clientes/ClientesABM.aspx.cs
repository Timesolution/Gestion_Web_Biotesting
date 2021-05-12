using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Millas_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Task_Api;
using Task_Api.Entitys;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ClientesABM : System.Web.UI.Page
    {
        // verifico si es de uruguay el cuit.
        int esUruguay = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsUruguay"));
        int esCCW = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsCCW"));
        //mensajes popUp
        Mensajes m = new Mensajes();
        //controladores
        private controladorCliente controlador = new controladorCliente();
        private ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        private controladorTipoCliente controladorTipoCliente = new controladorTipoCliente();
        private controladorGrupoCliente controladorGrupoCliente = new controladorGrupoCliente();
        private controladorCategoria controladorCatCliente = new controladorCategoria();
        private controladorEstadoCliente controladorEstCliente = new controladorEstadoCliente();
        private controladorVendedor contVendedor = new controladorVendedor();
        private controladorUsuario contUser = new controladorUsuario();
        private ControladorExpreso contExpreso = new ControladorExpreso();
        private controladorZona contZona = new controladorZona();
        ControladorPlanCuentas contPlanCta = new ControladorPlanCuentas();
        controladorFunciones controladorFunciones = new controladorFunciones();
        controladorUsuario controladorUsuario = new controladorUsuario();
        controladorCobranza controladorCobranza = new controladorCobranza();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        //private string cuit;
        private int idCliente;
        //cliente del formulario
        private cliente cl;
        private string codigo;
        private string cuit;
        private int EditarDir;
        private int EditarCon;
        private int PosDir;
        private int PosCon;
        private int crm;
        private int totalPuntos = 0;

        public class IIBBTemporal
        {
            public string Id { get; set; }
            public string Provincia { get; set; }
            public string Percepcion { get; set; }
            public string Retencion { get; set; }
            public string Modo { get; set; }
            public string IdCliente { get; set; }
            public string PlanCuentas { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (esUruguay == 1)
                {
                    lbCUITDNI.InnerText = "RUT";
                }

                //if (chbDisparaTarea.Checked == true)
                //{
                //    divVencimientoTarea.Visible = true;
                //    divTarea.Visible = true;
                //    divSituacion.Visible = true;
                //}
                //else
                //{
                //    divVencimientoTarea.Visible = false;
                //    divTarea.Visible = false;
                //    divSituacion.Visible = false;
                //}

                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.idCliente = Convert.ToInt32(Request.QueryString["id"]);
                this.crm = Convert.ToInt32(Request.QueryString["crm"]);
                hiddenIdCliente.Value = idCliente.ToString();
                string perfil = Session["Login_NombrePerfil"] as string;

                this.verificarPermisos();

                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

                if (!IsPostBack)
                {

                    drpCRMSituacion.SelectedValue = "1";
                    txtFechaEvento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaVencimiento.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    cl = contClienteEntity.ObtenerClienteId(idCliente);
                    if (cl != null)
                    {
                        hiddenOrigenCliente.Value = cl.origen.ToString();
                    }
                    else
                    {
                        switch (accion)
                        {
                            case 1:
                                hiddenOrigenCliente.Value = "1";
                                break;
                            case 3:
                                hiddenOrigenCliente.Value = "2";
                                break;
                            default:
                                break;
                        }
                    }
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    //Confirguraciones
                    this.LitCliente_1.Text = WebConfigurationManager.AppSettings.Get("Clientes_1");

                    //Cliente c = new Cliente();
                    //Session.Add("ClientesABM_Cliente",c);
                    Session["ClientesABM_Cliente"] = null;

                    this.EditarCon = 0;
                    this.EditarDir = 0;
                    Session.Add("ClientesABM_EditarCon", EditarCon);
                    Session.Add("ClientesABM_EditarDir", EditarDir);

                    this.PosCon = 0;
                    this.PosDir = 0;
                    Session.Add("ClientesABM_PosCon", PosCon);
                    Session.Add("ClientesABM_PosDir", PosDir);

                    this.cargarTipoClientes();
                    this.cargarGrupoClientes();
                    //this.cargarCategoriaClientes();
                    this.cargarEstadoClientes();
                    this.cargarIvaClientes();
                    this.cargarPaises();
                    this.cargarListasPrecios();
                    this.cargarVendedores();
                    this.cargarFormaPAgo();
                    this.cargarFormasVenta();
                    this.cargarProvincias();
                    this.cargarSucursales();
                    this.cargarDescuentoCliente();
                    this.cargarExpresos();
                    this.cargarEmpleados();
                    this.cargarEntregas();
                    this.cargarZonas();
                    this.cargarClientesReferir();
                    this.cargarBTB();
                    this.cargarEstadosFiltro();
                    this.cargarPlanCuentas();
                    //this.cargarTipoContacto();

                    this.asignarNombreLabel(accion);

                    //modifico
                    if (this.accion == 2)
                    {
                        this.cargarCliente(this.idCliente);
                        //habilito la carga de sucursales
                        this.UpdatePanelSucursales.Visible = true;
                        if (perfil == "SuperAdministrador")
                        {
                            this.linkSucursales.Visible = true;
                        }
                        this.linkExpreso.Visible = true;
                        this.linkEntregas.Visible = true;
                        this.linkEventos.Visible = true;
                        this.linkFamilia.Visible = true;
                        this.linkCodigoBTB.Visible = true;
                        //this.linkEmpleado.Visible = true;
                        //this.linkExportacion.Visible = true;//lo muestro solo si el cliente es iva del exteriros
                    }
                    if (this.accion == 4)
                    {
                        this.linkGanancias.Visible = true;
                        this.linkOrdenesCompra.Visible = true;
                        this.linkCodigoBTB.Visible = true;
                        this.phCodigoBTB2.Visible = true;
                        this.cargarCuentas();
                        this.cargarGanancias();
                        this.cargarProveedor_OC();
                        this.cargarDatosCuentaProveedor();
                        this.cargarProveedor(this.idCliente);
                    }
                    //si es nuevo genero codigo
                    if (this.accion == 1 || this.accion == 3)
                    {
                        this.generarCodigo();
                        this.txtFechaAlta.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    }
                    this.verificarPermisoCtaCte();
                }
                //solo cuando es abm cliente origen = 1
                if (this.accion == 1 || this.accion == 2)
                {
                    this.LabelFormasVenta.Visible = true;
                    this.ListFormasVenta.Visible = true;
                    this.LabelDescuentoPorCantidad.Visible = true;
                    this.ListDescuentoPorCantidad.Visible = true;
                    this.verificarConfiguracionIva();
                    if (this.accion == 2)
                    {
                        this.cargarEventosCliente();
                    }
                }
                if (Session["ClientesABM_Cliente"] != null)
                {
                    this.cargarTablaDireccion();
                    this.cargarTablaContacto();
                    this.cargarTablaPuntos();
                }
                TabName.Value = Request.Form[TabName.UniqueID];
                if (crm == 1)
                {
                    //liEvento.Add("class","active");
                }
                else
                {
                    //liEvento.Attributes.Add("class", "active");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formulario. " + ex.Message));
            }
        }

        private void cargarPlanCuentas()
        {
            try
            {
                ControladorPlanCuentas controladorPlan = new ControladorPlanCuentas();
                this.ListPlanCuentas.DataSource = controladorPlan.obtenerCuentasContablesByJerarquia(5);
                this.ListPlanCuentas.DataValueField = "id";
                this.ListPlanCuentas.DataTextField = "descripcion";

                this.ListPlanCuentas.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        #region Verificaciones
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Clientes.Clientes") != 1)
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
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil.ToLower() == "lider")
                {
                    return 0;
                }
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "20" || s == "9")
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
        private void verificarConfiguracionIva()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.siemprePRP == "1")
                {
                    this.DropListIva.SelectedValue = "13";
                    this.DropListIva.Attributes.Add("disabled", "true");
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void verificarPermisoCtaCte()
        {
            try
            {
                //verifico si puede
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permiso2 = listPermisos.Where(x => x == "103").FirstOrDefault();
                if (permiso2 == null)
                {
                    if (this.accion == 1 || this.accion == 3)
                    {
                        try
                        {
                            this.DropListFormaPago.Items.Remove(this.DropListFormaPago.Items.FindByText("Cuenta Corriente"));
                        }
                        catch { }
                    }
                    else
                    {
                        if (this.DropListFormaPago.SelectedItem.Text != "Cuenta Corriente")
                        {
                            try
                            {
                                this.DropListFormaPago.Items.Remove(this.DropListFormaPago.Items.FindByText("Cuenta Corriente"));
                            }
                            catch { }
                        }
                    }
                }

            }
            catch
            {

            }
        }
        private void verificarPermisos()
        {
            try
            {
                this.verificarPermisoModificarEstadoCliente();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en verificarPermisoModificarEstadoCliente. ex: " + ex.Message));
            }
        }
        private void verificarPermisoModificarEstadoCliente()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permiso = listPermisos.Where(x => x == "170").FirstOrDefault();
                if (permiso == null)
                {
                    DropListEstado.Enabled = false;
                    DropListEstado.CssClass = "form-control";
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando verificarPermisoModificarEstadoCliente. Ex: " + ex.Message));
            }
        }
        private void asignarNombreLabel(int accion)
        {
            try
            {
                if (accion == 1 || accion == 2)
                {
                    labelNombre.Text = "Cliente";
                }
                if (accion == 3 || accion == 4)
                {
                    labelNombre.Text = "Proveedor";
                    this.panel1.Visible = false;
                }
                if (accion == 2)
                {
                    Cliente c = this.controlador.obtenerClienteID(this.idCliente);
                    if (c != null)
                    {
                        labelNombreCliente.Text = c.razonSocial;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
        }
        #endregion

        #region carga datos
        public void cargarFormaPAgo()
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                DataTable dt = contFact.obtenerFormasPago();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["forma"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListFormaPago.DataSource = dt;
                this.DropListFormaPago.DataValueField = "id";
                this.DropListFormaPago.DataTextField = "forma";

                this.DropListFormaPago.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }

        public void cargarEstadosFiltro()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                var estados = controladorClienteEntity.ObtenerEstadosEventoCliente();

                //estados.Insert(0, new Gestion_Api.Entitys.Estados_Clientes_Eventos
                //{
                //    Id = 0,
                //    descripcion = "seleccione"
                //});
                //agrego todos
                this.drpCRMSituacion.DataSource = estados;
                this.drpCRMSituacion.DataValueField = "Id";
                this.drpCRMSituacion.DataTextField = "descripcion";
                this.drpCRMSituacion.DataBind();

                //drpCRMSituacion.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados filtros. " + ex.Message));
            }
        }

        public void cargarFormasVenta()
        {
            try
            {
                controladorFactEntity contFcEnt = new controladorFactEntity();
                List<Gestion_Api.Entitys.Formas_Venta> formas = contFcEnt.obtenerFormasVenta();
                formas = formas.OrderBy(x => x.Nombre).ToList();

                this.ListFormasVenta.DataSource = formas;
                this.ListFormasVenta.DataValueField = "Id";
                this.ListFormasVenta.DataTextField = "Nombre";
                this.ListFormasVenta.DataBind();

                this.ListFormasVenta.Items.Insert(0, new ListItem("Ninguna", "-1"));
            }
            catch
            {

            }
        }
        private void cargarExpresos()
        {
            try
            {
                List<expreso> listaExpresos = contExpreso.obtenerExpresos();
                listaExpresos.Insert(0, (new expreso { id = -1, nombre = "Seleccione..." }));

                this.ddlExpresos.DataSource = listaExpresos;
                this.ddlExpresos.DataValueField = "id";
                this.ddlExpresos.DataTextField = "nombre";
                this.ddlExpresos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista expresos. " + ex.Message));
            }
        }

        private void cargarEmpleados()
        {
            try
            {
                controladorEmpleado contEmpleado = new controladorEmpleado();
                List<Gestion_Api.Modelo.Empleado> empleados = contEmpleado.obtenerEmpleadosReduc();

                foreach (Gestion_Api.Modelo.Empleado emp in empleados.OrderBy(x => x.nombre).OrderBy(y => y.apellido))
                {
                    this.listEmpleados.Items.Add(new ListItem
                    {
                        Text = emp.nombre + " " + emp.apellido,
                        Value = emp.id.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista empleados. " + ex.Message));
            }
        }

        private void cargarEntregas()
        {
            try
            {
                List<TiposEntrega> listaEntregas = contExpreso.obtenerTiposEntrega();
                listaEntregas.Insert(0, (new TiposEntrega { Id = -1, Descripcion = "Seleccione..." }));

                this.ListTipoEntrega.DataSource = listaEntregas;
                this.ListTipoEntrega.DataValueField = "Id";
                this.ListTipoEntrega.DataTextField = "Descripcion";

                this.ListTipoEntrega.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista entregas. " + ex.Message));
            }
        }

        private void cargarZonas()
        {
            try
            {
                List<Zona> listaZonas = contZona.obtenerZona();
                listaZonas.Insert(0, (new Zona { id = -1, nombre = "Seleccione..." }));

                this.ListZonaEntrega.DataSource = listaZonas;
                this.ListZonaEntrega.DataValueField = "id";
                this.ListZonaEntrega.DataTextField = "nombre";

                this.ListZonaEntrega.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista zonas. " + ex.Message));
            }
        }

        private void cargarTipoClientes()
        {
            try
            {
                controladorTipoCliente contTipoCliente = new controladorTipoCliente();
                this.DropListTipo.DataSource = contTipoCliente.obtenerTiposClientes();
                this.DropListTipo.DataValueField = "id";
                this.DropListTipo.DataTextField = "tipo";

                this.DropListTipo.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista tipo cliente. " + ex.Message));
            }
        }

        public void cargarListasPrecios()
        {
            try
            {
                ControladorFormasPago contForma = new ControladorFormasPago();

                if (this.accion == 2 || this.accion == 4)
                {
                    Cliente cl = this.controlador.obtenerClienteID(idCliente);
                    if (cl != null)
                    {
                        List<Gestion_Api.Entitys.listasPrecio> listas = contForma.obtenerListasByFormaPago(cl.formaPago.id);
                        if (listas != null && listas.Count > 0)
                        {
                            this.ListListaPrecios.DataSource = listas;
                            this.ListListaPrecios.DataValueField = "id";
                            this.ListListaPrecios.DataTextField = "nombre";
                            this.ListListaPrecios.DataBind();
                        }
                        else
                        {
                            DataTable dt = this.controlador.obtenerListaPrecios();

                            //agrego todos
                            DataRow dr = dt.NewRow();
                            dr["nombre"] = "Seleccione...";
                            dr["id"] = -1;
                            dt.Rows.InsertAt(dr, 0);

                            this.ListListaPrecios.DataSource = dt;
                            this.ListListaPrecios.DataValueField = "id";
                            this.ListListaPrecios.DataTextField = "nombre";

                            this.ListListaPrecios.DataBind();
                        }
                    }
                }
                else
                {
                    DataTable dt = this.controlador.obtenerListaPrecios();

                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["nombre"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListListaPrecios.DataSource = dt;
                    this.ListListaPrecios.DataValueField = "id";
                    this.ListListaPrecios.DataTextField = "nombre";

                    this.ListListaPrecios.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        public void cargarListasPreciosForma(int idForma)
        {
            try
            {
                ControladorFormasPago contForma = new ControladorFormasPago();

                List<Gestion_Api.Entitys.listasPrecio> listas = contForma.obtenerListasByFormaPago(idForma);
                if (listas != null && listas.Count > 0)
                {
                    this.ListListaPrecios.DataSource = listas;
                    this.ListListaPrecios.DataValueField = "id";
                    this.ListListaPrecios.DataTextField = "nombre";
                    this.ListListaPrecios.DataBind();
                }
                else
                {
                    DataTable dt = this.controlador.obtenerListaPrecios();

                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["nombre"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.ListListaPrecios.DataSource = dt;
                    this.ListListaPrecios.DataValueField = "id";
                    this.ListListaPrecios.DataTextField = "nombre";

                    this.ListListaPrecios.DataBind();
                }
            }
            catch
            {

            }
        }

        private void cargarGrupoClientes()
        {
            try
            {
                controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();
                this.DropListGrupo.DataSource = contGrupoCliente.obtenerGruposClientes();
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        public void cargarVendedores()
        {
            try
            {
                int suc = (int)Session["Login_SucUser"];
                ListVendedores.Items.Clear();
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = new DataTable();
                if (this.accion == 1 || this.accion == 3)
                {
                    dt = contVendedor.obtenerVendedoresBySuc(suc);
                }
                else
                {
                    dt = contVendedor.obtenerVendedores();
                }


                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedores.Items.Add(item);
                }



                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }

        private void cargarCategoriaClientes()
        {
            //try
            //{
            //    controladorCategoria contCatCliente = new controladorCategoria();
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista categoria cliente. " + ex.Message));
            //}
        }

        private void cargarEstadoClientes()
        {
            try
            {
                controladorEstadoCliente contEstCliente = new controladorEstadoCliente();
                this.DropListEstado.DataSource = contEstCliente.obtenerEstadosClientes();
                this.DropListEstado.DataValueField = "id";
                this.DropListEstado.DataTextField = "estadoCliente";

                this.DropListEstado.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista estado cliente. " + ex.Message));
            }
        }

        private void cargarPaises()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.DropListPais.DataSource = controladorPais.obtenerPaisesClientes();
                this.DropListPais.DataValueField = "id";
                this.DropListPais.DataTextField = "pais";

                this.DropListPais.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  paises. " + ex.Message));
            }
        }

        private void cargarProvincias()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListProvincia.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvincia.DataValueField = "Provincia";
                this.ListProvincia.DataTextField = "Provincia";
                this.ListProvincia.DataBind();
                this.ListProvincia.Items.Insert(0, new ListItem("Seleccione...", "-1"));


                ControladorProvincias ContProvincia = new ControladorProvincias();
                this.IngresosBrutos_DropList_Provincias.DataSource = ContProvincia.ObtenerProvinciaSinBuenosAires();
                this.IngresosBrutos_DropList_Provincias.DataValueField = "Id";
                this.IngresosBrutos_DropList_Provincias.DataTextField = "Provincia1";
                this.IngresosBrutos_DropList_Provincias.DataBind();
                //this.IngresosBrutos_DropList_Provincias.Items.RemoveAt(0);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  provincias. " + ex.Message));
            }
        }

        private void cargarLocalidades(string provincia)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListLocalidad.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidad.DataValueField = "Localidad";
                this.ListLocalidad.DataTextField = "Localidad";

                this.ListLocalidad.DataBind();
                //cargo el codigo postal
                this.cargarCodigoPostal(this.ListProvincia.SelectedValue, this.ListLocalidad.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  localidades. " + ex.Message));
            }
        }

        private void cargarCodigoPostal(string provincia, string localidad)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerCodPostalByLocalidadProvincia(provincia, localidad);
                foreach (DataRow dr in dt.Rows)
                {
                    this.txtCodigoPostal.Text = dr[0].ToString();
                    this.txtLocalidad.Text = localidad;
                }
                //this.ListCodPostal.DataValueField = "Postal";
                //this.ListCodPostal.DataTextField = "Postal";

                //this.ListCodPostal.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  codigo postales. " + ex.Message));
            }
        }

        private void cargarIvaClientes()
        {
            try
            {
                this.DropListIva.DataSource = controlador.obtenerIvaClientes();
                this.DropListIva.DataValueField = "id";
                this.DropListIva.DataTextField = "descripcion";

                this.DropListIva.DataBind();
                ListItem ls = new ListItem();
                ls.Text = "Seleccione...";
                ls.Value = "-1";

                this.DropListIva.Items.Insert(0, ls);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
            }
        }

        private void cargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.ListSucursales.DataSource = dt;
                this.ListSucursales.DataValueField = "Id";
                this.ListSucursales.DataTextField = "nombre";
                this.ListSucursales.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        private void cargarBTB()//
        {
            try
            {
                var btb = contClienteEntity.obtenerCodigoBTBbyIdClienteProveedor(this.idCliente);
                if (btb != null)
                {
                    txtCodigoBTB1.Text = btb.CodigoBTB1;
                    txtCodigoBTB2.Text = btb.CodigoBTB2;
                }
            }
            catch
            {

            }
        }
        #endregion

        #region datos clientes
        /// <summary>
        /// Consulta los datos del cliente en la DB y lo devuelve
        /// </summary>
        /// <param name="id">id del Cliente</param>
        private void cargarCliente(int idCliente)
        {
            try
            {
                DateTime fechaAlta = new DateTime();
                Cliente cl = this.controlador.obtenerClienteIDConFecha(idCliente, ref fechaAlta);
                var clDatos = this.contClienteEntity.obtenerClienteDatosByCliente(idCliente);
                string perfil = Session["Login_NombrePerfil"] as string;
                if (cl != null)
                {
                    this.txtID.Text = cl.id.ToString();
                    this.txtCodCliente.Text = cl.codigo;
                    this.DropListTipo.SelectedValue = cl.tipoCliente.id.ToString();
                    this.txtRazonSocial.Text = cl.razonSocial;
                    this.txtClienteExportacion.Text = cl.razonSocial;
                    this.DropListGrupo.SelectedValue = cl.grupo.id.ToString();
                    if (fechaAlta != null)
                        this.txtFechaAlta.Text = fechaAlta.ToString("dd/MM/yyyy");
                    else
                        this.txtFechaAlta.Text = string.Empty;
                    //como elimine categoria por defectno no pongo nada
                    //this.DropListCategoria.SelectedValue = 1;
                    if (this.DropListTipo.SelectedItem.Text.Contains("DNI"))
                    {
                        this.txtCuit.Text = cl.cuit.PadLeft(8, '0');
                    }
                    else
                    {
                        this.txtCuit.Text = this.formatearCuit(cl.cuit);
                    }
                    //this.DropListIva.Text = cl.iva;
                    this.DropListIva.SelectedIndex = this.DropListIva.Items.IndexOf(DropListIva.Items.FindByText(cl.iva));
                    this.DropListPais.SelectedValue = cl.pais.id.ToString();
                    this.txtSaldoMaximo.Text = cl.saldoMax.ToString();
                    this.txtVencFC.Text = cl.vencFC.ToString();
                    this.txtDescFC.Text = cl.descFC.ToString();
                    this.txtObservaciones.Text = cl.observaciones;
                    this.txtAlias.Text = cl.alias;
                    this.DropListEstado.SelectedValue = cl.estado.id.ToString();
                    this.ListListaPrecios.SelectedValue = cl.lisPrecio.id.ToString();
                    this.ListVendedores.SelectedValue = cl.vendedor.id.ToString();
                    this.DropListFormaPago.SelectedValue = cl.formaPago.id.ToString();
                    if (clDatos.Count > 0)
                        this.ListDescuentoPorCantidad.SelectedValue = clDatos[0].AplicaDescuentoCantidad.ToString();

                    codigo = cl.codigo;
                    cuit = cl.cuit;

                    Session.Add("ClientesABM_CodCliente", codigo);
                    Session.Add("ClientesABM_CuitCliente", cuit);
                    Session.Add("ClientesABM_Cliente", cl);
                    this.cargarTablaDireccion();
                    this.cargarTablaContacto();

                    //cargo alerta
                    this.cargarAlerta();
                    //cargo sucursales
                    this.cargarSucursalesCliente();
                    //cargo expreso
                    this.cargarExpresoCliente();
                    //cargo datos de entrega
                    this.cargarDatosEntregaCliente();
                    //cargo datos de IIBB
                    this.cargarIBBCliente();
                    //cargo mail envio FC
                    this.cargarDatosMailCliente();
                    //cargo forma de venta porcentual 
                    this.cargarFormaVentaCliente();
                    //cargo datos empleado
                    this.cargarEmpleadoCliente();
                    //cargo clientes referidos del cliente
                    this.cargarReferidosCliente();

                    //Si es iva cliente del exterior
                    if (cl.iva.Contains("Exterior") == true)
                    {
                        this.linkExportacion.Visible = true;
                        this.validarIdImpositivoCliente();
                    }
                    //cargo sistema millas
                    this.cargarDatosMillas();

                    this.cargarEventosCliente();
                    if (perfil == "Distribuidor")
                    {
                        var cr = this.contClienteEntity.obtenerClienteReferidoPorHijo(cl.id);
                        if (cr != null)
                        {
                            if (this.DropListTipo.SelectedItem.Text.ToLower() == "experta")
                            {
                                this.PanelFamilia.Visible = true;
                                this.cargarClientesFamilia();
                                this.DropListFamilia.SelectedValue = cr.Padre.ToString();
                            }
                            Session.Add("ClientesABM_ClienteReferido", cr);
                        }
                    }

                }
                else
                {
                    //Pop up 
                    //El cliente no existe
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo cargar cliente desde la base"));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cliente. " + ex.Message));
            }
        }

        private void cargarProveedor(int idProveedor)
        {
            try
            {
                Cliente cl = this.controlador.obtenerProveedorID(idProveedor);
                if (cl != null)
                {
                    this.txtID.Text = cl.id.ToString();
                    this.txtCodCliente.Text = cl.codigo;
                    this.DropListTipo.SelectedValue = cl.tipoCliente.id.ToString();
                    this.txtRazonSocial.Text = cl.razonSocial;
                    this.DropListGrupo.SelectedValue = cl.grupo.id.ToString();
                    //Como elimine categoria lo comento
                    //this.DropListCategoria.SelectedValue = cl.categoria.id.ToString();
                    this.txtCuit.Text = cl.cuit;
                    //this.DropListIva.SelectedValue = cl.iva;
                    this.DropListIva.SelectedValue = this.DropListIva.Items.FindByText(cl.iva).Value;
                    this.DropListPais.SelectedValue = cl.pais.id.ToString();
                    this.txtSaldoMaximo.Text = cl.saldoMax.ToString();
                    this.txtVencFC.Text = cl.vencFC.ToString();
                    this.txtDescFC.Text = cl.descFC.ToString();
                    this.txtObservaciones.Text = cl.observaciones;
                    this.txtAlias.Text = cl.alias;
                    this.DropListEstado.SelectedValue = cl.estado.id.ToString();

                    //this.ListListaPrecios.SelectedValue = cl.lisPrecio.id.ToString();
                    //this.ListVendedores.SelectedValue = cl.vendedor.id.ToString();

                    codigo = cl.codigo;
                    cuit = this.formatearCuit(cl.cuit);

                    Session.Add("ClientesABM_CodCliente", codigo);
                    Session.Add("ClientesABM_CuitCliente", cuit);
                    Session.Add("ClientesABM_Cliente", cl);

                    this.cargarTablaDireccion();

                    this.cargarTablaContacto();

                    //cargo alerta
                    this.cargarAlerta();

                    this.cargarIBBProveedor();

                }
                else
                {
                    //Pop up 
                    //El cliente no existe
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo cargar proveedor desde la base."));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedor. " + ex.Message));
            }
        }

        public void cargarContacto()
        {
            try
            {

                contacto contacto = new contacto();
                contacto.cargo = txtCargoContacto.Text;
                contacto.mail = txtMailContacto.Text;
                contacto.nombreComp = txtNombreContacto.Text;
                contacto.numero = txtNumeroContacto.Text;
                contacto.tipoCont.id = 1;
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cliente = new Cliente();
                    Session.Add("ClientesABM_Cliente", cliente);
                }
                Cliente cliente2 = Session["ClientesABM_Cliente"] as Cliente;
                cliente2.contactos.Add(contacto);
                Session.Add("ClientesABM_Cliente", cliente2);
                this.cargarTablaContacto();
                this.limpiarCampos();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));

            }

        }

        public void cargarContactoModificar()
        {
            try
            {
                int E = (int)Session["ClientesABM_EditarCon"];
                int P = (int)Session["ClientesABM_PosCon"];
                if (E == 0)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    contacto contacto = new contacto();
                    contacto.cargo = txtCargoContacto.Text;
                    contacto.mail = txtMailContacto.Text;
                    contacto.nombreComp = txtNombreContacto.Text;
                    contacto.numero = txtNumeroContacto.Text;
                    contacto.tipoCont.id = 1;
                    contacto.id = controlador.agregarContactoMod(contacto, c.id);
                    if (contacto.id > 0)
                    {
                        c.contactos.Add(contacto);
                        Session.Add("ClientesABM_Cliente", c);
                        this.cargarTablaContacto();
                        this.limpiarCampos();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando contacto"));

                    }
                }
                if (E == 1)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    contacto contacto = Session["ClientesABM_Contacto"] as contacto;
                    contacto.cargo = txtCargoContacto.Text;
                    contacto.mail = txtMailContacto.Text;
                    contacto.nombreComp = txtNombreContacto.Text;
                    contacto.numero = txtNumeroContacto.Text;
                    int i = controlador.ModificarContactoMod(contacto, c.id);
                    if (i > 0)
                    {
                        //c.contactos.Remove(cont);
                        c.contactos.Add(contacto);
                        Session.Add("ClientesABM_Cliente", c);
                        this.cargarTablaContacto();
                        Session.Remove("ClientesABM_EditarCon");
                        this.PosCon = 0;
                        this.EditarCon = 0;
                        Session.Add("ClientesABM_PosCon", PosCon);
                        Session.Add("ClientesABM_EditarCon", EditarCon);
                        this.limpiarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando contacto"));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));

            }

        }

        // direcciones
        public void cargarDireccion()
        {
            try
            {

                direccion dir = new direccion();
                dir.nombre = this.ListTipoDireccion.SelectedValue;
                dir.direc = this.txtDirecDireccion.Text;
                dir.provincia = this.ListProvincia.SelectedValue;
                //dir.localidad = this.ListLocalidad.SelectedValue;
                dir.localidad = this.txtLocalidad.Text;
                dir.codPostal = this.txtCodigoPostal.Text;
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cliente = new Cliente();
                    Session.Add("ClientesABM_Cliente", cliente);
                }
                Cliente cliente2 = Session["ClientesABM_Cliente"] as Cliente;
                cliente2.direcciones.Add(dir);
                Session.Add("ClientesABM_Cliente", cliente2);
                this.cargarTablaDireccion();
                this.limpiarCamposDir();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));

            }

        }

        public void cargarDireccionModificar()
        {
            try
            {
                int E = (int)Session["ClientesABM_EditarDir"];
                int P = (int)Session["ClientesABM_PosDir"];
                if (E == 0)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    direccion dir = new direccion();
                    dir.nombre = this.ListTipoDireccion.SelectedValue;
                    dir.direc = this.txtDirecDireccion.Text;
                    dir.provincia = this.ListProvincia.SelectedValue;
                    //dir.localidad = this.ListLocalidad.SelectedValue;
                    dir.localidad = this.txtLocalidad.Text;
                    dir.codPostal = this.txtCodigoPostal.Text;
                    dir.id = controlador.agregarDireccionMod(dir, c.id);
                    if (dir.id > 0)
                    {
                        c.direcciones.Add(dir);
                        Session.Add("ClientesABM_Cliente", c);
                        this.cargarTablaDireccion();
                        this.limpiarCamposDir();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando direccion"));

                    }
                }
                if (E == 1)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    direccion dir = Session["ClientesABM_Direccion"] as direccion;
                    dir.nombre = this.ListTipoDireccion.SelectedValue;
                    dir.direc = this.txtDirecDireccion.Text;
                    dir.provincia = this.ListProvincia.SelectedValue;
                    //dir.localidad = this.ListLocalidad.SelectedValue;
                    dir.localidad = this.txtLocalidad.Text;
                    dir.codPostal = this.txtCodigoPostal.Text;
                    int i = controlador.ModificarDireccionMod(dir, c.id);
                    if (i > 0)
                    {
                        //c.contactos.Remove(cont);
                        c.direcciones.Add(dir);
                        Session.Add("ClientesABM_Cliente", c);
                        this.cargarTablaDireccion();
                        Session.Remove("ClientesABM_EditarDir");
                        this.PosDir = 0;
                        this.EditarDir = 0;
                        Session.Add("ClientesABM_PosDir", PosDir);
                        Session.Add("ClientesABM_EditarCon", EditarDir);
                        this.limpiarCamposDir();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando direccion"));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando direccion. " + ex.Message));

            }

        }

        /// <summary>
        /// Carga las sucursales del Cliente en la TAB de sucursales
        /// </summary>
        private void cargarSucursalesCliente()
        {
            try
            {
                var sucursales = this.contClienteEntity.obtenerSucursalesCliente(this.idCliente);
                if (sucursales != null)
                {
                    this.ListBoxSucursales.DataSource = sucursales;

                    this.ListBoxSucursales.DataValueField = "id";
                    this.ListBoxSucursales.DataTextField = "nombre";
                    this.ListBoxSucursales.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga las descuentos
        /// </summary>
        private void cargarDescuentoCliente()
        {
            try
            {
                if (this.idCliente > 0)
                {
                    var descuentos = this.contClienteEntity.obtenerDescuentosCliente(this.idCliente);
                    if (descuentos != null & descuentos.Count > 0)
                    {
                        var desc = descuentos.Select(x => new { x.Id, valor = x.Descripcion + " - " + x.Descuento }).ToList();
                        this.ListBoxDescuentos.DataSource = desc;

                        this.ListBoxDescuentos.DataValueField = "Id";
                        this.ListBoxDescuentos.DataTextField = "valor";
                        this.ListBoxDescuentos.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando decuentos. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga el expresso
        /// </summary>
        private void cargarExpresoCliente()
        {
            try
            {
                var exp = this.contClienteEntity.obtenerExpresoCliente(this.idCliente);
                this.ddlExpresos.SelectedValue = exp.id.ToString();
                cargarDatosExpreso(exp.id);
                this.txtObservacionesExpreso.Text = exp.Cliente_Expresos.Where(x => x.Cliente == this.idCliente).FirstOrDefault().Observacion;
                //if (exp.Count > 0 & exp != null)
                //{
                //    var e = exp.FirstOrDefault();
                //    //this.ddlExpresos.SelectedValue = e.id.ToString();
                //    cargarDatosExpreso(e.id);
                //}
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando decuentos. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga empleado
        /// </summary>
        private void cargarEmpleadoCliente()
        {
            try
            {
                controladorEmpleado contEmpleado = new controladorEmpleado();

                var d = this.contClienteEntity.obtenerClienteDatosByCliente(this.idCliente).FirstOrDefault();

                var emp = contEmpleado.obtenerEmpleadoID(Convert.ToInt32(d.Empleado));
                this.listEmpleados.SelectedValue = emp.id.ToString();
                this.txtNombreEepleado.Text = emp.nombre;
                this.txtApellidoEmpleado.Text = emp.apellido;
                this.txtDNIEmpleado.Text = emp.dni;
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando decuentos. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga datos de entrega
        /// </summary>
        private void cargarDatosEntregaCliente()
        {
            try
            {
                var entrega = this.contClienteEntity.obtenerEntregaCliente(this.idCliente);
                if (entrega != null)
                {
                    this.ListTipoEntrega.SelectedValue = entrega.TipoEntrega.ToString();
                    this.txtHorarioEntrega.Text = entrega.HorarioEntrega;
                    this.ListZonaEntrega.SelectedValue = entrega.ZonaEntrega.ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de entrega. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga iiBB
        /// </summary>
        private void cargarIBBCliente()
        {
            try
            {
                var IIBB = this.contClienteEntity.obtenerIngresosBrutoCliente(this.idCliente);
                if (IIBB != null)
                {
                    this.txtIngBrutos.Text = IIBB.Percepcion.ToString();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de ingresos brutos. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga iiBB
        /// </summary>
        private void cargarIBBProveedor()
        {
            try
            {
                var IIBB = this.contClienteEntity.obtenerIngresosBrutoCliente(this.idCliente);
                if (IIBB != null)
                {
                    this.txtIngBrutos.Text = IIBB.Retencion.ToString();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de ingresos brutos. " + ex.Message));
            }
        }
        private void cargarDatosMailCliente()
        {
            try
            {
                var mail = this.contClienteEntity.obtenerClienteDatosByCliente(this.idCliente);
                if (mail != null && mail.Count > 0)
                {
                    this.txtMailEntrega.Text = mail.FirstOrDefault().Mail;
                    this.txtEnviarMailCRM.Value = mail.FirstOrDefault().Mail.ToString();

                    if (!String.IsNullOrEmpty(mail.FirstOrDefault().Celular))
                    {
                        if (mail.FirstOrDefault().Celular.Contains("-"))
                        {
                            this.txtCodArea.Text = mail.FirstOrDefault().Celular.Split('-')[0];
                            this.txtCelularSMS.Text = mail.FirstOrDefault().Celular.Split('-')[1];
                        }
                        else
                        {

                            this.txtCodArea.Text = mail.FirstOrDefault().Celular.Substring(0, 2);
                            this.txtCelularSMS.Text = mail.FirstOrDefault().Celular.Remove(0, 2);
                        }

                    }
                    if (mail.FirstOrDefault().FechaNacimiento != null)
                        this.txtFechaNacimientoSMS.Text = mail.FirstOrDefault().FechaNacimiento.Value.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos mail envio FC. " + ex.Message));
            }
        }
        private void cargarFormaVentaCliente()
        {
            try
            {
                var forma = this.contClienteEntity.obtenerFormaVentaCliente(this.idCliente);
                if (forma != null)
                {
                    this.ListFormasVenta.SelectedValue = forma.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos forma venta. " + ex.Message));
            }
        }
        private void validarIdImpositivoCliente()
        {
            try
            {
                int cliente = this.idCliente;
                DataTable dt = this.controlador.obtenerIdImpositivoCliente(cliente);

                if (dt.Rows.Count > 0)
                {
                    this.txtIdImpositivo.Text = dt.Rows[0].ItemArray[2].ToString();
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo datos de cliente para exportacion." + ex.Message));
            }
        }
        #endregion

        #region ABM Cliente / Proveedor
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (IsValid)
                {
                    if (this.accion == 1 || this.accion == 3)
                        this.agregarCliente();

                    if (this.accion == 2 || this.accion == 4)
                        this.modificarCliente();
                }
                else
                {
                    this.btnAgregar.Attributes.Remove("Disabled");
                }
            }
            catch
            {

            }
        }

        private void agregarCliente()
        {
            try
            {
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cli = new Cliente();
                    Session.Add("ClientesABM_Cliente", cli);
                }
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                string perfil = Session["Login_NombrePerfil"] as string;

                //antes de guardar vuelvo a cargar el ultimo codigo 
                this.generarCodigo();

                cliente.codigo = txtCodCliente.Text;
                cliente.tipoCliente.id = Convert.ToInt32(this.DropListTipo.SelectedValue);
                cliente.tipoCliente.descripcion = this.DropListTipo.SelectedItem.Text;
                cliente.razonSocial = txtRazonSocial.Text;
                cliente.grupo.id = Convert.ToInt32(this.DropListGrupo.SelectedValue);
                //como elimine la lista le pongo por defecto 0
                //cliente.categoria.id = Convert.ToInt32(this.DropListCategoria.SelectedValue);
                cliente.categoria.id = 1;
                cliente.estado.id = Convert.ToInt32(this.DropListEstado.SelectedValue);
                //saco guiones al CUIT
                cliente.cuit = txtCuit.Text.Replace("-", String.Empty);
                cliente.iva = this.DropListIva.SelectedValue.ToString();
                cliente.pais.id = Convert.ToInt32(this.DropListPais.SelectedValue);
                cliente.expreso.id = 1;
                string saldMax = txtSaldoMaximo.Text.Replace('.', ',');
                cliente.saldoMax = Convert.ToDecimal(saldMax);
                cliente.vencFC = Convert.ToInt32(this.txtVencFC.Text);
                string descFC = this.txtDescFC.Text.Replace('.', ',');
                cliente.descFC = Convert.ToDecimal(descFC);
                cliente.observaciones = this.txtObservaciones.Text;

                //si es un dni lo dejo de 8 digitos
                if (this.DropListTipo.SelectedItem.Text == "DNI")
                {
                    cliente.cuit = cliente.cuit.PadLeft(8, '0');
                }

                // si tiene 0, quiere decir que no es uruguay y tiene que corroborar que tenga 11 digitos, delo contrario si es uruguay no lo valida.
                if (esUruguay == 0)
                {
                    if (cliente.cuit.Length < 11 && this.DropListIva.SelectedItem.Text == "Responsable Inscripto")
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El tipo de iva Resp. Inscripto requiere de un CUIT de 11 digitos."));
                        return;
                    }
                }

                //alerta cliente                
                cliente.alerta.descripcion = this.txtAlerta.Text;
                cliente.alerta.idCliente = cliente.id;

                cliente.hijoDe = 0;
                cliente.alias = this.txtAlias.Text;
                cliente.sucursal.id = 1;

                int idForma = 0;

                if (accion == 1)
                {
                    cliente.vendedor.id = Convert.ToInt32(this.ListVendedores.SelectedValue);
                    cliente.lisPrecio.id = Convert.ToInt32(this.ListListaPrecios.SelectedValue);
                    cliente.formaPago.id = Convert.ToInt32(this.DropListFormaPago.SelectedValue);
                    if (this.ListFormasVenta.Items.Count > 0)
                        idForma = Convert.ToInt32(this.ListFormasVenta.SelectedValue);
                }
                if (accion == 3)
                {
                    cliente.vendedor.id = this.controlador.obtenerPrimerVendedor();
                    cliente.lisPrecio.id = this.controlador.obtenerPrimeraListaPrecios();
                    cliente.formaPago.id = this.controlador.obtenerPrimerFormaPago();
                }

                if (cliente.direcciones.Count == 0)
                    cliente.direcciones = this.obtenerListDirecciones();


                if (esUruguay == 0)
                {
                    if (this.controlador.validateCuit(this.txtCuit.Text, this.DropListTipo.SelectedItem.Text) || this.DropListIva.SelectedValue == "1")
                    {
                        if (accion == 1) //Si es 1 es cliente 
                        {
                            cliente.origen = 1;
                            int i = this.controlador.agregarCliente(cliente);

                            //Actualizo IIBB según el padrón de Clientes
                            if (i > 0)
                            {
                                int iibb = this.controlador.actualizarPadronCliente(i, this.txtCuit.Text.Replace("-", ""), 1);//1 = padron clientes, 2 = padron proveedores
                            }

                            if (idForma > 0)
                            {
                                this.contClienteEntity.agregarFormaVentaACliente(cliente.id, idForma);
                            }

                            //Verifico si utiliza modo distribución y si quien lo da de alta es dsitribuidor, se agrega al distribuidor como padre 
                            if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                            {
                                if (perfil == "Distribuidor")
                                {
                                    var idDistribuidor = (int)Session["Login_Vendedor"];
                                    Clientes_Referidos cr = new Clientes_Referidos();
                                    if (DropListTipo.SelectedItem.Text.ToLower() == "lider")
                                    {
                                        cr.Padre = idDistribuidor;
                                        cr.Hijo = i;
                                    }
                                    if (DropListTipo.SelectedItem.Text.ToLower() == "experta")
                                    {
                                        cr.Padre = Convert.ToInt32(DropListFamilia.SelectedValue);
                                        cr.Hijo = i;
                                    }

                                    this.contClienteEntity.agregarClienteReferido(cr);
                                }
                            }


                            //datos de mail para envio FC
                            Cliente_Datos datos = new Cliente_Datos();
                            datos.Mail = this.txtMailEntrega.Text;
                            datos.IdCliente = cliente.id;
                            datos.Celular = this.txtCodArea.Text + "-" + this.txtCelularSMS.Text;
                            datos.AplicaDescuentoCantidad = Convert.ToInt32(this.ListDescuentoPorCantidad.SelectedValue);
                            if (!String.IsNullOrEmpty(this.txtFechaNacimientoSMS.Text))
                            {
                                datos.FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoSMS.Text, new CultureInfo("es-AR"));
                            }
                            this.contClienteEntity.agregarClienteDatos(datos);

                            this.verificarAgregarTareaAvisoCumpleanios(datos);

                            this.RespuestaAgregarCliente(i);
                        }
                        if (accion == 3) // Si es 3 es proveedor
                        {
                            cliente.origen = 2;
                            int i = this.controlador.agregarCliente(cliente);
                            if (i > 0)
                            {
                                //agrego cuenta al proveedor
                                ControladorCCProveedor contCCP = new ControladorCCProveedor();
                                contCCP.agregarCuentaProveedor(i);
                                int iibb = this.controlador.actualizarPadronCliente(i, this.txtCuit.Text.Replace("-", ""), 2);
                            }
                            this.RespuestaAgregarProveedor(i);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El CUIT ingresado es incorrecto"));

                    }
                }
                else
                {
                    if (accion == 1) //Si es 1 es cliente 
                    {
                        cliente.origen = 1;
                        int i = this.controlador.agregarCliente(cliente);

                        //Actualizo IIBB según el padrón de Clientes
                        if (i > 0)
                        {
                            int iibb = this.controlador.actualizarPadronCliente(i, this.txtCuit.Text.Replace("-", ""), 1);//1 = padron clientes, 2 = padron proveedores
                        }

                        if (idForma > 0)
                        {
                            this.contClienteEntity.agregarFormaVentaACliente(cliente.id, idForma);
                        }

                        //Verifico si utiliza modo distribución y si quien lo da de alta es dsitribuidor, se agrega al distribuidor como padre 
                        if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                        {
                            if (perfil == "Distribuidor")
                            {
                                var idDistribuidor = (int)Session["Login_Vendedor"];
                                Clientes_Referidos cr = new Clientes_Referidos();
                                if (DropListTipo.SelectedItem.Text.ToLower() == "lider")
                                {
                                    cr.Padre = idDistribuidor;
                                    cr.Hijo = i;
                                }
                                if (DropListTipo.SelectedItem.Text.ToLower() == "experta")
                                {
                                    cr.Padre = Convert.ToInt32(DropListFamilia.SelectedValue);
                                    cr.Hijo = i;
                                }

                                this.contClienteEntity.agregarClienteReferido(cr);
                            }
                        }


                        //datos de mail para envio FC
                        Cliente_Datos datos = new Cliente_Datos();
                        datos.Mail = this.txtMailEntrega.Text;
                        datos.IdCliente = cliente.id;
                        datos.Celular = this.txtCodArea.Text + "-" + this.txtCelularSMS.Text;
                        datos.AplicaDescuentoCantidad = Convert.ToInt32(this.ListDescuentoPorCantidad.SelectedValue);
                        if (!String.IsNullOrEmpty(this.txtFechaNacimientoSMS.Text))
                        {
                            datos.FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoSMS.Text, new CultureInfo("es-AR"));
                        }
                        this.contClienteEntity.agregarClienteDatos(datos);

                        this.verificarAgregarTareaAvisoCumpleanios(datos);

                        this.RespuestaAgregarCliente(i);
                    }
                    if (accion == 3) // Si es 3 es proveedor
                    {
                        cliente.origen = 2;
                        int i = this.controlador.agregarCliente(cliente);
                        if (i > 0)
                        {
                            //agrego cuenta al proveedor
                            ControladorCCProveedor contCCP = new ControladorCCProveedor();
                            contCCP.agregarCuentaProveedor(i);
                            int iibb = this.controlador.actualizarPadronCliente(i, this.txtCuit.Text.Replace("-", ""), 2);
                        }
                        this.RespuestaAgregarProveedor(i);
                    }
                }



            }
            catch (Exception ex)
            {
                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando cliente. " + ex.Message));

            }
        }

        private void RespuestaAgregarCliente(int i)
        {
            try
            {
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                switch (i)
                {
                    default:
                        Session.Remove("ClientesABM_Cliente");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Cliente: " + this.txtRazonSocial.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cliente agregado", "ClientesABM.aspx?accion=1"));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar contacto"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar direccion"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar alerta"));
                        break;
                    case -5:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Codigo del Cliente ya fue ingresado"));
                        cliente.codigo = txtCodCliente.Text;
                        break;
                    case -6:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El CUIT del Cliente ya fue ingresado"));
                        cliente.cuit = txtCuit.Text;
                        break;
                    case 0:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente"));
                        break;

                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
            //int i = this.controlador.agregarCliente(cliente);
            //if (i > 0)
            //{
            //    Session.Remove("ClientesABM_Cliente");
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cliente agregado", "ClientesABM.aspx"));

            //}
            //else
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar cliente"));
            //}
        }

        private void RespuestaAgregarProveedor(int i)
        {
            try
            {
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                switch (i)
                {
                    //case 1:
                    //    Session.Remove("ClientesABM_Cliente");
                    //    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Proveedor: " + this.txtRazonSocial.Text);
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proveedor agregado", "ClientesABM.aspx?accion=3"));
                    //    break;
                    default:
                        Session.Remove("ClientesABM_Cliente");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Proveedor: " + this.txtRazonSocial.Text);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Proceso finalizado.", "Proveedor agregado"));
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proveedor agregado", "ClientesABM.aspx?accion=3"));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Proveedor"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Proveedor ya que ocurrio un error al agregar contacto"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Proveedor ya que ocurrio un error al agregar direccion"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Proveedor ya que ocurrio un error al agregar alerta"));
                        break;
                    case -5:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Codigo del Proveedor ya fue ingresado"));
                        cliente.codigo = txtCodCliente.Text;
                        break;
                    case -6:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El CUIT del Proveedor ya fue ingresado"));
                        cliente.cuit = txtCuit.Text;
                        break;
                    case 0:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Proveedor"));
                        break;
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
        }

        private List<direccion> obtenerListDirecciones()
        {
            try
            {
                List<direccion> direcciones = new List<direccion>();

                foreach (Control control in this.phDireccion.Controls)
                {
                    direccion d = new direccion();
                    TableRow tr = control as TableRow;
                    d.nombre = tr.Cells[0].Text;
                    d.direc = tr.Cells[1].Text;
                    d.provincia = tr.Cells[2].Text;
                    d.localidad = tr.Cells[3].Text;
                    d.codPostal = tr.Cells[4].Text;

                    direcciones.Add(d);
                }

                return direcciones;
            }
            catch
            {
                return null;
            }
        }
        private void modificarCliente()
        {

            try
            {
                string codCli = Session["ClientesABM_CodCliente"] as string;
                string cuitCli = Session["ClientesABM_CuitCliente"] as string;
                string perfil = Session["Login_NombrePerfil"] as string;
                Clientes_Referidos cr = Session["ClientesABM_ClienteReferido"] as Clientes_Referidos;
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;

                Configuracion c = new Configuracion();
                this.modificarDatosCliente(cliente);

                cliente.id = Convert.ToInt32(this.txtID.Text);
                cliente.codigo = txtCodCliente.Text;
                cliente.tipoCliente.id = Convert.ToInt32(this.DropListTipo.SelectedValue);
                cliente.tipoCliente.descripcion = this.DropListTipo.SelectedItem.Text;
                cliente.razonSocial = txtRazonSocial.Text;
                cliente.grupo.id = Convert.ToInt32(this.DropListGrupo.SelectedValue);
                //como elimino categoria le asigno 0
                //cliente.categoria.id = Convert.ToInt32(this.DropListCategoria.SelectedValue);
                cliente.categoria.id = 1;
                cliente.estado.id = Convert.ToInt32(this.DropListEstado.SelectedValue);
                //quito guiones del cuit

                cliente.cuit = txtCuit.Text.Replace("-", String.Empty);
                cliente.iva = this.DropListIva.SelectedValue.ToString();
                cliente.pais.id = Convert.ToInt32(this.DropListPais.SelectedValue);
                cliente.expreso.id = 1;
                string saldMax = txtSaldoMaximo.Text.Replace(',', '.');
                cliente.saldoMax = Convert.ToDecimal(saldMax, CultureInfo.InvariantCulture);
                cliente.vencFC = Convert.ToInt32(this.txtVencFC.Text);
                string descFC = this.txtDescFC.Text.Replace(',', '.');
                cliente.descFC = Convert.ToDecimal(descFC, CultureInfo.InvariantCulture);
                cliente.observaciones = this.txtObservaciones.Text;

                //alerta cliente   
                alerta a = this.controlador.obtenerAlertaClienteByID(this.idCliente);
                a.descripcion = this.txtAlerta.Text;
                a.idCliente = cliente.id;

                cliente.alerta = a;

                cliente.hijoDe = 0;
                cliente.alias = this.txtAlias.Text;
                cliente.sucursal.id = 1;
                cliente.activo = 1;
                int idForma = 0;
                if (accion == 2)
                {
                    if (cliente.descFC > Convert.ToDecimal(c.maxDtoFactura) && Convert.ToDecimal(c.maxDtoFactura) > 0)
                    {
                        this.RespuestaModificarCliente(-8);
                        return;
                    }
                    cliente.vendedor.id = Convert.ToInt32(this.ListVendedores.SelectedValue);
                    cliente.lisPrecio.id = Convert.ToInt32(this.ListListaPrecios.SelectedValue);
                    cliente.formaPago.id = Convert.ToInt32(this.DropListFormaPago.SelectedValue);
                    if (this.ListFormasVenta.Items.Count > 0)
                        idForma = Convert.ToInt32(this.ListFormasVenta.SelectedValue);

                }
                if (accion == 4)
                {
                    cliente.vendedor.id = this.controlador.obtenerPrimerVendedor();
                    cliente.lisPrecio.id = this.controlador.obtenerPrimeraListaPrecios();
                    cliente.formaPago.id = this.controlador.obtenerPrimerFormaPago();
                }

                if (esUruguay == 0)
                {
                    if (this.controlador.validateCuit(this.txtCuit.Text.Replace("-", String.Empty), this.DropListTipo.SelectedItem.Text) || this.DropListIva.SelectedValue == "1")
                    {
                        if (accion == 2)
                        {
                            cliente.origen = 1;
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico cliente: " + this.txtCodCliente.Text);
                            int i = this.controlador.modificarCliente(cliente, cuitCli, codCli);
                            if (idForma > 0)
                            {
                                this.contClienteEntity.modificarFormaVentaACliente(cliente.id, idForma);
                            }

                            //Verifico si utiliza modo distribución y si quien lo da de alta es dsitribuidor, se agrega al distribuidor como padre 
                            if (WebConfigurationManager.AppSettings.Get("Distribucion") != "1")
                            {
                                //if (perfil == "Distribuidor")
                                //{

                                //    var idDistribuidor = (int)Session["Login_Vendedor"];
                                //    if (DropListTipo.SelectedItem.Text == "Lider")
                                //    {
                                //        cr.Padre = idDistribuidor;
                                //        cr.Hijo = cliente.id;
                                //    }
                                //    if (DropListTipo.SelectedItem.Text == "Experta")
                                //    {
                                //        cr.Padre = Convert.ToInt32(DropListFamilia.SelectedValue);
                                //        cr.Hijo = cliente.id;
                                //    }
                                //    this.contClienteEntity.modificarClienteReferido(cr);
                                //}
                                this.contClienteEntity.quitarFormaVentaACliente(idCliente, -1);
                            }
                            //else
                            //{
                            //}
                            this.RespuestaModificarCliente(i);
                        }
                        if (accion == 4)
                        {
                            cliente.origen = 2;
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico proveedor: " + this.txtCodCliente.Text);
                            int i = this.controlador.modificarProveedor(cliente, cuitCli, codCli);
                            this.RespuestaModificarProveedor(i);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El CUIT ingresado es incorrecto"));
                    }
                }
                else
                {
                    if (accion == 2)
                    {
                        cliente.origen = 1;
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico cliente: " + this.txtCodCliente.Text);
                        int i = this.controlador.modificarCliente(cliente, cuitCli, codCli);
                        if (idForma > 0)
                        {
                            this.contClienteEntity.modificarFormaVentaACliente(cliente.id, idForma);
                        }

                        //Verifico si utiliza modo distribución y si quien lo da de alta es dsitribuidor, se agrega al distribuidor como padre 
                        if (WebConfigurationManager.AppSettings.Get("Distribucion") != "1")
                        {
                            //if (perfil == "Distribuidor")
                            //{

                            //    var idDistribuidor = (int)Session["Login_Vendedor"];
                            //    if (DropListTipo.SelectedItem.Text == "Lider")
                            //    {
                            //        cr.Padre = idDistribuidor;
                            //        cr.Hijo = cliente.id;
                            //    }
                            //    if (DropListTipo.SelectedItem.Text == "Experta")
                            //    {
                            //        cr.Padre = Convert.ToInt32(DropListFamilia.SelectedValue);
                            //        cr.Hijo = cliente.id;
                            //    }
                            //    this.contClienteEntity.modificarClienteReferido(cr);
                            //}
                            this.contClienteEntity.quitarFormaVentaACliente(idCliente, -1);
                        }
                        //else
                        //{
                        //}
                        this.RespuestaModificarCliente(i);
                    }
                    if (accion == 4)
                    {
                        cliente.origen = 2;
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico proveedor: " + this.txtCodCliente.Text);
                        int i = this.controlador.modificarProveedor(cliente, cuitCli, codCli);
                        this.RespuestaModificarProveedor(i);
                    }
                }

            }
            catch (Exception ex)
            {

                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
            }

        }

        private void RespuestaModificarCliente(int i)
        {
            try
            {
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                switch (i)
                {
                    case 1:
                        Session.Remove("ClientesABM_Cliente");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Cliente: " + this.txtRazonSocial.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cliente modificado con exito", "Clientesaspx.aspx"));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Cliente"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Cliente ya que ocurrio un error al agregar contacto"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Cliente ya que ocurrio un error al agregar direccion"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Cliente ya que ocurrio un error al agregar alerta"));
                        break;
                    case -5:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error obteniendo datos del cliente"));
                        break;
                    case -6:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El CUIT del Cliente ya fue ingresado"));
                        cliente.cuit = txtCuit.Text;
                        break;
                    case -7:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El codigo del Cliente ya fue ingresado"));
                        cliente.codigo = txtCodCliente.Text;
                        break;
                    case -8:
                        Configuracion config = new Configuracion();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El descuento del cliente no puede ser mayor al " + config.maxDtoFactura + "%."));
                        cliente.codigo = txtCodCliente.Text;
                        break;
                    case 0:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Cliente"));
                        break;

                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
        }

        private void RespuestaModificarProveedor(int i)
        {
            try
            {
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                switch (i)
                {
                    case 1:
                        Session.Remove("ClientesABM_Cliente");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Proveedor: " + this.txtRazonSocial.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proveedor modificado con exito", "ProveedoresF.aspx"));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Proveedor"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Proveedor ya que ocurrio un error al agregar contacto"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Proveedor ya que ocurrio un error al agregar direccion"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Proveedor ya que ocurrio un error al agregar alerta"));
                        break;
                    case -5:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error obteniendo datos del proveedor"));
                        break;
                    case -6:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El CUIT del Proveedor ya fue ingresado"));
                        cliente.cuit = txtCuit.Text;
                        break;
                    case -7:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El codigo del Proveedor ya fue ingresado"));
                        cliente.codigo = txtCodCliente.Text;
                        break;
                    case 0:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar Proveedor"));
                        break;

                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
        }

        protected void btnAgregarTipoCliente_Click(object sender, EventArgs e)
        {
            try
            {

                int i = this.controladorTipoCliente.agregarTipoCliente(this.txtTipoCliente2.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo grupos
                    this.cargarTipoClientes();
                    this.txtTipoCliente2.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar nuevo tipo cliente"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando tipo cliente. " + ex.Message));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                int i = this.controladorGrupoCliente.agregarGrupo(this.txtGrupo2.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo grupos
                    this.cargarGrupoClientes();
                    this.txtGrupo2.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar nuevo grupo cliente"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando grupo cliente. " + ex.Message));
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            try
            {

                int i = this.controladorCatCliente.agregarCategoriaCliente(this.txtCategoria2.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarCategoriaClientes();
                    this.txtCategoria2.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar nueva categoria cliente"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando categoria cliente. " + ex.Message));
            }
        }

        protected void btnAgregarEstado_Click(object sender, EventArgs e)
        {
            try
            {

                int i = this.controladorEstCliente.agregarEstadoCliente(txtDescEstado.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarEstadoClientes();
                    this.txtDescEstado.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar nuevo estado cliente"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando estado cliente. " + ex.Message));
            }
        }

        #endregion

        #region Funcion botones
        protected void btnAgregarDireccion_Click(object sender, EventArgs e)
        {
            if (accion == 1 || accion == 3)
                this.cargarDireccion();
            if (accion == 2 || accion == 4)
                this.cargarDireccionModificar();

        }
        protected void btnAgregarContacto_Click(object sender, EventArgs e)
        {
            if (this.accion == 1 || this.accion == 3)
                this.cargarContacto();
            if (this.accion == 2 || this.accion == 4)
                this.cargarContactoModificar();
        }
        private void limpiarCampos()
        {
            try
            {
                txtCargoContacto.Text = "";
                txtMailContacto.Text = "";
                txtNombreContacto.Text = "";
                txtNumeroContacto.Text = "";
            }
            catch
            {

            }
        }
        private void limpiarCamposDir()
        {
            try
            {
                this.ListTipoDireccion.SelectedIndex = 0;
                this.txtDirecDireccion.Text = "";
                this.ListProvincia.SelectedIndex = 0;
                //this.ListLocalidad.SelectedIndex = 0;
                this.txtLocalidad.Text = "";
                this.txtCodigoPostal.Text = "";
            }
            catch
            {

            }
        }

        private void limpiarCamposCRM()
        {
            try
            {
                this.txtDetalleEvento.Text = "";
                this.txtTarea.Text = "";
                this.drpCRMSituacion.SelectedIndex = 0;
            }
            catch
            {

            }
        }
        protected void btnAgregarAlerta_Click(object sender, EventArgs e)
        {
            this.cargarAlerta();
            this.cargarTablaAlerta();
        }
        public void cargarAlerta()
        {
            try
            {
                alerta a = this.controlador.obtenerAlertaClienteByID(this.idCliente);
                this.txtAlerta.Text = a.descripcion;

                //a.descripcion = txtDescAlerta.Text;
                //if (Session["ClientesABM_Cliente"] == null)
                //{
                //    Cliente cliente = new Cliente();
                //    Session.Add("ClientesABM_Cliente", cliente);
                //}
                //Cliente cliente2 = Session["ClientesABM_Cliente"] as Cliente;
                //cliente2.alertas.Add(a);
                //Session.Add("ClientesABM_Cliente", cliente2);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando alerta. " + ex.Message));

            }
        }
        public void cargarTablaAlerta()
        {
            try
            {
                //Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                //this.phAlertas.Controls.Clear();
                //foreach (alerta a in c.alertas)
                //{
                //    this.cargarPHAlerta(a);
                //}
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando alertas" + ex.Message));
            }
        }
        public void cargarPHAlerta(alerta a)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celAlerta = new TableCell();
                celAlerta.Text = a.descripcion;
                celAlerta.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAlerta);

                phAlertas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando alertas en PH " + ex.Message));
            }
        }
        public void cargarTablaContacto()
        {
            try
            {
                if (Session["ClientesABM_Cliente"] != null)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    this.phContactos.Controls.Clear();
                    int id = 0;
                    foreach (contacto ct in c.contactos)
                    {
                        this.cargarPHContacto(ct, id);
                        id++;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando contactos" + ex.Message));
            }
        }
        public void cargarPHContacto(contacto ct, int id)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = "Contacto_" + id.ToString();

                //Celdas

                TableCell celNombre = new TableCell();
                celNombre.Text = ct.nombreComp;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(25);
                tr.Cells.Add(celNombre);

                TableCell celCargo = new TableCell();
                celCargo.Text = ct.cargo;
                celCargo.VerticalAlign = VerticalAlign.Middle;
                celCargo.Width = Unit.Percentage(20);
                tr.Cells.Add(celCargo);

                TableCell celNumero = new TableCell();
                celNumero.Text = ct.numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.Width = Unit.Percentage(20);
                tr.Cells.Add(celNumero);

                TableCell celMail = new TableCell();
                celMail.Text = ct.mail;
                celMail.VerticalAlign = VerticalAlign.Middle;
                celMail.Width = Unit.Percentage(20);
                tr.Cells.Add(celMail);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.ID = "btnEditar_" + id;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.EditarItemContacto);
                celAccion.Controls.Add(btnEditar);


                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + id;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarItemContacto);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(15);
                tr.Cells.Add(celAccion);

                phContactos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contactos en PH " + ex.Message));
            }
        }


        public void cargarTablaPuntos()
        {
            try
            {
                if (Session["ClientesABM_Cliente"] != null)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    DataTable puntos = controladorCobranza.ObtenerPuntosCliente(c.id);
                    this.phPuntos.Controls.Clear();
                    int id = 0;
                    foreach (DataRow dt in puntos.Rows)
                    {
                        this.cargarPHPuntos(dt, id);
                        id++;
                    }
                    this.labelPuntos.Text = totalPuntos.ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando contactos" + ex.Message));
            }
        }

        public void cargarPHPuntos(DataRow dr, int id)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = "Puntos_" + id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(dr["fecha"]).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.Width = Unit.Percentage(25);
                tr.Cells.Add(celFecha);

                TableCell celCobro = new TableCell();
                celCobro.Text = dr["numero"].ToString();
                celCobro.VerticalAlign = VerticalAlign.Middle;
                celCobro.Width = Unit.Percentage(20);
                tr.Cells.Add(celCobro);

                TableCell celPuntos = new TableCell();
                celPuntos.Text = dr["puntos"].ToString();
                totalPuntos += Convert.ToInt32(dr["puntos"]);
                celPuntos.VerticalAlign = VerticalAlign.Middle;
                celPuntos.Width = Unit.Percentage(20);
                tr.Cells.Add(celPuntos);

                TableCell celTipoPago = new TableCell();
                celTipoPago.Text = dr["tipoPago"].ToString();
                celTipoPago.VerticalAlign = VerticalAlign.Middle;
                celTipoPago.Width = Unit.Percentage(20);
                tr.Cells.Add(celTipoPago);

                phPuntos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando puntos en PH " + ex.Message));
            }
        }

        private void QuitarItemContacto(object sender, EventArgs e)
        {
            try
            {
                string idContacto = (sender as LinkButton).ID.Substring(12, 1);
                //obtengo el cliente del session
                Cliente cl = Session["ClientesABM_Cliente"] as Cliente;

                if (accion == 1 || accion == 3)
                {
                    cl.contactos.Remove(cl.contactos[Convert.ToInt32(idContacto)]);
                }
                if (accion == 2 || accion == 4)
                {
                    int i = controlador.EliminarContactoMod(cl.contactos[Convert.ToInt32(idContacto)], cl.id);
                    if (i > 0)
                    {
                        cl.contactos.Remove(cl.contactos[Convert.ToInt32(idContacto)]);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando contacto"));

                    }
                }
                //cargo el nuevo cliente a la sesion
                Session["ClientesABM_Cliente"] = cl;

                //vuelvo a cargar las direcciones
                this.cargarTablaContacto();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item contacto. " + ex.Message));
            }
        }
        private void EditarItemContacto(object sender, EventArgs e)
        {
            try
            {
                string idContacto = (sender as LinkButton).ID.Substring(10, 1);
                //obtengo el cliente del session
                Cliente cl = Session["ClientesABM_Cliente"] as Cliente;
                contacto ct = cl.contactos[Convert.ToInt32(idContacto)];
                txtNombreContacto.Text = ct.nombreComp;
                txtCargoContacto.Text = ct.cargo;
                txtNumeroContacto.Text = ct.numero;
                txtMailContacto.Text = ct.mail;
                Session.Add("ClientesABM_Contacto", ct);
                cl.contactos.Remove(cl.contactos[Convert.ToInt32(idContacto)]);
                if (accion == 2 || accion == 4)
                {
                    this.EditarCon = 1;
                    this.PosCon = Convert.ToInt32(idContacto);
                    Session.Add("ClientesABM_PosCon", PosCon);
                    Session.Add("ClientesABM_EditarCon", EditarCon);
                }
                Session.Add("ClientesABM_Cliente", cl);
                //cargo el nuevo cliente a la sesion
                Session["ClientesABM_Cliente"] = cl;
                //vuelvo a cargar las direcciones
                this.cargarTablaContacto();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando contacto " + ex.Message));
            }
        }
        protected void btnAgregarLista_Click(object sender, EventArgs e)
        {
            try
            {
                listaPrecio lst = new listaPrecio();
                lst.nombre = txtNombreLista.Text;
                int i = this.controlador.agregarlistaPrecio(lst);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarListasPrecios();
                    this.txtNombreLista.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar lista de precio "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista de precio " + ex.Message));
            }
        }
        protected void lbGenerarCodigoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.generarCodigo();
            }
            catch
            {

            }
        }
        private void generarCodigo()
        {
            try
            {
                string p = this.controlador.obtenerLastCodigoCliente();
                int newp = Convert.ToInt32(p);
                this.txtCodCliente.Text = newp.ToString().PadLeft(6, '0');
            }
            catch
            {

            }
        }
        protected void btnAgregarVendedor_Click(object sender, EventArgs e)
        {
            try
            {
                Gestion_Api.Modelo.Empleado emp = new Gestion_Api.Modelo.Empleado();
                emp.legajo = Convert.ToDecimal(this.txtLegajo.Text);
                emp.nombre = this.txtNombre.Text;
                emp.apellido = this.txtApellido.Text;
                emp.direccion = this.txtDireccion.Text;
                emp.dni = this.txtDni.Text;
                emp.fecNacimiento = Convert.ToDateTime(this.txtFechaNacimiento.Text, new CultureInfo("es-AR"));
                emp.fecIngreso = Convert.ToDateTime(this.txtFechaIngreso.Text, new CultureInfo("es-AR"));
                emp.cuitCuil = this.txtCuitVendedor.Text;
                emp.observaciones = this.txtObservaciones.Text;

                int i = this.contVendedor.agregarEmpleadoVendedor(emp, Convert.ToDecimal(this.txtComision.Text.Replace(',', '.'), CultureInfo.InvariantCulture));
                if (i > 0)
                {
                    this.txtLegajo.Text = "";
                    this.txtNombre.Text = "";
                    this.txtApellido.Text = "";
                    this.txtDireccion.Text = "";
                    this.txtDni.Text = "";
                    this.txtFechaNacimiento.Text = "";
                    this.txtFechaIngreso.Text = "";
                    this.txtCuitVendedor.Text = "";
                    this.txtObservaciones.Text = "";
                    this.txtComision.Text = "";
                    this.cargarVendedores();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puedo agregar Empleado "));
                }
            }
            catch
            {

            }
        }
        protected void btnAgregarFP_Click(object sender, EventArgs e)
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                int i = contFact.agregarFormaPAgo(this.txtFormaPago.Text);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    this.cargarFormaPAgo();
                    this.txtFormaPago.Text = "";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede agregar forma de pago "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando forma de pago. " + ex.Message));
            }
        }
        protected void ListProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarLocalidades(this.ListProvincia.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error seleccionando provincia para cargar localidad. " + ex.Message));
            }
        }
        protected void ListLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCodigoPostal(this.ListProvincia.SelectedValue, this.ListLocalidad.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error seleccionando localidad para cargar codigo postal. " + ex.Message));
            }
        }
        protected void btnAgregarClienteImposivito_Click(object sender, EventArgs e)
        {
            try
            {
                String impositivo = this.txtIdImpositivo.Text;
                DataTable dt = this.controlador.obtenerIdImpositivoCliente(this.idCliente);

                if (dt.Rows.Count > 0)
                {
                    int i = 0;

                    if (impositivo != "")
                    {
                        i = this.controlador.modificarImpositivoCliente(this.idCliente, impositivo);
                    }

                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"ID Impositivo Cliente Actualizado\", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"No se pudo actualizar ID Impositivo Cliente\", {type: \"info\"});", true);
                    }
                }
                else
                {
                    int j = 0;

                    if (impositivo != "")
                    {
                        j = this.controlador.agregarImpositivoCliente(this.idCliente, impositivo);
                    }

                    if (j > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"ID Impositivo Cliente Agregado\", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"No se pudo agregar ID Impositivo Cliente\", {type: \"info\"});", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"Error al procesar la operacion. " + ex.Message + "\", {type: \"error\"});", true);
            }
        }
        protected void DropListTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListTipo.SelectedItem.Text == "DNI")
                {
                    this.lbCUITDNI.InnerText = "DNI";
                    this.txtCuit.Text = "";
                }
                else
                {
                    this.lbCUITDNI.InnerText = "Cuit";
                }

                if (DropListTipo.SelectedItem.Text.ToLower() == "experta")
                {
                    this.cargarClientesFamilia();
                    PanelFamilia.Visible = true;
                }
                else
                {
                    PanelFamilia.Visible = false;
                }

            }
            catch { }
        }
        protected void DropListFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarListasPreciosForma(Convert.ToInt32(this.DropListFormaPago.SelectedValue));
            }
            catch
            {

            }
        }
        private string formatearCuit(string cuit)
        {
            try
            {
                if (cuit.Length == 11)
                {
                    string r = cuit.Substring(0, 2) + "-" + cuit.Substring(2, 8) + "-" + cuit.Substring(10, 1);
                    return r;
                }
            }
            catch (Exception ex)
            {

            }
            return cuit;
        }
        #endregion

        #region Direccion

        // direccion

        public void cargarTablaDireccion()
        {
            try
            {
                if (Session["ClientesABM_Cliente"] != null)
                {
                    Cliente c = Session["ClientesABM_Cliente"] as Cliente;
                    this.phDireccion.Controls.Clear();
                    int id = 0;
                    foreach (direccion d in c.direcciones)
                    {
                        this.cargarPHDireccion(d, id);
                        id++;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error dibujando direcciones" + ex.Message));
            }
        }

        public void cargarPHDireccion(direccion d, int id)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = "Direcion_" + id.ToString();

                //Celdas

                TableCell celNombre = new TableCell();
                celNombre.Text = d.nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(15);
                tr.Cells.Add(celNombre);

                TableCell celCargo = new TableCell();
                celCargo.Text = d.direc;
                celCargo.VerticalAlign = VerticalAlign.Middle;
                celCargo.Width = Unit.Percentage(20);
                tr.Cells.Add(celCargo);

                TableCell celNumero = new TableCell();
                celNumero.Text = d.provincia;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.Width = Unit.Percentage(20);
                tr.Cells.Add(celNumero);

                TableCell celMail = new TableCell();
                celMail.Text = d.localidad;
                celMail.VerticalAlign = VerticalAlign.Middle;
                celMail.Width = Unit.Percentage(20);
                tr.Cells.Add(celMail);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = d.codPostal;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.Width = Unit.Percentage(10);
                tr.Cells.Add(celCodigo);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.ID = "btnEditarD_" + id;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.EditarItemDireccion);
                celAccion.Controls.Add(btnEditar);


                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminarD_" + id;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarItemDireccion);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(15);
                tr.Cells.Add(celAccion);

                phDireccion.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Direcciones en PH " + ex.Message));
            }
        }

        private void QuitarItemDireccion(object sender, EventArgs e)
        {
            try
            {
                string idContacto = (sender as LinkButton).ID.Substring(13, 1);
                //obtengo el cliente del session
                Cliente cl = Session["ClientesABM_Cliente"] as Cliente;

                if (accion == 1 || accion == 3)
                {
                    cl.direcciones.Remove(cl.direcciones[Convert.ToInt32(idContacto)]);
                }
                if (accion == 2 || accion == 4)
                {
                    int i = controlador.EliminarDireccionMod(cl.direcciones[Convert.ToInt32(idContacto)], cl.id);
                    if (i > 0)
                    {
                        cl.direcciones.Remove(cl.direcciones[Convert.ToInt32(idContacto)]);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando direccion"));

                    }
                }
                //cargo el nuevo cliente a la sesion
                Session["ClientesABM_Cliente"] = cl;

                //vuelvo a cargar las direcciones
                this.cargarTablaDireccion();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error quitando item contacto. " + ex.Message));
            }
        }

        private void EditarItemDireccion(object sender, EventArgs e)
        {
            try
            {
                string idContacto = (sender as LinkButton).ID.Substring(11, 1);
                //obtengo el cliente del session
                Cliente cl = Session["ClientesABM_Cliente"] as Cliente;
                direccion d = cl.direcciones[Convert.ToInt32(idContacto)];
                //this.txtTipo.Text = d.nombre;
                this.ListTipoDireccion.SelectedValue = d.nombre;
                this.txtDirecDireccion.Text = d.direc;
                this.txtLocalidad.Text = d.localidad;
                //this.ListLocalidad.SelectedValue = d.localidad;
                this.ListProvincia.SelectedValue = d.provincia;
                this.txtCodigoPostal.Text = d.codPostal;
                Session.Add("ClientesABM_Direccion", d);
                cl.direcciones.Remove(cl.direcciones[Convert.ToInt32(idContacto)]);
                if (accion == 2 || accion == 4)
                {
                    this.EditarDir = 1;
                    this.PosDir = Convert.ToInt32(idContacto);
                    Session.Add("ClientesABM_PosDir", PosCon);
                    Session.Add("ClientesABM_EditarDir", EditarCon);
                }
                Session.Add("ClientesABM_Cliente", cl);
                //cargo el nuevo cliente a la sesion
                Session["ClientesABM_Cliente"] = cl;
                //vuelvo a cargar las direcciones
                this.cargarTablaContacto();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando direccion " + ex.Message));
            }
        }

        #endregion

        #region sucursal
        protected void btnAgregarSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                //agrego a la DB
                int i = this.contClienteEntity.agregarSucursalACliente(this.idCliente, Convert.ToInt32(this.ListSucursales.SelectedValue));
                if (i > 0)
                {
                    this.ListBoxSucursales.Items.Add(this.ListSucursales.SelectedItem.Text);
                    this.ListSucursales.SelectedIndex = 0;
                    this.cargarSucursalesCliente();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }

        protected void btnQuitarSucursal_Click(object sender, EventArgs e)
        {
            try
            {
                //agrego a la DB
                int i = this.contClienteEntity.quitarSucursalACliente(this.idCliente, Convert.ToInt32(this.ListBoxSucursales.SelectedValue));
                if (i > 0)
                {
                    this.ListBoxSucursales.Items.Remove(this.ListBoxSucursales.SelectedItem);
                    this.ListSucursales.SelectedIndex = 0;
                    this.cargarSucursalesCliente();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }

        #endregion

        #region descuento
        protected void btnAgregarDescuento_Click(object sender, EventArgs e)
        {
            try
            {
                Configuracion config = new Configuracion();
                Cliente_Descuentos descuento = new Cliente_Descuentos();
                descuento.Descripcion = this.txtDescripcionDescuento.Text;
                descuento.Descuento = Convert.ToDecimal(this.txtPorcentajeDesc.Text);

                //agrego a la DB
                int i = this.contClienteEntity.agregarDescuentoACliente(this.idCliente, descuento);
                if (i > 0)
                {
                    this.ListBoxDescuentos.Items.Add(descuento.Descripcion + " - " + descuento.Descuento);
                    //this.cargarSucursalesCliente();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                //agrego a la DB
                int i = this.contClienteEntity.quitarDescuentoACliente(this.idCliente, Convert.ToInt32(this.ListBoxDescuentos.SelectedValue));
                if (i > 0)
                {
                    this.ListBoxDescuentos.Items.Remove(this.ListBoxDescuentos.SelectedItem);
                    this.ListBoxDescuentos.SelectedIndex = 0;
                    this.cargarSucursalesCliente();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }

        protected void btnDescuento_Click(object sender, EventArgs e)
        {
            try
            {
                decimal total = 0;
                decimal auxDesc = 100;
                decimal desc2 = 0;
                decimal descTotal = 0;
                Configuracion config = new Configuracion();
                //recorro los descuentos y sumos
                foreach (var item in ListBoxDescuentos.Items)
                {
                    string sdesc = item.ToString().Split('-')[1].Trim();

                    decimal desc = Convert.ToDecimal(sdesc, CultureInfo.InvariantCulture);
                    desc = desc / 100;
                    //cargo el valor 
                    desc2 = auxDesc - (auxDesc * desc);

                    auxDesc = desc2;
                }
                descTotal = 100 - auxDesc;
                if (accion == 2)//solo valido en clientes
                {
                    if (descTotal > Convert.ToDecimal(config.maxDtoFactura) && Convert.ToDecimal(config.maxDtoFactura) > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El descuento del cliente no puede ser mayor al " + config.maxDtoFactura + "%."));
                        return;
                    }
                }
                this.txtDescFC.Text = descTotal.ToString("N");

            }
            catch (Exception ex)
            {

            }
        }
        protected void txtDescFC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Configuracion config = new Configuracion();
                decimal desc = Convert.ToDecimal(this.txtDescFC.Text);
                if (desc > Convert.ToDecimal(config.maxDtoFactura))
                {
                    this.txtDescFC.Text = "0";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se puede agregar un descuento mayor a: " + config.maxDtoFactura + "% \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede agregar un descuento mayor a: " + config.maxDtoFactura));
                }
            }
            catch
            {

            }
        }
        #endregion

        #region expresso
        protected void AgregarExpreso_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente_Expresos ce = new Cliente_Expresos();
                ce.Cliente = this.idCliente;
                ce.Expreso = Convert.ToInt64(this.ddlExpresos.SelectedValue);
                ce.Observacion = this.txtObservacionesExpreso.Text;
                //agrego a la DB
                //int i = this.contClienteEntity.agregarExpresoACliente(this.idCliente, Convert.ToInt64(this.ddlExpresos.SelectedValue));
                int i = this.contClienteEntity.agregarExpresoACliente(ce);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Expreso Agregado\", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo agregar expreso\", {type: \"error\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando expreso." + ex.Message + "\", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }
        /// <summary>
        /// Carga los datos del expreso en el fomulario.
        /// </summary>
        /// <param name="idExpreso">id del expreso a cargar</param>
        private void cargarDatosExpreso(long idExpreso)
        {
            try
            {
                this.ddlExpresos.SelectedValue = idExpreso.ToString();
                expreso e = contExpreso.obtenerExpresoPorID(idExpreso);
                this.txtNombreExpreso.Text = e.nombre;
                this.txtCuitExpreso.Text = e.cuit.ToString();
                this.txtTelefonoExpreso.Text = e.telefono;
                this.txtDireccionExpreso.Text = e.direccion;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos expreso en el formulario." + ex.Message));
            }
        }
        protected void ddlExpresos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarDatosExpreso(Convert.ToInt64(ddlExpresos.SelectedValue));
        }

        #endregion

        #region datos entregas
        protected void btnAgregarEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                //agrego a la DB
                int i = this.contClienteEntity.agregarEntregaACliente(this.idCliente, Convert.ToInt64(this.ListTipoEntrega.SelectedValue), this.txtHorarioEntrega.Text, Convert.ToInt32(this.ListZonaEntrega.SelectedValue));
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel8, UpdatePanel8.GetType(), "alert", "$.msgbox(\"Datos entrega Guardados\", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel8, UpdatePanel8.GetType(), "alert", "$.msgbox(\"No se guardaron datos de entrega\", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel8, UpdatePanel8.GetType(), "alert", "$.msgbox(\"Error agregando datos de entrega." + ex.Message + "\", {type: \"error\"});", true);
            }
        }


        #endregion

        #region ingresos brutos
        protected void btnActualizarIngrBrutos_Click(object sender, EventArgs e)
        {
            try
            {
                //actualiza el padron del cuit actual
                int i = -1;
                if (this.accion == 3 || this.accion == 4)
                {
                    i = this.controlador.actualizarPadronCliente(this.idCliente, this.txtCuit.Text.Replace("-", ""), 2);//1 = padron clientes, 2 = padron proveedores
                }
                else
                {
                    i = this.controlador.actualizarPadronCliente(this.idCliente, this.txtCuit.Text.Replace("-", ""), 1);//1 = padron clientes, 2 = padron proveedores
                }

                if (i >= 0)
                {
                    //bien
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ingresos Brutos Actualizado\", {type: \"info\"});", true);
                    if (this.accion == 2)
                    {
                        //cargo datos de IIBB
                        this.cargarIBBCliente();
                    }
                    if (this.accion == 4)
                    {
                        //cargo datos de IIBB
                        this.cargarIBBProveedor();
                    }
                }
                else
                {
                    //error
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Ingresos Brutos\", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error actualizando Ingresos Brutos" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        #endregion

        #region datos mail cumpleaños sms
        private void modificarDatosCliente(Cliente cliente)
        {
            try
            {
                var datosMail = this.contClienteEntity.obtenerClienteDatosByCliente(cliente.id);
                if (datosMail != null)
                {
                    if (datosMail.Count > 0)
                    {
                        datosMail.FirstOrDefault().Mail = this.txtMailEntrega.Text;
                        datosMail.FirstOrDefault().Celular = this.txtCodArea.Text + "-" + this.txtCelularSMS.Text;
                        datosMail.FirstOrDefault().AplicaDescuentoCantidad = Convert.ToInt32(this.ListDescuentoPorCantidad.SelectedValue);
                        if (!String.IsNullOrEmpty(this.txtFechaNacimientoSMS.Text))
                        {
                            datosMail.FirstOrDefault().FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoSMS.Text, new CultureInfo("es-AR"));
                        }
                        this.contClienteEntity.modificarClienteDatos(datosMail.FirstOrDefault());
                    }
                    else
                    {
                        Cliente_Datos datos = new Cliente_Datos();
                        datos.Mail = this.txtMailEntrega.Text;
                        datos.IdCliente = cliente.id;
                        datos.Celular = this.txtCodArea.Text + "-" + this.txtCelularSMS.Text;
                        datos.AplicaDescuentoCantidad = Convert.ToInt32(this.ListDescuentoPorCantidad.SelectedValue);
                        if (!String.IsNullOrEmpty(this.txtFechaNacimientoSMS.Text))
                        {
                            datos.FechaNacimiento = Convert.ToDateTime(this.txtFechaNacimientoSMS.Text, new CultureInfo("es-AR"));
                        }
                        datosMail.Add(datos);
                        this.contClienteEntity.agregarClienteDatos(datos);
                    }

                    this.verificarAgregarTareaAvisoCumpleanios(datosMail.FirstOrDefault());
                }
            }
            catch
            {

            }
        }
        private void verificarAgregarTareaAvisoCumpleanios(Gestion_Api.Entitys.Cliente_Datos datos)
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();
                ControladorTareas contTareas = new ControladorTareas();
                ControladorConfiguracion contConfig = new ControladorConfiguracion();

                string origen = WebConfigurationManager.AppSettings.Get("OrigenSMS");
                string telefono = "+549" + this.txtCodArea.Text + this.txtCelularSMS.Text;//+54 9 cod + tel

                if (this.txtCodArea.Text.Length + this.txtCelularSMS.Text.Length != 10)
                {
                    return;
                }

                Gestion_Api.Entitys.Configuraciones_SMS configAvisos = contConfig.ObtenerConfiguracionesAlertasSMS();
                //var asunto = contSMS.obtenerAsuntoSMSNombre("CUMPLEAÑOS");

                if (configAvisos.AlertaCumpleanios.Value == 1)
                {
                    var tarea = contTareas.obtenerTareaByClienteAsunto(datos.IdCliente.Value.ToString(), "CUMPLEAÑOS");
                    if (tarea == null)//si no hay ninguna agregada aun
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego Tarea SMS por aviso cumpleaños.");

                        string fc = datos.FechaNacimiento.Value.ToString("dd/MM/");
                        fc += DateTime.Today.Year.ToString();

                        contTareas.agregarTareaClienteNuevo(datos.IdCliente.Value, telefono, fc, configAvisos.MensajeCumpleanios, origen);
                    }
                    else
                    {
                        var listTareas = contTareas.obtenerTareasByClienteAsunto(datos.IdCliente.Value.ToString(), "CUMPLEAÑOS");
                        foreach (var t in listTareas)
                        {
                            string fc = datos.FechaNacimiento.Value.ToString("dd/MM/") + t.Fecha.Value.Year;
                            t.Fecha = Convert.ToDateTime(fc, new CultureInfo("es-AR")).AddHours(10);
                        }
                        contTareas.modificarTareas(listTareas);
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        #region sistema millas

        private void cargarDatosMillas()
        {
            try
            {
                ControladorSocios contSocios = new ControladorSocios();

                string sistema = WebConfigurationManager.AppSettings.Get("Millas");

                if (!String.IsNullOrEmpty(sistema))
                {
                    //this.linkMillas.Visible = true;
                    string cuitDoc = this.txtCuit.Text;
                    string dniReal = "";
                    if (cuit.Length >= 11)//cuit con guiones
                    {
                        dniReal = cuitDoc.Split('-')[1];
                    }
                    else//dni solo
                    {
                        dniReal = cuitDoc.PadLeft(8, '0');
                    }

                    var existe = contSocios.obtenerSocioByDoc(dniReal);
                    if (existe != null)
                    {
                        this.PanelMillas.Visible = true;
                        this.ListSiMillas.SelectedValue = "1";
                        this.txtNroSocioMillas.Text = existe.Id.ToString().PadLeft(6, '0');
                        this.txtNroTarjetaMillas.Text = existe.Tarjetas.FirstOrDefault().Numero.ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        this.ListSiMillas.SelectedValue = "0";
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnAgregarClienteMillas_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListSiMillas.SelectedValue == "1")
                {
                    this.agregarClienteMillas();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void agregarClienteMillas()
        {
            try
            {
                ControladorSocios contSocios = new ControladorSocios();
                string cuitDoc = this.txtCuit.Text;
                string dniReal = "";
                if (cuitDoc.Length >= 11)//cuit con guiones
                {
                    dniReal = cuitDoc.Split('-')[1];
                }
                else//dni solo
                {
                    dniReal = cuitDoc.PadLeft(8, '0');
                }
                var existe = contSocios.obtenerSocioByDoc(dniReal);
                if (existe == null)//si no existe
                {
                    //Millas_Api.Entity.Socio socio = new Millas_Api.Entity.Socio();
                    //socio.Telefono = "+549" + this.txtCodArea.Text + this.txtCelularSMS.Text;
                    //socio.Documento = dniReal;
                    //int ok = contSocios.agregrarSocio(socio);
                    //if (ok > 0)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Guardados con exito.\", {type: \"info\"});", true);
                    //    this.cargarDatosMillas();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"No se pudo guardar.\", {type: \"error\"});", true);
                    //}
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Ya es socio del sistema de millas!.\");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel10, UpdatePanel10.GetType(), "alert", "$.msgbox(\"Ocurrio un error. " + ex.Message + ".\", {type: \"error\"});", true);
            }
        }
        #endregion

        #region Eventos Cliente CRM

        /// <summary>
        /// Este metodo carga los eventos CRM del cliente.
        /// </summary>
        private void cargarEventosCliente()
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se va a cagar el CRM del cliente");
                this.phEventos.Controls.Clear();

                List<Clientes_Eventos> eventos = this.contClienteEntity.obtenerEventosClienteByClienteReducido(this.idCliente);

                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                foreach (var e in eventos)
                {
                    string estado = controladorClienteEntity.OtenerDescripcionEventoClienteById((int)e.Estado);

                    this.cargarEventosClientePH(e, estado);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Error al cargar CRM del cliente. Ubicacion: ClientesABM.cargarEventosCliente. Excepcion: " + ex.Message);
            }
        }

        /// <summary>
        /// Este metodo carga los eventos en la tabla PH para mostrar
        /// </summary>
        /// <param name="e">objeto de tipo Clientes_Eventos</param>
        /// <param name="estado">estado del evento</param>
        private void cargarEventosClientePH(Clientes_Eventos e, string estado)
        {
            try
            {
                controladorUsuario controladorUsuario = new controladorUsuario();
                TableRow tr = new TableRow();

                TableCell celFecha = new TableCell();
                celFecha.Text = e.Fecha.Value.ToString("dd/MM/yyyy");
                tr.Cells.Add(celFecha);

                TableCell celDetalle = new TableCell();
                celDetalle.Text = e.Descripcion;
                celDetalle.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDetalle);

                TableCell cellTarea = new TableCell();
                cellTarea.Text = e.Tarea;
                cellTarea.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(cellTarea);

                if (e.Vencimiento != null)
                {
                    TableCell celVencimiento = new TableCell();
                    celVencimiento.Text = e.Vencimiento.Value.ToString("dd/MM/yyyy");
                    celVencimiento.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celVencimiento);
                }
                else
                {
                    TableCell celVencimiento = new TableCell();
                    celVencimiento.Text = "";
                    celVencimiento.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celVencimiento);
                }

                TableCell Estado = new TableCell();
                Estado.Text = estado;
                Estado.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(Estado);

                TableCell Usuario = new TableCell();
                Usuario.Text = controladorUsuario.obtenerUsuariosID((int)e.Usuario).usuario;
                Usuario.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(Usuario);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditarEvento_" + e.Id;
                btnEditar.CssClass = "btn btn-info";
                btnEditar.Text = "<i class='shortcut-icon icon-pencil'></i>";
                btnEditar.Click += new EventHandler(EditarEvento);
                celAccion.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + e.Id;
                btnEliminar.CssClass = "btn btn-danger";
                //btnEliminar.Attributes.Add("data-toggle", "modal");
                this.btnEliminar.Attributes.Add("onClienClick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnEliminar, null) + ";");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(EliminarEvento);
                celAccion.Controls.Add(btnEliminar);

                tr.Cells.Add(celAccion);

                this.phEventos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Error al cargar CRM del cliente en la tabla PH. Ubicacion: ClientesABM.cargarEventosClientePH. Excepcion: " + ex.Message);
            }
        }

        /// <summary>
        /// Este metodo se llama al hacer click sobre el boton guardar evento. Y diferencia entre modificar o agregar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAgregarEventoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.lblIdEventoCliente.Text);
                if (id == 0)
                {
                    this.agregarEventoCliente();
                }
                else
                {
                    this.modificarEventoCliente();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Error al cargar CRM del cliente en la tabla PH. Ubicacion: ClientesABM.lbtnAgregarEventoCliente_Click. Excepcion: " + ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Ocurrio un error.\", {type: \"error\"});", true);
            }
        }

        /// <summary>
        /// Este metodo procesa la informacion para guardar el evento del cliente y enviarlo por mail si se lo indica.
        /// </summary>
        private void agregarEventoCliente()
        {
            try
            {
                Clientes_Eventos eventos = new Clientes_Eventos();
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Busca al cliente a la BD para obtener la razonSocial e enviarla por mail. Ubicacion: ClientesABM.agregarEventoCliente.");
                cliente cliente = controladorClienteEntity.ObtenerClienteId(this.idCliente);
                eventos.Cliente = cliente.id;
                eventos.Descripcion = this.txtDetalleEvento.Text;
                eventos.Fecha = Convert.ToDateTime(this.txtFechaEvento.Text, new CultureInfo("es-AR"));
                eventos.Usuario = Convert.ToInt32((int)Session["Login_IdUser"]);

                if (chbDisparaTarea.Checked == true)
                {
                    eventos.Tarea = this.txtTarea.Text;
                    eventos.Estado = Convert.ToInt32(drpCRMSituacion.SelectedValue);
                    eventos.Vencimiento = Convert.ToDateTime(this.txtFechaVencimiento.Text, new CultureInfo("es-AR"));
                }
                else
                {
                    eventos.Tarea = "";
                    eventos.Estado = 0;
                    eventos.Vencimiento = null;
                }

                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Voy a agregar el evento a la BD. Ubicacion: ClientesABM.agregarEventoCliente.");
                int ok = this.contClienteEntity.agregarEventoCliente(eventos);

                if (ok > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El evento se agrego a la BD.Voy a mandar el email. Ubicacion: ClientesABM.agregarEventoCliente.");
                    Attachment adjunto = null;
                    string mensajeEnvioMail = "";
                    Usuario usuario = new Usuario();
                    if (FileUpload1.HasFile)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se adjunto un archivo al CRM");
                        HttpPostedFile file = FileUpload1.PostedFile;
                        adjunto = new Attachment(file.InputStream, FileUpload1.FileName);
                    }
                    else
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se adjunto ningun archivo al CRM");
                    usuario = controladorUsuario.obtenerUsuariosID(Convert.ToInt32((int)Session["Login_IdUser"]));
                    if (this.chbEnviarMailCRM.Checked == true)
                    {
                        if (txtEnviarMailCRM.Value.ToString() != "")
                        {
                            if (controladorFunciones.enviarMailCRM(cliente.razonSocial, usuario, txtFechaEvento.Text.ToString(), txtDetalleEvento.Text.ToString(), txtEnviarMailCRM.Value.ToString(), txtTarea.Text.ToString(), drpCRMSituacion.SelectedItem.ToString(), txtFechaVencimiento.Text, adjunto, esCCW) > 0)
                                mensajeEnvioMail = "Correo enviado a " + txtEnviarMailCRM.Value.ToString() + " con exito.";
                            else
                                mensajeEnvioMail = "No se pudo enviar email a " + txtEnviarMailCRM.Value.ToString() + ".";
                        }
                        else
                        {
                            mensajeEnvioMail = "No se pudo enviar email. Texto vacio. Destilde la casilla si no desea enviar CRM por email.";
                        }

                    }

                    this.txtDetalleEvento.Text = "";
                    this.lblIdEventoCliente.Text = "0";
                    this.txtTarea.Text = "";
                    drpCRMSituacion.SelectedIndex = 0;

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Evento guardado con exito! " + mensajeEnvioMail, null));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Evento guardado. " + mensajeEnvioMail + "\", {type: \"info\"});", true);
                    this.cargarEventosCliente();
                    limpiarCamposCRM();
                }
                else
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE. No se pudo agregar el evento. Ubicacion: ClientesABM.agregarEventoCliente.");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlWarning("Atencion", "Disculpe, no se pudo guardar el evento. Por favor, contacte con el area de <strong><a href='/Formularios/Herramientas/Soporte.aspx'>soporte</a></strong>."));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"No se pudo guardar.\");", true);
                }
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "CATCH: No se pudo agregar el evento. Ubicacion: ClientesABM. Metodo: agregarEventoCliente. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        /// <summary>
        /// Este metodo procesa la informacion para editar un evento del cliente y enviarlo por mail si se lo indica.
        /// </summary>
        private void modificarEventoCliente()
        {
            try
            {
                string fecha = this.txtFechaEvento.Text;
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                Clientes_Eventos ev = this.contClienteEntity.obtenerEventosClienteByID(Convert.ToInt32(this.lblIdEventoCliente.Text));
                cliente cliente = controladorClienteEntity.ObtenerClienteId(this.idCliente);
                ev.Descripcion = this.txtDetalleEvento.Text;
                ev.Fecha = Convert.ToDateTime(this.txtFechaEvento.Text, new CultureInfo("es-AR"));
                ev.Usuario = Convert.ToInt32((int)Session["Login_IdUser"]);

                if (chbDisparaTarea.Checked == true)
                {
                    ev.Tarea = this.txtTarea.Text;
                    ev.Estado = Convert.ToInt32(drpCRMSituacion.SelectedValue);
                    ev.Vencimiento = Convert.ToDateTime(this.txtFechaVencimiento.Text, new CultureInfo("es-AR"));
                }
                else
                {
                    ev.Tarea = "";
                    ev.Estado = 0;
                    ev.Vencimiento = null;
                }

                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Voy a agregar el evento a la BD. Ubicacion: ClientesABM.modificarEventoCliente.");
                int ok = this.contClienteEntity.modificarEventoCliente(ev);

                if (ok > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Evento agregado a la BD. Voy a mandar el email. Ubicacion: ClientesABM.modificarEventoCliente.");
                    Attachment adjunto = null;
                    string mensajeEnvioMail = "";
                    Usuario usuario = new Usuario();

                    if (FileUpload1.HasFile)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se adjunto un archivo al CRM");
                        HttpPostedFile file = FileUpload1.PostedFile;
                        adjunto = new Attachment(file.InputStream, FileUpload1.FileName);
                    }
                    else
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "No se adjunto ningun archivo al CRM");

                    usuario = controladorUsuario.obtenerUsuariosID(Convert.ToInt32((int)Session["Login_IdUser"]));
                    if (this.chbEnviarMailCRM.Checked == true)
                    {
                        if (txtEnviarMailCRM.Value.ToString() != "")
                        {
                            if (controladorFunciones.enviarMailCRM(cliente.razonSocial, usuario, txtFechaEvento.Text.ToString(), txtDetalleEvento.Text.ToString(), txtEnviarMailCRM.Value.ToString(), txtTarea.Text.ToString(), drpCRMSituacion.SelectedItem.ToString(), txtFechaVencimiento.Text, adjunto, esCCW) > 0)
                                mensajeEnvioMail = "Correo enviado a " + txtEnviarMailCRM.Value.ToString() + " con exito.";
                            else
                                mensajeEnvioMail = "No se pudo enviar email a " + txtEnviarMailCRM.Value.ToString() + ".";
                        }
                        else
                        {
                            mensajeEnvioMail = "No se pudo enviar email. Texto vacio. Destilde la casilla si no desea enviar CRM por email.";
                        }
                    }
                    this.txtDetalleEvento.Text = "";
                    this.lblIdEventoCliente.Text = "0";
                    this.txtTarea.Text = "";
                    drpCRMSituacion.SelectedIndex = 0;

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Evento modificado con exito", null));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Evento modificado. " + mensajeEnvioMail + "\", {type: \"info\"});", true);
                    this.cargarEventosCliente();
                    limpiarCamposCRM();
                }
                else
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE. No se pudo agregar el evento. Ubicacion: ClientesABM.agregarEventoCliente.");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar el evento"));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"No se pudo modificar evento.\");", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH. No se pudo agregar el evento. Ubicacion: ClientesABM.agregarEventoCliente. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al modificar evento. Avisar a Soporte (Revisar Log)"));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Ocurrio un error.\", {type: \"error\"});", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditarEvento(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID;
                int idEvento = Convert.ToInt32(id.Split('_')[1]);
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Voy a cargar el evento " + idEvento.ToString() + " a editar. Ubicacion: ClientesABM.modificarEventoCliente.");

                Clientes_Eventos ev = this.contClienteEntity.obtenerEventosClienteByID(idEvento);
                if (ev != null)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Evento N# " + idEvento.ToString() + ",datos encontrados. Ubicacion: ClientesABM.modificarEventoCliente.");
                    this.txtFechaEvento.Text = ev.Fecha.Value.ToString("dd/MM/yyyy");
                    this.txtDetalleEvento.Text = ev.Descripcion;
                    this.lblIdEventoCliente.Text = ev.Id.ToString();

                    if (ev.Tarea != null)
                    {
                        divVencimientoTarea.Visible = true;
                        divTarea.Visible = true;
                        divSituacion.Visible = true;
                        chbDisparaTarea.Checked = true;
                        drpCRMSituacion.SelectedIndex = (int)ev.Estado;
                    }

                    this.txtFechaVencimiento.Text = ev.Vencimiento.Value.ToString("dd/MM/yyyy");
                    this.txtTarea.Text = ev.Tarea;
                    drpCRMSituacion.SelectedValue = ev.Estado.ToString();

                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Error al editar evento. Ubicacion: ClientesABM.modificarEventoCliente. Excepcion: " + ex.Message);
            }
        }

        //public void btnSiEventoCliente_Click(object obj, EventArgs eventArgs)
        //{
        //    try
        //    {
        //        int idEvento = Convert.ToInt32(this.txtMovimientoEventoCliente.Text);
        //        Clientes_Eventos ev = this.contClienteEntity.obtenerEventosClienteByID(idEvento);
        //        int i = this.contClienteEntity.eliminarEventoCliente(ev);
        //        if (i > 0)
        //        {
        //            //agrego bien
        //            limpiarCamposCRM();
        //            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja evento cliente id: " + ev.Cliente);
        //            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Evento eliminado.", "../ClientesABM.aspx?accion=2&id=" + ev.Cliente));
        //            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Evento eliminado.\", {type: \"info\"});", true);
        //            this.cargarEventosCliente();
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"No se pudo eliminar.\", {type: \"error\"});", true);
        //            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo eliminar."));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Ocurrio un error.\", {type: \"error\"});", true);
        //        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
        //    }
        //}

        /// <summary>
        /// Este metodo elimina el evento de la BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EliminarEvento(object sender, EventArgs e)
        {
            try
            {
                this.btnEliminar.Attributes.Add("disabled", "disabled");
                string id = (sender as LinkButton).ID;
                int idEvento = Convert.ToInt32(id.Split('_')[1]);
                Clientes_Eventos ev = this.contClienteEntity.obtenerEventosClienteByID(idEvento);

                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Va a eliminar evento N# " + idEvento.ToString());
                int i = this.contClienteEntity.eliminarEventoCliente(ev);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Evento N# " + idEvento.ToString() + " eliminado con exito.");

                    limpiarCamposCRM();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTareas, UpdatePanelTareas.GetType(), "alert", "$.msgbox(\"Evento eliminado\", {type: \"info\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Evento eliminado con exito!", null));
                    this.cargarEventosCliente();
                }
                else
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "ELSE. No pudo eliminar el evento. Ubicacion: ClientesABM.EliminarEvento");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo eliminar el CRM."));

                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH. No pudo eliminar el evento. Ubicacion: ClientesABM.EliminarEvento. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }

        }

        #endregion

        #region plan cuentas

        private void cargarDatosCuentaProveedor()
        {
            try
            {
                var cta = this.contPlanCta.obtenerCuentaContableProveedor(this.idCliente);
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
                if (idCta > 0)
                {
                    int ok = 0;
                    var cta = this.contPlanCta.obtenerCuentaContableProveedor(this.idCliente);
                    if (cta != null)
                    {
                        cta.IdCuentaContable = idCta;
                        ok = this.contPlanCta.modificarCuentaContableProveedor(cta);
                    }
                    else
                    {
                        ok = this.contPlanCta.agregarCuentaContableProveedor(this.idCliente, idCta);
                    }

                    if (ok >= 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Guardado con exito", null));
                        this.cargarDatosCuentaProveedor();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo guardar."));
                    }

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando cta contable a proveedor. " + ex.Message));
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

        #region Cliente empleado
        protected void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                var datos = this.contClienteEntity.obtenerClienteDatosByCliente(this.idCliente).FirstOrDefault();

                int i = 0;
                if (datos != null)
                {
                    datos.Empleado = Convert.ToInt32(this.listEmpleados.SelectedValue);

                    //agrego a la DB
                    //int i = this.contClienteEntity.agregarExpresoACliente(this.idCliente, Convert.ToInt64(this.ddlExpresos.SelectedValue));
                    i = this.contClienteEntity.modificarClienteDatos(datos);
                }
                else
                {
                    Cliente_Datos cd = new Cliente_Datos();
                    cd.IdCliente = this.idCliente;
                    cd.Empleado = Convert.ToInt32(this.listEmpleados.SelectedValue);
                    i = this.contClienteEntity.agregarClienteDatos(cd);
                }


                if (i >= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Empleado Agregado\", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo agregar empleado\", {type: \"error\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar sucursal."));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando empleado." + ex.Message + "\", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sucursal a lista." + ex.Message));
            }
        }

        protected void listEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                controladorEmpleado contEmpleado = new controladorEmpleado();

                var emp = contEmpleado.obtenerEmpleadoID(Convert.ToInt32(this.listEmpleados.SelectedValue));
                this.listEmpleados.SelectedValue = emp.id.ToString();
                this.txtNombreEepleado.Text = emp.nombre;
                this.txtApellidoEmpleado.Text = emp.apellido;
                this.txtDNIEmpleado.Text = emp.dni;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos empleado en el formulario." + ex.Message));
            }
        }



        #endregion

        #region Familia
        private void cargarClientesReferir()
        {
            try
            {
                DataTable dt = this.controlador.obtenerClientesDT();

                this.ListClientesReferir.DataSource = dt;
                this.ListClientesReferir.DataValueField = "Id";
                this.ListClientesReferir.DataTextField = "razonSocial";
                this.ListClientesReferir.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes en lista de clientes para referir. " + Ex.Message));
            }
        }
        protected void lbtnReferirCliente_Click(object sender, EventArgs e)
        {
            try
            {
                Clientes_Referidos cr = new Clientes_Referidos();
                cr.Padre = this.idCliente;
                cr.Hijo = Convert.ToInt32(this.ListClientesReferir.SelectedValue);

                int i = this.contClienteEntity.agregarClienteReferido(cr);
                if (i > 0)
                {
                    this.ListBoxClientesReferidos.Items.Add(this.ListClientesReferir.SelectedItem.Text);
                    this.ListClientesReferir.SelectedIndex = 0;
                    this.cargarReferidosCliente();
                }
                if (i == -2)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel9, UpdatePanel9.GetType(), "alert", "$.msgbox(\"El cliente ya se encuentra referido. " + "\", {type: \"error\"});", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo referir al cliente."));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando cliente referido a lista. Excepción: " + Ex.Message));
            }
        }
        protected void lbtnQuitarReferido_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.contClienteEntity.eliminarClienteReferido(this.idCliente, Convert.ToInt32(this.ListBoxClientesReferidos.SelectedValue));
                if (i > 0)
                {
                    this.ListBoxClientesReferidos.Items.Remove(this.ListBoxClientesReferidos.SelectedItem);
                    this.ListClientesReferir.SelectedIndex = 0;
                    this.cargarReferidosCliente();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo eliminar el cliente referido."));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando el cliente referido. Excepción: " + Ex.Message));
            }
        }
        protected void lbtnBuscarClienteReferir_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtClienteReferir.Text.Replace(' ', '%');
                DataTable dtClientes = this.controlador.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.ListClientesReferir.DataSource = dtClientes;
                this.ListClientesReferir.DataValueField = "id";
                this.ListClientesReferir.DataTextField = "razonSocial";
                this.ListClientesReferir.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes en lista de clientes a referir. " + ex.Message));
            }
        }
        private void cargarReferidosCliente()
        {
            try
            {
                var dt = this.contClienteEntity.obtenerClienteReferido(this.idCliente);
                if (dt != null)
                {

                    this.ListBoxClientesReferidos.DataSource = dt;

                    this.ListBoxClientesReferidos.DataValueField = "idHijo";
                    this.ListBoxClientesReferidos.DataTextField = "Hijo";
                    this.ListBoxClientesReferidos.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));
            }
        }
        private void cargarClientesFamilia()
        {
            try
            {
                var idDistribuidor = (int)Session["Login_Vendedor"];
                var cliente = this.controlador.obtenerClienteID(idDistribuidor);
                DataTable dt = this.contClienteEntity.obtenerLideresPorDistribuidor(idDistribuidor);
                this.DropListFamilia.DataSource = dt;
                this.DropListFamilia.DataValueField = "Id";
                this.DropListFamilia.DataTextField = "razonSocial";
                this.DropListFamilia.DataBind();

                this.DropListFamilia.Items.Insert(0, new ListItem(cliente.alias, cliente.id.ToString()));

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando distribuidores a la lista. Excepcion: " + Ex.Message));
            }
        }

        #endregion

        #region Ganancias
        protected void lbtnAgregarGanancias_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    //El metodo agregarGanancias verifica si ya existe un registro para el proveedor, y si existe, realiza la modificación.Sino, agrega un nuevo registro.
                    if (this.accion == 4)
                        this.agregarGanancias();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando/modificando ganancias a proveedor. Excepción: " + Ex.Message));
            }
        }
        private void agregarGanancias()
        {
            try
            {
                Ganancia g = new Ganancia();
                g.MinimoNoImponible = Convert.ToDecimal(this.txtMinimoNoImponible.Text, CultureInfo.InvariantCulture);
                g.Retencion = Convert.ToDecimal(this.txtRetencionGanancias.Text);
                g.Proveedor = this.idCliente;
                int i = this.contClienteEntity.agregarGanancias(g);
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Se cargaron los datos de ganancias correctamente."));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando ganancias a proveedor. Excepción: " + Ex.Message));
            }
        }
        private void cargarGanancias()
        {
            try
            {
                Ganancia g = new Ganancia();
                g = this.contClienteEntity.obtenerGananciasPorProveedor(this.idCliente);
                if (g != null)
                {
                    this.txtMinimoNoImponible.Text = g.MinimoNoImponible.ToString();
                    this.txtRetencionGanancias.Text = g.Retencion.ToString();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando ganancias de proveedor. Excepción: " + Ex.Message));
            }
        }

        #endregion

        #region Ordenes de Compra
        protected void lbtnAgregarProveedorOC_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    //El metodo agregarProveedor_OC verifica si ya existe un registro para el proveedor, y si existe, realiza la modificación.Sino, agrega un nuevo registro.
                    if (this.accion == 4)
                        this.agregarProveedor_OC();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando/modificando datos Ordenes de Compra del proveedor. Excepción: " + Ex.Message));
            }
        }
        private void agregarProveedor_OC()
        {
            try
            {
                Proveedores_OC poc = new Proveedores_OC();
                poc.Proveedor = this.idCliente;
                poc.Mail = this.txtMailEnvioOC.Text.Trim();
                poc.RequiereAutorizacion = Convert.ToInt32(this.ListRequiereAutorizacionOC.SelectedValue);
                poc.MontoAutorizacion = Convert.ToDecimal(this.txtMontoAutorizacionOC.Text);
                poc.RequiereAnticipo = Convert.ToInt32(this.ListRequiereAnticipoOC.SelectedValue);
                poc.FormaDePago = this.txtFormaDePago_OC.Text;
                int i = this.contClienteEntity.agregarProveedor_OC(poc);
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se cargaron los datos para las Ordenes de Compra correctamente.", null));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos para las Ordenes de Compra a proveedor. Excepción: " + Ex.Message));
            }
        }
        private void cargarProveedor_OC()
        {
            try
            {
                Proveedores_OC poc = new Proveedores_OC();
                poc = this.contClienteEntity.obtenerProveedor_OC_PorProveedor(this.idCliente);
                if (poc != null)
                {
                    this.txtMailEnvioOC.Text = poc.Mail;
                    this.ListRequiereAutorizacionOC.SelectedValue = poc.RequiereAutorizacion.ToString();
                    this.txtMontoAutorizacionOC.Text = poc.MontoAutorizacion.ToString();
                    this.ListRequiereAnticipoOC.SelectedValue = poc.RequiereAnticipo.ToString();
                    this.txtFormaDePago_OC.Text = poc.FormaDePago;
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos para las  Ordenes de Compra de proveedor. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region codigo BTB
        protected void lbtnCodigoBTB_Click(object sender, EventArgs e)//Guarda los nuevos valores de BTB
        {
            try
            {
                Clientes_CodigoBTB cliBTB = new Clientes_CodigoBTB();
                cliBTB.Cliente = this.idCliente;
                cliBTB.CodigoBTB1 = this.txtCodigoBTB1.Text.PadLeft(5, '0');
                cliBTB.CodigoBTB2 = this.txtCodigoBTB2.Text.PadLeft(5, '0');
                int ok = contClienteEntity.generarCodigoBTB(cliBTB);
                if (ok >= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Codigo BTB actualizado con exito.", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando codigo BTB."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("SE ha producido un error en ClientesABM. Metodo: lbtnCodigoBTB_Click. Exception: " + ex.Message));
            }
        }
        #endregion

        #region IngresosBrutos/Percepciones
        [WebMethod]
        public static string AgregarIngresosBrutosYObtenerLosRegistros(string idClienteString, string IdProvincia, string origenCliente, string percepcionORetencion, string modo, string idPlanCuenta)
        {
            try
            {
                int idCliente = Convert.ToInt32(idClienteString);
                int idPlan = Convert.ToInt32(idPlanCuenta);
                int respuesta = 0;
                decimal percepcion = 0;
                decimal retencion = 0;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON;

                List<IIBBTemporal> listaTemporal = new List<IIBBTemporal>();
                ControladorProvincias controladorProvincias = new ControladorProvincias();
                controladorCliente controladorCliente = new controladorCliente();

                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                var cliente = controladorClienteEntity.ObtenerClienteId(Convert.ToInt32(idCliente));
                var prov = controladorProvincias.ObtenerProvinciaById(Convert.ToInt32(IdProvincia));
                if (prov != null)
                {
                    if (Convert.ToInt32(origenCliente) == 1)
                    {
                        percepcion = Convert.ToDecimal(percepcionORetencion);
                    }
                    else
                    {
                        retencion = Convert.ToDecimal(percepcionORetencion);
                    }

                    ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
                    respuesta = contClienteEntity.AgregarIngresosBrutosAlCliente(Convert.ToInt32(idCliente), prov.Id, percepcion, retencion, modo, idPlan);
                }
                resultadoJSON = respuesta.ToString();

                if (respuesta > 0)
                {
                    List<Cliente_IIBB_Provincias> listaIIBB = controladorClienteEntity.ObtenerIngresoBrutosIIBB_Provincia_ByCliente(idCliente);
                    foreach (var item in listaIIBB)
                    {
                        string descripcionPlanCuentas = controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id) != null
                        && controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id).Rows.Count > 0
                        ? controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id).Rows[0][1].ToString()
                        : "Sin Plan";
                        listaTemporal.Add(new IIBBTemporal
                        {
                            Id = item.Id.ToString(),
                            IdCliente = item.IdCliente.ToString(),
                            Provincia = item.Provincia.Provincia1,
                            Percepcion = item.Percepcion.ToString(),
                            Retencion = item.Retencion.ToString(),
                            Modo = item.Modo,
                            PlanCuentas = descripcionPlanCuentas
                        });
                    }
                    resultadoJSON = serializer.Serialize(listaTemporal);
                }
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        [WebMethod]
        public static string ObtenerRegistrosIIBBProvinciaByCliente(string IdCliente)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                controladorCliente controladorCliente = new controladorCliente();

                List<Cliente_IIBB_Provincias> listaIIBB = controladorClienteEntity.ObtenerIngresoBrutosIIBB_Provincia_ByCliente(Convert.ToInt32(IdCliente));
                List<IIBBTemporal> listaTemporal = new List<IIBBTemporal>();
                foreach (var item in listaIIBB)
                {
                    
                    string descripcionPlanCuentas = controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id) != null
                        && controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id).Rows.Count > 0  
                        ? controladorCliente.obtenerPlanCuentaByIdIIBB(item.Id).Rows[0][1].ToString() 
                        : "Sin Plan" ;
                    listaTemporal.Add(new IIBBTemporal
                    {
                        Id = item.Id.ToString(),
                        IdCliente = item.IdCliente.ToString(),
                        Provincia = item.Provincia.Provincia1,
                        Percepcion = item.Percepcion.ToString(),
                        Retencion = item.Retencion.ToString(),
                        Modo = item.Modo,
                        PlanCuentas = descripcionPlanCuentas
                    });
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaTemporal);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string EliminarRegistroIIBBProvincia(string IdIIBBProvincia, string IdCliente)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                int resultado = controladorClienteEntity.EliminarIngresoBrutoDelCliente_IIBB_Provincia(Convert.ToInt32(IdIIBBProvincia));

                List<Cliente_IIBB_Provincias> listaIIBB = controladorClienteEntity.ObtenerIngresoBrutosIIBB_Provincia_ByCliente(Convert.ToInt32(IdCliente));
                List<IIBBTemporal> listaTemporal = new List<IIBBTemporal>();
                foreach (var item in listaIIBB)
                {
                    listaTemporal.Add(new IIBBTemporal
                    {
                        Id = item.Id.ToString(),
                        IdCliente = item.IdCliente.ToString(),
                        Provincia = item.Provincia.Provincia1,
                        Percepcion = item.Percepcion.ToString(),
                        Retencion = item.Retencion.ToString(),
                        Modo = item.Modo
                    });
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaTemporal);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected void DropListIva_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DropListIva.SelectedValue == "1")
                {
                    txtCuit.MaxLength = 20;
                }
                else
                {
                    txtCuit.MaxLength = 11;
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ubicacion en ClientesABM.DropListIva_SelectedIndexChanged .Excepcion: " + ex.Message);
            }
        }

        protected void lbtnBuscarNiveles_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorPlanCuentas controladorPlanCuentas = new ControladorPlanCuentas();
                //Articulo art = this.controlador.obtenerArticuloCodigo(busqueda);
                List<Cuentas_Contables> dtPlanCuentas = controladorPlanCuentas.BusquedaUltimoNivelByDescripcion(5,txtBusqueda.Text);

                if (dtPlanCuentas != null)
                {

                    this.ListPlanCuentas.DataSource = dtPlanCuentas;
                    this.ListPlanCuentas.DataValueField = "id";
                    this.ListPlanCuentas.DataTextField = "Descripcion";

                    this.ListPlanCuentas.DataBind();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra plan de cuenta " + txtBusqueda.Text));
                }
            }
            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando niveles de plan de cuentas. " + ex.Message));

            }
        }

        protected void lbtnNivelBusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorPlanCuentas controladorPlanCuentas = new ControladorPlanCuentas();
                //Articulo art = this.controlador.obtenerArticuloCodigo(busqueda);
                List<Cuentas_Contables> dtPlanCuentas = controladorPlanCuentas.BusquedaUltimoNivelByDescripcion(5,txtBusqueda.Text);

                if (dtPlanCuentas != null)
                {

                    this.ListPlanCuentas.DataSource = dtPlanCuentas;
                    this.ListPlanCuentas.DataValueField = "id";
                    this.ListPlanCuentas.DataTextField = "Descripcion";

                    this.ListPlanCuentas.DataBind();

                }
                else
                {
                }
            }
            catch (Exception ex)
            {


            }
        }



        #endregion

        //protected void rdSiNo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Log.EscribirSQL(1, "ERROR", "Tilde una opcion en el CRM");
        //    try
        //    {
        //        string file = null;

        //        if (FileUpload1.HasFile)
        //        {
        //            file = FileUpload1.FileName.ToString();
        //        }

        //        if (RadButtonNo.Checked == true)
        //        {
        //            divTarea.Visible = true;
        //            divVencimientoTarea.Visible = true;
        //            divSituacion.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "ERROR", "ERROR CATCH rdSiNo_SelectedIndexChanged");
        //    }

        //}

        //protected void RadButton1_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string file = null;

        //        if (FileUpload1.HasFile)
        //        {
        //            file = FileUpload1.FileName.ToString();
        //        }
        //        RadButtonNo.Checked = false;
        //        RadButtonSi.Attributes.Add("onClick", "javascript: validateRadButtons()");

        //        //divTarea.Visible = true;
        //        //divVencimientoTarea.Visible = true;
        //        //divSituacion.Visible = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "ERROR", "ERROR CATCH rdSiNo_SelectedIndexChanged. Mensaje: " + ex.Message);
        //    }
        //}

        //protected void RadButton2_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        RadButtonSi.Checked = false;
        //        string file = null;

        //        if (FileUpload1.HasFile)
        //        {
        //            file = FileUpload1.FileName.ToString();
        //        }
        //        RadButtonNo.Attributes.Add("onClick", "javascript: validateRadButtons()");
        //        //divTarea.Visible = false;
        //        //divVencimientoTarea.Visible = false;
        //        //divSituacion.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "ERROR", "ERROR CATCH rdSiNo_SelectedIndexChanged. Mensaje:" + ex.Message);
        //    }
        //}

        //protected void chbDisparaTarea_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string file = null;

        //        if (FileUpload1.HasFile)
        //        {
        //            file = FileUpload1.FileName.ToString();
        //        }
        //        RadButtonNo.Attributes.Add("onClick", "javascript: validateRadButtons()");
        //        //divTarea.Visible = false;
        //        //divVencimientoTarea.Visible = false;
        //        //divSituacion.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "ERROR", "ERROR CATCH rdSiNo_SelectedIndexChanged. Mensaje:" + ex.Message);
        //    }
        //}
    }
}