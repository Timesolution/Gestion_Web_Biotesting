using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ChequesF : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        ControladorBanco contBanco = new ControladorBanco();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private string fechaDCobro;
        private string fechaHCobro;
        private string fechaDImp;
        private string fechaHImp;
        private int idCliente;
        private int tipoFecha;
        private int origen;
        private int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.fechaDCobro = Request.QueryString["fdC"];
                this.fechaHCobro = Request.QueryString["fhC"];
                this.fechaDImp = Request.QueryString["fdI"];
                this.fechaHImp = Request.QueryString["fhI"];
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                this.origen = Convert.ToInt32(Request.QueryString["o"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 && idCliente == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarClientes();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaDCobro = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaHCobro = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaDImp = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaHImp = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");                        
                        txtDesdeC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtDesdeI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        estado = Convert.ToInt32(DropListEstado.SelectedValue);
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListClientes.SelectedValue = idCliente.ToString();                        
                    }

                    this.cargarSucursal();
                    this.cargarClientes();
                    this.cargarCuentas();
                    this.cargarBancos();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtDesdeC.Text = fechaDCobro;
                    txtHastaC.Text = fechaHCobro;
                    if (String.IsNullOrEmpty(fechaDCobro) && String.IsNullOrEmpty(fechaHCobro))
                    {
                        txtDesdeC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    txtDesdeI.Text = fechaDImp;
                    txtHastaI.Text = fechaHImp;
                    if (String.IsNullOrEmpty(fechaDImp) && String.IsNullOrEmpty(fechaHImp))
                    {
                        txtDesdeI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    this.txtFechaAgregar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFechaCAgregar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaImputar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DropListClientes.SelectedValue = idCliente.ToString();
                    DropListSucursal.SelectedValue = suc.ToString();
                    DropDownListOrigen.SelectedValue = origen.ToString();
                    this.DropListEstado.SelectedValue = this.estado.ToString();

                    this.verificarPermisoImputar();
                }
                
                this.cargarChequesRango(fechaD, fechaH, suc, idCliente);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "47")
                        {    //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                this.verficarPermisoCambiarSucursal();
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

        public void verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                string permiso = listPermisos.Where(x => x == "84").FirstOrDefault();

                if (permiso != null)
                {
                    this.DropListSucursal.Attributes.Remove("disabled");                    
                }
                else
                {
                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();                                
                }

            }
            catch
            {

            }
        }
        private void verificarPermisoImputar()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permisoImputar = listPermisos.Where(x => x == "87").FirstOrDefault();

                if (permisoImputar == null)
                {
                    this.btnImputar.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();
                DataTable dt2 = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                ////drop list modal imputar
                //this.DropListSucursalImputar.DataSource = dt2;
                //this.DropListSucursalImputar.DataValueField = "Id";
                //this.DropListSucursalImputar.DataTextField = "nombre";

                //this.DropListSucursalImputar.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarCuentas()
        {
            try
            {
                ControladorBanco contBanco = new ControladorBanco();
                var cuentas = contBanco.obtenerCuentasBancarias();
                //this.DropListCuentas.Items.Add("Seleccione...");
                foreach (var c in cuentas)
                {
                    this.DropListCuentas.Items.Add(c.Id + " - " + c.Banco1.entidad + " - " + c.Descripcion + " - " + c.Numero + " - " + c.Cuit + " - " + c.Librador);
                    this.DropListCuentas.Items[this.DropListCuentas.Items.Count - 1].Value = c.Id.ToString();
                }

                //DataTable dt = new DataTable();


                ////agrego todos
                //DataRow dr = dt.NewRow();
                //dr["nombre"] = "Seleccione...";
                //dr["id"] = -1;
                //dt.Rows.InsertAt(dr, 0);

                //DataRow dr2 = dt.NewRow();
                //dr2["nombre"] = "Todos";
                //dr2["id"] = 0;
                //dt.Rows.InsertAt(dr2, 1);


                //this.DropListSucursal.DataSource = dt;
                //this.DropListSucursal.DataValueField = "Id";
                //this.DropListSucursal.DataTextField = "nombre";

                //this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarBancos()
        {
            try
            {
                DataTable dt = this.controlador.obtenerBancosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListBancoAgregar.DataSource = dt;
                this.ListBancoAgregar.DataValueField = "id";
                this.ListBancoAgregar.DataTextField = "entidad";
                this.ListBancoAgregar.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando bancos a la lista. " + ex.Message));
            }
        }
        
        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();
                DataTable dt2 = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

                ////list modal imputar
                //this.DropListClienteImputar.DataSource = dt2;
                //this.DropListClienteImputar.DataValueField = "id";
                //this.DropListClienteImputar.DataTextField = "alias";

                //this.DropListClienteImputar.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        private void cargarChequesRango(string fechaD, string fechaH, int idSuc, int idCliente)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idCliente == 0)
                {
                    //DataTable dtCheques = controlador.obtenerDatosCheque(fechaD, fechaH, idSuc, idCliente);

                    List<ChequesValores> lstCheques = controlador.obtenerDatosCheque2(fechaD, fechaH, idSuc, idCliente, fechaDCobro,fechaHCobro,this.tipoFecha,this.origen,this.estado,this.fechaDImp,this.fechaHImp);                    

                    decimal saldo = 0;
                    foreach (ChequesValores ch in lstCheques)
                    {
                        this.cargarEnPh(ch);
                        saldo += ch.Cheque.importe;
                    }
                    lblSaldo.Text = saldo.ToString("C");
                    if (this.tipoFecha == 0)
                    {
                        this.cargarLabel(fechaD, fechaH, idSuc, idCliente);
                    }
                    else
                    {
                        this.cargarLabel(fechaDCobro, fechaHCobro, idSuc, idCliente);
                    }
                }
                else
                {
                    List<ChequesValores> lstCheques = controlador.obtenerDatosCheque2(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), idCliente, this.txtDesdeC.Text, this.txtHastaC.Text, this.tipoFecha, this.origen, this.estado, this.txtDesdeI.Text, this.txtHastaI.Text);

                    decimal saldo = 0;
                    foreach (ChequesValores ch in lstCheques)
                    {
                        this.cargarEnPh(ch);
                        saldo += ch.Cheque.importe;
                    }
                    lblSaldo.Text = saldo.ToString("C");
                    if (this.tipoFecha == 0)
                    {
                        this.cargarLabel(fechaD, fechaH, idSuc, idCliente);
                    }
                    else
                    {
                        this.cargarLabel(fechaDCobro, fechaHCobro, idSuc, idCliente);
                    }

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Cheques. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idCliente)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idCliente > -1)
                {
                    label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(ChequesValores ch)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                int i = (ch.Cheque.fecha.AddDays(30) - DateTime.Today).Days;
                if (i <= 10 && ch.Cheque.estado == 1)
                {
                    tr.ForeColor = System.Drawing.Color.Orange;
                }
                if (i <= 5 && ch.Cheque.estado == 1)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }  

                //Celdas
                TableCell celFechaC = new TableCell();
                celFechaC.Text = ch.FechaCobro.ToString("dd/MM/yyyy");
                if (ch.ReciboPago != "" && ch.ReciboPago != null)
                {
                    celFechaC.Text = ch.FechaPago.ToString("dd/MM/yyyy");
                }                
                celFechaC.VerticalAlign = VerticalAlign.Middle;
                celFechaC.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaC);

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(ch.Cheque.fecha);
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celFechaImp = new TableCell();
                Cheques_Cuentas datos = this.contBanco.obtenerDatosImputacionChequeById(ch.Cheque.id);
                if (datos != null)
                {
                    celFechaImp.Text = datos.fechaImputado.Value.ToString("dd/MM/yyyy");
                }
                else
                    celFechaImp.Text = "";
                celFechaImp.VerticalAlign = VerticalAlign.Middle;
                celFechaImp.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaImp);

                TableCell celReciboC = new TableCell();
                celReciboC.Text = ch.ReciboCobro.ToString();
                celReciboC.VerticalAlign = VerticalAlign.Middle;
                celReciboC.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celReciboC);

                TableCell celReciboP = new TableCell();
                celReciboP.Text = ch.ReciboPago.ToString();
                celReciboP.VerticalAlign = VerticalAlign.Middle;
                celReciboP.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celReciboP);

                TableCell celImporte = new TableCell();
                celImporte.Text = ch.Cheque.importe.ToString("C");
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celTipo = new TableCell();
                celTipo.Text = ch.Cheque.numero.ToString();
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celEntidad = new TableCell();
                celEntidad.Text = ch.Cheque.banco.entidad.ToString();
                celEntidad.VerticalAlign = VerticalAlign.Middle;
                celEntidad.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEntidad);

                TableCell celCuenta = new TableCell();
                celCuenta.Text = ch.Cheque.cuenta.ToString();
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCuenta);

                TableCell celCliente = new TableCell();                
                celCliente.Text = ch.Cliente;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celSucursal = new TableCell();

                if (!String.IsNullOrEmpty(ch.sucursalCobro.nombre))
                    celSucursal.Text = ch.sucursalCobro.nombre;
                if (!String.IsNullOrEmpty(ch.sucursalPago.nombre))
                    celSucursal.Text = ch.sucursalPago.nombre;

                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);

                TableCell celObservacion = new TableCell();
                celObservacion.Text = this.controlador.obtenerObservacionChequeManual(ch.Cheque.id);
                celObservacion.VerticalAlign = VerticalAlign.Middle;
                celObservacion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celObservacion);

                TableCell celEstado = new TableCell();
                if (ch.Cheque.estado == 1)
                {
                    celEstado.Text = "Disponible";
                }
                if (ch.Cheque.estado == 2)
                {
                    celEstado.Text = "Depositado";
                }
                if (ch.Cheque.estado == 3)
                {
                    celEstado.Text = "Entregado";
                }
                if (ch.Cheque.estado == 4)
                {
                    celEstado.Text = "Disponible";
                }
                if (ch.Cheque.estado == 5)
                {
                    celEstado.Text = "Imputado a Cta.";
                }
                if (ch.Cheque.estado == 6)
                {
                    celEstado.Text = "Debitado";
                }
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEstado);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();

                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + ch.Cheque.id + "_" + ch.idCobro.ToString() + "_" + ch.idPago.ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.detalleCobro);

                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + ch.Cheque.id.ToString();
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;

                celAccion.Controls.Add(cbSeleccion);
                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);

                phCheques.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Cheques en PH. " + ex.Message));
            }

        }

        private void detalleCobro(object sender, EventArgs e)
        {
            try
            {
                ////obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCobro = atributos[2];
                string idPago = atributos[3];
                string script = "";

                if (idCobro != "0")
                {
                    script = "window.open('../Cobros/ImpresionCobro.aspx?Cobro=" + idCobro + "&valor=2','_blank');";
                }
                if (idPago != "0")
                {
                    script += "window.open('../Pagos/ReportesR.aspx?id=" + idPago + "&valor=2','_blank');";
                }
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Cobro desde la interfaz. " + ex.Message);
            }
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        if (this.RadioFechaRecibo.Checked == true)
                        {
                            Response.Redirect("ChequesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue);
                        }
                        else
                        {
                            if (this.RadioFechaCobro.Checked == true)
                            {
                                Response.Redirect("ChequesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=1");
                            }
                            else
                            {
                                Response.Redirect("ChequesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&fdI=" + txtDesdeI.Text + "&fhI=" + txtHastaI.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=2");
                            }
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


                //if (this.RadioFechaRecibo.Checked == true)
                //{
                //    if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                //    {
                //        if (DropListSucursal.SelectedValue != "-1")
                //        {
                //            //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                //            Response.Redirect("ChequesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue);
                //        }
                //        else
                //        {
                //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                //        }
                //    }
                //    else
                //    {
                //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                //    }
                //}
                //else
                //{
                //    if (this.RadioFechaCobro.Checked == true)
                //    {
                //        if (!String.IsNullOrEmpty(txtDesdeC.Text) && (!String.IsNullOrEmpty(txtHastaC.Text)))
                //        {
                //            if (DropListSucursal.SelectedValue != "-1")
                //            {
                //                //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                //                Response.Redirect("ChequesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&tf=1" + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue);
                //            }
                //            else
                //            {
                //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                //            }
                //        }
                //        else
                //        {
                //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                //        }
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos de caja. " + ex.Message));

            }
        }

        //protected void btnExportarExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("ImpresionValores.aspx?valor=2");
        //    }
        //    catch
        //    {

        //    }
        //}

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try 
            {
                this.imprimirCheques(0);                
            }
            catch
            {

            }
        }
        protected void btnImprimir2_Click(object sender, EventArgs e)
        {
            try
            {
                this.imprimirCheques(1);
            }
            catch
            {

            }
        }
        private void imprimirCheques(int agrupado)
        {
            try
            {
                //List<ChequesValores> lstCheques = controlador.obtenerDatosCheque2(fechaD, fechaH, this.suc, idCliente, fechaDCobro, fechaHCobro, this.tipoFecha, this.origen, this.estado);
                                
                //DataTable dtDatos = new DataTable();
                //dtDatos.Columns.Add("id");
                //dtDatos.Columns.Add("fechaRE");
                //dtDatos.Columns.Add("fechaA");
                //dtDatos.Columns.Add("reciboC");
                //dtDatos.Columns.Add("reciboP");
                //dtDatos.Columns.Add("importe",typeof(double));
                //dtDatos.Columns.Add("numero");
                //dtDatos.Columns.Add("banco");
                //dtDatos.Columns.Add("cuenta");
                //dtDatos.Columns.Add("cliente");
                //dtDatos.Columns.Add("Sucursal");
                //dtDatos.Columns.Add("estado");
                //dtDatos.Columns.Add("cuit");

                //foreach (ChequesValores ch in lstCheques)
                //{
                //    DataRow drDatos = dtDatos.NewRow();
                //    drDatos["id"] = ch.Cheque.id;
                //    drDatos["fechaRE"] = ch.FechaCobro;
                //    if (ch.ReciboPago != "" && ch.ReciboPago != null)
                //    {
                //        drDatos["fechaRE"] = ch.FechaPago.ToString("dd/MM/yyyy");
                //    }
                //    drDatos["fechaA"] = ch.Cheque.fecha.ToString("dd/MM/yyyy");
                //    drDatos["reciboC"] = ch.ReciboCobro;
                //    drDatos["reciboP"] = ch.ReciboPago;
                //    drDatos["importe"] = ch.Cheque.importe;
                //    drDatos["numero"] = ch.Cheque.numero;
                //    drDatos["banco"] = ch.Cheque.banco.entidad;
                //    drDatos["cuenta"] = ch.Cheque.cuenta;
                //    drDatos["cliente"] = ch.Cliente;
                    
                //    if (!String.IsNullOrEmpty(ch.sucursalCobro.nombre))
                //        drDatos["Sucursal"] = ch.sucursalCobro.nombre;
                //    if (!String.IsNullOrEmpty(ch.sucursalPago.nombre))
                //        drDatos["Sucursal"] = ch.sucursalPago.nombre;

                //    drDatos["estado"] = ch.Cheque.estado;
                //    drDatos["cuit"] = ch.Cheque.cuit;

                //    if (ch.Cheque.estado == 1)
                //    {
                //        drDatos["estado"] = "Disponible";
                //    }
                //    if (ch.Cheque.estado == 2)
                //    {
                //        drDatos["estado"] = "Depositado";
                //    }
                //    if (ch.Cheque.estado == 3)
                //    {
                //        drDatos["estado"] = "Entregado";
                //    }
                //    if (ch.Cheque.estado == 4)
                //    {
                //        drDatos["estado"] = "Disponible";
                //    }
                //    if (ch.Cheque.estado == 5)
                //    {
                //        drDatos["estado"] = "Imputado a Cta.";
                //    }

                //    dtDatos.Rows.Add(drDatos);
                //}

                //Session.Add("datosCheques", dtDatos);
                //Session.Add("totalCheques", this.lblSaldo.Text);
                
                if (agrupado == 1)//reporte agrupado por chk
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Valores/ImpresionValores.aspx?a=1&g=1&valor=1', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    Response.Redirect("/Formularios/Valores/ImpresionValores.aspx?a=1&g=1&valor=1&FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&fdI=" + txtDesdeI.Text + "&fhI=" + txtHastaI.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=" + this.tipoFecha);
                }
                else
                {
                    Response.Redirect("/Formularios/Valores/ImpresionValores.aspx?a=1&g=0&valor=1&FD=" + txtFechaDesde.Text + "&FH=" + txtFechaHasta.Text + "&S=" + DropListSucursal.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&fdC=" + txtDesdeC.Text + "&fhC=" + txtHastaC.Text + "&fdI=" + txtDesdeI.Text + "&fhI=" + txtHastaI.Text + "&o=" + DropDownListOrigen.SelectedValue + "&e=" + DropListEstado.SelectedValue + "&tf=" + this.tipoFecha);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Valores/ImpresionValores.aspx?a=1&g=0&valor=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }

            }
            catch
            {

            }
        }
        protected void btnImputar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorBanco contBanco = new ControladorBanco();

                string idtildado = "";
                //recorro los cheques en pantalla
                foreach (Control C in phCheques.Controls)
                {
                    TableRow tr = C as TableRow;
                    String estadoCh = tr.Cells[12].Text;
                    String origen = tr.Cells[9].Text;
                    CheckBox ch = tr.Cells[13].Controls[2] as CheckBox;
                    //Si esta seleccionado, tiene estado disponible y es de terceros guardo el id.
                    if (ch.Checked == true && estadoCh == "Disponible")// && origen != "Propio" )
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (idtildado == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo se pueden imputar cheques en estado Disponible y de Terceros. "));
                }
                else
                {
                    int i = 0;
                    int idUser = (int)Session["Login_IdUser"];
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")//por cada id de cheque que tenga creo un cheque_cuentas, lo imputo a cta y lo pongo en estado (5)-imputado.
                        {
                            Cheques_Cuentas cc = new Cheques_Cuentas();
                            cc.idCheque = Convert.ToInt32(id);
                            cc.idCuenta = Convert.ToInt32(this.DropListCuentas.SelectedValue);
                            cc.fechaImputado = Convert.ToDateTime(txtFechaImputar.Text, new CultureInfo("es-AR"));
                            i = contBanco.imputarChequeCuenta(cc);
                            if (i > 0)
                            {
                                Log.EscribirSQL(idUser, "INFO", "Imputo cheque id: " + id + " a cuenta.");
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error imputando cheques a cta bancaria. " + ex.Message));
            }

        }        
        protected void lbtnAgregarCh_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorCobranzaEntity contCobranzaEnt = new ControladorCobranzaEntity();
                Gestion_Api.Modelo.Cheque ch = new Gestion_Api.Modelo.Cheque();
                ch.fecha = Convert.ToDateTime(this.txtFechaCAgregar.Text, new CultureInfo("es-AR"));
                ch.importe = Convert.ToDecimal(this.txtImporteAgregar.Text);
                ch.numero = Convert.ToDecimal(this.txtNumeroAgregar.Text);
                ch.librador = this.txtLibradorAgregar.Text;
                ch.cuit = this.txtCuitAgregar.Text;
                ch.cuenta = this.txtCuentaAgregar.Text;
                ch.banco.id = Convert.ToInt32(this.ListBancoAgregar.SelectedValue);

                string observacion = this.txtObservacionAgregar.Text;
                string fechaRecibo = this.txtFechaAgregar.Text;

                int suc = this.suc;
                if(suc == 0)
                    suc = (int)Session["Login_SucUser"];

                int idUser = (int)Session["Login_IdUser"];
                int ok = this.controlador.validarChequeManualExiste(ch.numero.ToString(), ch.banco.id, ch.cuenta);

                if (ok < 0)
                {
                    if (ok == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El numero de cheque ya existe!. "));
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error validando numero de cheque a ingresar."));
                        return;
                    }
                }

                Log.EscribirSQL(idUser, "INFO", "Inicio proceso agregar cheque manual.");
                int i = this.controlador.agregarChequeManual(ch, "M", suc, fechaRecibo,observacion);
                if (i > 0)
                {
                    //Busco el id del cobro que se generó para poder generar el registro en la tabla Cheques_Datos
                    int idCobro = contCobranzaEnt.obtenerIdCobroPorCheque(i);
                    if (idCobro > 0)
                    {
                        Cheques_Datos cd = new Cheques_Datos();
                        cd.Cheque = i;
                        cd.Cobro = idCobro;
                        cd.CobroFtp = 1;

                        int k = contCobranzaEnt.agregarCheques_Datos(cd);
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ","ChequesF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar cheque. "));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error procesando cheque. "+ex.Message));
            }
        }

        protected void RadioFechaImp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.RadioFechaImp.Checked == true)
                    this.DropListEstado.SelectedValue = "5";
                else
                    this.DropListEstado.SelectedValue = "0";
            }
            catch
            {

            }
        }

        protected void lbtnSiDebitar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorCobranzaEntity contCobEnt = new ControladorCobranzaEntity();

                string idtildado = "";
                //recorro los cheques en pantalla
                foreach (Control C in phCheques.Controls)
                {
                    TableRow tr = C as TableRow;
                    String estadoCh = tr.Cells[12].Text;
                    CheckBox ch = tr.Cells[13].Controls[2] as CheckBox;
                    var cheque = this.controlador.obtenerChequeId(Convert.ToInt32(ch.ID.Split('_')[1]));
                    //Si esta seleccionado, tiene estado entregado .
                    if (ch.Checked == true && estadoCh == "Entregado" && cheque.origen == "P")// origen = "Propio"
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (idtildado == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Solo se pueden debitar cheques en estado Entregados y Propios. "));
                }
                else
                {
                    int i = 0;
                    int idUser = (int)Session["Login_IdUser"];
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")//por cada id de cheque que tenga creo un cheques_debitos, y lo pongo en estado (6) debitado.
                        {
                            DateTime fechaDebitado = Convert.ToDateTime(txtFechaDebitar.Text, new CultureInfo("es-AR"));
                            i = contCobEnt.debitarChequeEntregado(Convert.ToInt32(id), fechaDebitado);
                            if (i > 0)
                            {
                                Log.EscribirSQL(idUser, "INFO", "Debito cheque id: " + id);
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error debitando cheques. " + ex.Message));
            }
        }
    }
}