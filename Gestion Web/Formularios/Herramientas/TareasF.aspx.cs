﻿using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Task_Api;
using Task_Api.Entitys;
using Mascotas_Api.Controladores;
using Mascotas_Api.Entity;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class TareasF : System.Web.UI.Page
    {        
        controladorUsuario controlador = new controladorUsuario();
        ControladorTareas contTareas = new ControladorTareas();
        controladorCliente contCliente = new controladorCliente();
        ControladorPropietarios contPropietarios = new ControladorPropietarios();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int estado;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.estado = Convert.ToInt32(Request.QueryString["e"]);

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaD) && String.IsNullOrEmpty(this.fechaD))
                    {
                        this.fechaD = DateTime.Today.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                    }

                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                    this.ListEstado.SelectedValue = this.estado.ToString();
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
                this.phTareas.Controls.Clear();

                decimal cantidad = 0;
                string origen = WebConfigurationManager.AppSettings.Get("OrigenSMS");
                if (origen == null)
                {
                    origen = "0";
                }
                DateTime desde = Convert.ToDateTime(this.txtFechaDesde.Text,new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                List<Tarea> tareas = this.contTareas.obtenerTareasByFechaEstado(desde, hasta, this.estado,Convert.ToInt32(origen));
                foreach (var t in tareas)
                {                    
                    this.cargarMensajesPH(t);
                    cantidad++;
                }
                this.labelTotal.Text = cantidad.ToString();
            }
            catch
            {

            }
        }
        private void cargarMensajesPH(Tarea t)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = t.Id.ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = t.Fecha.Value.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Center;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.Width = Unit.Percentage(10);
                tr.Cells.Add(celFecha);

                TableCell celCliente = new TableCell();
                celCliente.Text = "";

                if(t.Origen==4)
                {
                    var idCliente = this.contPropietarios.obtenerClienteByIdPropietario(Convert.ToInt32(t.Datos));
                    if(idCliente > 0)
                    {
                        var c = this.contCliente.obtenerClienteID(idCliente);
                        if (c != null)
                            celCliente.Text = c.razonSocial;
                    }
                }
                else
                {
                    var c = this.contCliente.obtenerClienteID(Convert.ToInt32(t.Datos));
                    if (c != null)
                        celCliente.Text = c.razonSocial;
                }
                
                celCliente.HorizontalAlign = HorizontalAlign.Center;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.Width = Unit.Percentage(20);
                tr.Cells.Add(celCliente);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = t.TareasDetalles.FirstOrDefault().Numero;
                celTelefono.HorizontalAlign = HorizontalAlign.Center;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                celTelefono.Width = Unit.Percentage(10);
                tr.Cells.Add(celTelefono);

                TableCell celMensaje = new TableCell();
                celMensaje.Text = t.TareasDetalles.FirstOrDefault().Mensaje;
                celMensaje.HorizontalAlign = HorizontalAlign.Center;
                celMensaje.VerticalAlign = VerticalAlign.Middle;
                celMensaje.Width = Unit.Percentage(50);
                tr.Cells.Add(celMensaje);

                TableCell celEstado = new TableCell();
                if (t.Ejecutada.Value == 0)
                    celEstado.Text = "Pendiente";
                else
                    celEstado.Text = "Enviado";
                celEstado.HorizontalAlign = HorizontalAlign.Center;
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.Width = Unit.Percentage(10);
                tr.Cells.Add(celEstado);

                this.phTareas.Controls.Add(tr);
            }
            catch
            {

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TareasF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&e=" + this.ListEstado.SelectedValue);
            }
            catch (Exception ex)
            {                

            }
        }
    }
}
