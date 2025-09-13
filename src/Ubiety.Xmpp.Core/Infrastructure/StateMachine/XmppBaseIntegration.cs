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
//   limitations under the License.

using System;
using System.Collections.Generic;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Shows how XmppBase would be modified to use the state machine coordinator.
/// This is an example of the integration pattern, not actual production code.
/// </summary>
public abstract class EnhancedXmppBase : XmppBase
{
    private readonly ILog _logger;
    private readonly IStateMachineCoordinator _stateCoordinator;
    private bool _disposedValue;

    protected EnhancedXmppBase() : base()
    {
        _logger = Log.Get<EnhancedXmppBase>();
        _stateCoordinator = new XmppStateMachineCoordinator(new DisconnectedState());

        // Subscribe to state machine events
        _stateCoordinator.StateTransitioned += OnStateTransitioned;
        _stateCoordinator.StateTransitionFailed += OnStateTransitionFailed;

        _logger.Log(LogLevel.Debug, $"{GetType()} created with state machine coordinator");
    }

    /// <summary>
    /// Event that is raised when a stream error occurs during XMPP communication.
    /// </summary>
    public new event EventHandler<Ubiety.Xmpp.Core.Common.ErrorEventArgs>? Error;

    /// <summary>
    /// Event raised when the connection state changes.
    /// </summary>
    public event EventHandler<StateTransitionEventArgs>? StateChanged;

    /// <summary>
    /// Event raised when a state transition fails.
    /// </summary>
    public event EventHandler<StateTransitionFailedEventArgs>? StateTransitionError;

    /// <summary>
    /// Gets the current state through the coordinator.
    /// </summary>
    public new IState State => _stateCoordinator.CurrentState;

    /// <summary>
    /// Gets the state machine coordinator for advanced operations.
    /// </summary>
    protected IStateMachineCoordinator StateCoordinator => _stateCoordinator;

    /// <summary>
    /// Handles incoming XMPP tags from the parser using the state machine coordinator.
    /// This replaces the original Parser_Tag method.
    /// </summary>
    /// <param name="sender">The source of the tag event.</param>
    /// <param name="e">Event arguments containing the parsed XMPP tag.</param>
    protected new void Parser_Tag(object sender, TagEventArgs e)
    {
        _logger.Log(LogLevel.Debug, $"Processing tag '{e.Tag?.GetType().Name}' through state coordinator");

        // Use the coordinator to process the tag
        var result = _stateCoordinator.ProcessTag(this, e.Tag);

        if (!result.Success)
        {
            _logger.Log(LogLevel.Error, $"State processing failed: {result.ErrorMessage}");

            // Handle processing failures
            OnError(this, new Ubiety.Xmpp.Core.Common.ErrorEventArgs
            {
                Message = result.ErrorMessage,
                StreamError = null // Could extract from the result if needed
            });

            // Attempt to transition to disconnect state on failure
            _stateCoordinator.TransitionTo(new DisconnectState(), "Processing failure");
        }
    }

    /// <summary>
    /// Transitions to a new state with validation and logging.
    /// This replaces direct state assignments like: xmpp.State = new SomeState();
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    /// <param name="context">Optional context for the transition.</param>
    /// <returns>True if transition was successful.</returns>
    public bool TransitionToState(IState newState, string? context = null)
    {
        return _stateCoordinator.TransitionTo(newState, context);
    }

    /// <summary>
    /// Checks if a transition to the specified state type is currently valid.
    /// </summary>
    /// <typeparam name="TState">The state type to check.</typeparam>
    /// <returns>True if the transition is valid.</returns>
    public bool CanTransitionTo<TState>() where TState : IState
    {
        return _stateCoordinator.CanTransitionTo<TState>();
    }

    /// <summary>
    /// Gets the transition history for debugging purposes.
    /// </summary>
    public IReadOnlyList<StateTransitionRecord> GetTransitionHistory()
    {
        return _stateCoordinator.TransitionHistory;
    }

    /// <summary>
    /// Resets the connection state machine.
    /// </summary>
    public void ResetStateMachine()
    {
        _stateCoordinator.Reset();
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            _logger.Log(LogLevel.Debug, "Disposing enhanced XMPP base");

            // Unsubscribe from events
            _stateCoordinator.StateTransitioned -= OnStateTransitioned;
            _stateCoordinator.StateTransitionFailed -= OnStateTransitionFailed;
        }

        _disposedValue = true;

        // Call base class disposal
        base.Dispose(disposing);
    }

    private void OnStateTransitioned(object? sender, StateTransitionEventArgs e)
    {
        _logger.Log(LogLevel.Debug,
            $"State transitioned from {e.FromState.GetType().Name} to {e.ToState.GetType().Name}. Context: {e.Context ?? "none"}");

        // Raise the public event
        StateChanged?.Invoke(this, e);
    }

    private void OnStateTransitionFailed(object? sender, StateTransitionFailedEventArgs e)
    {
        _logger.Log(LogLevel.Warning,
            $"State transition failed from {e.CurrentState.GetType().Name} to {e.AttemptedStateType?.Name ?? "unknown"}. Reason: {e.Reason}");

        // Raise the public event
        StateTransitionError?.Invoke(this, e);

        // Could implement automatic error recovery here
        // For example, transition to a safe state on certain types of failures
    }

    private void OnError(object sender, Ubiety.Xmpp.Core.Common.ErrorEventArgs e)
    {
        Error?.Invoke(sender, e);
    }
}

/// <summary>
/// Example of how XmppClient would be updated to use the enhanced base class.
/// </summary>
public class ExampleEnhancedXmppClient : EnhancedXmppBase, IClient
{
    private readonly ILog _logger;

    public ExampleEnhancedXmppClient()
    {
        _logger = Log.Get<ExampleEnhancedXmppClient>();
        _logger.Log(LogLevel.Debug, "Enhanced XMPP client created");
    }

    public Jid Id { get; set; }
    public string Password { get; set; }
    public new int Port { get; set; } = 5222;
    public new bool UseSsl { get; internal init; }
    public new bool UseIPv6 { get; internal init; }
    public bool Authenticated { get; internal set; }

    public void Connect(Jid jid, string password)
    {
        _logger.Log(LogLevel.Debug, $"Connecting to server for {jid}");
        
        ArgumentException.ThrowIfNullOrEmpty(jid);
        ArgumentException.ThrowIfNullOrEmpty(password);

        Id = jid;
        Password = password;

        // Use the coordinator to transition to connecting state
        var success = TransitionToState(new ConnectingState(), $"Connecting to {jid}");
        if (!success)
        {
            throw new InvalidOperationException("Failed to transition to connecting state");
        }

        // Execute the new state (in enhanced model, this would be handled automatically)
        State.Execute(this);
    }
}
