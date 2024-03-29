﻿using Disipar.Models;
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
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras {
    using Gestion_Api.Entitys;
    using System.Reflection;
    using System.Web.Providers.Entities;

    public partial class RemitosABM : System.Web.UI.Page {
        controladorCompraEntity controlador = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();

        DataTable dtItemsTemp;

        DataTable dtItemsTemp2;

        List<TrazaTemp> trazabilidadTmp;
        Mensajes m = new Mensajes();
        int accion;
        int ordenReparacion;
        int ordenReparacionId;
        protected void Page_Load(object sender, EventArgs e) {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.ordenReparacion = Convert.ToInt32(Request.QueryString["or"]);
                this.ordenReparacionId = Convert.ToInt32(Request.QueryString["orID"]);
                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                this.VerificarLogin();
                this.CargarItems();

                if (!IsPostBack)
                {
                    //cargo fecha de hoy
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.cargarProveedores();
                    this.cargarSucursal();
                    this.dtItemsTemp = new DataTable();
                    this.dtItemsTemp2 = new DataTable();
                    this.trazabilidadTmp = new List<TrazaTemp>();
                    this.CrearTablaItems();
                    this.CrearTablaItems2();
                    this.CrearListaTrazabilidad();
                }

                if (ordenReparacion == 1)
                    GenerarRemitoDesdeOrdenReparacion();

                //verifico si es postback y tengo que llenar la tabla de las trazas para poder obtener el estado de los chkbox
                if (this.lblMovTraza.Text != "")
                {
                    string idTraza = this.lblMovTraza.Text;
                    int idArticulo = Convert.ToInt32(idTraza.Split('_')[1]);
                    this.CargarTrazasPH(idArticulo);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void VerificarLogin() {
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

        private int verificarAcceso() {
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
                                this.ListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                int i = this.verficarPermisoCambiarSucursal();
                                if (i <= 0)
                                {
                                    this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
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

        public int verficarPermisoCambiarSucursal() {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "85")
                        {
                            this.ListSucursal.Attributes.Remove("disabled");
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

        private void CrearTablaItems() {
            try
            {
                dtItemsTemp.Columns.Add("Codigo");
                dtItemsTemp.Columns.Add("Cant");
                dtItemsTemp.Columns.Add("Descripcion");
                dtItemsTemp.Columns.Add("Costo");
                dtItemsTemp.Columns.Add("IdArticulos");

                dtItems = dtItemsTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando tabla de item. " + ex.Message));
            }

        }
        private void CrearTablaItems2() {
            try
            {
                dtItemsTemp2.Columns.Add("Codigo");


                dtItems2 = dtItemsTemp2;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando tabla de item. " + ex.Message));
            }

        }

        private void CrearListaTrazabilidad() {
            try
            {
                vsTraza = trazabilidadTmp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando lista de trazabilidad. " + ex.Message));
            }

        }

        protected DataTable dtItems
        {
            get
            {
                if (ViewState["dtItems"] != null)
                {
                    return (DataTable)ViewState["dtItems"];
                }
                else
                {
                    return dtItemsTemp;
                }
            }
            set
            {
                ViewState["dtItems"] = value;
            }
        }

        protected DataTable dtItems2
        {
            get
            {
                if (ViewState["dtItems2"] != null)
                {
                    return (DataTable)ViewState["dtItems2"];
                }
                else
                {
                    return dtItemsTemp2;
                }
            }
            set
            {
                ViewState["dtItems2"] = value;
            }
        }

        protected List<TrazaTemp> vsTraza
        {
            get
            {
                if (ViewState["vsTraza"] != null)
                {
                    //var temp = ViewState["vsTraza"];
                    //return (List<Trazabilidad>)ViewState["vsTraza"];
                    //var dt
                    DataTable table = (DataTable)ViewState["vsTraza"];
                    List<TrazaTemp> lista = Helpers.ToListof<TrazaTemp>(table);
                    return lista;
                }
                else
                {
                    return new List<TrazaTemp>();
                }
            }
            set
            {
                var dt = Helpers.ListToDataTable(value);
                ViewState["vsTraza"] = dt;
            }
        }

        public void cargarProveedores() {
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

        public void cargarSucursal() {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

                this.ListSucursal.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        private void agregarItemATabla(string codigo, string Descripcion, decimal cant, decimal precio, int idArticulo) {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = idArticulo.ToString();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCodigo);
                celCodigo.Width = Unit.Percentage(15);

                TableCell celCant = new TableCell();
                celCant.Text = Descripcion;
                celCant.VerticalAlign = VerticalAlign.Middle;
                celCant.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCant);
                celCant.Width = Unit.Percentage(30);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$ " + precio;
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celPrecio);
                celPrecio.Width = Unit.Percentage(10);

                TableCell celCantidad = new TableCell();
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(5);

                TextBox txtCantidad = new TextBox();
                if (cant > 0)
                {
                    txtCantidad.Text = cant.ToString();
                }
                else
                {
                    txtCantidad.Text = "";
                }
                txtCantidad.TextMode = TextBoxMode.Number;
                txtCantidad.ID = "txtCantidad_" + idArticulo;
                txtCantidad.Attributes.Add("Style", "text-align: right;");
                //txtCantidad.Attributes.Add("min", "0");
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                celCantidad.Controls.Add(txtCantidad);
                tr.Cells.Add(celCantidad);

                TableCell celLote = new TableCell();
                celLote.HorizontalAlign = HorizontalAlign.Right;
                celLote.Width = Unit.Percentage(15);

                TextBox txtLote = new TextBox();
                txtLote.ID = "txtLote_" + idArticulo;
                txtLote.Attributes.Add("Style", "text-align: right;");
                celLote.Controls.Add(txtLote);
                tr.Cells.Add(celLote);

                TableCell celVencimiento = new TableCell();
                celVencimiento.HorizontalAlign = HorizontalAlign.Right;
                celVencimiento.Width = Unit.Percentage(15);

                TextBox txtVencimiento = new TextBox();
                txtVencimiento.ID = "txtVencimiento_" + idArticulo;
                txtVencimiento.Attributes.Add("Style", "text-align: right;");
                celVencimiento.Controls.Add(txtVencimiento);
                tr.Cells.Add(celVencimiento);

                TableCell celAccion = new TableCell();
                celAccion.Width = Unit.Percentage(15);

                LinkButton btnDetails = new LinkButton();
                //btnDetails.ID = art.id.ToString();
                btnDetails.CssClass = "btn btn-info ui-tooltip";
                btnDetails.Attributes.Add("data-toggle", "tooltip");
                btnDetails.Attributes.Add("title data-original-title", "Ver y/o Editar");
                btnDetails.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetails.Attributes.Add("onclick", "window.open('../Articulos/ArticulosABM.aspx?accion=2&id=" + idArticulo + "')");
                celAccion.Controls.Add(btnDetails);

                articulo articulo = this.contArticulosEntity.obtenerArticuloEntity(idArticulo);
                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton lbtnTraza = new LinkButton();
                if (accion == 5)//viene de un pedido
                {
                    lbtnTraza.Visible = false;
                    lbtnTraza.CssClass = "btn btn-default";
                }
                lbtnTraza.ID = "btnTraza_" + idArticulo;
                lbtnTraza.CssClass = "btn btn-info ui-tooltip";
                lbtnTraza.Text = "<span class='shortcut-icon icon-road'></span>";
                lbtnTraza.Click += new EventHandler(this.TrazabilidadItem);
                celAccion.Controls.Add(lbtnTraza);

                int trazable = this.contArticulos.verificarGrupoTrazableByID((int)articulo.grupo);
                if (trazable <= 0)
                {
                    lbtnTraza.Visible = false;
                }

                celAccion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celAccion);

                this.phProductos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando item a tabla. " + ex.Message));
            }
        }

        private void CargarItems() {
            try
            {
                this.phProductos.Controls.Clear();
                if (this.dtItems != null)
                {
                    foreach (DataRow item in this.dtItems.Rows)
                    {
                        this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]), Convert.ToDecimal(item["Costo"]), Convert.ToInt32(item["IdArticulos"]));
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }

        protected void lbtnAgregarArticuloASP_Click(object sender, EventArgs e) {
            try
            {

                DataTable dt = this.dtItems;
                //verifico que no este agregado a la grilla
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Codigo"].ToString() == this.txtCodigo.Text)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El articulo con codigo " + this.txtCodigo.Text + " ya se encuentra en la grilla"));
                        return;
                    }
                }

                //verifico que exista el codigo de articulo
                var a = this.contArticulos.obtenerArticuloCodigo(this.txtCodigo.Text);
                if (a == null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No exite articulo registrado con el codigo " + this.txtCodigo.Text + ". "));
                    return;
                }

                DataRow drFila = dt.NewRow();

                drFila["Codigo"] = this.txtCodigo.Text;
                drFila["Descripcion"] = a.descripcion;
                drFila["Cant"] = this.txtCantidad.Text;
                drFila["Costo"] = a.costo;
                drFila["IdArticulos"] = a.id;

                dt.Rows.Add(drFila);

                this.dtItems = dt;

                this.CargarItems();
                //limpio los campos
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }

        bool ComprobarNumeracionCorrectaRemito()
        {
            if(string.IsNullOrEmpty(txtPVenta.Text) || string.IsNullOrEmpty(txtNumero.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Los campos de numeracion estan vacios!\", {type: \"alert\"});", true);
                return false;
            }                
            if (txtPVenta.Text.Length != 4 || txtNumero.Text.Length != 8)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Los campos de numeracion son incorrectos!\", {type: \"alert\"});", true);
                return false;
            }

            return true;
        }

        private void guardarRemito() {
            try
            {
                if (!ComprobarNumeracionCorrectaRemito())
                    return;

                RemitosCompra rc = new RemitosCompra();

                rc.IdProveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                rc.Numero = this.txtPVenta.Text + this.txtNumero.Text;
                rc.Fecha = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                rc.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                rc.Tipo = Convert.ToInt32(this.ListTipoRemito.SelectedValue);
                rc.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                rc.RemitosCompras_Comentarios.Observacion = this.txtComentario.Text;
                rc.Devolucion = Convert.ToInt32(this.ListDevolucion.SelectedValue);

                if (rc.Devolucion > 0 && String.IsNullOrEmpty(this.txtComentario.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe cargar los comentarios cuando es Devolucion de Mercaderia!.\"); ", true);
                    return;
                }

                int cargarDatos = verificarProcedenciaProv();
                if (cargarDatos > 0)
                {
                    RemitosCompras_Despachos datos = new RemitosCompras_Despachos();
                    if (!String.IsNullOrEmpty(this.txtFechaDespacho.Text))
                    {
                        datos.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                    }
                    else
                    {
                        datos.FechaDespacho = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                    }
                    datos.NumeroDespacho = this.txtNumeroDespacho.Text;
                    datos.Lote = this.txtLote.Text;
                    datos.Vencimiento = this.txtVencimiento.Text;
                    rc.RemitosCompras_Despachos.Add(datos);
                }

                rc.RemitosCompras_Items = this.obtenerItems();

                if (rc.RemitosCompras_Items.Count > 0)
                {
                    int i = this.controlador.agregarRemito(rc, (int)rc.IdSucursal);
                    if (i > 0)
                    {
                        this.devolverTrazasSeleccionadas();
                        Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Remito nro " + rc.Numero + " generado con exito.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Remito Guardado con exito\", {type: \"info\"}); location.href='RemitosABM.aspx?a=1';", true);
                    }
                    else
                    {
                        if (i == -1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar remito. Reintente\", {type: \"error\"});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encontro stock para uno o mas articulos. Reintente\", {type: \"alert\"});", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Items del remito debe ser mayor a 0.\", {type: \"alert\"}); ", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remito. " + ex.Message));
            }
        }

        private void limpiarCampos() {
            try
            {
                this.ListProveedor.SelectedIndex = 0;
                this.txtPVenta.Text = "";
                this.txtNumero.Text = "";
                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error limpiando campos. " + ex.Message));
            }
        }

        private List<RemitosCompras_Items> obtenerItems() {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();
                int devolucion = Convert.ToInt32(this.ListDevolucion.SelectedValue);

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[3].Controls[0] as TextBox;
                    TextBox txtLote = tr.Cells[4].Controls[0] as TextBox;
                    TextBox txtVencimiento = tr.Cells[5].Controls[0] as TextBox;
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        //if (Convert.ToDecimal(txt.Text) > 0)
                        if (Convert.ToDecimal(txt.Text) != 0)
                        {
                            var item = new RemitosCompras_Items();
                            string idArt = txt.ID.Split('_')[1];
                            //Articulo A = contArticulos.obtenerArticuloCodigo((tr.Cells[0]).Text);
                            Articulo A = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArt));
                            item.Codigo = A.id;

                            decimal cantidad = Convert.ToDecimal(txt.Text);
                            if (devolucion > 0)
                            {
                                item.Cantidad = Math.Abs(cantidad) * -1;
                            }
                            else
                            {
                                item.Cantidad = cantidad;
                            }
                            //this.sumarCantAlArticuloSiHabiaTrazasSeleccionadas();

                            //datos despacho
                            item.Lote = txtLote.Text;
                            item.Vencimiento = txtVencimiento.Text;
                            item.NumeroDespacho = this.txtNumeroDespacho.Text;
                            try
                            {
                                if (!String.IsNullOrEmpty(this.txtFechaDespacho.Text))
                                    item.FechaDespacho = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                                else
                                    item.FechaDespacho = DateTime.Now;
                            }
                            catch { }

                            items.Add(item);
                            int trazable = contArticulos.verificarGrupoTrazableByID(A.grupo.id);
                            if (trazable > 0)
                            {
                                item.Trazabilidad = 1;
                            }
                            else
                            {
                                item.Trazabilidad = 0;
                            }
                        }

                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando items a remito" + ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error cargando items a remito. " + ex.Message + ". \", {type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo items. " + ex.Message));
                return null;
            }
        }

        private List<RemitosCompras_Items> filtrarItems() {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[3].Controls[0] as TextBox;
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        tr.Visible = true;
                    }
                    else
                    {
                        tr.Visible = false;
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo items. " + ex.Message));
                return null;
            }
        }

        private int verificarProcedenciaProv() {
            try
            {
                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                if (!c.pais.descripcion.Contains("Argentina"))
                {
                    return 1;
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e) {
            try
            {
                if (this.accion == 1) this.guardarRemito();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remito. " + ex.Message));
            }
        }

        private void cargarArticulosProveedor(int idPRoveedor) {
            try
            {
                List<Articulo> articulos = this.contArticulos.obtenerArticulosByProveedor(idPRoveedor);
                //limpio el dt
                this.dtItems.Rows.Clear();
                //this.GridProductos.AutoGenerateColumns = false;
                //this.GridProductos.DataSource = this.dtItems;


                foreach (var a in articulos)
                {
                    DataTable dt = this.dtItems;

                    DataRow drFila = dt.NewRow();


                    //cargo otros proveedores, si lo tiene configuraco
                    string codProveedor = WebConfigurationManager.AppSettings.Get("CodProveedorCompras");
                    if (codProveedor == "1" && !String.IsNullOrEmpty(codProveedor))
                    {
                        List<ProveedorArticulo> ProvArticulo = this.contArticulos.obtenerProveedorArticulosByArticulo(a.id);
                        string codArtProveedor = "";
                        foreach (var p in ProvArticulo)
                        {
                            codArtProveedor += p.codigoProveedor + " - ";
                        }

                        if (codArtProveedor.Length > 0)//saco el ultimo guion
                        {
                            codArtProveedor = codArtProveedor.Substring(0, codArtProveedor.Length - 3);
                        }

                        drFila["Codigo"] = a.codigo + " (" + codArtProveedor + ") ";
                    }
                    else
                    {
                        drFila["Codigo"] = a.codigo;
                    }

                    drFila["Descripcion"] = a.descripcion;
                    drFila["Cant"] = 0;
                    drFila["Costo"] = a.costo;
                    drFila["IdArticulos"] = a.id;

                    dt.Rows.Add(drFila);

                    this.dtItems = dt;
                }


                //this.GridProductos.DataBind();
                this.CargarItems();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos del proveedor. " + ex.Message));
            }
        }

        protected void ListProveedor_SelectedIndexChanged(object sender, EventArgs e) {
            Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
            c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
            if (!String.IsNullOrEmpty(c.alerta.descripcion))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
            }

            this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));

            if (!c.pais.descripcion.Contains("Argentina"))
            {
                this.panelDespacho.Visible = true;
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e) {
            this.filtrarItems();
        }

        protected void lbtnAgregarArticuloASP_Click1(object sender, EventArgs e) {

        }

        public void GenerarRemitoDesdeOrdenReparacion() {
            try
            {
                ControladorOrdenReparacionEntity contOrdenReparacionEnt = new ControladorOrdenReparacionEntity();

                var or = contOrdenReparacionEnt.ObtenerOrdenReparacionPorID(ordenReparacionId);

                ListTipoRemito.SelectedIndex = 1;

                ListDevolucion.SelectedIndex = 2;

                ListProveedor.SelectedValue = contArticulos.obtenerArticuloByID((int)or.Producto).proveedor.id.ToString();

                txtComentario.Text = or.DescripcionFalla;

                Cliente c = contCliente.obtenerProveedorID(Convert.ToInt32(this.ListProveedor.SelectedValue));
                c.alerta = contCliente.obtenerAlertaClienteByID(c.id);
                if (!String.IsNullOrEmpty(c.alerta.descripcion))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Alerta Proveedor: " + c.alerta.descripcion + ". \");", true);
                }

                this.cargarArticuloOrdenReparacion(or);

                //this.cargarArticulosProveedor(Convert.ToInt32(this.ListProveedor.SelectedValue));

                if (!c.pais.descripcion.Contains("Argentina"))
                {
                    this.panelDespacho.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando remito desde orden de reparacion " + ex.Message);
            }
        }

        private void cargarArticuloOrdenReparacion(OrdenReparacion or) {
            try
            {
                Articulo a = this.contArticulos.obtenerArticuloByID((int)or.Producto);
                //limpio el dt
                this.dtItems.Rows.Clear();

                DataTable dt = this.dtItems;

                DataRow drFila = dt.NewRow();

                //cargo otros proveedores, si lo tiene configuraco
                string codProveedor = WebConfigurationManager.AppSettings.Get("CodProveedorCompras");
                if (codProveedor == "1" && !String.IsNullOrEmpty(codProveedor))
                {
                    List<ProveedorArticulo> ProvArticulo = this.contArticulos.obtenerProveedorArticulosByArticulo(a.id);
                    string codArtProveedor = "";
                    foreach (var p in ProvArticulo)
                    {
                        codArtProveedor += p.codigoProveedor + " - ";
                    }

                    if (codArtProveedor.Length > 0)//saco el ultimo guion
                    {
                        codArtProveedor = codArtProveedor.Substring(0, codArtProveedor.Length - 3);
                    }

                    drFila["Codigo"] = a.codigo + " (" + codArtProveedor + ") ";
                }
                else
                {
                    drFila["Codigo"] = a.codigo;
                }

                drFila["Descripcion"] = a.descripcion;
                drFila["Cant"] = 1;
                drFila["Costo"] = a.costo;
                drFila["IdArticulos"] = a.id;

                dt.Rows.Add(drFila);

                this.dtItems = dt;

                this.CargarItems();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos del proveedor. " + ex.Message));
            }
        }

        #region devolucionTrazas
        private void TrazabilidadItem(object sender, EventArgs e) {
            try
            {
                string idBoton = (sender as LinkButton).ID;
                int idArticulo = Convert.ToInt32(idBoton.Split('_')[1]);

                this.lblMovTraza.Text = idBoton;

                this.CargarTrazasPH(idArticulo);

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('')", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar trazabilidad item factura. " + ex.Message));
            }
        }

        private void CargarTrazasPH(int idArt) {
            try
            {
                Articulo a = this.contArticulos.obtenerArticuloByID(idArt);
                this.cargarCamposGrupoTrazabilidad(a);
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarCamposGrupoTrazabilidad(Articulo art) {
            try
            {
                lblTrazaActual.Text = 0.ToString();
                phCamposTrazabilidad.Controls.Clear();
                List<Gestion_Api.Entitys.Trazabilidad_Campos> lstCampos = this.contArticulos.obtenerCamposTrazabilidadByGrupo(art.grupo.id);

                foreach (Gestion_Api.Entitys.Trazabilidad_Campos campos in lstCampos)
                {
                    TableHeaderCell th = new TableHeaderCell();
                    th.Text = campos.nombre;
                    phCamposTrazabilidad.Controls.Add(th);
                }
                this.CargarTrazasArticulo(art.id, lstCampos.Count);
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarTrazasArticulo(int idArticulo, int cantCampos) {
            try
            {
                controladorCompraEntity contCompra = new controladorCompraEntity();
                this.phItemsTrazabilidad.Controls.Clear();

                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                DataTable dt = new DataTable();
                dt = contCompra.obtenerTrazabilidadItemByArticulo(idArticulo, suc);

                int pos = 0;
                int columnas = 0;
                TableRow tr = new TableRow();
                string idTrazas = "";

                foreach (DataRow row in dt.Rows)
                {
                    if (columnas == 0)
                    {
                        tr = new TableRow();

                        TableCell celIndice = new TableCell();
                        celIndice.Text = (pos + 1).ToString();
                        celIndice.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celIndice);
                    }
                    if (columnas < cantCampos)
                    {
                        TableCell celCampo1 = new TableCell();
                        celCampo1.Text = row["valor"].ToString();
                        celCampo1.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celCampo1);
                        columnas++;
                        idTrazas += row["Id"].ToString() + "-";
                    }
                    if (columnas == (cantCampos))
                    {
                        TableCell celAccion = new TableCell();
                        CheckBox chkSeleccionT = new CheckBox();
                        chkSeleccionT.ID = "chkSeleccionT_" + idTrazas;
                        this.activarChkBox(chkSeleccionT, idTrazas);
                        chkSeleccionT.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        chkSeleccionT.Attributes.Add("onchange", "javascript:return updatebox(" + 1 + ",'" + chkSeleccionT.ID + "');");
                        celAccion.Controls.Add(chkSeleccionT);
                        celAccion.Width = Unit.Percentage(5);
                        celAccion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celAccion);

                        columnas = 0;
                        pos++;
                        idTrazas = "";
                        lblTrazaTotal.Text = pos.ToString();
                        phItemsTrazabilidad.Controls.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }

        protected void AgregarTraza_Click(object sender, EventArgs e) {
            try
            {
                this.agregarOQuitarTrazasDeLaSesion();
            }
            catch (Exception ex)
            {

            }
        }

        private void agregarOQuitarTrazasDeLaSesion() {
            try
            {
                string idArtBtnAceptarTraza = this.lblMovTraza.Text;
                int idArticulo = Convert.ToInt32(idArtBtnAceptarTraza.Split('_')[1]);
                List<TrazaTemp> trazasADevolver = new List<TrazaTemp>();

                //borro si es q ya habia trazas guardadas en el viewstate de ese articulo
                List<TrazaTemp> trazasAEliminar = new List<TrazaTemp>();

                if (vsTraza != null)
                {
                    if (vsTraza.Count > 0)
                    {
                        trazasADevolver = vsTraza;
                        trazasAEliminar = trazasADevolver.Where(x => x.IdArticulo == idArticulo).ToList();
                        trazasADevolver = trazasADevolver.Except(trazasAEliminar).ToList();
                    }
                }

                //recorro todos los items traza tildados y los agrego a una lista
                decimal cantidadXArticulo = 0;
                foreach (Control control in phItemsTrazabilidad.Controls)
                {
                    String idTildado = "";
                    TableRow tr = control as TableRow;
                    CheckBox ch = tr.Cells[tr.Cells.Count - 1].Controls[0] as CheckBox;

                    if (ch.Checked == true)
                    {
                        idTildado += ch.ID.Split('_')[1];
                        idTildado = idTildado.Substring(0, idTildado.Length - 1);
                        var idsTrazas = idTildado.Split('-');
                        cantidadXArticulo++;
                        foreach (var item in idsTrazas)
                        {
                            TrazaTemp trazaItem = new TrazaTemp();
                            trazaItem.IdTraza = Convert.ToInt32(item);
                            trazaItem.IdArticulo = idArticulo;

                            trazasADevolver.Add(trazaItem);
                        }
                    }
                }
                //seteo la cantidad en el txtbox del articulo por las trazas seleccionadas
                this.setearCantidadAlTxtBoxDelArticulo(cantidadXArticulo, idArticulo);

                DataRow dr = dtItems2.NewRow();
                dr[0] = trazasADevolver.Count();

                dtItems2.Rows.Add(dr);

                vsTraza = trazasADevolver;
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel8, UpdatePanel8.GetType(), "alert", "$.msgbox(\"Trazas seleccionadas correctamente. \", {type: \"info\"}); cerrarModal(); ", true);

            }
            catch (Exception ex)
            {

            }
        }

        private void setearCantidadAlTxtBoxDelArticulo(decimal cantidadXArticulo, int idArt) {
            try
            {
                foreach (Control control in phProductos.Controls)
                {
                    TableRow tr = control as TableRow;
                    LinkButton btn = tr.Cells[6].Controls[2] as LinkButton;
                    string idArticuloBtn = btn.ID.Split('_')[1];

                    if (idArticuloBtn.Contains(idArt.ToString()))
                    {
                        (tr.Cells[3].Controls[0] as TextBox).Text = cantidadXArticulo.ToString();
                        if (cantidadXArticulo == 0)
                        {
                            (tr.Cells[3].Controls[0] as TextBox).Text = "";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// activa si se habia apretado antes el checkbox
        /// </summary>
        /// <returns></returns>
        private void activarChkBox(CheckBox checkBox, string idTraza) {
            try
            {
                long traza = Convert.ToInt64(idTraza.Split('-')[0]);
                List<Trazabilidad> trazasADevolver = new List<Trazabilidad>();

                List<TrazaTemp> vsTrazabilidad = vsTraza;
                if (vsTrazabilidad != null)
                {
                    if (vsTrazabilidad.Count > 0)
                    {
                        foreach (TrazaTemp item in vsTrazabilidad)
                        {
                            trazasADevolver.Add(new Trazabilidad()
                            {
                                Id = item.IdTraza,
                                idArticulo = item.IdArticulo
                            });
                        }

                        var existe = trazasADevolver.Where(x => x.Id == traza).FirstOrDefault();
                        if (existe != null)
                        {
                            checkBox.Checked = true;
                            lblTrazaActual.Text = (Convert.ToInt32(lblTrazaActual.Text) + 1).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// cambia el estado a 4 de las trazas seleccionada y crea registros en la tabla Trazabilidad_Movimientos
        /// </summary>
        /// <param name="trazasADevolver"></param>
        private void devolverTrazasSeleccionadas() {
            try
            {
                int i = 0;
                if (vsTraza != null)
                {
                    foreach (var item in vsTraza)
                    {
                        i += this.contArticulos.CambiarEstadoTrazabilidadByID((int)item.IdTraza, 4);
                    }
                    if (i == vsTraza.Count)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}