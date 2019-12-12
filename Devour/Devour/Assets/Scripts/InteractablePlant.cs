using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlant : MonoBehaviour {

    private Animator anim;
    bool isTouching;
    void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isTouching) {
            if (GameController.Instance.Player.FacingDirection == 1) {
                anim.SetTrigger("Type1FromLeftTrigger");
                isTouching = true;
            } else if (GameController.Instance.Player.FacingDirection == -1 && !isTouching) {
                anim.SetTrigger("Type1FromRightTrigger");
                isTouching = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isTouching = false;
        }
    }
}
