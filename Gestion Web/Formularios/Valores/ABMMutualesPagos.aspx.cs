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

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMMutualesPagos : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();
        //valores
        private int valor;
        private int idMutual;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idMutual = Convert.ToInt32(Request.QueryString["id"]);
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    this.cargarMutuales();
                    if (valor == 2)
                    {
                        this.cargarMutualPago(idMutual);
                    }                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
        public void cargarMutuales()
        {
            try
            {
                controladorFactEntity controlMutual = new controladorFactEntity();
                List<Gestion_Api.Entitys.Mutuale> mutuales = controlMutual.obtenerMutuales();
                this.ListMutuales.DataSource = mutuales;
                this.ListMutuales.DataValueField = "Id";
                this.ListMutuales.DataTextField = "Nombre";

                this.ListMutuales.DataBind();

                this.ListMutuales.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarMutualPago(int id)
        {
            try
            {
                Mutuales_Pagos m = this.controlador.obtenerMutualPagosByID(id);
                if (m != null)
                {
                    this.txtNombre.Text = m.Nombre;
                    this.txtCuotas.Text = m.Cuotas.ToString();
                    this.txtCoeficiente.Text = m.Coeficiente.ToString();
                    this.ListMutuales.SelectedValue = m.Id_Mutual.ToString();
                }
            }
            catch
            {

            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.valor > 0)
                    this.modificarMutual();
                else
                    this.agregarMutual();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Mututal. " + ex.Message));
            }

        }
        public void agregarMutual()
        {
            try
            {
                Mutuales_Pagos m = new Mutuales_Pagos();
                m.Id_Mutual = Convert.ToInt32(this.ListMutuales.SelectedValue);
                m.Nombre = this.txtNombre.Text;
                m.Coeficiente = Convert.ToDecimal(this.txtCoeficiente.Text);
                m.Cuotas = Convert.ToInt32(this.txtCuotas.Text);
                m.Estado = 1;
                int i = this.controlador.agregarMutualPagos(m);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego Mutual Cuota: " + m.Nombre);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Agregada con exito!. \", {type: \"info\"});location.href = 'MutualesPagosF.aspx';", true);
                    this.borrarCampos();                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando. \", {type: \"error\"});", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando. " + ex.Message + " \", {type: \"error\"});", true);
            }
        }
        public void modificarMutual()
        {
            try
            {
                Mutuales_Pagos m = this.controlador.obtenerMutualPagosByID(this.idMutual);
                m.Id_Mutual = Convert.ToInt32(this.ListMutuales.SelectedValue);
                m.Nombre = this.txtNombre.Text;
                m.Coeficiente = Convert.ToDecimal(this.txtCoeficiente.Text);
                m.Cuotas = Convert.ToInt32(this.txtCuotas.Text);
                int i = this.controlador.modificarMutualPagos(m);
                if (i >= 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Mutual Pago: " + m.Nombre);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Modificado con exito!. \", {type: \"info\"});location.href = 'MutualesPagosF.aspx';", true);
                    this.borrarCampos();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }
        public void borrarCampos()
        {
            try
            {
                this.txtNombre.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }        

    }
}