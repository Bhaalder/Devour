//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2SonicSnipeBeam : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private float beamDamage;

    private void Start()
    {
        beamDamage = boss.GetComponent<Boss2>().SonicSnipeBeamDamage;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = beamDamage,
                enemyPosition = transform.position
            };
            ptde.FireEvent();
        }
    }
}
