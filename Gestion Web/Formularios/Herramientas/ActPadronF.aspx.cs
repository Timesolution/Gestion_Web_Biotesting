using Disipar.Models;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using System.Threading;
using System.IO;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ActPadronF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        ControladorConfiguracion contConf = new ControladorConfiguracion();
        ControladorIngresosBrutos controladorIngresosBrutos = new ControladorIngresosBrutos();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (!IsPostBack)
                {
                }
                this.cargarUltimaFecha();
                this.cargarUltimaFechaCaba();
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
                        if (s == "68")
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
        private void cargarUltimaFecha()
        {
            try
            {
                string fecha = this.contConf.ObtenerConfiguracionId(2);
                if (fecha != null)
                {
                    this.txtFechaActualizacion.Text = fecha;
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo obtener ultima fecha de actualizacion."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo ultima fecha de actualizacion. " + ex.Message));
            }
        }

        protected void lbtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contCli = new controladorCliente();
                contCli.actualizarPadron();
                this.cargarUltimaFecha();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso Concluido con exito.", null));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando ingresos brutos. " + ex.Message));

            }
        }

        protected void lbtnCargarPadronCABA_Click(object sender, EventArgs e)
        {
            //int size = -1;
            //var sdf = new OpenFileDialog();

            //DialogResult result = sdf.ShowDialog(); // Show the dialog.
            //if (result == DialogResult.OK) // Test result.
            //{
            //    string file = sdf.FileName;
            //    try
            //    {
            //        string text = File.ReadAllText(file);
            //        size = text.Length;
            //    }
            //    catch (Exception ex) { }
            //}

            //string selectedPath = "";

            //Thread t = new Thread((ThreadStart)(() => {
            //    OpenFileDialog saveFileDialog1 = new OpenFileDialog();

            //    saveFileDialog1.Filter = "JSON Files (*.json)|*.json";
            //    saveFileDialog1.FilterIndex = 2;
            //    saveFileDialog1.RestoreDirectory = true;

            //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        selectedPath = saveFileDialog1.FileName;
            //    }
            //}));

            try
            {
                ControladorIngresosBrutos controladorIngresosBrutos = new ControladorIngresosBrutos();
                //var t = new Thread((ThreadStart)(() =>
                //{
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "JSON Files (*.json)|*.json";
                //openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                        return;

                    txtPath.Text = lbtnCargarPadronCABA.FileName;
                    path.Value = openFileDialog.FileName;
                //}));

                //t.SetApartmentState(ApartmentState.STA);
                //t.Start();
                //t.Join();
            }
            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("(CATCH) Error cargando ingresos brutos desde el archivo. " + ex.Message));
            }
        }

        protected void lbtnActualizarCABA_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(path.Value))
                {
                    string[] filelines = File.ReadAllLines(path.Value.ToString());
                    

                    if (controladorIngresosBrutos.LlenarTablaIngresosBrutosCABA(filelines) != -1)
                    {
                        this.cargarUltimaFechaCaba();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "info", m.mensajeBoxInfo("Actualizacion concluida con EXITO.", null));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "error", m.mensajeBoxError("ERROR al actualizar archivo."));
                    }
                    txtPath.Text = string.Empty;
                    path.Value = null;
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "error", m.mensajeBoxError("Debe seleccionar un archivo."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("(CATCH) Error actualizando ingresos brutos desde el archivo. " + ex.Message));
            }
        }

        private void cargarUltimaFechaCaba()
        {
            try
            {
                string fecha = this.contConf.ObtenerConfiguracionId(62);
                if (fecha != null)
                {
                    this.txtFechaActualizacionCABA.Text = fecha;
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo obtener ultima fecha de actualizacion CABA."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo ultima fecha de actualizacion CABA. " + ex.Message));
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            txtPath.Text = "";
            path.Value = null;
        }
    }
}