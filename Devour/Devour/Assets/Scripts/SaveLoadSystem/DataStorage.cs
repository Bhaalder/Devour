using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Marcus Söderberg
public class DataStorage : MonoBehaviour
{
    public int KillCount { get; set; }


    public PlayerData PlayerDataStorage { get; set; }
    public GameData GameData { get; set; }

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
        }
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadInMainMenu();
        }

        if(GameData != null)
        {
            RestingScene = GameData.RestingScene;
        }

    }

    private void Update()
    {
        SaveGame();
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

    #region PlayerData

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayerData();

        if (data != null)
        {
            GameController.Instance.Player.PlayerAbilities = data.PlayerAbilities;

            //GameController.Instance.Player.MaxHealth = data.MaxHealth;
            //GameController.Instance.Player.Health = data.Health;
            //GameController.Instance.Player.MaxPlayerVoid = data.MaxPlayerVoid;
            //GameController.Instance.Player.PlayerVoid = data.PlayerVoid;
            //GameController.Instance.Player.DamageReduction = data.DamageReduction;
            //GameController.Instance.Player.MeleeDamage = data.MeleeDamage;
            //GameController.Instance.Player.ProjectileDamage = data.ProjectileDamage;
            //GameController.Instance.Player.KnockbackForce = data.KnockbackForce;
            //GameController.Instance.Player.BounceForce = data.BounceForce;
            //GameController.Instance.Player.MeleeCooldown = data.MeleeCooldown;
            //GameController.Instance.Player.UntilNextMeleeAttack = data.UntilNextMeleeAttack;
            //GameController.Instance.Player.MeleeLifeLeech = data.MeleeLifeLeech;
            //GameController.Instance.Player.MeleeVoidLeech = data.MeleeVoidLeech;
            //GameController.Instance.Player.ProjectileCooldown = data.ProjectileCooldown;
            //GameController.Instance.Player.UntilNextProjectileAttack = data.UntilNextProjectileAttack;
            //GameController.Instance.Player.ProjectileHealthcost = data.ProjectileHealthcost;

            //GameController.Instance.Player.ExtraJumps = data.ExtraJumps;
            GameController.Instance.Player.MovementSpeed = data.MovementSpeed;
            GameController.Instance.Player.DashCooldown = data.DashCooldown;

            GameController.Instance.Player.TalentMeleeDamage = data.TalentMeleeDamage;
            GameController.Instance.Player.TalentProjectileDamage = data.TalentProjectileDamage;
            GameController.Instance.Player.TalentHealth = data.TalentHealth;
            GameController.Instance.Player.TalentLifeLeech = data.TalentLifeLeech;
            GameController.Instance.Player.TalentMovementSpeed = data.TalentMovementSpeed;
            GameController.Instance.Player.TalentDashCooldown = data.TalentDashCooldown;
            GameController.Instance.Player.TalentPlayerVoid = data.TalentPlayerVoid;
            GameController.Instance.Player.TalentVoidLeech = data.TalentVoidLeech;

            GameController.Instance.Player.TalentPoints = data.TalentPoints;
            GameController.Instance.Player.Collectibles = data.Collectibles;

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

            //PlayerLifeForce = gameController.PlayerLifeForce;

            GameController.Instance.DestroyedDestructibles = data.DestroyedDestructibles;
            GameController.Instance.DestroyedPlatforms = data.DestroyedPlatforms;
            GameController.Instance.CollectedVoidEssences = data.CollectedVoidEssences;
            GameController.Instance.DestroyedVoidGenerators = data.DestroyedVoidGenerators;
            GameController.Instance.OneTimeTips = data.OneTimeTips;
            GameController.Instance.HiddenAreasFound = data.HiddenAreasFound;
            GameController.Instance.KilledBosses = data.KilledBosses;

        }
    }
    #endregion;

    #region SettingsData
    public void LoadSettingsData()
    {

        SettingsData settingsData = SaveSystem.LoadSettingsData();

        if (settingsData != null)
        {
            #region Audio Sliders
            AudioMixerVolumeEvent MasterVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.MASTER,
                volume = settingsData.MasterVolumeSliderValue
            };
            AudioMixerVolumeEvent MusicVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.MUSIC,
                volume = settingsData.MusicVolumeSliderValue
            };
            AudioMixerVolumeEvent SfxVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.SFX,
                volume = settingsData.SfxVolumeSliderValue
            };
            AudioMixerVolumeEvent VoiceVolumeEvent = new AudioMixerVolumeEvent
            {
                soundMixerType = SoundMixerType.VOICE,
                volume = settingsData.VoiceVolumeSliderValue
            };
            MasterVolumeEvent.FireEvent();
            MusicVolumeEvent.FireEvent();
            SfxVolumeEvent.FireEvent();
            VoiceVolumeEvent.FireEvent();

            #endregion;
        }
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
        LoadSettingsData();
    }

}
