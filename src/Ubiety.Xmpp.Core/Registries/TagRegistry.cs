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
using Ubiety.Xmpp.Core.Attributes;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Registries
{
    /// <summary>
    ///     Tag registry.
    /// </summary>
    public class TagRegistry
    {
        private static readonly ILog Logger = Log.Get<TagRegistry>();
        private readonly Dictionary<XName, Type> _types = new Dictionary<XName, Type>();

        /// <summary>
        ///     Add tags from the assembly to the registry.
        /// </summary>
        /// <param name="assembly">Assembly to add tags from.</param>
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
        ///     Retrieves a tag from the registry.
        /// </summary>
        /// <typeparam name="T">Type of tag to retrieve.</typeparam>
        /// <param name="name">Name of the tag.</param>
        /// <param name="ns">Namespace of the tag.</param>
        /// <returns>Tag requested from the registry.</returns>
        public T GetTag<T>(string name, string ns)
            where T : Tag
        {
            Logger.Log(LogLevel.Debug, "GetTag<T>(string, string) called");
            return GetTag<T>(XName.Get(name, ns));
        }

        /// <summary>
        ///     Retrieves a tag from the registry.
        /// </summary>
        /// <typeparam name="T">Type of tag to retrieve.</typeparam>
        /// <param name="name">XML name of the tag.</param>
        /// <returns>Tag requested from the registry.</returns>
        public T GetTag<T>(XName name)
        {
            Logger.Log(LogLevel.Debug, "GetTag<T>(XName) called");
            var tag = default(T);

            Logger.Log(LogLevel.Debug, $"Finding tag {name.LocalName}...");

            if (_types.TryGetValue(name, out var type))
            {
                var constructor = Tag.GetConstructor(type, new Type[] { });
                if (constructor is null)
                {
                    constructor = Tag.GetConstructor(type, new[] { typeof(XName) });
                    if (constructor != null)
                    {
                        tag = (T)constructor.Invoke(new object[] { name });
                    }
                }
                else
                {
                    tag = (T)constructor.Invoke(new object[] { });
                }
            }
            else
            {
                return default(T);
            }

            Logger.Log(LogLevel.Debug, "Tag found");

            return tag;
        }

        /// <summary>
        ///     Get a tag from the registry based on the provided XML element.
        /// </summary>
        /// <typeparam name="T">Type of tag to return.</typeparam>
        /// <param name="element">Element to search for.</param>
        /// <returns>Tag from the registry.</returns>
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
                    var constructor = type.GetConstructor(new[] { element.GetType() });
                    if (constructor is null)
                    {
                        var defaultConstructorInfo = Tag.GetConstructor(element.GetType(), new[] { typeof(Tag) });
                        if (defaultConstructorInfo is null)
                        {
                            return default(T);
                        }

                        return (T)defaultConstructorInfo.Invoke(new object[] { element });
                    }

                    return (T)constructor.Invoke(new object[] { element });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return default(T);
        }
    }
}
