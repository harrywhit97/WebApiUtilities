using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Extenstions
{
    public static class ExtensionHelpers
    {
        public static IEnumerable<Type> GetEntities(Assembly assembly)
        {
            return ExtractTypesFromAssembly(assembly, typeof(IEntity<>));
        }

        public static IEnumerable<Type> ExtractTypesFromAssembly(Assembly assembly, Type genericInterface)
        {
            return assembly.GetExportedTypes()
                            .Where(t => t.GetInterfaces().Any(i =>
                                i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface))
                            .ToList();
        }
    }
}
