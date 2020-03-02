using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class DocumentosF : System.Web.UI.Page
    {
        controladorCuentaCorriente controlador = new controladorCuentaCorriente();
        controladorFacturacion contFact = new controladorFacturacion();
        controladorCliente contrCliente = new controladorCliente();
        Cliente cliente = new Cliente();
        CuentaCorriente cuenta = new CuentaCorriente();
        Mensajes mje = new Mensajes();
        string documentos = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.documentos = Request.QueryString["documentos"];
            CargarMovimientos(documentos);
            
        }

        public void CargarMovimientos(string doc)
        {
            string[] documento = doc.Split(new Char[] { ';' });
            List<Movimiento> movimiento = new List<Movimiento>();
            decimal totalSaldo = 0;
            foreach (string d in documento)
            {
                if (!String.IsNullOrEmpty(d))
                {
                    Movimiento m = controlador.obtenerMovimientoID(Convert.ToInt32(d));
                    totalSaldo += m.saldo;
                    this.cargarEnPh(m);
                    
                }
            }
            labelSaldo.Text = "$ " + totalSaldo.ToString();
            
        }

        private void cargarEnPh(Movimiento m)
        {

            MovimientoView movV = new MovimientoView();
            movV = m.ListarMovimiento();
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = movV.id.ToString();

                //Celdas

                TableCell celNumero = new TableCell();
                celNumero.Text = movV.tipo.tipo + " " + movV.numero.ToString().PadLeft(8, '0');
                celNumero.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celNumero);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = "$ " + movV.saldo;
                celSaldo.Width = Unit.Percentage(15);
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSaldo);

                TableCell celImputar = new TableCell();
                TextBox txtImputar = new TextBox();
                txtImputar.CssClass = "form-control";
                txtImputar.AutoPostBack = true;
                txtImputar.ID = "txtImputar_" + movV.id;
                txtImputar.TextChanged += new EventHandler(this.ActualizarTotales);
                txtImputar.Attributes.Add("onpresskey", "javascript:return validarNro(event)");
                celImputar.Controls.Add(txtImputar);
                celImputar.Width = Unit.Percentage(15);
                celImputar.HorizontalAlign = HorizontalAlign.Right;
                celImputar.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celImputar);


                phDocumentos.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error agregando documentos. " + ex.Message));
            }

        }

        public void ActualizarTotales(object sender, EventArgs e)
        {
           
            try
            {
                decimal suma = 0;
                foreach(Control C in phDocumentos.Controls)
                {   
                    TableRow tr = C as TableRow;
                    TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                    txt.Text = txt.Text.Replace('.', ',');
                    if(!String.IsNullOrEmpty(txt.Text))
                    { 
                        suma += Convert.ToDecimal(txt.Text);
                    }
                }
                labelTotal.Text = "$ " + suma.ToString();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Error actualizando totales. " + ex.Message));
            }

        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            List<Imputacion> imputaciones = new List<Imputacion>();
            foreach(Control c in phDocumentos.Controls)
            {
                Imputacion imp = new Imputacion();
                TableRow tr = c as TableRow;
                TextBox txt = tr.Cells[2].Controls[0] as TextBox;
                //imp.id_doc = Convert.ToInt32(tr.ID);
                txt.Text = txt.Text.Replace('.', ',');
                imp.movimiento = controlador.obtenerMovimientoID(Convert.ToInt32(tr.ID));
                //MovimientoView movV = new MovimientoView();
                //movV = m.ListarMovimiento();
                //imp.tp = movV.tipo;
                //imp.saldo = Convert.ToDecimal(movV.saldo);
                imp.total = Convert.ToDecimal(tr.Cells[1].Text.Substring(1));
                if(!String.IsNullOrEmpty(txt.Text))
                { 
                    imp.imputar = Convert.ToDecimal(txt.Text);
                }
                else
                {
                    imp.imputar = 0;
                }

                imputaciones.Add(imp);
                    
            }
            Session.Add("ListaImputaciones", imputaciones);
            Response.Redirect("ABMCobros.aspx");
        }

        protected void txtPrueba_TextChanged(object sender, EventArgs e)
        {
            this.labelTotal.Text = "hola";
        }

    }
}