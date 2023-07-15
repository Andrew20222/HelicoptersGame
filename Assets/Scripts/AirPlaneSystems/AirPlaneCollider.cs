
using CheckPoints;
using LandingSystem.Areas;
using UnityEngine;

namespace AirPlaneSystems
{
    public class AirPlaneCollider : MonoBehaviour
    {
        private bool _collideSomething;
        public bool CollideSomething
        {
            get => _collideSomething;
            set => _collideSomething = value;
        }
        
        [HideInInspector] public AirPlaneController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AirPlaneCollider>() == null &&
                other.GetComponent<LandingArea>() == null)
            {
                _collideSomething = true;
            }

            if (other.GetComponent<Checkpoint>())
            {
                _collideSomething = false;
            }
        }
    }
}
    
