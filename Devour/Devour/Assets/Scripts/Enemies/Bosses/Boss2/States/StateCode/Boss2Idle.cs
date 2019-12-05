using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2IdleState")]
public class Boss2Idle : Boss2BaseState
{

    [SerializeField] private float timeBetweenAttacks = 1f;
    [Tooltip("Percentage chance of Sonic Thrust being the chosen Attack")]
    [SerializeField] [Range(0, 100)] private float sonicThrustPercentage;
    [Tooltip("Percentage of remaining, ie. 50% of remaining 40% = 20% total")]
    [SerializeField] [Range(0, 100)] private float sonicSnipePercentage;

    private float currentCooldown;

    public override void Enter()
    {
        base.Enter();
        currentCooldown = timeBetweenAttacks;
        owner.State = Boss2State.IDLE;
        owner.HitBoxHorizontal.SetActive(true);
        owner.HitBoxVertical.SetActive(false);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        TimeBetweenAttacks();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void TimeBetweenAttacks()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = timeBetweenAttacks;
        ChooseAttack();
    }

    private void ChooseAttack()
    {
        int chooseAttack = Random.Range(1, 100);

        if (chooseAttack <= sonicThrustPercentage)
        {
            owner.Transition<Boss2SonicThrustMovement>();
        }
        else
        {
            chooseAttack = Random.Range(1, 100);
            if (chooseAttack <= sonicSnipePercentage)
            {
                owner.Transition<Boss2SonicSnipeMovement>();
            }
            else
            {
                owner.Transition<Boss2SonicDashTelegraph>();
            }
        }
    }
}
