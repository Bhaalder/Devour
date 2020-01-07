//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5MovementState")]
public class Enemy5Movement : EnemyMovement
{

    [SerializeField] private float enemySpeed = 400;
    [SerializeField] private float distanceBeforeTurning = 3f;
    [SerializeField] private float attackDistance = 10f;
    [SerializeField] private float tooCloseToJumpTime = 2f;

    private Vector2 direction;
    private Vector2 force;
    private Vector2 stuckCheckPosition;

    private float stuckCooldown = 1.5f;
    private float currentStuckCooldown;
    private float currentTooCloseCooldown;
    private float dropDownDistance;

    private bool movingRight = true;
    private bool stuckCheckStarted;

    public override void Enter()
    {
        base.Enter();
        currentStuckCooldown = stuckCooldown;
        currentTooCloseCooldown = tooCloseToJumpTime;
        owner.GetComponent<Enemy5>().State = Enemy5State.IDLE;
        dropDownDistance = owner.GetComponent<Enemy5>().LedgeJumpDownDistance;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        owner.GetComponent<Enemy5>().State = Enemy5State.MOVEMENT;

        if (!owner.IsIdle)
        {
            Movement();
        }
        
        if(DistanceToPlayer() < attackDistance && CheckPlayer() && !owner.GetComponent<Enemy5>().TooCloseToJump)
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
        StuckCheck();

        if (owner.GetComponent<Enemy5>().TooCloseToJump)
        {
            TooCloseToJump();
        }

    }

    private void CheckGround()
    {
        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, direction, distanceBeforeTurning, layerMask);
        if (obstructed.collider == true)
        {
            movingRight = !movingRight;
        }
        RaycastHit2D noMoreGround = Physics2D.Raycast(owner.rb.position + new Vector2(direction.x * 2 , 0), Vector2.down, dropDownDistance, layerMask);

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

    private void StuckCheck()
    {
        if (stuckCheckStarted)
        {
            StuckCheckCooldown();
        }
        else if (!stuckCheckStarted)
        {
            stuckCheckPosition = new Vector2(Mathf.Round(owner.rb.position.x), Mathf.Round(owner.rb.position.y));
            stuckCheckStarted = true;
        }

    }

    private void StuckCheckCooldown()
    {
        currentStuckCooldown -= Time.deltaTime;

        if (currentStuckCooldown > 0)
        {
            return;
        }

        Vector2 currentPosition = new Vector2(Mathf.Round(owner.rb.position.x), Mathf.Round(owner.rb.position.y));

        if (currentPosition == stuckCheckPosition)
        {
            movingRight = !movingRight;
        }

        stuckCheckStarted = false;
        currentStuckCooldown = stuckCooldown;
    }

    private void TooCloseToJump()
    {
        currentTooCloseCooldown -= Time.deltaTime;

        if (currentTooCloseCooldown > 0)
        {
            return;
        }

        owner.GetComponent<Enemy5>().TooCloseToJump = false;
    }
}
