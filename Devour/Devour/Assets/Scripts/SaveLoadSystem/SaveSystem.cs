using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Author: Marcus Söderberg
public static class SaveSystem
{

    private static string playerDataString = "/playerdata.sav";
    private static string enemyDataString = "/enemydata.sav";
    private static string levelDataString = "/leveldata.sav";
    private static string[] allpaths = new string[] { playerDataString,enemyDataString,levelDataString};
    private static List<EnemyData> dummyEnemyDataList = new List<EnemyData>();

    #region PlayerData
    public static void SavePlayer(GameController gameController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + playerDataString;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    #region EnemyData

    public static void WriteEnemyDataToFile(List<EnemyData> enemies)
    {

        DeleteEnemySaveFile();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + enemyDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, enemies);
        stream.Close();

        GameObject.FindObjectOfType<DataStorage>().ClearEnemyList();
    }

    public static List<EnemyData> LoadEnemies()
    {

        string path = Application.persistentDataPath + enemyDataString;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<EnemyData> enemies = formatter.Deserialize(stream) as List<EnemyData>;

            stream.Close();

            return enemies;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return dummyEnemyDataList;
        }
    }
    public static void DeleteEnemySaveFile()
    {
        string path = Application.persistentDataPath + enemyDataString;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

    }
    #endregion

    #region LevelData
    public static void SaveLevelData(DataStorage levelData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + levelDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levelData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevelData()
    {
        string path = Application.persistentDataPath + levelDataString;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    public static void DeleteAllSaveFiles()
    {
        foreach (string savepath in allpaths)
        {
            string path = Application.persistentDataPath + savepath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

}
