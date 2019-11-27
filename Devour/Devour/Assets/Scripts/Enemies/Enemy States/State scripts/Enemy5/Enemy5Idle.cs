using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5IdleState")]
public class Enemy5Idle : EnemyMovement
{

    [SerializeField] private float pauseBetweenAttacks = 2f;
    [SerializeField] private float jumpCollisionStunnedTime = 2f;

    private float currentJumpCooldown;

    private bool isPaused;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy5>().State = Enemy5State.IDLE;

        isPaused = true;
        currentJumpCooldown = jumpCollisionStunnedTime;
    }

    public override void HandleUpdate()
    {
        if (isPaused)
        {
            PauseBetweenAttacks();
        }
        if (owner.GetComponent<Enemy5>().jumpCollision)
        {
            JumpStunnedTime();
        }
        else
        {
            if (IsGrounded())
            {
                owner.Transition<Enemy5Movement>();
            }
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void PauseBetweenAttacks()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = pauseBetweenAttacks;
        isPaused = false;
    }

    private void JumpStunnedTime()
    {
        currentJumpCooldown -= Time.deltaTime;

        if (currentJumpCooldown > 0)
        {
            return;
        }

        currentJumpCooldown = jumpCollisionStunnedTime;
        owner.GetComponent<Enemy5>().jumpCollision = false;
    }

    private bool IsGrounded()
    {
        bool lineHit = Physics2D.Raycast(owner.transform.position, Vector2.down, 1f, layerMask);
        return lineHit;
    }
}
