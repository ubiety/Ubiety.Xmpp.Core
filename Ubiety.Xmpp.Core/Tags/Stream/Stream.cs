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

using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Attributes;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Tags.Stream
{
    /// <summary>
    ///     XMPP Stream tag
    /// </summary>
    [XmppTag("stream", Namespaces.Stream, typeof(Stream))]
    public class Stream : Stanza
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Stream" /> class
        /// </summary>
        public Stream()
            : base(XmlName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stream" /> class
        /// </summary>
        /// <param name="other">Element to base the tag on</param>
        public Stream(XElement other)
            : base(other)
        {
        }

        /// <summary>
        ///     Gets the XML name of the tag
        /// </summary>
        public static XName XmlName { get; } = XName.Get("stream", Namespaces.Stream);

        /// <summary>
        ///     Gets or sets the stream version
        /// </summary>
        public string Version
        {
            get => GetAttributeValue("version");
            set => SetAttributeValue("version", value);
        }

        /// <summary>
        ///     Gets or sets the stream namespace
        /// </summary>
        public string Namespace
        {
            get => GetAttributeValue("xmlns");
            set => SetAttributeValue("xmlns", value);
        }

        /// <summary>
        ///     Gets the stream errors
        /// </summary>
        public IEnumerable<Error> Errors => Elements<Error>(XName.Get("error", Namespaces.Stream));

        /// <summary>
        ///     Gets the stream features
        /// </summary>
        public Features Features => Element<Features>(XName.Get("features", Namespaces.Stream));

        /// <summary>
        ///     Gets the start tag of the stream
        /// </summary>
        public string StartTag
        {
            get
            {
                var tag = new StringBuilder(
                    $"<{XmlName.LocalName}:{XmlName.LocalName} xmlns:{XmlName.LocalName}=\'{XmlName.NamespaceName}\'");
                foreach (var attribute in Attributes())
                    tag.Append($" {attribute.Name.LocalName}=\'{attribute.Value}\'");

                tag.Append(">");

                return tag.ToString();
            }
        }
    }
}