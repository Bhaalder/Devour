//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Player Player { get; set; }

    public Transform Camera { get; set; }
    public bool GameIsPaused { get; set; }

    public Transform SceneCheckpoint { get; set; } //om man rör vid en "killzone"
    public Transform RestingCheckpoint { get; set; } //senaste platsen man restade på
    public Transform VoidEssenceLocation { get; set; } //platsen man dog på och måste hämta sin essence

    [SerializeField] private Transform sceneCheckpoint;//TILLFÄLLIGT

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

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Camera = UnityEngine.Camera.main.transform.parent;

        try {//TILLFÄLLIGT
            SceneCheckpoint = sceneCheckpoint;//
        } catch (System.NullReferenceException) {//
            Debug.LogWarning("GameController har ingen deklarerad checkpoint!");//
        }//
        
    }

    public void GamePaused() {
        if (GameIsPaused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }
}
