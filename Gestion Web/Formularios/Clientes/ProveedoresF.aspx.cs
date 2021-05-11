using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Proveedores
{
    public partial class ProveedoresF : System.Web.UI.Page
    {
        int esUruguay = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EsUruguay"));
        private controladorCliente controlador = new controladorCliente();
        private controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        public Dictionary<string, string> camposClientes = null;

        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (esUruguay == 1)
                {
                    thCuit.InnerText = "Root";
                }

                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["accion"]);
                this.cargarClientes();
                Page.Form.DefaultButton = this.lbBuscar.UniqueID;
            }
            catch
            {

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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Proveedores") != 1)
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
                        if (s == "9")
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


        /// <summary>
        /// Carga los clientes en la tabla
        /// </summary>
        private void cargarClientes()
        {
            try
            {
                List<Cliente> clientes = new List<Cliente>();
                clientes = this.controlador.obtenerClientesReduc(2);
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

                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = cl.codigo;
                    celCodigo.Width = Unit.Percentage(10);
                    celCodigo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celRazonSocial = new TableCell();
                    celRazonSocial.Text = cl.razonSocial;
                    celRazonSocial.Width = Unit.Percentage(25);
                    celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                    TableCell celAlias = new TableCell();
                    celAlias.Text = cl.alias;
                    celAlias.Width = Unit.Percentage(25);
                    celAlias.VerticalAlign = VerticalAlign.Middle;

                    TableCell celTipo = new TableCell();
                    celTipo.Text = cl.tipoCliente.descripcion;
                    celTipo.Width = Unit.Percentage(15);
                    celTipo.VerticalAlign = VerticalAlign.Middle;

                    TableCell celCuit = new TableCell();
                    celCuit.Text = cl.cuit;
                    celCuit.Width = Unit.Percentage(15);
                    celCuit.VerticalAlign = VerticalAlign.Middle;

                    TableCell celImage = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = cl.id.ToString();
                    btnDetails.CssClass = "btn btn-info ui-tooltip";
                    btnDetails.Attributes.Add("data-toggle", "tooltip");
                    btnDetails.Attributes.Add("title data-original-title", "Editar");
                    btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                    //btnDetails.Height = Unit.Pixel(10);
                    //btnEditar.Font.Size = 9;
                    btnDetails.PostBackUrl = "ClientesABM.aspx?accion=4&id=" + cl.id.ToString();
                    //btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);
                    celImage.Controls.Add(btnDetails);

                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celImage.Controls.Add(l2);


                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + cl.id;
                    btnEliminar.CssClass = "btn btn-info";
                    btnEliminar.Attributes.Add("data-toggle", "modal");
                    btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    //btnEliminar.Font.Size = 9;
                    //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                    //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                    btnEliminar.OnClientClick = "abrirdialog(" + cl.id + ");";
                    //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                    celImage.Controls.Add(btnEliminar);
                    celImage.Width = Unit.Percentage(10);
                    
                    
                    //celImage.Width = Unit.Percentage(5);
                    celImage.VerticalAlign = VerticalAlign.Middle;

                    //cargo el primer cliente en detalle
                    //if (i == 0)
                    //{
                    //    this.cargarClienteDetalle(cl.id);
                    //    this.linkEditar.HRef = "ClientesABM.aspx?accion=4&id=" + cl.id;
                    //}

                    i++;

                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = cl.id + "_1";
                    //tr.ID = "tr_" + cl.cuit;
                    //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                    //tr.VerticalAlign = VerticalAlign.Middle;
                    //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));


                    //agrego celda a filas
                    //tr.Cells.Add(celCuit);
                    tr.Cells.Add(celCodigo);
                    tr.Cells.Add(celRazonSocial);
                    tr.Cells.Add(celAlias);
                    tr.Cells.Add(celTipo);
                    tr.Cells.Add(celCuit);
                    tr.Cells.Add(celImage);
                    //arego fila a tabla
                    this.phProveedores.Controls.Add(tr);
                }
                //agrego la tabla al placeholder
                
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarProveedoresTable(List<Cliente> clientes)
        {
            //vacio place holder
            phProveedores.Controls.Clear();

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

                TableCell celCodigo = new TableCell();
                celCodigo.Text = cl.codigo;
                celCodigo.Width = Unit.Percentage(10);
                celCodigo.VerticalAlign = VerticalAlign.Middle;

                TableCell celRazonSocial = new TableCell();
                celRazonSocial.Text = cl.razonSocial;
                celRazonSocial.Width = Unit.Percentage(25);
                celRazonSocial.VerticalAlign = VerticalAlign.Middle;

                TableCell celAlias = new TableCell();
                celAlias.Text = cl.alias;
                celAlias.Width = Unit.Percentage(25);
                celAlias.VerticalAlign = VerticalAlign.Middle;

                TableCell celTipo = new TableCell();
                celTipo.Text = cl.tipoCliente.descripcion;
                celTipo.Width = Unit.Percentage(15);
                celTipo.VerticalAlign = VerticalAlign.Middle;

                TableCell celCuit = new TableCell();
                celCuit.Text = cl.cuit;
                celCuit.Width = Unit.Percentage(15);
                celCuit.VerticalAlign = VerticalAlign.Middle;

                TableCell celImage = new TableCell();
                LinkButton btnDetails = new LinkButton();
                btnDetails.ID = cl.id.ToString();
                btnDetails.CssClass = "btn btn-info ui-tooltip";
                btnDetails.Attributes.Add("data-toggle", "tooltip");
                btnDetails.Attributes.Add("title data-original-title", "Editar");
                btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnDetails.Height = Unit.Pixel(10);
                //btnEditar.Font.Size = 9;
                btnDetails.PostBackUrl = "ClientesABM.aspx?accion=4&id=" + cl.id.ToString();
                //btnDetails.Click += new EventHandler(this.mostrarClienteDetalles);
                celImage.Controls.Add(btnDetails);

                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celImage.Controls.Add(l2);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + cl.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + cl.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celImage.Controls.Add(btnEliminar);
                celImage.Width = Unit.Percentage(10);


                //celImage.Width = Unit.Percentage(5);
                celImage.VerticalAlign = VerticalAlign.Middle;

                //cargo el primer cliente en detalle
                //if (i == 0)
                //{
                //    this.cargarClienteDetalle(cl.id);
                //    this.linkEditar.HRef = "ClientesABM.aspx?accion=4&id=" + cl.id;
                //}

                i++;

                //fila
                TableRow tr = new TableRow();
                tr.ID = cl.id + "_1";
                //tr.ID = "tr_" + cl.cuit;
                //tr.Attributes.Add("onclick", "javascript:document.getElementById('MainContent_" + tr.ID + "').style.background-color = 'orange'; return false;");
                //tr.VerticalAlign = VerticalAlign.Middle;
                //tr.Attributes.Add("onclick", string.Format("javascript:alert('hola)"));


                //agrego celda a filas
                //tr.Cells.Add(celCuit);
                tr.Cells.Add(celCodigo);
                tr.Cells.Add(celRazonSocial);
                tr.Cells.Add(celAlias);
                tr.Cells.Add(celTipo);
                tr.Cells.Add(celCuit);
                tr.Cells.Add(celImage);
                //arego fila a tabla
                this.phProveedores.Controls.Add(tr);

            }
            //agrego la tabla al placeholder

        }

        private void mostrarClienteDetalles(object sender, EventArgs e)
        {
            try
            {
                //this.cargarClienteDetalle(Convert.ToInt32((sender as LinkButton).ID));
                //agrego el link al boton editar
                //this.linkEditar.HRef = "ClientesABM.aspx?accion=4&id=" + (sender as LinkButton).ID;
                //Table tt = this.phClientes.Controls[0] as Table;
                TableRow tr = this.phProveedores.Controls[0].FindControl((sender as LinkButton).ID + "1") as TableRow;
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
                //this.cargarClienteDetalle((sender as Button).ID);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Carga los datos del cliente en el detalle de la pagina
        /// </summary>
        /// <param name="cuit">cuit del cliente que se quiere mostrar</param>
        private void cargarClienteDetalle(int id)
        {
            try
            {
                //limpio el placeholder
                //this.phProveedorDetalle.Controls.Clear();
                //obtengo cliente con el cuit
                this.camposClientes = this.controlador.obtenerCamposProveedorID(id);
                //cargo el encabezado
                this.cargarEncabezado(id);
                //cargo los campos
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                //Label1.Text = "";
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
                //this.phProveedorDetalle.Controls.Add(table);

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

        private void cargarEncabezado(int id)
        {
            try
            {
                //phProveedorEncabezado.Controls.Clear();
                Cliente cl = this.controlador.obtenerProveedorID(id);
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


                //phProveedorEncabezado.Controls.Add(table);

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
                if (!String.IsNullOrEmpty(this.txtBusqueda.Text))
                {
                    this.buscar(this.txtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        protected void lbtnImpresion_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionProveedores.aspx?ex=1&proveedores=" + this.txtBusqueda.Text);
            }
            catch
            {

            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idProveedor = Convert.ToInt32(this.txtMovimiento.Text);


                int i = this.controlador.eliminarProveedor(idProveedor);
                if (i == 1)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Cliente: " + idProveedor);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proveedor eliminado con exito", null));
                    Response.Redirect("ProveedoresF.aspx");
                }
                if (i == 2)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Intento de baja de cliente: " + idProveedor + " No fue posible. El cliente tiene saldo. ");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("El cliente Tiene saldo positivo en Cuenta Corriente no se puede eliminar. ", null));
                    this.cargarClientes();
                }
                if (i <= 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo borrar proveedor"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Cliente. " + ex.Message));
            }
        }
    }
}