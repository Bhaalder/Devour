﻿//Author: Patrik Ahlgren
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
    [SerializeField] private Button visualsButton;

    [Header("Sound")]
    [SerializeField] private GameObject soundOptionsGO;
    [SerializeField] private MenuAudioSliders audioSliders;
    [SerializeField] private Button soundBackButton;

    [Header("Animations")]
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private Animator buttonAnim;

    [Header("Visual")]
    [SerializeField] private GameObject visualOptionsGO;
    [SerializeField] private Button visualBackButton;

    private string sceneToLoad;

    private void Awake() {
        try {
            if (GameController.Instance.gameObject) {

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
        AddListener(visualsButton, visualOptionsGO, optionsGO);
        AddListener(visualBackButton, optionsGO, visualOptionsGO);

    }


    private void Start()
    {
        if (DataStorage.Instance)
        {
            DataStorage.Instance.BackToMainMenu();
        }
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
                SaveSystem.NewGame();
                DataStorage.Instance.GameDataReset();
                DataStorage.Instance.isNewGame = true;
                sceneToLoad = newGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                cameraAnim.Play("GameStartAnim");
                buttonAnim.Play("MenuAnim");
                SwitchSceneEvent switchScene = new SwitchSceneEvent {
                    enteringSceneName = newGameScene,
                    leavingSceneName = SceneManager.GetActiveScene().name
                };
                switchScene.FireEvent();
                PlaySound("StartGameClick");
                break;
            case "loadGame":
                DataStorage.Instance.isNewGame = false;
                if (DataStorage.Instance.RestingScene != null)
                {
                    loadGameScene = DataStorage.Instance.RestingScene;
                }
                else
                {
                    loadGameScene = newGameScene;
                }
                sceneToLoad = loadGameScene;
                Invoke("FadeScene", loadingSequenceLength);
                
                cameraAnim.Play("GameStartAnim");
                buttonAnim.Play("MenuAnim");
                SwitchSceneEvent switchSceneLoad = new SwitchSceneEvent
                {
                    enteringSceneName = loadGameScene,
                    leavingSceneName = SceneManager.GetActiveScene().name
                };
                switchSceneLoad.FireEvent();
                PlaySound("StartGameClick");
                break;
            case "exitButton":
                Application.Quit();
                break;
        }
        
    }

    private void MenuNavigation(GameObject gameObjectSetActive, GameObject gameObjectSetInactive) {
        gameObjectSetActive.SetActive(true);
        gameObjectSetInactive.SetActive(false);
        //if(gameObjectSetInactive == soundOptionsGO)
        //{
        //    SaveSystem.SaveSettingsData(DataStorage.Instance.Settings);
        //}
        if (gameObjectSetInactive == visualOptionsGO)
        {
            visualOptionsGO.GetComponent<VisualSettings>().SaveVisualSettings();
        }
        if (gameObjectSetInactive == optionsGO)
        {
            DataStorage.Instance.SaveSettings();
        }
        PlaySound("ButtonClick");
    }

    private void PlaySound(string soundName) {
        AudioPlaySoundEvent playSound = new AudioPlaySoundEvent {
            name = soundName,
            isRandomPitch = false,
            soundType = SoundType.SFX
        };
        playSound.FireEvent();
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
