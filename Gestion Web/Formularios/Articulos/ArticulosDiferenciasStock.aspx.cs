using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ArticulosDiferenciasStock : System.Web.UI.Page
    {
        controladorSucursal controladorSucursal = new controladorSucursal();
        controladorArticulo controladorArticulo = new controladorArticulo();
        Mensajes mensajes = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            try
            {
                if (!IsPostBack)
                {
                    CargarEmpresasEnLista();
                    CargarSucursalesEnLista();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error cargando pagina ArticulosDiferenciasStock. Excepción: " + ex.Message + ". StackTrace: " + ex.StackTrace);
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
                else
                {
                    if (VerificarAcceso() != 1)
                    {
                        Response.Redirect("/Default.aspx?m=1", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        private int VerificarAcceso()
        {
            int poseePermiso = 0;

            string permisos = Session["Login_Permisos"] as string;
            string[] listPermisos = permisos.Split(';');

            string permisoArticulosDiferenciasStock = listPermisos.Where(x => x == "180").FirstOrDefault();

            if (!string.IsNullOrEmpty(permisoArticulosDiferenciasStock))
            {
                poseePermiso = 1;
            }

            return poseePermiso;

        }

        private void CargarEmpresasEnLista()
        {
            var dtEmpresas = controladorSucursal.obtenerEmpresas();

            if (dtEmpresas != null)
            {
                if (dtEmpresas.Rows.Count > 1)
                {
                    DataRow drEmpresa = dtEmpresas.NewRow();
                    drEmpresa["Razon Social"] = "Seleccione...";
                    drEmpresa["Id"] = "0";
                    dtEmpresas.Rows.InsertAt(drEmpresa, 0);
                }

                ListEmpresa.DataSource = dtEmpresas;
                ListEmpresa.DataTextField = "Razon Social";
                ListEmpresa.DataValueField = "Id";
                ListEmpresa.DataBind();
            }
        }

        private void CargarSucursalesEnLista(int idEmpresa = 0)
        {
            DataTable dtSucursales = new DataTable();

            if (idEmpresa > 0)
            {
                dtSucursales = controladorSucursal.obtenerSucursalesDT(idEmpresa);
            }
            else
            {
                dtSucursales = controladorSucursal.obtenerSucursales();
            }

            if (dtSucursales != null)
            {
                if (dtSucursales.Rows.Count > 1)
                {
                    DataRow drSucursal = dtSucursales.NewRow();
                    drSucursal["nombre"] = "Seleccione...";
                    drSucursal["id"] = "0";
                    dtSucursales.Rows.InsertAt(drSucursal, 0);
                }

                ListSucursal.DataSource = dtSucursales;
                ListSucursal.DataTextField = "nombre";
                ListSucursal.DataValueField = "id";
                ListSucursal.DataBind();
            }
        }

        protected void ListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargarSucursalesEnLista(Convert.ToInt32(ListEmpresa.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error cargando sucursales según empresa. Excepción: " + ex.Message + ". StackTrace: " + ex.StackTrace);
            }
        }

        protected void btnGenerarDiferenciasStock_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarFileUpload())
                {
                    GenerarDiferenciasStock();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Ocurrió un error generando diferencias de stock. Excepción: " + ex.Message + ". StackTrace: " + ex.StackTrace);
            }
        }

        private bool ValidarFileUpload()
        {
            if (FileUpload.HasFile)
            {
                return true;
            }

            return false;
        }

        private void GenerarDiferenciasStock()
        {
            string path = ObtenerPathArchivoDeDiferenciasStock();
            GuardarArchivoEnFileSystem(path);
            var resultado = controladorArticulo.GenerarDiferenciasStockEnSucursalDesdeExcel(path + FileUpload.FileName, Convert.ToInt32(ListSucursal.SelectedValue));
        }

        private string ObtenerPathArchivoDeDiferenciasStock()
        {
            string pathArchivoDiferenciasStock = Server.MapPath("../../ArticulosDiferenciasStock/" + DateTime.Now.ToString("ddMMyyymmss") + "/");

            if (!Directory.Exists(pathArchivoDiferenciasStock))
            {
                Directory.CreateDirectory(pathArchivoDiferenciasStock);
            }

            return pathArchivoDiferenciasStock;
        }

        private void GuardarArchivoEnFileSystem(string path)
        {
            FileUpload.PostedFile.SaveAs(path + FileUpload.FileName);
        }
    }
}