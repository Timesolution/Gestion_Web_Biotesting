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

namespace Gestion_Web.Formularios.Valores
{
    public partial class LiquidacionesF : System.Web.UI.Page
    {
        ControladorBanco contBanco = new ControladorBanco();
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        
        private string fechaD;
        private string fechaH;
        private int suc;
        private int emp;
        private int tipoFecha;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["fd"];
                this.fechaH = Request.QueryString["fh"];
                this.suc = Convert.ToInt32(Request.QueryString["suc"]);
                this.emp = Convert.ToInt32(Request.QueryString["emp"]);
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null)
                    {
                        this.suc = (int)Session["Login_SucUser"];
                        this.emp = (int)Session["Login_EmpUser"];
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    this.cargarEmpresas();
                    this.cargarCuentasBcarias();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    this.ListEmpresa.SelectedValue = this.emp.ToString();
                    this.cargarSucursalByEmpresa(this.emp);
                    this.ListSucursal.SelectedValue = this.suc.ToString();
                }

                this.cargarPagosLiquidaciones(fechaD, fechaH, this.suc, this.emp, this.tipoFecha);
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
                        if (s == "104")
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
        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                this.ListEmpresa.DataSource = dt;
                this.ListEmpresa.DataValueField = "Id";
                this.ListEmpresa.DataTextField = "Razon Social";
                this.ListEmpresa.DataBind();

                this.ListEmpresa.Items.Insert(0, new ListItem("Seleccione...","-1"));
                this.ListEmpresa.Items.Insert(1, new ListItem("Todas", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }
        public void cargarSucursalByEmpresa(int idEmpresa)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(idEmpresa);

                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";
                this.ListSucursal.DataBind();

                this.ListSucursal.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListSucursal.Items.Insert(1, new ListItem("Todas", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        private void cargarCuentasBcarias()
        {
            try
            {
                this.ListCuentaBcoCargar.Items.Clear();
                List<Gestion_Api.Entitys.CuentasBancaria> ctas = this.contBanco.obtenerCuentasBancarias();
                foreach (var cta in ctas)
                {
                    string text = cta.Banco1.entidad + " - " + cta.Numero;
                    this.ListCuentaBcoCargar.Items.Add(new ListItem(text, cta.Id.ToString()));
                }

                this.ListCuentaBcoCargar.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarPagosLiquidaciones(string fechaD, string fechaH, int idEmp, int idSuc,int tipoFecha)
        {
            try
            {
                DataTable dtTarjetas = controlador.ObtenerLiquidacionesPagosTarjetasAgrupado(fechaD, fechaH, idEmp, idSuc, tipoFecha);

                decimal saldo = 0;
                foreach (DataRow dr in dtTarjetas.Rows)
                {
                    this.cargarEnPh(dr);
                    saldo += Convert.ToDecimal(dr["importe"]);
                }
                lblSaldo.Text = saldo.ToString("C", new CultureInfo("es-AR"));
                this.cargarLabel(fechaD, fechaH, idEmp ,idSuc);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Tarjetas. " + ex.Message));
            }
        }
        private void cargarEnPh(DataRow dr)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celOperador = new TableCell();
                celOperador.Text = dr["operador"].ToString();
                celOperador.VerticalAlign = VerticalAlign.Middle;
                celOperador.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celOperador);

                TableCell celLiquidacion = new TableCell();
                celLiquidacion.Text = dr["Liquidacion"].ToString();
                celLiquidacion.VerticalAlign = VerticalAlign.Middle;
                celLiquidacion.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celLiquidacion);

                TableCell celImporte = new TableCell();
                celImporte.Text = Convert.ToDecimal(dr["importe"].ToString()).ToString("C");
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                var l = this.controlador.obtenerLiquidacionByOperadorNumero(Convert.ToInt32(dr["id"]), dr["Liquidacion"].ToString());
                decimal importe = 0;
                string fechaLiq = "-";
                string ctaBancaria = "-";

                if (l != null)
                {
                    importe = l.TotalLiquidar.Value;
                    fechaLiq = l.FechaAcreditacion.Value.ToString("dd/MM/yyyy");
                    var cta = this.contBanco.obtenerCuentaBancariaByID(l.IdCuenta.Value);
                    if (cta != null)
                        ctaBancaria = cta.Descripcion + "-" + cta.Numero + "-" + cta.Librador;
                }

                TableCell celImporteLiquidado = new TableCell();
                celImporteLiquidado.Text = importe.ToString("C");
                celImporteLiquidado.VerticalAlign = VerticalAlign.Middle;
                celImporteLiquidado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporteLiquidado);

                TableCell celFechaLiquidado = new TableCell();
                celFechaLiquidado.Text = fechaLiq;
                celFechaLiquidado.VerticalAlign = VerticalAlign.Middle;
                celFechaLiquidado.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celFechaLiquidado);

                TableCell celCtaBancaria = new TableCell();
                celCtaBancaria.Text = ctaBancaria;
                celCtaBancaria.VerticalAlign = VerticalAlign.Middle;
                celCtaBancaria.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celCtaBancaria);

                TableCell celAccion = new TableCell();
                LinkButton lbtnEditar = new LinkButton();
                lbtnEditar.ID = "lbtnEditar_" + dr["Operador"].ToString() + "_" + dr["Liquidacion"].ToString()+ "_" + dr["id"].ToString();
                lbtnEditar.CssClass = "btn btn-info ui-tooltip";
                lbtnEditar.Attributes.Add("data-toggle", "tooltip");
                lbtnEditar.Attributes.Add("title data-original-title", "Cargar Liquidacion");
                lbtnEditar.Click += new EventHandler(this.CargarLiquidacion);
                lbtnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                celAccion.Controls.Add(lbtnEditar);
                tr.Cells.Add(celAccion);

                phLiquidaciones.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Tarjetas en PH. " + ex.Message));
            }

        }
        private void cargarLabel(string fechaD, string fechaH,int idEmpresa ,int idSucursal)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (this.ListEmpresa.Items.FindByValue(idEmpresa.ToString()) != null)
                {
                    label += this.ListEmpresa.Items.FindByValue(idEmpresa.ToString()).Text;
                }                
                if (this.ListSucursal.Items.FindByValue(idSucursal.ToString()) != null)
                {
                    label += this.ListSucursal.Items.FindByValue(idSucursal.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    Response.Redirect("LiquidacionesF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&emp=" + this.ListEmpresa.SelectedValue + "&suc=" + this.ListSucursal.SelectedValue + "&tf=" + this.ListTipoFecha.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos tarjeta. " + ex.Message));

            }
        }
        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.ListEmpresa.SelectedValue);
                this.cargarSucursalByEmpresa(id);
            }
            catch
            {

            }
        }
        private void CargarLiquidacion(object sender, EventArgs e)
        {
            try
            {
                //"lbtnEditar_" + dr["Operador"].ToString() + "_" + dr["Liquidacion"].ToString() + "_" + dr["id"].ToString();
                string [] datos = (sender as LinkButton).ID.Split('_');

                if(datos[2] != "")
                {
                    this.txtOperadorCargar.Text = datos[1];
                    this.txtNroLiquidacionCargar.Text = datos[2];
                    this.lblIdMov.Text = datos[3] + "_" + datos[2];
                    //this.txtTotalLiquidadoCargar.Text = datos[3];
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog();", true);                    
                }
            }
            catch
            {

            }
        }
        private void agregarLiquidacion()
        {
            try
            {
                Liquidacione l = new Liquidacione();
                l.IdCuenta = Convert.ToInt32(this.ListCuentaBcoCargar.SelectedValue);
                l.IdOperador = Convert.ToInt32(this.lblIdMov.Text.Split('_')[0]);
                l.NroLiquidacion = this.lblIdMov.Text.Split('_')[1];
                l.TotalLiquidar = Convert.ToDecimal(this.txtTotalLiquidadoCargar.Text);
                l.FechaAcreditacion = Convert.ToDateTime(this.txtFechaLiquidadoCargar.Text);
                l.Estado = 1;
                int ok = this.controlador.agregarLiquidacion(l);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo guardar. "));
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void modificarLiquidacion()
        {
            try
            {
                string[] datos = this.lblIdMov.Text.Split('_');
                Liquidacione l = this.controlador.obtenerLiquidacionByOperadorNumero(Convert.ToInt32(datos[0]), datos[1]);
                l.IdCuenta = Convert.ToInt32(this.ListCuentaBcoCargar.SelectedValue);
                l.IdOperador = Convert.ToInt32(this.lblIdMov.Text.Split('_')[0]);
                l.NroLiquidacion = this.lblIdMov.Text.Split('_')[1];
                l.TotalLiquidar = Convert.ToDecimal(this.txtTotalLiquidadoCargar.Text);
                l.FechaAcreditacion = Convert.ToDateTime(this.txtFechaLiquidadoCargar.Text);
                l.Estado = 1;
                int ok = this.controlador.modificarLiquidacion(l);
                if (ok >= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo guardar. "));
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbtnCargarLiquidacion_Click(object sender, EventArgs e)
        {
            try
            {
                string [] datos = this.lblIdMov.Text.Split('_');
                var l = this.controlador.obtenerLiquidacionByOperadorNumero(Convert.ToInt32(datos[0]),datos[1]);
                if (l != null)
                {
                    this.modificarLiquidacion();
                }
                else
                {
                    this.agregarLiquidacion();
                }
            }
            catch
            {

            }
        }
    }
}