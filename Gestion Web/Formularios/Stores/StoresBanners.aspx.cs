using Disipar.Models;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Stores
{
    public partial class StoresBanners : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        private int idUsuario;
        private string idStore;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idStore = Request.QueryString["idStore"];
                if (!IsPostBack)
                {
                    this.idUsuario = (int)Session["Login_IdUser"];                    
                }

                this.cargarBanners();
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
                    //if(this.contUser.validarAcceso(this.idUsuario,"Maestro.Articulos.Grupos") != 1)
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    //if (!String.IsNullOrEmpty(s))
                    //{
                    //    if (s == "15")
                    //    {
                           return 1;
                    //    }
                    //}
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        private void cargarBanners()
        {
            try
            {
                //cargo imagen
                String path = Server.MapPath("../../images/Store/" + idStore + "/");

                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);

                    var files = di.GetFiles();
                    foreach (var f in files)
                    {
                        if (f.Name.Contains("Banner1"))
                        {
                            this.Image1.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner2"))
                        {
                            this.Image2.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner3"))
                        {
                            this.Image3.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner4"))
                        {
                            this.Image4.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner5"))
                        {
                            this.Image5.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner6"))
                        {
                            this.Image6.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner7"))
                        {
                            this.Image7.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                        if (f.Name.Contains("Banner8"))
                        {
                            this.Image8.ImageUrl = "../../images/Store/" + idStore + "/" + f.Name;
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    //this.Image1.ImageUrl = "../images/Store/no_picture.jpg";
                }

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos del oferente. " + ex.Message));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Error cargando datos de pop-up. " + ex.Message + "');", true);
            }
        }
        protected void btnAct1_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner1", FileUpload1);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }

        protected void btnAct2_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner2", FileUpload2);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }

        protected void btnAct3_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner3", FileUpload3);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }
        protected void btnAct4_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner4", FileUpload4);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }
        protected void btnAct5_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner5", FileUpload5);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }
        protected void btnAct6_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner6", FileUpload6);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }
        protected void btnAct7_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner7", FileUpload7);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al subir banner " + ex.Message);
            }
        }
        protected void btnAct8_Click(object sender, EventArgs e)
        {
            try
            {
                SubirBanner("Banner8",FileUpload8);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al subir banner " + ex.Message);
            }
        }
        private string modificarNombre(string file, string banner)
        {
            try
            {
                FileInfo fi = new FileInfo(file);

                //borro el banner
                banner = Server.MapPath("../../images/Store/" + idStore + "/") + banner + "_" + DateTime.Now.ToString("ddMMyyhhmm") + fi.Extension;
                file = Server.MapPath("../../images/Store/" + idStore + "/") + file;

                FileInfo nuevoNombreDeBanner = new FileInfo(banner);

                File.Copy(file, banner, true);
                File.Delete(file);
                return nuevoNombreDeBanner.Name;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Error cambiando nombre de banner" + ex.Message + " ');", true);
                return null;

            }
        }

        public void BorrarImagenesPrevias(string bannerABorrar)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("../../images/Store/" + idStore + "/"));

                foreach (var item in di.GetFiles().ToList())
                {
                    if (item.Name.Contains(bannerABorrar))
                        item.Delete();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"ERROR","Error al intentar borrar las imagenes previas " + ex.Message);
            }
            
        }

        public void SubirBanner(string bannerName, FileUpload fileUpload)
        {
            try
            {
                if (IsPostBack)
                {
                    BorrarImagenesPrevias(bannerName);
                    Boolean fileOK = false;
                    String path = Server.MapPath("../../images/Store/" + idStore + "/");
                    if (fileUpload.HasFile)
                    {
                        String fileExtension =
                            System.IO.Path.GetExtension(fileUpload.FileName).ToLower();
                        String[] allowedExtensions = { ".jpg", ".png", ".jpeg" };
                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            if (fileExtension == allowedExtensions[i])
                            {
                                fileOK = true;
                            }
                        }
                    }

                    if (fileOK)
                    {
                        try
                        {
                            //creo el directorio si no exites y subo la foto
                            Log.EscribirSQL(1, "Info", "Voy a subir imagen");
                            if (!Directory.Exists(path))
                            {
                                Log.EscribirSQL(1, "Info", "No existe directorio. " + path + ". lo creo");
                                Directory.CreateDirectory(path);
                                Log.EscribirSQL(1, "Info", "directorio creado");
                            }
                            fileUpload.PostedFile.SaveAs(path
                                + fileUpload.FileName);
                            //modifico e nombre
                            this.modificarNombre(fileUpload.FileName, bannerName);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Banner Modificado con Exito');", true);
                            Response.Redirect("StoresBanners.aspx?idStore=" + idStore);
                        }
                        catch (Exception ex)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Error actualizando el banner. " + ex.Message + "');", true); ;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Formato de archivo no permitido. Solo se permiten imagenes con extension JPG ');", true); ;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al subir banner " + ex.Message);
            }            
        }

        protected void btnEliminar_1_Click(object sender, EventArgs e)
        {
            try
            {
                string idBoton = (sender as Button).ID;

                string[] atributos = idBoton.Split('_');
                string idBanner = atributos[1];

                EliminarBanners(idBanner);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al eliminar banner " + ex.Message);
            }
        }

        public void EliminarBanners(string idBanner)
        {
            try
            {
                String path = Server.MapPath("../../images/Store/" + idStore + "/");

                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);

                    var files = di.GetFiles();
                    foreach (var f in files)
                    {
                        if (f.Name.ToLower().Contains("banner" + idBanner))
                        {
                            f.Delete();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Banner Eliminado con exito!", "StoresBanners.aspx?idStore="+idStore));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al eliminar banner " + ex.Message);
            }
        }
    }
}