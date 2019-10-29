﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEvent : Event<CameraShakeEvent> {

    public float startValue;
    public float startDuration;

}

public class CameraBoundsChangeEvent : Event<CameraBoundsChangeEvent> {

    public BoxCollider2D cameraBounds;

}