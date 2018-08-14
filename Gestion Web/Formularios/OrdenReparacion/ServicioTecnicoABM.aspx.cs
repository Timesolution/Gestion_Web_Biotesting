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

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class ServicioTecnicoABM : System.Web.UI.Page
    {
        controladorArticulo contArt = new controladorArticulo();
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

                if (accion == 2)
                {
                    btnGuardar.Visible = true;
                    btnAgregar.Visible = false;
                    ModificarServicioTecnico();
                }
            }

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
                        if (s == "57")
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
                List<string> stList = new List<string>();

                SetearServicioTecnico(st);

                foreach (var item in ListBoxMarcas.Items)
                {
                    stList.Add(item.ToString());
                }

                var temp = contServTecEnt.AgregarServicioTecnico(st,stList);

                if(temp > 0)
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "info", " $.msgbox(\"Servicio tecnico agregado correctamente! \", {type: \"info\"}); location.href = '../OrdenReparacion/ServicioTecnicoABM.aspx';", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", "$.msgbox(\"Error agregando servicio tecnico!. \", {type: \"error\"});", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando servicio tecnico. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error agregando servicio tecnico " + ex.Message);
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

                TableCell celMarcas = new TableCell();
                var marcas = contServTecEnt.ObtenerMarcasByIDServicioTecnico(st.Id);
                for (int i = 0; i < marcas.Count; i++)
                {
                    if(i == marcas.Count - 1)
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

        public void ModificarServicioTecnico()
        {
            try
            {
                var st = contServTecEnt.ObtenerServicioTecnicoByID(stID);
                txtNombre.Text = st.Nombre;
                txtDireccion.Text = st.Direccion;

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
                    ListBoxMarcas.Items.Add(item.descripcion);
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

                List<string> marcas = new List<string>();

                foreach (var item in ListBoxMarcas.Items)
                {
                    marcas.Add(item.ToString());
                }

                var temp = contServTecEnt.ModificarServicioTecnico(st,marcas);

                if (temp > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Servicio tecnico modificado con exito!", "ServicioTecnicoABM.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando servicio tecnico."));
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
                st.Estado = 1;
            }
            catch (Exception)
            {
                Log.EscribirSQL(1,"Error","Error seteando campos de servicio tecnico");
            }
        }

        protected void lbtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ServicioTecnicoABM.aspx");
            }
            catch (Exception)
            {   

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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Servicio tecnico eliminado con exito!", "ServicioTecnicoABM.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Servicio tecnico"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Servicio tecnico. " + ex.Message));
                Log.EscribirSQL(1,"Error","Error eliminando Servicio tecnico " + ex.Message);
            }
        }
    }
}