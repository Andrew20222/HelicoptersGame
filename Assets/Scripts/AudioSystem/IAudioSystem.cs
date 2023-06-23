using HeneGames.Airplane;
using UnityEngine;

public interface IAudioSystem
{
    void AudioSetVolumeWithState(AirplaneStateB airplaneStateB, AudioSource engineSoundSource, float defaultSoundPitch, float maxEngineSound, float turboSoundPitch);
}
