//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy4HurtState")]

public class Enemy4Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy4>().State = Enemy4State.IDLE;
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (owner.Stunned)
        {
            StunnedCooldown();
        }
        else
        {
            if (owner.GetComponent<Enemy4>().IsIdle)
            {
                owner.Transition<Enemy4Idle>();
            }
            else
            {
                owner.Transition<Enemy4Movement>();
            }
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
