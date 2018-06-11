using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMTrazabilidad : System.Web.UI.Page
    {
        controladorArticulo controlador = new controladorArticulo();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();

        public int valor;
        public int idCampo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idCampo = Convert.ToInt32(Request.QueryString["id"]);
                
                if (!IsPostBack)
                {
                    this.cargarGruposArticulos();

                    if (valor == 2)//editar
                    {
                        Trazabilidad_Campos campo = this.controlador.obtenerCamposTrazabilidadById(this.idCampo);
                        this.cargarDatosCampo(campo);
                    }
                }

                if (this.DropListGrupo.SelectedValue != "-1")
                {
                    this.cargarCampos();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "25")
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
        private void cargarGruposArticulos()
        {
            try
            {
                DataTable dt = controlador.obtenerGruposArticulos();

                //agrego Seleccione
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListGrupo.DataSource = dt;
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarDatosCampo(Trazabilidad_Campos campo)
        {
            try
            {
                this.DropListGrupo.SelectedValue = campo.idGrupo.ToString();
                this.txtNombre.Text = campo.nombre;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error Cargando datos de campo trazabilidad. " + ex.Message));
            }
        }
        private void cargarPH(Trazabilidad_Campos campo)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celNombre = new TableCell();
                celNombre.Text = campo.nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(25);
                tr.Cells.Add(celNombre);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + campo.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.editarCampo);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + campo.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + campo.id + ");";                
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phCamposTrazabilidad.Controls.Add(tr);
            }
            catch (Exception ex)
            {

            }
        }

        private void altaCampo()
        {
            try
            {
                Trazabilidad_Campos campo = new Trazabilidad_Campos();

                campo.idGrupo = Convert.ToInt32(DropListGrupo.SelectedValue);
                campo.nombre = this.txtNombre.Text;

                int i = this.controlador.agregarCampoTrazabilidad(campo);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Campo trazabilidad : " + i);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Campo agregado con exito", null));

                    this.txtNombre.Text = "";
                    this.DropListGrupo.SelectedValue = "-1";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando campo trazabilidad. " + ex.Message));
            }
        }

        private void modificarCampo()
        {
            try
            {
                Trazabilidad_Campos campo = new Trazabilidad_Campos();
                                
                campo.idGrupo = Convert.ToInt32(DropListGrupo.SelectedValue);
                campo.nombre = this.txtNombre.Text;

                int i = this.controlador.modificarCampoTrazabilidad(this.idCampo, campo.nombre);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Campo trazabilidad : " + campo.nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Campo modificado con exito", "ABMTrazabilidad.aspx"));

                    this.txtNombre.Text = "";
                    this.DropListGrupo.SelectedValue = "-1";
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando campo trazabilidad. " + ex.Message));
            }
        }

        private void cargarCampos()
        {
            try
            {
                this.phCamposTrazabilidad.Controls.Clear();
                List<Trazabilidad_Campos> listCampos = this.controlador.obtenerCamposTrazabilidadByGrupo(Convert.ToInt32(this.DropListGrupo.SelectedValue));

                foreach (Trazabilidad_Campos campo in listCampos)
                {
                    this.cargarPH(campo);
                }
            }
            catch(Exception ex)
            {

            }
        }
        protected void DropListGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCampos();
        }
        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    this.modificarCampo();
                }
                else
                {
                    //alta sublista
                    this.altaCampo();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void editarCampo(object sender, EventArgs e)
        {
            try
            {
                string id =  (sender as LinkButton).ID.Split('_')[1];
                Response.Redirect("ABMTrazabilidad.aspx?valor=2&id=" + id);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar campo de trazabilidad. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idCampo = Convert.ToInt32(this.txtMovimiento.Text);
                int i = this.controlador.eliminarCampoTrazabilidad(idCampo);
                if (i > 0)
                {
                    //elimino bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Campo trazabilidad : " + idCampo);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Campo trazabilidad eliminada con exito", null));
                    this.cargarCampos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Campo trazabilidad"));

                }
            }
            catch(Exception ex)
            {

            }
        }

    }
}