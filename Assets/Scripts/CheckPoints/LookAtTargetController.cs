using UnityEngine;

namespace CheckPoints.LookAtTarget.Controller
{
    public class LookAtTargetController : MonoBehaviour
    {
        public Transform Target;
        public bool smooth = true;
        public float damping = 6.0f;

        private void LateUpdate()
        {
            if (Target != null)
            {
                if (smooth)
                {
                    var rotation = Quaternion.LookRotation(Target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                }
                else
                {
                    transform.LookAt(Target);
                }
            }
        }
    }
} 