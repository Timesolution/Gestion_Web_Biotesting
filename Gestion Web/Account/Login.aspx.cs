using System;
using System.Web;
using System.Web.UI;
using Gestion_Web.Models;
using System.Web.Security;
using Disipar.Models;
using Microsoft.Web.Administration;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System.Web.Services;

namespace Gestion_Web.Account
{
    public partial class Login : Page
    {
        Mensajes m = new Mensajes();
        controladorUsuario controlador = new controladorUsuario();
        public string cerrar;
        public string userGestion;
        public string passGestion;
        public int mascotasFc;
        public int cliente;
        public int idArticulo;
        public int idAgenda;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.cerrar = Request.QueryString["cerrar"];
                //si vengo del modulo de mascotas para facturar
                this.userGestion = Request.QueryString["us"];
                this.passGestion = Request.QueryString["pw"];
                this.mascotasFc = Convert.ToInt32(Request.QueryString["mascotas"]);
                this.cliente = Convert.ToInt32(Request.QueryString["cliente"]);
                this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                this.idAgenda = Convert.ToInt32(Request.QueryString["idag"]);

                if (this.cerrar == "si")
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se deslogueo del sistema");
                    Session.Clear();
                    Session.Abandon();

                    string mascotas = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Mascotas");
                    if (mascotas == "1")
                    {
                        Response.Redirect("../Mascotas/Account/Login.aspx?cerrar=si");
                    }

                    Response.Redirect("Login.aspx");
                }
                if (!String.IsNullOrEmpty(this.userGestion) && !String.IsNullOrEmpty(this.passGestion) && this.mascotasFc > 0)
                {
                    this.LogAndRedirect();
                }
                Page.Form.DefaultButton = this.btnIniciarSesion.UniqueID;
            }
            catch (Exception Ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error en el Page Load del Login. Excepción: " + Ex.Message);
            }
            
        }

        

        protected void LogIn(object sender, EventArgs e)
        {
            try 
            {
                if (IsValid)
                {
                    var manager = new UserManager();
                    Usuario usuario = new Usuario();
                    //ApplicationUser user = manager.Find(this.txtUsuario.Text, this.txtpassword.Text);
                    if (usuario.validar(this.txtUsuario.Text, this.txtpassword.Text))
                    {
                        usuario = controlador.ObtenerUsuario(this.txtUsuario.Text, this.txtpassword.Text);
                        Session.Add("Login_SucUser", usuario.sucursal.id);
                        Session.Add("Login_PtoUser", usuario.ptoVenta.id);
                        Session.Add("Login_EmpUser", usuario.empresa.id);
                        Session.Add("User", this.txtUsuario.Text);
                        Session.Add("Pass", this.txtpassword.Text);
                        Session.Add("Login_IdUser", usuario.id);
                        Session.Add("Login_NombrePerfil", usuario.perfil.nombre);
                        Session.Add("Login_IdPerfil", usuario.perfil.id);
                        
                        Session.Add("Login_Permisos", usuario.permisos);
                        Session.Add("Login_Vendedor", usuario.vendedor.id);
                        Session.Add("Login_UserNafta", usuario.usuario);//Gestion YPF Cordoba Menu Venta combustible
                        Log.EscribirSQL(usuario.id, "INFO", "Login");
                        //string appStr = Login.GetApplicationPoolNames();
                        //Log.EscribirSQL(usuario.id, "INFO", "datos app pool name: " + appStr);
                        //IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                        //authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        //var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);


                        //IdentityHelper.SignIn(manager, user, RememberMe.Checked);

                        //var miMaster = (SiteMaster)this.Master;
                        //((Label)miMaster.FindControl("LabelInicio")).Text = "Gestion Solution Software Online  " + usuario.empresa.RazonSocial;


                        //HttpContext.Current.User.Identity.IsAuthenticated;
                        FormsAuthentication.RedirectFromLoginPage(this.txtUsuario.Text, this.Field.Checked);
                        //Response.Redirect("/Default.aspx");

                    }
                    else
                    {
                        //Response.Redirect("Account/Login.aspx");
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", m.mensajeBoxAtencion("Usuario y/o contraseña incorrectos"));
                        txtUsuario.Text = "";
                    }
                    
                }
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrio un error en Login." + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se puede iniciar session a ocurrido un error"));
            }
        }
        private void LogAndRedirect()
        {
            try
            {
                Usuario usuario = new Usuario();
                if (usuario.validar(this.userGestion, this.passGestion))
                {
                    usuario = controlador.ObtenerUsuario(this.userGestion, this.passGestion);
                    Session.Add("Login_SucUser", usuario.sucursal.id);
                    Session.Add("Login_PtoUser", usuario.ptoVenta.id);
                    Session.Add("Login_EmpUser", usuario.empresa.id);
                    Session.Add("User", this.userGestion);
                    Session.Add("Pass", this.passGestion);
                    Session.Add("Login_IdUser", usuario.id);
                    Session.Add("Login_NombrePerfil", usuario.perfil.nombre);
                    Session.Add("Login_Permisos", usuario.permisos);
                    Session.Add("Login_Vendedor", usuario.vendedor.id);
                    Session.Add("Login_UserNafta", usuario.usuario);

                    Log.EscribirSQL(usuario.id, "INFO", "Login");
                    FormsAuthentication.RedirectFromLoginPage(this.userGestion, this.Field.Checked);
                    if (this.mascotasFc == 1)
                        Response.Redirect("../Formularios/Facturas/ABMFacturas.aspx?accion=11&cliente=" + this.cliente);
                    if (this.mascotasFc == 2)
                        Response.Redirect("../Default.aspx");
                    if(this.mascotasFc == 3)
                        Response.Redirect("../Formularios/Facturas/CuentaCorrienteF.aspx?a=2&Cliente=" + this.cliente + "&Sucursal=" + usuario.sucursal.id + "&Tipo=-1&fd=01/01/2000&fh=" + DateTime.Now.ToString("dd/MM/yyyy"));
                    if (this.mascotasFc == 4)
                        Response.Redirect("../Formularios/Facturas/CRM.aspx?fechadesde=01/01/2000&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaVencimientoDesde=01/01/2000&fechaVencimientoHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&cl=" + this.cliente + "&estado=0&fpf=1&fpfv=0&us=-1");
                    if (this.mascotasFc == 5)
                        Response.Redirect("../Formularios/Facturas/ABMPedidos.aspx?c=1" + "&cliente=" + this.cliente);
                    if (this.mascotasFc == 6)
                        Response.Redirect("../Formularios/Facturas/CotizacionesC.aspx?fechadesde=01/01/2000&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&Sucursal=" + usuario.sucursal.id + "&Cliente=" + this.cliente + "&estado=0&v=0");
                    if (this.mascotasFc == 7)
                        Response.Redirect("../Formularios/Clientes/ClientesABM.aspx?accion=2&id=" + this.cliente);
                    if (this.mascotasFc == 8)
                        Response.Redirect("../Formularios/Facturas/ABMFacturasLargo.aspx?pac=" + this.cliente + "&artsub=" + idArticulo + "&idag=" + idAgenda);

                }
            }
            catch
            {

            }
        }
        public static string GetApplicationPoolNames()
        {
            try
            {
                ServerManager manager = new ServerManager();

                string DefaultSiteName = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();

                var defaultSite = manager.Sites[DefaultSiteName];

                string appVirtaulPath = HttpRuntime.AppDomainAppVirtualPath;



                string appPoolName = string.Empty;

                foreach (Application app in defaultSite.Applications)
                {

                    string appPath = app.Path;

                    if (appPath == appVirtaulPath)
                    {

                        appPoolName = app.ApplicationPoolName;

                    }

                }

                return appPoolName;
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrio un error en GetApplicationPoolNames(). " + ex.Message);
                return null;
            }
        }

        [WebMethod]
        public static int ReporteClientes_Click(string email)
        {
            try
            {
                controladorUsuario controladorUsuario = new controladorUsuario();
                int i = controladorUsuario.recuperarPass(email);
                if (i > 0)
                    return 1;
                return 0;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "CATCH: Ocurrio un error al recuperar la contrasena mediante un mail. Ubicacion: Login.aspx.ReporteClientes_Click. Excepcion: " + ex.Message);
                return -1;
            }

        }

        //protected void btnReporteClientes_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int i = this.controlador.recuperarPass(this.txtMail.Text.Trim());
        //        if (i > 0)
        //        {
        //            this.txtMail.Text = "";
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se ha enviado la contraseña al mail " + this.txtMail.Text + ". Verifique la casilla en unos instantes.",null)); 
        //        }
        //        else
        //        {
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo enviar correo de recupero. Por favor verifique que el usuario sea el correcto."));
        //        }
        //    }
        //    catch(Exception ex)
        //    {
                
        //    }
        //}
    }
}