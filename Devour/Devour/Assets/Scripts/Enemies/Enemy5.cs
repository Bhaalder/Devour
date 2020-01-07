//Author: Marcus Söderberg
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
    [SerializeField] private float ledgeJumpDownDistance = 15f;
    public Enemy5State State { get; set; }
    public Animator Animator { get; set; }
    public bool JumpCollision { get; set; }
    public bool TooCloseToJump { get; set; }
    public float LedgeJumpDownDistance { get => ledgeJumpDownDistance; set => ledgeJumpDownDistance = value; }

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        JumpCollision = false;
        TooCloseToJump = false;
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

            Transition<Enemy5Hurt>();
        }
    }

    public override void TakeDamage(PlayerAttackEvent attackEvent)
    {
        if (invulnerabilityTimer <= 0)
        {
            try
            {
                if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds) && boxCollider2D.enabled == true)
                {
                    ChangeEnemyHealth(-attackEvent.damage);
                    HurtSoundAndParticles();
                    if (attackEvent.isMeleeAttack)
                    {
                        PlayerHealEvent phe = new PlayerHealEvent
                        {
                            isLifeLeech = true
                        };
                        if (attackEvent.player.HasAbility(PlayerAbility.VOIDMEND))
                        {
                            PlayerVoidChangeEvent voidEvent = new PlayerVoidChangeEvent
                            {
                                amount = attackEvent.player.MeleeVoidLeech
                            };
                            voidEvent.FireEvent();
                        }
                        phe.FireEvent();
                        if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack)
                        {
                            attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                            attackEvent.player.DashesLeft = attackEvent.player.NumberOfDashes;
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                            return;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
            }
        }
    }

}
