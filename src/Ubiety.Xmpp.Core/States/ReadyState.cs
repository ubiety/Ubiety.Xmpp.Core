// Copyright 2025 Dieter Lunn
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
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States;

/// <summary>
/// Represents the state responsible for all messages after connection established.
/// </summary>
public class ReadyState : IState
{
    /// <summary>
    /// Executes the ready operations.
    /// </summary>
    /// <param name="xmpp">The XMPP base instance used for communication.</param>
    /// <param name="tag">The received tag to process, or null if initiating the bind operation.</param>
    public void Execute(XmppBase xmpp, Tag tag = null)
    {
        var client = xmpp as XmppClient;
        client?.OnStanza(tag);
    }
}
