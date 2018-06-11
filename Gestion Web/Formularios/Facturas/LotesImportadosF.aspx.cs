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

namespace Gestion_Web.Formularios.Facturas
{
    public partial class LotesImportadosF : System.Web.UI.Page
    {
        controladorTarjeta controlador = new controladorTarjeta();
        controladorImportacion contImp = new controladorImportacion();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private DateTime fechaD;
        private DateTime fechaH;
        private int idCliente;
        private string nombreTarjeta;
        private int estado;
        private int tipoFecha;
        private int operador;
        private int origenPago;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Convert.ToDateTime(Request.QueryString["Fechadesde"], new CultureInfo("es-AR"));
                this.fechaH = Convert.ToDateTime(Request.QueryString["FechaHasta"], new CultureInfo("es-AR"));
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.estado = Convert.ToInt32(Request.QueryString["estado"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 && idCliente == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        
                        //this.cargarClientes();
                        fechaD = DateTime.Now;
                        fechaH = DateTime.Today.AddHours(23).AddMinutes(59);
                        tipoFecha = 1;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        
                        DropListSucursal.SelectedValue = suc.ToString();
                       
                        //DropListClientes.SelectedValue = idCliente.ToString();
                        Response.Redirect("LotesImportadosF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue);

                    }
                    this.cargarSucursal();
                    
                    //this.cargarClientes();
                    txtFechaDesde.Text = fechaD.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = fechaH.ToString("dd/MM/yyyy");
                  
                    //DropListClientes.SelectedValue = idCliente.ToString();
                    DropListSucursal.SelectedValue = suc.ToString();
                    
                }
                this.cargarLotesRango(fechaD, fechaH, estado);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


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
       
        private void cargarLotesRango(DateTime fechaD, DateTime fechaH, int estado)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idCliente == 0)
                {
                    var lotes = this.contImp.FiltrarLotesImportacion(fechaD, fechaH, this.estado);
                    
                    decimal saldo = 0;
                    foreach (var l in lotes)
                    {
                        this.cargarEnPh(l);                        
                    }
                    //lblSaldo.Text = "$" + saldo.ToString();
                    //this.cargarLabel(fechaD, fechaH, idSuc, idCliente);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Tarjetas. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idCliente)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(LotesImportacion l)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(l.Fecha);
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celFecha);

                TableCell celRecibo = new TableCell();
                celRecibo.Text = l.NombreArchivo;
                celRecibo.VerticalAlign = VerticalAlign.Middle;
                celRecibo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRecibo);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = l.Comrobantes.ToString();
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSucursal);

                TableCell celTipo = new TableCell();
                celTipo.Text = l.ComprobantesCorrectos.ToString();
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celTipo);

                TableCell celImporte = new TableCell();
                celImporte.Text = l.ComprobantesIncorrectos.ToString();
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                

                TableCell celLiquidacion = new TableCell();
                if (l.Estado >= 0)
                {
                    celLiquidacion.Text = "CORRECTO";
                }
                else
                {
                    celLiquidacion.Text = "INCORRECTO";
                }

                celLiquidacion.VerticalAlign = VerticalAlign.Middle;
                celLiquidacion.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celLiquidacion);

                TableCell celNumLote = new TableCell();
                celNumLote.Text = l.Resultado;
                celNumLote.VerticalAlign = VerticalAlign.Middle;
                celNumLote.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumLote);

               

                //TableCell celAccion = new TableCell();

                //CheckBox cbSeleccion = new CheckBox();
                ////cbSeleccion.Text = "&nbsp;Imputar";
                //cbSeleccion.ID = "cbSeleccion_" + l.Id;
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;

                //celAccion.Controls.Add(cbSeleccion);
                ////celAccion.Width = Unit.Percentage(5);
                //tr.Cells.Add(celAccion);

                //this.phLotes.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lotes en PH. " + ex.Message));
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("LotesImportadosF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de lotes importados. " + ex.Message));

            }
        }

       

    }
}