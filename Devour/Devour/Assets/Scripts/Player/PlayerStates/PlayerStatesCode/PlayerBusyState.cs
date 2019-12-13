//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBusyState")]
public class PlayerBusyState : PlayerBaseState {

    public override void Enter() {
        owner.PlayerLog("BusyState");
        owner.PlayerState = PlayerState.BUSY;
        owner.MovementIsStopped = true;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }

    public override void Exit() {
        owner.MovementIsStopped = false;
        owner.OutOfBusyStateJumpCancelTime = 0.1f;
        base.Exit();
    }
}
