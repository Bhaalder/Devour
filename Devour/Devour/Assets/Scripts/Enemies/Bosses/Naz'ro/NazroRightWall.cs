using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroRightWall : MonoBehaviour{

    public bool Break { get; set; }

    [SerializeField] private Nazro nazro;
    [SerializeField] private GameObject breakParticleGO;
    [SerializeField] private GameObject[] objectsToDisable;

    private void Start() {
        
    }

    private void Update() {
        if (Break) {
            for(int i = 0; i < objectsToDisable.Length; i++) {
                objectsToDisable[i].SetActive(false);
            }
            GetComponent<BoxCollider2D>().enabled = false;
            OnBreak();
            Break = false;
        }
    }

    private void OnBreak() {
        if (breakParticleGO != null) {
            Instantiate(breakParticleGO, transform.position, Quaternion.identity);
        }
        CameraShakeEvent cameraShake = new CameraShakeEvent {
            startValue = 0.85f,
            startDuration = 0.50f
        };
        cameraShake.FireEvent();
        AudioPlaySoundAtLocationEvent breakWallSound = new AudioPlaySoundAtLocationEvent {
            name = "NazroBreakWall",
            isRandomPitch = false,
            soundType = SoundType.SFX,
            gameObject = gameObject
        };
        breakWallSound.FireEvent();
    }

    private void OnDestroy() {
        if (breakParticleGO != null) {
            Instantiate(breakParticleGO, transform.position, Quaternion.identity);
        }
        CameraShakeEvent cameraShake = new CameraShakeEvent {
            startValue = 0.85f,
            startDuration = 0.50f
        };
        cameraShake.FireEvent();
    }

}