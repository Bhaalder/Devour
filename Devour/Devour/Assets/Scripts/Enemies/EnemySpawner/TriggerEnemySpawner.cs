using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemySpawner : MonoBehaviour
{
    //Author: Teo
    public SpawnManager spawnerScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
      
            spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
            GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
