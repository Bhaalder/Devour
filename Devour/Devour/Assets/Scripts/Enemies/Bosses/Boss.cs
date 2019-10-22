//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy{

    [SerializeField] private string bossName;

    protected override void Awake() {
        base.Awake();
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void TakeDamage(PlayerAttackEvent attackEvent) {
        try {
            if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
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
                }
            }
        } catch (System.NullReferenceException) {

        }
    }

    public override void ChangeEnemyHealth(float amount) {
        Debug.Log(bossName + ": " + Health + " health left");
        Health += amount;
        if (Health <= 0) {
            EnemyDeath();
        }
    }

    public override void EnemyDeath() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        //Bossen gör en animation som tar X lång tid
        //Bossen dör
        Destroy(gameObject);
    }
}
