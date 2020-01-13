using Disipar.Models;
using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class RetencionesF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        public class Facturas_IIBB_Provincias_Temporal
        {
            public string Id { get; set; }
            public string Fecha { get; set; }
            public string Provincia { get; set; }
            public string Percepcion { get; set; }
            public string RazonSocial{ get; set; }
            public string Neto { get; set; }
            public string MontoPercepcion { get; set; }
            public string Factura { get; set; }
            public string CUIT { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();
                if (!IsPostBack)
                {
                    CargarDropLists();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        private void CargarDropLists()
        {
            CargarSucursales();
            CargarProvincias();
        }

        public void CargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                DropListSucursal.Items.Insert(0, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        private void CargarProvincias()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerPRovincias();
                this.DropListProvincias.DataSource = dt;
                this.DropListProvincias.DataValueField = "Provincia";
                this.DropListProvincias.DataTextField = "Provincia";

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Provincia"] = "Todas";
                dt.Rows.InsertAt(dr, 0);

                this.DropListProvincias.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        [WebMethod]
        public static string TraerRegistrosDe_CuentasContables_MayorTipoMovimiento(string FechaDesde, string FechaHasta, string Provincia, string Sucursal)
        {
            try
            {
                controladorFacturacion controladorFacturacion = new controladorFacturacion();
                controladorFactEntity contFactEntity = new controladorFactEntity();
                var facturas_IIBB = contFactEntity.GetAll_Facturas_IIBB_Provincias(FechaDesde, FechaHasta, Provincia, Convert.ToInt32(Sucursal));
                List<Facturas_IIBB_Provincias_Temporal> facturas_IIBB_Provincias_Temporal = new List<Facturas_IIBB_Provincias_Temporal>();

                foreach (var item in facturas_IIBB)
                {
                    var fact = controladorFacturacion.obtenerFacturaId(item.IdFactura);
                    facturas_IIBB_Provincias_Temporal.Add(new Facturas_IIBB_Provincias_Temporal
                    {
                        Id = item.Id.ToString(),
                        Factura = fact.numero,
                        Provincia = item.Cliente_IIBB_Provincias.Provincia.Provincia1,
                        Fecha = fact.fecha.ToString("dd/MM/yyyy"),
                        Percepcion = Math.Round(item.Cliente_IIBB_Provincias.Percepcion, 2).ToString(),
                        MontoPercepcion = Math.Round(fact.netoNGrabado * item.Cliente_IIBB_Provincias.Percepcion / 100, 2).ToString(),
                        Neto = Math.Round(fact.netoNGrabado,2).ToString(),
                        RazonSocial = fact.cliente.razonSocial,
                        CUIT = fact.cliente.cuit
                    });
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(facturas_IIBB_Provincias_Temporal);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}