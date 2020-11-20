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
    ///     SASL authentication challenge tag.
    /// </summary>
    [XmppTag("challenge", Namespaces.Sasl, typeof(Challenge))]
    public class Challenge : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Challenge" /> class.
        /// </summary>
        /// <param name="other"><see cref="XElement" /> to derive the tag from.</param>
        public Challenge(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Challenge" /> class.
        /// </summary>
        public Challenge()
            : base(XmlName)
        {
        }

        /// <summary>
        ///     Gets the XML name of the tag.
        /// </summary>
        public static XName XmlName { get; } = XName.Get("challenge", Namespaces.Sasl);
    }
}
