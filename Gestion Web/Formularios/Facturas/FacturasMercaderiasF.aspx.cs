using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class FacturasMercaderiasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFactEntity contFactEntity = new controladorFactEntity();

        int accion = 0;
        int sucursalDestino = 0;
        int sucursalOrigen = 0;
        int estado = 0;
        string fechaD = "";
        string fechaH = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            accion = Convert.ToInt32(Request.QueryString["a"]);
            sucursalDestino = Convert.ToInt32(Request.QueryString["sd"]);
            sucursalOrigen = Convert.ToInt32(Request.QueryString["so"]);
            estado = Convert.ToInt32(Request.QueryString["e"]);
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(fechaD))
                {
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaD = txtFechaDesde.Text;
                }                    
                else
                    txtFechaDesde.Text = fechaD.ToString(new CultureInfo("es-AR"));

                if (string.IsNullOrEmpty(fechaH))
                {
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = txtFechaHasta.Text;
                }                    
                else
                    txtFechaHasta.Text = fechaH.ToString(new CultureInfo("es-AR"));

                sucursalDestino = Convert.ToInt32(Session["Login_SucUser"]);
                estado = 1;

                cargarSucursales();
                cargarEstados();
                CargarFacturasMercaderias();
            }

            if (accion == 1)
                CargarFacturasMercaderias();
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
                int tienePermiso = 0;
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "163")
                            tienePermiso = 1;
                        
                        if(tienePermiso == 1)
                        {
                            if(s == "165")
                            {
                                this.DropListSucursalDestino.Enabled = true;
                            }
                        }
                    }
                }

                if(tienePermiso == 1)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursalOrigen.DataSource = dt;
                this.DropListSucursalOrigen.DataValueField = "Id";
                this.DropListSucursalOrigen.DataTextField = "nombre";
                this.DropListSucursalOrigen.DataBind();

                this.DropListSucursalDestino.DataSource = dt;
                this.DropListSucursalDestino.DataValueField = "Id";
                this.DropListSucursalDestino.DataTextField = "nombre";
                this.DropListSucursalDestino.DataBind();

                this.DropListSucursalDestino.SelectedValue = Session["Login_SucUser"].ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarEstados()
        {
            try
            {
                var dt = contFactEntity.ObtenerFacturasMercaderias_Estados();

                this.DropListEstados.DataSource = dt;
                this.DropListEstados.DataValueField = "Id";
                this.DropListEstados.DataTextField = "Descripcion";
                this.DropListEstados.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FacturasMercaderiasF.aspx?a=1&sd=" + this.DropListSucursalDestino.SelectedValue + "&so=" + DropListSucursalOrigen.SelectedValue + "&fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&e=" + DropListEstados.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
            }
        }

        public void CargarFacturasMercaderias()
        {
            try
            {
                phFacturas.Controls.Clear();

                var facturas = contFactEntity.ObtenerFacturasYFacturasMercaderias(estado, sucursalOrigen, sucursalDestino, Convert.ToDateTime(fechaD, new CultureInfo("es-AR")), Convert.ToDateTime(fechaH, new CultureInfo("es-AR")));

                foreach (DataRow factura in facturas.Rows)
                {
                    cargarEnPh(factura);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar facturas mercaderias por filtro. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al cargar facturas mercaderias por filtro. " + ex.Message);
            }
        }
        private void cargarEnPh(DataRow f)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();

                //fila
                TableRow tr = new TableRow();
                tr.ID = f["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                DateTime date = Convert.ToDateTime(f["fecha"].ToString());
                celFecha.Text = date.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumeroFactura = new TableCell();
                celNumeroFactura.Text = f["numero"].ToString();
                celNumeroFactura.HorizontalAlign = HorizontalAlign.Left;
                celNumeroFactura.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroFactura);

                TableCell celSucursalOrigen = new TableCell();
                var idSuc = Convert.ToInt32(f["Id_Suc"].ToString());
                celSucursalOrigen.Text = contSucursal.obtenerSucursalID(idSuc).nombre;
                celSucursalOrigen.HorizontalAlign = HorizontalAlign.Left;
                celSucursalOrigen.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalOrigen);

                TableCell celEstado = new TableCell();
                celEstado.Text = contFactEntity.ObtenerFacturasMercaderias_EstadoByID(Convert.ToInt32(f["Estado"].ToString())).Descripcion;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEstado);

                TableCell celAccion = new TableCell();

                Literal lAccept = new Literal();
                lAccept.ID = "btnFactura_" + f["id"].ToString();
                lAccept.Text = "<a href=\"AceptarMercaderia.aspx?fc=" + f["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lAccept.Text += "<span class=\"shortcut-icon icon-search\"></span>";
                lAccept.Text += "</a>";

                celAccion.Controls.Add(lAccept);

                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }
    }
}