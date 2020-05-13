using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class MateriasPrimasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorMateriaPrima contMateriaPrima = new controladorMateriaPrima();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.cargarMateriasPrimasAlPH();
            }
            catch (Exception ex)
            {

            }
        }

        #region funciones pageLoad
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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                if (listPermisos.Contains("194"))
                    return 1;

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarMateriasPrimasAlPH()
        {
            try
            {
                var lista = this.contMateriaPrima.obtenerMateriasPrimas();

                foreach (var item in lista)
                {
                    if (item.Estado == 1)
                    {
                        TableRow tr = new TableRow();

                        TableCell celCodigo = new TableCell();
                        celCodigo.Text = item.Codigo;
                        celCodigo.Width = Unit.Percentage(5);
                        celCodigo.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celCodigo);

                        TableCell celDescipcion = new TableCell();
                        celDescipcion.Text = item.Descripcion;
                        celDescipcion.Width = Unit.Percentage(5);
                        celDescipcion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celDescipcion);

                        TableCell celStockMinimo = new TableCell();
                        celStockMinimo.Text = item.StockMinimo.ToString();
                        celStockMinimo.Width = Unit.Percentage(5);
                        celStockMinimo.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celStockMinimo);

                        TableCell celUnidad = new TableCell();
                        celUnidad.Text = item.UnidadMedida;
                        celUnidad.Width = Unit.Percentage(5);
                        celUnidad.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celUnidad);

                        TableCell celImporte = new TableCell();
                        celImporte.Text = item.Importe.ToString();
                        celImporte.Width = Unit.Percentage(5);
                        celImporte.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celImporte);

                        TableCell celMoneda = new TableCell();
                        celMoneda.Text = item.moneda1.moneda1.ToString();
                        celMoneda.Width = Unit.Percentage(5);
                        celMoneda.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celMoneda);

                        TableCell celAction = new TableCell();
                        celAction.Width = Unit.Percentage(20);
                        celAction.HorizontalAlign = HorizontalAlign.Center;

                        LinkButton btnEditar = new LinkButton();
                        btnEditar.ID = "btnEditar_" + item.Id;
                        btnEditar.CssClass = "btn btn-info ui-tooltip";
                        btnEditar.Attributes.Add("data-toggle", "tooltip");
                        btnEditar.Attributes.Add("title data-original-title", "Stock");
                        btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                        btnEditar.PostBackUrl = "MateriasPrimasABM.aspx?a=2&id=" + item.Id;
                        celAction.Controls.Add(btnEditar);

                        LinkButton btnStock = new LinkButton();
                        btnStock.ID = "btnStock_" + item.Id;
                        btnStock.CssClass = "btn btn-info ui-tooltip";
                        btnStock.Attributes.Add("data-toggle", "tooltip");
                        btnStock.Attributes.Add("title data-original-title", "Stock");
                        btnStock.Text = "<span class='shortcut-icon icon-list-alt'></span>";
                        btnStock.PostBackUrl = "StockFMP.aspx?mp=" + item.Id;
                        celAction.Controls.Add(btnStock);

                        Literal l2 = new Literal();
                        l2.Text = "&nbsp";
                        celAction.Controls.Add(l2);

                        LinkButton btnEliminar = new LinkButton();
                        btnEliminar.ID = "btnEliminar_" + item.Id;
                        btnEliminar.CssClass = "btn btn-info";
                        btnEliminar.Attributes.Add("data-toggle", "modal");
                        btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                        btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                        btnEliminar.OnClientClick = "abrirdialog(" + item.Id.ToString() + ");";
                        celAction.Controls.Add(btnEliminar);

                        tr.Cells.Add(celAction);

                        this.phMateriasPrimas.Controls.Add(tr); //agrego la fila a la tabla
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region funciones botones
        protected void btnEliminarMateriaPrima_Click(object sender, EventArgs e)
        {
            try
            {
                int idMateriaPrima = Convert.ToInt32(this.txtMovimiento.Text);
                MateriaPrima materiaPrima = this.contMateriaPrima.obtenerMateriaPrima(idMateriaPrima);
                materiaPrima.Estado = 0;
                int i = this.contMateriaPrima.modificarMateriaPrima(materiaPrima);
                if (i > 0)
                {
                    //elimino bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja de materia prima: " + materiaPrima.Descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Materia prima eliminada con exito", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Materia prima"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Materia prima. " + ex.Message));
            }
            finally
            {
                Response.Redirect("MateriasPrimasF.aspx");
            }
        }
    }
    #endregion

}