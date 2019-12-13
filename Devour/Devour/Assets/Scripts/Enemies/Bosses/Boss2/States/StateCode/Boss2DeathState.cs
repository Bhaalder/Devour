using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2DeathState")]
public class Boss2DeathState : EnemyDeathState
{
    private float abilityEssenceTimerCooldown;

    public override void Enter()
    {
        currentCooldown = deathTimer;
        abilityEssenceTimerCooldown = deathTimer;
        owner.rb.gravityScale = 0;
        owner.GetComponent<Boss2>().HitBoxHorizontal.SetActive(false);
        owner.GetComponent<Boss2>().HitBoxVertical.SetActive(false);
        if (owner.GetComponent<BoxCollider2D>() != null)
        {
            owner.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (owner.GetComponent<CircleCollider2D>() != null)
        {
            owner.GetComponent<CircleCollider2D>().enabled = false;
        }

        for (int i = 0; i < owner.GetComponent<Boss2>().SonicDashParticles.Count; i++)
        {
            if (owner.GetComponent<Boss2>().SonicDashParticles[i] != null)
            {
                Destroy(owner.GetComponent<Boss2>().SonicDashParticles[i]);
            }
        }

        owner.GetComponent<Boss2>().FadeBossMusic();
        Debug.Log("Entered Death state");
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        AbilityEssenceTimer();
        owner.rb.velocity = Vector2.zero;
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

        owner.GetComponent<Boss2>().SpawnAbilityEssence();
    }
}