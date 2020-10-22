using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Disipar.Models
{
    public class Mensajes
    {

        public string mensajeBoxAtencion(string mensaje)
        {
            string message = "Order Placed Successfully.";
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgbox(\""+ mensaje +"\");");
            sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        public string mensajeDenegado()
        {
            string message = "Order Placed Successfully.";
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("document.getElementById('abreDenegado').click();");
            sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public string mensajeBoxInfo(string mensaje, string UrlRedirect)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgbox(\"" + mensaje + "\", {type: \"info\"});");
            if (!String.IsNullOrEmpty(UrlRedirect))
            {
                sb.Append("location.href = '" + UrlRedirect + "';");
            }
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        public string mensajeBoxError(string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgbox(\"<h2>Ups!</h2></br>Disculpe, ha ocurrido un error inesperado. </br>Por favor, haga click en <strong><a href='../Formularios/Herramientas/Soporte.aspx' target='_blank'>Como Generar un Ticket</a></strong> y siga con los pasos para ayudarnos a agilizar en la deteccion del incoveniente.</br>Gracias!\", {type: \"error\"});");
            sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        #region Mensaje Growls


        /// <summary>
        /// Mensaje Growl de tipo Advertencia
        /// </summary>
        /// <param name="encabezado"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public string mensajeGrowlInfo(string encabezado, string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgGrowl ({ type: 'info', title: '" + encabezado + "',  text: '" + mensaje + "' });");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        /// <summary>
        /// Mensaje Growl de tipo Exito
        /// </summary>
        /// <param name="encabezado"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public string mensajeGrowlSucces(string encabezado, string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgGrowl ({ type: 'success', title: '" + encabezado + "',  text: '" + mensaje + "' });");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");
            return sb.ToString();
        }

        /// <summary>
        /// Mensaje Growl de tipo Error
        /// </summary>
        /// <param name="encabezado"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public string mensajeGrowlError(string encabezado, string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgGrowl ({ type: 'error', title: '" + encabezado + "',  text: '" + mensaje + "' });");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        /// <summary>
        /// Mensaje Growl de tipo Advertencia
        /// </summary>
        /// <param name="encabezado"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public string mensajeGrowlWarning(string encabezado, string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgGrowl ({ type: 'warning', title: '" + encabezado + "',  text: '" + mensaje + "' });");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        #endregion

        public string foco(string idElemento)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append(" document.getElementById('" + idElemento + "').focus();");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append(" }");
            sb.Append("</script>");

            return sb.ToString();

        }

        public string volverAtras()
        {



            StringBuilder sb = new StringBuilder();

            sb.Append("<script type = 'text/javascript'>");

            sb.Append("window.onload=function(){");

            sb.Append("window.history.back();");

            sb.Append("window.history.back();");

            sb.Append(";");

            //sb.Append(message);

            sb.Append("};");

            sb.Append("</script>");
            

            return sb.ToString();
            

        }

    }
}