// Copyright 2020 Dieter Lunn
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Registries;

/// <summary>
///     SASL authentication mechanism registry.
/// </summary>
public class SaslRegistry
{
    private static readonly ILog Logger = Log.Get<SaslRegistry>();
    private readonly Dictionary<string, (Type processor, int weight, bool binding, MechanismTypes type)> _mechanisms = new ();

    /// <summary>
    ///     Add assembly to the registry.
    /// </summary>
    /// <param name="assembly">Assembly to add.</param>
    public void AddAssembly(Assembly assembly)
    {
        Logger.Log(LogLevel.Information, $"Adding assembly {assembly.FullName} to SASL registry.");

        var attributes = assembly.GetAttributes<SaslAttribute>();
        foreach (var attribute in attributes)
        {
            _mechanisms.Add(attribute.MechanismName, (attribute.ProcessorType, attribute.Weight, attribute.ChannelBinding, attribute.Type));
        }
    }

    /// <summary>
    ///     Gets the SASL processor with the highest weight supported by the server.
    /// </summary>
    /// <param name="serverMechanisms">SASL mechanisms supported by the server.</param>
    /// <param name="client">XMPP client instance.</param>
    /// <returns><see cref="SaslProcessor" /> that is to be used for authentication.</returns>
    public SaslProcessor GetProcessor(IEnumerable<Mechanism> serverMechanisms, XmppBase client)
    {
        var (processorType, _, binding, type) = (from attr in _mechanisms
            join server in serverMechanisms on attr.Key equals server.Value
            orderby attr.Value.weight descending
            select attr.Value).First();

        var processor = (SaslProcessor)Activator.CreateInstance(processorType);

        switch (processor)
        {
            case PlainProcessor when !client.ClientSocket.Secure:
                throw new InvalidOperationException("Do not use PLAIN SASL processor on an unsecured connection.");
            case null:
                return null;
            default:
                processor.ChannelBinding = binding;
                processor.Client = client;
                processor.MechanismType = type;

                return processor;
        }
    }
}
