using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class DiferenciasMercaderiaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFactEntity contFactEntity = new controladorFactEntity();

        int accion = 0;
        int sucursalDestino = 0;
        int sucursalOrigen = 0;
        string fechaD = "";
        string fechaH = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            accion = Convert.ToInt32(Request.QueryString["a"]);
            sucursalDestino = Convert.ToInt32(Request.QueryString["sd"]);
            sucursalOrigen = Convert.ToInt32(Request.QueryString["so"]);
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];

            if (accion == 0)
            {
                PrimeraCarga();
            }

            if (!IsPostBack)
            {
                this.txtFechaDesde.Text = fechaD;
                this.txtFechaHasta.Text = fechaH;

                cargarSucursales();
            }

            if (accion == 1)
                CargarFacturasMercaderiasDiferencias();
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
                        if (s == "164")
                            tienePermiso = 1;

                        if (tienePermiso == 1)
                        {
                            if (s == "166")
                            {
                                this.DropListSucursalDestino.Enabled = true;
                            }
                        }
                    }
                }

                if (tienePermiso == 1)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        protected void PrimeraCarga()
        {
            try
            {
                fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                sucursalDestino = Convert.ToInt32(Session["Login_SucUser"]);

                FiltrarDiferenciaMercaderia();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al hacer la primera carga de diferencias mercaderias. " + ex.Message);
            }
        }

        protected void FiltrarDiferenciaMercaderia()
        {
            try
            {
                Response.Redirect("DiferenciasMercaderiaF.aspx?a=1&sd=" + sucursalDestino + "&so=" + sucursalOrigen + "&fd=" + fechaD + "&fh=" + fechaH);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
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

        public void CargarFacturasMercaderiasDiferencias()
        {
            try
            {
                phFacturas.Controls.Clear();

                var diferencias = contFactEntity.ObtenerFacturasMercaderiasDiferencias();

                foreach (var diferencia in diferencias)
                {
                    cargarEnPh(diferencia);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar facturas mercaderias diferencias. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al cargar facturas mercaderias diferencias. " + ex.Message);
            }
        }

        private void cargarEnPh(FacturasMercaderias_Diferencias f)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();
                var factura = contFacturacion.obtenerFacturaId((int)f.FacturasMercaderias_Detalle.Facturas_Mercaderias.Factura);
                //fila
                TableRow tr = new TableRow();
                //tr.ID = f["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                DateTime date = Convert.ToDateTime(f.FacturasMercaderias_Detalle.Facturas_Mercaderias.FechaFactura);
                celFecha.Text = date.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumeroFactura = new TableCell();
                celNumeroFactura.Text = factura.numero;
                celNumeroFactura.HorizontalAlign = HorizontalAlign.Left;
                celNumeroFactura.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroFactura);

                TableCell celSucursalOrigen = new TableCell();
                celSucursalOrigen.Text = contSucursal.obtenerSucursalID(factura.sucursal.id).nombre;
                celSucursalOrigen.HorizontalAlign = HorizontalAlign.Left;
                celSucursalOrigen.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalOrigen);

                TableCell celSucursalDestino = new TableCell();
                celSucursalDestino.Text = contSucursal.obtenerSucursalID(factura.sucursalFacturada).nombre;
                celSucursalDestino.HorizontalAlign = HorizontalAlign.Left;
                celSucursalDestino.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalDestino);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = f.FacturasMercaderias_Detalle.itemsFactura.articulo1.descripcion;
                celArticulo.HorizontalAlign = HorizontalAlign.Left;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell celEnviado = new TableCell();
                celEnviado.Text = f.FacturasMercaderias_Detalle.CantidadEnviada.ToString();
                celEnviado.HorizontalAlign = HorizontalAlign.Left;
                celEnviado.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEnviado);

                TableCell celRecibido = new TableCell();
                celRecibido.Text = f.FacturasMercaderias_Detalle.CantidadRecibida.ToString();
                celRecibido.HorizontalAlign = HorizontalAlign.Left;
                celRecibido.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRecibido);

                TableCell celDiferencia = new TableCell();
                celDiferencia.Text = f.FacturasMercaderias_Detalle.Diferencia.ToString();
                celDiferencia.HorizontalAlign = HorizontalAlign.Left;
                celDiferencia.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDiferencia);

                TableCell celEstado = new TableCell();
                celEstado.Text = contFactEntity.ObtenerFacturasMercaderias_EstadoByID((int)f.Estado).Descripcion;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEstado);

                TableCell celAccion = new TableCell();

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + f.FacturasMercaderias_Detalle.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Comentarios");
                btnDetalles.ID = "btnComent_" + f.Observaciones + "_" + f.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-comment'></span>";
                btnDetalles.Font.Size = 12;
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                btnDetalles.Click += new EventHandler(this.ComentariosCaja);
                celAccion.Controls.Add(btnDetalles);

                //celAccion.Controls.Add(lAccept);

                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando diferencias de mercaderia en el PH. " + ex.Message));
            }
        }
        private void ComentariosCaja(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string observacion = atributos[1];

                if (String.IsNullOrEmpty(observacion))
                {
                    observacion = "";
                }
                else
                {
                    observacion = Regex.Replace(observacion, @"\t|\n|\r", "");
                }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog('" + observacion + "')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar comentario. " + ex.Message));
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            CargarVariableConValoresDeFiltro();
            FiltrarDiferenciaMercaderia();
        }
        protected void CargarVariableConValoresDeFiltro()
        {
            try
            {
                fechaD = txtFechaDesde.Text;
                fechaH = txtFechaHasta.Text;
                sucursalOrigen = Convert.ToInt32(DropListSucursalDestino.SelectedValue);
                sucursalDestino = Convert.ToInt32(DropListSucursalDestino.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al hacer la primera carga de diferencias mercaderias. " + ex.Message);
            }
        }

        protected void btnSubirStockOrigen_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FacturaMercaderiaDetalleTildada())
                    return;

                List<int> idsFacturasMercaderias = ObtenerFacturasMercaderiasTildadas();

                string observacion = "Agrego stock en sucursal origen por diferencia al aceptar mercaderia. ";

                observacion += txtConfirmacionOrigen.Text;

                int i = contFactEntity.ProcesarSubirStockOrigen(idsFacturasMercaderias,"Agrego stock en sucursal origen por diferencia al aceptar mercaderia", (int)Session["Login_IdUser"], observacion,5);

                if(i>0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Stock agregado correctamente en sucursal origen.", "DiferenciasMercaderiaF.aspx?a=1&sd=" + sucursalDestino + "&so=" + sucursalOrigen + "&fd=" + fechaD + "&fh=" + fechaH));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al subir stock en sucursal origen."));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al subir stock en la sucursal de origen " + ex.Message);
            }
        }

        protected void btnSubirStockDestino_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FacturaMercaderiaDetalleTildada())
                    return;

                List <int> idsFacturasMercaderias = ObtenerFacturasMercaderiasTildadas();

                string observacion = "Agrego stock en sucursal destino por diferencia al aceptar mercaderia. ";

                observacion += txtConfirmacionDestino.Text;

                int i = contFactEntity.ProcesarSubirStockDestino(idsFacturasMercaderias, "Agrego stock en sucursal destino por diferencia al aceptar mercaderia", (int)Session["Login_IdUser"], observacion,5);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Stock agregado correctamente en sucursal destino.", "DiferenciasMercaderiaF.aspx?a=1&sd=" + sucursalDestino + "&so=" + sucursalOrigen + "&fd=" + fechaD + "&fh=" + fechaH));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al subir stock en sucursal destino."));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir stock en la sucursal de destino " + ex.Message);
            }
        }

        protected void btnResolucionAdministrativa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FacturaMercaderiaDetalleTildada())
                    return;


            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al realizar una resolucion administrativa " + ex.Message);
            }
        }

        public bool FacturaMercaderiaDetalleTildada()
        {
            try
            {
                int tildados = 0;

                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[9].Controls[0] as CheckBox;

                    if (ch.Checked == true)
                        tildados++;
                }

                if (tildados <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No hay diferencias seleccionadas!"));
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

        public List<int> ObtenerFacturasMercaderiasTildadas()
        {
            try
            {
                List<int> idsFacturasMercaderias = new List<int>();

                foreach (Control C in phFacturas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[9].Controls[0] as CheckBox;

                    if (ch.Checked == true)
                    {
                        idsFacturasMercaderias.Add(Convert.ToInt32(ch.ID.Split('_')[1]));
                    }
                }

                return idsFacturasMercaderias;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al obtener facturas diferencias tildadas " + ex.Message);
                return null;
            }
        }
    }
}