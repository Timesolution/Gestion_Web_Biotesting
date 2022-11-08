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

namespace Gestion_Web.Formularios.Facturas
{
    public partial class ConsolidarP3 : System.Web.UI.Page
    {

        ControladorPedido controladorPedido = new ControladorPedido();
        controladorArticulo controladorArticulo = new controladorArticulo();
        controladorSucursal cs = new controladorSucursal();
        controladorCliente cc = new controladorCliente();
        Configuracion confEstados = new Configuracion();
        ControladorClienteEntity contClienteEntity = new ControladorClienteEntity();
        private ControladorPedidoEntity contPedidoEntity = new ControladorPedidoEntity();


        TipoDocumento tp = new TipoDocumento();
        Mensajes m = new Mensajes();
        List<Pedido> listaPedidos;
        private int idVendedor;
        private string pedidos;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                    this.VerificarLogin();
                this.idVendedor = (int)Session["Login_Vendedor"];
                pedidos = Request.QueryString["pedidos"];
                if (!IsPostBack)
                {
                    lblRealizador.Text = (string)Session["Login_NombrePerfil"];
                    CargarPedidos(pedidos);//carga el front

                    CargarDireccionesDistribuidor_y_Pedidos(pedidos);

                    //CargarDireccionesDistribuidor_e_Hijos();
                    obtenerNroPedido();// carga el front
                    CargarDistribuidor();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Ocurrio un error " + ex.Message));
            }
        }

        private void CargarDireccionesDistribuidor_y_Pedidos(string pedidos)
        {
            try
            {
                
                controladorCliente cc = new controladorCliente();
                controladorDireccion cd = new controladorDireccion();
               
                string[] pedidosSplit = pedidos.Split(';');

                List<Cliente> clientes = new List<Cliente>();
                List<Pedido> listaPedidosConsolidados= controladorPedido.obtenerPedidosConsolidados(pedidos);
                int IdClienteAnterior;
                //listaPedidosConsolidados.Distinct();
                listaPedidosConsolidados.OrderBy(x => x.cliente.id).ToList();
                IdClienteAnterior = listaPedidosConsolidados.First().cliente.id;
                bool primero = true;

                foreach ( Pedido p in listaPedidosConsolidados)
                {
                    if (primero == true)
                    {
                        IdClienteAnterior = p.cliente.id;
                        clientes.Add(p.cliente);
                        primero = false;
                    }

                    if (IdClienteAnterior != p.cliente.id)
                    {
                     
                        clientes.Add(p.cliente);

                    }
                }



                Cliente clienteDistribuidor = cc.obtenerClienteID(idVendedor);
                
                var lista = cc.obtenerDirecciones(idVendedor);

                var listaHijos = cd.ObtenerDireccionesDeHijos(idVendedor);

                lista.Insert(0, (new direccion { id = -1, direc = "Seleccione..." }));
                listDirecciones.Items.Clear();

                foreach (direccion d in lista)
                {
                    if (d.direc == "Seleccione...")
                        listDirecciones.Items.Add(new ListItem(d.direc, d.id.ToString()));
                    else
                        listDirecciones.Items.Add(new ListItem(clienteDistribuidor.razonSocial + ": " + d.direc + ", " + d.localidad + ", CP:" + d.codPostal + ", " + d.provincia, d.id.ToString()));
                }
                foreach (var cl in clientes)
                {
                    //cl.direcciones = cc.obtenerDirecciones(cl.id);
                    
                    foreach (direccion d in cl.direcciones)
                    {
                        listDirecciones.Items.Add(new ListItem(cl.razonSocial + ": " + d.direc + ", " + d.localidad + ", CP:" + d.codPostal + ", " + d.provincia, d.id.ToString()));
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CargarDistribuidor()
        {
            try
            {
                controladorCliente cc = new controladorCliente();

                Cliente clienteDistribuidor = cc.obtenerClienteID(idVendedor);

                lblDistribuidor.Text = clienteDistribuidor.alias;

            }
            catch (Exception)
            {

                throw;
            }        
        }

        private void CargarDatosPedido()
        {
            try
            {

                ControladorEmpresa ce = new ControladorEmpresa();
                controladorVendedor cv = new controladorVendedor();
                ControladorFormasPago cfp = new ControladorFormasPago();
                controladorListaPrecio clp = new controladorListaPrecio();

                int Empresa = 1; //Laboratorio Bioessencia SA.
                int Sucursal = 21; // Bioesencia
                int vendedor = 30;
                int formapago = 7;// cuenta corriente.
                int lista = 3; //probablemente no se utilice aca.

                Pedido p = new Pedido();
                
                p.domicilioEntrega = listDirecciones.SelectedItem.Text;
                p.cliente = cc.obtenerClienteID(idVendedor);
                p.empresa = ce.obtenerEmpresaByIdSucursal(Sucursal);
                p.vendedor = cv.obtenerVendedorID(vendedor);
                p.formaPAgo.forma = "Cuenta Corriente";
                p.formaPAgo.id = 7;
                p.listaP = clp.obtenerlistaPrecioID(lista);
                //p.numero = lblNroPedido.Text.Replace("Pedido N° ","");

                ObtenerItemsPedidoDeHijos(pedidos,p);
                Session["Pedido"] = p;

            }
            catch (Exception ex)
            {

            }
        }
        private void generarPedido()
        {
            try
            {
                Pedido p = Session["Pedido"] as Pedido;
                Cliente cl = this.cc.obtenerClienteID(idVendedor);
                //p.cliente = cl;
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + 62210 + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'PedidosP.aspx';", true);//ConsolidarP3.aspx?" + pedidos + "

                //Verifico que el tipo de moneda de todos los items sea el mismo
                //if (!this.verificarMonedaItems())
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("El tipo de moneda de los items no puede ser distinto, verifíquelo. "));
                //    return;
                //}


                if (p.cliente.id > 0)
                {
                    if (p.items.Count > 0)
                    {
                        //this.Pedido.items = items;
                        ////obtengo datos
                        // cambiar
                        p.empresa.id = 1;
                        p.sucursal.id = 21;
                        p.ptoV = cs.obtenerPtoVentaId(35);
                        p.fecha = DateTime.Now;
                        p.cotizacion.id = -1;
                        p.vendedor.id = cl.vendedor.id;
                        p.formaPAgo.id = cl.formaPago.id;
                        p.listaP.id = cl.lisPrecio.id;
                        p.comentario = "";
                        p.neto10 = Convert.ToDecimal(0.0);
                        //datos entrega
                        p.entrega.Id = -1;
                        p.fechaEntrega = DateTime.Now;
                        //p.domicilioEntrega = this.txtDomicilioEntrega.Text;
                        //p.domicilioEntrega = this.dropList_DomicilioEntrega.Text;

                        p.domicilioEntrega = listDirecciones.Items.Count > 1 ? listDirecciones.SelectedItem.Text : "-";

                        //dropList_DomicilioEntrega.Items.Add(new ListItem(item.ItemArray[1] + ", " + item.ItemArray[2] + ", " + item.ItemArray[3], item.ItemArray[3].ToString()));
                        p.horaEntrega = "";
                        p.zonaEntrega = "";
                        //p.senia = this.txtSenia.Text;
                        //ObtenerItemsPedidoDeHijos(pedidos, p);

                        tp = controladorPedido.obtenerTipoDoc("Pedido");
                        
                        p.tipo = tp;
                        if (ViewState["Borrador"] != null)
                        {
                            p.estado = this.controladorPedido.obtenerEstadoDesc("Borrador");
                            ViewState["Borrador"] = null;
                        }
                        else
                        {
                            string perfil = Session["Login_NombrePerfil"] as string;
                            if (perfil == "Vendedor")
                            {
                                p.estado = this.controladorPedido.obtenerEstadoDesc("Pendiente Vendedor");
                            }
                            else
                            {
                                if (perfil == "Cliente" || perfil =="Bio-Lider")
                                {
                                    p.estado = this.controladorPedido.obtenerEstadoDesc("A Autorizar");
                                }
                                else
                                {
                                    p.estado.id = int.Parse(confEstados.EstadoInicialPedidos);
                                }
                            }
                        }
                        

                        int i = 0;
                        
                        i = this.controladorPedido.ProcesarPedido(p, pedidos);
                       
                        if (i > 0)
                        {
                            ControladorPedidoEntity contPedEnt = new ControladorPedidoEntity();
                            ControladorClienteEntity controladorClienteEntity = new ControladorClienteEntity();
                            ControladorPedido Contpedido1 = new ControladorPedido();

                            //if (cotizacion == 1 && ListReferido.SelectedValue != "-1")
                            //{
                            //    controladorReferido.EliminarReferidoCotizacion(i);
                            //    controladorReferido.AgregarReferidoCotizacion(i, Convert.ToInt32(ListReferido.SelectedValue));
                            //}
                            //else if (cotizacion == 0 && ListReferido.SelectedValue != "-1")
                            //{
                            //    controladorReferido.EliminarReferidoPedido(i);
                            //    controladorReferido.AgregarReferidoPedido(i, Convert.ToInt32(ListReferido.SelectedValue));
                            //}
                            //Log.EscribirSQL(1, "INFO", "verificando check a enviar mail" + chkEnviarMail.Checked + txtMailEntrega.Text);
                            //if (this.chkEnviarMail.Checked == true && !String.IsNullOrEmpty(this.txtMailEntrega.Text))
                            //{

                            //    Log.EscribirSQL(1, "INFO", "entrando a enviar mail idpedido: " + i);

                            //    this.Enviarmail(i);
                            //}
                            //else
                            //{
                            //    Log.EscribirSQL(1, "INFO", "no ingreso al enviar mail");

                            //}


                            //if (accion == 4)//agrego el dato de la cotizacion y el pedido generado
                            //{
                            //    int t = contPedEnt.agregarPedidoCotizacion(i, idCotizacion);
                            //    //cambio estado a cotizacion
                            //    t = contPedEnt.CambiarEstadoCotizaciones(idCotizacion, 6);
                            //}
                            //Verifico si utiliza modo distribución (Cliente_Referidos, Pedidos_Referidos)
                            //if (WebConfigurationManager.AppSettings.Get("Distribucion") == "1")
                            //{
                            //    int j = contPedEnt.agregarPedidoReferido(i, p.cliente.id);
                            //}


                            Pedido pedido1 = Contpedido1.obtenerPedidoId(i);

                            Clientes_Eventos eventos = new Clientes_Eventos();

                            eventos.Cliente = p.cliente.id;

                            //if (cotizacion == 1)
                            //{
                            //    eventos.Descripcion = "Emisión de Cotización # " + pedido1.numero;
                            //}
                            //else
                            //{
                                eventos.Descripcion = "Emisión de Pedido # " + pedido1.numero;
                            //}

                            eventos.Fecha = Convert.ToDateTime(DateTime.Now, new CultureInfo("es-AR"));
                            eventos.Usuario = Convert.ToInt32((int)Session["Login_IdUser"]);
                            eventos.Tarea = "";
                            eventos.Estado = controladorClienteEntity.ObtenerIdEstadoByDescripcion("Finalizado");
                            eventos.Vencimiento = null;

                            this.contClienteEntity.agregarEventoCliente(eventos);
                            if (contPedidoEntity.verificarPadre(p.cliente.id) > 0 && (int) Session["Login_IdPerfil"] ==24)
                            {
                                int j = contPedidoEntity.agregarPedidoReferido(i, p.cliente.id);

                            }
                            //guardo los articulos a pedir
                            //this.guardarArticulosPedir();
                            //Session.Remove("Pedido");
                            //this.btnAgregar.Visible = false;
                            //this.btnNuevo.Visible = true;
                            Log.EscribirSQL((int)Session["Login_IdUser"], "INFO", "Alta  Pedido: " + this.lblNroPedido.Text);
                            //if (cotizacion == 1)
                            //{
                            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&co=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'ABMPedidos.aspx?c=1';", true);
                            //}
                            //else
                            //{
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "window.open('ImpresionPedido.aspx?a=1&Pedido=" + i + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');location.href = 'PedidosP.aspx';", true);
                            //Response.Redirect("PedidosP.aspx");
                            //}
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("No se pudo agregar Pedido "));
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe agregar articulos al Pedido "));
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Debe seleccionar un cliente "));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando Pedido. " + ex.Message));
            }
        }


        private void guardarArticulosPedir()
        {
            try
            {
                ControladorArticulosEntity contArtEnt = new ControladorArticulosEntity();
                List<Articulos_PedirOC> artPedir = Session["ArtPedir"] as List<Articulos_PedirOC>;
                if (artPedir.Count > 0 && artPedir != null)
                {
                    contArtEnt.agregarArticulosAPedir(artPedir);
                }
                Session.Remove("ArtPedir");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error guardando articulos a pedir. " + ex.Message));
            }
        }
        
        
        private void obtenerNroPedido()
        {
            try
            {
                  
                        int ptoVenta =35;//el ptoventa =35 es el punto de venta electronico 
                        PuntoVenta pv = cs.obtenerPtoVentaId(35);
                        int nro = this.controladorPedido.obtenerPedidoNumero(ptoVenta, "Pedido");
                        this.lblNroPedido.Text = pv.puntoVenta + "-" + nro.ToString().PadLeft(8, '0');
                    
                

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error obteniendo numero de Cotizacion. " + ex.Message));
            }
        }
        private void CargarDireccionesDistribuidor_e_Hijos()
        {
            try
            {
                //DataTable Direcciones = new DataTable();
                controladorCliente cc = new controladorCliente();
                controladorDireccion cd = new controladorDireccion();
                //Direcciones = cc.obtenerDireccionCliente(idVendedor,1);
                Cliente clienteDistribuidor = cc.obtenerClienteID(idVendedor);
                var lista = cc.obtenerDirecciones(idVendedor);
                var listaHijos = cd.ObtenerDireccionesDeHijos(idVendedor);
                var clientes = cc.obtenerClientesReducDistribuidor(1,idVendedor);
                //lista =lista.Concat(listaHijos).ToList();
                lista.Insert(0, (new direccion { id = -1, direc = "Seleccione..." }));
                //var seleccione= new direccion { id = -1, direc = "Seleccione..." };
                listDirecciones.Items.Clear();
                //listDirecciones.Items.Add(new ListItem("Seleccione", "-1"));

                foreach (direccion d in lista)
                {
                    if(d.direc== "Seleccione...")
                        listDirecciones.Items.Add(new ListItem(d.direc , d.id.ToString()));
                    else
                    listDirecciones.Items.Add(new ListItem(clienteDistribuidor.razonSocial + ": " + d.direc+", "+ d.localidad + ", CP:" + d.codPostal + ", " + d.provincia, d.id.ToString()));
                }
                foreach(var cl in clientes)
                {
                    cl.direcciones= cc.obtenerDirecciones(cl.id);

                    foreach (direccion d in cl.direcciones)
                    {
                            listDirecciones.Items.Add(new ListItem(cl.razonSocial + ": "+ d.direc + ", " + d.localidad + ", CP:" + d.codPostal + ", " + d.provincia, d.id.ToString()));
                    }
                }
                


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SetearColumnasPedidos(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string codigo = dr["codigo"].ToString();
                    string descripcion = dr["descripcion"].ToString();
                    decimal cantidad = Convert.ToDecimal(dr["cantidad"]);
                    string pedidos = dr["idPedido"].ToString();
                    string ubicacion = dr["ubicacion"].ToString();


                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = id.ToString();

                    //Celdas
                    TableCell celCodigo = new TableCell();
                    celCodigo.Text = codigo;
                    celCodigo.VerticalAlign = VerticalAlign.Middle;
                    celCodigo.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celCodigo);

                    //Celdas
                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = descripcion;
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celDescripcion);



                    ////Celdas
                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = cantidad.ToString();
                    celCantidad.VerticalAlign = VerticalAlign.Middle;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(celCantidad);

                    ////Celdas
                    //TableCell celUbicacion = new TableCell();
                    //celUbicacion.Text = ubicacion;
                    //celUbicacion.VerticalAlign = VerticalAlign.Middle;
                    //celUbicacion.HorizontalAlign = HorizontalAlign.Left;
                    //tr.Cells.Add(celUbicacion);
                    ////arego fila a tabla

                    //TableCell celAccion = new TableCell();
                    //Literal l1 = new Literal();
                    //l1.Text = "&nbsp";
                    //celAccion.Controls.Add(l1);
                    //LinkButton btnEliminar = new LinkButton();
                    //btnEliminar.CssClass = "btn btn-info";
                    //btnEliminar.ID = tr.ID + "," + pedidos;
                    //btnEliminar.Text = "<span class='shortcut-icon icon-arrow-up'></span>";
                    ////btnEliminar.Attributes.Add("onclick", "editarCantidades");
                    //btnEliminar.Click += new EventHandler(this.editarCantidades);
                    //celAccion.Controls.Add(btnEliminar);
                    //celAccion.Width = Unit.Percentage(21);
                    //celAccion.VerticalAlign = VerticalAlign.Middle;


                    //tr.Cells.Add(celAccion);


                    phPedidos.Controls.Add(tr);


                }
            }
            catch (Exception ex)
            {

                
            }
            
        }

        public void SetearColumnasArticulos(DataTable dt,string idArticulo)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string numero = dr["numero"].ToString();
                    decimal cantidad = Convert.ToDecimal(dr["cantidad"]);


                    //fila
                    TableRow tr = new TableRow();
                    tr.ID = id.ToString() +","+ idArticulo;
                    //Celdas
                    TableCell celDescripcion = new TableCell();
                    celDescripcion.Text = numero;
                    celDescripcion.VerticalAlign = VerticalAlign.Middle;
                    celDescripcion.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(celDescripcion);

                    //Celdas
                    TableCell celCantidad = new TableCell();
                    celCantidad.Text = cantidad.ToString();
                    celCantidad.VerticalAlign = VerticalAlign.Middle;
                    celCantidad.HorizontalAlign = HorizontalAlign.Right;
                    celCantidad.Width = Unit.Percentage(40);
                    tr.Cells.Add(celCantidad);

                    //arego fila a tabla

                    TableCell celCantidadEntregar = new TableCell();
                    TextBox txtCant = new TextBox();
                    txtCant.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    txtCant.ID = "Txt"+tr.ID;
                    txtCant.CssClass = "form-control";
                    txtCant.Style.Add("text-align", "Right");
                    //txtCant.TextMode = TextBoxMode.Number;
                    txtCant.Text = celCantidad.Text;
                    celCantidadEntregar.Controls.Add(txtCant);
                    celCantidadEntregar.Width = Unit.Percentage(20);
                    tr.Cells.Add(celCantidadEntregar);


                    //phArticulos.Controls.Add(tr);


                }
            }
            catch (Exception ex)
            {

            }
            
        }
        private void editarCantidades(object sender, EventArgs e)
        {
            try
            {
                //phArticulos.Controls.Clear();
                LinkButton lb = sender as LinkButton;
                string[] fd = lb.ID.Split(',');
                string fd2 = lb.ID.Remove(0, lb.ID.IndexOf(','));
                ViewState["idArticulo"] = fd[0];
                ViewState["Pedidos"] = fd2;
                var pedidosxArticulo = controladorPedido.ObtenerPedidosPorArticulo(fd2, Convert.ToInt32(fd[0]));
                var articulo = controladorArticulo.obtenerArticuloByID(Convert.ToInt32(fd[0]));
                SetearColumnasArticulos(pedidosxArticulo, fd[0]);
                //lblCodigoArticulo.Text = articulo.codigo;
                
                //lblDescripcionArticulo.Text = articulo.descripcion;


            }
            catch (Exception Ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al editar tipo evento. Excepción: " + Ex.Message));
            }
        }

        public void CargarPedidos(string pedidos)
        {
            try
            {
                string pedidos2 = pedidos.Replace(";", ",");
                var listaArticulos = controladorPedido.obtenerConsolidado(pedidos2);
                

                SetearColumnasPedidos(listaArticulos);
                if (ViewState["idArticulo"] != null && ViewState["Pedidos"] != null)
                {
                    string listaPedidosxArticulo = ViewState["Pedidos"].ToString();
                    string idArticulo = ViewState["idArticulo"].ToString();
                    var pedidosxArticulo = controladorPedido.ObtenerPedidosPorArticulo(listaPedidosxArticulo, Convert.ToInt32(idArticulo));
                    //phArticulos.Controls.Clear();
                    SetearColumnasArticulos(pedidosxArticulo,idArticulo);
                }
            }
            catch (Exception ex)
            {

                
            }
       
        }

        private void ObtenerItemsPedidoDeHijos(string pedidos, Pedido p)
        {
            try
            {
                ControladorPedido cp = new ControladorPedido();
                string[] listPedido = pedidos.Split(';');
                controladorCliente cc = new controladorCliente();
                controladorArticulo ca = new controladorArticulo();

                Cliente c = cc.obtenerClienteID(idVendedor);
                decimal total = 0;

                foreach (string i in listPedido)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        var articulosP = cp.obtenerItemsPedido(Convert.ToInt32(i));
                        foreach (ItemPedido ip in articulosP)
                        {
                            Articulo articulo = ca.obtenerArticuloFacturar(ip.articulo.codigo,c.lisPrecio.id);
                            ip.articulo = articulo;
                            ip.precioUnitario = articulo.precioVenta;
                            ip.total = ip.cantidad * articulo.precioVenta;
                            total += ip.total;
                            
                            p.agregarItem(ip);
                        }
                        p.total = total;

                    }
                }
                
            }
            catch (Exception ex)
            {

                throw;
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

        #endregion

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ItemPedido itemPedido;
        //        int idPedido;
        //        int idArticulo;

        //        foreach (Control C in phArticulos.Controls)
        //        {
        //            itemPedido = new ItemPedido();

        //            TableRow tr = C as TableRow;
        //            string[] Ids = tr.ID.Split(',');
        //            idPedido = Convert.ToInt32(Ids[0]);
        //            idArticulo = Convert.ToInt32(Ids[1]);
        //            TextBox txt = tr.Cells[2].Controls[0] as TextBox;
        //            itemPedido.articulo.id = idArticulo;
        //            itemPedido.cantidad = Convert.ToDecimal(txt.Text);
        //            controladorPedido.modificarCantidades(itemPedido, idPedido);
        //            controladorPedido.actualizarTotales(idPedido);
                    

        //        }
        //        phPedidos.Controls.Clear();
        //        CargarPedidos(Request.QueryString["pedidos"]);
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "CantidadesEditadas()", true);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
            

        //}

        protected void btnGuardarP_Click(object sender, EventArgs e)
        {
            try
            {
                CargarDatosPedido();
                generarPedido();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

