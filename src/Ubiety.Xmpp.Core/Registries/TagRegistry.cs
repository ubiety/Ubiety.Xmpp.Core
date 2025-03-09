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
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Registries
{
    /// <summary>
    /// Manages the registration and retrieval of XMPP tags, providing functionality
    /// to add assemblies containing tag definitions and retrieve tags by type, name, or namespace.
    /// </summary>
    public class TagRegistry
    {
        private static readonly ILog Logger = Log.Get<TagRegistry>();
        private readonly Dictionary<XName, Type> _types = new ();

        /// <summary>
        /// Adds all the tags from the specified assembly to the registry.
        /// </summary>
        /// <param name="assembly">The assembly to load tags from.</param>
        public void AddAssembly(Assembly assembly)
        {
            Logger.Log(LogLevel.Debug, "AddAssembly(Assembly) called");
            Logger.Log(LogLevel.Debug, $"Loading tags from assembly: {assembly.FullName}");

            var attributes = assembly.GetAttributes<XmppTagAttribute>();
            foreach (var attribute in attributes)
            {
                Logger.Log(LogLevel.Debug, $"Adding tag {attribute.Name} as {attribute.TagType}");
                _types.Add(attribute.Name, attribute.TagType);
            }
        }

        /// <summary>
        /// Retrieves a tag from the registry by its type, name, and namespace.
        /// </summary>
        /// <typeparam name="T">The type of the tag to retrieve.</typeparam>
        /// <param name="name">The name of the tag to retrieve.</param>
        /// <param name="ns">The namespace of the tag to retrieve.</param>
        /// <returns>Returns the instance of the requested tag from the registry.</returns>
        public T GetTag<T>(string name, string ns)
            where T : Tag
        {
            Logger.Log(LogLevel.Debug, "GetTag<T>(string, string) called");
            return GetTag<T>(XName.Get(name, ns));
        }

        /// <summary>
        /// Retrieves the requested tag from the registry based on its name.
        /// </summary>
        /// <typeparam name="T">The type of tag to retrieve.</typeparam>
        /// <param name="name">The local name of the tag.</param>
        /// <returns>The requested tag of type T, or the default value if not found.</returns>
        public T GetTag<T>(XName name)
        {
            Logger.Log(LogLevel.Debug, "GetTag<T>(XName) called");
            var tag = default(T);

            Logger.Log(LogLevel.Debug, $"Finding tag {name.LocalName}...");

            if (_types.TryGetValue(name, out var type))
            {
                var constructor = Tag.GetConstructor(type, Array.Empty<Type>());
                if (constructor is null)
                {
                    constructor = Tag.GetConstructor(type, [typeof(XName)]);
                    if (constructor != null)
                    {
                        tag = (T)constructor.Invoke([name]);
                    }
                }
                else
                {
                    tag = (T)constructor.Invoke([]);
                }
            }
            else
            {
                return default;
            }

            Logger.Log(LogLevel.Debug, "Tag found");

            return tag;
        }

        /// <summary>
        /// Retrieves a tag of the specified type from the registry based on the provided element.
        /// </summary>
        /// <typeparam name="T">The type of tag to retrieve.</typeparam>
        /// <param name="element">The XML element to match against the tag in the registry.</param>
        /// <returns>The matching tag of the specified type if found; otherwise, the default value of the specified type.</returns>
        public T GetTag<T>(XElement element)
        {
            Logger.Log(LogLevel.Debug, "GetTag<T>(XElement) called");
            Logger.Log(LogLevel.Debug, $"Finding tag for element: {element.Name.LocalName}");

            try
            {
                var gotType = _types.TryGetValue(element.Name, out var type);

                if (!gotType)
                {
                    switch (element.Name.LocalName)
                    {
                        case "iq":
                        case "presence":
                        case "message":
                        case "error":
                            element.Name = XName.Get(element.Name.LocalName, Namespaces.Client);
                            gotType = _types.TryGetValue(element.Name, out type);
                            break;
                    }
                }

                if (gotType)
                {
                    Logger.Log(LogLevel.Debug, $"Constructing type: {type}");
                    var constructor = type.GetConstructor([element.GetType()]);
                    if (constructor is null)
                    {
                        var defaultConstructorInfo = Tag.GetConstructor(element.GetType(), [typeof(Tag)]);
                        if (defaultConstructorInfo is null)
                        {
                            return default;
                        }

                        return (T)defaultConstructorInfo.Invoke([element]);
                    }

                    return (T)constructor.Invoke([element]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return default;
        }
    }
}
