using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Millas_Api.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Task_Api;
using Task_Api.Entitys;

namespace Gestion_Web.Formularios.Clientes
{
    public partial class ProspectosABM : System.Web.UI.Page
    {
        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private int IdProspecto;
        protected void Page_Load(object sender, EventArgs e)
        {

            this.IdProspecto = Convert.ToInt32(Request.QueryString["id"]);
            this.accion = Convert.ToInt32(Request.QueryString["a"]);

            if (!IsPostBack)
            {
                CargarTipoDocumento();
                CargarEstadoCivil();
                CargarEstudiosAlcanzados();
                CargartipoVivienda();
                CargarCondicionIVA();
                CargarProvincias();
                CargarClientes();

                if (accion == 2 && IdProspecto > 0)
                {
                    CargarProspecto(IdProspecto);

                }

            }
            if (accion == 2 && IdProspecto > 0)
            {
                CargarTablaDireccion();
                CargarDireccionRecepcion();
            }

            CargarTablaDireccion();

        }


        #region Carga Inicial

        public void CargarDireccionRecepcion()
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();

                int idprospecto = 0;

                if (ViewState["idDireccion"] != null)
                {
                    idprospecto = (int)ViewState["idDireccion"];
                }
                else if (this.IdProspecto > 0)
                {
                    idprospecto = this.IdProspecto;
                }

                List<Prospecto_Direccion> prospecto = controladorProspectos.ObtenerDireccionRecepcionByIdProspecto(idprospecto);

                this.ListRecepcionDireccion.DataSource = prospecto;
                this.ListRecepcionDireccion.DataValueField = "Id";
                this.ListRecepcionDireccion.DataTextField = "Calle";
                this.ListRecepcionDireccion.DataBind();

                UpdatePanel4.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanel4.Update();

                UpdatePanel4.UpdateMode = UpdatePanelUpdateMode.Always;
            }
            catch (Exception ex)
            {

            }
        }

        public void CargarTipoDocumento()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("DNI", "1"));
            items.Add(new ListItem("CI", "2"));
            items.Add(new ListItem("LC", "3"));
            ListTipoDocumento.Items.AddRange(items.ToArray());
            ListTiposDocumentosGarante.Items.AddRange(items.ToArray());

        }
        public void CargarEstadoCivil()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Soltero/a", "1"));
            items.Add(new ListItem("Casado/a", "2"));
            items.Add(new ListItem("Viudo/a", "3"));
            ListEstadoCivil.Items.AddRange(items.ToArray());
            ListEstadoCivilGarante.Items.AddRange(items.ToArray());
        }

        public void CargarEstudiosAlcanzados()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Primario", "1"));
            items.Add(new ListItem("Secundario", "2"));
            items.Add(new ListItem("Terciario/Universitario", "3"));
            items.Add(new ListItem("Posgrado", "4"));
            ListEstudiosAlcanzados.Items.AddRange(items.ToArray());
        }

        public void CargartipoVivienda()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Departamento", "1"));
            items.Add(new ListItem("Casa", "2"));
            items.Add(new ListItem("Terreno", "3"));
            ListTipoVivienda.Items.AddRange(items.ToArray());
            ListTiposViviendasGarante.Items.AddRange(items.ToArray());
        }

        public void CargarCondicionIVA()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Resp.Inscripto", "1"));
            items.Add(new ListItem("Monotributista", "2"));
            items.Add(new ListItem("Sij.NoCategoria", "3"));
            listCondicionIVA.Items.AddRange(items.ToArray());
        }

        private void CargarProvincias()
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListProvincia.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvincia.DataValueField = "Provincia";
                this.ListProvincia.DataTextField = "Provincia";
                this.ListProvincia.DataBind();
                this.ListProvincia.Items.Insert(0, new ListItem("Seleccione", "-1"));

                this.ListProvinciaPatrimonial.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvinciaPatrimonial.DataValueField = "Provincia";
                this.ListProvinciaPatrimonial.DataTextField = "Provincia";
                this.ListProvinciaPatrimonial.DataBind();
                this.ListProvinciaPatrimonial.Items.Insert(0, new ListItem("Seleccione", "-1"));

                this.ListProvinciaGarantes.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvinciaGarantes.DataValueField = "Provincia";
                this.ListProvinciaGarantes.DataTextField = "Provincia";
                this.ListProvinciaGarantes.DataBind();
                this.ListProvinciaGarantes.Items.Insert(0, new ListItem("Seleccione", "-1"));

                this.ListProvinciaGarantePatrimoniales.DataSource = controladorPais.obtenerPRovincias();
                this.ListProvinciaGarantePatrimoniales.DataValueField = "Provincia";
                this.ListProvinciaGarantePatrimoniales.DataTextField = "Provincia";
                this.ListProvinciaGarantePatrimoniales.DataBind();
                this.ListProvinciaGarantePatrimoniales.Items.Insert(0, new ListItem("Seleccione", "-1"));

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  provincias. " + ex.Message + "')", true);
            }
        }


        #endregion


        public void CargarProspecto(int IdProspecto)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();

                var pro = controladorProspectos.ObtenerProspectoById(IdProspecto);

                if (pro != null)
                {
                    LimpiarDropDownListSelection();

                    // Datos personales
                    txtNombreApellido.Text = pro.NombreApellido;
                    txtFechaNacimiento.Text = pro.FechaNacimiento.ToString("dd/MM/yyyy");
                    txtNacionalidad.Text = pro.Nacionalidad;
                    ListTipoDocumento.Items.FindByText(pro.TipoDocumento).Selected = true;
                    txtNumeroDocumento.Text = pro.Documento;
                    ListEstadoCivil.Items.FindByText(pro.EstadoCivil).Selected = true;
                    txtNombreYApellidoConyuge.Text = pro.Nombre_ApellidoConguye;
                    txtDniConyuge.Text = pro.DNIConguye;
                    ListEstudiosAlcanzados.Items.FindByText(pro.Estudio).Selected = true;
                    txtProfesion.Text = pro.Profesion;


                    // Direcciones
                    CargarTablaDireccion(pro);

                    // Datos fiscales
                    if (pro.Prospecto_DatosFiscales.Count > 0)
                    {
                        listCondicionIVA.ClearSelection();
                        txtRazonSocial.Text = pro.Prospecto_DatosFiscales.FirstOrDefault().RazonSocial;
                        txtCuit.Text = pro.Prospecto_DatosFiscales.FirstOrDefault().Cuit;
                        listCondicionIVA.Items.FindByText(pro.Prospecto_DatosFiscales.FirstOrDefault().CondicionIVA).Selected = true;
                    }

                    // Recepcion mercaderia
                    if (pro.Prospecto_Recepcion.Count > 0)
                    {
                        txtIndicacionEspecialDomicilio.Text = pro.Prospecto_Recepcion.FirstOrDefault().IndicacionesDomicilio;
                        txtIndicacionEspecialDomicilioDirectaEmpresa.Text = pro.Prospecto_Recepcion.FirstOrDefault().IndicacionesEntrega;
                        txtExpreso.Text = pro.Prospecto_Recepcion.FirstOrDefault().Expreso;
                        ListRecepcionDireccion.SelectedValue = pro.Prospecto_Recepcion.FirstOrDefault().Id_Direccion.ToString();
                    }

                    // Datos Patrimoniales
                    if (pro.Prospecto_DatosPatrimoniales.Count > 0)
                    {
                        ListRodado.ClearSelection();
                        ListLocalidad.ClearSelection();
                        ListProvincia.ClearSelection();
                        txtAñoRodado.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Año;
                        txtModeloRodado.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Modelo;
                        ListRodado.SelectedValue = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Rodado.ToString();
                        ListPoseeVivienda.SelectedValue = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Vivienda.ToString();
                        ListProvinciaPatrimonial.SelectedValue = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Provincia.ToString();
                        CargarLocalidadesPatrimoniales(ListProvinciaPatrimonial.SelectedValue);
                        ListLocalidadPatrimonial.SelectedValue = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Localidad.ToString();
                        txtDireccionPatrimonial.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Direccion.ToString();
                        txtObservacionesPatrimoniales.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Observaciones.ToString();
                    }

                    // Contacto
                    if (pro.Prospecto_Contacto.Count > 0)
                    {
                        txtNumeroCelular.Text = pro.Prospecto_Contacto.FirstOrDefault().NumeroCelular;
                        txtNumeroTelefono.Text = pro.Prospecto_Contacto.FirstOrDefault().NumeroTelefono;
                        txtMailContacto.Text = pro.Prospecto_Contacto.FirstOrDefault().Email;
                    }
                    // Contacto
                    if (pro.Prospecto_DatoDistribucion.Count > 0)
                    {
                        ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                        controladorCliente controladorCliente = new controladorCliente();
                        txtNombreGrupo.Text = pro.Prospecto_DatoDistribucion.FirstOrDefault().NombreGrupo;
                        var cliente = controladorCliente.obtenerClienteID((int)pro.Prospecto_DatoDistribucion.FirstOrDefault().IdCliente);

                        this.ListDistribuidor.Items.Insert(0, new ListItem(cliente.alias, cliente.id.ToString()));
                        ListDistribuidor.SelectedValue = cliente.id.ToString();
                    }

                    if (controladorProspectos.VerificarGaranteProspecto(IdProspecto) == -3)
                    {
                        CargarGarantes(IdProspecto);
                    }
                    cargarDocumentacion();




                }
            }
            catch (Exception ex)
            {

            }
        }
        public void cargarDocumentacion()
        {
            try
            {
                //Busco si tiene un archivo cargado
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("../../DocumentacionProspecto/" + IdProspecto + "/"));
                var folders = di.GetDirectories();
                foreach (DirectoryInfo folder in folders)
                {
                    var files = folder.GetFiles();
                    if (files != null)
                    {
                        switch (folder.Name)
                        {
                            case "ConstanciaAFIP":
                                btnDescargarArchivoConstanciaAFIP.Visible = true;
                                btnDescargarArchivoConstanciaAFIP.ToolTip = files[0].Name;
                                btnDescargarArchivoConstanciaAFIP.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString()+"/ConstanciaAFIP/"+ files[0].Name;
                                btnDescargarArchivoConstanciaAFIP.Attributes["download"] = files[0].Name;
                                break;
                            case "ContratoComercial":
                                btnDescargarArchivoContratoComercial.Visible = true;
                                btnDescargarArchivoContratoComercial.ToolTip = files[0].Name;
                                btnDescargarArchivoContratoComercial.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/ContratoComercial/" + files[0].Name;
                                btnDescargarArchivoContratoComercial.Attributes["download"] = files[0].Name;
                                break;
                            case "DNIDistribuidor":
                                btnDescargarArchivoDNIDistribuidor.Visible = true;
                                btnDescargarArchivoDNIDistribuidor.ToolTip = files[0].Name;
                                btnDescargarArchivoDNIDistribuidor.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/DNIDistribuidor/" + files[0].Name;
                                btnDescargarArchivoDNIDistribuidor.Attributes["download"] = files[0].Name;
                                break;
                            case "DNIGarante":
                                btnDescargarArchivoDNIGarante.Visible = true;
                                btnDescargarArchivoDNIGarante.ToolTip = files[0].Name;
                                btnDescargarArchivoDNIGarante.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/DNIGarante/" + files[0].Name;
                                btnDescargarArchivoDNIGarante.Attributes["download"] = files[0].Name;
                                break;
                            case "Fianza":
                                btnDescargarArchivoFianza.Visible = true;
                                btnDescargarArchivoFianza.ToolTip = files[0].Name;
                                btnDescargarArchivoFianza.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/Fianza/" + files[0].Name;
                                btnDescargarArchivoFianza.Attributes["download"] = files[0].Name;
                                break;
                            case "Pagare":
                                btnDescargarArchivoPagare.Visible = true;
                                btnDescargarArchivoPagare.ToolTip = files[0].Name;
                                btnDescargarArchivoPagare.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/Pagare/" + files[0].Name;
                                btnDescargarArchivoPagare.Attributes["download"] = files[0].Name;
                                break;
                            case "ReciboSueldoGarante":
                                btnDescargarArchivoReciboSueldoGarante.Visible = true;
                                btnDescargarArchivoReciboSueldoGarante.ToolTip = files[0].Name;
                                btnDescargarArchivoReciboSueldoGarante.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/ReciboSueldoGarante/" + files[0].Name;
                                btnDescargarArchivoReciboSueldoGarante.Attributes["download"] = files[0].Name;
                                break;
                            case "ServicioDistribuidor":
                                btnDescargarArchivoServicioDistribuidor.Visible = true;
                                btnDescargarArchivoServicioDistribuidor.ToolTip = files[0].Name;
                                btnDescargarArchivoServicioDistribuidor.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/ServicioDistribuidor/" + files[0].Name;
                                btnDescargarArchivoServicioDistribuidor.Attributes["download"] = files[0].Name;
                                break;
                            case "ServicioGarante":
                                btnDescargarArchivoServicioGarante.Visible = true;
                                btnDescargarArchivoServicioGarante.ToolTip = files[0].Name;
                                btnDescargarArchivoServicioGarante.Attributes["href"] = "/DocumentacionProspecto/" + IdProspecto.ToString() + "/ServicioGarante/" + files[0].Name;
                                btnDescargarArchivoServicioGarante.Attributes["download"] = files[0].Name;
                                break;
                        }

                    }

                    }

                }
            catch (Exception ex)
            {

                throw;
            }




        }
        public void CargarGarantes(int IdProspecto)
        {

            ControladorProspectos controladorProspecto = new ControladorProspectos();
            var garante = controladorProspecto.ObtenerGaranteByIdProspecto(IdProspecto);
            var direccion = controladorProspecto.ObtenerDireccionById((int)garante.IdDireccion);
            ViewState["idgarante"] = (int)garante.Id;
            ListLocalidadGarantes.ClearSelection();
            ListProvinciaGarantes.ClearSelection();
            ListTiposDocumentosGarante.ClearSelection();
            ListEstadoCivilGarante.ClearSelection();
            ListRelacionDependenciaGarante.ClearSelection();

            txtNombreGarante.Text = garante.Nombre;
            txtApellidoGarante.Text = garante.Apellido;
            txtFechaNacimientoGarante.Text = garante.FechaNacimiento.ToString("dd/MM/yyyy");
            txtNacionalidadGarante.Text = garante.Nacionalidad;
            ListTiposDocumentosGarante.SelectedValue = garante.TipoDocumento;
            txtDocumentoGarante.Text = garante.Documento;
            ListEstadoCivilGarante.SelectedValue = garante.EstadoCivil;
            txtProfesionGarante.Text = garante.Profesion;
            ListRelacionDependenciaGarante.SelectedValue = garante.RelacionDependencia.ToString();
            txtAntiguedadGarante.Text = garante.Antiguedad;
            txtCargoGarante.Text = garante.Cargo;
            txtRazonSocialEmpleadorGarante.Text = garante.RazonSocialEmpleador;
            txtDomicilioEmpleadorGarante.Text = garante.DomicilioEmpleador;
            txtCUITEmpleadorGarante.Text = garante.CUIT;
            txtNombreConyugeGarante.Text = garante.NombreConyuge;
            txtApellidoConyugeGarante.Text = garante.ApellidoConyuge;
            txtDNIConyugeGarante.Text = garante.DNIConyuge;

            txtDomicilioGarante.Text = direccion.Calle;
            txtNumeroDireccionGarante.Text = direccion.Numero;

            ListProvinciaGarantes.SelectedValue = direccion.Provincia;
            CargarLocalidadesGarantes(ListProvinciaGarantes.SelectedValue);
            ListLocalidadGarantes.SelectedValue = direccion.Localidad;
            CargarContactoGarantes(garante.Id);
            CargarPatrimonioGarantes(garante.Id);




        }
        public void CargarContactoGarantes(int idGarante)
        {
            try
            {
                ControladorProspectos controladorProspecto = new ControladorProspectos();
                var contactoGarante = controladorProspecto.ObtenerContactoByIdGarante(idGarante);
                txtTelefonoFijoGarante.Text = contactoGarante.TelefonoFijo;
                txtTelefonoCelularGarante.Text = contactoGarante.TelefonoCelular;
                txtCorreoElectronicoGarante.Text = contactoGarante.CorreoElectronico;
                txtFacebookGarante.Text = contactoGarante.Facebook;
            }
            catch (Exception ex)
            {


            }

        }
        public void CargarPatrimonioGarantes(int idGarante)
        {
            try
            {
                ControladorProspectos controladorProspecto = new ControladorProspectos();
                var patrimonioGarante = controladorProspecto.ObtenerPatrimonioByIdGarante(idGarante);
                var direccion = controladorProspecto.ObtenerDireccionById((int)patrimonioGarante.IdDireccion);

                ListPoseeViviendaGarantePatrimonial.ClearSelection();
                ListProvinciaGarantePatrimoniales.ClearSelection();
                ListLocalidadGarantePatrimoniales.ClearSelection();
                ListTiposViviendasGarante.ClearSelection();
                ListPoseeRodadoGarantePatrimonial.ClearSelection();

                ListPoseeViviendaGarantePatrimonial.SelectedValue = patrimonioGarante.Vivienda.ToString();
                txtCallePatrimonialGarante.Text = direccion.Calle;
                txtDomicilioPatrimonialGarante.Text = direccion.Numero;
                ListProvinciaGarantePatrimoniales.SelectedValue = direccion.Provincia;
                //ver 
                CargarLocalidadesGarantesPatrimoniales(ListProvinciaGarantePatrimoniales.SelectedValue);
                ListLocalidadGarantePatrimoniales.SelectedValue = direccion.Localidad;
                txtCodigoPostalGarantePatrimonial.Text = direccion.CodigoPostal;
                ListTiposViviendasGarante.SelectedValue = direccion.Tipo;
                txtMetrosViviendaGarante.Text = direccion.Metros;
                ListPoseeRodadoGarantePatrimonial.SelectedValue = patrimonioGarante.Rodado.ToString();
                txtModeloRodadoGarante.Text = patrimonioGarante.Modelo;
                txtMarcaRodadoGarante.Text = patrimonioGarante.Marca;
                txtAñoRodadoGarante.Text = patrimonioGarante.Año.ToString();



            }
            catch (Exception ex)
            {


            }

        }

        public void CargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();
                dt = contCliente.obtenerClientesDT();


                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListDistribuidor.DataSource = dt;
                this.ListDistribuidor.DataValueField = "id";
                this.ListDistribuidor.DataTextField = "alias";

                this.ListDistribuidor.DataBind();

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        public void LimpiarDropDownListSelection()
        {
            try
            {
                ListTipoDocumento.ClearSelection();
                ListEstadoCivil.ClearSelection();
                ListEstudiosAlcanzados.ClearSelection();
                ListRelacionDependencia.ClearSelection();

            }
            catch (Exception ex)
            {

            }
        }

        protected void ListProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarLocalidades(this.ListProvincia.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error seleccionando provincia para cargar localidad. " + ex.Message + "')", true);

            }
        }

        private void CargarLocalidades(string provincia)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListLocalidad.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidad.DataValueField = "Localidad";
                this.ListLocalidad.DataTextField = "Localidad";

                this.ListLocalidad.DataBind();
                //cargo el codigo postal
                this.CargarCodigoPostal(this.ListProvincia.SelectedValue, this.ListLocalidad.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  localidades. " + ex.Message + "')", true);
            }
        }
        private void CargarLocalidadesPatrimoniales(string provincia)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListLocalidadPatrimonial.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidadPatrimonial.DataValueField = "Localidad";
                this.ListLocalidadPatrimonial.DataTextField = "Localidad";

                this.ListLocalidadPatrimonial.DataBind();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  localidades. " + ex.Message + "')", true);
            }
        }

        private void CargarLocalidadesGarantes(string provincia)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListLocalidadGarantes.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidadGarantes.DataValueField = "Localidad";
                this.ListLocalidadGarantes.DataTextField = "Localidad";

                this.ListLocalidadGarantes.DataBind();
                //cargo el codigo postal
                this.CargarCodigoPostalGarante(this.ListProvinciaGarantes.SelectedValue, this.ListLocalidadGarantes.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  localidades. " + ex.Message + "')", true);
            }
        }
        private void CargarLocalidadesGarantesPatrimoniales(string provincia)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                this.ListLocalidadGarantePatrimoniales.DataSource = controladorPais.obtenerLocalidadProvincia(provincia);
                this.ListLocalidadGarantePatrimoniales.DataValueField = "Localidad";
                this.ListLocalidadGarantePatrimoniales.DataTextField = "Localidad";

                this.ListLocalidadGarantePatrimoniales.DataBind();
                //cargo el codigo postal
                this.CargarCodigoPostalGarantePatrimonial(this.ListProvinciaGarantePatrimoniales.SelectedValue, this.ListLocalidadGarantePatrimoniales.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  localidades. " + ex.Message + "')", true);
            }
        }

        private void CargarCodigoPostal(string provincia, string localidad)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerCodPostalByLocalidadProvincia(provincia, localidad);
                foreach (DataRow dr in dt.Rows)
                {
                    this.txtCodigoPostal.Text = dr[0].ToString();
                    this.txtLocalidad.Text = localidad;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  codigo postales.: " + ex.Message + "')", true);
            }
        }

        private void CargarCodigoPostalGarante(string provincia, string localidad)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerCodPostalByLocalidadProvincia(provincia, localidad);
                foreach (DataRow dr in dt.Rows)
                {
                    this.txtCodigoPostalGarante.Text = dr[0].ToString();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  codigo postales.: " + ex.Message + "')", true);
            }
        }
        private void CargarCodigoPostalGarantePatrimonial(string provincia, string localidad)
        {
            try
            {
                controladorPais controladorPais = new controladorPais();
                DataTable dt = controladorPais.obtenerCodPostalByLocalidadProvincia(provincia, localidad);
                foreach (DataRow dr in dt.Rows)
                {
                    this.txtCodigoPostalGarantePatrimonial.Text = dr[0].ToString();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error cargando lista de  codigo postales.: " + ex.Message + "')", true);
            }
        }

        protected void ListLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarCodigoPostal(this.ListProvincia.SelectedValue, this.ListLocalidad.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        protected void btnAgregarDireccion_Click(object sender, EventArgs e)
        {
            try
            {
                if ((ViewState["idprospecto"] != null || this.IdProspecto > 0) && ViewState["idDireccion"] == null)
                {
                    this.AgregarDireccion();

                }
                else if (ViewState["idDireccion"] != null && (ViewState["idprospecto"] != null || this.IdProspecto >= 0))
                {
                    this.ModificarDireccion();
                }
                //else if (this.IdProspecto > 0 && ViewState["idDireccion"] == null)
                //{

                //}
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }
        public void AgregarDireccion()
        {
            try
            {
                int idprospecto = 0;
                if (ViewState["idprospecto"] != null)
                {
                    idprospecto = (int)ViewState["idprospecto"];
                }
                else if (this.IdProspecto > 0)
                {
                    idprospecto = this.IdProspecto;
                }

                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospecto_Direccion prospecto_Direccion = new Prospecto_Direccion();

                prospecto_Direccion.Provincia = ListProvincia.SelectedItem.Text;
                prospecto_Direccion.Localidad = txtLocalidad.Text;
                prospecto_Direccion.CodigoPostal = txtCodigoPostal.Text;
                prospecto_Direccion.Barrio = txtBarrio.Text;
                prospecto_Direccion.Calle = txtCalle.Text;
                prospecto_Direccion.Numero = txtCalleNumero.Text;
                prospecto_Direccion.Tipo = ListTipoVivienda.SelectedItem.Text;
                prospecto_Direccion.TipoDireccion = ListTipo.SelectedItem.Text;
                prospecto_Direccion.Metros = txtMetrosVivienda.Text;
                prospecto_Direccion.Estado = 1;

                var agregado = controladorProspectos.AgregaDireccion(prospecto_Direccion, idprospecto);

                if (agregado != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                }

                LimpiarCamposDir();
                CargarTablaDireccion();
                CargarDireccionRecepcion();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }

        }

        public void ModificarDireccion()
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospecto_Direccion prospecto_Direccion = new Prospecto_Direccion();

                string aux = ViewState["idDireccion"].ToString();
                int idDireccion = Convert.ToInt32(aux);

                prospecto_Direccion.Id = idDireccion;
                prospecto_Direccion.Provincia = ListProvincia.SelectedItem.Text;
                prospecto_Direccion.Localidad = txtLocalidad.Text;
                prospecto_Direccion.CodigoPostal = txtCodigoPostal.Text;
                prospecto_Direccion.Barrio = txtBarrio.Text;
                prospecto_Direccion.Calle = txtCalle.Text;
                prospecto_Direccion.Numero = txtCalleNumero.Text;
                prospecto_Direccion.Tipo = ListTipoVivienda.SelectedItem.Text;
                prospecto_Direccion.Metros = txtMetrosVivienda.Text;
                prospecto_Direccion.TipoDireccion = ListTipo.SelectedItem.Text;
                prospecto_Direccion.Estado = 1;

                var modificar = controladorProspectos.ModificarDireccion(prospecto_Direccion);

                if (modificar != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                }

                LimpiarCamposDir();
                ViewState["idDireccion"] = null;
                CargarTablaDireccion();
                CargarDireccionRecepcion();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        private void LimpiarCamposDir()
        {
            try
            {
                this.txtCalle.Text = "";
                this.ListProvincia.SelectedIndex = 0;
                //this.ListLocalidad.SelectedIndex = 0;
                this.txtLocalidad.Text = "";
                this.txtCodigoPostal.Text = "";
                this.txtCalleNumero.Text = "";
                this.txtBarrio.Text = "";
                this.txtMetrosVivienda.Text = "";
                this.ListTipoVivienda.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        public void CargarTablaDireccion(Prospectos prospecto = null)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                this.phDireccion.Controls.Clear();
                int idProspecto = 0;
                List<Prospecto_direccion_relacion> direcciones = new List<Prospecto_direccion_relacion>();

                if (ViewState["idprospecto"] != null && accion == 1)
                {
                    idProspecto = (int)ViewState["idprospecto"];
                    direcciones = controladorProspectos.ObtenerDireccionesByIdProspecto(idProspecto);

                    foreach (Prospecto_direccion_relacion d in direcciones)
                    {
                        this.CargarPHDireccion(d);
                    }
                }
                else if (prospecto != null && accion == 1)
                {
                    foreach (Prospecto_direccion_relacion d in prospecto.Prospecto_direccion_relacion)
                    {
                        this.CargarPHDireccion(d);
                    }
                }
                else if (this.IdProspecto > 0 && accion == 2)
                {
                    direcciones = controladorProspectos.ObtenerDireccionesByIdProspecto(this.IdProspecto);
                    foreach (Prospecto_direccion_relacion d in direcciones)
                    {
                        this.CargarPHDireccion(d);
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        public void CargarPHDireccion(Prospecto_direccion_relacion d)
        {
            try
            {
                //Fila
                TableRow tr = new TableRow();
                tr.ID = "Direcion_" + d.Id_Direccion.ToString();

                //Celdas
                TableCell celNombre = new TableCell();
                celNombre.Text = d.Prospecto_Direccion.Tipo;
                celNombre.VerticalAlign = VerticalAlign.Middle;
                celNombre.Width = Unit.Percentage(15);
                tr.Cells.Add(celNombre);

                TableCell celCargo = new TableCell();
                celCargo.Text = d.Prospecto_Direccion.Calle;
                celCargo.VerticalAlign = VerticalAlign.Middle;
                celCargo.Width = Unit.Percentage(20);
                tr.Cells.Add(celCargo);

                TableCell celNumero = new TableCell();
                celNumero.Text = d.Prospecto_Direccion.Provincia;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.Width = Unit.Percentage(20);
                tr.Cells.Add(celNumero);

                TableCell celMail = new TableCell();
                celMail.Text = d.Prospecto_Direccion.Localidad;
                celMail.VerticalAlign = VerticalAlign.Middle;
                celMail.Width = Unit.Percentage(20);
                tr.Cells.Add(celMail);

                TableCell celBarrio = new TableCell();
                celBarrio.Text = d.Prospecto_Direccion.Barrio;
                celBarrio.VerticalAlign = VerticalAlign.Middle;
                celBarrio.Width = Unit.Percentage(10);
                tr.Cells.Add(celBarrio);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = d.Prospecto_Direccion.CodigoPostal;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                celCodigo.Width = Unit.Percentage(10);
                tr.Cells.Add(celCodigo);

                TableCell celAccion = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.CssClass = "btn btn-info";
                btnEditar.ID = "btnEditarD_" + d.Id_Direccion;
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                btnEditar.Click += new EventHandler(this.EditarItemDireccion);
                celAccion.Controls.Add(btnEditar);


                Literal l = new Literal();
                l.Text = "&nbsp";
                celAccion.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.ID = "btnEliminarD_" + d.Id_Direccion;
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Click += new EventHandler(this.QuitarItemDireccion);
                celAccion.Controls.Add(btnEliminar);
                celAccion.Width = Unit.Percentage(15);
                tr.Cells.Add(celAccion);

                phDireccion.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        private void QuitarItemDireccion(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                string idDireccion = (sender as LinkButton).ID.Substring(13);

                var eliminado = controladorProspectos.EliminarDireccion(Convert.ToInt32(idDireccion));

                if (eliminado != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEliminado()", true);
                }

                //Vuelvo a cargar las direcciones
                this.CargarTablaDireccion();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        private void EditarItemDireccion(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                string idDireccion = (sender as LinkButton).ID.Substring(11);

                var direccion = controladorProspectos.ObtenerDireccionById(Convert.ToInt32(idDireccion));

                this.ListProvincia.SelectedValue = direccion.Provincia.ToString();
                this.ListTipoVivienda.Items.FindByText(direccion.Tipo.ToString()).Selected = true;
                this.txtLocalidad.Text = direccion.Localidad.ToString();
                this.txtCalle.Text = direccion.Calle.ToString();
                this.txtCodigoPostal.Text = direccion.CodigoPostal.ToString();
                this.txtBarrio.Text = direccion.Barrio.ToString();
                this.txtCalleNumero.Text = direccion.Numero.ToString();
                this.txtMetrosVivienda.Text = direccion.Metros.ToString();

                ViewState["idDireccion"] = idDireccion;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.accion == 1)
                    AgregarProspecto();

                if (this.accion == 2)
                    ModificarProspecto();
            }
            catch (Exception ex)
            {

            }
        }

        public void AgregarProspecto()
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospectos prospecto = new Prospectos();

                prospecto.NombreApellido = txtNombreApellido.Text;
                prospecto.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text, new CultureInfo("es-AR"));
                prospecto.Nacionalidad = txtNacionalidad.Text;
                prospecto.Documento = txtNumeroDocumento.Text;
                prospecto.TipoDocumento = ListTipoDocumento.SelectedItem.Text;
                prospecto.EstadoCivil = ListEstadoCivil.SelectedItem.Text;
                prospecto.Nombre_ApellidoConguye = txtNombreYApellidoConyuge.Text;
                prospecto.DNIConguye = txtDniConyuge.Text;
                prospecto.Estudio = ListEstudiosAlcanzados.SelectedItem.Text;
                prospecto.Profesion = txtProfesion.Text;
                prospecto.Relacion_Dependencia = Convert.ToInt32(ListRelacionDependencia.SelectedValue);
                prospecto.Estado = 1;

                var pro = controladorProspectos.AgregarProspecto(prospecto);

                if (pro != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                }

                ViewState["idprospecto"] = prospecto.Id;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }

        public void ModificarProspecto()
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospectos prospecto = new Prospectos();

                prospecto.Id = IdProspecto;
                prospecto.NombreApellido = txtNombreApellido.Text;
                prospecto.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text);
                prospecto.Nacionalidad = txtNacionalidad.Text;
                prospecto.Documento = txtNumeroDocumento.Text;
                prospecto.TipoDocumento = ListTipoDocumento.SelectedItem.Text;
                prospecto.EstadoCivil = ListEstadoCivil.SelectedItem.Text;
                prospecto.Nombre_ApellidoConguye = txtNombreYApellidoConyuge.Text;
                prospecto.DNIConguye = txtDniConyuge.Text;
                prospecto.Estudio = ListEstudiosAlcanzados.SelectedItem.Text;
                prospecto.Profesion = txtProfesion.Text;
                prospecto.Relacion_Dependencia = Convert.ToInt32(ListRelacionDependencia.SelectedValue);
                prospecto.Estado = 1;

                controladorProspectos.ModificarProspecto(prospecto);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnAgregarDatosFiscales_Click(object sender, EventArgs e)
        {

            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospecto_DatosFiscales prospecto_DatosFiscales = new Prospecto_DatosFiscales();

                if (ViewState["idprospecto"] != null && this.accion == 1)
                {
                    int idProspecto = (int)ViewState["idprospecto"];
                    prospecto_DatosFiscales.Id_Prospecto = idProspecto;
                    prospecto_DatosFiscales.RazonSocial = txtRazonSocial.Text;
                    prospecto_DatosFiscales.Cuit = txtCuit.Text;
                    prospecto_DatosFiscales.CondicionIVA = listCondicionIVA.SelectedItem.Text;

                    var datosFiscal = controladorProspectos.AgregarDatoFiscal(prospecto_DatosFiscales);

                    if (datosFiscal != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }

                }
                else if (ViewState["idprospecto"] == null && this.accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
                }
                else if (this.IdProspecto > 0 && this.accion == 2)
                {
                    int idProspecto = this.IdProspecto;
                    prospecto_DatosFiscales.Id_Prospecto = idProspecto;
                    prospecto_DatosFiscales.RazonSocial = txtRazonSocial.Text;
                    prospecto_DatosFiscales.Cuit = txtCuit.Text;
                    prospecto_DatosFiscales.CondicionIVA = listCondicionIVA.SelectedItem.Text;

                    var dato = controladorProspectos.ModificarOAgregarDatoFiscal(prospecto_DatosFiscales);

                    if (dato != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void btnAgregarRecepcionMercaderia_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Prospecto_Recepcion prospecto_Recepcion = new Prospecto_Recepcion();

                if (ViewState["idprospecto"] != null && this.accion == 1)
                {
                    int idProspecto = (int)ViewState["idprospecto"];
                    prospecto_Recepcion.Id_Prospecto = idProspecto;
                    prospecto_Recepcion.Id_Direccion = Convert.ToInt32(ListRecepcionDireccion.SelectedValue);
                    prospecto_Recepcion.IndicacionesDomicilio = txtIndicacionEspecialDomicilio.Text;
                    prospecto_Recepcion.IndicacionesEntrega = txtIndicacionEspecialDomicilioDirectaEmpresa.Text;
                    prospecto_Recepcion.Expreso = txtExpreso.Text;

                    var datosFiscal = controladorProspectos.AgregarRecepcion(prospecto_Recepcion);

                    if (datosFiscal != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }

                }
                else if (ViewState["idprospecto"] == null && this.accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
                }
                else if (this.IdProspecto > 0 && this.accion == 2)
                {
                    int idProspecto = this.IdProspecto;
                    prospecto_Recepcion.Id_Prospecto = idProspecto;
                    prospecto_Recepcion.Id_Direccion = Convert.ToInt32(ListRecepcionDireccion.SelectedValue);
                    prospecto_Recepcion.IndicacionesDomicilio = txtIndicacionEspecialDomicilio.Text;
                    prospecto_Recepcion.IndicacionesEntrega = txtIndicacionEspecialDomicilioDirectaEmpresa.Text;
                    prospecto_Recepcion.Expreso = txtExpreso.Text;

                    var dato = controladorProspectos.ModificarOAgregarDatoFiscal(prospecto_Recepcion);

                    if (dato != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnAgregarDatoDistribucion_Click(object sender, EventArgs e)
        {
            ControladorProspectos controladorProspectos = new ControladorProspectos();
            Prospecto_DatoDistribucion prospecto_DatoDistribucion = new Prospecto_DatoDistribucion();

            if (ViewState["idprospecto"] != null && this.accion == 1)
            {
                int idProspecto = (int)ViewState["idprospecto"];
                prospecto_DatoDistribucion.Id_Prospecto = idProspecto;
                prospecto_DatoDistribucion.NombreGrupo = txtNombreGrupo.Text;
                prospecto_DatoDistribucion.IdCliente = Convert.ToInt32(ListDistribuidor.SelectedValue);

                var dato = controladorProspectos.AgregarDatoDistribucion(prospecto_DatoDistribucion);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                }
            }
            else if (ViewState["idprospecto"] == null && this.accion == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
            }
            else if (this.IdProspecto > 0 && this.accion == 2)
            {
                prospecto_DatoDistribucion.Id_Prospecto = this.IdProspecto;
                prospecto_DatoDistribucion.NombreGrupo = txtNombreGrupo.Text;
                prospecto_DatoDistribucion.IdCliente = Convert.ToInt32(ListDistribuidor.SelectedValue);

                var dato = controladorProspectos.ModificarOAgregarDatoDistribucion(prospecto_DatoDistribucion);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                }
            }

        }

        protected void btnAgregarDatosPatrimoniales_Click(object sender, EventArgs e)
        {
            ControladorProspectos controladorProspectos = new ControladorProspectos();
            Prospecto_DatosPatrimoniales patrimonial = new Prospecto_DatosPatrimoniales();

            if (ViewState["idprospecto"] != null && this.accion == 1)
            {
                int idProspecto = (int)ViewState["idprospecto"];
                patrimonial.Id_Prospecto = idProspecto;
                patrimonial.Rodado = Convert.ToInt32(ListRodado.SelectedValue);
                patrimonial.Modelo = txtModeloRodado.Text;
                patrimonial.Año = txtAñoRodado.Text;
                patrimonial.Vivienda = Convert.ToInt32(ListPoseeVivienda.SelectedValue);
                patrimonial.Provincia = ListProvinciaPatrimonial.SelectedValue;
                patrimonial.Localidad = ListLocalidadPatrimonial.SelectedValue;
                patrimonial.Direccion = txtDireccionPatrimonial.Text;
                patrimonial.Observaciones = txtObservacionesPatrimoniales.Text;

                var dato = controladorProspectos.AgregarDatoPatrimonial(patrimonial);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                }
            }
            else if (ViewState["idprospecto"] == null && this.accion == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
            }
            else if (this.IdProspecto > 0 && this.accion == 2)
            {
                patrimonial.Id_Prospecto = IdProspecto;
                patrimonial.Rodado = Convert.ToInt32(ListRodado.SelectedValue);
                patrimonial.Modelo = txtModeloRodado.Text;
                patrimonial.Año = txtAñoRodado.Text;
                patrimonial.Vivienda = Convert.ToInt32(ListPoseeVivienda.SelectedValue);
                patrimonial.Provincia = ListProvinciaPatrimonial.SelectedValue;
                patrimonial.Localidad = ListLocalidadPatrimonial.SelectedValue;
                patrimonial.Direccion = txtDireccionPatrimonial.Text;
                patrimonial.Observaciones = txtObservacionesPatrimoniales.Text;

                var dato = controladorProspectos.ModificarOAgregarDatoPatrimonial(patrimonial);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                }
            }
        }

        protected void lbtnAgregarContacto_Click(object sender, EventArgs e)
        {
            ControladorProspectos controladorProspectos = new ControladorProspectos();
            Prospecto_Contacto contacto = new Prospecto_Contacto();

            if (ViewState["idprospecto"] != null && this.accion == 1)
            {
                int idProspecto = (int)ViewState["idprospecto"];
                contacto.Id_Prospecto = idProspecto;
                contacto.NumeroCelular = txtNumeroCelular.Text;
                contacto.NumeroTelefono = txtNumeroTelefono.Text;
                contacto.Email = txtMailContacto.Text;

                var dato = controladorProspectos.AgregarContacto(contacto);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                }
            }
            else if (ViewState["idprospecto"] == null && this.accion == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeError()", true);
            }
            else if (this.IdProspecto > 0 && this.accion == 2)
            {
                contacto.Id_Prospecto = this.IdProspecto;
                contacto.NumeroCelular = txtNumeroCelular.Text;
                contacto.NumeroTelefono = txtNumeroTelefono.Text;
                contacto.Email = txtMailContacto.Text;

                var dato = controladorProspectos.ModificarOAgregarContacto(contacto);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                }
            }
        }

        protected void ListProvinciaPatrimonial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarLocalidadesPatrimoniales(this.ListProvinciaPatrimonial.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error seleccionando provincia para cargar localidad. " + ex.Message + "')", true);

            }
        }
        protected void btnBuscarCodigoDistribuidor_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity contrClienteEntity = new ControladorClienteEntity();
                var clienteDistribuidor = contrClienteEntity.ObtenerListClientesByCodigo(txtCodDistribuidor.Text);

                //cargo la lista
                this.ListDistribuidor.DataSource = clienteDistribuidor;
                this.ListDistribuidor.DataValueField = "id";
                this.ListDistribuidor.DataTextField = "alias";
                this.ListDistribuidor.DataBind();

            }
            catch (Exception Ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando proveedores a la lista. Excepción: " + Ex.Message));
            }
        }

        protected void ListProvinciaGarantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarLocalidadesGarantes(this.ListProvinciaGarantes.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error seleccionando provincia para cargar localidad. " + ex.Message + "')", true);

            }
        }

        protected void ListProvinciaGarantePatrimoniales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarLocalidadesGarantesPatrimoniales(this.ListProvinciaGarantePatrimoniales.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error seleccionando provincia para cargar localidad. " + ex.Message + "')", true);

            }
        }

        protected void btnAgregarPatrimonioGarante_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Garantes_Patrimonial garantePatrimonial = new Garantes_Patrimonial();
                Prospecto_Direccion prospectoDireccion = new Prospecto_Direccion();
                Prospecto_Direccion direccionAgregada = null;
                prospectoDireccion.Calle = txtCallePatrimonialGarante.Text;
                prospectoDireccion.Numero = txtDomicilioPatrimonialGarante.Text;
                prospectoDireccion.CodigoPostal = txtCodigoPostalGarantePatrimonial.Text;
                prospectoDireccion.Provincia = ListProvinciaGarantePatrimoniales.SelectedValue;
                prospectoDireccion.Localidad = ListLocalidadGarantePatrimoniales.SelectedValue;
                prospectoDireccion.Tipo = ListTiposViviendasGarante.SelectedItem.Text;
                prospectoDireccion.Metros = txtMetrosViviendaGarante.Text;


                garantePatrimonial.Vivienda = Convert.ToInt32(ListPoseeViviendaGarantePatrimonial.SelectedValue);
                garantePatrimonial.Rodado = Convert.ToInt32(ListPoseeRodadoGarantePatrimonial.SelectedValue);
                garantePatrimonial.Modelo = txtModeloRodadoGarante.Text;
                garantePatrimonial.Marca = txtMarcaRodadoGarante.Text;
                if (!String.IsNullOrEmpty(txtAñoRodadoGarante.Text))
                {
                    garantePatrimonial.Año = Convert.ToInt32(txtAñoRodadoGarante.Text);
                }

                if (ViewState["idprospecto"] != null && this.accion == 1 && ViewState["idgarante"] != null)
                {
                    garantePatrimonial.IdGarante = (int)ViewState["idgarante"];
                    if (controladorProspectos.ObtenerIdDireccionByIdGarante(garantePatrimonial.IdGarante) > 0)
                    {
                        prospectoDireccion.Id = controladorProspectos.ObtenerIdDireccionByIdGarante(garantePatrimonial.IdGarante);
                    }

                    if (prospectoDireccion.Provincia != "-1" && garantePatrimonial.Vivienda == 1)
                    {

                        direccionAgregada = controladorProspectos.AgregarOModificarDireccionPatrimonioGarante(prospectoDireccion);
                    }

                    if (direccionAgregada != null)
                    {
                        garantePatrimonial.IdDireccion = direccionAgregada.Id;
                    }
                    var garantePatrimonialAgregado = controladorProspectos.AgregarOModificarPatrimonioGarante(garantePatrimonial);
                    if (garantePatrimonialAgregado != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }



                }
                else if (ViewState["idprospecto"] == null && this.accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAlert('Debe rellenar los datos del garante primero')", true);
                }
                else if (this.IdProspecto > 0 && this.accion == 2)
                {

                    garantePatrimonial.IdGarante = (int)ViewState["idgarante"];
                    if (controladorProspectos.ObtenerIdDireccionByIdGarante(garantePatrimonial.IdGarante) > 0)
                    {
                        prospectoDireccion.Id = controladorProspectos.ObtenerIdDireccionByIdGarante(garantePatrimonial.IdGarante);
                    }
                    if (prospectoDireccion.Provincia != "-1")
                    {
                        if (garantePatrimonial.Vivienda == 1)
                        {
                            prospectoDireccion.Estado = 1;
                            var direccionAgregado = controladorProspectos.AgregarOModificarDireccionPatrimonioGarante(prospectoDireccion);
                            garantePatrimonial.IdDireccion = direccionAgregado.Id;
                        }
                        else
                        {
                            prospectoDireccion.Estado = 0;
                            var direccionAgregado = controladorProspectos.AgregarOModificarDireccionPatrimonioGarante(prospectoDireccion);
                            garantePatrimonial.IdDireccion = direccionAgregado.Id;
                        }
                    }

                    Garantes_Patrimonial garanteAgregado = new Garantes_Patrimonial();
                    garanteAgregado = controladorProspectos.AgregarOModificarPatrimonioGarante(garantePatrimonial);
                    if (garanteAgregado != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnAgregarContactoGarante_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Garantes_Contacto garanteContacto = new Garantes_Contacto();
                garanteContacto.TelefonoCelular = txtTelefonoCelularGarante.Text;
                garanteContacto.TelefonoFijo = txtTelefonoFijoGarante.Text;
                garanteContacto.CorreoElectronico = txtCorreoElectronicoGarante.Text;
                garanteContacto.Facebook = txtFacebookGarante.Text;
                if (ViewState["idprospecto"] != null && ViewState["idgarante"] != null && this.accion == 1)
                {
                    garanteContacto.IdGarante = (int)ViewState["idgarante"];

                    var garanteContactoAgregado = controladorProspectos.AgregarOModificarContactoGarante(garanteContacto);
                    if (garanteContactoAgregado != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                    }
                }
                else if (ViewState["idprospecto"] == null && this.accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAlert('Debe rellenar los datos personales de garante')", true);
                }
                else if (this.IdProspecto > 0 && this.accion == 2)
                {
                    garanteContacto.IdGarante = (int)ViewState["idgarante"];
                    var contactoAgregado = controladorProspectos.AgregarOModificarContactoGarante(garanteContacto);
                    if (contactoAgregado != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                    }





                }

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }

        }

        protected void btnAgregarDatosGarante_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorProspectos controladorProspectos = new ControladorProspectos();
                Garante garante = new Garante();
                Prospecto_Direccion direccion = new Prospecto_Direccion();

                direccion.Calle = txtDomicilioGarante.Text;
                direccion.Numero = txtNumeroDireccionGarante.Text;
                direccion.CodigoPostal = txtCodigoPostalGarante.Text;
                direccion.Provincia = ListProvinciaGarantes.SelectedValue;
                direccion.Localidad = ListLocalidadGarantes.SelectedValue;


                garante.Nombre = txtNombreGarante.Text;
                garante.Apellido = txtApellidoGarante.Text;
                garante.FechaNacimiento = Convert.ToDateTime(txtFechaNacimientoGarante.Text);
                garante.Nacionalidad = txtNacionalidadGarante.Text;
                garante.Documento = txtDocumentoGarante.Text;
                garante.TipoDocumento = ListTiposDocumentosGarante.SelectedValue;
                garante.EstadoCivil = ListEstadoCivilGarante.SelectedValue.ToString();
                garante.Profesion = txtProfesionGarante.Text;
                garante.RelacionDependencia = Convert.ToInt32(ListRelacionDependenciaGarante.SelectedValue);
                garante.Antiguedad = txtAntiguedadGarante.Text;
                garante.Cargo = txtCargoGarante.Text;
                garante.RazonSocialEmpleador = txtRazonSocialEmpleadorGarante.Text;
                garante.DomicilioEmpleador = txtDomicilioEmpleadorGarante.Text;
                garante.CUIT = txtCUITEmpleadorGarante.Text;
                garante.NombreConyuge = txtNombreConyugeGarante.Text;
                garante.ApellidoConyuge = txtApellidoConyugeGarante.Text;
                garante.DNIConyuge = txtDNIConyugeGarante.Text;
                if (ViewState["idprospecto"] != null && ViewState["idgarante"] == null && this.accion == 1)
                {
                    int idProspecto = (int)ViewState["idprospecto"];

                    var direccionAgregada = controladorProspectos.AgregaDireccionGarante(direccion);

                    if (direccionAgregada != null)
                    {
                        garante.IdDireccion = direccionAgregada.Id;
                        var garanteAgregado = controladorProspectos.AgregarGarante(garante, idProspecto);
                        if (garanteAgregado > 0)
                        {
                            ViewState["idgarante"] = garanteAgregado;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgregado()", true);
                        }
                        else if (garanteAgregado == -3)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAlert('El prospecto ya tiene un garante agregado')", true);
                        }
                    }


                }


                else if (ViewState["idprospecto"] == null && this.accion == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAlert('Debe rellenar los datos del prospecto primero')", true);
                }
                else if (this.IdProspecto > 0 && this.accion == 2)
                {
                    var GaranteObtenido = controladorProspectos.ObtenerGaranteByIdProspecto(IdProspecto);

                    // Si garante existe se modifica
                    if (GaranteObtenido != null)
                    {

                        var direccionModificada = controladorProspectos.ModificarDireccionGarante(direccion, (int)GaranteObtenido.IdDireccion);
                        var garanteAgregado = controladorProspectos.ModificarGarante(garante, GaranteObtenido.Id, direccionModificada.Id);
                        if (garanteAgregado != null || direccionModificada != null)
                        {
                            ViewState["idgarante"] = garanteAgregado.Id;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                        }

                    }
                    // Si garante no existe se agrega
                    else
                    {
                        var direccionAgregada = controladorProspectos.AgregaDireccionGarante(direccion);
                        if (direccionAgregada != null)
                        {
                            garante.IdDireccion = direccionAgregada.Id;
                            var garanteAgregado = controladorProspectos.AgregarGarante(garante, IdProspecto);
                            if (garanteAgregado != null)
                            {
                                ViewState["idgarante"] = garanteAgregado;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeEditado()", true);
                            }
                        }
                    }

                }
                else if (ViewState["idgarante"] != null && (this.accion == 1 || this.accion == 2))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAlert('El prospecto ya tiene un garante agregado')", true);
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }

        }
        protected void ListLocalidadGarantePatrimoniales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarCodigoPostalGarantePatrimonial(this.ListProvinciaGarantePatrimoniales.SelectedValue, this.ListLocalidadGarantePatrimoniales.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }
        protected void ListLocalidadGarantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarCodigoPostalGarante(this.ListProvinciaGarantes.SelectedValue, this.ListLocalidadGarantes.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeErrorCatch('Error: " + ex.Message + "')", true);
            }
        }
        protected void verificarBoton(object sender, EventArgs e)
        {
            String path = null;
            LinkButton clickedButton = (LinkButton)sender;
            FileUpload fileupload = null;
            switch (clickedButton.ID)
            {
                case "btnAgregarArchivoFianza":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/Fianza/");
                    fileupload = FileFianza;
                    break;
                case "btnAgregarArchivoPagare":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/Pagare/");
                    fileupload = FilePagare;
                    break;
                case "btnAgregarArchivoContratoComercial":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/ContratoComercial/");
                    fileupload = FileContratoComercial;
                    break;
                case "btnAgregarArchivoDNIDistribuidor":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/DNIDistribuidor/");
                    fileupload = FileDNIDistribuidor;
                    break;
                case "btnAgregarArchivoDNIGarante":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/DNIGarante/");
                    fileupload = FileDNIGarante;
                    break;
                case "btnAgregarArchivoServicioDistribuidor":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/ServicioDistribuidor/");
                    fileupload = FileServicioDistribuidor;
                    break;
                case "btnAgregarArchivoServicioGarante":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/ServicioGarante/");
                    fileupload = FileServicioGarante;
                    break;
                case "btnAgregarArchivoConstanciaAFIP":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/ConstanciaAFIP/");
                    fileupload = FileConstanciaAFIP;
                    break;
                case "btnAgregarArchivoReciboSueldoGarante":
                    path = Server.MapPath("../../DocumentacionProspecto/" + this.IdProspecto + "/ReciboSueldoGarante/");
                    fileupload = FileReciboSueldoGarante;


                    break;
            }
            subirDocumento(path, fileupload);

        }

        public void subirDocumento(String path, FileUpload file)
        {

            //if(!String.IsNullOrEmpty(this.txtCodArticulo.Text))
            if (this.IdProspecto > 0)
            {
                if (IsPostBack)
                {
                    Boolean fileOK = false;

                    if (file.HasFile)
                    {
                        String fileExtension =
                            System.IO.Path.GetExtension(file.FileName).ToLower();

                        String[] allowedExtensions = { ".doc", ".docx", "pdf", ".jpg", ".png", ".jpeg" };

                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            if (fileExtension == allowedExtensions[i])
                            {
                                fileOK = true;
                                break;

                            }
                        }
                    }
                    if (fileOK)
                    {
                        try
                        {
                            //creo el directorio si no exites y subo la foto
                            Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "Voy a subir el documento");

                            if (!Directory.Exists(path))
                            {
                                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "No existe directorio. " + path + ". lo creo");

                                Directory.CreateDirectory(path);
                                Log.EscribirSQL((int)Session["Login_IdUser"], "Info", "directorio creado");
                            }

                            //vacio la carpeta para que haya un solo archivo
                            this.borrarCarpeta(path);
                            //guardo nombre archivo
                            string documento = file.FileName;

                            //lo subo
                            file.PostedFile.SaveAs(path + file.FileName);
                            cargarDocumentacion();

                        }

                        catch (Exception ex)
                        {
                            //Label1.Text = "File could not be uploaded.";
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error actualizando imagen " + ex.Message + " ');", true);
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error actualizando imagen " + ex.Message));
                        }
                    }
                    else
                    {
                        //Label1.Text = "Cannot accept files of this type.";
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("El archivo debe ser JPG o PNG "));
                    }
                }
            }
            else
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe ingresar el Codigo de Articulo para poder Subir Imagenes"));

            }
        }
        public int borrarCarpeta(String path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception Ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Disculpe, ha ocurrido un error al cargar la configuracion del E-Mail. Contacte con soporte.');", true);
                return -1;
            }
        }
    }
}
