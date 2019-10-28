using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class LevelData
{

    public float Timer { get; set; }
    public int SceneBuildIndex { get; set; }
    public int CurrentCheckpoint { get; set; }
    public int KillCount { get; set; }

    public LevelData(DataStorage data)
    {
        Timer = data.Timer;
        SceneBuildIndex = data.SceneBuildIndex;
        CurrentCheckpoint = data.CurrentCheckpoint;
        KillCount = data.KillCount;
    }
}