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

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ConsolidarP : System.Web.UI.Page
    {
 
        ControladorPedido controladorPedido = new ControladorPedido();

        Mensajes m = new Mensajes();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                string pedidos = Request.QueryString["pedidos"];
                if(!IsPostBack)
                {
                    CargarPedidos(pedidos);
                }
                



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }


        public void CargarPedidos(string pedidos)
        {
            var listaPedidos = controladorPedido.obtenerPedidos(pedidos);

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

        #endregion
    }
}

