using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vroom.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyValue<T>(this T Item, string PropertyName)
        {
            return Item.GetType().GetProperty(PropertyName).GetValue(Item, null).ToString();
        }
    }
}
