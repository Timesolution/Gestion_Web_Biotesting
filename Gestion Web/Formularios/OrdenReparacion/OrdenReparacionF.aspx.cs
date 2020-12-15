using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    using Gestion_Api.Entitys;
    using Gestion_Api.Controladores;
    using Gestion_Api.Modelo;
    using Disipar.Models;
    using Gestor_Solution.Controladores;
    using System.Globalization;
    using System.Data;
    using Microsoft.Reporting.WebForms;

    public partial class OrdenReparacionF : System.Web.UI.Page
    {
        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        controladorServicioTecnicoEntity contServTecnico = new controladorServicioTecnicoEntity();
        Mensajes m = new Mensajes();
        controladorCliente contCliente = new controladorCliente();
        ControladorConfiguracion contConfig = new ControladorConfiguracion();
        controladorSucursal contSucursal = new controladorSucursal();
        int accion = 0;
        int numeroOrden = 0;
        int cliente = 0;
        int sucursal = 0;
        int sucursalOR = 0;
        int estado = 0;
        int articulo = 0;
        string fechaD = "";
        string fechaH = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                accion = Convert.ToInt32(Request.QueryString["a"]);
                numeroOrden = Convert.ToInt32(Request.QueryString["n"]);
                cliente = Convert.ToInt32(Request.QueryString["c"]);
                sucursal = Convert.ToInt32(Request.QueryString["s"]);
                sucursalOR = Convert.ToInt32(Request.QueryString["sor"]);
                articulo = Convert.ToInt32(Request.QueryString["art"]);

                estado = Convert.ToInt32(Request.QueryString["e"]);
                fechaD = Request.QueryString["fd"];
                fechaH = Request.QueryString["fh"];

                if (!IsPostBack)
                {
                    if(string.IsNullOrEmpty(fechaD))
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtFechaDesde.Text = fechaD.ToString(new CultureInfo("es-AR"));

                    if (string.IsNullOrEmpty(fechaH))
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtFechaHasta.Text = fechaH.ToString(new CultureInfo("es-AR"));

                    this.cargarSucursal();
                    this.cargarClientes();
                    this.cargarEstados();
                    this.cargarSucursalOR();
                    this.cargarArticulos();
                }

                //esto lo hacemos para que se mantengan los servicios tecnicos tildados cuando vas a accion > seleccionar servicio tecnico y buscas servicios tecnicos
                if(!String.IsNullOrEmpty(txtServicioTecnico.Text))
                    ObtenerServiciosTecnicos(txtServicioTecnico.Text);

                if (accion == 0)
                    CargarOrdenesReparacion();
                else if(accion == 1)
                    BuscarPorNumeroOrden();
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error en pageload orden de reparacion. " + ex.Message);
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
                int permisoActivo = 0;
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "154")
                            permisoActivo = 1;

                        if(permisoActivo == 1)
                        {
                            if (s == "155")
                                lbtnAnular.Visible = true;

                            if (s == "156")
                                ltbnsServiceOficial.Visible = true;

                            if (s == "157")
                                lbtnDevolucionProveedor.Visible = true;

                            if (s == "158")
                                lbtnsRepararLocalmente.Visible = true;

                            if (s == "159")
                                lbtnsFinalizarOR.Visible = true;

                            if (s == "160")
                                lbtnEnviarSMS.Visible = true;

                            if (s == "182")
                                lbtnEnviarARepararLocalmente.Visible = true;

                            if (s == "183")
                                lbtnRepararLocalmente.Visible = true;

                            if (s == "184")
                                lbtnReparado.Visible = true;

                            if (s == "185")
                                lbtnDevolverASucursal.Visible = true;

                            if (s == "186")
                                lbtnSucOrigenRecibeProducto.Visible = true;

                            if (s == "187")
                                lbtnSeleccionarServiceOficial.Visible = true;

                            if (s == "188")
                                lbtnAsignarServiceOficial.Visible = true;

                            if (s == "189")
                                lbtnEnviarASucursalGarantias.Visible = true;

                            if (s == "190")
                                lbtnDevolverSucOrigen.Visible = true;

                            if (s == "191")
                                lbtnRecibidoSucOrigen.Visible = true;
                        }                        
                    }
                }

                return permisoActivo;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarEnPh(OrdenReparacion or)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();

                //fila
                TableRow tr = new TableRow();
                tr.ID = or.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = or.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumeroOrden = new TableCell();
                celNumeroOrden.Text = or.NumeroOrdenReparacion.Value.ToString("D8");
                celNumeroOrden.HorizontalAlign = HorizontalAlign.Left;
                celNumeroOrden.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroOrden);

                TableCell celNumeroSerie = new TableCell();
                celNumeroSerie.Text = or.NumeroSerie;
                celNumeroSerie.HorizontalAlign = HorizontalAlign.Left;
                celNumeroSerie.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroSerie);

                TableCell celSucursal = new TableCell();                
                celSucursal.Text = contSucursal.obtenerSucursalID((int)or.SucursalOrigen).nombre;
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);

                TableCell celPRP = new TableCell();
                celPRP.Text = contFacturacion.obtenerFacturaId((int)or.NumeroPRP).numero;
                celPRP.HorizontalAlign = HorizontalAlign.Left;
                celPRP.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPRP);

                TableCell celFechaCompra = new TableCell();
                celFechaCompra.Text = or.FechaCompra.Value.ToString("dd/MM/yyyy");
                celFechaCompra.HorizontalAlign = HorizontalAlign.Left;
                celFechaCompra.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFechaCompra);

                TableCell celCliente = new TableCell();
                celCliente.Text = contCliente.obtenerClienteID((int)or.Cliente).razonSocial;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCliente);

                TableCell celPlazoReparacion = new TableCell();
                celPlazoReparacion.Text = or.PlazoLimiteReparacion.ToString();
                celPlazoReparacion.HorizontalAlign = HorizontalAlign.Left;
                celPlazoReparacion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPlazoReparacion);

                TableCell celTopeReparacion = new TableCell();
                celTopeReparacion.Text = or.Fecha.Value.AddDays((int)or.PlazoLimiteReparacion).ToString("dd/MM/yyyy");
                celTopeReparacion.HorizontalAlign = HorizontalAlign.Left;
                celTopeReparacion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTopeReparacion);

                TableCell celEstado = new TableCell();
                celEstado.Text = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID((int)or.Estado).Descripcion;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEstado);

                TableCell celProgressBar = new TableCell();
                Literal lProgressBar = new Literal();
                int progreso = 0;
                //si la or tiene lapso de finalizacion calculo el progreso con ese valor, sino esta finalizada lo calculo con la fecha de creacion de la or
                if (or.LapsoFinalizacion == null)
                    progreso = CalcularProgressBar((DateTime)or.Fecha, (int)or.PlazoLimiteReparacion);
                else
                    progreso = (int)or.LapsoFinalizacion;

                if(progreso <= 70)
                {
                    lProgressBar.Text = "<div class=\"progress\"> <div class=\"progress-bar progress-bar-success\" style=\"width: " + progreso + "% \"></div></div>";
                }
                else if(progreso > 70 && progreso < 90)
                {
                    lProgressBar.Text = "<div class=\"progress\">";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-warning\" style=\"width: " + (progreso*20)/100 + "% \"></div>";
                    lProgressBar.Text += "</div>";
                }
                else if(progreso >= 90 && progreso <= 100)
                {
                    lProgressBar.Text = "<div class=\"progress\">";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-warning\" style=\"width: 20% \"></div>";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-danger\" style=\"width: " + (progreso*10)/100 + "% \"></div>";
                    lProgressBar.Text += "</div>";
                }
                else
                {
                    lProgressBar.Text = "<div class=\"progress\">";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-warning\" style=\"width: 20% \"></div>";
                    lProgressBar.Text += "<div class=\"progress-bar progress-bar-danger\" style=\"width: 10% \"></div>";
                    lProgressBar.Text += "</div>";
                }
                celProgressBar.Controls.Add(lProgressBar);
                tr.Cells.Add(celProgressBar);

                TableCell celSucursalOR = new TableCell();
                celSucursalOR.Text = contSucursal.obtenerSucursalID((int)or.SucursalOR).nombre;
                celSucursalOR.HorizontalAlign = HorizontalAlign.Left;
                celSucursalOR.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalOR);

                TableCell celAccion = new TableCell();

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + or.Id.ToString();
                lDetail.Text = "<a href=\"OrdenReparacionABM.aspx?a=2&idordenreparacion=" + or.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lDetail.Text += "<span class=\"shortcut-icon icon-pencil\"></span>";
                lDetail.Text += "</a>";

                celAccion.Controls.Add(lDetail);

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAccion.Controls.Add(l1);

                Literal lReport = new Literal();
                lReport.ID = "btnReporte_" + or.Id.ToString();
                lReport.Text = "<a href=\"ImpresionOrdenReparacion.aspx?a=1&or=" + or.Id.ToString() + "&prp=" + or.NumeroPRP.ToString() + "\"" + "target =\"_blank\"" + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lReport.Text += "<span class=\"shortcut-icon icon-search\"></span>";
                lReport.Text += "</a>";

                celAccion.Controls.Add(lReport);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                Literal lObservacion = new Literal();
                lObservacion.ID = "btnObservacion_" + or.Id.ToString();
                lObservacion.Text = "<a href=\"OrdenReparacionObservacionesABM.aspx?or=" + or.Id.ToString() + "\"" + "target =\"_blank\"" + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lObservacion.Text += "<span class=\"shortcut-icon icon-comment\"></span>";
                lObservacion.Text += "</a>";

                celAccion.Controls.Add(lObservacion);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + or.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                tr.Cells.Add(celAccion);

                phOrdenReparacion.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }

        public int CalcularProgressBar(DateTime fechaCompra,int plazoReparacion)
        {
            try
            {
                return (Convert.ToInt32((DateTime.Today - fechaCompra).TotalDays) * 100) / plazoReparacion;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al calcular la progress bar " + ex.Message);
                return -1;
            }
        }

        private void cargarEnPhServiceOficial(DataRow dr)
        {
            try
            {
                var marcas = contServTecnico.ObtenerMarcasByIDServicioTecnico(Convert.ToInt32(dr["Id"].ToString()));

                //fila
                TableRow tr = new TableRow();
                tr.ID = dr["Id"].ToString();

                //Celdas

                TableCell celNombre = new TableCell();
                celNombre.Text = dr["Nombre"].ToString();
                celNombre.HorizontalAlign = HorizontalAlign.Left;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celDireccion = new TableCell();
                celDireccion.Text = dr["Direccion"].ToString();
                celDireccion.HorizontalAlign = HorizontalAlign.Left;
                celDireccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDireccion);

                TableCell celObservaciones = new TableCell();
                celObservaciones.Text = dr["Observaciones"].ToString();
                celObservaciones.HorizontalAlign = HorizontalAlign.Left;
                celObservaciones.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celObservaciones);

                TableCell celMarcas = new TableCell();
                for (int i = 0; i < marcas.Count; i++)
                {
                    if (i == marcas.Count - 1)
                        celMarcas.Text += marcas[i].descripcion;
                    else
                        celMarcas.Text += marcas[i].descripcion + ", ";
                }
                celMarcas.HorizontalAlign = HorizontalAlign.Left;
                celMarcas.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMarcas);

                TableCell celAccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + dr["Id"].ToString();
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                tr.Cells.Add(celAccion);

                phServicioTecnico.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }

        //private void DetalleOrdenReparacion(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //obtengo numero factura
        //        string idBoton = (sender as LinkButton).ID;
        //        string[] atributos = idBoton.Split('_');
        //        string idOrdenReparacion = atributos[1];

        //        OrdenReparacion or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idOrdenReparacion));

        //        or.Estado = 0;

        //        var temp = contOrdenReparacion.ModificarOrdenReparacion();

        //        if (temp > 0)
        //        {
        //            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Pongo orden de reparacion en estado 0 " + or.Id);
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación eliminada con exito!", null));
        //        }
        //        else if(temp == -1)
        //        {
        //            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al modificar orden de reparación. " + or.Id);
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al modificar orden de reparación"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
        //        Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
        //    }
        //}

        public void CargarOrdenesReparacion()
        {
            try
            {
                phOrdenReparacion.Controls.Clear();

                var desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR")); //Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                var hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")); //Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));
                var sucursal = this.sucursal; //Convert.ToInt32(DropListSucursal.SelectedValue);
                var sucursalOR = this.sucursalOR; //Convert.ToInt32(DropListSucursal.SelectedValue);
                var cliente = this.cliente; //Convert.ToInt32(DropListClientes.SelectedValue);
                var articulo = this.articulo; //Convert.ToInt32(DropListClientes.SelectedValue);
                //var estado = Convert.ToInt32(DropListEstados.SelectedValue);

                var ordenesReparacion = contOrdenReparacion.ObtenerOrdenesReparacionFiltro(desde,hasta,sucursal,cliente, estado, sucursalOR,articulo);

                foreach (var item in ordenesReparacion)
                {
                    cargarEnPh(item);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar ordenes de reparacion por filtro. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al cargar ordenes de reparacion por filtro. " + ex.Message);
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                idtildado = ObtenerIdTildadoOrdenReparacion();

                if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),6))
                    return;

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));
                or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(6).Id;

                var temp = contOrdenReparacion.ModificarOrdenReparacion();

                if (temp > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Anulo orden de reparacion " + or.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación anulada con exito!", ParametrosFiltrar()));
                }
                else if (temp == -1)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al anular orden de reparación. " + or.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al anular orden de reparación"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al anular orden de reparacion. " + ex.Message);
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("OrdenReparacionF.aspx?a=0&c=" + this.DropListClientes.Text + "&s=" + this.DropListSucursal.Text + "&e=" + this.DropListEstados.Text + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&art=" + this.DropListArticulo.Text);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarSucursalOR()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursalOR.DataSource = dt;
                this.DropListSucursalOR.DataValueField = "Id";
                this.DropListSucursalOR.DataTextField = "nombre";
                this.DropListSucursalOR.DataBind();

                this.DropListSucursalOR.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales OR. " + ex.Message));
            }
        }

        public void cargarArticulos()
        {
            try
            {
                controladorArticulo contArticulo = new controladorArticulo();
                DataTable dt = contArticulo.obtenerArticulos2();

                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Todos";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListArticulo.DataSource = dt;
                this.DropListArticulo.DataValueField = "id";
                this.DropListArticulo.DataTextField = "descripcion";
                this.DropListArticulo.DataBind();

                this.DropListArticulo.SelectedValue = Session["Login_SucUser"].ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Articulos. " + ex.Message));
            }
        }
        public void cargarEstados()
        {
            try
            {
                var listEstados = contOrdenReparacion.ObtenerEstadosOrdenReparacion();

                listEstados = listEstados.Where(x => x.Id != 6).ToList(); //6 es anuladas

                listEstados.Insert(0, new OrdenReparacion_Estados
                {
                    Id = 0,
                    Descripcion = "Todos"
                });

                listEstados.Insert(1, new OrdenReparacion_Estados
                {
                    Id = 1,
                    Descripcion = "Vencidas"
                });

                this.DropListEstados.DataSource = listEstados;
                this.DropListEstados.DataValueField = "Id";
                this.DropListEstados.DataTextField = "Descripcion";
                this.DropListEstados.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();

                dt = contCliente.obtenerClientesDT();                

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr1 = dt.NewRow();
                dr1["alias"] = "Todos";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 1);

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

        protected void btnBuscarNumeros_Click(object sender, EventArgs e)
        {
            try
            {
                //hago esto para sacarle los 0 que tiene a la izquierda
                int numeroOrden = Convert.ToInt32(this.txtNumeroOrden.Text);
                Response.Redirect("OrdenReparacionF.aspx?a=1&n=" + numeroOrden.ToString());
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al buscar orden de reparacion por numero de orden. " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al buscar orden de reparacion por numero de orden. " + ex.Message));
            }
        }

        public void BuscarPorNumeroOrden()
        {
            try
            {
                OrdenReparacion or = contOrdenReparacion.ObtenerOrdenReparacionPorNumeroOrden(numeroOrden);
                this.phOrdenReparacion.Controls.Clear();
                this.cargarEnPh(or);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al buscar orden de reparacion por numero de orden. " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al buscar orden de reparacion por numero de orden. " + ex.Message));
            }
        }

        protected void btnGuardarServiceOficial_Click(object sender, EventArgs e)
        {
            try
            {
                //chequeo si hay un servicio tecnico seleccionado
                if (ComprobarServicioTecnicoTildado())
                {
                    string idtildado = "";

                    //compruebo si hay una sola orden de reparacion tildada
                    if (ComprobarOrdenReparacionTildada())
                    {
                        idtildado = ObtenerIdTildadoOrdenReparacion();

                        if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),4))
                            return;

                        int temp;

                        //obtengo la orden de reparacion
                        var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));                        

                        //linqueo la orden de reparacion con el servicio tecnico
                        OrdenReparacion_ServicioTecnico or_st = new OrdenReparacion_ServicioTecnico();

                        or_st.IdOrdenReparacion = or.Id;
                        or_st.IdServicioTecnico = Convert.ToInt32(lblIdServicioTecnico.Text);

                        var tempOr_St = contOrdenReparacion.AgregarOrdenReparacion_ServicioTecnico(or_st);

                        if (tempOr_St > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Servicio tecnico seleccionado con exito!");
                        }
                        else if (tempOr_St == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al seleccionar servicio tecnico.");
                        }

                        //chequeo si estaba en reparacion local para entonces restar el stock de la sucursal de reparacion
                        //if (or.Estado == 3 || or.Estado == 7 || or.Estado == 9)
                        //{
                        //    string comentario = "Descuento stock por envio a servicio tecnico: " + contServTecnico.ObtenerServicioTecnicoByID((int)or_st.IdServicioTecnico).Nombre + ". Orden de reparacion: " + or.NumeroOrdenReparacion.Value.ToString("D8");
                        //    int r = contOrdenReparacion.EliminarStockSucursalReparacion((int)Session["Login_IdUser"], or, comentario);
                        //    if (r < 1)
                        //    {
                        //        Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error al eliminar stock por envio a servicio tecnico!");
                        //    }
                        //}

                        //creo la observacion
                        AgregarObservacion(or.Id, "Servicio tecnico asignado a la orden de reparacion");

                        //le cambio el estado a la orden de reparacion, la modifico a lo ultimo asi lo redirijo a la misma pagina y evito problemas
                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(4).Id;

                        temp = contOrdenReparacion.ModificarOrdenReparacion();

                        if (temp > 0)
                        {                           

                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Servicio tecnico seleccionado con exito!");
                            //Session["Login_idcliente"] = or.Cliente;
                            //Session["Login_idArticulo"] = or.Producto;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Servicio tecnico seleccionado con exito!", ParametrosFiltrar()));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al seleccionar servicio tecnico.");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al seleccionar servicio tecnico."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al enviar orden de reparacion al service oficial. " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparacion al service oficial. " + ex.Message));
            }
        }

        public bool ComprobarOrdenReparacionTildada()
        {
            try
            {
                int tildados = 0;

                foreach (Control C in phOrdenReparacion.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[12].Controls[6] as CheckBox;

                    if (ch.Checked == true)
                        tildados++;
                }

                if (tildados <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una orden de reparacion!"));
                    return false;
                }
                if (tildados > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo una orden de reparacion!"));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error comprobando si hay una sola orden de reparacion seleccionada. " + ex.Message);
                return false;
            }
        }

        public bool ComprobarServicioTecnicoTildado()
        {
            try
            {
                //chequeo si hay servicios tecnicos tildados, si hay guardo el id en un label invisible

                int tildados = 0;

                foreach (Control C in phServicioTecnico.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[0] as CheckBox;

                    if (ch.Checked == true)
                    {
                        tildados++;
                        lblIdServicioTecnico.Text = ch.ID.Split('_')[1];
                    }
                }

                if (tildados <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un servicio tecnico!"));
                    return false;
                }
                if (tildados > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar solo un servicio tecnico!"));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error comprobando si hay una sola orden de reparacion seleccionada. " + ex.Message);
                return false;
            }
        }

        protected void btnSiDevolucionProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                //compruebo si hay una sola orden de reparacion tildada
                if (ComprobarOrdenReparacionTildada())
                {
                    idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),5))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));
                    or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(5).Id;

                    var temp = contOrdenReparacion.ModificarOrdenReparacion();

                    AgregarObservacion(or.Id, "Se envia producto al proveedor");

                    if (temp > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Orden de reparacion enviada a proveedor " + or.Id);
                        //Session["Login_idcliente"] = or.Cliente;
                        //Session["Login_idArticulo"] = or.Producto;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación enviada a proveedor con exito!", "../Compras/RemitosABM.aspx?a=1&or=1&orID=" + or.Id));
                    }
                    else if (temp == -1)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al enviar orden de reparación al proveedor. " + or.Id);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparación al proveedor."));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al enviar orden de reparacion al proveedor. " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparacion al proveedor. " + ex.Message));
            }
        }

        protected void btnSiEnviarAReparacionLocalmente_Click(object sender, EventArgs e)
        {
            string idtildado = "";

            //compruebo si hay una sola orden de reparacion tildada
            if (ComprobarOrdenReparacionTildada())
            {
                idtildado = ObtenerIdTildadoOrdenReparacion();

                if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),9))
                    return;

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));
                or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(9).Id;
                or.SucursalOR = contSucursal.obtenerSucursalID(Convert.ToInt32(contConfig.ObtenerConfiguracionId(51))).id;

                var temp = contOrdenReparacion.ModificarOrdenReparacion();

                AgregarObservacion(or.Id, "Se envia producto a sucursal de reparacion");

                if (temp >= 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Orden de reparacion enviada a sucursal de reparacion " + or.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación enviada con exito a sucursal de reparacion!", ParametrosFiltrar()));
                    //Session["Login_idcliente"] = or.Cliente;//TODO porque esta info se guarda en la session??
                    //Session["Login_idArticulo"] = or.Producto;//TODO porque esta info se guarda en la session?? para que se usa?

                    //temp = contOrdenReparacion.AgregarStockSucursalReparacion((int)Session["Login_IdUser"],or);

                    //if(temp > 0)
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación enviada con exito a sucursal de reparacion!", "OrdenReparacionF.aspx"));
                    //else
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparación a sucursal de reparacion."));
                }
                else if (temp == -2)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al enviar orden de reparación a sucursal de reparacion. " + or.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparación a sucursal de reparacion."));
                }
            }
        }
        
        /// <summary>
        ///este metodo es el boton de la lupita cuando se selecciona el servicio tecnico, 
        ///al apretar la lupita se vuelve a cargar la pagina y en el page load se guarda la opcion tildada, NO BORRAR ESTE METODO 
        /// </summary>
        protected void btnBuscarServicioTecnico_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al buscar servicio tecnico. " + ex.Message);
            }
        }

        
        public void ObtenerServiciosTecnicos(string buscar)
        {
            try
            {
                var serviciosTecnicos = contServTecnico.ObtenerServiciosTecnicosByCampoDT(buscar);

                foreach (DataRow item in serviciosTecnicos.Rows)
                {
                    cargarEnPhServiceOficial(item);
                }

                UpdatePanel7.Update();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al buscar servicio tecnico. " + ex.Message);
            }
        }

        protected void btnAgregarOrdenReparacionServicioTecnico_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string idtildado = ObtenerIdTildadoOrdenReparacion();

                if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),8))
                    return;

                int temp;
                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                //obtengo OrdenReparacion_ServicioTecnico
                OrdenReparacion_ServicioTecnico or_st = contOrdenReparacion.ObtenerOrdenReparacion_ServicioTecnicoPorORdenReparacionID(Convert.ToInt32(or.Id));

                //asi chequeo si previamente se le asigno un servicio tecnico a la orden de reparacion
                if (or_st != null)
                {
                    var servTecnico = contServTecnico.ObtenerServicioTecnicoByID((int)or_st.IdServicioTecnico);

                    or_st.Fecha = Convert.ToDateTime(txtFechaReparar.Text, new CultureInfo("es-AR"));
                    or_st.NumeroOrden = txtNumOrdenReparacion.Text;
                    or_st.PlazoReparacion = Convert.ToInt32(txtPlazoEstimadoReparacion.Text);

                    temp = contOrdenReparacion.ModificarOrdenReparacion_ServicioTecnico();

                    if (temp > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifique correctamente OrdenReparacion_ServicioTecnico, le asigne fecha, numero de orden y plazo reparacion");
                    }
                    else if (temp == -1)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al modificar OrdenReparacion_ServicioTecnico.");
                    }

                    contOrdenReparacion.EliminarStockSucursal((int)Session["Login_IdUser"], or, "Descuento stock por envio a sucursal de service oficial",51);
                    temp = contOrdenReparacion.AgregarStockSucursal((int)Session["Login_IdUser"], or,56);

                    if (temp > 0)
                    {
                        or.SucursalOR = contSucursal.obtenerSucursalID(Convert.ToInt32(contConfig.ObtenerConfiguracionId(56))).id;

                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(8).Id;

                        temp = contOrdenReparacion.ModificarOrdenReparacion();

                        AgregarObservacion(or.Id, "Producto enviado al servicio tecnico: " + servTecnico.Nombre + " Direccion: " + servTecnico.Direccion + ", numero de orden: " + txtNumOrdenReparacion.Text);

                        if (temp > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se envio la orden de reparacion al service oficial correctamente!");
                            //string script = "window.open('ABMRemitos.aspx?accion=5&cliente=" + or.Cliente + "&articulo=" + or.Producto + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');";
                            //script += " $.msgbox(\"Se envio la orden de reparacion al service oficial correctamente!. \", {type: \"info\"}); location.href = '" + ParametrosFiltrar() + "';";
                            //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se envio la orden de reparacion al service oficial correctamente!", "../Facturas/ABMRemitos.aspx?accion=5&cliente=" + servTecnico.Cliente + "&articulo=" + or.Producto));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al enviar orden de reparacion al servicio tecnico.");
                            //string script = " $.msgbox(\"Error al enviar orden de reparacion al servicio tecnico. \", {type: \"error\"});";
                            //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "error", script, true);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparacion al servicio tecnico."));
                        }
                    }                    
                }
                else
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Falta asignarle un servicio tecnico a la orden de reparacion.");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Falta asignarle un servicio tecnico a la orden de reparacion."));
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al enviar orden de reparacion al servicio tecnico. " + ex.Message);
            }
        }

        protected void btnSiReparacionLocalmente_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                //compruebo si hay una sola orden de reparacion tildada
                if (ComprobarOrdenReparacionTildada())
                {
                    idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),7))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    if (or.Estado == 9)
                    {
                        //var temp = contOrdenReparacion.AgregarStockSucursalReparacion((int)Session["Login_IdUser"], or);

                        //if (temp < 1)
                        //        Log.EscribirSQL(1, "ERROR", "Error al agregar stock en la sucursal de reparacion");

                        AgregarObservacion(or.Id, "Producto en reparacion");

                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(7).Id;
                        var temp = contOrdenReparacion.ModificarOrdenReparacion();

                        if (temp > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El producto se encuentra en reparacion");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto se encuentra en reparacion!", ParametrosFiltrar()));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al poner el producto en reparacion.");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al poner el producto en reparacion."));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El producto no fue enviado a la sucursal de reparacion!"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al cambiar de estado de orden de reparacion a \"En reparacion\". " + ex.Message);
            }
        }

        public string ObtenerIdTildadoOrdenReparacion()
        {
            try
            {
                foreach (Control C in phOrdenReparacion.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[12].Controls[6] as CheckBox;
                    if (ch.Checked == true)
                    {
                        return ch.ID.Split('_')[1];
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al obtener id tildado de la orden de reparacion. " + ex.Message);
                return string.Empty;
            }
        }

        protected void btnReparado_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                //compruebo si hay una sola orden de reparacion tildada
                if (ComprobarOrdenReparacionTildada())
                {
                    idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),3))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    if (or.Estado == 7)
                    {
                        AgregarObservacion(or.Id, "Reparacion finalizada");

                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(3).Id;
                        var temp = contOrdenReparacion.ModificarOrdenReparacion();

                        if (temp > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El producto se encuentra reparado");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto se encuentra reparado!", ParametrosFiltrar()));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al poner el producto en reparacion.");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al poner el producto en estado reparado."));
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El producto debe encontrarse reparado en la sucursal de reparacion!"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al cambiar el estado del producto a reparado. " + ex.Message);
            }
        }

        protected void btnDevolverASucursalOrigen_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                //compruebo si hay una sola orden de reparacion tildada
                if (ComprobarOrdenReparacionTildada())
                {
                    idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),10))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    //string comentario = "Resto stock por producto reparado, se envia a sucursal: " + contSucursal.obtenerSucursalID((int)or.SucursalOrigen).nombre + ". OR: " + or.NumeroOrdenReparacion;

                    //contOrdenReparacion.EliminarStockSucursalReparacion((int)Session["Login_IdUser"], or, comentario);

                    or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(10).Id;
                    or.SucursalOR = or.SucursalOrigen;
                    var temp = contOrdenReparacion.ModificarOrdenReparacion();

                    AgregarObservacion(or.Id, "Producto enviado a la sucursal de origen");

                    if (temp > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El producto fue devuelto a la sucursal de origen");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto fue devuelto a la sucursal de origen!", ParametrosFiltrar()));
                    }
                    else if (temp == -1)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al devolver el producto a la sucursal de origen.");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al devolver el producto a la sucursal de origen."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El producto debe encontrarse reparado!"));
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al devolver el producto a la sucursal de origen. " + ex.Message);
            }
        }

        protected void lbtnEtiqueta_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarOrdenReparacionTildada())
                {
                    string idtildado = ObtenerIdTildadoOrdenReparacion();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionOrdenReparacion.aspx?a=2&or=" + idtildado + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una orden de reparacion!"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al generar etiqueta. " + ex.Message);
            }
        }

        protected void lbtnEnviarSMS_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarOrdenReparacionTildada())
                {
                    ImpresionOrdenReparacion imprimirOR = new ImpresionOrdenReparacion();

                    Configuraciones_SMS configs = this.contConfig.ObtenerConfiguracionesAlertasSMS();
                    string idtildado = ObtenerIdTildadoOrdenReparacion();
                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));
                    var cliente = contCliente.obtenerClienteID((int)or.Cliente);

                    if (or.CambiaProducto == "Si")
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Al cliente ya se le entrego un producto nuevo"));
                    }
                    else
                    {
                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(11).Id;
                        contOrdenReparacion.ModificarOrdenReparacion();

                        AgregarObservacion(or.Id, "Mensaje y mail enviados al cliente");

                        var temp = contOrdenReparacion.EnviarSMSProductoReparado(or.Celular, configs.MensajeProductoReparado, cliente.razonSocial, (int)Session["Login_IdUser"]);

                        if (temp > 0)
                        {
                            imprimirOR.enviarPorMail = 1;
                            imprimirOR.idPresupuesto = (int)or.NumeroPRP;
                            imprimirOR.ordenReparacion = (int)or.Id;
                            temp = imprimirOR.GenerarImpresion();

                            if(temp > 0)
                            {
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Mensaje y mail de producto reparado enviado correctamente");
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Mensaje y mail de producto reparado enviado correctamente!", ParametrosFiltrar()));
                            }                            
                        }
                        else
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al enviar mensaje y mail de producto reparado.");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar mensaje y mail de producto reparado."));
                        }
                    }
                        
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al enviar SMS " + ex.Message);
            }
        }

        public void AgregarObservacion(int orID, string mensaje)
        {
            try
            {
                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(orID));

                string sucursalOR = contSucursal.obtenerSucursalID((int)or.SucursalOR).nombre;

                var temp = contOrdenReparacion.AgregarObservacionOrdenReparacion(orID, (int)Session["Login_IdUser"], mensaje + ". Sucursal OR: " + sucursalOR);

                if (temp > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agregue correctamente la observacion a la orden de reparacion. ID: " + orID);
                }
                else if (temp == -1)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al agregar observacion a la orden de reparacion. ID: " + orID);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al agregar observacion " + ex.Message);
            }
        }

        protected void btnRetiraCliente_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarOrdenReparacionTildada())
                {
                    string idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),2))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));     
                                   
                    if(or.CambiaProducto == "Si")
                    {
                        string script = " $.msgbox(\"Al cliente ya se le entrego un producto nuevo. \", {type: \"alert\"});";
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                    }
                    else
                    {
                        string comentario = "Resto stock por entregar producto reparado al cliente. " + "OR: " + or.NumeroOrdenReparacion.Value.ToString("D8");

                        contOrdenReparacion.EliminarStockSucursal((int)Session["Login_IdUser"], or, comentario,51);

                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(2).Id;
                        or.FechaFinalizacion = DateTime.Now;
                        or.LapsoFinalizacion = CalcularProgressBar((DateTime)or.Fecha, (int)or.PlazoLimiteReparacion);
                        or.SucursalOR = or.SucursalOrigen;
                        var temp = contOrdenReparacion.ModificarOrdenReparacion();

                        AgregarObservacion(or.Id, "El producto fue retirado por el cliente");

                        AgregarObservacion(or.Id, txtObservacionRetiraCliente.Text);

                        if (temp > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El producto fue retirado por el cliente");
                            string script = "window.open('ImpresionOrdenReparacion.aspx?a=1&or=" + or.Id.ToString() + "&prp=" + or.NumeroPRP.ToString() + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');";
                            script += " $.msgbox(\"El producto fue retirado por el cliente. \", {type: \"info\"}); location.href = '" + ParametrosFiltrar() + "';";
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto fue retirado por el cliente!", "OrdenReparacionF.aspx"));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al cambiar al estado retirado por el cliente.");
                            string script = " $.msgbox(\"Error al cambiar al estado retirado por el cliente. \", {type: \"error\"});";
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "error", script, true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cambiar al estado retirado por el cliente."));
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al cambiar al estado retirado por el cliente " + ex.Message);
            }
        }

        protected void btnFinalizada_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarOrdenReparacionTildada())
                {
                    string idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),12))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    AgregarObservacion(or.Id, "Orden de reparacion finalizada");

                    AgregarObservacion(or.Id, txtObservacionFinalizada.Text);

                    string comentario = "Resto stock por orden de reparacion finalizada. " + "OR: " + or.NumeroOrdenReparacion.Value.ToString("D8");

                    contOrdenReparacion.EliminarStockSucursal((int)Session["Login_IdUser"], or, comentario,51);

                    or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(12).Id;
                    or.FechaFinalizacion = DateTime.Now;
                    or.LapsoFinalizacion = CalcularProgressBar((DateTime)or.Fecha, (int)or.PlazoLimiteReparacion);
                    or.SucursalOR = or.SucursalOrigen;
                    var temp = contOrdenReparacion.ModificarOrdenReparacion();

                    if (temp >= 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Orden de reparacion finalizada " + or.Id);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación finalizada con exito!", ParametrosFiltrar()));
                    }
                    else if (temp == -2)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al finalizar orden de reparación. " + or.Id);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al finalizar orden de reparación."));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al finalizar orden de reparacion. " + ex.Message);
            }
        }

        protected void btnSucOrigenRecibeMercaderia_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                //compruebo si hay una sola orden de reparacion tildada
                if (ComprobarOrdenReparacionTildada())
                {
                    idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),14))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    if (or.Estado == 10)
                    {
                        AgregarObservacion(or.Id, "Producto reparado recibido en la sucursal de origen");

                        //string comentario = "Resto stock por producto reparado, se envia a sucursal: " + contSucursal.obtenerSucursalID((int)or.SucursalOrigen).nombre + ". OR: " + or.NumeroOrdenReparacion;

                        //contOrdenReparacion.EliminarStockSucursalReparacion((int)Session["Login_IdUser"], or, comentario);

                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(14).Id;
                        or.SucursalOR = or.SucursalOrigen;
                        var temp = contOrdenReparacion.ModificarOrdenReparacion();

                        if (temp > 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "El producto reparado fue recibido en la sucursal de origen");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto reparado fue recibido en la sucursal de origen!", ParametrosFiltrar()));
                        }
                        else if (temp == -1)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al recibir el producto reparado en la sucursal de origen.");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al recibir el producto reparado en la sucursal de origen."));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El producto debe haberse enviado a la sucursal de origen previamente!"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al recibir el producto en la sucursal de origen. " + ex.Message);
            }
        }

        protected void btnEnviarASucursalGarantias_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComprobarOrdenReparacionTildada())
                {
                    string idtildado = ObtenerIdTildadoOrdenReparacion();

                    if (ChequearSiORFinalizo(Convert.ToInt32(idtildado),13))
                        return;

                    var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(Convert.ToInt32(idtildado));

                    contOrdenReparacion.EliminarStockSucursal((int)Session["Login_IdUser"], or, "Descuento stock por envio a sucursal de servicio tecnico", 56);
                    var temp = contOrdenReparacion.AgregarStockSucursal((int)Session["Login_IdUser"], or, 51);

                    if(temp > 0)
                    {
                        or.Estado = contOrdenReparacion.ObtenerEstadoOrdenReparacionPorID(13).Id;
                        or.FechaFinalizacion = DateTime.Now;
                        or.LapsoFinalizacion = CalcularProgressBar((DateTime)or.Fecha, (int)or.PlazoLimiteReparacion);
                        or.SucursalOR = contSucursal.obtenerSucursalID(Convert.ToInt32(contConfig.ObtenerConfiguracionId(51))).id;

                        temp = contOrdenReparacion.ModificarOrdenReparacion();

                        AgregarObservacion(or.Id, "El producto fue reparado por el service oficial y fue enviado a la sucursal de servicio tecnico");

                        if (temp >= 0)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Orden de reparacion enviada a la sucursal de servicio tecnico " + or.Id);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El producto reparado por el service oficial fue recibido en la sucursal de servicio tecnico!", ParametrosFiltrar()));
                        }
                        else if (temp == -2)
                        {
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Error", "Error al enviar orden de reparación a la sucursal de servicio tecnico. " + or.Id);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al enviar orden de reparación a la sucursal de servicio tecnico."));
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al cambiar al estado: Reparado por Service Oficial - Enviado a Garantias " + ex.Message);
            }
        }
        //Le paso por parametro el Estado de la orden y un estado a comparar en caso de que coincidan significa que ya se encuentra en ese estado
        public bool ChequearSiORFinalizo(int idOR,int Opc)
        {
            try
            {
                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(idOR);
                if(or.Estado== Opc)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden ya se encuentra en el estado al que se quiere modificar"));
                    return true;
                }
                if (or.Estado == 2 || or.Estado == 12)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La orden de reparacion ya se encuentra finalizada!"));
                    return true;
                }                    

                return false;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error chequeando si la OR finalizo " + ex.Message);
                return false;
            }
        }

        public string ParametrosFiltrar()
        {
            try
            {
                string filtro = "OrdenReparacionF.aspx?a=0&c=" + this.cliente + "&s=" + this.sucursal + "&e=" + this.estado + "&fd=" + this.fechaD + "&fh=" + this.fechaH + "&art=" + this.articulo;

                return filtro;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar por parametros. " + ex.Message);
                return string.Empty;
            }
        }

        protected void btnBuscarCodArt_Click(object sender, EventArgs e)
        {
            try
            {
                controladorArticulo contArticulo = new controladorArticulo();

                String buscar = this.txtDescArticulo.Text.Replace(' ', '%');
                DataTable dt = contArticulo.obtenerArticulosByDescDT(buscar);

                //cargo la lista
                this.DropListArticulo.DataSource = dt;
                this.DropListArticulo.DataValueField = "id";
                this.DropListArticulo.DataTextField = "descripcion";
                this.DropListArticulo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
    }
}