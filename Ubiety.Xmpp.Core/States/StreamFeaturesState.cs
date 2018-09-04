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

using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.States
{
    /// <inheritdoc />
    public class StreamFeaturesState : IState
    {
        private static readonly ILog Logger;

        static StreamFeaturesState()
        {
            Logger = Log.Get<StreamFeaturesState>();
        }

        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            Features features;

            Logger.Log(LogLevel.Debug, "Starting to parse features");
            switch (tag)
            {
                case Stream s when s.Version.StartsWith("1."):
                    features = s.Features;
                    break;
                case Features f:
                    features = f;
                    break;
                default:
                    Logger.Log(LogLevel.Error, "Unexpected tag. Wrong state executed");
                    return;
            }

            if ((xmpp.UseSsl && features.StartTls != null) || features.StartTls.Required)
            {
                Logger.Log(LogLevel.Debug, "SSL/TLS is required or it is supported and we want to use it");
                xmpp.State = new StartTlsState();
                xmpp.State.Execute(xmpp);
                return;
            }
        }
    }
}