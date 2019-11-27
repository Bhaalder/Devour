using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSoundManager : MonoBehaviour{

    public string CurrentMusicPlaying { get; set; }
    public string CurrentBackgroundSoundPlaying { get; set; }


    [SerializeField] private SceneBackgroundSound[] backgroundSounds;
    private string musicToFade;
    private string ambienceToFade;
    private string musicToStart;
    private string ambienceToStart;

    private void Start() {
        SwitchSceneEvent.RegisterListener(OnSceneSwitch);  
        for (int i = 0; i < backgroundSounds.Length; i++) {
            if (SceneManager.GetActiveScene().name == backgroundSounds[i].sceneName) {
                CurrentMusicPlaying = backgroundSounds[i].musicName;
                CurrentBackgroundSoundPlaying = backgroundSounds[i].ambienceName;
                StartSound(CurrentMusicPlaying, SoundType.MUSIC);
                StartSound(CurrentBackgroundSoundPlaying, SoundType.SFX);
                break;
            }
        }

    }

    private void OnSceneSwitch(SwitchSceneEvent sceneEvent) {
        bool isSameMusic = false;
        bool isSameAmbience = false;
        for(int i = 0; i < backgroundSounds.Length; i++) {
            if(SceneManager.GetActiveScene().name == backgroundSounds[i].sceneName) {
                musicToFade = backgroundSounds[i].musicName;
                ambienceToFade = backgroundSounds[i].ambienceName;
            }
            if(sceneEvent.enteringSceneName == backgroundSounds[i].sceneName) {
                musicToStart = backgroundSounds[i].musicName;
                ambienceToStart = backgroundSounds[i].ambienceName;
            }
        }
        if(musicToStart == CurrentMusicPlaying) {
            isSameMusic = true;
        }
        if (musicToStart == "" || musicToStart == null) {
            musicToFade = CurrentMusicPlaying;
        } else {
            CurrentMusicPlaying = musicToStart;
        }
        if (!isSameMusic) {
            SwitchSound(musicToFade, musicToStart, SoundType.MUSIC, 1f);
        }
        if(ambienceToStart == CurrentBackgroundSoundPlaying) {
            isSameAmbience = true;
        }
        if (ambienceToStart == "" || ambienceToStart == null) {
            ambienceToStart = CurrentBackgroundSoundPlaying;
        } else {
            CurrentBackgroundSoundPlaying = ambienceToStart;
        }
        if (!isSameAmbience) {
            SwitchSound(ambienceToFade, ambienceToStart, SoundType.DEFAULT, 1f);
        }
    }

    private void StartSound(string soundToStart, SoundType soundType) {
        AudioSwitchBackgroundSoundEvent startSound = new AudioSwitchBackgroundSoundEvent {
            backgroundSoundNametoStart = soundToStart,
            backgroundSoundTypeToStart = soundType
        };
        startSound.FireEvent();
    }

    private void SwitchSound(string soundToFadeOut, string soundToStart, SoundType soundType, float fadeDuration) {
        AudioSwitchBackgroundSoundEvent soundSwitch = new AudioSwitchBackgroundSoundEvent {
            backgroundSoundNameToFadeOut = soundToFadeOut,
            backgroundSoundNametoStart = soundToStart,
            backgroundSoundTypeToFadeOut = soundType,
            backgroundSoundTypeToStart = soundType,
            fadeDuration = fadeDuration,
            soundVolumePercentage = 0
        };
        soundSwitch.FireEvent();
    }

    private void OnDestroy() {
        SwitchSceneEvent.UnRegisterListener(OnSceneSwitch);
    }

}
