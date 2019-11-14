//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
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

        foreach (Sound player_S in playerSounds) { allSoundsDictionary[player_S.name] = player_S; sfxDictionary[player_S.name] = player_S; }
        foreach (Sound enemy_S in enemySounds) { allSoundsDictionary[enemy_S.name] = enemy_S; sfxDictionary[enemy_S.name] = enemy_S; }
        foreach (Sound environment_S in environmentSounds) { allSoundsDictionary[environment_S.name] = environment_S; sfxDictionary[environment_S.name] = environment_S; }
        foreach (Sound music_S in musicSounds) { allSoundsDictionary[music_S.name] = music_S; musicDictionary[music_S.name] = music_S; }
        foreach (Sound voice_S in voiceSounds) { allSoundsDictionary[voice_S.name] = voice_S; voiceDictionary[voice_S.name] = voice_S; }

        foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
            s.Value.source = gameObject.AddComponent<AudioSource>();
            s.Value.source.clip = s.Value.clip;
            s.Value.source.volume = s.Value.volume;
            s.Value.source.pitch = s.Value.pitch;
            s.Value.source.spatialBlend = s.Value.spatialBlend_2D_3D;
            s.Value.source.rolloffMode = (AudioRolloffMode)s.Value.rolloffMode;
            s.Value.source.minDistance = s.Value.minDistance;
            s.Value.source.maxDistance = s.Value.maxDistance;
            s.Value.source.loop = s.Value.loop;
            s.Value.source.outputAudioMixerGroup = masterMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
            s.Value.source.outputAudioMixerGroup = sfxMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in musicDictionary) {
            s.Value.source.outputAudioMixerGroup = musicMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in voiceDictionary) {
            s.Value.source.outputAudioMixerGroup = voiceMixerGroup;
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
        foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
            try {
                s.Value.source.Stop();
            } catch (System.NullReferenceException) {

            }
        }
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
            foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
                try {
                    s.Value.source.Pause();
                } catch (System.Exception) {

                }
            }
        } else if (!pause) {
            foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
                try {
                    s.Value.source.UnPause();
                } catch (System.Exception) {

                }
            }
        }
    }

    private void PauseAllSounds(bool pause) {
        if (pause) {
            foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
                try {
                    s.Value.source.Pause();
                } catch (System.Exception) {

                }
            }
        } else if (!pause) {
            foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {

                try {
                    s.Value.source.UnPause();
                } catch (System.Exception) {

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
                    StartCoroutine(FadeOutAudio(fadeEvent.fadeDuration, (fadeEvent.soundVolumePercentage / 100), sound));
                    return;
                }
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(fadeEvent.name);
        }
        Debug.LogWarning("A sound fademethod was called but not declared as fadeIn or fadeOut, soundName: " + fadeEvent.name);
    }

    private IEnumerator FadeInAudio(float fadeDuration, float soundVolume, Sound sound) {
        continueFadeIn = true;
        continueFadeOut = false;
        float startSoundValue = 0;
        if (continueFadeIn) {
            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
                float normalizedTime = time / fadeDuration;
                sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
                yield return null;
            }
        }
    }

    private IEnumerator FadeOutAudio(float fadeDuration, float soundVolume, Sound sound) {
        continueFadeIn = false;
        continueFadeOut = true;
        float startSoundValue = sound.source.volume;
        if (continueFadeOut) {
            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
                if (sound.source.volume <= 0.01) {
                    sound.source.Stop();
                }
                float normalizedTime = time / fadeDuration;
                sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
                yield return null;
            }
        }
        sound.source.volume = startSoundValue;
    }
    #endregion

    #region FindSound, InWorldSpaceRoutine

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
        } catch (KeyNotFoundException) {
            Debug.LogWarning("The sound with name '" + name + "' could not be found in list. Is it spelled correctly? (KeyNotFoundException)");
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
        float waitTime = 0;
        for (int i = 0; i <= playSequenceEvent.name.Length;) {
            try {
                FindSound(playSequenceEvent.name[i], playSequenceEvent.soundType);
                StartCoroutine(WaitToPlay(sound, waitTime));
                waitTime += sound.source.clip.length;
            } catch (System.NullReferenceException) {

            }
        }
    }

    private IEnumerator WaitToPlay(Sound sound, float waitTime) {
        yield return new WaitForSecondsRealtime(waitTime);
        sound.source.Play();
        yield return null;
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
        Debug.LogWarning("The sound with name '" + name + "' could not be found in list. Is it spelled correctly? (NullReferenceException)");
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
    }

}
