using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Planario_Api.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ComparativaProveedorF : System.Web.UI.Page
    {
        Mensajes mje = new Mensajes();
        //controlador
        controladorEstadoCliente controlador = new controladorEstadoCliente();
        controladorUsuario contUser = new controladorUsuario();
        //valores
        private int _idArticulo;
        private int rowIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                if (!IsPostBack)
                {
                    CargarProveedoresColumnas();
                    InicializarViewState();
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Ocurrio un error. " + ex.Message));
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
                    //if (this.contUser.validarAcceso((int)Session["Login_IdUser"], "Maestro.Clientes.Estados") != 1)
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
                        if (s == "26")
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

        public void GuardarDatosViewState(TableRow row)
        {
            //si es la primera vez que vas a guardar, verificas que el viewstat este vacio
            var rows = ViewState["rows"] as List<TableRow>;

            if (rows == null)//nunca agregue ninguna filla
            {
                //creas el registro en el viewstate
                rows = new List<TableRow>();
                ViewState["rows"] = rows;
            }

            //agrego la fila 
            rows.Add(row);
            //vuelvo a guardar en el viewstate
            ViewState["rows"] = rows;

        }

        public List<TableRow> CargarDatosViewState()
        {
            //si es la primera vez que vas a guardar, verificas que el viewstat este vacio
            var rows = ViewState["rows"] as List<TableRow>;

            if (rows == null)//nunca agregue ninguna filla
            {
                //creas el registro en el viewstate
                return new List<TableRow>();
            }
            //sino quiere decir que tengo cosas de antes
            return rows;

        }

        public void InicializarViewState()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[7]
                {
                new DataColumn("idArticulo"),
                new DataColumn("idProveedor"),
                new DataColumn("Codigo"),
                new DataColumn("Descripcion"),
                new DataColumn("Proveedor"),
                new DataColumn("Precio"),
                new DataColumn("Fecha"),
                });

                ViewState["rows"] = dt;
                BindGrid();


            }
            catch (Exception ex)
            {

            }

        }

        public void BindGrid()
        {
            GridTable.DataSource = (DataTable)ViewState["rows"];
            GridTable.DataBind();
        }

        public void CargarProveedoresColumnas()
        {
            try
            {
                //ListItem i;
                //i = new ListItem("1", "2");
                //ListProveedorNumero.Items.Add(i);
                //i = new ListItem("2", "3");
                //ListProveedorNumero.Items.Add(i);
                //i = new ListItem("3", "4");
                //ListProveedorNumero.Items.Add(i);
                //i = new ListItem("4", "5");
                //ListProveedorNumero.Items.Add(i);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                //controladorCliente contrCliente = new controladorCliente();
                //DataTable dtProveedores = contrCliente.obtenerProveedorNombreDT(this.txtCodProveedor.Text);

                ////cargo la lista
                //this.ListProveedor.DataSource = dtProveedores;
                //this.ListProveedor.DataValueField = "id";
                //this.ListProveedor.DataTextField = "alias";

                //this.ListProveedor.DataBind();

                //DataRow dr2 = dtProveedores.NewRow();
                //dr2["razonSocial"] = "Todos";
                //dr2["id"] = -1;
                //dtProveedores.Rows.InsertAt(dr2, 0);

            }
            catch (Exception ex)
            {

            }


        }

        protected void btnBuscarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                ControladorArticulosEntity controladorArticulosEntity = new ControladorArticulosEntity();



                var listArticulos = controladorArticulosEntity.obtenerArticuloEntityByCodigo(this.txtCodArticulo.Text);

                //cargo la lista
                this.ListArticulos.DataSource = listArticulos;
                this.ListArticulos.DataValueField = "id";
                this.ListArticulos.DataTextField = "descripcion";

                this.ListArticulos.DataBind();

                //DataRow dr2 = listArticulos.NewRow();
                //dr2["descripcion"] = "Todos";
                //dr2["id"] = -1;
                //listArticulos.Rows.InsertAt(dr2, 0);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnAgregarArticulo_Click(object sender, EventArgs e)
        {

            try
            {
                controladorArticulo controladorArticulo = new controladorArticulo();

                if (!string.IsNullOrEmpty(ListArticulos.SelectedValue))
                {
                    int idArticulo = Convert.ToInt32(ListArticulos.SelectedValue);

                    int vericar = VerificarExistenciaIdArticulo(idArticulo);

                    if (vericar == 0)
                    {
                        DataTable articuloProveedor = controladorArticulo.ObtenerArticuloConProveedoresComparativa(idArticulo);
                        DataTable dt = (DataTable)ViewState["rows"];

                        foreach (DataRow row in articuloProveedor.Rows)
                        {
                            dt.Rows.Add(row["idArticulo"].ToString(), row["idProveedor"].ToString(), row["codigo"].ToString(), row["descripcion"].ToString(), row["razonSocial"].ToString(), "$ " + row["precio"].ToString(), row["fecha"].ToString());
                        }

                        ViewState["Customers"] = dt;
                        this.BindGrid();

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msgSucces", "msgSucces()", true);

                    }
                    else if(vericar == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("El articulo ya se encuentra cargado"));
                    }
                    
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeBoxError("Debe seleccionar un articulo"));
                }

            }
            catch (Exception ex)
            {

            }
        }

        public int VerificarExistenciaIdArticulo(int idArticulo)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["rows"];

                foreach (DataRow row in dt.Rows)
                {
                    int idArt = Convert.ToInt32(row["idArticulo"].ToString());

                    if (idArt == idArticulo)
                    {
                        return 1;
                    }

                    

                }


                foreach (GridViewRow row in GridTable.Rows)
                {
                    
                }

                return 0;

                

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        protected void lbtnAgregarProveedor_Click(object sender, EventArgs e)
        {
            //int valorColumna = Convert.ToInt32(ListProveedorNumero.SelectedValue);
            //int cantidadFilas = myTable.Rows.Count - 1;

            ////myTable.Rows.Cells[valorColumna].Text = "New Value";
            //myTable.Rows[1].Cells[1].Text = "New Value";

        }

        protected void GridTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    _idArticulo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "idArticulo").ToString());
            //}
        }

        protected void GridTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //bool newRow = false;
            //if ((_idArticulo > 0) && (DataBinder.Eval(e.Row.DataItem, "idArticulo") != null))
            //{
            //    if (_idArticulo != Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "idArticulo").ToString()))
            //        newRow = true;
            //}
            //if ((_idArticulo > 0) && (DataBinder.Eval(e.Row.DataItem, "idArticulo") == null))
            //{
            //    newRow = true;
            //    rowIndex = 0;
            //}
            //if (newRow)
            //{
            //    AddNewRow(sender, e);
            //}

        }

        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            //GridView GridView1 = (GridView)sender;
            //GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            //NewTotalRow.Font.Bold = true;
            //NewTotalRow.BackColor = System.Drawing.Color.Aqua;
            //TableCell HeaderCell = new TableCell();
            //HeaderCell.Height = 10;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderCell.ColumnSpan = 5;
            //NewTotalRow.Cells.Add(HeaderCell);
            //GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow);
            //rowIndex++;
        }
    }
}