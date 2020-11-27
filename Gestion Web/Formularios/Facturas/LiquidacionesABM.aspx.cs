using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Disipar.Models;
using Gestor_Solution.Controladores;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class LiquidacionesABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    CargarSucursales();
                    CargarProductos();
                }
            }
            catch (Exception ex)
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
                        if (s == "39")
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

        private void CargarSucursales()
        {
            try
            {
                controladorSucursal contSuc = new controladorSucursal();

                this.ListSucursales.Items.Clear();

                var dtSuc = contSuc.obtenerSucursalesList2();

                this.ListSucursales.DataSource = dtSuc;
                this.ListSucursales.DataValueField = "id";
                this.ListSucursales.DataTextField = "nombre";

                this.ListSucursales.DataBind();

                if (this.ListSucursales.Items.Count >= 0)
                {
                    this.ListSucursales.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarProductos()
        {
            try
            {
                controladorArticulosNew contArt = new controladorArticulosNew();

                ListProductos.Items.Clear();

                var listpr = contArt.ObtenerArticulosStoreLiq();

                ListProductos.DataSource = listpr;
                ListProductos.DataValueField = "id";
                ListProductos.DataTextField = "descripcion";

                ListProductos.DataBind();

                if (ListProductos.Items.Count >= 0)
                {
                    ListProductos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                }
            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        public static string GetArticuloForTable(int id, int cant)
        {
            try
            {
                controladorArticulosNew contArt = new controladorArticulosNew();
                articulo art = contArt.ObtenerArticuloById(id);

                ArticuloTemp artTemp = new ArticuloTemp();
                artTemp.codigo = art.codigo;
                artTemp.descripcion = art.descripcion;
                artTemp.id = art.id;
                artTemp.cantidad = cant;

                JavaScriptSerializer javaScript = new JavaScriptSerializer();
                javaScript.MaxJsonLength = 5000000;
                string resultadoJSON = javaScript.Serialize(artTemp);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                controladorArticulo contArt = new controladorArticulo();
                controladorSucursal contSuc = new controladorSucursal();
                controladorFacturacion contFact = new controladorFacturacion();
                controladorCliente contCliente = new controladorCliente();

                // PREPARO PRIMER PRP///
                Sucursal suc = contSuc.obtenerSucursalID(Convert.ToInt32(ListSucursales.SelectedValue));

                Articulo art = contArt.obtenerArticuloByID(12);
                ItemFactura item = new ItemFactura();
                item.articulo = art;
                item.articulo.descripcion = item.articulo.descripcion + " Numero " + txtNroLiqui.Text;
                item.cantidad = 1;
                item.precioUnitario = Convert.ToDecimal(txtImporte.Text);
                item.total = Convert.ToDecimal(txtImporte.Text);
                item.precioSinIva = Convert.ToDecimal(txtImporte.Text);
                item.datosExtras = null;

                DataTable dt = new DataTable();
                Factura f = new Factura();

                f.fecha = Convert.ToDateTime(txtFecha.Text, new CultureInfo("es-AR"));
                f.empresa.id = (int)Session["Login_EmpUser"];
                f.sucursal.id = (int)Session["Login_SucUser"];
                f.ptoV = contSuc.obtenerPtoVentaId((int)Session["Login_PtoUser"]);
                f.tipo = contFact.obtenerTipoDocId(17);
                f.comentario = txtNroLiqui.Text;
                //CHEQUEO QUE EXISTA UN CLIENTE POR DEFECTO
                if (suc.clienteDefecto < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La sucursal con la que inicio sesion no tiene un cliente por defecto cargado."));
                    hiddenProd.Value = "";
                    return;
                }
                else
                {
                    f.cliente = contCliente.obtenerClienteID(suc.clienteDefecto);
                }
                f.vendedor.id = f.cliente.vendedor.id;
                f.formaPAgo = contFact.obtenerFormaPagoFP("Cuenta Corriente");
                f.items.Add(item);
                f.listaP.id = 1;
                f.total = Convert.ToDecimal(txtImporte.Text);
                //TERMINO PRIMER PRP


                //PREPARO SEGUNDO PRP
                Factura f2 = new Factura();
                Sucursal suc2 = contSuc.obtenerSucursalID(Convert.ToInt32(ListSucursales.SelectedValue));

                f2.fecha = Convert.ToDateTime(txtFecha.Text, new CultureInfo("es-AR"));
                f2.sucursal.id = suc2.id;
                f2.empresa.id = suc2.empresa.id;
                f2.ptoV = contSuc.obtenerPtoVentaSucursal(suc2.id).First();
                f2.tipo = contFact.obtenerTipoDocId(17);
                f2.comentario = txtNroLiqui.Text;
                //CHEQUEO QUE EXISTA UN CLIENTE POR DEFECTO
                if (suc2.clienteDefecto < 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("La sucursal que selecciono no tiene un cliente por defecto cargado."));
                    hiddenProd.Value = "";
                    return;
                }
                else
                {
                    f2.cliente = contCliente.obtenerClienteID(suc2.clienteDefecto);
                }
                f2.vendedor.id = f2.cliente.vendedor.id;
                f2.formaPAgo = contFact.obtenerFormaPagoFP("Cuenta Corriente");

                string[] items2 = hiddenProd.Value.Split(';');
                foreach (var pr in items2)
                {
                    //item
                    ItemFactura item2 = new ItemFactura();
                    string[] producto = pr.Split(',');                    
                    Articulo art2 = contArt.obtenerArticuloByID(Convert.ToInt32(producto[0]));
                    item2.articulo = art2;
                    item2.cantidad = Convert.ToDecimal(producto[2]);
                    item2.descuento = 0;
                    item2.precioUnitario = 0;
                    item2.total = item2.precioUnitario * item2.cantidad;
                    item2.precioSinIva = 0;
                    item2.datosExtras = null;

                    f2.items.Add(item2);
                }
                f2.listaP.id = 1;
                f2.total = 0;

                //TERMINO SEGUNDO PRP///

                int i = contFact.ProcesarFactura(null,f, dt, (int)Session["Login_IdUser"], 0);


                int j = contFact.ProcesarFactura(null,f2, dt, (int)Session["Login_IdUser"], 0);

                if (i > 0 && j > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Liquidaciones Cargadas Correctamente. ", ""));
                    Response.Redirect("/Formularios/Facturas/FacturasF.aspx");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo cargar las Liquidaciones."));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando las Liquidaciones " + ex.Message));
            }
        }
    }

    public class ArticuloTemp
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
    }
}