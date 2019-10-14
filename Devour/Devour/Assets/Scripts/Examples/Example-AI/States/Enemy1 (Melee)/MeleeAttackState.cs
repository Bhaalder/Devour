//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/MeleeAttackState")]
public class MeleeAttackState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which the Enemy stops trying to attack and starts chasing the Player.")]
    [SerializeField] private float chaseDistance;
    [Tooltip("Time in Seconds between Attacks.")]
    [SerializeField] private float cooldown;
    [Tooltip("Damage done to Player with each MeleeAttack.")]
    [SerializeField] private int damage;


    private float currentCooldown;

    // Methods
    public override void Enter()
    {
        base.Enter();
        owner.EnemyMeleeDamage = damage;
    }

    public override void HandleUpdate()
    {
        if (owner.getIsDead() == false)
        {
            Attack();

            if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > chaseDistance || CanSeePlayer() == false)
            {
                owner.Transition<MeleeChaseState>();
            }
        }
    }

    private void Attack()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        if (CanSeePlayer() == true)
        {
            owner.animator.SetBool("isRunning", false);
            owner.animator.SetBool("isIdle", false);
            owner.animator.SetBool("isAttacking", true);
            owner.CanDamage = true;
        }
        currentCooldown = cooldown;
    }
    
}
