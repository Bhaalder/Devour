using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5HurtState")]
public class Enemy5Hurt : EnemyMovement
{
    public override void Enter()
    {
        owner.GetComponent<Enemy5>().State = Enemy5State.HURT;
        base.Enter();       
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
            owner.Transition<Enemy5Idle>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
