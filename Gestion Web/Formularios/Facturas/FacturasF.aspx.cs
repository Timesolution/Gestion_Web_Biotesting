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

        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int tipo;
        private int cliente;
        private int tipofact;
        private int lista;
        private int anuladas;
        private int empresa;
        private int vendedor;
        private int formaPago;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                //datos de filtro
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                empresa = Convert.ToInt32(Request.QueryString["Emp"]);
                tipo = Convert.ToInt32(Request.QueryString["tipo"]);
                tipofact = Convert.ToInt32(Request.QueryString["doc"]);
                cliente = Convert.ToInt32(Request.QueryString["cl"]);
                lista = Convert.ToInt32(Request.QueryString["ls"]);
                anuladas = Convert.ToInt32(Request.QueryString["e"]);
                vendedor = Convert.ToInt32(Request.QueryString["vend"]);
                formaPago = Convert.ToInt32(Request.QueryString["fp"]);

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.verificarModoBlanco();

                    //Verifico si tiene habilitado el boton Facturar PRP según la configuracion
                    this.verificarFacturarPRP();

                    if (fechaD == null && fechaH == null && suc == 0 && cliente == 0 && empresa == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        empresa = (int)Session["Login_EmpUser"];
                        this.cargarEmpresas();
                        this.cargarSucursalByEmpresa(empresa);
                        //this.cargarSucursal();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        //tipo de documento??
                        tipo = 0;
                        tipofact = 0;
                        cliente = 0;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesdeDto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHastaDto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListEmpresa.SelectedValue = empresa.ToString();
                        ListEmpresaDto.SelectedValue = empresa.ToString();
                        this.cargarSucursal();
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListTipo.SelectedValue = tipo.ToString();

                    }
                    this.cargarEmpresas();
                    this.cargarSucursalByEmpresa(empresa);
                    this.cargarPuntosVenta(Convert.ToInt32(this.DropListSucursalAgregarFC.SelectedValue));
                    
                    this.cargarClientes();
                    this.cargarListaPrecio();
                    this.cargarFormaPago();
                    this.cargarChkListaPrecio();
                    this.cargarVendedores();

                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                    DropListEmpresa.SelectedValue = empresa.ToString();
                    DropListTipo.SelectedValue = tipo.ToString();
                    DropListDocumento.SelectedValue = tipofact.ToString();
                    DropListClientes.SelectedValue = cliente.ToString();
                    DropListListas.SelectedValue = lista.ToString();
                    DropListVendedor.SelectedValue = vendedor.ToString();
                    DropListFormasPago.SelectedValue = formaPago.ToString();                    
                    this.chkAnuladas.Checked = Convert.ToBoolean(this.anuladas);
                }
                btnAgregarFC.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregarFC, null) + ";");
                btnCalcularDiferenciaCambio.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnCalcularDiferenciaCambio, null) + ";");
                btnGenerarNotaDebitoCreditoDiferenciaCambio.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnGenerarNotaDebitoCreditoDiferenciaCambio, null) + ";");

                //verifico si el perfil tiene permiso para anular
                this.verficarPermisoAnular();
                //verifico si el perfil tiene permiso para editar FC y agregar FC
                this.verificarPermisoEditarFC();
                this.verificarPermisoAgregarFC();
                this.cargarFacturasRango2(fechaD, fechaH, suc, tipo, cliente, tipofact, lista, empresa, vendedor, formaPago);
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

                        //Permiso boton Nota de Credito seleccionando PRP
                        string permisoNotaCreditoDesdePRP = listPermisos.Where(x => x == "148").FirstOrDefault();
                        if (string.IsNullOrEmpty(permisoNotaCreditoDesdePRP))
                            this.lbtnNotaCredito.Visible = false;
                    }
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
                if(config.modoBlanco == "1")
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
                ListItem item = new ListItem("Seleccione...","-1");
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
        private void cargarFacturasRango(string fechaD, string fechaH, int idSuc, int tipo, int idCliente, int tipofact, int idLista, int idEmp, int idVendedor, int formaPago)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && tipo != 0 && idCliente != 0)
                {
                    List<Factura> Facturas = controlador.obtenerFacturasRango(fechaD, fechaH, idSuc, tipo, idCliente, tipofact, idLista, this.anuladas, idEmp, idVendedor, formaPago);
                    decimal saldo = 0;
                    decimal neto = 0;
                    decimal iva = 0;
                    decimal ib = 0;
                    foreach (Factura f in Facturas)
                    {
                        this.cargarEnPh(f);
                        if (f.estado == 1)
                        {
                            saldo += f.total;
                            neto += f.netoNGrabado;
                            iva += f.neto21;
                            ib += f.retencion;
                        }
                    }

                    labelSaldo.Text = "$" + saldo.ToString("N");

                }
                else
                {
                    List<Factura> Facturas = controlador.obtenerFacturasRango(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListDocumento.SelectedValue), Convert.ToInt32(DropListListas.SelectedValue), this.anuladas, Convert.ToInt32(DropListEmpresa.SelectedValue), Convert.ToInt32(DropListVendedor.SelectedValue), Convert.ToInt32(DropListFormasPago.SelectedValue));
                    decimal saldo = 0;
                    decimal neto = 0;
                    decimal iva = 0;
                    decimal ib = 0;
                    foreach (Factura f in Facturas)
                    {
                        this.cargarEnPh(f);
                        if (f.estado == 1)
                        {
                            saldo += f.total;
                            neto += f.netoNGrabado;
                            iva += f.neto21;
                            ib += f.retencion;
                        }
                    }

                    labelSaldo.Text = "$" + saldo.ToString("N");

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando facturas.  " + ex.Message));
            }
        }
        private void cargarFacturasRango2(string fechaD, string fechaH, int idSuc, int tipo, int idCliente, int tipofact, int idLista, int idEmp, int idVendedor, int formaPago)
        {
            try
            {
                DataTable dtFacturas = controlador.obtenerFacturasRangoTipoDTLista(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue), Convert.ToInt32(DropListTipo.SelectedValue), Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListDocumento.SelectedValue), Convert.ToInt32(DropListListas.SelectedValue), this.anuladas, Convert.ToInt32(DropListEmpresa.SelectedValue), Convert.ToInt32(DropListVendedor.SelectedValue), Convert.ToInt32(DropListFormasPago.SelectedValue));
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

                //LinkButton btnEliminar = new LinkButton();
                //btnEliminar.ID = "btnEliminar_" + f.id;
                //btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.Attributes.Add("data-toggle", "modal");
                //btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                //btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                ////btnEliminar.Font.Size = 9;
                ////btnEliminar.Click += new EventHandler(this.eliminarCobro);
                ////btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                //btnEliminar.OnClientClick = "abrirConfirmacion(" + f.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                //Literal l2 = new Literal();
                //l2.Text = "&nbsp";
                //celAccion.Controls.Add(l2);

                //Literal l3 = new Literal();
                //l3.Text = "&nbsp";
                //celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + f.id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Controls.Add(btnEliminar);


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
        private void cargarEnPhDR(DataRow row)
        {
            try
            {
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
            
                if(!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if(DropListSucursal.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(),"alert",m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                  ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch(Exception ex)
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
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
                        if(i == -1)
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

                    int i = this.controlador.ProcesoRefacturarPRP(idtildado, user, idClienteAux);//this.controlador.ProcesoEliminarFactura(idtildado, user);
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
            catch(Exception ex)
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
                    if (ch.Checked == true && tr.Cells[1].Text.Contains("Presupuesto"))
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
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
            catch(Exception ex)
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
                Decimal valor = 0;
                foreach (Control C in phFacturas.Controls)
                {

                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
                    //valor += Convert.ToDecimal(tr.Cells[4].Text);
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                //Session.Add("datosMov", dtDatos);
                if (!String.IsNullOrEmpty(idtildado))
                {
                    //Session.Add("valor", valor);
                    Response.Redirect("GuiaDespachoF.aspx?accion=6&f=" + idtildado);
                }
                //else
                //{
                //    Response.Redirect("GuiaDespachoF.aspx?accion=7");
                //}

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
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
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
                //foreach (GridViewRow row in GridVentas.Rows)
                //{
                //    if (row.RowType == DataControlRowType.DataRow)
                //    {
                //        CheckBox chkRow = (row.Cells[9].FindControl("chkRow") as CheckBox);
                //        if (chkRow.Checked)
                //        {
                //            string name = row.Cells[1].Text;
                //        }
                //    }
                //}

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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
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

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ","FacturasF.aspx"));

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
                Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(this.txtIdEnvioFC.Text));
                string destinatarios = this.txtEnvioMail.Text + ";" + this.txtEnvioMail2.Text;
                String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");

                int i = this.GenerarImpresionPDF(f, path);
                if (i > 0)
                {
                    Attachment adjunto = new Attachment(path);

                    int ok = this.contFunciones.enviarMailFactura(adjunto, f, destinatarios);
                    if (ok > 0)
                    {
                        adjunto.Dispose();
                        File.Delete(path);
                        this.txtEnvioMail.Text = "";
                        this.txtEnvioMail2.Text = "";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Factura enviada correctamente!", ""));
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
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando factura por mail. " + ex.Message));
            }
        }
        protected void lbtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity contCliEnt = new ControladorClienteEntity();

                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    this.txtIdEnvioFC.Text = idtildado;

                    Factura f = this.controlador.obtenerFacturaId(Convert.ToInt32(this.txtIdEnvioFC.Text));
                    var datos = contCliEnt.obtenerClienteDatosByCliente(f.cliente.id);

                    if (datos != null)
                    {
                        foreach (var d in datos)
                        {
                            this.txtEnvioMail.Text += d.Mail + ";";
                        }
                    }


                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalMail", "openModalMail();", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un documento!. "));
                }
            }
            catch(Exception ex)
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
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
                string idtildado = "";
                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
                    if (ch.Checked == true && !tr.Cells[1].Text.Contains("Presupuesto") && !tr.Cells[1].Text.Contains("Nota de Credito PRP"))
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Inicio EDITAR factura id: " + idtildado);
                    this.lblIdFact.Text = idtildado;
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "Obtengo fc desde la base");
                    Factura FC = this.controlador.obtenerFacturaId(Convert.ToInt32(idtildado));
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo id");
                    this.lblIdFactura.Text = FC.id.ToString();
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo numero factura");
                    this.lblNroFactura.Text = FC.numero;
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe neto");
                    this.lblNetoFactura.Text = FC.subTotal.ToString("'$'#,0.00");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe Iva");
                    this.lblIvaFactura.Text = FC.neto21.ToString("'$'#,0.00");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe total");
                    this.lblTotalFactura.Text = FC.total.ToString("'$'#,0.00");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo numero punto de venta en el campo del nuevo punto de venta");
                    this.txtNuevoPuntoVenta.Text = FC.numero.Substring(0, 4);
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo numero factura en el campo del nuevo numero de factura");
                    this.txtNuevoNumeroFactura.Text = FC.numero.Substring(5, 8);
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo fecha");
                    this.lblFechaFactura.Text = FC.fecha.ToString("dd/MM/yyyy");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo fecha en el campo de la nueva fecha de factura");
                    this.txtNuevaFecha.Text = FC.fecha.ToString("dd/MM/yyyy");
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe neto en el campo del nuevo importe neto de factura");
                    this.txtNuevoNetoFactura.Text = FC.subTotal.ToString();
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe iva en el campo del nuevo importe iva de factura");
                    this.txtNuevoIvaFactura.Text = FC.neto21.ToString();
                    Gestion_Api.Modelo.Log.EscribirSQL(1, "INFO", "cargo importe total en el campo del nuevo importe total de factura");
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
            catch(Exception ex)
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
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
                            //Factura f = new Factura();
                            //f = this.controlador.obtenerFacturaId(Convert.ToInt32(id));
                            //if (f != null)
                            //{
                            //    int i = this.contRemito.RemitirDesdeFactura(f);

                            //    if (i < 1)
                            //    {
                            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Remito desde factura. "));
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo factura a remitir. "));
                            //    return;
                            //}
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;

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
        private int GenerarImpresionPDF(Factura f,string pathGenerar)
        {
            try
            {
                
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
                if (f.tipo.tipo.Contains("Factura B") || f.tipo.tipo.Contains("Debito B") || f.tipo.tipo.Contains("Credito B"))
                {
                    #region Fact B
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
                    string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc + "/Logo.jpg");
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
                if(this.verificarPuntoVentaElectronico(Convert.ToInt32(this.DropListPuntoVentaAgregarFC.SelectedValue)))
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
                        if(actualizar > 0)
                        {
                            int j = 0;
                            for (int i = 0; i <= cantidad; i++)
                            {
                                j += this.controlador.agregarFcManual(idEmpresa, idSucursal, idVendedor, idList, idPtoVta, idCliente, td, a);
                            }
                            if (j == cantidad + 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Las facturas fueron agregadas exitósamente. "));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Se cargaron " + j + " facturas de " + cantidad+1 + " seleccionadas."));
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
                if(pv.formaFacturar != "Electronica")
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
                int idFact = Convert.ToInt32(this.lblIdFact.Text);
                string puntoVenta = this.txtNuevoPuntoVenta.Text;
                string fecha = this.txtNuevaFecha.Text;
                string numero = this.txtNuevoNumeroFactura.Text;
                string neto = this.txtNuevoNetoFactura.Text;
                string iva = this.txtNuevoIvaFactura.Text;
                string total = this.txtNuevoTotalFactura.Text;

                int cliente = Convert.ToInt32(ListClientesEditar.SelectedValue);

                int i = this.controlador.modificarTotalesFactura(idFact, neto, iva, total, numero, puntoVenta, fecha,cliente);
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar la factura. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
                }
            }
            catch (Exception ex)
            {
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error modificando datos de factura " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar la factura. ", "FacturasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Emp=" + DropListEmpresa.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&doc=" + DropListDocumento.SelectedValue + "&cl=" + DropListClientes.SelectedValue + "&ls=" + DropListListas.SelectedValue + "&vend=" + Convert.ToInt32(this.DropListVendedor.SelectedValue) + "&e=" + Convert.ToInt32(this.chkAnuladas.Checked) + "&fp=" + Convert.ToInt32(this.DropListFormasPago.SelectedValue)));
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
                    CheckBox ch = tr.Cells[9].Controls[2] as CheckBox;
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

                    int i = this.controlador.GenerarNotaCreditoDebitoDiferenciaCambio(1, Convert.ToDecimal(lblDiferenciaCambio.Text), factura, tipoNota);

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
                        var divMonedaOriginal = factura.total / Convert.ToDecimal(lblTipoCambioOriginalNotaDebitoCreditoDiferenciaCambio.Text);
                        var prodMonedaNueva = divMonedaOriginal * Convert.ToDecimal(txtNuevoCambioNotaDebitoCreditoDiferenciaCambio.Text);
                        var diferencia = prodMonedaNueva - factura.total;

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


    }
} 