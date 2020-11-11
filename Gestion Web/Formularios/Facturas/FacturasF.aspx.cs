using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Gestion_Api.Entitys;
using System.Globalization;
using System.Web.Configuration;
using Gestion_Api.Modelo.Enums;
using System.Diagnostics;
using Task_Api.Entitys;
using Gestion_Api.Controladores.ControladoresEntity;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class FacturasF : System.Web.UI.Page
    {
        controladorFacturacion controlador = new controladorFacturacion();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorRemitos contRemito = new controladorRemitos();
        controladorCliente contCliente = new controladorCliente();
        controladorFactEntity contFactEntity = new controladorFactEntity();
        controladorSucursal cs = new controladorSucursal();
        controladorFunciones contFunciones = new controladorFunciones();
        ControladorEmpresa controlEmpresa = new ControladorEmpresa();
        ControladorFormasPago contFormPago = new ControladorFormasPago();
        controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();
        ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();

        Mensajes m = new Mensajes();
        private int accion;
        private string numeroFactura = string.Empty;
        private string razonSocialCliente = string.Empty;
        private string observacion = string.Empty;
        private int suc;
        private string fechaD;
        private string fechaH;
        private int tipo;
        private int cliente;
        private int tipoCliente;
        private int tipofact;
        private int lista;
        private int anuladas;
        private int empresa;
        private int vendedor;
        private int formaPago;
        private int editarPago;
        DataTable lstPagosTemp;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                this.VerificarLogin();
                //datos de filtro
                accion = Convert.ToInt32(Request.QueryString["a"]);
                numeroFactura = Request.QueryString["n"];
                razonSocialCliente = Request.QueryString["rz"];
                observacion = Request.QueryString["o"];
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                empresa = Convert.ToInt32(Request.QueryString["Emp"]);
                tipo = Convert.ToInt32(Request.QueryString["tipo"]);
                tipofact = Convert.ToInt32(Request.QueryString["doc"]);
                cliente = Convert.ToInt32(Request.QueryString["cl"]);
                tipoCliente = Convert.ToInt32(Request.QueryString["tc"]);
                lista = Convert.ToInt32(Request.QueryString["ls"]);
                anuladas = Convert.ToInt32(Request.QueryString["e"]);
                vendedor = Convert.ToInt32(Request.QueryString["vend"]);
                formaPago = Convert.ToInt32(Request.QueryString["fp"]);


                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.verificarModoBlanco();

                    txtTotalPago.Style["text-align"] = "right";
                    txtRestaPago.Style["text-align"] = "right";
                    txtImportePago.Style["text-align"] = "right";
                    btnAgregarPago.Attributes.Add("disabled", "true");
                    //Verifico si tiene habilitado el boton Facturar PRP según la configuracion
                    this.verificarFacturarPRP();

                    if (fechaD == null && fechaH == null && suc == 0 && cliente == 0 && empresa == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        empresa = (int)Session["Login_EmpUser"];
                        this.cargarEmpresas();
                        this.cargarSucursalByEmpresa(empresa);
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        //tipo de documento??
                        tipo = 0;
                        tipofact = 0;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesdeDto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHastaDto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListEmpresa.SelectedValue = empresa.ToString();
                        ListEmpresaDto.SelectedValue = empresa.ToString();
                        this.cargarSucursal();
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListTipo.SelectedValue = tipo.ToString();

                        lstPagosTemp = new DataTable();
                        InicializarListaPagos();

                    }
                    this.cargarEmpresas();
                    this.cargarSucursalByEmpresa(empresa);
                    this.cargarPuntosVenta(Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue));

                    this.cargarClientes();
                    this.cargarDropListTipoCliente();
                    this.cargarListaPrecio();
                    this.cargarFormaPago();
                    this.cargarChkListaPrecio();
                    this.cargarVendedores();
                    this.CargarListaProveedoresPatentamiento();

                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                    DropListEmpresa.SelectedValue = empresa.ToString();
                    DropListTipo.SelectedValue = tipo.ToString();
                    DropListDocumento.SelectedValue = tipofact.ToString();
                    DropListClientes.SelectedValue = cliente.ToString();
                    DropListTipoCliente.SelectedValue = tipoCliente.ToString();
                    DropListListas.SelectedValue = lista.ToString();
                    DropListVendedor.SelectedValue = vendedor.ToString();
                    DropListFormasPago.SelectedValue = formaPago.ToString();
                    this.chkAnuladas.Checked = Convert.ToBoolean(this.anuladas);
                }

                //ACCION 1: Busca por numero de factura
                if (accion == 1)
                {
                    this.BuscarPorNumeroFactura();
                }
                //ACCION 2: Busca por razon social
                if (accion == 2)
                {
                    this.BuscarPorRazonSocial();
                }
                //ACCION 3: Busca por observacion/comentario.
                if (accion == 3)
                {
                    this.BuscarPorObservacion();
                }

                btnAgregarFC.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarFC, null) + ";");
                btnCalcularDiferenciaCambio.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnCalcularDiferenciaCambio, null) + ";");
                btnGenerarNotaDebitoCreditoDiferenciaCambio.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnGenerarNotaDebitoCreditoDiferenciaCambio, null) + ";");

                this.cargarTablaPAgos();
                //verifico si el perfil tiene permiso para anular
                this.verficarPermisoAnular();
                //verifico si el perfil tiene permiso para editar FC y agregar FC
                this.verificarPermisoEditarFC();
                this.verificarPermisoAgregarFC();
                this.cargarFacturasRango(fechaD, fechaH, suc, tipo, cliente, tipofact, lista, empresa, vendedor, formaPago, tipoCliente,0);
                this.Form.DefaultButton = this.btnBuscarCod.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        #region carga inicial
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
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "39")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                                this.DropListEmpresa.Attributes.Remove("disabled");
                                this.ListEmpresaDto.Attributes.Remove("disabled");
                                this.ListSucursalesDto.Attributes.Remove("disabled");
                            }
                            else
                            {
                                if (perfil == "Vendedor")
                                {
                                    //deshabilito el list de vendedor
                                    this.vendedor = (int)Session["Login_Vendedor"];
                                    this.DropListVendedor.Attributes.Add("disabled", "true");
                                    this.DropListVendedor.SelectedValue = this.vendedor.ToString();
                                    this.txtCodCliente.Visible = false;
                                    this.btnBuscarCod.Visible = false;
                                }

                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                int i = this.verficarPermisoCambiarSucursal();
                                if (i <= 0)
                                {
                                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                    this.DropListEmpresa.SelectedValue = Session["Login_EmpUser"].ToString();
                                    this.ListEmpresaDto.SelectedValue = Session["Login_EmpUser"].ToString();
                                    this.ListSucursalesDto.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                                this.btnExportarVentasSucursal.Visible = false;
                                this.btnImprimirVentasSucursal.Visible = false;
                            }
                            valor = 1;
                        }

                        //Permiso ver saldo
                        if (s == "117")
                            this.labelSaldo.Visible = true;

                        //Permiso Generar Orden de Reparacion
                        if (s == "162")
                            this.lbtnOrdenReparacion.Visible = true;

                        //Permiso boton Nota de Credito seleccionando PRP
                        string permisoNotaCreditoDesdePRP = listPermisos.Where(x => x == "148").FirstOrDefault();
                        if (string.IsNullOrEmpty(permisoNotaCreditoDesdePRP))
                            this.lbtnNotaCredito.Visible = false;

                        if (s == "174")
                            lbtnRefacturar.Visible = true;

                        if (s == "175")
                            lbtnRemitir.Visible = true;

                        if (s == "202")
                            lbtnPatentamiento.Visible = true;
                    }
                }
                if (!listPermisos.Contains("203"))//permiso filtrar prp
                {
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("Ambos"));
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("PRP"));
                }
                if (listPermisos.Contains("208"))//permiso habilitar accion ND/NC diferencia de cambio
                {
                    this.phNotaDebitoCreditoDiferenciaCambio.Visible = true;
                }
                return valor;
            }
            catch
            {
                return -1;
            }
        }
        public void verificarModoBlanco()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.modoBlanco == "1")
                {
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("Ambos"));
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("PRP"));
                }
            }
            catch
            {

            }
        }
        public void verificarFacturarPRP()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.FacturarPRP == "0")
                {
                    this.lbtnRefacturar.Visible = false;
                }
            }
            catch
            {

            }
        }
        public int verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "102")//Permiso.Ventas.CambioSucursal
                        {
                            this.DropListSucursal.Attributes.Remove("disabled");
                            this.DropListEmpresa.Attributes.Remove("disabled");
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
        public void verficarPermisoAnular()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "76")
                        {
                            this.lbtnAnular.Visible = true;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();
                string perfil = Session["Login_NombrePerfil"] as string;

                if (perfil == "Vendedor")
                {
                    int v = (int)Session["Login_Vendedor"];//id vendedor
                    dt = contCliente.obtenerClientesByVendedorDT(v);
                }
                else
                {
                    dt = contCliente.obtenerClientesDT();
                }

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

                this.ListClientePresupuesto.DataSource = dt;
                this.ListClientePresupuesto.DataValueField = "id";
                this.ListClientePresupuesto.DataTextField = "alias";

                this.ListClientePresupuesto.DataBind();

                if (this.cliente > 0)//agrego al usuario mismo como cliente?? 
                {
                    Gestor_Solution.Modelo.Cliente cl = contCliente.obtenerClienteID(cliente);
                    if (cl != null)
                    {
                        ListItem item = new ListItem(cl.alias, cl.id.ToString());
                        if (this.DropListClientes.Items.Contains(item) == false)
                        {
                            DropListClientes.Items.Add(item);
                        }
                        if (this.ListClientePresupuesto.Items.Contains(item) == false)
                        {
                            ListClientePresupuesto.Items.Add(item);
                        }
                    }
                }

                //Lleno lista de clientes del modal Editar FC
                this.ListClientesEditar.DataSource = dt;
                this.ListClientesEditar.DataValueField = "id";
                this.ListClientesEditar.DataTextField = "alias";

                this.ListClientesEditar.DataBind();

                //Lleno lista de clientes del modal Agregar FC
                this.DropListClienteAgregarFC.DataSource = dt;
                this.DropListClienteAgregarFC.DataValueField = "id";
                this.DropListClienteAgregarFC.DataTextField = "alias";

                this.DropListClienteAgregarFC.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        public void cargarDropListTipoCliente()
        {
            try
            {
                controladorTipoCliente contTipoCliente = new controladorTipoCliente();
                DataTable dt = contTipoCliente.obtenerTiposClientes();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["tipo"] = "Todos";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListTipoCliente.DataSource = dt;
                this.DropListTipoCliente.DataValueField = "id";
                this.DropListTipoCliente.DataTextField = "tipo";
                this.DropListTipoCliente.DataBind();

                this.ListTipoClientePresupuesto.DataSource = dt;
                this.ListTipoClientePresupuesto.DataValueField = "id";
                this.ListTipoClientePresupuesto.DataTextField = "tipo";
                this.ListTipoClientePresupuesto.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargarDropListTipoCliente. " + ex.Message));
            }
        }
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListEmpresa.DataSource = dt;
                this.DropListEmpresa.DataValueField = "Id";
                this.DropListEmpresa.DataTextField = "Razon Social";
                this.DropListEmpresa.DataBind();

                this.ListEmpresaDto.DataSource = dt;
                this.ListEmpresaDto.DataValueField = "Id";
                this.ListEmpresaDto.DataTextField = "Razon Social";
                this.ListEmpresaDto.DataBind();

                this.DropListEmpresaAgregarFC.DataSource = dt;
                this.DropListEmpresaAgregarFC.DataValueField = "Id";
                this.DropListEmpresaAgregarFC.DataTextField = "Razon Social";
                this.DropListEmpresaAgregarFC.DataBind();


                this.ListEmpresaPresupuesto.DataSource = dt;
                this.ListEmpresaPresupuesto.DataValueField = "Id";
                this.ListEmpresaPresupuesto.DataTextField = "Razon Social";
                this.ListEmpresaPresupuesto.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
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

                DataRow dr1 = dt.NewRow();
                dr1["nombre"] = "Todas";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 1);

                this.ListSucursalesDto.DataSource = dt;
                this.ListSucursalesDto.DataValueField = "Id";
                this.ListSucursalesDto.DataTextField = "nombre";
                this.ListSucursalesDto.DataBind();

                //this.DropListSucursal.DataSource = dt;
                //this.DropListSucursal.DataValueField = "Id";
                //this.DropListSucursal.DataTextField = "nombre";

                //this.DropListSucursal.DataBind();

                this.DropListSucursalAgregarFC.DataSource = dt;
                this.DropListSucursalAgregarFC.DataValueField = "Id";
                this.DropListSucursalAgregarFC.DataTextField = "nombre";
                this.DropListSucursalAgregarFC.DataBind();


                this.ListSucursalPresupuesto.DataSource = dt;
                this.ListSucursalPresupuesto.DataValueField = "Id";
                this.ListSucursalPresupuesto.DataTextField = "nombre";
                this.ListSucursalPresupuesto.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                // agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr1 = dt.NewRow();
                dr1["nombre"] = "Todas";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 1);

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.DropListSucursalAgregarFC.DataSource = dt;
                this.DropListSucursalAgregarFC.DataValueField = "Id";
                this.DropListSucursalAgregarFC.DataTextField = "nombre";
                this.DropListSucursalAgregarFC.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarSucursalPresupuesto(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                // agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr1 = dt.NewRow();
                dr1["nombre"] = "Todas";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 1);

                this.ListSucursalPresupuesto.DataSource = dt;
                this.ListSucursalPresupuesto.DataValueField = "Id";
                this.ListSucursalPresupuesto.DataTextField = "nombre";
                this.ListSucursalPresupuesto.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntosVenta(int idSucursal)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(idSucursal);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListPuntoVentaAgregarFC.DataSource = dt;
                this.DropListPuntoVentaAgregarFC.DataValueField = "Id";
                this.DropListPuntoVentaAgregarFC.DataTextField = "NombreFantasia";

                this.DropListPuntoVentaAgregarFC.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se cargaron los puntos de venta. " + Ex.Message));
            }
        }
        public void CargarListaProveedoresPatentamiento()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerProveedoresPatentamientoDT();

                //agrego todos
                if (dt != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["razonSocial"] = "Seleccione";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    this.dropDownListProveedoresPatentamiento.DataSource = dt;
                    this.dropDownListProveedoresPatentamiento.DataValueField = "id";
                    this.dropDownListProveedoresPatentamiento.DataTextField = "razonSocial";

                    this.dropDownListProveedoresPatentamiento.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        public void cargarListaPrecio()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerListaPrecios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListListas.DataSource = dt;
                this.DropListListas.DataValueField = "id";
                this.DropListListas.DataTextField = "nombre";

                this.DropListListas.DataBind();

                this.ListPrecioPresupuesto.DataSource = dt;
                this.ListPrecioPresupuesto.DataValueField = "id";
                this.ListPrecioPresupuesto.DataTextField = "nombre";
                this.ListPrecioPresupuesto.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        public void cargarFormaPago()
        {
            try
            {
                DataTable dt = this.controlador.obtenerFormasPago();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["forma"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListFormasPago.DataSource = dt;
                this.DropListFormasPago.DataValueField = "id";
                this.DropListFormasPago.DataTextField = "forma";

                this.DropListFormasPago.DataBind();


                this.ListFormaPagoPresupuesto.DataSource = dt;
                this.ListFormaPagoPresupuesto.DataValueField = "id";
                this.ListFormaPagoPresupuesto.DataTextField = "forma";

                this.ListFormaPagoPresupuesto.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }
        public void cargarVendedores()
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();
                this.DropListVendedor.Items.Clear();
                this.DropListVendedorAgregarFC.Items.Clear();
                //agrego todos
                DataRow dr3 = dt.NewRow();
                dr3["nombre"] = "Todos";
                dr3["id"] = 0;
                dt.Rows.InsertAt(dr3, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    this.DropListVendedor.Items.Add(item);
                    this.DropListVendedorAgregarFC.Items.Add(item);
                    this.ListVendedorPresupuesto.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        private void cargarIvaClientes()
        {
            try
            {
                this.ListIvaFact.DataSource = this.contCliente.obtenerIvaClientes();
                this.ListIvaFact.DataValueField = "id";
                this.ListIvaFact.DataTextField = "descripcion";

                this.ListIvaFact.DataBind();
                this.ListIvaFact.Items.Remove(this.ListIvaFact.Items.FindByText("No Informa"));
                ListItem item = new ListItem("Seleccione...", "-1");
                this.ListIvaFact.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
            }
        }
        private void cargarProvincias()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListProvinciaFact.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvinciaFact.DataValueField = "Provincia";
                this.ListProvinciaFact.DataTextField = "Provincia";

                this.ListProvinciaFact.DataBind();
                //cargo la localidad
                this.cargarLocalidades(this.ListProvinciaFact.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  provincias. " + ex.Message));
            }
        }
        private void cargarChkListaPrecio()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerListaPrecios();

                foreach (DataRow lista in dt.Rows)
                {
                    if (lista["nombre"].ToString() != "Seleccione...")
                    {
                        ListItem item = new ListItem(lista["nombre"].ToString(), lista["id"].ToString());
                        this.chkListListas.Items.Add(item);
                        int i = this.chkListListas.Items.IndexOf(item);
                        this.chkListListas.Items[i].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        private void cargarLocalidades(string provincia)
        {
            try
            {
                this.ListLocalidadFact.Items.Clear();

                controladorPais controladorPais = new controladorPais();
                this.ListLocalidadFact.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidadFact.DataValueField = "Localidad";
                this.ListLocalidadFact.DataTextField = "Localidad";

                this.ListLocalidadFact.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de  localidades. " + ex.Message));
            }
        }
        private void cargarFacturasBySuc(int idSuc)
        {
            try
            {
                List<Factura> Facturas = controlador.obtenerFacturas();

                foreach (Factura f in Facturas)
                {
                    this.cargarEnPh(f);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }
        private void cargarFacturasRango(string fechaD, string fechaH, int idSuc, int tipo, int idCliente, int tipofact, int idLista, int idEmp, int idVendedor, int formaPago, int idTipoCliente,int PRPFacturados)
        {
            try
            {
                DataTable dtFacturas = controlador.obtenerFacturasRangoTipoDTLista(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListDocumento.SelectedValue), Convert.ToInt32(DropListListas.SelectedValue), this.anuladas, Convert.ToInt32(DropListEmpresa.SelectedValue), Convert.ToInt32(DropListVendedor.SelectedValue), Convert.ToInt32(DropListFormasPago.SelectedValue), Convert.ToInt32(DropListTipoCliente.SelectedValue),PRPFacturados);
                decimal saldo = 0;

                foreach (DataRow row in dtFacturas.Rows)
                {
                    this.cargarEnPhDR(row);
                    if (row["estado"].ToString() == "1")
                    {
                        saldo += Convert.ToDecimal(row["total"].ToString());
                    }
                }
                labelSaldo.Text = "$" + saldo.ToString("N");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando facturas.  " + ex.Message));
            }
        }
        private void cargarLabel(string fechaD, string fechaH, int idSuc, int tipo)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSuc > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSuc.ToString()).Text + ",";
                }
                if (tipo > -1)
                {
                    label += DropListTipo.Items.FindByValue(tipo.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }
        private void cargarFacturas()
        {
            try
            {
                List<Factura> Facturas = controlador.obtenerFacturas();
                foreach (Factura f in Facturas)
                {
                    this.cargarEnPh(f);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }
        private void cargarEnPh(Factura f)
        {
            try
            {
                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");
                string restaHoras;

                if (Convert.ToInt32(modificoHora) == 1)
                    restaHoras = WebConfigurationManager.AppSettings.Get("HorasDiferencia");

                //fila
                TableRow tr = new TableRow();
                tr.ID = f.id.ToString();

                int estaRefact = this.contFactEntity.verificarRefacturado(f.id);
                if (estaRefact > 0)
                {
                    tr.ForeColor = System.Drawing.Color.DarkGreen;
                    tr.Font.Bold = true;
                }

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

                TableCell celRazon = new TableCell();
                celRazon.Text = f.cliente.razonSocial;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);
                //si estoy cargando una nota de credito
                if (f.tipo.tipo.Contains("Credito"))
                {
                    f.netoNGrabado = f.netoNGrabado * -1;
                    f.neto21 = f.neto21 * -1;
                    f.retencion = f.retencion * -1;
                    f.subTotal = f.subTotal * -1;
                    f.total = f.total * -1;
                }

                TableCell celNeto = new TableCell();
                celNeto.Text = "$" + f.subTotal;
                //celNeto.Text = "$" + f.netoNGrabado;
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNeto);

                TableCell celIva = new TableCell();
                celIva.Text = "$" + f.neto21;
                celIva.VerticalAlign = VerticalAlign.Middle;
                celIva.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celIva);

                TableCell celPercepcion = new TableCell();
                celPercepcion.Text = "$" + f.retencion;
                celPercepcion.VerticalAlign = VerticalAlign.Middle;
                celPercepcion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPercepcion);

                TableCell celPercepcionIvaCF = new TableCell();
                celPercepcionIvaCF.Text = "$" + f.iva10;
                celPercepcionIvaCF.VerticalAlign = VerticalAlign.Middle;
                celPercepcionIvaCF.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPercepcionIvaCF);

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
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + f.id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                if (f.estado == 1)
                {
                    tr.Cells.Add(celAccion);
                }
                else
                {
                    TableCell celEstado = new TableCell();
                    celEstado.Text = "*Anulada*";
                    celEstado.VerticalAlign = VerticalAlign.Middle;
                    celEstado.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(celEstado);

                    tr.ForeColor = System.Drawing.Color.Red;
                }

                phFacturas.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }

        }

        private bool ExisteFilaAndTieneDatos(DataRow fila, string nombreCampo)
        {
            try
            {
                if (fila.Table.Columns.Contains(nombreCampo))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Ocurrió un error en ExisteFila de controladorArticulos. Excepción: " + ex.Message);
                return false;
            }
        }
        private void cargarEnPhDR(DataRow row)
        {
            try
            {
                int idFactura = Convert.ToInt32(row["id"]);
                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");

                //fila
                TableRow tr = new TableRow();
                tr.ID = Convert.ToInt32(row["id"]).ToString();

                int estaRefact = this.contFactEntity.verificarRefacturado(Convert.ToInt32(row["id"]));
                if (estaRefact > 0)
                {
                    tr.ForeColor = System.Drawing.Color.DarkGreen;
                    tr.Font.Bold = true;
                }

                if (ExisteFilaAndTieneDatos(row, "patentamiento"))
                {
                    if (!string.IsNullOrEmpty(row["patentamiento"].ToString()) && Convert.ToInt32(row["patentamiento"]) == 1)
                    {
                        tr.ForeColor = System.Drawing.Color.Violet;
                        tr.Font.Bold = true;
                    }
                }

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(row["fecha"].ToString()).ToString("dd/MM/yyyy hh:mm");
                if (modificoHora == "1")
                {
                    string restaHoras = WebConfigurationManager.AppSettings.Get("HorasDiferencia");
                    var fechaAux = Convert.ToDateTime(row["fecha"].ToString());
                    celFecha.Text = fechaAux.AddHours(Convert.ToInt32(restaHoras)).ToString("dd/MM/yyyy hh:mm");
                }
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                celTipo.Text = row["tipo"].ToString();
                celTipo.HorizontalAlign = HorizontalAlign.Center;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);


                TableCell celNumero = new TableCell();
                celNumero.Text = row["numero"].ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = row["razonSocial"].ToString();
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);
                //si estoy cargando una nota de credito
                if (row["tipo"].ToString().Contains("Credito"))
                {
                    row["netoNoGrabado"] = Convert.ToDecimal(row["netoNoGrabado"]) * -1;
                    row["neto21"] = Convert.ToDecimal(row["neto21"]) * -1;
                    row["retenciones"] = Convert.ToDecimal(row["retenciones"]) * -1;
                    row["iva10"] = Convert.ToDecimal(row["iva10"]) * -1;
                    row["subtotal"] = Convert.ToDecimal(row["subtotal"]) * -1;
                    row["total"] = Convert.ToDecimal(row["total"]) * -1;
                }

                TableCell calSumaIIBB_Provincias = new TableCell();
                decimal porcentajeSumaIIBB = contFactEntity.ObtenerSumaIngresosBrutosDeLasProvinciasByFactura(idFactura);
                decimal neto = Convert.ToDecimal(row["subtotal"]);
                calSumaIIBB_Provincias.Text = "$ 0.00";
                if (porcentajeSumaIIBB > 0)
                {
                    calSumaIIBB_Provincias.Text = "$ " + Math.Round(porcentajeSumaIIBB * neto / 100, 2).ToString();
                }
                calSumaIIBB_Provincias.VerticalAlign = VerticalAlign.Middle;
                calSumaIIBB_Provincias.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(calSumaIIBB_Provincias);

                TableCell celNeto = new TableCell();
                celNeto.Text = "$" + row["subtotal"].ToString();
                //celNeto.Text = "$" + f.netoNGrabado;
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNeto);

                TableCell celIva = new TableCell();
                celIva.Text = "$" + row["neto21"].ToString();
                celIva.VerticalAlign = VerticalAlign.Middle;
                celIva.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celIva);

                TableCell celPercepcion = new TableCell();
                celPercepcion.Text = "$" + row["retenciones"].ToString();
                celPercepcion.VerticalAlign = VerticalAlign.Middle;
                celPercepcion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPercepcion);

                TableCell celPercepcionIvaCF = new TableCell();
                celPercepcionIvaCF.Text = "$" + row["iva10"].ToString();
                celPercepcionIvaCF.VerticalAlign = VerticalAlign.Middle;
                celPercepcionIvaCF.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPercepcionIvaCF);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + row["total"].ToString();
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + row["id"].ToString() + "_" + row["tipo_doc"].ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + row["id"].ToString();
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                if (row["estado"].ToString() == "1")
                {
                    tr.Cells.Add(celAccion);
                }
                else
                {
                    TableCell celEstado = new TableCell();
                    celEstado.Text = "*Anulada*";
                    celEstado.VerticalAlign = VerticalAlign.Middle;
                    celEstado.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(celEstado);

                    tr.ForeColor = System.Drawing.Color.Red;
                }

                phFacturas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }

        }
        #endregion

        #region Eventos Controles
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }
        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idEmpresa = Convert.ToInt32(this.DropListEmpresa.SelectedValue);
                this.cargarSucursalByEmpresa(idEmpresa);
            }
            catch
            {

            }
        }
        protected void ListEmpresaPresupuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idEmpresa = Convert.ToInt32(this.ListEmpresaPresupuesto.SelectedValue);
                this.cargarSucursalPresupuesto(idEmpresa);
            }
            catch
            {

            }
        }
        protected void ListEmpresaDto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                int idEmpresa = Convert.ToInt32(this.ListEmpresaDto.SelectedValue);
                if (idEmpresa > 0)
                {
                    DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                    // agrego todos
                    DataRow dr = dt.NewRow();
                    dr["nombre"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr1 = dt.NewRow();
                    dr1["nombre"] = "Todas";
                    dr1["id"] = 0;
                    dt.Rows.InsertAt(dr1, 1);

                    this.ListSucursalesDto.DataSource = dt;
                    this.ListSucursalesDto.DataValueField = "Id";
                    this.ListSucursalesDto.DataTextField = "nombre";

                    this.ListSucursalesDto.DataBind();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        Response.Redirect("FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&tc=" + DropListTipoCliente.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de facturas. " + ex.Message));

            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int user = (int)Session["Login_IdUser"];

                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int okCierre = this.verificarCierreCaja(Convert.ToInt32(idtildado.Split(';')[0]));
                    if (okCierre < 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede anular ya que se realizo un cierre de caja en el dia de hoy para el punto de venta de este documento."));
                        return;
                    }

                    int i = this.controlador.ProcesoEliminarFactura(idtildado, user);
                    if (i > 0)
                    {
                        Gestion_Api.Modelo.Log.EscribirSQL(user, "INFO", "ANULACION factura id: " + idtildado);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Facturas anuladas con exito. ", "FacturasF.aspx"));
                    }
                    else
                    {
                        bool mensaje = false;
                        if (i == -2)
                        {
                            mensaje = true;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron anular las facturas seleccionadas ya que una de ellas registra cancelaciones."));

                        }
                        if (i == -4)
                        {
                            mensaje = true;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede anular una factura Fiscal o Electronica. "));
                        }
                        if (i == -1)
                        {
                            mensaje = true;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Facturas. "));
                        }
                        if (i == -5)
                        {
                            mensaje = true;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Uno de los remitos de las facturas seleccionadas no se pudo anular. Por favor revise los remitos correspondientes. "));
                        }
                        if (i == -6)
                        {
                            mensaje = true;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede anular una factura de movimiento entre sucursales. "));
                        }
                        if (!mensaje)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Facturas. "));
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Documento"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando documentos para anular. " + ex.Message));
            }
        }
        protected void btnSiFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                int user = (int)Session["Login_IdUser"];
                string idtildado = this.lblIdFact.Text;

                if (!String.IsNullOrEmpty(idtildado))
                {
                    Factura fact = this.controlador.obtenerFacturaId(Convert.ToInt32(idtildado));
                    int ok = this.controlador.verificarRefacturarProveedor(fact);
                    if (ok < 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede refacturar porque uno o más articulos tienen proveedor con condicion IVA NO INFORMA. "));
                        return;
                    }

                    int modCliente = this.actualizarDatosRefacturarCliente();

                    if (modCliente < 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar datos del cliente a facturar. "));
                        return;
                    }


                    var idClienteAux = 0;
                    if (!string.IsNullOrEmpty(this.listClienteFacturarPRP.SelectedValue))
                    {
                        idClienteAux = Convert.ToInt32(this.listClienteFacturarPRP.SelectedValue);
                    }

                    int i = this.controlador.ProcesoRefacturarPRP(null,idtildado, user, idClienteAux);//this.controlador.ProcesoEliminarFactura(idtildado, user);
                    if (i > 0)
                    {
                        this.restablecerIvaCliente();
                        Gestion_Api.Modelo.Log.EscribirSQL(user, "INFO", "REFACTURAR presupuesto id: " + idtildado);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "FacturasF.aspx"));
                    }
                    else
                    {
                        if (i == -2)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede facturar Presupuesto a cliente con tipo iva: No informado o Cuit 00-00000000-0. "));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo facturar Presupuesto!. "));
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Presupuesto"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }
        protected void lbtnRefacturar_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargarIvaClientes();
                this.cargarProvincias();

                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true && tr.Cells[1].Text.Contains("Presupuesto"))
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    if (this.controlador.obtenerNotasCreditoFactura(Convert.ToInt32(idtildado)))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Este presupuesto posee una Nota de Credito!. "));
                        return;
                    }
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Inicio REFACTURAR presupuesto id: " + idtildado);
                    this.lblIdFact.Text = idtildado;
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Obtengo prp desde la base");
                    Factura prpFC = this.controlador.obtenerFacturaId(Convert.ToInt32(idtildado));
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "obtengo datos cliente");
                    prpFC.cliente = this.contCliente.obtenerClienteID(prpFC.cliente.id);//le cargo todos los datos
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo id");
                    this.lblIdCliente.Text = prpFC.cliente.id.ToString();
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo tipo y num prp");
                    this.lblNroFact.Text = prpFC.tipo.tipo + " " + prpFC.numero;
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo alias clientes");
                    this.lblCliente.Text = prpFC.cliente.alias;
                    //this.ListIvaFact.SelectedValue = this.ListIvaFact.Items.FindByText(prpFC.cliente.iva).Value;
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo cuit clientes" + prpFC.cliente.cuit);
                    this.txtCUITfact.Text = prpFC.cliente.cuit;
                    if (prpFC.cliente.direcciones != null)
                    {

                        if (prpFC.cliente.direcciones.Count > 0)
                        {
                            Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo direcciones");
                            this.txtDirFact.Text = prpFC.cliente.direcciones.FirstOrDefault().direc;
                            this.lblIdDirFact.Text = prpFC.cliente.direcciones.FirstOrDefault().id.ToString();
                            this.ListProvinciaFact.SelectedValue = this.ListProvinciaFact.Items.FindByText(prpFC.cliente.direcciones.FirstOrDefault().provincia).Value;
                            this.cargarLocalidades(this.ListProvinciaFact.SelectedValue);
                            this.ListLocalidadFact.SelectedValue = this.ListLocalidadFact.Items.FindByText(prpFC.cliente.direcciones.FirstOrDefault().localidad).Value;
                        }
                    }
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Cargo neto no grabado" + prpFC.netoNGrabado.ToString("'$'#,0.00"));
                    this.lblNetoFact.Text = prpFC.netoNGrabado.ToString("'$'#,0.00");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Cargo neto 21" + prpFC.neto21.ToString("'$'#,0.00"));
                    this.lblIvaFact.Text = prpFC.neto21.ToString("'$'#,0.00");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Cargo total" + prpFC.total.ToString("'$'#,0.00"));
                    this.lblTotalFact.Text = prpFC.total.ToString("'$'#,0.00");
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal", "openModal();", true);

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un Presupuesto!. "));
                }
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error generando Factura desde PRP " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error generando Factura desde PRP " + ex.Message));
            }
        }
        protected void lbtnGuiaDespacho_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("GuiaDespachoF.aspx?accion=6&f=" + idtildado);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para despachar. " + ex.Message));
            }
        }
        protected void lbImpresion_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de facturas. " + ex.Message));

            }
        }
        protected void lbtnNotaCredito_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                    if (idtildado.Split(';').Count() > 1 && idtildado.Split(';')[1] != "")
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar solo una Factura"));
                        return;
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("ABMFacturas.aspx?accion=6&facturas=" + idtildado);
                }
                else
                {
                    Response.Redirect("ABMFacturas.aspx?accion=7");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para facturar. " + ex.Message));
            }
        }
        protected void lbtnRemitir_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "" && id != null)
                        {
                            Factura f = new Factura();
                            f = this.controlador.obtenerFacturaId(Convert.ToInt32(id));
                            if (f != null)
                            {
                                int i = this.contRemito.RemitirDesdeFactura(f);

                                if (i < 1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Remito desde factura. "));
                                    return;
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo factura a remitir. "));
                                return;
                            }
                        }
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "FacturasF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un documento!. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error remitiendo facturas. " + ex.Message));
            }
        }
        protected void btnSiEditarPRP_Click(object sender, EventArgs e)
        {
            try
            {
                int modCliente = this.actualizarDatosRefacturarCliente();

                if (modCliente < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar datos del cliente a facturar. "));
                    return;
                }

                string idtildado = this.lblIdFact.Text;
                if (!String.IsNullOrEmpty(idtildado))
                {
                    //por si modifica el cliente
                    Response.Redirect("ABMFacturas?accion=9&prps=" + idtildado + "&prpsc=" + this.listClienteFacturarPRP.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Presupuesto"));
                }

            }
            catch
            {

            }
        }
        protected void ListProvinciaFact_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarLocalidades(this.ListProvinciaFact.SelectedValue);
            }
            catch
            {

            }
        }
        protected void lbtnEnviarMail_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var idFactura in this.txtIdEnvioFC.Text.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(idFactura))
                    {
                        Factura factura = this.controlador.obtenerFacturaId(Convert.ToInt32(idFactura));
                        if (factura != null)
                        {
                            string destinatarios = this.txtEnvioMail.Text + ";" + this.txtEnvioMail2.Text;
                            String path = Server.MapPath("../../Facturas/" + factura.empresa.id + "/" + "/fc-" + factura.numero + "_" + factura.id + ".pdf");
                            //string path = Server.MapPath("C:\\"+ "fc - 0001 - 00000946_3565.pdf");

                            int i = this.GenerarImpresionPDF(factura, path);
                            if (i > 0)
                            {
                                Attachment adjunto = new Attachment(path);

                                int ok = this.contFunciones.enviarMailFactura(adjunto, factura, destinatarios);
                                if (ok > 0)
                                {
                                    adjunto.Dispose();
                                    File.Delete(path);
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar factura por mail. "));
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar impresion factura a enviar. "));
                            }
                        }
                    }
                }
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura/s enviada correctamente!", ""));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando factura por mail. " + ex.Message));
            }
        }

        protected void lbtnEnviar_Click(object sender, EventArgs e)
        {
            asignarMailsDeClienteAlTextBoxEnvioMail();
        }
        private void asignarMailsDeClienteAlTextBoxEnvioMail()
        {
            try
            {
                ControladorClienteEntity contCliEnt = new ControladorClienteEntity();

                this.txtEnvioMail.Text = "";
                string idsListaFacturasTildados = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idsListaFacturasTildados += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idsListaFacturasTildados))
                {
                    this.txtIdEnvioFC.Text = idsListaFacturasTildados;
                    var idsFacturas = idsListaFacturasTildados.Split(';');
                    foreach (var idFactura in idsFacturas)
                    {
                        if (!string.IsNullOrWhiteSpace(idFactura))
                        {
                            var factura = this.controlador.obtenerFacturaId(Convert.ToInt32(idFactura));
                            if (factura != null)
                            {
                                var clienteDatos = contCliEnt.obtenerClienteDatosByCliente(factura.cliente.id);
                                if (clienteDatos != null)
                                {
                                    foreach (var dato in clienteDatos)
                                    {
                                        if (!string.IsNullOrWhiteSpace(dato.Mail) && !txtEnvioMail.Text.Contains(dato.Mail))
                                        {
                                            this.txtEnvioMail.Text += dato.Mail + ";";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalMail", "openModalMail();", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un documento!. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando factura para envio via mail. " + ex.Message));
            }
        }
        protected void lbtnReimprimirFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                int total = 0;
                int impresas = 0;
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                        total++;
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (string id in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            Factura fc = this.controlador.obtenerFacturaId(Convert.ToInt32(id));
                            if (fc != null)
                            {
                                if (fc.ptoV.formaFacturar == "Fiscal")
                                {
                                    int puede = this.controlador.verificarImpresoFiscal(fc.id);
                                    if (puede > 0)
                                    {
                                        int ok = this.controlador.imprimirFactura(fc);
                                        if (ok > 0)
                                        {
                                            impresas++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (impresas > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se enviaron a imprimir " + impresas + " de " + total + " facturas.", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar impresion."));
                    }

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Seleccione al menos un documento."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error. " + ex.Message));
            }
        }
        protected void btnEditarDatosFC_Click(object sender, EventArgs e)
        {
            try
            {
                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");

                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true && !tr.Cells[1].Text.Contains("Presupuesto") && !tr.Cells[1].Text.Contains("Nota de Credito PRP"))
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    this.lblIdFact.Text = idtildado;
                    Factura FC = this.controlador.obtenerFacturaId(Convert.ToInt32(idtildado));
                    this.lblIdFactura.Text = FC.id.ToString();
                    this.lblNroFactura.Text = FC.numero;
                    this.lblNetoFactura.Text = FC.subTotal.ToString("'$'#,0.00");
                    this.lblIvaFactura.Text = FC.neto21.ToString("'$'#,0.00");
                    this.lblTotalFactura.Text = FC.total.ToString("'$'#,0.00");
                    this.txtNuevoPuntoVenta.Text = FC.numero.Substring(0, 4);
                    this.txtNuevoNumeroFactura.Text = FC.numero.Substring(5, 8);

                    if (modificoHora == "1")
                    {
                        string restaHoras = WebConfigurationManager.AppSettings.Get("HorasDiferencia");
                        this.lblFechaFactura.Text = FC.fecha.AddHours(Convert.ToInt32(restaHoras)).ToString("dd/MM/yyyy hh:mm");
                        this.txtNuevaFecha.Text = FC.fecha.AddHours(Convert.ToInt32(restaHoras)).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        this.lblFechaFactura.Text = FC.fecha.ToString("dd/MM/yyyy hh:mm");
                        this.txtNuevaFecha.Text = FC.fecha.ToString("dd/MM/yyyy");
                    }

                    this.txtNuevoNetoFactura.Text = FC.subTotal.ToString();
                    this.txtNuevoIvaFactura.Text = FC.neto21.ToString();
                    this.txtNuevoTotalFactura.Text = FC.total.ToString();
                    ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "openModalEditarFactura", "openModalEditarFactura();", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una Factura! "));
                }
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error generando Factura desde Editar Factura " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error generando Factura desde Editar Factura " + ex.Message));
            }
        }
        protected void btnClienteFacturarPRP_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtClienteFacturarPRP.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.listClienteFacturarPRP.DataSource = dtClientes;
                this.listClienteFacturarPRP.DataValueField = "id";
                this.listClienteFacturarPRP.DataTextField = "alias";
                this.listClienteFacturarPRP.DataBind();

                this.lblCliente.Text = this.listClienteFacturarPRP.SelectedItem.Text;
                this.lblIdCliente.Text = this.listClienteFacturarPRP.SelectedItem.Value;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        protected void listClienteFacturarPRP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblCliente.Text = this.listClienteFacturarPRP.SelectedItem.Text;
                this.lblIdCliente.Text = this.listClienteFacturarPRP.SelectedItem.Value;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error seleccionando cliente. " + ex.Message));
            }
        }
        protected void btnForm12_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "" && id != null)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=4&Presupuesto=" + id + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "FacturasF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un documento!. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error remitiendo facturas. " + ex.Message));
            }
        }
        protected void lbtnImprimirTicketDeCambio_Click(object sender, EventArgs e)
        {
            try
            {
                controladorFacturacion contFacturacion = new controladorFacturacion();
                Factura factura = new Factura();
                int idTildado = 0;
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idTildado = Convert.ToInt32(ch.ID.Split('_')[1]);
                        break;
                    }
                }
                if (idTildado != 0)
                {
                    factura = contFacturacion.obtenerFacturaId(idTildado);
                    int resultado = contFacturacion.generarXMLTicketDeCambio(factura);
                    if (resultado > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Ticket de cambio impreso correctamente.", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo imprimir Ticket de cambio."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe tildar una factura antes."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: lbtnImprimirTicketDeCambio_Click. " + ex.Message));
            }
        }
        #endregion

        #region exportacion

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(1);
        }
        protected void btnImprimirIvaVentas_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(1);
        }

        protected void btnExportarIIBB_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(2);
        }
        protected void btnImprimirIIBB_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(2);
        }

        protected void btnExportarDetalle2_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(3);
        }
        protected void btnImprimirDetalle2_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(3);
        }

        protected void btnExportarVentas_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(4);
        }
        protected void btnImprimirVentas_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(4);
        }
        protected void btnExportarVentasVendedor_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(9);
        }
        protected void btnImprimirVentasVendedor_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(9);
        }
        protected void btnImprimirReporteVentasVendedor_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(12);
        }
        protected void btnExportarReporteVentasVendedor_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(12);
        }
        protected void btnExportarVentasSucursal_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(5);
        }
        protected void btnImprimirVentasSucursal_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(5);
        }

        protected void btnExportarVentasListaP_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(6);
        }
        protected void btnImprimirVentasListaP_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(6);
        }
        protected void btnCitiVentas_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(7);
        }
        protected void btnPercepciones_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(8);
        }
        protected void btnImprimirDescuentos_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(10);
        }
        protected void btnExportarDescuentos_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(10);
        }

        protected void btnExportarPantalla_Click(object sender, EventArgs e)
        {
            try
            {
                this.ExportToExcel(11);
            }
            catch
            {

            }
        }
        protected void btnImprimirPantalla_Click(object sender, EventArgs e)
        {
            try
            {
                this.PrintToPDF(11);
            }
            catch
            {

            }
        }
        protected void lbtnImprimirTodo_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("pdfs/");

                //limpio la carpeta donde van los pdfs asi no muestra pdfs viejos
                BorrarPDFS(path);

                string idtildado = "";
                int contadorOk = 0;
                int contadorTotal = 0;

                //chequeo lo que este tildado y lo imprimo
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;

                    if (ch.Checked)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                        contadorTotal++;
                    }
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (string id in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(id));
                            string fileName = "fc-" + f.numero + "_" + f.id + ".pdf";
                            int i = this.GenerarImpresionPDF(f, path + fileName);
                            if (i > 0)
                            {
                                contadorOk++;
                            }
                        }
                    }
                }
                //si no tengo tildada ninguna factura ejecuto la busqueda en la base y genero un pdf de todas
                else
                {
                    DataTable dtFacturas = controlador.obtenerFacturasRangoTipoDTLista(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListDocumento.SelectedValue), Convert.ToInt32(DropListListas.SelectedValue), this.anuladas, Convert.ToInt32(DropListEmpresa.SelectedValue), Convert.ToInt32(DropListVendedor.SelectedValue), Convert.ToInt32(DropListFormasPago.SelectedValue));

                    foreach (DataRow row in dtFacturas.Rows)
                    {
                        var idfactura = row["id"];
                        idfactura = Convert.ToInt32(idfactura);
                        Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(idfactura));
                        string fileName = "fc-" + f.numero + "_" + f.id + ".pdf";
                        int i = this.GenerarImpresionPDF(f, path + fileName);
                        if (i > 0)
                        {
                            contadorOk++;
                        }
                    }

                }

                string[] pdfs = Directory.GetFiles(path);
                string nombre = path + "fc-" + DateTime.Now.ToString("dd-MM-yyyy_hhmmss") + ".pdf";
                int ok = this.contFunciones.CombineMultiplePDFs(pdfs, nombre);
                if (ok > 0)
                {
                    this.descargar(nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Realizados con exito:" + contadorOk + "de " + contadorTotal, ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo imprimir"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        public void BorrarPDFS(string path)
        {
            string[] pdfs = Directory.GetFiles(path);

            foreach (string filePath in pdfs)
            {
                File.Delete(filePath);
            }
        }

        private void PrintToPDF(int accion)
        {
            try
            {
                int anuladas = 0;
                if (this.chkAnuladas.Checked == true)
                {
                    anuladas = 1;
                }
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        if (accion == 1)
                        {
                            string fi = WebConfigurationManager.AppSettings.Get("FormatoIva");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=1&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fo=" + fi + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 2)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=2&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 3)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=3&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 4)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=4&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 5)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=5&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&anuladas=" + anuladas + "&Emp=" + DropListEmpresa.SelectedValue + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 6)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=6&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 7)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=7&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 8)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=8&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 9)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=10&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 10)
                        {
                            string listas = "";
                            foreach (ListItem lista in chkListListas.Items)
                            {
                                if (lista.Selected == true)
                                {
                                    listas += lista.Value + ",";
                                }
                            }
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=11&fechadesde=" + txtFechaDesdeDto.Text + "&fechaHasta=" + txtFechaHastaDto.Text + "&Sucursal=" + ListSucursalesDto.SelectedValue + "&Emp=" + ListEmpresaDto.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&anuladas=" + anuladas + "&l=" + listas + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 11)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=12&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&anuladas=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 12)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=13&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 14)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=14&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch
            {

            }
        }
        protected void ExportToExcel(int accion)
        {
            try
            {
                int anuladas = 0;
                if (this.chkAnuladas.Checked == true)
                {
                    anuladas = 1;
                }
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        if (accion == 1)
                        {
                            string fi = WebConfigurationManager.AppSettings.Get("FormatoIva");
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=1&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fo=" + fi + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 2)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=2&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 3)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=3&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 4)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=4&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 5)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=5&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&e=1" + "&anuladas=" + anuladas + "&Emp=" + DropListEmpresa.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 6)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=6&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&e=1" + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionFacturas.aspx?a=6&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&anuladas=" + anuladas + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                        }
                        if (accion == 7)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=7&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 8)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=8&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 9)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=10&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 10)
                        {
                            string listas = "";
                            foreach (ListItem lista in chkListListas.Items)
                            {
                                if (lista.Selected == true)
                                {
                                    listas += lista.Value + ",";
                                }
                            }
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=11&fechadesde=" + txtFechaDesdeDto.Text + "&fechaHasta=" + txtFechaHastaDto.Text + "&Sucursal=" + ListSucursalesDto.SelectedValue + "&Emp=" + ListEmpresaDto.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&l=" + listas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 11)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=12&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&anuladas=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue) + "&e=1");
                        }
                        if (accion == 12)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=13&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        if (accion == 14)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=14&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&e=1" + "&anuladas=" + anuladas + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                        }
                        
                    }
                   
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de facturas. " + ex.Message));

            }
        }
        private void ExportToExcelPresupuesto(int accion)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesdePresupuesto.Text) && (!String.IsNullOrEmpty(txtFechaHastaPresupuesto.Text)))
                {

                    if (ListSucursalPresupuesto.SelectedValue != "-1")
                    {
                        if (accion == 15)
                        {
                            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=15&fechadesde=" + txtFechaDesdePresupuesto.Text + "&fechaHasta=" + txtFechaHastaPresupuesto.Text + "&Sucursal=" + ListSucursalPresupuesto.SelectedValue + "&Emp=" + ListEmpresaPresupuesto.SelectedValue + "&tipo=0" + "&doc=0" + "&cl=" + ListClientePresupuesto.SelectedValue + "&e=1" + "&anuladas=0" + "&vend=" + Convert.ToInt32(this.ListVendedorPresupuesto.SelectedValue) + "&fp=" + Convert.ToInt32(this.ListFormaPagoPresupuesto.SelectedValue),true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }

            }
               
            catch (Exception ex)
            {
                Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "error", ex.Message.ToString());
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de facturas. " + ex.Message));
            }
           
        }
        private void descargar(string path)
        {
            try
            {
                System.IO.FileInfo toDownload =
                     new System.IO.FileInfo(path);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition",
                           "attachment; filename=" + toDownload.Name);
                HttpContext.Current.Response.AddHeader("Content-Length",
                           toDownload.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.WriteFile(path);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", " Error descargando excel de facturacion. " + ex.Message);

            }
        }
        private int GenerarImpresionPDF(Factura f, string pathGenerar)
        {
            try
            {
                Configuracion configuracion = new Configuracion();

                if (f.tipo.tipo.Contains("Factura A") || f.tipo.tipo.Contains("Debito A") || f.tipo.tipo.Contains("Credito A")
                || f.tipo.tipo.Contains("Factura E") || f.tipo.tipo.Contains("Debito E") || f.tipo.tipo.Contains("Credito E"))
                {
                    #region Fact A/E
                    //obtengo detalle de items
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);

                    //datos de encabezado y pie
                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    //nro remito factura
                    DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(f.id);

                    //Factura fact = controlador.obtenerFacturaId(idPresupuesto);

                    //datos del emisor
                    String razonSoc = String.Empty;
                    String direComer = String.Empty;
                    String condIVA = String.Empty;
                    String ingBrutos = String.Empty;
                    String fechaInicio = String.Empty;
                    String cuitEmpresa = String.Empty;
                    String nroFactura = String.Empty;
                    String tipoDoc = String.Empty;
                    String letraDoc = String.Empty;
                    String CodigoDoc = String.Empty;
                    String CAE = String.Empty;
                    String ptoVta = String.Empty;
                    String codBarra = String.Empty;
                    String fechaVto = string.Empty;
                    String cotizacionFecha = String.Empty;

                    //levanto los datos de la factura
                    var drDatosFactura = dtDetalle.Rows[0];
                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }
                    //datos cotizacion al momento de fc
                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                    {
                        cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                    }
                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        controladorSucursal contSuc = new controladorSucursal();
                        Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }


                    //datos empresa emisora
                    DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                    foreach (DataRow row in dtEmpresa.Rows)
                    {
                        //verifico cual es la empresa de la factura
                        //if ((int)row[0] == )
                        //{
                        cuitEmpresa = row.ItemArray[1].ToString();
                        razonSoc = row.ItemArray[2].ToString();
                        ingBrutos = row.ItemArray[3].ToString();
                        fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();
                        //fechaInicio = Convert.ToDateTime(fechaInicio).ToShortDateString();
                        condIVA = row.ItemArray[5].ToString();
                        direComer = row.ItemArray[6].ToString();
                        //}
                    }

                    //datos factura
                    string auxNro = drDatosFactura["Numero"].ToString();
                    nroFactura = auxNro.Substring(auxNro.Length - 13, 13);
                    //nombre tipo documento para el parametro
                    tipoDoc = auxNro.Substring(0, auxNro.Length - 16);
                    //letra y cod. factura                
                    if (tipoDoc == "Factura E ")
                    {
                        letraDoc = "E";
                        CodigoDoc = "Cod. 19";
                    }
                    else
                    {
                        letraDoc = "A";
                        CodigoDoc = "Cod. 01";
                    }

                    if (drDatosFactura["Cae"] != null)
                    {
                        CAE = drDatosFactura["Cae"].ToString();
                        //CAE = "-";
                    }
                    else
                    {
                        CAE = "-";
                    }
                    ptoVta = drDatosFactura["ptoVenta"].ToString();
                    fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                    codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                    if (string.IsNullOrEmpty(codBarra))
                    {
                        codBarra = "0";

                    }

                    //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                    PuntoVenta pv = this.cs.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                    int esPreimpresa = 0;
                    if (pv != null)
                    {
                        if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                        {
                            esPreimpresa = 1;
                        }
                    }

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];

                    //neto no grabado
                    decimal subtotal = Convert.ToDecimal(dr[4]);

                    decimal descuento = Convert.ToDecimal(dr[1]);

                    //subtotal menos el descuento
                    decimal subtotal2 = Convert.ToDecimal(dr[5]);

                    //iva discriminado de la fact
                    decimal iva = Convert.ToDecimal(dr[3]);

                    //IIBB (retenciones)
                    decimal retencion = Convert.ToDecimal(dr["retenciones"]);

                    //total de la factura
                    decimal total = Convert.ToDecimal(dr[2]);

                    //letras
                    string totalS = Gestion_Api.Auxiliares.Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                    //string totalS = Numalet.ToCardinal("18.25");

                    //cant unidades
                    decimal cant = 2;

                    //Total equivalente en dolares
                    controladorMoneda contMoneda = new controladorMoneda();
                    Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                    decimal TotalDolares = 0;
                    String textoDolares = String.Empty;
                    if (dolar != null)
                    {
                        TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                    }
                    if (tipoDoc.Contains("Nota de"))
                    {
                        textoDolares = " ";
                    }
                    else
                    {
                        textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                    }

                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    if (dtDireccion != null)
                    {
                        foreach (DataRow drl in dtDireccion.Rows)
                        {
                            if (drl[0].ToString() == "Legal")
                            {
                                direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                    drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                            if (drl[0].ToString() == "Entrega")
                            {
                                direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                    drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                        }
                    }


                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //obtengo id empresa para buscar el logo correspondiente
                    int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                    string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/Logo.jpg");
                    Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);
                    BarcodeProfessional bcp = new BarcodeProfessional();

                    //Barcode settings
                    bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;
                    bcp.BarHeight = 0.25f;
                    bcp.Code = codBarra;

                    byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                    DataTable dtImagen = new DataTable();

                    DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));

                    dtImagen.Columns.Add(ColumnImagen);
                    dtImagen.Rows.Add(prodBarcode);
                    //Generate the barcode image and store it into the Barcode Column

                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    if (letraDoc == "A")
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                    }
                    if (letraDoc == "E")
                    {
                        //letras
                        totalS = Gestion_Api.Auxiliares.Numalet.ToCardinal(subtotal2.ToString().Replace(',', '.'));
                        if (CAE == "-")
                        {
                            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE_2.rdlc");
                        }
                        else
                        {
                            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRE.rdlc");
                        }

                    }
                    //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaR.rdlc");
                    this.ReportViewer1.LocalReport.EnableExternalImages = true;

                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("DatosFactura", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                    ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                    ReportParameter param31 = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                    //logo
                    //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///C:\Imagen\Logo.jpg");
                    ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                    Log.EscribirSQL(1, "INFO", @"Asigno Ruta file:///" + logo);

                    //string imagePath = Server.MapPath("~/images/Facturas/GS_LOGO.png");
                    //ReportParameter paramImg = new ReportParameter("ParamImagen", imagePath);

                    ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                    ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                    ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                    ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                    ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                    ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                    ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                    ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                    ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                    ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                    ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                    ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                    ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                    ReportParameter param19 = new ReportParameter("ParamPtoVta", ptoVta);
                    ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);
                    ReportParameter param21 = new ReportParameter("ParamLetra", letraDoc);
                    ReportParameter param22 = new ReportParameter("ParamCodDoc", CodigoDoc);
                    ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);
                    ReportParameter param24 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());

                    ReportParameter param25 = new ReportParameter("ParamCambioDolar", cotizacionFecha);


                    //ReportParameter param16 = new ReportParameter("ParamRazonSoc", nroFactura);


                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param03);//sucfacturada
                    this.ReportViewer1.LocalReport.SetParameters(param31);
                    this.ReportViewer1.LocalReport.SetParameters(param4);

                    this.ReportViewer1.LocalReport.SetParameters(param3b);
                    this.ReportViewer1.LocalReport.SetParameters(param4b);

                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);

                    this.ReportViewer1.LocalReport.SetParameters(param7);
                    this.ReportViewer1.LocalReport.SetParameters(param8);

                    //parametros datos empresa
                    this.ReportViewer1.LocalReport.SetParameters(param10);
                    this.ReportViewer1.LocalReport.SetParameters(param11);
                    this.ReportViewer1.LocalReport.SetParameters(param12);
                    this.ReportViewer1.LocalReport.SetParameters(param13);
                    this.ReportViewer1.LocalReport.SetParameters(param14);
                    this.ReportViewer1.LocalReport.SetParameters(param15);
                    this.ReportViewer1.LocalReport.SetParameters(param16);
                    this.ReportViewer1.LocalReport.SetParameters(param17);
                    this.ReportViewer1.LocalReport.SetParameters(param18);
                    this.ReportViewer1.LocalReport.SetParameters(param19);
                    this.ReportViewer1.LocalReport.SetParameters(param20);
                    this.ReportViewer1.LocalReport.SetParameters(param21);
                    this.ReportViewer1.LocalReport.SetParameters(param22);
                    //equivalente total dolares
                    this.ReportViewer1.LocalReport.SetParameters(param23);
                    this.ReportViewer1.LocalReport.SetParameters(param25);
                    //param esPreimpresa o no
                    this.ReportViewer1.LocalReport.SetParameters(param24);
                    //imagen
                    this.ReportViewer1.LocalReport.SetParameters(param32);

                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    //String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");

                    FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    #endregion
                }
                if (f.tipo.tipo.Contains("Factura B") || f.tipo.tipo.Contains("Debito B") || f.tipo.tipo.Contains("Credito B")
                    || f.tipo.tipo.Contains("Factura C") || f.tipo.tipo.Contains("Debito C") || f.tipo.tipo.Contains("Credito C"))
                {
                    #region Fact B || Fact C
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);
                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    //nro remito factura
                    DataTable dtNroRemito = controlador.obtenerNroRemitoByFactura(f.id);

                    //levanto los datos de la factura
                    var drDatosFactura = dtDetalle.Rows[0];
                    //datos empresa emisora
                    DataTable dtEmpresa = this.controlEmpresa.obtenerEmpresaById((int)drDatosFactura["Empresa"]);

                    String razonSoc = String.Empty;
                    String direComer = String.Empty;
                    String condIVA = String.Empty;
                    String ingBrutos = String.Empty;
                    String fechaInicio = String.Empty;
                    String cuitEmpresa = String.Empty;
                    String nroFactura = String.Empty;
                    String tipoDoc = String.Empty;
                    String CAE = String.Empty;
                    String ptoVta = String.Empty;
                    String codBarra = String.Empty;
                    String fechaVto = String.Empty;
                    String cotizacionFecha = String.Empty;

                    foreach (DataRow row in dtEmpresa.Rows)
                    {
                        //verifico cual es la empresa de la factura
                        //if ((int)row[0] == )
                        //{
                        cuitEmpresa = row.ItemArray[1].ToString();
                        razonSoc = row.ItemArray[2].ToString();
                        ingBrutos = row.ItemArray[3].ToString();
                        fechaInicio = Convert.ToDateTime(row["Fecha Inicio"]).ToString("dd/MM/yyyy");// .ItemArray[4].ToString();
                        //fechaInicio = Convert.ToDateTime(fechaInicio).ToShortDateString();
                        condIVA = row.ItemArray[5].ToString();
                        direComer = row.ItemArray[6].ToString();
                        //}
                    }

                    //datos factura
                    string auxNro = drDatosFactura["Numero"].ToString();
                    nroFactura = auxNro.Substring(auxNro.Length - 13, 13);

                    tipoDoc = auxNro.Substring(0, auxNro.Length - 16);

                    if (drDatosFactura["Cae"].ToString() != "")
                    {
                        CAE = drDatosFactura["Cae"].ToString();
                        //CAE = "-";
                    }
                    else
                    {
                        CAE = "-";
                    }
                    ptoVta = drDatosFactura["ptoVenta"].ToString();
                    fechaVto = Convert.ToDateTime(drDatosFactura["Fecha"]).AddDays(10).ToString("ddMMyyyy");
                    codBarra = controlador.obtenerCodigoBarraFactura(drDatosFactura["CUIT"].ToString(), ptoVta, CAE, fechaVto);

                    if (configuracion.monotributo == "1")
                    {
                        if (tipoDoc.Contains("Debito"))
                        {
                            tipoDoc = "Nota de Debito C";
                        }
                        else
                        {
                            if (tipoDoc.Contains("Credito"))
                            {
                                tipoDoc = "Nota de Credito C";
                            }
                            else
                            {
                                tipoDoc = "Factura C";
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(codBarra))
                    {
                        codBarra = "0";

                    }

                    //verifico si el pto de venta es preimpresa para mostrar o no el logo de "cbte no fiscal".
                    PuntoVenta pv = this.cs.obtenerPuntoVentaPV(ptoVta, Convert.ToInt32(dtDetalle.Rows[0]["Sucursal"]), Convert.ToInt32(dtDetalle.Rows[0]["Empresa"]));
                    int esPreimpresa = 0;
                    if (pv != null)
                    {
                        if (pv.formaFacturar == "Preimpresa" || pv.formaFacturar == "Fiscal")
                        {
                            esPreimpresa = 1;
                        }
                    }

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }

                    //datos cotizacion al momento de fc
                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                    {
                        cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                    }

                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        controladorSucursal contSuc = new controladorSucursal();
                        Sucursal s = contSuc.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];

                    //neto no grabado
                    decimal subtotal = Convert.ToDecimal(dr[4]);

                    //subtotal menos el descuento
                    decimal subtotal2 = Convert.ToDecimal(dr[5]);

                    //iva discriminado de la fact
                    decimal iva = Convert.ToDecimal(dr[3]);

                    subtotal = subtotal + iva;

                    //total de la factura
                    decimal total = Convert.ToDecimal(dr[2]);

                    //retenciones
                    decimal retencion = Convert.ToDecimal(dr[6]);

                    //percepcion IVA Cons. Final
                    decimal percepIVA = Convert.ToDecimal(dr[8]);

                    //decimal descuento = Convert.ToDecimal(dr[1]);
                    //sumo el total de items - total de factura y saco el descuento
                    DataTable dtDescuento = controlador.obtenerTotalItem2(f.id);
                    decimal descuento = 0;
                    foreach (DataRow drr in dtDescuento.Rows)
                    {
                        descuento = Convert.ToDecimal(drr[0]);
                    }

                    descuento = decimal.Round(((descuento + retencion) - total), 2);
                    if (Math.Abs(descuento) == Convert.ToDecimal(0.01))
                    {
                        descuento = 0;
                    }

                    //letras
                    string totalS = Gestion_Api.Auxiliares.Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                    //string totalS = Numalet.ToCardinal("18.25");

                    //cant unidades
                    decimal cant = 2;

                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    if (dtDireccion != null)
                    {
                        foreach (DataRow drl in dtDireccion.Rows)
                        {
                            if (drl[0].ToString() == "Legal")
                            {
                                //direLegal = "-";
                                direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                            if (drl[0].ToString() == "Entrega")
                            {
                                //direEntrega = "";
                                direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                            }
                        }
                    }
                    if (direLegal != "-" && direEntrega == "-")
                    {
                        direEntrega = direLegal;
                    }

                    //Total equivalente en dolares
                    controladorMoneda contMoneda = new controladorMoneda();
                    Moneda dolar = contMoneda.obtenerMonedaDesc("Dolar");
                    decimal TotalDolares = 0;
                    String textoDolares = String.Empty;
                    if (dolar != null)
                    {
                        TotalDolares = Decimal.Round((total / dolar.cambio), 3);
                    }
                    if (tipoDoc.Contains("Nota de"))
                    {
                        textoDolares = " ";
                    }
                    else
                    {
                        textoDolares = "ESTA FACTURA EQUIVALE A USD $" + TotalDolares + " DOLARES ESTADOUNIDENSES PAGADERO  EN PESOS AL CIERRE DOLAR TIPO VENDEDOR DEL DÍA ANTERIOR A LA FECHA DE PAGO.";
                    }

                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //obtengo id empresa para buscar el logo correspondiente
                    int idEmpresa = Convert.ToInt32(drDatosFactura["Empresa"]);
                    //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/Logo.jpg");
                    string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/" + pv.id + "/Logo.jpg");
                    //codigo barra codBarra
                    //Create an instance of Barcode Professional
                    BarcodeProfessional bcp = new BarcodeProfessional();

                    //Barcode settings

                    bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code39;

                    bcp.BarHeight = 0.25f;
                    bcp.Code = codBarra;

                    byte[] prodBarcode = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                    DataTable dtImagen = new DataTable();

                    DataColumn ColumnImagen = new DataColumn("Imagen", Type.GetType("System.Byte[]"));

                    dtImagen.Columns.Add(ColumnImagen);

                    //DataRow drImagen = dtImagen.NewRow();
                    // object [] rowArray = new object[1];
                    // rowArray.SetValue(prodBarcode, 0);

                    //drImagen.ItemArray = rowArray;
                    //drImagen. = prodBarcode;

                    dtImagen.Rows.Add(prodBarcode);
                    //Generate the barcode image and store it into the Barcode Column


                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FacturaRB.rdlc");
                    this.ReportViewer1.LocalReport.EnableExternalImages = true;
                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("DatosPresupuesto", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("dtImagen", dtImagen);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);
                    ReportDataSource rds5 = new ReportDataSource("NumeroRemito", dtNroRemito);

                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param3a = new ReportParameter("ParamRetencion", retencion.ToString("C"));
                    ReportParameter param3a2 = new ReportParameter("ParamPercepIVA", percepIVA.ToString("C"));//percepIVA
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact                

                    ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                    param3b.Visible = false;
                    ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));
                    param4b.Visible = false;

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                    ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                    ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                    //parametros Datos empresa,cae y doc
                    ReportParameter param10 = new ReportParameter("ParamRazonSoc", razonSoc);
                    ReportParameter param11 = new ReportParameter("ParamIngresosBrutos", ingBrutos);
                    ReportParameter param12 = new ReportParameter("ParamFechaIni", fechaInicio);
                    ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                    ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);
                    ReportParameter param15 = new ReportParameter("ParamCuitEmp", cuitEmpresa);
                    ReportParameter param16 = new ReportParameter("ParamNroFac", nroFactura);
                    ReportParameter param17 = new ReportParameter("ParamTipoDoc", tipoDoc);
                    ReportParameter param18 = new ReportParameter("ParamCAE", CAE);
                    ReportParameter param19 = new ReportParameter("ParamPreimpresa", esPreimpresa.ToString());
                    ReportParameter param20 = new ReportParameter("ParamBarra", codBarra);

                    ReportParameter param23 = new ReportParameter("ParamTotalDolares", textoDolares);
                    ReportParameter param23b = new ReportParameter("ParamCambioDolar", cotizacionFecha);

                    ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                    this.ReportViewer1.LocalReport.SetParameters(param32);

                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds5);
                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param3a);//retencion
                    this.ReportViewer1.LocalReport.SetParameters(param3a2);//percepcion iva cf
                    this.ReportViewer1.LocalReport.SetParameters(param03);
                    this.ReportViewer1.LocalReport.SetParameters(param4);

                    this.ReportViewer1.LocalReport.SetParameters(param3b);
                    this.ReportViewer1.LocalReport.SetParameters(param4b);

                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);

                    this.ReportViewer1.LocalReport.SetParameters(param7);
                    this.ReportViewer1.LocalReport.SetParameters(param8);

                    //parametros datos empresa
                    this.ReportViewer1.LocalReport.SetParameters(param10);
                    this.ReportViewer1.LocalReport.SetParameters(param11);
                    this.ReportViewer1.LocalReport.SetParameters(param12);
                    this.ReportViewer1.LocalReport.SetParameters(param13);
                    this.ReportViewer1.LocalReport.SetParameters(param14);
                    this.ReportViewer1.LocalReport.SetParameters(param15);
                    this.ReportViewer1.LocalReport.SetParameters(param16);
                    this.ReportViewer1.LocalReport.SetParameters(param17);
                    this.ReportViewer1.LocalReport.SetParameters(param18);
                    this.ReportViewer1.LocalReport.SetParameters(param19);
                    this.ReportViewer1.LocalReport.SetParameters(param20);

                    this.ReportViewer1.LocalReport.SetParameters(param23);
                    this.ReportViewer1.LocalReport.SetParameters(param23b);

                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    //String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                    FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    #endregion
                }
                if (f.tipo.tipo.Contains("Presupuesto") || f.tipo.tipo.Contains("PRP"))
                {
                    #region presupuesto
                    //obtengo detalle de items
                    //DataTable dtDatos = controlador.obtenerDatosPresupuesto(idPresupuesto);
                    DataTable dtDatos = controlador.obtenerDatosPresupuesto(f.id);

                    DataTable dtDetalle = controlador.obtenerDetallePresupuesto(f.id);

                    DataRow srCliente = dtDetalle.Rows[0];
                    string codigoCliente = srCliente[5].ToString();

                    //DataTable dtTotal = controlador.obtenerTotalPresupuesto(idPresupuesto);
                    DataTable dtTotales = controlador.obtenerTotalPresupuesto2(f.id);
                    DataRow dr = dtTotales.Rows[0];
                    decimal subtotal = Convert.ToDecimal(dr[0]);

                    decimal descuento = Convert.ToDecimal(dr[1]);
                    decimal total = Convert.ToDecimal(dr[2]);

                    String cotizacionFecha = String.Empty;

                    //obtengo el telefono del cliente para agregarlo al pedido
                    string telefono = "";
                    List<contacto> contactosClientes = this.contCliente.obtenerContactos(Convert.ToInt32(srCliente["idCliente"]));
                    if (contactosClientes.Count > 0 & contactosClientes != null)
                    {
                        telefono = contactosClientes[0].numero;
                    }
                    if (String.IsNullOrEmpty(telefono))
                    {
                        telefono = "-";
                    }

                    //direccion cliente
                    string direLegal = "-";
                    string direEntrega = "-";
                    DataTable dtDireccion = controlador.obtenerDireccionPresupuesto(f.id);
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl[0].ToString() == "Legal")
                        {
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString();
                        }
                        if (drl[0].ToString() == "Entrega")
                        {
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString();
                        }
                    }

                    //sucursal venta
                    string sucursalOrigen = dtDetalle.Rows[0]["Sucursal"].ToString();
                    Sucursal sucvta = this.cs.obtenerSucursalID(Convert.ToInt32(sucursalOrigen));
                    //sucursalfacturada                
                    string sucursalFact = dtDetalle.Rows[0]["SucursalFacturada"].ToString();
                    if (sucursalFact != "0")
                    {
                        Sucursal s = this.cs.obtenerSucursalID(Convert.ToInt32(sucursalFact));
                        sucursalFact = "-" + s.nombre;
                    }
                    else
                    {
                        sucursalFact = " ";
                    }

                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["CondicionIva"].ToString()))
                    {
                        dtDetalle.Rows[0]["IVA"] = dtDetalle.Rows[0]["IVA2"];
                    }
                    //datos cotizacion al momento de fc
                    if (!String.IsNullOrEmpty(dtDetalle.Rows[0]["TipoCambio"].ToString()))
                    {
                        cotizacionFecha = dtDetalle.Rows[0]["TipoCambio"].ToString();
                    }

                    //string logo = Server.MapPath("../../Images/Logo.jpg");

                    //Cargo configuracion para mostrar Precio de venta con o sin IVA.
                    Configuracion c = new Configuracion();

                    this.ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    if (c.porcentajeIva != "0")
                    {
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                    }
                    else
                    {
                        //subtotal sin iva
                        subtotal = Convert.ToDecimal(dr[4]);
                        total = subtotal - descuento;
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2SinIva.rdlc");
                    }

                    //Comentario factura
                    DataTable dtComentarios = this.controlador.obtenerComentarioPresupuesto(f.id);

                    //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("Presupesto2.rdlc");
                    ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                    ReportDataSource rds2 = new ReportDataSource("ItemsPresupuesto", dtDatos);
                    ReportDataSource rds3 = new ReportDataSource("DatosPresupuesto", dtDatos);
                    ReportDataSource rds4 = new ReportDataSource("DetalleComentario", dtComentarios);


                    ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                    ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                    ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));
                    ReportParameter param03 = new ReportParameter("ParamSucFact", sucursalFact);//sucursalFact
                    ReportParameter param04 = new ReportParameter("ParamSucursal", sucvta.nombre);//sucursalVta

                    //ReportParameter param32 = new ReportParameter("Porcentaje", porcentaje.ToString("N"));

                    ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                    ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                    ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);
                    ReportParameter param7 = new ReportParameter("TelefonoEntrega", telefono);
                    ReportParameter param8 = new ReportParameter("ParamCambioDolar", cotizacionFecha);

                    //ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);
                    //this.ReportViewer1.LocalReport.SetParameters(param32);


                    this.ReportViewer1.LocalReport.DataSources.Clear();
                    this.ReportViewer1.LocalReport.DataSources.Add(rds);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                    this.ReportViewer1.LocalReport.DataSources.Add(rds4);

                    this.ReportViewer1.LocalReport.SetParameters(param);
                    this.ReportViewer1.LocalReport.SetParameters(param2);
                    this.ReportViewer1.LocalReport.SetParameters(param3);
                    this.ReportViewer1.LocalReport.SetParameters(param03);
                    this.ReportViewer1.LocalReport.SetParameters(param04);

                    this.ReportViewer1.LocalReport.SetParameters(param4);
                    this.ReportViewer1.LocalReport.SetParameters(param5);
                    this.ReportViewer1.LocalReport.SetParameters(param6);
                    this.ReportViewer1.LocalReport.SetParameters(param7);
                    this.ReportViewer1.LocalReport.SetParameters(param8);

                    this.ReportViewer1.LocalReport.Refresh();

                    Warning[] warnings;

                    string mimeType, encoding, fileNameExtension;

                    string[] streams;

                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //save the generated report in the server
                    //String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                    FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                    stream.Write(pdfContent, 0, pdfContent.Length);
                    stream.Close();

                    #endregion
                }

                return 1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error enviando factura por mail. " + ex.Message));
                return -1;
            }
        }


        #endregion

        #region funciones auxiliares
        private int restablecerIvaCliente()
        {
            try
            {
                //despues de refacturar dejo el IVA en no informa
                Cliente c = this.contCliente.obtenerClienteID(Convert.ToInt32(this.lblIdCliente.Text));
                int idIva = this.contCliente.obtenerIvaIdClienteByNombre("No Informa");
                //c.iva = this.ListIvaFact.Items.FindByValue(idIva.ToString()).Text;
                c.iva = idIva.ToString();
                int cMod = this.contCliente.modificarCliente(c, c.cuit, c.codigo);
                return cMod;
            }
            catch
            {
                return -1;
            }
        }
        public int verificarCierreCaja(int idFactura)
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                Factura fact = this.controlador.obtenerFacturaId(idFactura);
                int sucursal = fact.sucursal.id;
                int ptoVenta = fact.ptoV.id;

                var fecha = contCaja.obtenerUltimaApertura(sucursal, ptoVenta);
                //si la fecha de apertura es mas gande q hoy no lo dejo
                if (DateTime.Now < fecha)
                {
                    //ya existe una un cierre para el dia de hoy
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se realizo un cierre de caja en el dia de hoy para este punto de venta. La proxima fecha de apertura es " + fecha.ToString("dd/MM/yyyy")));
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando cierre de caja. " + ex.Message));
                return -2;
            }
        }
        protected void verificarPermisoEditarFC()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "110")
                        {
                            this.phEditarFC.Visible = true;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void detalleFactura(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idFactura = atributos[1];
                int tipo = Convert.ToInt32(atributos[2]);
                TipoDocumento tp = this.contDocumentos.obtenerTipoDoc("Presupuesto");

                if (tipo == tp.id || tipo == 11 || tipo == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    if (tipo == 1 || tipo == 9 || tipo == 4 || tipo == 24 || tipo == 25 || tipo == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

                    }
                    else
                    {
                        //facturab
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }

                }




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        private int actualizarDatosRefacturarCliente()
        {
            try
            {
                //cargo todos los datos del cliente
                Cliente c = this.contCliente.obtenerClienteID(Convert.ToInt32(this.lblIdCliente.Text));
                c.cuit = this.txtCUITfact.Text;
                c.alerta = this.contCliente.obtenerAlertaClienteByID(Convert.ToInt32(this.lblIdCliente.Text));
                c.alerta.idCliente = c.id;
                c.iva = this.ListIvaFact.SelectedValue.ToString();

                int cMod = this.contCliente.modificarCliente(c, c.cuit, c.codigo);
                if (cMod > 0)
                {
                    controladorDireccion contDir = new controladorDireccion();
                    if (c.direcciones != null)
                    {
                        if (c.direcciones.Count == 0)
                        {
                            direccion dir = new direccion();
                            dir.nombre = "Legal";
                            dir.direc = this.txtDirFact.Text;
                            dir.localidad = this.ListLocalidadFact.SelectedValue;
                            dir.provincia = this.ListProvinciaFact.SelectedValue;
                            dir.pais = "Argentina";
                            dir.codPostal = "0000";

                            int i = this.contCliente.agregarDireccionReduc(c, dir);

                            return i;
                        }
                        else
                        {
                            direccion d = contDir.obtenerContactoId(Convert.ToInt32(this.lblIdDirFact.Text));
                            d.direc = this.txtDirFact.Text;
                            d.localidad = this.ListLocalidadFact.SelectedValue;
                            d.provincia = this.ListProvinciaFact.SelectedValue;
                            int j = this.contCliente.ModificarDireccionMod(d, c.id);
                            return j;
                        }
                    }
                }
                else
                {
                    return -1;
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion

        #region Agregar FC Manual
        protected void verificarPermisoAgregarFC()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "113")
                        {
                            this.phAgregarFC.Visible = true;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        protected void lbtnAgregarFC_Click(object sender, EventArgs e)
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.ItemCancelado != "0" && config.ClienteCancelado != "0" && config.ListaPrecioCancelado != "0")
                {
                    controladorArticulo contArt = new controladorArticulo();
                    var a = contArt.obtenerArticuloByID(Convert.ToInt32(config.ItemCancelado));
                    var c = this.contCliente.obtenerClienteID(Convert.ToInt32(config.ClienteCancelado));
                    var l = this.contFormPago.obtenerListaPrecioEntByID(Convert.ToInt32(config.ListaPrecioCancelado));
                    this.lblItemAgregarFC.Text = a.descripcion;
                    this.DropListClienteAgregarFC.SelectedValue = c.id.ToString();
                    this.lblListaPrecioAgregarFC.Text = l.nombre;
                }
                ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "openModalAgregarFactura", "openModalAgregarFactura();", true);
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error obteniendo campos para AgregarFC Manual" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error obteniendo campos para AgregarFC Manual " + ex.Message));
            }
        }
        protected void btnAgregarFC_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifico que las listas tengan valores correctos
                if (!this.verificarListasAgregarFC())
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los campos seleccionados son incorrectos. Verifíquelos. "));
                    return;
                }

                //Verifico de que el punto de venta seleccionado no sea electrónico
                if (this.verificarPuntoVentaElectronico(Convert.ToInt32(this.DropListPuntoVentaAgregarFC.SelectedValue)))
                {
                    Configuracion config = new Configuracion();
                    controladorArticulo contArt = new controladorArticulo();
                    Articulo a = contArt.obtenerArticuloByID(Convert.ToInt32(config.ItemCancelado));
                    Gestion_Api.Modelo.TipoDocumento td = null;

                    int idEmpresa = Convert.ToInt32(this.DropListEmpresaAgregarFC.SelectedValue);
                    int idSucursal = Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue);
                    int idVendedor = Convert.ToInt32(this.DropListVendedorAgregarFC.SelectedValue);
                    int idList = Convert.ToInt32(config.ListaPrecioCancelado);
                    int idPtoVta = Convert.ToInt32(this.DropListPuntoVentaAgregarFC.SelectedValue);
                    int idCliente = Convert.ToInt32(this.DropListClienteAgregarFC.SelectedValue);
                    td = this.controlador.obtenerTipoDoc(this.DropListDocumentoAgregarFC.SelectedItem.ToString().Trim());
                    int cantidad = Convert.ToInt32(this.txtNumeroHastaAgregarFC.Text) - Convert.ToInt32(this.txtNumeroDesdeAgregarFC.Text);

                    int numero = Convert.ToInt32(this.txtNumeroDesdeAgregarFC.Text) - 1;
                    string nuevoNumeroFc = numero.ToString().PadLeft(8, '0');
                    if (cantidad >= 0)
                    {
                        //Actualizo la tabla numeracion con los numeros ingresados en los textbox
                        int actualizar = this.controlador.actualizarNumeroFactura2(nuevoNumeroFc, idPtoVta, td.id);
                        if (actualizar > 0)
                        {
                            int j = 0;
                            for (int i = 0; i <= cantidad; i++)
                            {
                                j += this.controlador.agregarFcManual(null,idEmpresa, idSucursal, idVendedor, idList, idPtoVta, idCliente, td, a);
                            }
                            if (j == cantidad + 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Las facturas fueron agregadas exitósamente. "));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Se cargaron " + j + " facturas de " + cantidad + 1 + " seleccionadas."));
                            }
                        }
                        this.limpiarCamposAgregarFC();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El rango seleccionado es incorrecto. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El punto de venta no puede ser electrónico. "));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error generando Factura desde AgregarFC Manual " + Ex.Message));
            }
        }
        protected void DropListEmpresaAgregarFC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursalByEmpresa(Convert.ToInt32(this.DropListEmpresaAgregarFC.SelectedValue));
        }
        protected void DropListSucursalAgregarFC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarPuntosVenta(Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue));
                this.cargarVendedoresBySucursal(Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue));
            }
            catch (Exception Ex)
            {

            }

        }
        private void cargarVendedoresBySucursal(int idSucursal)
        {
            try
            {
                try
                {
                    controladorVendedor contVendedor = new controladorVendedor();
                    DataTable dt = contVendedor.obtenerVendedoresBySuc(idSucursal);
                    this.DropListVendedorAgregarFC.Items.Clear();
                    //agrego todos
                    DataRow dr3 = dt.NewRow();
                    dr3["nombre"] = "Todos";
                    dr3["id"] = 0;
                    dt.Rows.InsertAt(dr3, 0);

                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem item = new ListItem();
                        item.Value = dr["id"].ToString();
                        item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                        this.DropListVendedorAgregarFC.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando VendedoresBySucursal. " + ex.Message));
                }
            }
            catch (Exception Ex)
            {

            }
        }
        private void limpiarCamposAgregarFC()
        {
            try
            {
                this.txtNumeroDesdeAgregarFC.Text = string.Empty;
                this.txtNumeroHastaAgregarFC.Text = string.Empty;
                this.DropListEmpresaAgregarFC.SelectedIndex = 0;
                this.DropListSucursalAgregarFC.SelectedIndex = 0;
                this.DropListVendedorAgregarFC.SelectedIndex = 0;
                this.DropListPuntoVentaAgregarFC.SelectedIndex = 0;
                this.DropListDocumentoAgregarFC.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {

            }
        }
        private bool verificarPuntoVentaElectronico(int idPuntoVenta)
        {
            try
            {
                controladorSucursal contSuc = new controladorSucursal();
                PuntoVenta pv = contSuc.obtenerPtoVentaId(idPuntoVenta);
                if (pv.formaFacturar != "Electronica")
                {
                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo verificar el punto de venta. " + Ex.Message));
                return false;
            }
        }
        protected void lbtnBuscarClienteAgregarFC_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtBuscarClienteAgregarFC.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClienteAgregarFC.DataSource = dtClientes;
                this.DropListClienteAgregarFC.DataValueField = "id";
                this.DropListClienteAgregarFC.DataTextField = "alias";
                this.DropListClienteAgregarFC.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        private bool verificarListasAgregarFC()
        {
            try
            {
                if (Convert.ToInt32(this.DropListEmpresaAgregarFC.SelectedValue) < 1)
                    return false;
                if (Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue) < 1)
                    return false;
                if (Convert.ToInt32(this.DropListVendedorAgregarFC.SelectedValue) < 1)
                    return false;
                if (Convert.ToInt32(this.DropListPuntoVentaAgregarFC.SelectedValue) < 1)
                    return false;
                if (Convert.ToInt32(this.DropListClienteAgregarFC.SelectedValue) < 1)
                    return false;
                if (Convert.ToInt32(this.DropListDocumentoAgregarFC.SelectedValue) < 1)
                    return false;

                return true;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error verificando elementos seleccionados de las listas en AgregarFC Manual. Excepción: " + Ex.Message));
                return false;
            }
        }
        #endregion

        #region Editar FC

        protected void btnEditarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                string modificoHora = WebConfigurationManager.AppSettings.Get("ModificoHora");

                int idFact = Convert.ToInt32(this.lblIdFact.Text);
                string puntoVenta = this.txtNuevoPuntoVenta.Text;

                DateTime horaFactura = new DateTime();
                string horaFacturaFinal = "";

                if (modificoHora == "1")
                {
                    string restaHoras = WebConfigurationManager.AppSettings.Get("HorasDiferencia");
                    horaFactura = DateTime.ParseExact(lblFechaFactura.Text, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
                    horaFacturaFinal = horaFactura.AddHours(Convert.ToInt32(restaHoras) * -1).ToString("hh:mm");
                }
                else
                {
                    horaFactura = DateTime.ParseExact(lblFechaFactura.Text, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
                    horaFacturaFinal = horaFactura.ToString("hh:mm");
                }

                string fecha = this.txtNuevaFecha.Text + " " + horaFacturaFinal;
                string numero = this.txtNuevoNumeroFactura.Text;
                string neto = this.txtNuevoNetoFactura.Text;
                string iva = this.txtNuevoIvaFactura.Text;
                string total = this.txtNuevoTotalFactura.Text;

                int cliente = Convert.ToInt32(ListClientesEditar.SelectedValue);

                int i = this.controlador.modificarTotalesFactura(idFact, neto, iva, total, numero, puntoVenta, fecha, cliente);
                if (i > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura modificada con exito!. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
                }
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se modificaron algunos datos de la factura. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar la factura."));
                }
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error modificando datos de factura " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar la factura."));
            }
        }

        protected void btnBuscarClienteEditar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodClienteEditar.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.ListClientesEditar.DataSource = dtClientes;
                this.ListClientesEditar.DataValueField = "id";
                this.ListClientesEditar.DataTextField = "alias";
                this.ListClientesEditar.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
        protected void btnAnularFactura_Click(object sender, EventArgs e)
        {
            try
            {
                int idFact = Convert.ToInt32(this.lblIdFact.Text);
                int i = this.controlador.eliminarFacturaDesdeEditarFC(idFact);
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura anulada con éxito!. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo anular la factura. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
                }
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error anulando factura desde Editar FC " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo anular la factura. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
            }
        }

        #endregion

        #region Servicio de Informes
        protected void btnSolicitarInformeIIBB_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorInformesEntity contInfEnt = new ControladorInformesEntity();
                Informes_Pedidos ip = new Informes_Pedidos();
                InformeXML infXML = new InformeXML();

                //Cargo el objeto Informes_Pedidos
                cargarDatosInformePedido(ip);

                //Cargo el objeto InformeXML
                cargarDatosInformeXML(infXML);

                //Mando a grabar el pedido de informe, y genero el XML. Si todo es correcto retorna 1. En caso contrario, revisar error segun el entero.
                int i = contInfEnt.generarPedidoDeInforme(infXML, ip);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se ha generado la solicitud de informe! ", "/Formularios/Reportes/InformesF.aspx?us=" + ip.Usuario.ToString()));
                if (i == -1)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error grabando el pedido de informe! "));
                if (i == -2)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error generando el Informe XML! "));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error solicitando Informe de IIBB " + Ex.Message));
            }
        }

        public void cargarDatosInformeXML(InformeXML infXML)
        {
            try
            {
                DateTime fechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR"));

                infXML.FechaDesde = fechaDesde.ToString("dd/MM/yyyy");
                infXML.FechaHasta = fechaHasta.ToString("dd/MM/yyyy");
                infXML.Empresa = Convert.ToInt32(this.DropListEmpresa.SelectedValue);
                infXML.Sucursal = Convert.ToInt32(this.DropListSucursal.SelectedValue);
                infXML.Cliente = Convert.ToInt32(this.DropListClientes.SelectedValue);
                infXML.Tipo = Convert.ToInt32(this.DropListTipo.SelectedValue);
                infXML.Documento = Convert.ToInt32(this.DropListDocumento.SelectedValue);
                infXML.ListaPrecio = Convert.ToInt32(this.DropListListas.SelectedValue);
                infXML.FormaPago = Convert.ToInt32(this.DropListFormasPago.SelectedValue);
                infXML.Vendedor = Convert.ToInt32(this.DropListVendedor.SelectedValue);
                infXML.Anuladas = Convert.ToInt32(this.chkAnuladas.Checked);

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error cargando datos para InformeXML. Excepción: " + Ex.Message));
            }
        }

        public void cargarDatosInformePedido(Informes_Pedidos ip)
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                ip.Fecha = DateTime.Now;
                ip.Informe = 2;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;
                ip.NombreInforme = "IIBB-" + fechaD.ToString("MMyyyy");
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrió un error cargando datos para Informe Pedido. Excepción: " + Ex.Message));
            }

        }
        #endregion

        #region Busqueda de Facturas

        /// <summary>
        /// Este metodo busca por el numero de factura introducido la caja de texto del modal
        /// </summary>
        private void BuscarPorNumeroFactura()
        {
            try
            {
                List<Factura> listaFacturas = this.controlador.obtenerFacturaByNumero(this.numeroFactura);
                int cantidadFacturasEncontradas = listaFacturas.Count();
                if (cantidadFacturasEncontradas != 0)
                {
                    this.phFacturas.Controls.Clear();
                    foreach (var factura in listaFacturas)
                    {
                        this.cargarEnPh(factura);
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se encontraron " + cantidadFacturasEncontradas + " facturas.", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se ha encontrado ninguna factura con esta numeracion: " + this.numeroFactura));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando pedido por numero. CATCH:" + ex.Message));
            }
        }

        /// <summary>
        /// Este metodo busca por la observacion/comentario de la factura.
        /// </summary>
        private void BuscarPorRazonSocial()
        {
            try
            {
                List<Factura> listaFacturas = this.controlador.obtenerFacturaByRazonSocial(this.razonSocialCliente);
                int cantidadFacturasEncontradas = listaFacturas.Count();
                if (cantidadFacturasEncontradas != 0)
                {
                    this.phFacturas.Controls.Clear();
                    foreach (var factura in listaFacturas)
                    {
                        this.cargarEnPh(factura);
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se encontraron " + cantidadFacturasEncontradas + " facturas.", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se ha encontrado ninguna factura con estos caracteres: " + this.razonSocialCliente));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Factura por Cliente. CATCH:  " + ex.Message));
            }
        }

        /// <summary>
        /// Este metodo busca por la observacion/comentario de la factura.
        /// </summary>
        private void BuscarPorObservacion()
        {
            try
            {
                List<Factura> listaFacturas = this.controlador.obtenerFacturaPorObservacion(this.observacion);
                int cantidadFacturasEncontradas = listaFacturas.Count();
                if (cantidadFacturasEncontradas != 0)
                {
                    this.phFacturas.Controls.Clear();
                    foreach (var factura in listaFacturas)
                    {
                        this.cargarEnPh(factura);
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se encontraron " + cantidadFacturasEncontradas + " facturas.", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se ha encontrado ninguna factura con esta observacion: " + this.observacion));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("WEB: Error buscando Factura por observacion.CATCH:  " + ex.Message));
            }
        }

        #endregion

        #region Nota Debito/Credito por Diferencia de Cambio

        protected void lbtnDebitoCreditoDiferenciaCambio_Click(object sender, EventArgs e)
        {
            try
            {
                string idTildado = string.Empty;
                int flag = 0;
                this.lblMensajeDiferenciaCambio.Text = string.Empty;
                this.lblDiferenciaCambio.Text = string.Empty;
                txtNuevoCambioNotaDebitoCreditoDiferenciaCambio.Enabled = true;

                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idTildado = ch.ID.Split('_')[1];
                        flag++;
                    }
                }

                if (string.IsNullOrEmpty(idTildado))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar alguna factura"));
                    return;
                }

                if (flag > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar sólo una factura"));
                    return;
                }

                var factura = this.controlador.obtenerFacturaId(Convert.ToInt32(idTildado));
                if (factura == null)
                {
                    return;
                }
                if (!controlador.DevuelveTrueSiLaFacturaEsA_B_C_E(factura.tipo.id))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una factura A,B,C o E"));
                    return;
                }

                var facturasIvas = this.contFactEntity.obtenerDatosIvasFactura(Convert.ToInt32(idTildado));

                if (facturasIvas != null)
                {
                    var cambio = (decimal)facturasIvas.TipoCambio;
                    lblTipoCambioOriginalNotaDebitoCreditoDiferenciaCambio.Text = cambio.ToString("N");
                }
                else
                {
                    lblTipoCambioOriginalNotaDebitoCreditoDiferenciaCambio.Text = "1.00";
                }

                lblIdFacturaNotaDebitoCreditoDiferenciaCambio.Text = idTildado;

                ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "openModalNotaDebitoCreditoDiferenciaCambio", "openModalNotaDebitoCreditoDiferenciaCambio();", true);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error abriendo modal para generar nota de debito/credito por diferencia de cambio. Excepción: " + Ex.Message));
            }
        }

        private void generarNotaDebitoCreditoDiferenciaCambio()
        {
            try
            {
                var factura = this.controlador.obtenerFacturaId(Convert.ToInt32(lblIdFacturaNotaDebitoCreditoDiferenciaCambio.Text));

                if (factura != null)
                {
                    string tipoNota = "Nota de Debito ";

                    if (lblMensajeDiferenciaCambio.Text.Contains("Credito"))
                    {
                        tipoNota = "Nota de Credito ";
                    }

                    int i = this.controlador.GenerarNotaCreditoDebitoDiferenciaCambio(null,1, Convert.ToDecimal(lblDiferenciaCambio.Text), factura, tipoNota);

                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelNotaDebitoCreditoDiferenciaCambio, UpdatePanelNotaDebitoCreditoDiferenciaCambio.GetType(), "alert", " $.msgbox(\"La " + tipoNota + " se generó correctamente. \", {type: \"info\"}); location.href = 'FacturasF.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelNotaDebitoCreditoDiferenciaCambio, UpdatePanelNotaDebitoCreditoDiferenciaCambio.GetType(), "alert", "$.msgbox(\"Ocurrió un error generando la " + tipoNota + ". \", {type: \"error\"});", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando nota debito/credito por diferencia de cambio. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnCalcularDiferenciaCambio_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblIdFacturaNotaDebitoCreditoDiferenciaCambio.Text))
                {
                    var factura = this.controlador.obtenerFacturaId(Convert.ToInt32(lblIdFacturaNotaDebitoCreditoDiferenciaCambio.Text));
                    if (factura != null)
                    {
                        decimal totalItemsMonedaExtranjera = controlador.ObtenerTotalEnPesosDeLosItemsQueSeanMonedaExtrangeraByFactura(factura.items);

                        var divMonedaOriginal = totalItemsMonedaExtranjera / Convert.ToDecimal(lblTipoCambioOriginalNotaDebitoCreditoDiferenciaCambio.Text);
                        var prodMonedaNueva = divMonedaOriginal * Convert.ToDecimal(txtNuevoCambioNotaDebitoCreditoDiferenciaCambio.Text);
                        var diferencia = prodMonedaNueva - totalItemsMonedaExtranjera;

                        if (diferencia > 0)
                        {
                            lblMensajeDiferenciaCambio.Text = "Se va a generar una Nota de Debito por ";
                        }
                        else
                        {
                            lblMensajeDiferenciaCambio.Text = "Se va a generar una Nota de Credito por ";
                            diferencia = diferencia * -1;
                        }

                        lblDiferenciaCambio.Text = Decimal.Round(diferencia, 2).ToString();
                        btnGenerarNotaDebitoCreditoDiferenciaCambio.Attributes.Remove("disabled");

                        txtNuevoCambioNotaDebitoCreditoDiferenciaCambio.Enabled = false;
                        txtNuevoCambioNotaDebitoCreditoDiferenciaCambio.CssClass = "form-control";
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando diferencia de cambio. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnGenerarNotaDebitoCreditoDiferenciaCambio_Click(object sender, EventArgs e)
        {
            try
            {
                generarNotaDebitoCreditoDiferenciaCambio();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando a generar nota de debito/credito por diferencia de cambio. Excepción: " + Ex.Message));
            }
        }


        #endregion
        protected void lbtnOrdenReparacion_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("../OrdenReparacion/OrdenReparacionABM.aspx?a=1&presupuesto=" + idtildado);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al generar orden de reparacion " + ex.Message);
            }
        }

        protected void btnGenerarPatentamiento_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> facturasTildadas = new List<int>();
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        facturasTildadas.Add(Convert.ToInt32(ch.ID.Split('_')[1]));
                    }
                }
                if (facturasTildadas.Count > 0)
                {
                    if (ValidarCamposPatentamientosEstenCompletos())
                    {
                        if (!controladorCompraEntity.ComprobarSiLasFacturasFueronPatentadas(facturasTildadas))
                            GenerarCompraPorPatentamiento(facturasTildadas);
                        else
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Alguna de las facturas seleccionadas ya fue patentada."));
                    }
                }
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una factura."));

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al generar patentamiento " + ex.Message);
            }
        }

        private bool ValidarCamposPatentamientosEstenCompletos()
        {
            if (Convert.ToInt32(dropDownListProveedoresPatentamiento.SelectedValue) > 0 && !string.IsNullOrEmpty(txtTotalPatentamiento.Text) && Convert.ToDecimal(txtTotalPatentamiento.Text) > 0)
                return true;
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Compruebe que haya un proveedor seleccionado y el monto ingresado sea mayor a 0."));
                return false;
            }
        }

        private void GenerarCompraPorPatentamiento(List<int> idsTildados)
        {
            if (!controladorCompraEntity.ComprobarFacturasPatentamientoSonFacturas(idsTildados))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Los documentos seleccionados no son facturas!"));
                return;
            }

            Usuario user = contUser.obtenerUsuariosID((int)Session["Login_IdUser"]);
            var proveedor = contCliente.obtenerProveedorID(Convert.ToInt32(dropDownListProveedoresPatentamiento.SelectedValue));

            Gestion_Api.Entitys.Compra compra = new Gestion_Api.Entitys.Compra();

            compra.IdEmpresa = Convert.ToInt32(user.empresa.id);
            compra.IdSucursal = Convert.ToInt32(user.sucursal.id);
            compra.IdPuntoVenta = Convert.ToInt32(user.ptoVenta.id);
            compra.Fecha = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
            compra.TipoDocumento = "Presupuesto";
            compra.PuntoVenta = user.ptoVenta.puntoVenta;
            compra.Numero = ObtenerUltimaCompraProveedor();
            compra.Proveedor = Convert.ToInt32(dropDownListProveedoresPatentamiento.SelectedValue);
            compra.Cuit = proveedor.cuit;
            compra.Iva = proveedor.iva;
            compra.NetoNoGrabado = Convert.ToDecimal(txtTotalPatentamiento.Text);
            compra.Total = Convert.ToDecimal(txtTotalPatentamiento.Text);
            compra.Vencimiento = Convert.ToDateTime(DateTime.Now.AddDays(30), new CultureInfo("es-AR"));
            compra.FechaImputacion = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
            compra.Tipo = 3;

            int i = controladorCompraEntity.agregarCompra(compra);

            if (i > 0)
                ProcesarFacturasPatentamiento(compra.Id, compra.Proveedor.Value, idsTildados);
        }

        private void ProcesarFacturasPatentamiento(long idCompra, int idProveedor, List<int> idsTildados)
        {
            List<Gestion_Api.Entitys.Facturas_Patentamiento> facturas_Patentamiento = new List<Facturas_Patentamiento>();

            string idsTildadosString = "";

            foreach (var idFactura in idsTildados)
            {
                idsTildadosString += idFactura.ToString() + ",";
                facturas_Patentamiento.Add(GenerarFacturasPatentamiento(idCompra, idFactura, idProveedor));
            }

            idsTildadosString = idsTildadosString.Remove(idsTildadosString.Length - 1);

            int i = controladorCompraEntity.ProcesarFacturas_Patentamiento(facturas_Patentamiento, idsTildadosString);

            if (i > 0)
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Patentamiento relizado con exito!", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al realizar el patentamiento!."));
        }

        private Facturas_Patentamiento GenerarFacturasPatentamiento(long idCompra, int idFactura, int idProveedor)
        {
            Gestion_Api.Entitys.Facturas_Patentamiento facturas_Patentamiento = new Gestion_Api.Entitys.Facturas_Patentamiento();

            facturas_Patentamiento.IdCompra = idCompra;
            facturas_Patentamiento.IdFactura = idFactura;
            facturas_Patentamiento.IdProveedor = idProveedor;

            return facturas_Patentamiento;
        }

        private string ObtenerUltimaCompraProveedor()
        {
            controladorCompraEntity controladorCompraEntity = new controladorCompraEntity();

            var compras = controladorCompraEntity.buscarComprasProveedorDoc(Convert.ToInt32(dropDownListProveedoresPatentamiento.SelectedValue));

            return (Convert.ToInt32(compras.LastOrDefault().Numero) + 1).ToString("D8");
        }

        protected void btnExportarVentasConSolicitudes_Click(object sender, EventArgs e)
        {
            this.ExportToExcel(14);
        }

        protected void btnImprimirVentasConSolicitudes_Click(object sender, EventArgs e)
        {
            this.PrintToPDF(14);
        }

        protected void btnSiEnviarMailPorCliente_Click(object sender, EventArgs e)
        {
            try
            {
                int user = (int)Session["Login_IdUser"];

                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int mailsEnviados = 0;
                    int mailsNoEnviados = 0;
                    string emailsNoEncontrados = "";
                    string[] IDs = idtildado.Split(';');

                    for (int i = 0; i < (IDs.Length - 1); i++)
                    {
                        Factura factura = this.controlador.obtenerFacturaId(Convert.ToInt32(IDs[i]));
                        String path = Server.MapPath("../../Facturas/" + factura.empresa.id + "/" + "/fc-" + factura.numero + "_" + factura.id + ".pdf");

                        if (factura != null)
                        {
                            var mail = this.controladorClienteEntity.obtenerClienteDatosByCliente(factura.cliente.id);

                            if (mail != null && mail.Count > 0)
                            {
                                string destinatarios = mail.FirstOrDefault().Mail;
                                if (!string.IsNullOrEmpty(destinatarios))
                                {
                                    int j = this.GenerarImpresionPDF(factura, path);
                                    if (j > 0)
                                    {
                                        Attachment adjunto = new Attachment(path);

                                        int ok = this.contFunciones.enviarMailFactura(adjunto, factura, destinatarios);
                                        if (ok > 0)
                                        {
                                            mailsEnviados++;
                                            adjunto.Dispose();
                                            File.Delete(path);
                                        }
                                        else
                                        {
                                            mailsNoEnviados++;
                                            Log.EscribirSQL(1, "ERROR", "Ubicacion: FacturasF.aspx. Metodo: btnSiEnviarMailPorCliente_Click. No se pudo enviar el mail, revisar funcion this.contFunciones.enviarMailFactura(adjunto, factura, destinatarios). Cliente ID: " + factura.cliente.id.ToString());
                                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar factura por mail. "));
                                        }
                                    }
                                    else
                                    {
                                        mailsNoEnviados++;
                                        Log.EscribirSQL(1, "ERROR", "Ubicacion: FacturasF.aspx. Metodo: btnSiEnviarMailPorCliente_Click. No se pudo generar impresion factura a enviar.Cliente ID: " + factura.cliente.id.ToString());
                                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar impresion factura a enviar. "));
                                    }
                                }
                                else
                                {
                                    mailsNoEnviados++;
                                    emailsNoEncontrados += factura.cliente.razonSocial + " | ";
                                    Log.EscribirSQL(1, "ERROR", "Ubicacion: FacturasF.aspx. Metodo: btnSiEnviarMailPorCliente_Click. Email viene vacio. Cliente ID: " + factura.cliente.id.ToString());
                                }
                            }
                            else
                            {
                                mailsNoEnviados++;
                                emailsNoEncontrados += factura.cliente.razonSocial + " | ";
                                Log.EscribirSQL(1, "ERROR", "Ubicacion: FacturasF.aspx. Metodo: btnSiEnviarMailPorCliente_Click. No se encontro email.Cliente ID: " + factura.cliente.id.ToString());
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encontro e-mail de los clientes de una o varias de estas facturas"));
                            }
                        }
                    }

                    if (mailsEnviados > 0)
                    {
                        if (!string.IsNullOrEmpty(emailsNoEncontrados))
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura/s enviada correctamente! </br> Enviados: " + mailsEnviados.ToString(), ""));
                        else
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura/s enviada correctamente! </br> Enviados:  " + mailsEnviados.ToString() + " </br> No enviados:  " + mailsNoEnviados.ToString() + " </br> No se encontro e-mail de los siguientes clientes: </br> " + emailsNoEncontrados + "", ""));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar ninguna factura.</br> No enviados: " + mailsNoEnviados.ToString() + " </br> No se encontro e-mail de los siguientes clientes: </br> " + emailsNoEncontrados));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una factura."));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "CATCH: FacturasF.aspx. Metodo:btnSiEnviarMailPorCliente_Click.Error " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando factura por mail. " + ex.Message));
            }
        }

        protected void lbtnEnviarFacturaMailPorCliente_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalMailPorCliente", "openModalMailPorCliente();", true);
        }

        protected DataTable lstPago
        {

            get
            {
                if (Session["ListaPagos2"] != null)
                {
                    return (DataTable)Session["ListaPagos2"];
                }
                else
                {
                    return lstPagosTemp;
                }
            }
            set
            {
                Session["ListaPagos2"] = value;
            }
        }

        private void InicializarListaPagos()
        {
            try
            {
                lstPagosTemp.Columns.Add("Importe");
                lstPagosTemp.Columns.Add("Resta");
                lstPagosTemp.Columns.Add("Fecha");
                lstPagosTemp.Columns.Add("Observacion");
                lstPagosTemp.Columns.Add("IdFactura");
                lstPago = lstPagosTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Se produjo un error generado Listas " + ex.Message));

            }

        }

        protected void btnAgregarPago_Click(object sender, EventArgs e)
        {
            try
            {

                if (CorroborarTotalPagos() == 1)
                {
                    Clientes_Eventos eventoCliente = new Clientes_Eventos();
                    ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                    controladorFacturacion controladorFacturacion = new controladorFacturacion();
                    PagosProgramados pagosProgramados = new PagosProgramados();

                    var factura = controladorFacturacion.obtenerFacturaId(Convert.ToInt32(idFactura.Value));

                    DataTable dt = this.lstPago;

                    if (hiddenEditarPago.Value == "1")
                    {
                        controladorFacturacion.BorrarPagos(factura.id);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {

                        string fecha = dr["Fecha"].ToString();
                        pagosProgramados.IdDocumento = Convert.ToInt32(dr["IdFactura"]);

                        pagosProgramados.Importe = Convert.ToDecimal(dr["Importe"]);
                        //if (hiddenEditarPago.Value == "1")
                        //{
                        //    pagosProgramados.Importe = Convert.ToDecimal(dr["Importe"]);

                        //}
                        //else
                        //{
                        //    pagosProgramados.Importe = Convert.ToInt32(dr["Importe"]);

                        //}

                        pagosProgramados.Fecha = Convert.ToDateTime(fecha);

                        //if (hiddenEditarPago.Value == "1")
                        //{
                        //    //pagosProgramados.Fecha = Convert.ToDateTime(fecha);
                        //    pagosProgramados.Fecha = DateTime.ParseExact(fecha, "dd/MM/yyyy", new CultureInfo("es-AR"));

                        //}
                        //else
                        //{
                        //    pagosProgramados.Fecha = Convert.ToDateTime(fecha);

                        //}

                        pagosProgramados.Observacion = dr["Observacion"].ToString();
                        pagosProgramados.IdCliente = factura.cliente.id;


                        var cronogramaPago = controladorFacturacion.AgregarPago(pagosProgramados);

                        if (cronogramaPago != null)
                        {
                            eventoCliente.Cliente = factura.cliente.id;
                            eventoCliente.Fecha = DateTime.Now;
                            eventoCliente.Descripcion = "Cronograma de pago: " + Convert.ToDecimal(dr["Importe"]);
                            eventoCliente.Tarea = "";
                            eventoCliente.Vencimiento = Convert.ToDateTime(fecha);
                            eventoCliente.Estado = 1;
                            if (Session["Login_IdUser"] != null)
                            {
                                eventoCliente.Usuario = (int)Session["Login_IdUser"];
                            }
                            controladorClienteEntity.agregarEventoCliente(eventoCliente);

                        }


                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pagos no realizados ya que no coincide los importe con el total. ", "FacturasF.aspx"));
                }


                lstPago.Clear();
                idFactura.Value = null;
                hiddenTotalPago.Value = null;
                this.lblAvisoImporte.Visible = false;
                hiddenEditarPago.Value = "0";

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pagos programados agregado con exito. ", "FacturasF.aspx"));

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al generar pagos programables btnAgregarPago_Click, FacturaF, GestionWEB, error: " + ex.Message);
            }

        }

        private int CorroborarTotalPagos()
        {
            try
            {

                decimal total = 0;
                decimal totalFC = Convert.ToDecimal(this.hiddenTotalPago.Value);
                DataTable dt = this.lstPago;
                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["Importe"].ToString());
                }

                if (total == totalFC)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando monto igresado con tarjeta." + ex.Message));
                return -1;
            }
        }

        protected void lbtnAgregarImporte_Click(object sender, EventArgs e)
        {
            try
            {
                //Factura f = Session["Factura"] as Factura;
                //if (f.items.Count > 0)
                //{
                decimal montoIngresado = Convert.ToDecimal(this.txtImportePago.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                decimal totalFactura = Convert.ToDecimal(this.txtTotalPago.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                controladorMoneda contMoneda = new controladorMoneda();

                //System.Diagnostics.Debug.WriteLine(ViewState["ListaPagos"].ToString());

                DataTable dt = this.lstPago;

                decimal sumaDeMontosPagos = 0;

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sumaDeMontosPagos += Convert.ToDecimal(dr["Importe"]);
                    }
                }

                sumaDeMontosPagos += montoIngresado;

                if (totalFactura >= sumaDeMontosPagos && montoIngresado > 0)
                {
                    DataRow dr = dt.NewRow();

                    dr["Importe"] = montoIngresado;
                    dr["Resta"] = montoIngresado;
                    dr["Observacion"] = txtObservacionPago.Text;
                    dr["Fecha"] = Convert.ToDateTime(txtFechaPago.Text, new CultureInfo("es-AR")).ToString();
                    dr["IdFactura"] = idFactura.Value;

                    dt.Rows.Add(dr);

                    lstPago = dt;

                    this.cargarTablaPAgos();
                    this.txtImportePago.Text = "";
                    this.txtObservacionPago.Text = "";

                    this.lblAvisoImporte.Visible = false;
                }
                else
                {
                    this.lblAvisoImporte.Text = "El Monto Ingresado supera al total de la factura. ";
                    this.lblAvisoImporte.Visible = true;
                    this.txtImportePago.Focus();
                    this.txtImportePago.Text = "0";
                }
                //}
                //else
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos a la factura "));
                //}
                MostrarElEfectivoRestanteAlTxtEfectivoDePagosConTarjeta();
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarTablaPAgos()
        {
            try
            {

                if (!string.IsNullOrEmpty(txtTotalPago.Text))
                {
                    decimal totalFactura = Convert.ToDecimal(this.txtTotalPago.Text.Replace(',', '.'), CultureInfo.InvariantCulture);

                    DataTable dt = this.lstPago;

                    this.phPagosProgramables.Controls.Clear();

                    foreach (DataRow dr in dt.Rows)
                    {
                        int pos = dt.Rows.IndexOf(dr);
                        this.cargarPHPagosProgramables(dr, pos);
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista Pagos " + ex.Message));
            }
        }
        protected void cargarPHPagosProgramables(DataRow dr, int pos)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas

                //TableCell celCodigo = new TableCell();
                //celCodigo.Text = dr["DetallePago"].ToString();
                //celCodigo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Fecha"].ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Center;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                TableCell celNeto = new TableCell();
                celNeto.Text = dr["Importe"].ToString();
                celNeto.HorizontalAlign = HorizontalAlign.Center;
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNeto);

                TableCell celObservacion = new TableCell();
                celObservacion.Text = dr["Observacion"].ToString();
                celObservacion.HorizontalAlign = HorizontalAlign.Center;
                celObservacion.VerticalAlign = VerticalAlign.Middle;
                celObservacion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celObservacion);


                TableCell celAccion = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminar_" + pos.ToString();
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarPago);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phPagosProgramables.Controls.Add(tr);
            }
            catch
            {

            }
        }

        private void QuitarPago(object sender, EventArgs e)
        {
            try
            {
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                //obtengo el pedido del session
                DataTable dt = lstPago;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dt.Rows.IndexOf(dr).ToString() == codigo[1])
                    {
                        //lo quito
                        dt.Rows.RemoveAt(Convert.ToInt32(codigo[1]));
                        this.lblAvisoImporte.Visible = false;
                        break;
                    }
                }
                //cargo el nuevo pedido a la sesion
                lstPago = dt;

                //vuelvo a cargar los items
                this.cargarTablaPAgos();

                this.MostrarElEfectivoRestanteAlTxtEfectivoDePagosConTarjeta();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error quitando el pago. Por favor, contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        private void MostrarElEfectivoRestanteAlTxtEfectivoDePagosConTarjeta()
        {
            try
            {
                DataTable dt = lstPago;
                decimal resta = 0;
                foreach (DataRow row in dt.Rows)
                {
                    resta += Convert.ToDecimal(row["Importe"]);
                }

                this.txtRestaPago.Text = (Convert.ToDecimal(txtTotalPago.Text) - resta).ToString();

                if (txtRestaPago.Text == "0" || txtRestaPago.Text == "0.00")
                {
                    //btnAgregarPago1.Attributes.Remove("disabled");
                    //btnAgregarPago1.Attributes.Add("disabled", "false");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "Showbutton();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "Hidebutton();", true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnPagosProgramados_Click(object sender, EventArgs e)
        {

            controladorFacturacion controladorFacturacion = new controladorFacturacion();
            this.editarPago = 0;

            string idtildado = "";
            foreach (Control C in phFacturas.Controls)
            {
                TableRow tr = C as TableRow;
                CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;

                //!tr.Cells[1].Text.Contains("Presupuesto") && !tr.Cells[1].Text.Contains("Nota de Credito PRP")

                if (ch.Checked == true && (tr.Cells[1].Text.Contains("Presupuesto") || tr.Cells[1].Text.Contains("Factura A") || tr.Cells[1].Text.Contains("Factura B") || tr.Cells[1].Text.Contains("Factura C")))
                {
                    idtildado = ch.ID.Split('_')[1];
                }
            }
            if (!String.IsNullOrEmpty(idtildado))
            {
                var factura = controladorFacturacion.obtenerFacturaId(Convert.ToInt32(idtildado));

                txtTotalPago.Text = Convert.ToDecimal(factura.total).ToString();
                txtRestaPago.Text = Convert.ToDecimal(factura.total).ToString();
                idFactura.Value = factura.id.ToString();

                hiddenTotalPago.Value = txtTotalPago.Text;

                if (BuscarPagosProgramadosByFactura(factura.id) == 1)
                {
                    this.cargarTablaPAgos();
                    this.txtImportePago.Text = "";
                    this.txtObservacionPago.Text = "";
                    hiddenEditarPago.Value = "1";
                    txtRestaPago.Text = "0";
                }
                else
                {
                    lstPago.Clear();
                    hiddenEditarPago.Value = "0";
                }

                this.lblAvisoImporte.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalPagos();", true);

            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una Factura o Presupuesto"));
            }
        }

        public int BuscarPagosProgramadosByFactura(int idFactura)
        {
            controladorFacturacion controladorFacturacion = new controladorFacturacion();


            var tareas = controladorFacturacion.BuscarTareasProgramadosByIdFactura(idFactura);
            lstPago.Clear();

            if (tareas != null && tareas.Count > 0)
            {
                DataTable dt = this.lstPago;


                foreach (var tarea in tareas)
                {
                    DataRow dr = dt.NewRow();

                    dr["Importe"] = tarea.Importe;
                    dr["Resta"] = "0";
                    dr["Observacion"] = tarea.Observacion;
                    dr["Fecha"] = tarea.Fecha;
                    dr["IdFactura"] = tarea.IdDocumento;

                    dt.Rows.Add(dr);

                    lstPago = dt;
                }

                return 1;
            }


            return -1;

        }

        /// <summary>
        /// Metodo utilizado para buscar las facturas por numero o razon social del cliente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarNumerosFacturas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtNumeroFactura.Text))
            {
                if (string.IsNullOrEmpty(this.txtRazonSocial.Text) && string.IsNullOrEmpty(this.txtObservacion.Text))
                    Response.Redirect("FacturasF.aspx?a=1&n=" + this.txtNumeroFactura.Text);
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe escribir en una sola caja de texto. "));
            }
            if (!string.IsNullOrEmpty(this.txtRazonSocial.Text))
            {
                if (string.IsNullOrEmpty(this.txtObservacion.Text) && string.IsNullOrEmpty(this.txtNumeroFactura.Text))
                    Response.Redirect("FacturasF.aspx?a=2&rz=" + this.txtRazonSocial.Text);
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe escribir en una sola caja de texto. "));
            }
            if (!string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                if (string.IsNullOrEmpty(this.txtNumeroFactura.Text) && string.IsNullOrEmpty(this.txtRazonSocial.Text))
                    Response.Redirect("FacturasF.aspx?a=3&o=" + this.txtObservacion.Text);
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe escribir en una sola caja de texto. "));
            }
            if (string.IsNullOrEmpty(this.txtNumeroFactura.Text) && string.IsNullOrEmpty(this.txtRazonSocial.Text) && string.IsNullOrEmpty(this.txtObservacion.Text))
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe escribir en al menos una caja de texto. "));
        }

        /// <summary>
        /// Este metodo, levanta un PopUp para elegir en que divisa desea imprimir la FC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnImprimirFC_En_Otra_Divisa_Click(object sender, EventArgs e)
        {
            try
            {
                controladorMoneda controladorMoneda = new controladorMoneda();

                string idsListaFacturasTildados = "";
                int contadorFacturasTildadas = 0;
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        contadorFacturasTildadas++;
                        idsListaFacturasTildados += ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idsListaFacturasTildados) && contadorFacturasTildadas == 1)
                {
                    DropListDivisa.ClearSelection();

                    controladorCobranza controladorCobranza = new controladorCobranza();
                    controladorFacturacion controladorFacturacion = new controladorFacturacion();
                    ControladorFacturaMoneda controladorFacturaMoneda = new ControladorFacturaMoneda();

                    Factura factura = controladorFacturacion.obtenerFacturaId(Convert.ToInt32(idsListaFacturasTildados));
                    lblNumeroFC.Text = factura.numero;
                    this.hfIDFactura.Value = factura.id.ToString();
                    DataTable dt = controladorCobranza.obtenerMonedasDT();

                    //agrego todos
                    //DataRow dr = dt.NewRow();
                    //dr["moneda"] = "Seleccione...";
                    //dr["id"] = -1;
                    //dt.Rows.InsertAt(dr, 0);

                    this.DropListDivisa.DataSource = dt;
                    this.DropListDivisa.DataValueField = "id";
                    this.DropListDivisa.DataTextField = "moneda";
                    this.DropListDivisa.DataBind();

                    //Verificar si tiene alguna divisa por defecto guardada en la tabla Factuas_Moneda
                    Facturas_Moneda facturas_Moneda = controladorFacturaMoneda.ObtenerFacturaMonedaById(factura.id);
                    if (facturas_Moneda != null)
                    {
                        DropListDivisa.SelectedValue = Convert.ToString(facturas_Moneda.idMoneda);
                        txtCotizacion.Text = Convert.ToString(facturas_Moneda.ValorMoneda);
                        string monedaGuardada = DropListDivisa.Items.FindByValue(DropListDivisa.SelectedValue).Text;
                        lblFacturaMonedaGuardada.Text = monedaGuardada + ": $";
                        lblFacturaMonedaValor.Text = facturas_Moneda.ValorMoneda.ToString();
                    }
                    else
                    {
                        DropListDivisa.SelectedValue = DropListDivisa.Items.FindByText("Pesos").Value;
                        txtCotizacion.Text = "1.00";
                        lblFacturaMonedaGuardada.Text = "-";
                        lblFacturaMonedaValor.Text = "";
                    }

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalImprimirFC_EnOtraDivisa", "openModalImprimirFC_EnOtraDivisa();", true);
                }
                else if (contadorFacturasTildadas > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar <strong style='color:black'>solo</strong> un documento"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos <strong style='color:black'>un</strong> documento"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrio un error en FacturasF.lbtnImprimirFC_En_Otra_Divisa_Click. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor, contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        protected void DropListDivisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                controladorCobranza controladorCobranza = new controladorCobranza();
                string moneda = lblFacturaMonedaGuardada.Text;
                if(moneda != "-")
                {
                    moneda = moneda.Replace(": $", string.Empty);
                }
                if (!string.IsNullOrEmpty(lblFacturaMonedaValor.Text) && DropListDivisa.SelectedItem.Text.Contains(moneda))
                {
                    txtCotizacion.Text = lblFacturaMonedaValor.Text.Replace(',', '.');
                }
                else
                {
                    txtCotizacion.Text = Decimal.Round(controladorCobranza.obtenerCotizacion(Convert.ToInt32(DropListDivisa.SelectedValue)), 2).ToString().Replace(',', '.');
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrio un error en FacturasF.DropListDivisa_SelectedIndexChanged. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor, contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        protected void btnImprimirFCDivisa_Click(object sender, EventArgs e)
        {
            try
            {
                controladorFacturacion controladorFacturacion = new controladorFacturacion();
                //Factura factura = controladorFacturacion.obtenerFacturaByNumero(lblNumeroFC.Text).FirstOrDefault();
                Factura factura = controladorFacturacion.obtenerFacturaId(Convert.ToInt32(this.hfIDFactura.Value));
                if (factura != null)
                    this.DetalleFactura_OtraDivisa(factura);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrio un error en FacturasF.DropListDivisa_SelectedIndexChanged. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor, contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        protected void DetalleFactura_OtraDivisa(Factura factura)
        {
            try
            {
                //obtengo numero factura

                string idFactura = Convert.ToString(factura.id);
                int tipo = factura.tipo.id;

                TipoDocumento tp = this.contDocumentos.obtenerTipoDoc("Presupuesto");

                if (tipo == tp.id || tipo == 11 || tipo == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "&div=" + DropListDivisa.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    if (tipo == 1 || tipo == 9 || tipo == 4 || tipo == 24 || tipo == 25 || tipo == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "&div=" + DropListDivisa.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

                    }
                    else
                    {
                        //facturab
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "&div=" + DropListDivisa.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "CATCH: Error cargando articulos detalle desde la interfaz. Ubicacion: FacturasF.DetalleFactura_OtraDivisa. Excepcion:" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error al mostrar el detalle de la factura desde la interfaz. Por favor, contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        protected void btnExportarPRPFacturados_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelPresupuesto(15);
            }
            catch (Exception ex)
            {
                Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "error", ex.Message.ToString());
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error exportando a excel. " + ex.Message));
            }
        }

        protected void btnBuscarCodPresupuesto_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodClientePresupuesto.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                this.ListClientePresupuesto.DataSource = dtClientes;
                this.ListClientePresupuesto.DataValueField = "id";
                this.ListClientePresupuesto.DataTextField = "alias";
                this.ListClientePresupuesto.DataBind();

            }
            catch (Exception ex)
            {
                Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "error", ex.Message.ToString());
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

    
    }
}