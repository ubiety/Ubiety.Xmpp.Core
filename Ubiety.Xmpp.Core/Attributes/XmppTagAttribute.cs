using System;
using System.Xml.Linq;

namespace Ubiety.Xmpp.Core.Attributes
{
    /// <summary>
    ///     XMPP tag attribute
    /// </summary>
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XmppTagAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppTagAttribute"/> class
        /// </summary>
        /// <param name="localName">local name of the tag</param>
        /// <param name="namespaceName">namespace name of the tag</param>
        /// <param name="tagType">Class type of the tag</param>
        public XmppTagAttribute(string localName, string namespaceName, Type tagType)
        {
            Name = XName.Get(localName, namespaceName);
            TagType = tagType;
        }

        /// <summary>
        ///     Gets the name of the tag
        /// </summary>
        public XName Name { get; }

        /// <summary>
        ///     Gets the class type of the tag
        /// </summary>
        public Type TagType { get; }
    }
}
