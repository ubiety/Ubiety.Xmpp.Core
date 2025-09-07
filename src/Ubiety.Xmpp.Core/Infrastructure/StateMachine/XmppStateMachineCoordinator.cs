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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Coordinates XMPP state transitions with validation, logging, and error handling.
/// </summary>
public class XmppStateMachineCoordinator : IStateMachineCoordinator
{
    private readonly ILog _logger;
    private readonly object _stateLock = new();
    private readonly ConcurrentQueue<StateTransitionRecord> _transitionHistory = new();
    private readonly int _maxHistorySize;

    private IState _currentState;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmppStateMachineCoordinator"/> class.
    /// </summary>
    /// <param name="initialState">The initial state of the state machine.</param>
    /// <param name="maxHistorySize">Maximum number of transition records to keep in history.</param>
    public XmppStateMachineCoordinator(IState initialState = null, int maxHistorySize = 100)
    {
        _logger = Log.Get<XmppStateMachineCoordinator>();
        _maxHistorySize = maxHistorySize;
        _currentState = initialState ?? new DisconnectedState();

        _logger.Log(LogLevel.Debug, $"State machine coordinator initialized with state: {CurrentStateType.Name}");
    }

    /// <inheritdoc />
    public event EventHandler<StateTransitionEventArgs> StateTransitioned;

    /// <inheritdoc />
    public event EventHandler<StateTransitionFailedEventArgs> StateTransitionFailed;

    /// <inheritdoc />
    public IState CurrentState
    {
        get
        {
            lock (_stateLock)
            {
                return _currentState;
            }
        }
    }

    /// <inheritdoc />
    public Type CurrentStateType => CurrentState.GetType();

    /// <inheritdoc />
    public IReadOnlyList<StateTransitionRecord> TransitionHistory
    {
        get
        {
            return _transitionHistory.ToArray();
        }
    }

    /// <inheritdoc />
    public StateProcessingResult ProcessTag(XmppBase xmpp, Tag tag)
    {
        _logger.Log(LogLevel.Debug, $"Processing tag '{tag?.GetType().Name ?? "null"}' in state '{CurrentStateType.Name}'");

        try
        {
            StateProcessingResult result;

            // Check if current state implements enhanced interface
            if (CurrentState is IEnhancedState enhancedState)
            {
                result = enhancedState.ProcessTag(xmpp, tag);
            }
            else
            {
                // Fallback to legacy state execution
                CurrentState.Execute(xmpp, tag);
                result = StateProcessingResult.CreateSuccess();
            }

            // Handle state transitions if requested
            if (result.ShouldTransition && result.NextState != null)
            {
                var transitionSuccess = TransitionTo(result.NextState, result.Context);
                if (!transitionSuccess)
                {
                    return StateProcessingResult.Failure($"Failed to transition from {CurrentStateType.Name} to {result.NextState.GetType().Name}");
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Error processing tag in state {CurrentStateType.Name}: {ex.Message}");

            OnStateTransitionFailed(new StateTransitionFailedEventArgs
            {
                CurrentState = CurrentState,
                AttemptedStateType = null,
                Reason = "Exception during tag processing",
                Exception = ex,
            });

            return StateProcessingResult.Failure($"Exception during processing: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public bool TransitionTo(IState newState, string context = null)
    {
        if (newState == null)
        {
            _logger.Log(LogLevel.Warning, "Attempted to transition to null state");
            return false;
        }

        var newStateType = newState.GetType();
        _logger.Log(LogLevel.Debug, $"Attempting transition from {CurrentStateType.Name} to {newStateType.Name}");

        lock (_stateLock)
        {
            // Validate transition if current state supports it
            if (CurrentState is IEnhancedState enhancedState && !enhancedState.CanTransitionTo(newStateType))
            {
                var reason = $"Invalid transition from {CurrentStateType.Name} to {newStateType.Name}";
                _logger.Log(LogLevel.Warning, reason);

                OnStateTransitionFailed(new StateTransitionFailedEventArgs
                {
                    CurrentState = CurrentState,
                    AttemptedStateType = newStateType,
                    Reason = reason,
                });

                RecordTransition(CurrentStateType, newStateType, context, false, reason);
                return false;
            }

            try
            {
                var previousState = _currentState;

                // Call OnExit on current state if it supports it
                if (_currentState is IEnhancedState currentEnhanced)
                {
                    currentEnhanced.OnExit(null, newState); // XmppBase reference would need to be passed in
                }

                // Transition to new state
                _currentState = newState;

                // Call OnEnter on new state if it supports it
                if (newState is IEnhancedState newEnhanced)
                {
                    newEnhanced.OnEnter(null, context); // XmppBase reference would need to be passed in
                }

                _logger.Log(LogLevel.Debug, $"Successfully transitioned from {previousState.GetType().Name} to {newStateType.Name}");

                // Record successful transition
                RecordTransition(previousState.GetType(), newStateType, context, true);

                // Raise event
                OnStateTransitioned(new StateTransitionEventArgs
                {
                    FromState = previousState,
                    ToState = newState,
                    Context = context,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Exception during state transition: {ex.Message}");

                OnStateTransitionFailed(new StateTransitionFailedEventArgs
                {
                    CurrentState = CurrentState,
                    AttemptedStateType = newStateType,
                    Reason = "Exception during transition",
                    Exception = ex,
                });

                RecordTransition(CurrentStateType, newStateType, context, false, ex.Message);
                return false;
            }
        }
    }

    /// <inheritdoc />
    public bool CanTransitionTo<TState>()
        where TState : IState
    {
        var targetType = typeof(TState);

        if (CurrentState is IEnhancedState enhancedState)
        {
            return enhancedState.CanTransitionTo(targetType);
        }

        // For legacy states, we can't validate transitions
        _logger.Log(LogLevel.Debug, $"Cannot validate transition to {targetType.Name} - current state {CurrentStateType.Name} doesn't implement IEnhancedState");
        return true; // Allow any transition for backward compatibility
    }

    /// <inheritdoc />
    public void Reset()
    {
        _logger.Log(LogLevel.Debug, "Resetting state machine to initial state");

        lock (_stateLock)
        {
            var previousState = _currentState;
            _currentState = new DisconnectedState();

            RecordTransition(previousState.GetType(), typeof(DisconnectedState), "Reset", true);

            OnStateTransitioned(new StateTransitionEventArgs
            {
                FromState = previousState,
                ToState = _currentState,
                Context = "State machine reset",
            });
        }
    }

    private void RecordTransition(Type fromStateType, Type toStateType, string context, bool wasSuccessful, string errorMessage = null)
    {
        var record = new StateTransitionRecord
        {
            FromStateType = fromStateType,
            ToStateType = toStateType,
            Context = context,
            Timestamp = DateTime.UtcNow,
            WasSuccessful = wasSuccessful,
            ErrorMessage = errorMessage,
        };

        _transitionHistory.Enqueue(record);

        // Keep history size bounded
        while (_transitionHistory.Count > _maxHistorySize && _transitionHistory.TryDequeue(out _))
        {
            // Remove oldest records
        }
    }

    private void OnStateTransitioned(StateTransitionEventArgs args)
    {
        try
        {
            StateTransitioned?.Invoke(this, args);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Exception in StateTransitioned event handler: {ex.Message}");
        }
    }

    private void OnStateTransitionFailed(StateTransitionFailedEventArgs args)
    {
        try
        {
            StateTransitionFailed?.Invoke(this, args);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Exception in StateTransitionFailed event handler: {ex.Message}");
        }
    }
}
