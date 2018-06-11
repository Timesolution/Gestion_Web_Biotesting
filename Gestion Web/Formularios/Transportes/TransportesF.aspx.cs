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

namespace Gestion_Web.Formularios.Transportes
{
    public partial class TransportesF : System.Web.UI.Page
    {
        ControladorExpreso controlador = new ControladorExpreso();
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();
            Cargar_Expresos();            
            
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
                        if (s == "63")
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

        void Cargar_Expresos()
        {
            try
            {
                List<expreso> expresos = controlador.obtenerExpresos();

                phTransporte.Controls.Clear();
                foreach (expreso exp in expresos)
                {
                    Cargar_tabla(exp);
                }
            }
            catch(Exception e)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando expresos." + e.Message));
            }
            
        }

        void Cargar_tabla(expreso aCargar)
        {
            try
            {
                TableRow fila = new TableRow();

                TableCell celda_nombre = new TableCell();
                celda_nombre.Text = aCargar.nombre;
                fila.Cells.Add(celda_nombre);

                TableCell celda_direccion = new TableCell();
                celda_direccion.Text = aCargar.direccion;
                fila.Cells.Add(celda_direccion);

                TableCell celda_telefono = new TableCell();
                celda_telefono.Text = aCargar.telefono;
                fila.Cells.Add(celda_telefono);

                TableCell celda_cuit = new TableCell();
                celda_cuit.Text = aCargar.cuit.ToString();                
                fila.Cells.Add(celda_cuit);

                TableCell accion = new TableCell();
                LinkButton btn_editar = new LinkButton();
                
                btn_editar.CssClass = "btn btn-info ui-tooltip";
                btn_editar.Attributes.Add("data-toggle", "tooltip");
                btn_editar.Attributes.Add("title data-original-title", "Editar");
                btn_editar.Text = "<span class='shortcut-icon icon-search'></span>";
                btn_editar.PostBackUrl = "TransportesABM.aspx?a=2&e="+aCargar.id;
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

                phTransporte.Controls.Add(fila);

            }
            catch(Exception e)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando tabla."+e.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idTransporte = Convert.ToInt32(this.txtMovimiento.Text);


                int i = this.controlador.quitarExpreso(idTransporte);
                if (i == 1)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Cliente: " + idTransporte);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Transporte eliminado con exito", null));
                    this.Cargar_Expresos();
                }
                
                if (i <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo borrar Transporte"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Transporte. " + ex.Message));
            }
        }
    }
}