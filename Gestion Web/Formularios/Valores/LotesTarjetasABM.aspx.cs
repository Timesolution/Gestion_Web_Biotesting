using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class LotesTarjetasABM : System.Web.UI.Page
    {
        controladorTarjeta contTarjetas = new controladorTarjeta();
        Mensajes m = new Mensajes();
        
        int accion;
        int tarjeta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                accion = Convert.ToInt32(Request.QueryString["a"]);
                tarjeta = Convert.ToInt32(Request.QueryString["t"]);

                if (!IsPostBack)
                {
                    this.cargarDatosIniciales();
                }
                if (Convert.ToInt32(this.DropListPosnet.SelectedValue) > 0)
                {
                    this.cargarTarjetasPantalla();
                    this.obtenerUltimoNumero();
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando pagina " + ex.Message));
            }
        }

        private void cargarDatosIniciales()
        {
            try
            {
                this.cargarPosnets();                

                this.txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en carga inicial de pagina " + ex.Message));
            }
        }
        public void cargarPosnets()
        {
            try
            {
                List<Posnet> lstPosnets = this.contTarjetas.ObtenerPosnets();

                //agrego Seleccione
                ListItem item = new ListItem("Seleccione...", "-1");
                
                //modalbusqueda
                this.DropListPosnet.DataSource = lstPosnets;
                this.DropListPosnet.DataValueField = "Id";
                this.DropListPosnet.DataTextField = "Nombre";
                this.DropListPosnet.DataBind();
                this.DropListPosnet.Items.Insert(0, item);
                }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando posnets. " + ex.Message));
            }
        }
        public void cargarTarjetas()
        {
            try
            {
                if (Convert.ToInt32(this.DropListPosnet.SelectedValue) > 0)
                {
                    Posnet p = this.contTarjetas.ObtenerPosnetByID(Convert.ToInt32(this.DropListPosnet.SelectedValue));

                    this.ListTarjetas.DataSource = p.Posnet_Tarjetas;
                    this.ListTarjetas.DataValueField = "Id";
                    this.ListTarjetas.DataTextField = "Nombre";

                    this.ListTarjetas.DataBind();
                }

                //agrego Seleccione
                ListItem item = new ListItem("Seleccione...", "-1");
                this.ListTarjetas.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tarjetas. " + ex.Message));
            }
        }
        public void obtenerUltimoNumero()
        {
            try
            {
                int pos = Convert.ToInt32(this.DropListPosnet.SelectedValue);
                //int tar = Convert.ToInt32(this.ListTarjetas.SelectedValue);
                string numero = this.contTarjetas.obtenerUltimoNumeroLotePosnet(pos);

                if (!String.IsNullOrEmpty(numero))
                {
                    this.txtNumero.Text = (Convert.ToInt32(numero) + 1).ToString().PadLeft(4, '0');
                }
                else
                {
                    this.txtNumero.Text = "0000";
                    this.txtNumero.Attributes.Remove("disabled");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando ultimo nro de lote. " + ex.Message));
            }
        }

        public void cargarTarjetasPantalla()
        {
            try
            {
                Posnet p = this.contTarjetas.ObtenerPosnetByID(Convert.ToInt32(this.DropListPosnet.SelectedValue));
                foreach (Posnet_Tarjetas t in p.Posnet_Tarjetas)
                {
                    CamposLotesTarjetas tarjeta = (CamposLotesTarjetas)Page.LoadControl("../../Controles/CamposLotesTarjetas.ascx");
                    tarjeta.lblCampo.InnerText = t.Nombre;
                    tarjeta.ID = t.Id.ToString();

                    this.phTarjetas.Controls.Add(tarjeta);
                }
            }
            catch
            {

            }
        }

        #region funciones cierre
        
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Tarjetas_Lotes> lstLotes = new List<Tarjetas_Lotes>();
                foreach (CamposLotesTarjetas tarjeta in phTarjetas.Controls)
                {
                    Tarjetas_Lotes lote = new Tarjetas_Lotes();

                    lote.FechaCierre = Convert.ToDateTime(this.txtFecha.Text, new CultureInfo("es-AR"));
                    lote.Cupones = Convert.ToInt32(tarjeta.txtCupones.Text);
                    lote.Importe = Convert.ToDecimal(tarjeta.txtImporte.Text);
                    lote.NumeroLote = this.txtNumero.Text;
                    lote.IdPosnet = Convert.ToInt32(this.DropListPosnet.SelectedValue);
                    lote.IdTarjetaPosnet = Convert.ToInt32(tarjeta.ID);

                    lstLotes.Add(lote);                                        
                }

                int i = this.contTarjetas.AgregarLotesTarjeta(lstLotes);

                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Cierre de lote agregado con exito!. \", {type: \"info\"});location.href='LotesTarjetasF.aspx';", true);    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo agregar cierre de lote. \");", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error agregando cierre de lote. " + ex.Message + ". \", {type: \"error\"});", true);    
            }
        }      

        #endregion

        
    }
}