using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Estetica_Api;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class PanelAlertasMail : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion configuracion = new Configuracion();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    VerificarEnviosSMS();
                    CargarAlertasMail();
                    CargarFormularios();
                    CargarPrefijosCelular();
                }

                if (ddlUbicacion.SelectedValue == "1")
                {
                    divFirma.Attributes.Remove("display");
                }

            }
            catch
            {

            }
        }


        public void CargarFormularios()
        {
            try
            {
                Estetica_Api.Controladores.ControladorHistorial contHist = new Estetica_Api.Controladores.ControladorHistorial();

                List<Estetica_Api.Entity.Formularios> forms = contHist.obtenerFormularios();

                ListItem items = new ListItem("Seleccionar...", "-1");
                ddlFormularioNotificacion.Items.Add(items);
                if (forms != null)
                {
                    foreach (Estetica_Api.Entity.Formularios f in forms)
                    {
                        string text = f.IdFormulario + " - " + f.NombreFormulario;
                        ListItem item = new ListItem(text, f.IdFormulario.ToString());
                        ddlFormularioNotificacion.Items.Add(item);
                    }
                }
                if (!String.IsNullOrEmpty(this.configuracion.FormularioDespuesAtendido)) 
                {
                    ddlFormularioNotificacion.SelectedValue = this.configuracion.FormularioDespuesAtendido;
                }


            }
            catch (Exception ex)
            {


            }

        }

        public void CargarPrefijosCelular()
        {
            Estetica_Api.Controladores.ControladorFunciones contFun = new Estetica_Api.Controladores.ControladorFunciones();
            var prefijos = contFun.ObtenerPrefijosTelefonicosPaises();

            ddlPaisesCelular.DataSource = prefijos;
            ddlPaisesCelular.DataValueField = "id";
            ddlPaisesCelular.DataTextField = "Pais";

            this.ddlPaisesCelular.DataBind();

            this.ddlPaisesCelular.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            if (!String.IsNullOrEmpty(this.configuracion.PaisCelularPrederminado))
            {
                ddlPaisesCelular.SelectedValue = this.configuracion.PaisCelularPrederminado;
            }
        }
        
        private void VerificarEnviosSMS()
        {
            if (this.configuracion.EnviarSMSRecordatorio == "1")
            {
                lbEnvioSMS.Attributes["class"] = "btn btn-success";
                lbEnvioSMS.Text = "<span class='shortcut-icon icon-ok'></span>";
                txtNombreFantasia.Text = configuracion.NombreFantasiaSMS; 
                DivNombreFantasia.Visible = true;
            }
            else if (this.configuracion.EnviarSMSRecordatorio == "0") 
            {
                lbEnvioSMS.Attributes["class"] = "btn btn-danger";
                lbEnvioSMS.Text = "<span class='shortcut-icon icon-remove'></span>";
                DivNombreFantasia.Visible = false;
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
                //verifico si es super admin
                string perfil = Session["Login_NombrePerfil"] as string;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                int ok = 0;
                if (perfil == "SuperAdministrador")
                {
                    return 1;
                }
                return ok;
            }
            catch
            {
                return -1;
            }
        }

        public void CargarAlertasMail()
        {
            try
            {
                txtObservacionesAgenda.Text = this.configuracion.ObservacionesMailAgenda;
                txtObservacionesRecordatorioMail.Text = this.configuracion.ObservacionesMailRecordatorio;
                txtDiasRecordatorioMail.Text = this.configuracion.DiasRecordatorioMail;
                txtEnvioDocumento.Text = this.configuracion.ObservacionesMailDocumento;
                txtEnvioFormulario.Text = this.configuracion.ObservacionesMailFormulario;
                txtDiasDespuesDeTurno.Text = this.configuracion.DiasDespuesAtendidoMail;
                txtEnvioNotificacion.Text = this.configuracion.ObservacionDespuesAtendido;
                ddlFormularioNotificacion.SelectedValue = this.configuracion.FormularioDespuesAtendido;
                ddlUbicacion.SelectedValue = this.configuracion.UbicacionLogoMail;
                txtFirma.Text = this.configuracion.ObservacionFirmaMail;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar las alertas de mails, Error: " + ex.Message);
            }
        }

        protected void lbtnObservacionesMailAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailAgenda = txtObservacionesAgenda.Text;

                int i = configuracion.ModificarObservacionesMailAgenda(configuracion.ObservacionesMailAgenda);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de obervaciones predeterminado para los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion de mails predeterminado \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: ObservacionesMailAgenda " + ex.Message);
            }
        }

        protected void lbtnObservacionesRecordatorioMail_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailRecordatorio = txtObservacionesRecordatorioMail.Text;

                int i = configuracion.ModificarObservacionesMailRecordatorio(configuracion.ObservacionesMailRecordatorio);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de obervaciones predeterminado para los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion de recordatorio de los mails  \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: ObservacionesMailRecordatorio " + ex.Message);
            }
        }

        protected void lbtnDiasRecordatorioMail_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.DiasRecordatorioMail = txtDiasRecordatorioMail.Text;

                int i = configuracion.ModificarDiasRecordatorioMail(configuracion.DiasRecordatorioMail);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de dias para los recordatorio de los mails de la agenda.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de dias para los recordatorio de los mails de la agenda. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la configuracion de dias \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: DiasRecordatorioMail " + ex.Message);
            }
        }

        protected void lbtnNombreFantasia_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtNombreFantasia.Text))
                {
                    this.configuracion.NombreFantasiaSMS = txtNombreFantasia.Text;

                    int i = configuracion.ModificarEnviarNombreFantasiaSMS(configuracion.NombreFantasiaSMS);
                    if (i > 0)
                    {
                     //   configuracion.ModificarEnviarEnvioSMSEstetica("1");
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de Nombre de Fantasia.");
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion del Nombre de Fantasia. \", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la configuracion del Nombre de Fantasia \", {type: \"info\"});", true);
                    }
                }
                else 
                {
                    lbEnvioSMS.Attributes["class"] = "btn btn-danger";
                    lbEnvioSMS.Text = "<span class='shortcut-icon icon-remove'></span>";
                    this.configuracion.EnviarSMSRecordatorio = "0";
                    DivNombreFantasia.Visible = false;
                    int i = configuracion.ModificarEnviarSMSRecordatorio(configuracion.EnviarSMSRecordatorio);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe completar en el campo Nombre Fantasia \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: NombreFantasiaSMS " + ex.Message);
            }
        }

        protected void lbEnvioSMS_Click(object sender, EventArgs e)
        {
            try
            {  
                if (lbEnvioSMS.Attributes["class"] == "btn btn-success")
                {
                    lbEnvioSMS.Attributes["class"] = "btn btn-danger";
                    lbEnvioSMS.Text = "<span class='shortcut-icon icon-remove'></span>";
                    this.configuracion.EnviarSMSRecordatorio = "0";
                    DivNombreFantasia.Visible = false;
                    int i = configuracion.ModificarEnviarSMSRecordatorio(configuracion.EnviarSMSRecordatorio);
                    if (i > 0)
                    {
                       Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico configuracion de Envio de SMS.");
                       ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de Envio de SMS. \", {type: \"info\"});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la configuracion de Envio de SMS \", {type: \"info\"});", true);
                    }
                }

                else if(lbEnvioSMS.Attributes["class"] == "btn btn-danger")
                {
                    DivNombreFantasia.Visible = true;
                    lbEnvioSMS.Attributes["class"] = "btn btn-success";
                    lbEnvioSMS.Text = "<span class='shortcut-icon icon-ok'></span>";
                    this.configuracion.EnviarSMSRecordatorio = "1";
                    int i = configuracion.ModificarEnviarSMSRecordatorio(configuracion.EnviarSMSRecordatorio);
                     ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de Envio de SMS \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: Envio de SMS " + ex.Message);
            }
        }

        protected void lbtnEnvioDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailDocumento = txtEnvioDocumento.Text;

                int i = configuracion.ModificarObservacionesMailDocumento(configuracion.ObservacionesMailDocumento);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para el envio de documentos.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de observaciones predeterminado para el envio de documentos. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion predeterminada para el envio de documentos \", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: DiasRecordatorioMail " + ex.Message);
            }
        }

        protected void lbtnEnvioFormulario_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionesMailFormulario = txtEnvioFormulario.Text;

                int i = configuracion.ModificarObservacionesMailFormulario(configuracion.ObservacionesMailFormulario);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para el envio de formularios");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modificola  configuracion de observaciones predeterminado para el envio de formularios. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion predeterminada para el envio de formularios\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: DiasRecordatorioMail " + ex.Message);
            }
        }

        protected void lbtnEnvioNotificacion_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionDespuesAtendido = txtEnvioNotificacion.Text;

                int i = configuracion.ModificarObservacionDespuesAtendido(configuracion.ObservacionDespuesAtendido);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para el envio de notificaciones despues de ser atendido");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de observaciones predeterminado para el envio de notificaciones despues de ser atendido. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion predeterminada para el envio de notificaciones despues de ser atendido\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: ObservacionDespuesAtendido " + ex.Message);
            }
        }

        protected void lbtnFormularioNotificacion_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.FormularioDespuesAtendido = ddlFormularioNotificacion.SelectedValue;

                int i = configuracion.ModificarFormularioDespuesAtendido(configuracion.FormularioDespuesAtendido);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de observaciones predeterminado para el envio de formulario despues de ser atendido");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de observaciones predeterminado para el envio de formulario despues de ser atendido. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la observacion predeterminada para el envio de formularios despues de ser atendido\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: FormularioDespuesAtendido " + ex.Message);
            }
        }

        protected void lbtnDiasFinalizadoTurno_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.DiasDespuesAtendidoMail = txtDiasDespuesDeTurno.Text;

                int i = configuracion.ModificarDiasDespuesAtendidoMail(configuracion.DiasDespuesAtendidoMail);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de cantidad de dias para mandar la notificacion despues de ser atendido");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de cantidad de dias para mandar la notificacion despues de ser atendido. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la cantidad de dias para mandar la notificacion despues de ser atendido\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: DiasDespuesAtendidoMail " + ex.Message);
            }
        }

        protected void lbtnPaisesCelular_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.PaisCelularPrederminado = ddlPaisesCelular.SelectedValue;

                int i = configuracion.ModificarPaisCelularPrederminado(configuracion.PaisCelularPrederminado);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de Pais predeterminado para los celulares");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de Pais predeterminado para los celulares. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la Pais predeterminado para los celulares\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: PaisCelularPrederminado " + ex.Message);
            }
        }

        protected void lbtnUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.UbicacionLogoMail = ddlUbicacion.SelectedValue;

                int i = configuracion.ModificarUbicacionLogoMail(configuracion.UbicacionLogoMail);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion de la ubicacion del logo en el mail");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion de la ubicacion del logo en el mail. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar la ubicacion del logo en el mail\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: UbicacionLogoMail " + ex.Message);
            }
        }

        protected void lbtnFirma_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.ObservacionFirmaMail = txtFirma.Text;

                int i = configuracion.ModificarObservacionFirmaMail(configuracion.ObservacionFirmaMail);
                if (i > 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modifico la configuracion del texto de la firma en los mails.");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Se modifico la configuracion del texto de la firma en los mails. \", {type: \"info\"});", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar el texto de la firma en los mails\", {type: \"info\"});", true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error actualizar Configuracion: FormularioDespuesAtendido " + ex.Message);
            }
        }
    }
}