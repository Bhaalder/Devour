//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerProjectileAttackState")]
public class PlayerProjectileAttackState : PlayerBaseState {

    [Tooltip("How long time the attack lasts")]
    [SerializeField] private float startAttackTime;
    [Tooltip("Projectile Gameobject")]
    [SerializeField] private GameObject playerProjectilePrefab;
    [Tooltip("How fast the projectile is")]
    [SerializeField] private float projectileSpeed;
    private float attackTime;


    public override void Enter() {
        //owner.PlayerLog("ProjectileAttackState");
        owner.PlayerState = PlayerState.PROJECTILEATTACK;
        attackTime = startAttackTime;
        owner.UntilNextProjectileAttack = owner.ProjectileCooldown;
        owner.Rb2D.gravityScale = 0;
        owner.Rb2D.freezeRotation = false;
        owner.Aim.GetComponent<SpriteRenderer>().enabled = true;        
    }

    private void Shoot() {
        GameObject projectile;
        PlayerProjectile playerProjectile;
        projectile = Instantiate(playerProjectilePrefab, owner.Aim.position, Quaternion.identity);
        playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.Damage = owner.ProjectileDamage;
        playerProjectile.ProjectileHealthcost = owner.ProjectileHealthcost;
        playerProjectile.Direction = owner.Aim.right * owner.FacingDirection;
        playerProjectile.Player = owner;
        playerProjectile.Speed = projectileSpeed;
    }

    public override void HandleFixedUpdate() {
        if (Input.GetButton("Projectile")) {

            float horizontalInput = Input.GetAxis("Horizontal");
            if (owner.FacingDirection == -1 && Input.GetAxis("Horizontal") == 0) {
                horizontalInput *= -1;
            }
            owner.transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(Input.GetAxis("Vertical"), (horizontalInput * owner.FacingDirection)) * 180 / Mathf.PI) * owner.FacingDirection);
        }
        //base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        GetMovementInput();
        CooldownTimers();
        Attack();
        //base.HandleUpdate();
    }

    protected void Attack() {
        owner.Rb2D.velocity = new Vector2(0, 0);
        if (Input.GetButtonUp("Projectile")) {
            Shoot();
            attackTime = 0;
        }
        if (attackTime <= 0) {
            if (owner.IsGrounded) {
                owner.Transition<PlayerIdleState>();
            } else {
                owner.Transition<PlayerAirState>();
            }
        }
        attackTime -= Time.deltaTime;
    }

    public override void Exit() {
        owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        owner.Rb2D.gravityScale = 6;
        owner.Rb2D.freezeRotation = true;
        owner.UntilNextProjectileAttack = owner.ProjectileCooldown;
        owner.Aim.GetComponent<SpriteRenderer>().enabled = false;
    }

}
