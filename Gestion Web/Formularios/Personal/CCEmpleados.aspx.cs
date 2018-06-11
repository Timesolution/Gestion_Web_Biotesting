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

namespace Gestion_Web.Formularios.Personal
{
    public partial class CCEmpleados : System.Web.UI.Page
    {
        //mensajes popUp
        Mensajes m = new Mensajes();

        controladorCCEmpleado controlador = new controladorCCEmpleado();
        controladorEmpleado contEmpleado = new controladorEmpleado();

        int idEmpleado;
        int sucursal;
        int tipo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idEmpleado = Convert.ToInt32(Request.QueryString["emp"]);
                this.sucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.tipo = Convert.ToInt32(Request.QueryString["t"]);
                if (!IsPostBack)
                {
                    this.cargarEmpleados();
                    this.cargarSucursal();
                }
                if (idEmpleado > 0)
                {
                    lblParametros.Text = "Empleado: " + this.contEmpleado.obtenerEmpleadoID(idEmpleado).apellido;
                    this.cargarMovimientos(idEmpleado);
                }
                else
                {
                    lblParametros.Text = "Seleccione Empleado.";
                }
            }
            catch (Exception ex)
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Articulos.Articulos") != 1)
                    if (this.verificarAcceso() != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                    else
                    {
                       
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
                        if (s == "32")
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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

                this.ListEmpleado.DataSource = dt;
                this.ListEmpleado.DataValueField = "id";
                this.ListEmpleado.DataTextField = "empleado";

                this.ListEmpleado.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados a la lista. " + ex.Message));
            }
        }

        private void cargarMovimientos(int empleado)
        {
            try
            {
                var mov = this.controlador.obtenerMovimientosEmpleado(empleado, this.sucursal);
                this.phCuentaCorriente.Controls.Clear();
                decimal saldoAcumulado = 0;
                foreach (var m in mov)
                {
                    if (Math.Abs(m.Debe.Value) > 0)
                    {
                        saldoAcumulado += (decimal)m.Debe;
                    }
                    if (Math.Abs(m.Haber.Value) > 0)
                    {
                        saldoAcumulado -= (decimal)m.Haber;
                    }
                    this.cargarMovimientoPH(m, saldoAcumulado);
                }
                this.lblSaldo.Text = "$ " + saldoAcumulado.ToString("N");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarMovimientoPH(MovimientosCCE m, decimal saldoAcumulado)
        {
            try
            {
                TableRow tr = new TableRow();

                tr.ID = m.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(m.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                if (m.TipoDocumento.Value == 28)
                {
                    celNumero.Text = "Remuneracion Nº " + m.Numero;
                }
                if (m.TipoDocumento.Value == 29)
                {
                    celNumero.Text = "Pago Remuneracion Nº " + m.Numero;
                }
                if (m.TipoDocumento.Value == 30)
                {
                    celNumero.Text = "Pago a Cuenta Nº " + m.Numero;
                }
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + m.Debe;
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + m.Haber;
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + m.Saldo;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldo);

                TableCell celSaldoAcumulado = new TableCell();
                celSaldoAcumulado.Text = "$" + saldoAcumulado;
                celSaldoAcumulado.VerticalAlign = VerticalAlign.Middle;
                celSaldoAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldoAcumulado);

                this.phCuentaCorriente.Controls.Add(tr);

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error cargando movimeinto en PH. " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int empleado = Convert.ToInt32(this.ListEmpleado.SelectedValue);
                Response.Redirect("CCEmpleados.aspx?emp=" + empleado + "&s=" + this.DropListSucursal.SelectedValue );
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
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "window.open('ReportesR.aspx?a=1&p=" + idPago + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');",true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Pago Remuneracion desde la interfaz. " + ex.Message));
                Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Pago Remuneracion desde la interfaz. " + ex.Message);
            }
        }
        private void detalleRemuneracion(object sender, EventArgs e)
        {
            try
            {
                
                //string idBoton = (sender as LinkButton).ID;

                //string[] atributos = idBoton.Split('_');
                //string idRemun = atributos[1];
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "window.open('ReportesR.aspx?r=" + idRemun + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de Remuneracion desde la interfaz. " + ex.Message));
                //Gestion_Api.Modelo.Log.EscribirSQL(1, "ERROR", "Error al mostrar detalle de Remuneracion desde la interfaz. " + ex.Message);
            }
        }
    


    }
}