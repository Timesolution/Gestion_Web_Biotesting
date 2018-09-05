using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web
{
    public partial class SiteMaster : MasterPage
    {
        controladorSucursal contSucu = new controladorSucursal();
        controladorUsuario contUsers = new controladorUsuario();
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                // El código siguiente ayuda a proteger frente a ataques XSRF
                var requestCookie = Request.Cookies[AntiXsrfTokenKey];
                Guid requestCookieGuidValue;
                if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
                {
                    // Utilizar el token Anti-XSRF de la cookie
                    _antiXsrfTokenValue = requestCookie.Value;
                    Page.ViewStateUserKey = _antiXsrfTokenValue;
                }
                else
                {
                    // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                    _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                    Page.ViewStateUserKey = _antiXsrfTokenValue;

                    var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                    {
                        HttpOnly = true,
                        Value = _antiXsrfTokenValue
                    };
                    if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                    {
                        responseCookie.Secure = true;
                    }
                    Response.Cookies.Set(responseCookie);
                }

                Page.PreLoad += master_Page_PreLoad;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error iniciando aplicacion desde site.master. " + ex.Message);
            }
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
             
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    int idSuc = (int)Session["Login_SucUser"];
                    Sucursal s = this.contSucu.obtenerSucursalID(idSuc);
                    this.Label1.Text = Context.User.Identity.Name;
                    this.Label2.Text = s.nombre;
                    this.Label3.Text = Session["Login_NombrePerfil"] as string;
                    this.cargarIniciales();
                }
                System.Uri asd = Request.Url;// poner .contains = mi formulario
                if (asd.AbsolutePath.Contains("ABMFacturasImagenes.aspx"))
                {
                    phMenuCompleto.Visible = false;
                }
            }
            catch { }
        }

        private void cargarIniciales()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil != "Cliente"  && perfil != "Importacion")
                {
                    this.phMenu.Visible = true;
                }

                if (perfil == "SuperAdministrador")
                {
                    this.phRentabilidad.Visible = true;
                }
                //combustible
                string combustible = WebConfigurationManager.AppSettings.Get("Combustible");
                //string nombreUser = Session["Login_UserNafta"] as string;

                if (combustible=="1" && !String.IsNullOrEmpty(combustible))
                {
                    this.phCombustible.Visible = true;
                }

                string facturarImagenes = WebConfigurationManager.AppSettings.Get("FacturarImagenes");
                if (facturarImagenes == "1" && !String.IsNullOrEmpty(facturarImagenes))
                {
                    this.phImagenes.Visible = true;
                }

                //tapice
                string tapice = WebConfigurationManager.AppSettings.Get("Tapice");
               
                if (tapice == "1" && !String.IsNullOrEmpty(tapice))
                {
                    
                    this.phTapice.Visible = true;
                }

                //Orden Reparacion
                string ordenReparacion = WebConfigurationManager.AppSettings.Get("OrdenReparacion");

                if (ordenReparacion == "1" && !String.IsNullOrEmpty(ordenReparacion))
                {
                    this.phOrdenReparacion.Visible = true;
                    this.phServicioTecnico.Visible = true;
                }

                string millas = WebConfigurationManager.AppSettings.Get("Millas");
                if (!String.IsNullOrEmpty(millas))
                {
                    string permisos = Session["Login_Permisos"] as string;
                    string[] listPermisos = permisos.Split(';');
                    foreach (string s in listPermisos)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            if (s == "105")
                            {
                                string url = WebConfigurationManager.AppSettings.Get("UrlMillas");
                                int user = (int)Session["Login_IdUser"];
                                Usuario u = this.contUsers.obtenerUsuariosID(user);
                                url += "Login.aspx?u=" + u.usuario + "&p=" + u.contraseña;
                                this.litMillas.Text = "<a onclick=\"windowOpen('" + url + "')\" >Sistema Millas</a>";
                            }
                        }
                    }
                    
                }
            }
            catch
            { }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                FormsAuthentication.SignOut();
                Response.Redirect("/Account/Login.aspx?cerrar=si");
            }
            catch
            {
                
            }
        }

        //protected void btnMillas_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int user = (int)Session["Login_IdUser"];
        //        Usuario u = this.contUsers.obtenerUsuariosID(user);
        //        string url = WebConfigurationManager.AppSettings.Get("UrlMillas");

        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('" + url + "Login.aspx?socio=" + socio.Id + "','_blank');");
        //        Response.Redirect(url + "Login.aspx?u=" + u.usuario + "&p=" + u.contraseña);
        //    }
        //    catch
        //    {

        //    }
        //}
    }

}