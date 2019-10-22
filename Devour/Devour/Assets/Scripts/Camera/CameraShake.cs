//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour {  
    private float shakeValue;
    private float shakeDuration;

    private float shakePercentage;
    private float startValue;
    private float startDuration;

    private bool isShaking = false;

    [Header("Smooth")]
    [SerializeField] private bool isSmooth;
    [SerializeField] private float smoothValue = 3f;

    private void Start() {
        CameraShakeEvent.RegisterListener(Shake);
    }

    private void Shake(CameraShakeEvent shakeEvent) {
        startValue = shakeEvent.startValue;
        startDuration = shakeEvent.startDuration;
        shakeValue = startValue;
        shakeDuration = startDuration;
        if (!isShaking) {
            StartCoroutine(ShakeCamera());
        }
    }

    private IEnumerator ShakeCamera() {
        isShaking = true;
        while (shakeDuration > 0.01f) {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeValue;
            rotationAmount.z = 0;

            shakePercentage = shakeDuration / startDuration;

            shakeValue = startValue * shakePercentage;
            //shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//lerpa eller inte?
            shakeDuration -= 1 * Time.deltaTime;

            if (isSmooth) {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothValue);
            } else {
                transform.localRotation = Quaternion.Euler(rotationAmount);
            }

            yield return null;
        }
        transform.localRotation = Quaternion.identity;
        isShaking = false;
    }
}