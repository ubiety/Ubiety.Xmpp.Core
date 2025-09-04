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

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Record of a state transition for debugging and auditing.
/// </summary>
public record StateTransitionRecord
{
    /// <summary>
    /// Gets or sets the type of the state being transitioned from.
    /// </summary>
    public Type FromStateType { get; init; }

    /// <summary>
    /// Gets or sets the type of the state being transitioned to.
    /// </summary>
    public Type ToStateType { get; init; }

    /// <summary>
    /// Gets or sets additional context about the transition.
    /// </summary>
    public string Context { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when the transition occurred.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the transition was successful.
    /// </summary>
    public bool WasSuccessful { get; init; }

    /// <summary>
    /// Gets or sets the error message if the transition failed.
    /// </summary>
    public string ErrorMessage { get; init; }
}
