using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Marcus Söderberg
public class DataStorage : MonoBehaviour
{
    public float Timer { get; set; }
    public int SceneBuildIndex { get; set; }
    public int CurrentCheckpoint { get; set; }
    public int KillCount { get; set; }

    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy3;
    [SerializeField] private GameObject Enemy4;

    private List<EnemyData> EnemiesStorage { get; set; } = new List<EnemyData>();
    public PlayerData PlayerDataStorage { get; set; }
    public LevelData levelDataStorage { get; set; }

    public bool NewGame { get; set; }


    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            LoadGameData();
        }
        else
        {
            LoadInMainMenu();
        }

        NewGame = false;
        DontDestroyOnLoad(gameObject);
    }

    #region PlayerData

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if(data != null)
        {
            //Load Values from file
        }
    }
    #endregion;

    #region EnemyData
    public void SaveEnemyData()
    {
        SaveSystem.DeleteEnemySaveFile();
        try
        {
            ClearEnemyList();
        }
        catch (System.Exception e)
        {
            Debug.Log("Could not clear DataStorage.EnemiesStorage: " + e);
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in gameObjects)
        {
            //Save enemies to list here
            //EnemyData data = new EnemyData(target.GetComponent<Enemy>().SaveEnemyData());
            //EnemiesStorage.Add(data);
            //Debug.Log("Enemy Saved");
        }

        SaveSystem.WriteEnemyDataToFile(EnemiesStorage);
    }

    public void LoadEnemyData()
    {
        EnemiesStorage = SaveSystem.LoadEnemies();

        if (EnemiesStorage != null)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject target in gameObjects)
            {
                Destroy(target);
            }

            foreach (EnemyData enemyData in EnemiesStorage)
            {
                //load enemies from save
            }
        }
    }

    public void ClearEnemyList()
    {
        EnemiesStorage.Clear();
    }
    #endregion

    #region LevelData
    public void SaveLevelData()
    {
        SaveSystem.SaveLevelData(this);
    }
    public void LoadLevelData()
    {
        LevelData data = SaveSystem.LoadLevelData();

        if (data != null)
        {
            //load level variables here
        }
    }
    #endregion

    public void SaveGame()
    {
        SaveEnemyData();
        SaveLevelData();
    }

    public void LoadGameData()
    {
        LoadLevelData();
        LoadEnemyData();
    }

    public void LoadLastLevelData()
    {
        LevelData data = SaveSystem.LoadLevelData();
        if (data != null)
        {
            SceneBuildIndex = data.SceneBuildIndex;
        }
    }

    IEnumerator OnApplicationQuit()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            SaveGame();
        }
        yield return new WaitForSeconds(3f);
        Debug.Log("Game Exit");
    }

    private void LoadInMainMenu()
    {
        EnemiesStorage = SaveSystem.LoadEnemies();
        levelDataStorage = SaveSystem.LoadLevelData();
        PlayerDataStorage = SaveSystem.LoadPlayer();
    }
}
