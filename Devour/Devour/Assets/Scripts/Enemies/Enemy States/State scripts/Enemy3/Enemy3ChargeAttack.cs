using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3ChargeAttack")]

public class Enemy3ChargeAttack : Enemy3Movement
{
    [SerializeField] private float dashForce = 40f;
    [SerializeField] private float startDashTime = 0.4f;


    private float dashTime;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy3>().State = Enemy3State.CHARGE;
        dashTime = startDashTime;
        TurnedRight();
        FindTargetDirection();
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
        owner.rb.velocity = new Vector2((dashForce * direction.x), 0);

        if (dashTime <= 0)
        {
            owner.rb.velocity = new Vector2(0, owner.rb.velocity.y);
            owner.Transition<Enemy3Movement>();
        }
        dashTime -= Time.deltaTime;

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
    }
}
