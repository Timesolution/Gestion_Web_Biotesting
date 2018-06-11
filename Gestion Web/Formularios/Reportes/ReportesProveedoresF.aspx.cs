using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
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
    public partial class ReportesProveedoresF : System.Web.UI.Page
    {
        ControladorCCProveedor controlador = new ControladorCCProveedor();
        controladorSucursal contSucu = new controladorSucursal();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        controladorCuentaCorriente contrCC = new controladorCuentaCorriente();
        Mensajes m = new Mensajes();
                
        private string fechaH;
        private int idProveedor;        
        private int idEmpresa;
        private int idSucursal;
        private int tipo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaH = Request.QueryString["FechaHasta"];
                idProveedor = Convert.ToInt32(Request.QueryString["Prov"]);                
                tipo = Convert.ToInt32(Request.QueryString["tipo"]);
                idSucursal = Convert.ToInt32(Request.QueryString["suc"]);                

                if (!IsPostBack)
                {
                    if (fechaH == null && idSucursal == 0)
                    {
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        idSucursal = (int)Session["Login_SucUser"];
                        idEmpresa = (int)Session["Login_EmpUser"];
                    }

                    this.cargarSucursal();
                    this.cargarProveedores();

                    this.txtFechaHasta.Text = fechaH;

                    DropListClientes.SelectedValue = idProveedor.ToString();                    
                    DropListTipo.SelectedValue = tipo.ToString();

                }

                this.cargarDatosRango(fechaH,idProveedor,idSucursal,tipo);                
                    
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        #region carga inicial
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
                        if (s == "64")
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

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

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

                this.DropListSucursal.SelectedValue = this.idSucursal.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        #endregion
        private void cargarDatosRango(string hasta, int proveedor,int sucursal, int tipo)
        {
            try
            {
                if (hasta != null && proveedor >= 0 && sucursal >= 0)
                {
                    DataTable dtImpagas = this.controlador.obtenerMovimientosProveedorRango(hasta, proveedor, sucursal, tipo);
                    Decimal saldo = 0;

                    if (dtImpagas.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtImpagas.Rows)
                        {
                            saldo += Convert.ToDecimal(row["Saldo"]);
                            this.cargarTopClientesCantidadTable(row);
                        }
                    }

                    if (Math.Abs(saldo) == (decimal)0.01)
                        saldo = 0;

                    this.lblDocumentosImpagosPesos.Text = saldo.ToString("C");

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando movimientos impagas proveedores."));
            }
        }

        private void cargarTopClientesCantidadTable(DataRow dr)
        {
            try
            {
                if (Convert.ToDecimal(dr["Saldo"]) > 0)
                {
                    TableRow tr = new TableRow();

                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = dr["Proveedor"].ToString();
                    celCodigo.VerticalAlign = VerticalAlign.Bottom;
                    celCodigo.HorizontalAlign = HorizontalAlign.Left;
                    celCodigo.Width = Unit.Percentage(65);
                    tr.Cells.Add(celCodigo);

                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = Convert.ToDecimal(dr["Saldo"]).ToString("C");
                    celCantidad.Width = Unit.Percentage(30);
                    celCantidad.VerticalAlign = VerticalAlign.Bottom;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celCantidad);

                    TableCell celPuntoVta = new TableCell();

                    LinkButton btnEditar = new LinkButton();
                    btnEditar.ID = dr["id"].ToString();
                    btnEditar.CssClass = "btn btn-info ui-tooltip";
                    btnEditar.Attributes.Add("data-toggle", "tooltip");
                    btnEditar.Attributes.Add("title data-original-title", "Editar");
                    btnEditar.Text = "<span class='shortcut-icon icon-search'></span>";
                    btnEditar.Font.Size = 9;                    
                    int PuntoVenta = this.contSucu.obtenerPrimerPuntoVenta(Convert.ToInt32(this.DropListSucursal.SelectedValue), idEmpresa);
                    btnEditar.PostBackUrl = "../../Formularios/Compras/CCCompraF.aspx?p=" + dr["id"].ToString() + "&s=" + Convert.ToInt32(this.DropListSucursal.SelectedValue) + "&t=" + DropListTipo.SelectedValue;
                    celPuntoVta.Controls.Add(btnEditar);
                    celPuntoVta.Width = Unit.Percentage(5);
                    tr.Cells.Add(celPuntoVta);


                    phTopProveedores.Controls.Add(tr);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes. " + ex.Message));

            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                    Response.Redirect("ReportesProveedoresF.aspx?fechaHasta=" + txtFechaHasta.Text + "&suc=" + this.DropListSucursal.SelectedValue + "&Prov=" + DropListClientes.SelectedValue + "&tipo=" + DropListTipo.SelectedValue);

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar fecha"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de impagas. " + ex.Message));

            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClientes = this.contCliente.obtenerProveedorNombreDT(this.txtCodCliente.Text);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
                //this.cargarClientesTable(cliente);

                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "razonSocial";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('../../Formularios/Compras/ImpresionCompras.aspx?a=4&fh=" + txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&prov=" + DropListClientes.SelectedValue + "&t=" + DropListTipo.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            //Response.Redirect("../../Formularios/Compras/ImpresionCompras.aspx?a=4&fh=" + txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&prov=" + DropListClientes.SelectedValue + "&t=" + DropListTipo.SelectedValue);
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../Formularios/Compras/ImpresionCompras.aspx?a=4&ex=1&fh=" + txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&prov=" + DropListClientes.SelectedValue + "&t=" + DropListTipo.SelectedValue);
        }
    }
}