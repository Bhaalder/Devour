//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy4State
{
    NONE, IDLE, MOVEMENT, RANGE_ATTACK, HURT, DEATH
}
public class Enemy4 : Enemy
{
    public Enemy4State State { get; set; }
    public Animator Animator { get; set; }
    public bool AttackOnlyOnCanSeePlayer { get => attackOnlyOnCanSeePlayer; set => attackOnlyOnCanSeePlayer = value; }

    [SerializeField] private bool attackOnlyOnCanSeePlayer;

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();

        if (isIdle)
        {
            Transition<Enemy4Idle>();
        }
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

    protected override void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            Stunned = true;
            Transition<Enemy4Hurt>();
        }
    }
}
