using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5IdleState")]
public class Enemy5Idle : EnemyMovement
{

    [SerializeField] private float pauseBetweenAttacks = 2f;

    private bool isPaused;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy5>().State = Enemy5State.IDLE;

        isPaused = true;        
    }

    public override void HandleUpdate()
    {
        if (isPaused)
        {
            PauseBetweenAttacks();
        }
        else
        {
            owner.Transition<Enemy5Movement>();
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
}
