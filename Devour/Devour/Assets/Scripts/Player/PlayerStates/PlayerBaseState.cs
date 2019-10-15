using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBaseState")]
public class PlayerBaseState : State {

    protected Player owner;

    public override void Enter() {
        owner.PlayerLog("Initialized Playerstates!");
        owner.WallCheckDistanceValue = owner.WallCheckDistance;
        owner.Transition<PlayerIdleState>();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        FacingDirection();
        MovePlayer();
        base.HandleFixedUpdate();
    }

    private void FacingDirection() {
        if (owner.XInput < 0) {
            Flip(-owner.XScale);
            owner.WallCheckDistanceValue = -owner.WallCheckDistance;
        } else if (owner.XInput > 0) {
            Flip(owner.XScale);
            owner.WallCheckDistanceValue = owner.WallCheckDistance;
        }
    }

    private void Flip(float direction) {
        Vector3 myScale = owner.transform.localScale;
        myScale.x = direction;
        owner.transform.localScale = myScale;
    }

    protected virtual void MovePlayer() {
        owner.Rb2D.velocity = new Vector2(owner.XInput * owner.MovementSpeed, owner.Rb2D.velocity.y);

    }

    public override void HandleUpdate() {
        if (owner.IsWallSliding && owner.PlayerState != PlayerState.WALLSLIDE) {
            owner.Transition<PlayerWallslideState>();
        }
        CollisionCheck();
        GetInput();
        JumpCheck();
        base.HandleUpdate();
    }

    private void CollisionCheck() {
        owner.IsGrounded = Physics2D.OverlapCircle(owner.GroundCheck.position, owner.GroundCheckDistance, owner.WhatIsGround);
        owner.IsTouchingWall = Physics2D.Raycast(owner.WallCheck.position, owner.transform.right, owner.WallCheckDistanceValue, owner.WhatIsGround);
        WallSlideCheck();
    }

    private void WallSlideCheck() {
        if (owner.IsTouchingWall && !owner.IsGrounded && owner.Rb2D.velocity.y < 0) {
            owner.IsWallSliding = true;
        } else {
            owner.IsWallSliding = false;
        }
    }

    private void GetInput() {
        owner.XInput = Input.GetAxisRaw("Horizontal");
    }

    private void JumpCheck() {
        if (owner.IsGrounded || owner.IsWallSliding) {
            owner.ExtraJumpingLeft = owner.ExtraJumps;
        }
        if (owner.IsWallSliding && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (owner.IsGrounded && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (!owner.IsGrounded && owner.ExtraJumpingLeft > 0 && Input.GetButtonDown("Jump")) {
            owner.ExtraJumpingLeft--;
            owner.Rb2D.velocity = new Vector2(0, 0);
            Jump(5);
        }
        if (Input.GetButtonUp("Jump") && owner.Rb2D.velocity.y > 0) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, owner.Rb2D.velocity.y * owner.VariableJumpHeight);
        }
    }

    private void Jump(float extra) {
        owner.Rb2D.velocity = Vector2.up * (owner.JumpForce + extra);
    }



    public override void Initialize(StateMachine owner) {
        this.owner = (Player)owner;
    }
}
