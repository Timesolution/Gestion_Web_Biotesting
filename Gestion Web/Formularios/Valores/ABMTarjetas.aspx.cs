using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class ABMTarjetas : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorTarjeta controlador = new controladorTarjeta();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int valor;
        private int idTarjeta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                this.idTarjeta = Convert.ToInt32(Request.QueryString["id"]);

                this.VerificarLogin();
                this.cargarTarjetas();                
                if (!IsPostBack)
                {
                    this.cargarOperadores();
                    if (valor == 2)
                    {
                        Tarjeta t = this.controlador.obtenerTarjetaID(this.idTarjeta);
                        txtNombre.Text = t.nombre;
                        txtDiasAcreditados.Text = t.diasAcreditados.ToString();
                        txtCuotas.Text = t.cuotas.ToString();
                        txtRecargo.Text = t.recargo.ToString();
                        txtFechaAcreditacion.Text = t.fechaAcreditacion.ToString();
                        txtCierre.Text = t.diaCierre.ToString();                        
                        if (t.tipoAcreditacion == 1)
                        {
                            this.rbtnFecha.Checked = true;
                        }
                        this.checkearTipoAcreditacion();
                        
                        
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
        private void cargarOperadores()
        {
            try
            {                
                List<Gestion_Api.Entitys.Operadores_Tarjeta> operadores = this.controlador.obtenerOperadores();

                this.ListOperadores.DataSource = operadores;
                this.ListOperadores.DataValueField = "Id";
                this.ListOperadores.DataTextField = "Operador";
                this.ListOperadores.DataBind();

                this.ListOperadores.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando operadores. " + ex.Message));

            }
        }
        private void cargarTarjetas()
        {
            try
            {
                phTarjetas.Controls.Clear();
                List<Tarjeta> tarjetas = this.controlador.obtenerTarjetas();
                foreach (Tarjeta t in tarjetas)
                {
                    this.cargarTarjetaPH(t);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error cargando tarjetas. " + ex.Message));

            }
        }

        private void cargarTarjetaPH(Tarjeta t)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celOperador = new TableCell();
                Gestion_Api.Entitys.Operadores_Tarjeta op = this.controlador.obtenerOperadorById(t.operador);
                if (op != null)
                {
                    celOperador.Text = op.Operador;
                }
                celOperador.VerticalAlign = VerticalAlign.Middle;
                celOperador.Width = Unit.Percentage(20);
                tr.Cells.Add(celOperador);


                TableCell celNombre = new TableCell();
                celNombre.Text = t.nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(30);
                tr.Cells.Add(celNombre);

                TableCell celDiasAcreditados = new TableCell();
                celDiasAcreditados.Text = t.diasAcreditados.ToString();
                celDiasAcreditados.VerticalAlign = VerticalAlign.Middle;
                celDiasAcreditados.HorizontalAlign = HorizontalAlign.Right;
                celDiasAcreditados.Width = Unit.Percentage(10);
                if (t.tipoAcreditacion == 0)
                {
                    celDiasAcreditados.ForeColor = System.Drawing.Color.Green;
                    celDiasAcreditados.Font.Bold = true;
                }
                tr.Cells.Add(celDiasAcreditados);

                TableCell celFechaAcreditacion = new TableCell();
                celFechaAcreditacion.Text = t.fechaAcreditacion.ToString();
                celFechaAcreditacion.VerticalAlign = VerticalAlign.Middle;
                celFechaAcreditacion.HorizontalAlign = HorizontalAlign.Right;
                celFechaAcreditacion.Width = Unit.Percentage(10);
                if (t.tipoAcreditacion == 1)
                {
                    celFechaAcreditacion.ForeColor = System.Drawing.Color.Green;
                    celFechaAcreditacion.Font.Bold = true;
                }
                tr.Cells.Add(celFechaAcreditacion);

                TableCell celCuotas = new TableCell();
                celCuotas.Text = t.cuotas.ToString();
                celCuotas.VerticalAlign = VerticalAlign.Middle;
                celCuotas.HorizontalAlign = HorizontalAlign.Right;
                celCuotas.Width = Unit.Percentage(10);
                tr.Cells.Add(celCuotas);

                TableCell celRecargo = new TableCell();
                celRecargo.Text = t.recargo.ToString() + "%";
                celRecargo.VerticalAlign = VerticalAlign.Middle;
                celRecargo.HorizontalAlign = HorizontalAlign.Right;
                celRecargo.Width = Unit.Percentage(10);
                tr.Cells.Add(celRecargo);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = t.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarTarjeta);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + t.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + t.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phTarjetas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando tarjeta en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (valor == 2)
                {
                    Gestion_Api.Entitys.Tarjeta t = this.controlador.obtenerTarjeraEntityById(this.idTarjeta);
                    //t.id = this.idTarjeta;
                    t.nombre = txtNombre.Text;
                    string recargo = this.txtRecargo.Text.Replace(',', '.');
                    t.recargo = Convert.ToDecimal(recargo);
                    t.diasAcreditacion = Convert.ToInt32(txtDiasAcreditados.Text);
                    t.cuotas = Convert.ToInt32(txtCuotas.Text);
                    t.estado = 1;
                    t.Operador = Convert.ToInt32(this.ListOperadores.SelectedValue);

                    if (this.rbtnDias.Checked == true)
                    {
                        t.tipoAcreditacion = 0;
                        t.fechaAcreditacion = 0;
                        t.diaCierre = 0;
                    }
                    else
                    {
                        t.tipoAcreditacion = 1;
                        t.fechaAcreditacion = Convert.ToInt32(this.txtFechaAcreditacion.Text);
                        t.diaCierre = Convert.ToInt32(this.txtCierre.Text);
                    }

                    int i = this.controlador.modificarTarjetaEntity(t);
                    if (i >= 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Tarjeta : " + t.nombre);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tarjeta modificada con exito", null));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Tarjeta modificada con exito!. \", {type: \"info\"});location.href = 'ABMTarjetas.aspx';", true);                        
                        this.borrarCampos();
                        this.cargarTarjetas();

                    }
                    else
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error modificando tarjeta"));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error modificando tarjeta. \", {type: \"error\"});", true);                        

                    }

                }
                else
                {
                    Gestion_Api.Entitys.Tarjeta t = new Gestion_Api.Entitys.Tarjeta();
                    t.nombre = txtNombre.Text;
                    string recargo = this.txtRecargo.Text.Replace(',', '.');
                    t.recargo = Convert.ToDecimal(recargo);
                    t.diasAcreditacion = Convert.ToInt32(txtDiasAcreditados.Text);
                    t.cuotas = Convert.ToInt32(txtCuotas.Text);
                    t.Operador = Convert.ToInt32(this.ListOperadores.SelectedValue);
                    t.estado = 1;
                    if (this.rbtnDias.Checked == true)
                    {
                        t.tipoAcreditacion = 0;
                        t.fechaAcreditacion = 0;
                        t.diaCierre = 0;
                    }
                    else
                    {
                        t.tipoAcreditacion = 1;
                        t.fechaAcreditacion = Convert.ToInt32(this.txtFechaAcreditacion.Text);
                        t.diaCierre = Convert.ToInt32(this.txtCierre.Text);
                    }

                    int i = this.controlador.agregarTarjetaEntity(t);
                    if (i > 0)
                    {
                        //agrego bien
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Tarjeta : " + t.nombre);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tarjeta agregada con exito", null));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Tarjeta agregada con exito!. \", {type: \"info\"});", true);                        
                        this.borrarCampos();
                        this.cargarTarjetas();

                    }
                    else
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando tarjeta"));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, UpdatePanel2.GetType(), "alert", "$.msgbox(\"Error modificando tarjeta. \", {type: \"error\"});", true); 

                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando tarjeta. " + ex.Message));
            }

        }

        public void borrarCampos()
        {
            try
            {
                this.txtNombre.Text = "";
                this.txtDiasAcreditados.Text = "";
                this.txtCuotas.Text = "";
                this.txtRecargo.Text = "";
                this.txtFechaAcreditacion.Text = "";
                this.txtCierre.Text = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error borrando campos de item. " + ex.Message));
            }
        }

        private void editarTarjeta(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMTarjetas.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al editar tarjeta. " + ex.Message));
            }
        }
        
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idTarjeta = Convert.ToInt32(this.txtMovimiento.Text);
                Tarjeta tar = this.controlador.obtenerTarjetaID(idTarjeta);
                tar.estado = 0;
                int i = this.controlador.modificar(tar);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Tarjeta : " + tar.nombre);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Tarjeta eliminada con exito", null));
                    this.cargarTarjetas();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error eliminando tarjeta"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error al eliminar tarjeta. " + ex.Message));
            }
        }
        
        private void checkearTipoAcreditacion()
        {
            try
            {
                if (this.rbtnFecha.Checked == true)
                {
                    this.txtDiasAcreditados.Attributes.Add("disabled", "true");
                    this.txtFechaAcreditacion.Attributes.Remove("disabled");
                    this.txtCierre.Attributes.Remove("disabled");
                }
                else
                {
                    this.txtFechaAcreditacion.Attributes.Add("disabled", "true");
                    this.txtCierre.Attributes.Add("disabled", "true");
                    this.txtDiasAcreditados.Attributes.Remove("disabled");
                }
                this.cargarTarjetas();
            }
            catch
            {

            }
        }
        protected void rbtnFecha_CheckedChanged(object sender, EventArgs e)
        {
            this.checkearTipoAcreditacion();
        }

    }
}