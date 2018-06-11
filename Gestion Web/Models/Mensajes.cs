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
            sb.Append("$.msgbox(\"" + mensaje + "\", {type: \"error\"});");
            sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

        public string mensajeGrowlSucces(string encabezado, string mensaje)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("$.msgGrowl ({ title: '" + encabezado + "',  text: '" + mensaje + "' });");
            //sb.Append(";");
            //sb.Append(message);
            sb.Append("};");
            sb.Append("</script>");

            return sb.ToString();

        }

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