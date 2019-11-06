﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBlock : MonoBehaviour
{

    [SerializeField] GameObject blocker;
    [SerializeField] GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            boss.GetComponent<Bnath>().BossFightStart = true;
            blocker.SetActive(true);
        }
    }
}
