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
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Coordinates state transitions and manages the XMPP connection state machine.
/// </summary>
public interface IStateMachineCoordinator
{
    /// <summary>
    /// Event raised when a state transition occurs.
    /// </summary>
    event EventHandler<StateTransitionEventArgs>? StateTransitioned;

    /// <summary>
    /// Event raised when a state transition is invalid or fails.
    /// </summary>
    event EventHandler<StateTransitionFailedEventArgs>? StateTransitionFailed;

    /// <summary>
    /// Gets the current state of the state machine.
    /// </summary>
    IState CurrentState { get; }

    /// <summary>
    /// Gets the current state type for easier identification.
    /// </summary>
    Type CurrentStateType { get; }

    /// <summary>
    /// Gets the history of state transitions for debugging.
    /// </summary>
    IReadOnlyList<StateTransitionRecord> TransitionHistory { get; }

    /// <summary>
    /// Processes an incoming tag through the current state.
    /// </summary>
    /// <param name="xmpp">The XMPP base instance.</param>
    /// <param name="tag">The incoming tag to process.</param>
    /// <returns>The result of state processing.</returns>
    StateProcessingResult ProcessTag(XmppBase xmpp, Tag? tag);

    /// <summary>
    /// Transitions to a new state with validation.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    /// <param name="context">Optional context for the transition.</param>
    /// <returns>True if transition was successful, false otherwise.</returns>
    bool TransitionTo(IState newState, string? context = null);

    /// <summary>
    /// Checks if a transition to the specified state type is valid from the current state.
    /// </summary>
    /// <typeparam name="TState">The state type to check.</typeparam>
    /// <returns>True if the transition is valid.</returns>
    bool CanTransitionTo<TState>()
        where TState : IState;

    /// <summary>
    /// Resets the state machine to its initial state.
    /// </summary>
    void Reset();
}
