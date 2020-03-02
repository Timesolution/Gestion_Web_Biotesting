using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Vendedores
{
    public partial class VendedoresABM : System.Web.UI.Page
    {
        controladorEmpleado controladorEmpleado = new controladorEmpleado();
        controladorVendedor controlador = new controladorVendedor();
        controladorUsuario contUser = new controladorUsuario();
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorVendedorEntity contVendedorEnt = new ControladorVendedorEntity();
        Mensajes m = new Mensajes();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private int idVendedor;
        private string legajo;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.legajo = Request.QueryString["codigo"];                

                if (!IsPostBack)
                {
                    this.cargarSucursal();
                    this.cargarGruposArticulos();

                    //if (this.accion == 1)
                    //{
                    //    this.cargarEmpleados();
                    //}
                    if (this.accion == 2)
                    {
                        this.cargarVendedor();
                        this.UpdatePanelComision.Visible = true;
                    }
                }
                if (this.accion == 1)
                {
                    this.cargarEmpleados();
                }
                //if (this.accion == 2)
                //{
                //    this.cargarVendedor();
                //}
            }
            catch
            {

            }

            //}
        }

        #region Funciones Auxiliares
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
                        if (s == "13")
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
        #endregion

        #region Carga Inicial
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                // agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

                int idSuc = (int)Session["Login_SucUser"];
                this.DropListSucursal.SelectedValue = idSuc.ToString();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        private void cargarVendedor()
        {
            try
            {
                //reinicio
                //this.empleado = new Empleado();

                Vendedor ven = this.controlador.obtenerVendedorLegajo(Convert.ToDecimal(this.legajo));
                if (ven != null)
                {
                    //this.idTemp = this.articulo.id;
                    this.lblIdVendedor.Text = ven.id.ToString();
                    this.tLegajo.Value = ven.emp.legajo.ToString();
                    this.tNombre.Value = ven.emp.nombre;
                    this.tApellido.Value = ven.emp.apellido;
                    this.txtComision.Text = ven.comision.ToString();
                    this.DropListSucursal.SelectedValue = ven.sucursal.ToString();
                    this.cargarGruposComisionesVendedor();
                }
                else
                {
                    //Pop up 
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo cargar el vendedor"));
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void cargarGruposArticulos()
        {
            try
            {
                DataTable dt = contArticulo.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListGruposArticulos.DataSource = dt;
                this.ListGruposArticulos.DataValueField = "id";
                this.ListGruposArticulos.DataTextField = "descripcion";

                this.ListGruposArticulos.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error cargando grupos de articulos a la lista. Excepción: " + Ex.Message));
            }
        }
        private void cargarEmpleados()
        {
            try
            {
                List<Gestion_Api.Modelo.Empleado> empleados = this.controladorEmpleado.obtenerEmpleadosNoVendedoresReduc();
                //Table table = new Table();

                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                foreach (Gestion_Api.Modelo.Empleado emp in empleados)
                {
                    //fila
                    TableRow tr = new TableRow();

                    //Celdas
                    TableCell celLegajo = new TableCell();
                    celLegajo.Text = emp.legajo.ToString();
                    celLegajo.Width = Unit.Percentage(10);
                    celLegajo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celLegajo);

                    TableCell celNombre = new TableCell();
                    celNombre.Text = emp.nombre;
                    celNombre.Width = Unit.Percentage(40);
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNombre);

                    TableCell celApellido = new TableCell();
                    celApellido.Text = emp.apellido;
                    celApellido.Width = Unit.Percentage(20);
                    celApellido.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celApellido);

                    TableCell celAction = new TableCell();
                    Button btnDetails = new Button();
                    btnDetails.ID = i + "_" + emp.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "Seleccionar";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    btnDetails.Click += new EventHandler(this.seleccionarEmpleado);

                    celAction.Controls.Add(btnDetails);
                    celAction.Width = Unit.Percentage(20);
                    celAction.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celAction);

                    //cargo el primer cliente en detalle
                    //if (i == 0)
                    //{
                    //    this.cargarEmpleadoDetalle(emp.legajo);
                    //    //agrego el link al boton editar
                    //    this.linkEditar.HRef = "ArticulosABM.aspx?accion=2&codigo=" + emp.legajo;
                    //}

                    i++;

                    //arego fila a tabla
                    this.phVendedores.Controls.Add(tr);

                }
                //agrego la tabla al placeholder

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados. " + ex.Message));
            }
        }
        private void cargarGruposComisionesVendedor()
        {
            try
            {
                var vgc = this.contVendedorEnt.obtenerVendedores_GruposComisionesPorVendedor(Convert.ToInt32(this.lblIdVendedor.Text));
                if(vgc!=null)
                {
                    this.ListBoxGruposComisiones.DataSource = vgc;
                    this.ListBoxGruposComisiones.DataValueField = "Id";
                    this.ListBoxGruposComisiones.DataTextField = "GrupoComision";
                    this.ListBoxGruposComisiones.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando contacto. " + ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
                this.agregarVendedor();

            if (this.accion == 2)
                this.modificarVendedor();

        }
        protected void btnAgregarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorArticulosEntity contArticuloEnt = new ControladorArticulosEntity();

                Vendedores_GruposComisiones vgc = new Vendedores_GruposComisiones();
                vgc.Grupo = Convert.ToInt32(this.ListGruposArticulos.SelectedValue);
                vgc.Vendedor = Convert.ToInt32(this.lblIdVendedor.Text);
                int i = this.contVendedorEnt.agregarVendedor_GrupoComision(vgc);
                if (i > 0)
                {
                    decimal comision = contArticuloEnt.obtenerComisionDeGrupo(Convert.ToInt32(this.ListGruposArticulos.SelectedValue));
                    this.ListBoxGruposComisiones.Items.Add(this.ListGruposArticulos.SelectedItem.Text + " - " + comision);
                    this.ListGruposArticulos.SelectedIndex = 0;
                    this.cargarGruposComisionesVendedor();
                }
                if(i == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanelComision, UpdatePanelComision.GetType(), "alert", "$.msgbox(\"El vendedor ya tiene ese grupo asignado.\", {type: \"info\"}); location.href='VendedoresABM.aspx?accion=2&codigo=" + this.tLegajo.Value + "';", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo asignar el grupo al vendedor."));
                }
            }
            catch (Exception Ex)
            {

            }
        }
        protected void btnQuitarGrupoComision_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.contVendedorEnt.eliminarVendedor_GrupoComision(Convert.ToInt64(this.ListBoxGruposComisiones.SelectedValue));
                if (i > 0)
                {
                    this.ListBoxGruposComisiones.Items.Remove(this.ListBoxGruposComisiones.SelectedItem);
                    this.ListGruposArticulos.SelectedIndex = 0;
                    this.cargarGruposComisionesVendedor();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo quitar el grupo al vendedor."));
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrió un error eliminando el grupo al vendedor."));
            }
        }
        private void seleccionarEmpleado(object sender, EventArgs e)
        {
            try
            {
                //obtengo dato de id del botton
                string idBoton = (sender as Button).ID.ToString();
                int nroRow = Convert.ToInt32(idBoton.Split('_')[0]);
                int idEmpleado = Convert.ToInt32(idBoton.Split('_')[1]);

                TableRow tr = this.phVendedores.Controls[nroRow] as TableRow;

                //TextBox txtCantidad =  this.table.Rows[nroRow].Cells[5].Controls[0] as TextBox;
                this.tLegajo.Value = tr.Cells[0].Text;
                this.tNombre.Value = tr.Cells[1].Text;
                this.tApellido.Value = tr.Cells[2].Text;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos del empleado. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando datos del empleado. " + ex.Message);
            }
        }
        #endregion

        #region ABM
        private void agregarVendedor()
        {
            try
            {
                Vendedor ven = new Vendedor();
                Gestion_Api.Modelo.Empleado emp = this.controladorEmpleado.obtenerEmpleadoLegajo(Convert.ToDecimal(this.tLegajo.Value));
                ven.emp = emp;
                ven.comision = Convert.ToDecimal(this.txtComision.Text);
                ven.sucursal = Convert.ToInt32(this.DropListSucursal.SelectedValue);

                int i = this.controlador.agregarVendedor(ven);
                if (i > 0)
                {
                    //Agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Vendedor : " + ven.emp.nombre + " " + ven.emp.apellido);
                    //ScriptManager.RegisterStartupScript(this.UpdateVendedores, UpdateVendedores.GetType(), "alert", "Vendedor agregado con exito", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vendedor agregado con exito", "Vendedores.aspx"));
                    //Response.Redirect("Clientesabm.aspx");
                }
                else
                {
                    //Agrego mal
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo agregar vendedor. Verifique los datos ingresados", null));
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando vendedor. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error agregando vendedor. " + ex.Message);
            }
        }

        private void modificarVendedor()
        {
            try
            {
                //aux para obtener id
                Vendedor venAux = this.controlador.obtenerVendedorLegajo(Convert.ToDecimal(this.legajo));

                Vendedor ven = new Vendedor();
                ven.id = venAux.id;
                ven.emp.legajo = Convert.ToDecimal(this.tLegajo.Value);
                ven.comision = Convert.ToDecimal(this.txtComision.Text);
                ven.estado = 1;
                ven.sucursal = Convert.ToInt32(this.DropListSucursal.SelectedValue);

                int i = this.controlador.modificarVendedor(ven);

                if (i > 0)
                {
                    //Agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Vendedor : " + ven.emp.nombre + " " + ven.emp.apellido);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vendedor modificado con exito", "Vendedores.aspx"));
                    //Response.Redirect("Clientesabm.aspx");
                }
                else
                {
                    //Agrego mal
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar Empleado. Verifique los datos ingresados", null));
                }


                //this.PopUp1.Show("Cliente agregado con exito", MessageType.Information);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando Empleado" + ex.Message));
                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
            }
        }
        #endregion

    }
}