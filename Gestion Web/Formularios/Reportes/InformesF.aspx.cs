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
    public partial class InformesF : System.Web.UI.Page
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
                    this.cargarInformes();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                }

                if (accion == 0)
                {
                    this.cargarInformesPedidos();
                }


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
                        if (s == "143")
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
        #endregion

        #region Carga Inicial
        private void cargarInformesPedidos()
        {
            try
            {
                var ip = this.controlador.obtenerInformesPedidosFiltro(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.idUsuario), this.idTipoInforme);

                if (ip != null)
                {
                    foreach (Informes_Pedidos infPed in ip)
                    {
                        this.cargarEnPh(infPed);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Informes Pedidos. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Informes Pedidos. " + ex.Message));
            }
        }
        private void cargarEnPh(Informes_Pedidos ip)
        {
            try
            {
                //Obtengo el nombre de usuario
                controladorUsuario contUsua = new controladorUsuario();
                Usuario user = contUsua.obtenerUsuariosID(Convert.ToInt32(ip.Usuario));

                //Filas
                TableRow tr = new TableRow();
                tr.ID = ip.Id.ToString();

                //Celdas
                TableCell celFechaD = new TableCell();
                celFechaD.Text = ip.Fecha.Value.ToString("dd/MM/yyyy hh:mm");
                celFechaD.VerticalAlign = VerticalAlign.Middle;
                celFechaD.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFechaD);

                TableCell celInforme = new TableCell();
                celInforme.Text = ip.NombreInforme;
                celInforme.VerticalAlign = VerticalAlign.Middle;
                celInforme.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celInforme);

                TableCell celUsuario = new TableCell();
                celUsuario.Text = user.usuario;
                celUsuario.VerticalAlign = VerticalAlign.Middle;
                celUsuario.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celUsuario);

                TableCell celEstado = new TableCell();
                celEstado.Text = "Generando";
                if (ip.Estado == 1)
                {
                    //LinkButton btnDetalles = new LinkButton();
                    HyperLink hpDetalles = new HyperLink();
                    if (ip.NombreInforme.Contains("Importacion Articulos"))
                    {
                        hpDetalles.Text = "Importado";
                    }
                    else
                    {
                        hpDetalles.CssClass = "btn btn-info ui-tooltip";
                        hpDetalles.Attributes.Add("data-toggle", "tooltip");
                        hpDetalles.Attributes.Add("title data-original-title", "Detalles");
                        hpDetalles.ID = "btnSelec_" + ip.Id.ToString();
                        hpDetalles.Text = "<span class='shortcut-icon icon-search'></span>";
                        hpDetalles.Font.Size = 12;
                        hpDetalles.NavigateUrl = "/Informes/" + ip.Id + "/" + ip.NombreInforme;
                        //btnDetalles.Click += new EventHandler(this.descargarArchivos);

                        if (ip.Informe1.Id == 1)
                            hpDetalles.NavigateUrl += ".zip";
                        if (ip.Informe1.Id == 2 || ip.Informe1.Id == 5 || ip.Informe1.Id == 8 || ip.Informe1.Id == 11)
                            hpDetalles.NavigateUrl += ".xls";
                        if (ip.Informe1.Id == 3 || ip.Informe1.Id == 4)
                            hpDetalles.NavigateUrl += ".xlsx";
                        if (ip.Informe1.Id == 6)
                            hpDetalles.NavigateUrl += ".pdf";
                        if (ip.Informe1.Id == 9 || ip.Informe1.Id == 10)
                            hpDetalles.NavigateUrl += ".txt";
                    }
                    celEstado.Controls.Add(hpDetalles);
                    tr.Controls.Add(celEstado);

                }
                celEstado.VerticalAlign = VerticalAlign.Middle;
                celEstado.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEstado);

                this.phInformes.Controls.Add(tr);
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Informes Pedidos a PH. " + Ex.Message));
            }
        }
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
        public void cargarInformes()
        {
            try
            {
                var inf = this.controlador.obtenerInformes();
                if (inf != null)
                {
                    inf.Insert(0, new Informe
                    {
                        Id = 0,
                        Nombre = "Seleccione..."
                    });


                    this.DropListInformes.DataSource = inf;
                    this.DropListInformes.DataValueField = "Id";
                    this.DropListInformes.DataTextField = "Nombre";
                    this.DropListInformes.DataBind();
                }
            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista informes. Excepción: " + Ex.Message));
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


    }
}