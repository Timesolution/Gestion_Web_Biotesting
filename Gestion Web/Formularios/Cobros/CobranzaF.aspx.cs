using Disipar.Models;
using Estetica_Api.Entity;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CobranzaF : System.Web.UI.Page
    {
        controladorCobranza contCobranza = new controladorCobranza();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorCuentaCorriente contrCC = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity contFactEnt = new controladorFactEntity();
        Cliente cliente = new Cliente();
        //CuentaCorriente cuenta = new CuentaCorriente();
        Configuracion config = new Configuracion();

        Mensajes mje = new Mensajes();
        private int idCliente;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private int idTipo;
        private int accion;
        private String senia;
        private string txtNumeroCobro;
        //int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idCliente = Convert.ToInt32(Request.QueryString["cliente"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["empresa"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["sucursal"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);
                this.idTipo = Convert.ToInt32(Request.QueryString["tipo"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);

                if (!IsPostBack)
                {
                    this.verificarModoBlanco();
                    if (accion == 1)//viene de Pedidos->Seña
                    {
                        btnPagoCuenta.Visible = true;
                        txtImputar.Text = Request.QueryString["s"];
                        btnSiguiente.Visible = false;
                    }

                    if (idEmpresa == 0)
                    {
                        //this.idCliente = 1;
                        this.idEmpresa = (int)Session["Login_EmpUser"];
                        //this.puntoVenta = this.contCobranza.obtenerPrimerPuntoVenta(idSucursal, idEmpresa);
                        //this.puntoVenta = 1;
                    }
                    if (idSucursal == 0)
                    {
                        this.idSucursal = (int)Session["Login_SucUser"];
                    }
                    this.cargarClientes();
                    this.DropListClientes.SelectedValue = this.idCliente.ToString();
                    this.ListRazonSocial.SelectedValue = this.idCliente.ToString();
                    this.DropListClientes.SelectedValue = idCliente.ToString();
                    if (this.DropListClientes.SelectedValue == "-1")
                    {
                        this.cargarClienteEnLista(idCliente);
                    }
                    this.cargarEmpresas();
                    this.DropListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.DropListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.DropListPuntoVta.SelectedValue = this.puntoVenta.ToString();
                    this.DropListTipo.SelectedValue = this.idTipo.ToString();
                }
                if (this.idCliente > 0)
                {
                    this.cargarMovimientos();
                    this.txtImputar.Enabled = true;
                }
                else
                {
                    this.txtImputar.Enabled = false;
                }
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros") != 1)
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
                        if (s == "41")
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

        private void verificarAlerta()
        {
            try
            {
                Cliente c = this.contrCliente.obtenerClienteID(Convert.ToInt32(this.DropListClientes.SelectedValue));
                c.alerta = this.contrCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Alerta Cliente: " + c.alerta.descripcion + ". \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Alerta Cliente: " + c.alerta.descripcion + "."));
                }

            }
            catch (Exception ex)
            {

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

                DataRow dr2 = dt.NewRow();
                dr2["razonSocial"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                this.ListRazonSocial.DataSource = dt;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";

                this.ListRazonSocial.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cliente a la lista. " + ex.Message));
            }
        }

        private void cargarClienteEnLista(int idCliente)
        {
            var c = contrCliente.obtenerClienteID(idCliente);
            if (c != null)
            {
                this.DropListClientes.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
                this.DropListClientes.SelectedValue = idCliente.ToString();
                this.ListRazonSocial.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
                this.ListRazonSocial.SelectedValue = this.idCliente.ToString();
            }
        }

        private void cargarMovimientos()
        {
            try
            {
                if (idCliente == 0 && idEmpresa == 0 && idSucursal == 0 && puntoVenta == 0)
                {
                    this.idCliente = Convert.ToInt32(DropListClientes.SelectedValue);
                    this.idSucursal = (int)Session["Login_SucUser"];
                    this.idEmpresa = (int)Session["Login_EmpUser"];
                    this.puntoVenta = Convert.ToInt32(DropListPuntoVta.SelectedValue);
                    this.idTipo = Convert.ToInt32(DropListTipo.SelectedValue);

                    //cliente = this.contrCliente.obtenerClienteID(Convert.ToInt32(DropListClientes.SelectedValue));
                    //cuenta = this.contrCC.obtenerCuentaCorrienteCliente(Convert.ToInt32(DropListClientes.SelectedValue));
                    List<Movimiento> Movimiento = this.contrCC.obtenerMovimientosBySaldo(idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));
                    phCobranzas.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Movimiento m in Movimiento)
                    {
                        saldo += m.saldo;
                        this.cargarEnPh(m);
                    }
                    this.labelSaldo.Text = saldo.ToString("C");
                    //this.lblSaldo.Text = "Saldo $ " + saldo.ToString();
                    this.cargarLabel(idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));

                }
                else
                {
                    //cliente = this.contrCliente.obtenerClienteID(Convert.ToInt32(idCliente));
                    //cuenta = this.contrCC.obtenerCuentaCorrienteCliente(Convert.ToInt32(DropListClientes.SelectedValue));
                    List<Movimiento> Movimiento = this.contrCC.obtenerMovimientosBySaldo(Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));
                    phCobranzas.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Movimiento m in Movimiento)
                    {
                        saldo += m.saldo;
                        this.cargarEnPh(m);
                    }
                    this.labelSaldo.Text = saldo.ToString("C");
                    //this.lblSaldo.Text = "Saldo $ " + saldo.ToString();
                    this.cargarLabel(Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarLabel(int idCliente, int idPuntoVenta, int idEmpresa, int idSucursal, int idTipo)
        {
            try
            {
                string label = "";
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

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

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

                //agrego seleccione...
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                //agrego todos
                //DataRow dr1 = dt.NewRow();
                //dr1["NombreFantasia"] = "Todos";
                //dr1["Id"] = 0;
                //dt.Rows.InsertAt(dr1, 1);

                this.DropListPuntoVta.DataSource = dt;
                this.DropListPuntoVta.DataValueField = "Id";
                this.DropListPuntoVta.DataTextField = "NombreFantasia";

                this.DropListPuntoVta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.DropListPuntoVta.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
            }
        }

        private void cargarEnPh(Movimiento m)
        {
            MovimientoView movV = new MovimientoView();
            movV = m.ListarMovimiento();
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = movV.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = movV.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                string ddMMyyyyFV = "";
                if (movV.fechaVenc != "")
                {
                    string[] fechaV = movV.fechaVenc.Split(' ')[0].Split('/');
                    ddMMyyyyFV = fechaV[1] + "/" + fechaV[0] + "/" + fechaV[2];
                }

                TableCell celFechaV = new TableCell();
                celFechaV.Text = ddMMyyyyFV;
                celFechaV.VerticalAlign = VerticalAlign.Middle;
                celFechaV.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaV);

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                //celDebe.Text = "$" + movV.debe.ToString().Replace(',', '.');
                celDebe.Text = "$" + movV.debe.ToString("#,##0.00");
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                //celHaber.Text = "$" + movV.haber.ToString().Replace(',', '.');
                celHaber.Text = "$" + movV.haber.ToString("#,##0.00");
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                //celSaldo.Text = "$" + movV.saldo.ToString().Replace(',', '.');
                celSaldo.Text = "$" + movV.saldo.ToString("#,##0.00");
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                //celSaldo.Width = Unit.Percentage(20);
                tr.Cells.Add(celSaldo);

                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + movV.id;
                cbSeleccion.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Attributes.Add("onclick", "javascript:return updatebox(" + movV.saldo + "," + movV.id.ToString() + ");");
                cbSeleccion.Attributes.Add("onchange", "javascript:return updatebox(" + movV.saldo + "," + movV.id.ToString() + ");");

                celSeleccion.Controls.Add(cbSeleccion);
                celSeleccion.Width = Unit.Percentage(15);
                //celSeleccion.VerticalAlign = VerticalAlign.Middle;
                celSeleccion.HorizontalAlign = HorizontalAlign.Center;

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celSeleccion.Controls.Add(l2);

                LinkButton btnComentarios = new LinkButton();
                btnComentarios.CssClass = "btn btn-info ui-tooltip";
                btnComentarios.Attributes.Add("data-toggle", "tooltip");
                btnComentarios.Attributes.Add("title data-original-title", "Comentarios");
                btnComentarios.ID = "btnComentario_" + movV.id_doc + "_" + movV.tipo.id;
                btnComentarios.Text = "<span class='shortcut-icon icon-comment'></span>";
                btnComentarios.Font.Size = 12;
                btnComentarios.Click += new EventHandler(this.ComentariosDocumento);
                celSeleccion.Controls.Add(btnComentarios);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celSeleccion.Controls.Add(l3);

                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Imprimir");
                if (movV.tipo.tipo.Contains("Recibo"))
                    btnDetalles.ID = "btnImprimir_" + movV.id + "_15";
                else
                    btnDetalles.ID = "btnImprimir_" + movV.id_doc + "_" + movV.tipo.id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celSeleccion.Controls.Add(btnDetalles);

                tr.Cells.Add(celSeleccion);

                phCobranzas.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando movimiento a PH. " + ex.Message));
            }
        }

        private int verificarCierreCaja()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                int sucursal = Convert.ToInt32(this.DropListSucursal.SelectedValue);
                int ptoVenta = Convert.ToInt32(this.DropListPuntoVta.SelectedValue);

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CobranzaF.aspx?cliente=" + DropListClientes.SelectedValue + "&empresa=" + DropListEmpresa.SelectedValue + "&sucursal=" + DropListSucursal.SelectedValue + "&puntoVenta=" + DropListPuntoVta.SelectedValue + "&tipo=" + DropListTipo.SelectedValue);
                //cargarMovimientos();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }

        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {

                string idtildado = "";
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                //Chequeo si se ingreso dinero a imputar
                if (String.IsNullOrEmpty(this.txtImputar.Text) || this.txtImputar.Text == "0")
                {
                    foreach (Control C in phCobranzas.Controls)
                    {
                        TableRow tr = C as TableRow;
                        //CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                        CheckBox ch = tr.Cells[6].Controls[0] as CheckBox;
                        if (ch.Checked == true)
                        {
                            idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        }
                    }

                    //Verifico si se selecciono una factura que fue generado por liquidacion de cobros
                    string pagares = this.verificarCobroLiquidacionPagares(idtildado);

                    if (!String.IsNullOrEmpty(idtildado))
                    {
                        Response.Redirect("ABMCobros.aspx?documentos=" + idtildado + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=0" + "&valor=1&tipo=" + this.DropListTipo.SelectedValue + "&pagares=" + pagares);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe seleccionar al menos un movimiento"));
                    }
                }
                else
                {
                    //Consulto si hay documentos para Imputar
                    string movimientos = this.contrCC.obtenerMovimientosImputar(Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture), this.idCliente, this.idSucursal, this.idEmpresa, this.puntoVenta, this.idTipo);

                    if (!String.IsNullOrEmpty(movimientos))
                    {
                        //validamos que cada movimiento sea del tipo de doc que figura en la url (idTipo)

                        string[] mov_temp = movimientos.Split(';');
                        string mov_filtrados = "";
                        foreach (var item in mov_temp)
                        {
                            if (item != "")
                            {
                                DataTable dtMov = contrCC.obtenerMovimientoByID(Convert.ToInt32(item));
                                foreach (DataRow itemMov in dtMov.Rows)
                                {
                                    if (idTipo == 2)
                                    {
                                        if (itemMov["tipo_doc"].ToString() != "15")
                                        {
                                            mov_filtrados += item + ";";
                                        }
                                    }
                                    else if (idTipo == 1)
                                    {
                                        if (itemMov["tipo_doc"].ToString() != "18")
                                        {
                                            mov_filtrados += item + ";";
                                        }
                                    }
                                    else
                                    {
                                        mov_filtrados += item + ";";
                                    }
                                }
                            }
                        }
                        Response.Redirect("ABMCobros.aspx?documentos=" + mov_filtrados + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=" + txtImputar.Text + "&valor=1&tipo=" + this.DropListTipo.SelectedValue);
                        //Response.Redirect("ABMCobros.aspx?documentos=" + movimientos + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=" + txtImputar.Text + "&valor=1&tipo=" + this.DropListTipo.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El monto ingresado es menor al saldo de los documentos a imputar. "));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error enviando documentos impagos al formulario de Cobros. " + ex.Message));
            }
        }

        #region eventos listas desplegablees
        protected void ListRazonSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.DropListClientes.SelectedValue = this.ListRazonSocial.SelectedValue;
                this.verificarAlerta();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en cliente. " + ex.Message));
            }
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListRazonSocial.SelectedValue = this.DropListClientes.SelectedValue;
                this.verificarAlerta();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en razon social. " + ex.Message));
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

        #endregion
        private void GenerarPagoCuenta()
        {
            try
            {
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                PCuenta pago = new PCuenta();
                pago.tipo = this.contDocumentos.obtenerTipoDoc("Pago a Cuenta");
                pago.cliente.id = idCliente;
                pago.empresa.id = idEmpresa;
                pago.sucursal.id = idSucursal;
                pago.total = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.saldoaFavor = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.puntoVenta = this.contSucursal.obtenerPtoVentaId(puntoVenta);
                pago.fecha = DateTime.Now;

                int i = this.contCobranza.agregarPagoCuenta(pago);
                if (i > 0)
                {
                    Response.Redirect("ABMCobros.aspx?documentos=" + i + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=0&valor=1&tipo=" + this.DropListTipo.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error generando Pago a Cuenta. "));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));

            }
        }

        protected void btnPagoCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                if (!String.IsNullOrEmpty(this.txtImputar.Text) && this.txtImputar.Text != "0")
                {
                    Response.Redirect("ABMCobros.aspx?documentos=0;&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=" + this.txtImputar.Text + "&valor=2&tipo=" + this.DropListTipo.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar un valor"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));

            }
        }

        //busqueda por cliente
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

                this.ListRazonSocial.DataSource = dtClientes;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";

                this.ListRazonSocial.DataBind();

                this.verificarAlerta();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        protected void lbtnGenerarNC_Click(object sender, EventArgs e)
        {
            try
            {
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                string facturas = "";

                foreach (Control C in phCobranzas.Controls)
                {
                    TableRow tr = C as TableRow;
                    //string tipoDoc = tr.Cells[1].Text;
                    string tipoDoc = tr.Cells[2].Text;
                    //CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                    CheckBox ch = tr.Cells[6].Controls[0] as CheckBox;
                    if (ch.Checked == true && (tipoDoc.Contains("Factura") || tipoDoc.Contains("Presupuesto")))
                    {
                        facturas += tipoDoc + ";";
                    }
                }
                if (String.IsNullOrEmpty(facturas))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("Debe seleccionar al menos un documento!."), true);
                    return;
                }

                decimal total = Convert.ToDecimal(this.txtFinal.Text);
                if (total > 0)
                {
                    int i = this.contCobranza.GenerarNotaCreditoDescuento(null, this.idEmpresa, this.idSucursal, this.puntoVenta, this.idTipo, this.idCliente, total, facturas);

                    if (i > 0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxInfo("Proceso finalizado con exito!.", ""), true);
                        Response.Redirect("CobranzaF.aspx?cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&tipo=" + idTipo);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo generar la nota de credito."), true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("El monto debe ser mayor a cero!."), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxError("Error generando nota de credito. " + ex.Message), true);
            }
        }

        protected void btnImputar_Click(object sender, EventArgs e)
        {
            try
            {
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                string idtildadoNC = "";
                string idtildadoFC = "";
                string idDocTildados = "";
                foreach (Control C in phCobranzas.Controls)
                {
                    TableRow tr = C as TableRow;
                    //string tipoDoc = tr.Cells[1].Text;
                    string tipoDoc = tr.Cells[2].Text;
                    //CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                    CheckBox ch = tr.Cells[6].Controls[0] as CheckBox;
                    if (ch.Checked == true && tipoDoc.Contains("Credito") || tipoDoc.Contains("Recibo"))
                    {
                        idtildadoNC += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                    if (ch.Checked == true && (tipoDoc.Contains("Factura") || tipoDoc.Contains("Presupuesto") || tipoDoc.Contains("Debito")))
                    {
                        idtildadoFC += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                    if (ch.Checked)
                    {
                        idDocTildados += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildadoNC) && !String.IsNullOrEmpty(idtildadoFC))
                {
                    List<MovimientoView> movimientosNC = contrCC.obtenerListaMovimientos(idtildadoNC);
                    List<MovimientoView> movimientosFC = contrCC.obtenerListaMovimientos(idtildadoFC);
                    List<MovimientoView> movDocTildados = contrCC.obtenerListaMovimientos(idDocTildados);
                    hacerCobro();

                    this.contCobranza.imputarReciboCobroAFactura(movimientosNC, movimientosFC);

                    sadoDeTodosLosDocEn0(movDocTildados);

                    actualizarSaldoImputado(movDocTildados);
                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe seleccionar al menos un movimiento nota de credito y una factura, nota de debito o presupuesto."));
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void hacerCobro()
        {
            try
            {
                //obtengo las imputaciones
                //string imputado = TotalDoc;
                string imputado = "0";
                List<Imputacion> imputaciones = obtenerImputaciones();
                //Configuracion config = new Configuracion();
                if (imputaciones != null)
                {
                    //si hay imputaciones ingreso los pagos
                    DataTable dtRecCobro = getRecibosDeCobro();
                    DataTable dtNotaCredito = getNotaDeCredito();
                    List<Pago> listPago = contCobranza.obtenerPagosRC_NC_desdeDT(dtRecCobro, dtNotaCredito);

                    if (listPago.Count > 0 & listPago != null)
                    {
                        Cobro cobro = new Cobro();
                        //cobro.fecha = Convert.ToDateTime(DateTime.Now.ToString(new CultureInfo("es-AR")).Split(' ')[0]);
                        cobro.fecha = DateTime.Now;
                        cobro.cliente.id = this.idCliente;
                        cobro.Doc_imputar = imputaciones;
                        cobro.pagos = listPago;
                        cobro.empresa.id = this.idEmpresa;
                        cobro.sucursal.id = this.idSucursal;
                        cobro.puntoVenta = contSucursal.obtenerPtoVentaId(this.puntoVenta);
                        //cobro.total = Convert.ToDecimal(txtTotalIngresado.Replace(',', '.'), CultureInfo.InvariantCulture);
                        decimal txtTotalIngresado = 0;
                        foreach (DataRow dataRow in dtRecCobro.Rows)
                        {
                            txtTotalIngresado += Convert.ToDecimal(dataRow["monto"]);
                        }
                        foreach (DataRow dataRow2 in dtNotaCredito.Rows)
                        {
                            txtTotalIngresado += Convert.ToDecimal(dataRow2["monto"]);
                        }
                        cobro.ingresado = Math.Abs(txtTotalIngresado);
                        foreach (var items in imputaciones)
                        {
                            txtTotalIngresado += Convert.ToDecimal(items.total);
                        }
                        cobro.total = txtTotalIngresado;
                        //cobro.imputado = Convert.ToDecimal(imputado.Replace(',', '.'), CultureInfo.InvariantCulture);
                        cobro.imputado = Convert.ToDecimal(imputaciones[0].imputar);
                        //cobro.ingresado = Convert.ToDecimal(txtTotalIngresado.Replace(',', '.'), CultureInfo.InvariantCulture);
                        //cobro.ingresado = txtTotalIngresado;
                        cobro.comentarios = "";
                        obtenerNroRecibo();
                        //if (this.config.numeracionCobros == "0")
                        //{
                        cobro.numero = txtNumeroCobro;
                        //}

                        //agrego el tipo de documento que se imputa                        
                        if (this.idTipo == 1)
                        {
                            cobro.tipoDocumento.tipo = "Factura";
                        }
                        if (this.idTipo == 2)
                        {
                            cobro.tipoDocumento.tipo = "Presupuesto";
                        }

                        int i = 0;
                        //if (Math.Abs(cobro.ingresado) >= cobro.imputado)
                        //{
                            //var idRecCobro = cobro.pagos[0].idReciboCobro;
                        i = contCobranza.ProcesarCobro(cobro, -1, this.idTipo);

                            //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"El cobro es mayor a lo imputado. \");", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El cobro es mayor a lo imputado"));
                        //}
                        if (i > 0)
                        {
                            int idMov = contCobranza.transformarIdCobroEnIdMov(i);

                            if (idMov > 0)
                            {
                                contCobranza.saveMovimiento_Cuenta_FechaVencimientos(idMov);

                                contCobranza.modificarMovimientoCuenta_Haber(idMov);

                                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "alert", "window.open('ImpresionCobro.aspx?Cobro=" + idMov + "&valor=2', 'fullscreen', 'top=0,left=0,width=' + (screen.availWidth) + ',height =' + (screen.availHeight) + ',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0'); location.href = 'CobranzaF.aspx';", true);
                            }
                        }

                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se cargaron pagos. \");", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se cargaron pagos. "));
                    }
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanelAgregar, UpdatePanelAgregar.GetType(), "alert", " $.msgbox(\"No se pudo obtener imputaciones. \");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargaron imputaciones "));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void sadoDeTodosLosDocEn0(List<MovimientoView> movs)
        {
            try
            {
                foreach (var mov in movs) 
                {
                    contCobranza.actualizarSaldoNC(mov.id,0);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void actualizarSaldoImputado(List<MovimientoView> movs)
        {
            try
            {
                decimal saldoSum = movs.Sum(x => x.saldo);
                
                if (saldoSum < 0)
                {
                    int idMovMenorA0 = movs.Where(x => x.saldo < 0).FirstOrDefault().id;
                    contCobranza.actualizarSaldoNC(idMovMenorA0, saldoSum);
                }
                else if (saldoSum > 0)
                {
                    int idMovMayorA0 = movs.Where(x => x.saldo > 0).FirstOrDefault().id;
                    contCobranza.actualizarSaldoNC(idMovMayorA0, saldoSum);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<Imputacion> obtenerImputaciones()
        {
            try
            {
                List<Imputacion> imputaciones = new List<Imputacion>();
                foreach (Control c in phCobranzas.Controls)
                {
                    TableRow tr1 = c as TableRow;
                    string tipoDoc = tr1.Cells[2].Text;
                    CheckBox ch = tr1.Cells[6].Controls[0] as CheckBox;

                    if (ch.Checked == true && (tipoDoc.Contains("Factura") || tipoDoc.Contains("Presupuesto") || tipoDoc.Contains("Debito")))
                    {
                        Imputacion imp = new Imputacion();
                        string txt = tr1.Cells[5].Text.TrimStart('$');

                        imp.movimiento = contrCC.obtenerMovimientoID(Convert.ToInt32(ch.ID.Substring(12, ch.ID.Length - 12)));

                        imp.total = Convert.ToDecimal(tr1.Cells[5].Text.Substring(1), CultureInfo.InvariantCulture);

                        if (!String.IsNullOrEmpty(txt))
                        {
                            decimal resto = imp.movimiento.saldo - imp.total;
                            imp.imputar = resto + Convert.ToDecimal(txt, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            imp.imputar = 0;
                        }

                        imputaciones.Add(imp);
                    }
                }
                return imputaciones;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ha ocurrido un error obteniendo Lista de Imputaciones. " + ex.Message));
                return null;
            }
        }

        public DataTable getRecibosDeCobro()
        {
            try
            {
                DataTable dtPagos = new DataTable();
                dtPagos.Columns.Add("id");
                dtPagos.Columns.Add("monto");
                dtPagos.Columns.Add("idRecCobro");

                foreach (Control c in phCobranzas.Controls)
                {


                    TableRow tr1 = c as TableRow;
                    string tipoDoc = tr1.Cells[2].Text;
                    CheckBox ch = tr1.Cells[6].Controls[0] as CheckBox;

                    if (ch.Checked == true && tipoDoc.Contains("Recibo"))
                    {
                        string txt = tr1.Cells[5].Text.TrimStart('$');

                        DataRow dr = dtPagos.NewRow();
                        dr["monto"] = txt;
                        dr["idRecCobro"] = ch.ID.Substring(12, ch.ID.Length - 12);
                        dtPagos.Rows.Add(dr);
                    }


                }
                return dtPagos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getNotaDeCredito()
        {
            try
            {
                DataTable dtPagos = new DataTable();
                dtPagos.Columns.Add("id");
                dtPagos.Columns.Add("monto");
                dtPagos.Columns.Add("idNotaCredito");

                foreach (Control c in phCobranzas.Controls)
                {
                    TableRow tr1 = c as TableRow;
                    string tipoDoc = tr1.Cells[2].Text;
                    CheckBox ch = tr1.Cells[6].Controls[0] as CheckBox;

                    if (ch.Checked == true && tipoDoc.Contains("Credito"))
                    {
                        string txt = tr1.Cells[5].Text.TrimStart('$');

                        DataRow dr = dtPagos.NewRow();
                        dr["monto"] = txt;
                        dr["idNotaCredito"] = ch.ID.Substring(12, ch.ID.Length - 12);
                        dtPagos.Rows.Add(dr);
                    }

                }
                return dtPagos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void obtenerNroRecibo()
        {
            try
            {
                //como estoy en cotizacion pido el ultimo numero de este documento
                PuntoVenta pv = contSucursal.obtenerPtoVentaId(this.puntoVenta);
                int nro = this.contCobranza.obtenerReciboNumero(this.puntoVenta, "Recibo de Cobro - FC");
                if (this.config.numeracionCobros != "0")
                {
                    txtNumeroCobro = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error obteniendo numero de Cobro. " + ex.Message));
            }
        }

        private void ComentariosDocumento(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');

                int idDoc = Convert.ToInt32(atributos[1]);
                int tipoDoc = Convert.ToInt32(atributos[2]);

                string comentario = "";

                if (tipoDoc != 15 && tipoDoc != 16 && tipoDoc != 18 && tipoDoc != 31 && tipoDoc != 32)
                {
                    Factura f = this.contFact.obtenerFacturaId(Convert.ToInt32(idDoc));
                    if (f != null)
                        comentario = f.comentario;
                }

                this.txtComentario.Text = Regex.Replace(comentario, @"\t|\n|\r", "");

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "openModalComentario()", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        private void detalleFactura(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');

                string idDoc = atributos[1];
                int tipoDoc = Convert.ToInt32(atributos[2]);

                string script = "";
                if (tipoDoc == 17 || tipoDoc == 11 || tipoDoc == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    script = "window.open('../Facturas/ImpresionPresupuesto.aspx?Presupuesto=" + idDoc + "','_blank');";
                }
                else
                {
                    if (tipoDoc == 1 || tipoDoc == 9 || tipoDoc == 4 || tipoDoc == 24 || tipoDoc == 25 || tipoDoc == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        script = "window.open('../Facturas/ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idDoc + "','_blank');";
                    }
                    else
                    {
                        if (tipoDoc == 2 || tipoDoc == 5 || tipoDoc == 8)
                        {
                            //facturab
                            script = "window.open('../Facturas/ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idDoc + "','_blank');";
                        }
                        else
                        {
                            if (tipoDoc == 15 || tipoDoc == 18)
                            {
                                //recibo de cobro
                                script = "window.open('ImpresionCobro.aspx?valor=1&Cobro=" + idDoc + "','_blank');";
                            }
                        }
                    }

                }
                if (script != "")
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", script, true);

            }
            catch (Exception ex)
            {
            }
        }
        private string verificarCobroLiquidacionPagares(string documentos)
        {
            try
            {
                string listPagares = string.Empty;
                string listPagaresAux = string.Empty;

                //Obtengo el movimiento de cuenta corriente
                List<MovimientoView> movFc = this.contrCC.obtenerListaMovimientos(documentos);

                if (movFc != null)
                {
                    //Busco los datos de los pagarés en la tabla Pagares_Liquidaciones
                    var pagLiq = this.contFactEnt.obtenerPagares_LiquidacionesByFactura(movFc.FirstOrDefault().id_doc);

                    if (pagLiq != null && pagLiq.Count > 0)
                    {
                        //Si existe, seteo la variable pagarés con los id de los pagarés
                        foreach (var item in pagLiq)
                        {
                            listPagaresAux += item.Pagare.ToString() + ";";
                        }

                        listPagares = listPagaresAux.Substring(0, listPagaresAux.Length - 1);
                    }
                }

                return listPagares;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void lbtnGenerarNCND_Click(object sender, EventArgs e)
        {
            try
            {
                //verifico cierre caja
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                string facturas = "";

                foreach (Control C in phCobranzas.Controls)
                {
                    TableRow tr = C as TableRow;
                    //string tipoDoc = tr.Cells[1].Text;
                    string tipoDoc = tr.Cells[2].Text;
                    //CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                    CheckBox ch = tr.Cells[6].Controls[0] as CheckBox;
                    if (ch.Checked == true && (tipoDoc.Contains("Factura") || tipoDoc.Contains("Presupuesto")))
                    {
                        facturas += tipoDoc + ";";
                    }
                }
                if (String.IsNullOrEmpty(facturas))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("Debe seleccionar al menos un documento!."), true);
                    return;
                }

                decimal importe = Convert.ToDecimal(this.txtImporte.Text);
                if (importe > 0)
                {

                    string tipoNCND = drpNCND.SelectedValue;
                    string descripcionArt = txtDescripcionArt.Text;

                    int i = this.contCobranza.GenerarNotaCreditoDebito(null, this.idEmpresa, this.idSucursal, this.puntoVenta, this.idTipo, this.idCliente, importe, facturas, tipoNCND, descripcionArt);

                    if (i > 0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxInfo("Proceso finalizado con exito!.", ""), true);
                        Response.Redirect("CobranzaF.aspx?cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&tipo=" + idTipo);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo generar la nota de credito."), true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxAtencion("El monto debe ser mayor a cero!."), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", mje.mensajeBoxError("Error generando nota de credito. " + ex.Message), true);
            }
        }
    }



}