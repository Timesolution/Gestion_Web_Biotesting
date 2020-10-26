using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CuentaCorrienteF : System.Web.UI.Page
    {
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        controladorDespacho contDesp = new controladorDespacho();
        Configuracion configuracion = new Configuracion();

        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();

        int idCliente;
        int idSucursal;
        int idTipo;
        int idVendedor;
        int accion;
        string fechaDesde;
        string fechaHasta;
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idTipo = Convert.ToInt32(Request.QueryString["Tipo"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.VerificarLogin();
                this.fechaDesde = Request.QueryString["fd"];
                this.fechaHasta = Request.QueryString["fh"];
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                //this.cargarMovimientos();
                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaDesde) && String.IsNullOrEmpty(this.fechaHasta))
                    {
                        this.fechaDesde = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                        this.fechaHasta = DateTime.Today.ToString("dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(configuracion.FechaFiltrosCuentaCorriente))
                            this.fechaDesde = configuracion.FechaFiltrosCuentaCorriente.Substring(11, 10).Replace(";", "/");
                    }

                    this.txtFechaDesde.Text = this.fechaDesde;
                    this.txtFechaHasta.Text = this.fechaHasta;

                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.verificarModoBlanco();
                    if (idCliente == 0 && idSucursal == 0 && idTipo == 0)
                    {
                        idSucursal = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarClientes(1);
                        DropListSucursal.SelectedValue = idSucursal.ToString();
                    }
                    this.cargarSucursal();
                    this.cargarClientes(1);
                    DropListSucursal.SelectedValue = idSucursal.ToString();
                    DropListTipo.SelectedValue = idTipo.ToString();
                    DropListClientes.SelectedValue = idCliente.ToString();
                    ListRazonSocial.SelectedValue = idCliente.ToString();
                    
                }
                if (idCliente > 0)
                {
                    this.cargarMovimientos(idCliente, idSucursal, idTipo);
                }
                if (accion > 0)
                {
                    this.btnImpagas.Visible = true;
                    this.btnImprimir.Visible = true;
                    this.btnAccion.Visible = true;
                }

                if (accion == 1)
                {                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "cambiarIcono", "cambiarIcono('fa fa-toggle-off','Ventas > Cuentas Corrientes > Impagas');", true);
                }
                if (accion == 2)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "cambiarIcono", "cambiarIcono('fa fa-toggle-on','Ventas > Cuentas Corrientes');", true);
                }
                this.Form.DefaultButton = lbtnBuscar.UniqueID;
            }
            catch
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
                        if (s == "40")
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                int i = this.verficarPermisoCambiarSucursal();
                                if (i <= 0)
                                {
                                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                            }

                            if (perfil == "Cliente")
                            {
                                this.btnBuscarCod.Visible = false;
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
                        if (s == "88")
                        {
                            this.DropListSucursal.Attributes.Remove("disabled");
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
        #region cargas iniciales
        public void cargarClientes(int tipoCarga)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil == "Vendedor")
                {
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);

                    this.DropListClientes.DataSource = dt;
                    this.DropListClientes.DataValueField = "id";
                    this.DropListClientes.DataTextField = "alias";
                    this.DropListClientes.DataBind();
                    this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                    this.ListRazonSocial.DataSource = dt;
                    this.ListRazonSocial.DataValueField = "id";
                    this.ListRazonSocial.DataTextField = "razonSocial";
                    this.ListRazonSocial.DataBind();
                    this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                }
                else
                {
                    if (perfil == "Cliente")
                    {
                        dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                        this.DropListClientes.DataSource = dt;
                        this.DropListClientes.DataValueField = "id";
                        this.DropListClientes.DataTextField = "alias";
                        this.DropListClientes.DataBind();
                        this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                        this.ListRazonSocial.DataSource = dt;
                        this.ListRazonSocial.DataValueField = "id";
                        this.ListRazonSocial.DataTextField = "razonSocial";
                        this.ListRazonSocial.DataBind();
                        this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    }
                    else
                    {
                        dt = contCliente.obtenerClientesDT();

                        this.DropListClientes.DataSource = dt;
                        this.DropListClientes.DataValueField = "id";
                        this.DropListClientes.DataTextField = "alias";
                        this.DropListClientes.DataBind();
                        this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                        this.ListRazonSocial.DataSource = dt;
                        this.ListRazonSocial.DataValueField = "id";
                        this.ListRazonSocial.DataTextField = "razonSocial";
                        this.ListRazonSocial.DataBind();
                        this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();
                DataTable dt2 = contSucu.obtenerSucursales();

                //agrego seleccion
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                //droplist modal compensacion
                //agrego seleccion
                DataRow dr3 = dt2.NewRow();
                dr3["nombre"] = "Seleccione...";
                dr3["id"] = -1;
                dt2.Rows.InsertAt(dr3, 0);

                this.ListSucursalDestino.DataSource = dt2;
                this.ListSucursalDestino.DataValueField = "Id";
                this.ListSucursalDestino.DataTextField = "nombre";

                this.ListSucursalDestino.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        
        public void cargarPuntoVentaDestino(int sucu)
        {
            controladorSucursal contSucu = new controladorSucursal();
            DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

            //agrego todos
            DataRow dr = dt.NewRow();
            dr["NombreFantasia"] = "Seleccione...";
            dr["id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            this.ListPuntoVentaDestino.DataSource = dt;
            this.ListPuntoVentaDestino.DataValueField = "Id";
            this.ListPuntoVentaDestino.DataTextField = "NombreFantasia";

            this.ListPuntoVentaDestino.DataBind();
        }

        #endregion
        private void cargarMovimientos(int idCliente, int idSucursal, int idTipo)
        {
            try
            {
                this.GridCtaCte.AutoGenerateColumns = false;

                DateTime fdesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                //DataTable datos = new DataTable();
                //datos.Columns.Add("Id");
                //datos.Columns.Add("Fecha");
                //datos.Columns.Add("Numero");
                //datos.Columns.Add("Debe",typeof(decimal));
                //datos.Columns.Add("Haber", typeof(decimal));
                //datos.Columns.Add("Saldo", typeof(decimal));
                //datos.Columns.Add("Acumulado", typeof(decimal));
                ////datos.Columns.Add("GuiaDespacho");
                //datos.Columns.Add("TipoDoc");

                //List<Movimiento> Movimiento = controlador.obtenerMovimientosByCuenta(idCliente,idSucursal,idTipo, this.accion);
                //Movimiento = Movimiento.OrderBy(x => x.fecha).ToList();
                DataTable datos = controlador.obtenerMovimientosByCuentaDT(idCliente, idSucursal, idTipo, this.accion,fdesde,fhasta);
                this.GridCtaCte.DataSource = datos;
                //var suma = datos.Compute("Sum(Saldo)", "");
                decimal saldoAcumulado = 0;

                foreach (DataRow row in datos.Rows)
                {
                    if (this.accion == 2)
                    {
                        if (Math.Abs(Convert.ToDecimal(row["debe"])) > 0)
                        {
                            saldoAcumulado += Convert.ToDecimal(row["debe"]);
                        }
                        if (Math.Abs(Convert.ToDecimal(row["haber"])) > 0)
                        {
                            saldoAcumulado -= Convert.ToDecimal(row["haber"]);
                        }
                    }
                    else
                    {
                        saldoAcumulado += Convert.ToDecimal(row["saldo"]);
                    }

                    row["SaldoAcumulado"] = saldoAcumulado.ToString();
                }

                #region old
                //if (this.accion == 2)
                //{
                //    foreach (Movimiento m in Movimiento)//cta cte
                //    {
                //        DataRow dr = datos.NewRow();
                //        MovimientoView movV = new MovimientoView();
                //        movV = m.ListarMovimiento();

                //        if (movV.tipo.tipo.Contains("Factura") || movV.tipo.tipo.Contains("Credito") || movV.tipo.tipo.Contains("Debito") || movV.tipo.tipo.Contains("Presupuesto"))
                //        {
                //            Gestion_Api.Entitys.Despacho despacho = this.contDesp.obtenerDespachoByIdFactura(movV.id_doc);
                //            if (despacho != null)
                //            {
                //                if (despacho.contrareembolso.Value == 1)
                //                    dr["GuiaDespacho"] = "*CONTRA-REEMBOLSO*";
                //            }
                //        }

                //        if (Math.Abs(m.debe) > 0)
                //        {
                //            //saldoAcumulado += m.saldo;
                //            saldoAcumulado += m.debe;

                //        }
                //        if (Math.Abs(m.haber) > 0)
                //        {
                //            saldoAcumulado -= m.haber;

                //        }
                //        dr["Id"] = movV.id;
                //        dr["Fecha"] = movV.fecha.ToString("dd/MM/yyyy");
                //        dr["Numero"] = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                //        dr["Debe"] = movV.debe;
                //        dr["Haber"] = movV.haber;
                //        dr["Saldo"] = movV.saldo;
                //        dr["Acumulado"] = saldoAcumulado.ToString();
                //        if (movV.tipo.tipo.Contains("Recibo"))
                //            dr["TipoDoc"] = movV.id + "_15";
                //        else
                //            dr["TipoDoc"] = movV.id_doc + "_" + movV.tipo.id;

                //        datos.Rows.Add(dr);

                //        //this.cargarEnPh(m, saldoAcumulado);
                //    }
                //}
                //else
                //{
                //    foreach (Movimiento m in Movimiento)//impaga
                //    {
                //        DataRow dr = datos.NewRow();
                //        MovimientoView movV = new MovimientoView();
                //        movV = m.ListarMovimiento();

                //        saldoAcumulado += m.saldo;

                //        dr["Id"] = movV.id;
                //        dr["Fecha"] = movV.fecha.ToString("dd/MM/yyyy");
                //        dr["Numero"] = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                //        dr["Debe"] = movV.debe;
                //        dr["Haber"] = movV.haber;
                //        dr["Saldo"] = movV.saldo;
                //        dr["Acumulado"] = saldoAcumulado.ToString();
                //        if (movV.tipo.tipo.Contains("Recibo"))
                //            dr["TipoDoc"] = movV.id_doc + "_15";
                //        else
                //            dr["TipoDoc"] = movV.id_doc + "_" + movV.tipo.id;

                //        datos.Rows.Add(dr);
                //        //this.cargarEnPh(m, saldoAcumulado);
                //    }
                //}
                #endregion
                
                this.GridCtaCte.DataBind();
                

                //this.lblSaldo.Text = "Saldo $ " + saldoAcumulado.ToString();
                this.labelSaldo.Text = saldoAcumulado.ToString("C");                
                this.cargarLabel(idCliente,idSucursal,idTipo);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarLabel(int idCliente, int idSucursal, int idTipo)
        {
            try
            {
                string label = "";
                if (idCliente > 0)
                {
                    if (DropListClientes.Items.FindByValue(idCliente.ToString()) != null)
                    {
                        label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                    }
                    else
                    {
                        Cliente cl = this.contrCliente.obtenerClienteID(idCliente);
                        label += cl.razonSocial + ",";
                    }
                    
                }
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idTipo > -1)
                {
                    label += DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(Movimiento m, decimal saldoAcumulado)
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
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + movV.debe;
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + movV.haber;
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + movV.saldo;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldo);

                TableCell celSaldoAcumulado = new TableCell();
                celSaldoAcumulado.Text = "$" + saldoAcumulado.ToString();
                celSaldoAcumulado.VerticalAlign = VerticalAlign.Middle;
                celSaldoAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldoAcumulado);

                //arego fila a tabla

                //TableCell celAccion = new TableCell();

                //Button btnEliminar = new Button();
                //btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.ID = "btnSelec_" + f.id;
                //btnEliminar.Text = "Detalle";
                ////btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //btnEliminar.Click += new EventHandler(this.detalleFactura);
                //celAccion.Controls.Add(btnEliminar);
                //celAccion.Width = Unit.Percentage(10);
                //celAccion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion);

                //phCuentaCorriente.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }

        }
        
        #region eventos
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CuentaCorrienteF.aspx?a=2&Cliente=" + DropListClientes.SelectedValue + "&Sucursal=" + DropListSucursal.SelectedValue + "&Tipo=" + DropListTipo.SelectedValue + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }
        }

        protected void ListRazonSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.DropListClientes.SelectedValue = this.ListRazonSocial.SelectedValue;                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en cliente. " + ex.Message));
            }
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListRazonSocial.SelectedValue = this.DropListClientes.SelectedValue;                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en razon social. " + ex.Message));
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

                this.ListRazonSocial.DataSource = dtClientes;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";

                this.ListRazonSocial.DataBind();

            }
            catch (Exception ex)
            {

            }
        }
        protected void ListSucursalDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Attributes.Remove("hidden");
                this.lbAguarde.Attributes.Add("hidden", "true");
                this.cargarPuntoVentaDestino(Convert.ToInt32(this.ListSucursalDestino.SelectedValue));
            }
            catch
            {

            }
        }
        
        protected void btnImpagas_Click(object sender, EventArgs e)
        {
            try
            {
                if (accion == 1)// 1 = impagas
                {
                    Response.Redirect("CuentaCorrienteF.aspx?a=2&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
                }
                else
                {
                    Response.Redirect("CuentaCorrienteF.aspx?a=1&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);                
                }
                
            }
            catch
            {
 
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = (DataTable)this.GridCtaCte.DataSource;

                Session.Add("datosMov", dtDatos);
                Session.Add("saldoMov", labelSaldo.Text);

                Response.Redirect("ImpresionReportes.aspx?Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&a=" + this.accion);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando reporte cta cte detalle desde la interfaz. " + ex.Message);
            }
            
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dtDatos = (DataTable)this.GridCtaCte.DataSource;

                Session.Add("datosMov", dtDatos);
                Session.Add("saldoMov", labelSaldo.Text);

                Response.Redirect("ImpresionReportes.aspx?Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&a=" + this.accion + "&e=1");                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cta cte desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error exportando detalles de cta cte a excel. " + ex.Message);
            }
        }

        protected void btnCompensar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                //this.lblMovimiento.Text = btn.CommandArgument;
                string movimiento = btn.CommandArgument;
                Session.Add("Mov_CompCta", movimiento);

                this.panel1.Attributes.Remove("hidden");
                this.lbAguarde.Attributes.Add("hidden", "true");
            }
            catch
            {

            }
        }
        protected void btnComentario_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                string movimiento = btn.CommandArgument;                
                string idDoc = movimiento.Split('_')[0];
                string tipoDoc = movimiento.Split('_')[1];

                this.txtComentario.Text = movimiento;

                if (tipoDoc != "15" && tipoDoc != "16" && tipoDoc != "18" && tipoDoc != "31" && tipoDoc != "32")
                {
                    Factura f = this.contFact.obtenerFacturaId(Convert.ToInt32(idDoc));
                    if(f != null)
                        this.txtComentario.Text = f.comentario;
                }

                this.panel2.Attributes.Remove("hidden");
                this.lbAguarde2.Attributes.Add("hidden", "true");
            }
            catch
            {

            }
        }
        protected void btnImprimir_Click1(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                string movimiento = btn.CommandArgument;
                if (!String.IsNullOrEmpty(movimiento))
                {
                    int idDoc = Convert.ToInt32(movimiento.Split('_')[0]);
                    int tipoDoc = Convert.ToInt32(movimiento.Split('_')[1]);
                    string script = "";
                    if (tipoDoc == 17 || tipoDoc == 11 || tipoDoc == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                    {
                        script ="window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idDoc + "','_blank');";
                    }
                    else
                    {
                        if (tipoDoc == 1 || tipoDoc == 9 || tipoDoc == 4 || tipoDoc == 24 || tipoDoc == 25 || tipoDoc == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                        {
                            //factura
                            script = "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idDoc + "','_blank');";
                        }
                        else
                        {
                            if (tipoDoc == 2 || tipoDoc == 5 || tipoDoc == 8)
                            {
                                //facturab
                                script = "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idDoc + "','_blank');";
                            }
                            else
                            {
                                if (tipoDoc == 15 || tipoDoc == 18)
                                {
                                    //recibo de cobro
                                    script = "window.open('../Cobros/ImpresionCobro.aspx?valor=2&Cobro=" + idDoc + "','_blank');";
                                }
                            }
                        }

                    }

                    if (script != "")
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", script, true);

                }

            }
            catch
            {

            }
        }
        protected void lbtnAgregarCompensacion_Click(object sender, EventArgs e)
        {
            try
            {
                //int idMov = Convert.ToInt32(this.labelMov.Text);
                string idMov = Session["Mov_CompCta"].ToString();
                Session["Mov_CompCta"] = null;
                int sucDestino = Convert.ToInt32(this.ListSucursalDestino.SelectedValue);
                int ptoVentaDestino = Convert.ToInt32(this.ListPuntoVentaDestino.SelectedValue);

                Movimiento mov = this.controlador.obtenerMovimientoID(Convert.ToInt32(idMov));

                if (Math.Abs(mov.saldo) > 0 && ((Math.Abs(mov.saldo) == Math.Abs(mov.debe)) || (Math.Abs(mov.saldo) == Math.Abs(mov.haber))))
                {
                    int i = this.controlador.GenerarCompensacionCuentas(sucDestino, ptoVentaDestino, Convert.ToInt32(idMov));

                    if (i > 0)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito", Request.Url.ToString()));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito \",{type: \"info\"});", true);
                        Response.Redirect(Request.Url.ToString());
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar mov de compensacion!."), true);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo generar mov de compensacion!.\");", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Solo puede compensar mov que no tengan cancelaciones!.\");", true);                    
                }

                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error al generar movimiento compensacion de cta. \",{type: \"error\"});", true);                    
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error al generar movimiento compensacion de cta. " + ex.Message));
            }
        }
        #endregion
        protected void GridInforme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridCtaCte.PageIndex = e.NewPageIndex;
            this.GridCtaCte.DataBind();
        }
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }


        protected void lbtnExportartxt_Click(object sender, EventArgs e)
        {
            try
            {
                controladorReportes controladorReportes = new controladorReportes();

                string rutaTxt = Server.MapPath("../ArchivosExportacion/Salida/");

                if (!Directory.Exists(rutaTxt))
                {
                    Directory.CreateDirectory(rutaTxt);
                }

                string archivos = controladorReportes.generarArchivoCuentaCorriente(rutaTxt);

                System.IO.FileStream fs = null;
                fs = System.IO.File.Open(archivos, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs.Length];
                fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/octet-stream";
                //this.Response.AddHeader("content-length", comprobante.Length.ToString());
                this.Response.AddHeader("Content-disposition", "attachment; filename= " + archivos);
                this.Response.BinaryWrite(btFile);
                this.Response.End();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Thread was being aborted"))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "alert", "MensajeArchivoDescargado()", true);
                }
                else
                {
                    int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "CATCH: No se pudo generar el archivo.txt con la cuenta corriente. Ubicacion: CuentaCorrienteF.aspx. Metodo: lbtnExportarCuentaCorriente_Click. Excepcion: " + ex.Message);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
                }
            }

        }
    }
}