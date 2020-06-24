using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ExportarTapiceF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorReportes contReportes = new controladorReportes();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo(" cargo pagina. ",null));
        }

        protected void lbtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                
                string rutaTxt = Server.MapPath("../Herramientas/Tapice/");

                if (!Directory.Exists(rutaTxt))
                {
                    Directory.CreateDirectory(rutaTxt);
                }

                string archivos = this.contReportes.generarArchivoTapice(rutaTxt);

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
                //this.Response.Flush();
                //this.Response.SuppressContent = true;
                this.Response.Close();
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Exito", "../Herramientas/ExportarTapiceF.aspx"));
                //this.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //this.Response.Headers.Clear();
                //this.Response.Redirect("../Herramientas/ExportarTapiceF.aspx", false);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Exito", "../Herramientas/ExportarTapiceF.aspx"));
                //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
            catch(Exception ex)
            {
                lbtnGenerar.Enabled = true;
                Log.EscribirSQL(1, "ERROR", "CATCH: No se pudieron importar articulos desde base externta.Ubicacion: ExportarTapiceF. Metodo: lbtnGenerar_Click. Mensaje: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo generar el archivo. Contacte a soporte."));
            }
            finally
            {
                lbtnGenerar.Enabled = true;
                this.Response.Redirect("../Herramientas/ExportarTapiceF.aspx", false);
                //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClosePopup", "window.close();", true);

            }
        }

        protected void descargarArchivo(byte[] btFile, string archivos)
        {
            
            //this.Response.Write(btFile.ToString());
            //this.Response.End();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Exito", "../Herramientas/ExportarTapiceF.aspx"));
        }
    }
}