
using AirPlaneSystems;

namespace StateMachines
{
    public class TakeoffState : State
    {
        private AirPlaneController _player;

        public TakeoffState(AirPlaneController player)
        {
            _player = player;
        }
        public override void Enter()
        {
            _player.TakeoffUpdate();
        }

        public override void Exit()
        {
            //
        }
    }
}