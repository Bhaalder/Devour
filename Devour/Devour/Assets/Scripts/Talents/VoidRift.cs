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
    private float timeBetweenSound = 0.5f;
    private float timeToNextSound;
    private int previouslyPlayedSound;
    

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
            PlaySound();
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
            timeToNextSound = timeBetweenSound;
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
        }
    }

    private void PlaySound() {
        timeToNextSound -= Time.unscaledDeltaTime;
        if(timeToNextSound <= 0) {
            timeBetweenSound = Random.Range(0.5f, 0.55f);
            timeToNextSound = timeBetweenSound;
            int i = Random.Range(1, 6 + 1);
            while(i == previouslyPlayedSound) {
                i = Random.Range(1, 6 + 1);
            }
            previouslyPlayedSound = i;
            if (GameController.Instance.Player.Health != GameController.Instance.Player.MaxHealth) {
                AudioPlaySoundEvent healSound = new AudioPlaySoundEvent {
                    name = "HealSound" + i,
                    isRandomPitch = false,
                    soundType = SoundType.SFX
                };
                healSound.FireEvent();
            }
        }
    }

}
