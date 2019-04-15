using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Cobros
{
    public partial class CobrosRealizadosF : System.Web.UI.Page
    {
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
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.vendedor = Convert.ToInt32(Request.QueryString["vend"]);

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
                if (this.idCliente > -1)
                {
                    this.cargarMovimientos();
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
                    List<Movimiento_Cobro> Movimiento = this.contrCC.obtenerCobrosRealizados(fechaD,fechaH,idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal,idTipo,this.vendedor);
                    phCobrosRealizados.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Movimiento_Cobro m in Movimiento)
                    {
                        saldo += m.cob.total;
                        this.cargarEnPh(m);
                    }
                    this.labelSaldo.Text = saldo.ToString("C");
                    //(this.lblSaldo.Text = "Saldo $ " + saldo.ToString("0.00");
                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, idCliente, Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));

                }
                else
                {
                    //cliente = this.contrCliente.obtenerClienteID(Convert.ToInt32(idCliente));
                    //cuenta = this.contrCC.obtenerCuentaCorrienteCliente(Convert.ToInt32(DropListClientes.SelectedValue));
                    List<Movimiento_Cobro> Movimiento = this.contrCC.obtenerCobrosRealizados(txtFechaDesde.Text, txtFechaHasta.Text, idCliente, puntoVenta, idEmpresa, idSucursal, idTipo,this.vendedor);
                    phCobrosRealizados.Controls.Clear();
                    decimal saldo = 0;
                    foreach (Movimiento_Cobro m in Movimiento)
                    {
                        saldo += m.cob.total;
                        this.cargarEnPh(m);
                    }
                    this.labelSaldo.Text = saldo.ToString("C");
                    //this.lblSaldo.Text = "Saldo $ " + saldo.ToString("0.00");
                    this.cargarLabel(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListClientes.SelectedValue), Convert.ToInt32(DropListPuntoVta.SelectedValue), idEmpresa, idSucursal, Convert.ToInt32(DropListTipo.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idCliente, int idPuntoVenta, int idEmpresa, int idSucursal, int idTipo)
        {
            try
            {
                string label = fechaD + ","  + fechaH + ",";
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

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

        private void cargarEnPh(Movimiento_Cobro m)
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

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celCliente = new TableCell();
                celCliente.Text = m.cob.cliente.razonSocial;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + movV.haber.ToString().Replace(',', '.');
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celComentarios = new TableCell();
                celComentarios.Text = m.cob.comentarios;
                celComentarios.VerticalAlign = VerticalAlign.Middle;
                celComentarios.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celComentarios);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + movV.id_doc;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.detalleCobro);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + movV.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + movV.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);

                phCobrosRealizados.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
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
                Response.Redirect("CobrosRealizadosF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&cliente=" + DropListClientes.SelectedValue + "&empresa=" + DropListEmpresa.SelectedValue + "&sucursal=" + DropListSucursal.SelectedValue + "&puntoVenta=" + DropListPuntoVta.SelectedValue + "&tipo=" + DropListTipo.SelectedValue + "&vend=" + DropListVendedores.SelectedValue);
                cargarMovimientos();
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
                if(i > 0)
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

        protected void lbtnReporteCobranzaPDF_Click(object sender, EventArgs e)
        {
            try
            {
                
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "window.open('ImpresionCobro.aspx?valor=7&fd=" + this.fechaD + "&fh=" + this.fechaH + "&cli=" + this.idCliente + "&suc=" + this.idSucursal + "&pv=" + this.puntoVenta + "&e=" + this.idEmpresa + "&t=" + this.idTipo + "&ven=" + this.vendedor + "','_blank');", true);
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
                string listaCobros = string.Empty;
                foreach (Control C in phCobrosRealizados.Controls)
                {
                    TableRow tr = C as TableRow;
                    LinkButton lbtn = tr.Cells[5].Controls[0] as LinkButton;
                    listaCobros += lbtn.ID.Split('_')[1] + ";";
                }
                if (!String.IsNullOrEmpty(listaCobros))
                {
                    Response.Cookies["listaReporteDetalleCobros"].Value = listaCobros;
                    Response.Cookies["listaReporteDetalleCobros"].Expires = DateTime.Now.AddMinutes(10);
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