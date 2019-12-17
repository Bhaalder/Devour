//Author: Patrik Ahlgren
//Återanvänd och adapterad för in-game meny av Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InGameMenuController : MonoBehaviour
{

    [SerializeField] private GameObject InGameMenuGO;

    [Header("InGameMenu")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private string mainMenuScene;
    [SerializeField] private float loadingSequenceLength;

    [Header("Options")]
    [SerializeField] private GameObject optionsGO;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button visualsButton;
    [SerializeField] private Button optionsBackButton;

    [Header("Sound")]
    [SerializeField] private GameObject soundOptionsGO;
    [SerializeField] private MenuAudioSliders audioSliders;
    [SerializeField] private Button soundBackButton;

    [Header("Visual")]
    [SerializeField] private GameObject visualOptionsGO;
    [SerializeField] private Button visualBackButton;

    private string sceneToLoad;

    private void Awake()
    {
        AddListener(resumeButton, "resumeGame");
        AddListener(optionsButton, optionsGO, InGameMenuGO);
        AddListener(mainMenuButton, "exitToMainMenu");

        AddListener(soundButton, soundOptionsGO, optionsGO);
        AddListener(soundBackButton, optionsGO, soundOptionsGO);

        AddListener(visualsButton, visualOptionsGO, optionsGO);
        AddListener(visualBackButton, optionsGO, visualOptionsGO);

        AddListener(optionsBackButton, InGameMenuGO, optionsGO);
    }

    private void OnEnable()
    {
        if (DataStorage.Instance)
        {
            DataStorage.Instance.LoadSettingsData();
        }
    }

    private void AddListener(Button button, string buttonName)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => { MenuExit(buttonName); });
        }
    }

    private void AddListener(Button button, GameObject gameObjectSetActive, GameObject gameObjectSetInactive)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => { MenuNavigation(gameObjectSetActive, gameObjectSetInactive); });
        }
    }

    private void MenuExit(string buttonName)
    {
        Debug.Log("KLICKADE PÅ " + buttonName);
        switch (buttonName)
        {
            case "resumeGame":
                InGameMenuEvent closeScreen = new InGameMenuEvent { };
                closeScreen.FireEvent();
                break;
            case "exitToMainMenu":
                sceneToLoad = mainMenuScene;
                if(DataStorage.Instance != null) {
                    DataStorage.Instance.BackToMainMenu();
                }
                Invoke("FadeScene", loadingSequenceLength);
                SwitchSceneEvent switchScene = new SwitchSceneEvent
                {
                    enteringSceneName = mainMenuScene,
                    leavingSceneName = SceneManager.GetActiveScene().name
                };
                switchScene.FireEvent();
                InGameMenuEvent mainMenu = new InGameMenuEvent { };
                mainMenu.FireEvent();
                MainMenuEvent goToMainMenuEvent = new MainMenuEvent { };
                goToMainMenuEvent.FireEvent();
                break;
        }

    }

    private void MenuNavigation(GameObject gameObjectSetActive, GameObject gameObjectSetInactive)
    {
        gameObjectSetActive.SetActive(true);
        gameObjectSetInactive.SetActive(false);
        if (gameObjectSetInactive == optionsGO)
        {
            SaveSystem.SaveSettingsData(DataStorage.Instance.Settings);
        }
        if(gameObjectSetInactive == visualOptionsGO)
        {
            visualOptionsGO.GetComponent<VisualSettings>().SaveVisualSettings();
        }
    }

    private void FadeScene()
    {
        FadeScreenEvent fadeScreen = new FadeScreenEvent
        {
            isFadeOut = true
        };
        fadeScreen.FireEvent();
        Invoke("LoadScene", 1.5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
