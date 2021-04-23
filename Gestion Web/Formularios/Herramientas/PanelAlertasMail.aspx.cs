using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class PanelAlertasMail : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion configuracion = new Configuracion();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    CargarAlertasMail();
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

        public void CargarAlertasMail()
        {
            try
            {
                txtObservacionesAgenda.Text = this.configuracion.ObservacionesMailAgenda;
                txtObservacionesRecordatorioMail.Text = this.configuracion.ObservacionesMailRecordatorio;
                txtDiasRecordatorioMail.Text = this.configuracion.DiasRecordatorioMail;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar las alertas de mails, Error: " + ex.Message);
            }
        }

        protected void lbtnObservacionesMailAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailAgenda = txtObservacionesAgenda.Text;

                int i = configuracion.ModificarObservacionesMailAgenda(configuracion.ObservacionesMailAgenda);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion de obervaciones predeterminado para los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico configuracion de obervaciones predeterminado para los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion de mails predeterminado \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: ObservacionesMailAgenda " + ex.Message);
            }
        }



        protected void lbtnObservacionesRecordatorioMail_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailRecordatorio = txtObservacionesRecordatorioMail.Text;

                int i = configuracion.ModificarObservacionesMailRecordatorio(configuracion.ObservacionesMailRecordatorio);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion de obervaciones predeterminado para los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico configuracion de obervaciones predeterminado para los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion de recordatorio de los mails  \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: ObservacionesMailRecordatorio " + ex.Message);
            }
        }

        protected void lbtnDiasRecordatorioMail_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.DiasRecordatorioMail = txtDiasRecordatorioMail.Text;

                int i = configuracion.ModificarDiasRecordatorioMail(configuracion.DiasRecordatorioMail);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion de dias para los recordatorio de los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico configuracion de dias para los recordatorio de los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la configuracion de dias \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: DiasRecordatorioMail " + ex.Message);
            }
        }
    }
}