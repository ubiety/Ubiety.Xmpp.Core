// Copyright 2019 Dieter Lunn
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

namespace Ubiety.Xmpp.Core.Tags.Client
{
    /// <summary>
    ///     IQ tag type.
    /// </summary>
    public enum IqType
    {
        /// <summary>
        ///     IQ Get.
        /// </summary>
        Get,

        /// <summary>
        ///     IQ Set.
        /// </summary>
        Set,

        /// <summary>
        ///     IQ Error.
        /// </summary>
        Error,

        /// <summary>
        ///     IQ Result.
        /// </summary>
        Result,
    }

    /// <summary>
    ///     XMPP Iq tag.
    /// </summary>
    [XmppTag("iq", Namespaces.Client, typeof(Iq))]
    public class Iq : Stanza
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Iq"/> class.
        /// </summary>
        public Iq()
            : base(XmlName)
        {
            Id = GetNextPacketId();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Iq"/> class.
        /// </summary>
        /// <param name="element"><see cref="XElement"/> to derive the tag from.</param>
        public Iq(XElement element)
            : base(element)
        {
            Id = GetNextPacketId();
        }

        /// <summary>
        ///     Gets the XML name of the tag.
        /// </summary>
        public static XName XmlName { get; } = XName.Get("iq", Namespaces.Client);

        /// <summary>
        ///     Gets or sets the IQ tag type.
        /// </summary>
        public IqType IqType
        {
            get => GetAttributeEnumValue<IqType>("type");
            set => SetAttributeEnumValue("type", value);
        }
    }
}
