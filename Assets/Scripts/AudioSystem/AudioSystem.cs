using HeneGames.Airplane;
using UnityEngine;
using Zenject;

namespace AudioSystem
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField]
        private AirplaneState airplaneState;
        [SerializeField]
        private AudioSource engineSoundSource;
        [SerializeField]
        private float defaultSoundPitch;
        [SerializeField]
        private float maxEngineSound;
        [SerializeField]
        private float turboSoundPitch;

        private IAudioSystem _audioSystem;

        [Inject]
        private void Construct(IAudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
        }
        private void Update()
        {
            _audioSystem.AudioSetVolumeWithState(airplaneState, engineSoundSource, defaultSoundPitch, maxEngineSound, turboSoundPitch);
        }
    }
}