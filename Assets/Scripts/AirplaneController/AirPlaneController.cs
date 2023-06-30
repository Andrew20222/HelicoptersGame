using System.Collections.Generic;
using HeneGames.Airplane;
using StateMachine;
using StateMachines;
using UnityEngine;
using UnityEngine.Serialization;

namespace HeheGames.Simple_Airplane_Controller
{
    [RequireComponent(typeof(Rigidbody))]
    public class AirPlaneController : MonoBehaviour, IMovable, IAudioSystem
    {

        #region Private variables
        private float _maxSpeed = 0.6f;
        private float _currentYawSpeed;
        private float _currentPitchSpeed;
        private float _currentRollSpeed;
        private float _currentSpeed;
        private float _currentEngineLightIntensity;
        private float _currentEngineSoundPitch;

        private bool _planeIsDead;

        private Rigidbody _rb;
        private Runway currentRunway;
        private List<SimpleAirPlaneCollider> airPlaneColliders = new();

        //Input variables
        private float _inputH;
        private float _inputV;
        private bool _inputTurbo;
        private bool _inputYawLeft;
        private bool _inputYawRight;

        #endregion

        [Header("Wing trail effects")]
        [Range(0.01f, 1f)]
        [SerializeField] private float trailThickness = 0.045f;
        [SerializeField] private TrailRenderer[] wingTrailEffects;

        [Header("Rotating speeds")]
        [Range(5f, 500f)]
        [SerializeField] private float yawSpeed = 50f;

        [Range(5f, 500f)]
        [SerializeField] private float pitchSpeed = 100f;

        [Range(5f, 500f)]
        [SerializeField] private float rollSpeed = 200f;

        [Header("Rotating speeds multiples when turbo is used")]
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
        [SerializeField] private float deaccelerating = 5f;

        [Header("Sideway force")]
        [Range(0.1f, 15f)]
        [SerializeField] private float sidewaysMovement = 15f;

        [Range(0.001f, 0.05f)]
        [SerializeField] private float sidewaysMovementXRot = 0.012f;

        [Range(0.1f, 5f)]
        [SerializeField] private float sidewaysMovementYRot = 1.5f;

        [Range(-1, 1f)]
        [SerializeField] private float sidewaysMovementYPos = 0.1f;
        
        [Header("Engine propellers settings")]
        [Range(10f, 10000f)]
        [SerializeField] private float propelSpeedMultiplier = 100f;

        [SerializeField] private GameObject[] propellers;

        [Header("Turbine light settings")]
        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightDefault = 1f;

        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightTurbo = 5f;

        [SerializeField] private Light[] turbineLights;

        [Header("Colliders")]
        [SerializeField] private Transform crashCollidersRoot;
        
        [Header("Takeoff settings")]
        [Tooltip("How far must the plane be from the runway before it can be controlled again")]
        [SerializeField] private float takeoffLenght = 30f;

        //feature times
        public AirStateMachine _SM;
        private FlyState _flyState;
        private TakeoffState _takeoffState;
        private LandState _landState;
        public State currentState;

        private void Awake()
        {
            _SM = new AirStateMachine();
            _flyState = new FlyState(this);
            _takeoffState = new TakeoffState(this);
            _landState = new LandState(this);
            _SM.Initialize(_landState);
            
            Debug.Log(_SM.CurrentState);
            
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
            if (Input.GetKey(KeyCode.W))
            {
                _SM.ChangeState(_flyState);
                currentState = _flyState;
            }
            else
            {
                _SM.ChangeState(_landState);
                currentState = _landState;
            }

            if (Input.GetKey(KeyCode.A))
            {
                currentState = _takeoffState;
            }
            
            Debug.Log(_SM.CurrentState);
        }

        #region Flying State
        
        public void FlyingUpdate()
        {
            UpdatePropellersAndLights();

            //Airplane move only if not dead
            if (!_planeIsDead)
            {
                SidewaysForceCalculation();
            }
            else
            {
                ChangeWingTrailEffectThickness(0f);
            }


            if (!_planeIsDead && HitSometing())
            {
                Crash();
            }
        }

        public void SidewaysForceCalculation()
        {
            float mutiplierXRot = sidewaysMovement * sidewaysMovementXRot;
            float mutiplierYRot = sidewaysMovement * sidewaysMovementYRot;
            float mutiplierYPos = sidewaysMovement * sidewaysMovementYPos;

            //Right side 
            if (transform.localEulerAngles.z > 270f && transform.localEulerAngles.z < 360f)
            {
                float angle = (transform.localEulerAngles.z - 270f) / (360f - 270f);
                float invert = 1f - angle;

                transform.Rotate(Vector3.up * (invert * mutiplierYRot) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-invert * mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);

                transform.Translate(transform.up * (invert * mutiplierYPos) * Time.deltaTime);
            }

            //Left side
            if (transform.localEulerAngles.z > 0f && transform.localEulerAngles.z < 90f)
            {
                float _angle = transform.localEulerAngles.z / 90f;

                transform.Rotate(-Vector3.up * (_angle * mutiplierYRot) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-_angle * mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);

                transform.Translate(transform.up * (_angle * mutiplierYPos) * Time.deltaTime);
            }

            //Right side down
            if (transform.localEulerAngles.z > 90f && transform.localEulerAngles.z < 180f)
            {
                float angle = (transform.localEulerAngles.z - 90f) / (180f - 90f);
                float invert = 1f - angle;

                transform.Translate(transform.up * (invert * mutiplierYPos) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-invert * mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);
            }

            //Left side down
            if (transform.localEulerAngles.z > 180f && transform.localEulerAngles.z < 270f)
            {
                float angle = (transform.localEulerAngles.z - 180f) / (270f - 180f);

                transform.Translate(transform.up * (angle * mutiplierYPos) * Time.deltaTime);
                transform.Rotate(Vector3.right * (-angle * mutiplierXRot) * _currentPitchSpeed * Time.deltaTime);
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
                transform.Rotate(Vector3.up * (-_currentYawSpeed * Time.deltaTime));
            }

            //Accelerate and decelerate
            if (_currentSpeed < _maxSpeed)
            {
                _currentSpeed += accelerating * Time.deltaTime;
            }
            else
            {
                _currentSpeed -= deaccelerating * Time.deltaTime;
            }

            //Turbo
            if (inputTurbo)
            {
                //Set speed to turbo speed and rotation to turbo values
                _maxSpeed = turboSpeed;

                _currentYawSpeed = yawSpeed * yawTurboMultiplier;
                _currentPitchSpeed = pitchSpeed * pitchTurboMultiplier;
                _currentRollSpeed = rollSpeed * rollTurboMultiplier;

                //Engine lights
                _currentEngineLightIntensity = turbineLightTurbo;

                //Effects
                ChangeWingTrailEffectThickness(trailThickness);
            }
            else
            {
                //Speed and rotation normal
                _maxSpeed = defaultSpeed;

                _currentYawSpeed = yawSpeed;
                _currentPitchSpeed = pitchSpeed;
                _currentRollSpeed = rollSpeed;

                //Engine lights
                _currentEngineLightIntensity = turbineLightDefault;

                //Effects
                ChangeWingTrailEffectThickness(0f);
            }
        }

        #endregion

        #region Landing State
        
        public void AddLandingRunway(Runway _landingThisRunway)
        {
            currentRunway = _landingThisRunway;
        }
        public void LandingUpdate()
        {
            UpdatePropellersAndLights();

            ChangeWingTrailEffectThickness(0f);

            //Stop speed
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime);

            //Set local rotation to zero
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f,0f,0f), 2f * Time.deltaTime);
        }

        #endregion

        #region Takeoff State

        public void TakeoffUpdate()
        {
            UpdatePropellersAndLights();

            //Reset colliders
            foreach (SimpleAirPlaneCollider airPlaneCollider in airPlaneColliders)
            {
                airPlaneCollider.collideSometing = false;
            }

            //Accelerate
            if (_currentSpeed < turboSpeed)
            {
                _currentSpeed += (accelerating * 2f) * Time.deltaTime;
            }

            //Move forward
            transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));
            
            float _distanceToRunway = Vector3.Distance(transform.position, currentRunway.transform.position);
            if(_distanceToRunway > takeoffLenght)
            {
                currentRunway = null;
                currentState = _takeoffState;
            }
        }

        #endregion

        #region Audio
        public void AudioSetVolumeWithState(AirplaneStateB airplaneStateB, AudioSource engineSoundSource, float defaultSoundPitch, float maxEngineSound, float turboSoundPitch)
        {
            if (engineSoundSource == null)
                return;

            if (currentState == _flyState)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, defaultSoundPitch, 10f * Time.deltaTime);

                if (_planeIsDead)
                {
                    engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, 0f, 10f * Time.deltaTime);
                }
                else
                {
                    engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, maxEngineSound, 1f * Time.deltaTime);
                }
            }
            else if (currentState == _landState)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, defaultSoundPitch, 1f * Time.deltaTime);
                engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, 0f, 1f * Time.deltaTime);
            }
            else if (currentState == _takeoffState)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, turboSoundPitch, 1f * Time.deltaTime);
                engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, maxEngineSound, 1f * Time.deltaTime);
            }
        }

        #endregion

        #region Private methods

        private void UpdatePropellersAndLights()
        {
            if(!_planeIsDead)
            {
                //Rotate propellers if any
                if (propellers.Length > 0)
                {
                    RotatePropellers(propellers, _currentSpeed * propelSpeedMultiplier);
                }

                //Control lights if any
                if (turbineLights.Length > 0)
                {
                    ControlEngineLights(turbineLights, _currentEngineLightIntensity);
                }
            }
            else
            {
                //Rotate propellers if any
                if (propellers.Length > 0)
                {
                    RotatePropellers(propellers, 0f);
                }

                //Control lights if any
                if (turbineLights.Length > 0)
                {
                    ControlEngineLights(turbineLights, 0f);
                }
            }
        }

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
                SimpleAirPlaneCollider _airplaneCollider = _currentObject.AddComponent<SimpleAirPlaneCollider>();
                airPlaneColliders.Add(_airplaneCollider);

                //Add airplane conroller reference to collider
                // _airplaneCollider.controller = this;

                //Add rigid body to it
                Rigidbody _rb = _currentObject.AddComponent<Rigidbody>();
                _rb.useGravity = false;
                _rb.isKinematic = true;
                _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }

        private void RotatePropellers(GameObject[] rotateThese, float speed)
        {
            for (int i = 0; i < rotateThese.Length; i++)
            {
                rotateThese[i].transform.Rotate(Vector3.forward * (-speed * Time.deltaTime));
            }
        }

        private void ControlEngineLights(Light[] lights, float intensity)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if(!_planeIsDead)
                {
                    lights[i].intensity = Mathf.Lerp(lights[i].intensity, intensity, 10f * Time.deltaTime);
                }
                else
                {
                    lights[i].intensity = Mathf.Lerp(lights[i].intensity, 0f, 10f * Time.deltaTime);
                }
               
            }
        }

        private void ChangeWingTrailEffectThickness(float thickness)
        {
            for (int i = 0; i < wingTrailEffects.Length; i++)
            {
                wingTrailEffects[i].startWidth = Mathf.Lerp(wingTrailEffects[i].startWidth, thickness, Time.deltaTime * 10f);
            }
        }

        private bool HitSometing()
        {
            for (int i = 0; i < airPlaneColliders.Count; i++)
            {
                if (airPlaneColliders[i].collideSometing)
                {
                    //Reset colliders
                    foreach(SimpleAirPlaneCollider _airPlaneCollider in airPlaneColliders)
                    {
                        _airPlaneCollider.collideSometing = false;
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
            for (int i = 0; i < airPlaneColliders.Count; i++)
            {
                airPlaneColliders[i].GetComponent<Collider>().isTrigger = false;
                Destroy(airPlaneColliders[i].GetComponent<Rigidbody>());
            }

            //Kill player
            _planeIsDead = true;

            //Here you can add your own code...
        }

        #endregion

        #region Variables

        /// <summary>
        /// Returns a percentage of how fast the current speed is from the maximum speed between 0 and 1
        /// </summary>
        /// <returns></returns>
        public float PercentToMaxSpeed()
        {
            float percentToMax;
            percentToMax = _currentSpeed / turboSpeed;

            return percentToMax;
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