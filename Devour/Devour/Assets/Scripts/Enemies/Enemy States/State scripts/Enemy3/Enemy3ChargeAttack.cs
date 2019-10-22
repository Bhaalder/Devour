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
        Debug.Log("Enter A; Attack Target:"+ owner.GetComponent<Enemy3>().ChargeTarget);
    }

    public override void HandleUpdate()
    {
        //base.HandleUpdate();
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
            owner.rb.velocity = new Vector2(0f,0f);
            owner.Stunned = true;
            owner.Transition<Enemy3Movement>();
        }

        FindTargetDirection();
        force = direction.normalized * chargeSpeed * Time.deltaTime;
        owner.rb.AddForce(force);

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
}
