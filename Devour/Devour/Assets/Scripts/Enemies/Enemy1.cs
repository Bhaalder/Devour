using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy1State
{
    NONE, IDLE, MOVEMENT, HURT, DEATH
}
public class Enemy1 : Enemy
{
    public Enemy1State State { get; set; }
    public Animator Animator { get; set; }

    protected override void Awake() {
        base.Awake();
        Animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        //Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            Stunned = true;
            Transition<Enemy1Hurt>();
        }
    }
}
