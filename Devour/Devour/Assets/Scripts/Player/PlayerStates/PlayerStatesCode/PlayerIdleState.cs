//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerIdleState")]
public class PlayerIdleState : PlayerBaseState {

    public override void Enter() {
        //owner.PlayerLog("IdleState");
        owner.PlayerState = PlayerState.IDLE;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(Input.GetAxisRaw("Horizontal") != 0 && owner.IsGrounded) {
            owner.Transition<PlayerWalkState>();
        }
        if (!owner.IsGrounded) {
            owner.Transition<PlayerAirState>();
        }
        base.HandleUpdate();
    }
}
