//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2BaseState")]
public class Boss2BaseState : State
{
    protected Boss2 owner;
    protected Vector2 force;
    protected Vector2 direction;

    private float distanceToPlayer;

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (owner.IntroStarted && !owner.Transitioned)
        {
            owner.Transitioned = true;
            owner.Transition<Boss2Intro>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    public override void Initialize(StateMachine owner)
    {
        this.owner = (Boss2)owner;
    }

    protected float DistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(owner.rb.position, owner.Player.transform.position);
        return distanceToPlayer;
    }

    protected void TurnedRight()
    {
        if (owner.Player.transform.position.x > owner.rb.position.x)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (owner.Player.transform.position.x < owner.rb.position.x)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
    }

    protected void FindTargetDirection()
    {
        if (owner.Player.transform.position.x > owner.rb.position.x)
        {
            direction = new Vector2(1f, 0f);
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (owner.Player.transform.position.x < owner.rb.position.x)
        {
            direction = new Vector2(-1f, 0f);
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
    }
}
