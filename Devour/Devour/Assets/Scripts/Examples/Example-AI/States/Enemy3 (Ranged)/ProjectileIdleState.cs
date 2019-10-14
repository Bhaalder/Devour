//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ProjectileIdleState")]
public class ProjectileIdleState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which the Enemy starts chasing the Player if it can see the Player.")]
    [SerializeField] private float chaseDistance;
    [Tooltip("Distance at which Enemy will start Attacking the Player")]
    [SerializeField] private float attackDistance;

    // Methods
    public override void Enter()
    {
        base.Enter();
        //Animation
        owner.animator.SetBool("isIdle", true);
        owner.animator.SetBool("isRunning", false);
        owner.animator.SetBool("isAttacking", false);
    }

    public override void HandleUpdate()
    {
        if (CanSeePlayer() && DistanceToPlayer() < attackDistance)
        {
            owner.Transition<ProjectileAttackState>();
        }
        else if(CanSeePlayer() && DistanceToPlayer() < chaseDistance)
        {
            owner.Transition<ProjectileChaseState>();
        }
    }
}
