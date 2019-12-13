using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Millas_Api.Controladores;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class Clientesaspx : System.Web.UI.Page
    {
        private controladorCliente contCliente = new controladorCliente();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        controladorTipoCliente contTipoCliente = new controladorTipoCliente();
        controladorUsuario contUser = new controladorUsuario();
        ControladorProvincias contProvincias = new ControladorProvincias();
        controladorEstadoCliente contEstadoCliente = new controladorEstadoCliente();
        Mensajes m = new Mensajes();
        public Dictionary<string, string> camposClientes = null;

        int accion;
        string busqueda;
        int tipoCliente;
        string provincia;
        int idVendedor;
        int idGrupoCliente;
        int idEstadoCliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.busqueda = Request.QueryString["b"];

                this.tipoCliente = Convert.ToInt32(Request.QueryString["tc"]);
                this.provincia = Request.QueryString["pr"];
                this.idVendedor = Convert.ToInt32(Request.QueryString["v"]);
                this.idGrupoCliente = Convert.ToInt32(Request.QueryString["gc"]);
                this.idEstadoCliente = Convert.ToInt32(Request.QueryString["ec"]);

                this.cargarVendedor();

                if (!IsPostBack)
                {
                    //Datos carga rapida cliente
                    this.generarCodigo();
                    this.cargarVendedores();
                    this.cargarTipoClientes();
                    this.cargarGrupoClientes();
                    this.cargarIvaClientes();
                    this.cargarListasPrecios();
                    this.cargarFormaPAgo();
                    this.cargarSucursal();
                    this.CargarGruposClientes();
                    this.cargarDropList_ModalBusqueda();
                    this.CargarEstadosClientes();
                    this.ListSucursalesMillas.SelectedValue = Session["LogIn_SucUser"].ToString();
                }
                //filtro
                if (this.accion == 2)
                {
                    this.filtrar(tipoCliente, provincia, idVendedor, idGrupoCliente, idEstadoCliente);
                }
                else
                {
                    this.cargarClientes();
                }

                this.txtBusqueda.Focus();
                Page.Form.DefaultButton = this.lbBuscar.UniqueID;
                this.verificarConfiguracionIva();
                this.verificarPermisoCtaCte();
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Clientes.Clientes") != 1)
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
                        //if (s == "9" || s =="20")
                        if (s == "20")
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
        private void verificarPermisoCtaCte()
        {
            try
            {
                //verifico si puede
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                string permiso2 = listPermisos.Where(x => x == "103").FirstOrDefault();
                if (permiso2 == null)
                {
                    try
                    {
                        this.DropListFormaPagoAR.Items.Remove(this.DropListFormaPagoAR.Items.FindByText("Cuenta Corriente"));
                    }
                    catch { }
                }

            }
            catch
            {

            }
        }
        public void cargarVendedor()
        {
            try
            {
                controladorVendedor contVendedor = new controladorVendedor();
                //DataTable dt = contVendedor.obtenerVendedores();
                int idSucursal = (int)Session["Login_SucUser"];
                DataTable dt = contVendedor.obtenerVendedores();// obtenerVendedoresBySuc(idSucursal);

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    DropListVendedores.Items.Add(item);

                    ListVendedores_ModalBusqueda.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                this.ListSucursalesMillas.DataSource = dt;
                this.ListSucursalesMillas.DataValueField = "Id";
                this.ListSucursalesMillas.DataTextField = "nombre";
                this.ListSucursalesMillas.DataBind();
                this.ListSucursalesMillas.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                this.ListSucursalesMillas.Items.Insert(1, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarDropList_ModalBusqueda()
        {
            cargarDropList_TipoCliente();
            cargarDropList_Provincias();
        }
        public void cargarDropList_TipoCliente()
        {
            try
            {
                DataTable dt = contTipoCliente.obtenerTiposClientes();

                this.DropListTipoCliente.DataSource = dt;
                this.DropListTipoCliente.DataValueField = "id";
                this.DropListTipoCliente.DataTextField = "tipo";

                this.DropListTipoCliente.DataBind();
                this.DropListTipoCliente.Items.Insert(0, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargarDropList_TipoCliente. " + ex.Message));
            }
        }
        public void cargarDropList_Provincias()
        {
            try
            {
                var dt = contProvincias.ObtenerProvincias().OrderBy(x => x.Provincia1).ToList();

                this.ListProvincias_ModalBusqueda.DataSource = dt;
                this.ListProvincias_ModalBusqueda.DataValueField = "Id";
                this.ListProvincias_ModalBusqueda.DataTextField = "Provincia1";

                this.ListProvincias_ModalBusqueda.DataBind();
                this.ListProvincias_ModalBusqueda.Items.Insert(0, new ListItem("Todas", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargarDropList_Provincias. " + ex.Message));
            }
        }
        /// <summary>
        /// Carga los clientes en la tabla
        /// </summary>
        private void cargarClientes()
        {
            try
            {
                this.phClientes.Controls.Clear();

                List<Cliente> clientes = new List<Cliente>();

                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil == "Distribuidor")
                {
                    var idDistribuidor = (int)Session["Login_Vendedor"];
                    if (this.accion > 0 && !String.IsNullOrEmpty(this.busqueda))
                    {
                        clientes = this.contCliente.obtenerClientesAliasDistribuidor(this.busqueda, idDistribuidor);
                        var cliAux = this.contCliente.obtenerClienteID(idDistribuidor);
                        clientes.Add(cliAux);
                    }
                    else
                    {
                        clientes = this.contCliente.obtenerClientesReducDistribuidor(1, idDistribuidor);
                        var cliAux = this.contCliente.obtenerClienteID(idDistribuidor);
                        clientes.Add(cliAux);
                    }
                }
                else
                {

                    if (this.accion > 0 && !String.IsNullOrEmpty(this.busqueda))
                    {
                        clientes = this.contCliente.obtenerClientesAlias(this.busqueda);
                    }
                    else
                    {
                        clientes = this.contCliente.obtenerClientesReduc(1);
                    }
                }
                cargarClientesAlPlaceholder(clientes);
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarClientesTable(List<Cliente> clientes)
        {
            //vacio place holder
            phClientes.Controls.Clear();

            //para cargar el cliente
            foreach (Cliente cl in clientes)
            {
                TableCell celCodigo = new TableCell();
                celCodigo.Text = cl.codigo;
                celCodigo.Width = Unit.Percentage(5);
                celCodigo.VerticalAlign = VerticalAlign.Middle;

                TableCell celRazonSocial = new TableCell();
                celRazonSocial.Text = cl.razonSocial;
                celRazonSocial.Width = Unit.Percentage(25);
                celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                TableCell celAlias = new TableCell();
                celAlias.Text = cl.alias;
                celAlias.Width = Unit.Percentage(25);
                celAlias.VerticalAlign = VerticalAlign.Middle;

                TableCell celTipo = new TableCell();
                celTipo.Text = cl.tipoCliente.descripcion;
                celTipo.Width = Unit.Percentage(15);
                celTipo.VerticalAlign = VerticalAlign.Middle;

                TableCell celCuit = new TableCell();
                celCuit.Text = formatearCuit(cl.cuit);
                celCuit.Width = Unit.Percentage(15);
                celCuit.VerticalAlign = VerticalAlign.Middle;

                TableCell celImage = new TableCell();
                LinkButton btnDetails = new LinkButton();
                btnDetails.ID = cl.id.ToString();
                btnDetails.CssClass = "btn btn-info ui-tooltip";
                btnDetails.Attributes.Add("data-toggle", "tooltip");
                btnDetails.Attributes.Add("title data-original-title", "Editar");
                btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnDetails.PostBackUrl = "ClientesABM.aspx?accion=2&id=" + cl.id.ToString();
                //btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);
                celImage.Controls.Add(btnDetails);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celImage.Controls.Add(l2);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + cl.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + cl.id + ");";
                celImage.Controls.Add(btnEliminar);

                Literal l3 = new Literal();
                l3.Text = "&nbsp";
                celImage.Controls.Add(l3);

                LinkButton btnMillas = new LinkButton();
                btnMillas.ID = "btnMillas_" + cl.id;
                int existe = this.verificarExisteMillas(cl.id);
                if (existe > 0)
                {
                    if (existe == 2)
                    {
                        btnMillas.CssClass = "btn btn-success ui-tooltip";
                    }
                    else
                    {
                        btnMillas.CssClass = "btn btn-success ui-tooltip";
                    }
                }
                else
                {
                    btnMillas.CssClass = "btn btn-default ui-tooltip";
                }

                btnMillas.Attributes.Add("data-toggle", "tooltip");
                btnMillas.Attributes.Add("title data-original-title", "Sist. Millas");
                btnMillas.Text = "<span class='shortcut-icon icon-credit-card'></span>";
                btnMillas.Click += new EventHandler(this.cargarInfoMillas);
                celImage.Controls.Add(btnMillas);

                celImage.Width = Unit.Percentage(15);

                //fila
                TableRow tr = new TableRow();
                tr.ID = cl.id + "_1";

                //agrego celda a filas
                //tr.Cells.Add(celCuit);
                tr.Cells.Add(celCodigo);
                tr.Cells.Add(celRazonSocial);
                tr.Cells.Add(celAlias);
                tr.Cells.Add(celTipo);
                tr.Cells.Add(celCuit);
                tr.Cells.Add(celImage);
                //arego fila a tabla
                this.phClientes.Controls.Add(tr);

            }
            //agrego la tabla al placeholder

        }

        private void buscar(string alias)
        {
            try
            {
                List<Cliente> cliente = this.contCliente.obtenerClientesAlias(alias);
                this.cargarClientesTable(cliente);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Clientesaspx.aspx?accion=1&b=" + this.txtBusqueda.Text);
                //if(!String.IsNullOrEmpty(this.txtBusqueda.Text))
                //{
                //    this.buscar(this.txtBusqueda.Text);
                //}
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idCliente = Convert.ToInt32(this.txtMovimiento.Text);

                int i = this.contCliente.eliminarCliente(idCliente);
                if (i == 1)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Cliente: " + idCliente);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cliente eliminado con exito", null));
                    this.cargarClientes();
                }
                if (i == 2)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Intento de baja de cliente: " + idCliente + " No fue posible. El cliente tiene saldo. ");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El cliente Tiene saldo positivo en Cuenta Corriente no se puede eliminar. ", null));
                    this.cargarClientes();
                }
                if (i <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo borrar Cliente"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Cliente. " + ex.Message));
            }
        }

        private string formatearCuit(string cuit)
        {
            try
            {
                if (cuit.Length == 11)
                {
                    string r = cuit.Substring(0, 2) + "-" + cuit.Substring(2, 8) + "-" + cuit.Substring(10, 1);
                    return r;
                }

            }
            catch (Exception ex)
            {

            }
            return cuit;
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtBusqueda.Text != "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Clientes/ImpresionClientes.aspx?Cli=" + this.txtBusqueda.Text + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    //Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?Cli="+ this.txtBusqueda.Text);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Clientes/ImpresionClientes.aspx?Cli=" + this.busqueda + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                    //Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            if (this.txtBusqueda.Text != "")
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Clientes/ImpresionClientes.aspx?Cli=" + this.txtBusqueda.Text + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?Cli=" + this.txtBusqueda.Text + "&ex=1");
            }
            else
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Clientes/ImpresionClientes.aspx?ex=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?ex=1");
            }
        }

        protected void btnReporteClientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?a=1&ex=1&ven=" + DropListVendedores.SelectedValue);
        }
        protected void lbtnExportarZona_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?a=3&ex=1");
        }

        protected void lbtnExportarProvincia_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Formularios/Clientes/ImpresionClientes.aspx?a=2&ex=1");
            }
            catch
            {

            }
        }

        #region millas
        private void cargarInfoMillas(object sender, EventArgs e)
        {
            try
            {
                ControladorSocios contSocios = new ControladorSocios();
                ControladorClienteEntity contCliEntity = new ControladorClienteEntity();

                string id = (sender as LinkButton).ID.ToString().Split('_')[1];
                Cliente cl = this.contCliente.obtenerClienteID(Convert.ToInt32(id));
                this.lblIdClienteMillas.Text = id;

                int puede = this.verificarPermisoSistemaMillas();
                if (puede <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"No tiene permisos para realizar esta accion.\");", true);
                    return;
                }

                int existe = this.verificarExisteMillas(Convert.ToInt32(id));
                if (existe <= 0)//si no existe
                {

                    if (cl != null)
                    {
                        string cuitDoc = cl.cuit;
                        string dniReal = "";
                        if (cuitDoc.Length >= 11)//cuit con guiones
                        {
                            dniReal = cuitDoc.Substring(2, 8);
                        }
                        else//dni solo
                        {
                            dniReal = cuitDoc.PadLeft(8, '0');
                        }
                        this.txtNombreMillas.Text = cl.alias;
                        this.txtApellidoMillas.Text = cl.alias.Split(' ')[0];
                        this.txtDNIMillas.Text = dniReal;

                        var mail = contCliEntity.obtenerClienteDatosByCliente(Convert.ToInt32(id));
                        if (mail != null && mail.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(mail.FirstOrDefault().Celular))
                            {
                                if (mail.FirstOrDefault().Celular.Contains('-'))
                                {
                                    this.txtCodAreaMillas.Text = mail.FirstOrDefault().Celular.Split('-')[0];
                                    this.txtTelefonoMillas.Text = mail.FirstOrDefault().Celular.Split('-')[1];
                                    if (mail.FirstOrDefault().FechaNacimiento != null)
                                    {
                                        this.txtFechaNacMillas.Text = mail.FirstOrDefault().FechaNacimiento.Value.ToString("dd/MM/yyyy");
                                    }
                                }
                            }
                            this.txtMailMillas.Text = mail.FirstOrDefault().Mail;
                        }

                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "abrirModalMillas();", true);
                    }
                }
                else//si existe
                {
                    string cuitDoc = cl.cuit;
                    string dniReal = "";
                    if (cuitDoc.Length >= 11)//cuit con guiones
                    {
                        dniReal = cuitDoc.Substring(2, 8);
                    }
                    else//dni solo
                    {
                        dniReal = cuitDoc.PadLeft(8, '0');
                    }
                    var socio = contSocios.obtenerSocioByDoc(dniReal);

                    int user = (int)Session["Login_IdUser"];
                    Usuario u = this.contUser.obtenerUsuariosID(user);
                    string url = WebConfigurationManager.AppSettings.Get("UrlMillas");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "window.open('" + url + "Login.aspx?u=" + u.usuario + "&p=" + u.contraseña + "&s=" + socio.Id + "','_blank');", true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private int verificarExisteMillas(int cliente)
        {
            try
            {
                ControladorSocios contSocios = new ControladorSocios();
                Cliente cl = this.contCliente.obtenerClienteID(cliente);
                string cuitDoc = cl.cuit.Replace("-", "");
                string dniReal = "";
                if (cuitDoc.Length >= 11)//cuit con guiones
                {
                    dniReal = cuitDoc.Substring(2, 8);
                }
                else//dni solo
                {
                    dniReal = cuitDoc.PadLeft(8, '0');
                }
                var existe = contSocios.obtenerSocioByDoc(dniReal);
                if (existe == null)//si no existe
                {
                    return 0;
                }
                else
                {
                    if (existe.Tarjetas.Count > 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }
        protected void lbtnAgregarSistMillas_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorSocios contSocios = new ControladorSocios();

                Usuario user = this.contUser.obtenerUsuariosID((int)Session["Login_IdUser"]);

                string origen = WebConfigurationManager.AppSettings.Get("OrigenSMS");

                Millas_Api.Entity.Socio socio = new Millas_Api.Entity.Socio();
                socio.Nombre = this.txtNombreMillas.Text;
                socio.Apellido = this.txtApellidoMillas.Text;
                socio.Celular = this.txtCodAreaMillas.Text + this.txtTelefonoMillas.Text;
                socio.Documento = this.txtDNIMillas.Text;
                socio.Mail = this.txtMailMillas.Text;
                socio.FechaAlta = DateTime.Now;
                socio.FechaModificacion = DateTime.Now;
                socio.Origen = Convert.ToInt32(origen);
                socio.FechaNamiento = Convert.ToDateTime(this.txtFechaNacMillas.Text, new CultureInfo("es-AR"));
                socio.Vendedore = new Millas_Api.Entity.Vendedore();
                socio.Vendedore.Vendedor = user.usuario;
                socio.Sucursale = new Millas_Api.Entity.Sucursale();
                socio.Sucursale.Sucursal = this.ListSucursalesMillas.SelectedItem.Text;
                socio.Estado = 1;
                socio.Extra = this.lblIdClienteMillas.Text;

                this.modificarDatosCliente(Convert.ToInt32(this.lblIdClienteMillas.Text));

                int ok = contSocios.agregrarSocio(socio, socio.Vendedore.Vendedor, socio.Sucursale.Sucursal);
                if (ok > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Guardados con exito", Request.Url.ToString()));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Guardados con exito.\", {type: \"info\"});", true);                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo guardar."));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"No se pudo guardar.\", {type: \"error\"});", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error." + ex.Message));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Ocurrio un error. " + ex.Message + ".\", {type: \"error\"});", true);
            }
        }
        private void modificarDatosCliente(int id)
        {
            try
            {
                var datosMail = this.contClienteEntity.obtenerClienteDatosByCliente(id);
                if (datosMail != null)
                {
                    if (datosMail.Count > 0)
                    {
                        datosMail.FirstOrDefault().Celular = this.txtCodAreaMillas.Text + "-" + this.txtTelefonoMillas.Text;
                        if (!String.IsNullOrEmpty(this.txtFechaNacMillas.Text))
                        {
                            datosMail.FirstOrDefault().FechaNacimiento = Convert.ToDateTime(this.txtFechaNacMillas.Text, new CultureInfo("es-AR"));
                        }
                        this.contClienteEntity.modificarClienteDatos(datosMail.FirstOrDefault());
                    }
                    else
                    {
                        Cliente_Datos datos = new Cliente_Datos();
                        datos.Mail = this.txtMailMillas.Text;
                        datos.IdCliente = id;
                        datos.Celular = this.txtCodAreaMillas.Text + "-" + this.txtTelefonoMillas.Text;
                        if (!String.IsNullOrEmpty(this.txtFechaNacMillas.Text))
                        {
                            datos.FechaNacimiento = Convert.ToDateTime(this.txtFechaNacMillas.Text, new CultureInfo("es-AR"));
                        }
                        datosMail.Add(datos);
                        this.contClienteEntity.agregarClienteDatos(datos);
                    }

                }
            }
            catch
            {

            }
        }
        private int verificarPermisoSistemaMillas()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "105")
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
        protected void lbtnEnviarCodigoMillas_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorPlenario contPlena = new ControladorPlenario();

                if (this.txtCodAreaMillas.Text.Length + this.txtTelefonoMillas.Text.Length != 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");desbloquearEnvioCod();", true);
                    return;
                }
                if (String.IsNullOrEmpty(this.txtDNIMillas.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Debe completar el campo DNI !. \");desbloquearEnvioCod();", true);
                    return;
                }

                string dni = this.txtDNIMillas.Text;
                string telefono = "+549" + this.txtCodAreaMillas.Text + this.txtTelefonoMillas.Text;//+54 9 cod + tel

                int ok = contPlena.validarTelefonoDNI(dni, telefono);
                if (ok > 0)
                {
                    CodigosTelefono registro = new CodigosTelefono();
                    registro.DNI = dni;
                    registro.Telefono = telefono;
                    registro.IdEmpresa = (int)Session["Login_EmpUser"];
                    registro.IdSucursal = (int)Session["Login_SucUser"];
                    registro.IdVendedor = (int)Session["Login_IdUser"];
                    int envioCodigo = contPlena.enviarCodigoTelefono(registro);
                    this.lblIdRegistro.Text = registro.Id.ToString();
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo enviado. \", {type: \"info\"});desbloquearEnvioCod();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Este telefono ya fue utilizado con otro DNI!. \");desbloquearEnvioCod();", true);
                }
            }
            catch
            {

            }
        }
        protected void lbtnValidarCodigoMillas_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    ControladorPlenario contPlena = new ControladorPlenario();
                    string codigo = this.txtCodigoMillas.Text;
                    string dni = this.txtDNIMillas.Text;
                    string telefono = "+549" + this.txtCodAreaMillas.Text + this.txtTelefonoMillas.Text;//+54 9 cod + tel

                    if (this.txtCodAreaMillas.Text.Length + this.txtTelefonoMillas.Text.Length != 10)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo de area y/o numero invalido/s!. \");desbloquearEnvioCod();", true);
                        return;
                    }
                    if (String.IsNullOrEmpty(this.txtDNIMillas.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Debe completar el campo DNI !. \");desbloquearEnvioCod();", true);
                        return;
                    }

                    int ok = contPlena.validarCodigoVerificacion(dni, telefono, codigo);
                    if (ok > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo validado. \", {type: \"info\"});", true);
                        this.lblIdRegistro.Text = ok.ToString();
                        this.panelMillas.Visible = true;
                    }
                    else
                    {
                        if (ok == -2)
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Codigo incorrecto. \");", true);
                        else
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"No se pudo validar. \", {type: \"error\"});", true);

                        this.panelMillas.Visible = false;
                    }
                }
                catch
                {

                }

            }
            catch
            {

            }
        }
        protected void lbtnValidarLuegoMillas_Click(object sender, EventArgs e)
        {
            try
            {
                this.panelMillas.Visible = true;
            }
            catch
            {

            }
        }

        #endregion

        #region alta rapida cliente

        private void generarCodigo()
        {
            try
            {
                string p = this.contCliente.obtenerLastCodigoCliente();
                int newp = Convert.ToInt32(p);
                this.txtCodigoAR.Text = newp.ToString().PadLeft(6, '0');
            }
            catch
            {

            }
        }
        public void cargarVendedores()
        {
            try
            {
                ListVendedoresAR.Items.Clear();
                controladorVendedor contVendedor = new controladorVendedor();
                DataTable dt = contVendedor.obtenerVendedores();

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    ListVendedoresAR.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }
        public void cargarFormaPAgo()
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                DataTable dt = contFact.obtenerFormasPago();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["forma"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListFormaPagoAR.DataSource = dt;
                this.DropListFormaPagoAR.DataValueField = "id";
                this.DropListFormaPagoAR.DataTextField = "forma";

                this.DropListFormaPagoAR.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }
        private void cargarTipoClientes()
        {
            try
            {
                controladorTipoCliente contTipoCliente = new controladorTipoCliente();
                this.DropListTipoAR.DataSource = contTipoCliente.obtenerTiposClientes();
                this.DropListTipoAR.DataValueField = "id";
                this.DropListTipoAR.DataTextField = "tipo";

                this.DropListTipoAR.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista tipo cliente. " + ex.Message));
            }
        }
        private void cargarIvaClientes()
        {
            try
            {
                this.DropListIvaAR.DataSource = contCliente.obtenerIvaClientes();
                this.DropListIvaAR.DataValueField = "id";
                this.DropListIvaAR.DataTextField = "descripcion";

                this.DropListIvaAR.DataBind();
                ListItem ls = new ListItem();
                ls.Text = "Seleccione...";
                ls.Value = "-1";

                this.DropListIvaAR.Items.Insert(0, ls);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
            }
        }
        public void cargarListasPrecios()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerListaPrecios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListListaPreciosAR.DataSource = dt;
                this.ListListaPreciosAR.DataValueField = "id";
                this.ListListaPreciosAR.DataTextField = "nombre";

                this.ListListaPreciosAR.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        private void cargarGrupoClientes()
        {
            try
            {
                controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();
                this.DropListGrupoAR.DataSource = contGrupoCliente.obtenerGruposClientes();
                this.DropListGrupoAR.DataValueField = "id";
                this.DropListGrupoAR.DataTextField = "descripcion";

                this.DropListGrupoAR.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarClientesFamilia()
        {
            try
            {
                var idDistribuidor = (int)Session["Login_Vendedor"];
                var cliente = this.contCliente.obtenerClienteID(idDistribuidor);
                DataTable dt = this.contClienteEntity.obtenerLideresPorDistribuidor(idDistribuidor);
                this.DropListFamiliaAR.DataSource = dt;
                this.DropListFamiliaAR.DataValueField = "Id";
                this.DropListFamiliaAR.DataTextField = "razonSocial";
                this.DropListFamiliaAR.DataBind();

                this.DropListFamiliaAR.Items.Insert(0, new ListItem(cliente.alias, cliente.id.ToString()));

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando distribuidores a la lista. Excepcion: " + Ex.Message));
            }
        }
        private void verificarConfiguracionIva()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.siemprePRP == "1")
                {
                    this.DropListIvaAR.SelectedValue = "13";
                    this.DropListIvaAR.Attributes.Add("disabled", "true");
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void RespuestaAgregarCliente(int i, Cliente cliente)
        {
            try
            {

                switch (i)
                {
                    default:

                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Cliente: " + this.txtRazonAR.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Cliente agregado", "ClientesABM.aspx?accion=2&id=" + cliente.id));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar contacto"));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar direccion"));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente ya que ocurrio un error al agregar alerta"));
                        break;
                    case -5:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El Codigo del Cliente ya fue ingresado"));
                        cliente.codigo = txtCodigoAR.Text;
                        break;
                    case -6:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El CUIT del Cliente ya fue ingresado"));
                        cliente.cuit = txtCuitAR.Text;
                        break;
                    case 0:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Cliente"));
                        break;

                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ocurrio un error"));
            }
        }
        protected void btnAltaRapida_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cRapido = new Cliente();
                cRapido.codigo = this.txtCodigoAR.Text;
                cRapido.razonSocial = this.txtRazonAR.Text;
                cRapido.alias = this.txtRazonAR.Text;
                cRapido.tipoCliente.id = Convert.ToInt32(this.DropListTipoAR.SelectedValue);
                cRapido.tipoCliente.descripcion = this.DropListTipoAR.SelectedItem.Text;
                cRapido.grupo.id = Convert.ToInt32(this.DropListGrupoAR.SelectedValue);
                cRapido.categoria.id = 1;
                cRapido.cuit = this.txtCuitAR.Text;
                if (cRapido.cuit.Length < 11)
                    cRapido.cuit = cRapido.cuit.PadLeft(8, '0');

                cRapido.iva = this.DropListIvaAR.SelectedValue.ToString();
                cRapido.formaPago.id = Convert.ToInt32(this.DropListFormaPagoAR.SelectedValue);
                cRapido.vendedor.id = Convert.ToInt32(this.ListVendedoresAR.SelectedValue);
                cRapido.lisPrecio.id = Convert.ToInt32(this.ListListaPreciosAR.SelectedValue);
                cRapido.saldoMax = 0;
                cRapido.vencFC = 0;
                cRapido.descFC = 0;
                cRapido.observaciones = "";
                cRapido.hijoDe = 0;
                cRapido.sucursal.id = (int)Session["Login_SucUser"];
                cRapido.origen = 1;
                cRapido.alerta.descripcion = "";
                cRapido.alerta.idCliente = cRapido.id;
                cRapido.estado.id = 1;
                cRapido.pais.id = 1;

                if (this.contCliente.validateCuit(this.txtCuitAR.Text, this.DropListTipoAR.SelectedItem.Text))
                {
                    int i = this.contCliente.agregarCliente(cRapido);
                    cRapido.id = i;

                    if (i > 0)
                    {
                        //Verifico si utiliza modo distribución y si quien lo da de alta es dsitribuidor, se agrega al distribuidor como padre 
                        if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "Distribuidor")
                            {
                                var idDistribuidor = (int)Session["Login_Vendedor"];
                                Clientes_Referidos cr = new Clientes_Referidos();
                                if (DropListTipoAR.SelectedItem.Text.ToLower() == "lider")
                                {
                                    cr.Padre = idDistribuidor;
                                    cr.Hijo = i;
                                }
                                if (DropListTipoAR.SelectedItem.Text.ToLower() == "experta")
                                {
                                    cr.Padre = Convert.ToInt32(DropListFamiliaAR.SelectedValue);
                                    cr.Hijo = i;
                                }

                                this.contClienteEntity.agregarClienteReferido(cr);
                            }
                        }
                    }

                    this.RespuestaAgregarCliente(i, cRapido);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("CUIT invalido."));
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void DropListTipoAR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                {
                    if (DropListTipoAR.SelectedItem.Text.ToLower() == "experta")
                    {
                        this.cargarClientesFamilia();
                        PanelFamiliaAR.Visible = true;
                    }
                    else
                    {
                        PanelFamiliaAR.Visible = false;
                    }
                }

            }
            catch (Exception Ex)
            {

            }
        }

        protected void CargarGruposClientes()
        {
            try
            {
                controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();
                DataTable dt = contGrupoCliente.obtenerGruposClientes();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListGruposClientes_ModalBusqueda.DataSource = dt;
                this.ListGruposClientes_ModalBusqueda.DataValueField = "id";
                this.ListGruposClientes_ModalBusqueda.DataTextField = "descripcion";

                this.ListGruposClientes_ModalBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        protected void CargarEstadosClientes()
        {
            try
            {
                var dt = contEstadoCliente.obtenerEstadosClientes();

                this.ListEstadoCliente_ModalBusqueda.DataSource = dt;
                this.ListEstadoCliente_ModalBusqueda.DataValueField = "id";
                this.ListEstadoCliente_ModalBusqueda.DataTextField = "estadoCliente";

                this.ListEstadoCliente_ModalBusqueda.DataBind();

                if (ListEstadoCliente_ModalBusqueda.Items.Count >= 0)
                {
                    ListEstadoCliente_ModalBusqueda.Items.Insert(0, new ListItem("Todos", "0"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Clientesaspx.aspx?accion=2&tc=" + this.DropListTipoCliente.SelectedValue + "&pr=" + this.ListProvincias_ModalBusqueda.SelectedItem.Text + "&v=" + this.ListVendedores_ModalBusqueda.SelectedValue + "&gc=" + this.ListGruposClientes_ModalBusqueda.SelectedValue + "&ec=" + this.ListEstadoCliente_ModalBusqueda.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        public void filtrar(int idTipoCliente, string provincia, int idVendedor, int idGrupoCliente, int idEstadoCliente)
        {
            try
            {
                var clientes = contCliente.FiltrarClientesEnClientesAspx(idTipoCliente, provincia, idVendedor, idGrupoCliente, idEstadoCliente);
               
                List<Cliente> listName = clientes.AsEnumerable().Select(m => new Cliente()
                {
                    id = m.Field<int>("id"),
                    codigo = m.Field<string>("codigo"),
                    razonSocial = m.Field<string>("razonSocial"),
                    alias = m.Field<string>("alias"),
                    cuit = m.Field<string>("cuit"),
                }).ToList();

                cargarClientesAlPlaceholder(listName);
            }
            catch (Exception ex)
            {

            }
        }

        public void cargarClientesAlPlaceholder(List<Cliente> clientes)
        {
            try
            {
                foreach (Cliente cl in clientes)
                {
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = cl.codigo;
                    celCodigo.Width = Unit.Percentage(5);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celRazonSocial = new TableCell();
                    celRazonSocial.Text = cl.razonSocial;
                    celRazonSocial.Width = Unit.Percentage(25);
                    celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                    TableCell celAlias = new TableCell();
                    celAlias.Text = cl.alias;
                    celAlias.Width = Unit.Percentage(25);
                    celAlias.VerticalAlign = VerticalAlign.Middle;

                    List<contacto> contactos = this.contCliente.obtenerContactos(cl.id);
                    string mail = "";
                    string tel = "";
                    var contac = contactos.FirstOrDefault();
                    if (contac != null)
                    {
                        mail = contac.mail;
                        tel = contac.numero;
                    }
                    TableCell celMail = new TableCell();
                    celMail.Text = "<a href='mailto:" + mail + "' target='_top'>" + mail + "</a>";
                    celMail.Width = Unit.Percentage(7.5);
                    celMail.VerticalAlign = VerticalAlign.Middle;

                    TableCell celTelefono = new TableCell();
                    celTelefono.Text = tel;
                    celTelefono.Width = Unit.Percentage(7.5);
                    celTelefono.VerticalAlign = VerticalAlign.Middle;

                    TableCell celCuit = new TableCell();
                    celCuit.Text = this.formatearCuit(cl.cuit);
                    celCuit.Width = Unit.Percentage(15);
                    celCuit.VerticalAlign = VerticalAlign.Middle;

                    TableCell celImage = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = cl.id.ToString();
                    btnDetails.CssClass = "btn btn-info ui-tooltip";
                    btnDetails.Attributes.Add("data-toggle", "tooltip");
                    btnDetails.Attributes.Add("title data-original-title", "Editar");
                    btnDetails.Text = "<span class='shortcut-icon icon-search'></span>";
                    btnDetails.PostBackUrl = "ClientesABM.aspx?accion=2&id=" + cl.id.ToString();
                    celImage.Controls.Add(btnDetails);

                    string perfil = Session["Login_NombrePerfil"] as string;
                    if (perfil.ToLower() == "lider")
                    {
                        btnDetails.Visible = false;
                        btnAltaRapida.Visible = false;
                    }

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celImage.Controls.Add(l2);

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + cl.id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.OnClientClick = "abrirdialog(" + cl.id + ");";
                    celImage.Controls.Add(btnEliminar);

                    string sistema = WebConfigurationManager.AppSettings.Get("Millas");
                    if (!String.IsNullOrEmpty(sistema))
                    {

                        Literal l4 = new Literal();
                        l4.Text = "&nbsp";
                        celImage.Controls.Add(l4);

                        LinkButton btnMillas = new LinkButton();
                        btnMillas.ID = "btnMillas_" + cl.id;
                        int existe = this.verificarExisteMillas(cl.id);
                        if (existe > 0)
                        {
                            if (existe == 2)
                            {
                                btnMillas.CssClass = "btn btn-info ui-tooltip";
                            }
                            else
                            {
                                btnMillas.CssClass = "btn btn-success ui-tooltip";
                            }
                        }
                        else
                        {
                            btnMillas.CssClass = "btn btn-default ui-tooltip";
                        }
                        btnMillas.Attributes.Add("data-toggle", "tooltip");
                        btnMillas.Attributes.Add("title data-original-title", "Sistema beneficios");
                        btnMillas.Text = "<span class='shortcut-icon icon-credit-card'></span>";
                        btnMillas.Click += new EventHandler(this.cargarInfoMillas);
                        celImage.Controls.Add(btnMillas);

                        celImage.Width = Unit.Percentage(15);
                    }
                    else
                    {
                        celImage.Width = Unit.Percentage(10);
                    }

                    Literal l3 = new Literal();
                    l3.Text = "&nbsp";
                    celImage.Controls.Add(l3);

                    CheckBox cbSeleccion = new CheckBox();
                    cbSeleccion.ID = "Chk_IdCliente_" + cl.id;
                    cbSeleccion.CssClass = "btn btn-info";
                    cbSeleccion.Font.Size = 12;
                    celImage.Controls.Add(cbSeleccion);

                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = cl.id + "_1";

                    //agrego celda a filas
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celRazonSocial);
                    tr.Cells.Add(celAlias);
                    tr.Cells.Add(celMail);
                    tr.Cells.Add(celTelefono);
                    //tr.Cells.Add(celTelefono);
                    tr.Cells.Add(celCuit);
                    tr.Cells.Add(celImage);
                    //arego fila a tabla
                    this.phClientes.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {

            }
        }

        public void btnEnviarSMSUnSoloCliente_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorSMS contSMS = new ControladorSMS();

                foreach (Control C in phClientes.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[6] as CheckBox;
                    if (ch.Checked == true)
                    {
                        int idCliente = Convert.ToInt32(ch.ID.Split('_')[2]);
                        var cl = contClienteEntity.obtenerClienteDatosByIdCliente(idCliente);

                        if (cl != null)
                        {
                            if (!string.IsNullOrWhiteSpace(cl.Celular))
                            {
                                contSMS.enviarSMS(cl.Celular, txtEnviarSMS.Text, (int)Session["Login_IdUser"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}