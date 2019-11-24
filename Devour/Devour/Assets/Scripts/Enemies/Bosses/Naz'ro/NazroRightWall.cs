using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroRightWall : MonoBehaviour{

    public bool Break { get; set; }

    [SerializeField] private Nazro nazro;
    [SerializeField] private GameObject breakParticleGO;

    private void Start() {
        
    }

    private void Update() {
        if (Break) {
            Destroy(gameObject);
        }
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