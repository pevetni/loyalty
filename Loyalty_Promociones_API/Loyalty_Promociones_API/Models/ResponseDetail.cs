using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loyalty_Promociones_API.Models
{
    public class ResponseDetail
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}
