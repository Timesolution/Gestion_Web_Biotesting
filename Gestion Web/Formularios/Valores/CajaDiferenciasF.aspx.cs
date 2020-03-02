using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CajaDiferenciasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorCajaEntity contCajaCierre = new controladorCajaEntity();
        int suc;
        int ptoVenta;
        String fDesde;
        String fHasta;
        int accion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    suc = Convert.ToInt32(Request.QueryString["s"]);
                    this.ptoVenta = Convert.ToInt32(Request.QueryString["v"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.fDesde = Request.QueryString["fd"];
                    this.fHasta = Request.QueryString["fh"];
                    this.cargarSucursal();
                    this.cargarFechas();

                    if (accion==2)
                    {
                        this.cargarPuntoVta(suc);
                        this.DropListSucursal.SelectedValue = suc.ToString();

                        this.cargarDiferencias();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina. " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Valores.Caja") != 1)
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
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "112")
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
        
        #region carga de datos iniciales
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                //modalbusqueda
                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarFechas()
        {
            try
            {
                if (fDesde != null & fHasta != null)
                {
                    this.txtFechaDesde.Text = this.fDesde;
                    this.txtFechaHasta.Text = this.fHasta;
                }
                else
                {
                    this.txtFechaDesde.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando fecha. " + ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtPtoVenta.Text = this.ListSucursal.SelectedValue;
            cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }
        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["NombreFantasia"] = "TODOS";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                //modalBusqueda
                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";
                this.ListPuntoVenta.DataBind();
                

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta.SelectedIndex = 1;
                    
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarDiferencias()
        {
            try
            {
                List<Caja_Diferencias> diferencias = this.contCajaCierre.obtenerDiferenciasPorSucPtoVenta(this.suc, this.ptoVenta, Convert.ToDateTime(this.fDesde, new CultureInfo("es-AR")), Convert.ToDateTime(this.fHasta, new CultureInfo("es-AR"))); 
                decimal saldo = 0;
                foreach (var d in diferencias)
                {
                    saldo += d.Total.Value;
                    cargarEnPh(d);
                }

                this.Label1.Text = "$" + saldo.ToString("N");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando diferencia en ph. " + ex.Message));
            }
        }

        #endregion



        private void cargarEnPh(Caja_Diferencias d)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = d.Id.ToString();

                //Celdas

                TableCell celFechaCierre = new TableCell();
                celFechaCierre.Text = d.Fecha.Value.ToString("dd/MM/yyyy");
                celFechaCierre.VerticalAlign = VerticalAlign.Middle;
                celFechaCierre.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaCierre);

                TableCell celFechaApertura2 = new TableCell();
                celFechaApertura2.Text = d.sucursale.nombre;;
                celFechaApertura2.VerticalAlign = VerticalAlign.Middle;
                celFechaApertura2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaApertura2);

                TableCell celFechaApertura = new TableCell();
                celFechaApertura.Text = d.PuntoVta.NombreFantasia;
                celFechaApertura.VerticalAlign = VerticalAlign.Middle;
                celFechaApertura.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaApertura);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + d.Total.Value;
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celDif = new TableCell();
                celDif.Text = "$" + d.Diferencia.Value;
                celDif.VerticalAlign = VerticalAlign.Middle;
                celDif.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDif);

                ////agrego fila a tabla
                //TableCell celAccion = new TableCell();
                //LinkButton btnDetalles = new LinkButton();
                //btnDetalles.CssClass = "btn btn-info ui-tooltip";
                //btnDetalles.Attributes.Add("data-toggle", "tooltip");
                //btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                //btnDetalles.ID = "btnSelec_" + c.Id.ToString();
                //btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                //btnDetalles.Font.Size = 12;
                ////btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                ////btnDetalles.Click += new EventHandler(this.detalleFactura);
                //celAccion.Controls.Add(btnDetalles);

                //Literal l3 = new Literal();
                //l3.Text = "&nbsp";
                //celAccion.Controls.Add(l3);

                //LinkButton btnEliminar = new LinkButton();
                //btnEliminar.ID = "btnEliminar_" + d.Id;
                //btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.Attributes.Add("data-toggle", "modal");
                //btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                //btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.OnClientClick = "abrirdialog(" + c.Id + ");";

                ////CheckBox cbSeleccion = new CheckBox();
                //////cbSeleccion.Text = "&nbsp;Imputar";
                ////cbSeleccion.ID = "cbSeleccion_" + c.Id;
                ////cbSeleccion.CssClass = "btn btn-info";
                ////cbSeleccion.Font.Size = 12;
                ////celAccion.Controls.Add(cbSeleccion);

                ////Literal l2 = new Literal();
                ////l2.Text = "&nbsp";
                ////celAccion.Controls.Add(l2);

                //celAccion.Controls.Add(btnEliminar);

                //celAccion.Width = Unit.Percentage(15);
                //celAccion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion);

                this.phDiferencias.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando diferencias. " + ex.Message));
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CajaDiferenciasF.aspx?a=2&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&v=" + this.ListPuntoVenta.SelectedValue + "&s=" + this.DropListSucursal.SelectedValue);
        }

        
    }
}