using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemySpawner : MonoBehaviour
{
    //Author: Teo
    public SpawnManager spawnerScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractionPlayer"))
        {
      
            spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
            Destroy(gameObject);

        }

    }
}
