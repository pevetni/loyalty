using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGM_LOYALTY
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