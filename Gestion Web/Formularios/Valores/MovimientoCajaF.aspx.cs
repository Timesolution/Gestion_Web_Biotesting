using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class MovimientoCajaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        //controlador
        ControladorCaja controlador = new ControladorCaja();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
        //valores
        private int valor;
        private int idMovimiento;

        class ListItemTemporal
        {
            public string id;
            public string nombre;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.valor = Convert.ToInt32(Request.QueryString["valor"]);
            this.idMovimiento = Convert.ToInt32(Request.QueryString["id"]);
            try
            {
                this.VerificarLogin();
                this.cargarMovimientos();
                if (!IsPostBack)
                {
                    this.cargarCuentas();
                    //this.cargarCuentasNivel1();

                    CargarDropLists();
                    if (valor == 2)
                    {
                        this.cargarMovimientoEditar();
                    }
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
                        if (s == "6")
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

        private void cargarMovimientos()
        {
            try
            {
                phMovimientosCaja.Controls.Clear();
                List<MovimientoCaja> movimientos = this.controlador.obtenerMovimientosCajaList();
                foreach (MovimientoCaja m in movimientos)
                {
                    this.cargarMovimientosPH(m);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando Movimientos de Caja. " + ex.Message));

            }
        }
        private void cargarCuentas()
        {
            try
            {
                List<Cuentas_Contables> cuentas = this.contPlanCuentas.obtenerCuentasContables();
                if (cuentas.Count > 0)
                {
                    this.PanelCtaContable.Visible = true;
                    //this.lbtnAgregar.Visible = false;
                    this.phColumna.Visible = true;
                }
            }
            catch
            {

            }
        }

        private void cargarMovimientosPH(MovimientoCaja mov)
        {
            try
            {
                TableRow tr = new TableRow();


                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = mov.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celTipo = new TableCell();
                celTipo.Text = "Ingreso";
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);
                if (mov.tipo == 2)
                {
                    celTipo.Text = "Egreso";
                }

                List<Cuentas_Contables> cuentas = this.contPlanCuentas.obtenerCuentasContables();
                if (cuentas.Count > 0)
                {
                    TableCell celCuenta = new TableCell();
                    var cta = this.contPlanCuentas.obtenerCuentaContableTipoMovCaja(mov.id);
                    if (cta != null)
                    {
                        celCuenta.Text = cta.Cuentas_Contables.Codigo + " - " + cta.Cuentas_Contables.Descripcion;
                    }
                    celCuenta.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celCuenta);
                }

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = mov.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarGrupos);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + mov.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + mov.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phMovimientosCaja.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Movimiento en la lista. " + ex.Message));
            }
        }
        private void cargarMovimientoEditar()
        {
            try
            {
                MovimientoCaja mov = this.controlador.obtenerMovimientoCajaID(this.idMovimiento);
                this.txtMov.Text = mov.descripcion;
                this.dropListTipo_Debe_Haber.SelectedValue = mov.tipo.ToString();
                var cta = this.contPlanCuentas.obtenerCuentaContableTipoMovCaja(mov.id);
                if (cta != null)
                {
                    this.cargarCuentasNivel1();
                    this.ListCtaContables1.SelectedValue = cta.Cuentas_Contables.Nivel1.ToString();
                    this.cargarCuentasNivel2();
                    this.ListCtaContables2.SelectedValue = cta.Cuentas_Contables.Nivel2.ToString();
                    this.cargarCuentasNivel3();
                    this.ListCtaContables3.SelectedValue = cta.Cuentas_Contables.Nivel3.ToString();
                    this.cargarCuentasNivel4();
                    this.ListCtaContables4.SelectedValue = cta.Cuentas_Contables.Nivel4.ToString();
                    this.cargarCuentasNivel5();
                    this.ListCtaContables5.SelectedValue = cta.IdCuentaContable.ToString();
                }
            }
            catch
            {

            }
        }
        //static public void AgregarOModificarMovimiento()
        [WebMethod]
        static public string AgregarOModificarMovimiento(int queryString_idMovimiento_Caja, int queryString_valor, string textDescripcionDelMovimiento, int valorDropListTipo_Debe_Haber, int idCuentaContable_Nivel5)
        {
            try
            {
                ControladorCaja controladorCaja = new ControladorCaja();
                MovimientoCaja mov = controladorCaja.obtenerMovimientoCajaID(queryString_idMovimiento_Caja);
                int i = 0;
                if (queryString_valor == 2)
                {
                    mov.descripcion = textDescripcionDelMovimiento;
                    mov.tipo = valorDropListTipo_Debe_Haber;
                    mov.estado = 1;
                    i = controladorCaja.modificarMovimientoCaja(mov);
                    if (i > 0)
                    {
                        //agrego bien  
                        if (modificarCtaContableTipoMov(mov, idCuentaContable_Nivel5) > 0)
                        {
                            return ConvertirStringToJSON("Cuenta modificada con exito");
                        }
                        return ConvertirStringToJSON("Cuenta editada con exito");
                    }
                    else
                    {
                        return ConvertirStringToJSON("No se pudo modificar la cuenta");
                    }
                }
                else
                {
                    mov = new MovimientoCaja();
                    mov.descripcion = textDescripcionDelMovimiento;
                    mov.tipo = valorDropListTipo_Debe_Haber;
                    mov.estado = 1;
                    i = controladorCaja.agregarMovimientoCaja(mov);
                    if (i > 0)
                    {
                        //agrego bien
                        agregarCtaContableTipoMov(mov, idCuentaContable_Nivel5);
                        return ConvertirStringToJSON("Cuenta creada con exito");
                    }
                    else
                    {
                        return ConvertirStringToJSON("No se pudo crear la cuenta");
                    }
                }
            }
            catch
            {
                return ConvertirStringToJSON("Error en la fun: AgregarOModificarMovimiento");
            }
        }

        public static string ConvertirStringToJSON(string texto)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(texto);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //void btnAgregarOModificarMovimiento_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //this.valor = Convert.ToInt32(Request.QueryString["valor"]);
        //        if (valor == 2)
        //        {
        //            //MovimientoCaja mov = this.controlador.obtenerMovimientoCajaID(this.idMovimiento);
        //            //mov.descripcion = this.txtMov.Text;
        //            //mov.tipo = Convert.ToInt32(this.ListTipos.SelectedValue);
        //            mov.estado = 1;
        //            int i = this.controlador.modificarMovimientoCaja(mov);
        //            if (i > 0)
        //            {
        //                //agrego bien  
        //                this.modificarCtaContableTipoMov(mov);
        //                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  Movimiento de Caja: " + mov.descripcion);
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Movimiento de Caja modificado con exito", "MovimientoCajaF.aspx"));
        //                this.borrarCampos();
        //            }
        //            else
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando Movimiento de Caja"));
        //            }
        //        }
        //        else
        //        {
        //            MovimientoCaja mov = new MovimientoCaja();
        //            mov.descripcion = this.txtMov.Text;
        //            mov.tipo = Convert.ToInt32(this.ListTipos.SelectedValue);
        //            mov.estado = 1;
        //            int i = this.controlador.agregarMovimientoCaja(mov);
        //            if (i > 0)
        //            {
        //                //agrego bien
        //                this.agregarCtaContableTipoMov(mov);
        //                Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Movimiento de Caja: " + this.txtMovimiento.Text);
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Movimiento de Caja cargado con exito", "MovimientoCajaF.aspx"));

        //            }
        //            else
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Movimiento de Caja"));

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Movimiento de Caja. " + ex.Message));
        //    }
        //}

        private void editarGrupos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("MovimientoCajaF.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar Movimiento de Caja. " + ex.Message));
            }
        }

        static public int agregarCtaContableTipoMov(MovimientoCaja mov, int idCuentaContable_Nivel5)
        {
            try
            {
                ControladorPlanCuentas controladorPlanCuentas = new ControladorPlanCuentas();
                if (idCuentaContable_Nivel5 != 0)
                {
                    int idCta = idCuentaContable_Nivel5;
                    if (idCta > 0)
                    {
                        return controladorPlanCuentas.agregarCuentaContableTipoMovCaja(mov.id, idCta);
                    }
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        [WebMethod]
        static public int modificarCtaContableTipoMov(MovimientoCaja mov, int idCuentaContable_Nivel5)
        {
            try
            {
                if (idCuentaContable_Nivel5 > 0)
                {
                    ControladorPlanCuentas controladorPlanCuentas = new ControladorPlanCuentas();
                    var cta = controladorPlanCuentas.obtenerCuentaContableTipoMovCaja(mov.id);
                    if (cta != null)
                    {
                        cta.IdCuentaContable = idCuentaContable_Nivel5;
                        return controladorPlanCuentas.modificarCuentaContableTipoMovCaja(cta);
                    }
                    return agregarCtaContableTipoMov(mov, idCuentaContable_Nivel5);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idGrupo = Convert.ToInt32(this.txtMovimiento.Text);
                MovimientoCaja m = this.controlador.obtenerMovimientoCajaID(idGrupo);
                m.estado = 0;
                int i = this.controlador.modificarMovimientoCaja(m);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Movimiento de Caja: " + m.descripcion);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxInfo("Movimiento de Caja eliminado con exito", "MovimientoCajaF.aspx"));

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", this.m.mensajeBoxError("Error eliminando Movimiento de Caja"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Movimiento de Caja. " + ex.Message));
            }
        }

        #region funciones droplist
        protected void ListCtaContables1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel2();
            }
            catch
            {

            }
        }
        protected void ListCtaContables2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel3();
            }
            catch
            {

            }
        }
        protected void ListCtaContables3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel4();
            }
            catch
            {

            }
        }
        protected void ListCtaContables4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarCuentasNivel5();
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel1()
        {
            try
            {
                var ctas = this.contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);

                this.ListCtaContables1.DataSource = ctas.ToList();
                this.ListCtaContables1.DataValueField = "Id";
                this.ListCtaContables1.DataTextField = "Descripcion";
                this.ListCtaContables1.DataBind();

                this.ListCtaContables1.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }
        private void cargarCuentasNivel2()
        {
            try
            {
                var ctas = this.contPlanCuentas.obtenerCuentasContablesByNivel(2, Convert.ToInt32(this.ListCtaContables1.SelectedValue));

                this.ListCtaContables2.DataSource = ctas.ToList();
                this.ListCtaContables2.DataValueField = "Id";
                this.ListCtaContables2.DataTextField = "Descripcion";
                this.ListCtaContables2.DataBind();

                this.ListCtaContables2.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel3()
        {
            try
            {
                var ctas = this.contPlanCuentas.obtenerCuentasContablesByNivel(3, Convert.ToInt32(this.ListCtaContables2.SelectedValue));

                this.ListCtaContables3.DataSource = ctas.ToList();
                this.ListCtaContables3.DataValueField = "Id";
                this.ListCtaContables3.DataTextField = "Descripcion";
                this.ListCtaContables3.DataBind();
                this.ListCtaContables3.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel4()
        {
            try
            {
                var ctas = this.contPlanCuentas.obtenerCuentasContablesByNivel(4, Convert.ToInt32(this.ListCtaContables3.SelectedValue));

                this.ListCtaContables4.DataSource = ctas.ToList();
                this.ListCtaContables4.DataValueField = "Id";
                this.ListCtaContables4.DataTextField = "Descripcion";
                this.ListCtaContables4.DataBind();
                this.ListCtaContables4.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }
        private void cargarCuentasNivel5()
        {
            try
            {
                var ctas = this.contPlanCuentas.obtenerCuentasContablesByNivel(5, Convert.ToInt32(this.ListCtaContables4.SelectedValue));

                this.ListCtaContables5.DataSource = ctas.ToList();
                this.ListCtaContables5.DataValueField = "Id";
                this.ListCtaContables5.DataTextField = "Descripcion";
                this.ListCtaContables5.DataBind();
                this.ListCtaContables5.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }

        private void CargarDropLists()
        {
            CargarNivelesDeLosDropDown();
        }
        public void CargarNivelesDeLosDropDown()
        {
            try
            {
                DropDownList[] ddls = { ListCtaContables1, ListCtaContables2, ListCtaContables3, ListCtaContables4, ListCtaContables5 };
                List<Cuentas_Contables> lista = new List<Cuentas_Contables>();

                for (int i = 0; i < ddls.Length; i++)
                {
                    if (i == 0)
                    {
                        lista = contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);
                    }
                    if (lista != null)
                    {
                        ddls[i].DataSource = lista;
                        ddls[i].DataTextField = "Descripcion";
                        ddls[i].DataValueField = "Id";
                        ddls[i].DataBind();
                    }
                    lista = contPlanCuentas.obtenerCuentasContablesByNivel(i + 2, Convert.ToInt32(ddls[i].SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }
        [WebMethod]
        public static string ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel(int jerarquia, int nivel)
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                var listaCuentas = contPlanCuentas.obtenerCuentasContablesByNivel(jerarquia, nivel);

                List<ListItemTemporal> listaCuentasTemporal = new List<ListItemTemporal>();

                foreach (var item in listaCuentas)
                {
                    listaCuentasTemporal.Add(new ListItemTemporal
                    {
                        id = item.Id.ToString(),
                        nombre = item.Descripcion
                    });
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaCuentasTemporal);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}