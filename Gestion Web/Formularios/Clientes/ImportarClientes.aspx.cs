using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ImportarClientes : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        Configuracion config = new Configuracion();
        controladorCliente contCliente = new controladorCliente();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        ControladorProvincias contProvincias = new ControladorProvincias();
        controladorVendedor contVendedor = new controladorVendedor();
        ControladorVendedorEntity contVendedorEntity = new ControladorVendedorEntity();
        controladorGrupoCliente contGrupoCliente = new controladorGrupoCliente();
        controladorDireccion contDireccion = new controladorDireccion();
        controladorUsuario contUsuario = new controladorUsuario();
        ControladorEmpresa contEmpresa = new ControladorEmpresa();
        controladorSucursal contSucursal = new controladorSucursal();
        controladorEmpleado contEmpleado = new controladorEmpleado();
        controladorTipoCliente contTipoCliente = new controladorTipoCliente();
        controladorEstadoCliente contEstadoCliente = new controladorEstadoCliente();

        class ClienteTemporalSpeed
        {
            public string Direccion { get; set; }
            public string Altura { get; set; }
            public string Localidad { get; set; }
            public string Canal { get; set; }
            public string AgrupCanal { get; set; }
        }

        class ClienteTemporalGestion
        {
            public string Apellido { get; set; }
            public string Nombre { get; set; }
            public string FechaNacimiento { get; set; }
            public string Provincia { get; set; }
            public string Telefono { get; set; }
            public string CUIT { get; set; }
            public string Grupo { get; set; }
            public string Estado { get; set; }
            public string Proveedor { get; set; }
            public string Vendedor { get; set; }
        }

        public class CuitsEstados
        {
            public string Cuit { get; set; }
            public string Estado { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProvinciasEnLista();
                CargarVendedoresEnLista();
            }
        }

        private void CargarVendedoresEnLista()
        {
            try
            {
                var dtVendedores = contVendedor.obtenerVendedores();

                if (dtVendedores != null && dtVendedores.Rows.Count > 0)
                {
                    dropList_Vendedores.DataSource = dtVendedores;
                    dropList_Vendedores.DataValueField = "id";
                    dropList_Vendedores.DataTextField = "apellido";
                    dropList_Vendedores.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarProvinciasEnLista()
        {
            try
            {
                var listProvincias = contProvincias.ObtenerProvincias().OrderBy(x => x.Provincia1).ToList();

                if (listProvincias != null)
                {
                    if (listProvincias.Count > 1)
                    {
                        dropList_Provincias.DataSource = listProvincias;
                        dropList_Provincias.DataValueField = "Id";
                        dropList_Provincias.DataTextField = "Provincia1";
                        dropList_Provincias.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnImportarUsuarios_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;
                    int contador = 0;
                    sr.ReadLine();//para saltar la primer linea

                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(';');//obtengo datos del registro
                        ClienteTemporalSpeed clienteTemporal = new ClienteTemporalSpeed();
                        if (datos.Count() >= 4)
                        {
                            List<string> datosExcel = datos.ToList();
                            clienteTemporal.Direccion = datos[0].Trim();
                            clienteTemporal.Altura = datos[1].Trim();
                            clienteTemporal.Localidad = datos[2].Trim();
                            clienteTemporal.Canal = datos[3].Trim();
                            clienteTemporal.AgrupCanal = datos[4].Trim();

                            int respuesta = ImportarCliente(clienteTemporal);
                            if (respuesta <= 0)
                            {
                                contador++;
                            }
                        }
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista importada correctamente", null));
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnImportarClientes_Click(object sender, EventArgs e)
        {
            try
            {
                List<CuitsEstados> listaCuits = new List<CuitsEstados>();

                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;

                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(';');//obtengo datos del registro
                        ClienteTemporalGestion clienteTemporal = new ClienteTemporalGestion();
                        if (datos.Count() >= 4)
                        {
                            List<string> datosExcel = datos.ToList();
                            clienteTemporal.Apellido = datos[1].Trim();
                            clienteTemporal.Nombre = datos[2].Trim();
                            clienteTemporal.FechaNacimiento = datos[4].Trim();
                            clienteTemporal.Provincia = datos[5].Trim();
                            clienteTemporal.Telefono = datos[7].Trim();
                            clienteTemporal.Proveedor = datos[8].Trim();
                            clienteTemporal.CUIT = datos[11].Trim();
                            clienteTemporal.Vendedor = datos[10].Trim();
                            clienteTemporal.Grupo = datos[12].Trim();
                            clienteTemporal.Estado = datos[14].Trim();

                            if (VerificarQueEstenTodosLosCamposBienCompletados(datos))
                            {
                                int respuesta = ImportarClienteGestion(clienteTemporal);
                                switch (respuesta)
                                {
                                    case 1:
                                        listaCuits.Add(new CuitsEstados
                                        {
                                            Cuit = clienteTemporal.CUIT,
                                            Estado = "Importado"
                                        });
                                        break;
                                    case 0:
                                        listaCuits.Add(new CuitsEstados
                                        {
                                            Cuit = clienteTemporal.CUIT,
                                            Estado = "Modificado"
                                        });
                                        break;
                                    case -1:
                                        listaCuits.Add(new CuitsEstados
                                        {
                                            Cuit = clienteTemporal.CUIT,
                                            Estado = "No_Importado"
                                        });
                                        break;
                                }
                            }
                        }
                    }
                    ExportarInformeDelResultado(listaCuits);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista importada correctamente", null));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool VerificarQueEstenTodosLosCamposBienCompletados(string[] datos)
        {
            try
            {
                for (int i = 0; i < 14; i++)
                {
                    if (string.IsNullOrWhiteSpace(datos[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int ImportarCliente(ClienteTemporalSpeed clienteTemporal)
        {
            try
            {
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cli = new Cliente();
                    Session.Add("ClientesABM_Cliente", cli);
                }
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                string perfil = Session["Login_NombrePerfil"] as string;

                cliente.codigo = (contClienteEntity.ObtenerUltimoIdCliente() + 1).ToString();
                cliente.tipoCliente.id = 4; //CONSUMIDOR FINAL
                cliente.tipoCliente.descripcion = "CONSUMIDOR FINAL";
                cliente.razonSocial = cliente.codigo;

                CrearElGrupoSiNoExiste(clienteTemporal.Canal);

                cliente.grupo.id = contGrupoCliente.obtenerGrupoDesc(clienteTemporal.Canal).id;
                cliente.categoria.id = 1;
                cliente.estado.id = 1;
                cliente.cuit = "00000000000";
                cliente.iva = "13";
                cliente.pais.id = 1;//ARGENTINA
                cliente.expreso.id = 1;
                string saldMax = "0";
                cliente.saldoMax = Convert.ToDecimal(saldMax);
                cliente.vencFC = 0;
                cliente.descFC = 0;
                cliente.observaciones = "";

                //alerta cliente                
                cliente.alerta.descripcion = "";
                cliente.alerta.idCliente = cliente.id;

                cliente.hijoDe = 0;
                cliente.alias = cliente.codigo;

                Vendedor vendedor = contVendedor.obtenerVendedorID(Convert.ToInt32(dropList_Vendedores.SelectedValue));
                cliente.sucursal.id = vendedor.sucursal;//preguntar

                cliente.vendedor.id = vendedor.id;
                cliente.lisPrecio.id = 1;
                cliente.formaPago.id = 1;//CONTADO

                string codigoPostal = ObtenerCodigoPostalByLocalidad(dropList_Provincias.SelectedItem.Text, clienteTemporal.Localidad);

                cliente.direcciones = obtenerListDirecciones(clienteTemporal, codigoPostal);

                cliente.origen = 1;

                if (CrearElClienteSiNoExiste(cliente) > 0)
                {
                    CrearUsuarioAlCliente(clienteTemporal, cliente);
                }

                return 1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando cliente. " + ex.Message));
                return 0;
            }
        }

        private int ImportarClienteGestion(ClienteTemporalGestion clienteTemporal)
        {
            try
            {
                if (Session["ClientesABM_Cliente"] == null)
                {
                    Cliente cli = new Cliente();
                    Session.Add("ClientesABM_Cliente", cli);
                }
                Cliente cliente = Session["ClientesABM_Cliente"] as Cliente;
                string perfil = Session["Login_NombrePerfil"] as string;

                cliente.codigo = (contClienteEntity.ObtenerUltimoIdCliente() + 1).ToString();
                if (cliente.codigo == "0")
                {
                    cliente.codigo = "1";
                }

                int idTipoCliente = CrearYObtenerTipoClienteSiNoExiste(clienteTemporal.Proveedor);

                cliente.tipoCliente.id = idTipoCliente;
                cliente.tipoCliente.descripcion = clienteTemporal.Proveedor;

                cliente.razonSocial = clienteTemporal.CUIT;

                CrearElGrupoSiNoExiste(clienteTemporal.Grupo);

                cliente.grupo.id = contGrupoCliente.obtenerGrupoDesc(clienteTemporal.Grupo).id;
                cliente.categoria.id = 1;
                cliente.estado.id = 1;
                cliente.cuit = clienteTemporal.CUIT;
                cliente.iva = "13";
                cliente.pais.id = 1;//ARGENTINA
                cliente.expreso.id = 1;
                string saldMax = "0";
                cliente.saldoMax = Convert.ToDecimal(saldMax);
                cliente.vencFC = 0;
                cliente.descFC = 0;
                cliente.observaciones = "";

                //alerta cliente                
                cliente.alerta.descripcion = "";
                cliente.alerta.idCliente = cliente.id;

                cliente.hijoDe = 0;
                cliente.alias = clienteTemporal.Apellido + " " + clienteTemporal.Nombre;

                //Vendedor vendedor = contVendedor.obtenerVendedorID(Convert.ToInt32(dropList_Vendedores.SelectedValue));

                cliente.vencFC = 20;

                //cliente.vendedor.id = vendedor.id;
                cliente.lisPrecio.id = 1;
                cliente.formaPago.id = 1;//CONTADO

                //crear direccion por defecto
                List<direccion> direcciones = new List<direccion>();
                Provincia provincia = contProvincias.ObtenerProvinciaByNombre(clienteTemporal.Provincia.ToUpper());
                direcciones.Add(new direccion
                {
                    codPostal = "0000",
                    direc = "Sin direccion",
                    localidad = "Sin localidad",
                    nombre = "Sin nombre",
                    pais = "Argentina",
                    provincia = provincia.Provincia1
                });
                cliente.direcciones = direcciones;

                cliente.origen = 1;

                string cel = clienteTemporal.Telefono.Substring(clienteTemporal.Telefono.Length - 10, 10);
                cel = cel.Insert(2, "-");

                var Vendedor = CrearVendedorSiNoExiste(clienteTemporal);
                cliente.vendedor.id = Vendedor.id;
                cliente.sucursal.id = Vendedor.sucursal;//preguntar
                cliente.estado.id = CrearElClienteEstadoSiNoExiste(clienteTemporal.Estado);

                //crea
                if (!contClienteEntity.ExisteClienteConEsteCUITEnElCampoRazonSocial(clienteTemporal.CUIT))
                {
                    if (contCliente.agregarCliente(cliente) > 0)
                    {
                        if (contClienteEntity.agregarClienteDatos(new Cliente_Datos
                        {
                            IdCliente = cliente.id,
                            Celular = cel
                        }) > 0)
                        {
                            return 1;
                        }
                    }
                    return -1;
                }

                //actualiza
                int idClienteAModificar = contCliente.obtenerClienteCuit(clienteTemporal.CUIT).id;
                cliente.id = idClienteAModificar;
                if (contCliente.modificarCliente(cliente, clienteTemporal.CUIT, cliente.codigo) > 0)
                {
                    int idClienteDatos = (int)contClienteEntity.obtenerClienteDatosByIdCliente(cliente.id).Id;
                    if (contClienteEntity.modificarClienteDatos(new Cliente_Datos
                    {
                        Id = idClienteDatos,
                        IdCliente = cliente.id,
                        Celular = cel
                    }) >= 0)
                    {
                        return 0;
                    }
                    return -1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando cliente. " + ex.Message));
                return -1;
            }
        }

        int CrearElClienteEstadoSiNoExiste(string estadoCliente)
        {
            try
            {
                var estado = contEstadoCliente.obtenerEstadoDesc(estadoCliente);
                if (estado.id == 0)
                {
                    estado.id = contEstadoCliente.agregarEstadoCliente(estadoCliente);
                }
                return estado.id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        void CrearElGrupoSiNoExiste(string nombreDelGrupo)
        {
            try
            {
                string grupoDB = contGrupoCliente.obtenerGrupoDesc(nombreDelGrupo).descripcion;
                if (grupoDB != nombreDelGrupo)
                {
                    int i = contGrupoCliente.agregarGrupo(nombreDelGrupo);
                }
            }
            catch (Exception ex)
            {

            }
        }

        string ObtenerCodigoPostalByLocalidad(string provincia, string localidad)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerCodPostalByLocalidadProvincia(provincia, localidad);
                foreach (DataRow dr in dt.Rows)
                {
                    return dr[0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private List<direccion> obtenerListDirecciones(ClienteTemporalSpeed clienteTemporal, string codigoPostal)
        {
            try
            {
                List<direccion> direcciones = new List<direccion>();
                direcciones.Add(new direccion
                {
                    nombre = "Legal",
                    localidad = clienteTemporal.Localidad,
                    pais = "Argentina",
                    direc = clienteTemporal.Direccion + " " + clienteTemporal.Altura,
                    provincia = dropList_Provincias.SelectedItem.Text,
                    codPostal = codigoPostal
                });
                return direcciones;
            }
            catch
            {
                return null;
            }
        }

        int CrearElClienteSiNoExiste(Cliente cliente)
        {
            try
            {
                int respuesta = 0;
                var direccion = cliente.direcciones.FirstOrDefault();
                var direccionDB = contDireccion.obtenerDireccionCompleta(direccion.codPostal, direccion.direc, direccion.localidad, direccion.provincia, direccion.pais);
                if (direccionDB == null)
                {
                    respuesta = contCliente.agregarCliente(cliente);
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        void CrearUsuarioAlCliente(ClienteTemporalSpeed clienteTemporal, Cliente cliente)
        {
            try
            {
                var perfil = contUsuario.obtenerPerfilDesc("Cliente");
                var empresa = contEmpresa.obtenerEmpresaByIdSucursal(cliente.sucursal.id);

                var puntoDeVenta = contSucursal.obtenerPtoVentaSucursal(cliente.sucursal.id);

                Vendedor vendedorCliente = new Vendedor();
                vendedorCliente.id = cliente.id;

                Usuario usuario = new Usuario();
                usuario.usuario = clienteTemporal.Direccion.Trim();//chequear q no haya espacios.
                usuario.contraseña = cliente.direcciones.FirstOrDefault().codPostal + clienteTemporal.Altura;
                usuario.sucursal = cliente.sucursal;
                usuario.perfil = perfil;//CLIENTE
                usuario.empresa = empresa;
                usuario.estado = 1;
                usuario.vendedor = vendedorCliente;
                usuario.ptoVenta = puntoDeVenta.FirstOrDefault();
                contUsuario.agregarUsuarios(usuario);
            }
            catch (Exception ex)
            {

            }
        }

        private Vendedor CrearVendedorSiNoExiste(ClienteTemporalGestion clienteTemporalGestion)
        {
            try
            {
                var vendedor = contVendedorEntity.ObtenerVendedorByNombre(clienteTemporalGestion.Vendedor);
                Vendedor ven = new Vendedor();
                if (vendedor == null)
                {
                    Gestion_Api.Modelo.Empleado emp = new Gestion_Api.Modelo.Empleado();

                    if (contClienteEntity.ObtenerUltimoIdCliente() == -1)
                    {
                        emp.legajo = 1;
                    }
                    else
                    {
                        emp.legajo = contClienteEntity.ObtenerUltimoIdCliente() + 1;
                    }
                    emp.nombre = clienteTemporalGestion.Vendedor;
                    emp.apellido = "";
                    emp.direccion = "";
                    emp.dni = "00000000";
                    emp.fecNacimiento = DateTime.Now;
                    emp.fecIngreso = DateTime.Now;
                    emp.cuitCuil = "00000000000";
                    emp.observaciones = "";
                    emp.remuneracion = 0;

                    int e = this.contEmpleado.agregarEmpleado(emp);

                    Gestion_Api.Modelo.Empleado empDB = this.contEmpleado.obtenerEmpleadoByNombre(emp.nombre);
                    ven.emp = empDB;
                    ven.comision = 0;
                    ven.sucursal = contSucursal.obtenerSucursalesList().FirstOrDefault().id;

                    ven.id = this.contVendedor.agregarVendedor(ven);
                }
                else
                {
                    ven.id = vendedor.id;
                    ven.sucursal = (int)vendedor.sucursal;
                }
                return ven;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Empleado" + ex.Message));
                return null;
            }
        }

        public int CrearYObtenerTipoClienteSiNoExiste(string tipoClienteString)
        {
            try
            {
                var tipoCliente = contTipoCliente.obtenerTipoDesc(tipoClienteString);
                if (tipoCliente.id == 0)
                {
                    tipoCliente.id = contTipoCliente.agregarTipoCliente(tipoClienteString);
                }
                return tipoCliente.id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void ExportarInformeDelResultado(List<CuitsEstados> listaCuits)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Cuit");
                dt.Columns.Add("Estado");

                foreach (var item in listaCuits)
                {
                    DataRow dr = dt.NewRow();

                    dr["Cuit"] = item.Cuit;
                    dr["Estado"] = item.Estado;
                    dt.Rows.Add(dr);
                }

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("InformeImportarClientes.rdlc");

                ReportDataSource rds = new ReportDataSource("dsInformeImportacionClientes", dt);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                //get xls content
                Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                String filename = string.Format("{0}.{1}", "Informe_Importacion_Clientes", "xls");

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                this.Response.BinaryWrite(xlsContent);

                this.Response.End();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al imprimir detalle de ventas generarReporte15. " + ex.Message));
            }
        }
    }
}
