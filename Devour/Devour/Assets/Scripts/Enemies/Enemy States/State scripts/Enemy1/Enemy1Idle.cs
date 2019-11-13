using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Idle : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy1>().State = Enemy1State.IDLE;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

}
