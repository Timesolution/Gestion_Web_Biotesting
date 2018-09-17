using Disipar.Models;
using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class FacturasMercaderiasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            if (!IsPostBack)
            {
                cargarSucursales();
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

                return 1;

                //foreach (string s in listPermisos)
                //{

                //    if (!String.IsNullOrEmpty(s))
                //    {
                        
                //    }
                //}
            }
            catch
            {
                return -1;
            }
        }

        public void cargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursalOrigen.DataSource = dt;
                this.DropListSucursalOrigen.DataValueField = "Id";
                this.DropListSucursalOrigen.DataTextField = "nombre";
                this.DropListSucursalOrigen.DataBind();

                this.DropListSucursalOrigen.SelectedValue = Session["Login_SucUser"].ToString();

                this.DropListSucursalDestino.DataSource = dt;
                this.DropListSucursalDestino.DataValueField = "Id";
                this.DropListSucursalDestino.DataTextField = "nombre";
                this.DropListSucursalDestino.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Response.Redirect("OrdenReparacionF.aspx?a=0&c=" + this.DropListClientes.SelectedValue + "&s=" + DropListSucursal.SelectedValue + "&fd=" + txtFechaDesde.Text + "&fh=" + txtFechaHasta.Text + "&e=" + DropListEstados.SelectedValue);
            //}
            //catch (Exception ex)
            //{
            //    Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
            //}
        }

        //private void cargarEnPh()
        //{
        //    try
        //    {
        //        controladorSucursal contSucursal = new controladorSucursal();
        //        controladorFacturacion contFacturacion = new controladorFacturacion();

        //        //fila
        //        TableRow tr = new TableRow();
        //        tr.ID = or.Id.ToString();

        //        //Celdas

        //        TableCell celFecha = new TableCell();
        //        celFecha.Text = or.Fecha.Value.ToString("dd/MM/yyyy");
        //        celFecha.HorizontalAlign = HorizontalAlign.Left;
        //        celFecha.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celFecha);

        //        TableCell celNumeroOrden = new TableCell();
        //        celNumeroOrden.Text = or.NumeroOrdenReparacion.Value.ToString("D8");
        //        celNumeroOrden.HorizontalAlign = HorizontalAlign.Left;
        //        celNumeroOrden.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celNumeroOrden);

        //        TableCell celAccion = new TableCell();

        //        Literal lDetail = new Literal();
        //        lDetail.ID = "btnEditar_" + or.Id.ToString();
        //        lDetail.Text = "<a href=\"OrdenReparacionABM.aspx?a=2&idordenreparacion=" + or.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
        //        lDetail.Text += "<span class=\"shortcut-icon icon-pencil\"></span>";
        //        lDetail.Text += "</a>";

        //        celAccion.Controls.Add(lDetail);

        //        Literal l1 = new Literal();
        //        l1.Text = "&nbsp";
        //        celAccion.Controls.Add(l1);

        //        Literal lReport = new Literal();
        //        lReport.ID = "btnReporte_" + or.Id.ToString();
        //        lReport.Text = "<a href=\"ImpresionOrdenReparacion.aspx?a=1&or=" + or.Id.ToString() + "&prp=" + or.NumeroPRP.ToString() + "\"" + "target =\"_blank\"" + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
        //        lReport.Text += "<span class=\"shortcut-icon icon-search\"></span>";
        //        lReport.Text += "</a>";

        //        celAccion.Controls.Add(lReport);                

        //        tr.Cells.Add(celAccion);

        //        phOrdenReparacion.Controls.Add(tr);

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
        //    }
        //}
    }
}