using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy4MovementState")]
public class Enemy4Movement : EnemyMovement
{

    [SerializeField] private float enemySpeed = 400;
    [SerializeField] private float distanceBeforeTurning = 3f;
    [SerializeField] private float attackDistance = 15f;


    private Vector2 direction;
    private Vector2 force;
    private Vector2 noGroundAhead;
    private bool movingRight = true;



    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy4>().State = Enemy4State.MOVEMENT;
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

        if (!owner.Stunned)
        {
            currentCooldown = timeStunned;

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
        else if (owner.Stunned)
        {
            StunnedCooldown();
        }

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

    private void CheckAttackDistance()
    {

        if (owner.GetComponent<Enemy4>().AttackOnlyOnCanSeePlayer)
        {
            if (CanSeePlayer() && Vector2.Distance(owner.rb.position, owner.Player.transform.position) < attackDistance)
            {
                owner.Transition<Enemy4RangeAttack>();
            }
        }
        else
        {
            if (Vector2.Distance(owner.rb.position, owner.Player.transform.position) < attackDistance)
            {
                owner.Transition<Enemy4RangeAttack>();
            }
        }
    }


}
