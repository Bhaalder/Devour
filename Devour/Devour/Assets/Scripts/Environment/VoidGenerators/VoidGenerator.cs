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

    private void Start() {
        if (GameController.Instance.DestroyedVoidGenerators.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> voidGenerator in GameController.Instance.DestroyedVoidGenerators) {
                if (voidGenerator.Key == SceneManager.GetActiveScene().name) {
                    if (voidGenerator.Value.Contains(voidGeneratorID)) {
                        if (voidBeamsToDestroy != null) {
                            foreach (GameObject voidBeam in voidBeamsToDestroy) {
                                Destroy(voidBeam);
                            }
                        }
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
        boxCollider2D = GetComponent<BoxCollider2D>();
        PlayerAttackEvent.RegisterListener(TakeDamage);
    }

    private void TakeDamage(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
            health -= attackEvent.damage;

            if (particles != null) {
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = transform.position;
                AudioPlaySoundAtLocationEvent rockAttackSound = new AudioPlaySoundAtLocationEvent {
                    name = "HitRockWall",
                    isRandomPitch = true,
                    minPitch = 0.95f,
                    maxPitch = 1,
                    soundType = SoundType.SFX,
                    gameObject = instantiatedParticle
                };
                rockAttackSound.FireEvent();
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
            string[] breakRockWall = { "BreakRockWall1", "BreakRockWall1" };
            AudioPlayRandomSoundAtLocationEvent rockBreakSound = new AudioPlayRandomSoundAtLocationEvent {
                name = breakRockWall,
                isRandomPitch = true,
                minPitch = 0.95f,
                maxPitch = 1,
                soundType = SoundType.SFX,
                gameObject = instantiatedParticle
            };
            rockBreakSound.FireEvent();
        }
        if (GameController.Instance.DestroyedVoidGenerators.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> voidGenerator in GameController.Instance.DestroyedVoidGenerators) {
                if (voidGenerator.Key == SceneManager.GetActiveScene().name) {
                    if (voidGenerator.Value.Contains(voidGeneratorID)) {
                        Debug.LogWarning("A generator with the same ID [" + voidGeneratorID + "] has already been destroyed in this scene [" + SceneManager.GetActiveScene().name + "]");
                        Destroy(gameObject);
                        return;
                    }
                    voidGenerator.Value.Add(voidGeneratorID);
                }
            }
        } else {
            List<int> newVoidGeneratorList = new List<int> { voidGeneratorID };
            GameController.Instance.DestroyedVoidGenerators.Add(SceneManager.GetActiveScene().name, newVoidGeneratorList);
        }
        if(voidBeamsToDestroy != null) {
            foreach(GameObject voidBeam in voidBeamsToDestroy) {
                Destroy(voidBeam);
            }
        }
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
    }

}
