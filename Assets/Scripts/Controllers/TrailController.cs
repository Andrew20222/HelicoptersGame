using AirPlaneSystems;
using UnityEngine;

namespace TrailSystem.Controlls
{
    public class TrailController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer[] wingTrailEffects;
        [SerializeField] private AirPlaneController airPlaneController;

        private void OnEnable()
        {
            airPlaneController.TrailEffect += ChangeWingTrailEffectThickness;
        }

        private void OnDisable()
        {
            airPlaneController.TrailEffect -= ChangeWingTrailEffectThickness;
        }

        private void ChangeWingTrailEffectThickness(float _thickness)
        {
            for (int i = 0; i < wingTrailEffects.Length; i++)
            {
                wingTrailEffects[i].startWidth = Mathf.Lerp(wingTrailEffects[i].startWidth, _thickness, Time.deltaTime * 10f);
            }
        }
    }
}