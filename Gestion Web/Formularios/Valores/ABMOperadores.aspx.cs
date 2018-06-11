using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMOperadores : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idOperador;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idOperador = Convert.ToInt32(Request.QueryString["id"]);
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);

                this.VerificarLogin();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        this.cargarOperador(this.idOperador);
                    }                    
                }
                this.cargarOperadores();
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
                        if (s == "11")
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
        private void cargarOperador(int id)
        {
            try
            {
                Gestion_Api.Entitys.Operadores_Tarjeta ope = this.controlador.obtenerOperadorById(id);
                if (ope != null)
                {
                    this.txtNombre.Text = ope.Operador;
                }
            }
            catch
            {

            }
        }
        private void cargarOperadores()
        {
            try
            {
                phTarjetas.Controls.Clear();
                List<Gestion_Api.Entitys.Operadores_Tarjeta> operadores = this.controlador.obtenerOperadores();
                foreach (Gestion_Api.Entitys.Operadores_Tarjeta o in operadores)
                {
                    this.cargarOperadoresPH(o);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando tarjetas. " + ex.Message));

            }
        }

        private void cargarOperadoresPH(Gestion_Api.Entitys.Operadores_Tarjeta o)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celNombre = new TableCell();
                celNombre.Text = o.Operador;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(25);
                tr.Cells.Add(celNombre);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = o.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarTarjeta);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + o.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + o.Id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phTarjetas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando operadores en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    Gestion_Api.Entitys.Operadores_Tarjeta ope = this.controlador.obtenerOperadorById(this.idOperador);
                    ope.Operador = this.txtNombre.Text;

                    int i = this.controlador.modificarOperador(ope);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Operador modificado con exito", "ABMOperadores.aspx"));                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo modificar Operador"));
                    }
                }
                else
                {
                    Gestion_Api.Entitys.Operadores_Tarjeta ope = new Gestion_Api.Entitys.Operadores_Tarjeta();
                    ope.Operador = this.txtNombre.Text;
                    ope.Estado = 1;

                    int i = this.controlador.agregarOperador(ope);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Operador : " + ope.Operador);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Operador agregado con exito", null));
                        this.cargarOperadores();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se pudo agregar Operador"));
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando operador. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtNombre.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void editarTarjeta(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMOperadores.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar operador. " + ex.Message));
            }
        }
        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idOperador = Convert.ToInt32(this.txtMovimiento.Text);
                Gestion_Api.Entitys.Operadores_Tarjeta ope = this.controlador.obtenerOperadorById(idOperador);
                if (ope.Tarjetas.Count == 0)
                {
                    int i = this.controlador.eliminarOperador(idOperador);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Operador : " + ope.Operador);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Operador eliminada con exito", null));
                        this.cargarOperadores();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Operador"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No puede eliminar un operador que contiene tarjetas."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Operador. " + ex.Message));
            }
        }
        

    }
}