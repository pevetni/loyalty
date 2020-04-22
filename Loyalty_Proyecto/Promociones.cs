using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGM_LOYALTY
{
    public class Promociones
    {
        public string Catalog { get; set; }
        public string CompanyId { get; set; }
        public string CardId { get; set; }
        public string CardTye { get; set; }
        public string Contract { get; set; }
        public string CustomerId { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public double Amount { get; set; }
    }
}