//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/B'nathDeathState")]

public class BnathDeathState : EnemyDeathState
{

    public override void Enter()
    {
        currentCooldown = deathTimer;
        owner.GetComponent<Bnath>().State = BossBnathState.DEATH;
        currentCooldown = deathTimer;
        owner.BoxCollider2D.enabled = false;
        owner.rb.gravityScale = 0;
        owner.rb.velocity = new Vector2(0, 0);
        owner.GetComponent<Bnath>().FadeBossMusic();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        AbilityEssenceTimer();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void AbilityEssenceTimer()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        owner.GetComponent<Bnath>().SpawnAbilityEssence();
    }
}
