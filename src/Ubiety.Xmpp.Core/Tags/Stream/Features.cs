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

using System.Linq;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;
using Ubiety.Xmpp.Core.Tags.Binding;
using Ubiety.Xmpp.Core.Tags.Sasl;
using Ubiety.Xmpp.Core.Tags.Tls;

namespace Ubiety.Xmpp.Core.Tags.Stream
{
    /// <summary>
    /// Represents the features tag in the XMPP stream, allowing access to various negotiated features and mechanisms.
    /// </summary>
    [XmppTag("features", Namespaces.Stream, typeof(Features))]
    public class Features : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Features" /> class.
        /// </summary>
        /// <param name="other"><see cref="XElement" /> to derive tag from.</param>
        public Features(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Features" /> class.
        /// </summary>
        public Features()
            : base(XmlName)
        {
        }

        /// <summary>
        ///     Gets the XML name of the tag.
        /// </summary>
        public static XName XmlName { get; } = XName.Get("features", Namespaces.Stream);

        /// <summary>
        /// Gets the StartTLS feature tag, which is used to negotiate
        /// the start of a secure TLS session in the XMPP stream.
        /// </summary>
        public StartTls StartTls => Element<StartTls>(XName.Get("starttls", Namespaces.Tls));

        /// <summary>
        /// Gets the SASL mechanisms supported by the server for authentication.
        /// </summary>
        public Mechanisms Mechanisms => Element<Mechanisms>(XName.Get("mechanisms", Namespaces.Sasl));

        /// <summary>
        /// Gets the bind tag element from the XMPP feature set, representing a server binding request or requirement.
        /// </summary>
        public Bind Bind => Element<Bind>(XName.Get("bind", Namespaces.Bind));

        /// <summary>
        /// Gets the count of child feature elements contained within the features tag.
        /// </summary>
        public int FeatureCount => Elements().Count();
    }
}
