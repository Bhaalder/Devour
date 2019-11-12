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
            Debug.Log("TRIGGERED");
            boss.GetComponent<Boss2>().IntroStarted = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
