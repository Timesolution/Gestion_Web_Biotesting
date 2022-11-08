using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Disipar.Models;
using System.Data;
using System.IO;
using System.Net;
namespace Gestion_Web.Formularios.Reportes
{
    public partial class InformesN : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        ControladorInformesEntity controlador = new ControladorInformesEntity();
        private string fechaD;
        private string fechaH;
        private string idUsuario;
        private int accion;
        private long idTipoInforme;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                fechaD = Request.QueryString["fd"];
                fechaH = Request.QueryString["fh"];
                idUsuario = Request.QueryString["us"];
                accion = Convert.ToInt32(Request.QueryString["a"]);
                idTipoInforme = Convert.ToInt64(Request.QueryString["tipo"]);

                if (!IsPostBack)
                {
                    if (string.IsNullOrEmpty(fechaD) && string.IsNullOrEmpty(fechaH))
                    {
                        DateTime fechaDesde = DateTime.Now;
                        DateTime fechaHasta = DateTime.Now;

                        fechaD = fechaDesde.ToString("dd/MM/yyyy");
                        fechaH = fechaHasta.ToString("dd/MM/yyyy");

                    }
                    this.cargarUsuarios();
       
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtFechaDesdeMRPF.Text = fechaD;
                    txtFechaHastaMRPF.Text = fechaH;
                }

                //if (accion == 0)
                //{
                //    this.cargarInformesPedidos();
                //}


            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + Ex.Message));
            }
        }

        #region Funciones Auxiliares
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
                        //if (s == "143")
                        //{
                        return 1;
                        //}
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region Carga Inicial
       
       
        public void cargarUsuarios()
        {
            try
            {
                controladorUsuario contUsuario = new controladorUsuario();
                DataTable dt = contUsuario.obtenerUsuarios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["usuario"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListUsuario.DataSource = dt;
                this.DropListUsuario.DataValueField = "Id";
                this.DropListUsuario.DataTextField = "usuario";

                this.DropListUsuario.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista usuarios. Excepción: " + Ex.Message));
            }
        }
       
        #endregion

        #region Eventos Controles
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("InformesF.aspx?fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&us=" + this.DropListUsuario.SelectedValue + "&tipo=" + this.DropListInformes.SelectedValue);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error filtrando. Excepción: " + Ex.Message));
            }
        }
        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                controladorUsuario contUsuario = new controladorUsuario();
                //String buscar = this.txtBuscarUsuario.Text.Replace(' ', '%');
                DataTable dt = contUsuario.obtenerUsuarios();

                if (dt.Rows.Count > 1)
                {
                    //agrego todos
                    DataRow dr = dt.NewRow();
                    dr["usuario"] = "Seleccione...";
                    dr["id"] = -1;
                    dt.Rows.InsertAt(dr, 0);
                }

                this.DropListUsuario.DataSource = dt;
                this.DropListUsuario.DataValueField = "Id";
                this.DropListUsuario.DataTextField = "usuario";

                this.DropListUsuario.DataBind();
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error buscando usuarios. Excepción: " + Ex.Message));
            }
        }
        private void descargarArchivos(object sender, EventArgs e)
        {
            try
            {
                //obtengo numero factura
                string idBoton = (sender as LinkButton).ID;
                string[] atributos = idBoton.Split('_');
                string idInformePedido = atributos[1];

                DirectoryInfo di = new DirectoryInfo(Server.MapPath("../../Informes/" + idInformePedido + "/"));
                FileInfo[] files = di.GetFiles();

                foreach (FileInfo fi in files)
                {
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    string filePath = idInformePedido;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename=XMLFile.xml");
                    byte[] data = req.DownloadData(Server.MapPath("../../Informes/" + idInformePedido + "/" + fi.Name));
                    response.BinaryWrite(data);
                    response.End();
                }




            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cotizacion desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando articulos detalle desde la interfaz. " + ex.Message);
            }
        }

        #endregion

        protected void lbtnGenerarMRPF_Click(object sender, EventArgs e)
        {

        }

        protected void lbtnRankingProductos_Click(object sender, EventArgs e)
        {
            try
            {
                int idperfil = Convert.ToInt32( Session["Login_IdPerfil"].ToString());
                int IdSolicitante=(int)Session["Login_Vendedor"];
                //switch (idperfil)
                //{
                //    case 6: //es distribuidor por tanto busco como cliente 
                //        IdSolicitante = (int)Session["Login_Vendedor"];
                //        break;
                //    case 18:
                        
                //        break;
                //}
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Facturas/ImpresionPedido.aspx?a=9&tp=" +  idperfil +"&sl="+IdSolicitante + "&fd=" + txtFechaDesdeMRS.Text + "&fh=" + txtFechaHastaMRS.Text + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error generando el reporte ranking: " + ex.Message));
            }
        }
    }
}