using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds: MonoBehaviour{

    [SerializeField] private GameObject bounds;
    private BoxCollider2D boxCollider2D;

    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (!bounds.activeSelf) {
                bounds.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            bounds.SetActive(false);
        }
    }
}
