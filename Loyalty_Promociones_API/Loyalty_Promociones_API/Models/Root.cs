using System.Collections.Generic;

namespace Loyalty_Promociones_API.postPromociones
{
    public class Root
    {
        public string companyId { get; set; }
        public string catalog { get; set; }
        public List<Params> @params { get; set; }
        public List<Item> items { get; set; }
    }
}
