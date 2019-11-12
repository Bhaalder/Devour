using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicSnipeAttackState")]
public class Boss2SonicSnipeAttack : Boss2BaseState
{
    [SerializeField] private float attackTime = 1f;

    private float currentCooldown;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_SNIPE_ATTACK;
        currentCooldown = attackTime;
        owner.SnipeBeamSprite.color = new Color(0, 0, 0);
        owner.SonicSnipeBeam.GetComponentInChildren<BoxCollider2D>().enabled = true;
    }

    public override void HandleUpdate()
    {
        AttackTime();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void AttackTime()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = attackTime;
        owner.Transition<Boss2SonicSnipeExit>();
    }
}
