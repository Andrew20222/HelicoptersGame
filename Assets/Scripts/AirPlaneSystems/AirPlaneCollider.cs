using CheckPoints;
using LandingSystem.Areas;
using UnityEngine;

namespace AirPlaneSystems
{
    public class AirPlaneCollider : MonoBehaviour
    {
        private bool collideSomething;
        public bool CollideSomething { get; set; }
        
        [HideInInspector] public AirPlaneController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AirPlaneCollider>() == null &&
                other.GetComponent<LandingArea>() == null)
            {
                collideSomething = true;
            }

            if (other.GetComponent<Checkpoint>())
            {
                collideSomething = false;
            }
        }
    }
}
    
