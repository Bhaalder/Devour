﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3MovementState")]

public class Enemy3Movement : EnemyMovement
{
    [SerializeField] private float enemySpeed = 400f;
    [SerializeField] private float distanceBeforeTurning = 3f;
    [SerializeField] private float patrolMoveRange = 3;
    [SerializeField] protected float attackDistance = 15f;

    protected Vector2 force;
    protected Vector2 direction;
    private Vector2 startingPosition;
    private Vector2 newPosition;
    private Vector2 noGroundAhead;

    private bool movingRight = true;
    private bool startPositionSet = false;

    private float cooldownTime = .5f;
    private float currentPositionCooldown;


    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
        if (!owner.Stunned)
        {
            if (owner.GetComponent<Enemy3>().PatrolEnemy == true)
            {
                Patrol();
            }
            else
            {
                Movement();
            }
        }
        else if (owner.Stunned)
        {
            StunnedCooldown();
        }

        base.HandleUpdate();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Movement()
    {
        if (movingRight)
        {
            direction = new Vector2(1f, 0f);
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (!movingRight)
        {
            direction = new Vector2(-1f, 0f);
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }

        force = direction.normalized * enemySpeed * Time.deltaTime;

        owner.rb.AddForce(force);


        CheckAttackDistance();
        CheckGround();
    }

    private void Patrol()
    {
        if (!startPositionSet)
        {
            startingPosition = owner.rb.position;
            startPositionSet = true;
        }

        positionUpdateCooldown();

        direction = (newPosition - owner.rb.position).normalized;
        direction.y = 0;    

        force = direction * enemySpeed * Time.deltaTime;
        owner.rb.AddForce(force);

        if (owner.rb.position == newPosition)
        {
            setNewPosition();
        }

        CheckAttackDistance();
        CheckGround();
    }

    private void CheckGround()
    {
        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, direction, distanceBeforeTurning, layerMask);
        if (obstructed.collider == true)
        {
            movingRight = !movingRight;
        }
        noGroundAhead = new Vector2(direction.x, -1);
        RaycastHit2D noMoreGround = Physics2D.Raycast(owner.rb.position, noGroundAhead, distanceBeforeTurning + 2f, layerMask);

        if (noMoreGround.collider == false)
        {
            movingRight = !movingRight;
        }
    }


    private void positionUpdateCooldown()
    {
        currentPositionCooldown -= Time.deltaTime;

        if (currentPositionCooldown > 0)
        {
            return;
        }

        setNewPosition();
        currentPositionCooldown = cooldownTime;
    }

    private void setNewPosition()
    {
        newPosition = new Vector2(startingPosition.x + Random.Range(0, patrolMoveRange), 0);
    }

    private void CheckAttackDistance()
    {
        if (CanSeePlayer() && Vector2.Distance(owner.rb.position, target.position) < attackDistance)
        {
            if (owner.GetComponent<Enemy3>().ChargeEnemy == true)
            {
                SetChargeTarget();
                Debug.Log("Handing over to Telegraph, Charge at: " + owner.GetComponent<Enemy3>().ChargeTarget);
                owner.Transition<Enemy3TelegraphCharge>();
            }
            else
            {
                owner.Transition<Enemy3FollowAttack>();
            }
        }
    }

    protected void SetChargeTarget()
    {
        owner.GetComponent<Enemy3>().ChargeTarget = new Vector2(target.position.x, 0);
    }
}
