using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class ResetearStock : System.Web.UI.Page
    {
        controladorSucursal contSucursal = new controladorSucursal();

        class SucursalesTemporal
        {
            public string id;
            public string nombre;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        public static string RecargarSucursales(int empresa)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();

            DataTable dt = null;

            if (empresa > 0)
                dt = controladorSucursal.obtenerSucursalesDT(empresa);
            else
                dt = controladorSucursal.obtenerSucursales();

            DataRow dr = dt.NewRow();
            dr["nombre"] = "Todas";
            dr["Id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            List<SucursalesTemporal> sucursales = new List<SucursalesTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                SucursalesTemporal sucursalesTemporal = new SucursalesTemporal();
                sucursalesTemporal.id = row["Id"].ToString();
                sucursalesTemporal.nombre = row["nombre"].ToString();
                sucursales.Add(sucursalesTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(sucursales);
            return resultadoJSON;
        }
    }
}