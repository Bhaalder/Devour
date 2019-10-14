//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileEnemy : Enemy
{

    [SerializeField] private Vector3 projectileOffset;
    [SerializeField] private float enemyHealth;
    private ProjectileWeapon enemyWeapon;


    protected override void Awake()
    {
        enemyWeapon = WeaponController.Instance.GetEnemyProjectileWeapon();
        health = enemyHealth;
        base.Awake();
    }
    public override void TakeDamage(float damage)
    {

        if (damage - damageResistance < 0)
        {

        }
        else
        {
            health -= (damage - damageResistance);
        }
        if (health <= 0)
        {
            Death();
        }
        if (isDamaged == false)
        {
            Transition<ProjectileChaseState>();
            isDamaged = true;
        }
    }
    public void ProjectileAttack()
    {
        GameObject enemyProj = Instantiate(WeaponController.Instance.EnemyWeaponProjectileGO, transform.position + transform.forward * 2 + projectileOffset, Quaternion.identity);
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileSpeed(enemyWeapon.GetProjectileSpeed());
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileTravelDistance(enemyWeapon.GetRange());
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileDamage(enemyWeapon.GetDamage());
    }
}
