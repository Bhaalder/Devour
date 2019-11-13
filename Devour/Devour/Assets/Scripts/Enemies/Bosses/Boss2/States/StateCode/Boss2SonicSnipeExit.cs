using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicSnipeExitState")]
public class Boss2SonicSnipeExit : Boss2BaseState
{
    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_SNIPE_EXIT;
        owner.Transition<Boss2Idle>();
        owner.SonicSnipeBeam.GetComponentInChildren<SpriteRenderer>().enabled = false;
        owner.SonicSnipeBeam.GetComponentInChildren<BoxCollider2D>().enabled = false;
        owner.rb.gravityScale = 6;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        owner.rb.velocity = new Vector2(0, 0);
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
