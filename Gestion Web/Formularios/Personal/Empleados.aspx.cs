using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Gestion_Web.Formularios.Personal
{
    public partial class Empleados : System.Web.UI.Page
    {
        int esUruguay = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsUruguay"));
        controladorEmpleado controlador = new controladorEmpleado();
        controladorRemuneraciones contRem = new controladorRemuneraciones();

        Mensajes m = new Mensajes();
        controladorUsuario contUser = new controladorUsuario();
        public Dictionary<string, string> camposEmpleados = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (esUruguay == 1) {
                    thTxtCUIT.InnerText = "RUT";
                    thTxtDNI.InnerText = "C.I";
                }
                
                if (!IsPostBack)
                {
                    this.cargarEmpresas();
                    this.cargarSucursal();
                }

                this.cargarEmpleados();
            }
            catch(Exception ex)
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
                        if (s == "7")
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

        private void cargarEmpresas()
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de Empresas. " + ex.Message));
            }
        }
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga los empleados en la tabla de empleados
        /// </summary>
        private void cargarEmpleados()
        {
            try
            {
                List<Gestion_Api.Modelo.Empleado> empleados = this.controlador.obtenerEmpleadosReduc();
                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                phEmpleados.Controls.Clear();

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
                    celNombre.Text = emp.nombre + " " + emp.apellido;
                    celNombre.Width = Unit.Percentage(30);
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNombre);

                    TableCell celDni = new TableCell();
                    celDni.Text = emp.dni;
                    celDni.Width = Unit.Percentage(10);
                    celDni.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celDni);

                    TableCell celCuit = new TableCell();
                    celCuit.Text = emp.cuitCuil;
                    celCuit.Width = Unit.Percentage(15);
                    celCuit.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celCuit);

                    TableCell celFecha = new TableCell();
                    celFecha.Text = emp.fecIngreso.ToString("dd/MM/yyyy");
                    celFecha.VerticalAlign = VerticalAlign.Middle;
                    celFecha.HorizontalAlign = HorizontalAlign.Left;
                    celFecha.Width = Unit.Percentage(10);
                    tr.Cells.Add(celFecha);

                    TableCell celRemuneracion = new TableCell();
                    celRemuneracion.Text = emp.remuneracion.ToString();
                    celRemuneracion.VerticalAlign = VerticalAlign.Middle;
                    celRemuneracion.HorizontalAlign = HorizontalAlign.Right;
                    celRemuneracion.Width = Unit.Percentage(10);
                    tr.Cells.Add(celRemuneracion);
                    
                    TableCell celAction = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = emp.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    //btnDetails.Click += new EventHandler(this.mostrarArticuloDetalles);
                    btnDetails.PostBackUrl = "EmpleadosABM.aspx?accion=2&codigo=" + emp.id.ToString();

                    celAction.Controls.Add(btnDetails);

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAction.Controls.Add(l2);

                    CheckBox cbSeleccion = new CheckBox();
                    cbSeleccion.ID = "cbSeleccion_" + emp.id.ToString();
                    cbSeleccion.CssClass = "btn btn-info";
                    cbSeleccion.Font.Size = 12;

                    celAction.Controls.Add(cbSeleccion);
                    celAction.Width = Unit.Percentage(10);
                    celAction.VerticalAlign = VerticalAlign.Middle;

                    Literal l5 = new Literal();
                    l5.Text = "&nbsp";
                    celAction.Controls.Add(l5);

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + emp.id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.OnClientClick = "abrirdialog(" + emp.id + ");";

                    celAction.Controls.Add(btnEliminar);


                    tr.Cells.Add(celAction);

                    //cargo el primer cliente en detalle
                    //if (i == 0)
                    //{
                    //    this.cargarEmpleadoDetalle(emp.id);
                    //    //agrego el link al boton editar
                    //    this.linkEditar.HRef = "EmpleadosABM.aspx?accion=2&codigo=" + emp.id;
                    //}

                    i++;

                    //arego fila a tabla
                    this.phEmpleados.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados. " + ex.Message));
            }
        }

            private void cargarEmpleadosTabla(List<Gestion_Api.Modelo.Empleado> empleados)
        {
            try
            {

                phEmpleados.Controls.Clear();

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
                    celNombre.Text = emp.nombre + " " + emp.apellido;
                    celNombre.Width = Unit.Percentage(30);
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNombre);

                    TableCell celDni = new TableCell();
                    celDni.Text = emp.dni;
                    celDni.Width = Unit.Percentage(10);
                    celDni.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celDni);

                    TableCell celCuit = new TableCell();
                    celCuit.Text = emp.cuitCuil;
                    celCuit.Width = Unit.Percentage(15);
                    celCuit.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celCuit);

                    TableCell celFecha = new TableCell();
                    celFecha.Text = emp.fecIngreso.ToString("dd/MM/yyyy");
                    celFecha.VerticalAlign = VerticalAlign.Middle;
                    celFecha.HorizontalAlign = HorizontalAlign.Left;
                    celFecha.Width = Unit.Percentage(10);
                    tr.Cells.Add(celFecha);

                    TableCell celRemuneracion = new TableCell();
                    celRemuneracion.Text = emp.remuneracion.ToString();
                    celRemuneracion.VerticalAlign = VerticalAlign.Middle;
                    celRemuneracion.HorizontalAlign = HorizontalAlign.Right;
                    celRemuneracion.Width = Unit.Percentage(10);
                    tr.Cells.Add(celRemuneracion);

                    TableCell celAction = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = emp.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    //btnDetails.Click += new EventHandler(this.mostrarArticuloDetalles);
                    btnDetails.PostBackUrl = "EmpleadosABM.aspx?accion=2&codigo=" + emp.id.ToString();

                    celAction.Controls.Add(btnDetails);

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAction.Controls.Add(l2);

                    CheckBox cbSeleccion = new CheckBox();
                    cbSeleccion.ID = "cbSeleccion_" + emp.id.ToString();
                    cbSeleccion.CssClass = "btn btn-info";
                    cbSeleccion.Font.Size = 12;

                    celAction.Controls.Add(cbSeleccion);
                    celAction.Width = Unit.Percentage(10);
                    celAction.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celAction);

                    //cargo el primer cliente en detalle
                    if (i == 0)
                    //{
                    //    this.cargarEmpleadoDetalle(emp.id);
                    //    //agrego el link al boton editar
                    //    this.linkEditar.HRef = "EmpleadosABM.aspx?accion=2&codigo=" + emp.id;
                    //}

                    i++;

                    //arego fila a tabla
                    this.phEmpleados.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados. " + ex.Message));
            }
        }
        /// <summary>
        /// Evento al hacer click en detalle
        /// </summary>
        private void mostrarArticuloDetalles(object sender, EventArgs e)
        {
            try
            {
                this.cargarEmpleadoDetalle(Convert.ToInt32((sender as LinkButton).ID));
                //agrego el link al boton editar
                //this.linkEditar.HRef = "EmpleadosABM.aspx?accion=2&codigo=" + (sender as LinkButton).ID;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de empleado desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando empleado detalle desde la interfaz. " + ex.Message);
            }
        }

        /// <summary>
        /// Carga los datos del empleado en el detalle de la pagina
        /// </summary>
        /// <param name="cuit">legajo del empleado que se quiere mostrar</param>
        private void cargarEmpleadoDetalle(int idEmpleado)
        {
            try
            {
                //borro
                //this.phEmpDetalle.Controls.Clear();
                //obtengo cliente con el cuit
                this.camposEmpleados = this.controlador.obtenerCamposEmpleados(idEmpleado);

                //cargo los campos
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                //Label1.Text = "";
                this.cargarEncabezadoDetalle(idEmpleado);
                foreach (KeyValuePair<string, string> kvp in camposEmpleados)
                {
                    //lo agrego a la etiqueta en el widget
                    //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                    //fila
                    TableRow tr = new TableRow();

                    //Celdas

                    TableCell celTitulo = new TableCell();
                    celTitulo.Text = kvp.Key;
                    celTitulo.Width = Unit.Percentage(30);
                    celTitulo.ForeColor = Color.Black;
                    //celTitulo.BorderStyle = BorderStyle.Solid;
                    celTitulo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celTitulo);


                    TableCell celValor = new TableCell();
                    celValor.Text = kvp.Value;
                    celValor.Width = Unit.Percentage(70);
                    celValor.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celValor);

                    //arego fila a tabla
                    table.Controls.Add(tr);
                }
                //phEmpDetalle.Controls.Add(table);

                return;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados detalle desde la interfaz. " + ex.Message));
            }
        }

        /// <summary>
        /// Carga los datos de legajo, nombre y apellido
        /// </summary>
        /// <param name="cuit">Legajo del empleado</param>
        private void cargarEncabezadoDetalle(int idEmpleado)
        {
            try
            {
                //this.phEmpEncabezado.Controls.Clear();
                Gestion_Api.Modelo.Empleado emp = this.controlador.obtenerEmpleadoID(idEmpleado);
                //cargo los campos
                Table table = new Table();
                table.CssClass = "btn btn-primary";
                table.BackColor = Color.Orange;
                table.Width = Unit.Percentage(100);
                //Label1.Text = "";

                //lo agrego a la etiqueta en el widget
                //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = emp.legajo.ToString();
                celCodigo.Width = Unit.Percentage(30);
                celCodigo.ForeColor = Color.Black;
                //celTitulo.BorderStyle = BorderStyle.Solid;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescrip = new TableCell();
                celDescrip.Text = emp.nombre + " " + emp.apellido;
                celDescrip.Width = Unit.Percentage(70);
                celDescrip.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescrip);

                //arego fila a tabla
                table.Controls.Add(tr);

                //phEmpEncabezado.Controls.Add(table);

                return;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando encabezado desde la interfaz. " + ex.Message));
            }
        }

        private void buscar(string nombre)
        {
            try
            {
                List<Gestion_Api.Modelo.Empleado> Empleados = this.controlador.obtenerEmpleadosNombre(nombre);
                this.cargarEmpleadosTabla(Empleados);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando empleado. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    this.buscar(this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando empleado. " + ex.Message));
            }
        }
                

        protected void DropListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarPuntoVta(Convert.ToInt32(DropListSucursal.SelectedValue));
            }
            catch
            {

            }
        }

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSucursal(Convert.ToInt32(DropListEmpresa.SelectedValue));
            }
            catch
            {

            }
        }

        protected void lbtnAgregarRemuneracion_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                //recorro los cheques en pantalla
                foreach (Control C in phEmpleados.Controls)
                {
                    TableRow tr = C as TableRow;
                    CheckBox ch = tr.Cells[6].Controls[2] as CheckBox;
                    //Si esta seleccionado, tiene estado disponible y es de terceros guardo el id.
                    if (ch.Checked == true)
                    {
                        idtildado += ch.ID.Substring(12, ch.ID.Length - 12) + ";";
                    }
                }
                if (!String.IsNullOrEmpty(idtildado))
                {
                    //Response.Redirect("PagoRemuneracionABM.aspx?a=1&emp=" + idtildado + "&e=" + this.DropListEmpresa.SelectedValue + "&s=" + this.DropListSucursal.SelectedValue + "&pv=" + this.DropListPtoVenta.SelectedValue);
                    int flag = 0;

                    foreach (string empleado in idtildado.Split(';'))
                    {
                        if (!String.IsNullOrEmpty(empleado))
                        {
                            Gestion_Api.Modelo.Empleado emp = this.controlador.obtenerEmpleadoID(Convert.ToInt32(empleado));

                            Remuneracione r = new Remuneracione();
                            r.Fecha = DateTime.Now;
                            r.Empleado = Convert.ToInt32(empleado);
                            r.Empresa = Convert.ToInt32(DropListEmpresa.SelectedValue);
                            r.Sucursal = Convert.ToInt32(DropListSucursal.SelectedValue);
                            r.PuntoVta = Convert.ToInt32(DropListPtoVenta.SelectedValue);
                            //pago.Numero = "0000";
                            r.Total = emp.remuneracion;
                            r.Periodo = this.DropListMes.SelectedValue + this.txtPeriodo.Text;

                            int i = this.contRem.agregarRemuneracion(r);

                            if (i <= 0)
                            {
                                flag = i;
                            }

                        }
                    }

                    if (flag == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito!. \", {type: \"info\"});", true);                        
                    }
                    else
                    {
                        if (flag == -2)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Una o mas remuneraciones ya se pasaron para ese periodo. \", {type: \"error\"});", true);                            
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Una o mas remuneraciones no se pudieron procesar. \", {type: \"error\"});", true);                            
                        }
                        
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Debe seleccionar una o mas remuneraciones. \");", true);                            
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idEmpleado = Convert.ToInt32(this.txtMovimiento.Text);

                int resultado = controlador.EliminarEmpleado(idEmpleado);

                if (resultado > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Personal eliminado con exito", null));
                }

                cargarEmpleados();

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "CATCH: Ocurrió un error. Ubicacion: Empleados. Metodo: btnSi_Click. Excepción: " + ex.Message);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }
        }
    }
}