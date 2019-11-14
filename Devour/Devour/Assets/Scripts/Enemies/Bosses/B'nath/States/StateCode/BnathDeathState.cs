using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/B'nathDeathState")]

public class BnathDeathState : EnemyDeathState
{

    public override void Enter()
    {
        currentCooldown = deathTimer;
        owner.rb.gravityScale = 0;
        if (owner.GetComponent<BoxCollider2D>() != null)
        {
            owner.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (owner.GetComponent<CircleCollider2D>() != null)
        {
            owner.GetComponent<CircleCollider2D>().enabled = false;
        }
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
