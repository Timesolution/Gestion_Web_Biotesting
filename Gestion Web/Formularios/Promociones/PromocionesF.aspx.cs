using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Promociones
{
    public partial class PromocionesF : System.Web.UI.Page
    {        
        controladorUsuario contUser = new controladorUsuario();
        controladorArticulo contArt = new controladorArticulo();
        ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
        controladorSucursal contSucu = new controladorSucursal();
        ControladorFormasPago contFP = new ControladorFormasPago();

        Mensajes m = new Mensajes();

        int accion;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.accion = Convert.ToInt32(Request.QueryString["a"]);

                if (!IsPostBack)
                {
                    if (this.accion > 0)
                    {
                        this.btnTodas.Visible = true;
                        this.btnVigentes.Visible = false;
                    }
                }
                this.cargarPromociones();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                        if (s == "89")
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
        public void cargarPromocion()
        {
            //try
            //{
            //    Promocione p = this.contArtEnt.obtenerPromocionByID(this.id);
            //    if (p != null)
            //    {
            //        this.txtPromocion.Text = p.Promocion;
            //    }
            //}
            //catch
            //{

            //}
        }
        public void cargarPromociones()
        {
            try
            {
                this.phPromos.Controls.Clear();

                List<Promocione> promo = this.contArtEnt.obtenerPromociones();
                foreach (var p in promo)
                {
                    this.cargarPromocionesPH(p);   
                }

            }
            catch(Exception ex)
            {

            }
        }
        public void cargarPromocionesPH(Promocione p)
        {
            try
            {
                TableRow tr = new TableRow();
                tr.ID = p.Id.ToString();

                TableCell celPromo = new TableCell();
                celPromo.Text = p.Promocion;
                celPromo.HorizontalAlign = HorizontalAlign.Left;
                celPromo.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celPromo);

                TableCell celDesde = new TableCell();
                celDesde.Text = p.Desde.Value.ToString("dd/MM/yyyy");
                celDesde.HorizontalAlign = HorizontalAlign.Left;
                celDesde.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celDesde);

                TableCell celHasta = new TableCell();
                celHasta.Text = p.Hasta.Value.ToString("dd/MM/yyyy");
                celHasta.HorizontalAlign = HorizontalAlign.Left;
                celHasta.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celHasta);

                //TableCell celEmpresa = new TableCell();
                //if (p.Empresa.Value > 0)
                //    celEmpresa.Text = this.contSucu.obtenerEmpresaID(p.Empresa.Value).RazonSocial;
                //else
                //    celEmpresa.Text = "TODAS";
                //celEmpresa.HorizontalAlign = HorizontalAlign.Left;
                //celEmpresa.VerticalAlign = VerticalAlign.Middle;
                //tr.Controls.Add(celEmpresa);

                TableCell celForma = new TableCell();
                if (p.FormaPago.Value > 0)
                    celForma.Text = this.contFP.obtenerFormaPagoEntByID(p.FormaPago.Value).forma;
                else
                    celForma.Text = "TODAS";
                celForma.HorizontalAlign = HorizontalAlign.Left;
                celForma.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celForma);

                TableCell celLista = new TableCell();
                if (p.ListaPrecio.Value > 0)
                    celLista.Text = this.contFP.obtenerListaPrecioEntByID(p.ListaPrecio.Value).nombre;
                else
                    celLista.Text = "TODAS";
                celLista.HorizontalAlign = HorizontalAlign.Left;
                celLista.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celLista);

                TableCell celDto = new TableCell();
                celDto.Text = p.Descuento.ToString() + "%";
                celDto.HorizontalAlign = HorizontalAlign.Right;
                celDto.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celDto);

                TableCell celPrecio = new TableCell();
                celPrecio.Text = p.PrecioFijo.Value.ToString("C");
                celPrecio.HorizontalAlign = HorizontalAlign.Right;
                celPrecio.VerticalAlign = VerticalAlign.Middle;
                tr.Controls.Add(celPrecio);

                TableCell celAction = new TableCell();
                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = "btnEditar_" + p.Id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                btnEditar.Click += new EventHandler(this.editarPromo);
                celAction.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + p.Id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.OnClientClick = "abrirdialog(" + p.Id + ");";
                celAction.Controls.Add(btnEliminar);
                celAction.Width = Unit.Percentage(10);
                celAction.VerticalAlign = VerticalAlign.Middle;
                celAction.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celAction);

                if (this.validarVigenciaPromo(p) == 0)
                {
                    tr.ForeColor = System.Drawing.Color.Red;
                    if(this.accion == 0)
                        this.phPromos.Controls.Add(tr);
                }
                else
                {
                    this.phPromos.Controls.Add(tr);
                }

                
            }
            catch(Exception ex)
            {

            }
        }
        public int validarVigenciaPromo(Promocione p)
        {
            try
            {
                if (DateTime.Today >= p.Desde && DateTime.Today <= p.Hasta)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                Promocione p = this.contArtEnt.obtenerPromocionByID(Convert.ToInt32(this.txtMovimiento.Text));
                p.Estado = 0;

                int i = this.contArtEnt.modificarPromocion(p);
                if (i > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Promocion eliminada con exito!. \", {type: \"info\"}); location.href = 'ABMPromociones.aspx';", true);
                    this.cargarPromociones();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo eliminar Promocion. \";", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo eliminar Promocion." + ex.Message + " .\";", true);
            }
        }
        private void editarPromo(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ABMPromociones.aspx?a=2&id=" + (sender as LinkButton).ID.Split('_')[1]);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar promocion. " + ex.Message));
            }
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPromociones.aspx?a=1&ex=0&v=" + this.accion + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        protected void lbtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ImpresionPromociones.aspx?a=1&ex=1&v=" + this.accion);
            }
            catch
            {

            }
        }
    }
}