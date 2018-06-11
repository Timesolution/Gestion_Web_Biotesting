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
    public partial class LotesTarjetasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorTarjeta contTarjetas = new controladorTarjeta();
        int posnet;
        int ptoVenta;
        int tarjeta;
        String fDesde;
        String fHasta;
        string cero;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                
                this.tarjeta = Convert.ToInt32(Request.QueryString["t"]);
                this.fDesde = Request.QueryString["fd"];
                this.fHasta = Request.QueryString["fh"];
                this.cero = Request.QueryString["cero"];
                this.posnet = Convert.ToInt32(Request.QueryString["p"]);

                if (!IsPostBack)
                {
                    this.cargarPosnets();
                    this.cargarFechas();                    
                    this.DropListPosnet.SelectedValue = posnet.ToString();
                    this.cargarTarjetas();
                    this.ListTarjetas.SelectedValue = tarjeta.ToString();
                    this.txtFechaLiquidacion.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }

                this.cargarCierres();
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

        #region carga de datos iniciales
        public void cargarPosnets()
        {
            try
            {
                List<Posnet> lstPosnets = this.contTarjetas.ObtenerPosnets();                               

                //modalbusqueda
                this.DropListPosnet.DataSource = lstPosnets;
                this.DropListPosnet.DataValueField = "Id";
                this.DropListPosnet.DataTextField = "Nombre";
                this.DropListPosnet.DataBind();
                //agrego Seleccione
                ListItem item = new ListItem("Seleccione...", "-1");
                this.DropListPosnet.Items.Insert(0, item);
                ListItem item2 = new ListItem("Todos", "0");
                this.DropListPosnet.Items.Insert(1, item2);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando posnets. " + ex.Message));
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
            catch(Exception ex){
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando fecha. " + ex.Message));
            }
        }
        public void cargarTarjetas()
        {
            try
            {
                this.ListTarjetas.Items.Clear();
                if (Convert.ToInt32(this.DropListPosnet.SelectedValue) > 0)
                {
                    Posnet p = this.contTarjetas.ObtenerPosnetByID(Convert.ToInt32(this.DropListPosnet.SelectedValue));

                    this.ListTarjetas.DataSource = p.Posnet_Tarjetas;
                    this.ListTarjetas.DataValueField = "Id";
                    this.ListTarjetas.DataTextField = "Nombre";

                    this.ListTarjetas.DataBind();

                    //agrego Seleccione
                    ListItem item = new ListItem("Seleccione...", "-1");
                    this.ListTarjetas.Items.Insert(0, item);
                    //todas
                    ListItem item2 = new ListItem("Todas", "0");
                    this.ListTarjetas.Items.Insert(1, item2);
                }
                else
                {
                    ListItem item2 = new ListItem("Todas", "0");
                    this.ListTarjetas.Items.Insert(0, item2);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tarjetas. " + ex.Message));
            }
        }
        #endregion
        public void cargarCierres()
        {
            try
            {                
                this.phCierres.Controls.Clear();

                
                List<Tarjetas_Lotes> listaLotes = new List<Tarjetas_Lotes>();
                listaLotes = this.contTarjetas.ObtenerLotesTarjetas(Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR")), Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR")), Convert.ToInt32(DropListPosnet.SelectedValue), Convert.ToInt32(ListTarjetas.SelectedValue));

                decimal saldo = 0;
                foreach (var cierre in listaLotes)
                {
                    if (this.cero == "1")
                    {
                        saldo += cierre.Importe.Value;
                        cargarEnPh(cierre);
                    }
                    else
                    {
                        if (cierre.Importe > 0)
                        {
                            saldo += cierre.Importe.Value;
                            cargarEnPh(cierre);
                        }
                    }
                    
                }

                this.Label1.Text = "$" + saldo.ToString("N");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cierren en ph. " + ex.Message));
            }
        }
        private void cargarEnPh(Tarjetas_Lotes t)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = t.Id.ToString();

                //Celdas
                TableCell celFechaCierre = new TableCell();
                celFechaCierre.Text = t.FechaCierre.Value.ToString("dd/MM/yyyy");
                celFechaCierre.VerticalAlign = VerticalAlign.Middle;
                celFechaCierre.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaCierre);

                Posnet p = this.contTarjetas.ObtenerPosnetByID(t.IdPosnet.Value);

                TableCell celPosnet = new TableCell();
                celPosnet.Text = p.Nombre;
                celPosnet.VerticalAlign = VerticalAlign.Middle;
                celPosnet.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celPosnet);

                TableCell celTarjeta = new TableCell();
                celTarjeta.Text = p.Posnet_Tarjetas.Where(x => x.Id == t.IdTarjetaPosnet.Value).FirstOrDefault().Nombre;
                celTarjeta.VerticalAlign = VerticalAlign.Middle;
                celTarjeta.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTarjeta);

                TableCell celNumero = new TableCell();
                celNumero.Text = t.NumeroLote;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celCupones = new TableCell();
                celCupones.Text = t.Cupones.Value.ToString();
                celCupones.VerticalAlign = VerticalAlign.Middle;
                celCupones.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCupones);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + t.Importe.Value;
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);                

                TableCell celLiquidacion = new TableCell();
                celLiquidacion.Text = "$" + t.Tarjetas_Liquidaciones.Sum(x => x.Importe).Value;
                celLiquidacion.VerticalAlign = VerticalAlign.Middle;
                celLiquidacion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celLiquidacion);

                TableCell celResta = new TableCell();
                celResta.Text = "$" + (t.Importe.Value - t.Tarjetas_Liquidaciones.Sum(x => x.Importe).Value);
                celResta.VerticalAlign = VerticalAlign.Middle;
                celResta.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celResta);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + t.Id;
                btnEditar.CssClass = "btn btn-info";
                btnEditar.PostBackUrl = "LiquidacionesLotesF.aspx?l=" + t.Id.ToString();
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";                
                celAccion.Controls.Add(btnEditar);

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAccion.Controls.Add(l1);

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + t.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + t.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";          
                btnEliminar.OnClientClick = "abrirdialog(" + t.Id + ");";                                
                celAccion.Controls.Add(btnEliminar);

                celAccion.Width = Unit.Percentage(15);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phCierres.Controls.Add(tr);                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }
        }
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("LotesTarjetasF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&p=" + this.DropListPosnet.SelectedValue + "&t=" + this.ListTarjetas.SelectedValue + "&cero=" + Convert.ToInt32(this.chkImporteCero.Checked));
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("LotesTarjetasABM.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error redirijiendo a cierre. " + ex.Message));
            }
        }   
        protected void btnQuitarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                string id = this.txtCierre.Text;

                int i = contTarjetas.EliminarCierreLote(Convert.ToInt32(id));

                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre anulado con exito. ", "LotesTarjetasF.aspx"));
                }
                else
                {
                    if (i == -1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando cierre. "));
                    }
                    if (i == -2)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se puede anular lote con liquidaciones ingresadas. "));
                    }
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando cierre de lote. " + ex.Message));
            }
            
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                string liquidacion = this.txtNumeroLiquidacion.Text;
                string fechaLiq = this.txtFechaLiquidacion.Text;
                decimal importe = Convert.ToDecimal(this.txtImporteLiquidar.Text);
                int forma = 0;


                if (this.rbtnTotal.Checked)
                {
                    forma = 1;
                }

                if (this.txtImporteLiquidar.Text == "" && forma == 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe ingresar un importe a liquidar. "));
                    return;
                }
                if (this.rbtnImporte.Checked == false && this.rbtnTotal.Checked == false)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar una forma de liquidar. "));
                    return;
                }
                
                foreach (Control c in phCierres.Controls)
                {
                    TableRow tr = c as TableRow;
                    CheckBox ch = tr.Cells[8].Controls[2] as CheckBox;
                    if (ch.Checked == true )
                    {
                        idtildado += ch.ID.Split('_')[1] + ";";
                    }
                }
                if (idtildado != "")
                {
                    int i = 0;
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")
                        {
                            int idLote = Convert.ToInt32(id);

                            i = this.contTarjetas.LiquidarLotesTarjeta(idLote, liquidacion, importe, fechaLiq,forma);
                            if (i < 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Una o mas liquidaciones no se agregaron debido a un problema. "));
                            }
                        }

                    }                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Seleccione al menos un cierre a liquidar. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error liquidando pagos con tarjeta. " + ex.Message));
            }
        }
        
        protected void DropListPosnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarTarjetas();
        }

        protected void rbtnTotal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnTotal.Checked)
                {
                    this.txtImporteLiquidar.Text = "0";
                }
            }
            catch
            {

            }
        }

        protected void rbtnImporte_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnImporte.Checked)
                {
                    this.txtImporteLiquidar.Attributes.Remove("disabled");
                }
            }
            catch
            {

            }
        }

        

    }
}