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

using System;
using System.Text;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    ///     PLAIN SASL authentication processor
    /// </summary>
    public class PlainProcessor : SaslProcessor
    {
        /// <summary>
        ///     Process the next SASL step
        /// </summary>
        /// <param name="tag">Tag from the server</param>
        /// <returns>Tag to send the server</returns>
        public override Tag Step(Tag tag)
        {
            return tag;
        }

        /// <summary>
        ///     Initializes the PLAIN SASL processor
        /// </summary>
        /// <param name="id"><see cref="Jid" /> of the user to authenticate</param>
        /// <param name="password">Password to use for authentication</param>
        /// <returns>Tag to send to server</returns>
        public override Tag Initialize(Jid id, string password)
        {
            base.Initialize(id, password);

            var auth = $"{(char)0}{id.User}{(char)0}{password}";

            var authTag = Client.Registry.GetTag<Auth>(XName.Get("auth", Namespaces.Sasl));
            authTag.MechanismType = MechanismTypes.Plain;
            authTag.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            return authTag;
        }
    }
}
