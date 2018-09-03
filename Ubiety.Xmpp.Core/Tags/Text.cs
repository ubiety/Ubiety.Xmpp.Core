using System.Xml.Linq;
using Ubiety.Xmpp.Core.Attributes;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     Generic text tag
    /// </summary>
    /// <inheritdoc />
    [XmppTag("text", Namespaces.XmppStreams, typeof(Text))]
    public class Text : Tag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Text"/> class
        /// </summary>
        /// <inheritdoc />
        public Text() : base(XmlName)
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Text"/> class
        /// </summary>
        /// <param name="other">Element to base the tag on</param>
        /// <inheritdoc />
        public Text(XElement other) : base(other)
        {
        }

        /// <summary>
        ///     Xml name of the tag
        /// </summary>
        public static XName XmlName { get; } = XName.Get("text", Namespaces.XmppStreams);
    }
}
