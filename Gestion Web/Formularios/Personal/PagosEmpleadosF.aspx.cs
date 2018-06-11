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
    public partial class PagosEmpleadosF : System.Web.UI.Page
    {
        controladorCCEmpleado controlador = new controladorCCEmpleado();
        controladorEmpleado contEmpleado = new controladorEmpleado();

        Mensajes m = new Mensajes();
        private int idEmpleado;
        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;        
        private int bn;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.idEmpleado = Convert.ToInt32(Request.QueryString["emp"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["pv"]);
                this.bn = Convert.ToInt32(Request.QueryString["bn"]);
                

                if (!IsPostBack)
                {
                    this.cargarEmpresas();
                    this.cargarEmpleados();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                }
                if (this.idEmpleado > 0)
                {
                    this.cargarMovimientos();
                    this.txtImputar.Enabled = true;
                }
            }
            catch(Exception ex)
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
                        Response.Redirect("../../Default.aspx?m=1", false);
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
                        if (s == "33")
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

        private void cargarEmpresas()
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Empresas. " + ex.Message));
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

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados a la lista. " + ex.Message));
            }
        }

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }        

        private void cargarMovimientos()
        {
            try
            {
                var movimientos = controlador.obtenerMovimientosRemuneracionesEmpleado(this.idEmpleado, this.idEmpresa, this.idSucursal, this.puntoVenta);

                phCobranzas.Controls.Clear();
                decimal saldo = 0;
                foreach (MovimientosCCE m in movimientos)
                {
                    saldo +=(decimal)m.Saldo;
                    this.cargarEnPh(m);
                }
                this.labelSaldo.Text = "$ " +  saldo.ToString("N");
                //this.lblSaldo.Text = "Saldo $ " + saldo.ToString();
                this.cargarLabel();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando movimientos. " + ex.Message));
            }
        }

        private void cargarLabel()
        {
            try
            {
                string label = "";
                if (this.idEmpleado > 0)
                {
                    label += ListEmpleados.Items.FindByValue(idEmpleado.ToString()).Text + ",";
                }
                if (idEmpresa > 0)
                {
                    label += DropListEmpresa.Items.FindByValue(idEmpresa.ToString()).Text + ",";
                }
                //if (idSucursal > 0)
                //{
                //    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                //}
                //if (idPuntoVenta > 0)
                //{
                //    label += DropListPuntoVta.Items.FindByValue(idPuntoVenta.ToString()).Text + ",";
                //}
                //if (idTipo > -1)
                //{
                //    label += DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                //}

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(MovimientosCCE m)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = m.Id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = Convert.ToDateTime(m.Fecha, new CultureInfo("es-AR")).ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = m.Numero;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + m.Debe.ToString().Replace(',', '.');
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + m.Haber.ToString().Replace(',', '.');
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + m.Saldo.ToString().Replace(',', '.');
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.Width = Unit.Percentage(20);
                tr.Cells.Add(celSaldo);

                TableCell celSeleccion = new TableCell();
                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + m.Id;
                cbSeleccion.CssClass = "btn btn-info";
                celSeleccion.Controls.Add(cbSeleccion);
                celSeleccion.Width = Unit.Percentage(5);
                //celSeleccion.VerticalAlign = VerticalAlign.Middle;
                celSeleccion.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celSeleccion);

                phCobranzas.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error agregando movimiento a PH. " + ex.Message));
            }
        }

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PagosEmpleadosF.aspx?emp=" + ListEmpleados.SelectedValue + "&e=" + DropListEmpresa.SelectedValue + "&s=" + DropListSucursal.SelectedValue + "&pv=" + ListPuntoVenta.SelectedValue );
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                //Chequeo si se ingreso dinero a imputar
                if (String.IsNullOrEmpty(this.txtImputar.Text) || this.txtImputar.Text == "0")
                {
                    foreach (Control C in phCobranzas.Controls)
                    {
                        TableRow tr = C as TableRow;
                        CheckBox ch = tr.Cells[5].Controls[0] as CheckBox;
                        if (ch.Checked == true)
                        {
                            idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                        }
                    }
                    if (!String.IsNullOrEmpty(idtildado))
                    {
                        Response.Redirect("PagoRemuneracionABM.aspx?d=" + idtildado + "&emp=" + idEmpleado + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=0&a=1");
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos un movimiento"));
                    }
                }
                else
                {
                    //Consulto si hay documentos para Imputar
                    //string movimientos = this.controlador.obtenerMovimientosImputarPago(Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture), this.idEmpleado, this.idSucursal, this.idEmpresa, this.puntoVenta);
                    //if (!String.IsNullOrEmpty(movimientos))
                    //{                        
                    //    Response.Redirect("PagoRemuneracionABM.aspx?d=" + movimientos + "&emp=" + idEmpleado + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=" + this.txtImputar.Text.Replace(',', '.') + "&a=1");
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El monto ingresado es menor al saldo de los documentos a imputar. "));
                    //}
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error enviando documentos impagos al formulario de Cobros. " + ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
        }

        protected void btnPagoCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                controladorPagos contPago = new controladorPagos();
                controladorDocumentos contDocumentos = new controladorDocumentos();
                controladorSucursal contSucursal = new controladorSucursal();

                PagoRemuneracione pago = new PagoRemuneracione();                
                pago.Empleado = this.idEmpleado;
                pago.Empresa = this.idEmpresa;
                pago.Sucursal = this.idSucursal;
                pago.Total = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.Imputado = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.Ingresado = Convert.ToDecimal(this.txtImputar.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                pago.PuntoVta = this.puntoVenta;
                pago.Fecha = DateTime.Now;

                //int i = contPago.agregarPagoCuentaEmpleado(pago);
                //if (i > 0)
                //{                    
                //    //Response.Redirect("ABMCobros.aspx?documentos=" + i + "&cliente=" + idCliente + "&empresa=" + idEmpresa + "&sucursal=" + idSucursal + "&puntoVenta=" + puntoVenta + "&monto=0&valor=1&tipo=" + this.DropListTipo.SelectedValue);
                Response.Redirect("PagoRemuneracionABM.aspx?bn=" + this.bn + "&d=0&emp=" + idEmpleado + "&e=" + idEmpresa + "&s=" + idSucursal + "&pv=" + puntoVenta + "&m=" + this.txtImputar.Text + "&a=2");
                //}
                //else
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error generando Pago a Cuenta. "));
                //}


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando Pago a Cuenta. " + ex.Message));

            }
        }

        

    }
}