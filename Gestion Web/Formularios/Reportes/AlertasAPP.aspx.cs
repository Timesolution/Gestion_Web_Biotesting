using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestion_Api.Controladores.APP;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Globalization;
using Gestion_Api.Entitys;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class AlertasAPP : System.Web.UI.Page
    {
        Mensajes _m = new Mensajes();

        controladorCliente _controladorCliente = new controladorCliente();        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();

                if (!IsPostBack)
                {
                    CargarClientes();
                    CargarVendedores();
                    CargarTiposAlertas();
                    CargarEstadosAlertas();
                }                    
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar alertasAPP. " + ex.Message);
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

                    }
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        void CargarClientes()
        {
            try
            {
                DataTable dt = _controladorCliente.obtenerClientesDT();

                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropListCliente.DataSource = dt;
                DropListCliente.DataValueField = "id";
                DropListCliente.DataTextField = "alias";

                DropListCliente.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando clientes. " + ex.Message));
            }
        }

        void CargarVendedores()
        {
            try
            {
                controladorVendedor _controladorVendedor = new controladorVendedor();
                DataTable dt = _controladorVendedor.obtenerVendedores();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DropListVendedor.DataSource = dt;
                DropListVendedor.DataValueField = "id";
                DropListVendedor.DataTextField = "nombre";

                DropListVendedor.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando vendedores. " + ex.Message));
            }
        }

        void CargarTiposAlertas()
        {
            try
            {
                ControladorAlertaAPP _controladorAlertaAPP = new ControladorAlertaAPP();
                var tiposAlerta = _controladorAlertaAPP.ObtenerTiposAlerta();

                AlertasAPP_Tipo tiposTodos = new AlertasAPP_Tipo();
                tiposTodos.Id = -1;
                tiposTodos.Tipo = "Todos";

                tiposAlerta.Insert(0, tiposTodos);

                DropListTipoAlerta.DataSource = tiposAlerta;
                DropListTipoAlerta.DataValueField = "Id";
                DropListTipoAlerta.DataTextField = "Tipo";

                DropListTipoAlerta.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando tipos alerta. " + ex.Message));
            }
        }

        void CargarEstadosAlertas()
        {
            try
            {
                ControladorAlertaAPP _controladorAlertaAPP = new ControladorAlertaAPP();
                var estadosAlerta = _controladorAlertaAPP.ObtenerEstadosAlerta();

                AlertasAPP_Estados estadoTodos = new AlertasAPP_Estados();
                estadoTodos.Id = -1;
                estadoTodos.Estado = "Todos";

                estadosAlerta.Insert(0, estadoTodos);

                DropListEstadoAlerta.DataSource = estadosAlerta;
                DropListEstadoAlerta.DataValueField = "Id";
                DropListEstadoAlerta.DataTextField = "Estado";

                DropListEstadoAlerta.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", _m.mensajeBoxError("Error cargando estados alerta. " + ex.Message));
            }
        }

        [WebMethod]
        public static string ObtenerCliente(string cliente)
        {
            controladorCliente controladorCliente = new controladorCliente();
            string buscar = cliente.Replace(' ', '%');
            DataTable dtClientes = controladorCliente.obtenerClientesAliasDT(buscar);

            List<ClienteTemporal> clientes = new List<ClienteTemporal>();

            foreach (DataRow row in dtClientes.Rows)
            {
                ClienteTemporal clienteTemporal = new ClienteTemporal();
                clienteTemporal.id = row["id"].ToString();
                clienteTemporal.alias = row["alias"].ToString();
                clientes.Add(clienteTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(clientes);
            return resultadoJSON;
        }

        [WebMethod]
        public static string ObtenerVendedor(string vendedor)
        {
            controladorVendedor controladorVendedor = new controladorVendedor();
            string buscar = vendedor.Replace(' ', '%');
            var vendedores = controladorVendedor.obtenerVendedoresNombre(buscar);

            List<VendedorTemporal> vendedoresTemporal = new List<VendedorTemporal>();

            foreach (var vendedorTemp in vendedores)
            {
                VendedorTemporal vendedorTemporal = new VendedorTemporal();
                vendedorTemporal.id = vendedorTemp.id.ToString();
                vendedorTemporal.nombre = vendedorTemp.emp.nombre;
                vendedoresTemporal.Add(vendedorTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultadoJSON = serializer.Serialize(vendedoresTemporal);
            return resultadoJSON;
        }

        [WebMethod]
        public static string Filtrar(DateTime fechaDesde, DateTime fechaHasta, int idCliente, int idVendedor, int tipoAlerta, int estado)
        {
            ControladorAlertaAPP controladorAlertaAPP = new ControladorAlertaAPP();
            controladorCliente controladorCliente = new controladorCliente();
            controladorVendedor controladorVendedor = new controladorVendedor();

            var alertas = controladorAlertaAPP.ObtenerAlertas(fechaDesde, fechaHasta, idCliente, idVendedor, tipoAlerta, estado);

            List<AlertaTemporal> alertasTemporales = new List<AlertaTemporal>();

            foreach (var alerta in alertas)
            {
                AlertaTemporal alertaTemporal = new AlertaTemporal();
                alertaTemporal.id = alerta.Id.ToString();
                alertaTemporal.fecha = Convert.ToDateTime(alerta.Fecha, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy hh:mm");
                alertaTemporal.cliente = controladorCliente.obtenerClienteID((int)alerta.IdCliente).alias;
                alertaTemporal.vendedor = controladorVendedor.obtenerVendedorID((int)alerta.IdVendedor).emp.nombre;
                alertaTemporal.tipoAlerta = controladorAlertaAPP.ObtenerTiposAlerta((int)alerta.IdTipoAlerta).FirstOrDefault().Tipo;
                alertaTemporal.mensaje = alerta.Mensaje;
                alertaTemporal.estado = controladorAlertaAPP.ObtenerEstadosAlerta((int)alerta.Estado).FirstOrDefault().Estado;
                alertaTemporal.vencimiento = CalcularProgressBar(Convert.ToDateTime(alerta.Fecha, CultureInfo.InvariantCulture), 30).ToString();
                alertasTemporales.Add(alertaTemporal);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string resultadoJSON = serializer.Serialize(alertasTemporales);
            return resultadoJSON;
        }

        [WebMethod]
        public static void CambiarEstadoAlertas(string idsAlertas)
        {
            var ids = idsAlertas.Split(';').ToList();
            ControladorAlertaAPP controladorAlertaAPP = new ControladorAlertaAPP();
            var alertas = controladorAlertaAPP.ObtenerAlertasPorID(ids);

            foreach (var alerta in alertas)
            {
                alerta.Estado = 2;
            }

            controladorAlertaAPP.ModificarAlertas(alertas);
        }

        static int CalcularProgressBar(DateTime fechaAlerta, int plazoMaximo)
        {
            try
            {
                return (Convert.ToInt32((DateTime.Today - fechaAlerta).TotalDays) * 100) / plazoMaximo;
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al calcular la progress bar " + ex.Message);
                return -1;
            }
        }

        class AlertaTemporal
        {
            public string id;
            public string fecha;
            public string cliente;
            public string vendedor;
            public string tipoAlerta;
            public string mensaje;
            public string estado;
            public string vencimiento;
        }

        class ClienteTemporal
        {
            public string id;
            public string alias;
        }

        class VendedorTemporal
        {
            public string id;
            public string nombre;
        }
    }
}