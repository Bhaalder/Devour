//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicDashExitState")]
public class Boss2SonicDashExit : Boss2BaseState
{
    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_DASH_EXIT;
        owner.LastAttack = Boss2Attacks.DASH;
        TurnedRight();
        owner.HitBoxHorizontal.SetActive(true);
        owner.HitBoxVertical.SetActive(false);
        owner.HitBoxHorizontal.GetComponent<BoxCollider2D>().isTrigger = false;
        owner.HitBoxVertical.GetComponent<BoxCollider2D>().isTrigger = false;
        owner.BoxCollider2D = owner.HitBoxVertical.GetComponent<BoxCollider2D>();
        owner.Transition<Boss2Idle>();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
