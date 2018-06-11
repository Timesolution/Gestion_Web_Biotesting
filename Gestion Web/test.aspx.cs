using Gestion_Api.AccesoDatos;
using Gestion_Api.Controladores;
using Gestion_Api.Modelo;
using Gestor_Solution.Controladores;
using Gestor_Solution.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace Gestion_Web
{
    public partial class test : System.Web.UI.Page
    {
        //controladorCliente cont = new controladorCliente();
        controladorArticulo cont = new controladorArticulo();
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.obtenerClientesImportar();
            //int i = cont.agregarStockArticulos(null);
            //if (i > 0)
            //{
 
            //}
            //controladorSucursal cont = new controladorSucursal();
            //cont.obtenerTiposDocumento(2);
            //this.verificarStore();

            controladorCliente contCli = new controladorCliente();
            //contCli.actualizarPadron();

            //controladorFacturacion cont = new controladorFacturacion();

            //cont.obtenerCodigoBarraFactura("30677349065", "2", "64425220467017", "30102014");

        }

        //public void SerializeCollection()
        //{
        //    FacturaXML fact = new FacturaXML();
        //    fact.TipoFactura = "Factura A";
        //    fact.Comprador = "Time solution SA";
        //    fact.Cuit = "20000000001";
        //    fact.TipoIva = "Responsable Inscripto";
        //    fact.Direccion = "Maipu 821";

        //    FacturaItemXML item = new FacturaItemXML();
        //    item.Descripcion = "Desarrollo";
        //    item.Cantidad = 1;
        //    item.PrecioUnitario = (decimal)10.00;
        //    item.AlicuotaIva = "21.00";
        //    item.ImpuestosInternos = "3.00";

        //    fact.items.Add(item);

        //    FacturaItemXML item2 = new FacturaItemXML();
        //    item2.Descripcion = "Desarrollo2";
        //    item2.Cantidad = 2;
        //    item2.PrecioUnitario = (decimal)10.00;
        //    item2.AlicuotaIva = "21.00";
        //    item2.ImpuestosInternos = "3.00";

        //    fact.items.Add(item2);

        //    // Note that only the collection is serialized -- not the 
        //    // CollectionName or any other public property of the class.

        //    XmlSerializer x = new XmlSerializer(typeof(FacturaXML));
        //    TextWriter writer = new StreamWriter(@"D:\One\Time_Solution\Gestion\Gestion_Web\Factura.xml");
        //    x.Serialize(writer, fact);
        //}

        public void verificarStore()
        {
            try
            {
                controladorArticulo contArt = new controladorArticulo();
                var articulos = contArt.obtenerArticulosReduc2();
                foreach (var a in articulos)
                {
                    if (this.verificarImagen(a.codigo))
                    {
                        contArt.AgregarQuitarStore(a.id, 1);
                    }
                }
            }
            catch
            {

            }
        }

        public bool verificarImagen(string codigo)
        {
            try
            {
                String path = Server.MapPath("/images/Productos/" + codigo + "/");
                if (!Directory.Exists(path))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public void obtenerClientesImportar()
        //{
        //    try
        //    {
        //        AccesoDB db = new AccesoDB();

        //        SqlCommand command = new SqlCommand();
        //        //command.Connection = connection;
        //        //command.CommandText = "select * from clientes_Baires";
        //        command.CommandText = "select * from clientes_Baires";
        //        command.CommandType = CommandType.Text;


        //        //SqlDataReader dr = this.ac.ExecuteReader(command);
        //        DataTable dt = db.execDT(command);
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            this.importarCliente(dr);
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        //public void importarCliente(DataRow dr)
        //{
        //    try
        //    {

        //        Cliente cliente = new Cliente();

        //        //codigo
        //        string p = this.cont.obtenerLastCodigoCliente();
        //        int newp = Convert.ToInt32(p) + 1;
        //        cliente.codigo = newp.ToString().PadLeft(6, '0');

                

        //        cliente.razonSocial = dr[21].ToString();
        //        cliente.alias = dr[20].ToString();
                
        //        // ver de asignar sin grupo
        //        string grupo = dr[1].ToString();
        //        if (grupo == "Cliente directo")
        //        {
        //            cliente.grupo.id = 18;
        //        }
        //        if (grupo == "Revendedor")
        //        {
        //            cliente.grupo.id = 19;
        //        }
        //        if (grupo == "Contacto")
        //        {
        //            cliente.grupo.id = 20;
        //        }
                

        //        //como elimine la lista le pongo por defecto 0
        //        //cliente.categoria.id = Convert.ToInt32(this.DropListCategoria.SelectedValue);
        //        cliente.categoria.id = 1;
        //        cliente.estado.id = 1;

        //        string iva = dr[19].ToString();
        //        //verifico que no es persona
        //        if (dr[12].ToString() == "N")
        //        {
        //            //creae cliente directo
        //            cliente.tipoCliente.id = 3;

        //            //saco guiones al CUIT
        //            cliente.cuit = dr[4].ToString();
        //            //pongo iva

        //            bool paso = false;

        //            if (iva == "IVA EXENTO")
        //            {
        //                iva = "11";
        //                paso = true;
        //            }

        //            if (iva == "CONSUMIDOR FINAL")
        //            {
        //                iva = "2";
        //                paso = true;
        //            }

        //            if (iva == "RESPONSABLE INSCRIPTO")
        //            {
        //                iva = "9";
        //                paso = true;
        //            }

        //            if (iva == "MONOTRIBUTO")
        //            {
        //                iva = "10";
        //                paso = true;
        //            }

        //            if (string.IsNullOrEmpty(iva))
        //            {
        //                //le pongo responsable inscripto por defecto
        //                iva = "9";
        //                paso = true;
        //            }
        //            // si no asigne ninguno asigno por defecto resp inscripto
        //            if (paso == false)
        //            {
        //                iva = "9";
        //                paso = true;
        //            }

        //        }
        //        else
        //        {
        //            //creae cliente directo
        //            cliente.tipoCliente.id = 6;

        //            //personas le pongo cuit consumidor final
        //            cliente.cuit = "00000000000";
        //            //verifico los iva deberian ir todos en cons final
        //            //pongo iva

        //            bool paso = false;

        //            if (iva == "IVA EXENTO")
        //            {
        //                iva = "11";
        //                paso = true;
        //            }

        //            if (iva == "CONSUMIDOR FINAL")
        //            {
        //                iva = "2";
        //                paso = true;
        //            }

        //            if (iva == "RESPONSABLE INSCRIPTO")
        //            {
        //                iva = "9";
        //                paso = true;
        //            }

        //            if (iva == "MONOTRIBUTO")
        //            {
        //                iva = "10";
        //                paso = true;
        //            }

        //            if (string.IsNullOrEmpty(iva))
        //            {
        //                //le pongo Consumidor Final por defecto
        //                iva = "2";
        //                paso = true;
        //            }
        //            // si no asigne ninguno asigno por defecto resp inscripto
        //            if (paso == false)
        //            {
        //                iva = "2";
        //                paso = true;
        //            }

        //        }

        //        cliente.iva = iva;
        //        //asigno todos pais argentina
        //        cliente.pais.id = 1;
        //        cliente.expreso.id = 1;

        //        cliente.saldoMax = 0;
        //        cliente.vencFC = 0;

        //        cliente.descFC = 0;
        //        string userML = dr[17].ToString();
        //        if (!string.IsNullOrEmpty(userML))
        //        {
        //            cliente.observaciones = "Usuario Mercado Libre: " + userML;
        //        }
        //        cliente.hijoDe = 0;

        //        cliente.sucursal.id = 1;

        //        cliente.vendedor.id = 1;
        //        cliente.lisPrecio.id = 1;
        //        cliente.formaPago.id = 1;

        //        // agregar direccion
        //        direccion dir = new direccion();
        //        dir.nombre = "Legal";
        //        dir.direc = dr[7].ToString();
        //        dir.provincia = dr[15].ToString();
        //        dir.localidad = dr[10].ToString();
        //        dir.codPostal = "0001";
                
                
        //        cliente.direcciones.Add(dir);

        //        //agregar telefono
        //        contacto contacto = new contacto();
        //        contacto.cargo = "";
        //        contacto.mail = dr[13].ToString();
        //        contacto.nombreComp = dr[5].ToString() + " " + dr[6].ToString();
        //        contacto.numero = dr[22].ToString();
        //        contacto.tipoCont.id = 1;
                
        //        cliente.contactos.Add(contacto);

        //        cliente.origen = 1;
        //        int i = this.cont.agregarCliente(cliente);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }
}