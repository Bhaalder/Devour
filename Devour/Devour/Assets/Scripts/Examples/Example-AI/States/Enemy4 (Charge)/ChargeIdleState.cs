//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChargeIdleState")]
public class ChargeIdleState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which the Enemy starts chasing the Player if it can see the Player.")]
    [SerializeField] private float chaseDistance;

    // Methods
    public override void Enter()
    {
        base.Enter();
        //Animation Idle
        owner.animator.SetBool("isRunning", false);
        owner.animator.SetBool("isIdle", true);
        owner.animator.SetBool("isAttacking", false);
        owner.animator.SetBool("isBuilding", false);
        owner.animator.SetBool("isCharging", false);
        owner.animator.SetBool("isStunned", false);
    }

    public override void HandleUpdate()
    {
        if (CanSeePlayer() && Vector3.Distance(owner.transform.position, owner.player.transform.position) < chaseDistance)
        {
            owner.Transition<ChargeChaseState>();
        }
    }
}
