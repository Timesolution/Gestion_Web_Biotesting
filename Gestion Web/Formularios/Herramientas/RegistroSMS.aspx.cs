﻿using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class RegistroSMS : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        ControladorSMS contSMS = new ControladorSMS();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int asunto;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.asunto = Convert.ToInt32(Request.QueryString["a"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaD) && String.IsNullOrEmpty(this.fechaD))
                    {
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                    }

                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                }                
                this.cargarMensajes();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
        private void cargarMensajes()
        {
            try
            {
                this.phCodigos.Controls.Clear();

                decimal cantidad = 0;

                DateTime desde = Convert.ToDateTime(this.txtFechaDesde.Text,new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<SMS_Enviados> mensajes = this.contSMS.obtenerMensajesByFecha(desde, hasta,this.asunto);
                foreach (var mje in mensajes)
                {                    
                    this.cargarMensajesPH(mje);
                    cantidad++;
                }
                this.labelTotal.Text = cantidad.ToString();
            }
            catch
            {

            }
        }
        private void cargarMensajesPH(SMS_Enviados mje)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = mje.Id.ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = mje.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Center;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.Width = Unit.Percentage(15);
                tr.Cells.Add(celFecha);

                TableCell celCliente = new TableCell();
                celCliente.Text = mje.Cliente;
                celCliente.HorizontalAlign = HorizontalAlign.Center;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.Width = Unit.Percentage(15);
                tr.Cells.Add(celCliente);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = mje.Telefono;
                celTelefono.HorizontalAlign = HorizontalAlign.Center;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                celTelefono.Width = Unit.Percentage(15);
                tr.Cells.Add(celTelefono);

                TableCell celMensaje = new TableCell();
                celMensaje.Text = mje.Mensaje;
                celMensaje.HorizontalAlign = HorizontalAlign.Center;
                celMensaje.VerticalAlign = VerticalAlign.Middle;
                celMensaje.Width = Unit.Percentage(55);
                tr.Cells.Add(celMensaje);

                this.phCodigos.Controls.Add(tr);
            }
            catch
            {

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("RegistroSMS.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
            }
            catch (Exception ex)
            {                

            }
        }
    }
}
