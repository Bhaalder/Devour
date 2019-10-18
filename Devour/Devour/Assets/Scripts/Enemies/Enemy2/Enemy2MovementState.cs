using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;


[CreateAssetMenu(menuName = "Enemy/Enemy2MovementState")]

public class Enemy2MovementState : EnemyBaseState
{

    [SerializeField] private Transform target;
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

    private float cooldownTime = .5f;
    private float currentCooldown;

    public override void Enter()
    {
        base.Enter();
        seeker = owner.GetComponent<Seeker>();
        target = FindObjectOfType<Player>().transform;
        currentCooldown = cooldownTime;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        pathUpdateCooldown();
        Movement();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Movement()
    {
        if (!isWithinAttackDistance)
        {
            if (Vector2.Distance(owner.rb.position, target.position) <= attackDistance)
            {
                isWithinAttackDistance = true;
            }
        }

        if (isWithinAttackDistance)
        {
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
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(owner.rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void pathUpdateCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        UpdatePath();
        currentCooldown = cooldownTime;
    }
}
