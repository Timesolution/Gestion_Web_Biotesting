using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Gestion_Web.Formularios.Personal
{
    public partial class EmpleadosABM : System.Web.UI.Page
    {

        int esUruguay = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsUruguay"));
        Mensajes m = new Mensajes();
        //controlador
        controladorEmpleado controlador = new controladorEmpleado();
        controladorUsuario contUser = new controladorUsuario();
        //articulo
        Empleado empleado = new Empleado();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private int codigo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (esUruguay == 1)
                {
                    labelCUIT.InnerText = "RUT";
                    labelDNI.InnerText = "C.I";
                    txtDni.MaxLength = 20;
                    txtCuit.MaxLength = 20;
                }

                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.codigo = Convert.ToInt32(Request.QueryString["codigo"]);
                if (!IsPostBack)
                {
                    //cargo combos
                    //this.cargarProveedores();
                    //this.cargarGruposArticulos(1);
                    //this.cargarSubGruposArticulos(Convert.ToInt32(DropListGrupo.SelectedValue));
                    //this.cargarMonedas();
                    //this.cargarMonedasVenta();
                    //this.cargarPaises();

                    //modifico
                    if (this.accion == 2)
                    {
                        this.cargarEmpleado(this.codigo);
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

        /// <summary>
        /// Consulta los datos del empleado en la DB y lo devuelve
        /// </summary>
        /// <param name="id">Legajo del empleado</param>
        private void cargarEmpleado(int idEmpleado)
        {
            try
            {
                //reinicio
                this.empleado = new Empleado();

                this.empleado = this.controlador.obtenerEmpleadoID(idEmpleado);
                if (this.empleado != null)
                {
                    
                    this.txtLegajo.Text = this.empleado.legajo.ToString();
                    this.txtNombre.Text = this.empleado.nombre;
                    this.txtApellido.Text = this.empleado.apellido;
                    this.txtDni.Text = this.empleado.dni;
                    this.txtDireccion.Text = this.empleado.direccion;
                    this.txtFechaNacimiento.Text = this.empleado.fecNacimiento.ToString("dd/MM/yyyy");
                    this.txtFechaIngreso.Text = this.empleado.fecIngreso.ToString("dd/MM/yyyy");
                    this.txtCuit.Text = this.empleado.cuitCuil;
                    this.txtObservaciones.Text = this.empleado.observaciones;
                    this.txtRemuneracion.Text = this.empleado.remuneracion.ToString();
                    Session.Add("EmpleadosABM_Legajo", empleado.legajo);
                    Session.Add("EmpleadosABM_DNI", empleado.dni);
                    Session.Add("EmpleadosABM_CUIT", empleado.cuitCuil);
                }
                else
                {
                    //Pop up 
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo cargar el empleado"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando campos del empleado. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.accion == 1)
                this.agregarEmpleado();

            if (this.accion == 2)
                this.modificarEmpleado();
        }

        private void agregarEmpleado()
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
                emp.cuitCuil = this.txtCuit.Text;
                emp.observaciones = this.txtObservaciones.Text;
                emp.remuneracion = Convert.ToDecimal(this.txtRemuneracion.Text);

                int i = this.controlador.agregarEmpleado(emp);
                if (i > 0)
                {
                    this.RespuestaAgregarEmpleado(1);
                }
                else
                {
                    this.RespuestaAgregarEmpleado(i);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Empleado" + ex.Message));
                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
            }
        }

        private void modificarEmpleado()
        {
            try
            {
                Empleado emp = new Empleado();
                decimal legajo = (decimal)Session["EmpleadosABM_Legajo"];
                string dni = Session["EmpleadosABM_DNI"] as string;
                string cuit = Session["EmpleadosABM_CUIT"] as string;
                emp.id = this.codigo;
                emp.legajo = Convert.ToDecimal(this.txtLegajo.Text);
                emp.nombre = this.txtNombre.Text;
                emp.apellido = this.txtApellido.Text;
                emp.direccion = this.txtDireccion.Text;
                emp.dni = this.txtDni.Text;
                emp.fecNacimiento = Convert.ToDateTime(this.txtFechaNacimiento.Text, new CultureInfo("es-AR"));
                emp.fecIngreso = Convert.ToDateTime(this.txtFechaIngreso.Text, new CultureInfo("es-AR"));
                emp.cuitCuil = this.txtCuit.Text;
                emp.observaciones = this.txtObservaciones.Text;
                emp.estado = 1;

                emp.remuneracion = Convert.ToDecimal(this.txtRemuneracion.Text);

                int i = this.controlador.modificarEmpleado(emp,legajo,dni,cuit);
                this.RespuestaModificarEmpleado(i);


                //this.PopUp1.Show("Cliente agregado con exito", MessageType.Information);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando Empleado" + ex.Message));
                //this.PopUp1.Show("Error agregando cliente" + ex.Message, MessageType.Error);
            }
        }


        private void RespuestaAgregarEmpleado(int i)
        {
            try
            {
                switch (i)
                {
                    case 1:
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Alta Empleado: " + this.txtNombre + " " + this.txtApellido.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Empleado agregado con exito", "Empleados.aspx"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El Legajo ya fue ingresado", null));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El CUIL ya fue ingresado", null));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El DNI ya fue ingresado", null));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo agregar Empleado. Verifique los datos ingresados", null));
                        break;
                    default:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo agregar Empleado", null));
                        break;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void RespuestaModificarEmpleado(int i)
        {
            try
            {
                switch (i)
                {
                    case 1:
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Modifico Empleado: " + this.txtNombre.Text + " " + this.txtApellido.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Empleado modificado con exito", "Empleados.aspx"));
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El Legajo ya fue ingresado", null));
                        break;
                    case -3:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El CUIL ya fue ingresado", null));
                        break;
                    case -4:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El DNI ya fue ingresado", null));
                        break;
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar Empleado. Verifique los datos ingresados", null));
                        break;
                    default:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No se pudo modificar Empleado", null));
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}