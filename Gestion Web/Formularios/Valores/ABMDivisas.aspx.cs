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
    public partial class ABMDivisas : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorMoneda controlador = new controladorMoneda();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idMoneda;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idMoneda = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                

                this.VerificarLogin();
                this.cargarMonedas();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        Moneda m = this.controlador.obtenerMonedaID(this.idMoneda);
                        txtCambio.Text = m.cambio.ToString();
                        txtMoneda.Text = m.moneda;
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
                        if (s == "49")
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

        private void cargarMonedas()
        {
            try
            {
                phDivisas.Controls.Clear();
                List<Moneda> monedas = this.controlador.obtenerMonedas();
                foreach (Moneda m in monedas)
                {
                    this.cargarMonedaPH(m);
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando monedas. " + ex.Message));

            }
        }

        private void cargarMonedaPH(Moneda m)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celMoneda = new TableCell();
                celMoneda.Text = m.moneda;
                celMoneda.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMoneda);

                TableCell celCambio = new TableCell();
                celCambio.Text = m.cambio.ToString();
                celCambio.VerticalAlign = VerticalAlign.Middle;
                celCambio.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCambio);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = m.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarMoneda);
                celAction.Controls.Add(btnEditar);

                Literal l1 = new Literal();
                l1.Text = "&nbsp";
                celAction.Controls.Add(l1);

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "Eliminar_" + m.id.ToString();
                btnEliminar.CssClass = "btn btn-info ui-tooltip";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Font.Size = 9;
                btnEliminar.OnClientClick = "abrirdialog(" + m.id.ToString() + ");";
                //btnEliminar.Click += new EventHandler(this.eliminarMoneda);
                celAction.Controls.Add(btnEliminar);

                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phDivisas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando moneda en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //estoy editando una moneda
                if (valor == 2)
                {
                    Moneda m = new Moneda();
                    m.id = this.idMoneda;
                    m.moneda = this.txtMoneda.Text;
                    if (!String.IsNullOrEmpty(txtCambio.Text))
                    {
                        string cambio = this.txtCambio.Text;
                        m.cambio = Convert.ToDecimal(cambio);
                        
                        int i = this.controlador.Modificar(m);
                        this.cargarMonedas();
                        if (i > 0)
                        {
                            //agrego bien
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Moneda : " + m.moneda);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Moneda modificada con exito", null));
                            this.borrarCampos();

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando moneda"));

                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se ingreso valor"));

                    }   
                }
                else //estoy agregando una moneda
                {
                    Moneda m = new Moneda();
                    m.moneda = this.txtMoneda.Text;
                    if (!String.IsNullOrEmpty(txtCambio.Text))
                    {
                        string cambio = this.txtCambio.Text.Replace(',', '.');
                        m.cambio = Convert.ToDecimal(cambio);
                        int i = this.controlador.agregar(m);
                        this.cargarMonedas();
                        if (i > 0)
                        {
                            //agrego bien
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Moneda : " + m.moneda);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Moneda cargada con exito", null));
                            this.borrarCampos();

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando moneda"));

                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se ingreso valor"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando moneda. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtCambio.Text = "";
                this.txtMoneda.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void editarMoneda(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMDivisas.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar moneda. " + ex.Message));
            }
        }        

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idMovimiento = Convert.ToInt32(this.txtMovimiento.Text);
                Moneda m = this.controlador.obtenerMonedaID(idMovimiento);

                int i = this.controlador.Eliminar(m);
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Divisa eliminada con exito.", "ABMDivisas.aspx"));
                }
                else
                {
                    if (i == -4)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se puedo eliminar divisa. Hay articulos cargados con esta divisa. Modifiquelos y vuelva a intentar."));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("No se pudo eliminar divisa."));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar moneda. " + ex.Message));
            }
        }
    }
}