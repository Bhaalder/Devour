using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3FollowAttackState")]

public class Enemy3FollowAttack : EnemyMovement
{

    [SerializeField] private float followSpeed = 300f;
    [SerializeField] private float abortFollow = 25f;

    private Vector2 direction;
    private Vector2 force;

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        FindTargetDirection();
        force = direction.normalized * followSpeed * Time.deltaTime;
        owner.rb.AddForce(force);

        if (owner.rb.velocity.x <= 0.01f)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (owner.rb.velocity.x >= -0.01f)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }

        if (Vector2.Distance(owner.rb.position, target.position) >= abortFollow)
        {
            owner.Transition<Enemy3Movement>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void FindTargetDirection()
    {
        if (target.position.x > owner.rb.position.x)
        {
            direction = new Vector2(1f, 0f);
        }
        else if (target.position.x < owner.rb.position.x)
        {
            direction = new Vector2(-1f, 0f);
        }
    }
}
