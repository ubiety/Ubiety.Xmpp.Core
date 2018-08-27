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
        ///     Initializes a new instance of the <see cref="Stanza"/> class
        /// </summary>
        /// <param name="name">Name of the tag with namespace</param>
        /// <inheritdoc />
        public Stanza(XName name) : base(name)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stanza"/> class
        /// </summary>
        /// <param name="other">Tag to create the stanza with</param>
        /// <inheritdoc />
        public Stanza(XElement other) : base(other)
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
