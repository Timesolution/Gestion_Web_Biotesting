using Disipar.Models;
using Gestion_Api.Auxiliares;
using Gestion_Api.Controladores;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class ReportesVentas : System.Web.UI.Page
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
        private int idGrupo;
        private int idSubGrupo;
        private int idArticulo;
        private int idCliente;
        private int idVendedor;
        private int idProveedor;
        private string listas;
        private int idTipo;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.VerificarLogin();
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                suc = Convert.ToInt32(Request.QueryString["Sucursal"]);
                idArticulo = Convert.ToInt32(Request.QueryString["Articulo"]);
                idSubGrupo = Convert.ToInt32(Request.QueryString["SubGrupo"]);
                idGrupo = Convert.ToInt32(Request.QueryString["Grupo"]);
                idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                idVendedor = Convert.ToInt32(Request.QueryString["Vendedor"]);
                idProveedor = Convert.ToInt32(Request.QueryString["Prov"]);
                idTipo = Convert.ToInt32(Request.QueryString["tipo"]);
                listas = Request.QueryString["l"];

                if (!IsPostBack)
                {
                    if (fechaD == null && fechaH == null && suc == 0)
                    {
                        suc = (int)Session["Login_SucUser"];
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        DropListSucursal.SelectedValue = suc.ToString();
                        this.idTipo = -1;
                    }
                    this.cargarSucursal();
                    this.cargarArticulos();
                    this.cargarGruposArticulos();
                    DropListGrupo.SelectedValue = idGrupo.ToString();
                    this.cargarSubGruposArticulos(Convert.ToInt32(DropListGrupo.SelectedValue));
                    DropListSubGrupo.SelectedValue = idSubGrupo.ToString();
                    this.cargarClientes();
                    this.cargarProveedores();
                    this.cargarVendedores();
                    this.cargarListaPrecio();
                    txtFechaDesde.Text = fechaD;
                    txtFechaHasta.Text = fechaH;
                    DropListSucursal.SelectedValue = suc.ToString();
                    DropListClientes.SelectedValue = idCliente.ToString();
                    DropListArticulos.SelectedValue = idArticulo.ToString();
                    DropListVendedores.SelectedValue = idVendedor.ToString();
                    DropListProveedores.SelectedValue = idProveedor.ToString();
                    ListTipo.SelectedValue = this.idTipo.ToString();

                    this.lblParametrosUrl.Text = this.fechaD + "&" + this.fechaH + "&" + this.suc + "&" + this.idArticulo + "&" + this.idSubGrupo + "&" + this.idGrupo + "&" + this.idCliente + "&" + this.idVendedor + "&" + this.idProveedor + "&" + this.idTipo + "&" + this.listas;
                }
                this.cargarDatosRango(fechaD, fechaH, suc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, idTipo);

            }
            catch (Exception ex)
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

        private void cargarGruposArticulos()
        {
            try
            {

                DataTable dt = contArticulo.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListGrupo.DataSource = dt;
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
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

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        public void cargarProveedores()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = contCliente.obtenerProveedoresReducDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["alias"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListProveedores.DataSource = dt;
                this.DropListProveedores.DataValueField = "id";
                this.DropListProveedores.DataTextField = "alias";

                this.DropListProveedores.DataBind();

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

        public void cargarVendedores()
        {
            try
            {
                DropListVendedores.Items.Clear();

                DataTable dt = contVendedor.obtenerVendedores();

                //agrego todos
                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Seleccione...";
                dr2["id"] = -1;
                dt.Rows.InsertAt(dr2, 0);

                DataRow dr3 = dt.NewRow();
                dr3["nombre"] = "Todos";
                dr3["id"] = 0;
                dt.Rows.InsertAt(dr3, 1);

                foreach (DataRow dr in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = dr["id"].ToString();
                    item.Text = dr["nombre"].ToString() + " " + dr["apellido"].ToString();
                    DropListVendedores.Items.Add(item);
                }



                //this.DropListVendedor.DataSource = dt;
                //this.DropListVendedor.DataValueField = "id";
                //this.DropListVendedor.DataTextField = "nombre" + "apellido";

                //this.DropListVendedor.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Vendedores. " + ex.Message));
            }
        }

        private void cargarSubGruposArticulos(int grupo)
        {
            try
            {
                DataTable dt = contArticulo.obtenerSubGruposArticulos(grupo);

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["descripcion"] = "Todos";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                this.DropListSubGrupo.DataSource = dt;
                this.DropListSubGrupo.DataValueField = "id";
                this.DropListSubGrupo.DataTextField = "descripcion";

                this.DropListSubGrupo.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Subgrupos de articulos a la lista. " + ex.Message));
            }
        }

        private void cargarDatosRango(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {

                if (fechaD != null && fechaH != null && listas != null)
                {
                    this.lbDevoluciones.Text = contFacturacion.obtenerTotalDevoluciones(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor).ToString();
                    this.lblPedidosPendientes.Text = contPedido.obtenerTotalPedidosPendientes(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor).ToString();
                    //para que me incluya hasta el final de dia
                    fechaH += " 23:59:59.000";
                    listas = listas.Remove(listas.Length - 1);

                    this.lblProductosVendidos.Text = contFacturacion.obtenerTotalProductosVendidos(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo).ToString();
                    this.lblVentasRealizadas.Text = contFacturacion.obtenerTotalVentasRealizadas(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo).ToString();
                    this.cargarTablaTopArticulosCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    this.cargarTablaTopClientesCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    this.cargarTablaTopVendedoresCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    this.cargarTablaTopArticulosImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    this.cargarTablaTopClientesImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    this.cargarTablaTopVendedoresImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                    //this.cargarTablaTopSubGrupos(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);
                    //this.cargarTablaVentasUltimosMeses();
                    this.cargarLabel(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor);
                }
                else
                {
                    listas = "";

                    this.lbDevoluciones.Text = contFacturacion.obtenerTotalDevoluciones(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue)).ToString();
                    this.lblPedidosPendientes.Text = contPedido.obtenerTotalPedidosPendientes(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue)).ToString();
                    this.lblProductosVendidos.Text = contFacturacion.obtenerTotalProductosVendidos(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), listas, tipo).ToString();
                    this.lblVentasRealizadas.Text = contFacturacion.obtenerTotalVentasRealizadas(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), listas, tipo).ToString();
                    this.cargarTablaTopArticulosCantidad(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    this.cargarTablaTopClientesCantidad(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    this.cargarTablaTopVendedoresCantidad(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    this.cargarTablaTopArticulosImporte(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    this.cargarTablaTopClientesImporte(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    this.cargarTablaTopVendedoresImporte(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue), listas, tipo);
                    //this.cargarTablaTopSubGrupos(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue));
                    //this.cargarTablaVentasUltimosMeses();
                    this.cargarLabel(this.txtFechaDesde.Text, this.txtFechaHasta.Text, Convert.ToInt32(this.DropListSucursal.SelectedValue), Convert.ToInt32(this.DropListGrupo.SelectedValue), Convert.ToInt32(this.DropListSubGrupo.SelectedValue), Convert.ToInt32(this.DropListArticulos.SelectedValue), Convert.ToInt32(this.DropListClientes.SelectedValue), Convert.ToInt32(this.DropListVendedores.SelectedValue), Convert.ToInt32(this.DropListProveedores.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo Datos. " + ex.Message));
            }
        }

        private void cargarLabel(string fechaD, string fechaH, int idSucursal, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor)
        {
            try
            {
                string label = "";
                label += fechaD + "," + fechaH + ",";
                if (idCliente > 0)
                {
                    label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                }
                if (idSucursal > 0)
                {
                    label += DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idGrupo > 0)
                {
                    label += DropListGrupo.Items.FindByValue(idGrupo.ToString()).Text + ",";
                }
                if (idSubGrupo > 0)
                {
                    label += DropListSubGrupo.Items.FindByValue(idSubGrupo.ToString()).Text + ",";
                }
                //if (idArticulo > 0)
                //{
                //    label += DropListArticulos.Items.FindByValue(idArticulo.ToString()).Text + ",";
                //}
                if (idCliente > 0)
                {
                    label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                }
                if (idVendedor > 0)
                {
                    label += DropListVendedores.Items.FindByValue(idVendedor.ToString()).Text + ",";
                }
                if (idProveedor > 0)
                {
                    label += DropListProveedores.Items.FindByValue(idProveedor.ToString()).Text;
                }

                this.lblParametros.Text = label;

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
                foreach (ListItem lista in this.chkListListas.Items)
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
                        //this.cargarFacturasRango(fechaD,fechaH,Convert.ToInt32(DropListSucursal.SelectedValue));
                        Response.Redirect("ReportesVentas.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&Sucursal=" + DropListSucursal.SelectedValue + "&Articulo=" + DropListArticulos.SelectedValue + "&SubGrupo=" + DropListSubGrupo.SelectedValue + "&Grupo=" + DropListGrupo.SelectedValue + "&Cliente=" + DropListClientes.SelectedValue + "&Vendedor=" + DropListVendedores.SelectedValue + "&Prov=" + DropListProveedores.SelectedValue + "&l=" + listas + "&tipo=" + this.ListTipo.SelectedValue);
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
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de cotizaciones. " + ex.Message));

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

        //Cantidad

        //TOP Articulos

        public void cargarTablaTopArticulosCantidad(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                DataTable dt = contFacturacion.obtenerTopArticulosCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopArticulosCantidadTable(dr);
                }
                lbArticulosXVenta.Text = obtenerPromedioCantidadArticulosVendidosXVenta().ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Cantidad" + ex.Message));
            }
        }

        public decimal obtenerPromedioCantidadArticulosVendidosXVenta()
        {
            try
            {
                if (Convert.ToDecimal(lblVentasRealizadas.Text) > 0)
                {
                    decimal cantidadArticulosVendidosXVenta = Math.Round(Convert.ToDecimal(lblProductosVendidos.Text) / Convert.ToDecimal(lblVentasRealizadas.Text), 2);
                    return cantidadArticulosVendidosXVenta;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
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
                //celDescripcion.Width = Unit.Percentage(40);
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopArticulosCantidad.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Cantidad. " + ex.Message));

            }
        }

        // Top Clientes

        public void cargarTablaTopClientesCantidad(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                DataTable dt = contFacturacion.obtenerTopClientesCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopClientesCantidadTable(dr);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes por Cantidad" + ex.Message));
            }
        }

        private void cargarTopClientesCantidadTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["alias"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(70);
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.Width = Unit.Percentage(30);
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celCantidad);

                phTopClientesCantidad.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes. " + ex.Message));

            }
        }


        // Top Vendedores

        public void cargarTablaTopVendedoresCantidad(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                DataTable dt = contFacturacion.obtenerTopVendedoresCantidad(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopVendedoresCantidadTable(dr);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Vendedores por Cantidad. " + ex.Message));
            }
        }

        private void cargarTopVendedoresCantidadTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["Vendedor"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(70);
                tr.Cells.Add(celCodigo);


                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.VerticalAlign = VerticalAlign.Bottom;
                celCantidad.HorizontalAlign = HorizontalAlign.Right;
                celCantidad.Width = Unit.Percentage(30);
                tr.Cells.Add(celCantidad);

                phTopVendedoresCantidad.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos" + ex.Message));
            }
        }

        //Importe

        //TOP Articulos

        public void cargarTablaTopArticulosImporte(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                decimal montoTotalDeArticulosVendidos = 0;

                DataTable dt = contFacturacion.obtenerTopArticulosImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopArticulosImporteTable(dr);
                    montoTotalDeArticulosVendidos += Convert.ToDecimal(dr[2]);
                }
                lbVentaPromedio.Text = obtenerVentaPromedio(montoTotalDeArticulosVendidos).ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos por Importe" + ex.Message));
            }
        }

        private decimal obtenerVentaPromedio(decimal montoTotal)
        {
            try
            {
                decimal ventasRealizadas = Convert.ToDecimal(lblVentasRealizadas.Text);
                if (ventasRealizadas > 0)
                {
                    return Math.Round((montoTotal / ventasRealizadas), 2);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
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
                //celDescripcion.Width = Unit.Percentage(40);
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

        // Top Clientes

        public void cargarTablaTopClientesImporte(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                DataTable dt = contFacturacion.obtenerTopClientesImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopClientesImporteTable(dr);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes por Importe" + ex.Message));
            }
        }

        private void cargarTopClientesImporteTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["alias"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(70);
                tr.Cells.Add(celCodigo);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + dr["Importe"].ToString();
                celImporte.VerticalAlign = VerticalAlign.Bottom;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                celImporte.Width = Unit.Percentage(30);
                tr.Cells.Add(celImporte);

                phTopClientesImporte.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Clientes. " + ex.Message));

            }
        }


        // Top Vendedores

        /// <summary>
        /// Carga el Tabla de Top Vendedores por Importe
        /// </summary>
        /// <param name="fechaDesde">Fecha desde</param>
        /// <param name="fechaHasta">Fecha Hasta</param>
        /// <param name="idSuc">ID de Sucursal</param>
        /// <param name="idGrupo">ID de Grupo de Articulo</param>
        /// <param name="idSubGrupo">ID de SubGrupo de Articulo</param>
        /// <param name="idArticulo">ID de Articulo</param>
        /// <param name="idCliente">ID de Cliente</param>
        /// <param name="idVendedor">ID de Vendedor</param>
        /// <returns></returns>
        public void cargarTablaTopVendedoresImporte(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor, int idProveedor, string listas, int tipo)
        {
            try
            {
                DataTable dt = contFacturacion.obtenerTopVendedoresImporte(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor, idProveedor, listas, tipo);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarTopVendedoresImporteTable(dr);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Vendedores por Importe. " + ex.Message));
            }
        }

        private void cargarTopVendedoresImporteTable(DataRow dr)
        {
            try
            {

                TableRow tr = new TableRow();

                TableCell celCodigo = new TableCell();
                celCodigo.Text = dr["Vendedor"].ToString();
                celCodigo.VerticalAlign = VerticalAlign.Bottom;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.Width = Unit.Percentage(50);
                tr.Cells.Add(celCodigo);

                TableCell celImporte = new TableCell();
                celImporte.Text = "$" + dr["Importe"].ToString();
                celImporte.VerticalAlign = VerticalAlign.Bottom;
                celImporte.HorizontalAlign = HorizontalAlign.Right;
                celImporte.Width = Unit.Percentage(20);
                tr.Cells.Add(celImporte);

                TableCell celComision = new TableCell();
                celComision.Text = dr["Comision"].ToString() + "% ";
                celComision.VerticalAlign = VerticalAlign.Bottom;
                celComision.HorizontalAlign = HorizontalAlign.Right;
                celComision.Width = Unit.Percentage(10);
                tr.Cells.Add(celComision);

                TableCell celTotal = new TableCell();
                celTotal.Text = "$" + Decimal.Round((Convert.ToDecimal(dr["Importe"].ToString()) * (Convert.ToDecimal(dr["Comision"].ToString()) / 100)), 2);
                celTotal.VerticalAlign = VerticalAlign.Bottom;
                celTotal.HorizontalAlign = HorizontalAlign.Right;
                celTotal.Width = Unit.Percentage(20);
                tr.Cells.Add(celTotal);



                phTopVendedorImporte.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla de Top Articulos" + ex.Message));

            }
        }

        /// <summary>
        /// Carga el grafico de Top Sub Grupos por Cantidad
        /// </summary>
        /// <param name="fechaDesde">Fecha desde</param>
        /// <param name="fechaHasta">Fecha Hasta</param>
        /// <param name="idSuc">ID de Sucursal</param>
        /// <param name="idGrupo">ID de Grupo de Articulo</param>
        /// <param name="idSubGrupo">ID de SubGrupo de Articulo</param>
        /// <param name="idArticulo">ID de Articulo</param>
        /// <param name="idCliente">ID de Cliente</param>
        /// <param name="idVendedor">ID de Vendedor</param>
        /// <returns></returns>
        //public void cargarTablaTopSubGrupos(string fechaD, string fechaH, int idSuc, int idGrupo, int idSubGrupo, int idArticulo, int idCliente, int idVendedor)
        //{
        //    try
        //    {
        //        DataTable dt = contFacturacion.obtenerTopSubGrupos(fechaD, fechaH, idSuc, idGrupo, idSubGrupo, idArticulo, idCliente, idVendedor);
        //        //Chart1.DataSource = dt;
        //        //Chart1.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grafico Top SubGrupos. " + ex.Message));
        //    }
        //}

        /// <summary>
        /// Carga el Grafico de Barra de Ventas de los Ultimos 12 Meses
        /// </summary>
        //public void cargarTablaVentasUltimosMeses()
        //{
        //    try
        //    {
        //        DataTable dt = contFacturacion.obtenerVentasUltimos12Meses();
        //        Chart1.DataSource = dt;
        //        Chart1.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grafico Top SubGrupos. " + ex.Message));
        //    }
        //}

        #region controles
        /// <summary>
        /// Agrega los sub-grupos del grupo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void DropListGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarSubGruposArticulos(Convert.ToInt32(DropListGrupo.SelectedValue));
            }
            catch
            {

            }
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=1&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        protected void lbtnImprimir2_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=2&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnExportar2_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=2&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        protected void lbtnImprimir3_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=3&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnExportar3_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=3&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        protected void lbtnImprimir4_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=4&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnExportar4_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=4&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        protected void lbtnExportar5_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=5&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        protected void lbtnExportar6_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=6&ex=1&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo);
        }

        #endregion

        #region grafico

        [WebMethod(BufferResponse = false)]
        public static string cargarDatosChartVentas(string parametros)
        {
            try
            {

                string[] param = parametros.Split('&');
                string listas = param[10].Remove(param[10].Length - 1);
                controladorFacturacion contFacturacion = new controladorFacturacion();
                //fechaD, fechaH, idSuc, idArticulo,idSubGrupo,idGrupo, idSubGrupo,  idCliente, idVendedor, idProveedor, listas, tipo
                DataTable dt = contFacturacion.obtenerTopCantidadArticulosAnualGrafico(param[0], param[1], Convert.ToInt32(param[2]), Convert.ToInt32(param[5]), Convert.ToInt32(param[4]), Convert.ToInt32(param[3]), Convert.ToInt32(param[6]), Convert.ToInt32(param[7]), Convert.ToInt32(param[8]), listas, Convert.ToInt32(param[9]));


                DataTable dt2 = contFacturacion.obtenerTopImporteArticulosAnualGrafico(param[0], param[1], Convert.ToInt32(param[2]), Convert.ToInt32(param[5]), Convert.ToInt32(param[4]), Convert.ToInt32(param[3]), Convert.ToInt32(param[6]), Convert.ToInt32(param[7]), Convert.ToInt32(param[8]), listas, Convert.ToInt32(param[9]));



                List<DatosGraficoVentas> datos = new List<DatosGraficoVentas>();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DatosGraficoVentas reg = new DatosGraficoVentas();
                        reg.cantidad = Convert.ToDecimal(row["Cantidad"]);
                        reg.mes = row["Mes"].ToString();
                        datos.Add(reg);
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string resultadoJSON = serializer.Serialize(datos);
                return resultadoJSON;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(BufferResponse = false)]
        public static string cargarDatosChartVentasImportes(string parametros)
        {
            try
            {

                string[] param = parametros.Split('&');
                string listas = param[10].Remove(param[10].Length - 1);
                controladorFacturacion contFacturacion = new controladorFacturacion();
                //fechaD, fechaH, idSuc, idArticulo,idSubGrupo,idGrupo, idSubGrupo,  idCliente, idVendedor, idProveedor, listas, tipo
                DataTable dt = contFacturacion.obtenerTopImporteArticulosAnualGrafico(param[0], param[1], Convert.ToInt32(param[2]), Convert.ToInt32(param[5]), Convert.ToInt32(param[4]), Convert.ToInt32(param[3]), Convert.ToInt32(param[6]), Convert.ToInt32(param[7]), Convert.ToInt32(param[8]), listas, Convert.ToInt32(param[9]));

                List<DatosGraficoVentasImporte> datos = new List<DatosGraficoVentasImporte>();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DatosGraficoVentasImporte reg = new DatosGraficoVentasImporte();
                        reg.importe = Convert.ToDecimal(row["Importe"]);
                        reg.mes = row["Mes"].ToString();
                        datos.Add(reg);
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string resultadoJSON = serializer.Serialize(datos);
                return resultadoJSON;
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region Reporte Articulos Por Grupo
        protected void lbtnReporteArticulosPorGrupo_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=10&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "&ex=1");
        }
        protected void lbtnReporteArticulosPorGrupoPDF_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=10&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }
        #endregion

        protected void lbtnReporteArticulosPorCategoriaAndProveedorXLS_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=14&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "&ex=1");
        }

        protected void lbtnReporteArticulosPorCategoriaAndProveedorPDF_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=14&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "&ex=1");
        }

        protected void lbtnReporteVentasPorPuntoDeVentaExcel_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('/Formularios/Reportes/ImpresionReporte.aspx?valor=15&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
        }

        protected void lbtnReporteVentasPorPuntoDeVentaPDF_Click(object sender, EventArgs e)
        {
            string fd = this.txtFechaDesde.Text.ToString();
            string fh = this.txtFechaHasta.Text.ToString();
            Response.Redirect("/Formularios/Reportes/ImpresionReporte.aspx?valor=15&fd=" + fd + "&fh=" + fh + "&s=" + suc + "&prov=" + idProveedor + "&a=" + idArticulo + "&sg=" + idSubGrupo + "&g=" + idGrupo + "&c=" + idCliente + "&v=" + idVendedor + "&l=" + this.listas + "&t=" + this.idTipo + "&ex=1");
        }
    }
}