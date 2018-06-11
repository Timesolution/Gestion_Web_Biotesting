using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class TestOpen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                String script = "window.open('ImpresionPresupuesto?Presupuesto=16668', '_blank'); window.open('ImpresionPresupuesto?a=3&Presupuesto=37', '_blank');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);
            }
            catch(Exception ex)
            {

            }
        }
    }
}