//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBaseState")]
public class PlayerBaseState : State {

    protected Player owner;

    protected Vector2 impulse;

    public override void Enter() {
        //owner.PlayerLog("Initialized Playerstates!");
        owner.Transition<PlayerIdleState>();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        MovePlayer();
        
        FacingDirection();
        

        base.HandleFixedUpdate();
    }

    private void FacingDirection() {
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
        CooldownTimers();
        CollisionCheck();
        if (owner.PlayerHasAbility(PlayerAbility.DASH)) {
            DashCheck();
        }
        GetInput();
        JumpCheck();
        
        base.HandleUpdate();
    }

    public void CollisionCheck() {
        owner.IsGrounded = Physics2D.OverlapCircle(owner.GroundCheck.position, owner.GroundCheckDistance, owner.WhatIsGround);
        owner.IsTouchingWall = Physics2D.Raycast(owner.WallCheck.position, owner.transform.right, (owner.WallCheckDistance * owner.FacingDirection), owner.WhatIsGround);
        if (owner.PlayerHasAbility(PlayerAbility.WALLSLIDE)) {
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

    private void CooldownTimers() {
        owner.UntilNextDash -= Time.deltaTime;
        if(owner.UntilNextDash <= 0) {
            owner.UntilNextDash = 0;
        }
    }

    protected virtual void GetInput() {
        owner.XInput = Input.GetAxisRaw("Horizontal");
    }

    private void DashCheck() {
        if (Input.GetButtonDown("Dash") && owner.UntilNextDash <= 0) {
            owner.Transition<PlayerDashState>();
        }
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
            if (owner.PlayerState != PlayerState.AIR) {
                owner.Transition<PlayerAirState>();
            }
        }
        if (owner.PlayerHasAbility(PlayerAbility.DOUBLEJUMP)) {
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
        if (owner.PlayerState != PlayerState.AIR) {
            owner.Transition<PlayerAirState>();
        }
        if (!owner.IsWallSliding) {
            owner.Rb2D.velocity = Vector2.up * (owner.JumpForce + extra);
        }      
    }



    public override void Initialize(StateMachine owner) {
        this.owner = (Player)owner;
    }
}
