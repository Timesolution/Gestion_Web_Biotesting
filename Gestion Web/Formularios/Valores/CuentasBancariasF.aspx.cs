using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Valores
{
    public partial class CuentasBancariasF : System.Web.UI.Page
    {
        ControladorBanco cont = new ControladorBanco();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();
            this.obtenerCuentas();
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

                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Bancos.Entidadades Bancarias") != 1)
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
                        if (s == "17")
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

        private void obtenerCuentas()
        {
            try
            {
                List<CuentasBancaria> cuentas = this.cont.obtenerCuentasBancarias();
                this.phCuentas.Controls.Clear();
                foreach (var c in cuentas)
                {
                    this.cargarCuentaTabla(c);
                }
            }
            catch (Exception ex)
            {
 
            }
        }

        private void cargarCuentaTabla(CuentasBancaria c)
        {
            try
            {
                TableRow tr = new TableRow();

                TableCell celBanco = new TableCell();
                celBanco.Text = c.Banco1.entidad;
                
                celBanco.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celBanco);

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = c.Descripcion;
                
                celDescripcion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celDescripcion);

                TableCell celNumero = new TableCell();
                celNumero.Text = c.Numero;
                
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celCUIT = new TableCell();
                celCUIT.Text = c.Cuit;
                
                celCUIT.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCUIT);

                TableCell celLibrador = new TableCell();
                celLibrador.Text = c.Librador;                
                celLibrador.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celLibrador);

                TableCell celAccion = new TableCell();
                Literal lDetail = new Literal();
                lDetail.ID = c.Id.ToString();
                lDetail.Text = "<a href=\"CuentasBancariasABM.aspx?a=2&id=" + c.Id.ToString() + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >";                
                lDetail.Text += "<i class=\"shortcut-icon icon-search\"></i>";
                lDetail.Text += "</a>";
                celAccion.Controls.Add(lDetail);

                tr.Cells.Add(celAccion);

                this.phCuentas.Controls.Add(tr);

                
            }
            catch(Exception ex)
            {
 
            }
        }
    }
}