using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/AttackState")]
public class Enemy6Attack : EnemyMovement
{
    [SerializeField] private float attackTime = 1f;

    private float attackTimeCooldown;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.ATTACK;
        owner.GetComponent<Enemy6>().Weapon.SetActive(true);
        attackTimeCooldown = attackTime;
    }
    public override void HandleUpdate()
    {
        AttackTimer();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void AttackTimer()
    {
        attackTimeCooldown -= Time.deltaTime;

        if(attackTimeCooldown > 0)
        {
            return;
        }

        owner.GetComponent<Enemy6>().Weapon.SetActive(false);
        owner.Transition<Enemy6Idle>();
    }

    public override void Exit()
    {
        owner.GetComponent<Enemy6>().Weapon.SetActive(false);
        base.Exit();
        
    }
}
