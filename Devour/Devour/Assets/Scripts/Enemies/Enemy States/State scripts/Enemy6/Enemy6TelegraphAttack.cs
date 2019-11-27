using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy6/TelegraphAttackState")]
public class Enemy6TelegraphAttack : EnemyMovement
{
    [SerializeField] private float telegraphTime = 2f;
    private float telegraphCurrentCooldown;

    private SpriteRenderer weaponSprite;

    public override void Enter()
    {
        base.Enter();
        owner.GetComponent<Enemy6>().State = Enemy6State.TELEGRAPH_ATTACK;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<BoxCollider2D>().enabled = false;
        owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>().enabled = true;
        weaponSprite = owner.GetComponent<Enemy6>().Weapon.GetComponent<SpriteRenderer>();
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
            if(weaponSprite.color == new Color(255, 255, 255))
            {
                weaponSprite.color = new Color(0, 0, 0);
            }
            else
            {
                weaponSprite.color = new Color(255,255,255);
            }
            return;
        }

        telegraphCurrentCooldown = telegraphTime;
        owner.Transition<Enemy6Attack>();
    }
}
