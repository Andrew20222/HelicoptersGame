using AirPlaneSystems;
using State.Enums;
using UnityEngine;

namespace LandingSystem.Areas
{
    public class LandingArea : MonoBehaviour
    {
        [SerializeField] private Runway.LandingAreas.Runway runway;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<AirPlaneCollider>(out AirPlaneCollider _airPlaneCollider))
            {
                Vector3 dirFromLandingAreaToPlayerPlane = (transform.position - _airPlaneCollider.transform.position).normalized;
                float _directionFloat = Vector3.Dot(transform.forward, dirFromLandingAreaToPlayerPlane);
                
                if (_directionFloat > 0.5f)
                {
                    AirPlaneController _controller = _airPlaneCollider.controller;

                    runway.landingAdjuster.position = _controller.transform.position;

                    runway.AddAirplane(_controller);
                    _controller.airplaneState = AirplaneState.Landing;
                    _controller.AddLandingRunway(runway);
                }
            }
        }
    }
}