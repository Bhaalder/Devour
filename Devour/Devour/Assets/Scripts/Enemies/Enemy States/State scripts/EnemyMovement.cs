using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EnemyBaseState
{

    [SerializeField] protected float timeStunned = 2f;

    protected float currentCooldown;

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    protected void StunnedCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        owner.Stunned = false;
        currentCooldown = timeStunned;
    }

}
