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
    public partial class MovimientoCajaF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        ControladorCaja controlador = new ControladorCaja();
        controladorUsuario contUser = new controladorUsuario();
        ControladorPlanCuentas contPlanCta = new ControladorPlanCuentas();
        //valores
        private int valor;
        private int idMovimiento;
        private int idUsuario;
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
                    this.idUsuario = (int)Session["Login_IdUser"];
                    this.cargarCuentas();
                    this.cargarCuentasNivel1();
                    if (valor == 2)
                    {
                        this.cargarMovimientoEditar();
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando Movimientos de Caja. " + ex.Message));

            }
        }
        private void cargarCuentas()
        {
            try
            {
                List<Cuentas_Contables> cuentas = this.contPlanCta.obtenerCuentasContables();
                if (cuentas.Count > 0)
                {                    
                    this.PanelCtaContable.Visible = true;
                    this.lbtnAgregar.Visible = false;
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
                if(mov.tipo == 2)
                {
                    celTipo.Text = "Egreso";
                }

                List<Cuentas_Contables> cuentas = this.contPlanCta.obtenerCuentasContables();
                if (cuentas.Count > 0)
                {
                    TableCell celCuenta = new TableCell();
                    var cta = this.contPlanCta.obtenerCuentaContableTipoMovCaja(mov.id);
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Movimiento en la lista. " + ex.Message));
            }
        }
        private void cargarMovimientoEditar()
        {
            try
            {
                MovimientoCaja mov = this.controlador.obtenerMovimientoCajaID(this.idMovimiento);
                this.txtMov.Text = mov.descripcion;
                this.ListTipos.SelectedValue = mov.tipo.ToString();
                var cta = this.contPlanCta.obtenerCuentaContableTipoMovCaja(mov.id);
                if (cta != null)
                {
                    this.cargarCuentasNivel1();
                    this.ListCtaContables1.SelectedValue = cta.Cuentas_Contables.Nivel1.ToString();
                    this.cargarCuentasNivel2();
                    this.ListCtaContables2.SelectedValue = cta.Cuentas_Contables.Nivel2.ToString();
                    this.cargarCuentasNivel3();
                    this.ListCtaContables3.SelectedValue = cta.Cuentas_Contables.Nivel3.ToString();
                    this.cargarCuentasNivel4();
                    this.ListCtaContables.SelectedValue = cta.IdCuentaContable.ToString();
                }
            }
            catch
            {

            }
        }
        private void agregarMovimiento()
        {
            try
            {
                if (valor == 2)
                {
                    //MovimientoCaja mov = new MovimientoCaja();
                    //mov.id = this.idMovimiento;

                    MovimientoCaja mov = this.controlador.obtenerMovimientoCajaID(this.idMovimiento);
                    mov.descripcion = this.txtMov.Text;
                    mov.tipo = Convert.ToInt32(this.ListTipos.SelectedValue);
                    mov.estado = 1;
                    int i = this.controlador.modificarMovimientoCaja(mov);
                    if (i > 0)
                    {
                        //agrego bien  
                        this.modificarCtaContableTipoMov(mov);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico  Movimiento de Caja: " + mov.descripcion);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Movimiento de Caja modificado con exito", "MovimientoCajaF.aspx"));
                        this.borrarCampos();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando Movimiento de Caja"));

                    }
                }
                else
                {
                    MovimientoCaja mov = new MovimientoCaja();
                    mov.descripcion = this.txtMov.Text;
                    mov.tipo = Convert.ToInt32(this.ListTipos.SelectedValue);
                    mov.estado = 1;
                    int i = this.controlador.agregarMovimientoCaja(mov);
                    if (i > 0)
                    {
                        //agrego bien
                        this.agregarCtaContableTipoMov(mov);
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Movimiento de Caja: " + this.txtMovimiento.Text);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Movimiento de Caja cargado con exito", "MovimientoCajaF.aspx"));

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Movimiento de Caja"));

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Movimiento de Caja. " + ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.agregarMovimiento();
            }
            catch
            {

            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtMov.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos. " + ex.Message));
            }
        }

        private void editarGrupos(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("MovimientoCajaF.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar Movimiento de Caja. " + ex.Message));
            }
        }

        private void agregarCtaContableTipoMov(MovimientoCaja mov)
        {
            try
            {
                if (this.ListCtaContables.Items.Count > 0)
                {
                    int idCta = Convert.ToInt32(this.ListCtaContables.SelectedValue);
                    if (idCta > 0)
                    {
                        this.contPlanCta.agregarCuentaContableTipoMovCaja(mov.id, idCta);
                    }
                }
            }
            catch
            {

            }
        }
        private void modificarCtaContableTipoMov(MovimientoCaja mov)
        {
            try
            {
                int idCta = Convert.ToInt32(this.ListCtaContables.SelectedValue);
                if (idCta > 0)
                {
                    var cta = this.contPlanCta.obtenerCuentaContableTipoMovCaja(mov.id);
                    if (cta != null)
                    {
                        cta.IdCuentaContable = idCta;
                        this.contPlanCta.modificarCuentaContableTipoMovCaja(cta);
                    }
                    else
                    {
                        this.agregarCtaContableTipoMov(mov);
                    }

                }
            }
            catch(Exception ex)
            {

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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Movimiento de Caja eliminado con exito", "MovimientoCajaF.aspx"));

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando Movimiento de Caja"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar Movimiento de Caja. " + ex.Message));
            }
        }

        protected void lbtnAgregarMovCtaCbe_Click(object sender, EventArgs e)
        {
            try
            {
                this.agregarMovimiento();
            }
            catch
            {

            }
        }

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

        private void cargarCuentasNivel1()
        {
            try
            {
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(1, 0);

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
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(2, Convert.ToInt32(this.ListCtaContables1.SelectedValue));

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
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(3, Convert.ToInt32(this.ListCtaContables2.SelectedValue));

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
                var ctas = this.contPlanCta.obtenerCuentasContablesByNivel(4, Convert.ToInt32(this.ListCtaContables3.SelectedValue));

                this.ListCtaContables.DataSource = ctas.ToList();
                this.ListCtaContables.DataValueField = "Id";
                this.ListCtaContables.DataTextField = "Descripcion";
                this.ListCtaContables.DataBind();
                this.ListCtaContables.Items.Insert(0, new ListItem("Seleccione...", "-1"));

            }
            catch
            {

            }
        }    
    }
}