using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    //Main author: Brackeys@youtube
    //Secondary author: Patrik Ahlgren
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spatialBlend_2D_3D;

    public enum AudioRolloff { Logarithmic, Linear }
    public AudioRolloff rolloffMode;
    [Range(0f, 1000f)]
    public float minDistance;
    [Range(1.01f, 1000f)]
    public float maxDistance;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
