using Disipar.Models;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using System;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class FamiliaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.VerificarLogin();
                    CargarClientes();
                }

                Page.Form.DefaultButton = this.lbtnBuscarPadre.UniqueID;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        #region carga inicial
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

        public void CargarClientes()
        {
            try
            {
                controladorCliente controladorCliente = new controladorCliente();

                ListPadre.DataSource = controladorCliente.obtenerClientesDT();
                ListPadre.DataValueField = "id";
                ListPadre.DataTextField = "alias";

                ListPadre.DataBind();

                ListNieto.Items.Clear();
                ListNieto.Items.RemoveAt(0);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando la lista de los padres " + ex.Message));
            }
        }

        #endregion

        protected void ListPadre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorCliente = new ControladorClienteEntity();

                ListHijo.DataSource = controladorCliente.ObtenerFamiliaDelCliente(Convert.ToInt32(ListPadre.SelectedValue));
                ListHijo.DataValueField = "id";
                ListHijo.DataTextField = "alias";

                ListHijo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando la lista de los padres " + ex.Message));
            }

        }

        protected void ListHijo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorCliente = new ControladorClienteEntity();

                ListNieto.DataSource = controladorCliente.ObtenerFamiliaDelCliente(Convert.ToInt32(ListHijo.SelectedValue));
                ListNieto.DataValueField = "id";
                ListNieto.DataTextField = "alias";

                ListNieto.DataBind();
            }
            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando la lista de los padres " + ex.Message));
            }


        }

        protected void lbtnBuscarPadre_Click(object sender, EventArgs e)
        {

            try
            {
                controladorCliente controladorCliente = new controladorCliente();

                ListPadre.DataSource = controladorCliente.obtenerClientesAliasDT(txtBusqueda.Text);
                ListPadre.DataValueField = "id";
                ListPadre.DataTextField = "razonSocial";

                ListPadre.DataBind();

            }
            catch (Exception ex)
            {

            }

        }
    }
}