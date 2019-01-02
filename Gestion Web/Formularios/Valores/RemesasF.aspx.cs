using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
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
    public partial class RemesasF : System.Web.UI.Page
    {
        ControladorRemesaEntity contRemesaEnt = new ControladorRemesaEntity();
        controladorSucursal contSucu = new controladorSucursal();

        private string fechaD;
        private string fechaH;
        private int sucursalOrigen;
        private int sucursalDestino;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            this.fechaD = Request.QueryString["fd"];
            this.fechaH = Request.QueryString["fh"];
            this.sucursalOrigen = Convert.ToInt32(Request.QueryString["so"]);
            this.sucursalDestino = Convert.ToInt32(Request.QueryString["sd"]);

            if (!IsPostBack)
            {
                if (fechaD == null && fechaH == null)
                {
                    fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                    fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                this.CargarSucursales();

                this.txtFechaDesde.Text = fechaD;
                this.txtFechaHasta.Text = fechaH;
                ObtenerYCargarRemesas();
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
                    //if (!String.IsNullOrEmpty(s))
                    //{
                    //    if (s == "44")
                    //    {
                    //        return 1;
                    //    }
                    //}
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public void CargarSucursales()
        {
            try
            {               
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListSucDestino.DataSource = dt;
                this.ListSucDestino.DataValueField = "id";
                this.ListSucDestino.DataTextField = "nombre";
                this.ListSucDestino.DataBind();

                this.ListSucOrigen.DataSource = dt;
                this.ListSucOrigen.DataValueField = "id";
                this.ListSucOrigen.DataTextField = "nombre";
                this.ListSucOrigen.DataBind();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar las sucursales en drop down list " + ex.Message);
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("RemesasF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&so=" + this.ListSucOrigen.SelectedValue + "&sd=" + this.ListSucDestino.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al buscar por filtro " + ex.Message);
            }
        }

        public void ObtenerYCargarRemesas()
        {
            try
            {
                DateTime desde = Convert.ToDateTime(this.fechaD, new CultureInfo("es-AR"));
                DateTime hasta = Convert.ToDateTime(this.fechaH, new CultureInfo("es-AR"));

                var remesas = contRemesaEnt.ObtenerRemesaPorFiltro(desde,hasta,sucursalOrigen,sucursalDestino);

                foreach (var remesa in remesas)
                {
                    CargarRemesasEnPH(remesa);
                }
                
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al obtener y cargar remesas " + ex.Message);
            }
        }

        public void CargarRemesasEnPH(Remesa remesa)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = remesa.Id.ToString();

                TableCell celFecha = new TableCell();
                celFecha.Text = remesa.Fecha.Value.ToString("dd/MM/yyyy");
                tr.Controls.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = remesa.NumeroRemesa.Value.ToString("D8");
                celNumero.HorizontalAlign = HorizontalAlign.Right;
                tr.Controls.Add(celNumero);

                TableCell celSucOrigen = new TableCell();
                celSucOrigen.Text = contSucu.obtenerSucursalID((int)remesa.SucursalOrigen).nombre;
                celSucOrigen.HorizontalAlign = HorizontalAlign.Right;
                tr.Controls.Add(celSucOrigen);

                TableCell celSucDestino = new TableCell();
                celSucDestino.Text = contSucu.obtenerSucursalID((int)remesa.SucursalDestino).nombre;
                celSucDestino.HorizontalAlign = HorizontalAlign.Right;
                tr.Controls.Add(celSucDestino);

                TableCell celAccion = new TableCell();

                Literal lReport = new Literal();
                lReport.ID = "btnReporte_" + remesa.Id.ToString();
                lReport.Text = "<a href=\"ImpresionValores.aspx?a=11&remesa=" + remesa.Id.ToString() + "\"" + "target =\"_blank\"" + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lReport.Text += "<span class=\"shortcut-icon icon-search\"></span>";
                lReport.Text += "</a>";

                celAccion.Controls.Add(lReport);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celAccion.Controls.Add(l3);

                CheckBox cbSeleccion = new CheckBox();
                cbSeleccion.ID = "cbSeleccion_" + remesa.Id;
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;
                celAccion.Controls.Add(cbSeleccion);

                tr.Cells.Add(celAccion);

                tr.Controls.Add(celAccion);

                this.phRemesas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar remesa en ph " + ex.Message);
            }
        }

        protected void lbtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control C in phRemesas.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[4].Controls[2] as CheckBox;

                    string id = tr.ID;

                    if (ch.Checked == true)
                    {
                        contRemesaEnt.EliminarRemesa(Convert.ToInt32(id));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al eliminar remesa " + ex.Message);
            }
        }
    }
}