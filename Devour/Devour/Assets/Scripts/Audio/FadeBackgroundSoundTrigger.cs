using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBackgroundSoundTrigger : MonoBehaviour{

    [SerializeField] private bool fadeMusic;
    [SerializeField] private bool fadeAmbience;
    [SerializeField] private float fadeDuration;
    [Tooltip("If this boss is dead, the fade will not trigger (if nothing is entered it will function as a normal fade)")]
    [SerializeField] private string bossName;

    private void Start() {
        if (GameController.Instance.KilledBosses.Contains(bossName)) {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            FadeBackgroundSoundEvent backgroundSoundEvent = new FadeBackgroundSoundEvent {
                fadeCurrentSceneMusic = fadeMusic,
                fadeCurrentSceneAmbience = fadeAmbience,
                fadeDuration = fadeDuration
            };
            backgroundSoundEvent.FireEvent();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            gameObject.SetActive(false);
        }
    }

}
