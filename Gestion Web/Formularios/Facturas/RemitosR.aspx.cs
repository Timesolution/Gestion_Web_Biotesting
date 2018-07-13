using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        controladorFunciones contFunciones = new controladorFunciones();
        controladorFacturacion controladorF = new controladorFacturacion();
        controladorSucursal controlSucursal = new controladorSucursal();
        controladorCliente controlCliente = new controladorCliente();
        Mensajes m = new Mensajes();
        Configuracion c = new Configuracion();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int original;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc= Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.original = Convert.ToInt32(Request.QueryString["o"]);

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
        protected void lbtnImprimirTodo_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("pdfs/");

                //limpio la carpeta donde van los pdfs asi no muestra pdfs viejos
                BorrarPDFS(path);

                string idtildado = "";
                int contadorOk = 0;
                int contadorTotal = 0;

                //chequeo lo que este tildado y lo imprimo
                foreach (Control C in phRemitos.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[2] as CheckBox;

                    if (ch.Checked)
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                        contadorTotal++;
                    }
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!String.IsNullOrEmpty(idtildado))
                {
                    foreach (string id in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            Remito r = this.controlador.obtenerRemitoId(Convert.ToInt32(id));
                            string fileName = "rem-" + r.numero + "_" + r.id + ".pdf";
                            int i = this.GenerarImpresionPDF(r.id, path + fileName);
                            if (i > 0)
                            {
                                contadorOk++;
                            }
                        }
                    }
                }
                //si no tengo tildada ninguna factura ejecuto la busqueda en la base y genero un pdf de todas
                else
                {
                    List<Remito> listRemitos = controlador.obtenerRemitosRango(txtFechaDesde.Text, txtFechaHasta.Text, Convert.ToInt32(DropListSucursal.SelectedValue));

                    foreach (var remito in listRemitos)
                    {
                        var idRemito = remito.id;
                        idRemito = Convert.ToInt32(idRemito);
                        Remito r = this.controlador.obtenerRemitoId(Convert.ToInt32(idRemito));
                        string fileName = "rem-" + r.numero + "_" + r.id + ".pdf";
                        int i = this.GenerarImpresionPDF(r.id, path + fileName);
                        if (i > 0)
                        {
                            contadorOk++;
                        }
                    }
                }

                string[] pdfs = Directory.GetFiles(path);
                string nombre = path + "rem-" + DateTime.Now.ToString("dd-MM-yyyy_hhmmss") + ".pdf";
                int ok = this.contFunciones.CombineMultiplePDFs(pdfs, nombre);
                if (ok > 0)
                {
                    this.descargar(nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Realizados con exito:" + contadorOk + "de " + contadorTotal, ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo imprimir"));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }
        public void BorrarPDFS(string path)
        {
            string[] pdfs = Directory.GetFiles(path);

            foreach (string filePath in pdfs)
            {
                File.Delete(filePath);
            }
        }
        private void descargar(string path)
        {
            try
            {
                System.IO.FileInfo toDownload =
                     new System.IO.FileInfo(path);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition",
                           "attachment; filename=" + toDownload.Name);
                HttpContext.Current.Response.AddHeader("Content-Length",
                           toDownload.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.WriteFile(path);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", " Error descargando excel de facturacion. " + ex.Message);

            }
        }
        private int GenerarImpresionPDF(int idRemito, string pathGenerar)
        {
            try
            {
                DataTable dtDatos = controladorF.obtenerDetalleRemito(idRemito);

                DataTable dtDetalle = controladorF.obtenerDatosRemito(idRemito);

                DataTable dtNroFacturas = controladorF.obtenerNroFacturaByRemito(idRemito);
                int sucursalFacturada = 0;
                Sucursal sucursalOrigen = new Sucursal();
                Sucursal sucursalRemitida = new Sucursal();
                //Comentario factura
                DataTable dtComentariosFactura = new DataTable();

                //Si el remito es de una factura.
                if (dtNroFacturas.Rows.Count > 0)
                {
                    //busco comentario con el idfactura que esta en el remito
                    int nroFactura = Convert.ToInt32(dtNroFacturas.Rows[0].ItemArray[1].ToString());
                    dtComentariosFactura = this.controladorF.obtenerComentarioPresupuesto(nroFactura);

                    //obtengo el numero de la suc a la que se movio stock
                    string sucFact = dtNroFacturas.Rows[0]["SucursalFacturada"].ToString();

                    if (!String.IsNullOrEmpty(sucFact))
                    {
                        sucursalFacturada = Convert.ToInt32(sucFact);
                    }

                    ////si el comentario de la factura tiene la fecha en null o vacio, elimino la fila para que no se muestre la tabla en el report.
                    //if (dtComentariosFactura.Rows[0].ItemArray[2].ToString() == null || dtComentariosFactura.Rows[0].ItemArray[2].ToString() == "")
                    //{
                    //    dtComentariosFactura.Rows.Remove(dtComentariosFactura.Rows[0]);
                    //}                    
                }

                //suc de donde se hizo
                int idSuc = Convert.ToInt32(dtDetalle.Rows[0]["sucursal"].ToString());
                sucursalOrigen = this.controlSucursal.obtenerSucursalID(idSuc);

                //suc remitida
                if (sucursalFacturada > 0)
                {
                    sucursalRemitida = this.controlSucursal.obtenerSucursalID(sucursalFacturada);
                }

                DataRow srCliente = dtDetalle.Rows[0];
                string codigoCliente = srCliente[5].ToString();
                int idCliente = Convert.ToInt32(srCliente["idCliente"].ToString());

                //DataTable dtTotal = controladorF.obtenerTotalPresupuesto(idPresupuesto);
                DataTable dtTotales = controladorF.obtenerTotalRemito(idRemito);
                DataRow dr = dtTotales.Rows[0];

                //Busco senias de los pedidos remitidos
                decimal senia = controlador.obtenerSeniaRemito(idRemito);
                string seniaLetras = Numalet.ToCardinal(senia);

                //neto no grabado
                decimal subtotal = Convert.ToDecimal(dr[4]);

                decimal descuento = Convert.ToDecimal(dr[1]);

                //subtotal menos el descuento
                decimal subtotal2 = Convert.ToDecimal(dr[5]);

                //iva discriminado de la fact
                decimal iva = Convert.ToDecimal(dr[3]);

                subtotal = subtotal + iva;

                //total de la factura
                decimal total = Convert.ToDecimal(dr[2]);

                //letras
                string totalS = Numalet.ToCardinal(total.ToString().Replace(',', '.'));
                //string totalS = Numalet.ToCardinal("18.25");

                //cant unidades
                decimal cant = 2;

                //direccion cliente
                string direLegal = "-";
                string direEntrega = "-";

                DataTable dtDireccion = controlCliente.obtenerDireccionesById(idCliente);
                if (dtDireccion != null)
                {
                    foreach (DataRow drl in dtDireccion.Rows)
                    {
                        if (drl["nombre"].ToString() == "Legal")
                        {
                            direLegal = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                        if (drl["nombre"].ToString() == "Entrega")
                        {
                            direEntrega = drl[1].ToString() + " " + drl[2].ToString() + " " + drl[3].ToString() + " " +
                                drl[4].ToString() + " " + drl[5].ToString() + " ";
                        }
                    }
                }

                //controladorFRemitos contRemito = new controladorFRemitos();
                //obtengo id empresa para buscar el logo correspondiente    
                Remito r = controlador.obtenerRemitoId(idRemito);
                //string logo = Server.MapPath("../../Facturas/" + idEmpresa + "/" + pv.id_suc +"/Logo.jpg");                
                string logo = Server.MapPath("../../Facturas/" + sucursalOrigen.empresa.id + "/" + sucursalOrigen.id + "/" + r.ptoV.id + "/Logo.jpg");
                Log.EscribirSQL(1, "INFO", "Ruta Logo " + logo);

                //datos cai
                string cai = "";
                string fechaVencCai = "";
                try
                {
                    var pv = this.controlSucursal.obtenerPuntoVentaPV(r.ptoV.puntoVenta.PadLeft(4, '0'), r.sucursal.id, r.empresa.id);
                    cai = pv.caiRemito;
                    fechaVencCai = pv.caiVencimiento.ToString("dd/MM/yyyy");
                }
                catch
                { }

                DataTable dtComentarios = controlador.obtenerComentarioRemito(idRemito);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("RemitoR.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DetallePresupuesto", dtDetalle);
                ReportDataSource rds2 = new ReportDataSource("DatosPresupuesto", dtDatos);
                ReportDataSource rds3 = new ReportDataSource("DetalleComentarios", dtComentarios);
                ReportDataSource rds4 = new ReportDataSource("NumerosFacturas", dtNroFacturas);
                ReportDataSource rds5 = new ReportDataSource("DetalleComentariosFactura", dtComentariosFactura);
                //ReportDataSource rds3 = new ReportDataSource("TotalPresupuesto", dtTotal);
                ReportParameter param = new ReportParameter("TotalPresupuesto", total.ToString("C"));
                ReportParameter param2 = new ReportParameter("Subtotal", subtotal.ToString("C"));
                ReportParameter param3 = new ReportParameter("Descuento", descuento.ToString("C"));

                ReportParameter param3b = new ReportParameter("Subtotal2", subtotal2.ToString("C"));
                param3b.Visible = false;
                ReportParameter param4b = new ReportParameter("Iva", iva.ToString("C"));
                param4b.Visible = false;

                ReportParameter param4 = new ReportParameter("DomicilioEntrega", direEntrega);
                ReportParameter param5 = new ReportParameter("DomicilioLegal", direLegal);

                ReportParameter param6 = new ReportParameter("CodigoCliente", codigoCliente);

                ReportParameter param7 = new ReportParameter("TotalLetras", totalS);
                ReportParameter param8 = new ReportParameter("TotalUnidades", cant.ToString());

                ReportParameter param9 = new ReportParameter("ParamSenia", "Seña: $" + senia.ToString());
                ReportParameter param10 = new ReportParameter("ParamSeniaLetras", seniaLetras);

                ReportParameter param11 = new ReportParameter("ParamSucursalRemitida", sucursalRemitida.nombre);

                ReportParameter param12 = new ReportParameter("ParamReimprime", this.original.ToString());

                ReportParameter param13 = new ReportParameter("ParamSucursal", sucursalOrigen.nombre);

                ReportParameter param32 = new ReportParameter("ParamImagen", @"file:///" + logo);

                ReportParameter param33 = new ReportParameter();

                //cot
                if (c.cot == "1")
                {
                    try
                    {
                        ControladorRemitoEntity contRemEntity = new ControladorRemitoEntity();
                        var rd = contRemEntity.obtenerRemitoDatosByRemito(idRemito);
                        param33 = new ReportParameter("COT", rd.COT + " CODIGO UNICO " + rd.CodUnico);
                    }
                    catch
                    {

                    }
                }

                string imagen = this.generarCodigo(idRemito);
                ReportParameter param34 = new ReportParameter("ParamCodBarra", @"file:///" + imagen);

                ReportParameter param35 = new ReportParameter("CAI", cai);

                ReportParameter param36 = new ReportParameter("CAIVencimiento", fechaVencCai);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rds2);
                this.ReportViewer1.LocalReport.DataSources.Add(rds3);
                this.ReportViewer1.LocalReport.DataSources.Add(rds4);
                this.ReportViewer1.LocalReport.DataSources.Add(rds5);

                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.SetParameters(param2);
                this.ReportViewer1.LocalReport.SetParameters(param3);
                this.ReportViewer1.LocalReport.SetParameters(param4);

                this.ReportViewer1.LocalReport.SetParameters(param3b);
                this.ReportViewer1.LocalReport.SetParameters(param4b);

                this.ReportViewer1.LocalReport.SetParameters(param5);
                this.ReportViewer1.LocalReport.SetParameters(param6);

                this.ReportViewer1.LocalReport.SetParameters(param7);
                this.ReportViewer1.LocalReport.SetParameters(param8);

                this.ReportViewer1.LocalReport.SetParameters(param9);
                this.ReportViewer1.LocalReport.SetParameters(param10);
                this.ReportViewer1.LocalReport.SetParameters(param11);
                this.ReportViewer1.LocalReport.SetParameters(param12);
                this.ReportViewer1.LocalReport.SetParameters(param13);

                this.ReportViewer1.LocalReport.SetParameters(param32);

                this.ReportViewer1.LocalReport.SetParameters(param33);
                this.ReportViewer1.LocalReport.SetParameters(param34);
                this.ReportViewer1.LocalReport.SetParameters(param35);
                this.ReportViewer1.LocalReport.SetParameters(param36);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                //save the generated report in the server
                //String path = Server.MapPath("../../Facturas/" + f.empresa.id + "/" + "/fc-" + f.numero + "_" + f.id + ".pdf");
                FileStream stream = File.Create(pathGenerar, pdfContent.Length);
                stream.Write(pdfContent, 0, pdfContent.Length);
                stream.Close();

                return 1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error enviando factura por mail. " + ex.Message));
                return -1;
            }
        }
        public string generarCodigo(int idRemito)
        {
            try
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = false;

                code128.Code = idRemito.ToString();

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                String path = HttpContext.Current.Server.MapPath("/Remitos/" + idRemito + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string archivo = path + "Codigo_" + idRemito + ".bmp";
                bm.Save(archivo, System.Drawing.Imaging.ImageFormat.Bmp);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error generando codigo de barra para pedido. " + ex.Message);
                return null;
            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
    }

    
}