using AirPlaneSystems;
using UnityEngine;

namespace PropellesSystem.Controlls
{
    public class PropellesController : MonoBehaviour
    {
        [SerializeField] private AirPlaneController airPlaneController;
        [SerializeField] private GameObject[] propellers;
        [SerializeField] private float propelSpeedMultiplier;

        private void Update()
        {
            UpdatePropellers();
        }

        private void UpdatePropellers()
        {
            if(!airPlaneController.PlaneIsDead())
            {
                if (propellers.Length > 0)
                {
                    RotatePropellers(propellers, airPlaneController.CurrentSpeed() * propelSpeedMultiplier);
                }
            }
            else
            {
                if (propellers.Length > 0)
                {
                    RotatePropellers(propellers, 0f);
                }
            }
        }

        private void RotatePropellers(GameObject[] _rotateThese, float _speed)
        {
            for (int i = 0; i < _rotateThese.Length; i++)
            {
                _rotateThese[i].transform.Rotate(Vector3.forward * (-_speed * Time.deltaTime));
            }
        }
    }
}