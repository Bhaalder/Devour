using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    [SerializeField] private float zoomChange;

    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            CameraZoomEvent zoomIn = new CameraZoomEvent {
                zoomValue = zoomChange
            };
            zoomIn.FireEvent();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            CameraZoomEvent zoomOut = new CameraZoomEvent {
                zoomValue = 0
            };
            zoomOut.FireEvent();
        }
    }

}
