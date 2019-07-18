using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMGruposArticulos : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorArticulo controlador = new controladorArticulo();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idGrupo;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idGrupo = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarGrupos();
                if (!IsPostBack)
                {
                    
                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (valor == 2)
                    {
                        grupo sg = this.controlador.obtenerGrupoID(this.idGrupo);
                        txtGrupo.Text = sg.descripcion;
                        //carga la imagen
                        cargarImagenGrupo(idGrupo.ToString());
                    }
                }
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
                    if(this.verificarAcceso() != 1)
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
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "15")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarGrupos()
        {
            try
            {
                phGruposArticulos.Controls.Clear();
                List<grupo> grupos = this.controlador.obtenerGruposArticulosList();
                foreach (grupo sg in grupos)
                {
                    this.cargarGruposPH(sg);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Grupos. " + ex.Message));

            }
        }

        private void cargarGruposPH(grupo sg)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = sg.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = sg.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarGrupos);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + sg.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + sg.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                LinkButton btnImagen = new LinkButton();
                btnImagen.ID = "btnImagen_" + sg.id;
                btnImagen.CssClass = "btn btn-info ui-tooltip";
                btnImagen.Attributes.Add("data-toggle", "tooltip");
                btnImagen.Text = "<span class='shortcut-icon icon-picture'></span>";
                btnImagen.Click += new EventHandler(this.abrirModalImagen);
                celAction.Controls.Add(btnImagen);
                celAction.Width = Unit.Percentage(20);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                //LinkButton btnImagen = new LinkButton();
                //btnImagen.ID = "btnImagen_" + sg.id;
                //btnImagen.CssClass = "btn btn-info";
                //btnImagen.Attributes.Add("data-toggle", "modal");
                //btnImagen.Text = "<span class='shortcut-icon icon-picture'></span>";
                //btnImagen.Click += new EventHandler(this.abrirModalImagen);
                //celAction.Controls.Add(btnImagen);
                //celAction.Width = Unit.Percentage(20);
                //celAction.VerticalAlign = VerticalAlign.Middle;
                //celAction.HorizontalAlign = HorizontalAlign.Center;
                //tr.Cells.Add(celAction);

                phGruposArticulos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    grupo sg = new grupo();
                    sg.id = this.idGrupo;
                    sg.descripcion = txtGrupo.Text;
                    sg.estado = 1;
                    int i = this.controlador.modificarGrupo(sg);
                    this.cargarGrupos();
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL(idUsuario, "INFO", "Modifico el Grupo de Articulo: " + this.idGrupo);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  Grupo de Articulo: " + sg.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo modificada con exito", "ABMGruposArticulos.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Grupo"));
                    }
                }
                else
                {
                    int i = this.controlador.agregarGrupo(this.txtGrupo.Text);
                    if (i > 0)
                    {
                        //agrego bien
                        //Log.EscribirSQL(idUsuario, "INFO", "Agrego el Grupo de Articulo: " + i);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Grupo de Articulo: " + this.txtGrupo.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo cargada con exito", null));
                        this.borrarCampos();
                        this.cargarGrupos();
                        Response.Redirect("ABMGruposArticulos.aspx");

                    }
                    else
                    {
                        if (i == -2)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya existe un grupo con el nombre " + this.txtGrupo.Text));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo"));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Grupo. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtGrupo.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarGrupos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMGruposArticulos.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Grupo de Articulo. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idGrupo = Convert.ToInt32(this.txtMovimiento.Text);
                grupo g = this.controlador.obtenerGrupoID(idGrupo);
                g.estado = 0;
                int i = this.controlador.eliminarGrupo(g);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Grupo de Articulo: " + g.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Grupo eliminado con exito", null));
                    this.cargarGrupos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Grupo"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Grupo. " + ex.Message));
            }
        }

        protected void lbtnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                this.subirImagen1();
            }
            catch(Exception ex)
            {

            }
        }

        public void subirImagen1()
        {
            if (IsPostBack)
            {
                Boolean fileOK = false;
                String tipoExt = "";
                String path = Server.MapPath("../../images/Grupos/" + Convert.ToInt32(this.lblIdGrupo.Text) + "/");

                if (FileUpload1.HasFile)
                {
                    String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".jpg", ".png", "jpeg" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                            tipoExt = fileExtension;
                        }
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        //creo el directorio si no existe y subo la foto
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        DirectoryInfo di = new DirectoryInfo(path);
                        var files = di.GetFiles();
                        foreach (var f in files)
                        {
                            f.Delete();
                        }
                        //guardo nombre archivo
                        string imagen = FileUpload1.FileName;

                        //lo subo
                        FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);
                        string fechaHoy = DateTime.Now.ToString("ddMMyy_hhmmss");
                        this.modificarNombre(FileUpload1.FileName, this.lblIdGrupo.Text + fechaHoy + tipoExt);

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Imagen agregada con exito ", null));
                        Response.Redirect("ABMGruposArticulos.aspx?valor=2&id=" + this.lblIdGrupo.Text);
                    }

                    catch (Exception ex)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error actualizando imagen " + ex.Message));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("El archivo debe ser JPG o PNG "));
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Debe ingresar el Codigo de Articulo para poder Subir Imagenes"));
            }
        }

        private string modificarNombre(string pathFile, string id)
        {
            try
            {
                string imagenCodigo = Server.MapPath("../../images/Grupos/" + id + "//") + id + ".jpg";
                File.Copy(pathFile, imagenCodigo, true);
                File.Delete(pathFile);
                return imagenCodigo;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Error cambiando nombre de Imagen. " + ex.Message + " ');", true);
                return String.Empty;
            }
        }

        private void cargarImagenGrupo(string idGrupo)
        {
            try
            {
                string[] imagenes = Directory.GetFiles(Server.MapPath("../../images/Grupos/" + idGrupo + "/"));
                TableRow tr = new TableRow();
                //limpio el placeholder
                this.phImagenGrupo.Controls.Clear();
                FileInfo fi = new FileInfo(imagenes[0]);
                if (fi == null) {
                    phImagenGrupo.Visible = false;
                    return;
                }
                TableCell celImagen = new TableCell();
                Label gallery = new Label();
                gallery.Text += @"<li>";
                gallery.Text += @"<a href=../../images/Grupos/" + idGrupo + "/" + fi.Name + " class=\"ui-lightbox\" >";
                gallery.Text += "<img height=\"100\" width = \"100\" src=\"/images/Grupos/" + idGrupo + "/" + fi.Name + "\" alt=\"\" />";
                gallery.Text += @"</a>";
                gallery.Text += @"<a href=../../images/Grupos/" + idGrupo + "/" + fi.Name + " class=\"preview\"></a>";
                gallery.Text += @" </li>";
                gallery.Text += "<br/>";
                celImagen.Controls.Add(gallery);
                tr.Cells.Add(celImagen);
                phImagenGrupo.Controls.Add(tr);
            }
            catch
            {

            }
        }

        private void abrirModalImagen(object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID;
                var grupo = id.Split('_');
                string idGrupo = grupo[1];

                this.lblIdGrupo.Text = idGrupo;
                ScriptManager.RegisterStartupScript(updatePanelAgregarImagen, updatePanelAgregarImagen.GetType(), "openModalImagen", "openModalImagen();", true);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}