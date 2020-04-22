using System;
using System.Linq;

namespace Loyalty_Promociones_API.Models
{
    public static class Tools
    {
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }
    }
}
