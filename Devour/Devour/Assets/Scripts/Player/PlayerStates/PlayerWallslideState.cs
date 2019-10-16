using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallslideState")]
public class PlayerWallslideState : PlayerBaseState {

    [Tooltip("The speed of the player sliding down on walls")]
    [SerializeField] private float wallSlideSpeed;

    [SerializeField] private Vector2 wallJumpForce;

    public override void Enter() {
        owner.PlayerLog("WallslideState");
        owner.PlayerState = PlayerState.WALLSLIDE;
        owner.WallJumpForce = wallJumpForce;

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
        Vector2 impulse = new Vector2((owner.WallJumpForce.x * -owner.FacingDirection), owner.WallJumpForce.y);
        owner.Rb2D.AddForce(impulse, ForceMode2D.Impulse);
        Flip(owner.XScale * -owner.FacingDirection);
        owner.FacingDirection *= -1;
    }

    public override void Exit() {
        
    }

}
