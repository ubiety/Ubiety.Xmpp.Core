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
using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Event arguments for failed state transitions.
/// </summary>
public class StateTransitionFailedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the current state when the transition failed.
    /// </summary>
    public IState CurrentState { get; init; }

    /// <summary>
    /// Gets or sets the type of state that was attempted to transition to.
    /// </summary>
    public Type AttemptedStateType { get; init; }

    /// <summary>
    /// Gets or sets the reason for the transition failure.
    /// </summary>
    public string Reason { get; init; }

    /// <summary>
    /// Gets or sets the exception that caused the failure, if any.
    /// </summary>
    public Exception Exception { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when the failure occurred.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
