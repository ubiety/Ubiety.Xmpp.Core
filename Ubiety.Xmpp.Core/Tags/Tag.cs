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

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     An XMPP tag
    /// </summary>
    /// <inheritdoc />
    public abstract class Tag : XElement
    {
        /// <inheritdoc />
        protected Tag(XElement other) : base(other)
        {
        }

        /// <inheritdoc />
        protected Tag(XName name) : base(name)
        {
        }

        /// <inheritdoc />
        protected Tag(XName name, object content) : base(name, content)
        {
        }

        /// <inheritdoc />
        protected Tag(XName name, params object[] content) : base(name, content)
        {
        }

        /// <inheritdoc />
        protected Tag(XStreamingElement other) : base(other)
        {
        }

        /// <summary>
        ///     Gets the value of a tag attribute
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns>String contents of the attribute</returns>
        protected string GetAttributeValue(XName name)
        {
            var attribute = Attribute(name);
            return attribute?.Value;
        }
    }
}
