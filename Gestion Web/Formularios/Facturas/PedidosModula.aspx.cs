using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class PedidosModula : System.Web.UI.Page
    {
        ControladorPedido controlador = new ControladorPedido();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPedidoEntity contPedEntity = new ControladorPedidoEntity();
        controladorArticulo contArticulos = new controladorArticulo();

        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private string fechaEntregaD;
        private string fechaEntregaH;
        private int idCliente;
        private int idEstado;
        int idVendedor;
        int tipoEntrega;
        int tipoFecha;
        int tipoFecha2;
        int idArticulo;

        int accion;
        string numero = string.Empty;
        string cliente = string.Empty;
        //para el de la lista
        int Vendedor;

        DataTable dtPedidosTemp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                estadoPedido est = controlador.obtenerEstadoDesc("Pendiente");
                
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idEstado = Convert.ToInt32(Request.QueryString["estado"]);
                Vendedor = Convert.ToInt32(Request.QueryString["V"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.tipoEntrega = Convert.ToInt32(Request.QueryString["te"]);
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                this.tipoFecha2 = Convert.ToInt32(Request.QueryString["tf2"]);
                this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);

                this.numero = Request.QueryString["n"];
                this.cliente = Request.QueryString["c"];

                if (!IsPostBack)
                {
                    dtPedidosTemp = new DataTable();
                    if (fechaD == null && fechaH == null && suc == 0)
                    {
                        idEstado = est.id;
                        string perfil2 = Session["Login_NombrePerfil"] as string;
                        if (perfil2 == "Cliente")
                        {
                            idEstado = 0;
                        }
                        suc = (int)Session["Login_SucUser"];
                        fechaD = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaEntregaD = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
                        fechaEntregaH = DateTime.Now.ToString("dd/MM/yyyy");
                        this.tipoFecha = 1;
                    }

                    //txtFechaDesde.Text = fechaD;
                    //txtFechaHasta.Text = fechaH;
                    //txtFechaDesdeCantidad.Text = fechaD;
                    //txtFechaHastaCantidad.Text = fechaH;
                    //txtFechaEntregaDesde.Text = fechaD;
                    //txtFechaEntregaHasta.Text = fechaH;
                    //DropListSucursal.SelectedValue = suc.ToString();
                    //DropListClientes.SelectedValue = idCliente.ToString();
                    //ListVendedor.SelectedValue = Vendedor.ToString();
                    //DropListEstado.SelectedValue = idEstado.ToString();
                    //ListTipoEntrega.SelectedValue = this.tipoEntrega.ToString();
                    //this.RadioFechaPedido.Checked = Convert.ToBoolean(this.tipoFecha);
                    //this.RadioFechaEntrega.Checked = Convert.ToBoolean(this.tipoFecha2);
                }

                dtPedidosTemp = new DataTable();
                if (fechaD == null && fechaH == null && suc == 0)
                {
                    idEstado = est.id;
                    string perfil2 = Session["Login_NombrePerfil"] as string;
                    if (perfil2 == "Cliente")
                    {
                        idEstado = 0;
                    }
                    suc = (int)Session["Login_SucUser"];
                    fechaD = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaEntregaD = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
                    fechaEntregaH = DateTime.Now.ToString("dd/MM/yyyy");
                    this.tipoFecha = 1;
                }

                string perfil = Session["Login_NombrePerfil"] as string;
                //por defecto
                if (accion == 0)
                {
                    //if (perfil == "Vendedor")
                    //{
                    //    //deshabilito el list de vendedor
                    //    this.ListVendedor.Attributes.Add("disabled", "true");
                    //    this.ListVendedor.SelectedValue = this.idVendedor.ToString();
                    //    this.Vendedor = idVendedor;
                    //}
                    //if (perfil == "Cliente")
                    //{
                    //    //deshabilito el list de vendedor
                    //    this.ListVendedor.Attributes.Add("disabled", "true");
                    //    //para asegurarme de cargar por defecto los pedidos del cliente
                    //    this.cargarPedidosRango(fechaD, fechaH, suc, this.idVendedor, idEstado, this.Vendedor);
                    //}
                    //else
                    //{
                        this.cargarPedidosRango(fechaD, fechaH, suc, idCliente, idEstado, this.Vendedor);
                    //}

                    //if (idCliente > 0)
                    //{
                    //    //this.lbtnFacturar.Enabled = true;
                    //    //this.btnAccion.Disabled = false;
                    //    this.lbtnFacturar.Visible = true;
                    //    //this.lbtnRemitir.Visible = true;
                    //}
                }
                //busca por pedido
                //if (accion == 1)
                //{
                //    this.buscarPorNumero();
                //    this.lbtnFacturar.Visible = true;
                //}
                ////busca por numero
                //if (accion == 2)
                //{
                //    this.buscarPorCliente();
                //    this.lbtnFacturar.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        public DataTable dtPedidos
        {

            get
            {
                if (ViewState["Pedidos"] != null)
                {
                    return (DataTable)ViewState["Pedidos"];
                }
                else
                {
                    return dtPedidosTemp;
                }
            }
            set
            {
                ViewState["Pedidos"] = value;
            }
        }

        private void cargarPedidosRango(string fechaD, string fechaH, int idSuc, int idCliente, int idEstado, int vendedor)
        {
            try
            {
                #region old
                //selecciono un cliente y una sucursal
                //if (fechaD != null && fechaD != null && idCliente != 0 && idSuc != 0 && idEstado != 0 && vendedor != 0)
                //{
                //    List<Pedido> p = this.controlador.obtenerPedidosRango(fechaD, fechaH,fechaD, fechaH, idSuc, idCliente, idEstado, vendedor,this.tipoEntrega, this.tipoFecha, this.tipoFecha2);
                //    this.cargarLista(p);
                //    this.cargarLabel(fechaD, fechaH, idSuc, idCliente, idEstado);
                //}
                //else
                //{
                //    //List<Pedido> p = this.controlador.obtenerPedidosRango(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue));
                //    List<Pedido> p = this.controlador.obtenerPedidosRango(this.txtFechaDesde.Text, this.txtFechaHasta.Text, this.txtFechaEntregaDesde.Text, this.txtFechaEntregaHasta.Text,
                //        Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue),
                //        Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue), Convert.ToInt32(this.ListTipoEntrega.SelectedValue),
                //        Convert.ToInt32(this.RadioFechaPedido.Checked),Convert.ToInt32(this.RadioFechaEntrega.Checked));
                //    this.cargarLista(p);
                //    this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue));
                //}
                #endregion

                if (fechaD != null && fechaD != null && idCliente != 0 && idSuc != 0 && idEstado != 0 && vendedor != 0 && idArticulo != 0)
                {
                    DataTable dt = this.controlador.obtenerPedidosRangoDT(fechaD, fechaH, fechaD, fechaH, idSuc, idCliente, idEstado, vendedor, this.tipoEntrega, this.tipoFecha, this.tipoFecha2);
                    this.cargarPedidos(dt);
                }
                else
                {
                    DataTable dt = this.controlador.obtenerPedidosRangoDT(fechaD, fechaH, fechaD, fechaH, idSuc, idCliente, idEstado, vendedor, this.tipoEntrega, this.tipoFecha, this.tipoFecha2);
                    this.cargarPedidos(dt);
                    //DataTable dt = this.controlador.obtenerPedidosRangoDT(this.txtFechaDesde.Text, this.txtFechaHasta.Text, this.txtFechaEntregaDesde.Text, this.txtFechaEntregaHasta.Text,
                    //    Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue),
                    //    Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue), Convert.ToInt32(this.ListTipoEntrega.SelectedValue),
                    //    Convert.ToInt32(this.RadioFechaPedido.Checked), Convert.ToInt32(this.RadioFechaEntrega.Checked));
                    //this.cargarPedidos(dt);

                }

                }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        
        }
        private void cargarPedidos(DataTable dtPedidos)
        {
            try
            {               
                foreach (DataRow row in dtPedidos.Rows)
                {  
                    this.cargarEnPhDR(row);
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Pedidos. " + ex.Message));

            }
        }

        private void cargarEnPhDR(DataRow p)
        {
            try
            {
                LinkButton btnImprimir = new LinkButton();
                //btnImprimir.CssClass = "shortcut";
                //btnImprimir.Attributes.Add("data-toggle", "tooltip");
                btnImprimir.Attributes.Add("class", "shortcut");
                btnImprimir.ID = "btnSelec_" + p["id"].ToString();
                //btnImprimir.Text = "<a href=\"#\" class=\"shortcut\"><i class=\"shortcut - icon icon - bookmark\"></i> <span class=\"shortcut - label\">0004-000007</span></a>";
                btnImprimir.Text = "<i class=\"shortcut-icon icon-file-text\"></i> <span class=\"shortcut-label\">" + p["numero"].ToString().PadLeft(8, '0') + "</span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //btnImprimir.Font.Size = 12;
                btnImprimir.Click += new EventHandler(this.mandarModula);

                this.phPedidos.Controls.Add(btnImprimir);
                ////fila
                //TableRow tr = new TableRow();
                //tr.ID = p["id"].ToString();

                ////Celdas

                //TableCell celFecha = new TableCell();
                //celFecha.Text = Convert.ToDateTime(p["fecha"]).ToString("dd/MM/yyyy");
                //celFecha.HorizontalAlign = HorizontalAlign.Left;
                //celFecha.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celFecha);

                //TableCell celNumero = new TableCell();
                //celNumero.Text = p["numero"].ToString().PadLeft(8, '0');
                //celNumero.HorizontalAlign = HorizontalAlign.Left;
                //celNumero.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celNumero);

                //TableCell celRazon = new TableCell();
                //celRazon.Text = p["razonSocial"].ToString();
                //celRazon.HorizontalAlign = HorizontalAlign.Left;
                //celRazon.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celRazon);


                //TableCell celTotal = new TableCell();
                //celTotal.Text = Convert.ToDecimal(p["total"]).ToString("C");
                //celTotal.VerticalAlign = VerticalAlign.Middle;
                //celTotal.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celTotal);

                //TableCell celTipo = new TableCell();
                //var estado = this.controlador.obtenerEstadoID(Convert.ToInt32(p["estado"]));
                //celTipo.Text = estado.descripcion;
                //celTipo.HorizontalAlign = HorizontalAlign.Left;
                //celTipo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celTipo);

                ////arego fila a tabla

                //TableCell celAccion = new TableCell();

                //LinkButton btnImprimir = new LinkButton();
                //btnImprimir.CssClass = "btn btn-info ui-tooltip";
                //btnImprimir.Attributes.Add("data-toggle", "tooltip");
                //btnImprimir.Attributes.Add("title data-original-title", "Detalles");
                //btnImprimir.ID = "btnSelec_" + p["id"].ToString();
                //btnImprimir.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                ////btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //btnImprimir.Font.Size = 12;
                //btnImprimir.Click += new EventHandler(this.detallePedido);
                //celAccion.Controls.Add(btnImprimir);
                //celAccion.Width = Unit.Percentage(15);
                //celAccion.VerticalAlign = VerticalAlign.Middle;

                //Literal l3 = new Literal();
                //l3.Text = "&nbsp";
                //celAccion.Controls.Add(l3);

                //CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.ID = "cbSeleccion_" + p["id"].ToString();//_id
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;
                //celAccion.Controls.Add(cbSeleccion);

                //Literal l2 = new Literal();
                //l2.Text = "&nbsp";
                //celAccion.Controls.Add(l2);

                //Literal lDetail = new Literal();
                //lDetail.ID = "btnEditar_" + p["id"].ToString();
                //lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" >";
                //lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                //lDetail.Text += "</a>";
                //if (this.verificarPermisoEditar() > 0)
                //{
                //    celAccion.Controls.Add(lDetail);
                //}

                //tr.Cells.Add(celAccion);

                //phPedidos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando pedido. " + ex.Message));
            }

        }

        private void mandarModula(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idPedido = atributos[1];

                try
                {
                    if (!String.IsNullOrEmpty(idPedido))
                    {
                        int i = this.controlador.generarPedidosModula(idPedido);
                        if (i > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedidos enviados a modula con exito. ", "PedidosModula.aspx"));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar pedidos a modula"));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un pedido"));
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Pedido Enviado a modula", "PedidosModula.aspx"));
                }
                catch { }

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + idPedido + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog(" + idPedido + ")", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

    }
}