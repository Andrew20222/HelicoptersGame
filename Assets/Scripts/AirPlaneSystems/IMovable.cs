using State.Enums;

namespace Mechanics.Movement
{
    public interface IMovable
    {
        void Move(float inputH, float inputV, bool inputYawRight, bool inputYawLeft, bool inputTurbo);
        void SidewaysForceCalculation();
        bool PlaneIsDead();
        AirplaneState AirplaneState { get; }
    }
}

