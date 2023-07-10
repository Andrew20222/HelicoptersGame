using AirPlaneSystems;
using LandingSystem.Areas;
using State.Enums;
using UnityEngine;

namespace Runway.LandingAreas
{
    public class Runway : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private KeyCode launchKey = KeyCode.Space;

        [Header("Runway references")]
        public string runwayName = "Runway";

        public Transform landingAdjuster;
        [SerializeField] private Transform landingfinalPos;

        private bool _landingCompleted;
        private float _landingSpeed;
        private AirPlaneController _landingAirplaneController;
        private Vector3 _landingAdjusterStartLocalPos;

        private void Start()
        {
            _landingSpeed = 1f;
            _landingAdjusterStartLocalPos = landingAdjuster.localPosition;
        }

        private void Update()
        {
            if(_landingAirplaneController != null)
            {
                _landingAirplaneController.transform.SetParent(landingAdjuster.transform);
                
                if(!_landingCompleted)
                {
                    _landingSpeed += Time.deltaTime;
                    landingAdjuster.localPosition = Vector3.Lerp(landingAdjuster.localPosition, landingfinalPos.localPosition, _landingSpeed * Time.deltaTime);

                    float _distanceToLandingFinalPos = Vector3.Distance(landingAdjuster.position, landingfinalPos.position);
                    if (_distanceToLandingFinalPos < 0.1f)
                    {
                        _landingCompleted = true;
                    }
                }
                else
                {
                    landingAdjuster.localPosition = Vector3.Lerp(landingAdjuster.localPosition, landingfinalPos.localPosition, _landingSpeed * Time.deltaTime);
                    
                    if (Input.GetKeyDown(launchKey))
                    {
                        _landingAirplaneController.airplaneState = AirplaneState.Takeoff;
                    }
                    
                    if (_landingAirplaneController.airplaneState == AirplaneState.Flying)
                    {
                        _landingAirplaneController.transform.SetParent(null);
                        _landingAirplaneController = null;
                        _landingCompleted = false;
                        _landingSpeed = 1f;
                        landingAdjuster.localPosition = _landingAdjusterStartLocalPos;
                    }
                }
            }
        }
        
        public void AddAirplane(AirPlaneController airPlane)
        {
            _landingAirplaneController = airPlane;
        }

        public bool AirplaneLandingCompleted()
        {
            if (_landingAirplaneController != null)
            {
                if (_landingAirplaneController.airplaneState != AirplaneState.Takeoff)
                {
                    return _landingCompleted;
                }
            }

            return false;
        }

        public bool AirplaneIsLanding()
        {
            if(_landingAirplaneController != null && !_landingCompleted)
            {
                return true;
            }

            return false;
        }

        public bool AirplaneIsTakingOff()
        {
            if (_landingAirplaneController != null)
            {
                if(_landingAirplaneController.airplaneState == AirplaneState.Takeoff)
                {
                    return true;
                }
            }

            return false;
        }
    }
}