using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2IntroState")]
public class Boss2Intro : Boss2BaseState
{
    private bool isPlayingIntro;

    public override void Enter()
    {
        base.Enter();
        isPlayingIntro = true;
        owner.State = Boss2State.INTRO;
    }

    public override void HandleUpdate()
    {
        if (isPlayingIntro)
        {
            Intro();
        }
        else if (!isPlayingIntro)
        {
            owner.Transition<Boss2Idle>();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Intro()
    {
        if (owner.IntroStarted)
        {
            isPlayingIntro = false;
        }
    }
}
