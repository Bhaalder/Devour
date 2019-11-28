using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemySpawner : MonoBehaviour
{
    //Author: Teo
    public SpawnManager spawnerScript;
    //public GameObject [] doors;

    public GameObject door1;


    private void Awake()
    {

        //for (int i = 0; i > doors.Length; i++)
        //{
        //    doors[i].SetActive(false);
        //}

        door1.SetActive(false);
        
    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
      
            spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
            GetComponent<BoxCollider2D>().enabled = false;


            //for (int i=0; i > doors.Length; i++)
            //    {
            //    doors[i].SetActive(true);
            //    }

            door1.SetActive(true);
            
        }

    }
}
