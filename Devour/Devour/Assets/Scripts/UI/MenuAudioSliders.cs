//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAudioSliders : MonoBehaviour {

    public Slider MasterVolumeSlider { get; set; }
    public Slider MusicVolumeSlider { get; set; }
    public Slider SfxVolumeSlider { get; set; }
    public Slider VoiceVolumeSlider { get; set; }

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;

    void Start() {

        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterValueChangeCheck(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicValueChangeCheck(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SFXValueChangeCheck(); });
        if(voiceVolumeSlider != null) {
            voiceVolumeSlider.onValueChanged.AddListener(delegate { VoiceValueChangeCheck(); });
        }
        MasterVolumeSlider = masterVolumeSlider;
        MusicVolumeSlider = musicVolumeSlider;
        SfxVolumeSlider = sfxVolumeSlider;
        VoiceVolumeSlider = voiceVolumeSlider;
    }
    //alla sliders ska vara mellan -80 till 0 i valuerange
    public void MasterValueChangeCheck() {
        AudioMixerVolumeEvent masterVolumeEvent = new AudioMixerVolumeEvent {
            soundMixerType = SoundMixerType.MASTER,
            volume = masterVolumeSlider.value
        };
        masterVolumeEvent.FireEvent();
    }

    public void MusicValueChangeCheck() {
        AudioMixerVolumeEvent musicVolumeEvent = new AudioMixerVolumeEvent {
            soundMixerType = SoundMixerType.MUSIC,
            volume = musicVolumeSlider.value
        };
        musicVolumeEvent.FireEvent();
    }

    public void SFXValueChangeCheck() {
        AudioMixerVolumeEvent sfxVolumeEvent = new AudioMixerVolumeEvent {
            soundMixerType = SoundMixerType.SFX,
            volume = sfxVolumeSlider.value
        };
        sfxVolumeEvent.FireEvent();
    }

    public void VoiceValueChangeCheck() {
        AudioMixerVolumeEvent voiceVolumeEvent = new AudioMixerVolumeEvent {
            soundMixerType = SoundMixerType.VOICE,
            volume = voiceVolumeSlider.value
        };
        voiceVolumeEvent.FireEvent();
    }
}
