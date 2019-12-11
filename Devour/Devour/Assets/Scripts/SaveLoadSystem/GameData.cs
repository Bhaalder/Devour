using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class GameData
{
    public float[] SceneCheckpoint { get; set; } //om man rör vid en "killzone"
    public float[] RestingCheckpoint { get; set; } //senaste platsen man restade på
    public string RestingScene { get; set; } //senaste scenen man restade på    
    public float[] PlayerLifeForceLocation { get; set; } //platsen man dog på och måste hämta sin lifeForce
    public string PlayerLifeForceSceneName { get; set; }
    public Collectible PlayerLifeForceCollectible { get; set; }

    public Dictionary<string, List<int>> DestroyedDestructibles { get; set; }
    public Dictionary<string, List<int>> DestroyedPlatforms { get; set; }
    public Dictionary<string, List<int>> CollectedVoidEssences { get; set; }
    public Dictionary<string, List<int>> DestroyedVoidGenerators { get; set; }
    public Dictionary<string, List<int>> OneTimeTips { get; set; }
    public Dictionary<string, List<int>> HiddenAreasFound { get; set; }
    public Dictionary<string, List<int>> ClearedSpawners { get; set; }
    public List<string> KilledBosses { get; set; }
    public List<string> BossIntroPlayed { get; set; }


    public GameData(GameController gameController)
    {
        SceneCheckpoint = new float[3];
        SceneCheckpoint[0] = gameController.SceneCheckpoint.x;
        SceneCheckpoint[1] = gameController.SceneCheckpoint.y;
        SceneCheckpoint[2] = gameController.SceneCheckpoint.z;

        RestingCheckpoint = new float[3];
        RestingCheckpoint[0] = gameController.RestingCheckpoint.x;
        RestingCheckpoint[1] = gameController.RestingCheckpoint.y;
        RestingCheckpoint[2] = gameController.RestingCheckpoint.z;

        if(gameController.PlayerLifeForce != null)
        {
            PlayerLifeForceLocation = new float[3];
            PlayerLifeForceLocation[0] = gameController.PlayerLifeForce.Location.x;
            PlayerLifeForceLocation[1] = gameController.PlayerLifeForce.Location.y;
            PlayerLifeForceLocation[2] = gameController.PlayerLifeForce.Location.z;

            PlayerLifeForceSceneName = gameController.PlayerLifeForce.SceneName;

            PlayerLifeForceCollectible = gameController.PlayerLifeForce.Collectible;
        }

        RestingScene = gameController.RestingScene;

        DestroyedDestructibles = gameController.DestroyedDestructibles;
        DestroyedPlatforms = gameController.DestroyedPlatforms;
        CollectedVoidEssences = gameController.CollectedVoidEssences;
        DestroyedVoidGenerators = gameController.DestroyedVoidGenerators;
        OneTimeTips = gameController.OneTimeTips;
        HiddenAreasFound = gameController.HiddenAreasFound;
        ClearedSpawners = gameController.ClearedSpawners;
        KilledBosses = gameController.KilledBosses;
        BossIntroPlayed = gameController.BossIntroPlayed;
        
    }
}
