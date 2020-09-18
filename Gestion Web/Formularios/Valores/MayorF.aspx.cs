using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class MayorF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        public ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
        public controladorSucursal contSucursal = new controladorSucursal();
        public ControladorEmpresa contEmpresa = new ControladorEmpresa();

        class MayorTemporal
        {
            public string Id;
            public string Fecha;
            public string TipoMovimiento;
            public string Empresa;
            public string Sucursal;
            public string PuntoDeVenta;
            public string Nivel1;
            public string Nivel2;
            public string Nivel3;
            public string Nivel4;
            public string Nivel5;
            public string Credito;
            public string Debito;
            public string NumeroDocumento;
            public string Observaciones;
            
        }
        class ListItemTemporal
        {
            public string id;
            public string nombre;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            VerificarLogin();
            if (!IsPostBack)
            {
                CargarDropLists();
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
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        #region cargas drop list
        private void CargarDropLists()
        {
            CargarEmpresasEnLista();
            CargarSucursalesEnLista();
            CargarPuntosDeVentaEnLista();
            CargarTiposMovimientosEnLista();
            CargarNivel1EnLista();
            CargarNivelesDropList_Busqueda_ConElItemTODOS();
            CargarNivelesDeLosDropDown();
        }

        private void CargarEmpresasEnLista()
        {
            try
            {
                var dtEmpresas = contSucursal.obtenerEmpresas();

                if (dtEmpresas != null)
                {
                    //Log.EscribirSQL(1, "ERROR", "CargarEmpresasEnLista() existen empresas");
                    if (dtEmpresas.Rows.Count > 1)
                    {
                        DataRow drEmpresa = dtEmpresas.NewRow();
                        drEmpresa["Razon Social"] = "Seleccione...";
                        drEmpresa["Id"] = "0";
                        dtEmpresas.Rows.InsertAt(drEmpresa, 0);

                        //Log.EscribirSQL(1, "ERROR", "Count: " + dtEmpresas.Rows.Count);
                    }

                    DropListEmpresa.DataSource = dtEmpresas;
                    DropListEmpresa.DataTextField = "Razon Social";
                    DropListEmpresa.DataValueField = "Id";
                    DropListEmpresa.DataBind();

                    //Log.EscribirSQL(1, "ERROR", "count 2: " + dtEmpresas.Rows.Count);
                    //Log.EscribirSQL(1, "ERROR", "Id: " + dtEmpresas.Rows[0][0]);
                    //Log.EscribirSQL(1, "ERROR", "razon: " + dtEmpresas.Rows[0][1]);

                    DropListEmpresa_ModalAgregarRegistro.DataSource = dtEmpresas;
                    DropListEmpresa_ModalAgregarRegistro.DataTextField = "Razon Social";
                    DropListEmpresa_ModalAgregarRegistro.DataValueField = "Id";
                    DropListEmpresa_ModalAgregarRegistro.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name + " Ex: " + ex.Message);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name + " Ex: " + ex.Message));
            }
        }

        private void CargarSucursalesEnLista(int idEmpresa = 0)
        {
            DataTable dtSucursales = new DataTable();

            if (idEmpresa > 0)
            {
                dtSucursales = contSucursal.obtenerSucursalesDT(idEmpresa);
            }
            else
            {
                dtSucursales = contSucursal.obtenerSucursales();
            }

            if (dtSucursales != null)
            {
                if (dtSucursales.Rows.Count > 1)
                {
                    DataRow drSucursal = dtSucursales.NewRow();
                    drSucursal["nombre"] = "Seleccione...";
                    drSucursal["id"] = "0";
                    dtSucursales.Rows.InsertAt(drSucursal, 0);
                }

                DropListSucursal.DataSource = dtSucursales;
                DropListSucursal.DataTextField = "nombre";
                DropListSucursal.DataValueField = "id";
                DropListSucursal.DataBind();

                DropListSucursal_ModalAgregarRegistro.DataSource = dtSucursales;
                DropListSucursal_ModalAgregarRegistro.DataTextField = "nombre";
                DropListSucursal_ModalAgregarRegistro.DataValueField = "id";
                DropListSucursal_ModalAgregarRegistro.DataBind();
            }
        }

        private void CargarPuntosDeVentaEnLista(int idSucursal = 0)
        {
            DataTable dtPuntosDeVenta = new DataTable();

            controladorSucursal controladorSucursal = new controladorSucursal();

            if (idSucursal > 0)
            {
                dtPuntosDeVenta = controladorSucursal.obtenerPuntoVentaDT(idSucursal);
            }
            else
            {
                dtPuntosDeVenta = controladorSucursal.obtenerPuntoVenta();
            }

            if (dtPuntosDeVenta != null)
            {
                DropListPuntoVenta.DataSource = dtPuntosDeVenta;
                DropListPuntoVenta.DataTextField = "NombreFantasia";
                DropListPuntoVenta.DataValueField = "Id";
                DropListPuntoVenta.DataBind();

                dropListPuntoVenta_ModalAgregarRegistro.DataSource = dtPuntosDeVenta;
                dropListPuntoVenta_ModalAgregarRegistro.DataTextField = "NombreFantasia";
                dropListPuntoVenta_ModalAgregarRegistro.DataValueField = "Id";
                dropListPuntoVenta_ModalAgregarRegistro.DataBind();
            }
        }

        private void CargarTiposMovimientosEnLista()
        {
            var dtMayor_TipoMovimiento = contPlanCuentas.GetAll_Mayor_TipoMovimiento();

            if (dtMayor_TipoMovimiento != null)
            {
                if (dtMayor_TipoMovimiento.Count > 1)
                {
                    dtMayor_TipoMovimiento.Insert(0, new Mayor_TipoMovimiento
                    {
                        Id = 0,
                        TipoMovimiento = "Todos"
                    });
                }
                DropListTipoDeMovimiento.DataSource = dtMayor_TipoMovimiento;
                DropListTipoDeMovimiento.DataTextField = "TipoMovimiento";
                DropListTipoDeMovimiento.DataValueField = "Id";
                DropListTipoDeMovimiento.DataBind();
            }
        }

        public void CargarNivel1EnLista()
        {
            try
            {
                var lista = contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);
                var lista_ModalAgregarRegistro = contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);
                if (lista != null)
                {
                    if (lista.Count > 0)
                    {
                        lista.Insert(0, new Cuentas_Contables
                        {
                            Id = 0,
                            Descripcion = "Todos"
                        });
                    }
                    dropListNivel1_ModalBusqueda.DataSource = lista;
                    dropListNivel1_ModalBusqueda.DataTextField = "Descripcion";
                    dropListNivel1_ModalBusqueda.DataValueField = "Id";
                    dropListNivel1_ModalBusqueda.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarNivelesDropList_Busqueda_ConElItemTODOS()
        {
            try
            {
                DropDownList[] DropList_Niveles_Busqueda = { dropListNivel2_ModalBusqueda, dropListNivel3_ModalBusqueda, dropListNivel4_ModalBusqueda, dropListNivel5_ModalBusqueda};

                foreach (var item in DropList_Niveles_Busqueda)
                {
                    item.Items.Add(new ListItem
                    {
                        Value = "0",
                        Text = "Todos"
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CargarNivelesDeLosDropDown()
        {
            try
            {
                DropDownList[] ddls = { DropListNivel1_ModalAgregarRegistro, DropListNivel2_ModalAgregarRegistro, DropListNivel3_ModalAgregarRegistro, DropListNivel4_ModalAgregarRegistro, DropListNivel5_ModalAgregarRegistro };
                List<Cuentas_Contables> lista = new List<Cuentas_Contables>();

                for (int i = 0; i < ddls.Length; i++)
                {
                    if (i == 0)
                    {
                        lista = contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);
                    }
                    if (lista != null)
                    {
                        ddls[i].DataSource = lista;
                        ddls[i].DataTextField = "Descripcion";
                        ddls[i].DataValueField = "Id";
                        ddls[i].DataBind();
                    }
                    lista = contPlanCuentas.obtenerCuentasContablesByNivel(i + 2, Convert.ToInt32(ddls[i].SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        [WebMethod]
        public static string ObtenerSucursalesDependiendoDeLaEmpresa(int empresa)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();

            DataTable dt = null;

            if (empresa > 0)
                dt = controladorSucursal.obtenerSucursalesDT(empresa);
            else
                dt = controladorSucursal.obtenerSucursales();

            List<ListItemTemporal> listaTemporal = new List<ListItemTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                ListItemTemporal listItemTemporal = new ListItemTemporal();
                listItemTemporal.id = row["id"].ToString();
                listItemTemporal.nombre = row["nombre"].ToString();
                listaTemporal.Add(listItemTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(listaTemporal);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerPuntosDeVentaDependiendoDeLaSucursal(int sucursal)
        {
            controladorSucursal controladorSucursal = new controladorSucursal();

            DataTable dt = null;

            if (sucursal > 0)
                dt = controladorSucursal.obtenerPuntoVentaDT(sucursal);
            else
                dt = controladorSucursal.obtenerPuntoVenta();

            List<ListItemTemporal> puntosVenta = new List<ListItemTemporal>();

            foreach (DataRow row in dt.Rows)
            {
                ListItemTemporal puntoVentaTemporal = new ListItemTemporal();
                puntoVentaTemporal.id = row["Id"].ToString();
                puntoVentaTemporal.nombre = row["NombreFantasia"].ToString();
                puntosVenta.Add(puntoVentaTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(puntosVenta);
            return resultadoJSON;
        }
        #endregion

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta, int idTipoMovimiento, int idEmpresa, int idSucursal, int idPuntoVenta)
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                controladorSucursal contSucursal = new controladorSucursal();
                ControladorEmpresa contEmpresa = new ControladorEmpresa();

                List<Mayor> listaMayor = contPlanCuentas.ObtenerTodosMayor(fechaDesde, fechaHasta.AddHours(23).AddMinutes(59), idTipoMovimiento, idEmpresa, idSucursal, idPuntoVenta);
                List<MayorTemporal> listaMayorTemporal = new List<MayorTemporal>();
                foreach (var item in listaMayor)
                {
                    
                    listaMayorTemporal.Add(new MayorTemporal
                    {
                        Fecha = item.Fecha.Value.ToString("dd/MM/yyyy"),
                        TipoMovimiento = item.Mayor_TipoMovimiento.TipoMovimiento,
                        Id = item.Id.ToString(),
                        Empresa = contEmpresa.obtenerEmpresa((int)item.Empresa).RazonSocial.ToString(),
                        Sucursal = item.sucursale.nombre,
                        PuntoDeVenta = item.PuntoVta.PtoVenta,
                        Nivel1 = item.Cuentas_Contables.Descripcion,
                        Nivel2 = item.Cuentas_Contables1.Descripcion,
                        Nivel3 = item.Cuentas_Contables2.Descripcion,
                        Nivel4 = item.Cuentas_Contables3.Descripcion,
                        Nivel5 = item.Cuentas_Contables4.Descripcion,
                        Debito = item.Debito.ToString(),
                        Credito = item.Credito.ToString(),
                        NumeroDocumento = item.NumeroDocumento,
                        Observaciones = item.Observaciones ?? ""

                    });;
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaMayorTemporal);
                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [WebMethod]
        public static string ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel(int jerarquia, int nivel)
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                var listaCuentas = contPlanCuentas.obtenerCuentasContablesByNivel(jerarquia, nivel);

                List<ListItemTemporal> listaCuentasTemporal = new List<ListItemTemporal>();

                foreach (var item in listaCuentas)
                {
                    listaCuentasTemporal.Add(new ListItemTemporal
                    {
                        id = item.Id.ToString(),
                        nombre = item.Descripcion
                    });
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaCuentasTemporal);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void InsertarRegistroEnLaTablaMayor_JSON(string[] objetos)
        {
            try
            {
                int idUser = (int)HttpContext.Current.Session["Login_IdUser"];

                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                controladorSucursal contSucursal = new controladorSucursal();

                for (int i = 0; i < objetos.Length; i++)
                {
                    var linea = objetos[i].Split('_');

                    string fecha = linea[0].ToString();
                    string[] fechaArray = fecha.Split('/');
                    string nuevaFecha = fechaArray[1] + "/" + fechaArray[0] + "/" + fechaArray[2] + " 00:00";
                    DateTime fechaDateTime = Convert.ToDateTime(nuevaFecha);

                    decimal debe = Convert.ToDecimal(linea[1]);
                    decimal haber = Convert.ToDecimal(linea[2]);
                    int idEmpresa = Convert.ToInt32(linea[3]);
                    int idSucursal = Convert.ToInt32(linea[4]);
                    int idPuntoDeVenta = Convert.ToInt32(linea[5]);
                    int idNivel1 = Convert.ToInt32(linea[6]);
                    int idNivel2 = Convert.ToInt32(linea[7]);
                    int idNivel3 = Convert.ToInt32(linea[8]);
                    int idNivel4 = Convert.ToInt32(linea[9]);
                    int idNivel5 = Convert.ToInt32(linea[10]);
                    string observaciones = linea[11].ToString();

                    string puntoDeVenta = contSucursal.obtenerPtoVentaEntityID(idPuntoDeVenta).NombreFantasia;
                    string numeroDocumento = "0000-" + contPlanCuentas.ObtenerCantidadDeRegistrosCreadosDeLaTabla_MayorFiltradoPorTipoDeMayor(1).ToString().PadLeft(8,'0');

                    contPlanCuentas.AgregarRegistroToTableMayor(new Mayor
                    {
                        Observaciones = observaciones,
                        Fecha = fechaDateTime,
                        Debito = debe,
                        Credito = haber,
                        Empresa = idEmpresa,
                        Sucursal = idSucursal,
                        PuntoDeVenta = idPuntoDeVenta,
                        Usuario = idUser,
                        TipoMovimiento = 1,//Manual
                        Estado = 1,
                        NumeroDocumento = numeroDocumento,
                        Nivel1 = idNivel1,
                        Nivel2 = idNivel2,
                        Nivel3 = idNivel3,
                        Nivel4 = idNivel4,
                        Nivel5 = idNivel5
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnCrearRegistroManual_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = Convert.ToDateTime(txtFecha_ModalAgregarRegistro.Text, new CultureInfo("es-AR"));

                int idEmpresa = Convert.ToInt32(DropListEmpresa_ModalAgregarRegistro.SelectedValue);
                int idSucursal = Convert.ToInt32(DropListSucursal_ModalAgregarRegistro.SelectedValue);
                var puntoVenta = contSucursal.obtenerPtoVentaId(idSucursal);
                int idPuntoVenta = Convert.ToInt32(dropListPuntoVenta_ModalAgregarRegistro.SelectedValue);

                var cuenta_contable = contPlanCuentas.obtenerCuentaById(Convert.ToInt32(DropListNivel4_ModalAgregarRegistro.SelectedValue));

                decimal importe = Convert.ToDecimal(txtImporte_ModalAgregarRegistro.Text);

                int cantidadRegistrosCreadosFiltradoPorTipo = contPlanCuentas.ObtenerCantidadDeRegistrosCreadosDeLaTabla_MayorFiltradoPorTipoDeMayor(1) + 1;

                string numeroDocumento = puntoVenta.puntoVenta + "-" + cantidadRegistrosCreadosFiltradoPorTipo.ToString().PadLeft(8, '0');

                bool tipoOperacionIngreso = false;
                if (dropList_TipoOperacion_ModalAgregarRegistro.SelectedValue == "1")
                {
                    tipoOperacionIngreso = true;
                }

                Mayor mayor = new Mayor
                {
                    Fecha = fecha,
                    Empresa = idEmpresa,
                    Sucursal = idSucursal,
                    PuntoDeVenta = idPuntoVenta,
                    Nivel1 = cuenta_contable.Nivel1,
                    Nivel2 = cuenta_contable.Nivel2,
                    Nivel3 = cuenta_contable.Nivel3,
                    Nivel4 = cuenta_contable.Nivel4,
                    Nivel5 = cuenta_contable.Id,
                    Usuario = (int)Session["Login_IdUser"],
                    TipoMovimiento = 1,//Manual
                    NumeroDocumento = numeroDocumento,
                    Estado = 1
                };
                mayor.Debito = 0;
                mayor.Credito = 0;
                if (tipoOperacionIngreso)
                {
                    mayor.Credito = importe;
                }
                else
                {
                    mayor.Debito = importe;
                }
                if (contPlanCuentas.AgregarRegistroToTableMayor(mayor))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Registro creado correctamente", ""));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo crear el registro"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }
    }
}