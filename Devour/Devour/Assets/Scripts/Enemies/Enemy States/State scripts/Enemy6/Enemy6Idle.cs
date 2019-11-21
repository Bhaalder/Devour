using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/IdleState")]
public class Enemy6Idle : EnemyMovement
{
    [SerializeField] private float pauseBetweenAttacks = 2f;

    private float attackDistance;

    private bool isPaused;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.IDLE;

        attackDistance = owner.GetComponent<Enemy6>().AttackDistance;

        isPaused = true;
    }

    public override void HandleUpdate()
    {
        if (isPaused)
        {
            PauseBetweenAttacks();
        }
        else
        {
            if (!owner.IsIdle)
            {
                owner.Transition<Enemy6Movement>();
            }
            else
            {
                if(DistanceToPlayer() < attackDistance && CanSeePlayer())
                {
                    owner.Transition<Enemy6TelegraphAttack>();
                }
            }
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void PauseBetweenAttacks()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = pauseBetweenAttacks;
        isPaused = false;
    }
}
