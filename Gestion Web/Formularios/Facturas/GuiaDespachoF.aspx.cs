using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
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

namespace Gestion_Web.Formularios.Facturas
{
    public partial class GuiaDespachoF : System.Web.UI.Page
    {
        controladorDespacho controladorDespachos = new controladorDespacho();
        controladorSucursal contrSucu = new controladorSucursal();
        controladorFacturacion controladorFacturas = new controladorFacturacion();
        controladorCliente cl = new controladorCliente();
        ControladorExpreso ex = new ControladorExpreso();
        Mensajes m = new Mensajes();
        Decimal valor = 0;
        String idFacturas;

        DataTable dtDespachoTemp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                idFacturas = Request.QueryString["f"];
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    txtFechaDespacho.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    dtDespachoTemp = new DataTable();
                    cargarValores();
                    InicializarTableDespacho();
                 
                }
                else
                {
                    cargarTablaBultos();
                }
                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
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
                        if (s == "39")
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
        private void InicializarTableDespacho()
        {
            try
            {
                dtDespachoTemp.Columns.Add("Tipo");
                dtDespachoTemp.Columns.Add("Cantidad");
                dtDespacho = dtDespachoTemp;
            }
            catch(Exception ex){
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error inicializanco tabla temporal " + ex.Message));
            }
        
        }
        protected DataTable dtDespacho
        {

            get
            {
                if (ViewState["dtDespacho"] != null)
                {
                    return (DataTable)ViewState["dtDespacho"];
                }
                else
                {
                    return dtDespachoTemp;
                }
            }
            set
            {
                ViewState["dtDespacho"] = value;
            }
        }
        private void cargarTablaBultos()
        {
            try
            {

                DataTable dt = this.dtDespacho;

                //limpio el Place holder
                this.phDespacho.Controls.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    //que me cargue la tabla
                    int pos = dt.Rows.IndexOf(dr);
                    this.cargarEnPHBultos(dr, pos);

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando tabla bultos " + ex.Message));               
            }
        }
        private void cargarValores()
        {
            try
            {
                int i = 0;
                Decimal saldo = 0;
                String nombreExpreso;

                foreach (var item in idFacturas.Split(';'))
                {
                    if (!String.IsNullOrEmpty(idFacturas.Split(';')[i]))
                    {
                        Factura f = controladorFacturas.obtenerFacturaId(Convert.ToInt32(idFacturas.Split(';')[i]));
                        TipoDocumento nroDespacho = controladorFacturas.obtenerFacturaNumero(f.ptoV.id,22);                        
                            
                        nombreExpreso = obtenerExpresoCliente(f.cliente.id);
                        this.txtTransportes.Text = nombreExpreso;

                        String Nro = (nroDespacho.idNumeracion).ToString("000000");
                        txtNroDespacho.Text = Nro;

                        saldo += f.total;
                        i++;
                        txtValorDespacho.Text = saldo.ToString();

                    }
                    
                }

            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error valores en el formulario " + ex.Message));
            }
        }
        private String obtenerExpresoCliente(int idCliente)
        {
            try
            {
                ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
                var exp = contClienteEntity.obtenerExpresoCliente(idCliente);
                String expreso = "";
                if (exp != null)
                {
                    expreso = exp.nombre;
                    //cargarDatosExpreso(e.id);
                }
                //if (exp.Count > 0 & exp != null)
                //{
                //    var e = exp.FirstOrDefault();
                //    expreso = e.nombre;
                //    //cargarDatosExpreso(e.id);
                //}
                return expreso;
            }
            catch (Exception ex)
            {
                return "";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando decuentos. " + ex.Message));
            }
        }
        protected void btnAgregarDespacho_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDespachoTemporal = this.dtDespacho;
                DataRow desp = dtDespachoTemporal.NewRow();
            
                desp[0] = txtTipoBulto.Text;            
                desp[1] = txtCantidadBulto.Text;
                dtDespachoTemporal.Rows.Add(desp);
                this.dtDespacho = dtDespachoTemporal;
                cargarTablaBultos();

                this.txtTipoBulto.Text = "";
                this.txtCantidadBulto.Text = "";
                this.txtTipoBulto.Focus();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error recargando tabla bultos " + ex.Message));
            }
            

        }
        private void cargarEnPHBultos(DataRow dr, int pos)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = phDespacho.Controls.Count.ToString();

                TableCell celTipo = new TableCell();
                celTipo.Text = dr["Tipo"].ToString();
                celTipo.Width = Unit.Percentage(10);
                celTipo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celTipo);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = dr["Cantidad"].ToString();
                celCantidad.Width = Unit.Percentage(10);
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);

                TableCell celAccion = new TableCell();

                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + pos;
                btnEliminar.CssClass = "btn btn-info";
                
                btnEliminar.Click += new EventHandler(this.QuitarBultoPH);
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                //accion sacar del list.

                celAccion.Controls.Add(btnEliminar);

                tr.Cells.Add(celAccion);

                phDespacho.Controls.Add(tr);
            }

            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando placeholder " + ex.Message));
            }

        }
        private void QuitarBultoPH(object sender, EventArgs e)
        {
            try
            {
                String pos = (sender as LinkButton).ID.Split('_')[1]; //.Substring(14, 3);.Substring(12, 1);                
                dtDespacho.Rows[Convert.ToInt32(pos)].Delete();
                this.cargarTablaBultos();
            }
            
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando bulto de la tabla " + ex.Message));
            }

        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try //agrego despacho y sus detalles
            {
                Despacho d = new Despacho();
                Factura f = controladorFacturas.obtenerFacturaId(Convert.ToInt32(idFacturas.Split(';')[0]));

                d.Valor = Convert.ToDecimal(this.txtValorDespacho.Text);
                d.Fecha = Convert.ToDateTime(this.txtFechaDespacho.Text, new CultureInfo("es-AR"));
                d.numero = this.txtNroDespacho.Text;
                d.expreso = txtTransportes.Text;
                d.contrareembolso = Convert.ToInt32(this.chkContraReembolso.Checked);
                d.estado = 1;
                d.sucursal = f.sucursal.id;

                foreach (DataRow row in dtDespacho.Rows)
                {
                    detalleDespacho detalle = new detalleDespacho();

                    detalle.TipoBulto = row[0].ToString();
                    detalle.Cantidad = Convert.ToInt32(row[1]);

                    d.detalleDespachoes.Add(detalle);

                }

                foreach (String f_id in idFacturas.Split(';'))
                {
                    if (!String.IsNullOrEmpty(f_id))
                    {
                        Facturas_Despachos factura = new Facturas_Despachos();

                        factura.Factura = Convert.ToInt32(f_id);

                        d.Facturas_Despachos.Add(factura);
                    }                    
                }

                int i = controladorDespachos.agregarDespacho(d);
                if (i > 0)
                {
                    btnAgregar.Visible = false;
                    btnVolver.Visible = true;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('../Facturas/ImpresionReportes.aspx?a=1&Desp=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("No se pudo guardar la guia de despacho"));
                }                
                

                //DataTable dtDatos = new DataTable();
                //dtDatos.Columns.Add("TipoBulto");
                //dtDatos.Columns.Add("Cantidad");

                //int cantBultos = 0;
                //foreach (var control in this.phDespacho.Controls)
                //{
                //    DataRow drDatos = dtDatos.NewRow();
                //    TableRow tr = control as TableRow;
                                        
                //    drDatos[0] = tr.Cells[0].Text;
                //    drDatos[1] = tr.Cells[1].Text;
                //    dtDatos.Rows.Add(drDatos);
                //    cantBultos += Convert.ToInt32(tr.Cells[1].Text);


                //}

                //Session.Add("detalleDesp", dtDatos);
                //Session.Add("numeroDespacho", this.txtNroDespacho.Text);
                //Session.Add("nombreTransporte", this.txtTransportes.Text);
                //Session.Add("cantTotal",cantBultos);
                
                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error Guardando despacho" + ex.Message));
            }
         
         
        }


    }
}