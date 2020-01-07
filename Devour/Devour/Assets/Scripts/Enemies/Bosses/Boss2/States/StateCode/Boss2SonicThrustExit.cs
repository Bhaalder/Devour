//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicThrustExitState")]
public class Boss2SonicThrustExit : Boss2BaseState
{
    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_THRUST_EXIT;
        owner.LastAttack = Boss2Attacks.THRUST;
        owner.rb.velocity = new Vector2(0, 0);
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
