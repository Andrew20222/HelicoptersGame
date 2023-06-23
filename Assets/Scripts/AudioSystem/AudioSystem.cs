using HeneGames.Airplane;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace AudioSystem
{
    public class AudioSystem : MonoBehaviour
    {
        [FormerlySerializedAs("airplaneState")] [SerializeField]
        private AirplaneStateB airplaneStateB;
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
            _audioSystem.AudioSetVolumeWithState(airplaneStateB, engineSoundSource, defaultSoundPitch, maxEngineSound, turboSoundPitch);
        }
    }
}