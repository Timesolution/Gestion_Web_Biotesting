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
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class StockInicialABM : System.Web.UI.Page
    {
        controladorCompraEntity controlador = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorSucursal contSuc = new controladorSucursal();
        controladorCliente contCliente = new controladorCliente();

        DataTable dtItemsTemp;
        Mensajes m = new Mensajes();
        int accion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.VerificarLogin();
                this.CargarItems();

                if (!IsPostBack)
                {
                    //cargo fecha de hoy
                    this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.cargarProveedorGenerico();
                    this.cargarSucursal();
                    this.dtItemsTemp = new DataTable();
                    this.CrearTablaItems();
                }
            }
            catch (Exception Ex)
            {

            }
        }

        #region Carga Inicial
        public void cargarSucursal()
        {
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

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarProveedorGenerico()
        {
            try
            {
                Configuracion config = new Configuracion();

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

                var proveedor = this.contCliente.obtenerProveedorID(Convert.ToInt32(config.ProveedorGenerico));
                if(proveedor != null)
                {
                    this.ListProveedor.SelectedValue = Convert.ToString(proveedor.id);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedor generico a la lista. " + ex.Message));
            }
        }
        private void CargarItems()
        {
            try
            {
                int verCargados = Convert.ToInt32(this.lblVerCargados.Text);
                this.phProductos.Controls.Clear();
                if (this.dtItems != null)
                {
                    foreach (DataRow item in this.dtItems.Rows)
                    {
                        if (verCargados > 0)
                        {
                            if (item["Cant"].ToString() != "0" && !String.IsNullOrEmpty(item["Cant"].ToString()))
                            {
                                this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]));
                            }
                        }
                        else
                        {
                            this.agregarItemATabla(item["Codigo"].ToString(), item["Descripcion"].ToString(), Convert.ToDecimal(item["Cant"]));
                        }
                    }
                }
                //this.UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }
        #endregion

        #region ABM
        private void guardarRemito()
        {
            try
            {
                RemitosCompra rc = new RemitosCompra();
                
                rc.IdProveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                rc.Numero = this.txtPVenta.Text + this.txtNumero.Text;
                rc.Fecha = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                rc.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                rc.Tipo = Convert.ToInt32(this.ListTipoRemito.SelectedValue);
                rc.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                rc.RemitosCompras_Comentarios.Observacion = this.txtComentario.Text;
                rc.Devolucion = Convert.ToInt32(this.ListDevolucion.SelectedValue);

                //obengo items
                rc.RemitosCompras_Items = this.obtenerItems();

                if (rc.RemitosCompras_Items.Count >= 0)
                {
                    int i = this.controlador.agregarRemito(rc, (int)rc.IdSucursal);
                    if (i > 0)
                    {
                        Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Remito nro " + rc.Numero + " generado con exito.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Remito Guardado con exito\", {type: \"info\"}); location.href='RemitosABM.aspx?a=1';", true);
                    }
                    else
                    {
                        if (i == -1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar remito. Reintente\", {type: \"warning\"});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encontro stock para uno o mas articulos. Reintente\", {type: \"warning\"});", true);
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Items del remito debe ser mayor a 0.\", {type: \"warning\"}); ", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando remito. " + ex.Message));
            }
        }
        private List<RemitosCompras_Items> obtenerItems()
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();
                int devolucion = Convert.ToInt32(this.ListDevolucion.SelectedValue);

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        if (Convert.ToDecimal(txt.Text) != 0)
                        {
                            var item = new RemitosCompras_Items();
                            Articulo A = contArticulos.obtenerArticuloCodigo((tr.Cells[0]).Text);
                            if (A != null)
                            {
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
        private void eliminarItem(object sender, EventArgs e)
        {
            try
            {
                string codigo = (sender as LinkButton).ID.ToString();
                string[] datos = codigo.Split('_');
                codigo = datos[1];

                //busco el item en la tabla según el código que tiene seteado el botón
                foreach (DataRow item in this.dtItems.Rows)
                {
                    if (item["Codigo"].ToString() == codigo)
                    {
                        dtItems.Rows.Remove(item);
                        break;
                    }
                }
                //cargo nuevamente los items a la tabla
                this.CargarItems();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando item de la tabla. " + Ex.Message));
            }
        }
        #endregion

        #region Funciones Auxiliares
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
                        if (s == "28")
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
        private string obtenerArticulosIncorrectos()
        {
            try
            {
                string aux = "";

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    if (!String.IsNullOrEmpty(txt.Text))
                    {
                        Articulo A = contArticulos.obtenerArticuloCodigo((tr.Cells[0]).Text);
                        if (A == null)
                        {
                            aux += tr.Cells[0].Text + ", ";
                        }
                    }
                }
                return aux;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un errror en obtenerArticulosIncorrectos. Excepción: " + Ex.Message));
                return null;
            }
        }
        private void CrearTablaItems()
        {
            try
            {
                dtItemsTemp.Columns.Add("Codigo");
                dtItemsTemp.Columns.Add("Descripcion");
                dtItemsTemp.Columns.Add("Cant");


                dtItems = dtItemsTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando tabla de item. " + ex.Message));
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
        private void agregarItemATabla(string codigo, string Descripcion, decimal cant)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCodigo);

                TableCell celCant = new TableCell();
                celCant.Text = Descripcion;
                celCant.VerticalAlign = VerticalAlign.Middle;
                celCant.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCant);

                TableCell celCantidad = new TableCell();
                celCantidad.HorizontalAlign = HorizontalAlign.Right;

                TextBox txtCantidad = new TextBox();
                txtCantidad.ID = codigo;
                if (cant > 0)
                {
                    txtCantidad.Text = cant.ToString();
                }
                else
                {
                    txtCantidad.Text = "";
                }
                txtCantidad.TextMode = TextBoxMode.Number;
                txtCantidad.Attributes.Add("Style", "text-align: right;");
                txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                txtCantidad.AutoPostBack = true;
                txtCantidad.TextChanged += new EventHandler(this.cargarCantidadItem);
                celCantidad.Controls.Add(txtCantidad);
                tr.Cells.Add(celCantidad);


                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Eliminar");
                btnEliminar.ID = "btnEliminar_" + codigo;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.eliminarItem);
                btnEliminar.Attributes.Add("target", "_blank");
                celAccion.Controls.Add(btnEliminar);
                celAccion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celAccion);

                this.phProductos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando item a tabla. " + ex.Message));
            }
        }
        private void cargarCantidadItem(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as TextBox).ID;
                foreach (DataRow row in this.dtItems.Rows)
                {
                    if (row["codigo"] == id)
                    {
                        row["Cant"] = (sender as TextBox).Text;
                    }
                }
            }
            catch
            {

            }
        }
        private int verificarProcedenciaProv()
        {
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
        
        #endregion

        #region Eventos Controles
        protected void lbtnAgregarArticuloASP_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = this.dtItems;

                //verifico que no este agregado a la grilla
                foreach (DataRow dr in dt.Rows)
                {
                    
                    if (dr["Codigo"].ToString() == this.txtCodigo.Text)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"El articulo con codigo " + this.txtCodigo.Text.Trim() + " ya se encuentra en la grilla\", {type: \"error\"});", true);
                        return;
                    }
                }

                DataRow drFila = dt.NewRow();

                drFila["Codigo"] = this.txtCodigo.Text;
                drFila["Descripcion"] = "";
                var art = this.contArticulos.obtenerArticuloCodigo(this.txtCodigo.Text.Trim());
                if (art != null)
                {
                    drFila["Descripcion"] = art.descripcion;
                }
                drFila["Cant"] = this.txtCantidad.Text;

                dt.Rows.Add(drFila);

                this.dtItems = dt;

                this.agregarItemATabla(drFila["Codigo"].ToString(), drFila["Descripcion"].ToString(), Convert.ToDecimal(drFila["Cant"]));
                //this.CargarItems();
                //limpio los campos
                this.txtCodigo.Text = "";
                this.txtCantidad.Text = "";
                //this.txtDescripcion.Text = "";
                this.txtCantidad.Focus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                RemitosCompra rc = new RemitosCompra();
                string codigosIncorrectos = "";

                if ((this.phProductos.Controls.Count - rc.RemitosCompras_Items.Count) > 0)
                {
                    codigosIncorrectos = this.obtenerArticulosIncorrectos();
                }

                //if (this.phProductos.Controls.Count != 0)
                //{
                    if(!string.IsNullOrEmpty(codigosIncorrectos))
                    {
                        lblCodigosIncorrectos.Text = codigosIncorrectos.Substring(0, codigosIncorrectos.Length - 2);
                        msjAlerta.Visible = true;
                        lblAlerta.Text = "Los artículos listados no serán cargados en el sistema.";
                    }
                    else
                    {
                        lblCodigosIncorrectos.Text = "";
                        msjAlerta.Visible = false;
                        lblAlerta.Text = "";
                    }
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "alert", "abrirdialog()", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar al menos un item para agregar. \", {type: \"error\"});", true);
                //}
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrió un error al generar el Remito. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        protected void btnConfirmarRemito_Click(object sender, EventArgs e)
        {
            try
            {
                this.guardarRemito();
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrió un error al generar el Remito. Excepción: " + Ex.Message + " \", {type: \"error\"});", true);
            }
        }
        #endregion


    }
}