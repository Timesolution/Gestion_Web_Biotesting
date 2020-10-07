using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.MateriasPrimas
{
    public partial class MateriasPrimasABM : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        controladorMateriaPrima contMateriaPrima = new controladorMateriaPrima();
        controladorArticulo contArticulo = new controladorArticulo();
        MateriaPrima materiaPrima = new MateriaPrima();

        //para saber si es alta(1) o modificacion(2)
        private int accion;
        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.VerificarLogin();
            this.accion = Convert.ToInt32(Request.QueryString["a"]);
            this.id = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                this.cargarDDLs();
                if (accion == 2)
                {
                    this.llenarCampos();
                }
            }
        }

        #region funcionesIniciales
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
                int valor = 0;

                string permisos = Session["Login_Permisos"] as string;
                string[] listPermisos = permisos.Split(';');

                if (listPermisos.Contains("194"))
                    return 1;

                return valor;
            }
            catch
            {
                return -1;
            }
        }

        private void cargarDDLs()
        {
            cargarMonedasVenta();
            cargarUnidadesDeMedida();
        }
        /// <summary>
        /// lleno los campos porque se va a editar la materia prima
        /// </summary>
        private void llenarCampos()
        {
            controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();
            Unidades_De_Medidas unidad = new Unidades_De_Medidas();
            materiaPrima = contMateriaPrima.obtenerMateriaPrima(id);

            unidad = controladorMateriaPrima.VerificarExistenciaUnidadDeMedida(materiaPrima.UnidadMedida);

            if (unidad.Id > 0)
            {
                ddlUnidadDeMedida.SelectedValue = unidad.Id.ToString();
            }
            else
            {
                ddlUnidadDeMedida.SelectedValue = "1";
            }

            txtCodMateriaPrima.Text = materiaPrima.Codigo;
            txtDescripcion.Text = materiaPrima.Descripcion;
            txtImporte.Text = materiaPrima.Importe.ToString();
            txtStockMinimo.Text = materiaPrima.StockMinimo.ToString();
            ddlEstado.SelectedValue = materiaPrima.Estado.ToString();
            ddlMonedaVenta.SelectedIndex = (int)materiaPrima.Moneda;

            

        }
        #endregion

        #region cargaDDLs
        private void cargarMonedasVenta()
        {
            try
            {
                DataTable dt = contArticulo.obtenerMonedas();

                //agrego todos
                DataRow dr = dt.NewRow();
                dr["moneda"] = "Seleccione...";
                dr["id"] = -1;
                dt.Rows.InsertAt(dr, 0);

                this.ddlMonedaVenta.DataSource = dt;
                this.ddlMonedaVenta.DataValueField = "id";
                this.ddlMonedaVenta.DataTextField = "moneda";

                this.ddlMonedaVenta.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando monedas de venta a la lista. " + ex.Message));
            }
        }

        private void cargarUnidadesDeMedida()
        {
            try
            {
                controladorMateriaPrima controladorMateriaPrima = new controladorMateriaPrima();

                var UnidadesDeMedidas = controladorMateriaPrima.GetAllUnidades();

                this.ddlUnidadDeMedida.DataSource = UnidadesDeMedidas;
                this.ddlUnidadDeMedida.DataValueField = "Id";
                this.ddlUnidadDeMedida.DataTextField = "Descripcion";

                this.ddlUnidadDeMedida.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando monedas de venta a la lista. " + ex.Message));
            }
        }
        #endregion

        #region Acciones Botones
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.materiaPrima.Id = id;
                this.materiaPrima.Codigo = txtCodMateriaPrima.Text;
                this.materiaPrima.Descripcion = txtDescripcion.Text;
                this.materiaPrima.Estado = Convert.ToInt32(ddlEstado.SelectedValue);
                this.materiaPrima.Importe = Convert.ToDecimal(txtImporte.Text);
                this.materiaPrima.Moneda = ddlMonedaVenta.SelectedIndex;
                this.materiaPrima.UnidadMedida = ddlUnidadDeMedida.SelectedItem.Text;
                this.materiaPrima.StockMinimo = Convert.ToDecimal(txtStockMinimo.Text);
                int sucursal = Convert.ToInt32(Session["Login_SucUser"]);
                
                if (accion == 1)//crea
                {
                    int i = contMateriaPrima.agregarMateriaPrima(materiaPrima);
                    if (i > 0)
                    {
                        MateriaPrima_Composiciones materiaPrimaComposicion = new MateriaPrima_Composiciones();
                        materiaPrimaComposicion.MateriaPrima = new MateriaPrima();
                        materiaPrimaComposicion.Id_MateriaPrima = this.materiaPrima.Id;
                        materiaPrimaComposicion.Cantidad_real = 0;
                        contMateriaPrima.AgregarMateriaPrimaStock(materiaPrimaComposicion, sucursal);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Materia prima agregada correctamente."));
                        Response.Redirect("MateriasPrimasF.aspx",true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error agregando materia prima."));
                    }
                }
                else//modifica
                {
                    MateriaPrima mp = this.contMateriaPrima.obtenerMateriaPrima(id);
                    mp.Codigo = txtCodMateriaPrima.Text;
                    mp.Descripcion = txtDescripcion.Text;
                    mp.Estado = Convert.ToInt32(ddlEstado.SelectedValue);
                    mp.Importe = Convert.ToDecimal(txtImporte.Text);
                    mp.Moneda = ddlMonedaVenta.SelectedIndex;
                    mp.UnidadMedida = ddlUnidadDeMedida.SelectedItem.Text;
                    mp.StockMinimo = Convert.ToDecimal(txtStockMinimo.Text);

                    int i = contMateriaPrima.modificarMateriaPrima(mp);
                    if (i > 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Materia prima modificada correctamente."));
                        Response.Redirect("MateriasPrimasF.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error modificando materia prima."));
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en fun: btnAgregar_Click. " + ex.Message));
            }
        }
        #endregion



    }
}