using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2IntroState")]
public class Boss2Intro : Boss2BaseState
{
    [SerializeField] private float introTime;

    private float currentIntroCooldown = 5f;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.INTRO;
        currentIntroCooldown = introTime;
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
        owner.Transition<Boss2Idle>();
    }

    public override void Exit() {
        owner.BossIntroEnd();
        base.Exit();
    }
}
