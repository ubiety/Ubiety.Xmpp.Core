// Copyright 2025 Dieter Lunn
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

namespace Ubiety.Xmpp.Core.Tags.Binding;

/// <summary>
///     Jid tag for binding resources.
/// </summary>
[XmppTag("jid", Namespaces.Bind, typeof(Jid))]
public class Jid : Tag
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Binding.Jid"/> class.
    /// </summary>
    public Jid()
        : base(XmlName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Binding.Jid"/> class.
    /// </summary>
    /// <param name="other">XElement to derive <see cref="Binding.Jid"/> from.</param>
    public Jid(XElement other)
        : base(other)
    {
    }

    /// <summary>
    /// Gets the Jabber ID (JID) representation of the tag's value.
    /// </summary>
    public Common.Jid Id => Common.Jid.Parse(Value, true);

    /// <summary>
    ///     Gets the XML name of the tag.
    /// </summary>
    public static XName XmlName { get; } = XName.Get("jid", Namespaces.Bind);
}
