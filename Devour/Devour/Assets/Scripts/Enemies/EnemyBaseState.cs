//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State
{

    protected Enemy owner;

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
    public override void Initialize(StateMachine owner)
    {
        this.owner = (Enemy)owner;
    }
}
