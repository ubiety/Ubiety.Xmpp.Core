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

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     Defines an XMPP stanza
    /// </summary>
    /// <inheritdoc />
    public class Stanza : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Stanza" /> class
        /// </summary>
        /// <param name="name">Name of the tag with namespace</param>
        public Stanza(XName name)
            : base(name)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stanza" /> class
        /// </summary>
        /// <param name="other">Tag to create the stanza with</param>
        public Stanza(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Gets or sets the JID of the user receiving the message
        /// </summary>
        public Jid To
        {
            get => new Jid(GetAttributeValue("to"));
            set => SetAttributeValue("to", value);
        }

        /// <summary>
        ///     Gets or sets the JID of the user sending the message
        /// </summary>
        public Jid From
        {
            get => new Jid(GetAttributeValue("from"));
            set => SetAttributeValue("from", value);
        }

        /// <summary>
        ///     Gets or sets the stanza id
        /// </summary>
        public string Id
        {
            get => GetAttributeValue("id");
            set => SetAttributeValue("id", value);
        }
    }
}
