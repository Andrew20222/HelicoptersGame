using HeheGames.Simple_Airplane_Controller;

namespace StateMachine
{
    public class FlyState : State
    {
        private AirPlaneController _player;
        public FlyState(AirPlaneController player)
        {
            _player = player;
        }
        public override void Enter()
        {
            _player.FlyingUpdate();
        }

        public override void Update()
        {
            _player.FlyingUpdate();
        }

        public override void Exit()
        {
            //
        }
    }
}