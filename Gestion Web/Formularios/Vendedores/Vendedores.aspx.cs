using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Vendedores
{
    public partial class Vendedores : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorVendedor controlador = new controladorVendedor();
        controladorEmpleado contEmpleado = new controladorEmpleado();
        controladorUsuario contUser = new controladorUsuario();
        public Dictionary<string, string> camposVendedores = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.cargarVendedores();
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
                        if (s == "13")
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

        private void cargarVendedores()
        {
            try
            {
                phVendedores.Controls.Clear();

                List<Vendedor> vendedores = this.controlador.obtenerVendedoresReduc();
                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                foreach (Vendedor ven in vendedores)
                {
                    //fila
                    TableRow tr = new TableRow();

                    //Celdas
                    TableCell celLegajo = new TableCell();
                    celLegajo.Text = ven.emp.legajo.ToString();
                    celLegajo.Width = Unit.Percentage(20);
                    celLegajo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celLegajo);

                    TableCell celNombre = new TableCell();
                    celNombre.Text = ven.emp.nombre;
                    celNombre.Width = Unit.Percentage(30);
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNombre);

                    TableCell celApellido = new TableCell();
                    celApellido.Text = ven.emp.apellido;
                    celApellido.Width = Unit.Percentage(30);
                    celApellido.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celApellido);

                    TableCell celComision = new TableCell();
                    celComision.Text = ven.comision.ToString();
                    celComision.Width = Unit.Percentage(10);
                    celComision.VerticalAlign = VerticalAlign.Middle;
                    celComision.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celComision);

                    TableCell celAction = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = ven.emp.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    btnDetails.Font.Size = 9;
                    //btnDetails.Click += new EventHandler(this.mostrarVendedorDetalles);
                    btnDetails.PostBackUrl = "VendedoresABM.aspx?accion=2&codigo=" + ven.emp.legajo.ToString();
                    celAction.Controls.Add(btnDetails);




                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAction.Controls.Add(l2);

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + ven.emp.legajo.ToString();
                    btnEliminar.CssClass = "btn btn-info ui-tooltip";
                    btnEliminar.Attributes.Add("data-toggle", "tooltip");
                    btnEliminar.Attributes.Add("title data-original-title", "Eliminar");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    //btnEliminar.Font.Size = 9;
                    btnEliminar.Click += new EventHandler(this.eliminarVendedor);
                    celAction.Controls.Add(btnEliminar);
                    celAction.Width = Unit.Percentage(10);
                    tr.Controls.Add(celAction);

                    i++;

                    //arego fila a tabla
                    this.phVendedores.Controls.Add(tr);
                }                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando vendedores. " + ex.Message));
            }
        }

        private void cargarVendedoresTabla(List<Vendedor> vendedores)
        {
            try
            {
                phVendedores.Controls.Clear();

                //Table table = new Table();
                //table.CssClass = "table table-striped table-bordered";
                //table.Width = Unit.Percentage(100);

                //para cargar el cliente
                int i = 0;

                foreach (Vendedor ven in vendedores)
                {
                    //fila
                    TableRow tr = new TableRow();

                    //Celdas
                    TableCell celLegajo = new TableCell();
                    celLegajo.Text = ven.emp.legajo.ToString();
                    celLegajo.Width = Unit.Percentage(20);
                    celLegajo.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celLegajo);

                    TableCell celNombre = new TableCell();
                    celNombre.Text = ven.emp.nombre;
                    celNombre.Width = Unit.Percentage(30);
                    celNombre.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celNombre);

                    TableCell celApellido = new TableCell();
                    celApellido.Text = ven.emp.apellido;
                    celApellido.Width = Unit.Percentage(30);
                    celApellido.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(celApellido);

                    TableCell celComision = new TableCell();
                    celComision.Text = ven.comision.ToString();
                    celComision.Width = Unit.Percentage(10);
                    celComision.VerticalAlign = VerticalAlign.Middle;
                    celComision.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celComision);

                    TableCell celAction = new TableCell();
                    LinkButton btnDetails = new LinkButton();
                    btnDetails.ID = ven.emp.id.ToString();
                    btnDetails.CssClass = "btn btn-info";
                    btnDetails.Text = "<span class='shortcut-icon icon-pencil'></span>";
                    //btnDetails.Height = Unit.Pixel(30);
                    //btnDetails.Font.Size = 9;
                    //btnDetails.Click += new EventHandler(this.mostrarVendedorDetalles);
                    btnDetails.PostBackUrl = "VendedoresABM.aspx?accion=2&codigo=" + ven.emp.legajo.ToString();
                    celAction.Controls.Add(btnDetails);


                    Literal l2 = new Literal();
                    l2.Text = "&nbsp";
                    celAction.Controls.Add(l2);

                    LinkButton btnEliminar = new LinkButton();
                    btnEliminar.ID = "btnEliminar_" + ven.id.ToString();
                    btnEliminar.CssClass = "btn btn-info ui-tooltip";
                    btnEliminar.Attributes.Add("data-toggle", "tooltip");
                    btnEliminar.Attributes.Add("title data-original-title", "Eliminar");
                    btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                    //btnEliminar.Font.Size = 9;
                    btnEliminar.Click += new EventHandler(this.eliminarVendedor);
                    celAction.Controls.Add(btnEliminar);
                    celAction.Width = Unit.Percentage(10);
                    tr.Controls.Add(celAction);

                    i++;

                    //arego fila a tabla
                    this.phVendedores.Controls.Add(tr);
                }  
                //agrego la tabla al placeholder
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando empleados. " + ex.Message));
            }
        }

        /// <summary>
        /// Evento al hacer click en detalle
        /// </summary>
        private void mostrarVendedorDetalles(object sender, EventArgs e)
        {
            try
            {
                //this.cargarVendedorDetalle(Convert.ToDecimal((sender as LinkButton).ID));
                //agrego el link al boton editar
                //this.linkEditar.HRef = "VendedoresABM.aspx?accion=2&codigo=" + (sender as LinkButton).ID;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de empleado desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando empleado detalle desde la interfaz. " + ex.Message);
            }
        }

        /// <summary>
        /// Carga los datos del empleado en el detalle de la pagina
        /// </summary>
        /// <param name="cuit">legajo del empleado que se quiere mostrar</param>
        //private void cargarVendedorDetalle(decimal legajo)
        //{
        //    try
        //    {
        //        //obtengo cliente con el cuit
        //        this.camposVendedores = this.controlador.obtenerCamposVendedor(legajo);

        //        //cargo los campos
        //        Table table = new Table();
        //        table.Width = Unit.Percentage(100);
        //        //Label1.Text = "";
        //        this.cargarEncabezadoDetalle(legajo);
        //        foreach (KeyValuePair<string, string> kvp in camposVendedores)
        //        {
        //            //lo agrego a la etiqueta en el widget
        //            //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

        //            //fila
        //            TableRow tr = new TableRow();

        //            //Celdas

        //            TableCell celTitulo = new TableCell();
        //            celTitulo.Text = kvp.Key;
        //            celTitulo.Width = Unit.Percentage(30);
        //            celTitulo.ForeColor = Color.Black;
        //            //celTitulo.BorderStyle = BorderStyle.Solid;
        //            celTitulo.VerticalAlign = VerticalAlign.Middle;
        //            tr.Cells.Add(celTitulo);


        //            TableCell celValor = new TableCell();
        //            celValor.Text = kvp.Value;
        //            celValor.Width = Unit.Percentage(70);
        //            celValor.VerticalAlign = VerticalAlign.Middle;
        //            tr.Cells.Add(celValor);

        //            //arego fila a tabla
        //            table.Controls.Add(tr);
        //        }
        //        phVendDetalle.Controls.Add(table);

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando vendedor detalle desde la interfaz. " + ex.Message));
        //    }
        //}

        ///// <summary>
        ///// Carga los datos de legajo, nombre y apellido
        ///// </summary>
        ///// <param name="cuit">Legajo del empleado</param>
        //private void cargarEncabezadoDetalle(decimal legajo)
        //{
        //    try
        //    {
        //        Vendedor vend = this.controlador.obtenerVendedorLegajo(legajo);
        //        //cargo los campos
        //        Table table = new Table();
        //        table.BackColor = Color.Orange;
        //        table.CssClass = "btn btn-primary";
        //        table.Width = Unit.Percentage(100);
        //        //Label1.Text = "";

        //        //lo agrego a la etiqueta en el widget
        //        //Label1.Text += @"<li  ><strong >" + kvp.Key + ": </strong>" + kvp.Value + "</li>";

        //        //fila
        //        TableRow tr = new TableRow();

        //        //Celdas
        //        TableCell celCodigo = new TableCell();
        //        celCodigo.Text = vend.emp.legajo.ToString();
        //        celCodigo.Width = Unit.Percentage(30);
        //        celCodigo.ForeColor = Color.Black;
        //        //celTitulo.BorderStyle = BorderStyle.Solid;
        //        celCodigo.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celCodigo);

        //        TableCell celDescrip = new TableCell();
        //        celDescrip.Text = vend.emp.nombre + " " + vend.emp.apellido;
        //        celDescrip.Width = Unit.Percentage(70);
        //        celDescrip.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celDescrip);

        //        //arego fila a tabla
        //        table.Controls.Add(tr);

        //        phVendEncabezado.Controls.Add(table);

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando encabezado desde la interfaz. " + ex.Message));
        //    }
        //}

        private void buscar(string nombre)
        {
            try
            {
                List<Vendedor> Empleados = this.controlador.obtenerVendedoresNombre(nombre);
                this.cargarVendedoresTabla(Empleados);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando vendedor. " + ex.Message));
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando vendedor. " + ex.Message));
            }
        }

        private void eliminarVendedor(object sender, EventArgs e)
        {
            try
            {
                string[] t = (sender as LinkButton).ID.Split(new Char[] { '_' });
                Vendedor ven = this.controlador.obtenerVendedorLegajo(Convert.ToDecimal(t[1]));
                ven.estado = 0;
                int i = this.controlador.modificarVendedor(ven);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Baja Vendedor : " + ven.emp.nombre + " " + ven.emp.apellido);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Vendedor eliminado con exito", "Vendedores.aspx"));
                    this.cargarVendedores();

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Vendedor"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Vendedor. " + ex.Message));
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionVendedores.aspx?a=1', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionVendedores.aspx?a=1&ex=1");
            }
            catch
            {

            }
        }


    }
}