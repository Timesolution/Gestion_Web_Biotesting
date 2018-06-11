using Gestion_Api.Modelo;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(Gestion_Web.Startup))]
namespace Gestion_Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            try
            {
                ConfigureAuth(app);
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error iniciando aplicacion. " + ex.Message);
            }
        }
    }
}
