//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ProjectileAttackState")]
public class ProjectileAttackState : EnemyBaseState
{
    // Attributes
    [Tooltip("Distance at which the Enemy stops trying to attack and starts chasing the Player.")]
    [SerializeField] private float chaseDistance;
    [SerializeField] private Vector3 projectileOffset;

    private ProjectileWeapon enemyWeapon;
    private float cooldown;
    private float currentCool;

    // Methods
    public override void Enter()
    {
        base.Enter();
        enemyWeapon = WeaponController.Instance.GetEnemyProjectileWeapon();
        //Animation
        
        cooldown = enemyWeapon.GetFireRate();
        currentCool = cooldown;
    }

    public override void HandleUpdate()
    { 
        Attack();

        if (DistanceToPlayer() > chaseDistance || CanSeePlayer() == false)
        {
            owner.Transition<ProjectileChaseState>();
        }
    }

    private void Attack()
    {
        currentCool -= Time.deltaTime;

        if (currentCool > 0)
        {
            return;
        }

        if (CanSeePlayer() == true && IsGrounded())
        {
            owner.transform.LookAt(owner.player.transform, Vector3.up);
            owner.animator.SetBool("isIdle", false);
            owner.animator.SetBool("isRunning", false);
            owner.animator.SetBool("isAttacking", true);
        }

        currentCool = cooldown;
    }

    public void ProjectileAttack()
    {
        GameObject enemyProj = Instantiate(WeaponController.Instance.EnemyWeaponProjectileGO, owner.transform.position + owner.transform.forward * 2 + projectileOffset, Quaternion.identity);
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileSpeed(enemyWeapon.GetProjectileSpeed());
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileTravelDistance(enemyWeapon.GetRange());
        enemyProj.GetComponent<EnemyProjectile>().SetProjectileDamage(enemyWeapon.GetDamage());
    }
}
