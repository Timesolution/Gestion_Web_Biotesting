using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Sucursales
{
    public partial class ABMPuntoVenta : System.Web.UI.Page
    {
        Mensajes m = new Mensajes();
        //controlador
        controladorSucursal controlador = new controladorSucursal();
        ControladorEmpresa contr = new ControladorEmpresa();
        controladorUsuario contUser = new controladorUsuario();
        //PuntoVenta
        PuntoVenta ptoVenta = new PuntoVenta();
        Empresa emp = new Empresa();
        //para saber si es alta(1) o modificacion(2)
        private int  codigo;
        private int empresa;
        private int valor;
        private int idPuntoVenta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificarLogin();
                this.codigo = Convert.ToInt32(Request.QueryString["codigo"]);
                this.empresa = Convert.ToInt32(Request.QueryString["empresa"]);

                this.cargarPuntoVenta();

                this.cargarLabelSucursal();

                this.btnAgregar.HRef = "PuntoVentaABM.aspx?codigo=" + codigo + "&empresa=" + empresa + "&valor=1";
            }
            catch(Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error cargando puntos de venta. " + ex.Message));
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
                        if (s == "10")
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


        private void cargarLabelSucursal()
        {
            try
            {
                Sucursal s = controlador.obtenerSucursalID(this.codigo);
                lblSucursal.Text = " Sucursal: " + s.nombre;
            }
            catch
            {

            }
        }

        private void cargarPuntoVenta()
        {
            try
            {
                phPuntosVenta.Controls.Clear();
                List<PuntoVenta> ptoVenta = this.controlador.obtenerPtoVentaSucursal(codigo);
                foreach (PuntoVenta ptoV in ptoVenta)
                {
                    this.cargarPuntoVentaTable(ptoV);
                }

                

            }
            catch
            {

            }
        }

        private void cargarPuntoVentaTable(PuntoVenta ptoVenta)
        {
            try
            {


                TableRow tr = new TableRow();
                TableCell CelPuntoVenta = new TableCell();
                CelPuntoVenta.Text = ptoVenta.puntoVenta;
                CelPuntoVenta.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelPuntoVenta);

                TableCell CelFormaFacturar = new TableCell();
                CelFormaFacturar.Text = ptoVenta.formaFacturar;
                //CelFormaFacturar.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelFormaFacturar);

                if(ptoVenta.retiene_ib)
                {
                    TableCell CelRetieneIB = new TableCell();
                    CelRetieneIB.Text = "Si";
                    CelRetieneIB.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(CelRetieneIB);
                }
                else
                {
                    TableCell CelRetieneIB = new TableCell();
                    CelRetieneIB.Text = "No";
                    CelRetieneIB.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(CelRetieneIB);
                }

                if(ptoVenta.retiene_gan)
                {
                    TableCell CelRetieneGanancias = new TableCell();
                    CelRetieneGanancias.Text = "Si";
                    CelRetieneGanancias.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(CelRetieneGanancias);
                }
                else
                {
                    TableCell CelRetieneGanancias = new TableCell();
                    CelRetieneGanancias.Text = "No";
                    CelRetieneGanancias.VerticalAlign = VerticalAlign.Middle;
                    tr.Cells.Add(CelRetieneGanancias);
                }

                TableCell CelNombreFantasia = new TableCell();
                CelNombreFantasia.Text = ptoVenta.nombre_fantasia;
                CelNombreFantasia.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelNombreFantasia);

                TableCell CelDireccion = new TableCell();
                CelDireccion.Text = ptoVenta.direccion;
                CelDireccion.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelDireccion);

                TableCell CelEmpresa = new TableCell();
                //CelEmpresa.Text = ptoVenta.empresa.id.ToString();
                Empresa emp = contr.obtenerEmpresa(ptoVenta.empresa.id);
                CelEmpresa.Text = emp.RazonSocial;
                CelEmpresa.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(CelEmpresa);

                TableCell celPuntoVta = new TableCell();

                LinkButton btnEditar = new LinkButton();
                btnEditar.ID = ptoVenta.id.ToString();
                btnEditar.CssClass = "btn btn-info ui-tooltip";
                btnEditar.Attributes.Add("data-toggle", "tooltip");
                btnEditar.Attributes.Add("title data-original-title", "Editar");
                btnEditar.Text = "<span class='shortcut-icon icon-pencil'></span>";
                //btnEditar.Font.Size = 9;
                //btnEditar.Click += new EventHandler(this.editarPuntoVenta);
                btnEditar.PostBackUrl = "PuntoVentaABM.aspx?codigo=" + codigo + "&empresa=" + empresa + "&valor=2&id=" + ptoVenta.id;
                celPuntoVta.Controls.Add(btnEditar);

                Literal l = new Literal();
                l.Text = "&nbsp";
                celPuntoVta.Controls.Add(l);


                LinkButton btnEliminar = new LinkButton();
                btnEliminar.ID = "btnEliminar_" + ptoVenta.id;
                btnEliminar.CssClass = "btn btn-info";
                btnEliminar.Attributes.Add("data-toggle", "modal");
                btnEliminar.Attributes.Add("href", "#modalConfirmacion");
                btnEliminar.Text = "<span class='shortcut-icon icon-trash'></span>";
                btnEliminar.Font.Size = 9;
                //btnEliminar.Click += new EventHandler(this.eliminarCobro);
                //btnEliminar.Attributes.Add("onclientclick", "abrirdialog("+ movV.id +")");
                btnEliminar.OnClientClick = "abrirdialog(" + ptoVenta.id + ");";
                //btnEliminar.OnClientClick = "mostrarMensaje(this.id)";
                celPuntoVta.Controls.Add(btnEliminar);
                celPuntoVta.Width = Unit.Percentage(10);
                tr.Cells.Add(celPuntoVta);

                phPuntosVenta.Controls.Add(tr);


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando punto de venta en la lista. " + ex.Message));
            }
        }


        protected void btnSi_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(this.txtMovimiento.Text);
                PuntoVenta pv = this.controlador.obtenerPtoVentaId(idPerfil);
                pv.estado = 0;
                int i = this.controlador.editarPtoVenta(pv);
                if (i > 0)
                {
                    //agrego bien
                    Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", " Baja Punto de Venta:  " + pv.nombre_fantasia);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Punto de Venta eliminado con exito", "ABMPuntoVenta.aspx?codigo=" + codigo + "&empresa=" + empresa));
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error eliminando Punto de Venta"));

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al eliminar Punto de Venta. " + ex.Message));
            }
        }
    }
}