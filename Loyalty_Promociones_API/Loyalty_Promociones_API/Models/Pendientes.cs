
namespace Loyalty_Promociones_API.Models
{
    public class Pendientes
    {
        public Pendientes() { }
        public Pendientes(int _idAlta, string _tipoDocumentoCliente, string _documentoCliente, string _json)
        {
            idAlta = _idAlta;
            tipoDocumentoCliente = _tipoDocumentoCliente;
            documentoCliente = _documentoCliente;
            json = _json;
        }
        public int idAlta { get; set; }
        public string tipoDocumentoCliente { get; set; }
        public string documentoCliente { get; set; }
        public string json { get; set; }
    }
}
