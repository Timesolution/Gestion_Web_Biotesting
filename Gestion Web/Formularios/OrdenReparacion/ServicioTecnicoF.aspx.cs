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

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class ServicioTecnicoF : System.Web.UI.Page
    {
        controladorServicioTecnicoEntity contServTecEnt = new controladorServicioTecnicoEntity();
        controladorCliente contCliente = new controladorCliente();
        Mensajes m = new Mensajes();

        int accion = 0;
        string buscar = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            buscar = Request.QueryString["buscar"];
            accion = Convert.ToInt32(Request.QueryString["a"]);

            Page.Form.DefaultButton = this.lbBuscar.UniqueID;

            if (accion == 3)
                CargarServiciosTecnicosPorBusqueda();
            else
                CargarServiciosTecnicos();
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

        public void CargarServiciosTecnicosPorBusqueda()
        {
            try
            {
                phServicioTecnico.Controls.Clear();

                foreach (var item in contServTecEnt.ObtenerServiciosTecnicosByCampo(buscar))
                {
                    cargarEnPh(item);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error cargando servicios tecnicos " + ex.Message);
            }
        }
        public void CargarServiciosTecnicos()
        {
            try
            {
                phServicioTecnico.Controls.Clear();

                foreach (var item in contServTecEnt.ObtenerServiciosTecnicos())
                {
                    cargarEnPh(item);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error cargando servicios tecnicos " + ex.Message);
            }
        }

        private void cargarEnPh(ServicioTecnico st)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = st.Id.ToString();

                //Celdas
                TableCell celLocalidad = new TableCell();
                celLocalidad.Text = st.Nombre;
                celLocalidad.HorizontalAlign = HorizontalAlign.Left;
                celLocalidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celLocalidad);

                TableCell celDireccion = new TableCell();
                celDireccion.Text = st.Direccion;
                celDireccion.HorizontalAlign = HorizontalAlign.Left;
                celDireccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDireccion);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = st.Telefono;
                celTelefono.HorizontalAlign = HorizontalAlign.Left;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTelefono);

                TableCell celObservaciones = new TableCell();
                celObservaciones.Text = st.Observaciones;
                celObservaciones.HorizontalAlign = HorizontalAlign.Left;
                celObservaciones.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celObservaciones);

                TableCell celCliente = new TableCell();
                celCliente.Text = contCliente.obtenerClienteID((int)st.Cliente).razonSocial;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCliente);

                TableCell celMarcas = new TableCell();
                var marcas = contServTecEnt.ObtenerMarcasByIDServicioTecnico(st.Id);
                for (int i = 0; i < marcas.Count; i++)
                {
                    if (i == marcas.Count - 1)
                        celMarcas.Text += marcas[i].descripcion;
                    else
                        celMarcas.Text += marcas[i].descripcion + ", ";
                }

                celMarcas.HorizontalAlign = HorizontalAlign.Left;
                celMarcas.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMarcas);

                TableCell celAccion = new TableCell();

                Literal lDetail = new Literal();
                lDetail.ID = "btnEditar_" + st.Id.ToString();
                lDetail.Text = "<a href=\"ServicioTecnicoABM.aspx?a=2&st=" + st.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                lDetail.Text += "<span class=\"shortcut-icon icon-pencil\"></span>";
                lDetail.Text += "</a>";

                celAccion.Controls.Add(lDetail);

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAccion.Controls.Add(l1);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + st.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + st.Id + ");";
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(10);
                celAccion.VerticalAlign = VerticalAlign.Middle;
                celAccion.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAccion);

                celAccion.Controls.Add(btnEliminar);

                tr.Cells.Add(celAccion);

                phServicioTecnico.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idServicioTecnico = Convert.ToInt32(this.txtMovimiento.Text);
                ServicioTecnico st = contServTecEnt.ObtenerServicioTecnicoByID(idServicioTecnico);

                int i = contServTecEnt.EliminarServicioTecnicoByServicioTecnico(st);
                if (i >= 0)
                {
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Elimino el servicio tecnico: " + st.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Servicio tecnico eliminado con exito!", "ServicioTecnicoF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Servicio tecnico"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Servicio tecnico. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error eliminando Servicio tecnico " + ex.Message);
            }
        }

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ServicioTecnicoF.aspx?a=3&buscar=" + txtBusqueda.Text);
            }
            catch (Exception ex)
            {

            }
        }
    }
}