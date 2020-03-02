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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ABMAsuntosSMS : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion configuracion = new Configuracion();
        ControladorSMS controlador = new ControladorSMS();

        private int accion;
        private int id;
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
                        this.cargarAsunto();
                    }   
                }
            }
            catch
            {
                
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
                    return 1;
                }
                return ok;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarAsunto()
        {
            try
            {
                SMS_Asuntos a = this.controlador.obtenerAsuntoSMSId(this.id);
                this.txtNombreAsunto.Text = a.Nombre;
                this.txtFechaAsunto.Text = a.FechaProgramada.Value.ToString("dd/MM/yyyy");
                this.txtHoraAsunto.Text = a.FechaProgramada.Value.TimeOfDay.ToString();
            }
            catch
            {

            }
        }
        private void agregarAsunto()
        {
            try
            {
                SMS_Asuntos asunto = new SMS_Asuntos();
                asunto.Nombre = this.txtNombreAsunto.Text;
                string fh = this.txtFechaAsunto.Text + " " + this.txtHoraAsunto.Text;
                asunto.FechaProgramada = Convert.ToDateTime(fh, new CultureInfo("es-AR"));
                asunto.Estado = 1;
                int ok = this.controlador.verificarNombreAsuntoSMS(asunto.Nombre);
                if (ok < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Nombre ya en uso!.\");", true);
                    return;
                }

                int i = this.controlador.agregarAsuntoSMS(asunto);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});location.href='AsuntosSMS.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                }
            }
            catch
            {

            }
        }
        private void modificarAsunto()
        {
            try
            {
                SMS_Asuntos asunto = this.controlador.obtenerAsuntoSMSId(this.id);
                asunto.Nombre = this.txtNombreAsunto.Text;
                string fh = this.txtFechaAsunto.Text + " " + this.txtHoraAsunto.Text;
                asunto.FechaProgramada = Convert.ToDateTime(fh, new CultureInfo("es-AR"));
                asunto.Estado = 1;
                int ok = this.controlador.verificarNombreAsuntoSMS(asunto.Nombre);
                if (ok < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Nombre ya en uso!.\");", true);
                    return;
                }

                int i = this.controlador.modificarAsuntoSMS(asunto);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});location.href='AsuntosSMS.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                }
            }
            catch
            {

            }
        }
            

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (accion > 0)
                {
                    this.modificarAsunto();
                }
                else
                {
                    this.agregarAsunto();
                }
            }
            catch
            {

            }
        }
    }
}