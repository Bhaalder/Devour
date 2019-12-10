//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType {
    DEFAULT, MUSIC, SFX, VOICE
}

public enum SoundMixerType {
    MASTER, MUSIC, SFX, VOICE
}

public class AudioController : MonoBehaviour {

    [Header("Player")]
    [SerializeField] private Sound[] playerSounds;
    [Header("Enemies")]
    [SerializeField] private Sound[] enemySounds;
    [Header("Environment")]
    [SerializeField] private Sound[] environmentSounds;
    [Header("Music")]
    [SerializeField] private Sound[] musicSounds;
    [Header("VoiceLines")]
    [SerializeField] private Sound[] voiceSounds;
    [Header("Other")]
    [SerializeField] private Sound[] otherSounds;
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup voiceMixerGroup;

    private Dictionary<string, Sound> musicDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> sfxDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> voiceDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> allSoundsDictionary = new Dictionary<string, Sound>();

    private Dictionary<string, float> soundTimerDictonary = new Dictionary<string, float>();

    private bool continueFadeIn;
    private bool continueFadeOut;

    private Sound sound;

    private static bool exists;

    private void Awake() {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }
        
        for(int i = 0; i < playerSounds.Length; i++) {
            allSoundsDictionary[playerSounds[i].name] = playerSounds[i];
            sfxDictionary[playerSounds[i].name] = playerSounds[i];
        }
        for (int i = 0; i < enemySounds.Length; i++) {
            allSoundsDictionary[enemySounds[i].name] = enemySounds[i];
            sfxDictionary[enemySounds[i].name] = enemySounds[i];
        }
        for (int i = 0; i < environmentSounds.Length; i++) {
            allSoundsDictionary[environmentSounds[i].name] = environmentSounds[i];
            sfxDictionary[environmentSounds[i].name] = environmentSounds[i];
        }
        for (int i = 0; i < musicSounds.Length; i++) {
            allSoundsDictionary[musicSounds[i].name] = musicSounds[i];
            musicDictionary[musicSounds[i].name] = musicSounds[i];
        }
        for (int i = 0; i < voiceSounds.Length; i++) {
            allSoundsDictionary[voiceSounds[i].name] = voiceSounds[i];
            voiceDictionary[voiceSounds[i].name] = voiceSounds[i];
        }
        for(int i = 0; i < otherSounds.Length; i++) {
            allSoundsDictionary[otherSounds[i].name] = otherSounds[i];
            sfxDictionary[otherSounds[i].name] = otherSounds[i];
        }
        for (int i = 0; i < allSoundsDictionary.Count; i++) {
            Sound s = allSoundsDictionary.ElementAt(i).Value;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialBlend_2D_3D;
            s.source.rolloffMode = (AudioRolloffMode)s.rolloffMode;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = masterMixerGroup;
        }

        for(int i = 0; i < sfxDictionary.Count; i++) {
            sfxDictionary.ElementAt(i).Value.source.outputAudioMixerGroup = sfxMixerGroup;
        }

        for (int i = 0; i < musicDictionary.Count; i++) {
            musicDictionary.ElementAt(i).Value.source.outputAudioMixerGroup = musicMixerGroup;
        }

        for (int i = 0; i < voiceDictionary.Count; i++) {
            voiceDictionary.ElementAt(i).Value.source.outputAudioMixerGroup = voiceMixerGroup;
        }

        AudioPlaySoundEvent.RegisterListener(Play);
        AudioStopSoundEvent.RegisterListener(Stop);
        AudioPauseSoundEvent.RegisterListener(Pause);
        AudioPlaySoundAtLocationEvent.RegisterListener(Play_InWorldspace);
        AudioPlayRandomSoundAtLocationEvent.RegisterListener(PlayRandom_InWorldspace);
        AudioPlayRandomSoundEvent.RegisterListener(PlayRandom);
        AudioPlaySequence.RegisterListener(PlaySequence);
        AudioFadeSoundEvent.RegisterListener(Fade);
        AudioSoundVolumeEvent.RegisterListener(SoundSetVolume);
        AudioMixerVolumeEvent.RegisterListener(MixerSetVolume);
        AudioMixerPitchEvent.RegisterListener(MixerSetPitch);
        AudioSwitchBackgroundSoundEvent.RegisterListener(SwitchSound);
        AudioStopAllCoroutinesEvent.RegisterListener(OnStopAllCoroutines);
    }

    #region Play/Stop Methods

    private void Play(AudioPlaySoundEvent soundEvent) {
        try {
            FindSound(soundEvent.name, soundEvent.soundType);
            if (soundEvent.isRandomPitch) {
                sound.source.pitch = Random.Range(soundEvent.minPitch, soundEvent.maxPitch);
            } else {
                sound.source.pitch = sound.pitch;
            }
            sound.source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(soundEvent.name);
        }
    }

    private void Play_InWorldspace(AudioPlaySoundAtLocationEvent soundEvent) {
        try {
            if (allSoundsDictionary.ContainsKey(soundEvent.name)) {
                InWorldSpaceRoutine(soundEvent.name, soundEvent.soundType, soundEvent.gameObject);
                if (soundEvent.isRandomPitch) {
                    sound.source.pitch = Random.Range(soundEvent.minPitch, soundEvent.maxPitch);
                } else {
                    sound.source.pitch = sound.pitch;
                }
                sound.source.Play();
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(soundEvent.name);
        }
    }

    private void Stop(AudioStopSoundEvent stopSoundEvent) {
        if (stopSoundEvent.stopAllSounds) {
            StopAllSounds();
            return;
        }
        try {
            FindSound(stopSoundEvent.name, 0);
            sound.source.Stop();
        } catch (System.NullReferenceException) {
            AudioNotFound(stopSoundEvent.name);
        }
    }

    private void StopAllSounds() {
        for(int i = 0; i < allSoundsDictionary.Count; i++) {
            try {
                allSoundsDictionary.ElementAt(i).Value.source.Stop();
            } catch (System.NullReferenceException) {

            }
        }
    }

    private void OnStopAllCoroutines(AudioStopAllCoroutinesEvent stopAllCoroutinesEvent){
        StopAllCoroutines();
    }

    #endregion

    #region PlayRandom

    private void PlayRandom(AudioPlayRandomSoundEvent randomSoundEvent) {
        int i = Random.Range(0, randomSoundEvent.name.Length);
        try {
            FindSound(randomSoundEvent.name[i], randomSoundEvent.soundType);
            if (randomSoundEvent.isRandomPitch) {
                sound.source.pitch = Random.Range(randomSoundEvent.minPitch, randomSoundEvent.maxPitch);
            } else {
                sound.source.pitch = sound.pitch;
            }
            sound.source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(randomSoundEvent.name[i]);
        }
    }

    private void PlayRandom_InWorldspace(AudioPlayRandomSoundAtLocationEvent randomSoundEvent) {
        int i = Random.Range(0, randomSoundEvent.name.Length);
        try {
            if (allSoundsDictionary.ContainsKey(randomSoundEvent.name[i])) {
                InWorldSpaceRoutine(randomSoundEvent.name[i], randomSoundEvent.soundType, randomSoundEvent.gameObject);
                if (randomSoundEvent.isRandomPitch) {
                    sound.source.pitch = Random.Range(randomSoundEvent.minPitch, randomSoundEvent.maxPitch);
                } else {
                    sound.source.pitch = sound.pitch;
                }
                sound.source.Play();
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(randomSoundEvent.name[i]);
        }
    }

    #endregion

    #region Volume / Pitch Methods

    private void SoundSetVolume(AudioSoundVolumeEvent soundVolumeEvent) {
        try {
            FindSound(soundVolumeEvent.name, soundVolumeEvent.soundType);
            sound.source.volume = soundVolumeEvent.volume;
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    private void MixerSetVolume(AudioMixerVolumeEvent mixerVolumeEvent) {
        string mixerName = "";
        switch (mixerVolumeEvent.soundMixerType) {
            case SoundMixerType.MASTER:
                mixerName = "MasterVolume";
                break;
            case SoundMixerType.MUSIC:
                mixerName = "MusicVolume";
                break;
            case SoundMixerType.SFX:
                mixerName = "SFXVolume";
                break;
            case SoundMixerType.VOICE:
                mixerName = "VoiceVolume";
                break;
            default:
                Debug.LogWarning("SoundMixerType is not defined, define it in the event");
                return;
        }
        if (mixerVolumeEvent.volume == -80) {
            audioMixer.SetFloat(mixerName, mixerVolumeEvent.volume);
        } else {
            audioMixer.SetFloat(mixerName, (mixerVolumeEvent.volume/2));
        }
    }

    private void MixerSetPitch(AudioMixerPitchEvent mixerPitchEvent) {
        string mixerName = "";
        if(mixerPitchEvent.pitch < 1) {
            Debug.LogWarning("Too low pitch, minimum value 1");
        }
        if (mixerPitchEvent.pitch > 1000) {
            Debug.LogWarning("Too high pitch, maximum value 1000");
        }
        switch (mixerPitchEvent.soundMixerType) {
            case SoundMixerType.MASTER:
                mixerName = "MasterPitch";
                break;
            case SoundMixerType.MUSIC:
                mixerName = "MusicPitch";
                break;
            case SoundMixerType.SFX:
                mixerName = "SFXPitch";
                break;
            case SoundMixerType.VOICE:
                mixerName = "VoicePitch";
                break;
            default:
                Debug.LogWarning("SoundMixerType is not defined, define it in the event");
                return;
        }
        audioMixer.SetFloat(mixerName, mixerPitchEvent.pitch);
    }

    #endregion

    #region Pause Methods

    private void Pause(AudioPauseSoundEvent pauseSoundEvent) {
        if (pauseSoundEvent.pauseAllSounds) {
            PauseAllSounds(true);
            return;
        }
        if (pauseSoundEvent.pauseAllSFX) {
            PauseAllSFX(true);
            return;
        }
        try {
            FindSound(pauseSoundEvent.name, 0);
            sound.source.Pause();
        } catch (System.NullReferenceException) {
            AudioNotFound(pauseSoundEvent.name);
        }
    }

    private void UnPause(AudioUnPauseSoundEvent unPauseSoundEvent) {
        if (unPauseSoundEvent.unPauseAllSounds) {
            PauseAllSounds(false);
            return;
        }
        if (unPauseSoundEvent.unPauseAllSFX) {
            PauseAllSFX(false);
            return;
        }
        try {
            FindSound(unPauseSoundEvent.name, 0);
            sound.source.UnPause();
        } catch (System.NullReferenceException) {
            AudioNotFound(unPauseSoundEvent.name);
        }
    }

    private void PauseAllSFX(bool pause) {
        if (pause) {
            for (int i = 0; i < sfxDictionary.Count; i++) {
                try {
                    sfxDictionary.ElementAt(i).Value.source.Pause();
                } catch (System.NullReferenceException) {

                }
            }
        } else if (!pause) {
            for (int i = 0; i < sfxDictionary.Count; i++) {
                try {
                    sfxDictionary.ElementAt(i).Value.source.UnPause();
                } catch (System.NullReferenceException) {

                }
            }
        }
    }

    private void PauseAllSounds(bool pause) {
        if (pause) {
            for (int i = 0; i < allSoundsDictionary.Count; i++) {
                try {
                    allSoundsDictionary.ElementAt(i).Value.source.Pause();
                } catch (System.NullReferenceException) {

                }
            }
        } else if (!pause) {
            for (int i = 0; i < allSoundsDictionary.Count; i++) {
                try {
                    allSoundsDictionary.ElementAt(i).Value.source.UnPause();
                } catch (System.NullReferenceException) {

                }
            }
        }
    }


    #endregion

    #region FadeIn/Out Methods

    private void Fade(AudioFadeSoundEvent fadeEvent) {
        try {
            FindSound(fadeEvent.name, fadeEvent.soundType);
            if (sound != null) {
                if (fadeEvent.isFadeIn) {
                    StartCoroutine(FadeInAudio(fadeEvent.fadeDuration, (fadeEvent.soundVolumePercentage / 100), sound));
                    return;
                }
                if (fadeEvent.isFadeOut) {
                    if(fadeEvent.stopValue == 0) {
                        fadeEvent.stopValue = 0.1f;
                    }
                    StartCoroutine(FadeOutAudio(fadeEvent.fadeDuration, (fadeEvent.soundVolumePercentage / 100), fadeEvent.stopValue, sound));
                    return;
                }
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(fadeEvent.name);
            return;
        }
        Debug.LogWarning("A sound fademethod was called but not declared as fadeIn or fadeOut, soundName: " + fadeEvent.name);
    }

    private IEnumerator FadeInAudio(float fadeDuration, float soundVolume, Sound sound) {
        continueFadeIn = true;
        continueFadeOut = false;
        float startSoundValue = 0;
        for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
            float normalizedTime = time / fadeDuration;
            sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
            yield return null;
        }
        StopAllCoroutines();
    }

    private IEnumerator FadeOutAudio(float fadeDuration, float soundVolume, float stopValue, Sound sound) {
        continueFadeIn = false;
        continueFadeOut = true;
        float startSoundValue = sound.source.volume;
        if (continueFadeOut) {
            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
                float normalizedTime = time / fadeDuration;
                sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
                if (sound.source.volume <= stopValue) {
                    sound.source.Stop();
                    break;
                }
                yield return null;
            }
        }
        sound.source.volume = startSoundValue;
        StopAllCoroutines();
    }
    #endregion

    #region SwitchBackgroundSound, FindSound, InWorldSpaceRoutine

    private void SwitchSound(AudioSwitchBackgroundSoundEvent audioSwitch) {
        if(audioSwitch.backgroundSoundNameToFadeOut != "" && audioSwitch.backgroundSoundNameToFadeOut != null) {
            FindSound(audioSwitch.backgroundSoundNameToFadeOut, audioSwitch.backgroundSoundTypeToFadeOut);
            if (sound.source.isPlaying) {
                StartCoroutine(FadeOutAudio(audioSwitch.fadeDuration, (audioSwitch.soundVolumePercentage / 100), 0.1f, sound));
            }
        }
        sound = null;
        FindSound(audioSwitch.backgroundSoundNametoStart, audioSwitch.backgroundSoundTypeToStart);
        if (sound != null) {
            sound.source.Play();
        }
    }

    private void FindSound(string name, SoundType soundType) {
        try {
            switch (soundType) {
                case SoundType.DEFAULT:
                    sound = allSoundsDictionary[name];
                    break;
                case SoundType.MUSIC:
                    sound = musicDictionary[name];
                    break;
                case SoundType.SFX:
                    sound = sfxDictionary[name];
                    break;
                case SoundType.VOICE:
                    sound = voiceDictionary[name];
                    break;
                default:
                    sound = allSoundsDictionary[name];
                    break;
            }
        } catch (System.ArgumentNullException) {
            AudioNotFound(name);
        } catch (KeyNotFoundException) {
            AudioNotFound(name);
        }
    }

    private void InWorldSpaceRoutine(string name, SoundType soundType, GameObject soundAtLocationGO) {
        switch (soundType) {
            case SoundType.DEFAULT:
                sound = allSoundsDictionary[name];
                break;
            case SoundType.MUSIC:
                sound = musicDictionary[name];
                break;
            case SoundType.SFX:
                sound = sfxDictionary[name];
                break;
            case SoundType.VOICE:
                sound = voiceDictionary[name];
                break;
        }
        sound.source = soundAtLocationGO.GetComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.spatialBlend = sound.spatialBlend_2D_3D;
        sound.source.rolloffMode = (AudioRolloffMode)sound.rolloffMode;
        sound.source.minDistance = sound.minDistance;
        sound.source.maxDistance = sound.maxDistance;
        sound.source.loop = sound.loop;
    }

    #endregion

    #region WaitForFinish/PlayDelay/PlaySequence Methods

    private void PlaySequence(AudioPlaySequence playSequenceEvent) {
        try {
            FindSound(playSequenceEvent.name[0], SoundType.MUSIC);
            Sound firstSound = sound;
            FindSound(playSequenceEvent.name[1], SoundType.MUSIC);
            Sound secondSound = sound;
            firstSound.source.Play();
            StartCoroutine(WaitToPlay(secondSound, firstSound.source.clip.length));
        } catch (System.NullReferenceException) {
            AudioNotFound(playSequenceEvent.name[0]); AudioNotFound(playSequenceEvent.name[1]);
        }
    }

    //public void Play_ThenPlay(string name, string otherName) {
    //    try {
            
    //    } catch (System.NullReferenceException) {
    //        AudioNotFound(name); AudioNotFound(otherName);
    //    }
    //}

    private IEnumerator WaitToPlay(Sound sound, float waitTime) {
        yield return new WaitForSecondsRealtime(waitTime);
        sound.source.Play();
        StopAllCoroutines();
    }

    //public void Play_Delay(string name, float minDelay, float maxDelay) {
    //    try {
    //        sound = FindSound(name);
    //        StartCoroutine(PlayDelaySequence(sound, minDelay, maxDelay));
    //    } catch (System.NullReferenceException) {
    //        AudioNotFound(name);
    //    }
    //}

    //public void Play_Delay(string name, float minDelay, float maxDelay, float minPitch, float maxPitch) {
    //    try {
    //        sound = FindSound(name);
    //        StartCoroutine(PlayDelaySequence(sound, minDelay, maxDelay, minPitch, maxPitch));
    //    } catch (System.NullReferenceException) {
    //        AudioNotFound(name);
    //    }
    //}

    //private IEnumerator PlayDelaySequence(Sound sound, float minDelay, float maxDelay) {
    //    float delay = Random.Range(minDelay, maxDelay);

    //    sound.source.PlayDelayed(delay);
    //    yield return new WaitForSeconds(delay + sound.source.clip.length);
    //    Play_Delay(sound.name, minDelay, maxDelay);
    //    yield return null;
    //}

    //private IEnumerator PlayDelaySequence(Sound sound, float minDelay, float maxDelay, float minPitch, float maxPitch) {
    //    float delay = Random.Range(minDelay, maxDelay);
    //    float pitch = Random.Range(minPitch, maxPitch);

    //    sound.source.pitch = pitch;
    //    sound.source.PlayDelayed(delay);
    //    yield return new WaitForSeconds(delay + sound.source.clip.length);
    //    Play_Delay(sound.name, minDelay, maxDelay, minPitch, maxPitch);
    //    yield return null;
    //}

    //public void PlaySFX_Finish(string name, float minPitch, float maxPitch, float extraTimeIntervall) {
    //    sound = FindSFX(name);
    //    try {
    //        if (!soundTimerDictonary.ContainsKey(sound.name)) {
    //            soundTimerDictonary[sound.name] = 0f;
    //        }
    //        sound.source.pitch = Random.Range(minPitch, maxPitch);
    //        if (CanPlaySound(sound, extraTimeIntervall)) {
    //            sound.source.Play();
    //        }
    //    } catch (System.NullReferenceException) {
    //        AudioNotFound(name);
    //    }
    //}

    //public void PlaySFX_Finish(string name, float minPitch, float maxPitch, float extraTimeIntervall, float minVolume, float maxVolume) {
    //    sound = FindSFX(name);
    //    try {
    //        if (!soundTimerDictonary.ContainsKey(sound.name)) {
    //            soundTimerDictonary[sound.name] = 0f;
    //        }
    //        sound.source.pitch = Random.Range(minPitch, maxPitch);
    //        sound.source.volume = Random.Range(minVolume, maxVolume);
    //        if (CanPlaySound(sound, extraTimeIntervall)) {
    //            sound.source.Play();
    //        }
    //    } catch (System.NullReferenceException) {
    //        AudioNotFound(name);
    //    }
    //}

    //private bool CanPlaySound(Sound sound, float extraTime) {
    //    if (soundTimerDictonary.ContainsKey(sound.name)) {
    //        float lastTimePlayed = soundTimerDictonary[sound.name];
    //        float soundTime = sound.source.clip.length;
    //        if (lastTimePlayed + soundTime + extraTime < Time.time) {
    //            soundTimerDictonary[sound.name] = Time.time;
    //            return true;
    //        } else {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    #endregion

    #region GetSoundLength Methods

    private float GetSoundLength(string name) {
        if (allSoundsDictionary.ContainsKey(name)) {
            return allSoundsDictionary[name].source.clip.length;
        }
        return 0;
    }

    #endregion

    private void AudioNotFound(string name) {
        Debug.LogWarning("The sound with name '" + name + "' could not be found in list. Is it spelled correctly?");
    }

    private void OnDestroy() {
        AudioPlaySoundEvent.UnRegisterListener(Play);
        AudioStopSoundEvent.UnRegisterListener(Stop);
        AudioPauseSoundEvent.UnRegisterListener(Pause);
        AudioPlaySoundAtLocationEvent.UnRegisterListener(Play_InWorldspace);
        AudioPlayRandomSoundAtLocationEvent.UnRegisterListener(PlayRandom_InWorldspace);
        AudioPlayRandomSoundEvent.UnRegisterListener(PlayRandom);
        AudioPlaySequence.UnRegisterListener(PlaySequence);
        AudioFadeSoundEvent.UnRegisterListener(Fade);
        AudioSoundVolumeEvent.UnRegisterListener(SoundSetVolume);
        AudioMixerVolumeEvent.UnRegisterListener(MixerSetVolume);
        AudioMixerPitchEvent.UnRegisterListener(MixerSetPitch);
        AudioSwitchBackgroundSoundEvent.UnRegisterListener(SwitchSound);
        AudioStopAllCoroutinesEvent.UnRegisterListener(OnStopAllCoroutines);
    }

}
