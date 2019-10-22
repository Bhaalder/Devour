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
        //start telegraph visuals
    }
    public override void HandleUpdate()
    {
        //base.HandleUpdate();
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
        Debug.Log(owner.ToString() + "Beginning my charge!");
        owner.Transition<Enemy3ChargeAttack>();

    }
}
