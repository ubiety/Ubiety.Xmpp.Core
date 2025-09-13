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
/// Event arguments for state transitions.
/// </summary>
public class StateTransitionEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the state being transitioned from.
    /// </summary>
    public IState FromState { get; init; }

    /// <summary>
    /// Gets or sets the state being transitioned to.
    /// </summary>
    public IState ToState { get; init; }

    /// <summary>
    /// Gets or sets additional context about the transition.
    /// </summary>
    public string Context { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when the transition occurred.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
