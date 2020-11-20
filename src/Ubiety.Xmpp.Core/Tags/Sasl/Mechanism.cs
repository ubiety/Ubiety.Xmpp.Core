﻿// Copyright 2018 Dieter Lunn
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

using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;

namespace Ubiety.Xmpp.Core.Tags.Sasl
{
    /// <summary>
    ///     SASL authentication mechanism.
    /// </summary>
    [XmppTag("mechanism", Namespaces.Sasl, typeof(Mechanism))]
    public class Mechanism : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Mechanism" /> class.
        /// </summary>
        public Mechanism()
            : base(XmlName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mechanism" /> class.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> to derive tag from.</param>
        public Mechanism(XElement element)
            : base(element)
        {
        }

        /// <summary>
        ///     Gets the XML name of the tag.
        /// </summary>
        public static XName XmlName { get; } = XName.Get("mechanism", Namespaces.Sasl);

        /// <summary>
        ///     Gets or sets the mechanism type.
        /// </summary>
        public MechanismTypes Type
        {
            get => ToTypeFromString(Value);

            set => Value = ToStringFromType(value);
        }

        /// <summary>
        ///     Convert a mechanism to its type format.
        /// </summary>
        /// <param name="type">String type of the mechanism.</param>
        /// <returns>Type of the mechanism.</returns>
        public static MechanismTypes ToTypeFromString(string type)
        {
            return type switch
            {
                "PLAIN" => MechanismTypes.Plain,
                "DIGEST-MD5" => MechanismTypes.DigestMd5,
                "EXTERNAL" => MechanismTypes.External,
                "SCRAM-SHA-1" => MechanismTypes.Scram,
                "SCRAM-SHA-1-PLUS" => MechanismTypes.ScramPlus,
                _ => MechanismTypes.None,
            };
        }

        /// <summary>
        ///     Converts a mechanism type to a string.
        /// </summary>
        /// <param name="type">Type to convert.</param>
        /// <returns>String name of the mechanism.</returns>
        public static string ToStringFromType(MechanismTypes type)
        {
            return type switch
            {
                MechanismTypes.None => string.Empty,
                MechanismTypes.Plain => "PLAIN",
                MechanismTypes.DigestMd5 => "DIGEST-MD5",
                MechanismTypes.External => "EXTERNAL",
                MechanismTypes.Scram => "SCRAM-SHA-1",
                MechanismTypes.ScramPlus => "SCRAM-SHA-1-PLUS",
                MechanismTypes.Default => string.Empty,
                _ => string.Empty,
            };
        }
    }
}
