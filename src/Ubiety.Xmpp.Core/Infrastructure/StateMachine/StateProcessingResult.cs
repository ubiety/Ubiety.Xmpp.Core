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

using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core.Infrastructure.StateMachine;

/// <summary>
/// Represents the result of state processing.
/// </summary>
public record StateProcessingResult
{
    /// <summary>
    /// Indicates if the processing was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Error message if processing failed.
    /// </summary>
    public string ErrorMessage { get; init; }

    /// <summary>
    /// Indicates if a state transition should occur.
    /// </summary>
    public bool ShouldTransition { get; init; }

    /// <summary>
    /// The state to transition to, if any.
    /// </summary>
    public IState NextState { get; init; }

    /// <summary>
    /// Additional context about the processing result.
    /// </summary>
    public string Context { get; init; }

    /// <summary>
    /// Creates a successful result without transition.
    /// </summary>
    /// <returns>A successful state processing result.</returns>
    public static StateProcessingResult CreateSuccess() => new() { Success = true };

    /// <summary>
    /// Creates a successful result with a state transition.
    /// </summary>
    /// <param name="nextState">The next state to transition to.</param>
    /// <param name="context">Optional context for the transition.</param>
    /// <returns>A successful state processing result with transition.</returns>
    public static StateProcessingResult SuccessWithTransition(IState nextState, string? context = null) =>
        new() { Success = true, ShouldTransition = true, NextState = nextState, Context = context };

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>A failed state processing result.</returns>
    public static StateProcessingResult Failure(string error) => new() { Success = false, ErrorMessage = error };
}
