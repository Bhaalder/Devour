using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2IntroTrigger : MonoBehaviour
{

    [SerializeField] private GameObject boss;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            boss.GetComponent<Boss2>().IntroStarted = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
