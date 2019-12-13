using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3MovementState")]

public class Enemy3Movement : EnemyMovement
{
    [SerializeField] private float enemySpeed = 400f;
    [SerializeField] protected float distanceBeforeTurning = 3f;
    [SerializeField] private float newPositionCooldownTime = 5f;
    [SerializeField] protected float attackDistance = 15f;
    [SerializeField] private float iWasStuckCooldown = 5f;


    protected Vector2 force;
    protected Vector2 direction;
    protected Vector2 noGroundAhead;

    private Vector2 startingPosition;
    private Vector2 newPosition;

    protected bool movingRight = true;
    private bool startPositionSet = false;
    private bool movingToNewPosition = false;

    private float currentPositionCooldown;
    private float patrolMoveRange;
    private float iWasStuckCurrentCooldown;


    public override void Enter()
    {
        base.Enter();
        iWasStuckCurrentCooldown = iWasStuckCooldown;
        owner.GetComponent<Enemy3>().State = Enemy3State.MOVEMENT;
    }

    public override void HandleUpdate()
    {
        if (owner.IsIdle)
        {
            CheckAttackDistance();
            return;
        }
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

        CheckDirection();

        if (newPosition.x < owner.rb.position.x)
        {
            direction = new Vector2(-1f, 0f);
        }
        else if (newPosition.x > owner.rb.position.x)
        {
            direction = new Vector2(1f, 0f);
        }

        if (movingToNewPosition)
        {
            force = direction * enemySpeed * Time.deltaTime;
            owner.rb.AddForce(force);
        }
        else
        {
            positionUpdateCooldown();
        }

        if (Mathf.Round(owner.rb.position.x) == Mathf.Round(newPosition.x))
        {
            owner.rb.velocity = new Vector2(0f, 0f);
            movingToNewPosition = false;
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
        currentPositionCooldown = newPositionCooldownTime;
    }

    private void IWasStuckCooldown()
    {
        iWasStuckCurrentCooldown -= Time.deltaTime;

        if (iWasStuckCurrentCooldown > 0)
        {
            return;
        }

        owner.GetComponent<Enemy3>().IWasStuck = false;
        iWasStuckCurrentCooldown = iWasStuckCooldown;
    }

    private void setNewPosition()
    {
        patrolMoveRange = owner.GetComponent<Enemy3>().PatrolMoveRange;
        newPosition = new Vector2(startingPosition.x + Random.Range(-patrolMoveRange, patrolMoveRange), 0);
        movingToNewPosition = true;
    }

    private void CheckAttackDistance()
    {
        if (owner.GetComponent<Enemy3>().IWasStuck == false)
        {
            if (CanSeePlayer() && Vector2.Distance(owner.rb.position, owner.Player.transform.position) < attackDistance)
            {
                if (owner.GetComponent<Enemy3>().ChargeEnemy == true)
                {
                    SetChargeTarget();
                    owner.Transition<Enemy3TelegraphCharge>();
                }
                else
                {
                    owner.Transition<Enemy3FollowAttack>();
                }
            }
        }
        else
        {
            IWasStuckCooldown();
        }
    }

    protected void SetChargeTarget()
    {
        owner.GetComponent<Enemy3>().ChargeTarget = new Vector2(target.position.x, 0);
    }

    private void CheckDirection()
    {
        if (owner.rb.velocity.x <= 0.01f && movingToNewPosition == true)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (owner.rb.velocity.x >= -0.01f && movingToNewPosition == true)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
    }

}

