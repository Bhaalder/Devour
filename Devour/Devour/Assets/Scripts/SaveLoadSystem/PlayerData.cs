using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class PlayerData
{
    public float PlayerHP { get; set; }
    public float[] PlayerPosition { get; set; }

    public PlayerData(GameController gameController)
    {

    }
}
