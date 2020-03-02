using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Compras
{
    public partial class RemitoDetalleF : System.Web.UI.Page
    {
        controladorCompraEntity cont = new controladorCompraEntity();
        controladorArticulo contArt = new controladorArticulo();
        controladorCliente contCL = new controladorCliente();
        

        long idRemito;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.idRemito = Convert.ToInt64(Request.QueryString["r"]);
                this.cargarRemito();
            }
            catch(Exception ex)
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

        private void cargarRemito()
        {
            try
            {
                var remito = this.cont.obtenerRemito(this.idRemito);

                controladorSucursal contsuc = new controladorSucursal();
                Sucursal suc = contsuc.obtenerSucursalID(remito.IdSucursal.Value);

                this.LitFecha.Text = Convert.ToDateTime(remito.Fecha).ToString("dd/MM/yyyy");
                this.LitNumero.Text = remito.Numero;
                this.LitSucursal.Text = suc.nombre;
                if (remito.RemitosCompras_Comentarios != null)
                {
                    this.LitComentario.Text = remito.RemitosCompras_Comentarios.Observacion;
                }
                if (remito.Tipo == 1)
                {
                    this.LitTipo.Text = "FC";
                }
                else
                {
                    this.LitTipo.Text = "PRP";
                }
                var p = this.contCL.obtenerProveedorID(Convert.ToInt32(remito.IdProveedor));
                if (p != null)
                {
                    this.LitProveedor.Text = p.razonSocial;
                }
                foreach (var item in remito.RemitosCompras_Items)
                {
                    this.cargarItemRemito(item);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        private void cargarItemRemito(RemitosCompras_Items item)
        {
            try
            {
                TableRow tr = new TableRow();

                var art = this.contArt.obtenerArticuloByID(Convert.ToInt32(item.Codigo));

                //Celdas
                TableCell celFecha = new TableCell();
                celFecha.Text = art.codigo;
                celFecha.VerticalAlign = VerticalAlign.Middle;
                celFecha.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celFecha);

                TableCell celNumero = new TableCell();
                celNumero.Text = art.descripcion;
                celNumero.VerticalAlign = VerticalAlign.Middle;
                celNumero.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celNumero);

                TableCell celRazon = new TableCell();
                celRazon.Text = item.Cantidad.ToString();
                celRazon.VerticalAlign = VerticalAlign.Middle;
                celRazon.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(celRazon);
                
                phRemito.Controls.Add(tr);
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}