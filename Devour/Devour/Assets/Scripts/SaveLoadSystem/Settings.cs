using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float MasterVolumeSliderValue { get; set; }
    public float MusicVolumeSliderValue { get; set; }
    public float SfxVolumeSliderValue { get; set; }
    public float VoiceVolumeSliderValue { get; set; }
    public bool DepthBlur { get; set; }

    void Start()
    {
        AudioMixerVolumeEvent.RegisterListener(MixerSetVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MixerSetVolume(AudioMixerVolumeEvent mixerVolumeEvent)
    {
        string mixerName = "";
        switch (mixerVolumeEvent.soundMixerType)
        {
            case SoundMixerType.MASTER:
                mixerName = "MasterVolume";
                MasterVolumeSliderValue = mixerVolumeEvent.volume;
                break;
            case SoundMixerType.MUSIC:
                mixerName = "MusicVolume";
                MusicVolumeSliderValue = mixerVolumeEvent.volume;
                break;
            case SoundMixerType.SFX:
                mixerName = "SFXVolume";
                SfxVolumeSliderValue = mixerVolumeEvent.volume;
                break;
            case SoundMixerType.VOICE:
                mixerName = "VoiceVolume";
                VoiceVolumeSliderValue = mixerVolumeEvent.volume;
                break;
            default:
                Debug.LogWarning("SoundMixerType is not defined, define it in the event");
                return;
        }
        Debug.Log(mixerName);
    }

    private void OnDestroy()
    {
        AudioMixerVolumeEvent.UnRegisterListener(MixerSetVolume);
    }
}
