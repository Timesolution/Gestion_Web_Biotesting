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
    public partial class ComisionesF : System.Web.UI.Page
    {
        controladorCliente contCliente = new controladorCliente();

        Mensajes m = new Mensajes();
        private string fechaD;
        private string fechaH;
        private int idcliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                //datos de filtro
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                idcliente = Convert.ToInt32(Request.QueryString["cl"]);

                if (!IsPostBack)
                {


                    if (fechaD == null && fechaH == null && idcliente == 0)
                    {
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    }

                    this.cargarClientes();
                }

                if (idcliente > 0 || idcliente == -1)
                {

                    CargarComisiones();
                }

                this.Form.DefaultButton = this.btnBuscarCod.UniqueID;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }


        private void CargarComisiones()
        {
            try
            {
                controladorCuentaCorriente controladorCuentaCorriente = new controladorCuentaCorriente();

                var lista = controladorCuentaCorriente.ObtenerComisiones(fechaD, fechaH, idcliente);

                foreach (DataRow row in lista.Rows)
                {
                    decimal total = (Convert.ToDecimal(row["saldo"].ToString()) - Convert.ToDecimal(row["iva"].ToString())) - Convert.ToDecimal(row["arttotal"].ToString());
                    this.cargarEnPh(row, total);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando eventos clientes en CRM.  " + ex.Message));
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

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();

                dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        private void cargarEnPh(DataRow row, decimal total)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = row["id"].ToString();

                //Celdas

                TableCell celCliente = new TableCell();
                celCliente.Text = row["razonSocial"].ToString();
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = row["saldo"].ToString();
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSaldo);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = row["arttotal"].ToString();
                celArticulo.HorizontalAlign = HorizontalAlign.Right;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell Iva21 = new TableCell();
                Iva21.Text = row["iva"].ToString();
                Iva21.HorizontalAlign = HorizontalAlign.Right;
                Iva21.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(Iva21);

                TableCell Total = new TableCell();
                Total.Text = Convert.ToString(total);
                Total.HorizontalAlign = HorizontalAlign.Right;
                Total.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(Total);

                TableCell padre = new TableCell();
                padre.HorizontalAlign = HorizontalAlign.Right;
                padre.VerticalAlign = VerticalAlign.Middle;
                TextBox txt1 = new TextBox();
                padre.Controls.Add(txt1);
                tr.Cells.Add(padre);

                TableCell abuelo = new TableCell();
                abuelo.HorizontalAlign = HorizontalAlign.Right;
                abuelo.VerticalAlign = VerticalAlign.Middle;
                TextBox txt2 = new TextBox();
                abuelo.Controls.Add(txt2);
                tr.Cells.Add(abuelo);

                //TableCell celAccion = new TableCell();
                //LinkButton btnConfirmacion = new LinkButton();
                //btnConfirmacion.CssClass = "btn btn-info";
                //btnConfirmacion.Attributes.Add("data-toggle", "modal");
                //btnConfirmacion.Attributes.Add("href", "#modalConfirmarFinalizado");
                //btnConfirmacion.ID = "btnSelec_" + clientes_Eventos.Id.ToString();
                //btnConfirmacion.Text = "<span class='shortcut-icon icon-ok'></span>";
                //btnConfirmacion.ToolTip = "Marcar como finalizado";
                //btnConfirmacion.OnClientClick = "abrirdialog(" + clientes_Eventos.Id + ");";
                //btnConfirmacion.Font.Size = 12;
                //celAccion.Controls.Add(btnConfirmacion);
                //tr.Cells.Add(celAccion);

                phComsiones.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Comisiones. " + ex.Message));
            }

        }

        #endregion

        #region Eventos Controles
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    Response.Redirect("ComisionesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&cl=" + DropListClientes.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));

            }
        }

        #endregion
    }
}