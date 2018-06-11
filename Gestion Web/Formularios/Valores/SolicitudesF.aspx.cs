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
    public partial class SolicitudesF : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        ControladorPlenario contPlenario = new ControladorPlenario();
        controladorFacturacion contFc = new controladorFacturacion();
        controladorVendedor contVend = new controladorVendedor();
        controladorSucursal contSucu = new controladorSucursal();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;                
        private int empresa;
        private int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.empresa = Convert.ToInt32(Request.QueryString["emp"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaD) && String.IsNullOrEmpty(this.fechaH))
                    {
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        this.empresa = (int)Session["Login_EmpUser"];
                    }
                    this.cargarEmpresas();

                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                    this.ListEmpresa.SelectedValue = this.empresa.ToString();
                    this.ListEstado.SelectedValue = this.estado.ToString();
                }

                //para evitar que cuando hago click en el buscar, la carga se haga mas rapida
                Control control = null;
                string controlName = Request.Params["__EVENTTARGET"];
                if (!String.IsNullOrEmpty(controlName))
                {
                    control = FindControl(controlName);
                    if (control != null)
                    {
                        if (!control.ID.Contains("btnBuscar"))
                        {
                            this.cargarSolicitudes();
                        }
                    }
                    else
                    {
                        this.cargarSolicitudes();
                    }
                }
                else
                {
                    this.cargarSolicitudes();
                }
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
        public void cargarEmpresas()
        {
            try
            {
                
                DataTable dt = this.contSucu.obtenerEmpresas();

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();
                this.ListEmpresa.Items.Insert(0, new ListItem("Seleccione...", "-1"));                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        private void cargarSolicitudes()
        {
            try
            {
                

                if (!String.IsNullOrEmpty(this.txtFechaDesde.Text))
                {
                    DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                    DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                    List<Planario_Api.Entidades.SolicitudPlenario> solicitudes = contPlenario.obtenerSolicitudesPlenariosByFecha(desde, hasta,this.empresa);

                    this.phCreditos.Controls.Clear();
                    foreach (var s in solicitudes)
                    {
                        this.cargarSolicitudPH(s);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarSolicitudPH(Planario_Api.Entidades.SolicitudPlenario s)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                //tr.ID = s.NroSolicitud.ToString();
                //tr.ID = "TR_" + s.NroSolicitud.ToString() + "_" + s.Id.ToString();

                string nombreEmp = "";
                string nombreSuc = "";
                Empresa emp = this.contPlenario.obtenerEmpresaByIdEntidadPlenario(s.IdEntidad);
                Sucursal sucur = this.contPlenario.obtenerSucursalByIdEntidadPlenario(s.IdEntidad);

                if (emp != null)
                {
                    nombreEmp = emp.RazonSocial;
                }
                if (sucur != null)
                {
                    nombreSuc = sucur.nombre;
                }

                TableCell celEmpresaPlena = new TableCell();
                celEmpresaPlena.Text = nombreEmp;
                //celEmpresaPlena.Width = Unit.Percentage(15);
                celEmpresaPlena.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEmpresaPlena);

                TableCell celSucursalPlena = new TableCell();
                celSucursalPlena.Text = nombreSuc;
                //celSucursalPlena.Width = Unit.Percentage(15);
                celSucursalPlena.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalPlena);

                DataTable fact = new DataTable();
                if (s.Factura != null)
                {
                    fact = contFc.obtenerFacturaIdDT(s.Factura.Value);
                }
                string fechaDoc = "";
                string nroDoc = "";
                string sucDoc = "";
                if (fact != null && fact.Rows.Count > 0)
                {
                    fechaDoc = Convert.ToDateTime(fact.Rows[0]["fecha"].ToString()).ToString("dd/MM/yyyy");
                    nroDoc = fact.Rows[0]["tipo"].ToString() + " " + fact.Rows[0]["numero"].ToString();
                    Sucursal sucurDoc = this.contSucu.obtenerSucursalID(Convert.ToInt32(fact.Rows[0]["Id_Suc"].ToString()));
                    sucDoc = sucurDoc.nombre;
                }

                TableCell celFechaDoc = new TableCell();
                celFechaDoc.Text = fechaDoc;
                //celFechaDoc.Width = Unit.Percentage(10);
                celFechaDoc.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFechaDoc);

                TableCell celDoc = new TableCell();
                celDoc.Text = nroDoc;
                //celDoc.Width = Unit.Percentage(20);
                celDoc.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDoc);

                TableCell celSucDoc = new TableCell();
                celSucDoc.Text = sucDoc;
                //celSucDoc.Width = Unit.Percentage(20);
                celSucDoc.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucDoc);

                TableCell celFecha = new TableCell();
                celFecha.Text = s.FechaOperacion.ToString("dd/MM/yyyy");
                //celFecha.Width = Unit.Percentage(10);
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celDNI = new TableCell();
                celDNI.Text = s.Dni.ToString();
                //celDNI.Width = Unit.Percentage(10);
                celDNI.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDNI);

                TableCell celNumero = new TableCell();
                celNumero.Text = s.NroSolicitud.ToString();
                //celNumero.Width = Unit.Percentage(10);
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celCapital = new TableCell();
                celCapital.Text = "$" + s.Capital;
                //celCapital.Width = Unit.Percentage(10);
                celCapital.VerticalAlign = VerticalAlign.Middle;
                celCapital.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCapital);

                TableCell celAnticipo = new TableCell();
                celAnticipo.Text = "$" + s.Anticipo;
                //celAnticipo.Width = Unit.Percentage(10);
                celAnticipo.VerticalAlign = VerticalAlign.Middle;
                celAnticipo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celAnticipo);

                
                

                bool estadoSolicitud = true;
                if (nombreSuc != "" && nombreSuc != sucDoc)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                    tr.Font.Bold = true;                    
                    estadoSolicitud = false;
                    
                }

                if (this.estado == 0)
                {
                    this.phCreditos.Controls.Add(tr);
                }
                else
                {
                    if (estadoSolicitud == true && this.estado == 1)
                    {
                        this.phCreditos.Controls.Add(tr);
                    }
                    if (estadoSolicitud == false && this.estado == 2)
                    {
                        this.phCreditos.Controls.Add(tr);
                    }
                }
            }
            catch
            {

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

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirModalEditar()", true);

            }
            catch
            {

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SolicitudesF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&emp=" + this.ListEmpresa.SelectedValue + "&e=" + this.ListEstado.SelectedValue);
            }
            catch (Exception ex)
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

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Modificado con exito!. \");cerrarModalEditar();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo modificar. \");", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error." + ex.Message + " \");", true);
            }
        }
    }
}