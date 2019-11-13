using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy1HurtState")]
public class Enemy1Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy1>().State = Enemy1State.HURT;
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
