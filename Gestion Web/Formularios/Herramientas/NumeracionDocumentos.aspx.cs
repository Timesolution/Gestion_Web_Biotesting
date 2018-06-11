using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Herramientas
{
    public partial class NumeracionDocumentos : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorSucursal contrSucu = new controladorSucursal();
        controladorUsuario contUser = new controladorUsuario();
        private int idSucursal;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                    if(!IsPostBack)
                    {
                        //this.cargarPuntoVta();
                        this.cargarSucursal();
                    }
                    if(this.ListPuntoVenta.SelectedValue != "-1")
                    {
                        this.cargarTablaNumeracion(Convert.ToInt32(this.ListPuntoVenta.SelectedValue));
                    }
            }
            catch
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
                        if (s == "56")
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
        public void cargarSucursal()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerSucursales();

                // agrego todos
                DataRow dr = dt.NewRow();
                dr["nombre"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);


                this.ListSucursal.DataSource = dt;
                this.ListSucursal.DataValueField = "Id";
                this.ListSucursal.DataTextField = "nombre";

                this.ListSucursal.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVta()
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                DataTable dt = contSucu.obtenerPuntoVentaAllDT();


                //agrego todos
                DataRow dr = dt.NewRow();
                dr["NombreFantasia"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ListPuntoVenta.DataSource = dt;
                this.ListPuntoVenta.DataValueField = "Id";
                this.ListPuntoVenta.DataTextField = "NombreFantasia";

                this.ListPuntoVenta.DataBind();


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
        }
        public void cargarPuntoVtaBySuc(int idSuc)
        {
            try
            {
                controladorSucursal contSucu = new controladorSucursal();
                List<PuntoVenta> list = contSucu.obtenerPtoVentaSucursal(idSuc);
                this.ListPuntoVenta.Items.Clear();
                foreach (PuntoVenta pv in list)
                {
                    this.ListPuntoVenta.Items.Add(new ListItem(pv.nombre_fantasia, pv.id.ToString()));
                }

                this.ListPuntoVenta.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
            catch
            {

            }
        }

        protected void DropListClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ListPuntoVenta.SelectedValue != "-1")
                {
                    this.cargarTablaNumeracion(Convert.ToInt32(this.ListPuntoVenta.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Numeracion. " + ex.Message));
            }
        }

        private void cargarTablaNumeracion(int idPuntoVenta)
        {
            try
            {
                List<Numeracion> Numeraciones = contrSucu.obtenerNumeracionPuntoVenta(idPuntoVenta);
                phNumeracion.Controls.Clear();
                foreach (Numeracion n in Numeraciones)
                {
                    this.cargarNumeracionEnPh(n);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Tabla de Numeracion. " + ex.Message));

            }
        }

        private void cargarNumeracionEnPh(Numeracion n)
        {
            try
            {

                //fila
                TableRow tr = new TableRow();
                tr.ID = n.id.ToString();

                //Celdas
                TableCell celCodigo = new TableCell();
                celCodigo.Text = n.tipo.tipo;
                celCodigo.Width = Unit.Percentage(80);
                celCodigo.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(celCodigo);

                TableCell celCantidad = new TableCell();
                TextBox txtCant = new TextBox();
                txtCant.ID = "Text_" + n.id.ToString();
                txtCant.AutoPostBack = true;
                txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                txtCant.CssClass = "form-control";
                txtCant.Style.Add("text-align", "Right");
                txtCant.Text = n.numeracion.ToString();
                txtCant.TextChanged += new EventHandler(ActualizarValor);
                celCantidad.Controls.Add(txtCant);
                celCantidad.Width = Unit.Percentage(20);
                tr.Cells.Add(celCantidad);

                phNumeracion.Controls.Add(tr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando Numeraciones. " + ex.Message));
            }

        }

        protected void lbtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Numeracion modificada con exito", null));
                //Response.Redirect("PanelControl.aspx");
            }
            catch
            {

            }
        }

        protected void ActualizarValor(object sender, EventArgs e)
        {
            try
            {
                string posicion = (sender as TextBox).ID.ToString().Substring(5, (sender as TextBox).ID.Length - 5);
                Numeracion n = this.contrSucu.obtenerNumeracionID(Convert.ToInt32(posicion));
                if (!String.IsNullOrEmpty((sender as TextBox).Text))
                {
                    n.numeracion = Convert.ToInt32((sender as TextBox).Text);
                }
                else
                {
                    n.numeracion = 0;
                }
                int i = this.contrSucu.modificarNumeracion(n);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error calculando total. Verifique que ingreso numeros en cantidad" + ex.Message));
            }
        }

        protected void ListSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.ListSucursal.SelectedValue) > 0)
                {
                    this.cargarPuntoVtaBySuc(Convert.ToInt32(this.ListSucursal.SelectedValue));
                }
            }
            catch
            {

            }
        }
    }
}