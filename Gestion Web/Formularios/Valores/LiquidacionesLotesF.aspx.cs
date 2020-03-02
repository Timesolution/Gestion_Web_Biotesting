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
    public partial class LiquidacionesLotesF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorTarjeta contTarjetas = new controladorTarjeta();

        int idLote;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idLote = Convert.ToInt32(Request.QueryString["l"]);                

                if (!IsPostBack)
                {
                    this.obtenerDatosLote();
                }

                if (idLote > 0)
                {
                    this.cargarLiquidaciones();
                }
                
            }
            catch(Exception ex)
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
                        if (s == "51")
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

        private void obtenerDatosLote()
        {
            try
            {
                Tarjetas_Lotes Lote = new Tarjetas_Lotes();
                Lote = this.contTarjetas.ObtenerLotesTarjetasById(this.idLote);

                this.LitNumero.Text = Lote.NumeroLote;
                this.LitImporte.Text = "$" + Lote.Importe;
            }
            catch
            {

            }
        }
        public void cargarLiquidaciones()
        {
            try
            {                
                this.phLiquidaciones.Controls.Clear();


                Tarjetas_Lotes Lote = new Tarjetas_Lotes();
                Lote = this.contTarjetas.ObtenerLotesTarjetasById(this.idLote);

                if (Lote != null)
                {
                    decimal saldo = 0;
                    foreach (var liq in Lote.Tarjetas_Liquidaciones)
                    {
                        saldo += liq.Importe.Value;
                        cargarEnPh(liq);
                    }

                    this.Label1.Text = "$" + saldo.ToString("N");
                }

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cierren en ph. " + ex.Message));
            }
        }
        private void cargarEnPh(Tarjetas_Liquidaciones l)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = l.Id.ToString();

                //Celdas
                TableCell celFechaLote = new TableCell();
                celFechaLote.Text = l.Fecha.Value.ToString("dd/MM/yyyy");
                celFechaLote.VerticalAlign = VerticalAlign.Middle;
                celFechaLote.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaLote);

                TableCell celNumero = new TableCell();
                celNumero.Text = l.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + l.Importe.Value;
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + l.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";          
                btnEliminar.OnClientClick = "abrirdialog(" + l.Id + ");";                                
                celAccion.Controls.Add(btnEliminar);

                celAccion.Width = Unit.Percentage(15);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phLiquidaciones.Controls.Add(tr);                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }
        }                
        protected void btnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = this.txtCierre.Text;

                int i = contTarjetas.EliminarLiquidacionLote(this.idLote,Convert.ToInt32(id));

                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre anulado con exito. ", "LiquidacionesLotesF.aspx?l="+this.idLote));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando liquidacion. "));
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando liquidacion de lote. " + ex.Message));
            }
            
        }

    }
}