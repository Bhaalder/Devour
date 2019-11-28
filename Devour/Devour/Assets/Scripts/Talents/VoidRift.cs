using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoidRift : MonoBehaviour{

    [TextArea(0, 5)]
    [SerializeField] private string voidRiftInfo;

    private TextMeshProUGUI voidText;
    private bool playerIsInRadius;
    private float timeBetweenHeals = 0.1f;
    private float timeLeft;

    private void Awake() {
        voidText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        timeLeft = timeBetweenHeals;
    }

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
            voidText.text = voidRiftInfo;
            HealPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerIsInRadius = false;
            voidText.text = "";
            timeLeft = timeBetweenHeals;
        }
    }

    private void HealPlayer() {
        timeLeft -= Time.unscaledDeltaTime;
        if(timeLeft <= 0) {
            timeLeft = timeBetweenHeals;
            PlayerHealEvent heal = new PlayerHealEvent {
                amount = 1.5f
            };
            heal.FireEvent();
        }
    }

}
