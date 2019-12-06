using Disipar.Models;
using Gestion_Api.Controladores.APP;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class Notificaciones : System.Web.UI.Page
    {
        Mensajes _m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();
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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    return 1;
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        bool ComprobarCampos()
        {
            if(string.IsNullOrEmpty(TextBoxNombreCampaña.Text) || string.IsNullOrEmpty(TextBoxMensaje.Text) || string.IsNullOrEmpty(TextBoxTituloNotificacion.Text))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxAtencion("Todos los campos deben estar completos!"));
                return false;
            }

            return true;
        }

        protected void lbtnEnviarNotificacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ComprobarCampos())
                    return;

                ControladorNotificaciones controladorNotificaciones = new ControladorNotificaciones();

                NotificacionesAPP notificacionesAPP = new NotificacionesAPP();

                notificacionesAPP.Campania = TextBoxNombreCampaña.Text;
                notificacionesAPP.Contenido = TextBoxMensaje.Text;
                notificacionesAPP.Titulo = TextBoxTituloNotificacion.Text;
                notificacionesAPP.Fecha = DateTime.Now;

                int i = controladorNotificaciones.GenerarNotificacion(notificacionesAPP);

                if(i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxInfo("Notificacion enviada con exito!","NotificacionesABM.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Notificacion enviada con exito!"));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al enviar notificacion. " + ex.Message);
            }
        }
    }
}