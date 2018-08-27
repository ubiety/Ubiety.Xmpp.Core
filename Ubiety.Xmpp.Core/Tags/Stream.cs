using System.Text;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Tags
{
    /// <summary>
    ///     XMPP Stream tag
    /// </summary>
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
