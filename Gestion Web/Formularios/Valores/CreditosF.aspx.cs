using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CreditosF : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        ControladorPlenario contPlenario = new ControladorPlenario();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorVendedor contVend = new controladorVendedor();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int idUsuario;
        private int sucursal;
        private int empresa;
        private int vendedor;
        private int ptoVenta;
        private int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];                            
                this.sucursal = Convert.ToInt32(Request.QueryString["suc"]);
                this.ptoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.empresa = Convert.ToInt32(Request.QueryString["emp"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaD) && String.IsNullOrEmpty(this.fechaH))
                    {
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        this.sucursal = (int)Session["Login_SucUser"];
                        this.ptoVenta = (int)Session["Login_PtoUser"];
                        this.empresa = (int)Session["Login_EmpUser"];
                    }
                    this.cargarEmpresas();
                    this.cargarSucursalByEmpresa(this.empresa);
                    this.cargarPuntoVta(this.sucursal);
                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                    this.ListEmpresa.SelectedValue = this.empresa.ToString();
                    this.ListSucursal.SelectedValue = this.sucursal.ToString();
                    this.ListPuntoVta.SelectedValue = this.ptoVenta.ToString();
                    this.ListEstados.SelectedValue = this.estado.ToString();
                }
                this.cargarCreditosAValidar();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "97")
                        {    //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.ListSucursal.Attributes.Remove("disabled");
                            }
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
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();
                this.ListEmpresa.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListEmpresa.Items.Insert(1, new ListItem("Todas", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();
                this.ListSucursal.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListSucursal.Items.Insert(1, new ListItem("Todas", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                this.ListPuntoVta.DataSource = dt;
                this.ListPuntoVta.DataValueField = "Id";
                this.ListPuntoVta.DataTextField = "NombreFantasia";
                this.ListPuntoVta.DataBind();

                this.ListPuntoVta.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListPuntoVta.Items.Insert(1, new ListItem("Todos", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pto ventas. " + ex.Message));
            }
        }
        private void cargarCreditosAValidar()
        {
            try
            {
                ControladorPlenario contPlenario = new ControladorPlenario();
                controladorFacturacion contFc = new controladorFacturacion();

                //sucursal y PV
                controladorSucursal contSucu = new controladorSucursal();
                int emp = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                int suc = Convert.ToInt32(this.ListSucursal.SelectedValue);
                int pv = Convert.ToInt32(this.ListPuntoVta.SelectedValue);

                int validas = 0;
                int validar = 0;

                if (!String.IsNullOrEmpty(this.txtFechaDesde.Text))
                {
                    List<Planario_Api.Entidades.SolicitudPlenario> solicitudes = contPlenario.obtenerSolicitudesCierreCaja(emp, suc, pv, this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.ListEstados.SelectedValue));

                    this.phCreditos.Controls.Clear();
                    foreach (var s in solicitudes)
                    {
                        //fila
                        TableRow tr = new TableRow();
                        //tr.ID = s.NroSolicitud.ToString();
                        tr.ID = s.Id.ToString();
                        Factura f = contFc.obtenerFacturaId(s.Factura.Value);

                        TableCell celFechaDoc = new TableCell();
                        celFechaDoc.Text = f.fecha.ToString("dd/MM/yyyy");
                        celFechaDoc.Width = Unit.Percentage(10);
                        celFechaDoc.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celFechaDoc);

                        TableCell celDoc = new TableCell();                        
                        celDoc.Text = f.tipo.tipo + " Nº " + f.numero;
                        celDoc.Width = Unit.Percentage(25);
                        celDoc.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celDoc);

                        TableCell celFecha = new TableCell();
                        celFecha.Text = s.FechaOperacion.ToString("dd/MM/yyyy");
                        celFecha.Width = Unit.Percentage(15);
                        celFecha.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celFecha);

                        TableCell celDNI = new TableCell();
                        celDNI.Text = s.Dni.ToString();
                        celDNI.Width = Unit.Percentage(15);
                        celDNI.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celDNI);

                        TableCell celNumero = new TableCell();
                        celNumero.Text = s.NroSolicitud.ToString();
                        celNumero.Width = Unit.Percentage(10);
                        celNumero.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celNumero);


                        TableCell celCapital = new TableCell();
                        celCapital.Text = "$" + s.Capital;
                        celCapital.Width = Unit.Percentage(10);
                        celCapital.VerticalAlign = VerticalAlign.Middle;
                        celCapital.HorizontalAlign = HorizontalAlign.Right;
                        tr.Cells.Add(celCapital);

                        TableCell celAnticipo = new TableCell();
                        celAnticipo.Text = "$" + s.Anticipo;
                        celAnticipo.Width = Unit.Percentage(10);
                        celAnticipo.VerticalAlign = VerticalAlign.Middle;
                        celAnticipo.HorizontalAlign = HorizontalAlign.Right;
                        tr.Cells.Add(celAnticipo);

                        if (s.Validada != null)
                        {
                            if (s.Validada.Value > 0)
                            {
                                validas++;
                                TableCell celAccion = new TableCell();
                                LinkButton btnValidar = new LinkButton();
                                btnValidar.CssClass = "btn btn-success";
                                btnValidar.ID = "btnOK_" + s.Id;
                                btnValidar.Text = "<span class='shortcut-icon icon-ok'></span>";
                                celAccion.Controls.Add(btnValidar);
                                celAccion.Width = Unit.Percentage(15);
                                celAccion.VerticalAlign = VerticalAlign.Middle;
                                tr.Cells.Add(celAccion);
                            }
                            else
                            {
                                validar++;
                                TableCell celAccion = new TableCell();
                                LinkButton btnValidarNO = new LinkButton();
                                btnValidarNO.CssClass = "btn btn-danger ui-tooltip";
                                btnValidarNO.Attributes.Add("data-toggle", "tooltip");
                                btnValidarNO.ID = "btnNO_" + s.Id;
                                btnValidarNO.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                btnValidarNO.Attributes.Add("onclick", "grisarValidar('" + btnValidarNO.ID + "')");
                                btnValidarNO.Click += new EventHandler(this.VerificarSolicitud);
                                btnValidarNO.Text = "<span class='shortcut-icon icon-ok'></span>";
                                btnValidarNO.Attributes.Add("title data-original-title", "Validar");
                                celAccion.Controls.Add(btnValidarNO);

                                Literal l1 = new Literal();
                                l1.Text = "&nbsp";
                                celAccion.Controls.Add(l1);

                                LinkButton btnEditar = new LinkButton();
                                btnEditar.ID = "btnEditar_" + s.Id;
                                btnEditar.CssClass = "btn btn-info";
                                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                                btnEditar.Click += new EventHandler(this.modificarSolicitud);
                                btnEditar.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                btnEditar.Attributes.Add("onclick", "grisarClick('" + btnEditar.ID + "')");
                                celAccion.Controls.Add(btnEditar);

                                celAccion.Width = Unit.Percentage(15);
                                celAccion.VerticalAlign = VerticalAlign.Middle;
                                tr.Cells.Add(celAccion);
                            }
                        }
                        else
                        {
                            validar++;
                            TableCell celAccion = new TableCell();
                            LinkButton btnValidarNO = new LinkButton();
                            btnValidarNO.CssClass = "btn btn-danger ui-tooltip";
                            btnValidarNO.Attributes.Add("data-toggle", "tooltip");
                            btnValidarNO.ID = "btnNO_" + s.Id;
                            btnValidarNO.ClientIDMode = System.Web.UI.ClientIDMode.Static;                            
                            btnValidarNO.Attributes.Add("onclick", "grisarValidar('" + btnValidarNO.ID + "')");
                            btnValidarNO.Click += new EventHandler(this.VerificarSolicitud);
                            btnValidarNO.Text = "<span class='shortcut-icon icon-ok'></span>";
                            btnValidarNO.Attributes.Add("title data-original-title", "Validar");
                            celAccion.Controls.Add(btnValidarNO);

                            Literal l1 = new Literal();
                            l1.Text = "&nbsp";
                            celAccion.Controls.Add(l1);

                            LinkButton btnEditar = new LinkButton();
                            btnEditar.ID = "btnEditar_" + s.Id;
                            btnEditar.CssClass = "btn btn-info";
                            btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                            btnEditar.Click += new EventHandler(this.modificarSolicitud);
                            btnEditar.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            btnEditar.Attributes.Add("onclick", "grisarClick('" + btnEditar.ID + "')");
                            celAccion.Controls.Add(btnEditar);

                            celAccion.Width = Unit.Percentage(15);
                            celAccion.VerticalAlign = VerticalAlign.Middle;
                            tr.Cells.Add(celAccion);
                        }

                        this.phCreditos.Controls.Add(tr);
                    }
                    this.lblValidas.Text = validas.ToString();
                    this.lblValidar.Text = validar.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CreditosF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&emp=" + this.ListEmpresa.SelectedValue + "&suc=" + this.ListSucursal.SelectedValue + "&pv=" + this.ListPuntoVta.SelectedValue + "&e=" + this.ListEstados.SelectedValue);
            }
            catch (Exception ex)
            {                

            }
        }
        private void VerificarSolicitud(object sender, EventArgs e)
        {
            try
            {
                ControladorPlenario contPlenario = new ControladorPlenario();
                
                string idBoton = (sender as LinkButton).ID;
                int Solicitud = Convert.ToInt32(idBoton.Split('_')[1]);

                int i = contPlenario.validarSolicitudPlenario(Solicitud);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Solicitud validada id: " + Solicitud);
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "$.msgbox(\"Solicitud validada con exito!. \");", true);
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Solicitud validada con exito!. \");", true);                                            
                }
                else
                {
                    if (i == -3)
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "$.msgbox(\"No se pudo verificar solicitud. Capital no coincide!. \");", true);
                    if (i == -4)
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "$.msgbox(\"No se pudo verificar solicitud. Anticipo no coincide!. \");", true);                    
                    if (i == -2)
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "$.msgbox(\"No se pudo verificar solicitud. Nro de solicitud no encontrado!. \");", true);
                    if (i == -1)
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "$.msgbox(\"No se pudo verificar solicitud. \");", true);                                       
                }
                this.cargarCreditosAValidar();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Error al verificar solicitud. " + ex.Message + " \", {type: \"error\"});", true); ;
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al verificar solicitud. " + ex.Message));
            }
        }
        private void modificarSolicitud(object sender, EventArgs e)
        {
            try
            {
                string ID = (sender as LinkButton).ID;
                this.lblIdSolicitud.Text = ID.Split('_')[1];

                int idSolicitud = Convert.ToInt32(ID.Split('_')[1]);
                Planario_Api.Plenario p = new Planario_Api.Plenario();

                SolicitudPlenario sol = p.obtenerSolicitudesGestionById(idSolicitud);

                if (sol != null)
                {
                    this.txtDniModificar.Text = sol.Dni;
                    this.txtNroModificar.Text = sol.NroSolicitud.ToString();
                }

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirModalEditar();", true);

            }
            catch
            {

            }
        }
        protected void lbtnEditarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                int idSolicitud = Convert.ToInt32(this.lblIdSolicitud.Text);
                Planario_Api.Plenario p = new Planario_Api.Plenario();

                SolicitudPlenario sol = p.obtenerSolicitudesGestionById(idSolicitud);
                sol.Dni = this.txtDniModificar.Text;
                sol.NroSolicitud = Convert.ToInt32(this.txtNroModificar.Text);

                int ok = p.modificarSolicitud(sol);
                if (ok >= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Modificado con exito!. \", {type: \"info\"});cerrarModalEditar();", true);
                    this.cargarCreditosAValidar();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo modificar. \");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error." + ex.Message + " \");", true);
            }
        }
        protected void BtnValidarCreditosManuales_Click(object sender, EventArgs e)
        {
            try
            {
                this.validarCreditosManuales();
            }
            catch
            {

            }
        }
        private void validarCreditosManuales()
        {
            try
            {
                ControladorPlenario contPlenario = new ControladorPlenario();
                int validadas = 0;
                int total = 0;

                foreach (Control control in phCreditos.Controls)
                {
                    TableRow tr = control as TableRow;
                    LinkButton btn = tr.Cells[5].Controls[0] as LinkButton;
                    if (btn.ID.Contains("btnNO_"))
                    {
                        int Solicitud = Convert.ToInt32(btn.ID.Split('_')[1]);
                        int i = contPlenario.validarSolicitudPlenario(Solicitud);
                        if (i > 0)
                        {
                            validadas++;
                        }
                        total++;
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Solicitudes validadas: " + validadas + " de " + total + ". \", {type: \"info\"});", true);
                this.cargarCreditosAValidar();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Ocurrio un error validando solicitudes. " + ex.Message + ". \", {type: \"error\"});", true);
            }
        }
        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
            }
            catch
            {

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSucursalByEmpresa(Convert.ToInt32(this.ListEmpresa.SelectedValue));
            }
            catch
            {

            }
        }
    }
}