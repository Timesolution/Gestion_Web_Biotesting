using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Modelo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Gestion_Web.Formularios.Articulos
{   
    public partial class ImpresionListaPrecios : System.Web.UI.Page
    {
        private controladorArticulo controlador = new controladorArticulo();
        ControladorArticulosEntity contArtEntity = new ControladorArticulosEntity();

        Mensajes m = new Mensajes();
        private int accion;
        private int lista;
        private int precioSinIva;
        private int grupo;
        private int subgrupo;
        private int dias;
        private int desactualizados;
        private int proveedor;
        private int sinIva;
        private string textoBuscar;
        private int excel;
        private int marca;
        private int descuentoPorCantidad;
        string descSubGrupo;
        private int valor;

        private DataTable dtArticulos = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //ListaPrecio y Categoria
                    this.lista = Convert.ToInt32(Request.QueryString["l"]);
                    this.precioSinIva = Convert.ToInt32(Request.QueryString["psi"]);
                    this.grupo = Convert.ToInt32(Request.QueryString["g"]);
                    this.subgrupo = Convert.ToInt32(Request.QueryString["sg"]);                    
                    this.dias = Convert.ToInt32(Request.QueryString["d"]);
                    this.desactualizados = Convert.ToInt32(Request.QueryString["desact"]);
                    this.proveedor = Convert.ToInt32(Request.QueryString["p"]);
                    this.accion = Convert.ToInt32(Request.QueryString["a"]);
                    this.valor = Convert.ToInt32(Request.QueryString["v"]);
                    this.excel = Convert.ToInt32(Request.QueryString["ex"]);
                    this.sinIva = Convert.ToInt32(Request.QueryString["iva"]);
                    this.textoBuscar = Request.QueryString["t"];
                    this.marca = Convert.ToInt32(Request.QueryString["m"]);
                    this.descSubGrupo = Request.QueryString["dsg"];
                    this.descuentoPorCantidad = Convert.ToInt32(Request.QueryString["dpc"]);

                    //if (this.valor == 0 && descuentoPorCantidad == 0)
                    //{
                    //    generarReporte();
                    //}
                    //if (this.valor == 1 && descuentoPorCantidad == 0)
                    //{
                    //    generarReporte2();
                    //}
                    //if (this.valor == 0 && descuentoPorCantidad == 1)
                    //{
                    //    generarReporte3();
                    //}
                    //if (this.valor == 1 && descuentoPorCantidad == 1)
                    //{
                    //    generarReporte4();
                    //}
                    if(descuentoPorCantidad == 1)
                        GenerarListaPreciosDescuentoPorCantidad();
                    else
                        GenerarListaPrecios();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error al mostrar Lista de precios. " + ex.Message));
                Log.EscribirSQL(1, "ERROR", "Error al mostrar Lista de precios. " + ex.Message);
            }
        }

        private void cargarTableArticulos(int grupo, int subgrupo, int proveedor, int dias)
        {
            try
            {
                List<Articulo> listArticulos = new List<Articulo>();
                List<Gestion_Api.Entitys.Articulos_Catalogo> listArticulosCatalogo = new List<Gestion_Api.Entitys.Articulos_Catalogo>();
                string descuentoCantidad = "0";
                Decimal descuento = 0.00m;
                Decimal descuento2 = 0.00m;
                Decimal descuento3 = 0.00m;

                if (accion == 1)//por filtro
                {
                    if (dias == 0)
                    {
                        listArticulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor,"", this.marca, this.descSubGrupo);
                    }
                    else
                    {                        
                        listArticulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor, DateTime.Now.AddDays(-dias).ToString("yyyyMMdd"), this.marca, this.descSubGrupo);
                    }
                    
                }
                if (accion == 2)//default top 20
                    listArticulos = this.controlador.obtenerArticulosReduc();

                if (accion == 3)//x busqueda
                    listArticulos = this.controlador.buscarArticuloList(this.textoBuscar);

                if (accion == 4)//x fecha act/desact
                {
                    if (this.desactualizados > 0)
                    {
                        DateTime fecha = DateTime.Today.AddDays(this.dias * -1);
                        listArticulos = this.controlador.obtenerArticuloDesactualizadosByFecha(fecha);
                    }
                    else
                    {
                        DateTime fecha = DateTime.Today.AddDays(this.dias * -1);
                        listArticulos = this.controlador.obtenerArticuloByFechaActualizacion(fecha);
                    }
                }

                //Me traigo los registros de la tabla Articulos_Catalogo en caso de que se haya seleccionado Agrupar x Ubicación.
                listArticulosCatalogo = this.contArtEntity.obtenerArticulos_Catalogo();

                CrearDataTableArticulos(dtArticulos);

                foreach (Articulo articulo in listArticulos)
                {
                    //Creo una variable flag. Esta variable va a servir para ver si el articulo no tiene que aparecer en la impresion de la lista de precios
                    int apareceListaPrecio = 1;

                    descuentoCantidad = "0";
                    descuento = 0.00m;
                    descuento2 = 0.00m;
                    descuento3 = 0.00m;

                    DataRow drDatos = dtArticulos.NewRow();
                    Gestion_Api.Entitys.articulo artEntity = this.contArtEntity.obtenerArticuloEntity(articulo.id);

                    if (this.sinIva == 1)//Opcion precio lista sin iva == 1 | Opcion precio lista con iva == 2
                    {
                        //remplazo el precio de venta con el precio sin iva, para que calcule el precio de lista sobre el precioSinIva.
                        Decimal sinIva = articulo.precioSinIva;
                        articulo.precioVenta = sinIva;
                    }
                    
                    articulo.precioVenta = this.controlador.obtenerPrecioLista2(articulo, this.lista,this.sinIva);                    

                    drDatos[0] = articulo.codigo;
                    drDatos[1] = articulo.descripcion;
                    drDatos[2] = articulo.grupo.descripcion;
                    drDatos[3] = articulo.subGrupo.descripcion;
                    drDatos[4] = articulo.monedaVenta.moneda;
                    drDatos[5] = Decimal.Round(articulo.precioVenta,2);
                    drDatos[7] = articulo.proveedor.razonSocial;

                    //Genero objeto artMed en donde voy a tener la ubicación del catalogo
                    Gestion_Api.Entitys.Articulos_Catalogo artMed = new Gestion_Api.Entitys.Articulos_Catalogo();

                    if (artEntity != null)
                    {
                        if (artEntity.Articulos_Presentaciones.Count() > 0)
                        {
                            drDatos["Min"] = artEntity.Articulos_Presentaciones.FirstOrDefault().Minima;
                            drDatos["Med"] = artEntity.Articulos_Presentaciones.FirstOrDefault().Media;
                            drDatos["Max"] = artEntity.Articulos_Presentaciones.FirstOrDefault().Maxima;
                        }
                        if (artEntity.Articulos_Descuentos.Count > 0)
                        {
                            drDatos["CantidadDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Desde; //Desde
                            drDatos["CantidadHastaDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Hasta;
                            drDatos["PocentajeDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Descuento;
                            descuento = (decimal)(articulo.precioVenta * (1 - (artEntity.Articulos_Descuentos.FirstOrDefault().Descuento / 100)));
                            descuento = decimal.Round(descuento, 2);
                           

                            //descuentoCantidad = "Desde " + drDatos["CantidadDto1"] + " Hasta " + drDatos["CantidadHastaDto1"] + " " +drDatos["PocentajeDto1"] + "%  - $"
                            //    + descuento.ToString();

                            if (artEntity.Articulos_Descuentos.Count > 1)
                            {
                                drDatos["CantidadDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Desde;
                                drDatos["CantidadHastaDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Hasta;
                                drDatos["PocentajeDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Descuento;
                                descuento2 = (decimal)(articulo.precioVenta * (1-(artEntity.Articulos_Descuentos.ElementAt(1).Descuento / 100)));
                                descuento2 = decimal.Round(descuento2, 2);


                                //descuentoCantidad += Environment.NewLine +  "Desde " + drDatos["CantidadDto2"] + " Hasta " + drDatos["CantidadHastaDto2"] + " " + drDatos["PocentajeDto2"] + "%  - $"
                                //+ descuento2.ToString();

                                if (artEntity.Articulos_Descuentos.Count > 2)
                                {
                                    drDatos["CantidadDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Desde;
                                    drDatos["CantidadHastaDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Hasta;
                                    drDatos["PocentajeDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Descuento;
                                    descuento3 = (decimal)(articulo.precioVenta * (1 - (artEntity.Articulos_Descuentos.ElementAt(2).Descuento / 100)));
                                    descuento3 = decimal.Round(descuento3, 2);


                                    //descuentoCantidad += Environment.NewLine +  "Desde " + drDatos["CantidadDto2"] + " Hasta " + drDatos["CantidadHastaDto2"] + " " + drDatos["PocentajeDto2"] + "%  - $"
                                    //+ descuento2.ToString();
                                }
                            }
                        }

                        drDatos["DescuentoPorCantidad"] = descuentoCantidad;
                        drDatos["Descuento1"] = descuento;
                        drDatos["Descuento2"] = descuento2;
                        drDatos["Descuento3"] = descuento3;

                        var marca = artEntity.Articulos_Marca.FirstOrDefault();
                        if (marca != null)
                        {
                            drDatos["Marca"] = marca.Marca.marca1;
                        }
                        else
                        {
                            drDatos["Marca"] = "SIN MARCA";
                        }

                        #region Catalogo y Aparece en Lista de Precios
                        //Verifico que la lista de Articulos_Catalogo no sea null y tenga registros. Obtengo el campo Catalogo y lo seteo a la columna Observaciones.
                        //Utilizo la misma tabla para incluir o no el articulo en la impresion de lista de precios.
                        if (listArticulosCatalogo != null && listArticulosCatalogo.Count > 0)
                        {
                            artMed = listArticulosCatalogo.Where(x => x.Articulo == articulo.id).FirstOrDefault();
                            if (artMed != null)
                            {
                                try
                                {
                                    //Verifico si el campo catalogo no es nulo o no está vacio para setear el valor del catalogo
                                    if (!string.IsNullOrEmpty(artMed.Catalogo))
                                        drDatos["Observaciones"] = artMed.Catalogo;

                                    //Verifico si el campo aparece lista no es nulo y que tenga valor 0. Si tiene valor 0 quiere decir que no quieren que el articulo aparezca en la impresion de lista de precios
                                    if (artMed.ApareceLista != null && artMed.ApareceLista == 0)
                                        apareceListaPrecio = 0;
                                }
                                catch { }
                            }

                        }
                        #endregion

                    }

                    if ((this.valor != 1) || (this.valor == 1 && artMed != null && artMed.Catalogo != ""))
                    {
                        if (articulo.apareceLista == 1)
                        {
                            //Por defecto, esta variable entero viene en 1. Sólo en caso de que se haya seteado que el articulo no aparezca en la impresion de listas de precios (Tabla Articulos_Catalogo)
                            //el articulo será omitido para la impresión.
                            if (apareceListaPrecio == 1)
                                this.dtArticulos.Rows.Add(drDatos);
                        }
                    }
                }

                DataView dv = dtArticulos.DefaultView;
                dv.Sort = "descripcion";
                this.dtArticulos = dv.ToTable();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void CargarTablaArticulos(int grupo, int subgrupo, int proveedor, int dias)
        {
            try
            {
                List<Articulo> listArticulos = new List<Articulo>();
                List<Gestion_Api.Entitys.Articulos_Catalogo> listArticulosCatalogo = new List<Gestion_Api.Entitys.Articulos_Catalogo>();
                
                if (accion == 1)//por filtro
                {
                    if (dias == 0)
                    {
                        listArticulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor, "", this.marca, this.descSubGrupo);
                    }
                    else
                    {
                        listArticulos = this.controlador.filtrarArticulosGrupoSubGrupo(grupo, subgrupo, proveedor, DateTime.Now.AddDays(-dias).ToString("yyyyMMdd"), this.marca, this.descSubGrupo);
                    }
                }
                if (accion == 2)//default top 20
                    listArticulos = this.controlador.obtenerArticulosReduc();

                if (accion == 3)//x busqueda
                    listArticulos = this.controlador.buscarArticuloList(this.textoBuscar);

                if (accion == 4)//x fecha act/desact
                {
                    if (this.desactualizados > 0)
                    {
                        DateTime fecha = DateTime.Today.AddDays(this.dias * -1);
                        listArticulos = this.controlador.obtenerArticuloDesactualizadosByFecha(fecha);
                    }
                    else
                    {
                        DateTime fecha = DateTime.Today.AddDays(this.dias * -1);
                        listArticulos = this.controlador.obtenerArticuloByFechaActualizacion(fecha);
                    }
                }

                //Me traigo los registros de la tabla Articulos_Catalogo en caso de que se haya seleccionado Agrupar x Ubicación.
                listArticulosCatalogo = this.contArtEntity.obtenerArticulos_Catalogo();

                CrearDataTableArticulos(dtArticulos);

                foreach (Articulo articulo in listArticulos)
                {
                    //Creo una variable flag. Esta variable va a servir para ver si el articulo no tiene que aparecer en la impresion de la lista de precios
                    int apareceListaPrecio = 1;

                    DataRow drDatos = dtArticulos.NewRow();
                    Gestion_Api.Entitys.articulo artEntity = this.contArtEntity.obtenerArticuloEntity(articulo.id);

                    if (this.sinIva == 1)//Opcion precio lista sin iva == 1 | Opcion precio lista con iva == 2
                    {
                        //remplazo el precio de venta con el precio sin iva, para que calcule el precio de lista sobre el precioSinIva.
                        Decimal sinIva = articulo.precioSinIva;
                        articulo.precioVenta = sinIva;
                    }

                    articulo.precioVenta = this.controlador.obtenerPrecioLista2(articulo, this.lista, this.sinIva);

                    drDatos[0] = articulo.codigo;
                    drDatos[1] = articulo.descripcion;
                    drDatos[2] = articulo.grupo.descripcion;
                    drDatos[3] = articulo.subGrupo.descripcion;
                    drDatos[4] = articulo.monedaVenta.moneda;
                    drDatos[5] = Decimal.Round(articulo.precioVenta, 2);
                    drDatos[7] = articulo.proveedor.razonSocial;

                    //Genero objeto artMed en donde voy a tener la ubicación del catalogo
                    Gestion_Api.Entitys.Articulos_Catalogo artMed = new Gestion_Api.Entitys.Articulos_Catalogo();

                    if (artEntity != null)
                    {
                        SetearDescuentoPorCantidadALATabla(drDatos, articulo, artEntity);
                        
                        var marca = artEntity.Articulos_Marca.FirstOrDefault();
                        if (marca != null)
                        {
                            drDatos["Marca"] = marca.Marca.marca1;
                        }
                        else
                        {
                            drDatos["Marca"] = "SIN MARCA";
                        }

                        #region Catalogo y Aparece en Lista de Precios
                        //Verifico que la lista de Articulos_Catalogo no sea null y tenga registros. Obtengo el campo Catalogo y lo seteo a la columna Observaciones.
                        //Utilizo la misma tabla para incluir o no el articulo en la impresion de lista de precios.
                        if (listArticulosCatalogo != null && listArticulosCatalogo.Count > 0)
                        {
                            artMed = listArticulosCatalogo.Where(x => x.Articulo == articulo.id).FirstOrDefault();
                            if (artMed != null)
                            {
                                try
                                {
                                    //Verifico si el campo catalogo no es nulo o no está vacio para setear el valor del catalogo
                                    if (!string.IsNullOrEmpty(artMed.Catalogo))
                                        drDatos["Observaciones"] = artMed.Catalogo;

                                    //Verifico si el campo aparece lista no es nulo y que tenga valor 0. Si tiene valor 0 quiere decir que no quieren que el articulo aparezca en la impresion de lista de precios
                                    if (artMed.ApareceLista != null && artMed.ApareceLista == 0)
                                        apareceListaPrecio = 0;
                                }
                                catch { }
                            }
                        }
                        #endregion
                    }

                    if ((this.valor != 1) || (this.valor == 1 && artMed != null && artMed.Catalogo != ""))
                    {
                        if (articulo.apareceLista == 1)
                        {
                            //Por defecto, esta variable entero viene en 1. Sólo en caso de que se haya seteado que el articulo no aparezca en la impresion de listas de precios (Tabla Articulos_Catalogo)
                            //el articulo será omitido para la impresión.
                            if (apareceListaPrecio == 1)
                                this.dtArticulos.Rows.Add(drDatos);
                        }
                    }
                }
                DataView dv = dtArticulos.DefaultView;
                dv.Sort = "descripcion";
                this.dtArticulos = dv.ToTable();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxError("Error buscando articulo. " + ex.Message));
            }
        }

        private void SetearDescuentoPorCantidadALATabla(DataRow drDatos,Articulo articulo, Gestion_Api.Entitys.articulo artEntity)
        {
            try
            {
                string descuentoCantidad = "0";
                Decimal descuento = 0.00m;
                Decimal descuento2 = 0.00m;
                Decimal descuento3 = 0.00m;

                if (artEntity.Articulos_Descuentos.Count > 0 && descuentoPorCantidad == 1)
                {
                    drDatos["CantidadDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Desde; //Desde
                    drDatos["CantidadHastaDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Hasta;
                    drDatos["PocentajeDto1"] = artEntity.Articulos_Descuentos.FirstOrDefault().Descuento;
                    descuento = (decimal)(articulo.precioVenta * (1 - (artEntity.Articulos_Descuentos.FirstOrDefault().Descuento / 100)));
                    descuento = decimal.Round(descuento, 2);

                    if (artEntity.Articulos_Descuentos.Count > 1)
                    {
                        drDatos["CantidadDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Desde;
                        drDatos["CantidadHastaDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Hasta;
                        drDatos["PocentajeDto2"] = artEntity.Articulos_Descuentos.ElementAt(1).Descuento;
                        descuento2 = (decimal)(articulo.precioVenta * (1 - (artEntity.Articulos_Descuentos.ElementAt(1).Descuento / 100)));
                        descuento2 = decimal.Round(descuento2, 2);

                        if (artEntity.Articulos_Descuentos.Count > 2)
                        {
                            drDatos["CantidadDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Desde;
                            drDatos["CantidadHastaDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Hasta;
                            drDatos["PocentajeDto3"] = artEntity.Articulos_Descuentos.ElementAt(2).Descuento;
                            descuento3 = (decimal)(articulo.precioVenta * (1 - (artEntity.Articulos_Descuentos.ElementAt(2).Descuento / 100)));
                            descuento3 = decimal.Round(descuento3, 2);
                        }
                    }
                }
                drDatos["DescuentoPorCantidad"] = descuentoCantidad;
                drDatos["Descuento1"] = descuento;
                drDatos["Descuento2"] = descuento2;
                drDatos["Descuento3"] = descuento3;
            }
            catch (Exception ex)
            {

            }
        }

        private void CrearDataTableArticulos(DataTable dtArticulos)
        {
            dtArticulos.Columns.Add("codigo");
            dtArticulos.Columns.Add("descripcion");
            dtArticulos.Columns.Add("grupo");
            dtArticulos.Columns.Add("subgrupo");
            dtArticulos.Columns.Add("moneda");
            dtArticulos.Columns.Add("precioVta", typeof(decimal));
            dtArticulos.Columns.Add("Ubicacion");
            dtArticulos.Columns.Add("Proveedor");
            dtArticulos.Columns.Add("Min");
            dtArticulos.Columns.Add("Med");
            dtArticulos.Columns.Add("Max");
            dtArticulos.Columns.Add("CantidadDto1"); //Desde
            dtArticulos.Columns.Add("CantidadHastaDto1");
            dtArticulos.Columns.Add("PocentajeDto1");
            dtArticulos.Columns.Add("CantidadDto2"); //Desde
            dtArticulos.Columns.Add("CantidadDto3");
            dtArticulos.Columns.Add("CantidadHastaDto2");
            dtArticulos.Columns.Add("CantidadHastaDto3");
            dtArticulos.Columns.Add("PocentajeDto2");
            dtArticulos.Columns.Add("PocentajeDto3");
            dtArticulos.Columns.Add("Marca");
            dtArticulos.Columns.Add("DescuentoPorCantidad");
            dtArticulos.Columns.Add("Descuento1");
            dtArticulos.Columns.Add("Descuento2");
            dtArticulos.Columns.Add("Descuento3");
            dtArticulos.Columns.Add("Observaciones");
        }

        private void GenerarListaPreciosDescuentoPorCantidad()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                //CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                var listaDePrecios = contList.ObtenerListaDePreciosDescuentoPorCantidad(this.lista, precioSinIva);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosDescuentoCantidadR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPreciosDescuentoCantidad", listaDePrecios);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando lista de precios con descuento por cantidad. " + ex.Message);
            }
        }

        private void GenerarListaPrecios()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                //CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                var listaDePrecios = contList.ObtenerListaDePrecios(this.lista,precioSinIva);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPrecios", listaDePrecios);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando lista de precios. " + ex.Message);
            }
        }

        private void generarReporte()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPrecios", this.dtArticulos);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);
                                
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_"+DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }

        private void generarReporte2()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                DataView dv = this.dtArticulos.DefaultView;
                dv.Sort = "Observaciones";
                this.dtArticulos = dv.ToTable();

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosUbicacionR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPrecios", this.dtArticulos);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }

        private void generarReporte3()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosDescuentoCantidadR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPreciosDescuentoCantidad", this.dtArticulos);

                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();

                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);

                    this.Response.End();
                }
                else
                {
                    //get pdf content

                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);

                    this.Response.End();
                }

            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }

        private void generarReporte4()
        {
            try
            {
                controladorListaPrecio contList = new controladorListaPrecio();
                listaPrecio lista = contList.obtenerlistaPrecioID(this.lista);
                String nombreLista = lista.nombre;

                CargarTablaArticulos(this.grupo, this.subgrupo, this.proveedor, this.dias);

                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("ListaPreciosUbicacionR.rdlc");

                ReportDataSource rds = new ReportDataSource("ListaPrecios", this.dtArticulos);
                ReportParameter param = new ReportParameter("ParamLista", nombreLista);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;

                string mimeType, encoding, fileNameExtension;

                string[] streams;

                if (this.excel == 1)
                {
                    //get xls content
                    Byte[] xlsContent = this.ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    String filename = string.Format("{0}.{1}", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy"), "xls");

                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    this.Response.BinaryWrite(xlsContent);
                    this.Response.End();
                }
                else
                {
                    //get pdf content
                    Byte[] pdfContent = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    this.Response.Clear();
                    this.Response.Buffer = true;
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("content-length", pdfContent.Length.ToString());
                    this.Response.BinaryWrite(pdfContent);
                    this.Response.End();
                }
            }
            catch (Exception ex)
            {
                Log.EscribirSQL((int)Session["Login_IdUser"], "ERROR", "Error generando reporte de Presupuesto. " + ex.Message);
            }
        }
    }
}