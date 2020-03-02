using Gestion_Api.Modelo;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Disipar.Models;
using System.Data;
using Gestor_Solution.Controladores;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class ServicioTecnicoABM : System.Web.UI.Page
    {
        controladorArticulo contArt = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorServicioTecnicoEntity contServTecEnt = new controladorServicioTecnicoEntity();
        Mensajes m = new Mensajes();

        int accion = 0;
        int stID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            accion = Convert.ToInt32(Request.QueryString["a"]);
            stID = Convert.ToInt32(Request.QueryString["st"]);

            if (!IsPostBack)
            {
                cargarMarcas();
                cargarClientes();

                if (accion == 2)
                {
                    btnGuardar.Visible = true;
                    btnAgregar.Visible = false;
                    ModificarServicioTecnico();
                }
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Herramientas.Presupuesto") != 1)
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
                        if (s == "161")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarMarcas()
        {
            try
            {
                DataTable dt = contArt.obtenerMarcasDT();

                this.ListMarcas.DataSource = dt;
                this.ListMarcas.DataValueField = "id";
                this.ListMarcas.DataTextField = "marca";

                this.ListMarcas.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de marcas. " + Ex.Message));
                Log.EscribirSQL(1, "Error", "Error cargando lista de marcas " + Ex.Message);
            }
        }
        public void cargarClientes()
        {
            try
            {
                DataTable dt = contCliente.obtenerClientesDT();

                this.ListClientes.DataSource = dt;
                this.ListClientes.DataValueField = "id";
                this.ListClientes.DataTextField = "razonSocial";

                this.ListClientes.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de marcas. " + Ex.Message));
                Log.EscribirSQL(1, "Error", "Error cargando lista de marcas " + Ex.Message);
            }
        }

        protected void btnBuscarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                var marca = contArtEnt.ObtenerMarcaByDescripcion(txtDescMarca.Text);
                
                this.ListMarcas.SelectedValue = marca.FirstOrDefault().id.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                //var clientes = contCliente.obtenerClientesAlias(txtCliente.Text);
                DataTable clientes = this.contCliente.obtenerClientesAliasDT(this.txtCliente.Text);

                this.ListClientes.DataSource = clientes;
                this.ListClientes.DataValueField = "id";
                this.ListClientes.DataTextField = "razonSocial";
                this.ListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando cliente " + ex.Message);
            }
        }

        protected void btnAgregarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                if(!ListBoxMarcas.Items.Contains(this.ListMarcas.SelectedItem))
                    this.ListBoxMarcas.Items.Add(this.ListMarcas.SelectedItem);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnQuitarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxMarcas.Items.Remove(this.ListBoxMarcas.SelectedItem);

                //despues de borrar una marca dejo seleccionado el primero ya que con el required field validator si no hay uno seleccionado no te deja guardar
                if (ListBoxMarcas.Items.Count > 0)
                    ListBoxMarcas.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioTecnico st = new ServicioTecnico();
                List<int> stList = new List<int>();

                SetearServicioTecnico(st);

                foreach (ListItem item in ListBoxMarcas.Items)
                {
                    stList.Add(Convert.ToInt32(item.Value));
                }
                
                var temp = contServTecEnt.AgregarServicioTecnico(st,stList);

                if(temp > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", m.mensajeBoxInfo("Servicio tecnico agregado correctamente!", "ServicioTecnicoF.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "error", m.mensajeBoxError("Error agregando servicio tecnico!"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando servicio tecnico. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error agregando servicio tecnico " + ex.Message);
            }
        }

        public void ModificarServicioTecnico()
        {
            try
            {
                var st = contServTecEnt.ObtenerServicioTecnicoByID(stID);
                txtNombre.Text = st.Nombre;
                txtDireccion.Text = st.Direccion;
                ListClientes.SelectedValue = contCliente.obtenerClienteID((int)st.Cliente).id.ToString();
                string numeroCelular = st.Telefono.Trim();

                if (numeroCelular.StartsWith("11"))
                {
                    txtCodArea.Text = "11";
                    txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 8));
                }
                else
                {
                    txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 6));
                    numeroCelular = numeroCelular.Replace(txtCelular.Text, string.Empty);
                    txtCodArea.Text = numeroCelular;
                }

                txtObservaciones.Text = st.Observaciones;

                var marcas = contServTecEnt.ObtenerMarcasByIDServicioTecnico(stID);

                ListBoxMarcas.Items.Clear();

                foreach (var item in marcas)
                {
                    ListBoxMarcas.Items.Add(new ListItem(item.descripcion,item.id.ToString()));
                }

                ListBoxMarcas.Items[0].Selected = true;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando servicio tecnico. " + ex.Message));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioTecnico st = contServTecEnt.ObtenerServicioTecnicoByID(stID);

                SetearServicioTecnico(st);

                //List<string> marcas = new List<string>();
                List<int> marcas = new List<int>();

                foreach (ListItem item in ListBoxMarcas.Items)
                {
                    marcas.Add(Convert.ToInt32(item.Value));
                }

                var temp = contServTecEnt.ModificarServicioTecnico(st,marcas);

                if (temp > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", m.mensajeBoxInfo("Servicio tecnico modificado con exito!", "ServicioTecnicoF.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "error", m.mensajeBoxError("Error modificando servicio tecnico!"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando servicio tecnico. " + ex.Message));
            }
        }

        public void SetearServicioTecnico(ServicioTecnico st)
        {
            try
            {
                st.Nombre = txtNombre.Text;
                st.Direccion = txtDireccion.Text;
                st.Telefono = txtCodArea.Text + txtCelular.Text;
                st.Observaciones = txtObservaciones.Text;
                st.Cliente = contCliente.obtenerClienteID(Convert.ToInt32(ListClientes.SelectedValue)).id;
                st.Estado = 1;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error seteando campos de servicio tecnico " + ex.Message);
            }
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ServicioTecnicoF.aspx");
            }
            catch (Exception)
            {   

            }
        }        

        protected void btnAgregarClientes_Click(object sender, EventArgs e)
        {

        }
    }
}