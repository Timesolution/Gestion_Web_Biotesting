using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class PedidoDetallesF : System.Web.UI.Page
    {
        ControladorPedido controlador = new ControladorPedido();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPedidoEntity contPedEntity = new ControladorPedidoEntity();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        int accion;

        private int idCliente;
        private int idEstado;
        private int vendedor;

        int idVendedor;
        string observacion = string.Empty;
        int idArticulo;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                // todo el codigo lo saque de facturas/cotizacionesC
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idEstado = Convert.ToInt32(Request.QueryString["estado"]);
                vendedor = Convert.ToInt32(Request.QueryString["V"]);
                idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                accion = Convert.ToInt32(Request.QueryString["a"]);

                if (!IsPostBack)
                {
                    this.cargarClientes();
                    this.cargarEstados();
                    this.cargarArticulos();


                    if (fechaD == null && fechaH == null)
                    {
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListEstado.SelectedValue = idEstado.ToString();
                    DropListClientes.SelectedValue = idCliente.ToString();
                }
                
                this.cargarCotizacionesRango(fechaD, fechaH);

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
                        if (s == "36")
                        {
                            valor = 1;
                        }
                        //Permiso ver saldo
                        if (s == "119")
                            this.lblSaldo.Visible = true;
                    }
                }

                //if (listPermisos.Contains("213"))
                //    lbtnGenPedido.Visible = true;

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarArticulos()
        {
            try
            {
                controladorArticulo contArticulo = new controladorArticulo();

                DataTable dt = contArticulo.obtenerArticulosReducido();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListArticulos.DataSource = dt;
                this.DropListArticulos.DataValueField = "id";
                this.DropListArticulos.DataTextField = "descripcion";

                this.DropListArticulos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + ex.Message));
            }
        }

        public void cargarClientes()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;

                controladorCliente contCliente = new controladorCliente();
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

                DataTable dt = new DataTable();
                bool auxiliar = false;

                if (perfil == "Vendedor")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);
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
                if (perfil == "Cliente")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                    this.btnBuscarCod.Visible = false;
                    this.txtCodCliente.Attributes.Add("disabled", "true");
                    //Oculto el Saldo en caso de que el logueado sea un Cliente
                    //this.phSaldo.Visible = false;
                }
                if (perfil == "Distribuidor")
                {
                    auxiliar = true;
                    dt = contClienteEnt.obtenerReferidosDeCliente(this.idVendedor);
                    DataRow dr = dt.NewRow();
                    Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                    dr["alias"] = "Distribuidor";
                    if (c != null)
                    {
                        dr["alias"] = c.razonSocial;
                    }

                    dr["id"] = this.idVendedor;
                    dt.Rows.InsertAt(dr, 0);
                }
                if (auxiliar == false)
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        public void cargarEstados()
        {
            try
            {

                DataTable dt = controlador.obtenerEstadosPedidos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListEstado.DataSource = dt;
                this.DropListEstado.DataValueField = "id";
                this.DropListEstado.DataTextField = "descripcion";

                this.DropListEstado.DataBind();



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Estados de Pedidos a la lista. " + ex.Message));
            }
        }

        private void cargarCotizacionesRango(string fechaD, string fechaH)
        {
            try
            {
                // if (fechaD != null && fechaD != null && idCliente != 0 && idSuc != 0 && idEstado != 0 && vendedor != 0)
                //if (fechaD != null && fechaD != null)
                //{
                DataTable dt = this.controlador.obtenerCotizacionesRangoDT1(fechaD, fechaH, idCliente, idArticulo, idEstado);
                this.cargarCotizacion(dt);
                //this.cargarLabel(fechaD, fechaH, idSuc, idCliente, idEstado);
                //}
                //else
                //{
                //    DataTable dt = this.controlador.obtenerCotizacionesRangoDT(this.txtFechaDesde.Text, this.txtFechaHasta.Text,
                //        Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue),
                //        Convert.ToInt32(this.DropListEstado.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue));
                //    this.cargarCotizacion(dt);
                //    //this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListEstado.SelectedValue));
                //}

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cotizaciones rango. " + ex.Message));
            }
        }

        private void cargarCotizacion(DataTable dtCotizaciones)
        {
            try
            {
                decimal saldo = 0;
                foreach (DataRow row in dtCotizaciones.Rows)
                {
                    saldo += Convert.ToDecimal(row["total"]);
                    this.cargarEnPhDR(row);
                }

                lblSaldo.Text = saldo.ToString("C");

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrio un error cargando cotizaciones. Ubicacion: PedidosDetalleF.cargarCotizacion. Excepcion:" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }

        private void cargarEnPhDR(DataRow p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p["fecha"]).ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p["numero"].ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p["razonSocial"].ToString();
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = p["codigo"].ToString();
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = p["descripcion"].ToString();
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                //TableCell celTotal = new TableCell();
                ////si es cliente lo muestro sin iva para Team
                //string perfil2 = Session["Login_NombrePerfil"] as string;
                //if (perfil2 == "Cliente")
                //{
                //    decimal total = Convert.ToDecimal(p["total"]);
                //    celTotal.Text = "$" + Decimal.Round((total / (decimal)1.21), 2).ToString();
                //}
                //else
                //{
                //    celTotal.Text = Convert.ToDecimal(p["total"]).ToString("C");
                //}
                //celTotal.VerticalAlign = VerticalAlign.Middle;
                //celTotal.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                var estado = this.controlador.obtenerEstadoID(Convert.ToInt32(p["estado"]));
                celTipo.Text = estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                //arego fila a tabla

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
                ////if (this.verificarPermisoEditar() > 0)
                ////{
                //celAccion.Controls.Add(lDetail);
                ////}

                //if (estado.id != 3)
                //{
                //    tr.Cells.Add(celAccion);
                //}
                //else
                //{
                //    TableCell celEstado = new TableCell();
                //    celEstado.Text = "*Anulada*";
                //    celEstado.VerticalAlign = VerticalAlign.Middle;
                //    celEstado.HorizontalAlign = HorizontalAlign.Center;
                //    tr.Cells.Add(celEstado);
                //    tr.ForeColor = System.Drawing.Color.Red;
                //}

                phCotizaciones.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cotizacion en ph dr. " + ex.Message));
            }

        }

        private void cargarEnPh(Pedido p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = p.fecha.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = p.numero.ToString().PadLeft(8, '0');
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = p.cliente.razonSocial;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);


                TableCell celTotal = new TableCell();
                //si es cliente lo muestro sin iva para Team
                string perfil2 = Session["Login_NombrePerfil"] as string;
                if (perfil2 == "Cliente")
                {
                    celTotal.Text = "$" + Decimal.Round((p.total / (decimal)1.21), 2).ToString();
                }
                else
                {
                    celTotal.Text = "$" + p.total;
                }
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                TableCell celTipo = new TableCell();
                celTipo.Text = p.estado.descripcion;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Detalles");
                btnEliminar.ID = "btnSelec_" + p.id;
                btnEliminar.Text = "<span class='shortcut-icon fa fa-search-plus'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnEliminar.Font.Size = 12;
                btnEliminar.Click += new EventHandler(this.detallePedido);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + p.id;//_id
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + p.id.ToString();
                lDetail.Text = "<a href=\"ABMPedidos.aspx?accion=2&id=" + p.id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" >";
                lDetail.Text += "<i class=\"shortcut-icon icon-pencil\"></i>";
                lDetail.Text += "</a>";

                //if (this.verificarPermisoEditar() > 0)
                //{
                    celAccion.Controls.Add(lDetail);
                //}
                tr.Cells.Add(celAccion);

                phCotizaciones.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando cotizacion. " + ex.Message));
            }

        }
        private void detallePedido(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idPedido = atributos[1];

                try
                {//Si es un pedido pendiente de vendedor, lo cambio a pendiente la primera vez que lo imprimo
                    Pedido ped = this.controlador.obtenerPedidoId(Convert.ToInt32(idPedido));
                    if (ped.estado.id == 5)
                    {
                        int i = this.contPedEntity.cambiarEstadoPedido(ped.id, 1);
                        if (i > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Cambio estado pendiente vendedor a pendiente pedido id: " + ped.id);
                        }
                    }
                }
                catch { }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "window.open('ImpresionPedido.aspx?co=1&a=1&Pedido=" + idPedido + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog(" + idPedido + ")", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    
                        Response.Redirect("PedidoDetallesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Cliente=" + DropListClientes.SelectedValue
                            + "&estado=" + DropListEstado.SelectedValue + "&art=" + DropListArticulos.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de detallle de cotizaciones. " + ex.Message));

            }
        }

        private void buscar(string cot)
        {
            try
            {
                List<Cotizaciones> facturas = new List<Cotizaciones>();
                string query = "select * from articulos where descripcion like '%" + cot + "%'";
                //facturas = this.controlador.buscarArticuloList(query);
                //this.cargarArticulosTabla(articulos);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void UpdatePanel3_Load(object sender, EventArgs e)
        {
            //
            this.labelIva.Text = "testc escribo desde el update panel amigo!!";


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        //protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.obtenerVendedor(Convert.ToInt32(this.DropListClientes.SelectedValue));
        //    }
        //    catch
        //    {

        //    }
        //}

        //obtengo vendedor del cliente
        private void obtenerVendedor(int Cliente)
        {
            try
            {
                controladorCliente contCl = new controladorCliente();
                var cl = contCl.obtenerClienteID(Cliente);
                //this.ListVendedor.SelectedValue = cl.vendedor.id.ToString();
            }
            catch
            {

            }
        }

        //boton buscar en el cliente
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

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
                //controladorCliente contrCliente = new controladorCliente();
                //Cliente cl = contrCliente.obtenerClienteCodigo(txtCodCliente.Text);

                //if (cl != null)
                //{
                //    this.DropListClientes.SelectedValue = cl.id.ToString();
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se encontro cliente con el codigo ingresado\", {type: \"info\"});", true);
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        private void buscarPorObservacion()
        {
            try
            {
                DataTable dtCotizaciones = this.controlador.ObtenerDataTablePedidosPorObservacion(this.observacion, 10); //10 es el tipo de documento pedido
                if (dtCotizaciones.Rows.Count > 0)
                {
                    this.cargarCotizacion(dtCotizaciones);
                }
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Busqueda Completada", "Se han encontrado " + dtCotizaciones.Rows.Count.ToString() + " cotizaciones."));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrio un error buscando cotizacion por observacion. Ubicacion: PedidosDetalleF.buscarPorObservacion. Excepcion:" + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor contacte con el area de soporte via WhatsApp (+54 9 11 3782-0435) para informarnos sobre este problema."));
            }
        }
        protected void btnBuscarNumeros_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                Response.Redirect("CotizacionesC.aspx?a=1&o=" + this.txtObservacion.Text);
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phCotizaciones.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[5].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        //idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    int i = this.controlador.anularPedidos(idtildado);
                    if (i > 0)
                    {
                        Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "ANULACION Pedido id: " + idtildado);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cotizaciones anuladas con exito. ", "CotizacionesC.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Cotizaciones. "));

                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una cotizacion"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando cotizaciones para facturar. " + ex.Message));
            }
        }

        protected void btnBuscarCodArt_Click(object sender, EventArgs e)
        {
            controladorArticulo contArticulo = new controladorArticulo();

            String buscar = this.txtDescArticulo.Text.Replace(' ', '%');
            DataTable dt = contArticulo.obtenerArticulosByDescDT(buscar);

            //cargo la lista
            this.DropListArticulos.DataSource = dt;
            this.DropListArticulos.DataValueField = "id";
            this.DropListArticulos.DataTextField = "descripcion";
            this.DropListArticulos.DataBind();
        }
    }
}
