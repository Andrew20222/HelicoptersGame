
namespace StateMachines
{
    public class StateMachine 
    {
        public State CurrentState { get; set; }

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