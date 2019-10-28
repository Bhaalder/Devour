using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour {
    //Glöm ej att sätta ut spawnpoint vid rätt plats, det är dess child
    [Header("Scene IDs")]
    [Tooltip("The name of the scene to load, CaseSensitive")]
    [SerializeField] private string sceneToLoad;
    [Tooltip("The ID this spawnpoint has")]
    [SerializeField] private int sceneSpawnPointID;
    [Tooltip("The ID of the spawnpoint that the player is being transfered to")]
    [SerializeField] private int setSceneSpawnPointID;

    private static int scenePointID;
    private static bool switchedScene;

    private void Start() {
        if (switchedScene) {
            if (scenePointID == sceneSpawnPointID) {
                switchedScene = false;
                GameController.Instance.Player.transform.position = transform.GetChild(0).position;
                GameController.Instance.Camera.position = transform.GetChild(0).position + new Vector3(0, 0, GameController.Instance.Camera.position.z);
                GameController.Instance.SceneCheckpoint = transform.GetChild(0).position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(sceneToLoad);
            scenePointID = setSceneSpawnPointID;
            switchedScene = true;

        }
    }
}
