using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EnemyBaseState
{

    [SerializeField] protected float timeStunned = 2f;
    [SerializeField] protected LayerMask layerMask;


    protected float currentCooldown;

    protected Transform target;


    public override void Enter()
    {
        base.Enter();
        target = FindObjectOfType<Player>().transform;
        currentCooldown = timeStunned;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    protected bool CanSeePlayer()
    {
        bool lineHit = Physics2D.Linecast(owner.transform.position, owner.Player.transform.position, layerMask);
        return !lineHit;
    }

    protected void StunnedCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            owner.rb.velocity = new Vector2(0f, 0f);
            return;
        }

        owner.Stunned = false;
        currentCooldown = timeStunned;
    }

}
