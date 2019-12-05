using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Author: Marcus Söderberg
public static class SaveSystem
{

    private static string gameDataString = "/GameData.sav";
    private static string settingsDataString = "/SettingsData.sav";
    private static string playerDataString = "/playerData.sav";
    private static string[] allpaths = new string[] { gameDataString, settingsDataString, playerDataString };

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
    public static void SaveGame()
    {
        SaveGameData(GameController.Instance);
        SavePlayerData(GameController.Instance.Player);
    }

    #region GameData
    public static void SaveGameData(GameController gameController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + gameDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + gameDataString;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion;


    #region PlayerData

    public static void SavePlayerData(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadPlayerData()
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
    #endregion;

    #region SettingsData

    public static void SaveSettingsData(Settings settingsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + settingsDataString;
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(settingsData);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Settings Saved");
    }

    public static SettingsData LoadSettingsData()
    {
        string path = Application.persistentDataPath + settingsDataString;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion;
}
