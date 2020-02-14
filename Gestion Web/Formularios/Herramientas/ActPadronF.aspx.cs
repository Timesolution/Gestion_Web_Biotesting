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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ActPadronF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        ControladorConfiguracion contConf = new ControladorConfiguracion();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (!IsPostBack)
                {
                }
                    this.cargarUltimaFecha();
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
            catch(Exception ex)
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso Concluido con exito.",null));
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando ingresos brutos. " + ex.Message));
               
            }
        }

        protected void lbtnCargarPadronCABA_Click(object sender, EventArgs e)
        {
            try
            {
                var t = new Thread((ThreadStart)(() => {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.ShowNewFolderButton = true;
                    if (fbd.ShowDialog() == DialogResult.Cancel)
                        return;

                    txtFechaActualizacionCABA.Text = fbd.SelectedPath;
                }));

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}