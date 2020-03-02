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

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class FormasVenta : System.Web.UI.Page
    {
        controladorFactEntity contFactEntity = new controladorFactEntity();
        Mensajes mje = new Mensajes();        
        //valores
        private int valor;
        private int idForma;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idForma = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarFormas();

                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        this.cargarDatosForma(this.idForma);
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
                        if (s == "83")
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
        private void cargarDatosForma(int id)
        {
            try
            {
                var fv = this.contFactEntity.obtenerFormaVentaById(id);
                this.txtNombre.Text = fv.Nombre;
                this.txtPorcentajeA.Text = fv.PorcentajeA.Value.ToString();
                this.txtPorcentajeB.Text = fv.PorcentajeB.Value.ToString();

            }
            catch
            {

            }
        }
        private void cargarFormas()
        {
            try
            {
                this.phFormas.Controls.Clear();
                List<Formas_Venta> formas = this.contFactEntity.obtenerFormasVenta();

                foreach (var f in formas)
                {
                    this.cargarFormasPH(f);
                }

            }
            catch
            {

            }
        }
        private void cargarFormasPH(Formas_Venta f)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = f.Nombre;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celPorc1 = new TableCell();
                celPorc1.Text = f.PorcentajeA.Value.ToString();
                celPorc1.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPorc1);

                TableCell celPorc2 = new TableCell();
                celPorc2.Text = f.PorcentajeB.Value.ToString();
                celPorc2.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celPorc2);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = f.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.editarPerfil);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + f.Id.ToString();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + f.Id + ");";

                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phFormas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando forma en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal porcA = Convert.ToDecimal(this.txtPorcentajeA.Text);
                decimal porcB = Convert.ToDecimal(100 - porcA);

                if ((porcA + porcB) != 100)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("La suma de los ambos porcentajes debe ser igual a 100."));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxAtencion("La suma de los ambos porcentajes debe ser igual a 100."), true);
                    return;
                }
                if(porcA < 0 || porcB < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Los porcentajes deben ser ambos mayor o igual a cero."));
                    return;
                }
                if ((porcA == 0) || (porcA == 100))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Los porcentajes no pueden ser ni 0 ni 100."));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxAtencion("La suma de los ambos porcentajes debe ser igual a 100."), true);
                    return;
                }

                if (valor == 2)
                {
                    Gestion_Api.Entitys.Formas_Venta fv = this.contFactEntity.obtenerFormaVentaById(this.idForma);
                    fv.Nombre = this.txtNombre.Text;
                    fv.PorcentajeA = porcA;
                    fv.PorcentajeB = porcB;

                    int i = this.contFactEntity.modificarFormaVenta(fv);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Modifico Forma venta: " + fv.Nombre);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Forma venta modificada con exito", "FormasVenta.aspx"));                        
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxInfo("Forma venta modificada con exito", "FormasVenta.aspx"), true);                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Forma venta"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxError("Error modificando Forma venta"), true);
                    }
                }
                else
                {
                    Gestion_Api.Entitys.Formas_Venta fv = new Formas_Venta();
                    fv.Nombre = this.txtNombre.Text;
                    fv.PorcentajeA = porcA;
                    fv.PorcentajeB = porcB;
                    fv.Estado = 1;

                    int i = this.contFactEntity.agregarFormaVenta(fv);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Alta Forma venta: " + this.txtNombre.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Forma venta cargado con exito", "FormasVenta.aspx"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxInfo("Forma venta cargado con exito", "FormasVenta.aspx"), true);                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Formas Venta"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxError("Error agregando Formas Venta"), true);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Formas Venta. " + ex.Message));
                //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", mje.mensajeBoxError("Error cargando Formas Venta. " + ex.Message), true);
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarPerfil(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FormasVenta.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar forma. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idForma = Convert.ToInt32(this.txtMovimiento.Text);
                int i = this.contFactEntity.eliminarFormaVenta(idForma);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Baja Forma de venta id: " + idForma);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Proceso finalizado con exito", "FormasVenta.aspx"));

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando forma"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar forma. " + ex.Message));
            }
        }

        protected void txtPorcentajeA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtPorcentajeA.Text))
                {
                    decimal porcA = Convert.ToDecimal(this.txtPorcentajeA.Text);
                    if (porcA <= 100)
                    {
                        this.txtPorcentajeB.Text = (100 - porcA).ToString();
                    }
                    else
                    {
                        this.txtPorcentajeA.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

    }
}