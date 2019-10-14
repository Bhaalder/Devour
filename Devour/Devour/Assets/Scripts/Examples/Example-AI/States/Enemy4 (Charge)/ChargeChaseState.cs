//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChargeChaseState")]
public class ChargeChaseState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which Enemy will start Attacking the Player.")]
    [SerializeField] private float attackDistance;
    [Tooltip("Distance at which Enemy will stop chasing the Player if it can no longer see the Player.")]
    [SerializeField] private float stopChaseDistance;
    private bool isChasing;

    // Methods

    public override void Enter()
    {
        base.Enter();
        isChasing = true;
        //Animation Chase
        owner.animator.SetBool("isRunning", true);
        owner.animator.SetBool("isIdle", false);
        owner.animator.SetBool("isAttacking", false);
        owner.animator.SetBool("isBuilding", false);
        owner.animator.SetBool("isCharging", false);
        owner.animator.SetBool("isStunned", false);
    }
    public override void HandleUpdate()
    {

        if (DistanceToPlayer() < attackDistance && isChasing && owner.HasRecentlyCharged == false)
        {
            isChasing = false;
            
            owner.Transition<ChargeBuildUpState>();
            return;
        }
        owner.agent.SetDestination(owner.player.transform.position);

        if (owner.HasRecentlyCharged == true)
        {
            owner.Transition<ChargeMeleeAttackState>();
        }

        if (DistanceToPlayer() < stopChaseDistance && CanSeePlayer() == false)
        {
            owner.Transition<ChargeIdleState>();
        }

    }
}
