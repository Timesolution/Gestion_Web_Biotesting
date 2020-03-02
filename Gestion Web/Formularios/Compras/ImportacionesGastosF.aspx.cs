using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImportacionesGastosF : System.Web.UI.Page
    {
        ControladorImportaciones contImportacion = new ControladorImportaciones();

        DataTable dtItemsTemp;
        Mensajes m = new Mensajes();

        int idImportacion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                this.idImportacion = Convert.ToInt32(Request.QueryString["id"]);
                this.VerificarLogin();                

                if (!IsPostBack)
                {
                    this.cargarMonedasImportacion();
                    this.cargarTiposGastoImportacion();
                }

                this.cargarDetalleImportacion();
                
            }
            catch (Exception ex)
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "74")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        public void cargarMonedasImportacion()
        {
            try
            {
                List<Monedas_Importacion> list = this.contImportacion.obtenerMonedasImportacion();

                this.ListMonedaImportacion.DataSource = list;
                this.ListMonedaImportacion.DataValueField = "Id";
                this.ListMonedaImportacion.DataTextField = "Moneda";
                this.ListMonedaImportacion.DataBind();

                this.ListMonedaImportacion.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        public void cargarTiposGastoImportacion()
        {
            try
            {
                List<TipoGastos_Importacion> list = this.contImportacion.obtenerTiposGastoImportacion();
                list = list.OrderBy(x => x.TipoGasto).ToList();
                this.ListTipoGasto.DataSource = list;
                this.ListTipoGasto.DataValueField = "Id";
                this.ListTipoGasto.DataTextField = "TipoGasto";
                this.ListTipoGasto.DataBind();

                this.ListTipoGasto.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarDetalleImportacion()
        {
            try
            {
                this.phGastos.Controls.Clear();
                decimal totalGastos = 0;
                Importacione i = this.contImportacion.obtenerImportacionByID(this.idImportacion);
                if (i != null)
                {
                    foreach (var item in i.Importaciones_Gastos)
                    {
                        this.cargarGastoImportacionPH(item);
                        totalGastos += item.ImportePesos.Value;
                    }
                }
                this.lblTotalGastos.Text = totalGastos.ToString("C");
            }
            catch
            {

            }
        }
        private void cargarGastoImportacionPH(Importaciones_Gastos gasto)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = "tr_" + gasto.Id;                

                TableCell celTipo = new TableCell();
                celTipo.Text = gasto.TipoGastos_Importacion.TipoGasto;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celTipo);

                TableCell celImportePesos = new TableCell();
                celImportePesos.Text = gasto.ImportePesos.Value.ToString("C");
                celImportePesos.HorizontalAlign = HorizontalAlign.Right;
                celImportePesos.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celImportePesos);

                TableCell celMoneda = new TableCell();
                celMoneda.Text = gasto.Monedas_Importacion.Moneda;
                celMoneda.HorizontalAlign = HorizontalAlign.Left;
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celMoneda);

                TableCell celCambioMoneda = new TableCell();
                celCambioMoneda.Text = gasto.TipoCambio.Value.ToString("C");
                celCambioMoneda.HorizontalAlign = HorizontalAlign.Right;
                celCambioMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celCambioMoneda);                

                TableCell celImporteTotal = new TableCell();
                celImporteTotal.Text = decimal.Round((gasto.ImportePesos.Value / gasto.TipoCambio.Value), 2).ToString("C");
                celImporteTotal.HorizontalAlign = HorizontalAlign.Right;
                celImporteTotal.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celImporteTotal);

                TableCell celAccion = new TableCell();
                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + gasto.Id.ToString();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + gasto.Id.ToString() + ");";
                celAccion.Controls.Add(btnEliminar);
                tr.Controls.Add(celAccion);

                this.phGastos.Controls.Add(tr);
            }
            catch
            {

            }
        }       
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.txtMovimiento.Text);

                int ok = this.contImportacion.eliminarGastoImportacion(id);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("ELminado con exito!. ", Request.Url.ToString()));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error anulando gasto. " + ex.Message));
            }
        }
        protected void lbtnAgregarGasto_Click(object sender, EventArgs e)
        {
            try
            {
                Importacione i = this.contImportacion.obtenerImportacionByID(this.idImportacion);
                Importaciones_Gastos gasto = new Importaciones_Gastos();
                gasto.IdTipoGasto = Convert.ToInt32(this.ListTipoGasto.SelectedValue);
                gasto.IdMonedaImportacion = Convert.ToInt32(this.ListMonedaImportacion.SelectedValue);
                gasto.TipoCambio = Convert.ToDecimal(this.txtCambioMonedaImportacion.Text);                
                gasto.ImportePesos = Convert.ToDecimal(this.txtTotalPesos.Text);
                i.Importaciones_Gastos.Add(gasto);

                int ok = this.contImportacion.modificarImportacion(i);
                if (ok > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Agregada con Exito\", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se Pudo agregar.\";", true);
                }

            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error agregando. " + ex.Message + ".\";", true);
            }
        }
    }
}