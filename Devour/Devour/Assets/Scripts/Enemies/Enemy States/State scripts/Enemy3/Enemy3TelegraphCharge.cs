using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3TelegraphCharge")]
public class Enemy3TelegraphCharge : Enemy3Movement
{

    [SerializeField] private float telegraphTime;
    private float currentTCooldown;
    private Color chargeIndicatorColor;

    public override void Enter()
    {
        base.Enter();
        currentTCooldown = telegraphTime;
        owner.rb.velocity = new Vector2(0f, 0f);
        owner.GetComponent<Enemy3>().State = Enemy3State.IDLE;
        TurnedRight();
        chargeIndicatorColor = owner.GetComponent<Enemy3>().ChargeIndicator.color;

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
            if (owner.GetComponent<Enemy3>().ChargeIndicator.color == chargeIndicatorColor)
            {
                owner.GetComponent<Enemy3>().ChargeIndicator.color = new Color(0, 0, 0);
            }
            else
            {
                owner.GetComponent<Enemy3>().ChargeIndicator.color = chargeIndicatorColor;
            }
            return;
        }

        currentTCooldown = telegraphTime;
        owner.GetComponent<Enemy3>().ChargeIndicator.color = chargeIndicatorColor;
        owner.Transition<Enemy3ChargeAttack>();

    }
}
