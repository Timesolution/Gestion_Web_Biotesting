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

namespace Gestion_Web.Formularios.Sucursales
{
    public partial class ABMSucursales : System.Web.UI.Page
    {
        controladorSucursal controlador = new controladorSucursal();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int valor;
        private int idSucursal;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                valor = Convert.ToInt32(Request.QueryString["valor"]);
                idSucursal = Convert.ToInt32(Request.QueryString["id"]);

                VerificarLogin();
                cargarSucursal();
                if (!IsPostBack)
                {
                    cargarClientes();
                    if (valor == 2)
                    {
                        CargarDatosSucursal();
                    }
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
                        if (s == "10")
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

        void CargarDatosSucursal()
        {
            controladorCliente controladorCliente = new controladorCliente();
            Sucursal s = controlador.obtenerSucursalID(idSucursal);
            txtNombre.Text = s.nombre;
            txtDireccion.Text = s.direccion;
            DropListClientes.SelectedValue = s.clienteDefecto.ToString();
            if (DropListClientes.SelectedValue == "-1")
            {
                var c = controladorCliente.obtenerClienteID(s.clienteDefecto);
                if (c != null)
                {
                    DropListClientes.Items.Add(new ListItem { Value = c.id.ToString(), Text = c.alias });
                    DropListClientes.SelectedValue = c.id.ToString();
                }
            }
            if (s.clienteDefecto == -2)
            {
                checkPrivada.Checked = true;
            }
        }

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Ninguno";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                //this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        private void cargarSucursal()
        {
            try
            {
                phSucursales.Controls.Clear();
                int empresa = (int)Session["Login_EmpUser"];
                List<Sucursal> sucursales = this.controlador.obtenerSucursalesList();
                foreach (Sucursal sucu in sucursales)
                {
                    if (sucu.empresa.id == empresa)
                    this.cargarSucursalesTable(sucu);
                }

            }
            catch
            {
 
            }
        }

        private void cargarSucursalesTable(Sucursal sucu)
        {
            try
            {
               
                TableRow tr = new TableRow();
                if (sucu.clienteDefecto == -2)
                {
                    tr.Attributes.Add("style", "color:red");
                }

                TableCell celNombre = new TableCell();
                celNombre.Text = sucu.nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(30);
                tr.Cells.Add(celNombre);

                TableCell celDireccion = new TableCell();
                celDireccion.Text = sucu.direccion;
                celDireccion.VerticalAlign = VerticalAlign.Middle;
                celDireccion.Width = Unit.Percentage(30);
                tr.Cells.Add(celDireccion);

                TableCell celEmpresa = new TableCell();
                Empresa emp = contr.obtenerEmpresa(sucu.empresa.id);
                celEmpresa.Text = emp.RazonSocial;
                celEmpresa.VerticalAlign = VerticalAlign.Middle;
                celEmpresa.Width = Unit.Percentage(25);
                tr.Cells.Add(celEmpresa);

                LinkButton btnPuntoVenta = new LinkButton();
                TableCell celPuntoVta = new TableCell();
                btnPuntoVenta.ID = "btnPuntoVenta_" + sucu.id.ToString();
                btnPuntoVenta.CssClass = "btn btn-info ui-tooltip";
                btnPuntoVenta.Attributes.Add("data-toggle", "tooltip");
                btnPuntoVenta.Attributes.Add("title data-original-title", "Punto de Venta");
                btnPuntoVenta.Text = "<span class='shortcut-icon icon-plus'></span>" + " PV";
                btnPuntoVenta.PostBackUrl = "../../Formularios/Sucursales/ABMPuntoVenta.aspx?codigo=" + sucu.id + "&empresa="+ sucu.empresa.id;
                celPuntoVta.Controls.Add(btnPuntoVenta);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celPuntoVta.Controls.Add(l2);


                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = sucu.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarSucursal);
                celPuntoVta.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celPuntoVta.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + sucu.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + sucu.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celPuntoVta.Controls.Add(btnEliminar);
                celPuntoVta.Width = Unit.Percentage(15);
                tr.Cells.Add(celPuntoVta);




                phSucursales.Controls.Add(tr);

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursal en la lista. " + ex.Message));
            }
        }
        
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try 
            {
                if(valor == 2)
                {
                    Sucursal suc = new Sucursal();
                    suc.id = this.idSucursal;
                    suc.nombre = this.txtNombre.Text;
                    suc.direccion = this.txtDireccion.Text;
                    suc.empresa.id = (int)Session["Login_EmpUser"];
                    suc.estado = 1;
                    if (checkPrivada.Checked)
                    {
                        suc.clienteDefecto = -2;
                    }
                    else
                    {
                        suc.clienteDefecto = Convert.ToInt32(this.DropListClientes.SelectedValue);
                    }
                    int i = this.controlador.editarSucursal(suc);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Sucursal: " + suc.nombre);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Sucursal modificada con exito", null));
                        this.cargarSucursal();
                        txtDireccion.Text = "";
                        txtNombre.Text = "";

                    }
                    else
                    {
                        //agrego mal
                    }
                }
                else
                {
                    Sucursal suc = new Sucursal();
                    suc.nombre = this.txtNombre.Text;
                    //suc.direccion.id = Convert.ToInt32(this.txtDireccion.Text);
                    suc.direccion = this.txtDireccion.Text;
                    suc.empresa.id = (int)Session["Login_EmpUser"];
                    if (checkPrivada.Checked)
                    {
                        suc.clienteDefecto = -2;
                    }
                    else
                    {
                        suc.clienteDefecto = Convert.ToInt32(this.DropListClientes.SelectedValue);
                    }
                    
                    //suc.puntoVenta = this.txtPuntoVenta.Text;
                    //suc.formaFacturar = this.ListFacturar.SelectedValue;

                    int i = this.controlador.agregarSucursal(suc);
                    if (i > 0)
                    {
                        //agrego bien
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Sucursal cargada con exito", null));
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Alta Sucursal: " + suc.nombre);
                        this.cargarSucursal();
                        txtDireccion.Text = "";
                        txtNombre.Text = "";

                    }
                    else
                    {
                        //agrego mal
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo agregar la sucursal . "));
                    }
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando y/o modificando sucursal . " + ex.Message));
            }
        }

        private void limpiarCampos()
        {
            try
            {
                this.txtNombre.Text = "";
                this.txtDireccion.Text = "";
                //this.txtPuntoVenta.Text = "";

            }
            catch
            { }
        }

        private void editarSucursal(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMSucursales.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar sucursales. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(this.txtMovimiento.Text);
                Sucursal sucu = this.controlador.obtenerSucursalID(idPerfil);
                sucu.estado = 0;
                int i = this.controlador.eliminarSucursal(sucu);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Sucursal: " + sucu.nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Sucursal eliminado con exito", null));
                    this.cargarSucursal();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Sucursal"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Sucursal. " + ex.Message));
            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = txtCodigoCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);
                controladorFactEntity controladorFacturasEntity = new controladorFactEntity();

                if (dtClientes == null)
                    return;

                DropListClientes.Items.Clear();
                DropListClientes.DataSource = dtClientes;
                DropListClientes.DataValueField = "id";
                DropListClientes.DataTextField = "alias";
                DropListClientes.SelectedValue = dtClientes.Rows[0]["id"].ToString();
                DropListClientes.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }
    }
}