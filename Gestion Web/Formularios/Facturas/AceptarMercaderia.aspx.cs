using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class AceptarMercaderia : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        controladorFactEntity contFactEntity = new controladorFactEntity();
        int fc = 0;
        int fm = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            fc = Convert.ToInt32(Request.QueryString["fc"]);
            fm = Convert.ToInt32(Request.QueryString["fm"]);

            btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

            if (!IsPostBack)
            {
                CargarDatosDeFactura();
            }

            CargarItemsFacturaEnPH();
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

                return 1;

                //foreach (string s in listPermisos)
                //{
                //    if (!String.IsNullOrEmpty(s))
                //    {
                //        if (s == "28")
                //        {
                //            return 1;
                //        }
                //    }
                //}

                //return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void CargarDatosDeFactura()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                var factura = contFacturacion.obtenerFacturaId(fc);
                var sucursalOrigen = contSucu.obtenerSucursalID(factura.sucursal.id);
                var sucursalDestino = contSucu.obtenerSucursalID(factura.sucursalFacturada);
                var fm = contFactEntity.ObtenerFacturas_MercaderiasByFacturaID(fc);

                if (fm.Estado > 1)
                    btnAgregar.Visible = false;

                ListSucursalOrigen.Items.Add(new ListItem
                {
                    Value = sucursalOrigen.id.ToString(),
                    Text = sucursalOrigen.nombre
                });
                ListSucursalOrigen.CssClass = "form-control";

                ListSucursalDestino.Items.Add(new ListItem
                {
                    Value = sucursalDestino.id.ToString(),
                    Text = sucursalDestino.nombre
                });
                ListSucursalDestino.CssClass = "form-control";

                string[] numero = factura.numero.Split('-');
                numero[0] = numero[0].Replace("-", string.Empty);

                txtPVenta.Text = numero[0];
                txtNumero.Text = numero[1];
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar los datos de la factura en pantalla " + ex.Message);
            }
        }

        public void CargarItemsFacturaEnPH()
        {
            try
            {
                var items = contFacturacion.obtenerItemsFact(fc);

                foreach (var item in items)
                {
                    cargarEnPh(item);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar los items de la factura en el PH " + ex.Message);
            }
        }

        private void cargarEnPh(ItemFactura fi)
        {
            try
            {
                var fm = contFactEntity.ObtenerFacturas_MercaderiasByFacturaID(fc);

                //fila
                TableRow tr = new TableRow();
                tr.ID = fi.Id.ToString() + "_" + fi.articulo.id;

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = fi.articulo.codigo;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = fi.articulo.descripcion;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                int cantidad = Convert.ToInt32(fi.cantidad);
                celCantidad.Text = cantidad.ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Left;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);

                TableCell celAccion = new TableCell();

                TextBox celCantidadRecibida = new TextBox();
                celCantidadRecibida.TextMode = TextBoxMode.Number;
                if (fm.Estado > 1)
                    celCantidadRecibida.Enabled = false;
                celCantidadRecibida.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                celCantidadRecibida.Text = cantidad.ToString();

                celAccion.Controls.Add(celCantidadRecibida);

                tr.Cells.Add(celAccion);

                phProductos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                bool hayDiferencia = false;

                List<FacturasMercaderias_Detalle> fcmDetallelista = new List<FacturasMercaderias_Detalle>();

                foreach (var item in phProductos.Controls)
                {
                    TableRow tr = item as TableRow;
                    TextBox txtCantidadRecibidaTB = tr.Cells[3].Controls[0] as TextBox;
                    int cantidadEnviada = Convert.ToInt32(tr.Cells[2].Text);
                    int cantidadRecibida = Convert.ToInt32(txtCantidadRecibidaTB.Text);

                    string[] tempTexts = tr.ID.Split('_');
                    int idItemFactura = Convert.ToInt32(tempTexts[0]);
                    int idArticulo = Convert.ToInt32(tempTexts[1]);
                    
                    FacturasMercaderias_Detalle fcmDetalle = new FacturasMercaderias_Detalle();

                    fcmDetalle.IdItemFactura = Convert.ToInt32(idItemFactura);
                    fcmDetalle.CantidadEnviada = cantidadEnviada;
                    fcmDetalle.CantidadRecibida = cantidadRecibida;
                    fcmDetalle.Diferencia = cantidadEnviada - cantidadRecibida;
                    fcmDetalle.Articulo = idArticulo;

                    if (fcmDetalle.Diferencia > 0)
                        hayDiferencia = true;

                    fcmDetallelista.Add(fcmDetalle);
                }

                int temp = contFactEntity.GuardarFacturasMercaderiasDetalles((int)Session["Login_IdUser"], fcmDetallelista, fc,hayDiferencia);

                if(temp > 0)
                {
                    Log.EscribirSQL(1, "Info", "Se guardaron los detalles de la mercaderia con exito");
                    string script = " $.msgbox(\"Mercaderia aceptada con exito! \", {type: \"info\"}); location.href = 'FacturasMercaderiasF.aspx'";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);                    
                }
                else
                {
                    Log.EscribirSQL(1, "Error", "Error al guardar los detalles de la mercaderia con exito");
                    string script = " $.msgbox(\"Error al guardar los detalles de la mercaderia! \", {type: \"error\"});";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al aceptar la mercaderia " + ex.Message);
            }
        }
    }
}