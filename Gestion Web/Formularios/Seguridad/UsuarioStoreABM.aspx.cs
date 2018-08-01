using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Seguridad
{
    public partial class UsuarioStoreABM : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        private int idUsuario;
        private int idUsuarioStore;
        private int idStore;
        controladorUsuario controlador = new controladorUsuario();
        Store_Api.Controladores.ControladorUsuario controladorUsuarioStore;
        //Store_Api.Controladores.ControladorUsuario controladorUsuarioStore2 = new Store_Api.Controladores.ControladorUsuario("Store_Entities2");
        controladorCliente contCliente = new controladorCliente();
        controladorStore contStore = new controladorStore();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idUsuario = Convert.ToInt32(Request.QueryString["idUsuario"]);
                this.idUsuarioStore = Convert.ToInt32(Request.QueryString["idUsuarioStore"]);
                this.idStore = Convert.ToInt32(Request.QueryString["idStore"]);

                if(idStore==1)
                    controladorUsuarioStore = new Store_Api.Controladores.ControladorUsuario();
                else if (idStore==2)                
                    controladorUsuarioStore = new Store_Api.Controladores.ControladorUsuario("Store_Entities2");
                

                if (!IsPostBack)
                {
                    //this.idUsuario = (int)Session["Login_IdUser"];
                    cargarPerfilesStore();
                    cargarStores();
                    CargarDatosUsuario();
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
                    //if(this.contUser.validarAcceso(this.idUsuario,"Maestro.Articulos.Grupos") != 1)
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
                    //if (!String.IsNullOrEmpty(s))
                    //{
                    //    if (s == "15")
                    //    {
                    return 1;
                    //    }
                    //}
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        protected void CargarDatosUsuario()
        {
            try
            {
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                Gestor_Solution.Modelo.Cliente cliente = new Gestor_Solution.Modelo.Cliente();
                cliente = contCliente.obtenerClienteID(user.vendedor.id);

                Store_Api.Entidades.Usuario usuarioStore = new Store_Api.Entidades.Usuario();
                usuarioStore = controladorUsuarioStore.obtenerUsuario(user.usuario);
                CargarDatos(cliente, user, usuarioStore, controladorUsuarioStore, contStore);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al modificar usuario del store. " + ex.Message);
            }
        }

        public void CargarDatos(Gestor_Solution.Modelo.Cliente cliente, Usuario user, Store_Api.Entidades.Usuario usuarioStore, Store_Api.Controladores.ControladorUsuario controladorUsuarioStore, controladorStore contStore)
        {
            if (this.User != null && cliente != null)
            {
                this.DropStore.Enabled = false;
                this.DropStore.CssClass = "form-control";

                this.txtUsuarioStore.Enabled = false;
                this.txtUsuarioStore.CssClass = "form-control";

                this.txtTelefonoStore.Enabled = false;
                this.txtTelefonoStore.CssClass = "form-control";

                this.txtApellidoStore.Enabled = false;
                this.txtApellidoStore.CssClass = "form-control";

                this.DropPerfilStore.Enabled = false;
                this.DropPerfilStore.CssClass = "form-control";

                this.txtNombreStore.Enabled = false;
                this.txtNombreStore.CssClass = "form-control";

                //this.txtUsuarioStore.Text = user.usuario;
                //this.txtContraseñaStore.Text = user.contraseña;
                //this.txtNombreStore.Text = cliente.razonSocial;

                if (cliente.contactos.Count > 0)
                {
                    if (!String.IsNullOrEmpty(cliente.contactos[0].numero))
                        this.txtTelefonoStore.Text = cliente.contactos[0].numero.ToString();
                    if (!String.IsNullOrEmpty(cliente.contactos[0].mail))
                        this.txtMailStore.Text = cliente.contactos[0].mail.ToString();
                }

                if (usuarioStore != null)
                {
                    this.txtUsuarioStore.Text = usuarioStore.usuario1;
                    this.txtContraseñaStore.Text = usuarioStore.contraseña;
                    this.txtNombreStore.Text = usuarioStore.nombre;
                    this.txtTelefonoStore.Text = usuarioStore.telefono;
                    this.txtMailStore.Text = usuarioStore.mail;
                    this.txtApellidoStore.Text = usuarioStore.apellido;
                    this.txtCoeficienteStore.Text = usuarioStore.coeficiente.ToString();
                    this.DropPerfilStore.SelectedValue = controladorUsuarioStore.obtenerPerfilesStorePorID((int)usuarioStore.perfil).@int.ToString();
                    this.DropStore.SelectedValue = contStore.ObtenerStoresPorID(idStore).Id.ToString();
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargar el Usuario"));
            }
        }

        public void cargarPerfilesStore()
        {
            try
            {
                var perfilesStore = controladorUsuarioStore.obtenerPerfilesStore();

                perfilesStore.Insert(0, new Store_Api.Entidades.Perfile
                {
                    @int = 0,
                    Perfil = "Seleccione..."
                }
                );

                this.DropPerfilStore.DataSource = perfilesStore;
                this.DropPerfilStore.DataValueField = "int";
                this.DropPerfilStore.DataTextField = "Perfil";
                this.DropPerfilStore.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Perfiles. " + ex.Message));
            }
        }
        public void cargarStores()
        {
            try
            {
                var stores = contStore.ObtenerStores();

                stores.Insert(0, new Gestion_Api.Entitys.Store
                {
                    Id = 0,
                    Descripcion = "Seleccione..."
                }
                );

                this.DropStore.DataSource = stores;
                this.DropStore.DataValueField = "Id";
                this.DropStore.DataTextField = "Descripcion";
                this.DropStore.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Perfiles. " + ex.Message));
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.GuardarDatos();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al guardar usuario del store. " + ex.Message);
            }
        }

        public void GuardarDatos()
        {
            try
            {
                // obtengo el usuario que voy a modificar
                var usuarioStore = controladorUsuarioStore.obtenerUsuarioPorID(this.idUsuarioStore);

                //obtengo datos a modificar desde la pantalla
                usuarioStore = this.obtnerDatosDesdePantalla(usuarioStore);

                //guardo las modificaciones
                int res = controladorUsuarioStore.ModificarUsuario(usuarioStore);


                //verifico el error
                if (res >= 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Usuario modificado correctamente", "ABMUsuarios.aspx?valor=2&id=" + idUsuario)); //?id=" + idUsuario
                if (res == -2)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo guardar el Usuario"));

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al modificar usuario del store. " + ex.Message);
            }            
        }

        private Store_Api.Entidades.Usuario obtnerDatosDesdePantalla(Store_Api.Entidades.Usuario user)
        {
            try
            {                
                user.usuario1 = this.txtUsuarioStore.Text;
                user.mail = txtMailStore.Text;
                user.contraseña = txtContraseñaStore.Text;
                user.coeficiente = Convert.ToDecimal(txtCoeficienteStore.Text);
                return user;

                //if (usuarioStore != null)
                //{
                //    usuarioStore.telefono = txtTelefonoStore.Text;
                //    usuarioStore.mail = txtMailStore.Text;
                //    usuarioStore.apellido = txtApellidoStore.Text;
                    
                //    usuarioStore.perfil = Convert.ToInt32(DropPerfilStore.SelectedValue);
                //    usuarioStore.store = Convert.ToInt32(DropStore.SelectedValue);
                //}
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error obteniendo datos del usuario desde pantalla. " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Error obteniendo datos del usuario desde pantalla. " + ex.Message));
                return null;
            }
        }
    }
}