using System;
using StateMachine;

namespace StateMachines
{
    public class AirStateMachine 
    {
        public State CurrentState { get; private set; }

        public void Initialize(State state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }
        
        public void ChangeState(State newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}