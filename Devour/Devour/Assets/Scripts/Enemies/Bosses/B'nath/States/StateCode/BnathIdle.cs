using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/IdleState")]

public class BnathIdle : BnathBaseState
{

    [SerializeField] [Range(1, 100)] private float bodySlamPercentage;
    [SerializeField] private float pauseBetweenAttacks = 2f;

    private bool isPaused;

    private float currentCooldown;

    public override void Enter()
    {
        owner.State = BossBnathState.IDLE;
        base.Enter();
        currentCooldown = pauseBetweenAttacks;
        isPaused = true;
        //owner.GetComponent<Bnath>().Blocker.SetActive(true);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        if (isPaused)
        {
            PauseBetweenAttacks();
        }
        else
        {
            ChooseAttack();
        }
    }

    private void ChooseAttack()
    {
        int chooseSide = (int)Random.Range(1, 100);

        if(chooseSide <= bodySlamPercentage)
        {
            owner.Transition<BnathBodySlam>();
        }
        else
        {
            owner.Transition<BnathClimbDash>();
        }

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
