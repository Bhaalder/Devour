﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyPauseOnDamageState")]
public class EnemyPauseOnDamage : EnemyBaseState
{

    [SerializeField] private float timePaused = 2f;
    private float currentCooldown;
    private bool iHavePaused = false;


    public override void Enter()
    {
        base.Enter();
        currentCooldown = timePaused;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        TimePausedCooldownTimer();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void TimePausedCooldownTimer()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = timePaused;

        if (owner.GetComponent<Enemy1>() != null)
        {
            owner.Transition<Enemy1MovementState>();
        }
        else if (owner.GetComponent<Enemy2>() != null)
        {
            owner.Transition<Enemy2MovementState>();
        }
        else if (owner.GetComponent<Enemy3>() != null)
        {
            owner.Transition<Enemy3Movement>();
        }

    }

}
