using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Enemy2MovementState")]

public class Enemy2MovementState : EnemyBaseState
{
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
