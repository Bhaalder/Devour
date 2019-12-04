//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour{

    [Header("MainMenu")]
    [SerializeField] private GameObject mainMenuGO;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private string newGameScene;
    [SerializeField] private string loadGameScene;
    [SerializeField] private float loadingSequenceLength;

    [Header("Options")]
    [SerializeField] private GameObject optionsGO;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button backButton;

    [Header("Sound")]
    [SerializeField] private GameObject soundOptionsGO;
    [SerializeField] private MenuAudioSliders audioSliders;
    [SerializeField] private Button soundBackButton;

    [Header("Animations")]
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private Animator buttonAnim;

    private string sceneToLoad;

    private void Awake() {
        try {
            if (GameController.Instance.gameObject) {
                Destroy(GameController.Instance.Player.gameObject);
                Destroy(GameController.Instance.Canvas.gameObject);
                Destroy(GameController.Instance.gameObject);
            }
        } catch (System.NullReferenceException) {

        }
        
        AddListener(newGameButton, "newGame");
        AddListener(loadGameButton, "loadGame");
        AddListener(optionsButton, optionsGO, mainMenuGO);
        AddListener(exitButton, "exitButton");
        AddListener(soundButton, soundOptionsGO, optionsGO);
        AddListener(soundBackButton, optionsGO, soundOptionsGO);
        AddListener(backButton, mainMenuGO, optionsGO);
    }

    private void AddListener(Button button, string buttonName) {
        if(button != null) {
            button.onClick.AddListener(() => { SetSceneAndPlayAnimation(buttonName); });
        }
    }

    private void AddListener(Button button, GameObject gameObjectSetActive, GameObject gameObjectSetInactive) {
        if (button != null) {
            button.onClick.AddListener(() => { MenuNavigation(gameObjectSetActive, gameObjectSetInactive); });
        }
    }

    private void SetSceneAndPlayAnimation(string buttonName) {
        Debug.Log("KLICKADE PÅ " + buttonName);
        switch (buttonName) {
            case "newGame":
                sceneToLoad = newGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                cameraAnim.Play("GameStartAnim");
                buttonAnim.Play("MenuAnim");
                SwitchSceneEvent switchScene = new SwitchSceneEvent {
                    enteringSceneName = newGameScene,
                    leavingSceneName = SceneManager.GetActiveScene().name
                };
                switchScene.FireEvent();
                break;
            case "loadGame":
                sceneToLoad = loadGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                break;
            case "exitButton":

                break;
        }
        
    }

    private void MenuNavigation(GameObject gameObjectSetActive, GameObject gameObjectSetInactive) {
        gameObjectSetActive.SetActive(true);
        gameObjectSetInactive.SetActive(false);
        if(gameObjectSetInactive == soundOptionsGO)
        {
            SaveSystem.SaveSettingsData(FindObjectOfType<Settings>());
        }
    }

    private void FadeScene() {
        FadeScreenEvent fadeScreen = new FadeScreenEvent {
            isFadeOut = true
        };
        fadeScreen.FireEvent();
        Invoke("LoadScene", 1.5f);
    }

    private void LoadScene() {
        //TEMPORARY TEST FIX
        if(DataStorage.Instance.RestingScene == null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(DataStorage.Instance.RestingScene);
        }
    }

}
