using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ExportarTapiceF : System.Web.UI.Page
    {
        controladorReportes contReportes = new controladorReportes();
        protected void Page_Load(object sender, EventArgs e)
        {

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
                this.Response.End();
            }
            catch
            {

            }
        }
    }
}