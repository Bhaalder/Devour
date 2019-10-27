using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public bool OneToTwo, TwoToOne, TwoToBoss;

    ASceneManager sceneMan;

    void Start()
    {
        sceneMan = GameObject.Find("ASceneManager").GetComponent<ASceneManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (OneToTwo)
            {
                sceneMan.LoadLevelTwo();
            }
            if (TwoToOne)
            {
                sceneMan.LoadLevelOne();
            }
            if (TwoToBoss)
            {
                sceneMan.LoadBossArea1();
            }
        }

        
    }
}
