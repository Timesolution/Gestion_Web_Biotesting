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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Cobros
{
    public partial class CobrosRealizadosF : System.Web.UI.Page
    {
        int TeamHermanos = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("ReporteVendedores"));
        controladorCobranza contCobranza = new controladorCobranza();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorCuentaCorriente contrCC = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();
        private string fechaD;
        private string fechaH;
        private int idCliente;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private int idTipo;
        private int vendedor;
        private int filtro = 0;
        int firstLoad = 0;//Esta variable sirve para indicar la primera carga de movimientos del dia actual

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (TeamHermanos == 1)
                {
                    BtnExcelVendedores.Visible = false;
                }
                this.VerificarLogin();
                this.idCliente = Convert.ToInt32(Request.QueryString["cliente"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["empresa"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["sucursal"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);
                this.idTipo = Convert.ToInt32(Request.QueryString["tipo"]);
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.vendedor = Convert.ToInt32(Request.QueryString["vend"]);
                filtro = Convert.ToInt32(Request.QueryString["filtro"]);

                if (!IsPostBack)
                {
                    this.verificarModoBlanco();
                    if (idEmpresa == 0 && idSucursal == 0)
                    {
                        //this.idCliente = 1;
                        this.idSucursal = (int)Session["Login_SucUser"];
                        this.idEmpresa = (int)Session["Login_EmpUser"];
                        this.fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        this.idCliente = -1;
                        //this.puntoVenta = this.contCobranza.obtenerPrimerPuntoVenta(idSucursal, idEmpresa);
                        //this.puntoVenta = 1;
                    }
                    this.cargarClientes();
                    this.cargarVendedores();
                    this.DropListClientes.SelectedValue = this.idCliente.ToString();
                    this.DropListVendedores.SelectedValue = this.vendedor.ToString();
                    this.cargarEmpresas();
                    this.DropListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.DropListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.DropListPuntoVta.SelectedValue = this.puntoVenta.ToString();
                    this.DropListTipo.SelectedValue = this.idTipo.ToString();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                }
                //if (this.idCliente > -1 && filtro == 1)
                //{
                this.cargarMovimientos();
                //}
                this.Form.DefaultButton = lbBuscar.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros Realizados") != 1)
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
                        if (s == "42")
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
        public void verificarModoBlanco()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.modoBlanco == "1")
                {
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("Ambos"));
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("PRP"));
                    this.idTipo = 0;
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
                    int idVendedor = (int)Session["Login_Vendedor"];
                    dt = contCliente.obtenerClientesByVendedorDT(idVendedor);

                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                }
                else
                {
                    dt = contCliente.obtenerClientesDT();
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }



                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cliente a la lista. " + ex.Message));
            }
        }
        private void cargarMovimientos()
        {
            try
            {
                int generarReporte = 0;

                if (idCliente == 0 && idEmpresa == 0 && idSucursal == 0 && puntoVenta == 0)
                {
                    this.idCliente = Convert.ToInt32(DropListClientes.SelectedValue);
                    this.idSucursal = (int)Session["Login_SucUser"];
                    this.idEmpresa = (int)Session["Login_EmpUser"];
                    this.puntoVenta = Convert.ToInt32(DropListPuntoVta.SelectedValue);
                    this.idTipo = Convert.ToInt32(DropListTipo.SelectedValue);
                    this.fechaD = this.txtFechaDesde.Text;
                    this.fechaH = this.txtFechaHasta.Text;
                    //cliente = this.contrCliente.obtenerClienteID(Convert.ToInt32(DropListClientes.SelectedValue));
                    //cuenta = this.contrCC.obtenerCuentaCorrienteCliente(Convert.ToInt32(DropListClientes.SelectedValue));
                    DataTable dtMovimiento = this.contrCC.Get_CobrosRealizadosDT(fechaD, fechaH, idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, idTipo, this.vendedor);
                    phCobrosRealizados.Controls.Clear();
                    decimal saldo = 0;

                    ///Si la cantidad de registros obtenidos es mayor a 2000, entonces generamos un reporte en segundo plano para que no lance el timeOut
                    //if (dtMovimiento != null && dtMovimiento.Rows.Count <= 2000)
                    //{

                    saldo += dtMovimiento.AsEnumerable().Sum(row => row.Field<decimal>("total"));
                    this.cargarEnPh(dtMovimiento);

                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));

                    //}
                    //else
                    //generarReporte = 1;


                    this.labelSaldo.Text = saldo.ToString("C");
                }
                else
                {
                    DataTable dtMovimiento = this.contrCC.Get_CobrosRealizadosDT(fechaD, fechaH, idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, idTipo, this.vendedor);
                    phCobrosRealizados.Controls.Clear();
                    decimal saldo = 0;

                    ///Si la cantidad de registros obtenidos es mayor a 2000, entonces generamos un reporte en segundo plano para que no lance el timeOut
                    //if (dtMovimiento != null && dtMovimiento.Rows.Count <= 2000)
                    //{

                    saldo += dtMovimiento.AsEnumerable().Sum(row => row.Field<decimal>("total"));
                    this.cargarEnPh(dtMovimiento);

                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));

                    //}
                    //else
                    //generarReporte = 1;

                    this.labelSaldo.Text = saldo.ToString("C");
                }

                if (generarReporte == 1 && filtro == 1)
                {
                    //SolicitarReporte_CobrosRealizados();
                    //filtro = 0;
                }
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CobrosRealizadosF.aspx. Metodo: cargarMovimientos. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }
        }

        #region Generar Pedido Reporte

        public void SolicitarReporte_CobrosRealizados()
        {
            try
            {
                ControladorInformesEntity controladorInformesEntity = new ControladorInformesEntity();
                Informes_Pedidos ip = new Informes_Pedidos();
                InformeXML infXML = new InformeXML();

                int id_a_setear = controladorInformesEntity.ObtenerUltimoIdInformePedido();

                if (id_a_setear >= 0)
                {
                    ///Cargo el objeto Informes_Pedidos
                    cargarDatosInformePedido(ip, 1);

                    ///Cargo el objeto InformeXML
                    cargarDatosInformeXML(infXML);

                    ///Concatenamos el ID de la insercion al reporte a guardar
                    ip.NombreInforme += (id_a_setear + 1).ToString();

                    ///Agrego el informe para ejecutar la funcion de reporte de filtro de ventas. Si todo es correcto retorna 1.
                    int i = controladorInformesEntity.generarPedidoDeInforme(infXML, ip);

                    if (i > 0)
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", mje.mensajeBoxInfo("Se ha generado una solicitud de reporte de cobros con el nombre de <strong>" + ip.NombreInforme + "</strong> porque la cantidad de registros encontrados es mayor a 2000. Podra visualizar el estado del reporte en <strong><a href='/Formularios/Reportes/InformesF.aspx'>Informes Solicitados</a></strong>.", null));
                    else
                    {
                        int idError = Log.ObtenerUltimoIDLog();
                        Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE: No pudo generar un pedido para el reporte de cobros. Ubicacion: ControladorInformesEntity. Metodo: generarPedidoDeInforme.");
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
                    }
                }
                else
                {
                    int idError = Log.ObtenerUltimoIDLog();
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE: No pudo generar un pedido para el reporte de cobros. Varible 'id_a_setear' es igual a -1. Ubicacion: ControladorInformesEntity. Metodo: ObtenerUltimoIdInformePedido.");
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
                }

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CobrosRealizadosF.aspx. Metodo: SolicitarReporte_CobrosRealizados. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }
        }

        /// <summary>
        /// Este metodo setea los campos al objeto InformeXML, para deserializar al momento de leer los parametros/configuraciones de la solicitud del informe.
        /// </summary>
        /// <param name="infXML"></param>
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
                infXML.PuntoVenta = Convert.ToInt32(this.DropListPuntoVta.SelectedValue);
                infXML.Tipo = Convert.ToInt32(this.DropListTipo.SelectedValue);
                infXML.Vendedor = Convert.ToInt32(this.DropListVendedores.SelectedValue);
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CobrosRealizadosF.aspx. Metodo: cargarDatosInformeXML. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }
        }

        /// <summary>
        /// Este metodo setea los campos al objeto de Informeas_Pedidos, recibiendo una accion para saber que tipo de informe es.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="accion"></param>
        public void cargarDatosInformePedido(Informes_Pedidos ip, int accion)
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                ip.Fecha = DateTime.Now;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;

                switch (accion)
                {
                    ///Informe para Cobros Realizados
                    case 1:
                        ip.Informe = 11;
                        ip.Usuario = (int)Session["Login_IdUser"];
                        ip.NombreInforme = "REPORTE-COBROS-REALIZADOS_";
                        break;
                    case 2:
                        ip.Informe = 13;
                        ip.Usuario = (int)Session["Login_IdUser"];
                        ip.NombreInforme = "REPORTE-COBROS-REALIZADOS-VENDEDORES_";
                        break;
                    default:
                        break;
                }

            }
            catch (Exception Ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CobrosRealizadosF.aspx. Metodo: cargarDatosInformePedido. Excepcion: " + Ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }

        }

        #endregion


        private void cargarLabel(string fechaD, string fechaH, int idCliente, int idPuntoVenta, int idEmpresa, int idSucursal, int idTipo)
        {
            try
            {
                string label = fechaD + "," + fechaH + ",";
                if (idCliente > 0)
                {
                    label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                }
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idEmpresa > 0)
                {
                    label += DropListEmpresa.Items.FindByValue(idEmpresa.ToString()).Text + ",";
                }
                if (idPuntoVenta > 0)
                {
                    label += DropListPuntoVta.Items.FindByValue(idPuntoVenta.ToString()).Text + ",";
                }
                if (idTipo > -1)
                {
                    label += DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                }

                this.lblParametros.Text = label;
                lbtnExportar.Visible = true;

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CobrosRealizadosF.aspx. Metodo: cargarLabel. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }
        }

        public void cargarVendedores()
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();
                this.DropListVendedores.Items.Clear();
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
                    this.DropListVendedores.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando vendedores. " + ex.Message));
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

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego opcion seleccione
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                //agrego opcion todos
                DataRow dr2 = dt.NewRow();
                dr2["NombreFantasia"] = "Todos";
                dr2["Id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListPuntoVta.DataSource = dt;
                this.DropListPuntoVta.DataValueField = "Id";
                this.DropListPuntoVta.DataTextField = "NombreFantasia";

                this.DropListPuntoVta.DataBind();

                if (dt.Rows.Count == 3)
                {
                    this.DropListPuntoVta.SelectedIndex = 2;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
            }
        }

        private void cargarEnPh(DataTable dt)
        {
            //MovimientoView movV = new MovimientoView();
            //movV = m.ListarMovimiento();

            try
            {

                foreach (DataRow row in dt.Rows)
                {
                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = row["id"].ToString();

                    TableCell celFecha = new TableCell();
                    celFecha.Text = ((DateTime)(row["fecha"])).ToString("dd/MM/yyyy");
                    celFecha.VerticalAlign = VerticalAlign.Middle;
                    celFecha.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celFecha);

                    TableCell celNumero = new TableCell();
                    celNumero.Text = row["TipoDocumento"].ToString() + " " + row["numero"].ToString().PadLeft(8, '0');
                    celNumero.VerticalAlign = VerticalAlign.Middle;
                    celNumero.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celNumero);

                    TableCell celCliente = new TableCell();
                    celCliente.Text = row["Cliente"].ToString();
                    celCliente.VerticalAlign = VerticalAlign.Middle;
                    celCliente.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celCliente);

                    TableCell celHaber = new TableCell();
                    celHaber.Text = "$" + row["haber"].ToString().Replace(',', '.');
                    celHaber.VerticalAlign = VerticalAlign.Middle;
                    celHaber.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celHaber);

                    TableCell celComentarios = new TableCell();
                    celComentarios.Text = row["Observaciones"].ToString();
                    celComentarios.VerticalAlign = VerticalAlign.Middle;
                    celComentarios.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celComentarios);

                    //agrego fila a tabla
                    TableCell celAccion = new TableCell();
                    LinkButton btnDetalles = new LinkButton();
                    btnDetalles.CssClass = "btn btn-info ui-tooltip";
                    btnDetalles.Attributes.Add("data-toggle", "tooltip");
                    btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                    btnDetalles.Attributes.Add("name", "btnNombre");
                    btnDetalles.ID = "btnSelec_" + row["id"].ToString();
                    btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                    btnDetalles.Click += new EventHandler(this.detalleCobro);
                    celAccion.Controls.Add(btnDetalles);

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAccion.Controls.Add(l2);

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + row["id"].ToString();
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.OnClientClick = "abrirdialog(" + row["id"].ToString() + ");";
                    celAccion.Controls.Add(btnEliminar);
                    celAccion.Width = Unit.Percentage(10);
                    tr.Cells.Add(celAccion);

                    phCobrosRealizados.Controls.Add(tr);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando movimiento a PH. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CobrosRealizadosF.aspx?filtro=1&Fechadesde=" + this.fechaD + "&FechaHasta=" + this.fechaH + "&cliente=" + DropListClientes.SelectedValue + "&empresa=" + DropListEmpresa.SelectedValue + "&sucursal=" + DropListSucursal.SelectedValue + "&puntoVenta=" + DropListPuntoVta.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&vend=" + DropListVendedores.SelectedValue);
                //cargarMovimientos();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }

        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }

        protected void DropListPuntoVta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void detalleCobro(object sender, EventArgs e)
        {
            try
            {
                ////obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCobro = atributos[1];
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCobro.aspx?Cobro=" + idCobro + "&valor=2', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message);
            }
        }

        private void eliminarCobro(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCobro = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "document.getElementById('abreDialog').click();", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idMovimiento = Convert.ToInt32(this.txtMovimiento.Text);
                int cierreOK = this.verificarCierreCaja(idMovimiento);
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya se realizo un cierre de caja en el dia de hoy para el punto de venta del cobro a anular."));
                    return;
                }

                //int i = this.contCobranza.ProcesoEliminarCobro(idMovimiento);
                int i = this.contCobranza.ProcesoEliminarCobroCompensacion(idMovimiento);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Recibo de Cobro. id mov: " + idMovimiento);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Cobro eliminado con exito", "CobrosRealizadosF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo eliminar cobro "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Cobro. " + ex.Message));

            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClientes = this.contrCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
                //this.cargarClientesTable(cliente);

                //this.ListRazonSocial.DataSource = dtClientes;
                //this.ListRazonSocial.DataValueField = "id";
                //this.ListRazonSocial.DataTextField = "razonSocial";

                //this.ListRazonSocial.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {

                //DataTable dtDatos = new DataTable();
                //dtDatos.Columns.Add("id");
                //dtDatos.Columns.Add("Fecha");
                //dtDatos.Columns.Add("Numero");
                //dtDatos.Columns.Add("Cliente");
                //dtDatos.Columns.Add("saldo");


                //foreach (var control in this.phCobrosRealizados.Controls)
                //{
                //    DataRow drDatos = dtDatos.NewRow();
                //    TableRow tr = control as TableRow;

                //    drDatos[0] = tr.ID;
                //    drDatos[1] = tr.Cells[0].Text;
                //    drDatos[2] = tr.Cells[1].Text;
                //    drDatos[3] = tr.Cells[2].Text;
                //    drDatos[4] = tr.Cells[3].Text;


                //    dtDatos.Rows.Add(drDatos);
                //}
                //Session.Add("datosRc", dtDatos);
                Session.Add("saldoRc", labelSaldo.Text);

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCobro.aspx?valor=6&fd="+this.fechaD+"&fh="+this.fechaH+"&cli="+this.idCliente+"&suc="+this.idSucursal+"&pv="+this.puntoVenta+"&e="+this.idEmpresa+"&t="+this.idTipo+"', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                Response.Redirect("ImpresionCobro.aspx?valor=6&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&pv=" + this.puntoVenta + "&e=" + this.idEmpresa + "&t=" + this.idTipo + "&ven=" + this.vendedor);
            }
            catch (Exception ex)
            {

            }

        }
        private int verificarCierreCaja(int idMov)
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                Movimiento_Cobro movCobro = this.contCobranza.obtenerMovimientoCobroID(idMov);
                int sucursal = movCobro.cob.sucursal.id;
                int ptoVenta = movCobro.cob.puntoVenta.id;

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

        protected void lbtnReporteCobranza_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("ImpresionCobro.aspx?valor=7&ex=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&pv=" + this.puntoVenta + "&e=" + this.idEmpresa + "&t=" + this.idTipo + "&ven=" + this.vendedor);

            }
            catch
            {

            }
        }

        private void SolicitarReporteCobranzaVendedores()
        {
            try
            {
                ControladorInformesEntity controladorInformesEntity = new ControladorInformesEntity();
                Informes_Pedidos ip = new Informes_Pedidos();
                InformeXML infXML = new InformeXML();

                infXML.FechaDesde = this.fechaD;
                infXML.FechaHasta = this.fechaH;
                infXML.Cliente = idCliente;
                infXML.Vendedor = this.vendedor;
                infXML.PuntoVenta = this.puntoVenta;
                infXML.Tipo = this.idTipo;
                infXML.Sucursal = this.idSucursal;

                ///Cargo el objeto Informes_Pedidos
                cargarDatosInformePedido(ip, 2);

                ///Cargo el objeto InformeXML

                ///Concatenamos el ID de la insercion al reporte a guardar
                ip.NombreInforme += (controladorInformesEntity.ObtenerUltimoIdInformePedido() + 1).ToString();

                ///Agrego el informe para ejecutar la funcion de reporte de filtro de ventas. Si todo es correcto retorna 1.
                int i = controladorInformesEntity.generarPedidoDeInforme(infXML, ip);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Se ha generado la solicitud de reporte de ventas con el nombre de <strong>" + ip.NombreInforme + "</strong> porque la cantidad de registros encontrados es mayor a 2000. Podra visualizar el estado del reporte en <strong><a href='/Formularios/Reportes/InformesF.aspx'>Informes Solicitados</a></strong>.", null));
                else
                {
                    int idError = Log.ObtenerUltimoIDLog();
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE: No pudo generar un pedido para el reporte de ventas. Ubicacion: Articulos.aspx. Metodo: cargarFacturasRango.");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
                }

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: Articulos.aspx. Metodo: cargarFacturasRango. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError(idError.ToString()));
            }

        }

        protected void lbtnReporteCobranzaPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string reporteVendedores = WebConfigurationManager.AppSettings["ReporteVendedores"];
                if (reporteVendedores == "1")
                {
                    SolicitarReporteCobranzaVendedores();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('ImpresionCobro.aspx?valor=7&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&pv=" + this.puntoVenta + "&e=" + this.idEmpresa + "&t=" + this.idTipo + "&ven=" + this.vendedor + "','_blank');", true);
                }
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCobro.aspx?valor=7&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&pv=" + this.puntoVenta + "&e=" + this.idEmpresa + "&t=" + this.idTipo + "&ven=" + this.vendedor + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void lbtnReporteDetalleCobros_Click(object sender, EventArgs e)
        {
            try
            {
                string listaCobros = string.Empty;
                foreach (Control C in phCobrosRealizados.Controls)
                {
                    TableRow tr = C as TableRow;
                    LinkButton lbtn = tr.Cells[5].Controls[0] as LinkButton;
                    listaCobros += lbtn.ID.Split('_')[1] + ",";
                }

                if (!String.IsNullOrEmpty(listaCobros))
                {
                    Response.Cookies["listaReporteDetalleCobros"].Value = listaCobros;
                    Response.Cookies["listaReporteDetalleCobros"].Expires = DateTime.Now.AddMinutes(10);
                    Response.Redirect("ImpresionCobro.aspx?valor=10&ex=1");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
                }

            }
            catch (Exception Ex)
            {

            }
        }

        protected void lbtnReporteDetalleCobrosPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string listaCobros = String.Empty;
                if (!String.IsNullOrWhiteSpace(txtIDsCobros.Text))
                {
                    listaCobros = txtIDsCobros.Text;
                }

                if (!String.IsNullOrEmpty(listaCobros))
                {
                    Response.Cookies["listaReporteDetalleCobros"].Value = listaCobros;
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('ImpresionCobro.aspx?valor=10','_blank');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
                }
            }
            catch (Exception Ex)
            {

            }
        }

    }
}