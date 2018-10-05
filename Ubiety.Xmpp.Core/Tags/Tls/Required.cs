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
using Ubiety.Xmpp.Core.Attributes;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Tags.Tls
{
    /// <inheritdoc />
    [XmppTag("required", Namespaces.Tls, typeof(Required))]
    public class Required : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Required" /> class
        /// </summary>
        /// <param name="other"><see cref="XElement" /> to derive the tag from</param>
        public Required(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Required" /> class
        /// </summary>
        public Required()
            : base(XmlName)
        {
        }

        /// <summary>
        ///     Gets the XML name of the tag
        /// </summary>
        public static XName XmlName { get; } = XName.Get("required", Namespaces.Tls);
    }
}