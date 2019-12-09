using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/BnathIntroState")]


public class BnathIntro : BnathBaseState
{
    [SerializeField] private float introTime = 5f;

    private float currentIntroCooldown;
    public override void Enter()
    {
        base.Enter();
        owner.State = BossBnathState.INTRO;
        if (!GameController.Instance.BossIntroPlayed.Contains(owner.BossName))
        {
            currentIntroCooldown = introTime;
        }
        else
        {
            currentIntroCooldown = 1;
        }
        owner.BossIntroSequence();
    }

    public override void HandleUpdate()
    {
        Intro();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Intro()
    {
        currentIntroCooldown -= Time.deltaTime;

        if (currentIntroCooldown > 0)
        {
            return;
        }

        currentIntroCooldown = introTime;
        owner.Transition<BnathIdle>();
    }

    public override void Exit() {
        owner.BossIntroEnd();
        base.Exit();
    }

}
