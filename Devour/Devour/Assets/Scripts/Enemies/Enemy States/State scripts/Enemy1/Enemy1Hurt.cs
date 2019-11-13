﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        //owner.State = Enemy1State.HURT;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if(owner.Stunned)
        {
            StunnedCooldown();
        }
        else
        {
            owner.Transition<Enemy1MovementState>();

        }
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
