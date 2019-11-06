﻿//Main Author: Marcus Söderberg
//Secondary Author: Patrik Ahlgren (TakeDamage(), ChangeEnemyHealth(), DeathSound(), HurtSound(), invulnerability, lade till get/set på health & damage, lade till Player)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    public Player Player { get; set; }
    public float Health { get; set; }
    public float Damage { get; set; }
    public bool Stunned { get; set; }
    public bool IsAlive { get; set; }
    public GameObject[] ChildrenToDisable { get; set; }

    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float damageToPlayerOnContact = 5;
    
    [SerializeField] public Rigidbody2D rb { get; set; }

    [SerializeField] private Transform enemyGFX;
    [SerializeField] private GameObject[] childrenToDisable;

    protected BoxCollider2D boxCollider2D;
    protected float startInvulnerability = 0.2f;
    protected float invulnerabilityTimer;

    private void Start() {
        Player = GameController.Instance.Player;
    }

    protected override void Awake(){
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        Health = enemyHealth;
        Damage = damageToPlayerOnContact;
        ChildrenToDisable = childrenToDisable;
        Stunned = false;
        IsAlive = true;

        PlayerAttackEvent.RegisterListener(TakeDamage);
        EnemyTouchKillzoneEvent.RegisterListener(EnemyTouchKillzone);
    }

    protected override void Update(){
        base.Update();
        if (invulnerabilityTimer > 0) {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    protected override void FixedUpdate(){
        base.FixedUpdate();
    }

    public virtual void TakeDamage(PlayerAttackEvent attackEvent){
        if (invulnerabilityTimer <= 0) {
            try {
                if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
                    Vector2 knockBack;
                    ChangeEnemyHealth(-attackEvent.damage);
                    HurtSound();
                    if (attackEvent.isMeleeAttack) {
                        PlayerHealEvent phe = new PlayerHealEvent {
                            isLifeLeech = true
                        };
                        if (attackEvent.player.HasAbility(PlayerAbility.VOIDMEND)) {
                            PlayerGainVoidEvent voidEvent = new PlayerGainVoidEvent {
                                amount = attackEvent.player.MeleeVoidLeech
                            };
                            voidEvent.FireEvent();
                        }
                        phe.FireEvent();
                        if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                            attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                            attackEvent.player.DashesLeft = attackEvent.player.NumberOfDashes;
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                            return;
                        }
                        if (attackEvent.player.IsAttackingUp) {
                            knockBack = KnockBack(new Vector2(0, attackEvent.player.KnockbackForce));
                            rb.velocity = knockBack;
                            return;
                        }
                    }
                    knockBack = KnockBack(new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce, 0));
                    rb.velocity = knockBack;
                }
            } catch (NullReferenceException) {
                Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
            }
        }
    }

    public virtual Vector2 KnockBack(Vector2 knockback) {
        return knockback;
    }

    protected void EnemyTouchKillzone(EnemyTouchKillzoneEvent killzoneEvent) {
        if(killzoneEvent.enemy == this) {
            EnemyDeath();
        }
    }

    public virtual void ChangeEnemyHealth(float amount) {
        Debug.Log("I took " + amount + " damage! (" + gameObject.name +")");
        Health += amount;
        if (Health <= 0) {
            EnemyDeath();
        }
        invulnerabilityTimer = startInvulnerability;
    }

    protected void HurtSound() {
        string[] soundNames = { "Hit1", "Hit2"};
        AudioPlayRandomSoundAtLocationEvent hurtSound = new AudioPlayRandomSoundAtLocationEvent {
            name = soundNames,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1f,
            soundType = SoundType.SFX,
            gameObject = gameObject
        };
        hurtSound.FireEvent();
    }

    public virtual void EnemyDeath()
    {     
        Destroy(gameObject);
    }

    public void setGFX(Vector3 v)
    {
        enemyGFX.localScale = v;
    }

    protected virtual void DeathSound() {
        string[] soundNames = { "Enemy134Death1", "Enemy134Death2", "Enemy134Death3" };
        AudioPlayRandomSoundAtLocationEvent enemyDie = new AudioPlayRandomSoundAtLocationEvent {
            name = soundNames,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1,
            soundType = SoundType.SFX,
            gameObject = gameObject
        };
        enemyDie.FireEvent();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)    {
        if (collision.gameObject.tag == "Player" && IsAlive)
        {
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            Stunned = true;
        }
    }

    protected virtual void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        EnemyTouchKillzoneEvent.UnRegisterListener(EnemyTouchKillzone);
    }

}
