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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class PanelControl : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion configuracion = new Configuracion();
        controladorSucursal contrSucu = new controladorSucursal();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPedido contPedido = new ControladorPedido();
        private int idSucursal;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                //this.idPuntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    this.cargarConfiguracion();
                    this.cargarEstados();
                }
                if (this.configuracion.editarArticulo == "1")
                {
                    this.panelPrecio.Visible = true;
                }
            }
            catch
            {
                
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
                        if (s == "57")
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

        private void cargarConfiguracion()
        {
            try
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Carga de configuraciones.");

                this.DropListPorcentajeIVA.SelectedValue = configuracion.porcentajeIva.Replace(',', '.');
                this.DropListPrecioArticulo.SelectedValue = configuracion.precioArticulo;
                this.DropListEditarDescripcion.SelectedValue = configuracion.editarArticulo;
                this.DropListConsumidorFinalCC.SelectedValue = configuracion.consumidorFinalCC;
                this.DropListNumeracionPagos.SelectedValue = configuracion.numeracionPagos;
                this.DropListSeparador.SelectedValue = configuracion.separadorListas;
                this.DropListEgresoStock.SelectedValue = configuracion.egresoStock;
                this.txtMaxDtoArticulo.Text = configuracion.maxDtoUnitario;
                this.txtMaxDtoFc.Text = configuracion.maxDtoFactura;
                this.DropListEdicionPrecio.SelectedValue = configuracion.edicionPrecioUnitario;
                this.txtLimiteDif.Text = configuracion.limiteDifCaja;
                this.txtMaxApertura.Text = configuracion.diasMaxApertura;
                this.DropListNumeracionArt.SelectedValue = configuracion.numeracionArticulos;
                this.DropListNumeracionCobros.SelectedValue = configuracion.numeracionCobros;
                this.DropListItemsEnCero.SelectedValue = configuracion.ItemsEnCero;
                this.txtDiasArticulosSinActualizar.Text = configuracion.AlertaArticulosSinActualizar;
                this.txtFechaCuentaCorrienteCompras.Text = configuracion.FechaFiltrosCuentaCorriente.Substring(0, 10).Replace(";","/");
                this.txtFechaCuentaCorrienteVentas.Text = configuracion.FechaFiltrosCuentaCorriente.Substring(11, 10).Replace(";", "/");
                this.txtTopeMinimoRetenciones.Text = configuracion.TopeMinimoRetencion;
                this.DropListRedondearPrecioVenta.SelectedValue = configuracion.RedondearPrecioVenta;
                this.DropListFacturarPRP.SelectedValue = configuracion.FacturarPRP;
                this.DropListEstadoPedidos.SelectedValue = configuracion.EstadoInicialPedidos;

                VisualizacionArticulos vista = new VisualizacionArticulos();
                this.CheckBoxProv.Checked = Convert.ToBoolean(vista.columnaProveedores);
                this.CheckBoxGrupo.Checked = Convert.ToBoolean(vista.columnaGrupo);
                this.CheckBoxSubGrupo.Checked = Convert.ToBoolean(vista.columnaSubGrupo);
                this.CheckBoxMoneda.Checked = Convert.ToBoolean(vista.columnaMoneda);
                this.CheckBoxAct.Checked = Convert.ToBoolean(vista.columnaActualizacion);
                this.CheckBoxPresen.Checked = Convert.ToBoolean(vista.columnaPresentacion);
                this.CheckBoxStock.Checked = Convert.ToBoolean(vista.columnaStock);
                this.CheckBoxMarca.Checked = Convert.ToBoolean(vista.columnaMarca);
                this.CheckBoxPrecioVentaMonedaOriginal.Checked = Convert.ToBoolean(vista.columnaPrecioVentaMonedaOriginal);

                if (configuracion.modoBlanco == "1")
                {                    
                    this.lbtnModoSeguro.CssClass = "btn btn-success";
                    this.lbtnModoSeguro.Text = "Activado";
                }
                else
                {
                    this.lbtnModoSeguro.CssClass = "btn btn-danger";
                    this.lbtnModoSeguro.Text = "Desactivado";
                }

                //Visualizacion de Cheques
                this.cargarVisualizacionCheques();

                //Visualizacion de Stock
                this.cargarVisualizacionStock();
                
            }
            catch
            {

            }
        }

        private void cargarVisualizacionCheques()
        {
            try
            {
                VisualizacionCheques vista = new VisualizacionCheques();

                this.CheckBoxChReciboCobro.Checked = Convert.ToBoolean(vista.columnaReciboCobro);
                this.CheckBoxChReciboPago.Checked = Convert.ToBoolean(vista.columnaReciboPago);
                this.CheckBoxChSucCobro.Checked = Convert.ToBoolean(vista.columnaSucursalCobro);
                this.CheckBoxChSucPago.Checked = Convert.ToBoolean(vista.columnaSucursalPago);
                this.CheckBoxChObservacion.Checked = Convert.ToBoolean(vista.columnaObservacion);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando visualización de cheques. Excepción: " + Ex.Message));
            }
        }

        private void cargarVisualizacionStock()
        {
            try
            {
                VisualizacionStock vista = new VisualizacionStock();

                this.CheckBoxStockImportacionesP.Checked = Convert.ToBoolean(vista.columnaImportacionesPendientes);
                this.CheckBoxStockRemitosP.Checked = Convert.ToBoolean(vista.columnaRemitosPendientes);
                this.CheckBoxStockReal.Checked = Convert.ToBoolean(vista.columnaStockReal);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando visualización de stock. Excepción: " + Ex.Message));
            }
        }

        //funcion carga DDL
        public void cargarEstados()
        {
            try
            {

                DataTable dt = this.contPedido.obtenerEstadosPedidos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListEstadoPedidos.DataSource = dt;
                this.DropListEstadoPedidos.DataValueField = "id";
                this.DropListEstadoPedidos.DataTextField = "descripcion";

                this.DropListEstadoPedidos.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Estados de Pedidos a la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListPorcentajeIVA.SelectedValue != "-1")
                {
                    configuracion.porcentajeIva = this.DropListPorcentajeIVA.SelectedValue;
                    int i = configuracion.ModificarPresupuestoIVA();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_SucUser"], "INFO", "Se modifico configuracion porcentaje iva PRP.");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Presupuesto modificado con exito", "PanelControl.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo actualizar Porcentaje IVA de los Presupuestos."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una opcion!."));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error, no se ha podido completar la operacion!. " + ex.Message));
            }
        }

        protected void btnAgregarPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListPrecioArticulo.SelectedValue != "-1")
                {
                    configuracion.precioArticulo = this.DropListPrecioArticulo.SelectedValue;
                    int i = configuracion.ModificarArticuloPrecio();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion modificar precio articulo.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Precio de venta del articulo modificado con exito!. \", {type: \"info\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Precio de venta del articulo modificado con exito!.", "PanelControl.aspx"));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Iva del Precio de venta.. \", {type: \"error\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo actualizar Iva del Precio de venta."));
                    }
                }
                else 
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);                                                                
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una opcion!."));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error, no se ha podido completar la operacion!. "+ ex.Message));
            }
        
        }

        protected void lbtnEditarDescripcion_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListEditarDescripcion.SelectedValue != "-1")
                {
                    configuracion.editarArticulo = this.DropListEditarDescripcion.SelectedValue;
                    int i = configuracion.ModificarEditarDescripcionArticulo();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion editar descripcion.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Editar Descripcion modificada con exito!. \", {type: \"info\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Opcion: Editar Descripcion modificada con exito!.", "PanelControl.aspx"));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Editar Descripcion. \", {type: \"error\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo actualizar Opcion: Editar Descripcion."));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);                                                                
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una opcion!."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error, no se ha podido completar la operacion!. " + ex.Message));
            }
        }

        protected void lbtnConsumidorFinalCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListConsumidorFinalCC.SelectedValue != "-1")
                {
                    configuracion.consumidorFinalCC = this.DropListConsumidorFinalCC.SelectedValue;
                    int i = configuracion.ModificarConsumidorFinalCC();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion consumidor final en cta cte.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: ConsumidorFinal Cta Cte modificada con exito!. \", {type: \"info\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Opcion: ConsumidorFinal Cta Cte modificada con exito!.", "PanelControl.aspx"));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: ConsumidorFinal Cta Cte.!. \", {type: \"error\"});", true);                                                                
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo actualizar Opcion: ConsumidorFinal Cta Cte."));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);                                                                
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una opcion!."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error, no se ha podido completar la operacion!. " + ex.Message));
            }
        }
        
        protected void btnGuardarPersonalizar_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                VisualizacionArticulos vista = new VisualizacionArticulos();

                //cuento que haya solo 4 seleccionados
                if (CheckBoxProv.Checked)
                    count++;
                if (CheckBoxGrupo.Checked)
                    count++;
                if (CheckBoxSubGrupo.Checked)
                    count++;
                if (CheckBoxMoneda.Checked)
                    count++;
                if (CheckBoxAct.Checked)
                    count++;
                if (CheckBoxPresen.Checked)
                    count++;
                if (CheckBoxStock.Checked)
                    count++;
                if (CheckBoxMarca.Checked)
                    count++;
                if (CheckBoxPrecioVentaMonedaOriginal.Checked)
                    count++;

                if (count == 4)
                {
                    controladorVisualizacion contVisual = new controladorVisualizacion();
                    vista.columnaProveedores = Convert.ToInt32(CheckBoxProv.Checked);
                    vista.columnaGrupo = Convert.ToInt32(CheckBoxGrupo.Checked);
                    vista.columnaSubGrupo = Convert.ToInt32(CheckBoxSubGrupo.Checked);
                    vista.columnaMoneda = Convert.ToInt32(CheckBoxMoneda.Checked);
                    vista.columnaActualizacion = Convert.ToInt32(CheckBoxAct.Checked);
                    vista.columnaPresentacion = Convert.ToInt32(CheckBoxPresen.Checked);
                    vista.columnaStock = Convert.ToInt32(CheckBoxStock.Checked);
                    vista.columnaMarca = Convert.ToInt32(CheckBoxMarca.Checked);
                    vista.columnaPrecioVentaMonedaOriginal = Convert.ToInt32(CheckBoxPrecioVentaMonedaOriginal.Checked);

                    int i = contVisual.ModificarVisualizacionArticulo(vista);

                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion visualizavion articulos.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!.", ""), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxError("No se pudo finalizar la operacion!."), false);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo 4 campos!."), false);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxError("Ocurrio un error guardando configuracion!. " + ex.Message), false);
            }
        }

        protected void lbtnNumeracionPagos_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListNumeracionPagos.SelectedValue != "-1")
                {
                    configuracion.numeracionPagos = this.DropListNumeracionPagos.SelectedValue;
                    int i = configuracion.ModificarNumeracionPagos();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion numeracion pagos.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Numeracion Pagos modificada con exito! \", {type: \"info\"});", true);                                                                
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Numeracion Pagos \", {type: \"error\"});", true);                                                                
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);                                                                                    
                }
            }
            catch
            {

            }
        }

        protected void lbtnSeparador_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.separadorListas = this.DropListSeparador.SelectedValue;
                int i = configuracion.ModificarSeparadorListas();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion separador listas.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Separador Listas modificada con exito! \", {type: \"info\"});", true);                                        
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Separador Listas! \", {type: \"error\"});", true);                                        
                }
            }
            catch
            {

            }
        }

        protected void lbtnEgresoStock_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.egresoStock = this.DropListEgresoStock.SelectedValue;
                int i = configuracion.ModificarEgresoStock();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion egreso stock.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Egreso Stock modificada con exito!. \", {type: \"info\"});", true);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Egreso Stock!. \", {type: \"info\"});", true);                                       
                }
            }
            catch
            {

            }
        }

        protected void lbtnMaxDtoArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.maxDtoUnitario = this.txtMaxDtoArticulo.Text;
                int i = configuracion.ModificarMaxDtoUnitario();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion Max dto articulo.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Max dto en articulo modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Max dto en articulo. \", {type: \"error\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo actualizar Opcion: Max dto en articulo"));
                }
            }
            catch
            {

            }
        }

        protected void lbtnMaxDtoFc_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.maxDtoFactura = this.txtMaxDtoFc.Text;
                int i = configuracion.ModificarMaxDtoFactura();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion Max dto en fc.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Max dto en factura modificada con exito!. \", {type: \"info\"});", true);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Max dto en factura. \", {type: \"error\"});", true);                    
                }
            }
            catch
            {

            }
        }

        protected void lbtnEdicionPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.edicionPrecioUnitario = this.DropListEdicionPrecio.SelectedValue;
                int i = configuracion.ModificarEdicionPrecioUnitario();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion edicion precio.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Edicion precio unitario modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Edicion precio unitario!. \", {type: \"error\"});", true);                    
                }
            }
            catch
            {

            }
        }

        protected void DropListEditarDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListEditarDescripcion.SelectedValue == "1")
                {
                    this.panelPrecio.Visible = true;
                    this.DropListEdicionPrecio.Focus();
                }
                else
                {
                    this.panelPrecio.Visible = false;
                    this.DropListEditarDescripcion.Focus();
                }
            }
            catch { }
        }

        protected void lbtnNumArt_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.numeracionArticulos = this.DropListNumeracionArt.SelectedValue;
                int i = configuracion.ModificarNumeracionArticulos();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion numeracion articulos.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Edicion numeracion Articulos modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: numeracion Articulos!. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }

        protected void lbtnLimiteDif_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.limiteDifCaja = this.txtLimiteDif.Text;
                int i = configuracion.ModificarLimiteDifCaja();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion Limite dif caja.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Limite dif caja modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Limite dif caja!. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }

        protected void lbtnMaxApertura_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    configuracion.diasMaxApertura = this.txtMaxApertura.Text;
                    int i = configuracion.ModificarDiasMaxApertura();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion Max dias apertura.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Dias max apertura caja modificada con exito!. \", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Dias max apertura caja!. \", {type: \"error\"});", true);
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
        }

        protected void lbtnModoSeguro_Click(object sender, EventArgs e)
        {
            try
            {
                if(configuracion.modoBlanco == "0")
                    configuracion.modoBlanco = "1";
                else                
                    configuracion.modoBlanco = "0";

                int i = configuracion.ModificarModoBlanco();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion Modo Seguro.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Modo Seguro modificada con exito!. \", {type: \"info\"}); location.href('PanelControl.aspx');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Modo Seguro!. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }

        protected void lbtnNumeracionCobros_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListNumeracionCobros.SelectedValue != "-1")
                {
                    configuracion.numeracionCobros = this.DropListNumeracionCobros.SelectedValue;
                    int i = configuracion.ModificarNumeracionCobros();
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion numeracion cobros.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Numeracion Cobros modificada con exito! \", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Opcion: Numeracion Cobros \", {type: \"error\"});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
                }
            }
            catch
            {

            }
        }

        protected void lbtnItemsEnCero_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.ItemsEnCero = this.DropListItemsEnCero.SelectedValue;
                int i = configuracion.ModificarItemsEnCero();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion items en cero.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Items en Cero modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Items en Cero!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void ltbnAlertaPreciosArticulosSinActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.AlertaArticulosSinActualizar = this.txtDiasArticulosSinActualizar.Text;
                int i = configuracion.ModificarAlertaArticulosSinActualizar();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modificó configuración alerta articulos sin actualizar.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Configuración: Alerta precios artículos sin actualizar modificada con éxito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Alerta precios artículos sin actualizar!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnFiltroFechaCuentaCorriente_Click(object sender, EventArgs e)
        {
            try
            {
                string fechaCuentaCorrienteCompras = txtFechaCuentaCorrienteCompras.Text.Replace("/",";");
                string fechaCuentaCorrienteVentas = txtFechaCuentaCorrienteVentas.Text.Replace("/", ";");
                configuracion.FechaFiltrosCuentaCorriente = fechaCuentaCorrienteCompras + ";" + fechaCuentaCorrienteVentas;
                int i = configuracion.ModificarFechaFiltrosCuentaCorriente();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion items en cero.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Fecha filtros Cuenta Corriente modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Fecha filtros Cuenta Corriente!. \", {type: \"info\"});", true);
                }

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnTopeMinimoRetenciones_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.TopeMinimoRetencion = this.txtTopeMinimoRetenciones.Text;
                int i = configuracion.ModificarTopeMinimoRetenciones();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modificó configuración tope minimo retencion.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Configuración: Tope minimo retencion modificada con éxito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Tope minimo retencion!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnRedondearPrecioVenta_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.RedondearPrecioVenta = this.DropListRedondearPrecioVenta.SelectedValue;
                int i = configuracion.ModificarRedondearPrecioVenta();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion redondear precio de venta.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Redondear Precio de Venta modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Redondear Precio de Venta!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnRefacturarPRP_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.FacturarPRP = this.DropListFacturarPRP.SelectedValue;
                int i = configuracion.ModificarFacturarPRP();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion facturar PRP.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Facturar PRP modificada con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Facturar PRP!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnGuardarPersonalizarCheques_Click(object sender, EventArgs e)
        {
            try
            {
                controladorVisualizacion contVisualizacion = new controladorVisualizacion();

                //Para que no se produzca un desfazaje de las columnas, solo puede haber 2 items tildados

                //Creo un contador para contabilizar los items tildados
                int c = 0;

                foreach (Control ctrl in UpdatePanelVisualizacionCheques.ContentTemplateContainer.Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        if ((ctrl as CheckBox).Checked)
                            c++;
                    }
                }

                //Si el contador es mayor a 2, no lo dejo
                if (c != 2)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Sólo puede seleccionar dos columnas"));
                    return;
                }

                VisualizacionCheques vista = new VisualizacionCheques();

                vista.columnaReciboCobro = Convert.ToInt16(this.CheckBoxChReciboCobro.Checked);
                vista.columnaReciboPago = Convert.ToInt16(this.CheckBoxChReciboPago.Checked);
                vista.columnaSucursalCobro = Convert.ToInt16(this.CheckBoxChSucCobro.Checked);
                vista.columnaSucursalPago = Convert.ToInt16(this.CheckBoxChSucPago.Checked);
                vista.columnaObservacion = Convert.ToInt16(this.CheckBoxChObservacion.Checked);

                int i = contVisualizacion.ModificarVisualizacionCheques(vista);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vista de cheques modificada con éxito!",null));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar la vista de cheques"));

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando visualización de Cheques. Excepción: " + Ex.Message));
            }
        }

        protected void lbtnEstadoIniPedidos_Click(object sender, EventArgs e)
        {
            try
            {
                configuracion.EstadoInicialPedidos = this.DropListEstadoPedidos.SelectedValue;
                int i = configuracion.ModificarEstadoInicialPedido();
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion de estado inicial pedidos.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Opcion: Estado inicial pedidos modificado con exito!. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo actualizar Configuracion: Estado inicial pedidos!. \", {type: \"info\"});", true);
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una opcion!. \");", true);
            }
        }

        protected void lbtnGuardarPersonalizarStock_Click(object sender, EventArgs e)
        {
            try
            {
                controladorVisualizacion contVisualizacion = new controladorVisualizacion();

                //Para que no se produzca un desfazaje de las columnas, solo puede haber 2 items tildados

                //Creo un contador para contabilizar los items tildados
                //int c = 0;

                //foreach (Control ctrl in UpdatePanelVisualizacionStock.ContentTemplateContainer.Controls)
                //{
                //    if (ctrl is CheckBox)
                //    {
                //        if ((ctrl as CheckBox).Checked)
                //            c++;
                //    }
                //}

                ////Si el contador es mayor a 2, no lo dejo
                //if (c != 2)
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Sólo puede seleccionar dos columnas"));
                //    return;
                //}

                VisualizacionStock vista = new VisualizacionStock();

                vista.columnaRemitosPendientes = Convert.ToInt16(this.CheckBoxStockRemitosP.Checked);
                vista.columnaImportacionesPendientes = Convert.ToInt16(this.CheckBoxStockImportacionesP.Checked);
                vista.columnaStockReal = Convert.ToInt16(this.CheckBoxStockReal.Checked);

                int i = contVisualizacion.ModificarVisualizacionStock(vista);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vista de stock modificada con éxito!", null));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo modificar la vista de stock"));

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando visualización de stock. Excepción: " + Ex.Message));
            }
        }
    }
}