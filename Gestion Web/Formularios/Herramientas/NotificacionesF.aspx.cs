using Gestion_Api.Controladores.APP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class NotificacionesF : System.Web.UI.Page
    {
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

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta)
        {
            ControladorNotificaciones controladorNotificaciones = new ControladorNotificaciones();

            var notificaciones = controladorNotificaciones.ObtenerNotificacionesPorFecha(fechaDesde,fechaHasta);

            JsonSerializerSettings formatSettings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy HH:mm"
            };
            string resultadoJSON = JsonConvert.SerializeObject(notificaciones, formatSettings);
            return resultadoJSON;
        }
    }
}