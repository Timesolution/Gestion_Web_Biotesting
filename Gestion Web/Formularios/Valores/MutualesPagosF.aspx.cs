using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class MutualesPagosF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorUsuario contUser = new controladorUsuario();
        controladorFactEntity controlador = new controladorFactEntity();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                if (!IsPostBack)
                {
                                   
                }

                this.cargarMutuales();
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
                        if (s == "91")
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
        private void cargarMutuales()
        {
            try
            {
                phMutuales.Controls.Clear();
                List<Mutuales_Pagos> cuotas = this.controlador.obtenerMutualesPagos();
                foreach (Mutuales_Pagos p in cuotas)
                {
                    this.cargarCuotasMutualesPH(p);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando mutuales. " + ex.Message));

            }
        }
        private void cargarCuotasMutualesPH(Mutuales_Pagos p)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celMutual= new TableCell();
                celMutual.Text = p.Mutuale.Nombre;
                celMutual.VerticalAlign = VerticalAlign.Middle;
                celMutual.Width = Unit.Percentage(15);
                tr.Cells.Add(celMutual);

                TableCell celNombre = new TableCell();
                celNombre.Text = p.Nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(15);
                tr.Cells.Add(celNombre);

                TableCell celCuotas = new TableCell();
                celCuotas.Text = p.Cuotas.ToString();
                celCuotas.HorizontalAlign = HorizontalAlign.Right;
                celCuotas.VerticalAlign = VerticalAlign.Middle;
                celCuotas.Width = Unit.Percentage(5);
                tr.Cells.Add(celCuotas);

                TableCell celCoeficiente = new TableCell();
                celCoeficiente.Text = p.Coeficiente.ToString() + "%";
                celCoeficiente.HorizontalAlign = HorizontalAlign.Right;
                celCoeficiente.VerticalAlign = VerticalAlign.Middle;
                celCoeficiente.Width = Unit.Percentage(5);
                tr.Cells.Add(celCoeficiente);

                TableCell celAction = new TableCell();                
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = p.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.PostBackUrl = "ABMMutualesPAgos.aspx?valor=2&id=" + p.Id;           
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + p.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + p.Id + ");";

                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phMutuales.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando operadores en la lista. " + ex.Message));
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                Mutuales_Pagos m = this.controlador.obtenerMutualPagosByID(Convert.ToInt32(this.txtMovimiento.Text));
                m.Estado = 0;
                int i = this.controlador.modificarMutualPagos(m);                
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Mutual Pago: " + m.Id);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Mutual eliminada con exito", null));
                    this.cargarMutuales();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Mutual"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Mutual. " + ex.Message));
            }
        }
        

    }
}