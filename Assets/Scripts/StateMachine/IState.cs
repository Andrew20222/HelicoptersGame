namespace StateMachines
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
    }
}