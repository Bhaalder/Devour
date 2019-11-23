﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoidObstacleState {
    SPAWNED, ARMED
}

public class NazroVoidObstacle : Enemy{

    public VoidObstacleState VoidObstacleState { get; set; }

    public Nazro Nazro { get; set; }
    public float ArmingTime { get; set; }

    protected override void Awake() {
        base.Awake();
        BossDiedEvent.RegisterListener(BossDied);
    }

    protected override void Update() {
        base.Update();
        //Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void EnemyTouchKillzone(EnemyTouchKillzoneEvent killzoneEvent) {
        //Can't touch killzone
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        if(VoidObstacleState == VoidObstacleState.ARMED) {
            if (collision.gameObject.tag == "Player") {
                PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                    damage = damageToPlayerOnContact
                };
                ptde.FireEvent();
                Stunned = true;
            }
        }
    }

    public override void TakeDamage(PlayerAttackEvent attackEvent) {
        if (invulnerabilityTimer <= 0) {
            try {
                if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
                    ChangeEnemyHealth(-attackEvent.damage);
                    HurtSoundAndParticles();
                    if (attackEvent.isMeleeAttack) {
                        //PlayerHealEvent phe = new PlayerHealEvent { //MAN BORDE INTE LIFELEECHA FRÅN DESSA
                        //    isLifeLeech = true
                        //};
                        //phe.FireEvent();
                        if (attackEvent.player.HasAbility(PlayerAbility.VOIDMEND)) {
                            PlayerVoidEvent voidEvent = new PlayerVoidEvent {
                                amount = attackEvent.player.MeleeVoidLeech
                            };
                            voidEvent.FireEvent();
                        }
                        if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                            attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                            return;
                        }
                    }
                }
            } catch (System.NullReferenceException) {
                Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
            }
        }
    }

    public override void EnemyDeath() {
        Nazro.NazroVoidObstacles.Remove(gameObject);
        Destroy(gameObject);//FÖR TILLFÄLLET
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);//borde inte explodera då
    }

    protected override void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        EnemyTouchKillzoneEvent.UnRegisterListener(EnemyTouchKillzone);
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
