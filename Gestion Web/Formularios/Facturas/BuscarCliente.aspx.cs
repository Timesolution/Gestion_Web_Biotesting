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
    public partial class BuscarCliente : System.Web.UI.Page
    {       
        private controladorCliente controlador = new controladorCliente();
        private Mensajes m = new Mensajes();
        private int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.txtBuscarCliente.Focus();

                if (String.IsNullOrEmpty(txtBuscarCliente.Text))
                {
                    this.cargarClientes();
                }
                else
                {
                    this.buscar(this.txtBuscarCliente.Text);
                }

                this.Form.DefaultButton = lbBuscarCliente.UniqueID;
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Carga los clientes en la tabla
        /// </summary>
        private void cargarClientes()
        {
            try
            {
                List<Cliente> clientes = new List<Cliente>();
                clientes = this.controlador.obtenerClientesReduc(1);
                //Table table = new Table();

                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                foreach (Cliente cl in clientes)
                {
                    //celda
                    //TableCell celCuit = new TableCell();
                    //celCuit.Text = cl.cuit;

                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = cl.codigo;
                    celCodigo.Width = Unit.Percentage(10);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celRazonSocial = new TableCell();
                    celRazonSocial.Text = cl.razonSocial;
                    celRazonSocial.Width = Unit.Percentage(45);
                    celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                    TableCell celAlias = new TableCell();
                    celAlias.Text = cl.alias;
                    celAlias.Width = Unit.Percentage(45);
                    celAlias.VerticalAlign = VerticalAlign.Middle;

                    TableCell celImage = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = cl.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-ok'></span>";
                    btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    //btnDetails.PostBackUrl = "ABMFacturas.aspx?accion=3&cliente=" + cl.id;
                    btnDetails.Click += new EventHandler(this.RedireccionarCliente);

                    celImage.Controls.Add(btnDetails);
                    celImage.Width = Unit.Percentage(10);
                    celImage.VerticalAlign = VerticalAlign.Middle;


                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = cl.id + "tr";
                    //tr.ID = "tr_" + cl.cuit;
                    //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                    //tr.VerticalAlign = VerticalAlign.Middle;
                    //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));
                    //agrego celda a filas
                    //tr.Cells.Add(celCuit);
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celRazonSocial);
                    tr.Cells.Add(celAlias);
                    tr.Cells.Add(celImage);
                    //arego fila a tabla
                    this.phClientes.Controls.Add(tr);

                }
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarClientesTable(List<Cliente> clientes)
        {
            //vacio place holder
            phClientes.Controls.Clear();

            //Table table = new Table();
            //table.CssClass = "table table-striped table-bordered";
            //table.Width = Unit.Percentage(100);

            //para cargar el cliente
            int i = 0;

            foreach (Cliente cl in clientes)
            {
                //celda
                //TableCell celCuit = new TableCell();
                //celCuit.Text = cl.cuit;

                //si el estado es activo lo muestro
                if(cl.estado.descripcion == "Activo")
                {
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = cl.codigo;
                    celCodigo.Width = Unit.Percentage(10);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celRazonSocial = new TableCell();
                    celRazonSocial.Text = cl.razonSocial;
                    celRazonSocial.Width = Unit.Percentage(40);
                    celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                    TableCell celAlias = new TableCell();
                    celAlias.Text = cl.alias;
                    celAlias.Width = Unit.Percentage(40);
                    celAlias.VerticalAlign = VerticalAlign.Middle;

                    TableCell celImage = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = cl.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-ok'></span>";
                    btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    btnDetails.Click += new EventHandler(this.RedireccionarCliente);
                    //btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);

                    celImage.Controls.Add(btnDetails);
                    celImage.Width = Unit.Percentage(10);
                    celImage.VerticalAlign = VerticalAlign.Middle;

                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = cl.id + "TR";
                    //tr.ID = "tr_" + cl.cuit;
                    //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                    //tr.VerticalAlign = VerticalAlign.Middle;
                    //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));


                    //agrego celda a filas
                    //tr.Cells.Add(celCuit);
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celRazonSocial);
                    tr.Cells.Add(celAlias);
                    tr.Cells.Add(celImage);
                    //arego fila a tabla
                    this.phClientes.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
            }
        }

        private void buscar(string alias)
        {
            try
            {
                List<Cliente> cliente = this.controlador.obtenerClientesAlias(alias);
                this.cargarClientesTable(cliente);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.buscar(this.txtBuscarCliente.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void RedireccionarCliente(object sender, EventArgs e)
        {
            try
            {
                if(accion == 1)
                {
                    Session.Add("FacturasABM_ClienteModal", Convert.ToInt32((sender as LinkButton).ID));
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMFacturas.aspx?accion=3&cliente=" + (sender as Button).ID);
                   
                }
                if (accion == 2)
                {
                    Session.Add("CotizacionesABM_ClienteModal", Convert.ToInt32((sender as LinkButton).ID));
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMCotizaciones.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
                if (accion == 3)
                {
                    Session.Add("RemitosABM_ClienteModal", Convert.ToInt32((sender as LinkButton).ID));
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMRemitos.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
                if (accion == 4)
                {
                    Session.Add("PedidosABM_ClienteModal", Convert.ToInt32((sender as LinkButton).ID));
                    Modal.Close(this, "OK");
                    //Response.Redirect("ABMPedidos.aspx?accion=3&cliente=" + (sender as Button).ID);
                }
            }
            catch
            {

            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                Modal.Close(this, "OK");
            }
            catch
            {

            }
        }

    }
}