using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy4IdleState")]

public class Enemy4Idle : EnemyMovement
{
    [SerializeField] private float attackDistance;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy4>().State = Enemy4State.IDLE;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        CheckAttackDistance();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void CheckAttackDistance()
    {
        if (CanSeePlayer() && Vector2.Distance(owner.rb.position, target.position) < attackDistance)
        {
            owner.Transition<Enemy4RangeAttack>();
        }
    }
}
