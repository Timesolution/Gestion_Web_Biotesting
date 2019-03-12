using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
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

    public partial class EntregasMercaderiaF : System.Web.UI.Page
    {
        controladorSucursal contSuc = new controladorSucursal();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCompraEntity contComprasEnt = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        Mensajes m = new Mensajes();
        long ordenCompra = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ordenCompra = Convert.ToInt64(Request.QueryString["oc"]);

            this.VerificarLogin();

            btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");

            if (!IsPostBack)
            {
                cargarProveedores();
                cargarSucursal();
                CargarDatosDesdeOrdenCompra();
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
                int cambiarSucursal = 0;
                int cambiarPuntovta = 0;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "167")
                            cambiarSucursal = 1;

                        if (s == "168")
                            cambiarPuntovta = 1;
                    }
                }

                if (cambiarSucursal == 1)
                    ListSucursal.Enabled = true;
                else
                    ListSucursal.Enabled = false;

                if (cambiarPuntovta == 1)
                    ListPtoVenta.Enabled = true;
                else
                    ListPtoVenta.Enabled = false;

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public void CargarDatosDesdeOrdenCompra()
        {
            try
            {
                var oc = contComprasEnt.obtenerOrden(ordenCompra);

                ListSucursal.SelectedValue = oc.IdSucursal.ToString();

                ListPtoVenta.CssClass = "form-control";

                cargarPuntoVta(Convert.ToInt32(ListSucursal.SelectedValue), oc.IdPtoVenta.Value);

                ListProveedor.SelectedValue = oc.IdProveedor.ToString();

                txtFechaMercaderiaArribo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaOC.Text = oc.FechaEntrega.Value.ToString("dd/MM/yyyy");
                txtFechaMercaderiaIngresada.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //this.txtNumero.Text = oc.Numero.ToString().PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error cargando datos desde la orden de compra" + ex.Message);
            }
        }

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();

                ListProveedor.Enabled = false;
                ListProveedor.CssClass = "form-control";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
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

                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

                ListSucursal.CssClass = "form-control";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVta(int sucu, int ptoventa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListPtoVenta.DataSource = dt;
                this.ListPtoVenta.DataValueField = "Id";
                this.ListPtoVenta.DataTextField = "NombreFantasia";

                this.ListPtoVenta.DataBind();

                ListPtoVenta.SelectedValue = ptoventa.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Pto Venta. " + ex.Message));
            }
        }

        public void CargarItemsFacturaEnPH()
        {
            try
            {
                var ordenCompra = contComprasEnt.obtenerOrden(this.ordenCompra);

                foreach (var item in ordenCompra.OrdenesCompra_Items)
                {
                    if (item.CantidadYaRecibida < item.Cantidad)
                    {
                        var diferenciasOrdenCompra = ordenCompra.RemitoCompraOrdenCompra_Diferencias.Where(x => x.OrdenCompra == item.IdOrden && x.Articulo.ToString() == item.Codigo).ToList();
                        cargarEnPh(item, diferenciasOrdenCompra);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar los items de la factura en el PH " + ex.Message);
            }
        }

        private void cargarEnPh(OrdenesCompra_Items ocItem, List<RemitoCompraOrdenCompra_Diferencias> diferencias = null)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = ocItem.Codigo.ToString();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = contArticulos.obtenerArticuloByID(Convert.ToInt32(ocItem.Codigo)).codigo;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = ocItem.Descripcion;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                decimal cantidad = Convert.ToDecimal(ocItem.Cantidad);
                celCantidad.Text = cantidad.ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Left;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);

                TableCell celCantidadYaRecibida = new TableCell();
                decimal cantidadYaRecibidas = ObtenerCantidadesYaRecibidas(diferencias);
                celCantidadYaRecibida.Text = "0";

                if(diferencias != null && diferencias.Count > 0)
                    celCantidadYaRecibida.Text = cantidadYaRecibidas.ToString();

                celCantidadYaRecibida.HorizontalAlign = HorizontalAlign.Left;
                celCantidadYaRecibida.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidadYaRecibida);

                TableCell celAccion = new TableCell();

                TextBox celCantidadRecibida = new TextBox();
                celCantidadRecibida.TextMode = TextBoxMode.Number;
                celCantidadRecibida.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                celCantidadRecibida.Text = (cantidad - cantidadYaRecibidas).ToString();

                celAccion.Controls.Add(celCantidadRecibida);

                tr.Cells.Add(celAccion);

                phProductos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun cargarEnPh EntregasMercaderiaF. Ex: " + ex.Message));
            }
        }

        private decimal ObtenerCantidadesYaRecibidas(List<RemitoCompraOrdenCompra_Diferencias> diferencias)
        {
            decimal totalDiferencias = 0;

            foreach (var item in diferencias)
            {
                totalDiferencias = (decimal)item.CantidadYaRecibida + (decimal)item.CantidadRecibida;
            }

            return totalDiferencias;
        }

        //private List<RemitoCompraOrdenCompra_Diferencias> obtenerDiferencias()
        //{
        //    List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();
        //    var orden = this.contComprasEnt.obtenerOrden(ordenCompra);
        //    RemitosCompra rc = new RemitosCompra();
        //    rc = this.obtenerRemitoCompraAPartirDeLosDatosDeLaVista(rc);
        //    var itemsCargar = this.obtenerItems(rc);
        //    rc.RemitosCompras_Items = itemsCargar.Item1;
        //    diferencias = itemsCargar.Item2;

        //    return diferencias;
        //}

        private RemitosCompra obtenerRemitoCompraAPartirDeLosDatosDeLaVista(RemitosCompra rc)
        {
            try
            {
                rc.IdProveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                rc.Numero = this.txtPVenta.Text + this.txtNumero.Text;
                rc.Fecha = Convert.ToDateTime(this.txtFechaMercaderiaArribo.Text, new CultureInfo("es-AR"));
                rc.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                rc.Tipo = 1;
                rc.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                rc.RemitosCompras_Comentarios.Observacion = this.txtObservaciones.Text;
                rc.Devolucion = 0;
                rc.FechaIngresoMercaderia = Convert.ToDateTime(this.txtFechaMercaderiaIngresada.Text, new CultureInfo("es-AR"));

                return rc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void procesarOrden()
        {
            bool cantidadesMenoresRecibidas = this.contieneCantidadesRecibidasMenoresAlasSolictados();
            bool cantidadesMayoresRecibidas = this.contieneCantidadesRecibidasMayoresAlasSolictados();

            if(cantidadesMenoresRecibidas)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal2", "openModal2('');", true);
            else if (cantidadesMayoresRecibidas)            
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal", "openModal('');", true);
            else
            {
                var resp = GenerarOrden(true);

                if (resp)
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "location.href = 'RemitoF.aspx';", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden procesada con exito!", "RemitoF.aspx"));
            }
        }

        public bool GenerarOrden(bool cerrarOrden)
        {
            var ordenDeCompra = this.contComprasEnt.obtenerOrden(ordenCompra);

            RemitosCompra rc = new RemitosCompra();
            rc = this.obtenerRemitoCompraAPartirDeLosDatosDeLaVista(rc);

            var tuplaItemsYDiferencias = this.obtenerItems(rc);
            rc.RemitosCompras_Items = tuplaItemsYDiferencias.Item1;

            List<RemitoCompraOrdenCompra_Diferencias> itemsConDiferencias = new List<RemitoCompraOrdenCompra_Diferencias>();
            itemsConDiferencias = tuplaItemsYDiferencias.Item2;

            actualizarCantidadesYaRecibidasOrdenDeCompra_Items();

            if (!cerrarOrden)
                ModificarOrdenCompraFechaEntrega(ordenDeCompra);

            var resp = contComprasEnt.ProcesarEntregas(itemsConDiferencias, rc, ordenCompra,cerrarOrden);
                        
            if (resp.resultadoProcesarEntrega)
            {
                Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Remito nro " + rc.Numero + " generado con exito.");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=8&rc=" + rc.Id + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0'); $.msgbox(\"Remito Generado con exito\", {type: \"info\"}); ", true); /*location.href = 'RemitoF.aspx';*/
            }

            return resp.resultadoProcesarEntrega;
        }

        public void ModificarOrdenCompraFechaEntrega(OrdenesCompra ordenCompra)
        {
            ordenCompra.FechaEntrega = Convert.ToDateTime(txtNuevaFechaEntrega.Text, new CultureInfo("es-AR"));
        }

        private Tuple<List<RemitosCompras_Items>, List<RemitoCompraOrdenCompra_Diferencias>> obtenerItems(RemitosCompra remitoCompra)
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    string txt = tr.ID;
                    decimal cantidadPedida = Convert.ToDecimal(tr.Cells[2].Text);
                    TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
                    decimal cantidadRecibida = Convert.ToDecimal(cantidadRecibidaTB.Text);
                    TableCell cantidadYaRecibidaTB = tr.Cells[3] as TableCell;
                    decimal cantidadYaRecibida = Convert.ToDecimal(cantidadYaRecibidaTB.Text);

                    if (!String.IsNullOrEmpty(txt))
                    {
                        var remitoCompra_Items = new RemitosCompras_Items();
                        string idArt = txt;
                        Articulo A = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArt));
                        remitoCompra_Items.Codigo = A.id;
                        remitoCompra_Items.Cantidad = cantidadRecibida;

                        items.Add(remitoCompra_Items);

                        int trazable = contArticulos.verificarGrupoTrazableByID(A.grupo.id);
                        if (trazable > 0)
                        {
                            remitoCompra_Items.Trazabilidad = 1;
                        }
                        else
                        {
                            remitoCompra_Items.Trazabilidad = 0;
                        }

                        if (cantidadPedida != cantidadRecibida)
                        {
                            RemitoCompraOrdenCompra_Diferencias diferencia = new RemitoCompraOrdenCompra_Diferencias();

                            diferencia.RemitosCompra = remitoCompra;
                            diferencia.OrdenCompra = ordenCompra;
                            diferencia.CantidadPedida = cantidadPedida;
                            diferencia.CantidadRecibida = cantidadRecibida;
                            diferencia.Diferencia = cantidadPedida - cantidadRecibida;
                            diferencia.Articulo = A.id;
                            diferencia.CantidadYaRecibida = cantidadYaRecibida;

                            diferencias.Add(diferencia);
                        }
                    }
                }
                return new Tuple<List<RemitosCompras_Items>, List<RemitoCompraOrdenCompra_Diferencias>>(items, diferencias);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando items a remito" + ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error cargando items a remito. " + ex.Message + ". \", {type: \"error\"});", true);
                return null;
            }
        }

        private void actualizarCantidadesYaRecibidasOrdenDeCompra_Items()
        {
            foreach (var c in this.phProductos.Controls)
            {
                TableRow tr = c as TableRow;
                string txt = tr.ID;
                TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
                decimal cantidadRecibida = Convert.ToDecimal(cantidadRecibidaTB.Text);

                if (!String.IsNullOrEmpty(txt))
                {
                    string idArt = txt;
                    Articulo articulo = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArt));
                    OrdenesCompra_Items ordenesCompra_Items = contComprasEnt.OrdenCompra_ItemGetOne(ordenCompra, articulo.id.ToString());
                    ordenesCompra_Items.CantidadYaRecibida += cantidadRecibida;
                }
            }
        }

        private bool contieneCantidadesRecibidasMayoresAlasSolictados()
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    string txt = tr.ID;
                    decimal cantidadPedida = Convert.ToDecimal(tr.Cells[2].Text);
                    TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
                    decimal cantidadRecibida = string.IsNullOrEmpty(cantidadRecibidaTB.Text) ? 0 : Convert.ToDecimal(cantidadRecibidaTB.Text);
                    TableCell cantidadYaRecibidaTB = tr.Cells[3] as TableCell;
                    decimal cantidadYaRecibida = Convert.ToDecimal(cantidadYaRecibidaTB.Text);

                    if (!String.IsNullOrEmpty(txt))
                    {
                        if (cantidadRecibida > cantidadPedida)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error en fun:contieneCantidadesRecibidasMayoresAlasSolictados. " + ex.Message + ". \", {type: \"error\"});", true);
                return false;
            }
        }

        private bool contieneCantidadesRecibidasMenoresAlasSolictados()
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;
                    string txt = tr.ID;
                    decimal cantidadPedida = Convert.ToDecimal(tr.Cells[2].Text);
                    TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
                    decimal cantidadRecibida = string.IsNullOrEmpty(cantidadRecibidaTB.Text) ? 0 : Convert.ToDecimal(cantidadRecibidaTB.Text);
                    TableCell cantidadYaRecibidaTB = tr.Cells[3] as TableCell;
                    decimal cantidadYaRecibida = Convert.ToDecimal(cantidadYaRecibidaTB.Text);

                    if (!String.IsNullOrEmpty(txt))
                    {
                        if (cantidadYaRecibida + cantidadRecibida < cantidadPedida)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error en fun:contieneCantidadesRecibidasMayoresAlasSolictados. " + ex.Message + ". \", {type: \"error\"});", true);
                return false;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                this.procesarOrden();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al generar la orden " + ex.Message);
            }
        }

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue), -1);
            }
            catch
            {

            }
        }

        protected void lbtnRecibirlaOrdenConCantidadesMayores_Click(object sender, EventArgs e)
        {

        }

        protected void lbtnRecibirLoSolicitado_Click(object sender, EventArgs e)
        {

        }

        protected void lbtnCerrar_Click(object sender, EventArgs e)
        {
            var resp = GenerarOrden(true);

            if (resp)
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden procesada con exito!", "RemitoF.aspx"));
        }

        protected void lbtnGuardar_Click(object sender, EventArgs e)
        {
            var resp = GenerarOrden(false);

            if(resp)
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden procesada con exito!", "RemitoF.aspx"));            
        }
    }
}