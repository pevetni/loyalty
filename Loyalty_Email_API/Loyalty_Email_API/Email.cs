using System;

namespace Loyalty_Email_API
{
    public class Email
    {
        public string NombreRemitente { get; set; }
        public string AsuntoMail { get; set; }
        public string CuerpoMail { get; set; }
        public string Destinatario { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
