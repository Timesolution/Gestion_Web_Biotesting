using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Surtidores
{
    public partial class ABMSurtidoresCierre : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador        
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();
        private int accion;
        private int id;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.id = Convert.ToInt32(Request.QueryString["id"]);
                this.accion = Convert.ToInt32(Request.QueryString["a"]);

                this.VerificarLogin();                
                if (!IsPostBack)
                {
                    this.txtFechaCierre.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    this.txtHoraCierre.Text = DateTime.Now.ToString("HH:mm");
                    this.cargarSurtidores();
                    this.cargarUsuarios();
                    this.ListUsuario.SelectedValue = Session["Login_IdUser"].ToString();
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
                        if (s == "100")
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
        private void cargarSurtidores()
        {
            try
            {
                List<Surtidore> list = this.controlador.obtenerSurtidores();
                this.ListSurtidor.DataSource = list;
                this.ListSurtidor.DataTextField = "Descripcion";
                this.ListSurtidor.DataValueField = "Id";
                this.ListSurtidor.DataBind();

                this.ListSurtidor.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarUsuarios()
        {
            try
            {
                DataTable dt = this.contUser.obtenerUsuarios();                
                this.ListUsuario.DataSource = dt;
                this.ListUsuario.DataTextField = "usuario";
                this.ListUsuario.DataValueField = "id";
                this.ListUsuario.DataBind();
                                
                this.ListUsuario.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void agregarCierreSurtidor()
        {
            try
            {
                Surtidore s = this.controlador.obtenerSurtidorByID(Convert.ToInt32(this.ListSurtidor.SelectedValue));
                Surtidores_Cierre cierre = new Surtidores_Cierre();
                string fh = this.txtFechaCierre.Text + " " + this.txtHoraCierre.Text;
                cierre.Fecha = Convert.ToDateTime(fh, new CultureInfo("es-AR"));
                cierre.IdSurtidor = Convert.ToInt32(this.ListSurtidor.SelectedValue);
                cierre.IdUsuario = Convert.ToInt32(this.ListUsuario.SelectedValue);
                cierre.Observaciones = this.txtObservacion.Text;
                cierre.CantidadInicial = Convert.ToDecimal(this.txtCantInicial.Text);
                cierre.CantidadCierre= Convert.ToDecimal(this.txtCantCierre.Text);
                cierre.PrecioVenta = s.Precio;
                cierre.Estado = 1;

                int ok = this.controlador.agregarCierreSurtidor(cierre);
                if (ok > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego cierre surtidor : " + cierre.Id);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Cierre Surtidor agregado con exito!. \", {type: \"info\"});location.href = 'SurtidoresCierreF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando Cierre surtidor. \", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error. " + ex.Message + ". \", {type: \"error\"});", true);
            }
        }
        private void calcularTotalCantidades()
        {
            try
            {
                int idSurt = Convert.ToInt32(this.ListSurtidor.SelectedValue);
                if (idSurt > 0)
                {
                    Surtidore s = this.controlador.obtenerSurtidorByID(idSurt);
                    decimal precio = s.Precio.Value;
                    decimal cantidad = Convert.ToDecimal(this.txtCantCierre.Text) - Convert.ToDecimal(this.txtCantInicial.Text);
                    decimal totalVenta = precio * cantidad;

                    this.txtCantLitrosTotal.Text = decimal.Round(cantidad, 2).ToString("C");
                    this.txtCantPesosTotal.Text = decimal.Round(totalVenta, 2).ToString("C");
                }

            }
            catch
            {

            }
        }
        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.agregarCierreSurtidor();
            }
            catch
            {

            }
        }
        protected void ListSurtidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.ListSurtidor.SelectedValue);
                if (id > 0)
                {
                    Surtidores_Cierre ultimo = this.controlador.obtenerUltimoCierreSurtidor(id);
                    if (ultimo != null)
                    {
                        this.txtCantInicial.Text = Convert.ToDecimal(ultimo.CantidadCierre.Value).ToString();
                    }
                }
            }
            catch
            {

            }
        }
        protected void txtCantCierre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.calcularTotalCantidades();
            }
            catch
            {

            }
        }

    }
}