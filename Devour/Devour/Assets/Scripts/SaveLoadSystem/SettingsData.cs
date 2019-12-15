using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class SettingsData
{
    public float MasterVolumeSliderValue { get; set; }
    public float MusicVolumeSliderValue { get; set; }
    public float SfxVolumeSliderValue { get; set; }
    public float VoiceVolumeSliderValue { get; set; }
    public bool DepthBlur { get; set; }

    public SettingsData(Settings settingsData)
    {
        MasterVolumeSliderValue = settingsData.MasterVolumeSliderValue;
        MusicVolumeSliderValue = settingsData.MusicVolumeSliderValue;
        SfxVolumeSliderValue = settingsData.SfxVolumeSliderValue;
        VoiceVolumeSliderValue = settingsData.VoiceVolumeSliderValue;
        DepthBlur = settingsData.DepthBlur;
    }
}