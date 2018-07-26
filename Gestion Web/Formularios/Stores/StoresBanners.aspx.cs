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
            if (IsPostBack)
            {
                BorrarImagenesPrevias("Banner1");
                Boolean fileOK = false; 
                String path = Server.MapPath("../../images/Store/" + idStore + "/");
                if (FileUpload1.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                    String[] allowedExtensions = { ".jpg",".png", ".jpeg" };
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
                        FileUpload1.PostedFile.SaveAs(path
                            + FileUpload1.FileName);
                        //modifico e nombre
                        this.modificarNombre(FileUpload1.FileName, "Banner1");

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Banner Modificado con Exito');", true);
                        Response.Redirect("StoresBanners.aspx?idStore="+idStore);
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

        protected void btnAct2_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BorrarImagenesPrevias("Banner2");
                Boolean fileOK = false;
                String path = Server.MapPath("../../images/Store/" + idStore + "/");
                if (FileUpload2.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload2.FileName).ToLower();
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
                        FileUpload2.PostedFile.SaveAs(path
                            + FileUpload2.FileName);
                        //modifico e nombre
                        this.modificarNombre(FileUpload2.FileName, "Banner2");
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

        protected void btnAct3_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BorrarImagenesPrevias("Banner3");
                Boolean fileOK = false;
                String path = Server.MapPath("../../images/Store/" + idStore + "/");
                if (FileUpload3.HasFile)
                {
                    String fileExtension =
                        System.IO.Path.GetExtension(FileUpload3.FileName).ToLower();
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
                        FileUpload3.PostedFile.SaveAs(path
                            + FileUpload3.FileName);
                        //modifico e nombre
                        this.modificarNombre(FileUpload3.FileName, "Banner3");
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
    }
}