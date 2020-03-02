using Disipar.Models;
using Gestion_Api.Controladores;
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
    public partial class RetencionesF : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int idTipo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idTipo = Convert.ToInt32(Request.QueryString["Tipo"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 && idTipo == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarTiposRetencion();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListTipos.SelectedValue = idTipo.ToString();

                    }
                    this.cargarSucursal();
                    this.cargarTiposRetencion();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListTipos.SelectedValue = idTipo.ToString();
                    DropListSucursal.SelectedValue = suc.ToString();
                }
                this.cargarRetencionRango(fechaD, fechaH, suc, idTipo);
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
                        if (s == "50")
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

        public void cargarTiposRetencion()
        {
            try
            {
                DataTable dt = controlador.obtenerTiposRetencionDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["tipo"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["tipo"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListTipos.DataSource = dt;
                this.DropListTipos.DataValueField = "id";
                this.DropListTipos.DataTextField = "tipo";
                this.DropListTipos.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tipos de Retencion a la lista. " + ex.Message));
            }
        }

        private void cargarRetencionRango(string fechaD, string fechaH, int idSuc, int idCliente)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idCliente == 0)
                {

                    DataTable dtRetenciones = controlador.obtenerDatosRetencion(fechaD, fechaH, idSuc, idCliente);

                    decimal saldo = 0;
                    foreach (DataRow dr in dtRetenciones.Rows)
                    {
                        this.cargarEnPh(dr);
                        saldo += Convert.ToDecimal(dr["Retencion"]);
                    }
                    lblSaldo.Text = "$" + saldo.ToString("0.00");
                    this.cargarLabel(fechaD, fechaH, idSuc, idTipo);


                }
                else
                {

                    DataTable dtRetenciones = controlador.obtenerDatosRetencion(fechaD, fechaH, idSuc, idCliente);

                    decimal saldo = 0;
                    foreach (DataRow dr in dtRetenciones.Rows)
                    {
                        this.cargarEnPh(dr);
                        saldo += Convert.ToDecimal(dr["Retencion"]);
                    }
                    lblSaldo.Text = "$" + saldo.ToString("0.00");
                    this.cargarLabel(fechaD, fechaH, idSuc, idTipo);

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Retenciones. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idTipo)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idTipo > -1)
                {
                    label += DropListTipos.Items.FindByValue(idTipo.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(DataRow dr)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(dr["fecha"], new CultureInfo("es-AR"));
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celRecibo = new TableCell();
                celRecibo.Text = dr["razonSocial"].ToString();
                celRecibo.VerticalAlign = VerticalAlign.Middle;
                celRecibo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRecibo);

                TableCell celCuit = new TableCell();
                celCuit.Text = dr["cuit"].ToString();
                celCuit.VerticalAlign = VerticalAlign.Middle;
                celCuit.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCuit);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$ " + dr["Retencion"].ToString().Replace(',', '.');
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celTipo = new TableCell();
                celTipo.Text = dr["tipo"].ToString();
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                phRetenciones.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Retenciones en PH. " + ex.Message));
            }

        }

        protected void UpdatePanel3_Load(object sender, EventArgs e)
        {
            //
            this.labelIva.Text = "testc escribo desde el update panel amigo!!";


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("RetencionesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Tipo=" + DropListTipos.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos de caja. " + ex.Message));

            }
        }
    }
}