using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Gestion_Web.Formularios.Seguridad
{
    public partial class ABMUsuarios : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        controladorUsuario controlador = new controladorUsuario();
        controladorVendedor contVendedor = new controladorVendedor();
        private int idUsuario;
        private int valor;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idUsuario = Convert.ToInt32(Request.QueryString["id"]);                

                if (!IsPostBack)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.cargarEmpresas();
                    this.cargarPerfiles();
                    this.cargarPerfilesStore();
                    this.cargarVendedores();
                    this.cargarClientes();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
                    this.cargarStores();                    
                }

                //esto va afuera del post back porque sino te pisa los datos de los textbox
                if (this.valor == 2)
                {
                    this.cargarUsuario(idUsuario);
                }

            }
            catch
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
                        if (s == "59")
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
        #region cargas iniciales
        public void cargarVendedores()
        {
            try
            {
                ListVendedores.Items.Clear();
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedores.Items.Add(item);
                }



                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }

        public void cargarClientes()
        {
            try
            {
                ListClientes.Items.Clear();
                controladorCliente contCliente = new controladorCliente();
                DataTable dt = contCliente.obtenerClientesDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListClientes.DataSource = dt;
                this.ListClientes.DataValueField = "id";
                this.ListClientes.DataTextField = "alias";

                this.ListClientes.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }

        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListEmpresa.DataSource = dt;
                this.DropListEmpresa.DataValueField = "Id";
                this.DropListEmpresa.DataTextField = "Razon Social";

                this.DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando empresas. " + ex.Message));
            }
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                //agrego todos
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListPtoVenta.DataSource = dt;
                this.DropListPtoVenta.DataValueField = "Id";
                this.DropListPtoVenta.DataTextField = "NombreFantasia";

                this.DropListPtoVenta.DataBind();
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPerfiles()
        {
            try
            {
                DataTable dt = controlador.obtenerPerfiles();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Perfil"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListPerfil.DataSource = dt;
                this.DropListPerfil.DataValueField = "Id";
                this.DropListPerfil.DataTextField = "Perfil";

                this.DropListPerfil.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Perfiles. " + ex.Message));
            }
        }
        public void cargarPerfilesStore()
        {
            try
            {
                Store_Api.Controladores.ControladorUsuario contStoreUsuario = new Store_Api.Controladores.ControladorUsuario();

                var perfilesStore = contStoreUsuario.obtenerPerfilesStore();

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
                controladorStore contStore = new controladorStore();

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
        #endregion
        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
            }
            catch
            {

            }
        }
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                DataTable dtClientes = contCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

                //cargo la lista
                this.ListClientes.DataSource = dtClientes;
                this.ListClientes.DataValueField = "id";
                this.ListClientes.DataTextField = "alias";
                this.ListClientes.DataBind();

            }
            catch (Exception ex)
            {

            }
        }
        private void cargarUsuario(int idUsuario)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                if (this.User != null)
                {
                    this.txtUsuario.Text = user.usuario;
                    this.txtContraseña.Text = user.contraseña;
                    this.DropListEmpresa.SelectedValue = user.empresa.id.ToString();
                    this.cargarSucursal(user.empresa.id);
                    this.DropListSucursal.SelectedValue = user.sucursal.id.ToString();
                    this.DropListPerfil.SelectedValue = user.perfil.id.ToString();
                    this.cargarPuntoVta(user.sucursal.id);
                    this.DropListPtoVenta.SelectedValue = user.ptoVenta.id.ToString();
                    if (this.DropListPerfil.SelectedItem.Text == "Vendedor")
                    {
                        this.panelVendedor.Visible = true;
                        this.ListVendedores.SelectedValue = user.vendedor.id.ToString(); 
                    }
                    if (this.DropListPerfil.SelectedItem.Text == "Cliente" || this.DropListPerfil.SelectedItem.Text == "Distribuidor")
                    {
                        this.panelClientes.Visible = true;
                        Gestor_Solution.Modelo.Cliente cli = contCliente.obtenerClienteID(user.vendedor.id);
                        //var temp = cli.id.ToString();

                        this.ListClientes.SelectedValue = cli.id.ToString();

                        if (this.ListClientes.SelectedValue == "-1")
                        {
                            this.cargarClienteEnLista(cli.id);
                        }

                        this.ListClientes.SelectedValue = cli.id.ToString();
                        //this.ListClientes.SelectedValue = user.vendedor.id.ToString(); 
                    }
                    //if (this.DropListPerfil.SelectedItem.Text == "Cliente")
                    //{
                    //    this.btnAgregarStore.Visible = true;
                    //}
                    VerificarEstadoAgregarStore();

                }
                else
                {
                    //Pop up 
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargar el Usuario"));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando campos del Usuario. " + ex.Message));
            }
        }
        private void cargarClienteEnLista(int idCliente)
        {
            controladorCliente contCliente = new controladorCliente();
            var c = contCliente.obtenerClienteID(idCliente);

            if (c != null)
            {
                this.ListClientes.Items.Add(new ListItem { Value = idCliente.ToString(), Text = c.alias });
                this.ListClientes.SelectedValue = idCliente.ToString();
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 1)
                {
                    this.agregarUsuario();
                }
                if (valor == 2)
                {
                    this.modificarUsuario();
                }
            }
            catch
            {

            }
        }

        private void agregarUsuario()
        {
            try
            {
                Usuario user = new Usuario();
                user.usuario = this.txtUsuario.Text;
                user.contraseña = this.txtContraseña.Text;
                user.sucursal.id = Convert.ToInt32(this.DropListSucursal.SelectedValue);
                user.empresa.id = Convert.ToInt32(this.DropListEmpresa.SelectedValue);
                user.perfil.id = Convert.ToInt32(this.DropListPerfil.SelectedValue);
                user.ptoVenta.id = Convert.ToInt32(this.DropListPtoVenta.SelectedValue);
                if(this.DropListPerfil.SelectedItem.Text == "Vendedor")
                {
                    user.vendedor.id = Convert.ToInt32(this.ListVendedores.SelectedValue);
                }
                if (this.DropListPerfil.SelectedItem.Text == "Cliente" || this.DropListPerfil.SelectedItem.Text == "Distribuidor" || this.DropListPerfil.SelectedItem.Text == "Lider" || this.DropListPerfil.SelectedItem.Text == "Experta")
                {
                    user.vendedor.id = Convert.ToInt32(this.ListClientes.SelectedValue);
                }
                user.estado = 1;
                int i = this.controlador.agregarUsuarios(user);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Alta Usuario: " + user.usuario);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Usuario agregado con exito", "ABMUsuarios.aspx?valor=1"));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Cliente agregado.\", {type: \"info\"});", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Cliente agregado.\", {type: \"info\"});", true);
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Usuario"));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando cliente.\", {type: \"error\"});", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregando cliente.\", {type: \"error\"});", true);
                }
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error agregando usuario. " + ex.Message);
            }
        }

        private void modificarUsuario()
        {
            try
            {
                Usuario user = new Usuario();
                user.id = this.idUsuario;
                user.usuario = this.txtUsuario.Text;
                user.contraseña = this.txtContraseña.Text;
                user.sucursal.id = Convert.ToInt32(this.DropListSucursal.SelectedValue);
                user.empresa.id = Convert.ToInt32(this.DropListEmpresa.SelectedValue);
                user.perfil.id = Convert.ToInt32(this.DropListPerfil.SelectedValue);
                user.ptoVenta.id = Convert.ToInt32(this.DropListPtoVenta.SelectedValue);

                if (this.DropListPerfil.SelectedItem.Text == "Vendedor")
                {
                    user.vendedor.id = Convert.ToInt32(this.ListVendedores.SelectedValue);
                }
                if (this.DropListPerfil.SelectedItem.Text == "Cliente" || this.DropListPerfil.SelectedItem.Text == "Distribuidor" || this.DropListPerfil.SelectedItem.Text == "Lider" || this.DropListPerfil.SelectedItem.Text == "Experta")
                {
                    user.vendedor.id = Convert.ToInt32(this.ListClientes.SelectedValue);
                }
                user.estado = 1;
                int i = this.controlador.modificarUsuarios(user);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Modifico Usuario: " + user.usuario);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Usuario modificado con exito", "UsuariosF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Usuario"));

                }
            }
            catch
            {

            }
        }

        protected void btnAgregarVendedor_Click(object sender, EventArgs e)
        {
            try
            {
                Empleado emp = new Empleado();
                emp.legajo = Convert.ToDecimal(this.txtLegajo.Text);
                emp.nombre = this.txtNombre.Text;
                emp.apellido = this.txtApellido.Text;
                emp.direccion = this.txtDireccion.Text;
                emp.dni = this.txtDni.Text;
                emp.fecNacimiento = Convert.ToDateTime(this.txtFechaNacimiento.Text, new CultureInfo("es-AR"));
                emp.fecIngreso = Convert.ToDateTime(this.txtFechaIngreso.Text, new CultureInfo("es-AR"));
                emp.cuitCuil = this.txtCuitVendedor.Text;
                emp.observaciones = this.txtObservacionesVendedor.Text;

                int i = this.contVendedor.agregarEmpleadoVendedor(emp, Convert.ToDecimal(this.txtComision.Text.Replace(',', '.'), CultureInfo.InvariantCulture));
                if (i > 0)
                {
                    this.txtLegajo.Text = "";
                    this.txtNombre.Text = "";
                    this.txtApellido.Text = "";
                    this.txtDireccion.Text = "";
                    this.txtDni.Text = "";
                    this.txtFechaNacimiento.Text = "";
                    this.txtFechaIngreso.Text = "";
                    this.txtCuitVendedor.Text = "";
                    this.txtObservacionesVendedor.Text = "";
                    this.txtComision.Text = "";
                    this.cargarVendedores();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puedo agregar Empleado "));
                }
            }
            catch
            {

            }
        }

        protected void DropListPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this.DropListPerfil.SelectedItem.Text == "Cliente")                
                //    this.btnAgregarStore.Visible = true;
                //else
                //    this.btnAgregarStore.Visible = false;
                VerificarEstadoAgregarStore();

                if (this.DropListPerfil.SelectedItem.Text == "Vendedor" || this.DropListPerfil.SelectedItem.Text == "Cliente" || this.DropListPerfil.SelectedItem.Text == "Distribuidor" || this.DropListPerfil.SelectedItem.Text == "Lider" || this.DropListPerfil.SelectedItem.Text == "Experta")
                {
                    if (this.DropListPerfil.SelectedItem.Text == "Cliente" || this.DropListPerfil.SelectedItem.Text == "Distribuidor" || this.DropListPerfil.SelectedItem.Text == "Lider" || this.DropListPerfil.SelectedItem.Text == "Experta")
                    {
                        //cargo clientes en vez de vendedores
                        this.panelClientes.Visible = true;
                        this.panelVendedor.Visible = false;
                    }
                    else
                    {
                        this.panelVendedor.Visible = true;
                        this.panelClientes.Visible = false;
                    }
                }
                else
                {
                    this.panelVendedor.Visible = false;
                    this.panelClientes.Visible = false;
                }

                //if(this.DropListPerfil.SelectedItem.Text == "Distribuidor" || this.DropListPerfil.SelectedItem.Text == "Lider" || this.DropListPerfil.SelectedItem.Text == "Experta")
                //{
                //    this.PanelFamilia.Visible = true;
                //}
                //else
                //{
                //    this.PanelFamilia.Visible = false;
                //}

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ocurrió un error al seleccionar perfil. Excepción: " + Ex.Message));
            }
        }

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarPuntoVta(Convert.ToInt32(this.DropListSucursal.SelectedValue));
            }
            catch
            {

            }
        }

        protected void btnAgregarStore_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                Gestor_Solution.Modelo.Cliente cliente = new Gestor_Solution.Modelo.Cliente();
                cliente = contCliente.obtenerClienteID(user.vendedor.id);

                Store_Api.Controladores.ControladorUsuario controladorUsuarioStore = new Store_Api.Controladores.ControladorUsuario();
                Store_Api.Entidades.Usuario usuarioStore = new Store_Api.Entidades.Usuario();
                usuarioStore = controladorUsuarioStore.obtenerUsuario(user.usuario);
                controladorStore contStore = new controladorStore();

                if (this.User != null && cliente != null)
                {
                    this.txtUsuarioStore.Enabled = false;
                    this.txtUsuarioStore.Text = user.usuario;
                    this.txtContraseñaStore.Text = user.contraseña;
                    this.txtNombreStore.Text = cliente.razonSocial;

                    if (cliente.contactos.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(cliente.contactos[0].numero))
                            this.txtTelefonoStore.Text = cliente.contactos[0].numero.ToString();
                        if (!String.IsNullOrEmpty(cliente.contactos[0].mail))
                            this.txtMailStore.Text = cliente.contactos[0].mail.ToString();
                    }

                    if (usuarioStore != null)
                    {
                        this.txtTelefonoStore.Text = usuarioStore.telefono;
                        this.txtMailStore.Text = usuarioStore.mail;
                        this.txtApellidoStore.Text = usuarioStore.apellido;
                        this.txtCoeficienteStore.Text = usuarioStore.coeficiente.ToString();
                        this.DropPerfilStore.SelectedValue = controladorUsuarioStore.obtenerPerfilesStorePorID((int)usuarioStore.perfil).@int.ToString();
                        this.DropStore.SelectedValue = contStore.ObtenerStoresPorID((int)usuarioStore.store).Id.ToString();
                    }                    
                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo cargar el Usuario"));
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModalAgregarUsuarioAlStore", "openModalAgregarUsuarioAlStore();", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando campos del Usuario Store." + ex.Message));
            }
            
        }

        public void btnAgregarUsuarioAlStore_Click(object sender, EventArgs e)
        {
            try
            {
                Log.EscribirSQL(1, "Info", "Voy a agregar un nuevo usuario al store");
                controladorCliente contCliente = new controladorCliente();
                Log.EscribirSQL(1, "Info", "voy a obtener el usuario");
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                Log.EscribirSQL(1, "Info", "Creo el cliente");
                Gestor_Solution.Modelo.Cliente cliente = new Gestor_Solution.Modelo.Cliente();
                Log.EscribirSQL(1, "Info", "Voy a obtener el cliente");
                cliente = contCliente.obtenerClienteID(user.vendedor.id);
                
                Store_Api.Controladores.ControladorUsuario controladorUsuarioStore = new Store_Api.Controladores.ControladorUsuario();
                Store_Api.Controladores.ControladorUsuario controladorUsuarioStore2 = new Store_Api.Controladores.ControladorUsuario("Store_Entities2");
                Store_Api.Entidades.Usuario usuarioStore = new Store_Api.Entidades.Usuario();

                CargarDatosUsuarioStore(usuarioStore, cliente);

                Log.EscribirSQL(1, "Info", "Cargue todos los datos, voy a modificar el usuario");
                if (usuarioStore.store == 1)
                {
                    AgregarOModificarUsuario(controladorUsuarioStore,user,usuarioStore,cliente);
                }
                else if (usuarioStore.store == 2)
                {
                    AgregarOModificarUsuario(controladorUsuarioStore2, user, usuarioStore,cliente);
                }

                Log.EscribirSQL(1, "Info", "Usuario agregado o modificado");                

                CargarUsuariosEnPH();

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error agregando usuario en el store." + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando usuario en el store." + ex.Message));
            }
        }

        public void AgregarOModificarUsuario(Store_Api.Controladores.ControladorUsuario controladorUsuarioStore,Usuario user, Store_Api.Entidades.Usuario usuarioStore, Gestor_Solution.Modelo.Cliente cliente)
        {
            try
            {
                int temp = 0;
                Store_Api.Entidades.Usuario usuarioStoreTemp = controladorUsuarioStore.obtenerUsuario(user.usuario);

                if (usuarioStoreTemp == null)
                {
                    temp = controladorUsuarioStore.agregarUsuarioStore(usuarioStore);
                    if (temp == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Usuario agregado con exito.", null));
                    }
                }
                else if (usuarioStoreTemp != null)
                {
                    if (usuarioStoreTemp.usuario1.ToLower().Trim() == usuarioStore.usuario1.ToLower().Trim())
                    {
                        CargarDatosUsuarioStore(usuarioStoreTemp, cliente);

                        temp = controladorUsuarioStore.ModificarUsuario(usuarioStoreTemp);

                        if (temp >= 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Usuario modificado con exito.", null));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Error modificando usuario.", null));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error agregando o modificando usuario en el store." + ex.Message);
            }           
            
        }

        public void CargarDatosUsuarioStore(Store_Api.Entidades.Usuario usuarioStore, Gestor_Solution.Modelo.Cliente cliente)
        {
            try
            {
                //si modifico griseo el campo usuario
                Log.EscribirSQL(1, "Info", "asigno los valores");
                usuarioStore.usuario1 = this.txtUsuarioStore.Text;
                Log.EscribirSQL(1, "Info", "1");
                usuarioStore.contraseña = this.txtContraseñaStore.Text;
                Log.EscribirSQL(1, "Info", "2");
                usuarioStore.nombre = this.txtNombreStore.Text;
                Log.EscribirSQL(1, "Info", "3");
                usuarioStore.apellido = this.txtApellidoStore.Text;
                Log.EscribirSQL(1, "Info", "4");
                usuarioStore.telefono = this.txtTelefonoStore.Text;
                Log.EscribirSQL(1, "Info", "5");
                usuarioStore.mail = this.txtMailStore.Text;
                Log.EscribirSQL(1, "Info", "6");
                usuarioStore.listaPrecios = cliente.lisPrecio.id;
                Log.EscribirSQL(1, "Info", "7");
                usuarioStore.idUsuario = idUsuario;
                Log.EscribirSQL(1, "Info", "8");
                usuarioStore.idCliente = cliente.id;
                Log.EscribirSQL(1, "Info", "9");
                usuarioStore.coeficiente = Convert.ToDecimal(this.txtCoeficienteStore.Text);
                Log.EscribirSQL(1, "Info", "10");
                usuarioStore.perfil = Convert.ToInt32(DropPerfilStore.SelectedValue);
                Log.EscribirSQL(1, "Info", "11");
                usuarioStore.store = Convert.ToInt32(DropStore.SelectedValue);

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error cargando datos de usuario store." + ex.Message);
            }            
        }

        public void VerificarEstadoAgregarStore()
        {
            try
            {
                var modoIntegradoGestion = WebConfigurationManager.AppSettings.Get("ModoIntegradoGestion");

                if (Convert.ToInt32(modoIntegradoGestion) == 1 && this.valor == 2 && this.DropListPerfil.SelectedItem.Text == "Cliente")
                {
                    PHUsuariosStore.Visible = true;
                    CargarUsuariosEnPH();
                    this.btnAgregarStore.Visible = true;
                }
                else
                {
                    PHUsuariosStore.Visible = false;
                    this.btnAgregarStore.Visible = false;
                }
                    
            }
            catch (Exception mensaje)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error verificando el usuario en el store." + mensaje.Message));
            }
        }

        public void CargarUsuariosEnPH()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();
                Usuario user = this.controlador.obtenerUsuariosID(idUsuario);
                Gestor_Solution.Modelo.Cliente cliente = new Gestor_Solution.Modelo.Cliente();
                cliente = contCliente.obtenerClienteID(user.vendedor.id);

                var controladorUsuarioStore = new Store_Api.Controladores.ControladorUsuario();
                var controladorUsuarioStore2 = new Store_Api.Controladores.ControladorUsuario("Store_Entities2");

                var usuarioStore = controladorUsuarioStore.obtenerUsuario(user.usuario);
                var usuarioStore2 = controladorUsuarioStore2.obtenerUsuario(user.usuario);

                controladorStore contStore = new controladorStore();

                this.PHUsuariosStoreTabla.Controls.Clear();

                //seteo-configuro el ph (esto esta hecho asi porque necesitamos crear 2 controladores diferentes para apuntar a los diferentes stores)
                if (usuarioStore != null)
                    SetearPH(usuarioStore, controladorUsuarioStore, contStore);
                if(usuarioStore2 != null)
                    SetearPH(usuarioStore2, controladorUsuarioStore2, contStore);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando usuario en el PH" + ex.Message));
                Log.EscribirSQL(1,"Error", "Error cargando usuario en el PH" + ex.Message);
                Log.EscribirSQL(1,"Error", "Error cargando usuario en el PH, inner exception" + ex.InnerException.Message);
            }
            
        }

        public void SetearPH(Store_Api.Entidades.Usuario usuarioStore, Store_Api.Controladores.ControladorUsuario controladorUsuarioStore, controladorStore contStore)
        {
            TableRow tr = new TableRow();

            TableCell celUsuario = new TableCell();
            celUsuario.Text = usuarioStore.usuario1;
            celUsuario.VerticalAlign = VerticalAlign.Middle;
            celUsuario.Width = Unit.Percentage(20);
            tr.Cells.Add(celUsuario);

            TableCell celContraseña = new TableCell();
            celContraseña.Text = usuarioStore.contraseña;
            celContraseña.VerticalAlign = VerticalAlign.Middle;
            celContraseña.Width = Unit.Percentage(20);
            tr.Cells.Add(celContraseña);

            TableCell celNombre = new TableCell();
            celNombre.Text = usuarioStore.nombre;
            celNombre.VerticalAlign = VerticalAlign.Middle;
            celNombre.Width = Unit.Percentage(20);
            tr.Cells.Add(celNombre);

            TableCell celApellido = new TableCell();
            celApellido.Text = usuarioStore.apellido;
            celApellido.VerticalAlign = VerticalAlign.Middle;
            celApellido.Width = Unit.Percentage(20);
            tr.Cells.Add(celApellido);

            TableCell celTelefono = new TableCell();
            celTelefono.Text = usuarioStore.telefono;
            celTelefono.VerticalAlign = VerticalAlign.Middle;
            celTelefono.Width = Unit.Percentage(20);
            tr.Cells.Add(celTelefono);

            TableCell celMail = new TableCell();
            celMail.Text = usuarioStore.mail;
            celMail.VerticalAlign = VerticalAlign.Middle;
            celMail.Width = Unit.Percentage(20);
            tr.Cells.Add(celMail);

            TableCell celCoeficiente = new TableCell();
            celCoeficiente.Text = usuarioStore.coeficiente.ToString();
            celCoeficiente.VerticalAlign = VerticalAlign.Middle;
            celCoeficiente.Width = Unit.Percentage(20);
            tr.Cells.Add(celCoeficiente);

            TableCell celPerfil = new TableCell();
            celPerfil.Text = controladorUsuarioStore.obtenerPerfilesStorePorID((int)usuarioStore.perfil).Perfil;
            celPerfil.VerticalAlign = VerticalAlign.Middle;
            celPerfil.Width = Unit.Percentage(20);
            tr.Cells.Add(celPerfil);

            TableCell celStore = new TableCell();
            celStore.Text = contStore.ObtenerStoresPorID((int)usuarioStore.store).Descripcion;
            celStore.VerticalAlign = VerticalAlign.Middle;
            celStore.Width = Unit.Percentage(20);
            tr.Cells.Add(celStore);

            TableCell celAction = new TableCell();
            celAction.Width = Unit.Percentage(20);

            Literal lDetail = new Literal();
            //lDetail.ID = usuarioStore.id.ToString();
            
            lDetail.Text = "<a href=\"UsuarioStoreABM.aspx?idUsuarioStore=" + usuarioStore.id.ToString() +  "&idUsuario=" + usuarioStore.idUsuario.ToString() + "&idStore=" + usuarioStore.store.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >";
            lDetail.Text += "<i class=\"shortcut-icon icon-search\"></i>";
            lDetail.Text += "</a>";
            celAction.Controls.Add(lDetail);

            Literal l2 = new Literal();
            l2.Text = "&nbsp";
            celAction.Controls.Add(l2);

            

            LinkButton btn_eliminar = new LinkButton();
            btn_eliminar.CssClass = "btn btn-info";
            btn_eliminar.Attributes.Add("data-toggle", "modal");
            //btn_eliminar.Attributes.Add("href", "#modalConfirmacion");
            btn_eliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
            btn_eliminar.OnClientClick = "abrirConfirmacion('" + usuarioStore.id + "_" + usuarioStore.store + "');";
            


            celAction.Controls.Add(btn_eliminar);

            tr.Cells.Add(celAction);            

            PHUsuariosStoreTabla.Controls.Add(tr);
        }

        private void BorrarUsuario(int idUsuarioStore, int idStore)
        {
            try
            {
                Store_Api.Controladores.ControladorUsuario contUsuario = new Store_Api.Controladores.ControladorUsuario();
                Store_Api.Controladores.ControladorUsuario contUsuario2 = new Store_Api.Controladores.ControladorUsuario("Store_Entities2");
                //obtengo numero factura
                
                if (idStore == 1)
                    contUsuario.BorrarUsuarioStorePorID(idUsuarioStore);
                else if (idStore == 2)
                    contUsuario2.BorrarUsuarioStorePorID(idUsuarioStore);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Usuario borrado con exito. ",null));

                Response.Redirect("ABMUsuarios.aspx?valor=2&id=" + idUsuario);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al borrar usuario del store. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al borrar usuario del store. " + ex.Message);
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            
            string[] atributos = txtIDUsuario.Text.Split('_');
            int idUsuarioStore = Convert.ToInt32(atributos[0]);
            int idStore = Convert.ToInt32(atributos[1]);

            BorrarUsuario(idUsuarioStore,idStore);
        }
    }
}