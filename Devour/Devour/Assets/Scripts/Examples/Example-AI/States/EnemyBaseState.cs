// Daniel Fors
//Secondary Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State
{
    // Attributes
    [SerializeField] protected float moveSpeed;
    protected Enemy owner;
    private float distanceToPlayer;
    private Vector3 lineCastYOffset;
    private float distanceToGround;

    // Methods
    public override void Enter()
    {
        owner.agent.speed = moveSpeed;
        lineCastYOffset.Set(0, 2, 0);
        distanceToGround = owner.GetComponent<Collider>().bounds.extents.y;
    }

    public override void Initialize(StateMachine owner)
    {
        this.owner = (Enemy)owner;
    }

    protected bool CanSeePlayer()
    {
        bool lineHit = Physics.Linecast(owner.transform.position + lineCastYOffset, owner.player.transform.position + lineCastYOffset, out RaycastHit hit, owner.visionMask);
        Debug.DrawLine(owner.transform.position, hit.point, Color.red);
        return !lineHit;
    }

    protected bool IsGrounded()
    {
        return Physics.Raycast(owner.transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    protected float DistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(owner.transform.position, owner.player.transform.position);
        return distanceToPlayer;
    }

}
