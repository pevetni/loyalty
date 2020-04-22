using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SGM_LOYALTY.Utilities
{
    public class MailSender
    {
        public string ServidorSMTP { get; set; }
        public Int16 PuertoServidorSMTP { get; set; }
        public string RemitenteMailAltaUsuario { get; set; }
        public string PassRemitenteMailAltaUsuario { get; set; }
        public string NombreRemitenteMailAltaUsuario { get; set; }

        public void SendMailValidacion(string hashValidacion, string apellido, string nombre, string email)
        {
            SmtpClient smtpClient = new SmtpClient(ServidorSMTP, PuertoServidorSMTP);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(RemitenteMailAltaUsuario, PassRemitenteMailAltaUsuario);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.Body = CrearCuerpoMailValidacion(hashValidacion, nombre + " " + apellido);
            mail.Subject = "Bienvenido a Diarco+";
            mail.From = new MailAddress(RemitenteMailAltaUsuario, NombreRemitenteMailAltaUsuario);
            mail.To.Add(new MailAddress(email));

            smtpClient.Send(mail);
        }
        public string CrearCuerpoMailValidacion(string hashValidacion, string cliente)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("/PlantillasMail/MailActivacion.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("(Nombre)", cliente);
            body = body.Replace("(URL)", "wwww.diarco.com/activate.aspx?c=" + hashValidacion); 

            return body;
        }
    }

}