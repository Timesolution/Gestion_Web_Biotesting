using Gestion_Api.Controladores;
using Gestion_Api.Controladores.APP;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ImportacionPedidosAPP : System.Web.UI.Page
    {
        ControladorPedidosAPP _controladorPedidosAPP = new ControladorPedidosAPP();
        int _idUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _idUsuario = (int)Session["Login_IdUser"];

                _controladorPedidosAPP.ImportarPedidoAPP(_idUsuario);

                Response.Redirect("../Facturas/PedidosP.aspx");
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al importar los pedidos de la APP. " + ex.Message);
            }
            
        }
    }
}