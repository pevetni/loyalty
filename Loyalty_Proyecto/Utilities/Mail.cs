using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SGM_LOYALTY.Utilities
{
    public static class Mail
    {
        public static string CrearCuerpoMailValidacion(string hashValidacion, string cliente, string urlValidacion)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("/PlantillasMail/MailActivacion.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Nombre}", cliente);
            body = body.Replace("{URL}", urlValidacion + hashValidacion);

            return body;
        }
        public static string CrearCuerpoMailValidacionRealizada(string cliente, string genero, string tarjeta)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("/PlantillasMail/MailConfirmacionAlta.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Nombre}", cliente);
            body = body.Replace("{Bienvenido}", genero == "M" ? "Bienvenido" : "Bienvenida");
            body = body.Replace("{Tarjeta}", tarjeta);

            return body;
        }
    }
}