//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/B'nathBaseState")]
public class BnathBaseState : State
{
    protected Bnath owner;

    protected Vector2 startPoint;
    protected Vector2 endPoint;
    protected Vector2 middlePoint;

    private float distanceToPlayer;

    public override void Enter()
    {
        base.Enter();
        owner.IntroStarted = false;
        owner.Transitioned = false;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (owner.IntroStarted && !owner.Transitioned)
        {
            Debug.Log("GOTO INTRO");
            owner.Transitioned = true;
            owner.Transition<BnathIntro>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    protected virtual void Movement()
    {
        owner.Transition<BnathIdle>();
    }
    public override void Initialize(StateMachine owner)
    {
        this.owner = (Bnath)owner;
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

}
