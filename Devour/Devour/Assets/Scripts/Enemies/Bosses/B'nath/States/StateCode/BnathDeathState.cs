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
