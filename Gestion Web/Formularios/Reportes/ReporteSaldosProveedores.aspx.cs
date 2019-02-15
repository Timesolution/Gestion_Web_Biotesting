using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReporteSaldosProveedores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        ControladorCCProveedor controlador = new ControladorCCProveedor();
        controladorCliente contCliente = new controladorCliente();

        int idProveedor;
        int sucursal;
        int tipo;
        int tipoDocumento;
        string fechaHasta;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            this.idProveedor = Convert.ToInt32(Request.QueryString["p"]);
            this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
            this.tipo = Convert.ToInt32(Request.QueryString["t"]);
            this.fechaHasta = Request.QueryString["fh"];
            this.tipoDocumento = Convert.ToInt32(Request.QueryString["td"]);

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(this.fechaHasta))                
                    this.fechaHasta = DateTime.Today.ToString("dd/MM/yyyy");                

                this.txtFechaHasta.Text = this.fechaHasta;

                this.cargarProveedores();
                this.cargarSucursal();
            }

            cargarMovimientos(idProveedor);

            //cargarDatosRango(fechaHasta, idProveedor, sucursal, tipo);
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
                        if (s == "200")
                        {
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
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

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int proveedor = Convert.ToInt32(this.ListProveedor.SelectedValue);
                Response.Redirect("ReporteSaldosProveedores.aspx?&fh=" + this.txtFechaHasta.Text + "&p=" + proveedor + "&s=" + this.DropListSucursal.SelectedValue + "&t=" + this.DropListTipo.SelectedValue + "&td=" + this.DropListTipo.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);

                //cargo la lista
                this.ListProveedor.DataSource = dtClientes;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";
                this.ListProveedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 1);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
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
                dr["nombre"] = "Todas";
                dr["id"] = 0;
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

        private void cargarMovimientos(int proveedor)
        {
            try
            {
                DateTime fdesde = Convert.ToDateTime(new DateTime(2000,01,01), new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                var mov = this.controlador.obtenerSaldosProveedor(fhasta,proveedor, this.sucursal, this.tipoDocumento);

                this.phTopProveedores.Controls.Clear();
                decimal saldoAcumulado = 0;

                foreach (DataRow dr in mov.Rows)
                {
                    this.cargarMovimientoPH(dr["razonsocial"].ToString(), Convert.ToDecimal(dr["Saldo"].ToString()));

                    saldoAcumulado += Convert.ToDecimal(dr["Saldo"].ToString());
                }
                
                this.lblSaldosPesos.Text = "$ " + saldoAcumulado.ToString("N");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarMovimientoPH(string proveedor, decimal saldoAcumulado)
        {
            try
            {
                TableRow tr = new TableRow();

                //Celdas
                TableCell celProveedor = new TableCell();
                celProveedor.Text = proveedor;
                celProveedor.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celProveedor);

                TableCell celSaldoAcumulado = new TableCell();
                celSaldoAcumulado.Text = "$ " + saldoAcumulado;
                celSaldoAcumulado.VerticalAlign = VerticalAlign.Middle;
                celSaldoAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldoAcumulado);

                this.phTopProveedores.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error cargando movimiento en PH. " + ex.Message));
            }
        }
    }
    
}