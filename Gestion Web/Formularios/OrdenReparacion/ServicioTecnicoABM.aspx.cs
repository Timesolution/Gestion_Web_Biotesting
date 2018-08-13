﻿using Gestion_Api.Modelo;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Disipar.Models;
using System.Data;

namespace Gestion_Web.Formularios.OrdenReparacion
{
    public partial class ServicioTecnicoABM : System.Web.UI.Page
    {
        controladorArticulo contArt = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorServicioTecnicoEntity contServTecEnt = new controladorServicioTecnicoEntity();
        Mensajes m = new Mensajes();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarMarcas();
            }
            CargarServicioTecnicos();

        }

        public void cargarMarcas()
        {
            try
            {
                DataTable dt = contArt.obtenerMarcasDT();

                this.ListMarcas.DataSource = dt;
                this.ListMarcas.DataValueField = "id";
                this.ListMarcas.DataTextField = "marca";

                this.ListMarcas.DataBind();

            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando lista de marcas. " + Ex.Message));
                Log.EscribirSQL(1, "Error", "Error cargando lista de marcas " + Ex.Message);
            }
        }

        protected void btnBuscarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                var marca = contArtEnt.ObtenerMarcaByDescripcion(txtDescMarca.Text);
                
                this.ListMarcas.SelectedValue = marca.FirstOrDefault().id.ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnAgregarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                if(!ListBoxMarcas.Items.Contains(this.ListMarcas.SelectedItem))
                    this.ListBoxMarcas.Items.Add(this.ListMarcas.SelectedItem);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnQuitarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                this.ListBoxMarcas.Items.Remove(this.ListBoxMarcas.SelectedItem);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando marca. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error buscando marca " + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioTecnico st = new ServicioTecnico();
                List<string> stList = new List<string>();

                st.Localidad = txtLocalidad.Text;
                st.Direccion = txtDireccion.Text;
                st.Telefono = txtCodArea.Text + txtCelular.Text;
                st.Observaciones = txtObservaciones.Text;
                st.Estado = 1;

                foreach (var item in ListBoxMarcas.Items)
                {
                    stList.Add(item.ToString());
                }

                var temp = contServTecEnt.AgregarServicioTecnico(st,stList);

                if(temp > 0)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Servicio tecnico agregado con exito!","ServicioTecnicoABM.aspx"));
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando servicio tecnico."));

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando servicio tecnico. " + ex.Message));
                Log.EscribirSQL(1, "Error", "Error agregando servicio tecnico " + ex.Message);
            }
        }

        public void CargarServicioTecnicos()
        {
            try
            {
                foreach (var item in contServTecEnt.ObtenerServiciosTecnicos())
                {
                    cargarEnPh(item);
                } 
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error cargando servicios tecnicos " + ex.Message);
            }
        }
        private void cargarEnPh(ServicioTecnico st)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = st.Id.ToString();

                //Celdas
                TableCell celLocalidad = new TableCell();
                celLocalidad.Text = st.Localidad;
                celLocalidad.HorizontalAlign = HorizontalAlign.Left;
                celLocalidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celLocalidad);                

                TableCell celDireccion = new TableCell();
                celDireccion.Text = st.Direccion;
                celDireccion.HorizontalAlign = HorizontalAlign.Left;
                celDireccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDireccion);

                TableCell celTelefono = new TableCell();
                celTelefono.Text = st.Telefono;
                celTelefono.HorizontalAlign = HorizontalAlign.Left;
                celTelefono.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTelefono);

                TableCell celObservaciones = new TableCell();
                celObservaciones.Text = st.Observaciones;
                celObservaciones.HorizontalAlign = HorizontalAlign.Left;
                celObservaciones.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celObservaciones);

                TableCell celMarcas = new TableCell();
                var marcas = contServTecEnt.ObtenerMarcasByIDServicioTecnico(st.Id);
                for (int i = 0; i < marcas.Count; i++)
                {
                    if(i == marcas.Count)
                        celMarcas.Text += marcas[i].descripcion;
                    else
                        celMarcas.Text += marcas[i].descripcion + ", ";
                }
                               
                celMarcas.HorizontalAlign = HorizontalAlign.Left;
                celMarcas.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celMarcas);

                //TableCell celAccion = new TableCell();

                //Literal lDetail = new Literal();
                //lDetail.ID = "btnEditar_" + st.Id.ToString();
                //lDetail.Text = "<a href=\"OrdenReparacionABM.aspx?a=2&idordenreparacion=" + or.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                //lDetail.Text += "<span class=\"shortcut-icon icon-pencil\"></span>";
                //lDetail.Text += "</a>";

                //celAccion.Controls.Add(lDetail);

                //Literal l1 = new Literal();
                //l1.Text = "&nbsp";
                //celAccion.Controls.Add(l1);

                //Literal lReport = new Literal();
                //lReport.ID = "btnReporte_" + or.Id.ToString();
                //lReport.Text = "<a href=\"ImpresionOrdenReparacion.aspx?a=1&or=" + or.Id.ToString() + "&prp=" + or.NumeroPRP.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                //lReport.Text += "<span class=\"shortcut-icon icon-search\"></span>";
                //lReport.Text += "</a>";

                //celAccion.Controls.Add(lReport);

                //Literal l2 = new Literal();
                //l2.Text = "&nbsp";
                //celAccion.Controls.Add(l2);

                //CheckBox cbSeleccion = new CheckBox();
                //cbSeleccion.ID = "cbSeleccion_" + or.Id;
                //cbSeleccion.CssClass = "btn btn-info";
                //cbSeleccion.Font.Size = 12;
                //celAccion.Controls.Add(cbSeleccion);

                //tr.Cells.Add(celAccion);

                phServicioTecnico.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }
    }
}