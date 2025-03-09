// Copyright 2018 Dieter Lunn
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    /// A static class containing extension methods for the Assembly type.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Retrieves all attributes of the specified type from the given assembly.
        /// </summary>
        /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
        /// <param name="assembly">The assembly to search for attributes.</param>
        /// <returns>An enumerable collection of attributes of the specified type found in the assembly.</returns>
        public static IEnumerable<T> GetAttributes<T>(this Assembly assembly)
            where T : Attribute
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
