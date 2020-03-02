using Disipar.Models;
using Gestion_Api.Controladores;
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

namespace Gestion_Web.Formularios.Compras
{
    public partial class ComprasF : System.Web.UI.Page
    {
        controladorCompras controlador = new controladorCompras();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorCompraEntity contCompraEntity = new controladorCompraEntity();

        Mensajes m = new Mensajes();
        private int suc;
        private int tipoFecha;
        private string fechaD;
        private string fechaH;
        private string tipoDoc;
        private int puntoVenta;
        private int proveedor;
        private int empresa;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                //datos de filtro                
                fechaD = Request.QueryString["fd"];
                fechaH = Request.QueryString["fh"];
                tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                suc = Convert.ToInt32(Request.QueryString["s"]);
                proveedor = Convert.ToInt32(Request.QueryString["prov"]);
                tipoDoc = Request.QueryString["t"];
                puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                empresa = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null)
                    {
                        suc = (int)Session["Login_SucUser"];
                        //this.cargarSucursal();
                        fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                        fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        //tipo de documento??
                        //tipo = "Factura A";
                        //txtFechaDesde.Text = fechaD;
                        //txtFechaHasta.Text = fechaH;
                        //DropListSucursal.SelectedValue = suc.ToString();
                    }

                    //this.cargarSucursal();
                    cargarEmpresas();
                    cargarSucursalByEmpresa((int)Session["Login_EmpUser"]);
                    cargarProveedores();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtFechaDesdeImp.Text = fechaD;
                    txtFechaHastaImp.Text = fechaH;

                    DropListSucursal.SelectedValue = suc.ToString();
                    this.cargarPuntoVta(suc);
                    if (!String.IsNullOrEmpty(tipoDoc))
                    {
                        this.btnAnular.Visible = true;
                        DropListTipo.SelectedValue = tipoDoc.ToString();
                        if (tipoDoc == "1")
                        {
                            btnCitiCompras.Visible = true;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(fechaD) && !String.IsNullOrEmpty(fechaH) && !String.IsNullOrEmpty(tipoDoc))
                {
                    this.buscar(fechaD, fechaH, tipoDoc, suc, puntoVenta, proveedor, tipoFecha, empresa);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "31")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                int i = this.verficarPermisoCambiarSucursal();
                                if (i <= 0)
                                {
                                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                            }

                            valor = 1;
                        }

                        //Permiso para ver saldo
                        if (s == "116")
                            this.labelSaldo.Visible = true;
                    }
                }

                return valor;
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
                        if (s == "75")
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

        public void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                DataRow dr1 = dt.NewRow();
                dr1["nombre"] = "Todas";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 0);

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

                DropListEmpresa.SelectedValue = Session["Login_EmpUser"].ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarProveedores()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;

                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();


                dt = contCliente.obtenerProveedoresDT();


                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListProveedor.DataSource = dt;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";

                this.DropListProveedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
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
        private void cargarComras(List<Gestion_Api.Entitys.Compra> compras)
        {
            try
            {
                //borro todo
                this.phCompra.Controls.Clear();
                decimal saldo = 0;
                foreach (var c in compras)
                {
                    this.cargarEnPh(c);
                    if (c.TipoDocumento.Contains("Crédito"))
                    {
                        saldo += (decimal)c.Total * -1;
                    }
                    else
                    {
                        saldo += c.Total.Value;
                    }
                }

                this.labelSaldo.Text = "$" + saldo.ToString("N");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando compras. " + ex.Message));
            }
        }

        private void cargarEnPh(Gestion_Api.Entitys.Compra c)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = c.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(c.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celFechaImputacion = new TableCell();
                celFechaImputacion.Text = Convert.ToDateTime(c.FechaImputacion, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFechaImputacion.VerticalAlign = VerticalAlign.Middle;
                celFechaImputacion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaImputacion);

                TableCell celTipo = new TableCell();
                celTipo.Text = c.TipoDocumento;
                celTipo.HorizontalAlign = HorizontalAlign.Center;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celNumero = new TableCell();
                celNumero.Text = c.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                controladorCliente cont = new controladorCliente();
                var p = cont.obtenerProveedorID((int)c.Proveedor);
                TableCell celRazon = new TableCell();
                celRazon.Text = p.razonSocial;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);

                //si estoy cargando una nota de credito
                TableCell celNeto = new TableCell();
                if (c.TipoDocumento.Contains("Crédito"))
                {
                    celNeto.Text = "$" + ((decimal)c.Total * -1).ToString("N");
                }
                else
                {
                    celNeto.Text = "$" + ((decimal)c.Total).ToString("N");
                }
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNeto);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + c.Id + "_";
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.PostBackUrl = "ComprasABM.aspx?a=2&c=" + c.Id;
                //btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Detalles");
                btnEditar.ID = "btnEdit_" + c.Id + "_";
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 12;
                //btnEditar.PostBackUrl = "ComprasABM.aspx?a=5&c=" + c.Id;
                btnEditar.Click += new EventHandler(EditarFactura);
                celAccion.Controls.Add(btnEditar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + c.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Controls.Add(btnEliminar);

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phCompra.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando comra a la tabla. " + ex.Message));
            }
        }

        protected void EditarFactura(object sender, EventArgs e)
        {
            try
            {
                string idCompra = (sender as LinkButton).ID;
                string[] atributos = idCompra.Split('_');
                idCompra = atributos[1];

                var tienePago = contCompraEntity.ComprobarSiCompraTienePagoImputado(Convert.ToInt32(idCompra));

                if (tienePago)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede editar la compra porque tiene un pago imputado!"));
                else
                    Response.Redirect("ComprasABM.aspx?a=5&c=" + idCompra);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al editar Factura " + ex.Message);
            }
        }

        private void buscar(string fDesde, string fHasta, string tipoDoc, int sucursal, int puntoVenta, int proveedor, int tipoFecha, int empresa)
        {
            try
            {
                DateTime desde = Convert.ToDateTime(fDesde, new CultureInfo("es-AR"));
                DateTime Hasta = Convert.ToDateTime(fHasta, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                if (tipoDoc == "0")
                    tipoDoc = null;

                List<Gestion_Api.Entitys.Compra> compras = this.contCompraEntity.buscarCompras(desde, Hasta, tipoDoc, sucursal, puntoVenta, proveedor, tipoFecha, empresa);
                if (compras != null)
                {
                    this.cargarComras(compras);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando compras. " + ex.Message));
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
                        if (this.RadioFechaCompra.Checked == true)
                        {
                            //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                            Response.Redirect("ComprasF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&s=" + DropListSucursal.SelectedValue + "&t=" + DropListTipo.SelectedValue + "&pv=" + ListPuntoVenta.SelectedValue + "&prov=" + this.DropListProveedor.SelectedValue + "&e=" + this.DropListEmpresa.SelectedValue);
                        }
                        else
                        {
                            Response.Redirect("ComprasF.aspx?tf=1&fd=" + txtFechaDesdeImp.Text + "&fh=" + txtFechaHastaImp.Text + "&s=" + DropListSucursal.SelectedValue + "&t=" + DropListTipo.SelectedValue + "&pv=" + ListPuntoVenta.SelectedValue + "&prov=" + this.DropListProveedor.SelectedValue + "&e=" + this.DropListEmpresa.SelectedValue);
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

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {

                string idtildado = "";
                foreach (Control C in phCompra.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[8].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                //if (!String.IsNullOrEmpty(idtildado))
                //{
                //    int i = this.controlador.ProcesoEliminarFactura(idtildado);
                //    if (i > 0)
                //    {
                //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Facturas anuladas con exito. ", "FacturasF.aspx"));
                //    }
                //    else
                //    {
                //        if (i == -2)
                //        {
                //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron anular las facturas seleccionadas ya que una de ellas registra cancelaciones."));

                //        }
                //        else
                //        {

                //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Facturas. "));
                //        }
                //    }
                //}
                //else
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Documento"));
                //}

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para anular. " + ex.Message));
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
                foreach (Control C in phCompra.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[8].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
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

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }


        protected void btnCitiCompras_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fechaD == null && this.fechaH == null)
                {
                    fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                }
                Response.Redirect("ImpresionCompras.aspx?a=2&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&t=" + this.tipoDoc + "&ex=1");
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                foreach (Control C in phCompra.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[4] as CheckBox;
                    if (ch != null)
                    {
                        if (ch.Checked == true)
                        {
                            idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "" && id != null)
                        {
                            int i = this.contCompraEntity.anularCompra(Convert.ToInt64(id));

                            if (i > 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Compra anulada con exito!. ", "ComprasF.aspx?fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&t=" + this.tipoDoc));
                            }
                            else
                            {
                                if (i == -1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando compra. "));
                                }
                                if (i == -2)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La compra seleccionada tiene pagos realizados. Por favor elimine los pagos realizados primero. "));
                                }


                            }
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una Compra."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando compras para anular. " + ex.Message));
            }
        }
        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtCodProveedor.Text);

                //cargo la lista
                this.DropListProveedor.DataSource = dtProveedores;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";

                this.DropListProveedor.DataBind();

                DataRow dr2 = dtProveedores.NewRow();
                dr2["razonSocial"] = "Todos";
                dr2["id"] = -1;
                dtProveedores.Rows.InsertAt(dr2, 0);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fechaD == null && this.fechaH == null)
                {
                    fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                }
                Response.Redirect("ImpresionCompras.aspx?a=1&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&e=" + this.empresa + "&t=" + this.tipoDoc + "&ex=1");
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fechaD == null && this.fechaH == null)
                {
                    fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                }
                //Response.Redirect("ImpresionCompras.aspx?a=1&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&t=" + this.tipoDoc + "&ex=0");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=1&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&e="+ this.empresa + "&t=" + this.tipoDoc + "&ex=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnExportarDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fechaD == null && this.fechaH == null)
                {
                    fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                }
                Response.Redirect("ImpresionCompras.aspx?a=7&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&t=" + this.tipoDoc + "&e=" + this.empresa + "&ex=1");
            }
            catch
            {

            }
        }
        protected void lbtnImprimirDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fechaD == null && this.fechaH == null)
                {
                    fechaD = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                }
                //Response.Redirect("ImpresionCompras.aspx?a=1&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&t=" + this.tipoDoc + "&ex=0");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=7&tf=" + this.tipoFecha + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&pv=" + this.puntoVenta + "&prov=" + this.proveedor + "&t=" + this.tipoDoc + "&e=" + this.empresa + "&ex=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }
    }
}