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
    public partial class CajaCierreF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorCajaEntity contCajaCierre = new controladorCajaEntity();
        int suc;
        int ptoVenta;
        String fDesde;
        String fHasta;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    suc = (int)Session["Login_SucUser"];
                    this.ptoVenta = Convert.ToInt32(Request.QueryString["v"]);
                    this.fDesde = Request.QueryString["fd"];
                    this.fHasta = Request.QueryString["fh"];
                    this.cargarSucursal();
                    this.cargarFechas();                    

                    if (suc > 0)
                    {
                        this.cargarPuntoVta(suc);
                        this.DropListSucursal.SelectedValue = suc.ToString();
                        this.DropListSucursal2.SelectedValue = suc.ToString();
                        if (ptoVenta > 0)
                        {
                            this.ListPuntoVenta.SelectedValue = ptoVenta.ToString();
                            this.ListPuntoVenta2.SelectedValue = ptoVenta.ToString();
                            this.cargarCierres();
                        }                        
                        
                        
                    }
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

                string permiso = listPermisos.Where(x => x == "45").FirstOrDefault();

                if (permiso == null)
                {
                    return 0;
                }

                string permiso2 = listPermisos.Where(x => x == "114").FirstOrDefault();

                if (permiso2 != null)
                {
                    this.DropListSucursal.Attributes.Remove("disabled");
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int suc = Convert.ToInt32(this.DropListSucursal2.SelectedValue);
                int ptoVenta = Convert.ToInt32(this.ListPuntoVenta2.SelectedValue);
                if (suc > 0 & ptoVenta > 0)
                {
                    //verifico que el punto de venta no tenga cierre
                    ControladorCaja contCaja = new ControladorCaja();
                    var fecha = contCaja.obtenerUltimaApertura(suc, ptoVenta);
                    //si la fecha de apertura es mas gande q hoy no lo dejo
                    if (DateTime.Now > fecha)
                    {
                        Response.Redirect("CajaCierreABM.aspx?a=1&s=" + suc + "&pv=" + ptoVenta);
                    }
                    else
                    {
                        //ya existe una un cierre para el dia de hoy
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se realizo un cierre de caja en el dia de hoy para este punto de venta. La proxima fecha de apertura es " + fecha.ToString("dd/MM/yyyy")));    
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar sucursal y punto de venta para hacer el cierre. "));    
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error redirijiendo a cierre. " + ex.Message));
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

                //modalbusqueda
                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();              
                //modalAgregar
                this.DropListSucursal2.DataSource = dt;
                this.DropListSucursal2.DataValueField = "Id";
                this.DropListSucursal2.DataTextField = "nombre";

                this.DropListSucursal2.DataBind();

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
            catch(Exception ex){
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

                //modalBusqueda
                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";
                this.ListPuntoVenta.DataBind();
                //modalAgregar
                this.ListPuntoVenta2.DataSource = dt;
                this.ListPuntoVenta2.DataValueField = "Id";
                this.ListPuntoVenta2.DataTextField = "NombreFantasia";
                this.ListPuntoVenta2.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta.SelectedIndex = 1;
                    this.ListPuntoVenta2.SelectedIndex = 1;                    
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarCierres()
        {
            try
            {
                List<Caja_Cierre> listaCierres = new List<Caja_Cierre>();
                listaCierres = this.contCajaCierre.obtenerCierres(Convert.ToInt32(DropListSucursal.SelectedValue), Convert.ToInt32(ListPuntoVenta.SelectedValue), Convert.ToDateTime(txtFechaDesde.Text, new CultureInfo("es-AR")), Convert.ToDateTime(txtFechaHasta.Text, new CultureInfo("es-AR")));

                decimal saldo = 0;
                foreach (var cierre in listaCierres)
                {
                    saldo += cierre.Total.Value;
                    cargarEnPh(cierre);                    
                }

                this.Label1.Text = "$" + saldo.ToString("N");
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cierren en ph. " + ex.Message));
            }
        }

        #endregion

        protected void ListPuntoVenta3_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.cargarCierres();

        }


        private void cargarEnPh(Caja_Cierre c)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = c.Id.ToString();

                //Celdas

                TableCell celFechaCierre = new TableCell();
                //celFechaCierre.Text = c.FechaCierre.Value.ToString("dd/MM/yyyy");
                celFechaCierre.Text = c.FechaCierre.Value.ToString("dd/MM/yyyy hh:mm");
                celFechaCierre.VerticalAlign = VerticalAlign.Middle;
                celFechaCierre.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaCierre);

                TableCell celFechaApertura = new TableCell();
                celFechaApertura.Text = c.FechaApertura.Value.ToString("dd/MM/yyyy");
                celFechaApertura.VerticalAlign = VerticalAlign.Middle;
                celFechaApertura.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaApertura);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + c.Total;
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.OnClientClick = "return false";
                btnDetalles.ID = "btnSelec_" + c.Id.ToString();
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;
                //btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //btnDetalles.Click += new EventHandler();
                celAccion.Controls.Add(btnDetalles);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + c.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.OnClientClick = "return false";
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";          
                btnEliminar.OnClientClick = "abrirdialog(" + c.Id + ");";                
                
                //CheckBox cbSeleccion = new CheckBox();
                ////cbSeleccion.Text = "&nbsp;Imputar";
                //cbSeleccion.ID = "cbSeleccion_" + c.Id;
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;
                //celAccion.Controls.Add(cbSeleccion);

                //Literal l2 = new Literal();
                //l2.Text = "&nbsp";
                //celAccion.Controls.Add(l2);

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
            Response.Redirect("CajaCierreF.aspx?fd="+this.txtFechaDesde.Text+"&fh="+this.txtFechaHasta.Text+"&v="+this.ListPuntoVenta.SelectedValue);
        }

        //nombre representativo??
        protected void btnQuitarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                int sucursal = Convert.ToInt32(this.DropListSucursal.SelectedValue);
                int puntoVta = Convert.ToInt32(this.ListPuntoVenta.SelectedValue);

                long idCierre = Convert.ToInt32(this.txtCierre.Text);

                int i = contCajaCierre.verificarAnular(idCierre, sucursal, puntoVta);

                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cierre anulado con exito. ", "CajaCierreF.aspx"));                    
                }
                if (i == -2)
                {                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error anulando cierre. No puede anular cierre si no es el ultimo mov. realizado."));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error anulando cierre. "));
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error anulando cierre de caja. " + ex.Message));
            }
            
        }

    }
}