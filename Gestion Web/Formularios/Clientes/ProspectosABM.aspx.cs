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
                else if(this.IdProspecto > 0)
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

        }
        public void CargarEstadoCivil()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Soltero/a", "1"));
            items.Add(new ListItem("Casado/a", "2"));
            items.Add(new ListItem("Viudo/a", "3"));
            ListEstadoCivil.Items.AddRange(items.ToArray());
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
                        txtAñoRodado.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Año;
                        txtModeloRodado.Text = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Modelo;
                        ListRodado.SelectedValue = pro.Prospecto_DatosPatrimoniales.FirstOrDefault().Rodado.ToString();
                    }

                    // Contacto
                    if (pro.Prospecto_Contacto.Count > 0)
                    {
                        ListRodado.ClearSelection();
                        txtNumeroCelular.Text = pro.Prospecto_Contacto.FirstOrDefault().NumeroCelular;
                        txtNumeroTelefono.Text = pro.Prospecto_Contacto.FirstOrDefault().NumeroTelefono;
                        txtMailContacto.Text = pro.Prospecto_Contacto.FirstOrDefault().Email;
                    }

                }
            }
            catch (Exception ex)
            {

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
                prospecto_DatoDistribucion.DistribuidorSuperior = txtDistribuidorSuperior.Text;

                var dato = controladorProspectos.AgregarDatoDistribucion(prospecto_DatoDistribucion);

                if (dato != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "mensajeAgreado()", true);
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
                prospecto_DatoDistribucion.DistribuidorSuperior = txtDistribuidorSuperior.Text;

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
    }
}