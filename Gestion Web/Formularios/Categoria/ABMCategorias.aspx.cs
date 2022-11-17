using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Categoria
{
    public partial class ABMCategorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();
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

        [WebMethod]
        public static string getCategorias()
        {
            try
            {
                controladorCategoria cCategorias = new controladorCategoria();

                DataTable dt =cCategorias.getCategoria();

                string json = "[";
                foreach (DataRow item in dt.Rows)
                {
                    json += "{" +
                        "\"id\": \"" + item["id"] + "\"," +
                        "\"nombre\": \"" + item["nombre"] + "\"," +
                        "\"alerta\": \"" + item["alerta"] + "\"," +
                        "\"descripcion\": \"" + item["descripcion"] + "\"" +
                        "},";
                }
                json = json.Remove(json.Length -1 )+"]";
                return json;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string getAlertas()
        {
            try
            {
                controladorAlertaCategoria cAlertas = new controladorAlertaCategoria();

                DataTable dt = cAlertas.getAlerta();

                string json = "[";
                foreach (DataRow item in dt.Rows)
                {
                    json += "{" +
                        "\"id\": \"" + item["id"] + "\"," +
                        "\"nombre\": \"" + item["descripcion"] + "\"" +
                        "},";
                }
                json = json.Remove(json.Length -1 )+"]";
                return json;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string addCategorias(string nombre, string alerta)
        {
            try
            {
                controladorCategoria cCategorias = new controladorCategoria();

                int i =cCategorias.addCategoria(nombre,alerta);

                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string modifCategorias(string id, string nombre, string alerta)
        {
            try
            {
                controladorCategoria cCategorias = new controladorCategoria();

                int i =cCategorias.modifCategorias(id, nombre, alerta);

                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string EliminarCategorias(string id, string estado)
        {
            try
            {
                controladorCategoria cCategorias = new controladorCategoria();
                controladorArticulo cArt = new controladorArticulo();

                // Validamos si existe algun articulo con esta categoria
                DataTable dt = cArt.getArticulo_CategoriaByIdCateg(Convert.ToInt32(id));

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows.Count.ToString();
                }
                else
                {
                    int i =cCategorias.cambiarEstadoByIdCategoria(id, estado);
                    return "0";
                }

            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
    }
}