using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class RetencionesF : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();        
        
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                //datos de filtro
                fechaD = Request.QueryString["fd"];
                fechaH = Request.QueryString["fh"];
                suc = Convert.ToInt32(Request.QueryString["suc"]);

                if (!IsPostBack)
                {
                    this.txtFechaDesde.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    this.cargarSucursal();

                    if (fechaD == null && fechaH == null)
                    {                        
                        suc = (int)Session["Login_SucUser"];
                        this.fechaD = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        //this.cargarRetencionesRango(this.txtFechaDesde.Text, this.txtFechaHasta.Text);
                    }
                    else
                    {
                        this.DropListSucursal.SelectedValue = this.suc.ToString();
                        //this.cargarRetencionesRango(this.fechaD, this.fechaH);
                        this.lblParametros.Text = fechaD + "," + fechaH + "," + this.DropListSucursal.SelectedItem;
                    }
                }
                else
                {
                    if (fechaD == null && fechaH == null)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.fechaD = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Today.ToString("dd/MM/yyyy");
                        //this.cargarRetencionesRango(this.txtFechaDesde.Text, this.txtFechaHasta.Text);
                    }
                }

                this.cargarRetencionesRango(this.fechaD, this.fechaH);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                        if (s == "39")
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

        private void cargarRetencionesRango(string fechaDesde, string fechaHasta)
        {
            try
            {
                DataTable dtReten = this.controlador.obtenerCobrosRetenciones(Convert.ToDateTime(fechaDesde, new CultureInfo("es-AR")).ToString(), Convert.ToDateTime(fechaHasta, new CultureInfo("es-AR")).ToString(), this.suc,"P");
                dtReten.DefaultView.Sort = "fecha";
                dtReten = dtReten.DefaultView.ToTable();

                decimal saldo = 0;
                foreach (DataRow row in dtReten.Rows)
                {
                    this.cargarEnPh(row);
                    saldo += Convert.ToDecimal(row["Retencion"]);
                }
                this.labelSaldo.Text = "$" + saldo.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando PH. " + ex.Message));
            }
        }

        private void cargarEnPh(DataRow dr)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = dr["id"].ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(dr["fecha"], new CultureInfo("es-AR")).ToString("dd/MM/yyyy");             
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                celTipo.Text = dr["Tipo"].ToString();
                celTipo.HorizontalAlign = HorizontalAlign.Center;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);


                TableCell celNumero = new TableCell();
                celNumero.Text = dr["numero"].ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = dr["Cliente"].ToString();
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazon);

                TableCell celRazonSocial = new TableCell();
                celRazonSocial.Text = dr["RazonSocial"].ToString();
                celRazonSocial.VerticalAlign = VerticalAlign.Middle;
                celRazonSocial.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRazonSocial);


                TableCell celNeto = new TableCell();
                celNeto.Text = "$" + dr["Retencion"].ToString();
                celNeto.VerticalAlign = VerticalAlign.Middle;
                celNeto.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNeto);               

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + dr["id"].ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Click += new EventHandler(this.detalleRetencion);
                btnDetalles.Font.Size = 12;

                celAccion.Controls.Add(btnDetalles);
                
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando retenciones en ph. " + ex.Message));
            }
 
        }

        private void detalleRetencion(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero retencion.
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string retencion = atributos[1];

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionRetenciones.aspx?a=1&Retencion=" + retencion + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Retencion. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando detalle retencion desde la interfaz. " + ex.Message);
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RetencionesF.aspx?suc=" + this.DropListSucursal.SelectedValue + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
        }

        protected void btnRetenciones_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionRetenciones.aspx?a=2&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&sdo=" + this.labelSaldo.Text + "','_blank');", true);
        }

        protected void lbtnReporteComprasRetencionesXLS_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImpresionRetenciones.aspx?a=2&fd=" + this.fechaD + "&fh=" + this.fechaH + "&s=" + this.suc + "&sdo=" + this.labelSaldo.Text + "&xls=1");
        }
    }
}