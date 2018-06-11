using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ABMBancos : System.Web.UI.Page
    {
        controladorCobranza controlador = new controladorCobranza();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.cargarBancos();
            }
            catch(Exception ex)
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
                    
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Bancos.Entidadades Bancarias") != 1)
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
                        if (s == "19")
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



        private void cargarBancos()
        {
            try
            {
                phBancos.Controls.Clear();
                List<Banco> bancos = this.controlador.obtenerBancosList();
                foreach (Banco ban in bancos)
                {
                    this.cargarBancosTable(ban);
                }

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando bancos. " + ex.Message));
            }
        }

        private void cargarBancosTable(Banco ban)
        {
            try
            {


                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = ban.codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.Width = Unit.Percentage(10);
                tr.Cells.Add(celCodigo);

                TableCell celEntidad = new TableCell();
                celEntidad.Text = ban.entidad;
                celEntidad.VerticalAlign = VerticalAlign.Middle;
                celEntidad.Width = Unit.Percentage(90);
                tr.Cells.Add(celEntidad);

                phBancos.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando banco en la lista. " + ex.Message));
            }
        }


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Banco ban = new Banco();
                ban.codigo = txtCodigo.Text;
                ban.entidad = txtEntidad.Text;

                if(!this.controlador.verificarBanco(ban.codigo))
                {
                    int i = this.controlador.agregarBanco(ban);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Bancos: " + ban.entidad);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Banco cargado con exito", null));
                        this.cargarBancos();
                        this.limpiarCampos();
                    }
                    else
                    {
                        //agrego mal
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ya hay un banco cargado con ese codigo"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando banco . " + ex.Message));
                
            }
        }

        private void limpiarCampos()
        {
            try
            {
                this.txtCodigo.Text = "";
                this.txtEntidad.Text = "";

            }
            catch
            { }
        }
    }
}