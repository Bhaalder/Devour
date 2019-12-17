using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoidRift : MonoBehaviour{

    [TextArea(0, 5)]
    [SerializeField] private string voidRiftInfo;
    [Tooltip("How much the player should regenerate foreach 0.1 seconds")]
    [SerializeField] private float regeneration;

    private TextMeshProUGUI voidText;
    private bool playerIsInRadius;
    private float timeBetweenHeals = 0.1f;
    private float timeLeft;
    private bool healSoundIsPlaying;

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
            HealPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerIsInRadius = true;
            voidText.text = voidRiftInfo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerIsInRadius = false;
            voidText.text = "";
            timeLeft = timeBetweenHeals;
            StopHealSound();
        }
    }

    private void HealPlayer() {
        timeLeft -= Time.unscaledDeltaTime;
        if (timeLeft <= 0) {
            timeLeft = timeBetweenHeals;
            PlayerHealEvent heal = new PlayerHealEvent {
                amount = regeneration
            };
            heal.FireEvent();
            if (GameController.Instance.Player.Health != GameController.Instance.Player.MaxHealth && !healSoundIsPlaying) {
                AudioPlaySoundAtLocationEvent healSound = new AudioPlaySoundAtLocationEvent {
                    name = "VoidRiftHeal",
                    isRandomPitch = false,
                    soundType = SoundType.SFX,
                    gameObject = gameObject
                };
                healSound.FireEvent();
                healSoundIsPlaying = true;
            }
            if (GameController.Instance.Player.Health == GameController.Instance.Player.MaxHealth && healSoundIsPlaying) {
                StopHealSound();
            }
        }
    }

    private void StopHealSound() {
        AudioStopSoundEvent stopSound = new AudioStopSoundEvent {
            name = "VoidRiftHeal"
        };
        stopSound.FireEvent();
        healSoundIsPlaying = false;
    }

}
