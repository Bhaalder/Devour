﻿//Main Author: Marcus Söderberg
//Secondary Author: Patrik Ahlgren (TakeDamage(), ChangeEnemyHealth(), Start(), lade till get/set på health & damage)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    public float Health { get; set; }
    public float Damage { get; set; }
    public bool Stunned { get; set; }

    [SerializeField] private float enemyHealth;
    [SerializeField] private float damageToPlayerOnContact = 5;
    
    [SerializeField] public Rigidbody2D rb { get; set; }

    [SerializeField] private Transform enemyGFX;

    protected BoxCollider2D boxCollider2D;


    private void Start()
    {
        Health = enemyHealth;
        Damage = damageToPlayerOnContact;
        Stunned = false;
    }

    protected override void Awake(){
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        PlayerAttackEvent.RegisterListener(TakeDamage);
    }

    protected override void Update(){
        base.Update();
    }

    protected override void FixedUpdate(){
        base.FixedUpdate();
    }

    public virtual void TakeDamage(PlayerAttackEvent attackEvent){
        try {
            if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
                Vector2 knockBack;
                ChangeEnemyHealth(-attackEvent.damage);
                if (attackEvent.isMeleeAttack) {
                    PlayerHealEvent phe = new PlayerHealEvent {
                        isLifeLeech = true
                    };
                    phe.FireEvent();
                    if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                        attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                        attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                        attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                        return;
                    }
                    if (attackEvent.player.IsAttackingUp) {
                        knockBack = new Vector2(0, attackEvent.player.KnockbackForce);
                        rb.velocity = knockBack;
                        return;
                    }
                }
                knockBack = new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce, 0);
                rb.velocity = knockBack;
            }
        } catch (NullReferenceException) {

        }
    }

    public virtual void ChangeEnemyHealth(float amount) {
        Debug.Log("I took " + amount + " damage! (" + gameObject.name +")");
        Health += amount;
        if (Health <= 0) {
            EnemyDeath();
        }
    }

    public virtual void EnemyDeath()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        Destroy(gameObject);
    }

    public void setGFX(Vector3 v)
    {
        enemyGFX.localScale = v;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision is made");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("collision is player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            Stunned = true;
        }

    }

}
