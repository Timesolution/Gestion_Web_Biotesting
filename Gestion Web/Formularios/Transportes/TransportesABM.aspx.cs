using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Transportes
{
    public partial class TransportesABM : System.Web.UI.Page
    {
        //mensajes popUp
        Mensajes m = new Mensajes();
        //controladores
        private ControladorExpreso controlador = new ControladorExpreso();
        
        //para saber si es alta(1) o modificacion(2)
        private int accion;
        //private string cuit;
        private long idExpreso;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.idExpreso = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (this.accion == 2)
                    {
                        //cargar expreso
                        Cargar_expreso();
                        
                    }
                }

            }
            catch (Exception ex)
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

        private void Cargar_expreso(){

            try
            {
                expreso exp = controlador.obtenerExpresoPorID(this.idExpreso);
                this.txtNombreExpreso.Text = exp.nombre;
                this.txtDireccionExpreso.Text = exp.direccion;
                this.txtTelefonoExpreso.Text = exp.telefono;
                this.txtCuitExpreso.Text = exp.cuit.ToString();
                this.txtObservacionesExpreso.Text = exp.otros;
            }
            catch(Exception e)
            {

            }
        }


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
            {
                this.nuevoExpreso();
            }
            else if(this.accion == 2)
            {
                this.modificarExpreso();
            }
        }

        private void nuevoExpreso()
        {
            try
            {
                expreso e = this.obtenerDatosExpreso();
                int i = this.controlador.agregarExpreso(e);
                if (i > 0)
                {
                    this.limpiarCampos();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"transporte Agregado con Exito\", {type: \"info\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Compra Agregada con Exito", "ComprasABM.aspx?a=1"));
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se Pudo agregar Transporte\";", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se Pudo agregar compra"));
                }
            }
            catch(Exception ex)
            {
 
            }
        }

        private void modificarExpreso()
        {
            try
            {
                expreso e = this.obtenerDatosExpreso();
                e.id = idExpreso;
                int i = this.controlador.modificarExpreso(e);

                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"transporte Agregado con Exito\", {type: \"info\"});", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Compra Agregada con Exito", "ComprasABM.aspx?a=1"));
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"No se Pudo agregar Transporte\";", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se Pudo agregar compra"));
                }
            }

            catch(Exception ex)
            {

            }
        }
        private Gestion_Api.Entitys.expreso obtenerDatosExpreso()
        {
            try
            {
                Gestion_Api.Entitys.expreso e = new Gestion_Api.Entitys.expreso();
                e.nombre = this.txtNombreExpreso.Text;
                e.direccion = this.txtDireccionExpreso.Text;
                e.telefono = this.txtTelefonoExpreso.Text;
                e.cuit = 0;
                if (this.txtCuitExpreso.Text != "")
                {
                    e.cuit = Convert.ToDecimal(this.txtCuitExpreso.Text);
                }                
                e.otros = this.txtObservacionesExpreso.Text;
                
                return e;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error obteniendo datos del expreso. " + ex.Message + "\", {type: \"error\"});", true);

                return null;
            }
        }

        private void limpiarCampos()
        {
            this.txtNombreExpreso.Text = "";
            this.txtDireccionExpreso.Text = "";
            this.txtTelefonoExpreso.Text = "";
            this.txtCuitExpreso.Text = "";
            this.txtObservacionesExpreso.Text = "";
        }

    }
}