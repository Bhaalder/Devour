//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakableFloor : MonoBehaviour {

    [SerializeField] private GameObject particles;
    [SerializeField] private int platformID;

    private void Start() {
        if (GameController.Instance.DestroyedPlatforms.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> platform in GameController.Instance.DestroyedPlatforms) {
                if (platform.Key == SceneManager.GetActiveScene().name) {
                    if (platform.Value.Contains(platformID)) {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player") {
            TriggerFloor();
        }
    }

    private void TriggerFloor() {
        GameObject instantiatedParticle = Instantiate(particles, transform.position, Quaternion.identity);
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
        if (GameController.Instance.DestroyedPlatforms.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> platform in GameController.Instance.DestroyedPlatforms) {
                if (platform.Key == SceneManager.GetActiveScene().name) {
                    if (platform.Value.Contains(platformID)) {
                        Debug.LogWarning("A platform with the same ID [" + platformID + "] has already been destroyed in this scene [" + SceneManager.GetActiveScene().name + "]");
                        Destroy(gameObject);
                        return;
                    }
                    platform.Value.Add(platformID);
                }
            }
        } else {
            List<int> newPlatformList = new List<int> { platformID };
            GameController.Instance.DestroyedPlatforms.Add(SceneManager.GetActiveScene().name, newPlatformList);
        }
        Destroy(gameObject);
    }
}
