using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Newtonsoft.Json;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ActividadClientes : System.Web.UI.Page
    {
        controladorSucursal _controladorSucursal = new controladorSucursal();
        Mensajes _m = new Mensajes();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ObtenerSucursales();
                ObtenerProvincias();
            }
        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../../Account/Login.aspx");
                }
                else
                {
                    if (this.VerificarAcceso() != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("../../../Account/Login.aspx");
            }
        }

        private int VerificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                //if (!permisos.Contains("206"))
                //    return 0;

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        void ObtenerProvincias()
        {
            try
            {
                ControladorProvincias controladorProvincias = new ControladorProvincias();

                var provincias = controladorProvincias.ObtenerProvincias();

                Provincia provincia = new Provincia
                {
                    Provincia1 = "Todas",
                    Id = -1
                };

                provincias.Insert(0,provincia);

                DropDownListProvincias.DataSource = provincias;
                DropDownListProvincias.DataValueField = "Id";
                DropDownListProvincias.DataTextField = "Provincia1";

                DropDownListProvincias.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando provincias. " + ex.Message));
            }
        }

        void ObtenerSucursales()
        {
            try
            {
                DataTable dt = null;
                int empresa = Convert.ToInt32(Session["Login_EmpUser"]);

                dt = _controladorSucursal.obtenerSucursalesDT(empresa);

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropDownListSucursales.DataSource = dt;
                DropDownListSucursales.DataValueField = "Id";
                DropDownListSucursales.DataTextField = "nombre";

                DropDownListSucursales.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        [WebMethod]
        public static string ObtenerLocalidades(string provincia)
        {
            controladorPais controladorPais = new controladorPais();
            DataTable localidades = controladorPais.obtenerLocalidadProvincia(provincia);

            DataRow dr = localidades.NewRow();
            dr["Localidad"] = "Todos";
            localidades.Rows.InsertAt(dr, 0);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = JsonConvert.SerializeObject(localidades);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerVendedores(int idSucursal)
        {
            controladorVendedor controladorVendedor = new controladorVendedor();
            DataTable dt = null;

            if (idSucursal > 0)
                dt = controladorVendedor.obtenerVendedoresBySuc(idSucursal);
            else
                dt = controladorVendedor.obtenerVendedores();

            DataRow dr = dt.NewRow();
            dr["nombre"] = "Todos";
            dr["id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = JsonConvert.SerializeObject(dt);
            return resultadoJSON;
        }
    }
}