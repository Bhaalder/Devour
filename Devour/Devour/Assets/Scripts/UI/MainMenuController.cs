﻿using System.Collections;
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
        
        if (newGameButton != null) {
            newGameButton.onClick.AddListener(() => { SetSceneAndPlayAnimation("newGame"); });
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

        switch (buttonName) {
            case "newGame":
                sceneToLoad = newGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                break;
            case "loadGame":
                sceneToLoad = loadGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                break;
            case "optionsButton":

                break;
            case "exitButton":

                break;
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
        SceneManager.LoadScene(sceneToLoad);
    }

}
