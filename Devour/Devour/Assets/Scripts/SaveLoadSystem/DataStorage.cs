﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Author: Marcus Söderberg
public class DataStorage : MonoBehaviour
{

    [SerializeField] private float saveGameInterval = 15f;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGame;
    [SerializeField] private EventSystem eventSystem;
    
    private float currentSaveGameIntervalTime;
    public PlayerData PlayerDataStorage { get; set; }
    public GameData GameData { get; set; }
    public SettingsData SettingsData { get; set; }
    public Settings Settings { get; set; }

    public string RestingScene { get; set; }

    private static DataStorage instance;

    public static DataStorage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataStorage>();
#if UNITY_EDITOR
                if (FindObjectsOfType<DataStorage>().Length > 1)
                {
                    Debug.LogError("There is more than one game controller in the scene");
                }
#endif
            }
            return instance;
        }
    }

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }
        DontDestroyOnLoad(gameObject);

        if (GameData != null)
        {
            RestingScene = GameData.RestingScene;
        }

        currentSaveGameIntervalTime = saveGameInterval;

        Settings = GetComponent<Settings>();

        MainMenuEvent.RegisterListener(OnMainMenuSwitch);
    }

    private void Update()
    {
        SaveGameInterval();
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            SaveSystem.SaveGameData(GameController.Instance);
            SaveSystem.SavePlayerData(GameController.Instance.Player);
            Debug.Log("Game Saved");
        }

    }

    public void BackToMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            SaveSystem.SaveGameData(GameController.Instance);
            SaveSystem.SavePlayerData(GameController.Instance.Player);
            Debug.Log("Game Saved");
        }

        LoadInMainMenu();
    }

    #region PlayerData

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayerData();

        if (data != null)
        {
            GameController.Instance.Player.Health = data.Health;
            GameController.Instance.Player.PlayerAbilities = data.PlayerAbilities;
            GameController.Instance.Player.Collectibles = data.Collectibles;

            for (int i = 0; i < data.TalentPoints.Count; i++)
            {
                Debug.Log("Changing Talent: " + data.TalentPoints[i].talentPointType + " : " + data.TalentPoints[i].variablesToChange[0].amount);
                TalentPointGainEvent talentGain = new TalentPointGainEvent
                {
                    talentPoint = data.TalentPoints[i]
                };
                talentGain.FireEvent();
            }

        }
    }
    #endregion;

    #region GameData
    public void LoadGameData()
    {
        GameData data = SaveSystem.LoadGameData();

        if (data != null)
        {
            GameController.Instance.SceneCheckpoint = new Vector3(data.SceneCheckpoint[0], data.SceneCheckpoint[1], data.SceneCheckpoint[2]);

            GameController.Instance.RestingCheckpoint = new Vector3(data.RestingCheckpoint[0], data.RestingCheckpoint[1], data.RestingCheckpoint[2]);

            GameController.Instance.RestingScene = data.RestingScene;

            if(data.PlayerLifeForceLocation != null)
            {
                GameController.Instance.PlayerLifeForce = new PlayerLifeForce(data.PlayerLifeForceSceneName, new Vector3(data.PlayerLifeForceLocation[0], data.PlayerLifeForceLocation[1], data.PlayerLifeForceLocation[2]), data.PlayerLifeForceCollectible);
            }

            GameController.Instance.DestroyedDestructibles = data.DestroyedDestructibles;
            GameController.Instance.DestroyedPlatforms = data.DestroyedPlatforms;
            GameController.Instance.CollectedVoidEssences = data.CollectedVoidEssences;
            GameController.Instance.DestroyedVoidGenerators = data.DestroyedVoidGenerators;
            GameController.Instance.OneTimeTips = data.OneTimeTips;
            GameController.Instance.HiddenAreasFound = data.HiddenAreasFound;
            GameController.Instance.ClearedSpawners = data.ClearedSpawners;
            GameController.Instance.KilledBosses = data.KilledBosses;
            GameController.Instance.BossIntroPlayed = data.BossIntroPlayed;

        }
    }
    #endregion;

    #region SettingsData
    public void LoadSettingsData()
    {

        SettingsData = SaveSystem.LoadSettingsData();

        if (SettingsData != null)
        {
            #region Audio Sliders
            AudioMixerVolumeEvent MasterVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.MASTER,
                volume = SettingsData.MasterVolumeSliderValue
            };
            AudioMixerVolumeEvent MusicVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.MUSIC,
                volume = SettingsData.MusicVolumeSliderValue
            };
            AudioMixerVolumeEvent SfxVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.SFX,
                volume = SettingsData.SfxVolumeSliderValue
            };
            AudioMixerVolumeEvent VoiceVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.VOICE,
                volume = SettingsData.VoiceVolumeSliderValue
            };
            MasterVolumeEvent.FireEvent();
            MusicVolumeEvent.FireEvent();
            SfxVolumeEvent.FireEvent();
            VoiceVolumeEvent.FireEvent();

            #endregion;

            Settings.DepthBlur = SettingsData.DepthBlur;
        }

        
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettingsData(Settings);
    }

    #endregion;

    IEnumerator OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SaveGame();
        }
        yield return new WaitForSeconds(3f);
        Debug.Log("Game Exit");
    }


    private void LoadInMainMenu()
    {
        PlayerDataStorage = SaveSystem.LoadPlayerData();
        GameData = SaveSystem.LoadGameData();
        SettingsData = SaveSystem.LoadSettingsData();
        LoadSettingsData();
        if(PlayerDataStorage == null)
        {
            newGame.Select();
            eventSystem.SetSelectedGameObject(newGame.gameObject);
            
            continueButton.interactable = false;
            continueButton.gameObject.SetActive(false);
        }
        if (GameData != null)
        {
            RestingScene = GameData.RestingScene;
        }
    }

    private void SaveGameInterval()
    {
        currentSaveGameIntervalTime -= Time.deltaTime;

        if (currentSaveGameIntervalTime > 0)
        {
            return;
        }

        SaveGame();
        currentSaveGameIntervalTime = saveGameInterval;
    }

    private void OnMainMenuSwitch(MainMenuEvent menuEvent)
    {
        Invoke("LoadInMainMenu", 3f);
    }

    public void EndGameReset()
    {
        PlayerDataStorage = null;
    }

    private void OnDestroy()
    {
        MainMenuEvent.UnRegisterListener(OnMainMenuSwitch);
    }

}
