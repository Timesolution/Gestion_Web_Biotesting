using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.PlanCuentas
{
    public partial class Cuentas_Contables_Mayor : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();

        class ListItemTemporal
        {
            public string id;
            public string nombre;
        }

        class CuentasContables_MayorTipoMovimiento_Temporal
        {
            public string Id;
            public string IdCuenta_Contable;
            public string IdMayor_TipoMovimiento;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarLogin();
                if (!IsPostBack)
                {
                    CargarDropList_TiposMovimientosEnLista();
                    CargarDropLists();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
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
            }
            catch
            {
                Response.Redirect("../../Account/Login.aspx");
            }
        }

        #region cargas drop list
        private void CargarDropLists()
        {
            CargarNivelesDeLosDropDown();
        }
        private void CargarDropList_TiposMovimientosEnLista()
        {
            try
            {
                var dtMayor_TipoMovimiento = contPlanCuentas.GetAll_Mayor_TipoMovimiento();

                if (dtMayor_TipoMovimiento != null)
                {
                    dropList_Mayor_TipoDeMovimiento.DataSource = dtMayor_TipoMovimiento;
                    dropList_Mayor_TipoDeMovimiento.DataTextField = "TipoMovimiento";
                    dropList_Mayor_TipoDeMovimiento.DataValueField = "Id";
                    dropList_Mayor_TipoDeMovimiento.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }
        public void CargarNivelesDeLosDropDown()
        {
            try
            {
                DropDownList[] ddls = { DropListNivel1, DropListNivel2, DropListNivel3, DropListNivel4 };
                List<Cuentas_Contables> lista = new List<Cuentas_Contables>();

                for (int i = 0; i < ddls.Length; i++)
                {
                    if (i == 0)
                    {
                        lista = contPlanCuentas.obtenerCuentasContablesByNivel(1, 0);
                    }
                    if (lista != null)
                    {
                        ddls[i].DataSource = lista;
                        ddls[i].DataTextField = "Descripcion";
                        ddls[i].DataValueField = "Id";
                        ddls[i].DataBind();
                    }
                    lista = contPlanCuentas.obtenerCuentasContablesByNivel(i + 2, Convert.ToInt32(ddls[i].SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }
        [WebMethod]
        public static string ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel(int jerarquia, int nivel)
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                var listaCuentas = contPlanCuentas.obtenerCuentasContablesByNivel(jerarquia, nivel);

                List<ListItemTemporal> listaCuentasTemporal = new List<ListItemTemporal>();

                foreach (var item in listaCuentas)
                {
                    listaCuentasTemporal.Add(new ListItemTemporal
                    {
                        id = item.Id.ToString(),
                        nombre = item.Descripcion
                    });
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(listaCuentasTemporal);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion

        [WebMethod]
        public static string EliminarRegistroDeTabla(int idCuentasContable_MayorTipoMovimiento)
        {
            try
            {
                int resultado = 0;
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();

                var mayortipoMovimiento_cuentaContable = contPlanCuentas.GetOne_CuentasContables_MayorTipoMovimiento(idCuentasContable_MayorTipoMovimiento);
                if (mayortipoMovimiento_cuentaContable.Id != 0)
                {
                    mayortipoMovimiento_cuentaContable.Estado = 0;

                    bool correcto = contPlanCuentas.CreateOrUpdateInDB_CuentasContables_MayorTipoMovimiento(mayortipoMovimiento_cuentaContable);
                    if (correcto) resultado = 1;
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(resultado);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        protected void lbtnCrearRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                bool existeUnaRelacion = contPlanCuentas.VerificarSiEl_MayorTipoMovimentoYaEstaAsignadoAUna_CuentaContable(Convert.ToInt32(dropList_Mayor_TipoDeMovimiento.SelectedValue));

                if (!existeUnaRelacion)
                {
                    CuentasContables_MayorTipoMovimiento cuentasContables_MayorTipoMovimiento = new CuentasContables_MayorTipoMovimiento();
                    cuentasContables_MayorTipoMovimiento.IdCuenta_Contable = Convert.ToInt32(DropListNivel4.SelectedValue);
                    cuentasContables_MayorTipoMovimiento.IdMayor_TipoMovimiento = Convert.ToInt32(dropList_Mayor_TipoDeMovimiento.SelectedValue);
                    cuentasContables_MayorTipoMovimiento.Estado = 1;

                    if (contPlanCuentas.AgregarRegistroToTable_CuentasContables_MayorTipoMovimiento(cuentasContables_MayorTipoMovimiento))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Registro creado correctamente", "Cuentas_Contables_Mayor.aspx"));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Ya se hay una cuenta contable relacionada con ese tipo de movimiento, debe eliminarlo si quiere agregar otro distinto."));
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error en clase: " + this + " Funcion: " + MethodBase.GetCurrentMethod().Name) + " Ex: " + ex.Message);
            }
        }

        [WebMethod]
        public static string TraerRegistrosDe_CuentasContables_MayorTipoMovimiento()
        {
            try
            {
                ControladorPlanCuentas contPlanCuentas = new ControladorPlanCuentas();
                var CuentasContables_MayorTipoMovimiento = contPlanCuentas.ObtenerTodos_CuentasContables_MayorTipoMovimiento();
                List<CuentasContables_MayorTipoMovimiento_Temporal> cuentasContables_MayorTipoMovimiento_Temporal = new List<CuentasContables_MayorTipoMovimiento_Temporal>();

                foreach (var item in CuentasContables_MayorTipoMovimiento)
                {
                    cuentasContables_MayorTipoMovimiento_Temporal.Add(new CuentasContables_MayorTipoMovimiento_Temporal
                    {
                        Id = item.Id.ToString(),
                        IdCuenta_Contable = item.Cuentas_Contables.Descripcion.ToString(),
                        IdMayor_TipoMovimiento = item.Mayor_TipoMovimiento.TipoMovimiento.ToString(),
                    });
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 5000000;
                string resultadoJSON = serializer.Serialize(cuentasContables_MayorTipoMovimiento_Temporal);

                return resultadoJSON;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }
}