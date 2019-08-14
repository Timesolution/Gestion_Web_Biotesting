using Gestion_Api.Controladores;
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
        ControladorPedidoEntity _controladorPedidoEntity = new ControladorPedidoEntity();
        int _idUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            _idUsuario = (int)Session["Login_IdUser"];

            _controladorPedidoEntity.ImportarPedidoAPP(_idUsuario);
        }
    }
}