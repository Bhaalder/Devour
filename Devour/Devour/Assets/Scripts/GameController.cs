//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Player Player { get; set; }
    public Transform Canvas { get; set; }
    public bool GameIsPaused { get; set; }

    public Vector3 SceneCheckpoint { get; set; } //om man rör vid en "killzone"
    public string RestingScene { get; set; } //senaste scenen man restade på
    public Vector3 RestingCheckpoint { get; set; } //senaste platsen man restade på
    public Vector3 VoidEssenceLocation { get; set; } //platsen man dog på och måste hämta sin essence

    public Dictionary<string, List<int>> DestroyedDestructibles { get; set; }
    public Dictionary<string, List<int>> DestroyedPlatforms { get; set; }
    public Dictionary<string, List<int>> CollectedVoidEssences { get; set; }
    
    private static GameController instance;

    public static GameController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameController>();
#if UNITY_EDITOR
                if (FindObjectsOfType<GameController>().Length > 1) {
                    Debug.LogError("There is more than one game controller in the scene");
                }
#endif
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
        }
        DontDestroyOnLoad(gameObject);

        DestroyedDestructibles = new Dictionary<string, List<int>>();
        CollectedVoidEssences = new Dictionary<string, List<int>>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (SceneCheckpoint == null) {
            SceneCheckpoint = Player.transform.position;
        }
        if (RestingCheckpoint == null) {
            SceneCheckpoint = Player.transform.position;
        }
        if (RestingScene == null) {
            RestingScene = SceneManager.GetActiveScene().name;
        }
        PlayerDiedEvent.RegisterListener(OnPlayerDied);
    }

    private void OnPlayerDied(PlayerDiedEvent diedEvent) {
        FadeScreenEvent fadeScreen = new FadeScreenEvent {
            isFadeIn = true
        };
        fadeScreen.FireEvent();
        try {
            if(RestingCheckpoint != null) {
                Debug.Log("RESPAWN SUCCESS! " + RestingScene);
                SceneManager.LoadScene(RestingScene);
                diedEvent.player.transform.position = RestingCheckpoint;
                return;
            }
        } catch (UnassignedReferenceException) {
            Debug.LogError("No 'RestingCheckpoint' assigned in GameController to be able to respawn after death! Spawning at SceneCheckpoint...");
        }
        diedEvent.player.transform.position = SceneCheckpoint;
    }

    public void GamePaused(bool gameIsPaused) {
        if (gameIsPaused) {
            GameIsPaused = true;
            Time.timeScale = 0f;
        } else {
            GameIsPaused = false;
            Time.timeScale = 1f;
        }
    }

    private void OnDestroy() {
        PlayerDiedEvent.UnRegisterListener(OnPlayerDied);
    }

}
