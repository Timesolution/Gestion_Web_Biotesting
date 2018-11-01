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
    public partial class OrdenesCompraF : System.Web.UI.Page
    {
        controladorCompraEntity contCompraEntity = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private string fechaEntregaD;
        private string fechaEntregaH;
        private int filtroPorFecha;
        private int filtroPorFechaEntrega;
        private int sucursal;
        private int proveedor;
        private int estado;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();
            //datos de filtro
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];
            fechaEntregaD = Request.QueryString["fed"];
            fechaEntregaH = Request.QueryString["feh"];
            sucursal = Convert.ToInt32(Request.QueryString["suc"]);
            proveedor = Convert.ToInt32(Request.QueryString["p"]);
            estado = Convert.ToInt32(Request.QueryString["e"]);
            filtroPorFecha = Convert.ToInt32(Request.QueryString["fpf"]);
            filtroPorFechaEntrega = Convert.ToInt32(Request.QueryString["fpfe"]);

            if (!IsPostBack)
            {
                this.cargarProveedores();

                if (fechaD == null && fechaH == null)
                {
                    sucursal = (int)Session["Login_SucUser"];
                    proveedor = 0;
                    //fechaD = DateTime.Now.AddMonths(-2).ToString("dd/MM/yyyy");
                    //fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    //tipo de documento??
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaEntregaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaEntregaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.btnAccion.Visible = false;
                    estado = 0;
                    filtroPorFecha = 1;
                    filtroPorFechaEntrega = 0;
                }
                else
                {
                    this.btnAccion.Visible = true;
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtFechaEntregaDesde.Text = fechaEntregaD;
                    txtFechaEntregaHasta.Text = fechaEntregaH;
                }                

                if(proveedor > 0)
                    lbtnEntregas.Visible = true;

                this.cargarEstadosFiltro();
                this.cargarEstados();
                this.cargarSucursal();
                //txtFechaDesde.Text = fechaD;
                //txtFechaHasta.Text = fechaH;                
            }

            if (fechaD != null && fechaH != null)
            {
                this.buscar(fechaD, fechaH, proveedor, sucursal,estado,fechaEntregaD,fechaEntregaH,filtroPorFecha,filtroPorFechaEntrega);
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
                int tienePermiso = 0;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        //Verifico si posee permisos para modificar estados de las Ordenes de Compra
                        if (s == "115")
                        {
                            this.phCambiarEstadoOC.Visible = true;
                        }

                        if (s == "28")
                        {
                            //verifico si es super admin
                            //string perfil = Session["Login_NombrePerfil"] as string;
                            //if (perfil == "SuperAdministrador")
                            //{
                            //}
                            tienePermiso = 1;
                        }

                        if (s == "177")
                            this.DropListSucursal.Attributes.Remove("disabled");

                        if (s == "178")
                            ltbnCambiarEstado.Visible = true;
                    }
                }

                return tienePermiso;
            }
            catch
            {
                return -1;
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
                dr["nombre"] = "Todas...";
                dr["id"] = -1;
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

        public void cargarEstadosFiltro()
        {
            try
            {
                var estados = contCompraEntity.obtenerOrdenesCompra_Estados();

                estados.Insert(0, new Gestion_Api.Entitys.OrdenesCompra_Estados
                {
                    Id = 0,
                    TipoEstado = "Todos"
                });

                //agrego todos

                this.DropListEstadoFiltro.DataSource = estados;
                this.DropListEstadoFiltro.DataValueField = "Id";
                this.DropListEstadoFiltro.DataTextField = "TipoEstado";
                this.DropListEstadoFiltro.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarEstados()
        {
            try
            {
                var estados = contCompraEntity.obtenerOrdenesCompra_Estados();                

                //agrego todos
                this.DropListEstados.DataSource = estados;
                this.DropListEstados.DataValueField = "Id";
                this.DropListEstados.DataTextField = "TipoEstado";
                this.DropListEstados.DataBind();

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
        private void buscar(string fDesde, string fHasta, int proveedor,int idSucursal,int estado, string fEntregaD,string fEntregaH,int filtroPorFecha, int filtroPorFechaEntrega)
        {
            try
            {
                DateTime desde = Convert.ToDateTime(fDesde, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(fHasta, new CultureInfo("es-AR"));
                DateTime entregaD = Convert.ToDateTime(fEntregaD, new CultureInfo("es-AR"));
                DateTime entregaH = Convert.ToDateTime(fEntregaH, new CultureInfo("es-AR"));
                //int estado = Convert.ToInt32(DropListEstado.SelectedValue);

                List<Gestion_Api.Entitys.OrdenesCompra> ordenes = this.contCompraEntity.buscarOrden(desde, hasta, proveedor, idSucursal, estado, entregaD, entregaH, filtroPorFecha, filtroPorFechaEntrega);

                this.cargarOrdenes(ordenes);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando compras. " + ex.Message));
            }
        }

        private void cargarOrdenes(List<Gestion_Api.Entitys.OrdenesCompra> ordenes)
        {
            try
            {
                //borro todo
                this.phOrdenes.Controls.Clear();
                foreach (var oc in ordenes)
                {
                    this.cargarEnPh(oc);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarEnPh(Gestion_Api.Entitys.OrdenesCompra oc)
        {
            try
            {
                controladorSucursal contsuc = new controladorSucursal();
                controladorCompraEntity contComprasEnt = new controladorCompraEntity();
                Sucursal suc = contsuc.obtenerSucursalID(oc.IdSucursal.Value);

                var oce = this.contCompraEntity.obtenerOrdenCompra_Estado_PorId((int)oc.Estado);

                //fila
                TableRow tr = new TableRow();
                tr.ID = oc.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(oc.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = oc.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                Gestor_Solution.Modelo.Cliente p = this.contCliente.obtenerProveedorID(oc.IdProveedor.Value);
                celRazon.Text = p.razonSocial;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = suc.nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);

                TableCell celEstado = new TableCell();
                celEstado.Text = oce.TipoEstado;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEstado);


                //si estoy cargando una nota de credito

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + oc.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                //btnDetalles.PostBackUrl = "ImpresionCompras.aspx?a=3&oc=" + oc.Id;
                btnDetalles.Click += new EventHandler(this.detalleOrden);
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);                

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + oc.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Controls.Add(btnEliminar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Detalles");
                btnEditar.ID = "btnEdit_" + oc.Id;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 12;
                btnEditar.PostBackUrl = "OrdenesCompraABM.aspx?a=2&oc=" + oc.Id;
                celAccion.Controls.Add(btnEditar);

                Literal l4 = new Literal();
                l4.Text = "&nbsp";
                celAccion.Controls.Add(l4);

                LinkButton btnDetallesExcel = new LinkButton();
                btnDetallesExcel.CssClass = "btn btn-info ui-tooltip";
                btnDetallesExcel.Attributes.Add("data-toggle", "tooltip");
                btnDetallesExcel.Attributes.Add("title data-original-title", "DetallesExcel");
                btnDetallesExcel.ID = "btnSelecEx_" + oc.Id;
                btnDetallesExcel.Text = "<span class='fa fa-file-text-o'></span>";
                btnDetallesExcel.Font.Size = 12;
                btnDetallesExcel.PostBackUrl = "ImpresionCompras.aspx?a=3&ex=1&oc=" + oc.Id;
                celAccion.Controls.Add(btnDetallesExcel);
                
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phOrdenes.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando ordenes a la tabla. " + ex.Message));
            }
        }

        private void detalleOrden(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idOrden = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=3&oc="+ idOrden +"', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando detalle orden desde la interfaz. " + ex.Message);
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
                        Response.Redirect("OrdenesCompraF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" +this.DropListSucursal.SelectedValue + "&e=" + this.DropListEstadoFiltro.SelectedValue + "&fed=" + txtFechaEntregaDesde.Text + "&feh=" + txtFechaEntregaHasta.Text + "&fpf=" + Convert.ToInt32(RadioFechaOrdenCompra.Checked) + "&fpfe=" + Convert.ToInt32(RadioFechaEntrega.Checked));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar un proveedor"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de ordenes. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int j = 0;
                string idtildado = "";
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[1] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (String idOrden in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(idOrden))
                        {
                            int i = this.contCompraEntity.anularOrden(Convert.ToInt64(idOrden));
                            if (i < 1)                            
                            {
                                j++;
                                if (i == -1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Ordenes de compra. "));
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Ordenes de compra. "));
                                }
                            }
                        }
                    }
                    if (j > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Una o más Ordenes de compra no pudieron ser anuladas. "));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Ordenes de compra anuladas con exito. ", "OrdenesCompraF.aspx"));
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

        protected void lbtnPendienteOC_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(1);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnAtrasadoOC_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(2);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnAutorizado_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(3);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnObservado_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(4);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnRechazado_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(5);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnEntregaParcial_Click(object sender, EventArgs e)
        {
            try
            {
                this.modificarEstadoOrdenCompra(6);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error modificando el estado de la Orden de Compra. Excepción: " + Ex.Message));
            }
        }
        private void modificarEstadoOrdenCompra(int estado)
        {
            try
            {
                string idtildado = string.Empty;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.contCompraEntity.modificarEstadoOrdenCompraId(Convert.ToInt64(idtildado), estado);

                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Estado de la Orden de Compra modificado con éxito. ", "OrdenesCompraF.aspx?fd="+ txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar el estado de la Orden de Compra seleccionada. "));
                    }
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this.GetType(), UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar algún documento!"));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando el estado de una Orden de Compra. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnEntregas_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("EntregasMercaderiaF.aspx?oc="+idtildado);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando entregas de mercaderia. " + ex.Message);
            }
        }

        protected void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    var or = contCompraEntity.obtenerOrden(Convert.ToInt64(idtildado));

                    int estadoNuevo = Convert.ToInt32(DropListEstados.SelectedValue);
                    string observacion = txtObservaciones.Text;

                    int temp = contCompraEntity.AgregarYGuardarOrdenesCompra_Observaciones(or.Id,(int)or.Estado, estadoNuevo, observacion);

                    if(temp < 0)
                        Log.EscribirSQL(1, "ERROR", "Error agregando ordenCompra_observacion.");
                }
                
                modificarEstadoOrdenCompra(Convert.ToInt32(DropListEstados.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cambiando estado orden de compra. " + ex.Message);
            }
        }
    }
}