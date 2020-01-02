using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Wave
{
    //Author: Marcus Söderberg
    public GameObject[] Enemies;
    public int[] Numberofenemy;
}

public class SpawnManager : MonoBehaviour
{
    //Author: Marcus Söderberg
    [SerializeField] private int spawnerID;
    [SerializeField] private Wave[] Waves; // class to hold information per wave
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private GameObject doorOpenAudioGO;

    public float TimeBetweenEnemies = 2f;

    public int TotalEnemiesInCurrentWave { get; set; }
    public int EnemiesInWaveLeft { get; set; }
    public int SpawnedEnemies { get; set; }
    public bool StartedSpawning { get; set; }
    public bool IsRoomCleared { get => isRoomCleared; set => isRoomCleared = value; }
    public int TotalEnemiesLeft { get => totalEnemiesLeft;}

    public int CurrentWave { get; set; }
    private int totalWaves;
    private int spawnPointIndex = 0;
    private int totalEnemiesLeft;

    // Designer input
    [SerializeField] private bool isRoomCleared;
    [SerializeField] private GameObject door;
    [SerializeField] private float timeBeforeEnemySpawn;


    void Start()
    {
        if (GameController.Instance.ClearedSpawners.ContainsKey(SceneManager.GetActiveScene().name))
        {
            if (GameController.Instance.ClearedSpawners[SceneManager.GetActiveScene().name].Contains(spawnerID))
            {
                isRoomCleared = true;
                try
                {
                    door.SetActive(false);
                }
                catch (NullReferenceException) { Debug.Log("No Door To Open"); }
                return;
            }
        }
        CurrentWave = -1; // avoid off by 1
        totalWaves = Waves.Length - 1; // adjust, because we're using 0 index
        isRoomCleared = false;
        StartedSpawning = false;
        // StartNextWave(); //used for testing

        for (int i = 0; i < Waves.Length; i++)
        {
            for (int a = 0; a < Waves[i].Numberofenemy.Length; a++)
            {
                totalEnemiesLeft += Waves[i].Numberofenemy[a];
            }
        }
        Debug.Log("Enemies Left in Arena: " + totalEnemiesLeft);
    }


    public void InitializeSpawner()
    {
        StartNextWave();
        StartedSpawning = true;
    }

    void StartNextWave()
    {
        Debug.Log("Next wave started");

        CurrentWave++;

        int[] enemycount;
        enemycount = null;

        enemycount = Waves[CurrentWave].Numberofenemy;

        for (int i = 0; i < enemycount.Length; i++)
        {
            TotalEnemiesInCurrentWave += enemycount[i];
        }

        EnemiesInWaveLeft = 0;
        SpawnedEnemies = 0;
        spawnPointIndex = 0;

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int enemiesCount = Waves[CurrentWave].Enemies.Length;
        GameObject[] enemies = Waves[CurrentWave].Enemies;

        while (SpawnedEnemies < TotalEnemiesInCurrentWave)
        {

            foreach (GameObject enemy in enemies)
            {
                int place = System.Array.IndexOf(enemies, enemy);
                int numberofenemytospawn = Waves[CurrentWave].Numberofenemy[place];

                for (int i = 0; i < SpawnPoints.Length; i++)
                {
                    GameObject teleport = Instantiate(teleportEffect, SpawnPoints[i].position, Quaternion.identity);

                    Destroy(teleport, timeBeforeEnemySpawn);
                }

                yield return new WaitForSeconds(timeBeforeEnemySpawn);

                for (int i = 0; i < numberofenemytospawn; i++)
                {


                    GameObject newEnemy1 = Instantiate(enemies[place], SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
                    newEnemy1.transform.parent = gameObject.transform;

                    if (spawnPointIndex == SpawnPoints.Length - 1) { spawnPointIndex = 0; }
                    SpawnedEnemies++;
                    EnemiesInWaveLeft++;
                    spawnPointIndex++;
                    yield return new WaitForSeconds(TimeBetweenEnemies);
                }
            }

        }
        yield return null;
    }

    
    // called by an enemy when they're defeated
    public void EnemyDefeated()
    {
        EnemiesInWaveLeft--;
        totalEnemiesLeft--;
        Debug.Log("Enemies Left in Arena: " + TotalEnemiesLeft);

        // We start the next wave once we have spawned and defeated them all
        if (EnemiesInWaveLeft == 0 && SpawnedEnemies == TotalEnemiesInCurrentWave)
        {

            // Check to see if the last enemy was killed from the last wave
            if (CurrentWave == totalWaves && EnemiesInWaveLeft == 0 && SpawnedEnemies == TotalEnemiesInCurrentWave)
            {
                Debug.Log("clear condition has been reached");
                StopCoroutine(SpawnEnemies());

                try
                {
                    door.SetActive(false);
                    if(doorOpenAudioGO != null) {
                        AudioPlaySoundAtLocationEvent openDoorSound = new AudioPlaySoundAtLocationEvent {
                            name = "DoorOpen",
                            isRandomPitch = false,
                            soundType = SoundType.SFX,
                            gameObject = doorOpenAudioGO
                        };
                        openDoorSound.FireEvent();
                    }
                }
                catch (NullReferenceException) { }

                if (GameController.Instance.ClearedSpawners.ContainsKey(SceneManager.GetActiveScene().name))
                {
                    if (GameController.Instance.ClearedSpawners[SceneManager.GetActiveScene().name].Contains(spawnerID))
                    {
                        Debug.LogWarning("A Spawner with the same ID [" + spawnerID + "] has already been Cleared in this scene [" + SceneManager.GetActiveScene().name + "]");
                        isRoomCleared = true;
                        return;
                    }
                    GameController.Instance.ClearedSpawners[SceneManager.GetActiveScene().name].Add(spawnerID);
                }
                else
                {
                    List<int> newSpawnerList = new List<int> { spawnerID };
                    GameController.Instance.ClearedSpawners.Add(SceneManager.GetActiveScene().name, newSpawnerList);
                }

                isRoomCleared = true;
                return;
            }
            else {
                TotalEnemiesInCurrentWave = 0;
                StartNextWave();
            }

        }
        
    }

}