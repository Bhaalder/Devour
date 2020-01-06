//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCutscene : MonoBehaviour{

    [SerializeField] private Transform cameraTarget;

    private void Start() {
        if (!GameController.Instance.KilledBosses.Contains("Zvixa")) {
            Destroy(gameObject);
        } else if (GameController.Instance.BarrierCutsceneHasPlayed) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameController.Instance.Player.IsTotallyInvulnerable = true;
            CameraChangeTargetEvent cameraTargetEvent = new CameraChangeTargetEvent {
                newTarget = cameraTarget
            };
            cameraTargetEvent.FireEvent();
            CameraOffsetEvent cameraOffset = new CameraOffsetEvent {
                newOffset = new Vector3(0, 0, -20),
                setBoundsInactive = true
            };
            cameraOffset.FireEvent();
            PlayerBusyEvent busyEvent = new PlayerBusyEvent {
                playerIsBusy = true
            };
            busyEvent.FireEvent();
            Invoke("SwitchBack", 5);
        }
    }

    private void SwitchBack() {
        GameController.Instance.Player.IsTotallyInvulnerable = false;
        GameController.Instance.Player.IsInvulnerable = true;
        GameController.Instance.Player.UntilInvulnerableEnds = 2;
        CameraChangeTargetEvent cameraTargetEvent = new CameraChangeTargetEvent {
            playerTarget = true
        };
        cameraTargetEvent.FireEvent();
        CameraOffsetEvent cameraOffset = new CameraOffsetEvent {
            newOffset = new Vector3(0, 0, -20),
            setBoundsInactive = false
        };
        cameraOffset.FireEvent();
        PlayerBusyEvent busyEvent = new PlayerBusyEvent {
            playerIsBusy = false
        };
        busyEvent.FireEvent();
        GameController.Instance.BarrierCutsceneHasPlayed = true;
        Destroy(gameObject);
    }

}
