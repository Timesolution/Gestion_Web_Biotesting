using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Entitys;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web
{
    public partial class _Default : Page
    {
        Mensajes mje = new Mensajes();
        controladorSucursal contSucu = new controladorSucursal();
        controladorFacturacion contFac = new controladorFacturacion();
        controladorCliente contCliente = new controladorCliente();
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorEmpresa contEmp = new ControladorEmpresa();
        controladorUsuario contUser= new controladorUsuario();
        UsuarioMemo user = new UsuarioMemo();
        int idMemo;
        int valor;
        int facturasSucursales;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.valor = Convert.ToInt32(Request.QueryString["m"]);
                if (!IsPostBack)
                {
                    
                    this.VerificarLogin();
                    this.cargarSucursal();
                    this.cargarMemo();
                    this.CargarBotonMuñoz();
                    this.obtenerAlertaPedidos();
                    this.cargarVencimientos();
                    this.cargarVencidos();
                    this.PopUpCRMPendiente();

                    string mascotas = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Mascotas");
                    if (mascotas == "1")
                    {
                        this.panelMascotas.Visible = true;
                    }
                }

                if (this.valor == 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", mje.mensajeDenegado());
                }

            }
            catch
            {

            }
        }

        private void CargarBotonMuñoz()
        {
            if (WebConfigurationManager.AppSettings["EsMunioz"] == "1"){
                iconoMuñoz.Attributes.Add("class", "shortcut-icon icon-mobile-phone");
                spanMuñoz.InnerText = "Consola Mobile";
                etiquetaMuñoz.HRef = "Formularios/Articulos/ArticulosMobile.aspx";
                
            }
            else
            {
                iconoMuñoz.Attributes.Add("class", "shortcut-icon  icon-adjust");
                spanMuñoz.InnerText = "Balance";
                etiquetaMuñoz.HRef = "Formularios/Reportes/BalanceF.aspx";
            }
        

        }

        private void VerificarLogin()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Account/Login.aspx");
                }
                else
                {
                    //verifico perfil
                    string perfil = Session["Login_NombrePerfil"] as string;
                    if (WebConfigurationManager.AppSettings["EsTestingBio"] == "1")
                    {

                        if (perfil == "Cliente")
                        {
                            //desactivo acciones
                            this.phClientes.Visible = true;
                            DataTable dt = contUser.ComprobarVCCyVP();
                            if (dt.Rows[0]["estado"].ToString() == "0" && perfil == "Cliente")
                            {
                                btnHacerPedido.Visible = false;
                            }
                            if (dt.Rows[1]["estado"].ToString() == "0" && perfil == "Cliente")
                            {
                                btnCuentaCorriente.Visible = false;
                            }
                            if (perfil == "Cliente")
                            {
                                btnCliente2.Visible = false;
                            }
                        }
                        if (perfil == "Distribuidor")
                        {
                            this.phClientes.Visible = true;
                            btnCliente2.Visible = true;
                        }

                        if (perfil != "Distribuidor" && perfil != "Cliente")
                        {

                            //importacion, usa PARKER
                            if (perfil == "Importacion")
                            {
                                this.phImportacion.Visible = true;
                                return;
                            }

                            this.phMain.Visible = true;
                            //verifico si es super admin                        
                            if (perfil == "SuperAdministrador")
                            {
                                this.facturasSucursales = 1;
                            }
                            if (perfil == "Vendedor")
                            {
                                this.lbtnAlertaPedidos.Attributes.Add("style", "display:none;");
                            }
                        }
                    }
                    else
                    {

                        if (perfil == "Cliente")
                        {
                            //desactivo acciones
                            this.phClientes.Visible = true;

                        }
                        else
                        {
                            //importacion, usa PARKER
                            if (perfil == "Importacion")
                            {
                                this.phImportacion.Visible = true;
                                return;
                            }

                            this.phMain.Visible = true;
                            //verifico si es super admin                        
                            if (perfil == "SuperAdministrador")
                            {
                                this.facturasSucursales = 1;
                            }
                            if (perfil == "Vendedor")
                            {
                                this.lbtnAlertaPedidos.Attributes.Add("style", "display:none;");
                            }
                        }
                        if (WebConfigurationManager.AppSettings["EsDonus"] == "1")
                        {
                            btnRecepcionMercaderia.Visible = true;
                        }
                        else
                        {
                            btnHacerPedido.Visible = true;
                        }
                    }

                    string nombreUser = Session["Login_UserNafta"] as string;
                    if (nombreUser == "GNC" || nombreUser == "Nafta" || nombreUser == "Playa")
                    {
                        Response.Redirect("/Formularios/Facturas/ABMFacturasRapido.aspx");
                    }
                }
            }
            catch
            {

            }
        }

        //private void cargarDatos()
        //{
        //    try
        //    {
        //        string usuario = Session["User"] as string;
        //        lblUsuario.Text = usuario;

        //        int idSucursal = (int)Session["Login_SucUser"];
        //        Sucursal s = this.contSucu.obtenerSucursalID(idSucursal);
        //        lblSucursal.Text = s.nombre;

        //        int Tclientes = this.contCliente.obtenerTotalClientes();
        //        int TFacturas = this.contFac.obtenerTotalFacturasDia(idSucursal);
        //        int TArticulos = this.contArticulo.obtenerTotalArticulos();

        //        lblTotalClientes.Text = Tclientes.ToString();
        //        lblTotalFacturas.Text = TFacturas.ToString();
        //        lblTotalProductos.Text = TArticulos.ToString();


        //    }
        //    catch
        //    {

        //    }
        //}



        //private void cargarVencidosPH(Clientes_Eventos crm, string nombreCliente, int tipo)
        //{
        //    try
        //    {


        //        TableRow tr = new TableRow();

        //        TableCell celVencimiento = new TableCell();
        //        celVencimiento.BorderStyle = BorderStyle.Dotted;

        //        Label vencimiento = new Label();
        //        vencimiento.Text += @"<li style='border-bottom: 1px dotted #CCC;width:100%;'>";
        //        vencimiento.Text += @"<div class='news-item-detail'>";
        //        vencimiento.Text += @"<a href='Formularios/Facturas/CRM.aspx?fechadesde=01/01/2000&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaVencimientoDesde=01/01/2000&fechaVencimientoHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&cl=" + crm.Cliente + "&estado=" + crm.Estado + "&fpf=1&fpfv=0&us=" + crm.Usuario + "' class='news-item-title'>" + nombreCliente + "</a>";

        //        vencimiento.Text += @"<p class='news-item-preview'>" + this.generarDetalle(crm) + "</p>";
        //        vencimiento.Text += @"</div>";
        //        vencimiento.Text += @"<div class='news-item-date'>";
        //        vencimiento.Text += @"<span class='news-item-day'>" + crm.Fecha.Value.Day + "</span>";
        //        vencimiento.Text += @"<span class='news-item-month'>" + crm.Fecha.Value.ToString("MMMM yyyy", new CultureInfo("es-AR")) + "</span>";
        //        vencimiento.Text += @"</div>";
        //        vencimiento.Text += @"</li>";
        //        celVencimiento.Controls.Add(vencimiento);

        //        tr.Controls.Add(celVencimiento);
        //        this.phSeguimientoVencidos.Controls.Add(tr);
        //    }
        //    catch
        //    {

        //    }
        //}

        public void PopUpCRMPendiente()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                int idUsuario = Convert.ToInt32(Session["Login_IdUser"]);

                List<Clientes_Eventos> listSeguimiento = controladorClienteEntity.obtenerVencimientosProximosTOP(idUsuario);
                if (listSeguimiento != null && listSeguimiento.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "modalCRM();", true);

                }

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error cargando sucursales. " + ex.Message));
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarVencidos()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                int idUsuario = Convert.ToInt32(Session["Login_IdUser"]);

                List<Clientes_Eventos> listSeguimiento = controladorClienteEntity.obtenerVencidosTOP(idUsuario);
                if (listSeguimiento != null)
                {
                    this.phSeguimientoVencidos.Controls.Clear();
                    foreach (var list in listSeguimiento)
                    {
                        var cliente = controladorClienteEntity.ObtenerClienteId((int)list.Cliente);
                        this.cargarVencimientosPH(list, cliente.razonSocial, 1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarVencimientos()
        {
            try
            {
                ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                int idUsuario = Convert.ToInt32(Session["Login_IdUser"]);

                List<Clientes_Eventos> listSeguimiento = controladorClienteEntity.obtenerVencimientosProximosTOP(idUsuario);
                if (listSeguimiento != null)
                {
                    this.phSeguimiento.Controls.Clear();
                    foreach (var list in listSeguimiento)
                    {
                        var cliente = controladorClienteEntity.ObtenerClienteId((int)list.Cliente);
                        this.cargarVencimientosPH(list, cliente.razonSocial, 2);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cargarVencimientosPH(Clientes_Eventos crm, string nombreCliente, int tipo)
        {
            try
            {


                TableRow tr = new TableRow();

                TableCell celVencimiento = new TableCell();
                celVencimiento.BorderStyle = BorderStyle.Dotted;

                Label vencimiento = new Label();
                vencimiento.Text += @"<li style='border-bottom: 1px dotted #CCC;width:100%;'>";
                vencimiento.Text += @"<div class='news-item-detail'>";
                vencimiento.Text += @"<a href='Formularios/Facturas/CRM.aspx?fechadesde=01/01/2000&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaVencimientoDesde=01/01/2000&fechaVencimientoHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&cl=" + crm.Cliente + "&estado=" + crm.Estado + "&fpf=1&fpfv=0&us=" + crm.Usuario + "' class='news-item-title'>" + nombreCliente + "</a>";

                vencimiento.Text += @"<p class='news-item-preview'>" + this.generarDetalle(crm) + "</p>";
                vencimiento.Text += @"</div>";
                vencimiento.Text += @"<div class='news-item-date'>";
                vencimiento.Text += @"<span class='news-item-day'>" + crm.Vencimiento.Value.Day + "</span>";
                vencimiento.Text += @"<span class='news-item-month'>" + crm.Vencimiento.Value.ToString("MMMM yyyy", new CultureInfo("es-AR")) + "</span>";
                vencimiento.Text += @"</div>";
                vencimiento.Text += @"</li>";
                celVencimiento.Controls.Add(vencimiento);

                tr.Controls.Add(celVencimiento);

                if (tipo == 1)
                {
                    this.phSeguimientoVencidos.Controls.Add(tr);
                }

                if (tipo == 2)
                {
                    this.phSeguimiento.Controls.Add(tr);

                }


            }
            catch
            {

            }
        }

        private string generarDetalle(Clientes_Eventos crm)
        {
            try
            {
                string detalle = "";
                TimeSpan ts = crm.Vencimiento.Value - DateTime.Today;
                {
                    if (ts.Days >= 0)
                    {
                        detalle = "Quedan " + ts.Days + " dias de " + crm.Tarea;
                    }
                    else
                    {
                        detalle = "*Pendiente* " + crm.Tarea + " hace " + Math.Abs(ts.Days) + " dias.";
                    }

                    return detalle;
                }
            }
            catch
            {
                return null;
            }
        }

        private void cargarSucursal()
        {
            try
            {
                int idEmpresa = (int)Session["Login_EmpUser"];
                phSucursales.Controls.Clear();
                DataTable dt = this.contSucu.obtenerSucursalesDT(idEmpresa);
                foreach (DataRow dr in dt.Rows)
                {
                    this.cargarSucursalesTable(dr);
                }
            }
            catch
            {

            }
        }

        private void cargarSucursalesTable(DataRow dr)
        {
            try
            {
                int emp = (int)Session["Login_EmpUser"];
                TableRow tr = new TableRow();

                TableCell celNombre = new TableCell();
                celNombre.Text = dr["nombre"].ToString();
                celNombre.VerticalAlign = VerticalAlign.Bottom;
                tr.Cells.Add(celNombre);

                LinkButton btnPuntoVenta = new LinkButton();
                TableCell celPuntoVta = new TableCell();
                //btnPuntoVenta.ID = sucu.id.ToString();
                btnPuntoVenta.CssClass = "badge pull-right";
                int TotalFactura = this.contFac.obtenerTotalFacturasDia(Convert.ToInt32(dr["id"]));
                btnPuntoVenta.Text = TotalFactura.ToString();
                //btnPuntoVenta.PostBackUrl = "Formularios/Facturas/FacturasF?fechadesde=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&Sucursal=" + Convert.ToInt32(dr["id"]);
                btnPuntoVenta.PostBackUrl = "Default.aspx";
                if (this.facturasSucursales == 1)
                {
                    btnPuntoVenta.PostBackUrl = "Formularios/Facturas/FacturasF?fechadesde=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&Sucursal=" + Convert.ToInt32(dr["id"]) + "&Emp=" + emp;
                }
                else
                {
                    int suc = (int)Session["Login_SucUser"];
                    if (suc == Convert.ToInt32(dr["id"]))
                    {
                        btnPuntoVenta.PostBackUrl = "Formularios/Facturas/FacturasF?fechadesde=" + DateTime.Now.ToString("dd/MM/yyyy") + "&fechaHasta=" + DateTime.Now.ToString("dd/MM/yyyy") + "&Sucursal=" + Convert.ToInt32(dr["id"]) + "&Emp=" + emp;
                    }
                }

                celPuntoVta.VerticalAlign = VerticalAlign.Middle;
                celPuntoVta.Controls.Add(btnPuntoVenta);
                tr.Cells.Add(celPuntoVta);

                phSucursales.Controls.Add(tr);

            }
            catch
            {

            }
        }

        private void cargarMemo()
        {
            try
            {
                int idUsuario = (int)Session["Login_IdUser"];
                //phBlockNotas.Controls.Clear();
                DataTable dt = this.user.obtenerMemoByUsuario(idUsuario);
                foreach (DataRow dr in dt.Rows)
                {
                    this.idMemo = Convert.ToInt32(dr["id"]);
                    Session.Add("Default_Memo", idMemo);
                    txtMemo.Text = dr["memo"].ToString();
                }
            }
            catch
            {

            }
        }


        //private void editarMemo(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Response.Redirect("ABMTarjetas.aspx?valor=2&id=" + (sender as LinkButton).ID);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private string obtenerFecha()
        {
            string dia = DateTime.Now.DayOfWeek.ToString(new CultureInfo("es-ES"));
            string fecha = DateTime.Now.Day.ToString();
            string año = DateTime.Now.Year.ToString();
            string fechaCompleta = DateTime.Now.ToLongDateString();
            return fechaCompleta.ToUpper();
        }

        private void obtenerAlertaPedidos()
        {
            try
            {
                ControladorPedidoEntity contPedidoEnt = new ControladorPedidoEntity();
                int cant = contPedidoEnt.obtenerCantidadPedidosPendienteVendedor();
                if (cant < 0)
                    cant = 0;

                this.lbtnAlertaPedidos.Text += cant.ToString();

            }
            catch
            {

            }
        }
        protected void lbtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtMemo.Text))
                {
                    UsuarioMemo um = new UsuarioMemo();
                    int idUsuario = (int)Session["Login_IdUser"];
                    um.usuario.id = idUsuario;
                    um.memo = this.txtMemo.Text;
                    int i = um.agregar(um);
                    if (i > 0)
                    {
                        //agrego bien
                        this.txtMemo.Text = "";
                        //this.cargarMemo();

                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
        }

        protected void txtMemo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.txtMemo.Text))
                {
                    UsuarioMemo um = new UsuarioMemo();
                    int idUsuario = (int)Session["Login_IdUser"];
                    um.id = (int)Session["Default_Memo"];
                    um.usuario.id = idUsuario;
                    um.memo = this.txtMemo.Text;
                    int i = um.modificar(um);
                    if (i > 0)
                    {
                        //agrego bien
                        //this.cargarMemo();

                    }
                    else
                    {

                    }
                }
                else
                {
                    UsuarioMemo um = new UsuarioMemo();
                    int idUsuario = (int)Session["Login_IdUser"];
                    um.id = (int)Session["Default_Memo"];
                    um.usuario.id = idUsuario;
                    um.memo = " ";
                    int i = um.modificar(um);
                    if (i > 0)
                    {
                        //agrego bien
                        //this.cargarMemo();

                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
        }

        protected void lbtnAlertaPedidos_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Formularios/Facturas/PedidosP.aspx?&Fechadesde=" + DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy") + "&FechaHasta=" + DateTime.Today.ToString("dd/MM/yyyy") + "&estado=5&tf=1");
            }
            catch
            {

            }
        }

        private void redirigirMascotas(int menu)
        {
            try
            {
                string user = Session["User"].ToString();
                string pass = Session["Pass"].ToString();

                Response.Redirect("/Mascotas/Account/Login.aspx?menu=" + menu + "&us=" + user + "&pw=" + pass);
            }
            catch
            {

            }
        }
        protected void lbtnMascotasLink_Click(object sender, EventArgs e)
        {
            try
            {
                this.redirigirMascotas(1);
            }
            catch
            {

            }
        }

        protected void lbtnMascotasLink2_Click(object sender, EventArgs e)
        {
            try
            {
                this.redirigirMascotas(2);
            }
            catch
            {

            }
        }

        protected void lbtnMascotasLink3_Click(object sender, EventArgs e)
        {
            try
            {
                this.redirigirMascotas(3);
            }
            catch
            {

            }
        }

        protected void lbtnMascotasLink4_Click(object sender, EventArgs e)
        {
            try
            {
                this.redirigirMascotas(4);
            }
            catch
            {

            }
        }
    }
}