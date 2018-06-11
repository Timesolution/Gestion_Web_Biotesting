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
    public partial class AsuntosSMS : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        ControladorSMS contSMS = new ControladorSMS();

        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int idUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];

                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(fechaD) && String.IsNullOrEmpty(fechaH))
                    {
                        this.fechaD = DateTime.Today.AddYears(-1).ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                    }
                    this.txtFechaDesde.Text = this.fechaD;
                    this.txtFechaHasta.Text = this.fechaH;
                }
                this.cargarAsuntos();             
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
        private void cargarAsuntos()
        {
            try
            {
                this.phMotivos.Controls.Clear();
                DateTime desde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR"));
                decimal total = 0;

                List<SMS_Asuntos> asuntos = this.contSMS.obtenerAsuntosSMSByFecha(desde,hasta);
                foreach (var a in asuntos)
                {
                    this.cargarAsuntosPH(a);
                    total += a.SMS_Enviados.Count;
                }
                this.labelTotal.Text = total.ToString();
            }
            catch
            {

            }
        }
        private void cargarAsuntosPH(SMS_Asuntos a)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = a.Id.ToString();

                TableCell celFecha = new TableCell();                
                celFecha.Text = a.FechaProgramada.Value.ToString("dd/MM/yyyy hh:mm:ss tt");
                celFecha.Width = Unit.Percentage(20);
                tr.Controls.Add(celFecha);

                TableCell celNombre = new TableCell();
                celNombre.Text = a.Nombre;
                celNombre.Width = Unit.Percentage(50);
                tr.Controls.Add(celNombre);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = a.SMS_Enviados.Count.ToString();
                celCantidad.Width = Unit.Percentage(10);
                tr.Controls.Add(celCantidad);

                TableCell celAccion = new TableCell();

                LinkButton btnVer = new LinkButton();
                btnVer.ID = "btnVer_" + a.Id;
                btnVer.CssClass = "btn btn-info";
                btnVer.PostBackUrl = "RegistroSMS.aspx?a=" + a.Id + "&fd=" + a.FechaProgramada.Value.ToString("dd/MM/yyyy") + "&fh=" + DateTime.Today.ToString("dd/MM/yyyy");
                btnVer.Text = "<span class='shortcut-icon icon-search'></span>";
                btnVer.Font.Size = 9;
                celAccion.Controls.Add(btnVer);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + a.Id;
                btnEditar.CssClass = "btn btn-info";
                btnEditar.PostBackUrl = "ABMAsuntosSMS.aspx?a=2&id=" + a.Id.ToString();
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 9;
                celAccion.Controls.Add(btnEditar);
                if (a.Id == 1)
                {
                    btnEditar.Attributes.Add("disabled", "disabled");
                }

                celAccion.Width = Unit.Percentage(20);
                tr.Controls.Add(celAccion);

                this.phMotivos.Controls.Add(tr);
            }
            catch
            {

            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AsuntosSMS.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
            }
            catch
            {

            }
        }
    }
}
