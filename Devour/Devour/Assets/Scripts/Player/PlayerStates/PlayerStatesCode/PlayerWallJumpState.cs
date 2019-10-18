﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallJumpState")]
public class PlayerWallJumpState : PlayerWallslideState {

    public override void Enter() {
        //owner.PlayerLog("WallJumpState");
        owner.PlayerState = PlayerState.WALLJUMP;
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
        base.HandleUpdate();
    }
}
