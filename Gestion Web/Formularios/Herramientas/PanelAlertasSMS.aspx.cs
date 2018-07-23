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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class PanelAlertasSMS : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion configuracion = new Configuracion();
        ControladorConfiguracion contConfig = new ControladorConfiguracion();
        ControladorSMS contSMS = new ControladorSMS();
        Mensajes mje = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                //this.idPuntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    this.cargarAlertas();
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

        private void cargarAlertas()
        {
            try
            {
                Configuraciones_SMS configs = this.contConfig.ObtenerConfiguracionesAlertasSMS();

                if (configs != null)
                {
                    if (configs.Estado == 1)
                    {
                        if (configs.AlertaFC.Value == 1)
                        {
                            this.chkAlertaFC.Checked = true;                            
                            this.txtEnvioFact.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaNC.Value == 1)
                        {
                            this.chkAlertaNC.Checked = true;                            
                            this.txtEnvioNC.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaPRP.Value == 1)
                        {
                            this.chkAlertaPRP.Checked = true;                            
                            this.txtEnvioPRP.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaNCPRP.Value == 1)
                        {
                            this.chkAlertaNCPRP.Checked = true;                            
                            this.txtEnvioNCPRP.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaFcVencida.Value == 1)
                        {
                            this.chkAlertaFCVencida.Checked = true;                            
                            this.txtEnvioFactVencida.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaSaldoCC.Value == 1)
                        {
                            this.chkAlertaSaldoCtaCte.Checked = true;                            
                            this.txtDiaSaldoCtaCte.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaSaldoMax.Value == 1)
                        {
                            this.chkAlertaSaldoMax.Checked = true;                            
                            this.txtEnvioSaldoMax.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaCumpleanios.Value == 1)
                        {
                            this.chkAlertaCumple.Checked = true;
                            this.txtEnvioCumple.Attributes.Remove("disabled");
                        }
                        if (configs.AlertaCobro.Value == 1)
                        {
                            this.chkAlertaCobro.Checked = true;
                            this.txtEnvioCobro.Attributes.Remove("disabled");
                        }
                        this.txtEnvioFact.Text = configs.MensajeFC;
                        this.txtEnvioNC.Text = configs.MensajeNC;
                        this.txtEnvioPRP.Text = configs.MensajePRP;
                        this.txtEnvioNCPRP.Text = configs.MensajeNCPRP;
                        this.txtEnvioFactVencida.Text = configs.MensajeFcVencida;
                        this.txtDiaSaldoCtaCte.Text = configs.MensajeSaldoCC;
                        this.txtEnvioSaldoMax.Text = configs.MensajeSaldoMax;
                        this.txtEnvioCumple.Text = configs.MensajeCumpleanios;
                        this.txtEnvioCobro.Text = configs.MensajeCobro;

                        this.PanelConfig.Visible = true;
                        this.PanelCondiciones.Visible = false;
                        this.lbtnActivarSMS.Visible = false;
                        this.lbtnDesactivarSMS.Visible = true;
                    }
                }
            }
            catch
            {

            }
        }

        protected void lbtnActivarSMS_Click(object sender, EventArgs e)
        {
            try
            {
                Configuraciones_SMS config = this.contConfig.ObtenerConfiguracionesAlertasSMS();
                if (config != null)
                {
                    config.Estado = 1;

                    int i = this.contConfig.guardarConfiguracionesSMS(config);
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});setTimeout(location.href='PanelAlertasSMS.aspx',1500);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                    }
                }
                else
                {
                    Configuraciones_SMS c = new Configuraciones_SMS();
                    c.Estado = 1;
                    int i = this.contConfig.altaConfiguracionesSMS(c);
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});location.href='PanelAlertasSMS.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                    }
                }

            }
            catch
            {

            }
        }

        protected void lbtnDesactivarSMS_Click(object sender, EventArgs e)
        {
            try
            {
                Configuraciones_SMS config = this.contConfig.ObtenerConfiguracionesAlertasSMS();
                if (config != null)
                {
                    config.Estado = 0;

                    int i = this.contConfig.guardarConfiguracionesSMS(config);
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});setTimeout(location.href='PanelAlertasSMS.aspx',1500);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                    }
                }
            }
            catch
            {

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtEnvioCobro.Text.Contains("@@COBRO"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No eliminar esto '@@COBRO'. \", {type: \"error\"});", true);
                    txtEnvioCobro.Text = "'Ingrese su texto' @@COBRO ";
                }
                else
                {
                    Configuraciones_SMS config = this.contConfig.ObtenerConfiguracionesAlertasSMS();

                    config.AlertaFC = Convert.ToInt32(this.chkAlertaFC.Checked);
                    config.AlertaNC = Convert.ToInt32(this.chkAlertaNC.Checked);
                    config.AlertaPRP = Convert.ToInt32(this.chkAlertaPRP.Checked);
                    config.AlertaNCPRP = Convert.ToInt32(this.chkAlertaNCPRP.Checked);
                    config.AlertaFcVencida = Convert.ToInt32(this.chkAlertaFCVencida.Checked);
                    config.AlertaSaldoCC = Convert.ToInt32(this.chkAlertaSaldoCtaCte.Checked);
                    config.AlertaSaldoMax = Convert.ToInt32(this.chkAlertaSaldoMax.Checked);
                    config.AlertaCumpleanios = Convert.ToInt32(this.chkAlertaCumple.Checked);
                    config.AlertaCobro = Convert.ToInt32(this.chkAlertaCobro.Checked);

                    config.MensajeFC = this.txtEnvioFact.Text;
                    config.MensajeFcVencida = this.txtEnvioFactVencida.Text;
                    config.MensajeNC = this.txtEnvioNC.Text;
                    config.MensajeNCPRP = this.txtEnvioNCPRP.Text;
                    config.MensajePRP = this.txtEnvioPRP.Text;
                    config.MensajeSaldoCC = this.txtDiaSaldoCtaCte.Text;
                    config.MensajeSaldoMax = this.txtEnvioSaldoMax.Text;
                    config.MensajeCumpleanios = this.txtEnvioCumple.Text;
                    config.MensajeCobro = this.txtEnvioCobro.Text;

                    int i = this.contConfig.guardarConfiguracionesSMS(config);
                    if (i > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Guardado con exito!.\", {type: \"info\"});location.href='PanelAlertasSMS.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo guardar cambios\";", true);
                    }
                }
            }
            catch
            {

            }
        }

        protected void lbtnEnviarSMSPrueba_Click(object sender, EventArgs e)
        {
            try
            {
                string codArea = this.txtCodArea.Text;
                string numero = this.txtTelefono.Text;
                if (codArea.Length + numero.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTestSMS, UpdatePanelTestSMS.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalidos!.\");", true);
                    return;
                }

                string telefono = "+549" + codArea + numero;
                string mensaje = this.txtMensajeSMS.Text;
                
                int i = this.contSMS.enviarAlertaTesting(telefono, mensaje, (int)Session["Login_IdUser"]);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTestSMS, UpdatePanelTestSMS.GetType(), "alert", "$.msgbox(\"SMS enviado con exito!.\", {type: \"info\"});", true);
                }
                else
                {
                    if (i == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTestSMS, UpdatePanelTestSMS.GetType(), "alert", "$.msgbox(\"No se encuentra habilitado el envio de SMS.\");", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanelTestSMS, UpdatePanelTestSMS.GetType(), "alert", "$.msgbox(\"No se pudo enviar SMS.\");", true);
                    }
                }
            }
            catch
            {

            }
        }
    }
}