using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour{

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private string newGameScene;
    [SerializeField] private string loadGameScene;
    [SerializeField] private float loadingSequenceLength;

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
        newGameButton.onClick.AddListener(() => { SetSceneAndPlayAnimation("newGame"); });
        if (newGameButton != null) {
            
        }
        if (loadGameButton != null) {
            loadGameButton.onClick.AddListener(() => { SetSceneAndPlayAnimation("loadGame"); });
        }
        if (optionsButton != null) {
            optionsButton.onClick.AddListener(() => { SetSceneAndPlayAnimation("optionsButton"); });
        }
        if (exitButton != null) {
            exitButton.onClick.AddListener(() => { SetSceneAndPlayAnimation("exitButton"); });
        }
    }


    private void SetSceneAndPlayAnimation(string buttonName) {
        Debug.Log("KLICKADE PÅ " + buttonName);
        FadeScreenEvent fadeScreen = new FadeScreenEvent {
            isFadeOut = true
        };
        switch (buttonName) {
            case "newGame":
                sceneToLoad = newGameScene;
                Invoke("LoadScene", loadingSequenceLength);
                break;
            case "loadGame":
                sceneToLoad = loadGameScene;
                Invoke("LoadScene", loadingSequenceLength);
                break;
            case "optionsButton":

                break;
            case "exitButton":

                break;
        }
        fadeScreen.FireEvent();
    }

    private void LoadScene() {
        SceneManager.LoadScene(sceneToLoad);
    }

}
