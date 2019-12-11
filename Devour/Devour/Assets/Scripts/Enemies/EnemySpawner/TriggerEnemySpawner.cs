using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEnemySpawner : MonoBehaviour
{
    //Author: Teo
    public SpawnManager spawnerScript;
    //public GameObject [] doors;

    public GameObject door1;


    private void Awake()
    {
        door1.SetActive(false);
    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (!spawnerScript.IsRoomCleared)
            {
                spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
                GetComponent<BoxCollider2D>().enabled = false;
                door1.SetActive(true);
            }
        }
    }
}
