using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        Mensajes _m = new Mensajes();
        controladorSucursal _controladorSucursal = new controladorSucursal();

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
                DataTable dt = _controladorSucursal.obtenerEmpresas();

                DropListEmpresa.DataSource = dt;
                DropListEmpresa.DataValueField = "Id";
                DropListEmpresa.DataTextField = "Razon Social";

                DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        void CargarSucursal(int empresa)
        {
            try
            {
                DataTable dt = _controladorSucursal.obtenerSucursalesDT(empresa);

                DropListSucursal.DataSource = dt;
                DropListSucursal.DataValueField = "Id";
                DropListSucursal.DataTextField = "nombre";

                DropListSucursal.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        void CargarPuntoVenta(int sucursal)
        {
            try
            {
                DataTable dt = _controladorSucursal.obtenerPuntoVentaDT(sucursal);

                DropListPuntoVenta.DataSource = dt;
                DropListPuntoVenta.DataValueField = "Id";
                DropListPuntoVenta.DataTextField = "NombreFantasia";

                DropListPuntoVenta.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando pto ventas. " + ex.Message));
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

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta, int idEmpresa, int idSucursal, int idPuntoVenta)
        {
            controladorVendedor controladorVendedor = new controladorVendedor();

            DataTable dt = controladorVendedor.ObtenerVentasPorComisionByGrupo(fechaDesde, fechaHasta, idEmpresa, idSucursal, idPuntoVenta);            

            List<DatosFiltradosTemporal> datosFiltradosTemporales = new List<DatosFiltradosTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                DatosFiltradosTemporal datosFiltradosTemporal = new DatosFiltradosTemporal();
                datosFiltradosTemporal.fecha = Convert.ToDateTime(row["fecha"].ToString(), CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                datosFiltradosTemporal.tipo = row["tipo"].ToString();
                datosFiltradosTemporal.codigo = row["codigo"].ToString();
                datosFiltradosTemporal.descripcion = row["descripcion"].ToString();
                datosFiltradosTemporal.nombre = row["nombre"].ToString();
                datosFiltradosTemporal.precioSinIVA = "$" + row["precioSinIVA"].ToString();
                datosFiltradosTemporal.grupoArticulo = row["grupo"].ToString();
                datosFiltradosTemporal.comision = "$" + row["comision"].ToString();
                datosFiltradosTemporal.total = "$" + CalcularTotal(datosFiltradosTemporal.precioSinIVA, datosFiltradosTemporal.comision);
                datosFiltradosTemporales.Add(datosFiltradosTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(datosFiltradosTemporales);
            return resultadoJSON;
        }

        static string CalcularTotal(string comision, string neto)
        {
            decimal comisionTemp = Convert.ToDecimal(comision.Split('$')[1]);
            decimal netoTemp = Convert.ToDecimal(neto.Split('$')[1]);

            decimal total = decimal.Round((comisionTemp / 100) * netoTemp,2);

            return total.ToString();
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
    class DatosFiltradosTemporal
    {
        public string fecha;
        public string tipo;
        public string codigo;
        public string descripcion;
        public string nombre;
        public string precioSinIVA;
        public string grupoArticulo;
        public string comision;
        public string total;
    }
}