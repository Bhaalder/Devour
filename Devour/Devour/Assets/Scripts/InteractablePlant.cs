using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlant : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (GameController.Instance.Player.FacingDirection == 1) {
                anim.SetTrigger("Type1FromLeftTrigger");
            } else if (GameController.Instance.Player.FacingDirection == -1) {
                anim.SetTrigger("Type1FromRightTrigger");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {

        }
    }
}
