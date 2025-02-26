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
using Ubiety.Xmpp.Core.Tags.Binding;

namespace Ubiety.Xmpp.Core.Tags.Client
{
    /// <summary>
    /// Represents the IQ (Info/Query) types used in XMPP messages.
    /// </summary>
    public enum IqType
    {
        /// <summary>
        /// IQ Get.
        /// </summary>
        Get,

        /// <summary>
        /// IQ Set.
        /// </summary>
        Set,

        /// <summary>
        /// IQ Error.
        /// </summary>
        Error,

        /// <summary>
        /// IQ Result.
        /// </summary>
        Result,
    }

    /// <summary>
    /// Represents an IQ (Info/Query) stanza, a fundamental XMPP protocol element used for sending structured information and requests.
    /// </summary>
    /// <remarks>
    /// An IQ stanza is used for structured XML exchange in the XMPP network. It supports the exchange
    /// of queries or command requests from one entity to another and is one of the main types within
    /// the XMPP protocol alongside 'message' and 'presence'.
    /// </remarks>
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
        /// Gets the XML name for the IQ stanza.
        /// </summary>
        /// <remarks>
        /// Represents the qualified name of the "iq" tag within the XMPP client namespace.
        /// </remarks>
        public static XName XmlName { get; } = XName.Get("iq", Namespaces.Client);

        /// <summary>
        /// Gets or sets the type of the IQ (Info/Query) stanza.
        /// </summary>
        /// <remarks>
        /// Determines the specific purpose or operation of the IQ stanza within XMPP communication.
        /// Supported types include requests for information, configuration changes, error notifications, and operation results.
        /// </remarks>
        public IqType IqType
        {
            get => GetAttributeEnumValue<IqType>("type");
            set => SetAttributeEnumValue("type", value);
        }

        /// <summary>
        /// Gets the Bind tag associated with the IQ stanza for resource binding in XMPP.
        /// </summary>
        public Bind Bind => Element<Bind>(XName.Get("bind", Namespaces.Bind));
    }
}
