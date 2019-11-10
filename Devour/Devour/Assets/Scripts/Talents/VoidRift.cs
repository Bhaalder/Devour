using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRift : MonoBehaviour{

    private bool playerIsInRadius;

    private void LateUpdate() {
        if (playerIsInRadius) {
            if (Input.GetButtonDown("Interact")) {
                VoidTalentScreenEvent e = new VoidTalentScreenEvent { };
                e.FireEvent();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerIsInRadius = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerIsInRadius = false;
        }
    }
}
