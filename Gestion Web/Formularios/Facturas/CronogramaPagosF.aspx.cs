using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CronogramaPagosF : System.Web.UI.Page
    {
        controladorCuentaCorriente controladorCuentaCorriente = new controladorCuentaCorriente();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        int accion;

        private int idCliente;
        private int idEstado;
        private int vendedor;

        int idVendedor;
        string observacion = string.Empty;
        int idArticulo;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                // todo el codigo lo saque de facturas/cotizacionesC
                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idEstado = Convert.ToInt32(Request.QueryString["estado"]);
                vendedor = Convert.ToInt32(Request.QueryString["V"]);
                idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                accion = Convert.ToInt32(Request.QueryString["a"]);

                if (!IsPostBack)
                {

                    this.cargarClientes();

                    if (fechaD == null && fechaH == null)
                    {
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListClientes.SelectedValue = idCliente.ToString();
                }
                
                this.cargarCotizacionesRango(fechaD, fechaH);

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
                int valor = 0;
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "36")
                        {
                            valor = 1;
                        }
                        //Permiso ver saldo
                        if (s == "119")
                            this.lblSaldo.Visible = true;
                    }
                }

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarClientes()
        {
            try
            {
                string perfil = Session["Login_NombrePerfil"] as string;

                controladorCliente contCliente = new controladorCliente();
                ControladorClienteEntity contClienteEnt = new ControladorClienteEntity();

                DataTable dt = new DataTable();
                bool auxiliar = false;

                if (perfil == "Vendedor")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }
                if (perfil == "Cliente")
                {
                    auxiliar = true;
                    dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                    this.btnBuscarCod.Visible = false;
                    this.txtCodCliente.Attributes.Add("disabled", "true");
                    //Oculto el Saldo en caso de que el logueado sea un Cliente
                    //this.phSaldo.Visible = false;
                }
                if (perfil == "Distribuidor")
                {
                    auxiliar = true;
                    dt = contClienteEnt.obtenerReferidosDeCliente(this.idVendedor);
                    DataRow dr = dt.NewRow();
                    Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                    dr["alias"] = "Distribuidor";
                    if (c != null)
                    {
                        dr["alias"] = c.razonSocial;
                    }

                    dr["id"] = this.idVendedor;
                    dt.Rows.InsertAt(dr, 0);
                }
                if (auxiliar == false)
                {
                    dt = contCliente.obtenerClientesDT();
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["alias"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);

                    DataRow dr2 = dt.NewRow();
                    dr2["alias"] = "Todos";
                    dr2["id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        private void cargarCotizacionesRango(string fechaD, string fechaH)
        {
            try
            {

                DataTable dt = this.controladorCuentaCorriente.obtenerPagosProgramados(fechaD, fechaH, idCliente);
                this.cargarCotizacion(dt);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cotizaciones rango. " + ex.Message));
            }
        }

        private void cargarCotizacion(DataTable dtCotizaciones)
        {
            try
            {
                decimal saldo = 0;
                foreach (DataRow row in dtCotizaciones.Rows)
                {
                    saldo += Convert.ToDecimal(row["Importe"]);
                    this.cargarEnPhDR(row);
                }

                lblSaldo.Text = saldo.ToString("C");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Cronograma de pagos. " + ex.Message));

            }
        }

        private void cargarEnPhDR(DataRow p)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = p["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p["Fecha"]).ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celRazon = new TableCell();
                celRazon.Text = p["razonSocial"].ToString();
                celRazon.HorizontalAlign = HorizontalAlign.Left;
                celRazon.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRazon);

                TableCell celFactura = new TableCell();
                celFactura.Text = p["numero"].ToString();
                celFactura.HorizontalAlign = HorizontalAlign.Left;
                celFactura.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFactura);

                TableCell celNumero = new TableCell();
                celNumero.Text = p["Importe"].ToString();
                celNumero.HorizontalAlign = HorizontalAlign.Right;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                phCronogramaPagos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando cronograma de pagos en ph error:. " + ex.Message));
            }

        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {

                    Response.Redirect("CronogramaPagosF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Cliente=" + DropListClientes.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de detallle de cotizaciones. " + ex.Message));

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        //boton buscar en el cliente
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarClienteFiltro();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error Buscando Cliente" + ex.Message + "\", {type: \"error\"});", true);
            }
        }

        public void BuscarClienteFiltro()
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
