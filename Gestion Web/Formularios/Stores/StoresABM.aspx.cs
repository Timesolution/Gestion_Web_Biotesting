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

namespace Gestion_Web.Formularios.Stores
{
    public partial class StoresABM : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        private int idUsuario;
        private int accion;
        private int idStore;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                this.idUsuario = (int)Session["Login_IdUser"];
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.idStore = Convert.ToInt32(Request.QueryString["idStore"]);

                if (!IsPostBack)
                {
                    if (accion == 1)
                    {
                        btnAgregar.Visible = true;
                        //AgregarStore();
                    }                        
                    else if (accion == 2)
                    {
                        btnAceptar.Visible = true;
                        CargarDatos();
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
                    //if (!String.IsNullOrEmpty(s))
                    //{
                    //    if (s == "15")
                    //    {
                    return 1;
                    //    }
                    //}
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        
        public void CargarDatos()
        {
            try
            {
                controladorStore contStore = new controladorStore();

                var store = contStore.ObtenerStoresPorID(idStore);

                txtNombreStore.Text = store.Descripcion;
                txtDetalleStore.Text = store.Detalle;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error al cargar datos. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Ocurrio un error al cargar datos. " + ex.Message);
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorStore contStore = new controladorStore();

                var store = contStore.ObtenerStoresPorID(idStore);

                if(store != null)
                {
                    store.Descripcion = txtNombreStore.Text;
                    store.Detalle = txtDetalleStore.Text;
                }               

                int temp = contStore.ModificarStore();

                if(temp >= 1)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Modificaciones realizadas con exito!",null),false);
                else if(temp == 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("No se realizaron cambios"),false);
                else if(temp < 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "error", mje.mensajeBoxError("Error realizando modificaciones"),false);

                Response.Redirect("StoresF.aspx");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error al guardar datos. " + ex.Message));
                Log.EscribirSQL(1,"Error","Error al guardar cambios en el store. " + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorStore contStore = new controladorStore();
                Store store = new Store();

                store.Descripcion = txtNombreStore.Text;
                store.Detalle = txtDetalleStore.Text;

                int temp = contStore.AgregarStore(store);

                if (temp > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxInfo("Store agregado con exito!.", null));
                    Log.EscribirSQL(1, "Info", "Store agregado con exito.");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "info", mje.mensajeBoxError("Error agregando store."));
                    Log.EscribirSQL(1, "Info", "Error agregando store.");
                }

                Response.Redirect("StoresF.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error al agregar store. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Ocurrio un error al agregar store. " + ex.Message);
            }
        }
    }
}