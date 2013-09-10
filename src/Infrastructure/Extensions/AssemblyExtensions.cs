using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiMovieRest.Infrastructure.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> TypesOf<T>(this Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Where(type => typeof(T).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract);
        }
    }
}