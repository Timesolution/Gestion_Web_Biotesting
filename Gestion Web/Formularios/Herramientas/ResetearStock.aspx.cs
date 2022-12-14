using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
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
        controladorCliente contCliente = new controladorCliente();
        controladorArticulo contArticulo = new controladorArticulo();
        Mensajes m = new Mensajes();
        public int subGrupo;

        class ListItemTemporal
        {
            public string id;
            public string nombre;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            try
            {
                if (!IsPostBack)
                {
                    CargarDropLists();
                }
            }
            catch (Exception ex)
            {

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

            string perfil = Session["Login_NombrePerfil"] as string;

            //verifico si es super admin                        
            if (perfil == "SuperAdministrador")
            {
                poseePermiso = 1;
                return poseePermiso;
            }
            return poseePermiso;
        }

        #region cargas drop list
        private void CargarDropLists()
        {
            CargarEmpresasEnLista();
            CargarSucursalesEnLista();
            CargarProveedores();
            CargarGruposArticulos();
            CargarSubGruposArticulos(subGrupo);
            CargarMarcasArticulos();
        }
        private void CargarEmpresasEnLista()
        {
            var dtEmpresas = contSucursal.obtenerEmpresas();

            if (dtEmpresas != null)
            {
                if (dtEmpresas.Rows.Count > 1)
                {
                    DataRow drEmpresa = dtEmpresas.NewRow();
                    drEmpresa["Razon Social"] = "Seleccione...";
                    drEmpresa["Id"] = "0";
                    dtEmpresas.Rows.InsertAt(drEmpresa, 0);
                }

                DropListEmpresa.DataSource = dtEmpresas;
                DropListEmpresa.DataTextField = "Razon Social";
                DropListEmpresa.DataValueField = "Id";
                DropListEmpresa.DataBind();
            }
        }
        private void CargarSucursalesEnLista(int idEmpresa = 0)
        {
            DataTable dtSucursales = new DataTable();

            if (idEmpresa > 0)
            {
                dtSucursales = contSucursal.obtenerSucursalesDT(idEmpresa);
            }
            else
            {
                dtSucursales = contSucursal.obtenerSucursales();
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

                DropListSucursal.DataSource = dtSucursales;
                DropListSucursal.DataTextField = "nombre";
                DropListSucursal.DataValueField = "id";
                DropListSucursal.DataBind();
            }
        }
        [WebMethod]
        public static string ObtenerSucursalesDependiendoDeLaEmpresa(int empresa)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();

            DataTable dt = null;

            if (empresa > 0)
                dt = controladorSucursal.obtenerSucursalesDT(empresa);
            else
                dt = controladorSucursal.obtenerSucursales();

            List<ListItemTemporal> listaTemporal = new List<ListItemTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                ListItemTemporal listItemTemporal = new ListItemTemporal();
                listItemTemporal.id = row["Id"].ToString();
                listItemTemporal.nombre = row["nombre"].ToString();
                listaTemporal.Add(listItemTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(listaTemporal);
            return resultadoJSON;
        }
        public void CargarProveedores()
        {
            try
            {
                DataTable dt = new DataTable();

                dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListProveedor.DataSource = dt;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";

                this.DropListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        private void CargarGruposArticulos()
        {
            try
            {
                DataTable dt = contArticulo.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Todos";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListGrupo.DataSource = dt;
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }
        private void CargarSubGruposArticulos(int subGrupo)
        {
            try
            {
                if (subGrupo == 0)
                {
                    //agrego todos
                    ListItem listItem = new ListItem("Todos", "0");
                    this.DropListSubGrupo.Items.Add(listItem);
                }
                else
                {
                    DataTable dt = contArticulo.obtenerSubGruposArticulos(subGrupo);

                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["descripcion"] = "Todos";
                    dr["id"] = 0;
                    dt.Rows.InsertAt(dr, 0);

                    this.DropListSubGrupo.DataSource = dt;
                    this.DropListSubGrupo.DataValueField = "id";
                    this.DropListSubGrupo.DataTextField = "descripcion";
                    this.DropListSubGrupo.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
            }
        }
        [WebMethod]
        public static string ObtenerSubGruposDependiendoDelGrupo(int grupo)
        {
            try
            {
                controladorArticulo contArticulo = new controladorArticulo();
                DataTable dt = null;
                List<ListItemTemporal> listaTemporal = new List<ListItemTemporal>();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string resultadoJSON;
                if (grupo > 0)
                {
                    dt = contArticulo.obtenerSubGrupoDT(grupo);
                    DataRow dr = dt.NewRow();
                    dr["descripcion"] = "Todos";
                    dr["Id"] = 0;
                    dt.Rows.InsertAt(dr, 0);
                    foreach (DataRow row in dt.Rows)
                    {
                        ListItemTemporal listItemTemporal = new ListItemTemporal();
                        listItemTemporal.id = row["Id"].ToString();
                        listItemTemporal.nombre = row["descripcion"].ToString();
                        listaTemporal.Add(listItemTemporal);
                    }
                }
                else
                {
                    ListItemTemporal listItemTemporal = new ListItemTemporal();
                    listItemTemporal.id = "0";
                    listItemTemporal.nombre = "Todos";
                    listaTemporal.Add(listItemTemporal);
                }
                resultadoJSON = serializer.Serialize(listaTemporal);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private void CargarMarcasArticulos()
        {
            try
            {
                DataTable dt = contArticulo.obtenerMarcasDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["marca"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListMarca.DataSource = dt;
                this.DropListMarca.DataValueField = "id";
                this.DropListMarca.DataTextField = "marca";

                this.DropListMarca.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando marcas de articulos a la lista. " + ex.Message));
            }
        }
        #endregion

        protected void ProcesarReinicioDeStock_Click(object sender, EventArgs e)
        {
            try
            {
                int IdUsuario = (int)Session["Login_IdUser"];
                int IdSucursal = Convert.ToInt32(DropListSucursal.SelectedValue);
                int IdProveedor = Convert.ToInt32(DropListProveedor.SelectedValue);
                int IdGrupo = Convert.ToInt32(DropListGrupo.SelectedValue);
                int IdSubGrupo = Convert.ToInt32(DropListSubGrupo.SelectedValue);
                int IdMarca = Convert.ToInt32(DropListMarca.SelectedValue);

                int respuesta = contArticulo.ReiniciarStock(IdUsuario, IdSucursal, IdProveedor, IdGrupo, IdSubGrupo, IdMarca);

                if (respuesta > 0)
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel, UpdatePanel.GetType(), "alert", m.mensajeBoxError("Stock reiniciado correctamente."), true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Stock reiniciado correctamente.", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Hubo un problema al reiniciar el Stock."));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}