using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Api.Modelo.Responses.Orden_Compra;
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
        bool _verCantidadPedida = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ordenCompra = Convert.ToInt64(Request.QueryString["oc"]);

            this.VerificarLogin();

            ConfigurarBotonesAguarde();

            if (!IsPostBack)
            {
                cargarProveedores();
                cargarSucursal();
                CargarDatosDesdeOrdenCompra();
                ConfigurarNuevaFechaEntrega();
            }
            CargarItemsFacturaEnPH();
        }

        private void ConfigurarBotonesAguarde()
        {
            btnRecibirTodo.Attributes.Add("onclick", " this.disabled = true;  " + btnIngresoManual.ClientID + ".disabled=true;" + btnRechazarTodo.ClientID + ".disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnRecibirTodo, null) + ";");
            btnIngresoManual.Attributes.Add("onclick", " this.disabled = true;  " + btnRecibirTodo.ClientID + ".disabled=true;" + btnRechazarTodo.ClientID + ".disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnIngresoManual, null) + ";");
            btnRechazarTodo.Attributes.Add("onclick", " this.disabled = true;  " + btnRecibirTodo.ClientID + ".disabled=true;" + btnIngresoManual.ClientID + ".disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnRechazarTodo, null) + ";");
            //btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
            btnGuardar.Attributes.Add("onclick", " this.disabled = true; " + btnCerrar.ClientID + ".disabled=true;" + " this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnGuardar, null) + ";");
            btnCerrar.Attributes.Add("onclick", " this.disabled = true; " + btnGuardar.ClientID + ".disabled=true;" + " this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnCerrar, null) + ";");
        }

        public void ConfigurarNuevaFechaEntrega()
        {
            if (string.IsNullOrEmpty(txtNuevaFechaEntrega.Text))
                txtNuevaFechaEntrega.Text = txtFechaOC.Text;
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

                        if (s == "210")
                            _verCantidadPedida = true;
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

                if (!permisos.Contains("204"))
                    btnCerrar.Visible = false;

                if (permisos.Contains("205"))
                    txtNuevaFechaEntrega.Enabled = true;

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
                txtNuevaFechaEntrega.CssClass = "form-control";

                cargarPuntoVta(Convert.ToInt32(ListSucursal.SelectedValue), oc.IdPtoVenta.Value);

                ListProveedor.SelectedValue = oc.IdProveedor.ToString();

                txtFechaMercaderiaArribo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaOC.Text = oc.FechaEntrega.Value.ToString("dd/MM/yyyy");
                txtFechaMercaderiaIngresada.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtObservaciones.Text = "Orden de Compra: " + oc.Numero;
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
                if (!_verCantidadPedida)
                {
                    phCantidadPedida.Visible = false;
                    celCantidad.Visible = false;
                }                    
                tr.Cells.Add(celCantidad);

                TableCell celCantidadYaRecibida = new TableCell();
                decimal cantidadYaRecibidas = ObtenerCantidadesYaRecibidas(diferencias);
                celCantidadYaRecibida.Text = "0";

                if(diferencias != null && diferencias.Count > 0)
                    celCantidadYaRecibida.Text = cantidadYaRecibidas.ToString();

                celCantidadYaRecibida.HorizontalAlign = HorizontalAlign.Left;
                celCantidadYaRecibida.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidadYaRecibida);

                TableCell celAccionCantidadRecibida = new TableCell();
                TextBox celCantidadRecibida = new TextBox();
                celCantidadRecibida.CssClass = "form-control";
                celCantidadRecibida.TextMode = TextBoxMode.Number;
                celCantidadRecibida.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                celCantidadRecibida.TextChanged += new EventHandler(ValidarCampo);
                celCantidadRecibida.Text = "0";
                
                celAccionCantidadRecibida.Controls.Add(celCantidadRecibida);
                tr.Cells.Add(celAccionCantidadRecibida);

                TableCell celAccion = new TableCell();
                DropDownList celDropDownList = new DropDownList();
                celDropDownList.CssClass = "form-control";
                DataTable dt = new DataTable();
                dt.Columns.Add("NombreAccion");
                dt.Columns.Add("Valor");

                DataRow dr = dt.NewRow();
                dr["NombreAccion"] = "Agregar";
                dr["Valor"] = "agregar";
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["NombreAccion"] = "Rechazar";
                dr2["Valor"] = "rechazar";
                dt.Rows.InsertAt(dr2, 1);

                DataRow dr3 = dt.NewRow();
                dr3["NombreAccion"] = "Recepcion parcial";
                dr3["Valor"] = "parcial";
                dt.Rows.InsertAt(dr3, 2);

                celDropDownList.DataSource = dt;
                celDropDownList.DataValueField = "Valor";
                celDropDownList.DataTextField = "NombreAccion";
                celDropDownList.DataBind();

                celAccion.Controls.Add(celDropDownList);

                tr.Cells.Add(celAccion);

                phProductos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun cargarEnPh EntregasMercaderiaF. Ex: " + ex.Message));
            }
        }

        private void ValidarCampo(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (sender as TextBox);

                if(string.IsNullOrEmpty(textBox.Text) || Convert.ToInt32(textBox.Text) < 0)
                    textBox.Text = "0";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al validar el campo. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error validando campo. " + ex.Message);
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

        private RemitosCompra ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(RemitosCompra remitoCompra)
        {
            try
            {
                remitoCompra.IdProveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                remitoCompra.Numero = this.txtPVenta.Text + this.txtNumero.Text;
                remitoCompra.Fecha = Convert.ToDateTime(this.txtFechaMercaderiaArribo.Text, new CultureInfo("es-AR"));
                remitoCompra.IdSucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                remitoCompra.Tipo = 1;
                remitoCompra.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                remitoCompra.RemitosCompras_Comentarios.Observacion = this.txtObservaciones.Text;
                remitoCompra.Devolucion = 0;
                remitoCompra.FechaIngresoMercaderia = Convert.ToDateTime(this.txtFechaMercaderiaIngresada.Text, new CultureInfo("es-AR"));

                return remitoCompra;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void ProcesarEntrega()
        {
            bool cantidadesMenoresRecibidas = this.ContieneCantidadesRecibidasMenoresAlasSolictados();
            bool cantidadesMayoresRecibidas = this.ContieneCantidadesRecibidasMayoresAlasSolictados();

            if(cantidadesMenoresRecibidas)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal2", "openModal2('');", true);
            else if (cantidadesMayoresRecibidas)
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal", "openModal('');", true);
            else
            {
                var resp = GenerarEntrega(true,false,false);                

                if (resp.resultadoProcesarEntrega)
                {
                    ImprimirRemito(resp.idRemito, 0);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "location.href = 'RemitoF.aspx';", true);
                }                    
            }
        }

        public bool ModificarOrdenCompraFechaEntrega()
        {
            try
            {
                var ordenDeCompra = this.contComprasEnt.obtenerOrden(ordenCompra);

                var nuevaFechaEntrega = Convert.ToDateTime(txtNuevaFechaEntrega.Text, new CultureInfo("es-AR"));

                if (ordenDeCompra.FechaEntrega > nuevaFechaEntrega)
                {
                    lblFechaEntregaError.Visible = true;
                    return false;
                }
                else
                {
                    ordenDeCompra.FechaEntrega = Convert.ToDateTime(txtNuevaFechaEntrega.Text, new CultureInfo("es-AR"));
                    return true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("La fecha ingresada es incorrecta!"));
                return false;
            }

        }                

        private void ActualizarCantidadesYaRecibidasOrdenDeCompra_Items()
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

        private bool ContieneCantidadesRecibidasMayoresAlasSolictados()
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
                        if (cantidadRecibida + cantidadYaRecibida > cantidadPedida)
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

        private bool ContieneCantidadesRecibidasMenoresAlasSolictados()
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

                this.ProcesarEntrega();
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

        protected void lbtnCerrar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var resp = GenerarEntrega(true,false,false);
            
            if (resp.resultadoProcesarEntrega)
            {
                ImprimirRemito(resp.idRemito, 0);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden procesada con exito!", "RemitoF.aspx"));
            }
                
        }

        protected void lbtnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var nuevaFechaEntrega = Convert.ToDateTime(txtNuevaFechaEntrega.Text, new CultureInfo("es-AR"));

            if (nuevaFechaEntrega < DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"La nueva fecha ingresada es inferior a la fecha de hoy!\", {type: \"alert\"});", true);
                return;
            }

            var resp = GenerarEntrega(false,false,false);

            if (resp.resultadoProcesarEntrega)
            {
                ImprimirRemito(resp.idRemito, 0);
                //ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", " $.msgbox(\"Orden procesada con exito!\", {type: \"alert\"}); location.href = 'RemitoF.aspx';", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden procesada con exito!", "RemitoF.aspx"));
            }                
            else
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal2", "openModal2('');", true);
        }        

        private void ImprimirRemito(int idRemito, int idRemitoDevolucion)
        {
            try
            {
                string script = "";

                script = "window.open('ImpresionCompras.aspx?a=8&rc=" + idRemito + "','_blank');";

                if(idRemitoDevolucion > 0)
                    script += "window.open('ImpresionCompras.aspx?a=8&rc=" + idRemitoDevolucion + "','_blank');";

                script += " $.msgbox(\"Entrega generada. \", {type: \"info\"}); location.href = 'RemitoF.aspx';";

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "Error al imprimir remito. " + ex.Message, true);
            }
        }

        private bool ValidarCampoNumero()
        {
            if (string.IsNullOrEmpty(txtPVenta.Text) || string.IsNullOrEmpty(txtNumero.Text))
            {
                btnRecibirTodo.Attributes.Remove("Disabled");
                btnRechazarTodo.Attributes.Remove("Disabled");
                btnIngresoManual.Attributes.Remove("Disabled");
                btnIngresoManual.Text = "Ingreso manual";
                return false;
            }
            return true;
        }

        private bool ValidarEntregaConValoresMayoresACero()
        {
            bool valoresMayoresACero = false;

            foreach (var c in phProductos.Controls)
            {
                TableRow tr = c as TableRow;
                TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
                decimal cantidadRecibida = Convert.ToDecimal(cantidadRecibidaTB.Text);

                if(cantidadRecibida > 0)
                {
                    valoresMayoresACero = true;
                }

            }
            return valoresMayoresACero;
        }

        protected void btnRecibirTodo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidarCampoNumero())
                    return;

                if (!ValidarEntregaConValoresMayoresACero())
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Todos los valores de mercaderia recibida se encuentran en cero! \");", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtPVenta.Text) || string.IsNullOrEmpty(txtNumero.Text))
                {
                    btnRecibirTodo.Attributes.Remove("Disabled");
                    btnRechazarTodo.Attributes.Remove("Disabled");
                    btnIngresoManual.Attributes.Remove("Disabled");
                    btnIngresoManual.Text = "Ingreso manual";
                    return;
                }

                var ordenDeCompra = contComprasEnt.obtenerOrden(ordenCompra);

                var nuevoRemitoCompra = GenerarEntrega(true,false,true);

                ImprimirRemito(nuevoRemitoCompra.idRemito,0);

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al recibir la orden de compra cuando la mercaderia era mayor a la solicitada " + ex.Message);
            }
        }

        public ProcesarEntregaResponse GenerarEntrega(bool cerrarOrden, bool devolucionTotalMercaderia,bool generarDiferenciaTotal, RemitosCompra remitoCompra = null)
        {
            if (remitoCompra == null)
            {
                remitoCompra = new RemitosCompra();
                remitoCompra = ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompra);
            }

            List<RemitoCompraOrdenCompra_Diferencias> itemsConDiferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

            remitoCompra.RemitosCompras_Items = ObtenerItems(remitoCompra, devolucionTotalMercaderia);

            itemsConDiferencias = ObtenerDiferencias(remitoCompra, generarDiferenciaTotal);

            ActualizarCantidadesYaRecibidasOrdenDeCompra_Items();

            if (!cerrarOrden)
            {
                if (!ModificarOrdenCompraFechaEntrega())
                {
                    return new ProcesarEntregaResponse()
                    {
                        resultadoProcesarEntrega = false,
                        detalleResultadoProcesarEntrega = "La nueva fecha de entrega ingresada es incorrecta"
                    };
                }
            }

            var resp = contComprasEnt.ProcesarEntregas(itemsConDiferencias, remitoCompra, ordenCompra, cerrarOrden);

            if (resp.resultadoProcesarEntrega)
            {
                Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Remito nro " + remitoCompra.Numero + " generado con exito.");
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Remito Generado con exito\", {type: \"info\"}); ", true); /*location.href = 'RemitoF.aspx';*/
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=8&rc=" + remitoCompra.Id + "','_blank'); $.msgbox(\"Remito Generado con exito\", {type: \"info\"}); ", true);
            }

            return resp;
        }

        private List<RemitoCompraOrdenCompra_Diferencias> ObtenerDiferencias(RemitosCompra remitoCompra, bool generarDiferenciaTotal)
        {
            List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

            foreach (var c in this.phProductos.Controls)
            {
                TableRow tr = c as TableRow;

                var cantidades = RecorrerPHyObtenerCantidades(tr);

                string idArticulo = cantidades.Item1;
                decimal cantidadPedida = cantidades.Item2;
                TextBox cantidadRecibidaTB = cantidades.Item3;
                decimal cantidadRecibida = cantidades.Item4;
                TableCell cantidadYaRecibidaTB = cantidades.Item5;
                decimal cantidadYaRecibida = cantidades.Item6;

                if (!String.IsNullOrEmpty(idArticulo) && cantidadPedida != cantidadRecibida)
                {
                    RemitoCompraOrdenCompra_Diferencias diferencia = new RemitoCompraOrdenCompra_Diferencias();
                    diferencia.RemitosCompra = remitoCompra;
                    diferencia.OrdenCompra = ordenCompra;
                    diferencia.CantidadPedida = cantidadPedida;
                    diferencia.CantidadRecibida = cantidadRecibida;

                    if (generarDiferenciaTotal)
                        diferencia.Diferencia = cantidadRecibida;
                    else
                        diferencia.Diferencia = cantidadPedida - cantidadRecibida;

                    diferencia.Articulo = Convert.ToInt32(idArticulo);
                    diferencia.CantidadYaRecibida = cantidadYaRecibida;

                    diferencias.Add(diferencia);
                }
            }
            return diferencias;
        }

        private List<RemitosCompras_Items> ObtenerItems(RemitosCompra remitoCompra, bool devolucionTotalMercaderia)
        {
            try
            {
                List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

                List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

                foreach (var c in this.phProductos.Controls)
                {
                    TableRow tr = c as TableRow;

                    var cantidades = RecorrerPHyObtenerCantidades(tr);

                    string idArticulo = cantidades.Item1;
                    decimal cantidadPedida = cantidades.Item2;
                    TextBox cantidadRecibidaTB = cantidades.Item3;
                    decimal cantidadRecibida = cantidades.Item4;
                    TableCell cantidadYaRecibidaTB = cantidades.Item5;
                    decimal cantidadYaRecibida = cantidades.Item6;

                    if (cantidadRecibida <= 0)
                        continue;

                    if (!String.IsNullOrEmpty(idArticulo))
                    {
                        var remitoCompra_Items = new RemitosCompras_Items();
                        string idArt = idArticulo;
                        Articulo A = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArt));
                        remitoCompra_Items.Codigo = A.id;

                        if (remitoCompra.Devolucion == 1)
                        {
                            if (devolucionTotalMercaderia)
                                remitoCompra_Items.Cantidad = cantidadRecibida;
                            else
                                remitoCompra_Items.Cantidad = cantidadRecibida - cantidadPedida; //al hacer cantidad recibida menos cantidad pedida el numero deberia quedar siempre positivo
                        }
                        else
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
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error cargando items a remito" + ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error cargando items a remito. " + ex.Message + ". \", {type: \"error\"});", true);
                return null;
            }
        }

        //protected void btnRecibirLoSolicitado_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Page.IsValid)
        //            return;

        //        var resp = GenerarEntregaAceptarLoSolicitado();

        //        if (resp.resultadoProcesarEntrega)
        //        {
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se acepto solo la mercaderia solicitada con exito!", "RemitoF.aspx"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "Error", "Error al recibir la mercaderia solicitada de la orden de compra cuando la mercaderia era mayor a la solicitada " + ex.Message);
        //    }
        //}

        protected void lbtnRechazarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidarCampoNumero())
                    return;

                if (!ValidarEntregaConValoresMayoresACero())
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Todos los valores de mercaderia recibida se encuentran en cero! \");", true);
                    return;
                }

                var resp = GenerarEntregaRechazada();

                if (resp.resultadoProcesarEntrega)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden rechazada con exito!", "RemitoF.aspx"));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al rechazar la orden de compra cuando la mercaderia era mayor a la solicitada " + ex.Message);
            }
        }

        //private ProcesarEntregaResponse GenerarEntregaAceptarLoSolicitado()
        //{
        //    var ordenDeCompra = this.contComprasEnt.obtenerOrden(ordenCompra);

        //    RemitosCompra remitoCompraDevolucion = new RemitosCompra();
        //    RemitosCompra remitoCompra = new RemitosCompra();
        //    List<RemitoCompraOrdenCompra_Diferencias> itemsConDiferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

        //    ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompra);
        //    ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompraDevolucion);

        //    remitoCompraDevolucion.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
        //    remitoCompraDevolucion.RemitosCompras_Comentarios.Observacion = "Se genera remito de devolucion por exceso de mercaderia aceptada en la orden de compra numero" + ordenDeCompra.Numero;
        //    remitoCompraDevolucion.Devolucion = 1;

        //    remitoCompra.RemitosCompras_Items = ObtenerItems(remitoCompra, false);
        //    remitoCompraDevolucion.RemitosCompras_Items = ObtenerItems(remitoCompraDevolucion, false);

        //    itemsConDiferencias = ObtenerDiferencias(remitoCompra,false);

        //    ActualizarCantidadesYaRecibidasOrdenDeCompra_Items();

        //    var entregaRechazadaResponse = contComprasEnt.ProcesarEntregaRecibirSoloLoSolicitado(itemsConDiferencias, remitoCompra, remitoCompraDevolucion, ordenCompra);

        //    ImprimirRemito(entregaRechazadaResponse.idRemito, entregaRechazadaResponse.idRemitoDevolucion);

        //    return entregaRechazadaResponse;
        //}

        private ProcesarEntregaResponse GenerarEntregaRechazada()
        {
            var ordenDeCompra = this.contComprasEnt.obtenerOrden(ordenCompra);

            RemitosCompra remitoCompraDevolucion = new RemitosCompra();
            RemitosCompra remitoCompra = new RemitosCompra();
            List<RemitoCompraOrdenCompra_Diferencias> itemsConDiferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

            ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompra);
            ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompraDevolucion);

            remitoCompraDevolucion.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
            remitoCompraDevolucion.RemitosCompras_Comentarios.Observacion = "Se genera remito de devolucion por exceso de mercaderia aceptada en la orden de compra numero" + ordenDeCompra.Numero;
            remitoCompraDevolucion.Devolucion = 1;

            remitoCompra.RemitosCompras_Items = ObtenerItemsPorRechazoEntrega(false);
            remitoCompraDevolucion.RemitosCompras_Items = ObtenerItemsPorRechazoEntrega(true);

            itemsConDiferencias.AddRange(ObtenerDiferenciasRechazoEntrega(remitoCompra));

            ActualizarCantidadesYaRecibidasOrdenDeCompra_Items();

            var entregaRechazadaResponse = contComprasEnt.ProcesarEntregaRechazada(itemsConDiferencias, remitoCompra, remitoCompraDevolucion, ordenCompra);

            ImprimirRemito(entregaRechazadaResponse.idRemito, entregaRechazadaResponse.idRemitoDevolucion);

            return entregaRechazadaResponse;
        }

        private List<RemitosCompras_Items> ObtenerItemsPorRechazoEntrega(bool esRemitoDevolucion)
        {
            List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();

            foreach (var c in this.phProductos.Controls)
            {
                TableRow tr = c as TableRow;

                var cantidades = RecorrerPHyObtenerCantidades(tr);

                string idArticulo = cantidades.Item1;
                decimal cantidadPedida = cantidades.Item2;
                TextBox cantidadRecibidaTB = cantidades.Item3;
                decimal cantidadRecibida = cantidades.Item4;
                TableCell cantidadYaRecibidaTB = cantidades.Item5;
                decimal cantidadYaRecibida = cantidades.Item6;

                if (cantidadRecibida <= 0)
                    continue;

                if (!String.IsNullOrEmpty(idArticulo))
                {
                    var remitoCompra_Items = new RemitosCompras_Items();
                    string idArt = idArticulo;

                    Articulo articulo = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArt));

                    remitoCompra_Items.Codigo = articulo.id;
                    if (esRemitoDevolucion)
                    {
                        remitoCompra_Items.Cantidad = cantidadRecibida * -1;
                    }
                    else
                    {
                        remitoCompra_Items.Cantidad = cantidadRecibida;
                    }

                    items.Add(remitoCompra_Items);

                    int trazable = contArticulos.verificarGrupoTrazableByID(articulo.grupo.id);
                    if (trazable > 0)
                    {
                        remitoCompra_Items.Trazabilidad = 1;
                    }
                    else
                    {
                        remitoCompra_Items.Trazabilidad = 0;
                    }
                }
            }

             return items;
        }        

        private List<RemitoCompraOrdenCompra_Diferencias> ObtenerDiferenciasRechazoEntrega(RemitosCompra remitoCompra)
        {
            List<RemitoCompraOrdenCompra_Diferencias> diferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

            foreach (var c in this.phProductos.Controls)
            {
                TableRow tr = c as TableRow;

                var cantidades = RecorrerPHyObtenerCantidades(tr);

                string idArticulo = cantidades.Item1;
                decimal cantidadPedida = cantidades.Item2;
                TextBox cantidadRecibidaTB = cantidades.Item3;
                decimal cantidadRecibida = cantidades.Item4;
                TableCell cantidadYaRecibidaTB = cantidades.Item5;
                decimal cantidadYaRecibida = cantidades.Item6;
                                
                if (!String.IsNullOrEmpty(idArticulo) && cantidadPedida != cantidadRecibida)
                {
                    RemitoCompraOrdenCompra_Diferencias diferencia = new RemitoCompraOrdenCompra_Diferencias();
                    diferencia.RemitosCompra = remitoCompra;
                    diferencia.OrdenCompra = ordenCompra;
                    diferencia.CantidadPedida = cantidadPedida;
                    diferencia.CantidadRecibida = cantidadRecibida;
                    diferencia.Diferencia = cantidadRecibida;
                    diferencia.Articulo = Convert.ToInt32(idArticulo);
                    diferencia.CantidadYaRecibida = cantidadYaRecibida;

                    diferencias.Add(diferencia);
                }
            }
            return diferencias;
        }

        public Tuple<string,decimal,TextBox,decimal,TableCell,decimal> RecorrerPHyObtenerCantidades(TableRow tr)
        {
            string idArticulo = tr.ID;
            decimal cantidadPedida = Convert.ToDecimal(tr.Cells[2].Text);
            TextBox cantidadRecibidaTB = tr.Cells[4].Controls[0] as TextBox;
            decimal cantidadRecibida = Convert.ToDecimal(cantidadRecibidaTB.Text);
            TableCell cantidadYaRecibidaTB = tr.Cells[3] as TableCell;
            decimal cantidadYaRecibida = Convert.ToDecimal(cantidadYaRecibidaTB.Text);

            return new Tuple<string, decimal, TextBox, decimal, TableCell, decimal>(idArticulo, cantidadPedida, cantidadRecibidaTB, cantidadRecibida, cantidadYaRecibidaTB, cantidadYaRecibida);
        }

        protected void btnIngresoManual_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidarCampoNumero())
                    return;

                if (!ValidarEntregaConValoresMayoresACero())
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Todos los valores de mercaderia recibida se encuentran en cero! \");", true);
                    return;
                }

                var resp = GenerarEntregaIngresoManual();

                if (resp.resultadoProcesarEntrega)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Ingreso manual realizado con exito!", "RemitoF.aspx"));
                }                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al realizar un ingreso manual en entregas. " + ex.Message);
            }
        }

        private ProcesarEntregaResponse GenerarEntregaIngresoManual()
        {
            var ordenDeCompra = contComprasEnt.obtenerOrden(ordenCompra);
            RemitosCompra remitoCompraDevolucion = null;
            RemitosCompra remitoCompra = new RemitosCompra();
            List<RemitosCompras_Items> items = new List<RemitosCompras_Items>();
            List<RemitosCompras_Items> itemsDevolucion = new List<RemitosCompras_Items>();
            List<RemitoCompraOrdenCompra_Diferencias> itemsConDiferencias = new List<RemitoCompraOrdenCompra_Diferencias>();

            ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompra);            

            foreach (var c in phProductos.Controls)
            {
                TableRow tr = c as TableRow;

                DropDownList dropDownList = tr.Cells[5].Controls[0] as DropDownList;
                var condicion = dropDownList.Text;
                var cantidades = RecorrerPHyObtenerCantidades(tr);

                string idArticulo = cantidades.Item1;
                decimal cantidadPedida = cantidades.Item2;
                decimal cantidadRecibida = cantidades.Item4;

                if (cantidadRecibida <= 0)
                    continue;

                if (condicion == "agregar")
                {
                    var itemIngresoTemp = GenerarIngresoArticulo(idArticulo, cantidadRecibida);
                    items.Add(itemIngresoTemp);
                }
                else if (condicion == "rechazar")
                {
                    var itemIngresoTemp = GenerarIngresoArticulo(idArticulo, cantidadRecibida);
                    var itemRechazoTemp = GenerarRechazoArticulo(idArticulo, cantidadRecibida);
                    items.Add(itemIngresoTemp);
                    itemsDevolucion.Add(itemRechazoTemp);
                }
                else
                {
                    if (cantidadRecibida > cantidadPedida)
                    {
                        var itemIngresoTemp = GenerarIngresoArticulo(idArticulo, cantidadRecibida);
                        var itemRechazoTemp = GenerarRechazoArticulo(idArticulo, cantidadRecibida - cantidadPedida);
                        items.Add(itemIngresoTemp);
                        itemsDevolucion.Add(itemRechazoTemp);
                    }
                    else
                    {
                        var itemIngresoTemp = GenerarIngresoArticulo(idArticulo, cantidadRecibida);
                        items.Add(itemIngresoTemp);
                    }
                }
            }

            remitoCompra.RemitosCompras_Items = items;

            if(itemsDevolucion.Count > 0)
            {
                remitoCompraDevolucion = new RemitosCompra();
                ObtenerRemitoCompraAPartirDeLosDatosDeLaVista(remitoCompraDevolucion);
                remitoCompraDevolucion.RemitosCompras_Items = itemsDevolucion;
                remitoCompraDevolucion.RemitosCompras_Comentarios = new RemitosCompras_Comentarios();
                remitoCompraDevolucion.RemitosCompras_Comentarios.Observacion = "Se genera remito de devolucion por exceso de mercaderia aceptada en la orden de compra numero" + ordenDeCompra.Numero;
                remitoCompraDevolucion.Devolucion = 1;
            }            

            itemsConDiferencias = ObtenerDiferencias(remitoCompra, false);

            ActualizarCantidadesYaRecibidasOrdenDeCompra_Items();

            var entregaParcialResponse = contComprasEnt.ProcesarEntregaRecibirSoloLoSolicitado(itemsConDiferencias, remitoCompra, remitoCompraDevolucion, ordenCompra);

            ImprimirRemito(entregaParcialResponse.idRemito, entregaParcialResponse.idRemitoDevolucion);

            return entregaParcialResponse;
        }

        public RemitosCompras_Items GenerarIngresoArticulo(string idArticulo, decimal cantidadRecibida)
        {
            try
            {
                Articulo A = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArticulo));
                RemitosCompras_Items remitosCompras_Items = new RemitosCompras_Items();
                remitosCompras_Items.Codigo = A.id;
                remitosCompras_Items.Cantidad = cantidadRecibida;
                int trazable = contArticulos.verificarGrupoTrazableByID(A.grupo.id);
                if (trazable > 0)
                    remitosCompras_Items.Trazabilidad = 1;
                else
                    remitosCompras_Items.Trazabilidad = 0;

                return remitosCompras_Items;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar ingreso de articulo. " + ex.Message);
            }
        }

        public RemitosCompras_Items GenerarRechazoArticulo(string idArticulo, decimal cantidadRecibida)
        {
            try
            {
                Articulo A = contArticulos.obtenerArticuloByID(Convert.ToInt32(idArticulo));
                RemitosCompras_Items remitosCompras_ItemsDevolucion = new RemitosCompras_Items();
                remitosCompras_ItemsDevolucion.Codigo = A.id;
                remitosCompras_ItemsDevolucion.Cantidad = cantidadRecibida * -1;
                int trazable = contArticulos.verificarGrupoTrazableByID(A.grupo.id);
                if (trazable > 0)
                    remitosCompras_ItemsDevolucion.Trazabilidad = 1;
                else
                    remitosCompras_ItemsDevolucion.Trazabilidad = 0;

                return remitosCompras_ItemsDevolucion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar rechazo de articulo. " + ex.Message);
            }
        }
    }
}