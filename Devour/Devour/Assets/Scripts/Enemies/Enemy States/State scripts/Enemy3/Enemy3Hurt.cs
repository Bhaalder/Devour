using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3HurtState")]

public class Enemy3Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy3>().State = Enemy3State.HURT;
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
            owner.Transition<Enemy3Movement>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
