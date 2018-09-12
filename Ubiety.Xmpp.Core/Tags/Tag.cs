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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     An XMPP tag
    /// </summary>
    /// <inheritdoc />
    public abstract class Tag : XElement
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tag" /> class
        /// </summary>
        /// <param name="other"><see cref="XElement" /> to derive the tag from</param>
        protected Tag(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tag" /> class
        /// </summary>
        /// <param name="name"><see cref="XName" /> of the tag</param>
        protected Tag(XName name)
            : base(name)
        {
        }

        /// <summary>
        ///     Gets or sets the tag contents as a byte array
        /// </summary>
        public byte[] Bytes
        {
            get => System.Convert.FromBase64String(Value);
            set => Value = System.Convert.ToBase64String(value);
        }

        /// <summary>
        ///     Gets the constructor for a tag
        /// </summary>
        /// <param name="type">Type of the tag</param>
        /// <param name="parameters">Constructor parameters</param>
        /// <returns>Constructor info of the tag constructor</returns>
        public static ConstructorInfo GetConstructor(Type type, IReadOnlyCollection<Type> parameters)
        {
            var results = from constructor in type.GetTypeInfo().DeclaredConstructors
                let constructorParameters = constructor.GetParameters().Select(_ => _.ParameterType).ToArray()
                where constructorParameters.Length == parameters.Count &&
                      !constructorParameters.Except(parameters).Any() &&
                      !parameters.Except(constructorParameters).Any()
                select constructor;

            return results.FirstOrDefault();
        }

        /// <summary>
        ///     Get child tags
        /// </summary>
        /// <typeparam name="T">Type of tags</typeparam>
        /// <param name="name">Name of the tags</param>
        /// <returns>Enumerable of the tags</returns>
        protected IEnumerable<T> Elements<T>(XName name)
            where T : XElement
        {
            return Elements(name).Select(Convert<T>);
        }

        /// <summary>
        ///     Get a child tag
        /// </summary>
        /// <typeparam name="T">Type of the child to get</typeparam>
        /// <param name="name">XML name of the tag</param>
        /// <returns>Child tag</returns>
        protected T Element<T>(XName name)
            where T : XElement
        {
            return Convert<T>(Element(name));
        }

        /// <summary>
        ///     Gets the value of a tag attribute
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns>String contents of the attribute</returns>
        protected string GetAttributeValue(XName name)
        {
            var attribute = Attribute(name);
            return attribute?.Value;
        }

        private static T Convert<T>(XElement element)
            where T : XElement
        {
            if (element is null) return default(T);

            var constructor = GetConstructor(typeof(T), new[] {typeof(XElement)});
            return (T) constructor?.Invoke(new object[] {element});
        }
    }
}