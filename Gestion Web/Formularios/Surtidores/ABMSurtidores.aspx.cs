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

namespace Gestion_Web.Formularios.Surtidores
{
    public partial class ABMSurtidores : System.Web.UI.Page
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
                    if (this.accion > 1)
                    {
                        this.cargarSurtidor(this.id);
                    }
                    else
                    {
                        this.obtenerNuevoCodigo();
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
                        if (s == "99")
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
        private void cargarSurtidor(int id)
        {
            try
            {
                Surtidore s = this.controlador.obtenerSurtidorByID(id);
                this.txtCodigo.Text = s.Codigo;
                this.txtDescripcion.Text = s.Descripcion;
                this.txtNumero.Text = s.Numero;
                this.txtContador.Text = s.Contador.Value.ToString();
                this.txtPrecioLitro.Text = s.Precio.Value.ToString();
            }
            catch
            {

            }
        }
        private void obtenerNuevoCodigo()
        {
            try
            {
                List<Surtidore> list = this.controlador.obtenerSurtidores();
                this.txtCodigo.Text = (list.Count + 1).ToString().PadLeft(6, '0');
            }
            catch
            {

            }
        }
        private void agregarSurtidor()
        {
            try
            {
                Surtidore s = new Surtidore();
                s.Codigo = this.txtCodigo.Text;
                s.Descripcion = this.txtDescripcion.Text;
                s.Numero = this.txtNumero.Text;
                s.Contador = Convert.ToDecimal(this.txtContador.Text);
                s.Precio = Convert.ToDecimal(this.txtPrecioLitro.Text);
                s.Estado = 1;

                int ok = this.controlador.agregarSurtidor(s);
                if (ok > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego surtidor : " + s.Codigo);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Surtidor agregado con exito!. \", {type: \"info\"});location.href = 'SurtidoresF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando surtidor. \", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error. " + ex.Message + ". \", {type: \"error\"});", true);
            }
        }
        private void modificarSurtidor()
        {
            try
            {
                Surtidore s = this.controlador.obtenerSurtidorByID(this.id);
                s.Codigo = this.txtCodigo.Text;
                s.Descripcion = this.txtDescripcion.Text;
                s.Numero = this.txtNumero.Text;
                s.Contador = Convert.ToDecimal(this.txtContador.Text);
                s.Precio = Convert.ToDecimal(this.txtPrecioLitro.Text);

                int ok = this.controlador.modificarSurtidor(s);
                if (ok > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico surtidor : " + s.Codigo);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Surtidor modificado con exito!. \", {type: \"info\"});location.href = 'SurtidoresF.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificado surtidor. \", {type: \"error\"});", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ha ocurrido un error. " + ex.Message + ". \", {type: \"error\"});", true);
            }
        }
        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.accion > 1)
                {
                    this.modificarSurtidor();
                }
                else
                {
                    this.agregarSurtidor();
                }
            }
            catch
            {

            }
        }

    }
}