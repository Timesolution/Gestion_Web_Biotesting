using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class UnidadMedida : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario contUser = new controladorUsuario();
        controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();
        //valores
        private int valor;
        private int idUnidad;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idUnidad = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarUnidades();
                if (!IsPostBack)
                {

                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (valor == 2)
                    {
                        Unidades_De_Medidas unidad = controladorMateriaPrima.ObtenerUnidadDeMedidaPorID(this.idUnidad);
                        txtUnidad.Text = unidad.Descripcion;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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

        private void cargarUnidades()
        {
            try
            {
                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();
                phGruposArticulos.Controls.Clear();
                List<Unidades_De_Medidas> unidades = controladorMateriaPrima.GetAllUnidades();

                foreach (Unidades_De_Medidas un in unidades)
                {
                    this.cargarGruposPH(un);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Grupos. " + ex.Message));

            }
        }

        private void cargarGruposPH(Unidades_De_Medidas unidad)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = unidad.Descripcion;
                celDescripcion.HorizontalAlign = HorizontalAlign.Center;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = unidad.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarUnidadMedida);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + unidad.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + unidad.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                phGruposArticulos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando unidades de medidas en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();


                // MODIFCAR
                if (valor == 2)
                {
                    Unidades_De_Medidas unidad = new Unidades_De_Medidas();

                    unidad.Id = this.idUnidad;
                    unidad.Descripcion = txtUnidad.Text;
                    unidad.Estado = 1;

                    unidad = controladorMateriaPrima.ModificarUnidadDeMedida(unidad);
                    this.cargarUnidades();

                    if (unidad != null)
                    {
                        //agrego bien
                        //Log.EscribirSQL(idUsuario, "INFO", "Modifico el Grupo de Articulo: " + this.idGrupo);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  unidad de medida: " + unidad.Descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("unidad modificada con exito", "UnidadMedida.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando unidad"));
                    }
                }
                else
                {
                    // AGREGAR

                    Unidades_De_Medidas unidad = new Unidades_De_Medidas();
                    Unidades_De_Medidas unidadAux = new Unidades_De_Medidas();

                    unidad.Estado = 1;
                    unidad.Descripcion = txtUnidad.Text;

                    if(txtUnidad.Text != "")
                    {
                        unidadAux = controladorMateriaPrima.VerificarExistenciaUnidadDeMedida(unidad.Descripcion);

                        if (unidadAux.Id == 0)
                        {
                            unidad = controladorMateriaPrima.AgregarUnidadDeMedida(unidad);

                            if (unidad.Descripcion != "")
                            {
                                //agrego bien
                                //Log.EscribirSQL(idUsuario, "INFO", "Agrego el Grupo de Articulo: " + i);
                                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta unidad de medida: " + this.txtUnidad.Text);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("unidad cargada con exito", null));
                                this.borrarCampos();
                                this.cargarUnidades();
                                Response.Redirect("UnidadMedida.aspx");

                            }

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya existe una unidad de medida con el mismo nombre " + this.txtUnidad.Text));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Debe ingresar una descripcion de unidad de medida. " + this.txtUnidad.Text));
                    }

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Unidad de medida. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtUnidad.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarUnidadMedida(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("UnidadMedida.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar unidad de medida. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();
                int idUnidad = Convert.ToInt32(this.txtMovimiento.Text);
                int idUnidadAux = controladorMateriaPrima.EliminarUnidadMedida(idUnidad);
                if (idUnidadAux > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja unidad de medida: " + idUnidadAux);
                    this.cargarUnidades();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Unidad de medida eliminado con exito", "UnidadMedida.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando unidad de medida"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar unidad de medida. " + ex.Message));
            }
        }

    }
}