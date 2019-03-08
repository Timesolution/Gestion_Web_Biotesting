using Disipar.Models;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras.Entregas
{
    public partial class EntregasF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        private int proveedor;
        private string fechaD;
        private string fechaH;

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();

            if (fechaD == null && fechaH == null)
            {
                fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                
                txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            txtFechaDesde.Text = fechaD;
            txtFechaHasta.Text = fechaH;
            ListProveedor.SelectedValue = proveedor.ToString();
        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("../../../Account/Login.aspx");
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
                Response.Redirect("../../../Account/Login.aspx");
            }
        }

        private int verificarAcceso()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                //string[] listPermisos = permisos.Split(';');
                //foreach (string s in listPermisos)
                //{
                //    if (!String.IsNullOrEmpty(s))
                //    {

                //    }
                //}


                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListProveedor.DataSource = dt;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";

                this.ListProveedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscarCodigoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodProveedor.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerProveedorNombreDT(buscar);
                this.phEntregas.Controls.Clear();

                this.ListProveedor.DataSource = dtClientes;
                this.ListProveedor.DataValueField = "id";
                this.ListProveedor.DataTextField = "alias";
                this.ListProveedor.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }

        //public void CargarItemsFacturaEnPH()
        //{
        //    try
        //    {
        //        var ordenCompra = contComprasEnt.obtenerOrden(this.ordenCompra);

        //        foreach (var item in ordenCompra.OrdenesCompra_Items)
        //        {
        //            if (item.CantidadYaRecibida < item.Cantidad)
        //            {
        //                var diferenciasOrdenCompra = ordenCompra.RemitoCompraOrdenCompra_Diferencias.Where(x => x.OrdenCompra == item.IdOrden && x.Articulo.ToString() == item.Codigo).ToList();
        //                cargarEnPh(item, diferenciasOrdenCompra);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.EscribirSQL(1, "Error", "Error al cargar los items de la factura en el PH " + ex.Message);
        //    }
        //}

        //private void cargarEnPh(OrdenesCompra_Items ocItem, List<RemitoCompraOrdenCompra_Diferencias> diferencias = null)
        //{
        //    try
        //    {
        //        //fila
        //        TableRow tr = new TableRow();
        //        tr.ID = ocItem.Codigo.ToString();

        //        //Celdas
        //        TableCell celCodigo = new TableCell();
        //        celCodigo.Text = contArticulos.obtenerArticuloByID(Convert.ToInt32(ocItem.Codigo)).codigo;
        //        celCodigo.HorizontalAlign = HorizontalAlign.Left;
        //        celCodigo.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celCodigo);

        //        TableCell celDescripcion = new TableCell();
        //        celDescripcion.Text = ocItem.Descripcion;
        //        celDescripcion.HorizontalAlign = HorizontalAlign.Left;
        //        celDescripcion.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celDescripcion);

        //        TableCell celCantidad = new TableCell();
        //        decimal cantidad = Convert.ToDecimal(ocItem.Cantidad);
        //        celCantidad.Text = cantidad.ToString();
        //        celCantidad.HorizontalAlign = HorizontalAlign.Left;
        //        celCantidad.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celCantidad);

        //        TableCell celCantidadYaRecibida = new TableCell();
        //        decimal cantidadYaRecibidas = ObtenerCantidadesYaRecibidas(diferencias);
        //        celCantidadYaRecibida.Text = "0";

        //        if (diferencias != null && diferencias.Count > 0)
        //            celCantidadYaRecibida.Text = cantidadYaRecibidas.ToString();

        //        celCantidadYaRecibida.HorizontalAlign = HorizontalAlign.Left;
        //        celCantidadYaRecibida.VerticalAlign = VerticalAlign.Middle;
        //        tr.Cells.Add(celCantidadYaRecibida);

        //        TableCell celAccion = new TableCell();

        //        TextBox celCantidadRecibida = new TextBox();
        //        celCantidadRecibida.TextMode = TextBoxMode.Number;
        //        celCantidadRecibida.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
        //        celCantidadRecibida.Text = (cantidad - cantidadYaRecibidas).ToString();

        //        celAccion.Controls.Add(celCantidadRecibida);

        //        tr.Cells.Add(celAccion);

        //        phProductos.Controls.Add(tr);

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun cargarEnPh EntregasMercaderiaF. Ex: " + ex.Message));
        //    }
        //}
    }
}