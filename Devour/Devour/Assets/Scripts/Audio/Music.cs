using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour{//DENNA ÄR TEMPORÄR, FIXA SOUNDMANAGER

    [SerializeField] private string soundName;
    [SerializeField] private SoundType soundType;

    private void Start() {
        AudioPlaySoundEvent ambience = new AudioPlaySoundEvent {
            name = soundName,
            soundType = soundType
        };
        ambience.FireEvent();
    }

}
