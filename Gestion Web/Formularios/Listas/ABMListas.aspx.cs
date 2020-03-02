using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class AdminListas : System.Web.UI.Page
    {
        controladorCliente controlador = new controladorCliente();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorFacturacion contFact = new controladorFacturacion();

        //Mensajes m = new Mensajes();

        //public int valor;
        //public int idLista;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        this.valor = Convert.ToInt32(Request.QueryString["valor"]);
        //        this.idLista = Convert.ToInt32(Request.QueryString["id"]);

        //        this.VerificarLogin();


        //        if(!IsPostBack)
        //        {
        //            this.cargarListas();
        //            this.cargarSubListas();

        //            if(valor == 2)
        //            {
        //                listaPrecio lp = this.controlador.obtenerListaPID(this.idLista);
        //                this.txtNombre.Text = lp.nombre;
        //                this.txtPorcentaje.Text = lp.dtoPrecioVenta.ToString();
        //                this.ListAumentoDescuento.SelectedValue = lp.AumentoDescuento.ToString();
        //                this.ListCostoVenta.Text = lp.CostoVenta.ToString();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista de precio " + ex.Message));
        //    }
        //}


        //private void VerificarLogin()
        //{
        //    try
        //    {
        //        if (Session["User"] == null)
        //        {
        //            Response.Redirect("../../Account/Login.aspx");
        //        }
        //    }
        //    catch
        //    {
        //        Response.Redirect("../../Account/Login.aspx");
        //    }
        //}


        //private void cargarListas()
        //{
        //    try
        //    {
        //        phListas.Controls.Clear();
        //        List<listaPrecio> precios = this.controlador.obtenerLPreciosList();
        //        foreach (listaPrecio lp in precios)
        //        {
        //            this.cargarListaPreciosTable(lp);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando lista de precio " + ex.Message));
        //    }
        //}

        //private void cargarListaPreciosTable(listaPrecio lp)
        //{
        //    try
        //    {

        //        TableRow tr = new TableRow();

        //        TableCell celNombre = new TableCell();
        //        celNombre.Text = lp.nombre;
        //        celNombre.VerticalAlign = VerticalAlign.Middle;
        //        celNombre.Width = Unit.Percentage(25);
        //        tr.Cells.Add(celNombre);

        //        TableCell celPorcentaje = new TableCell();
        //        celPorcentaje.Text = lp.dtoPrecioVenta.ToString() + "%";
        //        celPorcentaje.VerticalAlign = VerticalAlign.Middle;
        //        celPorcentaje.HorizontalAlign = HorizontalAlign.Right;
        //        celPorcentaje.Width = Unit.Percentage(20);
        //        tr.Cells.Add(celPorcentaje);

        //        TableCell celAumento = new TableCell();
        //        if(lp.AumentoDescuento == 1)
        //        {
        //            celAumento.Text = "Aumento";
        //            celAumento.VerticalAlign = VerticalAlign.Middle;
        //            celAumento.Width = Unit.Percentage(25);
        //            tr.Cells.Add(celAumento);
        //        }
        //        if(lp.AumentoDescuento == 2)
        //        {
        //            celAumento.Text = "Descuento";
        //            celAumento.VerticalAlign = VerticalAlign.Middle;
        //            celAumento.Width = Unit.Percentage(25);
        //            tr.Cells.Add(celAumento);
        //        }

        //        TableCell celCosto = new TableCell();
        //        if (lp.CostoVenta == 1)
        //        {
        //            celCosto.Text = "Costo";
        //            celCosto.VerticalAlign = VerticalAlign.Middle;
        //            celCosto.Width = Unit.Percentage(20);
        //            tr.Cells.Add(celCosto);
        //        }
        //        if (lp.CostoVenta == 2)
        //        {
        //            celCosto.Text = "Venta";
        //            celCosto.VerticalAlign = VerticalAlign.Middle;
        //            celCosto.Width = Unit.Percentage(20);
        //            tr.Cells.Add(celCosto);
        //        }

        //        TableCell celAction = new TableCell();
        //        LinkButton btnEditar = new LinkButton();
        //        btnEditar.ID = lp.id.ToString();
        //        btnEditar.CssClass = "btn btn-info ui-tooltip";
        //        btnEditar.Attributes.Add("data-toggle", "tooltip");
        //        btnEditar.Attributes.Add("title data-original-title", "Editar");
        //        btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
        //        //btnEditar.Font.Size = 9;
        //        btnEditar.Click += new EventHandler(this.editarLista);
        //        celAction.Controls.Add(btnEditar);

        //        Literal l = new Literal();
        //        l.Text = "&nbsp";
        //        celAction.Controls.Add(l);


        //        LinkButton btnEliminar = new LinkButton();
        //        btnEliminar.ID = "btnEliminar_" + lp.id.ToString();
        //        btnEliminar.CssClass = "btn btn-info ui-tooltip";
        //        btnEliminar.Attributes.Add("data-toggle", "tooltip");
        //        btnEliminar.Attributes.Add("title data-original-title", "Eliminar");
        //        btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
        //        //btnEliminar.Font.Size = 9;
        //        btnEliminar.Click += new EventHandler(this.eliminarLista);
        //        celAction.Controls.Add(btnEliminar);
        //        celAction.Width = Unit.Percentage(10);
        //        celAction.VerticalAlign = VerticalAlign.Middle;
        //        celAction.HorizontalAlign = HorizontalAlign.Center;
        //        tr.Cells.Add(celAction);

        //        phListas.Controls.Add(tr);


        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de precio en la lista. " + ex.Message));
        //    }
        //}


        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if(valor==2)
        //        {
        //            listaPrecio lst = new listaPrecio();
        //            lst.id = this.idLista;
        //            lst.nombre = txtNombre.Text;
        //            lst.AumentoDescuento = Convert.ToInt32(ListAumentoDescuento.SelectedValue);
        //            lst.CostoVenta = Convert.ToInt32(ListCostoVenta.SelectedValue);
        //            if (!String.IsNullOrEmpty(txtPorcentaje.Text))
        //            {
        //                lst.dtoPrecioVenta = Convert.ToDecimal(txtPorcentaje.Text);
        //            }
        //            else
        //            {
        //                lst.dtoPrecioVenta = 0;
        //            }
        //            lst.estado = 1;
        //            int i = this.controlador.modificarlistaPrecio(lst);
        //            if (i > 0)
        //            {
        //                //se agrego correctamente, recargo categorias
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio modificada con exito", null));
        //                this.cargarListas();
        //                this.limpiarCampos();
        //                Response.Redirect("ABMListas.aspx");
        //            }
        //            else
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
        //            }
        //        }
        //        else
        //        {
        //            listaPrecio lst = new listaPrecio();
        //            lst.nombre = txtNombre.Text;
        //            lst.AumentoDescuento = Convert.ToInt32(ListAumentoDescuento.SelectedValue);
        //            lst.CostoVenta = Convert.ToInt32(ListCostoVenta.SelectedValue);
        //            if (!String.IsNullOrEmpty(txtPorcentaje.Text))
        //            {
        //                lst.dtoPrecioVenta = Convert.ToDecimal(txtPorcentaje.Text);
        //            }
        //            else
        //            {
        //                lst.dtoPrecioVenta = 0;
        //            }
        //            int i = this.controlador.agregarlistaPrecio(lst);
        //            if (i > 0)
        //            {
        //                //se agrego correctamente, recargo categorias
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio cargada con exito", null));
        //                this.cargarListas();
        //                this.limpiarCampos();
        //                Response.Redirect("ABMListas.aspx");
        //            }
        //            else
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. "));
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
        //    }
        //}

        //private void limpiarCampos()
        //{
        //    try
        //    {
        //            this.txtNombre.Text = "";
        //            this.txtPorcentaje.Text = "";
        //            ListAumentoDescuento.SelectedValue = "-1";
        //            ListCostoVenta.SelectedValue = "-1";

        //    }
        //    catch
        //    { }
        //}

        //private void editarLista(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("ABMListas.aspx?valor=2&id=" + (sender as LinkButton).ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar tarjeta. " + ex.Message));
        //    }
        //}

        //private void eliminarLista(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string[] t = (sender as LinkButton).ID.Split(new Char[] { '_' });
        //        listaPrecio lp = this.controlador.obtenerListaPID(Convert.ToInt32(t[1]));
        //        lp.estado = 0;
        //        int i = this.controlador.modificarlistaPrecio(lp);
        //        if (i > 0)
        //        {
        //            //agrego bien
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista de Precio eliminada con exito", null));
        //            this.cargarListas();

        //        }
        //        else
        //        {
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando lista de Precio"));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar lista de Precio. " + ex.Message));
        //    }
        //}
    }
}
