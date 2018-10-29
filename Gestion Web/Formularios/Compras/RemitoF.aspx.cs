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
    public partial class RemitoF : System.Web.UI.Page
    {
        controladorCompraEntity contCompraEntity = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int sucursal;
        private int proveedor;
        private int tipo;
        private int devolucion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();
            //datos de filtro
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];
            sucursal = Convert.ToInt32(Request.QueryString["suc"]);
            proveedor = Convert.ToInt32(Request.QueryString["p"]);
            tipo = Convert.ToInt32(Request.QueryString["t"]);
            devolucion = Convert.ToInt32(Request.QueryString["dev"]);
            if (!IsPostBack)
            {
                this.cargarProveedores();                

                if (fechaD == null && fechaH == null)
                {
                    sucursal = (int)Session["Login_SucUser"];
                    proveedor = 0;
                    tipo = 0;
                    fechaD = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    //tipo de documento??
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.btnAccion.Visible = false;
                    
                }
                else
                {
                    this.btnAccion.Visible = true;
                }

                this.cargarSucursal();
                txtFechaDesde.Text = fechaD;
                txtFechaHasta.Text = fechaH;
                this.ListTipoRemito.SelectedValue = tipo.ToString();
                this.DropListProveedor.SelectedValue = proveedor.ToString();
                this.ListSiDevolucion.SelectedValue = this.devolucion.ToString();
            }
            //verifico si el perfil tiene permiso para anular
            this.verficarPermisoAnular();

            if (fechaD != null && fechaH != null)
            {
                this.buscar(fechaD, fechaH, proveedor, sucursal,tipo,devolucion);
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
                        if (s == "29")
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

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);                


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.DropListSucursal.SelectedValue = this.sucursal.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

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
                        if (s == "78")
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
        private void buscar(string fDesde, string fHasta, int proveedor,int idSucursal,int tipo, int devolucion)
        {
            try
            {
                DateTime desde = Convert.ToDateTime(fDesde, new CultureInfo("es-AR"));
                DateTime Hasta = Convert.ToDateTime(fHasta, new CultureInfo("es-AR")).AddHours(23);
                List<Gestion_Api.Entitys.RemitosCompra> remitos = this.contCompraEntity.buscarRemito(desde, Hasta, proveedor, idSucursal, tipo, devolucion);

                this.cargarRemitos(remitos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando compras. " + ex.Message));
            }
        }

        private void cargarRemitos(List<Gestion_Api.Entitys.RemitosCompra> remitos)
        {
            try
            {
                //borro todo
                this.phRemitos.Controls.Clear();
                foreach (var r in remitos)
                {
                    this.cargarEnPh(r);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarEnPh(Gestion_Api.Entitys.RemitosCompra rc)
        {
            try
            {
                controladorSucursal contsuc = new controladorSucursal();
                Sucursal suc = contsuc.obtenerSucursalID(rc.IdSucursal.Value);

                //fila
                TableRow tr = new TableRow();
                tr.ID = rc.Id.ToString();
                if (rc.Estado == 0)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                }

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(rc.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                if(rc.Devolucion != null)
                {
                    if (rc.Devolucion > 0)
                        celTipo.Text = "Devolucion";
                    else
                        celTipo.Text = "Ingreso";
                }
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celNumero = new TableCell();
                celNumero.Text = rc.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = rc.cliente.alias;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = suc.nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);


                //si estoy cargando una nota de credito

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + rc.Id + "_";
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                btnDetalles.Click += new EventHandler(this.detalleRemito);
                //btnDetalles.PostBackUrl = "RemitoDetalleF.aspx?r=" + rc.Id;
                
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + rc.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Controls.Add(btnEliminar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnDetallesExcel = new LinkButton();
                btnDetallesExcel.CssClass = "btn btn-info ui-tooltip";
                btnDetallesExcel.Attributes.Add("data-toggle", "tooltip");
                btnDetallesExcel.Attributes.Add("title data-original-title", "DetallesExcel");
                btnDetallesExcel.ID = "btnSelecEx_" + rc.Id;
                btnDetallesExcel.Text = "<span class='fa fa-file-text-o'></span>";
                btnDetallesExcel.Font.Size = 12;
                btnDetallesExcel.PostBackUrl = "ImpresionCompras.aspx?a=8&ex=1&rc=" + rc.Id;
                celAccion.Controls.Add(btnDetallesExcel);

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phRemitos.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando comra a la tabla. " + ex.Message));
            }
        }

        private void detalleRemito(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idRemito = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=8&rc=" + idRemito + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de remito desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando detalle remito desde la interfaz. " + ex.Message);
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListProveedor.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("RemitoF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&t=" + this.ListTipoRemito.SelectedValue + "&dev=" + this.ListSiDevolucion.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una proveedor"));
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
                int usuario = (int)Session["Login_IdUser"];
                int j = 0;
                string idtildado = "";
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String idRemito in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(idRemito))
                        {
                            int i = this.contCompraEntity.anularRemito(Convert.ToInt64(idRemito), usuario);
                            if (i < 1)                            
                            {
                                j++;
                                if (i == -1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Remitos. "));
                                }
                                if (i == -2)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error quitando stock de los articulos del remito. "));
                                }
                                if (i == -3)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error registrando el movimiento de stock. "));
                                }
                                if (i == -4)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No puede eliminar el remito ya que contiene trazabilidad cargada que fue utilizada!. "));
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Remitos. "));
                                }
                            }
                            Gestion_Api.Modelo.Log.EscribirSQL(usuario, "INFO", "ANULACION remito compra id: " + idRemito);
                        }
                    }
                    if (j > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Uno o más remitos no pudieron ser anulados. "));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Remitos anulados con exito. ", "RemitoF.aspx"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Documento"));
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionCompras.aspx?a=5&ex=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.sucursal + "&prov=" + this.proveedor + "&tipo=" + this.tipo + "&dev=" + this.devolucion);
            }
            catch
            {

            }
        }

        protected void lbtnTrazabilidad_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String idRemito in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(idRemito))
                        {
                            Response.Redirect("TrazabilidadF.aspx?rc=" + idRemito);
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Documento"));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=5&ex=0&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.sucursal + "&prov=" + this.proveedor + "&tipo=" + this.tipo + "&dev=" + this.devolucion + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void lbtnImprimirEtiquetas_Click(object sender, EventArgs e)
        {
            try
            {
                string idTildados = "";
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idTildados += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idTildados))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=9&ex=0&ids=" + idTildados + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Documento"));
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void lbtnGenerarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                int cantidadTildada = 0;
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        cantidadTildada++;
                    }
                    
                }
                if (!String.IsNullOrEmpty(idtildado) && cantidadTildada == 1)
                {
                    foreach (String idRemito in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(idRemito))
                        {
                            Response.Redirect("ComprasABM.aspx?a=3&r=" + idRemito);
                        }
                    }
                }
                else if(!String.IsNullOrEmpty(idtildado) && cantidadTildada > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo un Documento"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un Documento"));
                }                
            }
            catch
            {

            }
        }
    }
}