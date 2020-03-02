using Disipar.Models;
using Gestion_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Gestion_Web.Formularios.Seguridad
{
    public partial class AuditoriaF : System.Web.UI.Page
    {
        //controladorCobranza controlador = new controladorCobranza();
        controladorUsuario controlador = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int idUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idUsuario = Convert.ToInt32(Request.QueryString["Usuario"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 && idUsuario == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarUsuarios();
                        this.idUsuario = -1;
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                        DropListUsuarios.SelectedValue = idUsuario.ToString();

                    }
                    this.cargarSucursal();
                    this.cargarUsuarios();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListUsuarios.SelectedValue = idUsuario.ToString();
                    DropListSucursal.SelectedValue = suc.ToString();
                }
                if(idUsuario > -1)
                {
                    this.cargarAuditoriasRango(fechaD, fechaH, suc, idUsuario);
                }
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
                        if (s == "61")
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

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarUsuarios()
        {
            try
            {
                DataTable dt = this.controlador.obtenerUsuarios();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["usuario"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["usuario"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.DropListUsuarios.DataSource = dt;
                this.DropListUsuarios.DataValueField = "Id";
                this.DropListUsuarios.DataTextField = "usuario";

                this.DropListUsuarios.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Usuarios. " + ex.Message));
            }
        }
        private void cargarAuditoriasRango(string fechaD, string fechaH, int idSuc, int idUsuario)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idUsuario == 0)
                {
                    DataTable dtCheques = controlador.obtenerDatosAuditoria(fechaD, fechaH, idSuc, idUsuario);
                    foreach (DataRow dr in dtCheques.Rows)
                    {
                        this.cargarEnPh(dr);
                    }

                    this.cargarLabel(fechaD, fechaH, idSuc, idUsuario);
                }
                else
                {
                    DataTable dtCheques = controlador.obtenerDatosAuditoria(fechaD, fechaH, idSuc, idUsuario);
                    foreach (DataRow dr in dtCheques.Rows)
                    {
                        this.cargarEnPh(dr);
                    }

                    this.cargarLabel(fechaD, fechaH, idSuc, idUsuario);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Auditoria de Usuarios. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idUsuarios)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idUsuarios > -1)
                {
                    label += DropListUsuarios.Items.FindByValue(idUsuarios.ToString()).Text;
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(DataRow dr)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["GestionEntities"].ToString();
                if (conStr.ToLower().Contains("tcp:serversql.database.windows.net,1433"))
                {
                        DateTime fechaModificada = Convert.ToDateTime(dr["fechaHora"]);
                        dr["fechaHora"] = fechaModificada.AddHours(-3);
                }

                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(dr["fechaHora"], new CultureInfo("es-AR"));
                celFecha.Text = fecha.ToString("dd/MM/yyyy hh:mm:ss");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celRecibo = new TableCell();
                celRecibo.Text = dr["descripcion"].ToString();
                celRecibo.VerticalAlign = VerticalAlign.Middle;
                celRecibo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRecibo);

                phAuditoria.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Auditoria de Usuarios en PH. " + ex.Message));
            }

        }

        protected void UpdatePanel3_Load(object sender, EventArgs e)
        {
            //
            this.labelIva.Text = "testc escribo desde el update panel amigo!!";


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("AuditoriaF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Usuario=" + DropListUsuarios.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos de caja. " + ex.Message));

            }
        }
    }
}