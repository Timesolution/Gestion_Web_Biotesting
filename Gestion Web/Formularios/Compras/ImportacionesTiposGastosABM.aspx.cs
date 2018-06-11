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

namespace Gestion_Web.Formularios.Compras
{
    public partial class ImportacionesTiposGastosABM : System.Web.UI.Page
    {        
        Mensajes mje = new Mensajes();
        //controlador
        ControladorImportaciones contImportaciones = new ControladorImportaciones();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idGastos;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idGastos = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarTiposGasto();
                if (!IsPostBack)
                {
                    this.idUsuario = (int)Session["Login_IdUser"];
                    if (valor == 2)
                    {
                        TipoGastos_Importacion tipo = this.contImportaciones.obtenerTipoGastoImportacion(this.idGastos);
                        this.txtTipoGasto.Text = tipo.TipoGasto;
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

        private void cargarTiposGasto()
        {
            try
            {
                phTipos.Controls.Clear();
                List<TipoGastos_Importacion> tipos = this.contImportaciones.obtenerTiposGastoImportacion();
                foreach (TipoGastos_Importacion t in tipos)
                {
                    this.cargarTiposGastoPH(t);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando list. " + ex.Message));

            }
        }
        private void cargarTiposGastoPH(TipoGastos_Importacion t)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = t.TipoGasto;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = t.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarTipoGasto);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + t.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + t.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phTipos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Tipo en la lista. " + ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    TipoGastos_Importacion tipo = this.contImportaciones.obtenerTipoGastoImportacion(this.idGastos);
                    tipo.TipoGasto = this.txtTipoGasto.Text;

                    int i = this.contImportaciones.modificarTipoGastoImportacion(tipo);
                    if (i > 0)
                    {
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Tipo Gastos_Importacion: " + tipo.TipoGasto);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tipo Gasto modificado con exito", "ImportacionesTiposGastosABM.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando."));
                    }
                }
                else
                {
                    TipoGastos_Importacion tipo = new TipoGastos_Importacion();
                    tipo.TipoGasto = this.txtTipoGasto.Text;
                    tipo.Estado = 1;
                    int i = this.contImportaciones.agregarTipoGastoImportacion(tipo);
                    if (i > 0)
                    {
                        //agrego bien                        
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Tipo Gasto: " + this.txtTipoGasto.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tipo Gasto cargada con exito", null));
                        Response.Redirect("ImportacionesTiposGastosABM.aspx");

                    }
                    else
                    {
                        if (i == -2)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Ya existe un tipo de gasto con el nombre " + this.txtTipoGasto.Text));
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando."));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error guardando. " + ex.Message));
            }

        }
        public void borrarCampos()
        {
            try
            {
                this.txtTipoGasto.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }
        private void editarTipoGasto(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImportacionesTiposGastosABM.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idGrupo = Convert.ToInt32(this.txtMovimiento.Text);
                TipoGastos_Importacion tipo = this.contImportaciones.obtenerTipoGastoImportacion(idGrupo);
                tipo.Estado = 0;
                int i = this.contImportaciones.modificarTipoGastoImportacion(tipo);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Tipo Gastos Importacion: " + tipo.TipoGasto);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Eliminado con exito", null));
                    this.cargarTiposGasto();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando."));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar.. " + ex.Message));
            }
        }
    }
}