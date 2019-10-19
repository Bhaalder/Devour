﻿//Main Author: Marcus Söderberg
//Secondary Author: Patrik Ahlgren (TakeDamage)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    [SerializeField] private float enemyHealth;
    private CircleCollider2D circleCollider2D;
    [SerializeField] public Rigidbody2D rb { get; set; }

    [SerializeField] private Transform enemyGFX;


    void Start()
    {

    }
    protected override void Awake(){
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        PlayerAttackEvent.RegisterListener(TakeDamage);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void TakeDamage(PlayerAttackEvent attackEvent){
        try {
            if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {           
                ChangeEnemyHealth(-attackEvent.damage);
                if (attackEvent.isMeleeAttack) {
                    PlayerHealEvent phe = new PlayerHealEvent {
                        isLifeLeech = true
                    };
                    phe.FireEvent();
                }
                if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                    attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                    attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                    attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 15);
                }
                if (!attackEvent.player.IsAttackingDown) {
                    Vector2 knockBack = new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce, 0);
                    rb.velocity = knockBack;
                }
                if (attackEvent.player.IsAttackingUp) {
                    Vector2 knockBack = new Vector2(0, attackEvent.player.KnockbackForce);
                    rb.velocity = knockBack;
                }
            }
        } catch (NullReferenceException) {

        }
    }

    public void ChangeEnemyHealth(float amount) {
        Debug.Log("I took damage! " + gameObject.name);
        enemyHealth += amount;
        if (enemyHealth <= 0) {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        Destroy(gameObject);
    }

    public void setGFX(Vector3 v)
    {
        enemyGFX.localScale = v;
    }

}
