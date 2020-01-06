using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScriptTemp : MonoBehaviour
{
    public GameObject lockedEnterWall;
    public GameObject lockedExitWall;
    public GameObject enemiesParent;
    void Start()
    {
        lockedEnterWall.SetActive(false);
        enemiesParent.SetActive(false);
        lockedExitWall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        try {
            if (enemiesParent.transform.childCount <= 0)
            {
                lockedExitWall.SetActive(false);
                lockedEnterWall.SetActive(false);

            }
        }
        catch(NullReferenceException e)
        {
            Debug.Log("Null Error");
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(activateArena());
        }
    }

    IEnumerator activateArena()
    {
        lockedEnterWall.SetActive(true);
        lockedExitWall.SetActive(true);

        yield return new WaitForSeconds(3);
        
        enemiesParent.SetActive(true);
    }
}
