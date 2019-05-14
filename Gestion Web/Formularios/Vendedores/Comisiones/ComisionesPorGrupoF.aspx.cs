using Disipar.Models;
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

namespace Gestion_Web.Formularios.Vendedores.Comisiones
{
    public partial class ComisionesPorGrupoF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorSucursal controladorSucursal = new controladorSucursal();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarEmpresas();
                CargarSucursal(Convert.ToInt32(DropListEmpresa.SelectedValue));
                CargarPuntoVenta(Convert.ToInt32(DropListSucursal.SelectedValue));
            }
        }

        void CargarEmpresas()
        {
            try
            {                
                DataTable dt = controladorSucursal.obtenerEmpresas();

                DropListEmpresa.DataSource = dt;
                DropListEmpresa.DataValueField = "Id";
                DropListEmpresa.DataTextField = "Razon Social";

                DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        void CargarSucursal(int empresa)
        {
            try
            {
                DataTable dt = controladorSucursal.obtenerSucursalesDT(empresa);

                DropListSucursal.DataSource = dt;
                DropListSucursal.DataValueField = "Id";
                DropListSucursal.DataTextField = "nombre";

                DropListSucursal.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        void CargarPuntoVenta(int sucursal)
        {
            try
            {
                DataTable dt = controladorSucursal.obtenerPuntoVentaDT(sucursal);

                DropListPuntoVenta.DataSource = dt;
                DropListPuntoVenta.DataValueField = "Id";
                DropListPuntoVenta.DataTextField = "NombreFantasia";

                DropListPuntoVenta.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pto ventas. " + ex.Message));
            }
        }

        [WebMethod]
        public static string RecargarSucursales(int empresa)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();
            DataTable dt = controladorSucursal.obtenerSucursalesDT(empresa);

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

        [WebMethod]
        public static string RecargarPuntoVenta(int sucursal)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();
            DataTable dt = controladorSucursal.obtenerPuntoVentaDT(sucursal);

            List<PuntoVentaTemporal> puntosVenta = new List<PuntoVentaTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                PuntoVentaTemporal puntoVentaTemporal = new PuntoVentaTemporal();
                puntoVentaTemporal.id = row["Id"].ToString();
                puntoVentaTemporal.nombreFantasia = row["NombreFantasia"].ToString();
                puntosVenta.Add(puntoVentaTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(puntosVenta);
            return resultadoJSON;
        }
    }

    class SucursalesTemporal
    {
        public string id;
        public string nombre;
    }
    class PuntoVentaTemporal
    {
        public string id;
        public string nombreFantasia;
    }
}