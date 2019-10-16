﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWalkState")]
public class PlayerWalkState : PlayerBaseState {

    public override void Enter() {
        owner.PlayerLog("WalkState");
        owner.PlayerState = PlayerState.WALK;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(!Input.GetButton("Horizontal") && owner.IsGrounded) {
            owner.Transition<PlayerIdleState>();
        }
        base.HandleUpdate();
    }
}
