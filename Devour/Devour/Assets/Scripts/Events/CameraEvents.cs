//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEvent : Event<CameraShakeEvent> {

    public float startValue;
    public float startDuration;

}

public class CameraChangeTargetEvent : Event<CameraChangeTargetEvent> {

    public Transform newTarget;
    public bool playerTarget;
}

public class CameraBoundsChangeEvent : Event<CameraBoundsChangeEvent> {

    public BoxCollider2D cameraBounds;

}

public class CameraTiltEvent : Event<CameraTiltEvent> {

    public float tiltValue;

}

public class CameraZoomEvent : Event<CameraZoomEvent> {

    public float zoomValue;

}