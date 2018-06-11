using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMMutuales : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();
        controladorCliente contCliente = new controladorCliente();
        //valores
        private int valor;
        private int idMutual;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idMutual = Convert.ToInt32(Request.QueryString["id"]);
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    this.cargarClientes();

                    if (valor == 2)
                    {
                        this.cargarMutual(idMutual);
                    }                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "91")
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
        private void cargarMutual(int id)
        {
            try
            {
                Mutuale m = this.controlador.obtenerMutualByID(id);
                if (m != null)
                {
                    this.txtNombre.Text = m.Nombre;
                    this.txtDireccion.Text = m.Direccion;
                    this.txtTelefono.Text = m.Telefono;
                    this.txtObservacion.Text = m.Observaciones;
                    this.txtNumero.Text = m.NroPagare.ToString();
                    try
                    {
                        var cl = this.contCliente.obtenerClientesByClienteDT((int)m.Cliente);

                        this.ListCliente.DataSource = cl;
                        this.ListCliente.DataValueField = "id";
                        this.ListCliente.DataTextField = "alias";

                        this.ListCliente.DataBind();

                        this.ListCliente.SelectedValue = m.Cliente.ToString();
                    }
                    catch { }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando datos de Mututal. Excepción: " + Ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.valor > 0)
                    this.modificarMutual();
                else
                    this.agregarMutual();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Mututal. " + ex.Message));
            }

        }
        public void agregarMutual()
        {
            try
            {
                Mutuale m = new Mutuale();
                m.Nombre = this.txtNombre.Text;
                m.Direccion = this.txtDireccion.Text;
                m.Telefono = this.txtTelefono.Text;
                m.Observaciones = this.txtObservacion.Text;
                m.Comision = Convert.ToDecimal(this.txtComision.Text);
                m.NroPagare = Convert.ToInt32(this.txtNumero.Text);
                m.Estado = 1;
                m.Cliente = Convert.ToInt32(this.ListCliente.SelectedValue);

                int i = this.controlador.agregarMutual(m);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego Mutual : " + m.Nombre);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Mutual agregada con exito!. \", {type: \"info\"});location.href = 'MutualesF.aspx';", true);
                    this.borrarCampos();                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Mutual. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }
        public void modificarMutual()
        {
            try
            {
                Mutuale m = this.controlador.obtenerMutualByID(this.idMutual);

                if (String.IsNullOrEmpty(this.txtComision.Text))
                    this.txtComision.Text = "0.00";

                m.Nombre = this.txtNombre.Text;
                m.Direccion = this.txtDireccion.Text;
                m.Telefono = this.txtTelefono.Text;
                m.Observaciones = this.txtObservacion.Text;
                m.Comision = Convert.ToDecimal(this.txtComision.Text);
                m.NroPagare = Convert.ToInt32(this.txtNumero.Text);
                m.Cliente = Convert.ToInt32(this.ListCliente.SelectedValue);
                int i = this.controlador.modificarMutual(m);
                if (i >= 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Mutual : " + m.Nombre);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Mutual modificada con exito!. \", {type: \"info\"});location.href = 'MutualesF.aspx';", true);
                    this.borrarCampos();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Mutual. \", {type: \"error\"});", true);
                }
            }
            catch
            {

            }
        }
        public void borrarCampos()
        {
            try
            {
                this.txtNombre.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }
        protected void btnBuscarCodigoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contCliente.obtenerClientesAliasDT(buscar);

                if (dtClientes != null)
                {
                    if (dtClientes.Rows.Count > 1)
                    {
                        DataRow dr = dtClientes.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["Id"] = "-1";
                        dtClientes.Rows.InsertAt(dr, 0);
                    }

                    //Cargo la lista
                    this.ListCliente.DataSource = dtClientes;
                    this.ListCliente.DataValueField = "id";
                    this.ListCliente.DataTextField = "alias";
                    this.ListCliente.DataBind();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error buscando cliente. Excepción: " + Ex.Message));
            }
        }
        protected void ListCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cargarClientes()
        {
            try
            {
                var dt = contCliente.obtenerClientesDT();

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["alias"] = "Seleccione...";
                        dr["id"] = -1;
                        dt.Rows.InsertAt(dr, 0);
                    }

                    this.ListCliente.DataSource = dt;
                    this.ListCliente.DataValueField = "id";
                    this.ListCliente.DataTextField = "alias";

                    this.ListCliente.DataBind();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando clientes a la lista."));
                }
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando clientes a la lista. Excepción: " + Ex.Message));
            }
        }

    }
}