using UnityEngine;

public interface IMovable
{
    void Move(float inputH, float inputV, bool inputYawRight, bool inputYawLeft, bool inputTurbo);
    void SidewaysForceCalculation();
}
