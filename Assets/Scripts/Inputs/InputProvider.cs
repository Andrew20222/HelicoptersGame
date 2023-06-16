using System;
using UnityEngine;
using Zenject;

namespace Inputs
{
    public class InputProvider : MonoBehaviour
    {
        private float _inputH;
        private float _inputV;
        private bool _inputYawLeft;
        private bool _inputYawRight;
        private bool _inputTurbo;
        private IMovable _movable;

        [Inject]
        private void Construct(IMovable movable)
        {
            _movable = movable;
        }

        private void Update()
        {
            //Rotate inputs
            _inputH = Input.GetAxis("Horizontal");
            _inputV = Input.GetAxis("Vertical");

            //Yaw axis inputs
            _inputYawLeft = Input.GetKey(KeyCode.Q);
            _inputYawRight = Input.GetKey(KeyCode.E);

            //Turbo
            _inputTurbo = Input.GetKey(KeyCode.LeftShift);

            SetValueInputs();
        }

        private void SetValueInputs()
        {
            _movable.Move(_inputH, _inputV, _inputYawRight, _inputYawLeft, _inputTurbo);
        }
    }
}
