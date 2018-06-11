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
    public partial class ABMPosnet : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idPosnet;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idPosnet = Convert.ToInt32(Request.QueryString["id"]);

                this.VerificarLogin();
                this.cargarPosnets();
                if (!IsPostBack)
                {
                    if (valor == 2)
                    {
                        Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                        this.txtPosnet.Text = p.Nombre;
                        this.txtNumero.Text = p.Numero;
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

        private void cargarPosnets()
        {
            try
            {
                phPosnets.Controls.Clear();
                List<Posnet> posnets = this.controlador.ObtenerPosnets();
                foreach (Posnet p in posnets)
                {
                    this.cargarPosnetPH(p);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Posnets. " + ex.Message));

            }
        }

        private void cargarPosnetPH(Posnet p)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celNombre = new TableCell();
                celNombre.Text = p.Nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celNumero = new TableCell();
                celNumero.Text = p.Numero.ToString();
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celNumero);

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(15);

                LinkButton btnTarjeta = new LinkButton();                
                btnTarjeta.ID = "btnPuntoVenta_" + p.Id.ToString();
                btnTarjeta.CssClass = "btn btn-info ui-tooltip";
                btnTarjeta.Attributes.Add("data-toggle", "tooltip");
                btnTarjeta.Attributes.Add("title data-original-title", "Tarjeta");
                btnTarjeta.Text = "<span class='shortcut-icon icon-credit-card'></span>" + " +";
                btnTarjeta.PostBackUrl = "../../Formularios/Valores/ABMPosnetTarjetas.aspx?id=" + p.Id;
                celAction.Controls.Add(btnTarjeta);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = p.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarPosnet);
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
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phPosnets.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando posnet en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                    p.Nombre = this.txtPosnet.Text;
                    p.Numero = this.txtNumero.Text;

                    int i = this.controlador.ModificarPosnet(p);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Posnet : " + p.Nombre);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Posnet modificada con exito!. \", {type: \"info\"});location.href='ABMPosnet.aspx';", true);                        
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet modificada con exito!.", "ABMPosnet.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Posnet"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Posnet. \", {type: \"error\"});", true);                        
                    }

                }
                else
                {
                    Posnet p = new Posnet();
                    p.Nombre = this.txtPosnet.Text;
                    p.Numero = this.txtNumero.Text;

                    int i = this.controlador.AgregarPosnet(p);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Posnet : " + p.Nombre);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Posnet agregado con exito!. \", {type: \"info\"});location.href='ABMPosnet.aspx';", true);                        
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet agregado con exito!.", "ABMPosnet.aspx"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando Posnet"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error agregado Posnet. \", {type: \"error\"});", true); 
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Posnet. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtPosnet.Text = "";
                this.txtNumero.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void editarPosnet(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMPosnet.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar posnet. " + ex.Message));
            }
        }
        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idPos = Convert.ToInt32(this.txtMovimiento.Text);
                Posnet p = this.controlador.ObtenerPosnetByID(idPos);
                string nombre = p.Nombre;

                int i = this.controlador.EliminarPosnet(idPos);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Posnet : " + nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet eliminada con exito", null));
                    this.cargarPosnets();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando tarjeta"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar posnet. " + ex.Message));
            }
        }

    }
}