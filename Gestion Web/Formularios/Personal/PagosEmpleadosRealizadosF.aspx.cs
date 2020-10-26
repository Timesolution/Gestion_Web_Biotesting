using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Personal
{
    public partial class PagosEmpleadosRealizadosF : System.Web.UI.Page
    {

        controladorCCEmpleado controlador = new controladorCCEmpleado();
        controladorEmpleado contEmpleado = new controladorEmpleado();
        controladorPagos contPago = new controladorPagos();
        controladorDocumentos contDocumentos = new controladorDocumentos();
        controladorUsuario contUser = new controladorUsuario();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();

        private string fechaD;
        private string fechaH;
        private int idEmpleado;
        private int idEmpresa;
        private int idSucursal;
        //private int puntoVenta;
        //private int idTipo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idEmpleado = Convert.ToInt32(Request.QueryString["emp"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];

                if (!IsPostBack)
                {

                    if (idEmpresa == 0 && idSucursal == 0)
                    {
                        this.idSucursal = (int)Session["Login_SucUser"];
                        this.idEmpresa = (int)Session["Login_EmpUser"];
                        this.fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        this.fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                    }                    
                    this.cargarEmpresas();
                    this.cargarEmpleados();
                    this.DropListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.DropListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                }
                if (this.idEmpleado > 0)
                {
                    this.cargarMovimientos(this.idEmpleado);
                }
                this.Form.DefaultButton = lbBuscar.UniqueID;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Ventas.Cobros Realizados") != 1)
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
                        if (s == "34")
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

        #region cargas iniciales
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListEmpresa.DataSource = dt;
                this.DropListEmpresa.DataValueField = "Id";
                this.DropListEmpresa.DataTextField = "Razon Social";

                this.DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                if (sucu > 0)
                {
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["NombreFantasia"] = "Seleccione...";
                    dr["Id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                }
                else
                {
                    DataRow dr2 = dt.NewRow();
                    dr2["NombreFantasia"] = "Todas";
                    dr2["Id"] = 0;
                    dt.Rows.InsertAt(dr2, 1);
                }


                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";

                this.ListPuntoVenta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.ListPuntoVenta.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
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
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarEmpleados()
        {
            try
            {
                DataTable dt = this.contEmpleado.obtenerEmpleadosDT();
                dt.Columns.Add("empleado");

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        row["empleado"] = row["apellido"].ToString() + ", " + row["nombre"].ToString();
                    }
                }

                dt.DefaultView.Sort = "empleado";

                this.ListEmpleados.DataSource = dt;
                this.ListEmpleados.DataValueField = "id";
                this.ListEmpleados.DataTextField = "empleado";

                this.ListEmpleados.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando empleados a la lista. " + ex.Message));
            }
        }

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }


        
        #endregion

        private void cargarMovimientos(int empleado)
        {
            try
            {
                var mov = this.controlador.obtenerPagosEmpleados(empleado, this.idSucursal);
                this.phCobranzas.Controls.Clear();
                decimal saldo = 0;
                foreach (var pago in mov)
                {
                    string observaciones=contPago.ObtenerObservacionesbyIdPago(pago.Id);
                    this.cargarMovimientoPH(pago, observaciones);
                    saldo += pago.Total.Value;
                }
                this.labelSaldo.Text = saldo.ToString("$0.00");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarMovimientoPH(PagoRemuneracione p,string observaciones)
        {
            try
            {
                TableRow tr = new TableRow();

                tr.ID = p.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(p.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = "Pago Remuneracion Nº " + p.Numero + "-" + observaciones;                
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + p.Total;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldo);

                //agrego fila a tabla
                TableCell celAccion = new TableCell();
                LinkButton btnDetalles = new LinkButton();
                btnDetalles.CssClass = "btn btn-info ui-tooltip";
                btnDetalles.Attributes.Add("data-toggle", "tooltip");
                btnDetalles.Attributes.Add("title data-original-title", "Detalles");
                btnDetalles.ID = "btnSelec_" + p.Id;
                btnDetalles.Text = "<span class='shortcut-icon icon-search'></span>";                
                btnDetalles.Click += new EventHandler(this.detallePago);
                
                celAccion.Controls.Add(btnDetalles);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + p.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";                
                btnEliminar.OnClientClick = "abrirdialog(" + p.Id + ");";                
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                tr.Cells.Add(celAccion);


                this.phCobranzas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.mje.mensajeBoxError("Error cargando movimeinto en PH. " + ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int empleado = Convert.ToInt32(this.ListEmpleados.SelectedValue);
                Response.Redirect("PagosEmpleadosRealizadosF.aspx?emp=" + empleado + "&s=" + this.DropListSucursal.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }

        private void detallePago(object sender, EventArgs e)
        {
            try
            {

                string idBoton = (sender as LinkButton).ID;

                string[] atributos = idBoton.Split('_');
                string idPago = atributos[1];
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "window.open('ReportesR.aspx?a=1&p=" + idPago + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al mostrar detalle de Pago Remuneracion desde la interfaz. " + ex.Message));
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pago Remuneracion desde la interfaz. " + ex.Message);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorPagos contPago = new controladorPagos();

                string id = this.txtMovimiento.Text;
                ////obtengo numero pago
                //string idBoton = (sender as LinkButton).ID;

                //string[] atributos = idBoton.Split('_');
                //string idPago = atributos[1];

                int i = contPago.quitarPagoRemuneracion(Convert.ToInt64(id));
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Pago eliminado con exito!.", "PagosEmpleadosRealizadosF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error tratando de quitar pagos de empleados. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al intentar eliminar pago. " + ex.Message));
            }
        }


     
    }
}