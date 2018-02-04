using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEvents.Frontend.Common.Helpers
{
    public class EnumHelper
    {

        public static List<T> CreateCollection<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"The type {typeof(T)} is not enum!");
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

    }
}
