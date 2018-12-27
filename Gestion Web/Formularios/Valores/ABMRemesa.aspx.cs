using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMRemesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosEnRemesa();
                //CargarArticulosDropDownList();
                //if (accion == 1)
                //    
                //else if (accion == 2)
                //    ModificarOrdenReparacion();
            }
        }

        public void CargarDatosEnRemesa()
        {
            try
            {
                txtNumeroRemesa.CssClass = "form-control";
                txtFecha.CssClass = "form-control";
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar datos en remesa");
            }
        }
    }
}