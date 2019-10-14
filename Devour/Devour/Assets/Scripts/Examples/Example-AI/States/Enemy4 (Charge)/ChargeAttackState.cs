//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChargeAttackState")]
public class ChargeAttackState : EnemyBaseState
{
    // Attributes
    [Tooltip("Time in Seconds the charge will last for.")]
    [SerializeField] private float isChargingForSeconds;

    private float currentCooldown;
    private Vector3 chargePoint;

    // Methods
    public override void Enter()
    {
        base.Enter();
        chargePoint = owner.player.transform.position;
        currentCooldown = 0;
        owner.animator.SetBool("isRunning", false);
        owner.animator.SetBool("isIdle", false);
        owner.animator.SetBool("isAttacking", false);
        owner.animator.SetBool("isBuilding", false);
        owner.animator.SetBool("isCharging", true);
        owner.animator.SetBool("isStunned", false);
    }

    public override void HandleUpdate()
    {
        Attack();
    }

    private void Attack()
    {
        currentCooldown += Time.deltaTime;

        if (CanSeePlayer() == true)
        {
            if (owner.transform.position == chargePoint)
            {
                currentCooldown = 0;
                owner.HasRecentlyCharged = true;
                owner.Transition<ChargeStunnedState>();
            }

            if (currentCooldown < isChargingForSeconds)
            {
                owner.agent.SetDestination(chargePoint);
            }
            else
            {
                currentCooldown = 0;
                owner.HasRecentlyCharged = true;
                owner.Transition<ChargeStunnedState>();
            }
        }


    }
}
