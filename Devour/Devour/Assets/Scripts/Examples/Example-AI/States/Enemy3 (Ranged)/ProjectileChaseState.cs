//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ProjectileChaseState")]
public class ProjectileChaseState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which Enemy will start Attacking the Player")]
    [SerializeField] private float attackDistance;
    [Tooltip("Distance at which Enemy will stop chasing the Player if it can no longer see the Player")]
    [SerializeField] private float stopChaseDistance;

    // Methods
    public override void Enter() {
        base.Enter();
        //Animation
        owner.animator.SetBool("isIdle", false);
        owner.animator.SetBool("isRunning", true);
        owner.animator.SetBool("isAttacking", false);
    }
    public override void HandleUpdate()
    {
        owner.agent.SetDestination(owner.player.transform.position);

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < attackDistance)
        {
            owner.Transition<ProjectileAttackState>();
        }

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < stopChaseDistance && CanSeePlayer() == false)
        {
            owner.Transition<ProjectileIdleState>();
        }
    }
}
