using Newtonsoft.Json;

namespace Loyalty_Promociones_API.Models
{
    public class Response
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("detail")]
        public ResponseDetail Detail { get; set; }
    }
}
