using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy1MovementState")]
public class Enemy1MovementState : EnemyBaseState
{

    [SerializeField] private float enemySpeed = 400;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distanceBeforeTurning = 1f;

    private Vector2 direction;
    private Vector2 force;
    private bool movingRight = true;
    

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
        Movement();
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
        }
        else if (!movingRight)
        {
            direction = new Vector2(-1f, 0f);
        }

        force = direction.normalized * enemySpeed * Time.deltaTime;

        owner.rb.AddForce(force);

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

        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, direction, distanceBeforeTurning, layerMask);
        if (obstructed.collider == true)
        {
            movingRight = !movingRight;
        }
    }


}
