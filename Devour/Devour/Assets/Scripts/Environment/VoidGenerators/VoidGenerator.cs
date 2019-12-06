//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidGenerator : MonoBehaviour{

    [SerializeField] private int voidGeneratorID;
    [SerializeField] private float health;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject[] voidBeamsToDestroy;
    private BoxCollider2D boxCollider2D;
    public Animator anim;

    private void Start() {
        if (GameController.Instance.DestroyedVoidGenerators.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.DestroyedVoidGenerators[SceneManager.GetActiveScene().name].Contains(voidGeneratorID)) {
                if (voidBeamsToDestroy != null) {
                    for(int i = 0; i < voidBeamsToDestroy.Length; i++) {
                        Destroy(voidBeamsToDestroy[i]);
                    }
                }
                Destroy(gameObject);
                return;
            }
        }
        boxCollider2D = GetComponent<BoxCollider2D>();
        PlayerAttackEvent.RegisterListener(TakeDamage);
    }

    private void TakeDamage(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
            health -= attackEvent.damage;
            //anim.SetTrigger("Hit");
            anim.Play("VoidCrystal_Hit");
            if (particles != null) {
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = transform.position;
                string[] attackCrystal = { "HitVoidCrystal1", "HitVoidCrystal2" };
                AudioPlayRandomSoundAtLocationEvent crystalHitSound = new AudioPlayRandomSoundAtLocationEvent {
                    name = attackCrystal,
                    isRandomPitch = true,
                    minPitch = 0.95f,
                    maxPitch = 1,
                    soundType = SoundType.SFX,
                    gameObject = instantiatedParticle
                };
                crystalHitSound.FireEvent();
                Destroy(instantiatedParticle, 1f);
            }

            if (health <= 0) {
                DestroyObject();
            }
        }
    }

    private void DestroyObject() {
        if (particles != null) {
            GameObject instantiatedParticle = Instantiate(particles, null);
            instantiatedParticle.transform.position = transform.position;
            string[] breakRockWall = { "VoidCrystalBreak", "VoidCrystalBreak" };
            AudioPlayRandomSoundAtLocationEvent rockBreakSound = new AudioPlayRandomSoundAtLocationEvent {
                name = breakRockWall,
                isRandomPitch = false,
                soundType = SoundType.SFX,
                gameObject = instantiatedParticle
            };
            rockBreakSound.FireEvent();
            Destroy(instantiatedParticle, 1f);
        }
        if (GameController.Instance.DestroyedVoidGenerators.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.DestroyedVoidGenerators[SceneManager.GetActiveScene().name].Contains(voidGeneratorID)) {
                Debug.LogWarning("A generator with the same ID [" + voidGeneratorID + "] has already been destroyed in this scene [" + SceneManager.GetActiveScene().name + "]");
                Destroy(gameObject);
                return;
            }
            GameController.Instance.DestroyedVoidGenerators[SceneManager.GetActiveScene().name].Add(voidGeneratorID);
        } else {
            List<int> newVoidGeneratorList = new List<int> { voidGeneratorID };
            GameController.Instance.DestroyedVoidGenerators.Add(SceneManager.GetActiveScene().name, newVoidGeneratorList);
        }
        if (voidBeamsToDestroy != null) {
            for (int i = 0; i < voidBeamsToDestroy.Length; i++) {
                Destroy(voidBeamsToDestroy[i]);
            }
        }
        
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
    }

}
