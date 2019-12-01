//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatform : MonoBehaviour{

    [SerializeField] private GameObject brokenParticles;
    [SerializeField] private GameObject aboutToBreakParticles;

    [Tooltip("How long until it is destroyed")]
    [SerializeField] private float timeUntilDestroyed;
    [Tooltip("How long until it respawns after being destroyed")]
    [SerializeField] private float timeUntilRespawn;
    private Animator animator;
    private bool startToBreak;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !startToBreak) {
            GameObject instantiatedParticle = Instantiate(aboutToBreakParticles, transform.GetChild(0).position, Quaternion.identity);
            AudioPlaySoundAtLocationEvent rockBreakingSound = new AudioPlaySoundAtLocationEvent {
                name = "HitRockWall",
                isRandomPitch = true,
                minPitch = 0.95f,
                maxPitch = 1f,
                soundType = SoundType.SFX,
                gameObject = instantiatedParticle
            };
            animator.Play("timedPlatformAnimShake");
            animator.SetBool("PlayNext", true);
            rockBreakingSound.FireEvent();
            startToBreak = true;
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break() {
        
        yield return new WaitForSecondsRealtime(timeUntilDestroyed);
        GameObject instantiatedParticle = Instantiate(brokenParticles, transform.GetChild(0).position, Quaternion.identity);
        string[] breakRockWall = { "BreakRockWall1", "BreakRockWall1" };
        AudioPlayRandomSoundAtLocationEvent rockBreakSound = new AudioPlayRandomSoundAtLocationEvent {
            name = breakRockWall,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1f,
            soundType = SoundType.SFX,
            gameObject = instantiatedParticle
        };
        animator.SetBool("PlayNext", false);
        rockBreakSound.FireEvent();
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }



    private IEnumerator Respawn() {
        yield return new WaitForSecondsRealtime(timeUntilRespawn);
        transform.GetChild(0).gameObject.SetActive(true);
        startToBreak = false;
    }
}
