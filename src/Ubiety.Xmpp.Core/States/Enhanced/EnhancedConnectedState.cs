// Copyright 2024 Dieter Lunn
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
// limitations under the License.

using System;
using System.Collections.Generic;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.StateMachine;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.States.Enhanced;

/// <summary>
/// Enhanced version of ConnectedState that works with the state machine coordinator.
/// Shows how to migrate from direct state transitions to coordinator-managed transitions.
/// </summary>
public class EnhancedConnectedState : EnhancedStateBase
{
    private static readonly ILog Logger = Log.Get<EnhancedConnectedState>();

    /// <inheritdoc />
    public override string StateName => "Connected";

    /// <inheritdoc />
    protected override HashSet<Type> ValidTransitions => new()
    {
        typeof(StreamFeaturesState),
        typeof(DisconnectState),
        typeof(DisconnectedState)
    };

    /// <inheritdoc />
    public override StateProcessingResult ProcessTag(XmppBase xmpp, Tag tag = null)
    {
        Logger.Log(LogLevel.Debug, "Processing in EnhancedConnectedState");

        if (xmpp is not XmppClient client)
        {
            return Failure("XMPP instance is not a client");
        }

        try
        {
            // Create and configure the stream
            var stream = xmpp.TagRegistry.GetTag<Stream>(Stream.XmlName);
            stream.Version = "1.0";
            stream.To = client.Id.Server;
            stream.Namespace = Namespaces.Client;

            // Send the stream start
            client.ClientSocket.Send(stream.StartTag);
            client.ClientSocket.SetReadClear();

            Logger.Log(LogLevel.Debug, "Stream initialization sent, transitioning to StreamFeaturesState");

            // Return result indicating we should transition to StreamFeaturesState
            return SuccessWithTransition(new StreamFeaturesState(), "Stream initialization completed");
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error during stream initialization: {ex.Message}");
            return Failure($"Stream initialization failed: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public override void OnEnter(XmppBase xmpp, string context = null)
    {
        Logger.Log(LogLevel.Debug, $"Entered Connected state. Context: {context ?? "none"}");
        
        // Initialize any resources needed in this state
        // This is called by the coordinator when transitioning into this state
    }

    /// <inheritdoc />
    public override void OnExit(XmppBase xmpp, IState nextState = null)
    {
        Logger.Log(LogLevel.Debug, $"Exiting Connected state to {nextState?.GetType().Name ?? "unknown"}");
        
        // Clean up any resources used by this state
        // This is called by the coordinator when transitioning out of this state
    }

    /// <inheritdoc />
    public override bool CanTransitionTo(Type targetStateType)
    {
        var canTransition = base.CanTransitionTo(targetStateType);
        
        if (!canTransition)
        {
            Logger.Log(LogLevel.Warning, $"Invalid transition attempt from {StateName} to {targetStateType.Name}");
        }
        
        return canTransition;
    }
}
