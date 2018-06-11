using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Reportes
{
    public partial class TrazabilidadABM : System.Web.UI.Page
    {
        controladorCompraEntity controlador = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();
        
        Mensajes m = new Mensajes();

        int idTraza;
        int idGrupo;
        int idArticulo;
        int idSucursal;
        int accion;
        int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                this.idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                this.idArticulo = Convert.ToInt32(Request.QueryString["a"]);
                this.idTraza = Convert.ToInt32(Request.QueryString["t"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.accion = Convert.ToInt32(Request.QueryString["valor"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);
                this.VerificarLogin();                

                if (!IsPostBack)
                {
                    Articulo art = this.contArticulos.obtenerArticuloByID(this.idArticulo);
                    this.lblCodigo.Text = "Codigo: " + art.codigo;
                    this.lblArticulo.Text = art.descripcion;
                }

                this.cargarCamposGrupo();

                if(this.accion == 2)
                {
                    this.cargarDetalleTrazabilidad();
                }                
            }
            catch (Exception ex)
            {
 
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
                foreach (string s in listPermisos)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (s == "70")
                        {
                            return 1;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }
                
        private void cargarCamposGrupo()
        {
            try
            {
                List<Trazabilidad_Campos> lstCampos = this.contArticulos.obtenerCamposTrazabilidadByGrupo(this.idGrupo);
                List<Trazabilidad> lstTraza = this.contArticulos.ObtenerTrazabilidadByTraza(this.idTraza,this.idArticulo);

                foreach (Trazabilidad_Campos campos in lstCampos)
                {
                    CampoDinamico campo = (CampoDinamico)Page.LoadControl("../../Controles/CampoDinamico.ascx");
                    campo.lblCampo.InnerText = campos.nombre;
                    campo.ID = lstTraza.Where(x => x.idCampo == campos.id).FirstOrDefault().Id.ToString();//ID Traza
                    campo.txtCampo.Text = lstTraza.Where(x => x.idCampo == campos.id).FirstOrDefault().valor;//Valor campo traza
                    phCampos.Controls.Add(campo);

                }

            }
            catch(Exception ex)
            {

            }
        }

        public void cargarDetalleTrazabilidad()
        {
            try
            {
                controladorSucursal contSucur = new controladorSucursal();
                controladorFacturacion contFacturacion = new controladorFacturacion();
                controladorRemitos contRemitos = new controladorRemitos();

                List<Trazabilidad> lstTraza = this.contArticulos.ObtenerTrazabilidadByTraza(this.idTraza, this.idArticulo);
                List<Trazabilidad_Movimientos> lstMovimientos = this.contArticulos.ObtenerMovimientosTraza(this.idArticulo, this.idTraza);
                
                Sucursal sucOrigen = contSucur.obtenerSucursalID(lstTraza.FirstOrDefault().RemitosCompras.FirstOrDefault().IdSucursal.Value);
                Sucursal sucSalida = contSucur.obtenerSucursalID(lstTraza.FirstOrDefault().Sucursal.Value);                
                
                this.txtSucIngreso.Text = sucOrigen.nombre;
                this.txtRemitoIngreso.Text = "Remito Compra Nº " + lstTraza.FirstOrDefault().RemitosCompras.FirstOrDefault().Numero;
                this.txtSucEgreso.Text = sucSalida.nombre;
                this.txtFechaIngreso.Text = lstTraza.FirstOrDefault().RemitosCompras.FirstOrDefault().Fecha.Value.ToString("dd/MM/yyyy");
                if (lstTraza.FirstOrDefault().estado.Value == 2)
                {
                    var remito = lstMovimientos.Where(x => x.TipoDoc.Value == 14);
                    if (remito != null)
                    {
                        var ultimoRemito = remito.Where(x => x.Fecha == remito.Max(z => z.Fecha).Value).FirstOrDefault();
                        Remito r = contRemitos.obtenerRemitoId(Convert.ToInt32(ultimoRemito.Documento.Value));
                        this.txtRemitoEgreso.Text = "Remito Nº " + r.numero;

                        DataTable dtFact = contFacturacion.obtenerNroFacturaByRemito(Convert.ToInt32(ultimoRemito.Documento.Value));
                        string idFact = dtFact.Rows[0]["idFactura"].ToString();
                        Factura f = contFacturacion.obtenerFacturaId(Convert.ToInt32(idFact));
                        this.txtFactura.Text = f.tipo.tipo + " Nº " + f.numero;
                        this.txtCliente.Text = f.cliente.razonSocial;
                        this.txtFechaEgreso.Text = r.fecha.ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                    this.txtFechaEgreso.Text = "-";
                    this.txtFactura.Text = "-";
                    this.txtCliente.Text = "-";
                }
                this.panelDatos.Attributes.Add("style", "display:none;");
                this.panelDetalle.Visible = true;
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando detalle trazabilidad. " + ex.Message));
            }
        }
       

        #region eventos controles
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                bool error = false;
                foreach (CampoDinamico control in phCampos.Controls)
                {
                    string idTraza = control.ID;
                    string valor = control.txtCampo.Text;
                    int i = this.contArticulos.modificarTraza(Convert.ToInt32(idTraza),valor);

                    if (i < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Uno o mas campos no se pudieron modificar. \");", true);
                        error = true;
                        break;
                    }
                }

                if (error == false)
                {
                    Gestion_Api.Modelo.Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Modifico valor traza id: " + idTraza);
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Traza cargada con exito!. \", {type: \"info\"});location.href = 'ReportesTrazabilidad.aspx?s=" + this.idSucursal + "&g=" + this.idGrupo + "&art=" + this.idArticulo + "';", true);
                }
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportesTrazabilidad.aspx?g=" + this.idGrupo + "&s=" + this.idSucursal + "&art=" + this.idArticulo + "&e=" + this.estado);
        }

        #endregion

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ReportesTrazabilidad.aspx?g=" + this.idGrupo + "&s=" + this.idSucursal + "&art=" + this.idArticulo + "&e=" + this.estado);
            }
            catch { }
        }
    }
}