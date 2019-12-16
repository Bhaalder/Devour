//Author: Patrik Ahlgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaySoundEvent : Event<AudioPlaySoundEvent> {

    public string name;
    public SoundType soundType;
    public bool isRandomPitch;
    public float minPitch;
    public float maxPitch;

}

public class AudioPauseSoundEvent : Event<AudioPauseSoundEvent> {

    public string name;
    public SoundType soundType;
    public bool pauseAllSounds;
    public bool pauseAllSFX;

}

public class AudioUnPauseSoundEvent : Event<AudioUnPauseSoundEvent> {

    public string name;
    public SoundType soundType;
    public bool unPauseAllSounds;
    public bool unPauseAllSFX;

}

public class AudioStopSoundEvent : Event<AudioStopSoundEvent> {

    public string name;
    public bool stopAllSounds;
    public bool stopAllSFXSounds;

}

public class AudioPlayRandomSoundEvent: Event<AudioPlayRandomSoundEvent> {

    public string[] name;
    public SoundType soundType;
    public bool isRandomPitch;
    public float minPitch;
    public float maxPitch;

}

public class AudioPlaySoundAtLocationEvent : Event<AudioPlaySoundAtLocationEvent> {

    public string name;
    public SoundType soundType;
    public bool isRandomPitch;
    public float minPitch;
    public float maxPitch;
    public GameObject gameObject;

}

public class AudioPlayRandomSoundAtLocationEvent : Event<AudioPlayRandomSoundAtLocationEvent> {

    public string[] name;
    public SoundType soundType;
    public bool isRandomPitch;
    public float minPitch;
    public float maxPitch;
    public GameObject gameObject;

}

public class AudioSwitchBackgroundSoundEvent : Event<AudioSwitchBackgroundSoundEvent> {

    public string backgroundSoundNameToFadeOut;
    public SoundType backgroundSoundTypeToFadeOut;
    public float fadeDuration;
    public float soundVolumePercentage;
    public string backgroundSoundNametoStart;
    public SoundType backgroundSoundTypeToStart;

}

public class AudioPlaySequence : Event<AudioPlaySequence> {

    public string[] name;
    public SoundType soundType;

}

public class AudioFadeSoundEvent : Event<AudioFadeSoundEvent> {

    public string name;
    public SoundType soundType;
    public bool isFadeOut;
    public bool isFadeIn;
    public float fadeDuration;
    public float soundVolumePercentage;
    public float stopValue;

}

public class AudioSoundVolumeEvent : Event<AudioSoundVolumeEvent> {

    public string name;
    public SoundType soundType;
    public float volume;

}

public class AudioMixerVolumeEvent : Event<AudioMixerVolumeEvent> {

    public float volume;
    public SoundMixerType soundMixerType;

}

public class AudioMixerPitchEvent : Event<AudioMixerPitchEvent> {

    public float pitch;
    public SoundMixerType soundMixerType;

}

public class AudioStopAllCoroutinesEvent : Event<AudioStopAllCoroutinesEvent> {

}

public class FadeBackgroundSoundEvent : Event<FadeBackgroundSoundEvent> {

    public bool fadeCurrentSceneMusic;
    public bool fadeCurrentSceneAmbience;
    public float fadeDuration;
    public SoundType soundType;

}