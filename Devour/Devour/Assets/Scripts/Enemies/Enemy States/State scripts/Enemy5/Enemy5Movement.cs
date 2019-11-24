using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5MovementState")]
public class Enemy5Movement : EnemyMovement
{

    [SerializeField] private float enemySpeed = 400;
    [SerializeField] private float distanceBeforeTurning = 3f;
    [SerializeField] private float attackDistance = 10f;

    private Vector2 direction;
    private Vector2 force;
    private Vector2 noGroundAhead;
    private bool movingRight = true;

    public override void Enter()
    {
        base.Enter();   
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        owner.GetComponent<Enemy5>().State = Enemy5State.MOVEMENT;
        Movement();
        if(DistanceToPlayer() < attackDistance && CanSeePlayer())
        {
            owner.Transition<Enemy5Attack>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Movement()
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

    private bool CheckPlayer()
    {
        bool lineHit = Physics2D.Linecast(owner.transform.position + new Vector3(0,3,0), owner.Player.transform.position, layerMask);
        return !lineHit;
    }
}
