using AirPlaneSystems;
using HeneGames.Airplane;
using State.Enums;
using UnityEngine;

namespace Inputs
{
    public class InputProvider : MonoBehaviour
    {
        [SerializeField] private AirPlaneController _movable;
        private float _inputH;
        private float _inputV;
        private bool _inputYawLeft;
        private bool _inputYawRight;
        private bool _inputTurbo;

        private void Update()
        {
            _inputH = Input.GetAxis("Horizontal");
            _inputV = Input.GetAxis("Vertical");
            
            _inputYawLeft = Input.GetKey(KeyCode.Q);
            _inputYawRight = Input.GetKey(KeyCode.E);

            _inputTurbo = Input.GetKey(KeyCode.LeftShift);

            SetValueInputs();
        }

        private void SetValueInputs()
        {
            if(_movable.airplaneState == AirplaneState.Landing) return;
            
            if(!_movable.PlaneIsDead())
                _movable.Move(_inputH, _inputV, _inputYawRight, _inputYawLeft, _inputTurbo);
        }
    }
}
