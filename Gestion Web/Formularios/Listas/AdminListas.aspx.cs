using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
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
    public partial class AdminListas1 : System.Web.UI.Page
    {
        controladorListaPrecio controlador = new controladorListaPrecio();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();

        public int valor;
        public int idSublista;
        public int idLista;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["valor"]);
                
                string parametros = Request.QueryString["id"];
                this.cargarParametros(parametros);
                
                this.VerificarLogin();
                this.cargarListas();

                if (!IsPostBack)
                {
                    this.cargarListaPrecio();
                    this.cargarSubListas();

                    if (valor == 2)
                    {
                        //pongo disable lista
                        SubListaPrecio sl = this.controlador.obtenerSubListaPrecioID(this.idSublista);
                        this.cargarDatosSublista(sl);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        private void cargarParametros(string parametros)
        {
            if (parametros != null)
            {
                string[] ids = parametros.Split(';');

                this.idSublista = Convert.ToInt32(ids[0]);
                this.idLista = Convert.ToInt32(ids[1]);
            }
        }

        private void cargarDatosSublista(SubListaPrecio sl)
        {
            try
            {
                this.DropListLista.SelectedValue = this.idLista.ToString();
                this.DropListSubLista.SelectedValue = sl.categoria.id.ToString();
                //this.txtNombre.Text = sl.nombre;
                this.txtPorcentaje.Text = sl.porcentaje.ToString("N");
                this.ListAumentoDescuento.SelectedValue = sl.AumentoDescuento.ToString();
                this.ListCostoVenta.Text = sl.CostoVenta.ToString();
                this.DropListLista.Enabled = false;
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error Cargando datos de la sublista. " + ex.Message));
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
                        if (s == "25")
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

        public void cargarListaPrecio()
        {
            try
            {
                DataTable dt = this.controlador. obtenerListasPrecios();
                

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListLista.DataSource = dt;
                this.DropListLista.DataValueField = "id";
                this.DropListLista.DataTextField = "nombre";

                this.DropListLista.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando categorias. " + ex.Message));
            }
        }

        public void cargarSubListas()
        {
            try
            {
                DataTable dt = this.controlador.obtenerCategoriasSubListasPreciosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["categoria"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSubLista.DataSource = dt;
                this.DropListSubLista.DataValueField = "id";
                this.DropListSubLista.DataTextField = "categoria";

                this.DropListSubLista.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando categoria de sublista. " + ex.Message));
            }
        }

        private void cargarListas()
        {
            try
            {
                phAdminListas.Controls.Clear();
                List<listaPrecio> listas = this.controlador.obtenerlistaPrecioList();
                foreach (listaPrecio l in listas)
                {
                    foreach (SubListaPrecio sl in l.sublistas)
                    {
                        this.cargarSubListasTable(l, sl);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista de precio " + ex.Message));
            }
        }

        private void cargarSubListasTable(listaPrecio lp, SubListaPrecio sl)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celNombre = new TableCell();
                celNombre.Text = lp.nombre;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(25);
                tr.Cells.Add(celNombre);

                TableCell celSubLista = new TableCell();
                celSubLista.Text = sl.categoria.categoria;
                celSubLista.VerticalAlign = VerticalAlign.Middle;
                celSubLista.Width = Unit.Percentage(25);
                tr.Cells.Add(celSubLista);

                TableCell celPorcentaje = new TableCell();
                celPorcentaje.Text = sl.porcentaje.ToString("N") + "%";
                celPorcentaje.VerticalAlign = VerticalAlign.Middle;
                celPorcentaje.HorizontalAlign = HorizontalAlign.Right;
                celPorcentaje.Width = Unit.Percentage(10);
                tr.Cells.Add(celPorcentaje);

                TableCell celAumento = new TableCell();
                if (sl.AumentoDescuento == 1)
                {
                    celAumento.Text = "Aumento";
                    celAumento.VerticalAlign = VerticalAlign.Middle;
                    celAumento.Width = Unit.Percentage(15);
                    tr.Cells.Add(celAumento);
                }
                if (sl.AumentoDescuento == 2)
                {
                    celAumento.Text = "Descuento";
                    celAumento.VerticalAlign = VerticalAlign.Middle;
                    celAumento.Width = Unit.Percentage(15);
                    tr.Cells.Add(celAumento);
                }

                TableCell celCosto = new TableCell();
                if (sl.CostoVenta == 1)
                {
                    celCosto.Text = "Costo";
                    celCosto.VerticalAlign = VerticalAlign.Middle;
                    celCosto.Width = Unit.Percentage(15);
                    tr.Cells.Add(celCosto);
                }
                if (sl.CostoVenta == 2)
                {
                    celCosto.Text = "Venta";
                    celCosto.VerticalAlign = VerticalAlign.Middle;
                    celCosto.Width = Unit.Percentage(15);
                    tr.Cells.Add(celCosto);
                }

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = sl.id.ToString() + ";" + lp.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarLista);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + sl.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + sl.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(celAction);

                phAdminListas.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando configuracion de lista de precio en la lista. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (valor == 2)
                {
                    this.modificarSubLista();
                }
                else
                {
                    //alta sublista
                    this.altaSublista();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        private void altaSublista()
        {
            try
            {

                SubListaPrecio sl = new SubListaPrecio();
                //sl.nombre = this.txtNombre.Text;
                sl.categoria.id = Convert.ToInt32(this.DropListSubLista.SelectedValue);
                sl.porcentaje = Convert.ToDecimal(txtPorcentaje.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                sl.AumentoDescuento = Convert.ToInt32(ListAumentoDescuento.SelectedValue);
                sl.CostoVenta = Convert.ToInt32(ListCostoVenta.SelectedValue);

              
                
                int idLista = Convert.ToInt32(DropListLista.SelectedValue);

                //verifico si ya existe la sublista
                if (!this.controlador.verificarExiste(idLista, sl.categoria.id))
                {
                    int i = this.controlador.agregarSubListaPrecio(sl, idLista);
                    if (i > 0)
                    {
                        //se agrego correctamente, recargo categorias
                        Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta Administrador de Lista : " + i);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio cargada con exito", null));
                        this.cargarListas();
                        this.limpiarCampos();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya existe una lista con la categoria elegida. "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando sublista. " + ex.Message));
            }
        }

        public void modificarSubLista()
        {
            try
            {
                SubListaPrecio sl = new SubListaPrecio();
                sl.id = this.idSublista;
                //sl.nombre = this.txtNombre.Text;
                sl.categoria.id = Convert.ToInt32(this.DropListSubLista.SelectedValue);
                sl.porcentaje = Convert.ToDecimal(txtPorcentaje.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                sl.AumentoDescuento = Convert.ToInt32(ListAumentoDescuento.SelectedValue);
                sl.CostoVenta = Convert.ToInt32(ListCostoVenta.SelectedValue);
                int i = this.controlador.modificarSubListaPrecio(sl, this.idLista);
                if (i > 0)
                {
                    //se agrego correctamente, recargo categorias
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico Administrador de Lista : " + i);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Sublista modificada", "AdminListas.aspx"));
                    this.cargarListas();
                    this.limpiarCampos();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando sublista. Reintente "));
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando sublista. " + ex.Message));
            }
            
        }

        private void limpiarCampos()
        {
            try
            {
                this.DropListLista.SelectedValue = "-1";
                this.DropListSubLista.SelectedValue = "-1";
                this.txtPorcentaje.Text = "";
                //this.txtNombre.Text = "";
                this.ListAumentoDescuento.SelectedValue = "-1";
                this.ListCostoVenta.SelectedValue = "-1";

            }
            catch
            { }
        }

        private void editarLista(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AdminListas.aspx?valor=2&id=" + (sender as LinkButton).ID);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar Configuracion de Listas de Precios. " + ex.Message));
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idSublista = Convert.ToInt32(this.txtMovimiento.Text);
                int i = this.controlador.eliminarSubListaPrecio(idSublista);
                if (i > 0)
                {
                    //modifico bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Administrador de Lista : " + i);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Sublista Lista de Precio eliminada con exito", null));
                    this.cargarListas();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Configuracion de lista de Precio"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Configuracion de lista de Precio. " + ex.Message));
            }
        }

        protected void lbtnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtPrecioCalcular.Text))
                {
                    decimal precio = decimal.Parse(txtPrecioCalcular.Text);
                    txtPorcentaje.Text = Convert.ToString((precio*100)-100);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe colocar un precio para calcular el porcentaje. "));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}