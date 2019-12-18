using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
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
    public partial class HistorialSMS : System.Web.UI.Page
    {
        class SMSResgistroTemporal
        {
            public string Id;
            public string Fecha;
            public string AliasCliente;
            public string Titulo;
            public string CuerpoDeMensaje;
            public string Celular;
            public string Estado;
        }
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
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();

                List<SMS_HistorialRegistros> listaResgistrosSMS = contSMS.SMS_HistorialRegistros_GetAllByDate(fechaDesde, fechaHasta.AddHours(23).AddMinutes(59));
                List<SMSResgistroTemporal> listaSmsTemporal = new List<SMSResgistroTemporal>();
                foreach (var item in listaResgistrosSMS)
                {
                    listaSmsTemporal.Add(new SMSResgistroTemporal
                    {
                        Id = item.Id.ToString(),
                        Fecha = item.Fecha.Value.ToString("dd/MM/yyyy HH:ss"),
                        AliasCliente = item.cliente.alias,
                        Celular = item.Celular,
                        CuerpoDeMensaje = item.CuerpoDeMensaje,
                        Titulo = item.Titulo,
                        Estado = item.Estado.ToString()
                    });
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaSmsTemporal);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}