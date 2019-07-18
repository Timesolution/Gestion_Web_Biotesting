using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
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
    public partial class ImportacionesDetalleF : System.Web.UI.Page
    {
        ControladorImportaciones contImportacion = new ControladorImportaciones();
        controladorArticulo contArticulos = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorMoneda contMoneda = new controladorMoneda();

        decimal totalFob = 0;
        decimal totalCompra = 0;
        Mensajes m = new Mensajes();

        int idImportacion;
        int idArtDetalle;//id del articulo a editar
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idImportacion = Convert.ToInt32(Request.QueryString["id"]);
                this.idArtDetalle = Convert.ToInt32(Request.QueryString["idArt"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);

                this.VerificarLogin();

                if (!IsPostBack)
                {
                    if (accion == 2)
                    {
                        this.llenarCamposArt();
                    }
                }
                this.cargarDetalleImportacion();

            }
            catch (Exception ex)
            {

            }

        }

        private void llenarCamposArt()
        {
            try
            {
                Importacione importacion = contImportacion.obtenerImportacionByID(idImportacion);
                Importaciones_Detalle itemDet = new Importaciones_Detalle();
                itemDet = contImportacion.obtenerDetalleImportacionById(idArtDetalle);
                Articulo articulo = contArticulos.obtenerArticuloByID(Int32.Parse(itemDet.Articulo.ToString()));
                this.txtCantidad.Text = itemDet.Cantidad.ToString();
                this.txtSIM.Text = itemDet.SIM;
                this.txtFOB.Text = itemDet.FOB.ToString();
                this.ListArticulosBusqueda.Items.Clear();
                ListItem valor = new ListItem(articulo.descripcion, articulo.id.ToString());
                this.ListArticulosBusqueda.Items.Add(valor);
                //divido el precio de venta en pesos por la moneda utilizada en el articulo
                this.txtPrecioCompra.Text = articulo.costo.ToString();
                Moneda moneda = new Moneda();
                moneda = contMoneda.obtenerMonedaDesc(articulo.monedaVenta.moneda);
                decimal precioVentaMonedaOriginal = Math.Round(articulo.costoReal / Convert.ToDecimal(moneda.cambio), 2);
                this.txtPrecioVenta.Text = precioVentaMonedaOriginal.ToString("0.00");
                this.txtPPP.Text = this.contImportacion.obtenerPPPArticulo(articulo.id).ToString();
                stock s = this.contArtEnt.obtenerStockArticuloLocal(articulo.id, importacion.Sucursal.Value);
                if (s != null)
                {
                    this.txtStockActual.Text = s.stock1.Value.ToString();
                }
               
            }
            catch (Exception)
            {

                throw;
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
                        if (s == "74")
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
        private void cargarProducto(string busqueda)
        {
            try
            {
                //Articulo art = this.controlador.obtenerArticuloCodigo(busqueda);
                DataTable dtArticulos = this.contArticulos.buscarArticulosDT(busqueda);

                if (dtArticulos != null)
                {
                    //agrego todos
                    DataRow dr = dtArticulos.NewRow();
                    dr["Descripcion"] = "Seleccione...";
                    dr["id"] = -1;
                    dtArticulos.Rows.InsertAt(dr, 0);

                    this.ListArticulosBusqueda.DataSource = dtArticulos;
                    this.ListArticulosBusqueda.DataValueField = "id";
                    this.ListArticulosBusqueda.DataTextField = "Descripcion";
                    this.ListArticulosBusqueda.DataBind();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se encuentra Articulo " + busqueda));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando Articulo. " + ex.Message));
            }
        }
        private void cargarDetalleImportacion()
        {
            try
            {
                this.phItems.Controls.Clear();

                Importacione i = this.contImportacion.obtenerImportacionByID(this.idImportacion);
                if (i != null)
                {
                    foreach (var item in i.Importaciones_Detalle)
                    {
                        this.totalFob += item.FOB.Value * item.Cantidad.Value;
                        this.totalCompra += item.PrecioCompra.Value * item.Cantidad.Value;
                    }
                    foreach (var item in i.Importaciones_Detalle)
                    {
                        this.cargarItemImportacionPH(item);
                    }
                }

                this.lblTotalFob.Text = this.totalFob.ToString("C");
                this.lblTotalCompra.Text = this.totalCompra.ToString("C");
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarItemImportacionPH(Importaciones_Detalle item)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = "tr_" + item.Id;

                TableCell celCodigo = new TableCell();
                var art = this.contArtEnt.obtenerArticuloEntity(item.Articulo.Value);
                celCodigo.Text = art.codigo;
                celCodigo.HorizontalAlign = HorizontalAlign.Center;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCodigo);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = art.descripcion;
                celArticulo.HorizontalAlign = HorizontalAlign.Center;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celArticulo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = item.Cantidad.Value.ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCantidad);

                TableCell celSIM = new TableCell();
                celSIM.Text = item.SIM;
                celSIM.HorizontalAlign = HorizontalAlign.Center;
                celSIM.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celSIM);

                TableCell celFOB = new TableCell();
                celFOB.Text = item.FOB.Value.ToString("C");
                celFOB.HorizontalAlign = HorizontalAlign.Right;
                celFOB.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celFOB);

                TableCell celTotalFOB = new TableCell();
                celTotalFOB.Text = (item.FOB.Value * item.Cantidad.Value).ToString("C");
                celTotalFOB.HorizontalAlign = HorizontalAlign.Right;
                celTotalFOB.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celTotalFOB);

                TableCell celPPP = new TableCell();
                celPPP.Text = item.PPP.Value.ToString("C");
                celPPP.HorizontalAlign = HorizontalAlign.Right;
                celPPP.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celPPP);

                TableCell celCompra = new TableCell();
                celCompra.Text = item.PrecioCompra.Value.ToString("C");
                celCompra.HorizontalAlign = HorizontalAlign.Right;
                celCompra.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCompra);

                TableCell celTotalCompra = new TableCell();
                celTotalCompra.Text = (item.PrecioCompra.Value * item.Cantidad.Value).ToString("C");
                celTotalCompra.HorizontalAlign = HorizontalAlign.Right;
                celTotalCompra.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celTotalCompra);

                TableCell celPrecioArgentina = new TableCell();
                decimal totalGastos = item.Importacione.Importaciones_Gastos.Sum(x => x.ImportePesos.Value);
                totalGastos = decimal.Round(totalGastos, 2);
                decimal porcentualGastos = ((item.FOB.Value * item.Cantidad.Value) / this.totalFob) * totalGastos;
                porcentualGastos = decimal.Round(porcentualGastos, 2);
                decimal totalLocal = ((porcentualGastos / item.Cantidad.Value) + (item.FOB.Value * item.Importacione.DolarDespacho.Value));
                totalLocal = decimal.Round(totalLocal, 2);
                celPrecioArgentina.Text = totalLocal.ToString("C");
                celPrecioArgentina.HorizontalAlign = HorizontalAlign.Right;
                celPrecioArgentina.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celPrecioArgentina);

                TableCell celVenta = new TableCell();
                celVenta.Text = item.PrecioVenta.Value.ToString("C");
                celVenta.HorizontalAlign = HorizontalAlign.Right;
                celVenta.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celVenta);

                TableCell celStock = new TableCell();
                celStock.Text = item.StockActual.Value.ToString();
                celStock.HorizontalAlign = HorizontalAlign.Right;
                celStock.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celStock);


                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + item.Id.ToString();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + item.Id.ToString() + ");";
                celAccion.Controls.Add(btnEliminar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnActualizar = new LinkButton();
                btnActualizar.ID = "btnActualizar_" + item.Id.ToString();
                btnActualizar.CssClass = "btn btn-info";
                btnActualizar.Text = "<span class='shortcut-icon icon-refresh'></span>";
                //btnActualizar.Attributes.Add("data-toggle", "modal");
                //btnActualizar.Attributes.Add("href", "#modalActualizacion");
                //btnActualizar.OnClientClick = "abrirdialog2(" + item.Id.ToString() + ");";
                btnActualizar.Click += new EventHandler(this.ActualizarCosto);
                celAccion.Controls.Add(btnActualizar);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal l3 = new Literal();

                l3.Text = "<a href=\"ImportacionesDetalleF.aspx?a=2&id=" + this.idImportacion + "&idArt=" + item.Id +
                "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >";
                l3.Text += "<span class='shortcut-icon icon-pencil'></span>";
                l3.Text += "</a>";

                celAccion.Controls.Add(l3);

                tr.Controls.Add(celAccion);

                this.phItems.Controls.Add(tr);
            }
            catch
            {

            }
        }

        protected void lbtnCancelarEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("../Compras/ImportacionesDetalleF.aspx?id=" + this.idImportacion);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void lbtnAgregarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if(accion == 2)
                {
                    EditarDetalleArticulos();
                }
                else
                {
                    AgregarDetalleArticulos();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error agregando. " + ex.Message + ".\";", true);
            }
        }
        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    this.cargarProducto(this.txtBusqueda.Text);
                }
            }
            catch
            {

            }
        }
        protected void ListArticulosBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Importacione i = this.contImportacion.obtenerImportacionByID(this.idImportacion);
                int art = Convert.ToInt32(this.ListArticulosBusqueda.SelectedValue);
                if (art > 0)
                {
                    Articulo a = this.contArticulos.obtenerArticuloId(art);
                    if (a != null)
                    {
                        //divido el precio de venta en pesos por la moneda utilizada en el articulo
                        this.txtPrecioCompra.Text = a.costo.ToString();
                        Moneda moneda = new Moneda();
                        moneda = contMoneda.obtenerMonedaDesc(a.monedaVenta.moneda);
                        decimal precioVentaMonedaOriginal = Math.Round(a.costoReal / Convert.ToDecimal(moneda.cambio), 2);
                        this.txtPrecioVenta.Text = precioVentaMonedaOriginal.ToString("0.00");
                        this.txtPPP.Text = this.contImportacion.obtenerPPPArticulo(art).ToString();
                        stock s = this.contArtEnt.obtenerStockArticuloLocal(a.id, i.Sucursal.Value);
                        if (s != null)
                        {
                            this.txtStockActual.Text = s.stock1.Value.ToString();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.txtMovimiento.Text);

                int ok = this.contImportacion.eliminarDetalleImportacion(id);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Eliminado con exito!. ", Request.Url.ToString()));
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando item. "));
            }
        }
        private void ActualizarCosto(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID.Split('_')[1];
                int idDetalle = Convert.ToInt32(idBoton);
                Importacione i = this.contImportacion.obtenerImportacionByID(this.idImportacion);
                Importaciones_Detalle item = i.Importaciones_Detalle.Where(x => x.Id == idDetalle).FirstOrDefault();
                if (item != null)
                {
                    articulo art = this.contArtEnt.obtenerArticuloEntity(item.Articulo.Value);

                    decimal totalGastos = item.Importacione.Importaciones_Gastos.Sum(x => x.ImportePesos.Value);
                    totalGastos = decimal.Round(totalGastos, 2);

                    decimal porcentualGastos = ((item.FOB.Value * item.Cantidad.Value) / this.totalFob) * totalGastos;
                    porcentualGastos = decimal.Round(porcentualGastos, 2);

                    decimal totalLocal = ((porcentualGastos / item.Cantidad.Value) + (item.FOB.Value * item.Importacione.DolarDespacho.Value));
                    totalLocal = decimal.Round(totalLocal, 2);

                    Moneda moneda = this.contMoneda.obtenerMonedaID(art.monedaVenta.Value);
                    decimal costoNuevo = decimal.Round(totalLocal / moneda.cambio, 2);
                    decimal incidencia = costoNuevo + (costoNuevo * (art.incidencia.Value / 100));
                    incidencia = decimal.Round(incidencia * moneda.cambio, 2);
                    decimal costoReal = incidencia + (incidencia * (art.impInternos.Value / 100)) + (incidencia * (art.ingresosBrutos.Value / 100));
                    costoReal = decimal.Round(costoReal, 2);

                    decimal margen = ((art.precioSinIva.Value / costoReal) - 1) * 100;
                    margen = decimal.Round(margen, 2);


                    decimal precioNuevo = costoReal * (1 + (art.margen.Value / 100));
                    //precioNuevo = precioNuevo * moneda.cambio;
                    precioNuevo = decimal.Round(precioNuevo * (1 + (art.porcentajeIva.Value / 100)), 2);

                    this.txtCostoNuevo.Text = costoNuevo.ToString();
                    this.txtCostoNuevo2.Text = costoNuevo.ToString();
                    this.txtPrecioVentaActual.Text = art.precioVenta.Value.ToString();
                    this.txtMargenActual.Text = art.margen.Value.ToString();
                    this.txtMargenNuevo.Text = margen.ToString();
                    this.txtPrecioVentaNuevo.Text = precioNuevo.ToString();

                    this.txtIdArticulo.Text = art.id.ToString();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "abrirdialog2(0);", true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnActualizarPrecioVta_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo art = this.contArticulos.obtenerArticuloByID(Convert.ToInt32(this.txtIdArticulo.Text));
                art.costo = Convert.ToDecimal(this.txtCostoNuevo.Text);
                art = this.contArticulos.obtenerPrecioVentaDesdeCosto(art);
                int ok = this.contArticulos.modificarArticulo(art, art.codigo,1);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Actualizado con Exito\", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se Pudo actualizar.\");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error. " + ex.Message + "\";", true);
            }
        }
        protected void lbtnActualizarMargen_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo art = this.contArticulos.obtenerArticuloByID(Convert.ToInt32(this.txtIdArticulo.Text));
                art.costo = Convert.ToDecimal(this.txtCostoNuevo.Text);
                art.margen = Convert.ToDecimal(this.txtMargenNuevo.Text);
                art = this.contArticulos.obtenerPrecioVentaDesdeCosto(art);
                art = this.contArticulos.obtenerPrecioVentaDesdeVenta(art, Convert.ToDecimal(this.txtPrecioVentaActual.Text));
                int ok = this.contArticulos.modificarArticulo(art, art.codigo,1);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Actualizado con Exito\", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se Pudo actualizar.\");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Ocurrio un error. " + ex.Message + "\";", true);
            }
        }

        private void AgregarDetalleArticulos()//Agrega un nuevo detalle de articulo a la importacion
        {
            try
            {
                Importaciones_Detalle item = new Importaciones_Detalle();
                traerDatosDePantalla(item);
                int ok = this.contImportacion.agregarDetalleImportacion(item);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Agregada con Exito\", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se Pudo agregar.\";", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error al agregar el articulo. " + ex.Message + ".\";", true);
                throw;
            }
        }
        private void EditarDetalleArticulos()//Guarda el articulo editado a la importacion
        {
            try
            {
                Importaciones_Detalle item = this.contImportacion.obtenerDetalleImportacionById(idArtDetalle);
                this.traerDatosDePantalla(item);
                int agregar = contImportacion.modificarDetalleImportacion(item);
                if (agregar > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Detalle actualizado con Exito\", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                    Response.Redirect("../Compras/ImportacionesDetalleF.aspx?id=" + this.idImportacion);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se Pudo modificar el detalle.\";", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error al editar el detalle del articulo. " + ex.Message + ".\";", true);
                
            }
        }
        Importaciones_Detalle traerDatosDePantalla(Importaciones_Detalle obj)//trae los datos de los textbox y asigna a un objeto y lo devuelve
        {
            try
            {
                obj.Articulo = Convert.ToInt32(this.ListArticulosBusqueda.SelectedValue);
                obj.IdImportacion = this.idImportacion;
                obj.FOB = Convert.ToDecimal(this.txtFOB.Text);
                obj.SIM = this.txtSIM.Text;
                obj.Cantidad = Convert.ToDecimal(this.txtCantidad.Text);
                obj.StockActual = Convert.ToDecimal(this.txtStockActual.Text);
                obj.PrecioCompra = Convert.ToDecimal(this.txtPrecioCompra.Text.Replace("$", ""));
                obj.PrecioVenta = Convert.ToDecimal(this.txtPrecioVenta.Text.Replace("$", ""));
                decimal ultimoPpp = Convert.ToDecimal(this.txtPPP.Text.Replace("$", ""));
                obj.PPP = ((ultimoPpp * obj.StockActual) + (obj.FOB * obj.Cantidad)) / (obj.StockActual + obj.Cantidad);
                return obj;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error al obtener los datos de la pantalla. " + ex.Message + ".\";", true);
                throw;
            }
        }
    }
}


