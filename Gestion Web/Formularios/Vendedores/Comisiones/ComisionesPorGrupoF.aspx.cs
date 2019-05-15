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

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta, int idEmpresa, int idSucursal, int idPuntoVenta)
        {
            //try
            //{
                controladorVendedor controladorVendedor = new controladorVendedor();
                fechaDesde = Convert.ToDateTime(fechaDesde.ToString("dd/MM/yyyy"), CultureInfo.InvariantCulture);
                fechaHasta = Convert.ToDateTime("05/15/2019");
                DataTable dt = controladorVendedor.ObtenerVentasPorComisionByGrupo(fechaDesde, fechaHasta, idEmpresa, idSucursal, idPuntoVenta);

                //foreach (DataRow dataRow in dt.Rows)
                //{
                //    CargarPH(dataRow);
                //}
            //}
            //catch (Exception ex)
            //{
            //    Log.EscribirSQL(1,"Error","Error al filtrar y llenar el PH de ventas por comision " + ex.Message);
            //}

            List<DatosFiltradosTemporal> datosFiltradosTemporales = new List<DatosFiltradosTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                DatosFiltradosTemporal datosFiltradosTemporal = new DatosFiltradosTemporal();
                datosFiltradosTemporal.fecha = Convert.ToDateTime(row["fecha"].ToString(), CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                datosFiltradosTemporal.tipo = row["tipo"].ToString();
                datosFiltradosTemporal.codigo = row["codigo"].ToString();
                datosFiltradosTemporal.descripcion = row["descripcion"].ToString();
                datosFiltradosTemporal.nombre = row["nombre"].ToString();
                datosFiltradosTemporal.precioSinIVA = row["precioSinIVA"].ToString();
                datosFiltradosTemporal.comision = row["comision"].ToString();
                datosFiltradosTemporales.Add(datosFiltradosTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(datosFiltradosTemporales);
            return resultadoJSON;
        }

        //static void CargarPH(DataRow dataRow)
        //{
        //    try
        //    {
        //        TableRow tr = new TableRow();
        //        //tr.ID = item.articulo.codigo.ToString() + pos;

        //        TableCell celFecha = new TableCell();
        //        celFecha.Text = Convert.ToDateTime(dataRow["fecha"].ToString(), CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
        //        celFecha.Width = Unit.Percentage(15);
        //        celFecha.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celFecha);

        //        TableCell celDocumento = new TableCell();
        //        celDocumento.Text = dataRow["tipo"].ToString();
        //        celDocumento.Width = Unit.Percentage(15);
        //        celDocumento.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celDocumento);

        //        TableCell celCodigoArticulo = new TableCell();
        //        celCodigoArticulo.Text = dataRow["codigo"].ToString();
        //        celCodigoArticulo.Width = Unit.Percentage(15);
        //        celCodigoArticulo.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celCodigoArticulo);

        //        TableCell celDescripcion = new TableCell();
        //        celDescripcion.Text = dataRow["descripcion"].ToString();
        //        celDescripcion.Width = Unit.Percentage(15);
        //        celDescripcion.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celDescripcion);

        //        TableCell celVendedor = new TableCell();
        //        celVendedor.Text = dataRow["nombre"].ToString();
        //        celVendedor.Width = Unit.Percentage(15);
        //        celVendedor.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celVendedor);

        //        TableCell celNeto = new TableCell();
        //        celNeto.Text = dataRow["precioSinIva"].ToString();
        //        celNeto.Width = Unit.Percentage(15);
        //        celNeto.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celNeto);

        //        TableCell celComision = new TableCell();
        //        celComision.Text = dataRow["comision"].ToString();
        //        celComision.Width = Unit.Percentage(15);
        //        celComision.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celComision);

        //        //TableCell celTotal = new TableCell();
        //        //celTotal.Text = dataRow["fecha"].ToString();
        //        //celTotal.Width = Unit.Percentage(15);
        //        //celTotal.VerticalAlign = VerticalAlign.Middle;
        //        //tr.Cells.Add(celTotal);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }            
        //}
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
        public string comision;
    }
}