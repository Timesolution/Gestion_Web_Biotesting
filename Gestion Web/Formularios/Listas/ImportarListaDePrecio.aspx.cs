using Disipar.Models;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestion_Web.Formularios.Articulos
{
    public partial class ImportarListaDePrecio : System.Web.UI.Page
    {
        controladorArticulo contArticulo = new controladorArticulo();
        ControladorArticulosEntity contArticulosEntity = new ControladorArticulosEntity();
        controladorListaPrecio contListaPrecio = new controladorListaPrecio();
        Mensajes m = new Mensajes();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnImportarListaDePrecio_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;

                if (FileUpload1.HasFile)
                {
                    String fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();

                    String[] allowedExtensions = { ".csv" };

                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }
                }
                if (fileOK)
                {
                    StreamReader sr = new StreamReader(FileUpload1.FileContent);
                    Configuracion config = new Configuracion();
                    string linea;
                    int contador = 0;
                    sr.ReadLine();//para saltar la primer linea

                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] datos = linea.Split(';');//obtengo datos del registro

                        if (datos.Count() >= 4)
                        {
                            List<string> datosExcel = datos.ToList();
                            int respuesta = this.AgregarArticuloToListaDePrecio(datosExcel);
                            if (respuesta < 0)
                            {
                                contador++;
                            }
                        }
                    }
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxInfo("Lista importada correctamente", null));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public int AgregarArticuloToListaDePrecio(List<string> datosExcel)
        {
            try
            {
                Articulo articulo = contArticulo.obtenerArticuloCodigo(datosExcel[0]);
                if (articulo != null)
                {
                    if (AgregarCodigoArticulo_SiNoExiste_ToTableListaPreciosCategoria(articulo.codigo))
                    {
                        if (EliminarLosRegistrosDeLaTablaSubListasPreciosByCategoria(articulo.codigo))
                        {
                            AsignarElIdDeLaTablaListaPreciosCategorias_Al_CampoSubListaDelArticulo(articulo);

                            AgregarArticuloToTableSubListasPrecios(articulo, datosExcel);
                        }
                    }
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion(this.ToString() + this.GetType() + ". Ex: " + ex.Message));
                return -1;
            }
        }

        public bool AgregarCodigoArticulo_SiNoExiste_ToTableListaPreciosCategoria(string CodigoArticulo)
        {
            try
            {
                var categoria = contListaPrecio.obtenerCategoriaByCategoria(CodigoArticulo);
                if (categoria == null)
                {
                    contListaPrecio.agregarCategoria(new ListaCategoria
                    {
                        categoria = CodigoArticulo,
                        estado = 1
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error en fun: " + this.ToString() + " Ex: " + ex.Message));
                return false;
            }
        }

        public void AsignarElIdDeLaTablaListaPreciosCategorias_Al_CampoSubListaDelArticulo(Articulo articulo)
        {
            try
            {
                var categoria = contListaPrecio.obtenerCategoriaByCategoria(articulo.codigo);
                articulo.listaCategoria.id = categoria.id;
                contArticulo.modificarArticulo(articulo, articulo.codigo, 0);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", m.mensajeBoxAtencion("Error en fun: " + this.ToString() + " Ex: " + ex.Message));
            }
        }

        public void AgregarArticuloToTableSubListasPrecios(Articulo articulo, List<string> datosExcel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(articulo.codigo))
                {
                    return;
                }
                for (int numeroDeLista = 1; numeroDeLista <= 3; numeroDeLista++)
                {
                    decimal precioNuevo = 0;
                    int aumentoDescuento = 0;
                    if (decimal.TryParse(datosExcel[numeroDeLista].Replace(",", ""), out precioNuevo))
                    {
                        decimal porcentaje = (1 - (precioNuevo / articulo.precioVenta)) * 100;

                        if (porcentaje < 0)//es aumento
                        {
                            aumentoDescuento = 1;
                            porcentaje = porcentaje * -1;
                        }
                        else//es descuento
                        {
                            aumentoDescuento = 2;
                        }

                        var Categoria = contListaPrecio.obtenerCategoriaByCategoria(articulo.codigo);
                        contListaPrecio.agregarSubListaPrecio(new SubListaPrecio
                        {
                            categoria = Categoria,
                            AumentoDescuento = aumentoDescuento,
                            CostoVenta = 2,
                            porcentaje = porcentaje,
                            estado = 1
                        }, numeroDeLista);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool EliminarLosRegistrosDeLaTablaSubListasPreciosByCategoria(string categoria)
        {
            try
            {
                var categoriaDB = contListaPrecio.obtenerCategoriaByCategoria(categoria);
                bool resultado = contListaPrecio.eliminarCategoriasByCategoria(Convert.ToInt32(categoriaDB.id));
                if (resultado)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}