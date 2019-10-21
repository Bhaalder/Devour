//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallJumpState")]
public class PlayerWallJumpState : PlayerWallslideState {

    [Tooltip("How long time the jump lasts in horizontal")]
    [SerializeField] private float startJumpTime;
    private float jumpTime;

    public override void Enter() {
        //owner.PlayerLog("WallJumpState");
        owner.PlayerState = PlayerState.WALLJUMP;
        jumpTime = startJumpTime;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (Input.GetAxisRaw("Vertical") < 0) {
            owner.IsAttackingDown = true;
        } else {
            owner.IsAttackingDown = false;
        }

        if (jumpTime <= 0) {
            owner.Rb2D.velocity = new Vector2(0, owner.Rb2D.velocity.y);
            if (owner.IsGrounded) {
                owner.Transition<PlayerIdleState>();
            } else {
                owner.Transition<PlayerAirState>();
            }
        }
        jumpTime -= Time.deltaTime;

        base.HandleUpdate();
    }
}
