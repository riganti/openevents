using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenEvents.Backend.Common
{
    public static class ReflectionExtensions
    {

        public static IEnumerable<Type> FindAllImplementations<T>(this Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(typeof(T).IsAssignableFrom);
        }

        public static IEnumerable<(Type Interface, Type Implementation)> FindAllOpenGenericImplementations(this Assembly assembly, Type type)
        {
            if (!type.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"The type {type} is not an open generic type!", nameof(type));
            }

            return assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => 
                    t.GetBaseTypes()
                        .Concat(t.GetInterfaces())
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                        .Select(i => (i, t))
                );
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            while (type.BaseType != null)
            {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }
    }
}
