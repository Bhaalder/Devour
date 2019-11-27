using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy5State
{
    NONE, INTRO, IDLE, ATTACK, CLIMBING, DASH_TELEGRAPH, DASHING, VOID_ASSAULT, DEATH, MOVEMENT, HURT
}
public class Enemy5 : Enemy
{
    public Enemy5State State { get; set; }
    public Animator Animator { get; set; }
    public bool jumpCollision { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        jumpCollision = false;
    }

    protected override void Update()
    {
        base.Update();
        Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerStay2D(Collider2D collision)
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
            Transition<Enemy5Hurt>();
        }
    }

}
