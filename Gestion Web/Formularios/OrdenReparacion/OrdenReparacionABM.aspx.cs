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
    using Gestor_Solution.Controladores;
    using System.Globalization;

    public partial class OrdenReparacionABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorArticulo contArticulo = new controladorArticulo();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        ControladorOrdenReparacionEntity contOrdenReparacion = new ControladorOrdenReparacionEntity();
        controladorServicioTecnicoEntity contServTecnico = new controladorServicioTecnicoEntity();
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

                btnAgregar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnAgregar, null) + ";");
                btnGuardar.Attributes.Add("onclick", " this.disabled = true; this.value='Aguarde…'; " + ClientScript.GetPostBackEventReference(btnGuardar, null) + ";");

                if (!IsPostBack)
                {
                    //CargarArticulosDropDownList();
                    if (accion == 1)
                        CargarDatosFacturaEnOrdenReparacion();
                    else if (accion == 2)
                        ModificarOrdenReparacion();
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
                        if (s == "162")
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

                or.Estado = 1;

                SetearValoresEnOrdenReparacion(or);                

                var temp = contOrdenReparacion.AgregarOrdenReparacion(or);

                contOrdenReparacion.AgregarObservacionOrdenReparacion(or.Id, (int)Session["Login_IdUser"], "Se genera orden de reparacion numero " + or.NumeroOrdenReparacion.Value.ToString("D8"));

                if (temp > 0)
                {
                    temp = contOrdenReparacion.AgregarStockSucursalReparacion((int)Session["Login_IdUser"], or);

                    if(temp < 1)
                        Log.EscribirSQL(1, "ERROR", "Error al agregar stock en la sucursal de reparacion");

                    if(or.CambiaProducto == "Si")
                    {
                        string comentario = "Elimino stock por cambio de producto fallido al cliente. OR: " + or.NumeroOrdenReparacion.Value.ToString("D8");
                        temp = contOrdenReparacion.EliminarStockSucursalOrigen((int)Session["Login_IdUser"], or, comentario);

                        if (temp < 1)
                            Log.EscribirSQL(1, "ERROR", "Error al eliminar stock en la sucursal de origen");
                    }

                    Log.EscribirSQL(1, "Info", "Orden de reparacion agregada con exito");
                    string script = "window.open('ImpresionOrdenReparacion.aspx?a=1&or=" + or.Id.ToString() + "&prp=" + or.NumeroPRP.ToString() + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');";
                    script += " $.msgbox(\"Orden de reparación agregada con exito! \", {type: \"info\"}); location.href = 'OrdenReparacionF.aspx'";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Orden de reparación agregada con exito!.", "OrdenReparacionF.aspx"));
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
                or.Celular = txtCodArea.Text + txtCelular.Text;
                or.Cliente = Convert.ToInt32(ListCliente.SelectedValue);
                //or.DatosTrazabilidad = txtDatosTrazabilidad.Text;
                or.DescripcionFalla = txtDescripcionFalla.Text;
                or.Fecha = Convert.ToDateTime(txtFecha.Text, new CultureInfo("es-AR"));
                or.FechaCompra = Convert.ToDateTime(txtFechaCompra.Text,new CultureInfo("es-AR"));
                or.NumeroOrdenReparacion = Convert.ToInt32(txtNumeroOrden.Text);
                or.NumeroPRP = Convert.ToInt32(ListNumeroPRPoFactura.SelectedValue);
                or.NumeroSerie = txtNumeroSerie.Text;
                or.PlazoLimiteReparacion = Convert.ToInt32(DropListPlazoLimite.SelectedValue);
                or.Producto = Convert.ToInt32(ListProductos.SelectedValue);
                or.SucursalOrigen = Convert.ToInt32(ListSucursal.SelectedValue);
                or.CambiaProducto = DropListCambiaProducto.Text;
                or.EstadoDelProducto = txtEstadoDelProducto.Text;
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

                ListSucursal.Items.Add(new ListItem
                {
                    Value = f.sucursal.id.ToString(),
                    Text = f.sucursal.nombre
                });
                ListSucursal.CssClass = "form-control";

                ListCliente.Items.Add(new ListItem
                {
                    Value = f.cliente.id.ToString(),
                    Text = f.cliente.razonSocial
                });
                ListCliente.CssClass = "form-control";

                ListNumeroPRPoFactura.Items.Add(new ListItem
                {
                    Value = f.id.ToString(),
                    Text = f.numero
                });
                ListNumeroPRPoFactura.CssClass = "form-control";

                txtFechaCompra.Text = f.fecha.ToString("dd/MM/yyyy");
                txtFechaCompra.CssClass = "form-control";

                #region celular
                if (f.cliente.contactos.Count > 0 && f.cliente.contactos[0].numero != null)
                {
                    string numeroCelular = f.cliente.contactos[0].numero.Trim();

                    if (numeroCelular.StartsWith("11"))
                    {
                        txtCodArea.Text = "11";
                        txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 8));
                    }
                    else
                    {
                        txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 6));
                        numeroCelular = numeroCelular.Replace(txtCelular.Text, string.Empty);
                        txtCodArea.Text = numeroCelular;
                    }
                }
                #endregion
                CargarArticulosDropDownList(f.items);

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar los datos del prp en la orden de reparacion " + ex.Message);
            }
        }

        public void ModificarOrdenReparacion()
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorCliente contCliente = new controladorCliente();

                btnGuardar.Visible = true;

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(idOrdenReparacion);

                txtNumeroOrden.Text = or.NumeroOrdenReparacion.Value.ToString("D8");
                txtNumeroOrden.CssClass = "form-control";
                txtFecha.Text = or.Fecha.Value.ToString("dd/MM/yyyy");
                txtFecha.CssClass = "form-control";

                var suc = contSucursal.obtenerSucursalID((int)or.SucursalOrigen);

                ListSucursal.Items.Add(new ListItem
                {
                    Value = suc.id.ToString(),
                    Text = suc.nombre
                });
                ListSucursal.CssClass = "form-control";

                var cliente = contCliente.obtenerClienteID((int)or.Cliente);

                ListCliente.Items.Add(new ListItem
                {
                    Value = cliente.id.ToString(),
                    Text = cliente.razonSocial
                });
                ListCliente.CssClass = "form-control";

                var f = contFacturacion.obtenerFacturaId((int)or.NumeroPRP);

                ListNumeroPRPoFactura.Items.Add(new ListItem
                {
                    Value = f.id.ToString(),
                    Text = f.numero
                });
                ListNumeroPRPoFactura.CssClass = "form-control";
                txtFechaCompra.Text = or.FechaCompra.Value.ToString("dd/MM/yyyy");
                txtFechaCompra.CssClass = "form-control";
                txtCelular.Text = or.Celular;

                #region celular
                string numeroCelular = or.Celular.Trim();

                if (numeroCelular.StartsWith("11"))
                {
                    txtCodArea.Text = "11";
                    txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 8));
                }
                else
                {
                    txtCelular.Text = numeroCelular.Substring(Math.Max(0, numeroCelular.Length - 6));
                    numeroCelular = numeroCelular.Replace(txtCelular.Text, string.Empty);
                    txtCodArea.Text = numeroCelular;
                }
                #endregion

                txtAutoriza.Text = or.Autoriza;
                txtDescripcionFalla.Text = or.DescripcionFalla;
                txtEstadoDelProducto.Text = or.EstadoDelProducto;
                txtNumeroSerie.Text = or.NumeroSerie;
                DropListPlazoLimite.Text = or.PlazoLimiteReparacion.ToString();
                DropListCambiaProducto.Text = or.CambiaProducto.ToString();

                var art = contArticulo.obtenerArticuloByID(Convert.ToInt32(or.Producto));

                ListProductos.Items.Add(new ListItem
                {
                    Value = art.id.ToString(),
                    Text = art.codigo + " - " + art.descripcion
                } );

                ListProductos.Enabled = false;
                ListProductos.CssClass = "form-control";
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al modificar de la orden de reparacion " + ex.Message);
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

                //uso los articulos que tenia la factura para buscar en la tabla articulos y asi la descripcion viene sin la trazabilidad
                List<Articulo> articulos = articulosFactura.Select(x => contArticulo.obtenerArticuloByID(x.id)).ToList();

                //cargo el datatable con los articulos que tiene la factura. uso el tolist al final porque al ser lazy no se ejecuta el select inmediatamente y entonces no le pasa valores al datatable
                articulos.Select(x => dtArticulos.Rows.Add(x.id, x.codigo + " - " + x.descripcion)).ToList();

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
                btnGuardar.Enabled = false;
                btnGuardar.CssClass = "form-control";

                var or = contOrdenReparacion.ObtenerOrdenReparacionPorID(idOrdenReparacion);

                SetearValoresEnOrdenReparacion(or);

                var temp = contOrdenReparacion.ModificarOrdenReparacion();

                if (temp >= 0)
                {
                    Log.EscribirSQL(1, "Info", "Orden de reparacion modificada con exito");
                    string script = " $.msgbox(\"Orden de reparación modificada con exito! \", {type: \"info\"}); location.href = 'OrdenReparacionF.aspx'";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                }
                else if (temp < 0)
                {
                    string script = " $.msgbox(\"Error modificando Orden de Reparación. \", {type: \"error\"});";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel5, UpdatePanel5.GetType(), "alert", script, true);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al modificar orden de reparacion " + ex.Message);
            }
        }
    }
}