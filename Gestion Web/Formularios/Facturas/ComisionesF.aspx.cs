using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using Neodynamic.WebControls.BarcodeProfessional;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Gestion_Api.Entitys;
using System.Globalization;
using System.Web.Configuration;
using Gestion_Api.Modelo.Enums;
using System.ServiceModel.Security.Tokens;
using Gestion_Web.TS_Gestion_DesarrolloDataSetTableAdapters;
using Microsoft.Owin.Security;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ComisionesF : System.Web.UI.Page
    {
        controladorCliente contCliente = new controladorCliente();

        Mensajes m = new Mensajes();
        private string fechaD;
        private string fechaH;
        private int idcliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();

                //datos de filtro
                fechaD = Request.QueryString["Fechadesde"];
                fechaH = Request.QueryString["FechaHasta"];
                idcliente = Convert.ToInt32(Request.QueryString["cl"]);

                if (!IsPostBack)
                {


                    if (fechaD == null && fechaH == null && idcliente == 0)
                    {
                        fechaD = DateTime.Now.ToString("dd/MM/yyyy");
                        fechaH = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    }

                    this.cargarClientes();
                }

                if (idcliente > 0 || idcliente == -1)
                {

                    CargarComisiones();
                }

                this.Form.DefaultButton = this.btnBuscarCod.UniqueID;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }


        private void CargarComisiones()
        {
            try
            {
                controladorCuentaCorriente controladorCuentaCorriente = new controladorCuentaCorriente();

                var lista = controladorCuentaCorriente.ObtenerComisiones(fechaD, fechaH, idcliente);

                foreach (DataRow row in lista.Rows)
                {
                    decimal saldo = !string.IsNullOrEmpty(row["saldo"].ToString()) ? Convert.ToDecimal(row["saldo"].ToString()) : 0;
                    decimal iva = !string.IsNullOrEmpty(row["iva"].ToString()) ? Convert.ToDecimal(row["iva"].ToString()) : 0;
                    decimal totalArt = !string.IsNullOrEmpty(row["arttotal"].ToString()) ? Convert.ToDecimal(row["arttotal"].ToString()) : 0;

                    decimal total = (saldo - iva) - totalArt;
                    this.cargarEnPh(row, total);
                }

                lbtnGenerarComisiones.Visible = true;

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando eventos clientes en CRM.  " + ex.Message));
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
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        public void cargarClientes()
        {
            try
            {
                controladorCliente contCliente = new controladorCliente();

                DataTable dt = new DataTable();

                dt = contCliente.obtenerClientesDT();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["alias"] = "Todos";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.DropListClientes.DataSource = dt;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";

                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        private void cargarEnPh(DataRow row, decimal total)
        {
            try
            {
                //fila
                TableRow tr = new TableRow();
                tr.ID = row["id"].ToString();

                //Celdas

                TableCell celCliente = new TableCell();
                celCliente.Text = row["razonSocial"].ToString();
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.VerticalAlign = VerticalAlign.Middle;
                celCliente.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(celCliente);

                TableCell celSaldo = new TableCell();
                celSaldo.Text = row["saldo"].ToString();
                celSaldo.HorizontalAlign = HorizontalAlign.Right;
                celSaldo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celSaldo);

                TableCell celArticulo = new TableCell();
                celArticulo.Text = row["arttotal"].ToString();
                celArticulo.HorizontalAlign = HorizontalAlign.Right;
                celArticulo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celArticulo);

                TableCell Iva21 = new TableCell();
                Iva21.Text = row["iva"].ToString();
                Iva21.HorizontalAlign = HorizontalAlign.Right;
                Iva21.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(Iva21);

                TableCell Total = new TableCell();
                Total.Text = Convert.ToString(total);
                Total.HorizontalAlign = HorizontalAlign.Right;
                Total.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(Total);

                TableCell padre = new TableCell();
                padre.HorizontalAlign = HorizontalAlign.Right;
                padre.VerticalAlign = VerticalAlign.Middle;
                TextBox txt1 = new TextBox();
                txt1.ID = "txtPadre_" + row["id"].ToString();
                txt1.TextMode = TextBoxMode.Number;
                padre.Controls.Add(txt1);
                tr.Cells.Add(padre);

                TableCell abuelo = new TableCell();
                abuelo.HorizontalAlign = HorizontalAlign.Right;
                abuelo.VerticalAlign = VerticalAlign.Middle;
                TextBox txt2 = new TextBox();
                txt2.ID = "txtAbuelo_" + row["id"].ToString();
                txt2.TextMode = TextBoxMode.Number;
                abuelo.Controls.Add(txt2);
                tr.Cells.Add(abuelo);

                //TableCell celAccion = new TableCell();
                //LinkButton btnConfirmacion = new LinkButton();
                //btnConfirmacion.CssClass = "btn btn-info";
                //btnConfirmacion.Attributes.Add("data-toggle", "modal");
                //btnConfirmacion.Attributes.Add("href", "#modalConfirmarFinalizado");
                //btnConfirmacion.ID = "btnSelec_" + clientes_Eventos.Id.ToString();
                //btnConfirmacion.Text = "<span class='shortcut-icon icon-ok'></span>";
                //btnConfirmacion.ToolTip = "Marcar como finalizado";
                //btnConfirmacion.OnClientClick = "abrirdialog(" + clientes_Eventos.Id + ");";
                //btnConfirmacion.Font.Size = 12;
                //celAccion.Controls.Add(btnConfirmacion);
                //tr.Cells.Add(celAccion);

                phComsiones.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando Comisiones. " + ex.Message));
            }

        }

        #endregion

        #region Eventos Controles
        protected void btnBuscarCod_Click(object sender, EventArgs e)
        {
            try
            {
                controladorCliente contrCliente = new controladorCliente();
                String buscar = this.txtCodCliente.Text.Replace(' ', '%');
                DataTable dtClientes = contrCliente.obtenerClientesAliasDT(buscar);

                //cargo la lista
                this.DropListClientes.DataSource = dtClientes;
                this.DropListClientes.DataValueField = "id";
                this.DropListClientes.DataTextField = "alias";
                this.DropListClientes.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando clientes a la lista. " + ex.Message));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtFechaDesde.Text) && (!String.IsNullOrEmpty(txtFechaHasta.Text)))
                {
                    Response.Redirect("ComisionesF.aspx?fechadesde=" + txtFechaDesde.Text + "&fechaHasta=" + txtFechaHasta.Text + "&cl=" + DropListClientes.SelectedValue);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Debe cargar ambas fechas"));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));

            }
        }

        #endregion

        protected void lbtnGenerarComisiones_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                DataTable dt = new DataTable();
                DataTable detalleDT = new DataTable();

                // Le pongo las columnas al detalle
                detalleDT.Columns.Add("idCliente"); // beneficiario
                detalleDT.Columns.Add("dador"); // este es el cliente que le da el porcentaje al beneficiario (idcliente)
                detalleDT.Columns.Add("importe");// el importe es el total de comision
                detalleDT.Columns.Add("comision"); // el porcentaje de la comision
                detalleDT.Columns.Add("subtotal"); // el subtotal es el total
                detalleDT.Columns.Add("alias");
                detalleDT.Columns.Add("total");

                // Le pongo las columnas al datatable.
                dt.Columns.Add("idCliente");
                dt.Columns.Add("alias");
                dt.Columns.Add("total");

                // Recorro el phComsiones para obtener cada row de la tabla.
                foreach (Control C in phComsiones.Controls)
                {
                    decimal padre = 0;
                    decimal abuelo = 0;
                    TableRow tr = C as TableRow;
                    TextBox padre1 = tr.Cells[tr.Cells.Count - 2].Controls[0] as TextBox;
                    TextBox abuelo1 = tr.Cells[tr.Cells.Count - 1].Controls[0] as TextBox;
                    string dador = tr.Cells[0].Text;
                    decimal total = Convert.ToDecimal(tr.Cells[tr.Cells.Count - 3].Text);

                    // Verifico si el padre tiene algo, para poder convertirlo a int.
                    if (!string.IsNullOrEmpty(padre1.Text))
                    {
                        padre = Convert.ToDecimal(padre1.Text);

                    }

                    // Verifico si el abuelo tiene algo, para poder convertirlo a int.
                    if (!string.IsNullOrEmpty(abuelo1.Text))
                    {
                        abuelo = Convert.ToDecimal(abuelo1.Text);

                    }

                    // obtengo el id del cliente que el tr contiene el id del mismo.
                    string idCliente = tr.ID;

                    var datos = controladorClienteEntity.ObtenerPadreAbueloDelCliente(Convert.ToInt32(idCliente));


                    // verifico si el datatable tiene rows.
                    // si tiene, voy a recorrer cada 1 verificando si existe el idcliente asi se la suma al total de la comision a percibir.
                    if (dt.Rows.Count > 0)
                    {

                        var modifico = SumarComision(dt, datos, total, padre, abuelo);


                        //if (modifico.modificoPadre == 0 && modifico.modificoAbuelo == 0)
                        //{
                        //    AgregarFilas(dt, datos, padre, abuelo, total);
                        //}

                        if (modifico.modificoPadre == 0)
                        {
                            DataRow row = datos.Rows[0];

                            if (!string.IsNullOrEmpty(row["idPadre"].ToString()))
                            {
                                string id = row["idPadre"].ToString();
                                string nombre = row["Padre"].ToString();

                                if (padre > 0 && total > 0)
                                {
                                    AgregarFila(dt, id, nombre, total, padre);
                                    AgregarFilaDetalle(id, total, padre, detalleDT, dador);
                                }

                            }

                        }

                        if (modifico.modificoAbuelo == 0)
                        {
                            DataRow row = datos.Rows[0];

                            if (!string.IsNullOrEmpty(row["idAbuelo"].ToString()))
                            {
                                string id = row["idAbuelo"].ToString();
                                string nombre = row["Abuelo"].ToString();

                                if (abuelo > 0 && total > 0)
                                {
                                    AgregarFila(dt, id, nombre, total, abuelo);
                                    AgregarFilaDetalle(id, total, abuelo, detalleDT, dador);
                                }

                            }
                        }

                    }
                    else if (dt.Rows.Count == 0)
                    {
                        // aca seteo las primeras 2 filas o 1 sola, depende si tiene padre o abuelo o los 2.
                        // si tiene los 2 se crean 2 nuevos rows. sino solo 1.
                        AgregarFilas(dt, datos, padre, abuelo, total, detalleDT, dador);

                    }

                }

                DataView detalleDTSort = detalleDT.DefaultView;
                detalleDTSort.Sort = "idCliente desc";
                DataTable sortedDT = detalleDTSort.ToTable();

                DataTable final = CombinarDataTable(dt, sortedDT);


                Session["informeComisiones"] = final;

                Response.Redirect("ImpresionComisiones.aspx?ex=1&a=1");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));
            }
        }


        public DataTable CombinarDataTable(DataTable dt, DataTable detalleDT)
        {
            try
            {

                foreach (DataRow rowDT in dt.Rows)
                {

                    foreach (DataRow rowDetalleDT in detalleDT.Rows)
                    {

                        if (rowDT["idCliente"].ToString() == rowDetalleDT["idCliente"].ToString())
                        {
                            rowDetalleDT["alias"] = rowDT["alias"].ToString();
                            rowDetalleDT["total"] = rowDT["total"].ToString();
                        }

                    }

                }

                return detalleDT;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public (int modificoPadre, int modificoAbuelo) SumarComision(DataTable dt, DataTable datos, decimal total, decimal padre, decimal abuelo)
        {
            try
            {
                int modificoPadre = 0;
                int modificoAbuelo = 0;
                // recorro la tabla a exportar que estoy rellenando
                foreach (DataRow row in dt.Rows)
                {
                    // verifico que el total sea mayor a 0 sino no tiene sentido tener que agregar una comision a ese cliente
                    if (total > 0)
                    {
                        // obtengo los datos de la familia
                        DataRow familia = datos.Rows[0];

                        // verifico si existe el id del cliente en la tabla a exportar
                        if (row["idCliente"].ToString() == familia["idPadre"].ToString())
                        {
                            decimal comisionTotal = Convert.ToDecimal(row["total"].ToString());
                            comisionTotal += total * (padre / 100);
                            row["total"] = String.Format("{0:n}", comisionTotal);
                            modificoPadre = 1;
                        }

                        // verifico si existe el id del cliente en la tabla a exportar
                        else if (row["idCliente"].ToString() == familia["idAbuelo"].ToString())
                        {
                            decimal comisionTotal = Convert.ToDecimal(row["total"].ToString());
                            comisionTotal += total * (abuelo / 100);
                            row["total"] = String.Format("{0:n}", comisionTotal);
                            modificoAbuelo = 1;
                        }

                    }

                }

                return (modificoPadre, modificoAbuelo);



            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));
                return (-1, -1);
            }
        }

        public void AgregarFila(DataTable dt, string id, string nombre, decimal total, decimal porcentaje)
        {
            try
            {
                // verifico que idpadre sea dif de null o vacio ya que puede ser que no tenga padre.
                if (!string.IsNullOrEmpty(id))
                {
                    if (porcentaje > 0 && total > 0)
                    {
                        DataRow firstRow = dt.NewRow();
                        firstRow["idCliente"] = id;
                        firstRow["alias"] = nombre;

                        decimal comisionTotal = total * (porcentaje / 100);
                        firstRow["total"] = String.Format("{0:n}", comisionTotal);

                        dt.Rows.Add(firstRow);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AgregarFilaDetalle(string id, decimal total, decimal porcentaje, DataTable detalleDT, string dador)
        {
            try
            {

                DataRow newRow = detalleDT.NewRow();

                newRow["idCliente"] = id;
                newRow["dador"] = dador;
                newRow["importe"] = total; 
                newRow["comision"] = porcentaje;
                newRow["alias"] = "";
                newRow["total"] = "";

                decimal comisionTotal = total * (porcentaje / 100);
                newRow["subtotal"] = String.Format("{0:n}", comisionTotal);

                detalleDT.Rows.Add(newRow);

            }
            catch (Exception ex)
            {

            }
        }

        public void AgregarFilas(DataTable dt, DataTable datos, decimal padre, decimal abuelo, decimal total, DataTable detalleDT, string dador)
        {
            try
            {

                DataRow row = datos.Rows[0];

                // verifico que idpadre sea dif de null o vacio ya que puede ser que no tenga padre.
                if (!string.IsNullOrEmpty(row["idPadre"].ToString()))
                {
                    if (padre > 0 && total > 0)
                    {
                        DataRow firstRow = dt.NewRow();
                        firstRow["idCliente"] = row["idPadre"].ToString();
                        firstRow["alias"] = row["Padre"].ToString();

                        decimal comisionTotal = total * (padre / 100);
                        firstRow["total"] = String.Format("{0:n}", comisionTotal);

                        dt.Rows.Add(firstRow);

                        AgregarFilaDetalle(row["idPadre"].ToString(), total, padre, detalleDT, dador);
                    }

                }

                // verifico que idabuelo sea dif de null o vacio ya que puede ser que no tenga abuelo.
                if (!string.IsNullOrEmpty(row["idAbuelo"].ToString()))
                {
                    if (abuelo > 0 && total > 0)
                    {
                        DataRow secondRow = dt.NewRow();
                        secondRow["idCliente"] = row["idAbuelo"].ToString();
                        secondRow["alias"] = row["Abuelo"].ToString();

                        decimal comisionTotal = total * (abuelo / 100);
                        secondRow["total"] = String.Format("{0:n}", comisionTotal);

                        dt.Rows.Add(secondRow);

                        AgregarFilaDetalle(row["idAbuelo"].ToString(), total, abuelo, detalleDT, dador);

                    }


                }

            }
            catch (Exception ex)
            {

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error obteniendo listado. " + ex.Message));

            }
        }

    }





}