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
    public partial class RemitosR : System.Web.UI.Page
    {
        //controladorCotizaciones controlador = new controladorCotizaciones();
        controladorRemitos controlador = new controladorRemitos();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc= Convert.ToInt32(Request.QueryString["Sucursal"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                    }
                    this.cargarSucursal();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();

                }
                    this.cargarRemitosRango(fechaD, fechaH, suc);

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
                        if (s == "38")
                        {
                            valor = 1;
                        }

                        if (s == "118")
                            this.lblSaldo.Visible = true;
                    }
                }

                return valor;
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

        private void cargarRemitosRango(string fechaD, string fechaH, int idSuc)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0)
                {
                    List<Remito> remitos = controlador.obtenerRemitosRango(fechaD, fechaH, idSuc);
                    decimal saldo = 0;
                    foreach (Remito r in remitos)
                    {
                        saldo += r.total;
                        this.cargarEnPh(r);
                    }

                    lblSaldo.Text = saldo.ToString("C");
                }
                else
                {
                    List<Remito> remitos = controlador.obtenerRemitosRango(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue));
                    decimal saldo = 0;
                    foreach (Remito r in remitos)
                    {
                        saldo += r.total;
                        this.cargarEnPh(r);
                    }

                    lblSaldo.Text = "Total: $" + saldo.ToString();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarRemitos()
        {
            try
            {

                List<Remito> Remito = controlador.obtenerRemitos();

                foreach (Remito r in Remito)
                {
                    this.cargarEnPh(r);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void cargarEnPh(Remito r)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = r.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = r.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                //TableCell celTipo = new TableCell();
                //celTipo.Text = r.tipo.tipo;
                //celTipo.HorizontalAlign = HorizontalAlign.Center;
                //celTipo.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celTipo);


                TableCell celNumero = new TableCell();
                celNumero.Text = r.numero.ToString().PadLeft(8, '0');

                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = r.cliente.razonSocial;

                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celRazon);

                //TableCell celNeto = new TableCell();
                //celNeto.Text = "$" + r.netoNGrabado;
                //celNeto.VerticalAlign = VerticalAlign.Middle;
                //celNeto.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celNeto);

                //TableCell celIva = new TableCell();
                //celIva.Text = "$" + r.neto21;
                //celIva.VerticalAlign = VerticalAlign.Middle;
                //celIva.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celIva);


                //TableCell celPercepcion = new TableCell();
                //celPercepcion.Text = "$" + r.retencion;
                //celPercepcion.VerticalAlign = VerticalAlign.Middle;
                //celPercepcion.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(celPercepcion);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + r.total;

                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                //arego fila a tabla

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Detalles");
                btnEliminar.ID = "btnSelec_" + r.id;
                btnEliminar.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnEliminar.Click += new EventHandler(this.detalleRemito);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + r.id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                //LinkButton btnGenerar = new LinkButton();
                //btnGenerar.CssClass = "btn btn-info ui-tooltip";
                //btnGenerar.Attributes.Add("data-toggle", "tooltip");
                //btnGenerar.Attributes.Add("title data-original-title", "Generar Factura");
                ////btnGenerar.ID = "btnSelec_" + c.id;
                //btnGenerar.Text = "<span class='shortcut-icon icon-plus'></span>";
                //btnGenerar.PostBackUrl = "../../Formularios/Facturas/ABMFacturas.aspx?accion=4&id_rem=" + r.id;
                //celAccion.Controls.Add(btnGenerar);

                tr.Cells.Add(celAccion);

                phRemitos.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }

        }

        private void detalleRemito(object sender, EventArgs e)
        {
            //try
            //{
            //    //obtengo numero factura
            //    string idBoton = (sender as LinkButton).ID;

            //    string[] atributos = idBoton.Split('_');
            //    string idRemito = atributos[1];
            //    Remito r = this.controlador.obtenerRemitoId(Convert.ToInt32(idRemito));

            //    foreach (ItemRemito item in r.items)
            //    {
            //        this.agregarItemRemito(item);

            //    }

            //    //totales
            //    this.CargarTotales(r);
            //    //encabezado
            //    this.CargarEncabezado(r);

            //    //(sender as Button).PostBackUrl = "#modalFacturaDetalle";

            //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "function clickButton()  {  document.getElementById('abreDialog').click()  }");
            //    ////ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "function clickButton()  {  alert('hola');  }", true);

            //    //modalFacturaDetalle.Visible = true;
            //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);

            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
            //    Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            //}
            try
            {
                //obtengo numero remito


                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idRemito = atributos[1];


                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=3&Presupuesto=" + idRemito + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de remito desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error mostrando reporte de remito. " + ex.Message);
            }
        }

        private void agregarItemRemito(ItemRemito item)
        {
            try
            {
                //fila
                TableRow tr1 = new TableRow();
                tr1.ID = item.articulo.codigo.ToString();

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = item.articulo.codigo;
                celCodigo.Width = Unit.Percentage(15);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr1.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = item.cantidad.ToString();
                celCantidad.Width = Unit.Percentage(10);
                celCantidad.HorizontalAlign = HorizontalAlign.Center;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr1.Cells.Add(celCantidad);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = item.articulo.descripcion;
                celDescripcion.Width = Unit.Percentage(40);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr1.Cells.Add(celDescripcion);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = "$" + item.articulo.precioVenta.ToString();
                celPrecio.Width = Unit.Percentage(10);
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                tr1.Cells.Add(celPrecio);

                TableCell celDescuento = new TableCell();
                celDescuento.Text = "$" + item.descuento.ToString();
                celDescuento.Width = Unit.Percentage(10);
                celDescuento.VerticalAlign = VerticalAlign.Middle;
                celDescuento.HorizontalAlign = HorizontalAlign.Right;
                tr1.Cells.Add(celDescuento);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + item.total.ToString();
                celTotal.Width = Unit.Percentage(10);
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr1.Cells.Add(celTotal);

                //arego fila a tabla

                //TableCell celAccion = new TableCell();

                //Button btnEliminar = new Button();
                //btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.ID = "btnEliminar" + item.articulo.codigo;
                //btnEliminar.Text = "Borrar";
                //celAccion.Controls.Add(btnEliminar);
                //celAccion.Width = Unit.Percentage(10);
                //celAccion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion);
                //Label l = new Label();
                //l.Text = "estos es una prueba";

                phitems2.Controls.Add(tr1);
                //phitems2.Controls.Add(l);

                //phItems.Controls.Add(tr);

                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }
        }

        private void CargarTotales(Remito r)
        {
            try
            {
                //fila

                labelNeto.Text = "$" + r.netoNGrabado.ToString();
                labelDescuento.Text = "$" + r.descuento.ToString();
                labelSubTotal.Text = "$" + r.subTotal.ToString();
                labelIva.Text = "$" + r.neto21.ToString();
                labelRetencion.Text = "$" + r.retencion.ToString();
                labelTotal.Text = "$" + r.total.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando totales de remitos. " + ex.Message));
            }
        }

        private void CargarEncabezado(Remito r)
        {
            try
            {
                //fila
                labelNombreCliente.Text = r.cliente.razonSocial;
                labelDireccion.Text = "Calle 123";
                labelTipoCuit.Text = r.cliente.iva;
                labelNroCuit.Text = r.cliente.cuit;
                //LabelNroFactura.Text = "";
                labelVendedor.Text = r.vendedor.emp.nombre + " " + r.vendedor.emp.apellido;
                LabelFormaPAgo.Text = r.formaPAgo.forma;

                labelTipoRemito.Text = r.tipo.tipo + " " + r.numero; ;
                labelFechaRemito.Text = r.fecha.ToString("dd/MM/yyyy");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando totales de remito. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("RemitosR.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue);
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de remitos. " + ex.Message));

            }
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {

                string idtildado = "";
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (string id in idtildado.Split(';'))
                    {
                        if (id != "")
                        {
                            int i = this.controlador.AnularRemito(Convert.ToInt32(id));
                            if (i > 0)
                            {
                                Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "ANULACION remito vta id: " + id);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Remitos anulados con exito. ", "RemitosR.aspx"));
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando Remitos. "));
                            }
                        }                    
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Remito"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando pedidos para anular. " + ex.Message));
            }
        }

        protected void lbtnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[2] as CheckBox;
                    if (ch.Checked == true)
                    {
                        idtildado = ch.ID.Split('_')[1];
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    Response.Redirect("ABMFacturas.aspx?accion=4&id_rem=" + idtildado);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un Remito"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando remitos para facturar. " + ex.Message));
            }
        }

    }
}