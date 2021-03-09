using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class FacturasSucursalesF : System.Web.UI.Page
    {        
        controladorFacturacion controlador = new controladorFacturacion();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        controladorRemitos contRemito = new controladorRemitos();
        controladorCliente contCliente = new controladorCliente();

        Mensajes m = new Mensajes();
        private int sucOrigen;
        private int sucDestino;
        private string fechaD;
        private string fechaH;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                //datos de filtro
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                sucOrigen = Convert.ToInt32(Request.QueryString["Origen"]);
                sucDestino = Convert.ToInt32(Request.QueryString["Destino"]);


                if (!IsPostBack)
                {                    
                    if (fechaD == null && fechaH == null && sucOrigen == 0 && sucDestino == 0)
                    {                        
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        this.txtFechaDesde.Text = fechaD;
                        this.txtFechaHasta.Text = fechaH;
                        this.DropListSucursalDestino.SelectedValue = this.sucDestino.ToString();
                        this.DropListSucursalOrigen.SelectedValue = this.sucOrigen.ToString();
                    }

                    this.cargarSucursal();
                }

                if (this.sucDestino > 0 || this.sucOrigen > 0)
                {
                    this.cargarFacturasRango(this.fechaD,this.fechaH,this.sucOrigen,this.sucDestino);
                }

                
            }
            catch (Exception ex)
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
                        if (s == "73")//Ventas.Ventas.Sucursales
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador" || perfil == "Control de Stock")
                            {
                                this.DropListSucursalOrigen.Attributes.Remove("disabled");
                                this.DropListSucursalDestino.Attributes.Remove("disabled");
                            }
                            else
                            {
                                if (this.radioDestino.Checked)
                                {
                                    this.DropListSucursalOrigen.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                                else
                                {
                                    this.DropListSucursalDestino.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                            }
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

                DataRow dr1 = dt.NewRow();
                dr1["nombre"] = "Todas";
                dr1["id"] = 0;
                dt.Rows.InsertAt(dr1, 1);


                this.DropListSucursalOrigen.DataSource = dt;
                this.DropListSucursalOrigen.DataValueField = "Id";
                this.DropListSucursalOrigen.DataTextField = "nombre";

                this.DropListSucursalOrigen.DataBind();

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

        private void cargarFacturasRango(string fechaD, string fechaH, int idSucOrigen, int idSucDestino)
        {
            try
            {
                List<Factura> Facturas = this.controlador.obtenerFacturasEntreSucursal(fechaD,fechaH,idSucOrigen,idSucDestino);
                decimal saldo = 0;                
                foreach (Factura f in Facturas)
                {
                    this.cargarEnPh(f);
                    if (f.estado == 1)
                    {
                        saldo += f.total;
                    }
                }

                labelSaldo.Text = "$" + saldo.ToString("N");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando facturas.  " + ex.Message));
            }
        }

        private void cargarEnPh(Factura f)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = f.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = f.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celTipo = new TableCell();
                celTipo.Text = f.tipo.tipo;
                celTipo.HorizontalAlign = HorizontalAlign.Center;
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celNumero = new TableCell();
                celNumero.Text = f.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celSucOrigen = new TableCell();
                celSucOrigen.Text = f.sucursal.nombre;
                celSucOrigen.VerticalAlign = VerticalAlign.Middle;
                celSucOrigen.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucOrigen);

                TableCell celSucDestino = new TableCell();
                celSucDestino.Text = this.DropListSucursalDestino.Items.FindByValue(f.sucursalFacturada.ToString()).Text;
                celSucDestino.VerticalAlign = VerticalAlign.Middle;
                celSucDestino.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucDestino);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + f.total;
                celTotal.VerticalAlign = VerticalAlign.Middle;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTotal);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + f.id + "_" + f.tipo.id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                btnDetalles.Font.Size = 12;                
                btnDetalles.Click += new EventHandler(this.detalleFactura);
                celAccion.Controls.Add(btnDetalles);

                celAccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando facturas. " + ex.Message));
            }
 
        }

        private void detalleFactura(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                
                string[] atributos =  idBoton.Split('_');
                string idFactura = atributos[1];
                int tipo = Convert.ToInt32(atributos[2]);
                TipoDocumento tp = this.contDocumentos.obtenerTipoDoc("Presupuesto");

                if (tipo == tp.id || tipo == 11 || tipo == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    if (tipo == 1 || tipo == 9 || tipo == 4 || tipo == 24 || tipo == 25 || tipo == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                    {
                        //factura
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

                    }
                    else
                    {
                        //facturab
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idFactura + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    }
                    
                }


               

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        
        #region eventos controles
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
            
                if(!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursalOrigen.SelectedValue != "-1" && DropListSucursalDestino.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("FacturasSucursalesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Origen=" + DropListSucursalOrigen.SelectedValue + "&Destino=" + DropListSucursalDestino.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(),"alert",m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                  ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de facturas. " + ex.Message));

            }
        }
        protected void radioOrigen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int suc = (int)Session["Login_SucUser"];

                if (radioOrigen.Checked == true)
                {
                    this.DropListSucursalDestino.SelectedValue = suc.ToString();
                    this.DropListSucursalOrigen.Attributes.Remove("disabled");
                }
            }
            catch
            {

            }
        }
        protected void radioDestino_CheckedChanged(object sender, EventArgs e)
        {
            try
            {          
                int suc = (int)Session["Login_SucUser"];
                if (radioDestino.Checked == true)
                {
                    this.DropListSucursalOrigen.SelectedValue = suc.ToString();
                    this.DropListSucursalDestino.Attributes.Remove("disabled");
                }
            }
            catch
            {

            }
        }
        
        #endregion

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Facturas/ImpresionFacturas.aspx?a=9&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Origen=" + DropListSucursalOrigen.SelectedValue + "&Destino=" + DropListSucursalDestino.SelectedValue + "&e=0', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            //Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=9&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Origen=" + DropListSucursalOrigen.SelectedValue + "&Destino=" + DropListSucursalDestino.SelectedValue + "&e=0");
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Formularios/Facturas/ImpresionFacturas.aspx?a=9&fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Origen=" + DropListSucursalOrigen.SelectedValue + "&Destino=" + DropListSucursalDestino.SelectedValue + "&e=1");
        }
    }
} 