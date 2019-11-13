using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3TelegraphCharge")]
public class Enemy3TelegraphCharge : Enemy3Movement
{

    [SerializeField] private float telegraphTime;
    private float currentTCooldown;

    public override void Enter()
    {
        base.Enter();
        currentTCooldown = telegraphTime;
        owner.rb.velocity = new Vector2(0f, 0f);
        owner.GetComponent<Enemy3>().State = Enemy3State.CHARGE_TELEGRAPH;
    }
    public override void HandleUpdate()
    {
        TelegraphTime();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    void TelegraphTime()
    {
        currentTCooldown -= Time.deltaTime;

        if (currentTCooldown > 0)
        {
            return;
        }

        currentTCooldown = telegraphTime;
        owner.Transition<Enemy3ChargeAttack>();

    }
}
