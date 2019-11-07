using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3ChargeAttack")]

public class Enemy3ChargeAttack : Enemy3Movement
{
    [SerializeField] private float chargeSpeed = 700f;

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
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
        if(Mathf.Approximately( (int)owner.rb.position.x, (int)owner.GetComponent<Enemy3>().ChargeTarget.x))
        {
            owner.rb.velocity = new Vector2(0f,owner.rb.velocity.y);
            owner.Stunned = true;
            owner.Transition<Enemy3Movement>();
        }

        FindTargetDirection();
        force = direction.normalized * chargeSpeed * Time.deltaTime;
        owner.rb.AddForce(force);
        CheckGround();

    }

    private void FindTargetDirection()
    {
        if (owner.GetComponent<Enemy3>().ChargeTarget.x > owner.rb.position.x)
        {
            direction = new Vector2(1f, 0f);
        }
        else if (owner.GetComponent<Enemy3>().ChargeTarget.x < owner.rb.position.x)
        {
            direction = new Vector2(-1f, 0f);
        }
    }

    private void CheckGround()
    {
        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, direction, distanceBeforeTurning, layerMask);
        if (obstructed.collider == true)
        {
            movingRight = !movingRight;
            owner.Transition<Enemy3Movement>();
        }
        noGroundAhead = new Vector2(direction.x, -1);
        RaycastHit2D noMoreGround = Physics2D.Raycast(owner.rb.position, noGroundAhead, distanceBeforeTurning + 2f, layerMask);

        if (noMoreGround.collider == false)
        {
            movingRight = !movingRight;
            owner.Transition<Enemy3Movement>();
        }
    }
}
