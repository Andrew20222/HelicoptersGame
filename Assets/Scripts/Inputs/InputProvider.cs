using Mechanics.Movement;
using UnityEngine;

namespace Inputs
{
    public class InputProvider : MonoBehaviour
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";
        private IMovable _movable;
        
        private float _inputH;
        private float _inputV;
        private bool _inputYawLeft;
        private bool _inputYawRight;
        private bool _inputTurbo;
        
        public void Initialize(IMovable movable)
        {
            _movable = movable;
        }
        private void Update()
        {
            _inputH = Input.GetAxis(HORIZONTAL_AXIS_NAME);
            _inputV = Input.GetAxis(VERTICAL_AXIS_NAME);
            
            _inputYawLeft = Input.GetKey(KeyCode.Q);
            _inputYawRight = Input.GetKey(KeyCode.E);
            _inputTurbo = Input.GetKey(KeyCode.LeftShift);

            SetValueInputs();
        }

        private void SetValueInputs()
        {
            if(!_movable.PlaneIsDead())
                _movable.Move(_inputH, _inputV, _inputYawRight, _inputYawLeft, _inputTurbo);
        }
    }
}
