//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/HurtState")]
public class Enemy6Hurt : EnemyMovement
{
    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.IDLE;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<BoxCollider2D>().enabled = false;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void HandleUpdate()
    {
        if (owner.Stunned)
        {
            StunnedCooldown();
        }
        else
        {
            owner.Transition<Enemy6Idle>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
