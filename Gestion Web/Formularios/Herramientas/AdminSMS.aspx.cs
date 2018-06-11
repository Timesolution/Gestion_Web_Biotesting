﻿using Disipar.Models;
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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class AdminSMS : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        ControladorPlenario contPlenario = new ControladorPlenario();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorVendedor contVend = new controladorVendedor();
        controladorSucursal contSuc = new controladorSucursal();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int idUsuario;
        private string dni;
        private string telefono;
        private int empresa;
        private int sucursal;
        private int vendedor;
        private int estado;

        //permisos
        int permisoValidar;
        int permisoOmitir;
        int permisoAnular;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.dni = Request.QueryString["dni"];
                this.telefono = Request.QueryString["tel"];
                this.empresa = Convert.ToInt32(Request.QueryString["emp"]);
                this.sucursal = Convert.ToInt32(Request.QueryString["suc"]);
                this.vendedor = Convert.ToInt32(Request.QueryString["ven"]);
                this.estado = Convert.ToInt32(Request.QueryString["est"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaD) && String.IsNullOrEmpty(this.fechaD))
                    {
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        this.empresa = (int)Session["Login_EmpUser"];
                        this.sucursal = (int)Session["Login_SucUser"];
                    }
                    this.cargarEmpresas();
                    this.cargarSucursalByEmpresa(this.empresa);
                    this.cargarVendedor(this.sucursal);

                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                    this.txtDniBuscar.Text = this.dni;
                    this.txtTelefonoBuscar.Text = this.telefono;
                    this.ListEmpresa.SelectedValue = this.empresa.ToString();
                    this.ListSucursal.SelectedValue = this.sucursal.ToString();
                    this.ListVendedor.SelectedValue = this.vendedor.ToString();
                    this.ListEstado.SelectedValue = this.estado.ToString();
                }                
                this.cargarCodigos();
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
                //verifico si es super admin
                string perfil = Session["Login_NombrePerfil"] as string;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                int ok = 0;
                if (perfil == "SuperAdministrador")
                {
                    this.ListSucursal.Attributes.Remove("disabled");
                    this.ListEmpresa.Attributes.Remove("disabled");
                    this.permisoAnular = 1;
                    this.permisoOmitir = 1;
                    this.permisoValidar = 1;
                    return 1;
                }
                else
                {
                    foreach (string s in listPermisos)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            if (s == "92")//para entrar
                            {
                                ok = 1;
                            }
                            if (s == "93")//para validar
                            {
                                this.permisoValidar = 1;
                            }
                            if (s == "94")//para omitir
                            {
                                this.permisoOmitir = 1;
                            }
                            if (s == "95")//para anular
                            {
                                this.permisoAnular = 1;
                            }
                            if (s == "96")//para cambio de suc
                            {
                                this.ListSucursal.Attributes.Remove("disabled");
                                this.ListEmpresa.Attributes.Remove("disabled");
                            }
                        }
                    }
                }

                return ok;
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();
                
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
        public void cargarVendedor(int idSuc)
        {
            try
            {
                this.ListVendedor.Items.Clear();
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedoresBySuc(idSuc);
                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedor.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        private void cargarCodigos()
        {
            try
            {
                this.phCodigos.Controls.Clear();

                decimal cantidad = 0;
                decimal validos = 0;
                decimal aValidar= 0;
                decimal total = 0;

                DateTime desde = Convert.ToDateTime(this.txtFechaDesde.Text,new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<CodigosTelefono> registros = this.contPlenario.obtenerCodigos(this.txtDniBuscar.Text, this.txtTelefonoBuscar.Text, desde, hasta, Convert.ToInt32(this.ListEmpresa.SelectedValue), Convert.ToInt32(this.ListSucursal.SelectedValue), Convert.ToInt32(this.ListVendedor.SelectedValue), Convert.ToInt32(this.ListEstado.SelectedValue));

                foreach (var reg in registros)
                {
                    this.cargarCodigosPH(reg);
                    if (reg.Codigo != null)
                        cantidad++;

                    if (reg.Estado == 0)
                    {
                        aValidar++;
                    }
                    if (reg.Estado == 1 || reg.Estado == 2)
                    {
                        validos++;
                    }
                }

                this.labelSaldo.Text = cantidad.ToString();
                this.labelValidos.Text = validos.ToString();
                this.labelValidar.Text = aValidar.ToString();
                this.labelTotal.Text = registros.Count().ToString();
            }
            catch
            {

            }
        }
        private void cargarCodigosPH(CodigosTelefono reg)
        {
            try 
            {
                TableRow tr = new TableRow();
                tr.ID = reg.Id.ToString();
                if (reg.Estado == 9)
                    tr.ForeColor = System.Drawing.Color.Red;

                TableCell celFecha = new TableCell();
                celFecha.Text = reg.FechaHora.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Center;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celDni = new TableCell();
                celDni.Text = reg.DNI;
                celDni.HorizontalAlign = HorizontalAlign.Center;
                celDni.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDni);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = reg.Telefono;
                celTelefono.HorizontalAlign = HorizontalAlign.Center;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTelefono);

                TableCell celCodigo = new TableCell();
                if (!String.IsNullOrEmpty(reg.Codigo))
                    celCodigo.Text = reg.Codigo.Substring(0, 1) + "***";
                celCodigo.HorizontalAlign = HorizontalAlign.Center;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celMotivo = new TableCell();
                celMotivo.Text = reg.Motivo;
                celMotivo.HorizontalAlign = HorizontalAlign.Center;
                celMotivo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMotivo);

                TableCell celDoc = new TableCell();
                celDoc.HorizontalAlign = HorizontalAlign.Center;
                celDoc.VerticalAlign = VerticalAlign.Middle;
                TableCell celSucursal = new TableCell();
                celSucursal.HorizontalAlign = HorizontalAlign.Center;
                celSucursal.VerticalAlign = VerticalAlign.Middle;                
                TableCell celVendedor = new TableCell();
                celVendedor.HorizontalAlign = HorizontalAlign.Center;
                celVendedor.VerticalAlign = VerticalAlign.Middle;

                if (reg.Factura != null)
                {
                    //Factura fact = this.contFact.obtenerFacturaId(reg.Factura.Value);
                    DataTable fact = this.contFact.obtenerFacturaIdDT(reg.Factura.Value);
                    if (fact != null && fact.Rows.Count > 0)
                    {
                        //celDoc.Text = fact.tipo.tipo + " Nº" + fact.numero;
                        celDoc.Text = fact.Rows[0]["tipo"] + " Nº" + fact.Rows[0]["numero"];
                    }
                }
                if (reg.IdVendedor != null)
                {
                    Vendedor vend = this.contVend.obtenerVendedorID(reg.IdVendedor.Value);
                    if (vend != null)
                        celVendedor.Text = vend.emp.nombre + " " + vend.emp.apellido;
                }
                if (reg.IdSucursal != null)
                {
                    Sucursal s = this.contSuc.obtenerSucursalID(reg.IdSucursal.Value);
                    celSucursal.Text = s.nombre;
                }
                tr.Cells.Add(celDoc);
                tr.Cells.Add(celSucursal);
                tr.Cells.Add(celVendedor);

                TableCell celAccion = new TableCell();
                celAccion.HorizontalAlign = HorizontalAlign.Center;
                celAccion.VerticalAlign = VerticalAlign.Middle;

                LinkButton btnValidar = new LinkButton();
                btnValidar.ID = "btnValidar_" + reg.Id;
                if (reg.Estado.Value > 0)
                    btnValidar.CssClass = "btn btn-success";
                else
                {
                    btnValidar.CssClass = "btn btn-danger ui-tooltip";
                    btnValidar.Attributes.Add("data-toggle", "modal");
                    btnValidar.Attributes.Add("href", "#modalValidar");                    
                    btnValidar.OnClientClick = "abrirdialog2(" + reg.Id + ");";                    
                    btnValidar.Attributes.Add("title data-original-title", "Validar");
                }
                btnValidar.Text = "<span class='shortcut-icon icon-ok'></span>";

                if (this.permisoValidar == 1)
                    celAccion.Controls.Add(btnValidar);

                Literal lit = new Literal();
                lit.Text = "&nbsp";
                celAccion.Controls.Add(lit);

                LinkButton btnOmitir = new LinkButton();
                btnOmitir.ID = "btnOmitir_" + reg.Id;
                btnOmitir.CssClass = "btn btn-info ui-tooltip";
                btnOmitir.Attributes.Add("data-toggle", "modal");
                btnOmitir.Attributes.Add("href", "#modalConfirmacion2");
                btnOmitir.Text = "<span class='shortcut-icon fa fa-times'></span>";
                btnOmitir.OnClientClick = "abrirdialog3(" + reg.Id + ");";
                btnOmitir.Attributes.Add("title data-original-title", "Omitir");
                if (reg.Estado > 0)
                    btnOmitir.Attributes.Add("disabled", "disabled");

                if (this.permisoOmitir == 1)
                    celAccion.Controls.Add(btnOmitir);

                Literal lit2 = new Literal();
                lit2.Text = "&nbsp";
                celAccion.Controls.Add(lit2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + reg.Id;
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + reg.Id + ");";
                btnEliminar.Attributes.Add("title data-original-title", "Eliminar");

                if (this.permisoAnular == 1)
                    celAccion.Controls.Add(btnEliminar);

                celAccion.Width = Unit.Percentage(20);
                tr.Cells.Add(celAccion);

                this.phCodigos.Controls.Add(tr);
            }
            catch
            {

            }
        }        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AdminSMS.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&dni=" + this.txtDniBuscar.Text + "&tel=" + this.txtTelefonoBuscar.Text + "&emp=" + this.ListEmpresa.SelectedValue + "&suc=" + this.ListSucursal.SelectedValue + "&ven=" + this.ListVendedor.SelectedValue + "&est=" + this.ListEstado.SelectedValue);
            }
            catch (Exception ex)
            {                

            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idRegistro = Convert.ToInt32(this.txtMovimiento.Text);
                CodigosTelefono reg = this.contPlenario.obtenerCodigoByID(idRegistro);
                Usuario user = this.controlador.obtenerUsuariosID((int)Session["Login_IdUser"]);

                //int i = this.contPlenario.anularRegistroTelefonoDniByID(idRegistro);
                int i = this.contPlenario.anularRegistrosByTelefono(reg.Telefono);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Telefono: " + reg.Telefono + " del DNI: " + reg.DNI + " por el usuario: " + user.usuario);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Registro eliminado con exito", null));
                    this.cargarCodigos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando registro"));

                }
            }
            catch
            {

            }
        }
        protected void btnEnviarCodigoValidar_Click(object sender, EventArgs e)
        {
            try
            {
                Planario_Api.Plenario p = new Planario_Api.Plenario();
                if (this.txtCodAreaValidar.Text.Substring(0,1) == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Codigo de area no debe comenzar con 0 (cero)!. \");", true);
                    return;
                }

                if (this.txtCodAreaValidar.Text.Length + this.txtTelefonoValidar.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");", true);
                    return;
                }

                //CodigosTelefono registro = contPlenario.obtenerCodigoByID(Convert.ToInt32(txtMovimientoValidar.Text));                
                CodigosTelefono registro = p.obtenerRegistroTelefonoDNI(Convert.ToInt32(txtMovimientoValidar.Text));
                string telefono = "+549" + this.txtCodAreaValidar.Text + this.txtTelefonoValidar.Text;//+54 9 cod + tel

                int ok = contPlenario.validarTelefonoDNI(registro.DNI, telefono);
                if(ok > 0)
                {
                    registro.Telefono = telefono;
                    p.modificarTelefonoDNI(registro);
                    //contPlenario.modificarCodigoTelefono(registro);

                    int envioCodigo = contPlenario.enviarCodigoTelefonoModificar(registro.Id);                    
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Codigo enviado. \", {type: \"info\"});desbloquearCodigo();", true);

                    try
                    {
                        Usuario user = this.controlador.obtenerUsuariosID((int)Session["Login_IdUser"]);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Envio codigo para revalidacion por el usuario: " + user.usuario + " para DNI: " + registro.DNI + " TEL: " + telefono);
                    }
                    catch { }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Este telefono ya fue utilizado con otro DNI!. \");", true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCodAreaValidar.Text.Length + this.txtTelefonoValidar.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");", true);
                    return;
                }

                CodigosTelefono registro = contPlenario.obtenerCodigoByID(Convert.ToInt32(txtMovimientoValidar.Text));
                string telefono = "+549" + this.txtCodAreaValidar.Text + this.txtTelefonoValidar.Text;//+54 9 cod + tel

                int ok = contPlenario.validarCodigoVerificacion(registro.DNI, telefono, this.txtCodigoValidar.Text);
                if (ok > 0)
                {
                    //contPlenario.validarTelefonoDNITodos(registro.DNI, telefono);
                    try
                    {
                        Usuario user = this.controlador.obtenerUsuariosID((int)Session["Login_IdUser"]);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Registro ID: " + registro.Id + " validado por el usuario: " + user.usuario + " para DNI: " + registro.DNI + " TEL: " + telefono);
                    }
                    catch { }
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo validado. \", {type: \"info\"});bloquear();location.href = '" + Request.Url + "';", true);
                    this.txtCodAreaValidar.Text = "";
                    this.txtTelefonoValidar.Text = "";
                    this.txtCodigoValidar.Text = "";
                    this.cargarCodigos();
                }
                else
                {
                    if (ok == -2)
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo incorrecto. \");desbloquearCodigo();", true);
                    else
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"No se pudo validar. \", {type: \"error\"});desbloquearCodigo();", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error. \", {type: \"error\"});desbloquearCodigo();", true);
            }
        }
        protected void btnSiOmitir_Click(object sender, EventArgs e)
        {
            try
            {
                int idRegistro = Convert.ToInt32(this.txtMovimientoOmitir.Text);
                CodigosTelefono reg = this.contPlenario.obtenerCodigoByID(idRegistro);
                Usuario user = this.controlador.obtenerUsuariosID((int)Session["Login_IdUser"]);

                reg.Estado = 2;

                int i = this.contPlenario.modificarCodigoTelefono(reg);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Omitio validacion ID: "+ reg.Id +" Telefono: " + reg.Telefono + " del DNI: " + reg.DNI + " por el usuario: " + user.usuario);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito", null));
                    this.cargarCodigos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo validar."));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ha ocurrido un error. " + ex.Message));
            }
        }
        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (Convert.ToInt32(this.ListSucursal.SelectedValue) > 0)
                    this.cargarVendedor(Convert.ToInt32(this.ListSucursal.SelectedValue));
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
