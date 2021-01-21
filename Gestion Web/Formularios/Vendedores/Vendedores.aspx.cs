using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Gestion_Web.Formularios.Vendedores
{
    public partial class Vendedores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorVendedor controlador = new controladorVendedor();
        controladorEmpleado contEmpleado = new controladorEmpleado();
        controladorUsuario contUser = new controladorUsuario();
        public Dictionary<string, string> camposVendedores = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    this.cargarVendedores();
                }

                Page.Form.DefaultButton = this.lbBuscar.UniqueID;
            }
            catch
            {

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
                        if (s == "13")
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

        private void cargarVendedores()
        {
            try
            {
                phVendedores.Controls.Clear();

                List<Vendedor> vendedores = this.controlador.obtenerVendedoresReduc();

                foreach (Vendedor v in vendedores)
                {
                    cargarVendedoresTabla(v);

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando vendedores. " + ex.Message));
            }
        }

        private void cargarVendedoresTabla(Vendedor ven)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                //tr.ID = ven.emp.id.ToString();

                //Celdas
                TableCell celLegajo = new TableCell();
                celLegajo.Text = ven.emp.legajo.ToString();
                celLegajo.Width = Unit.Percentage(20);
                celLegajo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celLegajo);

                TableCell celNombre = new TableCell();
                celNombre.Text = ven.emp.nombre;
                celNombre.Width = Unit.Percentage(30);
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celApellido = new TableCell();
                celApellido.Text = ven.emp.apellido;
                celApellido.Width = Unit.Percentage(30);
                celApellido.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celApellido);

                TableCell celComision = new TableCell();
                celComision.Text = ven.comision.ToString();
                celComision.Width = Unit.Percentage(10);
                celComision.VerticalAlign = VerticalAlign.Middle;
                celComision.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celComision);

                TableCell celAction = new TableCell();
                LinkButton btnDetails = new LinkButton();
                btnDetails.ID = ven.emp.id.ToString();
                btnDetails.CssClass = "btn btn-info";
                btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnDetails.PostBackUrl = "VendedoresABM.aspx?accion=2&codigo=" + ven.emp.legajo.ToString();
                celAction.Controls.Add(btnDetails);


                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + ven.id.ToString();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("title data-original-title", "Borrar Vendedor");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + ven.emp.legajo.ToString() + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                tr.Controls.Add(celAction);

                //arego fila a tabla
                this.phVendedores.Controls.Add(tr);
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados. " + ex.Message));
            }
        }

        private void buscar(string nombre)
        {
            try
            {
                List<Vendedor> Empleados = this.controlador.obtenerVendedoresNombre(nombre);

                foreach (Vendedor v in Empleados)
                {
                    cargarVendedoresTabla(v);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando vendedor. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    this.buscar(this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando vendedor. " + ex.Message));
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionVendedores.aspx?a=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionVendedores.aspx?a=1&ex=1");
            }
            catch
            {

            }
        }

        protected void btnExportartxt_Click(object sender, EventArgs e)
        {
            try
            {
                controladorReportes controladorReportes = new controladorReportes();

                string rutaTxt = Server.MapPath("../ArchivosExportacion/Salida/");

                if (!Directory.Exists(rutaTxt))
                {
                    Directory.CreateDirectory(rutaTxt);
                }

                string archivos = controladorReportes.generarArchivoVendedores(rutaTxt);

                System.IO.FileStream fs = null;
                fs = System.IO.File.Open(archivos, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs.Length];
                fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/octet-stream";
                //this.Response.AddHeader("content-length", comprobante.Length.ToString());
                this.Response.AddHeader("Content-disposition", "attachment; filename= " + archivos);
                this.Response.BinaryWrite(btFile);
                this.Response.End();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Thread was being aborted"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "MensajeArchivoDescargado()", true);
                }
                else
                {
                    Log.EscribirSQL(1, "ERROR", "CATCH: No se pudo generar el archivo.txt con la cuenta corriente .Ubicacion: CuentaCorrienteF.lbtnExportarCuentaCorriente_Click. Excepcion: " + ex.Message);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Disculpe, ha ocurrido un error inesperado. Por favor, contacte con el area de soporte para informarnos sobre este error."));
                }
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                decimal idVendedor = Convert.ToDecimal(this.txtMovimiento.Text);
                Vendedor ven = this.controlador.obtenerVendedorLegajo(idVendedor);
                ven.estado = 0;
                int i = this.controlador.modificarVendedor(ven);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Vendedor : " + ven.emp.nombre + " " + ven.emp.apellido);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vendedor eliminado con exito", "Vendedores.aspx"));
                    this.cargarVendedores();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Vendedor"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Vendedor. " + ex.Message));
            }
        }
    }
}