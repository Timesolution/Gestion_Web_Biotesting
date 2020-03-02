using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMConceptos : System.Web.UI.Page
    {        
        
        controladorUsuario contUser = new controladorUsuario();
        ControladorBanco contBancos = new ControladorBanco();
        ControladorPlanCuentas contPlanCta = new ControladorPlanCuentas();

        Mensajes m = new Mensajes();

        int id;
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.id = Convert.ToInt32(Request.QueryString["id"]);
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);

                if (!IsPostBack)
                {
                    this.cargarPlanCuentas();
                    if (this.accion == 2)
                    {
                        this.cargarConcepto(this.id);
                    }
                }
                this.cargarConceptos();
            }
            catch(Exception ex)
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
                    
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Bancos.Entidadades Bancarias") != 1)
                    if (this.verificarAcceso() != 1)
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
                        if (s == "91")
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
        private void cargarPlanCuentas()
        {
            try
            {
                List<Cuentas_Contables> ctas = this.contPlanCta.obtenerCuentasContables();

                foreach (var c in ctas)
                {
                    string texto = c.Codigo + " - " + c.Descripcion;
                    this.ListCuentasContables.Items.Add(new ListItem(texto, c.Id.ToString()));
                }

                this.ListCuentasContables.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarConcepto(int id)
        {
            try
            {
                Bancos_Conceptos concepto = this.contBancos.obtenerConceptosBancariosByID(this.id);
                this.txtConcepto.Text = concepto.Descripcion;
                this.ListCuentasContables.SelectedValue = concepto.PlanCuenta.Value.ToString();
            }
            catch
            {

            }
        }
        private void cargarConceptos()
        {
            try
            {
                this.phConceptos.Controls.Clear();
                List<Bancos_Conceptos> conceptos = this.contBancos.obtenerConceptosBancarios();

                foreach (var c in conceptos)
                {
                    this.cargarConceptosPH(c);
                }
            }
            catch
            {

            }
        }
        private void cargarConceptosPH(Bancos_Conceptos c)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celConcepto = new TableCell();
                celConcepto.Text = c.Descripcion;
                celConcepto.HorizontalAlign = HorizontalAlign.Left;
                celConcepto.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celConcepto);

                TableCell celCuenta = new TableCell();
                Cuentas_Contables plancta = this.contPlanCta.obtenerCuentaById(c.PlanCuenta.Value);
                if (plancta != null)
                {
                    celCuenta.Text = plancta.Codigo + " - " + plancta.Descripcion;
                }
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCuenta);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + c.Id;
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.editarConcepto);
                celAccion.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + c.Id;
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + c.Id + ");";
                celAccion.Controls.Add(btnEliminar);

                tr.Controls.Add(celAccion);

                this.phConceptos.Controls.Add(tr);
            }
            catch(Exception ex)
            {

            }
        }
        private void editarConcepto(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID.ToString().Split('_')[1];

                Response.Redirect("ABMConceptos.aspx?accion=2&id=" + id);
            }
            catch
            {

            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                {
                    this.modificarConcepto();
                }
                else
                {
                    this.agregarConcepto();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando banco . " + ex.Message));                
            }
        }
        private void agregarConcepto()
        {
            try
            {
                Bancos_Conceptos concepto = new Bancos_Conceptos();
                concepto.PlanCuenta = Convert.ToInt32(this.ListCuentasContables.SelectedValue);
                concepto.Descripcion = this.txtConcepto.Text;
                concepto.Estado = 1;
                
                int ok = this.contBancos.agregarConceptoBancario(concepto);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Guardado con exito!", "ABMConceptos.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puedo guardar"));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando concepto . " + ex.Message));                
            }
        }
        private void modificarConcepto()
        {
            try
            {
                Bancos_Conceptos concepto = this.contBancos.obtenerConceptosBancariosByID(this.id);
                concepto.PlanCuenta = Convert.ToInt32(this.ListCuentasContables.SelectedValue);
                concepto.Descripcion = this.txtConcepto.Text;

                int ok = this.contBancos.modificarConceptoBancario(concepto);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Guardado con exito!", "ABMConceptos.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puedo guardar"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando concepto . " + ex.Message));
            }
        }
        private void limpiarCampos()
        {
            try
            {
                
            }
            catch
            { 

            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.txtMovimiento.Text);
                Bancos_Conceptos c = this.contBancos.obtenerConceptosBancariosByID(id);
                c.Estado = 0;
                int i = this.contBancos.modificarConceptoBancario(c);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Concepto bancario: " + c.Descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Concepto eliminado con exito", Request.Url.ToString()));                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo eliminar"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar. " + ex.Message));
            }
        }

        protected void lbtnBuscarCta_Click(object sender, EventArgs e)
        {
            try
            {
                string desc = this.txtBusquedaCta.Text;
                this.ListCuentasContables.Items.Clear();
                List<Cuentas_Contables> ctas = this.contPlanCta.obtenerCuentasContablesByDesc(desc);

                foreach (var c in ctas)
                {
                    string texto = c.Codigo + " - " + c.Descripcion;
                    this.ListCuentasContables.Items.Add(new ListItem(texto, c.Id.ToString()));
                }
                
            }
            catch
            {

            }
        }
    }
}