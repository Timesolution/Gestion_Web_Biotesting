﻿using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class DiferenciasMercaderiaF : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFactEntity contFactEntity = new controladorFactEntity();

        int accion = 0;
        int sucursalDestino = 0;
        int sucursalOrigen = 0;
        string fechaD = "";
        string fechaH = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            accion = Convert.ToInt32(Request.QueryString["a"]);
            sucursalDestino = Convert.ToInt32(Request.QueryString["sd"]);
            sucursalOrigen = Convert.ToInt32(Request.QueryString["so"]);
            fechaD = Request.QueryString["fd"];
            fechaH = Request.QueryString["fh"];

            if (accion == 0)
            {
                PrimeraCarga();
            }

            if (!IsPostBack)
            {
                this.txtFechaDesde.Text = fechaD;
                this.txtFechaHasta.Text = fechaH;

                cargarSucursales();
            }

            if (accion == 1)
                CargarFacturasMercaderiasDiferencias();
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
                int tienePermiso = 0;
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "164")
                            tienePermiso = 1;

                        if (tienePermiso == 1)
                        {
                            if (s == "166")
                            {
                                this.DropListSucursalDestino.Enabled = true;
                            }
                        }
                    }
                }

                if (tienePermiso == 1)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        protected void PrimeraCarga()
        {
            try
            {
                fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                sucursalDestino = Convert.ToInt32(Session["Login_SucUser"]);

                FiltrarDiferenciaMercaderia();
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al hacer la primera carga de diferencias mercaderias. " + ex.Message);
            }
        }

        protected void FiltrarDiferenciaMercaderia()
        {
            try
            {
                Response.Redirect("DiferenciasMercaderiaF.aspx?a=1&sd=" + sucursalDestino + "&so=" + sucursalOrigen + "&fd=" + fechaD + "&fh=" + fechaH);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al filtrar. " + ex.Message);
            }
        }

        public void cargarSucursales()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                DataRow dr = dt.NewRow();
                dr["nombre"] = "Todas";
                dr["id"] = 0;
                dt.Rows.InsertAt(dr, 0);

                this.DropListSucursalOrigen.DataSource = dt;
                this.DropListSucursalOrigen.DataValueField = "Id";
                this.DropListSucursalOrigen.DataTextField = "nombre";
                this.DropListSucursalOrigen.DataBind();

                this.DropListSucursalDestino.DataSource = dt;
                this.DropListSucursalDestino.DataValueField = "Id";
                this.DropListSucursalDestino.DataTextField = "nombre";
                this.DropListSucursalDestino.DataBind();

                this.DropListSucursalDestino.SelectedValue = Session["Login_SucUser"].ToString();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }

        public void CargarFacturasMercaderiasDiferencias()
        {
            try
            {
                phFacturas.Controls.Clear();

                var diferencias = contFactEntity.ObtenerFacturasMercaderiasDiferencias();

                foreach (var diferencia in diferencias)
                {
                    cargarEnPh(diferencia);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al cargar facturas mercaderias diferencias. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al cargar facturas mercaderias diferencias. " + ex.Message);
            }
        }

        private void cargarEnPh(FacturasMercaderias_Diferencias f)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();
                var factura = contFacturacion.obtenerFacturaId((int)f.FacturasMercaderias_Detalle.Facturas_Mercaderias.Factura);
                //fila
                TableRow tr = new TableRow();
                //tr.ID = f["id"].ToString();

                //Celdas
                TableCell celFecha = new TableCell();
                DateTime date = Convert.ToDateTime(f.FacturasMercaderias_Detalle.Facturas_Mercaderias.FechaFactura);
                celFecha.Text = date.ToString("dd/MM/yyyy");
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celFecha);

                TableCell celNumeroFactura = new TableCell();
                celNumeroFactura.Text = factura.numero;
                celNumeroFactura.HorizontalAlign = HorizontalAlign.Left;
                celNumeroFactura.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumeroFactura);

                TableCell celSucursalOrigen = new TableCell();
                celSucursalOrigen.Text = contSucursal.obtenerSucursalID(factura.sucursal.id).nombre;
                celSucursalOrigen.HorizontalAlign = HorizontalAlign.Left;
                celSucursalOrigen.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalOrigen);

                TableCell celSucursalDestino = new TableCell();
                celSucursalDestino.Text = contSucursal.obtenerSucursalID(factura.sucursalFacturada).nombre;
                celSucursalDestino.HorizontalAlign = HorizontalAlign.Left;
                celSucursalDestino.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSucursalDestino);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = f.FacturasMercaderias_Detalle.itemsFactura.articulo1.descripcion;
                celArticulo.HorizontalAlign = HorizontalAlign.Left;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell celEnviado = new TableCell();
                celEnviado.Text = f.FacturasMercaderias_Detalle.CantidadEnviada.ToString();
                celEnviado.HorizontalAlign = HorizontalAlign.Left;
                celEnviado.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celEnviado);

                TableCell celRecibido = new TableCell();
                celRecibido.Text = f.FacturasMercaderias_Detalle.CantidadRecibida.ToString();
                celRecibido.HorizontalAlign = HorizontalAlign.Left;
                celRecibido.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celRecibido);

                TableCell celDiferencia = new TableCell();
                celDiferencia.Text = f.FacturasMercaderias_Detalle.Diferencia.ToString();
                celDiferencia.HorizontalAlign = HorizontalAlign.Left;
                celDiferencia.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDiferencia);

                //TableCell celEstado = new TableCell();
                //celEstado.Text = contFactEntity.ObtenerFacturasMercaderias_EstadoByID(Convert.ToInt32(f["Estado"].ToString())).Descripcion;
                //celEstado.HorizontalAlign = HorizontalAlign.Left;
                //celEstado.VerticalAlign = VerticalAlign.Middle;
                //tr.Cells.Add(celEstado);

                //TableCell celAccion = new TableCell();

                //Literal lAccept = new Literal();
                //lAccept.ID = "btnFactura_" + f["id"].ToString();
                //lAccept.Text = "<a href=\"AceptarMercaderia.aspx?fc=" + f["id"].ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Editar\" style =\"font-size:12pt\"> ";
                //lAccept.Text += "<span class=\"shortcut-icon icon-search\"></span>";
                //lAccept.Text += "</a>";

                //celAccion.Controls.Add(lAccept);

                //tr.Cells.Add(celAccion);

                phFacturas.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando diferencias de mercaderia en el PH. " + ex.Message));
            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            CargarVariableConValoresDeFiltro();
            FiltrarDiferenciaMercaderia();
        }
        protected void CargarVariableConValoresDeFiltro()
        {
            try
            {
                fechaD = txtFechaDesde.Text;
                fechaH = txtFechaHasta.Text;
                sucursalOrigen = Convert.ToInt32(DropListSucursalDestino.SelectedValue);
                sucursalDestino = Convert.ToInt32(DropListSucursalDestino.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "ERROR", "Error al hacer la primera carga de diferencias mercaderias. " + ex.Message);
            }
        }
    }
}