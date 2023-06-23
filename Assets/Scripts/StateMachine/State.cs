namespace StateMachine
{
    public abstract class State: IState
    {
        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update() { }
    }
}
