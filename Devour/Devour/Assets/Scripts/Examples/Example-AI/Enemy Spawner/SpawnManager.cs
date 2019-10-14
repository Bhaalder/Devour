using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] private Wave[] Waves; // class to hold information per wave
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private GameObject teleportEffect;
    public float TimeBetweenEnemies = 2f;

    public int TotalEnemiesInCurrentWave { get; set; }
    public int EnemiesInWaveLeft { get; set; }
    public int SpawnedEnemies { get; set; }
    public bool StartedSpawning { get; set; }


    public int CurrentWave { get; set; }
    private int totalWaves;
    private int spawnPointIndex = 0;

    public int SpawnerInstancedID { get; set; }

    // Designer input
    [SerializeField] private bool isRoomCleared;
    [SerializeField] private bool isArenaSpawner;
    [SerializeField] private bool isLevel2;
    [SerializeField] private bool isCommandoRoom;
    [SerializeField] private GameObject door;


    void Start()
    {
        CurrentWave = -1; // avoid off by 1
        totalWaves = Waves.Length - 1; // adjust, because we're using 0 index
        isRoomCleared = false;
        StartedSpawning = false;
        // StartNextWave(); //used for testing
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

        StartCoroutine(SpawnEnemies());
    }

    // Coroutine to spawn all of our enemies
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

                for (int i = 0; i < numberofenemytospawn; i++)
                {
                    SpawnedEnemies++;
                    EnemiesInWaveLeft++;
                    spawnPointIndex++;

                    GameObject newEnemy1 = Instantiate(enemies[place], SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
                    newEnemy1.transform.parent = gameObject.transform;
                    GameObject teleport = Instantiate(teleportEffect, newEnemy1.transform.position, Quaternion.identity);
                    AudioController.Instance.Play_InWorldspace("Teleport", newEnemy1, 0.95f, 1.05f);
                    try
                    {
                        newEnemy1.GetComponent<Enemy>().ParentID = gameObject.GetInstanceID();
                    }
                    catch (Exception e)
                    {
                        newEnemy1.GetComponent<Enemy>().ParentID = 0;
                    }
                    if (spawnPointIndex == SpawnPoints.Length - 1) { spawnPointIndex = 0; }
                    Destroy(teleport, 3f);
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

        // We start the next wave once we have spawned and defeated them all
        if (EnemiesInWaveLeft == 0 && SpawnedEnemies == TotalEnemiesInCurrentWave)
        {

            // Check to see if the last enemy was killed from the last wave
            if (CurrentWave == totalWaves && EnemiesInWaveLeft == 0 && SpawnedEnemies == TotalEnemiesInCurrentWave)
            {
                Debug.Log("clear condition has been reached");
                StopCoroutine(SpawnEnemies());
                if (isArenaSpawner)
                {
                    GameController.Instance.SceneCompletedSequence(true);
                    if (isLevel2) {
                        AudioController.Instance.FadeOut("Song4Loop", 5, 0);
                        door.GetComponent<Animator>().SetBool("isOpen", true);
                        AudioController.Instance.Play_InWorldspace("Alarm", door);
                    } else {
                        AudioController.Instance.FadeOut("Song3Loop", 5, 0);
                    }
                    
                    

                    try {
                        door.SetActive(false);
                    } catch (NullReferenceException) {

                    }
                    
                }
                if (isCommandoRoom)
                {
                    door.SetActive(false);
                    AudioController.Instance.FadeOut("Song2Loop", 5, 0);
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

    public void SaveSpawnerData()
    {
        FindObjectOfType<DataStorage>().SaveSpawnerData(this);
    }

    private IEnumerator CheckForChildren()
    {
        yield return new WaitForSeconds(3f);
        if(transform.childCount <= 0)
        {
            EnemiesInWaveLeft = 0;
        }
        if(StartedSpawning == true && totalWaves > CurrentWave)
        {
            StartNextWave();
        }
    }

}