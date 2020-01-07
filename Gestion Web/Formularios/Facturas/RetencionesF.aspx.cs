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
            public string Retencion { get; set; }
            public string Factura { get; set; }
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

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

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
                this.DropListProvincias.DataSource = controladorPais.obtenerPRovincias();
                this.DropListProvincias.DataValueField = "Provincia";
                this.DropListProvincias.DataTextField = "Provincia";
                this.DropListProvincias.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.DropListProvincias.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        [WebMethod]
        public static string TraerRegistrosDe_CuentasContables_MayorTipoMovimiento()
        {
            try
            {
                controladorFactEntity contFactEntity = new controladorFactEntity();
                var facturas_IIBB = contFactEntity.GetAll_Facturas_IIBB_Provincias();
                List<Facturas_IIBB_Provincias_Temporal> facturas_IIBB_Provincias_Temporal = new List<Facturas_IIBB_Provincias_Temporal>();

                foreach (var item in facturas_IIBB)
                {
                    facturas_IIBB_Provincias_Temporal.Add(new Facturas_IIBB_Provincias_Temporal
                    {
                        Id = item.Id.ToString(),
                        Factura = item.factura.numero,
                        Provincia = item.Cliente_IIBB_Provincias.Provincia.Provincia1,
                        Fecha = item.factura.fecha.ToString(),
                        Retencion = Math.Round(item.Cliente_IIBB_Provincias.Retencion, 2).ToString()
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