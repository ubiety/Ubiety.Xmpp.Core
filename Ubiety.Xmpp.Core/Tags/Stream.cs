using System.Text;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Attributes;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     XMPP Stream tag
    /// </summary>
    [XmppTag("stream", Namespaces.Stream, typeof(Stream))]
    public class Stream : Stanza
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Stream"/> class
        /// </summary>
        public Stream() : base(XmlName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stream"/> class
        /// </summary>
        /// <param name="other"></param>
        public Stream(XElement other) : base(other)
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
        ///     Gets the start tag of the stream
        /// </summary>
        public string StartTag
        {
            get
            {
                var tag = new StringBuilder($"<{XmlName.LocalName}:{XmlName.LocalName} xmlns:{XmlName.LocalName}=\'{XmlName.NamespaceName}\'");
                foreach (var attribute in Attributes())
                {
                    tag.Append($" {attribute.Name.LocalName}=\'{attribute.Value}\'");
                }

                tag.Append(">");

                return tag.ToString();
            }
        }
    }
}
