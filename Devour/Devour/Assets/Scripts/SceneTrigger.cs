//Author: Patrik Ahlgren
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

    private void Awake() {
        Destroy(transform.GetChild(0).GetComponent<SpriteRenderer>());
    }

    private void Start() {
        if (switchedScene) {
            FadeScreenEvent fadeScreen = new FadeScreenEvent {
                isFadeIn = true
            };
            fadeScreen.FireEvent();
            if (scenePointID == sceneSpawnPointID) {
                switchedScene = false;
                GameController.Instance.Player.transform.position = transform.GetChild(0).position;
                Camera.main.transform.parent.position = transform.GetChild(0).position + new Vector3(0, 0, Camera.main.transform.parent.position.z);
                GameController.Instance.SceneCheckpoint = transform.GetChild(0).position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameController.Instance.Player.IsInvulnerable = true;
            GameController.Instance.Player.UntilInvulnerableEnds = 2;
            SwitchSceneEvent switchScene = new SwitchSceneEvent {
                enteringSceneName = sceneToLoad,
                leavingSceneName = SceneManager.GetActiveScene().name
            };
            switchScene.FireEvent();
            FadeScreenEvent fadeScreen = new FadeScreenEvent {
                isFadeOut = true
            };
            fadeScreen.FireEvent();
            Invoke("SceneSwitch", 1f);      
        }
    }

    private void SceneSwitch() {
        SceneManager.LoadScene(sceneToLoad);
        scenePointID = setSceneSpawnPointID;
        switchedScene = true;
    }
}
