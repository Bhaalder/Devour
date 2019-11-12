using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2SonicSnipeBeam : Boss2
{
    private float beamDamage;

    protected override void Awake()
    {
        beamDamage = SonicSnipeBeamDamage;
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
