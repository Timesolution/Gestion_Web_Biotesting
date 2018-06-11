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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios
{
    public partial class PagosF : System.Web.UI.Page
    {
        ControladorCCProveedor controlador = new ControladorCCProveedor();
        controladorCliente contCliente = new controladorCliente();
        controladorCompraEntity contCompra = new controladorCompraEntity();

        Mensajes m = new Mensajes();
        private int idProveedor;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;
        private int idTipo;
        private int bn;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.idProveedor = Convert.ToInt32(Request.QueryString["p"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.bn = Convert.ToInt32(Request.QueryString["bn"]);
                //this.idTipo = Convert.ToInt32(Request.QueryString["tipo"]);

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.verificarModoBlanco();
                    this.cargarEmpresas();
                    this.cargarProveedores();
                    this.cargarSucursal(Convert.ToInt32(this.ListProveedor.SelectedValue));
                }
                if (this.idProveedor > 0)
                {
                    this.cargarMovimientos();
                    this.txtImputar.Enabled = true;
                }
                this.Form.DefaultButton = btnBuscarProveedor.UniqueID;
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina. " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Articulos.Articulos") != 1)
                    if (this.verificarAcceso() != 1)
                    {
                        Response.Redirect("../../Default.aspx?m=1", false);
                    }
                    else
                    {

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
                        if (s == "33")
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
                    //this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("Ambos"));
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("PRP"));
                    this.idTipo = 0;
                }
            }
            catch
            {

            }
        }
        private void cargarEmpresas()
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Empresas. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu)
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

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }

        private void verificarAlerta()
        {
            try
            {
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel4, UpdatePanel4.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void cargarMovimientos()
        {
            try
            {
                var movimientos = controlador.obtenerMovimientosPagosCompras(this.idProveedor, this.idEmpresa, this.idSucursal, this.puntoVenta);
                var movimientosFavor = this.controlador.obtenerMovimientosPagosFavor(this.idProveedor, this.idEmpresa, this.idSucursal, this.puntoVenta, this.bn);

                //Si son compras y pagos en blanco
                if (this.bn == 1)
                    movimientos = movimientos.Where(x => x.Compra.Ftp == 0).ToList();
                    
                //Si son compras en negro
                if (this.bn == 2)
                    movimientos = movimientos.Where(x => x.Compra.Ftp == 1).ToList();

                movimientos.AddRange(movimientosFavor);

                phCobranzas.Controls.Clear();
                decimal saldo = 0;
                movimientos = movimientos.OrderBy(x => x.Fecha).ToList();
                foreach (MovimientosCCP m in movimientos)
                {
                    saldo +=(decimal)m.Saldo;
                    this.cargarEnPh(m);
                }
                this.labelSaldo.Text = "$ " +  saldo.ToString("N");
                this.cargarLabel();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarLabel()
        {
            try
            {
                string label = "";
                if (this.idProveedor > 0)
                {
                    label += ListProveedor.Items.FindByValue(idProveedor.ToString()).Text + ",";
                }
                if (idEmpresa > 0)
                {
                    label += DropListEmpresa.Items.FindByValue(idEmpresa.ToString()).Text + ",";
                }
                //if (idSucursal > 0)
                //{
                //    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                //}
                
                //if (idPuntoVenta > 0)
                //{
                //    label += DropListPuntoVta.Items.FindByValue(idPuntoVenta.ToString()).Text + ",";
                //}
                //if (idTipo > -1)
                //{
                //    label += DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                //}

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(MovimientosCCP m)
        {
            try
            {
                string tipoPago = string.Empty;

                if (this.bn == 1)
                    tipoPago = "Pago FC Nº ";
                if (this.bn == 2)
                    tipoPago = "Pago PRP Nº ";
                if (m.Ftp == 2)
                    tipoPago = "Pago Nº ";

                //Fila
                TableRow tr = new TableRow();
                tr.ID = m.Id.ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                if (m.TipoDocumento == 19)
                {
                    celFecha.Text = Convert.ToDateTime(m.Compra.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                }
                else
                {
                    celFecha.Text = Convert.ToDateTime(m.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                }
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                if (m.TipoDocumento == 19)
                    celNumero.Text = m.Compra.TipoDocumento + " " + m.Numero;
                else
                    celNumero.Text = tipoPago + m.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + m.Debe.ToString().Replace(',', '.');
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + m.Haber.ToString().Replace(',', '.');
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + m.Saldo.ToString().Replace(',', '.');
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.Width = Unit.Percentage(20);
                tr.Cells.Add(celSaldo);

                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + m.Id;
                cbSeleccion.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Attributes.Add("onchange", "javascript:return updatebox(" + m.Saldo + "," + m.Id.ToString() + ");");
                celSeleccion.Controls.Add(cbSeleccion);
                celSeleccion.Width = Unit.Percentage(5);
                //celSeleccion.VerticalAlign = VerticalAlign.Middle;
                celSeleccion.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celSeleccion);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                if (m.TipoDocumento == 19)
                {
                    LinkButton btnDetalles = new LinkButton();
                    btnDetalles.CssClass = "btn btn-info ui-tooltip";
                    btnDetalles.Attributes.Add("data-toggle", "tooltip");
                    btnDetalles.Attributes.Add("title data-original-title", "Comentarios");
                    btnDetalles.ID = "btnComent_" + m.Compra.Id;
                    btnDetalles.Text = "<span class='shortcut-icon icon-comment'></span>";
                    btnDetalles.Font.Size = 12;
                    //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                    btnDetalles.Click += new EventHandler(this.ComentariosCaja);
                    celAccion.Controls.Add(btnDetalles);
                }
                tr.Cells.Add(celAccion);

                phCobranzas.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error agregando movimiento a PH. " + ex.Message));
            }
        }
        private int verificarCierreCaja()
        {
            try
            {
                ControladorCaja contCaja = new ControladorCaja();
                int sucursal = Convert.ToInt32(this.idSucursal);
                int ptoVenta = Convert.ToInt32(this.puntoVenta);

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
        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PagosF.aspx?p=" + ListProveedor.SelectedValue + "&e=" + DropListEmpresa.SelectedValue + "&s=" + DropListSucursal.SelectedValue + "&pv=" + ListPuntoVenta.SelectedValue + "&bn=" + DropListTipo.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                 string idtildado = "";
                 int cierreOK = this.verificarCierreCaja();
                 if (cierreOK < 0)
                 {
                     ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                     return;
                 }

                //Chequeo si se ingreso dinero a imputar
                if (String.IsNullOrEmpty(this.txtImputar.Text) || this.txtImputar.Text == "0")
                {
                    foreach (Control C in phCobranzas.Controls)
                    {
                        TableRow tr = C as TableRow;
                        CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                        if (ch.Checked == true)
                        {
                            idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        }
                    }
                    if (!String.IsNullOrEmpty(idtildado))
                    {
                        Response.Redirect("PagosABM.aspx?bn=" + this.bn  +  "&d=" + idtildado + "&p=" + idProveedor + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=0&a=1");
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un movimiento"));
                    }
                }
                else
                {
                    //Consulto si hay documentos para Imputar
                    string movimientos = this.controlador.obtenerMovimientosImputarPago(Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture), this.idProveedor, this.idSucursal, this.idEmpresa, this.puntoVenta, this.idTipo, this.bn);
                    if (!String.IsNullOrEmpty(movimientos))
                    {
                        //Response.Redirect("ABMCobros.aspx?documentos=" + movimientos + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=" + txtImputar.Text + "&valor=1&tipo=" + this.DropListTipo.SelectedValue);

                        Response.Redirect("PagosABM.aspx?d=" + movimientos + "&p=" + idProveedor + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=" + this.txtImputar.Text.Replace(',', '.') + "&a=1" + "&bn=" + this.bn);

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El monto ingresado es menor al saldo de los documentos a imputar. "));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando documentos impagos al formulario de Cobros. " + ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }

        protected void btnPagoCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                //chequeo si esta la caja cerrada
                int cierreOK = this.verificarCierreCaja();
                if (cierreOK < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede hacer un cobro mientras la caja este cerrada para este punto de venta."));
                    return;
                }

                controladorPagos contPago = new controladorPagos();
                controladorDocumentos contDocumentos = new controladorDocumentos();
                controladorSucursal contSucursal = new controladorSucursal();

                PCuenta pago = new PCuenta();
                pago.tipo = contDocumentos.obtenerTipoDoc("Pago a Cuenta");
                pago.cliente.id = this.idProveedor;
                pago.empresa.id = idEmpresa;
                pago.sucursal.id = idSucursal;
                pago.total = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.saldoaFavor = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.puntoVenta = contSucursal.obtenerPtoVentaId(puntoVenta);
                pago.fecha = DateTime.Now;

                int i = contPago.agregarPagoCuenta(pago);
                if (i > 0)
                {
                    //Response.Redirect("ABMCobros.aspx?documentos=" + i + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=0&valor=1&tipo=" + this.DropListTipo.SelectedValue);
                    Response.Redirect("PagosABM.aspx?bn=" + this.bn + "&d=0&p=" + idProveedor + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=" + this.txtImputar.Text + "&a=2");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando Pago a Cuenta. "));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));

            }
        }

        protected void ListProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.verificarAlerta();
        }
        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtCodProveedor.Text);

                //cargo la lista
                this.ListProveedor.DataSource = dtProveedores;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void ComentariosCaja(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idCompra = atributos[1];
                string comentario = "-";

                 Gestion_Api.Entitys.Compra c = this.contCompra.obtenerCompraId(Convert.ToInt32(idCompra));
                if (c != null && c.Compras_Observaciones.Count > 0)
                {
                    comentario = c.Compras_Observaciones.FirstOrDefault().Observacion;
                }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('" + comentario + "')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar comentario. " + ex.Message));
            }
        }

        protected void btnImputar_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildadoNC = "";
                string idtildadoFC = "";

                foreach (Control C in phCobranzas.Controls)
                {
                    TableRow tr = C as TableRow;
                    string tipoDoc = tr.Cells[1].Text;
                    CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                    if (ch.Checked == true && tipoDoc.Contains("Crédito"))
                    {
                        idtildadoNC = ch.ID.Substring(12, ch.ID.Length - 12);
                    }
                    if (ch.Checked == true && (tipoDoc.Contains("Factura") || tipoDoc.Contains("Presupuesto")))
                    {
                        idtildadoFC = ch.ID.Substring(12, ch.ID.Length - 12);
                    }
                }
                if (!String.IsNullOrEmpty(idtildadoNC) && !String.IsNullOrEmpty(idtildadoFC))
                {
                    int i = this.controlador.imputarNotaCreditoACompra(Convert.ToInt32(idtildadoNC), Convert.ToInt32(idtildadoFC));

                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!.", Request.Url.ToString()));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error imputando Nota/s de Credito a Factura/s."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un movimiento"));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error intentando imputar documentos. " + ex.Message));
            }
        }
    }
}