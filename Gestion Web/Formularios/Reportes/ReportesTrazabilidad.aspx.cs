using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestion_Web.Controles;
using Gestor_Solution.Controladores;
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
    public partial class ReportesTrazabilidad : System.Web.UI.Page
    {
        controladorCompraEntity contCompra = new controladorCompraEntity();
        controladorSucursal contSucu = new controladorSucursal();
        controladorArticulo contArticulos = new controladorArticulo();

        Mensajes m = new Mensajes();
        DataTable dtItemsTemp;

        private int idSucursal;
        private int idGrupo;
        private int idArticulo;
        private int estado;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.idGrupo = Convert.ToInt32(Request.QueryString["g"]);
                this.idSucursal = Convert.ToInt32(Request.QueryString["s"]);
                this.idArticulo = Convert.ToInt32(Request.QueryString["art"]);
                this.estado = Convert.ToInt32(Request.QueryString["e"]);

                this.VerificarLogin();

                if (!IsPostBack)
                {
                    this.cargarGruposArticulos();
                    this.cargarSucursal();
                    this.cargarArticulos();
                    this.cargarLabel();
                    dtItemsTemp = new DataTable();
                }
                if (idGrupo > 0)
                {
                    this.cargarCamposGrupo();
                    this.CargarItems();
                }  
                    
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error. " + ex.Message));
            }
        }

        #region carga inicial
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
                            //verifico si es super admin
                            string perfil = Session["Login_NombrePerfil"] as string;
                            //if (perfil == "SuperAdministrador")
                            //{
                            //    this.DropListSucursal.Attributes.Remove("disabled");
                            //}
                            //else
                            //{
                            //    this.DropListSucursal.SelectedValue = Session["Login_SucUser"].ToString();
                            //}
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                DataRow dr2 = dt.NewRow();
                dr2["nombre"] = "Todas";
                dr2["id"] = 0;
                dt.Rows.InsertAt(dr2, 1);

                //modalbusqueda
                this.DropListSucursal.DataSource = dt;
                this.DropListSucursal.DataValueField = "Id";
                this.DropListSucursal.DataTextField = "nombre";
                this.DropListSucursal.DataBind();

                this.DropListSucursal.SelectedValue = this.idSucursal.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        private void cargarGruposArticulos()
        {
            try
            {

                DataTable dt = this.contArticulos.obtenerGruposArticulos();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["descripcion"] = "Seleccione";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListGrupo.DataSource = dt;
                this.DropListGrupo.DataValueField = "id";
                this.DropListGrupo.DataTextField = "descripcion";

                this.DropListGrupo.DataBind();



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando grupos de articulos a la lista. " + ex.Message));
            }
        }
        private void cargarArticulos()
        {
            try
            {
                this.DropListArticulos.Items.Clear();
                if (Convert.ToInt32(this.DropListGrupo.SelectedValue) > 0)
                {
                    int grupo = Convert.ToInt32(this.DropListGrupo.SelectedValue) ;
                    List<Articulo> lstArticulos = this.contArticulos.filtrarArticulosGrupoSubGrupo(grupo,0,0,"",0,"");
                    
                    ListItem todos = new ListItem("Todos","-1");
                    DropListArticulos.Items.Add(todos);

                    foreach (Articulo art in lstArticulos)
                    {                        
                        ListItem item = new ListItem(art.descripcion, art.id.ToString());
                        DropListArticulos.Items.Add(item);
                    }

                    
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando articulos a la lista. " + ex.Message));
            }
        }

        private void cargarLabel()
        {
            try
            {
                if (this.idGrupo > 0)
                {
                    this.lblParametros.Text += this.DropListGrupo.Items.FindByValue(this.idGrupo.ToString()).Text;
                }
                else
                {
                    this.lblParametros.Text += "TODOS";
                }
                if (this.idSucursal > 0)
                {
                    this.lblParametros.Text += "," + this.DropListSucursal.Items.FindByValue(this.idSucursal.ToString()).Text;
                }
                else
                {
                    this.lblParametros.Text += ",TODAS";
                }
                if (this.idArticulo > 0)
                {
                    this.lblParametros.Text += "," + this.contArticulos.obtenerArticuloByID(this.idArticulo).descripcion;
                }
                else
                {
                    this.lblParametros.Text += ",TODOS";
                }



            }
            catch
            {

            }
        }
        #endregion
        private void cargarCamposGrupo()
        {
            try
            {
                List<Trazabilidad_Campos> lstCampos = this.contArticulos.obtenerCamposTrazabilidadByGrupo(this.idGrupo);

                foreach (Trazabilidad_Campos campos in lstCampos)
                {
                    TableHeaderCell th = new TableHeaderCell();
                    th.Text = campos.nombre;
                    phTabla.Controls.Add(th);

                }
                this.CrearTablaItems(lstCampos);

            }
            catch (Exception ex)
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
                DataTable dt = this.contCompra.obtenerTrazabilidadItemByArticuloGrupo(this.idGrupo,this.idArticulo, this.idSucursal,this.estado);
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
                        celIndice.Text = (Convert.ToInt32(row["traza"]) + 1).ToString();//(pos + 1).ToString();
                        celIndice.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celIndice);

                        Articulo arti = this.contArticulos.obtenerArticuloByID(Convert.ToInt32(row["idArticulo"]));
                        TableCell celCodigo = new TableCell();
                        celCodigo.Text = arti.codigo;
                        celCodigo.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celCodigo);

                        TableCell celDescripcion = new TableCell();
                        celDescripcion.Text = arti.descripcion;
                        celDescripcion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celDescripcion);
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
                        TableCell celEstado = new TableCell();
                        if (row["estado"].ToString() == "1")
                        {
                            celEstado.Text = "EN STOCK";
                        }
                        if (row["estado"].ToString() == "2")
                        {
                            celEstado.Text = "VENDIDO";
                        }
                        if (row["estado"].ToString() == "3")
                        {
                            celEstado.Text = "TOMADA PEDIDO";
                        }
                        tr.Cells.Add(celEstado);

                        TableCell celSucursal = new TableCell();
                        celSucursal.Text = row["sucursal"].ToString();                        
                        tr.Cells.Add(celSucursal);

                        TableCell celAccion = new TableCell();

                        LinkButton btnDetalle = new LinkButton();
                        btnDetalle.CssClass = "btn btn-info";
                        btnDetalle.ID = "btnDetalle_" + row["idArticulo"].ToString() + "_" + row["traza"].ToString() + "_" + row["id"].ToString();
                        btnDetalle.Text = "<span class='shortcut-icon icon-search'></span>";
                        btnDetalle.Click += new EventHandler(this.DetalleTraza);
                        celAccion.Controls.Add(btnDetalle);

                        Literal l2 = new Literal();
                        l2.Text = "&nbsp";
                        celAccion.Controls.Add(l2);

                        LinkButton btnEditar = new LinkButton();
                        btnEditar.CssClass = "btn btn-info";
                        btnEditar.ID = "btnEditar_" + row["idArticulo"].ToString()+ "_" + row["traza"].ToString() + "_" + row["id"].ToString();
                        btnEditar.Text = "<span class='shortcut-icon icon-edit'></span>";
                        btnEditar.Click += new EventHandler(this.EditarTraza);
                        celAccion.Controls.Add(btnEditar);
                        //celAccion.Width = Unit.Percentage(15);
                        celAccion.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(celAccion);

                        columnas = 0;
                        pos++;
                        idTrazas = "";
                        phTrazabilidad.Controls.Add(tr);

                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando items. " + ex.Message));
            }
        }

        private void EditarTraza(object sender, EventArgs e)
        {
            try
            {
                //obtengo indice de la fila a borrar
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                string traza = codigo[2];
                string art = codigo[1];

                Response.Redirect("TrazabilidadABM.aspx?t=" + traza + "&g=" + this.idGrupo + "&a=" + art +"&s="+this.idSucursal);

            }
            catch (Exception ex)
            {                
            }
        }

        private void DetalleTraza(object sender, EventArgs e)
        {
            try
            {
                //obtengo indice de la fila a borrar
                string[] codigo = (sender as LinkButton).ID.Split(new Char[] { '_' });
                string traza = codigo[2];
                string art = codigo[1];

                Response.Redirect("TrazabilidadABM.aspx?valor=2&t=" + traza + "&g=" + this.idGrupo + "&a=" + art + "&s=" + this.idSucursal + "&e=" + this.estado);
            }
            catch
            {

            }
        }

        protected void lbtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ReportesTrazabilidad.aspx?g=" + this.DropListGrupo.SelectedValue + "&s=" + this.DropListSucursal.SelectedValue + "&art=" + this.DropListArticulos.SelectedValue + "&e="+this.DropListEstado.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado de impagas. " + ex.Message));

            }
        }

        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                String buscar = this.txtDescArticulo.Text.Replace(' ', '%');
                DataTable dt = this.contArticulos.obtenerArticulosByDescDT(buscar);

                //cargo la lista
                this.DropListArticulos.DataSource = dt;
                this.DropListArticulos.DataValueField = "id";
                this.DropListArticulos.DataTextField = "descripcion";
                this.DropListArticulos.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void DropListGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarArticulos();
            }
            catch
            {

            }
        }

        protected void lbtnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImpresionReporte.aspx?valor=8&g="+this.idGrupo+"&a="+this.idArticulo+"&s="+this.idSucursal+"&es="+this.estado);
        }

        
    }
}