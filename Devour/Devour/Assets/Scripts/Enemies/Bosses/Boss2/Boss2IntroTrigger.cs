using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2IntroTrigger : MonoBehaviour
{

    [SerializeField] private GameObject boss;
    [SerializeField] GameObject blocker;
    [SerializeField] private float timeBeforeBoss = 2f;

    private float currentCooldown;
    private bool isTriggered;

    private void Start()
    {
        currentCooldown = timeBeforeBoss;
        isTriggered = false;
        boss.SetActive(true);
        if (boss.GetComponent<Boss2>().IsAlive)
        {
            blocker.SetActive(true);
        }
    }
    private void Update()
    {
        if (isTriggered)
        {
            BossStart();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (blocker != null)
            {
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
            boss.SetActive(true);
            boss.GetComponent<Boss2>().IntroStarted = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
