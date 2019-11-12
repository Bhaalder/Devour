﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSound : MonoBehaviour{

    [SerializeField] private string soundName;
    [SerializeField] private SoundType soundType;

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
    }

    private void Start() {
        AudioPlaySoundEvent ambience = new AudioPlaySoundEvent {
            name = soundName,
            soundType = soundType
        };
        ambience.FireEvent();
    }


}