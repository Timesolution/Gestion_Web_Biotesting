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

namespace Gestion_Web.Formularios.Compras
{
    public partial class TrazabilidadF : System.Web.UI.Page
    {
        controladorCompraEntity controlador = new controladorCompraEntity();
        controladorArticulo contArticulos = new controladorArticulo();
        controladorCliente contCliente = new controladorCliente();

        DataTable dtItemsTemp;
        Mensajes m = new Mensajes();
                
        int idRemito;
        int idGrupo;
        int idArticulo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                this.idRemito = Convert.ToInt32(Request.QueryString["rc"]);
                this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                this.VerificarLogin();                

                if (!IsPostBack)
                {
                    dtItemsTemp = new DataTable();                    
                }

                this.cargarItemsRemito(idRemito);

                if (idArticulo > 0)
                {
                    this.obtenerGrupoTrazabilidad(idRemito,idArticulo);
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
                        if (s == "74")
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
        protected DataTable dtItems
        {
            get
            {
                if (ViewState["dtItems"] != null)
                {
                    return (DataTable)ViewState["dtItems"];
                }
                else
                {
                    return dtItemsTemp;
                }
            }
            set
            {
                ViewState["dtItems"] = value;
            }
        }

        private void cargarItemsRemito(int idRemito)
        {
            try
            {
                this.phItems.Controls.Clear();
                RemitosCompra rc = this.controlador.obtenerRemito(idRemito);

                if (rc != null)
                {
                    int pos = 0;
                    foreach (RemitosCompras_Items item in rc.RemitosCompras_Items)
                    {
                        this.CargarItemsRemitosPH(item, pos);
                        pos++;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando items remito. " + ex.Message));
            }
        }
        private void CargarItemsRemitosPH(RemitosCompras_Items item, int pos)
        {
            try
            {
                TableRow tr = new TableRow();
                Articulo art = this.contArticulos.obtenerArticuloByID(item.Codigo.Value);

                TableCell celCodigo = new TableCell();
                celCodigo.Text = art.codigo;
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = art.descripcion;
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celCantidad = new TableCell();
                celCantidad.Text = item.Cantidad.ToString();
                celCantidad.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCantidad);

                TableCell celAccion = new TableCell();
                LinkButton btnSeleccionar = new LinkButton();
                btnSeleccionar.CssClass = "btn btn-info";
                btnSeleccionar.ID = "btnSeleccionar_" + item.Codigo;
                btnSeleccionar.Text = "Seleccionar";
                btnSeleccionar.Click += new EventHandler(this.SeleccionItem);
                celAccion.Controls.Add(btnSeleccionar);
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                
                Literal l2 = new Literal();
                l2.Text = "&nbsp";
                celAccion.Controls.Add(l2);

                LinkButton btnEtiqueta = new LinkButton();
                btnEtiqueta.CssClass = "btn btn-info";
                btnEtiqueta.ID = "btnEtiqueta_" + item.Codigo;
                btnEtiqueta.Text = "<span class='shortcut-icon icon-list-alt'></span>";
                btnEtiqueta.Click += new EventHandler(this.ImprimirEtiqueta);
                celAccion.Controls.Add(btnEtiqueta);
                celAccion.Width = Unit.Percentage(5);
                celAccion.VerticalAlign = VerticalAlign.Middle;

                tr.Cells.Add(celAccion);

                phItems.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items remitos ph. " + ex.Message));
            }
        }
        
        private void obtenerGrupoTrazabilidad(int idRemito,int idArt)
        {
            try
            {                
                RemitosCompra rc = this.controlador.obtenerRemito(idRemito);

                if (rc != null)
                {
                    RemitosCompras_Items item = rc.RemitosCompras_Items.Where(x =>  x.Codigo == idArt).FirstOrDefault();                    
                    if (item != null)
                    {                        
                        Articulo art = this.contArticulos.obtenerArticuloByID(item.Codigo.Value);

                        this.lblCantidad.Text = Convert.ToInt32(item.Cantidad).ToString();
                        this.lblCodigo.Text = art.codigo;
                        this.lblDescripcion.Text = art.descripcion;

                        this.idGrupo = art.grupo.id;
                        this.cargarCamposGrupo();
                        this.CargarItems();
                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("No hay trazabilidad pendiente de articulos a cargar. ",""));
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void cargarCamposGrupo()
        {
            try
            {
                List<Trazabilidad_Campos> lstCampos = this.contArticulos.obtenerCamposTrazabilidadByGrupo(this.idGrupo);                

                foreach (Trazabilidad_Campos campos in lstCampos)
                {
                    CampoDinamico campo = (CampoDinamico)Page.LoadControl("../../Controles/CampoDinamico.ascx");
                    campo.lblCampo.InnerText = campos.nombre;
                    phCampos.Controls.Add(campo);

                    TableHeaderCell th = new TableHeaderCell();
                    th.Text = campos.nombre;
                    phTabla.Controls.Add(th);

                }
                this.CrearTablaItems(lstCampos);
            }
            catch(Exception ex)
            {

            }
        }
        private void CrearTablaItems(List<Trazabilidad_Campos> campos)
        {
            try
            {
                int indice = 1;
                foreach (Trazabilidad_Campos campo in campos)
                {
                    string nombreColumna = "Campo" + indice.ToString();
                    dtItemsTemp.Columns.Add(nombreColumna);
                    indice++;
                }                
                
                dtItems = dtItemsTemp;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error creando tabla de campos. " + ex.Message));
            }

        }        
        private void CargarItems()
        {
            try
            {
                this.phTrazabilidad.Controls.Clear();
                DataTable dt = this.controlador.obtenerTrazabilidadItemByRemito(this.idRemito,this.idArticulo);
                int pos = 0;
                int columnas = 0;
                TableRow tr = new TableRow();
                string idTrazas = "";

                foreach (DataRow row in dt.Rows)
                {
                    //this.cargarEnPH(row, pos);                    
                    if (columnas == 0)
                    {
                        tr = new TableRow();

                        TableCell celIndice = new TableCell();
                        celIndice.Text = (pos + 1).ToString();
                        celIndice.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celIndice);
                    }

                    if (columnas < this.dtItems.Columns.Count)
                    {
                        TableCell celCampo1 = new TableCell();
                        celCampo1.Text = row["valor"].ToString();
                        celCampo1.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celCampo1);
                        columnas++;
                        idTrazas += row["Id"].ToString() + ";";
                    }
                    if (columnas == (this.dtItems.Columns.Count))
                    {
                        TableCell celAccion = new TableCell();
                        LinkButton btnEliminar = new LinkButton();
                        btnEliminar.CssClass = "btn btn-info";
                        btnEliminar.ID = "btnEliminar_" + idTrazas;
                        btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                        btnEliminar.Click += new EventHandler(this.QuitarTraza);
                        celAccion.Controls.Add(btnEliminar);
                        celAccion.Width = Unit.Percentage(5);
                        celAccion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celAccion);

                        
                        columnas = 0;
                        pos++;
                        idTrazas = "";
                        phTrazabilidad.Controls.Add(tr);
                        
                    }                     
                }

                if (pos == Convert.ToDecimal(this.lblCantidad.Text))
                {
                    this.btnCargar.Visible = false;
                }

                this.lblCantCargada.Text = pos.ToString();
                
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }
        private void LimpiarCampos()
        {
            try
            {
                foreach (CampoDinamico txt in phCampos.Controls)
                {
                    txt.txtCampo.Text = "";
                }
            }
            catch
            {

            }
        }

        #region eventos controles
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                //por cada campodinamico es una traza
                int indiceRow = 0;
                Articulo a = this.contArticulos.obtenerArticuloByID(this.idArticulo);
                RemitosCompra rc = this.controlador.obtenerRemito(this.idRemito);

                int NroTraza = 0;
                int CantCargada = Convert.ToInt32(this.lblCantCargada.Text);
                
                NroTraza = this.contArticulos.obtenerUltimoNumeroTrazaArticulo(a.id);

                foreach (CampoDinamico txt in phCampos.Controls)
                {

                    Trazabilidad traza = new Trazabilidad();
                    Trazabilidad_Campos campo = this.contArticulos.obtenerCamposTrazabilidadByNombre(txt.lblCampo.InnerText, a.grupo.id);

                    traza.idArticulo = this.idArticulo;
                    traza.idCampo = campo.id;
                    traza.valor = txt.txtCampo.Text;
                    traza.estado = 1;
                    traza.Sucursal = rc.IdSucursal;
                    traza.Traza = NroTraza + 1;

                    rc.Trazabilidads.Add(traza);

                    int i = this.contArticulos.agregarTrazabilidad(traza, this.idRemito);
                    if (i < 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando traza nº " + indiceRow+1 ));
                        return;
                    }
                    
                    indiceRow++;
                }

                //this.controlador.modificarRemito(rc);
            
                //verifico si ya cargue todos
                decimal cant = Convert.ToDecimal(this.lblCantidad.Text);
                if (dtItems.Rows.Count == Convert.ToInt32(cant))
                {
                    this.btnCargar.Visible = false;
                }

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Traza cargada con exito!.", Request.Url.ToString()));
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Traza cargada con exito!. \", {type: \"info\"});location.href = '"+Request.Url.ToString()+"';", true);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }
        private void QuitarTraza(object sender, EventArgs e)
        {
            try
            {
                //obtengo indice de la fila a borrar
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                string[] ids = codigo[1].Split(';');

                foreach (string id in ids)
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        int i = this.controlador.anularTraza(this.idRemito, Convert.ToInt32(id));

                        if (i > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Traza anulada con exito!. \", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"No se pudo anular traza. \");", true);
                        }                
                    }
                }
                
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Ocurrio un error trantando de anular traza!. \", {type: \"error\"});", true);
            }
        }
        private void SeleccionItem(object sender, EventArgs e)
        {
            try
            {
                //obtengo indice de la fila a borrar
                string[] codigo = (sender as LinkButton).ID.Split('_');
                
                Response.Redirect("TrazabilidadF.aspx?rc="+this.idRemito+"&art="+codigo[1]);
            }
            catch
            {

            }
        }

        private void ImprimirEtiqueta(object sender, EventArgs e)
        {
            try
            {
                //obtengo indice de la fila a borrar
                string[] codigo = (sender as LinkButton).ID.Split('_');

                //Response.Redirect("ImpresionCompras.aspx?a=10&rc=" + this.idRemito + "&art=" + codigo[1]);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=10&rc=" + this.idRemito + "&art=" + codigo[1] + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);

                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "window.open('ImpresionCompras.aspx?a=10&g=" + this.idGrupo + "&rc=" + this.idRemito + "&art=" + codigo[1] + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');", true);
            }
            catch
            {

            }
        }

        #endregion

        protected void btnCargarPorCantidad_Click(object sender, EventArgs e)
        {
            try
            {
                decimal cant = Convert.ToDecimal(this.lblCantidad.Text);
                
                //verifico que la cantidad ingresada no sea mayor a lo ya cargado
                int cantidadTrazas = Convert.ToInt32(txtCantidadTrazas.Text);

                if (Convert.ToInt32(this.lblCantCargada.Text) + cantidadTrazas <= cant)
                {
                    //por cada campodinamico es una traza
                    int indiceRow = 0;
                    Articulo a = this.contArticulos.obtenerArticuloByID(this.idArticulo);
                    RemitosCompra rc = this.controlador.obtenerRemito(this.idRemito);

                    int NroTraza = 0;
                    int CantCargada = Convert.ToInt32(this.lblCantCargada.Text);

                    for(int j = 0; j<cantidadTrazas; j++)
                    {
                        NroTraza = this.contArticulos.obtenerUltimoNumeroTrazaArticulo(a.id);

                        foreach (CampoDinamico txt in phCampos.Controls)
                        {

                            Trazabilidad traza = new Trazabilidad();
                            Trazabilidad_Campos campo = this.contArticulos.obtenerCamposTrazabilidadByNombre(txt.lblCampo.InnerText, a.grupo.id);

                            traza.idArticulo = this.idArticulo;
                            traza.idCampo = campo.id;
                            traza.valor = txt.txtCampo.Text;
                            traza.estado = 1;
                            traza.Sucursal = rc.IdSucursal;
                            traza.Traza = NroTraza + 1;

                            rc.Trazabilidads.Add(traza);

                            int i = this.contArticulos.agregarTrazabilidad(traza, this.idRemito);
                            if (i < 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando traza nº " + indiceRow + 1));
                                return;
                            }

                            indiceRow++;
                        }
                    }

                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Traza cargada con exito!.", Request.Url.ToString()));
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"Traza cargada con exito!. \", {type: \"info\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("La cantidad ingresada es mayor a la permitida."));
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "alert", "$.msgbox(\"La cantidad ingresada es mayor a la permitida. \", {type: \"error\"});location.href = '" + Request.Url.ToString() + "';", true);
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando items. " + ex.Message));
            }
        }
    }
}