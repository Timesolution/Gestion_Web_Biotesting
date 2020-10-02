using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Gestion_Api.Entitys;
using System.Globalization;
using System.Web.Configuration;
using Gestion_Api.Modelo.Enums;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class FamiliaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.VerificarLogin();
                    CargarClientes();
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        #region carga inicial
        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        public void CargarClientes()
        {
            controladorCliente controladorCliente = new controladorCliente();

            ListPadre.DataSource = controladorCliente.obtenerClientesDT();
            ListPadre.DataValueField = "id";
            ListPadre.DataTextField = "razonSocial";

            ListPadre.DataBind();
        }

        #endregion

        protected void ListPadre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = ListPadre.SelectedValue;

            }
            catch (Exception ex)
            {

            }

        }
    }
}