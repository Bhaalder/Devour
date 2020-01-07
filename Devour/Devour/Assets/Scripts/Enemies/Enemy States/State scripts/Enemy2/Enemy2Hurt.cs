//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy2HurtState")]
public class Enemy2Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy2>().State = Enemy2State.HURT;
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
            owner.Transition<Enemy2IdleState>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
