﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBaseState")]
public class PlayerBaseState : State {

    protected Player owner;

    protected Vector2 impulse;
    
    public override void Enter() {
        //owner.PlayerLog("Initialized Playerstates!");
        if(owner.PlayerState == PlayerState.NONE) {
            owner.Transition<PlayerIdleState>();
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        if (!owner.MovementIsStopped) {
            MovePlayer();
            FacingDirection();
            FallspeedCheck();
            base.HandleFixedUpdate();
        } else {
            owner.Rb2D.velocity = Vector2.zero;
        }
    }

    protected void FacingDirection() {
        if (owner.XInput < 0) {
            Flip(-owner.XScale);
            owner.FacingDirection = -1;
        } else if (owner.XInput > 0) {
            Flip(owner.XScale);
            owner.FacingDirection = 1;
        }
    }

    protected void Flip(float direction) {
        Vector3 myScale = owner.transform.localScale;
        myScale.x = direction;
        owner.transform.localScale = myScale;
    }

    protected virtual void MovePlayer() {
        owner.Rb2D.velocity = new Vector2(owner.XInput * owner.MovementSpeed, owner.Rb2D.velocity.y);
    }

    public override void HandleUpdate() {
        JumpCheck();
        VoidMendCheck();
        CooldownTimers();
        CollisionCheck();
        DashCheck();
        GetMovementInput();
        GetCombatInput();

        base.HandleUpdate();
    }

    private void JumpCheck() {
        if (owner.IsWallSliding && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (owner.IsGrounded && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (Input.GetButtonUp("Jump") && owner.Rb2D.velocity.y > 0) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, owner.Rb2D.velocity.y * owner.VariableJumpHeight);
        }
        if (owner.HasAbility(PlayerAbility.DOUBLEJUMP)) {
            if (owner.IsGrounded || owner.IsWallSliding) {
                owner.ExtraJumpsLeft = owner.ExtraJumps;
            }
            if (!owner.IsGrounded && owner.ExtraJumpsLeft > 0 && Input.GetButtonDown("Jump")) {
                owner.ExtraJumpsLeft--;
                owner.Rb2D.velocity = new Vector2(0, 0);
                Jump(0);
            }
        }
    }

    protected virtual void Jump(float extra) {
        if (owner.ExtraJumpsLeft > 0) {
            AudioPlaySoundEvent jumpSound = new AudioPlaySoundEvent {
                name = "Step2",
                soundType = SoundType.SFX,
                isRandomPitch = true,
                minPitch = 0.9f,
                maxPitch = 1f
            };
            jumpSound.FireEvent();
        }
        AudioPlaySoundEvent capeJumpSound = new AudioPlaySoundEvent {
            name = "Jump2",
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.93f,
            maxPitch = 1.03f
        };
        capeJumpSound.FireEvent();
        if (owner.PlayerState != PlayerState.JUMP) {
            owner.Transition<PlayerJumpState>();
        }
        if (!owner.IsWallSliding) {
            owner.Rb2D.velocity = Vector2.up * (owner.JumpForce + extra);
        }
    }

    public void VoidMendCheck() {
        if (Input.GetButtonDown("VoidMend") && owner.HasAbility(PlayerAbility.VOIDMEND)) {
            if(owner.PlayerVoid == owner.MaxPlayerVoid) {
                if(owner.Health != owner.MaxHealth) {
                    float voidUse = owner.MaxHealth - owner.Health;
                    if(voidUse > owner.MaxPlayerVoid) {
                        voidUse = owner.MaxPlayerVoid;
                    }
                    PlayerVoidEvent voidEvent = new PlayerVoidEvent {
                        amount = -voidUse
                    };
                    PlayerHealEvent healEvent = new PlayerHealEvent {
                        amount = voidUse
                    };
                    voidEvent.FireEvent();
                    healEvent.FireEvent();
                }
            }
            return;
        }
    }

    public void CollisionCheck() {
        RaycastHit2D left = Physics2D.Raycast(owner.GroundChecks[0].position, Vector2.down, owner.GroundCheckDistance, owner.WhatIsGround);
        RaycastHit2D right = Physics2D.Raycast(owner.GroundChecks[1].position, Vector2.down, owner.GroundCheckDistance, owner.WhatIsGround);
        if(left || right) {
            owner.IsGrounded = true;
        }
        if(!left && !right) {
            owner.IsGrounded = false;
        }
        //owner.IsGrounded = Physics2D.OverlapCircle(owner.GroundCheck.position, owner.GroundCheckDistance, owner.WhatIsGround);
        owner.IsTouchingWall = Physics2D.Raycast(owner.WallCheck.position, owner.transform.right * owner.FacingDirection, owner.WallCheckDistance, owner.WhatIsGround);
        if (owner.HasAbility(PlayerAbility.WALLCLIMB)) {
            WallSlideCheck();
        }       
    }

    public void WallSlideCheck() {
        if (owner.IsTouchingWall && !owner.IsGrounded && owner.Rb2D.velocity.y < 0) {
            owner.IsWallSliding = true;
        } else {
            owner.IsWallSliding = false;
        }
        if (owner.IsWallSliding && owner.PlayerState != PlayerState.WALLSLIDE && owner.Rb2D.velocity.y < 0) {
            owner.Transition<PlayerWallslideState>();
        }
    }

    protected void CooldownTimers() {
        owner.UntilNextDash -= Time.deltaTime;
        owner.UntilNextProjectileAttack -= Time.deltaTime;
        if (owner.UntilNextDash <= 0) {
            owner.UntilNextDash = 0;
        }
        if (owner.UntilNextProjectileAttack <= 0) {
            owner.UntilNextProjectileAttack = 0;
        }
    }

    protected virtual void GetMovementInput() {
        owner.XInput = Input.GetAxis("Horizontal");
        if(owner.XInput < -0.35f) {
            owner.XInput = -1;
        } else if(owner.XInput > 0.35f) {
            owner.XInput = 1;
        } else {
            owner.XInput = 0;
        }
    }

    protected void GetCombatInput() {
        GetMeleeInput();
        GetProjectileInput();      
    }

    protected void GetMeleeInput() {
        if (owner.UntilNextMeleeAttack > 0) {
            owner.UntilNextMeleeAttack -= Time.deltaTime;
            return;
        }
        if (!owner.IsWallSliding) {
            if (Input.GetButtonDown("Attack")) {
                BoxCollider2D attackCollider;
                attackCollider = owner.HorizontalMeleeCollider;
                owner.IsAttackingUp = false;
                owner.Animator.SetBool("IsAttackingUp", false);
                if (Input.GetAxisRaw("Vertical") > 0.35f) {
                    attackCollider = owner.UpMeleeCollider;
                    owner.IsAttackingUp = true;
                    owner.Animator.SetBool("IsAttackingUp", true);
                }
                if (Input.GetAxisRaw("Vertical") < -0.35f && !owner.IsGrounded) {
                    attackCollider = owner.DownMeleeCollider;
                    owner.IsAttackingDown = true;
                    owner.Animator.SetBool("IsAttackingDown", true);
                } else {
                    owner.IsAttackingDown = false;
                    owner.Animator.SetBool("IsAttackingDown", false);
                }
                PlayerAttackEvent playerAttack = new PlayerAttackEvent {
                    attackCollider = attackCollider,
                    damage = owner.MeleeDamage,
                    playerPosition = owner.transform.position,
                    player = owner.GetComponent<Player>(),
                    isMeleeAttack = true
                };
                playerAttack.FireEvent();
                owner.Transition<PlayerAttackState>();
            }
        }        
    }

    private void FallspeedCheck() {
        if(owner.Rb2D.velocity.y <= owner.FallSpeed) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, owner.FallSpeed);
        }
    }

    protected void GetProjectileInput() {
        if (owner.HasAbility(PlayerAbility.PROJECTILE)) {
            if (Input.GetButtonDown("Projectile") && owner.UntilNextProjectileAttack <= 0) {
                if(owner.Health <= owner.ProjectileHealthcost) {
                    Debug.Log("Too low HP!");
                    return;
                }
                owner.Transition<PlayerProjectileAttackState>();
            }
        }
    }

    private void DashCheck() {
        if(owner.IsWallSliding || owner.IsGrounded) {
            owner.DashesLeft = owner.NumberOfDashes;
        }
        if (owner.HasAbility(PlayerAbility.DASH)) {
            if(Input.GetAxis("Dash") > 0 && owner.DashesLeft > 0 && owner.UntilNextDash <= 0) {
                owner.DashesLeft--;
                owner.Transition<PlayerDashState>();
            }
            if (Input.GetButtonDown("Dash") && owner.DashesLeft > 0 && owner.UntilNextDash <= 0) {
                owner.DashesLeft--;
                owner.Transition<PlayerDashState>();
            }
        }
    }

    

    public override void Initialize(StateMachine owner) {
        this.owner = (Player)owner;
    }
}
