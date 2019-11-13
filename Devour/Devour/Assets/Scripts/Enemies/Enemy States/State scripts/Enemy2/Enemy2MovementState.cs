using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;


[CreateAssetMenu(menuName = "Enemy/Enemy2MovementState")]

public class Enemy2MovementState : EnemyMovement
{

    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float attackDistance = 25f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool isWithinAttackDistance = false;


    private Seeker seeker;

    private Vector2 direction;
    private Vector2 force;
    private Vector2 startPosition;

    private float cooldownTime = .5f;
    private float currentPathUpdateCooldown;

    public override void Enter()
    {
        base.Enter();
        seeker = owner.GetComponent<Seeker>();
        currentPathUpdateCooldown = cooldownTime;
        startPosition = owner.GetComponent<Enemy2>().StartPosition;
        owner.GetComponent<Enemy2>().State = Enemy2State.MOVEMENT;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        pathUpdateCooldown();

        if (!owner.Stunned)
        {
            Movement();
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

        if (Vector2.Distance(owner.rb.position, target.position) <= attackDistance)
        {
            isWithinAttackDistance = true;
        }
        else if (Vector2.Distance(owner.rb.position, target.position) >= attackDistance)
        {
            isWithinAttackDistance = false;
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - owner.rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        owner.rb.AddForce(force);

        float distance = Vector2.Distance(owner.rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (owner.rb.velocity.x >= 0.01f)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (owner.rb.velocity.x <= -0.01f)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
    }

    void UpdatePath()
    {
        if (isWithinAttackDistance)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(owner.rb.position, target.position, OnPathComplete);
            }
        }
        else if (!isWithinAttackDistance)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(owner.rb.position, startPosition, OnPathComplete);
            }
        }

    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            if (!isWithinAttackDistance)
            {
                Vector2 cp = new Vector2(Mathf.Round(owner.rb.position.x), Mathf.Round(owner.rb.position.y));
                Vector2 op = new Vector2(Mathf.Round(startPosition.x), Mathf.Round(startPosition.y));

                if(cp == op)
                {
                    owner.Transition<Enemy2IdleState>();
                }
            }
        }
    }

    private void pathUpdateCooldown()
    {
        currentPathUpdateCooldown -= Time.deltaTime;

        if (currentPathUpdateCooldown > 0)
        {
            return;
        }

        UpdatePath();
        currentPathUpdateCooldown = cooldownTime;
    }
}
