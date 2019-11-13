﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBlock : MonoBehaviour
{

    [SerializeField] GameObject blocker;
    [SerializeField] GameObject boss;
    [SerializeField] private float timeBeforeBoss = 2f;


    private float currentCooldown;
    private bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        blocker.SetActive(false);
        currentCooldown = timeBeforeBoss;
        isTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            BossStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(blocker != null)
            {
                blocker.SetActive(true);
                isTriggered = true;
            }
        }
    }

    private void BossStart()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }
        if (boss != null)
        {
            boss.GetComponent<Bnath>().BossFightStart = true;
            boss.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
