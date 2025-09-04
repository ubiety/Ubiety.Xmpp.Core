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

using FluentAssertions;
using Ubiety.Xmpp.Core.Infrastructure.StateMachine;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags;
using Xunit;

namespace Ubiety.Xmpp.Test.Infrastructure.StateMachine;

/// <summary>
/// Tests for the XMPP state machine coordinator functionality.
/// </summary>
public class StateMachineCoordinatorTests
{
    /// <summary>
    /// Test state that implements enhanced state interface for testing.
    /// </summary>
    private class TestEnhancedState : EnhancedStateBase
    {
        public override string StateName { get; }
        protected override HashSet<Type> ValidTransitions { get; }

        public TestEnhancedState(string name, params Type[] validTransitions)
        {
            StateName = name;
            ValidTransitions = new HashSet<Type>(validTransitions);
        }

        public override StateProcessingResult ProcessTag(Common.XmppBase xmpp, Tag tag = null)
        {
            // Simple test implementation
            return Success();
        }
    }

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultDisconnectedState()
    {
        // Arrange & Act
        var coordinator = new XmppStateMachineCoordinator();

        // Assert
        coordinator.CurrentState.Should().BeOfType<DisconnectedState>();
        coordinator.CurrentStateType.Should().Be(typeof(DisconnectedState));
        coordinator.TransitionHistory.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldAcceptCustomInitialState()
    {
        // Arrange
        var initialState = new ConnectingState();

        // Act
        var coordinator = new XmppStateMachineCoordinator(initialState);

        // Assert
        coordinator.CurrentState.Should().BeSameAs(initialState);
        coordinator.CurrentStateType.Should().Be(typeof(ConnectingState));
    }

    [Fact]
    public void TransitionTo_ShouldSucceedForValidTransition()
    {
        // Arrange
        var coordinator = new XmppStateMachineCoordinator();
        var newState = new ConnectingState();
        var transitionFired = false;
        
        coordinator.StateTransitioned += (_, _) => transitionFired = true;

        // Act
        var result = coordinator.TransitionTo(newState, "Test transition");

        // Assert
        result.Should().BeTrue();
        coordinator.CurrentState.Should().BeSameAs(newState);
        coordinator.CurrentStateType.Should().Be(typeof(ConnectingState));
        transitionFired.Should().BeTrue();
        coordinator.TransitionHistory.Should().HaveCount(1);
        coordinator.TransitionHistory[0].ToStateType.Should().Be(typeof(ConnectingState));
        coordinator.TransitionHistory[0].Context.Should().Be("Test transition");
        coordinator.TransitionHistory[0].WasSuccessful.Should().BeTrue();
    }

    [Fact]
    public void TransitionTo_ShouldFailForInvalidTransition()
    {
        // Arrange
        var validState = new TestEnhancedState("Valid", typeof(ConnectingState));
        var invalidState = new DisconnectedState();
        var coordinator = new XmppStateMachineCoordinator(validState);
        var failureFired = false;

        coordinator.StateTransitionFailed += (_, _) => failureFired = true;

        // Act
        var result = coordinator.TransitionTo(invalidState, "Invalid transition");

        // Assert
        result.Should().BeFalse();
        coordinator.CurrentState.Should().BeSameAs(validState);
        failureFired.Should().BeTrue();
        coordinator.TransitionHistory.Should().HaveCount(1);
        coordinator.TransitionHistory[0].WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void TransitionTo_ShouldFailForNullState()
    {
        // Arrange
        var coordinator = new XmppStateMachineCoordinator();

        // Act
        var result = coordinator.TransitionTo(null);

        // Assert
        result.Should().BeFalse();
        coordinator.CurrentState.Should().BeOfType<DisconnectedState>();
    }

    [Fact]
    public void CanTransitionTo_ShouldReturnCorrectValidation()
    {
        // Arrange
        var state = new TestEnhancedState("Test", typeof(ConnectingState), typeof(ConnectedState));
        var coordinator = new XmppStateMachineCoordinator(state);

        // Act & Assert
        coordinator.CanTransitionTo<ConnectingState>().Should().BeTrue();
        coordinator.CanTransitionTo<ConnectedState>().Should().BeTrue();
        coordinator.CanTransitionTo<DisconnectedState>().Should().BeFalse();
    }

    [Fact]
    public void CanTransitionTo_ShouldReturnTrueForLegacyStates()
    {
        // Arrange - using a legacy state that doesn't implement IEnhancedState
        var coordinator = new XmppStateMachineCoordinator(new ConnectingState());

        // Act & Assert - legacy states allow any transition for backward compatibility
        coordinator.CanTransitionTo<ConnectedState>().Should().BeTrue();
        coordinator.CanTransitionTo<DisconnectedState>().Should().BeTrue();
    }

    [Fact]
    public void ProcessTag_WithEnhancedState_ShouldHandleSuccess()
    {
        // Arrange
        var state = new TestEnhancedState("Test", typeof(ConnectingState));
        var coordinator = new XmppStateMachineCoordinator(state);

        // Act
        var result = coordinator.ProcessTag(null, null);

        // Assert
        result.Success.Should().BeTrue();
        result.ShouldTransition.Should().BeFalse();
    }

    [Fact]
    public void ProcessTag_WithTransitionRequest_ShouldPerformTransition()
    {
        // Arrange
        var nextState = new ConnectingState();
        var state = new TestTransitionState(nextState);
        var coordinator = new XmppStateMachineCoordinator(state);
        var transitionFired = false;

        coordinator.StateTransitioned += (_, _) => transitionFired = true;

        // Act
        var result = coordinator.ProcessTag(null, null);

        // Assert
        result.Success.Should().BeTrue();
        coordinator.CurrentState.Should().BeSameAs(nextState);
        transitionFired.Should().BeTrue();
    }

    [Fact]
    public void Reset_ShouldTransitionToDisconnectedState()
    {
        // Arrange
        var coordinator = new XmppStateMachineCoordinator(new ConnectingState());
        var transitionFired = false;

        coordinator.StateTransitioned += (_, e) =>
        {
            transitionFired = true;
            e.Context.Should().Be("State machine reset");
        };

        // Act
        coordinator.Reset();

        // Assert
        coordinator.CurrentState.Should().BeOfType<DisconnectedState>();
        transitionFired.Should().BeTrue();
        coordinator.TransitionHistory.Should().HaveCount(1);
        coordinator.TransitionHistory[0].Context.Should().Be("Reset");
    }

    [Fact]
    public void TransitionHistory_ShouldBeBounded()
    {
        // Arrange
        var coordinator = new XmppStateMachineCoordinator(maxHistorySize: 2);

        // Act - perform more transitions than the max history size
        coordinator.TransitionTo(new ConnectingState(), "First");
        coordinator.TransitionTo(new ConnectedState(), "Second");
        coordinator.TransitionTo(new DisconnectedState(), "Third");

        // Assert - history should only contain the most recent transitions
        coordinator.TransitionHistory.Should().HaveCount(2);
        coordinator.TransitionHistory[0].Context.Should().Be("Second");
        coordinator.TransitionHistory[1].Context.Should().Be("Third");
    }

    [Fact]
    public void StateTransitionEvents_ShouldProvideCorrectInformation()
    {
        // Arrange
        var coordinator = new XmppStateMachineCoordinator();
        StateTransitionEventArgs capturedArgs = null;

        coordinator.StateTransitioned += (_, args) => capturedArgs = args;

        // Act
        coordinator.TransitionTo(new ConnectingState(), "Test event");

        // Assert
        capturedArgs.Should().NotBeNull();
        capturedArgs.FromState.Should().BeOfType<DisconnectedState>();
        capturedArgs.ToState.Should().BeOfType<ConnectingState>();
        capturedArgs.Context.Should().Be("Test event");
        capturedArgs.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Helper test state that always requests a transition.
    /// </summary>
    private class TestTransitionState : EnhancedStateBase
    {
        private readonly IState _nextState;

        public TestTransitionState(IState nextState)
        {
            _nextState = nextState;
        }

        public override string StateName => "TestTransition";
        protected override HashSet<Type> ValidTransitions => new() { _nextState.GetType() };

        public override StateProcessingResult ProcessTag(Common.XmppBase xmpp, Tag tag = null)
        {
            return SuccessWithTransition(_nextState, "Test transition");
        }
    }
}

/// <summary>
/// Integration tests showing how the coordinator works with enhanced states.
/// </summary>
public class StateMachineIntegrationTests
{
    [Fact]
    public void EnhancedState_OnEnterAndOnExit_ShouldBeCalled()
    {
        // Arrange
        var initialState = new TrackingEnhancedState("Initial");
        var nextState = new TrackingEnhancedState("Next");
        var coordinator = new XmppStateMachineCoordinator(initialState);

        // Act
        coordinator.TransitionTo(nextState, "Integration test");

        // Assert
        initialState.OnExitCalled.Should().BeTrue();
        nextState.OnEnterCalled.Should().BeTrue();
        nextState.EnterContext.Should().Be("Integration test");
    }

    /// <summary>
    /// Enhanced state that tracks method calls for testing.
    /// </summary>
    private class TrackingEnhancedState : EnhancedStateBase
    {
        public bool OnEnterCalled { get; private set; }
        public bool OnExitCalled { get; private set; }
        public string EnterContext { get; private set; }

        public TrackingEnhancedState(string name)
        {
            StateName = name;
        }

        public override string StateName { get; }
        protected override HashSet<Type> ValidTransitions => new() { typeof(TrackingEnhancedState), typeof(DisconnectedState) };

        public override StateProcessingResult ProcessTag(Common.XmppBase xmpp, Tag tag = null)
        {
            return Success();
        }

        public override void OnEnter(Common.XmppBase xmpp, string context = null)
        {
            OnEnterCalled = true;
            EnterContext = context;
        }

        public override void OnExit(Common.XmppBase xmpp, IState nextState = null)
        {
            OnExitCalled = true;
        }
    }
}
