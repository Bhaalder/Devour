using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class PlayerData
{
    public List<PlayerAbility> PlayerAbilities { get; set; }
    public List<TalentPoint> TalentPoints { get; set; }
    public List<Collectible> Collectibles { get; set; }

    public PlayerData(Player playerData)
    {
        PlayerAbilities = playerData.PlayerAbilities;
        TalentPoints = playerData.TalentPoints;
        Collectibles = playerData.Collectibles;
    }
}