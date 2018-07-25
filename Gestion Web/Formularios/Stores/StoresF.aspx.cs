using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Stores
{
    public partial class StoresF1 : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        private int idUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                if (!IsPostBack)
                {
                    this.idUsuario = (int)Session["Login_IdUser"];
                    
                    this.cargarStoresTablaDT();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                    //if(this.contUser.validarAcceso(this.idUsuario,"Maestro.Articulos.Grupos") != 1)
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
                    //if (!String.IsNullOrEmpty(s))
                    //{
                    //    if (s == "15")
                    //    {
                    return 1;
                    //    }
                    //}
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarStoresTablaDT()
        {
            try
            {
                controladorStore contStore = new controladorStore();
                List<Store> stores = contStore.ObtenerStores();
                //vacio place holder
                this.phArticulos.Controls.Clear();

                foreach (var row in stores)
                {
                    this.cargarStorePH(row);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando Stores dt. " + ex.Message));
            }
        }
        private void cargarStorePH(Store row)
        {
            try
            {
                //Agrego las celdas que seleccione en la configuracion de visualizacion.                    
                //VisualizacionArticulos vista = new VisualizacionArticulos();

                //Gestion_Api.Entitys.articulo artEnt = this.contArtEnt.obtenerArticuloEntity(Convert.ToInt32(row["id"]));

                //Celdas
                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = row.Descripcion;
                celDescripcion.Width = Unit.Percentage(5);
                celDescripcion.VerticalAlign = VerticalAlign.Middle;

                TableCell celDetalle = new TableCell();
                celDetalle.Text = row.Detalle;
                celDetalle.Width = Unit.Percentage(5);
                celDetalle.VerticalAlign = VerticalAlign.Middle;

                TableCell celAction = new TableCell();
                celAction.Width = Unit.Percentage(20);

                Literal lDetail = new Literal();
                lDetail.ID = row.Id.ToString();

                lDetail.Text = "<a href=\"StoresABM.aspx?accion=2&idStore=" + row.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >";
                lDetail.Text += "<i class=\"shortcut-icon icon-search\"></i>";
                lDetail.Text += "</a>";
                celAction.Controls.Add(lDetail);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l);

                Literal lBanner = new Literal();
                lBanner.ID = row.Id.ToString();
                celAction.Controls.Add(lBanner);

                lBanner.Text = "<a href=\"StoresBanners.aspx?idStore=" + row.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Modificar Banners\" >";
                //lDetail.Text += "style=\"width: 100%\">";
                lBanner.Text += "<i class=\"shortcut-icon icon-list-alt\"></i>";
                lBanner.Text += "</a>";                

                Literal l2 = new Literal();
                l.Text = "&nbsp";
                celAction.Controls.Add(l2);

                TableRow tr = new TableRow();
                ////tr.ID = art.id + "1";
                //tr.ID = "tr_" + row.Id.ToString();
                //if (Convert.ToInt32(row["apareceLista"]) == 0)
                //{
                //    tr.ForeColor = System.Drawing.Color.Red;
                //}

                //arego fila a tabla
                //table.Controls.Add(tr);
                tr.Cells.Add(celDescripcion);
                tr.Cells.Add(celDetalle);
                tr.Cells.Add(celAction);
                

                //if (!String.IsNullOrEmpty(oferta))
                //{
                //    this.LitReferencia.Visible = true;
                //    tr.ForeColor = System.Drawing.Color.ForestGreen;
                //}

                this.phArticulos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error cargando stores en PH. " + ex.Message));
            }
        }
    }
}