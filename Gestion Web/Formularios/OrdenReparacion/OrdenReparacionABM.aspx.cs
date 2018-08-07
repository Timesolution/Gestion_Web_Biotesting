using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    using Gestion_Api.Entitys;
    public partial class OrdenReparacionABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        int idPresupuesto;
        int idOrdenReparacion;
        int accion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                idPresupuesto = Convert.ToInt32(Request.QueryString["presupuesto"]);
                idOrdenReparacion = Convert.ToInt32(Request.QueryString["idordenreparacion"]);
                accion = Convert.ToInt32(Request.QueryString["a"]);

                //if (!IsPostBack)
                //{
                //    //CargarArticulosDropDownList();
                    
                //}

                if (accion == 1)
                    CargarDatosFacturaEnOrdenReparacion();
                else if (accion == 2)
                    ModificarOrdenReparacion();

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
                        if (s == "57")
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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var or = new OrdenReparacion();

                SetearValoresEnOrdenReparacion(or);

                var temp = contOrdenReparacion.AgregarOrdenReparacion(or);

                if(temp > 0)
                {
                    Log.EscribirSQL(1, "Info", "Orden de reparacion agregada con exito");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación agregada con exito!.", "OrdenReparacionF.aspx"));
                }                    
                else if(temp == -1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Orden de Reparación."));
                }                   

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al agregar orden de reparacion " + ex.Message);
            }
        }

        public void SetearValoresEnOrdenReparacion(OrdenReparacion or)
        {
            try
            {
                or.Autoriza = txtAutoriza.Text;
                or.Celular = txtCelular.Text;
                or.Cliente = txtCliente.Text;
                //or.DatosTrazabilidad = txtDatosTrazabilidad.Text;
                or.DescripcionFalla = txtDescripcionFalla.Text;
                or.Estado = 1;
                or.Fecha = txtFecha.Text.ToString();
                or.FechaCompra = txtFechaCompra.Text;
                or.NumeroOrdenReparacion = Convert.ToInt32(txtNumeroOrden.Text);
                or.NumeroPRP = txtNumeroPRP.Text;
                or.NumeroSerie = txtNumeroSerie.Text;
                or.PlazoLimiteReparacion = Convert.ToInt32(DropListPlazoLimite.SelectedValue);
                or.Producto = ListProductos.SelectedValue;
                or.SucursalOrigen = txtSucOrigen.Text;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al setear valores en orden de reparacion " + ex.Message);
            }
        }

        public void CargarDatosFacturaEnOrdenReparacion()
        {
            try
            {
                btnAgregar.Visible = true;
                //controladorSucursal contSucursal = new controladorSucursal();

                Factura f = contFacturacion.obtenerFacturaId(idPresupuesto);

                //obtengo la ultima orden de reparacion y le sumo 1
                txtNumeroOrden.Text = (contOrdenReparacion.ObtenerUltimaNumeracionOrdenReparacion() + 1).ToString("D8");
                txtNumeroOrden.CssClass = "form-control";
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha.CssClass = "form-control";
                txtSucOrigen.Text = f.sucursal.nombre;
                txtSucOrigen.CssClass = "form-control";
                txtNumeroPRP.Text = f.numero;
                txtNumeroPRP.CssClass = "form-control";
                txtFechaCompra.Text = f.fecha.ToString("dd/MM/yyyy");
                txtFechaCompra.CssClass = "form-control";
                txtCliente.Text = f.cliente.razonSocial;
                txtCliente.CssClass = "form-control";

                if (f.cliente.contactos.Count > 0 && f.cliente.contactos[0].numero != null)
                    txtCelular.Text = f.cliente.contactos[0].numero;

                CargarArticulosDropDownList(f.items);
                //var itemsFactura = contFacturacion.obtenerItemsFact(f.id);                

                //foreach (var itemFactura in itemsFactura)
                //{
                //    var trazas = contArticulo.ObtenerTrazabilidadByFacturaArticulo(itemFactura.articulo.id, f.id);
                //}

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar los datos del prp en la orden de compra " + ex.Message);
            }
        }

        public void ModificarOrdenReparacion()
        {
            try
            {
                btnGuardar.Visible = true;

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(idOrdenReparacion);

                txtNumeroOrden.Text = or.NumeroOrdenReparacion.Value.ToString("D8");
                txtNumeroOrden.CssClass = "form-control";
                txtFecha.Text = or.Fecha;
                txtFecha.CssClass = "form-control";
                txtSucOrigen.Text = or.SucursalOrigen;
                txtSucOrigen.CssClass = "form-control";
                txtNumeroPRP.Text = or.NumeroPRP;
                txtNumeroPRP.CssClass = "form-control";
                txtFechaCompra.Text = or.FechaCompra;
                txtFechaCompra.CssClass = "form-control";
                txtCliente.Text = or.Cliente;
                txtCliente.CssClass = "form-control";
                txtCelular.Text = or.Celular;
                txtAutoriza.Text = or.Autoriza;
                //txtDatosTrazabilidad.Text = or.DatosTrazabilidad;
                txtDescripcionFalla.Text = or.DescripcionFalla;
                txtNumeroSerie.Text = or.NumeroSerie;
                DropListPlazoLimite.Text = or.PlazoLimiteReparacion.ToString();
                var art = contArticulo.obtenerArticuloByID(Convert.ToInt32(or.Producto));
                ListProductos.Items.Add(art.codigo + " - " + art.descripcion);
                ListProductos.Enabled = false;
                ListProductos.CssClass = "form-control";
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar los datos del prp en la orden de compra " + ex.Message);
            }
        }

        private void CargarArticulosDropDownList(List<ItemFactura> itemsFactura)
        {
            try
            {
                //creo un datatable de articulos
                DataTable dtArticulos = new DataTable();
                dtArticulos.Columns.Add("id", typeof(int));
                dtArticulos.Columns.Add("descripcion", typeof(string));

                //obtengo los articulos de la factura
                var articulosFactura = itemsFactura.Select(x => x.articulo).ToList();

                List<Articulo> articulos = new List<Articulo>();

                //uso los articulos que tenia la factura para buscar en la tabla articulos y asi la descripcion viene sin la trazabilidad
                foreach (var articulo in articulosFactura)
                {
                    articulos.Add(contArticulo.obtenerArticuloByID(articulo.id));
                }

                //cargo el datatable con los articulos que tiene la factura
                foreach (var articulo in articulos)
                {
                    dtArticulos.Rows.Add(articulo.id,articulo.codigo + " - " + articulo.descripcion);
                }

                this.ListProductos.DataSource = dtArticulos;
                this.ListProductos.DataValueField = "id";
                this.ListProductos.DataTextField = "descripcion";

                this.ListProductos.DataBind();
                ListItem item = new ListItem("Seleccione...", "-1");
                this.ListProductos.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error al cargar los articulos en drop down list " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(idOrdenReparacion);

                SetearValoresEnOrdenReparacion(or);

                var temp = contOrdenReparacion.ModificarOrdenReparacion();

                if (temp > 0)
                {
                    Log.EscribirSQL(1, "Info", "Orden de reparacion modificada con exito");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación modificada con exito!.", "OrdenReparacionF.aspx"));
                }
                else if (temp == -1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando Orden de Reparación."));
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al modificar orden de reparacion " + ex.Message);
            }
        }
    }
}