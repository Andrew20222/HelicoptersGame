using AirPlaneSystems;
using UnityEngine;

namespace LightingSystem.Controlls
{
    public class LightController : MonoBehaviour
    {
        [Header("Turbine light settings")]
        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightDefault = 1f;

        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightTurbo = 5f;

        [SerializeField] private Light[] turbineLights;
        
        [SerializeField] private AirPlaneController airPlaneController;
        
        private float _currentEngineLightIntensity;

        private void OnEnable()
        {
            airPlaneController.TurboInput += SetLightTurbo;
            airPlaneController.NotTurboInput += SetLightDefault;
        }
        
        private void OnDisable()
        {
            airPlaneController.TurboInput -= SetLightTurbo;
            airPlaneController.NotTurboInput -= SetLightDefault;
        }

        private void Update()
        {
            UpdateLights();
        }

        private void SetLightTurbo()
        {
            _currentEngineLightIntensity = turbineLightTurbo;
        }
        
        private void SetLightDefault()
        {
            _currentEngineLightIntensity = turbineLightDefault;
        }
        
        private void ControlEngineLights(Light[] _lights, float _intensity)
        {
            for (int i = 0; i < _lights.Length; i++)
            {
                if(!airPlaneController.PlaneIsDead())
                {
                    _lights[i].intensity = Mathf.Lerp(_lights[i].intensity, _intensity, 10f * Time.deltaTime);
                }
                else
                {
                    _lights[i].intensity = Mathf.Lerp(_lights[i].intensity, 0f, 10f * Time.deltaTime);
                }
               
            }
        }
        private void UpdateLights()
        {
            if(!airPlaneController.PlaneIsDead())
            {
                if (turbineLights.Length > 0)
                {
                    ControlEngineLights(turbineLights, _currentEngineLightIntensity);
                }
            }
            else
            {
                if (turbineLights.Length > 0)
                {
                    ControlEngineLights(turbineLights, 0f);
                }
            }
        }
    }
}