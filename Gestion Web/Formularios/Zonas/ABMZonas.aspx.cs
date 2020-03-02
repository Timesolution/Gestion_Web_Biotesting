using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Zonas
{
    public partial class ABMZonas : System.Web.UI.Page
    {
        //mensajes popUp
        Mensajes m = new Mensajes();
        //controlador
        controladorZona controlador = new controladorZona();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private long idZona;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.idZona = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (this.accion == 1)
                    {
                        //obtengo ultimo nro de zona
                        cargarNroZona();

                    }
                    if (this.accion == 2)
                    {
                        //cargar zona
                        Cargar_zona();

                    }
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error inicializando formulario. " + ex.Message));
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
                        if (s == "39")
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

        private void Cargar_zona()
        {

            try
            {
                Zona zon = controlador.obtenerZonaPorID(this.idZona);

                this.txtNroZona.Text = zon.id.ToString();
                this.txtNombreZona.Text = zon.nombre;

            }
            catch (Exception e)
            {

            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
            {
                this.nuevoZona();
            }
            else if (this.accion == 2)
            {
                this.modificarZona();
            }

            Response.Redirect("../Zonas/ZonasF.aspx");

        }

        private void nuevoZona()
        {
            try
            {
                Zona z = obtenerDatosZona();
                int i = this.controlador.agregarZona(z);
                if (i > 0)
                {
                    this.limpiarCampos();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Zona agregada con exito\", {type: \"info\"});", true);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo agregar Zona\";", true);                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void modificarZona()
        {
            try
            {
                Zona z = this.obtenerDatosZona();
                z.id = idZona;
                int i = this.controlador.modificarZona(z);

                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Zona agregada con exito\", {type: \"info\"});", true);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se pudo agregar Zona\";", true);                    
                }
            }

            catch (Exception ex)
            {

            }
        }

        private Gestion_Api.Entitys.Zona obtenerDatosZona()
        {
            try
            {
                Gestion_Api.Entitys.Zona z = new Gestion_Api.Entitys.Zona();
                z.id = Convert.ToInt32(this.txtNroZona.Text);
                z.nombre = this.txtNombreZona.Text;                
                return z;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error obteniendo datos dela zona. " + ex.Message + "\", {type: \"error\"});", true);

                return null;
            }
        }

        private void limpiarCampos()
        {
            this.txtNombreZona.Text = "";
            this.txtNroZona.Text = "";
        }

        private void cargarNroZona()
        {
            try
            {
                List<Zona> lsZonas = controlador.obtenerZona();
                this.txtNroZona.Text = (lsZonas.Count + 1).ToString().PadLeft(3, '0');
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error obteniendo nro zona. " + ex.Message + "\", {type: \"error\"});", true);
            }
        }

    }
}