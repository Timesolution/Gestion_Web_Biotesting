using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class OrdenReparacionObservacionesABM : System.Web.UI.Page
    {
        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        controladorUsuario contUsuario = new controladorUsuario();
        Mensajes m = new Mensajes();
        int idOrdenReparacion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            idOrdenReparacion = Convert.ToInt32(Request.QueryString["or"]);

            if (!IsPostBack)
            {
                CargarObservaciones();
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
                        if (s == "57")
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

        public void CargarObservaciones()
        {
            try
            {
                var observaciones = contOrdenReparacion.ObtenerObservacionesPorOrdenReparacion(idOrdenReparacion);

                foreach (var item in observaciones)
                {
                    CargarEnPH(item);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar observaciones " + ex.Message);
            }
        }

        public void CargarEnPH(OrdenReparacion_Observaciones or_observaciones)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = or_observaciones.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = or_observaciones.Fecha.Value.ToString("dd/MM/yyyy H:mm");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);                

                TableCell celObservaciones = new TableCell();
                celObservaciones.Text = or_observaciones.Observaciones;
                celObservaciones.HorizontalAlign = HorizontalAlign.Left;
                celObservaciones.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celObservaciones);

                TableCell celUsuario = new TableCell();
                celUsuario.Text = contUsuario.obtenerUsuariosID((int)or_observaciones.Usuario).usuario;
                celUsuario.HorizontalAlign = HorizontalAlign.Left;
                celUsuario.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celUsuario);

                phEventos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar observaciones en PH " + ex.Message);
            }
        }

        protected void lbtnAgregarObservacion_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = contOrdenReparacion.AgregarObservacionOrdenReparacion(idOrdenReparacion, (int)Session["Login_IdUser"], txtDetalleObservacion.Text);

                if(temp >= 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Observacion agregada con exito!", "OrdenReparacionObservacionesABM.aspx?or="+ idOrdenReparacion));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al agregar observacion"));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al agregar observacion " + ex.Message);
            }
        }
    }
}