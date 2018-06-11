using Disipar.Models;
using Gestion_Api.Controladores;
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

namespace Gestion_Web.Formularios.Formas
{
    public partial class FormasPagoF : System.Web.UI.Page
    {
        ControladorFormasPago contFormas = new ControladorFormasPago();
        controladorUsuario contUser = new controladorUsuario();
        controladorListaPrecio contLista = new controladorListaPrecio();
        Mensajes m = new Mensajes();

        public int valor;
        public int idSublista;
        public int idLista;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);                
                string parametros = Request.QueryString["id"];                
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    this.cargarListaPrecio();
                    this.cargarFormaPago();

                    if (valor == 2)
                    {
                        
                    }
                }

                this.cargarTableFormasLista();

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
                        if (s == "79")
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
        public void cargarFormaPago()
        {
            try
            {
                controladorFacturacion contFact = new controladorFacturacion();
                DataTable dt = contFact.obtenerFormasPago();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["forma"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListFormaPago.DataSource = dt;
                this.DropListFormaPago.DataValueField = "id";
                this.DropListFormaPago.DataTextField = "forma";

                this.DropListFormaPago.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando formas pago. " + ex.Message));
            }
        }
        public void cargarListaPrecio()
        {
            try
            {
                DataTable dt = this.contLista.obtenerListasPrecios();               

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListLista.DataSource = dt;
                this.DropListLista.DataValueField = "id";
                this.DropListLista.DataTextField = "nombre";

                this.DropListLista.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando categorias. " + ex.Message));
            }
        }
        public void cargarTableFormasLista()
        {
            try
            {
                this.phFormasListas.Controls.Clear();
                List<Gestion_Api.Entitys.formasPago> formas = this.contFormas.obtenerListFormasListas();

                if (formas != null)
                {
                    foreach (var f in formas)
                    {
                        this.cargarTablePH(f);
                    }
                }
            }
            catch
            {

            }
        }
        public void cargarTablePH(Gestion_Api.Entitys.formasPago forma)
        {
            try
            {
                foreach (Gestion_Api.Entitys.listasPrecio l in forma.listasPrecios)
                {
                    TableRow tr = new TableRow();

                    TableCell celNombre = new TableCell();
                    celNombre.Text = forma.forma;
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    celNombre.Width = Unit.Percentage(25);
                    tr.Cells.Add(celNombre);

                    TableCell celSubLista = new TableCell();
                    celSubLista.Text = l.nombre;
                    celSubLista.VerticalAlign = VerticalAlign.Middle;
                    celSubLista.Width = Unit.Percentage(25);
                    tr.Cells.Add(celSubLista);

                    TableCell celAction = new TableCell();

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + forma.id + "_" + l.id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    btnEliminar.OnClientClick = "abrirdialog(" + forma.id + "," + l.id + ");";
                    celAction.Controls.Add(btnEliminar);
                    celAction.Width = Unit.Percentage(10);
                    celAction.VerticalAlign = VerticalAlign.Middle;
                    celAction.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(celAction);

                    phFormasListas.Controls.Add(tr);
                }
            }
            catch
            {
                
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int forma = Convert.ToInt32(this.DropListFormaPago.SelectedValue);
                int lista = Convert.ToInt32(this.DropListLista.SelectedValue);

                int i = this.contFormas.agregarListaFormaPago(forma, lista);
                if (i > 0)
                {
                    //modifico bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Administrador de Forma pago Lista : " + i);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio agregada a forma de pago con exito", null));
                    this.cargarTableFormasLista();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista a forma pago"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idFormaLista = this.txtMovimiento.Text;
                int forma = Convert.ToInt32(idFormaLista.Split('_')[0]);
                int lista = Convert.ToInt32(idFormaLista.Split('_')[1]);

                int i = this.contFormas.eliminarListaFormaPago(forma,lista);
                if (i > 0)
                {
                    //modifico bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Administrador de Forma pago Lista : " + i);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio eliminada de forma de pago con exito", null));
                    this.cargarTableFormasLista();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Configuracion de lista de Precio"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Configuracion de forma pago lista. " + ex.Message));
            }
        }
    }
}