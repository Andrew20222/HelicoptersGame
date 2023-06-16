using HeneGames.Airplane;
using UnityEngine;

public interface IAudioSystem
{
    void AudioSetVolumeWithState(AirplaneState airplaneState, AudioSource engineSoundSource, float defaultSoundPitch, float maxEngineSound, float turboSoundPitch);
}
