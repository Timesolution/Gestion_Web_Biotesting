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
    public partial class ProspectosF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private string fechaD;
        private string fechaH;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.VerificarLogin();

                if (!IsPostBack)
                {
                    fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");

                    CargarProspectos();
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



        private void CargarProspectos(List<Prospectos> pros = null)
        {
            try
            {

                ControladorProspectos controladorProspectos = new ControladorProspectos();

                if(pros != null)
                {
                    foreach (Prospectos pro in pros)
                    {
                        this.CargarEnPh(pro);
                    }
                }
                else
                {
                    var prospectos = controladorProspectos.ObtenerProspectos();

                    foreach (Prospectos pro in prospectos)
                    {
                        this.CargarEnPh(pro);
                    }
                }

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }
        private void CargarEnPh(Prospectos pro)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = pro.Id.ToString();

                TableCell celNombre = new TableCell();
                celNombre.Text = pro.NombreApellido;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNombre);

                TableCell celFecha = new TableCell();
                celFecha.Text = pro.FechaNacimiento.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Center;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipoDocumento = new TableCell();
                celTipoDocumento.Text = pro.TipoDocumento;
                celTipoDocumento.HorizontalAlign = HorizontalAlign.Center;
                celTipoDocumento.VerticalAlign = VerticalAlign.Middle;
                celTipoDocumento.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipoDocumento);

                TableCell celDocumento = new TableCell();
                celDocumento.Text = pro.Documento;
                celDocumento.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDocumento);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + pro.Id;
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                btnEditar.PostBackUrl = "ProspectosABM?a=2&id=" + pro.Id.ToString();
                celAccion.Controls.Add(btnEditar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + pro.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmar");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + pro.Id + ");";
                celAccion.Controls.Add(btnEliminar);

                //Literal l3 = new Literal();
                //l3.Text = "&nbsp";
                //celAccion.Controls.Add(l3);

                //LinkButton btnRedireccionarCRM = new LinkButton();
                //btnRedireccionarCRM.ID = "btnRedireccionar_" + clientes_Eventos.Id;
                //btnRedireccionarCRM.CssClass = "btn btn-info ui-tooltip";
                //btnRedireccionarCRM.Attributes.Add("data-toggle", "tooltip");
                //btnRedireccionarCRM.PostBackUrl = "../Clientes/ClientesABM.aspx?accion=2&id=" + clientes_Eventos.Cliente.ToString();
                //btnRedireccionarCRM.Text = "<span class='shortcut-icon icon-user'></span>";
                //btnRedireccionarCRM.Attributes.Add("title data-original-title", "Ir a CRM");
                //celAccion.Controls.Add(btnRedireccionarCRM);
                tr.Cells.Add(celAccion);

                phProspectos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando CRM. " + ex.Message));
            }

        }

        #endregion

        protected void lbtnEliminarProspecto_Click(object sender, EventArgs e)
        {
            ControladorProspectos controladorProspectos = new ControladorProspectos();

            int idProspecto = Convert.ToInt32(this.txtMovimiento.Text);


            // Se pone en estado 0 el prospecto
            var prospecto = controladorProspectos.EliminarProspectoById(idProspecto);

            if(prospecto != null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Registro eliminado con exito", null));
            }

            CargarProspectos();

        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspecto = new ControladorProspectos();

                DateTime desde = Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR"));
                hasta = hasta.AddHours(23).AddMinutes(59);

                var prospectos = controladorProspecto.Obtenerfiltro(desde, hasta, txtDocumento.Text);

                CargarProspectos(prospectos);

            }
            catch (Exception ex)
            {

            }
        }
    }
}