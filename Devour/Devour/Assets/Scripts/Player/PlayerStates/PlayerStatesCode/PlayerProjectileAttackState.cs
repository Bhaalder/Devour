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
        PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
            damage = owner.ProjectileHealthcost,
            isSelfInflicted = true
        };
        playerTakeDamage.FireEvent();
        //owner.Health -= owner.ProjectileHealthcost;
        //AudioPlaySoundEvent projectileSound = new AudioPlaySoundEvent {
        //    name = "Projectile",
        //    isRandomPitch = true,
        //    minPitch = 0.95f,
        //    maxPitch = 1,
        //    soundType = SoundType.SFX
        //};
        //projectileSound.FireEvent();
        Shoot();
    }

    private void Shoot() {
        GameObject projectile;
        PlayerProjectile playerProjectile;
        projectile = Instantiate(playerProjectilePrefab, owner.transform.position + new Vector3(owner.FacingDirection*2, 0, 0), Quaternion.identity);
        playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.Damage = owner.ProjectileDamage;
        playerProjectile.Direction = new Vector2(owner.FacingDirection, 0);
        playerProjectile.Player = owner;
        playerProjectile.Speed = projectileSpeed;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        Attack();
        base.HandleUpdate();
    }

    protected void Attack() {
        owner.Rb2D.velocity = new Vector2(0, 0);
        if (attackTime <= 0) {
            if (owner.IsGrounded) {
                owner.Transition<PlayerIdleState>();
            } else {
                owner.Transition<PlayerAirState>();
            }
        }
        attackTime -= Time.deltaTime;
    }

    protected override void GetMovementInput() {
        owner.XInput = 0;
    }

    protected override void MovePlayer() {

    }

    protected override void Jump(float extra) {

    }

}
