//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWalkState")]
public class PlayerWalkState : PlayerBaseState {

    public override void Enter() {
        //owner.PlayerLog("WalkState");
        owner.PlayerState = PlayerState.WALK;
        owner.Animator.SetBool("IsLanding", false);
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (owner.XInput == 0 && owner.IsGrounded) {
            owner.Transition<PlayerIdleState>();
        }
        if (!owner.IsGrounded) {
            owner.Transition<PlayerJumpState>();
        }
    }
}
