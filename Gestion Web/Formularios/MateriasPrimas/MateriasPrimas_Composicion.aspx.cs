using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class ComposicionMateriaPrima : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
        controladorMateriaPrima contMateriaPrima = new controladorMateriaPrima();
        controladorCobranza contCobranza = new controladorCobranza();
        ControladorCobranzaEntity contCobranzaEntity = new ControladorCobranzaEntity();
        Mensajes m = new Mensajes();
        int idArt;
        List<MateriaPrima_Composiciones> listaComposiciones = new List<MateriaPrima_Composiciones>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idArt = Convert.ToInt32(Request.QueryString["idArt"]);
                this.lbNombreArticulo.Text = this.contArticulosEntity.obtenerArticuloEntity(idArt).descripcion;
                if (!IsPostBack)
                {
                    this.cargarDDLMateriasPrimas(null);
                }
                this.cargarComposicionAlPH();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.updatePanel1, updatePanel1.GetType(), "alert", "$.msgbox(\"Error en fun: Page_Load de MateriaPrima_Composicion" + ex.Message + "\", {type: \"error\"});", true);
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

                if (listPermisos.Contains("195"))
                    return 1;

                return valor;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region funciones botones
        protected void lbtnBuscarMP_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    DataTable materiasPrimasDT = this.contMateriaPrima.obtenerMateriasPrimasByDescDT(this.txtMPBusqueda.Text.Replace(' ', '%'));
                    this.cargarDDLMateriasPrimas(materiasPrimasDT);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando materias primas a la lista. " + ex.Message));
            }
        }

        protected void lbtnAgregarMP_Click(object sender, EventArgs e)
        {
            try
            {
                MateriaPrima_Composiciones materiaPrima_Composiciones = new MateriaPrima_Composiciones();
                materiaPrima_Composiciones.Id_Articulo = Convert.ToInt32(idArt);
                materiaPrima_Composiciones.Id_MateriaPrima = Convert.ToInt32(ListMPBusqueda.SelectedValue);
                materiaPrima_Composiciones.Cantidad = Convert.ToDecimal(txtCantidadMP.Text);

                int i = contMateriaPrima.agregarComposicion(materiaPrima_Composiciones);
                if (i > 0)
                {
                    this.cargarComposicionAlPH();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idComposicion = Convert.ToInt32(this.txtMovimiento.Text);
                MateriaPrima_Composiciones materiaPrima_Composiciones = this.contMateriaPrima.obtenerComposicionById(idComposicion);
                string desc = materiaPrima_Composiciones.MateriaPrima.Descripcion;
                int i = contMateriaPrima.eliminarComposicionById(idComposicion);
                if (i > 0)
                {
                    //elimino bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja de composicion materia prima: " + desc);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Materia prima eliminada con exito", null));
                    this.cargarComposicionAlPH();
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
        }
        #endregion

        #region funciones adicionales
        private void cargarDDLMateriasPrimas(DataTable tabla)
        {
            try
            {
                String buscar = this.txtMPBusqueda.Text.Replace(' ', '%');
                DataTable dt = contMateriaPrima.obtenerMateriasPrimasByDescDT(buscar);

                if (tabla != null)
                {
                    dt = tabla;
                }

                //cargo la lista
                this.ListMPBusqueda.DataSource = dt;
                this.ListMPBusqueda.DataValueField = "id";
                this.ListMPBusqueda.DataTextField = "descripcion";
                this.ListMPBusqueda.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarComposicionAlPH()
        {
            try
            {
                decimal totalCosto = 0;
                this.listaComposiciones = this.contMateriaPrima.obtenerComposicionesByArticulo(this.idArt);
                this.phComposiciones.Controls.Clear();
                foreach (var item in this.listaComposiciones)
                {
                    TableRow tr = new TableRow();

                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = item.MateriaPrima.Codigo;
                    celCodigo.Width = Unit.Percentage(3);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celCodigo);

                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = item.MateriaPrima.Descripcion;
                    celDescripcion.Width = Unit.Percentage(10);
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celDescripcion);

                    TableCell celCantidadMP = new TableCell();
                    celCantidadMP.Text = item.Cantidad.ToString();
                    celCantidadMP.Width = Unit.Percentage(3);
                    celCantidadMP.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celCantidadMP);

                    TableCell celUnidad = new TableCell();
                    celUnidad.Text = item.MateriaPrima.UnidadMedida;
                    celUnidad.Width = Unit.Percentage(7);
                    celUnidad.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celUnidad);

                    TableCell celImporte = new TableCell();
                    decimal importe = this.obtenerImporteEnPesos((decimal)item.MateriaPrima.Importe, (int)item.MateriaPrima.Moneda);
                    importe = Math.Round(importe, 2);
                    celImporte.Text = "$ " + importe;
                    celImporte.Width = Unit.Percentage(3);
                    celImporte.VerticalAlign = VerticalAlign.Middle;
                    celImporte.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celImporte);

                    TableCell celTotal = new TableCell();
                    decimal total = (decimal)item.Cantidad * importe;
                    celTotal.Text = "$ " + Math.Round(total, 2);
                    celTotal.Width = Unit.Percentage(5);
                    celTotal.VerticalAlign = VerticalAlign.Middle;
                    celTotal.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celTotal);

                    TableCell celAction = new TableCell();
                    celAction.Width = Unit.Percentage(3);
                    celAction.HorizontalAlign = HorizontalAlign.Center;

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + item.Id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.OnClientClick = "abrirdialog(" + item.Id + ");";
                    celAction.Controls.Add(btnEliminar);

                    tr.Cells.Add(celAction);

                    this.phComposiciones.Controls.Add(tr); //agrego la fila a la tabla

                    totalCosto += (decimal)item.Cantidad * (decimal)importe;
                }
                this.lbCostoTotalComposicion.Text = Math.Round((decimal)totalCosto,2).ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private decimal obtenerImporteEnPesos(decimal importe, int idMoneda)
        {
            if (idMoneda != 1)
            {
                importe = Math.Round(this.contCobranza.obtenerCotizacion(idMoneda) * importe, 2);
            }
            return importe;
        }
        #endregion

        protected void lbtnActualizarCostoComposicion_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo art = this.contArticulo.obtenerArticuloByID(this.idArt);

                art.costo = Math.Round(Convert.ToDecimal(lbCostoTotalComposicion.Text) /
                this.contCobranzaEntity.obtenerCotizacionPorNombreMoneda(art.monedaVenta.moneda), 2);

                Articulo a = this.contArticulo.obtenerPrecioVentaDesdeCosto(art);

                int i = this.contArticulo.modificarArticulo(a, a.codigo, 0);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.updatePanel1, updatePanel1.GetType(), "alert", "$.msgbox(\"Costo recalculado con exito\", {type: \"info\"});", true);
                }
            }
            catch
            {

            }
        }
    }
}