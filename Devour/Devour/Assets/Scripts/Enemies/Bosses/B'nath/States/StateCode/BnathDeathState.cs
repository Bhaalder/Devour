using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/B'nathDeathState")]

public class BnathDeathState : EnemyDeathState
{

    public override void Enter()
    {
        owner.GetComponent<Bnath>().State = BossBnathState.DEATH;
        currentCooldown = deathTimer;
        owner.BoxCollider2D.enabled = false;
        owner.rb.gravityScale = 0;
        owner.rb.velocity = new Vector2(0, 0);
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
