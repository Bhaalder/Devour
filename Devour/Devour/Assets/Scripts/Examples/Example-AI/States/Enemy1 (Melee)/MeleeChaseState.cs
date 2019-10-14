//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/MeleeChaseState")]
public class MeleeChaseState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which Enemy will start Attacking the Player")]
    [SerializeField] private float attackDistance;
    [Tooltip("Distance at which Enemy will stop chasing the Player if it can no longer see the Player")]
    [SerializeField] private float stopChaseDistance;

    // Methods

    public override void Enter() {
        base.Enter();
        owner.animator.SetBool("isIdle", false);
        owner.animator.SetBool("isRunning", true);
        owner.animator.SetBool("isAttacking", false);
    }

    public override void HandleUpdate()
    {
        owner.agent.SetDestination(owner.player.transform.position);

        if (DistanceToPlayer() < attackDistance)
        {
            owner.Transition<MeleeAttackState>();
        }

        if (DistanceToPlayer() > stopChaseDistance && CanSeePlayer() == false)
        {
            owner.Transition<MeleeIdleState>();
        }
    }
}
