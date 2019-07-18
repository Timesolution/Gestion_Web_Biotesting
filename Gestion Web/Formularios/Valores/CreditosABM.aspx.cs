using System;
using System.Linq;
using Disipar.Models;
using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
using Planario_Api;
using Planario_Api.Entidades;
using Gestion_Api.Controladores;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CreditosABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Plenario p = new Plenario();
        ControladorPlenario contPlenario = new ControladorPlenario();
        private Int64 idSolicitud;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idSolicitud = Convert.ToInt64(Request.QueryString["id"]);

                lbtnGuardar.Attributes.Add("onclick", " this.disabled = true;  " + lbtnGuardar.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(lbtnGuardar, null) + ";");
                btnValidarSeguro.Attributes.Add("onclick", " this.disabled = true;  " + btnValidarSeguro.ClientID + ".disabled=true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnValidarSeguro, null) + ";");

                verificarLogin();

                if (!IsPostBack)
                {
                    if (idSolicitud > 0)
                        this.cargarSolicitud(idSolicitud);

                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando página. Excepción: " + Ex.Message));
            }
        }

        #region Carga Inicial
        private void verificarLogin()
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
                        Response.Redirect("../../Default.aspx?m=1", false);
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
                Int16 valor = 0;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                string permiso = listPermisos.Where(x => x == "97").FirstOrDefault();
                if (!string.IsNullOrEmpty(permiso))
                    valor = 1;

                return valor;
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error verificando accesos. Excepción: " + Ex.Message));
                return -1;
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lblIdSolicitud.Text))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No hay solicitud de crédito para editar"));
                    return;
                }

                if (string.IsNullOrEmpty(txtDni.Text) || string.IsNullOrEmpty(txtNumeroSolicitud.Text))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe ingresar los datos"));
                    return;
                }

                editarSolicitud();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando a editar Solicitud de Crédito. Excepción: " + Ex.Message));
            }
        }

        protected void btnValidarSeguro_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtComentarioValidarSeguro.Text))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe ingresar un comentario"));
                    return;
                }

                validarSolicitud();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error enviando a validar solicitud. Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Funciones ABM
        public void cargarSolicitud(Int64 idSolicitud)
        {
            try
            {
                SolicitudPlenario solicitud = p.obtenerSolicitudesGestionById(idSolicitud);

                if (solicitud != null)
                {
                    lblIdSolicitud.Text = solicitud.Id.ToString();
                    txtDni.Text = solicitud.Dni.ToString();
                    txtNumeroSolicitud.Text = solicitud.NroSolicitud.ToString();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de solicitud. Excepción: " + Ex.Message));
            }
        }

        public void editarSolicitud()
        {
            try
            {
                SolicitudPlenario solicitud = p.obtenerSolicitudesGestionById(Convert.ToInt64(lblIdSolicitud.Text));

                if (solicitud == null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Solicitud de Crédito"));
                    return;
                }

                solicitud.Dni = txtDni.Text.Trim();
                solicitud.NroSolicitud = Convert.ToInt32(txtNumeroSolicitud.Text.Trim());

                int ok = p.modificarSolicitud(solicitud);
                if (ok >= 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Modificado con éxito!","CreditosF.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando Solicitud de Crédito"));

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error editando solicitud. Excepción: " + Ex.Message));
            }
        }

        public void validarSolicitud()
        {
            try
            {
                SolicitudesValidada sv = new SolicitudesValidada();
                sv.Solicitud = Convert.ToInt64(lblIdSolicitud.Text);
                sv.Fecha = DateTime.Now;
                sv.Usuario = (int)Session["Login_IdUser"];
                sv.Comentario = txtComentarioValidarSeguro.Text;

                int i = contPlenario.forzarValidarSolicitud(sv);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Solicitud validada con éxito!", "CreditosF.aspx"));

                if (i == 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se encontró la solicitud para validar."));

                if (i == -1)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error guardando los datos de la validación segura."));

                if (i == -2)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error modificando registro de solicitud."));

                if (i == -3)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error validando solicitud."));
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error enviando a validar solicitud. Excepción: " + Ex.Message));
            }
        }
        #endregion

        
    }
}