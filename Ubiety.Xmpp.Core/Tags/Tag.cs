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
    }
}
