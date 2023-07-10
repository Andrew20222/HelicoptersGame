using AirPlaneSystems;
using HeneGames.Airplane;
using State.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace AudioSystems
{
    public class AirAudioSystem : MonoBehaviour
    {
        [SerializeField] private AirplaneState airplaneState;
        [SerializeField] private AudioSource engineSoundSource;
        [SerializeField] private AirPlaneController airPlaneController;
        [SerializeField] private float defaultSoundPitch;
        [SerializeField] private float maxEngineSound;
        [SerializeField] private float turboSoundPitch;

        private float _currentEngineSoundPitch;
        
        
        private void OnEnable()
        {
            airPlaneController.TurboInput += SetTurboPitch;
            airPlaneController.NotTurboInput += SetDefaultPitch;
        }

        private void OnDisable()
        {
            airPlaneController.TurboInput -= SetTurboPitch;
            airPlaneController.NotTurboInput -= SetDefaultPitch;
        }

        private void Update()
        {
            AudioSystem();
        }

        private void SetTurboPitch()
        {
            _currentEngineSoundPitch = turboSoundPitch;
        }

        private void SetDefaultPitch()
        {
            _currentEngineSoundPitch = defaultSoundPitch;
        }
        
        private void AudioSystem()
        {
            if (engineSoundSource == null)
                return;

            if (airplaneState == AirplaneState.Flying)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, _currentEngineSoundPitch, 10f * Time.deltaTime);

                if (airPlaneController.PlaneIsDead())
                {
                    engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, 0f, 10f * Time.deltaTime);
                }
                else
                {
                    engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, maxEngineSound, 1f * Time.deltaTime);
                }
            }
            else if (airplaneState == AirplaneState.Landing)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, defaultSoundPitch, 1f * Time.deltaTime);
                engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, 0f, 1f * Time.deltaTime);
            }
            else if (airplaneState == AirplaneState.Takeoff)
            {
                engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, turboSoundPitch, 1f * Time.deltaTime);
                engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, maxEngineSound, 1f * Time.deltaTime);
            }
        }
    }
}