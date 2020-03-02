using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestFacturaElectronica;

namespace Gestion_Web.Formularios.Zonas
{
    public partial class ConsultaFactura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                FacturaElectronica fc= new FacturaElectronica();
                fc.Cuit = this.txtCuit.Text;
                fc.PuntoVenta = Convert.ToInt32(this.txtPuntoVenta.Text);
                fc.NroFactura = this.txtComprobante.Text;
                fc.TipoFactura = this.txtTipoFactura.Text;
                
                String path = HttpContext.Current.Server.MapPath("../../Facturas/1//");

                fc.Certificado = path + "Certificado.pfx";
                if (!File.Exists(fc.Certificado))
                {
                    //no existe licencia
                    Log.EscribirSQL(1, "Info", "No Existe archivo de licencia " + fc.Certificado);

                }
                else
                {
                    Log.EscribirSQL(1, "Info", "Existe archivo de licencia " + fc.Certificado);
                }


                fc.Licencia = path + "WSAFIPFE.lic";

                if (!File.Exists(fc.Licencia))
                {
                    //no existe licencia
                    Log.EscribirSQL(1, "Info", "No Existe archivo de licencia " + fc.Licencia);
                    
                }
                else
                {
                    Log.EscribirSQL(1, "Info", "Existe archivo de licencia " + fc.Licencia);
                }

                String pathXML = HttpContext.Current.Server.MapPath("../../Facturas/1/");
                fc.pathXML = pathXML;

                fc.Consultar();
                //fc.VerificarUltimaFactElec();

            }
            catch(Exception ex)
            {
                
            }
        }

        protected void btnPlenario_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch
            {

            }
        }
    }
}