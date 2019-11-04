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
        owner.Rb2D.gravityScale = 0;
        owner.Rb2D.freezeRotation = false;
        owner.Aim.GetComponent<SpriteRenderer>().enabled = true;
        //owner.Health -= owner.ProjectileHealthcost;
        //AudioPlaySoundEvent projectileSound = new AudioPlaySoundEvent {
        //    name = "Projectile",
        //    isRandomPitch = true,
        //    minPitch = 0.95f,
        //    maxPitch = 1,
        //    soundType = SoundType.SFX
        //};
        //projectileSound.FireEvent();
        
    }

    private void Shoot() {
        GameObject projectile;
        PlayerProjectile playerProjectile;
        projectile = Instantiate(playerProjectilePrefab, owner.Aim.position, Quaternion.identity);//owner.transform.position + new Vector3(owner.FacingDirection*2, 0, owner.transform.position.z), Quaternion.identity);
        playerProjectile = projectile.GetComponent<PlayerProjectile>();
        playerProjectile.Damage = owner.ProjectileDamage;
        playerProjectile.Direction = owner.Aim.right * owner.FacingDirection;//new Vector2(owner.FacingDirection, 0);
        playerProjectile.Player = owner;
        playerProjectile.Speed = projectileSpeed;
    }

    public override void HandleFixedUpdate() {
        if (Input.GetButton("Projectile")) {
            float horizontalInput = Input.GetAxis("Horizontal");
            if(owner.FacingDirection == -1 && Input.GetAxis("Horizontal") == 0) {
                horizontalInput *= -1;
            }
            owner.transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(Input.GetAxis("Vertical"), (horizontalInput * owner.FacingDirection)) * 180/ Mathf.PI)*owner.FacingDirection);
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

    //protected override void GetMovementInput() {
    //    owner.XInput = 0;
    //}

    protected override void MovePlayer() {

    }

    protected override void Jump(float extra) {

    }

    public override void Exit() {
        if(owner.transform.rotation.z < -90) {
            //Flip(-owner.XScale);
            Flip(owner.XScale *= -owner.XScale);
            owner.FacingDirection *= -owner.FacingDirection;
        } else {
        }
        owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        owner.Rb2D.gravityScale = 6;
        owner.Rb2D.freezeRotation = true;
        owner.UntilNextProjectileAttack = owner.ProjectileCooldown;
        owner.Aim.GetComponent<SpriteRenderer>().enabled = false;
    }

}
