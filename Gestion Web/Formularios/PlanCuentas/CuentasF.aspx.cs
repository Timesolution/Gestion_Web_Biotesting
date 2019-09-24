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

namespace Gestion_Web.Formularios.PlanCuentas
{
    public partial class CuentasF : System.Web.UI.Page
    {
        ControladorPlanCuentas contPlanCta = new ControladorPlanCuentas();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();

        int id;
        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.id = Convert.ToInt32(Request.QueryString["id"]);

                if (!IsPostBack)
                {
                    if (this.accion == 2)
                    {
                        this.cargarDatosCta();
                    }
                    this.cargarCuentasNivel1();
                }

                this.cargarCuentas();
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
                int tienePermiso = 0;
                int tienePermisoParaEditarCodigo = 0;

                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "171")
                            tienePermiso = 1;

                        if (tienePermiso == 1)
                        {
                            if (s == "172")
                                tienePermisoParaEditarCodigo = 1;
                        }
                    }
                }

                if (tienePermisoParaEditarCodigo == 0)
                {
                    txtCodigo.Enabled = false;
                    txtCodigo.CssClass = "form-control";
                }

                return tienePermiso;
            }
            catch
            {
                return -1;
            }
        }
        private void cargarCuentas()
        {
            try
            {
                this.phCuentas.Controls.Clear();

                List<Cuentas_Contables> cuentas = this.contPlanCta.obtenerCuentasContables();

                foreach (var cta in cuentas)
                {
                    this.cargarCuentaPH(cta);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void cargarCuentaPH(Cuentas_Contables cta)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = cta.Id.ToString();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = cta.Codigo;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCuenta = new TableCell();
                celCuenta.Text = cta.Descripcion;
                celCuenta.HorizontalAlign = HorizontalAlign.Left;
                celCuenta.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCuenta);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + cta.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarCuenta);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + cta.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + cta.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                this.phCuentas.Controls.Add(tr);
            }
            catch (Exception ex)
            {

            }
        }
        private void agregarCuenta()
        {
            try
            {
                Cuentas_Contables cta = new Cuentas_Contables();
                cta.Codigo = this.txtCodigo.Text;
                cta.Descripcion = this.txtDescripcion.Text;
                cta.Jerarquia = Convert.ToInt32(this.ListJerarquia.SelectedValue);
                cta.Nivel1 = 0;
                cta.Nivel2 = 0;
                cta.Nivel3 = 0;
                cta.Nivel4 = 0;

                if (cta.Jerarquia == 2)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                }
                if (cta.Jerarquia == 3)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                    cta.Nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                }
                if (cta.Jerarquia == 4)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                    cta.Nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                    cta.Nivel3 = Convert.ToInt32(this.ListNivel3.SelectedValue);
                }
                if (cta.Jerarquia == 5)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                    cta.Nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                    cta.Nivel3 = Convert.ToInt32(this.ListNivel3.SelectedValue);
                    cta.Nivel4 = Convert.ToInt32(this.ListNivel4.SelectedValue);
                }

                cta.Estado = 1;

                int i = this.contPlanCta.agregarCuenta(cta);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Cuenta agregada con exito!. \", {type: \"info\"}); location.href = 'CuentasF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo agregar cuenta contable. \";", true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void modificarCuenta()
        {
            try
            {
                Cuentas_Contables cta = this.contPlanCta.obtenerCuentaById(this.id);
                cta.Descripcion = this.txtDescripcion.Text;
                cta.Jerarquia = Convert.ToInt32(this.ListJerarquia.SelectedValue);
                cta.Codigo = this.txtCodigo.Text;

                if (cta.Jerarquia == 2)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                }
                if (cta.Jerarquia == 3)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                    cta.Nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                }
                if (cta.Jerarquia == 4)
                {
                    cta.Nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                    cta.Nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                    cta.Nivel3 = Convert.ToInt32(this.ListNivel3.SelectedValue);
                }


                int i = this.contPlanCta.modificarCuenta(cta);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Cuenta modificada con exito!. \", {type: \"info\"}); location.href = 'CuentasF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo modificar cuenta contable. \";", true);
                }
            }
            catch
            {

            }
        }
        private void cargarDatosCta()
        {
            try
            {
                Cuentas_Contables cta = this.contPlanCta.obtenerCuentaById(this.id);
                this.txtCodigo.Text = cta.Codigo;
                this.txtDescripcion.Text = cta.Descripcion;
            }
            catch
            {

            }
        }
        private void obtenerProximoCodigo()
        {
            try
            {
                int jerarquia = Convert.ToInt32(this.ListJerarquia.SelectedValue);
                int nivel1 = Convert.ToInt32(this.ListNivel1.SelectedValue);
                int nivel2 = Convert.ToInt32(this.ListNivel2.SelectedValue);
                int nivel3 = Convert.ToInt32(this.ListNivel3.SelectedValue);
                int nivel4 = Convert.ToInt32(this.ListNivel4.SelectedValue);
                string codigo = "";

                if (jerarquia == 1)
                    codigo = this.contPlanCta.obtenerProximoCodigoCuenta(jerarquia, 0, 0, 0, 0);
                if (jerarquia == 2)
                    codigo = this.contPlanCta.obtenerProximoCodigoCuenta(jerarquia, nivel1, 0, 0, 0);
                if (jerarquia == 3)
                    codigo = this.contPlanCta.obtenerProximoCodigoCuenta(jerarquia, nivel1, nivel2, 0, 0);
                if (jerarquia == 4)
                    codigo = this.contPlanCta.obtenerProximoCodigoCuenta(jerarquia, nivel1, nivel2, nivel3, 0);
                if (jerarquia == 5)
                    codigo = this.contPlanCta.obtenerProximoCodigoCuenta(jerarquia, nivel1, nivel2, nivel3, nivel4);

                this.txtCodigo.Text = codigo;
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel1()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(1, 0);

                this.ListNivel1.DataSource = ctas.ToList();
                this.ListNivel1.DataValueField = "Id";
                this.ListNivel1.DataTextField = "Descripcion";
                this.ListNivel1.DataBind();

                this.ListNivel1.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel2()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(2, Convert.ToInt32(this.ListNivel1.SelectedValue));

                this.ListNivel2.DataSource = ctas.ToList();
                this.ListNivel2.DataValueField = "Id";
                this.ListNivel2.DataTextField = "Descripcion";
                this.ListNivel2.DataBind();

                this.ListNivel2.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel3()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(3, Convert.ToInt32(this.ListNivel2.SelectedValue));

                this.ListNivel3.DataSource = ctas.ToList();
                this.ListNivel3.DataValueField = "Id";
                this.ListNivel3.DataTextField = "Descripcion";
                this.ListNivel3.DataBind();

                this.ListNivel3.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel4()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(4, Convert.ToInt32(this.ListNivel3.SelectedValue));

                this.ListNivel4.DataSource = ctas.ToList();
                this.ListNivel4.DataValueField = "Id";
                this.ListNivel4.DataTextField = "Descripcion";
                this.ListNivel4.DataBind();

                this.ListNivel4.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idCta = Convert.ToInt32(this.txtMovimiento.Text);
                Cuentas_Contables cta = this.contPlanCta.obtenerCuentaById(idCta);
                cta.Estado = 0;
                int i = this.contPlanCta.modificarCuenta(cta);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Cuenta eliminado con exito!. \", {type: \"info\"}); location.href = 'CuentasF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo eliminar cuenta contable. \";", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo eliminar cuenta contable." + ex.Message + " .\";", true);
            }
        }
        private void editarCuenta(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CuentasF.aspx?a=2&id=" + (sender as LinkButton).ID.Split('_')[1]);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar cuenta. " + ex.Message));
            }
        }

        protected void ListJerarquia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ListJerarquia.SelectedValue == "1")
                {
                    this.panelNivel1.Visible = false;
                    this.panelNivel2.Visible = false;
                    this.panelNivel3.Visible = false;
                    this.panelNivel4.Visible = false;
                    this.obtenerProximoCodigo();
                }
                if (this.ListJerarquia.SelectedValue == "2")
                {
                    this.panelNivel1.Visible = true;
                    this.panelNivel2.Visible = false;
                    this.panelNivel3.Visible = false;
                    this.panelNivel4.Visible = false;
                }
                if (this.ListJerarquia.SelectedValue == "3")
                {
                    this.panelNivel1.Visible = true;
                    this.panelNivel2.Visible = true;
                    this.panelNivel3.Visible = false;
                    this.panelNivel4.Visible = false;
                }
                if (this.ListJerarquia.SelectedValue == "4")
                {
                    this.panelNivel1.Visible = true;
                    this.panelNivel2.Visible = true;
                    this.panelNivel3.Visible = true;
                    this.panelNivel4.Visible = false;
                }
                if (this.ListJerarquia.SelectedValue == "5")
                {
                    this.panelNivel1.Visible = true;
                    this.panelNivel2.Visible = true;
                    this.panelNivel3.Visible = true;
                    this.panelNivel4.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion == 2)
                    this.modificarCuenta();
                else
                    this.agregarCuenta();
            }
            catch
            {

            }
        }

        protected void ListNivel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.ListJerarquia.SelectedValue) == 2 && Convert.ToInt32(this.ListNivel1.SelectedValue) > 0)
                    this.obtenerProximoCodigo();

                this.cargarCuentasNivel2();
            }
            catch
            {

            }
        }

        protected void ListNivel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.ListJerarquia.SelectedValue) == 3 && Convert.ToInt32(this.ListNivel1.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel2.SelectedValue) > 0)
                    this.obtenerProximoCodigo();

                this.cargarCuentasNivel3();
            }
            catch
            {

            }
        }

        protected void ListNivel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.ListJerarquia.SelectedValue) == 4 && Convert.ToInt32(this.ListNivel1.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel2.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel3.SelectedValue) > 0)
                    this.obtenerProximoCodigo();

                this.cargarCuentasNivel4();
            }
            catch
            {

            }
        }

        protected void ListNivel4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.ListJerarquia.SelectedValue) == 5 && Convert.ToInt32(this.ListNivel1.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel2.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel3.SelectedValue) > 0 && Convert.ToInt32(this.ListNivel4.SelectedValue) > 0)
                    this.obtenerProximoCodigo();
            }
            catch
            {

            }
        }
    }
}