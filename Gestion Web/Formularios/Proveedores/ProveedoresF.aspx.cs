using Disipar.Models;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Proveedores
{
    public partial class ProveedoresF : System.Web.UI.Page
    {
        private controladorCliente controlador = new controladorCliente();
        Mensajes m = new Mensajes();
        public Dictionary<string, string> camposClientes = null;

        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.accion = Convert.ToInt32(Request.QueryString["accion"]);
            if (String.IsNullOrEmpty(txtBusqueda.Text))
            {
                this.cargarClientes();
            }
            else
            {
                this.buscar(txtBusqueda.Text);
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
                clientes = this.controlador.obtenerClientesReduc(2);
                Table table = new Table();

                table.CssClass = "table table-striped table-bordered";
                table.Width = Unit.Percentage(100);


                //para cargar el cliente
                int i = 0;

                foreach (Cliente cl in clientes)
                {
                    //celda
                    //TableCell celCuit = new TableCell();
                    //celCuit.Text = cl.cuit;

                    TableCell celRazonSocial = new TableCell();
                    celRazonSocial.Text = cl.razonSocial;
                    celRazonSocial.Width = Unit.Percentage(40);
                    celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                    TableCell celAlias = new TableCell();
                    celAlias.Text = cl.alias;
                    celAlias.Width = Unit.Percentage(40);
                    celAlias.VerticalAlign = VerticalAlign.Middle;

                    TableCell celImage = new TableCell();
                    Button btnDetails = new Button();
                    if (this.accion == 2)
                    {
                        btnDetails.ID = cl.cuit;
                        btnDetails.CssClass = "btn btn-info";
                        btnDetails.Text = "Seleccionar";
                        //btnDetails.Height = Unit.Pixel(30);
                        btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMFacturas.aspx?accion=3&cliente=" + cl.cuit;

                    }
                    if (this.accion == 3)
                    {
                        btnDetails.ID = cl.cuit;
                        btnDetails.CssClass = "btn btn-info";
                        btnDetails.Text = "Seleccionar";
                        //btnDetails.Height = Unit.Pixel(30);
                        btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMCotizaciones.aspx?accion=3&cliente=" + cl.cuit;

                    }
                    if (this.accion == 4)
                    {
                        btnDetails.ID = cl.cuit;
                        btnDetails.CssClass = "btn btn-info";
                        btnDetails.Text = "Seleccionar";
                        //btnDetails.Height = Unit.Pixel(30);
                        btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMPedidos.aspx?accion=3&cliente=" + cl.cuit;

                    }
                    else
                    {
                        btnDetails.ID = cl.cuit;
                        btnDetails.CssClass = "btn btn-info";
                        btnDetails.Text = "Detalles";
                        btnDetails.Height = Unit.Pixel(30);
                        btnDetails.Font.Size = 9;
                        btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);
                    }
                    celImage.Controls.Add(btnDetails);
                    celImage.Width = Unit.Percentage(20);
                    celImage.VerticalAlign = VerticalAlign.Middle;

                    //cargo el primer cliente en detalle
                    if (i == 0)
                    {
                        this.cargarClienteDetalle(cl.cuit);
                        this.linkEditar.HRef = "/../../Formularios/Clientes/ClientesABM.aspx?accion=4&cuit=" + cl.cuit;
                    }

                    i++;

                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = cl.cuit + "1";
                    //tr.ID = "tr_" + cl.cuit;
                    //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                    //tr.VerticalAlign = VerticalAlign.Middle;
                    //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));


                    //agrego celda a filas
                    //tr.Cells.Add(celCuit);
                    tr.Cells.Add(celRazonSocial);
                    tr.Cells.Add(celAlias);
                    tr.Cells.Add(celImage);
                    //arego fila a tabla
                    table.Controls.Add(tr);

                }
                //agrego la tabla al placeholder
                this.phProveedores.Controls.Add(table);
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarProveedoresTable(List<Cliente> clientes)
        {
            //vacio place holder
            phProveedores.Controls.Clear();

            Table table = new Table();
            table.CssClass = "table table-striped table-bordered";
            table.Width = Unit.Percentage(100);


            //para cargar el cliente
            int i = 0;

            foreach (Cliente cl in clientes)
            {
                //celda
                //TableCell celCuit = new TableCell();
                //celCuit.Text = cl.cuit;

                TableCell celRazonSocial = new TableCell();
                celRazonSocial.Text = cl.razonSocial;
                celRazonSocial.Width = Unit.Percentage(40);
                celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                TableCell celAlias = new TableCell();
                celAlias.Text = cl.alias;
                celAlias.Width = Unit.Percentage(40);
                celAlias.VerticalAlign = VerticalAlign.Middle;

                TableCell celImage = new TableCell();
                Button btnDetails = new Button();
                if (this.accion == 2)
                {
                    btnDetails.ID = cl.cuit;
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "Seleccionar";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMFacturas.aspx?accion=3&cliente=" + cl.cuit;

                }
                if (this.accion == 3)
                {
                    btnDetails.ID = cl.cuit;
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "Seleccionar";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMCotizaciones.aspx?accion=3&cliente=" + cl.cuit;

                }
                if (this.accion == 4)
                {
                    btnDetails.ID = cl.cuit;
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "Seleccionar";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.PostBackUrl = "../../Formularios/Facturas/ABMPedidos.aspx?accion=3&cliente=" + cl.cuit;

                }
                else
                {
                    btnDetails.ID = cl.cuit;
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "Detalles";
                    btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);
                }
                celImage.Controls.Add(btnDetails);
                celImage.Width = Unit.Percentage(20);
                celImage.VerticalAlign = VerticalAlign.Middle;

                //cargo el primer cliente en detalle
                if (i == 0)
                {
                    this.cargarClienteDetalle(cl.cuit);
                    this.linkEditar.HRef = "ClientesABM.aspx?accion=4&cuit=" + cl.cuit;
                }

                i++;

                //fila
                TableRow tr = new TableRow();
                tr.ID = cl.cuit + "1";
                //tr.ID = "tr_" + cl.cuit;
                //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                //tr.VerticalAlign = VerticalAlign.Middle;
                //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));


                //agrego celda a filas
                //tr.Cells.Add(celCuit);
                tr.Cells.Add(celRazonSocial);
                tr.Cells.Add(celAlias);
                tr.Cells.Add(celImage);
                //arego fila a tabla
                table.Controls.Add(tr);

            }
            //agrego la tabla al placeholder
            this.phProveedores.Controls.Add(table);

        }

        private void mostrarClienteDetalles(object sender, EventArgs e)
        {
            try
            {
                this.cargarClienteDetalle((sender as Button).ID);
                //agrego el link al boton editar
                this.linkEditar.HRef = "/../../Formularios/Clientes/ClientesABM.aspx?accion=4&cuit=" + (sender as Button).ID;
                //Table tt = this.phClientes.Controls[0] as Table;
                TableRow tr = this.phProveedores.Controls[0].FindControl((sender as Button).ID + "1") as TableRow;
                string hex = "#cc7a00";

                tr.CssClass = "active";
            }
            catch (Exception ex)
            {

            }
        }

        private void test(object sender, EventArgs e)
        {
            try
            {
                this.cargarClienteDetalle((sender as Button).ID);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Carga los datos del cliente en el detalle de la pagina
        /// </summary>
        /// <param name="cuit">cuit del cliente que se quiere mostrar</param>
        private void cargarClienteDetalle(string cuit)
        {
            try
            {
                //limpio el placeholder
                this.phProveedorDetalle.Controls.Clear();
                //obtengo cliente con el cuit
                this.camposClientes = this.controlador.obtenerCamposProveedorCuit(cuit);
                //cargo el encabezado
                this.cargarEncabezado(cuit);
                //cargo los campos
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                Label1.Text = "";
                foreach (KeyValuePair<string, string> kvp in camposClientes)
                {
                    //por cada valor una nueva fila y dos celdas
                    TableRow tr = new TableRow();

                    TableCell tcKey = new TableCell();

                    //string ss = @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                    tcKey.Text = kvp.Key;
                    tcKey.HorizontalAlign = HorizontalAlign.Left;
                    tcKey.ForeColor = Color.Black;
                    tcKey.Width = Unit.Percentage(30);

                    TableCell tcValue = new TableCell();
                    tcValue.Text = kvp.Value;
                    tcValue.Width = Unit.Percentage(70);

                    tr.Cells.Add(tcKey);
                    tr.Cells.Add(tcValue);

                    //string ss = @"<li><strong >" + kvp.Key + ": " + kvp.Value + "</strong></li>";
                    table.Controls.Add(tr);

                    //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";


                    //phClienteDetalle.Page.Response.Write(@"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>");

                }
                this.phProveedorDetalle.Controls.Add(table);

                //this.LaCodCliente.Text = cl.codigo;
                //this.LaRazonSocial.Text = cl.razonSocial;
                //this.LaAlias.Text = cl.alias;
                //this.LaGrupo.Text = cl.grupo.descripcion;
                ////this.txtPais.Text = cl.pais.descripcion;
                //this.LaCuit.Text = cl.cuit;
                //this.LaIva.Text = cl.iva;


                return;
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarEncabezado(string cuit)
        {
            try
            {
                phProveedorEncabezado.Controls.Clear();
                Cliente cl = this.controlador.obtenerProveedorCuit(cuit);
                //cargo los campos
                Table table = new Table();
                table.CssClass = "btn btn-primary";
                table.Width = Unit.Percentage(100);
                //Label1.Text = "";

                //lo agrego a la etiqueta en el widget
                //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

                //fila
                TableRow tr = new TableRow();

                //Celdas

                TableCell celCodigo = new TableCell();
                celCodigo.Text = cl.alias + "-";
                celCodigo.Attributes.Add("style", "font-size:medium");
                celCodigo.Width = Unit.Percentage(30);
                //celCodigo.ForeColor = Color.Black;
                //celTitulo.BorderStyle = BorderStyle.Solid;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);


                TableCell celDescrip = new TableCell();
                celDescrip.Text = cl.razonSocial;
                celDescrip.Attributes.Add("style", "font-size:medium");
                celDescrip.Width = Unit.Percentage(70);
                celDescrip.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescrip);

                //arego fila a tabla
                table.Controls.Add(tr);


                phProveedorEncabezado.Controls.Add(table);

                return;
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando encabezado desde la interfaz. " + ex.Message));
            }
        }

        //protected void LinkButton1_Click(object sender, EventArgs e)
        //{
        //    LinkButton1.PostBackUrl = "ClientesABM.aspx?accion=2&cuit=" + this.txtCuit.Text;
        //}

        private void buscar(string alias)
        {
            try
            {
                List<Cliente> cliente = this.controlador.obtenerProveedoresAlias(alias);
                this.cargarProveedoresTable(cliente);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando proveedor. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.buscar(this.txtBusqueda.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }
    }
}