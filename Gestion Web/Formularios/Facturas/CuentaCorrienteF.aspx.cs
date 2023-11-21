using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using Gestion_Api.Entitys;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class CuentaCorrienteF : System.Web.UI.Page
    {
        controladorCobranza contCobranza = new controladorCobranza();
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        controladorUsuario contUser = new controladorUsuario();
        controladorDespacho contDesp = new controladorDespacho();
        Configuracion configuracion = new Configuracion();

        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();

        int idCliente;
        int idSucursal;
        int idTipo;
        int idVendedor;
        int accion;
        string fechaDesde;
        string fechaHasta;
        string ordX;
        string venc = "0";
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.idCliente = Convert.ToInt32(Request.QueryString["Cliente"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["Sucursal"]);
                this.idTipo = Convert.ToInt32(Request.QueryString["Tipo"]);
                this.idVendedor = (int)Session["Login_Vendedor"];
                this.VerificarLogin();
                this.fechaDesde = Request.QueryString["fd"];
                this.fechaHasta = Request.QueryString["fh"];
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.ordX = Request.QueryString["ordX"];
                this.venc = Request.QueryString["venc"];
                //this.cargarMovimientos();
                if (!IsPostBack)
                {
                    if (String.IsNullOrEmpty(this.fechaDesde) && String.IsNullOrEmpty(this.fechaHasta))
                    {
                        this.fechaDesde = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                        this.fechaHasta = DateTime.Today.ToString("dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(configuracion.FechaFiltrosCuentaCorriente))
                            this.fechaDesde = configuracion.FechaFiltrosCuentaCorriente.Substring(11, 10).Replace(";", "/");
                    }

                    this.txtFechaDesde.Text = this.fechaDesde;
                    this.txtFechaHasta.Text = this.fechaHasta;

                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", Request.Url.ToString());
                    this.verificarModoBlanco();
                    if (idCliente == 0 && idSucursal == 0 && idTipo == 0)
                    {
                        idSucursal = (int)Session["Login_SucUser"];
                        this.cargarSucursal();
                        this.cargarClientes(1);
                        DropListSucursal.SelectedValue = idSucursal.ToString();
                    }
                    this.cargarSucursal();
                    this.cargarClientes(1);
                    DropListSucursal.SelectedValue = idSucursal.ToString();
                    //DropListTipo.SelectedValue = idTipo.ToString();
                    DropListClientes.SelectedValue = idCliente.ToString();
                    ListRazonSocial.SelectedValue = idCliente.ToString();

                    string perfil2 = Session["Login_NombrePerfil"] as string;
                    if (perfil2 == "Distribuidor")
                    {
                        phSucursales.Visible = false;
                        phTipo.Visible = false;
                        phClienteBusqueda.Visible = false;
                        //this.txtCodCliente.Visible = false;
                        //this.btnBuscarCod.Visible = false;
                        //this.DropListTipo.Visible = false;
                        //this.DropListSucursal.Visible = false;
                        txtCodCliente.Attributes.Add("disabled", "disabled");
                        btnBuscarCod.Attributes.Add("disabled", "disabled");
                        DropListTipo.SelectedValue = "-1";
                        DropListTipo.Attributes.Add("disabled", "disabled");
                    }

                }

                if (idCliente > 0)
                {
                    //GridCtaCte.PageSize = Convert.ToInt32(configuracion.PageSizeGridView);
                    GridCtaCte.PageSize = Convert.ToInt32(11);
                    this.cargarMovimientos(idCliente, idSucursal, idTipo);
                }


                if (accion > 0)
                {
                    //this.btnImpagas.Visible = true;
                    this.btnImprimir.Visible = true;
                    this.btnAccion.Visible = true;
                    if (idTipo >= 0)
                    {
                        this.lbnCobros.Visible = true;
                    }
                    else
                    {
                        this.lbnCobros.Visible = false;
                    }
                }
                if (accion == 0)
                {
                    this.btnAccion.Visible = true;

                }
                if (accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "cambiarIcono", "cambiarIcono('fa fa-toggle-off','Ventas > Cuentas Corrientes > Impagas');", true);
                }
                if (accion == 2)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "cambiarIcono", "cambiarIcono('fa fa-toggle-on','Ventas > Cuentas Corrientes');", true);
                }
                this.Form.DefaultButton = lbtnBuscar.UniqueID;


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
                        if (s == "40")
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "SuperAdministrador")
                            {
                                this.DropListSucursal.Attributes.Remove("disabled");
                            }
                            else
                            {
                                int i = this.verficarPermisoCambiarSucursal();
                                if (i <= 0)
                                {
                                    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                                }
                                //this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                            }

                            if (perfil == "Cliente")
                            {
                                this.btnBuscarCod.Visible = false;
                            }

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
        public int verficarPermisoCambiarSucursal()
        {
            try
            {
                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "88")
                        {
                            this.DropListSucursal.Attributes.Remove("disabled");
                            return 1;
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public void verificarModoBlanco()
        {
            try
            {
                Configuracion config = new Configuracion();
                if (config.modoBlanco == "1")
                {
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("Ambos"));
                    this.DropListTipo.Items.Remove(this.DropListTipo.Items.FindByText("PRP"));
                    this.idTipo = 0;
                }
            }
            catch
            {

            }
        }
        #region cargas iniciales
        public void cargarClientes(int tipoCarga)
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();
                string perfil = Session["Login_NombrePerfil"] as string;
                if (perfil == "Vendedor")
                {
                    dt = contCliente.obtenerClientesByVendedorDT(this.idVendedor);

                    this.DropListClientes.DataSource = dt;
                    this.DropListClientes.DataValueField = "id";
                    this.DropListClientes.DataTextField = "alias";
                    this.DropListClientes.DataBind();
                    this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                    this.ListRazonSocial.DataSource = dt;
                    this.ListRazonSocial.DataValueField = "id";
                    this.ListRazonSocial.DataTextField = "razonSocial";
                    this.ListRazonSocial.DataBind();
                    this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                }
                else
                {
                    if (perfil == "Distribuidor")
                    {
                        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
                        //dt = contCliente.obtenerClientesByDistribuidorDT(this.idVendedor);//idvendedor es un cliente
                        dt = contClienteEntity.obtenerLideresPorDistribuidor(this.idVendedor);

                        DataRow dr = dt.NewRow();
                        Gestor_Solution.Modelo.Cliente c = contCliente.obtenerClienteID(this.idVendedor);
                        if (c != null)
                        {
                            dr["Id"] = c.id.ToString();
                            dr["Alias"] = c.alias;
                            dr["RazonSocial"] = c.razonSocial;
                            dt.Rows.Add(dr);
                        }
                        this.DropListClientes.DataSource = dt;
                        this.DropListClientes.DataValueField = "Id";
                        this.DropListClientes.DataTextField = "Alias";
                        this.DropListClientes.DataBind();
                        this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                        this.ListRazonSocial.DataSource = dt;
                        this.ListRazonSocial.DataValueField = "Id";
                        this.ListRazonSocial.DataTextField = "RazonSocial";
                        this.ListRazonSocial.DataBind();
                        this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    }
                    else if (perfil == "Cliente")
                    {
                        dt = contCliente.obtenerClientesByClienteDT(this.idVendedor);
                        this.DropListClientes.DataSource = dt;
                        this.DropListClientes.DataValueField = "id";
                        this.DropListClientes.DataTextField = "alias";
                        this.DropListClientes.DataBind();
                        this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                        this.ListRazonSocial.DataSource = dt;
                        this.ListRazonSocial.DataValueField = "id";
                        this.ListRazonSocial.DataTextField = "razonSocial";
                        this.ListRazonSocial.DataBind();
                        this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    }
                    else
                    {
                        dt = contCliente.obtenerClientesDT();

                        this.DropListClientes.DataSource = dt;
                        this.DropListClientes.DataValueField = "id";
                        this.DropListClientes.DataTextField = "alias";
                        this.DropListClientes.DataBind();
                        this.DropListClientes.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                        this.ListRazonSocial.DataSource = dt;
                        this.ListRazonSocial.DataValueField = "id";
                        this.ListRazonSocial.DataTextField = "razonSocial";
                        this.ListRazonSocial.DataBind();
                        this.ListRazonSocial.Items.Insert(0, new ListItem("Seleccione...", "-1"));
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando proveedores a la lista. " + ex.Message));
            }
        }

        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();
                DataTable dt2 = contSucu.obtenerSucursales();

                //agrego seleccion
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

                //droplist modal compensacion
                //agrego seleccion
                DataRow dr3 = dt2.NewRow();
                dr3["nombre"] = "Seleccione...";
                dr3["id"] = -1;
                dt2.Rows.InsertAt(dr3, 0);

                this.ListSucursalDestino.DataSource = dt2;
                this.ListSucursalDestino.DataValueField = "Id";
                this.ListSucursalDestino.DataTextField = "nombre";

                this.ListSucursalDestino.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void cargarPuntoVentaDestino(int sucu)
        {
            controladorSucursal contSucu = new controladorSucursal();
            DataTable dt = contSucu.obtenerPuntoVentaDT(sucu);

            //agrego todos
            DataRow dr = dt.NewRow();
            dr["NombreFantasia"] = "Seleccione...";
            dr["id"] = -1;
            dt.Rows.InsertAt(dr, 0);

            this.ListPuntoVentaDestino.DataSource = dt;
            this.ListPuntoVentaDestino.DataValueField = "Id";
            this.ListPuntoVentaDestino.DataTextField = "NombreFantasia";

            this.ListPuntoVentaDestino.DataBind();
        }

        #endregion
        private void cargarMovimientos(int idCliente, int idSucursal, int idTipo)
        {
            try
            {
                this.GridCtaCte.AutoGenerateColumns = false;

                DateTime fdesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                int vencimiento = 0;
                if (!String.IsNullOrEmpty(venc))
                {
                    vencimiento = Convert.ToInt32(venc);
                }
                //Metodo viejo para cargar los movimientos
                //DataTable datos = controlador.obtenerMovimientosByCuentaDT(idCliente, idSucursal, idTipo, this.accion, fdesde, fhasta, vencimiento);

                //Agrego este camino alternativo ya que no se que tanto puede llegar a romperse con las modificaciones de los dias vencidos
                DataTable datos = controlador.obtenerMovimientosByCuentaDiasVencidosDT(idCliente, idSucursal, idTipo, this.accion, fdesde, fhasta, vencimiento);

                if (String.IsNullOrEmpty(ordX) || ordX == "3" || ordX == "4")
                {
                    datos.DefaultView.Sort = "fechaVenc, id";
                    DataTable tOrd = datos.DefaultView.ToTable();
                    datos = tOrd.Copy();
                }

                decimal saldoAcumulado = 0; //Declaro e inicializo la variable que va a guardar el saldo acumulado

                foreach (DataRow row in datos.Rows) //Recorremos las filas una a una
                {
                    if (this.accion == 2)
                    {
                        if (Math.Abs(Convert.ToDecimal(row["debe"])) > 0) //Si el debe es mayor a 0 se trata de una factura
                        {
                            saldoAcumulado += Convert.ToDecimal(row["debe"]); //Por lo que la deuda aumenta y por tanto el saldo acumulado
                        }
                        if (Math.Abs(Convert.ToDecimal(row["haber"])) > 0) //Si el haber es mayor a 0 se trata de un cobro
                        {
                            saldoAcumulado -= Convert.ToDecimal(row["haber"]); //Por lo que se salda parte de la deuda y por tanto el saldo disminuye
                        }
                    }
                    else
                    {
                        saldoAcumulado += Convert.ToDecimal(row["saldo"]); 
                    }

                    row["SaldoAcumulado"] = saldoAcumulado.ToString(); //Llevamos el registro del calculo a medida que vamos cargando los documentos en la columna SaldoAcumulado
                }

                //Ordenamos la tabla
                datos.DefaultView.Sort = "fechaVenc desc, id desc";
                DataTable tablaOrd = datos.DefaultView.ToTable();
                datos = tablaOrd.Copy();

                string tipoOrden = "FechaVenc desc";

                switch (ordX)
                {
                    case "1":
                        datos.DefaultView.Sort = "fecha asc, id";
                        tipoOrden = "Fecha asc";
                        break;
                    case "2":
                        datos.DefaultView.Sort = "fecha desc, id desc";
                        tipoOrden = "Fecha desc";
                        break;
                    case "3":
                        datos.DefaultView.Sort = "fechaVenc asc, id";
                        tipoOrden = "FechaVenc asc";
                        break;
                    case "4":
                        datos.DefaultView.Sort = "fechaVenc desc, id desc";
                        tipoOrden = "FechaVenc desc";
                        break;
                }


                //Copiamos la tabla ordenada
                DataTable sortedTable = datos.DefaultView.ToTable();
                datos = sortedTable.Copy();

                //Y la asignamos al datagridview para poder mostrar los datos en el front
                if (venc == "1")
                {
                    this.GridCtaCte.DataSource = datos;
                }
                else
                {
                    this.GridCtaCte.DataSource = datos;
                }

                this.GridCtaCte.DataBind();



                //Cargamos el label acá así no damos 50 vueltas al pedo
                Cliente client = contrCliente.obtenerClienteID(idCliente);
                int maxCantDias = ObtenerMayorNumeroDiasFacturaVencida(datos);
                //El número de "Días Vencidos" debe cambiar de color según si hay documentos vencidos o no.Verde si no hay nada vencido, Rojo si hay documentos(con saldo) vencidos.
                this.CargarLabelDiasVencidos(maxCantDias,client);


                this.labelSaldo.Text = saldoAcumulado.ToString("C");
                this.cargarLabel(idCliente, idSucursal, idTipo);
                //"Saldo Máximo".Debe cambiar de color según si el total de los documentos impagos suman o no más que lo que dice en "Saldo Máximo".Verde si no supera, Rojo si supera.
               cargarLabelSaldos(client);
                lblParametros.Text += " Ordenado por " + tipoOrden;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error buscando cliente. " + ex.Message));
            }
        }

        private void CargarLabelDiasVencidos(int maxCantDias, Cliente client)
        {
            //En el label DiasVencidos, se asigna la cantidad de días vencidos que tiene un cliente segun su configuracion personal
            //algunos tiene 0, otros tienen 7, 30, etc.
            int dias = Convert.ToInt32(client.vencFC);
            //Si maxCantDias es DIFERENTE de ese numero significa que hay facturas impagas vencidas
            bool hayVencidas = maxCantDias != -2712 ? true : false; 
            string color = hayVencidas ? "danger" : "success"; //Estilo de bootstrap para pintar de rojo o de verde
            lblDiasVenc.Text = "<span class=\"text-" + color + "\">" + dias + "</span>";
        }

        private void cargarLabelSaldos(Cliente client)
        {
            try
            {
                DateTime fdesde = Convert.ToDateTime(this.txtFechaDesde.Text, new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(this.txtFechaHasta.Text, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                DataTable dtVencidas = controlador.obtenerMovimientosByCuentaDT(idCliente, idSucursal, idTipo, 1, fdesde, fhasta, 1); //Vencidas
                DataTable dtImpagas = controlador.obtenerMovimientosByCuentaDT(idCliente, idSucursal, idTipo, 2, fdesde, fhasta, 0); //Impagas

                decimal saldoAcumuladoVenc = 0;
                decimal saldoAcumuladoImp = 0;

                foreach (DataRow item in dtVencidas.Rows)
                {
                    if (!String.IsNullOrEmpty(item.ItemArray[12].ToString()))
                    {
                        if (Convert.ToInt32(item.ItemArray[12]) > 0)
                        {
                            saldoAcumuladoVenc += Convert.ToDecimal(item["saldo"]);
                        }
                    }
                }

                #region Ordenamos los movimientos

                dtImpagas.DefaultView.Sort = "fechaVenc desc";

                string tipoOrden = "FechaVenc desc";
                //Session["fechaVenc_estado"] = "";
                if (ordX == "1")
                {
                    dtImpagas.DefaultView.Sort = "fecha asc";
                    tipoOrden = "Fecha asc";
                }
                else if (ordX == "2")
                {
                    dtImpagas.DefaultView.Sort = "fecha desc";
                    tipoOrden = "Fecha desc";
                }
                else if (ordX == "3")
                {
                    dtImpagas.DefaultView.Sort = "fechaVenc asc";
                    tipoOrden = "FechaVenc asc";
                }
                else if (ordX == "4")
                {
                    dtImpagas.DefaultView.Sort = "fechaVenc desc";
                    tipoOrden = "FechaVenc desc";
                }


                DataTable sortedTable = dtImpagas.DefaultView.ToTable();
                dtImpagas = sortedTable.Copy();

                #endregion

                foreach (DataRow row in dtImpagas.Rows)
                {
                    if (Math.Abs(Convert.ToDecimal(row["debe"])) > 0)
                    {
                        saldoAcumuladoImp += Convert.ToDecimal(row["debe"]);
                    }
                    if (Math.Abs(Convert.ToDecimal(row["haber"])) > 0)
                    {
                        saldoAcumuladoImp -= Convert.ToDecimal(row["haber"]);
                    }
                }

                this.labelSaldoImp.Text = saldoAcumuladoImp.ToString("C");
                this.labelSaldoVenc.Text = saldoAcumuladoVenc.ToString("C");

                ///decimal saldoOperativo = ObtenerSaldoOperativo();
                string color = Convert.ToDecimal(client.saldoMax) > saldoAcumuladoImp ? "success" : "danger";
                lblSaldoMax.Text = "<span class=\"text-" + color + "\">$ " + client.saldoMax.ToString("N", new CultureInfo("is-IS")) + "</span>";
                
                Session.Add("saldoVenc", saldoAcumuladoVenc.ToString("C"));
                Session.Add("saldoMax", client.saldoMax.ToString("N", new CultureInfo("is-IS")));
                Session.Add("diasVenc", client.vencFC.ToString());
            }
            catch (Exception ex)
            {
               
            }
        }

        private decimal ObtenerSaldoOperativo()
        {
            DataTable datos = controlador.obtenerMovimientosByCuentaDT(this.cliente.id, 0, -1, 2, Convert.ToDateTime("01/01/2000"), DateTime.Today, Convert.ToInt32(venc));
            decimal saldoOperativo;
            decimal saldoAcumulado = 0;

            foreach (DataRow row in datos.Rows)
            {
                if (this.accion == 2)
                {
                    if (Math.Abs(Convert.ToDecimal(row["debe"])) > 0)
                    {
                        saldoAcumulado += Convert.ToDecimal(row["debe"]);
                    }
                    if (Math.Abs(Convert.ToDecimal(row["haber"])) > 0)
                    {
                        saldoAcumulado -= Convert.ToDecimal(row["haber"]);
                    }
                }
                else
                {
                    saldoAcumulado += Convert.ToDecimal(row["saldo"]);
                }

                row["SaldoAcumulado"] = saldoAcumulado.ToString();
            }

            //saldoOperativo = cliente.saldoMax - saldoAcumulado;
            saldoOperativo = saldoAcumulado;
            return saldoOperativo;
        }

        //Vamos a recorrer la tabla de datos y vamos a devolver el mayor numero de dias vencidos que tiene una factura (vencida)
        private int ObtenerMayorNumeroDiasFacturaVencida(DataTable datos)
        {
            try
            {
                DateTime fdesde = Convert.ToDateTime("2000/01/01", new CultureInfo("es-AR"));
                DateTime fhasta = Convert.ToDateTime(DateTime.Today, new CultureInfo("es-AR")).AddHours(23).AddMinutes(59);

                // DataTable datos = controlador.obtenerMovimientosByCuentaDT(idCliente, idSucursal, -1, this.accion, fdesde, fhasta, Convert.ToInt32(venc));
                
                //Agrego el nuevo metodo que levanta el procedimiento nuevo que trae los dias corregidos 
                //DataTable datosdias = controlador.obtenerMovimientosByCuentaDiasVencidosDT(idCliente, idSucursal, -1, this.accion, fdesde, fhasta, Convert.ToInt32(venc));

                int maxCantDias = -2712;
                bool primerValor = true;
                string columna = "";
                bool tieneDias = false;
                bool esNumero = false;
                int dias = 0;
                //foreach (DataRow item in datosdias.Rows) //Recorremos las filas de la tabla
                foreach (DataRow item in datos.Rows) //Recorremos las filas de la tabla
                {
                    double columnaSaldo = Convert.ToDouble(item["saldo"]);
                    bool esImpaga = columnaSaldo > 0; //Preguntamos por el valor de la columna saldo

                    if (esImpaga) //Contamos los dias vencidos de la factura
                    {
                        columna = item["diasVencidos"].ToString(); //Guardamos el contenido de la columna en una variable
                        tieneDias = !String.IsNullOrEmpty(columna); //Verificamos que no este vacia
                        esNumero = columna != "-"; //A las notas y cobros les pone un - en la consulta de la DB
                        if (esNumero)
                        {
                            dias = Convert.ToInt32(item["diasVencidos"]);
                        }
                        //guardo el primer "diasVencidos" como el mas alto
                        if (primerValor)
                        {
                            if (tieneDias && esNumero)
                            {
                                maxCantDias = dias;
                                primerValor = false;
                            }
                        }
                        else
                        {
                            if (tieneDias && esNumero)
                            {
                                if(dias > maxCantDias)
                                {
                                    maxCantDias = dias;
                                    //primerValor = false;
                                }
                                
                            }
                        }
                    }
                }
                return maxCantDias;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private void cargarLabel(int idCliente, int idSucursal, int idTipo)
        {
            try
            {
                string label = "";
                if (idCliente > 0)
                {
                    if (DropListClientes.Items.FindByValue(idCliente.ToString()) != null)
                    {
                        label += DropListClientes.Items.FindByValue(idCliente.ToString()).Text + ",";
                    }
                    else
                    {
                        Cliente cl = this.contrCliente.obtenerClienteID(idCliente);
                        label += cl.razonSocial + ",";
                    }

                }
                if (idSucursal > 0)
                {
                    label += " "+ DropListSucursal.Items.FindByValue(idSucursal.ToString()).Text + ",";
                }
                if (idTipo > -1)
                {
                    label += " "+ DropListTipo.Items.FindByValue(idTipo.ToString()).Text;
                }
                if(!String.IsNullOrEmpty(venc))
                {
                    if(Convert.ToInt32(venc) == 1)
                    label += " Vencidas,";

                }else if (accion == 1)
                {
                    label += " Impagas,";
                }

                this.lblParametros.Text = label;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando datos de Busqueda. " + ex.Message));

            }
        }

        private void cargarEnPh(Movimiento m, decimal saldoAcumulado)
        {

            MovimientoView movV = new MovimientoView();
            movV = m.ListarMovimiento();
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = movV.id.ToString();

                //Celdas

                TableCell celFecha = new TableCell();
                celFecha.Text = movV.fecha.ToString("dd/MM/yyyy");
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celDebe = new TableCell();
                celDebe.Text = "$" + movV.debe;
                celDebe.VerticalAlign = VerticalAlign.Middle;
                celDebe.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celDebe);

                TableCell celHaber = new TableCell();
                celHaber.Text = "$" + movV.haber;
                celHaber.VerticalAlign = VerticalAlign.Middle;
                celHaber.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celHaber);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$" + movV.saldo;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldo);

                TableCell celSaldoAcumulado = new TableCell();
                celSaldoAcumulado.Text = "$" + saldoAcumulado.ToString();
                celSaldoAcumulado.VerticalAlign = VerticalAlign.Middle;
                celSaldoAcumulado.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celSaldoAcumulado);

                //arego fila a tabla

                //TableCell celAccion = new TableCell();

                //Button btnEliminar = new Button();
                //btnEliminar.CssClass = "btn btn-info";
                //btnEliminar.ID = "btnSelec_" + f.id;
                //btnEliminar.Text = "Detalle";
                ////btnEliminar.PostBackUrl = "#modalFacturaDetalle";
                //btnEliminar.Click += new EventHandler(this.detalleFactura);
                //celAccion.Controls.Add(btnEliminar);
                //celAccion.Width = Unit.Percentage(10);
                //celAccion.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celAccion);

                //phCuentaCorriente.Controls.Add(tr);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"), true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeGrowlSucces("Exito", "Articulos agregado con exito"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando articulos. " + ex.Message));
            }

        }

        #region eventos
        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CuentaCorrienteF.aspx?a=2&Cliente=" + DropListClientes.SelectedValue + "&Sucursal=" + DropListSucursal.SelectedValue + "&Tipo=" + DropListTipo.SelectedValue + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error buscando movimientos. " + ex.Message));
            }
        }

        protected void ListRazonSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.DropListClientes.SelectedValue = this.ListRazonSocial.SelectedValue;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en cliente. " + ex.Message));
            }
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ListRazonSocial.SelectedValue = this.DropListClientes.SelectedValue;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error seleccionando valor en razon social. " + ex.Message));
            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClientes = this.contrCliente.obtenerClientesAliasDT(this.txtCodCliente.Text);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();
                //this.cargarClientesTable(cliente);

                this.ListRazonSocial.DataSource = dtClientes;
                this.ListRazonSocial.DataValueField = "id";
                this.ListRazonSocial.DataTextField = "razonSocial";

                this.ListRazonSocial.DataBind();

            }
            catch (Exception ex)
            {

            }
        }
        protected void ListSucursalDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Attributes.Remove("hidden");
                this.lbAguarde.Attributes.Add("hidden", "true");
                this.cargarPuntoVentaDestino(Convert.ToInt32(this.ListSucursalDestino.SelectedValue));
            }
            catch
            {

            }
        }

        protected void btnImpagas_Click(object sender, EventArgs e)
        {
            try
            {
               /* if (accion == 1)// 1 = impagas
                {
                    Response.Redirect("CuentaCorrienteF.aspx?a=2&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
                }
                else
                {*/
                    Response.Redirect("CuentaCorrienteF.aspx?a=1&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
                //}

            }
            catch
            {

            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = (DataTable)this.GridCtaCte.DataSource;

                if (String.IsNullOrEmpty(ordX) || ordX == "3" || ordX == "4")
                {
                    dtDatos.DefaultView.Sort = "fechaVenc, id";
                    DataTable tOrd = dtDatos.DefaultView.ToTable();
                    dtDatos = tOrd.Copy();
                }
                else if (ordX == "1" || ordX == "2")
                {
                    dtDatos.DefaultView.Sort = "fecha, id";
                    DataTable tOrd = dtDatos.DefaultView.ToTable();
                    dtDatos = tOrd.Copy();
                }

                decimal saldoAcumulado = 0;

                foreach (DataRow row in dtDatos.Rows)
                {
                    if (this.accion == 2)
                    {
                        if (Math.Abs(Convert.ToDecimal(row["debe"])) > 0)
                        {
                            saldoAcumulado += Convert.ToDecimal(row["debe"]);
                        }
                        if (Math.Abs(Convert.ToDecimal(row["haber"])) > 0)
                        {
                            saldoAcumulado -= Convert.ToDecimal(row["haber"]);
                        }
                    }
                    else
                    {
                        saldoAcumulado += Convert.ToDecimal(row["saldo"]);
                    }

                    row["SaldoAcumulado"] = saldoAcumulado.ToString();
                }

                //DataView dtV = dtDatos.DefaultView;

                dtDatos.DefaultView.Sort = "fechaVenc desc, id desc";
                DataTable tablaOrd = dtDatos.DefaultView.ToTable();
                dtDatos = tablaOrd.Copy();

                string tipoOrden = "FechaVenc desc";


                //dtV.Sort = "FechaVenc desc";
                //string tipoOrden = "FechaVenc desc";

                if (ordX == "1")
                {
                    dtDatos.DefaultView.Sort = "fecha asc, id";
                    tipoOrden = "Fecha asc";
                }
                else if (ordX == "2")
                {
                    dtDatos.DefaultView.Sort = "fecha desc, id desc";
                    tipoOrden = "Fecha desc";
                }
                else if (ordX == "3")
                {
                    dtDatos.DefaultView.Sort = "fechaVenc asc, id";
                    tipoOrden = "FechaVenc asc";
                }
                else if (ordX == "4")
                {
                    dtDatos.DefaultView.Sort = "fechaVenc desc, id desc";
                    tipoOrden = "FechaVenc desc";
                }

                dtDatos = dtDatos.DefaultView.ToTable();

                if (venc == "1")
                {
                    DataTable datosVenc = dtDatos.Clone();
                    foreach (DataRow item in dtDatos.Rows)
                    {
                        if (!String.IsNullOrEmpty(item.ItemArray[12].ToString()))
                        {
                            if (Convert.ToInt32(item.ItemArray[12]) > 0)
                            {
                                DataRow dr = datosVenc.NewRow();
                                dr["Id"] = item.ItemArray[0];
                                dr["fecha"] = item.ItemArray[1];
                                dr["Numero"] = item.ItemArray[2];
                                dr["id_doc"] = item.ItemArray[3];
                                dr["tipo_doc"] = item.ItemArray[4];
                                dr["debe"] = item.ItemArray[5];
                                dr["haber"] = item.ItemArray[6];
                                dr["saldo"] = item.ItemArray[7];
                                dr["SaldoAcumulado"] = item.ItemArray[8];
                                dr["sucursal"] = item.ItemArray[9];
                                dr["GuiaDespacho"] = item.ItemArray[10];
                                dr["fechaVenc"] = item.ItemArray[11];
                                dr["diasVencidos"] = item.ItemArray[12];
                                dr["TipoDoc"] = item.ItemArray[13];
                                datosVenc.Rows.Add(dr);
                            }
                        }
                    }
                    dtDatos.Clear();
                    foreach (DataRow dtRow in datosVenc.Rows)
                    {
                        dtDatos.ImportRow(dtRow);
                    }
                }

                Cliente clientImpReport = contrCliente.obtenerClienteID(idCliente);
                Session.Add("datosMov", dtDatos);
                Session.Add("saldoMov", labelSaldo.Text);

                //Response.Redirect("ImpresionReportes.aspx?Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&a=" + this.accion);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionReportes.aspx?Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&a=" + this.accion + "','_blank')", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de factura desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error cargando reporte cta cte detalle desde la interfaz. " + ex.Message);
            }

        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dtDatos = (DataTable)this.GridCtaCte.DataSource;

                Session.Add("datosMov", dtDatos);
                Session.Add("saldoMov", labelSaldo.Text);

                Response.Redirect("ImpresionReportes.aspx?Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&a=" + this.accion + "&e=1");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar detalle de cta cte desde la interfaz. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error exportando detalles de cta cte a excel. " + ex.Message);
            }
        }

        protected void btnCompensar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                //this.lblMovimiento.Text = btn.CommandArgument;
                string movimiento = btn.CommandArgument;
                Session.Add("Mov_CompCta", movimiento);

                this.panel1.Attributes.Remove("hidden");
                this.lbAguarde.Attributes.Add("hidden", "true");
            }
            catch
            {

            }
        }
        protected void btnComentario_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                string movimiento = btn.CommandArgument;
                string idDoc = movimiento.Split('_')[0];
                string tipoDoc = movimiento.Split('_')[1];

                this.txtComentario.Text = movimiento;

                if (tipoDoc != "15" && tipoDoc != "16" && tipoDoc != "18" && tipoDoc != "31" && tipoDoc != "32")
                {
                    Factura f = this.contFact.obtenerFacturaId(Convert.ToInt32(idDoc));
                    if (f != null)
                        this.txtComentario.Text = f.comentario;
                }

                this.panel2.Attributes.Remove("hidden");
                this.lbAguarde2.Attributes.Add("hidden", "true");
            }
            catch
            {

            }
        }
        protected void btnImprimir_Click1(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;
                string movimiento = btn.CommandArgument;
                if (!String.IsNullOrEmpty(movimiento))
                {
                    int idDoc = Convert.ToInt32(movimiento.Split('_')[0]);
                    int tipoDoc = Convert.ToInt32(movimiento.Split('_')[1]);
                    string script = "";
                    if (tipoDoc == 17 || tipoDoc == 11 || tipoDoc == 12)//Si es PRP o Nota Cred. PRP o Nota Deb. PRP
                    {
                        script = "window.open('ImpresionPresupuesto.aspx?Presupuesto=" + idDoc + "','_blank');";
                    }
                    else
                    {
                        if (tipoDoc == 1 || tipoDoc == 9 || tipoDoc == 4 || tipoDoc == 24 || tipoDoc == 25 || tipoDoc == 26)//Si es Factura A/E, Nota credito A/E o Nota debito A/E
                        {
                            //factura
                            script = "window.open('ImpresionPresupuesto.aspx?a=1&Presupuesto=" + idDoc + "','_blank');";
                        }
                        else
                        {
                            if (tipoDoc == 2 || tipoDoc == 5 || tipoDoc == 8)
                            {
                                //facturab
                                script = "window.open('ImpresionPresupuesto.aspx?a=2&Presupuesto=" + idDoc + "','_blank');";
                            }
                            else
                            {
                                if (tipoDoc == 15 || tipoDoc == 18)
                                {
                                    //recibo de cobro
                                    int idMov = contCobranza.transformarIdCobroEnIdMov(Convert.ToInt32(idDoc));
                                    script = "window.open('../Cobros/ImpresionCobro.aspx?valor=2&Cobro=" + idMov + "','_blank');";
                                }
                            }
                        }

                    }

                    if (script != "")
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel3, UpdatePanel3.GetType(), "alert", script, true);

                }

            }
            catch
            {

            }
        }
        protected void lbtnAgregarCompensacion_Click(object sender, EventArgs e)
        {
            try
            {
                //int idMov = Convert.ToInt32(this.labelMov.Text);
                string idMov = Session["Mov_CompCta"].ToString();
                Session["Mov_CompCta"] = null;
                int sucDestino = Convert.ToInt32(this.ListSucursalDestino.SelectedValue);
                int ptoVentaDestino = Convert.ToInt32(this.ListPuntoVentaDestino.SelectedValue);

                Movimiento mov = this.controlador.obtenerMovimientoID(Convert.ToInt32(idMov));

                if (Math.Abs(mov.saldo) > 0 && ((Math.Abs(mov.saldo) == Math.Abs(mov.debe)) || (Math.Abs(mov.saldo) == Math.Abs(mov.haber))))
                {
                    int i = this.controlador.GenerarCompensacionCuentas(sucDestino, ptoVentaDestino, Convert.ToInt32(idMov));

                    if (i > 0)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Proceso finalizado con exito", Request.Url.ToString()));
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Proceso finalizado con exito \",{type: \"info\"});", true);
                        Response.Redirect(Request.Url.ToString());
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", m.mensajeBoxAtencion("No se pudo generar mov de compensacion!."), true);
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo generar mov de compensacion!.\");", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Solo puede compensar mov que no tengan cancelaciones!.\");", true);
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error al generar movimiento compensacion de cta. \",{type: \"error\"});", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error al generar movimiento compensacion de cta. " + ex.Message));
            }
        }
        #endregion
        protected void GridInforme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridCtaCte.PageIndex = e.NewPageIndex;
            this.GridCtaCte.DataBind();
        }
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }


        protected void lbtnExportartxt_Click(object sender, EventArgs e)
        {
            try
            {

                SolicitarReporteCuentaCorriente();
            }
            catch (Exception ex)
            {



                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "CATCH: No se pudo generar el archivo.txt con la cuenta corriente. Ubicacion: CuentaCorrienteF.aspx. Metodo: lbtnExportarCuentaCorriente_Click. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));

            }

        }
        public void cargarDatosInformePedido(Informes_Pedidos ip, int accion)
        {
            try
            {
                ip.Fecha = DateTime.Now;
                ip.Usuario = (int)Session["Login_IdUser"];
                ip.Estado = 0;

                switch (accion)
                {
                    ///Informe para IIBB
                    case 1:
                        ip.Informe = 10;
                        ip.NombreInforme = "ECOMMERCE-CUENTACORRIENTE_";
                        ip.Usuario = (int)Session["Login_IdUser"];
                        break;
                    ///Informe para Ventas Filtradas

                    default:
                        break;
                }

            }
            catch (Exception Ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: Articulos.aspx. Metodo: cargarDatosInformePedido. Excepcion: " + Ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }

        }

        public void SolicitarReporteCuentaCorriente()
        {
            try
            {
                ControladorInformesEntity controladorInformesEntity = new ControladorInformesEntity();
                Informes_Pedidos ip = new Informes_Pedidos();
                InformeXML infXML = new InformeXML();

                ///Cargo el objeto Informes_Pedidos
                cargarDatosInformePedido(ip, 1);

                ///Cargo el objeto InformeXML

                ///Concatenamos el ID de la insercion al reporte a guardar
                ip.NombreInforme += (controladorInformesEntity.ObtenerUltimoIdInformePedido() + 1).ToString();

                ///Agrego el informe para ejecutar la funcion de reporte de filtro de ventas. Si todo es correcto retorna 1.
                int i = controladorInformesEntity.generarPedidoDeInforme(infXML, ip);

                if (i > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Se ha generado la solicitud de reporte de ventas con el nombre de <strong>" + ip.NombreInforme + "</strong> porque la cantidad de registros encontrados es mayor a 2000. Podra visualizar el estado del reporte en <strong><a href='/Formularios/Reportes/InformesF.aspx'>Informes Solicitados</a></strong>.", null));
                else
                {
                    int idError = Log.ObtenerUltimoIDLog();
                    Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "ELSE: No pudo generar un pedido para el reporte de ventas. Ubicacion: Articulos.aspx. Metodo: cargarFacturasRango.");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
                }

            }
            catch (Exception ex)
            {
                int idError = Log.EscribirSQLDevuelveID((int)Session["Login_IdUser"], "ERROR", "Ubicacion: Articulos.aspx. Metodo: cargarFacturasRango. Excepcion: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError(idError.ToString()));
            }

        }

        protected void lbnCobros_Click(object sender, EventArgs e)
        {
            if (this.idCliente != 0 && this.idTipo >= 0 && this.idSucursal > 0)
            {
                //Response.Redirect("../Cobros/CobranzaF.aspx?cliente=" + this.idCliente + "&sucursal=" + this.idSucursal + "&tipo=" + this.idTipo);
                //ScriptManager.RegisterStartupScript(this, GetType(), "cambiarIcono", "abrirCobros('"+ this.idCliente+"', '"+ this.idSucursal+"', '"+idTipo+"')", true);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('../Cobros/CobranzaF.aspx?cliente=" + this.idCliente + "&sucursal=" + this.idSucursal + "&tipo=" + this.idTipo + "', '_blank');");
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {

        }

        protected void btnOrdenarX_Click(object sender, EventArgs e)
        {

        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=2&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text);
        }

        protected void btnVencidas_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&venc=1");
        }

        protected void btnFEAsc_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&ordX=1");
        }

        protected void btnFEDesc_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&ordX=2");
        }

        protected void btnFVAsc_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&ordX=3");
        }

        protected void btnFVDesc_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=" + this.idTipo + "&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&ordX=4");
        }

        protected void tipoAmbos_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=-1&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&venc=" + venc);
        }

        protected void tipoFC_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=0&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&venc=" + venc);
        }

        protected void tipoPRP_Click(object sender, EventArgs e)
        {
            Response.Redirect("CuentaCorrienteF.aspx?a=" + accion + "&Cliente=" + this.idCliente + "&Sucursal=" + this.idSucursal + "&Tipo=1&fd=" + this.txtFechaDesde.Text + "&fh=" + this.txtFechaHasta.Text + "&venc=" + venc);
        }

        protected void GridCtaCte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool esNota = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Acceder a los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                DataRow row = rowView.Row;

                // Obtener los valores de las columnas necesarias
                int diasVencidos = Convert.ToInt32(row["DiasVencidos"]); // Asegúrate de usar "DiasVencidos" con la letra "D" mayúscula
                string descripcion = row["Numero"].ToString();
                if (descripcion.Contains("Nota"))
                {
                    esNota = true;
                }
                // Acceder a la celda específica en la fila actual
                TableCell diasVencidosCell = e.Row.Cells[getColumnIndexByName("Dias Vencidos")]; // Asegúrate de usar "Dias Vencidos" con la letra "D" mayúscula

                // Verificar si el valor de "diasVencidos" es igual a -2712 o si se trata de una Nota ya sea de credito o debito
                if (diasVencidos == -2712 || esNota)
                {
                    // Cambiar el texto en la celda
                    diasVencidosCell.Text = "-";
                }
                else if (diasVencidos < 0)
                {
                    diasVencidosCell.ForeColor = System.Drawing.Color.Green;
                }
                else if (diasVencidos > 0)
                {
                    diasVencidosCell.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private int getColumnIndexByName(string columnName)
        {
            foreach (DataControlField field in GridCtaCte.Columns)
            {
                if (field.HeaderText == columnName)
                {
                    return GridCtaCte.Columns.IndexOf(field);
                }
            }
            return -1; // Retornar -1 si la columna no se encuentra
        }
    }
}