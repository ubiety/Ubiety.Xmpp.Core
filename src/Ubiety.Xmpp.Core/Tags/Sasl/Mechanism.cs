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

using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;

namespace Ubiety.Xmpp.Core.Tags.Sasl
{
    /// <summary>
    /// Represents an XMPP SASL mechanism tag used in the authentication process.
    /// Inherits from the <see cref="Tag"/> class.
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
        /// Gets or sets the type of the SASL mechanism.
        /// </summary>
        public MechanismTypes Type
        {
            get => ToTypeFromString(Value);

            set => Value = ToStringFromType(value);
        }

        /// <summary>
        /// Converts a string representation of a mechanism type to its corresponding <see cref="MechanismTypes"/> enum value.
        /// </summary>
        /// <param name="type">The string representation of the mechanism type.</param>
        /// <returns>The corresponding <see cref="MechanismTypes"/> enum value. Returns <see cref="MechanismTypes.None"/> if the string does not match any known type.</returns>
        public static MechanismTypes ToTypeFromString(string type)
        {
            return type switch
            {
                "PLAIN" => MechanismTypes.Plain,
                "DIGEST-MD5" => MechanismTypes.DigestMd5,
                "EXTERNAL" => MechanismTypes.External,
                "SCRAM-SHA-1" => MechanismTypes.Scram1,
                "SCRAM-SHA-1-PLUS" => MechanismTypes.Scram1Plus,
                "SCRAM-SHA-256" => MechanismTypes.Scram256,
                "SCRAM-SHA-256-PLUS" => MechanismTypes.Scram256Plus,
                "SCRAM-SHA-512" => MechanismTypes.Scram512,
                "SCRAM-SHA-512-PLUS" => MechanismTypes.Scram512Plus,
                _ => MechanismTypes.None,
            };
        }

        /// <summary>
        /// Converts a given <see cref="MechanismTypes" /> enumeration value to its string representation.
        /// </summary>
        /// <param name="type">The <see cref="MechanismTypes" /> value to convert.</param>
        /// <returns>A string representation of the given mechanism type.</returns>
        public static string ToStringFromType(MechanismTypes type)
        {
            return type switch
            {
                MechanismTypes.None => string.Empty,
                MechanismTypes.Plain => "PLAIN",
                MechanismTypes.DigestMd5 => "DIGEST-MD5",
                MechanismTypes.External => "EXTERNAL",
                MechanismTypes.Scram1 => "SCRAM-SHA-1",
                MechanismTypes.Scram1Plus => "SCRAM-SHA-1-PLUS",
                MechanismTypes.Scram256 => "SCRAM-SHA-256",
                MechanismTypes.Scram256Plus => "SCRAM-SHA-256-PLUS",
                MechanismTypes.Scram512 => "SCRAM-SHA-512",
                MechanismTypes.Scram512Plus => "SCRAM-SHA-512-PLUS",
                MechanismTypes.Default => string.Empty,
                _ => string.Empty,
            };
        }
    }
}
