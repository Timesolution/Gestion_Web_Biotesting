using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Gestion_Api.Entitys;
using System.Globalization;
using System.Web.Configuration;
using Gestion_Api.Modelo.Enums;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ConsolidarP : System.Web.UI.Page
    {

        ControladorPedido controladorPedido = new ControladorPedido();
        controladorArticulo controladorArticulo = new controladorArticulo();

        Mensajes m = new Mensajes();
        List<Pedido> listaPedidos;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                string pedidos = Request.QueryString["pedidos"];



                CargarPedidos(pedidos);





            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        public void SetearColumnasPedidos(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string codigo = dr["codigo"].ToString();
                    string descripcion = dr["descripcion"].ToString();
                    decimal cantidad = Convert.ToDecimal(dr["cantidad"]);
                    string pedidos = dr["idPedido"].ToString();
                    string ubicacion = dr["ubicacion"].ToString();


                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = id.ToString();

                    //Celdas
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = codigo;
                    celCodigo.VerticalAlign = VerticalAlign.Middle;
                    celCodigo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celCodigo);

                    //Celdas
                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = descripcion;
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celDescripcion);

                    

                    //Celdas
                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = cantidad.ToString();
                    celCantidad.VerticalAlign = VerticalAlign.Middle;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celCantidad);

                    //Celdas
                    TableCell celUbicacion = new TableCell();
                    celUbicacion.Text = ubicacion;
                    celUbicacion.VerticalAlign = VerticalAlign.Middle;
                    celUbicacion.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celUbicacion);
                    //arego fila a tabla

                    TableCell celAccion = new TableCell();
                    Literal l1 = new Literal();
                    l1.Text = "&nbsp";
                    celAccion.Controls.Add(l1);
                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.ID = tr.ID + "," + pedidos;
                    btnEliminar.Text = "<span class='shortcut-icon icon-arrow-up'></span>";
                    //btnEliminar.Attributes.Add("onclick", "editarCantidades");
                    btnEliminar.Click += new EventHandler(this.editarCantidades);
                    celAccion.Controls.Add(btnEliminar);
                    celAccion.Width = Unit.Percentage(21);
                    celAccion.VerticalAlign = VerticalAlign.Middle;


                    tr.Cells.Add(celAccion);


                    phPedidos.Controls.Add(tr);


                }
            }
            catch (Exception ex)
            {

                
            }
            
        }

        public void SetearColumnasArticulos(DataTable dt,string idArticulo)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string numero = dr["numero"].ToString();
                    decimal cantidad = Convert.ToDecimal(dr["cantidad"]);


                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = id.ToString() +","+ idArticulo;
                    //Celdas
                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = numero;
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celDescripcion);

                    //Celdas
                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = cantidad.ToString();
                    celCantidad.VerticalAlign = VerticalAlign.Middle;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    celCantidad.Width = Unit.Percentage(40);
                    tr.Cells.Add(celCantidad);

                    //arego fila a tabla

                    TableCell celCantidadEntregar = new TableCell();
                    TextBox txtCant = new TextBox();
                    txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    txtCant.ID = "Txt"+tr.ID;
                    txtCant.CssClass = "form-control";
                    txtCant.Style.Add("text-align", "Right");
                    //txtCant.TextMode = TextBoxMode.Number;
                    txtCant.Text = celCantidad.Text;
                    celCantidadEntregar.Controls.Add(txtCant);
                    celCantidadEntregar.Width = Unit.Percentage(20);
                    tr.Cells.Add(celCantidadEntregar);


                    phArticulos.Controls.Add(tr);


                }
            }
            catch (Exception ex)
            {

            }
            
        }
        private void editarCantidades(object sender, EventArgs e)
        {
            try
            {
                phArticulos.Controls.Clear();
                LinkButton lb = sender as LinkButton;
                string[] fd = lb.ID.Split(',');
                string fd2 = lb.ID.Remove(0, lb.ID.IndexOf(','));
                ViewState["idArticulo"] = fd[0];
                ViewState["Pedidos"] = fd2;
                var pedidosxArticulo = controladorPedido.ObtenerPedidosPorArticulo(fd2, Convert.ToInt32(fd[0]));
                var articulo = controladorArticulo.obtenerArticuloByID(Convert.ToInt32(fd[0]));
                SetearColumnasArticulos(pedidosxArticulo, fd[0]);
                lblCodigoArticulo.Text = articulo.codigo;
                
                lblDescripcionArticulo.Text = articulo.descripcion;


            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar tipo evento. Excepción: " + Ex.Message));
            }
        }

        public void CargarPedidos(string pedidos)
        {
            try
            {
                string pedidos2 = pedidos.Replace(";", ",");
                var listaArticulos = controladorPedido.obtenerConsolidado(pedidos2);
                SetearColumnasPedidos(listaArticulos);
                if (ViewState["idArticulo"] != null && ViewState["Pedidos"] != null)
                {
                    string listaPedidosxArticulo = ViewState["Pedidos"].ToString();
                    string idArticulo = ViewState["idArticulo"].ToString();
                    var pedidosxArticulo = controladorPedido.ObtenerPedidosPorArticulo(listaPedidosxArticulo, Convert.ToInt32(idArticulo));
                    phArticulos.Controls.Clear();
                    SetearColumnasArticulos(pedidosxArticulo,idArticulo);
                }
            }
            catch (Exception ex)
            {

                
            }
       
        }




        #region carga inicial
        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../Account/Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        #endregion

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ItemPedido itemPedido;
                int idPedido;
                int idArticulo;

                foreach (Control C in phArticulos.Controls)
                {
                    itemPedido = new ItemPedido();

                    TableRow tr = C as TableRow;
                    string[] Ids = tr.ID.Split(',');
                    idPedido = Convert.ToInt32(Ids[0]);
                    idArticulo = Convert.ToInt32(Ids[1]);
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    itemPedido.articulo.id = idArticulo;
                    itemPedido.cantidad = Convert.ToDecimal(txt.Text);
                    controladorPedido.modificarCantidades(itemPedido, idPedido);
                    controladorPedido.actualizarTotales(idPedido);
                    

                }
                phPedidos.Controls.Clear();
                CargarPedidos(Request.QueryString["pedidos"]);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "CantidadesEditadas()", true);
            }
            catch (Exception ex)
            {

            }
            

        }


    }
}

