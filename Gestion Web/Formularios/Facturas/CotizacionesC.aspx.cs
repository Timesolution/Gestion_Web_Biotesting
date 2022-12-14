using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Controladores.ControladoresEntity;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CotizacionesC : System.Web.UI.Page
    {
        #region Variables y Controladores

        ControladorPedido controlador = new ControladorPedido();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPedidoEntity contPedEntity = new ControladorPedidoEntity();
        ControladorPedido controladorPedido = new ControladorPedido();

        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        int accion;

        /// <summary>
        /// 1: btnImprimirCTDivisa
        /// 2: lbtnImprimirCT_En_Otra_Divisa
        /// </summary>
        public static int botonApretado = 0;
        
        private int idCliente;
        private int idEstado;
        private int vendedor;

        int idVendedor;

        string numeroCotizacion = string.Empty;
        string clienteCotizacion = string.Empty;
        string observacionCotizacion = string.Empty;
        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idEstado = Convert.ToInt32(Request.QueryString["estado"]);
                vendedor = Convert.ToInt32(Request.QueryString["V"]);
                accion = Convert.ToInt32(Request.QueryString["a"]);
                this.numeroCotizacion = Request.QueryString["n"];
                this.clienteCotizacion = Request.QueryString["c"];
                this.observacionCotizacion = Request.QueryString["o"];

                if (!IsPostBack)
                {
                    this.cargarSucursal();
                    this.cargarClientes();
                    this.cargarVendedor();
                    this.cargarEstados();


                    if (fechaD == null && fechaH == null && suc == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                    }
                    this.cargarSucursal();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                    ListVendedor.SelectedValue = vendedor.ToString();
                    DropListEstado.SelectedValue = idEstado.ToString();
                    DropListClientes.SelectedValue = idCliente.ToString();
                }

                if (accion == 0)
                {
                    this.cargarCotizacionesRango(fechaD, fechaH, suc);
                }
                if (accion == 1)///Busqueda por Observacion
                {
                    BuscarPorObservacion();
                }
                if (accion == 2)///Busqueda por Numero Cliente
                {
                    BuscarPorNumeroCliente();
                }
                if (accion == 3)///Busqueda por Numero Cotizacion
                {
                    BuscarPorNumeroCotizacion();
                }

                //if (idCliente <= 0)
                //    lbtnGenPedido.Visible = false;

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: Page_Load. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #region Verificacion Usuario

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
                        if (s == "36")
                        {
                            valor = 1;
                        }
                        //Permiso ver saldo
                        if (s == "119")
                            this.lblSaldo.Visible = true;
                    }
                }

                if (listPermisos.Contains("213"))
                    lbtnGenPedido.Visible = true;

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        #endregion

        #region Cargas de Datos

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


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarSucursal. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        public void cargarClientes()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;

                controladorCliente contCliente = new controladorCliente();
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

                DataTable dt = new DataTable();
                bool auxiliar = false;

                if (perfil == "Vendedor")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);
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
                if (perfil == "Cliente")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                    this.btnBuscarCod.Visible = false;
                    this.txtCodCliente.Attributes.Add("disabled", "true");
                    //Oculto el Saldo en caso de que el logueado sea un Cliente
                    //this.phSaldo.Visible = false;
                }
                if (perfil == "Distribuidor")
                {
                    auxiliar = true;
                    dt = contClienteEnt.obtenerReferidosDeCliente(this.idVendedor);
                    DataRow dr = dt.NewRow();
                    Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                    dr["alias"] = "Distribuidor";
                    if (c != null)
                    {
                        dr["alias"] = c.razonSocial;
                    }

                    dr["id"] = this.idVendedor;
                    dt.Rows.InsertAt(dr, 0);
                }
                if (auxiliar == false)
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
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarClientes. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));

            }
        }

        public void cargarVendedor()
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                DataRow dr3 = dt.NewRow();
                dr3["nombre"] = "Todos";
                dr3["id"] = 0;
                dt.Rows.InsertAt(dr3, 1);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedor.Items.Add(item);
                }

                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarVendedor. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        public void cargarEstados()
        {
            try
            {

                DataTable dt = controlador.obtenerEstadosPedidos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListEstado.DataSource = dt;
                this.DropListEstado.DataValueField = "id";
                this.DropListEstado.DataTextField = "descripcion";

                this.DropListEstado.DataBind();

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarEstados. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));

            }
        }

        private void cargarCotizacionesRango(string fechaD, string fechaH, int idSuc)
        {
            try
            {
                // if (fechaD != null && fechaD != null && idCliente != 0 && idSuc != 0 && idEstado != 0 && vendedor != 0)
                if (fechaD != null && fechaD != null)
                {
                    DataTable dt = this.controlador.obtenerCotizacionesRangoDT(fechaD, fechaH, idSuc, idCliente, idEstado, vendedor);
                    this.cargarCotizacion(dt);
                    //this.cargarLabel(fechaD, fechaH, idSuc, idCliente, idEstado);
                }
                else
                {
                    DataTable dt = this.controlador.obtenerCotizacionesRangoDT(this.txtFechaDesde.Text, this.txtFechaHasta.Text,
                        Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue),
                        Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue));
                    this.cargarCotizacion(dt);
                    //this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarCotizacionesRango. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        private void cargarCotizacion(DataTable dtCotizaciones)
        {
            try
            {
                decimal saldo = 0;
                foreach (DataRow row in dtCotizaciones.Rows)
                {
                    saldo += Convert.ToDecimal(row["total"]);
                    this.cargarEnPhDR(row);
                }

                lblSaldo.Text = saldo.ToString("C");

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarCotizacion. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        private void cargarEnPhDR(DataRow p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p["fecha"]).ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p["numero"].ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p["razonSocial"].ToString();
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);


                TableCell celTotal = new TableCell();
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    decimal total = Convert.ToDecimal(p["total"]);
                    celTotal.Text = "$" + Decimal.Round((total / (decimal)1.21), 2).ToString();
                }
                else
                {
                    celTotal.Text = Convert.ToDecimal(p["total"]).ToString("C");
                }
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                var estado = this.controlador.obtenerEstadoID(Convert.ToInt32(p["estado"]));
                celTipo.Text = estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnImprimir = new LinkButton();
                btnImprimir.CssClass = "btn btn-info ui-tooltip";
                btnImprimir.Attributes.Add("data-toggle", "tooltip");
                btnImprimir.Attributes.Add("title data-original-title", "Detalles");
                btnImprimir.ID = "btnSelec_" + p["id"].ToString();
                btnImprimir.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnImprimir.Font.Size = 12;
                btnImprimir.Click += new EventHandler(this.detallePedido);
                celAccion.Controls.Add(btnImprimir);
                celAccion.Width = Unit.Percentage(15);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + p["id"].ToString();//_id
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + p["id"].ToString();
                lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style=\"font-size:12pt;\">";
                lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                lDetail.Text += "</a>";
                //if (this.verificarPermisoEditar() > 0)
                //{
                celAccion.Controls.Add(lDetail);
                //}

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAccion.Controls.Add(l1);

                Literal lCRM = new Literal();
                lCRM.ID = "btnCRM_" + p["id"].ToString();
                lCRM.Text = "<a href=\"../Clientes/ClientesABM.aspx?accion=2&id=" + p["Cliente"].ToString() + "#Eventos\"  class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"CRM\" style=\"font-size:12pt;\">";
                lCRM.Text += "<i class=\"shortcut-icon icon-eye-open\"></i>";
                lCRM.Text += "</a>";
    
                celAccion.Controls.Add(lCRM);

                if (estado.id != 3)
                {
                    tr.Cells.Add(celAccion);
                }
                else
                {
                    TableCell celEstado = new TableCell();
                    celEstado.Controls.Add(btnImprimir);
                    tr.Cells.Add(celEstado);
                    tr.ForeColor = System.Drawing.Color.Red;
                }
                phCotizaciones.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarEnPhDR. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }

        }

        private void cargarEnPh(Pedido p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = p.fecha.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p.numero.ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p.cliente.razonSocial;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);


                TableCell celTotal = new TableCell();
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    celTotal.Text = "$" + Decimal.Round((p.total / (decimal)1.21), 2).ToString();
                }
                else
                {
                    celTotal.Text = "$" + p.total;
                }
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                celTipo.Text = p.estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Detalles");
                btnEliminar.ID = "btnSelec_" + p.id;
                btnEliminar.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnEliminar.Font.Size = 12;
                btnEliminar.Click += new EventHandler(this.detallePedido);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + p.id;//_id
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + p.id.ToString();
                lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p.id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" >";
                lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                lDetail.Text += "</a>";

                //if (this.verificarPermisoEditar() > 0)
                //{
                celAccion.Controls.Add(lDetail);
                //}
                tr.Cells.Add(celAccion);

                phCotizaciones.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: cargarEnPh. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }

        }

        #endregion

        #region Impresion Documento

        private void detallePedido(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idPedido = atributos[1];

                try
                {//Si es un pedido pendiente de vendedor, lo cambio a pendiente la primera vez que lo imprimo
                    Pedido ped = this.controlador.obtenerPedidoId(Convert.ToInt32(idPedido));
                    if (ped.estado.id == 5)
                    {
                        int i = this.contPedEntity.cambiarEstadoPedido(ped.id, 1);
                        if (i > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Cambio estado pendiente vendedor a pendiente pedido id: " + ped.id);
                        }
                    }
                }
                catch { }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "window.open('ImpresionPedido.aspx?co=1&a=1&Pedido=" + idPedido + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog(" + idPedido + ")", true);

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: detallePedido. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #region Modal Filtro

        /// <summary>
        /// Metodo que detecta el cambio en la lista de clientes para el filtro de cotizaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.obtenerVendedor(Convert.ToInt32(this.DropListClientes.SelectedValue));
            }
            catch
            {

            }
        }

        /// <summary>
        /// Metodo que obtiene vendedor correspondiente del cliente
        /// </summary>
        /// <param name="Cliente"></param>
        private void obtenerVendedor(int Cliente)
        {
            try
            {
                controladorCliente contCl = new controladorCliente();
                var cl = contCl.obtenerClienteID(Cliente);
                this.ListVendedor.SelectedValue = cl.vendedor.id.ToString();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Metodo que busca al cliente a traves del inputText 'txtCodCliente'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

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
                //controladorCliente contrCliente = new controladorCliente();
                //Cliente cl = contrCliente.obtenerClienteCodigo(txtCodCliente.Text);

                //if (cl != null)
                //{
                //    this.DropListClientes.SelectedValue = cl.id.ToString();
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encontro cliente con el codigo ingresado\", {type: \"info\"});", true);
                //}
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: btnBuscarCod_Click. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        /// <summary>
        /// Metodo que realiza la busqueda de cotizaciones con los parametros del filtro ya seteados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        Response.Redirect("CotizacionesC.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue
                            + "&estado=" + DropListEstado.SelectedValue + "&v=" + ListVendedor.SelectedValue);
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
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: btnBuscar_Click. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #region Busqueda

        /// <summary>
        /// Metodo que realiza un postback de la pagina enviando por parametro como queryString, a la accion a llamar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarNumeros_Click(object sender, EventArgs e)
        {
            botonApretado = 1;

            if (!string.IsNullOrEmpty(this.txtNumeroCotizacion.Text))
            {
                Response.Redirect("CotizacionesC.aspx?a=3&n=" + this.txtNumeroCotizacion.Text);
            }
            if (!string.IsNullOrEmpty(this.txtCodigoCliente.Text))
            {
                Response.Redirect("CotizacionesC.aspx?a=2&c=" + this.txtCodigoCliente.Text);
            }
            if (!string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                Response.Redirect("CotizacionesC.aspx?a=1&o=" + this.txtObservacion.Text);
            }
        }

        /// <summary>
        /// Metodo que realiza la busqueda por observacion en la cotizacion
        /// </summary>
        private void BuscarPorObservacion()
        {
            try
            {
                DataTable dtCotizaciones = this.controlador.ObtenerDataTablePedidosPorObservacion(this.observacionCotizacion, 10); //10 es el tipo de documento pedido
                if (dtCotizaciones.Rows.Count > 0)
                {
                    this.cargarCotizacion(dtCotizaciones);
                    if (botonApretado == 1)/// apreto el boton btnImprimirCTDivisa
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlSucces("Busqueda Completada", "Se han encontrado " + dtCotizaciones.Rows.Count.ToString() + " cotizaciones."));
                }
                else if (dtCotizaciones == null || dtCotizaciones.Rows.Count == 0 && botonApretado == 1)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlWarning("Busqueda Completada", "No se han encontrado cotizaciones con la observacion: " + this.observacionCotizacion.ToString()));

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: BuscarPorObservacion. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        /// <summary>
        /// Metodo que realiza la busqueda por numero de cotizacion
        /// </summary>
        private void BuscarPorNumeroCotizacion()
        {
            try
            {
                DataTable dtCotizaciones = this.controlador.ObtenerDataTablePedidosPorNumero(this.numeroCotizacion, 10); //10 es el tipo de documento Cotizacion
                if (dtCotizaciones.Rows.Count > 0)
                {
                    this.cargarCotizacion(dtCotizaciones);

                    if(botonApretado == 1)/// apreto el boton btnImprimirCTDivisa
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlSucces("Busqueda Completada", "Se han encontrado " + dtCotizaciones.Rows.Count.ToString() + " cotizaciones."));
                }
                else if (dtCotizaciones == null || dtCotizaciones.Rows.Count == 0 && botonApretado == 1)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlWarning("Busqueda Completada", "No se han encontrado cotizaciones con el N° Cotizacion: " + this.numeroCotizacion));

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: BuscarPorNumeroCotizacion. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        /// <summary>
        /// Metodo que realiza la busqueda por numero de cliente de la cotizacion
        /// </summary>
        private void BuscarPorNumeroCliente()
        {
            try
            {
                DataTable dtCotizaciones = this.controlador.ObtenerDataTablePedidosPorCliente(this.clienteCotizacion, 10); //10 es el tipo de documento Cotizacion
                if (dtCotizaciones.Rows.Count > 0)
                {
                    this.cargarCotizacion(dtCotizaciones);
                    if (botonApretado == 1)/// apreto el boton btnImprimirCTDivisa
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlSucces("Busqueda Completada", "Se han encontrado " + dtCotizaciones.Rows.Count.ToString() + " cotizaciones."));
                }
                else if (dtCotizaciones == null || dtCotizaciones.Rows.Count == 0 && botonApretado == 1)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeGrowlWarning("Busqueda Completada", "No se han encontrado cotizaciones con el N° Cliente : " + this.clienteCotizacion));

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: BuscarPorNumeroCliente. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #region Acciones

        #region Imprimir en Otra Divisa

        protected void lbtnImprimirCT_En_Otra_Divisa_Click(object sender, EventArgs e)
        {
            try
            {
                botonApretado = 2;

                controladorMoneda controladorMoneda = new controladorMoneda();

                string idsListaCotizacionesTildadas = "";
                int contadorCotizacionesTildadas = 0;
                foreach (Control C in phCotizaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        contadorCotizacionesTildadas++;
                        idsListaCotizacionesTildadas += ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idsListaCotizacionesTildadas) && contadorCotizacionesTildadas == 1)
                {
                    DropListDivisa.ClearSelection();

                    controladorCobranza controladorCobranza = new controladorCobranza();
                    controladorCotizaciones controladorCotizacion = new controladorCotizaciones();
                    ControladorFacturaMoneda controladorFacturaMoneda = new ControladorFacturaMoneda();

                    Pedido cotizacion = controlador.obtenerPedidoId(Convert.ToInt32(idsListaCotizacionesTildadas));
                    lblNumeroCT.Text = cotizacion.numero;
                    this.hfIDCotizacion.Value = cotizacion.id.ToString();
                    DataTable dt = controladorCobranza.obtenerMonedasDT();

                    this.DropListDivisa.DataSource = dt;
                    this.DropListDivisa.DataValueField = "id";
                    this.DropListDivisa.DataTextField = "moneda";
                    this.DropListDivisa.DataBind();

                    DropListDivisa.SelectedValue = DropListDivisa.Items.FindByText("Pesos").Value;
                    txtCotizacion.Text = "1.00";

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalImprimirCT_EnOtraDivisa", "openModalImprimirCT_EnOtraDivisa();", true);
                }
                else if (contadorCotizacionesTildadas > 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar <strong style='color:black'>solo</strong> un documento"));
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos <strong style='color:black'>un</strong> documento"));
                }
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: lbtnImprimirCT_En_Otra_Divisa_Click. Excepción: " + ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        protected void btnImprimirCTDivisa_Click(object sender, EventArgs e)
        {
            try
            {
                

                try
                {
                    ///Si es un pedido pendiente de vendedor, lo cambio a pendiente la primera vez que lo imprimo
                    Pedido ped = this.controlador.obtenerPedidoId(Convert.ToInt32(hfIDCotizacion.Value));
                    if (ped.estado.id == 5)
                    {
                        int i = this.contPedEntity.cambiarEstadoPedido(ped.id, 1);
                        if (i > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Cambio estado pendiente vendedor a pendiente pedido id: " + ped.id);
                        }
                    }
                }
                catch (Exception ex) { }

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?co=1&a=1&div=" + DropListDivisa.SelectedValue + "&Pedido=" + Convert.ToInt32(hfIDCotizacion.Value) + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: btnImprimirCTDivisa_Click. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }

        }

        protected void DropListDivisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                controladorCobranza controladorCobranza = new controladorCobranza();
                txtCotizacion.Text = Decimal.Round(controladorCobranza.obtenerCotizacion(Convert.ToInt32(DropListDivisa.SelectedValue)), 2).ToString().Replace(',', '.');
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: DropListDivisa_SelectedIndexChanged. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }
        #endregion

        #region Anular Cotizacion

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phCotizaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    if (!tr.Cells[4].Text.Contains("Anulada"))
                    {
                        CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                        if (ch.Checked == true)
                        {
                            //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                            idtildado += ch.ID.Split('_')[1] + ";";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.controlador.anularPedidos(idtildado);
                    if (i > 0)
                    {
                        Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "ANULACION Pedido id: " + idtildado);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cotizaciones anuladas con exito. ", "CotizacionesC.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Cotizaciones. "));

                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una cotizacion"));
                }

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: btnSi_Click. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #region Generar Pedido

        /// <summary>
        /// Metodo que genera un pedido a traves de la cotizacion seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnGenPedido_Click(object sender, EventArgs e)
        {
            int x = 0;
            try
            {
                try
                {
                    int idC = 0;
                    string idtildado = "";
                    foreach (Control C in phCotizaciones.Controls)
                    {
                        TableRow tr = C as TableRow;
                        if (!tr.Cells[4].Text.Contains("Anulado"))
                        {
                            CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[2] as CheckBox;

                            if (ch.Checked == true)
                            {
                                x++;
                                idtildado += ch.ID.Split('_')[1] + ";";// .Substring(12, ch.ID.Length - 12) + ";";
                            }
                        }


                    }
                    if (x != 0)
                    {
                        if (!String.IsNullOrEmpty(idtildado))
                        {
                            int aux = 0;
                            int contadorRepetido = 0;
                            decimal descuento = 0;
                            bool validarDescuento = false;

                            string errores = null;
                            string[] j = idtildado.Split(';');

                            for (int i = 0; i < x; i++)
                            {

                                var cotizacion = controladorPedido.obtenerPedidoId(Convert.ToInt32(j[i]));

                                if (cotizacion != null)
                                {
                                    if (i == 0)
                                    {
                                        idC = cotizacion.cliente.id; //FIJO
                                        descuento = cotizacion.neto10;
                                    }
                                    else
                                    {
                                        if (descuento != cotizacion.neto10)
                                        {
                                            validarDescuento = true;
                                        }
                                    }
                                    aux = cotizacion.cliente.id; //CAMBIA

                                    if (idC != aux && i != 0)
                                    {
                                        contadorRepetido++;
                                    }
                                }
                                else
                                {
                                    errores += Convert.ToString(j[i]) + " | ";
                                }

                            }
                            if (!validarDescuento)
                            {
                                if (errores == null)
                                {
                                    if (contadorRepetido == 0)
                                    {
                                        Response.Redirect("../../Formularios/Facturas/ABMPedidos.aspx?accion=4&Cot=" + idtildado + "&cliente=" + aux, false);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar cotizaciones que sean del mismo cliente."));
                                    }
                                }
                                else
                                {
                                    Log.EscribirSQL(1, "ERROR", "Ubicacion: CotizacionesC.aspx. Metodo: lbtnGenPedido_Click. Una de las cotizaciones arrojo null al buscarla en la BD. Codigo: var cotizacion = controladorPedido.obtenerPedidoId(Convert.ToInt32(j[i]));");
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Esta cotizaciones no se puedieron procesar,</br> ID Cotizacion: " + errores + ""));
                                }
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar cotizaciones con el mismo porcentaje de descuento"));
                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una cotizacion"));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo debe seleccionar una cotizacion"));
                    }
                }
                catch (Exception ex)
                {
                    int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: lbtnGenPedido_Click. Excepción: " + ex.Message);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
                }
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: lbtnGenPedido_Click. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }

        #endregion

        #endregion

        #region  Funcions Varias

        private void buscar(string cot)
        {
            try
            {
                List<Cotizaciones> facturas = new List<Cotizaciones>();
                string query = "select * from articulos where descripcion like '%" + cot + "%'";
                //facturas = this.controlador.buscarArticuloList(query);
                //this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: CotizacionesC. Metodo: buscar. Excepción: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));

            }
        }

        protected void UpdatePanel3_Load(object sender, EventArgs e)
        {
            this.labelIva.Text = "testc escribo desde el update panel amigo!!";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        #endregion

    }
}
