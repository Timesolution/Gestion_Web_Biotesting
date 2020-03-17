﻿using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CuentasBancariasABM : System.Web.UI.Page
    {
        ControladorBanco controlador = new ControladorBanco();
        Mensajes mje = new Mensajes();
        int accion;
        int id;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.accion = Convert.ToInt32(Request.QueryString["a"]);
                this.id = Convert.ToInt32(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    this.cargarBancos();
                    if (this.accion == 2)
                    {
                        this.cargarCuenta(this.id);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void cargarCuenta(int id)
        {
            try
            {
                CuentasBancaria cb = this.controlador.obtenerCuentaBancariaByID(id);
                this.ListBanco.SelectedValue = cb.Banco.Value.ToString();
                this.txtNumero.Text = cb.Numero;
                this.txtDescripcion.Text = cb.Descripcion;
                this.txtCuit.Text = cb.Cuit;
                this.txtLibrador.Text = cb.Librador;
            }
            catch
            {

            }
        }
        public void cargarBancos()
        {
            try
            {
                controladorCobranza contCobranza = new controladorCobranza();
                
                DataTable dt = contCobranza.obtenerBancosDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["entidad"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListBanco.DataSource = dt;
                this.ListBanco.DataValueField = "id";
                this.ListBanco.DataTextField = "entidad";
                this.ListBanco.DataBind();

                //txtCotizacion.Text = contCobranza.obtenerCotizacion(DropListTipo.DataTextField).ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando bancos a la lista. " + ex.Message));
            }
        }
        private void nuevaCuenta()
        {
            try
            {
                CuentasBancaria cb = new CuentasBancaria();
                cb.Banco = Convert.ToInt32(this.ListBanco.SelectedValue);
                cb.Numero = this.txtNumero.Text;
                cb.Descripcion = this.txtDescripcion.Text;
                cb.Cuit = this.txtCuit.Text;
                cb.Librador = this.txtLibrador.Text;

                int i = controlador.agregarCuenta(cb);
                if (i > 0)
                {
                    //limpiar
                    this.limpiarCampos();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Cuenta Cargada con exito. ", null));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Error cargando cuenta"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cuenta bancaria. " + ex.Message));
            }
        }
        private void modificarCuenta()
        {
            try
            {
                CuentasBancaria cb = this.controlador.obtenerCuentaBancariaByID(id);
                cb.Banco = Convert.ToInt32(this.ListBanco.SelectedValue);
                cb.Numero = this.txtNumero.Text;
                cb.Descripcion = this.txtDescripcion.Text;
                cb.Cuit = this.txtCuit.Text;
                cb.Librador = this.txtLibrador.Text;

                int i = controlador.modificarCuenta(cb);
                if (i > 0)
                {
                    //limpiar
                    this.limpiarCampos();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxInfo("Cuenta modificada con exito. ", "CuentasBancariasF.aspx"));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxAtencion("Error cargando cuenta"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando cuenta bancaria. " + ex.Message));
            }
        }
        private void limpiarCampos()
        {
            try
            {
                
                this.ListBanco.SelectedIndex = 0;
                this.txtNumero.Text = "";
                this.txtNumero.Text = "";
                this.txtCuit.Text = "";
                this.txtLibrador.Text = "";

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error limpiando campos. " + ex.Message));
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.accion==1)
                    this.nuevaCuenta();
                if (this.accion == 2)
                    this.modificarCuenta();
            }
            catch (Exception ex)
            {
 
            }
        }
    }
}