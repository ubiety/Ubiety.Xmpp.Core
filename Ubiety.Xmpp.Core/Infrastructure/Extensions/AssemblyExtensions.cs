using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    ///     Assembly class extensions
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        ///    Gets all attributes of a specific type from the assembly
        /// </summary>
        /// <typeparam name="T">Type of the attribute to retrieve</typeparam>
        /// <param name="assembly">Assembly to iterate</param>
        /// <returns>Enumerable of attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this Assembly assembly) where T : Attribute
        {
            var attributes = new List<T>();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                type.GetCustomAttributes<T>(true).Apply(attributes.Add);
            }

            return attributes;
        }
    }
}
