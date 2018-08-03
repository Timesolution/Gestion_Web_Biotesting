using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Entitys;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class OrdenReparacionABM : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        string idPresupuesto;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                idPresupuesto = Request.QueryString["presupuesto"];

                if (!IsPostBack)
                {
                    //CargarArticulosDropDownList();
                    CargarDatosPRPenOrdenReparacion();
                }

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
                ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
                //Gestion_Api.Modelo.OrdenReparacion
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al agregar orden de reparacion " + ex.Message);
            }
        }

        public void CargarDatosPRPenOrdenReparacion()
        {
            try
            {
                //controladorSucursal contSucursal = new controladorSucursal();

                Factura f = contFacturacion.obtenerFacturaId(Convert.ToInt32(idPresupuesto));

                txtSucOrigen.Text = f.sucursal.nombre;
                txtNumeroPRP.Text = f.numero;
                txtFechaCompra.Text = f.fecha.ToString("dd/MM/yyyy");
                txtCliente.Text = f.cliente.razonSocial;

                if(f.cliente.contactos.Count > 0 && f.cliente.contactos[0].numero != null)
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
                //this.ListProductos.Items.Remove(this.ListProductos.Items.FindByText("No Informa"));
                ListItem item = new ListItem("Seleccione...", "-1");
                this.ListProductos.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de tipos de IVA. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error al cargar los articulos en drop down list " + ex.Message);
            }
        }
    }
}