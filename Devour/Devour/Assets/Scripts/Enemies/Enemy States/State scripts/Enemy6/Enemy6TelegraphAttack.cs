using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/TelegraphAttackState")]
public class Enemy6TelegraphAttack : EnemyMovement
{
    [SerializeField] private float telegraphTime = 2f;
    private float telegraphCurrentCooldown;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.TELEGRAPH_ATTACK;
        telegraphCurrentCooldown = telegraphTime;
        TurnedRight();
    }
    public override void HandleUpdate()
    {
        Telegraph();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Telegraph()
    {
        telegraphCurrentCooldown -= Time.deltaTime;

        if (telegraphCurrentCooldown > 0)
        {
            return;
        }

        telegraphCurrentCooldown = telegraphTime;
        owner.Transition<Enemy6Attack>();
    }
}
