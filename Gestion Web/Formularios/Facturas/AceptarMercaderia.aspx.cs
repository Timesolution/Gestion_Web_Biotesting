﻿using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class AceptarMercaderia : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorFacturacion contFacturacion = new controladorFacturacion();
        controladorFactEntity contFactEntity = new controladorFactEntity();
        int fc = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();

            fc = Convert.ToInt32(Request.QueryString["fc"]);

            if (!IsPostBack)
            {
                CargarDatosDeFactura();                
            }

            CargarItemsFacturaEnPH();
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

                return 1;

                //foreach (string s in listPermisos)
                //{
                //    if (!String.IsNullOrEmpty(s))
                //    {
                //        if (s == "28")
                //        {
                //            return 1;
                //        }
                //    }
                //}

                //return 0;
            }
            catch
            {
                return -1;
            }
        }

        public void CargarDatosDeFactura()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                var factura = contFacturacion.obtenerFacturaId(fc);
                var sucursalOrigen = contSucu.obtenerSucursalID(factura.sucursal.id);
                var sucursalDestino = contSucu.obtenerSucursalID(factura.sucursalFacturada);

                ListSucursalOrigen.Items.Add(new ListItem
                {
                    Value = sucursalOrigen.id.ToString(),
                    Text = sucursalOrigen.nombre
                });
                ListSucursalOrigen.CssClass = "form-control";

                ListSucursalDestino.Items.Add(new ListItem
                {
                    Value = sucursalDestino.id.ToString(),
                    Text = sucursalDestino.nombre
                });
                ListSucursalDestino.CssClass = "form-control";

                string[] numero = factura.numero.Split('-');
                numero[0] = numero[0].Replace("-", string.Empty);

                txtPVenta.Text = numero[0];
                txtNumero.Text = numero[1];
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al cargar los datos de la factura en pantalla " + ex.Message);
            }
        }

        public void CargarItemsFacturaEnPH()
        {
            try
            {
                var items = contFacturacion.obtenerItemsFact(fc);

                foreach (var item in items)
                {
                    cargarEnPh(item);
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1,"Error","Error al cargar los items de la factura en el PH " + ex.Message);
            }
        }

        private void cargarEnPh(ItemFactura fi)
        {
            try
            {
                controladorSucursal contSucursal = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();

                //fila
                TableRow tr = new TableRow();
                tr.ID = fi.Id.ToString();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = fi.articulo.codigo;
                celCodigo.HorizontalAlign = HorizontalAlign.Left;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = fi.articulo.descripcion;
                celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                int cantidad = Convert.ToInt32(fi.cantidad);
                celCantidad.Text = cantidad.ToString();
                celCantidad.HorizontalAlign = HorizontalAlign.Left;
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);

                TableCell celAccion = new TableCell();

                TextBox celCantidadRecibida = new TextBox();
                celCantidadRecibida.TextMode = TextBoxMode.Number;
                celCantidadRecibida.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                celCantidadRecibida.Text = cantidad.ToString();

                celAccion.Controls.Add(celCantidadRecibida);

                tr.Cells.Add(celAccion);

                phProductos.Controls.Add(tr);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando order de reparacion. " + ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in phProductos.Controls)
                {
                    TableRow tr = item as TableRow;
                    TextBox txtCantidadRecibidaTB = tr.Cells[3].Controls[0] as TextBox;
                    int cantidadEnviada = Convert.ToInt32(tr.Cells[2].Text);
                    int cantidadRecibida = Convert.ToInt32(txtCantidadRecibidaTB.Text);

                    if (cantidadEnviada == cantidadRecibida)
                    {
                        contFactEntity.AgregarFacturasMercaderiasDetalles(Convert.ToInt32(tr.ID), cantidadEnviada, cantidadRecibida);
                    }
                }

                int temp = contFactEntity.GuardarFacturasMercaderiasDetalles();

                if(temp > 0)
                {
                    Log.EscribirSQL(1, "Info", "Se guardaron los detalles de la mercaderia con exito");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Mercaderia aceptada con exito!", "FacturasMercaderiasF.aspx"));
                }
                else
                {
                    Log.EscribirSQL(1, "Error", "Error al guardar los detalles de la mercaderia con exito");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al guardar los detalles de la mercaderia!"));
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL(1, "Error", "Error al aceptar la mercaderia " + ex.Message);
            }
        }
    }
}