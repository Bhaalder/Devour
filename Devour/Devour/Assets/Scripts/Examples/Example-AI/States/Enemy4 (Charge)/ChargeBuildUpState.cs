//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChargeBuildUpState")]
public class ChargeBuildUpState : EnemyBaseState
{
    [Tooltip("Time in Seconds before the Enemy charges at the Player.")]
    [SerializeField] private float buildUpTime;
    private float currentCool;


    public override void Enter()
    {
        base.Enter();
        currentCool = buildUpTime;
        owner.animator.SetBool("isRunning", false);
        owner.animator.SetBool("isIdle", false);
        owner.animator.SetBool("isAttacking", false);
        owner.animator.SetBool("isBuilding", true);
        owner.animator.SetBool("isCharging", false);
        owner.animator.SetBool("isStunned", false);
    }

    public override void HandleUpdate()
    {
        BuildUpTime();
    }

    void BuildUpTime()
    {
        currentCool -= Time.deltaTime;

        if (currentCool > 0)
            return;

        currentCool = buildUpTime;
        Debug.Log("I am done building up");

        owner.Transition<ChargeAttackState>();
        
    }

}
