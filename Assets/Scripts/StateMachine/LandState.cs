
using AirPlaneSystems;

namespace StateMachines
{
    public class LandState : State
    {
        private AirPlaneController _player;

        public LandState(AirPlaneController player)
        {
            _player = player;
        }
        public override void Enter()
        {
            _player.LandingUpdate();
        }

        public override void Exit()
        {
           //
        }
    }
}