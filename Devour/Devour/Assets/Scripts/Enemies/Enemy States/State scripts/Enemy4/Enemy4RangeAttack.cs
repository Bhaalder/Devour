using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy4RangeAttackState")]
public class Enemy4RangeAttack : EnemyBaseState
{

    [SerializeField] private float abortAttack;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float count = 1f;
    [SerializeField] private float middlePointCurve = 5f;
    [SerializeField] private Vector2 projectileOffset;
    [SerializeField] private GameObject enemy4Projectile;

    private Transform target;
 
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 middlePoint;

    private float currentAttackCooldown;

    private bool canAttack = false;


    public override void Enter()
    {
        base.Enter();
        target = FindObjectOfType<Player>().transform;

    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (Vector2.Distance(owner.rb.position, target.position) >= abortAttack)
        {
            if (owner.GetComponent<Enemy4>().IsIdle)
            {
                owner.Transition<Enemy4Idle>();
            }
            else
            {
                owner.Transition<Enemy4Movement>();
            }
        }

        if (canAttack)
        {
            owner.rb.velocity = new Vector2(0f, 0f);
            SetTarget();
            Attack();
            canAttack = false;
        }

        AttackCooldown();
        TurnToPlayer();

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void Attack()
    {
        GameObject enemy4Proj = Instantiate(enemy4Projectile, owner.rb.position + projectileOffset, Quaternion.identity);
        enemy4Proj.GetComponent<Enemy4Projectile>().StartPoint = startPoint;
        enemy4Proj.GetComponent<Enemy4Projectile>().MiddlePoint = middlePoint;
        enemy4Proj.GetComponent<Enemy4Projectile>().EndPoint = endPoint;
        enemy4Proj.GetComponent<Enemy4Projectile>().Count = count;
    }

    private void SetTarget()
    {
        startPoint = owner.rb.position;
        endPoint = (Vector2)target.position;
        middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector2.up * middlePointCurve;
    }

    private void AttackCooldown()
    {
        currentAttackCooldown -= Time.deltaTime;

        if (currentAttackCooldown > 0)
        {
            return;
        }

        currentAttackCooldown = attackCooldown;
        canAttack = true;
    }

    private void TurnToPlayer()
    {
        if (target.position.x < owner.rb.position.x)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
    }

}
