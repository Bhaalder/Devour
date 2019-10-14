//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChargeMeleeAttackState")]
public class ChargeMeleeAttackState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which the Enemy stops trying to attack and starts chasing the Player.")]
    [SerializeField] private float chaseDistance;
    [Tooltip("Time in Seconds between Attacks.")]
    [SerializeField] private float cooldown;
    [Tooltip("Damage done to Player with each Attack.")]
    [SerializeField] private int damage;
    [Tooltip("Distance from Player where Enemy starts Attack")]
    [SerializeField] private float attackDistance;

    private float currentCooldown;

    // Methods
    public override void Enter()
    {
        base.Enter();
        owner.EnemyMeleeDamage = damage;
        
    }

    public override void HandleUpdate()
    {

        if (DistanceToPlayer() < attackDistance)
        {
            Attack();
            return;
        }
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > chaseDistance || CanSeePlayer() == false)
        {
            owner.Transition<ChargeChaseState>();
        }
        owner.agent.SetDestination(owner.player.transform.position);

        if(CanSeePlayer() == true)
        {
            float step = 3 * Time.deltaTime;
            Vector3.RotateTowards(owner.transform.forward, owner.player.transform.position, step, 0.0f);
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
            owner.transform.LookAt(owner.player.transform, Vector3.up);
            //owner.animator.Play("Attack");
            owner.animator.SetBool("isRunning", false);
            owner.animator.SetBool("isIdle", false);
            owner.animator.SetBool("isAttacking", true);
            owner.animator.SetBool("isBuilding", false);
            owner.animator.SetBool("isCharging", false);
            owner.animator.SetBool("isStunned", false);
            //GameController.Instance.TakeDamage(damage); // animationen träffar efter 1 sekund
        }

        currentCooldown = cooldown;
    }
}
