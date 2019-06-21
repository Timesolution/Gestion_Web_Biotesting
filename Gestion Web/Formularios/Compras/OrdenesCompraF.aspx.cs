using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
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
        private int estadoGeneral;

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
            estadoGeneral = Convert.ToInt32(Request.QueryString["eg"]);
            filtroPorFecha = Convert.ToInt32(Request.QueryString["fpf"]);
            filtroPorFechaEntrega = Convert.ToInt32(Request.QueryString["fpfe"]);

            Page.Form.DefaultButton = btnBuscarCodigoProveedor.UniqueID;

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
                    estadoGeneral = 0;
                    filtroPorFecha = 1;
                    filtroPorFechaEntrega = 0;
                    fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    this.btnAccion.Visible = true;
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtFechaEntregaDesde.Text = fechaEntregaD;
                    txtFechaEntregaHasta.Text = fechaEntregaH;
                    DropListProveedor.SelectedValue = proveedor.ToString();
                    DropListEstadoFiltro.SelectedValue = estado.ToString();
                    DropListEstadoGeneralFiltro.SelectedValue = estadoGeneral.ToString();
                }                

                if(proveedor > 0)
                    lbtnEntregasPH.Visible = true;              

                this.cargarEstadosFiltro();
                //this.cargarEstados();
                this.cargarSucursal();
                cargarEstadosGeneralesFiltro();

                //this.buscar(fechaD, fechaH, proveedor, sucursal, estado, fechaEntregaD, fechaEntregaH, filtroPorFecha, filtroPorFechaEntrega, estadoGeneral);
                //txtFechaDesde.Text = fechaD;
                //txtFechaHasta.Text = fechaH;                
            }

            if (fechaD != null && fechaH != null)
            {
                this.buscar(fechaD, fechaH, proveedor, sucursal,estado,fechaEntregaD,fechaEntregaH,filtroPorFecha,filtroPorFechaEntrega,estadoGeneral);
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

                        //if (s == "178")
                        //    ltbnCambiarEstado.Visible = true;
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
                var estados = this.contCompraEntity.obtenerOrdenesCompra_Estados();

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados filtros. " + ex.Message));
            }
        }
        public void cargarEstadosGeneralesFiltro()
        {
            try
            {
                var estados = this.contCompraEntity.obtenerOrdenesCompra_EstadosGenerales();

                estados.Insert(0, new Gestion_Api.Entitys.OrdenesCompra_Estados
                {
                    Id = 0,
                    TipoEstado = "Todos"
                });

                //agrego todos

                this.DropListEstadoGeneralFiltro.DataSource = estados;
                this.DropListEstadoGeneralFiltro.DataValueField = "Id";
                this.DropListEstadoGeneralFiltro.DataTextField = "TipoEstado";
                this.DropListEstadoGeneralFiltro.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados generales. " + ex.Message));
            }
        }
        //public void cargarEstados()
        //{
        //    try
        //    {
        //        var estados = contCompraEntity.obtenerOrdenesCompra_Estados();                

        //        //agrego todos
        //        this.DropListEstados.DataSource = estados;
        //        this.DropListEstados.DataValueField = "Id";
        //        this.DropListEstados.DataTextField = "TipoEstado";
        //        this.DropListEstados.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando estados. " + ex.Message));
        //    }
        //}
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 0);

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
        private void buscar(string fDesde, string fHasta, int proveedor,int idSucursal,int estado, string fEntregaD,string fEntregaH,int filtroPorFecha, int filtroPorFechaEntrega, int estadoGeneral)
        {
            try
            {
                DateTime desde = Convert.ToDateTime(fDesde, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(fHasta, new CultureInfo("es-AR"));
                DateTime entregaD = Convert.ToDateTime(fEntregaD, new CultureInfo("es-AR"));
                DateTime entregaH = Convert.ToDateTime(fEntregaH, new CultureInfo("es-AR"));

                lbtnExportarExcel.Visible = true;
                //int estado = Convert.ToInt32(DropListEstado.SelectedValue);

                List<Gestion_Api.Entitys.OrdenesCompra> ordenes = this.contCompraEntity.buscarOrden(desde, hasta, proveedor, idSucursal, estado, entregaD, entregaH, filtroPorFecha, filtroPorFechaEntrega, estadoGeneral);

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
                var ordenCompraEstadoGeneral = this.contCompraEntity.obtenerOrdenCompra_Estado_PorId((int)oc.EstadoGeneral);

                //fila
                TableRow tr = new TableRow();
                tr.ID = oc.Id.ToString();
                if (oc.Estado == 9)                
                    tr.ForeColor = System.Drawing.Color.Green;
                else if(oc.Estado == 4)
                    tr.ForeColor = System.Drawing.Color.Gold;
                else if (oc.Estado == 5)
                    tr.ForeColor = System.Drawing.Color.Red;

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(oc.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celFechaEntrega = new TableCell();
                celFechaEntrega.Text = Convert.ToDateTime(oc.FechaEntrega, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFechaEntrega.VerticalAlign = VerticalAlign.Middle;
                celFechaEntrega.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaEntrega);

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

                TableCell celEstadoGeneral = new TableCell();
                celEstadoGeneral.Text = ordenCompraEstadoGeneral.TipoEstado;
                celEstadoGeneral.VerticalAlign = VerticalAlign.Middle;
                celEstadoGeneral.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEstadoGeneral);

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
                cbSeleccion.Attributes.Add("name", "checkbox");
                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Controls.Add(btnEliminar);

                if(oc.Estado == 1 && oc.EstadoGeneral != 12)
                {
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
                }                

                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phOrdenes.Controls.Add(tr);
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
                        Response.Redirect("OrdenesCompraF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" +this.DropListSucursal.SelectedValue + "&e=" + this.DropListEstadoFiltro.SelectedValue + "&fed=" + txtFechaEntregaDesde.Text + "&feh=" + txtFechaEntregaHasta.Text + "&fpf=" + Convert.ToInt32(RadioFechaOrdenCompra.Checked) + "&fpfe=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&eg=" + this.DropListEstadoGeneralFiltro.SelectedValue);
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
                    CheckBox ch = tr.Cells[6].Controls[2] as CheckBox;
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

        //private void modificarEstadoOrdenCompra(int estado)
        //{
        //    try
        //    {
        //        string idtildado = string.Empty;
        //        foreach (Control C in phOrdenes.Controls)
        //        {
        //            TableRow tr = C as TableRow;
        //            CheckBox ch = tr.Cells[6].Controls[2] as CheckBox;
        //            if (ch.Checked == true)
        //            {
        //                idtildado = ch.ID.Split('_')[1];
        //            }
        //        }
        //        if (!String.IsNullOrEmpty(idtildado))
        //        {
        //            int i = this.contCompraEntity.modificarEstadoOrdenCompraId(Convert.ToInt64(idtildado), estado);

        //            if (i > 0)
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Estado de la Orden de Compra modificado con éxito. ", "OrdenesCompraF.aspx?fd="+ txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&fed=" + txtFechaEntregaDesde.Text + "&feh=" + txtFechaEntregaHasta.Text + "&fpf=" + Convert.ToInt32(RadioFechaOrdenCompra.Checked) + "&fpfe=" + Convert.ToInt32(RadioFechaEntrega.Checked)));
        //            }
        //            else
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar el estado de la Orden de Compra seleccionada. "));
        //            }
        //        }
        //        else
        //        {
        //            //ScriptManager.RegisterClientScriptBlock(this.GetType(), UpdatePanel4.GetType(), "alert", "$.msgbox(\"Debe filtrar por algún movimiento!" + "\", {type: \"error\"});", true);
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar algún documento!"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando el estado de una Orden de Compra. Excepción: " + Ex.Message));
        //    }
        //}

        protected void ltbnConsolidados_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                int estadoItems = 1;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    if (rbtnItemsPendientes.Checked)
                        estadoItems = 2;
                    else if (rbtnItemsRecibidos.Checked)
                        estadoItems = 4;
                    else
                        estadoItems = 0;

                    Response.Redirect("ImpresionCompras.aspx?a=12&ex=1&" + "ordenesCompra=" + idtildado + "&estadoItems=" + estadoItems);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una orden de compra"));
                }
            }
            catch
            {

            }

        }

        protected void lbtnConsolidadosPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                int estadoItems = 0;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    if (rbtnItemsPendientes.Checked)
                        estadoItems = 2;
                    else if (rbtnItemsRecibidos.Checked)
                        estadoItems = 1;
                    else
                        estadoItems = 0;

                    Response.Redirect("ImpresionCompras.aspx?a=12&ex=0&" + "ordenesCompra=" + idtildado + "&estadoItems=" + estadoItems);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una orden de compra"));
                }
            }
            catch
            {

            }
        }

        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                List<long> ids = new List<long>();
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                        ids.Add(Convert.ToInt64(idtildado));
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.contCompraEntity.ModificarEstadoOrdenesCompraId(ids, 3);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Estado de la Orden de Compra modificado con éxito. ", "OrdenesCompraF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&fed=" + txtFechaEntregaDesde.Text + "&feh=" + txtFechaEntregaHasta.Text + "&fpf=" + Convert.ToInt32(RadioFechaOrdenCompra.Checked) + "&fpfe=" + Convert.ToInt32(RadioFechaEntrega.Checked)));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar el estado de la Orden de Compra seleccionada. "));
                    }
                    //modificarEstadoOrdenCompra(3);
                }
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar algún documento!"));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cambiando estado orden de compra. " + ex.Message);
            }
        }

        private void enviarMail(OrdenesCompra oc) 
        {
            try
            {
                controladorFunciones contFunciones = new controladorFunciones();
                ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
                //string destinatarios = this.lblMailOC.Text;

                var prov = contClienteEntity.obtenerProveedor_OC_PorProveedor((int)oc.IdProveedor);
                
                if (!String.IsNullOrEmpty(prov.Mail))
                {
                    String pathArchivoGenerar = Server.MapPath("../../OrdenesCompra/" + oc.Id + "/" + "/oc-" + oc.Numero + "_" + oc.Id + ".pdf");
                    string pathDirectorio = Server.MapPath("../../OrdenesCompra/" + oc.Id + "/");

                    //Si el directorio no existe, lo creo
                    if (!Directory.Exists(pathDirectorio))
                    {
                        Directory.CreateDirectory(pathDirectorio);
                    }

                    int i = this.generarOrdenCompraPDF(oc, pathArchivoGenerar);
                    if (i > 0)
                    {
                        Attachment adjunto = new Attachment(pathArchivoGenerar);

                        int ok = contFunciones.enviarMailOrdenesCompra(adjunto, oc, prov.Mail);
                        if (ok > 0)
                        {
                            adjunto.Dispose();
                            File.Delete(pathArchivoGenerar);
                            Directory.Delete(pathDirectorio);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de Compra enviada correctamente!", ""));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar la Orden de Compra por mail. "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar impresion Orden de Compra a enviar. "));
                    }
                }

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando mail. Excepción: " + Ex.Message));
            }
        }

        private int generarOrdenCompraPDF(OrdenesCompra ordenCompra, string pathGenerar)
        {
            try
            { 
                controladorCliente cont = new controladorCliente();
                controladorSucursal contSuc = new controladorSucursal();
                ControladorEmpresa controlEmpresa = new ControladorEmpresa();


                //Gestion_Api.Entitys.OrdenesCompra ordenCompra = this.contCompraEntity.obtenerOrden(this.ordenCompra);
                Gestor_Solution.Modelo.Cliente p = cont.obtenerProveedorID(ordenCompra.IdProveedor.Value);

                Sucursal s = contSuc.obtenerSucursalID(ordenCompra.IdSucursal.Value);

                //datos empresa emisora
                DataTable dtEmpresa = controlEmpresa.obtenerEmpresaById(s.empresa.id);

                String razonSoc = String.Empty;
                String direComer = String.Empty;
                String condIVA = String.Empty;

                String Fecha = " ";
                String FechaEntrega = " ";
                String Numero = " ";
                String Proveedor = " ";
                String Observacion = "-";

                foreach (DataRow row in dtEmpresa.Rows)//Datos empresa 
                {
                    razonSoc = row["Razon Social"].ToString();
                    condIVA = row["Condicion IVA"].ToString();
                    direComer = row["Direccion"].ToString();
                }

                if (ordenCompra != null && p != null)
                {
                    Fecha = ordenCompra.Fecha.Value.ToString("dd/MM/yyyy");
                    FechaEntrega = ordenCompra.FechaEntrega.Value.ToString("dd/MM/yyyy");
                    Numero = "Nº " + ordenCompra.Numero;
                    Proveedor = p.razonSocial;
                    Observacion = ordenCompra.Observaciones;
                }

                string logo = Server.MapPath("../../Facturas/" + s.empresa.id + "/Logo.jpg");

                List<Gestion_Api.Entitys.OrdenesCompra_Items> itemsOrdenes = ordenCompra.OrdenesCompra_Items.ToList();//obtengo los items de la OC
                DataTable dtItems = ListToDataTable(itemsOrdenes);//Paso la list a datatable para pasarlo al report.
                dtItems.Columns.Add("CodProv");

                foreach (DataRow row in dtItems.Rows)
                {
                    ProveedorArticulo codProv = this.contArticulos.obtenerProveedorArticuloByArticulo(Convert.ToInt32(row["Codigo"]));

                    Articulo art = this.contArticulos.obtenerArticuloByID(Convert.ToInt32(Convert.ToInt32(row["Codigo"])));

                    if (art != null)
                    {
                        row["Codigo"] = art.codigo;
                    }
                    if (codProv != null)
                    {
                        row["CodProv"] = codProv.codigoProveedor;
                    }
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("OrdenesCompraR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource("ItemsOrden", dtItems);
                ReportParameter param1 = new ReportParameter("ParamFecha", Fecha);
                ReportParameter param2 = new ReportParameter("ParamFechaEntrega", FechaEntrega);
                ReportParameter param3 = new ReportParameter("ParamNumero", Numero);
                ReportParameter param4 = new ReportParameter("ParamProveedor", Proveedor);
                ReportParameter param5 = new ReportParameter("ParamObservacion", Observacion);

                ReportParameter param12 = new ReportParameter("ParamRazonSoc", razonSoc);
                ReportParameter param13 = new ReportParameter("ParamDomComer", direComer);
                ReportParameter param14 = new ReportParameter("ParamCondIva", condIVA);

                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param1);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);
                this.ReportViewer1.LocalReport.SetParameters(param5);

                this.ReportViewer1.LocalReport.SetParameters(param12);//datos empresa
                this.ReportViewer1.LocalReport.SetParameters(param13);
                this.ReportViewer1.LocalReport.SetParameters(param14);

                this.ReportViewer1.LocalReport.SetParameters(param32);//logo

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                return 1;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al intentar guardar la Orden de Compra. Excepción: " + Ex.Message));
                return -1;
            }
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

        protected void DropListEstadoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListEstadoFiltro.SelectedItem.Text == "Observado")
                {
                    this.phDropListEstadosItemOC.Visible = true;
                }
                else
                {
                    this.phDropListEstadosItemOC.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: DropListEstadoFiltro_SelectedIndexChanged. Excepción: " + ex.Message));
            }
        }

        protected void lbtnProcesarEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    var oc = contCompraEntity.obtenerOrden(Convert.ToInt64(idtildado));

                    if(oc.Estado == 1)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra no se encuentra autorizada!"));
                    else if (oc.Estado == 5)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra ya fue rechazada!"));
                    else if(oc.Estado == 9)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra ya fue aceptada!"));
                    else
                    {
                        if (oc.EstadoGeneral == 12)
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra se encuentra cerrada!"));
                        else
                            Response.Redirect("EntregasMercaderiaF.aspx?oc=" + idtildado);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando entregas de mercaderia. " + ex.Message);
            }
        }

        protected void ConfirmarRechazarEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    var oc = contCompraEntity.obtenerOrden(Convert.ToInt64(idtildado));

                    int temp = 0;

                    if (oc.Estado == 1)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra no se encuentra autorizada!"));
                    else if (oc.Estado == 5)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra ya fue rechazada!"));
                    else if (oc.Estado == 9)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra ya fue aceptada!"));
                    else
                    {
                        if (oc.EstadoGeneral == 12)
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de compra se encuentra cerrada!"));
                        else
                            temp = contCompraEntity.RechazarOrdenCompra(oc);
                    }                       

                    if(temp > 0)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("La orden de compra fue rechazada correctamente!", "OrdenesCompraF.aspx?fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&p=" + DropListProveedor.SelectedValue + "&suc=" + this.DropListSucursal.SelectedValue + "&e=" + this.DropListEstadoFiltro.SelectedValue + "&fed=" + txtFechaEntregaDesde.Text + "&feh=" + txtFechaEntregaHasta.Text + "&fpf=" + Convert.ToInt32(RadioFechaOrdenCompra.Checked) + "&fpfe=" + Convert.ToInt32(RadioFechaEntrega.Checked) + "&eg=" + this.DropListEstadoGeneralFiltro.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando entregas de mercaderia. " + ex.Message);
            }
        }

        protected void lbtnGenerarFacturaCompra_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = string.Empty;
                int ordenesTildadas = 0;

                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                        ordenesTildadas++;
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    if(ordenesTildadas > 1)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo un documento!"));
                    else
                        Response.Redirect("ComprasABM.aspx?a=4&oc=" + idtildado);
                }
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar algún documento!"));                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando factura de compra desde orden de compra. " + ex.Message);
            }
        }

        protected void btnBuscarCodigoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodProveedor.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);

                if (txtCodProveedor.Text.Trim().ToLower() == "todo" || txtCodProveedor.Text.Trim().ToLower() == "todos")
                {
                    DataRow dr2 = dtClientes.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dtClientes.Rows.InsertAt(dr2, 0);
                }                

                //cargo la lista
                this.DropListProveedor.DataSource = dtClientes;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";
                this.DropListProveedor.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string idOrden = "";
                int contador = 0;
                foreach (Control C in phOrdenes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[7].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idOrden += ch.ID.Split('_')[1];
                        contador++;
                    }
                }
                if (!String.IsNullOrEmpty(idOrden))
                {
                    if(contador == 1)
                        Response.Redirect("ImpresionCompras.aspx?a=3&ex=1&oc=" + idOrden);
                    else if(contador == 0)
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una orden de compra"));
                    else
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una sola orden de compra"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al exportar orden de compra a excel " + ex.Message);
            }
        }
    }
}