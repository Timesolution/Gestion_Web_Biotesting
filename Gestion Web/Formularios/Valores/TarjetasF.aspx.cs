using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class TarjetasF : System.Web.UI.Page
    {
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int idCliente;
        private string nombreTarjeta;
        private int estado;
        private int tipoFecha;
        private int operador;
        private int origenPago;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.fechaD = Request.QueryString["Fechadesde"];
                this.fechaH = Request.QueryString["FechaHasta"];
                this.suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                this.nombreTarjeta = Request.QueryString["Nombre"];
                this.estado = Convert.ToInt32(Request.QueryString["estado"]);
                this.tipoFecha = Convert.ToInt32(Request.QueryString["tf"]);
                this.operador = Convert.ToInt32(Request.QueryString["op"]);
                this.origenPago = Convert.ToInt32(Request.QueryString["origenP"]);

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 && idCliente == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarTarjetas();
                        //this.cargarClientes();
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        tipoFecha = 1;
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaAgregar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtDesdeV.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtHastaV.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                        ListSucursalAgregar.SelectedValue = suc.ToString();
                        //DropListClientes.SelectedValue = idCliente.ToString();
                        Response.Redirect("TarjetasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue + "&nombre=" + txtTarjeta.Text + "&tf=0");

                    }
                    this.cargarSucursal();
                    this.cargarTarjetas();
                    this.cargarOperadores();
                    //this.cargarClientes();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    txtDesdeV.Text = fechaD;
                    txtHastaV.Text = fechaH;
                    //DropListClientes.SelectedValue = idCliente.ToString();
                    DropListSucursal.SelectedValue = suc.ToString();
                    ListSucursalAgregar.SelectedValue = suc.ToString();
                    this.ListOperadores.SelectedValue = this.operador.ToString();
                    this.ListOrigenPago.SelectedValue = this.origenPago.ToString();
                }
                this.cargarTarjetasRango(fechaD, fechaH, suc, nombreTarjeta, estado,operador,origenPago);
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
                        if (s == "51")
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

        public void cargarTarjetas()
        {
            try
            {
                controladorTarjeta contTarj = new controladorTarjeta();
                DataTable dt = contTarj.obtenerTarjetasDT();

                DataRow dr = dt.NewRow();
                dr["id"] = "-1";
                dr["nombre"] = "Seleccione...";
                dt.Rows.InsertAt(dr, 0);

                this.ListAgregarTarjeta.DataSource = dt;
                this.ListAgregarTarjeta.DataValueField = "id";
                this.ListAgregarTarjeta.DataTextField = "nombre";
                this.ListAgregarTarjeta.DataBind();

                DataTable dt2 = contTarj.obtenerTarjetasDT2();
                this.ListEditarTarjeta.DataSource = dt2;
                this.ListEditarTarjeta.DataValueField = "id";
                this.ListEditarTarjeta.DataTextField = "nombre";
                this.ListEditarTarjeta.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tarjetas. " + ex.Message));
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

                this.ListSucursalAgregar.DataSource = dt;
                this.ListSucursalAgregar.DataValueField = "Id";
                this.ListSucursalAgregar.DataTextField = "nombre";

                this.ListSucursalAgregar.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarOperadores()
        {
            try
            {
                List<Gestion_Api.Entitys.Operadores_Tarjeta> operadores = this.controlador.obtenerOperadores();

                this.ListOperadores.DataSource = operadores;
                this.ListOperadores.DataValueField = "Id";
                this.ListOperadores.DataTextField = "Operador";
                this.ListOperadores.DataBind();

                this.ListOperadores.Items.Insert(0, new ListItem("Todos", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando operadores. " + ex.Message));

            }
        }

        //public void cargarClientes()
        //{
        //    try
        //    {
        //        controladorCliente contCliente = new controladorCliente();

        //        DataTable dt = contCliente.obtenerClientesDT();

        //        //agrego todos
        //        DataRow dr = dt.NewRow();
        //        dr["alias"] = "Seleccione...";
        //        dr["id"] = -1;
        //        dt.Rows.InsertAt(dr, 0);

        //        DataRow dr2 = dt.NewRow();
        //        dr2["alias"] = "Todos";
        //        dr2["id"] = 0;
        //        dt.Rows.InsertAt(dr2, 1);

        //        this.DropListClientes.DataSource = dt;
        //        this.DropListClientes.DataValueField = "id";
        //        this.DropListClientes.DataTextField = "alias";

        //        this.DropListClientes.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
        //    }
        //}

        private void cargarTarjetasRango(string fechaD, string fechaH, int idSuc, string nombre, int estado,int operador,int origen)
        {
            try
            {
                if (fechaD != null && fechaH != null && suc != 0 && idCliente == 0)
                {

                    DataTable dtTarjetas = controlador.obtenerDatosTarjeta2(fechaD, fechaH, idSuc, nombre,this.tipoFecha,estado,operador,origen);

                    decimal saldo = 0;
                    foreach (DataRow dr in dtTarjetas.Rows)
                    {
                        this.cargarEnPh(dr);
                        saldo += Convert.ToDecimal(dr["monto"]);
                    }
                    lblSaldo.Text = "$" + saldo.ToString();
                    this.cargarLabel(fechaD, fechaH, idSuc, idCliente);


                }
                else
                {

                    DataTable dtTarjetas = controlador.obtenerDatosTarjeta2(fechaD, fechaH, idSuc, nombre, this.tipoFecha, estado,operador,origen);

                    decimal saldo = 0;
                    foreach (DataRow dr in dtTarjetas.Rows)
                    {
                        this.cargarEnPh(dr);
                        saldo += Convert.ToDecimal(dr["monto"]);
                    }
                    lblSaldo.Text = "$" + saldo.ToString();
                    this.cargarLabel(fechaD, fechaH, idSuc, idCliente);

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
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
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
                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celFecha = new TableCell();
                DateTime fecha = Convert.ToDateTime(dr["fecha"], new CultureInfo("es-AR"));
                celFecha.Text = fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celRecibo = new TableCell();
                celRecibo.Text = dr["Numero"].ToString();
                celRecibo.VerticalAlign = VerticalAlign.Middle;
                celRecibo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celRecibo);

                TableCell celSucursal = new TableCell();
                celSucursal.Text = dr["Sucursal"].ToString();
                celSucursal.VerticalAlign = VerticalAlign.Middle;
                celSucursal.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celSucursal);

                TableCell celTipo = new TableCell();
                celTipo.Text = dr["nombre"].ToString();
                celTipo.VerticalAlign = VerticalAlign.Middle;
                celTipo.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celTipo);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$ " + dr["monto"].ToString().Replace(',', '.');
                celImporte.VerticalAlign = VerticalAlign.Middle;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celImporte);

                TableCell celEntidad = new TableCell();
                int diasAcreditacion = Convert.ToInt32(dr["diasAcreditacion"]);
                int tipoAcreditacion = Convert.ToInt32(dr["tipoAcreditacion"]);
                int diaMesAcreditacion = Convert.ToInt32(dr["fechaAcreditacion"]);
                int diaCierre = Convert.ToInt32(dr["diaCierre"]);
                //DateTime fechaAcreditacion = fecha.AddDays(diasAcreditacion);
                //celEntidad.Text = fechaAcreditacion.ToString("dd/MM/yyyy");
                if (tipoAcreditacion == 0)
                {
                    DateTime fechaAcreditacion = fecha.AddDays(diasAcreditacion);
                    celEntidad.Text = fechaAcreditacion.ToString("dd/MM/yyyy");
                }
                else
                {
                    if (fecha.Day < diaCierre)
                    {
                        DateTime fechaAcreditacion = new DateTime(fecha.Year, fecha.AddMonths(1).Month, diaMesAcreditacion);
                        celEntidad.Text = fechaAcreditacion.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        DateTime fechaAcreditacion = new DateTime(fecha.Year, fecha.AddMonths(2).Month, diaMesAcreditacion);
                        celEntidad.Text = fechaAcreditacion.ToString("dd/MM/yyyy");
                    }
                }
                celEntidad.VerticalAlign = VerticalAlign.Middle;
                celEntidad.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celEntidad);

                TableCell celLiquidacion = new TableCell();
                celLiquidacion.Text = dr["Liquidacion"].ToString();
                celLiquidacion.VerticalAlign = VerticalAlign.Middle;
                celLiquidacion.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celLiquidacion);

                TableCell celNumLote = new TableCell();
                celNumLote.Text = dr["NumeroLote"].ToString();
                celNumLote.VerticalAlign = VerticalAlign.Middle;
                celNumLote.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumLote);

                TableCell celNumCupon = new TableCell();
                celNumCupon.Text = dr["NumeroCupon"].ToString();
                celNumCupon.VerticalAlign = VerticalAlign.Middle;
                celNumCupon.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumCupon);

                TableCell celNumTarjeta = new TableCell();
                celNumTarjeta.Text = dr["NumeroTarjeta"].ToString();
                celNumTarjeta.VerticalAlign = VerticalAlign.Middle;
                celNumTarjeta.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumTarjeta);

                TableCell celObservaciones = new TableCell();
                celObservaciones.Text = dr["Observaciones"].ToString();
                celObservaciones.VerticalAlign = VerticalAlign.Middle;
                celObservaciones.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celObservaciones);

                TableCell celOrigenPago = new TableCell();
                if (dr["OrigenPago"].ToString() == "0")
                    celOrigenPago.Text = "";
                if (dr["OrigenPago"].ToString() == "1")
                    celOrigenPago.Text = "Por venta";
                if (dr["OrigenPago"].ToString() == "2")
                    celOrigenPago.Text = "Por Anticipo";
                if (dr["OrigenPago"].ToString() == "3")
                    celOrigenPago.Text = "Por otras cobr.";
                celOrigenPago.VerticalAlign = VerticalAlign.Middle;
                celOrigenPago.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celOrigenPago);                

                TableCell celAccion = new TableCell();               

                CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.Text = "&nbsp;Imputar";
                cbSeleccion.ID = "cbSeleccion_" + dr["tarjeta"].ToString() + "_" + dr["pago"].ToString();
                cbSeleccion.CssClass = "btn btn-info";
                cbSeleccion.Font.Size = 12;

                celAccion.Controls.Add(cbSeleccion);
                //celAccion.Width = Unit.Percentage(5);
                tr.Cells.Add(celAccion);

                phTarjetas.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Datos de Tarjetas en PH. " + ex.Message));
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

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        if (this.RadioFecha.Checked == true)
                        {
                            Response.Redirect("TarjetasF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue + "&nombre=" + txtTarjeta.Text + "&tf=0&op=" + this.ListOperadores.SelectedValue + "&origenP=" + this.ListOrigenPago.SelectedValue);
                        }
                        else
                        {
                            Response.Redirect("TarjetasF.aspx?fechadesde=" + txtDesdeV.Text + "&fechaHasta=" + txtHastaV.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&estado=" + DropListEstado.SelectedValue + "&nombre=" + txtTarjeta.Text + "&tf=1&op=" + this.ListOperadores.SelectedValue + "&origenP=" + this.ListOrigenPago.SelectedValue);
                        }
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de movimientos tarjeta. " + ex.Message));

            }
        }
                
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";
                string idModificar = "";
                foreach (Control c in phTarjetas.Controls)
                {
                    TableRow tr = c as TableRow;
                    string liquidado = tr.Cells[6].Text;
                    string lote = tr.Cells[7].Text;
                    string cupon = tr.Cells[8].Text;
                    string digitos = tr.Cells[9].Text;

                    CheckBox ch = tr.Cells[12].Controls[0] as CheckBox;
                    if (ch.Checked == true) 
                    {
                        if (liquidado == "" && lote == "" && cupon == "" && digitos == "")
                        {
                            idtildado += ch.ID.Split('_')[1] + "_" + ch.ID.Split('_')[2] + ";";
                        }
                        else
                        {
                            idModificar += ch.ID.Split('_')[1] + "_" + ch.ID.Split('_')[2] + ";";
                        }
                    }

                    //if (ch.Checked == true && liquidado != "")
                    //{
                    //    idModificar += ch.ID.Split('_')[1] + "_" + ch.ID.Split('_')[2] + ";";
                    //}
                }
                if (idtildado != "")
                {
                    int i = 0;
                    foreach (String id in idtildado.Split(';'))
                    {
                        if (id != "")
                        {
                            Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones liquidacion = new Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones();
                            string tarjeta = id.Split('_')[0];
                            string pago = id.Split('_')[1];

                            liquidacion.IdPago = Convert.ToInt32(pago);
                            liquidacion.IdTarjeta = Convert.ToInt32(tarjeta);
                            liquidacion.Liquidacion = this.txtNumeroLiquidacion.Text;
                            //liquidacion.NumeroCupon = this.txtNumeroCupon.Text;
                            //liquidacion.NumeroLote = this.txtNumeroLote.Text;
                            //liquidacion.NumeroTarjeta = this.txtUltimosDigitos.Text;
                            //funcion con entity
                            i = this.controlador.AgregarLiquidacionPagoTarjeta(liquidacion);
                            //funcion vieja con stored procedure
                            //i = this.controlador.LiquidarPagosTarjetas(Convert.ToInt32(pago),Convert.ToInt32(tarjeta), this.txtNumeroLiquidacion.Text,0);
                            if (i < 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Una o mas liquidaciones no se agregaron debido a un problema. "));
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

                if (idModificar != "")
                {
                    int i = 0;
                    foreach (String id in idModificar.Split(';'))
                    {
                        if (id != "")
                        {
                            string tarjeta = id.Split('_')[0];
                            string pago = id.Split('_')[1];

                            Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones liquidacion = this.controlador.ObtenerLiquidacionPagoTarjeta(Convert.ToInt32(tarjeta), Convert.ToInt32(pago));
                            liquidacion.IdPago = Convert.ToInt32(pago);
                            liquidacion.IdTarjeta = Convert.ToInt32(tarjeta);
                            liquidacion.Liquidacion = this.txtNumeroLiquidacion.Text;
                            //liquidacion.NumeroCupon = this.txtNumeroCupon.Text;
                            //liquidacion.NumeroLote = this.txtNumeroLote.Text;
                            //liquidacion.NumeroTarjeta = this.txtUltimosDigitos.Text;

                            i = this.controlador.ModificarLiquidacionPagoTarjeta(liquidacion);
                            //i = this.controlador.LiquidarPagosTarjetas(Convert.ToInt32(pago), Convert.ToInt32(tarjeta), this.txtNumeroLiquidacion.Text,1);
                            if (i < 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Una o mas liquidaciones no se pudieron modificaron debido a un problema. "));
                            }
                        }

                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                }

                if (idModificar == "" && idtildado == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se selecciono ninguna operacion con tarjeta. "));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error liquidando pagos con tarjeta. " + ex.Message));
            }
        }

        protected void btnAgregarArchivo_Click(object sender, EventArgs e)
        {
            try
            {
                this.cargarArchivo();
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarArchivo()
        {
            try
            {
                if (this.nombreTarjeta != "" && this.nombreTarjeta.Contains("VISA"))
                {
                    controladorFunciones contFunc = new controladorFunciones();
                    Boolean fileOK = false;

                    String path = Server.MapPath("../../Tarjetas/");///Liquidacion_" + DateTime.Today.Month + "-" + DateTime.Today.Day + "/");

                    if (FileUpload1.HasFile)
                    {
                        String fileExtension =
                            System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                        String[] allowedExtensions = { ".csv" };

                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            if (fileExtension == allowedExtensions[i])
                            {
                                fileOK = true;
                            }
                        }
                    }
                    if (fileOK)
                    {
                        //guardo nombre archivo
                        string archivo = FileUpload1.FileName;
                        //lo subo
                        FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);

                        //obtengo las tarjetas en pantalla y voy a liquidar
                        int i = contFunc.procesarArchivoLiquidacion(path + archivo, this.fechaD, this.fechaH, this.suc, this.nombreTarjeta, this.tipoFecha, this.estado);

                        if (i > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito",Request.Url.ToString()));
                            //ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito", Request.Url.ToString()),true);
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron procesar una o mas liquidaciones."));
                            //ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxAtencion("No se pudieron procesar una o mas liquidaciones."), true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El archivo debe ser .CSV "));
                        //ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", m.mensajeBoxAtencion("El archivo debe ser .CSV "), true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Primero debe filtrar por nombre de tarjeta para poder completar la accion."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionValores.aspx?a=7&valor=1&fd=" + this.fechaD + "&fh=" + this.fechaH + "&Nombre=" + this.nombreTarjeta + "&estado=" + this.estado + "&tf=" + this.tipoFecha + "&S=" + this.suc + "&ope=" + this.ListOperadores.SelectedValue + "&origenP=" + this.ListOrigenPago.SelectedValue);
            }
            catch
            {

            }
        }

        protected void lbtnAgregarPagoTarjeta_Click(object sender, EventArgs e)
        {
            try
            {
                int suc = Convert.ToInt32(this.ListSucursalAgregar.SelectedValue);
                string fecha = this.txtFechaAgregar.Text;
                Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones liquidacion = new Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones();                

                liquidacion.IdTarjeta = Convert.ToInt32(this.ListAgregarTarjeta.SelectedValue);
                liquidacion.Liquidacion = this.txtAgregarNumeroLiquidacion.Text;
                liquidacion.NumeroCupon = this.txtAgregarNumeroCupon.Text;
                liquidacion.NumeroLote = this.txtAgregarNumeroLote.Text;
                liquidacion.NumeroTarjeta = this.txtAgregarUltimosDigitos.Text;
                liquidacion.Observaciones = this.txtAgregarObservaciones.Text;

                Pago_Tarjeta pago = new Pago_Tarjeta();
                pago.monto = Convert.ToDecimal(this.txtImporteAgregar.Text);
                pago.tarjeta.id = Convert.ToInt32(this.ListAgregarTarjeta.SelectedValue);

                int origenPago = Convert.ToInt32(this.ListAgregarOrigenTarjeta.SelectedValue);

                int idUser = (int)Session["Login_IdUser"];
                Log.EscribirSQL(idUser, "INFO", "Inicio proceso agregar tarjeta manual.");

                int i = this.controlador.agregarTarjetaManual(pago, liquidacion, suc, fecha, origenPago);
                if (i > 0)
                {                    
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", "TarjetasF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar pago de tarjeta. "));
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error agregando pagos con tarjeta. " + ex.Message));
            }
        }

        protected void lbtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                int idPago = Convert.ToInt32(this.lblIdPago.Text);
                int idTarjeta = Convert.ToInt32(this.lblIdTarjeta.Text);

                Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones liquidacion = this.controlador.ObtenerLiquidacionPagoTarjeta(idTarjeta, idPago);

                if (liquidacion != null)
                {
                    //liquidacion.IdPago = Convert.ToInt32(idPago);
                    //liquidacion.IdTarjeta = Convert.ToInt32(idTarjeta);
                    //liquidacion.Liquidacion = this.txtNumeroLiquidacion.Text;
                    liquidacion.NumeroCupon = this.txtNumeroCupon.Text;
                    liquidacion.NumeroLote = this.txtNumeroLote.Text;
                    liquidacion.NumeroTarjeta = this.txtUltimosDigitos.Text;
                    liquidacion.Observaciones = this.txtObservaciones.Text;
                    liquidacion.IdTarjeta = Convert.ToInt32(this.ListEditarTarjeta.SelectedValue);

                    int i = this.controlador.ModificarLiquidacionPagoTarjeta(liquidacion);
                    if (i >= 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar debido a un problema. "));
                    }
                }
                else
                {
                    Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones liquidacionNueva = new Gestion_Api.Entitys.Pago_Tarjetas_Liquidaciones();
                    liquidacionNueva.IdPago = Convert.ToInt32(idPago);
                    liquidacionNueva.IdTarjeta = Convert.ToInt32(idTarjeta);
                    liquidacionNueva.NumeroCupon = this.txtNumeroCupon.Text;
                    liquidacionNueva.NumeroLote = this.txtNumeroLote.Text;
                    liquidacionNueva.NumeroTarjeta = this.txtUltimosDigitos.Text;
                    liquidacionNueva.Observaciones = this.txtObservaciones.Text;

                    int i = this.controlador.AgregarLiquidacionPagoTarjeta(liquidacionNueva);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito!. ", Request.Url.ToString()));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo modificar debido a un problema. "));
                    }
                }
                

            }
            catch
            {

            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                string idtildado = "";

                string lote = "";
                string cupon = "";
                string digitos = "";
                string obser = "";

                foreach (Control c in phTarjetas.Controls)
                {
                    TableRow tr = c as TableRow;                   

                    CheckBox ch = tr.Cells[12].Controls[0] as CheckBox;

                    if (ch.Checked == true)
                    {                        
                        lote = tr.Cells[7].Text;
                        cupon = tr.Cells[8].Text;
                        digitos = tr.Cells[9].Text;
                        obser = tr.Cells[10].Text;
                        idtildado = ch.ID.Split('_')[1] + "_" + ch.ID.Split('_')[2];
                    }
                }
                if (idtildado != "")
                {
                    this.lblIdPago.Text = idtildado.Split('_')[1];
                    this.lblIdTarjeta.Text = idtildado.Split('_')[0];
                    this.txtNumeroCupon.Text = cupon;
                    this.txtNumeroLote.Text = lote;
                    this.txtUltimosDigitos.Text = digitos;
                    this.txtObservaciones.Text = obser;
                    this.ListEditarTarjeta.SelectedValue = idtildado.Split('_')[0];

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "openModal", "openModal();", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una!. "));
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una."), true);
                }
                
            }
            catch
            {

            }
        }
    }
}