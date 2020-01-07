//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/AttackState")]
public class Enemy6Attack : EnemyMovement
{
    [SerializeField] private float attackTime = 1f;

    private float attackTimeCooldown;
    private SpriteRenderer weaponSprite;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.ATTACK;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<BoxCollider2D>().enabled = true;
        weaponSprite = owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>();
        weaponSprite.enabled = true;
        weaponSprite.color = new Color(255, 255, 0);

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

        owner.GetComponent<Enemy6>().Weapon.GetComponent<BoxCollider2D>().enabled = false;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>().enabled = false;
        owner.Transition<Enemy6Idle>();
        weaponSprite.color = new Color(255, 255, 255);
    }

    public override void Exit()
    {
        owner.GetComponent<Enemy6>().Weapon.GetComponent<BoxCollider2D>().enabled = false;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>().enabled = false;
        weaponSprite.color = new Color(255, 255, 255);
        base.Exit();
        
    }
}
