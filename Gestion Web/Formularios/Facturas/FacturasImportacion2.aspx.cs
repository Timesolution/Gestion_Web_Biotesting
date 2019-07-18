using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class FacturasImportacion2 : System.Web.UI.Page
    {
        //controladorImportacion cont = new controladorImportacion();
        Mensajes m = new Mensajes();


        controladorImportacion contImp = new controladorImportacion();
        controladorUsuario contUser = new controladorUsuario();

        private int suc;
        private DateTime fechaD;
        private DateTime fechaH;
        private int idCliente;
        private string nombreTarjeta;
        private int estado;
        private int tipoFecha;
        private int operador;
        private int origenPago;

        private int idEmpresa;
        private int idSucursal;
        private int puntoVenta;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Convert.ToDateTime(Request.QueryString["Fechadesde"], new CultureInfo("es-AR"));
                this.fechaH = Convert.ToDateTime(Request.QueryString["FechaHasta"], new CultureInfo("es-AR"));
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.estado = Convert.ToInt32(Request.QueryString["estado"]);

                this.idCliente = Convert.ToInt32(Request.QueryString["cliente"]);
                this.idEmpresa = Convert.ToInt32(Request.QueryString["empresa"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["sucursal"]);
                this.puntoVenta = Convert.ToInt32(Request.QueryString["puntoVenta"]);

                if (!IsPostBack)
                {
                    if (fechaD == DateTime.MinValue && fechaH == DateTime.MinValue && suc == 0 && idCliente == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();

                        //this.cargarClientes();
                        fechaD = DateTime.Now;
                        fechaH = DateTime.Today.AddHours(23).AddMinutes(59);
                        tipoFecha = 1;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        ListSucursal.SelectedValue = suc.ToString();

                        //DropListClientes.SelectedValue = idCliente.ToString();
                        Response.Redirect("FacturasImportacion2.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + ListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue);

                    }
                    this.cargarEmpresas();
                    this.DropListEmpresa.SelectedValue = this.idEmpresa.ToString();
                    this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
                    this.ListSucursal.SelectedValue = this.idSucursal.ToString();
                    this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
                    this.DropListPuntoVta.SelectedValue = this.puntoVenta.ToString();
                    
                    

                    //this.cargarClientes();
                    txtFechaDesde.Text = fechaD.ToString("dd/MM/yyyy");
                    txtFechaHasta.Text = fechaH.ToString("dd/MM/yyyy");

                    //DropListClientes.SelectedValue = idCliente.ToString();
                    ListSucursal.SelectedValue = suc.ToString();



                }
                this.cargarLotesRango(fechaD, fechaH, estado);
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
                        if (s == "39")
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

        public void cargarEmpresas()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerEmpresas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["Razon Social"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.DropListEmpresa.DataSource = dt;
                this.DropListEmpresa.DataValueField = "Id";
                this.DropListEmpresa.DataTextField = "Razon Social";

                this.DropListEmpresa.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empresas. " + ex.Message));
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


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";

                this.ListSucursal.DataBind();



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        private void cargarLotesRango(DateTime fechaD, DateTime fechaH, int estado)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idCliente == 0)
                {
                    var lotes = this.contImp.FiltrarLotesImportacion(fechaD, fechaH.AddHours(23).AddMinutes(59), estado);

                    decimal saldo = 0;
                    foreach (var l in lotes)
                    {
                        this.cargarEnPh(l);
                    }
                    //lblSaldo.Text = "$" + saldo.ToString();
                    //this.cargarLabel(fechaD, fechaH, idSuc, idCliente);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo lista de Tarjetas. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idCliente)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idSucursal > 0)
                {
                    label += ListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));
            }
        }

        private void cargarEnPh(LotesImportacion l)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();

                //Celdas
                TableCell celLote = new TableCell();
                celLote.Text = l.Lote.ToString();
                celLote.VerticalAlign = VerticalAlign.Middle;
                celLote.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celLote);

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(l.Fecha);
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celRecibo = new TableCell();
                celRecibo.Text = l.NombreArchivo;
                celRecibo.VerticalAlign = VerticalAlign.Middle;
                celRecibo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRecibo);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = l.Comrobantes.ToString();
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);

                TableCell celTipo = new TableCell();
                celTipo.Text = l.ComprobantesCorrectos.ToString();
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celImporte = new TableCell();
                celImporte.Text = l.ComprobantesIncorrectos.ToString();
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celLiquidacion = new TableCell();

                if (l.Estado >= 0)
                {
                    celLiquidacion.Text = "CORRECTO";
                }
                else
                {
                    celLiquidacion.Text = "INCORRECTO";
                }
                celLiquidacion.VerticalAlign = VerticalAlign.Middle;
                celLiquidacion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celLiquidacion);

                TableCell celNumLote = new TableCell();
                celNumLote.Text = l.Resultado;
                celNumLote.VerticalAlign = VerticalAlign.Middle;
                celNumLote.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumLote);

                //TableCell celAccion = new TableCell();

                //CheckBox cbSeleccion = new CheckBox();
                ////cbSeleccion.Text = "&nbsp;Imputar";
                //cbSeleccion.ID = "cbSeleccion_" + l.Id;
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;

                //celAccion.Controls.Add(cbSeleccion);
                ////celAccion.Width = Unit.Percentage(5);
                //tr.Cells.Add(celAccion);

                this.phLotes.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lotes en PH. " + ex.Message));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "abrirdialog()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FacturasImportacion2.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text +  "&estado=" + DropListEstado.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de lotes importados. " + ex.Message));

            }
        }


        protected void btnImportarXML_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;

                String path = Server.MapPath("/XML/");
                
                fileOK = true;
                if (fileOK)
                {
                    //guardo nombre archivo
                    string archivo = FileUpload1.FileName;
                    //lo subo
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);

                    int empresa = Convert.ToInt32(this.DropListEmpresa.SelectedValue);
                    int sucursal = Convert.ToInt32(this.ListSucursal.SelectedValue);
                    int puntoVenta = Convert.ToInt32(this.DropListPuntoVta.SelectedValue);

                    int i = this.contImp.obtenerFacturasDesdeArchivo(path + archivo,empresa,sucursal,puntoVenta);

                    int cantRegistros = 0;

                    if (i > 0)
                    {
                        string script = "";
                        script += "window.onload=function(){";
                        script += "window.open('/Formularios/Facturas/FacturasF.aspx','_blank');";
                        //script += "window.open('/Formularios/Facturas/RemitosR.aspx','_blank');";

                        script += " $.msgbox(\"Archivo Importado. \", {type: \"info\"}); location.href = '/Formularios/Facturas/FacturasImportacion2.aspx';";
                        //sb.Append(message);
                        script += "};";

                        //script += m.mensajeBoxInfo("Importacion Finalizada con exito", "/FacturasImportacion.aspx");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);

                    }
                    else
                    {
                        //if (i == -2)
                        //{
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El lote ya fue importado."));
                        //    return;
                        //}
                        //if (i == -3)
                        //{
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo importar la totalidad del lote. Verificque estado del mismo."));
                        //    return;
                        //}
                        //if (i == -10)
                        //{
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El punto de venta tiene CAI vencido"));
                        //    return;
                        //}

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron procesar una o mas lotes."));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar un archivo!. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error subiendo archivo. " + ex.Message));
            }
        }

        protected void DropListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarSucursal(Convert.ToInt32(this.DropListEmpresa.SelectedValue));
        }

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarPuntoVta(Convert.ToInt32(this.ListSucursal.SelectedValue));
        }

        public void cargarSucursal(int emp)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursalesDT(emp);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";

                this.ListSucursal.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVta(int sucu)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

                //agrego seleccione...
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["Id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                //agrego todos
                //DataRow dr1 = dt.NewRow();
                //dr1["NombreFantasia"] = "Todos";
                //dr1["Id"] = 0;
                //dt.Rows.InsertAt(dr1, 1);

                this.DropListPuntoVta.DataSource = dt;
                this.DropListPuntoVta.DataValueField = "Id";
                this.DropListPuntoVta.DataTextField = "NombreFantasia";

                this.DropListPuntoVta.DataBind();

                if (dt.Rows.Count == 2)
                {
                    this.DropListPuntoVta.SelectedIndex = 1;
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando puntos de venta. " + ex.Message));
            }
        }

        
    }
}