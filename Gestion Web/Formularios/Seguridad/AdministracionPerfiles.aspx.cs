using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Seguridad
{
    public partial class AdministracionPerfiles : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        controladorUsuario controlador = new controladorUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                if(!IsPostBack)
                {
                    this.cargarPerfiles();
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                }
                if (this.DropListPermiso.SelectedValue != "-1")
                {
                    this.cargarTablaMenus(Convert.ToInt32(this.DropListPermiso.SelectedValue));
                    
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Herramientas.Presupuesto") != 1)
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
                        if (s == "60")
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

        public void cargarPerfiles()
        {
            try
            {
                DataTable dt = controlador.obtenerPerfiles();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Perfil"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListPermiso.DataSource = dt;
                this.DropListPermiso.DataValueField = "Id";
                this.DropListPermiso.DataTextField = "Perfil";

                this.DropListPermiso.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Perfiles. " + ex.Message));
            }
        }

        protected void DropListPermiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DropListPermiso.SelectedValue != "-1")
                {
                    this.cargarTablaMenus(Convert.ToInt32(this.DropListPermiso.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Numeracion. " + ex.Message));
            }
        }

        private void cargarTablaMenus1(int idPerfil)
        {
            try
            {
                //Obtengo la Key de Mascotas para verificar si tengo que mostrar o no los permisos del sistema de Mascotas
                string mascotas = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Mascotas");

                List<PerfilMenu> perfiles = controlador.obtenerMenusPorPerfil(idPerfil);
                perfiles = perfiles.OrderBy(x => x.menu.nombre).ToList();

                //Creo una lista auxiliar por si la key de mascotas esté en 1, entonces tengo la lista original guardada
                List<PerfilMenu> perfilesAux = perfiles;

                //Filtro la lista de menus, y dejo los menus que no contengan la palabra mascota
                perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("mascotas")).ToList();

                //Verifico, si tiene la key de mascotas, consulto si está en 1 
                if (!string.IsNullOrEmpty(mascotas))
                {
                    //Si contiene la key de mascotas en 1,  seteo la lista auxiliar de menus a la lista de menus
                    if (mascotas == "1")
                    {
                        perfiles = perfilesAux;
                    }
                }

                phMenus.Controls.Clear();
                foreach (PerfilMenu p in perfiles)
                {
                    this.cargarPerfilesEnPh(p);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Tabla de Menus por Permiso. " + ex.Message));

            }
        }

        private void cargarTablaMenus(int idPerfil)
        {
            try
            {
                //Obtengo la Key de Mascotas para verificar si tengo que mostrar o no los permisos del sistema de Mascotas
                string mascotas = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Mascotas");

                string estetica = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Estetica");

                List<PerfilMenu> perfiles = controlador.obtenerMenusPorPerfil(idPerfil);
                perfiles = perfiles.OrderBy(x => x.menu.nombre).ToList();

                //Creo una lista auxiliar por si la key de mascotas esté en 1, entonces tengo la lista original guardada
                //List<PerfilMenu> perfilesAux = perfiles;

                //Filtro la lista de menus, y dejo los menus que no contengan la palabra mascota
                //perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("mascotas")).ToList();
                //perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("estetica")).ToList();

                //Verifico, si tiene la key de mascotas, consulto si está en 1 
                if (!string.IsNullOrEmpty(mascotas))
                {
                    //Si contiene la key de mascotas en 1,  seteo la lista auxiliar de menus a la lista de menus
                    if (mascotas == "1")
                    {
                        perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("estetica")).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(estetica))
                {
                    //Si contiene la key de estetica en 1,  seteo la lista auxiliar de menus a la lista de menus
                    if (estetica == "1")
                    {
                        perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("mascotas")).ToList();
                    }
                }

                if (string.IsNullOrEmpty(mascotas) && string.IsNullOrEmpty(estetica))
                {
                    if( estetica != "1" && mascotas != "1")
                    {
                        perfiles = perfiles.Where(x => !x.menu.nombre.ToLower().Contains("mascotas") && !x.menu.nombre.ToLower().Contains("estetica")).ToList();
                    }
                }

                phMenus.Controls.Clear();
                foreach (PerfilMenu p in perfiles)
                {
                    this.cargarPerfilesEnPh(p);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Tabla de Menus por Permiso. " + ex.Message));

            }
        }

        private void cargarPerfilesEnPh(PerfilMenu p)
        {
            try
            {

                //fila
                TableRow tr = new TableRow();
                tr.ID = p.id.ToString();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = p.menu.nombre;
                celCodigo.Width = Unit.Percentage(95);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + p.perfil.id + "_" + p.menu.id;
                //cbSeleccion.CssClass = "btn btn-info";
                if(p.estado == 1)
                {
                    cbSeleccion.Checked = true;
                }
                celCantidad.Controls.Add(cbSeleccion);
                celCantidad.Width = Unit.Percentage(5);
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                celCantidad.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celCantidad);

                phMenus.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Menu en Lista. " + ex.Message));
            }

        }

        protected void lbtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                List<PerfilMenu> perfiles = new List<PerfilMenu>();
                foreach (Control C in phMenus.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[1].Controls[0] as CheckBox;
                    PerfilMenu p = new PerfilMenu();
                    string[] d = ch.ID.Split('_');
                    p.id = Convert.ToInt32(tr.ID);
                    p.menu.id = Convert.ToInt32(d[2]);
                    p.perfil.id = Convert.ToInt32(d[1]);
                    if (ch.Checked == true)
                    {
                        p.estado = 1;
                    }
                    else
                    {
                        p.estado = 0;
                    }

                    perfiles.Add(p);
                }


                int i = this.controlador.modificarPerfilesMenu(perfiles);
                //int i = 1;
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Modifico Configuracion de Permisos de Perfil: " + this.DropListPermiso.SelectedItem.Text);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Configuracion 2 actualizada con exito", "AdministracionPerfiles.aspx"));

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Configuración actualizada con exito.", ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Error actualizando Permisos"));

                }

            }
            catch
            {

            }
        }
    }
}