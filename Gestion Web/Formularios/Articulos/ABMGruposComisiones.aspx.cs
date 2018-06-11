using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestion_Api.Entitys;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ABMGruposComisiones : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArticuloEnt = new ControladorArticulosEntity();

        Mensajes mje = new Mensajes();
        private int accion;
        private int idGrupos_Comisiones;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.idGrupos_Comisiones = Convert.ToInt32(Request.QueryString["id"]);

                this.VerificarLogin();
                this.cargarGruposComisiones();
                if(!IsPostBack)
                {
                    this.cargarGruposArticulos();
                    if (accion == 2)
                    {
                        Grupos_Comisiones gc = this.contArticuloEnt.obtenerGrupos_ComisionesPorId(this.idGrupos_Comisiones);
                        this.txtComision.Text = gc.Comision.ToString();
                        this.ListGruposArticulos.SelectedValue = gc.Grupo.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error en Page_Load de ABMGruposComisiones.aspx.Excepción: " + Ex.Message));
            }
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
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "15")
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
        public void borrarCampos()
        {
            try
            {
                this.ListGruposArticulos.SelectedValue = "-1";
                this.txtComision.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }
        #endregion

        #region Eventos Controles
        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //Estoy editando una comisión
                if (accion == 2)
                {
                    var gc = this.contArticuloEnt.obtenerGrupos_ComisionesPorId(this.idGrupos_Comisiones);
                    if(gc!=null)
                    {
                        gc.Id = this.idGrupos_Comisiones;
                        gc.Grupo = Convert.ToInt32(this.ListGruposArticulos.SelectedValue);
                        if (!String.IsNullOrEmpty(txtComision.Text))
                        {
                            string comision = this.txtComision.Text.Replace(',', '.');
                            gc.Comision = Convert.ToDecimal(comision);

                            int i = this.contArticuloEnt.modificarGrupo_Comision(gc);
                            this.cargarGruposComisiones();
                            if (i > 0)
                            {
                                //Se modificó correctamente
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se modificó la comisión del Grupo " + this.ListGruposArticulos.SelectedItem);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Comisión modificada con éxito.", null));
                                this.borrarCampos();

                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo modificar la comisión."));

                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se ingreso valor de comisión."));

                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando comisión."));
                    } 
                    
                }
                else //Estoy agregando una comisión
                {
                    Grupos_Comisiones gc = new Grupos_Comisiones();
                    gc.Grupo = Convert.ToInt32(this.ListGruposArticulos.SelectedValue);
                    if (!String.IsNullOrEmpty(this.txtComision.Text))
                    {
                        string comision = this.txtComision.Text.Replace(',', '.');
                        gc.Comision = Convert.ToDecimal(comision);
                        int i = this.contArticuloEnt.agregarGrupo_Comision(gc);
                        this.cargarGruposComisiones();
                        if (i > 0)
                        {
                            //Se agregó correctamente
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Se agregó la comisión al grupo: " + this.ListGruposArticulos.SelectedItem);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Comisión agregada con éxito.", null));
                            this.borrarCampos();

                        }
                        if (i == -1) //Si el grupo ya posee una comisión, le aviso
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("El grupo seleccionado ya posee una comisión."));
                            this.borrarCampos();
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando comisión."));

                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se ingreso comisión."));

                    }
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando comisión. " + Ex.Message));
            }
        }
        #endregion

        #region ABM
        private void editarComision(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMGruposComisiones.aspx?a=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar comisión.Excepción: " + Ex.Message));
            }
        }
        private void eliminarComision (object sender, EventArgs e)
        {
            try
            {
                string id = (sender as LinkButton).ID.ToString();
                string[] datos = id.Split('_');
                id = datos[1];

                int i = this.contArticuloEnt.eliminarGrupo_Comision(Convert.ToInt64(id));
                if(i>0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Se eliminó la comisión del grupo seleccionado.",null));
                    this.cargarGruposComisiones();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo eliminar la comisión del grupo seleccionado."));
                }
                
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar comisión.Excepción: " + Ex.Message));
            }
        }
        #endregion

        #region Carga Inicial
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
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }
        private void cargarGruposComisiones()
        {
            try
            {
                phGruposComisiones.Controls.Clear();
                List<Grupos_Comisiones> grupos = this.contArticuloEnt.obtenerGrupos_Comisiones();
                foreach (Grupos_Comisiones gc in grupos)
                {
                    this.cargarGruposComisionesPH(gc);
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrió un error cargando las comisiones de los grupos.Excepción: " + Ex.Message));
            }
        }
        private void cargarGruposComisionesPH(Grupos_Comisiones gc)
        {
            try
            {
                TableRow tr = new TableRow();
                var g = this.contArticulo.obtenerGrupoID((int)gc.Grupo);
                
                TableCell celGrupo = new TableCell();
                celGrupo.Text = "";
                if (g != null)
                {
                    celGrupo.Text = g.descripcion;
                }
                celGrupo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celGrupo);

                TableCell celComision = new TableCell();
                celComision.Text = gc.Comision.ToString();
                celComision.VerticalAlign = VerticalAlign.Middle;
                celComision.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celComision);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = gc.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarComision);
                celAction.Controls.Add(btnEditar);

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAction.Controls.Add(l1);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "Eliminar_" + gc.Id.ToString();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "tooltip");
                btnEliminar.Attributes.Add("title data-original-title", "Eliminar");
                btnEliminar.Attributes.Add("target", "_blank");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Font.Size = 9;
                btnEliminar.Click += new EventHandler(this.eliminarComision);
                celAction.Controls.Add(btnEliminar);

                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phGruposComisiones.Controls.Add(tr);


            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando comisiones de los grupos en la lista. Excepción: " + Ex.Message));
            }
        }
        #endregion

    }
}