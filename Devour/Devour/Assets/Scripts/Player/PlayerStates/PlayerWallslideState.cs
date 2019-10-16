﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallslideState")]
public class PlayerWallslideState : PlayerBaseState {

    [Tooltip("The speed of the player sliding down on walls")]
    [SerializeField] private float wallSlideSpeed;
    [Tooltip("The force of the jump horizontally(x) and vertically(y)")]
    [SerializeField] private Vector2 wallJumpForce;

    public override void Enter() {
        owner.PlayerLog("WallslideState");
        owner.PlayerState = PlayerState.WALLSLIDE;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    protected override void MovePlayer() {
        //base.MovePlayer();
        if (!owner.IsWallSliding || !owner.IsTouchingWall) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, owner.Rb2D.velocity.y);
        }
        if (owner.Rb2D.velocity.y < -wallSlideSpeed && owner.IsWallSliding) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, -wallSlideSpeed);
        } else {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, owner.Rb2D.velocity.y);
        }
        if (!owner.IsWallSliding && Input.GetButtonUp("Jump") && !owner.IsGrounded) {
            owner.Transition<PlayerAirState>();
        } else if (!owner.IsWallSliding && Input.GetButtonUp("Jump") && owner.IsGrounded) {
            owner.Transition<PlayerIdleState>();
        }
    }

    public override void HandleUpdate() {
        if (!owner.IsWallSliding && owner.IsGrounded) {
            owner.Transition<PlayerIdleState>();
            return;
        }
        if (!owner.IsWallSliding && !Input.GetButton("Jump")) {
            owner.Transition<PlayerAirState>();
        }
        base.HandleUpdate();
    }

    protected override void Jump(float extra) {





        Vector2 impulse = new Vector2((wallJumpForce.x * -owner.FacingDirection), 0);
        owner.Rb2D.AddForce(impulse, ForceMode2D.Impulse);
        owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, wallJumpForce.y);
        if (owner.XInput != owner.FacingDirection) {
            Flip(owner.XScale * -owner.FacingDirection);
            owner.FacingDirection *= -1;
        }

        owner.Transition<PlayerWallJumpState>();
    }

    public override void Exit() {
        
    }

}
