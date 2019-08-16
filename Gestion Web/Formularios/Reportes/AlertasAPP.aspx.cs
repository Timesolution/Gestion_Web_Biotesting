using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores.APP;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class AlertasAPP : System.Web.UI.Page
    {
        Mensajes _m = new Mensajes();

        controladorCliente _controladorCliente = new controladorCliente();
        ControladorAlertaAPP controladorAlertaAPP = new ControladorAlertaAPP();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();

                if (!IsPostBack)
                {
                    CargarClientes();
                    CargarVendedores();
                }                    
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar alertasAPP. " + ex.Message);
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
                    if (this.verificarAcceso() != 1)
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

        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {

                    }
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        void CargarClientes()
        {
            try
            {
                DataTable dt = _controladorCliente.obtenerClientesDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropListCliente.DataSource = dt;
                DropListCliente.DataValueField = "id";
                DropListCliente.DataTextField = "alias";

                DropListCliente.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando clientes. " + ex.Message));
            }
        }

        void CargarVendedores()
        {
            try
            {
                controladorVendedor _controladorVendedor = new controladorVendedor();
                DataTable dt = _controladorVendedor.obtenerVendedores();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropListVendedor.DataSource = dt;
                DropListVendedor.DataValueField = "id";
                DropListVendedor.DataTextField = "nombre";

                DropListVendedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando vendedores. " + ex.Message));
            }
        }
    }
}