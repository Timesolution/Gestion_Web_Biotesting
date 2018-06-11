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
    public partial class ABMPosnetTarjetas : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idPosnet;
        private int idTarjeta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idPosnet = Convert.ToInt32(Request.QueryString["id"]);
                this.idTarjeta = Convert.ToInt32(Request.QueryString["t"]);

                this.VerificarLogin();
                this.cargarPosnets();
                if (!IsPostBack)
                {
                    Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                    this.txtPosnet.Text = p.Nombre;
                    if (valor == 2)
                    {                        
                        Posnet_Tarjetas t = p.Posnet_Tarjetas.Where(x => x.Id == idTarjeta).FirstOrDefault();                        
                        this.txtTarjeta.Text = t.Nombre;
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
                Posnet posnet = this.controlador.ObtenerPosnetByID(this.idPosnet);
                foreach (Posnet_Tarjetas t in posnet.Posnet_Tarjetas)
                {
                    this.cargarPosnetPH(t);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando tarjetas Posnets. " + ex.Message));

            }
        }

        private void cargarPosnetPH(Posnet_Tarjetas t)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celNombre = new TableCell();
                celNombre.Text = t.Nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNombre);

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(10);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAction.Controls.Add(l2);

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = t.Id.ToString();
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
                btnEliminar.ID = "btnEliminar_" + t.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + t.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phPosnets.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando tarjeta posnet en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.valor == 2)
                {
                    Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                    p.Posnet_Tarjetas.Where(x => x.Id == idTarjeta).FirstOrDefault().Nombre = this.txtTarjeta.Text;
                    
                    int i = this.controlador.ModificarPosnet(p);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Posnet_Tarjetas : " + this.txtTarjeta.Text);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Posnet modificada con exito!. \", {type: \"info\"});location.href='ABMPosnet.aspx';", true);                        
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet_Tarjetas modificada con exito!.", "ABMPosnet.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Posnet"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Posnet. \", {type: \"error\"});", true);                        
                    }
                }
                else
                {
                    Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                    Posnet_Tarjetas tarjeta = new Posnet_Tarjetas();
                    tarjeta.Nombre = this.txtTarjeta.Text;

                    p.Posnet_Tarjetas.Add(tarjeta);

                    int i = this.controlador.ModificarPosnet(p);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Agrego Posnet_Tarjetas : " + tarjeta.Nombre);
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Posnet modificada con exito!. \", {type: \"info\"});location.href='ABMPosnet.aspx';", true);                        
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet_Tarjetas agregada con exito!.", "ABMPosnet.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Posnet"));
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Posnet. \", {type: \"error\"});", true);                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Posnet. " + ex.Message));
            }

        }

        private void editarPosnet(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMPosnetTarjetas.aspx?valor=2&id="+this.idPosnet+"&t=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Posnet_Tarjetas. " + ex.Message));
            }
        }
        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idTarjeta = Convert.ToInt32(this.txtMovimiento.Text);

                Posnet p = this.controlador.ObtenerPosnetByID(this.idPosnet);
                Posnet_Tarjetas t = p.Posnet_Tarjetas.Where(x => x.Id == idTarjeta).FirstOrDefault();

                p.Posnet_Tarjetas.Remove(t);

                int i = this.controlador.ModificarPosnet(p);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Elimino Posnet_Tarjetas : " + t.Nombre);
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Posnet modificada con exito!. \", {type: \"info\"});location.href='ABMPosnet.aspx';", true);                        
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Posnet_Tarjetas modificada con exito!.", "ABMPosnet.aspx"));

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Posnet_Tarjetas"));
                    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Error modificando Posnet. \", {type: \"error\"});", true);                        
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar posnet. " + ex.Message));
            }
        }

    }
}