//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicThrustTelegraphState")]
public class Boss2SonicThrustTelegraph : Boss2BaseState
{
    [SerializeField] private float telegraphTime = 1f;

    private float currentCooldown;
    private float telegraphCurrentCooldown;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_THRUST_TELEGRAPH;

        FindTargetDirection();
        owner.dashStartDirection = direction;
        currentCooldown = telegraphTime;
        telegraphCurrentCooldown = 0;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        TelegraphTime();
        Telegraph();
        owner.rb.velocity = new Vector2(0, 0);
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
    private void TelegraphTime()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = telegraphTime;
        owner.Transition<Boss2SonicThrustAttack>();
    }

    private void Telegraph()
    {
        telegraphCurrentCooldown += Time.deltaTime;

        if (telegraphCurrentCooldown < telegraphTime)
        {
            return;
        }

        telegraphCurrentCooldown = 0;
    }
}
