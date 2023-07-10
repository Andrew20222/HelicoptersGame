using System;
using UnityEngine;
using System.Collections.Generic;
using HeneGames.Airplane;
using Mechanics.Movement;
using State.Enums;

namespace AirPlaneSystems
{
    [RequireComponent(typeof(Rigidbody))]
    public class AirPlaneController : MonoBehaviour, IMovable
    {
        #region Private variables

        private List<AirPlaneCollider> _airPlaneColliders = new();
        
        private float _maxSpeed = 0.6f;
        private float _currentYawSpeed;
        private float _currentPitchSpeed;
        private float _currentRollSpeed;
        private float _currentSpeed;

        private bool _planeIsDead;

        private Rigidbody _rb;
        private Runway.LandingAreas.Runway _currentRunway;
        
        #endregion

        public AirplaneState airplaneState;

        [Header("Rotating speeds")]
        [Range(5f, 500f)]
        [SerializeField] private float yawSpeed = 50f;

        [Range(5f, 500f)]
        [SerializeField] private float pitchSpeed = 100f;

        [Range(5f, 500f)]
        [SerializeField] private float rollSpeed = 200f;

        [Header("Rotating speeds multiplers when turbo is used")]
        [Range(0.1f, 5f)]
        [SerializeField] private float yawTurboMultiplier = 0.3f;

        [Range(0.1f, 5f)]
        [SerializeField] private float pitchTurboMultiplier = 0.5f;

        [Range(0.1f, 5f)]
        [SerializeField] private float rollTurboMultiplier = 1f;

        [Header("Moving speed")]
        [Range(5f, 100f)]
        [SerializeField] private float defaultSpeed = 10f;

        [Range(10f, 200f)]
        [SerializeField] private float turboSpeed = 20f;

        [Range(0.1f, 50f)]
        [SerializeField] private float accelerating = 10f;
        
        [Range(0.1f, 50f)]
        [SerializeField] private float deacelerating = 5f;

        [Header("Sideway force")]
        [Range(0.1f, 15f)]
        [SerializeField] private float sidewaysMovement = 15f;

        [Range(0.001f, 0.05f)]
        [SerializeField] private float sidewaysMovementXRot = 0.012f;

        [Range(0.1f, 5f)]
        [SerializeField] private float sidewaysMovementYRot = 1.5f;

        [Range(-1, 1f)]
        [SerializeField] private float sidewaysMovementYPos = 0.1f;

        [SerializeField] private float trailThickness = 0.045f;

        [Header("Colliders")]
        [SerializeField] private Transform crashCollidersRoot;

        [Header("Takeoff settings")]
        [Tooltip("How far must the plane be from the runway before it can be controlled again")]
        [SerializeField] private float takeoffLenght = 30f;

        public event Action TurboInput;
        public event Action NotTurboInput;
        public event Action<float> TrailEffect;

        private void Start()
        {
            //Setup speeds
            _maxSpeed = defaultSpeed;
            _currentSpeed = defaultSpeed;

            //Get and set rigidbody
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            SetupColliders(crashCollidersRoot);
        }

        private void Update()
        {

            switch (airplaneState)
            {
                case AirplaneState.Flying:
                    FlyingUpdate();
                    break;

                case AirplaneState.Landing:
                    LandingUpdate();
                    break;

                case AirplaneState.Takeoff:
                    TakeoffUpdate();
                    break;
            }
        }

        #region Flying State

        public void FlyingUpdate()
        {
            if (!_planeIsDead)
            {
                SidewaysForceCalculation();
            }
            else
            {
                TrailEffect?.Invoke(0f);
            }

            //Crash
            if (!_planeIsDead && HitSometing())
            {
                Crash();
            }
        }

        public void Move(float inputH, float inputV, bool inputYawRight, bool inputYawLeft, bool inputTurbo)
        {
            //Move forward
            transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));

            //Rotate airplane by inputs
            transform.Rotate(Vector3.forward * (-inputH * _currentRollSpeed * Time.deltaTime));
            transform.Rotate(Vector3.right * (inputV * _currentPitchSpeed * Time.deltaTime));

            //Rotate yaw
            if (inputYawRight)
            {
                transform.Rotate(Vector3.up * (_currentYawSpeed * Time.deltaTime));
            }
            else if (inputYawLeft)
            {
                transform.Rotate(-Vector3.up * (_currentYawSpeed * Time.deltaTime));
            }

            //Accelerate and deacclerate
            if (_currentSpeed < _maxSpeed)
            {
                _currentSpeed += accelerating * Time.deltaTime;
            }
            else
            {
                _currentSpeed -= deacelerating * Time.deltaTime;
            }

            //Turbo
            if (inputTurbo)
            {
                //Set speed to turbo speed and rotation to turbo values
                _maxSpeed = turboSpeed;

                _currentYawSpeed = yawSpeed * yawTurboMultiplier;
                _currentPitchSpeed = pitchSpeed * pitchTurboMultiplier;
                _currentRollSpeed = rollSpeed * rollTurboMultiplier;
                
                TrailEffect?.Invoke(trailThickness);
                TurboInput?.Invoke();
            }
            else
            {
                //Speed and rotation normal
                _maxSpeed = defaultSpeed;

                _currentYawSpeed = yawSpeed;
                _currentPitchSpeed = pitchSpeed;
                _currentRollSpeed = rollSpeed;
                
                TrailEffect?.Invoke(0f);
                NotTurboInput?.Invoke();
            }
        }

        public void SidewaysForceCalculation()
        {
            float _mutiplierXRot = sidewaysMovement * sidewaysMovementXRot;
            float _mutiplierYRot = sidewaysMovement * sidewaysMovementYRot;

            float _mutiplierYPos = sidewaysMovement * sidewaysMovementYPos;

            //Right side 
            if (transform.localEulerAngles.z > 270f && transform.localEulerAngles.z < 360f)
            {
                float _angle = (transform.localEulerAngles.z - 270f) / (360f - 270f);
                float _invert = 1f - _angle;

                transform.Rotate(Vector3.up * (_invert * _mutiplierYRot) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-_invert * _mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);

                transform.Translate(transform.up * (_invert * _mutiplierYPos) * Time.deltaTime);
            }

            //Left side
            if (transform.localEulerAngles.z > 0f && transform.localEulerAngles.z < 90f)
            {
                float _angle = transform.localEulerAngles.z / 90f;

                transform.Rotate(-Vector3.up * (_angle * _mutiplierYRot) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-_angle * _mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);

                transform.Translate(transform.up * (_angle * _mutiplierYPos) * Time.deltaTime);
            }

            //Right side down
            if (transform.localEulerAngles.z > 90f && transform.localEulerAngles.z < 180f)
            {
                float _angle = (transform.localEulerAngles.z - 90f) / (180f - 90f);
                float _invert = 1f - _angle;

                transform.Translate(transform.up * (_invert * _mutiplierYPos) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-_invert * _mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);
            }

            //Left side down
            if (transform.localEulerAngles.z > 180f && transform.localEulerAngles.z < 270f)
            {
                float _angle = (transform.localEulerAngles.z - 180f) / (270f - 180f);

                transform.Translate(transform.up * (_angle * _mutiplierYPos) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-_angle * _mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);
            }
        }

        #endregion

        #region Landing State

        public void AddLandingRunway(Runway.LandingAreas.Runway _landingThisRunway)
        {
            _currentRunway = _landingThisRunway;
        }

        public void LandingUpdate()
        {
            TrailEffect?.Invoke(0f); 

            //Stop speed
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime);

            //Set local rotation to zero
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f,0f,0f), 2f * Time.deltaTime);
        }

        #endregion

        #region Takeoff State

        public void TakeoffUpdate()
        {
            //Reset colliders
            foreach (AirPlaneCollider _airPlaneCollider in _airPlaneColliders)
            {
                _airPlaneCollider.CollideSomething = false;
            }

            //Accelerate
            if (_currentSpeed < turboSpeed)
            {
                _currentSpeed += (accelerating * 2f) * Time.deltaTime;
            }

            //Move forward
            transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));
 
            //Far enough from the runaway go back to flying state
            float _distanceToRunway = Vector3.Distance(transform.position, _currentRunway.transform.position);
            if(_distanceToRunway > takeoffLenght)
            {
                _currentRunway = null;
                airplaneState = AirplaneState.Flying;
            }
        }

        #endregion

        #region Private methods

        private void SetupColliders(Transform _root)
        {
            if (_root == null)
                return;

            //Get colliders from root transform
            Collider[] colliders = _root.GetComponentsInChildren<Collider>();

            //If there are colliders put components in them
            for (int i = 0; i < colliders.Length; i++)
            {
                //Change collider to trigger
                colliders[i].isTrigger = true;

                GameObject _currentObject = colliders[i].gameObject;

                //Add airplane collider to it and put it on the list
                AirPlaneCollider _airplaneCollider = _currentObject.AddComponent<AirPlaneCollider>();
                _airPlaneColliders.Add(_airplaneCollider);

                //Add airplane conroller reference to collider
                _airplaneCollider.controller = this;

                //Add rigid body to it
                Rigidbody _rb = _currentObject.AddComponent<Rigidbody>();
                _rb.useGravity = false;
                _rb.isKinematic = true;
                _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }
        
        private bool HitSometing()
        {
            for (int i = 0; i < _airPlaneColliders.Count; i++)
            {
                if (_airPlaneColliders[i].CollideSomething)
                {
                    //Reset colliders
                    foreach(AirPlaneCollider _airPlaneCollider in _airPlaneColliders)
                    {
                        _airPlaneCollider.CollideSomething = false;
                    }
                    return true;
                }
            }

            return false;
        }

        private void Crash()
        {
            //Set rigidbody to non cinematic
            _rb.isKinematic = false;
            _rb.useGravity = true;

            //Change every collider trigger state and remove rigidbodys
            for (int i = 0; i < _airPlaneColliders.Count; i++)
            {
                _airPlaneColliders[i].GetComponent<Collider>().isTrigger = false;
                Destroy(_airPlaneColliders[i].GetComponent<Rigidbody>());
            }

            //Kill player
            _planeIsDead = true;
        }

        #endregion

        #region Variables

        /// <summary>
        /// Returns a percentage of how fast the current speed is from the maximum speed between 0 and 1
        /// </summary>
        /// <returns></returns>
        public float PercentToMaxSpeed()
        {
            float _percentToMax = _currentSpeed / turboSpeed;

            return _percentToMax;
        }

        public bool PlaneIsDead()
        {
            return _planeIsDead;
        }

        public bool UsingTurbo()
        {
            if(_maxSpeed == turboSpeed)
            {
                return true;
            }

            return false;
        }

        public float CurrentSpeed()
        {
            return _currentSpeed;
        }

        #endregion
    }
}