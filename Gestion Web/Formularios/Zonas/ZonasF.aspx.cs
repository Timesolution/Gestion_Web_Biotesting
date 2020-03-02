using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Zonas
{
    public partial class ZonasF : System.Web.UI.Page
    {
        controladorZona controlador = new controladorZona();
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();
            Cargar_Zonas();
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
                        if (s == "12")
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

        private void Cargar_Zonas()
        {
            try
            {
                List<Zona> zonas = controlador.obtenerZona();

                phZonas.Controls.Clear();
                foreach (Zona zon in zonas)
                {
                    Cargar_tabla(zon);
                }
            }
            catch (Exception e)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando zonas." + e.Message));
            }
        }

        void Cargar_tabla(Zona aCargar)
        {
            try
            {
                TableRow fila = new TableRow();

                TableCell celda_id = new TableCell();
                celda_id.Text = aCargar.id.ToString().PadLeft(3,'0');
                fila.Cells.Add(celda_id);

                TableCell celda_nombre = new TableCell();
                celda_nombre.Text = aCargar.nombre;
                fila.Cells.Add(celda_nombre);                

                TableCell accion = new TableCell();
                accion.Width = Unit.Percentage(15);
                LinkButton btn_editar = new LinkButton();

                btn_editar.CssClass = "btn btn-info ui-tooltip";
                btn_editar.Attributes.Add("data-toggle", "tooltip");
                btn_editar.Attributes.Add("title data-original-title", "Editar");
                btn_editar.Text = "<span class='shortcut-icon icon-search'></span>";
                btn_editar.PostBackUrl = "ABMZonas.aspx?a=2&e=" + aCargar.id;
                accion.Controls.Add(btn_editar);

                Literal lbl_space = new Literal();
                lbl_space.Text = "&nbsp";
                accion.Controls.Add(lbl_space);

                LinkButton btn_eliminar = new LinkButton();
                btn_eliminar.CssClass = "btn btn-info";
                btn_eliminar.Attributes.Add("data-toggle", "modal");
                btn_eliminar.Attributes.Add("href", "#modalConfirmacion");
                btn_eliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btn_eliminar.OnClientClick = "abrirConfirmacion(" + aCargar.id + ");";

                accion.Controls.Add(btn_eliminar);


                fila.Cells.Add(accion);

                phZonas.Controls.Add(fila);

            }
            catch (Exception e)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando tabla." + e.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int id_zona = Convert.ToInt32(this.txtMovimiento.Text);


                int i = this.controlador.quitarZona(id_zona);
                if (i == 1)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Zona: " + id_zona);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Zona eliminado con exito", null));
                    this.Cargar_Zonas();
                }

                if (i <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo borrar zona"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Zona. " + ex.Message));
            }
        }

    }
}