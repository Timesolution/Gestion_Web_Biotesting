using Disipar.Models;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReportesArticulos : System.Web.UI.Page
    {
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorPedido contPedido = new ControladorPedido();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorVendedor contVendedor = new controladorVendedor();
        controladorCliente contCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        Mensajes m = new Mensajes();
        private int suc;
        private string fechaD;
        private string fechaH;
        private int idArticulo;
        private int idLista;
        private string listas;
        private int idProv;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["s"]);
                idArticulo = Convert.ToInt32(Request.QueryString["a"]);
                idProv = Convert.ToInt32(Request.QueryString["prov"]);
                listas = Request.QueryString["l"];

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0 )
                    {
                        suc = (int)Session["Login_SucUser"];                        
                        fechaD = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");                        
                    }
                    this.cargarSucursal();
                    this.cargarArticulos();
                    this.cargarListaPrecio();
                    this.cargarProveedores();

                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();                    
                    DropListArticulos.SelectedValue = idArticulo.ToString();
                    DropListProveedor.SelectedValue = idProv.ToString();

                }
                this.cargarLabel(fechaD,fechaH,suc,idArticulo,listas);
                this.cargarDatosRango(fechaD, fechaH, suc, idArticulo, listas,idProv);

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));

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
                        if (s == "52")
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

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego Seleccione
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);
                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);


                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";

                this.DropListSucursal.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        
        private void cargarArticulos()
        {
            try
            {

                DataTable dt = contArticulo.obtenerArticulos2();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListArticulos.DataSource = dt;
                this.DropListArticulos.DataValueField = "id";
                this.DropListArticulos.DataTextField = "descripcion";

                this.DropListArticulos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + ex.Message));
            }
        }
        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                this.DropListProveedor.DataSource = dt;
                this.DropListProveedor.DataValueField = "id";
                this.DropListProveedor.DataTextField = "alias";
                this.DropListProveedor.DataBind();

                this.DropListProveedor.Items.Insert(0, new ListItem("Todos", "0"));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }
        public void cargarListaPrecio()
        {
            try
            {
                DataTable dt = this.contCliente.obtenerListaPrecios();
                                
                foreach (DataRow lista in dt.Rows)
                {
                    if (lista["nombre"].ToString() != "Seleccione...")
                    {
                        ListItem item = new ListItem(lista["nombre"].ToString(), lista["id"].ToString());
                        this.chkListListas.Items.Add(item);
                        int i = this.chkListListas.Items.IndexOf(item);
                        this.chkListListas.Items[i].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Lista de precios. " + ex.Message));
            }
        }
        
        private void cargarDatosRango(string fechaD, string fechaH, int idSuc, int idArticulo, string listas,int idProv)
        {
            try
            {
                
                if (fechaD != null && fechaH != null && listas != null)
                {
                    listas = listas.Remove(listas.Length - 1);
                    this.cargarTablaTopArticulosCantidad(fechaD, fechaH, idSuc, idArticulo, listas, idProv);
                    this.cargarTablaTopArticulosImporte(fechaD, fechaH, idSuc, idArticulo, listas, idProv);
                }
                else
                {
                    this.cargarTablaTopArticulosCantidad(fechaD, fechaH, idSuc, idArticulo, listas, idProv);
                    this.cargarTablaTopArticulosImporte(fechaD, fechaH, idSuc, idArticulo, listas, idProv);          
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Datos. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idArticulo, string listas)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }

                if(!String.IsNullOrEmpty(listas))
                {
                    foreach (string lista in listas.Split(','))
                    {
                        if (lista != "")
                        {
                            label += this.chkListListas.Items.FindByValue(lista).Text + ",";
                        }
                    }
                    //label += DropListListas.Items.FindByValue(idLista.ToString()).Text + ",";
                }

                if (idArticulo > 0)
                {
                    label += this.contArticulo.obtenerArticuloByID(idArticulo).codigo;
                }

                this.lblParametros.Text = label;

                this.lblSucursal.Text = DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text;
                //this.lblLista.Text = DropListListas.Items.FindByValue(idLista.ToString()).Text;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string listas = "";
                foreach (ListItem lista in chkListListas.Items)
                {
                    if (lista.Selected == true)
                    {
                        listas += lista.Value + ",";
                    }
                }

                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    if (DropListSucursal.SelectedValue != "-1")
                    {
                        if (!String.IsNullOrEmpty(listas))
                        {
                            Response.Redirect("ReportesArticulos.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&s=" + DropListSucursal.SelectedValue + "&a=" + DropListArticulos.SelectedValue + "&l=" + listas + "&prov=" + this.DropListProveedor.SelectedValue);
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una lista"));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe seleccionar una sucursal"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error filtrando. " + ex.Message));

            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {

                String buscar = this.txtDescArticulo.Text.Replace(' ', '%');
                DataTable dt = contArticulo.obtenerArticulosByDescDT(buscar);                

                //cargo la lista
                this.DropListArticulos.DataSource = dt;
                this.DropListArticulos.DataValueField = "id";
                this.DropListArticulos.DataTextField = "descripcion";
                this.DropListArticulos.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        public void cargarTablaTopArticulosCantidad(string fechaD, string fechaH, int idSuc, int idArticulo, string listas,int idProv)
        {
            try
            {
                if (fechaD != null && fechaH != null && listas != null)
                {
                    DataTable dt = contFacturacion.obtenerTopCantidadArticulosListaSucursal(fechaD, fechaH, idSuc, idArticulo, listas, idProv);
                    foreach (DataRow dr in dt.Rows)
                    {
                        this.cargarTopArticulosCantidadTable(dr);
                    }
                }
                //else
                //{
                //    DataTable dt = contFacturacion.obtenerTopCantidadArticulosListaSucursal(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(DropListArticulos.SelectedValue), listas);
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        this.cargarTopArticulosCantidadTable(dr);
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Cantidad" + ex.Message));
            }
        }

        public void cargarTablaTopArticulosImporte(string fechaD, string fechaH, int idSuc, int idArticulo, string listas, int idProv)
        {
            try
            {
                if (fechaD != null && fechaH != null && listas != null)
                {
                    DataTable dt = contFacturacion.obtenerTopImporteArticulosListaSucursal(fechaD, fechaH, idSuc, idArticulo, listas,idProv);
                    foreach (DataRow dr in dt.Rows)
                    {
                        this.cargarTopArticulosImporteTable(dr);
                    }
                }
                //else
                //{
                //    DataTable dt = contFacturacion.obtenerTopImporteArticulosListaSucursal(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(DropListArticulos.SelectedValue), listas);
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        this.cargarTopArticulosImporteTable(dr);
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por importe" + ex.Message));
            }
        }
        
        private void cargarTopArticulosCantidadTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["codigo"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(30);
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["descripcion"].ToString();
                celDescripcion.VerticalAlign = VerticalAlign.Bottom;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.Width = Unit.Percentage(40);
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                //celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.Text = Decimal.Round(Convert.ToDecimal(dr["Cantidad"]), 2).ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopArticulosCantidad.Controls.Add(tr);


            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Cantidad. " + ex.Message));

            }
        }
        private void cargarTopArticulosImporteTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["codigo"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(30);
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = dr["descripcion"].ToString();
                celDescripcion.VerticalAlign = VerticalAlign.Bottom;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.Width = Unit.Percentage(40);
                tr.Cells.Add(celDescripcion);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$ " + Decimal.Round(Convert.ToDecimal(dr["Importe"]), 2).ToString();
                celImporte.VerticalAlign = VerticalAlign.Bottom;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                celImporte.Width = Unit.Percentage(30);
                tr.Cells.Add(celImporte);

                phTopArticulosImporte.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Importe. " + ex.Message));

            }
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.listas))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=7&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&a=" + this.idArticulo + "&l=" + this.listas + "&prov=" + this.DropListProveedor.SelectedValue + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"));                   
                }
            }
            catch
            {

            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            this.ExportarToExcel(1);
        }

        private void ExportarToExcel(int valor)
        {
            try
            {
                
                if (valor == 1)
                {
                    if (!String.IsNullOrEmpty(this.listas))
                    {
                        Response.Redirect("ImpresionReporte.aspx?valor=7&ex=1&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&a=" + this.idArticulo + "&l=" + this.listas + "&prov=" + this.DropListProveedor.SelectedValue);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnExportarFecha_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.listas))
            {
                Response.Redirect("ImpresionReporte.aspx?valor=12&ex=1&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&s=" + this.DropListSucursal.SelectedValue + "&a=" + this.idArticulo + "&l=" + this.listas + "&prov=" + this.DropListProveedor.SelectedValue);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar al menos una lista"));
            }
        }
    }
}