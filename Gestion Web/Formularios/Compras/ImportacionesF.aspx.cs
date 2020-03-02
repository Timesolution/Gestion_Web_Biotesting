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

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImportacionesF : System.Web.UI.Page
    {
        ControladorImportaciones contImportaciones = new ControladorImportaciones();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorCliente contCliente = new controladorCliente();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorCompraEntity contCompEntity = new controladorCompraEntity();
        Configuracion config = new Configuracion();
        Mensajes m = new Mensajes();

        private string fechaD;
        private string fechaH;
        private int sucursal;
        private int proveedor;
        private int tipoFecha;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();
            //datos de filtro
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];
            sucursal = Convert.ToInt32(Request.QueryString["suc"]);
            proveedor = Convert.ToInt32(Request.QueryString["p"]);
            tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);

            if (!IsPostBack)
            {
                this.cargarProveedores();

                if (fechaD == null && fechaH == null)
                {
                    sucursal = (int)Session["Login_SucUser"];
                    proveedor = 0;
                    fechaD = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.cargarSucursal();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    this.ListTipoFecha.SelectedValue = this.tipoFecha.ToString();
                    this.DropListProveedor.SelectedValue = proveedor.ToString();
                    Response.Redirect("ImportacionesF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&tf=" + this.ListTipoFecha.SelectedValue);

                    //this.btnAccion.Visible = false;

                }
                else
                {
                    //this.btnAccion.Visible = true;
                }

                this.cargarSucursal();
                txtFechaDesde.Text = fechaD;
                txtFechaHasta.Text = fechaH;
                this.ListTipoFecha.SelectedValue = this.tipoFecha.ToString();
                this.DropListProveedor.SelectedValue = proveedor.ToString();

            }
            //verifico si el perfil tiene permiso para anular
            this.verficarPermisoAnular();

            if (fechaD != null && fechaH != null)
            {
                this.buscar(fechaD, fechaH, proveedor, sucursal);
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
                            //this.lbtnAnular.Visible = true;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void buscar(string fDesde, string fHasta, int proveedor, int idSucursal)
        {
            try
            {
                this.phImportaciones.Controls.Clear();

                DateTime desde = Convert.ToDateTime(fDesde, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(fHasta, new CultureInfo("es-AR")).AddHours(23);
                List<Importacione> importaciones = this.contImportaciones.buscarImportaciones(desde, hasta, idSucursal, proveedor, this.tipoFecha);
                if (importaciones != null)
                {
                    foreach (var i in importaciones)
                    {
                        this.cargarImportacionesPH(i);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando compras. " + ex.Message));
            }
        }
        private void cargarImportacionesPH(Importacione i)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = "TR_" + i.Id;

                TableCell celFechaDespacho = new TableCell();
                celFechaDespacho.Text = i.FechaDespacho.Value.ToString("dd/MM/yyyy");
                tr.Controls.Add(celFechaDespacho);

                TableCell celFechaFactura = new TableCell();
                celFechaFactura.Text = i.FechaFactura.Value.ToString("dd/MM/yyyy");
                tr.Controls.Add(celFechaFactura);

                TableCell celNroDespacho = new TableCell();
                celNroDespacho.Text = i.NroDespacho;
                tr.Controls.Add(celNroDespacho);

                TableCell celNroFactura = new TableCell();
                celNroFactura.Text = i.NroFactura;
                tr.Controls.Add(celNroFactura);

                Sucursal s = this.contSucursal.obtenerSucursalID(i.Sucursal.Value);
                TableCell celSucursal = new TableCell();
                celSucursal.Text = s.nombre;
                tr.Controls.Add(celSucursal);

                Cliente prov = this.contCliente.obtenerProveedorIDReduc(i.Proveedor.Value);
                TableCell celProveedor = new TableCell();
                celProveedor.Text = prov.razonSocial;
                tr.Controls.Add(celProveedor);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + i.Id.ToString();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.PostBackUrl = "ImportacionesABM.aspx?a=2&id=" + i.Id.ToString();
                celAccion.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + i.Id.ToString();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.OnClientClick = "abrirdialog(" + i.Id.ToString() + ");";
                celAccion.Controls.Add(btnEliminar);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                CheckBox chkSeleccion = new CheckBox();
                chkSeleccion.ID = "chkSeleccion_" + i.Id.ToString();
                chkSeleccion.CssClass = "btn btn-info";
                celAccion.Controls.Add(chkSeleccion);
                tr.Controls.Add(celAccion);

                this.phImportaciones.Controls.Add(tr);
            }
            catch (Exception ex)
            {

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
                        Response.Redirect("ImportacionesF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&tf=" + this.ListTipoFecha.SelectedValue);
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
                int id = Convert.ToInt32(this.txtMovimiento.Text);
                Importacione i = this.contImportaciones.obtenerImportacionByID(id);
                i.Estado = 0;

                int ok = this.contImportaciones.modificarImportacion(i);
                if (ok > 0)
                {
                    Gestion_Api.Modelo.Log.EscribirSQL(usuario, "INFO", "ANULACION Importacion id: " + id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Importacion anulada con exito. ", Request.Url.ToString()));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Importacion. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando. " + ex.Message));
            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        protected void lbtnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phImportaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[4] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("ImportacionesDetalleF.aspx?id=" + idtildado);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnGastos_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phImportaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[4] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("ImportacionesGastosF.aspx?id=" + idtildado);
                }
            }
            catch
            {

            }
        }

        protected void lbtnArribos_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                if (string.IsNullOrEmpty(txtPuntoVenta.Text) || string.IsNullOrEmpty(txtNumeroRemito.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"Debe completar el numero de remito. \");", true);
                    return;
                }

                int usuario = (int)Session["Login_IdUser"];
                string idtildado = "";
                foreach (Control C in phImportaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[4] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                        flag++;
                    }
                }
                
                if (flag > 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"Debe seleccionar solo una importacion a la vez. \");", true);
                    return;
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    Importacione importacion = this.contImportaciones.obtenerImportacionByID(Convert.ToInt32(idtildado));
                    if (importacion.MercaderiaArribo == 0)
                    {
                        string datosRemito = config.ArriboMercaderia;
                        string idProv = datosRemito.Split(';')[0];
                        string idSuc = datosRemito.Split(';')[1];
                        RemitosCompra rc = new RemitosCompra();
                        rc.IdProveedor = Convert.ToInt32(idProv);
                        rc.Numero = txtPuntoVenta.Text + txtNumeroRemito.Text;
                        rc.Fecha = DateTime.Now;
                        rc.IdSucursal = Convert.ToInt32(idSuc);
                        rc.Tipo = 1;
                        rc.Devolucion = 0;
                        rc.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                        rc.RemitosCompras_Comentarios.Observacion = "Remito generado por Arribo de mercaderia";

                        rc.RemitosCompras_Items = obtenerItems(importacion);
                        if (rc.RemitosCompras_Items.Count > 0)
                        {
                            int i = this.contCompEntity.agregarRemito(rc, (int)rc.IdSucursal);
                            if (i > 0)
                            {
                                importacion.MercaderiaArribo = 1;
                                this.contImportaciones.modificarImportacion(importacion);
                                Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Arribo de mercaderia procesado con exito");
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"Arribo de mercaderia procesado con exito. \");", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", "$.msgbox(\"No se pudo guardar remito. Reintente\");", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", "$.msgbox(\"La cantidad de items de la importacion debe ser mayor a 0.\");", true);
                        }
                        #region Old
                        //int ok = this.contImportaciones.cargarStockMercaderiaImportacion(Convert.ToInt32(idtildado));
                        //if (ok > 0)
                        //{
                        //    Gestion_Api.Modelo.Log.EscribirSQL(usuario, "INFO", "Carga Mercaderia Importacion id: " + idtildado);
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Mercaderia cargada con exito. ", Request.Url.ToString()));
                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"La mercaderia ya ha sido arribada. \");", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"Debe seleccionar alguna importacion para arribar. \");", true);
                }
              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", " $.msgbox(\"Ocurrio un error. \");", true); 
            }
        }

        private List<RemitosCompras_Items> obtenerItems(Importacione importacion)
        {
            try
            {
                List<RemitosCompras_Items> list = new List<RemitosCompras_Items>();

                foreach (var item in importacion.Importaciones_Detalle)
                {
                    var itemRemitoCompra = new RemitosCompras_Items();

                    Articulo articulo = contArticulo.obtenerArticuloByID((int)item.Articulo);

                    itemRemitoCompra.Codigo = item.Articulo;
                    itemRemitoCompra.Cantidad = item.Cantidad;
                    itemRemitoCompra.Lote = string.Empty;
                    itemRemitoCompra.Vencimiento = string.Empty;
                    itemRemitoCompra.NumeroDespacho = string.Empty;
                    itemRemitoCompra.FechaDespacho = DateTime.Now;
                    itemRemitoCompra.Trazabilidad = 0;

                    if (articulo != null)
                    {
                        int trazable = contArticulo.verificarGrupoTrazableByID(articulo.id);
                        if (trazable > 0)
                        {
                            itemRemitoCompra.Trazabilidad = 1;
                        }
                    }

                    list.Add(itemRemitoCompra);
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando items a remito" + ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanelArriboMercaderia, UpdatePanelArriboMercaderia.GetType(), "alert", "$.msgbox(\"Error cargando items a remito. " + ex.Message + ". \", {type: \"error\"});", true);
                return null;
            }
        }
    }

}