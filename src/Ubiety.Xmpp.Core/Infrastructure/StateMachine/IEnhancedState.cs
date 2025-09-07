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
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Enhanced state interface that returns processing results instead of performing direct state transitions.
/// This allows the coordinator to manage transitions and validation.
/// </summary>
public interface IEnhancedState : States.IState
{
    /// <summary>
    /// Gets the name of this state for logging and debugging.
    /// </summary>
    string StateName { get; }

    /// <summary>
    /// Processes a tag and returns the result, allowing the coordinator to handle transitions.
    /// </summary>
    /// <param name="xmpp">The XMPP base instance.</param>
    /// <param name="tag">The tag to process (can be null for initialization).</param>
    /// <returns>The processing result indicating success/failure and next actions.</returns>
    StateProcessingResult ProcessTag(XmppBase xmpp, Tag tag = null);

    /// <summary>
    /// Called when entering this state. Allows for initialization.
    /// </summary>
    /// <param name="xmpp">The XMPP base instance.</param>
    /// <param name="context">Context about why we entered this state.</param>
    void OnEnter(XmppBase xmpp, string context = null);

    /// <summary>
    /// Called when exiting this state. Allows for cleanup.
    /// </summary>
    /// <param name="xmpp">The XMPP base instance.</param>
    /// <param name="nextState">The state we're transitioning to.</param>
    void OnExit(XmppBase xmpp, States.IState nextState = null);

    /// <summary>
    /// Determines if this state can transition to the specified state type.
    /// </summary>
    /// <param name="targetStateType">The type of state to transition to.</param>
    /// <returns>True if the transition is allowed.</returns>
    bool CanTransitionTo(Type targetStateType);
}

/// <summary>
/// Base class for enhanced states that provides common functionality.
/// </summary>
public abstract class EnhancedStateBase : IEnhancedState
{
    /// <summary>
    /// Valid transitions from this state type.
    /// </summary>
    protected abstract HashSet<Type> ValidTransitions { get; }

    /// <inheritdoc />
    public abstract string StateName { get; }

    /// <inheritdoc />
    public abstract StateProcessingResult ProcessTag(XmppBase xmpp, Tag tag = null);

    /// <inheritdoc />
    public virtual void OnEnter(XmppBase xmpp, string context = null)
    {
        // Default implementation - can be overridden by specific states
    }

    /// <inheritdoc />
    public virtual void OnExit(XmppBase xmpp, States.IState nextState = null)
    {
        // Default implementation - can be overridden by specific states
    }

    /// <inheritdoc />
    public virtual bool CanTransitionTo(Type targetStateType)
    {
        return ValidTransitions.Contains(targetStateType);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This is the legacy method from IState. Enhanced states should use ProcessTag instead.
    /// This implementation delegates to ProcessTag but doesn't handle transitions automatically.
    /// </remarks>
    public void Execute(XmppBase xmpp, Tag tag = null)
    {
        var result = ProcessTag(xmpp, tag);
        if (!result.Success)
        {
            throw new InvalidOperationException($"State {StateName} processing failed: {result.ErrorMessage}");
        }

        // Note: In the enhanced model, the coordinator handles transitions
        // This Execute method is kept for backward compatibility
    }

    /// <summary>
    /// Helper method to create a successful result without transition.
    /// </summary>
    protected StateProcessingResult Success() => StateProcessingResult.CreateSuccess();

    /// <summary>
    /// Helper method to create a successful result with a transition.
    /// </summary>
    /// <param name="nextState">The next state to transition to.</param>
    /// <param name="context">Optional context for the transition.</param>
    protected StateProcessingResult SuccessWithTransition(States.IState nextState, string context = null)
        => StateProcessingResult.SuccessWithTransition(nextState, context);

    /// <summary>
    /// Helper method to create a failure result.
    /// </summary>
    /// <param name="error">The error message.</param>
    protected StateProcessingResult Failure(string error) => StateProcessingResult.Failure(error);
}
