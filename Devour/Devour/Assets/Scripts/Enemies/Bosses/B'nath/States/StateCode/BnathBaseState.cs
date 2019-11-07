﻿using System.Collections;
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

    public SpriteRenderer bossSprite; //För tillfället för test


    public override void Enter()
    {
        base.Enter();
        bossSprite = owner.GetComponentInChildren<SpriteRenderer>(); //TEST
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
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

}
