﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour{

    [SerializeField] private Animator animator;
    private bool isFading;

    private void Awake() {
        FadeScreenEvent.RegisterListener(Fade);
    }

    private void Fade(FadeScreenEvent fadeScreen) {
        if (fadeScreen.isFadeIn) {
            CameraZoomEvent regularZoom = new CameraZoomEvent {
                zoomValue = 0
            };
            regularZoom.FireEvent();
            animator.SetTrigger("FadeIn");
        }
        if (fadeScreen.isFadeOut) {
            animator.SetTrigger("FadeOut");
        }
    }

    private void OnDestroy() {
        FadeScreenEvent.UnRegisterListener(Fade);
    }
    
}
