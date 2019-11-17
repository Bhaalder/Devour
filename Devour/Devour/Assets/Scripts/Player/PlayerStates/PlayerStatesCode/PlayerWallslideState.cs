//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallslideState")]
public class PlayerWallslideState : PlayerBaseState {

    [Tooltip("The speed of the player sliding down on walls")]
    [SerializeField] private float wallSlideSpeed;
    [Tooltip("The force of the jump horizontally(x) and vertically(y)")]
    [SerializeField] private Vector2 wallJumpForce;
    [Tooltip("The time before letting go when holding directional button towards the opposite side")]
    [SerializeField] private float gripTime;
    private float gripTimeLeft;

    public override void Enter() {
        //owner.PlayerLog("WallslideState");
        owner.PlayerState = PlayerState.WALLSLIDE;
        AudioPlaySoundEvent wallSlidePlay = new AudioPlaySoundEvent {
            name = "Wallslide",
            soundType = SoundType.SFX
        };
        wallSlidePlay.FireEvent();
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

    protected override void GetMovementInput() {
        owner.XInput = Input.GetAxis("Horizontal");
        if (WalljumpInputLeft()) {
            gripTimeLeft -= Time.deltaTime;
            Debug.Log("LEFT");
        } else if (WalljumpInputRight()) {
            gripTimeLeft -= Time.deltaTime;
            Debug.Log("RIGHT");
        } else {
            gripTimeLeft = gripTime;
        }
        if(gripTimeLeft <= 0) {
            owner.XInput = Input.GetAxis("Horizontal");
        }
        Debug.Log(owner.XInput);
    }

    private bool WalljumpInputLeft() {
        if (owner.XInput < 0f && owner.FacingDirection == 1) {
            owner.XInput = 0;
            return true;
        }
        return false;
    }
    private bool WalljumpInputRight() {
        if (owner.XInput > 0f && owner.FacingDirection == -1) {
            owner.XInput = 0;
            return true;
        }
        return false;
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
        owner.IsAttackingDown = false;
        AudioStopSoundEvent wallSlideStop = new AudioStopSoundEvent {
            name = "Wallslide"
        };
        wallSlideStop.FireEvent();
        base.Exit();
    }

}
